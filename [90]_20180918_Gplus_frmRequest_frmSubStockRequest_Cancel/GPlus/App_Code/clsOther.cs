using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;
using System.Data.SqlClient;
using System.Web.UI;
using GPlus;

/// <summary>
/// Summary description for ReceiveDetail
/// </summary>
public class clsOther
{
    public String lotProduct { get; set; }

    public String lotOther { get; set; }

    public String lotOtherNum { get; set; }

    public String qtyOther { get; set; }

    public String expireDateOther { get; set; }

    public String priceOther { get; set; }

    public String totalOther { get; set; }

    public String lotProductStock { get; set; } //stock

    public String lotStock { get; set; } //stock

    public String lotStockRun { get; set; } //stock

    public String qtyStock { get; set; } //stock

    public String idStock { get; set; } //stock

    public String nameStock { get; set; } //stock

    public String productID { get; set; } //head

    public String productName { get; set; } //head

    public String productQty { get; set; } //head

    public String productType { get; set; } //head

    public String productTypeText { get; set; } //head

    public String productTotalPrice { get; set; } //head



    static List<clsOther> list_other = new List<clsOther>();

    static List<clsOther> list_otherHead = new List<clsOther>();

    static List<clsOther> list_otherStock = new List<clsOther>();

    public clsOther()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void clearList()
    {
        list_other.Clear();
        list_otherHead.Clear();
        list_otherStock.Clear(); 
    }

    public void clearListStock()
    {
        list_otherStock.Clear();
    }

    public string fn_addLot(string str_lotProduct, string str_lotProductName, string str_lotProductQty, string str_lotProductType, string str_lotProductTypeText, string[] strArr_lotDetail,string[] strArr_lotDetailRun, string[] strArr_qtyDetail, string[] strArr_expireDetail, string[] strArr_priceDetail
        , string[] strArr_totalDetail)
    {

        int j = 0;
        double sum = 0;
        bool check = false;
        clsOther obj_other = new clsOther();
        try
        {

            foreach (String str in strArr_lotDetail)
            {       

                for (int i = 0; i < list_other.Count; i++)
                {
                    if (str_lotProduct == list_other[i].lotProduct && strArr_lotDetailRun[j] == list_other[i].lotOtherNum)
                    {
                        list_other[i].qtyOther = strArr_qtyDetail[j];
                        list_other[i].expireDateOther = strArr_expireDetail[j];
                        list_other[i].priceOther = strArr_priceDetail[j];
                        list_other[i].totalOther = strArr_totalDetail[j];
                        list_other[i].lotOther = strArr_lotDetail[j];

                        check = true;
                    }
                }
                if (check == false)
                {
                    obj_other.lotOtherNum = strArr_lotDetailRun[j];
                    obj_other.lotProduct = str_lotProduct;
                    obj_other.lotOther = strArr_lotDetail[j];
                    obj_other.qtyOther = strArr_qtyDetail[j];
                    obj_other.expireDateOther = strArr_expireDetail[j];
                    obj_other.priceOther = strArr_priceDetail[j];
                    obj_other.totalOther = strArr_totalDetail[j];

                    list_other.Add(obj_other);
                }

                sum += Double.Parse(strArr_totalDetail[j]);
                j++;

                obj_other = new clsOther();
            }

            check = false;
            for (int i = 0; i < list_otherHead.Count; i++)
            {
                //if (str_lotProduct == list_other[i].lotProduct)
                //{
                //    list_other[i].productQty = str_lotProductQty;
                //    list_other[i].productType = str_lotProductType;
                //    list_other[i].productTypeText = str_lotProductTypeText;
                //    list_other[i].productTotalPrice = sum.ToString();
                //    check = true;
                //}
                if (str_lotProduct == list_otherHead[i].productID)
                {
                    list_otherHead[i].productQty = str_lotProductQty;
                    list_otherHead[i].productType = str_lotProductType;
                    list_otherHead[i].productTypeText = str_lotProductTypeText;
                    list_otherHead[i].productTotalPrice = sum.ToString();
                    check = true;
                }
            }
            if (check == false)
            {
                obj_other.productID = str_lotProduct;
                obj_other.productName = str_lotProductName;
                obj_other.productQty = str_lotProductQty;
                obj_other.productType = str_lotProductType;
                obj_other.productTypeText = str_lotProductTypeText;
                obj_other.productTotalPrice = sum.ToString();

                list_otherHead.Add(obj_other);
            }

            return "";
        }
        catch(Exception ex)
        {
            return ex.ToString();
        }
       
    }


    public string fn_delete(string str_ID)
    {
        try
        {
            clsConnDb obj_con = new clsConnDb();
            string str_sql = "update [Inv_TransHead] set [Transaction_Status] = '0' where [Transaction_No] = '" + str_ID + "' ";
            obj_con._fn_openConn();
            obj_con._fn_queryNoReturn(str_sql);
            obj_con._fn_closeConn();
            return "";
        }
        catch(Exception ex)
        {
            return ex.ToString();
        }
    }

