using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace GPlus.Stock.Commons
{
    public class StockReceiveLot
    {
        public int Receive_StkItem_ID { get; set; }
        public int Stock_Lot_ID { get; set; }
        public int Stock_ID { get; set; }
        public int Inv_ItemID { get; set; }
        public int Pack_ID { get; set; }
        public DateTime In_Date { get; set; }
        public DateTime Expire_Date { get; set; }
        public string Lot_No { get; set; }
        public int Lot_Qty { get; set; }
        //public DateTime ExpireDate { get; set; }
        public string Barcode_No { get; set; }
        public int Barcode_PrintQty { get; set; }
        public decimal Lot_Amount { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal TradeDiscount_Percent { get; set; }
        public decimal TradeDiscount_Price { get; set; }
        public decimal CashDiscount_Percent { get; set; }
        public decimal CashDiscount_Price { get; set; }
        public decimal Total_before_Vat { get; set; }
        public decimal Net_Amount { get; set; }

        public List<StockReceiveLotLocation> LotLocation { get; set; }


        public StockReceiveLot()
        {
            this.LotLocation = new List<StockReceiveLotLocation>();
        }
        //public decimal Unit_Price { get; set; }

        //private DataTable _lotLocation;
        //public DataTable LotLocation
        //{ 
        //    get { return _lotLocation; }
        //    set
        //    {
        //        _lotLocation = value.Clone();
        //    }
        //}
    }
}