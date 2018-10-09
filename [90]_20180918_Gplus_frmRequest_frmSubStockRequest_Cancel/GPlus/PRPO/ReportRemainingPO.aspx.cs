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
    public partial class ReportRemainingPO : Pagebase
    {
        public DataTable ReportRemainingPOPackageTable
        {
            get
            {
                return (DataTable)Session["ReportRemainingPO"];
            }
            set
            {
                Session["ReportRemainingPO"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "309";
                //BindDropdown();
                ReportViewer1.Visible = false;

            }

        }




        //private void BindDropdown()
        //{
        //    DataTable dt = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
        //    ddlSupplierNameSearch.DataSource = dt;
        //    ddlSupplierNameSearch.DataTextField = "Supplier_Name";
        //    ddlSupplierNameSearch.DataValueField = "Supplier_ID";
        //    ddlSupplierNameSearch.DataBind();
        //    ddlSupplierNameSearch.Items.Insert(0, new ListItem("เลือกชื่อ", ""));

        //}



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (dtCreatePOStart.Text == "")
            {
                ShowMessageBox("กรุณาระบุวันที่เริ่มของการสั่งซื้อ");
                return;
            }
            if (dtCreatePOStop.Text == "")
            {
                ShowMessageBox("กรุณาระบุวันที่สิ้นสุดของการสั่งซื้อ");
                return;
            }
            BindData();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {

            dtCreatePOStart.Text = "";
            dtCreatePOStop.Text = "";
            txtSupplierCodeSearch.Text = "";
            //ddlSupplierNameSearch.SelectedIndex = 0;
            txtSupplierName.Text = "";
            txtItemId.Text = "";
            txtItemName.Text = "";
        }

        private void BindData()
        {
            //ShowMessageBox(dtCreatePOStart.Text+ " , "+dtCreatePOStop.Text);
            ReportViewer1.Visible = true;

            GPlus.DataAccess.SupplierDAO st = new DataAccess.SupplierDAO();

            //DataSet ds = st.GetRemainingPOReport(dtCreatePOStart.Text, dtCreatePOStop.Text, txtSupplierCodeSearch.Text, ddlSupplierNameSearch.SelectedValue);
            DataSet ds = st.GetRemainingPOReport(dtCreatePOStart.Text, dtCreatePOStop.Text, txtSupplierCodeSearch.Text, txtSupplierName.Text, txtItemId.Text, txtItemName.Text);

            ReportParameterCollection reportParams = new ReportParameterCollection();

            string start_date = "";
            string stop_date = "";

            string[] str_date = dtCreatePOStart.Text.Split('/');
            start_date  = str_date[0] + "/" + str_date[1] + "/" + (Convert.ToInt32(str_date[2] == "" ? "0" : str_date[2]) + 543).ToString();

            string[] end_date = dtCreatePOStop.Text.Split('/');
            stop_date = end_date[0] + "/" + end_date[1] + "/" + (Convert.ToInt32(end_date[2] == "" ? "0" : end_date[2]) + 543).ToString();

            reportParams.Add(new ReportParameter("Start_Date", start_date));
            reportParams.Add(new ReportParameter("Stop_Date", stop_date));
            
            ReportDataSource rds1 = new ReportDataSource("RemainingPODataSet", ds.Tables[0]);
            this.ReportViewer1.LocalReport.SetParameters(reportParams);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds1);
            if (ds.Tables[0].Rows.Count == 0)
            {
                ReportViewer1.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            this.ReportViewer1.LocalReport.Refresh();


        }

        
    }
}