using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class ReorderPointDAO
    {
        public DataSet GetReorderPoint(string stockID, string itemName, string packName, string type, string itemCode, int pageNum, int pageSize,
          string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Pack_Name", packName));
            param.Add(new SqlParameter("@Type", type));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Stock_OnHand_SelectPaging", param);
        }

        public DataTable GetReorderPoint(string stockID, string itemID, string packID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectByID2", param);
        }

        public void AddReorderPoint(string stockID, string itemID, string packID, string reorderPoint, string reorderPointCal, string maxQty, string packIDPurchase,
            string onhandQty, string onhandAmount, string unitPriceAvg, string baseUnitFlag, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));
            if(reorderPoint.Trim().Length > 0)
                param.Add(new SqlParameter("@Reorder_Point", reorderPoint));
            if(reorderPointCal.Trim().Length > 0)
                param.Add(new SqlParameter("@Reorder_PointCal", reorderPointCal));
            if(maxQty.Trim().Length > 0)
                param.Add(new SqlParameter("@Maximum_Qty", maxQty));
            if(packIDPurchase.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID_Purchase", packIDPurchase));
            if(onhandQty.Trim().Length > 0)
                param.Add(new SqlParameter("@OnHand_Qty", onhandQty));
            if(onhandAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@OnHand_Amount", onhandAmount));
            if(unitPriceAvg.Trim().Length > 0)
                param.Add(new SqlParameter("@UnitPrice_Avg", unitPriceAvg));

            param.Add(new SqlParameter("@BaseUnit_flag", baseUnitFlag));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stock_OnHand_Update2", param);
        }

        public DataTable GetReorderPointReport(string stockID, string cateID, string subCateID, string itemCode, string itemName)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Cate_ID", cateID));
            param.Add(new SqlParameter("@SubCate_ID", subCateID));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_OnHand_SelectReport", param);
        }

        #region Green

        public DataTable GetReorderPointPurchaseReport(List<SqlParameter> param)
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Stock_ReorderPointPurchase", param);
        }

        #endregion Green
    }
}