using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.DataAccess;
using System.Data.SqlClient;
using System.Text;

namespace GPlus.PRPO
{
    public partial class Routine : Pagebase
    {
        clsFuncMa Func = new clsFuncMa();
        RoutineStockDAO RoutineStockDAO = new RoutineStockDAO();

        public DataTable dtResult
        {
            get { return (ViewState["dtResult"] == null) ? null : (DataTable)ViewState["dtResult"]; }
            set { ViewState["dtResult"] = value; }
        }

        public DataTable dtResultAll
        {
            get { return (ViewState["dtResultAll"] == null) ? null : (DataTable)ViewState["dtResultAll"]; }
            set { ViewState["dtResultAll"] = value; }
        }

        public DataTable dt_Id_Request
        {
            get { return (ViewState["dt_Id_Request"] == null) ? null : (DataTable)ViewState["dt_Id_Request"]; }
            set { ViewState["dt_Id_Request"] = value; }
        }

        public DataTable dt_show_color
        {
            get { return (ViewState["dt_show_color"] == null) ? null : (DataTable)ViewState["dt_show_color"]; }
            set { ViewState["dt_show_color"] = value; }
        }
        
        public DataTable dtDetail
        {
            get { return (ViewState["dtDetail"] == null) ? null : (DataTable)ViewState["dtDetail"]; }
            set { ViewState["dtDetail"] = value; }
        }

        public DataTable dtAllocate
        {
            get { return (ViewState["dtAllocate"] == null) ? null : (DataTable)ViewState["dtAllocate"]; }
            set { ViewState["dtAllocate"] = value; }
        }

        public DataTable dt_Modify
        {
            get { return (ViewState["dt_Modify"] == null) ? null : (DataTable)ViewState["dt_Modify"]; }
            set { ViewState["dt_Modify"] = value; }
        }

        public bool ShowTextBox
        {
            get { return (ViewState["ShowTextBox"] == null) ? false : (bool)ViewState["ShowTextBox"]; }
            set { ViewState["ShowTextBox"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "CallJS", "afterpostback();", true);
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "CallJS", "gridviewScroll();", true);
            }

            

            if (Request.QueryString["id"] != null)
            {
                hid_Summary_ReqId.Value = Request.QueryString["id"];

                fn_check_type();

                //if (Request.QueryString["type"] == null)
                //{

                //    List<SqlParameter> param = new List<SqlParameter>();

                //    param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                //    DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);

                //    if (dt.Rows[0]["Status"].ToString() == "1")
                //    {
                //        List<SqlParameter> param2 = new List<SqlParameter>();

                //        param2.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                //        DataTable dt2 = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request_Chk_Edit", param2);


                //        if (dt2.Rows.Count > 0)
                //        {
                //            //check แล้ว

                //            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "Confirm()", true);
                //            //LinkButton1_ModalPopupExtender.Show();
                //        }
                //        else
                //        {



                //            div_search.Visible = false;
                //            div_data.Visible = true;

                //            ShowTextBox = true;
                //            BindData();
                //            //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                //            BindData_routine_From_ID();
                //            //BindData_routine();
                //            BindGrid_textbox();

                //            btn_Submit_Summary_Print.Enabled = false;
                //            btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();
                //        }
                //    }
                //    else
                //    {
                //        div_search.Visible = false;
                //        div_data.Visible = true;

                //        ShowTextBox = true;
                //        BindData();
                //        //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                //        BindData_routine_From_ID();
                //        //BindData_routine();
                //        BindGrid_textbox();

                //        btn_Submit_Summary_Print.Enabled = false;
                //        btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();
                //    }

                    

                //}
                //else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "N")
                //{
                //    //ดูข้อมูลเดิม


                //    List<SqlParameter> param = new List<SqlParameter>();

                //    param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                //    DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);


                //    div_search.Visible = false;
                //    div_data.Visible = true;

                //    ShowTextBox = false;
                //    BindData();
                //    //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                //    BindData_routine_From_ID_with_type();
                //    //BindData_routine();
                //    BindGrid_textbox();

                //    btn_Submit_Summary_Print.Enabled = false;
                //    btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();

                //}
                //else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "Y")
                //{
                //    //ยืนยัน ดูข้อมูลใหม่


                //    List<SqlParameter> param = new List<SqlParameter>();

                //    param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                //    DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);


                //    div_search.Visible = false;
                //    div_data.Visible = true;

                //    ShowTextBox = false;
                //    //BindData();
                //    //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                //    BindData_routine_with_type();

                //    hid_Summary_Date.Value = Func._fn_ConvertDateStored(dt.Rows[0]["Summary_Date"].ToString());
                //    hid_Stock_id.Value = dt.Rows[0]["Stock_Id"].ToString();
                //    hid_status.Value = dt.Rows[0]["Status"].ToString();
                //    lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[0]["Summary_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));

                    
                //    string Status = "";

                //    switch (dt.Rows[0]["Status"].ToString())
                //    {

                //        case "0":

                //            Status = "ยกเลิก";

                //            break;

                //        case "1":

                //            Status = "พิมพ์สรุป";

                //            break;

                //        case "2":

                //            Status = "Allocate";

                //            break;

                //        case "3":

                //            Status = "จ่ายแล้ว";

                //            break;

                //    }

                //    lbl_status.Text = Status;


                //    //BindData_routine();
                //    BindGrid();

                //    if (dtResult == null)
                //    {
                //        btn_Submit_Summary_Print.Enabled = false;

                //    }
                //    else
                //    {
                //        btn_Submit_Summary_Print.Enabled = true;

                //    }

                //    btn_RePrint.Enabled = false;
                //    btn_Submit_Allocate_Print.Enabled = false;


                    
                //}

                
            }
            else if (Request.QueryString["st_id"] != null)
            {
                div_search.Visible = true;
                hid_Stock_id.Value = Request.QueryString["st_id"];

                if (!IsPostBack)
                {

                    lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", DateTime.Today);

                    Routine_Date.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Today);
                    lbl_Routine_day.Text = String.Format("{0:dddd}", DateTime.Today);
                }

