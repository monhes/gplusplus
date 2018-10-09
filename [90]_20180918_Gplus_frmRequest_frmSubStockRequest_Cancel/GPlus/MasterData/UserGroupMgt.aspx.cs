using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class UserGroupMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "120";
                gvPermission.DataSource = new DataAccess.UserDAO().GetMenuAll();
                gvPermission.DataBind();
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
            txtUserGroupSearch.Text = "";
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
            TabContainer1.ActiveTabIndex = 0;
        }

        protected void gvUserGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["UserGroup_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" : 
                    "<span style='color:red'>InActive</span>";
                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["UserGroup_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvUserGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataAccess.UserDAO user = new DataAccess.UserDAO();
                DataTable dt = user.GetUserGroup(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtUserGroupCode.Text = dt.Rows[0]["UserGroup_Code"].ToString();
                    lblUserGroupName.Text = "ชื่อกลุ่มผู้ใช้งาน : " + dt.Rows[0]["UserGroup_Name"].ToString();
                    txtUserGroup.Text = dt.Rows[0]["UserGroup_Name"].ToString();
                    txtDescription.Text = dt.Rows[0]["UserGroup_Desc"].ToString();
                    rdbStatus.Items[0].Selected = dt.Rows[0]["UserGroup_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["UserGroup_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }
                DataView  dv = user.GetUserGroupMenu(e.CommandArgument.ToString()).DefaultView;

                for (int i = 0; i < gvPermission.Rows.Count; i++)
                {
                    HiddenField hdMenuID = (HiddenField)gvPermission.Rows[i].FindControl("hdMenuID");
                    CheckBox chkCanView = (CheckBox)gvPermission.Rows[i].FindControl("chkCanView");
                    CheckBox chkCanAdd = (CheckBox)gvPermission.Rows[i].FindControl("chkCanAdd");
                    CheckBox chkCanUpdate = (CheckBox)gvPermission.Rows[i].FindControl("chkCanUpdate");
                    CheckBox chkCanApprove = (CheckBox)gvPermission.Rows[i].FindControl("chkCanApprove");

                    dv.RowFilter = "Menu_Id = "+hdMenuID.Value;
                    if (dv.Count > 0)
                    {
                        chkCanView.Checked = dv[0]["Can_View"].ToString() == "1";
                        chkCanAdd.Checked = dv[0]["Can_Add"].ToString() == "1";
                        chkCanUpdate.Checked = dv[0]["Can_Update"].ToString() == "1";
                        chkCanApprove.Checked = dv[0]["Can_Approve"].ToString() == "1";
                    }
                }
                pnlDetail.Visible = true;
                TabContainer1.ActiveTabIndex = 0;
            }
        }

        protected void gvUserGroup_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvUserGroup);
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataAccess.UserDAO user = new DataAccess.UserDAO();
            string status = rdbStatus.SelectedIndex == 0 ? "1":"0";
            string retVal = "";
            if (hdID.Value == "")
            {
                retVal = user.AddUserGroup(txtUserGroupCode.Text,txtUserGroup.Text, txtDescription.Text, status, this.UserName);
                if (retVal != "0") hdID.Value = retVal;
            }
            else
            {
                retVal = user.UpdateUserGroup(hdID.Value, txtUserGroupCode.Text, txtUserGroup.Text, txtDescription.Text, status, this.UserName);
            }
            if (retVal == "0")
            {
                this.ShowMessageBox("มีรหัส "+txtUserGroupCode.Text+" อยู่ในระบบแล้ว");
                TabContainer1.ActiveTabIndex = 0;
                txtUserGroupCode.Focus();
                return;
            }
            

            user.DeleteUserGroupMenu(hdID.Value);
            for (int i = 0; i < gvPermission.Rows.Count; i++)
            {
                HiddenField hdMenuID = (HiddenField)gvPermission.Rows[i].FindControl("hdMenuID");
                CheckBox chkCanView = (CheckBox)gvPermission.Rows[i].FindControl("chkCanView");
                CheckBox chkCanAdd = (CheckBox)gvPermission.Rows[i].FindControl("chkCanAdd");
                CheckBox chkCanUpdate = (CheckBox)gvPermission.Rows[i].FindControl("chkCanUpdate");
                CheckBox chkCanApprove = (CheckBox)gvPermission.Rows[i].FindControl("chkCanApprove");

                user.AddUserGroupMenu(hdMenuID.Value, hdID.Value, chkCanView.Checked ? "1" : "0", chkCanAdd.Checked ? "1" : "0",
                    chkCanUpdate.Checked ? "1" : "0","0", chkCanApprove.Checked ? "1" : "0");
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

        string menuGroupID = "";
        protected void gvPermission_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                HiddenField hdMenuID = (HiddenField)e.Row.FindControl("hdMenuID");
                CheckBox chkCanView = (CheckBox)e.Row.FindControl("chkCanView");
                CheckBox chkCanAdd = (CheckBox)e.Row.FindControl("chkCanAdd");
                CheckBox chkCanUpdate = (CheckBox)e.Row.FindControl("chkCanUpdate");
                CheckBox chkCanApprove = (CheckBox)e.Row.FindControl("chkCanApprove");

                if (drv["MenuGroup_Id"].ToString() != menuGroupID)
                    menuGroupID = drv["MenuGroup_Id"].ToString();
                else
                    e.Row.Cells[0].Text = "";

                hdMenuID.Value = drv["Menu_Id"].ToString();
                chkCanAdd.Visible = drv["Have_Add"].ToString() == "1";
                chkCanUpdate.Visible = drv["Have_Update"].ToString() == "1";
                chkCanApprove.Visible = drv["Have_Approve"].ToString() == "1";
            }
        }


        private void BindData()
        {
            DataSet ds = new DataAccess.UserDAO().GetUserGroup(txtUserGroupCodeSearch.Text,txtUserGroupSearch.Text, ddlStatus.SelectedValue, 
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvUserGroup.DataSource = ds.Tables[0];
            gvUserGroup.DataBind();
            TabContainer1.ActiveTabIndex = 0;
            pnlDetail.Visible = false;
        }

        public void ClearData()
        {
            hdID.Value = "";
            txtDescription.Text = "";
            txtUserGroupCode.Text = "";
            txtUserGroup.Text = "";
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";

            for (int i = 0; i < gvPermission.Rows.Count; i++)
            {
                CheckBox chkCanView = (CheckBox)gvPermission.Rows[i].FindControl("chkCanView");
                CheckBox chkCanAdd = (CheckBox)gvPermission.Rows[i].FindControl("chkCanAdd");
                CheckBox chkCanUpdate = (CheckBox)gvPermission.Rows[i].FindControl("chkCanUpdate");
                CheckBox chkCanApprove = (CheckBox)gvPermission.Rows[i].FindControl("chkCanApprove");
                chkCanAdd.Checked = false;
                chkCanView.Checked = false;
                chkCanUpdate.Checked = false;
                chkCanApprove.Checked = false;
            }
        }

    }
}