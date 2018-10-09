using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus.Stock.Commons
{
    public class StockReceiveNotSame
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal UnitPrice { get; set; }
        public int PackId { get; set; }
        public string PackName { get; set; }
        public int ItemCount { get; set; }
    }
}