﻿using System;
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
    public partial class pop_ProductSelect2 : Pagebase
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
                BindDropdown();
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
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtProductCode.Text = "";
            txtProductName.Text = "";
            txtSupplierCode.Text = "";
            txtSupplierName.Text = "";
        }

        private void BindDropdown()
        {
            ddlUnit.DataSource = new DataAccess.PackageDAO().GetPackage("", "1", 1, 1000, "", "").Tables[0];
            ddlUnit.DataTextField = "Package_Name";
            ddlUnit.DataValueField = "Pack_ID";
            ddlUnit.DataBind();
            ddlUnit.Items.Insert(0, new ListItem("เลือกหน่วยนับ", ""));
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemPackandPrice(txtProductCode.Text, txtProductName.Text, "", ddlUnit.SelectedValue, txtSupplierCode.Text
                , txtSupplierName.Text, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvItem.DataSource = ds.Tables[0];
            gvItem.DataBind();
        }


        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.Script = "function CheckAllP(state){";
                ((CheckBox)e.Row.FindControl("chkDH")).Attributes.Add("onclick", "CheckAllP(this.checked);");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv["Unit_Price"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((decimal)drv["Unit_Price"]).ToString(this.CurrencyFormat);

                if (drv["LPur_TradeDiscount_Percent"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((decimal)drv["LPur_TradeDiscount_Percent"]).ToString(this.CurrencyFormat);

                if (drv["LPur_TradeDiscount_Amount"].ToString().Trim().Length > 0)
                    e.Row.Cells[7].Text = ((decimal)drv["LPur_TradeDiscount_Amount"]).ToString(this.CurrencyFormat);

                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                HiddenField hdUnitID = (HiddenField)e.Row.FindControl("hdUnitID");
                hdID.Value = drv["Inv_ItemID"].ToString();
                hdUnitID.Value = drv["Pack_ID"].ToString();
                CheckBox chkD = (CheckBox)e.Row.FindControl("chkD");
                this.Script += "document.getElementById('"+chkD.ClientID+"').checked = state;";
            }
            else if(e.Row.RowType == DataControlRowType.Footer)
            {
                this.Script += "}";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chk", this.Script, true);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            PRPOActualTable pat = PRPOActualTableFactory.CreateTable(Request["main"], Request["type"]);

            //if (Request["main"] == "pr")
            //{
            //    DataAccess.PRDAO db = new DataAccess.PRDAO();
            //    for (int i = 0; i < gvItem.Rows.Count; i++)
            //    {
            //        CheckBox chkD = (CheckBox)gvItem.Rows[i].FindControl("chkD");
            //        if (chkD.Checked)
            //        {
            //            HiddenField hdID = (HiddenField)gvItem.Rows[i].FindControl("hdID");
            //            HiddenField hdUnitID = (HiddenField)gvItem.Rows[i].FindControl("hdUnitID");
            //            TextBox txtQuantity = (TextBox)gvItem.Rows[i].FindControl("txtQuantity");
            //            if (txtQuantity.Text.Trim().Length == 0) txtQuantity.Text = "1";
            //            try
            //            {
            //                int a = int.Parse(txtQuantity.Text);
            //            }
            //            catch
            //            {
            //                txtQuantity.Text = "1";
            //            }

            //            if (!db.AddPRItem(Request["id"], hdID.Value, "", "", hdUnitID.Value, gvItem.Rows[i].Cells[5].Text.Replace(",", ""), 
            //                txtQuantity.Text.Replace(",", ""), "", "", "", "", "", "", "", "", ""))
            //            {
            //                ScriptManager.RegisterStartupScript(this, this.GetType(), "op", "alert('รายการสินค้าที่ขอซื้อซ้ำกัน');", true);
            //                return;
            //            }
            //        }
            //    }
            //}
            if (Request["main"] == "po" || Request["main"] == "pr")
            {
                for (int i = 0; i < gvItem.Rows.Count; i++)
                {
                    CheckBox chkD = (CheckBox)gvItem.Rows[i].FindControl("chkD");
                    if (chkD.Checked)
                    {
                        HiddenField hdID        = (HiddenField)gvItem.Rows[i].FindControl("hdID");      // Inv_ItemID 
                        HiddenField hdUnitID    = (HiddenField)gvItem.Rows[i].FindControl("hdUnitID");  // Pack_ID หน่วยนับ
                        TextBox txtQuantity     = (TextBox)gvItem.Rows[i].FindControl("txtQuantity");   // จำนวนสั่งซื้อ  
                        
                        if (txtQuantity.Text.Trim().Length == 0) 
                            txtQuantity.Text = "1";

                        try
                        {
                            int a = int.Parse(txtQuantity.Text);
                        }
                        catch
                        {
                            txtQuantity.Text = "1";
                        }

                        // Begin Green Edit
                        DataRow row = pat.FindItem(hdID.Value, hdUnitID.Value).FirstOrDefault();
                        if (row != null)
                        {
                            ScriptManager.RegisterStartupScript
                            (
                                this,
                                GetType(),
                                "popProductSelect",
                                "alert('รายการที่เลือกซ้ำ')",
                                true
                            );
                            return;
                        }
                        else
                        {
                            DataRow dr = pat.Table.NewRow();

                            dr["InvItemID"]     = hdID.Value;
                            dr["PackID"]        = hdUnitID.Value;
                            dr["InvItemCode"]   = gvItem.Rows[i].Cells[1].Text;
                            dr["InvItemName"]   = gvItem.Rows[i].Cells[2].Text;
                            dr["PackName"]      = gvItem.Rows[i].Cells[3].Text;
                            dr["UnitPrice"]     = gvItem.Rows[i].Cells[5].Text.Replace(",", "");
                            dr["UnitQuantity"]  = txtQuantity.Text;
                            dr["PopupType"]     = PRPOPopup.Product;
                            dr["Grouped"]       = "N";
                            dr["PrID"]          = "";
                            dr["PrItemID"]      = "";

                            pat.Table.Rows.Add(dr);
                            dr.AcceptChanges();
                        }
                    }
                }
            }

            // Begin Green Edit
            string js = 
                    "if (window.opener) {" +
                    "   window.opener.document.getElementById('btnRefreshI').click();" +
                    "}" +
                    "window.close();";
          
            // End Green Edit

            ScriptManager.RegisterStartupScript
            (
                this, 
                this.GetType(), 
                "cl", 
                js, 
                true
            );
        }
    }
}