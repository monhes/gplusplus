using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

using GPlus.PRPO;
using GPlus;
using GPlus.DataAccess;
using System.Data;

namespace GPlus.UserControls
{
    public partial class ItemOrgStructControl2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnDep.OnClientClick = Util.CreatePopUp("../UserControls/pop_OrgSelect.aspx",
                                    new string[] { "searchAll", "divName", "depName", "orgID", "divCode", "depCode" },
                                    new string[] { "true", txtItemDivName.ClientID, TxtItemDepName.ClientID, hdOrgId.ClientID, HdDiv.ClientID, HdDep.ClientID },
                                    "popDivDepSelect"
                                );
            }
        }

        public void Clear()
        {
            hdOrgId.Value = "";
            HdDiv.Value = "";
            HdDep.Value = "";
            txtItemDivName.Text = "";
            TxtItemDepName.Text = "";
        }

        public string OrgStructID
        {
            get
            {
                return hdOrgId.Value;
            }
            set
            {
                hdOrgId.Value = value;
            }
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

        public string DepCode
        {
            get
            {
                return HdDep.Value;
            }
            set
            {
                HdDep.Value = value;
            }
        }

        public string DivName
        {
            get
            {
                return txtItemDivName.Text;
            }
            set
            {
                txtItemDivName.Text = value;
            }
        }

        public string DepName
        {
            get
            {
                return TxtItemDepName.Text;
            }
            set
            {
                TxtItemDepName.Text = value;
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