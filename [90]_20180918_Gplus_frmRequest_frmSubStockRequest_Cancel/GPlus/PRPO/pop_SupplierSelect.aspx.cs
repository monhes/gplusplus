using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

using GPlus.DataAccess;
using System.Data;

namespace GPlus.PRPO
{
    public partial class pop_CompanySelect : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData(txtCompany.Text.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(txtCompany.Text.ToString());
        }

        private void BindData(string supplierName = "")
        {
            SQLParameterList sqlParamList = new SQLParameterList();

            sqlParamList.AddStringField("SupplierName", supplierName);
            if (Request["FilterStatus"] == "true")
            {
                sqlParamList.AddStringField("SupplierStatus", "true");
            }
            sqlParamList.AddIntegerField("PageNum", PagingControl1.CurrentPageIndex);
            sqlParamList.AddIntegerField("PageSize", PagingControl1.PageSize);

            DataSet ds = new PrPoDAO().GetSupplierName(sqlParamList);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvSupplier.DataSource = ds.Tables[0];
            gvSupplier.DataBind();
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");

                if (!string.IsNullOrEmpty(Request["SupplierDdl"]))
                {
                    string js =
                          "if (window.opener) {"
                        + "   var ddlSupplier = window.opener.document.getElementById('" + Request["SupplierDdl"] + "');"
                        + "   for (i = 0; i < ddlSupplier.options.length; ++i) {"
                        + "       if (ddlSupplier.options[i].value == " + drv["Supplier_ID"].ToString() + ") {"
                        + "           ddlSupplier.options[i].selected = true;"
                        + "           break;"
                        + "       }"
                        + "   }"
                        + "}"
                        + "window.close();"
                        + "return false;";

                    btnSelect.OnClientClick = js;
                }
                else
                {
                    btnSelect.OnClientClick = "if(window.opener){ " +
                    "window.opener.document.getElementById('" + Request["supplierName"] + "').value = '" + drv["Supplier_Name"].ToString() + "'; " +
                    "} window.close();return false;";
                }
            }
        }
    }
}