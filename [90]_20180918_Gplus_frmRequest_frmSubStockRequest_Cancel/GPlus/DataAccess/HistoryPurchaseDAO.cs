using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using GPlus.DataAccess;
using System.Data.SqlClient;
using System.Diagnostics;
namespace GPlus.DataAccess
{
    public class HistoryPurchaseDAO : DataAccessBase
    {
        public DataSet GetHistoryPurchase(string itemCode, string itemName, string supplierName, string supplierCount, string backMonth, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Supplier_Name", supplierName));
            param.Add(new SqlParameter("@Supplier_Count", supplierCount));
            param.Add(new SqlParameter("@Back_Month", backMonth));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Gpluz_HistoryPurchase_SelectPaging", param);
        }

        public DataSet GetSupplierName( string supplierName, int pageNum, int pageSize , string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@SupplierName", supplierName));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_HistoryPurchase_SupplierName", param);
        }


        public DataTable GetHistoryPurchase(string historyPurchaseID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@HistoryPurchase_ID", historyPurchaseID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_HistoryPurchase_SelectByID", param);
        }


        public string AddHistoryPurchase(string itemID, string packID, string supplierID, DateTime purchaseDate, string purchasePriceUnit, 
            string lPurTradeDisPercent, string lPurTradeDisAmount, string lPurPremiumQty, string lPurPremiumPackID, string proposePriceUnit, DateTime proposeDate
            ,string proposeTradeDiscountPercent, string prorposeTradeDiscountAmount, string proposePremiumQty, string proposePremiumPackID,string poID,string VatUnitType , string status,
            string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));
            if (supplierID.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_ID", supplierID));
            if (purchaseDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Purchase_Date", purchaseDate));

            if(purchasePriceUnit.Trim().Length > 0)
                param.Add(new SqlParameter("@Purchase_Price_Unit",purchasePriceUnit));
            if(lPurTradeDisPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_TradeDiscount_Percent", lPurTradeDisPercent));
            if(lPurTradeDisAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_TradeDiscount_Amount", lPurTradeDisAmount));
            if(lPurPremiumQty.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_Premium_Qty", lPurPremiumQty));
            if(lPurPremiumPackID.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_Premium_Pack_ID", lPurPremiumPackID));
            if(proposePriceUnit.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_Price_Unit", proposePriceUnit));
            if(proposeDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Propose_Date", proposeDate));
            if(proposeTradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_TradeDiscount_Percent", proposeTradeDiscountPercent));
            if(prorposeTradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_TradeDiscount_Amount", prorposeTradeDiscountAmount));
            if(proposePremiumQty.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_Premium_Qty", proposePremiumQty));
            if(proposePremiumPackID.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_Premium_Pack_ID", proposePremiumPackID));
            if (poID.Trim().Length > 0)
                param.Add(new SqlParameter("@PO_ID", poID));
            param.Add(new SqlParameter("@VatUnit_Type", VatUnitType));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_HistoryPurchase_Insert", param).ToString();
        }

        public void UpdateHistoryPurchase(string historyPurchaseID, string itemID, string packID,  string supplierName, DateTime purchaseDate, string purchasePriceUnit,
            string lPurTradeDisPercent, string lPurTradeDisAmount, string lPurPremiumQty, string lPurPremiumPackID, string proposePriceUnit, DateTime proposeDate
            , string proposeTradeDiscountPercent, string prorposeTradeDiscountAmount, string proposePremiumQty, string proposePremiumPackID, string VatUnitType, string status,
            string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@HistoryPurchase_ID", historyPurchaseID));
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));
            
            if (supplierName.Trim().Length > 0)
                param.Add(new SqlParameter("@Supplier_Name", supplierName));
            if (purchaseDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Purchase_Date", purchaseDate));
            if (purchasePriceUnit.Trim().Length > 0)
                param.Add(new SqlParameter("@Purchase_Price_Unit", purchasePriceUnit));
            if (lPurTradeDisPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_TradeDiscount_Percent", lPurTradeDisPercent));
            if (lPurTradeDisAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_TradeDiscount_Amount", lPurTradeDisAmount));
            if (lPurPremiumQty.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_Premium_Qty", lPurPremiumQty));
            if (lPurPremiumPackID.Trim().Length > 0)
                param.Add(new SqlParameter("@LPur_Premium_Pack_ID", lPurPremiumPackID));
            if (proposePriceUnit.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_Price_Unit", proposePriceUnit));
            if (proposeDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Propose_Date", proposeDate));
            if (proposeTradeDiscountPercent.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_TradeDiscount_Percent", proposeTradeDiscountPercent));
            if (prorposeTradeDiscountAmount.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_TradeDiscount_Amount", prorposeTradeDiscountAmount));
            if (VatUnitType.Trim().Length > 0)
                param.Add(new SqlParameter("@VatUnit_Type", VatUnitType));
            if (proposePremiumQty.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_Premium_Qty", proposePremiumQty));
            if (proposePremiumPackID.Trim().Length > 0)
                param.Add(new SqlParameter("@Propose_Premium_Pack_ID", proposePremiumPackID));
           
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_HistoryPurchase_Update", param).ToString();
        }

    }

}