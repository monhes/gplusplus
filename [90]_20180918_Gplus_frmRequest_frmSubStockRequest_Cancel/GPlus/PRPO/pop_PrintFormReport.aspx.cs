using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GPlus.PRPO
{
    public partial class pop_PrintFormReport : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.Visible = false;
                if (Request["id"] != null)
                {
                    DataSet ds = new DataAccess.PRDAO().GetPRReport(Request["id"]);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (ds.Tables[2].Rows[0]["Form_Type"].ToString() == "0")
                            ReportViewer1.LocalReport.ReportPath = "PRPO\\PrintFormReport.rdlc";
                        else
                            ReportViewer1.LocalReport.ReportPath = "PRPO\\PrintFormOldReport.rdlc";

                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            string printType = ds.Tables[2].Rows[0]["Print_Type"].ToString();
                            //0 = ฟอร์มคอมพิวเตอร์,  1 = เข้าเล่ม, 2 = เข้าชุด 3 = แผ่น, 4 = โบรชัว
                            switch (printType)
                            {
                                case "0": ds.Tables[2].Rows[0]["Print_Type"] = "ฟอร์มคอมพิวเตอร์"; break;
                                case "1": ds.Tables[2].Rows[0]["Print_Type"] = "เข้าเล่ม"; break;
                                case "2": ds.Tables[2].Rows[0]["Print_Type"] = "เข้าชุด"; break;
                                case "3": ds.Tables[2].Rows[0]["Print_Type"] = "แผ่น"; break;
                                case "4": ds.Tables[2].Rows[0]["Print_Type"] = "แผ่นพับ"; break;
                            }

                            string borrowType = ds.Tables[2].Rows[0]["Borrow_Type"].ToString();
                            //0 = เบิกระบุวันที่ 1 = เมื่อแบบพิมพ์เสร็จ 2 = เมื่อแบบพิมพ์เดิมหมดลง  3 = เมื่อแบบพิมพ์ใหม่เสร็จ
                            switch (borrowType)
                            {
                                case "0": ds.Tables[2].Rows[0]["Borrow_Type"] = "เบิกระบุวันที่"; break;
                                case "1": ds.Tables[2].Rows[0]["Borrow_Type"] = "เมื่อแบบพิมพ์เสร็จ"; break;
                                case "2": ds.Tables[2].Rows[0]["Borrow_Type"] = "เมื่อแบบพิมพ์เดิมหมดลง"; break;
                                case "3": ds.Tables[2].Rows[0]["Borrow_Type"] = "เมื่อแบบพิมพ์ใหม่เสร็จ"; break;
                            }

                            //FormBorrow_Type
                            string formBorrowType = ds.Tables[2].Rows[0]["FormBorrow_Type"].ToString();
                            //0 = ขอพิมพ์เพิ่มเติม 1 = เบิกใช้ครั้งเดียวหมด 2 = เก็บเข้า Stock
                            switch (formBorrowType)
                            {
                                case "0": ds.Tables[2].Rows[0]["FormBorrow_Type"] = "ขอพิมพ์เพิ่มเติม"; break;
                                case "1": ds.Tables[2].Rows[0]["FormBorrow_Type"] = "เบิกใช้ครั้งเดียวหมด"; break;
                                case "2": ds.Tables[2].Rows[0]["FormBorrow_Type"] = "เก็บเข้า Stock"; break;
                            }
                            //Format
                            string format = ds.Tables[2].Rows[0]["Format"].ToString();
                            //0 ตามที่แบบ 1 กรุณาออกแบบให้
                            switch (format)
                            {
                                case "0": ds.Tables[2].Rows[0]["Format"] = "ตามที่แบบ"; break;
                                case "1": ds.Tables[2].Rows[0]["Format"] = "กรุณาออกแบบให้"; break;
                            }
                        }

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("MainDataSet", ds.Tables[0]));
                        ReportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("FormDataSet", ds.Tables[2]));

                        ReportViewer1.Visible = true;
                    }
                }

            }
        }

    }
}