    public string fn_addStockLot(string str_lotProduct, string[] strArr_hidLotStock, string[] strArr_hidLotStockRun, string[] strArr_qtyStock, string[] strArr_idStock, string[] strArr_nameStock)
    {

        

        bool check = false;
        int j = 0;
        clsOther obj_otherStock = new clsOther();
        try
        {
            fn_deleteLotStockLast(str_lotProduct);


            foreach (String str in strArr_hidLotStock)
            {
                if (strArr_idStock[j].ToString() != "undefined")
                {
                    for (int i = 0; i < list_otherStock.Count; i++)
                    {
                        if (str_lotProduct == list_otherStock[i].lotProductStock && strArr_hidLotStock[j] == list_otherStock[i].lotStock && strArr_idStock[j] == list_otherStock[i].idStock)
                        {
                            list_otherStock[i].qtyStock = strArr_qtyStock[j];
                            list_otherStock[i].lotStockRun = strArr_hidLotStockRun[j];
                            check = true;
                        }
                    }
                    if (check == false)
                    {
                        obj_otherStock.lotProductStock = str_lotProduct;
                        obj_otherStock.lotStock = strArr_hidLotStock[j];
                        obj_otherStock.lotStockRun = strArr_hidLotStockRun[j];
                        obj_otherStock.qtyStock = strArr_qtyStock[j];
                        obj_otherStock.idStock = strArr_idStock[j];
                        obj_otherStock.nameStock = strArr_nameStock[j];

                        list_otherStock.Add(obj_otherStock);
                    }
                }

                j++;
                obj_otherStock = new clsOther();
                check = false;
            }
            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }

    }

    public void fn_getLotAddObject()
    {

    }

    public List<clsOther> fn_getLotHeadAddObject(string str_transactionID)
    {
        list_other.Clear();
        list_otherHead.Clear();
        list_otherStock.Clear();

        clsConnDb obj_con = new clsConnDb();
        DataTable obj_dtHead = new DataTable();
        DataTable obj_dtLot = new DataTable();   
        DataTable obj_dtStock = new DataTable();
        clsOther obj_other = new clsOther();
        clsOther obj_otherStock = new clsOther();
        string str_sql = "SELECT [Transaction_ItemID],[Transaction_ID],[Inv_ItemId],(select [Inv_ItemCode] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [Inv_TransDetail].[Inv_ItemId]) as ItemCode " +
                         " ,(select [Inv_ItemName] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [Inv_TransDetail].[Inv_ItemId]) as ItemName " +
                         " ,[Pack_ID],(select Description from [Inv_ItemPack] where [Inv_ItemPack].[Inv_ItemID] = [Inv_TransDetail].[Inv_ItemId] and [Inv_ItemPack].[Pack_Id] = [Inv_TransDetail].[Pack_ID]) as PackName " +
                         " ,[Qty],[Price_Unit],[Amount] FROM [Inv_TransDetail]" +
                         "  where [Transaction_ID] = '" + str_transactionID + "'  ";

        obj_con._fn_openConn();
        obj_dtHead = obj_con._fn_queryForDataTable(str_sql);
        

        for (int i = 0; i < obj_dtHead.Rows.Count; i++)
        {

                obj_other.productID = obj_dtHead.Rows[i]["ItemCode"].ToString();
                obj_other.productName = obj_dtHead.Rows[i]["ItemName"].ToString();
                obj_other.productQty = obj_dtHead.Rows[i]["Qty"].ToString();
                obj_other.productType = obj_dtHead.Rows[i]["Pack_ID"].ToString();
                obj_other.productTypeText = obj_dtHead.Rows[i]["PackName"].ToString();
                obj_other.productTotalPrice = obj_dtHead.Rows[i]["Amount"].ToString();

                list_otherHead.Add(obj_other);
                obj_other = new clsOther();    

            str_sql = "SELECT [Stock_Lot_ID],[Stock_ID],[Inv_ItemID],[Pack_ID],[Expire_Date],[Lot_No],[Lot_Qty]" +
                      ",(select [Inv_ItemCode] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [Inv_Stock_Lot].[Inv_ItemID]) as ItemCode " +
                      ",[Lot_Amount],[Unit_Price],[Transaction_type], [Transaction_id] from [Inv_Stock_Lot]" +
                      "  where [Transaction_id] = '" + obj_dtHead.Rows[i]["Transaction_ItemID"].ToString() + "' and  Transaction_type = 'G'";

            obj_dtLot = obj_con._fn_queryForDataTable(str_sql);


            for (int j = 0; j < obj_dtLot.Rows.Count; j++)
            {
                    obj_other.lotProduct = obj_dtLot.Rows[j]["ItemCode"].ToString();
                    obj_other.lotOther = obj_dtLot.Rows[j]["Lot_No"].ToString();
                    obj_other.qtyOther = obj_dtLot.Rows[j]["Lot_Qty"].ToString();
                    obj_other.expireDateOther = obj_con._fn_setDateFormatChirtForWeb(obj_dtLot.Rows[j]["Expire_Date"].ToString());
                    obj_other.priceOther = obj_dtLot.Rows[j]["Unit_Price"].ToString();
                    obj_other.totalOther = obj_dtLot.Rows[j]["Lot_Amount"].ToString();

                    list_other.Add(obj_other);
                    obj_other  = new clsOther();

                    str_sql = "SELECT [Stock_Lot_ID],[Location_ID],[Qty_Location] , (select Location_Name From Inv_Location where Inv_Location.Location_ID = Inv_Stock_Lot_Location.Location_ID) as LocationName" +
                         " from Inv_Stock_Lot_Location where [Stock_Lot_ID] = '" + obj_dtLot.Rows[j]["Stock_Lot_ID"].ToString() + "' ";

                obj_dtStock = obj_con._fn_queryForDataTable(str_sql);

                for (int k = 0; k < obj_dtStock.Rows.Count; k++)
                {
                    obj_otherStock.lotProductStock = obj_dtLot.Rows[j]["ItemCode"].ToString();
                    obj_otherStock.lotStock = (j + 1).ToString();
                    obj_otherStock.lotStockRun = (k + 1).ToString();
                    obj_otherStock.qtyStock = obj_dtStock.Rows[k]["Qty_Location"].ToString();
                    obj_otherStock.idStock = obj_dtStock.Rows[k]["Location_ID"].ToString();
                    obj_otherStock.nameStock = obj_dtStock.Rows[k]["LocationName"].ToString();

                    list_otherStock.Add(obj_otherStock);
                    obj_otherStock = new clsOther();
                }
            }
        }

        return list_otherHead;
    }


