using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GPlus.DataAccess;
using System.Data.SqlClient;

namespace GPlus.PRPO
{
    public partial class RequestReportPay : Pagebase
    {
        clsFuncMa Func = new clsFuncMa();
        RoutineStockDAO RoutineStockDAO = new RoutineStockDAO();

        public DataTable dtStockPayItem2
        {
            get { return (ViewState["dtStockPayItem2"] == null) ? null : (DataTable)ViewState["dtStockPayItem2"]; }
            set { ViewState["dtStockPayItem2"] = value; }
        }

        public DataTable dtRequestItem2
        {
            get { return (ViewState["dtRequestItem2"] == null) ? null : (DataTable)ViewState["dtRequestItem2"]; }
            set { ViewState["dtRequestItem2"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["req"] != null)
                {
                    string Req = Request.QueryString["req"].ToString();

                    //List<SqlParameter> param = new List<SqlParameter>();
                    //param.Add(new SqlParameter("@Request_Id", Req));
                    //DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Inv_StockPay", param);

                    if (Request.QueryString["sumId"] != null)
                        hid_Summary_ReqId.Value = Request.QueryString["sumId"].ToString();

                    hid_Request_Id.Value = Req;

                    BindStockPay();
                }
            }
        }


        private void BindStockPay()
        {
            List<SqlParameter> param = new List<SqlParameter>();

            param.Add(new SqlParameter("@Request_Id", hid_Request_Id.Value));

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Inv_StockPay_PayActive", param);

            if(dt.Rows.Count > 0)
                hid_Stock_Id_Pay.Value = dt.Rows[0]["Stock_Id_Pay"].ToString();

            gv_StockPay.DataSource = dt;
            gv_StockPay.DataBind();


        }

        protected void gv_StockPay_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Detail")
                {
                    //string Pay_Id = e.CommandArgument.ToString();

                    string[] data = e.CommandArgument.ToString().Split('|');
                    string Pay_Id = data[0];
                    DateTime Pay_Date = Convert.ToDateTime(data[1]);

                    //if (data[2] == "0")
                    //    btn_Submit_Cancel.Enabled = true;
                    //else
                    //    btn_Submit_Cancel.Enabled = false;
                        

                    hid_Pay_Id.Value = Pay_Id;

                    lbl_date.Text = String.Format("{0:dd/MM/yyyy}", Pay_Date, new System.Globalization.CultureInfo("th-TH"));
                    lbl_time.Text = String.Format("{0:HH:mm}", Pay_Date, new System.Globalization.CultureInfo("th-TH"));

                    Control_Panel_StockPayItem.Visible = true;


                    List<SqlParameter> param = new List<SqlParameter>();

                    param.Add(new SqlParameter("@Request_Id", hid_Request_Id.Value));
                    param.Add(new SqlParameter("@Pay_Id", Pay_Id));
                    dtStockPayItem2 = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Inv_StockPayItem", param);


                    gv_StockPayItem.DataSource = dtStockPayItem2;
                    gv_StockPayItem.DataBind();
                    
                }
                else if (e.CommandName == "Print")
                {
                    string Pay_Id = e.CommandArgument.ToString();
                    string Request_Id = Request["req"];

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "RequestReportPopup", "open_popup('../Request/RequestReport.aspx?id=" + Request_Id + "&pay_id=" + Pay_Id + "', 850, 450, 'pop', 'yes', 'yes', 'yes');", true);
                    
                }

            }
            catch (Exception ex) { }
        }

        protected void gv_StockPayItem_RowCreated(object sender, GridViewRowEventArgs e)
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
                    Text = "ลำดับ<br>สินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell1);


                TableCell headercell2 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "รหัส<br>สินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell2);


                TableCell headercell3 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "ชื่อสินค้า",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell3);

                TableCell headercell4 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 40,
                    Text = "หน่วย",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell4);

                TableCell headercell5 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 2,
                    Height = 20,
                    Text = "ราคา<br>ต่อหน่วย",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell5);

                TableCell headercell6 = new TableCell()
                {
                    ColumnSpan = 4,
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
                    Text = "เบิก",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell7);

                TableCell headercell8 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "จ่าย<br>สะสม",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell8);

                TableCell headercell9 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "คงค้าง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell9);

                TableCell headercell10 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "จำนวนจ่าย<br>ครั้งนี้",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell10);

                #region LPA 17012014

                TableCell headercell11 = new TableCell()
                {
                    ColumnSpan = 2,
                    RowSpan = 1,
                    Height = 20,
                    Text = "หมายเหตุ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow.Cells.Add(headercell11);

                TableCell headercell12 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "คลัง",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell12);

                TableCell headercell13 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Width = 40,
                    Text = "หน่วยงาน",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell13);
                #endregion

                gv_StockPayItem.Controls[0].Controls.AddAt(0, headerow);
                gv_StockPayItem.Controls[0].Controls.AddAt(1, headerow2);


            }




        }

        #region LPA 17012014
        protected void gv_StockPayItem_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdRemarkOrg = (HiddenField)e.Row.FindControl("hdRemarkOrg");
                HiddenField hdRemarkStock = (HiddenField)e.Row.FindControl("hdRemarkStock");

                DataRowView drv = (DataRowView)e.Row.DataItem;

                hdRemarkOrg.Value = drv["RemarkOrg"].ToString() == "" ? "" : drv["RemarkOrg"].ToString();
                hdRemarkStock.Value = drv["Pay_Remark"].ToString() == "" ? "" : drv["Pay_Remark"].ToString();

                string show_remarkOrg = drv["RemarkOrg"].ToString().Length > 5 ? drv["RemarkOrg"].ToString().Substring(0, 5) + "..." : drv["RemarkOrg"].ToString().Length == 0 ? "หมายเหตุ" : drv["RemarkOrg"].ToString();
                string show_remarkStock = drv["Pay_Remark"].ToString().Length > 5 ? drv["Pay_Remark"].ToString().Substring(0, 5) + "..." : drv["Pay_Remark"].ToString().Length == 0 ? "หมายเหตุ" : drv["Pay_Remark"].ToString();

                e.Row.Cells[9].Text = "<a href=\"javascript:open_popup('../Request/pop_Remark.aspx?ctl=" + hdRemarkStock.ClientID + "&chk=" + "f" + "&type=" + "stk"
                                          + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">" + show_remarkStock + "</a>";

                e.Row.Cells[10].Text = "<a href=\"javascript:open_popup('../Request/pop_Remark.aspx?ctl=" + hdRemarkOrg.ClientID + "&chk=" + "f" + "&type=" + "org" 
                                          + "', 550, 200, 'popD', 'yes', 'yes', 'yes');\">" + show_remarkOrg + "</a>";



            }
        }
        #endregion

        protected void btn_Close_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js2", @" 
            window.close();
            ", true);
        }

    }
}