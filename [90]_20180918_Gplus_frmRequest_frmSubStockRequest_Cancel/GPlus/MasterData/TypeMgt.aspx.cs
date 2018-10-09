using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.MasterData
{
    public partial class TypeMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "107";
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "Cate_Code", "").Tables[0];

            ddlMaterialTypeSearch.Items.Clear();
            ddlMaterialTypeSearch.Items.Insert(0, new ListItem("ทั้งหมด", ""));

            ddlMaterialType.Items.Clear();
            ddlMaterialType.Items.Insert(0, new ListItem("ประเภทวัสดุอุปกรณ์", ""));

            for (int i = 0; i < dt.Rows.Count; i++)
            {

                ddlMaterialTypeSearch.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));


                ddlMaterialType.Items.Add(new ListItem(dt.Rows[i]["Cate_Code"].ToString() + " - " + dt.Rows[i]["Cat_Name"].ToString(),
                    dt.Rows[i]["Cate_ID"].ToString()));
            }
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
            txtTypeCodeSearch.Text = "";
            txtTypeNameSearch.Text = "";
            ddlStatus.SelectedIndex = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ClearData();
            pnlDetail.Visible = true;
            txtTypeCode.Enabled = true;
            ddlMaterialType.Enabled = true;
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);
        }

        protected void gvType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[4].Text = drv["Type_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Type_ID"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.TypeDAO().GetType(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtTypeCode.Text = dt.Rows[0]["Type_Code"].ToString();
                    txtTypeName.Text = dt.Rows[0]["Type_Name"].ToString();

                    ddlMaterialType.SelectedValue = dt.Rows[0]["Cate_ID"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Type_Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Type_Status"].ToString() == "0";
                    lblCreateBy.Text = dt.Rows[0]["FullName_Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["FullName_Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }

                pnlDetail.Visible = true;
                txtTypeCode.Enabled = false;
                ddlMaterialType.Enabled = false;
            }
        }

        protected void gvType_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvType);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retVal = "";
            if (hdID.Value == "")
            {
                retVal = new DataAccess.TypeDAO().AddType(txtTypeCode.Text, txtTypeName.Text, ddlMaterialType.SelectedValue, status, this.UserName);
                if (retVal != "0") hdID.Value = retVal;
            }
            else
            {
                retVal = new DataAccess.TypeDAO().UpdateType(hdID.Value, txtTypeCode.Text, txtTypeName.Text, ddlMaterialType.SelectedValue, status, this.UserName);
            }
            if (retVal == "0")
            {
                this.ShowMessageBox("มีรหัส " + txtTypeCode.Text + " อยู่ในระบบแล้ว");
                txtTypeCode.Focus();
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



        private void BindData()
        {
            DataSet ds = new DataAccess.TypeDAO().GetType(txtTypeCodeSearch.Text, txtTypeNameSearch.Text, ddlStatus.SelectedValue,
                PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder , ddlMaterialTypeSearch.SelectedValue);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvType.DataSource = ds.Tables[0];
            gvType.DataBind();
        }

        private void ClearData()
        {
            hdID.Value = "";
            txtTypeCode.Text = "";
            txtTypeName.Text = "";
            rdbStatus.SelectedIndex = 0;
            ddlMaterialType.SelectedIndex = 0;
        }
    }
}