    public List<clsOther> fn_getLotHeadAddObjectForPay(string str_transactionID)
    {
        list_other.Clear();
        list_otherHead.Clear();
        list_otherStock.Clear();

        clsConnDb obj_con = new clsConnDb();
        DataTable obj_dtHead = new DataTable();
        DataTable obj_dtLot = new DataTable();
        DataTable obj_dtStock = new DataTable();
        clsOther obj_other = new clsOther();
        clsOther obj_otherStock = new clsOther();
        string str_sql = "SELECT [Transaction_ItemID],[Transaction_ID],[Inv_ItemId],(select [Inv_ItemCode] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [Inv_TransDetail].[Inv_ItemId]) as ItemCode " +
                         " ,(select [Inv_ItemName] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [Inv_TransDetail].[Inv_ItemId]) as ItemName " +
                         " ,[Pack_ID],(select Description from [Inv_ItemPack] where [Inv_ItemPack].[Inv_ItemID] = [Inv_TransDetail].[Inv_ItemId] and [Inv_ItemPack].[Pack_Id] = [Inv_TransDetail].[Pack_ID]) as PackName " +
                         " ,[Qty],[Price_Unit],[Amount] FROM [Inv_TransDetail]" +
                         "  where [Transaction_ID] = '" + str_transactionID + "'  ";

        obj_con._fn_openConn();
        obj_dtHead = obj_con._fn_queryForDataTable(str_sql);


        for (int i = 0; i < obj_dtHead.Rows.Count; i++)
        {

            obj_other.productID = obj_dtHead.Rows[i]["ItemCode"].ToString();
            obj_other.productName = obj_dtHead.Rows[i]["ItemName"].ToString();
            obj_other.productQty = obj_dtHead.Rows[i]["Qty"].ToString();
            obj_other.productType = obj_dtHead.Rows[i]["Pack_ID"].ToString();
            obj_other.productTypeText = obj_dtHead.Rows[i]["PackName"].ToString();
            obj_other.productTotalPrice = obj_dtHead.Rows[i]["Amount"].ToString();

            list_otherHead.Add(obj_other);
            obj_other = new clsOther();

            //str_sql = "SELECT [Stock_ID],[Inv_ItemID],[Pack_ID],[Stock_Lot_ID],sum([Qty_Out]) as Lot_Qty" +
            //          ",(select [Inv_ItemCode] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [INV_STOCK_LOT_LOG].[Inv_ItemID]) as ItemCode " +
            //          ",[Amount], Amount/(sum([Qty_Out])) as Unit_Price,[Transaction_type], [Transaction_id] from [INV_STOCK_LOT_LOG]" +
            //          "  where [Transaction_id] = " + str_transactionID + " and [Inv_ItemID]  = '" + obj_dtHead.Rows[i]["Inv_ItemId"].ToString() + "' and  Transaction_type = 'U'  " +
            //          " group by [Stock_Lot_ID],[Stock_ID],[Inv_ItemID],[Pack_ID],[Stock_Lot_ID],[Transaction_type], [Transaction_id],[Amount] ";

            str_sql = "SELECT [Stock_Lot_ID],[Stock_ID],[Inv_ItemID],[Pack_ID],sum([Qty_Out]) as Lot_Qty,SUM([Amount]) as Amount" +
                      ",SUM([Amount])/sum([Qty_Out]) as Unit_Price ,(select [Inv_ItemCode] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [INV_STOCK_LOT_LOG].[Inv_ItemID]) " +
                      " as ItemCode  from [INV_STOCK_LOT_LOG] " +
                      "  where [Transaction_id] = " + str_transactionID + " and [Inv_ItemID]  = '" + obj_dtHead.Rows[i]["Inv_ItemId"].ToString() + "' and  Transaction_type = 'U'  " +
                      " group by [Stock_Lot_ID],[Stock_ID],[Inv_ItemID],[Pack_ID]  ";

            obj_dtLot = obj_con._fn_queryForDataTable(str_sql);


            for (int j = 0; j < obj_dtLot.Rows.Count; j++)
            {
                obj_other.lotProduct = obj_dtLot.Rows[j]["ItemCode"].ToString();
                //obj_other.lotOther = obj_dtLot.Rows[j]["Lot_No"].ToString();
                obj_other.qtyOther = obj_dtLot.Rows[j]["Lot_Qty"].ToString();
                //obj_other.expireDateOther = obj_con._fn_setDateFormatChirtForWeb(obj_dtLot.Rows[j]["Expire_Date"].ToString());
                obj_other.priceOther = obj_dtLot.Rows[j]["Unit_Price"].ToString();
                obj_other.totalOther = obj_dtLot.Rows[j]["Amount"].ToString();

                list_other.Add(obj_other);
                obj_other = new clsOther();


                str_sql = "SELECT [Stock_Lot_ID],[Stock_ID],[Inv_ItemID],[Pack_ID],[Location_ID],[Qty_Out]" +
                     ",(select [Inv_ItemCode] from [Inv_Item] where [Inv_Item].[Inv_ItemID] = [INV_STOCK_LOT_LOG].[Inv_ItemID]) as ItemCode " +
                     ",[Amount],[Transaction_type], [Transaction_id] , (select Location_Name From Inv_Location where Inv_Location.Location_ID = [INV_STOCK_LOT_LOG].Location_ID) as LocationName" +
                     " from [INV_STOCK_LOT_LOG] where [Transaction_id] = " + str_transactionID + " and [Inv_ItemID]  = '" + obj_dtHead.Rows[i]["Inv_ItemId"].ToString() + "' and  Transaction_type = 'U'  " +
                     " and [Stock_Lot_ID] = '" + obj_dtLot.Rows[j]["Stock_Lot_ID"].ToString() + "' group by [Stock_Lot_ID],[Location_ID],[Stock_ID],[Qty_Out],[Inv_ItemID],[Pack_ID],[Stock_Lot_ID],[Transaction_type], [Transaction_id],[Amount] ";

              //  str_sql = "SELECT [Stock_Lot_ID],[Location_ID],[Qty_Location] , (select Location_Name From Inv_Location where Inv_Location.Location_ID = Inv_Stock_Lot_Location.Location_ID) as LocationName" +
               //      " from Inv_Stock_Lot_Location where [Stock_Lot_ID] = '" + obj_dtLot.Rows[j]["Stock_Lot_ID"].ToString() + "' ";

                obj_dtStock = obj_con._fn_queryForDataTable(str_sql);

                for (int k = 0; k < obj_dtStock.Rows.Count; k++)
                {
                    obj_otherStock.lotProductStock = obj_dtLot.Rows[j]["ItemCode"].ToString();
                    obj_otherStock.lotStock = (j + 1).ToString();
                    obj_otherStock.lotStockRun = (k + 1).ToString();
                    obj_otherStock.qtyStock = obj_dtStock.Rows[k]["Qty_Out"].ToString();
                    obj_otherStock.idStock = obj_dtStock.Rows[k]["Location_ID"].ToString();
                    obj_otherStock.nameStock = obj_dtStock.Rows[k]["LocationName"].ToString();

                    list_otherStock.Add(obj_otherStock);
                    obj_otherStock = new clsOther();
                }
            }
        }

        return list_otherHead;
    }

