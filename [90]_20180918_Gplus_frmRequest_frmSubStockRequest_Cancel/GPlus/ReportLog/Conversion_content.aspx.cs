using GPlus.DataAccess;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.ReportLog
{
    public partial class Conversion_content : Pagebase
    {
        DatabaseHelper db = new DatabaseHelper();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btn_summit_Click(object sender, EventArgs e)
        {
            //string tab = Request.QueryString["tab"].ToString();
            string tab = "1";
            ReportDataSource data_list = new ReportDataSource();

            if (tab == "1")
            {
                DataTable dt = Conversion();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Conversion", dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string MATL_CODE_err = dr.Field<string>("MATL_CODE_err");
                    string PackageName_err = dr.Field<string>("PackageName_err");
                    string Pack_Content_err = dr.Field<string>("Pack_Content_err");
                    string PackName_Base_err = dr.Field<string>("PackName_Base_err");
                    string stats = dr.Field<string>("Status");
                    
                    string status_out = dr.Field<string>("Supplier_Err");

                    MATL_CODE_err = (MATL_CODE_err != "") ? MATL_CODE_err = "[รหัสวัสดุอุปกรณ์: " + MATL_CODE_err + "]@" : MATL_CODE_err = "";
                    PackageName_err = (PackageName_err != "") ? PackageName_err = "[หน่วยนับ: " + PackageName_err + "]@" : PackageName_err = "";
                    Pack_Content_err = (Pack_Content_err != "") ? Pack_Content_err = "[จำนวน: " + Pack_Content_err + "]@" : Pack_Content_err = "";
                    PackName_Base_err = (PackName_Base_err != "") ? PackName_Base_err = "[หน่วยย่อย: " + PackName_Base_err + "]@" : PackName_Base_err = "";


                    stats = (stats != "") ? stats = "[สถานะข้อมูล: " + stats + "]" : stats = "";

                    status_out = (MATL_CODE_err == "" && PackageName_err == "" && Pack_Content_err == "" && PackName_Base_err == "" && stats != "C") ? "เพิ่มสำเร็จ" : MATL_CODE_err + PackageName_err + Pack_Content_err + PackName_Base_err + stats;
                    status_out = status_out.Replace("@", "" + System.Environment.NewLine);
                    dr.SetField("Supplier_Err", status_out);

                }


                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Conversion.rdlc");
                ReportViewer1.Visible = true;
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            CalendarControl1.Text = "";
            CalendarControl2.Text = "";
                D6.SelectedIndex = 0;
            txt_MaterialCode.Text = "";
        }

        private DataTable Conversion()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            string[] as_date = new string[2];
            string[] ae_date = new string[2];
            string start_date = "";
            string end_date = "";
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();

            if (DateTime.TryParse(CalendarControl1.Text.Trim(), out dt1))
            {
                as_date = CalendarControl1.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2])  + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(CalendarControl2.Text.Trim(), out dt2))
            {
                ae_date = CalendarControl2.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2])  + "-" + ae_date[1] + "-" + ae_date[0];
            }

            if (D6.SelectedIndex == 1)
            {
                if (!string.IsNullOrEmpty(start_date))
                {
                    param.Add(new SqlParameter("@Process_date_Start", start_date.Trim()));
                }
                if (!string.IsNullOrEmpty(end_date))
                {
                    param.Add(new SqlParameter("@Process_date_End", end_date.Trim()));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(start_date))
                {
                    param.Add(new SqlParameter("@Transfer_date_Start", start_date.Trim()));
                }
                if (!string.IsNullOrEmpty(end_date))
                {
                    param.Add(new SqlParameter("@Transfer_date_End", end_date.Trim()));
                }
            }

            if (!string.IsNullOrEmpty(txt_MaterialCode.Text.Trim()))
                param.Add(new SqlParameter("@Materia_CODE", txt_MaterialCode.Text.Trim()));

            //if (RadioButtonList1.SelectedIndex == 0) { param.Add(new SqlParameter("@status", "สำเร็จ")); }
            //else if (RadioButtonList1.SelectedIndex == 1) { param.Add(new SqlParameter("@status", "ผิดพลาด")); }
            //else { }

            param.Add(new SqlParameter("@Err", RadioButtonList1.SelectedValue.ToString()));

            return db.ExecuteDataTable("sp_Logfile_Conversion_Select", param);
        }
    }
}