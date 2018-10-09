using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GPlus.Stock.Commons;

namespace GPlus.Stock
{
    public partial class pop_SetStockAddItem : Pagebase
    {
        private string stock_id = "";
        
        private SetStockLotUserControl _c;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //this.Session["DataList_SetStk"] = null;
                //Session["Data_SetStk"] = null;

                //Session["IS_CLOSE_SetStk"] = null;

                //this.Session["Counter_SetStk"] = 0;

                //this.BindData();
                Session["IS_CLOSE_SetStk"] = null;
                this.Session["Counter_SetStk"] = 0;
                ClearSessionSetStk();
                lotPanel.Visible = false;
                panelButton.Visible = false;
                //BindData();
            }
            else
            {

            }
            stock_id = Request["stockID"];
        }

        private void ClearSessionSetStk()
        {
            Session["Data_SetStk"] = null;
            Session["DataList_SetStk"] = null;
            Session["BackupData_SetStk"] = null;
            Session["Inv_ItemCode_SetStk"] = null;
            Session["PackID_SetStk"] = null;
            Session["AvgCost_SetStk"] = null;
        }

        private void BindData()
        {
            //DataTable dt = GetDataFromSession(Request["ItemId"]);
            DataTable dt = GetDataFromSession(suControl1.Data.ItemID.ToString());
            //DataTable dt = GetDataFromSession("1");

            //Get primary item
            DataRow dr = null;
            if (dt.Select("Inv_ItemID='" + suControl1.Data.ItemID.ToString() + "'").Length > 0)
            {
                dr = dt.Select("Inv_ItemID='" + suControl1.Data.ItemID.ToString() + "'").First();
            }
            
            //DataRow dr = null;
            //if (dt.Select("Inv_ItemID= 1").Length > 0)
            //{
            //    dr = dt.Select("Inv_ItemID=1").First();
            //}


            //Set parent
            int countItem = 1;
            string selectedItemId = suControl1.Data.ItemID.ToString();
            //string selectedItemId = "1";
            foreach (DataRow eachRow in dt.Rows)
            {
                if (eachRow["Lot_Item_ID"].ToString() != selectedItemId)
                {
                    selectedItemId = eachRow["Lot_Item_ID"].ToString();
                    eachRow["rowCount"] = countItem++;
                    eachRow["isParent"] = "Y";
                }
                else
                {
                    eachRow["isParent"] = "N";
                }
            }


            if (dr != null)
            {

                Session["Data_SetStk"] = dt;

                //If it has privious information
                //if (!IsNewReceieve(dr))
                //{
                DataTable drs = GroupBy("Lot_Item_ID", "Lot_Item_ID", dt);
                // Find give away
                if (drs.Rows.Count > 1)
                {
                    //It has give away more than 1 types
                }
                else
                {
                    //It has give away only 1 type
                    //int numberOfGiveAway = Convert.ToInt32(dr["Total_Unit"]) - Convert.ToInt32(dr["Unit_Quantity"]);

                    DataTable dt2 = GroupBy("Lot_ID", "Lot_ID", dt);
                   
                    dlLot.DataSource = dt2;
                    dlLot.DataBind();
                }

            }
        }


        private DataTable GetDataFromSession(string itemId)
        {
            DataTable dt = null;
            if (Session["DataList_SetStk"] == null)
            {
                this.Session["DataList_SetStk"] = new Dictionary<string, DataTable>();

            }

            Dictionary<string, DataTable> dic = ((Dictionary<string, DataTable>)(Session["DataList_SetStk"]));
            if (dic.Keys.Contains(itemId))
            {
                dt = dic[itemId];
            }
            else
            {
                dt = new DataAccess.ItemDAO().GetItemSetStock(suControl1.Data.ItemID.ToString(), suControl1.PackID);
                //dt = new DataAccess.ItemDAO().GetItemSetStock("1", "1");
                dt.Columns.Add("IsNewLot", typeof(bool));
                dt.Columns.Add("rownumber", typeof(int));
                dt.AcceptChanges();
                //update first lot id
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Lot_ID"].ToString().Trim().Length == 0)
                    {
                        dr["Lot_ID"] = 1;
                        dr["Lot_Item_Code"] = dr["Inv_ItemCode"];
                        dr["Lot_Item_Name"] = dr["Inv_ItemName"];
                        dr["Qty_Location"] = 0;
                        dr["IsNewLot"] = true;
                    }
                }
            }
            Session["BackupData"] = dt.Copy();
            return dt;
        }

        private bool IsNewReceieve(DataRow dr)
        {
            if (string.IsNullOrEmpty(dr["Lot_Item_ID"].ToString()))
            {
                return true;
            }
            return false;
        }

        protected void BtnAddLotClick(object sender, EventArgs e)
        {
            //try
            //{
                if (suControl1.Data.ItemID.ToString() == "" || suControl1.Data.ItemID.ToString() == null || suControl1.Data.ItemID.ToString() == "0")
                {
                    ShowMessageBox("กรุณาเลือกรายการสินค้า");
                    return;
                }
                else
                {
                    DataTable dt = new DataAccess.StockDAO().SetStockCheckOnhand(suControl1.Data.ItemID.ToString(), suControl1.PackID,stock_id);
                    if (dt.Rows.Count > 0)
                    {
                        ShowMessageBox("สินค้ารายการนี้มีอยู่ใน Stock แล้ว กรุณาทำการ Adjust Stock");
                        return;
                    }
                    if (lotPanel.Visible == false)
                    {
                        //ทำการ disable ปุ่ม search และ dropdown หน่วย สินค้า หลังจากที่ กด AddLot แล้ว
                        string set = "";
                        suControl1.setDisableSearch = set;
                        Session["Inv_ItemCode_SetStk"] = suControl1.ItemCode;
                        Session["PackID_SetStk"] = suControl1.PackID;
                        lotPanel.Visible = true;
                        panelButton.Visible = true;
                        this.BindData();
                    }
                    else
                    {
                        AddNewLot();
                        Session["Counter_SetStk"] = (int)Session["Counter_SetStk"] + 1;
                    }
                }
            //}
            //catch {
            //    ShowMessageBox("กรุณาเลือกรายการสินค้า");
            //}
            
        }

        private void AddNewLot()
        {
            CollectDataRow();
            if ((Session["Data_SetStk"] != null))
            {
                DataTable dt = Session["Data_SetStk"] as DataTable;
                DataRow dr = dt.NewRow();
                DataTable dtlot = GroupBy("Lot_ID", "Lot_ID", dt);
                if (dtlot.Rows[dtlot.Rows.Count - 1]["Lot_ID"].ToString().Trim().Length == 0)
                    dtlot.Rows[dtlot.Rows.Count - 1]["Lot_ID"] = 0;
                int lotId = Convert.ToInt32(dtlot.Rows[dtlot.Rows.Count - 1]["Lot_ID"]) + 1;
                dr.ItemArray = dt.Rows[0].ItemArray;
                dr["Lot_ID"] = lotId;
                dr["Lot_No"] = string.Empty;
                dr["Lot_Qty"] = 0;
                dr["Barcode_PrintQty"] = 0;
                dr["rownumber"] = 1;
                dr["IsNewLot"] = true;
                //dr.AcceptChanges();
                dt.Rows.Add(dr);

                dtlot = GroupBy("Lot_ID", "Lot_ID", dt);
                dlLot.DataSource = dtlot;
                dlLot.DataBind();

                //_c = (StockReceiverLotUserControl)this.LoadControl("StockReceiverLotUserControl.ascx");
                //_c.LotId = lotId;
                //_c.Lot = dt;
                //this.LotContent.Controls.Add(_c);
            }
        }

        public DataTable GroupBy(string i_sGroupByColumn, string i_sAggregateColumn, DataTable i_dSourceTable)
        {

            DataView dv = new DataView(i_dSourceTable);

            //getting distinct values for group column
            DataTable dtGroup = dv.ToTable(true, new string[] { i_sGroupByColumn });

            //adding column for the row count
            dtGroup.Columns.Add("Count", typeof(int));

            //looping thru distinct values for the group, counting
            foreach (DataRow dr in dtGroup.Rows)
            {
                if (dr[i_sGroupByColumn].ToString().Trim().Length > 0)
                    dr["Count"] = i_dSourceTable.Compute("Count(" + i_sAggregateColumn + ")", i_sGroupByColumn + " = '" + dr[i_sGroupByColumn] + "'");
                else
                    dr["Count"] = 1;
            }

            //returning grouped/counted result
            return dtGroup;
        }


        protected void BtnSaveClick(object sender, EventArgs e)
        {
            //if (int.Parse(hdTotalUnit.Value) == 0)
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "err1", "alert('รายการนี้รับสินค้าครบแล้ว');", true);
            //    return;
            //}

            //if (int.Parse(txtTotalUnit.Text) > int.Parse(hdTotalUnit.Value))
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "err1", "alert('จำนวนสินค้าที่รับต้องไม่เกิน " + hdTotalUnit.Value + "');", true);
            //    return;
            //}
            if (suControl1.Data.ItemID.ToString() == "" || suControl1.Data.ItemID.ToString() == null || suControl1.Data.ItemID.ToString() == "0")
            {
                ShowMessageBox("กรุณาเลือกรายการสินค้า");
                return;
            }
            else
            {
                DataTable dt = new DataAccess.StockDAO().SetStockCheckOnhand(suControl1.Data.ItemID.ToString(), suControl1.PackID, stock_id);
                if (dt.Rows.Count > 0)
                {
                    ShowMessageBox("สินค้ารายการนี้มีอยู่ใน Stock แล้ว กรุณาทำการ Adjust Stock");
                    return;
                }

                if (CollectDataRow(true))
                {
                    SaveDataInfo();
                    DataTable dt2 = Session["Data_SetStk"] as DataTable;

                    if (dt2.Rows.Count > 0)
                    {
                        //new DataAccess.StockDAO().DeleteTempExcel(stock_id);
                        bool result = new DataAccess.StockDAO().insertDatatoTemp(dt2, stock_id, suControl1.SupplierBarcode.ToString());
                        if (!result)
                        {
                            ShowMessageBox("ไม่สามารถนำข้อมูลเข้าระบบได้");
                            return;
                        }

                    }

                    //new DataAccess.ReceiveStockDAO().InsertOrUpdate(dt2, Request["rid"], Request["PoId"], Request["stockID"], this.UserID, this.PercentVat);

                    Session["IS_CLOSE_SetStk"] = true;
                    string scriptStr = "if(window.opener)window.opener.document.getElementById('ContentPlaceHolder1_btnRefreshItem').click(); window.close();";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "closing", scriptStr, true);
                }
         
            }

        }


        private void SaveDataInfo()
        {
            Dictionary<string, DataTable> dic = (this.Session["DataList_SetStk"] as Dictionary<string, DataTable>);
            if (!dic.Keys.Contains(suControl1.Data.ItemID.ToString()))
                {
                    dic.Add(suControl1.Data.ItemID.ToString(), (Session["Data_SetStk"] as DataTable));
                }
                else
                {
                    dic[suControl1.Data.ItemID.ToString()] = (Session["Data_SetStk"] as DataTable);
                }

        }

        

        private bool CollectDataRow(bool showError = false)
        {
            DataTable dt = Session["Data_SetStk"] as DataTable;

            if (dt == null) return false;
            //lucLotMain.SubmitData();
            int sumItemAll = 0;
            string lotName = "";
               for (int i = 0; i < dlLot.Items.Count; i++)
                {
                    SetStockLotUserControl slu = (SetStockLotUserControl)dlLot.Items[i].FindControl("lucLot");
                    slu.SubmitData();
                    if (showError)
                        if (lotName.IndexOf("|" + slu.LotName + ",") > -1)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "err1", "alert('คุณระบุ Lot No ซ้ำกัน');", true);
                            return false;
                        }

                    lotName += "|" + slu.LotName + ",";
                    if (showError)
                        if (slu.LocationItemCount != slu.LotItemCount)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "err", "alert('คุณระบุจำนวนไม่ถูกต้อง');", true);
                            return false;
                        }
                    sumItemAll += slu.LotItemCount;
                }

                if (showError)
                    if (suControl1.ItemCount != sumItemAll)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "err2", "alert('คุณระบุจำนวนไม่ถูกต้อง');", true);
                        return false;
                    }
       
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Lot_Item_ID"].ToString().Trim().Length == 0)
                {
                    dt.Rows[i]["Lot_Item_ID"] = dt.Rows[i]["Inv_ItemID"];
                }
                if (string.IsNullOrEmpty(dt.Rows[i]["IsNewLot"].ToString()))
                {
                    dt.Rows[i]["IsNewLot"] = false;
                }
            }

            Session["Data_SetStk"] = dt;
            return true;
        }

        protected void dlLot_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                _c = (SetStockLotUserControl)e.Item.FindControl("lucLot");
                _c.LotId = Convert.ToInt32(drv["Lot_ID"]);
                _c.Lot = (DataTable)Session["Data_SetStk"];

                if (e.Item.ItemIndex == 0)
                    _c.DisableDelButton();
            }
        }



    }
}