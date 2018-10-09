using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Diagnostics;

namespace GPlus.UserControls
{
    public partial class NoPRPOControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnNoPRPO.OnClientClick = Util.CreatePopUp(
                                                "../PRPO/pop_NoPRSelect.aspx",
                                                new string[] { "noPR" },
                                                new string[] { txtNoPRPO.ClientID },
                                                "popPRType"
                                            );
        }
    }
}