using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using GPlus.PRPO.PRPOHelper;

namespace GPlus.DataAccess
{
    public class PODAO2 : DatabaseAccess
    {
        #region INSERT

        public string InsertInvPoForm1
        (
            string poCode,                  // 1
            string poType, 
            string typeInvAsset, 
            DateTime poDate, 
            string orgStrucID, 
            string objective, 
            string projectID,
            string itemFrom, 
            string havePrintFrom, 
            string supplierID,              // 10 
            string quotationCode1, 
            DateTime quotationDate1, 
            string quotationCode2, 
            DateTime quotationDate2,
            string isPayCheque, 
            string isPayCash, 
            string creditTermDay, 
            DateTime shippingDate, 
            string shippingAt, 
            string haveDiscount,            // 20
            string tradingDiscountType, 
            string tradingDiscountP, 
            string tradingDiscountA, 
            string cashDiscountT, 
            string cashDiscountP, 
            string cashDiscountA, 
            string vatType, 
            string vat, 
            string vatUnitType, 
            string createBy,                // 30
            string totalPrice, 
            string totalDiscount, 
            string totalBeforeVat, 
            string vatAmount, 
            string netAmount, 
            string contractName,
            DateTime reorderPointDate
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PO_Code", poCode));
            param.Add(new SqlParameter("@PO_Type", poType));
            param.Add(new SqlParameter("@Type_Inv_Asset", typeInvAsset));
            if (poDate > DateTime.MinValue)
                param.Add(new SqlParameter("@PO_Date", poDate));
            if (orgStrucID.Trim().Length > 0)
                param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));
            param.Add(new SqlParameter("@Objective", objective));
            if (projectID.Trim().Length > 0)
                param.Add(new SqlParameter("@Project_ID", projectID));
            param.Add(new SqlParameter("@Item_From", itemFrom));
            param.Add(new SqlParameter("@Have_PrintForm", havePrintFrom));
            if (supplierID.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@Quotation_Code1", quotationCode1));
            if (quotationDate1 > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date1", quotationDate1));
            param.Add(new SqlParameter("@Quotation_Code2", quotationCode2));
            if (quotationDate2 > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date2", quotationDate2));
            param.Add(new SqlParameter("@Is_PayCheque", isPayCheque));
            param.Add(new SqlParameter("@Is_PayCash", isPayCash));
            if (creditTermDay.Trim().Length > 0)
                param.Add(new SqlParameter("@CreditTerm_Day", creditTermDay));
            if (shippingDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Shipping_Date", shippingDate));
            param.Add(new SqlParameter("@Shipping_At", shippingAt));
            param.Add(new SqlParameter("@HaveDiscount", haveDiscount));
            param.Add(new SqlParameter("@TradeDiscount_Type", tradingDiscountType));
            if (tradingDiscountP.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount__Percent", tradingDiscountP));
            if (tradingDiscountA.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount__Amount", tradingDiscountA));
            param.Add(new SqlParameter("@CashDiscount_Type", cashDiscountT));
            if (cashDiscountP.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount__Percent", cashDiscountP));
            if (cashDiscountA.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount__Amount", cashDiscountA));
            param.Add(new SqlParameter("@Vat_Type", vatType));
            if (vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
            param.Add(new SqlParameter("@Create_By", createBy));
            if (totalPrice.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Price", totalPrice));
            if (totalDiscount.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Discount", totalDiscount));
            if (totalBeforeVat.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Before_Vat", totalBeforeVat));
            if (vatAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@VAT_Amount", vatAmount));
            if (netAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Net_Amonut", netAmount));
            if (contractName.Trim().Length > 0)
                param.Add(new SqlParameter("@Contract_Name", contractName));
            if (reorderPointDate > DateTime.MinValue)
                param.Add(new SqlParameter("@ReorderPoint_Date", reorderPointDate));

            return ((Decimal) ExecuteScalar("sp_Inv_PO_Form1_Insert", param)).ToString();
        }

        public string InsertInvPOItem
        (
            string poID 
            , string invItemID
            , string procureName
            , string specify
            , string packID
            , string unitPrice
            , string unitQuantity
            , string tradeDiscountPercent
            , string tradeDiscountAmount
            , string cashDiscountPercent
            , string cashDiscountAmount
            , string totalBeforeVat
            , string vat
            , string vatAmount
            , string netAmount
            , string specPurchase
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PoID", poID));
            param.Add(new SqlParameter("@InvItemID", invItemID));
            param.Add(new SqlParameter("@ProcureName", procureName));
            param.Add(new SqlParameter("@Specify", specify));
            param.Add(new SqlParameter("@PackID", packID));
            param.Add(new SqlParameter("@UnitPrice", unitPrice));
            param.Add(new SqlParameter("@UnitQuantity", unitQuantity));
            param.Add(new SqlParameter("@TradeDiscountPercent", tradeDiscountPercent));
            param.Add(new SqlParameter("@TradeDiscountAmount", tradeDiscountAmount));
            param.Add(new SqlParameter("@CashDiscountPercent", cashDiscountPercent));
            param.Add(new SqlParameter("@CashDiscountAmount", cashDiscountAmount));
            param.Add(new SqlParameter("@TotalBeforeVat", totalBeforeVat));
            param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatAmount", vatAmount));
            param.Add(new SqlParameter("@NetAmount", netAmount));
            param.Add(new SqlParameter("@InvSpecPurchase", specPurchase));

            return ((Decimal) ExecuteScalar("sp_Inv_PO_Items_Insert2", param)).ToString();
        }

        public void InsertInvPOPR(int poID, int poItemID, int prID, int prItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@POItem_ID", poItemID));
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@PRItem_ID", prItemID));

            ExecuteNonQuery("sp_Inv_PO_PR_Insert", param);
        }

        public void InsertInvPOAttach(string poID, string attachPath)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Attach_Path", attachPath));

            ExecuteNonQuery("sp_Inv_PO_Attach_Insert", param);
        }

        public void InsertInvPOForm2(string poID, string expenseID, string accExpenseID, string percentAllocate, string amountAllocate)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            if (expenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@Expense_ID", expenseID));
            if (accExpenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@AccExpense_ID", accExpenseID));
            if (percentAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Percent_Allocate", percentAllocate));
            if (amountAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Amount_Allocate", amountAllocate));

            ExecuteNonQuery("sp_Inv_PO_Form2_Insert", param);
        }

        public string InsertInvPRPOFormPrint
        (
            string prItemID
            , string poItemID               // WARNING !!! This should be PO_ID 
            , string formPrintCode
            , string formPrintName
            , string formType
            , string format
            , string paperType
            , string paperColor
            , string paperGram
            , string fontColor
            , string printType
            , string borrowType
            , DateTime newBorrowDate
            , string remark
            , string formBorrowType
            , string borrowQuantity
            , string borrowMonthQuantity
            , string borrowFirstQuantity
            , string borrowUnitID
            , string borrowMonthUnitID
            , string isRequestModify
            , string isFixedContent
            , string isPaper
            , string isFont
            , string remark2
            , int unit_qty
            , string tbRequestModifyDesc
            , string tbSizeDetail
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if (prItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@PRItem_ID", prItemID));
            if (poItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@POItem_ID", poItemID));
            param.Add(new SqlParameter("@FormPrint_Code", formPrintCode));
            param.Add(new SqlParameter("@FormPrint_Name", formPrintName));
            param.Add(new SqlParameter("@Form_Type", formType));
            param.Add(new SqlParameter("@Format", format));
            param.Add(new SqlParameter("@Paper_Type", paperType));
            param.Add(new SqlParameter("@Paper_Color", paperColor));
            param.Add(new SqlParameter("@Paper_Gram", paperGram));
            param.Add(new SqlParameter("@Font_Color", fontColor));
            param.Add(new SqlParameter("@Print_Type", printType));
            param.Add(new SqlParameter("@Borrow_Type", borrowType));
            if (newBorrowDate > DateTime.MinValue)
                param.Add(new SqlParameter("@NewBorrow_Date", newBorrowDate));
            param.Add(new SqlParameter("@Remark", remark));
            param.Add(new SqlParameter("@FormBorrow_Type", formBorrowType));
            if (borrowQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Quantity", borrowQuantity));
            if (borrowMonthQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Month_Quantity", borrowMonthQuantity));
            if (borrowFirstQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_First_Quantity", borrowFirstQuantity));
            if (borrowUnitID.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Unit_ID", borrowUnitID));
            if (borrowMonthUnitID.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Month_Unit_ID", borrowMonthUnitID));
            param.Add(new SqlParameter("@Is_Request_Modify", isRequestModify));
            param.Add(new SqlParameter("@Is_Fixed_Content", isFixedContent));
            param.Add(new SqlParameter("@Is_Paper", isPaper));
            param.Add(new SqlParameter("@Is_Font", isFont));
            param.Add(new SqlParameter("@Remark2", remark2));
            param.Add(new SqlParameter("@Unit_Qty", unit_qty));

            if (tbRequestModifyDesc.Trim().Length > 0)
                param.Add(new SqlParameter("@Request_Modify_Desc", tbRequestModifyDesc));
            if (tbSizeDetail.Trim().Length > 0)
                param.Add(new SqlParameter("@Size_Detail", tbSizeDetail));

            return ((Decimal)ExecuteScalar("sp_Inv_PR_PO_FormPrint_Insert", param)).ToString();
        }

        #endregion

        #region GET

        public DataTable GetPOItem(int poID, string type)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PoID", poID));
            param.Add(new SqlParameter("@Type", type)); 

            return ExecuteDataTable("sp_Inv_PO_Items_Select2", param);
        }
        public DataTable GetPOAttach(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return ExecuteDataTable("sp_Inv_PO_Attach_Select", param);
        }
        public DataTable GetPOAttachById(int poAttachID)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@PoAttachId", poAttachID));

            return ExecuteDataTable("sp_Inv_PO_Attach_SelectById", sqlParams);
        }
        public DataTable GetPrintForm(int printFormID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PrintFormID", printFormID));

            return ExecuteDataTable("sp_Inv_PR_PO_FormPrint_Select2", param);
        }
        public DataTable GetPOForm2(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return ExecuteDataTable("sp_Inv_PO_Form2_SelectByID", param);
        }
        public DataTable GetPoPrByPoID(int poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PoID", poID));

            return ExecuteDataTable("sp_Inv_PO_PR_SelectByPoID", param);
        }

        public DataTable GetPrItemByPO(int poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("PoID", poID));

            return ExecuteDataTable("sp_Inv_PR_Items_SelectByPO", param);
        }

        /// <summary>
        ///     ดึงรายการ PrintForm ล่าสุดของ PO
        ///     
        /// </summary>
        /// <param name="invItemID"></param>
        /// <param name="packID"></param>
        /// <returns></returns>
        public DataTable GetLastestPOPrintForm(int invItemID, int packID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@InvItemID", invItemID));
            param.Add(new SqlParameter("@PackID", packID));

            return ExecuteDataTable("sp_Inv_PO_PrintForm_Latest", param);
        }

        public DataTable GetPrItemPrintForm(int prItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PrItemID", prItemID));

            return ExecuteDataTable("sp_Inv_PR_Items_PrintForm_SelectByID", param);
        }

        public DataTable GetRefPR(int poId)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PoID", poId));

            return ExecuteDataTable("sp_Inv_PO_SelectRefPR", param);
        }

        #endregion GET

        #region DELETE

        public void DeletePOAttach(string poAttachID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_AttachID", poAttachID));

            ExecuteNonQuery("sp_Inv_PO_Attach_Delete", param);
        }
        public void DeletePOItem(int poItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@POItemID", poItemID));

            ExecuteNonQuery("sp_Inv_PO_Items_Delete2", param);
        }
        public void DeletePOPR(int poID, int prID, int prItemID, int poItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PoID", poID));
            param.Add(new SqlParameter("@PrID", prID));
            param.Add(new SqlParameter("@PrItemID", prItemID));
            param.Add(new SqlParameter("@PoItemID", poItemID));

            ExecuteNonQuery("sp_Inv_PO_PR_Delete2", param);
        }
        public void DeletePrintForm(int printFormID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_FormPrint_ID", printFormID));

            ExecuteNonQuery("sp_Inv_PR_PO_FormPrint_Delete", param);
        }
        public void DeletePOForm2(string form2ID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_Form2_ID", form2ID));

            ExecuteNonQuery("sp_Inv_PO_Form2_Delete", param);
        }
        public void DeletePO(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            ExecuteNonQuery("sp_Inv_PO_Delete", param);
        }

        #endregion

        #region UPDATE

        public void UpdatePOForm1
        (
            string poID
            , string poType
            , string typeInvAsset
            , DateTime poDate
            , string orgStrucID
            , string objective
            , string projectID
            , string itemFrom
            , string havePrintFrom
            , string supplierID
            , string quotationCode1
            , DateTime quotationDate1
            , string quotationCode2
            , DateTime quotationDate2
            , string isPayCheque
            , string isPayCash
            , string creditTermDay
            , DateTime shippingDate
            , string shippingAt
            , string haveDiscount
            , string tradingDiscountType
            , string tradingDiscountP
            , string tradingDiscountA
            , string cashDiscountT
            , string cashDiscountP
            , string cashDiscountA
            , string vatType
            , string vat
            , string vatUnitType
            , string updateBy
            , string totalPrice
            , string totalDiscount
            , string totalBeforeVat
            , string vatAmount
            , string netAmount
            , string contractName
            , DateTime reorderPointDate
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@PO_Type", poType));
            param.Add(new SqlParameter("@Type_Inv_Asset", typeInvAsset));
            if (poDate > DateTime.MinValue)
                param.Add(new SqlParameter("@PO_Date", poDate));
            if (orgStrucID.Trim().Length > 0)
                param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));
            param.Add(new SqlParameter("@Objective", objective));
            if (projectID.Trim().Length > 0)
                param.Add(new SqlParameter("@Project_ID", projectID));
            param.Add(new SqlParameter("@Item_From", itemFrom));
            param.Add(new SqlParameter("@Have_PrintForm", havePrintFrom));
            if (supplierID.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@Quotation_Code1", quotationCode1));
            if (quotationDate1 > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date1", quotationDate1));
            param.Add(new SqlParameter("@Quotation_Code2", quotationCode2));
            if (quotationDate2 > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date2", quotationDate2));
            param.Add(new SqlParameter("@Is_PayCheque", isPayCheque));
            param.Add(new SqlParameter("@Is_PayCash", isPayCash));
            if (creditTermDay.Trim().Length > 0)
                param.Add(new SqlParameter("@CreditTerm_Day", creditTermDay));
            if (shippingDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Shipping_Date", shippingDate));
            param.Add(new SqlParameter("@Shipping_At", shippingAt));
            param.Add(new SqlParameter("@HaveDiscount", haveDiscount));
            param.Add(new SqlParameter("@TradeDiscount_Type", tradingDiscountType));
            if (tradingDiscountP.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount__Percent", tradingDiscountP));
            if (tradingDiscountA.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount__Amount", tradingDiscountA));
            param.Add(new SqlParameter("@CashDiscount_Type", cashDiscountT));
            if (cashDiscountP.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount__Percent", cashDiscountP));
            if (cashDiscountA.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount__Amount", cashDiscountA));
            param.Add(new SqlParameter("@Vat_Type", vatType));
            if (vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
            param.Add(new SqlParameter("@Update_By", updateBy));
            if (totalPrice.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Price", totalPrice));
            if (totalDiscount.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Discount", totalDiscount));
            if (totalBeforeVat.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Before_Vat", totalBeforeVat));
            if (vatAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@VAT_Amount", vatAmount));
            if (netAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Net_Amonut", netAmount));
            if (contractName.Trim().Length > 0)
                param.Add(new SqlParameter("@Contract_Name", contractName));
            if (reorderPointDate > DateTime.MinValue)
                param.Add(new SqlParameter("@ReorderPoint_Date", reorderPointDate));

            ExecuteNonQuery("sp_Inv_PO_Form1_Update", param);
        }

        public void UpdatePOItem
        (
            string poItemID
            , string procureName
            , string specify
            , string itemId
            , int packId
            , string unitPrice
            , string unitQuantity
            , string tradeDiscountPercent
            , string tradeDiscountAmount
            , string cashDiscountPercent
            , string cashDiscountAmount
            , string totalBeforeVat
            , string vat
            , string vatAmount
            , string netAmount
            , string specPurchase
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@POItem_ID", poItemID));

            param.Add(new SqlParameter("@Procure_Name", procureName));
            param.Add(new SqlParameter("@Specify", specify));
            param.Add(new SqlParameter("@PackID", packId));

            if (itemId.Trim().Length > 0)
                param.Add(new SqlParameter("@ItemID", itemId));

            if (unitPrice.Trim().Length > 0)
                param.Add(new SqlParameter("@Unit_Price", unitPrice));
            if (unitQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Unit_Quantity", unitQuantity));
            if (tradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountPercent));
            if (tradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Amount", tradeDiscountAmount));
            if (cashDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountPercent));
            if (cashDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Amount", cashDiscountAmount));
            if (totalBeforeVat.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_before_Vat", totalBeforeVat));
            if (vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            if (vatAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat_Amount", vatAmount));
            if (netAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Net_Amount", netAmount));
            if (specPurchase.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_SpecPurchase", specPurchase));

            ExecuteNonQuery("sp_Inv_PO_Items_Update", param);
        }

        public void UpdatePOItem(int formPrintID, int poItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@FormPrintID", formPrintID));
            param.Add(new SqlParameter("@POItemID", poItemID));

            ExecuteNonQuery("sp_Inv_PO_Item_UpdateFormPrintID", param);
        }

        public void UpdatePrintForm
        (
            string formPrintID
            , string formPrintCode
            , string formPrintName
            , string formType
            , string format
            , string paperType
            , string paperColor
            , string paperGram
            , string fontColor
            , string printType
            , string borrowType
            , DateTime newBorrowDate
            , string remark
            , string formBorrowType
            , string borrowQuantity
            , string borrowMonthQuantity
            , string borrowFirstQuantity
            , string borrowUnitID
            , string borrowMonthUnitID
            , string isRequestModify
            , string isFixedContent
            , string isPaper
            , string isFont
            , string remark2
            , int unit_qty
            , string requestModifyDesc
            , string sizeDetail
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_FormPrint_ID", formPrintID));
            param.Add(new SqlParameter("@FormPrint_Code", formPrintCode));
            param.Add(new SqlParameter("@FormPrint_Name", formPrintName));
            param.Add(new SqlParameter("@Form_Type", formType));
            param.Add(new SqlParameter("@Format", format));
            param.Add(new SqlParameter("@Paper_Type", paperType));
            param.Add(new SqlParameter("@Paper_Color", paperColor));
            param.Add(new SqlParameter("@Paper_Gram", paperGram));
            param.Add(new SqlParameter("@Font_Color", fontColor));
            param.Add(new SqlParameter("@Print_Type", printType));
            param.Add(new SqlParameter("@Borrow_Type", borrowType));
            if (newBorrowDate > DateTime.MinValue)
                param.Add(new SqlParameter("@NewBorrow_Date", newBorrowDate));
            param.Add(new SqlParameter("@Remark", remark));
            param.Add(new SqlParameter("@FormBorrow_Type", formBorrowType));
            if (borrowQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Quantity", borrowQuantity));
            if (borrowMonthQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Month_Quantity", borrowMonthQuantity));
            if (borrowFirstQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_First_Quantity", borrowFirstQuantity));
            if (borrowUnitID.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Unit_ID", borrowUnitID));
            if (borrowMonthUnitID.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Month_Unit_ID", borrowMonthUnitID));
            param.Add(new SqlParameter("@Is_Request_Modify", isRequestModify));
            param.Add(new SqlParameter("@Is_Fixed_Content", isFixedContent));
            param.Add(new SqlParameter("@Is_Paper", isPaper));
            param.Add(new SqlParameter("@Is_Font", isFont));
            param.Add(new SqlParameter("@Remark2", remark2));
            param.Add(new SqlParameter("@Unit_Qty", unit_qty));

            if (requestModifyDesc.Trim().Length > 0)
                param.Add(new SqlParameter("@Request_Modify_Desc", requestModifyDesc));
            if (sizeDetail.Trim().Length > 0)
                param.Add(new SqlParameter("@Size_Detail", sizeDetail));

            ExecuteNonQuery("sp_Inv_PR_PO_FormPrint_Update", param);
        }

        public void UpdatePOForm2
        (
            string poForm2ID
            , string expenseID
            , string accExpenseID
            , string percentAllocate
            , string amountAllocate
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_Form2_ID", poForm2ID));
            if (expenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@Expense_ID", expenseID));
            if (accExpenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@AccExpense_ID", accExpenseID));
            if (percentAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Percent_Allocate", percentAllocate));
            if (amountAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Amount_Allocate", amountAllocate));

            ExecuteNonQuery("sp_Inv_PO_Form2_Update", param);
        }

        public void UpdatePRForm1(string prID, string status)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@Status", status));

            ExecuteNonQuery("sp_Inv_PR_Form1_UpdateStatus", param);
        }

        public void UpdatePrItemStatus(int prItemId, string status)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PrItemID", prItemId));
            if (status != null)
                param.Add(new SqlParameter("@Status", status));

            ExecuteNonQuery("sp_Inv_PR_Items_Update_Item_Status", param);
        }

        public void UpdatePOForm1Status(int poID, string status)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();

            sqlParams.Add(new SqlParameter("@PoID", poID));
            sqlParams.Add(new SqlParameter("@PoStatus", status));

            ExecuteNonQuery("sp_Inv_PO_Form1_UpdateStatus2", sqlParams);
        }

        public void UpdatePOForm1PaymentNo(int poID, string paymentNo)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PoId", poID));
            param.Add(new SqlParameter("@PaymentNo", paymentNo));

            ExecuteNonQuery("sp_Inv_PO_Form1_UpdatePaymentNo", param);
        }

        #endregion
    }
}