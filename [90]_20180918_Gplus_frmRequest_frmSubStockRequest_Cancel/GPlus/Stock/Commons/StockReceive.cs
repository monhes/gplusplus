using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus.Stock.Commons
{
    public class StockReceive
    {
        public int PO_ID { get; set; }
        public int Receive_Stk_ID { get; set; }
        public string Receive_Stk_No { get; set; }
        public DateTime Receive_Date { get; set; }
        public string Invoice_No { get; set; }
        //public string PaymentType { get; set; }
        //public string PaymentCreditTerm { get; set; }
        //public string PaymentDate { get; set; }
        //public string PaymentPrice { get; set; }
        public DateTime Delivery_Date { get; set; }
        public string TradeDiscount_Type { get; set; }
        public string CashDiscount_Type { get; set; }
        public decimal TradeDiscount__Percent { get; set; }
        public decimal TradeDiscount__Bath { get; set; }
        public decimal CashDiscount__Percent { get; set; }
        public decimal CashDiscount__Bath { get; set; }
        public string Vat_Type { get; set; }
        public decimal Vat { get; set; }
        public string VatUnit_Type { get; set; }
        public decimal Total_Price { get; set; }
        public decimal Total_Discount { get; set; }
        public decimal Total_Before_Vat { get; set; }
        public decimal Net_Amonut { get; set; }
        public decimal VAT_Amount { get; set; }
        public decimal Invoice_Amount { get; set; }
        public DateTime Invoice_Date { get; set; }
        public int UpdateBy { get; set; }
        public string Delivery_Doc { get; set; }

        public bool IsTradeDiscount { get; set; }
        public bool HasVat { get; set; }
        public bool IsCashDiscount { get; set; }

        public int Stock_Id { get; set; }

    }
}