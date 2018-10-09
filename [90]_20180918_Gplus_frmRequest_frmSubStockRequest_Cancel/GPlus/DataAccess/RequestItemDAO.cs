using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class RequestItemDAO
    {
        public DataSet GetRequestItem(string status, string requestBy, string requestDateStart, string requestDateEnd, int pageNum, int pageSize,
            string sortField, string sortOrder, string divCode)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Request_By", requestBy));
            param.Add(new SqlParameter("@RequestDate_Start", requestDateStart));
            param.Add(new SqlParameter("@RequestDate_End", requestDateEnd));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));
            // Begin Green Edit
            param.Add(new SqlParameter("@Div_Code", divCode));
            // End Green Edit

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Request_ItemCode_SelectPaging", param);
        }

        public DataTable GetRequestItem(string requestItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Req_ItemCode_ID", requestItemID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_ItemCode_SelectByID", param);
        }

        public DataTable GetRequestItemByName(string itemName)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemName", itemName));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Request_ItemCode_SelectByName", param);
        }

        public string AddRequestItem(string itemName, string packID, string packDesc, string invAttribute, string cateID, string typeID, string formID,
            string subCateID, string quantity, string remark, string requestBy, int orgStrucId)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            if (packID.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID", packID));
            param.Add(new SqlParameter("@Pack_Desc", packDesc));
            param.Add(new SqlParameter("@Inv_Attribute", invAttribute));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (typeID.Trim().Length > 0)
                param.Add(new SqlParameter("@Type_ID", typeID));
            if (formID.Trim().Length > 0)
                param.Add(new SqlParameter("@Form_Id", formID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            if (quantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Quantity", quantity));
            param.Add(new SqlParameter("@Remark", remark));
            param.Add(new SqlParameter("@Request_By", requestBy));
            // Begin Green Edit
            param.Add(new SqlParameter("@OrgStruc_Id", orgStrucId));
            // End Green Edit

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Request_ItemCode_Insert", param).ToString();
        }

        public void UpdateRequestItem(string requestItemID, string itemName, string packID, string packDesc, string invAttribute, string cateID, string typeID, string formID,
            string subCateID, string quantity, string remark, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Req_ItemCode_ID", requestItemID));
            param.Add(new SqlParameter("@Inv_ItemName", itemName));
            if (packID.Trim().Length > 0)
                param.Add(new SqlParameter("@Pack_ID", packID));
            param.Add(new SqlParameter("@Pack_Desc", packDesc));
            param.Add(new SqlParameter("@Inv_Attribute", invAttribute));
            if (cateID.Trim().Length > 0)
                param.Add(new SqlParameter("@Cate_ID", cateID));
            if (typeID.Trim().Length > 0)
                param.Add(new SqlParameter("@Type_ID", typeID));
            if (formID.Trim().Length > 0)
                param.Add(new SqlParameter("@Form_Id", formID));
            if (subCateID.Trim().Length > 0)
                param.Add(new SqlParameter("@SubCate_ID", subCateID));
            if (quantity.Trim().Length > 0)
                param.Add(new SqlParameter("@Quantity", quantity));
            param.Add(new SqlParameter("@Remark", remark));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Request_ItemCode_Update", param);
        }

        public void UpdateRequestItem(string approveItemID, string itemCode, string approveBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Req_ItemCode_ID", approveItemID));
            param.Add(new SqlParameter("@Inv_ItemCode", itemCode));
            //param.Add(new SqlParameter("@Inv_ItemName", itemName));
            param.Add(new SqlParameter("@Approve_By", approveBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Request_ItemCode_UpdateApprove", param);
        }

        public void DeleteRequestItem(string requestItemID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Req_ItemCode_ID", requestItemID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Request_ItemCode_Delete", param);
        }

    }
}