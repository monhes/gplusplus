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

namespace GPlus.Stock
{
    public partial class Pop_CloseReceiveStk : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
            
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void BindData()
        {
            string po_id = Request["id"].ToString();

            DataSet ds = new ReceiveStockDAO().GetCloseRec(po_id,1,1000,"","");
            DataTable dt = new ReceiveStockDAO().GetPO_Close_SelectByPOID(po_id);

            if (ds.Tables[1].Rows.Count > 0)
            {
                gvCloseRec.Visible = true;

                if(dt.Rows.Count > 0)
                {
                    txtPOCode.Text = dt.Rows[0]["PO_Code"].ToString();
                    txtPODate.Text = ((DateTime)dt.Rows[0]["PO_Date"]).ToString(this.DateFormat);
                    txtLastRecDate.Text = ((DateTime)dt.Rows[0]["Max_ReceiveDate"]).ToString(this.DateTimeFormat);
                }

                gvCloseRec.DataSource = ds.Tables[0];
                gvCloseRec.DataBind();
            }
            else
            {
                gvCloseRec.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
                return;
            }
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            //BindData(txtDivCode.Text.ToString(), txtDepCode.Text.ToString(), txtDescription.Text.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //BindData(txtDivCode.Text.ToString(), txtDepCode.Text.ToString(), txtDescription.Text.ToString());
        }

        protected void gvCloseRec_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                HiddenField hdPOItem_ID = (HiddenField)e.Row.FindControl("hdPOItem_ID");
                CheckBox chkClose = ((CheckBox)e.Row.FindControl("chkClose"));
                ImageButton CommentClose = ((ImageButton)e.Row.FindControl("CommentClose"));
                HiddenField hdRemarkClose = (HiddenField)e.Row.FindControl("hdRemarkClose");

                hdPOItem_ID.Value = drv["POItem_ID"].ToString();

                if (drv["DeliveryStop_UpdateDate"].ToString().Trim().Length > 0)
                    e.Row.Cells[9].Text = drv["FullName_Update_By"].ToString() + ", " + ((DateTime)drv["DeliveryStop_UpdateDate"]).ToString(this.DateTimeFormat);

                string bool_Close = "t";

                if (drv["DeliveryStop_flag"].ToString() == "1")
                {
                    chkClose.Checked = true;
                    hdRemarkClose.Value = drv["DeliveryStop_Remark"].ToString();
                }
                else
                {
                    chkClose.Checked = false;
                    hdRemarkClose.Value = "";
                }

                /*  Check ว่าสามารถปิดการรับ ได้หรือไม่ 
                           ปิดได้ ก็ต่อเมื่อ จำนวนที่สั่ง >  (จำนวนที่รับ) */

                if (Util.ToDecimal(drv["Unit_Quantity"].ToString()) > (Util.ToDecimal(drv["Receive_Quantity"].ToString())))
                {

                    chkClose.Enabled = true;
                    CommentClose.Enabled = true;
                    bool_Close = "t";
                }
                else
                {
                    chkClose.Enabled = false;
                    CommentClose.Enabled = true;
                    bool_Close = "f";
                }


                //CommentClose.OnClientClick = "open_popup('pop_Remark.aspx?ctl=" + hdRemarkClose.ClientID
                //                          + "&POItem_ID=" + drv["POItem_ID"].ToString() + "&chk=" + bool_Close
                //                          + "', 500, 200, 'CloseRec', 'yes', 'yes', 'yes'); return false;";

                //CommentClose.OnClientClick = "open_popup('pop_Remark.aspx, 500, 200, 'CloseRec', 'yes', 'yes', 'yes');";


                CommentClose.OnClientClick = "var Mleft = (screen.width/2)-(500/2);var Mtop = (screen.height/2)-(250/2);window.open( 'pop_Remark.aspx?ctl=" + hdRemarkClose.ClientID
                                           + "&POItem_ID=" + drv["POItem_ID"].ToString() + "&chk=" + bool_Close
                                           + "', 'CloseRec', 'height=200,width=500,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );";

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow item in gvCloseRec.Rows)
                {
                    HiddenField hdPOItem_ID = (HiddenField)item.FindControl("hdPOItem_ID");
                    CheckBox chkClose = ((CheckBox)item.FindControl("chkClose"));
                    HiddenField hdRemarkClose = (HiddenField)item.FindControl("hdRemarkClose");

                    string bool_chkclose = "";

                    if (chkClose.Checked == true)
                    {
                        bool_chkclose = "1";
                    }
                    else //ถ้าไม่ได้ติ๊กปิด Stock ให้ Remark เป็น ""
                    {
                        hdRemarkClose.Value = "";
                    }

                    new ReceiveStockDAO().UpdateCloseStk(hdPOItem_ID.Value, hdRemarkClose.Value, bool_chkclose, this.UserID);
                }

                ShowMessageBox("บันทึกข้อมูลเรียบร้อย");
            }
            catch (Exception ex)
            {
                ShowMessageBox("ไม่สามารถบันทึกได้");
                return;
            }


            ScriptManager.RegisterStartupScript(this, this.GetType(), "saved", "window.opener.document.getElementById('" + Request["ref"] +
                "').click();window.close();", true);

            
        }

    }
}