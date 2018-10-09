using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.MasterData
{
    public partial class TumbonMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "131";
                BindProvince();
                BindAmphur();
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
            DataSet ds = new DataAccess.AmphurDAO().GetTumbon(txtTumbonNameSearch.Text,ddlProvinceNameSearch.SelectedValue ,ddlAmphurSearch.SelectedValue ,
            PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvTumbon.DataSource = ds.Tables[0];
            gvTumbon.DataBind();

        }

        protected void ddlProvinceNameSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataAccess.AmphurDAO().GetAmphur1(ddlProvinceNameSearch.SelectedValue);

            ddlAmphurSearch.DataSource = dt;
            ddlAmphurSearch.DataTextField = "AmphurName";
            ddlAmphurSearch.DataValueField = "AmphurID";
            ddlAmphurSearch.DataBind();
            ddlAmphurSearch.Items.Insert(0, new ListItem("---------- เลือกอำเภอ ----------", ""));

            ddlAmphurSearch.Focus();
        }

        private void BindProvince()
        {
            DataTable dt = new DataAccess.ProvinceDAO().GetProvince();

            ddlProvinceNameSearch.DataSource = dt;
            ddlProvinceNameSearch.DataTextField = "ProvinceName";
            ddlProvinceNameSearch.DataValueField = "ProvinceID";
            ddlProvinceNameSearch.DataBind();
            ddlProvinceNameSearch.Items.Insert(0, new ListItem( "---------- เลือกจังหวัด ----------" , ""));

            ddllProvince.DataSource = dt;
            ddllProvince.DataTextField = "ProvinceName";
            ddllProvince.DataValueField = "ProvinceID";
            ddllProvince.DataBind();
            ddllProvince.Items.Insert(0, new ListItem("---------- เลือกจังหวัด ----------", ""));

        }

        private void BindAmphur()
        {
            DataTable dt = new DataAccess.AmphurDAO().bindAmphur();

            ddlAmphurSearch.DataSource = dt;
            ddlAmphurSearch.DataTextField = "AmphurName";
            ddlAmphurSearch.DataValueField = "AmphurID";
            ddlAmphurSearch.DataBind();
            ddlAmphurSearch.Items.Insert(0, new ListItem("---------- เลือกอำเภอ ----------", ""));

            ddllAmphur.DataSource = dt;
            ddllAmphur.DataTextField = "AmphurName";
            ddllAmphur.DataValueField = "AmphurID";
            ddllAmphur.DataBind();
            ddllAmphur.Items.Insert(0, new ListItem("---------- เลือกอำเภอ ----------", ""));

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            pnlDetail.Visible = false;
            BindData();
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            txtTumbonNameSearch.Text = "";
            ddlProvinceNameSearch.SelectedValue="";
            ddlAmphurSearch.SelectedValue="";
            BindData();
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
                , "window.location = 'TumbonMgt.aspx#panel'"
                , true
            );
        }

        private void ClearData()
        {
            hdID.Value = "";
            tbTumbonCode.Text = "";
            tbTumbonName.Text = "";
            tbSUBDST.Text = "";
            tbpostcode.Text = "";
            ddlProvinceNameSearch.SelectedValue = "";
            ddlAmphurSearch.SelectedValue = "";
            ddllProvince.SelectedValue = "";
            ddllAmphur.SelectedValue = "";
            tbpostcode.Text = "";
            rdbStatus.SelectedIndex = 0;
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
            if (ddllAmphur.SelectedValue == "")
            {
                this.ShowMessageBox("กรุณาเลือกอำเภอ");
                ddllAmphur.Focus();
                return;
            }

            if (hdID.Value == "")
            {

                retValue = new DataAccess.AmphurDAO().AddTumbon(tbTumbonName.Text, tbTumbonCode.Text, ddllProvince.SelectedValue, ddllAmphur.SelectedValue, tbSUBDST.Text, tbpostcode.Text, Status, this.UserID);
                if (retValue != "0") hdID.Value = retValue;
            }
            else
            {
                retValue = new DataAccess.AmphurDAO().UpdateTumbon(hdID.Value, tbTumbonCode.Text, tbTumbonName.Text, ddllProvince.SelectedValue ,ddllAmphur.SelectedValue ,tbSUBDST.Text , tbpostcode.Text , Status, this.UserID);
            }
            if (retValue == "0")
            {
                this.ShowMessageBox("มีตำบล " + tbTumbonName.Text + " อยู่ในระบบแล้ว");
                tbTumbonName.Focus();
                return;
            }
            ClearData();
            pnlDetail.Visible = false;
            BindData();
        }

        protected void ddllProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataAccess.AmphurDAO().GetAmphur1(ddllProvince.SelectedValue);

            ddllAmphur.DataSource = dt;
            ddllAmphur.DataTextField = "AmphurName";
            ddllAmphur.DataValueField = "AmphurID";
            ddllAmphur.DataBind();
            ddllAmphur.Items.Insert(0, new ListItem("---------- เลือกอำเภอ ----------", ""));

            ddllAmphur.Focus();
        }



        protected void gvTumbon_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                DataTable dt = new DataAccess.AmphurDAO().GetTumbonID(e.CommandArgument.ToString());
                hdID.Value = e.CommandArgument.ToString();
                if (dt.Rows.Count > 0)
                {
                    tbTumbonName.Text = dt.Rows[0]["TumbonName"].ToString();
                    tbTumbonCode.Text = dt.Rows[0]["TumbonCode"].ToString();
                    tbSUBDST.Text = dt.Rows[0]["SUBDST"].ToString();

                    if ((dt.Rows[0]["ProvinceID"].ToString() == "0") || (dt.Rows[0]["AmphurID"].ToString() == "0"))
                    {
                        ddllAmphur.SelectedValue = "";
                        ddllProvince.SelectedValue = "";
                    }
                    else
                    {
                        ddllProvince.SelectedValue = dt.Rows[0]["ProvinceID"].ToString();
                        ddllAmphur.SelectedValue = dt.Rows[0]["AmphurID"].ToString();
                    }

                    rdbStatus.Items[0].Selected = dt.Rows[0]["Status"].ToString() == "1";
                    rdbStatus.Items[1].Selected = dt.Rows[0]["Status"].ToString() == "0";

                    tbpostcode.Text = dt.Rows[0]["Postcode"].ToString();

                    lblCreateBy.Text = dt.Rows[0]["Create_By"].ToString();
                    lblUpdateBy.Text = dt.Rows[0]["Update_By"].ToString();
                    if (dt.Rows[0]["Create_Date"].ToString().Length > 0)
                        lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);

                    if (dt.Rows[0]["Update_Date"].ToString().Length > 0)
                        lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                }

                pnlDetail.Visible = true;

                ScriptManager.RegisterStartupScript
                (
                this
                , GetType()
                , "script"
                , "window.location = 'TumbonMgt.aspx#panel'"
                , true
                );

            }
        }

        protected void gvTumbon_DataBound(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                string status = drv["Status"].ToString();

                if(status == "1")
                {
                    e.Row.Cells[4].Text = "<span style='color:navy'>Active</span>" ;
                }
                else if(status == "0")
                {
                    e.Row.Cells[4].Text = "<span style='color:red'>InActive</span>";
                }

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["TumbonID"].ToString();

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            tbTumbonCode.Text = "";
            tbTumbonName.Text = "";
            tbSUBDST.Text = "";
            tbpostcode.Text = "";
            pnlDetail.Visible = false;
        }

        protected void Tumbon_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvTumbon);
        }



    }
}