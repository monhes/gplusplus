using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.UserControls
{
    public partial class AccountNameControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnAccSearch.OnClientClick = Util.CreatePopUp
                                             (
                                                //"../MasterData/PopAccName.aspx"
                                                "../reports/dgEmplyeeRequest3.aspx"
                                                , new string[] { "accName", "accId" }
                                                , new string[] { txtAccName.ClientID, HDAccId.ClientID } //tb.ClientID }
                                                , "AccName"
                                             );
            }
        }
        public void Clear()
        {
            HDAccId.Value = "";
            txtAccName.Text = "";
        }
        public string AccName
        {
            get
            {
                return txtAccName.Text;
            }
            set
            {
                txtAccName.Text = value;
            }
        }
        public string AccId
        {
            get
            {
                return HDAccId.Value;
            }
            set
            {
                HDAccId.Value = value;
            }
        }

    }
}