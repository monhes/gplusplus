using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class ProjectDAO
    {
        public DataSet GetProject(string projectCode, string projectName, string status, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Project_Code", projectCode));
            param.Add(new SqlParameter("@Project_Name", projectName));
            param.Add(new SqlParameter("@Project_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Project_SelectPaging", param);
        }

        public DataTable GetProject(string projectID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Project_ID", projectID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Project_SelectByID", param);
        }

        public string AddProject(string projectCode, string projectName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Project_Code", projectCode));
            param.Add(new SqlParameter("@Project_Name", projectName));
            param.Add(new SqlParameter("@Project_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Project_Insert", param).ToString();
        }

        public string UpdateProject(string projectID, string projectCode, string projectName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Project_ID", projectID));
            param.Add(new SqlParameter("@Project_Code", projectCode));
            param.Add(new SqlParameter("@Project_Name", projectName));
            param.Add(new SqlParameter("@Project_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Project_Update", param).ToString();
        }
    }
}