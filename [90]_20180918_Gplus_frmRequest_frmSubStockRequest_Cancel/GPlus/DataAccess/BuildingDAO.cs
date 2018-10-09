using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class BuildingDAO
    {
        public DataTable GetBuildingByCodeAndName(string buildingId, string floorDescription)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@BuildingId", buildingId));
            param.Add(new SqlParameter("@FloorDescription", floorDescription));


            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Floor_Select2", param);
        }


        public DataTable GetBuildingByFloorDesc(string Building_Id, string Building_Floor_Desc, string status, string Created_By)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_Id", Building_Id));
            param.Add(new SqlParameter("@Building_Floor_Desc", Building_Floor_Desc));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Created_By", Created_By));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Floor_Insert", param);
        }


        public DataTable GetBuildingByCodeAndName(string BuildingId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_Id", BuildingId));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Floor_Select3", param);
        }

        public DataTable UpdateBuildingFloor(string Building_Id, string Building_FloorId, string Building_Floor_Desc, string Status, string Updated_By)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_ID", Building_Id));
            param.Add(new SqlParameter("@Building_FloorId", Building_FloorId));
            param.Add(new SqlParameter("@Building_Floor_Desc", Building_Floor_Desc));
            param.Add(new SqlParameter("@Status", Status));
            param.Add(new SqlParameter("@Updated_By", Updated_By));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Floor_Update", param);
        }
        public DataSet GetBuilding(string BuildingCode, string BuildingName, string Buildingstatus, int pageNum, int pageSize,
          string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@BuildingCode", BuildingCode));
            param.Add(new SqlParameter("@BuildingName", BuildingName));
            param.Add(new SqlParameter("@BuildingStatus", Buildingstatus));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Building_Select2", param);
        }

        public DataTable GetBuilding(string BuildingID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_ID", BuildingID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Select2ByID", param);
        }

        public string AddBuilding(string BuildingCode, string BuildingName, string Status, string CreateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_Code", BuildingCode));
            param.Add(new SqlParameter("@Building_Name", BuildingName));
            param.Add(new SqlParameter("@Building_Status", Status));
            param.Add(new SqlParameter("@Create_By", CreateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Building_Select2_Insert", param).ToString();
        }
        public string UpdateBuilding(string BuildingID, string BuildingCode, string BuildingName, string Status, string UpdateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_ID", BuildingID));
            param.Add(new SqlParameter("@Building_Code", BuildingCode));
            param.Add(new SqlParameter("@Building_Name", BuildingName));
            param.Add(new SqlParameter("@Building_Status", Status));
            param.Add(new SqlParameter("@Update_By", UpdateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Building_Select2_Update", param).ToString();
        }

    }
}