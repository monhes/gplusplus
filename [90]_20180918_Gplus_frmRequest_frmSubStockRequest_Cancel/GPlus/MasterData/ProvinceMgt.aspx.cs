using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.MasterData
{
    public partial class ProvinceMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "129";
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
            txtProvinceCode.Text = "";
            txtProvinceName.Text = "";
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
                , "window.location = 'ProvinceMgt.aspx#panel'"
                , true
            );
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retValue = "";
            if (hdID.Value == "")
            {

                retValue = new DataAccess.ProvinceDAO().AddProvince(txtProvinceCode1.Text, txtProvinceName1.Text, Status, this.UserID);
                if (retValue != "0") hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.ProvinceDAO().UpdateProvince(hdID.Value, txtProvinceCode1.Text, txtProvinceName1.Text, Status, this.UserID);
            }
            if (retValue == "0")
            {
                this.ShowMessageBox("มีรหัสจังหวัด " + txtProvinceCode1.Text + " อยู่ในระบบแล้ว");
                txtProvinceCode1.Focus();
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

      //=============================================================================================== 


        protected void Province_RowDataBound(Object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["Province_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>" ;

                
                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["ProvinceID"].ToString();

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void Province_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.ProvinceDAO().GetProvince(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtProvinceCode1.Text = dt.Rows[0]["ProvinceCode"].ToString();
                    txtProvinceName1.Text = dt.Rows[0]["ProvinceName"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Province_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Province_Status"].ToString() == "0";
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

        private void BindData()
        {
            DataSet ds = new DataAccess.ProvinceDAO().GetProvince(txtProvinceCode.Text, txtProvinceName.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex,
               PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            GvProvince.DataSource = ds.Tables[0];
            GvProvince.DataBind();
            
        }
        public void ClearData()
        {
            hdID.Value = "";
            txtProvinceName1.Text = "";
            txtProvinceCode1.Text = "";
            rdbStatus.SelectedIndex = 0;
            //lblCreateBy.Text = "";
            //lblCreateDate.Text = "";
            //lblUpdateBy.Text = "";
            //lblUpdatedate.Text = "";
        }

        protected void Province_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(GvProvince);
        }
    }
}