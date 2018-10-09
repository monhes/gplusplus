using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.Diagnostics;
using Microsoft.Reporting.WebForms;

namespace GPlus.PRPO
{
    public partial class ReorderPointPurchaseReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "310";
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

            if (startReorderPoint.Text != "" && endReorderPoint.Text != "")
            {
                ReportParameterCollection reportParams = new ReportParameterCollection();

                reportParams.Add(new ReportParameter("StartReorderPoint", ConvertToThaiDate(startReorderPoint.Text)));
                reportParams.Add(new ReportParameter("EndReorderPoint", ConvertToThaiDate(endReorderPoint.Text)));
                ReportViewer1.LocalReport.SetParameters(reportParams);
            }
  

            SQLParameterList param = new SQLParameterList();
            param.AddStringField("StockID", ddlStock.SelectedValue);
            param.AddStringField("CateID", ddlCate.SelectedValue);
            param.AddStringField("SubCateID", ddlSubCate.SelectedValue);
            param.AddStringField("InvItemCode", txtItemCode.Text);
            param.AddStringField("InvItemName", txtItemName.Text);
            param.AddStringField("StartReorderPoint", startReorderPoint.Text);
            param.AddStringField("EndReorderPoint", endReorderPoint.Text);

            ReportViewer1.LocalReport.DataSources.Add(
                new Microsoft.Reporting.WebForms.ReportDataSource(
                    "ReorderPointPurchaseDataSet",
                    new DataAccess.ReorderPointDAO().GetReorderPointPurchaseReport(
                        param.GetSqlParameterList()
                    )
                )
            );
           
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

        private string ConvertToThaiDate(string date)
        {
            int index = date.LastIndexOf('/');
            string year = date.Substring(index + 1);

            return date.Substring(0, index + 1) + (Convert.ToInt32(year) + 543).ToString();
        }
    }
}