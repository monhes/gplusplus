using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class pop_SupplierAccount : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["sid"] == null)
                {
                    Response.End();
                    return;
                }
                BindData();
            }
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
                e.Row.Cells[6].Text = drv["Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Supplier_Account_ID"].ToString();
                ((LinkButton)e.Row.FindControl("btnDel")).CommandArgument = drv["Supplier_Account_ID"].ToString();
                ((LinkButton)e.Row.FindControl("btnDel")).OnClientClick = "return confirm('คุณต้องการลบรายการนี้หรือไม่?');";

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[7].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                hdID.Value = e.CommandArgument.ToString();
                DataTable dt = new DataAccess.SupplierDAO().GetSupplierAccount(e.CommandArgument.ToString());
                if (dt.Rows.Count > 0)
                {
                    txtFullName.Text = dt.Rows[0]["Account_FullName"].ToString();

                    if (dt.Rows[0]["Expire_Date"].ToString().Trim().Length > 0)
                        CalendarControl1.Value = (DateTime)dt.Rows[0]["Expire_Date"];

                    txtUserName.Text = dt.Rows[0]["Account_UserName"].ToString();
                    txtPassword.Text = dt.Rows[0]["Account_Password"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Status"].ToString() == "0";

                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }
                pnlDetail.Visible = true;
            }
            else if (e.CommandName == "Del")
            {
                new DataAccess.SupplierDAO().DeleteSupplierAccount(e.CommandArgument.ToString());

                BindData();
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
                retValue = new DataAccess.SupplierDAO().AddSupplierAccount(Request["sid"], txtFullName.Text, txtUserName.Text, txtPassword.Text, CalendarControl1.Value,
                    status, this.UserName, txtEmail.Text);
                if (retValue != "0") hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.SupplierDAO().UpdateSupplierAccount(hdID.Value, Request["sid"], txtFullName.Text, txtUserName.Text, txtPassword.Text,
                    CalendarControl1.Value, status, this.UserName, txtEmail.Text);
            }
            if (retValue == "0")
            {
                this.ShowMessageBox("มี User Name "+txtUserName.Text+" อยู่ในระบบแล้ว");
                txtUserName.Focus();
                return;
            }
            ClearData();
            BindData();
            pnlDetail.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearData(); 
            pnlDetail.Visible = false;
        }

        private void BindData()
        {
            gvAccount.DataSource = new DataAccess.SupplierDAO().GetSupplierAccount(Request["sid"],this.SortColumn, this.SortOrder);
            gvAccount.DataBind();
        }

        private void ClearData()
        {
            hdID.Value = "";
            txtFullName.Text = "";
            CalendarControl1.Text = "";
            txtPassword.Text = "";
            txtUserName.Text = "";
            txtEmail.Text = "";
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}