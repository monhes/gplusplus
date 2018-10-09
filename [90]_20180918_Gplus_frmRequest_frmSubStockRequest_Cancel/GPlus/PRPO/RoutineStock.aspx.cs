using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.DataAccess;
using System.Data.SqlClient;
using System.Drawing;

namespace GPlus.PRPO
{
    public partial class RoutineStock : Pagebase
    {
        clsFuncMa Func = new clsFuncMa();
        RoutineStockDAO RoutineStockDAO = new RoutineStockDAO();
        OrgStructureDAO OrgStructureDAO = new OrgStructureDAO();
        RequestDAO RequestDAO = new RequestDAO();

        public int No1
        {
            get { return (PagingControl1.CurrentPageIndex * PagingControl1.PageSize) - PagingControl1.PageSize; }
        }

        public int No2
        {
            get { return (PagingControl2.CurrentPageIndex * PagingControl2.PageSize) - PagingControl2.PageSize; }
        }

        public DataTable dtRequest
        {
            get { return (ViewState["dtRequest"] == null) ? null : (DataTable)ViewState["dtRequest"]; }
            set { ViewState["dtRequest"] = value; }
        }

        public DataTable dtRequestItem
        {
            get { return (ViewState["dtRequestItem"] == null) ? null : (DataTable)ViewState["dtRequestItem"]; }
            set { ViewState["dtRequestItem"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.PageID = "407";
                Bind_ddl();
                //Request_Date_From.Text = "21/06/2556";


                if (Session["data"] != null)
                {
                    string[] data = Session["data"].ToString().Split('|');

                    ddl_stock.SelectedValue = data[0];

                    if (data[1] == "True")
                    {
                        rdb_summary_req.Checked = true;
                        rdb_inv_req.Checked = false;

                        ccFrom.Text = data[11];
                        ccTo.Text = data[12];

                    }
                    else if (data[2] == "True")
                    {
                        rdb_summary_req.Checked = false;
                        rdb_inv_req.Checked = true;

                        Request_Date_From.Text = data[9];
                        Request_Date_To.Text = data[10];

                    }

                    if (data[5] == "True")
                    {
                        rdb_request_status_1.Checked = true;
                        rdb_request_status_2.Checked = false;
                        rdb_request_status_3.Checked = false;
                    }
                    else if (data[6] == "True")
                    {
                        rdb_request_status_1.Checked = false;
                        rdb_request_status_2.Checked = true;
                        rdb_request_status_3.Checked = false;
                    }
                    else if (data[7] == "True")
                    {
                        rdb_request_status_1.Checked = false;
                        rdb_request_status_2.Checked = false;
                        rdb_request_status_3.Checked = true;
                    }

                    txt_Request_No.Text = data[8];


                    if (data[13] == "True")
                        cb_OrgStruc.Checked = true;

                    if (data[14] == "True")
                        cb_Stock.Checked = true;

                    if (rdb_summary_req.Checked == true && rdb_inv_req.Checked == false)
                    {
                        rdb_request_status_1.Checked = true;
                        rdb_request_status_3.Checked = false;
                        rdb_request_status_3.Enabled = false;

                        txt_Request_No.Enabled = false;

                        cb_OrgStruc.Checked = false;
                        cb_Stock.Checked = false;

                        cb_OrgStruc.Enabled = false;
                        cb_Stock.Enabled = false;

                        PagingControl2.PageSize = int.Parse(data[16]);
                        PagingControl2.CurrentPageIndex = int.Parse(data[4]);

                        Control_Panel_Allocate.Visible = true;
                        Control_Panel_withdrawal.Visible = false;
                        div_Detail_withdrawal.Visible = false;

                        BindData_summary();
                    }
                    else
                    {
                        rdb_request_status_3.Enabled = true;
                        txt_Request_No.Enabled = true;

                        cb_OrgStruc.Enabled = true;
                        cb_Stock.Enabled = true;


                        PagingControl1.PageSize = int.Parse(data[15]);
                        PagingControl1.CurrentPageIndex = int.Parse(data[3]);

                        Control_Panel_withdrawal.Visible = true;
                        Control_Panel_Allocate.Visible = false;
                        div_Detail_withdrawal.Visible = false;

                        BindData_withdrawal();
                    }


                    Session["data"] = null;

                }
            }

            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);
            PagingControl2.CurrentPageIndexChanged += new EventHandler(PagingControl2_CurrentPageIndexChanged);




            //PRControl1.SavePR += new EventHandler(PRControl1_SavePR);
        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData_withdrawal(hid_Summary_ReqId.Value);
        }

        void PagingControl2_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData_summary();
        }

        private void BindData_withdrawal()
        {
            //DataSet ds = new DataAccess.ItemDAO().GetItem("", "", "",
            //    "", "", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Stock_Id_From", ddl_stock.SelectedValue));

            string request_status = "";

            if (rdb_request_status_1.Checked == true)
            {
                request_status = "(Request_Status = '2' or Request_Status = '3' or Request_Status = '4')";
            }
            else if (rdb_request_status_2.Checked == true)
            {
                request_status = "Request_Status = '6'";
            }
            else if (rdb_request_status_3.Checked == true)
            {
                request_status = "Request_Status = '5'";
            }

            param.Add(new SqlParameter("@Request_stts", request_status));

            if (txt_Request_No.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Request_No", txt_Request_No.Text.Trim()));
            }

            if (Request_Date_From.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Request_Date_from", Func._fn_ConvertDateStored(Request_Date_From.Text.Trim())));
            }

            if (Request_Date_To.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Request_Date_to", Func._fn_ConvertDateStored(Request_Date_To.Text.Trim())));
            }

            if (ccFrom.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Pay_Date_from", Func._fn_ConvertDateStored(ccFrom.Text.Trim())));
            }

            if (ccTo.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Pay_Date_to", Func._fn_ConvertDateStored(ccTo.Text.Trim())));
            }

            if (cb_OrgStruc.Checked == true)
            {
                param.Add(new SqlParameter("@OrgStruc_stts", "Y"));
            }

            if (cb_Stock.Checked == true)
            {
                param.Add(new SqlParameter("@Stock_stts", "Y"));
            }


            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request", param);
            //dt.Columns[6].ColumnName

            //PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            PagingControl1.RecordCount = (int)dt.Rows.Count;
            int page = (PagingControl1.CurrentPageIndex - 1) * PagingControl1.PageSize;
            if (dt.Rows.Count > 0)
                gvResult_withdrawal.DataSource = dt.Select().Skip(page).Take(PagingControl1.PageSize).AsEnumerable().CopyToDataTable();
            else
                gvResult_withdrawal.DataSource = dt;
            gvResult_withdrawal.DataBind();
        }

