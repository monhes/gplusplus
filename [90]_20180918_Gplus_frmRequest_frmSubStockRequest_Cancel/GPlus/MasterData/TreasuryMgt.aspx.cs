using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class TreasuryMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "109";
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtStockCodeSearch.Text = "";
            txtStockNameSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
        }

        protected void gvStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[5].Text = drv["Stock_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                e.Row.Cells[3].Text = drv["Stock_Type"].ToString() == "1" ? "คลังหลัก" : "คลังย่อย";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Stock_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.StockDAO().GetStock(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtStockCode.Text = dt.Rows[0]["Stock_Code"].ToString();
                    txtStockName.Text = dt.Rows[0]["Stock_Name"].ToString();
                    if (ddlStockType.Items.FindByValue(dt.Rows[0]["Stock_Type"].ToString()) != null)
                        ddlStockType.SelectedValue = dt.Rows[0]["Stock_Type"].ToString();

                    if (ddlStockLevel.Items.FindByValue(dt.Rows[0]["LevelStk_Id"].ToString()) != null)
                        ddlStockLevel.SelectedValue = dt.Rows[0]["LevelStk_Id"].ToString();

                    if (ddlFromMainStock.Items.FindByValue(dt.Rows[0]["Stock_CodeReq"].ToString()) != null)
                        ddlFromMainStock.SelectedValue = dt.Rows[0]["Stock_CodeReq"].ToString();

                    if (ddlFromSubStock.Items.FindByValue(dt.Rows[0]["LevelStk_IdReq"].ToString()) != null)
                        ddlFromSubStock.SelectedValue = dt.Rows[0]["LevelStk_IdReq"].ToString();

                    chkPack.Checked = dt.Rows[0]["BaseUnit_flag"].ToString() == "1";
                    chkApprove.Checked = dt.Rows[0]["MustApprove_Flag"].ToString() == "1";

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Stock_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Stock_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);

                    clearCheckboxList();

                    if (dt.Rows[0]["TempStk_Flag"].ToString() != "")
                    {
                        ChlTempStk_Flag.SelectedIndex = Convert.ToInt16(dt.Rows[0]["TempStk_Flag"].ToString())-1;
                    }
                    
                }

                pnlDetail.Visible = true;
            }
        }

        protected void gvStock_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvStock);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retValue = "";
            if (ddlStockType.SelectedValue == "1")
            {
                if (ddlStockLevel.SelectedIndex > 0)
                {
                    this.ShowMessageBox("คลังสินค้าหลักต้องไม่มีระดับคลังย่อย!");
                    ddlStockLevel.Focus();
                    return;
                }
                if (ddlFromMainStock.SelectedIndex > 0)
                {
                    this.ShowMessageBox("คลังสินค้าหลักต้องไม่มีการเบิกสินค้า!");
                    ddlFromMainStock.Focus();
                    return;
                }
                if (ddlFromSubStock.SelectedIndex > 0)
                {
                    this.ShowMessageBox("คลังสินค้าหลักต้องไม่มีเบิกสินค้าระหว่างคลังย่อย!");
                    ddlFromSubStock.Focus();
                    return;
                }
            }
            else if (ddlStockType.SelectedValue == "2")
            {
                if (ddlStockLevel.SelectedIndex == 0)
                {
                    this.ShowMessageBox("กรุณาระบุระดับคลังย่อย!");
                    ddlStockLevel.Focus();
                    return;
                }
                DataTable dtStock = null;
                if (ddlFromSubStock.SelectedValue != "")
                {
                    //dtStock = new DataAccess.StockDAO().GetStock(ddlFromMainStock.SelectedValue);
                    //if (dtStock.Rows.Count > 0)
                    //{
                    if (ddlFromSubStock.SelectedValue != ddlStockLevel.SelectedValue)
                        {
                            ddlFromSubStock.Focus();
                            this.ShowMessageBox("การเบิกสินค้าระหว่างคลังย่อยต้องเป็นระดับเดียวกัน");
                            return;
                        }
                    //}
                }
                if (ddlFromMainStock.SelectedValue != "")
                {
                    int stockLevel = int.Parse(ddlStockLevel.SelectedValue);
                    dtStock = new DataAccess.StockDAO().GetStock(ddlFromMainStock.SelectedValue);
                    if (dtStock.Rows.Count > 0)
                    {
                        if (stockLevel == 1 && dtStock.Rows[0]["LevelStk_Id"].ToString() != "")
                        {
                            ddlFromMainStock.Focus();
                            this.ShowMessageBox("คลังสินค้า Level " + stockLevel.ToString() + " ต้องเบิกจากคลังหลัก");
                            return;
                        }
                        else if (stockLevel > 1 && (stockLevel - 1).ToString() != dtStock.Rows[0]["LevelStk_Id"].ToString())
                        {
                            ddlFromMainStock.Focus();
                            this.ShowMessageBox("คลังสินค้า Level " + stockLevel.ToString() + " ต้องเบิกจากคลัง Level " + (stockLevel - 1).ToString());
                            return;
                        }
                    }
                }

            }
            string TempStk_Flag = "";

            for (int i = 0; i <= (int)this.ChlTempStk_Flag.Items.Count - 1; i++)
            {
                if (this.ChlTempStk_Flag.Items[i].Selected == true)
                {
                    TempStk_Flag = (i+1).ToString();
                }
            }

            if (hdID.Value == "")
            {
               retValue = new DataAccess.StockDAO().AddStock(txtStockCode.Text, txtStockName.Text,ddlStockType.SelectedValue, ddlStockLevel.SelectedValue,
                    ddlFromMainStock.SelectedValue, ddlFromSubStock.SelectedValue, chkPack.Checked?"1":"0",chkApprove.Checked?"1":"0", status, this.UserName, TempStk_Flag);
                if(retValue != "0")
                    hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.StockDAO().UpdateStock(hdID.Value, txtStockCode.Text, txtStockName.Text, ddlStockType.SelectedValue, ddlStockLevel.SelectedValue,
                    ddlFromMainStock.SelectedValue, ddlFromSubStock.SelectedValue, chkPack.Checked ? "1" : "0", chkApprove.Checked ? "1" : "0", status, this.UserName, TempStk_Flag);
            }

            if (retValue == "0")
            {
                this.ShowMessageBox("มีรหัส "+txtStockCode.Text+" อยู่ในระบบแล้ว");
                txtStockCode.Focus();
                return;
            }

            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }


        private void BindDropdown()
        {
            ddlFromMainStock.DataSource = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlFromMainStock.DataBind();
            ddlFromMainStock.Items.Insert(0, new ListItem("เลือกคลังสินค้า", ""));
            
            DataTable dt = new DataAccess.StockDAO().GetLevelStk( "", "1", 1, 1000, "", "").Tables[0];
            ddlStockLevel.DataSource = dt;
            ddlStockLevel.DataBind();
            ddlStockLevel.Items.Insert(0, new ListItem("เลือกระดับคลังย่อย", ""));

            ddlFromSubStock.DataSource = dt;
            ddlFromSubStock.DataBind();
            ddlFromSubStock.Items.Insert(0, new ListItem("เลือกระดับคลังย่อย", ""));
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.StockDAO().GetStock(txtStockCodeSearch.Text, txtStockNameSearch.Text, ddlStatus.SelectedValue, 
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvStock.DataSource = ds.Tables[0];
            gvStock.DataBind();
        }

        public void ClearData()
        {
            hdID.Value = "";
            txtStockCode.Text = "";
            txtStockName.Text = "";
            ddlStockType.SelectedIndex = 0;
            ddlStockLevel.SelectedIndex = 0;
            ddlFromMainStock.SelectedIndex = 0;
            ddlFromSubStock.SelectedIndex = 0;
            chkPack.Checked = false;
            chkApprove.Checked = false;
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
            clearCheckboxList();
        }

        public void clearCheckboxList()
        {
            for (int i = 0; i <= (int)this.ChlTempStk_Flag.Items.Count - 1; i++)
            {
                this.ChlTempStk_Flag.Items[i].Selected = false;
            }
        }

    }
}