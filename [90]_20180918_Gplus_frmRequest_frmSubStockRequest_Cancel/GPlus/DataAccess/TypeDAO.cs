using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class TypeDAO
    {
        public DataSet GetType(string typeCode, string typeName,string status, int pageNum, int pageSize,
         string sortField, string sortOrder, string CateID = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Type_Code", typeCode));
            param.Add(new SqlParameter("@Type_Name", typeName));
            param.Add(new SqlParameter("@Type_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));
            if (CateID.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Cate_ID", Convert.ToInt32(CateID == "" ?"0":CateID)));
            }

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Type_SelectPaging", param);
        }

        public DataTable GetType(string typeID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Type_ID", typeID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Type_SelectByID", param);
        }

        public DataTable GetTypeByCateID(string cateID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_ID", Convert.ToInt32(cateID == "" ? "0":cateID)));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Type_SelectByCateID", param);
        }

        public string AddType(string typeCode, string typeName, string cateID, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Type_Code", typeCode));
            param.Add(new SqlParameter("@Type_Name", typeName));
            param.Add(new SqlParameter("@Cate_ID", Convert.ToInt32(cateID == "" ? "0":cateID)));
            param.Add(new SqlParameter("@Type_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Type_Insert", param).ToString();
        }

        public string UpdateType(string typeID, string typeCode, string typeName, string cateID, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Type_ID", typeID));
            param.Add(new SqlParameter("@Type_Code", typeCode));
            param.Add(new SqlParameter("@Type_Name", typeName));
            param.Add(new SqlParameter("@Cate_ID", Convert.ToInt32(cateID == "" ? "0" : cateID)));
            param.Add(new SqlParameter("@Type_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Type_Update", param).ToString();
        }
    }
}