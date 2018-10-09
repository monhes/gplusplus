using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.Request
{

    public partial class RequestStockOnHand : Pagebase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "609";
                //BindData();
                BindDropdown();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ItemControl4.ItemID.Trim().Length == 0)
            {
                ShowMessageBox("กรุณาเลือกวัสดุอุปกรณ์");
                return;
            }
            //ShowMessageBox("Code : " + ItemControl4.ItemCode + " Name : " + ItemControl4.ItemName + " Pack : " + ItemControl4.PackName);
            BindData();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            LBStock.DataSource = dt;
            LBStock.DataTextField = "Stock_Name";
            LBStock.DataValueField = "Stock_Id";
            LBStock.DataBind();
            LBStock.Items.Insert(0, new ListItem("ทั้งหมด", ""));
            LBStock.SelectedIndex = 0;

        }


        protected void gvStock_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DataRowView drv = (DataRowView)e.Row.DataItem;
            //    e.Row.Cells[3].Text = drv["Cat_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
            //        "<span style='color:red'>InActive</span>";

            //    ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Cate_ID"].ToString();
            //    ((ImageButton)e.Row.FindControl("btnDetail2")).CommandArgument = drv["Cate_ID"].ToString();
            //    if (drv["Update_Date"].ToString().Trim().Length > 0)
            //        e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            //}
        }


        protected void gvStock_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvStock);
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            gvStock.Visible = false;
            ClearData();
        }

        private void BindData()
        {
            var getSelectedItem = "";
            foreach (int i in LBStock.GetSelectedIndices())
            {
                if (LBStock.Items[i].Value != "")
                {
                    getSelectedItem += LBStock.Items[i].Value + ",";
                }
                else
                {
                    getSelectedItem = "";
                    break;
                }
            }
            //ShowMessageBox(getSelectedItem);

            DataSet ds = new DataAccess.StockDAO().GetStocOnHandkRemaining(ItemControl4.ItemCode, ItemControl4.ItemName,
                ItemControl4.PackName, getSelectedItem, "1", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            if (ds.Tables[0].Rows.Count == 0)
            {
                gvStock.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            else
            {
                gvStock.Visible = true;
                PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

                gvStock.DataSource = ds.Tables[0];
                gvStock.DataBind();
            }
        }


        protected void txtMaterialTypeCodeSearch_TextChanged(object sender, EventArgs e)
        {
            //BindMaterialNameUnit();
        }

        //private void BindMaterialNameUnit()
        //{
        //    txtMaterialNameSearch.Text = "";
        //    ddlMaterialUnit.SelectedIndex = 0;

        //    DataTable dt = new DataAccess.ItemDAO().GetItemSearch2(txtMaterialTypeCodeSearch.Text, txtMaterialNameSearch.Text, 1, 1000, "", "").Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        txtMaterialNameSearch.Text = dt.Rows[0]["Inv_ItemName"].ToString();
        //        ddlMaterialUnit.DataSource = new DataAccess.ItemDAO().GetItemSearch2(txtMaterialTypeCodeSearch.Text, txtMaterialNameSearch.Text, 1, 1000, "", "").Tables[0];
        //        ddlMaterialUnit.DataTextField = "PackName";
        //        ddlMaterialUnit.DataValueField = "PackName";
        //        ddlMaterialUnit.DataBind();
        //        ddlMaterialUnit.Items.Insert(0, new ListItem("เลือกหน่วย", ""));
        //    }
        //    else
        //    {
        //        ShowMessageBox("ไม่พบข้อมูล");
        //    }



        //}

        public void ClearData()
        {
            //txtMaterialTypeCodeSearch.Text = "";
            //txtMaterialNameSearch.Text = "";
            //ddlMaterialUnit.SelectedIndex = 0;
            LBStock.SelectedIndex = 0;
            ItemControl4.Clear();
            //ddlMaterialUnit.Items.Clear();
            //ddlMaterialUnit.Items.Insert(0, new ListItem("เลือกหน่วย", ""));
        }

    }
}