    public void fn_getStockAddObject()
    {

    }




    public List<clsOther> fn_getLotHeadDetail()
    {
        return list_otherHead;
    }

    public List<clsOther> fn_getLotDetail(string str_productID)
    {
        clsOther obj_otherShow = new clsOther();
        List<clsOther> list_otherShow = new List<clsOther>();

        for (int i = 0; i < list_other.Count; i++)
        {

            if (str_productID == list_other[i].lotProduct)
            {
                obj_otherShow.lotProduct = list_other[i].lotProduct;
                obj_otherShow.lotOther = list_other[i].lotOther;
                obj_otherShow.qtyOther = list_other[i].qtyOther;
                obj_otherShow.expireDateOther = list_other[i].expireDateOther;
                obj_otherShow.priceOther = list_other[i].priceOther;
                obj_otherShow.totalOther = list_other[i].totalOther;

              
                list_otherShow.Add(obj_otherShow);
            }

            obj_otherShow = new clsOther();
        }

        return list_otherShow;
    }

    public List<clsOther> fn_getLotStockDetail(string str_productID)
    {
        clsOther obj_otherShow = new clsOther();
        List<clsOther> list_otherShow = new List<clsOther>();

        for (int i = 0; i < list_otherStock.Count; i++)
        {

            if (str_productID == list_otherStock[i].lotProductStock)
            {
                obj_otherShow.lotProductStock = list_otherStock[i].lotProductStock;
                obj_otherShow.lotStock = list_otherStock[i].lotStock;
                obj_otherShow.lotStockRun = list_otherStock[i].lotStockRun;
                obj_otherShow.qtyStock = list_otherStock[i].qtyStock;
                obj_otherShow.idStock = list_otherStock[i].idStock;
                obj_otherShow.nameStock = list_otherStock[i].nameStock;

                list_otherShow.Add(obj_otherShow);
            }

            obj_otherShow = new clsOther();
        }
        return list_otherShow;
    }

    public void fn_deleteLot(string str_productID)
    {
        for (int i = 0; i < list_otherHead.Count; i++)
        {

            if (str_productID == list_otherHead[i].productID)
            {
                    list_otherHead.RemoveAt(i);
                    break;
            }

        }

        for (int i = 0; i < list_other.Count; i++)
        {

            if (str_productID == list_other[i].lotProduct)
            {
                list_other.RemoveAt(i);
                break;
            }

        }

        for (int i = 0; i < list_otherStock.Count; i++)
        {

            if (str_productID == list_otherStock[i].lotProduct)
            {
                list_otherStock.RemoveAt(i);
                break;
            }

        }
      
    }


