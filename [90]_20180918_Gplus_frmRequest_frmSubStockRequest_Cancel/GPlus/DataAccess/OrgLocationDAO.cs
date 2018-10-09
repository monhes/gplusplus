using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace GPlus.DataAccess
{
    public class OrgLocationDAO
    {
        public OrgLocationDAO()
        {
        }

        public void InsertOrgLocation(int orgStructId, int building, int floor)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@OrgStrucId", orgStructId));
            param.Add(new SqlParameter("@Building", building));
            param.Add(new SqlParameter("@Floor", floor));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_OrgLocation_Insert", param);
        }

        public DataTable GetBuildingFloorByOrgID(int orgStrucId)
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@OrgStrucId", orgStrucId));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_OrgLocation_Select", param);
        }
    }
}