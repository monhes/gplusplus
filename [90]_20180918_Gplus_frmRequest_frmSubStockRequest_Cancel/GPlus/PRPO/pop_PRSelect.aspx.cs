using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PRSelect : Pagebase
    {
        public string Script
        {
            get
            {
                if (ViewState["Script"] == null)
                    ViewState["Script"] = "";

                return ViewState["Script"].ToString();
            }
            set
            {
                ViewState["Script"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindDropdown();
                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindDropdown()
        {
            ddlSupplier.DataSource = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
            ddlSupplier.DataTextField = "Supplier_Name";
            ddlSupplier.DataValueField = "Supplier_ID";
            ddlSupplier.DataBind();
            ddlSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));
        }


        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlSupplier.SelectedIndex = 0;
            ccFrom.Text = "";
            ccTo.Text = "";
            chkApprove.Checked = true;
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.PRDAO().GetPRForm1andItem(ccFrom.Text, ccTo.Text, ddlSupplier.SelectedValue,
                (chkApprove.Checked ? "2" : ""), Request["prtype"], PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder,"");

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPR.DataSource = ds.Tables[0];
            gvPR.DataBind();
        }

        protected void gvPR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.Script = "function CheckAllP(state){";
                ((CheckBox)e.Row.FindControl("chkDH")).Attributes.Add("onclick", "CheckAllP(this.checked);");
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv["Unit_Quantity"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((decimal)drv["Unit_Quantity"]).ToString("#,##0");

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateFormat);


                switch (drv["Status"].ToString())
                {
                    case "0": e.Row.Cells[9].Text = "ยกเลิก"; break;
                    case "1": e.Row.Cells[9].Text = "รออนุมัติ"; break;
                    case "2": e.Row.Cells[9].Text = "อนุมัติ"; break;
                    case "3": e.Row.Cells[9].Text = "ไม่อนุมัติ"; break;
                    case "4": e.Row.Cells[9].Text = "จัดซื้อไม่ดำเนินการ"; break;
                    case "5": e.Row.Cells[9].Text = "ออก PO แล้ว"; break;
                }

                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                HiddenField hdItemID = (HiddenField)e.Row.FindControl("hdItemID");
                hdID.Value = drv["PR_ID"].ToString();
                hdItemID.Value = drv["PRItem_ID"].ToString();
                CheckBox chkD = (CheckBox)e.Row.FindControl("chkD");
                this.Script += "document.getElementById('" + chkD.ClientID + "').checked = state;";
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                this.Script += "}";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "chk", this.Script, true);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            DataAccess.PODAO db = new DataAccess.PODAO();
            for (int i = 0; i < gvPR.Rows.Count; i++)
            {
                CheckBox chkD = (CheckBox)gvPR.Rows[i].FindControl("chkD");
                if (chkD.Checked)
                {
                    HiddenField hdID = (HiddenField)gvPR.Rows[i].FindControl("hdID");
                    HiddenField hdItemID = (HiddenField)gvPR.Rows[i].FindControl("hdItemID");

                    db.AddPOItemByPR(hdID.Value, Request["id"], hdItemID.Value);
                }
            }
            db.AddForm2FromPR(Request["id"]);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "cl", "if(window.opener)window.opener.document.getElementById('btnRefreshI').click();" +
                "window.close();", true);
        }

    }
}