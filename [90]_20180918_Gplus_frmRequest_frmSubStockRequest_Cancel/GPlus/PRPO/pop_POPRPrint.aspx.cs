using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_POPRPrint : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null)
                {
                    DataSet ds = new DataAccess.PODAO().GetPOPRReport(Request["id"]);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("PO_Date_TH");
                        //ds.Tables[0].Columns.Add("Request_Date_TH");
                        if (ds.Tables[0].Rows[0]["PO_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["PO_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        //if (ds.Tables[1].Rows[0]["Request_Date"].ToString().Length > 0)
                        //{
                        //    int dateYear = ((DateTime)ds.Tables[1].Rows[0]["Request_Date"]).Year;
                        //    if (dateYear < 2500)
                        //        dateYear += 543;
                        //    ds.Tables[0].Rows[0]["Request_Date_TH"] = ((DateTime)ds.Tables[1].Rows[0]["Request_Date"]).Day.ToString() + "/" +
                        //        ((DateTime)ds.Tables[1].Rows[0]["Request_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        //}

                        ds.Tables[1].Columns.Add("Request_Date_TH");
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        { 
                            int dateYear = ((DateTime)ds.Tables[1].Rows[i]["Request_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[1].Rows[i]["Request_Date_TH"] = ((DateTime)ds.Tables[1].Rows[i]["Request_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[1].Rows[i]["Request_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }
                        
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("HeaderDataSet", ds.Tables[0]));
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemDataSet", ds.Tables[1]));

                        ReportViewer1.Visible = true;
                    }
                }

            }
        }
    }
}