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

namespace GPlus.UserControls
{
    public partial class pop_OrgSelect_New : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindData(string divCode = "", string description = "")
        {
            //SQLParameterList sqlParamList = new SQLParameterList();
            //sqlParamList.AddIntegerField("OrgStrucId", Convert.ToInt32(Session["OrgID"]));
            //sqlParamList.AddStringField("DivCode", divCode);
            //sqlParamList.AddStringField("DepCode", "");
            //sqlParamList.AddStringField("Description", description);
            //sqlParamList.AddIntegerField("PageNum", PagingControl1.CurrentPageIndex);
            //sqlParamList.AddIntegerField("PageSize", PagingControl1.PageSize);

            //DataSet ds = new PrPoDAO().GetAllDivDep(sqlParamList);

            //if ((Request["searchAll"] == "true") && (divCode == "" && description == ""))
            //{
            //    DataRow row = ds.Tables[1].NewRow();

            //    row["Div_Code"] = "";
            //    row["Dep_Code"] = "";
            //    row["DivNameHidden"] = "";
            //    row["Description"] = "";

            //    ds.Tables[1].Rows.InsertAt(row, 0);
            //    ds.Tables[1].AcceptChanges();
            //}

            //PagingControl1.RecordCount = (int)ds.Tables[2].Rows[0][0];
            //gvDivDepDisplay.DataSource = ds.Tables[1];
            //gvDivDepDisplay.DataBind();




            DataAccess.OrgStructureDAO db = new DataAccess.OrgStructureDAO();
            DataSet dt = db.GetOrgStructure_new(divCode, "", description, "", "", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, "Div_Code", "");
            DataView dv = dt.Tables[0].DefaultView;
            //dv.RowFilter = "Dep_Code IS NULL OR Dep_Code = ''";

            PagingControl1.RecordCount = (int)dv.Table.Rows.Count;
            gvDivDepDisplay.DataSource = dv.Table;
            gvDivDepDisplay.DataBind();
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData(txtDivCode.Text.ToString(), txtDescription.Text.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(txtDivCode.Text.ToString(), txtDescription.Text.ToString());
        }

        protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnSelect = (LinkButton)e.Row.FindControl("btnSelect");

                btnSelect.OnClientClick = Util.FillControl(
                    new string[] { Request["divName"].ToString()                                        , Request["depName"].ToString() , Request["orgID"].ToString()   , Request["divCode"].ToString() , Request["depCode"].ToString() },
                    new string[] { drv["Div_Code"].ToString() + "/" + drv["Description"].ToString(), drv["Description"].ToString(), drv["OrgStruc_Id"].ToString(), drv["Div_Code"].ToString(), drv["Dep_Code"].ToString() }
                );
            }
        }        
    }
}