using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class PackageMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "102";
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
            txtPackageSearch.Text = "";
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

        protected void gvPackage_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[2].Text = drv["Pack_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Pack_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[3].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvPackage_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvPackage);
        }

        protected void gvPackage_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.PackageDAO().GetPackage(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if(dt.Rows.Count > 0)
                {
                    txtPackage.Text = dt.Rows[0]["Package_Name"].ToString();
                    rdbStatus.Items[0].Selected = dt.Rows[0]["Pack_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Pack_Status"].ToString() == "0";

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1":"0";
            DataTable dt = new DataAccess.PackageDAO().GetPackage(txtPackage.Text, "", 1, 1000, "", "").Tables[0];
            if (hdID.Value == "")
            {
                if (dt.Rows.Count > 0)
                {
                    this.ShowMessageBox("หน่วยบรรจุ นี้มีอยู่แล้ว กรุณาตรวจสอบ");
                    txtPackage.Focus();
                    return;
                }
                new DataAccess.PackageDAO().AddPackage(txtPackage.Text, status, this.UserName);
            }
            else
            {
                if (dt.Rows.Count > 0 && dt.Rows[0]["Pack_Id"].ToString() != hdID.Value)
                {
                    this.ShowMessageBox("หน่วยบรรจุ นี้มีอยู่แล้ว กรุณาตรวจสอบ");
                    txtPackage.Focus();
                    return;
                }
                new DataAccess.PackageDAO().UpdatePackage(hdID.Value, txtPackage.Text, status, this.UserName);
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

        private void BindData()
        {
            DataSet ds = new DataAccess.PackageDAO().GetPackage(txtPackageSearch.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex,
               PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPackage.DataSource = ds.Tables[0];
            gvPackage.DataBind();
            pnlDetail.Visible = false;
        }

        public void ClearData()
        {
            hdID.Value = "";
            txtPackage.Text = "";
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}