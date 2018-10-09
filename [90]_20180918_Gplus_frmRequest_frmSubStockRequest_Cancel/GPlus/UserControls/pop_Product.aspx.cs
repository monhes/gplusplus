using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class pop_Product : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                BindData();

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtProductCode.Text = "";
            txtProductName.Text = "";
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");
                btnSelect.OnClientClick = "if(window.opener){" +
                    "window.opener.document.getElementById('" + Request["id"] + "').value = '" + drv["Inv_ItemID"].ToString() + "'; " +
                    "window.opener.document.getElementById('" + Request["code"] + "').value = '" + drv["Inv_ItemCode"].ToString() + "'; " +
                    "window.opener.document.getElementById('" + Request["name"] + "').value = '" + drv["Inv_ItemName"].ToString() + "'; " +
                    "}window.close();return false;";
            }
        }

        protected void gvItem_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemSearch(txtProductCode.Text, txtProductName.Text, PagingControl1.CurrentPageIndex,
                PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvItem.DataSource = ds.Tables[0];
            gvItem.DataBind();
        }

    }
}