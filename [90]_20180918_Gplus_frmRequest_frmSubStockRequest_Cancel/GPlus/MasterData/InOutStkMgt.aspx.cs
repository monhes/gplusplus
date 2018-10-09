using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class InOutStkMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "124";
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
            txrReason_DesSearch.Text = "";
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

        protected void gvInOutStk_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[2].Text = drv["InOutStk_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Reason_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[3].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvInOutStk_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.StockDAO().GetInOutStkReasonByID(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtReason_Desc.Text = dt.Rows[0]["Reason_Description"].ToString();

                    rdbInOutStkType.Items[0].Selected = dt.Rows[0]["InOutStk_Type"].ToString() == "I";
                    rdbInOutStkType.Items[1].Selected = dt.Rows[0]["InOutStk_Type"].ToString() == "O";
                    chkIsCal_Avgcost.Checked = dt.Rows[0]["IsCal_AvgCost"].ToString() == "1";
                    rdbStatus.Items[0].Selected = dt.Rows[0]["InOutStk_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["InOutStk_Status"].ToString() == "0";
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

        protected void gvInOutStk_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvInOutStk);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string InOutStkType = rdbInOutStkType.SelectedIndex == 0 ? "I" : "O";
            string IsCal_AvgCost = chkIsCal_Avgcost.Checked == true ? "1" : "0";
            if (hdID.Value == "")
            {
               string result =  new DataAccess.StockDAO().AddInOutStkReason(txtReason_Desc.Text, InOutStkType,IsCal_AvgCost, status, this.UserID);
                if(Convert.ToInt16(result == ""?"0" : result) > 0)
               {
                   ShowMessageBox("บันทึกเรียบร้อย");
               }
            }
            else
            {
                string result =  new DataAccess.StockDAO().UpdateInOutStkReason(hdID.Value,txtReason_Desc.Text, InOutStkType, IsCal_AvgCost, status, this.UserID);
                if(Convert.ToInt16(result == ""?"0" : result) > 0)
               {
                   ShowMessageBox("บันทึกเรียบร้อย");
               }
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
            string InOutStkType = rdbInOutStkTypeSearch.SelectedIndex == 0 ? "I" : "O";

            DataSet ds = new DataAccess.StockDAO().GetInOutStkReason(txrReason_DesSearch.Text,InOutStkType, ddlStatus.SelectedValue, 
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvInOutStk.DataSource = ds.Tables[0];
            gvInOutStk.DataBind();
        }

        public void ClearData()
        {
            hdID.Value = "";
            txtReason_Desc.Text = "";
            chkIsCal_Avgcost.Checked = false;
            rdbInOutStkTypeSearch.SelectedIndex = 0;
            rdbInOutStkType.SelectedIndex = 0;
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}