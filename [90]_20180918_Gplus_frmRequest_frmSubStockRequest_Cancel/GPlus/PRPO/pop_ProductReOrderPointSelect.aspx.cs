using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_ProductReOrderPointSelect : Pagebase
    {
        public string Script
        {
            get
            {
                if (ViewState["Script"] == null)
                    ViewState["Script"] = "";

                return ViewState["Script"].ToString();
            }
            set
            {
                ViewState["Script"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
            if (this.Script.Trim().Length > 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "chk", this.Script, true);
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
            txtItemCode.Text = "";
            txtItemName.Text = "";
        }

        protected void gvProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.Script = "function CheckAllP(state){";
                ((CheckBox)e.Row.FindControl("chkDH")).Attributes.Add("onclick", "CheckAllP(this.checked);");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv["Avg_Cost"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((decimal)drv["Avg_Cost"]).ToString(this.CurrencyFormat);

                if (drv["OnHand_Qty"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((decimal)drv["OnHand_Qty"]).ToString("#,##0");

                if (drv["Reorder_Point"].ToString().Trim().Length > 0)
                    e.Row.Cells[7].Text = ((decimal)drv["Reorder_Point"]).ToString("#,##0");

                DropDownList ddlPack = (DropDownList)e.Row.FindControl("ddlPack");
                ddlPack.DataSource = new DataAccess.ItemDAO().GetItemPack(drv["Inv_ItemID"].ToString());
                ddlPack.DataBind();
                if (ddlPack.Items.FindByValue(drv["Pack_ID_Purchase"].ToString()) != null)
                    ddlPack.SelectedValue = drv["Pack_ID_Purchase"].ToString();

                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                HiddenField hdUnitID = (HiddenField)e.Row.FindControl("hdUnitID");
                hdID.Value = drv["Inv_ItemID"].ToString();
                hdUnitID.Value = drv["Pack_ID"].ToString();
                CheckBox chkD = (CheckBox)e.Row.FindControl("chkD");
                this.Script += "document.getElementById('" + chkD.ClientID + "').checked = state;";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                this.Script += "}";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chk", this.Script, true);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (Request["type"] == null)
            {
                DataAccess.PRDAO db = new DataAccess.PRDAO();
                for (int i = 0; i < gvProduct.Rows.Count; i++)
                {
                    CheckBox chkD = (CheckBox)gvProduct.Rows[i].FindControl("chkD");
                    if (chkD.Checked)
                    {
                        HiddenField hdID = (HiddenField)gvProduct.Rows[i].FindControl("hdID");
                        HiddenField hdUnitID = (HiddenField)gvProduct.Rows[i].FindControl("hdUnitID");
                        TextBox txtQuantity = (TextBox)gvProduct.Rows[i].FindControl("txtQuantity");
                        DropDownList ddlPack = (DropDownList)gvProduct.Rows[i].FindControl("ddlPack");
                        if (txtQuantity.Text.Trim().Length == 0) txtQuantity.Text = "1";

                        db.AddPRItem(Request["id"], hdID.Value, "", "", ddlPack.SelectedValue, gvProduct.Rows[i].Cells[5].Text, txtQuantity.Text,
                            "", "", "", "", "", "", "", "", "");
                    }
                }
            }
            else
            {
                DataAccess.PODAO db = new DataAccess.PODAO();
                for (int i = 0; i < gvProduct.Rows.Count; i++)
                {
                    CheckBox chkD = (CheckBox)gvProduct.Rows[i].FindControl("chkD");
                    if (chkD.Checked)
                    {
                        HiddenField hdID = (HiddenField)gvProduct.Rows[i].FindControl("hdID");
                        HiddenField hdUnitID = (HiddenField)gvProduct.Rows[i].FindControl("hdUnitID");
                        TextBox txtQuantity = (TextBox)gvProduct.Rows[i].FindControl("txtQuantity");
                        DropDownList ddlPack = (DropDownList)gvProduct.Rows[i].FindControl("ddlPack");
                        if (txtQuantity.Text.Trim().Length == 0) txtQuantity.Text = "1";

                        db.AddPOItem(Request["id"], hdID.Value, "", "", ddlPack.SelectedValue, gvProduct.Rows[i].Cells[5].Text, txtQuantity.Text,
                            "", "", "", "", "", "", "", "", "");
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "cl", "if(window.opener)window.opener.document.getElementById('btnRefreshI').click();" +
                "window.close();", true);
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemPackReorderPoint(txtItemCode.Text, txtItemName.Text,"", PagingControl1.CurrentPageIndex,
                PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvProduct.DataSource = ds.Tables[0];
            gvProduct.DataBind();
        }

    }
}