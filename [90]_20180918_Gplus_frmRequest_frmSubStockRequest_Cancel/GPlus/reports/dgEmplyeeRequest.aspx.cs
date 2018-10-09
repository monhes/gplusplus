using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;

namespace GPlus.Request
{
    public partial class dgEmplyeeRequest : System.Web.UI.Page
    {
        private RequestDAO _reqeustDAO;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _reqeustDAO = new RequestDAO();
                //DataView dv = _reqeustDAO.GetEmployee(Request["OrgId"]);
                DataView dv = _reqeustDAO.GetAllEmployee();
                this.gvEmployee.DataSource = dv;
                this.gvEmployee.DataBind();
            }
        }
        /// <summary>
        /// Search Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnSearchClick(object sender, EventArgs e)
        {
            string fnameKeyword = string.IsNullOrWhiteSpace(this.txtFname.Text) ? null : this.txtFname.Text;
            string lnameKeyword = string.IsNullOrWhiteSpace(this.txtLname.Text) ? null : this.txtLname.Text;
            if (_reqeustDAO == null)
            {
                _reqeustDAO = new RequestDAO();
            }
            //DataView dv = _reqeustDAO.GetEmployee(Request["OrgId"], fnameKeyword, lnameKeyword);
            DataView dv = _reqeustDAO.GetAllEmployee(fnameKeyword, lnameKeyword);
            this.gvEmployee.DataSource = dv;
            this.gvEmployee.DataBind();
        }

        protected void GvEmployeeRowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                if (Session["Request"] != null)
                {
                    DataTable dtt = Session["Request"] as DataTable;
                    if (_reqeustDAO == null)
                    {
                        _reqeustDAO = new RequestDAO();
                    }
                    //DataRow[] drSource = _reqeustDAO.GetEmployee(Request["OrgId"]).Table.Select("[Account_Id]='" + e.CommandArgument + "'");
                    DataRow[] drSource = _reqeustDAO.GetAllEmployee().Table.Select("[Account_Id]='" + e.CommandArgument + "'");
                    if (drSource.Length > 0)
                    {
                        dtt.Rows[0]["Account_Id"] = drSource[0]["Account_Id"];
                        dtt.Rows[0]["Account_Fname"] = drSource[0]["Account_Fname"];
                        dtt.Rows[0]["Account_Lname"] = drSource[0]["Account_Lname"];

                        //Nin Add 
                        //dtt.Rows[0]["OrgStruc_Id_Req"] = drSource[0]["OrgStruc_Id"];
                        //dtt.Rows[0]["Div_Code"] = drSource[0]["Div_Code"];
                        //dtt.Rows[0]["Description"] = drSource[0]["Div_Name"];
                        //End Nin Add

                        dtt.AcceptChanges();
                    }

                    ClientScript.RegisterStartupScript(this.GetType(), "onClose", "<script>window.close();</script>");
                }
            }
        }

        protected void GvEmployeeRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dvr = e.Row.DataItem as DataRowView;
                LinkButton selectLinkButton = ((LinkButton)e.Row.Cells[0].FindControl("btnSelected"));
                selectLinkButton.CommandArgument = dvr["Account_Id"].ToString();
            }
        }
    }
}