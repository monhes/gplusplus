using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class RequestDAO : DbConnectionBase
    {
        public RequestDAO()
        {
        }

        public void ApproveRequest(int requestId, int approveBy, int approveType, string approveReason)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Request_Id", requestId));
            this.Parameters.Add(Parameter("@Consider_Type", approveType));
            this.Parameters.Add(Parameter("@Consider_Reason", approveReason));
            this.Parameters.Add(Parameter("@Consider_Id", approveBy));

            this.ExecuteNonQuery("sp_Inv_Request_Approval", this.Parameters);
        }
        /// <summary>
        /// This method use for insert/update/delete request
        /// </summary>
        /// <param name="dtRequest">Request DataTable (request one row)</param>
        /// <param name="dtReqItem">Request Item DataTable</param>
        /// <returns>Request Id</returns>
        public int InsertOrUpdate(DataTable dtRequest, DataTable dtReqItem)
        {
            if (dtRequest.Rows.Count == 0 || dtReqItem.Select("[Is_Delete]='N'").Length == 0) return -1; 

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update
            if (!string.IsNullOrEmpty(drRequest["Request_Id"].ToString()))
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRequest["Request_Id"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Request_Id", DBNull.Value));
            }
            //*************************************************
            this.Parameters.Add(Parameter("@Request_No", !string.IsNullOrEmpty(drRequest["Request_No"].ToString()) ? drRequest["Request_No"] : DBNull.Value));
            this.Parameters.Add(Parameter("@Request_Date", Convert.ToDateTime(drRequest["Request_Date"])));
            this.Parameters.Add(Parameter("@Request_Type", Convert.ToInt32(drRequest["Request_Type"])));
            if(drRequest["Stock_Id_Req"].ToString().Trim().Length > 0)
                this.Parameters.Add(Parameter("@Stock_Id_Req", Convert.ToInt32(drRequest["Stock_Id_Req"]) ));
            else
                this.Parameters.Add(Parameter("@Stock_Id_Req", DBNull.Value)); //Convert.ToInt32(drRequest["Stock_Id_Req"])
            this.Parameters.Add(Parameter("@OrgStruc_Id_Req", Convert.ToInt32(drRequest["OrgStruc_Id_Req"]))); //Convert.ToInt32(drRequest["OrgStruc_Id_Req"])
            if(drRequest["Stock_Id_From"].ToString().Trim().Length > 0)
                this.Parameters.Add(Parameter("@Stock_Id_From", Convert.ToInt32(drRequest["Stock_Id_From"])));
            this.Parameters.Add(Parameter("@Request_By", Convert.ToInt32(drRequest["Account_Id"])));
            //Consider type
            if (!string.IsNullOrEmpty(drRequest["Consider_Type"].ToString()))
            {
                this.Parameters.Add(Parameter("@Consider_Type", Convert.ToInt32(drRequest["Consider_Type"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Consider_Type", DBNull.Value));
            }
            //Consider reason
            this.Parameters.Add(Parameter("@Consider_Reason", !string.IsNullOrEmpty(drRequest["Consider_Reason"].ToString()) ? drRequest["Consider_Reason"] : DBNull.Value));
            //Consider id
            if (!string.IsNullOrEmpty(drRequest["Consider_Type"].ToString()))
            {
                this.Parameters.Add(Parameter("@Consider_Id", Convert.ToInt32(drRequest["Consider_Id"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Consider_Id", DBNull.Value));
            }
            //consider date
            if (!string.IsNullOrEmpty(drRequest["Consider_Date"].ToString()))
            {
                this.Parameters.Add(Parameter("@Consider_Date", Convert.ToDateTime(drRequest["Consider_Date"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Consider_Date", DBNull.Value));
            }
            this.Parameters.Add(Parameter("@Request_Status", Convert.ToInt32(drRequest["Request_Status"])));
            this.Parameters.Add(Parameter("@Created_By", Convert.ToInt32(drRequest["Created_By"])));
            this.Parameters.Add(Parameter("@Updated_By", Convert.ToInt32(drRequest["Updated_By"])));

            DataTable dtResult = this.ExecuteDataTable("sp_Inv_Request_InsertOrUpdate", this.Parameters);
            int newRequestId = Convert.ToInt32(dtResult.Rows[0]["Request_Id"]);

            // Add or update request item(s)
            foreach (DataRow drRow in dtReqItem.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update
                if (!string.IsNullOrEmpty(drRow["Req_ItemID"].ToString()))
                {
                    this.Parameters.Add(Parameter("@Req_ItemID", Convert.ToInt32(drRow["Req_ItemID"])));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Req_ItemID", DBNull.Value));
                }
                //*************************************************
                this.Parameters.Add(Parameter("@Request_Id", newRequestId));
                this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                this.Parameters.Add(Parameter("@Order_Quantity", Convert.ToInt32(drRow["Order_Quantity"])));
                this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["Pay_Qty"])));
                this.Parameters.Add(Parameter("@Pay_Amount", DBNull.Value)); //Convert.ToDecimal(drRow["Pay_Amount"])
                this.Parameters.Add(Parameter("@Receive_Qty", Convert.ToInt32(drRow["Receive_Qty"])));
                this.Parameters.Add(Parameter("@Receive_Amount", DBNull.Value)); //Convert.ToDecimal(drRow["Receive_Amount"])
                this.Parameters.Add(Parameter("@Req_ItemStatus", Convert.ToInt32(drRow["Req_ItemStatus"])));
                //this.Parameters.Add(Parameter("@Order_Amount", Convert.ToInt32(drRow["Order_Amount"])));
                this.Parameters.Add(Parameter("@Order_Amount", Convert.ToDecimal(drRow["Order_Amount"])));
                this.Parameters.Add(Parameter("@Is_Delete", drRow["Is_Delete"]));
                this.Parameters.Add(Parameter("@Remark", drRow["Remark"]));
                this.ExecuteNonQuery("sp_Inv_ReqItem_InsertOrUpdate", this.Parameters);
            }

            this.CommitTransaction();// Request commit transaction

            return newRequestId;
        }
        /// <summary>
        /// This method use to get department name
        /// </summary>
        /// <param name="orgId">department id</param>
        /// <returns></returns>
        public DataTable GetDepartmentDetail(string orgId)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@ORG_ID", orgId));
            DataTable dt = this.ExecuteDataTable("sp_Inv_Request_GetDepartmentName", this.Parameters);
            if (dt.Rows.Count > 0)
            {
                return dt;
            }

            return null;
        }
        /// <summary>
        /// This method use to get all request status
        /// </summary>
        /// <returns></returns>
        public DataTable GetRequestStatus()
        {
            DataTable dt = this.ExecuteDataTable("sp_Inv_ReqStatus_GetActiveStatus");
            DataRow drAllStatus = dt.NewRow();
            drAllStatus["Status_Id"] = 0;
            drAllStatus["Status_Desc"] = "ทั้งหมด";
            drAllStatus["Is_Active"] = "Y";
            dt.Rows.InsertAt(drAllStatus, 0);
            dt.AcceptChanges();

            return dt;
        }
        /// <summary>
        /// This method use to get all employee in org
        /// </summary>
        /// <param name="orgId">org id</param>
        /// <param name="fname">first name filter</param>
        /// <param name="lname">last name filter</param>
        /// <returns>Employee dataview</returns>
        public DataView GetEmployee(string orgId, string fname = null, string lname = null)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@ORG_ID", orgId));
            DataTable dt = this.ExecuteDataTable("sp_Inv_Request_GetEmployee", this.Parameters);
            DataView dv = new DataView(dt);
            string filter = string.Empty;
            if (fname != null)
            {
                filter += "[Account_Fname]='" + fname + "'";
            }
            if (lname != null)
            {
                if (fname != null)
                {
                    filter += " AND ";
                }
                filter += "[Account_Lname]='" + lname + "'";
            }
            dv.RowFilter = filter;

            return dv;
        }

        #region Nin 14082013

        public DataView GetAllEmployee(string fname = null, string lname = null)
        {
            DataTable dt = this.ExecuteDataTable("sp_Inv_Request_GetAllEmployee");
            DataView dv = new DataView(dt);
            string filter = string.Empty;
            if (fname != null)
            {
                //filter += "[Account_Fname]='" + fname + "'";
                filter += "[Account_Fname] like '%" + fname + "%'";
            }
            if (lname != null)
            {
                if (fname != null)
                {
                    filter += " AND ";
                }
                //filter += "[Account_Lname]='" + lname + "'";
                filter += "[Account_Lname] like '%" + lname + "%'";
            }
            dv.RowFilter = filter;

            return dv;
        }


        #endregion
        /// <summary>
        /// This method use for get employee detail
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="orgId">Org Id</param>
        /// <returns></returns>
        public DataRow GetEmployeeDetail(string userId, string orgId)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@ORG_ID", orgId));
            DataTable dt = this.ExecuteDataTable("sp_Inv_Request_GetEmployee", this.Parameters);
            return dt.Select("[Account_Id]='" + userId + "'").FirstOrDefault();
        }
        /// <summary>
        /// This method use to get reqeust 
        /// </summary>
        /// <param name="requestId">Request Id [0: Get Schema]</param>
        /// <returns>Request DataTable</returns>
        public DataTable GetRequest(int requestId = 0)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Reqeust_Id", requestId));
            DataTable dt = this.ExecuteDataTable("sp_Inv_Reqeust_Select", this.Parameters);

            return dt;
        }
        /// <summary>
        /// This method use to get request item
        /// </summary>
        /// <param name="requestId">Request Id [0: Get Schema]</param>
        /// <returns>Request Item DataTable</returns>
        public DataTable GetRequestItem(int requestId = 0, string default_pay_id = "0")
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Request_Id", requestId));
            this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(default_pay_id)));
            DataTable dt = this.ExecuteDataTable("sp_Inv_ReqItem_Select", this.Parameters);

            return dt;
        }
        /// <summary>
        /// This method use for get item detail
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public DataRow GetItemDetail(string itemId)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Inv_ItemID", itemId));
            DataTable dt = this.ExecuteDataTable("sp_Inv_Request_GetItemDetail", this.Parameters);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }

            return null;
        }
        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public DataSet GetRequestList(string orgId, string statusId, string requestNo, string requestBy,
            string from , string to, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Org_Id", orgId));
            this.Parameters.Add(Parameter("@Request_Status", statusId));
            this.Parameters.Add(Parameter("@Request_No", requestNo));
            this.Parameters.Add(Parameter("@Request_By_FullName", requestBy));
            this.Parameters.Add(Parameter("@Request_DateFrom", from));
            this.Parameters.Add(Parameter("@Request_DateTo", to));
            this.Parameters.Add(Parameter("@PageNum", pageNum));
            this.Parameters.Add(Parameter("@PageSize", pageSize));
            this.Parameters.Add(Parameter("@SortField", sortField));
            this.Parameters.Add(Parameter("@SortOrder", sortOrder));

            DataSet ds = this.ExecuteDataSet("sp_Inv_Request_GetRequestListPaging", this.Parameters);
            return ds;
        }
        /// <summary>
        /// This method use to cancel request
        /// </summary>
        /// <param name="requestId">request id</param>
        /// <returns> request status</returns>
        public int CancelRequest(string requestId)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Request_Id", requestId));
                this.ExecuteNonQuery("sp_Inv_Request_CancelRequest", this.Parameters);
                return 1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public DataTable GetRequestReport(string requestID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Request_Id", requestID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_SelectReport", param);
        }

        public DataTable GetStockPay(string requestID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Request_Id", requestID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_SelectStockPay", param);
        }

        #region Nin 18/08/2013

        public bool InsertStockReqRec(DataTable dtRequestItem)
        {
            bool chk = false;
            int chk_query = dtRequestItem.Rows.Count+1;// Query ทั้งหมดที่ต้องได้ เท่ากับ 1 (insert ลงใน Table Inv_ReqRec) + จำนวน Item ทั้งหมด ใน list 
            int cnt_query = 0;

            this.UseTransaction = true; // Request Transaction
            this.BeginParameter();
            DataRow drRequest = dtRequestItem.Rows[0];

            // ****** IMPORTANT Scope for set request id *******

            if (!string.IsNullOrEmpty(drRequest["Request_Id"].ToString()))
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRequest["Request_Id"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
            }

            if (!string.IsNullOrEmpty(drRequest["Pay_Id"].ToString()))
            {
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(drRequest["Pay_Id"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32("0")));
            }

            if (!string.IsNullOrEmpty(drRequest["Receive_By"].ToString()))
            {
                this.Parameters.Add(Parameter("@Receive_By", Convert.ToInt32(drRequest["Receive_By"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Receive_By", Convert.ToInt32("0")));
            }

            if (!string.IsNullOrEmpty(drRequest["Receive_Status"].ToString()))
            {
                this.Parameters.Add(Parameter("@Receive_Status", drRequest["Receive_Status"].ToString()));
            }
            else
            {
                this.Parameters.Add(Parameter("@Receive_Status", "0"));
            }

            DataTable dtResult = this.ExecuteDataTable("sp_Inv_Request_Insert_ReqRec", this.Parameters);
            int newRecPayId = Convert.ToInt32(dtResult.Rows[0]["ReqRec_Id"]);
            if (newRecPayId != 0)
            {
                cnt_query++;


                //Add or update requestReceive item(s)
                foreach (DataRow drRow in dtRequestItem.Rows)
                {
                    this.BeginParameter();
                    
                    //*************************************************


                    this.Parameters.Add(Parameter("@RecPay_Id", newRecPayId));
                    

                    if (!string.IsNullOrEmpty(drRequest["Inv_ItemID"].ToString()))
                    {
                        this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                    }
                    else
                    {
                        this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32("0")));
                    }

                    if (!string.IsNullOrEmpty(drRequest["Pack_ID"].ToString()))
                    {
                        this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                    }
                    else
                    {
                        this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32("0")));
                    }
                    if (!string.IsNullOrEmpty(drRequest["Receive_Qty"].ToString()))
                    {
                        this.Parameters.Add(Parameter("@Receive_Qty", Convert.ToInt32(drRow["Receive_Qty"])));
                    }
                    else
                    {
                        this.Parameters.Add(Parameter("@Receive_Qty", Convert.ToInt32("0")));
                    }
                    this.Parameters.Add(Parameter("@Remark", drRow["Remark"].ToString()));

                    if (!string.IsNullOrEmpty(drRequest["Request_Id"].ToString()))
                    {
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));
                    }
                    else
                    {
                        this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
                    }

                    DataTable dtResultItem = this.ExecuteDataTable("sp_Inv_Request_Insert_ReqRecItem", this.Parameters);
                    int newRecPayItemId = Convert.ToInt32(dtResultItem.Rows[0]["RecPay_ItemId"]);
                    if (newRecPayItemId != 0)
                    {
                        cnt_query++;
                    }
                }
            }

            this.CommitTransaction();// Request commit transaction

            if (cnt_query == chk_query)
            {
                chk = true;
            }
            else
            {
                chk = false;
            }
            return chk;
        }

        public bool DeleteStockReqRec(string requestId, string payId)
        {
            bool chk = false;

            this.BeginParameter();
           
            // ****** IMPORTANT Scope for set request id *******

            if (requestId != "")
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(requestId)));
            }
            else
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
            }

            if (payId != "")
            {
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(payId)));
            }
            else
            {
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32("0")));
            }

            DataTable dtResult = this.ExecuteDataTable("sp_Inv_Request_Delete_ReqRec", this.Parameters);

            if (dtResult.Rows[0]["result"].ToString() == "1")
            {
                chk = true;
            }
            else
            {
                chk = false;
            }
            return chk;
        }

        public DataTable GetReqRecStatus(string requestID)
        {
            if (requestID != "")
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(requestID)));
            }
            else
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
            }

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_SelectReqRecStatus", this.Parameters);
        }

        public DataTable CheckRequestReceive(string orgID)
        {
            if (orgID != "")
            {
                this.Parameters.Add(Parameter("@OrgStruc_Id", Convert.ToInt32(orgID)));
            }
            else
            {
                this.Parameters.Add(Parameter("@OrgStruc_Id", Convert.ToInt32("0")));
            }

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_CheckAllReceive", this.Parameters);
        }

        public DataTable CheckCntPay(string RequestID)
        {
            if (RequestID != "")
            {
                this.Parameters.Add(Parameter("@RequestID", Convert.ToInt32(RequestID)));
            }
            else
            {
                this.Parameters.Add(Parameter("@RequestID", Convert.ToInt32("0")));
            }

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_CheckCntPay", this.Parameters);
        }

        public DataTable GetRequestReport_Pay(string requestID, string payID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Request_Id", requestID));
            param.Add(new SqlParameter("@Pay_Id", payID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_SelectReport_Pay", param);
        }


        public bool InsertStockReqRecAuto(string Req_Id,string Pay_Id,string User_Id)
        {
            string result = "";


            this.BeginParameter();


            if (!string.IsNullOrEmpty(Req_Id))
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(Req_Id)));
            }
            else
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
            }

            if (!string.IsNullOrEmpty(Pay_Id))
            {
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(Pay_Id)));
            }
            else
            {
                this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32("0")));
            }

            if (!string.IsNullOrEmpty(User_Id))
            {
                this.Parameters.Add(Parameter("@Receive_By", Convert.ToInt32(User_Id)));
            }
            else
            {
                this.Parameters.Add(Parameter("@Receive_By", Convert.ToInt32("0")));
            }

                this.Parameters.Add(Parameter("@Receive_Status", "1"));

                result = new DatabaseHelper().ExecuteScalar("sp_Inv_Request_Insert_ReqRecAuto", this.Parameters).ToString();

                if (result == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }

        }

        public DataTable GetRequest(string requestID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Request_Id", requestID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_SelectByRequestID", param);
        }


        #endregion

        #region PT


        //sp_Req_Inv_Request_SelectStockPay_ByRequestID


        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public DataTable ReqInvRequestSelectStockPayByRequestID(int Request_Id)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Request_Id", Request_Id));

                return this.ExecuteDataTable("sp_Req_Inv_Request_SelectStockPay_ByRequestID", this.Parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public DataTable GetReqGetStock(string Account_id)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Account_ID", Account_id));

                return this.ExecuteDataTable("sp_Req_GetStock", this.Parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public DataSet GetReqRequestList(string stk_id_req, string orgId, string statusId, string requestNo, string requestBy,
            string from, string to, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Org_Id", orgId));
                this.Parameters.Add(Parameter("@Request_Status", statusId));
                this.Parameters.Add(Parameter("@Request_No", requestNo));
                this.Parameters.Add(Parameter("@Request_By_FullName", requestBy));
                this.Parameters.Add(Parameter("@Request_DateFrom", from));
                this.Parameters.Add(Parameter("@Request_DateTo", to));
                this.Parameters.Add(Parameter("@PageNum", pageNum));
                this.Parameters.Add(Parameter("@PageSize", pageSize));
                this.Parameters.Add(Parameter("@SortField", sortField));
                this.Parameters.Add(Parameter("@SortOrder", sortOrder));

                this.Parameters.Add(Parameter("@Stock_Id_Req", stk_id_req));



                DataSet ds = this.ExecuteDataSet("sp_Req_Inv_Request_GetRequestListPaging", this.Parameters);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        /// <summary>
        /// This method use for insert/update/delete request
        /// </summary>
        /// <param name="dtRequest">Request DataTable (request one row)</param>
        /// <param name="dtReqItem">Request Item DataTable</param>
        /// <returns>Request Id</returns>
        public int InsertOrUpdate(DataTable dtRequest, DataTable dtReqItem, int stkIdReq)
        {
            if (dtRequest.Rows.Count == 0 || dtReqItem.Select("[Is_Delete]='N'").Length == 0) return -1;

            this.UseTransaction = true; // Requet Transaction
            this.BeginParameter();
            DataRow drRequest = dtRequest.Rows[0];

            // ****** IMPORTANT Scope for set request id *******
            // NULL / Empty = Insert
            // NOT NULL = Update
            if (!string.IsNullOrEmpty(drRequest["Request_Id"].ToString()))
            {
                this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRequest["Request_Id"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Request_Id", DBNull.Value));
            }
            //*************************************************
            this.Parameters.Add(Parameter("@Request_No", !string.IsNullOrEmpty(drRequest["Request_No"].ToString()) ? drRequest["Request_No"] : DBNull.Value));
            this.Parameters.Add(Parameter("@Request_Date", Convert.ToDateTime(drRequest["Request_Date"])));
            this.Parameters.Add(Parameter("@Request_Type", Convert.ToInt32(drRequest["Request_Type"])));

            // modefy PT    Old Code ----------------------------------------------------------
            //if (drRequest["Stock_Id_Req"].ToString().Trim().Length > 0)
            //    this.Parameters.Add(Parameter("@Stock_Id_Req", Convert.ToInt32(drRequest["Stock_Id_Req"])));
            //else
            //    this.Parameters.Add(Parameter("@Stock_Id_Req", DBNull.Value)); //Convert.ToInt32(drRequest["Stock_Id_Req"])

            //------------  New Code ----------------------------------------------------------

            this.Parameters.Add(Parameter("@Stock_Id_Req", stkIdReq));
            //--------------------------------------------------------------------


            this.Parameters.Add(Parameter("@OrgStruc_Id_Req", Convert.ToInt32(drRequest["OrgStruc_Id_Req"]))); //Convert.ToInt32(drRequest["OrgStruc_Id_Req"])
            if (drRequest["Stock_Id_From"].ToString().Trim().Length > 0)
                this.Parameters.Add(Parameter("@Stock_Id_From", Convert.ToInt32(drRequest["Stock_Id_From"])));
            this.Parameters.Add(Parameter("@Request_By", Convert.ToInt32(drRequest["Account_Id"])));
            //Consider type --------------- Modify
            //if (!string.IsNullOrEmpty(drRequest["Consider_Type"].ToString()))
            //{
            //    this.Parameters.Add(Parameter("@Consider_Type", Convert.ToInt32(drRequest["Consider_Type"])));
            //}
            //else
            //{
            //    this.Parameters.Add(Parameter("@Consider_Type", DBNull.Value));
            //}
            this.Parameters.Add(Parameter("@Consider_Type", 2));
            //-------------------------------------------------------------
            //Consider reason
            this.Parameters.Add(Parameter("@Consider_Reason", !string.IsNullOrEmpty(drRequest["Consider_Reason"].ToString()) ? drRequest["Consider_Reason"] : DBNull.Value));
            //Consider id
            //------------------- Modify   -------------------------------------
            //if (!string.IsNullOrEmpty(drRequest["Consider_Type"].ToString()))
            //{
            //    this.Parameters.Add(Parameter("@Consider_Id", Convert.ToInt32(drRequest["Consider_Id"])));
            //}
            //else
            //{
            //    this.Parameters.Add(Parameter("@Consider_Id", DBNull.Value));
            //}
            this.Parameters.Add(Parameter("@Consider_Id", 1));
            //-------------------------------------------------------------------------
            //consider date
            if (!string.IsNullOrEmpty(drRequest["Consider_Date"].ToString()))
            {
                this.Parameters.Add(Parameter("@Consider_Date", Convert.ToDateTime(drRequest["Consider_Date"])));
            }
            else
            {
                this.Parameters.Add(Parameter("@Consider_Date", DBNull.Value));
            }

            //----------------  Mo by PT   --------------------------------
            // this.Parameters.Add(Parameter("@Request_Status", Convert.ToInt32(drRequest["Request_Status"])));
            //--------------  Add 
            this.Parameters.Add(Parameter("@Request_Status", "2"));
            //-------------------------------------------------------------
            this.Parameters.Add(Parameter("@Created_By", Convert.ToInt32(drRequest["Created_By"])));
            this.Parameters.Add(Parameter("@Updated_By", Convert.ToInt32(drRequest["Updated_By"])));

            DataTable dtResult = this.ExecuteDataTable("sp_Req_Inv_Request_InsertOrUpdate", this.Parameters);
            int newRequestId = Convert.ToInt32(dtResult.Rows[0]["Request_Id"]);

            // Add or update request item(s)
            foreach (DataRow drRow in dtReqItem.Rows)
            {
                this.BeginParameter();
                // ****** IMPORTANT Scope for set req item id *******
                // NULL / Empty = Insert
                // NOT NULL = Update
                if (!string.IsNullOrEmpty(drRow["Req_ItemID"].ToString()))
                {
                    this.Parameters.Add(Parameter("@Req_ItemID", Convert.ToInt32(drRow["Req_ItemID"])));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Req_ItemID", DBNull.Value));
                }
                //*************************************************
                this.Parameters.Add(Parameter("@Request_Id", newRequestId));
                this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                this.Parameters.Add(Parameter("@Order_Quantity", Convert.ToInt32(drRow["Order_Quantity"])));
                this.Parameters.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["Pay_Qty"])));
                this.Parameters.Add(Parameter("@Pay_Amount", DBNull.Value)); //Convert.ToDecimal(drRow["Pay_Amount"])
                this.Parameters.Add(Parameter("@Receive_Qty", Convert.ToInt32(drRow["Receive_Qty"])));
                this.Parameters.Add(Parameter("@Receive_Amount", DBNull.Value)); //Convert.ToDecimal(drRow["Receive_Amount"])
                this.Parameters.Add(Parameter("@Req_ItemStatus", Convert.ToInt32(drRow["Req_ItemStatus"])));
                //this.Parameters.Add(Parameter("@Order_Amount", Convert.ToInt32(drRow["Order_Amount"])));
                this.Parameters.Add(Parameter("@Order_Amount", Convert.ToDecimal(drRow["Order_Amount"])));
                this.Parameters.Add(Parameter("@Is_Delete", drRow["Is_Delete"]));
                this.Parameters.Add(Parameter("@Remark", drRow["Remark"]));
                this.ExecuteNonQuery("sp_Inv_ReqItem_InsertOrUpdate", this.Parameters);
            }

            this.CommitTransaction();// Request commit transaction

            return newRequestId;
        }


        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public DataTable ReqInvReqRecItemSelectByRecPayID(int recPay_id)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@RecPay_Id", recPay_id));

                DataTable dt = this.ExecuteDataTable("sp_Req_Inv_ReqRecItem_SelectBy_RecPayID", this.Parameters);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public DataTable ReqInvReqRecSelectByPayID(int Pay_id)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Pay_Id", Pay_id));

                return this.ExecuteDataTable("sp_Req_Inv_ReqRec_SelectBy_PayID", this.Parameters);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }











        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public bool ReqSelectStockPayCompleteByPayID(int Pay_id)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Pay_Id", Pay_id));

                string flagComplete = this.ExecuteDataTable("sp_Req_SelectStockPay_Complete_ByPayID", this.Parameters).Rows[0]["isComplete"].ToString().Trim();
                return (flagComplete == "1") ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        /// <summary>
        /// This method use to get request list
        /// </summary>
        /// <returns></returns>
        public bool ReqInvRequestSelectReqStatus(int Req_id)
        {
            try
            {
                this.BeginParameter();
                this.Parameters.Add(Parameter("@Request_Id", Req_id));

                string flagComplete = this.ExecuteDataTable("sp_Req_Inv_Request_SelectReqStatus", this.Parameters).Rows[0]["isComplete"].ToString().Trim();
                return (flagComplete == "1") ? true : false;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public DataTable ReqInsertStockReqRec(DataTable dtRequestItem)
        {

            try
            {

                this.BeginParameter();
                DataRow drRequest = dtRequestItem.Rows[0];

                // ****** IMPORTANT Scope for set request id *******

                if (!string.IsNullOrEmpty(drRequest["Request_Id"].ToString()))
                {
                    this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRequest["Request_Id"])));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
                }

                if (!string.IsNullOrEmpty(drRequest["Pay_Id"].ToString()))
                {
                    this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(drRequest["Pay_Id"])));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32("0")));
                }

                if (!string.IsNullOrEmpty(drRequest["Receive_By"].ToString()))
                {
                    this.Parameters.Add(Parameter("@Receive_By", Convert.ToInt32(drRequest["Receive_By"])));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Receive_By", Convert.ToInt32("0")));
                }

                if (!string.IsNullOrEmpty(drRequest["Receive_Status"].ToString()))
                {
                    this.Parameters.Add(Parameter("@Receive_Status", drRequest["Receive_Status"].ToString()));
                }
                else
                {
                    this.Parameters.Add(Parameter("@Receive_Status", "0"));
                }

                DataTable dtReturn = new DataTable();
                dtReturn.Columns.Add("RecPay_ItemId");
                dtReturn.Columns.Add("Inv_ItemID");
                dtReturn.Columns.Add("Pack_ID");
                dtReturn.Columns.Add("Request_Id");
                dtReturn.Columns.Add("Request_No");
                dtReturn.Columns.Add("Pay_Id");
                DataTable dtResult = this.ExecuteDataTable("sp_Req_Inv_Request_Insert_ReqRec", this.Parameters);
                int newRecPayId = Convert.ToInt32(dtResult.Rows[0]["ReqRec_Id"]);
                if (newRecPayId != 0)
                {

                    //Add or update requestReceive item(s)
                    foreach (DataRow drRow in dtRequestItem.Rows)
                    {
                        this.BeginParameter();

                        //*************************************************


                        this.Parameters.Add(Parameter("@RecPay_Id", newRecPayId));


                        if (!string.IsNullOrEmpty(drRequest["Inv_ItemID"].ToString()))
                        {
                            this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                        }
                        else
                        {
                            this.Parameters.Add(Parameter("@Inv_ItemID", Convert.ToInt32("0")));
                        }

                        if (!string.IsNullOrEmpty(drRequest["Pack_ID"].ToString()))
                        {
                            this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                        }
                        else
                        {
                            this.Parameters.Add(Parameter("@Pack_ID", Convert.ToInt32("0")));
                        }
                        if (!string.IsNullOrEmpty(drRequest["Receive_Qty"].ToString()))
                        {
                            this.Parameters.Add(Parameter("@Receive_Qty", Convert.ToInt32(drRow["Receive_Qty"])));
                        }
                        else
                        {
                            this.Parameters.Add(Parameter("@Receive_Qty", Convert.ToInt32("0")));
                        }
                        this.Parameters.Add(Parameter("@Remark", drRow["Remark"].ToString()));

                        if (!string.IsNullOrEmpty(drRequest["Request_Id"].ToString()))
                        {
                            this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32(drRow["Request_Id"])));
                        }
                        else
                        {
                            this.Parameters.Add(Parameter("@Request_Id", Convert.ToInt32("0")));
                        }

                        DataTable dtResultItem = this.ExecuteDataTable("sp_Req_Inv_Request_Insert_ReqRecItem", this.Parameters);


                        DataRow rr = dtReturn.NewRow();
                        rr["RecPay_ItemId"] = Convert.ToInt32(dtResultItem.Rows[0]["RecPay_ItemId"]);
                        rr["Pack_ID"] = Convert.ToInt32(dtResultItem.Rows[0]["Pack_ID"]);
                        rr["Inv_ItemID"] = Convert.ToInt32(dtResultItem.Rows[0]["Inv_ItemID"]);
                        rr["Request_Id"] = Convert.ToInt32(dtResultItem.Rows[0]["Request_Id"]);
                        rr["Request_No"] = dtResultItem.Rows[0]["Request_No"];
                        rr["Pay_Id"] = Convert.ToInt32(dtResultItem.Rows[0]["Pay_Id"]);
                        dtReturn.Rows.Add(rr);

                    }
                }



                return dtReturn;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        /// <summary>
        /// This method use to get request item
        /// </summary>
        /// <param name="requestId">Request Id [0: Get Schema]</param>
        /// <returns>Request Item DataTable</returns>
        public DataTable ReqGetRequestItem(int requestId = 0, string default_pay_id = "0")
        {
            this.BeginParameter();
            this.Parameters.Add(Parameter("@Request_Id", requestId));
            this.Parameters.Add(Parameter("@Pay_Id", Convert.ToInt32(default_pay_id)));
            DataTable dt = this.ExecuteDataTable("sp_Req_Inv_ReqItem_Select", this.Parameters);

            return dt;
        }


        public bool ReqInvReqItemUpdateLater(int reqId, DataTable dtReqItem)
        {
            try
            {



                List<SqlParameter> param = new List<SqlParameter>();
                foreach (DataRow drRow in dtReqItem.Rows)
                {
                    param = new List<SqlParameter>();
                    if (!string.IsNullOrEmpty(drRow["Req_ItemID"].ToString()))
                    {
                        param.Add(Parameter("@Req_ItemID", Convert.ToInt32(drRow["Req_ItemID"])));
                    }
                    else
                    {
                        param.Add(Parameter("@Req_ItemID", DBNull.Value));
                    }
                    //*************************************************
                    param.Add(Parameter("@Request_Id", reqId));
                    param.Add(Parameter("@Inv_ItemID", Convert.ToInt32(drRow["Inv_ItemID"])));
                    param.Add(Parameter("@Pack_ID", Convert.ToInt32(drRow["Pack_ID"])));
                    param.Add(Parameter("@Order_Quantity", Convert.ToInt32(drRow["Order_Quantity"])));
                    param.Add(Parameter("@Pay_Qty", Convert.ToInt32(drRow["Pay_Qty"])));
                    param.Add(Parameter("@Pay_Amount", DBNull.Value)); //Convert.ToDecimal(drRow["Pay_Amount"])
                    param.Add(Parameter("@Receive_Qty", Convert.ToInt32(drRow["Receive_Qty"])));
                    param.Add(Parameter("@Receive_Amount", DBNull.Value)); //Convert.ToDecimal(drRow["Receive_Amount"])
                    param.Add(Parameter("@Req_ItemStatus", Convert.ToInt32(drRow["Req_ItemStatus"])));

                    param.Add(Parameter("@Order_Amount", Convert.ToDecimal(drRow["Order_Amount"])));
                    param.Add(Parameter("@Is_Delete", drRow["Is_Delete"]));
                    param.Add(Parameter("@Remark", drRow["Remark"]));
                    new DatabaseHelper().ExecuteNonQuery("sp_Inv_ReqItem_InsertOrUpdate", param);
                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        // sp_Req_Inv_Item_ReorderPoint

        public DataTable ReqInvItemReorderPoint(int cateId, int subCateId, int stkId)
        {

            try
            {

                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@Cate_ID", cateId));
                param.Add(new SqlParameter("@Stock_ID", stkId));
                param.Add(new SqlParameter("@SubCate_ID", subCateId));

                DataTable dt = new DatabaseHelper().ExecuteDataTable("sp_Req_Inv_Item_ReorderPoint", param);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool CancelReqRecItemsSubStock(int recPay_ItemID, int StockID, string updateBy)
        {

            try
            {
                List<SqlParameter> param = new List<SqlParameter>();

                param.Add(new SqlParameter("@RecPay_ItemID", recPay_ItemID));
                param.Add(new SqlParameter("@Stock_ID", StockID));
                param.Add(new SqlParameter("@Update_By ", updateBy));

                string status = new DatabaseHelper().ExecuteDataTable("sp_Req_Inv_Receive_ReqRecItem_Cancel", param).Rows[0]["IS_CANCEL"].ToString();
                return (status == "0" || status == "") ? true : false;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public bool ReqInvReqRecItemCancel(int stkId,int recPay_id,int RecPayItemId,string updateBy)
        //{
        //    try
        //    {


        //        List<SqlParameter> param = new List<SqlParameter>();
        //        param.Add(new SqlParameter("@Stock_ID", stkId));
        //        param.Add(new SqlParameter("@RecPay_Id", recPay_id));
        //        param.Add(new SqlParameter("@RecPay_ItemID", RecPayItemId));
        //        param.Add(new SqlParameter("@Update_By", updateBy));

        //       new DatabaseHelper().ExecuteNonQuery("sp_Req_Inv_ReqRecItem_Cancel", param);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}







        #endregion


    }
}