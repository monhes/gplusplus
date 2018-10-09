using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.MasterData
{
    public partial class AmphurMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "130";
                BindDropdown();
                BindData();
       
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.AmphurDAO().GetAmphur(ddlProvince.SelectedValue, txtAmphur.Text,
               PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvAmphur.DataSource = ds.Tables[0];
            gvAmphur.DataBind();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.AmphurDAO().GetAmphurr();

            ddlProvince.DataSource = dt;
            ddlProvince.DataTextField = "ProvinceName";
            ddlProvince.DataValueField = "ProvinceID";
            ddlProvince.DataBind();
            ddlProvince.Items.Insert(0, new ListItem("--เลือกจังหวัด--", ""));

            ddllProvince.DataSource = dt;
            ddllProvince.DataTextField = "ProvinceName";
            ddllProvince.DataValueField = "ProvinceID";
            ddllProvince.DataBind();
            ddllProvince.Items.Insert(0, new ListItem("--เลือกจังหวัด--", ""));
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlProvince.SelectedIndex = 0;
            txtAmphur.Text = "";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "script"
                , "window.location = 'AmphurMgt.aspx#panel'"
                , true
            );
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retValue = "";
            if (ddllProvince.SelectedValue == "")
            {
                this.ShowMessageBox("กรุณาเลือกจังหวัด");
                ddllProvince.Focus();
                return;
            }
            if (hdID.Value == "")
            {

                retValue = new DataAccess.AmphurDAO().AddAmphur(tbAmphurName.Text, tbAmphurCode.Text,ddllProvince.SelectedValue, Status, this.UserID);
                if (retValue != "0") hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.AmphurDAO().UpdateAmphur(hdID.Value, tbAmphurName.Text, tbAmphurCode.Text, ddllProvince.SelectedValue, Status, this.UserID);
            }
            if (retValue == "0")
            {
                this.ShowMessageBox("มีอำเภอ " + tbAmphurName.Text + " อยู่ในระบบแล้ว");
                tbAmphurName.Focus();
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

        public void ClearData()
        {
            hdID.Value = "";
            tbAmphurName.Text = "";
            tbAmphurCode.Text = "";
            ddllProvince.SelectedIndex = 0;
            rdbStatus.SelectedIndex = 0;
        }

        protected void Amphur_RowDataBound(Object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["Amphur_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";


                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["AmphurID"].ToString();

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void Amphur_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.AmphurDAO().GetAmphur(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    tbAmphurName.Text = dt.Rows[0]["AmphurName"].ToString();
                    tbAmphurCode.Text = dt.Rows[0]["AmphurCode"].ToString();
                    ddllProvince.SelectedValue = dt.Rows[0]["ProvinceID"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Amphur_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Amphur_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }

                pnlDetail.Visible = true;

            }
        }

        protected void Amphur_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvAmphur);
        }
    }
}