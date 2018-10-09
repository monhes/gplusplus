using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

using GPlus.DataAccess;
using System.Diagnostics;
using System.IO;
using GPlus.PRPO.PRPOHelper;
using System.Configuration;

namespace GPlus.PRPO
{
    public partial class UploadPO : Pagebase
    {
        string po_id = "";
        string Newfilename = "";

        public DataTable dtFile
        {
            get { return (ViewState["dtFile"] == null) ? null : (DataTable)ViewState["dtFile"]; }
            set { ViewState["dtFile"] = value; }
        }

        public DataTable dtFileDeleted
        {
            get { return (ViewState["dtFileDeleted"] == null) ? null : (DataTable)ViewState["dtFileDeleted"]; }
            set { ViewState["dtFileDeleted"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["dtFile"] = null;
                po_id = Request.QueryString["id"];
                GetPOCode();
                BindDataDB(); // ดึงค่าข้อมูล File ที่เคย Upload แล้วบน Server
                BindGridview();

            }

        }
        private void GetPOCode()
        {
            string po_code = "";

            DataTable dt = new DataAccess.PODAO().GetPOForm1(po_id);

            if(dt.Rows.Count > 0)
            {
                po_code = dt.Rows[0]["PO_Code"].ToString();
            }

            txtPoCode.Text = po_code;
        }

        private DataTable BindDataDB()
        {
            dtFile = new DataAccess.PODAO().GetPOUpDown(Request.QueryString["id"]);
            dtFile.Columns.Add("FileType", typeof(string));
            dtFile.Columns.Add("Upload_FileCut", typeof(string));
            if (dtFile.Rows.Count > 0)
            {
                for (int i = 0; i < dtFile.Rows.Count; i++)
                {
                    string filename = dtFile.Rows[i]["Upload_File"].ToString();
                    string file_cut = filename.Substring(0, filename.LastIndexOf("_"));
                    dtFile.Rows[i]["Upload_FileCut"] = file_cut;
                    dtFile.Rows[i]["FileType"] = 'O'; // FileType 'O' = มีอยู่แล้ว ,'N' = เพิ่มมาใหม่,'D' = ลบ  
                }
            }

            return dtFile;
        }

        private void BindGridview()
        {
            if (dtFile.Rows.Count > 0)
            {
                gvUploadPO.DataSource = dtFile;
                gvUploadPO.DataBind();
                gvUploadPO.Visible = true;
                divBtn.Visible = true;
            }
            else
            {
                gvUploadPO.Visible = false;
                //divBtn.Visible = false;
            }
        }

        protected void gvUploadPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HiddenField hdFileName = (HiddenField)e.Row.FindControl("hdFileName");
                HiddenField hdFileType = (HiddenField)e.Row.FindControl("hdFileType");
                HiddenField hdPoUpLoadID = (HiddenField)e.Row.FindControl("hdPoUpLoadID");
                ImageButton btnDelete = (ImageButton)e.Row.FindControl("btnDelete");
                LinkButton btnFileName = (LinkButton)e.Row.FindControl("btnFileName");

                hdFileName.Value = drv["Upload_File"].ToString();
                hdFileType.Value = drv["FileType"].ToString();
                hdPoUpLoadID.Value = drv["PO_Upload_ID"].ToString();

                btnDelete.CommandArgument = hdPoUpLoadID.Value + "&" + hdFileName.Value + "&" + hdFileType.Value;
                btnFileName.CommandArgument = hdPoUpLoadID.Value + "&" + hdFileName.Value + "&" + hdFileType.Value;

                btnDelete.Attributes.Add("onclick", "return confirm('ต้องการลบหรือไม่');"); 
            }
        }

