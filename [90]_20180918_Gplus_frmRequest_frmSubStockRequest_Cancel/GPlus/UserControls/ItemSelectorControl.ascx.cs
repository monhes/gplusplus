using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ItemSelectorControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string ItemID
        {
            get
            {
                return hdItemID.Value;
            }
            set
            {
                hdItemID.Value = value;
                DataTable dt = new DataAccess.ItemDAO().GetItem(hdItemID.Value);
                if (dt.Rows.Count > 0)
                {
                    txtProduct.Text = dt.Rows[0]["Inv_ItemCode"].ToString().PadRight(20, ' ') + dt.Rows[0]["Inv_ItemName"].ToString();
                    hdStatus.Value = "1";
                }
                else
                {
                    hdItemID.Value = "";
                    hdStatus.Value = "0";
                }
            }
        }

        public bool EnableValidate
        {
            get
            {
                return RequiredFieldValidator3.Enabled;
            }
            set
            {
                RequiredFieldValidator3.Enabled = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return txtProduct.Enabled;
            }
            set
            {
                txtProduct.Enabled = value;
            }
        }

    }
}