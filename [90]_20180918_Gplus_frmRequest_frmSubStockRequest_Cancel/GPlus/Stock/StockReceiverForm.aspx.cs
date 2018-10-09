using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using GPlus.Stock.Commons;
using System.Diagnostics;
using GPlus.ModelClass;
using System.Transactions;

namespace GPlus.Stock
{
    public partial class StockReciverForm : Pagebase
    {

        private ReceiveStkModel ReceiveStkModel
        {
            get
            {
                ReceiveStkModel rcvModel = (ReceiveStkModel)Session["ReceiveStkModel"];
                return rcvModel;
            }
            set
            {
                Session["ReceiveStkModel"] = value;
            }
        }


        private void ClearReceiveStkModel()
        {
            Session.Remove("ReceiveStkModel");
        }
        private List<StockReceiveItem> _stockItemCollection
        {
            get { return (List<StockReceiveItem>)Session["sriCollection"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            // รับเข้าคลัง
            if (rdStockType.SelectedIndex == 0)
            {
                btnClose.Visible = false;
                btnSave.Visible = true;
             
                gvStkItem.Columns[15].Visible = true;
                gvStkItem.Columns[16].Visible = false;

            

            }
            else   // รับแล้ว
            {
                btnClose.Visible = true;
                btnSave.Visible = false;
                gvStkItem.Columns[15].Visible = false;
                gvStkItem.Columns[16].Visible = true;


            }


            if (!IsPostBack)
            {
                this.PageID = "401";
                BindDropdown();
                BindSearchData();

                this.chkPaymentType.Attributes.Add("onclick", "radioMe(event);");
                this.txtTradeDiscountPercent.Attributes["onchange"] = "javascript:OnTextChange('" + this.txtTradeDiscountPercent.ClientID + "', '" + this.txtTradeDiscountAmount.ClientID + "')";
                this.txtTradeDiscountAmount.Attributes["onchange"] = "javascript:OnTextChange('" + this.txtTradeDiscountAmount.ClientID + "', '" + this.txtTradeDiscountPercent.ClientID + "')";
                this.txtCashDiscountPercent.Attributes["onchange"] = "javascript:OnTextChange('" + this.txtCashDiscountPercent.ClientID + "', '" + this.txtCashDiscountAmount.ClientID + "')";
                this.txtCashDiscountAmount.Attributes["onchange"] = "javascript:OnTextChange('" + this.txtCashDiscountAmount.ClientID + "', '" + this.txtCashDiscountPercent.ClientID + "')";

                dtCloseRec = new DataTable();
                dtCloseRec.Columns.Add("POItem_ID", typeof(string));
                dtCloseRec.Columns.Add("DeliveryStop_flag", typeof(string));
                dtCloseRec.Columns.Add("DeliveryStop_Remark", typeof(string));
                dtCloseRec.Columns.Add("DeliveryStop_FromSave", typeof(string)); //เป็นตัว check ว่าข้อมูลมาจาก DB หรือในหน้านี้
                //dtCloseRec.Columns.Add("DeliveryStop_UpdateBy", typeof(int));
                //dtCloseRec.Columns.Add("DeliveryStop_UpdateDate", typeof(DateTime));
                //dtCloseRec.Columns.Add("Receive_Quantity", typeof(decimal));
            }
            else
            {
                #region Nin 08042014
                if (dtCloseRec.Rows.Count > 0)
                {
                    dtCloseRec.Clear();
                }
                int i = 0;

                foreach (GridViewRow gv in gvStkItem.Rows)
                {
                    CheckBox chkClose = (CheckBox)gv.FindControl("chkClose");
                    HiddenField hdRemarkClose = (HiddenField)gv.FindControl("hdRemarkClose");
                    HiddenField hdPOItem_ID = (HiddenField)gv.FindControl("hdPOItem_ID");
                    HiddenField hdCheckFromDB = (HiddenField)gv.FindControl("hdCheckFromDB");

                    DataRow drr = dtCloseRec.NewRow();

                    drr["DeliveryStop_flag"] = chkClose.Checked == true ? "1" : "0";
                    drr["DeliveryStop_Remark"] = hdRemarkClose.Value;
                    drr["POItem_ID"] = hdPOItem_ID.Value;
                    drr["DeliveryStop_FromSave"] = hdCheckFromDB.Value;

                    dtCloseRec.Rows.Add(drr);
                }

                #endregion
            }
          //  gvStk.PageIndexChanging +=new GridViewPageEventHandler(gvStk_PageIndexChanging);
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }


        private void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindSearchData();
        }

        #region [ Button Click ]

        public void BtnCancelClick(object sender, EventArgs e)
        {
            gvStk.DataSource = null;
            gvStk.DataBind();
        }

        public void BtnSearchClick(object sender, EventArgs e)
        {
            PagingControl1.CurrentPageIndex = 1;
            BindSearchData();
        }

        public void BtnCancelClick1(object sender, EventArgs e)
        {
            Response.Redirect("../Stock/StockReceiverForm.aspx");
        }


        public bool isSetStock()
        {
            bool isSet = false;
            foreach(var a in this.ReceiveStkModel.ReceiveStkItemList){
                if(a.Temp_Receive_Quantity > 0){
                    isSet = true;
                }
            }
            return isSet;
        }
        public void BtnSaveClick(object sender, EventArgs e)
        {
            if (rdStockType.SelectedValue == "1")
            {  // รับ ของ

                if (!this.isSetStock())
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('กรุณาทำรายการรับเข้าคลัง');</script>");
                    return;
                }

                if ((dtDeliveryDate.Text.Trim() == "" || txtInvoiceAmount.Text.Trim() == "") && (Session["LotAction"] as string == "New" || Session["LotAction"] as string == "Change")) // รับใหม่หรือ รับอีกครั้งจากยกเลิก
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('กรุณากรอกจำนวนเงินตามใบส่งของและวันที่ส่งของ');</script>");
                    return;
                }
                ReceiveStkModel s =    this.ReceiveStkModel;

                try
                {
                    string rcvStkNo = "";
                    string rcvStkId = "";
                    using (TransactionScope scope = new TransactionScope())
                    {
                        

                      DataTable dt =  new ReceiveStockDAO().GetToReceiveStock(1,DateTime.Now,s.PO_ID,this.txtInvoiceNo.Text.Trim(),this.txtDeliveryDoc.Text.Trim(),Util.ToDateTime(dtInvoiceDate.Text.ToString()),Util.ToDecimal(txtInvoiceAmount.Text), Util.ToDateTime(dtDeliveryDate.Text.Trim())
                          ,s.TradeDiscount_Type,s.TradeDiscount_Percent,s.TradeDiscount_Price,s.Vat_Type,s.Vat,s.VatUnit_Type
                            ,s.Total_Price,s.Total_Discount,s.Total_Before_Vat,s.Vat_Amount,s.Net_Amount,"1",DateTime.Now,this.UserID,DateTime.Now,"","1");
                      rcvStkNo =   dt.Rows[0]["Receive_Stk_No"].ToString();
                      rcvStkId =   dt.Rows[0]["Receive_Stk_Id"].ToString();

                        foreach (var a in this.ReceiveStkModel.ReceiveStkItemList)
                        {
                            // รับ แต่ละ item เข้าคลัง

                            if(a.Temp_Receive_Quantity > 0){
                           
                                a.Receive_StkItem_ID =  new ReceiveStockDAO().GetToReceiveStkItemStk(rcvStkNo, a.Inv_ItemID, "", "", a.Pack_ID, a.Unit_Price, a.Temp_Receive_Quantity
                                    , (s.TradeDiscount_Percent > 0 )? s.TradeDiscount_Percent : a.POItemTradeDiscount_Percent, a.Total_Discount, a.Total_Before_Vat, a.POItem_Vat, a.Vat_Amount, a.Net_Amount
                                    , a.Total_Discount, (a.GiveAway_Quantity > 0) ? "1" : "0"
                                    , "", a.GiveAway_Quantity, 0, 0, "0");

                                new ReceiveStockDAO().GetStkUpdatePoItem(a.Temp_Receive_Quantity, a.POItem_ID);

                                new ReceiveStockDAO().GetToStockOnHand(a.Inv_ItemID, a.Pack_ID, 1,a.Temp_Receive_Quantity + a.GiveAway_Quantity, this.UserID, a.Net_Amount);

                                new ReceiveStockDAO().GetInvItemPackUpdateAvgCost(a.Inv_ItemID,a.Pack_ID);

                                new ReceiveStockDAO().GetUpdateStockMoveMent(1, a.Inv_ItemID, a.Pack_ID, "I", a.Temp_Receive_Quantity + a.GiveAway_Quantity, a.Net_Amount, "R", rcvStkNo, this.UserID, Util.ToInt(rcvStkId));
                            

                                foreach(var b in a.StockLotList){
                                    string stk_lot_id =   new ReceiveStockDAO().GetInvStockLotInsert(a.Receive_StkItem_ID, 1, b.LotNo, b.BarcodeNo, b.BarcodePrintQty, b.LotQty, b.ExpireDate, 1, b.LotQty, this.UserID);

                                    foreach (var c in b.StockLotLocationList)
                                    {
                                        new ReceiveStockDAO().GetInvStockLotLocationInsert(stk_lot_id, c.LocationID,1,int.Parse(rcvStkId), c.Qty_Location, this.UserID);
                                    }
                                }
                           
                            }

                        }

                        // Update PO is Recive Complete
                        new ReceiveStockDAO().GetUpdatePOIsReceiveComplete(s.PO_ID);

                        /////////////////      Create Package    ///////////////////////

                        for (int i = 0; i < gvStkItem.Rows.Count; i++)
                        {

                            CheckBox chkD = (CheckBox)gvStkItem.Rows[i].FindControl("chkPack");

                            if (chkD.Checked && this.ReceiveStkModel.ReceiveStkItemList[i].Temp_Receive_Quantity > 0)
                            {// update pack

                                new ReceiveStockDAO().CreatePackage(this.ReceiveStkModel.ReceiveStkItemList[i].Receive_StkItem_ID, 1, this.UserID);

                                chkD.Enabled = false;

                            }

                            #region Nin 13032014
                            /////////////////      Update CloseStock    ///////////////////////

                            CheckBox chkClose = (CheckBox)gvStkItem.Rows[i].FindControl("chkClose");
                            HiddenField hdRemarkClose = (HiddenField)gvStkItem.Rows[i].FindControl("hdRemarkClose");
                            HiddenField hdPOItem_ID = (HiddenField)gvStkItem.Rows[i].FindControl("hdPOItem_ID");
                            

                            if (chkClose.Checked)
                            {// Update CloseStock

                                new ReceiveStockDAO().UpdateCloseStk(hdPOItem_ID.Value, hdRemarkClose.Value, "1", this.UserID);

                                chkClose.Enabled = false;

                            }

                            #endregion

                        }

                        


                       
                        scope.Complete();
                    }
                        this.ClearReceiveStkModel();   // Clear Session
                        this.pnlDetail.Visible = false;
                        this.BindSearchData();
                        ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('เลขที่รับเข้าคลัง " + rcvStkNo + " ');</script>");
                  
                }
                catch (TransactionAbortedException ex)
                {
                    Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                    Response.Redirect("../Stock/Error.aspx");
                  //  ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('Error " + ex.ToString() + "');</script>");
                }
                catch (Exception ex)
                {
                    Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                    Response.Redirect("../Stock/Error.aspx");
                  //  ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('Error " + ex.ToString() + "');</script>");
                }
           

            }
    

        }


