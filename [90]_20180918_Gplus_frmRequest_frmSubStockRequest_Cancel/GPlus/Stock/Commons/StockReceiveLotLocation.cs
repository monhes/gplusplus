using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus.Stock.Commons
{
    public class StockReceiveLotLocation
    {
        public int Stock_Lot_ID { get; set; }
        public int Location_ID { get; set; }
        public int Qty_Location { get; set; }
        public int Create_By { get; set; }
    }
}