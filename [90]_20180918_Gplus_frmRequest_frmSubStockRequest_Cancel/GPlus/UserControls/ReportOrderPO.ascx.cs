using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class ReportOrderPO : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnDep.OnClientClick = Util.CreatePopUp("POP_SupplierSelectName.aspx", new string[] { "supplierName" }, new string[] { TxtSupplier.ClientID }, "");
        }

        public void Clear()
        {
            HdDiv.Value = "";
            TxtSupplier.Text = "";
        }

     
        public string DivCode
        {
            get
            {
                return HdDiv.Value;
            }
            set
            {
                HdDiv.Value = value;
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

       

        public void setSelectBtnEnable()
        {
            btnDep.Enabled = true;
        }

        public void setSelectBtnDisable()
        {
            btnDep.Enabled = false;
        }
    }
}