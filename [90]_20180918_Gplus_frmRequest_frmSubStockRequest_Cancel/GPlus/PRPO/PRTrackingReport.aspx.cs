using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

using Microsoft.Reporting.WebForms;

using GPlus.DataAccess;
using System.Data;

namespace GPlus.PRPO
{
    public partial class PRTrackingReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("OrgID = " + Convert.ToInt32(Session["OrgID"]).ToString());
            if (!IsPostBack)
            {
                PageID = "606";
                PRPOUtil.SetDivDep(ref txtDivDepName, Convert.ToInt32(Session["OrgID"].ToString()));
                ReportViewer1.Visible = false;

                ((Label)divdepCtrl.FindControl("lblDiv")).Text = "ชื่อฝ่ายที่ขอซื้อ";
                ((Label)divdepCtrl.FindControl("lblDep")).Text = "ชื่อทีมที่ขอซื้อ";
            }
        }

        private void BindData(string prCode = "", string dateFrom = "", string dateTo = "", 
                              string prType = "", string divName = "", string depName = "")
        {
            ReportViewer1.LocalReport.DataSources.Clear();

            // Prepare data to ReportViewer
            SQLParameterList sqlParamList = new SQLParameterList();
            sqlParamList.AddIntegerField("OrgStrucId", Convert.ToInt32(Session["OrgID"].ToString()));
            sqlParamList.AddStringField("PRCode", prCode);
            sqlParamList.AddStringField("DateFrom", dateFrom);
            sqlParamList.AddStringField("DateTo", dateTo);
            sqlParamList.AddStringField("PRType", prType);
            sqlParamList.AddStringField("DivName", divName);
            sqlParamList.AddStringField("DepName", depName);

            // Get PRTracking report
            DataSet ds = new PrPoDAO().GetPRTrackingReport(sqlParamList);

            // If have more than one row, Bind report data to ReportViewer
            if (ds.Tables[1].Rows.Count != 0)
            {
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PRTrackingReportDataSet", ds.Tables[1]));
                ReportViewer1.Visible = true;
            }
            else
            {
                ReportViewer1.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(Session["OrgID"].ToString());

            string prCode = ((TextBox) noPRPOCtrl.FindControl("txtNoPRPO")).Text;
            string dateFrom = ccFrom.Text;
            string dateTo = ccTo.Text;
            string prType = ddlPRType.SelectedValue;
            string divName = ((TextBox) divdepCtrl.FindControl("txtItemDivName")).Text;
            string depName = ((TextBox) divdepCtrl.FindControl("TxtItemDepName")).Text;

            Debug.WriteLine("prCode={0}, dateFrom={1}, dateTo={2}, prType={3}, divName={4}, depName={5}", 
                                prCode, dateFrom, dateTo, prType, divName, depName);

            BindData(prCode, dateFrom, dateTo, prType, divName, depName);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ccTo.Text = "";
            ccFrom.Text = "";
            ((TextBox) noPRPOCtrl.FindControl("txtNoPRPO")).Text = "";
            ((TextBox) divdepCtrl.FindControl("TxtItemDepName")).Text = "";

            string[] divdep = new PrPoDAO().GetDivDepName(Convert.ToInt32(Session["OrgID"].ToString()));

            if (divdep[0] == "ฝ่ายจัดซื้อ" || divdep[0] == "ฝ่ายธุรการ")
            {
                ((TextBox)divdepCtrl.FindControl("txtItemDivName")).Text = "";
            }

            ReportViewer1.Visible = false;
        }
    }
}