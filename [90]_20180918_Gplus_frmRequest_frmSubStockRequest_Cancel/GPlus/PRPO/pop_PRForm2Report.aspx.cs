using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PRForm2Report : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null)
                {
                    DataSet ds = new DataAccess.PRDAO().GetPRReport2(Request["id"]);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("Net_Amonut_TH");
                        ds.Tables[0].Rows[0]["Net_Amonut_TH"] = Util.ThaiBaht(ds.Tables[0].Rows[0]["Net_Amonut"].ToString());

                       /*
                        for (int i = ds.Tables[1].Rows.Count; i < 4; i++)
                        {
                            ds.Tables[1].Rows.Add(ds.Tables[1].NewRow());
                        }
                        */

                        ds.Tables[0].Columns.Add("CurrDate_TH"); 

                        int dateYear = DateTime.Today.Year;
                        if (dateYear < 2500)
                            dateYear += 543;
                        ds.Tables[0].Rows[0]["CurrDate_TH"] = DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + dateYear.ToString();

                        ds.Tables[0].Columns.Add("Request_Date_TH");
                        if (ds.Tables[0].Rows[0]["Request_Date"].ToString().Length > 0)
                        {
                            dateYear = ((DateTime)ds.Tables[0].Rows[0]["Request_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Request_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Request_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Request_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }


                        ds.Tables[2].Columns.Add("Percent_Allocate_TH");
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            if (ds.Tables[2].Rows[i]["Percent_Allocate"].ToString().Length > 0)
                                ds.Tables[2].Rows[i]["Percent_Allocate_TH"] = ((decimal)ds.Tables[2].Rows[i]["Percent_Allocate"]).ToString("0.00") + " %";
                        }
                        for (int i = ds.Tables[2].Rows.Count; i < 4; i++)
                        {
                            ds.Tables[2].Rows.Add(ds.Tables[2].NewRow());
                        }
                        


                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MainDataSet", ds.Tables[0]));
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemDataSet", ds.Tables[1]));
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("Form2DataSet", ds.Tables[2]));

                        ReportViewer1.Visible = true;
                    }
                }

            }
        }

    }
}