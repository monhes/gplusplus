using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Security.Cryptography;
using System.Globalization;
using System.Web;

namespace GPlus
{
    public class Util
    {
        public static string GetSiteRoot()
        {
            string Port = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (((Port == null) || (Port == "80")) || (Port == "443"))
            {
                Port = "";
            }
            else
            {
                Port = ":" + Port;
            }
            string Protocol = HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if ((Protocol == null) || (Protocol == "0"))
            {
                Protocol = "http://";
            }
            else
            {
                Protocol = "https://";
            }
            string url = (Protocol + HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + HttpContext.Current.Request.ApplicationPath);
            if (url[url.Length - 1] != '/')
                url += '/';
            return url;
        }

        public static string EncryptPassword(string target)
        {
            System.Security.Cryptography.SHA256 hash = System.Security.Cryptography.SHA256.Create();
            System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();
            byte[] combined = encoder.GetBytes("p" + target + "d");
            hash.ComputeHash(combined);
            return Convert.ToBase64String(hash.Hash);
        }

        public static string GetShotFileName(string fileName)
        {
            return fileName.Split(new char[] { '.' })[fileName.Split(new char[] { '.' }).Length - 1];
        }

        public static string RemovePathFile(string fileName)
        {
            string[] files = fileName.Split(new char[] { '\\' });
            return files[files.Length - 1];
        }


        public static string ThaiBaht(string txt)
        {
            string bahtTxt, n, bahtTH = "";
            double amount;
            try { amount = Convert.ToDouble(txt); }
            catch { amount = 0; }
            bahtTxt = amount.ToString("####.00");
            string[] num = { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า", "สิบ" };
            string[] rank = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
            string[] temp = bahtTxt.Split('.');
            string intVal = temp[0];
            string decVal = temp[1];
            if (Convert.ToDouble(bahtTxt) == 0)
                bahtTH = "ศูนย์บาทถ้วน";
            else
            {
                for (int i = 0; i < intVal.Length; i++)
                {
                    n = intVal.Substring(i, 1);
                    if (n != "0")
                    {
                        if ((i == (intVal.Length - 1)) && (n == "1") && i > 0)
                            bahtTH += "เอ็ด";
                        else if ((rank[(intVal.Length - i) - 1] == "สิบ") && (n == "2"))
                            bahtTH += "ยี่";
                        else if ((rank[(intVal.Length - i) - 1] == "สิบ") && (n == "1"))
                            bahtTH += "";
                        else
                            bahtTH += num[Convert.ToInt32(n)];

                        bahtTH += rank[(intVal.Length - i) - 1];

                    }
                    else if ((intVal.Length - i) - 1 == 6) bahtTH += rank[(intVal.Length - i) - 1];
                }
                bahtTH += "บาท";
                if (decVal == "00")
                    bahtTH += "ถ้วน";
                else
                {
                    for (int i = 0; i < decVal.Length; i++)
                    {
                        n = decVal.Substring(i, 1);
                        if (n != "0")
                        {
                            if ((i == decVal.Length - 1) && (n == "1"))
                                bahtTH += "เอ็ด";
                            else if ((i == (decVal.Length - 2)) && (n == "2"))
                                bahtTH += "ยี่";
                            else if ((i == (decVal.Length - 2)) && (n == "1"))
                                bahtTH += "";
                            else
                                bahtTH += num[Convert.ToInt32(n)];
                            bahtTH += rank[(decVal.Length - i) - 1];
                        }
                    }
                    bahtTH += "สตางค์";

                }
            }
            return bahtTH;
        }

#region Green 19072013
        public static string CreatePopUp(string url, string[] param, string[] control, string name)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("Url must not be null or empty.");

            string popup = "";
            string end = "', 850, 400, '" + name + "', 'yes', 'yes', 'yes'); return false;";

            if (param == null && control == null)
            {
                popup += "open_popup('" + url + end;
            }
            else if (param != null && control != null)
            {
                if (param.Length != control.Length)
                    throw new ArgumentException("Lenght of param must match with length of control.");
                else
                {
                    popup += "open_popup('" + url + "?";
                    for (int i = 0; i < param.Length; ++i)
                    {
                        popup += param[i] + "=" + control[i] + "&";
                    }

                    popup = popup.Remove(popup.Length - 1) + end;
                }
            }

            return popup;
        }

        public static string FillControl(string[] control, string[] value)
        {
            if (control.Length != value.Length)
                throw new ArgumentException("number of control must match with number of value");

            string js = "if(window.opener){ ";

            for (int i = 0; i < control.Length; ++i)
            {
                js += "window.opener.document.getElementById('" + control[i] + "').value = '" + value[i] + "'; ";
            }

            js += "} window.close(); return false;";

            return js;
        }
#endregion

 #region PT


        public static int ToInt(string s)
        {
            try
            {
                if (s == "") return 0;
                else return int.Parse(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
       
        }

        public static DateTime? ToDateTime(string s)
        {
            if (s == "") return null;

            try
            {
                string[] ss = s.Split('/');
                return new DateTime(Convert.ToInt32(ss[2]), Convert.ToInt32(ss[1]), Convert.ToInt32(ss[0]));
            }
            catch (Exception)
            {
                return null;
            }
            

            //if (s == "") return null;
            //else return DateTime.Parse(s);
        }

        public static Decimal ToDecimal(string s)
        {

            try
            {
                if (s == "") return 0;
                else return decimal.Parse(s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }


        public static bool IsNumber(string num)
        {
            int temp= 0;
            if (num.Trim() != "")
            {
                if (!(int.TryParse(num, out temp)))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
        #endregion
    }

    #region PT


    public class ExcecutionResult
    {
        public bool result;
        public string message;

    }

    #endregion

}