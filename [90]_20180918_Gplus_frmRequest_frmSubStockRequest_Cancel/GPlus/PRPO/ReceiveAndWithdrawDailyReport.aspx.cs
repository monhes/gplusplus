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
    public partial class ReceiveAndWithdrawDailyReport : Pagebase
    {
        public DataTable ReceiveAndWithdrawDailyReportPackageTable
        {
            get
            {
                return (DataTable)Session["ReceiveAndWithdrawDailyReport"];
            }
            set
            {
                Session["ReceiveAndWithdrawDailyReport"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "415";
                BindDropdown();
                ReportViewer1.Visible = false;
                ReportViewer2.Visible = false;

            }

        }


        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStock("Main01", "", "1", 1, 1000, "", "").Tables[0];
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

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
        }

        private void BindSubMaterialType()
        {
            if (ddlMaterialType.SelectedIndex > 0)
            {
                ddlSubMaterialType.DataSource = new DataAccess.CategoryDAO().GetSubCate("", ddlMaterialType.SelectedValue, "",
                    "1", 1, 1000, "", "").Tables[0];
                ddlSubMaterialType.DataTextField = "SubCate_Name";
                ddlSubMaterialType.DataValueField = "SubCate_ID";
                ddlSubMaterialType.DataBind();
                ddlSubMaterialType.Items.Insert(0, new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
            }
            else
                ddlSubMaterialType.Items.Clear();
        }

       

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            if (dtStart.Text == "")
            {
                ShowMessageBox("กรูณาระบุวันที่เริ่มต้น");
                return;
            }
            else if (dtStop.Text == "")
            {
                ShowMessageBox("กรูณาระบุวันที่สิ้นสุด");
                return;
            }
            BindData();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {
           
            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            dtStart.Text = "";
            dtStop.Text = "";
            txtItemCode.Text = "";
            txtItemName.Text = "";
            rdbType.SelectedIndex = 0;
            
        }

        private void BindData()
        {

            if (rdbType.SelectedIndex == 0) //รายงานการรับ
            {
                DataTable dt = new DataAccess.ReceiveStockDAO().GetReceiveDailyReport(ddlStock.SelectedValue, dtStart.Text, dtStop.Text, ddlMaterialType.SelectedValue, ddlSubMaterialType.SelectedValue, txtItemCode.Text, txtItemName.Text);
                ReportDataSource rds1 = new ReportDataSource("ReceiveDailyDataSet", dt);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds1);
                if (dt.Rows.Count == 0)
                {
                    ReportViewer1.Visible = false;
                    ShowMessageBox("ไม่พบข้อมูล");
                }
                else if (dt.Rows.Count > 0)
                {
                    ReportViewer1.Visible = true;
                    dt.Columns.Add("Start_Date");
                    string[] dtStartTemp = dtStart.Text.Split('/');

                    dt.Rows[0]["Start_Date"] = dtStartTemp[0] + "/" + dtStartTemp[1] + "/" + (Int32.Parse(dtStartTemp[2]) + 543).ToString();

                    dt.Columns.Add("End_Date");
                    string[] dtEndTemp = dtStop.Text.Split('/');
                    dt.Rows[0]["End_Date"] = dtEndTemp[0] + "/" + dtEndTemp[1] + "/" + (Int32.Parse(dtEndTemp[2]) + 543).ToString();

                }
                this.ReportViewer1.LocalReport.Refresh();
            }

            else //รายงานการเบิก
            {
                DataTable dt = new DataAccess.ReceiveStockDAO().GetWithdrawDailyReport(ddlStock.SelectedValue, dtStart.Text, dtStop.Text, ddlMaterialType.SelectedValue, ddlSubMaterialType.SelectedValue, txtItemCode.Text, txtItemName.Text);
                ReportDataSource rds1 = new ReportDataSource("WithdrawDailyDataSet", dt);
                this.ReportViewer2.LocalReport.DataSources.Clear();
                this.ReportViewer2.LocalReport.DataSources.Add(rds1);
                if (dt.Rows.Count == 0)
                {
                    ReportViewer2.Visible = false;
                    ShowMessageBox("ไม่พบข้อมูล");
                }
                else if (dt.Rows.Count > 0)
                {
                    ReportViewer2.Visible = true;
                    dt.Columns.Add("Start_Date");
                    string[] dtStartTemp = dtStart.Text.Split('/');

                    dt.Rows[0]["Start_Date"] = dtStartTemp[0] + "/" + dtStartTemp[1] + "/" + (Int32.Parse(dtStartTemp[2]) + 543).ToString();

                    dt.Columns.Add("End_Date");
                    string[] dtEndTemp = dtStop.Text.Split('/');
                    dt.Rows[0]["End_Date"] = dtEndTemp[0] + "/" + dtEndTemp[1] + "/" + (Int32.Parse(dtEndTemp[2]) + 543).ToString();

                }
                this.ReportViewer2.LocalReport.Refresh();
            
            }

        }

        protected void rdbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReportViewer1.Visible = false;
            ReportViewer2.Visible = false;
        }

        
    }
}