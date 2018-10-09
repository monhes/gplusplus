using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class ApproverDAO
    {
        public DataTable GetApprover(string orgStructureID, string approvePart)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));
            param.Add(new SqlParameter("@Approve_Part", approvePart));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Approve_Select", param);
        }

        public DataSet GetApproverAndTemp(string orgStructureID, string approvePart)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));
            param.Add(new SqlParameter("@Approve_Part", approvePart));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_Approve_Temp_Select", param);
        }

        public string AddApprover(string orgStructureID, string approvePart, string approveID, DateTime effectiveDate, DateTime expireDate,
            string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));
            param.Add(new SqlParameter("@Approve_Part", approvePart));
            param.Add(new SqlParameter("@Approve_ID", approveID));
            if(effectiveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Effective_Date", effectiveDate));
            if (expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Approve_Insert", param).ToString();
        }

        public void UpdateApprover(string approveOrgID, DateTime effectiveDate, DateTime expireDate,
            string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ApproveOrg_ID", approveOrgID));
            if (effectiveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Effective_Date", effectiveDate));
            if (expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteScalar("sp_Inv_Approve_Update", param);
        }

        public void DeleteApprover(string approveOrgID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ApproveOrg_ID", approveOrgID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Approve_Delete", param);
        }



        public DataTable GetTempApprover(string orgStructureID, string approvePart)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));
            param.Add(new SqlParameter("@Approve_Part", approvePart));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_TempApprove_Select", param);
        }

        public string AddTempApprover(string orgStructureID, string approvePart, string accountID, DateTime effectiveDate, DateTime expireDate,
           string reason, string status, string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));
            param.Add(new SqlParameter("@Approve_Part", approvePart));
            param.Add(new SqlParameter("@Account_ID", accountID));
            if (effectiveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Effective_Date", effectiveDate));
            if (expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Reason", reason));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_TempApprove_Insert", param).ToString();
        }

        public void UpdateTempApprover(string tempApproveID, DateTime effectiveDate, DateTime expireDate,
            string reason, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TempApprove_ID", tempApproveID));
            if (effectiveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Effective_Date", effectiveDate));
            if (expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Reason", reason));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteScalar("sp_Inv_TempApprove_Update", param);
        }

        public void DeleteTempApprover(string tempApproveID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@TempApprove_ID", tempApproveID));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_TempApprove_Delete", param);
        }

    }
}