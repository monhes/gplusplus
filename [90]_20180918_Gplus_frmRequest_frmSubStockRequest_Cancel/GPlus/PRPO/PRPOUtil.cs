using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GPlus.DataAccess;

namespace GPlus.PRPO
{
    public class PRPOUtil
    {
        public static void SetDivDep(ref TextBox txtBox, int orgID)
        {
            string[] divdep = new PrPoDAO().GetDivDepName(orgID);

            if (divdep[1] != "")
                txtBox.Text = divdep[0] + "/" + divdep[1];
            else
                txtBox.Text = divdep[0];
        }
    }
}