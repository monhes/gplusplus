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
    public partial class Stock_Issue_content : Pagebase
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
                DataTable dt = Stock_IS();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Stock_Issue", dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string Request_No_Err = dr.Field<string>("Request_No_Err");
                    string Request_Date_Err = dr.Field<string>("Request_Date_Err");
                    string Request_type_Err = dr.Field<string>("Request_type_Err");
                    string Div_code_Err = dr.Field<string>("Div_code_Err");
                    string Dep_code_Err = dr.Field<string>("Dep_code_Err");
                    string Stock_code_Err = dr.Field<string>("Stock_code_Err");
                    string Request_By_Err = dr.Field<string>("Request_By_Err");
                    string Request_Name_Err = dr.Field<string>("Request_Name_Err");
                    string MATL_CODE_Err = dr.Field<string>("MATL_CODE_Err");
                    string PACKAGE_NAME_Err = dr.Field<string>("PACKAGE_NAME_Err");
                    string Order_Quantity_Err = dr.Field<string>("Order_Quantity_Err");
                    string Status = dr.Field<string>("Status");


                    string all_err = null;

                    Request_No_Err = (Request_No_Err != "") ? Request_No_Err = "[เลขที่ใบเบิก: " + Request_No_Err + "]@" : Request_No_Err = "";
                    Request_Date_Err = (Request_Date_Err != "") ? Request_Date_Err = "[วันที่เบิก: " + Request_Date_Err + "]@" : Request_Date_Err = "";
                    Request_type_Err = (Request_type_Err != "") ? Request_type_Err = "[ประเภทการเบิก: " + Request_type_Err + "]@" : Request_type_Err = "";
                    Div_code_Err = (Div_code_Err != "") ? Div_code_Err = "[รหัสฝ่าย: " + Div_code_Err + "]@" : Div_code_Err = "";
                    Dep_code_Err = (Dep_code_Err != "") ? Dep_code_Err = "[รหัสทีม: " + Dep_code_Err + "]@" : Dep_code_Err = "";
                    Stock_code_Err = (Stock_code_Err != "") ? Stock_code_Err = "[คลัง: " + Stock_code_Err + "]@" : Stock_code_Err = "";
                    Request_By_Err = (Request_By_Err != "") ? Request_By_Err = "[รหัสผนักงาน: " + Request_By_Err + "]@" : Request_By_Err = "";
                    Request_Name_Err = (Request_Name_Err != "") ? Request_Name_Err = "[ชื่อผนักงาน: " + Request_Name_Err + "]@" : Request_Name_Err = "";
                    MATL_CODE_Err = (MATL_CODE_Err != "") ? MATL_CODE_Err = "[รหัสวัสดุอุปกรณ์: " + MATL_CODE_Err + "]@" : MATL_CODE_Err = "";
                    PACKAGE_NAME_Err = (PACKAGE_NAME_Err != "") ? PACKAGE_NAME_Err = "[หน่วยนับ: " + PACKAGE_NAME_Err + "]@" : PACKAGE_NAME_Err = "";
                    Order_Quantity_Err = (Order_Quantity_Err != "") ? Order_Quantity_Err = "[จำนวนที่เบิก: " + Order_Quantity_Err + "]@" : Order_Quantity_Err = "";
                    Status = (Status != "") ? Status = "[สถานะข้อมูล: " + Status + "]@" : Status = "";

                    all_err = (Request_No_Err == "" && Request_Date_Err == "" && Request_type_Err == "" && Div_code_Err == "" && Dep_code_Err == "" && Stock_code_Err == "" && Request_By_Err == "" &&
                        Request_Name_Err == "" && MATL_CODE_Err == "" && PACKAGE_NAME_Err == "" && Order_Quantity_Err == "" && Status == "") ? "สำเร็จ" :
                    Request_No_Err + Request_Date_Err + Request_type_Err + Div_code_Err + Dep_code_Err + Stock_code_Err + Request_By_Err + Request_Name_Err + MATL_CODE_Err + PACKAGE_NAME_Err + Order_Quantity_Err + Status;
                    all_err = all_err.Replace("@", "" + System.Environment.NewLine);
                    dr.SetField("Status", all_err);

                    
                }



                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Stock_Issue.rdlc");
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
            ReportViewer1.Visible = true;
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            CalendarControl1.Text = "";
            CalendarControl2.Text = "";
            CalendarControl3.Text = "";
            CalendarControl4.Text = "";
            Text_Req_No1.Text = "";
            Text_Matl_Code1.Text = "";
            D1.SelectedIndex = 0;
        }

        private DataTable Stock_IS()
        {
            List<SqlParameter> param = new List<SqlParameter>();
            string[] as_date = new string[2];
            string[] ae_date = new string[2];
            string[] rs_date = new string[2];
            string[] re_date = new string[2];

            string start_date = "";
            string end_date = "";

            string req_start_date = "";
            string req_end_date = ""; 
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();

            DateTime dt3 = new DateTime();
            DateTime dt4 = new DateTime();

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

            if (DateTime.TryParse(CalendarControl3.Text.Trim(), out dt3))
            {
                rs_date = CalendarControl3.Text.Trim().Split('/');
                req_start_date = int.Parse(rs_date[2]) + "-" + rs_date[1] + "-" + rs_date[0];
            }

            if (DateTime.TryParse(CalendarControl4.Text.Trim(), out dt4))
            {
                re_date = CalendarControl4.Text.Trim().Split('/');
                req_end_date = int.Parse(re_date[2]) + "-" + re_date[1] + "-" + re_date[0];
            }

            if (!string.IsNullOrEmpty(req_start_date))
            {
                param.Add(new SqlParameter("@Req_date_Start", req_start_date.Trim()));
            }
            if (!string.IsNullOrEmpty(req_end_date))
            {
                param.Add(new SqlParameter("@Req_date_End", req_end_date.Trim()));
            }

            if (D1.SelectedIndex == 1)
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

            if (!string.IsNullOrEmpty(Text_Req_No1.Text.Trim()))
                param.Add(new SqlParameter("@Request_No", Text_Req_No1.Text.Trim()));

            if (!string.IsNullOrEmpty(Text_Matl_Code1.Text.Trim()))
                param.Add(new SqlParameter("@MATL_CODE", Text_Matl_Code1.Text.Trim()));

         

            param.Add(new SqlParameter("@Err", RadioButtonList1.SelectedValue.ToString()));
            


            return db.ExecuteDataTable("sp_Logfile_Stock_Issue_Select", param);
        }
    }
}