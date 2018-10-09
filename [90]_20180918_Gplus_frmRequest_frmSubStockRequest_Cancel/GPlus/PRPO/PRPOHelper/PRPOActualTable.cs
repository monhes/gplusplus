using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public abstract class PRPOActualTable : TableBase
    {
        protected PRPOActualTable() : base() { }
        protected PRPOActualTable(DataTable table) : base(table) { }

        protected override void CreateColumns()
        {
            Table.Columns.Add("PoItemID");
            Table.Columns.Add("InvItemID");
            Table.Columns.Add("PackID");
            Table.Columns.Add("InvItemCode");
            Table.Columns.Add("InvItemName");
            Table.Columns.Add("PackName");
            Table.Columns.Add("UnitPrice");
            Table.Columns.Add("UnitQuantity");
            Table.Columns.Add("InvSpecPurchase");
            Table.Columns.Add("TradeDiscountPercent");
            Table.Columns.Add("TradeDiscountAmount");
            Table.Columns.Add("FormPrintID");
            Table.Columns.Add("TotalBeforeVat");
            Table.Columns.Add("VatPercent");
            Table.Columns.Add("VatAmount");
            Table.Columns.Add("NetAmount");
            Table.Columns.Add("UnitOrder");         // สำหรับ pop_ReorderPoint ใช้ หน่วยที่สั่งซื้อ มาเก็บลงใน gvItem ของ POControl
            Table.Columns.Add("PrID");
            Table.Columns.Add("PrItemID");
            Table.Columns.Add("PopupType");         // 0 = Reorder Point, 1 = ระบุรายการสินค้า, 2 = เลือกจาก P ; คอลัมน์นี้เก็บค่า popuptype ไว้ ไม่สำคัญมาก
            Table.Columns.Add("ProcureName");       // สำหรับ PR Select
            Table.Columns.Add("Specify");
            Table.Columns.Add("Grouped");
        }

        public DataRow[] FindItem(string invItemID, string packID)
        {
            return Table.Select(string.Format("InvItemID = '{0}' AND PackID = '{1}'", invItemID, packID));
        }

        public virtual DataRow[] FindItem(string invItemID, string packID, string prId, string prItemId)
        {
            return Table.Select(string.Format("InvItemID = '{0}' AND PackID = '{1}' AND PrID = '{2}' AND PrItemID = '{3}'", invItemID, packID, prId, prItemId));
        }

        public virtual DataRow[] FindItem(string poItemID)
        {
            return Table.Select(string.Format("PoItemID = '{0}'", poItemID));
        }

        public virtual DataRow[] FindPrItemID(string prItemID)
        {
            return Table.Select(string.Format("PrItemID = '{0}'", prItemID));
        }

        public DataRow[] GetPrItems()
        {
            return Table.Select("PrID <> '' AND PrItemID <> ''");
        }

        public virtual void DeleteItem(string invItemID, string packID)
        {
            DataRow[] rows = Table.Select(string.Format("InvItemID = '{0}' AND packID = '{1}'", invItemID, packID));

            foreach (DataRow row in rows)
                Table.Rows.Remove(row);

            Table.AcceptChanges();
        }

        public virtual void AddItem
        (
            string invItemID, string packID, string prID, string prItemID, string invItemCode, string invItemName,
            string packName, string unitPrice, string unitQuantity, string popupType, string grouped
        )
        {
            DataRow dr = Table.NewRow();

            dr["InvItemID"] = invItemID;
            dr["PackID"] = packID;
            dr["PrID"] = prID;
            dr["PrItemID"] = prItemID;
            dr["InvItemCode"] = invItemCode;
            dr["InvItemName"] = invItemName;
            dr["PackName"] = packName;
            dr["UnitPrice"] = unitPrice;
            dr["UnitQuantity"] = unitQuantity.Replace(",", "");
            dr["PopupType"] = popupType;
            dr["Grouped"] = grouped;

            Table.Rows.Add(dr);
            dr.AcceptChanges();
        }

        public void AddItem
        (
            string poItemID,
            string invItemID,
            string packID,
            string invItemCode,
            string invItemName,
            string packName,
            string unitPrice,
            string unitQuantity,
            string invSpecPurchase,
            string tradeDiscountPercent,
            string tradeDiscountAmount,
            string formPrintID,
            string totalBeforeVat,
            string vatPercent,
            string vatAmount,
            string netAmount,
            string unitOrder,
            string prID,
            string prItemID,
            string popupType,
            string procureName,
            string specify,
            string grouped
        )
        {
            DataRow row = Table.NewRow();

            row["POItemID"] = poItemID;
            row["InvItemID"] = invItemID;
            row["PackID"] = packID;
            row["InvItemCode"] = invItemCode;
            row["InvItemName"] = invItemName;
            row["PackName"] = packName;
            row["UnitPrice"] = unitPrice;
            row["UnitQuantity"] = unitQuantity;
            row["InvSpecPurchase"] = invSpecPurchase;
            row["TradeDiscountPercent"] = tradeDiscountPercent;
            row["TradeDiscountAmount"] = tradeDiscountAmount;
            row["FormPrintID"] = formPrintID;
            row["TotalBeforeVat"] = totalBeforeVat;
            row["VatPercent"] = vatPercent;
            row["VatAmount"] = vatAmount;
            row["NetAmount"] = netAmount;
            row["UnitOrder"] = unitOrder;
            row["PrID"] = prID;
            row["PrItemID"] = prItemID;
            row["PopupType"] = popupType;
            row["ProcureName"] = procureName;
            row["Grouped"] = grouped;
            row["Specify"] = specify;

            Table.Rows.Add(row);
            Table.AcceptChanges();
        }

        public bool hasPrItem()
        {
            DataRow[] rows = Table.Select("PrItemID <> ''");

            if (rows.Length > 0)
                return true;
            else
                return false;
        }

        public void UpdateItem
        (
            DataRow row,
            string invItemID,
            string packID,
            string invItemCode,
            string invItemName,
            string packName,
            string unitPrice,
            string unitQuantity,
            string invSpecPurchase,
            string tradeDiscountPercent,
            string tradeDiscountAmount,
            string formPrintID,
            string totalBeforeVat,
            string vatPercent,
            string vatAmount,
            string netAmount,
            string unitOrder,
            string prID,
            string prItemID,
            string popupType,
            string procureName,
            string specify,
            string grouped
        )
        {

            row["InvItemID"] = invItemID;
            row["PackID"] = packID;
            row["InvItemCode"] = invItemCode;
            row["InvItemName"] = invItemName;
            row["PackName"] = packName;
            row["UnitPrice"] = unitPrice;
            row["UnitQuantity"] = unitQuantity;
            row["InvSpecPurchase"] = invSpecPurchase;
            row["TradeDiscountPercent"] = tradeDiscountPercent;
            row["TradeDiscountAmount"] = tradeDiscountAmount;
            row["FormPrintID"] = formPrintID;
            row["TotalBeforeVat"] = totalBeforeVat;
            row["VatPercent"] = vatPercent;
            row["VatAmount"] = vatAmount;
            row["NetAmount"] = netAmount;
            row["UnitOrder"] = unitOrder;
            row["PrID"] = prID;
            row["PrItemID"] = prItemID;
            row["PopupType"] = popupType;
            row["ProcureName"] = procureName;
            row["Grouped"] = grouped;
            row["Specify"] = specify;

            row.AcceptChanges();
        }
    }

    /// <summary>
    ///     คลาสนี้อำนวยความสะดวกในการสร้างตารางชนิด PRPOActualTable
    ///     Popup ที่เรียกใช้ 
    ///         - pop_PRSelect
    ///         - pop_ProductReorderPointSelect
    ///         - pop_ProductSelect
    /// </summary>
    public static class PRPOActualTableFactory
    {
        /// <summary>
        ///     เมธอดสำหรับเลือกสร้างตาราง ควรใช้ enum ในพารา
        /// </summary>
        /// <param name="main">Po หรือ Pr</param>
        /// <param name="type">purchase, hire, 1, 2</param>
        /// <returns></returns>
        /// <remarks>ควรใช้ enum เป็นโดเมนในพารามิเตอร์ main, type</remarks>
        public static PRPOActualTable CreateTable(string main, string type)
        {
            if (main == "po" || main == "pr")
            {
                // เนื่องจากโค๊ดเก่า ใช้ type = 1 หรือ type = purchase
                if (type == "1" || type == "purchase")
                    return new PRPOPurchaseActualTable(PRPOSession.PurchaseActualTable);
                else if (type == "2" || type == "hire")
                    return new PRPOHireActualTable(PRPOSession.HireActualTable);
            }
            //else if (main == "pr")
            //{
            //    throw new NotImplementedException();
            //}

            return null;
        }
    }
}