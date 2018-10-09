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
    public partial class ReportStockOnHand : Pagebase
    {
        public DataTable ReportStockOnHandPackageTable
        {
            get
            {
                return (DataTable)Session["ReportStockOnHand"];
            }
            set
            {
                Session["ReportStockOnHand"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "416";
                BindDropdown();
                ReportViewer1.Visible = false;

            }
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

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
            BindData();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {
           
            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            ddlStock.SelectedIndex = 0;
            txtItemCode.Text = "";
            txtItemName.Text = "";
            rdbStatus.SelectedIndex = 0;
            rdbOnHand.SelectedIndex = 0;
            
        }

        private void BindData()
        {
           
            ReportViewer1.Visible = true;

            string status = "";
            string onHand = "";
            if (rdbStatus.SelectedIndex == 0)
            {
                status = "1";
            }
            else if (rdbStatus.SelectedIndex == 1)
            {
                status = "0";
            }

            if (rdbOnHand.SelectedIndex == 0)
            {
                onHand = "1";
            }
            else if (rdbOnHand.SelectedIndex == 1)
            {
                onHand = "0";
            }

            GPlus.DataAccess.StockDAO st = new DataAccess.StockDAO();

            DataSet ds = st.GetStocOnHandkReport(ddlStock.SelectedValue, ddlMaterialType.SelectedValue, ddlSubMaterialType.SelectedValue, txtItemCode.Text, txtItemName.Text, status, onHand);
            ReportDataSource rds1 = new ReportDataSource("StockOnHandDataSet", ds.Tables[0]);
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