using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GPlus.DataAccess
{
    public class AccountDAO
    {
        public DataSet GetAccount(string accountNumber, string accountName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_Number", accountNumber));
            param.Add(new SqlParameter("@Account_Name", accountName));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Master_Account_SelectPaging", param);
        }

        public DataTable GetAccount(string accountID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_ID", accountID));

            return new DatabaseHelper().ExecuteDataTable("sp_Master_Account_SelectByID", param);
        }

        public string AddAccount(string accountNumber, string accountName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_Number", accountNumber));
            param.Add(new SqlParameter("@Account_Name", accountName));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Master_Account_Insert", param).ToString();
        }

        public void UpdateAccount(string accountID, string accountNumber, string accountName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_ID", accountID));
            param.Add(new SqlParameter("@Account_Number", accountNumber));
            param.Add(new SqlParameter("@Account_Name", accountName));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@Create_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Master_Account_Update", param);
        }

        #region Mew 03/04/2014

        public DataSet GetAccountStcc(string StockID, string Fname, string Lname, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            
            param.Add(new SqlParameter("@Fname", Fname));
            param.Add(new SqlParameter("@Lname", Lname));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));
            if (StockID.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Stock_ID", Convert.ToInt32(StockID == "" ? "0" : StockID)));
            }

            return new DatabaseHelper().ExecuteDataSet("sp_Get_Inv_Stock_Gplus_Account", param);
        }

        public DataTable GetAccName()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Get_Gplus_Account"); 
        }

        public DataTable GetAccountID(string accountID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_Account_ID", accountID));

            return new DatabaseHelper().ExecuteDataTable("sp_Get_Stock_Gplus_Account1", param);
        }

        public string AddStockAcc(string accountId, string stockId, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Account_ID", accountId));
            param.Add(new SqlParameter("@Stock_ID", stockId));
            param.Add(new SqlParameter("@stock_account_status", status));
            param.Add(new SqlParameter("@create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Gplus_Account_Insert", param).ToString();
        }

        public string UpdateStockAcc(string stockAcc,string accountId, string stockId, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Stock_Account_ID", stockAcc));
            param.Add(new SqlParameter("@Account_ID", accountId));
            param.Add(new SqlParameter("@Stock_ID", stockId));
            param.Add(new SqlParameter("@stock_account_status", status));
            param.Add(new SqlParameter("@Update_by", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Gplus_Account_Update", param).ToString();
        }



        #endregion
    }
}