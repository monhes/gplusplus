using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using GPlus.DataAccess;

namespace GPlus.ModelClass
{
    public class ReceiveStkModel
    {
        public int Stock_ID
        {
            get;
            set;
        }
        public int Receive_Stk_ID
        {
            get;
            set;
        }



    
        public string Receive_Stk_No
        {
            get;
            set;
        }

        public DateTime? Receive_Date
        {
            get;
            set;
        }
        public string PO_Code
        {
            get;
            set;
        }
        public DateTime PO_Date
        {
            get;
            set;
        }
        public int PO_ID
        {
            get;
            set;
        }
        public string DeptName
        {
            get;
            set;
        }
        public string SupplierName
        {
            get;
            set;
        }
        public string ProjectName
        {
            get;
            set;
        }

        public string Reason
        {
            get;
            set;
        }

        public string VatUnit_Type
        {
            get;
            set;
        }
        public bool Is_PayCheque
        {
            get;
            set;
        }

        public bool Is_PayCash
        {
            get;
            set;
        }

        public string CreditTerm_Day
        {
            get;
            set;
        }
        public string TradeDiscount_Type
        {
            get;
            set;
        }
        public decimal TradeDiscount_Percent
        {
            get;
            set;
        }
        public decimal TradeDiscount_Price
        {
            get;
            set;
        }
        public decimal Vat
        {
            get;
            set;
        }
        public string Vat_Type
        {
            get;
            set;
        }
        public decimal Net_Amount
        {
            get;
            set;
        }
        public decimal Total_Price
        {
            get;
            set;
        }
        public decimal Total_Discount
        {
            get;
            set;
        }
        public decimal PONet_Amount
        {
            get;
            set;

        }

        public decimal Total_Before_Vat
        {
            get;
            set;
        }

        public decimal Vat_Amount
        {
            get;
            set;
        }


        public string Invoice_No
        {
            get;
            set;
        }
        public string Delivery_Doc
        {
            get;
            set;
        }
        public DateTime? Invoice_Date
        {
            get;
            set;
        }
        public DateTime? Delivery_Date
        {
            get;
            set;
        }
        public decimal Invoice_Amount
        {
            get;
            set;
        }
      
        public List<ReceiveStkItemModel> ReceiveStkItemList
        {
            get;
            set;
        }

        public ReceiveStkModel()
        {
            this.ReceiveStkItemList = new List<ReceiveStkItemModel>();
            this.Stock_ID = 1;
        }

        public ReceiveStkModel(int Stock_ID)
        {
            this.ReceiveStkItemList = new List<ReceiveStkItemModel>();
            this.Stock_ID = Stock_ID;
        }

