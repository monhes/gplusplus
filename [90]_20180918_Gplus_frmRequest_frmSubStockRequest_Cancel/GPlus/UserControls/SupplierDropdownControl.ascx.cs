using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class SupplierDropdownControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlSupplier.Items.Clear();
                ddlSupplier.DataSource = new DataAccess.SupplierDAO().GetSupplier("", "", "1", 1, 1000, "", "").Tables[0];
                ddlSupplier.DataTextField = "Supplier_Name";
                ddlSupplier.DataValueField = "Supplier_ID";
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("เลือก Supplier", ""));

                btnSupplierSearch.OnClientClick =
                    "open_popup('../PRPO/pop_SupplierSelect.aspx?supplierDdl=" + ddlSupplier.ClientID +
                    "&FilterStatus=true', 850, 400, 'popSupplier', 'yes', 'yes', 'yes'); return false;";
            }
        }

        public string SupplierID { get { return ddlSupplier.SelectedValue; } }

        public bool EnableValidator 
        { 
            set 
            { 
                RequiredFieldValidator1.Enabled = value; 
                RequiredFieldValidator1_ValidatorCalloutExtender.Enabled = value; 
            }    
        }

        public bool RequireValidator
        {
            set
            {
                RequiredFieldValidator1_ValidatorCalloutExtender.Enabled = value;
                RequiredFieldValidator1.Enabled = value;
            }
            get
            {
                return
                    RequiredFieldValidator1_ValidatorCalloutExtender.Enabled
                    || RequiredFieldValidator1.Enabled;
            }
        }

        public void setFirstDropdownIndex() { ddlSupplier.SelectedIndex = 0; }
    }
}