using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GPlus.PRPO.PRPOHelper;
using System.Data.SqlClient;
using System.Data;

namespace GPlus.DataAccess
{
    public class PRDAO2 : DatabaseAccess
    {
        #region INSERT

        public string InsertInvPRForm1
        (
            string prCode
            , string prType
            , string typeInvAsset
            , DateTime requestDate
            , string orgStrucID
            , string projectID
            , string objective
            , string supplierID
            , string havePrintForm
            , string haveBudget
            , string quotationCode
            , DateTime quotationDate
            , string haveDiscount
            , string tradeDiscountType
            , string tradeDiscountPercent
            , string tradeDiscountAmount
            , string cashDiscountType
            , string cashDiscountPercent
            , string cashDiscountAmount
            , string vatType
            , string vat
            , string vatUnitType
            , string reason
            , string createBy
            , string totalPrice
            , string totalDiscount
            , string totalBeforeVat
            , string vatAmount
            , string netAmount
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_Code", prCode));
            param.Add(new SqlParameter("@PR_Type", prType));
            param.Add(new SqlParameter("@Type_Inv_Asset", typeInvAsset));
            if (requestDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Request_Date", requestDate));
            if (orgStrucID.Trim().Length > 0)
                param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));
            if (projectID.Trim().Length > 0)
                param.Add(new SqlParameter("@Project_ID", projectID));
            param.Add(new SqlParameter("@Objective", objective));
            if (supplierID.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_Id", supplierID));
            param.Add(new SqlParameter("@Have_PrintForm", havePrintForm));
            param.Add(new SqlParameter("@Have_Budget", haveBudget));
            param.Add(new SqlParameter("@Quotation_code", quotationCode));
            if (quotationDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date", quotationDate));
            param.Add(new SqlParameter("@TradeDiscount_Type", tradeDiscountType));
            param.Add(new SqlParameter("@HaveDiscount", haveDiscount));
            if (tradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountPercent));
            if (tradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Amount", tradeDiscountAmount));
            param.Add(new SqlParameter("@CashDiscount_Type", cashDiscountType));
            if (cashDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountPercent));
            if (cashDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Amount", cashDiscountAmount));
            param.Add(new SqlParameter("@Vat_Type", vatType));
            if (vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
            param.Add(new SqlParameter("@Reason", reason));
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

            return ((Decimal) ExecuteScalar("sp_Inv_PR_Form1_Insert", param)).ToString();
        }

        public string InsertInvPRItems
        (
            string prID
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
            param.Add(new SqlParameter("@PR_ID", prID));
            if (invItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_ItemID", invItemID));
            param.Add(new SqlParameter("@Procure_Name", procureName));
            param.Add(new SqlParameter("@Specify", specify));
            unitPrice = unitPrice.Replace("&nbsp;", "");
            unitQuantity = unitQuantity.Replace("&nbsp;", "");
            if (packID.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID", packID));
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

            return ((Decimal)ExecuteScalar("sp_Inv_PR_Items_Insert", param)).ToString();
        }

        public void InsertInvPRAttach(string prID, string filePath)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@Attach_Path", filePath));

            ExecuteNonQuery("sp_Inv_PR_Attach_Insert", param);
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

        public void InsertInvPRForm2
        (
            string prID
            , string expenseID
            , string accExpenseID
            , string percentAllocate
            , string amountAllocate
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            if (expenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@Expense_ID", expenseID));
            if (accExpenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@AccExpense_ID", accExpenseID));
            if (percentAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Percent_Allocate", percentAllocate));
            if (amountAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Amount_Allocate", amountAllocate));

            ExecuteNonQuery("sp_Inv_PR_Form2_Insert", param);
        }

        #endregion INSERT

        #region DELETE

        public void DeleteInvPRItem(string prItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PRItem_ID", prItemID));

            ExecuteNonQuery("sp_Inv_PR_Items_Delete", param);
        }
        public void DeleteInvPRAttach(string prAttachID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_AttachID", prAttachID));

            ExecuteNonQuery("sp_Inv_PR_Attach_Delete", param);
        }
        public void DeleteInvPRForm2(string form2ID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_Form2_ID", form2ID));

            ExecuteNonQuery("sp_Inv_PR_Form2_Delete", param);
        }
        public void DeletePrintForm(int printFormID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_FormPrint_ID", printFormID));

            ExecuteNonQuery("sp_Inv_PR_PO_FormPrint_Delete", param);
        }
        public void DeletePR(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            ExecuteNonQuery("sp_Inv_PR_Delete", param);
        }

        #endregion

        #region UPDATE

        public void UpdateInvPRForm1
        (
            string prID
            , DateTime requestDate
            , string orgStrucID
            , string projectID
            , string objective
            , string supplierID
            , string havePrintForm
            , string haveBudget
            , string quotationCode
            , DateTime quotationDate
            , string haveDiscount
            , string tradeDiscountType
            , string tradeDiscountPercent
            , string tradeDiscountAmount
            , string cashDiscountType
            , string cashDiscountPercent
            , string cashDiscountAmount
            , string vatType
            , string vat
            , string vatUnitType
            , string reason
            , string updateBy
            , string totalPrice
            , string totalDiscount
            , string totalBeforeVat
            , string vatAmount
            , string netAmount
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            if (requestDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Request_Date", requestDate));
            if (orgStrucID.Trim().Length > 0)
                param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));
            if (projectID.Trim().Length > 0)
                param.Add(new SqlParameter("@Project_ID", projectID));
            param.Add(new SqlParameter("@Objective", objective));
            if (supplierID.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_Id", supplierID));
            param.Add(new SqlParameter("@Have_PrintForm", havePrintForm));
            param.Add(new SqlParameter("@Have_Budget", haveBudget));
            param.Add(new SqlParameter("@Quotation_code", quotationCode));
            if (quotationDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date", quotationDate));
            param.Add(new SqlParameter("@HaveDiscount", haveDiscount));
            param.Add(new SqlParameter("@TradeDiscount_Type", tradeDiscountType));
            if (tradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountPercent));
            if (tradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Amount", tradeDiscountAmount));
            param.Add(new SqlParameter("@CashDiscount_Type", cashDiscountType));
            if (cashDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountPercent));
            if (cashDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Amount", cashDiscountAmount));
            param.Add(new SqlParameter("@Vat_Type", vatType));
            if (vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
            param.Add(new SqlParameter("@Reason", reason));
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

            ExecuteNonQuery("sp_Inv_PR_Form1_Update", param);
        }

        public void UpdatePRItem(int formPrintID, int prItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@FormPrintID", formPrintID));
            param.Add(new SqlParameter("@PRItemID", prItemID));

            ExecuteNonQuery("sp_Inv_PR_Item_UpdateFormPrintID", param);
        }

        public void UpdateInvPRItem
        (
            string prItemID, 
            string procureName, 
            string invItemID,
            string packID,
            string specify, 
            string unitPrice, 
            string unitQuantity,
            string tradeDiscountPercent, 
            string tradeDiscountAmount, 
            string cashDiscountPercent, 
            string cashDiscountAmount, 
            string totalBeforeVat, 
            string vat,
            string vatAmount, 
            string netAmount, 
            string specPurchase
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PRItem_ID", prItemID));

            param.Add(new SqlParameter("@Procure_Name", procureName));
            param.Add(new SqlParameter("@Specify", specify));

            if (invItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@Item_ID", invItemID));
            if (packID.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID", packID));

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

            ExecuteNonQuery("sp_Inv_PR_Items_Update2", param);
        }

        public void UpdateInvPRForm2
        (
            string prForm2ID
            , string expenseID
            , string accExpenseID
            , string percentAllocate
            , string amountAllocate
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_Form2_ID", prForm2ID));
            if (expenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@Expense_ID", expenseID));
            if (accExpenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@AccExpense_ID", accExpenseID));
            if (percentAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Percent_Allocate", percentAllocate));
            if (amountAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Amount_Allocate", amountAllocate));

            ExecuteNonQuery("sp_Inv_PR_Form2_Update", param);
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

        #endregion

        #region GET

        public DataTable GetPRItem(int prID, string type)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PrID", prID));
            param.Add(new SqlParameter("@Type", type));

            return ExecuteDataTable("sp_Inv_PR_Items_SelectByID2", param);
        }

        public DataTable GetPrintForm(int printFormID)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PrintFormID", printFormID));

            return ExecuteDataTable("sp_Inv_PR_PO_FormPrint_Select2", param);
        }

        public DataTable GetPRForm2(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            return ExecuteDataTable("sp_Inv_PR_Form2_SelectByID", param);
        }

        public DataTable GetRefPO(int prId)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@PrID", prId));

            return ExecuteDataTable("sp_Inv_PR_SelectRefPO", param);
        }

        #endregion
    }
}