    public void fn_deleteLotStockLast(string str_productID)
    {

        List<clsOther> list_otherStockSub = new List<clsOther>();
        List<clsOther> list_otherStockAdd = new List<clsOther>();
        clsOther obj_otherStock = new clsOther();



        int count = 0;
        count = list_otherStock.Count;
        for (int i = 0; i < count; i++)
        {

            if (str_productID != list_otherStock[i].lotProductStock)
            {

                obj_otherStock.lotProductStock = list_otherStock[i].lotProductStock;
                obj_otherStock.lotStock = list_otherStock[i].lotStock ;
                obj_otherStock.lotStockRun = list_otherStock[i].lotStockRun ;
                obj_otherStock.qtyStock = list_otherStock[i].qtyStock ;
                obj_otherStock.idStock = list_otherStock[i].idStock ;
                obj_otherStock.nameStock = list_otherStock[i].nameStock ;
                list_otherStockSub.Add(obj_otherStock);
                
            }

            obj_otherStock = new clsOther();
        }


        for (int i = 0; i < count; i++)
        {

            if (str_productID == list_otherStock[i].lotProductStock)
            {

                obj_otherStock.lotProductStock = list_otherStock[i].lotProductStock;
                obj_otherStock.lotStock = list_otherStock[i].lotStock;
                obj_otherStock.lotStockRun = list_otherStock[i].lotStockRun;
                obj_otherStock.qtyStock = list_otherStock[i].qtyStock;
                obj_otherStock.idStock = list_otherStock[i].idStock;
                obj_otherStock.nameStock = list_otherStock[i].nameStock;
                list_otherStockAdd.Add(obj_otherStock);

            }

            obj_otherStock = new clsOther();
        }


            list_otherStock.Clear();
        

        if (list_otherStockAdd.Count > 1)
        {
            list_otherStockAdd.RemoveAt(list_otherStockAdd.Count - 1);
        }

        for (int i = 0; i < list_otherStockSub.Count; i++)
        {
                obj_otherStock.lotProductStock = list_otherStockSub[i].lotProductStock;
                obj_otherStock.lotStock = list_otherStockSub[i].lotStock;
                obj_otherStock.lotStockRun = list_otherStockSub[i].lotStockRun;
                obj_otherStock.qtyStock = list_otherStockSub[i].qtyStock;
                obj_otherStock.idStock = list_otherStockSub[i].idStock;
                obj_otherStock.nameStock = list_otherStockSub[i].nameStock;
                list_otherStock.Add(obj_otherStock);

                obj_otherStock = new clsOther();
        }

        for (int i = 0; i < list_otherStockAdd.Count; i++)
        {
            obj_otherStock.lotProductStock = list_otherStockAdd[i].lotProductStock;
            obj_otherStock.lotStock = list_otherStockAdd[i].lotStock;
            obj_otherStock.lotStockRun = list_otherStockAdd[i].lotStockRun;
            obj_otherStock.qtyStock = list_otherStockAdd[i].qtyStock;
            obj_otherStock.idStock = list_otherStockAdd[i].idStock;
            obj_otherStock.nameStock = list_otherStockAdd[i].nameStock;
            list_otherStock.Add(obj_otherStock);
            obj_otherStock = new clsOther();

        }

    }

