using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class PopCalendar : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "เลือกวัน";
        }

        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "cal", "if(window.opener){window.opener.document.getElementById('" + Request["cid"] +
                "').value = '" + Calendar1.SelectedDate.ToString("dd/MM/yyyy") + "';};window.close();", true);
        }
    }
}