        public DataTable GetStkItemView()
        {

            DataTable dtt = new DataTable();
            dtt.Columns.Add("Inv_ItemCode", typeof(string));
            dtt.Columns.Add("Inv_ItemID", typeof(int));
            dtt.Columns.Add("Inv_ItemName", typeof(string));
            dtt.Columns.Add("Pack_ID", typeof(int));
            dtt.Columns.Add("POItem_Vat", typeof(decimal));
            dtt.Columns.Add("POItem_TradeDiscount_Percent", typeof(decimal));

            

            dtt.Columns.Add("POItem_TradeDiscount_Price", typeof(decimal));
            dtt.Columns.Add("Package_Name", typeof(string));
            dtt.Columns.Add("Unit_Price", typeof(decimal));
            dtt.Columns.Add("Unit_Quantity", typeof(decimal));
            dtt.Columns.Add("Receive_Quantity", typeof(decimal));
                  
            dtt.Columns.Add("Temp_Receive_Quantity", typeof(decimal));
            dtt.Columns.Add("Total_Before_Vat", typeof(decimal));
            dtt.Columns.Add("Vat_Amount", typeof(decimal));


            dtt.Columns.Add("Net_Amount", typeof(decimal));
            dtt.Columns.Add("GiveAway_Unit", typeof(decimal));
            dtt.Columns.Add("rownumber", typeof(int));

            dtt.Columns.Add("ItemDiscount_Price", typeof(decimal));

            dtt.Columns.Add("POItem_ID", typeof(int));

            dtt.Columns.Add("Pack_Id_Base", typeof(int));

            dtt.Columns.Add("Cancel_flag", typeof(string));

            dtt.Columns.Add("POItem_Receive_Quantity", typeof(decimal)); 
       
            foreach (var a in this.ReceiveStkItemList)
            {

                DataRow drr = dtt.NewRow();

                drr["Cancel_flag"] = a.Cancel_flag;
                drr["Inv_ItemCode"] = a.Inv_ItemCode;
                drr["Inv_ItemID"] = a.Inv_ItemID;
                drr["Inv_ItemName"] = a.Inv_ItemName;
                drr["Pack_ID"] = a.Pack_ID;
                drr["POItem_Vat"] = a.POItem_Vat;
                drr["POItem_TradeDiscount_Percent"] = a.POItemTradeDiscount_Percent;
                drr["POItem_TradeDiscount_Price"] = a.POItemTradeDiscount_Price;
                drr["Package_Name"] = a.Package_Name;
                drr["Unit_Price"] = a.Unit_Price;
                drr["Unit_Quantity"] = a.Unit_Quantity;
                drr["Receive_Quantity"] = a.Receive_Quantity;
  
                drr["Temp_Receive_Quantity"] = a.Temp_Receive_Quantity;
                drr["GiveAway_Unit"] = a.GiveAway_Quantity;

                drr["ItemDiscount_Price"] = a.Total_Discount;

                drr["Net_Amount"] = a.Net_Amount; // a.Vat_Amount; //a.Total_Before_Vat;
                drr["Vat_Amount"] = a.Vat_Amount; //a.Total_Before_Vat;
                drr["Total_Before_Vat"] = a.Total_Before_Vat;
                drr["rownumber"] = a.rownumber;
                drr["POItem_ID"] = a.POItem_ID;

                drr["Pack_Id_Base"] = a.Pack_Id_Base;
                drr["POItem_Receive_Quantity"] = a.POItem_Receive_Quantity;
             
           
                dtt.Rows.Add(drr);

            }

            return dtt;
        }
       
      
     
