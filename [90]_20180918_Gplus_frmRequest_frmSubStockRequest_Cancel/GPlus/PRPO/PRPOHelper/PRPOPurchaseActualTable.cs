using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public sealed class PRPOPurchaseActualTable : PRPOActualTable
    {
        public PRPOPurchaseActualTable()
        {
            HttpContext.Current.Session[PRPOSession.PurchaseActualTable] = Table;
        }
        public PRPOPurchaseActualTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        public void MarkGrouped()
        {
            // มาร์กว่าถูกกรุ๊ปแล้ว
            DataRow[] rows = Table.Select("Grouped = 'N'");
            foreach (DataRow r in rows)
            {
                r["Grouped"] = "Y";
                r.AcceptChanges();
            }
        }

        public DataRow[] GetPrItems(string poItemID)
        {
            return Table.Select(string.Format("POItemID = '{0}' AND PrItemID <> '' AND PrID <> ''", poItemID));
        }

        public override void AddItem(string invItemID, string packID, string prID, string prItemID, string invItemCode, string invItemName, string packName, string unitPrice, string unitQuantity, string popupType, string grouped)
        {

            base.AddItem(invItemID, packID, prID, prItemID, invItemCode, invItemName, packName, unitPrice, unitQuantity, popupType, grouped);
        }
    }
}