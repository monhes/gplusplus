using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus.Stock.Commons
{
    public class StockReceiveItem
    {
        public int Receive_StkItem_ID { get; set; }
        public string PO_ID { get; set; }
        public string Inv_ItemId { get; set; }
        public decimal Recive_Quantity { get; set; }
        public decimal Total_before_Vat { get; set; }
        public decimal Net_Amount { get; set; }
        public decimal TradeDiscount_Percent { get; set; }
        public decimal TradeDiscount_Price { get; set; }
        public decimal CashDiscount_Percent { get; set; }
        public decimal CashDiscount_Price { get; set; }
        public GiveAwayTypes GiveAwayType { get; set; }
        public string procure_name { get; set; }
        public string specify { get; set; }
        public int Pack_ID { get; set; }
        public decimal Unit_Price { get; set; }
        public string IsGiveAway { get; set; }
        //public decimal Total_Discount { get; set; }
        public decimal Total_Discount { get; set; }
        public decimal TotalGiveAway { get; set; }


        public List<StockReceiveLot> StockLot { get; set; }

        public StockReceiveItem()
        {
            this.GiveAwayType = GiveAwayTypes.None;
            this.StockLot = new List<StockReceiveLot>();
        }
    }
}