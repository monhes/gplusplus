using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class AccountExpenseDAO
    {
        public DataSet GetAccountExpense(string accountExpenseCode, string accountExpenseName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AccExpense_Code", accountExpenseCode));
            param.Add(new SqlParameter("@AccExpense_Desc", accountExpenseName));
            param.Add(new SqlParameter("@AccExpense_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_AccountExpense_SelectPaging", param);
        }

        public DataTable GetAccountExpense(string accountExpenseID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AccExpense_ID", accountExpenseID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_AccountExpense_SelectByID", param);
        }

        public string AddAccountExpense(string accountExpenseCode, string accountExpenseName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AccExpense_Code", accountExpenseCode));
            param.Add(new SqlParameter("@AccExpense_Name", accountExpenseName));
            param.Add(new SqlParameter("@AccExpense_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_AccountExpense_Insert", param).ToString();
        }

        public string UpdateAccountExpense(string accountExpenseID,string accountExpenseCode, string accountExpenseName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AccExpense_ID", accountExpenseID));
            param.Add(new SqlParameter("@AccExpense_Code", accountExpenseCode));
            param.Add(new SqlParameter("@AccExpense_Name", accountExpenseName));
            param.Add(new SqlParameter("@AccExpense_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_AccountExpense_Update", param).ToString();
        }

    }
}