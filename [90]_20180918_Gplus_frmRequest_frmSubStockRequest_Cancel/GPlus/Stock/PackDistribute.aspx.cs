using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.Reporting.WebForms;
using GPlus.DataAccess;

namespace GPlus.Stock
{
    public partial class PackDistribute : Pagebase
    {
        public DataTable PackDistributePackageTable
        {
            get
            {
                return (DataTable)Session["PackDistribute"];
            }
            set
            {
                Session["PackDistribute"] = value;
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                this.PageID = "402";
                BindDropdown();
            }
           // PagingControl1.CurrentPageIndexChanged += new EventHandler(PagingControl1_CurrentPageIndexChanged);

        }

        void PagingControl1_CurrentPageIndexChanged(object sender, EventArgs e)
        {
            //BindData();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlStock.SelectedIndex == 0)
            {
                //ShowMessageBox("กรุณาเลือกคลังสินค้า");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาเลือกคลังสินค้า');", true);
                return;
            }
          
            if (ItemControl2.ItemID.Trim().Length == 0)
            {
               //ShowMessageBox("กรุณาเลือกรายการวัสดุอุปกรณ์");
               ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาเลือกรายการวัสดุอุปกรณ์');", true);
               return;
            }

            if (txt_InReceive.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาระบุจำนวนที่แบ่ง');", true);
                return;
            }

            Decimal sumReceive = 0;

            foreach (GridViewRow item in gvPackList.Rows)
            {
                TextBox txt_ReceiveQty_row = (TextBox)item.FindControl("txt_ReceiveQty");
                sumReceive += decimal.Parse(txt_ReceiveQty_row.Text == "" ? "0" : txt_ReceiveQty_row.Text.Replace(",", ""));
            }
            
            Decimal onhand_qty = 0;
            DataTable dt = new StockDAO().GetOnHand(ddlStock.SelectedValue, ItemControl2.ItemID, ItemControl2.PackID);
            if (dt.Rows.Count > 0)
            {
                onhand_qty = Convert.ToDecimal(dt.Rows[0]["OnHand_Qty"].ToString() == "" ? "0" : dt.Rows[0]["OnHand_Qty"].ToString());
            }

            Decimal InReceive = Convert.ToDecimal(txt_InReceive.Text.Replace(",", "") == "" ? "0" : txt_InReceive.Text.Replace(",", ""));
            Decimal InOnHand = Convert.ToDecimal(lb_InOnhand.Text.Replace(",", "") == "" ? "0" : lb_InOnhand.Text.Replace(",", ""));
            //ทำการ Check ว่าค่าที่รับทั้งหมด เท่ากับค่าที่แบ่งหรือไม่
             if (sumReceive != InReceive)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนที่แบ่งและจำนวนที่รับรวมทั้งหมดไม่เท่ากัน');", true);
                return;
            }

