using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class SupplierItemControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSupplierSearch.OnClientClick =
                   "open_popup('../PRPO/pop_SupplierSelect.aspx?supplierName=" + txtItemSupplierName.ClientID +
                   "', 850, 400, 'popSupplier', 'yes', 'yes', 'yes'); return false;";
            }
        }
    }
}