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
    public partial class MgtStockCardMonthlyClose : Pagebase
    {
        public DataTable MgtStockCardMonthlyClosePackageTable
        {
            get
            {
                return (DataTable)Session["MgtStockCardMonthlyClose"];
            }
            set
            {
                Session["MgtStockCardMonthlyClose"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "413";
                PopulateYearList();
                setLastUpdateTranAll();
                //DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
                //txtStockName.Text = dt.Rows[0]["Stock_Name"].ToString();
                //hdStockID.Value = dt.Rows[0]["Stock_ID"].ToString();
                //txtStockNameDetail.Text = dt.Rows[0]["Stock_Name"].ToString();

                DataTable dt = new DataAccess.StockDAO().GetStockAccount(this.UserID);
                ddlStock.DataSource = dt;
                ddlStock.DataTextField = "Stock_Name";
                ddlStock.DataValueField = "Stock_Id";
                ddlStock.DataBind();

                ddlStock_Detail.DataSource = dt;
                ddlStock_Detail.DataTextField = "Stock_Name";
                ddlStock_Detail.DataValueField = "Stock_Id";
                ddlStock_Detail.DataBind();

                BindData();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);

        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        protected void PopulateYearList()
        {
            int year;
            for (year = (DateTime.Now.Year - 10) + 543; year <= (DateTime.Now.Year) + 543; year++)
            {
                ddlYearStart.Items.Add(System.Convert.ToString(year));
                ddlYearEnd.Items.Add(System.Convert.ToString(year));
                ddlYearDetail.Items.Add(System.Convert.ToString(year));
            }
            ddlYearStart.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
            ddlYearEnd.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
            ddlYearDetail.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
            pnlDetail.Visible = false;
       
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {

            setLastUpdateTranAll();
            BindData();
            //pnlDetail.Visible = false;
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            btnChoice.Visible = true;
            btnSave.Visible = true;
            btnDelete.Visible = false;

            DataTable dt = new DataAccess.StockDAO().GetCloseStockNew(ddlStock.SelectedValue, "1");
            if (dt.Rows.Count > 0)
            {
                
                int n_month = 0;
                int n_year = 0;
                string str_month = "";
                string str_year = "";
                n_month = Convert.ToInt16(dt.Rows[0]["Month_End_SUM"].ToString().Substring(4, 2)) + 1;
                n_year = Convert.ToInt32(dt.Rows[0]["Month_End_SUM"].ToString().Substring(0, 4));

                if (n_month > 12)
                {
                    n_month = n_month - 12;
                    n_year = n_year + 1;
                }

                str_month = n_month.ToString();
                str_year = n_year.ToString();

                if (str_month.Length == 1)
                {
                    str_month = "0" + str_month;
                }

                ddlMonthDetail.SelectedValue = str_month;
                ddlYearDetail.SelectedValue = str_year;
                ddlMonthDetail.Enabled = false;
                ddlYearDetail.Enabled = false;
                //txtStockNameDetail.Text = dt.Rows[0]["Stock_Name"].ToString();
            }
            else
            {
                ddlMonthDetail.SelectedIndex = 0;
                ddlMonthDetail.Enabled = true;
                ddlYearDetail.Enabled = true;
            }

            ddlStock_Detail.SelectedValue = ddlStock.SelectedValue;
            

            rdbStatus.SelectedIndex = 0;
            //rdbStatus.Items[0].Enabled = true;
            //rdbStatus.Items[1].Enabled = false; //ให้เลือกเฉพาะปุ่ม Active เท่านั้น
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

            pnlDetail.Visible = true;
            //ClearData();
            //btnPopAccount.Visible = false;
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (ddlMonthDetail.SelectedValue == "00")
            {
                this.ShowMessageBox("กรุณาเลือกเดือนที่ต้องการปิด Stock");
                return;
            }
            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";
            string retValueInsert = "";
            //string retValueChangeStatus= "";

            //if (status == "1")
            //{
            //    retValueInsert = new DataAccess.StockDAO().AddCloseStock(ddlStock_Detail.SelectedValue, ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, status, this.UserID);

            //    if (retValueInsert == "1")
            //    {
            //        this.ShowMessageBox("ทำการปิด Stock เรียบร้อย");
            //        pnlDetail.Visible = false;
            //    }
            //    else
            //    {
            //        this.ShowMessageBox("ไม่สามารถปิด stock ได้");
            //        //this.ShowMessageBox("ไม่พบข้อมูลของเดือนที่ต้องการทำรายการ");
            //    }
            //}
            //else
            //{
            //    retValueChangeStatus = new DataAccess.StockDAO().CloseStockChangeStatus(ddlStock_Detail.SelectedValue, ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, status, this.UserID);
            //    if (retValueChangeStatus == "1")
            //    {
            //        this.ShowMessageBox("ทำการแก้ไขเรียบร้อย");
            //    }
            //    else
            //    {
            //        this.ShowMessageBox("ไม่สามารถแก้ไขข้อมูลได้");
            //    }
            //}

            retValueInsert = new DataAccess.StockDAO().AddCloseStock(ddlStock_Detail.SelectedValue, ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, status, this.UserID);

            if (retValueInsert == "1")
            {
                this.ShowMessageBox("ทำการปิด Stock เรียบร้อย");
                pnlDetail.Visible = false;
            }
            else
            {
                this.ShowMessageBox("ไม่สามารถปิด stock ได้");
                //this.ShowMessageBox("ไม่พบข้อมูลของเดือนที่ต้องการทำรายการ");
            }

            

            setLastUpdateTranAll();
            BindData();
            pnlDetail.Visible = false;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            string status = "0";
            string retValueChangeStatus = "";

            retValueChangeStatus = new DataAccess.StockDAO().CloseStockChangeStatus(ddlStock_Detail.SelectedValue, ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, status, this.UserID);
            if (retValueChangeStatus == "1")
            {
                this.ShowMessageBox("ทำการลบข้อมูลเรียบร้อย");
            }
            else
            {
                this.ShowMessageBox("ไม่สามารถลบข้อมูลได้");
            }

            setLastUpdateTranAll();
            BindData();
            pnlDetail.Visible = false;
        }

        private void BindData()
        {
            int n_Startdate = 0;
            int n_Enddate = 0;
            String str_Startdate = "";
            String str_Enddate = "";
            n_Startdate = Convert.ToInt32(ddlYearStart.SelectedValue + ddlMonthStart.SelectedValue);
            n_Enddate = Convert.ToInt32(ddlYearEnd.SelectedValue + ddlMonthEnd.SelectedValue);
            str_Startdate = ddlYearStart.SelectedValue + ddlMonthStart.SelectedValue;
            str_Enddate = ddlYearEnd.SelectedValue + ddlMonthEnd.SelectedValue;
            //ShowMessageBox(n_Startdate.ToString());
            //ShowMessageBox(n_Enddate.ToString());

            DataSet ds = new DataAccess.StockDAO().GetStocOnHandkCloseStock(ddlStock.SelectedValue, str_Startdate, str_Enddate, "",
            PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvMovementSum.Visible = true;
                PagingControl1.Visible = true;

                PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

                gvMovementSum.DataSource = ds.Tables[0];
                gvMovementSum.DataBind();

                
            }
            else
            {
                gvMovementSum.Visible = false;
                PagingControl1.Visible = false;
            }

        }

        protected void gvMovementSum_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (drv["Run_date"].ToString().Trim().Length > 0)
                    e.Row.Cells[1].Text = (drv["Run_date"].ToString()).Substring(4, 2) + "/" + (drv["Run_date"].ToString()).Substring(0, 4);

                e.Row.Cells[2].Text = drv["Run_Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";

                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Run_date"].ToString() + "&" + drv["Stock_ID"].ToString() + "&" + drv["Run_Status"].ToString()
                                                                                + "&" + drv["Create_Date"].ToString() + "&" + drv["Crt_By"].ToString()
                                                                                + "&" + drv["Update_Date"].ToString() + "&" + drv["Mdf_by"].ToString();
                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[3].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateTimeFormat);

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[5].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);

            }
        }

        protected void gvMovementSum_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                btnSave.Visible = false;
                //rdbStatus.Items[0].Enabled = true;
                //rdbStatus.Items[1].Enabled = true;

                string Last_tran_All = "";
                DataTable dt2 = new DataAccess.StockDAO().GetCloseStockNew(ddlStock.SelectedValue, "");
                if (dt2.Rows.Count > 0)
                {
                    Last_tran_All = dt2.Rows[0]["Month_End_SUM"].ToString();
                }

                string Last_tran_Active = ""; // เก็บค่า Transaction Date ล่าสุดที่มีสถานะ Active เป็น String
                int n_Last_tran_Active = 0; // เก็บค่า Transaction Date ล่าสุดที่มีสถานะ Active เป็น Integer
                DataTable dt3 = new DataAccess.StockDAO().GetCloseStockNew(ddlStock.SelectedValue, "1");
                if (dt3.Rows.Count > 0)
                {
                    Last_tran_Active = dt3.Rows[0]["Month_End_SUM"].ToString();
                }

                if (Last_tran_Active != "")
                {
                    try
                    {
                        n_Last_tran_Active = Convert.ToInt32(Last_tran_Active);
                    }
                    catch
                    {
                        n_Last_tran_Active = 0;
                    }
                }

                string[] str_cmd = e.CommandArgument.ToString().Split('&');

                string str_month = "";
                string str_year = "";
                string str_stock_id = "";
                string str_status = "";
                string str_run_date = "";
                string str_create_date = "";
                string str_create_by = "";
                string str_update_date = "";
                string str_update_by = "";


                str_run_date = str_cmd[0];
                str_month = str_run_date.Substring(4, 2);
                str_year = str_run_date.Substring(0, 4);

                str_stock_id = str_cmd[1];
                str_status = str_cmd[2];
                str_create_date = str_cmd[3];
                str_create_by = str_cmd[4];
                str_update_date = str_cmd[5];
                str_update_by  = str_cmd[6];


                ddlStock_Detail.SelectedValue = str_stock_id;
                ddlMonthDetail.SelectedValue = str_month;
                ddlYearDetail.SelectedValue = str_year;
                ddlMonthDetail.Enabled = false;
                ddlYearDetail.Enabled = false;
                rdbStatus.Items[0].Selected = str_status == "1";
                rdbStatus.Items[1].Selected = str_status == "0";

                //ทำการตรวจสอบว่าข้อมูลที่ดูรายละเอียดเป็น Transaction ที่ทำเดือนล่าสุด/Transaction ที่มี สถานะ Active ล่าสุด/มากกว่า Transaction Active ล่าสุด หรือไม่ ถ้าใช่จึงจะให้เลือกเป็น Inactive ได้
                if (Last_tran_All == str_run_date || Last_tran_Active == str_run_date || Convert.ToInt32(str_run_date) >= n_Last_tran_Active)
                {
                    //rdbStatus.Items[0].Enabled = true;
                    //rdbStatus.Items[1].Enabled = true;
                    btnDelete.Visible = true;
                }
                else
                {
                    //rdbStatus.Items[0].Enabled = true;
                    //rdbStatus.Items[1].Enabled = false;
                    btnDelete.Visible = false;
                }



                //ทำการตราวจสอบว่าถ้าข้อมูลที่กดดูรายละเอียด มีสถานะ เป็น Inactive ให้ Disable ปุ่ม Active และ InActive 
                if (str_status == "0")
                {
                    //rdbStatus.Items[0].Enabled = false;
                    //rdbStatus.Items[1].Enabled = false;
                    //btnChoice.Visible = false;
                    btnDelete.Visible = false;
                }
                lblCreateBy.Text = str_create_by;
                lblUpdateBy.Text = str_update_by;

                if (str_create_date.Length > 0)
                    //lblCreateDate.Text = ((DateTime)dt.Rows[0]["Create_Date"]).ToString(this.DateTimeFormat);
                    lblCreateDate.Text = str_create_date;

                if (str_update_date.Length > 0)
                    //lblUpdatedate.Text = ((DateTime)dt.Rows[0]["Update_Date"]).ToString(this.DateTimeFormat);
                    lblUpdatedate.Text = str_update_date;

                pnlDetail.Visible = true;
            }
        }

        protected void setLastUpdateTranAll()
        {
            DataTable dt = new DataAccess.StockDAO().GetCloseStockNew(ddlStock.SelectedValue, "");
            if (dt.Rows.Count > 0)
            {

                int n_month = 0;
                int n_year = 0;
                string str_month = "";
                string str_year = "";
                n_month = Convert.ToInt16(dt.Rows[0]["Month_End_SUM"].ToString().Substring(4, 2));
                n_year = Convert.ToInt32(dt.Rows[0]["Month_End_SUM"].ToString().Substring(0, 4));

                if (n_month > 12)
                {
                    n_month = n_month - 12;
                    n_year = n_year + 1;
                }

                str_month = n_month.ToString();
                str_year = n_year.ToString();

                if (str_month.Length == 1)
                {
                    str_month = "0" + str_month;
                }

                ddlMonthEnd.SelectedValue = str_month;
                ddlYearEnd.SelectedValue = str_year;
            }
        
        }


    }
}