using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using GPlus.PRPO.PRPOHelper;
using System.Linq;

namespace GPlus.PRPO
{
    public partial class pop_ProductReOrderPointSelect2 : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();

                InitializeCategoryDropDownList();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void InitializeCategoryDropDownList()
        {
            DataTable dtCategory = new DataAccess.CategoryDAO().GetCategoryAll();

            ddlCategory.DataSource = dtCategory;
            ddlCategory.DataBind();

            ddlCategory.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void bSearch_Click(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void bCancel1_Click(object sender, EventArgs e)
        {
            tbItemCode.Text = "";
            tbItemName.Text = "";
            ccStartDate.Text = "";
            ccEndDate.Text = "";
            ddlCategory.SelectedValue = "";
        }

        protected void gvProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                HiddenField hfItemID = e.Row.FindControl("hfItemID") as HiddenField;
                HiddenField hfPackID = e.Row.FindControl("hfPackID") as HiddenField;

                hfItemID.Value = drv["Inv_ItemID"].ToString();
                hfPackID.Value = drv["Pack_Id"].ToString();
            }
        }

        protected void bOK_Click(object sender, EventArgs e)
        {
            PRPOActualTable pat = PRPOActualTableFactory.CreateTable(Request["main"], Request["type"]);
            string reorderPoint_Date = "";


            if (Request["main"] == "pr")
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
            else if (Request["main"] == "po")
            {
                string itemSelected = "";

                for (int i = 0; i < gvProduct.Rows.Count; i++)
                {
                    CheckBox cbSelect = (CheckBox)gvProduct.Rows[i].FindControl("cbSelect");
                    if (cbSelect.Checked)
                    {
                        HiddenField hfItemID = gvProduct.Rows[i].FindControl("hfItemID") as HiddenField;
                        HiddenField hfPackID = gvProduct.Rows[i].FindControl("hfPackID") as HiddenField;
                        TextBox tbQuantity = gvProduct.Rows[i].FindControl("tbQuantity") as TextBox;

                        if (tbQuantity.Text.Trim().Length == 0) tbQuantity.Text = "1";

                        DataRow row = pat.FindItem(hfItemID.Value, hfPackID.Value).FirstOrDefault();
                        if (row != null)
                        {
                            ScriptManager.RegisterStartupScript
                            (
                                this,
                                typeof(Page),
                                "warning",
                                "alert(\"รายการที่เลือกซ้ำ\");",
                                true
                            );
                            return;
                        }
                        else
                        {
                            #region Nin Edit 21022014

                            if (itemSelected == "")
                            {
                                itemSelected = itemSelected + hfItemID.Value;
                            }
                            else
                            {
                                itemSelected = itemSelected + "," + hfItemID.Value;
                            }

                            #endregion

                            DataRow dr = pat.Table.NewRow();

                            dr["InvItemID"]     = hfItemID.Value;
                            dr["PackID"]        = hfPackID.Value;
                            dr["InvItemCode"]   = gvProduct.Rows[i].Cells[1].Text;
                            dr["InvItemName"]   = gvProduct.Rows[i].Cells[2].Text;
                            dr["PackName"]      = gvProduct.Rows[i].Cells[7].Text;
                            dr["UnitPrice"]     = gvProduct.Rows[i].Cells[4].Text.Replace(",", "");
                            dr["UnitQuantity"]  = tbQuantity.Text;
                            dr["PopupType"]     = PRPOPopup.ReorderPoint;
                            //dr["UnitOrder"]     = ddlPack.SelectedValue;
                            dr["Grouped"]       = "N";
                            dr["PrID"]          = "";
                            dr["PrItemID"]      = "";

                            pat.Table.Rows.Add(dr);
                            dr.AcceptChanges();
                        }
                    }
                }

                if (itemSelected != "")
                {
                    DataTable dt = new DataAccess.DatabaseHelper().ExecuteQuery
                                            (
                                              "SELECT TOP 1 "
                                            + "  ReorderPointMark_Date "
                                            + "FROM Inv_Stock_OnHand "
                                            + "WHERE Inv_ItemID IN ( " + itemSelected + " )"
                                            + "    AND Stock_ID = 1 "
                                            + "    AND ReorderPointMark_Date IS NOT NULL "
                                            + "    ORDER BY ReorderPointMark_Date DESC "
                                            ).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        reorderPoint_Date = ((DateTime)dt.Rows[0]["ReorderPointMark_Date"]).ToString(this.DateFormat);
                    }
                }
            }

            string scrpt = "";

            if (Request["main"] == "po")
            {
                scrpt = "window.opener.document.getElementById('hfreorderPoint_Date').value = '" + reorderPoint_Date + "';";
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "cl", "if(window.opener){"+scrpt+"window.opener.document.getElementById('btnRefreshI').click();}" +
                "window.close();", true);
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemPackReorderPoint2
            (
                tbItemCode.Text
                , tbItemName.Text
                , ddlCategory.SelectedValue
                , ccStartDate.Text
                , ccEndDate.Text
                , PagingControl1.CurrentPageIndex
                , PagingControl1.PageSize
            );

            PagingControl1.RecordCount = (int) ds.Tables[1].Rows[0][0];

            gvProduct.DataSource = ds.Tables[0];
            gvProduct.DataBind();
        }

    }
}