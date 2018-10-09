using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Web.Configuration;

/// <summary>
/// Summary description for clsConnDb
/// </summary>
public class clsConnDb
{
    private SqlConnection Obj_conn = null;
    private string Str_connectionString = "";
    public clsConnDb()
    {
        // Str_connectionString = "Data Source=NOPPORN-PC\\MSSQL;Initial Catalog=gPlusDB;User ID=plus;password = 1111" ;
        Str_connectionString = WebConfigurationManager.ConnectionStrings["Default"].ToString();
    }

    public String _fn_GetConnectionString()
    {
        return Str_connectionString;
    }


    public SqlConnection _fn_GetConn()
    {
        return Obj_conn;
    }

    public void _fn_openConn()
    {
        Obj_conn = new SqlConnection(Str_connectionString);
        Obj_conn.Open();

    }
    public void _fn_closeConn()
    {
        Obj_conn.Close();
    }
    public SqlDataReader _fn_query(String str_sql)
    {
        SqlDataReader obj_sqlreader = null;
        SqlCommand obj_command = new SqlCommand(str_sql, Obj_conn);
        obj_sqlreader = obj_command.ExecuteReader();
        return obj_sqlreader;
    }

    public SqlDataReader _fn_query(String str_sql, SqlTransaction obj_tran)
    {
        SqlDataReader obj_sqlreader = null;
        SqlCommand obj_command = new SqlCommand(str_sql, Obj_conn);
        obj_command.Transaction = obj_tran;
        obj_sqlreader = obj_command.ExecuteReader();
        return obj_sqlreader;
    }
    public int _fn_queryNoReturn(String str_sql)
    {
        SqlCommand obj_command = new SqlCommand(str_sql, Obj_conn);
        int int_rowEffect = obj_command.ExecuteNonQuery();
        obj_command.Dispose();
        return int_rowEffect;
    }

    public int _fn_queryNoReturn(String str_sql, SqlTransaction obj_tran)
    {
        SqlCommand obj_command = new SqlCommand(str_sql, Obj_conn);
        obj_command.Transaction = obj_tran;
        int int_rowEffect = obj_command.ExecuteNonQuery();
        obj_command.Dispose();
        return int_rowEffect;
    }

    public DataTable _fn_queryForDataTable(string str_sql)
    {
        DataSet obj_ds = new DataSet();
        SqlCommand obj_command = new SqlCommand(str_sql, Obj_conn);
        obj_command.CommandText = str_sql;
        SqlDataAdapter obj_da = new SqlDataAdapter(obj_command);
        obj_da.Fill(obj_ds);
        return obj_ds.Tables[0];
    }

    /// <summary>
    /// ฟังก์ชั่นสำหรับการ run query มากกว่า 1 query กรณีที่เกิด error exception 
    /// จะทำการ rollback 
    /// </summary>
    /// <param name="str_sql">query string มาต่อกันโดยแต่ละ query จะแบ่งด้วย ;
    /// เช่น  
    ///    DELETE FROM [ReturnDetail] WHERE cReturnID='R001/2554';INSERT INTO  [ReturnDetail](cReturnID,nProductID,fPrice,nQty,cRemark,dtUpdate,bFlag)VALUES('R001/2554',1,400,10,'',getdate(),1)
    ///   
    /// </param>
    /// <returns>bool true if successful and false if unsuccessful</returns>
    public string _fn_runTrans(string str_sql)
    {
        string str_result = "";
        SqlTransaction Trans;
        SqlCommand objCmd = new SqlCommand();
        string[] str_cmd = str_sql.Split(';');

        Trans = Obj_conn.BeginTransaction(IsolationLevel.ReadCommitted);  //*** Start Transaction ***//

        objCmd.Connection = Obj_conn;
        objCmd.Transaction = Trans;  //*** Command & Transaction ***//

        try
        {
            foreach (string sql in str_cmd)
            {
                if (sql != "")
                {
                    objCmd.CommandText = sql;
                    objCmd.CommandType = CommandType.Text;
                    objCmd.ExecuteNonQuery();
                }
            }

            Trans.Commit();  //*** Commit Transaction ***//



        }
        catch (Exception ex)
        {
            Trans.Rollback(); //*** RollBack Transaction ***//
            str_result = ex.Message;

        }


        return str_result;


    }



