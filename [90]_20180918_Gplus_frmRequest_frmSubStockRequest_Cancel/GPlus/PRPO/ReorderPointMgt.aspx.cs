using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class ReorderPointMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "120";
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        public void BindDropdown()
        {
            DataTable dtStock = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlSearchStock.Items.Clear();
            for (int i = 0; i < dtStock.Rows.Count; i++)
            {
                ddlSearchStock.Items.Add(new ListItem(dtStock.Rows[i]["Stock_Code"].ToString() + " - " + dtStock.Rows[i]["Stock_Name"].ToString(),
                    dtStock.Rows[i]["Stock_Id"].ToString()));
            }
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.ReorderPointDAO().GetReorderPoint(ddlSearchStock.SelectedValue, txtSearchProductName.Text,
                txtSearchUnitName.Text, rblType.SelectedValue, txtSearchProductCode.Text, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvReorderPoint.DataSource = ds;
            gvReorderPoint.DataBind();
        }
      

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlSearchStock.SelectedIndex = 0;
            txtSearchProductName.Text = "";
            txtSearchUnitName.Text = "";
        }

        protected void gvReorderPoint_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                HiddenField hdIStockID = (HiddenField)e.Row.FindControl("hdIStockID");
                HiddenField hdIItemID = (HiddenField)e.Row.FindControl("hdIItemID");
                HiddenField hdIPackID = (HiddenField)e.Row.FindControl("hdIPackID");
                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                Image ImgBaseUnitFlag = (Image)e.Row.FindControl("ImgBaseUnitFlag");
                btnDetail.CommandArgument = e.Row.RowIndex.ToString();

                hdIItemID.Value = drv["Inv_ItemID"].ToString();
                hdIPackID.Value = drv["Pack_ID"].ToString();
                hdIStockID.Value = drv["Stock_ID"].ToString();

                if (drv["Reorder_Point"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((decimal)drv["Reorder_Point"]).ToString("0");

                if (drv["Maximum_Qty"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((decimal)drv["Maximum_Qty"]).ToString("0");

                if (drv["Pack_ID"].ToString() == drv["BaseUnit_Pack_ID"].ToString())
                {
                    ImgBaseUnitFlag.Visible = true;
                }
            }
        }

        protected void gvReorderPoint_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                HiddenField hdIStockID = (HiddenField)gvReorderPoint.Rows[index].FindControl("hdIStockID");
                HiddenField hdIItemID = (HiddenField)gvReorderPoint.Rows[index].FindControl("hdIItemID");
                HiddenField hdIPackID = (HiddenField)gvReorderPoint.Rows[index].FindControl("hdIPackID");

                hdStockID.Value = ddlSearchStock.SelectedValue; hdItemID.Value = hdIItemID.Value; 

                //Nin Add
                //hdPackID.Value = hdIPackID.Value;

                DataTable dt = new DataAccess.ReorderPointDAO().GetReorderPoint(hdStockID.Value, hdItemID.Value, hdIPackID.Value);

                DataTable dtPack = new DataAccess.ItemDAO().GetItemPack_ItemSearch(hdIItemID.Value);
                ddlUnitPurchase.DataSource = dtPack;
                ddlUnitPurchase.DataTextField = "Pack_Description";
                ddlUnitPurchase.DataValueField = "Pack_Id";
                ddlUnitPurchase.DataBind();

                ddlPack.DataSource = dtPack;
                ddlPack.DataTextField = "Pack_Description";
                ddlPack.DataValueField = "Pack_Id";
                ddlPack.DataBind();

                pnlDetail.Visible = true;

                if (dt.Rows.Count > 0)
                {
                    txtProductCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();
                    txtProductName.Text = dt.Rows[0]["Inv_ItemName"].ToString();
                    if (ddlPack.Items.FindByValue(hdIPackID.Value) != null)
                        ddlPack.SelectedValue = hdIPackID.Value;
                    ddlPack.Enabled = false;

                    if(dt.Rows[0]["Reorder_Point"].ToString().Trim().Length > 0)
                        txtReorderPoint.Text = ((decimal)dt.Rows[0]["Reorder_Point"]).ToString("0");
                    else
                        txtReorderPoint.Text = "0";

                    if (dt.Rows[0]["Reorder_PointCal"].ToString().Trim().Length > 0)
                        txtCalReorderPoint.Text = ((decimal)dt.Rows[0]["Reorder_PointCal"]).ToString("0");
                    else
                        txtCalReorderPoint.Text = "0";

                    if (dt.Rows[0]["Maximum_Qty"].ToString().Trim().Length > 0)
                        txtMaxStock.Text = ((decimal)dt.Rows[0]["Maximum_Qty"]).ToString("0");
                    else
                        txtMaxStock.Text = "0";

                    if(dt.Rows[0]["Pack_ID"].ToString() == dt.Rows[0]["BaseUnit_Pack_ID"].ToString())
                    {
                        ChkBaseUnit.Checked = true;
                    }
                    else
                    {
                        ChkBaseUnit.Checked = false;
                    }

                    if (ddlUnitPurchase.Items.FindByValue(dt.Rows[0]["Pack_ID_Purchase"].ToString()) != null)
                    {
                        ddlUnitPurchase.SelectedValue = dt.Rows[0]["Pack_ID_Purchase"].ToString();
                        lblOldPack.Text = ddlUnitPurchase.SelectedItem.Text;

                        //Nin Edit
                        hdPackID.Value = ddlUnitPurchase.SelectedValue;
                    }
                    else // Nin Add
                    { 
                        hdPackID.Value = "";
                    }
                    //hdPackID.Value = ddlUnitPurchase.SelectedValue;

                    //chkPack.Checked = dt.Rows[0]["BaseUnit_flag"].ToString() == "1";
                    chkPack.Checked = dt.Rows[0]["Check_PackDS"].ToString() == "1";

                    txtOrderDetail.Text = dt.Rows[0]["Order_Detail"].ToString();

                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }
            }
        }

        protected void gvReorderPoint_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvReorderPoint);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if ((hdPackID.Value == "") || (ddlUnitPurchase.SelectedValue == hdPackID.Value))
            {
                SaveData();
            }
            else
            {
                mpeConfirmSave.Show();
            }
        }

        protected void btnConfirmSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            new DataAccess.ReorderPointDAO().AddReorderPoint(ddlSearchStock.SelectedValue, hdItemID.Value, ddlPack.SelectedValue, txtReorderPoint.Text,
                txtCalReorderPoint.Text, txtMaxStock.Text, ddlUnitPurchase.SelectedValue, "", "", "", chkPack.Checked?"1":"0", this.UserID);
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
            hdItemID.Value = "";
            hdPackID.Value = "";
            hdStockID.Value = "";
            txtProductCode.Text = "";
            txtProductName.Text = "";
            txtMaxStock.Text = "";
            txtCalReorderPoint.Text = "";
            txtReorderPoint.Text = "";
            ddlUnitPurchase.Items.Clear();
        }

    }
}