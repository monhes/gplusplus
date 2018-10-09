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
    public partial class HPSupplierSelect : Pagebase
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
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }
        private void BindData()
        {
            DataSet ds = new DataAccess.HistoryPurchaseDAO().GetSupplierName(txtCompany.Text, PagingControl1.CurrentPageIndex,
                PagingControl1.PageSize, this.SortColumn, this.SortOrder);

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

                    btnSelect.OnClientClick = "if(window.opener){ " +
                        "window.opener.document.getElementById('" + Request["supplierID"] + "').value = '" + drv["Supplier_ID"].ToString() + "'; " +
                        "window.opener.document.getElementById('" + Request["supplierName"] + "').value = '" + drv["Supplier_Name"].ToString() + "'; " +
                    "} window.close();return false;";
                
            }
        }
    }
}