using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Diagnostics;
using GPlus.DataAccess;
using System.Data.SqlClient;
using System.Data;

using GPlus.UserControls;
using System.Text.RegularExpressions;

namespace GPlus.MasterData
{
    public partial class CalendarPayGoodsTable : Pagebase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "123";
                SetDropDownListPageSize(PagingControl1.FindControl("ddlPageSize") as DropDownList);
            }

            BindData(txtMonthFrom.Text, txtYearFrom.Text, txtMonthTo.Text, txtYearTo.Text);
           
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
        }

        private void SetDropDownListPageSize(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("12"));
            ddl.Items.Add(new ListItem("16"));
            ddl.Items.Add(new ListItem("20"));
            ddl.Items.Add(new ListItem("28"));
            ddl.Items.Add(new ListItem("32"));
            ddl.Items.Add(new ListItem("52"));
            ddl.Items.Add(new ListItem("104"));
        }

        private void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData(txtMonthFrom.Text, txtYearFrom.Text, txtMonthTo.Text, txtYearTo.Text);
            ClearWeeks();
            pnlDetail.Visible = false;
        }

        private void ClearWeeks()
        {
            ((TextBox)CalendarPayGoodCtrl.FindControl("week1_1Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week1_2Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week2_1Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week2_2Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week3_1Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week3_2Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week4_1Calendar").FindControl("txtDate")).Text = "";
            ((TextBox)CalendarPayGoodCtrl.FindControl("week4_2Calendar").FindControl("txtDate")).Text = "";
        }

        /// <summary>
        ///     button "เพิ่มข้อมูล" is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddData_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = true;
            ClearWeeks();
            BindData(txtMonthFrom.Text, txtYearFrom.Text, txtMonthTo.Text, txtYearTo.Text);
            CalendarPayGoodsControl.SetStatus(CalendarPayGoodsControl.SaveStatus.INSERT);

            Label lblCreateDate = (Label)CalendarPayGoodCtrl.FindControl("lblCreateDate");
            Label lblCreator = (Label)CalendarPayGoodCtrl.FindControl("lblCreator");
            Label lblEditor = (Label)CalendarPayGoodCtrl.FindControl("lblEditor");
            Label lblEditDate = (Label)CalendarPayGoodCtrl.FindControl("lblEditDate");

            ThaiDateUtil dateUtil = new ThaiDateUtil();

            lblCreateDate.Text = dateUtil.GetDayMonthYear();
            lblCreator.Text = Session["FirstName"] + " " + Session["LastName"];
            lblEditor.Text = Session["FirstName"] + " " + Session["LastName"];
            lblEditDate.Text = dateUtil.GetDayMonthYear();

            ((DropDownList)CalendarPayGoodCtrl.FindControl("ddlMonth")).SelectedValue = Convert.ToString(DateTime.Now.Month);
            ((DropDownList)CalendarPayGoodCtrl.FindControl("ddlYear")).SelectedValue = Convert.ToString(DateTime.Now.Year);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(txtMonthFrom.Text, txtYearFrom.Text, txtMonthTo.Text, txtYearTo.Text);
            pnlDetail.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDetail.Visible = false;
            ClearMonthYear();
            BindData();
        }

        private void ClearMonthYear()
        {
            txtMonthFrom.Text = "";
            txtYearFrom.Text = "";
            txtMonthTo.Text = "";
            txtYearTo.Text = "";
        }

        private void BindData(string monthFrom = "", string yearFrom = "", string monthTo = "", string yearTo = "")
        {
            CalendarDAO calendarDAO = new CalendarDAO();

            try
            {
                // Prepare data to gridview
                SQLParameterList sqlParamList = new SQLParameterList();
                sqlParamList.AddStringField("MonthFrom", monthFrom);
                sqlParamList.AddStringField("YearFrom", yearFrom);
                sqlParamList.AddStringField("MonthTo", monthTo);
                sqlParamList.AddStringField("YearTo", yearTo);
                sqlParamList.AddIntegerField("PageNum", PagingControl1.CurrentPageIndex);
                sqlParamList.AddIntegerField("PageSize", PagingControl1.PageSize);

                // Execute store procedure 'sp_Inv_Calendar_Select'
                DataSet ds = calendarDAO.Select(sqlParamList);

                // Bind data to gridview
                DataRowCollection rows = ds.Tables[0].Rows;
                PagingControl1.RecordCount = (int) ds.Tables[1].Rows[0][0];
                gvPayGoodsCalendar.DataSource = ds.Tables[0];
                gvPayGoodsCalendar.DataBind();
            }
            catch (SqlException ex)
            {
                Page.ClientScript.RegisterStartupScript(
                    GetType(),
                    "YearChanged",
                    "<script type=text/javascript>alert('กรุณาตรวจสอบเดือนหรือปีให้ถูกต้อง')</script>"
                );

                ClearMonthYear();
            }
        }

        protected void gvRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                LinkButton btnDetail = (LinkButton)e.Row.FindControl("btnDetail");

                // Display information all cells only first week
                if (((e.Row.RowIndex + 1) % 4) != 1)
                {
                    e.Row.Cells[0].Text = "";
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[7].Text = "";
                }
                else
                {
                    e.Row.Cells[2].Text = new ThaiDateUtil().GetMonthName(Convert.ToInt32(e.Row.Cells[2].Text));
                }
            }
        }

        protected void gvRowCommand(Object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                BindData(txtMonthFrom.Text, txtYearFrom.Text, txtMonthTo.Text, txtYearTo.Text);
                CalendarPayGoodsControl.SetStatus(CalendarPayGoodsControl.SaveStatus.UPDATE);

                SetDateRangeForFourWeeks(Convert.ToInt32(e.CommandArgument));

                pnlDetail.Visible = true;

                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPayGoodsCalendar.Rows[index];

                int year = Convert.ToInt32(row.Cells[1].Text) - 543;
                int month = new ThaiDateUtil().GetMonthNumber(row.Cells[2].Text);

                SQLParameterList sqlParamList =  new SQLParameterList();
                sqlParamList.AddIntegerField("Year", year);
                sqlParamList.AddIntegerField("Month", month);

                DataSet ds = new DatabaseHelper().ExecuteDataSet(
                    "sp_Inv_Calendar_CreateDateCreateBy", 
                    sqlParamList.GetSqlParameterList()
                );

                Label lblCreateDate = (Label)CalendarPayGoodCtrl.FindControl("lblCreateDate");
                Label lblCreator = (Label)CalendarPayGoodCtrl.FindControl("lblCreator");
                Label lblEditor = (Label)CalendarPayGoodCtrl.FindControl("lblEditor");
                Label lblEditDate = (Label)CalendarPayGoodCtrl.FindControl("lblEditDate");

                lblCreateDate.Text = ds.Tables[0].Rows[0][0].ToString();
                lblCreator.Text = ds.Tables[0].Rows[0][1].ToString();
                lblEditor.Text = row.Cells[7].Text;
                lblEditDate.Text = row.Cells[6].Text;
            }
        }

        private void SetStartAndEndDateForWeek(string ctrl1, string value1, string ctrl2, string value2)
        {
            ((TextBox)CalendarPayGoodCtrl.FindControl(ctrl1).FindControl("txtDate")).Text = value1;
            ((TextBox)CalendarPayGoodCtrl.FindControl(ctrl2).FindControl("txtDate")).Text = value2;
        }

        private string[] SplitDate(string dateRange)
        {
            string[] date = dateRange.Split('-');

            date[0] = date[0].Trim();
            date[1] = date[1].Trim();

            return date;
        }

        private void SetDateRangeForFourWeeks(int startIndex)
        {
            CalendarPayGoodsControl.SetStatus(CalendarPayGoodsControl.SaveStatus.UPDATE);
            int endIndex = startIndex + 4;

            GridViewRow row = gvPayGoodsCalendar.Rows[startIndex];

            string year = row.Cells[1].Text;
            string month = row.Cells[2].Text;
            string status = row.Cells[5].Text;

            if (status == "Inactive")
            {
                RadioButtonList rdbStatus = (RadioButtonList) CalendarPayGoodCtrl.FindControl("rdbStatus");
                rdbStatus.SelectedValue = "0";
            }
            else if (status == "Active")
            {
                RadioButtonList rdbStatus = (RadioButtonList)CalendarPayGoodCtrl.FindControl("rdbStatus");
                rdbStatus.SelectedValue = "1";
            }

            DropDownList ddlMonth, ddlYear;

            ddlMonth = (DropDownList)CalendarPayGoodCtrl.FindControl("ddlMonth");
            ddlYear = (DropDownList)CalendarPayGoodCtrl.FindControl("ddlYear");

            ddlMonth.SelectedIndex = new ThaiDateUtil().GetMonthNumber(month) - 1;
            ddlMonth.Enabled = false;
            ddlYear.SelectedValue = Convert.ToString(Convert.ToInt32(year) - 543);
            ddlYear.Enabled = false;

            Page.ClientScript.RegisterStartupScript(
               GetType(),
               "MonthChanged",
               "<script type=text/javascript>setMonth('" + ddlMonth.SelectedItem.Value + "')</script>"
           );

            Page.ClientScript.RegisterStartupScript(
                GetType(),
                "YearChanged",
                "<script type=text/javascript>setYear('" + ddlYear.SelectedItem.Value + "')</script>"
            );

            string[] id = { "week1_1Calendar", "week1_2Calendar", 
                            "week2_1Calendar", "week2_2Calendar",
                            "week3_1Calendar", "week3_2Calendar",
                            "week4_1Calendar", "week4_2Calendar" };
            int  j = 0;

            for (int i = startIndex; i < endIndex; ++i)
            {
                row = gvPayGoodsCalendar.Rows[i];
                string[] date = SplitDate(row.Cells[4].Text);
                SetStartAndEndDateForWeek(id[j++], date[0], id[j++], date[1]);
            }
        }
    }
}