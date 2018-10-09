using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace GPlus.Stock
{
    public partial class StockCardReport : Pagebase
    {
        public DataTable StockCardReportPackageTable
        {
            get
            {
                return (DataTable)Session["StockCardReport"];
            }
            set
            {
                Session["StockCardReport"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "414";
                rdbDate.Checked = true;
                BindDropdown();
                //BindData();
                ReportViewer1.Visible = false;
                PopulateYearList();
            }

        }

        protected void PopulateYearList()
        {
            int year;
            for (year = (DateTime.Now.Year - 10) + 543; year <= (DateTime.Now.Year) + 543; year++)
            {
                ddlYearStart.Items.Add(System.Convert.ToString(year));
                //ddlYearEnd.Items.Add(System.Convert.ToString(year));
            }
            ddlYearStart.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
            //ddlYearEnd.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
            //ddlMonthStart.SelectedIndex = System.Convert.ToInt16(DateTime.Now.Month)-1;
        }


        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            //ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

            ddlMaterialType.DataSource = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
            ddlMaterialType.DataTextField = "Cat_Name";
            ddlMaterialType.DataValueField = "Cate_ID";
            ddlMaterialType.DataBind();
            ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
            ItemControlMaterial.MaterialID = ddlMaterialType.SelectedValue;
            ItemControlMaterial.MaterialName = ddlMaterialType.SelectedItem.Text;
            ItemControlMaterial.SubMaterialID = "";
            ItemControlMaterial.SubMaterialName = "";
        }

        protected void ddlSubMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemControlMaterial.MaterialID = ddlMaterialType.SelectedValue;
            ItemControlMaterial.MaterialName = ddlMaterialType.SelectedItem.Text;
            ItemControlMaterial.SubMaterialID = ddlSubMaterialType.SelectedValue;
            ItemControlMaterial.SubMaterialName = ddlSubMaterialType.SelectedItem.Text;

        }

        //protected void ddlMonthStart_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlMonthEnd.SelectedIndex = ddlMonthStart.SelectedIndex;
        //}

        //protected void ddlYearStart__SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    ddlYearEnd.SelectedIndex = ddlYearStart.SelectedIndex;
        //}

        private void BindSubMaterialType()
        {
            if (ddlMaterialType.SelectedIndex > 0)
            {
                ddlSubMaterialType.DataSource = new DataAccess.CategoryDAO().GetSubCate("", ddlMaterialType.SelectedValue, "",
                    "1", 1, 1000, "", "").Tables[0];
                ddlSubMaterialType.DataTextField = "SubCate_Name";
                ddlSubMaterialType.DataValueField = "SubCate_ID";
                ddlSubMaterialType.DataBind();
                ddlSubMaterialType.Items.Insert(0, new ListItem("เลือกประเภทอุปกรณ์ย่อย", ""));
            }
            else
                ddlSubMaterialType.Items.Clear();
        }

       

        protected void btnSearch_Click(object sender, EventArgs e)
        {

            BindData();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ReportViewer1.Visible = false;
            ClearData();
        }

        private void ClearData()
        {

            ddlStock.SelectedIndex = 0;
            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            ItemControlMaterial.Clear();
            dtStart.Text = "";
            dtStop.Text = "";
            ddlMonthStart.SelectedIndex = 0;
            //ddlMonthEnd.SelectedIndex = 0;
            ddlYearStart.SelectedValue = System.Convert.ToString(DateTime.Now.Year + 543);
            //ddlYearEnd.SelectedValue = System.Convert.ToString(DateTime.Now.Year + 543);
            rdbDate.Checked = true;
            rdbMonth.Checked = false;
            
        }

        private void BindData()
        {
            ReportViewer1.Visible = true;
            if (rdbDate.Checked == true)
            {

                if (dtStart.Text == "" && dtStop.Text == "")
                {
                    ShowMessageBox("กรูณาระบุวันที่เริ่มต้น และ สิ้นสุด");
                    ReportViewer1.Visible = false;
                    return;
                }
                else if (dtStart.Text != "" && dtStop.Text == "")
                {
                    ShowMessageBox("กรูณาระบุวันที่สิ้นสุด");
                    ReportViewer1.Visible = false;
                    return;
                }
                else if (dtStart.Text == "" && dtStop.Text != "")
                {
                    ShowMessageBox("กรูณาระบุวันที่เริ่มต้น");
                    ReportViewer1.Visible = false;
                    return;
                }

                DataSet ds = new DataAccess.StockDAO().GetStocCardReport(ddlStock.SelectedValue, ItemControlMaterial.MaterialID, ItemControlMaterial.SubMaterialID,
                ItemControlMaterial.ItemID, "", dtStart.Text, dtStop.Text);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    ReportViewer1.Visible = false;
                    ShowMessageBox("ไม่พบข้อมูล");
                }
                else
                {
                    ds.Tables[0].Columns.Add("Total_Qty");
                    int Total_Qty = Convert.ToInt32(ds.Tables[0].Rows[0]["All_Balance_Qty"]) + Convert.ToInt32(ds.Tables[0].Rows[0]["Recieve_Qty"]) - Convert.ToInt32(ds.Tables[0].Rows[0]["Pay_Qty"]) + Convert.ToInt32(ds.Tables[0].Rows[0]["Improve_Qty"]);
                    ds.Tables[0].Rows[0]["Total_Qty"] = Total_Qty;


                    ds.Tables[0].Columns.Add("StartDate");
                    string[] str_date = dtStart.Text.Split('/');
                    ds.Tables[0].Rows[0]["StartDate"] = str_date[0] + "/" + str_date[1] + "/" + (Convert.ToInt32(str_date[2] == "" ? "0" : str_date[2])+543).ToString();

                    ds.Tables[0].Columns.Add("EndDate");
                    string[] end_date = dtStop.Text.Split('/');
                    ds.Tables[0].Rows[0]["EndDate"] = end_date[0] + "/" + end_date[1] + "/" + (Convert.ToInt32(end_date[2] == "" ? "0" : end_date[2]) + 543).ToString();

                    ds.Tables[0].Columns.Add("TypeDate");
                    ds.Tables[0].Rows[0]["TypeDate"] = "Date";

                }

                ReportDataSource rds1 = new ReportDataSource("StockCardDataSet", ds.Tables[0]);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds1);
                

                this.ReportViewer1.LocalReport.Refresh();

            }
            else
            {
                string MonthYearStart = "";
                string MonthYearEnd = "";

                try
                {
                    MonthYearStart = ddlYearStart.SelectedValue + ddlMonthStart.SelectedValue;
                }
                catch 
                {
                    MonthYearStart = "";
                }

                MonthYearEnd = MonthYearStart;

                //try
                //{
                //    MonthYearEnd = ddlYearEnd.SelectedValue + ddlMonthEnd.SelectedValue;
                //}
                //catch
                //{
                //    MonthYearEnd = "";
                //}

                DataSet ds = new DataAccess.StockDAO().GetStocCardReportMonth(ddlStock.SelectedValue, ItemControlMaterial.MaterialID, ItemControlMaterial.SubMaterialID,
                ItemControlMaterial.ItemID, "", MonthYearStart, MonthYearEnd);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    ReportViewer1.Visible = false;
                    ShowMessageBox("ไม่พบข้อมูล");
                }
                else
                {
                    string CloseStock = new DataAccess.StockDAO().GetCloseStock(ddlStock.SelectedValue, MonthYearStart);
                    
                    ds.Tables[0].Columns.Add("CloseStock");

                    if (CloseStock == "Y")
                    {
                        ds.Tables[0].Rows[0]["CloseStock"] = " (ทำการปิด Stock เรียบร้อยแล้ว) ";
                    }
                    else
                    {
                        ds.Tables[0].Rows[0]["CloseStock"] = " (ยังไม่ได้ทำการปิด Stock) ";
                    }


                    ds.Tables[0].Columns.Add("MonthYear");
                    ds.Tables[0].Rows[0]["MonthYear"] = ddlMonthStart.SelectedItem.Text + "  " + ddlYearStart.SelectedItem.Text;

                    ds.Tables[0].Columns.Add("TypeDate");
                    ds.Tables[0].Rows[0]["TypeDate"] = "Month";

                    ds.Tables[0].Columns.Add("Total_Qty");

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int Total_Qty = Convert.ToInt32(ds.Tables[0].Rows[i]["All_Balance_Qty"]) + Convert.ToInt32(ds.Tables[0].Rows[i]["Recieve_Qty"]) - Convert.ToInt32(ds.Tables[0].Rows[i]["Pay_Qty"]) + Convert.ToInt32(ds.Tables[0].Rows[i]["Improve_Qty"]);
                        ds.Tables[0].Rows[i]["Total_Qty"] = Total_Qty.ToString();
                    }

                }

                ReportDataSource rds1 = new ReportDataSource("StockCardDataSet", ds.Tables[0]);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(rds1);
                
                this.ReportViewer1.LocalReport.Refresh();
            }
        }

        
    }
}