using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;

namespace GPlus
{
    /// <summary>
    /// Summary description for autocomplete
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]
    public class autocomplete : System.Web.Services.WebService
    {

        [WebMethod]
        public string[] GetItemCompletionList(string prefixText, int count)
        {
            List<string> items = new List<string>(count);

            DataTable dt = new DataAccess.ItemDAO().GetItemByName(prefixText);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                items.Add(dt.Rows[i]["Inv_ItemCode"].ToString().PadRight(15, ' ') + dt.Rows[i]["Inv_ItemName"].ToString());
            }

            return items.ToArray();
        }

        [WebMethod]
        public string[] GetItemAndPackCompletionList(string prefixText, int count)
        {
            List<string> items = new List<string>(count);

            DataTable dt = new DataAccess.ItemDAO().GetItemAndPackByName(prefixText);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                items.Add(dt.Rows[i]["Inv_ItemCode"].ToString().PadRight(15, ' ') + dt.Rows[i]["Inv_ItemName"].ToString().PadRight(105, ' ') +
                    dt.Rows[i]["Pack_Name"].ToString());
            }

            return items.ToArray();
        }

        [WebMethod]
        public string GetItemID(string content)
        {
            string itemID = "";

            DataTable dt = new DataAccess.ItemDAO().GetItemID(content.Substring(0, 20).Trim());
            if (dt.Rows.Count > 0)
            {
                itemID = dt.Rows[0]["Inv_ItemID"].ToString();
            }

            return itemID;
        }

        [WebMethod]
        public string GetItemPackID(string content)
        {
            string itemPackID = "";

            DataTable dt = new DataAccess.ItemDAO().GetItemPackID(content.Substring(0, 20).Trim());//, content.Substring(125).Trim());
            if (dt.Rows.Count > 0)
            {
                itemPackID = dt.Rows[0]["Pack_Id"].ToString();
            }

            return itemPackID;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "text";
        }

    }
}
