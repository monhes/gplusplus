using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using GPlus.DataAccess;
using System.Diagnostics;
using System.Data;

using Microsoft.Reporting.WebForms;

namespace GPlus.PRPO
{
    public partial class POTrackingReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageID = "607";
                PRPOUtil.SetDivDep(ref txtDivDepName, Convert.ToInt32(Session["OrgID"].ToString()));

                ((Label)divdepCtrl.FindControl("lblDiv")).Text = "ชื่อฝ่ายที่ขอซื้อ";
                ((Label)divdepCtrl.FindControl("lblDep")).Text = "ชื่อทีมที่ขอซื้อ";

                ReportViewer1.Visible = false;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string poCode = ((TextBox)noPOCtrl.FindControl("txtItemNoPO")).Text;
            string dateFrom = ccFrom.Text;
            string dateTo = ccTo.Text;
            string poType = ddlPOType.SelectedValue;
            string divName = ((TextBox)divdepCtrl.FindControl("txtItemDivName")).Text;
            string depName = ((TextBox)divdepCtrl.FindControl("TxtItemDepName")).Text;
            string supplier = ((TextBox)supplierCtrl.FindControl("txtItemSupplierName")).Text;

            BindData(poCode, dateFrom, dateTo, poType, supplier, divName, depName);
        }

        private void BindData(
            string poCode = "", string dateFrom = "", 
            string dateTo = "", string poType = "", string supplier = "", 
            string divName = "", string depName = ""
        )
        {
            ReportViewer1.LocalReport.DataSources.Clear();

            SQLParameterList sqlParamList = new SQLParameterList();
            sqlParamList.AddIntegerField("OrgStrucId", Convert.ToInt32(Session["OrgID"].ToString()));
            sqlParamList.AddStringField("POCode", poCode);
            sqlParamList.AddStringField("DateFrom", dateFrom);
            sqlParamList.AddStringField("DateTo", dateTo);
            sqlParamList.AddStringField("POType", poType);
            sqlParamList.AddStringField("Supplier", supplier);
            sqlParamList.AddStringField("DivName", divName);
            sqlParamList.AddStringField("DepName", depName);

            DataSet ds = new PrPoDAO().GetPOTrackingReport(sqlParamList);

            if (ds.Tables[1].Rows.Count != 0)
            {
                ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("POTrackingReportDataSet", ds.Tables[1]));
                ReportViewer1.Visible = true;
            }
            else
            {
                ReportViewer1.Visible = false;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ccTo.Text = "";
            ccFrom.Text = "";
            ((TextBox)noPOCtrl.FindControl("txtItemNoPO")).Text = "";
            ((TextBox)divdepCtrl.FindControl("TxtItemDepName")).Text = "";
            ((TextBox)supplierCtrl.FindControl("txtItemSupplierName")).Text = "";

            string[] divdep = new PrPoDAO().GetDivDepName(Convert.ToInt32(Session["OrgID"].ToString()));
            if (divdep[0] == "ฝ่ายจัดซื้อ" || divdep[0] == "ฝ่ายธุรการ")
            {
                ((TextBox)divdepCtrl.FindControl("txtItemDivName")).Text = "";
            }

            ReportViewer1.Visible = false;
        }
    }
}