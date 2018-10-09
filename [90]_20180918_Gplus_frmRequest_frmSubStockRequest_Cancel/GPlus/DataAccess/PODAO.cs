using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class PODAO
    {
        public DataSet GetPOForm1(
            string poCode,
            string poType,
            string typeInvAsset,
            string poDateStart,
            string poDateEnd,
            string createDateStart,
            string createDateEnd,
            string haveUpload,
            string supplierID,
            string status,
            string isWait,
            int pageNum,
            int pageSize,
            string sortField,
            string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_Code", poCode));
            param.Add(new SqlParameter("@PO_Type", poType));
            param.Add(new SqlParameter("@Type_Inv_Asset", typeInvAsset));
            param.Add(new SqlParameter("@PODate_Start", poDateStart));
            param.Add(new SqlParameter("@PODate_End", poDateEnd));
            param.Add(new SqlParameter("@CreateDate_Start", createDateStart));
            param.Add(new SqlParameter("@CreateDate_End", createDateEnd));
            param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@HaveUpload", haveUpload));

            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Is_Wait", isWait));

            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PO_Form1_SelectPaging", param);
        }

        public DataTable GetPOForm1(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PO_Form1_SelectByID", param);
        }

        public string AddPOForm1
        (
            string poCode, string poType, string typeInvAsset, DateTime poDate, string orgStrucID, string objective, string projectID,
            string itemFrom, string havePrintFrom, string supplierID, string quotationCode1, DateTime quotationDate1, string quotationCode2, DateTime quotationDate2,
            string isPayCheque, string isPayCash, string creditTermDay, DateTime shippingDate, string shippingAt, string haveDiscount,
            string tradingDiscountType, string tradingDiscountP,
            string tradingDiscountA, string cashDiscountT, string cashDiscountP, string cashDiscountA, string vatType, string vat, string vatUnitType, string createBy,
            string totalPrice, string totalDiscount, string totalBeforeVat, string vatAmount, string netAmount, string contractName
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

            // Begin Green Edit
            if (contractName.Trim().Length > 0)
                param.Add(new SqlParameter("@Contract_Name", contractName));
            // End Green Edit

            return new DatabaseHelper().ExecuteScalar("sp_Inv_PO_Form1_Insert", param).ToString();
        }


        public void UpdatePOForm1(string poID, string poType, string typeInvAsset, DateTime poDate, string orgStrucID, string objective, string projectID,
            string itemFrom, string havePrintFrom, string supplierID, string quotationCode1, DateTime quotationDate1, string quotationCode2, DateTime quotationDate2,
            string isPayCheque, string isPayCash, string creditTermDay, DateTime shippingDate, string shippingAt, string haveDiscount,
            string tradingDiscountType, string tradingDiscountP,
            string tradingDiscountA, string cashDiscountT, string cashDiscountP, string cashDiscountA, string vatType, string vat, string vatUnitType, string updateBy,
            string totalPrice, string totalDiscount, string totalBeforeVat, string vatAmount, string netAmount, string contractName)
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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form1_Update", param);
        }

        public void POForm1Update(string poID, string status, string reason, string approverID, DateTime approverDate, string tempApproverID,
            DateTime tempApproverDate, string updateBy, string considerType)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Reason", reason));
            if (approverID.Trim().Length > 0)
                param.Add(new SqlParameter("@Approver_ID", approverID));
            if (approverDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Approve_Date", approverDate));
            if (tempApproverID.Trim().Length > 0)
                param.Add(new SqlParameter("@Temp_Approver_ID", tempApproverID));
            if (tempApproverDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Temp_Approver_Date", tempApproverDate));
            param.Add(new SqlParameter("@Update_By", updateBy));
            param.Add(new SqlParameter("@considerType", considerType));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form1_UpdateStatus", param);
        }


        public void UpdatePOForm1(string poID, string status)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Status", status));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form1_UpdateStatus1", param);
        }

        public DataTable GetPOItem(string poID, string isType2 = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Is_Type2", isType2));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PO_Items_Select", param);
        }

        public void AddPOItem(string poID, string invItemID, string procureName, string specify, string packID, string unitPrice, string unitQuantity,
            string tradeDiscountP, string tradeDiscountA, string cashDiscountP, string cashDiscountA, string totalBeforeVat, string vat, string vatAmount,
            string netAmount, string specPurchase)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Inv_ItemID", invItemID));
            param.Add(new SqlParameter("@Procure_Name", procureName));
            param.Add(new SqlParameter("@Specify", specify));
            param.Add(new SqlParameter("@Pack_ID", packID));
            unitPrice = unitPrice.Replace("&nbsp;", "");
            unitQuantity = unitQuantity.Replace("&nbsp;", "");
            if (unitPrice.Trim().Length > 0)
                //param.Add(new SqlParameter("@Unit_Price", unitPrice));
                param.Add(new SqlParameter("@Unit_Price", unitPrice.Replace(",", "")));
            if (unitQuantity.Trim().Length > 0)
                //param.Add(new SqlParameter("@Unit_Quantity", unitQuantity));
                param.Add(new SqlParameter("@Unit_Quantity", unitQuantity.Replace(",", "")));
            if (tradeDiscountP.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountP));
            if (tradeDiscountA.Trim().Length > 0)
                param.Add(new SqlParameter("@TradeDiscount_Amount", tradeDiscountA));
            if (cashDiscountP.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountP));
            if (cashDiscountA.Trim().Length > 0)
                param.Add(new SqlParameter("@CashDiscount_Amount", cashDiscountA));
            if (totalBeforeVat.Trim().Length > 0)
                param.Add(new SqlParameter("@Total_Before_Vat", totalBeforeVat));
            if (vat.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat", vat));
            if (vatAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Vat_Amount", vatAmount));
            if (netAmount.Trim().Length > 0)
                //param.Add(new SqlParameter("@Net_Amount", netAmount));
                param.Add(new SqlParameter("@Net_Amount", netAmount.Replace(",", "")));
            param.Add(new SqlParameter("@Inv_SpecPurchase", specPurchase));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Items_Insert", param);
        }

        public void AddPOItemByPR(string prID, string poID, string prItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PR_ID", prID));
            param.Add(new SqlParameter("@PRItem_ID", prItemID));
            param.Add(new SqlParameter("@PO_ID", poID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_InsertItemByPR", param);
        }

        public void UpdatePOItem(string poItemID, string procureName, string specify, string unitPrice, string unitQuantity,
           string tradeDiscountPercent, string tradeDiscountAmount, string cashDiscountPercent, string cashDiscountAmount, string totalBeforeVat, string vat,
           string vatAmount, string netAmount, string specPurchase)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@POItem_ID", poItemID));

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
            if (specPurchase.Trim().Length > 0)
                param.Add(new SqlParameter("@Inv_SpecPurchase", specPurchase));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Items_Update", param);
        }

        public void UpdatePOItemForFormPrint(string poID, string poFormPrintID, string itemID, string packID, string unitPrice, string unitQuantity,
            string tradeDiscountPercent, string tradeDiscountAmount, string cashDiscountPercent, string cashDiscountAmount, string totalBeforeVat, string vat,
            string vatAmount, string netAmount)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@PO_FormPrint_ID", poFormPrintID));

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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Item_UpdateForForm", param);
        }

        public void UpdatePOItem(string oldPOID, string newPOID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Old_PO_ID", oldPOID));
            param.Add(new SqlParameter("@PO_ID", newPOID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Items_UpdateID", param);
        }


        public void DeletePOItem(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@POItem_ID", poID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Items_Delete", param);
        }



        public DataTable GetPOAttach(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PO_Attach_Select", param);
        }

        public void AddPOAttach(string poID, string attachPath)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Attach_Path", attachPath));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Attach_Insert", param);
        }

        public void DeletePOAttach(string poAttachID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_AttachID", poAttachID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Attach_Delete", param);
        }


        public DataTable GetPOUpDown(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PO_UpDown_Select", param);
        }

        public void AddPOUpDown(string poID, string uploadBy, string uploadPath, string uploadFile)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Upload_By", uploadBy));
            param.Add(new SqlParameter("@Upload_File", uploadFile));
            param.Add(new SqlParameter("@Upload_Path", uploadPath));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_UpDown_Insert", param);
        }

        public void UpdatePOUpDown(string poUploadID, string downloadBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_Upload_ID", poUploadID));
            param.Add(new SqlParameter("@Latest_Download_by", downloadBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_UpDown_UpdateDownload", param);
        }


        public void AddPOLog(DateTime transactionDate, string accountUserName, string supplierID, string actionFlag, string result, string screenCode, string poUploadID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if (transactionDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Transaction_Date", transactionDate));
            param.Add(new SqlParameter("@Account_Username", accountUserName));
            if (supplierID.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@Action_Flag", actionFlag));
            param.Add(new SqlParameter("@Result", result));
            if (screenCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Screen_Code", screenCode));
            if (poUploadID.Trim().Length > 0)
                param.Add(new SqlParameter("@PO_Upload_ID", poUploadID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Log_Insert", param);
        }


        public void DeletePO(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Delete", param);
        }


        public DataTable GetPOPR(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataTable("sp_PO_PR_Select", param);
        }

        public DataSet GetPOReport2(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PO_SelectReport3", param);
        }

        public DataSet GetPOPRReport(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PO_SelectPRReport", param);
        }

        public DataSet GetPOPrintFormReport(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_PO_SelectFormPrintReport2", param);
        }

        public DataTable GetPOForm2(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PO_Form2_SelectByID", param);
        }

        public void AddPOForm2(string poID, string expenseID, string accExpenseID, string percentAllocate, string amountAllocate)
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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form2_Insert", param);
        }

        public void UpdatePOForm2(string poForm2ID, string expenseID, string accExpenseID, string percentAllocate, string amountAllocate)
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

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form2_Update", param);
        }

        public void UpdatePOForm2(string oldPOID, string newPOID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Old_PO_ID", oldPOID));
            param.Add(new SqlParameter("@PO_ID", newPOID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form2_UpdateID", param);
        }

        internal void DeletePOForm2(string form2ID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_Form2_ID", form2ID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form2_Delete", param);
        }

        public void AddForm2FromPR(string poID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Form2_InsertByPR", param);
        }

        #region PT 03/10/2013

        public DataTable GetPOItemByInvItemID(string poID, string inv_itemId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Inv_ItemID", inv_itemId));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PO_Items_SelectItem", param);
        }

        public bool AppendPOItemReceiveQty(string poItemID, string append_qty)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@POItem_ID", poItemID));
                param.Add(new SqlParameter("@recieve_qty", append_qty));

                new DatabaseHelper().ExecuteNonQuery("sp_Inv_PO_Items_AppendReceiveQty", param);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Yui 02/10/2013
        public void POApprove(string poID, string approverID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@poID", poID));

            if (approverID.Trim().Length > 0)
                param.Add(new SqlParameter("@Approve_By", approverID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_HistoryPurchase_CaseApprove", param);
        }

        public void POReject(string poID, string approverID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@poID", poID));

            if (approverID.Trim().Length > 0)
                param.Add(new SqlParameter("@Approve_By", approverID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_HistoryPurchase_CaseReject", param);
        }
        #endregion
    }
}