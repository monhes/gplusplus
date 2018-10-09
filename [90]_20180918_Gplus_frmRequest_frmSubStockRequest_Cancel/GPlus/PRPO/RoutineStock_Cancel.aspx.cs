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
    public partial class RoutineStock_Cancel : Pagebase
    {
        clsFuncMa Func = new clsFuncMa();
        RoutineStockDAO RoutineStockDAO = new RoutineStockDAO();

        public DataTable dtStockPayItem
        {
            get { return (ViewState["dtStockPayItem"] == null) ? null : (DataTable)ViewState["dtStockPayItem"]; }
            set { ViewState["dtStockPayItem"] = value; }
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
                if (Request.QueryString["req"] != null)
                {
                    string Req = Request.QueryString["req"].ToString();

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

            DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Inv_StockPay", param);

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
                    string Summary_ReqId = data[3];
                    DateTime Pay_Date = Convert.ToDateTime(data[1]);

                    if (data[2] == "0")
                        btn_Submit_Cancel.Enabled = true;
                    else
                        btn_Submit_Cancel.Enabled = false;
                        
                    hid_Pay_Id.Value = Pay_Id;
                    hid_Summary_ReqId.Value = Summary_ReqId;

                    lbl_date.Text = String.Format("{0:dd/MM/yyyy}", Pay_Date, new System.Globalization.CultureInfo("th-TH"));
                    lbl_time.Text = String.Format("{0:HH:mm}", Pay_Date, new System.Globalization.CultureInfo("th-TH"));

                    Control_Panel_StockPayItem.Visible = true;


                    List<SqlParameter> param = new List<SqlParameter>();

                    param.Add(new SqlParameter("@Request_Id", hid_Request_Id.Value));
                    param.Add(new SqlParameter("@Pay_Id", Pay_Id));
                    dtStockPayItem = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_Inv_StockPayItem", param);

                    gv_StockPayItem.DataSource = dtStockPayItem;
                    gv_StockPayItem.DataBind();
                    
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

                TableCell headercell11 = new TableCell()
                {
                    ColumnSpan = 1,
                    RowSpan = 1,
                    Height = 20,
                    Text = "ยกเลิกจ่าย ",
                    HorizontalAlign = HorizontalAlign.Center
                };
                headerow2.Cells.Add(headercell11);


                gv_StockPayItem.Controls[0].Controls.AddAt(0, headerow);
                gv_StockPayItem.Controls[0].Controls.AddAt(1, headerow2);


            }




        }

        protected void btn_Close_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js2", @" 
            window.close();
            ", true);
        }

        protected void btn_Submit_Cancel_Click(object sender, EventArgs e)
        {

            //System.Threading.Thread.Sleep(5000);

            List<SqlParameter> param2 = new List<SqlParameter>();

            param2.Add(new SqlParameter("@Request_Id", hid_Request_Id.Value));

            dtRequestItem = new DatabaseHelper().ExecuteDataTable("sp_Inv_Select_RequestItem", param2);

            dtRequestItem.Columns.Add("StockPayItem_PayQty", System.Type.GetType("System.Decimal"));
            dtRequestItem.Columns.Add("StockPayItem_Amount", System.Type.GetType("System.Decimal"));

            int i = 0;
            foreach (DataRow drRow in dtRequestItem.Rows)
            {
                DataRow[] drow_StockPayItem = dtStockPayItem.Select("Inv_ItemCode = '" + drRow["Inv_ItemCode"] + "'");

                //drRow["StockPayItem_PayQty"] = decimal.Parse(dtStockPayItem.Rows[i]["Pay_Quantity"].ToString());
                //drRow["StockPayItem_Amount"] = decimal.Parse(dtStockPayItem.Rows[i]["Amount"].ToString());

                drRow["StockPayItem_PayQty"] = decimal.Parse(drow_StockPayItem[0]["Pay_Quantity"].ToString());
                drRow["StockPayItem_Amount"] = decimal.Parse(drow_StockPayItem[0]["Amount"].ToString());

                i++;
            }

            bool result = RoutineStockDAO.UpdateStockPay(dtRequestItem, this.UserName, this.UserID, hid_Request_Id.Value, hid_Pay_Id.Value, hid_Stock_Id_Pay.Value, hid_Summary_ReqId.Value);

            if (result)
            {
                BindStockPay();
                Control_Panel_StockPayItem.Visible = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('บันทึกข้อมุล');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ระบบขัดข้อง กรุณาลองใหม่อีกครั้ง');", true);
            }
        }
    }
}