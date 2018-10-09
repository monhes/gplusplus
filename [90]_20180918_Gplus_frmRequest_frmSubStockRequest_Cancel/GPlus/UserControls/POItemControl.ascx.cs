using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class POItemControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnPOSearch.OnClientClick = Util.CreatePopUp(
                    "../PRPO/pop_NoPOSelect.aspx",
                    new string[] { "noPO" },
                    new string[] { txtItemNoPO.ClientID },
                    "popPOSelect"
                );
            }
        }
    }
}