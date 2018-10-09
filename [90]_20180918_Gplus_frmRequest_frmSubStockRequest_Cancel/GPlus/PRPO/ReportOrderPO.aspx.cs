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
    public partial class ReportOrderPO : Pagebase
    {
        public DataTable ReportOrderPOPackageTable
        {
            get
            {
                return (DataTable)Session["ReportOrderPO"];
            }
            set
            {
                Session["ReportOrderPO"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "311";
                BindDropdown();
                ReportViewer1.Visible = false;

            }
        }

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
            ddlMaterialTypeSearch.SelectedIndex = 0;
            SupplierSearch.DivName = "";
            txtItemId.Text= "";
            txtItemName.Text = "";
        }

        private void BindData()
        {
            //ShowMessageBox(dtCreatePOStart.Text+ " , "+dtCreatePOStop.Text);
            ReportViewer1.Visible = true;

            GPlus.DataAccess.SupplierDAO st = new DataAccess.SupplierDAO();

            DataSet ds = st.GetOrderPOReport(dtCreatePOStart.Text, dtCreatePOStop.Text,ddlMaterialTypeSearch.SelectedValue, SupplierSearch.DivName, txtItemId.Text, txtItemName.Text);
           
            
            ReportParameterCollection reportParams = new ReportParameterCollection();

            string start_date = "";
            string stop_date = "";
            string Supplier_Name = "";
            string Cat_Name = "";
            
            
            string[] str_date = dtCreatePOStart.Text.Split('/');
            start_date = str_date[0] + "/" + str_date[1] + "/" + (Convert.ToInt32(str_date[2] == "" ? "0" : str_date[2]) + 543).ToString();

            string[] end_date = dtCreatePOStop.Text.Split('/');
            stop_date = end_date[0] + "/" + end_date[1] + "/" + (Convert.ToInt32(end_date[2] == "" ? "0" : end_date[2]) + 543).ToString();

            if (SupplierSearch.DivName == "")
            {
                Supplier_Name = SupplierSearch.DivName;
            }
            else
            {
                Supplier_Name = "Supplier " + "  "+ SupplierSearch.DivName;
            }
            Cat_Name = ddlMaterialTypeSearch.SelectedItem.Text;

            reportParams.Add(new ReportParameter("Start_Date", start_date));
            reportParams.Add(new ReportParameter("Stop_Date", stop_date));
            reportParams.Add(new ReportParameter("Supplier_Name", Supplier_Name));
            reportParams.Add(new ReportParameter("Cat_Name", Cat_Name));
           
            ReportDataSource rds1 = new ReportDataSource("ReportOrderDataSet", ds.Tables[0]);
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
        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];
            ddlMaterialTypeSearch.Items.Clear();
            ddlMaterialTypeSearch.Items.Insert(0, new ListItem("ทั้งหมด", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlMaterialTypeSearch.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));

            }

        }
        
    }
}