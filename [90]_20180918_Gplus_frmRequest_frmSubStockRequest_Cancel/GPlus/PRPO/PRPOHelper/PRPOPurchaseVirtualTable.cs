using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public sealed class PRPOPurchaseVirtualTable : PRPOActualTable
    {
        public PRPOPurchaseVirtualTable()
        {
            HttpContext.Current.Session[PRPOSession.PurchaseVirtualTable] = Table;
        }

        public PRPOPurchaseVirtualTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }
    }
}