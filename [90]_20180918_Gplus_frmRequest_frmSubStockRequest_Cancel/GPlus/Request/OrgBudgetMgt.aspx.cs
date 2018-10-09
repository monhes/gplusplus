using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.Request
{
    public partial class OrgBudgetMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "505";

                BindingOrgData();

                //หากเป็น User Groupid = 4,7 ให้เลือกฝ่ายและทีมได้
                DataTable dtGroupUser = new DataAccess.UserDAO().GetUserGroupUser(UserID);

                DataRow[] rows = dtGroupUser.Select("UserGroup_ID IN (4,7)");
                if (rows.Length > 0)
                {
                    orgCtrl.setSelectBtnEnable();
                }
                else
                {
                    orgCtrl.setSelectBtnDisable();
                }
                
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindingOrgData()
        {
            DataTable dtt = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(this.OrgID));
            if (dtt != null)
            {
                if (dtt.Rows.Count > 0)
                {
                    //this.txtDeptName.Text = dtt.Rows[0]["Description"].ToString();
                    orgCtrl.OrgStructID = this.OrgID;
                    orgCtrl.DivName = dtt.Rows[0]["DivName"].ToString();
                    orgCtrl.DepName = dtt.Rows[0]["DepName"].ToString();
                    orgCtrl.DivCode = dtt.Rows[0]["Div_Code"].ToString();
                    orgCtrl.DepCode = dtt.Rows[0]["Dep_Code"].ToString();
                }
            }

        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvBudget_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Adding a column manually once the header created
            if (e.Row.RowType == DataControlRowType.Header) // If header created
            {
                GridView gvBudget = (GridView)sender;

                // Creating a Row
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //Adding รายละเอียด
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "รายละเอียด";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding ปีพุทธศักราช
                HeaderCell = new TableCell();
                HeaderCell.Text = "ปีพุทธศักราช";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding ฝ่าย
                HeaderCell = new TableCell();
                HeaderCell.Text = "ฝ่าย";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding ทีมงาน
                HeaderCell = new TableCell();
                HeaderCell.Text = "ทีมงาน";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding งบประมาณ
                HeaderCell = new TableCell();
                HeaderCell.Text = "งบประมาณ";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 2; // For merging two columns (เครื่องเขียน, แบบพิมพ์)
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding วันที่สร้าง แก้ไข เครื่องเขียน
                HeaderCell = new TableCell();
                HeaderCell.Text = "เครื่องเขียน";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 4; // For merging 4 columns
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding วันที่สร้าง แก้ไข แบบพิมพ์
                HeaderCell = new TableCell();
                HeaderCell.Text = "แบบพิมพ์";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 4; // For merging 4 columns
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding the Row at the 0th position (first row) in the Grid
                gvBudget.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            //pnlDetail.Visible = false;
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            tbYear.Text = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            tbYearDetail.Enabled = true;

            tbDivNameDetail.Text = orgCtrl.DivName;
            tbDepNameDetail.Text = orgCtrl.DepName;

            // เครื่องเขียน
            lblCrtBy_Cate8.Text = this.FirstName + " " + this.LastName;
            lblMdfBy_Cate8.Text = this.FirstName + " " + this.LastName;
            lblCrtDate_Cate8.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblMdfDate_Cate8.Text = DateTime.Now.ToString(this.DateTimeFormat);

            // แบบพิมพ์
            lblCrtBy_Cate9.Text = this.FirstName + " " + this.LastName;
            lblMdfBy_Cate9.Text = this.FirstName + " " + this.LastName;
            lblCrtDate_Cate9.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblMdfDate_Cate9.Text = DateTime.Now.ToString(this.DateTimeFormat);
        }

        protected void gvBudget_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Invisibling columns on row header (normally created on binding)
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; // Invisibiling รายละเอียด Header Cell
                e.Row.Cells[1].Visible = false; // Invisibiling ปีพุทธศักราช Header Cell
                e.Row.Cells[2].Visible = false; // Invisibiling ฝ่าย Header Cell
                e.Row.Cells[3].Visible = false; // Invisibiling ทีมงาน Header Cell
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Budget_Year"].ToString() + "&" + drv["OrgStruc_Id"].ToString();
                if (drv["CrtDate_Budget8"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["CrtDate_Budget8"]).ToString(this.DateTimeFormat);

                if (drv["MdfDate_Budget8"].ToString().Trim().Length > 0)
                    e.Row.Cells[8].Text = ((DateTime)drv["MdfDate_Budget8"]).ToString(this.DateTimeFormat);

                if (drv["CrtDate_Budget9"].ToString().Trim().Length > 0)
                    e.Row.Cells[10].Text = ((DateTime)drv["CrtDate_Budget9"]).ToString(this.DateTimeFormat);

                if (drv["MdfDate_Budget9"].ToString().Trim().Length > 0)
                    e.Row.Cells[12].Text = ((DateTime)drv["MdfDate_Budget9"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvBudget_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                string[] str_cmd;
                str_cmd = e.CommandArgument.ToString().Split('&');
                DataTable dt = new DataAccess.OrgStructureDAO().GetOrgBudgetByID(str_cmd[0],str_cmd[1]);
                BindDataToTableBudgetDetail(dt);

                tbYearDetail.Enabled = false;
                tbYearDetail.Text = dt.Rows[0]["Budget_Year"].ToString();
                tbDivNameDetail.Text = dt.Rows[0]["DivName"].ToString();
                tbDepNameDetail.Text = dt.Rows[0]["DepName"].ToString();

                if (dt.Rows.Count == 2) // แสดงว่ามีข้อมูลทั้ง เครื่องเขียน และ แบบพิมพ์
                {
                    // เครื่องเขียน
                    rblCate8_Status.Items[0].Selected = dt.Rows[0]["status"].ToString() == "1";
                    rblCate8_Status.Items[1].Selected = dt.Rows[0]["status"].ToString() == "0";

                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCrtDate_Cate8.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblCrtDate_Cate8.Text = "";
                    lblCrtBy_Cate8.Text = dt.Rows[0]["CrtBy"].ToString();

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblMdfDate_Cate8.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblMdfDate_Cate8.Text = "";
                    lblMdfBy_Cate8.Text = dt.Rows[0]["MdfBy"].ToString();

                    // แบบพิมพ์
                    rblCate9_Status.Items[0].Selected = dt.Rows[1]["status"].ToString() == "1";
                    rblCate9_Status.Items[1].Selected = dt.Rows[1]["status"].ToString() == "0";

                    if (dt.Rows[1]["Create_Date"].ToString().Length > 0)
                        lblCrtDate_Cate9.Text = ((DateTime)dt.Rows[1]["Create_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblCrtDate_Cate9.Text = "";
                    lblCrtBy_Cate9.Text = dt.Rows[1]["CrtBy"].ToString();

                    if (dt.Rows[1]["Update_Date"].ToString().Length > 0)
                        lblMdfDate_Cate9.Text = ((DateTime)dt.Rows[1]["Update_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblMdfDate_Cate9.Text = "";
                    lblMdfBy_Cate9.Text = dt.Rows[1]["MdfBy"].ToString();
                }
                else if (dt.Rows.Count == 1 && dt.Rows[0]["Cate_ID"].ToString() == "8") // มีแต่ข้อมูลเครื่องเขียน
                {
                    // เครื่องเขียน
                    rblCate8_Status.Items[0].Selected = dt.Rows[0]["status"].ToString() == "1";
                    rblCate8_Status.Items[1].Selected = dt.Rows[0]["status"].ToString() == "0";

                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCrtDate_Cate8.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblCrtDate_Cate8.Text = "";
                    lblCrtBy_Cate8.Text = dt.Rows[0]["CrtBy"].ToString();

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblMdfDate_Cate8.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblMdfDate_Cate8.Text = "";
                    lblMdfBy_Cate8.Text = dt.Rows[0]["MdfBy"].ToString();


                    // แบบพิมพ์
                    rblCate9_Status.Items[0].Selected = false;
                    rblCate9_Status.Items[1].Selected = false;
                    lblCrtDate_Cate9.Text = "";
                    lblCrtBy_Cate9.Text = "";
                    lblMdfDate_Cate9.Text = "";
                    lblMdfBy_Cate9.Text = "";
                }
                else if (dt.Rows.Count == 1 && dt.Rows[0]["Cate_ID"].ToString() == "9") // มีแต่ข้อมูลแบบพิมพ์
                {
                    // เครื่องเขียน
                    rblCate8_Status.Items[0].Selected = false;
                    rblCate8_Status.Items[1].Selected = false;
                    lblCrtDate_Cate8.Text = "";
                    lblCrtBy_Cate8.Text = "";
                    lblMdfDate_Cate8.Text = "";
                    lblMdfBy_Cate8.Text = "";

                    // แบบพิมพ์

                    rblCate9_Status.Items[0].Selected = dt.Rows[0]["status"].ToString() == "1";
                    rblCate9_Status.Items[1].Selected = dt.Rows[0]["status"].ToString() == "0";

                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCrtDate_Cate9.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblCrtDate_Cate9.Text = "";
                    lblCrtBy_Cate9.Text = dt.Rows[0]["CrtBy"].ToString();

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblMdfDate_Cate9.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                    else
                        lblMdfDate_Cate9.Text = "";
                    lblMdfBy_Cate9.Text = dt.Rows[0]["MdfBy"].ToString();
                }

                pnlDetail.Visible = true;
            }
        }



        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (tbYearDetail.Text == "")
            {
                ShowMessageBox("กรุณาระบุปีพุทธศักราช");
                return;
            }

            DataTable dtSave = SaveData();
            if (dtSave.Rows[0]["Result"].ToString() == "1")
            {
                ShowMessageBox("บันทึกเรียบร้อย");
            }
            else
            {
                ShowMessageBox("ไม่สามารถบันทึกได้");
                return;
            }
            pnlDetail.Visible = false;
            ClearData();
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            
            ClearData();
            pnlDetail.Visible = false;
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.OrgStructureDAO().GetOrgBudget(tbYear.Text, orgCtrl.OrgStructID,
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvBudget.DataSource = ds.Tables[0];
            gvBudget.DataBind();
        }

        private void BindDataToTableBudgetDetail(DataTable dt)
        {
            DataTable dtBudget = new DataTable();

            bool chkCate8 = false;
            bool chkCate9 = false;
            string SUM_Budget8 = "";
            string SUM_Budget9 = "";

            if (dt.Rows.Count == 2) // แสดงว่ามีข้อมูลทั้ง เครื่องเขียน และ แบบพิมพ์
            {
                chkCate8 = true;
                chkCate9 = true;
                SUM_Budget8 = dt.Rows[0]["SUM_Budget"].ToString();
                SUM_Budget9 = dt.Rows[1]["SUM_Budget"].ToString();
            }
            else if (dt.Rows.Count == 1 && dt.Rows[0]["Cate_ID"].ToString() == "8") // มีแต่ข้อมูลเครื่องเขียน
            {
                chkCate8 = true;
                chkCate9 = false;
                SUM_Budget8 = dt.Rows[0]["SUM_Budget"].ToString();
                SUM_Budget9 = "0.00";
            }
            else if (dt.Rows.Count == 1 && dt.Rows[0]["Cate_ID"].ToString() == "9") // มีแต่ข้อมูลแบบพิมพ์
            {
                chkCate9 = true;
                chkCate8 = false;
                SUM_Budget8 = "0.00";
                SUM_Budget9 = dt.Rows[0]["SUM_Budget"].ToString();
            }

            //ทำการใส่ค่าที่ได้มาลงในช่อง budget แต่ละเดือน และ แต่ละประเภท

            //========================== มกราคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month1.Text = dt.Rows[0]["Budget_Month1"].ToString();
                tbBdgCate9_Month1.Text = dt.Rows[1]["Budget_Month1"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month1.Text = dt.Rows[0]["Budget_Month1"].ToString();
                tbBdgCate9_Month1.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month1.Text = "0.00";
                tbBdgCate9_Month1.Text = dt.Rows[0]["Budget_Month1"].ToString();
            }

            //========================== กุมภาพันธ์ ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month2.Text = dt.Rows[0]["Budget_Month2"].ToString();
                tbBdgCate9_Month2.Text = dt.Rows[1]["Budget_Month2"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month2.Text = dt.Rows[0]["Budget_Month2"].ToString();
                tbBdgCate9_Month2.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month2.Text = "0.00";
                tbBdgCate9_Month2.Text = dt.Rows[0]["Budget_Month2"].ToString();
            }

            //========================== มีนาคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month3.Text = dt.Rows[0]["Budget_Month3"].ToString();
                tbBdgCate9_Month3.Text = dt.Rows[1]["Budget_Month3"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month3.Text = dt.Rows[0]["Budget_Month3"].ToString();
                tbBdgCate9_Month3.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month3.Text = "0.00";
                tbBdgCate9_Month3.Text = dt.Rows[0]["Budget_Month3"].ToString();
            }

            //========================== เมษายน ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month4.Text = dt.Rows[0]["Budget_Month4"].ToString();
                tbBdgCate9_Month4.Text = dt.Rows[1]["Budget_Month4"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month4.Text = dt.Rows[0]["Budget_Month4"].ToString();
                tbBdgCate9_Month4.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month4.Text = "0.00";
                tbBdgCate9_Month4.Text = dt.Rows[0]["Budget_Month4"].ToString();
            }

            //========================== พฤษภาคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month5.Text = dt.Rows[0]["Budget_Month5"].ToString();
                tbBdgCate9_Month5.Text = dt.Rows[1]["Budget_Month5"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month5.Text = dt.Rows[0]["Budget_Month5"].ToString();
                tbBdgCate9_Month5.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month5.Text = "0.00";
                tbBdgCate9_Month5.Text = dt.Rows[0]["Budget_Month5"].ToString();
            }

            //========================== มิถุนายน ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month6.Text = dt.Rows[0]["Budget_Month6"].ToString();
                tbBdgCate9_Month6.Text = dt.Rows[1]["Budget_Month6"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month6.Text = dt.Rows[0]["Budget_Month6"].ToString();
                tbBdgCate9_Month6.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month6.Text = "0.00";
                tbBdgCate9_Month6.Text = dt.Rows[0]["Budget_Month6"].ToString();
            }

            //========================== กรกฎาคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month7.Text = dt.Rows[0]["Budget_Month7"].ToString();
                tbBdgCate9_Month7.Text = dt.Rows[1]["Budget_Month7"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month7.Text = dt.Rows[0]["Budget_Month7"].ToString();
                tbBdgCate9_Month7.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month7.Text = "0.00";
                tbBdgCate9_Month7.Text = dt.Rows[0]["Budget_Month7"].ToString();
            }

            //========================== สิงหาคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month8.Text = dt.Rows[0]["Budget_Month8"].ToString();
                tbBdgCate9_Month8.Text = dt.Rows[1]["Budget_Month8"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month8.Text = dt.Rows[0]["Budget_Month8"].ToString();
                tbBdgCate9_Month8.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month8.Text = "0.00";
                tbBdgCate9_Month8.Text = dt.Rows[0]["Budget_Month8"].ToString();
            }

            //========================== กันยายน ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month9.Text = dt.Rows[0]["Budget_Month9"].ToString();
                tbBdgCate9_Month9.Text = dt.Rows[1]["Budget_Month9"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month9.Text = dt.Rows[0]["Budget_Month9"].ToString();
                tbBdgCate9_Month9.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month9.Text = "0.00";
                tbBdgCate9_Month9.Text = dt.Rows[0]["Budget_Month9"].ToString();
            }

            //========================== ตุลาคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month10.Text = dt.Rows[0]["Budget_Month10"].ToString();
                tbBdgCate9_Month10.Text = dt.Rows[1]["Budget_Month10"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month10.Text = dt.Rows[0]["Budget_Month10"].ToString();
                tbBdgCate9_Month10.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month10.Text = "0.00";
                tbBdgCate9_Month10.Text = dt.Rows[0]["Budget_Month10"].ToString();
            }

            //========================== พฤษจิกายน ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month11.Text = dt.Rows[0]["Budget_Month11"].ToString();
                tbBdgCate9_Month11.Text = dt.Rows[1]["Budget_Month11"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month11.Text = dt.Rows[0]["Budget_Month11"].ToString();
                tbBdgCate9_Month11.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month11.Text = "0.00";
                tbBdgCate9_Month11.Text = dt.Rows[0]["Budget_Month11"].ToString();
            }

            //========================== ธันวาคม ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                tbBdgCate8_Month12.Text = dt.Rows[0]["Budget_Month12"].ToString();
                tbBdgCate9_Month12.Text = dt.Rows[1]["Budget_Month12"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                tbBdgCate8_Month12.Text = dt.Rows[0]["Budget_Month12"].ToString();
                tbBdgCate9_Month12.Text = "0.00";
            }
            else
            {
                tbBdgCate8_Month12.Text = "0.00";
                tbBdgCate9_Month12.Text = dt.Rows[0]["Budget_Month12"].ToString();
            }

            //========================== ยอดยกมาของการเบิกใช้ ===============================/
            if (chkCate8 == true && chkCate9 == true)
            {
                UseAmount_tbBdgCate8.Text = dt.Rows[0]["Use_Amount"].ToString();
                UseAmount_tbBdgCate9.Text = dt.Rows[1]["Use_Amount"].ToString();
            }
            else if (chkCate8 == true && chkCate9 == false)
            {
                UseAmount_tbBdgCate8.Text = dt.Rows[0]["Use_Amount"].ToString();
                UseAmount_tbBdgCate9.Text = "0.00";
            }
            else
            {
                UseAmount_tbBdgCate8.Text = "0.00";
                UseAmount_tbBdgCate9.Text = dt.Rows[0]["Use_Amount"].ToString();
            }

                // ส่วนรวม เป็น row สรุปสุดท้าย
                Total_tbBdgCate8.Text = SUM_Budget8;
                Total_tbBdgCate9.Text = SUM_Budget9;
                
        }


        public void ClearData()
        {
            tbYearDetail.Text = "";

            // เครื่องเขียน
            tbBdgCate8_Month1.Text = "";
            tbBdgCate8_Month2.Text = "";
            tbBdgCate8_Month3.Text = "";
            tbBdgCate8_Month4.Text = "";
            tbBdgCate8_Month5.Text = "";
            tbBdgCate8_Month6.Text = "";
            tbBdgCate8_Month7.Text = "";
            tbBdgCate8_Month8.Text = "";
            tbBdgCate8_Month9.Text = "";
            tbBdgCate8_Month10.Text = "";
            tbBdgCate8_Month11.Text = "";
            tbBdgCate8_Month12.Text = "";
            Total_tbBdgCate8.Text = "";
            rblCate8_Status.SelectedIndex = 0;
            UseAmount_tbBdgCate8.Text = "";

            // แบบพิมพ์
            tbBdgCate9_Month1.Text = "";
            tbBdgCate9_Month2.Text = "";
            tbBdgCate9_Month3.Text = "";
            tbBdgCate9_Month4.Text = "";
            tbBdgCate9_Month5.Text = "";
            tbBdgCate9_Month6.Text = "";
            tbBdgCate9_Month7.Text = "";
            tbBdgCate9_Month8.Text = "";
            tbBdgCate9_Month9.Text = "";
            tbBdgCate9_Month10.Text = "";
            tbBdgCate9_Month11.Text = "";
            tbBdgCate9_Month12.Text = "";
            Total_tbBdgCate9.Text = "";
            rblCate9_Status.SelectedIndex = 0;
            UseAmount_tbBdgCate9.Text = "";

        }

        protected void Calculate(object sender, EventArgs e)
        {
            decimal bdgCate8_M1 = 0;
            decimal bdgCate8_M2 = 0;
            decimal bdgCate8_M3 = 0;
            decimal bdgCate8_M4 = 0;
            decimal bdgCate8_M5 = 0;
            decimal bdgCate8_M6 = 0;
            decimal bdgCate8_M7 = 0;
            decimal bdgCate8_M8 = 0;
            decimal bdgCate8_M9 = 0;
            decimal bdgCate8_M10 = 0;
            decimal bdgCate8_M11 = 0;
            decimal bdgCate8_M12 = 0;

            decimal bdgCate9_M1 = 0;
            decimal bdgCate9_M2 = 0;
            decimal bdgCate9_M3 = 0;
            decimal bdgCate9_M4 = 0;
            decimal bdgCate9_M5 = 0;
            decimal bdgCate9_M6 = 0;
            decimal bdgCate9_M7 = 0;
            decimal bdgCate9_M8 = 0;
            decimal bdgCate9_M9 = 0;
            decimal bdgCate9_M10 = 0;
            decimal bdgCate9_M11 = 0;
            decimal bdgCate9_M12 = 0;

            bdgCate8_M1 = tbBdgCate8_Month1.Text == ""? 0 : Convert.ToDecimal(tbBdgCate8_Month1.Text);
            bdgCate8_M2 = tbBdgCate8_Month2.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month2.Text);
            bdgCate8_M3 = tbBdgCate8_Month3.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month3.Text);
            bdgCate8_M4 = tbBdgCate8_Month4.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month4.Text);
            bdgCate8_M5 = tbBdgCate8_Month5.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month5.Text);
            bdgCate8_M6 = tbBdgCate8_Month6.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month6.Text);
            bdgCate8_M7 = tbBdgCate8_Month7.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month7.Text);
            bdgCate8_M8 = tbBdgCate8_Month8.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month8.Text);
            bdgCate8_M9 = tbBdgCate8_Month9.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month9.Text);
            bdgCate8_M10 = tbBdgCate8_Month10.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month10.Text);
            bdgCate8_M11 = tbBdgCate8_Month11.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month11.Text);
            bdgCate8_M12 = tbBdgCate8_Month12.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month12.Text);

            bdgCate9_M1 = tbBdgCate9_Month1.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month1.Text);
            bdgCate9_M2 = tbBdgCate9_Month2.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month2.Text);
            bdgCate9_M3 = tbBdgCate9_Month3.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month3.Text);
            bdgCate9_M4 = tbBdgCate9_Month4.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month4.Text);
            bdgCate9_M5 = tbBdgCate9_Month5.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month5.Text);
            bdgCate9_M6 = tbBdgCate9_Month6.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month6.Text);
            bdgCate9_M7 = tbBdgCate9_Month7.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month7.Text);
            bdgCate9_M8 = tbBdgCate9_Month8.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month8.Text);
            bdgCate9_M9 = tbBdgCate9_Month9.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month9.Text);
            bdgCate9_M10 = tbBdgCate9_Month10.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month10.Text);
            bdgCate9_M11 = tbBdgCate9_Month11.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month11.Text);
            bdgCate9_M12 = tbBdgCate9_Month12.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month12.Text);

            Total_tbBdgCate8.Text = (bdgCate8_M1 + bdgCate8_M2 + bdgCate8_M3 + bdgCate8_M4 + bdgCate8_M5 + bdgCate8_M6 +
                               bdgCate8_M7 + bdgCate8_M8 + bdgCate8_M9 + bdgCate8_M10 + bdgCate8_M11 + bdgCate8_M12).ToString();

            Total_tbBdgCate9.Text = (bdgCate9_M1 + bdgCate9_M2 + bdgCate9_M3 + bdgCate9_M4 + bdgCate9_M5 + bdgCate9_M6 +
                               bdgCate9_M7 + bdgCate9_M8 + bdgCate9_M9 + bdgCate9_M10 + bdgCate9_M11 + bdgCate9_M12).ToString();
            
        }
        
        //ทำการนำจำนวนรวมของแต่ละประเภทอุปกรณ์ มาหาค่าเฉลี่ยต่อเดือน (หารด้วย 12) (เครื่องเขียน)
        protected void CalculateTotal_Cate8(object sender, EventArgs e)
        {
            if (Total_tbBdgCate8.Text == "")
            {
                ShowMessageBox("กรูณาระบุงบประมาณรวมของเครื่องเขียน");
                Total_tbBdgCate8.Focus();
                return;
            }

            decimal Avg_Cate8 = 0;


            Avg_Cate8 = (Total_tbBdgCate8.Text == "" ? 0 : Convert.ToDecimal(Total_tbBdgCate8.Text))/12;

            tbBdgCate8_Month1.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month2.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month3.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month4.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month5.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month6.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month7.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month8.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month9.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month10.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month11.Text = Avg_Cate8.ToString("0.00");
            tbBdgCate8_Month12.Text = Avg_Cate8.ToString("0.00");

        }

        //ทำการนำจำนวนรวมของแต่ละประเภทอุปกรณ์ มาหาค่าเฉลี่ยต่อเดือน (หารด้วย 12) (แบบพิมพ์)
        protected void CalculateTotal_Cate9(object sender, EventArgs e)
        {
            if (Total_tbBdgCate9.Text == "")
            {
                ShowMessageBox("กรูณาระบุงบประมาณรวมของแบบพิมพ์");
                Total_tbBdgCate9.Focus();
                return;

            }

            decimal Avg_Cate9 = 0;

            Avg_Cate9 = (Total_tbBdgCate9.Text == "" ? 0 : Convert.ToDecimal(Total_tbBdgCate9.Text)) / 12;

            tbBdgCate9_Month1.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month2.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month3.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month4.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month5.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month6.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month7.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month8.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month9.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month10.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month11.Text = Avg_Cate9.ToString("0.00");
            tbBdgCate9_Month12.Text = Avg_Cate9.ToString("0.00");

        }

        protected DataTable SaveData()
        {
            string status_Cate8 = rblCate8_Status.SelectedIndex == 0 ? "1" : "0";
            string status_Cate9 = rblCate9_Status.SelectedIndex == 0 ? "1" : "0";

            decimal bdgCate8_M1 = 0;
            decimal bdgCate8_M2 = 0;
            decimal bdgCate8_M3 = 0;
            decimal bdgCate8_M4 = 0;
            decimal bdgCate8_M5 = 0;
            decimal bdgCate8_M6 = 0;
            decimal bdgCate8_M7 = 0;
            decimal bdgCate8_M8 = 0;
            decimal bdgCate8_M9 = 0;
            decimal bdgCate8_M10 = 0;
            decimal bdgCate8_M11 = 0;
            decimal bdgCate8_M12 = 0;
            decimal UseAmount_Cate8 = 0;

            decimal bdgCate9_M1 = 0;
            decimal bdgCate9_M2 = 0;
            decimal bdgCate9_M3 = 0;
            decimal bdgCate9_M4 = 0;
            decimal bdgCate9_M5 = 0;
            decimal bdgCate9_M6 = 0;
            decimal bdgCate9_M7 = 0;
            decimal bdgCate9_M8 = 0;
            decimal bdgCate9_M9 = 0;
            decimal bdgCate9_M10 = 0;
            decimal bdgCate9_M11 = 0;
            decimal bdgCate9_M12 = 0;
            decimal UseAmount_Cate9 = 0;

            bdgCate8_M1 = tbBdgCate8_Month1.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month1.Text);
            bdgCate8_M2 = tbBdgCate8_Month2.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month2.Text);
            bdgCate8_M3 = tbBdgCate8_Month3.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month3.Text);
            bdgCate8_M4 = tbBdgCate8_Month4.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month4.Text);
            bdgCate8_M5 = tbBdgCate8_Month5.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month5.Text);
            bdgCate8_M6 = tbBdgCate8_Month6.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month6.Text);
            bdgCate8_M7 = tbBdgCate8_Month7.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month7.Text);
            bdgCate8_M8 = tbBdgCate8_Month8.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month8.Text);
            bdgCate8_M9 = tbBdgCate8_Month9.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month9.Text);
            bdgCate8_M10 = tbBdgCate8_Month10.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month10.Text);
            bdgCate8_M11 = tbBdgCate8_Month11.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month11.Text);
            bdgCate8_M12 = tbBdgCate8_Month12.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate8_Month12.Text);
            UseAmount_Cate8 = UseAmount_tbBdgCate8.Text == "" ? 0 : Convert.ToDecimal(UseAmount_tbBdgCate8.Text);

            bdgCate9_M1 = tbBdgCate9_Month1.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month1.Text);
            bdgCate9_M2 = tbBdgCate9_Month2.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month2.Text);
            bdgCate9_M3 = tbBdgCate9_Month3.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month3.Text);
            bdgCate9_M4 = tbBdgCate9_Month4.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month4.Text);
            bdgCate9_M5 = tbBdgCate9_Month5.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month5.Text);
            bdgCate9_M6 = tbBdgCate9_Month6.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month6.Text);
            bdgCate9_M7 = tbBdgCate9_Month7.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month7.Text);
            bdgCate9_M8 = tbBdgCate9_Month8.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month8.Text);
            bdgCate9_M9 = tbBdgCate9_Month9.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month9.Text);
            bdgCate9_M10 = tbBdgCate9_Month10.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month10.Text);
            bdgCate9_M11 = tbBdgCate9_Month11.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month11.Text);
            bdgCate9_M12 = tbBdgCate9_Month12.Text == "" ? 0 : Convert.ToDecimal(tbBdgCate9_Month12.Text);
            UseAmount_Cate9 = UseAmount_tbBdgCate9.Text == "" ? 0 : Convert.ToDecimal(UseAmount_tbBdgCate9.Text);

            GPlus.DataAccess.OrgStructureDAO st = new DataAccess.OrgStructureDAO();

            SQLParameterList sqlParams = new SQLParameterList();

            sqlParams.AddStringField("budget_year", tbYearDetail.Text);
            sqlParams.AddStringField("OrgStruc_Id", orgCtrl.OrgStructID);
            sqlParams.AddDecimalField("bdgCate8_M1", bdgCate8_M1);
            sqlParams.AddDecimalField("bdgCate8_M2", bdgCate8_M2);
            sqlParams.AddDecimalField("bdgCate8_M3", bdgCate8_M3);
            sqlParams.AddDecimalField("bdgCate8_M4", bdgCate8_M4);
            sqlParams.AddDecimalField("bdgCate8_M5", bdgCate8_M5);
            sqlParams.AddDecimalField("bdgCate8_M6", bdgCate8_M6);
            sqlParams.AddDecimalField("bdgCate8_M7", bdgCate8_M7);
            sqlParams.AddDecimalField("bdgCate8_M8", bdgCate8_M8);
            sqlParams.AddDecimalField("bdgCate8_M9", bdgCate8_M9);
            sqlParams.AddDecimalField("bdgCate8_M10", bdgCate8_M10);
            sqlParams.AddDecimalField("bdgCate8_M11", bdgCate8_M11);
            sqlParams.AddDecimalField("bdgCate8_M12", bdgCate8_M12);
            sqlParams.AddDecimalField("UseAmount_Cate8", UseAmount_Cate8);

            sqlParams.AddDecimalField("bdgCate9_M1", bdgCate9_M1);
            sqlParams.AddDecimalField("bdgCate9_M2", bdgCate9_M2);
            sqlParams.AddDecimalField("bdgCate9_M3", bdgCate9_M3);
            sqlParams.AddDecimalField("bdgCate9_M4", bdgCate9_M4);
            sqlParams.AddDecimalField("bdgCate9_M5", bdgCate9_M5);
            sqlParams.AddDecimalField("bdgCate9_M6", bdgCate9_M6);
            sqlParams.AddDecimalField("bdgCate9_M7", bdgCate9_M7);
            sqlParams.AddDecimalField("bdgCate9_M8", bdgCate9_M8);
            sqlParams.AddDecimalField("bdgCate9_M9", bdgCate9_M9);
            sqlParams.AddDecimalField("bdgCate9_M10", bdgCate9_M10);
            sqlParams.AddDecimalField("bdgCate9_M11", bdgCate9_M11);
            sqlParams.AddDecimalField("bdgCate9_M12", bdgCate9_M12);
            sqlParams.AddDecimalField("UseAmount_Cate9", UseAmount_Cate9);

            sqlParams.AddStringField("status_Cate8", status_Cate8);
            sqlParams.AddStringField("status_Cate9", status_Cate9);
            sqlParams.AddStringField("User_ID", this.UserID);

            return st.OrgBudgetInsertUpdate(sqlParams.GetSqlParameterList());
        }

    }
}