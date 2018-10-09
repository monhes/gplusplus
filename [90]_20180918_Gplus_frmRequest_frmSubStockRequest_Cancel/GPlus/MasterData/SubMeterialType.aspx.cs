using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class SubMeterialType : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "105";
                BindDropdown();
                if (Request["tid"] != null)
                {
                    if (ddlMaterialTypeSearch.Items.FindByValue(Request["tid"]) != null)
                        ddlMaterialTypeSearch.SelectedValue = Request["tid"];
                }
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
            txtSubMaterialTypeNameSearch.Text = "";
            txtSubMaterialTypeCodeSearch.Text = "";
            ddlMaterialTypeSearch.SelectedIndex = 0;
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

        protected void gvMaterialType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[4].Text = drv["SubCate_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["SubCate_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvMaterialType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.CategoryDAO().GetSubCate(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtSubMaterialTypeCode.Text = dt.Rows[0]["SubCate_Code"].ToString();
                    txtSubMaterialTypeName.Text = dt.Rows[0]["SubCate_Name"].ToString();

                    if (ddlMaterialType.Items.FindByValue(dt.Rows[0]["Cate_ID"].ToString()) != null)
                        ddlMaterialType.SelectedValue = dt.Rows[0]["Cate_ID"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["SubCate_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["SubCate_Status"].ToString() == "0";
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

        protected void gvMaterialType_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvMaterialType);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retVal = "";
            if (hdID.Value == "")
            {
                retVal = new DataAccess.CategoryDAO().AddSubCate(txtSubMaterialTypeCode.Text, ddlMaterialType.SelectedValue, txtSubMaterialTypeName.Text,
                    status, this.UserName);
                if (retVal != "0") hdID.Value = retVal;
            }
            else
            {
                retVal = new DataAccess.CategoryDAO().UpdateSubCate(hdID.Value, txtSubMaterialTypeCode.Text, ddlMaterialType.SelectedValue,
                    txtSubMaterialTypeName.Text, status, this.UserName);
            }
            if (retVal == "0")
            {
                this.ShowMessageBox("มีรหัส "+txtSubMaterialTypeCode.Text+" นี้อยู่ในระบบแล้ว");
                txtSubMaterialTypeCode.Focus();
                return;
            }

            pnlDetail.Visible = false;
            ClearData();
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearData();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];
            ddlMaterialTypeSearch.Items.Clear();
            ddlMaterialTypeSearch.Items.Insert(0, new ListItem("ทั้งหมด", ""));

            ddlMaterialType.Items.Clear();
            ddlMaterialType.Items.Insert(0, new ListItem("ทั้งหมด", ""));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ddlMaterialTypeSearch.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));

                ddlMaterialType.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));
            }

        }

        private void BindData()
        {
            DataSet ds = new DataAccess.CategoryDAO().GetSubCate( txtSubMaterialTypeCodeSearch.Text,ddlMaterialTypeSearch.SelectedValue, 
                txtSubMaterialTypeNameSearch.Text, ddlStatus.SelectedValue, PagingControl1.CurrentPageIndex, PagingControl1.PageSize,
                this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvMaterialType.DataSource = ds.Tables[0];
            gvMaterialType.DataBind();
            pnlDetail.Visible = false;
        }

        public void ClearData()
        {
            hdID.Value = "";
            txtSubMaterialTypeCode.Text = "";
            txtSubMaterialTypeName.Text = "";
            if (Request["tid"] != null)
            {
                if (ddlMaterialType.Items.FindByValue(Request["tid"]) != null)
                    ddlMaterialType.SelectedValue = Request["tid"];
            }
            else
                ddlMaterialType.SelectedIndex = 0;
            
            rdbStatus.SelectedIndex = 0;
            lblCreateBy.Text = "";
            lblCreateDate.Text = "";
            lblUpdateBy.Text = "";
            lblUpdatedate.Text = "";
        }

    }
}