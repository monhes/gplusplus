using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using GPlus.PRPO.PRPOHelper;
using GPlus.DataAccess;

namespace GPlus.PRPO
{
    public partial class UploadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bDelete.Attributes.Add("style", "display:none");
            if (PRPOSession.UserID == null)
                fileUploader.Attributes.Add("disabled", "disabled");

            if (!IsPostBack)
            {
                string poprId = "";

                if (!string.IsNullOrEmpty(PRPOSession.PoID))
                    poprId = PRPOSession.PoID;
                else if (!string.IsNullOrEmpty(PRPOSession.PrID))
                    poprId = PRPOSession.PrID;

                if (!string.IsNullOrEmpty(poprId))
                {
                    string id = "";
                    string js = "";
                    DataTable dt = null;

                    if (PRPOSession.Action == PRPOAction.VIEW_PR)
                    {
                        id = "PR_AttachID";
                        dt = new PRDAO().GetPRAttach(PRPOSession.PrID);
                    }
                    else if (PRPOSession.Action == PRPOAction.VIEW_PO)
                    {
                        id = "PO_AttachID";
                        dt = new PODAO().GetPOAttach(PRPOSession.PoID);
                    }

                    PRPOUploadFileTable puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);

                    foreach (DataRow r in dt.Rows)
                    {
                        string url = GetURLUploadedFile(r["Attach_Path"].ToString());

                        string urlFile = HttpUtility.JavaScriptStringEncode(url, true);

                        string fileName = GetFileNameWithOutDateTimeFormat(r["Attach_Path"].ToString());
                        fileName = HttpUtility.JavaScriptStringEncode(fileName, true);

                        puft.AddItem(Convert.ToInt32(r[id]), r["Attach_Path"].ToString());

                        js += "AddUploadingFiles(" + urlFile + "," + fileName + "," + r[id].ToString() + ");";
                    }

                    ScriptManager.RegisterStartupScript
                    (
                        Page
                        , GetType()
                        , "upload"
                        , ScriptAddUploadingFiles() + js
                        , true
                    );
                }
            }
        }

        protected void bUpload_Click(object sender, EventArgs e)
        {
            PRPOUploadFileTable puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);

            CreateTmpFolderIfNotExist();
            CreateUserIdFolderIfNotExist();

            SaveFileToUserIdFolder(puft);
            GenerateScriptAddUploadingFiles(puft);
        }

        protected void bDelete_Click(object sender, EventArgs e)
        {
            PRPOUploadFileTable puft = new PRPOUploadFileTable(PRPOSession.UploadFileTable);
            DeleteFile(puft);
            GenerateScriptAddUploadingFiles(puft);
        }

        private void GenerateScriptAddUploadingFiles(PRPOUploadFileTable puft)
        {
            string js = "";

            for (int i = 0; i < puft.Table.Rows.Count; ++i)
            {
                DataRow row = puft.Table.Rows[i];

                string uploadingPath = "";

                if ((int) row["Id"] > 0)
                    uploadingPath = HttpUtility.JavaScriptStringEncode(GetURLUploadedFile(row["FileName"].ToString()), true);
                else
                    uploadingPath = HttpUtility.JavaScriptStringEncode(GetURLUploadingFile(row["FileName"].ToString()), true);

                string fileName = HttpUtility.JavaScriptStringEncode(GetFileNameWithOutDateTimeFormat(row["FileName"].ToString()), true);

                js += "AddUploadingFiles(" + uploadingPath + "," + fileName + "," + row["Id"] + ");";
            }

            ScriptManager.RegisterStartupScript
            (
                this
                , GetType()
                , "AddUploadingFiles"
                , ScriptAddUploadingFiles() + js
                , true
            );
        }

        private string GetURLUploadingFile(string fileName)
        {
            string prefixPath = "";

            if (PRPOSession.Action == PRPOAction.ADD_PO || PRPOSession.Action == PRPOAction.VIEW_PO)
                prefixPath = "Uploads/PO/tmp/";
            else if (PRPOSession.Action == PRPOAction.ADD_PR || PRPOSession.Action == PRPOAction.VIEW_PR)
                prefixPath = "Uploads/PR/tmp/";

            return Util.GetSiteRoot() + prefixPath + PRPOSession.UserID + "/" + fileName;
        }

        private string GetURLUploadedFile(string fileName)
        {
            string prefixPath = "";

            if (PRPOSession.Action == PRPOAction.ADD_PO || PRPOSession.Action == PRPOAction.VIEW_PO)
                prefixPath = "Uploads/PO/";
            else if (PRPOSession.Action == PRPOAction.ADD_PR || PRPOSession.Action == PRPOAction.VIEW_PR)
                prefixPath = "Uploads/PR/";

            return Util.GetSiteRoot() + prefixPath + fileName;
        }

        private void SaveFileToUserIdFolder(PRPOUploadFileTable puft)
        {
            string fileName = Request.Files["fileUploader"].FileName;

            // prevent IE including local path
            fileName = Path.GetFileName(fileName);

            string fileNameWithDateTime = GetFileNameWithDateTimeFormat(fileName);

            Request.Files["fileUploader"].SaveAs(Path.Combine(GetUserIdUploadPath(), fileNameWithDateTime));
            puft.AddItem(fileNameWithDateTime);
        }

        private string GetFileNameWithDateTimeFormat(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string datetime = string.Format("{0:ddMMyyyyHHmmssffff}", DateTime.Now);

            return fileName.Substring(0, fileName.LastIndexOf('.')) + "_" + datetime + extension;
        }

        private string GetFileNameWithOutDateTimeFormat(string fileName)
        {
            int lastUnderscoreIndex = fileName.LastIndexOf('_');
            int lastDotIndex = fileName.LastIndexOf('.');

            return fileName.Remove(lastUnderscoreIndex, lastDotIndex - lastUnderscoreIndex);
        }

        private void DeleteFile(PRPOUploadFileTable puft)
        {
            int id = Convert.ToInt32(Request.Form["fileId"]);

            if (id < 0)
            {
                DataRow row = puft.Table.Select(string.Format("Id = {0}", id)).FirstOrDefault();
                if (row != null)
                {
                    string uploadingFilePath = Path.Combine(GetUserIdUploadPath(), row["FileName"].ToString());

                    if (File.Exists(uploadingFilePath))
                    {
                        try { File.Delete(uploadingFilePath); }
                        catch (Exception) { }
                    }

                    puft.Table.Rows.Remove(row);
                    puft.Table.AcceptChanges();
                }
            }
            else
            {
                PRPOAttachDeleteTable padt = new PRPOAttachDeleteTable(PRPOSession.AttachDeleteTable);
                DataRow row = puft.Table.Select(string.Format("Id = {0}", id)).FirstOrDefault();
                if (row != null)
                {
                    padt.AddItem(id, row["FileName"].ToString());
                    puft.Table.Rows.Remove(row);
                    puft.Table.AcceptChanges();
                }
            }
        }

        public string ScriptAddUploadingFiles()
        {
            string js = "function AddUploadingFiles(uploadingPath, fileName, id)"
                      + "{"
                      + "    var $uploadingFiles = document.getElementById('uploadingFiles');"
                      + "    var $div = document.createElement('div');"
                      + "    var $view = document.createElement('a');"
                      + "    var $delete = document.createElement('a');"
                      + "    var $filename = document.createElement('span');"
                      + "    var $space = document.createElement('span');"

                      + "    $filename.innerHTML = fileName + '&nbsp;&nbsp;';"

                      + "    $view.innerHTML = '<b>ดู</b>';"
                      + "    $view.style.color = 'blue';"
                      + "    $view.onclick = function ()"
                      + "    {"
                      + "        window.open(uploadingPath, '', 'width=800,height=500,resizable=yes,scrollbars=yes');"
                      + "    };"
                      + "    $view.onmouseover = function ()"
                      + "    {"
                      + "        this.style.cursor = 'pointer';"
                      + "    };"

                      + "    $delete.innerHTML = '<b>ลบ</b>';"
                      + "    $delete.style.color = 'red';"
                      + "    $delete.onclick = function ()"
                      + "    {"
                      + "        if (!confirm('ต้องการลบหรือไม่')) return;"
                      + "        var $hiddenId = document.getElementById('hiddenId');"
                      + "        var $input = document.createElement('input');"
                      + "        $input.type = 'hidden';"
                      + "        $input.name = 'fileId';"
                      + "        $input.value = id;"
                      + "        $hiddenId.appendChild($input);"
                      + "        document.getElementById('" + bDelete.ClientID + "').click();"
                      + "    };"
                      + "    $delete.onmouseover = function ()"
                      + "    {"
                      + "        this.style.cursor = 'pointer';"
                      + "    };"

                      + "    $space.innerHTML = '&nbsp;&nbsp;';"

                      + "    $div.style.padding = '5px';"
                      + "    $div.style.borderBottom = '1px solid #aaa';"
                      + "    $div.onmouseover = function ()"
                      + "    {"
                      + "        this.style.backgroundColor = 'pink';"
                      + "    };"
                      + "    $div.onmouseout = function ()"
                      + "    {"
                      + "        this.style.backgroundColor = '#E0ECF8';"
                      + "    };"
                      + "    $div.appendChild($filename);"
                      + "    $div.appendChild($view);"
                      + "    $div.appendChild($space);"
                      + "    $div.appendChild($delete);"

                      + "    $uploadingFiles.appendChild($div);"
                      + "}";

            return js;
        }

        private void CreateTmpFolderIfNotExist()
        {
            string tmpPath = GetTmpUploadPath();

            if (!Directory.Exists(tmpPath))
                Directory.CreateDirectory(tmpPath);
        }

        private void CreateUserIdFolderIfNotExist()
        {
            string userIdPath = GetUserIdUploadPath();

            if (!Directory.Exists(userIdPath))
                Directory.CreateDirectory(userIdPath);
        }

        private string GetTmpUploadPath()
        {
            if (PRPOSession.Action == PRPOAction.ADD_PO || PRPOSession.Action == PRPOAction.VIEW_PO)
                return Server.MapPath(PRPOPath.POTmpUpload);
            else if (PRPOSession.Action == PRPOAction.ADD_PR || PRPOSession.Action == PRPOAction.VIEW_PR)
                return Server.MapPath(PRPOPath.PRTmpUpload);
            return null;
        }

        private string GetUserIdUploadPath()
        {
            if (PRPOSession.Action == PRPOAction.ADD_PO || PRPOSession.Action == PRPOAction.VIEW_PO)
                return Path.Combine(Server.MapPath(PRPOPath.POTmpUpload), PRPOSession.UserID);
            else if (PRPOSession.Action == PRPOAction.ADD_PR || PRPOSession.Action == PRPOAction.VIEW_PR)
                return Path.Combine(Server.MapPath(PRPOPath.PRTmpUpload), PRPOSession.UserID);
            return null;
        }
    }
}