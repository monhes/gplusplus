using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.Stock
{
    public partial class SetStockItemSearchPopup : Pagebase
    {
        public void gvMaterial_Sorting(object sender, GridViewSortEventArgs e)
        {
            
        }
        DataTable dtPackage = null;
        StringBuilder sb = new StringBuilder();
        string isBaseUnitID = "";
        public void gvMaterial_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                ((LinkButton)e.Row.FindControl("btnSelected")).CommandArgument = drv["Inv_ItemID"].ToString();
                this._hfItemId.Value = drv["Inv_ItemID"].ToString();  
            }
        }

        public void gvMaterial_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                Session["ITEM_ID_SetStk"] = Int32.Parse(e.CommandArgument.ToString());
                string scriptStr = "<script>window.close();</script>";
                ClientScript.RegisterStartupScript(typeof(string), "closing", scriptStr);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
            ddlMaterialTypeSearch.DataSource = dt;
            ddlMaterialTypeSearch.DataTextField = "Cat_Name";
            ddlMaterialTypeSearch.DataValueField = "Cate_ID";
            ddlMaterialTypeSearch.DataBind();
            ddlMaterialTypeSearch.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItem(txtMaterialCodeSearch.Text, txtMaterialNameSearch.Text, ddlMaterialTypeSearch.SelectedValue,
                txtOldCodeSearch.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvMaterial.DataSource = ds.Tables[0];
            gvMaterial.DataBind();
        }

        protected void BtnSearchClick(object sender, EventArgs e)
        {
            BindData();
        }
    }
}