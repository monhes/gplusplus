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
    public partial class ReportStockGoodUnMoving : Pagebase
    {
        public DataTable ReportStockGoodUnMovingPackageTable
        {
            get
            {
                return (DataTable)Session["ReportStockGoodUnMoving"];
            }
            set
            {
                Session["ReportStockGoodUnMoving"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "423";
                BindDropdown();
                ReportViewer1.Visible = false;
                ddlFrequency.SelectedValue = "m";
                txtMonthBack.Text = "3";

            }
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();

            ddlMaterialType.DataSource = new DataAccess.CategoryDAO().GetCategory("", "", "1", 1, 1000, "", "").Tables[0];
            ddlMaterialType.DataTextField = "Cat_Name";
            ddlMaterialType.DataValueField = "Cate_ID";
            ddlMaterialType.DataBind();
            ddlMaterialType.Items.Insert(0, new ListItem("เลือกประเภท", ""));
        }

        protected void ddlMaterialType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindSubMaterialType();
        }

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
           
            ddlMaterialType.SelectedIndex = 0;
            ddlSubMaterialType.Items.Clear();
            ddlStock.SelectedIndex = 0;
            txtItemCode.Text = "";
            txtItemName.Text = "";

            ddlFrequency.SelectedValue = "m";
            txtMonthBack.Text = "3";
            
        }

        private void BindData()
        {

            // searchType = Date คือ ค้นหาตามช่วงวันที่, Back คือ ค้นหาแบบย้อนหลัง
            string searchType = (chkDate.Checked == true ? "Date" : "Back");

            if (searchType == "Date")
            {
                if (dtStart.Text == "" || dtStop.Text == "")
                {
                    ShowMessageBox("กรุณาระบุวันที่ค้นหา");
                    return;
                }
            }
            else
            {

                if (txtMonthBack.Text == "")
                {
                    ShowMessageBox("กรุณาระบุจำนวน " + ddlFrequency.SelectedItem.Text + " ที่ต้องการดูย้อนหลัง");
                    return;
                }
            }

            // Movement_Type = NoMovement คือไม่มีความเคลื่อนไหว , MovementCnt คือ เคลื่อนไหวไม่เกิน X ครั้ง
            string Movement_Type = (chk_NoMovement.Checked == true ? "NoMovement" : "MovementCnt");
            string Movement_Frequency_Text = "";

            if (Movement_Type == "MovementCnt")
            {
                if (txtMovementCnt.Text == "")
                {
                    ShowMessageBox("กรุณาระบุจำนวนครั้งของความเคลื่อนไหว");
                    return;
                }
                Movement_Frequency_Text = "เคลื่อนไหวไม่เกิน " + txtMovementCnt.Text + " ครั้ง";
            }
            else
            {
                Movement_Frequency_Text = "ไม่มีความเคลื่อนไหว";
            }


            ReportViewer1.Visible = true;

            GPlus.DataAccess.StockDAO st = new DataAccess.StockDAO();

            DataSet ds = st.GetStockGoodsUnMovingReport(ddlStock.SelectedValue, ddlMaterialType.SelectedValue, ddlSubMaterialType.SelectedValue, txtItemCode.Text, txtItemName.Text, searchType, dtStart.Text, dtStop.Text, ddlFrequency.SelectedValue, txtMonthBack.Text,Movement_Type,txtMovementCnt.Text);

            if (ds.Tables[1].Rows.Count > 0)
            {
                ds.Tables[1].Columns.Add("Max_TransDate_TH");


                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    if (ds.Tables[1].Rows[i]["Max_TransDate"].ToString().Length > 0)
                    {
                        int dateYear = ((DateTime)ds.Tables[1].Rows[i]["Max_TransDate"]).Year;
                        if (dateYear < 2500)
                            dateYear += 543;
                        ds.Tables[1].Rows[i]["Max_TransDate_TH"] = ((DateTime)ds.Tables[1].Rows[i]["Max_TransDate"]).Day.ToString() + "/" +
                            ((DateTime)ds.Tables[1].Rows[i]["Max_TransDate"]).Month.ToString() + "/" + dateYear.ToString();
                    }
                }
            }


            ReportParameterCollection reportParams = new ReportParameterCollection();

            string start_date = "";
            string stop_date = "";

            if (ds.Tables[0].Rows[0]["Mov_Start"].ToString() != "")
            {
                int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Mov_Start"]).Year;
                if (dateYear < 2500)
                    dateYear += 543;
                start_date = ((DateTime)ds.Tables[0].Rows[0]["Mov_Start"]).Day.ToString() + "/" +
                    ((DateTime)ds.Tables[0].Rows[0]["Mov_Start"]).Month.ToString() + "/" + dateYear.ToString();
            }

            if (ds.Tables[0].Rows[0]["Mov_End"].ToString() != "")
            {
                int dateYear = ((DateTime)ds.Tables[0].Rows[0]["Mov_End"]).Year;
                if (dateYear < 2500)
                    dateYear += 543;
                stop_date = ((DateTime)ds.Tables[0].Rows[0]["Mov_End"]).Day.ToString() + "/" +
                    ((DateTime)ds.Tables[0].Rows[0]["Mov_End"]).Month.ToString() + "/" + dateYear.ToString();
            }


            reportParams.Add(new ReportParameter("Stock_Name", ddlStock.SelectedItem.Text));
            reportParams.Add(new ReportParameter("Start_Date", start_date));
            reportParams.Add(new ReportParameter("Stop_Date", stop_date));
            reportParams.Add(new ReportParameter("Movement_Frequency_Text", Movement_Frequency_Text));

            ReportDataSource rds1 = new ReportDataSource("StockGoodsUnMovingDataSet", ds.Tables[1]);
            this.ReportViewer1.LocalReport.SetParameters(reportParams);
            this.ReportViewer1.LocalReport.DataSources.Clear();
            this.ReportViewer1.LocalReport.DataSources.Add(rds1);
            if (ds.Tables[1].Rows.Count == 0)
            {
                ReportViewer1.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            this.ReportViewer1.LocalReport.Refresh();


        }

        
    }
}