using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ItemControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_Product.aspx?id=" + hdID.ClientID +
                        "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
            }
        }

        public void BindItem(string itemID)
        {
            DataTable dt = new DataAccess.ItemDAO().GetItem(itemID);
            if (dt.Rows.Count > 0)
            {
                hdID.Value = itemID;
                txtItemCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();
                txtItemName.Text = dt.Rows[0]["Inv_ItemName"].ToString();
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
            set
            {
                txtItemCode.Text = value;
            }
        }

        public string ItemName
        {
            get
            {
                return txtItemName.Text;
            }
            set
            {
                txtItemName.Text = value;
            }
        }

        public void Clear()
        {
            hdID.Value = "";
            txtItemCode.Text = "";
            txtItemName.Text = "";
        }

    }
}