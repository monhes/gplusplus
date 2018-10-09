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
    public partial class ItemDivDepControl : System.Web.UI.UserControl
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

                btnDep.OnClientClick = Util.CreatePopUp("../PRPO/pop_DepSelect.aspx",
                                    new string[] { "divName", "depName" },
                                    new string[] { txtItemDivName.ClientID, TxtItemDepName.ClientID },
                                    "popDivDepSelect"
                                );
            }
        }
    }
}