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
    public partial class MgtStockCardMovment : Pagebase
    {
        public DataTable MgtStockCardMovmentPackageTable
        {
            get
            {
                return (DataTable)Session["MgtStockCardMovment"];
            }
            set
            {
                Session["MgtStockCardMovment"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                this.PageID = "407";
                BindDropdown();
            }
            PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);

        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindDropdown()
        {
            DataTable dt = new DataAccess.StockDAO().GetStock("", "", "1", 1, 1000, "", "").Tables[0];
            ddlStock.DataSource = dt;
            ddlStock.DataTextField = "Stock_Name";
            ddlStock.DataValueField = "Stock_Id";
            ddlStock.DataBind();
            ddlStock.Items.Insert(0, new ListItem("เลือกประเภท", ""));

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlStock.SelectedIndex == 0)
            {
                ShowMessageBox("กรุณาเลือกคลังสินค้า");
                return;
            }
            if (dtCreatePOStart.Text == "" || dtCreatePOStop.Text == "")
            {
                ShowMessageBox("กรุณาระบุวันที่เริ่มและสิ้นสุด");
                return;
            }
            if (ItemControl2.ItemID.Trim().Length == 0)
            {
               ShowMessageBox("กรุณาเลือกรายการวัสดุอุปกรณ์");
               return;
            }
            //ShowMessageBox("Code : " + ItemControl2.ItemCode + " Name : " + ItemControl2.ItemName + " Pack : " + ItemControl2.PackName);
            BindData();
            gvMovement.Visible = true;
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlStock.SelectedIndex = 0;
            dtCreatePOStart.Text = "";
            dtCreatePOStop.Text = "";
            ItemControl2.Clear();
            gvMovement.Visible = false;

            
        }

        protected void gvMovement_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            // Invisibling columns on row header (normally created on binding)
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false; // Invisibiling วันที่/เวลา Header Cell
                e.Row.Cells[5].Visible = false; // Invisibiling คงเหลือ Header Cell
                e.Row.Cells[6].Visible = false; // Invisibiling อ้างอิงเลขที่บันทึก Header Cell
                e.Row.Cells[7].Visible = false; // Invisibiling อ้างอิงเลขที่เอกสาร Audited By Header Cell
                e.Row.Cells[8].Visible = false; // Invisibiling หมายเหตุ Header Cell
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                 DataRowView drv = (DataRowView)e.Row.DataItem;

                 e.Row.Cells[5].Text = Convert.ToInt32(drv["Total"]).ToString("#,##0");
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DataRowView drv = (DataRowView)e.Row.DataItem;
            //    if (e.Row.RowIndex == 0)
            //    {
            //        e.Row.Cells[4].Text = drv["Income_Balance"].ToString();
            //    }
            //    if (e.Row.RowIndex > 0)
            //    {
            //        int Balance = 0;
            //        int Before_balance = 0;
            //        int Previous_row = 0;
            //        Previous_row = e.Row.RowIndex - 1;
            //        if (gvMovement.Rows[Previous_row].Cells[4].Text != "")
            //        {
            //            Before_balance = Convert.ToInt16(gvMovement.Rows[Previous_row].Cells[4].Text);
            //        }
            //        Balance = Before_balance + Convert.ToInt16(drv["Recieve_Qty"].ToString()) - Convert.ToInt16(drv["Pay_Qty"].ToString());
            //        e.Row.Cells[4].Text = Balance.ToString();
            //    }

            //}
        }

        protected void gvMovement_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // Adding a column manually once the header created
            if (e.Row.RowType == DataControlRowType.Header) // If header created
            {
                GridView StockMovement = (GridView)sender;

                // Creating a Row
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                //Adding วันที่ เวลา Column
                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "วันที่ เวลา";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding ประเภทความเคลื่อนไหว Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "ประเภทความเคลื่อนไหว";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 4; // For merging three columns (ยอดยกมา, รับเข้า, จ่ายออก)
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding คงเหลือ Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "คงเหลือ";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding อ้างอิงเลขที่บันทึก Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "อ้างอิงเลขที่บันทึก";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding อ้างอิงเลขที่อ้างอิงเลขที่เอกสาร Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "อ้างอิงเลขที่เอกสาร";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding หมายเหตุ Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "หมายเหตุ";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                //HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding the Row at the 0th position (first row) in the Grid
                StockMovement.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }

        private void BindData()
        {

            //DataSet ds = new DataAccess.StockDAO().StockOnHandMoving(ddlStock.SelectedValue, dtCreatePOStart.Text, dtCreatePOStop.Text,
            //    ItemControl2.ItemID, ItemControl2.PackID, PagingControl1.CurrentPageIndex, PagingControl1.PageSize, this.SortColumn, this.SortOrder);

            DataSet ds = new DataAccess.StockDAO().StockOnHandMoving(ddlStock.SelectedValue, dtCreatePOStart.Text, dtCreatePOStop.Text,
               ItemControl2.ItemID, ItemControl2.PackID, 1, 10000, this.SortColumn, this.SortOrder);

            DataTable dt = ds.Tables[0]; 

            //คำนวณหายอดคงเหลือของแต่ละแถว จาก คงเหลือ (แถวก่อนหน้า) + รับเข้า - จ่ายออก
            DataRow[] drRows = dt.Select(" 1 = 1 ");
            int temptotal = 0;

            for (int i = 0; i < drRows.Length; i++)
            {
                if (i == 0)
                {
                    drRows[i]["Total"] = dt.Rows[i]["Income_Balance"];
                    temptotal = Convert.ToInt32(dt.Rows[i]["Income_Balance"]);
                }
                else
                {
                    drRows[i]["Total"] = temptotal + Convert.ToInt32(dt.Rows[i]["Recieve_Qty"]) - Convert.ToInt32(dt.Rows[i]["Pay_Qty"]);
                    temptotal = temptotal + Convert.ToInt32(dt.Rows[i]["Recieve_Qty"]) - Convert.ToInt32(dt.Rows[i]["Pay_Qty"]);
                }
                drRows[i].AcceptChanges();
            }

            DataRow[] drs;

            if (PagingControl1.CurrentPageIndex == 1)
            {
                drs = dt.Select("[rownumber] >= '" + PagingControl1.CurrentPageIndex + "'" + " AND [rownumber] <= '" + (PagingControl1.CurrentPageIndex * PagingControl1.PageSize) + "'");
            }
            else
            {
                drs = dt.Select("[rownumber] >= '" + (((PagingControl1.CurrentPageIndex - 1) * PagingControl1.PageSize) + 1) + "'" + " AND [rownumber] <= '" + (PagingControl1.CurrentPageIndex * PagingControl1.PageSize) + "'");
            }


            //make a new "results" datatable via clone to keep structure
            DataTable dt2 = dt.Clone();

            int cnt = 0;
            //Import the Rows
            foreach (DataRow d in drs)
            {
                dt2.ImportRow(d);
                cnt++;
            }



            if (ds.Tables[0].Rows.Count == 0)
            {
                gvMovement.Visible = false;
                ShowMessageBox("ไม่พบข้อมูล");
            }
            else
            {
                gvMovement.Visible = true;
                PagingControl1.RecordCount = (int)ds.Tables[1].Rows[0][0];

                //gvMovement.DataSource = ds.Tables[0];

                //PagingControl1.RecordCount = (int)((int)ds.Tables[1].Rows[0][0] / (PagingControl1.PageSize));

                gvMovement.DataSource = dt2;

                gvMovement.DataBind();
            }
        }

    }
}