    public Boolean _fn_runTransForLotReceive(string str_sqlHead, string str_date, string str_doc, string str_stock, string str_user, string str_supType, List<clsOther> list_otherHeadQ, List<clsOther> list_otherQ, List<clsOther> list_otherStockQ)// for head detail
    {
        string str_result = "";
        SqlTransaction Trans;
        SqlCommand objCmd = new SqlCommand();
        SqlDataReader obj_dr = null;
        clsOther obj_other = new clsOther();
        bool bool_onHand = false;
        Trans = Obj_conn.BeginTransaction(IsolationLevel.ReadCommitted);  //*** Start Transaction ***//
        // Trans = Obj_conn.BeginTransaction();  //*** Start Transaction ***//

        objCmd.Connection = Obj_conn;
        objCmd.Transaction = Trans;  //*** Command & Transaction ***//

        try
        {
            string str_sql = "";
            objCmd.CommandText = str_sqlHead;
            objCmd.CommandType = CommandType.Text;
            obj_dr = objCmd.ExecuteReader();
            int InsertID = 0;
            int InsertIDDetail = 0;
            int InsertIDLot = 0;

            if (obj_dr.Read()) InsertID = int.Parse(obj_dr["newID"].ToString());
            obj_dr.Close();
            //str_sqlDetail = str_sqlDetail.Replace("@CustomerID", InsertID.ToString());
            //string[] str_cmd = str_sqlDetail.Split(';');

            for (int i = 0; i < list_otherHeadQ.Count; i++)
            {
                str_sql = "select count(Inv_ItemID) as countTnv from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' and Stock_ID = " + str_stock + " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " ;  ";
                objCmd.CommandText = str_sql;
                objCmd.CommandType = CommandType.Text;
                obj_dr = null;
                obj_dr = objCmd.ExecuteReader();
                if (obj_dr.Read())
                {
                    if (int.Parse(obj_dr["countTnv"].ToString()) > 0)
                    {
                        bool_onHand = true;
                    }
                    else
                    {
                        bool_onHand = false;
                    }
                }
                obj_dr.Close();

                if (bool_onHand == true)
                {
                    str_sql = "insert into Inv_TransDetail ([Transaction_ID],[Inv_ItemId],[Pack_ID],[Qty],[Price_Unit],[Amount]) " +
                         " values('" + InsertID + "','" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "','" + list_otherHeadQ[i].productType + "','" + list_otherHeadQ[i].productQty + "'" +
                         " ,'" + (double.Parse(list_otherHeadQ[i].productTotalPrice) / double.Parse(list_otherHeadQ[i].productQty)) + "','" + list_otherHeadQ[i].productTotalPrice + "') ; SELECT @@IDENTITY AS newID; " +
                         " insert into Inv_Stock_Movement ([Stock_ID],[Transaction_Date],[Inv_ItemID],[Pack_ID],[MoveMent_Type],[Qty_Movement] " +
                         ",[Amount],[Reference_Transaction_Type],[Reference_Transaction_No],[Reference_Transaction_ID],[Reference_Document_No],[Create_Date],[Create_By],Reason_ID) values  ( " +
                         " " + str_stock + ",getdate(),'" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "','" + list_otherHeadQ[i].productType + "' " +
                         " ,'I','" + list_otherHeadQ[i].productQty + "','" + list_otherHeadQ[i].productTotalPrice + "','G' ,'" + obj_other.getRunningStock(str_stock, "G") + "','" + InsertID + "','" + str_doc + "',getdate()," + str_user + ",'" + str_supType + "'  ) ;" +
                         " update [Inv_Stock_OnHand] set OnHand_Qty = OnHand_Qty + " + list_otherHeadQ[i].productQty + ", OnHand_Amount = OnHand_Amount + " + list_otherHeadQ[i].productTotalPrice + " " +
                         ", UnitPrice_Avg = ( OnHand_Amount + " + list_otherHeadQ[i].productTotalPrice + " ) /  ( OnHand_Qty + " + list_otherHeadQ[i].productQty + " ) " +
                         " where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' and Stock_ID = " + str_stock + " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " ; ";
                       
                }
                else
                {
                    str_sql = "insert into Inv_TransDetail ([Transaction_ID],[Inv_ItemId],[Pack_ID],[Qty],[Price_Unit],[Amount]) " +
                         " values('" + InsertID + "','" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "','" + list_otherHeadQ[i].productType + "','" + list_otherHeadQ[i].productQty + "'" +
                         " ,'" + (double.Parse(list_otherHeadQ[i].productTotalPrice) / double.Parse(list_otherHeadQ[i].productQty)) + "','" + list_otherHeadQ[i].productTotalPrice + "') ; SELECT @@IDENTITY AS newID; " +
                         " insert into Inv_Stock_Movement ([Stock_ID],[Transaction_Date],[Inv_ItemID],[Pack_ID],[MoveMent_Type],[Qty_Movement] " +
                         ",[Amount],[Reference_Transaction_Type],[Reference_Transaction_No],[Reference_Document_No],[Create_Date],[Create_By],Reason_ID) values  ( " +
                         " " + str_stock + ",getdate(),'" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "','" + list_otherHeadQ[i].productType + "' " +
                         " ,'I','" + list_otherHeadQ[i].productQty + "','" + list_otherHeadQ[i].productTotalPrice + "','G' ,'" + InsertID + "','" + str_doc + "',getdate()," + str_user + ",'" + str_supType + "'  ) ;" +
                         " insert into [Inv_Stock_OnHand] (OnHand_Qty, OnHand_Amount,UnitPrice_Avg,Inv_ItemID,Stock_ID,Pack_ID ) values(" + list_otherHeadQ[i].productQty + "," + list_otherHeadQ[i].productTotalPrice + " " +
                         "," + list_otherHeadQ[i].productTotalPrice + " / " + list_otherHeadQ[i].productQty + ",'" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "'," + str_stock + "," + list_otherHeadQ[i].productType + " ) ;";
                        

                }

                //" update [Inv_ItemPack] set [Avg_Cost] = case when (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                //       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ) > 0 then (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                //       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ) else 0 end  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                //       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ;";

                //" update [Inv_ItemPack] set [Avg_Cost] = (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                //        " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " )  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                //        " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " ;
                if (str_stock == "1")
                {
                    str_sql += " update [Inv_ItemPack] set [Avg_Cost] = case when (select count(Reason_ID) from [INV_INOUTSTK_REASON] where Reason_ID = '" + str_supType + "' " +
                       " and IsCal_AvgCost =  '1' ) > 0 then (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                       " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " and Stock_id =  " + str_stock + " ) else [Avg_Cost] end  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                       " and [Pack_ID] =  " + list_otherHeadQ[i].productType + "  ;";
                }


                objCmd.CommandText = str_sql;
                objCmd.CommandType = CommandType.Text;
                obj_dr = null;
                obj_dr = objCmd.ExecuteReader();
                InsertIDDetail = 0;
                if (obj_dr.Read()) InsertIDDetail = int.Parse(obj_dr["newID"].ToString());
                obj_dr.Close();


                for (int j = 0; j < list_otherQ.Count; j++)
                {

                    if (list_otherHeadQ[i].productID == list_otherQ[j].lotProduct)
                    {
                        if (list_otherQ[j].expireDateOther != "")
                        {
                            str_sql = " insert into [Inv_Stock_Lot]([Stock_ID],[Inv_ItemID],[Pack_ID],[In_Date],[Expire_Date],[Lot_No],[Lot_Qty],[Unit_Price] " +
                                    ",[Lot_Amount],[Transaction_type], [Transaction_id]) values('" + str_stock + "','" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "','" + list_otherHeadQ[i].productType + "'" +
                                    ",getdate(),'" + _fn_setDateFormatBuddhist(list_otherQ[j].expireDateOther) + "','" + list_otherQ[j].lotOther + "','" + list_otherQ[j].qtyOther + "'" +
                                    ",'" + list_otherQ[j].priceOther + "','" + list_otherQ[j].totalOther + "','G','" + InsertIDDetail + "') ; SELECT @@IDENTITY AS newID; ";
                        }
                        else
                        {
                            str_sql = " insert into [Inv_Stock_Lot]([Stock_ID],[Inv_ItemID],[Pack_ID],[In_Date],[Lot_No],[Lot_Qty],[Unit_Price] " +
                                ",[Lot_Amount],[Transaction_type], [Transaction_id]) values('" + str_stock + "','" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "','" + list_otherHeadQ[i].productType + "'" +
                                ",getdate(),'" + list_otherQ[j].lotOther + "','" + list_otherQ[j].qtyOther + "'" +
                                ",'" + list_otherQ[j].priceOther + "','" + list_otherQ[j].totalOther + "','G','" + InsertIDDetail + "') ; SELECT @@IDENTITY AS newID; ";
                        }

                        objCmd.CommandText = str_sql;
                        objCmd.CommandType = CommandType.Text;
                        obj_dr = null;
                        obj_dr = objCmd.ExecuteReader();
                        InsertIDLot = 0;
                        if (obj_dr.Read()) InsertIDLot = int.Parse(obj_dr["newID"].ToString());
                        obj_dr.Close();


                        for (int k = 0; k < list_otherStockQ.Count; k++)
                        {
                            if (list_otherQ[j].lotOtherNum == list_otherStockQ[k].lotStock && list_otherStockQ[k].lotProductStock == list_otherHeadQ[i].productID)
                            {

                                str_sql = " insert into [Inv_Stock_Lot_Location]([Stock_Lot_ID],[Location_ID],[Qty_Location],[Create_Date],[Create_By]) " +
                                    " values('" + InsertIDLot + "','" + list_otherStockQ[k].idStock + "','" + list_otherStockQ[k].qtyStock + "',getdate()," + str_user + ") ; " +
                                    " insert into Inv_Stock_Lot_Log ([Transaction_Type],[Transaction_ID],[Stock_ID],[Stock_Lot_ID],[Location_ID],[Inv_ItemID],[Pack_ID] " +
                                    ",[Qty_In],[Amount],[Status],[Create_Date],[Create_By] ) values ( " +
                                    " 'G'," + InsertID + "," + str_stock + "," + InsertIDLot + "," + list_otherStockQ[k].idStock + ",'" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "', " +
                                    " '" + list_otherHeadQ[i].productType + "','" + list_otherStockQ[k].qtyStock + "','" + list_otherHeadQ[i].productTotalPrice + "'" +
                                    " ,'1',getdate()," + str_user + " ) ;";



                                objCmd.CommandText = str_sql;
                                objCmd.CommandType = CommandType.Text;
                                objCmd.ExecuteNonQuery();
                            }

                        }

                    }


                }

            }

            Trans.Commit();  //*** Commit Transaction ***//

            return true;


        }
        catch (Exception ex)
        {
            Trans.Rollback(); //*** RollBack Transaction ***//
            str_result = ex.Message;
            return false;
        }

    }



