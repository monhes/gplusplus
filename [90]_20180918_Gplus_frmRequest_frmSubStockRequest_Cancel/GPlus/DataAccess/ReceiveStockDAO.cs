using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using GPlus.Stock.Commons;
using System.Diagnostics;


namespace GPlus.DataAccess
{
    public class ReceiveStockDAO : DbConnectionBase
    {
        private DatabaseHelper _dbHepler;
        public ReceiveStockDAO()
        {
            _dbHepler = new DatabaseHelper();
        }

        public DataSet GetRecieveStk(string receiveStkNo, string receiveStkDateFrom, string receiveStkDateTo
            , string poCode, string poDateFrom, string poDateTo, string recType, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Receive_Stk_No", receiveStkNo));
            param.Add(new SqlParameter("@Receive_Date_From", receiveStkDateFrom));
            param.Add(new SqlParameter("@Receive_Date_To", receiveStkDateTo));
            param.Add(new SqlParameter("@PO_Code", poCode));
            param.Add(new SqlParameter("@PO_Date_From", poDateFrom));
            param.Add(new SqlParameter("@PO_Date_To", poDateTo));
            param.Add(new SqlParameter("@Rec_Type", recType));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Receive_stk_SelectPaging", param);
        }

        public void UpdateCancelReceiveStock(string receiveStockID, string poID, string stockID, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Receive_Stk_ID", receiveStockID));
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Receive_stk_Cancel", param);
        }

