using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus
{
    public class Genbarcode
    {
        public string Genearetebarcode(string ItemId, string PackId,string LotNo)
        {
            string barcode = "";
            //string delim = "-";
            //string str_ItemId = ItemId.Trim(delim.ToCharArray());
            string str_ItemId = ItemId.Replace(@"-", string.Empty);
            string str_PackId = "";

            if (str_ItemId.Length != 10)
            {
                if (str_ItemId.Length == 9)
                {
                    str_ItemId = "0" + str_ItemId;
                }
            }

            if (PackId.Length < 3)
            {
                if (PackId.Length == 2)
                {
                    str_PackId = "0" + PackId;
                }
                else if (PackId.Length == 1)
                {
                    str_PackId = "00" + PackId;
                }
            }

            barcode = str_ItemId + str_PackId + LotNo;
            return barcode; 
        }

    }
}