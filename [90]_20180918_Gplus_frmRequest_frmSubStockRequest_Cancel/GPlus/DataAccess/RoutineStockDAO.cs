using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class RoutineStockDAO : DbConnectionBase
    {

        public RoutineStockDAO()
        {
        }


        public bool InsertSummary(List<SqlParameter> param, DataTable dt, DataTable dt_Id_Request, string UserID)
        {
            //if (dtRequest.Rows.Count == 0 || dtReqItem.Select("[Is_Delete]='N'").Length == 0) return -1;
            
            bool chk = false;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            //DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update

            int chk_Data_All = 1 + (dt.Rows.Count * 2) + dt_Id_Request.Rows.Count;
            int chk_Data = 0;

            string Summary_ReqId = "";
            try
            {
                this.BeginParameter();

                foreach (SqlParameter data in param)
                {
                    this.Parameters.Add(data);
                }

                int result = this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_Insert", this.Parameters);

                //int result = new DatabaseHelper().ExecuteNonQuery("sp_INV_SUMMARYREQ_Insert", param);

                if (result > 0)
                {
                    Summary_ReqId = param[0].Value.ToString();

                    chk_Data += 1;

                    //insert table 2


                    //int result2 = new DatabaseHelper().ExecuteNonQuery("sp_INV_SUMMARYREQ_Insert", param2);

                    foreach (DataRow drRow in dt_Id_Request.Rows)
                    {
                        this.BeginParameter();
                        // ****** IMPORTANT Scope for set req item id *******
                        // NULL / Empty = Insert
                        // NOT NULL = Update
                        
                        //*************************************************
                        this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(Summary_ReqId)));
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_DETAIL_Insert", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                        //chk_Data +=  this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_DETAIL_Insert", this.Parameters);
                    }


                    //insert table 3

                    foreach (DataRow drRow in dt.Rows)
                    {
                        this.BeginParameter();
                        
                        this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(Summary_ReqId)));
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));
                        

                        if (drRow["ID_TYPE"].ToString() == "1")
                        {
                            this.Parameters.Add(Parameter("@Stock_Id", Convert.ToInt32(drRow["ID"])));
                        }
                        else
                        {
                            this.Parameters.Add(Parameter("@OrgStruc_Id", Convert.ToInt32(drRow["ID"])));
                        }
                        
                        this.Parameters.Add(Parameter("@Inv_ItemId", Convert.ToInt32(drRow["Inv_ItemId"])));
                        this.Parameters.Add(Parameter("@Pack_Id", Convert.ToInt32(drRow["Pack_Id"])));
                        this.Parameters.Add(Parameter("@Order_Qty", Convert.ToInt32(drRow["ItemQTY"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_ITEM_Insert", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                        //chk_Data +=  this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_ITEM_Insert", this.Parameters);
                    }


                    //Update Inv_Request

                    foreach (DataRow drRow in dt.Rows)
                    {
                        this.BeginParameter();

                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));
                        this.Parameters.Add(Parameter("@Request_Status", "3"));
                        this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));


                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                        //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);
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
                else
                {
                    chk = false;
                    this.Rollback();
                }


            }
            catch (Exception ex)
            {
                this.Rollback();
                //this.CommitTransaction();// Request commit transaction
            }
            //finally
            //{
            //    this.CommitTransaction();// Request commit transaction
            //}

            return chk;
            
        }



        public bool UpdateSummary(DataTable dt, string Summary_ReqId, string UserID)
        {
            bool chk = false;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            //DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update

            int chk_Data_All = 2 + dt.Rows.Count;
            int chk_Data = 0;
            int result = 0;

            try
            {

            /* UPDATE Table 1
                UPDATE  Inv_SummaryReq
                 SET   Status   =  '2'
                WHERE  Summar_ReqId  = ?
            */


            this.BeginParameter();

            this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(Summary_ReqId)));
            this.Parameters.Add(Parameter("@Status", "2"));
            this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

            result = 0;

            result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq", this.Parameters);

            if (result > 0)
                chk_Data += result;
            else
            {
                this.Rollback();
                return false;
            }

            //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq", this.Parameters);


            /* UPDATE Table 2
                 	 UPDATE Inv_SummaryReq_Item
                     SET  Allocate_Qty  =  ?จำนวนที่อยู่ในช่อง จ่าย – ของแต่ละหน่วยงาน/คลัง,
                            On_Hand_Qty_After_Allocate   =  ?จำนวนสินค้าตามหน่วยบรรจุนั้นในคลังที่จ่าย
                     WHERE   ตาม Primary Key

            */


            foreach (DataRow drRow in dt.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update

                //*************************************************
                this.Parameters.Add(Parameter("@Summary_ReqItem_Id", Convert.ToInt32(drRow["Summary_ReqItem_Id"])));
                this.Parameters.Add(Parameter("@Allocate_Qty", Convert.ToInt32(drRow["Allocate_Qty"])));
                this.Parameters.Add(Parameter("@On_Hand_Qty_After_Allocate", Convert.ToInt32(drRow["OnHand_Qty"])));

                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);
            }




            /*
             UPDATE Table 3 
                Update Inv_Request 
                SET   Request_Status   =  '4' 
             */

            this.BeginParameter();

            this.Parameters.Add(Parameter("@Summary_ReqId", Summary_ReqId));
            this.Parameters.Add(Parameter("@Request_Status", "4"));
            this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

            result = 0;

            result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);

            if (result > 0)
                chk_Data += result;
            else
            {
                this.Rollback();
                return false;
            }

            //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);


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
                //this.CommitTransaction();// Request commit transaction
            }
            //finally
            //{
            //    this.CommitTransaction();// Request commit transaction
            //}

            return chk;

        }



        public bool UpdateSummary_Modify(List<SqlParameter> param, DataTable dt, DataTable dtTemp_insert_req, DataTable dtTemp_delete_req, DataTable dtTemp_delete_item, string Summary_ReqId, string UserID)
        {
            bool chk = false;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            //DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update

            int chk_Data_All = 2 + dt.Rows.Count + dtTemp_insert_req.Rows.Count + dtTemp_delete_req.Rows.Count + dtTemp_delete_item.Rows.Count;
            int chk_Data = 0;
            int result = 0;

            try
            {

                /* UPDATE Table 1
                    UPDATE  Inv_SummaryReq
                */


                this.BeginParameter();

                foreach (SqlParameter data in param)
                {
                    this.Parameters.Add(data);
                }

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_modify", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                /* UPDATE Table 2
                    Insert  Inv_SummaryReq_Detail
                */

                if (dtTemp_insert_req.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dtTemp_insert_req.Rows)
                    {
                        this.BeginParameter();
                        
                        this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(drRow["Summary_ReqId"])));
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_DETAIL_Insert", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                        //chk_Data +=  this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_DETAIL_Insert", this.Parameters);
                    }
                }

                /* UPDATE Table 3
                    Delete  Inv_SummaryReq_Detail */


                if (dtTemp_delete_req.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dtTemp_delete_req.Rows)
                    {
                        this.BeginParameter();

                        this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(drRow["Summary_ReqId"])));
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_Delete_INV_SUMMARYREQ_DETAIL", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                        //chk_Data +=  this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_DETAIL_Insert", this.Parameters);
                    }
                }

                

                /* UPDATE Table 5
                         UPDATE Inv_SummaryReq_Item
                         SET  Allocate_Qty  =  ?จำนวนที่อยู่ในช่อง จ่าย – ของแต่ละหน่วยงาน/คลัง,
                                On_Hand_Qty_After_Allocate   =  ?จำนวนสินค้าตามหน่วยบรรจุนั้นในคลังที่จ่าย
                         WHERE   ตาม Primary Key

                */


                foreach (DataRow drRow in dt.Rows)
                {
                    if (drRow.IsNull("Summary_ReqItem_Id") || drRow["Summary_ReqItem_Id"].ToString() == "")
                    {
                        // insert SummaryReq_Item

                        this.BeginParameter();

                        this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(Summary_ReqId)));
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));


                        if (drRow["ID_TYPE"].ToString() == "1")
                        {
                            this.Parameters.Add(Parameter("@Stock_Id", Convert.ToInt32(drRow["ID"])));
                        }
                        else
                        {
                            this.Parameters.Add(Parameter("@OrgStruc_Id", Convert.ToInt32(drRow["ID"])));
                        }

                        this.Parameters.Add(Parameter("@Inv_ItemId", Convert.ToInt32(drRow["Inv_ItemId"])));
                        this.Parameters.Add(Parameter("@Pack_Id", Convert.ToInt32(drRow["Pack_Id"])));
                        this.Parameters.Add(Parameter("@Order_Qty", Convert.ToInt32(drRow["ItemQTY"])));

                        this.Parameters.Add(Parameter("@Allocate_Qty", Convert.ToInt32(drRow["Allocate_Qty"])));
                        this.Parameters.Add(Parameter("@On_Hand_Qty_After_Allocate", Convert.ToInt32(drRow["OnHand_Qty"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_INV_SUMMARYREQ_ITEM_Insert", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }
                    }
                    else
                    {
                        // update SummaryReq_Item

                        this.BeginParameter();
                        
                        this.Parameters.Add(Parameter("@Summary_ReqItem_Id", Convert.ToInt32(drRow["Summary_ReqItem_Id"])));
                        this.Parameters.Add(Parameter("@Order_Qty", Convert.ToInt32(drRow["ItemQTY"])));
                        this.Parameters.Add(Parameter("@Allocate_Qty", Convert.ToInt32(drRow["Allocate_Qty"])));
                        this.Parameters.Add(Parameter("@On_Hand_Qty_After_Allocate", Convert.ToInt32(drRow["OnHand_Qty"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item_modify", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }
                    }

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);
                }



                if (dtTemp_delete_item.Rows.Count > 0)
                {
                    foreach (DataRow drRow in dtTemp_delete_item.Rows)
                    {
                        this.BeginParameter();

                        this.Parameters.Add(Parameter("@Summary_ReqItem_Id", Convert.ToInt32(drRow["Summary_ReqItem_Id"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_Delete_INV_SUMMARYREQ_ITEM", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                    }
                }


                /*
                 UPDATE Table 3 
                    Update Inv_Request 
                    SET   Request_Status   =  '4' 
                 */

                this.BeginParameter();

                this.Parameters.Add(Parameter("@Summary_ReqId", Summary_ReqId));
                this.Parameters.Add(Parameter("@Request_Status", "4"));
                this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);


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
                //this.CommitTransaction();// Request commit transaction
            }
            //finally
            //{
            //    this.CommitTransaction();// Request commit transaction
            //}

            return chk;

        }




        public bool InsertStockPay(List<SqlParameter> param, DataTable dtRequest, DataTable dtRequestItem, string UserName, string UserID, string Request_Id, string Summary_ReqId, out string result_PayId)
        {
            //if (dtRequest.Rows.Count == 0 || dtReqItem.Select("[Is_Delete]='N'").Length == 0) return -1;

            bool chk = false;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            //DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update

            int chk_Data_All = 3 + (dtRequestItem.Rows.Count * 6);
            int chk_Data = 0;

            string Pay_Id = "";
            result_PayId = "";
            try
            {

            /* insert Table 1  Inv_Stockpay */

            this.BeginParameter();
            
            foreach (SqlParameter data in param)
            {
                this.Parameters.Add(data);
            }

            //int result = new DatabaseHelper().ExecuteNonQuery("sp_Inv_Stockpay_Insert", param);

            int result = this.ExecuteNonQuery_Chk("sp_Inv_Stockpay_Insert", this.Parameters);
            
            if (result > 0)
            {
                Pay_Id = param[0].Value.ToString();

                result_PayId = Pay_Id;

                chk_Data += 1;

                /* insert table 2 Inv_StockPayItem */

                foreach (DataRow drRow in dtRequestItem.Rows)
                {
                    this.BeginParameter();
                    // ****** IMPORTANT Scope for set req item id *******
                    // NULL / Empty = Insert
                    // NOT NULL = Update

                    //*************************************************
                    this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(Pay_Id)));

                    this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                    this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));

                    this.Parameters.Add(Parameter("@Pay_Quantity", Convert.ToInt32(drRow["Pay_Quantity"])));
                    this.Parameters.Add(Parameter("@Unit_Price", Convert.ToDecimal(drRow["Unit_Price"])));
                    this.Parameters.Add(Parameter("@Amount", Convert.ToDecimal(drRow["Amount"])));

                    #region LPA 17012014

                    this.Parameters.Add(Parameter("@Pay_Remark",drRow["Pay_Remark"].ToString()));

                    #endregion

                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_StockPayItem_Insert", this.Parameters);

                    if (result > 0)
                        chk_Data += result;
                    else
                    {
                        this.Rollback();
                        return false;
                    }

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_StockPayItem_Insert", this.Parameters);
                }


                /*
                 UPDATE Number_Req_Dispense Table INV_SUMMARYREQ

                */

                if (Summary_ReqId != "")
                {
                    this.BeginParameter();

                    this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(Summary_ReqId)));
                    this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Number_Req_Dispense", this.Parameters);

                    if (result > 0)
                        chk_Data += result;
                    else
                    {
                        this.Rollback();
                        return false;
                    }
                }
                else
                {
                    chk_Data += 1;
                }

                
               

                /*
                 UPDATE Table 3 Inv_ReqItem
                    SET    Pay_Qty         =  Pay_Qty  + ? จำนวนที่จ่าย จากหน้าจอ
                           Pay_Amount      =  Pay_Amount + Inv_StockPayItem.Amount
                           Req_ItemStatus  = ?
                    WHERE  Req_ItemID  = ?

                */

                foreach (DataRow drRow in dtRequestItem.Rows)
                {
                    this.BeginParameter();
                    // ****** IMPORTANT Scope for set req item id *******
                    // NULL / Empty = Insert
                    // NOT NULL = Update

                    //*************************************************
                    this.Parameters.Add(Parameter("@Req_ItemID", Convert.ToInt32(drRow["Req_ItemID"])));
                    this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["Pay_Quantity"])));
                    this.Parameters.Add(Parameter("@Pay_Amount", Convert.ToDecimal(drRow["Amount"])));
                    this.Parameters.Add(Parameter("@Req_ItemStatus", drRow["Req_ItemStatus"].ToString()));

                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_Update_RequestItem", this.Parameters);

                    if (result > 0)
                        chk_Data += result;
                    else
                    {
                        this.Rollback();
                        return false;
                    }

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_RequestItem", this.Parameters);
                }

                /* UPDATE Table 4 Inv_SummaryReq_Item
                 
                  Update Inv_SummaryReq_Item
                  Set Pay_Qty =    Pay_Qty  +  จำนวนที่จ่าย จากหน้าจอ
                  Where Summary_ReqId         =  ?
                        AND  Stock_Id_Req     =  ? Inv_Request.Stock_Id_Req
                        AND  OrgStruc_Id_Req  =  ? Inv_Request.OrgStruc_Id_Req
                        AND  inv_ItemId       =  ?
                        AND  Pack_id          =  ?

                 */

                foreach (DataRow drRow in dtRequestItem.Rows)
                {
                    this.BeginParameter();
                    // ****** IMPORTANT Scope for set req item id *******
                    // NULL / Empty = Insert
                    // NOT NULL = Update

                    //*************************************************

                    if (drRow["Summary_ReqItem_Id"].ToString() == "")
                    {
                        chk_Data += 1;
                    }
                    else
                    {
                        this.Parameters.Add(Parameter("@Summary_ReqItem_Id", Convert.ToInt32(drRow["Summary_ReqItem_Id"])));
                        this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["Pay_Quantity"])));

                        result = 0;

                        result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);

                        if (result > 0)
                            chk_Data += result;
                        else
                        {
                            this.Rollback();
                            return false;
                        }

                        //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);
                    }
                }


                /* UPDATE Table 5 Inv_Stock_onhand
                 
                  // Update  ยอดสินค้าในคลังแต่ละรายการให้ลดลงตามจำนวนที่จ่าย
                    
                 * Update  inv_Stock_onhand
                    Set      onHand_Qty  =  onHand_Qty – จำนวนที่จ่ายจากหน้าจอ,
                             OnHand_Amount = OnHand_Amount -  Inv_StockPayItem.Amount
                             Update_Date     =   ? วัน-เวลา ปัจจุบัน
                             Update_By         =   ? User  Login
                    Where  Stock_id   = ? Inv_StockPay.Stock_Id_Pay
                    AND    Inv_itemID = ?
                    AND    pack_ID    = ?


                 */

                

                foreach (DataRow drRow in dtRequestItem.Rows)
                {

                    
                    this.BeginParameter();
                    // ****** IMPORTANT Scope for set req item id *******
                    // NULL / Empty = Insert
                    // NOT NULL = Update

                    //*************************************************
                    this.Parameters.Add(Parameter("@OnHand_Qty_After", Convert.ToDecimal("0")));

                    Parameters[0].Direction = ParameterDirection.Output;

                    this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToDecimal(drRow["Pay_Quantity"])));
                    this.Parameters.Add(Parameter("@Amount", Convert.ToDecimal(drRow["Amount"])));
                    this.Parameters.Add(Parameter("@Stock_ID", Convert.ToInt32(dtRequest.Rows[0]["Stock_Id_From"].ToString())));
                    this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                    this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                    this.Parameters.Add(Parameter("@Update_By", UserName));


                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_OnHand", this.Parameters);

                    if (result > 0)
                    {
                        if (Parameters[0].SqlValue.ToString() != "Null")
                        {
                            decimal OnHand_Qty = Convert.ToDecimal(Parameters[0].Value);

                            if (OnHand_Qty < 0)
                            {
                                this.Rollback();
                                return false;
                                //Roll Back
                            }
                            else
                            {
                                chk_Data += 1;
                            }
                        }
                        else
                        {
                            chk_Data += 1;
                        }
                        
                        //chk_Data += result;
                    }
                    else
                    {
                        this.Rollback();
                        return false;
                    }


                    //int result_2 = this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_OnHand", this.Parameters);
                    //int result_2 = new DatabaseHelper().ExecuteNonQuery("sp_Inv_Update_Stock_OnHand", this.Parameters);

                    //if (result_2 > 0)
                    //{
                    

                    //}

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_OnHand", this.Parameters);


                }


                /* INSERT Table 6 INV_Stock_Movement */

                foreach (DataRow drRow in dtRequestItem.Rows)
                {
                    this.BeginParameter();
                    // ****** IMPORTANT Scope for set req item id *******
                    // NULL / Empty = Insert
                    // NOT NULL = Update

                    //*************************************************
                    this.Parameters.Add(Parameter("@Stock_ID", Convert.ToInt32(dtRequest.Rows[0]["Stock_Id_From"].ToString())));
                    this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                    this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                    this.Parameters.Add(Parameter("@MoveMent_Type", "O"));
                    this.Parameters.Add(Parameter("@Qty_Movement", Convert.ToInt32(drRow["Pay_Quantity"])));
                    this.Parameters.Add(Parameter("@Amount", Convert.ToDecimal(drRow["Amount"])));

                    this.Parameters.Add(Parameter("@Reference_Transaction_Type", "P"));
                    this.Parameters.Add(Parameter("@Reference_Transaction_ID", Convert.ToInt32(Pay_Id)));
                    this.Parameters.Add(Parameter("@Reference_Transaction_No", drRow["Request_No"].ToString()));
                    this.Parameters.Add(Parameter("@Create_By", Convert.ToInt32(UserID)));


                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_Insert_Stock_MoveMent", this.Parameters);

                    if (result > 0)
                        chk_Data += result;
                    else
                    {
                        this.Rollback();
                        return false;
                    }

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Insert_Stock_MoveMent", this.Parameters);
                }


                /* UPDATE Table 7 Inv_Stock_lot_location
                 
                        Set  qty_out  = qty_out + ?
                             Update_date = ?
                             Update_by = ?
                        Where  Primary Key
                 
                   UPDATE Table 8 Inv_Stock_lot
                 
                    Update Inv_Stock_lot 
                    Set  qty_out  = qty_out + ?
                    Where Stock_lot_id  = ?
                  
                  
                 	INSERT Table 9 
                        Insert Inv_Stock_lot_log
                
                */


                foreach (DataRow drRow in dtRequestItem.Rows)
                {
                    this.BeginParameter();
                    // ****** IMPORTANT Scope for set req item id *******
                    // NULL / Empty = Insert
                    // NOT NULL = Update

                    //*************************************************
                    this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["Pay_Quantity"])));
                    this.Parameters.Add(Parameter("@Stock_ID", Convert.ToInt32(dtRequest.Rows[0]["Stock_Id_From"].ToString())));
                    this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                    this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));

                    this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(Pay_Id)));

                    this.Parameters.Add(Parameter("@Lot_No", drRow["Lot_No"].ToString()));
                    this.Parameters.Add(Parameter("@PayAmount", drRow["Pay_Amount"].ToString()));
                    

                    this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_Lot_No", this.Parameters);

                    if (result > 0)
                        chk_Data += result;
                    else
                    {
                        this.Rollback();
                        return false;
                    }

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_Lot", this.Parameters);
                }


                /* UPDATE Table 10 Inv_Request  
                 
                	UPDATE  Inv_Request
                    Choose Case Inv_ReqItem.Req_ItemStatus
                           Case  ‘1’   //  มีรายการสินค้ารายการใดที่ Inv_ReqItem.Req_ItemStatus = ‘1’ // ค้างจ่าย 
                                        Inv_Request.Request_Status = ‘5’

                                  Case Else  ทุกรายการสินค้า  in ( ‘2’ – จ่ายไม่ครบ(แต่ปิดการจ่าย) ,’3’  //จ่ายครบ )
                                        Inv_Request.Request_Status = ‘6’

                            End Case 
                 
                */


                string Request_Status = "";


                DataRow[] dataR = dtRequestItem.Select("Req_ItemStatus = '1'");

                if (dataR.Length > 0)
                {
                    Request_Status = "5";
                }
                else
                {
                    Request_Status = "6";
                }


                this.BeginParameter();

                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(Request_Id)));
                this.Parameters.Add(Parameter("@Request_Status", Convert.ToChar(Request_Status)));
                this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

                if (Request_Status == "5")
                {
                    this.Parameters.Add(Parameter("@Pending", "Y"));
                }

                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);


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
            else
            {
                chk = false;
                this.Rollback();
            }


            }
            catch (Exception ex)
            {
                this.Rollback();
                //this.CommitTransaction();// Request commit transaction
            }
            //finally
            //{
            //    this.CommitTransaction();// Request commit transaction
            //}

            return chk;

        }



        public bool UpdateStockPay(DataTable dtRequestItem, string UserName, string UserID, string Request_Id, string Pay_Id, string Stock_Id_Pay, string Summary_ReqId)
        {
            //if (dtRequest.Rows.Count == 0 || dtReqItem.Select("[Is_Delete]='N'").Length == 0) return -1;

            bool chk = false;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            //DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update

            int chk_Data_All = 3 + (dtRequestItem.Rows.Count * 4);
            int chk_Data = 0;

            int result = 0;

            try
            {

            /* UPDATE Table 1	
                    Update Inv_RequestItem (ของแต่ละรายการสินค้า)
                    //Update ลดยอดสะสมจำนวนที่จ่ายในใบเบิกออก
                          UPDATE  Inv_ReqItem
                          SET     Pay_Qty             =  Pay_Qty  -  Inv_StockPayItem.Pay_Quantity
                                  Pay_Amount         =  Pay_Amount - Inv_StockPayItem.Amount
                                  Req_ItemStatus   =  (If pay_qty[ค่าที่ Update แล้ว]  = 0  Then ‘O’  Else ‘1’ )
                          WHERE Req_ItemID  = ?
             */

            foreach (DataRow drRow in dtRequestItem.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update

                //*************************************************
                this.Parameters.Add(Parameter("@Req_ItemID", Convert.ToInt32(drRow["Req_ItemID"])));
                this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["StockPayItem_PayQty"])));
                this.Parameters.Add(Parameter("@Pay_Amount", Convert.ToDecimal(drRow["StockPayItem_Amount"])));

                string Req_ItemStatus = "";

                if ((Convert.ToInt32(drRow["Pay_Qty"]) - Convert.ToInt32(drRow["StockPayItem_PayQty"])) == 0)
                {
                    Req_ItemStatus = "0";
                }
                else
                {
                    Req_ItemStatus = "1";
                }

                this.Parameters.Add(Parameter("@Req_ItemStatus", Req_ItemStatus));
                this.Parameters.Add(Parameter("@Update_Cancel", "Y"));


                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_RequestItem", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_RequestItem", this.Parameters);
            }


            /*  UPDATE Table 2	  
                2.	Update ลดยอดสะสมในการจ่ายที่ Table Inv_SummaryReq_Item
               Update Inv_SummaryReq_Item
               Set           Pay_Qty =    Pay_Qty  -   Inv_StockPayItem.Pay_Quantity
               Where        Summary_ReqId = ?
                AND         Stock_Id_Req    	         =  ?Inv_Request.Stock_Id_Req
                AND         OrgStruc_Id_Req        =  ?Inv_Request.OrgStruc_Id_Req
                AND         inv_ItemId  = ?
                AND         Pack_id  = ?
            */

            foreach (DataRow drRow in dtRequestItem.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update

                //*************************************************

                if (drRow["Summary_ReqItem_Id"].ToString() == "")
                {
                    chk_Data += 1;
                }
                else
                {

                    this.Parameters.Add(Parameter("@Summary_ReqItem_Id", Convert.ToInt32(drRow["Summary_ReqItem_Id"])));
                    this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["StockPayItem_PayQty"])));
                    this.Parameters.Add(Parameter("@Update_Cancel", "Y"));


                    result = 0;

                    result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);

                    if (result > 0)
                        chk_Data += result;
                    else
                    {
                        this.Rollback();
                        return false;
                    }

                    //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Item", this.Parameters);

                }
            }

            /*  UPDATE Table 3 - 6 
                ทำการ Update ยอดสินค้าคงคลังแต่ละรายการเพิ่มเข้าไปในคลัง
              
                3) Update  inv_Stock_onhand
                4) Update  inv_Stock_lot_location
                5) Update inv_Stock_lot
                6) Update  Inv_Stock_lot_log
                
            */


            foreach (DataRow drRow in dtRequestItem.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update

                //*************************************************
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(Pay_Id)));
                this.Parameters.Add(Parameter("@Inv_ItemID_In", Convert.ToInt32(drRow["Inv_ItemID"])));
                this.Parameters.Add(Parameter("@Pack_ID_In", Convert.ToInt32(drRow["Pack_ID"])));
                this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));


                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_Lot_Cancel", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Stock_Lot_Cancel", this.Parameters);
            }


            /* INSERT Table 7 INV_Stock_Movement */

            foreach (DataRow drRow in dtRequestItem.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update

                //*************************************************
                this.Parameters.Add(Parameter("@Stock_ID", Convert.ToInt32(Stock_Id_Pay)));
                this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                this.Parameters.Add(Parameter("@MoveMent_Type", "I"));
                this.Parameters.Add(Parameter("@Qty_Movement", Convert.ToInt32(drRow["StockPayItem_PayQty"])));
                this.Parameters.Add(Parameter("@Amount", Convert.ToDecimal(drRow["StockPayItem_Amount"])));

                this.Parameters.Add(Parameter("@Reference_Transaction_Type", "P"));
                this.Parameters.Add(Parameter("@Reference_Transaction_ID", Convert.ToInt32(Pay_Id)));
                this.Parameters.Add(Parameter("@Reference_Transaction_No", drRow["Request_No"].ToString()));
                this.Parameters.Add(Parameter("@Create_By", Convert.ToInt32(UserID)));


                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Insert_Stock_MoveMent", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }

                //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Insert_Stock_MoveMent", this.Parameters);
            }


            /*  UPDATE Table 8
                
                Update inv_Stock_pay
                Set Pay_Status   = ‘0’
                    Update_Date = วัน-เวลาปัจจุบัน
                    Update_by     = User Login
                Where  pay_id = ?

            */

            this.BeginParameter();

            this.Parameters.Add(Parameter("@Pay_Status", "0"));
            this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));
            this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(Pay_Id)));

            result = 0;

            result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Stockpay", this.Parameters);

            if (result > 0)
                chk_Data += result;
            else
            {
                this.Rollback();
                return false;
            }

            //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Stockpay", this.Parameters);


            /*
                 UPDATE Number_Req_Dispense Table INV_SUMMARYREQ

                */

            if (Summary_ReqId != "")
            {
                this.BeginParameter();

                this.Parameters.Add(Parameter("@Summary_ReqId", Convert.ToInt32(Summary_ReqId)));
                this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

                result = 0;

                result = this.ExecuteNonQuery_Chk("sp_Inv_Update_SummaryReq_Number_Req_Dispense", this.Parameters);

                if (result > 0)
                    chk_Data += result;
                else
                {
                    this.Rollback();
                    return false;
                }
            }
            else
            {
                chk_Data += 1;
            }



            /*  UPDATE Table 9
                
                Update Inv_Request 
                (เมื่อ Update ทุกรายการสินค้าในกsารจ่ายครั้งนั้นเรียบร้อยแล้ว Update สถานะของใบเบิก)

            */



            string Request_Status = "";

            //List<SqlParameter> param = new List<SqlParameter>();

            //param.Add(new SqlParameter("@Request_Id", Request_Id));

            this.BeginParameter();

            this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(Request_Id)));

            DataSet ds = this.ExecuteDataSet_2("sp_Inv_Select_Chk_Update_Request_Status", this.Parameters);

            DataTable dt_RequestItem = null;
            DataTable dt_SummaryReq = null;

            if (ds != null)
            {
                dt_RequestItem = ds.Tables[0];
                if (ds.Tables[1] != null)
                    dt_SummaryReq = ds.Tables[1];
            }


            DataRow[] dataR = dt_RequestItem.Select("Pay_Qty > 0");

            if (dataR.Length > 0)
            {
                Request_Status = "5";
            }
            else
            {
                if (dt_SummaryReq != null)
                {
                    string stts = dt_SummaryReq.Rows[0]["Status"].ToString();

                    if (stts == "2")
                    {
                        Request_Status = "4";
                    }
                    else if (stts == "1")
                    {
                        Request_Status = "3";
                    }
                    else if (stts.Trim() == "")
                    {
                        Request_Status = "2";
                    }

                }
                else
                {
                    Request_Status = "2";
                }
            }


            this.BeginParameter();

            this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(Request_Id)));
            this.Parameters.Add(Parameter("@Request_Status", Convert.ToChar(Request_Status)));
            this.Parameters.Add(Parameter("@Update_By", Convert.ToInt32(UserID)));

            result = 0;

            result = this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);

            if (result > 0)
                chk_Data += result;
            else
            {
                this.Rollback();
                return false;
            }

            //chk_Data += this.ExecuteNonQuery_Chk("sp_Inv_Update_Request", this.Parameters);

            this.BeginParameter();
            this.Parameters.Add(Parameter("@PAY_ID", Convert.ToInt32(Pay_Id)));
            result = this.ExecuteNonQuery_Chk("SP_INV_inv_stockpayitem_Update", this.Parameters);

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
                //this.CommitTransaction();// Request commit transaction
            }
            //finally
            //{
            //    this.CommitTransaction();// Request commit transaction
            //}

            return chk;

        }
    }
}