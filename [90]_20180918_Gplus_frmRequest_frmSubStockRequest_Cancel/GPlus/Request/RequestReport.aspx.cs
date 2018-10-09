using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace GPlus.Request
{
    public partial class RequestReport : Pagebase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null && Request["pay_id"] != null)
                {
                    PrintWidthDraw_Pay();
                }
                else if (Request["id"] != null)
                {
                    PrintWidthDraw();
                }
            }
        }

        protected void PrintWidthDraw()
        {
            DataTable dt = new DataAccess.RequestDAO().GetRequestReport(Request["id"]);
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("Request_Date_TH");
                if (dt.Rows[0]["Request_Date"].ToString().Length > 0)
                {
                    int dateYear = ((DateTime)dt.Rows[0]["Request_Date"]).Year;
                    if (dateYear < 2500)
                        dateYear += 543;
                    dt.Rows[0]["Request_Date_TH"] = ((DateTime)dt.Rows[0]["Request_Date"]).Day.ToString() + "/" +
                        ((DateTime)dt.Rows[0]["Request_Date"]).Month.ToString() + "/" + dateYear.ToString();
                }

                for (int i = dt.Rows.Count; i < 6; i++)
                    dt.Rows.Add(dt.NewRow());

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(
                    new Microsoft.Reporting.WebForms.ReportDataSource("RequestDataSet", dt));

                ReportViewer1.Visible = true;
            }
        }


        protected void PrintWidthDraw_Pay()
        {
            DataTable dt = new DataAccess.RequestDAO().GetRequestReport_Pay(Request["id"], Request["pay_id"]);
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("Request_Date_TH");
                if (dt.Rows[0]["Request_Date"].ToString().Length > 0)
                {
                    int dateYear = ((DateTime)dt.Rows[0]["Request_Date"]).Year;
                    if (dateYear < 2500)
                        dateYear += 543;
                    dt.Rows[0]["Request_Date_TH"] = ((DateTime)dt.Rows[0]["Request_Date"]).Day.ToString() + "/" +
                        ((DateTime)dt.Rows[0]["Request_Date"]).Month.ToString() + "/" + dateYear.ToString();
                }

                dt.Columns.Add("Pay_Date_TH");
                if (dt.Rows[0]["Pay_Date"].ToString().Length > 0)
                {
                    int dateYear = ((DateTime)dt.Rows[0]["Pay_Date"]).Year;
                    if (dateYear < 2500)
                        dateYear += 543;
                    dt.Rows[0]["Pay_Date_TH"] = ((DateTime)dt.Rows[0]["Pay_Date"]).Day.ToString() + "/" +
                        ((DateTime)dt.Rows[0]["Pay_Date"]).Month.ToString() + "/" + dateYear.ToString();
                }


                for (int i = dt.Rows.Count; i < 6; i++)
                    dt.Rows.Add(dt.NewRow());

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(
                    new Microsoft.Reporting.WebForms.ReportDataSource("RequestDataSet", dt));

                ReportViewer1.Visible = true;
            }
        }


    }
}