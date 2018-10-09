using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using GPlus.PRPO.PRPOHelper;

namespace GPlus.DataAccess
{
    public class POUploadDAO : DatabaseAccess
    {
        #region INSERT

        public void AddPOUpDown(string poID, string uploadBy, string uploadPath, string uploadFile)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Upload_By", uploadBy));
            param.Add(new SqlParameter("@Upload_File", uploadFile));
            param.Add(new SqlParameter("@Upload_Path", uploadPath));

            ExecuteNonQuery("sp_Inv_PO_UpDown_Insert", param);
        }

        #endregion


        #region DELETE

        public void DeletePOUpDown(int poupload_id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_Upload_ID", poupload_id));

            ExecuteNonQuery("sp_Inv_PO_UpDown_Delete", param);
        }
   
        #endregion


    }
}