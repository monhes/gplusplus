using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.UserControls;
using GPlus.DataAccess;
using System.IO;

namespace GPlus.PRPO.PRPOHelper
{
    public class POFileUpload : Pagebase
    {
       
        public void Update(DataTable dtFile, DataTable dtFileDeleted)
        {
            POUploadDAO poUploadDAO = new POUploadDAO();

            try
            {
                poUploadDAO.BeginTransaction();

                // ลบรายการ inv_po_updown

                if (dtFileDeleted != null)
                {
                    if (dtFileDeleted.Rows.Count > 0)
                    {
                        foreach (DataRow r in dtFileDeleted.Rows)
                        {
                            int poupload_id = Convert.ToInt32(r["PoUploadID"].ToString() == "" ? "0" : r["PoUploadID"].ToString());
                            string filename = r["Upload_FileName"].ToString();

                            poUploadDAO.DeletePOUpDown(poupload_id);
                            // ลบไฟล์ใน ~/Uploads/POUpDown
                            string filePath = Path.Combine(Server.MapPath("~/Uploads/POUpDown"), filename);
                            if (File.Exists(filePath))
                            {
                                try { File.Delete(filePath); }
                                catch (Exception) { }
                            }
                        }
                    }
                }
                // --------------------------------- เพิ่มข้อมูล-----------------------------

                // เพิ่มรายการ inv_po_updown
                if (dtFile.Rows.Count > 0 && dtFile != null)
                {
                    foreach (DataRow row in dtFile.Rows)
                    {
                        if (Convert.ToInt32(row["PO_Upload_ID"]) < 0)
                        {
                            string filePath = Server.MapPath(Path.Combine("~/Uploads/POUpDown/tmp", this.UserID, row["Upload_File"].ToString()));
                            if (File.Exists(filePath))
                            {
                                File.Move(filePath, Server.MapPath(Path.Combine("~/Uploads/POUpDown", row["Upload_File"].ToString())));
                                poUploadDAO.AddPOUpDown(row["PO_ID"].ToString(), this.UserName, Server.MapPath("~/Uploads/POUpDown/"), row["Upload_File"].ToString());
                            }
                        }
                    }
                }

                poUploadDAO.CommitTransaction();
            }
            catch (Exception ex)   
            {
                poUploadDAO.RollbackTransaction();
                throw ex;
            }
        }
    }
}