using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using GPlus.DataAccess;
using System.Diagnostics;

namespace GPlus.UserControls
{
    public partial class pop_NoPRSelect : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");

                btnSelect.OnClientClick = Util.FillControl(new string[] { Request["noPR"] }, new string[] { drv["PR_Code"].ToString() });
            }
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData(txtNoPR.Text.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(txtNoPR.Text.ToString());
        }

        private void BindData(string noPrCode = "")
        {
            SQLParameterList sqlParamList = new SQLParameterList();
            sqlParamList.AddStringField("OrgStrucId", Session["OrgID"].ToString());
            sqlParamList.AddStringField("PRCode", noPrCode);
            sqlParamList.AddStringField("PRType", ddlPRType.SelectedValue);
            sqlParamList.AddIntegerField("PageNum", PagingControl1.CurrentPageIndex);
            sqlParamList.AddIntegerField("PageSize", PagingControl1.PageSize);

            DataSet ds = new PrPoDAO().GetPRCode(sqlParamList);

            DataRowCollection rows = ds.Tables[0].Rows;

            PagingControl1.RecordCount = (int)ds.Tables[2].Rows[0][0];

            gvNoPR.DataSource = ds.Tables[1];
            gvNoPR.DataBind();
        }
    }
}