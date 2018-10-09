using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class AmphurDAO
    {
        public DataSet GetAmphur(string ProvinceID, string AmphurName, int pageNum, int pageSize,
          string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@AmphurName", AmphurName));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));


            return new DatabaseHelper().ExecuteDataSet("sp_GPluz_Amphur_Search", param);
        }

        public DataTable GetAmphurr()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_GPluz_Province_Select");
        }

        public DataTable bindAmphur()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_GPluz_Amphurr_Select");
        }

        public DataTable GetAmphur(string AmphurID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AmphurID", AmphurID));

            return new DatabaseHelper().ExecuteDataTable("sp_GPluz_Amphur_SelectByID ", param);
        }

        public string AddAmphur(string AmphurName, string AmphurCode, string ProvinceID , string Status, string CreateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AmphurName", AmphurName));
            param.Add(new SqlParameter("@AmphurCode", AmphurCode));
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@AmphurStatus", Status));
            param.Add(new SqlParameter("@CreateBy", CreateBy));

            return new DatabaseHelper().ExecuteScalar("sp_GPluz_Amphur_Insert", param).ToString();
        }

        public string UpdateAmphur(string AmphurID, string AmphurName, string AmphurCode,string ProvinceID, string Status, string UpdateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AmphurID", AmphurID));
            param.Add(new SqlParameter("@AmphurName", AmphurName));
            param.Add(new SqlParameter("@AmphurCode", AmphurCode));
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@AmphurStatus", Status));
            param.Add(new SqlParameter("@UpdateBy", UpdateBy));

            return new DatabaseHelper().ExecuteScalar("sp_GPluz_Amphur_Update", param).ToString();
        }

        public DataSet GetTumbon(string TumbonName ,string ProvinceID, string AmphurID, int pageNum, int pageSize,
         string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TumbonName", TumbonName));
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@AmphurID", AmphurID));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));


            return new DatabaseHelper().ExecuteDataSet("sp_GPluz_Tumbon_Search", param);
        }

        public DataTable GetTumbonID(string TumbonID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TumbonID", TumbonID));

            return new DatabaseHelper().ExecuteDataTable("sp_GPluz_Tumbon_SelectByID ", param);
        }

        public string AddTumbon(string TumbonName, string TumbonCode, string ProvinceID , string AmphurID , string Subdst ,string postcode , string Status, string CreateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TumbonName", TumbonName));
            param.Add(new SqlParameter("@TumbonCode", TumbonCode));
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@AmphurID", AmphurID));
            param.Add(new SqlParameter("@Subdst", Subdst));
            param.Add(new SqlParameter("@postcode", postcode));
            param.Add(new SqlParameter("@TumbonStatus", Status));
            param.Add(new SqlParameter("@CreateBy", CreateBy));

            return new DatabaseHelper().ExecuteScalar("sp_GPluz_Tumbon_Insert", param).ToString();
        }

        public string UpdateTumbon(string TumbonID,string TumbonCode,string TumbonName, string ProvinceID, string AmphurID, string Subdst, string postcode, string Status, string UpdateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@TumbonID", TumbonID));
            param.Add(new SqlParameter("@TumbonCode", TumbonCode));
            param.Add(new SqlParameter("@TumbonName", TumbonName));
            param.Add(new SqlParameter("@ProvinceID", ProvinceID));
            param.Add(new SqlParameter("@AmphurID", AmphurID));
            param.Add(new SqlParameter("@Subdst", Subdst));
            param.Add(new SqlParameter("@postcode", postcode));
            param.Add(new SqlParameter("@TumbonStatus", Status));
            param.Add(new SqlParameter("@UpdateBy", UpdateBy));

            return new DatabaseHelper().ExecuteScalar("sp_GPluz_Tumbon_Update", param).ToString();
        }

        public DataTable GetAmphur1(string provinceID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ProvinceID", provinceID));

            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_Amphur_Select",param);
        }



    }
}