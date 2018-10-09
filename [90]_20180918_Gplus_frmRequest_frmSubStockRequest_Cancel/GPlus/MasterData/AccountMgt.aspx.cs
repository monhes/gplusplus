using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class AccountMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "117";
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
            txtAccountNameSearch.Text = "";
            txtAccountNumberSearch.Text = "";
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

        protected void gvAccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["AccExpense_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["AccExpense_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.AccountExpenseDAO().GetAccountExpense(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtAccountName.Text = dt.Rows[0]["AccExpense_Name"].ToString();
                    txtAccountNumber.Text = dt.Rows[0]["AccExpense_Code"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["AccExpense_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["AccExpense_Status"].ToString() == "0";
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

        protected void gvAccount_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvAccount);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retValue = "";
            if (hdID.Value == "")
            {
                retValue = new DataAccess.AccountExpenseDAO().AddAccountExpense(txtAccountNumber.Text, txtAccountName.Text, status, this.UserName);
                if (retValue == "0") hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.AccountExpenseDAO().UpdateAccountExpense(hdID.Value, txtAccountNumber.Text, txtAccountName.Text, status, this.UserName);
            }
            if (retValue == "0")
            {
                this.ShowMessageBox("มีเลขที่บัญชี " + txtAccountNumber.Text + " อยู่ในระบบแล้ว");
                txtAccountNumber.Focus();
                return;
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



        private void BindData()
        {
            DataSet ds = new DataAccess.AccountExpenseDAO().GetAccountExpense(txtAccountNumberSearch.Text, txtAccountNameSearch.Text, ddlStatus.SelectedValue,
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvAccount.DataSource = ds.Tables[0];
            gvAccount.DataBind();
        }

        private void ClearData()
        {
            hdID.Value = "";
            txtAccountName.Text = "";
            txtAccountNumber.Text = "";
            rdbStatus.SelectedIndex = 0;
        }
    }
}