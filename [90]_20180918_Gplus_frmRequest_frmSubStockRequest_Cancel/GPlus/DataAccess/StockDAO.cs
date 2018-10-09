using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace GPlus.DataAccess
{
    public class StockDAO
    {
        public DataSet GetLevelStk(string level, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@LevelStk_No", level));
            param.Add(new SqlParameter("@LevekStk_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_LevelStk_SelectPaging", param);
        }

        public DataTable GetLevelStk(string levelStkID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@LevelStk_ID", levelStkID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_LevelStk_SelectByID", param);
        }

        public string AddLevelStk(string levelNo, string description, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@LevelStk_No", levelNo));
            param.Add(new SqlParameter("@LevelStk_Desc", description));
            param.Add(new SqlParameter("@LevelStk_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_LevelStk_Insert", param).ToString();
        }

        public void UpdateLevelStk(string levelID,string levelNo, string description, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@LevelStk_ID", levelID));
            param.Add(new SqlParameter("@LevelStk_No", levelNo));
            param.Add(new SqlParameter("@LevelStk_Desc", description));
            param.Add(new SqlParameter("@LevelStk_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_LevelStk_Update", param);
        }



        public DataSet GetStock(string stockCode, string stockName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_Code", stockCode));
            param.Add(new SqlParameter("@Stock_Name", stockName));
            param.Add(new SqlParameter("@Stock_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_SelectPaging", param);
        }

        public DataTable GetStock(string stockID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_SelectByID", param);
        }

        public string AddStock(string stockCode, string stockName, string stockType, string levelStkID, string stockCodeReq, string reqLevelStkID, 
            string baseUnitFlag, string mustApproveFlag, string status, string createBy, string TempStk_Flag)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_Code", stockCode));
            param.Add(new SqlParameter("@Stock_Name", stockName));
            param.Add(new SqlParameter("@Stock_Type", stockType));
            if(levelStkID.Trim().Length > 0)
                param.Add(new SqlParameter("@LevelStk_Id", levelStkID));
            param.Add(new SqlParameter("@Stock_CodeReq", stockCodeReq));
            if(reqLevelStkID.Trim().Length > 0)
                param.Add(new SqlParameter("@LevelStk_IdReq", reqLevelStkID));
            param.Add(new SqlParameter("@BaseUnit_flag", baseUnitFlag));
            param.Add(new SqlParameter("@MustApprove_Flag", mustApproveFlag));
            param.Add(new SqlParameter("@Stock_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));
            if (TempStk_Flag.Trim().Length > 0)
                param.Add(new SqlParameter("@TempStk_Flag", TempStk_Flag));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Stock_Insert", param).ToString();
        }

        public string UpdateStock(string stockID, string stockCode, string stockName, string stockType, string levelStkID, string stockCodeReq,
            string reqLevelStkID, string baseUnitFlag, string mustApproveFlag, string status, string updateBy, string TempStk_Flag)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Stock_Code", stockCode));
            param.Add(new SqlParameter("@Stock_Name", stockName));
            param.Add(new SqlParameter("@Stock_Type", stockType));
            if (levelStkID.Trim().Length > 0)
                param.Add(new SqlParameter("@LevelStk_Id", levelStkID));
            param.Add(new SqlParameter("@Stock_CodeReq", stockCodeReq));
            if (reqLevelStkID.Trim().Length > 0)
                param.Add(new SqlParameter("@LevelStk_IdReq", reqLevelStkID));
            param.Add(new SqlParameter("@BaseUnit_flag", baseUnitFlag));
            param.Add(new SqlParameter("@MustApprove_Flag", mustApproveFlag));
            param.Add(new SqlParameter("@Stock_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));
            if (TempStk_Flag.Trim().Length > 0)
                param.Add(new SqlParameter("@TempStk_Flag", TempStk_Flag));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Stock_Update", param).ToString();
        }

#region Nin 19072013

        public DataSet GetStocOnHandkReport(string stockID, string cateID, string subCateID, string itemCode, string itemName, string status, string onHand)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@Asset_onHand", onHand));

            //return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectReport_test", param);
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_Report", param);
        }

        public DataSet GetStocOnHandkCloseStock(string stockID, string src_trans_startDate, string src_trans_endDate, string status, int pageNum, int pageSize,
                  string sortField, string sortOrder)
        {

            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Item_Stock_Id", stockID));
            param.Add(new SqlParameter("@Trans_StartDate", src_trans_startDate));
            param.Add(new SqlParameter("@Trans_EndDate", src_trans_endDate));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_Close_Stock", param);
        }
        public DataTable GetCloseStock(string cmdQuery)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@cmdQuery", cmdQuery));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_Close_Stock_SelectByID", param);
        }

        public DataTable GetCloseStockNew(string stockID, string status)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Item_Stock_Id", stockID));
            param.Add(new SqlParameter("@Asset_Status", status));
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_Close_Stock_SelectByStockID", param);
        }

        public string AddCloseStock(string stockID, string Tran_Month, string Tran_Year, string status, string User_Id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_Id", stockID));
            param.Add(new SqlParameter("@Transaction_Month", Tran_Month));
            param.Add(new SqlParameter("@Transaction_Year", Tran_Year));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@User_Id", User_Id));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Stock_OnHand_Close_Stock_Insert", param).ToString();
        }

        public string CloseStockChangeStatus(string stockID, string Tran_Month, string Tran_Year, string status, string User_Id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_Id", stockID));
            param.Add(new SqlParameter("@Transaction_Month", Tran_Month));
            param.Add(new SqlParameter("@Transaction_Year", Tran_Year));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@User_Id", User_Id));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Stock_OnHand_Close_Stock_ChangeStatus", param).ToString();
        }
        public DataSet StockOnHandMoving(string stockID, string search_startDate, string search_endDate, string itemID, string PackID, int pageNum, int pageSize,
                  string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Item_Stock_Id", stockID));
            param.Add(new SqlParameter("@Transaction_Start", search_startDate));
            param.Add(new SqlParameter("@Transaction_End", search_endDate));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Inv_Pack_ID", PackID));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_Moving_SelectPaging", param);
        }
        public DataSet GetStocOnHandkRemaining(string itemCode, string itemSearchDesc, string itemUnit, string stockSelected, string status, int pageNum, int pageSize,
                  string sortField, string sortOrder)
        {
            String stockItem = "";
            try
            {
                stockItem = stockSelected.Substring(0, stockSelected.LastIndexOf(","));
            }
            catch
            {
                stockItem = stockSelected;
            }


            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_Item_Search_desc", itemSearchDesc));
            param.Add(new SqlParameter("@Inv_Item_Pack_desc", itemUnit));
            param.Add(new SqlParameter("@Inv_Item_Stock_Id", stockItem));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            //return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectReport_test", param);
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_Remaining", param);
        }
        public DataSet GetStockLocation(string stockID, string LocationCode, string LocationName, string status, int pageNum, int pageSize,
                   string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Item_Stock_Id", stockID));
            param.Add(new SqlParameter("@Inv_Location_Code", LocationCode));
            param.Add(new SqlParameter("@Inv_Location_Name", LocationName));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_Location_SelectPaging", param);
        }

        public DataTable GetStockLocation(string levelStkID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Location_Id", levelStkID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Location_SelectByID", param);
        }

        public string AddStockLocation(string stockID, string LocationCode, string LocationName, string status, string createBy, string defaultFlag)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Stock_ID", stockID));
            param.Add(new SqlParameter("@Inv_Location_Code", LocationCode));
            param.Add(new SqlParameter("@Inv_Location_Name", LocationName));
            param.Add(new SqlParameter("@Location_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));
            param.Add(new SqlParameter("@Default_Flag", defaultFlag));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Stock_Location_Insert", param).ToString();
        }

        public void UpdateStockLocation(string stockID, string LocationCode, string LocationName, string status, string updateBy, string defaultFlag)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_Location_Id", stockID));
            param.Add(new SqlParameter("@Inv_Location_Code", LocationCode));
            param.Add(new SqlParameter("@Inv_Location_Name", LocationName));
            param.Add(new SqlParameter("@Inv_Location_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));
            param.Add(new SqlParameter("@Default_Flag", defaultFlag));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_Location_Update", param);
        }
        public DataTable GetStockAccount(string userID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@User_ID", userID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Account_ByUserID", param);
        }

        public DataSet GetStockFromAccount(string userID, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@User_ID", userID));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_StockFromAccount_SelectPaging", param);
        }
        public DataSet GetStocCardReport(string stockID, string cateID, string subCateID, string itemID, string PackID, string Transaction_Start, string Transaction_End)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", PackID));
            param.Add(new SqlParameter("@Transaction_Start", Transaction_Start));
            param.Add(new SqlParameter("@Transaction_End", Transaction_End));

            //return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectReport_test", param);
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_StockCard_Report", param);
        }

        public DataSet GetStocCardReportMonth(string stockID, string cateID, string subCateID, string itemID, string PackID, string Transaction_Start, string Transaction_End)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", PackID));
            param.Add(new SqlParameter("@Transaction_Start", Transaction_Start));
            param.Add(new SqlParameter("@Transaction_End", Transaction_End));

            //return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectReport_test", param);
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_StockCard_Month_Report", param);
        }


        public string GetCloseStock(string stockID, string Month_End_SUM)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Month_End_SUM", Month_End_SUM));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_StockCard_SelectCloseStock", param).ToString();
        }

        public void DeleteTempExcel(string stockID)
        {
            //sp_Inv_Stock_Lot_Delete
            List<SqlParameter> paramItem = new List<SqlParameter>();
            paramItem.Add(new SqlParameter("@Stock_ID", stockID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_tempExcel_Delete", paramItem);
        }

        public void DeleteTempExcelItem(string stockID, string itemID, string PackID)
        {
            //sp_Inv_Stock_Lot_Delete
            List<SqlParameter> paramItem = new List<SqlParameter>();

            if (stockID != "")
            {
                paramItem.Add(new SqlParameter("@Stock_ID", Convert.ToInt32(stockID)));
            }
            else
            {
                paramItem.Add(new SqlParameter("@Stock_ID", Convert.ToInt32("0")));
            }

            if (itemID != "")
            {
                paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(itemID)));
            }
            else
            {
                paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32("0")));
            }

            if (PackID != "")
            {
                paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(PackID)));
            }
            else
            {
                paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32("0")));
            }
            new DatabaseHelper().ExecuteNonQuery("sp_Inv_tempExcel_DeleteItem", paramItem);
        }

        public DataSet SetStockSumExcel(string stockID, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_SetStock_SumExcel_SelectPaging", param);
        }

        public string SetStockInsertUpdate(int stockID, int userID, string transactionDate, int orgID, int status)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@User_ID", userID));
            param.Add(new SqlParameter("@Transaction_Date", transactionDate));
            param.Add(new SqlParameter("@OrgStruc_Id", orgID));
            param.Add(new SqlParameter("@Status", status));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_SetStock_InsertUpdate", param).ToString();
        }

        public DataSet SetStockSearch(string stockID, string trans_type, string trans_no, string trans_date_start, string trans_date_stop
                                     , string tran_sub_1, string tran_sub_2, string tran_sub_3, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Trans_Type", trans_type));
            param.Add(new SqlParameter("@Trans_No", trans_no));
            param.Add(new SqlParameter("@Trans_Date_Start", trans_date_start));
            param.Add(new SqlParameter("@Trans_Date_Stop", trans_date_stop));
            param.Add(new SqlParameter("@Trans_Sub_Other1", tran_sub_1));
            param.Add(new SqlParameter("@Trans_Sub_Other2", tran_sub_2));
            param.Add(new SqlParameter("@Trans_Sub_Other3", tran_sub_3));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_SetStockSearch_SelectPaging", param);
        }

        public DataTable GetTransHead(string transHeadNo)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TransHead_ID", transHeadNo));
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Get_TransHead", param);
        }

        public DataSet GetTransDetail(string transHeadNo, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TransHead_ID", transHeadNo));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_Get_TransDetail", param);
        }

        public DataTable SetStockCheckOnhand(string itemID, string PackID, string stockID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", PackID));
            param.Add(new SqlParameter("@Stock_ID", stockID));
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_SetStock_Check_item_OnHand", param);
        }

        public bool insertDatatoTemp(DataTable dt, string stockID, string supplierBarcode)
        {
            string result = "";
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //ตรวจสอบว่าถ้า Qty_Location ไม่เท่ากับ 0 หรือ null จึงค่อยทำการ Insert
                    if (!(dt.Rows[i]["Qty_Location"].ToString() == "0" || dt.Rows[i]["Qty_Location"].ToString() == "" || dt.Rows[i]["Qty_Location"].ToString() == null))
                    {
                        List<SqlParameter> paramItem = new List<SqlParameter>();
                        paramItem.Add(new SqlParameter("@Stock_ID", stockID));
                        paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(dt.Rows[i]["Inv_ItemID"].ToString())));
                        paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(dt.Rows[i]["Pack_Id"].ToString())));
                        paramItem.Add(new SqlParameter("@Location_Id", Convert.ToInt32(dt.Rows[i]["Location_ID"].ToString())));
                        paramItem.Add(new SqlParameter("@Lot_No", dt.Rows[i]["Lot_No"].ToString()));
                        paramItem.Add(new SqlParameter("@Qty_Location", Convert.ToInt32(dt.Rows[i]["Qty_Location"].ToString())));
                        paramItem.Add(new SqlParameter("@Barcode_From_Supplier", supplierBarcode));
                        paramItem.Add(new SqlParameter("@Barcode_No", dt.Rows[i]["Barcode_No"].ToString()));
                        paramItem.Add(new SqlParameter("@Expire_Date", dt.Rows[i]["Expire_Date"].ToString()));
                        paramItem.Add(new SqlParameter("@Barcode_PrintQty", Convert.ToInt16(dt.Rows[i]["Barcode_PrintQty"].ToString())));
                        result = new DatabaseHelper().ExecuteScalar("sp_Inv_SetStock_Insert_Item_ToTemp", paramItem).ToString();

                        if (result == "0")
                        {
                            return false;
                        }

                    }
                }
            }

            return true;

        }

        public DataSet SetStockAddAdjust(string stockID, int All, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@All", All));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_SetStock_Search_AdjustStock", param);
        }

        public bool insertDataAdjustToTemp(DataTable dt, string stockID)
        {
            string result = "";
            int cnt_sql = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    List<SqlParameter> paramItem = new List<SqlParameter>();
                    paramItem.Add(new SqlParameter("@Stock_ID", stockID));
                    paramItem.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(dt.Rows[i]["Inv_ItemID"].ToString())));
                    paramItem.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(dt.Rows[i]["Pack_Id"].ToString())));
                    paramItem.Add(new SqlParameter("@Location_Id", Convert.ToInt32(1)));
                    paramItem.Add(new SqlParameter("@Lot_No", "1"));
                    paramItem.Add(new SqlParameter("@Qty_Location", Convert.ToInt32(dt.Rows[i]["Cnt_Qty"].ToString())));
                    paramItem.Add(new SqlParameter("@Barcode_From_Supplier", DBNull.Value));
                    paramItem.Add(new SqlParameter("@Barcode_No", DBNull.Value));
                    paramItem.Add(new SqlParameter("@Expire_Date", DBNull.Value));
                    paramItem.Add(new SqlParameter("@Barcode_PrintQty", Convert.ToInt16(0)));
                    paramItem.Add(new SqlParameter("@Remark", (dt.Rows[i]["Reason"].ToString() == "" ? null : dt.Rows[i]["Reason"].ToString())));
                    result = new DatabaseHelper().ExecuteScalar("sp_Inv_SetStock_Adjust_Insert_ToTemp", paramItem).ToString();

                    if (result == "0")
                    {
                        return false;
                    }
                    else if (result == "1")
                    {
                        cnt_sql++;
                    }


                } // End For

                if (cnt_sql != dt.Rows.Count)
                {
                    return false;
                }
            }

            return true;

        }

        public string SetStockAdjustInsertUpdate(int stockID, int userID, string transactionDate, int orgID, int status, string Transaction_Sub_Other)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@User_ID", userID));
            param.Add(new SqlParameter("@Transaction_Date", transactionDate));
            param.Add(new SqlParameter("@OrgStruc_Id", orgID));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Transaction_Sub_Other", Transaction_Sub_Other));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_SetStock_Adjust_InsertUpdate", param).ToString();
        }

        public DataTable GetOnHand(string stockID, string itemID, string packID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_Select", param);
        }


        public DataTable GetPayReportDivision(string stockID, string cateID,string Transaction_Start)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            param.Add(new SqlParameter("@Month_End_Sum", Transaction_Start));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_PayReportDivision", param);
        }

        public DataSet GetSummaryStockEvent(List<SqlParameter> param)
        {
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_GetSummaryStockEventReport", param);
        }

        public DataSet GetStockEvent(List<SqlParameter> param)
        {
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_GetStockEventReport", param);
        }


