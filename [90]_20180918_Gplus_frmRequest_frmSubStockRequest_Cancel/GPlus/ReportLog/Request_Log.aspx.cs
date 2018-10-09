using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.DataAccess;
using Microsoft.Reporting.WebForms;
using System.Data;

namespace GPlus.ReportLog
{
    public partial class Request_Log : Pagebase
    {
        DatabaseHelper db = new DatabaseHelper();



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

        }

        protected void btn_summit_Click(object sender, EventArgs e)
        {
            //string tab = Request.QueryString["tab"].ToString();
            string tab = "1";
            ReportDataSource data_list = new ReportDataSource();
           
            if (tab == "1")
            {
                DataTable dt = Close_IS();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Close_IS", dt);

                foreach(DataRow dr in dt.Rows)
                {
                    string Request_No_Err = dr.Field<string>("Request_No_Err");
                    string MATL_CODE_Err = dr.Field<string>("MATL_CODE_Err");
                    string PACKAGE_NAME_Err = dr.Field<string>("PACKAGE_NAME_Err");
                    string Return_Qty_Err = dr.Field<string>("Return_Qty_Err");
                    string status = dr.Field<string>("status");
                    
                    string all_err = null;

                    Request_No_Err = (Request_No_Err != "") ? Request_No_Err = "[เลขที่ใบเบิก: " + Request_No_Err + "]@" : Request_No_Err = "";
                    MATL_CODE_Err = (MATL_CODE_Err != "") ? MATL_CODE_Err = "[รหัสวัสดุอุปกรณ์: " + MATL_CODE_Err + "]@" : MATL_CODE_Err = "";
                    PACKAGE_NAME_Err = (PACKAGE_NAME_Err != "") ? PACKAGE_NAME_Err = "[หน่วยนับ: " + PACKAGE_NAME_Err + "]@" : PACKAGE_NAME_Err = "";
                    Return_Qty_Err = (Return_Qty_Err != "") ? Return_Qty_Err = "[จำนวนที่คืน: " + Return_Qty_Err + "]@" : Return_Qty_Err = "";
                    status = (status != "") ? status = "[สถานะข้อมูล: " + status + "]@" : status = "";

                    all_err = (Request_No_Err == "" && MATL_CODE_Err == "" && PACKAGE_NAME_Err == "" && Return_Qty_Err == "" && status == "" ) ? "สำเร็จ" :
                    Request_No_Err + MATL_CODE_Err+ PACKAGE_NAME_Err+ Return_Qty_Err+ status;
                    all_err = all_err.Replace("@", "" + System.Environment.NewLine);
                    dr.SetField("status", all_err);
                }
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Close_IS.rdlc");
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
            ReportViewer1.Visible = true;
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            CalendarControl1.Text = "";
            CalendarControl2.Text = "";
            D2.SelectedIndex = 0;
            Text_Req_No.Text = "";
                Text_Matl_Code.Text = "";
        }

        private DataTable Close_IS()
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
                start_date = int.Parse(as_date[2]) + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(CalendarControl2.Text.Trim(), out dt2))
            {
                ae_date = CalendarControl2.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) + "-" + ae_date[1] + "-" + ae_date[0];
            }

            if (D2.SelectedIndex == 1)
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

            if (!string.IsNullOrEmpty(Text_Req_No.Text.Trim()))
                param.Add(new SqlParameter("@Request_No", Text_Req_No.Text.Trim()));

            if (!string.IsNullOrEmpty(Text_Matl_Code.Text.Trim()))
                param.Add(new SqlParameter("@MATL_CODE", Text_Matl_Code.Text.Trim()));

            param.Add(new SqlParameter("@Err", RadioButtonList1.SelectedValue.ToString()));
            //if (RadioButtonList1.SelectedIndex == 0) { param.Add(new SqlParameter("@status", "สำเร็จ")); }
            //else if (RadioButtonList1.SelectedIndex == 1) { param.Add(new SqlParameter("@status", "ผิดพลาด")); }
            //else { }


            return db.ExecuteDataTable("sp_Logfile_Close_Stock_Issue_Select", param);
        }


    }
}