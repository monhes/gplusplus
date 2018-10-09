using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using GPlus.DataAccess;

namespace GPlus.PRPO
{
    public partial class TempRPT_Routine : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataSetReprot2 ds1 = (DataSetReprot2)this.Session["dtResultAll"];

                if (Request.QueryString["id"] != null)
                {
                    hid_SummaryReq_id.Value = Request.QueryString["id"];
                }


                if (ds1 != null && ds1.DataReprot.Rows.Count > 0 && !string.IsNullOrWhiteSpace(ds1.DataReprot.Rows[0]["Description13"].ToString()))
                {

                    ReportDocument rpt = new ReportDocument();
                    rpt.Load(Server.MapPath("~//PRPO//Report_Test.rpt"));
                    rpt.SetDataSource(this.Session["dtResultAll"]);
                    this.CrystalReportViewer1.ReportSource = rpt;
                    rpt.SetParameterValue("document", "ชุดที่ 1/2");


                    ReportDocument rpt2 = new ReportDocument();
                    rpt2.Load(Server.MapPath("~//PRPO//Report_Test_2.rpt"));
                    rpt2.SetDataSource(this.Session["dtResultAll"]);
                    this.CrystalReportViewer2.ReportSource = rpt2;
                    rpt2.SetParameterValue("document", "ชุดที่ 2/2");

                    CrystalReportViewer2.Visible = true;


                    
                    
                    CrystalReportViewer1.ReportSource = rpt;
                    Session["ReportDocument1"] = rpt;


                    CrystalReportViewer2.ReportSource = rpt2;
                    Session["ReportDocument2"] = rpt2;


                }
                else
                {
                    ReportDocument rpt = new ReportDocument();

                    rpt.Load(Server.MapPath("~//PRPO//Report_Test_3.rpt"));
                    rpt.SetDataSource(this.Session["dtResultAll"]);
                    this.CrystalReportViewer1.ReportSource = rpt;
                    rpt.SetParameterValue("document", "ชุดที่ 1/1");

                    CrystalReportViewer2.Visible = false;

                    CrystalReportViewer1.ReportSource = rpt;
                    Session["ReportDocument1"] = rpt;
                }


                //Page.RegisterClientScriptBlock("repage", "window.opener.location = 'RoutineStock.aspx'");

                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "repage", "window.opener.location = 'RoutineStock.aspx';", true);

            }
            else
            {
                DataSetReprot2 ds1 = (DataSetReprot2)this.Session["dtResultAll"];

                if (ds1 != null && ds1.DataReprot.Rows.Count > 0 && !string.IsNullOrWhiteSpace(ds1.DataReprot.Rows[0]["Description13"].ToString()))
                {
                    ReportDocument doc = (ReportDocument)Session["ReportDocument1"];
                    CrystalReportViewer1.ReportSource = doc;

                    ReportDocument doc2 = (ReportDocument)Session["ReportDocument2"];
                    CrystalReportViewer2.ReportSource = doc2;

                }
                else
                {
                    ReportDocument doc = (ReportDocument)Session["ReportDocument1"];
                    CrystalReportViewer1.ReportSource = doc;
                }
            }
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string repName = null;
            ReportDocument report = new ReportDocument();
            repName = CrystalReportSource1.ReportDocument.FileName;

            report.Load(repName);
            report.SetDataSource(this.Session["dtResultAll"]);
            report.SetParameterValue("document", "ชุดที่ 1/2");
            report.PrintToPrinter(1, false, 0, 0);


            report = new ReportDocument();
            repName = CrystalReportSource2.ReportDocument.FileName;

            report.Load(repName);
            report.SetDataSource(this.Session["dtResultAll"]);
            report.SetParameterValue("document", "ชุดที่ 2/2");
            report.PrintToPrinter(1, false, 0, 0); 



            // Update Reprint

            //if (hid_SummaryReq_id.Value != "")
            //{

            //    List<SqlParameter> param = new List<SqlParameter>();

            //    param.Add(new SqlParameter("@Summary_ReqId", hid_SummaryReq_id.Value));

            //    param.Add(new SqlParameter("@Print_By", Session["UserID"].ToString()));


            //    int result = new DatabaseHelper().ExecuteNonQuery("sp_Inv_Update_Reprint_SummaryReq", param);
            //}

        }
    }
}