    public Boolean _fn_runDeleteReceive(string str_receive, string str_user)// for head detail
    {
        string str_result = "";
         SqlTransaction Trans;
        SqlCommand objCmd = new SqlCommand();
        clsOther obj_other = new clsOther();
        DataTable obj_tranDetail = new DataTable();
        DataTable obj_lot = new DataTable();
        //Trans = Obj_conn.BeginTransaction(IsolationLevel.ReadCommitted);  //*** Start Transaction ***//
        // Trans = Obj_conn.BeginTransaction();  //*** Start Transaction ***//

        objCmd.Connection = Obj_conn;
        // objCmd.Transaction = Trans;  //*** Command & Transaction ***//

        try
        {
            string str_sql = "";

            str_sql = " update [Inv_TransHead] set [Transaction_Status] = '0',Update_Date = getDate(),Update_By = '" + str_user + "' where [Transaction_No] = '" + str_receive + "' ; ";
                      //" delete from [Inv_Stock_MoveMent] where [Reference_Transaction_ID] = '" + obj_other._fn_GetIDTran(str_receive) + "' and [Reference_Transaction_Type] = 'G' ";

            objCmd.CommandText = str_sql;
            objCmd.CommandType = CommandType.Text;
            objCmd.ExecuteNonQuery();

            str_sql = "select [Inv_TransDetail].*,[Inv_TransHead].Stock_ID,[Inv_TransHead].Refer_Document  " +
                      ",[Inv_TransHead].Transaction_No,[Inv_TransHead].Transaction_Sub_Type " +
                      " from [Inv_TransDetail],[Inv_TransHead] where [Inv_TransDetail].[Transaction_ID] =  '" + obj_other._fn_GetIDTran(str_receive) + "' " +
                      " and [Inv_TransHead].[Transaction_ID] = [Inv_TransDetail].[Transaction_ID] ";
            obj_tranDetail = _fn_queryForDataTable(str_sql);

            for (int i = 0; i < obj_tranDetail.Rows.Count; i++)
            {
                str_sql = "select [Stock_Lot_ID] from [Inv_Stock_Lot] where [Transaction_id]  =  '" + obj_tranDetail.Rows[i]["Transaction_ItemID"].ToString() + "' ";
                obj_lot = _fn_queryForDataTable(str_sql);



                for (int j = 0; j < obj_lot.Rows.Count; j++)
                {

                    str_sql = " update [Inv_Stock_Lot_Location] set [Qty_Out] = [Qty_Location] , Update_Date = getdate(),Update_By = '" + str_user + "'  where [Stock_Lot_ID] = '" + obj_lot.Rows[j]["Stock_Lot_ID"].ToString() + "' ; ";
                    str_sql += " update [INV_STOCK_LOT_LOG] set  Update_Date = getdate(),Update_By = '" + str_user + "',Status = 0  where [Stock_Lot_ID] = '" + obj_lot.Rows[j]["Stock_Lot_ID"].ToString() + "' ";
                    objCmd.CommandText = str_sql;
                    objCmd.CommandType = CommandType.Text;
                    objCmd.ExecuteNonQuery();
                }



                 str_sql = "update [Inv_Stock_Lot] set [Qty_Out] = [Lot_Qty] where [Transaction_id]  =  '" + obj_tranDetail.Rows[i]["Transaction_ItemID"].ToString() + "' ; " +
                           " insert into Inv_Stock_Movement ([Stock_ID],[Transaction_Date],[Inv_ItemID],[Pack_ID],[MoveMent_Type],[Qty_Movement] " +
                           ",[Amount],[Reference_Transaction_Type],[Reference_Transaction_No],[Reference_Transaction_ID],[Reference_Document_No],[Create_Date],[Create_By],Reason_ID) values  ( " +
                           " " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + ",getdate(),'" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "','" + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + "' " +
                           " ,'O','" + obj_tranDetail.Rows[i]["Qty"].ToString() + "','" + obj_tranDetail.Rows[i]["Amount"].ToString() + "','G' ,'" + obj_tranDetail.Rows[i]["Transaction_No"].ToString() + "' " +
                           ",'" + obj_tranDetail.Rows[i]["Transaction_ID"].ToString() + "','" + obj_tranDetail.Rows[i]["Refer_Document"].ToString() + "',getdate()," + str_user + "," + obj_tranDetail.Rows[i]["Transaction_Sub_Type"].ToString() + "  ) ;" +    
                           " update [Inv_Stock_OnHand] set OnHand_Qty = OnHand_Qty - " + obj_tranDetail.Rows[i]["Qty"].ToString() + ", OnHand_Amount = OnHand_Amount -" + obj_tranDetail.Rows[i]["Amount"].ToString() + " " +
                         ", UnitPrice_Avg = case when ( OnHand_Qty - " + obj_tranDetail.Rows[i]["Qty"].ToString() + " ) = 0  then 0 else ( OnHand_Amount - " + obj_tranDetail.Rows[i]["Amount"].ToString() + " ) /  ( OnHand_Qty - " + obj_tranDetail.Rows[i]["Qty"].ToString() + " ) end " +
                         " where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' and Stock_ID = " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ; ";

                //str_sql = "update [Inv_Stock_Lot] set [Qty_Out] = [Lot_Qty] where [Transaction_id]  =  '" + obj_tranDetail.Rows[i]["Transaction_ItemID"].ToString() + "' ; " +
                //         " update [Inv_Stock_OnHand] set OnHand_Qty = OnHand_Qty - " + obj_tranDetail.Rows[i]["Qty"].ToString() + ", OnHand_Amount = OnHand_Amount -" + obj_tranDetail.Rows[i]["Amount"].ToString() + " " +
                //         ", UnitPrice_Avg = ( OnHand_Amount - " + obj_tranDetail.Rows[i]["Amount"].ToString() + " ) /  ( OnHand_Qty - " + obj_tranDetail.Rows[i]["Qty"].ToString() + " ) " +
                //         " where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' and Stock_ID = " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ; ";
                       
                        //" update [Inv_ItemPack] set [Avg_Cost] = case when (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                         //" and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ) > 0 then (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                         //" and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ) else 0 end  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                         //" and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ;";


                if (obj_tranDetail.Rows[i]["Stock_ID"].ToString() == "1")
                {
                    str_sql += " update [Inv_ItemPack] set [Avg_Cost] = case when (select count(Reason_ID) from [INV_INOUTSTK_REASON] where Reason_ID = '" + obj_tranDetail.Rows[i]["Transaction_Sub_Type"].ToString() + "' " +
                       " and IsCal_AvgCost =  '1' ) > 0 then (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " and Stock_id =  " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + " ) else [Avg_Cost] end  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ;";
                }

                objCmd.CommandText = str_sql;
                objCmd.CommandType = CommandType.Text;
                objCmd.ExecuteNonQuery();

            }

            //  Trans.Commit();  //*** Commit Transaction ***//

            return true;


        }
        catch (Exception ex)
        {
            //  Trans.Rollback(); //*** RollBack Transaction ***//
            str_result = ex.Message;
            return false;
        }

    }



    public string _fn_runTransForLotPay(string str_sqlHead, string str_date, string str_doc, string str_stock, string str_user, string str_subType, List<clsOther> list_otherHeadQ, List<clsOther> list_otherQ, List<clsOther> list_otherStockQ)// for head detail
    {
        string str_result = "";
        SqlTransaction Trans;
        SqlCommand objCmd = new SqlCommand();
        SqlDataReader obj_dr = null;
        SqlDataReader obj_dr2 = null;
        clsOther obj_other = new clsOther();
        Trans = Obj_conn.BeginTransaction(IsolationLevel.ReadCommitted);  //*** Start Transaction ***//
        // Trans = Obj_conn.BeginTransaction();  //*** Start Transaction ***//

        objCmd.Connection = Obj_conn;
        objCmd.Transaction = Trans;  //*** Command & Transaction ***//

        try
        {
            string str_sql = "";
            objCmd.CommandText = str_sqlHead;
            objCmd.CommandType = CommandType.Text;
            obj_dr = objCmd.ExecuteReader();
            int InsertID = 0;
            int InsertIDDetail = 0;
            int InsertIDLot = 0;
            if (obj_dr.Read()) InsertID = int.Parse(obj_dr["newID"].ToString());
            obj_dr.Close();
            //str_sqlDetail = str_sqlDetail.Replace("@CustomerID", InsertID.ToString());
            //string[] str_cmd = str_sqlDetail.Split(';');

            for (int i = 0; i < list_otherHeadQ.Count; i++)
            {
                str_sql = "insert into Inv_TransDetail ([Transaction_ID],[Inv_ItemId],[Pack_ID],[Qty],[Price_Unit],[Amount]) " +
                         " values('" + InsertID + "','" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "','" + list_otherHeadQ[i].productType + "','" + list_otherHeadQ[i].productQty + "'" +
                         " ,'" + (double.Parse(list_otherHeadQ[i].productTotalPrice) / double.Parse(list_otherHeadQ[i].productQty)) + "','" + list_otherHeadQ[i].productTotalPrice + "') ; SELECT @@IDENTITY AS newID; " +
                         " insert into Inv_Stock_Movement ([Stock_ID],[Transaction_Date],[Inv_ItemID],[Pack_ID],[MoveMent_Type],[Qty_Movement] " +
                         ",[Amount],[Reference_Transaction_Type],[Reference_Transaction_No],[Reference_Transaction_ID],[Reference_Document_No],[Create_Date],[Create_By],Reason_ID) values  ( " +
                         " " + str_stock + ",getdate(),'" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "','" + list_otherHeadQ[i].productType + "' " +
                         " ,'O','" + list_otherHeadQ[i].productQty + "','" + list_otherHeadQ[i].productTotalPrice + "','U' ,'" + obj_other.getRunningStock(str_stock, "U") + "','" + InsertID + "','" + str_doc + "',getdate()," + str_user + ",'" + str_subType + "' ) ;" +
                         " update [Inv_Stock_OnHand] set OnHand_Qty = OnHand_Qty - " + list_otherHeadQ[i].productQty + ", OnHand_Amount = OnHand_Amount - " + list_otherHeadQ[i].productTotalPrice + " " +
                         ", UnitPrice_Avg = case when (OnHand_Qty - " + list_otherHeadQ[i].productQty + " ) = 0 then 0 else ( OnHand_Amount - " + list_otherHeadQ[i].productTotalPrice + " ) /  ( OnHand_Qty - " + list_otherHeadQ[i].productQty + " ) end " +
                         " where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' and Stock_ID = " + str_stock + " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " ; " ;

                if (str_stock == "1")
                {
                    str_sql += " update [Inv_ItemPack] set [Avg_Cost] = case when (select count(Reason_ID) from [INV_INOUTSTK_REASON] where Reason_ID = '" + str_subType + "' " +
                       " and IsCal_AvgCost =  '1' ) > 0 then (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                       " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " and Stock_id =  " + str_stock + " ) else [Avg_Cost] end  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                       " and [Pack_ID] =  " + list_otherHeadQ[i].productType + "  ;";
                }

                
                // " update [Inv_ItemPack] set [Avg_Cost] = (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                         // " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " )  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_other._fn_GetIDItem(list_otherHeadQ[i].productID) + "' " +
                         // " and [Pack_ID] =  " + list_otherHeadQ[i].productType + " ;";
                objCmd.CommandText = str_sql;
                objCmd.CommandType = CommandType.Text;
                obj_dr = null;
                obj_dr = objCmd.ExecuteReader();
                InsertIDDetail = 0;
                if (obj_dr.Read()) InsertIDDetail = int.Parse(obj_dr["newID"].ToString());
                obj_dr.Close();



                for (int j = 0; j < list_otherQ.Count; j++)
                {

                    if (list_otherHeadQ[i].productID == list_otherQ[j].lotProduct)
                    {
                        //str_sql = " SELECT count([Stock_Lot_ID]) as countTnv,Stock_Lot_ID  from [Inv_Stock_Lot] where [Stock_ID] = '" + str_stock + "' " +
                        //        " and ([Lot_Qty] - [Qty_Out]) >= " + list_otherQ[j].qtyOther + " and  [Inv_ItemID] = '" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "' and [Pack_ID] = '" + list_otherHeadQ[i].productType + "' group by Stock_Lot_ID ; ";

                        str_sql = " SELECT count([Inv_Stock_Lot].[Stock_Lot_ID]) as countTnv,[Inv_Stock_Lot].Inv_ItemID from [Inv_Stock_Lot],[INV_STOCK_LOT_LOCATION] where [Inv_Stock_Lot].Stock_Lot_ID = [INV_STOCK_LOT_LOCATION].Stock_Lot_ID and  [Stock_ID] = '" + str_stock + "'  " +
                                "  and  [Inv_ItemID] = '" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "' and ([INV_STOCK_LOT_LOCATION].[Qty_Location]) - ([INV_STOCK_LOT_LOCATION].[Qty_Out]) > 0 and [Pack_ID] = '" + list_otherHeadQ[i].productType + "' group by [Inv_Stock_Lot].Inv_ItemID  having (SUM([INV_STOCK_LOT_LOCATION].[Qty_Location]) - sum([INV_STOCK_LOT_LOCATION].[Qty_Out]))  >= " + list_otherQ[j].qtyOther + " ; ";

                        objCmd.CommandText = str_sql;
                        objCmd.CommandType = CommandType.Text;
                        obj_dr = null;
                        obj_dr = objCmd.ExecuteReader();
                        InsertIDLot = 0;
                        if (obj_dr.Read())
                        {
                            if (int.Parse(obj_dr["countTnv"].ToString()) <= 0)
                            {
                                obj_dr.Close();
                                Trans.Rollback();
                                return "Not";
                            }
                            else
                            {
                                obj_dr.Close();
                                str_sql = " SELECT count([Inv_Stock_Lot].[Stock_Lot_ID]) as countTnv,[Inv_Stock_Lot].Stock_Lot_ID ,SUM(Inv_Stock_Lot.Lot_Qty) - SUM(Inv_Stock_Lot.Qty_Out) as total_qty from [Inv_Stock_Lot],[INV_STOCK_LOT_LOCATION] where [Inv_Stock_Lot].Stock_Lot_ID = [INV_STOCK_LOT_LOCATION].Stock_Lot_ID and  [Stock_ID] = '" + str_stock + "'  " +
                                " and  [Inv_ItemID] = '" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "'and ([INV_STOCK_LOT_LOCATION].[Qty_Location]) - ([INV_STOCK_LOT_LOCATION].[Qty_Out]) > 0 and [Pack_ID] = '" + list_otherHeadQ[i].productType + "'  group by [Inv_Stock_Lot].Stock_Lot_ID ; ";

                                objCmd.CommandText = str_sql;
                                objCmd.CommandType = CommandType.Text;
                                obj_dr = null;
                                obj_dr = objCmd.ExecuteReader();

                                int sum = int.Parse(list_otherQ[j].qtyOther) ;
                                while (obj_dr.Read())
                                {
                                    if(sum > 0)
                                    {
                                        InsertIDLot = int.Parse(obj_dr["Stock_Lot_ID"].ToString());

                                        if (sum > int.Parse(obj_dr["total_qty"].ToString()))
                                        {
                                            str_sql += " update [Inv_Stock_Lot] set Qty_Out = Lot_Qty where  [Stock_ID] = '" + str_stock + "' " +
                                          " and  [Inv_ItemID] = '" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "' and [Pack_ID] = '" + list_otherHeadQ[i].productType + "'  and  [Stock_Lot_ID] = '" + obj_dr["Stock_Lot_ID"].ToString() + "' ;";
                                            sum = sum - int.Parse(obj_dr["total_qty"].ToString());


                                       
                                             str_sql += "update [Inv_Stock_Lot_Location] set Qty_Out = Qty_Out + " + obj_dr["total_qty"].ToString() + " where [Location_ID] = 1 and Stock_Lot_ID = " + InsertIDLot + "; " +
                                                            " insert into Inv_Stock_Lot_Log ([Transaction_Type],[Transaction_ID],[Stock_ID],[Stock_Lot_ID],[Location_ID],[Inv_ItemID],[Pack_ID] " +
                                                            ",[Qty_Out],[Amount],[Status],[Create_Date],[Create_By] ) values ( " +
                                                            " 'U'," + InsertID + "," + str_stock + "," + InsertIDLot + ",1,'" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "', " +
                                                            " '" + list_otherHeadQ[i].productType + "','" + obj_dr["total_qty"].ToString() + "','" + (double.Parse(list_otherHeadQ[i].productTotalPrice) / double.Parse(list_otherHeadQ[i].productQty)) * double.Parse(obj_dr["total_qty"].ToString()) + "'" +
                                                            " ,'1',getdate()," + str_user + " ) ; ";

                                                    
                                                       

                                        }
                                        else
                                        {
                                            str_sql += " update [Inv_Stock_Lot] set Qty_Out = Qty_Out + " + sum + " where  [Stock_ID] = '" + str_stock + "' " +
                                          " and  [Inv_ItemID] = '" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "' and [Pack_ID] = '" + list_otherHeadQ[i].productType + "'  and  [Stock_Lot_ID] = '" + obj_dr["Stock_Lot_ID"].ToString() + "' ;";

                                            str_sql += "update [Inv_Stock_Lot_Location] set Qty_Out = Qty_Out + " + sum + " where [Location_ID] = 1 and Stock_Lot_ID = " + InsertIDLot + "; " +
                                                            " insert into Inv_Stock_Lot_Log ([Transaction_Type],[Transaction_ID],[Stock_ID],[Stock_Lot_ID],[Location_ID],[Inv_ItemID],[Pack_ID] " +
                                                            ",[Qty_Out],[Amount],[Status],[Create_Date],[Create_By] ) values ( " +
                                                            " 'U'," + InsertID + "," + str_stock + "," + InsertIDLot + ",1,'" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "', " +
                                                            " '" + list_otherHeadQ[i].productType + "','" + sum + "','" + (double.Parse(list_otherHeadQ[i].productTotalPrice) / double.Parse(list_otherHeadQ[i].productQty)) * double.Parse(sum.ToString()) + "'" +
                                                            " ,'1',getdate()," + str_user + " ) ; ";
                                            sum = 0;
                                        }

                                    }
                                    
                                }

                                obj_dr.Close();
                            }


                            

                        }
                        else
                        {
                            obj_dr.Close();
                            Trans.Rollback();
                            return "Not";
                        }



                        /*  for (int k = 0; k < list_otherStockQ.Count; k++)
                          {
                              //if (list_otherQ[j].lotOther == list_otherStockQ[k].lotStock)
                              if (list_otherQ[j].lotOtherNum == list_otherStockQ[k].lotStock && list_otherStockQ[k].lotProductStock == list_otherHeadQ[i].productID)
                              {

                                  str_sql += "update [Inv_Stock_Lot_Location] set Qty_Out = Qty_Out + " + list_otherStockQ[k].qtyStock + " where [Location_ID] = '" + list_otherStockQ[k].idStock + "' and Stock_Lot_ID = " + InsertIDLot + "; " +
                                          " insert into Inv_Stock_Lot_Log ([Transaction_Type],[Transaction_ID],[Stock_ID],[Stock_Lot_ID],[Location_ID],[Inv_ItemID],[Pack_ID] " +
                                          ",[Qty_Out],[Amount],[Status],[Create_Date],[Create_By] ) values ( " +
                                          " 'U'," + InsertID + "," + str_stock + "," + InsertIDLot + "," + list_otherStockQ[k].idStock + ",'" + obj_other._fn_GetIDItem(list_otherQ[j].lotProduct) + "', " +
                                          " '" + list_otherHeadQ[i].productType + "','" + list_otherStockQ[k].qtyStock + "','" + (double.Parse(list_otherHeadQ[i].productTotalPrice) / double.Parse(list_otherHeadQ[i].productQty)) * double.Parse(list_otherStockQ[k].qtyStock) + "'" +
                                          " ,'1',getdate()," + str_user + " ) ; ";

                                  objCmd.CommandText = " select count(Stock_Lot_ID) as countTnv from Inv_Stock_Lot_Location  " +
                                          " where [Location_ID] = '" + list_otherStockQ[k].idStock + "' and Stock_Lot_ID = " + InsertIDLot + "  group by Stock_Lot_ID ; ";
                                  objCmd.CommandType = CommandType.Text;
                                  obj_dr2 = null;
                                  obj_dr2 = objCmd.ExecuteReader();
                                  //  InsertIDLot = 0;
                                  if (obj_dr2.Read())
                                  {
                                      if (int.Parse(obj_dr["countTnv"].ToString()) <= 0)
                                      {
                                          obj_dr2.Close();
                                          Trans.Rollback();
                                          return "NotLo";
                                      }

                                  }
                                  else
                                  {
                                      obj_dr2.Close();
                                      Trans.Rollback();
                                      return "Not";
                                  }

                                  obj_dr2.Close();
                              }
                       
                    

                          }*/



                        objCmd.CommandText = str_sql;
                        objCmd.CommandType = CommandType.Text;
                        obj_dr = null;
                        obj_dr = objCmd.ExecuteReader();
                        InsertIDLot = 0;
                        obj_dr.Close();
                    }


                }

            }

            Trans.Commit();  //*** Commit Transaction ***//

            return "True";


        }
        catch (Exception ex)
        {
            Trans.Rollback(); //*** RollBack Transaction ***//
            str_result = ex.Message;
            return "False";
        }

    }


    public Boolean _fn_runDeletePay(string str_pay, string str_user)// for head detail
    {
        string str_result = "";
        // SqlTransaction Trans;
        SqlCommand objCmd = new SqlCommand();
        clsOther obj_other = new clsOther();
        DataTable obj_tranDetail = new DataTable();
        DataTable obj_lot = new DataTable();
        DataTable obj_lotStock = new DataTable();
        //Trans = Obj_conn.BeginTransaction(IsolationLevel.ReadCommitted);  //*** Start Transaction ***//
        // Trans = Obj_conn.BeginTransaction();  //*** Start Transaction ***//

        objCmd.Connection = Obj_conn;
        // objCmd.Transaction = Trans;  //*** Command & Transaction ***//

        try
        {
            string str_sql = "";

            str_sql = " update [Inv_TransHead] set [Transaction_Status] = '0',Update_Date = getDate(),Update_By = '" + str_user + "' where [Transaction_No] = '" + str_pay + "' ; ";
                    //  " delete from [Inv_Stock_MoveMent] where [Reference_Transaction_ID] = '" + obj_other._fn_GetIDTran(str_pay) + "' and [Reference_Transaction_Type] = 'U' ";

            objCmd.CommandText = str_sql;
            objCmd.CommandType = CommandType.Text;
            objCmd.ExecuteNonQuery();

            //str_sql = "select [Transaction_ItemID],[Transaction_ID],[Inv_ItemId],[Qty],[Amount],[Pack_ID],(select [Stock_ID] from [Inv_TransHead] where [Inv_TransHead].[Transaction_ID] = [Inv_TransDetail].[Transaction_ID] ) as Stock_ID  " +
            //          ",(select [Transaction_Sub_Type] from [Inv_TransHead] where [Inv_TransHead].[Transaction_ID] = [Inv_TransDetail].[Transaction_ID] ) as Sub_type from [Inv_TransDetail] where [Transaction_ID] =  '" + obj_other._fn_GetIDTran(str_pay) + "' ";

            str_sql = "select [Inv_TransDetail].*,[Inv_TransHead].Stock_ID,[Inv_TransHead].Refer_Document  " +
                   ",[Inv_TransHead].Transaction_No,[Inv_TransHead].Transaction_Sub_Type " +
                   " from [Inv_TransDetail],[Inv_TransHead] where [Inv_TransDetail].[Transaction_ID] =  '" + obj_other._fn_GetIDTran(str_pay) + "' " +
                   " and [Inv_TransHead].[Transaction_ID] = [Inv_TransDetail].[Transaction_ID] ";

            obj_tranDetail = _fn_queryForDataTable(str_sql);

            for (int i = 0; i < obj_tranDetail.Rows.Count; i++)
            {
                str_sql = "select * from [INV_STOCK_LOT_LOG] where [Transaction_id]  =  '" + obj_tranDetail.Rows[i]["Transaction_ID"].ToString() + "' and  " +
                          " [Inv_ItemID] =  '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' ";
                obj_lot = _fn_queryForDataTable(str_sql);

                str_sql = " insert into Inv_Stock_Movement ([Stock_ID],[Transaction_Date],[Inv_ItemID],[Pack_ID],[MoveMent_Type],[Qty_Movement] " +
                           ",[Amount],[Reference_Transaction_Type],[Reference_Transaction_No],[Reference_Transaction_ID],[Reference_Document_No],[Create_Date],[Create_By],Reason_ID) values  ( " +
                           " " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + ",getdate(),'" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "','" + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + "' " +
                           " ,'I','" + obj_tranDetail.Rows[i]["Qty"].ToString() + "','" + obj_tranDetail.Rows[i]["Amount"].ToString() + "','U' ,'" + obj_tranDetail.Rows[i]["Transaction_No"].ToString() + "' " +
                           ",'" + obj_tranDetail.Rows[i]["Transaction_ID"].ToString() + "','" + obj_tranDetail.Rows[i]["Refer_Document"].ToString() + "',getdate()," + str_user + "," + obj_tranDetail.Rows[i]["Transaction_Sub_Type"].ToString() + "  ) ;" +  
                          " update [Inv_Stock_OnHand] set OnHand_Qty = OnHand_Qty + " + obj_tranDetail.Rows[i]["Qty"].ToString() + ", OnHand_Amount = OnHand_Amount + " + obj_tranDetail.Rows[i]["Amount"].ToString() + " " +
                          ", UnitPrice_Avg = ( OnHand_Amount + " + obj_tranDetail.Rows[i]["Amount"].ToString() + " ) /  ( OnHand_Qty + " + obj_tranDetail.Rows[i]["Qty"].ToString() + " ) " +
                          " where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' and Stock_ID = " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ; " ;
         
                          //" update [Inv_ItemPack] set [Avg_Cost] = (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                          //" and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " )  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' and [Pack_ID] = " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ; ";

                if (obj_tranDetail.Rows[i]["Stock_ID"].ToString() == "1")
                {
                    str_sql += " update [Inv_ItemPack] set [Avg_Cost] = case when (select count(Reason_ID) from [INV_INOUTSTK_REASON] where Reason_ID = '" + obj_tranDetail.Rows[i]["Transaction_Sub_Type"].ToString() + "' " +
                       " and IsCal_AvgCost =  '1' ) > 0 then (select top 1 UnitPrice_Avg from [Inv_Stock_OnHand] where Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " and Stock_id =  " + obj_tranDetail.Rows[i]["Stock_ID"].ToString() + " ) else [Avg_Cost] end  , [Avg_Cost_Date] = getdate()  where  Inv_ItemID = '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' " +
                       " and [Pack_ID] =  " + obj_tranDetail.Rows[i]["Pack_ID"].ToString() + " ;";
                }


                objCmd.CommandText = str_sql;
                objCmd.CommandType = CommandType.Text;
                objCmd.ExecuteNonQuery();

                for (int j = 0; j < obj_lot.Rows.Count; j++)
                {
                    str_sql = "select * from [INV_STOCK_LOT_LOG] where [Transaction_id]  =  '" + obj_tranDetail.Rows[i]["Transaction_ID"].ToString() + "' and  " +
                          " [Inv_ItemID] =  '" + obj_tranDetail.Rows[i]["Inv_ItemId"].ToString() + "' and  Stock_Lot_ID = '" + obj_lot.Rows[j]["Stock_Lot_ID"].ToString() + "' ";
                    obj_lotStock = _fn_queryForDataTable(str_sql);

                    str_sql = " update INV_STOCK_LOT set [Qty_Out] = [Qty_Out] - " + obj_lot.Rows[j]["Qty_Out"].ToString() + " where [Stock_Lot_ID] = '" + obj_lot.Rows[j]["Stock_Lot_ID"].ToString() + "' ";
                    //str_sql = " delete from [Inv_Stock_Lot_Location] where [Stock_Lot_ID] = '" + obj_lot.Rows[i]["Stock_Lot_ID"].ToString() + "' ";
                    objCmd.CommandText = str_sql;
                    objCmd.CommandType = CommandType.Text;
                    objCmd.ExecuteNonQuery();

                    for (int k = 0; k < obj_lotStock.Rows.Count; k++)
                    {

                        str_sql = " update INV_STOCK_LOT_LOCATION set [Qty_Out] = [Qty_Out] - " + obj_lotStock.Rows[k]["Qty_Out"].ToString() + " where [Stock_Lot_ID] = '" + obj_lotStock.Rows[k]["Stock_Lot_ID"].ToString() + "' and [Location_ID] =  '" + obj_lotStock.Rows[k]["Location_ID"].ToString() + "' ";
                        //str_sql = " delete from [Inv_Stock_Lot_Location] where [Stock_Lot_ID] = '" + obj_lot.Rows[i]["Stock_Lot_ID"].ToString() + "' ";
                        objCmd.CommandText = str_sql;
                        objCmd.CommandType = CommandType.Text;
                        objCmd.ExecuteNonQuery();
                    }
                }

            }

            //  Trans.Commit();  //*** Commit Transaction ***//

            return true;


        }
        catch (Exception ex)
        {
            //  Trans.Rollback(); //*** RollBack Transaction ***//
            str_result = ex.Message;
            return false;
        }

    }



    public SqlDataReader _fn_queryByCmd(SqlCommand obj_command) //by narm
    {
        SqlDataReader obj_sqlreader = null;
        obj_command.Connection = Obj_conn;
        obj_sqlreader = obj_command.ExecuteReader();
        return obj_sqlreader;
    }

    public string convertDate(string str_date)
    {
        str_date = str_date.Substring(6, 4) + "-" + str_date.Substring(3, 2) + "-" + str_date.Substring(0, 2);

        return str_date;
    }


    public string _fn_setDateFormatBuddhist(string sEngdate) //แปลง พศ เป็น คศ
    {
        string sResult = "";

        if (sEngdate == null || sEngdate == "")
        {
            return null;
        }
        else if (sEngdate.Length >= 10)
        {
            int int_date = int.Parse(sEngdate.Substring(6, 4));

            if(int_date > 2500)
            {
                int_date = int_date - 543;
            }

            sResult = int_date.ToString() + "-" + sEngdate.Substring(3, 2) + "-" + sEngdate.Substring(0, 2);
        }

        return sResult;
    }


    public string _fn_setDateFormatChirtForWeb(string sEngdate) //แปลง คศ เป็น พศ
    {
        string year, sResult = DateTime.Now.ToString("yyyy/mm/dd");
        if (sEngdate != "")
        {
            sEngdate = DateTime.Parse(sEngdate).ToShortDateString();

            string[] dmy = sEngdate.Split('/');
            if (dmy.Length > 1)
            {
                if (int.Parse(dmy[2]) < 2500)
                {
                    year = (int.Parse(dmy[2]) + 543).ToString();
                }
                else
                {
                    year = dmy[2];
                }
                sResult = dmy[0] + "/" + dmy[1] + "/" + year;
            }
        }
        else
        {
            sResult = "";
        }
        return sResult;

    }

}