using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class FileDAO
    {
        public DataTable GetFile(string referenceID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", referenceID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Picture_Select", param);
        }

        public string AddFile(string referenceID,  string fileSequence,  string fileURL)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", referenceID));
            param.Add(new SqlParameter("@pic_seq", fileSequence));
            param.Add(new SqlParameter("@Pic_path", fileURL));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Picture_Insert", param).ToString();
        }

        public void UpdateFile(string fileID, string sequence)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemPicID", fileID));
            param.Add(new SqlParameter("@pic_seq", sequence));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Picture_Update", param);
        }

        public void UpdateFileReference(string referenceID, string newReferenceID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", referenceID));
            param.Add(new SqlParameter("@NewInv_ItemID", newReferenceID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Picture_UpdateReference", param);
        }


        public void DeleteFile(string fileID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemPicID", fileID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Picture_Delete", param);
        }

    }
}