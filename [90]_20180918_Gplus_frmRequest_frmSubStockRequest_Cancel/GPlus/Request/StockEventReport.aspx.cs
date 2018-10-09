using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Diagnostics;
using GPlus.DataAccess;

namespace GPlus.Request
{
    public partial class StockEventReport : Pagebase
    {
        DatabaseHelper dbHelper;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "504";
                ReportViewerSummary.Visible = false;
                ReportViewer.Visible = false;
                gv.Visible = false;

                InitializeUI();
            }
            //BindGridview();
            dbHelper = new DatabaseHelper();
            //ReportViewerSummary.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        
        }

        public string Script
        {
            get
            {
                if (ViewState["Script"] == null)
                    ViewState["Script"] = "";

                return ViewState["Script"].ToString();
            }
            set
            {
                ViewState["Script"] = value;
            }
        }

        //private void InitializeSearchDivDep()
        //{
        //    // กำหนดค่าเริ่มต้นให้เป็น ชื่อฝ่าย/ทีมงาน ตามที่ผู้ใช้ Log-in
        //    DataTable dt = new OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt32(OrgID));

        //    string divCode = dt.Rows[0]["Div_Code"].ToString();
        //    string depCode = dt.Rows[0]["Dep_Code"].ToString();
        //    string divName = dt.Rows[0]["DivName"].ToString();
        //    string depName = dt.Rows[0]["DepName"].ToString();

        //    ItemOrgStructCtrl2.DivCode = divCode;
        //    ItemOrgStructCtrl2.DepCode = depCode;
        //    ItemOrgStructCtrl2.DivName = divName;
        //    ItemOrgStructCtrl2.DepName = depName;
        //    ItemOrgStructCtrl2.OrgStructID = OrgID;

        //    // กำหนดสิทธิ์การใช้งาน Search Helper 
        //    //DataTable dtUser = new UserDAO().GetUserGroupUser(UserID);
        //    //DataRow[] rows = dtUser.Select("UserGroup_ID IN (4,7)");
        //    //if (rows.Length == 0)
        //    //{
        //    //    ImageButton btnDep = ItemOrgStructCtrl2.FindControl("btnDep") as ImageButton;
        //    //    btnDep.Visible = false;
        //    //}
        //}

        private void InitializeCategoryDropDownList()
        {
            DataTable dtCategory = new DataAccess.CategoryDAO().GetCategoryAll();

            ddlCategory.DataSource = dtCategory;
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            ddlCategory.SelectedValue = "13";
            BindGridview();
        }



        private void BindGridview()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemSearchByCateID(tbItemCode.Text, tbItemName.Text, ddlCategory.SelectedValue,
                1, 1000, this.SortColumn, this.SortOrder);

            gvItem.DataSource = ds.Tables[0];
            gvItem.DataBind();
            gv.Visible = true;
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbItemCode.Text == "" && tbItemName.Text == "" && ddlCategory.SelectedValue != "")
            {
                BindGridview();
            }
            else
            {
                gv.Visible = false;
            }
        }

        protected void btnRefreshGv_Click(object sender, EventArgs e)
        {
            if (tbItemCode.Text == "" && tbItemName.Text == "" && ddlCategory.SelectedValue != "")
            {
                BindGridview();
            }
            else
            {
                gv.Visible = false;
            }
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.Script = "function CheckAllP(state){";
                //((CheckBox)e.Row.FindControl("chkDH")).Attributes.Add("onclick", "CheckAllP(this.checked);");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                
                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                HiddenField hdUnitID = (HiddenField)e.Row.FindControl("hdUnitID");
                hdID.Value = drv["Inv_ItemID"].ToString();
                hdUnitID.Value = drv["Pack_ID"].ToString();
                CheckBox chkD = (CheckBox)e.Row.FindControl("chkD");
                this.Script += "document.getElementById('" + chkD.ClientID + "').checked = state;";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                this.Script += "}";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chk", this.Script, true);
            }
        }

        private void InitializeUI()
        {
            //InitializeSearchDivDep();
            InitializeCategoryDropDownList();
            InitializeMonthYear();
        }
        
        /// <summary>
        ///     กำหนดค่าเดือนกับปี พ.ศ. โดยกำหนดให้ใช้เดือนก่อนหน้าปัจจุบัน
        ///     หากเดือนปัจจุบันเป็นเดือน "มกราคม" 
        ///     ให้กำหนดเป็นเดือน "ธันวาคม" ของปีก่อนหน้า
        /// </summary>
        private void InitializeMonthYear()
        {
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year + 543;

            const int JANUARY = 1;
            
            if (currentMonth == JANUARY)
            {
                currentYear = currentYear - 1;
                currentMonth = 12;
            }
            else
            {
                currentMonth = currentMonth - 1;
            }

            ddlMonth.SelectedValue = currentMonth.ToString();
            tbYear.Text = currentYear.ToString();
        }

        private bool ValidateYear()
        {
            if (rblDateType.SelectedValue == "M")
            {
                if (tbYear.Text == "")
                {
                    ScriptManager.RegisterStartupScript
                    (
                        this,
                        GetType(),
                        "stockEventReport",
                        "alert('กรุณาระบุปี');",
                        true
                    );
                    return false;
                }
                else
                {
                    try
                    {
                        Convert.ToInt32(tbYear.Text);
                    }
                    catch (FormatException)
                    {
                        ScriptManager.RegisterStartupScript
                        (
                            this,
                            GetType(),
                            "stockEventReport",
                            "alert('กรุณาระบุปีให้ถูกต้อง');",
                            true
                        );
                        return false;
                    }
                }
            }

            return true;
        }
        private bool ValidateDate()
        {
            if (rblDateType.SelectedValue == "D")
            {
                if (ccStartDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript
                    (
                        this,
                        GetType(),
                        "stockEventReport",
                        "alert('กรุณาระบุวันที่เริ่มต้น');",
                        true
                    );

                    return false;
                }

                if (ccEndDate.Text == "")
                {
                    ScriptManager.RegisterStartupScript
                    (
                        this,
                        GetType(),
                        "stockEventReport",
                        "alert('กรุณาระบุวันที่สิ้นสุด');",
                        true
                    );

                    return false;
                }
            }
            return true;
        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            if (ValidateDate() && ValidateYear())
            {
                BindData();
                //ReportViewerSummary.Visible = true;
            }
            else
                ReportViewerSummary.Visible = false;
        }

        protected void bCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Stock/StockEventReport.aspx");
        }

        

        private void BindData()
        {
            //string[,] arrItem = new string[,] { };
            List<Tuple<string, string>> arrItem = new List<Tuple<string, string>>();
            string whereItem = "";

            if (ItemOrgStructCtrl2.OrgStructID != "" && chkSummary.Checked == true)
            {
                ShowMessageBox("กรณีที่เลือกชื่อฝ่าย/ทีม ไม่สามารถออกรายงานพิมพ์สรุปได้");
                return;
            }

            if (tbItemCode.Text == "" && tbItemName.Text == "" && ddlCategory.SelectedValue == "")
            {
                ShowMessageBox("กรุณาเลือกรายการสินค้า");
                return;
            }
            else if (tbItemCode.Text == "" && tbItemName.Text == "")
            {
                if (gvItem.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvItem.Rows)
                    {
                        CheckBox chkD = (CheckBox)row.FindControl("chkD");
                        HiddenField hd_ItemId = (HiddenField)row.FindControl("hdID");
                        HiddenField hd_PackId = (HiddenField)row.FindControl("hdUnitID");

                        if (chkD.Checked == true) // ถ้ามีการติ๊กเลือก record นั้น
                        {
                            arrItem.Add(Tuple.Create(hd_ItemId.Value, hd_PackId.Value));
                        }
                    }

                    if (arrItem.Count <= 0)
                    {
                        ShowMessageBox("กรุณาเลือกรายการสินค้า");
                        return;
                    }
                }
                else
                {
                    ShowMessageBox("กรุณาเลือกรายการสินค้า");
                    return;
                }

                if (arrItem.Count > 0)
                {
                    // นำค่าใส่ใน where 
                    foreach (Tuple<string, string> t in arrItem)
                    {
                        string itemID = t.Item1;
                        string packID = t.Item2;

                        if (whereItem == "")
                        {
                            whereItem = whereItem + "( ( s.[Inv_ItemID] = " + itemID + " AND s.[Pack_ID] = " + packID + " )";
                        }
                        else
                        {
                            whereItem = whereItem + " OR " + "( s.[Inv_ItemID] = " + itemID + " AND s.[Pack_ID] = " + packID + " )";
                        }
                    }

                    if (whereItem != "")
                    {
                        whereItem = whereItem + " )";
                    }
                
                }


            }

            bool showSummary = chkSummary.Checked;

            // กรณีที่มีการใส่ค่ารหัส หรือ รายการสินค้า จะต้อง clear ค่าที่เลือกมาจาก Gridview
            if (tbItemCode.Text != "" || tbItemName.Text != "")
            {
                whereItem = "";
            }

            if (showSummary)
            {
                DataTable dtSum = GetSummaryStockEventReport(whereItem);
                if (dtSum.Rows.Count > 0)
                {
                    ReportDataSource rds = new ReportDataSource("StockEventReportDataSet", dtSum);
                    ReportViewerSummary.LocalReport.DataSources.Clear();

                    ReportParameterCollection reportParams = new ReportParameterCollection();

                    if (rblDateType.SelectedValue == "M")
                    {
                        reportParams.Add(new ReportParameter("DateOrMonth", "ประจำเดือน " + ddlMonth.SelectedItem.Text + " พ.ศ. " + tbYear.Text));
                        reportParams.Add(new ReportParameter("Month", ddlMonth.SelectedItem.Text));
                    }
                    else if (rblDateType.SelectedValue == "D")
                    {
                        reportParams.Add(new ReportParameter("DateOrMonth", "ประจำวันที่ " + ConvertToThaiDate(ccStartDate.Text) + " - " + ConvertToThaiDate(ccEndDate.Text)));
                    }

                    ReportViewerSummary.LocalReport.SetParameters(reportParams);
                    ReportViewerSummary.LocalReport.DataSources.Add(rds);
                    ReportViewerSummary.LocalReport.Refresh();
                    ReportViewerSummary.Visible = true;
                    ReportViewer.Visible = false;
                }
                else
                {
                    ShowMessageBox("ไม่พบข้อมูล");
                    ReportViewer.Visible = false;
                    ReportViewerSummary.Visible = false;
                }
            }
            else
            {
                DataTable dt = GetStockEventReport(whereItem);
                if (dt.Rows.Count > 0)
                {
                    ReportDataSource rds = new ReportDataSource("StockEventReportDataSet", dt);
                    ReportViewer.LocalReport.DataSources.Clear();

                    ReportParameterCollection reportParams = new ReportParameterCollection();

                    if (rblDateType.SelectedValue == "M")
                    {
                        reportParams.Add(new ReportParameter("DateOrMonth", "ประจำเดือน " + ddlMonth.SelectedItem.Text + " พ.ศ. " + tbYear.Text));
                        reportParams.Add(new ReportParameter("Month", ddlMonth.SelectedItem.Text));
                    }
                    else if (rblDateType.SelectedValue == "D")
                    {
                        reportParams.Add(new ReportParameter("DateOrMonth", "ประจำวันที่ " + ConvertToThaiDate(ccStartDate.Text) + " - " + ConvertToThaiDate(ccEndDate.Text)));
                    }

                    ReportViewer.LocalReport.SetParameters(reportParams);
                    ReportViewer.LocalReport.DataSources.Add(rds);
                    ReportViewer.LocalReport.Refresh();
                    ReportViewer.Visible = true;
                    ReportViewerSummary.Visible = false;
                }
                else
                {
                    ShowMessageBox("ไม่พบข้อมูล");
                    ReportViewer.Visible = false;
                    ReportViewerSummary.Visible = false;
                }
            }
  
        
        }

        private DataTable GetSummaryStockEventReport(string whereItem)
        {
            GPlus.DataAccess.StockDAO st = new DataAccess.StockDAO();

            SQLParameterList sqlParams = new SQLParameterList();

            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(tbYear.Text.Trim()) - 543;

            sqlParams.AddStringField("StartDate", ccStartDate.Text);
            sqlParams.AddStringField("EndDate", ccEndDate.Text);
            sqlParams.AddStringField("Month", ddlMonth.SelectedValue);
            sqlParams.AddStringField("Year", year.ToString());
            sqlParams.AddStringField("CateID", ddlCategory.SelectedValue);
            sqlParams.AddStringField("ItemCode", tbItemCode.Text);
            sqlParams.AddStringField("ItemName", tbItemName.Text);
            sqlParams.AddStringField("FlagDate", rblDateType.SelectedValue);    // วัน = "D", เดือน = "M"
            sqlParams.AddStringField("WhereItem", whereItem); 

            return st.GetSummaryStockEvent(sqlParams.GetSqlParameterList()).Tables[0];
        }

        private DataTable GetStockEventReport(string whereItem)
        {
            GPlus.DataAccess.StockDAO st = new DataAccess.StockDAO();

            SQLParameterList sqlParams = new SQLParameterList();

            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(tbYear.Text.Trim()) - 543;

            sqlParams.AddStringField("OrgStruct", ItemOrgStructCtrl2.OrgStructID);
            sqlParams.AddStringField("StartDate", ccStartDate.Text);
            sqlParams.AddStringField("EndDate", ccEndDate.Text);
            sqlParams.AddStringField("Month", ddlMonth.SelectedValue);
            sqlParams.AddStringField("Year", year.ToString());
            sqlParams.AddStringField("CateID", ddlCategory.SelectedValue);
            sqlParams.AddStringField("ItemCode", tbItemCode.Text);
            sqlParams.AddStringField("ItemName", tbItemName.Text);
            sqlParams.AddStringField("FlagDate", rblDateType.SelectedValue);    // วัน = "D", เดือน = "M"
            sqlParams.AddStringField("WhereItem", whereItem); 

            return st.GetStockEvent(sqlParams.GetSqlParameterList()).Tables[0];
        }

        private string ConvertToThaiDate(string date)
        {
            int index = date.LastIndexOf('/');
            string year = date.Substring(index + 1);

            return date.Substring(0, index + 1) + (Convert.ToInt32(year) + 543).ToString();
        }
    }
}