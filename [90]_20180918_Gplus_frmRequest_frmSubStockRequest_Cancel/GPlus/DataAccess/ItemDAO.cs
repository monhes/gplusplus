using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class ItemDAO : DbConnectionBase
    {
        public DataSet GetItem(string itemCode, string itemName, string cateID, string as400, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Item_Code", itemCode));
            param.Add(new SqlParameter("@Item_Name", itemName));
            param.Add(new SqlParameter("@Cate_ID", cateID));
            param.Add(new SqlParameter("@Inv_AS400", as400));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Item_SelectPaging", param);
        }

        public DataSet GetItemSearch(string itemCode, string itemName, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
           
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemSearch_SelectPaging", param);
        }

        public DataSet GetItemFormPrint(string itemCode, string itemName, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Item_Code", itemCode));
            param.Add(new SqlParameter("@Item_Name", itemName));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Item_SelectFormPrintPaging", param);
        }

        public DataTable GetItem(string itemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Item_SelectByID", param);
        }

        public DataTable GetItemSearch(string itemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_ItemSearch_SelectByID", param);
        }

        public DataTable GetItemByName(string name)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Item_CodeName", name));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Item_SelectByName", param);
        }

        public DataTable GetItemAndPackByName(string name)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Item_CodeName", name));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Item_SelectPackByName", param);
        }

        public DataTable GetItemID(string itemCode)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Item_SelectByItemCode", param);
        }

        public DataTable GetItemPackID(string itemCode)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_ItemPack_SelectByItemCode", param);
        }

        public string AddItem(string itemCode, string itemName, string attribute, string cateID, string typeID, string subCateID, string formID,
            string as400, string orderStatus, string status, string createBy,string keyCode)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Inv_Attrbute", attribute));
            if(cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if(typeID.Trim().Length > 0)
                param.Add(new SqlParameter("@Type_ID", typeID));
            if(subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            if(formID.Trim().Length > 0)
                param.Add(new SqlParameter("@Form_Id", formID));
            param.Add(new SqlParameter("@Inv_AS400", as400));
            param.Add(new SqlParameter("@Order_Detail", orderStatus));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));
            param.Add(new SqlParameter("@KeyCode", keyCode));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Item_Insert", param).ToString();
        }

        public string UpdateItem(string itemID, string itemCode, string itemName, string attribute, string cateID, string typeID, string subCateID, string formID,
            string as400, string orderStatus, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Inv_Attrbute", attribute));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (typeID.Trim().Length > 0)
                param.Add(new SqlParameter("@Type_ID", typeID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            if (formID.Trim().Length > 0)
                param.Add(new SqlParameter("@Form_Id", formID));
            param.Add(new SqlParameter("@Inv_AS400", as400));
            param.Add(new SqlParameter("@Order_Detail", orderStatus));
            param.Add(new SqlParameter("@Asset_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Item_Update", param).ToString();
        }

        public void UpdateItem(string itemID, string baseUnitID, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@BaseUnit_Pack_ID", baseUnitID));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Item_UpdateBaseUnit", param);
        }


        public DataSet GetItemPack(string itemCode, string itemName, string packName, string status, int pageNum, int pageSize,
           string sortField, string sortOrder, string isBase = "", string isPackBig = "", string item_status = "")
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Description", packName));
            param.Add(new SqlParameter("@ItemPack_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));
            if (isBase.Trim().Length > 0)
                param.Add(new SqlParameter("@IsBaseUnit", isBase));
            if (isPackBig.Trim().Length > 0)
                param.Add(new SqlParameter("@IsPackBig", isPackBig));
            if (item_status.Trim().Length > 0)
                param.Add(new SqlParameter("@ItemStatus", item_status));


            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemPack_SelectPaging", param);
        }

        public DataTable GetItemPack(string itemID, string packID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_ID", packID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_ItemPack_SelectByPackID", param);
        }

        public DataTable GetItemPack(string invItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", invItemID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_ItemPack_SelectByID", param);
        }

        public void UpdateItemPack(string invItemID, string packID, string packSeq, string packContent, string packIDBase, string description,
            string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", invItemID));
            param.Add(new SqlParameter("@Pack_Id", packID));
            param.Add(new SqlParameter("@Pack_Seq", packSeq));
            param.Add(new SqlParameter("@Pack_Content", packContent));
            param.Add(new SqlParameter("@Pack_Id_Base", packIDBase));
            param.Add(new SqlParameter("@Description", description));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_ItemPack_Update", param);
        }

        public void UpdateItemPackPrice(string itemID, string packID, string avgCost, DateTime avgConstDate, string sellingPrice,
            string barcode, string status, string updateBy)//string pricePurchaseLatest, DateTime latestPurchaseDate, string latestSupplierID,
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", itemID));
            param.Add(new SqlParameter("@Pack_Id", packID));
            if(avgCost.Trim().Length > 0)
                param.Add(new SqlParameter("@Avg_Cost", avgCost));
            if(avgConstDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Avg_Cost_Date", avgConstDate));
            if(sellingPrice.Trim().Length > 0)
                param.Add(new SqlParameter("@Selling_Price", sellingPrice));
            //if(pricePurchaseLatest.Trim().Length > 0)
            //    param.Add(new SqlParameter("@Price_Purchase_Latest", pricePurchaseLatest));
            //if(latestPurchaseDate > DateTime.MinValue)
            //    param.Add(new SqlParameter("@Latest_Purchase_Date", latestPurchaseDate));
            //if(latestSupplierID.Trim().Length > 0)
            //    param.Add(new SqlParameter("@Latest_Supplier_ID", latestSupplierID));
            param.Add(new SqlParameter("@Barcode_From_Supplier", barcode));
            param.Add(new SqlParameter("@ItemPack_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_ItemPack_UpdatePrice", param);
        }

        public void DeleteItemPack(string invItemID, string packID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", invItemID));
            param.Add(new SqlParameter("@Pack_Id", packID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_ItemPack_Delete", param);
        }

        public DataSet GetItemPackandPrice(string itemCode, string itemName, string description, string packID, string supplierCode, string supplierName,
            int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Description", description));
            param.Add(new SqlParameter("@Pack_ID", packID));
            param.Add(new SqlParameter("@Supplier_Code", supplierCode));
            param.Add(new SqlParameter("@Supplier_Name", supplierName));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemPack_SelectPricePaging", param);
        }


        public DataSet GetItemPackReorderPoint(string itemCode, string itemName,  string packID, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Pack_ID", packID));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemPack_SelectReorderPointPaging", param);
        }

        public DataSet GetItemPackReorderPoint2
        (
            string itemCode
            , string itemName
            , string cateID
            , string startReorderPoint
            , string endReorderPoint
            , int pageNum
            , int pageSize
        )
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Cate_ID", cateID));
            param.Add(new SqlParameter("@StartReorderPoint", startReorderPoint));
            param.Add(new SqlParameter("@EndReorderPoint", endReorderPoint));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemPack_SelectReorderPointPaging2", param);
        }

        #region Nin 19072013

        public DataSet GetItemPack2(string itemCode, string itemName, string packName, string status, int pageNum, int pageSize,
        string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Description", packName));
            param.Add(new SqlParameter("@ItemPack_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemPack2_SelectPaging", param);
        }

        public DataSet GetItemSearchMaterial(string materialID, string SubmaterialID, string itemCode, string itemName, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_materialID", materialID));
            param.Add(new SqlParameter("@Inv_SubmaterialID", SubmaterialID));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));

            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemSearchMaterial_SelectPaging", param);
        }

        public DataTable GetItemSetStock(string ItemID, string PackID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", ItemID));
            param.Add(new SqlParameter("@Inv_PackID", PackID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_SetStock_GetItem_SelectByID", param);
        }


        public string InsertLog(string account_id, string menu_name, string button_Name, string itemid, string packid,
            string fieldname, string before_value, string after_value)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_ID", account_id));
            param.Add(new SqlParameter("@Menu_Name", menu_name));
            param.Add(new SqlParameter("@Button_Name", button_Name));
            param.Add(new SqlParameter("@Inv_ItemID", itemid));
            param.Add(new SqlParameter("@Pack_ID", packid));
            param.Add(new SqlParameter("@Field_Name", fieldname));
            param.Add(new SqlParameter("@Before_Value", before_value));
            param.Add(new SqlParameter("@After_Value", after_value));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Insert_Inv_log", param).ToString();
        }

        public DataSet GetPackListByPackBase(string itemID, string packID, string stockID,
                                           int pageNum, int pageSize, string sortField, string sortOrder)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Inv_ItemID", itemID));
            this.Parameters.Add(Parameter("@Inv_PackID", packID));
            this.Parameters.Add(Parameter("@Stock_ID", stockID));
            this.Parameters.Add(Parameter("@PageNum", pageNum));
            this.Parameters.Add(Parameter("@PageSize", pageSize));
            this.Parameters.Add(Parameter("@SortField", sortField));
            this.Parameters.Add(Parameter("@SortOrder", sortOrder));

            DataSet ds = this.ExecuteDataSet("sp_Inv_ItemPack_ByPackBase_SelectPaging", this.Parameters);
            return ds;
        }

        public bool InsertUpdate_PackDistribute(string StockID, string UserID, string OrgStrucID, int status, string Inv_ItemID, string PackBig_ID, DataTable dtSmallPack)
        {

            bool chk = false;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();

            int chk_Data_All = dtSmallPack.Rows.Count;
            int chk_Data = 0;

            try
            {
                this.BeginParameter();


                foreach (DataRow drRow in dtSmallPack.Rows)
                {
                    this.BeginParameter();

                    this.Parameters.Add(Parameter("@Stock_ID", Convert.ToInt32(StockID == "" ? "0" : StockID)));
                    this.Parameters.Add(Parameter("@User_ID", Convert.ToInt32(UserID == "" ? "0" : UserID)));
                    this.Parameters.Add(Parameter("@OrgStruc_Id", Convert.ToInt32(OrgStrucID == "" ? "0" : OrgStrucID)));
                    this.Parameters.Add(Parameter("@Status", status));
                    this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(Inv_ItemID == "" ? "0" : Inv_ItemID)));
                    this.Parameters.Add(Parameter("@PackBig_ID", Convert.ToInt32(Inv_ItemID == "" ? "0" : PackBig_ID)));
                    this.Parameters.Add(Parameter("@PackSmall_ID", Convert.ToInt32(drRow["PackSmall_ID"] == "" ? "0" : drRow["PackSmall_ID"])));
                    this.Parameters.Add(Parameter("@PackDis_Qty", Convert.ToInt32(drRow["PackDis_Qty"] == "" ? "0" : drRow["PackDis_Qty"])));
                    this.Parameters.Add(Parameter("@PackReceive_Qty", Convert.ToInt32(drRow["PackReceive_Qty"] == "" ? "0" : drRow["PackReceive_Qty"])));

                    int result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_PackDistribute_InsertUpdate", this.Parameters);

                    if (result <= 0)
                    {
                        this.Rollback();
                        return false;
                    }
                    else
                    {
                        chk_Data++;
                    }

                }

                if (chk_Data == chk_Data_All)
                {
                    chk = true;
                    this.CommitTransaction();
                }
                else
                {
                    chk = false;
                    this.Rollback();
                }

            }
            catch (Exception ex)
            {
                this.Rollback();
            }

            return chk;

        }

        public DataTable GetItemPack_ItemSearch(string invItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", invItemID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_ItemSearch_SelectPack_ByID", param);
        }

        public DataSet GetItemSearchByCateID(string itemCode, string itemName, string cateID, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Cate_ID", cateID));

            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_ItemSearchByCateID_SelectPaging", param);
        }


        public string Delete_ItemPack( DataTable dtFileDeletedPack)
        {

            string chk = "";

            this.UseTransaction = true; // Request Transaction
            this.BeginParameter();

            try
            {
                //ลบ Pack ที่ต้องการ Delete
                if(dtFileDeletedPack.Rows.Count > 0)
                {
                    //ทำการ Check ก่อนว่ามี item และ pack นี้ใน stock movement รึยัง
                    int chkMovement = 0; // ใช้ count ค่าว่ามี item และ pack นี้ใน stock movement รึยัง ถ้ามากกว่า 0 แสดงว่ามีแล้ว
                    string PackCantDel = "";
                    foreach (DataRow drRow in dtFileDeletedPack.Rows)
                    {
                         DataSet ds =  new DatabaseHelper().ExecuteQuery("SELECT COUNT(*) AS CNT FROM [dbo].[INV_STOCK_MOVEMENT] WHERE [Inv_ItemID] = " + drRow["Inv_ItemID"].ToString() + " AND [Pack_ID] = "+ drRow["Pack_ID"].ToString());
                         if (Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"].ToString()) > 0)
                         {
                             DataSet dsPack = new DatabaseHelper().ExecuteQuery("SELECT [Package_Name] FROM [dbo].[Inv_Package] WHERE [Pack_ID] = " + drRow["Pack_ID"].ToString() + " AND  [Pack_Status] = '1'");
                             if (PackCantDel == "")
                             {
                                 PackCantDel = PackCantDel + " " + dsPack.Tables[0].Rows[0]["Package_Name"].ToString();
                             }
                             else
                             {
                                 PackCantDel = PackCantDel + " , " + dsPack.Tables[0].Rows[0]["Package_Name"].ToString();
                             }
                         }
                         chkMovement = chkMovement + Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"].ToString());
                    }

                    if (chkMovement > 0)
                    {
                        this.Rollback();
                        return PackCantDel;
                    }

                    foreach (DataRow drRow in dtFileDeletedPack.Rows)
                    {
                        this.BeginParameter();

                        this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"].ToString() == "" ? "0" : drRow["Inv_ItemID"].ToString())));
                        this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"].ToString() == "" ? "0" : drRow["Pack_ID"].ToString())));
                        int result = 0;
                            
                        result = this.ExecuteNonQuery_Chk("sp_Inv_ItemPack_Delete", this.Parameters);

                        if (result <= 0)
                        {
                            this.Rollback();
                            return "False";
                        }
                    }
                }

                //Add Pack ที่ต้องการ

                chk = "True";
                this.CommitTransaction();
                

            }
            catch (Exception ex)
            {
                chk = "False";
                this.Rollback();
            }

            return chk;

        }

        #endregion

        #region PT 03/10/2013
        public void InvItemPackUpdateAvgCost(string inv_itemid, string inv_itemPackid)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemID", inv_itemid));
            param.Add(new SqlParameter("@Pack_Id", inv_itemPackid));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_ItemPack_UpdateAvgCost", param);
        }
        #endregion

     


    }
}