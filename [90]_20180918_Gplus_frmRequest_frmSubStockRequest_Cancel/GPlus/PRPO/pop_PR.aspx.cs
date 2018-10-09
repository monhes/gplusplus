using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PR : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    btnPrint.OnClientClick = btnPrint.OnClientClick = "open_popup('pop_PRPrintMainReport.aspx?id=" + Request["id"] 
                        + "', 850, 600, 'PRReport', 'yes', 'yes', 'yes'); return false;";

                    btnPrint3.OnClientClick = "open_popup('pop_PrintFormReport.aspx?id=" + Request["id"]
                                + "', 850, 600, 'PRFormReport', 'yes', 'yes', 'yes'); return false;";
                    DataTable dt = new DataAccess.PRDAO().GetPRForm1(Request["id"]);
                    if (dt.Rows.Count > 0)
                    {
                        lblPR.Text = dt.Rows[0]["PR_Code"].ToString();

                        if (dt.Rows[0]["PR_Type"].ToString() == "2")
                        {
                            btnPrint2.Visible = true;
                            btnPrint2.OnClientClick = "open_popup('pop_PRForm2Report.aspx?id=" + Request["id"]
                                + "', 850, 600, 'PRForm2Report', 'yes', 'yes', 'yes'); return false;";
                        }

                        dt = new DataAccess.PRDAO().GetFromPrint(Request["id"], "");

                        if (dt.Rows.Count > 0)
                            btnPrint3.Visible = true;
                    }
                }
            }

        }

    }
}