using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.Request
{
    public partial class pop_Remark : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "getd", "document.getElementById('"+txtDetail.ClientID+
                //    "').value = window.opener.document.getElementById('"+Request["ctl"]+"').value;", true);

                // Check ว่าต้อง Disable textarea หรือไม่
                if (Request["chk"].ToString() == "f")
                    txtDetail.Enabled = false;
                else
                    txtDetail.Enabled = true;

                if (Request["type"].ToString() == "stk")
                {
                    lblHeader.Text = "หมายเหตุคลัง";
                }
                else if (Request["type"].ToString() == "org")
                {
                    lblHeader.Text = "หมายเหตุหน่วยงาน";
                }

                ScriptManager.RegisterStartupScript(this, this.GetType(), "getd", "document.getElementById('" + txtDetail.ClientID +
                    "').value = window.opener.document.getElementById('" + Request["ctl"] + "').value;", true);

                ////string js = "alert(window.opener.document.getElementById('" + Request["ctl"] + "'));";

                //ScriptManager.RegisterStartupScript
                //(
                //    this
                //    , GetType()
                //    , "script"
                //    , js
                //    , true
                //);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "saved", "window.opener.document.getElementById('" + Request["ctl"] +
                "').value = document.getElementById('" + txtDetail.ClientID +"').value;window.close();", true);
        }
    }
}