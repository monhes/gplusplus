using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class CategoryDAO
    {
        public DataSet GetCategory(string categoryCode, string categoryName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_Code", categoryCode));
            param.Add(new SqlParameter("@Cat_Name", categoryName));
            param.Add(new SqlParameter("@Cat_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Category_SelectPaging", param);
        }

        public DataTable GetCategory(string categoryID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_ID", categoryID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Category_SelectByID", param);
        }

        public string AddCategory(string categoryCode, string categoryName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_Code", categoryCode));
            param.Add(new SqlParameter("@Cat_Name", categoryName));
            param.Add(new SqlParameter("@Cat_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Category_Insert", param).ToString();
        }

        public string UpdateCategory(string categoryID,string categoryCode, string categoryName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_ID", categoryID));
            param.Add(new SqlParameter("@Cate_Code", categoryCode));
            param.Add(new SqlParameter("@Cat_Name", categoryName));
            param.Add(new SqlParameter("@Cat_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Category_Update", param).ToString();
        }



        public DataSet GetSubCate(string subCateCode, string categoryID, string subCateName, string status, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@SubCate_Code", subCateCode));
            param.Add(new SqlParameter("@Cate_ID", categoryID));
            param.Add(new SqlParameter("@SubCate_Name", subCateName));
            param.Add(new SqlParameter("@SubCate_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_SubCate_SelectPaging", param);
        }

        public DataTable GetSubCate(string subCateID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@SubCate_ID", subCateID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_SubCate_SelectByID", param);
        }

        public string AddSubCate(string subCateCode, string categoryID, string subCateName, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@SubCate_Code", subCateCode));
            param.Add(new SqlParameter("@Cate_ID", categoryID));
            param.Add(new SqlParameter("@SubCate_Name", subCateName));
            param.Add(new SqlParameter("@SubCate_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_SubCate_Insert", param).ToString();
        }

        public string UpdateSubCate(string subCateID,string subCateCode, string categoryID, string subCateName, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@SubCate_ID", subCateID));
            param.Add(new SqlParameter("@SubCate_Code", subCateCode));
            param.Add(new SqlParameter("@Cate_ID", categoryID));
            param.Add(new SqlParameter("@SubCate_Name", subCateName));
            param.Add(new SqlParameter("@SubCate_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_SubCate_Update", param).ToString();
        }

        #region Ed 31/07/2013
        public DataTable GetCategoryAll()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Category_Select");
        }
        #endregion


    }
}