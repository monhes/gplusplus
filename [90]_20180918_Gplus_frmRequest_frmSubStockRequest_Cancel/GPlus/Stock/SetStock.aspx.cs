using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
namespace GPlus.Stock
{
    public partial class SetStock : Pagebase
    {
        public DataTable SetStockPackageTable
        {
            get
            {
                return (DataTable)Session["SetStockAdjust"];
            }
            set
            {
                Session["SetStockAdjust"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "408";
                Session["FirstVisit"] = "True";
                
                BindDropdown();

                CreateDataTable();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
            PagingControl2.CurrentPageIndexChanged += new EventHandler(PagingControl2_CurrentPageIndexChanged);
            PagingControl3.CurrentPageIndexChanged += new EventHandler(PagingControl3_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();

            DataTable dt2 = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(this.OrgID));
            drpDivision.DataSource = dt2;
            drpDivision.DataTextField = "DivName";
            drpDivision.DataValueField = "Div_Code";
            drpDivision.DataBind();

            drpDepartment.DataSource = dt2;
            drpDepartment.DataTextField = "DepName";
            drpDepartment.DataValueField = "Dep_Code";
            drpDepartment.DataBind();

        }

        private void BindDivDep(string OrgID)
        {

            DataTable dt2 = new DataAccess.OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt16(OrgID));
            drpDivision.DataSource = dt2;
            drpDivision.DataTextField = "DivName";
            drpDivision.DataValueField = "Div_Code";
            drpDivision.DataBind();

            drpDepartment.DataSource = dt2;
            drpDepartment.DataTextField = "DepName";
            drpDepartment.DataValueField = "Dep_Code";
            drpDepartment.DataBind();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            hdTransHead_ID.Value = ""; //Clear ค่า Hidden ของ TransHead
            PagingControl2.CurrentPageIndex = 1;
            
            //ลบข้อมูลใน tempExcel ออก ที่ StockId เท่ากับ Stockid ของ User ที่ login เข้ามา
            new DataAccess.StockDAO().DeleteTempExcel(ddlStock.SelectedValue);

            ClearDataAdd();
            TxtTransBy.Text = this.FirstName + " " + this.LastName;
            BindDivDep(this.OrgID);
            if (rdbMode.SelectedIndex == 0 || rdbMode.SelectedIndex == 1) //ตรวจนับผ่านทาง Web หรือ ตรวจนับผ่านทาง Mobile
            {
                Session["FirstVisit"] = "True";
                CreateDataTable(); //Clear ค่าใน DataTable ที่เก็บการ Adjust Stock ทั้งหมด
                FileImport.Visible = false;
                btnImport.Visible = false;
                btnAddMoreSetStk.Visible = false;
                PagingControl2.Visible = false;
                pnlDetail.Visible = true;
                rdbCntStockType.SelectedIndex = 0;
                rdbCntStockType.Items[3].Enabled = false; // Disable ปุ่ม SetStock
                showPartDiff.Visible = true;
                RdbShowDiff.SelectedIndex = 1;
                BindDataAdjustSTock();
            }
            else if (rdbMode.SelectedIndex == 2) // SetStock
            {
                FileImport.Visible = true;
                btnImport.Visible = true;
                btnAddMoreSetStk.Visible = true;
                pnlDetail.Visible = true;
                rdbCntStockType.SelectedIndex = 3;
                rdbCntStockType.Items[0].Enabled = false; // Disable ปุ่มประจำปี
                rdbCntStockType.Items[1].Enabled = false; // Disable ปุ่มประจำเดือน
                rdbCntStockType.Items[2].Enabled = false; // Disable ปุ่มประจำวัน
                showPartDiff.Visible = false;
            }
            

        }

        protected void btnAddMoreSetStk_Click(object sender, EventArgs e)
        {
            //if (dtTransDate.Text == "")
            //{
            //    ShowMessageBox("กรุณาระบุวันที่ตรวจนับ");
            //    return;
            //}
            //else
            //{
                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowAddItemSetStock", "open_popup('pop_SetStockAddItem.aspx?stockID=" + ddlStock.SelectedValue +
                 "', 850, 450, 'pop', 'yes', 'yes', 'yes');", true);
            //}

        }

        protected void rdbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbMode.SelectedIndex == 2)
            {
                chkCntYear.Checked = false;
                chkCntMonth.Checked = false;
                chkCntDay.Checked = false;
                chkCntYear.Enabled = false;
                chkCntMonth.Enabled = false;
                chkCntDay.Enabled = false;
            }
            else
            {
                chkCntYear.Enabled = true;
                chkCntMonth.Enabled = true;
                chkCntDay.Enabled = true;
            }
        }


        //private void ClearDataAdd()
        //{
        //    txtTransactionNO.Text = "";
        //    dtTransDate.Text = "";
        //    rdbCntStockType.SelectedIndex = 0;
        //    rdbCntStockType.Enabled = true;
        //    FileImport.Enabled = true;
        //    btnImport.Enabled = true;
        //    Gv_SumExcel.Visible = false;
        //    Show_Gv_SumExcel.Visible = false;

        //}

