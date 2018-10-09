using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;
using GPlus.PRPO.PRPOHelper;

namespace GPlus.PRPO
{
    public partial class POApproveMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "302";
                BindData();

                ddlStatus.SelectedIndex = 0;
                ddlStatus.Enabled = false;
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
            ddlSupplier.SelectedIndex = 0;
            rdbIsWait.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ccFrom.Text = "";
            ccTo.Text = "";
            tbPoCode.Text = "";
        }
        
        private void BindData()
        {
            txtReason.Text = "";

            DataSet ds = new DataAccess.PODAO()
                        .GetPOForm1(
                            tbPoCode.Text,                      // poCode
                            "",                                 // poType
                            "",                                 // typeInvAsset
                            "",                                 // poDateStart
                            "",                                 // poDateEnd
                            ccFrom.Text,                        // createDateStart
                            ccTo.Text,                          // createDateEnd
                            "",                                 // haveUpload
                            "",                                 // supplierID
                            // feel free to edit these two values.
                            // according to store procedure 'sp_Inv_PO_Form1_SelectPaging' 
                            // the procedure is shared with 
                            //      POMgt.aspx, POSearch.aspx and POApproveMgt.aspx
                            // this two values use only with this page (POApproveMgt.aspx).
                            // so don't worry to edit
                            ddlStatus.SelectedValue,            // status "" = ทั้งหมด, "0" = อนุมัติ, "1" = ไม่อนุมัติ, "2" = ตรวจสอบ PO
                            rdbIsWait.SelectedValue,            // isWait
                            // -- end --
                            PagingControl1.CurrentPageIndex,    // pageNum
                            PagingControl1.PageSize,            // pageSize
                            this.SortColumn,                    // sortField
                            this.SortOrder                      // sortOrder
                        );

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPO.DataSource = ds.Tables[0];
            gvPO.DataBind();

            if (PagingControl1.RecordCount > 0)
                tbPoCode.Text = "";
        }

        protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
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
                    case "1": e.Row.Cells[7].Text = "รออนุมัติ"; break;
                    case "2": e.Row.Cells[7].Text = "อนุมัติ"; 
                        DataTable dt = new DataAccess.DatabaseHelper().ExecuteQuery("SELECT COUNT(*) AS RowNum FROM Inv_Receive_Stk WHERE PO_ID = " + drv["PO_ID"].ToString() + " AND Status <> '0'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["RowNum"].ToString() != "0")
                                e.Row.Cells[7].Text = "รับเข้าคลังแล้ว";
                        }
                        break;
                    case "3": e.Row.Cells[7].Text = "ไม่อนุมัติ"; break;
                    case "4": e.Row.Cells[7].Text = "ตรวจสอบ PO"; break;
                    case "5": e.Row.Cells[7].Text = "ไม่ดำเนินการสั่งซื้อ"; break;
                    case "U": e.Row.Cells[7].Text = "Upload PO"; break;
                    case "D": e.Row.Cells[7].Text = "Download PO"; break;
                }

                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                btnDetail.CommandArgument = drv["PO_ID"].ToString();
                ImageButton btnPrint = (ImageButton)e.Row.FindControl("btnPrint");
            }
        }

        protected void gvPO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                string poID = e.CommandArgument.ToString();

                DataTable dt = new DataAccess.PODAO().GetPOForm1(poID);
                DataSet dsApprover = new DataAccess.ApproverDAO().GetApproverAndTemp(this.OrgID, "2");

                PRPOSession.Action = PRPOAction.VIEW_PO;
                PRPOSession.PoID = poID;
                PRPOSession.UserID = null;

                POControl1.BindPO(poID);
                POControl1.DisableDetail();
                POControl1.DisableButtons();

                if (dt.Rows.Count > 0)
                {
                    hfPoStatus.Value = dt.Rows[0]["Status"].ToString();
                    hfPoID.Value = poID;
                }

                ddlApprover.Items.Clear();
                ddlApprover.DataSource = dsApprover.Tables[0];
                ddlApprover.DataBind();
                ddlApprover.Items.Insert(0, new ListItem("เลือกผู้อนุมัติ", ""));
                pnlDetail.Visible = true;

                if (hfPoStatus.Value.IndexOf("1") > -1)
                {
                    if (dsApprover.Tables[0].Select("Approve_ID = " + this.UserID).Length > 0)
                    {
                        trTemp.Visible = false;
                        ddlApprover.Visible = false;
                        txtApproverName.Visible = true;
                        RequiredFieldValidator1.Visible = false;

                        txtApproverName.Text = this.FirstName + "  " + this.LastName;
                        txtDateTime.Text = "";
                        txtReason.Text = "";
                    }
                    else if (dsApprover.Tables[1].Select("Account_ID = " + this.UserID).Length > 0)
                    {
                        trTemp.Visible = true;
                        ddlApprover.Visible = true;
                        RequiredFieldValidator1.Visible = true;
                        txtApproverName.Visible = false;

                        txtTempApprover.Text = this.FirstName + "  " + this.LastName;
                        txtTempApproverDate.Text = "";
                    }

                    rblApproverStatus.ClearSelection();
                }
                else
                {
                    //DataTable dt = new DataAccess.PODAO().GetPOForm1(e.CommandArgument.ToString());
                    if (dt.Rows.Count > 0)
                    {
                        //if(rblApproverStatus.Items.FindByValue(POControl1.Status) != null)
                        //  rblApproverStatus.SelectedValue = POControl1.Status;

                        if (hfPoStatus.Value == "0")               // ยกเลิก
                            rblApproverStatus.ClearSelection();
                        else if (hfPoStatus.Value == "2")          // อนุมัติ
                            rblApproverStatus.SelectedValue = "0";
                        else if (hfPoStatus.Value == "3")          // ไม่อนุมัติ
                            rblApproverStatus.SelectedValue = "1";
                        else if (hfPoStatus.Value == "4")          // ตรวจสอบ PO
                            rblApproverStatus.SelectedValue = "2";

                        if (dt.Rows[0]["Temp_Approver_ID"].ToString().Trim().Length == 0)
                        {
                            trTemp.Visible = false;
                            ddlApprover.Visible = false;
                            txtApproverName.Visible = true;
                            RequiredFieldValidator1.Visible = false;

                            lblHeader.Text = "ผู้บริหารอนุมัติ";
                            txtReason.Text = dt.Rows[0]["Reason"].ToString();
                            txtApproverName.Text = dt.Rows[0]["Approver_FullName"].ToString();
                            if (dt.Rows[0]["Approve_Date"].ToString().Trim().Length > 0)
                                txtDateTime.Text = ((DateTime)dt.Rows[0]["Approve_Date"]).ToString(this.DateTimeFormat);
                            btnApproverSave.Enabled = false;
                        }
                        else
                        {
                            trTemp.Visible = true;
                            ddlApprover.Visible = true;
                            RequiredFieldValidator1.Visible = true;
                            txtApproverName.Visible = false;

                            lblHeader.Text = "อนุมัติแทน";
                            ddlApprover.SelectedValue = dt.Rows[0]["Approver_ID"].ToString();
                            txtTempApprover.Text = dt.Rows[0]["Temp_Approver_FullName"].ToString();
                            txtReason.Text = dt.Rows[0]["Reason"].ToString();

                            if (dt.Rows[0]["Temp_Approver_Date"].ToString().Trim().Length > 0)
                                txtTempApproverDate.Text = ((DateTime)dt.Rows[0]["Temp_Approver_Date"]).ToString(this.DateTimeFormat);
                            btnApproverSave.Enabled = false;
                        }

                        btnApproverSave.Enabled = false;


                    }
                }

                if (hfPoStatus.Value == "1" || hfPoStatus.Value == "2" ||
                    hfPoStatus.Value == "3" || hfPoStatus.Value == "4")
                {
                    btnApproverSave.Visible = true;
                    btnApproverSave.Enabled = true;

                    if (hfPoStatus.Value == "2")
                    {
                        DataTable table = new DataAccess.DatabaseHelper().ExecuteQuery("SELECT COUNT(*) AS RowNum FROM Inv_Receive_Stk WHERE PO_ID = " + hfPoID.Value + " AND Status <> '0'").Tables[0];
                        if (table.Rows.Count > 0)
                        {
                            if (table.Rows[0]["RowNum"].ToString() != "0")
                                btnApproverSave.Visible = false;
                        }
                    }
                }
                else
                    btnApproverSave.Visible = false;

            }
        }

        protected void gvPO_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvPO);
        }

        protected void btnApproverSave_Click(object sender, EventArgs e)
        {
            DataTable dtUser = new DataAccess.UserDAO().GetAccount(this.UserName, Util.EncryptPassword(txtApproverPassword.Text)).Tables[0];

            if (dtUser.Rows.Count > 0)
            {
                string considerType = rblApproverStatus.SelectedValue;
                string old_considerType = hfPoStatus.Value;

                if (!trTemp.Visible)
                    new DataAccess.PODAO().POForm1Update(hfPoID.Value, rblApproverStatus.SelectedValue, txtReason.Text,
                        this.UserID, DateTime.Now, "", DateTime.MinValue, "", considerType);
                else
                    new DataAccess.PODAO().POForm1Update(hfPoID.Value, rblApproverStatus.SelectedValue, txtReason.Text,
                        "", DateTime.Now, this.UserID, DateTime.MinValue, "", considerType);

                //case อนุมัติ
                if (considerType == "0")
                {
                    new DataAccess.PODAO().POApprove(hfPoID.Value, this.UserID);
                }
                //case เดิมสถานะเป็นอนุมัติ แล้วเลือกสถานะเป็น ไม่อนุมัติ หรือ ตรวจสอบ PO
                else if ((old_considerType == "2") && (considerType == "1" || considerType == "2"))
                {
                    new DataAccess.PODAO().POReject(hfPoID.Value, this.UserID);
                }

                rblApproverStatus.SelectedIndex = -1;
                BindData();
                pnlDetail.Visible = false;
            }
            else
            {
                this.ShowMessageBox("รหัสผ่านไม่ถูกต้อง");
            }
        }

        protected void btnCancelSave_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }

        protected void rdbIsWait_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdbIsWait.SelectedValue == "0")
            {
                ddlStatus.Enabled = true;
                //ddlStatus.Items[0].Enabled = false;
            }
            else
            {
                ddlStatus.Enabled = false;
                ddlStatus.Items[0].Enabled = true;
                ddlStatus.SelectedIndex = 0;
            }
        }

    }
}