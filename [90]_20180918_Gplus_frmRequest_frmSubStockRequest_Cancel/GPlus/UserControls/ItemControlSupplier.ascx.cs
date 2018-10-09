using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ItemControlSupplier : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                btnDep.OnClientClick = "open_popup('../PRPO/HPSupplierSelect.aspx?supplierID=" + hdID.ClientID + "&supplierName=" + TxtSupplier.ClientID + "', 850, 400, 'popSupplier', 'yes', 'yes', 'yes');return false;";
            }
        }

        public void Clear()
        {
            hdID.Value = "";
            TxtSupplier.Text = "";
        }

        public void BindItem(string DivID)
        {
            DataTable dt = new DataAccess.HistoryPurchaseDAO().GetHistoryPurchase(DivID);
            if (dt.Rows.Count > 0)
            {
                hdID.Value = DivID;
                TxtSupplier.Text = dt.Rows[0]["Supplier_Name"].ToString();
            }
        }

        public string DivID
        {
            get
            {
                return hdID.Value;
            }
            set
            {
                hdID.Value = value;
            }
        }


        public string DivName
        {
            get
            {
                return TxtSupplier.Text;
            }
            set
            {
                TxtSupplier.Text = value;
            }
        }

    }
}