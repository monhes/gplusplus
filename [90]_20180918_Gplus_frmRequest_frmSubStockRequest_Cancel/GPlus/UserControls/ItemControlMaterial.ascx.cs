using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ItemControlMaterial : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_ProductMaterial.aspx?id=" + hdID.ClientID +
               "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID + "&materialID=" + hdMaterialID.Value.ToString() + "&materialName=" + hdMaterialName.Value.ToString() + "&SubmaterialID=" + hdSubMaterialID.Value.ToString() + "&SubmaterialName=" + hdSubMaterialName.Value.ToString() + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
            }
        }

        public void Clear()
        {
            hdID.Value = "";
            hdMaterialID.Value = "";
            hdMaterialName.Value = "";
            hdSubMaterialID.Value = "";
            hdSubMaterialName.Value = "";
            txtItemCode.Text = "";
            txtItemName.Text = "";
        }

        public void BindItem(string itemID)
        {
            DataTable dt = new DataAccess.ItemDAO().GetItem(itemID);
            if (dt.Rows.Count > 0)
            {
                hdID.Value = itemID;
                txtItemCode.Text = dt.Rows[0]["Inv_ItemCode"].ToString();
                txtItemName.Text = dt.Rows[0]["Inv_ItemName"].ToString();
                hdMaterialID.Value = "Test";
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

        public String MaterialID
        {
            get
            {
                return hdMaterialID.Value;
            }
            set
            {
                hdMaterialID.Value = value;

                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_ProductMaterial.aspx?id=" + hdID.ClientID +
                "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID + "&materialID=" + hdMaterialID.Value.ToString() + "&materialName=" + hdMaterialName.Value.ToString() + "&SubmaterialID=" + hdSubMaterialID.Value.ToString() + "&SubmaterialName=" + hdSubMaterialName.Value.ToString() + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
            }
        }

        public String MaterialName
        {
            get
            {
                return hdMaterialName.Value;
            }
            set
            {
                hdMaterialName.Value = value;

                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_ProductMaterial.aspx?id=" + hdID.ClientID +
                "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID + "&materialID=" + hdMaterialID.Value.ToString() + "&materialName=" + hdMaterialName.Value.ToString() + "&SubmaterialID=" + hdSubMaterialID.Value.ToString() + "&SubmaterialName=" + hdSubMaterialName.Value.ToString() + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
     
            }
        }

        public String SubMaterialID
        {
            get
            {
                return hdSubMaterialID.Value;
            }
            set
            {
                hdSubMaterialID.Value = value;
                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_ProductMaterial.aspx?id=" + hdID.ClientID +
                "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID + "&materialID=" + hdMaterialID.Value.ToString() + "&materialName=" + hdMaterialName.Value.ToString() + "&SubmaterialID=" + hdSubMaterialID.Value.ToString() + "&SubmaterialName=" + hdSubMaterialName.Value.ToString() + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
     

           }
        }

        public String SubMaterialName
        {
            get
            {
                return hdSubMaterialName.Value;
            }
            set
            {
                hdSubMaterialName.Value = value;
                btnSelect1.OnClientClick = "open_popup('../UserControls/pop_ProductMaterial.aspx?id=" + hdID.ClientID +
                "&code=" + txtItemCode.ClientID + "&name=" + txtItemName.ClientID + "&materialID=" + hdMaterialID.Value.ToString() + "&materialName=" + hdMaterialName.Value.ToString() + "&SubmaterialID=" + hdSubMaterialID.Value.ToString() + "&SubmaterialName=" + hdSubMaterialName.Value.ToString() + "', 850, 400, 'popProduct1', 'yes', 'yes', 'yes');return false;";
     
            }
        }

    }
}