using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ItemPackSelectorControl : System.Web.UI.UserControl
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
                    txtProductCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();
                    txtProductName.Text = dt.Rows[0]["Inv_ItemName"].ToString() + " " + dt.Rows[0]["Inv_Attrbute"].ToString();
                    hdStatus.Value = "1";
                }
                else
                {
                    hdItemID.Value = "";
                    hdStatus.Value = "0";
                }
            }
        }

        public string ItemPackID
        {
            get
            {
                return hdPackID.Value;
            }
            set
            {
                hdPackID.Value = value;
                if (hdPackID.Value.Trim().Length > 0 && hdItemID.Value.Trim().Length > 0)
                {
                    DataRow[] drs = new DataAccess.ItemDAO().GetItemPack(hdItemID.Value).Select("Pack_Id = " + hdPackID.Value);
                    if (drs.Length > 0)
                    {
                        txtPackage.Text += drs[0]["Description"].ToString();
                        hdStatus.Value = "1";
                    }
                    else
                    {
                        hdItemID.Value = "";
                        hdPackID.Value = "";
                        hdStatus.Value = "0";
                    }
                }
                else
                {
                    hdItemID.Value = "";
                    hdPackID.Value = "";
                    hdStatus.Value = "0";
                }
            }
        }


        public bool Enabled
        {
            get
            {
                return txtProductCode.Enabled;
            }
            set
            {
                txtProductCode.Enabled = value;
                txtProductName.Enabled = value;
                txtPackage.Enabled = value;
            }
        }
    }
}