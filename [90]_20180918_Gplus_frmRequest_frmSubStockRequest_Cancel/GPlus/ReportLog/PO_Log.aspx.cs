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
    public partial class PO_Log : Pagebase
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
                DataTable dt = Logfile_PO();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Log_PO", dt);


                foreach (DataRow dr in dt.Rows)
                {
                    string PO_DATE_Err = dr.Field<string>("PO_DATE_Err");
                    string PO_Type_Err = dr.Field<string>("PO_Type_Err");
                    string Div_Code_Err = dr.Field<string>("Div_Code_Err");
                    string Dep_Code_ERR = dr.Field<string>("Dep_Code_ERR");
                    string SUPPLIER_CODE_ERR = dr.Field<string>("SUPPLIER_CODE_ERR");
                    string Payment_type_Err = dr.Field<string>("Payment_type_Err");
                    string CreditTerm_Err = dr.Field<string>("CreditTerm_Err");
                    string MATL_CODE_Err = dr.Field<string>("MATL_CODE_Err");
                    string PACKAGE_NAME_Err = dr.Field<string>("PACKAGE_NAME_Err");
                    string Unit_Price_Err = dr.Field<string>("Unit_Price_Err");
                    string Order_Qty_Err = dr.Field<string>("Order_Qty_Err");
                    string TradeDiscount_Percent_Err = dr.Field<string>("TradeDiscount_Percent_Err");
                    string TradeDiscount_Amount_Err = dr.Field<string>("TradeDiscount_Amount_Err");
                    string Total_before_Vat_Err = dr.Field<string>("Total_before_Vat_Err");
                    string Vat_Err = dr.Field<string>("Vat_Err");
                    string Vat_Amount_Err = dr.Field<string>("Vat_Amount_Err");
                    string Net_Amount_Err = dr.Field<string>("Net_Amount_Err");
                    string status = dr.Field<string>("status");
                    
                    string all_err = null;

                    PO_DATE_Err = (PO_DATE_Err != "") ? PO_DATE_Err = "[รหัสประเภทอุปกรณ์: " + PO_DATE_Err + "]@" : PO_DATE_Err = "";
                    PO_Type_Err = (PO_Type_Err != "") ? PO_Type_Err = "[ประเภท: " + PO_Type_Err + "]@" : PO_Type_Err = "";
                    Div_Code_Err = (Div_Code_Err != "") ? Div_Code_Err = "[รหัสฝ่าย: " + Div_Code_Err + "]@" : Div_Code_Err = "";
                    Dep_Code_ERR = (Dep_Code_ERR != "") ? Dep_Code_ERR = "[รหัสทีม: " + Dep_Code_ERR + "]@" : Dep_Code_ERR = "";
                    SUPPLIER_CODE_ERR = (SUPPLIER_CODE_ERR != "") ? SUPPLIER_CODE_ERR = "[รหัสSupplier: " + SUPPLIER_CODE_ERR + "]@" : SUPPLIER_CODE_ERR = "";
                    Payment_type_Err = (Payment_type_Err != "") ? Payment_type_Err = "[Payment : " + Payment_type_Err + "]@" : Payment_type_Err = "";
                    CreditTerm_Err = (CreditTerm_Err != "") ? CreditTerm_Err = "[รหัสประเภทอุปกรณ์: " + CreditTerm_Err + "]@" : CreditTerm_Err = "";
                    MATL_CODE_Err = (MATL_CODE_Err != "") ? MATL_CODE_Err = "[รหัสสินค้า: " + MATL_CODE_Err + "]@" : MATL_CODE_Err = "";
                    PACKAGE_NAME_Err = (PACKAGE_NAME_Err != "") ? PACKAGE_NAME_Err = "[หน่วยนับ: " + PACKAGE_NAME_Err + "]@" : PACKAGE_NAME_Err = "";
                    Unit_Price_Err = (Unit_Price_Err != "") ? Unit_Price_Err = "[ราคาต่แหน่วย: " + Unit_Price_Err + "]@" : Unit_Price_Err = "";
                    Order_Qty_Err = (Order_Qty_Err != "") ? Order_Qty_Err = "[จำนวนที่เบิก: " + Order_Qty_Err + "]@" : Order_Qty_Err = "";
                    TradeDiscount_Percent_Err = (TradeDiscount_Percent_Err != "") ? TradeDiscount_Percent_Err = "[ส่วนลดเปอร์เซ็น: " + TradeDiscount_Percent_Err + "]@" : TradeDiscount_Percent_Err = "";
                    TradeDiscount_Amount_Err = (TradeDiscount_Amount_Err != "") ? TradeDiscount_Amount_Err = "[ส่วนลดจำนวนเงิน: " + TradeDiscount_Amount_Err + "]@" : TradeDiscount_Amount_Err = "";
                    Total_before_Vat_Err = (Total_before_Vat_Err != "") ? Total_before_Vat_Err = "[ราคาก่อนภาษี: " + Total_before_Vat_Err + "]@" : Total_before_Vat_Err = "";
                    Vat_Err = (Vat_Err != "") ? Vat_Err = "[ภาษีมูลค่าเพิ่ม: " + Vat_Err + "]@" : Vat_Err = "";
                    Vat_Amount_Err = (Vat_Amount_Err != "") ? Vat_Amount_Err = "[ภาษีมูลค่าเพิ่มจำนวนเงิน: " + Vat_Amount_Err + "]@" : Vat_Amount_Err = "";
                    Net_Amount_Err = (Net_Amount_Err != "") ? Net_Amount_Err = "[เงินรวม: " + Net_Amount_Err + "]@" : Net_Amount_Err = "";
                    status = (status != "") ? status = "[สถานะข้อมูล: " + status + "]@" : status = "";

                    all_err = (
                     PO_DATE_Err == "" &&
                     PO_Type_Err == "" &&
                     Div_Code_Err == "" &&
                     Dep_Code_ERR == "" &&
                     SUPPLIER_CODE_ERR == "" &&
                     Payment_type_Err == "" &&
                     CreditTerm_Err == "" &&
                     MATL_CODE_Err == "" &&
                     PACKAGE_NAME_Err == "" &&
                     Unit_Price_Err == "" &&
                     Order_Qty_Err == "" &&
                     TradeDiscount_Percent_Err == "" &&
                     TradeDiscount_Amount_Err == "" &&
                     Total_before_Vat_Err == "" &&
                     Vat_Err == "" &&
                     Vat_Amount_Err == "" &&
                     Net_Amount_Err == "" &&
                     status == ""
                     ) ? "เพิ่มสำเร็จ" :

                     PO_DATE_Err +
                     PO_Type_Err +
                     Div_Code_Err +
                     Dep_Code_ERR +
                     SUPPLIER_CODE_ERR +
                     Payment_type_Err +
                     CreditTerm_Err +
                     MATL_CODE_Err +
                     PACKAGE_NAME_Err +
                     Unit_Price_Err +
                     Order_Qty_Err +
                     TradeDiscount_Percent_Err +
                     TradeDiscount_Amount_Err +
                     Total_before_Vat_Err +
                     Vat_Err +
                     Vat_Amount_Err +
                     Net_Amount_Err +
                     status;

                     all_err = all_err.Replace("@", "" + System.Environment.NewLine);

                    dr.SetField("status", all_err);

                }

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Log_PO.rdlc");
            }

            ReportViewer1.LocalReport.DataSources.Add(data_list);
            ReportViewer1.Visible = true;
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {
            dtStart.Text = "";
            dtStop.Text = "";
            D3.SelectedIndex = 0;
            TextBox3.Text = "";
        }

        private DataTable Logfile_PO()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            string[] as_date = new string[2];
            string[] ae_date = new string[2];
            string start_date = "";
            string end_date = "";
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();

            if (DateTime.TryParse(dtStart.Text.Trim(), out dt1))
            {
                as_date = dtStart.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2]) + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(dtStop.Text.Trim(), out dt2))
            {
                ae_date = dtStop.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) + "-" + ae_date[1] + "-" + ae_date[0];
            }

            if (D3.SelectedIndex == 1)
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

            if (!string.IsNullOrEmpty(TextBox3.Text.Trim()))
                param.Add(new SqlParameter("@Search_PO_NUM", TextBox3.Text.Trim()));

           
            //if (!string.IsNullOrEmpty(TextBox4.Text.Trim()))
            //   param.Add(new SqlParameter("@EP_CODE", txt_ep5.Text.Trim()));

            param.Add(new SqlParameter("@Err", rdbType.SelectedValue.ToString()));

            //return db.ExecuteDataTable("sp_Logfile_GoodReceiving_Select", param);
            return db.ExecuteDataTable("sp_Logfile_OP_Select", param);
        }

    }
}