        protected void gvUploadPO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] str_cmd;
            str_cmd = e.CommandArgument.ToString().Split('&');

            if (e.CommandName == "View")
            {
                string uploadingFilePath = "";
                if (str_cmd[2] == "O")
                {
                    //uploadingFilePath = HttpUtility.JavaScriptStringEncode(Util.GetSiteRoot() + "Uploads/POUpDown/" + str_cmd[1], true);
                    uploadingFilePath = Util.GetSiteRoot() + "Uploads/POUpDown/" + HttpUtility.JavaScriptStringEncode(str_cmd[1]);
                }
                else if (str_cmd[2] == "N")
                {
                    uploadingFilePath = Util.GetSiteRoot() + "Uploads/POUpDown/tmp/"+this.UserID+"/"+ HttpUtility.JavaScriptStringEncode(str_cmd[1]);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),"js1","window.open('"+uploadingFilePath+"', '', 'width=800,height=500,resizable=yes,scrollbars=yes');",true);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "js1", "window.open('" + uploadingFilePath + "', '', 'width=800,height=500,resizable=yes,scrollbars=yes');", true);
            }
            else if (e.CommandName == "Delete")
            {
                int poUploadID = Convert.ToInt32(str_cmd[0] == "" ? "0" : str_cmd[0]);
                DeleteFile(poUploadID);
            }
        }

        protected void gvUploadPO_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow headerow = new GridViewRow(0, 0, DataControlRowType.Header,
                                                          DataControlRowState.Insert);
                e.Row.Cells.Clear();

                TableCell headercell1 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "ชื่อไฟล์",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell1);

                TableCell headercell2 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "ลบ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell2);

                TableCell headercell7 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "วันที่ Upload",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell7);

                GridViewRow headerow2 = new GridViewRow(0, 0, DataControlRowType.Header,
                                                               DataControlRowState.Insert);

                TableCell headercell3 = new TableCell()
                {
                    ColumnSpan = 3,
                    RowSpan = 1,
                    Height = 20,
                    Text = "Download",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell3);

                TableCell headercell4 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "จำนวนครั้ง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell4);

                TableCell headercell5 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "วันที่ล่าสุด",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell5);

                TableCell headercell6 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "ชื่อ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell6);

                gvUploadPO.Controls[0].Controls.AddAt(0, headerow);
                gvUploadPO.Controls[0].Controls.AddAt(1, headerow2);
            }
        }

        protected void gvUploadPO_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
        }

        protected void btnUploadAuto_Click(object sender, EventArgs e)
        {
            //ShowMessageBox("TestUploadAuto");
            CreateTmpFolderIfNotExist();
            CreateUserIdFolderIfNotExist();
            SaveFileToUserIdFolder();
            AddDataToDataTable();
        }

        private void CreateTmpFolderIfNotExist()
        {
            string tmpPath = Server.MapPath("~/Uploads/POUpDown/tmp");

            if (!Directory.Exists(tmpPath))
                Directory.CreateDirectory(tmpPath);
        }

        private void CreateUserIdFolderIfNotExist()
        {
            string userIdPath = GetUserIdUploadPath();

            if (!Directory.Exists(userIdPath))
                Directory.CreateDirectory(userIdPath);
        }

        private void SaveFileToUserIdFolder()
        {
            string fileName = Request.Files["fileUploader"].FileName;

            // prevent IE including local path
            fileName = Path.GetFileName(fileName);

            string fileNameWithDateTime = GetFileNameWithDateTimeFormat(fileName);
            Newfilename = fileNameWithDateTime;

            Request.Files["fileUploader"].SaveAs(Path.Combine(GetUserIdUploadPath(), fileNameWithDateTime));
            
        }

        private string GetUserIdUploadPath()
        {
            return Path.Combine(Server.MapPath("~/Uploads/POUpDown/tmp"), this.UserID);
        }

        private string GetFileNameWithDateTimeFormat(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string datetime = string.Format("{0:ddMMyyyyHHmmssffff}", DateTime.Now);

            return fileName.Substring(0, fileName.LastIndexOf('.')) + "_" + datetime + extension;
        }

        private void AddDataToDataTable()
        {
            int tmp_POUpload_ID = GetMinPOUpload_ID();
            
            DataRow drRow = dtFile.NewRow();

            drRow["PO_Upload_ID"] = tmp_POUpload_ID.ToString();
            drRow["PO_ID"] = Convert.ToInt32(Request.QueryString["id"] == ""?"0":Request.QueryString["id"]);
            drRow["Upload_By"] = DBNull.Value;
            drRow["Upload_Date"] = DBNull.Value;
            drRow["Upload_File"] = Newfilename;
            drRow["Upload_Path"] = DBNull.Value;
            drRow["Download_Count"] = DBNull.Value;
            drRow["Latest_Download_by"] = DBNull.Value;
            drRow["Latest_Download_Date"] = DBNull.Value;
            drRow["FileType"] = "N";
            drRow["Upload_FileCut"] = Newfilename.Substring(0, Newfilename.LastIndexOf("_"));

            dtFile.Rows.Add(drRow);
            BindGridview();
        }

        private int GetMinPOUpload_ID()
        {
            int minID = 0;
            int resultID = 0;

            if (dtFile.Rows.Count > 0)
            {
                minID = Convert.ToInt32(dtFile.Compute(" MIN(PO_Upload_ID)", string.Empty));
            }
            else
            {
                resultID = -1;
            }

            if (minID > 0 || minID == 0)
            {
                resultID = -1;
            }
            else
            {

                resultID = minID - 1;
            }

            return resultID;
        }

        private void DeleteFile(int upload_id)
        {
            if (upload_id < 0) //ไฟล์ที่เพิ่มเข้ามาใหม่อยู่ใน tmp
            {

                DataRow row = dtFile.Select(string.Format("PO_Upload_ID = {0}", upload_id)).FirstOrDefault();
                if (row != null)
                {
                    string uploadingFilePath = Path.Combine(GetUserIdUploadPath(), row["Upload_File"].ToString());

                    if (File.Exists(uploadingFilePath))
                    {
                        try { File.Delete(uploadingFilePath); }
                        catch (Exception) { }
                    }

                    dtFile.Rows.Remove(row);
                    dtFile.AcceptChanges();
                }
            }
            else //ไฟล์ที่อยู่ใน db ให้ลบออกจาก dt แต่ยังไม่ต้องลบไฟล์จริง
            {
                DataRow row = dtFile.Select(string.Format("PO_Upload_ID = {0}", upload_id)).FirstOrDefault();
                if (row != null)
                {
                    AddDeletedItem(upload_id, row["Upload_File"].ToString());
                    dtFile.Rows.Remove(row);
                    dtFile.AcceptChanges();
                }
            }
            BindGridview();
        }

        private void AddDeletedItem(int poupload_id,string filename)
        {
            if (dtFileDeleted == null)
            {
                dtFileDeleted = new DataTable();
                dtFileDeleted.Columns.Add("PoUploadID",typeof(int));
                dtFileDeleted.Columns.Add("Upload_FileName",typeof(string));
            }

            DataRow dr = dtFileDeleted.NewRow();
            dr["PoUploadID"] = poupload_id;
            dr["Upload_FileName"] = filename;

            dtFileDeleted.Rows.Add(dr);

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                POFileUpload pofileUpload = new POFileUpload();
                DataRow row = dtFile.Select(string.Format("PO_Upload_ID < 0")).FirstOrDefault();
                pofileUpload.Update(dtFile, dtFileDeleted);
                DeleteTmpUserFolder();
                BindDataDB();
                BindGridview();
               
                if (row != null) //ทำการตรวจสอบว่ามีการเพิ่ม PO_Upload ใหม่ หรือไม่ ถ้ามีการเพิ่มจึงจะส่ง Mail
                {
                    try
                    {
                        SendMail();
                    }
                    catch
                    {
                        ShowMessageBox("ส่ง E-mail ไม่สำเร็จ");
                    }
                }
                ShowMessageBox("บันทึกเรียบร้อย");
            }
            catch(Exception ex)
            {
                ShowMessageBox(ex.ToString());
            }
        }

        private void SendMail()
        {
            string mail_to = "";
            string mail_subject = "";
            string mail_message = "";
            string file_path = "";
            string supplier_name = "";

            //mail_to = GetSupplierMail();

            DataTable dt = new DataAccess.PODAO().GetPOForm1(Request.QueryString["id"]);

            if (dt.Rows.Count > 0)
            {
                mail_to = dt.Rows[0]["e-mail"].ToString();
                supplier_name = dt.Rows[0]["Supplier_Name"].ToString();
            }

            
            mail_subject = "บริษัทเมืองไทยประกันชีวิต : ใบสั่งซื้อ(PO)  เลขที่ " + txtPoCode.Text;

            /* ใช้กับ Mail Server LPA */
            //mail_message = "เรียน " + supplier_name + "\r\n\r\n\n สามารถ Download ใบสั่งซื้อได้ที่  URL : http://119.160.215.195/gplus_po_download/login.aspx";

            /* ใช้กับ Mail Server เมืองไทย */
            mail_message = "<html>" +
                            "<head>" +
                            "<title></title>" +
                            "</head>" +
                            "<body>" +
                            "เรียน " + supplier_name +
                            "<br/>" +
                            "<br/>" +
                            "    สามารถ Download ใบสั่งซื้อได้ที่  URL :   " +
                            "</body>" +
                            "</html>";
            
            //mail_to = "krongthong.t@loxbitpa.com";
            //mail_to = "ninna_nao_o@hotmail.com";

            if (mail_to != "")
            {
                /* ใช้กับ Mail Server เมืองไทย */
                EmailService.SendMail(ConfigurationManager.AppSettings["MailFrom"], mail_to.Trim(), mail_subject.Trim(), mail_message.Trim(), file_path);

                /* ใช้กับ Mail Server Gmail */
                //EmailService.Send_mail_LPA(ConfigurationManager.AppSettings["MailFrom"], mail_to.Trim(), mail_subject.Trim(), mail_message.Trim(), file_path);
            }
        }

        //private string GetSupplierMail()
        //{
        //    string email = "";

        //    DataTable dt = new DataAccess.PODAO().GetPOForm1(po_id);

        //    if (dt.Rows.Count > 0)
        //    {
        //        email = dt.Rows[0]["e-mail"].ToString();
        //    }

        //    return email;
        //}

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            dtFile = null;
            dtFileDeleted = null;
            DeleteTmpUserFolder();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", "window.close();", true);
        }

        private void DeleteTmpUserFolder()
        {
            string tmpPath = Path.Combine(Server.MapPath("~/Uploads/POUpDown/tmp"), this.UserID);

            if (Directory.Exists(tmpPath))
            {
                DirectoryInfo dir = new DirectoryInfo(tmpPath);

                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.IsReadOnly = false;
                    fi.Delete();
                }
                Directory.Delete(tmpPath);
            }
        }

      


    }
}