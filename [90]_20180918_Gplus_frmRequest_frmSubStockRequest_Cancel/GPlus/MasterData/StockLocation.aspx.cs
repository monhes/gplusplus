using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class StockLocation : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "122";
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
            pnlDetail.Visible = false;
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlStock.SelectedIndex = 0;
            ddlStockAdd.SelectedIndex = 0;
            txtLocationCodeSearch.Text = "";
            txtLocationNameSearch.Text = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

            BindDropdownStockAdd();
        }

        protected void gvStockLocation_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["Location_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Location_Id"].ToString();
                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateTimeFormat);

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvStockLocation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                BindDropdownStockAdd();

                DataTable dt = new DataAccess.StockDAO().GetStockLocation(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    hdStockID.Value = dt.Rows[0]["Stock_ID"].ToString();
                    
                    ddlStockAdd.SelectedValue = dt.Rows[0]["Stock_ID"].ToString();
                    ddlStockAdd.Enabled = false;
                    
                    txtLocationCode.Text = dt.Rows[0]["Location_Code"].ToString();
                    txtLocationName.Text = dt.Rows[0]["Location_Name"].ToString();

                    cbDefaultLocation.Checked = dt.Rows[0]["default_flag"].ToString() == "Y" ? true : false;

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Location_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Location_Status"].ToString() == "0";
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

        protected void gvStockLocation_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvStockLocation);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            if (hdID.Value == "")
            {
                DataTable dt = new DataAccess.StockDAO().GetDefaultFlagByStockID(ddlStockAdd.SelectedValue);
                DataRow[] rows = dt.Select("Default_Flag = 'Y'");
                if (rows.Length > 0 && cbDefaultLocation.Checked == true)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('มีการกำหนดสถานที่จัดเก็บสินค้าอัตโนมัติที่อื่นแล้ว');", true);
                    return;
                }
                else 
                {
                    new DataAccess.StockDAO().AddStockLocation(ddlStockAdd.SelectedValue, txtLocationCode.Text, txtLocationName.Text, status, this.UserID, cbDefaultLocation.Checked == true ? "Y" : null);
                }
            }
            else
            {
                DataTable dt = new DataAccess.StockDAO().GetDefaultFlagByStockID(hdStockID.Value);
                DataRow[] rows = dt.Select("Default_Flag = 'Y'");
                if (rows.Length > 0)
                {
                    if (rows[0]["Location_ID"].ToString() != hdID.Value && cbDefaultLocation.Checked == true)
                    {
                        cbDefaultLocation.Checked = false;
                        ScriptManager.RegisterStartupScript(this, GetType(), "script", "alert('มีการกำหนดสถานที่จัดเก็บสินค้าอัตโนมัติที่อื่นแล้ว');", true);
                        return;
                    }
                }

                new DataAccess.StockDAO().UpdateStockLocation(hdID.Value, txtLocationCode.Text, txtLocationName.Text, status, this.UserID, cbDefaultLocation.Checked == true ? "Y" : null);
                
            }
            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }

        private void BindDropdown()
        {
            //DataTable dt = new DataAccess.StockDAO().GetStockFromAccount(this.UserID, 1, 1000, "", "").Tables[0];
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            //ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        private void BindDropdownStockAdd()
        {
            ddlStockAdd.Items.Clear();
            ddlStockAdd.Items.Add(new ListItem("------- กรุณาเลือก -------", ""));
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStockAdd.DataSource = dt;
            ddlStockAdd.DataTextField = "Stock_Name";
            ddlStockAdd.DataValueField = "Stock_Id";
            ddlStockAdd.DataBind();
            ddlStockAdd.Enabled = true;
        }


        private void BindData()
        {
            DataSet ds = new DataAccess.StockDAO().GetStockLocation(ddlStock.SelectedValue, txtLocationCodeSearch.Text,txtLocationNameSearch.Text,"",
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvStockLocation.DataSource = ds.Tables[0];
            gvStockLocation.DataBind();
        }

        public void ClearData()
        {
            hdID.Value = "";
            txtLocationCode.Text = "";
            txtLocationName.Text = "";
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
            cbDefaultLocation.Checked = false;
            ddlStockAdd.Enabled = true;
        }

    }
}