        public DataSet GetReceiveStk(string poId, string receiveStockID)
        {
            Debug.WriteLine("PoId = " + poId + ", recvStkId = " + receiveStockID);

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poId));
            if(receiveStockID.Trim().Length > 0)
                param.Add(new SqlParameter("@Receive_Stk_ID", receiveStockID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Receive_Stk_SelectByID", param);
        }

        public string AddReceiveStock(string receiveStkID, string invItemID, string packID, string unitPrice, string receiveQty, string tradeDiscountPercent,
            string tradeDiscountPrice, string cashDiscountPercent, string cashDiscountPrice, string totalBeforeVat, string vat, string netAmount,
            string totalDiscount, string isGiveAway, string fromReceiveItemID)
        {
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Receive_Stk_ID", receiveStkID));
            paramItem.Add(new SqlParameter("@Inv_ItemID", invItemID));
            paramItem.Add(new SqlParameter("@procure_name", DBNull.Value));
            paramItem.Add(new SqlParameter("@specify", DBNull.Value));
            paramItem.Add(new SqlParameter("@Pack_ID", packID));
            if(unitPrice.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Unit_Price", unitPrice));
            if(receiveQty.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Recive_Quantity", receiveQty));
            if(tradeDiscountPercent.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@TradeDiscount_Percent", tradeDiscountPercent));
            if(tradeDiscountPrice.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@TradeDiscount_Price", tradeDiscountPrice));
            if(cashDiscountPercent.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@CashDiscount_Percent", cashDiscountPercent));
            if(cashDiscountPrice.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@CashDiscount_Price", cashDiscountPrice));
            if(totalBeforeVat.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Total_before_Vat", totalBeforeVat));
            if (vat.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Vat", vat));
            if (totalBeforeVat.Trim().Length > 0 && vat.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Vat_Amount", CalculateVatAmount(Convert.ToDecimal(totalBeforeVat), 
                    Convert.ToDecimal(vat) )));
            if(netAmount.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Net_Amount", netAmount));
            if(totalDiscount.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Total_Discount", totalDiscount));
            paramItem.Add(new SqlParameter("@IsGiveAway", isGiveAway));
            if(fromReceiveItemID.Trim().Length > 0)
                paramItem.Add(new SqlParameter("@From_Receive_StkItem_ID", fromReceiveItemID));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Receive_StkItem_Insert", paramItem).ToString();
        }

        public void UpdateReceiveStockReference(string oldreceiveStockID, string newReceiveStockID, string stockID, string updateBy,string invNo)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OldRef_ID", oldreceiveStockID));
            param.Add(new SqlParameter("@NewRef_ID", newReceiveStockID));
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Update_By", updateBy));
            param.Add(new SqlParameter("@Inv_no", invNo));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Receive_StkItem_UpdateReferenceID", param);
        }

        public string AddorUpdateReceiveStock(string receiveStkID, string stockID, string poID, string invoiceNo, string delivertDoc, DateTime invoiceDate, 
            string invoiceAmount, DateTime deliveryDate, string tradeDiscountType, string tradeDiscountPercent, string tradeDiscountBath, string cashDiscountType
           ,string cashDiscountPercent, string cashDiscountBath, string vatType, string vat, string vatUnitType, string totalPrice, string totalDiscount
           , string totalBeforeVat, string vatAmount, string netAmount, string createBy, string freeType)
        {
            DatabaseHelper dbHelper = new DatabaseHelper();
            List<SqlParameter> param = new List<SqlParameter>();
            if (receiveStkID[0] == '-')
            {
                HttpContext.Current.Session["newStockCode"] = GenerateReceiveCode(stockID);
                param.Add(new SqlParameter("@Receive_Stk_No", HttpContext.Current.Session["newStockCode"].ToString()));
            }
            else
                param.Add(new SqlParameter("@Receive_Stk_No", ""));

            param.Add(new SqlParameter("@Receive_Date", DateTime.Now));
            param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@Invoice_No", invoiceNo));
            param.Add(new SqlParameter("@Delivery_Doc", delivertDoc));
            if(invoiceDate > DateTime.MinValue) param.Add(new SqlParameter("@Invoice_Date", invoiceDate));
            if(invoiceAmount.Trim().Length > 0) param.Add(new SqlParameter("@Invoice_Amount", invoiceAmount));
            if(deliveryDate > DateTime.MinValue) param.Add(new SqlParameter("@Delivery_Date", deliveryDate));
            param.Add(new SqlParameter("@TradeDiscount_Type", tradeDiscountType));
            if(tradeDiscountPercent.Trim().Length > 0) param.Add(new SqlParameter("@TradeDiscount__Percent", tradeDiscountPercent));
            if(tradeDiscountBath.Trim().Length > 0) param.Add(new SqlParameter("@TradeDiscount__Bath", tradeDiscountBath));
            param.Add(new SqlParameter("@CashDiscount_Type", cashDiscountType));
            if(cashDiscountPercent.Trim().Length > 0) param.Add(new SqlParameter("@CashDiscount__Percent", cashDiscountPercent));
            if(cashDiscountBath.Trim().Length > 0) param.Add(new SqlParameter("@CashDiscount__Bath", cashDiscountBath));
            param.Add(new SqlParameter("@Vat_Type", vatType));
            if(vat.Trim().Length > 0) param.Add(new SqlParameter("@Vat", vat));
            param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
            if(totalPrice.Trim().Length > 0) param.Add(new SqlParameter("@Total_Price", totalPrice));
            if (totalDiscount.Trim().Length > 0) param.Add(new SqlParameter("@Total_Discount", totalDiscount));
            if (totalBeforeVat.Trim().Length > 0)param.Add(new SqlParameter("@Total_Before_Vat", totalBeforeVat));
            if (vatAmount.Trim().Length > 0) param.Add(new SqlParameter("@VAT_Amount", vatAmount));
            if (netAmount.Trim().Length > 0) param.Add(new SqlParameter("@Net_Amonut", netAmount));
            param.Add(new SqlParameter("@Create_By", createBy));
            param.Add(new SqlParameter("@FreeType", freeType));
            if (receiveStkID[0] == '-')
            {
                DataRow dr = dbHelper.ExecuteDataTable("sp_Inv_Receive_Stk_Insert", param).Rows[0];
                UpdateReceiveStockReference(receiveStkID, dr["Receive_Stk_ID"].ToString(), stockID, createBy, invoiceNo);
                receiveStkID = dr["Receive_Stk_ID"].ToString();   

                DataTable dt = GetReceiveStk(poID, receiveStkID).Tables[0];
                if (dt.Rows.Count > 0)
                    HttpContext.Current.Session["newStockCode"] = dt.Rows[0]["Receive_Stk_No"].ToString();
            }
            else
            {
                param.Insert(0, new SqlParameter("@Receive_Stk_ID", receiveStkID));
                dbHelper.ExecuteNonQuery("sp_Inv_Receive_Stk_Update", param);
            }
            return receiveStkID;
        }

        public DataTable GetReceiveStkItem(string poId, string receiveStockID, string invitemId,string packId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poId));
            param.Add(new SqlParameter("@Inv_ItemID", invitemId));
            param.Add(new SqlParameter("@Pack_ID", packId));
            if (receiveStockID.Trim().Length > 0)
                param.Add(new SqlParameter("@Receive_Stk_ID", receiveStockID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Receive_StkItem_SelectByID", param);
        }

        public DataTable GetItemData(string itemId, string poId, string receiveStkID, string packID)
        {
            //sp_Inv_Receive_stk_SelectItem
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Pack_ID", packID));
            param.Add(new SqlParameter("@Inv_ItemID", Int32.Parse(itemId)));
            param.Add(new SqlParameter("@PO_ID", Int32.Parse(poId)));
            if (receiveStkID.Trim().Length > 0)
                param.Add(new SqlParameter("@Receive_Stk_ID", receiveStkID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Receive_stk_SelectItem", param);
        }

        public DataTable GetItemPackage(string itemId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", Int32.Parse(itemId)));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Receive_Stk_GetItemPackage", param);
        }

        public DataTable GetLoacation(string stockID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Receive_stk_SelectAllLocation", param);
        }

        public DataTable GetItemDetailWithPackage(int itemId)
        {
            //sp_Inv_Receive_stk_SelectItemPackage
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemId));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Receive_stk_SelectItemPackage", param);
        }



        /// <summary>
        /// This method use to calculate rqty
        /// </summary>
        /// <param name="rowItems"></param>
        /// <returns></returns>
        private object CalculateReceiveQty(DataRow[] rowItems)
        {
            int rQty = 0;
            foreach (DataRow dr in rowItems)
            {
                if ((int)dr["Inv_ItemID"] == (int)dr["Lot_Item_ID"])
                {
                    rQty += string.IsNullOrEmpty(dr["Qty_Location"].ToString()) ? 0 : Convert.ToInt32(dr["Qty_Location"].ToString());
                }
            }

            return rQty;
        }

        public DataTable GetLotItem(int stkItemId, int stockId)
        {
            //sp_Inv_Stock_Lot_SelectLot
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Stock_ID", stockId));
            paramItem.Add(new SqlParameter("@Receive_StkItem_ID", stkItemId));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Lot_SelectLot", paramItem);
        }

        private void InsertLotLocation(DataRow eachLocation, int stockLotID, string createdBy)
        {
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Stock_Lot_ID", stockLotID));
            paramItem.Add(new SqlParameter("@Location_ID", Convert.ToInt32(eachLocation["Location_ID"])));
            paramItem.Add(new SqlParameter("@Qty_Location", Convert.ToInt32(eachLocation["Qty_Location"])));
            paramItem.Add(new SqlParameter("@Create_By", createdBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_Lot_Location_Insert", paramItem);
        }

        private void UpdateLotLocation(DataRow eachLocRow, int stocklotId, string updateBy)
        {
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Stock_Lot_ID", stocklotId));
            paramItem.Add(new SqlParameter("@Location_ID", Convert.ToInt32(eachLocRow["Location_ID"])));
            paramItem.Add(new SqlParameter("@Qty_Location", Convert.ToInt32(eachLocRow["Qty_Location"])));
            paramItem.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_Lot_Location_Update", paramItem);
        }

        public void DeletStockLot(int srItemId)
        {
            //sp_Inv_Stock_Lot_Delete
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Receive_StkItem_ID", srItemId));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_Lot_Delete", paramItem);
        }

        //private int InsertStockLot(string stockID, DataRow eachLot, int srItemId)
        //{
        //    List<SqlParameter> paramItem = new List<SqlParameter>();
        //    paramItem.Add(new SqlParameter("@Receive_StkItem_ID", srItemId));
        //    paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(eachLot["Inv_ItemID"])));
        //    paramItem.Add(new SqlParameter("@Stock_ID", stockID));
        //    paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(eachLot["Pack_ID"])));
        //    if (eachLot["Expire_Date"].ToString() == string.Empty || Convert.ToDateTime(eachLot["Expire_Date"].ToString()) == DateTime.MinValue)
        //    {
        //        paramItem.Add(new SqlParameter("@Expire_Date", DBNull.Value));
        //    }
        //    else
        //    {
        //        paramItem.Add(new SqlParameter("@Expire_Date", Convert.ToDateTime(eachLot["Expire_Date"].ToString())));
        //    }
        //    paramItem.Add(new SqlParameter("@Lot_No", eachLot["Lot_No"].ToString()));
        //    paramItem.Add(new SqlParameter("@Barcode_No", eachLot["Barcode_No"].ToString()));
        //    paramItem.Add(new SqlParameter("@Barcode_PrintQty", Convert.ToInt32(eachLot["Barcode_PrintQty"])));
        //    paramItem.Add(new SqlParameter("@Lot_Qty", Convert.ToInt32(eachLot["Lot_Qty"])));
        //    paramItem.Add(new SqlParameter("@Lot_Amount", ((Convert.ToDecimal(eachLot["Lot_Qty"]) - Convert.ToDecimal(eachLot["GiveAway_Unit"])) * Convert.ToDecimal(eachLot["Unit_Price"]))));
        //    paramItem.Add(new SqlParameter("@Unit_Price", Convert.ToInt32(eachLot["Unit_Price"])));
        //    if (eachLot["Item_TradeDiscount_Percent"].ToString().Trim().Length > 0)
        //        paramItem.Add(new SqlParameter("@TradeDiscount_Percent", Convert.ToDecimal(eachLot["Item_TradeDiscount_Percent"])));
        //    else
        //        paramItem.Add(new SqlParameter("@TradeDiscount_Percent", 0));
        //    if (eachLot["Item_TradeDiscount_Price"].ToString().Trim().Length > 0)
        //        paramItem.Add(new SqlParameter("@TradeDiscount_Price", Convert.ToDecimal(eachLot["Item_TradeDiscount_Price"])));
        //    else
        //        paramItem.Add(new SqlParameter("@TradeDiscount_Price", 0));
        //    if (eachLot["Item_CashDiscount_Percent"].ToString().Trim().Length > 0)
        //        paramItem.Add(new SqlParameter("@CashDiscount_Percent", Convert.ToDecimal(eachLot["Item_CashDiscount_Percent"])));
        //    else
        //        paramItem.Add(new SqlParameter("@CashDiscount_Percent", 0));
        //    if (eachLot["Item_CashDiscount_Price"].ToString().Trim().Length > 0)
        //        paramItem.Add(new SqlParameter("@CashDiscount_Price", Convert.ToDecimal(eachLot["Item_CashDiscount_Price"])));
        //    else
        //        paramItem.Add(new SqlParameter("@CashDiscount_Price", 0)); paramItem.Add(new SqlParameter("@Total_before_Vat", CalculateLotAmount(eachLot)));


        //    paramItem.Add(new SqlParameter("@Net_Amount", CalculateLotNetAmount(eachLot)));

        //    return Convert.ToInt32(new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Lot_DeleteAndInsert", paramItem).Rows[0]["Stock_Lot_ID"].ToString());
        //}

        private void UpdateStockLot(string stockID, DataRow eachLot, int srItemId, int stockLotId)
        {
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("Stock_Lot_ID", stockLotId));
            paramItem.Add(new SqlParameter("@Receive_StkItem_ID", srItemId));
            paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(eachLot["Inv_ItemID"])));
            paramItem.Add(new SqlParameter("@Stock_ID", stockID));
            paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(eachLot["Pack_ID"])));
            if (eachLot["Expire_Date"].ToString() == string.Empty || Convert.ToDateTime(eachLot["Expire_Date"].ToString()) == DateTime.MinValue)
            {
                paramItem.Add(new SqlParameter("@Expire_Date", DBNull.Value));
            }
            else
            {
                paramItem.Add(new SqlParameter("@Expire_Date", Convert.ToDateTime(eachLot["Expire_Date"].ToString())));
            }
            paramItem.Add(new SqlParameter("@Lot_No", eachLot["Lot_No"].ToString()));
            paramItem.Add(new SqlParameter("@Barcode_No", eachLot["Barcode_No"].ToString()));
            if(eachLot["Barcode_PrintQty"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Barcode_PrintQty", Convert.ToInt32(eachLot["Barcode_PrintQty"])));
            else
                paramItem.Add(new SqlParameter("@Barcode_PrintQty",DBNull.Value));
            if(eachLot["Lot_Qty"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@Lot_Qty", Convert.ToInt32(eachLot["Lot_Qty"])));
            else
                paramItem.Add(new SqlParameter("@Lot_Qty", DBNull.Value));
            paramItem.Add(new SqlParameter("@Lot_Amount", CalculateLotAmount(eachLot)));
            paramItem.Add(new SqlParameter("@Unit_Price", Convert.ToInt32(eachLot["Unit_Price"])));
            if(eachLot["Item_TradeDiscount_Percent"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@TradeDiscount_Percent", Convert.ToDecimal(eachLot["Item_TradeDiscount_Percent"])));
            else
                paramItem.Add(new SqlParameter("@TradeDiscount_Percent", 0));
            if(eachLot["Item_TradeDiscount_Price"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@TradeDiscount_Price", Convert.ToDecimal(eachLot["Item_TradeDiscount_Price"])));
            else
                paramItem.Add(new SqlParameter("@TradeDiscount_Price", 0));
            if(eachLot["Item_CashDiscount_Percent"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@CashDiscount_Percent", Convert.ToDecimal(eachLot["Item_CashDiscount_Percent"])));
            else
                paramItem.Add(new SqlParameter("@CashDiscount_Percent", 0));
            if(eachLot["Item_CashDiscount_Price"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@CashDiscount_Price", Convert.ToDecimal(eachLot["Item_CashDiscount_Price"])));
            else
                paramItem.Add(new SqlParameter("@CashDiscount_Price", 0));
            paramItem.Add(new SqlParameter("@Total_before_Vat", CalculateLotAmount(eachLot)));
            paramItem.Add(new SqlParameter("@Net_Amount", CalculateLotNetAmount(eachLot)));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_Lot_UpdateLot", paramItem);
        }


        private void UpdateReceiveQty(string poId, int itemId, string receiveId)
        {
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@PO_ID", poId));
            paramItem.Add(new SqlParameter("@Inv_ItemID", itemId));
            paramItem.Add(new SqlParameter("@Receive_Stk_ID", receiveId));

            _dbHepler.ExecuteNonQuery("sp_Inv_PO_Item_UpdateReceiveQty", paramItem);
        }

        public void AddStockMovement(string stockID, string invItemID, string packID, string movementType, string movementFlag, string qty, string amount,
            string referTransactionType, string referTransactionNo, string receiveFlagReference, string createBy )
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Stock_ID", stockID));
                param.Add(new SqlParameter("@Inv_ItemID", invItemID));
                param.Add(new SqlParameter("@Pack_ID", packID));
                param.Add(new SqlParameter("@Movement_Type", movementType));
                param.Add(new SqlParameter("@Movement_Flag", movementFlag));
                param.Add(new SqlParameter("@Qty_Movement", qty));
                if (amount.Trim().Length > 0)
                    param.Add(new SqlParameter("@Amount", amount));
                param.Add(new SqlParameter("@Reference_Transaction_Type", referTransactionType));
                param.Add(new SqlParameter("@Reference_Transaction_No", referTransactionNo));
                param.Add(new SqlParameter("@Receive_Flag_Reference", receiveFlagReference));
                param.Add(new SqlParameter("@Create_By", createBy));

                new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_MoveMent_Insert", param);
            }catch(Exception ex){
                throw ex;
            }
         
        }

        public void AddStockLotLog(string transactionType, string transactionID, string stockID, string stockLotID, string locationID, string invItemID,
            string packID, string qtyIn, string qtyOut, string amount, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Transaction_Type", transactionType));
            param.Add(new SqlParameter("@Transaction_ID", transactionID));
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Stock_Lot_ID", stockLotID));
            param.Add(new SqlParameter("@Location_ID", locationID));
            param.Add(new SqlParameter("@Inv_ItemID", invItemID));
            param.Add(new SqlParameter("@Pack_ID", packID));
            if(qtyIn.Trim().Length > 0)
                param.Add(new SqlParameter("@Qty_In", qtyIn));
            if(qtyOut.Trim().Length > 0)
                param.Add(new SqlParameter("@Qty_Out", qtyOut));
            if(amount.Trim().Length > 0)
                param.Add(new SqlParameter("@Amount", amount));
            param.Add(new SqlParameter("@Create_By", createBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_Lot_Log_Insert", param);
        }

        private decimal CalculateLotAmount(DataRow eachLot)
        {
            if (eachLot["Lot_Qty"].ToString().Trim().Length == 0) eachLot["Lot_Qty"] = 0;
            return Convert.ToInt32(eachLot["Lot_Qty"]) * Convert.ToDecimal(eachLot["Unit_Price"]); 
        }

        private decimal CalculateLotNetAmount(DataRow eachRow)
        {
            decimal de = CalculateLotAmount(eachRow);
            return de + (de * Convert.ToDecimal(eachRow["Vat"]) / 100m);
        }


        /// <summary>
        /// Calculate vat amount
        /// </summary>
        /// <param name="total"></param>
        /// <param name="vat"></param>
        /// <returns></returns>
        private decimal CalculateVatAmount(decimal total, decimal vat)
        {
            return (total * (vat / 100m));
        }
        /// <summary>
        /// This method use to generate receive code from database
        /// </summary>
        /// <returns></returns>
        public string GenerateReceiveCode(string stockId)
        {
            string code = string.Empty;
            //sp_Inv_Receive_Stk_SelectLastRow
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Stock_Id", stockId));

            code = _dbHepler.ExecuteDataTable("sp_Inv_Receive_Stk_SelectLastRow", paramItem).Rows[0]["Receive_Code"].ToString();

            int year = DateTime.Now.Year;
            if (year < 2500)
                year += 543;

            return code+year.ToString().Substring(2);
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
                    dr["Count"] = 0;
            }

            //returning grouped/counted result
            return dtGroup;
        }

        #region Nin
        public DataTable GetReceiveDailyReport(string stockID, string startDate, string endDate, string cateID, string subCateID, string itemCode, string itemName)
        {
            this.BeginParameter();

            this.Parameters.Add(Parameter("@Stock_ID", stockID));
            this.Parameters.Add(Parameter("@StartDate", startDate));
            this.Parameters.Add(Parameter("@EndDate", endDate));
            this.Parameters.Add(Parameter("@Cate_ID", cateID));
            this.Parameters.Add(Parameter("@SubCate_ID", subCateID));
            this.Parameters.Add(Parameter("@Inv_ItemCode", itemCode));
            this.Parameters.Add(Parameter("@Inv_ItemName", itemName));

            return this.ExecuteDataTable("sp_Inv_Receive_GetReceiveDaliyReport", this.Parameters);
        }

        public DataTable GetWithdrawDailyReport(string stockID, string startDate, string endDate, string cateID, string subCateID, string itemCode, string itemName)
        {
            this.BeginParameter();

            this.Parameters.Add(Parameter("@Stock_ID", stockID));
            this.Parameters.Add(Parameter("@StartDate", startDate));
            this.Parameters.Add(Parameter("@EndDate", endDate));
            this.Parameters.Add(Parameter("@Cate_ID", cateID));
            this.Parameters.Add(Parameter("@SubCate_ID", subCateID));
            this.Parameters.Add(Parameter("@Inv_ItemCode", itemCode));
            this.Parameters.Add(Parameter("@Inv_ItemName", itemName));

            return this.ExecuteDataTable("sp_Inv_Receive_GetWithdrawDailyReport", this.Parameters);
        }

        public DataTable GetReceiveAndWithdrawOtherReport(string TransType, string stockID, string startDate, string endDate, string supplierID, string cateID, string subCateID, string itemCode, string itemName)
        {
            this.BeginParameter();

            this.Parameters.Add(Parameter("@trans_type", TransType));
            this.Parameters.Add(Parameter("@Stock_ID", stockID));
            this.Parameters.Add(Parameter("@StartDate", startDate));
            this.Parameters.Add(Parameter("@EndDate", endDate));
            this.Parameters.Add(Parameter("@Supplier_ID", supplierID));
            this.Parameters.Add(Parameter("@Cate_ID", cateID));
            this.Parameters.Add(Parameter("@SubCate_ID", subCateID));
            this.Parameters.Add(Parameter("@Inv_ItemCode", itemCode));
            this.Parameters.Add(Parameter("@Inv_ItemName", itemName));

            return this.ExecuteDataTable("sp_Inv_ReceiveAndWithdrawCaseOtherReport", this.Parameters);
        }

        public void UpdateCloseStk(string poItemId, string remarkClose, string remarkStatus, string updateBy)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@POItem_ID", poItemId));
                param.Add(new SqlParameter("@DeliveryStop_Remark", remarkClose));
                param.Add(new SqlParameter("@remarkStatus", remarkStatus));
                param.Add(new SqlParameter("@Update_By", updateBy));


                dbHelper.ExecuteNonQuery("sp_Get_Inv_UpdateCloseStk", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPOItem_Close(string poItemId)
        {
            this.BeginParameter();

            this.Parameters.Add(Parameter("@POItem_ID", poItemId));

            return this.ExecuteDataTable("sp_Get_Inv_GetPOItem_Close", this.Parameters);
        }

        public DataSet GetCloseRec(string po_id, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", po_id));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Get_Inv_GetCloseRec_SelectPaging", param);
        }

        public DataTable GetPO_Close_SelectByPOID(string po_id)
        {
            this.BeginParameter();

            this.Parameters.Add(Parameter("@PO_ID", po_id));

            return this.ExecuteDataTable("sp_Get_Inv_GetPO_Close_SelectByPOID", this.Parameters);
        }

        #endregion


        #region PT



        //public DataTable GetInv_PO_Form1(int PO_ID)
        //{
        //    try
        //    {

        //        DatabaseHelper dbHelper = new DatabaseHelper();
        //        List<SqlParameter> param = new List<SqlParameter>();
        //        param.Add(new SqlParameter("@PO_ID", PO_ID));

        //        dbHelper.SQLCommandType = CommandType.StoredProcedure;

        //        return dbHelper.ExecuteDataTable("sp_Get_Inv_PO_Form1", param);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        public bool CheckItemCancel(string rcvStkItemId, string stockId)
        {

            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Receive_StkItem_ID", rcvStkItemId));
                param.Add(new SqlParameter("@Stock_ID", stockId));
                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                int i = (int)(dbHelper.ExecuteScalar("sp_Inv_Receive_StkItem_CheckCancel", param));
                if (i == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreatePackage(int rcvStkItemId, int stockId, string updateBy)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Receive_StkItem_ID", rcvStkItemId));
                param.Add(new SqlParameter("@Stock_ID", stockId));
                param.Add(new SqlParameter("@Update_By", updateBy));


                dbHelper.ExecuteNonQuery("sp_Get_Inv_Create_Package", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreatePackageSubStock(int recPayItemId, int stockId, string updateBy)
        {
            try
            {



                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@RecPay_ItemId", recPayItemId));
                param.Add(new SqlParameter("@Stock_ID", stockId));
                param.Add(new SqlParameter("@Update_By", updateBy));


                dbHelper.ExecuteNonQuery("sp_Req_Inv_Create_Package_SubStock", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetReceiveStkAllItems(string poId, string receiveStockID)
        {
            Debug.WriteLine("PoId = " + poId + ", recvStkId = " + receiveStockID);

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PO_ID", poId));
            if (receiveStockID.Trim().Length > 0)
                param.Add(new SqlParameter("@Receive_Stk_ID", receiveStockID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Receive_StkAllItem_SelectByID", param);
        }


        public bool UpdateTotalStkItem(string receiveStockID, string Inv_ItemId, string totalbeforevat, string vatamount, string netamount, string totaldiscount, string isGiveAway, string RcvQty, string PackId)
        {
            try
            {


                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Receive_Stk_ID", receiveStockID));
                param.Add(new SqlParameter("@Inv_ItemId", Inv_ItemId));
                param.Add(new SqlParameter("@Total_before_Vat", totalbeforevat));
                param.Add(new SqlParameter("@Vat_Amount", vatamount));
                param.Add(new SqlParameter("@Net_Amount", netamount));
                param.Add(new SqlParameter("@Total_Discount", totaldiscount));

                param.Add(new SqlParameter("@isGiveAway", isGiveAway));

                param.Add(new SqlParameter("@Recieve_Qty", RcvQty));

                param.Add(new SqlParameter("@Pack_Id", PackId));
                dbHelper.ExecuteNonQuery("sp_Inv_Receive_StkItem_TotalUpdate", param);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void updateLot(DataTable tMain, DataRow drItem, DataTable dataItem, string stockID, string updateBy, string stockReceiveID)
        {


            DataTable dtGroupItemId = GroupBy("Inv_ItemID", "Inv_ItemID", dataItem);

            foreach (DataRow eachItem in dtGroupItemId.Rows)
            {
                int srItemId = 0;
                DataRow[] rowItems = dataItem.Select("Inv_ItemID='" + eachItem["Inv_ItemID"] + "'");
                string currentItemID = string.Empty;
                foreach (DataRow eachStItem in rowItems)
                {

                    srItemId = Int32.Parse(drItem["Receive_StkItem_ID"].ToString());
                    //AddStockMovement(stockID, eachStItem["Inv_ItemID"].ToString(), eachStItem["Pack_ID"].ToString(),"I", "SS",
                    //     receiveQuantity.ToString(), netAmount.ToString(), "R", srItemId.ToString(), "0", updateBy);

                    DataTable dtGroupLotNo = GroupBy("Lot_ID", "Lot_ID", dataItem);
                    string currentLotId = string.Empty;
                    foreach (DataRow eachLot in dtGroupLotNo.Rows)
                    {

                        DataRow[] rowLots;
                        if (eachLot["Lot_ID"].ToString().Trim().Length > 0)
                            rowLots = dataItem.Select("Lot_ID='" + eachLot["Lot_ID"].ToString() + "'");
                        else
                            rowLots = dataItem.Select("Lot_ID IS NULL");

                        foreach (DataRow eachlotRow in rowLots)
                        {
                            if (currentLotId != eachlotRow["Lot_ID"].ToString())
                            {
                                currentLotId = eachlotRow["Lot_ID"].ToString();
                                //new lot
                                if ((bool)eachlotRow["IsNewLot"] == true)
                                {
                                    int stocklotId = InsertStockLot2(tMain, stockID, eachlotRow, srItemId);
                                    DataTable dtGroupLocation = GroupBy("Location_ID", "Location_ID", dataItem);
                                    string currentLocId = string.Empty;
                                    foreach (DataRow eachLocation in dtGroupLocation.Rows)
                                    {
                                        if (eachLocation["Location_ID"].ToString().Trim().Length == 0)
                                            eachLocation["Location_ID"] = "1";
                                        DataRow[] rowLocations = dataItem.Select("Location_ID='" + eachLocation["Location_ID"] + "' and Lot_ID='" + currentLotId + "'");
                                        foreach (DataRow eachLocRow in rowLocations)
                                        {
                                            if (currentLocId != eachLocRow["Location_ID"].ToString())
                                            {
                                                InsertLotLocation(eachLocRow, stocklotId, updateBy);
                                                AddStockLotLog("R", stockReceiveID, stockID, stocklotId.ToString(), eachLocation["Location_ID"].ToString(),
                                                eachStItem["Inv_ItemID"].ToString(), eachStItem["Pack_ID"].ToString(), eachLocRow["Qty_Location"].ToString(),
                                                "", eachLocRow["Net_Amount"].ToString(), updateBy);

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //old lot
                                    int stocklotId = Convert.ToInt32(eachlotRow["Lot_ID"]);
                                    UpdateStockLot(stockID, eachlotRow, srItemId, stocklotId);
                                    DataTable dtGroupLocation = GroupBy("Location_ID", "Location_ID", dataItem);
                                    string currentLocId = string.Empty;
                                    foreach (DataRow eachLocation in dtGroupLocation.Rows)
                                    {
                                        DataRow[] rowLocations = dataItem.Select("Location_ID='" + eachLocation["Location_ID"] + "' and Lot_ID='" + currentLotId + "'");
                                        foreach (DataRow eachLocRow in rowLocations)
                                        {
                                            if (currentLocId != eachLocRow["Location_ID"].ToString())
                                            {
                                                UpdateLotLocation(eachLocRow, stocklotId, updateBy);
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }


                }

            }



        }



        public void UpdateReceiveStkPrice(int rcvStkId, decimal totalPrice, decimal totalDis, decimal totalBVat, decimal vatAmount, decimal netAmount)
        {

            List<SqlParameter> param = new List<SqlParameter>();




            param.Add(new SqlParameter("@Receive_Stk_Id", rcvStkId));
            param.Add(new SqlParameter("@Total_Price", totalPrice));
            param.Add(new SqlParameter("@Total_Discount", totalDis));


            param.Add(new SqlParameter("@Total_Before_Vat", totalBVat));
            param.Add(new SqlParameter("@VAT_Amount", vatAmount));
            param.Add(new SqlParameter("@Net_Amount", netAmount));

            new DatabaseHelper().ExecuteNonQuery("sp_Get_Update_Stk_Price", param);
        }






        public bool CancelStkItems(int rev_stkitemId, int StockID, string updateBy, int ReturnQTY, int ReturnGiveAway, string fromProgramFlag)
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Receive_StkItem_ID", rev_stkitemId));
                param.Add(new SqlParameter("@Stock_ID", StockID));
                param.Add(new SqlParameter("@Update_By ", updateBy));
                param.Add(new SqlParameter("@Return_QTY", ReturnQTY));
                param.Add(new SqlParameter("@Return_GiveAway_QTY", ReturnGiveAway));
                param.Add(new SqlParameter("@from_program_flag", fromProgramFlag));

                string status = new DatabaseHelper().ExecuteDataTable("sp_Get_Inv_Receive_StkItem_Cancel", param).Rows[0]["IS_CANCEL"].ToString();
                return (status == "0") ? true : false;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public int InsertStockLot2(DataTable dt, string stockID, DataRow eachLot, int srItemId)
        {


            decimal totalBeforeVat = 0;// "Net_Amount";
            decimal NetAmount = 0;
            decimal tradeDisPer = 0;
            decimal tradeDisPrice = 0;
            foreach (DataRow a in dt.Rows)
            {
                if (a["Inv_ItemID"].ToString().Trim() == eachLot["Inv_ItemID"].ToString().Trim())
                {
                    NetAmount = decimal.Parse(a["Net_Amount"].ToString());
                    totalBeforeVat = decimal.Parse(a["Total_before_Vat"].ToString());

                    tradeDisPer = decimal.Parse(a["TradeDiscount_Percent"].ToString());
                    tradeDisPrice = decimal.Parse(a["TradeDiscount_Price"].ToString());
                }
            }
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Receive_StkItem_ID", srItemId));
            paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(eachLot["Inv_ItemID"])));
            paramItem.Add(new SqlParameter("@Stock_ID", stockID));
            paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(eachLot["Pack_ID"])));
            if (eachLot["Expire_Date"].ToString() == string.Empty || Convert.ToDateTime(eachLot["Expire_Date"].ToString()) == DateTime.MinValue)
            {
                paramItem.Add(new SqlParameter("@Expire_Date", DBNull.Value));
            }
            else
            {
                paramItem.Add(new SqlParameter("@Expire_Date", Convert.ToDateTime(eachLot["Expire_Date"].ToString())));
            }
            paramItem.Add(new SqlParameter("@Lot_No", eachLot["Lot_No"].ToString()));
            paramItem.Add(new SqlParameter("@Barcode_No", eachLot["Barcode_No"].ToString()));
            paramItem.Add(new SqlParameter("@Barcode_PrintQty", Convert.ToInt32(eachLot["Barcode_PrintQty"])));
            paramItem.Add(new SqlParameter("@Lot_Qty", Convert.ToInt32(eachLot["Lot_Qty"])));
            paramItem.Add(new SqlParameter("@Lot_Amount", Convert.ToDecimal(eachLot["GiveAway_Unit"]) * Convert.ToDecimal(eachLot["Unit_Price"])));
            paramItem.Add(new SqlParameter("@Unit_Price", Convert.ToInt32(eachLot["Unit_Price"])));
            if (eachLot["Item_TradeDiscount_Percent"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@TradeDiscount_Percent", Convert.ToDecimal(eachLot["Item_TradeDiscount_Percent"])));
            else
                paramItem.Add(new SqlParameter("@TradeDiscount_Percent", tradeDisPer));
            if (eachLot["Item_TradeDiscount_Price"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@TradeDiscount_Price", tradeDisPrice));
            else
                paramItem.Add(new SqlParameter("@TradeDiscount_Price", 0));
            if (eachLot["Item_CashDiscount_Percent"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@CashDiscount_Percent", Convert.ToDecimal(eachLot["Item_CashDiscount_Percent"])));
            else
                paramItem.Add(new SqlParameter("@CashDiscount_Percent", 0));
            if (eachLot["Item_CashDiscount_Price"].ToString().Trim().Length > 0)
                paramItem.Add(new SqlParameter("@CashDiscount_Price", Convert.ToDecimal(eachLot["Item_CashDiscount_Price"])));
            else
                paramItem.Add(new SqlParameter("@CashDiscount_Price", 0)); paramItem.Add(new SqlParameter("@Total_before_Vat", totalBeforeVat));


            paramItem.Add(new SqlParameter("@Net_Amount", NetAmount));

            return Convert.ToInt32(new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Lot_DeleteAndInsert", paramItem).Rows[0]["Stock_Lot_ID"].ToString());

        }

        ///////////////////////////////  20 OCT 13 BY PT  ///////////////////////////////////////////////////////////////////////////


        //  sp_Get_Inv_Receive_Stk_ByStkID
        //  sp_Get_Inv_Receive_Stk_ByStkID_GetMore

        public DataTable GetInvReceiveStkByStkIDGetMore(int poid)
        {
            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PO_ID", poid));
                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                return dbHelper.ExecuteDataTable("sp_Get_Inv_Receive_Stk_ByStkID_GetMore", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetReceiveStkFromStkByStkID(int rcv_Stk_ID)
        {
            try
            {


                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Receive_Stk_ID", rcv_Stk_ID));
                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                return dbHelper.ExecuteDataTable("sp_Get_Inv_Receive_Stk_ByStkID", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetReceiveStkByPoIDRcvStkId(int PO_ID, int rcvStkId)
        {
            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PO_ID", PO_ID));
                param.Add(new SqlParameter("@Receive_Stk_ID", rcvStkId));
                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_ByPoID_RcvStkId", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetReceiveStkFormPOByPOId(int PO_ID)
        {
            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PO_ID", PO_ID));
                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_FormPO_ByPOId", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetReceiveStkFormExistPOByPOId(int PO_ID, int stkId)
        {
            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@PO_ID", PO_ID));
                param.Add(new SqlParameter("@Receive_Stk_ID", stkId));
                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_FormExistPO_ByPOId", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //  public DataTable GetReceiveStkComplete()
        //  {
        //      try
        //      {

        //          DatabaseHelper dbHelper = new DatabaseHelper();
        //          List<SqlParameter> param = new List<SqlParameter>();

        //          dbHelper.SQLCommandType = CommandType.StoredProcedure;

        //          return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_Complete", param);


        //      }
        //      catch (Exception ex)
        //      {
        //          throw ex;
        //      }
        //  }
        ////  sp_Get_Receive_Stk_CompleteSearch


        public DataTable GetReceiveStkCompleteSearch(string pocode, string rcvStkno, DateTime? dateStartPO, DateTime? dateEndPo, DateTime? dateStartStk, DateTime? dateEndStk)
        {
            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();

                dbHelper.SQLCommandType = CommandType.StoredProcedure;



                param.Add(new SqlParameter("@PO_Code", pocode));
                param.Add(new SqlParameter("@Receive_Stk_No", rcvStkno));

                if (dateEndPo == null)
                {
                    param.Add(new SqlParameter("@Date_EndPO", new DateTime(2100, 1, 1)));
                }
                else
                {
                    DateTime D = DateTime.Parse(dateEndPo.ToString());
                    param.Add(new SqlParameter("@Date_EndPO", D.AddDays(1)));
                }


                if (dateStartPO == null)
                {
                    param.Add(new SqlParameter("@Date_StartPO", new DateTime(1900, 1, 1)));
                }
                else
                {
                    param.Add(new SqlParameter("@Date_StartPO", dateStartPO));
                }


                //////////////////////////////////

                if (dateEndStk == null)
                {
                    param.Add(new SqlParameter("@Date_EndStk", new DateTime(2100, 1, 1)));
                }
                else
                {
                    DateTime D = DateTime.Parse(dateEndStk.ToString());
                    param.Add(new SqlParameter("@Date_EndStk", D.AddDays(1)));
                }


                if (dateStartStk == null)
                {
                    param.Add(new SqlParameter("@Date_StartStk", new DateTime(1900, 1, 1)));
                }
                else
                {
                    param.Add(new SqlParameter("@Date_StartStk", dateStartStk));
                }


                return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_CompleteSearch", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataTable GetReceiveStkNotComplete()
        //{
        //    try
        //    {

        //        DatabaseHelper dbHelper = new DatabaseHelper();
        //        List<SqlParameter> param = new List<SqlParameter>();

        //        dbHelper.SQLCommandType = CommandType.StoredProcedure;



        //        return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_NotComplete", param);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataTable GetReceiveStkNotCompleteSearch(string pocode, string rcvStkno, DateTime? dateStartPO, DateTime? dateEndPo, DateTime? dateStartStk, DateTime? dateEndStk)
        {
            try
            {

                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();

                dbHelper.SQLCommandType = CommandType.StoredProcedure;



                param.Add(new SqlParameter("@PO_Code", pocode));
                param.Add(new SqlParameter("@Receive_Stk_No", rcvStkno));

                if (dateEndPo == null)
                {
                    param.Add(new SqlParameter("@Date_EndPO", new DateTime(2100, 1, 1)));
                }
                else
                {
                    // param.Add(new SqlParameter("@Date_EndPO", dateEndPo));

                    DateTime D = DateTime.Parse(dateEndPo.ToString());
                    param.Add(new SqlParameter("@Date_EndPO", D.AddDays(1)));
                }


                if (dateStartPO == null)
                {
                    param.Add(new SqlParameter("@Date_StartPO", new DateTime(1900, 1, 1)));
                }
                else
                {
                    param.Add(new SqlParameter("@Date_StartPO", dateStartPO));
                }


                //////////////////////////////////

                if (dateEndStk == null)
                {
                    param.Add(new SqlParameter("@Date_EndStk", new DateTime(2100, 1, 1)));
                }
                else
                {
                    DateTime D = DateTime.Parse(dateEndStk.ToString());
                    param.Add(new SqlParameter("@Date_EndStk", D.AddDays(1)));
                }


                if (dateStartStk == null)
                {
                    param.Add(new SqlParameter("@Date_StartStk", new DateTime(1900, 1, 1)));
                }
                else
                {
                    param.Add(new SqlParameter("@Date_StartStk", dateStartStk));
                }





                return dbHelper.ExecuteDataTable("sp_Get_Receive_Stk_NotCompleteSearch", param);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable GetToReceiveStock(int stockId, DateTime recvDate, int poId, string invNo, string devDoc, DateTime? invDate, decimal invAmount,
            DateTime? devDate, string tradeDisType, decimal tradeDisPer, decimal tradeDisPrice, string vatType, decimal vat, string vatUnitType,
            decimal totalPrice, decimal totalDis, decimal totalBVat, decimal vatAmount, decimal netAmount, string status, DateTime createDate, string createBy,
            DateTime updateDate, string updateBy, string freeType)
        {

            try
            {


                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();


                param.Add(new SqlParameter("@Stock_ID", stockId));
                param.Add(new SqlParameter("@PO_ID", poId));

                param.Add(new SqlParameter("@Invoice_No", invNo));
                param.Add(new SqlParameter("@Delivery_Doc", devDoc));


                if (invDate == null)
                {
                    param.Add(new SqlParameter("@Invoice_Date", DBNull.Value));
                }
                else
                {
                    param.Add(new SqlParameter("@Invoice_Date", invDate));
                }

                param.Add(new SqlParameter("@Invoice_Amount", invAmount));

                if (devDate == null)
                {
                    param.Add(new SqlParameter("@Delivery_Date", DBNull.Value));
                }
                else
                {
                    param.Add(new SqlParameter("@Delivery_Date", devDate));
                }


                param.Add(new SqlParameter("@TradeDiscount_Type", tradeDisType));
                param.Add(new SqlParameter("@TradeDiscount__Percent", tradeDisPer));
                param.Add(new SqlParameter("@TradeDiscount__Bath", tradeDisPrice));
                param.Add(new SqlParameter("@Vat_Type", vatType));
                param.Add(new SqlParameter("@Vat", vat));
                param.Add(new SqlParameter("@VatUnit_Type", vatUnitType));
                param.Add(new SqlParameter("@Total_Price", totalPrice));
                param.Add(new SqlParameter("@Total_Discount", totalDis));
                param.Add(new SqlParameter("@Total_Before_Vat", totalBVat));
                param.Add(new SqlParameter("@VAT_Amount", vatAmount));
                param.Add(new SqlParameter("@Net_Amonut", netAmount));
                param.Add(new SqlParameter("@Status", status));
                param.Add(new SqlParameter("@Create_Date", createDate));
                param.Add(new SqlParameter("@Create_By", createBy));
                param.Add(new SqlParameter("@Update_Date", DBNull.Value));

                param.Add(new SqlParameter("@Update_By", DBNull.Value));
                param.Add(new SqlParameter("@FreeType", freeType));

                dbHelper.SQLCommandType = CommandType.StoredProcedure;


                return dbHelper.ExecuteDataTable("sp_Get_To_Receive_Stock", param); //.Rows[0]["Receive_Stk_No"].ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public int GetToReceiveStkItemStk(string rcvStkNo, int invItemId, string procureName, string specify, int packId, decimal unitPrice,
            decimal recvQty, decimal tradeDisPer, decimal tradeDisPrice, decimal totalBVat, decimal vat, decimal vatAmount, decimal netAmount,
            decimal totalDis, string isGiveAway, string status, decimal giveAwayUnit, int fromRcvStkId, int packIdSplit, string cancelFlag)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Receive_Stk_No", rcvStkNo));
                param.Add(new SqlParameter("@Inv_ItemID", invItemId));
                param.Add(new SqlParameter("@procure_name", DBNull.Value));

                param.Add(new SqlParameter("@Pack_ID", packId));
                param.Add(new SqlParameter("@Unit_Price", unitPrice));
                param.Add(new SqlParameter("@Recive_Quantity", recvQty));
                param.Add(new SqlParameter("@TradeDiscount_Percent", tradeDisPer));
                param.Add(new SqlParameter("@TradeDiscount_Price", tradeDisPrice));


                param.Add(new SqlParameter("@specify", DBNull.Value));
                param.Add(new SqlParameter("@Total_before_Vat", totalBVat));
                param.Add(new SqlParameter("@Vat", vat));
                param.Add(new SqlParameter("@Vat_Amount", vatAmount));
                param.Add(new SqlParameter("@Net_Amount", netAmount));
                param.Add(new SqlParameter("@Total_Discount", totalDis));
                param.Add(new SqlParameter("@IsGiveAway", isGiveAway));
                param.Add(new SqlParameter("@Status", "1"));
                param.Add(new SqlParameter("@GiveAway_Unit", giveAwayUnit));
                param.Add(new SqlParameter("@From_Receive_StkItem_ID", DBNull.Value));
                param.Add(new SqlParameter("@Pack_Id_Split", DBNull.Value));
                param.Add(new SqlParameter("@Cancel_flag", cancelFlag));


                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                return Util.ToInt(dbHelper.ExecuteDataTable("sp_Get_To_Receive_StockItem", param).Rows[0]["Receive_StkItem_ID"].ToString());




            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public bool GetStkUpdatePoItem(decimal rcv_Qty, int poItemId)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Receive_Quantity", rcv_Qty));
                param.Add(new SqlParameter("@POItem_ID", poItemId));

                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                dbHelper.ExecuteNonQuery("sp_Get_Update_PoItem", param);

                return true;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void GetInvItemPackUpdateAvgCost(int inv_itemid, int inv_itemPackid)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", inv_itemid));
            param.Add(new SqlParameter("@Pack_Id", inv_itemPackid));



            new DatabaseHelper().ExecuteNonQuery("sp_Inv_ItemPack_UpdateAvgCost", param);
        }

        public bool GetToStockOnHand(int itemId, int packId, int StkId, decimal QtyIn, string updateby, decimal netAmount)
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper();
                List<SqlParameter> param = new List<SqlParameter>();



                param.Add(new SqlParameter("@Inv_ItemID", itemId));
                param.Add(new SqlParameter("@Pack_ID", packId));
                param.Add(new SqlParameter("@Stock_ID", StkId));
                param.Add(new SqlParameter("@Qty_In", QtyIn));
                param.Add(new SqlParameter("@Update_By", updateby));
                param.Add(new SqlParameter("@Net_Amount", netAmount));

                dbHelper.SQLCommandType = CommandType.StoredProcedure;

                dbHelper.ExecuteNonQuery("sp_Get_To_Stock_OnHand", param);

                return true;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // sp_Get_Inv_Receive_StkOnHand_ByStkID

        public DataTable GetInvReceiveStkOnHandByStkID(int rcvStkId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Receive_Stk_ID", rcvStkId));

                return new DatabaseHelper().ExecuteDataTable("sp_Get_Inv_Receive_StkOnHand_ByStkID", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetInvStockLotByStkItemID(int rcvStkItemId, int packId, int itemId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();


                param.Add(new SqlParameter("@Pack_ID", packId));
                param.Add(new SqlParameter("@Inv_ItemID", itemId));
                param.Add(new SqlParameter("@Receive_StkItem_ID", rcvStkItemId));

                return new DatabaseHelper().ExecuteDataTable("sp_Get_Inv_Stock_Lot_ByStkItemID", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReqGetToStockOnHand(int stkId, int packId, int invItemid, decimal qtyIn, int payId, string updateBy)
        {
            try
            {


                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Stock_ID", stkId));
                param.Add(new SqlParameter("@Inv_ItemID", invItemid));
                param.Add(new SqlParameter("@Pack_ID", packId));
                param.Add(new SqlParameter("@Qty_In", qtyIn));
                param.Add(new SqlParameter("@Update_By", updateBy));
                param.Add(new SqlParameter("@Pay_Id", payId));

                new DatabaseHelper().ExecuteNonQuery("sp_Req_Get_To_Stock_OnHand", param);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public bool ReqUpdateStockMoveMent(int stkId, int invItemId, int packId, string movementType, decimal qty, int pay_id,
       string refTranType, string refTranNo, string createBy, int refTranId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Stock_ID", stkId));
                param.Add(new SqlParameter("@Inv_ItemID", invItemId));
                param.Add(new SqlParameter("@Pack_ID", packId));
                param.Add(new SqlParameter("@Movement_Type", movementType));
                param.Add(new SqlParameter("@Movement_Flag", DBNull.Value));
                param.Add(new SqlParameter("@Qty_Movement", qty));
                param.Add(new SqlParameter("@Pay_Id", pay_id));

                param.Add(new SqlParameter("@Reference_Transaction_Type", refTranType));
                param.Add(new SqlParameter("@Reference_Transaction_No", refTranNo));

                param.Add(new SqlParameter("@Reference_Transaction_ID", refTranId));

                param.Add(new SqlParameter("@Receive_Flag_Reference", DBNull.Value));
                param.Add(new SqlParameter("@Create_By", createBy));


                // dbHelper.SQLCommandType = CommandType.StoredProcedure;

                new DatabaseHelper().ExecuteNonQuery("sp_Req_Inv_Stock_MoveMent_Insert", param);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool GetUpdateStockMoveMent(int stkId, int invItemId, int packId, string movementType, decimal qty, decimal Amount,
            string refTranType, string refTranNo, string createBy, int refTranId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Stock_ID", stkId));
                param.Add(new SqlParameter("@Inv_ItemID", invItemId));
                param.Add(new SqlParameter("@Pack_ID", packId));
                param.Add(new SqlParameter("@Movement_Type", movementType));
                param.Add(new SqlParameter("@Movement_Flag", DBNull.Value));
                param.Add(new SqlParameter("@Qty_Movement", qty));
                param.Add(new SqlParameter("@Amount", Amount));

                param.Add(new SqlParameter("@Reference_Transaction_Type", refTranType));
                param.Add(new SqlParameter("@Reference_Transaction_No", refTranNo));

                param.Add(new SqlParameter("@Reference_Transaction_ID", refTranId));

                param.Add(new SqlParameter("@Receive_Flag_Reference", DBNull.Value));
                param.Add(new SqlParameter("@Create_By", createBy));

                new DatabaseHelper().ExecuteNonQuery("sp_Get_Inv_Stock_MoveMent_Insert", param);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool GetUpdateReceiveStkStatus(int rcvStkId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Receive_Stk_ID", rcvStkId));

                new DatabaseHelper().ExecuteNonQuery("sp_Get_Update_ReceiveStk_Status", param);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetInvLotLocationSelectByStkLotID(int stkLotID)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Stock_Lot_ID", stkLotID));

                return new DatabaseHelper().ExecuteDataTable("sp_Get_Inv_Lot_Location_SelectByStkLotID", param);
                // return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool GetUpdatePOIsReceiveComplete(int poId)
        {
            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@PO_ID", poId));

                new DatabaseHelper().ExecuteNonQuery("sp_Get_UpdatePO_IsReceiveComplete", param);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public bool GetInvStockLotLocationInsert(string stkLotId, int locId, int stkId, int rcvStkId, int QtyLocation, string createBy)
        {

            try
            {


                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Stock_Lot_ID", stkLotId));
                param.Add(new SqlParameter("@Loc_ID", locId));
                param.Add(new SqlParameter("@Qty_Location", QtyLocation));
                param.Add(new SqlParameter("@Create_By", createBy));

                param.Add(new SqlParameter("@Stock_ID", stkId));

                param.Add(new SqlParameter("@Receive_Stk_ID", rcvStkId));





                new DatabaseHelper().ExecuteNonQuery("sp_Get_Inv_Stock_LotLocation_Insert", param);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public string GetInvStockLotInsert(int stkItemId, int stkId, string lotNo, string barNo, decimal barPrint, decimal lotQty, DateTime? expDate, int locId,
            decimal qtyLoc, string createBy)
        {

            try
            {

                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Receive_StkItem_ID", stkItemId));
                param.Add(new SqlParameter("@Stock_ID", stkId));
                param.Add(new SqlParameter("@Lot_No", (lotNo == null) ? "" : lotNo));
                param.Add(new SqlParameter("@Bar_No", (barNo == null) ? "" : barNo));
                param.Add(new SqlParameter("@Bar_PrintQty", barPrint));
                param.Add(new SqlParameter("@Lot_Qty", lotQty));

                if (expDate == null)
                {
                    param.Add(new SqlParameter("@ExpDate", DBNull.Value));
                }
                else
                {
                    param.Add(new SqlParameter("@ExpDate", expDate));
                }

                param.Add(new SqlParameter("@Loc_ID", locId));
                param.Add(new SqlParameter("@Qty_Location", qtyLoc));
                param.Add(new SqlParameter("@Create_By", createBy));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Get_Inv_Stock_Lot_Insert", param);
                return dt.Rows[0]["Stock_Lot_ID"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        public bool GetInvStockOnHandChkBaseFlag(int stkID, int invItemID, int packId)
        {
            try
            {



                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Inv_ItemId", invItemID));
                param.Add(new SqlParameter("@Stock_ID", stkID));
                param.Add(new SqlParameter("@Pack_ID", packId));

                string s = new DatabaseHelper().ExecuteDataTable("sp_Get_Inv_StockOnHand_Chk_BaseFlag", param).Rows[0]["BaseUnit_flag"].ToString().Trim();

                return (s == "1") ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // sp_Req_Inv_LocationID_Default



        public int ReqInvLocationIDDefault(int stkId)
        {

            try
            {

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@StockID", stkId));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Req_Inv_LocationID_Default", param);
                return Util.ToInt(dt.Rows[0]["LocationID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string ReqInvStockLotInsert(int recPayItemID, int reqId, int invItemid, int packId, int payId, int stkId, string lotNo, string barNo, decimal barPrint, decimal lotQty, DateTime? expDate,
      decimal qtyLoc, string createBy)
        {

            try
            {

                List<SqlParameter> param = new List<SqlParameter>();



                param.Add(new SqlParameter("@Inv_ItemID", invItemid));
                param.Add(new SqlParameter("@Pack_Id", packId));


                param.Add(new SqlParameter("@Request_Id", reqId));
                param.Add(new SqlParameter("@Pay_Id ", payId));
                param.Add(new SqlParameter("@Stock_ID", stkId));
                param.Add(new SqlParameter("@Lot_No", (lotNo == null) ? "" : lotNo));
                param.Add(new SqlParameter("@Bar_No", (barNo == null) ? "" : barNo));
                param.Add(new SqlParameter("@Bar_PrintQty", barPrint));
                param.Add(new SqlParameter("@Lot_Qty", lotQty));

                if (expDate == null)
                {
                    param.Add(new SqlParameter("@ExpDate", DBNull.Value));
                }
                else
                {
                    param.Add(new SqlParameter("@ExpDate", expDate));
                }




                param.Add(new SqlParameter("@RecPay_ItemID", recPayItemID));


                param.Add(new SqlParameter("@Qty_Location", qtyLoc));
                param.Add(new SqlParameter("@Create_By", createBy));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Req_Inv_Stock_Lot_Insert", param);
                return dt.Rows[0]["Stock_Lot_ID"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        #endregion
    }

 

}