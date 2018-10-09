using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.PRPO.PRPOHelper;

namespace GPlus.PRPO
{
    public partial class PRApproveMgt : Pagebase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "202";
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
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            rdbIsWait.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ccFrom.Text = "";
            ccTo.Text = "";
        }

        protected void gvPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[1].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateFormat);

                if (drv["Net_Amonut"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((decimal)drv["Net_Amonut"]).ToString(this.CurrencyFormat);

                switch (drv["Status"].ToString())
                {
                    case "0": e.Row.Cells[7].Text = "ยกเลิก"; break;
                    case "1":
                        if (drv["Consider_Type"].ToString() == "2")
                        {
                            e.Row.Cells[7].Text = "ส่งกลับไปแก้ไข"; 
                        }else{
                            e.Row.Cells[7].Text = "รออนุมัติ"; 
                        }
                       
                        break;
                    case "2": e.Row.Cells[7].Text = "อนุมัติ"; break;
                    case "3": e.Row.Cells[7].Text = "ไม่อนุมัติ"; break;
                    case "4": e.Row.Cells[7].Text = "จัดซื้อไม่ดำเนินการ"; break;
                    case "5": e.Row.Cells[7].Text = "ออก PO แล้ว"; break;
                }

                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                btnDetail.CommandArgument = drv["PR_ID"].ToString();
                ImageButton btnPrint = (ImageButton)e.Row.FindControl("btnPrint");
            }
        }

        protected void gvPR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                if (rdbIsWait.SelectedValue == "1")
                {
                    rblApproverStatus.SelectedIndex = -1;
                }

                string prId = e.CommandArgument.ToString();

                PRPOSession.Action = PRPOAction.VIEW_PR;
                PRPOSession.PrID = prId;
                PRPOSession.UserID = null;
                PRControl1.BindPR(prId);
                PRControl1.Disable();

                DataTable dt = new DataAccess.PRDAO().GetPRForm1(prId);
                if (dt.Rows.Count > 0)
                {
                    hfPrStatus.Value = dt.Rows[0]["Status"].ToString();
                    hfPrID.Value = prId;
                }

                DataSet dsApprover = new DataAccess.ApproverDAO().GetApproverAndTemp(this.OrgID, "1");

                ddlApprover.Items.Clear();
                ddlApprover.DataSource = dsApprover.Tables[0];
                ddlApprover.DataBind();
                ddlApprover.Items.Insert(0, new ListItem("เลือกผู้อนุมัติ", ""));
                pnlDetail.Visible = true;
                if (hfPrStatus.Value.IndexOf("1") > -1)
                {
                    if (dsApprover.Tables[0].Select("Approve_ID = " + this.UserID).Length > 0)
                    {
                        lblHeader.Text = "ผู้บริหารอนุมัติ";
                        pnlApprover.Visible = true;   
                        pnlTempApprove.Visible = false;
                        txtApproverName.Text = this.FirstName + "  " + this.LastName;
                        txtDateTime.Text = DateTime.Now.ToString(DateTimeFormat);;
                        txtReason.Text = "";
                    }
                    else if (dsApprover.Tables[1].Select("Account_ID = " + this.UserID).Length > 0)
                    {
                        lblHeader.Text = "อนุมัติแทน";
                        pnlApprover.Visible = false;
                        pnlTempApprove.Visible = true;
                        txtTempApprover.Text = this.FirstName + "  " + this.LastName;
                        txtTempApproverDate.Text = DateTime.Now.ToString(DateTimeFormat);;
                        txtTempReason.Text = "";
                    }
                    else 
                    {
                        pnlApprover.Visible = false;
                        pnlTempApprove.Visible = false;
                    }
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["Temp_Approver_ID"].ToString().Trim().Length == 0)
                        {
                            //rblApproverStatus.SelectedIndex = 1;
                            pnlApprover.Visible = true;
                            pnlTempApprove.Visible = false;
                            lblHeader.Text = "ผู้บริหารอนุมัติ";
                            txtReason.Text = dt.Rows[0]["Reason"].ToString();
                            txtApproverName.Text = dt.Rows[0]["Approver_FullName"].ToString();

                            String considerTypes = dt.Rows[0]["Consider_Type"].ToString();

                            if (considerTypes.Length == 0)
                            {
                                rblApproverStatus.SelectedIndex = -1;
                            }
                            else
                            {
                                rblApproverStatus.SelectedValue = dt.Rows[0]["Consider_Type"].ToString();
                            }

                            if (dt.Rows[0]["Approve_Date"].ToString().Trim().Length > 0)
                                txtDateTime.Text = ((DateTime)dt.Rows[0]["Approve_Date"]).ToString(this.DateTimeFormat);
                            btnApproverSave.Enabled = false;
                        }
                        else
                        {
                            pnlApprover.Visible = false;
                            pnlTempApprove.Visible = true;
                            lblHeader.Text = "อนุมัติแทน";
                            ddlApprover.SelectedValue = dt.Rows[0]["Approver_ID"].ToString();
                            //chetjung
                            //rblApproverStatus.SelectedValue = dt.Rows[0]["Consider_Type"].ToString();
                            rblTempApproverStatus.SelectedValue = dt.Rows[0]["Consider_Type"].ToString();

                            txtTempApprover.Text = dt.Rows[0]["Temp_Approver_FullName"].ToString();

                            if (dt.Rows[0]["Temp_Approver_Date"].ToString().Trim().Length > 0)
                                txtTempApproverDate.Text = ((DateTime)dt.Rows[0]["Temp_Approver_Date"]).ToString(this.DateTimeFormat);
                            txtTempReason.Text = dt.Rows[0]["Temp_Reason"].ToString();
                            btnTempApproverSave.Enabled = false;
                        }

                        if (dt.Rows[0]["Status"].ToString() == "1" || dt.Rows[0]["Status"].ToString() == "2" || dt.Rows[0]["Status"].ToString() == "3")
                        {
                            btnApproverSave.Enabled = true;
                            btnTempApproverSave.Enabled = true;
                        }
                        else
                        {
                            btnApproverSave.Enabled = false;
                            btnTempApproverSave.Enabled = false;
                        }
                    }
                }

            }
        }

        protected void gvPR_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvPR);
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.PRDAO().GetPRForm1("", "", this.OrgID, "", "", ddlStatus.SelectedValue, rdbIsWait.SelectedValue,
                ccFrom.Text, ccTo.Text, ddlStatus.SelectedValue, "1", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPR.DataSource = ds.Tables[0];
            gvPR.DataBind();
        }

        protected void btnApproverConfirmSave_Click(object sender, EventArgs e)
        {
            DataTable dtUser = new DataAccess.UserDAO().GetAccount(this.UserName, Util.EncryptPassword(txtApproverPassword.Text)).Tables[0];

            if (dtUser.Rows.Count > 0)
            {
                new DataAccess.PRDAO().UpdatePRForm1Approve(
                    hfPrID.Value, rblApproverStatus.SelectedValue, txtReason.Text, this.UserID, DateTime.Now, "", DateTime.MinValue, "");
                BindData();
                pnlApprover.Visible = false;
                pnlDetail.Visible = false;
                rblApproverStatus.SelectedIndex = -1;
            }
            else
            {
                this.ShowMessageBox("รหัสผ่านไม่ถูกต้อง");
            }
        }

        protected void btnCancelSave_Click(object sender, EventArgs e)
        {
            rblApproverStatus.Items.Clear();
            pnlApprover.Visible = false;
            pnlDetail.Visible = false;
        }

        protected void btnTempApproverCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            pnlTempApprove.Visible = false;
        }

        protected void btnTempApproverConfirmSave_Click(object sender, EventArgs e)
        {
            DataTable dtUser = new DataAccess.UserDAO().GetAccount(this.UserName, Util.EncryptPassword(txtTempApprovePassword.Text)).Tables[0];

            if (dtUser.Rows.Count > 0)
            {
                new DataAccess.PRDAO().UpdatePRForm1Approve(hfPrID.Value, "2", "", ddlApprover.SelectedValue, DateTime.MinValue,
                this.UserID, DateTime.Now, txtTempReason.Text);
                new DataAccess.PRDAO().UpdatePRForm1Approve(hfPrID.Value, rblTempApproverStatus.SelectedValue, "", ddlApprover.SelectedValue, DateTime.MinValue,
                this.UserID, DateTime.Now, txtTempReason.Text);
                BindData();
                pnlDetail.Visible = false;
                pnlTempApprove.Visible = false;
            }
            else
            {
                this.ShowMessageBox("รหัสผ่านไม่ถูกต้อง");
            }
        }
    }
}