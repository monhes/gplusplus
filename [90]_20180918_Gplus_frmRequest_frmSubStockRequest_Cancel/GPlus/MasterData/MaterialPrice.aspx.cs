using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class MaterialPrice : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "103";
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
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtMaterialCodeSearch.Text = "";
            txtMaterialNameSearch.Text = "";
            txtPackageName.Text = "";
            ddlStatus.SelectedIndex = 0;
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

        protected void gvMaterialPrice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv["Avg_Cost"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((decimal)drv["Avg_Cost"]).ToString("#,##0.0000");

                e.Row.Cells[5].Text = drv["ItemPack_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Inv_ItemID"].ToString() + "," + drv["Pack_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvMaterialPrice_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                string[] str = e.CommandArgument.ToString().Split(',');
                DataTable dt = new DataAccess.ItemDAO().GetItemPack(str[0], str[1]);
                hdID.Value = str[0];
                hdPackID.Value = str[1];
                hdOldAvg_Cost.Value = "0.0000";
                if (dt.Rows.Count > 0)
                {
                    ItemPackSelectorControl1.Enabled = false;
                    ItemPackSelectorControl1.ItemID = str[0];
                    ItemPackSelectorControl1.ItemPackID = str[1];

                    if (dt.Rows[0]["Avg_Cost"].ToString().Trim().Length > 0)
                    {
                        txtBasePrice.Text = dt.Rows[0]["Avg_Cost"].ToString();
                        hdOldAvg_Cost.Value = dt.Rows[0]["Avg_Cost"].ToString();
                    }

                    if (dt.Rows[0]["Avg_Cost_Date"].ToString().Trim().Length > 0)
                        CalendarControl1.Value = (DateTime)dt.Rows[0]["Avg_Cost_Date"];

                    if (dt.Rows[0]["Selling_Price"].ToString().Trim().Length > 0)
                        txtSalePrice.Text = dt.Rows[0]["Selling_Price"].ToString();

                    txtBarcode.Text = dt.Rows[0]["Barcode_From_Supplier"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["ItemPack_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["ItemPack_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }

                pnlDetail.Visible = true;
            }
        }

        protected void gvMaterialPrice_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvMaterialPrice);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            //if (hdID.Value == "")
            //{
            //    new DataAccess.MaterialDAO().AddMaterialPrice(MaterialSelectorControl1.MaterialID, ddlPackage.SelectedValue, "0", txtBasePrice.Text,
            //        txtSalePrice.Text, CalendarControl1.Value, txtMinQuantity.Text, txtBarcode.Text, status, this.UserID);
            //}
            //else
            //{
            string result = new DataAccess.ItemDAO().InsertLog(this.UserID, "~/MasterData/MaterialPrice.aspx", "btnSave", hdID.Value, hdPackID.Value, "txtBasePrice", hdOldAvg_Cost.Value, txtBasePrice.Text);

            if (Int32.Parse(result == "" ? "0" : result) < 1)
            {
                ShowMessageBox("ไม่สามารถทำการแก้ไขได้");
                return;
            }

            new DataAccess.ItemDAO().UpdateItemPackPrice(hdID.Value, hdPackID.Value, txtBasePrice.Text, CalendarControl1.Value, txtSalePrice.Text,
                txtBarcode.Text, status, this.UserName);

            //}
            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }


        private void BindData()
        {
            DataSet ds = new DataAccess.ItemDAO().GetItemPack(txtMaterialCodeSearch.Text, txtMaterialNameSearch.Text, txtPackageName.Text,
                ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvMaterialPrice.DataSource = ds.Tables[0];
            gvMaterialPrice.DataBind();
        }

        //private void BindMaterialPackage()
        //{
        //    //if (MaterialSelectorControl1.MaterialID.Trim().Length > 0)
        //    //{
        //    //    DataTable dt = new DataAccess.MaterialDAO().GetMaterialPackage(MaterialSelectorControl1.MaterialID);
        //    //    ddlPackage.DataSource = dt;
        //    //    ddlPackage.DataBind();
        //    //    ddlPackage.Items.Insert(0, new ListItem("เลือก Package", ""));

        //    //    DataRow[] drs = dt.Select("IsBasePackage = '1'");
        //    //    if (drs.Length > 0)
        //    //        lblBaseUnitName.Text = drs[0]["Description"].ToString();
        //    //}
        //}

        private void ClearData()
        {
            hdID.Value = "";
            hdPackID.Value = "";
            ItemPackSelectorControl1.ItemID = "";
            ItemPackSelectorControl1.ItemPackID = "";
            txtBasePrice.Text = "";
            CalendarControl1.Text = "";
            txtSalePrice.Text = "";

            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblUpdateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}