        protected void BtnCancelClick2(object sender, EventArgs e)
        {

            try
            {
                bool DoCancel = false;


                    using (TransactionScope scope = new TransactionScope())
                    {

                           ReceiveStkModel recvModel = this.ReceiveStkModel;


                    for (int i = 0; i < gvStkItem.Rows.Count; i++)
                    {
                        GridView drv = (GridView)gvStkItem;
                        CheckBox chkC = (CheckBox)gvStkItem.Rows[i].FindControl("chkCancelList");
                        var NumRecive = drv.Rows[i].Cells[8].Text;
                        var GiveAway = drv.Rows[i].Cells[11].Text;

                        if (chkC.Checked && recvModel.ReceiveStkItemList[i].Cancel_flag == "0")
                        {
                            bool isCancel = new ReceiveStockDAO().CancelStkItems(recvModel.ReceiveStkItemList[i].Receive_StkItem_ID, 1, this.UserID,Util.ToInt(NumRecive), Util.ToInt(GiveAway),"Y");

                            if (!isCancel)
                            {

                                scope.Dispose();
                                ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('รายการ + " + recvModel.ReceiveStkItemList[i].Inv_ItemName + " ได้มีการจ่ายสินค้าแล้ว');</script>");
                                return;
                            }
                            recvModel.ReceiveStkItemList[i].Cancel_flag = "1";
                            chkC.Enabled = false;
                            DoCancel = true;
                        }

                    }

                    if (DoCancel)
                            {
                                //-------------------  UPDATE STK PRICE  ------------------------


                                recvModel.CalculateUnCancelPrice();  //+++++++++++++    re- calculate price   +++++++
                                new ReceiveStockDAO().UpdateReceiveStkPrice(recvModel.Receive_Stk_ID, recvModel.Total_Price, recvModel.Total_Discount,
                                    recvModel.Total_Before_Vat, recvModel.Vat_Amount, recvModel.Net_Amount);

                                //---------------------------------------------------------------


                                //------------------- Update PO is Recive Complete  -------------

                                new ReceiveStockDAO().GetUpdatePOIsReceiveComplete(recvModel.PO_ID);

                                //---------------------------------------------------------------


                                //------------------- Update Receive STK Status  ----------------

                                new ReceiveStockDAO().GetUpdateReceiveStkStatus(recvModel.Receive_Stk_ID);

                                //---------------------------------------------------------------
                            }
                             
                   
                             scope.Complete();
                        }

                        if(DoCancel){

                            this.ClearReceiveStkModel();  
                            this.pnlDetail.Visible = false;
                            ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('ยกเลิกเรียบร้อย');</script>");
                            this.BindSearchData();
                         
                        }else{
                            
                            ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('ท่านยังไม่ได้เลือกรายการ หรือ ไม่มีรายการที่ให้ท่านยกเลิก');</script>");
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
       
        
        #endregion

        #region [ Binding ]
        //private void BindData_()
        //{


        //    try
        //    {
        //        if (rdStockType.SelectedIndex == 0)
        //        {
        //            DataTable dt = new DataTable();
        //            StockModel t = new StockModel();
        //            t.GetReceiveStkNotCompleteView(PagingControl1.CurrentPageIndex, PagingControl1.PageSize
        //                , txtPOCodeSearch.Text.Trim(), txtPRCodeSearch.Text.Trim()); // ReceiveStockDAO().GetReceiveStkNotComplete(); // ds.Tables[0];

        //            gvStk.DataSource = t.StockView;

        //            //   int i = PagingControl1.PageSize;
        //            PagingControl1.RecordCount = t.RecountCount; // dt.Rows.Count;

        //            gvStk.DataBind();
        //        }
        //        else
        //        {
        //            DataTable dt = new DataTable();
        //            StockModel t = new StockModel();
        //            t.GetReceiveStkCompleteView(PagingControl1.CurrentPageIndex, PagingControl1.PageSize
        //                , txtPOCodeSearch.Text.Trim(), txtPRCodeSearch.Text.Trim());

        //            gvStk.DataSource = t.StockView;

        //            PagingControl1.RecordCount = t.RecountCount;

        //            gvStk.DataBind();

        //        }

        //    }catch(Exception ex){

        //        Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
        //        Response.Redirect("../Stock/Error.aspx");
        //    }

    
        //}



        private void BindSearchData()
        {


            try
            {
                if (rdStockType.SelectedIndex == 0)
                {
                    DataTable dt = new DataTable();
                    StockModel t = new StockModel();


                    
                    t.GetReceiveStkNotCompleteViewSearch(PagingControl1.CurrentPageIndex, PagingControl1.PageSize
                        , txtPOCodeSearch.Text.Trim(), txtPRCodeSearch.Text.Trim(), Util.ToDateTime(poFrom.Text), Util.ToDateTime(poTo.Text), Util.ToDateTime(ccFrom.Text), Util.ToDateTime(ccTo.Text)); 

                    gvStk.DataSource = t.StockView;
                    
                
                    PagingControl1.RecordCount = t.RecountCount;
        
                
                   
                    gvStk.DataBind();
                }
                else
                {
                    DataTable dt = new DataTable();
                    StockModel t = new StockModel();
                    
                    t.GetReceiveStkCompleteViewSearch(PagingControl1.CurrentPageIndex, PagingControl1.PageSize
                        , txtPOCodeSearch.Text.Trim(), txtPRCodeSearch.Text.Trim(), Util.ToDateTime(poFrom.Text), Util.ToDateTime(poTo.Text), Util.ToDateTime(ccFrom.Text),  Util.ToDateTime(ccTo.Text));

                    gvStk.DataSource = t.StockView;


                    PagingControl1.RecordCount = t.RecountCount;



                    gvStk.DataBind();

                }

            }
            catch (Exception ex)
            {

                Session["Error"] = "เกิดข้อผิดพลาด  รายละเอียด   : " + ex.ToString();
                Response.Redirect("../Stock/Error.aspx");
            }


        }

        private void BindDropdown()
        {
            DataView dv = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0].DefaultView;
            dv.RowFilter = "stock_type = '1'";
            this.cbStock.DataSource = dv;
            this.cbStock.DataBind();
            //this.cbStock.Items.Insert(0, new ListItem("เลือกคลังสินค้า", ""));
        }
        #endregion

        #region Nin 08042014
        public DataTable dtCloseRec
        {
            get { return (ViewState["dtCloseRec"] == null) ? null : (DataTable)ViewState["dtCloseRec"]; }
            set { ViewState["dtCloseRec"] = value; }
        }
        #endregion

        protected void gvStk_PageChanging(object sender, GridViewRowEventArgs e)
        {
            BindSearchData();
        }
  
        protected void gvStk_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                LinkButton btnAdd = (LinkButton)e.Row.FindControl("btnAdd");
                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");


                if (drv["Receive_Stk_Status"].ToString().Trim() == "Supplier รับ PO")
                {
                    e.Row.Cells[12].Text = "<span style='color:navy'>Supplier รับ PO</span>";
                    e.Row.Cells[5].Text = "";
                    btnAdd.Visible = false;
                }
                else
                {
                    btnAdd.Visible = true;
                }

                if (drv["Receive_Stk_Status"].ToString().Trim() == "รับแล้ว")
                {
                    e.Row.Cells[12].Text = "<span style='color:red'>รับแล้ว</span>";
                    btnAdd.Visible = false;
               
                }

                if (drv["Receive_Stk_Status"].ToString().Trim() == "รอรับ")
                {
                    e.Row.Cells[12].Text = "<span style='color:navy'>รอรับ</span>";

                }

                if (drv["Receive_Stk_Status"].ToString().Trim() == "ยกเลิก")
                {
                    if (rdStockType.SelectedIndex == 1)
                    {

                     
                        btnAdd.Visible = false;
                    }
                    else
                    {

                        if (drv["IsReceiveComplete"].ToString() != "1")
                        {
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            btnAdd.Visible = false;
                        }
                      //  btnAdd.Visible = true;
                    }
                    e.Row.Cells[12].Text = "<span style='color:dimgray'>ยกเลิก</span>";
                }




                btnDetail.CommandArgument = drv["PO_ID"].ToString() + "," + drv["Receive_Stk_ID"].ToString()
                  + ","  + drv["Receive_Stk_Status"].ToString();


                btnAdd.CommandArgument   = drv["PO_ID"].ToString() + "," + drv["Receive_Stk_ID"].ToString()
                  + ","  + drv["Receive_Stk_Status"].ToString();
              
           


            }
        }

