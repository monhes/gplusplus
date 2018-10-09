using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class ApproverMgt : Pagebase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "114";
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
            txtDivCodeSearch.Text = "";
            txtDepCodeSearch.Text = "";
            txtDivNameSearch.Text = "";
            txtDepNameSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

     
        protected void gvDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[1].Text = drv["Div_Code"].ToString();
                if (drv["Dep_Code"].ToString().Trim().Length > 0)
                    e.Row.Cells[1].Text += "-" + drv["Dep_Code"].ToString();

                if (drv["Dep_Code"].ToString().ToString().Trim().Length == 0)
                    e.Row.Cells[2].Text = drv["Description"].ToString();
                else
                    e.Row.Cells[3].Text = drv["Description"].ToString();

                e.Row.Cells[5].Text = drv["OrgStruc_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["OrgStruc_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataAccess.OrgStructureDAO db = new DataAccess.OrgStructureDAO();
                DataTable dt = db.GetOrgStructure(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    BindDropdown();
                    txtDivCode.Text = dt.Rows[0]["Div_Code"].ToString();
                    txtDepCode.Text = dt.Rows[0]["Dep_Code"].ToString();

                    txtDeptName.Text = dt.Rows[0]["Description"].ToString();

                    ddlApprovePart_SelectedIndexChanged(this, new EventArgs());
                }

                pnlDetail.Visible = true;
            }
        }

        protected void gvDepartment_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvDepartment);
        }

        protected void ddlApprovePart_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindApprover();
            BindTempApprover();

            DataAccess.OrgStructureDAO db = new DataAccess.OrgStructureDAO();
            DataView dv = db.GetApproveByPass(hdID.Value).DefaultView;
            dv.RowFilter = "Approve_Part = '"+ddlApprovePart.SelectedValue+"' ";
            if (dv.Count > 0)
            {
                rblByPass.SelectedValue = dv[0]["Approve_Flag"].ToString();
                if(dv[0]["Effective_Date"].ToString().Trim().Length > 0)
                    ccByPassFrom.Value = (DateTime)dv[0]["Effective_Date"];
                if (dv[0]["Expire_Date"].ToString().Trim().Length > 0)
                    ccByPassTo.Value = (DateTime)dv[0]["Expire_Date"];
            }
            else
            {
                rblByPass.SelectedIndex = 0;
                ccByPassFrom.Text = "";
                ccByPassTo.Text = "";
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            new DataAccess.OrgStructureDAO().UpdateApproveByPass(hdID.Value, ddlApprovePart.SelectedValue, rblByPass.SelectedValue, ccByPassFrom.Value,
                ccByPassTo.Value, "1", this.UserName);
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
            //DataTable dtUser = new DataAccess.UserDAO().GetAccount("", hdID.Value, "", "", "", "1", 1, 1000, "", "").Tables[0];

            DataTable dtUser = new DataAccess.UserDAO().GetAllAccountByDivCode(hdID.Value, "1", 1, 1000, "", "").Tables[0];

            ddlApprover.Items.Clear();
            ddlTempApprover.Items.Clear();
            ddlApprover.Items.Add(new ListItem("เลือกผู้อนุมัติ", ""));
            ddlTempApprover.Items.Add(new ListItem("เลือกผู้อนุมัติแทน", ""));
            for (int i = 0; i < dtUser.Rows.Count; i++)
            {
                ddlApprover.Items.Add(new ListItem(dtUser.Rows[i]["Account_UserName"].ToString() + " - " + dtUser.Rows[i]["Account_Fname"].ToString() + "  " +
                    dtUser.Rows[i]["Account_Lname"].ToString(), dtUser.Rows[i]["Account_ID"].ToString()));

                ddlTempApprover.Items.Add(new ListItem(dtUser.Rows[i]["Account_UserName"].ToString() + " - " + dtUser.Rows[i]["Account_Fname"].ToString() + "  " +
                    dtUser.Rows[i]["Account_Lname"].ToString(), dtUser.Rows[i]["Account_ID"].ToString()));
            }
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.OrgStructureDAO().GetOrgStructure(txtDivCodeSearch.Text, txtDepCodeSearch.Text,
                txtDivNameSearch.Text, txtDepNameSearch.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize,
                this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvDepartment.DataSource = ds.Tables[0];
            gvDepartment.DataBind();
        }

        private void BindApprover()
        {
            gvApprover.DataSource = new DataAccess.ApproverDAO().GetApprover(hdID.Value, ddlApprovePart.SelectedValue);
            gvApprover.DataBind();
        }

        private void BindTempApprover()
        {
            gvTempApprove.DataSource = new DataAccess.ApproverDAO().GetTempApprover(hdID.Value, ddlApprovePart.SelectedValue);
            gvTempApprove.DataBind();
        }

        private void ClearData()
        {
            hdID.Value = "";
            ddlApprovePart.SelectedIndex = 0;
        }


        protected void gvApprover_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                if (drv["Effective_Date"].ToString().Length > 0)
                    e.Row.Cells[2].Text = ((DateTime)drv["Effective_Date"]).ToString(this.DateFormat);
                if (drv["Expire_Date"].ToString().Length > 0)
                    e.Row.Cells[3].Text = ((DateTime)drv["Expire_Date"]).ToString(this.DateFormat);

                e.Row.Cells[4].Text = drv["Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                hdID.Value = drv["ApproveOrg_ID"].ToString();

                LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
                btnDelete.CommandArgument = drv["ApproveOrg_ID"].ToString();
                btnDelete.OnClientClick = "return confirm('คุณต้องการลบรายการนี้หรือไม่?');";

                if (drv["Update_Date"].ToString().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateFormat);
            }
        }

        protected void gvApprover_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                new DataAccess.ApproverDAO().DeleteApprover(e.CommandArgument.ToString());
                BindApprover();
            }
        }

        protected void btnApproveSave_Click(object sender, EventArgs e)
        {
            string status = rdbApproveStatus.SelectedIndex == 0 ? "1" : "0";
            new DataAccess.ApproverDAO().AddApprover(hdID.Value, ddlApprovePart.SelectedValue, ddlApprover.SelectedValue,
                ccApproveStart.Value, ccApproveEnd.Value, status, this.UserName);

            BindApprover();

            ddlApprover.SelectedIndex = 0;
            ccApproveStart.Text = "";
            ccApproveEnd.Text = "";
            rdbApproveStatus.SelectedIndex = 0;
        }

        protected void btnApproveCancel_Click(object sender, EventArgs e)
        {
            ddlApprover.SelectedIndex = 0;
            ccApproveStart.Text = "";
            ccApproveEnd.Text = "";
            rdbApproveStatus.SelectedIndex = 0;
        }



        protected void gvTempApprove_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString();
                if (drv["Effective_Date"].ToString().Length > 0)
                    e.Row.Cells[2].Text = ((DateTime)drv["Effective_Date"]).ToString(this.DateFormat);
                if (drv["Expire_Date"].ToString().Length > 0)
                    e.Row.Cells[3].Text = ((DateTime)drv["Expire_Date"]).ToString(this.DateFormat);

                e.Row.Cells[5].Text = drv["Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                hdID.Value = drv["TempApprove_ID"].ToString();

                LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
                btnDelete.CommandArgument = drv["TempApprove_ID"].ToString();
                btnDelete.OnClientClick = "return confirm('คุณต้องการลบรายการนี้หรือไม่?');";

                if (drv["Update_Date"].ToString().Length > 0)
                    e.Row.Cells[7].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateFormat);
            }
        }

        protected void gvTempApprove_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                new DataAccess.ApproverDAO().DeleteTempApprover(e.CommandArgument.ToString());
                BindTempApprover();
            }
        }


        protected void btnTempApproveSave_Click(object sender, EventArgs e)
        {
            string status = rdbTempApproveStatus.SelectedIndex == 0 ? "1" : "0";
            new DataAccess.ApproverDAO().AddTempApprover(hdID.Value, ddlApprovePart.SelectedValue, ddlTempApprover.SelectedValue,
                ccTempStart.Value, ccTempEnd.Value, txtReason.Text, status, this.UserName);

            BindTempApprover();

            ddlTempApprover.SelectedIndex = 0;
            ccTempStart.Text = "";
            ccTempEnd.Text = "";
            txtReason.Text = "";
            rdbTempApproveStatus.SelectedIndex = 0;
        }

        protected void btnTempApproveCancel_Click(object sender, EventArgs e)
        {
            ddlTempApprover.SelectedIndex = 0;
            ccTempStart.Text = "";
            ccTempEnd.Text = "";
            txtReason.Text = "";
            rdbTempApproveStatus.SelectedIndex = 0;
        }

    }
}