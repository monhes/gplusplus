using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using System.Data;
using System.Transactions;

namespace GPlus.Request
{
    public partial class popupReceiveDetail : Pagebase
    {

        private void BindData(){

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack){

                try
                {
                    txtNoPay.Text = Request["Pay_No"].ToString();
                    txtWhoPay.Text = Request["Pay_Name"].ToString();
                    txtDatePay.Text = Request["Pay_Date"].ToString();


                    DataTable dt = new RequestDAO().ReqInvReqRecSelectByPayID(int.Parse(Request["PayID"].ToString()));

                    gvReqRec.DataSource = dt;
                    gvReqRec.DataBind();

                    if(dt.Rows.Count > 0){

                        txtNoRcv.Text = dt.Rows[0]["rownumber"].ToString();
                        txtRcvDate.Text = dt.Rows[0]["Receive_Date"].ToString();
                        txtRcver.Text = dt.Rows[0]["Account_Fname"].ToString();

                        if (dt.Rows[0]["Status"].ToString() == "0")
                        {
                            btCancelReceive.Visible = false;
                            gvReqRecItem.Columns[8].Visible = false;
                        }
                        else
                        {
                            btCancelReceive.Visible = true;
                            gvReqRecItem.Columns[8].Visible = true;
                        }

                        DataTable dtt = new RequestDAO().ReqInvReqRecItemSelectByRecPayID(int.Parse(dt.Rows[0]["RecPay_Id"].ToString()));



                        Session["ReqRecDataList"] = dtt;
                        gvReqRecItem.DataSource = dtt;
                        gvReqRecItem.DataBind();

                    }
                  
                }
                catch (Exception ex)
                {
                    Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                    Response.Redirect("../Stock/Error.aspx");
                }

            }
          
          
        }

        protected void gvReqRecItemDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HiddenField hdPack = e.Row.FindControl("packId") as HiddenField;

             
                DataRowView drv = (DataRowView)e.Row.DataItem;
                hdPack.Value = drv["Pack_ID"].ToString();
                int rQty = Util.ToInt(drv["Receive_Qty"].ToString());
                if(rQty ==0 ){
                 CheckBox chk =   e.Row.FindControl("chkCancelList") as CheckBox;
                 chk.Enabled = false;
                }
            }
        }

        protected void GvReqRecRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow){


                DataRowView drv = (DataRowView)e.Row.DataItem;


                if (drv["Status"].ToString().Trim() == "0")
                {
                    e.Row.Cells[4].Text = "<span style='color:dimgray;'>ยกเลิก</span>";
                }
                else
                {
                    e.Row.Cells[4].Text = "<span style='color:blue;'>ใช้งาน</span>";
                }

                LinkButton linkButton = e.Row.Cells[0].FindControl("btnDetail") as LinkButton;
                linkButton.CommandArgument = drv["rownumber"].ToString() + "," + drv["RecPay_Id"].ToString()
                     + "," + drv["Receive_Date"].ToString() + "," + drv["Account_Fname"].ToString() + "," + drv["Status"].ToString();

            }
          
        }
        protected void GvReqRecRowCommand(object sender,  GridViewCommandEventArgs e)
        {

            if(e.CommandName == "ViewReceive"){
             
               string [] s = e.CommandArgument.ToString().Split(',');
               bool isCancel = (s[4] == "0") ? false : true;
               if (isCancel)
               {
                   btCancelReceive.Visible = true;
                   gvReqRecItem.Columns[8].Visible = true;
               }
               else
               {
                   btCancelReceive.Visible = false;
                   gvReqRecItem.Columns[8].Visible = false;
               }
               txtNoRcv.Text = s[0];
               txtRcvDate.Text = s[2];
               txtRcver.Text = s[3];
               string recPayID = s[1];

               DataTable dt = new RequestDAO().ReqInvReqRecItemSelectByRecPayID(int.Parse(recPayID));
               Session["ReqRecDataList"] = dt;
               gvReqRecItem.DataSource = dt;
               gvReqRecItem.DataBind();


            }
        }

       
     


        protected void btCancelReceive_Click(object sender, EventArgs e)
        {
            //------------------    ยกเลิกรับ    -----------------------------

            try{

                    DataTable dt =  Session["ReqRecDataList"] as DataTable;
                    bool DoCancel = false;
                    using (TransactionScope scope = new TransactionScope())
                    {

                        foreach( GridViewRow gr in gvReqRecItem.Rows){
                          CheckBox c =  gr.FindControl("chkCancelList") as CheckBox;
                          HiddenField hdPack = gr.FindControl("packid") as HiddenField;
                            
                                string packId = gr.Cells[1].Text.Trim();
                                DataRow rr =   dt.AsEnumerable().Where(r => r["Pack_ID"].ToString() == hdPack.Value && r["Inv_ItemCode"].ToString() == packId ).FirstOrDefault();
                              
                                if (c.Checked && Util.ToInt(rr["Receive_Qty"].ToString()) > 0 )
                               {
                                   bool isCancel = new RequestDAO().CancelReqRecItemsSubStock(int.Parse(rr["RecPay_ItemID"].ToString()), int.Parse(rr["Stock_Id_Req"].ToString())
                                                , this.UserID);

                                   if (!isCancel)
                                   {

                                       scope.Dispose();
                                       ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('รายการ + " + rr["Inv_ItemName"].ToString() + " ได้มีการจ่ายสินค้าแล้ว');</script>");
                                       return;
                                   }
                                   DoCancel = true;
                               }
                        }

                        if (DoCancel)
                        {
                            scope.Complete();

                            ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('ยกเลิกเรียบร้อย');</script>");
                            string scriptStr = "if(window.opener)window.opener.document.getElementById('ContentPlaceHolder1_btnRefreshItem2').click(); window.close();";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "closing", scriptStr, true);
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('ท่านยังไม่ได้เลือกรายการ');</script>");
                        }

                       
                    }


               
            }catch (TransactionAbortedException ex)
            {
                   Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                   Response.Redirect("../Stock/Error.aspx");
                  
            }
               catch (Exception ex)
            {
                   Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                   Response.Redirect("../Stock/Error.aspx");
                  
            }
   

        }
    }
}