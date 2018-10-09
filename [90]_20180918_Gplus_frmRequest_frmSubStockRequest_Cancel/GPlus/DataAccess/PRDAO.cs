using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class PRDAO
    {
        public DataSet GetPRForm1(string prCode, string prType, string orgStructID, string startCreateDate, string endCreateDate,
            string status, string isWait, string startRequestDate, string endRequestDate,String considerType, string isApprover,
            int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_Code", prCode));
            param.Add(new SqlParameter("@PR_Type", prType));
            param.Add(new SqlParameter("@OrgStruc_ID", orgStructID));
            param.Add(new SqlParameter("@CreateDate_Start", startCreateDate));
            param.Add(new SqlParameter("@CreateDate_End", endCreateDate));

            param.Add(new SqlParameter("@RequestDate_Start", startRequestDate));
            param.Add(new SqlParameter("@RequestDate_End", endRequestDate));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Is_Wait", isWait));
            param.Add(new SqlParameter("@Consider_Type", considerType));
            param.Add(new SqlParameter("@IsApprover", isApprover));

            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PR_Form1_SelectPaging", param);
        }

        public DataTable GetPRForm1(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PR_Form1_SelectByID", param);
        }

        public string AddPRForm1(string prCode, string prType, string typeInvAsset, DateTime requestDate, string orgStrucID, string projectID, string objective,
            string supplierID, string havePrintForm ,string haveBudget, string quotationCode, DateTime quotationDate, string  haveDiscount, string tradeDiscountType, 
            string tradeDiscountPercent, string tradeDiscountAmount, string cashDiscountType , string cashDiscountPercent, string cashDiscountAmount,
            string vatType, string vat, string vatUnitType, string reason, string createBy, string totalPrice, string totalDiscount, 
            string totalBeforeVat, string vatAmount, string netAmount)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_Code", prCode));
            param.Add(new SqlParameter("@PR_Type", prType));
            param.Add(new SqlParameter("@Type_Inv_Asset", typeInvAsset));
            if(requestDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Request_Date", requestDate));
            if(orgStrucID.Trim().Length > 0)
                param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));
            if(projectID.Trim().Length > 0)
                param.Add(new SqlParameter("@Project_ID", projectID));
            param.Add(new SqlParameter("@Objective", objective));
            if(supplierID.Trim().Length> 0)
                param.Add(new SqlParameter("@Supplier_Id", supplierID));
            param.Add(new SqlParameter("@Have_PrintForm", havePrintForm));
            param.Add(new SqlParameter("@Have_Budget", haveBudget));
            param.Add(new SqlParameter("@Quotation_code", quotationCode));
            if(quotationDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Quotation_Date", quotationDate));
            param.Add(new SqlParameter("@TradeDiscount_Type", tradeDiscountType));
            param.Add(new SqlParameter("@HaveDiscount", haveDiscount));
            if(tradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountPercent));
            if(tradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Amount", tradeDiscountAmount));
            param.Add(new SqlParameter("@CashDiscount_Type", cashDiscountType));
            if(cashDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountPercent));
            if(cashDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Amount", cashDiscountAmount));
            param.Add(new SqlParameter("@Vat_Type", vatType));
            if(vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
            param.Add(new SqlParameter("@Reason", reason));
            param.Add(new SqlParameter("@Create_By", createBy));
            if(totalPrice.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Price", totalPrice));
            if(totalDiscount.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Discount", totalDiscount));
            if(totalBeforeVat.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Before_Vat", totalBeforeVat));
            if(vatAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@VAT_Amount", vatAmount));
            if(netAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Net_Amonut", netAmount));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_PR_Form1_Insert", param).ToString();
        }


        public void UpdatePRForm1(string prID, DateTime requestDate, string orgStrucID, string projectID, string objective,
            string supplierID, string havePrintForm, string haveBudget, string quotationCode, DateTime quotationDate, string haveDiscount, string tradeDiscountType,
            string tradeDiscountPercent, string tradeDiscountAmount, string cashDiscountType, string cashDiscountPercent, string cashDiscountAmount,
            string vatType, string vat, string vatUnitType, string reason, string updateBy, string totalPrice, string totalDiscount,
            string totalBeforeVat, string vatAmount, string netAmount)
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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form1_Update", param);
        }

        public void UpdatePRForm1(string prID, string status)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@Status", status));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form1_UpdateStatus", param);
        }

        public void UpdatePRForm1Approve(string prID, string status, string reason, string approveID, DateTime approveDate, string tempApproveID,
            DateTime tempApproveDate, string tempReason)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Consider_Type", status));
            if(reason.Trim().Length >  0)
                param.Add(new SqlParameter("@Reason", reason));
            if(approveID.Trim().Trim().Length > 0)
                param.Add(new SqlParameter("@Approver_ID", approveID));
            if (approveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Approve_Date", approveDate));
            if (tempApproveID.Trim().Length > 0)
                param.Add(new SqlParameter("@Temp_Approver_ID", tempApproveID));
            if(tempApproveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Temp_Approver_Date", tempApproveDate));
            if (tempReason.Trim().Length > 0)
                param.Add(new SqlParameter("@Temp_Reason", tempReason));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form1_UpdateApprove", param);
        }

        public void DeletePR(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Delete", param);
        }


        public DataTable GetPRForm2(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PR_Form2_SelectByID", param);
        }

        public void AddPRForm2(string prID, string expenseID, string accExpenseID, string percentAllocate, string amountAllocate)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            if(expenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@Expense_ID", expenseID));
            if(accExpenseID.Trim().Length > 0)
                param.Add(new SqlParameter("@AccExpense_ID", accExpenseID));
            if(percentAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Percent_Allocate", percentAllocate));
            if(amountAllocate.Trim().Length > 0)
                param.Add(new SqlParameter("@Amount_Allocate", amountAllocate));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form2_Insert", param);
        }

        public void UpdatePRForm2(string prForm2ID, string expenseID, string accExpenseID, string percentAllocate, string amountAllocate)
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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form2_Update", param);
        }

        public void UpdatePRForm2(string oldPRID, string newPRID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Old_PR_ID", oldPRID));
            param.Add(new SqlParameter("@PR_ID", newPRID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form2_UpdateID", param);
        }

        internal void DeletePRForm2(string form2ID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_Form2_ID", form2ID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Form2_Delete", param);
        }


        public DataTable GetPRItem(string prID, string isType2 = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@Is_Type2", isType2));

           return new DatabaseHelper().ExecuteDataTable("sp_Inv_PR_Items_SelectByID", param);
        }

        public bool AddPRItem(string prID, string invItemID, string procureName, string specify, string packID, string unitPrice, string unitQuantity,
            string tradeDiscountPercent,string tradeDiscountAmount, string cashDiscountPercent, string cashDiscountAmount, string totalBeforeVat, string vat,
            string vatAmount, string netAmount, string specPurchase)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            if(invItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_ItemID", invItemID));
            param.Add(new SqlParameter("@Procure_Name", procureName));
            param.Add(new SqlParameter("@Specify", specify));
            unitPrice = unitPrice.Replace("&nbsp;", "");
            unitQuantity = unitQuantity.Replace("&nbsp;", "");
            if(packID.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID", packID));
            if(unitPrice.Trim().Length > 0)
                param.Add(new SqlParameter("@Unit_Price", unitPrice));
            if(unitQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Unit_Quantity", unitQuantity));
            if(tradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountPercent));
            if(tradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Amount", tradeDiscountAmount));
            if(cashDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountPercent));
            if(cashDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Amount", cashDiscountAmount));
            if(totalBeforeVat.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_before_Vat", totalBeforeVat));
            if(vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            if(vatAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat_Amount", vatAmount));
            if(netAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Net_Amount", netAmount));
            if (specPurchase.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_SpecPurchase", specPurchase));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_PR_Items_Insert", param).ToString() == "1";
        }

        public void UpdatePRItem(string prItemID, string procureName, string specify, string unitPrice, string unitQuantity,
           string tradeDiscountPercent, string tradeDiscountAmount, string cashDiscountPercent, string cashDiscountAmount, string totalBeforeVat, string vat,
           string vatAmount, string netAmount, string specPurchase, string packID = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PRItem_ID", prItemID));
            
            param.Add(new SqlParameter("@Procure_Name", procureName));
            param.Add(new SqlParameter("@Specify", specify));
            
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

            if (packID.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID", packID));
            if (specPurchase.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_SpecPurchase", specPurchase));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Items_Update", param);
        }

        public void UpdatePRItemForFormPrint(string prID, string prFormPrintID, string itemID, string packID, string unitPrice, string unitQuantity,
            string tradeDiscountPercent,string tradeDiscountAmount, string cashDiscountPercent, string cashDiscountAmount, string totalBeforeVat, string vat,
            string vatAmount, string netAmount)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID",prID));
            param.Add(new SqlParameter("@PR_FormPrint_ID",prFormPrintID));

            if (itemID.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_ItemID", itemID));
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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Item_UpdateForForm", param);
        }


        public void UpdatePRItem(string oldPRID, string newPRID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Old_PR_ID", oldPRID));
            param.Add(new SqlParameter("@PR_ID", newPRID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Items_UpdateID", param);
        }

        public void DeletePRItem(string prItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PRItem_ID", prItemID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Items_Delete", param);
        }

        public DataTable GetFromPrint(string prID, string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if (prID.Trim().Length > 0)
                param.Add(new SqlParameter("@PR_ID", prID));
            if (poID.Trim().Length > 0)
                param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PR_PO_FormPrint_SelectByID", param);
        }

        public string AddFromPrint(string prItemID, string poItemID,string formPrintCode, string formPrintName, string formType, string format, string paperType,
            string paperColor, string paperGram, string fontColor, string printType, string borrowType, DateTime newBorrowDate, string remark, 
            string formBorrowType, string borrowQuantity, string borrowMonthQuantity, string borrowFirstQuantity, string borrowUnitID, string borrowMonthUnitID,
            string isRequestModify, string isFixedContent, string isPaper, string isFont, string remark2, int unit_qty,
            string tbRequestModifyDesc, string tbSizeDetail)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if(prItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@PRItem_ID", prItemID));
            if(poItemID.Trim().Length > 0)
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
            if(newBorrowDate > DateTime.MinValue)
                param.Add(new SqlParameter("@NewBorrow_Date",newBorrowDate));
            param.Add(new SqlParameter("@Remark", remark));
            param.Add(new SqlParameter("@FormBorrow_Type", formBorrowType));
            if(borrowQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Quantity", borrowQuantity));
            if(borrowMonthQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Month_Quantity", borrowMonthQuantity));
            if (borrowFirstQuantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_First_Quantity", borrowFirstQuantity));
            if(borrowUnitID.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Unit_ID",borrowUnitID));
            if(borrowMonthUnitID.Trim().Length > 0)
                param.Add(new SqlParameter("@Borrow_Month_Unit_ID", borrowMonthUnitID));
            param.Add(new SqlParameter("@Is_Request_Modify", isRequestModify));
            param.Add(new SqlParameter("@Is_Fixed_Content", isFixedContent));
            param.Add(new SqlParameter("@Is_Paper", isPaper));
            param.Add(new SqlParameter("@Is_Font", isFont));
            param.Add(new SqlParameter("@Remark2", remark2));
            param.Add(new SqlParameter("@Unit_Qty", unit_qty));
            // Begin Green Edit
            if (tbRequestModifyDesc.Trim().Length > 0)
                param.Add(new SqlParameter("@Request_Modify_Desc", tbRequestModifyDesc));
            if (tbSizeDetail.Trim().Length > 0)
                param.Add(new SqlParameter("@Size_Detail", tbSizeDetail));
            // End Green Edit

            return new DatabaseHelper().ExecuteScalar("sp_Inv_PR_PO_FormPrint_Insert", param).ToString();
        }

        public void UpdateFromPrint(string formPrintID, string formPrintCode, string formPrintName, string formType, string format, string paperType,
            string paperColor, string paperGram, string fontColor, string printType, string borrowType, DateTime newBorrowDate, string remark,
            string formBorrowType, string borrowQuantity, string borrowMonthQuantity, string borrowFirstQuantity, string borrowUnitID, string borrowMonthUnitID,
            string isRequestModify, string isFixedContent, string isPaper, string isFont, string remark2, int unit_qty,
            string requestModifyDesc,
            string sizeDetail)
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

            // Begin Green Edit
            if (requestModifyDesc.Trim().Length > 0) 
                param.Add(new SqlParameter("@Request_Modify_Desc", requestModifyDesc));
            if (sizeDetail.Trim().Length > 0)
                param.Add(new SqlParameter("@Size_Detail", sizeDetail));
            // End Green Edit

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_PO_FormPrint_Update", param);
        }

        public void UpdateFormPrint(string oldPRItemID, string prItemID, string oldPOItemID, string poItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if(oldPRItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@Old_PRItem_ID", oldPRItemID));
            if(prItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@PR_Item_ID", prItemID));
            if(oldPOItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@Old_POItem_ID", oldPOItemID));
            if(poItemID.Trim().Length > 0)
                param.Add(new SqlParameter("@PO_Item_ID", poItemID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_PO_FormPrint_UpdateReference", param);
        }

        public void DeleteFormPrint(string formPrintID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_FormPrint_ID", formPrintID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_PO_FormPrint_Delete", param);
        }


        public DataSet GetPRForm1andItem(string startCreateDate, string endCreateDate, string supplierID,
       string status, string prType, int pageNum, int pageSize, string sortField, string sortOrder, string prCode)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@CreateDate_Start", startCreateDate));
            param.Add(new SqlParameter("@CreateDate_End", endCreateDate));

            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@PR_Type", prType));

            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));
            param.Add(new SqlParameter("@PrCode", prCode));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PR_Form1_SelectItemPaging", param);
        }

        public DataTable GetPRAttach(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PR_Attach_Select", param);
        }

        public void AddPRAttach(string prID, string filePath)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@Attach_Path", filePath));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Attach_Insert", param);
        }

        public void DeletePRAttach(string prAttachID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_AttachID", prAttachID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PR_Attach_Delete", param);
        }

        public DataSet GetPRReport(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PR_SelectReport", param);
        }

        public  DataSet GetPOReport(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PO_SelectReport", param);
        }

        public DataSet GetPRReport2(string prID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PR_Form2_SelectReport", param);
        }
    }
}
