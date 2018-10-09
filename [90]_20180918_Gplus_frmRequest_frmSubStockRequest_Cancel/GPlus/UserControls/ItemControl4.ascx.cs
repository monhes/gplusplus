using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ItemControl4 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_Product4.aspx?id=" + hdID.ClientID +
                        "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID +
                        "&pid=" + hdUnitID.ClientID + "&pname=" + txtPackName.ClientID + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
            }
        }

        public void Clear()
        {
            hdID.Value = "";
            hdUnitID.Value = "";
            txtItemCode.Text = "";
            txtItemName.Text = "";
            txtPackName.Text = "";
        }

        public void BindItem(string itemID, string packID)
        {
            DataTable dt = new DataAccess.ItemDAO().GetItemPack(itemID, packID);
            DataTable dtP = new DataAccess.ItemDAO().GetItem(itemID);
            if (dt.Rows.Count > 0)
            {
                hdID.Value = itemID;
                hdUnitID.Value = packID;
                txtItemCode.Text = dtP.Rows[0]["Inv_ItemCode"].ToString();
                txtItemName.Text = dtP.Rows[0]["Inv_ItemName"].ToString();
                txtPackName.Text = dt.Rows[0]["Description"].ToString();
            }
        }

        public string ItemID
        {
            get
            {
                return hdID.Value;
            }
        }

        public string ItemCode
        {
            get
            {
                return txtItemCode.Text;
            }
        }

        public string ItemName
        {
            get
            {
                return txtItemName.Text;
            }
        }

        public string PackID
        {
            get
            {
                return hdUnitID.Value;
            }
        }

        public string PackName
        {
            get
            {
                return txtPackName.Text;
            }
        }
    }
}