        private void ClearDataAdd()
        {
            rdbCntStockType.Enabled = true;
            rdbCntStockType.Items[0].Enabled = true; // Enable ปุ่มประจำปี
            rdbCntStockType.Items[1].Enabled = true; // Enable ปุ่มประจำเดือน
            rdbCntStockType.Items[2].Enabled = true; // Enable ปุ่มประจำวัน
            rdbCntStockType.Items[3].Enabled = true; // Enable ปุ่ม SetStock

            txtTransactionNO.Text = "";
            dtTransDate.Text = "";
            Gv_SumExcel.Visible = false;
            Show_Gv_SumExcel.Visible = false;

        }

   

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (rdbCntStockType.Items[0].Selected) //ประจำปี
            {
                //ShowMessageBox(rdbCntStockType.Items[0].Text);
            }
            else if (rdbCntStockType.Items[1].Selected) //ประจำเดือน
            {
                //ShowMessageBox(rdbCntStockType.Items[1].Text);
            }
            else if (rdbCntStockType.Items[2].Selected) //ประจำวัน
            {
                //ShowMessageBox(rdbCntStockType.Items[2].Text);
            }
            else //SetStock
            {
                ImportSetStock();
            }


        }

        private void ImportSetStock()
        {
          
            rdbCntStockType.Enabled = false;

            //Delete File From Inv_tempExcel where stockID = User
            new DataAccess.StockDAO().DeleteTempExcel(ddlStock.SelectedValue);

            //file upload path
            string strConnection = new DataAccess.DatabaseHelper().ConnectionString;
            if(Path.GetFileNameWithoutExtension(FileImport.PostedFile.FileName) == "" )
            {
                ShowMessageBox("กรณา Browse file ใหม่อีกครั้ง");
                return;
            }

            string filename = Path.GetFileNameWithoutExtension(FileImport.PostedFile.FileName);
            string extension = Path.GetExtension(FileImport.PostedFile.FileName);
            string saveFileName = filename + System.DateTime.Now.ToString("_ddMMyyhhmmss") + extension;
            //FileImport.SaveAs(Server.MapPath("Files/" + saveFileName));
            //string path = Server.MapPath("Files/" + saveFileName);
            FileImport.SaveAs(Server.MapPath("~/Files/" + saveFileName));
            string path = Server.MapPath("~/Files/" + saveFileName);
            string excelConnectionString = "";
            //Create connection string to Excel work book
            switch (extension)
            {
                case ".xls": //Excel 97-03
                    excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0;Persist Security Info=False";
                    break;
                case ".xlsx": //Excel 07
                    excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    break;
            }
            //string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;Persist Security Info=False";
            //Create Connection to Excel work book
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
            try
            {
                //Create OleDbCommand to fetch data from Excel
                OleDbCommand cmd = new OleDbCommand("Select '',"+ddlStock.SelectedValue+" AS StockID,[รหัสสินค้า],[หน่วยนับ],[Lot_No],[สถานที่เก็บ],[จำนวนที่นับได้] from [Sheet1$]", excelConnection);
                excelConnection.Open();
                OleDbDataReader dReader;
                dReader = cmd.ExecuteReader();
                SqlBulkCopy sqlBulk = new SqlBulkCopy(strConnection);
                //Give your Destination table name
                sqlBulk.DestinationTableName = "Inv_tempExcel";
                // Set up the column mappings by name.
                SqlBulkCopyColumnMapping mapStockID =
                    new SqlBulkCopyColumnMapping("StockID", "Stock_id");
                sqlBulk.ColumnMappings.Add(mapStockID);

                SqlBulkCopyColumnMapping mapItemCode =
                    new SqlBulkCopyColumnMapping("รหัสสินค้า", "Inv_ItemCode");
                sqlBulk.ColumnMappings.Add(mapItemCode);

                SqlBulkCopyColumnMapping mapPackName =
                    new SqlBulkCopyColumnMapping("หน่วยนับ", "Package_Name");
                sqlBulk.ColumnMappings.Add(mapPackName);

                SqlBulkCopyColumnMapping mapLot_No =
                    new SqlBulkCopyColumnMapping("Lot_No", "Lot_No");
                sqlBulk.ColumnMappings.Add(mapLot_No);

                SqlBulkCopyColumnMapping mapLocCode =
                   new SqlBulkCopyColumnMapping("สถานที่เก็บ", "Location_Code");
                sqlBulk.ColumnMappings.Add(mapLocCode);

                SqlBulkCopyColumnMapping mapQty =
                    new SqlBulkCopyColumnMapping("จำนวนที่นับได้", "Lot_Qty");
                sqlBulk.ColumnMappings.Add(mapQty);

                sqlBulk.WriteToServer(dReader);
                excelConnection.Close();
                File.Delete(path);
                try
                {
                    BindDataSetStockExcel();
                }
                catch (Exception ex)
                {
                    ShowMessageBox("ไม่สามารถประมวลผลข้อมูลได้");
                    rdbCntStockType.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox("ไม่สามารถ Import ข้อมูลได้");
                excelConnection.Close();
                File.Delete(path);
                rdbCntStockType.Enabled = true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            //gvMovement.Visible = true;
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            rdbMode.SelectedIndex = 0;
            txtTransactionNOsearch.Text = "";
            dtCreateStart.Text = "";
            dtCreateStop.Text = "";
            chkCntYear.Checked = false;
            chkCntMonth.Checked = false;
            chkCntDay.Checked = false;
            gvSetStock.Visible = false;
            pnlDetail.Visible = false;
        }

        protected void gvSetStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[7].Text = drv["Transaction_Status"].ToString() == "True" ? "<span style='color:navy'>Active</span>" :
                   "<span style='color:red'>InActive</span>";

                if (drv["Transaction_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[2].Text = ((DateTime)drv["Transaction_Date"]).ToString(this.DateFormat);

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Transaction_ID"].ToString();
                hdTransHead_ID.Value = drv["Transaction_ID"].ToString();

            }
        }

        protected void gvSetStock_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvSetStock);
        }

        protected void gvSetStock_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                
                //if (rdbMode.SelectedIndex == 2)
                //{
                    PagingControl2.CurrentPageIndex = 1;
                    
                    DataTable dtTH = new DataAccess.StockDAO().GetTransHead(e.CommandArgument.ToString());
                    if (dtTH.Rows.Count > 0)
                    {
                        ClearDataAdd();

                        rdbCntStockType.Enabled = false;

                        txtTransactionNO.Text = dtTH.Rows[0]["Transaction_No"].ToString();
                        dtTransDate.Text = dtTH.Rows[0]["Transaction_Date"].ToString();

                        if (dtTH.Rows[0]["Transaction_Sub_Other"].ToString() == "1") //ตรวจนับประจำปี
                        {
                            rdbCntStockType.SelectedIndex = 0;
                        }
                        else if (dtTH.Rows[0]["Transaction_Sub_Other"].ToString() == "2") //ตรวจนับประจำเดือน
                        {
                            rdbCntStockType.SelectedIndex = 1;
                        }
                        else if (dtTH.Rows[0]["Transaction_Sub_Other"].ToString() == "3") //ตรวจนับประจำวัน
                        {
                            rdbCntStockType.SelectedIndex = 2;
                        }
                        else if (dtTH.Rows[0]["Transaction_Sub_Other"].ToString() == "4") //Set Stock
                        {
                            rdbCntStockType.SelectedIndex = 3;
                        }

                        TxtTransBy.Text = dtTH.Rows[0]["Create_Name"].ToString();

                        if (dtTH.Rows[0]["Refer_OrgStruc_Id"].ToString() != "")
                        {
                            BindDivDep(dtTH.Rows[0]["Refer_OrgStruc_Id"].ToString());
                        }

                        lblCreateBy.Text = dtTH.Rows[0]["Create_By_Name"].ToString();
                        lblUpdateBy.Text = dtTH.Rows[0]["Update_By_Name"].ToString();
                        lblCreateDate.Text = ((DateTime)dtTH.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);
                        lblUpdatedate.Text = ((DateTime)dtTH.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);

                        DataSet ds = new DataAccess.StockDAO().GetTransDetail(e.CommandArgument.ToString(), PagingControl2.CurrentPageIndex, PagingControl2.PageSize, this.SortColumn, this.SortOrder);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            Gv_SumExcel.Visible = false;
                            Show_Gv_SumExcel.Visible = false;
                            ShowMessageBox("ไม่พบข้อมูล");
                        }
                        else
                        {
                            Show_Gv_SumExcel.Visible = true;
                            Gv_SumExcel.Visible = true;
                            PagingControl2.RecordCount = (int)ds.Tables[1].Rows[0][0];
                            PagingControl2.Visible = true;

                            Gv_SumExcel.DataSource = ds.Tables[0];
                            Gv_SumExcel.DataBind();
                        }

                        FileImport.Visible = false;
                        btnImport.Visible = false;
                        btnSave.Visible = false;
                        btnAddMoreSetStk.Visible = false;
                        pnlDetail.Visible = true;
                        PagingControl3.Visible = false;
                        showPartDiff.Visible = false;
                        gvAdjust_Stock.Visible = false;
                    }   
                //}

            }
        }

        private void BindData()
        {
            string CntDay = "";
            string CntMonth = "";
            string CntYear = "";

            if (chkCntDay.Checked)
            {
                CntDay = "1";
            }
            if (chkCntMonth.Checked)
            {
                CntMonth = "2";
            }
            if (chkCntYear.Checked)
            {
                CntYear = "3";
            }
          


            DataSet ds = new DataAccess.StockDAO().SetStockSearch(ddlStock.SelectedValue, rdbMode.SelectedValue, txtTransactionNOsearch.Text,
                dtCreateStart.Text, dtCreateStop.Text, CntYear, CntMonth, CntDay,  PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            if (ds.Tables[0].Rows.Count == 0)
            {
                gvSetStock.Visible = false;
                PagingControl1.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
                pnlDetail.Visible = false;
            }
            else
            {
                gvSetStock.Visible = true;
                PagingControl1.Visible = true;
                PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

                gvSetStock.DataSource = ds.Tables[0];
                gvSetStock.DataBind();
            }
        }

        private void BindDataSetStockExcel()
        {

            DataSet ds = new DataAccess.StockDAO().SetStockSumExcel(ddlStock.SelectedValue, PagingControl2.CurrentPageIndex, PagingControl2.PageSize, this.SortColumn, this.SortOrder);

            if (ds.Tables[0].Rows.Count == 0)
            {
                Gv_SumExcel.Visible = false;
                Show_Gv_SumExcel.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            else
            {
                btnSave.Visible = true;
                Show_Gv_SumExcel.Visible = true;
                PagingControl2.Visible = true;
                PagingControl3.Visible = false;
                Gv_SumExcel.Visible = true;
                gvAdjust_Stock.Visible = false;
                PagingControl2.RecordCount = (int)ds.Tables[1].Rows[0][0];

                Gv_SumExcel.DataSource = ds.Tables[0];
                Gv_SumExcel.DataBind();

                
                lblCreateBy.Text = this.FirstName + " " + this.LastName;
                lblUpdateBy.Text = this.FirstName + " " + this.LastName;
                lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
                lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            }
        }


        private void BindDataTransDetail()
        {

            DataSet ds = new DataAccess.StockDAO().GetTransDetail(hdTransHead_ID.Value, PagingControl2.CurrentPageIndex, PagingControl2.PageSize, this.SortColumn, this.SortOrder);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            Gv_SumExcel.Visible = false;
                            Show_Gv_SumExcel.Visible = false;
                            ShowMessageBox("ไม่พบข้อมูล");
                        }
                        else
                        {
                            Show_Gv_SumExcel.Visible = true;
                            Gv_SumExcel.Visible = true;
                            PagingControl2.RecordCount = (int)ds.Tables[1].Rows[0][0];
                            PagingControl2.Visible = true;

                            Gv_SumExcel.DataSource = ds.Tables[0];
                            Gv_SumExcel.DataBind();
                        }

                        FileImport.Visible = false;
                        btnImport.Visible = false;
                        btnSave.Visible = false;
                        btnAddMoreSetStk.Visible = false;
                        pnlDetail.Visible = true;
                        PagingControl3.Visible = false;
                        showPartDiff.Visible = false;
                        gvAdjust_Stock.Visible = false;
              
        }



        void PagingControl2_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            if (hdTransHead_ID.Value == "")
            {
                BindDataSetStockExcel();
            }
            else
            {
                BindDataTransDetail();
            }
        }

        protected void Gv_SumExcel_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindDataSetStockExcel();
            this.GridViewSort(Gv_SumExcel);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(rdbMode.SelectedValue == "W") //ตรวจนับผ่านทาง Web
            {
                if (dtTransDate.Text == "")
                {
                    ShowMessageBox("กรุณาระบุวันที่ตรวจนับ");
                    return;
                }

                //ShowMessageBox("ตรวจนับผ่านทาง Web");
                SaveAdjustStock();
                DataView dv ;
                DataTable dtResult;
                if (SetStockPackageTable != null)
                { 
                    dv = new DataView(SetStockPackageTable);
                    dv.RowFilter = "Adjust = 'True'";
                    dtResult = dv.ToTable();

                    if (dtResult.Rows.Count < 1)
                    {
                        ShowMessageBox("กรุณาเลือกข้อมูลที่ต้องการ Adjust Stock ");
                        return;
                    }

                    bool result = new DataAccess.StockDAO().insertDataAdjustToTemp(dtResult, ddlStock.SelectedValue);
                    if (!result)
                    {
                        ShowMessageBox("ไม่สามารถนำข้อมูลเข้าระบบได้");
                        return;
                    }
                    else
                    {
                        SaveAllTransactionAdjustStock();
                    }
                    
                }

                //if (rdbCntStockType.Items[0].Selected) //ประจำปี
                //{
                //    ShowMessageBox(rdbCntStockType.Items[0].Text);
                //}
                //else if (rdbCntStockType.Items[1].Selected) //ประจำเดือน
                //{
                //    ShowMessageBox(rdbCntStockType.Items[1].Text);
                //}
                //else if (rdbCntStockType.Items[2].Selected) //ประจำวัน
                //{
                //    ShowMessageBox(rdbCntStockType.Items[2].Text);
                //}
            }
            else if (rdbMode.SelectedValue == "M") //ตรวจนับผ่านทาง Mobile
            {
                //ShowMessageBox("ตรวจนับผ่านทาง Mobile");
            }
            else //SetStock
            {
                //ShowMessageBox("SetStock");
                if (dtTransDate.Text == "")
                {
                    ShowMessageBox("กรุณาระบุวันที่ตรวจนับ");
                    return;
                }
                SaveImportSetStock();
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }

        private void ClearData()
        {
            txtTransactionNO.Text = "";
            dtTransDate.Text = "";
            rdbCntStockType.Enabled = true;
            rdbCntStockType.SelectedIndex= 0;
            lblCreateDate.Text = "";
            lblCreateBy.Text = "";
            lblUpdatedate.Text = "";
            lblUpdateBy.Text = "";
            Gv_SumExcel.Visible = false;
            Show_Gv_SumExcel.Visible = false;
            
        }

        private void SaveImportSetStock()
        {
            String result = new DataAccess.StockDAO().SetStockInsertUpdate(Convert.ToInt16(ddlStock.SelectedValue), Convert.ToInt16(this.UserID), dtTransDate.Text,Convert.ToInt16(this.OrgID),1);
            if (result == "1")
            {
                ShowMessageBox("ทำการบันทึกข้อมูลเรียบร้อย");
                BindData();
            }
            else
            {
                ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้");
            }

            rdbCntStockType.Enabled = true;
            Gv_SumExcel.Visible = false;
            Show_Gv_SumExcel.Visible = false;
            new DataAccess.StockDAO().DeleteTempExcel(ddlStock.SelectedValue);
        }


        private void SaveAdjustStock()
        {
            CopyDataTable();

            
            //String result = new DataAccess.StockDAO().SetStockInsertUpdate(Convert.ToInt16(ddlStock.SelectedValue), Convert.ToInt16(this.UserID), dtTransDate.Text, Convert.ToInt16(this.OrgID), 1);
            //if (result == "1")
            //{
            //    ShowMessageBox("ทำการบันทึกข้อมูลเรียบร้อย");
            //    BindData();
            //}
            //else
            //{
            //    ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้");
            //}

            //rdbCntStockType.Enabled = true;
            //Gv_SumExcel.Visible = false;
            //Show_Gv_SumExcel.Visible = false;
            //new DataAccess.StockDAO().DeleteTempExcel(ddlStock.SelectedValue);
        }

        private void SaveAllTransactionAdjustStock()
        {
            string Transaction_Sub_Other = "";

            if (rdbCntStockType.SelectedIndex == 0)
            {
                Transaction_Sub_Other = "1";
            }
            else if (rdbCntStockType.SelectedIndex == 1)
            {
                Transaction_Sub_Other = "2";
            }
            else if (rdbCntStockType.SelectedIndex == 2)
            {
                Transaction_Sub_Other = "3";
            }

            String result = new DataAccess.StockDAO().SetStockAdjustInsertUpdate(Convert.ToInt16(ddlStock.SelectedValue), Convert.ToInt16(this.UserID), dtTransDate.Text, Convert.ToInt16(this.OrgID), 1, Transaction_Sub_Other);
            if (result == "1")
            {
                ShowMessageBox("ทำการบันทึกข้อมูลเรียบร้อย");
                BindData();
            }
            else
            {
                ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้");
            }
            pnlDetail.Visible = false;

            new DataAccess.StockDAO().DeleteTempExcel(ddlStock.SelectedValue);
            
        }

        protected void btnRefreshItem_Click(object sender, EventArgs e)
        {
            //ShowMessageBox("แสดงข้อมูล");
            PagingControl2.CurrentPageIndex = 1;
            BindDataSetStockExcel();
        }

        protected void btnAdjustRefreshItem_Click(object sender, EventArgs e)
        {
            //ShowMessageBox("แสดงข้อมูล Adjust");
            BindDataAdjustSTock();
        }

        protected void gvAdjust_Stock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label lbl_onHand_Qty = (Label)e.Row.FindControl("lbl_onHand_Qty");
                TextBox txt_Reason = (TextBox)e.Row.FindControl("txt_Reason");
                Label lbl_onHand_diff = (Label)e.Row.FindControl("lbl_onHand_diff");
                Label lbl_summary = (Label)e.Row.FindControl("lbl_summary");
                CheckBox cb_Adjust = (CheckBox)e.Row.FindControl("cb_Adjust");
                TextBox txt_cnt = (TextBox)e.Row.FindControl("txt_cnt");

                if (SetStockPackageTable != null)
                {
                    if (SetStockPackageTable.Rows.Count > 0)
                    {
                        DataView dv = new DataView(SetStockPackageTable);
                        dv.RowFilter = "Inv_ItemId = '" + drv["Inv_ItemID"].ToString() + "' AND Pack_Id = '" + drv["Pack_ID"].ToString()+"'";
                        DataTable dtNew = dv.ToTable();
                        if (dtNew.Rows.Count > 0)
                        {
                            if (dtNew.Rows[0]["Adjust"].ToString() == "True")
                            {
                                cb_Adjust.Checked = true;
                            }
                            else
                            {
                                cb_Adjust.Checked = false;
                            }
                            txt_Reason.Text = dtNew.Rows[0]["Reason"].ToString();
                        }
                    }
                }

                string onHand_qty = lbl_onHand_Qty.Text.Replace(@",", string.Empty);
                //string cnt_qty = txt_cnt.Text.Replace(@",", string.Empty);
                //string cnt_qty = (txt_cnt.Text.Replace(@",", string.Empty) == "" ? "0" : txt_cnt.Text.Replace(@",", string.Empty));
                string cnt_qty = (txt_cnt.Text.Replace(@",", string.Empty) == "" ? "" : txt_cnt.Text.Replace(@",", string.Empty));



                //lbl_onHand_diff.Text = (Convert.ToInt32(lbl_onHand_Qty.Text == "" ? "0" : lbl_onHand_Qty.Text) - Convert.ToInt32(txt_cnt.Text == "" ? "0" : txt_cnt.Text)).ToString("###,##0");
                if (cnt_qty != "")
                {
                    lbl_onHand_diff.Text = (Convert.ToInt32(onHand_qty == "" ? "0" : onHand_qty) - Convert.ToInt32(cnt_qty == "" ? "0" : cnt_qty)).ToString("###,##0");
                }
                else
                {
                    lbl_onHand_diff.Text = "";
                }
                
                //lbl_onHand_diff.Text = (Convert.ToInt32(onHand_qty == "" ? "0" : onHand_qty) - Convert.ToInt32(cnt_qty == "" ? "" : cnt_qty)).ToString("###,##0");
                lbl_onHand_Qty.Text = Convert.ToInt32(drv["OnHand_Qty"]).ToString("###,##0");
                
                //txt_cnt.Text = cnt_qty == "" ? "0" : cnt_qty;

                if (txt_cnt.Text != "0" && txt_cnt.Text != "")
                {
                    //lbl_summary.Text = (Convert.ToInt32(txt_cnt.Text == "" ? "0" : txt_cnt.Text) * Convert.ToDouble(drv["Avg_Cost"].ToString() == "" ? "0" : drv["Avg_Cost"].ToString())).ToString("###,##0.0000");
                    lbl_summary.Text = (Convert.ToInt32(cnt_qty == "" ? "0" : cnt_qty) * Convert.ToDouble(drv["Avg_Cost"].ToString() == "" ? "0" : drv["Avg_Cost"].ToString())).ToString("###,##0.0000");
                }
                else if (txt_cnt.Text == "0")
                {
                    lbl_summary.Text = "0.0000";
                }
                else
                {
                    lbl_summary.Text = "";
                }

                if (lbl_onHand_diff.Text == "0" || lbl_onHand_diff.Text == "")
                {
                    cb_Adjust.Enabled = false;
                }
                else
                {
                    cb_Adjust.Enabled = true;
                }

                
            }
        }


        private void BindDataAdjustSTock()
        {

            DataSet ds = new DataAccess.StockDAO().SetStockAddAdjust(ddlStock.SelectedValue, RdbShowDiff.SelectedIndex, PagingControl3.CurrentPageIndex, PagingControl3.PageSize, "Inv_ItemCode", this.SortOrder);

            if (Session["FirstVisit"].ToString() == "True")
            {
                DataSet ds2 = new DataAccess.StockDAO().SetStockAddAdjust(ddlStock.SelectedValue, RdbShowDiff.SelectedIndex,1,10000, "Inv_ItemCode", this.SortOrder);
                DataTable dt2 = ds2.Tables[0];

                DataView view = new DataView(dt2);
                SetStockPackageTable = view.ToTable(true, new string[] {"Inv_ItemId","Pack_Id","Avg_Cost","Inv_ItemCode","Item_Search_Desc","Pack_Description","OnHand_Qty","Diff","Adjust","Qty_Amount","Reason"});

                //DataRow dr = SetStockPackageTable.NewRow();


                //for (int i = 0; i < dt2.Rows.Count; i++)
                //{
                //    dr["Inv_ItemId"] = dt2.Rows[i]["Inv_ItemId"];
                //    dr["Pack_Id"] = dt2.Rows[i]["Pack_Id"];
                //    dr["Avg_Cost"] = dt2.Rows[i]["Avg_Cost"];
                //    dr["Inv_ItemCode"] = dt2.Rows[i]["Inv_ItemCode"];
                //    dr["Item_Search_Desc"] = dt2.Rows[i]["Item_Search_Desc"];
                //    dr["Pack_Description"] = dt2.Rows[i]["Pack_Description"];
                //    dr["Cnt_Qty"] = dt2.Rows[i]["OnHand_Qty"];
                //    dr["Diff"] = dt2.Rows[i]["Diff"];
                //    dr["Adjust"] = dt2.Rows[i]["Adjust"];
                //    dr["Qty_Amount"] = dt2.Rows[i]["Qty_Amount"];
                //    dr["Reason"] = dt2.Rows[i]["Reason"];
                //    SetStockPackageTable.Rows.Add(dr);
                //    SetStockPackageTable.AcceptChanges();
                //}

                Session["FirstVisit"] = "False";
            } 


            if (ds.Tables[0].Rows.Count == 0)
            {
                gvAdjust_Stock.Visible = false;
                gvAdjust_Stock.Visible = false;
                PagingControl3.Visible = false;
                ShowMessageBox("ไม่พบข้อมูลสินค้าคงคลัง");
            }
            else
            {
                
                btnSave.Visible = true;

                gvAdjust_Stock.DataSource = ds.Tables[0];
                gvAdjust_Stock.DataBind();
                gvAdjust_Stock.Visible = true;
                PagingControl3.Visible = true;
                PagingControl3.RecordCount = (int)ds.Tables[1].Rows[0][0];
                Show_Gv_SumExcel.Visible = true;


                lblCreateBy.Text = this.FirstName + " " + this.LastName;
                lblUpdateBy.Text = this.FirstName + " " + this.LastName;
                lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
                lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

                CopyDataTable();
                
            }
        }


        private void BindDataAdjustSTock_RdbShowDiff()
        {
            if (SetStockPackageTable.Rows.Count == 0)
            {
                gvAdjust_Stock.Visible = false;
                gvAdjust_Stock.Visible = false;
                PagingControl3.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            else
            {
                DataTable dtResult;
                DataView dv;
                int cnt_record;
                btnSave.Visible = true;

                if (RdbShowDiff.SelectedIndex == 0) // แสดงเฉพาะส่วนต่าง
                {
                    dv = new DataView(SetStockPackageTable);
                    //dv.RowFilter = "ISNULL(Diff,0) <> '0'"; //ไม่แสดงส่วนต่าง ที่ Diff เท่ากับ 0
                    dv.RowFilter = "Diff <> '0' AND Diff <> '' ";
                    dv.Sort = "Inv_ItemCode ASC";
                    cnt_record = dv.Count;
                    //dtResult = dv.ToTable().AsEnumerable().Where((row, index) => index > 1 && index < 10).CopyToDataTable();
                    dtResult = dv.ToTable();
                }
                else
                {
                    dv = new DataView(SetStockPackageTable);
                    dv.Sort = "Inv_ItemCode ASC";
                    dtResult = dv.ToTable();
                    cnt_record = SetStockPackageTable.Rows.Count;
                }

                int record_start;
                int record_end;

                if (PagingControl3.CurrentPageIndex == 1)
                {
                    //drs = dt.Select("[rownumber] >= '" + PagingControl1.CurrentPageIndex + "'" + " AND [rownumber] <= '" + (PagingControl1.CurrentPageIndex * PagingControl1.PageSize) + "'");
                    record_start = PagingControl3.CurrentPageIndex-1;
                    record_end = (PagingControl3.CurrentPageIndex * PagingControl3.PageSize)-1;
                }
                else
                {
                    //drs = dt.Select("[rownumber] >= '" + (((PagingControl1.CurrentPageIndex - 1) * PagingControl1.PageSize) + 1) + "'" + " AND [rownumber] <= '" + (PagingControl1.CurrentPageIndex * PagingControl1.PageSize) + "'");
                    record_start = (((PagingControl3.CurrentPageIndex - 1) * PagingControl3.PageSize));
                    record_end = (PagingControl3.CurrentPageIndex * PagingControl3.PageSize)-1;
                }

                dtResult = dtResult.AsEnumerable().Where((row, index) => index >= record_start && index <= record_end).CopyToDataTable();

                dtResult.DefaultView.Sort = "Inv_ItemCode ASC,Pack_Id ASC";
                gvAdjust_Stock.DataSource = dtResult;
                gvAdjust_Stock.DataBind();
                gvAdjust_Stock.Visible = true;
                PagingControl3.Visible = true;
                //PagingControl3.RecordCount = (int)ds.Tables[1].Rows[0][0];
                //PagingControl3.RecordCount = dtResult.Rows.Count;
                PagingControl3.RecordCount = cnt_record;
                Show_Gv_SumExcel.Visible = true;


                lblCreateBy.Text = this.FirstName + " " + this.LastName;
                lblUpdateBy.Text = this.FirstName + " " + this.LastName;
                lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
                lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

                CopyDataTable();

            }
        }

        void PagingControl3_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            CopyDataTable();
            //BindDataAdjustSTock();
            BindDataAdjustSTock_RdbShowDiff();
        }

        private void CreateDataTable()
        {
            DataTable dtTemp = new DataTable();

            dtTemp.Columns.Add("Inv_ItemId", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Pack_Id", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Avg_Cost", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Inv_ItemCode", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Item_Search_Desc", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Pack_Description", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("OnHand_Qty", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Cnt_Qty", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Diff", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Adjust", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Qty_Amount", System.Type.GetType("System.String"));
            dtTemp.Columns.Add("Reason", System.Type.GetType("System.String"));

            SetStockPackageTable = dtTemp.Clone();
        }

        private void CopyDataTable()
        {
            DataTable dtTemp1 = new DataTable();

            dtTemp1.Columns.Add("Inv_ItemId", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Pack_Id", System.Type.GetType("System.String"));
            //dtTemp1.Columns.Add("Avg_Cost", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Avg_Cost", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Inv_ItemCode", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Item_Search_Desc", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Pack_Description", System.Type.GetType("System.String"));
            //dtTemp1.Columns.Add("OnHand_Qty", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("OnHand_Qty", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Cnt_Qty", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Diff", System.Type.GetType("System.String"));
            //dtTemp1.Columns.Add("Diff", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Adjust", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Qty_Amount", System.Type.GetType("System.String"));
            //dtTemp1.Columns.Add("Qty_Amount", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Reason", System.Type.GetType("System.String"));


            foreach (GridViewRow row in gvAdjust_Stock.Rows)
            {
                DataRow dr = dtTemp1.NewRow();


                HiddenField hd_ItemId = (HiddenField)row.FindControl("hd_ItemId");
                HiddenField hd_PackId = (HiddenField)row.FindControl("hd_PackId");
                HiddenField hd_AvgCost = (HiddenField)row.FindControl("hd_AvgCost");
                Label lbl_item_code = (Label)row.FindControl("lbl_item_code");
                Label lbl_item_name = (Label)row.FindControl("lbl_item_name");
                Label lbl_pack_name = (Label)row.FindControl("lbl_pack_name");
                Label lbl_onHand_Qty = (Label)row.FindControl("lbl_onHand_Qty");
                Label lbl_summary = (Label)row.FindControl("lbl_summary");
                TextBox txt_cnt = (TextBox)row.FindControl("txt_cnt");
                Label lbl_onHand_diff = (Label)row.FindControl("lbl_onHand_diff");
                TextBox txt_Reason = (TextBox)row.FindControl("txt_Reason");
                CheckBox cb_Adjust = (CheckBox)row.FindControl("cb_Adjust");


                dr["Inv_ItemId"] = hd_ItemId.Value;
                dr["Pack_Id"] = hd_PackId.Value;
                dr["Avg_Cost"] = hd_AvgCost.Value;
                dr["Inv_ItemCode"] = lbl_item_code.Text;
                dr["Item_Search_Desc"] = lbl_item_name.Text;
                dr["Pack_Description"] = lbl_pack_name.Text;
                //dr["OnHand_Qty"] = lbl_onHand_Qty.Text;
                dr["OnHand_Qty"] = lbl_onHand_Qty.Text.Replace(@",", string.Empty);
                //dr["Cnt_Qty"] = txt_cnt.Text;
                dr["Cnt_Qty"] = (txt_cnt.Text.Replace(@",", string.Empty) == "" ? "" : txt_cnt.Text.Replace(@",", string.Empty));
                //dr["Diff"] = txt_diif.Text;
                //dr["Cnt_Qty"] = lbl_onHand_cnt.Text;
                //dr["Diff"] = lbl_onHand_diff.Text;
                dr["Diff"] = lbl_onHand_diff.Text.Replace(@",", string.Empty);
                dr["Adjust"] = cb_Adjust.Checked;
                //dr["Qty_Amount"] = lbl_summary.Text;
                dr["Qty_Amount"] = lbl_summary.Text.Replace(@",", string.Empty);
                dr["Reason"] = txt_Reason.Text;
                //decimal amount = decimal.Parse(this.dtRequestItem.Rows[i]["Avg_Cost"].ToString()) * decimal.Parse(txt_qty.Text.Trim());

                //dr["Amount"] = amount;


                dtTemp1.Rows.Add(dr);

                //DataTable dtresults = SetStockPackageTable.DefaultView.ToTable(true, "Inv_ItemCode", "Pack_Description");
                //if (dtresults.Rows.Count > 0)
                //{
                //foreach (DataRow dr2 in SetStockPackageTable.Rows)
                //{
                //    if ((dr2["Inv_ItemCode"] == dtresults.Rows[0]["Inv_ItemCode"]) && (dr2["Pack_Description"] == dtresults.Rows[0]["Pack_Description"]))
                //    {
                //        SetStockPackageTable.Rows.Remove(dr2);
                //        //SetStockPackageTable.AcceptChanges();
                //    }
                //}
                if (SetStockPackageTable.Rows.Count > 0)
                {
                    for (int i = 0; i < SetStockPackageTable.Rows.Count; i++)
                    {
                        DataRow dr2 = SetStockPackageTable.Rows[i];
                        string Inv_ItemCode_temp = dr["Inv_ItemCode"].ToString();
                        string Inv_ItemCode = dr2["Inv_ItemCode"].ToString();
                        string Pack_Description_temp = dr["Pack_Description"].ToString();
                        string Pack_Description = dr2["Pack_Description"].ToString();
                        bool chkCode = false;
                        bool chkPack = false;
                        if (Inv_ItemCode_temp == Inv_ItemCode)
                        {
                            chkCode = true;
                        }

                        if (Pack_Description_temp == Pack_Description)
                        {
                            chkPack = true;
                        }

                        if ((dr["Inv_ItemCode"].ToString() == SetStockPackageTable.Rows[i]["Inv_ItemCode"].ToString()) && (dr["Pack_Description"].ToString() == SetStockPackageTable.Rows[i]["Pack_Description"].ToString()))
                        {
                            SetStockPackageTable.Rows.Remove(dr2);
                        }
                    }

                    SetStockPackageTable.AcceptChanges();
                }


            }

            SetStockPackageTable.Merge(dtTemp1);
            //DataTable dtresults2 = SetStockPackageTable.DefaultView.ToTable(true);

            //SetStockPackageTable.Merge(dtTemp1);
            //DataTable results = SetStockPackageTable.AsEnumerable().Distinct().CopyToDataTable();
            //DataTable dtresults = SetStockPackageTable.DefaultView.ToTable(true);
        }

        protected void RdbShowDiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            CopyDataTable();
            PagingControl3.CurrentPageIndex = 1;
            BindDataAdjustSTock_RdbShowDiff();
        }

        protected void txt_cnt_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)(((TextBox)sender).Parent.Parent));

            TextBox txt_cnt = (TextBox)sender;
            Label lbl_onHand_Qty = (Label)row.FindControl("lbl_onHand_Qty");
            Label lbl_onHand_diff = (Label)row.FindControl("lbl_onHand_diff");
            Label lbl_summary = (Label)row.FindControl("lbl_summary");
            CheckBox cb_Adjust = (CheckBox)row.FindControl("cb_Adjust");
            HiddenField hd_AvgCost = (HiddenField)row.FindControl("hd_AvgCost");

            string onHand_qty = lbl_onHand_Qty.Text.Replace(@",", string.Empty);
            //string cnt_qty = txt_cnt.Text.Replace(@",", string.Empty);
            //string cnt_qty = (txt_cnt.Text.Replace(@",", string.Empty) == "" ? "0" : txt_cnt.Text.Replace(@",", string.Empty));

            string cnt_qty = (txt_cnt.Text.Replace(@",", string.Empty) == "" ? "" : txt_cnt.Text.Replace(@",", string.Empty));


            //lbl_onHand_diff.Text = (Convert.ToInt32(lbl_onHand_Qty.Text == "" ? "0" : lbl_onHand_Qty.Text) - Convert.ToInt32(txt_cnt.Text == "" ? "0" : txt_cnt.Text)).ToString("###,##0");
            //lbl_onHand_diff.Text = (Convert.ToInt32(onHand_qty == "" ? "0" : onHand_qty) - Convert.ToInt32(cnt_qty == "" ? "0" : cnt_qty)).ToString("###,##0");
            if (cnt_qty != "")
            {
                lbl_onHand_diff.Text = (Convert.ToInt32(cnt_qty) - Convert.ToInt32(onHand_qty == "" ? "0" : onHand_qty)).ToString("###,##0");
            }
            else
            {
                lbl_onHand_diff.Text = "";
            }

            if (lbl_onHand_diff.Text == "0")
            {
                cb_Adjust.Enabled = false;
                if (cb_Adjust.Checked == true)
                {
                    cb_Adjust.Checked = false;
                }
            }
            else
            {
                cb_Adjust.Enabled = true;
            }

            if (txt_cnt.Text != "0" && txt_cnt.Text != "")
            {
                //lbl_summary.Text = (Convert.ToInt32(txt_cnt.Text == "" ? "0" : txt_cnt.Text) * Convert.ToDouble(hd_AvgCost.Value == "" ? "0" : hd_AvgCost.Value)).ToString("###,##0.0000");
                lbl_summary.Text = (Convert.ToInt32(cnt_qty == "" ? "0" : cnt_qty) * Convert.ToDouble(hd_AvgCost.Value == "" ? "0" : hd_AvgCost.Value)).ToString("###,##0.0000");
            }
            else if (txt_cnt.Text == "0")
            {
                lbl_summary.Text = "0.0000";
            }
            else
            { 
                lbl_summary.Text = "";
            }
            

        }



    }
}