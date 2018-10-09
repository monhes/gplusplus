using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class SupplierDAO
    {
        public DataSet GetSupplier(string supplierCode, string supplierName, string status, int pageNum, int pageSize,
          string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_Code", supplierCode));
            param.Add(new SqlParameter("@Supplier_Name", supplierName));
            param.Add(new SqlParameter("@Supplier_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Supplier_SelectPaging", param);
        }

        public DataTable GetSupplier(string supplierID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_ID", supplierID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Supplier_SelectByID", param);
        }

        public string AddSupplier(string supplierCode, string supplierName, string supplierAddress, string provinceCode, string amphurCode, string tumbonCode,
            string postalCode, string telephone, string email, string mailPOFlag, string faxNo, string billAddressSubID, string paymentType,
            string includeVatFlag, string creditTerm, string status, string createBy, string supplier_type)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_Code", supplierCode));
            param.Add(new SqlParameter("@Supplier_Name", supplierName));
            param.Add(new SqlParameter("@Supplier_Address", supplierAddress));
            if(provinceCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Province_Code", provinceCode));
            if(amphurCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Amphur_Code",amphurCode));
            if(tumbonCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Tumbol_Code", tumbonCode));
            param.Add(new SqlParameter("@Postal_Code",postalCode));
            param.Add(new SqlParameter("@Telephone_Number", telephone));
            param.Add(new SqlParameter("@email", email));
            param.Add(new SqlParameter("@MailPo_Flag", mailPOFlag));
            param.Add(new SqlParameter("@Fax_Number", faxNo));
            if(billAddressSubID.Trim().Length > 0)
                param.Add(new SqlParameter("@Billing_Address_Sup_Id", billAddressSubID));
            param.Add(new SqlParameter("@Payment_Type", paymentType));
            param.Add(new SqlParameter("@IncludeVat_Flag", includeVatFlag));
            if(creditTerm.Trim().Length > 0)
                param.Add(new SqlParameter("@Credit_Term", creditTerm));
            param.Add(new SqlParameter("@Supplier_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));
            param.Add(new SqlParameter("@Supplier_Type", supplier_type));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Supplier_Insert", param).ToString();
        }

        public string UpdateSupplier(string supplierID, string supplierCode, string supplierName, string supplierAddress, string provinceCode, string amphurCode, 
            string tumbonCode, string postalCode, string telephone, string email, string mailPOFlag, string faxNo, string billAddressSubID, string paymentType,
            string includeVatFlag, string creditTerm, string status, string updateBy, string supplier_type)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@Supplier_Code", supplierCode));
            param.Add(new SqlParameter("@Supplier_Name", supplierName));
            param.Add(new SqlParameter("@Supplier_Address", supplierAddress));
            if (provinceCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Province_Code", provinceCode));
            if (amphurCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Amphur_Code", amphurCode));
            if (tumbonCode.Trim().Length > 0)
                param.Add(new SqlParameter("@Tumbol_Code", tumbonCode));
            param.Add(new SqlParameter("@Postal_Code", postalCode));
            param.Add(new SqlParameter("@Telephone_Number", telephone));
            param.Add(new SqlParameter("@email", email));
            param.Add(new SqlParameter("@MailPo_Flag", mailPOFlag));
            param.Add(new SqlParameter("@Fax_Number", faxNo));
            if (billAddressSubID.Trim().Length > 0)
                param.Add(new SqlParameter("@Billing_Address_Sup_Id", billAddressSubID));
            param.Add(new SqlParameter("@Payment_Type", paymentType));
            param.Add(new SqlParameter("@IncludeVat_Flag", includeVatFlag));
            if (creditTerm.Trim().Length > 0)
                param.Add(new SqlParameter("@Credit_Term", creditTerm));
            param.Add(new SqlParameter("@Supplier_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));
            param.Add(new SqlParameter("@Supplier_Type", supplier_type));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Supplier_Update", param).ToString();
        }



        public DataTable GetSupplierAccount(string supplierID, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_ID", supplierID));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Supplier_Account_Select", param);
        }

        public DataTable GetSupplierAccount(string supplierAccountID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_Account_ID", supplierAccountID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Supplier_Account_SelectByID", param);
        }

        public string AddSupplierAccount(string supplierID, string fullName, string accountUserName, string accountPassword, DateTime expireDate, string status,
            string createBy, string email)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_Id", supplierID));
            param.Add(new SqlParameter("@Account_FullName", fullName));
            param.Add(new SqlParameter("@Account_Username", accountUserName));
            param.Add(new SqlParameter("@Account_Password", accountPassword));
            if(expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));
            param.Add(new SqlParameter("@Email", email));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Supplier_Account_Insert", param).ToString();
        }

         public string UpdateSupplierAccount(string supplierAccountID,string supplierID, string fullName, string accountUserName, string accountPassword,
             DateTime expireDate, string status, string updateBy, string email)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Supplier_Account_ID", supplierAccountID));
            param.Add(new SqlParameter("@Supplier_Id", supplierID));
            param.Add(new SqlParameter("@Account_FullName", fullName));
            param.Add(new SqlParameter("@Account_Username", accountUserName));
            param.Add(new SqlParameter("@Account_Password", accountPassword));
            if(expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));
            param.Add(new SqlParameter("@Email", email));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Supplier_Account_Update", param).ToString();
        }

         public void DeleteSupplierAccount(string supplierAccountID)
         {
             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@Supplier_Account_ID", supplierAccountID));

             new DatabaseHelper().ExecuteNonQuery("sp_Inv_Supplier_Account_Delete", param);
         }

#region Nin 19072013

         public DataSet GetRemainingPOReport(string CreatePOStartDate, string CreatePOEndDate, string SupplierCode, string SupplierName, string ItemCode, string ItemName)
         {
             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@PO_StartDate", CreatePOStartDate));
             param.Add(new SqlParameter("@PO_EndDate", CreatePOEndDate));
             param.Add(new SqlParameter("@Supplier_Code", SupplierCode));
             param.Add(new SqlParameter("@Supplier_Name", SupplierName));
             param.Add(new SqlParameter("@ItemCode", ItemCode));
             param.Add(new SqlParameter("@ItemName", ItemName));

             //return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectReport_test", param);
             return new DatabaseHelper().ExecuteDataSet("sp_Inv_Remaining_PO_Report", param);
         }

         public DataSet GetOrderPOReport(string CreatePOStartDate, string CreatePOEndDate, string categoryID,string Supplier_Name, string ItemCode, string ItemName)
         {
             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@PO_StartDate", CreatePOStartDate));
             param.Add(new SqlParameter("@PO_EndDate", CreatePOEndDate));
             param.Add(new SqlParameter("@Cate_ID", categoryID));
             param.Add(new SqlParameter("@Supplier_Name", Supplier_Name));
             param.Add(new SqlParameter("@ItemCode", ItemCode));
             param.Add(new SqlParameter("@ItemName", ItemName));

             return new DatabaseHelper().ExecuteDataSet("sp_Inv_Order_PO_Report", param);
         }

         public String AddSupplierMTL_Balance(string stockID, string Tran_Month, string Tran_Year, string balance, string status, string User_Id)
         {
             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@Stock_Id", stockID));
             param.Add(new SqlParameter("@Transaction_Month", Tran_Month));
             param.Add(new SqlParameter("@Transaction_Year", Tran_Year));
             param.Add(new SqlParameter("@Balance_Amount", balance));
             param.Add(new SqlParameter("@Asset_Status", status));
             param.Add(new SqlParameter("@User_Id", User_Id));

             return new DatabaseHelper().ExecuteScalar("sp_Inv_SupplierMTL_Balance_Insert", param).ToString();
         }

         public string SupplierMTL_BalanceChangeStatus(string stockID, string Tran_Month, string Tran_Year, string status, string User_Id)
         {
             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@Stock_Id", stockID));
             param.Add(new SqlParameter("@Transaction_Month", Tran_Month));
             param.Add(new SqlParameter("@Transaction_Year", Tran_Year));
             param.Add(new SqlParameter("@Asset_Status", status));
             param.Add(new SqlParameter("@User_Id", User_Id));

             return new DatabaseHelper().ExecuteScalar("sp_Inv_SupplierMTL_Balance_ChangeStatus", param).ToString();
         }

         public String UpdateSupplierMTL_Balance(string stockID, string Tran_Month, string Tran_Year, string balance, string status, string User_Id)
         {
             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@Stock_Id", stockID));
             param.Add(new SqlParameter("@Transaction_Month", Tran_Month));
             param.Add(new SqlParameter("@Transaction_Year", Tran_Year));
             param.Add(new SqlParameter("@Balance_Amount", balance));
             param.Add(new SqlParameter("@Asset_Status", status));
             param.Add(new SqlParameter("@User_Id", User_Id));

             return new DatabaseHelper().ExecuteScalar("sp_Inv_SupplierMTL_Balance_Update", param).ToString();
         }

         public DataSet GetSupplierMTL_Balance(string stockID, string src_trans_startDate, string src_trans_endDate, string status, int pageNum, int pageSize,
                   string sortField, string sortOrder)
         {

             List<SqlParameter> param = new List<SqlParameter>();
             param.Add(new SqlParameter("@Stock_Id", stockID));
             param.Add(new SqlParameter("@Trans_StartDate", src_trans_startDate));
             param.Add(new SqlParameter("@Trans_EndDate", src_trans_endDate));
             param.Add(new SqlParameter("@Asset_Status", status));
             param.Add(new SqlParameter("@PageNum", pageNum));
             param.Add(new SqlParameter("@PageSize", pageSize));
             param.Add(new SqlParameter("@SortField", sortField));
             param.Add(new SqlParameter("@SortOrder", sortOrder));

             return new DatabaseHelper().ExecuteDataSet("sp_Inv_SupplierMTL_Balance_SelectPaging", param);
         }

#endregion


         public object Supplier_Name { get; set; }
    }
}