        private void BindData_withdrawal(string Summary_ReqId)
        {
            //if (Summary_ReqId.Equals(string.Empty))
            //    Summary_ReqId = hid_Summary_ReqId.Value;

            if (string.IsNullOrEmpty(Summary_ReqId))
                BindData_withdrawal();
            else
            {

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Summary_ReqId", Summary_ReqId));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request", param);
                PagingControl1.RecordCount = (int)dt.Rows.Count;
                int page = (PagingControl1.CurrentPageIndex - 1) * PagingControl1.PageSize;

                if (dt.Rows.Count > 0)
                    gvResult_withdrawal.DataSource = dt.Select().Skip(page).Take(PagingControl1.PageSize).AsEnumerable().CopyToDataTable();
                else
                    gvResult_withdrawal.DataSource = dt;

                gvResult_withdrawal.DataBind();
            }
        }

        private void BindData_summary()
        {
            //DataSet ds = new DataAccess.ItemDAO().GetItem("", "", "",
            //    "", "", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);


            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Stock_Id", ddl_stock.SelectedValue));

            //string Status = "";

            //if (rdb_request_status_1.Checked == true)
            //{
            //    Status = "(Status = '1' or Status = '2')";
            //}
            //else if (rdb_request_status_2.Checked == true)
            //{
            //    Status = "Status = '3'";
            //}

            //param.Add(new SqlParameter("@Status", Status));

            if (ccFrom.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Summary_Date_from", Func._fn_ConvertDateStored(ccFrom.Text.Trim())));
            }

            if (ccTo.Text.Trim() != "")
            {
                param.Add(new SqlParameter("@Summary_Date_to", Func._fn_ConvertDateStored(ccTo.Text.Trim())));
            }

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_SummaryReq_Select", param);
            //PagingControl2.RecordCount = (int)ds.Tables[1].Rows[0][0];

            PagingControl2.RecordCount = (int)dt.Rows.Count;
            int page = (PagingControl2.CurrentPageIndex - 1) * PagingControl2.PageSize;

            if (dt.Rows.Count > 0)
                gvResult_Allocate.DataSource = dt.Select().Skip(page).Take(PagingControl2.PageSize).AsEnumerable().CopyToDataTable();
            else
                gvResult_Allocate.DataSource = dt;

            gvResult_Allocate.DataBind();

            //gvResult_Allocate.DataSource = dt;
            //gvResult_Allocate.DataBind();
        }


        private void Bind_ddl()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Account_ID", Session["UserID"].ToString()));

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Account_Select", param);

            ddl_stock.DataSource = dt;
            ddl_stock.DataBind();
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hid_Summary_ReqId.Value = null;

            Session["data"] = ddl_stock.SelectedValue + "|" + rdb_summary_req.Checked + "|" + rdb_inv_req.Checked + "|" + PagingControl1.CurrentPageIndex + "|" + PagingControl2.CurrentPageIndex + "|"
                            + rdb_request_status_1.Checked + "|" + rdb_request_status_2.Checked + "|" + rdb_request_status_3.Checked + "|" + txt_Request_No.Text + "|"
                            + Request_Date_From.Text + "|" + Request_Date_To.Text + "|" + ccFrom.Text + "|" + ccTo.Text + "|" + cb_OrgStruc.Checked + "|" + cb_Stock.Checked + "|"
                            + PagingControl1.PageSize + "|" + PagingControl2.PageSize;

            if (rdb_summary_req.Checked == true && rdb_inv_req.Checked == false)
            {

                if (ccFrom.Text.Trim() == "" && ccTo.Text.Trim() != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาระบุวันที่จ่าย');", true);
                    ccFrom.Focus();
                    return;
                }
                else if (ccFrom.Text.Trim() != "" && ccTo.Text.Trim() != "")
                {
                    try
                    {

                        DateTime dt_temp1 = Convert.ToDateTime(ccFrom.Text, new System.Globalization.CultureInfo("th-TH"));
                        DateTime dt_temp2 = Convert.ToDateTime(ccTo.Text, new System.Globalization.CultureInfo("th-TH"));

                        if (dt_temp1 > dt_temp2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบุวันที่จ่ายไม่ถูกต้อง');", true);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }


                PagingControl2.CurrentPageIndex = 1;
                PagingControl2.PageSize = 10;

                Control_Panel_Allocate.Visible = true;
                Control_Panel_withdrawal.Visible = false;
                div_Detail_withdrawal.Visible = false;

                BindData_summary();
            }
            else
            {
                if (Request_Date_From.Text.Trim() == "" && Request_Date_To.Text.Trim() != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาระบุวันที่เบิก');", true);
                    Request_Date_From.Focus();
                    return;
                }
                else if (Request_Date_From.Text.Trim() != "" && Request_Date_To.Text.Trim() != "")
                {
                    try
                    {

                        DateTime dt_temp1 = Convert.ToDateTime(Request_Date_From.Text, new System.Globalization.CultureInfo("th-TH"));
                        DateTime dt_temp2 = Convert.ToDateTime(Request_Date_To.Text, new System.Globalization.CultureInfo("th-TH"));

                        if (dt_temp1 > dt_temp2)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบุวันที่เบิกไม่ถูกต้อง');", true);
                            return;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (cb_OrgStruc.Checked == false && cb_Stock.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาเลือกใบเบิกที่ต้องการแสดง');", true);
                    cb_OrgStruc.Focus();
                    return;
                }



                PagingControl1.CurrentPageIndex = 1;
                PagingControl1.PageSize = 10;

                Control_Panel_withdrawal.Visible = true;
                Control_Panel_Allocate.Visible = false;
                div_Detail_withdrawal.Visible = false;

                BindData_withdrawal();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

            //Response.Write("<script type='text/javascript'>window.open('RoutineStock.aspx');</script>");

            Session["data"] = ddl_stock.SelectedValue + "|" + rdb_summary_req.Checked + "|" + rdb_inv_req.Checked + "|" + PagingControl1.CurrentPageIndex + "|" + PagingControl2.CurrentPageIndex + "|"
                            + rdb_request_status_1.Checked + "|" + rdb_request_status_2.Checked + "|" + rdb_request_status_3.Checked + "|" + txt_Request_No.Text + "|"
                            + Request_Date_From.Text + "|" + Request_Date_To.Text + "|" + ccFrom.Text + "|" + ccTo.Text + "|" + cb_OrgStruc.Checked + "|" + cb_Stock.Checked + "|"
                            + PagingControl1.PageSize + "|" + PagingControl2.PageSize;


            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", @" 
            window.open('Routine.aspx?st_id=" + ddl_stock.SelectedValue + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
            ", true);

            //Control_Panel.Visible = false;
            //Routine_Panel.Visible = true;

            //div_search.Visible = true;
            //div_data.Visible = false;

            //lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", DateTime.Today);

            //Routine_Date.Text = String.Format("{0:dd/MM/yyyy}", DateTime.Today);
            //lbl_Routine_day.Text = String.Format("{0:dddd}", DateTime.Today);

        }



        private void fn_Clear_Detail()
        {
            txt_req_No.Text = "";
            txt_req_date.Text = "";

            rdb_req_from1.Checked = false;
            rdb_req_from2.Checked = false;

            rdb_req_type_1.Checked = false;
            rdb_req_type_2.Checked = false;
            rdb_req_type_3.Checked = false;

            txt_Div.Text = "";
            txt_Dep.Text = "";
            txt_stock.Text = "";
            txt_req_by.Text = "";
            txt_Pay_dttm.Text = "";
            txt_Pay_by.Text = "";
            txt_Pay_address.Text = "";

            txt_barcode.Text = "";
            txt_No.Text = "";
            txt_item_name.Text = "";
            txt_pack.Text = "";
            txt_lot_no.Text = "";
            txt_qty.Text = "";
        }

        protected void gvResult_withdrawal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{
            if (e.CommandName == "Detail")
            {
                string Request_Id = e.CommandArgument.ToString();
                div_Detail_withdrawal.Visible = true;

                btn_Submit.Focus();

                fn_Clear_Detail();

                //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                //int RowIndex = gvr.RowIndex;

                //gvResult_withdrawal.Rows[RowIndex].Attributes.Add("background-color", "#EC467E");

                GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;
                //row.BackColor = Color.Red;

                hid_rowIndex.Value = row.RowIndex.ToString();

                for (int i = 0; i < gvResult_withdrawal.Rows.Count; i++)
                {
                    if (i == row.RowIndex)
                    {
                        row.BackColor = Color.FromArgb(58, 143, 227);
                    }
                    else
                        //row.BackColor = Color.Empty;
                        gvResult_withdrawal.Rows[i].BackColor = Color.Empty;

                }

                //LinkButton lnk = e.CommandSource as LinkButton;
                //if (lnk != null)
                //{
                //    DataControlFieldCell cell = lnk.Parent as DataControlFieldCell;
                //    cell.BackColor = Color.Yellow;

                //}



                hid_Request_Id.Value = Request_Id;

                Bind_Detail();

                //List<SqlParameter> param = new List<SqlParameter>();

                //param.Add(new SqlParameter("@Request_Id", Request_Id.Trim()));

                //dtRequest = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request", param);

                //txt_req_No.Text = dtRequest.Rows[0]["Request_No"].ToString().Trim();
                //txt_req_date.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtRequest.Rows[0]["Request_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));

                //if (!dtRequest.Rows[0].IsNull("OrgStruc_Id_Req"))
                //{
                //    rdb_req_from1.Checked = true;
                //    rdb_req_from2.Checked = false;

                //    txt_Div.Text = dtRequest.Rows[0]["Div"].ToString();
                //    txt_Dep.Text = dtRequest.Rows[0]["Dep"].ToString();
                //}
                //else
                //{
                //    rdb_req_from2.Checked = true;
                //    rdb_req_from1.Checked = false;

                //    txt_stock.Text = dtRequest.Rows[0]["Stock_Name"].ToString();
                //}

                //switch (dtRequest.Rows[0]["Request_Type"].ToString())
                //{

                //    case "0":

                //        rdb_req_type_1.Checked = true;

                //        break;

                //    case "1":

                //        rdb_req_type_2.Checked = true;

                //        break;

                //    case "2":

                //        rdb_req_type_3.Checked = true;

                //        break;

                //}

                //txt_req_by.Text = dtRequest.Rows[0]["Request_By"].ToString();
                //if (!dtRequest.Rows[0].IsNull("Pay_Date"))
                //{
                //    txt_Pay_dttm.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtRequest.Rows[0]["Pay_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));
                //}
                //txt_Pay_by.Text = dtRequest.Rows[0]["Pay_By"].ToString();


                //if (dtRequest.Rows[0]["Request_Status"].ToString() == "5" || dtRequest.Rows[0]["Request_Status"].ToString() == "6")
                //{
                //    btn_Cancel_Summary.Enabled = true;
                //}
                //else
                //{
                //    //btn_Cancel_Summary.Enabled = false;
                //}

                ////BindDetail_withdrawal();
                ////Bind Grid


                //List<SqlParameter> param2 = new List<SqlParameter>();

                //param2.Add(new SqlParameter("@Request_Id", Request_Id));
                //param2.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

                //dtRequestItem = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_RequestItem", param2);

                //gvDetail_withdrawal.DataSource = dtRequestItem;
                //gvDetail_withdrawal.DataBind();

                //decimal total_all = 0;
                //decimal total = 0;
                //foreach (DataRow drRow in dtRequestItem.Rows)
                //{
                //    total_all += Convert.ToInt32(drRow["Request_Id"]);

                //    decimal unit_price = decimal.Parse(drRow["Avg_Cost"].ToString());
                //    decimal Allocate = 0;
                //    decimal.TryParse(drRow["Allocate"].ToString(), out Allocate);
                //    total += (unit_price * Allocate);
                //}

                //lbl_total_all.Text = total.ToString("#,##0.00");
            }

            else if (e.CommandName == "Print")
            {
                string Request_Id = e.CommandArgument.ToString();

                //ทำการตรวจสอบใน Table Inv_StockPay ว่ามี count > 1 หรือไม่  ที่ Request_id = ? และ pay_status = 1

                DataTable dt = new DataAccess.RequestDAO().CheckCntPay(Request_Id);
                if (Convert.ToInt32(dt.Rows[0]["Cnt_Pay"].ToString()) <= 1) // ถ้าค่าน้อยกว่าหรือเท่ากับ 1 ให้ทำการ Print ใบเบิกอัตโนมัติ
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RequestReportPopup", "open_popup('../Request/RequestReport.aspx?id=" + Request_Id + "', 850, 450, 'pop', 'yes', 'yes', 'yes');", true);
                }
                else // ถ้าค่ามากกว่า 1 ให้ทำการ Pop Window ให้เลือกครั้งที่จ่าย
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", @" 
                            window.open('../Request/RequestReportPay.aspx?req=" + Request_Id + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');", true);
                }


            }

            //                else if (e.CommandName == "Print")
            //                {
            //                    List<SqlParameter> param2 = new List<SqlParameter>();

            //                    param2.Add(new SqlParameter("@Request_Id", e.CommandArgument.ToString()));
            //                    param2.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

            //                    dtRequestItem = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_RequestItem", param2);
            //                    if (dtRequestItem != null)
            //                    {

            //                        DataSetReprot_ReqItem _DataSetReprot_ReqItem = new DataSetReprot_ReqItem();
            //                        foreach (DataRow r in dtRequestItem.Rows)
            //                        {
            //                            _DataSetReprot_ReqItem.RequestItem.ImportRow(r);
            //                        }

            //                        List<SqlParameter> param = new List<SqlParameter>();
            //                        param.Add(new SqlParameter("@Request_Id", e.CommandArgument.ToString()));
            //                        dtRequest = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request", param);
            //                        _DataSetReprot_ReqItem.DataTable1.Rows.Add();
            //                        _DataSetReprot_ReqItem.DataTable1.Rows[0]["Date"] = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtRequest.Rows[0]["Request_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));
            //                        _DataSetReprot_ReqItem.DataTable1.Rows[0]["Request_By"] = dtRequest.Rows[0]["Request_By"].ToString();
            //                        _DataSetReprot_ReqItem.DataTable1.Rows[0]["Dep"] = dtRequest.Rows[0]["Dep"].ToString();

            //                        this.Session["dtReportRequestItem"] = _DataSetReprot_ReqItem;
            //                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", @" 
            //                        window.open('TempRPT_ReqItem.aspx','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
            //                        ", true);
            //                    }
            //                }

            //}
            //catch (Exception ex) {
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบพิมพ์ขัดข้อง');", true);
            //    return;
            //}
        }

        private void Bind_Detail()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Request_Id", hid_Request_Id.Value.Trim()));

            dtRequest = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request", param);

            txt_req_No.Text = dtRequest.Rows[0]["Request_No"].ToString().Trim();
            txt_req_date.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtRequest.Rows[0]["Request_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));

            if (!dtRequest.Rows[0].IsNull("OrgStruc_Id_Req"))
            {
                rdb_req_from1.Checked = true;
                rdb_req_from2.Checked = false;

                txt_Div.Text = dtRequest.Rows[0]["Div"].ToString();
                txt_Dep.Text = dtRequest.Rows[0]["Dep"].ToString();
            }
            else
            {
                rdb_req_from2.Checked = true;
                rdb_req_from1.Checked = false;

                txt_stock.Text = dtRequest.Rows[0]["Stock_Name"].ToString();
            }

            switch (dtRequest.Rows[0]["Request_Type"].ToString())
            {

                case "0":

                    rdb_req_type_1.Checked = true;

                    break;

                case "1":

                    rdb_req_type_2.Checked = true;

                    break;

                case "2":

                    rdb_req_type_3.Checked = true;

                    break;

            }

            //Edit by khak
            //txt_req_by.Text = dtRequest.Rows[0]["Request_By"].ToString();
            if (!string.IsNullOrEmpty(dtRequest.Rows[0]["Request_By"].ToString()))
            {
                txt_req_by.Text = dtRequest.Rows[0]["Request_By"].ToString();
            }
            else
            {
                txt_req_by.Text = dtRequest.Rows[0]["Request_Name"].ToString();
            }

            if (!dtRequest.Rows[0].IsNull("Pay_Date"))
            {
                txt_Pay_dttm.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtRequest.Rows[0]["Pay_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));
            }
            txt_Pay_address.Text = dtRequest.Rows[0]["delivery_location"].ToString();

            if (!dtRequest.Rows[0].IsNull("Pay_Date"))
            {
                txt_Pay_dttm.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dtRequest.Rows[0]["Pay_Date"].ToString(), new System.Globalization.CultureInfo("th-TH")));
            }
            txt_Pay_by.Text = dtRequest.Rows[0]["Pay_By"].ToString();


            if (dtRequest.Rows[0]["Request_Status"].ToString() == "5" || dtRequest.Rows[0]["Request_Status"].ToString() == "6")
            {
                btn_Cancel_Summary.Enabled = true;
            }
            else
            {
                //btn_Cancel_Summary.Enabled = false;
            }

            if (dtRequest.Rows[0]["Request_Status"].ToString() == "6")
            {
                btn_Submit.Enabled = false;
            }
            else
            {
                btn_Submit.Enabled = true;
            }

            //BindDetail_withdrawal();
            //Bind Grid

            hid_Request_Status.Value = dtRequest.Rows[0]["Request_Status"].ToString();

            List<SqlParameter> param2 = new List<SqlParameter>();

            param2.Add(new SqlParameter("@Request_Id", hid_Request_Id.Value));
            param2.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));

            dtRequestItem = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_RequestItem", param2);

            gvDetail_withdrawal.DataSource = dtRequestItem;
            gvDetail_withdrawal.DataBind();

            decimal total_all = 0;
            decimal total = 0;
            foreach (DataRow drRow in dtRequestItem.Rows)
            {
                //edit by Khak 
                int tmp_Request_Id = 0;
                int.TryParse(drRow["Request_Id"].ToString(), out tmp_Request_Id);
                total_all += tmp_Request_Id;
                //total_all += Convert.ToInt32(drRow["Request_Id"]);

                //edit by Khak 
                //decimal unit_price = decimal.Parse(drRow["Avg_Cost"].ToString());
                decimal unit_price = 0;
                decimal.TryParse(drRow["Avg_Cost"].ToString(), out unit_price);


                decimal Allocate = 0;
                decimal.TryParse(drRow["Allocate"].ToString(), out Allocate);

                //edit by Khak 
                //if (int.Parse(drRow["Order_Quantity"].ToString()) == int.Parse(drRow["Pay_Qty"].ToString()))
                int tmp_Order_Quantity = 0;
                int.TryParse(drRow["Order_Quantity"].ToString(), out tmp_Order_Quantity);
                int tmp_Pay_Qty = 0;
                int.TryParse(drRow["Pay_Qty"].ToString(), out tmp_Pay_Qty);

                if (tmp_Order_Quantity == tmp_Pay_Qty)
                    total += 0;
                else
                    total += (unit_price * Allocate);
                //end edit by Khak 
            }

            lbl_total_all.Text = total.ToString("#,##0.00");
        }

        private void BindDetail_withdrawal()
        {
            //DataSet ds = new DataAccess.ItemDAO().GetItem("", "", "",
            //    "", "", PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);


            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Request");

            //PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

            gvDetail_withdrawal.DataSource = dt;
            gvDetail_withdrawal.DataBind();
        }

        protected void gvDetail_withdrawal_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                GridViewRow headerow = new GridViewRow(0, 0, DataControlRowType.Header,
                                                          DataControlRowState.Insert);
                e.Row.Cells.Clear();


                TableCell headercell1 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 40,
                    Text = "ลำดับ<br>สินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell1);


                TableCell headercell2 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 77,
                    Text = "รหัส<br>สินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell2);


                TableCell headercell3 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 120,
                    Text = "ชื่อสินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell3);

                TableCell headercell4 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 50,
                    Text = "หน่วย",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell4);

                TableCell headercell5 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 20,
                    Width = 60,
                    Text = "ราคา<br>ต่อหน่วย",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell5);

                TableCell headercell6 = new TableCell()
                {
                    ColumnSpan = 5,
                    RowSpan = 1,
                    Height = 20,
                    Text = "จำนวน",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell6);

                GridViewRow headerow2 = new GridViewRow(0, 0, DataControlRowType.Header,
                                                          DataControlRowState.Insert);

                TableCell headercell7 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "เบิก",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell7);

                TableCell headercell8 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "จ่าย<br>สะสม",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell8);

                TableCell headercell9 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "คงค้าง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell9);

                TableCell headercell10 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "จำนวนจ่าย<br>ครั้งนี้",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell10);

                TableCell headercell11 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "ในคลัง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell11);

                TableCell headercell12 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 40,
                    Text = "ค้างจ่าย",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell12);

                TableCell headercell13 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 40,
                    Text = "ปิด<br>การจ่าย",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell13);


                TableCell headercell14 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 75,
                    Text = "รวมเงิน",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell14);

                TableCell headercell15 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Width = 50,
                    Text = "สถานะ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell15);

                #region LPA 17012014

                TableCell headercell16 = new TableCell()
                {
                    ColumnSpan = 2,
                    RowSpan = 1,
                    Height = 20,
                    Text = "หมายเหตุ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell16);

                TableCell headercell17 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "คลัง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell17);

                TableCell headercell18 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "หน่วยงาน",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell18);
                #endregion

                gvDetail_withdrawal.Controls[0].Controls.AddAt(0, headerow);
                gvDetail_withdrawal.Controls[0].Controls.AddAt(1, headerow2);


            }

        }



        protected void gvResult_Allocate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Req")
                {
                    string Summary_ReqId = e.CommandArgument.ToString();
                    hid_Summary_ReqId.Value = e.CommandArgument.ToString();

                    Control_Panel_withdrawal.Visible = true;
                    div_Detail_withdrawal.Visible = false;

                    PagingControl1.CurrentPageIndex = 1;
                    PagingControl1.PageSize = 10;
                    //BindData_withdrawal();
                    BindData_withdrawal(Summary_ReqId);

                }
                else if (e.CommandName == "Sum")
                {

                    Session["data"] = ddl_stock.SelectedValue + "|" + rdb_summary_req.Checked + "|" + rdb_inv_req.Checked + "|" + PagingControl1.CurrentPageIndex + "|" + PagingControl2.CurrentPageIndex + "|"
                            + rdb_request_status_1.Checked + "|" + rdb_request_status_2.Checked + "|" + rdb_request_status_3.Checked + "|" + txt_Request_No.Text + "|"
                            + Request_Date_From.Text + "|" + Request_Date_To.Text + "|" + ccFrom.Text + "|" + ccTo.Text + "|" + cb_OrgStruc.Checked + "|" + cb_Stock.Checked + "|"
                            + PagingControl1.PageSize + "|" + PagingControl2.PageSize;

                    string Summary_ReqId = e.CommandArgument.ToString();

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js2", @" 
                    window.open('Routine.aspx?id=" + Summary_ReqId + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
                    ", true);

                    //Control_Panel_withdrawal.Visible = false;
                    //div_search.Visible = false;
                    //Routine_Panel.Visible = true;
                    //div_data.Visible = true;

                    //lbl_day.Text = String.Format("{0:dddd dd/MM/yyyy}", DateTime.Today);

                    //BindData_routine();
                }

            }
            catch (Exception ex) { }
        }

        protected void rdb_request_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb_summary_req.Checked == true)
            {
                rdb_request_status_1.Checked = true;
                rdb_request_status_3.Checked = false;

                rdb_request_status_1.Enabled = false;
                rdb_request_status_2.Enabled = false;
                rdb_request_status_3.Enabled = false;

                txt_Request_No.Text = "";
                txt_Request_No.Enabled = false;

                Request_Date_From.Text = "";
                Request_Date_To.Text = "";

                cb_OrgStruc.Checked = false;
                cb_Stock.Checked = false;

                cb_OrgStruc.Enabled = false;
                cb_Stock.Enabled = false;

            }
            else if (rdb_inv_req.Checked == true)
            {
                rdb_request_status_1.Enabled = true;
                rdb_request_status_2.Enabled = true;
                rdb_request_status_3.Enabled = true;
                txt_Request_No.Enabled = true;

                cb_OrgStruc.Enabled = true;
                cb_Stock.Enabled = true;
                cb_OrgStruc.Checked = true;
                cb_Stock.Checked = true;

                ccFrom.Text = "";
                ccTo.Text = "";
            }
        }


        public static bool fn_chk_status(object Order_Quantity, object Pay_Qty, object Allocate, string cb, string type)
        {
            int OrderQty = Convert.ToInt32(Order_Quantity.ToString());
            int PayQty = Convert.ToInt32(Pay_Qty.ToString());
            int AllocateQty = 0;

            if (Allocate != System.DBNull.Value)
            {
                AllocateQty = Convert.ToInt32(Allocate.ToString());
            }

            if (Allocate == System.DBNull.Value)
            {
                if (type == "Chk")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            else if ((OrderQty - (PayQty + AllocateQty)) == 0) //จ่ายครบ
            {
                //cb_Close.Checked = true;
                if (cb == "cb_Close" && type == "Chk")
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //else if (cb == "cb_Close" && type == "Enabled")
                //{
                //    return false;
                //}
                //else if (cb == "cb_Remain" && type == "Chk")
                //{
                //    return false;
                //}
                //else if (cb == "cb_Remain" && type == "Enabled")
                //{
                //    return false;
                //}


            }
            else
            {
                //if ((PayQty + AllocateQty) > 0 && (PayQty + AllocateQty) < OrderQty)
                //{
                //ให้เลือกว่าจะ ปิดการจ่ายหรือค้างจ่าย
                if (cb == "cb_Close" && type == "Chk")
                {
                    return true;
                }
                else if ((cb == "cb_Close" && type == "Enabled") || (cb == "cb_Remain" && type == "Enabled"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //}


            }

        }
        public static int fn_chk_status_int(object Order_Quantity, object Pay_Qty, object Allocate)
        {
            int OrderQty = Convert.ToInt32(Order_Quantity.ToString());
            int PayQty = Convert.ToInt32(Pay_Qty.ToString());
            int AllocateQty = 0;

            if (Allocate != System.DBNull.Value)
            {
                AllocateQty = Convert.ToInt32(Allocate.ToString());
            }

            if (Allocate == System.DBNull.Value) //ยังไม่ Allocate
            {
                return 99;

            }
            else if ((OrderQty - (PayQty + AllocateQty)) == 0) //จ่ายครบ
            {
                //cb_Close.Checked = true;
                return 3;
            }
            else
            {
                //if ((PayQty + AllocateQty) > 0 && (PayQty + AllocateQty) < OrderQty) // Default ค้างจ่าย
                //{
                //ให้เลือกว่าจะ ปิดการจ่ายหรือค้างจ่าย
                return 2;
                //}
            }
        }

        public static string fn_chk_status_txt(object Order_Quantity, object Pay_Qty, object Allocate)
        {
            int OrderQty = Convert.ToInt32(Order_Quantity.ToString());
            int PayQty = Convert.ToInt32(Pay_Qty.ToString());
            int AllocateQty = 0;

            if (Allocate != System.DBNull.Value)
            {
                AllocateQty = Convert.ToInt32(Allocate.ToString());
            }

            if (Allocate == System.DBNull.Value) //ยังไม่ Allocate
            {
                return "";

            }
            else if ((OrderQty - (PayQty + AllocateQty)) == 0) //จ่ายครบ
            {
                //cb_Close.Checked = true;
                return "จ่ายครบ";
            }
            else
            {
                //if ((PayQty + AllocateQty) > 0 && (PayQty + AllocateQty) < OrderQty)  // Default ค้างจ่าย
                //{
                //ให้เลือกว่าจะ ปิดการจ่ายหรือค้างจ่าย
                return "ปิดการจ่าย";
                //}


            }

        }


        public static string fn_summary_cost(object Allocate, object Avg_Cost)
        {


            decimal Cost = 0;
            decimal AllocateQty = 0;

            if (Avg_Cost != System.DBNull.Value)
            {
                Cost = Convert.ToDecimal(Avg_Cost.ToString());
            }


            if (Allocate != System.DBNull.Value)
            {
                AllocateQty = Convert.ToDecimal(Allocate.ToString());
            }

            decimal sum_cost = (Cost * AllocateQty);


            return sum_cost.ToString("#,##0.0000");


        }

        protected void txt_qty_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)(((TextBox)sender).Parent.Parent));
            TextBox txt_qty_row = (TextBox)sender;
            Label lbl_unit_price = (Label)row.FindControl("lbl_unit_price");
            Label lbl_summary = (Label)row.FindControl("lbl_summary");
            Label lbl_Remain_Qty = (Label)row.FindControl("lbl_Remain_Qty");
            Label lbl_store_Qty = (Label)row.FindControl("lbl_store_Qty");
            Label lbl_Order_Qty = (Label)row.FindControl("lbl_Order_Qty");
            Label lbl_Pay_Qty = (Label)row.FindControl("lbl_Pay_Qty");


            Label lbl_status = (Label)row.FindControl("lbl_status");
            HiddenField hid_status = (HiddenField)row.FindControl("hid_status");

            HiddenField hid_Lot = (HiddenField)row.FindControl("hid_Lot");
            HiddenField hid_Amount = (HiddenField)row.FindControl("hid_Amount");

            //HiddenField hid_Inv_ItemID = (HiddenField)row.FindControl("hid_Inv_ItemID");
            //HiddenField hid_Pack_ID = (HiddenField)row.FindControl("hid_Pack_ID");


            CheckBox cb_Remain = (CheckBox)row.FindControl("cb_Remain");
            CheckBox cb_Close = (CheckBox)row.FindControl("cb_Close");

            Label lbl_summary_row;
            decimal total = 0;

            int txt_qty_int = int.Parse(txt_qty_row.Text == "" ? "0" : txt_qty_row.Text);

            int lbl_Order_Qty_int = 0;
            int.TryParse(lbl_Order_Qty.Text, out lbl_Order_Qty_int);

            int lbl_Pay_Qty_int = 0;
            int.TryParse(lbl_Pay_Qty.Text, out lbl_Pay_Qty_int);

            if (!txt_qty_row.Text.Equals(""))
            {

                decimal lbl_unit_price_decimal = decimal.Parse(lbl_unit_price.Text);
                int lbl_Remain_Qty_int = 0, lbl_store_Qty_int = 0;
                int.TryParse(lbl_Remain_Qty.Text, out lbl_Remain_Qty_int);
                int.TryParse(lbl_store_Qty.Text, out lbl_store_Qty_int);

                if ((lbl_Pay_Qty_int == 0) && (txt_qty_int > lbl_Order_Qty_int))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนจ่ายครั้งนี้ มากกว่า เบิก');", true);
                    txt_qty_row.Text = "";
                    hid_Lot.Value = "";
                    hid_Amount.Value = "";
                    lbl_summary.Text = "0.0000";
                    lbl_status.Text = "";
                    cb_Remain.Enabled = true;
                    cb_Remain.Checked = false;
                    cb_Close.Enabled = true;
                    cb_Close.Checked = false;
                    //lbl_total_all.Text = "0.00";

                    foreach (GridViewRow item in gvDetail_withdrawal.Rows)
                    {
                        lbl_summary_row = (Label)item.FindControl("lbl_summary");
                        total += decimal.Parse(lbl_summary_row.Text);
                    }

                    lbl_total_all.Text = total.ToString("#,##0.0000");

                    txt_qty_row.Focus();
                    return;
                }

                if (txt_qty_int > (lbl_Order_Qty_int - lbl_Pay_Qty_int))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนจ่ายครั้งนี้ มากกว่า ค้างจ่าย');", true);

                    if ((lbl_Order_Qty_int - lbl_Pay_Qty_int) <= lbl_store_Qty_int)
                    {
                        txt_qty_row.Text = (lbl_Order_Qty_int - lbl_Pay_Qty_int).ToString();

                        lbl_Remain_Qty.Text = (lbl_Order_Qty_int - (lbl_Pay_Qty_int + (lbl_Order_Qty_int - lbl_Pay_Qty_int))).ToString();

                        cb_Remain.Enabled = false;
                        cb_Remain.Checked = false;
                        cb_Close.Enabled = false;
                        cb_Close.Checked = true;


                        lbl_summary.Text = ((lbl_Order_Qty_int - lbl_Pay_Qty_int) * lbl_unit_price_decimal).ToString("#,##0.00");
                        lbl_status.Text = "จ่ายครบ";

                        foreach (GridViewRow item in gvDetail_withdrawal.Rows)
                        {
                            lbl_summary_row = (Label)item.FindControl("lbl_summary");
                            total += decimal.Parse(lbl_summary_row.Text);
                        }

                        lbl_total_all.Text = total.ToString("#,##0.0000");

                        txt_qty_row.Focus();
                        return;
                    }
                    else
                    {
                        txt_qty_row.Text = "";
                        hid_Lot.Value = "";
                        hid_Amount.Value = "";
                        lbl_summary.Text = "0.0000";
                        lbl_status.Text = "";
                        cb_Remain.Enabled = true;
                        cb_Remain.Checked = false;
                        cb_Close.Enabled = true;
                        cb_Close.Checked = false;
                        //lbl_total_all.Text = "0.00";

                        foreach (GridViewRow item in gvDetail_withdrawal.Rows)
                        {
                            lbl_summary_row = (Label)item.FindControl("lbl_summary");
                            total += decimal.Parse(lbl_summary_row.Text);
                        }

                        lbl_total_all.Text = total.ToString("#,##0.0000");

                        txt_qty_row.Focus();
                        return;
                    }
                }

                //if( (txt_qty_decimal > lbl_Remain_Qty_int ) )
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนจ่ายครั้งนี้ มากกว่า คงค้าง');", true);
                //    txt_qty_row.Text = "";
                //    hid_Lot.Value = "";
                //    hid_Amount.Value = "";
                //    lbl_summary.Text = "0.00";
                //    lbl_status.Text = "";
                //    cb_Remain.Enabled = true;
                //    cb_Remain.Checked = false;
                //    cb_Close.Enabled = true;
                //    cb_Close.Checked = false;
                //    //lbl_total_all.Text = "0.00";

                //    foreach (GridViewRow item in gvDetail_withdrawal.Rows)
                //    {
                //        lbl_summary_row = (Label)item.FindControl("lbl_summary");
                //        total += decimal.Parse(lbl_summary_row.Text);
                //    }

                //    lbl_total_all.Text = total.ToString("#,##0.00");

                //    txt_qty_row.Focus();
                //    return;
                //}

                if ((txt_qty_int > lbl_store_Qty_int))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนจ่ายครั้งนี้ มากกว่า ในคลัง');", true);
                    txt_qty_row.Text = "";
                    hid_Lot.Value = "";
                    hid_Amount.Value = "";
                    lbl_summary.Text = "0.0000";
                    lbl_status.Text = "";
                    cb_Remain.Enabled = true;
                    cb_Remain.Checked = false;
                    cb_Close.Enabled = true;
                    cb_Close.Checked = false;
                    //lbl_total_all.Text = "0.00";

                    foreach (GridViewRow item in gvDetail_withdrawal.Rows)
                    {
                        lbl_summary_row = (Label)item.FindControl("lbl_summary");
                        total += decimal.Parse(lbl_summary_row.Text);
                    }

                    lbl_total_all.Text = total.ToString("#,##0.0000");

                    txt_qty_row.Focus();
                    return;
                }




                if (txt_qty_int == (lbl_Order_Qty_int - lbl_Pay_Qty_int))
                {
                    lbl_status.Text = "จ่ายครบ";
                    hid_status.Value = "3";
                    cb_Remain.Enabled = false;
                    cb_Remain.Checked = false;
                    cb_Close.Enabled = false;
                    cb_Close.Checked = true;
                }
                else if (txt_qty_int < (lbl_Order_Qty_int - lbl_Pay_Qty_int))
                {
                    //lbl_status.Text = "ค้างจ่าย";
                    //hid_status.Value = "1";

                    lbl_status.Text = "ปิดการจ่าย";
                    hid_status.Value = "2";

                    cb_Remain.Enabled = true;
                    cb_Remain.Checked = false;
                    cb_Close.Enabled = true;
                    cb_Close.Checked = true;
                }



                lbl_summary.Text = ((txt_qty_int * lbl_unit_price_decimal).ToString("#,##0.0000"));

                bool chk_lot = false;

                List<string> lot = hid_Lot.Value.Split('|').ToList();
                List<string> amount = hid_Amount.Value.Split('|').ToList();

                if (e != System.EventArgs.Empty)
                {
                    for (int i = 0; i < lot.Count; i++)
                    {
                        if (txt_lot_no.Text.Trim() == "")
                        {
                            if (lot[i] == "-")
                            {
                                amount[i] = (int.Parse(amount[i]) + int.Parse(txt_qty.Text.Trim())).ToString();
                                chk_lot = true;
                                break;
                            }
                        }
                        else
                        {
                            if (lot[i] == txt_lot_no.Text.Trim())
                            {
                                amount[i] = (int.Parse(amount[i]) + int.Parse(txt_qty.Text.Trim())).ToString();
                                chk_lot = true;
                                break;
                            }
                        }
                    }
                }

                if (chk_lot)
                {
                    hid_Lot.Value = "";
                    hid_Amount.Value = "";


                    for (int i = 0; i < lot.Count; i++)
                    {
                        if (i == 0)
                        {
                            hid_Lot.Value = lot[i];
                            hid_Amount.Value = amount[i];
                        }
                        //else if (i == (lot.Count - 1))
                        //{
                        //    hid_Lot.Value = hid_Lot.Value + lot[i];
                        //    hid_Amount.Value = hid_Amount.Value + amount[i];
                        //}
                        else
                        {
                            hid_Lot.Value = hid_Lot.Value + "|" + lot[i];
                            hid_Amount.Value = hid_Amount.Value + "|" + amount[i];
                        }
                    }

                }
                else
                {

                    if (hid_Lot.Value == "" || e != null)
                    {
                        if (txt_lot_no.Text.Trim() == "" || e != null)
                            hid_Lot.Value = "-";
                        else
                            hid_Lot.Value = txt_lot_no.Text;
                    }
                    else
                    {
                        if (txt_lot_no.Text.Trim() == "")
                            hid_Lot.Value = hid_Lot.Value + "|-";
                        else
                            hid_Lot.Value = hid_Lot.Value + "|" + txt_lot_no.Text;
                    }

                    if (hid_Amount.Value == "" || e != null)
                    {
                        if (e != null)
                            hid_Amount.Value = txt_qty_row.Text;
                        else
                            hid_Amount.Value = txt_qty.Text;
                    }
                    else
                    {
                        if (e != null)
                            hid_Amount.Value = hid_Amount.Value + "|" + txt_qty_row.Text;
                        else
                            hid_Amount.Value = hid_Amount.Value + "|" + txt_qty.Text;
                    }
                }

            }
            else
            {
                hid_Lot.Value = "";
                hid_Amount.Value = "";

                lbl_summary.Text = "0.0000";
                lbl_status.Text = "";
                cb_Remain.Enabled = true;
                cb_Remain.Checked = false;
                cb_Close.Enabled = true;
                cb_Close.Checked = false;
            }

            lbl_Remain_Qty.Text = (lbl_Order_Qty_int - (lbl_Pay_Qty_int + txt_qty_int)).ToString();


            foreach (GridViewRow item in gvDetail_withdrawal.Rows)
            {
                lbl_summary_row = (Label)item.FindControl("lbl_summary");
                total += decimal.Parse(lbl_summary_row.Text);
                //if (row.RowIndex == item.RowIndex)
                //{
                //    total += decimal.Parse(lbl_summary.Text);
                //}
                //else
                //{
                //    lbl_summary_row = (Label)row.FindControl("lbl_unit_price");
                //    total += decimal.Parse(lbl_summary_row.Text);
                //}
            }

            lbl_total_all.Text = total.ToString("#,##0.0000");


            if (row.RowIndex < gvDetail_withdrawal.Rows.Count - 1)
            {
                TextBox txt_qty_nextRow = (TextBox)gvDetail_withdrawal.Rows[row.RowIndex + 1].FindControl("txt_qty");
                txt_qty_nextRow.Focus();
            }

        }

        protected void Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)(((CheckBox)sender).Parent.Parent));

            CheckBox cb_Remain = (CheckBox)row.FindControl("cb_Remain");
            CheckBox cb_Close = (CheckBox)row.FindControl("cb_Close");
            Label lbl_status = (Label)row.FindControl("lbl_status");
            HiddenField hid_status = (HiddenField)row.FindControl("hid_status");
            CheckBox checkbox = (CheckBox)sender;

            if (checkbox.ID.Equals("cb_Remain"))
            {
                if (checkbox.Checked)
                {
                    cb_Close.Checked = false;
                    hid_status.Value = "1";
                    lbl_status.Text = "ค้างจ่าย";
                }
            }
            else if (checkbox.ID.Equals("cb_Close"))
            {
                if (checkbox.Checked)
                {
                    cb_Remain.Checked = false;
                    hid_status.Value = "2";
                    lbl_status.Text = "ปิดการจ่าย";
                }
            }
        }

        protected void gvDetail_withdrawal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region LPA 17012014
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdRemarkOrg = (HiddenField)e.Row.FindControl("hdRemarkOrg");
                HiddenField hdRemarkStock = (HiddenField)e.Row.FindControl("hdRemarkStock");

                DataRowView drv = (DataRowView)e.Row.DataItem;

                hdRemarkOrg.Value = drv["RemarkOrg"].ToString() == "" ? "" : drv["RemarkOrg"].ToString();

                string show_remarkOrg = drv["RemarkOrg"].ToString().Length > 5 ? drv["RemarkOrg"].ToString().Substring(0, 5) + "..." : drv["RemarkOrg"].ToString().Length == 0 ? "หมายเหตุ" : drv["RemarkOrg"].ToString();
                //string show_remarkStock = hdRemarkStock.Value.Length > 5 ? hdRemarkStock.Value.Substring(0, 5) + "..." : hdRemarkStock.Value.Length == 0 ? "หมายเหตุ" : hdRemarkStock.Value;

                e.Row.Cells[14].Text = "<a href=\"javascript:open_popup('../Request/pop_Remark.aspx?ctl=" + hdRemarkStock.ClientID + "&chk=" + "t" + "&type=" + "stk"
                                          + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">" + "หมายเหตุ" + "</a>";

                e.Row.Cells[15].Text = "<a href=\"javascript:open_popup('../Request/pop_Remark.aspx?ctl=" + hdRemarkOrg.ClientID + "&chk=" + "f" + "&type=" + "org"
                                          + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">" + show_remarkOrg + "</a>";

                // Edit 20/05/2014
                TextBox tbQty = (TextBox)e.Row.FindControl("txt_qty");
                Label lbl_Order_Qty = (Label)e.Row.FindControl("lbl_Order_Qty");       // เบิก
                Label lbl_Pay_Qty = (Label)e.Row.FindControl("lbl_Pay_Qty");         // จ่ายสะสม
                HiddenField hdAllocate = (HiddenField)e.Row.FindControl("hid_Allocate");
                HiddenField hdAmount = (HiddenField)e.Row.FindControl("hid_Amount");
                int orderQty = string.IsNullOrEmpty(lbl_Order_Qty.Text) ? 0 : Convert.ToInt32(lbl_Order_Qty.Text);
                int payQty = string.IsNullOrEmpty(lbl_Pay_Qty.Text) ? 0 : Convert.ToInt32(lbl_Pay_Qty.Text);

                int remain = orderQty - payQty;

                if (remain < orderQty)
                    tbQty.Text = "0";
                else
                    tbQty.Text = hdAllocate.Value;

                hdAmount.Value = tbQty.Text;
                // End Edit

                // Edit 14/05/2015
                Label lbl_unit_price = (Label)e.Row.FindControl("lbl_unit_price");
                decimal unit_price = 0.0M;

                if (decimal.TryParse(lbl_unit_price.Text, out unit_price))
                {
                    if (unit_price < 0)
                    {
                        lbl_unit_price.Style["color"] = "red";
                    }
                }

                // End Edit
            }
            #endregion

        }

        protected void btn_assure_Click(object sender, EventArgs e)
        {

            Label lbl_item_no;
            TextBox txt_qty_row = new TextBox();
            //HiddenField hid_Lot;
            //HiddenField hid_Amount;

            int rowIndex = new int();

            bool IsConstain = false;

            foreach (GridViewRow item in gvDetail_withdrawal.Rows)
            {
                lbl_item_no = (Label)item.FindControl("lbl_item_no");
                txt_qty_row = (TextBox)item.FindControl("txt_qty");

                HiddenField hid_Inv_ItemID = (HiddenField)item.FindControl("hid_Inv_ItemID");
                HiddenField hid_Pack_ID = (HiddenField)item.FindControl("hid_Pack_ID");

                if (lbl_item_no.Text.Trim().Equals(txt_No.Text.Trim()))
                {

                    //DataTable dt = dtRequest.Rows[0]["Stock_Id_From"].ToString()

                    if (txt_lot_no.Text.Trim() != "")
                    {

                        List<SqlParameter> param = new List<SqlParameter>();

                        param.Add(new SqlParameter("@Stock_ID", dtRequest.Rows[0]["Stock_Id_From"].ToString()));
                        param.Add(new SqlParameter("@Inv_ItemID", hid_Inv_ItemID.Value));
                        param.Add(new SqlParameter("@Pack_ID", hid_Pack_ID.Value));
                        param.Add(new SqlParameter("@Lot_No", txt_lot_no.Text.Trim()));

                        DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_STOCK_LOT", param);

                        if (dt.Rows.Count <= 0)
                        {
                            txt_lot_no.Focus();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบสินค้า Lot No. " + txt_lot_no.Text.Trim() + @" ที่ระบุ');", true);
                            return;
                        }
                        else
                        {
                            if (int.Parse(txt_qty.Text.Trim()) > int.Parse(dt.Rows[0]["qty"].ToString()))
                            {
                                txt_qty.Focus();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('สินค้า Lot No. " + txt_lot_no.Text.Trim() + @" ไม่พอสำหรับการจ่าย');", true);
                                return;
                            }
                        }
                    }


                    IsConstain = true;
                    rowIndex = item.RowIndex;
                    break;
                }
            }
            if (IsConstain)
            {
                //string Lot_No = txt_lot_no.Text;

                int qty = int.Parse(txt_qty_row.Text == "" ? "0" : txt_qty_row.Text) + int.Parse(txt_qty.Text);
                txt_qty_row.Text = qty.ToString();
                txt_qty_TextChanged(txt_qty_row, null);


                //hid_Lot = (HiddenField)gvDetail_withdrawal.Rows[rowIndex].FindControl("hid_Lot");
                //hid_Amount = (HiddenField)gvDetail_withdrawal.Rows[rowIndex].FindControl("hid_Amount");

                //if(hid_Lot.Value == "")
                //    hid_Lot.Value = txt_lot_no.Text;
                //else
                //    hid_Lot.Value = hid_Lot + "|" + txt_lot_no.Text;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบสินค้า');", true);
                return;
            }
        }

        protected void btnItem_Help_Click(object sender, ImageClickEventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js3", @" 
            window.open('pop_ProductHelp.aspx?st_id=" + ddl_stock.SelectedValue + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
            ", true);
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (hid_Request_Status.Value == "3")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ใบเบิกยังไม่ถูก Allocate ไม่สามารถทำจ่ายได้');", true);
                return;
            }


            DataTable dtTemp1 = new DataTable();

            dtTemp1.Columns.Add("Request_No", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Req_ItemID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Inv_ItemID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Pack_ID", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("Pay_Quantity", System.Type.GetType("System.Int32"));
            dtTemp1.Columns.Add("OnHand_Qty", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Unit_Price", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Amount", System.Type.GetType("System.Decimal"));
            dtTemp1.Columns.Add("Req_ItemStatus", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Summary_ReqItem_Id", System.Type.GetType("System.String"));

            dtTemp1.Columns.Add("Lot_No", System.Type.GetType("System.String"));
            dtTemp1.Columns.Add("Pay_Amount", System.Type.GetType("System.String"));

            #region LPA 17012014

            dtTemp1.Columns.Add("Pay_Remark", System.Type.GetType("System.String"));

            #endregion


            int i = 0;

            foreach (GridViewRow row in gvDetail_withdrawal.Rows)
            {
                TextBox txt_qty = (TextBox)row.FindControl("txt_qty");
                Label lbl_Remain_Qty = (Label)row.FindControl("lbl_Remain_Qty");
                CheckBox cb_Remain = (CheckBox)row.FindControl("cb_Remain");
                CheckBox cb_Close = (CheckBox)row.FindControl("cb_Close");
                HiddenField hid_status = (HiddenField)row.FindControl("hid_status");
                Label lbl_store_Qty = (Label)row.FindControl("lbl_store_Qty");

                Label lbl_Order_Qty = (Label)row.FindControl("lbl_Order_Qty");
                Label lbl_Pay_Qty = (Label)row.FindControl("lbl_Pay_Qty");

                HiddenField hid_Lot = (HiddenField)row.FindControl("hid_Lot");
                HiddenField hid_Pay_Amount = (HiddenField)row.FindControl("hid_Amount");
                Label lbl_unit_price = (Label)row.FindControl("lbl_unit_price");
                Label lbl_item_no = (Label)row.FindControl("lbl_item_no");
                Label lbl_item_name = (Label)row.FindControl("lbl_item_name");

                #region LPA 17012014

                HiddenField hdRemarkStock = (HiddenField)row.FindControl("hdRemarkStock");

                #endregion

                if (txt_qty.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาระบุจำนวนจ่ายครั้งนี้');", true);
                    txt_qty.Focus();
                    return;
                }
                else if (int.Parse(txt_qty.Text.Trim()) > (int.Parse(lbl_Order_Qty.Text) - int.Parse(lbl_Pay_Qty.Text)))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนจ่ายครั้งนี้มากกว่าคงค้าง');", true);
                    txt_qty.Focus();
                    return;
                }
                else if (cb_Remain.Checked == false && cb_Close.Checked == false)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาเลือกสถานะ');", true);
                    cb_Remain.Focus();
                    return;
                }

                DataRow dr = dtTemp1.NewRow();

                dr["Request_No"] = this.dtRequestItem.Rows[i]["Request_No"].ToString();
                dr["Req_ItemID"] = this.dtRequestItem.Rows[i]["Req_ItemID"].ToString();
                dr["Inv_ItemID"] = this.dtRequestItem.Rows[i]["Inv_ItemID"].ToString();
                dr["Pack_ID"] = this.dtRequestItem.Rows[i]["Pack_ID"].ToString();
                dr["Pay_Quantity"] = decimal.Parse(txt_qty.Text.Trim());
                dr["OnHand_Qty"] = this.dtRequestItem.Rows[i].IsNull("OnHand_Qty") ? "0" : this.dtRequestItem.Rows[i]["OnHand_Qty"].ToString();

                dr["Unit_Price"] = this.dtRequestItem.Rows[i]["Avg_Cost"].ToString();

                decimal amount = decimal.Parse(this.dtRequestItem.Rows[i]["Avg_Cost"].ToString()) * decimal.Parse(txt_qty.Text.Trim());

                dr["Amount"] = amount;
                dr["Req_ItemStatus"] = hid_status.Value;
                dr["Summary_ReqItem_Id"] = this.dtRequestItem.Rows[i].IsNull("Summary_ReqItem_Id") ? "" : this.dtRequestItem.Rows[i]["Summary_ReqItem_Id"].ToString();

                dr["Lot_No"] = hid_Lot.Value;
                dr["Pay_Amount"] = hid_Pay_Amount.Value;

                #region LPA 17012014

                dr["Pay_Remark"] = hdRemarkStock.Value;

                #endregion

                dtTemp1.Rows.Add(dr);


                List<SqlParameter> param2 = new List<SqlParameter>();

                param2.Add(new SqlParameter("@Stock_id", dtRequest.Rows[0]["Stock_Id_From"].ToString()));
                param2.Add(new SqlParameter("@Inv_ItemID", this.dtRequestItem.Rows[i]["Inv_ItemID"].ToString()));
                param2.Add(new SqlParameter("@Pack_ID", this.dtRequestItem.Rows[i]["Pack_ID"].ToString()));

                DataTable dt_Chk_OnhandQty = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Stock_OnHand", param2);

                decimal Onhand_Now = 0;

                if (dt_Chk_OnhandQty.Rows.Count > 0)
                {
                    Onhand_Now = Convert.ToDecimal(dt_Chk_OnhandQty.Rows[0]["OnHand_Qty"]);
                }


                if (Onhand_Now < Convert.ToDecimal(txt_qty.Text.Trim()))
                {
                    lbl_store_Qty.Text = Onhand_Now.ToString("#,##0");
                    txt_qty.Text = "";
                    txt_qty.Focus();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('" + dt_Chk_OnhandQty.Rows[0]["Item_Search_Desc"].ToString() + @" มียอดคงคลังเหลือ : " + Onhand_Now.ToString("#,##0") + @"');", true);
                    return;

                }

                decimal unit_price = 0;
                if (!decimal.TryParse(lbl_unit_price.Text, out unit_price))
                    unit_price = 0;

                if (int.Parse(txt_qty.Text) > 0 && unit_price < 0)
                {
                    ScriptManager.RegisterStartupScript
                    (
                        this
                        , this.GetType()
                        , "fail"
                        , "alert('รหัส " + lbl_item_no.Text + " รายการ " + lbl_item_name.Text + " ราคาต่อหน่วยติดลบ กรุณาตรวจสอบ');"
                        , true
                    );
                    return;
                }


                i++;
            }


            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Pay_Id", 0));
            param[0].Direction = ParameterDirection.Output;

            param.Add(new SqlParameter("@Request_Id", dtRequest.Rows[0]["Request_Id"].ToString()));
            param.Add(new SqlParameter("@Stock_Id_Pay", dtRequest.Rows[0]["Stock_Id_From"].ToString()));
            param.Add(new SqlParameter("@Pay_By", this.UserID));
            param.Add(new SqlParameter("@Pay_Status", "1"));
            param.Add(new SqlParameter("@delivery_location", this.UserID));

            if (hid_Summary_ReqId.Value != "")
            {
                param.Add(new SqlParameter("@Summary_ReqId", hid_Summary_ReqId.Value));
            }

            param.Add(new SqlParameter("@Create_By", this.UserID));

            try
            {
                #region LPA 04102013

                int req_Id = Int32.Parse(hid_Request_Id.Value == "" ? "0" : hid_Request_Id.Value);
                RequestDAO req = new RequestDAO();
                DataTable dtReq = req.GetRequest(req_Id);
                string req_status = "";
                string req_consider_type = "";

                if (dtReq.Rows.Count > 0)
                {
                    req_status = dtReq.Rows[0]["Request_Status"].ToString();
                    req_consider_type = dtReq.Rows[0]["Consider_Type"].ToString();
                }

                #endregion

                string result_PayId = "";
                bool result = RoutineStockDAO.InsertStockPay(param, dtRequest, dtTemp1, this.UserName, this.UserID, hid_Request_Id.Value, hid_Summary_ReqId.Value, out result_PayId);

                //bool result = false;

                if (result)
                {
                    if (rdb_summary_req.Checked == true && rdb_inv_req.Checked == false)
                    {
                        BindData_summary();
                    }
                    else
                    {
                        BindData_withdrawal();
                    }

                    BindData_withdrawal(hid_Summary_ReqId.Value);
                    Bind_Detail();

                    for (int j = 0; j < gvResult_withdrawal.Rows.Count; j++)
                    {
                        if (j == int.Parse(hid_rowIndex.Value))
                        {
                            gvResult_withdrawal.Rows[j].BackColor = Color.FromArgb(58, 143, 227);
                        }
                        else
                            //row.BackColor = Color.Empty;
                            gvResult_withdrawal.Rows[j].BackColor = Color.Empty;

                    }

                    #region LPA 2509213

                    // Add ส่วนทำรับ Auto ในกรณที่ OrgStruc_Id_Req มี NotApprove_Flag = 1
                    //string req_Id = hid_Request_Id.Value;
                    //string req_status = dtRequest.Rows[0]["Request_Status"].ToString();
                    //string req_consider_type = dtRequest.Rows[0]["Consider_Type"].ToString();

                    //string OrgStruc_Id_Req = "";
                    //if (dtRequest.Rows.Count > 0)
                    //{
                    //    OrgStruc_Id_Req = dtRequest.Rows[0]["OrgStruc_Id_Req"].ToString();
                    //}

                    //DataTable dtOrg = OrgStructureDAO.GetOrgStructure(OrgStruc_Id_Req);
                    //if (dtOrg.Rows.Count > 0)
                    //{
                    //    //ถ้า NotApprove_Flag = 1 ให้ทำการรับ Auto
                    //    if(dtOrg.Rows[0]["NotApprove_Flag"].ToString() == "1")
                    //    {
                    //        RequestDAO.InsertStockReqRecAuto(req_Id, result_PayId, this.UserID);
                    //    }
                    //}

                    // Add ส่วนทำรับ Auto ในกรณที่ Request_Status = 2 และ Consider_Type = NULL
                    // 5/02/2014 เพิ่ม OrgStruc_Id_Req IS NOT NULL
                    string request_Id = hid_Request_Id.Value;
                    string OrgStruc_Id_Req = "";
                    if (dtRequest.Rows.Count > 0)
                    {
                        OrgStruc_Id_Req = dtRequest.Rows[0]["OrgStruc_Id_Req"].ToString();
                    }
                    if ((req_status == "2" || req_status == "3" || req_status == "4" || req_status == "5") && OrgStruc_Id_Req != "")
                    {
                        RequestDAO.InsertStockReqRecAuto(request_Id, result_PayId, this.UserID);
                    }


                    #endregion

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมุล');", true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RequestReportPopup", "open_popup('../Request/RequestReport.aspx?id=" + hid_Request_Id.Value + "&pay_id=" + result_PayId + "', 850, 450, 'pop', 'yes', 'yes', 'yes');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('" + ex.Message + @"');", true);
            }





        }

        protected void btn_Clear_Data_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvDetail_withdrawal.Rows)
            {
                TextBox txt_qty = (TextBox)row.FindControl("txt_qty");
                Label lbl_summary = (Label)row.FindControl("lbl_summary");
                CheckBox cb_Remain = (CheckBox)row.FindControl("cb_Remain");
                CheckBox cb_Close = (CheckBox)row.FindControl("cb_Close");

                Label lbl_status = (Label)row.FindControl("lbl_status");
                HiddenField hid_status = (HiddenField)row.FindControl("hid_status");

                txt_qty.Text = "";
                lbl_summary.Text = "";
                lbl_status.Text = "";
                cb_Remain.Enabled = true;
                cb_Remain.Checked = false;
                cb_Close.Enabled = true;
                cb_Close.Checked = false;

            }

            lbl_total_all.Text = "0.00";
        }

        protected void btn_Cancel_Summary_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js1", @" 
            window.open('RoutineStock_Cancel.aspx?req=" + hid_Request_Id.Value + @"&sumId=" + hid_Summary_ReqId.Value + @"','Popup','toolbar=0, menubar=0,location=0,status=1,scrollbars=1,resizable=1,left=130,top=0,width=1077,height=580');
            ", true);
        }

        protected void txt_No_TextChanged(object sender, EventArgs e)
        {
            Label lbl_item_no;
            TextBox txt_qty_row = new TextBox();
            Label lbl_item_name_grid;
            Label lbl_pack_name_grid;

            int rowIndex = new int();

            bool IsConstain = false;

            foreach (GridViewRow item in gvDetail_withdrawal.Rows)
            {
                lbl_item_no = (Label)item.FindControl("lbl_item_no");
                txt_qty_row = (TextBox)item.FindControl("txt_qty");

                lbl_item_name_grid = (Label)item.FindControl("lbl_item_name");
                lbl_pack_name_grid = (Label)item.FindControl("lbl_pack_name");

                if (lbl_item_no.Text.Trim().Equals(txt_No.Text.Trim()))
                {
                    IsConstain = true;

                    txt_item_name.Text = lbl_item_name_grid.Text;
                    txt_pack.Text = lbl_pack_name_grid.Text;

                    rowIndex = item.RowIndex;
                    break;
                }
            }
            if (IsConstain)
            {
                txt_lot_no.Focus();
            }
            else
            {
                txt_item_name.Text = "";
                txt_pack.Text = "";
                txt_No.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบสินค้า');", true);
                return;
            }
        }


    }
}