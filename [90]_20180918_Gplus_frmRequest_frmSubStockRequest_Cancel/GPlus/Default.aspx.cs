using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GPlus
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = ConfigurationManager.AppSettings["PageTitle"];
            if (!IsPostBack)
            {
                Pagebase pb = new Pagebase();
                pb.UserID = "";
                pb.UserName = "";
                txtUserName.Focus();
            }

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataAccess.UserDAO().GetAccount(txtUserName.Text, Util.EncryptPassword(txtPasword.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {
                Pagebase pb = new Pagebase();
                pb.UserID = ds.Tables[0].Rows[0]["Account_ID"].ToString();
                pb.UserName = ds.Tables[0].Rows[0]["Account_UserName"].ToString();
                pb.UserProfile = ds.Tables[0].Rows[0];
                pb.FirstName = ds.Tables[0].Rows[0]["Account_Fname"].ToString();
                pb.LastName = ds.Tables[0].Rows[0]["Account_Lname"].ToString();
                pb.OrgID = ds.Tables[0].Rows[0]["OrgStruc_ID"].ToString();
                pb.OrgName = ds.Tables[0].Rows[0]["OrgStruc_Name"].ToString();

                pb.Permission = ds.Tables[2];
                Response.Redirect("Home/Home.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('User Name หรือ Password ไม่ถูกต้อง');", true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPasword.Text = "";
            txtUserName.Text = "";
        }
    }
}