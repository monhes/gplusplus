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
    public partial class SupplierMTL_Balance : Pagebase
    {
        public DataTable SupplierMTL_BalancePackageTable
        {
            get
            {
                return (DataTable)Session["SupplierMTL_Balance"];
            }
            set
            {
                Session["SupplierMTL_Balance"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                this.PageID = "419";
                PopulateYearList();
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
            BindData();
            //pnlDetail.Visible = false;
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            hdID.Value = "";
            ddlMonthDetail.Enabled = true;
            ddlYearDetail.Enabled = true;
            ddlMonthDetail.SelectedIndex = 0;
            ddlYearDetail.Items.FindByValue(System.Convert.ToString(DateTime.Now.Year + 543)).Selected = true;
            txtBalance.Text = "";
            lblCreateBy.Text = this.FirstName + " " + this.LastName;
            lblUpdateBy.Text = this.FirstName + " " + this.LastName;
            lblCreateDate.Text = DateTime.Now.ToString(this.DateTimeFormat);
            lblUpdatedate.Text = DateTime.Now.ToString(this.DateTimeFormat);

            pnlDetail.Visible = true;

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (ddlMonthDetail.SelectedValue == "00")
            {
                this.ShowMessageBox("กรุณาเลือกเดือนที่ต้องการระบุยอดคงเหลือ");
                return;
            }


            string status = rdbStatus.SelectedIndex == 0 ? "1" : "0";

            if (hdID.Value == "")
            {
                string retValueInsert = "";

                retValueInsert = new DataAccess.SupplierDAO().AddSupplierMTL_Balance("1", ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, txtBalance.Text.Replace(",", ""), status, this.UserID);

                if (retValueInsert == "1")
                {
                    this.ShowMessageBox("ทำการบันทึกข้อมูลเรียบร้อย");
                    pnlDetail.Visible = false;
                }
                else if (retValueInsert == "2")
                {
                    this.ShowMessageBox("มีข้อมูลยอดคงเหลือของ " + ddlMonthDetail.SelectedValue + "/" + ddlYearDetail.SelectedValue + " แล้ว");
                }
                else
                {
                    this.ShowMessageBox("ไม่สามารถบันทึกข้อมูลได้");
                }
            }
            else
            {
                string retValueInsert = "";

                retValueInsert = new DataAccess.SupplierDAO().UpdateSupplierMTL_Balance("1", ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, txtBalance.Text.Replace(",", ""), status, this.UserID);

                if (retValueInsert == "1")
                {
                    this.ShowMessageBox("ทำการแก้ไขข้อมูลเรียบร้อย");
                    pnlDetail.Visible = false;
                }
                else
                {
                    this.ShowMessageBox("ไม่สามารถแก้ไขข้อมูลได้");
                }
            }

            BindData();
            pnlDetail.Visible = false;

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

            string status = "0";
            string retValueChangeStatus = "";

            retValueChangeStatus = new DataAccess.SupplierDAO().SupplierMTL_BalanceChangeStatus("1", ddlMonthDetail.SelectedValue, ddlYearDetail.SelectedValue, status, this.UserID);
            if (retValueChangeStatus == "1")
            {
                this.ShowMessageBox("ทำการลบข้อมูลเรียบร้อย");
            }
            else
            {
                this.ShowMessageBox("ไม่สามารถลบข้อมูลได้");
            }

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

            DataSet ds = new DataAccess.SupplierDAO().GetSupplierMTL_Balance("1", str_Startdate, str_Enddate, "",
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

                if (drv["Month_End_Sum"].ToString().Trim().Length > 0)
                    e.Row.Cells[1].Text = (drv["Month_End_Sum"].ToString()).Substring(4, 2) + "/" + (drv["Month_End_Sum"].ToString()).Substring(0, 4);

                e.Row.Cells[2].Text = Convert.ToDecimal(drv["Balance_Amount"] == "" ? "0" : drv["Balance_Amount"]).ToString("#,##0.0000");

                e.Row.Cells[3].Text = drv["Status"].ToString() == "1" ? "<span style='color:navy'>Active</span>" :
                    "<span style='color:red'>InActive</span>";


                ((LinkButton)e.Row.FindControl("btnDetail")).CommandArgument = drv["Month_End_Sum"].ToString() + "&" + drv["Stock_ID"].ToString() + "&"
                                                                                + drv["Status"].ToString() +"&"+ drv["Balance_Amount"].ToString()
                                                                                + "&" + drv["Create_Date"].ToString() + "&" + drv["Crt_By"].ToString()
                                                                                + "&" + drv["Update_Date"].ToString() + "&" + drv["Mdf_by"].ToString();
                if (drv["Create_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[4].Text = ((DateTime)drv["Create_Date"]).ToString(this.DateTimeFormat);

                if (drv["Update_Date"].ToString().Trim().Length > 0)
                    e.Row.Cells[6].Text = ((DateTime)drv["Update_Date"]).ToString(this.DateTimeFormat);

            }
        }

        protected void gvMovementSum_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edi")
            {
                string[] str_cmd = e.CommandArgument.ToString().Split('&');

                string str_month = "";
                string str_year = "";
                string str_stock_id = "";
                string str_status = "";
                string str_balance_amount = "";
                string str_month_end_sum = "";
                string str_create_date = "";
                string str_create_by = "";
                string str_update_date = "";
                string str_update_by = "";


                str_month_end_sum = str_cmd[0];
                hdID.Value = str_cmd[0];
                str_month = str_month_end_sum.Substring(4, 2);
                str_year = str_month_end_sum.Substring(0, 4);

                str_stock_id = str_cmd[1];
                str_status = str_cmd[2];
                str_balance_amount = str_cmd[3]; 
                str_create_date = str_cmd[4];
                str_create_by = str_cmd[5];
                str_update_date = str_cmd[6];
                str_update_by = str_cmd[7];

                ddlMonthDetail.SelectedValue = str_month;
                ddlYearDetail.SelectedValue = str_year;
                ddlMonthDetail.Enabled = false;
                ddlYearDetail.Enabled = false;
                txtBalance.Text = Convert.ToDecimal(str_balance_amount == "" ? "0" : str_balance_amount).ToString("#,##0.0000");
                rdbStatus.Items[0].Selected = str_status == "1";
                rdbStatus.Items[1].Selected = str_status == "0";
                
                lblCreateBy.Text = str_create_by;
                lblCreateDate.Text = str_create_date;
                lblUpdateBy.Text = str_update_by;
                lblUpdatedate.Text = str_update_date;
                pnlDetail.Visible = true;
            }
        }


    }
}