using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class ExpenseDAO
    {
        public DataSet GetExpense(string expenseCode, string expenseName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Expense_Code", expenseCode));
            param.Add(new SqlParameter("@Expense_Desc", expenseName));
            param.Add(new SqlParameter("@Expense_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Expense_SelectPaging", param);
        }

        public DataTable GetExpense(string expenseID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Expense_ID", expenseID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Expense_SelectByID", param);
        }

        public string AddExpense(string expenseCode, string expenseName, string comFlag, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Expense_Code", expenseCode));
            param.Add(new SqlParameter("@Expense_Desc", expenseName));
            param.Add(new SqlParameter("@Expense_Com_Flag", comFlag));
            param.Add(new SqlParameter("@Expense_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Expense_Insert", param).ToString();
        }

        public string UpdateExpense(string expenseID, string expenseCode, string expenseName, string comFlag, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Expense_ID", expenseID));
            param.Add(new SqlParameter("@Expense_Code", expenseCode));
            param.Add(new SqlParameter("@Expense_Desc", expenseName));
            param.Add(new SqlParameter("@Expense_Com_Flag", comFlag));
            param.Add(new SqlParameter("@Expense_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Expense_Update", param).ToString();
        }
    }
}