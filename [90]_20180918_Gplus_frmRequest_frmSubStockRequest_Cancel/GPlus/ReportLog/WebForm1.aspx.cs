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
    public partial class WebForm1 : Pagebase
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
            string tab = Request.QueryString["tab"].ToString();
            //string tab = "1";
            ReportDataSource data_list = new ReportDataSource();
            if (tab == "0")
            {
                DataTable dt = SUPPLIER();
                //ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("", dt);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Report2.rdlc");
                //ReportViewer1.LocalReport.DataSources.Clear();
            }
            else if (tab == "1")
            {
                //DataTable dt = Close_IS();
                //ReportViewer1.LocalReport.DataSources.Clear();
                //data_list = new ReportDataSource("Close_IS", dt);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("Close_IS.rdlc"); //udohere
            }
            else if (tab == "2")//inprogress
            {
                DataTable dt = Logfile_PO();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Log_PO", dt);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Log_PO.rdlc");
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            else if (tab == "3")//pass
            {
                DataTable dt = GoodReceiving();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("GoodReceiving", dt);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("GoodReceiving.rdlc");

            }
            else if (tab == "4")//pass
            {
                DataTable dt = Material();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Material", dt);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Material.rdlc");
            }
            else if (tab == "5")//pass
            {
                DataTable dt = Conversion();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Conversion", dt);
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("Conversion1.rdlc");
            }
            else if (tab == "6")//pass
            {
                DataTable dt = SUPPLIER();
                ReportViewer1.LocalReport.DataSources.Clear();
                data_list = new ReportDataSource("Supplier", dt);
                //ReportViewer1.LocalReport.ReportPath = Server.MapPath("Supplier.rdlc");
                ReportViewer1.LocalReport.ReportPath = "ReportLog//Supplier.rdlc";
            }
            //else if (tab == "7")
            //{
            //    DataTable dt = SUPPLIER();
            //    ReportViewer1.LocalReport.DataSources.Clear();
            //    data_list = new ReportDataSource("", dt);
            //    ReportViewer1.LocalReport.ReportPath = Server.MapPath("Report2.rdlc");
            //}


            ReportViewer1.LocalReport.DataSources.Add(data_list);
        }

        protected void btn_clear_Click(object sender, EventArgs e)
        {

        }

        //private DataTable Close_IS()
        //{
        //    List<SqlParameter> param = new List<SqlParameter>();
        //    string[] as_date = new string[2];
        //    string[] ae_date = new string[2];
        //    string start_date = "";
        //    string end_date = "";
        //    DateTime dt1 = new DateTime();
        //    DateTime dt2 = new DateTime();

        //    if (DateTime.TryParse(date_start_2.Text.Trim(), out dt1))
        //    {
        //        as_date = date_start_2.Text.Trim().Split('/');
        //        start_date = int.Parse(as_date[2]) - 543 + "-" + as_date[1] + "-" + as_date[0];
        //    }

        //    if (DateTime.TryParse(date_end_2.Text.Trim(), out dt2))
        //    {
        //        ae_date = date_end_2.Text.Trim().Split('/');
        //        end_date = int.Parse(ae_date[2]) - 543 + "-" + ae_date[1] + "-" + ae_date[0];
        //    }

        //    if (D2.SelectedIndex == 1)
        //    {
        //        if (!string.IsNullOrEmpty(start_date))
        //        {
        //            param.Add(new SqlParameter("@Process_date_Start", start_date.Trim()));
        //        }
        //        if (!string.IsNullOrEmpty(end_date))
        //        {
        //            param.Add(new SqlParameter("@Process_date_End", end_date.Trim()));
        //        }
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(start_date))
        //        {
        //            param.Add(new SqlParameter("@Transfer_date_Start", start_date.Trim()));
        //        }
        //        if (!string.IsNullOrEmpty(end_date))
        //        {
        //            param.Add(new SqlParameter("@Transfer_date_End", end_date.Trim()));
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(Text_Req_No.Text.Trim()))
        //        param.Add(new SqlParameter("@Request_No", Text_Req_No.Text.Trim()));

        //    if (!string.IsNullOrEmpty(Text_Matl_Code.Text.Trim()))
        //        param.Add(new SqlParameter("@MATL_CODE", Text_Matl_Code.Text.Trim()));

        //    return db.ExecuteDataTable("sp_Logfile_Close_Stock_Issue_Select", param);
        //}//udohere

        private DataTable Logfile_PO()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            string[] as_date = new string[2];
            string[] ae_date = new string[2];
            string start_date = "";
            string end_date = "";
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();

            if (DateTime.TryParse(date_start_3.Text.Trim(), out dt1))
            {
                as_date = date_start_3.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2]) - 543 + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(date_end_3.Text.Trim(), out dt2))
            {
                ae_date = date_end_3.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) - 543 + "-" + ae_date[1] + "-" + ae_date[0];
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

            //param.Add(new SqlParameter("@Err", rdl4.SelectedValue.ToString()));

            //return db.ExecuteDataTable("sp_Logfile_GoodReceiving_Select", param);
            return db.ExecuteDataTable("sp_Logfile_OP_Select", param);
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

            if (DateTime.TryParse(date_start_4.Text.Trim(), out dt1))
            {
                as_date = date_start_4.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2]) - 543 + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(date_end_4.Text.Trim(), out dt2))
            {
                ae_date = date_end_4.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) - 543 + "-" + ae_date[1] + "-" + ae_date[0];
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
                param.Add(new SqlParameter("@PO_CODE", txt_po5.Text.Trim()));

            if (!string.IsNullOrEmpty(txt_ep4.Text.Trim()))
                param.Add(new SqlParameter("@EP_CODE", txt_ep5.Text.Trim()));

            //param.Add(new SqlParameter("@Err", rdl4.SelectedValue.ToString()));

            // return db.ExecuteDataTable("sp_Logfile_GoodReceiving_Select", param);
            return db.ExecuteDataTable("sp_Logfile_OP_Select", param);
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

            if (DateTime.TryParse(date_start_5.Text.Trim(), out dt1))
            {
                as_date = date_start_5.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2]) - 543 + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(date_end_5.Text.Trim(), out dt2))
            {
                ae_date = date_end_5.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) - 543 + "-" + ae_date[1] + "-" + ae_date[0];
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
                param.Add(new SqlParameter("@PO_CODE", txt_po5.Text.Trim()));

            if (!string.IsNullOrEmpty(txt_ep5.Text.Trim()))
                param.Add(new SqlParameter("@EP_CODE", txt_ep5.Text.Trim()));

            param.Add(new SqlParameter("@Err", rdl5.SelectedValue.ToString()));

            return db.ExecuteDataTable("sp_Logfile_Material_Select", param);
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

            if (DateTime.TryParse(date_start_6.Text.Trim(), out dt1))
            {
                as_date = date_start_6.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2]) - 543 + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(date_end_6.Text.Trim(), out dt2))
            {
                ae_date = date_end_6.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) - 543 + "-" + ae_date[1] + "-" + ae_date[0];
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

            param.Add(new SqlParameter("@Err", rdl6.SelectedValue.ToString()));

            return db.ExecuteDataTable("sp_Logfile_Conversion_Select", param);
        }

        private DataTable SUPPLIER()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            string[] as_date = new string[2];
            string[] ae_date = new string[2];
            string start_date = "";
            string end_date = "";
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();

            if (DateTime.TryParse(date_start_7.Text.Trim(), out dt1))
            {
                as_date = date_start_7.Text.Trim().Split('/');
                start_date = int.Parse(as_date[2]) - 543 + "-" + as_date[1] + "-" + as_date[0];
            }

            if (DateTime.TryParse(date_end_7.Text.Trim(), out dt2))
            {
                ae_date = date_end_7.Text.Trim().Split('/');
                end_date = int.Parse(ae_date[2]) - 543 + "-" + ae_date[1] + "-" + ae_date[0];
            }

            if (D7.SelectedIndex == 0)
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

            if (!string.IsNullOrEmpty(txt_Supplier7.Text.Trim()))
                param.Add(new SqlParameter("@SUPPLIER_CODE", txt_Supplier7.Text.Trim()));

            param.Add(new SqlParameter("@Err", rdl7.SelectedValue.ToString()));

            return db.ExecuteDataTable("sp_Logfile_SUPPLIER_Select", param);
        }
    }
}