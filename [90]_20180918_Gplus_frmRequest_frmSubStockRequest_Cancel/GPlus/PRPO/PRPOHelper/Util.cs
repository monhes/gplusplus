using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus.PRPO.PRPOHelper
{
    public static class PRPOUtility
    {
        public static string To2PointString(string value)
        {
            if (value.Trim().Length > 0)
            {
                return Convert.ToDecimal(value).ToString("0.00");
            }

            return "";
        }

        public static string ToIntegerString(string value)
        {
            if (value.Trim().Length > 0)
            {
                return Convert.ToDecimal(value).ToString("0");
            }

            return "";
        }

        public static string MapAssetType(string value)
        {
            if (value == "0") 
                return "S";
            else if (value == "1")
                return "A";
            else
                throw new ArgumentException();
        }
    }
}