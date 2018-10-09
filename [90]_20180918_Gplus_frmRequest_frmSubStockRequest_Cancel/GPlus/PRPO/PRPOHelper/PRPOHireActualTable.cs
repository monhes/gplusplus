using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public sealed class PRPOHireActualTable : PRPOActualTable
    {
        public bool HasInserted { get; set; }

        public PRPOHireActualTable() { HttpContext.Current.Session[PRPOSession.HireActualTable] = Table; }
        public PRPOHireActualTable(string sessionName)
            : base(HttpContext.Current.Session[PRPOSession.HireActualTable] as DataTable) { }

        public string GetPrItemIdsThatGroupedAsN()
        {
            string prIds = "";

            DataRow[] rows = Table.Select("PrItemID <> '' AND Grouped = 'N'");

            foreach (DataRow row in rows)
                prIds += row["PrItemID"].ToString() + ",";

            if (prIds.Length > 0)
                return prIds.Remove(prIds.LastIndexOf(','));

            return null;
        }

        public override void AddItem(string invItemID, string packID, string prID, string prItemID, string invItemCode, string invItemName, string packName, string unitPrice, string unitQuantity, string popupType, string grouped)
        {
            if (hasPrItem())
            {
                DataRow[] rows = Table.Select(string.Format("PrID = '{0}'", prID));  // มี PRID เดียวกันหรือไม่

                if (rows.Length > 0)
                {
                    base.AddItem(invItemID, packID, prID, prItemID, invItemCode, invItemName, packName, unitPrice, unitQuantity, popupType, grouped);
                    HasInserted = true;
                }
                else
                {
                    // เนื่องจาก สามารถเลือกรายการ PR (PrItem) ของใบเดียวกันได้ทีละหลายรายการ
                    // ดังนั้น อาจเกิดกรณีที่ผู้ใช้เลือก PrItem ต่างใบกันได้ จึงจำเป็นต้องลบรายการล่าสุดที่เพิ่งใส่ลงตาราง

                    if (HasInserted)    // หากมีการใส่ข้อมูลในระหว่างเปิด Popup ให้ลบข้อมูลที่เพิ่งใส่ล่าสุดทิ้ง
                    {
                        Table.Rows.Remove(Table.Rows[Table.Rows.Count - 1]);
                        Table.AcceptChanges();
                    }

                    throw new Exception("รายการ PR ต้องมีเลขที่ PR เดียวกัน");
                }
            }
            else    // ยังไม่มี PR ในตาราง
            {
                base.AddItem(invItemID, packID, prID, prItemID, invItemCode, invItemName, packName, unitPrice, unitQuantity, popupType, grouped);
                HasInserted = true;
            }
        }

        /// <summary>
        ///     ใช้กับ PR ขอจ้าง
        /// </summary>
        public void AddItem()
        {
            DataRow row = Table.NewRow();

            if (MinPrItemID == 0 || MinPrItemID > 0)
                row["PrItemID"] = -1;
            else
                row["PrItemID"] = MinPrItemID - 1;

            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        public override void DeleteItem(string prID, string prItemID)
        {
            DataRow[] rows = Table.Select(string.Format("PrID = '{0}' AND PrItemID = '{1}'", prID, prItemID));

            if (rows.Length > 0)
            {
                Table.Rows.Remove(rows[0]);
                Table.AcceptChanges();
            }
        }

        private int MinPrItemID
        {
            get
            {
                if (Table.Rows.Count == 0)
                    return 0;
                else
                    return (int)Table.AsEnumerable().Min(prItemID => Convert.ToInt32(prItemID["PrItemID"]));
            }
        }
    }
}