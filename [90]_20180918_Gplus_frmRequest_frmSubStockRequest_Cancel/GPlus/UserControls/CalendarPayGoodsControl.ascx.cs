using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Diagnostics;
using System.Configuration;

using GPlus.MasterData;
using GPlus.DataAccess;
using System.Data.SqlClient;
using System.Data;

namespace GPlus.UserControls
{
    public partial class CalendarPayGoodsControl : System.Web.UI.UserControl
    {
        public enum     SaveStatus { INSERT, UPDATE };
        private static  SaveStatus _Status;

        protected void Page_Load(object sender, EventArgs e)
        {
            week1_1Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week1_2Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week2_1Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week2_2Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week3_1Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week3_2Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week4_1Calendar.SetAttributesOnTextbox("readonly", "readonly");
            week4_2Calendar.SetAttributesOnTextbox("readonly", "readonly");

            ddlMonth.Enabled = true;
            ddlYear.Enabled = true;

            if (!IsPostBack)
            {
                ddlMonth.SelectedIndex = DateTime.Now.Month - 1;
                ddlYear.SelectedValue = Convert.ToString(DateTime.Now.Year);
            }
        }

        protected virtual void MonthChanged(Object sender, EventArgs e)
        {
            SetMonthAndYearToJS();
            ClearWeeks();
        }

        protected virtual void YearChanged(Object sender, EventArgs e)
        {
            SetMonthAndYearToJS();
            ClearWeeks();
        }

        /// <summary>
        ///     Put value from ddlMonth dropdownlist to setMonth().
        ///     Put value from ddlYear dropdownlist to setYear().
        ///     Both functions is defined in "CalendarDependOnMonthYearControl.ascx"
        /// </summary>
        private void SetMonthAndYearToJS()
        {
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
        }

        /// <summary>
        ///     Clear all weeks from first to last week.
        /// </summary>
        private void ClearWeeks()
        {
            week1_1Calendar.Text = "";
            week1_2Calendar.Text = "";
            week2_1Calendar.Text = "";
            week2_2Calendar.Text = "";
            week3_1Calendar.Text = "";
            week3_2Calendar.Text = "";
            week4_1Calendar.Text = "";
            week4_2Calendar.Text = "";
        }

        /// <summary>
        ///     Save the data from user. This method was used for saving and updating.
        /// </summary>
        //
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (week1_1Calendar.Text == "" || week1_2Calendar.Text == "" ||
                week2_1Calendar.Text == "" || week2_2Calendar.Text == "" ||
                week3_1Calendar.Text == "" || week3_2Calendar.Text == "" ||
                week4_1Calendar.Text == "" || week4_2Calendar.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(
                    GetType(),
                    "Error",
                    "<script type=text/javascript>alert('กรุณากรอกช่วงวันที่ให้ครบทุกสัปดาห์')</script>"
                );
            }
            else
            {
                // Get Month and Year from selected dropdownlist
                int month = Convert.ToInt32(ddlMonth.SelectedValue);
                int year = Convert.ToInt32(ddlYear.SelectedValue);

                // Insert new calendar
                if (_Status == CalendarPayGoodsControl.SaveStatus.INSERT)
                {
                    SQLParameterList sqlParam = new SQLParameterList();
                    sqlParam.AddIntegerField("Month"   , month);
                    sqlParam.AddIntegerField("Year"    , year);
                    sqlParam.AddStringField("Week1_1"  , week1_1Calendar.Text);
                    sqlParam.AddStringField("Week1_2"  , week1_2Calendar.Text);
                    sqlParam.AddStringField("Week2_1"  , week2_1Calendar.Text);
                    sqlParam.AddStringField("Week2_2"  , week2_2Calendar.Text);
                    sqlParam.AddStringField("Week3_1"  , week3_1Calendar.Text);
                    sqlParam.AddStringField("Week3_2"  , week3_2Calendar.Text);
                    sqlParam.AddStringField("Week4_1"  , week4_1Calendar.Text);
                    sqlParam.AddStringField("Week4_2"  , week4_2Calendar.Text);
                    sqlParam.AddBooleanField("Status"  , Convert.ToBoolean(Convert.ToInt32(rdbStatus.SelectedValue)));
                    sqlParam.AddIntegerField("CreateBy", Convert.ToInt32(Session["UserID"].ToString()));
                    sqlParam.AddIntegerField("UpdateBy", Convert.ToInt32(Session["UserID"].ToString()));

                    try
                    {
                        new CalendarDAO().Insert(sqlParam);
                       
                        Page.ClientScript.RegisterStartupScript(
                            GetType(),
                            "Error",
                            "<script type=text/javascript>alert('เพิ่มข้อมูลลงในฐานข้อมูลเรียบร้อยแล้ว');window.location.replace('CalendarPayGoodsTable.aspx')</script>"
                        );
                    }
                    catch (SqlException ex)
                    {
                        // 2627 is the number that defined in System Error Message.
                        // Cannot insert with the same primary key.
                        // See Sql server book online.
                        if (ex.Number == 2627)
                        {
                            Page.ClientScript.RegisterStartupScript(
                                GetType(),
                                "Error",
                                "<script type=text/javascript>alert('ไม่สามารถเพิ่มปฏิทินที่มีเดือนปีซ้ำกันได้ เนื่องจากเดือนปีดังกล่าวมีในฐานข้อมูลแล้ว')</script>"
                            );
                        }
                    }
                    finally
                    {
                        ClearWeeks();
                    }
                }
                // UPDATE THE CALENDAR
                else
                {
                    SQLParameterList sqlParam = new SQLParameterList();
                    sqlParam.AddIntegerField("Month", month);
                    sqlParam.AddIntegerField("Year", year);
                    sqlParam.AddStringField("Week1_1", week1_1Calendar.Text);
                    sqlParam.AddStringField("Week1_2", week1_2Calendar.Text);
                    sqlParam.AddStringField("Week2_1", week2_1Calendar.Text);
                    sqlParam.AddStringField("Week2_2", week2_2Calendar.Text);
                    sqlParam.AddStringField("Week3_1", week3_1Calendar.Text);
                    sqlParam.AddStringField("Week3_2", week3_2Calendar.Text);
                    sqlParam.AddStringField("Week4_1", week4_1Calendar.Text);
                    sqlParam.AddStringField("Week4_2", week4_2Calendar.Text);
                    sqlParam.AddBooleanField("Status", Convert.ToBoolean(Convert.ToInt32(rdbStatus.SelectedValue)));
                    sqlParam.AddIntegerField("UpdateBy", Convert.ToInt32(Session["UserID"].ToString()));

                    new CalendarDAO().Update(sqlParam);

                    Page.ClientScript.RegisterStartupScript(
                        GetType(),
                        "Error",
                        "<script type=text/javascript>alert('บันทึกข้อมูลที่แก้ไขเรียบร้อยแล้ว');window.location.replace('CalendarPayGoodsTable.aspx')</script>"
                    );
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ddlMonth.Enabled = false;
            ddlYear.Enabled = false;
            ClearWeeks();
        }

        public static void SetStatus(SaveStatus status)
        {
            _Status = status;
        }

        public static int GetStatus()
        {
            return (int) _Status;
        }
    }
}