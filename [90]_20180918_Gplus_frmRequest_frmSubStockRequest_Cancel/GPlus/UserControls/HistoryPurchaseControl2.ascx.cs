using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GPlus.DataAccess;
using GPlus.UserControls;

namespace GPlus.UserControls
{
    public partial class HistoryPurchaseControl2 : System.Web.UI.UserControl
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

                btnSelect.OnClientClick = "open_popup('../UserControls/pop_Product2.aspx?id=" + hfItemID.ClientID +
                         "&code=" + tbItemCode.ClientID+ "&name=" + tbItemName.ClientID +
                         "&pid=" + hfPackID.ClientID + "&pname=" + tbUnit.ClientID+ "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
                
                //BindData();
              
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
       
          
        }

        //private void BindDropdown()
        //{
        //    DataTable dt = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];

        //    ddlSearchSupplier.DataSource = dt;
        //    ddlSearchSupplier.DataTextField = "Supplier_Name";
        //    ddlSearchSupplier.DataValueField = "Supplier_ID";
        //    ddlSearchSupplier.DataBind();
        //    ddlSearchSupplier.Items.Insert(0, new ListItem("--เลือก Supplier--", ""));
        //}


        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ItemControlSupplier.DivName == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "er1", "alert('กรุณาเลือก Supplier');", true);

            }
            else
            {
                PagingControl1.CurrentPageIndex = 1;
                pnlDetail.Visible = false;
                BindData();
            }
        }

        private void BindPackage()
        {
            if (hfItemID.Value.Trim().Length > 0)
            {
                DataTable dtItemPack = new DataAccess.ItemDAO().GetItemPack(hfItemID.Value);
                ddlLPurPack.DataSource = dtItemPack;
                ddlLPurPack.DataTextField = "Description";
                ddlLPurPack.DataValueField = "Pack_Id";
                ddlLPurPack.DataBind();

            }
            
        }

       


        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ItemControlSupplier.DivName = "";
            txtItem.Text = "";
            pnlDetail.Visible = false;
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.HistoryPurchaseDAO().GetHistoryPurchase("", txtItem.Text, ItemControlSupplier.DivName,
                "", "", PagingControl1.CurrentPageIndex, PagingControl1.PageSize,
                this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvHistory.DataSource = ds.Tables[0];
            gvHistory.DataBind();
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ItemControlSupplier.DivName == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "er1", "alert('กรุณาเลือก Supplier');", true);
            }
            
            else
            {
                ClearData();
                pnlDetail.Visible = true;
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
                HiddenField hdPackID = (HiddenField)e.Row.FindControl("hdPackID");
                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                hdItemID.Value = drv["Inv_ItemID"].ToString();
                hdPackID.Value = drv["Pack_ID"].ToString();
                if (drv["Purchase_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((DateTime)drv["Purchase_Date"]).ToString("dd/MM/yyyy");

                if (drv["Propose_Price_Unit"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((decimal)drv["Propose_Price_Unit"]).ToString("0.00");

                if (drv["LPur_TradeDiscount_Percent"].ToString().Trim().Length > 0)
                    e.Row.Cells[7].Text = ((decimal)drv["LPur_TradeDiscount_Percent"]).ToString("0");

                if (drv["LPur_TradeDiscount_Amount"].ToString().Trim().Length > 0)
                    e.Row.Cells[8].Text = ((decimal)drv["LPur_TradeDiscount_Amount"]).ToString("0.00");
                if (drv["LPur_Premium_Qty"].ToString().Length > 0)
                    e.Row.Cells[9].Text = ((decimal)drv["LPur_Premium_Qty"]).ToString("0");

                hdIID.Value = drv["HistoryPurchase_ID"].ToString();

                btnDetail.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        protected void gvHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
             
                int index = int.Parse(e.CommandArgument.ToString());
                DataTable dt = new DataAccess.HistoryPurchaseDAO().GetHistoryPurchase(
                     ((HiddenField)gvHistory.Rows[index].FindControl("hdIID")).Value);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    hdID.Value = dt.Rows[0]["HistoryPurchase_ID"].ToString();
                    BindItem(((HiddenField)gvHistory.Rows[index].FindControl("hdItemID")).Value,
                    ((HiddenField)gvHistory.Rows[index].FindControl("hdPackID")).Value);

                    //if (dr["Inv_ItemCode"].ToString().Trim().Length > 0)
                    //    tbItemCode.Text = dr["Inv_ItemCode"].ToString();

                    //if (dr["Inv_ItemName"].ToString().Trim().Length > 0)
                    //    tbItemName.Text = dr["Inv_ItemName"].ToString();

                    //if (dr["Description"].ToString().Trim().Length > 0)
                    //    tbUnit.Text = dr["Description"].ToString();

                    if (dr["Purchase_Date"].ToString().Trim().Length > 0)
                        ccPurchaseDate.Text = ((DateTime)dr["Purchase_Date"]).ToString("dd/MM/yyyy");


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

                    if (dr["LPur_Premium_Qty"].ToString().Length > 0)
                        txtFee.Text = ((decimal)dr["LPur_Premium_Qty"]).ToString("0");

                    if (ddlLPurPack.Items.FindByValue(dr["LPur_Premium_Pack_ID"].ToString()) != null)
                        ddlLPurPack.SelectedValue = dr["LPur_Premium_Pack_ID"].ToString();

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
                BindPackage();
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
                if (hfItemID.Value.Trim().Length == 0)
                {
                    new Pagebase().ShowMessageBox("กรุณาเลือกวัสดุอุปกรณ์");
                }
                
                else
                {
                    new DataAccess.HistoryPurchaseDAO().AddHistoryPurchase(hfItemID.Value, hfPackID.Value, ItemControlSupplier.DivID,
                        ccPurchaseDate.Value, txtUnitPrice.Text, txtLPurTradeP.Text, txtLPurTradeA.Text, txtFee.Text, ddlLPurPack.SelectedValue, txtProposePriceUnit.Text,
                        ccProposeDate.Value, "", "", "", "", "", VatUnitType, status, new Pagebase().UserID);
                }
            }
            else
            {
                if (ItemControlSupplier.DivName == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "er1", "alert('กรุณาเลือก Supplier');", true);
                }
                else
                {
                    new DataAccess.HistoryPurchaseDAO().UpdateHistoryPurchase(hdID.Value, hfItemID.Value, hfPackID.Value, ItemControlSupplier.DivName,
                        ccPurchaseDate.Value, txtUnitPrice.Text, txtLPurTradeP.Text, txtLPurTradeA.Text, txtFee.Text, ddlLPurPack.SelectedValue, txtProposePriceUnit.Text,
                        ccProposeDate.Value, "", "", "", "", VatUnitType, status, new Pagebase().UserID);
                }
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
           // ItemControl21.Clear();
            ccProposeDate.Text = "";
            ccPurchaseDate.Text = "";
            txtUnitPrice.Text = "";
            txtProposePriceUnit.Text = "";
            txtLPurTradeP.Text = "";
            txtLPurTradeA.Text = "";
            txtFee.Text = "";
            ddlLPurPack.Items.Clear();
            rdbStatus.SelectedIndex = 0;
            rdbVatUnitType.SelectedIndex = 0;
            hfItemID.Value = "";
            hfPackID.Value = "";
            tbItemCode.Text = "";
            tbItemName.Text = "";
            tbUnit.Text = "";
        
        }

       

        public void BindItem(string itemID, string packID)
        {
            DataTable dt = new DataAccess.ItemDAO().GetItemPack(itemID, packID);
            DataTable dtP = new DataAccess.ItemDAO().GetItem(itemID);
            if (dt.Rows.Count > 0)
            {
                hfItemID.Value= itemID;
                hfPackID.Value= packID;
                tbItemCode.Text = dtP.Rows[0]["Inv_ItemCode"].ToString();
                tbItemName.Text = dtP.Rows[0]["Inv_ItemName"].ToString();
                tbUnit.Text = dt.Rows[0]["Description"].ToString();
            }
        }


        protected void btnRefreshSelect_Click(object sender, EventArgs e)
        {
            btnRefreshSelect.Attributes.Add("style", "display:none");

            if (hfItemID.Value.Trim().Length > 0)
            {
                DataTable dt = new DataAccess.ItemDAO().GetItemPackID(tbItemCode.Text);
                ddlLPurPack.DataSource = dt;
                ddlLPurPack.DataTextField = "Description";
                ddlLPurPack.DataValueField = "Pack_Id";
                ddlLPurPack.DataBind();
            }
            
        }


    }
}
