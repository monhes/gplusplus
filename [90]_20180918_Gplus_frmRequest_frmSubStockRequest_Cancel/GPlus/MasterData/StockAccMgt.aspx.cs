using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using GPlus.DataAccess;

namespace GPlus.MasterData
{
    public partial class StockAccMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "126";
                BindData();
                BindDropdown();
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindDropdown()
        {
            DataTable dt = new StockDAO().GetStock();


            ddlStock.DataSource = dt;
            ddlStock1.DataSource = dt;

            ddlStock.DataBind();
            ddlStock.Items.Clear();
            ddlStock.Items.Insert(0, new ListItem("--เลือกคลังสินค้า--", ""));


            ddlStock1.DataBind();
            ddlStock1.Items.Clear();
            ddlStock1.Items.Insert(0, new ListItem("--เลือกคลังสินค้า--", ""));


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                ddlStock.Items.Add(new ListItem(dt.Rows[i]["Stock_ID"].ToString() + " - " + dt.Rows[i]["Stock_Name"].ToString(),
                    dt.Rows[i]["Stock_ID"].ToString()));


                ddlStock1.Items.Add(new ListItem(dt.Rows[i]["Stock_ID"].ToString() + " - " + dt.Rows[i]["Stock_Name"].ToString(),
                    dt.Rows[i]["Stock_ID"].ToString()));
            }

        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtAccFName.Text = "";
            txtAccLName.Text = "";

            ClearData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            pnlDetail.Visible = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            hdStatus.Value = "add";
            pnlDetail.Visible = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "script"
                , "window.location = 'StockAccMgt.aspx#pnlDetail'"
                , true
            );
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = e.Row.DataItem as DataRowView;

                string status = drv["stock_account_status"].ToString();
                if (status == "1")
                {
                    e.Row.Cells[5].Text = "<span style='color:navy'>Active</span>";
                }
                else if (status == "0")
                {
                    e.Row.Cells[5].Text = "<span style='color:red'>InActive</span>";
                }

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Stock_Account_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataAccess.AccountDAO user = new DataAccess.AccountDAO();
                DataTable dt = user.GetAccountID(e.CommandArgument.ToString());
                hdStatus.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {

                    AccountNameControl1.AccName = dt.Rows[0]["FullName"].ToString();
                    AccountNameControl1.AccId = dt.Rows[0]["Account_ID"].ToString();
                    ddlStock1.SelectedValue = dt.Rows[0]["Stock_ID"].ToString();
                    rdbStatus.Items[0].Selected = dt.Rows[0]["Stock_Account_Status"].ToString() == "True";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Stock_Account_Status"].ToString() == "False";
                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();

                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);

                }
            }
            pnlDetail.Visible = true;

        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(GridView1);
        }

        public void ClearData()
        {
            hdStatus.Value = "";
            txtAccFName.Text = "";
            txtAccLName.Text = "";
            AccountNameControl1.AccName = "";
            AccountNameControl1.AccId = "";
            ddlStock.SelectedIndex = 0;
            ddlStock1.SelectedIndex = 0;
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlStock1.SelectedValue == "")
            {
                this.ShowMessageBox("กรุณาเลือกคลังสินค้า");
                ddlStock1.Focus();
                return;
            }

            if (AccountNameControl1.AccName == "")
            {
                this.ShowMessageBox("กรุณาเลือกผู้ใช้งาน");
                AccountNameControl1.Focus();
                return;
            }

            string status = rdbStatus.SelectedIndex == 0 ? "True" : "False";
            DataAccess.AccountDAO ds = new DataAccess.AccountDAO();
            if (hdStatus.Value == "add")
            {
                string result = ds.AddStockAcc(AccountNameControl1.AccId, ddlStock1.SelectedValue, status, this.UserID);
                if (result == "1")
                {
                    ShowMessageBox("บันทึกข้อมูลเรียบร้อย");
                }
                else
                {
                    ShowMessageBox("ไม่สามารถเพิ่มข้อมูลได้ เนื่องจากมีผู้ใช้งาน และ คลังสินค้านี้อยู่ในระบบแล้ว");
                }
            }
            else
            {
                string result = ds.UpdateStockAcc(hdStatus.Value, AccountNameControl1.AccId, ddlStock1.SelectedValue, status, this.UserID);
                if (result == "1")
                {
                    ShowMessageBox("บันทึกข้อมูลเรียบร้อย");
                }
                else
                {
                    ShowMessageBox("ไม่สามารถแก้ไขข้อมูลได้ เนื่องจากมีผู้ใช้งาน และ คลังสินค้านี้อยู่ในระบบแล้ว");
                }
            }

            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        public void BindData()
        {

            string StockID = ddlStock.SelectedValue;
            string Fname = txtAccFName.Text;
            string Lname = txtAccLName.Text;


            DataSet ds = new AccountDAO().GetAccountStcc(StockID, Fname, Lname, 
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();
        }


    }
}