using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.Stock.Commons;
using System.Diagnostics;
using GPlus.DataAccess;
using GPlus.ModelClass;

namespace GPlus.Stock
{
    public partial class StockReceiverLotPopup : Pagebase
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

        private ReceiveStkItemModel ReceiveStkItemModel
        {
            get
            {
                int itemId = Util.ToInt(Request["Inv_ItemID"].ToString());
                int packId = Util.ToInt(Request["Pack_ID"].ToString());
                return this.ReceiveStkModel.ReceiveStkItemList.Where(m => m.Inv_ItemID == itemId && m.Pack_ID == packId).FirstOrDefault();

            }
        }
      
        
        private StockReceiverLotUserControl _c;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private void BindNewLot()
        {

        }

        private void SetUILotViewOnly()
        {
            this.txtTotalUnit.ReadOnly = true;
            this.btnAddItem.Visible = false;
            this.btAddLot.Visible = false;
            this.btnCancel.Visible = false;
            this.btnDelete.Visible = false;
            this.btnSave.Visible = false;
            this.rdGiveawayType.Enabled = false;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ReceiveStkModel rcvModel = this.ReceiveStkModel; // (ReceiveStkModel)Session["ReceiveStkModel"];
                ReceiveStkItemModel rcvItem = this.ReceiveStkItemModel; //rcvModel.ReceiveStkItemList.Where(m => m.Inv_ItemID == itemId && m.Pack_ID == packId).FirstOrDefault();


                decimal rcvMaxQty = 0; 

               //   SET UI MODE  -------------------  VIEW only OR ADD LOT   ----------------
                if (Session["LotAction"] as string == "View")
                {
                    rcvMaxQty = rcvItem.Receive_Quantity;
                    this.SetUILotViewOnly();
                }
                else
                {
                    rcvMaxQty = (rcvItem.Temp_Receive_Quantity == 0) ? rcvItem.Unit_Quantity- rcvItem.POItem_Receive_Quantity : rcvItem.Temp_Receive_Quantity;  //rcvItem.Unit_Quantity - rcvItem.Receive_Quantity;
                }
               

                // ----------------------   DISPLAY PRICE    ---------------------------------------
                  
                    txtTotalPrice.Text = rcvItem.Unit_Price.ToString("N2");
                    txtTotalUnit.Text = rcvMaxQty.ToString("N0");
                    txtTotalPrice.Text = (rcvItem.Unit_Price * rcvMaxQty).ToString("N2");
                    txtItemCode.Text = rcvItem.Inv_ItemCode;
                    txtItemName.Text = rcvItem.Inv_ItemName;
                    txtUnit.Text = rcvItem.Package_Name;
                    txtFreeItemName.Text = rcvItem.Inv_ItemName;
                    txtUnitPrice.Text = rcvItem.Unit_Price.ToString("N2");
                    ddPackList.Items.Add(rcvItem.Package_Name);
                    string tradePer = (rcvModel.TradeDiscount_Type == "0") ? rcvModel.TradeDiscount_Percent.ToString() : rcvItem.POItemTradeDiscount_Percent.ToString();
                    txtTradeDiscountPercent.Text = (Util.ToDecimal(tradePer) == 0) ? "" : tradePer;

                    InvoiceModel inv = new InvoiceModel()
                    {
                        TradeDiscount_Percent = rcvModel.TradeDiscount_Percent 
                        , TradeDiscount_Price = rcvModel.TradeDiscount_Price  
                        , TradeDiscount_Type = rcvModel.TradeDiscount_Type  
                        , Vat = rcvModel.Vat   
                        , Vat_Type = rcvModel.Vat_Type  
                        , VatUnit_Type = rcvModel.VatUnit_Type  

                    };


                    InvoiceItemModel mo = new InvoiceItemModel();
                    mo.Receive_Quantity = rcvMaxQty; //rcvItem.Unit_Quantity; // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                    mo.TradeDiscount_Percent = rcvItem.POItemTradeDiscount_Percent; // Util.ToDecimal(Request["POItemTradeDiscount_Percent"].ToString());
                    mo.TradeDiscount_Price = rcvItem.POItemTradeDiscount_Price;// Util.ToDecimal(Request["POItemTradeDiscount_Price"].ToString());
                    mo.Unit_Price = rcvItem.Unit_Price; // Util.ToDecimal(Request["Unit_Price"].ToString());
                    mo.Unit_Quantity = rcvItem.Unit_Quantity; // Util.ToDecimal(Request["Unit_Quantity"].ToString());

                    mo.Vat = rcvItem.POItem_Vat; // Util.ToDecimal(Request["PO_Vat"].ToString());

                    inv.InvoiceItem.Add(mo);



                    inv.CalculatePrice();

                    txtVatPrice.Text = inv.VAT_Amount.ToString("N2");
                    txtTotalPrice.Text = inv.Total_Price.ToString("N2");
                    txtIncludeVatPrice.Text = inv.Net_Amount.ToString("N2");
                    txtDiscountPrice.Text = inv.Total_Discount.ToString("N2");
                    txtTotalBeforeVat.Text = inv.Total_Before_Vat.ToString("N2");

                    hdVat.Value = (rcvModel.Vat_Type == "0") ? rcvModel.Vat.ToString() : rcvItem.POItem_Vat.ToString();
                    hdTradeDiscount_Type.Value = rcvModel.TradeDiscount_Type;
                    hdTotalUnit.Value = rcvMaxQty.ToString();
                    hdTradeDiscountPercent.Value = (rcvModel.TradeDiscount_Type == "0") ? rcvModel.TradeDiscount_Percent.ToString() : rcvItem.POItemTradeDiscount_Percent.ToString();
                    hdTradeDiscountPrice.Value = (rcvModel.TradeDiscount_Type == "0") ? rcvModel.TradeDiscount_Price.ToString() : rcvItem.POItemTradeDiscount_Price.ToString();
                    hdTotalPriceSum.Value = inv.Total_Price.ToString();
                    hdVatUnit_Type.Value = rcvModel.VatUnit_Type;
                    this.ReceiveStkModel = rcvModel;

                    ////////////////////////////////////////////////


                //-------------------------  DISPLAY GIVE AWAY  -----------------------------------


                    if (rcvItem.GiveAway_Quantity > 0)
                    {
                        rdGiveawayType.SelectedIndex = 1;
                        txtFreeItemTotalUnit.Text = rcvItem.GiveAway_Quantity.ToString("N0");
                        giveAwayItemNamePanel.Visible = true;
                    }
                    else
                    {
                        rdGiveawayType.SelectedIndex = 0;
                        giveAwayItemNamePanel.Visible = false;
                    }


                //-------------------------  DISPLAY LOT  -----------------------------------------



                    rcvItem.GetLot();
                    if(rcvItem.StockLotList.Count == 0){
                        rcvItem.CreateLot();
                    }
                    dlLot.DataSource = rcvItem.GetLotView();
                    dlLot.DataBind();

               

                    for (int i = 0; i < dlLot.Items.Count; i++)
                    {
                  
                        StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                        slu.ReceiveNumber = rcvItem.StockLotList[i].LotQty.ToString();
                        slu.LotNo = rcvItem.StockLotList[i].LotNo;
                        slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
                        slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
                        slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
                        slu.LotID = rcvItem.StockLotList[i].LotID.ToString();



                        slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
                        slu.gvStk.DataBind();
                        int ii = 0;
                        foreach(GridViewRow gr in slu.gvStk.Rows){
                            TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                            DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                            ddl.SelectedValue = rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID.ToString();
                            tb.Text = rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location.ToString();

                            if ((Session["LotAction"] as string == "New" || Session["LotAction"] as string == "Change") && rcvItem.Temp_Receive_Quantity == 0 && ii == 0)  // automatic add lot value for create new location
                            {
                                tb.Text = rcvItem.StockLotList[i].LotQty.ToString();
                            }
     

                            ii++;
                        }

                       
                    }

                ///////////////////////////////////////////////
            }
         
        }




        //protected void BtnAddLotClick(object sender, EventArgs e)
        //{


        //    btnDelete.Enabled = true;
        
        //    ReceiveStkItemModel rcvItem =   this.ReceiveStkItemModel; 
     
        //    rcvItem.AddLot();

        //    //----------------  DISPLAY LOT ------------------------------

        //    dlLot.DataSource = rcvItem.GetLotView();
        //    dlLot.DataBind();

        //    for (int i = 0; i < dlLot.Items.Count; i++)
        //    {
        //        StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
        //        slu.ReceiveNumber = rcvItem.StockLotList[i].LotQty.ToString();
        //        slu.LotNo = rcvItem.StockLotList[i].LotNo;
        //        slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
        //        slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
        //        slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
        //        slu.LotID = rcvItem.StockLotList[i].LotID.ToString();
        //        slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
        //        slu.gvStk.DataBind();
        //    }
      


        //}


        protected void RdGiveawayTypeSelectedIndexChanged(object sender, EventArgs e)
        {

            if (rdGiveawayType.SelectedValue == "1")
            {
                    this.giveAwayItemNamePanel.Visible = false;
                    this.lotPanel.Visible = true;
                    this.multiGiveAwayPanel.Visible = false;
                  //  this.multiLotPanel.Visible = false;

                    gdStrkItem.Visible = true;
               // ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('1');</script>");

            }
            else
            {

                    this.giveAwayItemNamePanel.Visible = true;
                    this.lotPanel.Visible = true;
                   this.multiGiveAwayPanel.Visible = false;
                  //  this.multiLotPanel.Visible = true;

                    btAddLot.Visible = true;


                   btAddLot.Visible = true;


             //   ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('2');</script>");

            }
          
        }


        protected void BtnSaveClick(object sender, EventArgs e)
        {

          ExcecutionResult exe =  GetLotData();


            // success
          if (exe.result)
          {

           //  ReceiveStkItemModel m = this.ReceiveStkItemModel;

                 string scriptStr = "if(window.opener)window.opener.document.getElementById('ContentPlaceHolder1_btnRefreshItem').click(); window.close();";
                  ScriptManager.RegisterStartupScript(this, this.GetType(), "closing", scriptStr, true);
      
             // ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('success');</script>");

          }
          else
          {


              ClientScript.RegisterStartupScript(this.GetType(), "onclick", "<script>alert('"+ exe.message+"');</script>");

          }


          
        }

   

      
        protected void BtnAddItemClick(object sender, EventArgs e)
        {
          //  AddNewGiveAway();
        }



   
        public void GdStrkItemRowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }

        public void GdStrkItemRowCommand(object sender, GridViewCommandEventArgs e)
        {

           
        }


       

        protected void dlLot_ItemDataBound(object sender, DataListItemEventArgs e)
        {
        
        }

        private bool chkDup(List<string> chk, string val){
           foreach(var a in chk){
               if(a == val){
                   return true;
               }
           }
           return false;
        }

        private ExcecutionResult GetLotData()
        {

            int lotNumAll = 0;
            int getLot = 0;
            int freeUnit = 0;
            int totalUnit = 0;
            if (txtFreeItemTotalUnit.Text.Trim() != "")
            {
                if (!(int.TryParse(txtFreeItemTotalUnit.Text.Trim().Replace(",", ""), out freeUnit)))
                {
                    return new ExcecutionResult()
                    {
                        result = false,
                        message = "ท่านกรอกจำนวนของแถมไม่ถูกต้อง"
                    };
                }
            }

            if (txtTotalUnit.Text.Trim() != "")
            {
                if (!(int.TryParse(txtTotalUnit.Text.Trim().Replace(",",""), out totalUnit)))
                {

                    return new ExcecutionResult()
                    {
                        result = false,
                        message = "ท่านกรอกจำนวนรับไม่ถูกต้อง"
                    };
                }
                else
                {
                    if(totalUnit > this.ReceiveStkItemModel.Unit_Quantity){

                        return new ExcecutionResult()
                        {
                            result = false,
                            message = "ท่านกรอกจำนวนรับไม่ถูกต้อง (เกินจาก PO)"
                        };
                    }
                }
            }
          
            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");

                if (int.TryParse(slu.ReceiveNumber, out getLot))
                {
                    lotNumAll += Util.ToInt(slu.ReceiveNumber);
                }
                else
                {
                    return new ExcecutionResult()
                    {
                        message = "ท่านกรอกจำนวน Lot ไม่ถุกต้อง"
                      , result = false
                    };
                }


                int locAcc = 0;

                //----------  Check Quantity of Lot Location --------------------------------

                GridView gvStk = (GridView)slu.FindControl("gvStk");
                foreach (GridViewRow gf in gvStk.Rows)
                {
                    DropDownList ddl = gf.FindControl("ddLocationList") as DropDownList;


                    TextBox txt = (TextBox)gf.FindControl("txtEachUnitNumber");
                    
                    int locQtyItem =0;
                    
                    if(int.TryParse(txt.Text, out locQtyItem))
                    {
                        locAcc += locQtyItem;


                    }else{
                        return new ExcecutionResult()
                        {
                            message = "ท่านกรอกจำนวน ใน Location ไม่ถุกต้อง"
                            ,
                            result = false
                        };
                    }

                }

                if(locAcc != getLot ){
                    return new ExcecutionResult()
                    {
                        message = "ท่านกรอกจำนวน ใน Location ไม่ถุกต้อง"
                        ,
                        result = false
                    };
                }
                //---------------------------------------------------------------------------
          
            }



            if(rdGiveawayType.SelectedIndex ==1 && freeUnit == 0){
                return new ExcecutionResult()
                {
                    result = false,
                    message = "ท่านกรอกจำนวนจำนวนของแถมไม่ถูกต้อง"
                };
            }

            if (lotNumAll != freeUnit + totalUnit)
            {

                return new ExcecutionResult()
                {
                      result = false,
                       message = "ท่านกรอกจำนวนรับไม่ถูกต้อง"
                };
              
            }
            else
            {
                //  get lot qty
                for (int i = 0; i < dlLot.Items.Count; i++)
                {
                       StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                
                      
                       this.ReceiveStkItemModel.StockLotList[i].LotQty = Util.ToInt(slu.ReceiveNumber);
                       this.ReceiveStkItemModel.StockLotList[i].LotNo = slu.LotNo;
                       this.ReceiveStkItemModel.StockLotList[i].BarcodeNo = slu.SupplierBarCode;
                       this.ReceiveStkItemModel.StockLotList[i].BarcodePrintQty = Util.ToInt(slu.PrintCounnt);
                       this.ReceiveStkItemModel.StockLotList[i].ExpireDate = Util.ToDateTime(slu.ExpireDate);


                    //------------------------       --------------------------------------
                       List<string> locationLst = new List<string>();
                       GridView gvStk = (GridView)slu.FindControl("gvStk");
                       int ii = 0;
                       foreach (GridViewRow gf in gvStk.Rows)
                       {

                               DropDownList ddl = gf.FindControl("ddLocationList") as DropDownList;

                               TextBox txt = (TextBox)gf.FindControl("txtEachUnitNumber");

                               if (chkDup(locationLst, ddl.SelectedItem.Value))
                               {
                                   return new ExcecutionResult()
                                   {
                                       result = false,
                                       message = "ท่านกรอก Location ไม่ถูกต้อง"
                                   };
                               }
                               this.ReceiveStkItemModel.StockLotList[i].StockLotLocationList[ii].Qty_Location = int.Parse(txt.Text.Trim());
                               this.ReceiveStkItemModel.StockLotList[i].StockLotLocationList[ii].Qty_Out = 0;
                               this.ReceiveStkItemModel.StockLotList[i].StockLotLocationList[ii].LocationID = int.Parse(ddl.SelectedItem.Value);
                               ii++;
                               locationLst.Add(ddl.SelectedItem.Value);
                       }

                    //----------------------------------------------------------------------
                   
                }
                //  get give away

                this.ReceiveStkItemModel.GiveAway_Quantity = freeUnit;
                this.ReceiveStkItemModel.Temp_Receive_Quantity = totalUnit;
                
                return new ExcecutionResult()
                {
                    result = true,
                    message = ""
                };
             
            }

      
        }

        protected void BtnDeleteLotClick(object sender, EventArgs e)
        {
            // store state for old state
            ReceiveStkItemModel rcvItem = this.ReceiveStkItemModel;
            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                rcvItem.StockLotList[i].LotQty = Util.ToInt(slu.ReceiveNumber);
                rcvItem.StockLotList[i].LotNo = slu.LotNo;
                rcvItem.StockLotList[i].BarcodeNo = slu.SupplierBarCode;
                rcvItem.StockLotList[i].BarcodePrintQty = Util.ToInt(slu.PrintCounnt);
                rcvItem.StockLotList[i].ExpireDate = (slu.ExpireDate == "") ? null : Util.ToDateTime(slu.ExpireDate);
                rcvItem.StockLotList[i].LotID = int.Parse(slu.LotID);
               

                int ii = 0;
                foreach (GridViewRow gr in slu.gvStk.Rows)
                {
                    TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                    DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                    rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID = Util.ToInt(ddl.SelectedValue);
                    rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location = Util.ToInt(tb.Text);

                    ii++;
                }
            }


            int count = rcvItem.GetLotView().Rows.Count;
            if (count > 1)
            {

                rcvItem.DeleteLot();
  

                if (!(count > 1)) btnDelete.Enabled = false;


                dlLot.DataSource = rcvItem.GetLotView();
                dlLot.DataBind();
            

               
                for (int i = 0; i < rcvItem.GetLotView().Rows.Count; i++)
                {
                    StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                    slu.ReceiveNumber = rcvItem.StockLotList[i].LotQty.ToString();
                    slu.LotNo = rcvItem.StockLotList[i].LotNo;
                    slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
                    slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
                    slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
                    slu.LotID = rcvItem.StockLotList[i].LotID.ToString();

                    slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
                    slu.gvStk.DataBind();

                    int ii = 0;
                    foreach (GridViewRow gr in slu.gvStk.Rows)
                    {
                        TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                        DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                        ddl.SelectedValue = rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID.ToString();
                        tb.Text = rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location.ToString();


                        ii++;
                    }

                  
                }
            }
            else
            {
                btnDelete.Enabled = false;
            }  

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }


        protected void BtnAddLotClick(object sender, EventArgs e)
        {
            btnDelete.Enabled = true;

            ReceiveStkItemModel rcvItem = this.ReceiveStkItemModel;
            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                rcvItem.StockLotList[i].LotQty = Util.ToInt(slu.ReceiveNumber);
                rcvItem.StockLotList[i].LotNo = slu.LotNo;
                rcvItem.StockLotList[i].BarcodeNo = slu.SupplierBarCode;
                rcvItem.StockLotList[i].BarcodePrintQty = Util.ToInt(slu.PrintCounnt);
                rcvItem.StockLotList[i].ExpireDate = (slu.ExpireDate == "") ? null : Util.ToDateTime(slu.ExpireDate);
                rcvItem.StockLotList[i].LotID = int.Parse(slu.LotID);


                int ii = 0;
                foreach (GridViewRow gr in slu.gvStk.Rows)
                {
                    TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                    DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                    rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID = Util.ToInt(ddl.SelectedValue);
                    rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location = Util.ToInt(tb.Text);

                    ii++;
                }
            }

            rcvItem.AddLot();

            //----------------  DISPLAY LOT ------------------------------

            dlLot.DataSource = rcvItem.GetLotView();
            dlLot.DataBind();

            for (int i = 0; i < dlLot.Items.Count; i++)
            {
                StockReceiverLotUserControl slu = (StockReceiverLotUserControl)dlLot.Items[i].FindControl("lucLot");
                slu.ReceiveNumber = rcvItem.StockLotList[i].LotQty.ToString();
                slu.LotNo = rcvItem.StockLotList[i].LotNo;
                slu.SupplierBarCode = rcvItem.StockLotList[i].BarcodeNo;
                slu.PrintCounnt = rcvItem.StockLotList[i].BarcodePrintQty.ToString();
                slu.ExpireDate = (rcvItem.StockLotList[i].ExpireDate == null) ? "" : rcvItem.StockLotList[i].ExpireDate.ToString();
                slu.LotID = rcvItem.StockLotList[i].LotID.ToString();
                slu.gvStk.DataSource = rcvItem.StockLotList[i].GetLotLocationView();
                slu.gvStk.DataBind();
                int ii = 0;
                foreach (GridViewRow gr in slu.gvStk.Rows)
                {
                    TextBox tb = gr.FindControl("txtEachUnitNumber") as TextBox;
                    DropDownList ddl = gr.FindControl("ddLocationList") as DropDownList;
                    ddl.SelectedValue = rcvItem.StockLotList[i].StockLotLocationList[ii].LocationID.ToString();
                    tb.Text = rcvItem.StockLotList[i].StockLotLocationList[ii].Qty_Location.ToString();

                    ii++;
                }
            }
        }

   
     

    }
}