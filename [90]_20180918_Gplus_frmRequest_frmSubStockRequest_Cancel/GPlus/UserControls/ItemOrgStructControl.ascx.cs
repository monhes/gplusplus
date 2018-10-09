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
    public partial class ItemOrgStructControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SQLParameterList sqlParamList = new SQLParameterList();
                sqlParamList.AddIntegerField("OrgStrucId", Convert.ToInt32(Session["OrgID"].ToString()));
                DataSet ds = new DatabaseHelper().ExecuteDataSet("sp_Inv_GetDivDep", sqlParamList.GetSqlParameterList());

                Debug.WriteLine(
                    ds.Tables[0].Rows[0][0].ToString() + ", " +
                    ds.Tables[0].Rows[0][1].ToString() + ", " +
                    ds.Tables[0].Rows[0][2].ToString() + ", " + 
                    ds.Tables[0].Rows[0][3].ToString()
                );

                if (ds.Tables[0].Rows[0][1].ToString() == "ฝ่ายจัดซื้อ" || ds.Tables[0].Rows[0][1].ToString() == "ฝ่ายธุรการ")
                {

                }
                else
                {
                    string[] divdep = new PrPoDAO().GetDivDepName(Convert.ToInt32(Session["OrgID"].ToString()));
                    txtItemDivName.Text = divdep[0];
                }

                btnDep.OnClientClick = Util.CreatePopUp("../UserControls/pop_OrgSelect.aspx",
                                    new string[] { "divName", "depName", "orgID", "divCode", "depCode" },
                                    new string[] { txtItemDivName.ClientID, TxtItemDepName.ClientID, hdOrgId.ClientID, HdDiv.ClientID, HdDep.ClientID },
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