        public void Get(bool isFromPO, int Id)
        {
            try
            {
                DataTable dt = new DataTable();
                if (isFromPO)  // true is get from po
                {
                    dt = new ReceiveStockDAO().GetReceiveStkFormPOByPOId(Id);
                }
                else   // false is get from stk
                {
                    dt = new ReceiveStockDAO().GetReceiveStkFromStkByStkID(Id);
                }




                if (dt.Rows.Count > 0)
                {
                    /////////

                    this.Invoice_No = dt.Rows[0]["Invoice_No"].ToString();
                    this.Delivery_Doc = dt.Rows[0]["Delivery_Doc"].ToString();
                    this.Invoice_Amount = Util.ToDecimal(dt.Rows[0]["Invoice_Amount"].ToString());
                    this.Invoice_Date = Util.ToDateTime(dt.Rows[0]["Invoice_Date"].ToString());
                    this.Delivery_Date = Util.ToDateTime(dt.Rows[0]["Delivery_Date"].ToString());
                  
                    /////////
                    this.PO_Code = dt.Rows[0]["PO_Code"].ToString();
                    this.Receive_Stk_ID = Util.ToInt(dt.Rows[0]["Receive_Stk_ID"].ToString());
                    this.Receive_Stk_No = dt.Rows[0]["Receive_Stk_No"].ToString();
                    this.PO_ID = Util.ToInt(dt.Rows[0]["PO_ID"].ToString());
                    this.PONet_Amount = Util.ToDecimal(dt.Rows[0]["PO_Net_Amount"].ToString());
                    this.PO_Date = DateTime.Parse(dt.Rows[0]["PO_Date"].ToString());
                    this.DeptName = dt.Rows[0]["Description"].ToString();
                    this.SupplierName = dt.Rows[0]["Supplier_Name"].ToString();
                    this.ProjectName = dt.Rows[0]["Project_Name"].ToString();
                    this.Reason = dt.Rows[0]["Reason"].ToString();
                    this.VatUnit_Type = dt.Rows[0]["VatUnit_Type"].ToString();
                    this.CreditTerm_Day = dt.Rows[0]["CreditTerm_Day"].ToString();
                    this.Is_PayCheque = (dt.Rows[0]["Is_PayCheque"].ToString().Trim() == "1") ? true : false;
                    this.Is_PayCash = (dt.Rows[0]["Is_PayCash"].ToString().Trim() == "1") ? true : false;
                    this.TradeDiscount_Type = dt.Rows[0]["TradeDiscount_Type"].ToString().Trim();
                    this.TradeDiscount_Percent = Util.ToDecimal(dt.Rows[0]["TradeDiscount_PercentPO"].ToString());
                    this.TradeDiscount_Price = Util.ToDecimal(dt.Rows[0]["TradeDiscount_PricePO"].ToString());
                    this.Vat = Util.ToDecimal(dt.Rows[0]["PO_Vat"].ToString());
                    this.Vat_Type = dt.Rows[0]["Vat_Type"].ToString().Trim();
                    this.Net_Amount = Util.ToDecimal(dt.Rows[0]["Sum_Net_Amount"].ToString().Trim());


                }



                foreach (DataRow r in dt.Rows)
                {
             
                    this.ReceiveStkItemList.Add(new ReceiveStkItemModel()
                    {
                        Inv_ItemCode = r["Inv_ItemCode"].ToString()
                       ,
                        Inv_ItemID = Util.ToInt(r["Inv_ItemID"].ToString())
                       ,
                        Inv_ItemName = r["Inv_ItemName"].ToString()
                       ,
                        Pack_ID = Util.ToInt(r["Pack_ID"].ToString())
                       ,
                        POItem_Vat = Util.ToDecimal(r["POItem_Vat"].ToString())
                       ,
                        POItemTradeDiscount_Percent = Util.ToDecimal(r["POItem_TradeDiscount_Percent"].ToString())
                       ,
                        POItemTradeDiscount_Price = Util.ToDecimal(r["POItem_TradeDiscount_Price"].ToString())
                       ,
                        Receive_Quantity = Util.ToDecimal(r["Receive_Quantity"].ToString())
                       ,
                        Package_Name = r["Package_Name"].ToString()
                       ,
                        Unit_Price = Util.ToDecimal(r["Unit_Price"].ToString())
                       ,
                        Unit_Quantity = Util.ToDecimal(r["Unit_Quantity"].ToString())
                       ,
                        Temp_Receive_Quantity = 0
                       ,
                        rownumber = Util.ToInt(r["rownumber"].ToString())
                       ,
                        POItem_ID = Util.ToInt(r["POItem_ID"].ToString())
                       ,
                        Pack_Id_Base = Util.ToInt(r["Pack_Id_Base"].ToString())
                       ,
                        Receive_StkItem_ID = Util.ToInt(r["Receive_StkItem_ID"].ToString())
                       ,
                        GiveAway_Quantity = Util.ToDecimal(r["GiveAway_Unit"].ToString())
                       ,
                        Total_Before_Vat = Util.ToDecimal(r["Total_Before_Vat"].ToString())
                       ,
                        Vat_Amount = Util.ToDecimal(r["Vat_Amount"].ToString())
                       ,
                        Total_Discount = Util.ToDecimal(r["ItemDiscount_Price"].ToString())
                       ,
                        Cancel_flag = (r["Cancel_flag"].ToString() == "") ? "0" : r["Cancel_flag"].ToString()
                       ,
                        Net_Amount = Util.ToDecimal(r["Net_Amount"].ToString())
                       ,
                        POItem_Receive_Quantity = Util.ToDecimal(r["POItem_Receive_Quantity"].ToString())

                    });


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Get(int PoID, int stkId)
        {
            // GetReceiveStkFormExistPOByPOId

            try
            {
                DataTable dt = new DataTable();
           
                dt = new ReceiveStockDAO().GetReceiveStkFormExistPOByPOId(PoID,stkId);


                if (dt.Rows.Count > 0)
                {
                    /////////

                    this.Invoice_No = dt.Rows[0]["Invoice_No"].ToString();
                    this.Delivery_Doc = dt.Rows[0]["Delivery_Doc"].ToString();
                    this.Invoice_Amount = Util.ToDecimal(dt.Rows[0]["Invoice_Amount"].ToString());
                    this.Invoice_Date = Util.ToDateTime(dt.Rows[0]["Invoice_Date"].ToString());
                    this.Delivery_Date = Util.ToDateTime(dt.Rows[0]["Delivery_Date"].ToString());

                    /////////
                    this.PO_Code = dt.Rows[0]["PO_Code"].ToString();
                    this.Receive_Stk_ID = Util.ToInt(dt.Rows[0]["Receive_Stk_ID"].ToString());
                    this.Receive_Stk_No = dt.Rows[0]["Receive_Stk_No"].ToString();
                    this.PO_ID = Util.ToInt(dt.Rows[0]["PO_ID"].ToString());
                    this.PONet_Amount = Util.ToDecimal(dt.Rows[0]["PO_Net_Amount"].ToString());
                    this.PO_Date = DateTime.Parse(dt.Rows[0]["PO_Date"].ToString());
                    this.DeptName = dt.Rows[0]["Description"].ToString();
                    this.SupplierName = dt.Rows[0]["Supplier_Name"].ToString();
                    this.ProjectName = dt.Rows[0]["Project_Name"].ToString();
                    this.Reason = dt.Rows[0]["Reason"].ToString();
                    this.VatUnit_Type = dt.Rows[0]["VatUnit_Type"].ToString();
                    this.CreditTerm_Day = dt.Rows[0]["CreditTerm_Day"].ToString();
                    this.Is_PayCheque = (dt.Rows[0]["Is_PayCheque"].ToString().Trim() == "1") ? true : false;
                    this.Is_PayCash = (dt.Rows[0]["Is_PayCash"].ToString().Trim() == "1") ? true : false;
                    this.TradeDiscount_Type = dt.Rows[0]["TradeDiscount_Type"].ToString().Trim();
                    this.TradeDiscount_Percent = Util.ToDecimal(dt.Rows[0]["TradeDiscount_PercentPO"].ToString());
                    this.TradeDiscount_Price = Util.ToDecimal(dt.Rows[0]["TradeDiscount_PricePO"].ToString());
                    this.Vat = Util.ToDecimal(dt.Rows[0]["PO_Vat"].ToString());
                    this.Vat_Type = dt.Rows[0]["Vat_Type"].ToString().Trim();
                    this.Net_Amount = Util.ToDecimal(dt.Rows[0]["Sum_Net_Amount"].ToString().Trim());


                }



                foreach (DataRow r in dt.Rows)
                {

                    this.ReceiveStkItemList.Add(new ReceiveStkItemModel()
                    {
                        Inv_ItemCode = r["Inv_ItemCode"].ToString()
                       ,
                        Inv_ItemID = Util.ToInt(r["Inv_ItemID"].ToString())
                       ,
                        Inv_ItemName = r["Inv_ItemName"].ToString()
                       ,
                        Pack_ID = Util.ToInt(r["Pack_ID"].ToString())
                       ,
                        POItem_Vat = Util.ToDecimal(r["POItem_Vat"].ToString())
                       ,
                        POItemTradeDiscount_Percent = Util.ToDecimal(r["POItem_TradeDiscount_Percent"].ToString())
                       ,
                        POItemTradeDiscount_Price = Util.ToDecimal(r["POItem_TradeDiscount_Price"].ToString())
                       ,
                        Receive_Quantity = Util.ToDecimal(r["Receive_Quantity"].ToString())
                       ,
                        Package_Name = r["Package_Name"].ToString()
                       ,
                        Unit_Price = Util.ToDecimal(r["Unit_Price"].ToString())
                       ,
                        Unit_Quantity = Util.ToDecimal(r["Unit_Quantity"].ToString())
                       ,
                        Temp_Receive_Quantity = 0
                       ,
                        rownumber = Util.ToInt(r["rownumber"].ToString())
                       ,
                        POItem_ID = Util.ToInt(r["POItem_ID"].ToString())
                       ,
                        Pack_Id_Base = Util.ToInt(r["Pack_Id_Base"].ToString())
                       ,
                        Receive_StkItem_ID = Util.ToInt(r["Receive_StkItem_ID"].ToString())
                       ,
                        GiveAway_Quantity = Util.ToDecimal(r["GiveAway_Unit"].ToString())
                       ,
                        Total_Before_Vat = Util.ToDecimal(r["Total_Before_Vat"].ToString())
                       ,
                        Vat_Amount = Util.ToDecimal(r["Vat_Amount"].ToString())
                       ,
                        Total_Discount = Util.ToDecimal(r["ItemDiscount_Price"].ToString())
                       ,
                        Cancel_flag = (r["Cancel_flag"].ToString() == "") ? "0" : r["Cancel_flag"].ToString()
                       ,
                        Net_Amount = Util.ToDecimal(r["Net_Amount"].ToString())
                       ,
                        POItem_Receive_Quantity = Util.ToDecimal(r["POItem_Receive_Quantity"].ToString())

                    });


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      

        public bool isReceiveCompletePO()
        {
            
            bool isComplete = true;

            if(this.ReceiveStkItemList.Count == 0){
                isComplete = false;
            }
            foreach(var a in this.ReceiveStkItemList){
                if(a.Temp_Receive_Quantity + a.Receive_Quantity < a.Unit_Quantity){
                    isComplete = false;
                }
            }
            return isComplete;
        }

        public void CalculateUnCancelPrice()
        {
            InvoiceModel inv = new InvoiceModel()
            {
                TradeDiscount_Percent = this.TradeDiscount_Percent // Util.ToDecimal( Request["POTradeDiscount_Percent"].ToString())
              ,
                TradeDiscount_Price = this.TradeDiscount_Price // Util.ToDecimal(Request["POTradeDiscount_Price"].ToString())
              ,
                TradeDiscount_Type = this.TradeDiscount_Type // Request["TradeDiscount_Type"].ToString()
              ,
                Vat = this.Vat  // Util.ToDecimal(Request["PO_Vat"].ToString())
              ,
                Vat_Type = this.Vat_Type //  Request["Vat_Type"].ToString()
              ,
                VatUnit_Type = this.VatUnit_Type //  Request["VatUnit_Type"].ToString()

            };

            this.ReceiveStkItemList = this.ReceiveStkItemList.Where(m => m.Cancel_flag != "1").ToList();

            foreach (var a in this.ReceiveStkItemList)
            {

                InvoiceItemModel mo = new InvoiceItemModel();
                mo.Receive_Quantity = a.Receive_Quantity; // Util.ToDecimal(Request["Unit_Quantity"].ToString());

                mo.TradeDiscount_Percent = a.POItemTradeDiscount_Percent; // Util.ToDecimal(Request["POItemTradeDiscount_Percent"].ToString());
                mo.TradeDiscount_Price = a.POItemTradeDiscount_Price;// Util.ToDecimal(Request["POItemTradeDiscount_Price"].ToString());
                mo.Unit_Price = a.Unit_Price; // Util.ToDecimal(Request["Unit_Price"].ToString());
                mo.Unit_Quantity = a.Unit_Quantity; // Util.ToDecimal(Request["Unit_Quantity"].ToString());
                mo.Vat = a.POItem_Vat; // Util.ToDecimal(Request["PO_Vat"].ToString());
                inv.InvoiceItem.Add(mo);

            }


            inv.CalculatePrice();


            // calculate price
            this.Net_Amount = inv.Net_Amount;
            this.Total_Price = inv.Total_Price;
            this.Total_Discount = inv.Total_Discount;
            this.Total_Before_Vat = inv.Total_Before_Vat;
            this.Vat_Amount = inv.VAT_Amount;

            for (int ii = 0; ii < this.ReceiveStkItemList.Count; ii++)
            {
              

                this.ReceiveStkItemList[ii].Total_Before_Vat = inv.InvoiceItem[ii].Total_before_Vat;
                this.ReceiveStkItemList[ii].Vat_Amount = inv.InvoiceItem[ii].Vat_Amount;
                this.ReceiveStkItemList[ii].Net_Amount = inv.InvoiceItem[ii].Net_Amount;
                this.ReceiveStkItemList[ii].Total_Discount = inv.InvoiceItem[ii].Discount_Price;


            }


        }
        public void CalculatePrice()
        {
            InvoiceModel inv = new InvoiceModel()
            {
                TradeDiscount_Percent = this.TradeDiscount_Percent 
              , TradeDiscount_Price = this.TradeDiscount_Price 
              , TradeDiscount_Type = this.TradeDiscount_Type
              , Vat = this.Vat  
              , Vat_Type = this.Vat_Type 
              , VatUnit_Type = this.VatUnit_Type 

            };


            foreach(var a in this.ReceiveStkItemList){
        

                    InvoiceItemModel mo = new InvoiceItemModel();
                    mo.Receive_Quantity = a.Temp_Receive_Quantity; 

                    mo.TradeDiscount_Percent = a.POItemTradeDiscount_Percent; 
                    mo.TradeDiscount_Price = a.POItemTradeDiscount_Price;
                    mo.Unit_Price = a.Unit_Price;
                    mo.Unit_Quantity = a.Unit_Quantity; 
                    mo.Vat = a.POItem_Vat; 
                    inv.InvoiceItem.Add(mo);

         
                
            }
        



            inv.CalculatePrice();


            // calculate price
            this.Net_Amount = inv.Net_Amount;
            this.Total_Price = inv.Total_Price;
            this.Total_Discount = inv.Total_Discount;
            this.Total_Before_Vat = inv.Total_Before_Vat;
            this.Vat_Amount = inv.VAT_Amount;

            for (int ii = 0; ii < this.ReceiveStkItemList.Count; ii++)
            {
             
                    this.ReceiveStkItemList[ii].Total_Before_Vat = inv.InvoiceItem[ii].Total_before_Vat;
                    this.ReceiveStkItemList[ii].Vat_Amount = inv.InvoiceItem[ii].Vat_Amount;
                    this.ReceiveStkItemList[ii].Net_Amount = inv.InvoiceItem[ii].Net_Amount;
                    this.ReceiveStkItemList[ii].Total_Discount = inv.InvoiceItem[ii].Discount_Price;
                  
            

            }
        }
    
    }


    public class ReceiveStkItemModel
    {

        public int rownumber
        {
            get;
            set;
        }

        public int RecPay_ItemId
        {
            get;
            set;
        }

        public int POItem_ID
        {
            get;
            set;
        }
        public int Inv_ItemID
        {
            get;
            set;
        }
        public int Receive_Stk_ID
        {
            get;
            set;
        }
        public int Pack_ID
        {
            get;
            set;
        }
        public int Pack_Id_Base
        {
            get;
            set;
        }


        public int Receive_StkItem_ID
        {
            get;
            set;
        }
        public string Package_Name
        {
            get;
            set;
        }
        public decimal Unit_Price
        {
            get;
            set;
        }
        public decimal Unit_Quantity
        {
            get;
            set;
        }
        public string Inv_ItemCode
        {
            get;
            set;
        }
        public string Inv_ItemName
        {
            get;
            set;
        }
        public decimal POItemTradeDiscount_Percent
        {
            get;
            set;
        }
        public decimal POItemTradeDiscount_Price
        {
            get;
            set;
        }
        public decimal POItem_Vat
        {
            get;
            set;
        }

        public decimal POItem_Receive_Quantity
        {
            get;
            set;
        }

        public decimal Receive_Quantity
        {
            get;
            set;
        }
        public decimal Temp_Receive_Quantity
        {
            get;
            set;
        }


        public decimal Total_Discount
        {
            get;
            set;
        }

    
        public decimal GiveAway_Quantity
        {
            get;
            set;
        }

        public decimal Total_Before_Vat
        {
            get;
            set;
        }

        public decimal Vat_Amount
        {
            get;
            set;
        }

        public decimal Net_Amount
        {
            get;
            set;
        }


        public string Cancel_flag
        {
            get;
            set;
        }
        
      
        public void CreateLot()
        {

            if (StockLotList == null)
            {
                this.StockLotList = new List<StockLotModel>();
            }
          
          
            StockLotModel lot = new StockLotModel();

            lot.CreateLotLocation();
            lot.LotID = this.StockLotList.Count+1;
            lot.LotQty = Convert.ToInt32(this.Unit_Quantity) - Convert.ToInt32(this.POItem_Receive_Quantity);
            this.StockLotList.Clear();
            this.StockLotList.Add(lot);
         
         
        }
    

        public void AddLot()
        {
            if (StockLotList == null)
            {
                this.StockLotList = new List<StockLotModel>();
            }

            StockLotModel lot = new StockLotModel();

            lot.CreateLotLocation();
            lot.LotID = this.StockLotList.Count + 1;
            this.StockLotList.Add(lot);
        }
        public void DeleteLot()
        {


            if (this.StockLotList != null)
            {
                if (this.StockLotList.Count > 1)
                {
                    this.StockLotList.RemoveAt(this.StockLotList.Count - 1);
                }
            }
       
        
        }

        public void GetLot()
        {
            try
            {
               

                if (this.Temp_Receive_Quantity == 0) 
                {

                    DataTable dt = new DataTable();
                    dt = new ReceiveStockDAO().GetInvStockLotByStkItemID(this.Receive_StkItem_ID, this.Pack_ID, this.Inv_ItemID);
                    this.StockLotList = new List<StockLotModel>();

                    foreach (DataRow r in dt.Rows)
                    {

                  
                            this.StockLotList.Add(new StockLotModel()
                            {
                                LotID = Util.ToInt(r["Stock_Lot_ID"].ToString()),
                                LotNo = r["Lot_No"].ToString(),
                                BarcodeNo = r["Barcode_No"].ToString(),
                                BarcodePrintQty = Util.ToInt(r["Barcode_PrintQty"].ToString()),
                                LotQty = Util.ToInt(r["Lot_Qty"].ToString()),
                                ExpireDate = Util.ToDateTime(r["Expire_Date"].ToString())

                            });
                      

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
        
        public List<StockLotModel> StockLotList
        {
            get;
            set;
        }


   
        public DataTable GetLotView()
        {
            DataTable dtt = new DataTable();
            dtt.Columns.Add("Lot_ID", typeof(string));
            dtt.Columns.Add("Lot_No", typeof(string));
            dtt.Columns.Add("Lot_Qty", typeof(string));
            dtt.Columns.Add("Barcode_PrintQty", typeof(string));
            dtt.Columns.Add("rownumber", typeof(string));
            dtt.Columns.Add("IsNewLot", typeof(string));


          
            foreach(var a in this.StockLotList){

                DataRow drr = dtt.NewRow();
                drr["Lot_ID"] = a.LotID;
                drr["Lot_No"] = a.LotNo;
                drr["Lot_Qty"] = a.LotQty.ToString();
                drr["Barcode_PrintQty"] = a.BarcodePrintQty;
                drr["rownumber"] = 1;
                drr["IsNewLot"] = true;
                dtt.Rows.Add(drr);
       
             
            }

            return dtt;
        }

        public void CreateDefaultLot(int LotQty,int stkId)
        {
            this.StockLotList = new List<StockLotModel>();
            this.StockLotList.Add(new StockLotModel()
            {
                 LotQty = LotQty
            });
            this.StockLotList[0].CreateLotLocation();
            this.StockLotList[0].StockLotLocationList[0].LocationID = this.GetDefaultLocationId(stkId);
            this.StockLotList[0].StockLotLocationList[0].Qty_Location = LotQty;
            this.StockLotList[0].StockLotLocationList[0].Qty_Out = 0;
                 
        }


        private int GetDefaultLocationId(int stkId)
        {
            try
            {
              int id =  new ReceiveStockDAO().ReqInvLocationIDDefault(stkId);
              return id;
            }catch(Exception ex){
                throw ex;
            }
       
         
        }
       
    }


    public class StockLotModel
    {

        public int LotID{
            get;
            set;
        }

   
        public string LotNo{
            get;
            set;
        }

        public DateTime? ExpireDate
        {
            get;
            set;
        }

        public int LotQty{
            get;
            set;
        }

        public int BarcodePrintQty{
            get;
            set;
        }

        public string BarcodeNo
        {
            get;
            set;
        }

        public int RowNumber{
            get;
            set;
        }

        public bool IsNewLot{
            get;
            set;
        }

        public List<StockLotLocationModel> StockLotLocationList
        {
            get;
            set;
        }

   

        public DataTable LotLocationView
        {
            get;
            set;
        }



        public void CreateLotLocation()
        {
            if (this.StockLotLocationList == null)
            {
                this.StockLotLocationList = new List<StockLotLocationModel>();
            }

            StockLotLocationModel loc = new StockLotLocationModel();
            loc.LotID = this.StockLotLocationList.Count;
            this.StockLotLocationList.Add(loc);
        }

        public void AddLotLocation()
        {
            StockLotLocationModel loc = new StockLotLocationModel();
            loc.LotID = this.StockLotLocationList.Count;
            this.StockLotLocationList.Add(loc);
        }

        public void DeleteLotLocation()
        {
            if( this.StockLotLocationList.Count > 1){
                this.StockLotLocationList.RemoveAt(this.StockLotLocationList.Count - 1);
            }
        
        }


        public void GetLotLocation()
        {

             DataTable dt =   new ReceiveStockDAO().GetInvLotLocationSelectByStkLotID(this.LotID);

             this.StockLotLocationList = new List<StockLotLocationModel>();
     
                foreach(DataRow a in dt.Rows){
                    this.StockLotLocationList.Add(new StockLotLocationModel()
                    {
                        Stock_Lot_ID = Util.ToInt(a["Stock_Lot_ID"].ToString())
                        , LocationID = Util.ToInt(a["Location_ID"].ToString())
                        , Qty_Location = Util.ToInt(a["Qty_Location"].ToString())
                        , Qty_Out = Util.ToInt(a["Qty_Out"].ToString())
                        ,
                        Location_Name = a["Location_Name"].ToString()
                    });
                }
   

        }

          public DataTable GetLotLocationView()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Location_ID", typeof(int));
            dt.Columns.Add("IsNewLot", typeof(bool));
            dt.Columns.Add("rownumber", typeof(int));
            dt.Columns.Add("Lot_ID", typeof(int));
            dt.Columns.Add("Lot_Item_ID", typeof(int));
            dt.Columns.Add("Qty_Location", typeof(int));

            dt.Columns.Add("Location_Name", typeof(string));
              

            int i = 0;
              // temp
            if (this.StockLotLocationList == null)
            {
                this.GetLotLocation();
            

                foreach(var a in this.StockLotLocationList){
                    DataRow drr = dt.NewRow();
                    drr["Location_ID"] = a.LocationID;// a.LocationID;
                    drr["IsNewLot"] = true;
                    drr["rownumber"] = ++i;
                    drr["Lot_ID"] = a.Stock_Lot_ID;
                    drr["Lot_Item_ID"] = 1;
                    drr["Qty_Location"] = a.Qty_Location;

                    drr["Location_Name"] = a.Location_Name;
                    dt.Rows.Add(drr);
                }
   
                return dt;
            }


            foreach(var a in this.StockLotLocationList){

                DataRow drr = dt.NewRow();
                drr["Location_ID"] = a.LocationID;
                drr["IsNewLot"] = true;
                drr["rownumber"] = ++i;
                drr["Lot_ID"] = a.LotID;
                drr["Lot_Item_ID"] = a.LotItemID;
                drr["Qty_Location"] = a.Qty_Location;

                drr["Location_Name"] = a.Location_Name;  // temp
                dt.Rows.Add(drr);
            }

            return dt;
            
        }


     
      
    }


    public class StockLotLocationModel
    {
        public int LocationID
        {
            get;
            set;
        }
        public int Stock_Lot_ID
        {
            get;
            set;
        }

        public string Location_Name
        {
            get;
            set;
        }
        public bool IsNewLot
        {
            get;
            set;
        }

        public int RowNumber
        {
            get;
            set;
        }

        public int LotID
        {
            get;
            set;
        }
        public int LotItemID
        {
            get;
            set;
        }

        public int Qty_Location
        {
            get;
            set;
        }
        public int Qty_Out
        {
            get;
            set;
        }

        public DateTime Create_Date
        {
            get;
            set;
        }

        public int Create_By
        {
            get;
            set;
        }


         
    }
}