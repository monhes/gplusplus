using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public sealed class PRPOPrintFormTable : TableBase
    {
        protected override void CreateColumns()
        {
            Table.Columns.Add("FormPrintID");
            Table.Columns.Add("PrItemID");
            Table.Columns.Add("PoItemID");
            Table.Columns.Add("FormPrintCode");
            Table.Columns.Add("FormPrintName");
            Table.Columns.Add("FormType");
            Table.Columns.Add("Format");
            Table.Columns.Add("PaperType");
            Table.Columns.Add("PaperColor");
            Table.Columns.Add("PaperGram");
            Table.Columns.Add("FontColor");
            Table.Columns.Add("PrintType");
            Table.Columns.Add("BorrowType");
            Table.Columns.Add("NewBorrowDate");
            Table.Columns.Add("Remark");
            Table.Columns.Add("FormBorrowType");
            Table.Columns.Add("BorrowQuantity");
            Table.Columns.Add("BorrowMonthQuantity");
            Table.Columns.Add("BorrowFirstQuantity");
            Table.Columns.Add("UnitPrice");                 // จัดเก็บค่า ราคา/หน่วย หลังจากการเลือกรหัสแบบพิมพ์
            Table.Columns.Add("BorrowUnitID");
            Table.Columns.Add("BorrowMonthUnitID");
            Table.Columns.Add("IsRequestModify");
            Table.Columns.Add("IsFixedContent");
            Table.Columns.Add("IsPaper");
            Table.Columns.Add("IsFont");
            Table.Columns.Add("Remark2");
            Table.Columns.Add("RequestModifyDesc");
            Table.Columns.Add("SizeDetail");
            Table.Columns.Add("UnitQuantity");
            // คอลัมน์ด้านล่างนี้ไม่ถูกนำมาใช้ใน PRPOPrintFormTable 
            // แต่จำเป็นต้องมีสำหรับการนำมา Bind กับ GVPurchase
            Table.Columns.Add("PrID");
            Table.Columns.Add("InvItemID");
            Table.Columns.Add("PackID");
            Table.Columns.Add("InvItemCode");
            Table.Columns.Add("InvItemName");
            Table.Columns.Add("PackName");
            Table.Columns.Add("TradeDiscountPercent");
            Table.Columns.Add("TradeDiscountAmount");
            Table.Columns.Add("TotalBeforeVat");
            Table.Columns.Add("VatPercent");
            Table.Columns.Add("VatAmount");
            Table.Columns.Add("NetAmount");
            Table.Columns.Add("PopupType");
            Table.Columns.Add("InvSpecPurchase");
        }

        public PRPOPrintFormTable()
        {
            Table.Rows.Add(Table.NewRow());

            Table.AcceptChanges();

            HttpContext.Current.Session[PRPOSession.PrintFormTable] = Table;
        }

        public PRPOPrintFormTable(string sessionName)
            : base(HttpContext.Current.Session[sessionName] as DataTable) { }

        public void ClearRow(int rowIndex)
        {
            DataRow row = Table.Rows[rowIndex];
            row["InvItemID"] = "";
            row["PackID"] = "";
            row["InvItemCode"] = "";
            row["InvItemName"] = "";
            row["PackName"] = "";
            row["UnitPrice"] = "";
            row["UnitQuantity"] = "";
            row["TradeDiscountPercent"] = "";
            row["TradeDiscountAmount"] = "";
            row["TotalBeforeVat"] = "";
            row["VatPercent"] = "";
            row["VatAmount"] = "";
            row["NetAmount"] = "";
            row["FormPrintID"] = "";
            row["PrItemID"] = "";
            row["PoItemID"] = "";
            row["FormPrintCode"] = "";
            row["FormPrintName"] = "";
            row["FormType"] = "";
            row["Format"] = "";
            row["PaperType"] = "";
            row["PaperColor"] = "";
            row["PaperGram"] = "";
            row["FontColor"] = "";
            row["PrintType"] = "";
            row["BorrowType"] = "";
            row["NewBorrowDate"] = DBNull.Value;
            row["Remark"] = "";
            row["FormBorrowType"] = "";
            row["BorrowQuantity"] = "";
            row["BorrowMonthQuantity"] = "";
            row["BorrowFirstQuantity"] = "";
            row["BorrowUnitID"] = "";
            row["BorrowMonthUnitID"] = "";
            row["IsRequestModify"] = "";
            row["IsFixedContent"] = "";
            row["IsPaper"] = "";
            row["IsFont"] = "";
            row["Remark2"] = "";
            row["UnitQuantity"] = "";
            row["RequestModifyDesc"] = "";
            row["SizeDetail"] = "";
            row["InvSpecPurchase"] = "";
        }

        public string GetFormType()
        {
            return Table.Rows[0]["FormType"].ToString();
        }

        public void AddItem
        (
            string formPrintID
            , string prItemID
            , string poItemID
            , string formPrintCode
            , string formPrintName
            , string formType
            , string format
            , string paperType
            , string paperGram
            , string paperColor
            , string fontColor
            , string printType
            , string borrowType
            , string newBorrowDate
            , string remark
            , string formBorrowType
            , string borrowQuantity
            , string borrowMonthQuantity
            , string borrowFirstQuantity
            , string unitPrice
            , string borrowUnitID
            , string borrowMonthUnitID
            , string isRequestModify
            , string isFixedContent
            , string isPaper
            , string isFont
            , string remark2
            , string requestModifyDesc
            , string sizeDetail
            , string invItemID
            , string packID
            , string invItemCode
            , string invItemName
            , string packName
            , string unitQuantity
            , string invSpecPurchase
        )
        {
            DataRow row = Table.Rows[0];
            row["FormPrintID"] = formPrintID;
            row["PrItemID"] = prItemID;
            row["PoItemID"] = poItemID;
            row["FormPrintCode"] = formPrintCode;
            row["FormPrintName"] = formPrintName;
            row["FormType"] = formType;
            row["Format"] = format;
            row["PaperType"] = paperType;
            row["PaperColor"] = paperColor;
            row["PaperGram"] = paperGram;
            row["FontColor"] = fontColor;
            row["PrintType"] = printType;
            row["BorrowType"] = borrowType;
            row["NewBorrowDate"] = newBorrowDate;
            row["Remark"] = remark;
            row["FormBorrowType"] = formBorrowType;
            row["BorrowQuantity"] = borrowQuantity;
            row["BorrowMonthQuantity"] = borrowMonthQuantity;
            row["BorrowFirstQuantity"] = borrowFirstQuantity;
            row["UnitPrice"] = unitPrice;               // จัดเก็บค่า ราคา/หน่วย หลังจากการเลือกรหัสแบบพิมพ์
            row["BorrowUnitID"] = borrowUnitID;
            row["BorrowMonthUnitID"] = borrowMonthUnitID;
            row["IsRequestModify"] = isRequestModify;
            row["IsFixedContent"] = isFixedContent;
            row["IsPaper"] = isPaper;
            row["IsFont"] = isFont;
            row["Remark2"] = remark2;
            row["RequestModifyDesc"] = requestModifyDesc;
            row["SizeDetail"] = sizeDetail;
            // คอลัมน์ด้านล่างนี้ไม่ถูกนำมาใช้ใน PRPOPrintFormTable 
            // แต่จำเป็นต้องมีสำหรับการนำมา Bind กับ GVPurchase
            //row["PrID"] = prID
            row["InvItemID"] = invItemID;
            row["PackID"] = packID;
            row["InvItemCode"] = invItemCode;
            row["InvItemName"] = invItemName;
            row["PackName"] = packName;
            row["UnitQuantity"] = unitQuantity;
            row["InvSpecPurchase"] = invSpecPurchase;
            //row["TradeDiscountPercent"] = 
            //row["TradeDiscountAmount"] =
            //row["TotalBeforeVat"] =
            //row["VatPercent"] =
            //row["VatAmount"] =
            //row["NetAmount"] =
            //row["PopupType"] =

            Table.AcceptChanges();
        }
    }
}