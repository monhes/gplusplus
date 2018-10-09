using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace GPlus.PRPO
{
    public partial class pop_POPrintMainReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null)
                {
                    DataSet ds = new DataAccess.PRDAO().GetPOReport(Request["id"]);

                    DataTable dt = new DataAccess.PODAO2().GetRefPR(Convert.ToInt32(Request["id"]));

                    string refPr = "";
                    string PRMore3 = "False";
                    if (dt.Rows.Count > 0 && dt.Rows.Count <= 3) // แสดง Ref PR เฉพาะกรณีที่ PR น้อยกว่า 3 ใบ ถ้ามากกว่าให้พิมพ์ใบแนบ PR แทน
                    {
                        foreach (DataRow row in dt.Rows)
                            refPr += row["PR_Code"].ToString() + ", ";

                        if (!string.IsNullOrEmpty(refPr))
                            refPr = refPr.Remove(refPr.Length - 2);
                    }
                    else if (dt.Rows.Count > 3) 
                    {
                        PRMore3 = "True";
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("PO_Date_TH");
                        ds.Tables[0].Columns.Add("Shipping_Date_TH");
                        ds.Tables[0].Columns.Add("Ref_PR");
                        ds.Tables[0].Columns.Add("PRMore3");

                        if (ds.Tables[0].Rows[0]["PO_Type"].ToString() == "2")
                            ReportViewer1.LocalReport.ReportPath = "PRPO\\POMainReport2.rdlc";

                        ds.Tables[0].Rows[0]["Net_Amount_TH"] = Util.ThaiBaht(ds.Tables[0].Rows[0]["Net_Amonut"].ToString());

                        if (ds.Tables[0].Rows[0]["PO_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["PO_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["PO_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        if (ds.Tables[0].Rows[0]["Shipping_Date"].ToString().Length > 0)
                        {
                            int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Year;
                            if (dateYear < 2500)
                                dateYear += 543;
                            ds.Tables[0].Rows[0]["Shipping_Date_TH"] = ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Day.ToString() + "/" +
                                ((DateTime)ds.Tables[0].Rows[0]["Shipping_Date"]).Month.ToString() + "/" + dateYear.ToString();
                        }

                        if (ds.Tables[0].Rows[0]["PO_Type"].ToString() != "2")
                        {
                            for (int i = ds.Tables[1].Rows.Count; i < 12; ++i)
                            {
                                ds.Tables[1].Rows.Add(ds.Tables[1].NewRow());
                            }
                        }
                            for (int i = 1; i < ds.Tables[2].Rows.Count; i++)
                            {
                                ds.Tables[2].Rows[0]["PR_Code"] += ", " + ds.Tables[2].Rows[i]["PR_Code"].ToString();
                            }

                            ds.Tables[0].Rows[0]["Ref_PR"] = refPr;
                            ds.Tables[0].Rows[0]["PRMore3"] = PRMore3;

                            ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MainDataSet", ds.Tables[0]));
                            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("ItemDataSet", ds.Tables[1]));
                            ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("PRDataSet", ds.Tables[2]));

                            ReportViewer1.Visible = true;
                        }
                    }

                }
            }

        }

}