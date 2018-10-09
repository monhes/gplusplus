using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class PackageDAO
    {
        public DataSet GetPackage(string packageName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Package_Name", packageName));
            param.Add(new SqlParameter("@Pack_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Package_SelectPaging", param);
        }

        public DataTable GetPackage(string packageID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Pack_ID", packageID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Package_SelectByID", param);
        }

        public string AddPackage(string packageName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Package_Name", packageName));
            param.Add(new SqlParameter("@Pack_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Package_Insert", param).ToString();
        }

        public string UpdatePackage(string packageID, string packageName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Pack_ID", packageID));
            param.Add(new SqlParameter("@Package_Name", packageName));
            param.Add(new SqlParameter("@Pack_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Package_Update", param).ToString();
        }

    }
}