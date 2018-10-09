using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using GPlus.DataAccess;
using System.Diagnostics;

namespace GPlus.PRPO
{
    public partial class pop_DepSelect : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string [] divdep = new PrPoDAO().GetDivDepName(Convert.ToInt32(Session["OrgID"].ToString()));

                if (divdep[0] == "ฝ่ายจัดซื้อ" || divdep[0] == "ฝ่ายธุรการ")
                {
                    BindData();
                }
                else
                {
                    // Get division code from Session["OrgID"]
                    string divCode = new PrPoDAO().GetDivCode(Convert.ToInt32(Session["OrgID"].ToString()));

                    txtDivCode.Text = divCode;
                    txtDivCode.Enabled = false;
                    txtDivCode.Style[HtmlTextWriterStyle.BackgroundColor] = "#ddd";

                    BindData(divCode);
                }
            }
            
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindData(string divCode = "", string depCode = "", string description = "")
        {
            // Prepare div and dep information to GridView
            SQLParameterList sqlParamList = new SQLParameterList();
            sqlParamList.AddIntegerField("OrgStrucId", Convert.ToInt32(Session["OrgID"]));
            sqlParamList.AddStringField("DivCode", divCode);
            sqlParamList.AddStringField("DepCode", depCode);
            sqlParamList.AddStringField("Description", description);
            sqlParamList.AddIntegerField("PageNum", PagingControl1.CurrentPageIndex);
            sqlParamList.AddIntegerField("PageSize", PagingControl1.PageSize);

            // Bind the information to GridView
            DataSet ds = new PrPoDAO().GetAllDivDep(sqlParamList);
            DataRowCollection rows = ds.Tables[0].Rows;
            PagingControl1.RecordCount = (int)ds.Tables[2].Rows[0][0];
            gvDivDepDisplay.DataSource = ds.Tables[1];
            gvDivDepDisplay.DataBind();
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData(txtDivCode.Text.ToString(), txtDepCode.Text.ToString(), txtDescription.Text.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(txtDivCode.Text.ToString(), txtDepCode.Text.ToString(), txtDescription.Text.ToString());
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");

                btnSelect.OnClientClick = Util.FillControl(
                    new string[] { Request["divName"].ToString(), Request["depName"].ToString() },
                    new string[] { drv["DivNameHidden"].ToString(), drv["Description"].ToString() }
                );
            }
        }
    }
}