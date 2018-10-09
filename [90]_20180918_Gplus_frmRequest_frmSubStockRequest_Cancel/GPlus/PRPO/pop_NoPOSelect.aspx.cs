using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

using GPlus.DataAccess;
using System.Data;

namespace GPlus.PRPO
{
    public partial class pop_NoPOSelect : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
            
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData(txtNoPO.Text.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(txtNoPO.Text.ToString());
        }

        private void BindData(string poCode = "")
        {
            SQLParameterList sqlParamList = new SQLParameterList();
            sqlParamList.AddStringField("OrgStrucId", Session["OrgID"].ToString());
            sqlParamList.AddStringField("POCode", poCode);
            sqlParamList.AddIntegerField("PageNum", PagingControl1.CurrentPageIndex);
            sqlParamList.AddIntegerField("PageSize", PagingControl1.PageSize);

            DataSet ds = new PrPoDAO().GetPOCode(sqlParamList);

            PagingControl1.RecordCount = (int)ds.Tables[2].Rows[0][0];

            gvNoPO.DataSource = ds.Tables[1];
            gvNoPO.DataBind();
        }

        protected void gvItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");

                btnSelect.OnClientClick = Util.FillControl(
                    new string[] { Request["noPO"] }, new string[] { drv["PO_Code"].ToString() });
            }
        }
    }
}