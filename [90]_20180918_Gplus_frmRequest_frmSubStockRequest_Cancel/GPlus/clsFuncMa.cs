using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus
{
    public class clsFuncMa
    {
        System.Globalization.CultureInfo cultureInfo;

        public String _fn_ConvertDateStored(DateTime datConvDate)
        {

            String strConvDate = "";

            try
            {

                strConvDate = datConvDate.Year.ToString("0000") + "/" + datConvDate.Month.ToString("00") + "/" + datConvDate.Day.ToString("00");

                return strConvDate;
            }

            catch (Exception ex)
            {
                return strConvDate;
            }

        }

        public String _fn_ConvertDateStored(String strConvDate)
        {
            try
            {
                if (strConvDate != null)
                {
                    DateTime dt_temp = Convert.ToDateTime(strConvDate, new System.Globalization.CultureInfo("th-TH"));
                    strConvDate = _fn_ConvertDateStored(dt_temp);
                }
                return strConvDate;
            }

            catch (Exception ex)
            {
                return strConvDate;
            }

        }

    }
}