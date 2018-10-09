using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class ReorderPointReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "307";
                BindDropdown();
                ReportViewer1.Visible = false;
            }
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];
            ddlCate.Items.Clear();
            ddlCate.Items.Add(new ListItem("เลือกประเภท", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlCate.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));
            }

            DataTable dtStock = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlStock.Items.Clear();
            for (int i = 0; i < dtStock.Rows.Count; i++)
            {
                ddlStock.Items.Add(new ListItem(dtStock.Rows[i]["Stock_Code"].ToString() + " - " + dtStock.Rows[i]["Stock_Name"].ToString(),
                    dtStock.Rows[i]["Stock_Id"].ToString()));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = true;
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ReporderPointDataSet",
                new DataAccess.ReorderPointDAO().GetReorderPointReport(ddlStock.SelectedValue, ddlCate.SelectedValue, ddlSubCate.SelectedValue,
                txtItemCode.Text, txtItemName.Text)));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtItemCode.Text = "";
            txtItemName.Text = "";
            ddlStock.SelectedIndex = 0;
            ddlCate.SelectedIndex = 0;
            ddlSubCate.SelectedIndex = 0;
        }

        protected void ddlCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCate.SelectedIndex > 0)
            {
                DataTable dt = new DataAccess.CategoryDAO().GetSubCate("", ddlCate.SelectedValue, "",
                    "1", 1, 1000, "SubCate_Code", "").Tables[0];
                ddlSubCate.Items.Clear();
                ddlSubCate.Items.Add(new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ddlSubCate.Items.Add(new ListItem(dt.Rows[i]["SubCate_Code"].ToString() + " - " + dt.Rows[i]["SubCate_Name"].ToString(),
                        dt.Rows[i]["SubCate_ID"].ToString()));
                }
            }
            else
                ddlSubCate.Items.Clear();
        }
    }
}