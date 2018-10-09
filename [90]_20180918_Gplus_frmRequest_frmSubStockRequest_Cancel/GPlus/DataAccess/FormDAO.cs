using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class FormDAO
    {
        public DataSet GetForm(string formCode, string formName, string status, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Form_Code", formCode));
            param.Add(new SqlParameter("@Form_Name", formName));
            param.Add(new SqlParameter("@Form_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Form_SelectPaging", param);
        }

        public DataTable GetForm(string formID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Form_ID", formID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Form_SelectByID", param);
        }

        public string AddForm(string formCode, string formName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Form_Code", formCode));
            param.Add(new SqlParameter("@Form_Name", formName));
            param.Add(new SqlParameter("@Form_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Form_Insert", param).ToString();
        }

        public string UpdateForm(string formID, string formCode, string formName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Form_ID", formID));
            param.Add(new SqlParameter("@Form_Code", formCode));
            param.Add(new SqlParameter("@Form_Name", formName));
            param.Add(new SqlParameter("@Form_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Form_Update", param).ToString();
        }
    }
}