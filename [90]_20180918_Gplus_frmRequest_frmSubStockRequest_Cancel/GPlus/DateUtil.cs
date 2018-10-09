using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GPlus
{
    public abstract class DateUtil
    {
        protected Dictionary<string, int> _MonthStringKey = new Dictionary<string, int>();
        protected Dictionary<int, string> _MonthIntKey = new Dictionary<int, string>();

        public DateUtil()
        {
        }

        public virtual int NowYear()
        {
            return DateTime.Now.Year;
        }

        public int GetMonthNumber(string month)
        {
            return _MonthStringKey[month];
        }

        public string GetMonthName(int month)
        {
            return _MonthIntKey[month];
        }
    }

    public class ThaiDateUtil : DateUtil
    {
        public override int NowYear()
        {
            return DateTime.Now.Year + 543;
        }

        public string GetDayMonthYear()
        {
            return Convert.ToString(DateTime.Now.Day)   + "/" + 
                   Convert.ToString(DateTime.Now.Month) + "/" + 
                   Convert.ToString(NowYear());
        }

        public ThaiDateUtil()
        {
            _MonthStringKey.Add("มกราคม", 1);
            _MonthStringKey.Add("กุมภาพันธ์", 2);
            _MonthStringKey.Add("มีนาคม", 3);
            _MonthStringKey.Add("เมษายน", 4);
            _MonthStringKey.Add("พฤษภาคม", 5);
            _MonthStringKey.Add("มิถุนายน", 6);
            _MonthStringKey.Add("กรกฎาคม", 7);
            _MonthStringKey.Add("สิงหาคม", 8);
            _MonthStringKey.Add("กันยายน", 9);
            _MonthStringKey.Add("ตุลาคม", 10);
            _MonthStringKey.Add("พฤศจิกายน", 11);
            _MonthStringKey.Add("ธันวาคม", 12);

            _MonthIntKey.Add(1, "มกราคม");
            _MonthIntKey.Add(2, "กุมภาพันธ์");
            _MonthIntKey.Add(3, "มีนาคม");
            _MonthIntKey.Add(4, "เมษายน");
            _MonthIntKey.Add(5, "พฤษภาคม");
            _MonthIntKey.Add(6, "มิถุนายน");
            _MonthIntKey.Add(7, "กรกฎาคม");
            _MonthIntKey.Add(8, "สิงหาคม");
            _MonthIntKey.Add(9, "กันยายน");
            _MonthIntKey.Add(10, "ตุลาคม");
            _MonthIntKey.Add(11, "พฤศจิกายน");
            _MonthIntKey.Add(12, "ธันวาคม");
        }
    }
}