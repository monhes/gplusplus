using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GPlus.DataAccess
{
    public class UserDAO
    {
        public DataSet GetAccount(string userGroupID, string orgStrucID, string userName, string firstName, string lastName, string status, 
            int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_ID", userGroupID));
            param.Add(new SqlParameter("@OrgStruc_ID", orgStrucID));
            param.Add(new SqlParameter("@Account_UserName", userName));
            param.Add(new SqlParameter("@Account_Fname", firstName));
            param.Add(new SqlParameter("@Account_Lname", lastName));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Gpluz_Account_SelectPaging", param);
        }

        public DataTable GetAccount(string accountID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_ID", accountID));

            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_Account_SelectByID", param);
        }

        public DataSet GetAccount(string userName, string password)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_UserName", userName));
            param.Add(new SqlParameter("@Account_Password", password));

            return new DatabaseHelper().ExecuteDataSet("sp_Gpluz_Account_SelectLogin", param);
        }


        public string AddAccount(string userGroupID, string departmentID, string userName, string password, string firstName, string lastName,
            string email, string stockID, string extNo, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_ID", userGroupID));
            param.Add(new SqlParameter("@OrgStruc_ID", departmentID));
            if(stockID.Trim().Length > 0)
                param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Account_UserName", userName));
            param.Add(new SqlParameter("@Account_Password", password));
            param.Add(new SqlParameter("@Account_Fname", firstName));
            param.Add(new SqlParameter("@Account_Lname", lastName));
            param.Add(new SqlParameter("@Account_Email", email));
            param.Add(new SqlParameter("@Ext_No", extNo));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Gpluz_Account_Insert", param).ToString();
        }

        public void UpdateAccount(string userID, string userGroupID, string departmentID, string stockID, string firstName, string lastName,
            string email, string extNo, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_Id", userID));
            param.Add(new SqlParameter("@UserGroup_Id", userGroupID));
            param.Add(new SqlParameter("@OrgStruc_ID", departmentID));
            if (stockID.Trim().Length > 0)
                param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@Account_Fname", firstName));
            param.Add(new SqlParameter("@Account_Lname", lastName));
            param.Add(new SqlParameter("@Account_Email", email));
            param.Add(new SqlParameter("@Ext_No", extNo));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Gpluz_Account_Update", param);
        }

        public void UpdateAccount(string userID, string password, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Account_ID", userID));
            param.Add(new SqlParameter("@Account_Password", password));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Gpluz_Account_UpdatePassword", param);
        }



        public DataSet GetUserGroup(string userGroupCode, string userGroupName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_Code", userGroupCode));
            param.Add(new SqlParameter("@UserGroup_Name", userGroupName));
            param.Add(new SqlParameter("@UserGroup_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_UserGroup_SelectPaging", param);
        }

        public DataTable GetUserGroup(string userGroupID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_ID", userGroupID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_UserGroup_SelectByID", param);
        }

        public string AddUserGroup(string userGroupCode, string userGroupName, string userGroupDesc, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_Code", userGroupCode));
            param.Add(new SqlParameter("@UserGroup_Name", userGroupName));
            param.Add(new SqlParameter("@UserGroup_Desc", userGroupDesc));
            param.Add(new SqlParameter("@UserGroup_Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_UserGroup_Insert", param).ToString();
        }

        public string UpdateUserGroup(string userGroupID, string userGroupCode, string userGroupName, string userGroupDesc, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_ID", userGroupID));
            param.Add(new SqlParameter("@UserGroup_Code", userGroupCode));
            param.Add(new SqlParameter("@UserGroup_Name", userGroupName));
            param.Add(new SqlParameter("@UserGroup_Desc", userGroupDesc));
            param.Add(new SqlParameter("@UserGroup_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_UserGroup_Update", param).ToString();
        }




        public DataTable GetUserGroupMenu(string userGroupID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_Id", userGroupID));

            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_UserGroupMenu_SelectByUserGroupID", param);
        }

        public void AddUserGroupMenu(string menuID, string userGroupID, string canView, string canAdd, string canUpdate, string canDelete, string canApprove)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Menu_Id", menuID));
            param.Add(new SqlParameter("@UserGroup_Id", userGroupID));
            if (canAdd.Trim().Length > 0) param.Add(new SqlParameter("@Can_View", canView));
            if (canAdd.Trim().Length > 0) param.Add(new SqlParameter("@Can_Add", canAdd));
            if (canUpdate.Trim().Length > 0) param.Add(new SqlParameter("@Can_Update", canUpdate));
            if (canDelete.Trim().Length > 0) param.Add(new SqlParameter("@Can_Delete", canDelete));
            if (canApprove.Trim().Length > 0) param.Add(new SqlParameter("@Can_Approve", canApprove));

            new DatabaseHelper().ExecuteNonQuery("sp_Gpluz_UserGroupMenu_Add", param);
        }

        public void DeleteUserGroupMenu(string userGroupID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserGroup_Id", userGroupID));

            new DatabaseHelper().ExecuteNonQuery("sp_Gpluz_UserGroupMenu_Delete", param);
        }


        public DataTable GetMenuAll()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_Menu_Select");
        }



        public DataTable GetUserGroupUser(string userID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@User_ID", userID));

            return new DatabaseHelper().ExecuteDataTable("sp_Gpluz_UserGroupUser_Select", param);
        }

        public void AddUserGroupUser(string userID, string userGroupID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@User_ID", userID));
            param.Add(new SqlParameter("@UserGroup_ID", userGroupID));

            new DatabaseHelper().ExecuteNonQuery("sp_Gpluz_UserGroupUser_Insert", param);
        }

        public void DeleteUserGroupUser(string userID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@User_ID", userID));

            new DatabaseHelper().ExecuteNonQuery("sp_Gpluz_UserGroupUser_Delete", param);
        }

        #region Nin
        public DataSet GetAllAccountByDivCode(string orgStrucID, string status, int pageNum, int pageSize, string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_ID", orgStrucID));
            param.Add(new SqlParameter("@Account_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Gpluz_Account_GetAllAccountByDivCode", param);
        }
        #endregion

    }
}
