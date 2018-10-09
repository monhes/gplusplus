using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace GPlus.UserControls
{
    public partial class POPRAttachControl : System.Web.UI.UserControl
    {

        public enum FileMode
        {
            PR,
            PO
        }

        public FileMode AttachFileMode
        {
            get
            {
                if (ViewState["AttachFileMode"] == null) ViewState["AttachFileMode"] = FileMode.PR;

                return (FileMode)ViewState["AttachFileMode"];
            }
            set
            {
                ViewState["AttachFileMode"] = value;
                if(value == FileMode.PR) this.SavePath = "~/Uploads/PR/";
                else this.SavePath = "~/Uploads/PO/";
                if (!Directory.Exists(Server.MapPath(this.SavePath))) Directory.CreateDirectory(Server.MapPath(this.SavePath));
            }
        }

        public void DisableEdit()
        {
            btnBrowse.Visible = false;
            btnDelete.Visible = false;
        }

        public void EnableEdit()
        {
            btnBrowse.Visible = true;
            btnDelete.Visible = true;
        }

        public string SavePath
        {
            get
            {
                if (ViewState["SavePath"] == null)
                    ViewState["SavePath"] = "~/Uploads/PR/";

                return ViewState["SavePath"].ToString();
            }
            set
            {
                ViewState["SavePath"] = value;
            }
        }

        public string ReferenceID
        {
            get
            {
                if (ViewState["ReferenceID"] == null)
                    ViewState["ReferenceID"] = "";

                return ViewState["ReferenceID"].ToString();
            }
            set
            {
                ViewState["ReferenceID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void BindFile(string referenceID, FileMode attachFileMode)
        {
            this.AttachFileMode = attachFileMode;
            this.ReferenceID = referenceID;
            BindFile();
        }

        public void UpdateReference(string newReferenceID)
        {
            new DataAccess.FileDAO().UpdateFileReference(this.ReferenceID, newReferenceID);
            this.ReferenceID = newReferenceID;
        }

        private void BindFile()
        {
            if (this.AttachFileMode == FileMode.PR)
            {
                DataTable dt = new DataAccess.PRDAO().GetPRAttach(this.ReferenceID);
                if (dt.Rows.Count > 0) lstFile.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstFile.Items.Add(new ListItem(dt.Rows[i]["Attach_Path"].ToString(), dt.Rows[i]["PR_AttachID"].ToString()));
                }
            }
            else
            {
                DataTable dt = new DataAccess.PODAO().GetPOAttach(this.ReferenceID);
                if (dt.Rows.Count > 0) lstFile.Items.Clear();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstFile.Items.Add(new ListItem(dt.Rows[i]["Attach_Path"].ToString(), dt.Rows[i]["PO_AttachID"].ToString()));
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex > -1)
            {
                string fileName = lstFile.SelectedItem.Text;
                //if (fileName.ToLower().IndexOf(".jpg") > -1 || fileName.ToLower().IndexOf(".png") > -1 || fileName.ToLower().IndexOf(".gif") > -1)
                //{
                    //imgView.ImageUrl = ;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fi", "window.open('" + Util.GetSiteRoot() + Path.Combine(this.SavePath, fileName).Replace("~/", "") + "');", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "errImg", "alert('ไฟล์นี้ไม่ใช่รูปภาพ!');", true);
                //}
            }
        }


        protected void btnBrowseSave_Click(object sender, EventArgs e)
        {
            if ((this.fudFile.PostedFile != null) && (this.fudFile.PostedFile.ContentLength > 0))
            {
                if ((this.GetShotFileName(this.fudFile.PostedFile.FileName).ToLower() == "pdf")
                   || (this.GetShotFileName(this.fudFile.PostedFile.FileName).ToLower() == "png")
                   || (this.GetShotFileName(this.fudFile.PostedFile.FileName).ToLower() == "jpg") || this.AttachFileMode == FileMode.PR)
                {
                    string fn = Util.RemovePathFile(this.fudFile.PostedFile.FileName);
                    string SaveLocation = Path.Combine(Server.MapPath(this.SavePath), fn);
                    if (File.Exists(SaveLocation)) File.Delete(SaveLocation);
                    this.fudFile.PostedFile.SaveAs(SaveLocation);

                    if (this.AttachFileMode == FileMode.PR)
                        new DataAccess.PRDAO().AddPRAttach(this.ReferenceID, fn);
                    else
                        new DataAccess.PODAO().AddPOAttach(this.ReferenceID, fn);
                    BindFile();
                }
                else
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "alert('กรุณาเลือกไฟล์ pdf, png, jpg');", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex > -1)
            {
                if (this.AttachFileMode == FileMode.PR)
                    new DataAccess.PRDAO().DeletePRAttach(lstFile.Items[lstFile.SelectedIndex].Value);
                else
                    new DataAccess.PODAO().DeletePOAttach(lstFile.Items[lstFile.SelectedIndex].Value);
                BindFile();
            }
        }

        public string GetShotFileName(string fileName)
        {
            return fileName.Split(new char[] { '.' })[fileName.Split(new char[] { '.' }).Length - 1];
        }

    }
}