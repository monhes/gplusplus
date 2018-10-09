using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_SpecPurchase : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "getd", "document.getElementById('"+txtDetail.ClientID+
                    "').value = window.opener.document.getElementById('"+Request["ctl"]+"').value;", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "saved", "window.opener.document.getElementById('" + Request["ctl"] +
                "').value = document.getElementById('" + txtDetail.ClientID +"').value;window.close();", true);
        }
    }
}