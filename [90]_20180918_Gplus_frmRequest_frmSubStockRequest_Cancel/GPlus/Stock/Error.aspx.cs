using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.Stock
{
    public partial class Error :  Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbError.Text =   Session["Error"] as string;
        }
    }
}