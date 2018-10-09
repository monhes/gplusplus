using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.Request
{
    public partial class dgItemCatagory : Pagebase
    {
        DataTable dtPackage = null;
        StringBuilder sb = new StringBuilder();

        public void GvMaterialRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                ((LinkButton)e.Row.FindControl("btnSelected")).CommandArgument = drv["Inv_ItemID"].ToString();
                this._hfItemId.Value = drv["Inv_ItemID"].ToString();
            }
        }

        public void GvMaterialRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                //Session["ItemId"] = Int32.Parse(e.CommandArgument.ToString());
                DataRow drRow = ((DataTable)Session["dtItems"]).Select("[Inv_ItemID]='" + e.CommandArgument + "'").FirstOrDefault();
                string itemCode = drRow["Inv_ItemCode"].ToString();
                foreach (GridViewRow c in this.gvMaterial.Rows)
                {
                    string selectedItemCode = c.Cells[1].Text;
                    if (selectedItemCode == itemCode)
                    {
                        Session["ItemQty"] = (c.Cells[6].FindControl("tbQty") as TextBox).Text;
                        break;
                    }
                }
                Session["Item"] = drRow;
                string scriptStr = "<script>if (window.opener) { window.opener.document.getElementById('btnRefresh').click(); } window.close();</script>";
                ClientScript.RegisterStartupScript(typeof(string), "closing", scriptStr);
            }
        }

        //private void BindDropdown()
        //{
        //    DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
        //    ddlMaterialTypeSearch.DataSource = dt;
        //    ddlMaterialTypeSearch.DataTextField = "Cat_Name";
        //    ddlMaterialTypeSearch.DataValueField = "Cate_ID";
        //    ddlMaterialTypeSearch.DataBind();
        //    ddlMaterialTypeSearch.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        //}

        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemPack(txtMaterialCodeSearch.Text, txtMaterialNameSearch.Text, "", ddlStatus.SelectedValue, 
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder, "1","", "1");

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvMaterial.DataSource = ds.Tables[0];
            gvMaterial.DataBind();

            Session["dtItems"] = ds.Tables[0];
        }

        public void BtnSearchClick(object sender, EventArgs e)
        {
            BindData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}