using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.MasterData
{
    public partial class BuildingMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "128";
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click1(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancelSearch_Click1(object sender, EventArgs e)
        {
            txtBuildingid.Text = "";
            txtBuildingna.Text = "";
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
            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "script"
                , "window.location = 'BuildingMgt.aspx#pnlDetail'"
                , true
            );
        }

        protected void gvBuilding_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[3].Text = drv["Building_Status"].ToString() == "True" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";
                
                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Building_ID"].ToString();
                ((ImageButton)e.Row.FindControl("btnDetail2")).CommandArgument = drv["Building_Id"].ToString();
                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void gvBuilding_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.BuildingDAO().GetBuilding(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    txtBuildingcode.Text = dt.Rows[0]["Building_Code"].ToString();
                    txtBuildingname.Text = dt.Rows[0]["Building_Name"].ToString();

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Building_Status"].ToString() == "True";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Building_Status"].ToString() == "False";
                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }

                pnlDetail.Visible = true;
            }
            else if (e.CommandName == "Detail")
            {
                Response.Redirect("BuildingFloorMgt.aspx?tid=" + e.CommandArgument.ToString());
            }
        }

        protected void gvBuilding_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvBuilding);
        }
            
        private void BindData()
        {
            DataSet ds = new DataAccess.BuildingDAO().GetBuilding(txtBuildingid.Text, txtBuildingna.Text, ddlStatus.SelectedValue,
               PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvBuilding.DataSource = ds.Tables[0];
            gvBuilding.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string Status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retValue = "";
            if (hdID.Value == "")
            {

                retValue = new DataAccess.BuildingDAO().AddBuilding(txtBuildingcode.Text, txtBuildingname.Text, Status, this.UserID);

                if (retValue != "0") hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.BuildingDAO().UpdateBuilding(hdID.Value, txtBuildingcode.Text, txtBuildingname.Text, Status, this.UserID);
            }
            if (retValue == "0")
            {
                this.ShowMessageBox("มีรหัสตึก/อาคาร " + txtBuildingcode.Text + " อยู่ในระบบแล้ว");
                //txtBuildingcode.Text.Focus();
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
        
        private void ClearData()
        {
            hdID.Value = "";
            txtBuildingcode.Text = "";
            txtBuildingname.Text = "";
            rdbStatus.SelectedIndex = 0;
        }

   
    }
}