    public void fn_saveReceive(string str_receiveID, string str_receiveDate, string str_number, string str_typeReceive, string str_remark, string str_supplier, string str_preSend, string str_preReceive, string str_stock, string str_user, string str_payID)
    {
        //Pagebase Page_p = new Pagebase();
       // string str_user = Page_p.UserID;
        
        //string str_user = "1" ;

        clsConnDb obj_con = new clsConnDb();

        string str_sql = "";

        if (str_payID == "")
        {
            str_sql = @"insert into [Inv_TransHead]([Transaction_No],[Transaction_Date],[Transaction_Type],[Transaction_Sub_Type],[Stock_ID],[Transaction_Status]
                            ,[Refer_Supplier_ID],[Refer_Document],[Refer_Contact_Person],[Transaction_By],[Reason],[Create_Date],[Create_By]) 
                            values('" + getRunningStock(str_stock, "G") + "','" + obj_con._fn_setDateFormatBuddhist(str_receiveDate) + "','G', '" + str_typeReceive + "','" + str_stock + "','1','" + str_supplier + "','" + str_number + "', '" + str_preSend + "','" + str_preReceive + "','" + str_remark + "',getdate(),'" + str_user + "') ; SELECT @@IDENTITY AS newID;";
        }
        else
        {
            str_sql = @"insert into [Inv_TransHead]([Transaction_No],[Transaction_Date],[Transaction_Type],[Transaction_Sub_Type],[Stock_ID],[Transaction_Status]
                            ,[Refer_Supplier_ID],[Refer_Document],[Refer_Contact_Person],[Transaction_By],[Reason],[Create_Date],[Create_By],pay_id) 
                            values('" + getRunningStock(str_stock, "G") + "','" + obj_con._fn_setDateFormatBuddhist(str_receiveDate) + "','G', '" + str_typeReceive + "','" + str_stock + "','1','" + str_supplier + "','" + str_number + "', '" + str_preSend + "','" + str_preReceive + "','" + str_remark + "',getdate(),'" + str_user + "','" + str_payID + "') ; SELECT @@IDENTITY AS newID;";
        }
      
           

            obj_con._fn_openConn();
            if (obj_con._fn_runTransForLotReceive(str_sql, str_receiveDate, str_number, str_stock, str_user, str_typeReceive, list_otherHead, list_other, list_otherStock))
            {
                setRunningStock(str_stock);
            }
            obj_con._fn_closeConn();
        
      
       


        list_other.Clear();
        list_otherHead.Clear();
        list_otherStock.Clear();
    }


    public string fn_delete(string str_ID,string str_type,string str_user)
    {
        try
        {
            clsConnDb obj_con = new clsConnDb();

            if (str_type == "R")
            {
                obj_con._fn_openConn();
                obj_con._fn_runDeleteReceive(str_ID, str_user);
                obj_con._fn_closeConn();
            }
            else
            {
                obj_con._fn_openConn();
                obj_con._fn_runDeletePay(str_ID, str_user);
                obj_con._fn_closeConn();
            }

            return "";
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }


    public string fn_savePay(string str_payID, string str_payDate, string str_typePay, string str_give, string str_preSend, string str_prePay, string str_stock,string str_user)
    {


        //Pagebase Page_p = new Pagebase();

        //string str_user = ((Pagebase)Page_p).UserID;
        //string str_user = "1";
        string str_result = "";
        clsConnDb obj_con = new clsConnDb();

        string str_sql = "";

        str_sql = @"insert into [Inv_TransHead]([Transaction_No],[Transaction_Date],[Transaction_Type],[Transaction_Sub_Type],[Stock_ID],[Transaction_Status]
                            ,[Approver_By],[Refer_Contact_Person],[Transaction_By],[Create_Date],[Create_By]) 
                            values('" + getRunningStock(str_stock, "U") + "','" + obj_con._fn_setDateFormatBuddhist(str_payDate) + "','U', '" + str_typePay + "','" + str_stock + "','1', '" + str_preSend + "','" + str_give + "','" + str_prePay + "',getdate(),'" + str_user + "') ; SELECT @@IDENTITY AS newID;";


        obj_con._fn_openConn();
        str_result = obj_con._fn_runTransForLotPay(str_sql, str_payDate, str_prePay, str_stock, str_user, str_typePay, list_otherHead, list_other, list_otherStock) ;
        if (str_result == "True")
        {
            setRunningStock(str_stock);
            str_result = "True";
            list_other.Clear();
            list_otherHead.Clear();
            list_otherStock.Clear();

        }
        else if (str_result == "False")
        {
            str_result = "False";
            list_other.Clear();
            list_otherHead.Clear();
            list_otherStock.Clear();
        }
        else
        {
            str_result = "ไม่มีสินค้านี้ในคลัง";
        }
       
        obj_con._fn_closeConn();
        return str_result;
    }


    public string getRunningStock(string str_stock , string str_type)
    {
        string str_running = "";
        StringBuilder strb_sql = new StringBuilder(" select [Stock_Code],[Running] FROM [Inv_Stock]");
        strb_sql.Append(" where Stock_Id = " + str_stock + " ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());
        int year = 0;

        if(li_Item.Read())
        {

            if (DateTime.Now.Year < 2500)
            {
                year = DateTime.Now.Year + 543;
            }

            if (str_type == "G")
            {
                str_running = li_Item["Stock_Code"] + "-C-" + li_Item["Running"] + "/" + year.ToString().Substring(2, 2);
            }
            else
            {
                str_running = li_Item["Stock_Code"] + "-U-" + li_Item["Running"] + "/" + year.ToString().Substring(2, 2);
            }
        }
        obj_db._fn_closeConn();
        return str_running;        
    }


    public string getRequestID(string str_id) // หน่วยงาน ID
    {
        string str_result = "";
        StringBuilder strb_sql = new StringBuilder("  select  Request_Id from Inv_Request ");
        strb_sql.Append(" where Request_NO  = '" + str_id + "' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());
        

        if (li_Item.Read())
        {
            str_result = li_Item["Request_Id"].ToString();
        }
        obj_db._fn_closeConn();
        return str_result;
    }


    public string getRequestDesc(string str_id) // หน่วยงาน รายละเอียด
    {
        string str_result = "";
        StringBuilder strb_sql = new StringBuilder("  select  Description from Inv_OrgStructure inner join Inv_Request on Inv_Request.OrgStruc_Id_Req = Inv_OrgStructure.OrgStruc_Id");
        strb_sql.Append(" where    Inv_Request.Request_Id  = " + str_id + " ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());
      

        if (li_Item.Read())
        {
            str_result = li_Item["Description"].ToString();
        }
        obj_db._fn_closeConn();
        return str_result;
    }


    public List<ListItem> getRequestDate(string str_id) // หน่วยงาน วันที่
    {
        List<ListItem> drp_li_Items = new List<ListItem>();

        StringBuilder strb_sql = new StringBuilder(" select pay_date,Pay_Id from inv_StockPay  ");
        strb_sql.Append(" Where Request_id = " + str_id + " and Pay_Status = '1' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        while (drd_li_Item.Read())
        {
            ListItem drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["pay_date"] + "";
            drp_li_Item.Value = drd_li_Item["Pay_Id"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }

    public string getDateForPayID(string str_id) // หน่วยงาน Pay ID
    {
        string str_result = "";
        StringBuilder strb_sql = new StringBuilder(" select  pay_date from inv_StockPay  ");
        strb_sql.Append(" Where Pay_Id = " + str_id + " ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());


        if (li_Item.Read())
        {
            str_result = li_Item["pay_date"].ToString();
        }
        obj_db._fn_closeConn();
        return str_result;
    }

    public string getRequestForPayID(string str_id) // หน่วยงาน Pay ID
    {
        string str_result = "";
        StringBuilder strb_sql = new StringBuilder(" select Request_No from inv_StockPay inner join Inv_Request on Inv_Request.Request_id = inv_StockPay.Request_id   ");
        strb_sql.Append(" Where Pay_Id = " + str_id + " ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());


        if (li_Item.Read())
        {
            str_result = li_Item["Request_No"].ToString();
        }
        obj_db._fn_closeConn();
        return str_result;
    }


    public string getOrgForPayID(string str_id) // หน่วยงาน Pay ID
    {
        string str_result = "";
        StringBuilder strb_sql = new StringBuilder(" select  Description from Inv_OrgStructure inner join Inv_Request on Inv_Request.OrgStruc_Id_Req = Inv_OrgStructure.OrgStruc_Id   ");
        strb_sql.Append(" inner join inv_StockPay on Inv_Request.Request_id = inv_StockPay.Request_id   Where Pay_Id = " + str_id + " ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());


        if (li_Item.Read())
        {
            str_result = li_Item["Description"].ToString();
        }
        obj_db._fn_closeConn();
        return str_result;
    }




    public string getUnitPrice(string str_id,string str_itemID, string str_packID) // หน่วยงาน ราคา
    {
        string str_result = "";
        StringBuilder strb_sql = new StringBuilder("select Unit_Price from inv_StockPayItem  ");
        strb_sql.Append(" where  Inv_pay_id = '" + str_id + "' and inv_Itemid = '" + _fn_GetIDItem(str_itemID) + "'  And Pack_id  = '" + str_packID + "' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());


        if (li_Item.Read())
        {
            str_result = li_Item["Unit_Price"].ToString();
        }
        obj_db._fn_closeConn();
        return str_result;
    }

    public string getCheckRequest(string str_itemID, string str_packID, string str_payID, string str_total) // หน่วยงาน check
    {
        string str_result = "";
        int int_pay = 0;
        int int_trans = 0;

        StringBuilder strb_sql = new StringBuilder(" Select inv_StockPayItem.Pay_Quantity ,");
        strb_sql.Append("(Select  sum(Inv_TransDetail.Qty) as itemQty  from inv_transHead inner join Inv_TransDetail on Inv_transhead.Transaction_id = Inv_TransDetail.Transaction_ID where  inv_transHead.pay_id = inv_StockPayItem.INV_Pay_Id )  as itemQty from inv_StockPayItem  ");
        strb_sql.Append(" where  inv_StockPayItem.INV_Pay_Id = '"+str_payID+"' and inv_Itemid = '"+ _fn_GetIDItem(str_itemID) +"'  And Pack_id  = '" + str_packID +"' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());



        if (li_Item.Read())
        {
            
            int_pay = int.Parse(li_Item["Pay_Quantity"].ToString());
            
            //strb_sql = new StringBuilder("Select  sum(Inv_TransDetail.Qty) as itemQty  from inv_transHead inner join Inv_TransDetail on Inv_transhead.Transaction_id = Inv_TransDetail.Transaction_ID");
            //strb_sql.Append(" where  inv_transHead.pay_id = '" + str_payID + "' ");
            //SqlDataReader li_Item2 = obj_db._fn_query(strb_sql.ToString());


            if (li_Item["itemQty"].ToString() != "")
            {
                int_trans = int.Parse(li_Item["itemQty"].ToString());
            }
            else
            {
                int_trans = 0;
            }

            int_pay = int_pay - int_trans;
            if (int.Parse(str_total) <= int_pay)
            {
                str_result = "";
            }
            else
            {
                str_result = "จำนวนเกินจำนวนคงเหลือที่จะคืนได้";
            }
        }
        else
        {
            str_result = "ไม่พบรายการวัสดุ - อุปกรณ์นี้ในใบเบิก";
        }

       

        obj_db._fn_closeConn();
        return str_result;
    }





    public void setRunningStock(string str_stock)
    {
        clsConnDb obj_con = new clsConnDb();

        string str_sql = "";

        str_sql = @"update [Inv_Stock] set  Running = Running + 1 where Stock_Id = " + str_stock + " ";

        obj_con._fn_openConn();
        obj_con._fn_queryNoReturn(str_sql);
        obj_con._fn_closeConn();  
    }



    public List<ListItem> _fn_GetStock()
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select Stock_Id,Stock_Name FROM [Inv_Stock]");
        strb_sql.Append(" where Stock_Status ='1' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        while (drd_li_Item.Read())
        {
            ListItem drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Stock_Name"] + "";
            drp_li_Item.Value = drd_li_Item["Stock_Id"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }


    public List<ListItem> _fn_GetType()//บันทึกรับอื่นๆ
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select Reason_ID,Reason_Description FROM [Inv_InOutStk_Reason]");
        strb_sql.Append(" where InOutStk_Type ='I' and InOutStk_Status = '1' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());
        ListItem drp_li_Item = new ListItem();
        drp_li_Item.Text = "--เลือกประเภทการรับ--";
        drp_li_Item.Value = "";
        drp_li_Items.Add(drp_li_Item);
        while (drd_li_Item.Read())
        {
             drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Reason_Description"] + "";
            drp_li_Item.Value = drd_li_Item["Reason_ID"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }


    public List<ListItem> _fn_GetTypePay()
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select Reason_ID,Reason_Description FROM [Inv_InOutStk_Reason]");
        strb_sql.Append(" where InOutStk_Type ='O' and InOutStk_Status = '1' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());
        ListItem drp_li_Item = new ListItem();
        drp_li_Item.Text = "--เลือกประเภทการจ่ายออก--";
        drp_li_Item.Value = "";
        drp_li_Items.Add(drp_li_Item);

        while (drd_li_Item.Read())
        {
            drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Reason_Description"] + "";
            drp_li_Item.Value = drd_li_Item["Reason_ID"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }

    public List<ListItem> _fn_GetSupplier()
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select Supplier_ID,Supplier_Name FROM [Inv_Supplier]");
        strb_sql.Append(" where Supplier_Status = '1' order by Supplier_Name asc ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        ListItem drp_li_Item = new ListItem();
        drp_li_Item.Text = "";
        drp_li_Item.Value = "";
        drp_li_Items.Add(drp_li_Item);

        while (drd_li_Item.Read())
        {
            drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Supplier_Name"] + "";
            drp_li_Item.Value = drd_li_Item["Supplier_ID"] + "";
            drp_li_Items.Add(drp_li_Item);
        }

        

        obj_db._fn_closeConn();
        return drp_li_Items;
    }

    public String _fn_GetIDItem(string str_itemCode)
    {
        string li_Items = "";
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select [Inv_ItemID] FROM [Inv_Item] where [Inv_ItemCode] = '" + str_itemCode + "' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());

        if (li_Item.Read())
        {
             li_Items = li_Item["Inv_ItemID"] + "";
            
        }

        obj_db._fn_closeConn();
        return li_Items;
    }


    public String _fn_GetIDTran(string str_No)
    {
        string li_Items = "";
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select [Transaction_ID] FROM [Inv_TransHead] where [Transaction_No] = '" + str_No + "' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader li_Item = obj_db._fn_query(strb_sql.ToString());

        if (li_Item.Read())
        {
            li_Items = li_Item["Transaction_ID"] + "";

        }

        obj_db._fn_closeConn();
        return li_Items;
    }

    public List<ListItem> _fn_GetPack(string str_itemID)
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select [Pack_ID],[Pack_Description] FROM [Inv_Item_Search]");
        strb_sql.Append(" where [Inv_ItemID] = '" + _fn_GetIDItem(str_itemID) + "' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        while (drd_li_Item.Read())
        {
            ListItem drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Pack_Description"] + "";
            drp_li_Item.Value = drd_li_Item["Pack_ID"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }

    public List<ListItem> _fn_GetProduct(string str_itemName,string str_itemID )
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select Top 100 [Inv_ItemCode],[Inv_ItemName] FROM [Inv_Item]");
        strb_sql.Append(" where [Inv_ItemName] like '%" + str_itemName + "%' and [Inv_ItemCode] like '%" + str_itemID + "%' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        while (drd_li_Item.Read())
        {
            ListItem drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Inv_ItemName"] + "";
            drp_li_Item.Value = drd_li_Item["Inv_ItemCode"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }


    public List<ListItem> _fn_GetUser(string str_user)
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select Top 100 (Account_Fname + ' ' + Account_Lname) as AccountName , [Account_ID] From GPLUZ_ACCOUNT ");
        strb_sql.Append(" where [Account_Fname] + ' ' + [Account_Lname] like '%" + str_user + "%' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        while (drd_li_Item.Read())
        {
            ListItem drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["AccountName"] + "";
            drp_li_Item.Value = drd_li_Item["Account_ID"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }


    public List<ListItem> _fn_GetLocation(string str_stock)
    {
        List<ListItem> drp_li_Items = new List<ListItem>();
        //ListItem drp_li_Item_first = new ListItem();
        //drp_li_drpStatusItem_first.Text = "";
        //drp_li_drpStatusItem_first.Value = "";
        //drp_li_drpStatusItems.Add(drp_li_drpStatusItem_first);
        StringBuilder strb_sql = new StringBuilder(" select [Location_ID],[Location_Name] FROM [Inv_Location] where Stock_ID = '" + str_stock + "' ");
        clsConnDb obj_db = new clsConnDb();
        obj_db._fn_openConn();
        SqlDataReader drd_li_Item = obj_db._fn_query(strb_sql.ToString());

        while (drd_li_Item.Read())
        {
            ListItem drp_li_Item = new ListItem();
            drp_li_Item.Text = drd_li_Item["Location_Name"] + "";
            drp_li_Item.Value = drd_li_Item["Location_ID"] + "";
            drp_li_Items.Add(drp_li_Item);
        }
        obj_db._fn_closeConn();
        return drp_li_Items;
    }

}