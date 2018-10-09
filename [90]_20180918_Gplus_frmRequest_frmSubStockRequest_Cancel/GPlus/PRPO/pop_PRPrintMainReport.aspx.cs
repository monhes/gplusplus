using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PRPrintMainReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null)
                {
                    DataSet ds = new DataAccess.PRDAO().GetPRReport(Request["id"]);
                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[0].Rows[0]["PR_Type"].ToString() == "2")
                        {
                            ReportViewer1.LocalReport.ReportPath = "PRPO\\PRMainReport2.rdlc";
                        }
                        ds.Tables[0].Columns.Add("Net_Amount_TH");

                        ds.Tables[0].Rows[0]["Net_Amount_TH"] = Util.ThaiBaht(ds.Tables[0].Rows[0]["Net_Amonut"].ToString());

                        if (ds.Tables[0].Rows[0]["PR_Type"].ToString() != "2")
                        {
                            for (int i = ds.Tables[1].Rows.Count; i < 8; i++)
                            {
                                ds.Tables[1].Rows.Add(ds.Tables[1].NewRow());
                            }
                        }
                        if (ds.Tables[0].Rows[0]["PR_Type"].ToString() == "2")
                        {
                            for (int i = ds.Tables[1].Rows.Count; i < 5; i++)
                            {
                                ds.Tables[1].Rows.Add(ds.Tables[1].NewRow());
                            }
                        }


                        ds.Tables[0].Columns.Add("Request_Date_TH");
                        if (ds.Tables[0].Rows[0]["Request_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Request_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Request_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Request_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Request_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        ds.Tables[0].Columns.Add("Rev");
                        ds.Tables[0].Rows[0]["Rev"] = "Rev2";
                        if (ds.Tables[0].Rows[0]["PR_Type"].ToString() == "1")
                        {
                            string firstDigit = "";
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                firstDigit = ds.Tables[1].Rows[0]["Inv_ItemCode"].ToString().Substring(0, ds.Tables[1].Rows[0]["Inv_ItemCode"].ToString().IndexOf("-"));
                                if (firstDigit == "2" || firstDigit == "6" || firstDigit == "8")
                                {
                                    ds.Tables[0].Rows[0]["Rev"] = "Rev1";
                                }
                            }
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