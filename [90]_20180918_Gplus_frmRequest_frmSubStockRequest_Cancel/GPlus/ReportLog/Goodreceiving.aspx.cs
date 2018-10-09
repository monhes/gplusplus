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
    public partial class Goodreceiving : Pagebase
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
                DataTable dt = GoodReceiving();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("GoodReceiving", dt);

                foreach (DataRow dr in dt.Rows)
                {
                   
                    string PO_NUM_Err = dr.Field<string>("PO_NUM_Err");
                    string MATL_CODE_Err = dr.Field<string>("MATL_CODE_Err");
                    string PACKAGE_NAME_Err = dr.Field<string>("PACKAGE_NAME_Err");
                    string Unit_Price_Err = dr.Field<string>("Unit_Price_Err");
                    string Receive_Qty_err = dr.Field<string>("Receive_Qty_err");
                    string TradeDiscount_Percent_err = dr.Field<string>("TradeDiscount_Percent_err");
                    string TradeDiscount_Price_err = dr.Field<string>("TradeDiscount_Price_err");
                    string CashDiscount_Percent_err = dr.Field<string>("CashDiscount_Percent_err"); 
                    string CashDiscount_Price_err = dr.Field<string>("CashDiscount_Price_err");
                    string Total_before_Vat_err = dr.Field<string>("Total_before_Vat_err");
                    string Vat_err = dr.Field<string>("Vat_err");
                    string Vat_Amount_err = dr.Field<string>("Vat_Amount_err");
                    string Net_Amount_err = dr.Field<string>("Net_Amount_err");
                    string GiveAway_QTY_err = dr.Field<string>("GiveAway_QTY_err");
                    string status_out = dr.Field<string>("status");

                    string all_err = null;

                    PO_NUM_Err = (PO_NUM_Err != "") ? PO_NUM_Err = "[เลขที่PO: " + PO_NUM_Err + "]@" : PO_NUM_Err = "";
                    MATL_CODE_Err = (MATL_CODE_Err != "") ? MATL_CODE_Err = "[รหัสสินค้า: " + MATL_CODE_Err + "]@" : MATL_CODE_Err = "";
                    PACKAGE_NAME_Err = (PACKAGE_NAME_Err != "") ? PACKAGE_NAME_Err = "[หน่วยนับ: " + PACKAGE_NAME_Err + "]@" : PACKAGE_NAME_Err = "";
                    Unit_Price_Err = (Unit_Price_Err != "") ? Unit_Price_Err = "[ราคาต่อหน่วย: " + Unit_Price_Err + "]@" : Unit_Price_Err = "";
                    Receive_Qty_err = (Receive_Qty_err != "") ? Receive_Qty_err = "[จำนวนที่รับตามPO: " + Receive_Qty_err + "]@" : Receive_Qty_err = "";
                    TradeDiscount_Percent_err = (TradeDiscount_Percent_err != "") ? TradeDiscount_Percent_err = "[ส่วนลดการค้า(เปอร์เซ็น): " + TradeDiscount_Percent_err + "]@" : TradeDiscount_Percent_err = "";
                    TradeDiscount_Price_err = (TradeDiscount_Price_err != "") ? TradeDiscount_Price_err = "[ส่วนลดการค้า(จำนวนเงิน): " + TradeDiscount_Price_err + "]@" : TradeDiscount_Price_err = "";
                    CashDiscount_Percent_err = (CashDiscount_Percent_err != "") ? CashDiscount_Percent_err = "[ส่วนลดเงินสด(เปอร์เซ็น): " + CashDiscount_Percent_err + "]@" : CashDiscount_Percent_err = "";
                    CashDiscount_Price_err = (CashDiscount_Price_err != "") ? CashDiscount_Price_err = "[ส่วนลดเงินสด(จำนวนเงิน): " + CashDiscount_Price_err + "]@" : CashDiscount_Price_err = "";
                    Total_before_Vat_err = (Total_before_Vat_err != "") ? Total_before_Vat_err = "[ราคาก่อนVAT: " + Total_before_Vat_err + "]@" : Total_before_Vat_err = "";
                    Vat_err = (Vat_err != "") ? Vat_err = "[ภาษีมูลค่าเพิ่ม(เปอร์เซ็น): " + Vat_err + "]@" : Vat_err = "";
                    Vat_Amount_err = (Vat_Amount_err != "") ? Vat_Amount_err = "[ภาษีมูลค่าเพิ่ม(จำนวนเงิน): " + Vat_Amount_err + "]@" : Vat_Amount_err = "";
                    Net_Amount_err = (Net_Amount_err != "") ? Net_Amount_err = "[รวมเงิน: " + Net_Amount_err + "]@" : Net_Amount_err = "";
                    GiveAway_QTY_err = (GiveAway_QTY_err != "") ? GiveAway_QTY_err = "[ของแถม: " + GiveAway_QTY_err + "]@" : GiveAway_QTY_err = "";

                    status_out = (status_out != "") ? status_out = "[สถานะข้อมูล: " + status_out + "]" : status_out = "";



                    all_err = (PO_NUM_Err == "" && MATL_CODE_Err == "" && PACKAGE_NAME_Err == "" && Unit_Price_Err == "" && Receive_Qty_err == "" && TradeDiscount_Percent_err == "" && TradeDiscount_Price_err == "" && CashDiscount_Percent_err == "" &&
                                CashDiscount_Price_err == "" && Total_before_Vat_err == "" && Vat_err == "" && Vat_Amount_err == "" && Net_Amount_err == "" && GiveAway_QTY_err == "" && status_out == "") ? "เพิ่มสำเร็จ" 
                                : PO_NUM_Err + MATL_CODE_Err + PACKAGE_NAME_Err + Unit_Price_Err + Receive_Qty_err + TradeDiscount_Percent_err+ TradeDiscount_Price_err+ CashDiscount_Percent_err+
                                CashDiscount_Price_err+ Total_before_Vat_err+ Vat_err+ Vat_Amount_err+ Net_Amount_err+ GiveAway_QTY_err+ status_out;

                    all_err = all_err.Replace("@", "" + System.Environment.NewLine);

                    dr.SetField("status", all_err);

                }


                ReportViewer1.LocalReport.ReportPath = Server.MapPath("GoodReceiving.rdlc");
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
            ReportViewer1.Visible = true;
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            CalendarControl2.Text = "";
                CalendarControl1.Text = "";
            txt_ep4.Text = "";
                txt_po4.Text = "";
            D4.SelectedIndex = 0;
        }

        private DataTable GoodReceiving()
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

            if (D4.SelectedIndex == 1)
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

            if (!string.IsNullOrEmpty(txt_po4.Text.Trim()))
                param.Add(new SqlParameter("@PO_CODE", txt_po4.Text.Trim()));

            if (!string.IsNullOrEmpty(txt_ep4.Text.Trim()))
                param.Add(new SqlParameter("@EP_CODE", txt_ep4.Text.Trim()));

            //if (RadioButtonList1.SelectedIndex == 0) { param.Add(new SqlParameter("@status", "สำเร็จ")); }
            //else if (RadioButtonList1.SelectedIndex == 1) { param.Add(new SqlParameter("@status", "ผิดพลาด")); }
            //else { }

            param.Add(new SqlParameter("@Err", RadioButtonList1.SelectedValue.ToString()));

            // return db.ExecuteDataTable("sp_Logfile_GoodReceiving_Select", param);
            return db.ExecuteDataTable("sp_Logfile_GoodReceiving_Select", param);
        }
    }
}