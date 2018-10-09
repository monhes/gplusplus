using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;

using GPlus.PRPO.PRPOHelper;

namespace GPlus.PRPO
{
    public partial class POMgt : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "301";
                ccFrom.Text = DateTime.Now.AddDays(-7).ToString(this.DateFormat);
                ccTo.Text = DateTime.Now.ToString(this.DateFormat);
                txtPOCodeSearch.Attributes.Add("onchange", "txtPOCodeSearchChange();");
                BindData();

                PRPOSession.UserID = this.UserID;
            }
            else
            {
                txtPOCodeSearch.Attributes.Remove("onchange");
            }

            if (Request["showDetail"] == "true")
            {
                pnlDetail.Visible = true;
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void ClearUploadPOTmp()
        {
            string path = Server.MapPath(Path.Combine(PRPOPath.POTmpUpload, UserID));

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    try { File.Delete(file); } 
                    catch (Exception) { }
                }
            }
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            PagingControl1.CurrentPageIndex = 1;
            BindData();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtPOCodeSearch.Text = "";
            ccFrom.Text = "";
            ccTo.Text = "";
            rblStockType.SelectedIndex = 2;
            ddlPOType.SelectedIndex = 0;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            PRPOSession.InitializePOPurchase();
            PRPOSession.Action = PRPOAction.ADD_PO;
            PRPOSession.PoID = null;
            PRPOSession.PrID = null;

            ClearUploadPOTmp();

            Response.Write("<script>window.location.href = '../PRPO/PoMgt.aspx?showDetail=true';</script>");

            //ScriptManager.RegisterStartupScript
            //(
            //    this, 
            //    GetType(), 
            //    "scroll",
            //    "$('body,html').animate({ scrollTop: $('body').height() });", 
            //    true
            //);
        }

        protected void gvPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                ((ImageButton)e.Row.FindControl("btnPrint")).OnClientClick = "open_popup('pop_PO.aspx?id=" + drv["PO_ID"].ToString()
                + "', 850, 450, 'pop', 'yes', 'yes', 'yes'); return false;";

                if (drv["PO_Type"].ToString() == "1")
                    e.Row.Cells[3].Text = "สั่งซื้อ";
                else if (drv["PO_Type"].ToString() == "2")
                    e.Row.Cells[3].Text = "สั่งจ้าง";

                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateFormat);

                if (drv["Net_Amonut"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((decimal)drv["Net_Amonut"]).ToString(this.CurrencyFormat);

                Button btnUpload = (Button)e.Row.FindControl("btnUpload");

                switch (drv["Status"].ToString().ToUpper())
                {
                    case "0": e.Row.Cells[8].Text = "ยกเลิก"; break;
                    case "1": e.Row.Cells[8].Text = "รออนุมัติ"; break;
                    case "2": e.Row.Cells[8].Text = "อนุมัติ"; 
                        btnUpload.Visible = true;
                       
                        #region Nin Add 07022014

                        DataTable dt2 = new DataAccess.DatabaseHelper().ExecuteQuery("SELECT MAX(PO_Upload_ID) AS Max_UploadID, MAX(Download_Count) AS Max_DownloadCnt FROM [dbo].[Inv_PO_UpDown] WHERE PO_ID = " + drv["PO_ID"].ToString()).Tables[0];
                        if (dt2.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dt2.Rows[0]["Max_DownloadCnt"].ToString() == "" ? "0" : dt2.Rows[0]["Max_DownloadCnt"].ToString()) > 0)
                                e.Row.Cells[8].Text = "Download PO";
                            else if (Convert.ToInt32(dt2.Rows[0]["Max_UploadID"].ToString() == "" ? "0" : dt2.Rows[0]["Max_UploadID"].ToString()) > 0)
                                e.Row.Cells[8].Text = "Upload PO";
                            else
                                e.Row.Cells[8].Text = "อนุมัติ";
                        }

                        #endregion

                         DataTable dt = new DataAccess.DatabaseHelper().ExecuteQuery("SELECT COUNT(*) AS RowNum FROM Inv_Receive_Stk WHERE PO_ID = " + drv["PO_ID"].ToString() + " AND Status <> '0'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["RowNum"].ToString() != "0")
                                e.Row.Cells[8].Text = "รับเข้าคลังแล้ว";
                        }

                        break;
                    case "3": e.Row.Cells[8].Text = "ไม่อนุมัติ"; break;
                    case "4": e.Row.Cells[8].Text = "ตรวจสอบ PO"; break;
                    case "5": e.Row.Cells[8].Text = "ไม่ดำเนินการสั่งซื้อ"; break;
                    //case "U": e.Row.Cells[8].Text = "Upload PO"; btnUpload.Visible = true; break;
                    //case "P": e.Row.Cells[8].Text = "Download PO"; btnUpload.Visible = true; break;
                }
                btnUpload.CommandArgument = drv["PO_ID"].ToString();

                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");
                btnDetail.CommandArgument = drv["PO_ID"].ToString();
                ImageButton btnPrint = (ImageButton)e.Row.FindControl("btnPrint");
            }
        }

        protected void gvPO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                pnlDetail.Visible = true;

                string poId = e.CommandArgument.ToString();

                PRPOSession.Action = PRPOAction.VIEW_PO;    // Use in UploadFile.aspx.cs
                PRPOSession.PoID = poId;                    // Use in UploadFile.aspx.cs
                PRPOSession.PrID = null;
                POControl1.BindPO(poId);
                
                //pnlDetail.Visible = true;

                //Button btnRecal = POControl1.FindControl("btnReCal") as Button;
                //btnRecal.Visible = false;

                //// ----------------------- fix 1, 3 ----------------------
                //HiddenField hdStatus = POControl1.FindControl("hdStatus") as HiddenField;
                //Button btnSave = POControl1.FindControl("btnSave") as Button;
                //Button btnDel = POControl1.FindControl("btnDelete") as Button;
                //if (hdStatus.Value == "1" || hdStatus.Value == "4")
                //{
                //    btnSave.Visible = true;
                //    btnSave.Enabled = true;
                //    btnDel.Visible = true;
                //    btnDel.Enabled = true;
                //}
                //else
                //{
                //    btnSave.Visible = false;
                //    btnSave.Enabled = false;
                //    btnDel.Visible = false;
                //    btnDel.Enabled = false;
                //}

                //Button btnDeleteItem = POControl1.FindControl("btnDeleteItem") as Button;
                //if (hdStatus.Value == "1")
                //{ 
                //    btnDeleteItem.Visible = true;
                //}
                //else
                //{
                //    btnDeleteItem.Visible = false;
                //}
                
                //ScriptManager.RegisterStartupScript(
                //    this, 
                //    this.GetType(), 
                //    "scroll",
                //    "$('body,html').animate({ scrollTop: $('body').height() });", 
                //    true
                //);
                // --------------------------------------------------------------
            }
            else if (e.CommandName == "Upd")
            {
                hdID.Value = e.CommandArgument.ToString();
                //mpeUpload.Show();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", @" 
                window.open('UploadPO.aspx?id=" + hdID.Value + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
                ", true);
            }
        }


        protected void btnBrowseSave_Click(object sender, EventArgs e)
        {
            if ((this.fudFile.PostedFile != null) && (this.fudFile.PostedFile.ContentLength > 0))
            {
                if ((this.GetShotFileName(this.fudFile.PostedFile.FileName).ToLower() == "pdf")
                    || (this.GetShotFileName(this.fudFile.PostedFile.FileName).ToLower() == "png")
                    || (this.GetShotFileName(this.fudFile.PostedFile.FileName).ToLower() == "jpg"))
                {
                    string savePath = "~/Uploads/POUpDown/";
                    if (!Directory.Exists(Server.MapPath(savePath))) Directory.CreateDirectory(Server.MapPath(savePath));

                    string fn = Util.RemovePathFile(this.fudFile.PostedFile.FileName);

                    #region Nin Edit 27/01/2014

                    string extension = Path.GetExtension(fn);
                    string datetime = string.Format("{0:ddMMyyyyHHmmssffff}", DateTime.Now);

                    fn = fn.Substring(0, fn.LastIndexOf('.')) + "_" + datetime + extension;

                    #endregion

                    string SaveLocation = Path.Combine(Server.MapPath(savePath), fn);
                    if (File.Exists(SaveLocation)) File.Delete(SaveLocation);
                    this.fudFile.PostedFile.SaveAs(SaveLocation);

                    new DataAccess.PODAO().AddPOUpDown(hdID.Value, this.UserName, Server.MapPath(savePath), fn);
                    hdID.Value = "";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "succ","alert('Upload ไฟล์สำเร็จ');", true);

                    BindData();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "succ", "alert('กรุณาเลือกไฟล์ pdf, png, jpg');", true);
                }
            }
        }

        protected void gvPO_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindData();
            this.GridViewSort(gvPO);
        }

        private void BindData()
        {
            DataSet ds = new DataAccess.PODAO().GetPOForm1(
                                txtPOCodeSearch.Text, 
                                ddlPOType.SelectedValue, 
                                rblStockType.SelectedValue,
                                ccFrom.Text, 
                                ccTo.Text,
                                "",
                                "", 
                                chkIsUpload.Checked?"1":"0", 
                                "", 
                                "", 
                                "", 
                                PagingControl1.CurrentPageIndex, 
                                PagingControl1.PageSize,
                                this.SortColumn, 
                                this.SortOrder); 

            PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvPO.DataSource = ds.Tables[0];
            gvPO.DataBind();
        }

        public string GetShotFileName(string fileName)
        {
            return fileName.Split(new char[] { '.' })[fileName.Split(new char[] { '.' }).Length - 1];
        }
    }
}