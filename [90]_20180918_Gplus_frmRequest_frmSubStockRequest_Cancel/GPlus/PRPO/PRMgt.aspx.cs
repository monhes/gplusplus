using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.PRPO.PRPOHelper;
using System.IO;

namespace GPlus.PRPO
{
    public partial class PRMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "201";
                BindData();

                PRPOSession.UserID = this.UserID;
            }

            if (Request["showDetail"] == "true")
                pnlDetail.Visible = true;

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PRControl1_DeletePR(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            BindData();
        }

        void PRControl1_CancelPR(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }

        void PRControl1_SavePR(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            BindData();
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
            txtPRCodeSearch.Text = "";
            ddlPRType.SelectedIndex = 0;
            ccFrom.Text = "";
            ccTo.Text = "";
        }

        protected void gvPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (drv["PR_Type"].ToString() == "1")
                    e.Row.Cells[3].Text = "ขอซื้อ";
                else if(drv["PR_Type"].ToString() == "2")
                    e.Row.Cells[3].Text = "ขอจ้าง";

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateFormat);

                if (drv["Net_Amonut"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((decimal)drv["Net_Amonut"]).ToString(this.CurrencyFormat);

                ((ImageButton)e.Row.FindControl("btnPrint")).OnClientClick = "open_popup('pop_PR.aspx?id=" + drv["PR_ID"].ToString()
                + "', 500, 270, 'pop', 'yes', 'yes', 'yes'); return false;";

                switch (drv["Status"].ToString())
                {
                    case "0": e.Row.Cells[8].Text = "ยกเลิก"; break;
                    case "1":
                        if (drv["Consider_Type"].ToString() == "2")
                        {
                            e.Row.Cells[8].Text = "ส่งกลับไปแก้ไข";
                        }
                        else
                        {
                            e.Row.Cells[8].Text = "รออนุมัติ";
                        }
                      
                        break;
                    case "2": e.Row.Cells[8].Text = "อนุมัติ"; break;
                    case "3": e.Row.Cells[8].Text = "ไม่อนุมัติ"; break;
                    case "4": e.Row.Cells[8].Text = "จัดซื้อไม่ดำเนินการ"; break;
                    case "5": e.Row.Cells[8].Text = "ออก PO แล้ว"; break;
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
                //Button btnDeleteItem = PRControl1.FindControl("btnDeleteItem") as Button;
                //HiddenField hdStatus = PRControl1.FindControl("hdStatus") as HiddenField;

                //Button btnRecal = PRControl1.FindControl("btnReCal") as Button;
                //btnRecal.Visible = false;
                //pnlDetail.Visible = true;

                string prId = e.CommandArgument.ToString();

                PRPOSession.Action = PRPOAction.VIEW_PR;    // Use in UploadFile.aspx.cs
                PRPOSession.PrID = prId;                    // Use in UploadFile.aspx.cs
                PRPOSession.PoID = null;
                PRControl1.BindPR(prId);

                pnlDetail.Visible = true;

                //if (hdStatus.Value == "1")
                //    btnDeleteItem.Visible = true;
                //else
                //    btnDeleteItem.Visible = false;
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
            DataSet ds = new DataAccess.PRDAO().GetPRForm1(txtPRCodeSearch.Text, ddlPRType.SelectedValue, this.OrgID, ccFrom.Text, ccTo.Text,
                "", "", "", "", "", "0", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPR.DataSource = ds.Tables[0];
            gvPR.DataBind();
        }

        private void ClearUploadPRTmp()
        {
            string path = Server.MapPath(Path.Combine(PRPOPath.PRTmpUpload, UserID));

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    try { File.Delete(file); }
                    catch (Exception) { }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = true;
            PRPOSession.Action = PRPOAction.ADD_PR;
            PRPOSession.PrID = null;
            PRPOSession.PoID = null;
            PRPOSession.InitializePRPurchase();

            ClearUploadPRTmp();

            Response.Write("<script>window.location.href = '../PRPO/PRMgt.aspx?showDetail=true';</script>");
        }
    }
}