                if (!string.IsNullOrWhiteSpace(hid_Summary_ReqId.Value))
                {
                    BindData();
                    //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                    BindData_routine_From_ID();
                    //BindData_routine();
                    BindGrid_textbox();

                    btn_Submit_Summary_Print.Enabled = false;
                    btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();
                }

            }


            


        }


        private void fn_check_type()
        {
            if (Request.QueryString["type"] == null)
            {

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);

                if (dt.Rows[0]["Status"].ToString() == "1")
                {
                    
                    List<SqlParameter> param2 = new List<SqlParameter>();

                    param2.Add(new SqlParameter("@Summary_Date", Func._fn_ConvertDateStored(dt.Rows[0]["Summary_Date"].ToString())));
                    param2.Add(new SqlParameter("@day_of_week", fn_Day_of_Week(dt.Rows[0]["Summary_Date"].ToString())));
                    param2.Add(new SqlParameter("@Stock_Id_Req", dt.Rows[0]["Stock_Id"].ToString()));
                    param2.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                    dt_Modify = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request_Chk_Edit", param2);


                    if (dt_Modify.Rows.Count > 0)
                    {
                        //check แล้ว

                        //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert2", "Confirm()", true);

                        //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "alert2", "Confirm();", true);

                        LinkButton1_ModalPopupExtender.Show();
                    }
                    else
                    {



                        div_search.Visible = false;
                        div_data.Visible = true;

                        ShowTextBox = true;
                        BindData();
                        //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                        BindData_routine_From_ID();
                        //BindData_routine();
                        BindGrid_textbox();

                        btn_Submit_Summary_Print.Enabled = false;
                        btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();
                    }
                }
                else
                {
                    div_search.Visible = false;
                    div_data.Visible = true;

                    ShowTextBox = true;
                    BindData();
                    //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                    BindData_routine_From_ID();
                    //BindData_routine();
                    BindGrid_textbox();

                    btn_Submit_Summary_Print.Enabled = false;
                    btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();
                }



            }
            else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "N")
            {
                //ดูข้อมูลเดิม


                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);


                div_search.Visible = false;
                div_data.Visible = true;

                ShowTextBox = true;
                BindData();
                //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                BindData_routine_From_ID_with_type();
                //BindData_routine();
                BindGrid_textbox();

                btn_Submit_Summary_Print.Enabled = false;
                btn_Submit_Allocate_Print.Enabled = true;

                btn_back.Visible = true;
                btn_back.Text = "ดูข้อมูลที่เปลี่ยนแปลง";

                btn_back.OnClientClick = "window.location.assign('Routine.aspx?id=" + hid_Summary_ReqId.Value + @"&type=Y')";

            }
            else if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "Y")
            {
                //ยืนยัน ดูข้อมูลใหม่


                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);


                div_search.Visible = false;
                div_data.Visible = true;

                ShowTextBox = true;
                //BindData();
                //Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                

                hid_Summary_Date.Value = Func._fn_ConvertDateStored(dt.Rows[0]["Summary_Date"].ToString());
                hid_Stock_id.Value = dt.Rows[0]["Stock_Id"].ToString();
                hid_status.Value = dt.Rows[0]["Status"].ToString();
                lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[0]["Summary_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));
                hid_Day_of_Week.Value = fn_Day_of_Week(dt.Rows[0]["Summary_Date"].ToString());
                
                BindData_routine_with_type();

                List<SqlParameter> param2 = new List<SqlParameter>();

                param2.Add(new SqlParameter("@Summary_Date", hid_Summary_Date.Value));
                param2.Add(new SqlParameter("@day_of_week", hid_Day_of_Week.Value));
                param2.Add(new SqlParameter("@Stock_Id_Req", hid_Stock_id.Value));
                param2.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                dt_Modify = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request_Chk_Edit", param2);

                string Status = "";

                switch (dt.Rows[0]["Status"].ToString())
                {

                    case "0":

                        Status = "ยกเลิก";

                        break;

                    case "1":

                        Status = "พิมพ์สรุป";

                        break;

                    case "2":

                        Status = "Allocate";

                        break;

                    case "3":

                        Status = "จ่ายแล้ว";

                        break;

                }

                lbl_status.Text = Status;


                //BindData_routine();
                BindGrid();

                if (dtResult == null)
                {
                    btn_Submit_Summary_Print.Enabled = false;

                }
                else
                {
                    btn_Submit_Summary_Print.Enabled = true;

                }

                btn_Submit_Summary_Print.Enabled = false;
                btn_Submit_Allocate_Print.Enabled = true;

                btn_back.Visible = true;
                btn_back.Text = "ดูข้อมูลเดิม";
                
                btn_back.OnClientClick = "window.location.assign('Routine.aspx?id=" + hid_Summary_ReqId.Value + @"&type=N')";


            }
        }


        private void BindData_routine_with_type()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Summary_Date", hid_Summary_Date.Value));
            param.Add(new SqlParameter("@day_of_week", hid_Day_of_Week.Value));
            param.Add(new SqlParameter("@Stock_Id_Req", hid_Stock_id.Value));
            param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

            DataSet ds = new DatabaseHelper().ExecuteDataSet("sp_Inv_Select_Routine_Stock_DIFF", param);

            if (ds != null)
            {
                //uc_pagenavigator1.ItemCount = int.Parse(ds.Tables[0].Rows[0]["count_row"].ToString());
                dtDetail = ds.Tables[0];
                if (ds.Tables.Count > 1)
                {
                    dtResultAll = ds.Tables[1];
                    dtResult = ds.Tables[2];
                    dt_Id_Request = ds.Tables[3];
                    dt_show_color = ds.Tables[4];


                    if (dtResult.Rows.Count > 0)
                    {
                        txt_Number_Of_Req.Text = dtDetail.Rows[0]["Number_Of_Req"].ToString();
                        txt_Number_Of_Routine.Text = dtDetail.Rows[0]["Number_Of_Routine"].ToString();
                        txt_Number_Of_Stat.Text = dtDetail.Rows[0]["Number_Of_Stat"].ToString();
                        txt_Number_Of_NotRoutine.Text = dtDetail.Rows[0]["Number_Of_NotRoutine"].ToString();

                        txt_Number_Pending_InDue.Text = dtDetail.Rows[0]["Number_Pending_InDue"].ToString();
                        txt_Number_Pending_OutDue.Text = dtDetail.Rows[0]["Number_Pending_OutDue"].ToString();
                    }

                    //List<SqlParameter> param = new List<SqlParameter>();

                    //param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                    //DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);

                    //hid_Summary_Date.Value = Func._fn_ConvertDateStored(dt.Rows[0]["Summary_Date"].ToString());
                    //hid_Stock_id.Value = dt.Rows[0]["Stock_Id"].ToString();
                    //hid_status.Value = dt.Rows[0]["Status"].ToString();
                    //lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[0]["Summary_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));

                    //txt_Number_Of_Req.Text = dt.Rows[0]["Number_Of_Req"].ToString();
                    //txt_Number_Of_Routine.Text = dt.Rows[0]["Number_Of_Routine"].ToString();
                    //txt_Number_Of_Stat.Text = dt.Rows[0]["Number_Of_Stat"].ToString();
                    //txt_Number_Of_NotRoutine.Text = dt.Rows[0]["Number_Of_NotRoutine"].ToString();

                    //txt_Number_Pending_InDue.Text = dt.Rows[0]["Number_Pending_InDue"].ToString();
                    //txt_Number_Pending_OutDue.Text = dt.Rows[0]["Number_Pending_OutDue"].ToString();

                    //string Status = "";

                    //switch (dt.Rows[0]["Status"].ToString())
                    //{

                    //    case "0":

                    //        Status = "ยกเลิก";

                    //        break;

                    //    case "1":

                    //        Status = "พิมพ์สรุป";

                    //        break;

                    //    case "2":

                    //        Status = "Allocate";

                    //        break;

                    //    case "3":

                    //        Status = "จ่ายแล้ว";

                    //        break;

                    //}

                    //lbl_status.Text = Status;

                }
            }
        }


        private void BindData_routine_From_ID_with_type()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

            DataSet ds = new DatabaseHelper().ExecuteDataSet("sp_Inv_Select_Routine_Stock_From_ID_DIFF", param);

            if (ds != null)
            {
                //uc_pagenavigator1.ItemCount = int.Parse(ds.Tables[0].Rows[0]["count_row"].ToString());
                dtDetail = ds.Tables[0];
                if (ds.Tables.Count > 1)
                {
                    dtResultAll = ds.Tables[1];
                    dtResult = ds.Tables[2];
                    dtAllocate = ds.Tables[3];
                    dt_show_color = ds.Tables[4];
                }
            }

            //dtResult = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Routine_Stock");
        }



        private bool fn_chk_summaryreq_Item_pay()
        {
            if (dtResultAll != null)
            {
                DataRow[] drow = dtResultAll.Select("Pay_Qty is not null and Pay_Qty > 0");

                if (drow.Length > 0)
                
                    return false;
                
                else
                    return true;
            }
            else
                return true;
        }


        private void fn_Printing(Boolean RePrint)
        {
            if (dtResult.Columns.Count - 6 > 25)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่สามารถพิมพ์หน่วยงานเกิน 25 หน่วย');", true);
                return;
            }

            /* •—————————————————————————•
               | Update                  |
               | Number_Of_Reprint  (+1) |
               | Latest_Reprint_By       |
               | Latest_Reprint_Date     |
               •—————————————————————————• */
            //bool result = RoutineStockDAO.Update_Inv_SummaryReq(hid_Summary_ReqId.Value, this.UserID); 
            bool result = true;

            if (!result)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบพิมพ์ขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
                return;
            }


            DataSetReprot2 _DataSetReprot2 = new DataSetReprot2();

            

            _DataSetReprot2.Tables["DateTime"].Rows.Add((Convert.ToDateTime(hid_Summary_Date.Value, new System.Globalization.CultureInfo("en-US"))).ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH")));
            //String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(hid_Summary_Date.Value, new System.Globalization.CultureInfo("th-TH")))



            for (int rows = 0; rows < gvResult_routine.Rows.Count; rows++)
            {

                _DataSetReprot2.Tables["DataReprot"].Rows.Add();
                int dt_col = 0;
                int x = 0;
                int tmp_col_DataReprot = 0;
                for (int col = 0; col < gvResult_routine.Rows[rows].Cells.Count; col++)
                {
                    if (col >= 4 && col < (gvResult_routine.Rows[rows].Cells.Count - 3)) //+1
                    {
                        if ((col % 2) != 0)
                        {
                            x++;
                            if (RePrint)
                            {
                                TextBox text2 = (TextBox)gvResult_routine.Rows[rows].FindControl(String.Format("2{0}{1}", rows, col - x));
                                _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1]["Data" + tmp_col_DataReprot] = ((text2 != null) ? text2.Text : "");
                            }
                            tmp_col_DataReprot++;
                        }
                        else
                        {
                            //_DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1]["Description" + tmp_col_DataReprot] = dtResultAll.Rows[tmp_col_DataReprot]["Description"];

                            string columnName = "";

                            DataRow[] drow = dtResultAll.Select("ID = '" + dtResult.Columns[tmp_col_DataReprot + 5].ColumnName + "'");

                            if (drow.Length > 0)
                            {
                                columnName = drow[0]["Description"].ToString();
                            }
                            
                            //_DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1]["Description" + tmp_col_DataReprot] = dtResult.Columns[tmp_col_DataReprot + 5].ColumnName;
                            _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1]["Description" + tmp_col_DataReprot] = columnName;
                            Label lb = (Label)gvResult_routine.Rows[rows].FindControl(String.Format("L2{0}{1}", rows, col - x));
                            _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1]["DataL" + tmp_col_DataReprot] = ((lb != null) ? lb.Text : "");
                        }
                    }
                    else
                    {
                        if (gvResult_routine.Rows[rows].Cells.Count == col + 1 && RePrint)
                        {
                            TextBox text2 = (TextBox)gvResult_routine.Rows[rows].FindControl(String.Format("3{0}", rows));
                            _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1]["Allocate"] = ((text2 != null) ? text2.Text : "");
                        }
                        else
                        {
                            if (col == 0)
                            {
                                Label lbl_no = (Label)gvResult_routine.Rows[rows].FindControl("lbl_No");
                                _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1][col] = lbl_no.Text;
                            }
                            else if (col > 0 && col <= 3)
                                _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1][col] = gvResult_routine.Rows[rows].Cells[col].Text != "&nbsp;" ? gvResult_routine.Rows[rows].Cells[col].Text : "";
                            else
                                _DataSetReprot2.Tables["DataReprot"].Rows[_DataSetReprot2.Tables["DataReprot"].Rows.Count - 1][_DataSetReprot2.Tables["DataReprot"].Columns.Count - (gvResult_routine.Rows[rows].Cells.Count - col)] = gvResult_routine.Rows[rows].Cells[col].Text != "&nbsp;" ? gvResult_routine.Rows[rows].Cells[col].Text : "";
                        }
                        dt_col++;
                    }

                }
            }

            this.Session["dtResultAll"] = _DataSetReprot2;
            if (dtResultAll != null)
                Response.Redirect("TempRPT_Routine.aspx?id=" + hid_Summary_ReqId.Value);
        }

        private void BindData()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);

            hid_Summary_Date.Value = Func._fn_ConvertDateStored(dt.Rows[0]["Summary_Date"].ToString());
            hid_Stock_id.Value = dt.Rows[0]["Stock_Id"].ToString();
            hid_status.Value = dt.Rows[0]["Status"].ToString();
            lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[0]["Summary_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));

            txt_Number_Of_Req.Text = dt.Rows[0]["Number_Of_Req"].ToString();
            txt_Number_Of_Routine.Text = dt.Rows[0]["Number_Of_Routine"].ToString();
            txt_Number_Of_Stat.Text = dt.Rows[0]["Number_Of_Stat"].ToString();
            txt_Number_Of_NotRoutine.Text = dt.Rows[0]["Number_Of_NotRoutine"].ToString();

            txt_Number_Pending_InDue.Text = dt.Rows[0]["Number_Pending_InDue"].ToString();
            txt_Number_Pending_OutDue.Text = dt.Rows[0]["Number_Pending_OutDue"].ToString();

            string Status = "";

            switch (dt.Rows[0]["Status"].ToString())
            {

                case "0":

                    Status = "ยกเลิก";

                    break;

                case "1":

                    Status = "พิมพ์สรุป";

                    break;

                case "2":

                    Status = "Allocate";

                    break;

                case "3":

                    Status = "จ่ายแล้ว";

                    break;

            }

            lbl_status.Text = Status;



        }

        private string fn_Day_of_Week(string day)
        {
            string Day_of_Week = "";
            string Day = Convert.ToDateTime(day).DayOfWeek.ToString();

            switch (Day)
            {

                case "Monday":

                    Day_of_Week = "1";

                    break;

                case "Tuesday":

                    Day_of_Week = "2";

                    break;

                case "Wednesday":

                    Day_of_Week = "3";

                    break;

                case "Thursday":

                    Day_of_Week = "4";

                    break;

                case "Friday":

                    Day_of_Week = "5";

                    break;

                case "Saturday":

                    Day_of_Week = "6";

                    break;

                case "Sunday":

                    Day_of_Week = "7";

                    break;
            }

            return Day_of_Week;
        
        }

        protected void btn_routine_search_Click(object sender, EventArgs e)
        {
            div_data.Visible = true;

            lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", Convert.ToDateTime(Routine_Date.Text));
            lbl_Routine_day.Text = String.Format("{0:dddd}", Convert.ToDateTime(Routine_Date.Text));

            hid_Summary_Date.Value = Func._fn_ConvertDateStored(Routine_Date.Text.Trim());
            hid_Day_of_Week.Value = fn_Day_of_Week(Routine_Date.Text.Trim());

            //CLEAR//
            txt_Number_Of_Req.Text = "";
            txt_Number_Of_Routine.Text = "";
            txt_Number_Of_Stat.Text = "";
            txt_Number_Of_NotRoutine.Text = "";

            txt_Number_Pending_InDue.Text = "";
            txt_Number_Pending_OutDue.Text = "";

            lbl_status.Text = "รอการ Allocate";

            //CLEAR//

            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Stock_Id", hid_Stock_id.Value));
            param.Add(new SqlParameter("@Summary_Date", hid_Summary_Date.Value));

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);

            if (dt.Rows.Count > 0)
            {
                hid_Summary_ReqId.Value = dt.Rows[0]["Summary_ReqId"].ToString();

                fn_check_type();


                ////BindData_routine_From_ID();
                ////BindGrid_textbox();
                //ShowTextBox = true;
                //BindData();
                ////Bind data From Inv_SummaryReq_Detail/Inv_SummaryReq_Item
                //BindData_routine_From_ID();
                ////BindData_routine();
                //BindGrid_textbox();

                //btn_Submit_Summary_Print.Enabled = false;
                //btn_Submit_Allocate_Print.Enabled = fn_chk_summaryreq_Item_pay();
                //btn_RePrint.Enabled = true;
            }
            else
            {
                ShowTextBox = false;
                BindData_routine();
                BindGrid();

                if (dtResult == null)
                {
                    btn_Submit_Summary_Print.Enabled = false;
                    
                }
                else
                {
                    btn_Submit_Summary_Print.Enabled = true;
                    
                }

                btn_RePrint.Enabled = false;
                btn_Submit_Allocate_Print.Enabled = false;
            }



            

        }

        private void BindData_routine_From_ID()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

            DataSet ds = new DatabaseHelper().ExecuteDataSet("sp_Inv_Select_Routine_Stock_From_ID", param);

            if (ds != null)
            {
                //uc_pagenavigator1.ItemCount = int.Parse(ds.Tables[0].Rows[0]["count_row"].ToString());
                dtDetail = ds.Tables[0];
                if (ds.Tables.Count > 1)
                {
                    dtResultAll = ds.Tables[1];
                    dtResult = ds.Tables[2];
                    dtAllocate = ds.Tables[3];
                    dt_show_color = ds.Tables[4];
                }
            }

            //dtResult = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Routine_Stock");
        }


        private void BindData_routine()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Summary_Date", hid_Summary_Date.Value));
            param.Add(new SqlParameter("@day_of_week", hid_Day_of_Week.Value));
            param.Add(new SqlParameter("@Stock_Id_Req", hid_Stock_id.Value));

            DataSet ds = new DatabaseHelper().ExecuteDataSet("sp_Inv_Select_Routine_Stock", param);

            if (ds != null)
            {
                //uc_pagenavigator1.ItemCount = int.Parse(ds.Tables[0].Rows[0]["count_row"].ToString());
                dtDetail = ds.Tables[0];
                if (ds.Tables.Count > 4)
                {
                    dtResultAll = ds.Tables[1];
                    dtResult = ds.Tables[2];
                    dt_Id_Request = ds.Tables[3];
                    dt_show_color = ds.Tables[4];


                    if (dtResult.Rows.Count > 0)
                    {
                        txt_Number_Of_Req.Text = dtDetail.Rows[0]["Number_Of_Req"].ToString();
                        txt_Number_Of_Routine.Text = dtDetail.Rows[0]["Number_Of_Routine"].ToString();
                        txt_Number_Of_Stat.Text = dtDetail.Rows[0]["Number_Of_Stat"].ToString();
                        txt_Number_Of_NotRoutine.Text = dtDetail.Rows[0]["Number_Of_NotRoutine"].ToString();

                        txt_Number_Pending_InDue.Text = dtDetail.Rows[0]["Number_Pending_InDue"].ToString();
                        txt_Number_Pending_OutDue.Text = dtDetail.Rows[0]["Number_Pending_OutDue"].ToString();
                    }
                }
                else
                {
                    dtResultAll = null;
                    dtResult = null;

                }
            }

            //dtResult = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Routine_Stock");
        }

        private void BindGrid_textbox()
        {
            gvResult_routine.Columns.Clear();

            BoundField b = new BoundField();
            b.HeaderStyle.Width = 50;
            b.DataField = "Inv_ItemId";
            b.HeaderText = "ลำดับที่";
            b.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvResult_routine.Columns.Add(b);

            BoundField b2 = new BoundField();
            b2.HeaderStyle.Width = 180;
            b2.DataField = "Inv_ItemName";
            b2.HeaderText = "รายการ";
            b2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            gvResult_routine.Columns.Add(b2);

            BoundField b3 = new BoundField();
            b3.HeaderStyle.Width = 90;
            b3.DataField = "Inv_ItemCode";
            b3.HeaderText = "รหัส";
            b3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            b3.ItemStyle.Width = 90;
            gvResult_routine.Columns.Add(b3);

            BoundField b4 = new BoundField();
            b4.HeaderStyle.Width = 70;
            b4.DataField = "ItemDescription";
            b4.HeaderText = "หน่วยนับ";
            b4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b4.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvResult_routine.Columns.Add(b4);


            int CountColumn = dtResult.Columns.Count - 2;

            for (int i = 5; i <= CountColumn; i++)
            {
                BoundField b5 = new BoundField();
                b5.HeaderText = "เบิก";
                b5.HeaderStyle.Width = 40;
                b5.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b5);
                //TemplateField T1 = new TemplateField();
                // string data = dtResult.Columns[i].ColumnName;
                // T1.HeaderText = "เบิก";
                // T1.ItemTemplate = new GridViewTemplate(ListItemType.Item, data, "txt_1");
                // gvResult_routine.Columns.Add(T1);

                BoundField b6 = new BoundField();

                b6.HeaderText = "จ่าย";
                b6.HeaderStyle.Width = 40;
                b6.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b6);
                //TemplateField T2 = new TemplateField();
                //T2.HeaderText = "จ่าย";
                //T2.ItemTemplate = new GridViewTemplate(ListItemType.EditItem, "", "txt_2");
                //gvResult_routine.Columns.Add(T2);

            }


            BoundField b7 = new BoundField();
            b7.DataField = "Grand Total";
            b7.HeaderText = "รวมเบิก";
            b7.HeaderStyle.Width = 60;
            b7.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b7.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvResult_routine.Columns.Add(b7);

            BoundField b8 = new BoundField();
            b8.DataField = "OnHand_Qty";
            //b8.DataFormatString = "{0:#}";
            b8.HeaderStyle.Width = 60;
            b8.HeaderText = "ยอดคงคลัง";
            b8.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b8.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvResult_routine.Columns.Add(b8);

            BoundField b9 = new BoundField();
            b9.HeaderStyle.Width = 60;
            b9.HeaderText = "Allocate";
            b9.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            b9.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            gvResult_routine.Columns.Add(b9);



            int width = (((dtResult.Columns.Count - 6) * 80) + 550);

            if (width < 786)
                width = 786;

            gvResult_routine.Width = width;
            //div_gvResult_routine.Style.Add("overflow-x", "scroll");

            gvResult_routine.DataSource = dtResult;
            gvResult_routine.DataBind();


            //gvResult_routine.Columns.Clear();

            //BoundField b = new BoundField();
            //b.DataField = "Inv_ItemId";

            //b.HeaderText = "ลำดับที่";
            //gvResult_routine.Columns.Add(b);

            //BoundField b2 = new BoundField();
            //b2.DataField = "Inv_ItemName";
            //b2.HeaderText = "รายการ";
            //b2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
            //gvResult_routine.Columns.Add(b2);

            //BoundField b3 = new BoundField();
            //b3.DataField = "Inv_ItemCode";
            //b3.HeaderText = "รหัส";
            //gvResult_routine.Columns.Add(b3);

            //BoundField b4 = new BoundField();
            //b4.DataField = "ItemDescription";
            //b4.HeaderText = "หน่วยนับ";
            //gvResult_routine.Columns.Add(b4);


            //int CountColumn = dtResult.Columns.Count - 2;

            //for (int i = 5; i <= CountColumn; i++)
            //{
            //    BoundField b5 = new BoundField();
            //    b5.HeaderText = "เบิก";
            //    gvResult_routine.Columns.Add(b5);
            //    //TemplateField T1 = new TemplateField();
            //    // string data = dtResult.Columns[i].ColumnName;
            //    // T1.HeaderText = "เบิก";
            //    // T1.ItemTemplate = new GridViewTemplate(ListItemType.Item, data, "txt_1");
            //    // gvResult_routine.Columns.Add(T1);

            //    BoundField b6 = new BoundField();

            //    b6.HeaderText = "จ่าย";
            //    gvResult_routine.Columns.Add(b6);
            //    //TemplateField T2 = new TemplateField();
            //    //T2.HeaderText = "จ่าย";
            //    //T2.ItemTemplate = new GridViewTemplate(ListItemType.EditItem, "", "txt_2");
            //    //gvResult_routine.Columns.Add(T2);

            //}


            //BoundField b7 = new BoundField();
            //b7.DataField = "Grand Total";
            //b7.HeaderText = "รวมเบิก";
            //gvResult_routine.Columns.Add(b7);

            //BoundField b8 = new BoundField();
            //b8.DataField = "OnHand_Qty";
            ////b8.DataFormatString = "{0:#}";

            //b8.HeaderText = "ยอด<br>คงคลัง";
            //gvResult_routine.Columns.Add(b8);

            //BoundField b9 = new BoundField();

            //b9.HeaderText = "Allocate";
            //gvResult_routine.Columns.Add(b9);



            //int width = (((dtResult.Columns.Count - 6) * 80) + 570);
            
            //if (width < 786)
            //    width = 786;
 
            //gvResult_routine.Width = width;
            //div_gvResult_routine.Style.Add("overflow-x", "scroll");

            //gvResult_routine.DataSource = dtResult;
            //gvResult_routine.DataBind();

        }

        private void BindGrid()
        {
            string width = "";

            gvResult_routine.Columns.Clear();
            if (dtResult != null)
            {

                BoundField b = new BoundField();
                b.HeaderStyle.Width = 50;
                b.DataField = "Inv_ItemId";
                b.HeaderText = "ลำดับที่";
                b.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b);

                BoundField b2 = new BoundField();
                b2.HeaderStyle.Width = 180;
                b2.DataField = "Inv_ItemName";
                b2.HeaderText = "รายการ";
                b2.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                gvResult_routine.Columns.Add(b2);

                BoundField b3 = new BoundField();
                b3.HeaderStyle.Width = 90;
                b3.DataField = "Inv_ItemCode";
                b3.HeaderText = "รหัส";
                b3.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b3.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                b3.ItemStyle.Width = 90;
                gvResult_routine.Columns.Add(b3);

                BoundField b4 = new BoundField();
                b4.HeaderStyle.Width = 70;
                b4.DataField = "ItemDescription";
                b4.HeaderText = "หน่วยนับ";
                b4.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b4.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b4);


                int CountColumn = dtResult.Columns.Count - 2;

                for (int i = 5; i <= CountColumn; i++)
                {
                    BoundField b5 = new BoundField();
                    b5.HeaderText = "เบิก";
                    b5.HeaderStyle.Width = 40;
                    b5.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    b5.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    gvResult_routine.Columns.Add(b5);
                    

                    BoundField b6 = new BoundField();
                    b6.HeaderText = "จ่าย";
                    b6.HeaderStyle.Width = 40;
                    b6.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    b6.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    gvResult_routine.Columns.Add(b6);

                }


                BoundField b7 = new BoundField();
                b7.DataField = "Grand Total";
                b7.HeaderText = "รวมเบิก";
                b7.HeaderStyle.Width = 60;
                b7.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b7.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b7);

                BoundField b8 = new BoundField();
                b8.DataField = "OnHand_Qty";
                //b8.DataFormatString = "{0:#}";
                b8.HeaderStyle.Width = 60;
                b8.HeaderText = "ยอดคงคลัง";
                b8.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b8.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b8);

                BoundField b9 = new BoundField();
                b9.HeaderStyle.Width = 60;
                b9.HeaderText = "Allocate";
                b9.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                b9.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                gvResult_routine.Columns.Add(b9);


                //BoundField b = new BoundField();
                //b.DataField = "Inv_ItemId";

                //b.HeaderText = "ลำดับที่";
                //gvResult_routine.Columns.Add(b);

                //BoundField b2 = new BoundField();
                //b2.DataField = "Inv_ItemName";
                //b2.ItemStyle.Width = 120;
                //b2.HeaderText = "รายการ";
                //b2.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
                //gvResult_routine.Columns.Add(b2);

                //BoundField b3 = new BoundField();
                //b3.DataField = "Inv_ItemCode";
                //b3.HeaderText = "รหัส";
                //gvResult_routine.Columns.Add(b3);

                //BoundField b4 = new BoundField();
                //b4.DataField = "ItemDescription";
                //b4.HeaderText = "หน่วยนับ";
                //gvResult_routine.Columns.Add(b4);


                //int CountColumn = dtResult.Columns.Count - 2;

                //for (int i = 5; i <= CountColumn; i++)
                //{
                //    BoundField b5 = new BoundField();
                //    b5.DataField = dtResult.Columns[i].ColumnName;
                //    b5.HeaderText = "เบิก";
                //    gvResult_routine.Columns.Add(b5);

                //    BoundField b6 = new BoundField();
                //    //b6.DataField = dtResult.Columns[i].ColumnName;
                //    b6.HeaderText = "จ่าย";
                //    gvResult_routine.Columns.Add(b6);
                //}


                //BoundField b7 = new BoundField();
                //b7.DataField = "Grand Total";
                //b7.HeaderText = "รวมเบิก";
                //gvResult_routine.Columns.Add(b7);

                //BoundField b8 = new BoundField();
                //b8.DataField = "OnHand_Qty";
                ////b8.DataFormatString = "{0:#}";
                //b8.HeaderText = "ยอด<br>คงคลัง";
                //gvResult_routine.Columns.Add(b8);

                //BoundField b9 = new BoundField();

                //b9.HeaderText = "Allocate";
                //gvResult_routine.Columns.Add(b9);



                width = (((dtResult.Columns.Count - 6) * 80) + 550).ToString();

                if (int.Parse(width) < 786)
                    width = "786";
                
                gvResult_routine.Width = int.Parse(width);
                //div_gvResult_routine.Style.Add("overflow-x", "scroll");

            }
            else
            {
                width = "786";
                gvResult_routine.Width = int.Parse(width);
                //div_gvResult_routine.Style.Add("overflow-x", "visible");
            }

            

            
            gvResult_routine.DataSource = dtResult;
            gvResult_routine.DataBind();


        }

        protected void gvResult_routine_RowCreated(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    GridViewRow headerow = new GridViewRow(0, 0, DataControlRowType.Header,
            //                                                DataControlRowState.Insert);
            //    e.Row.Cells.Clear();


            //    TableCell headercell1 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 50,
            //        Text = "ลำดับที่",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell1);


            //    TableCell headercell2 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 180,
            //        Text = "รายการ",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell2);


            //    TableCell headercell3 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 80,
            //        Text = "รหัส",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell3);

            //    TableCell headercell4 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 80,
            //        Text = "หน่วยนับ",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell4);


            //    GridViewRow headerow2 = new GridViewRow(0, 0, DataControlRowType.Header,
            //                                                DataControlRowState.Insert);


            //    int CountColumn = dtResult.Columns.Count - 2;

            //    int j = 1;
            //    string header = "header" + j.ToString();

            //    for (int i = 5; i <= CountColumn; i++)
            //    {
            //        string columnName = "";

            //        DataRow[] drow = dtResultAll.Select("ID = '" + dtResult.Columns[i].ColumnName + "'");

            //        if (drow.Length > 0)
            //        {
            //            columnName = drow[0]["Description"].ToString();
            //        }

            //        TableCell headercell5 = new TableCell()
            //        {
            //            ColumnSpan = 2,
            //            RowSpan = 1,
            //            Height = 20,
            //            Width = 80,
            //            Text = columnName,
            //            HorizontalAlign = HorizontalAlign.Center
            //        };
            //        headerow.Cells.Add(headercell5);

            //        TableCell headercell6 = new TableCell()
            //        {
            //            ColumnSpan = 1,
            //            RowSpan = 1,
            //            Height = 20,
            //            Width = 40,
            //            Text = "เบิก",
            //            HorizontalAlign = HorizontalAlign.Center
            //        };
            //        headerow2.Cells.Add(headercell6);

            //        TableCell headercell7 = new TableCell()
            //        {
            //            ColumnSpan = 1,
            //            RowSpan = 1,
            //            Height = 20,
            //            Width = 40,
            //            Text = "จ่าย",
            //            HorizontalAlign = HorizontalAlign.Center
            //        };
            //        headerow2.Cells.Add(headercell7);
            //    }


            //    TableCell headercell20 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 60,
            //        Text = "รวมเบิก",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell20);

            //    TableCell headercell21 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 60,
            //        Text = "ยอด<br>คงคลัง",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell21);


            //    TableCell headercell22 = new TableCell()
            //    {
            //        ColumnSpan = 1,
            //        RowSpan = 2,
            //        Height = 40,
            //        Width = 60,
            //        Text = "Allocate",
            //        HorizontalAlign = HorizontalAlign.Center
            //    };
            //    headerow.Cells.Add(headercell22);

            //    gvResult_routine.Controls[0].Controls.AddAt(0, headerow);
            //    gvResult_routine.Controls[0].Controls.AddAt(1, headerow2);

            //    gvResult_routine.HeaderStyle.CssClass = "HeaderFreez";



            //}


            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow gvHeaderRow = e.Row;
                GridViewRow gvHeaderRowCopy = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                this.gvResult_routine.Controls[0].Controls.AddAt(0, gvHeaderRowCopy);

                int headerCellCount = gvHeaderRow.Cells.Count;
                int cellIndex = 0;


                for (int i = 0; i < headerCellCount; i++)
                {
                    if ((i >= 0 && i <= 3) || (i >= (headerCellCount - 3) && i < headerCellCount))
                    {
                        TableCell tcHeader = gvHeaderRow.Cells[cellIndex];
                        tcHeader.RowSpan = 2;
                        gvHeaderRowCopy.Cells.Add(tcHeader);
                    }
                    else
                    {
                        cellIndex++;
                    }
                }

                int CountColumn = dtResult.Columns.Count - 2;


                for (int j = 5; j <= CountColumn; j++)
                {
                    string columnName = "";

                    DataRow[] drow = dtResultAll.Select("ID = '" + dtResult.Columns[j].ColumnName + "'");

                    if (drow.Length > 0)
                    {
                        columnName = drow[0]["Description"].ToString();
                    }

                    TableCell tcMerge = new TableCell();
                    tcMerge.Text = columnName;
                    tcMerge.ColumnSpan = 2;
                    tcMerge.HorizontalAlign = HorizontalAlign.Center;
                    gvHeaderRowCopy.Cells.AddAt(j - 1, tcMerge);


                }


            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal Grand_tatal = decimal.Parse(dtResult.Rows[e.Row.RowIndex]["Grand Total"].ToString());
                decimal OnHand_Qty = decimal.Parse(dtResult.Rows[e.Row.RowIndex].IsNull("OnHand_Qty") ? "0" : dtResult.Rows[e.Row.RowIndex]["OnHand_Qty"].ToString());

                string status = "XX";

                if(dtDetail != null)
                {
                    if(dtDetail.Columns.Contains("Status"))
                        status = dtDetail.Rows[0]["Status"].ToString();
                }

                Boolean IsFillData = false;

                string allocate = "";

                if (hid_Summary_ReqId.Value != "" && Request.QueryString["type"] == null)
                {
                    allocate = dtAllocate.Rows[e.Row.RowIndex][dtAllocate.Columns[dtAllocate.Columns.Count - 1].ColumnName].ToString();
                }

                //if ((allocate == "" || allocate == "0") && hid_Summary_ReqId.Value == "")
                //{
                //    if (Grand_tatal <= OnHand_Qty)
                //        IsFillData = true;
                //}

                if (status == "1")
                {
                    if (Grand_tatal <= OnHand_Qty)
                        IsFillData = true;
                    else
                        IsFillData = false;
                }
                else if (status == "XX")
                {
                    IsFillData = false;
                }
                else
                {
                    IsFillData = true;
                }

                Label lbl_no = new Label();

                lbl_no.Visible = true;
                lbl_no.ID = "lbl_No";
                lbl_no.Text = (e.Row.RowIndex + 1).ToString();
                lbl_no.Width = 50;
                lbl_no.Attributes.Add("style", "text-align:center");
                //t1.Enabled = false;
                
                TableCell tcNo = new TableCell();
                tcNo.Controls.Add(lbl_no);

                e.Row.Cells.RemoveAt(0);
                e.Row.Cells.AddAt(0, tcNo);

                
                int CountColumn = dtResult.Columns.Count - 2;
                int column = 4;


                if (ShowTextBox)
                {
                    for (int i = 5; i <= CountColumn; i++)
                    {
                        Label t1 = new Label();
                        t1.ID = String.Format("L2{0}{1}", e.Row.RowIndex, (i - 1));
                        t1.Visible = true;
                        t1.Text = dtResult.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString();

                        t1.ToolTip = dtResult.Rows[e.Row.RowIndex][dtResult.Columns[1]].ToString();


                        if (dt_show_color.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString() != "")
                        {
                            if (int.Parse(dt_show_color.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString()) > 0)
                                t1.Attributes.Add("style", "color: Red");
                        }

                        t1.Width = 35;
                        //t1.Enabled = false;
                        TableCell tc = new TableCell();
                        tc.Controls.Add(t1);
                        e.Row.Cells.RemoveAt(column);
                        e.Row.Cells.AddAt(column, tc);
                        column++;

                        TextBox t2 = new TextBox();
                        t2.ID = String.Format("2{0}{1}", e.Row.RowIndex, (i - 1));
                        t2.Visible = true;
                        t2.Attributes.Add("onkeypress", "return isNumericKey(event);");
                        t2.Attributes.Add("style", "text-align:center");

                        //if (IsFillData)
                        //    t2.Text = t1.Text;

                        //if (IsFillData)
                        //{
                        //    t2.Text = t1.Text;
                        //}
                        //else
                        //{
                        //    if (allocate == "0" && hid_Summary_ReqId.Value == "")
                        //        t2.Text = "";
                        //    else
                        //        t2.Text = dtAllocate.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString();

                        //    //txtAllocate.Text = allocate;
                        //}

                        if (!IsPostBack)
                        {
                            if (IsFillData)
                            {
                                if (status == "1")
                                    t2.Text = t1.Text;
                                else
                                    t2.Text = dtAllocate.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString();
                            }
                        }
                        else if (hid_Summary_ReqId.Value != "")
                        {
                            if (IsFillData)
                            {
                                if (status == "1")
                                {
                                    if (hid_chk_postback.Value == "" || hid_chk_postback.Value != dtResult.Rows.Count.ToString())
                                        t2.Text = t1.Text;
                                    else
                                        t2.Text = " ";
                                }
                                else
                                {
                                    if (hid_chk_postback.Value == "" || hid_chk_postback.Value != dtResult.Rows.Count.ToString())
                                        t2.Text = dtAllocate.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString();
                                    else
                                        t2.Text = " ";
                                }
                            }
                        }


                        t2.Width = 35;


                        if (t1.Text.Equals(""))
                        {
                            t2.Enabled = false;
                        }
                        else
                        {
                            t2.AutoPostBack = true;
                            t2.TextChanged += new EventHandler(txt_TextChanged);
                        }

                        HiddenField H1 = new HiddenField();
                        H1.ID = String.Format("hid2{0}{1}", e.Row.RowIndex, (i - 1));

                        H1.Value = t2.Text;


                        TableCell tc2 = new TableCell();
                        tc2.Controls.Add(t2);
                        tc2.Controls.Add(H1);
                        e.Row.Cells.RemoveAt(column);
                        e.Row.Cells.AddAt(column, tc2);

                        column++;
                    }

                    TextBox txtAllocate = new TextBox();
                    txtAllocate.ID = String.Format("3{0}", e.Row.RowIndex);
                    txtAllocate.Visible = true;
                    txtAllocate.Attributes.Add("onkeypress", "return isNumericKey(event);");
                    txtAllocate.Attributes.Add("style", "text-align:center");



                    //if (IsFillData)
                    //{
                    //    //string allocate = dtAllocate.Rows[e.Row.RowIndex][dtAllocate.Columns[dtAllocate.Columns.Count-1].ColumnName].ToString();

                    //    txtAllocate.Text = Grand_tatal.ToString();
                    //}
                    //else
                    //{
                    //    if (allocate == "0" && hid_Summary_ReqId.Value == "")
                    //        txtAllocate.Text = "";
                    //    else
                    //        txtAllocate.Text = allocate;

                    //    //txtAllocate.Text = allocate;
                    //}
                    if (!IsPostBack)
                    {
                        if (IsFillData)
                        {
                            if (status == "1")
                                txtAllocate.Text = Grand_tatal.ToString();
                            else
                                txtAllocate.Text = allocate;
                        }
                        //else 
                        //{
                        //    if (Grand_tatal > OnHand_Qty) 
                        //    {
                        //        txtAllocate.Text = "0";   
                        //    }
                        //}
                    }
                    else if (hid_Summary_ReqId.Value != "")
                    {
                        if (IsFillData)
                        {
                            if (status == "1")
                            {
                                if (hid_chk_postback.Value == "" || hid_chk_postback.Value != dtResult.Rows.Count.ToString())
                                    txtAllocate.Text = Grand_tatal.ToString();
                                else
                                    txtAllocate.Text = " ";
                            }
                            else
                            {
                                if (hid_chk_postback.Value == "" || hid_chk_postback.Value != dtResult.Rows.Count.ToString())
                                    txtAllocate.Text = allocate;
                                else
                                    txtAllocate.Text = " ";
                            }
                        }
                    }

                    HiddenField hid_allocate = new HiddenField();
                    hid_allocate.ID = String.Format("hidAllocate", e.Row.RowIndex);

                    hid_allocate.Value = txtAllocate.Text;

                    txtAllocate.Width = 35;
                    txtAllocate.AutoPostBack = true;
                    txtAllocate.TextChanged += new EventHandler(txtAllocate_TextChanged);
                    TableCell tcAllow = new TableCell();
                    tcAllow.Controls.Add(txtAllocate);
                    tcAllow.Controls.Add(hid_allocate);

                    e.Row.Cells.RemoveAt(gvResult_routine.Columns.Count - 1);
                    e.Row.Cells.AddAt(gvResult_routine.Columns.Count - 1, tcAllow);
                }

                else
                {
                    for (int i = 5; i <= CountColumn; i++)
                    {

                        Label t1 = new Label();
                        t1.ID = String.Format("L2{0}{1}", e.Row.RowIndex, (i - 1));
                        t1.Visible = true;
                        t1.Text = dtResult.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString();
                        t1.ToolTip = dtResult.Rows[e.Row.RowIndex][dtResult.Columns[1]].ToString();

                        if (dt_show_color.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString() != "")
                        {
                            if (int.Parse(dt_show_color.Rows[e.Row.RowIndex][dtResult.Columns[i].ColumnName].ToString()) > 0)
                                t1.Attributes.Add("style", "color: Red");
                        }

                        t1.Width = 35;
                        //t1.Enabled = false;
                        TableCell tc = new TableCell();
                        tc.Controls.Add(t1);
                        e.Row.Cells.RemoveAt(column);
                        e.Row.Cells.AddAt(column, tc);
                        column += 2;
                    }
                }


                if (hid_Summary_ReqId.Value != "" && hid_chk_postback.Value != dtResult.Rows.Count.ToString())
                    hid_chk_postback.Value = (int.Parse(hid_chk_postback.Value == "" ? "0" : hid_chk_postback.Value) + 1).ToString();

            }

        }
        protected void txtAllocate_TextChanged(object sender, EventArgs e)
        {
            int rowIndex = ((GridViewRow)(((TextBox)sender).Parent.Parent)).RowIndex;
            String Grand_Total = dtResult.Rows[rowIndex]["Grand Total"].ToString();
            String OnHand = dtResult.Rows[rowIndex].IsNull("OnHand_Qty") ? "0" : dtResult.Rows[rowIndex]["OnHand_Qty"].ToString();
            TextBox allocate = ((TextBox)sender);

            HiddenField hidAllocate = (HiddenField)gvResult_routine.Rows[rowIndex].FindControl("hidAllocate");

            string allocate_2 = allocate.Text == "" ? "0" : allocate.Text;
            string Grand_Total_2 = Grand_Total == "" ? "0" : Grand_Total;

            if (decimal.Parse(OnHand) < decimal.Parse(allocate_2))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate มากกว่า ยอดคงคลัง');", true);
                allocate.Text = OnHand;
                allocate.Focus();
                hidAllocate.Value = OnHand;
                return;
            }
            if (decimal.Parse(Grand_Total_2) < decimal.Parse(allocate_2))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate มากกว่า รวมเบิก');", true);
                allocate.Text = Grand_Total;
                allocate.Focus();
                hidAllocate.Value = Grand_Total;
                return;
            }
            


            if (decimal.Parse(Grand_Total_2) == decimal.Parse(allocate_2))
            {
                int CountColumn = dtResult.Columns.Count - 2;

                for (int i = 5; i <= CountColumn; i++)
                {
                    Label lbl = (Label)gvResult_routine.Rows[rowIndex].FindControl(String.Format("L2{0}{1}", rowIndex, (i - 1)));

                    TextBox text2 = (TextBox)gvResult_routine.Rows[rowIndex].FindControl(String.Format("2{0}{1}", rowIndex, (i - 1)));

                    HiddenField H1 = (HiddenField)gvResult_routine.Rows[rowIndex].FindControl(String.Format("hid2{0}{1}", rowIndex, (i - 1)));

                    text2.Text = lbl.Text;
                    H1.Value = lbl.Text;
                }

                hidAllocate.Value = allocate_2;

            }
            
            else if ((allocate.Text.Trim() == "0" || allocate.Text.Trim() == "") && (allocate.Text.Trim() != hidAllocate.Value))
            {
                int CountColumn = dtResult.Columns.Count - 2;

                for (int i = 5; i <= CountColumn; i++)
                {
                    //Label lbl = (Label)gvResult_routine.Rows[rowIndex].FindControl(String.Format("L2{0}{1}", rowIndex, (i - 1)));

                    TextBox text2 = (TextBox)gvResult_routine.Rows[rowIndex].FindControl(String.Format("2{0}{1}", rowIndex, (i - 1)));

                    HiddenField H1 = (HiddenField)gvResult_routine.Rows[rowIndex].FindControl(String.Format("hid2{0}{1}", rowIndex, (i - 1)));

                    text2.Text = "";
                    H1.Value = "";
                }

                hidAllocate.Value = allocate_2;

            }

            else if ((allocate.Text.Trim() != "0" || allocate.Text.Trim() != "") && (allocate.Text.Trim() != hidAllocate.Value))
            {
                int CountColumn = dtResult.Columns.Count - 2;

                for (int i = 5; i <= CountColumn; i++)
                {
                    TextBox text2 = (TextBox)gvResult_routine.Rows[rowIndex].FindControl(String.Format("2{0}{1}", rowIndex, (i - 1)));

                    if (i == 5)
                        hidAllocate.Value = text2.Text;
                    else
                        hidAllocate.Value = (int.Parse(hidAllocate.Value) + int.Parse(text2.Text)).ToString();


                }


            }

            //hidAllocate.Value = allocate_2;

        }
        protected void txt_TextChanged(object sender, EventArgs e)
        {
            int rowIndex = ((GridViewRow)(((TextBox)sender).Parent.Parent)).RowIndex;
            String id = ((TextBox)sender).ID;
            Label lbl = (Label)((TextBox)sender).Parent.Parent.FindControl("L" + id);

            HiddenField H1 = (HiddenField)gvResult_routine.Rows[rowIndex].FindControl("hid" + id);

            TextBox allocate = (TextBox)gvResult_routine.Rows[rowIndex].FindControl(String.Format("3{0}", rowIndex));
            HiddenField hidAllocate = (HiddenField)gvResult_routine.Rows[rowIndex].FindControl("hidAllocate");


            String OnHand = dtResult.Rows[rowIndex]["OnHand_Qty"].ToString();

            string H_2 = H1.Value == "" ? "0" : H1.Value;
            string lbl_2 = lbl.Text == "" ? "0" : lbl.Text;
            string allocate_2 = hidAllocate.Value == "" ? "0" : hidAllocate.Value;
            string txt = ((TextBox)sender).Text == "" ? "0" : ((TextBox)sender).Text;

            //if (OnHand == "" || allocate.Text == "")
            //{
            //    ((TextBox)sender).Text = "";
            //    txt = "0";
            //}
            //else 

            if (OnHand == "" || (OnHand == "" && allocate.Text == ""))
            {
                if (((TextBox)sender).Text == "0")
                    ((TextBox)sender).Text = "0";
                else
                    ((TextBox)sender).Text = H1.Value;
            }
            else
            {

                if (int.Parse(lbl_2) < int.Parse(txt))
                {
                    ((TextBox)sender).Text = H1.Value;
                    txt = H_2;
                }

                if (int.Parse(txt) <= int.Parse(lbl_2))
                {

                    if (int.Parse(H_2) < int.Parse(txt))
                    {
                        allocate.Text = (int.Parse(allocate_2) + (int.Parse(txt) - int.Parse(H_2))).ToString();
                        hidAllocate.Value = allocate.Text;
                    }
                    else if (int.Parse(H_2) > int.Parse(txt))
                    {
                        allocate.Text = (int.Parse(allocate_2) - (int.Parse(H_2) - int.Parse(txt))).ToString();
                        hidAllocate.Value = allocate.Text;
                    }

                    if (allocate.Text.Trim() != "" && int.Parse(allocate_2) < 0)
                    {
                        allocate.Text = "0";
                        hidAllocate.Value = "0";
                    }
                }
            }

            //if (OnHand == "" || allocate.Text == "")
            //    H1.Value = "";
            //else 

            if (OnHand == "" || (OnHand == "" && allocate.Text == ""))
            {
                if (((TextBox)sender).Text == "0")
                    H1.Value = "0";
                else
                    H1.Value = H1.Value;
            }
            else
            {

                if (int.Parse(lbl_2) < int.Parse(txt))
                    H1.Value = H1.Value;
                else
                    H1.Value = ((TextBox)sender).Text;
            }

        }
        protected void btn_exit_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js2", @" 
            window.close();
            ", true);

        }

        protected void btn_Submit_Summary_Print_Click(object sender, EventArgs e)
        {

            List<SqlParameter> param2 = new List<SqlParameter>();
            
            param2.Add(new SqlParameter("@Stock_Id", hid_Stock_id.Value));
            param2.Add(new SqlParameter("@Status", "Status <> '0'"));
            param2.Add(new SqlParameter("@Summary_Date", hid_Summary_Date.Value));

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param2);

            if (dt.Rows.Count > 0)
            {
                btn_routine_search_Click(null, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('มีการทำสรุปการจ่ายของวันที่ " + (Convert.ToDateTime(hid_Summary_Date.Value, new System.Globalization.CultureInfo("en-US"))).ToString("dd/MM/yyyy", new System.Globalization.CultureInfo("th-TH")) + @" แล้ว');", true);
                return;
            }
            else
            {

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Summary_ReqId", 0));
                param[0].Direction = ParameterDirection.Output;

                param.Add(new SqlParameter("@Stock_Id", hid_Stock_id.Value));
                param.Add(new SqlParameter("@Summary_Date", hid_Summary_Date.Value));
                param.Add(new SqlParameter("@Number_Of_Req", txt_Number_Of_Req.Text.Trim()));
                param.Add(new SqlParameter("@Number_Of_Routine", txt_Number_Of_Routine.Text.Trim()));
                param.Add(new SqlParameter("@Number_Of_Stat", txt_Number_Of_Stat.Text.Trim()));
                param.Add(new SqlParameter("@Number_Of_NotRoutine", txt_Number_Of_NotRoutine.Text.Trim()));
                param.Add(new SqlParameter("@Number_Pending_InDue", txt_Number_Pending_InDue.Text.Trim()));
                param.Add(new SqlParameter("@Number_Pending_OutDue", txt_Number_Pending_OutDue.Text.Trim()));

                param.Add(new SqlParameter("@Status", "1"));
                param.Add(new SqlParameter("@Create_By", this.UserID));


                //List<SqlParameter> param2 = new List<SqlParameter>();

                //param2.Add(new SqlParameter("@Summary_ReqId", "ก"));
                //param2[0].Direction = ParameterDirection.Output;

                //param2.Add(new SqlParameter("@aaa", "ก"));

                bool result = RoutineStockDAO.InsertSummary(param, dtResultAll, dt_Id_Request, this.UserID);

                if (result)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมูลเรียบร้อย');", true);

//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js3", @" 
//                    window.close();
//                    window.opener.location = 'RoutineStock.aspx';
//                    //window.opener.location.reload(false);
//                    ", true);

                    ShowTextBox = false;
                    BindGrid();

                    fn_Printing(false);

                    
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
                    return;
                }
            }
        }

        protected void gvResult_routine_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }


        private void fn_Submit_Allocate()
        {
            List<Dictionary<decimal, decimal>> data = new List<Dictionary<decimal, decimal>>();
            bool IsValid = true;
            DataTable dtTemp1 = new DataTable();
            dtTemp1.Columns.Add("Summary_ReqItem_Id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Summary_ReqId", System.Type.GetType("System.Int32"));
            //dtTemp1.Columns.Add("Stock_Id", System.Type.GetType("System.Int32"));
            //dtTemp1.Columns.Add("OrgStruc_Id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Allocate_Qty", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Onhand_Qty", System.Type.GetType("System.Decimal"));


            int j = 0;

            foreach (GridViewRow row in gvResult_routine.Rows)
            {
                decimal Total = 0;
                int CountColumn = dtResult.Columns.Count - 2;

                for (int i = 5; i <= CountColumn; i++)
                {
                    DataRow dr = dtTemp1.NewRow();

                    string a = gvResult_routine.Rows[j].Cells[2].Text;
                    string header = dtResult.Columns[i].ToString();

                    DataRow[] drow = dtResultAll.Select("Inv_ItemCode = '" + a + "' and ID = '" + header + "'");

                    if (drow.Length > 0)
                    {
                        dr["Summary_ReqItem_Id"] = drow[0]["Summary_ReqItem_Id"].ToString();
                        dr["Summary_ReqId"] = drow[0]["Summary_ReqId"].ToString();
                        dr["OnHand_Qty"] = drow[0].IsNull("OnHand_Qty") ? "0" : drow[0]["OnHand_Qty"].ToString();
                    }


                    //Label lbl = (Label)row.FindControl(String.Format("2{0}{1}", row.RowIndex, (0)));

                    TextBox text2 = (TextBox)row.FindControl(String.Format("2{0}{1}", row.RowIndex, (i - 1)));
                    if (text2.Enabled)
                    {
                        Total += text2.Text.Equals("") ? 0 : decimal.Parse(text2.Text);

                        //dr["Summary_ReqItem_Id"] = this.dtResultAll.Rows[j]["Summary_ReqItem_Id"].ToString();
                        //dr["Summary_ReqId"] = this.dtResultAll.Rows[j]["Summary_ReqId"].ToString();
                        ////dr["Stock_Id"] = this.dtResultAll.Rows[i - 5]["Stock_Id"];
                        ////dr["OrgStruc_Id"] = this.dtResultAll.Rows[i - 5]["OrgStruc_Id"];
                        dr["Allocate_Qty"] = text2.Text.Equals("") ? 0 : int.Parse(text2.Text);
                        //dr["OnHand_Qty"] = this.dtResultAll.Rows[j].IsNull("OnHand_Qty") ? "0" : this.dtResultAll.Rows[j]["OnHand_Qty"].ToString();

                        dtTemp1.Rows.Add(dr);


                    }


                }

                j++;

                TextBox txtAllocate = (TextBox)row.FindControl(String.Format("3{0}", row.RowIndex));
                string Allocate = txtAllocate.Text == "" ? "0" : txtAllocate.Text;
                //if (txtAllocate.Text.Equals(""))
                //{
                //    txtAllocate.Focus();
                //    IsValid = false;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
                //    return;

                //}
                //else
                //{
                if (Total > decimal.Parse(Allocate) || Total < decimal.Parse(Allocate))
                {
                    txtAllocate.Focus();
                    IsValid = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
                    return;
                }
                //}


            }
            if (IsValid)
            {
                //TODO: insert data
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ถูกต้อง');", true);

                //Update data
                bool result = RoutineStockDAO.UpdateSummary(dtTemp1, hid_Summary_ReqId.Value, this.UserID);

                if (result)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมูลเรียบร้อย');", true);

                    hid_chk_postback.Value = "";

                    ShowTextBox = true;
                    BindData();
                    
                    BindData_routine_From_ID();
                    BindGrid_textbox();

                    fn_Printing(true);

                    
//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js4", @" 
//                    window.close();
//                    window.opener.location = 'RoutineStock.aspx';
//                    //window.opener.location.reload(false);
//                    ", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
            }
        }

        private void fn_Submit_Allocate_2()
        {
            List<Dictionary<decimal, decimal>> data = new List<Dictionary<decimal, decimal>>();
            bool IsValid = true;
            DataTable dtTemp1 = new DataTable();
            dtTemp1.Columns.Add("Summary_ReqItem_Id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Summary_ReqId", System.Type.GetType("System.Int32"));

            dtTemp1.Columns.Add("Request_id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("ID_TYPE", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Inv_ItemId", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Pack_ID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("ItemQTY", System.Type.GetType("System.Int32"));
            

            //dtTemp1.Columns.Add("Stock_Id", System.Type.GetType("System.Int32"));
            //dtTemp1.Columns.Add("OrgStruc_Id", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Allocate_Qty", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Onhand_Qty", System.Type.GetType("System.Decimal"));


            int j = 0;

            foreach (GridViewRow row in gvResult_routine.Rows)
            {
                decimal Total = 0;
                int CountColumn = dtResult.Columns.Count - 2;

                for (int i = 5; i <= CountColumn; i++)
                {
                    DataRow dr = dtTemp1.NewRow();

                    string a = gvResult_routine.Rows[j].Cells[2].Text;
                    string header = dtResult.Columns[i].ToString();

                    DataRow[] drow = dtResultAll.Select("Inv_ItemCode = '" + a + "' and ID = '" + header + "'");

                    if (drow.Length > 0)
                    {
                        dr["Summary_ReqItem_Id"] = drow[0]["Summary_ReqItem_Id"];
                        dr["Summary_ReqId"] = drow[0]["Summary_ReqId"];

                        dr["Request_id"] = drow[0]["Request_id"];
                        dr["ID_TYPE"] = drow[0]["ID_TYPE"];
                        dr["ID"] = drow[0]["ID"];

                        dr["Inv_ItemId"] = drow[0]["Inv_ItemId"];
                        dr["Pack_ID"] = drow[0]["Pack_ID"];
                        dr["ItemQTY"] = drow[0]["ItemQTY"];

                        dr["OnHand_Qty"] = drow[0].IsNull("OnHand_Qty") ? "0" : drow[0]["OnHand_Qty"].ToString();
                    }


                    //Label lbl = (Label)row.FindControl(String.Format("2{0}{1}", row.RowIndex, (0)));

                    TextBox text2 = (TextBox)row.FindControl(String.Format("2{0}{1}", row.RowIndex, (i - 1)));
                    if (text2.Enabled)
                    {
                        Total += text2.Text.Equals("") ? 0 : decimal.Parse(text2.Text);

                        //dr["Summary_ReqItem_Id"] = this.dtResultAll.Rows[j]["Summary_ReqItem_Id"].ToString();
                        //dr["Summary_ReqId"] = this.dtResultAll.Rows[j]["Summary_ReqId"].ToString();
                        ////dr["Stock_Id"] = this.dtResultAll.Rows[i - 5]["Stock_Id"];
                        ////dr["OrgStruc_Id"] = this.dtResultAll.Rows[i - 5]["OrgStruc_Id"];
                        dr["Allocate_Qty"] = text2.Text.Equals("") ? 0 : int.Parse(text2.Text);
                        //dr["OnHand_Qty"] = this.dtResultAll.Rows[j].IsNull("OnHand_Qty") ? "0" : this.dtResultAll.Rows[j]["OnHand_Qty"].ToString();

                        dtTemp1.Rows.Add(dr);


                    }


                }

                j++;

                TextBox txtAllocate = (TextBox)row.FindControl(String.Format("3{0}", row.RowIndex));
                string Allocate = txtAllocate.Text == "" ? "0" : txtAllocate.Text;
                //if (txtAllocate.Text.Equals(""))
                //{
                //    txtAllocate.Focus();
                //    IsValid = false;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
                //    return;

                //}
                //else
                //{
                if (Total > decimal.Parse(Allocate) || Total < decimal.Parse(Allocate))
                {
                    txtAllocate.Focus();
                    IsValid = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
                    return;
                }

                

                //}


            }


            DataTable dtTemp_insert_req = new DataTable();

            dtTemp_insert_req.Columns.Add("Summary_ReqId", System.Type.GetType("System.Int32"));
            dtTemp_insert_req.Columns.Add("Request_id", System.Type.GetType("System.Int32"));

            DataTable dtTemp_delete_req = new DataTable();

            dtTemp_delete_req.Columns.Add("Summary_ReqId", System.Type.GetType("System.Int32"));
            dtTemp_delete_req.Columns.Add("Request_id", System.Type.GetType("System.Int32"));


            DataTable dtTemp_delete_item = new DataTable();

            dtTemp_delete_item.Columns.Add("Summary_ReqItem_Id", System.Type.GetType("System.Int32"));


            foreach (DataRow drRow in dt_Modify.Rows)
            {
                if (drRow["type"].ToString() == "I")
                {
                    DataRow dr_insert_req = dtTemp_insert_req.NewRow();

                    dr_insert_req["Summary_ReqId"] = hid_Summary_ReqId.Value;
                    dr_insert_req["Request_id"] = drRow["Request_id"];

                    dtTemp_insert_req.Rows.Add(dr_insert_req);
                }
                else if (drRow["type"].ToString() == "D")
                {
                    DataRow dr_delete_req = dtTemp_delete_req.NewRow();

                    dr_delete_req["Summary_ReqId"] = hid_Summary_ReqId.Value;
                    dr_delete_req["Request_id"] = drRow["Request_id"];

                    dtTemp_delete_req.Rows.Add(dr_delete_req);
                }
                else if (drRow["type"].ToString() == "D-item")
                {
                    DataRow dr_delete_item = dtTemp_delete_item.NewRow();

                    dr_delete_item["Summary_ReqItem_Id"] = drRow["Summary_ReqItem_Id"];

                    dtTemp_delete_item.Rows.Add(dr_delete_item);
                }
            }

            if (IsValid)
            {
                // Update INV_SUMMARYREQ

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                param.Add(new SqlParameter("@Number_Of_Req", txt_Number_Of_Req.Text.Trim()));
                param.Add(new SqlParameter("@Number_Of_Routine", txt_Number_Of_Routine.Text.Trim()));
                param.Add(new SqlParameter("@Number_Of_Stat", txt_Number_Of_Stat.Text.Trim()));
                param.Add(new SqlParameter("@Number_Of_NotRoutine", txt_Number_Of_NotRoutine.Text.Trim()));
                param.Add(new SqlParameter("@Number_Pending_InDue", txt_Number_Pending_InDue.Text.Trim()));
                param.Add(new SqlParameter("@Number_Pending_OutDue", txt_Number_Pending_OutDue.Text.Trim()));

                param.Add(new SqlParameter("@Status", "2"));
                param.Add(new SqlParameter("@Update_By", this.UserID));

                //Update data
                bool result = RoutineStockDAO.UpdateSummary_Modify(param, dtTemp1, dtTemp_insert_req, dtTemp_delete_req, dtTemp_delete_item, hid_Summary_ReqId.Value, this.UserID);

                if (result)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมูลเรียบร้อย');", true);

                    hid_chk_postback.Value = "";

                    ShowTextBox = true;
                    BindData();

                    BindData_routine_From_ID();
                    BindGrid_textbox();

                    fn_Printing(true);



//                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js4", @" 
//                    window.close();
//                    window.opener.location = 'RoutineStock.aspx';
//                    //window.opener.location.reload(false);
//                    ", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
            }
        }

        protected void btn_Submit_Allocate_Print_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "N")
            {
                LinkButton2_ModalPopupExtender.Show();
            }
            else
            {
                if (Request.QueryString["type"] != null && Request.QueryString["type"].ToString() == "Y")
                {
                    fn_Submit_Allocate_2();
                }
                else
                {
                    fn_Submit_Allocate();
                }

//                List<Dictionary<decimal, decimal>> data = new List<Dictionary<decimal, decimal>>();
//                bool IsValid = true;
//                DataTable dtTemp1 = new DataTable();
//                dtTemp1.Columns.Add("Summary_ReqItem_Id", System.Type.GetType("System.Int32"));
//                dtTemp1.Columns.Add("Summary_ReqId", System.Type.GetType("System.Int32"));
//                //dtTemp1.Columns.Add("Stock_Id", System.Type.GetType("System.Int32"));
//                //dtTemp1.Columns.Add("OrgStruc_Id", System.Type.GetType("System.Int32"));
//                dtTemp1.Columns.Add("Allocate_Qty", System.Type.GetType("System.Int32"));
//                dtTemp1.Columns.Add("Onhand_Qty", System.Type.GetType("System.Decimal"));


//                int j = 0;

//                foreach (GridViewRow row in gvResult_routine.Rows)
//                {
//                    decimal Total = 0;
//                    int CountColumn = dtResult.Columns.Count - 2;

//                    for (int i = 5; i <= CountColumn; i++)
//                    {
//                        DataRow dr = dtTemp1.NewRow();

//                        string a = gvResult_routine.Rows[j].Cells[2].Text;
//                        string header = dtResult.Columns[i].ToString();

//                        DataRow[] drow = dtResultAll.Select("Inv_ItemCode = '" + a + "' and ID = '" + header + "'");

//                        if (drow.Length > 0)
//                        {
//                            dr["Summary_ReqItem_Id"] = drow[0]["Summary_ReqItem_Id"].ToString();
//                            dr["Summary_ReqId"] = drow[0]["Summary_ReqId"].ToString();
//                            dr["OnHand_Qty"] = drow[0].IsNull("OnHand_Qty") ? "0" : drow[0]["OnHand_Qty"].ToString();
//                        }


//                        //Label lbl = (Label)row.FindControl(String.Format("2{0}{1}", row.RowIndex, (0)));

//                        TextBox text2 = (TextBox)row.FindControl(String.Format("2{0}{1}", row.RowIndex, (i - 1)));
//                        if (text2.Enabled)
//                        {
//                            Total += text2.Text.Equals("") ? 0 : decimal.Parse(text2.Text);

//                            //dr["Summary_ReqItem_Id"] = this.dtResultAll.Rows[j]["Summary_ReqItem_Id"].ToString();
//                            //dr["Summary_ReqId"] = this.dtResultAll.Rows[j]["Summary_ReqId"].ToString();
//                            ////dr["Stock_Id"] = this.dtResultAll.Rows[i - 5]["Stock_Id"];
//                            ////dr["OrgStruc_Id"] = this.dtResultAll.Rows[i - 5]["OrgStruc_Id"];
//                            dr["Allocate_Qty"] = text2.Text.Equals("") ? 0 : int.Parse(text2.Text);
//                            //dr["OnHand_Qty"] = this.dtResultAll.Rows[j].IsNull("OnHand_Qty") ? "0" : this.dtResultAll.Rows[j]["OnHand_Qty"].ToString();

//                            dtTemp1.Rows.Add(dr);


//                        }


//                    }

//                    j++;

//                    TextBox txtAllocate = (TextBox)row.FindControl(String.Format("3{0}", row.RowIndex));
//                    string Allocate = txtAllocate.Text == "" ? "0" : txtAllocate.Text;
//                    //if (txtAllocate.Text.Equals(""))
//                    //{
//                    //    txtAllocate.Focus();
//                    //    IsValid = false;
//                    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
//                    //    return;

//                    //}
//                    //else
//                    //{
//                    if (Total > decimal.Parse(Allocate) || Total < decimal.Parse(Allocate))
//                    {
//                        txtAllocate.Focus();
//                        IsValid = false;
//                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
//                        return;
//                    }
//                    //}


//                }
//                if (IsValid)
//                {
//                    //TODO: insert data
//                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ถูกต้อง');", true);

//                    //Update data
//                    bool result = RoutineStockDAO.UpdateSummary(dtTemp1, hid_Summary_ReqId.Value, this.UserID);

//                    if (result)
//                    {
//                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมูลเรียบร้อย');", true);

//                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js4", @" 
//                    window.close();
//                    window.opener.location = 'RoutineStock.aspx';
//                    //window.opener.location.reload(false);
//                    ", true);
//                    }
//                    else
//                    {
//                        ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
//                    }
//                }
//                else
//                {
//                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('Allocate ไม่ถูกต้อง');", true);
//                }


            }

        }

        protected void btn_RePrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (hid_status.Value == "2" || hid_status.Value == "3")
                    fn_Printing(true);
                else
                    fn_Printing(false);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบพิมพ์ขัดข้อง');", true);
                return;
            }
        }

        protected void btn_popup_cancel2_Click(object sender, EventArgs e)
        {
            LinkButton2_ModalPopupExtender.Hide();
        }

        protected void btn_popup_submit2_Click(object sender, EventArgs e)
        {
            fn_Submit_Allocate();
        }

        #region LPA 21012014

        protected void btn_Export_Excel_Click(object sender, EventArgs e)
        {
            if (dtResult.Rows.Count > 0)
            {
                fn_Export_Excel();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบข้อมูล');", true);
                return;
            }
        }

        //private void fn_Export_Excel()
        //{
        //    String xHTMLStart = @"<HTML><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><BODY>";
        //    String xHTMLEnd = @"</BODY></HTML>";
        //    String xHeader = @"<TABLE Border=0><TR><TD colspan = '4' align = 'center'>" + "<h3>รายงานการเข้าพบลูกค้าบุคคล</h3>" + "</TD></TR></TABLE>";
        //    String xSaleHead = "";
        //    String xTableAll = "";
        //    String xFooter = "";

        //    xFooter = @"<TABLE Border=0><TR><TD colspan = '4' align = 'left'>" + "วันเวลาที่พิมพ์ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "</TD></TR></TABLE>";
        //    String xFile = xHTMLStart + xHeader + xTableAll + "<br/>" + xFooter + xHTMLEnd;

        //    Response.Clear();
        //    Response.AddHeader("Content-Disposition", "Attachment;Filename=VisitReport.xls");
        //    Response.Buffer = true;
        //    Response.Charset = "UTF-8";
        //    Response.ContentEncoding = System.Text.ASCIIEncoding.UTF8;
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.Write(xFile);

        //}

        private void fn_Export_Excel()
        {
            string StockName = "";
            if (hid_Stock_id.Value != "")
            {
                DataTable dt = new DataAccess.StockDAO().GetStock(hid_Stock_id.Value);
                if (dt.Rows.Count > 0)
                {
                    StockName = dt.Rows[0]["Stock_Name"].ToString();
                }
            }
            int allCntColumn = ((dtResult.Columns.Count-6)*2)+6;

            String xHTMLStart = @"<HTML><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /></head><BODY>";
            String xHTMLEnd = @"</BODY></HTML>";
            //String xHeader = @"<TABLE Border=0><TR><TD colspan = '4' align = 'center'>" + "<h3>รายงานการเข้าพบลูกค้าบุคคล</h3>" + "</TD></TR></TABLE>";
            String xHeader = @"<TABLE Border=0><TR><TD colspan = '" + allCntColumn + "' align = 'center'>" + "<h3>" + StockName + "</h3>" + "</TD></TR>" +
                              "<TR><TD colspan = '" + allCntColumn + "' align = 'center'>" + "<h3>ใบสรุปการเบิก-จ่าย วัน" + lbl_day.Text + "</h3></TD></TR></TABLE>";
            String xTableAll = "";
            String xFooter = "";

            String xTableStart = @"<TABLE Border=1>";
            String xTableEnd = @"</TABLE>";
            String xTHead = "<TR height='35'>";
            String xTHeadChild = "<TR height='35'>";
            StringBuilder xTableData = new StringBuilder(); /* add using System.Text;*/

            xTHead += @"<TH rowspan='2'>ลำดับที่</TH>";
            xTHead += @"<TH rowspan='2'>รายการ</TH>";
            xTHead += @"<TH rowspan='2'>รหัส</TH>";
            xTHead += @"<TH rowspan='2'>หน่วยนับ</TH>";
            //xTHead += @"<TH colspan='2'>Company1</TH>";
            //xTHead += @"<TH colspan='2'>Company2</TH>";

            /* ทำการสร้างส่วนหัวตารางตาม OrgStruct */

            int CountColumn = dtResult.Columns.Count - 2;
            for (int j = 5; j <= CountColumn; j++)
            {
                string columnName = "";

                DataRow[] drow = dtResultAll.Select("ID = '" + dtResult.Columns[j].ColumnName + "'");

                if (drow.Length > 0)
                {
                    columnName = drow[0]["Description"].ToString();
                }

                xTHead += @"<TH colspan='2'>" + columnName + "</TH>";
                xTHeadChild += @"<TH>เบิก</TH>";
                xTHeadChild += @"<TH>จ่าย</TH>";
            }

            xTHead += @"<TH rowspan='2'>รวมเบิก</TH>";
            xTHead += @"<TH rowspan='2'>ยอดคงคลัง</TH>";
            xTHead += @"<TH rowspan='2'>Allocate</TH>";
            xTHead += @"</TR>";

            //xTHeadChild += @"<TH>เบิก</TH>";
            //xTHeadChild += @"<TH>จ่าย</TH>";
            //xTHeadChild += @"<TH>เบิก</TH>";
            //xTHeadChild += @"<TH>จ่าย</TH>";
            xTHeadChild += @"</TR>";

            for (int i = 0; i < dtResult.Rows.Count; i++) // Loop ของแต่ละ Item
            {
                int OnhandQty = 0;
                int sumItemQty = 0;
                int sumAllocateQty = 0;

                xTableData.Append(@"<TR>");
                xTableData.Append(@"<TD valign = 'top' align = 'center'>" + (i+1) + @"</TD>");
                xTableData.Append(@"<TD valign = 'top' align = 'left'>" + dtResult.Rows[i]["Inv_ItemName"].ToString() + @"</TD>");
                xTableData.Append(@"<TD valign = 'top' align = 'left'>" + dtResult.Rows[i]["Inv_ItemCode"].ToString() + @"</TD>");
                xTableData.Append(@"<TD valign = 'top' align = 'left'>" + dtResult.Rows[i]["ItemDescription"].ToString() + @"</TD>");

                for (int j = 5; j <= CountColumn; j++) // loop นี้ทำการใส่ค่าของแต่ละหน่วยงาน
                {
                    int itemQty = 0;
                    int AllocateQty = 0;

                    //DataRow[] drow = dtResultAll.Select("ID = '" + dtResult.Columns[j].ColumnName + "'" + " AND Inv_ItemId = '" + dtResult.Rows[i]["Inv_ItemId"] + "'" + " AND ItemDescription = '" + dtResult.Rows[i]["ItemDescription"] + "'");
                    //if (drow.Length > 0)
                    //{
                    //    itemQty = Convert.ToInt32(drow[0]["ItemQTY"].ToString() == "" ? "0" : drow[0]["ItemQTY"].ToString());
                    //    AllocateQty = Convert.ToInt32(drow[0]["Allocate_Qty"].ToString() == "" ? "0" : drow[0]["Allocate_Qty"].ToString());
                    //    sumItemQty = sumItemQty + itemQty;
                    //    sumAllocateQty = sumAllocateQty + AllocateQty;

                    //    xTableData.Append(@"<TD valign = 'top' align = 'right'>" + itemQty.ToString("#,##0") + @"</TD>");
                    //    xTableData.Append(@"<TD valign = 'top' align = 'right'>" + AllocateQty.ToString("#,##0") + @"</TD>");
                    //}
                    //else // กรณีที่ไม่มีข้อมูลการเบิกจ่าย item นี้ ของ หน่วยงานนั้น
                    //{
                    //    xTableData.Append(@"<TD valign = 'top' align = 'right'>" + "" + @"</TD>");
                    //    xTableData.Append(@"<TD valign = 'top' align = 'right'>" + "" + @"</TD>"); 
                    //}

                    /* ดึงข้อมูลจำนวนที่เบิก ของหน่วยงานนั้น*/
                    DataRow[] drow = dtResult.Select("Inv_ItemId = '" + dtResult.Rows[i]["Inv_ItemId"] + "'" + " AND ItemDescription = '" + dtResult.Rows[i]["ItemDescription"] + "'");
                    //string a = drow[0][dtResult.Columns[j].ColumnName].ToString();
                    if (drow.Length > 0 && (drow[0][dtResult.Columns[j].ColumnName].ToString() != ""))
                    {
                        itemQty = Convert.ToInt32(drow[0][dtResult.Columns[j].ColumnName].ToString() == "" ? "0" : drow[0][dtResult.Columns[j].ColumnName].ToString());
                        sumItemQty = sumItemQty + itemQty;
                        xTableData.Append(@"<TD valign = 'top' align = 'right'>" + itemQty.ToString("#,##0") + @"</TD>");
                    }
                    else
                    {
                        xTableData.Append(@"<TD valign = 'top' align = 'right'>" + "" + @"</TD>");
                    }

                    /* ดึงข้อมูลจำนวนที่ Allocate ของหน่วยงานนั้น*/
                    DataRow[] drowAllcate = dtAllocate.Select("Inv_ItemId = '" + dtAllocate.Rows[i]["Inv_ItemId"] + "'" + " AND ItemDescription = '" + dtAllocate.Rows[i]["ItemDescription"] + "'");
                    if (drowAllcate.Length > 0 && (drowAllcate[0][dtResult.Columns[j].ColumnName].ToString() != ""))
                    {
                        AllocateQty = Convert.ToInt32(drowAllcate[0][dtResult.Columns[j].ColumnName].ToString() == "" ? "0" : drowAllcate[0][dtResult.Columns[j].ColumnName].ToString());
                        sumAllocateQty = sumAllocateQty + AllocateQty;
                        xTableData.Append(@"<TD valign = 'top' align = 'right'>" + AllocateQty.ToString("#,##0") + @"</TD>");
                    }
                    else
                    {
                        xTableData.Append(@"<TD valign = 'top' align = 'right'>" + "" + @"</TD>");
                    }
                } // End for (loop นี้ทำการใส่ค่าของแต่ละหน่วยงาน)


                OnhandQty = Convert.ToInt32(dtResult.Rows[i]["OnHand_Qty"].ToString() == "" ? "0" : dtResult.Rows[i]["OnHand_Qty"].ToString());

                xTableData.Append(@"<TD valign = 'top' align = 'right'>" + sumItemQty.ToString("#,##0") + @"</TD>");
                xTableData.Append(@"<TD valign = 'top' align = 'right'>" + OnhandQty.ToString("#,##0") + @"</TD>");
                xTableData.Append(@"<TD valign = 'top' align = 'right'>" + sumAllocateQty.ToString("#,##0") + @"</TD>"); 

                xTableData.Append(@"</TR align = 'left'>");
            }


            String xTable = xTableStart + xTHead + xTHeadChild + xTableData.ToString() + xTableEnd;
            xTableAll = xTableAll + "<br/>" + xTable;



            xFooter = @"<TABLE Border=0><TR><TD colspan = '" + allCntColumn/2 + "' align = 'left'>" + "วันเวลาที่พิมพ์ " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "</TD>" +
                        "<TD colspan = '" + allCntColumn / 2 + "' align = 'right'>" + "FM-15-02-02-Rev-03" + "</TD>" + "</TR></TABLE>";
            String xFile = xHTMLStart + xHeader + xTableAll + "<br/>" + xFooter + xHTMLEnd;

            Response.Clear();
            Response.AddHeader("Content-Disposition", "Attachment;Filename=RoutineStock.xls");
            Response.Buffer = true;
            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.ASCIIEncoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(xFile);
        }
        #endregion
    }
}