        protected void gvStk_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string[] cmd = e.CommandArgument.ToString().Split(',');
            hdPO_ID.Value = cmd[0];

            if (e.CommandName == "Edi")
            {
                dtCloseRec.Clear();

                if (rdStockType.SelectedValue == "1")
                {
                    string[] ids = e.CommandArgument.ToString().Split(',');
         
                  
                    if (ids[2] == "Supplier รับ PO")
                    {
                        Session["LotAction"] = "New";
                        gvStkItem.Columns[8].Visible = false;
                        gvStkItem.Columns[9].Visible = true;
                      BindReceiveStockFromPO(Util.ToInt(ids[0]));
                    }
                    if (ids[2] == "รอรับ")
                    {
                        Session["LotAction"] = "View";
                        gvStkItem.Columns[8].Visible = true;
                        gvStkItem.Columns[9].Visible = false;
                        BindReceiveStockFromReceiveStk(Util.ToInt(ids[1]));
                    }
                    if (ids[2] == "ยกเลิก")
                    {
                        Session["LotAction"] = "View";
                        BindReceiveStockFromReceiveStk(Util.ToInt(ids[1]));
                    }
                 
             
                }
                else  // รับเข้าคลังแล้ว
                {
                    string[] ids = e.CommandArgument.ToString().Split(',');
                    gvStkItem.Columns[8].Visible = true;
                    gvStkItem.Columns[9].Visible = false;
                    Session["LotAction"] = "View";
                    BindReceiveStockFromReceiveStk(Util.ToInt(ids[1]));
                
                }

            }
            else if (e.CommandName == "Ad")
            {
                  dtCloseRec.Clear();
                  if (rdStockType.SelectedValue == "1")
                  {
                       Session["LotAction"] = "Change";
                       gvStkItem.Columns[8].Visible = false;
                       gvStkItem.Columns[9].Visible = true;
                       string[] ids = e.CommandArgument.ToString().Split(',');
                       BindReceiveStockFromExistPO(Util.ToInt(ids[0]), Util.ToInt(ids[1]));
                     
                  }
            
            }
        }


        private void BindReceiveStockFromPO(int PoId)
        {
            // Get Receive Stock from Stk Model
            ReceiveStkModel rcvModel = new ReceiveStkModel();

            rcvModel.Get(true, PoId);



            this.ReceiveStkModel = rcvModel;
            this.BindReceiveStockForm();
        }

        private void BindReceiveStockFromExistPO(int PoId,int stkId)
        {
            // Get Receive Stock from Stk Model
            ReceiveStkModel rcvModel = new ReceiveStkModel();

            rcvModel.Get(PoId,stkId);



            this.ReceiveStkModel = rcvModel;
            this.BindReceiveStockForm();
        }

        private void BindReceiveStockFromReceiveStk(int rcvStkId)
        {
            // Get Receive Stock from Stk Model
            ReceiveStkModel rcvModel = new ReceiveStkModel();

            rcvModel.Get(false, rcvStkId);

       
            this.ReceiveStkModel = rcvModel;
            this.BindReceiveStockForm();
        }


       
        protected void gvStk_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortGridView(e.SortExpression);
            BindSearchData();
            this.GridViewSort(gvStk);
        }


        private void BindReceiveStockForm()
        {

            ReceiveStkModel rcvModel =   this.ReceiveStkModel;
          
            #region BindReceiveStockHead

            this.txtRecieveStkNo.Text = rcvModel.Receive_Stk_No;
            this.txtNetAmount.Text = rcvModel.Net_Amount.ToString("N2");
            this.txtPOCode.Text = rcvModel.PO_Code;
            this.txtPODate.Text = rcvModel.PO_Date.ToShortDateString();
            this.txtDepName.Text = rcvModel.DeptName;
            this.txtProjectName.Text = rcvModel.ProjectName;
            this.txtRecieveDateTime.Text = rcvModel.Receive_Date.ToString();
            this.txtReason.Text = rcvModel.Reason;
            this.txtSupplierName.Text = rcvModel.SupplierName;
            this.rblVat.SelectedValue = rcvModel.VatUnit_Type;
            this.rblVatType.SelectedValue = rcvModel.Vat_Type;


            this.txtInvoiceNo.Text = rcvModel.Invoice_No;
            this.txtDeliveryDoc.Text = rcvModel.Delivery_Doc;
            this.dtInvoiceDate.Text = rcvModel.Invoice_Date.ToString();
            this.txtInvoiceAmount.Text = rcvModel.Invoice_Amount.ToString();
            this.dtDeliveryDate.Text = rcvModel.Delivery_Date.ToString();

 
            if (rcvModel.Is_PayCheque)
            {
                this.chkPaymentType.SelectedIndex = 0;
                this.txtCreditTerm.Text = rcvModel.CreditTerm_Day;
                this.txtCreditTerm.Enabled = true;
            }
            else if (rcvModel.Is_PayCash)
            {
                this.chkPaymentType.SelectedIndex = 1;
                this.txtCreditTerm.Enabled = false;

            }
            if (rcvModel.TradeDiscount_Type == "1")
            {
                this.chkDealDiscount.Checked = true;
                this.rdTradeDiscountType.SelectedIndex = 1;
            }
            else if (rcvModel.TradeDiscount_Type == "0")
            {
                this.chkDealDiscount.Checked = true;

                this.rdTradeDiscountType.SelectedIndex = 0;
            }
            else
            {
                this.chkDealDiscount.Checked = false;
                this.rdTradeDiscountType.Enabled = false;
            }

            this.txtTradeDiscountPercent.Text = rcvModel.TradeDiscount_Percent.ToString("N2");
            this.txtTradeDiscountAmount.Text = rcvModel.TradeDiscount_Price.ToString("N2");


            if (rcvModel.Vat_Type == "0") // vat รวม
            {
              //  this.chkHasVat.Checked = true;
                this.txtVat.Text = rcvModel.Vat.ToString("N2"); // Convert.ToDecimal(dt.Rows[0]["Vat"]).ToString("N0");
            }
            else // vat แยกหรือ ไม่มี vat
            {
              //  this.chkHasVat.Checked = false;
                this.txtVat.Text = rcvModel.Vat.ToString("N2");
            }

            #endregion

            #region BindReceiveStockItem

            gvStkItem.DataSource = rcvModel.GetStkItemView();//dt;
            gvStkItem.DataBind();

            pnlDetail.Visible = true;

            #endregion
        }

        protected void gvStkItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btn = ((LinkButton)e.Row.FindControl("btnDetail"));

                CheckBox chk = ((CheckBox)e.Row.FindControl("chkPack"));
                CheckBox chkCancel = ((CheckBox)e.Row.FindControl("chkCancelList"));

                CheckBox chkClose = ((CheckBox)e.Row.FindControl("chkClose"));

                ImageButton CommentClose = ((ImageButton)e.Row.FindControl("CommentClose"));
                HiddenField hdRemarkClose = (HiddenField)e.Row.FindControl("hdRemarkClose");
                HiddenField hdPOItem_ID = (HiddenField)e.Row.FindControl("hdPOItem_ID");
                HiddenField hdCheckFromDB = (HiddenField)e.Row.FindControl("hdCheckFromDB");
               

                chk.Enabled = false;
                chkClose.Enabled = false;

                var a =   this.ReceiveStkModel.ReceiveStkItemList.Where(m => m.Inv_ItemID == Util.ToInt(drv["Inv_ItemID"].ToString())
                    && m.Pack_ID == Util.ToInt(drv["Pack_ID"].ToString())).FirstOrDefault();


                if (Util.ToDecimal(drv["POItem_Receive_Quantity"].ToString()) == Util.ToDecimal(drv["Unit_Quantity"].ToString())

                    && Session["LotAction"] as string != "View")
                {
                    btn.Visible = false;
                }
                else
                {
                    btn.Visible = true;
                }


                // check ว่า แตก pack ได้หรือไม่

              bool isPack =  new ReceiveStockDAO().GetInvStockOnHandChkBaseFlag(1,Util.ToInt(drv["Inv_ItemID"].ToString()),Util.ToInt(drv["Pack_ID"].ToString()));
             
                
                if (Util.ToInt(drv["Pack_Id_Base"].ToString()) != Util.ToInt(drv["Pack_ID"].ToString()) && isPack)
                {
                    chk.Enabled = true;
                    chk.Checked = true;
                }
                else
                {
                    chk.Enabled = false;
                }

                if (a.Cancel_flag == "1")
                {
                    chkCancel.Checked = true;
                    chkCancel.Enabled = false;
                }
                else
                {
                    chkCancel.Checked = false;
                    chkCancel.Enabled = true;
                }


                hdPOItem_ID.Value = drv["POItem_ID"].ToString();

                DataTable dt = new DataTable();
                dt.Columns.Add("DeliveryStop_flag", typeof(string));
                dt.Columns.Add("DeliveryStop_Remark", typeof(string));
                //dt.Columns.Add("DeliveryStop_UpdateBy", typeof(int));
                //dt.Columns.Add("DeliveryStop_UpdateDate", typeof(DateTime));
                //dt.Columns.Add("Receive_Quantity", typeof(decimal));
              
                if (dtCloseRec.Rows.Count > 0)
                {
                    DataRow dr = dtCloseRec.Select("POItem_ID = '" + drv["POItem_ID"].ToString() + "'").FirstOrDefault();
                   // DataRow dr = dtCloseRec.Select("POItem_ID = " + drv["POItem_ID"].ToString()).FirstOrDefault();
                    if (dr != null)
                    {
                        DataRow dr_new = dt.NewRow();
                        dr_new["DeliveryStop_flag"] = dr["DeliveryStop_flag"];
                        dr_new["DeliveryStop_Remark"] = dr["DeliveryStop_Remark"];
                        dt.Rows.Add(dr_new);
                    }
                }
                else
                {
                    DataTable dt_result = new ReceiveStockDAO().GetPOItem_Close(drv["POItem_ID"].ToString());
                    if (dt_result.Rows.Count > 0)
                    {
                        DataRow dr_new = dt.NewRow();
                        dr_new["DeliveryStop_flag"] = dt_result.Rows[0]["DeliveryStop_flag"];
                        dr_new["DeliveryStop_Remark"] = dt_result.Rows[0]["DeliveryStop_Remark"];
                        dt.Rows.Add(dr_new);

                        if (dt_result.Rows[0]["DeliveryStop_flag"].ToString() == "1")
                        {
                            hdCheckFromDB.Value = "Y"; 
                        }
                    }
                }
                
                bool close_status = false;

                /*  Check ว่าปิดการรับไปแล้วรึยัง */
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["DeliveryStop_flag"].ToString() == "1")
                    {
                        close_status = true;

                    }
                    else
                    {
                        close_status = false;
                    }

                }


                /*  Check ว่าปิดการรับไปแล้วรึยัง
                    ถ้าปิดการรับแล้ว ไม่สามารถรับเพิ่มได้
                 *  ถ้ายังไม่ปิดการรับสามารถรับเพิ่มได้
                */


                btn.OnClientClick = "open_popup('StockReceiverLotPopup.aspx?Inv_ItemID="
                + drv["Inv_ItemID"].ToString()
                + "&Pack_ID=" + drv["Pack_ID"].ToString()
                + "', 860, 600, 'PRReport', 'yes', 'yes', 'yes'); return false;";

                if (Session["LotAction"].ToString() == "Change")
                {
                    if (dt.Rows[0]["DeliveryStop_flag"].ToString() == "1")
                    {
                        btn.OnClientClick = "alert('มีการปิดการรับแล้ว ไม่สารถรับเพิ่มได้'); return false;";
                    }

                }


                /*  Check rdStockType
                    ถ้าเป็น รอรับเข้าคลัง ให้สามารถ Enable ปิดการรับได้ ถ้า จำนวนที่สั่ง >  ( จำนวนที่รับแล้ว  + จำนวนที่รับ)
                    ถ้าเป็น รับเข้าคลังแล้ว ให้ Disable ปิดการรับ ดูได้อย่างเดียว    
                */
                string bool_Close = "";

                if (rdStockType.SelectedIndex == 0) //รอรับเข้าคลัง
                {
                    /*  Check ว่ามีการปิดรับ ? ถ้าปิดรับแล้ว ไม่สามารถแก้ chkClose ได้ */
                    if (close_status)
                    {
                        chkClose.Checked = true;
                        if (dt.Rows.Count > 0)
                        {
                            hdRemarkClose.Value = dt.Rows[0]["DeliveryStop_Remark"].ToString();
                        }
                        //chkClose.Enabled = false;
                        if (hdCheckFromDB.Value == "Y")
                        {
                            chkClose.Enabled = false;
                        }
                        else
                        {
                            chkClose.Enabled = true;
                        }

                    }
                    else
                    {

                        /*  Check ว่าสามารถปิดการรับ ได้หรือไม่ 
                            ปิดได้ ก็ต่อเมื่อ จำนวนที่สั่ง >  ( จำนวนที่รับแล้ว  + จำนวนที่รับ) */

                        if (Util.ToDecimal(drv["Unit_Quantity"].ToString()) > (Util.ToDecimal(drv["Receive_Quantity"].ToString()) + Util.ToDecimal(drv["Temp_Receive_Quantity"].ToString())))
                        {

                            chkClose.Enabled = true;
                            CommentClose.Enabled = true;
                            bool_Close = "t";
                        }
                        else
                        {
                            chkClose.Enabled = false;
                            CommentClose.Enabled = false;
                            bool_Close = "f";
                            chkClose.Checked = false;
                            hdRemarkClose.Value = "";
                        }
                    }
                }
                else //รับเข้าคลังแล้ว
                {
                    if (dt.Rows.Count > 0)
                    {

                        if (dt.Rows[0]["DeliveryStop_flag"].ToString() == "1")
                        {
                            chkClose.Checked = true;
                        }
                        else
                        {
                            chkClose.Checked = false;
                        }

                        hdRemarkClose.Value = dt.Rows[0]["DeliveryStop_Remark"].ToString();
                    }
                    chkClose.Enabled = false;
                    CommentClose.Enabled = true;
                    bool_Close = "f";
                }
                CommentClose.OnClientClick = "open_popup('pop_Remark.aspx?ctl=" + hdRemarkClose.ClientID
                                          + "&POItem_ID=" + drv["POItem_ID"].ToString() + "&chk=" + bool_Close
                                          + "', 500, 200, 'CloseRec', 'yes', 'yes', 'yes'); return false;";

             
            }
 

        }

        protected void btnRefreshItem_Click(object sender, EventArgs e)
        {
            ReceiveStkModel m =   this.ReceiveStkModel;
            m.CalculatePrice();
            gvStkItem.DataSource = m.GetStkItemView();
            gvStkItem.DataBind();

            txtNetAmount.Text = m.Net_Amount.ToString("N2");
            txtPOCode.Enabled = true;
            btnSave.Enabled = true;

        }

        protected void rdStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //รอรับเข้าคลัง ไม่แสดงปุ่มปิดการรับสินค้า
            if (rdStockType.SelectedIndex == 0)
            {
                btnCloseReceive.Visible = false;
            }
            else
            {
                btnCloseReceive.Visible = true;
            }

            BindSearchData();
            // gvStk.DataSource = null;
            //  gvStk.DataBind();
            pnlDetail.Visible = false;
       
        }

        protected void btnCloseReceiveClick(object sender, EventArgs e)
        {
           
            ScriptManager.RegisterClientScriptBlock
                (   
                    this
                    , this.GetType()
                    , "js1"
                    , @"window.open('Pop_CloseReceiveStk.aspx?id=" + hdPO_ID.Value + "&ref=" + btnRefreshClose.ClientID + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');"
                    , true);
        }

        protected void btnRefreshClose_Click(object sender, EventArgs e)
        {
            BindReceiveStockForm();
        }

    }




}