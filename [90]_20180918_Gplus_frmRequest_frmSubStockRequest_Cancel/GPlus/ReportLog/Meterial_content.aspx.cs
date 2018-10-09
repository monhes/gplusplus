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
    public partial class Meterial_content : Pagebase
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
                DataTable dt = Material();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Material", dt);

                foreach (DataRow dr in dt.Rows)
                {
                    string Cate_Code_Err = dr.Field<string>("Cate_Code_Err");
                    string Type_code_Err = dr.Field<string>("Type_code_Err");
                    string SubCate_Code_Err = dr.Field<string>("SubCate_Code_Err");
                    
                    string stats = dr.Field<string>("Status");

                    string status_out = dr.Field<string>("progress");

                    Cate_Code_Err = (Cate_Code_Err != "") ? Cate_Code_Err = "[รหัสประเภทอุปกรณ์: " + Cate_Code_Err + "]@" : Cate_Code_Err = "";
                    Type_code_Err = (Type_code_Err != "") ? Type_code_Err = "[รหัสกลุ่มผู้ใช้งาน: " + Type_code_Err + "]@" : Type_code_Err = "";
                    SubCate_Code_Err = (SubCate_Code_Err != "") ? SubCate_Code_Err = "[ประเภทอุปกรณ์ย่อย: " + SubCate_Code_Err + "]@" : SubCate_Code_Err = "";
                    


                    stats = (stats != "") ? stats = "[สถานะข้อมูล: " + stats + "]" : stats = "";

                    status_out = (Cate_Code_Err == "" && Type_code_Err == "" && SubCate_Code_Err == "" &&  stats != "C") ? "เพิ่มสำเร็จ" : Cate_Code_Err + Type_code_Err + SubCate_Code_Err + stats;
                    status_out = status_out.Replace("@", "" + System.Environment.NewLine);
                    dr.SetField("progress", status_out);

                }


                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Material.rdlc");
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
            ReportViewer1.Visible = true;
        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            D5.SelectedIndex = 0;
            CalendarControl1.Text = "";
            CalendarControl2.Text = "";
            txt_po5.Text = "";

        }

        private DataTable Material()
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

            if (D5.SelectedIndex == 1)
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

            if (!string.IsNullOrEmpty(txt_po5.Text.Trim()))
                param.Add(new SqlParameter("@MALT", txt_po5.Text.Trim()));

           

           param.Add(new SqlParameter("@Err", RadioButtonList1.SelectedValue.ToString()));

            return db.ExecuteDataTable("sp_Logfile_Material_Select", param);
        }
    }
}