#endregion 

        #region Green 19/08/2013
        public DataSet GetWithdrawGoods(List<SqlParameter> param)
        {
            /*
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Month_start", monthStart));
            param.Add(new SqlParameter("@Month_end", monthEnd));
            param.Add(new SqlParameter("@Year_start", yearStart));
            param.Add(new SqlParameter("@Year_end", yearEnd));
            */
            return new DatabaseHelper().ExecuteDataSet("sp_Inv_WithdrawGoods_Report2", param);
        }

        public DataTable GetDefaultFlagByStockID(string stockId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@stock_id", stockId));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Location_GetDefaultFlag", param);
        }
        #endregion

        #region Tee 9/10/2010

        public DataTable GetStockOnHandSelectByID(string stockID, string itemID, string packID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectByStockItem", param);
        }
        #endregion



        #region PT
        // sp_Rep_Inv_GetAllCatagory

        public DataTable RepInvGetAllCatagory()
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();


                return new DatabaseHelper().ExecuteDataTable("sp_Rep_Inv_GetAllCatagory", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public DataTable RepGetAllStock()
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();


                return new DatabaseHelper().ExecuteDataTable("sp_Rep_Inv_GetAllStock", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        public DataTable RepGetSubCateByCateID(int cateId)
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Cate_ID", cateId));

                return new DatabaseHelper().ExecuteDataTable("sp_Req_Inv_Subcate_SelectByCateID", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion

        #region Nin 24/01/2014

        public DataSet StockInvEditListing(string stockID, string search_startDate, string itemID, string PackID, string CateID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_Id", stockID));
            param.Add(new SqlParameter("@Transaction_Start", search_startDate));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", PackID));
            param.Add(new SqlParameter("@Cate_ID", CateID));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_Inv_Edit_Listing", param);
        }

        public DataSet GetStockGoodsUnMovingReport(string stockID, string cateID, string subCateID, string itemCode, string itemName, string searchType, string dtStart, string dtStop, string dateFrequency, string numFrequency, string movementType, string movementCnt)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@SearchType", searchType));
            param.Add(new SqlParameter("@Start_Date", dtStart));
            param.Add(new SqlParameter("@Stop_Date", dtStop));
            param.Add(new SqlParameter("@DateFrequency", dateFrequency));
            param.Add(new SqlParameter("@numFrequency", numFrequency));
            param.Add(new SqlParameter("@MovementType", movementType));
            param.Add(new SqlParameter("@MovementCnt", movementCnt));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_UnMoving", param);
        }

        public DataSet GetInOutStkReason(string reason_desc,string InOutStk_type,string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Reason_Description", reason_desc));
            param.Add(new SqlParameter("@InOutStk_Type", InOutStk_type));
            param.Add(new SqlParameter("@InOutStk_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_InOutStk_Reason_SelectPaging", param);
        }

        public DataTable GetInOutStkReasonByID(string reasonID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Reason_ID", reasonID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_InOutStk_Reason_SelectByID", param);
        }

        public string AddInOutStkReason(string reasonDesc, string InOutStk_Type, string isCal_AvgCost, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Reason_Description", reasonDesc));
            param.Add(new SqlParameter("@InOutStk_Type", InOutStk_Type));
            param.Add(new SqlParameter("@IsCal_AvgCost", isCal_AvgCost));
            param.Add(new SqlParameter("@InOutStk_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_InOutStkReason_Insert", param).ToString();
        }

        public string UpdateInOutStkReason(string reason_id,string reasonDesc, string InOutStk_Type, string isCal_AvgCost, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Reason_ID", reason_id));
            param.Add(new SqlParameter("@Reason_Description", reasonDesc));
            param.Add(new SqlParameter("@InOutStk_Type", InOutStk_Type));
            param.Add(new SqlParameter("@IsCal_AvgCost", isCal_AvgCost));
            param.Add(new SqlParameter("@InOutStk_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_InOutStkReason_Update", param).ToString();
        }

        #endregion

        #region mew 03/04/2014

        public DataTable GetStock()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_Select2");
        }

        #endregion mew
    }
}