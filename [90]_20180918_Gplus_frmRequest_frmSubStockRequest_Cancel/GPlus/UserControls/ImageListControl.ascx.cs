using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace GPlus.UserControls
{
    public partial class ImageListControl : System.Web.UI.UserControl
    {

        public string SavePath
        {
            get
            {
                if (ViewState["SavePath"] == null)
                    ViewState["SavePath"] = "~/Uploads/Items/";

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
            ShowThumb();
        }

        private void ShowThumb()
        {
            if (lstFile.SelectedItem != null)
            {
                string saveLocation = Path.Combine(Server.MapPath(this.SavePath), lstFile.SelectedItem.Text);

                imgPreview.ImageUrl = Path.Combine(this.SavePath, lstFile.SelectedItem.Text);
                imgPreview.Visible = true;
            }
            else
            {
                imgPreview.Visible = false;
            }
        }


        public void BindFile(string referenceID)
        {
            this.ReferenceID = referenceID;
            this.SelectedValue = "";
            BindFile();
        }

        public void UpdateReference(string newReferenceID)
        {
            new DataAccess.FileDAO().UpdateFileReference(this.ReferenceID, newReferenceID);
            this.ReferenceID = newReferenceID;
        }

        private void BindFile()
        {
            lstFile.Items.Clear();
            DataTable dt = new DataAccess.FileDAO().GetFile(this.ReferenceID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstFile.Items.Add(new ListItem(dt.Rows[i]["Pic_path"].ToString(), dt.Rows[i]["Inv_ItemPicID"].ToString()));
            }
            if (lstFile.Items.FindByValue(this.SelectedValue) != null)
                lstFile.SelectedValue = this.SelectedValue;
            ShowThumb();
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex > -1)
            {
                string fileName = lstFile.SelectedItem.Text;
                if (fileName.ToLower().IndexOf(".jpg") > -1 || fileName.ToLower().IndexOf(".png") > -1 || fileName.ToLower().IndexOf(".gif") > -1
                   || fileName.ToLower().IndexOf(".bmp") > -1)
                {
                    //imgView.ImageUrl = ;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fi", "window.open('" + Util.GetSiteRoot()+ Path.Combine(this.SavePath, fileName).Replace("~/","") + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errImg", "alert('ไฟล์นี้ไม่ใช่รูปภาพ!');", true);
                }
            }
        }

        public string SelectedValue
        {
            get
            {
                if (ViewState["SelectedValue"] == null)
                    ViewState["SelectedValue"] = "";

                return ViewState["SelectedValue"].ToString();
            }
            set
            {
                ViewState["SelectedValue"] = value;
            }
        }


        protected void btnBrowseSave_Click(object sender, EventArgs e)
        {
            if ((this.fudFile.PostedFile != null) && (this.fudFile.PostedFile.ContentLength > 0))
            {
                if (this.fudFile.PostedFile.FileName.ToLower().IndexOf(".jpg") > -1 || this.fudFile.PostedFile.FileName.ToLower().IndexOf(".png") > -1
                    || this.fudFile.PostedFile.FileName.ToLower().IndexOf(".gif") > -1 || this.fudFile.PostedFile.FileName.ToLower().IndexOf(".bmp") > -1)
                {
                    string fn = Util.RemovePathFile(this.fudFile.PostedFile.FileName);
                    string SaveLocation = Path.Combine(Server.MapPath(this.SavePath), fn);
                    if (File.Exists(SaveLocation)) File.Delete(SaveLocation);
                    this.fudFile.PostedFile.SaveAs(SaveLocation);

                    new DataAccess.FileDAO().AddFile(this.ReferenceID, (lstFile.Items.Count + 1).ToString(), fn);
                    BindFile();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "errImg", "alert('กรุณา Upload เฉพาะไฟล์รูปภาพ!');", true);
                }
            }
        }

        protected void btnUp_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex > 0)
            {
                DataAccess.FileDAO db = new DataAccess.FileDAO();
                this.SelectedValue = lstFile.Items[lstFile.SelectedIndex].Value;
                //Current Up
                db.UpdateFile(lstFile.Items[lstFile.SelectedIndex].Value,
                    lstFile.SelectedIndex.ToString());

                //Previous Down
                db.UpdateFile(lstFile.Items[lstFile.SelectedIndex-1].Value,
                    (lstFile.SelectedIndex+1).ToString());

                BindFile();
            }
        }

        protected void btnDown_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex < lstFile.Items.Count-1)
            {
                this.SelectedValue = lstFile.Items[lstFile.SelectedIndex].Value;
                DataAccess.FileDAO db = new DataAccess.FileDAO();

                //Current Down
                db.UpdateFile(lstFile.Items[lstFile.SelectedIndex].Value,
                    (lstFile.SelectedIndex + 1).ToString());

                //Next Up
                db.UpdateFile(lstFile.Items[lstFile.SelectedIndex + 1].Value,
                    lstFile.SelectedIndex.ToString());

                BindFile();
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstFile.SelectedIndex > -1)
            {
                new DataAccess.FileDAO().DeleteFile(lstFile.Items[lstFile.SelectedIndex].Value);
                BindFile();
            }
        }

    }
}