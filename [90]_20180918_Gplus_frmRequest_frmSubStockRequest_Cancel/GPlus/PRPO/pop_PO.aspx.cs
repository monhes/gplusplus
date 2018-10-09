using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PO : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["id"] != null)
                {
                    DataTable dt = new DataAccess.PODAO().GetPOForm1(Request["id"]);
                    if (dt.Rows.Count > 0)
                    {
                        lblPO.Text = dt.Rows[0]["PO_Code"].ToString();
                    }
                    btnPrint.OnClientClick = "open_popup('pop_POPrintMainReport.aspx?id=" + Request["id"] 
                        + "', 850, 600, 'PRReport', 'yes', 'yes', 'yes'); return false;";

                    dt = new DataAccess.PODAO().GetPOPR(Request["id"]);
                    btnPrintPR.Visible = dt.Rows.Count > 3;

                    dt = new DataAccess.PRDAO().GetFromPrint("", Request["id"]);
                    btnPrintForm.Visible = dt.Rows.Count > 0;
                    btnPrintForm.OnClientClick = "open_popup('pop_POPrintFormReport.aspx?id=" + Request["id"]
                        + "', 850, 600, 'PRReport', 'yes', 'yes', 'yes'); return false;";

                    btnPrintPR.OnClientClick = "open_popup('pop_POPRPrint.aspx?id=" + Request["id"]
                        + "', 850, 600, 'PRReport', 'yes', 'yes', 'yes'); return false;";

                    btnPrintAttach.OnClientClick = "open_popup('pop_POPrintAttachReport.aspx?id=" + Request["id"]
                        + "', 850, 600, 'PRReport', 'yes', 'yes', 'yes'); return false;";
                }
            }

        }
    }
}