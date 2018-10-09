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

namespace GPlus.PRPO
{
    public partial class WithdrawGoodsReport : Pagebase
    {
        DatabaseHelper dbHelper;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "503";
                ReportViewer.Visible = false;

                InitializeUI();
            }

            dbHelper = new DatabaseHelper();
            ReportViewer.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
        }

        private void InitializeSearchDivDep()
        {
            // กำหนดค่าเริ่มต้นให้เป็น ชื่อฝ่าย/ทีมงาน ตามที่ผู้ใช้ Log-in
            DataTable dt = new OrgStructureDAO().GetOrgStructureDivDep(Convert.ToInt32(OrgID));

            string divCode = dt.Rows[0]["Div_Code"].ToString();
            string depCode = dt.Rows[0]["Dep_Code"].ToString();
            string divName = dt.Rows[0]["DivName"].ToString();
            string depName = dt.Rows[0]["DepName"].ToString();

            ItemOrgStructCtrl2.DivCode = divCode;
            ItemOrgStructCtrl2.DepCode = depCode;
            ItemOrgStructCtrl2.DivName = divName;
            ItemOrgStructCtrl2.DepName = depName;

            // กำหนดสิทธิ์การใช้งาน Search Helper 
            DataTable dtUser = new UserDAO().GetUserGroupUser(UserID);
            DataRow[] rows = dtUser.Select("UserGroup_ID IN (4,7)");
            if (rows.Length == 0)
            {
                ImageButton btnDep = ItemOrgStructCtrl2.FindControl("btnDep") as ImageButton;
                btnDep.Visible = false;
            }
        }
        private void InitializeCategoryDropDownList()
        {
            DataTable dtCategory = new DataAccess.CategoryDAO().GetCategoryAll();

            ddlCategory.DataSource = dtCategory;
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }
        private void InitializeUI()
        {
            InitializeSearchDivDep();
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
                        "withdrawGoods",
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
                            "withdrawGoods",
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
                        "withdrawGoods",
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
                        "withdrawGoods",
                        "alert('กรุณาระบุวันที่สิ้นสุด');",
                        true
                    );

                    return false;
                }
            }
            return true;
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            string orgStrucId = e.Parameters["OrgStrucId"].Values[0];
            string cateId = e.Parameters["CateId"].Values[0];
            string divCode = e.Parameters["DivCode"].Values[0];
            string depCode = e.Parameters["DepCode"].Values[0];
            string catName = e.Parameters["CateName"].Values[0];
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            string year = tbYear.Text.Trim();

            SQLParameterList sqlParams = new SQLParameterList();

            sqlParams.AddStringField("OrgStrucId", orgStrucId);
            sqlParams.AddStringField("CateId", cateId);
            sqlParams.AddStringField("DivCode", divCode);
            sqlParams.AddStringField("DepCode", depCode);
            sqlParams.AddIntegerField("Month", month);
            sqlParams.AddStringField("Year", year);
            sqlParams.AddStringField("FlagPayReq", rblPayReq.SelectedValue);
            sqlParams.AddStringField("FlagDate", rblDateType.SelectedValue);

            DataTable dt = dbHelper.ExecuteDataTable("sp_Inv_WithdrawGoods_Summary_Report2", sqlParams.GetSqlParameterList());

            ReportDataSource rds = new ReportDataSource("WithdrawGoodsSummaryReport2", dt);

            e.DataSources.Add(rds);
        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            if (ValidateDate() && ValidateYear())
            {
                ReportViewer.Visible = true;
                BindData();
                //ReportViewer.Visible = true;
            }
            else
                ReportViewer.Visible = false;
        }

        protected void bCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Request/WithdrawGoodsReport.aspx");
        }

        private void BindData()
        {
            DataTable dt = GetWithdrawGoodsReport();

            ReportDataSource rds = new ReportDataSource("WithdrawGoodsReport2", dt);
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
                reportParams.Add(new ReportParameter("Month", ""));
            }

            ReportViewer.LocalReport.SetParameters(reportParams);
            ReportViewer.LocalReport.DataSources.Add(rds);
            ReportViewer.LocalReport.Refresh();
        }

        private DataTable GetWithdrawGoodsReport()
        {
            GPlus.DataAccess.StockDAO st = new DataAccess.StockDAO();

            SQLParameterList sqlParams = new SQLParameterList();

            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(tbYear.Text.Trim()) - 543;

            sqlParams.AddStringField("DivCode", ItemOrgStructCtrl2.DivCode);
            sqlParams.AddStringField("DepCode", ItemOrgStructCtrl2.DepCode);
            sqlParams.AddStringField("StartDate", ccStartDate.Text);
            sqlParams.AddStringField("EndDate", ccEndDate.Text);
            sqlParams.AddStringField("Month", ddlMonth.SelectedValue);
            sqlParams.AddStringField("Year", year.ToString());
            sqlParams.AddStringField("CateID", ddlCategory.SelectedValue);
            sqlParams.AddStringField("ItemCode", tbItemCode.Text);
            sqlParams.AddStringField("ItemName", tbItemName.Text);
            sqlParams.AddStringField("FlagPayReq", rblPayReq.SelectedValue);    // จ่าย = "P", เบิก = "R"
            sqlParams.AddStringField("FlagDate", rblDateType.SelectedValue);    // วัน = "D", เดือน = "M"

            return st.GetWithdrawGoods(sqlParams.GetSqlParameterList()).Tables[0];
        }

        private string ConvertToThaiDate(string date)
        {
            int index = date.LastIndexOf('/');
            string year = date.Substring(index + 1);

            return date.Substring(0, index + 1) + (Convert.ToInt32(year) + 543).ToString();
        }
    }
}