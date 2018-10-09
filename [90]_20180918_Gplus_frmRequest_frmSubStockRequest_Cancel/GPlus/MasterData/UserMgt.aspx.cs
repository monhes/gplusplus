using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.images
{
    public partial class UserMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "119";
                SetAccessMenu();
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void SetAccessMenu()
        {
            DataTable dt = this.GetAccessMenu(this.PageID, this.UserID);

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Can_Add"].ToString() == "0")
                {
                    btnAdd.Visible = false;
                }

                if (dt.Rows[0]["Can_Update"].ToString() == "0")
                {
                    btnAdd.Visible = false;
                    btnSave.Visible = false;
                }
            }
        } 

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtFirstNameSearch.Text = "";
            txtLastNameSearch.Text = "";
            txtUserNameSearch.Text = "";
            ddlUserGroupSearch.SelectedIndex = 0;
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

        protected void gvUserName_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[5].Text = drv["Account_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Account_Id"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvUserName_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataAccess.UserDAO db = new DataAccess.UserDAO();
                DataTable dt = db.GetAccount(e.CommandArgument.ToString());
                DataTable dtGU = db.GetUserGroupUser(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();

                for (int i = 0; i < dtGU.Rows.Count; i++)
                {
                    if (cblUserGroup.Items.FindByValue(dtGU.Rows[i]["UserGroup_ID"].ToString()) != null)
                        cblUserGroup.Items.FindByValue(dtGU.Rows[i]["UserGroup_ID"].ToString()).Selected = true;
                }
                if (dt.Rows.Count > 0)
                {
                    txtUserName.Text = dt.Rows[0]["Account_UserName"].ToString();
                    txtUserName.ReadOnly = true;
                    txtFirstName.Text = dt.Rows[0]["Account_Fname"].ToString();
                    txtLastName.Text = dt.Rows[0]["Account_Lname"].ToString();
                    txtEmail.Text = dt.Rows[0]["Account_Email"].ToString();
                    rfvPassword.Enabled = false;
                    RequiredFieldValidator4.Enabled = false;

                    //if (ddlUserGroup.Items.FindByValue(dt.Rows[0]["UserGroup_Id"].ToString()) != null)
                    //    ddlUserGroup.SelectedValue = dt.Rows[0]["UserGroup_Id"].ToString();

                    if (ddlDepartment.Items.FindByValue(dt.Rows[0]["OrgStruc_ID"].ToString()) != null)
                        ddlDepartment.SelectedValue = dt.Rows[0]["OrgStruc_ID"].ToString();

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

        protected void gvUserName_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvUserName);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            DataAccess.UserDAO db = new DataAccess.UserDAO();
            if (hdID.Value == "")
            {
                hdID.Value = db.AddAccount("", ddlDepartment.SelectedValue, txtUserName.Text, 
                    Util.EncryptPassword(txtPassword.Text),
                    txtFirstName.Text, txtLastName.Text, txtEmail.Text,ddlStock.SelectedValue, txtExt_No.Text, status, this.UserName);

                if (hdID.Value == "0")
                {
                    hdID.Value = "";
                    this.ShowMessageBox("มี User Name นี้ในระบบแล้ว กรุณาใช้ User Name อื่น");
                    return;
                }
            }
            else
            {
                db.UpdateAccount(hdID.Value, "", ddlDepartment.SelectedValue, ddlStock.SelectedValue,
                    txtFirstName.Text, txtLastName.Text, txtEmail.Text, txtExt_No.Text, status, this.UserName);

                if (txtPassword.Text.Trim().Length > 0)
                    db.UpdateAccount(hdID.Value, Util.EncryptPassword(txtPassword.Text), this.UserName);
                db.DeleteUserGroupUser(hdID.Value);
            }

            for (int i = 0; i < cblUserGroup.Items.Count; i++)
            {
                if (cblUserGroup.Items[i].Selected)
                    db.AddUserGroupUser(hdID.Value, cblUserGroup.Items[i].Value);
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
            DataTable dt = new DataAccess.UserDAO().GetUserGroup("","", "1", 1, 5000, "", "").Tables[0];
            ddlUserGroupSearch.DataSource = dt;
            ddlUserGroupSearch.DataBind();
            ddlUserGroupSearch.Items.Insert(0, new ListItem("เลือกกลุ่มผู้ใช้งาน", ""));

            cblUserGroup.DataSource = dt;
            cblUserGroup.DataBind();
            //ddlUserGroup.DataSource = dt;
            //ddlUserGroup.DataBind();
            //ddlUserGroup.Items.Insert(0, new ListItem("เลือกกลุ่มผู้ใช้งาน", ""));

            dt = new DataAccess.OrgStructureDAO().GetOrgStructure("", "", "","", "1", 1, 9000, "", "").Tables[0];
            string depName = "";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Dep_Code"].ToString().Trim().Length == 0)
                {
                    depName = dt.Rows[i]["Description"].ToString();
                    ddlDepartment.Items.Add(new ListItem(dt.Rows[i]["Div_Code"].ToString() + " - " + dt.Rows[i]["Description"].ToString()
                        , dt.Rows[i]["OrgStruc_Id"].ToString()));
                }
                else
                {
                    ddlDepartment.Items.Add(new ListItem(dt.Rows[i]["Div_Code"].ToString()+" / "+dt.Rows[i]["Dep_Code"].ToString() + " - " + depName + " / " + dt.Rows[i]["Description"].ToString(), dt.Rows[i]["OrgStruc_Id"].ToString()));
                }
            }
            ddlDepartment.Items.Insert(0, new ListItem("เลือกแผนก", ""));

            ddlStock.DataSource = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "");
            ddlStock.DataBind();
            ddlStock.Items.Insert(0, new ListItem("เลือกคลังสินค้า", ""));
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.UserDAO().GetAccount(ddlUserGroupSearch.SelectedValue, "", txtUserNameSearch.Text, txtFirstNameSearch.Text,
                txtLastNameSearch.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvUserName.DataSource = ds.Tables[0];
            gvUserName.DataBind();
        }

        private void ClearData()
        {
            txtUserName.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            ddlDepartment.SelectedIndex = 0;
            ddlStock.SelectedIndex = 0;
            rdbStatus.SelectedIndex = 0;
            hdID.Value = "";
            txtEmail.Text = "";
            txtExt_No.Text = "";
            //ddlUserGroup.SelectedIndex = 0;
            cblUserGroup.SelectedIndex = -1;
        }
    }
}