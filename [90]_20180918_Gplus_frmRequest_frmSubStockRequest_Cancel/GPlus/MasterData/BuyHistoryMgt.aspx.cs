using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class BuyHistoryMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "110";
                BindDropdown();
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

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlSupplier.Text = "";
            txtMaterialNameSearch.Text = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
        }

        protected void gvBuyHistory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;


                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["BuyHistory_ID"].ToString();
                
            }
        }

        protected void gvBuyHistory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Edi")
            //{
            //    DataTable dt = new DataAccess.BuyHistoryDAO().GetBuyHistory(e.CommandArgument.ToString());
            //    hdID.Value = e.CommandArgument.ToString();
            //    if (dt.Rows.Count > 0)
            //    {
            //        MaterialSelectorControl1.MaterialID = dt.Rows[0]["Material_ID"].ToString();
            //        BindMaterialPackage();

            //        if (ddlPackage.Items.FindByValue(dt.Rows[0]["MaterialPackage_ID"].ToString()) != null)
            //            ddlPackage.SelectedValue = dt.Rows[0]["MaterialPackage_ID"].ToString();

            //        if (dt.Rows[0]["PresentPrice"].ToString().Trim().Length > 0)
            //            txtPresentPrice.Text = ((decimal)dt.Rows[0]["PresentPrice"]).ToString(this.CurrencyFormat);

            //        if (dt.Rows[0]["PresentDate"].ToString().Trim().Length > 0)
            //            CalendarControl2.Value = (DateTime)dt.Rows[0]["PresentDate"];

            //        txtDiscount.Text = dt.Rows[0]["DiscountMarket"].ToString();

            //        if (dt.Rows[0]["Price"].ToString().Trim().Length > 0)
            //            txtDiscountPrice.Text = ((decimal)dt.Rows[0]["Price"]).ToString(this.CurrencyFormat);

            //        txtGiveup.Text = dt.Rows[0]["GiveAway"].ToString();
            //        txtGiveupUnit.Text = dt.Rows[0]["GiveAwayUnit"].ToString();

            //        lblCreateBy.Text = dt.Rows[0]["Create_ByName"].ToString();
            //        lblUpdateBy.Text = dt.Rows[0]["Update_ByName"].ToString();
            //        if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
            //            lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

            //        if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
            //            lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
            //    }

            //    pnlDetail.Visible = true;
            //}
        }

        protected void gvBuyHistory_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvBuyHistory);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //if (hdID.Value == "")
            //{
            //    new DataAccess.BuyHistoryDAO().AddBuyHistory(MaterialSelectorControl1.MaterialID, ddlPackage.SelectedValue, ddlSupplier.SelectedValue,
            //        txtPresentPrice.Text, CalendarControl2.Value, txtDiscount.Text, txtDiscountPrice.Text, txtGiveup.Text, txtGiveupUnit.Text, "", this.UserID);
            //}
            //else
            //{
            //    new DataAccess.BuyHistoryDAO().UpdateBuyHistory(hdID.Value,MaterialSelectorControl1.MaterialID, ddlPackage.SelectedValue, 
            //        ddlSupplier.SelectedValue, txtPresentPrice.Text, CalendarControl2.Value, txtDiscount.Text, txtDiscountPrice.Text, txtGiveup.Text, 
            //        txtGiveupUnit.Text, "", this.UserID);
            //}
            //ClearData();
            //pnlDetail.Visible = false;
            //BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }

        private void BindDropdown()
        {
            ddlSupplier.DataSource = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "");
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));
        }

        private void BindData()
        {
            //DataSet ds = new DataAccess.BuyHistoryDAO().GetBuyHistory(ddlSupplier.SelectedValue, txtMaterialNameSearch.Text,
            //     PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            //PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            //gvBuyHistory.DataSource = ds.Tables[0];
            //gvBuyHistory.DataBind();
        }

        private void BindMaterialPackage()
        {
            //if (MaterialSelectorControl1.MaterialID.Trim().Length > 0)
            //{
            //    DataTable dt = new DataAccess.MaterialDAO().GetMaterialPackage(MaterialSelectorControl1.MaterialID);
            //    ddlPackage.DataSource = dt;
            //    ddlPackage.DataBind();
            //    ddlPackage.Items.Insert(0, new ListItem("เลือก Package", ""));
            //}
        }

        private void ClearData()
        {
            hdID.Value = "";
            //MaterialSelectorControl1.Clear();
            ddlPackage.Items.Clear();
            CalendarControl1.Text = "";
            CalendarControl2.Text = "";
            txtLastPrice.Text = "";
            txtPresentPrice.Text = "";
            txtDiscount.Text = "";
            txtDiscountPrice.Text = "";

            txtGiveup.Text = "";
            txtGiveupUnit.Text = "";

            lblCreateBy.Text = "";
            lblUpdateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}