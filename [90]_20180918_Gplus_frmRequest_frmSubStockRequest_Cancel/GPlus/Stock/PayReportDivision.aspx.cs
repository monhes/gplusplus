using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace GPlus.PRPO
{
    public partial class PayReportDivision : Pagebase
    {
        public DataTable PayReportDivisionPackageTable
        {
            get
            {
                return (DataTable)Session["PayReportDivision"];
            }
            set
            {
                Session["PayReportDivision"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "418";
                BindDropdown();
                ReportViewer1.Visible = false;
                PopulateYearList();

            }

        }

        protected void PopulateYearList()
        {
            int year;
            for (year = (DateTime.Now.Year - 10) + 543; year <= (DateTime.Now.Year) + 543; year++)
            {
                ddlYearStart.Items.Add(System.Convert.ToString(year));
            }
            ddlYearStart.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
        }


        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            //ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            ddlMaterialType.DataSource = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
            ddlMaterialType.DataTextField = "Cat_Name";
            ddlMaterialType.DataValueField = "Cate_ID";
            ddlMaterialType.DataBind();
            ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindData();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {
            ddlStock.SelectedIndex = 0;
            ddlMaterialType.SelectedIndex = 0;
            ddlMonthStart.SelectedIndex = 0;
            ddlYearStart.SelectedValue = System.Convert.ToString(DateTime.Now.Year + 543);
            
        }

        private void BindData()
        {

            string MonthYearStart = "";

            try
            {
                MonthYearStart = ddlYearStart.SelectedValue + ddlMonthStart.SelectedValue;
            }
            catch 
            {
                MonthYearStart = "";
            }

            DataTable dt = new DataAccess.StockDAO().GetPayReportDivision(ddlStock.SelectedValue,ddlMaterialType.SelectedValue,MonthYearStart);
            ReportDataSource rds1 = new ReportDataSource("PayReportDivisionDataSet", dt);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds1);

            dt.Columns.Add("BalType_total");

            if (dt.Rows.Count == 0)
            {
                ReportViewer1.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            else if (dt.Rows.Count > 0)
            {
                ReportViewer1.Visible = true;
                
                string cate_code = "";
                string type_code = "";
                decimal balType_total = 0;
                decimal payType_total = 0;
                decimal bal = 0;
                decimal rec = 0;

                dt.Columns.Add("MonthYear");
                dt.Rows[0]["MonthYear"] = ddlMonthStart.SelectedItem.Text + "  " + ddlYearStart.SelectedItem.Text;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((dt.Rows[i]["Cate_Code"].ToString() != cate_code) || (dt.Rows[i]["Type_Code"].ToString() != type_code))
                    {
                        cate_code = dt.Rows[i]["Cate_Code"].ToString();
                        type_code = dt.Rows[i]["Type_Code"].ToString();

                        try
                        {
                            payType_total = Convert.ToDecimal(dt.Compute("Sum(Pay_Amt)", "Cate_Code = " + cate_code + " AND Type_Code = " + type_code));
                        }
                        catch
                        {
                            payType_total = 0;
                        }

                        bal = Convert.ToDecimal(dt.Rows[i]["Bal_Amt"].ToString());
                        rec = Convert.ToDecimal(dt.Rows[i]["Rec_Amt"].ToString());

                        balType_total = (bal + rec) - payType_total;

                        dt.Rows[i]["BalType_total"] = balType_total.ToString("#,##0.0000");

                    }
                    else
                    {
                        dt.Rows[i]["Type_Name"] = "";
                        dt.Rows[i]["Bal_Amt"] = DBNull.Value;
                        dt.Rows[i]["Rec_Amt"] = DBNull.Value;
                        dt.AcceptChanges();
                    }
                }
            }
            //else if (dt.Rows.Count > 0)
            //{
            //    ReportViewer1.Visible = true;

            //    dt.Columns.Add("StartDate");
            //    if (dtStart.Text != "")
            //    {
            //        string[] dtStartTemp = dtStart.Text.Split('/');
            //        dt.Rows[0]["StartDate"] = dtStartTemp[0] + "/" + dtStartTemp[1] + "/" + (Int32.Parse(dtStartTemp[2]) + 543).ToString();
            //    }

            //    dt.Columns.Add("EndDate");
            //    if (dtStop.Text != "")
            //    {
            //        string[] dtEndTemp = dtStop.Text.Split('/');
            //        dt.Rows[0]["EndDate"] = dtEndTemp[0] + "/" + dtEndTemp[1] + "/" + (Int32.Parse(dtEndTemp[2]) + 543).ToString();
            //    }

            //    dt.Columns.Add("CateName");
            //    if (ddlMaterialType.SelectedIndex != 0)
            //    {
            //        dt.Rows[0]["CateName"] = ddlMaterialType.SelectedItem.Text;
            //    }

            //    dt.Columns.Add("FormType");
            //    if (rdbType.SelectedIndex == 0)
            //    {
            //        dt.Rows[0]["FormType"] = "รายการรับสินค้าเข้ากรณีอื่นๆ " + ddlStock.SelectedItem.Text;
            //    }
            //    else if (rdbType.SelectedIndex == 1)
            //    {
            //        dt.Rows[0]["FormType"] = "รายการจ่ายออกสินค้ากรณีอื่นๆ " + ddlStock.SelectedItem.Text;
            //    }
            //    else
            //    {
            //        dt.Rows[0]["FormType"] = "รายการ Adjust Stock " + ddlStock.SelectedItem.Text;
            //    }

            //    dt.Columns.Add("RowColor");

            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        if (dt.Rows[i]["Row"].ToString() != "")
            //        {
            //            dt.Rows[i]["RowColor"] = "Plum";
            //        }
            //        else
            //        {
            //            dt.Rows[i]["RowColor"] = "Transparent";
            //        }
            //    }

            //}

            this.ReportViewer1.LocalReport.Refresh();
        }


        
    }
}