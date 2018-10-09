using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class ProvinceDAO
    {

        public DataTable GetProvince()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_Province_Select");
        }

        public DataTable GetAmphur(string provinceID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceID", provinceID));

            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_Amphur_Select", param);
        }

        public DataTable GetTumbon(string provinceID, string amphurID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceID", provinceID));
            param.Add(new SqlParameter("@AmphurID", amphurID));

            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_Tumbon_Select", param);
        }
        
        #region PED

        public DataSet GetProvince(string ProvinceCode, string ProvinceName, string ProvinceStatus, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceCode", ProvinceCode));
            param.Add(new SqlParameter("@ProvinceName", ProvinceName));
            param.Add(new SqlParameter("@ProvinceStatus", ProvinceStatus));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));


            return new DatabaseHelper().ExecuteDataSet("sp_GPluz_Province_Search", param);
        }
        
        //=============================================================================================== 
        public DataTable GetProvince(string provinceID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceID", provinceID));

            return new DatabaseHelper().ExecuteDataTable("sp_GPluz_Province_SelectByID ", param);
        }
        //=============================================================================================== 
        public string AddProvince(string ProvinceCode, string ProvinceName, string Status, string CreateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceCode", ProvinceCode));
            param.Add(new SqlParameter("@ProvinceName", ProvinceName));
            param.Add(new SqlParameter("@ProvinceStatus", Status));
            param.Add(new SqlParameter("@CreateBy", CreateBy));

            return new DatabaseHelper().ExecuteScalar("sp_GPluz_Province_Insert", param).ToString();
        }

        public string UpdateProvince(string ProvinceID, string ProvinceCode, string ProvinceName, string Status, string UpdateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@ProvinceCode", ProvinceCode));
            param.Add(new SqlParameter("@ProvinceName", ProvinceName));
            param.Add(new SqlParameter("@ProvinceStatus", Status));
            param.Add(new SqlParameter("@UpdateBy", UpdateBy));

            return new DatabaseHelper().ExecuteScalar("sp_GPluz_Province_Update", param).ToString();
        }
        #endregion
    }
}