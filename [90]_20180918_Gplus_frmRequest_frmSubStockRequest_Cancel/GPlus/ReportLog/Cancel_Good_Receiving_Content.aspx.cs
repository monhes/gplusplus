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
    public partial class Cancel_Good_Receiving_Content : Pagebase
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
                DataTable dt = CANCELGOOD();



                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("CancelGood", dt);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("Supplier.rdlc");


                foreach (DataRow dr in dt.Rows)
                {

                     //RefCancel_EP_err
                     //PO_NUM_Err  
                     //Reference_EP_Err 
                     //MATL_CODE_Err  
                     //PACKAGE_NAME_Err 
                     //Return_QTY_Err 
                     //Return_GiveAway_QTY_err 
                     //status

                    string RefCancel_EP_err = dr.Field<string>("RefCancel_EP_err");
                    string PO_NUM_Err = dr.Field<string>("PO_NUM_Err");
                    string Reference_EP_Err = dr.Field<string>("Reference_EP_Err");
                    string MATL_CODE_Err = dr.Field<string>("MATL_CODE_Err");
                    string PACKAGE_NAME_Err = dr.Field<string>("PACKAGE_NAME_Err");
                    string Return_QTY_Err = dr.Field<string>("Return_QTY_Err");
                    string Return_GiveAway_QTY_err = dr.Field<string>("Return_GiveAway_QTY_err");
                    string status = dr.Field<string>("status");

                    RefCancel_EP_err = (RefCancel_EP_err != "") ? RefCancel_EP_err = "[เลขที่ยกเลิก: " + RefCancel_EP_err + "]@" : RefCancel_EP_err = "";
                    PO_NUM_Err = (PO_NUM_Err != "") ? PO_NUM_Err = "[เลขที่ใบสั่งซื้อ: " + PO_NUM_Err + "]@" : PO_NUM_Err = "";
                    Reference_EP_Err = (Reference_EP_Err != "") ? Reference_EP_Err = "[เลขที่รับอ้างอิงจากระบบ: " + Reference_EP_Err + "]@" : Reference_EP_Err = "";
                    MATL_CODE_Err = (MATL_CODE_Err != "") ? MATL_CODE_Err = "[รหัสสินค้า: " + MATL_CODE_Err + "]@" : MATL_CODE_Err = "";
                    PACKAGE_NAME_Err = (PACKAGE_NAME_Err != "") ? PACKAGE_NAME_Err = "[หน่วยนับ: " + PACKAGE_NAME_Err + "]@" : PACKAGE_NAME_Err = "";
                    Return_QTY_Err = (Return_QTY_Err != "") ? Return_QTY_Err = "[จำนวนที่ยกเลิกรับเข้าคลัง: " + Return_QTY_Err + "]@" : Return_QTY_Err = "";
                    Return_GiveAway_QTY_err = (Return_GiveAway_QTY_err != "") ? Return_GiveAway_QTY_err = "[ของแถม: " + Return_GiveAway_QTY_err + "]@" : Return_GiveAway_QTY_err = "";
                    status = (status != "") ? status = "[สถานะข้อมูล: " + status + "]@" : status = "";

                    status = (RefCancel_EP_err == "" &&
                     PO_NUM_Err == "" &&
                     Reference_EP_Err == "" &&
                     MATL_CODE_Err == "" &&
                     PACKAGE_NAME_Err == "" &&
                     Return_QTY_Err == "" &&
                     Return_GiveAway_QTY_err == "" &&
                     status == "") ? "เพิ่มสำเร็จ" :
                     RefCancel_EP_err +
                     PO_NUM_Err +
                     Reference_EP_Err + 
                     MATL_CODE_Err+ 
                     PACKAGE_NAME_Err +
                     Return_QTY_Err+
                     Return_GiveAway_QTY_err+
                     status;

                    status = status.Replace("@", "" + System.Environment.NewLine);
                    dr.SetField("status", status);


                    

                }

                ReportViewer1.LocalReport.ReportPath = "ReportLog/Cancel_Good_Receiving.rdlc";   
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
            ReportViewer1.Visible = true;
        }
        protected void btn_clear_Click(object sender, EventArgs e)
        {
            D7.SelectedIndex = 0;
            CalendarControl1.Text = "";
            CalendarControl2.Text = "";
            TEXT_EP.Text = "";
            TEXT_PO.Text = "";
        }

        private DataTable CANCELGOOD()
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

            if (D7.SelectedIndex == 1)
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

            if (!string.IsNullOrEmpty(TEXT_PO.Text.Trim()))
                param.Add(new SqlParameter("@PO_NUM", TEXT_PO.Text.Trim()));

            if (!string.IsNullOrEmpty(TEXT_EP.Text.Trim()))
                param.Add(new SqlParameter("@EP_REF", TEXT_EP.Text.Trim()));

            param.Add(new SqlParameter("@Err", RadioButtonList1.SelectedValue.ToString()));

            return db.ExecuteDataTable("sp_Logfile_Cancel_Good_Receiving_Select", param);
        }
    }
}