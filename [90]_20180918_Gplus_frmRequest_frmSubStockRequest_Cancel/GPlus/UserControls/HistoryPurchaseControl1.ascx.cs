using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class HistoryPurchaseControl1 : System.Web.UI.UserControl
    {
        public string SortColumn
        {
            get
            {
                if (ViewState["SortColumn"] == null)
                    ViewState["SortColumn"] = "";

                return ViewState["SortColumn"].ToString();
            }
            set
            {
                ViewState["SortColumn"] = value;
            }
        }

        public string SortOrder
        {
            get
            {
                if (ViewState["SortOrder"] == null)
                    ViewState["SortOrder"] = "";

                return ViewState["SortOrder"].ToString();
            }
            set
            {
                ViewState["SortOrder"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindDropdown();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        //private void BindDropdown()
        //{
        //    DataTable dt = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
        //    ddlSupplier.DataSource = dt;
        //    ddlSupplier.DataTextField = "Supplier_Name";
        //    ddlSupplier.DataValueField = "Supplier_ID";
        //    ddlSupplier.DataBind();
        //    ddlSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));

        //    //ddlSearchSupplier.DataSource = dt;
        //    //ddlSearchSupplier.DataTextField = "Supplier_Name";
        //    //ddlSearchSupplier.DataValueField = "Supplier_ID";
        //    //ddlSearchSupplier.DataBind();
        //    //ddlSearchSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));
        //}

        //protected void btnSearch_Click(object sender, EventArgs e)
        //{
        //    if (ItemControl1.ItemID.Trim().Length == 0)
        //    {
        //        new Pagebase().ShowMessageBox("กรุณาเลือกวัสดุอุปกรณ์");
        //    }
        //    else
        //    {
        //        PagingControl1.CurrentPageIndex = 1;
        //        BindData();
        //    }
        //}

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlSearchBackMonth.SelectedIndex = 0;
            //ddlSearchSupplier.SelectedIndex = 0;
            ddlSearchSupplierCount.SelectedIndex = 0;
            ItemControl1.ItemCode = "";
            ItemControl1.ItemName = "";
            ItemControlSupplier.DivName = "";
            pnlDetail.Visible = false;
        }

        private void BindPackage()
        {
            if (ItemControl1.ItemID.Trim().Length > 0)
            {
                DataTable dtItemPack = new DataAccess.ItemDAO().GetItemPack(ItemControl1.ItemID);

                ddlUnit.DataSource = dtItemPack;
                ddlUnit.DataTextField = "Description";
                ddlUnit.DataValueField = "Pack_Id";
                ddlUnit.DataBind();

                ddlLPurPack.DataSource = dtItemPack;
                ddlLPurPack.DataTextField = "Description";
                ddlLPurPack.DataValueField = "Pack_Id";
                ddlLPurPack.DataBind();
            }
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.HistoryPurchaseDAO().GetHistoryPurchase(ItemControl1.ItemCode, "", ItemControlSupplier.DivName,
                ddlSearchSupplierCount.SelectedValue, ddlSearchBackMonth.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize,
                this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvHistory.DataSource = ds.Tables[0];
            gvHistory.DataBind();
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ItemControl1.ItemID.Trim().Length == 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "er1", "alert('กรุณาเลือกวัสดุอุปกรณ์');", true);
            }
            else
            {
                ClearData();
                pnlDetail.Visible = true;
                BindPackage();

                Pagebase pb = new Pagebase();

                lblCreateBy.Text = pb.FirstName + " " + pb.LastName;
                lblUpdateBy.Text = pb.FirstName + " " + pb.LastName;
                lblCreateDate.Text = DateTime.Now.ToString(pb.DateTimeFormat);
                lblUpdatedate.Text = DateTime.Now.ToString(pb.DateTimeFormat);
                
               
            }
        }

        protected void gvHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                HiddenField hdIID = (HiddenField)e.Row.FindControl("hdIID");
                HiddenField hdItemID = (HiddenField)e.Row.FindControl("hdItemID");
                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                if (drv["Purchase_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[2].Text = ((DateTime)drv["Purchase_Date"]).ToString("dd/MM/yyyy");

                hdIID.Value = drv["HistoryPurchase_ID"].ToString();
                hdItemID.Value = drv["Inv_ItemID"].ToString();
                //ItemControlSupplier1.DivName = drv["Supplier_Name"].ToString();
                btnDetail.CommandArgument = e.Row.RowIndex.ToString();
               
                e.Row.Cells[7].Text = drv["Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";
            }
        }

        protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                BindPackage();
                DataTable dt = new DataAccess.HistoryPurchaseDAO().GetHistoryPurchase(
                     ((HiddenField)gvHistory.Rows[int.Parse(e.CommandArgument.ToString())].FindControl("hdIID")).Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    hdID.Value = dt.Rows[0]["HistoryPurchase_ID"].ToString();
                    if (dr["Supplier_Name"].ToString().Trim().Length > 0)
                        ItemControlSupplier1.DivName = dr["Supplier_Name"].ToString();

                    if (dr["Purchase_Date"].ToString().Trim().Length > 0)
                        ccPurchaseDate.Text = ((DateTime)dr["Purchase_Date"]).ToString("dd/MM/yyyy");

                    if (ddlUnit.Items.FindByValue(dr["Pack_ID"].ToString()) != null)
                        ddlUnit.SelectedValue = dr["Pack_ID"].ToString();

                    if (dr["Purchase_Price_Unit"].ToString().Trim().Length > 0)
                        txtUnitPrice.Text = ((decimal)dr["Purchase_Price_Unit"]).ToString("0.00");

                    if (dr["Propose_Price_Unit"].ToString().Trim().Length > 0)
                        txtProposePriceUnit.Text = ((decimal)dr["Propose_Price_Unit"]).ToString("0.00");

                    if (dr["Propose_Date"].ToString().Trim().Length > 0)
                        ccProposeDate.Text = ((DateTime)dr["Propose_Date"]).ToString("dd/MM/yyyy");

                    if (dr["LPur_TradeDiscount_Percent"].ToString().Length > 0)
                        txtLPurTradeP.Text = ((decimal)dr["LPur_TradeDiscount_Percent"]).ToString("0");

                    if (dr["LPur_TradeDiscount_Amount"].ToString().Length > 0)
                        txtLPurTradeA.Text = ((decimal)dr["LPur_TradeDiscount_Amount"]).ToString("0.00");

                    if (ddlLPurPack.Items.FindByValue(dr["LPur_Premium_Pack_ID"].ToString()) != null)
                        ddlLPurPack.SelectedValue = dr["LPur_Premium_Pack_ID"].ToString();
                    if (dr["LPur_Premium_Qty"].ToString().Length > 0)
                        txtFee.Text = ((decimal)dr["LPur_Premium_Qty"]).ToString("0");

                    rdbVatUnitType.Items[0].Selected = dt.Rows[0]["VatUnit_Type"].ToString() == "1";
                    rdbVatUnitType.Items[1].Selected = dt.Rows[0]["VatUnit_Type"].ToString() == "0";

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Status"].ToString() == "0";

                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(new Pagebase().DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(new Pagebase().DateTimeFormat);

                }
                pnlDetail.Visible = true;
            }
        }

        protected void gvHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            Pagebase pb = new Pagebase();
            pb.SetSortGridView(e.SortExpression);
            BindData();
            pb.GridViewSort(gvHistory);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string VatUnitType = rdbVatUnitType.SelectedIndex == 0 ? "1" : "0";

            if (hdID.Value.Trim().Length == 0)
            {
                if (ItemControlSupplier1.DivName == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "er1", "alert('กรุณาเลือก Supplier');", true);
                }
             
                else
                {
                    new DataAccess.HistoryPurchaseDAO().AddHistoryPurchase(ItemControl1.ItemID, ddlUnit.SelectedValue, ItemControlSupplier1.DivID,
                    ccPurchaseDate.Value, txtUnitPrice.Text, txtLPurTradeP.Text, txtLPurTradeA.Text, txtFee.Text, ddlLPurPack.SelectedValue, txtProposePriceUnit.Text,
                    ccProposeDate.Value, "", "", "", "", "", VatUnitType, status, new Pagebase().UserID);

                }

            }

            else
            {
                
                new DataAccess.HistoryPurchaseDAO().UpdateHistoryPurchase(hdID.Value, ItemControl1.ItemID, ddlUnit.SelectedValue,ItemControlSupplier1.DivName,
                   ccPurchaseDate.Value, txtUnitPrice.Text, txtLPurTradeP.Text, txtLPurTradeA.Text, txtFee.Text, ddlLPurPack.SelectedValue, txtProposePriceUnit.Text,
                   ccProposeDate.Value, "", "", "", "", VatUnitType, status, new Pagebase().UserID);

            }
            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = false;

        }

        private void ClearData()
        {
            hdID.Value = "";
            ItemControlSupplier1.DivName = "";
            ccProposeDate.Text = "";
            ccPurchaseDate.Text = "";
            ddlUnit.Items.Clear();
            txtUnitPrice.Text = "";
            txtProposePriceUnit.Text = "";
            txtLPurTradeP.Text = "";
            txtLPurTradeA.Text = "";
            txtFee.Text = "";
            ddlLPurPack.Items.Clear();
            rdbStatus.SelectedIndex = 0;
            rdbVatUnitType.SelectedIndex = 0;
        }

       

        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            if (ItemControl1.ItemID == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "er1", "alert('กรุณาเลือกวัสดุอุปกรณ์');", true);
            }
            else
            {
                PagingControl1.CurrentPageIndex = 1;
                pnlDetail.Visible = false;
                BindData();
            }
         
            
        }

    }
}