            if (sumReceive > onhand_qty) //ทำการ Check ว่าค่าใน StockOnhand ของ Pack ใหญ่มีพอหรือไม่
            {
                if (InOnHand != onhand_qty) //ถ้าค่า Onhand ที่หน้าจอ ไม่เท่ากับค่าที่ดึงมาใหม่ตอนกด save
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนสินค้าคงคลังในขณะบันทึกมีไม่เพียงพอ');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนสินค้าคงคลังในมีน้อยกว่าจำนวนที่รับทั้งหมด');", true);
                }
                txt_InReceive.Text = "";
                RefreshData();
                ClearGridview_Receive();
                return; 
            }

            string Inv_ItemID = ItemControl2.ItemID;
            string PackBig_ID = ItemControl2.PackID;

            DataTable dtSmallPack= new DataTable();

            dtSmallPack.Columns.Add("PackSmall_ID", System.Type.GetType("System.Int32"));
            dtSmallPack.Columns.Add("PackDis_Qty", System.Type.GetType("System.Int32"));
            dtSmallPack.Columns.Add("PackReceive_Qty", System.Type.GetType("System.Int32"));

            int i = 0;

            foreach (GridViewRow item in gvPackList.Rows)
            {
                HiddenField hdSmallPackID = (HiddenField)item.FindControl("hdPackID"); 
                TextBox txt_ReceiveQty_row = (TextBox)item.FindControl("txt_ReceiveQty");

                DataRow dr = dtSmallPack.NewRow();

                dr["PackSmall_ID"] = Convert.ToInt32(hdSmallPackID.Value == "" ? "0" : hdSmallPackID.Value);
                dr["PackDis_Qty"] = Convert.ToInt32(txt_ReceiveQty_row.Text == "" ? "0" : txt_ReceiveQty_row.Text);
                dr["PackReceive_Qty"] = Convert.ToInt32(txt_ReceiveQty_row.Text == "" ? "0" : txt_ReceiveQty_row.Text);

                dtSmallPack.Rows.Add(dr);

                i++;
            }

            if (dtSmallPack.Rows.Count > 0)
            {
                bool result = new ItemDAO().InsertUpdate_PackDistribute(ddlStock.SelectedValue, this.UserID, this.OrgID, 1, Inv_ItemID, PackBig_ID, dtSmallPack);

                if (result)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "complete", "alert('ทำการแตก Pack เรียบร้อย');", true);
                    RefreshData();
                    txt_InReceive.Text = "";
                    ClearGridview_Receive();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่สามารถทำการแตก Pack ได้');", true);
                    return;
                }
            }
            //ShowMessageBox("Code : " + ItemControl2.ItemCode + " Name : " + ItemControl2.ItemName + " Pack : " + ItemControl2.PackName);
            //BindData();
            //gvMovement.Visible = true;
        }

        protected void btnCancelSearch_Click(object sender, EventArgs e)
        {
            ddlStock.SelectedIndex = 0;
            ItemControl2.Clear();
            lb_InOnhand.Text = "";
            txt_InReceive.Text = "";
            //gvPackList.Visible = false;
            //pagingControlReqList.Visible = false;
            pnPack.Visible = false;

            
        }

        private int _rowCount = 1;
        protected void gvPackList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TextBox txt_OnhandQty = (TextBox)e.Row.FindControl("txt_OnhandQty");
                TextBox txt_ReceiveQty = (TextBox)e.Row.FindControl("txt_ReceiveQty");
                //TextBox txt_SumQty = (TextBox)e.Row.FindControl("txt_SumQty");
                //TextBox txt_TotalQty = (TextBox)e.Row.FindControl("txt_TotalQty");
                Label lb_OnhandQty = (Label)e.Row.FindControl("lb_OnhandQty");
                Label lb_SumQty = (Label)e.Row.FindControl("lb_SumQty");
                Label lb_TotalQty = (Label)e.Row.FindControl("lb_TotalQty");
                HiddenField hdID = (HiddenField)e.Row.FindControl("hdID");
                HiddenField hdPackID = (HiddenField)e.Row.FindControl("hdPackID");
                HiddenField hdOnhandQty = (HiddenField)e.Row.FindControl("hdOnhandQty");
                HiddenField hdPackContent = (HiddenField)e.Row.FindControl("hdPackContent");

                DataRowView drv = (DataRowView)e.Row.DataItem;
                e.Row.Cells[0].Text = (_rowCount++).ToString();

                hdID.Value = drv["Inv_ItemID"].ToString();
                hdPackID.Value = drv["Pack_Id"].ToString();
                hdOnhandQty.Value = drv["OnHand_Qty"].ToString();
                hdPackContent.Value = drv["Pack_Content"].ToString();

                lb_OnhandQty.Text = (Convert.ToDecimal(drv["OnHand_Qty"].ToString() == "" ? "0" : drv["OnHand_Qty"].ToString())).ToString("#,##0");
                txt_ReceiveQty.Text = txt_InReceive.Text;
                if (txt_ReceiveQty.Text != "")
                { 
                    string rec_qty = txt_ReceiveQty.Text.Replace(",", "");
                    //txt_SumQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value)).ToString("#,###");
                    //txt_TotalQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value) + Convert.ToDecimal(hdOnhandQty.Value == "" ? "0" : hdOnhandQty.Value)).ToString("#,###");
                    lb_SumQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value)).ToString("#,###");
                    lb_TotalQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value) + Convert.ToDecimal(hdOnhandQty.Value == "" ? "0" : hdOnhandQty.Value)).ToString("#,###");
                }

                //txt_ReceiveQty.Attributes.Add("onchange", "txt_RecQtyChange('" + txt_ReceiveQty.ClientID + "', '" + txt_SumQty.ClientID + "', '" + txt_TotalQty.ClientID + "');");

                
            }
        }

        protected void btnRefreshI_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        protected void RefreshData()
        { 
            if (ddlStock.SelectedValue != "")
            {
                DataTable dt = new StockDAO().GetOnHand(ddlStock.SelectedValue, ItemControl2.ItemID, ItemControl2.PackID);

                if (dt.Rows.Count > 0)
                {
                    Decimal onhand_qty = Convert.ToDecimal(dt.Rows[0]["OnHand_Qty"].ToString() == "" ? "0" : dt.Rows[0]["OnHand_Qty"].ToString());
                    if (onhand_qty <= 0)
                    {
                         lb_InOnhand.Text = onhand_qty.ToString("#,##0");
                         txt_InReceive.Text = "";
                         txt_InReceive.Enabled = false;
                         pnPack.Visible = false;
                    }
                    else
                    {
                        lb_InOnhand.Text = onhand_qty.ToString("#,##0");
                        txt_InReceive.Enabled = true;
                        pnPack.Visible = true;
                    }
                }
                else
                {
                    //ShowMessageBox("ไม่พบข้อมูลรายการสินค้านี้ในคงคลัง");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบข้อมูลรายการสินค้านี้ในคงคลัง');", true);
                    pnPack.Visible = false;
                    txt_InReceive.Text = "";
                    txt_InReceive.Enabled = false;
                    lb_InOnhand.Text = "";
                }
            }
            else
            {
                //ShowMessageBox("กรุณาเลือกคลังสินค้า");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาเลือกคลังสินค้า');", true);
                return;
            }

            BindData();
        }

        protected void ddlStock_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStock.SelectedValue != "" && ItemControl2.ItemID != "")
            {
                DataTable dt = new StockDAO().GetOnHand(ddlStock.SelectedValue, ItemControl2.ItemID, ItemControl2.PackID);

                if (dt.Rows.Count > 0)
                {
                    Decimal onhand_qty = Convert.ToDecimal(dt.Rows[0]["OnHand_Qty"].ToString() == "" ? "0" : dt.Rows[0]["OnHand_Qty"].ToString());
                    if (onhand_qty <= 0)
                    {
                         lb_InOnhand.Text = onhand_qty.ToString("#,##0");
                         txt_InReceive.Text = "";
                         txt_InReceive.Enabled = false;
                         pnPack.Visible = false;
                    }
                    else
                    {
                        lb_InOnhand.Text = onhand_qty.ToString("#,##0");
                        txt_InReceive.Enabled = true;
                        pnPack.Visible = true;
                    }
                }
                else
                {
                    //ShowMessageBox("ไม่พบข้อมูลรายการสินค้านี้ในคงคลัง");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('ไม่พบข้อมูลรายการสินค้านี้ในคงคลัง');", true);
                    pnPack.Visible = false;
                    txt_InReceive.Text = "";
                    txt_InReceive.Enabled = false;
                    lb_InOnhand.Text = "";
                }
            }
            else if (ddlStock.SelectedValue == "")
            {
                //ShowMessageBox("กรุณาเลือกคลังสินค้า");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('กรุณาเลือกคลังสินค้า');", true);
                return;
            }


        }

        protected void txt_InReceive_TextChanged(object sender, EventArgs e)
        {
            Decimal InReceive = Convert.ToDecimal(txt_InReceive.Text.Replace(",", "") == ""?"0":txt_InReceive.Text.Replace(",", ""));
            Decimal InOnHand = Convert.ToDecimal(lb_InOnhand.Text.Replace(",", "") == "" ? "0" : lb_InOnhand.Text.Replace(",", ""));
            if (InReceive > InOnHand)
            {
                //ShowMessageBox("จำนวนที่แบ่งเกินจำนวนในคลัง");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนที่แบ่งเกินจำนวนในคลัง');", true);
                txt_InReceive.Text = "";
                txt_InReceive.Focus();
                return;
            }

            BindData();

        }

        private void BindData()
        {
            DataSet ds = new ItemDAO().GetPackListByPackBase(
                ItemControl2.ItemID, ItemControl2.PackID,ddlStock.SelectedValue, 1, 100, this.SortColumn, this.SortOrder);
            //End Nin EDIT
            pagingControlReqList.RecordCount = (int)ds.Tables[1].Rows[0][0];

            this.gvPackList.DataSource = ds.Tables[0];
            this.gvPackList.DataBind();
            //gvPackList.Visible = true;
            //pagingControlReqList.Visible = true;
            gvPackList.Visible = true;
        }

        protected void txt_ReceiveQty_textChanged(object sender, EventArgs e)
        {

            GridViewRow row = ((GridViewRow)(((TextBox)sender).Parent.Parent));
            int rowIndex = row.RowIndex;
            TextBox txt_ReceiveQty = (TextBox)sender;

            Decimal Receive = Convert.ToDecimal(txt_ReceiveQty.Text.Replace(",", "") == "" ? "0" : txt_ReceiveQty.Text.Replace(",", ""));
            Decimal InOnHand = Convert.ToDecimal(lb_InOnhand.Text.Replace(",", "") == "" ? "0" : lb_InOnhand.Text.Replace(",", ""));

            Decimal sumReceive = 0;

            Label lb_OnhandQty = (Label)row.FindControl("lb_OnhandQty");
            Label lb_SumQty = (Label)row.FindControl("lb_SumQty");
            Label lb_TotalQty = (Label)row.FindControl("lb_TotalQty");
            HiddenField hdID = (HiddenField)row.FindControl("hdID");
            HiddenField hdPackID = (HiddenField)row.FindControl("hdPackID");
            HiddenField hdOnhandQty = (HiddenField)row.FindControl("hdOnhandQty");
            HiddenField hdPackContent = (HiddenField)row.FindControl("hdPackContent");


            //if (Receive > InOnHand)
            //{
            //    ShowMessageBox("จำนวนที่แบ่งเกินจำนวนในคลัง");
            //    txt_ReceiveQty.Text = "";
            //    lb_SumQty.Text = "";
            //    lb_TotalQty.Text = "";
            //    txt_ReceiveQty.Focus();
            //    return;
            //}

            if (txt_ReceiveQty.Text != "")
            {
                foreach (GridViewRow item in gvPackList.Rows)
                {
                    TextBox txt_ReceiveQty_row = (TextBox)item.FindControl("txt_ReceiveQty");
                    sumReceive += decimal.Parse(txt_ReceiveQty_row.Text == "" ? "0" : txt_ReceiveQty_row.Text.Replace(",", ""));
                }

                if (sumReceive > InOnHand)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fail", "alert('จำนวนที่แบ่งเกินจำนวนในคลัง');", true);
                    txt_ReceiveQty.Text = "";
                    lb_SumQty.Text = "";
                    lb_TotalQty.Text = "";
                    txt_ReceiveQty.Focus();
                    return;
                }


                string rec_qty = txt_ReceiveQty.Text.Replace(",", "");
                //txt_SumQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value)).ToString("#,###");
                //txt_TotalQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value) + Convert.ToDecimal(hdOnhandQty.Value == "" ? "0" : hdOnhandQty.Value)).ToString("#,###");
                lb_SumQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value)).ToString("#,###");
                lb_TotalQty.Text = (Convert.ToDecimal(rec_qty == "" ? "0" : rec_qty) * Convert.ToDecimal(hdPackContent.Value == "" ? "0" : hdPackContent.Value) + Convert.ToDecimal(hdOnhandQty.Value == "" ? "0" : hdOnhandQty.Value)).ToString("#,###");
            }


        }

        protected void ClearGridview_Receive()
        {
            foreach (GridViewRow item in gvPackList.Rows)
            {
                TextBox txt_ReceiveQty_row = (TextBox)item.FindControl("txt_ReceiveQty");
                Label lb_SumQty_row = (Label)item.FindControl("lb_SumQty");
                Label lb_TotalQty_row = (Label)item.FindControl("lb_TotalQty");
                txt_ReceiveQty_row.Text = "";
                lb_SumQty_row.Text = "";
                lb_TotalQty_row.Text = "";

            }
        }

    }
}