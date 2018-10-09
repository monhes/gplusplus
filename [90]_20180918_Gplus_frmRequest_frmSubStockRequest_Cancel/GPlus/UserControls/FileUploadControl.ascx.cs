using System;
using System.IO;
using System.Web.UI;


namespace GPlus.UserControls
{
    public partial class FileUploadControl : System.Web.UI.UserControl
    {
        public event DeletedFileHandler DeletedFile;

        public event UploadFileHandler UploadFile;

        // Methods
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            this.OnDeletedFile(this.hplFilePath.Text);
            this.DeleteFile();
            this.hplFilePath.Text = "";
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                this.SetUploadFile();
            }
            catch (Exception err)
            {
                this.lblError.Text = err.Message;
            }
        }

        public bool DeleteFile()
        {
            if (this.IsDeleteFile && (this.hplFilePath.Text.Trim() != ""))
            {
                new FileInfo(base.Server.MapPath(this.SaveFilePath) + @"\" + this.hplFilePath.Text).Delete();
                imgFile.Visible = false;
                this.btnDelete.Visible = false;
                return true;
            }
            return false;
        }

        public bool DeleteFile(string fileName)
        {
            new FileInfo(Path.Combine(base.Server.MapPath(this.SaveFilePath), fileName)).Delete();
            return true;
        }

        private void DownLoadFile(string fileName)
        {
            FileInfo myFile = new FileInfo(base.Server.MapPath(this.SaveFilePath) + @"\" + fileName);
            if (myFile.Exists)
            {
                base.Response.Clear();
                base.Response.AddHeader("Content-Disposition", "attachment; filename=" + myFile.Name.Replace(".resources", ""));
                base.Response.AddHeader("Content-Length", myFile.Length.ToString());
                base.Response.ContentType = "application/octet-stream";
                base.Response.WriteFile(myFile.FullName);
                base.Response.End();
            }
        }

        public string GetShotFileName(string fileName)
        {
            return fileName.Split(new char[] { '.' })[fileName.Split(new char[] { '.' }).Length - 1];
        }

        private void hplFilePath_Click(object sender, EventArgs e)
        {
            this.DownLoadFile(this.hplFilePath.Text);
        }

        protected void hplFilePath_Click1(object sender, EventArgs e)
        {
            this.DownLoadFile(this.hplFilePath.Text);
        }

        protected void OnDeletedFile(string fileName)
        {
            if (this.DeletedFile != null)
            {
                this.DeletedFile(fileName);
            }
        }

        protected void OnUploadFile(string fileName)
        {
            if (this.UploadFile != null)
            {
                this.UploadFile(fileName);
            }
        }

        public void SetFileName(string fileName)
        {
            if (fileName.Trim().Length > 0)
            {
                this.hplFilePath.Text = fileName;
                if (this.IsNotPicture)
                {
                    this.hplFilePath.Visible = true;
                }
                else
                {
                    this.hplFilePath.Visible = false;
                    this.imgFile.ImageUrl = base.ResolveUrl(this.SaveFilePath) + "/" + fileName;
                    this.imgFile.Visible = true;
                }
                if (this.IsDeleteFile)
                {
                    this.btnDelete.Visible = true;
                }
            }
        }

        public string SetUploadFile()
        {
            if ((this.fileUpload.PostedFile != null) && (this.fileUpload.PostedFile.ContentLength > 0))
            {
                string shotFileName = this.GetShotFileName(this.fileUpload.PostedFile.FileName);
                string fn = Guid.NewGuid().ToString() + "." + this.GetShotFileName(this.fileUpload.PostedFile.FileName);
                string SaveLocation = Path.Combine(Server.MapPath(this.SaveFilePath), fn);
                try
                {
                    if ((((this.GetShotFileName(this.fileUpload.PostedFile.FileName).ToLower() == "jpg") || (this.GetShotFileName(this.fileUpload.PostedFile.FileName).ToLower() == "gif")) || ((this.GetShotFileName(this.fileUpload.PostedFile.FileName).ToLower() == "png") ||
                            (this.GetShotFileName(this.fileUpload.PostedFile.FileName).ToLower() == "bmp") || (this.GetShotFileName(this.fileUpload.PostedFile.FileName).ToLower() == "png"))) || this.IsNotPicture)
                    {
                        this.fileUpload.PostedFile.SaveAs(SaveLocation);

                        if (this.IsDeleteFile)
                        {
                            this.btnDelete.Visible = true;
                        }

                        this.OnUploadFile(fn);
                        this.ShowPathFileUpload(fn);
                        this.lblError.Text = "";
                        return fn;
                    }
                    this.lblError.Text = "Please select image file only.";
                }
                catch (Exception ex)
                {
                    this.lblError.Text = "Error: " + ex.Message;
                }
            }
            return "";
        }

        protected void ShowPathFileUpload(string fileName)
        {
            if ((this.fileUpload.PostedFile != null) && (this.fileUpload.PostedFile.ContentLength > 0))
            {
                string fn = fileName;
                try
                {
                    this.hplFilePath.Text = fn;
                    if (this.IsNotPicture)
                    {
                        this.hplFilePath.Visible = true;
                    }
                    else if (this.IsDeleteFile)
                    {
                        this.imgFile.ImageUrl = base.ResolveUrl(this.SaveFilePath) + "/" + fn;
                        this.imgFile.Visible = true;
                    }
                }
                catch
                {
                    this.hplFilePath.Text = "";
                    this.imgFile.Visible = false;
                }
            }
            else
            {
                this.hplFilePath.Text = "";
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        // Properties
        public bool Enabled
        {
            set
            {
                this.btnUpload.Enabled = value;
            }
        }

        public string GetFileName
        {
            get
            {
                return this.hplFilePath.Text;
            }
        }

        public bool IsDeleteFile
        {
            get
            {
                if (this.ViewState[this.ClientID + "IsDeleteFile"] == null)
                {
                    this.ViewState[this.ClientID + "IsDeleteFile"] = true;
                }
                return (bool)this.ViewState[this.ClientID + "IsDeleteFile"];
            }
            set
            {
                this.ViewState[this.ClientID + "IsDeleteFile"] = value;
                this.btnDelete.Visible = value;
            }
        }

        public bool IsNotPicture
        {
            get
            {
                if (this.ViewState[this.ClientID + "IsNotPicture"] == null)
                {
                    this.ViewState[this.ClientID + "IsNotPicture"] = false;
                }
                return (bool)this.ViewState[this.ClientID + "IsNotPicture"];
            }
            set
            {
                this.ViewState[this.ClientID + "IsNotPicture"] = value;
            }
        }

        public bool Resize
        {
            get
            {
                if (this.ViewState[this.ClientID + "Resize"] == null)
                {
                    this.ViewState[this.ClientID + "Resize"] = true;
                }
                return (bool)this.ViewState[this.ClientID + "Resize"];
            }
            set
            {
                this.ViewState[this.ClientID + "Resize"] = value;
            }
        }

        public string SaveFilePath
        {
            get
            {
                if (this.ViewState[this.ClientID + "SaveFilePath"] == null)
                {
                    this.ViewState[this.ClientID + "SaveFilePath"] = "~/Uploads";
                }
                return this.ViewState[this.ClientID + "SaveFilePath"].ToString();
            }
            set
            {
                this.ViewState[this.ClientID + "SaveFilePath"] = value;
            }
        }

        public bool VisibleAttachment
        {
            get
            {
                return this.fileUpload.Visible;
            }
            set
            {
                this.fileUpload.Visible = value;
                this.btnUpload.Visible = this.fileUpload.Visible;
            }
        }

        public bool VisibleControl
        {
            get
            {
                return this.Panel1.Visible;
            }
            set
            {
                this.Panel1.Visible = value;
            }
        }

        // Nested Types
        public delegate void DeletedFileHandler(string fileName);

        public delegate void UploadFileHandler(string fileName);
    }
}