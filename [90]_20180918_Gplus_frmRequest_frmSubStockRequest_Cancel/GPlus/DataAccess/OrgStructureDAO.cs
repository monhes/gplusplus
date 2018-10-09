using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GPlus.DataAccess
{
    public class OrgStructureDAO : DbConnectionBase
    {
        public DataSet GetOrgStructure(string divCode, string depCode, string divName, string depName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Div_Code", divCode));
            param.Add(new SqlParameter("@Dep_Code", depCode));
            param.Add(new SqlParameter("@Div_Name", divName));
            param.Add(new SqlParameter("@Dep_Name", depName));
            param.Add(new SqlParameter("@OrgStruc_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_OrgStructure_SelectPaging", param);
        }

        public DataSet GetOrgStructure_new(string divCode, string depCode, string divName, string depName, string status, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Div_Code", divCode));
            param.Add(new SqlParameter("@Dep_Code", depCode));
            param.Add(new SqlParameter("@Div_Name", divName));
            param.Add(new SqlParameter("@Dep_Name", depName));
            param.Add(new SqlParameter("@OrgStruc_Status", status));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_OrgStructure_SelectPaging_new", param);
        }

        public DataTable GetOrgStructure(string orgStructureID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_OrgStructure_SelectByID", param);
        }

        public string AddOrgStructure(string divCode, string depCode, string description, string notApproveFlag, string status, string buildingFloorID
            , string createBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Div_Code", divCode));
            param.Add(new SqlParameter("@Dep_Code", depCode));
            param.Add(new SqlParameter("@Description", description));
            param.Add(new SqlParameter("@NotApprove_Flag", notApproveFlag));
            param.Add(new SqlParameter("@OrgStruc_Status", status));
            if (buildingFloorID.Trim().Length > 0)
                param.Add(new SqlParameter("@Building_FloorId", buildingFloorID));
            param.Add(new SqlParameter("@Create_By", createBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_OrgStructure_Insert", param).ToString();
        }

        public string UpdateOrgStructure(string orgStructureID, string divCode, string depCode, string description, string notApproveFlag, string status,
            string buildingFloorID, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));
            param.Add(new SqlParameter("@Div_Code", divCode));
            param.Add(new SqlParameter("@Dep_Code", depCode));
            param.Add(new SqlParameter("@Description", description));
            param.Add(new SqlParameter("@NotApprove_Flag", notApproveFlag));
            param.Add(new SqlParameter("@OrgStruc_Status", status));
            if (buildingFloorID.Trim().Length > 0)
                param.Add(new SqlParameter("@Building_FloorId", buildingFloorID));
            param.Add(new SqlParameter("@Update_By", updateBy));

            return new DatabaseHelper().ExecuteScalar("sp_Inv_OrgStructure_Update", param).ToString();
        }


        public DataTable GetOrgStk(string orgStructureID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStructureID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_OrgStk_Select", param);
        }

        public void UpdateOrgStk(string orgStrucID, string stockID, string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_ID", orgStrucID));
            param.Add(new SqlParameter("@Stock_ID", stockID));
            param.Add(new SqlParameter("@OrgStk_Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_OrgStk_Update", param);
        }

        public DataTable GetBuilding()
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Select");
        }

        public DataTable GetBuildingFloor(string buildingID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Building_Id", buildingID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Building_Floor_Select", param);
        }

        public DataTable GetApproveByPass(string orgStrucID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_Approve_ByPass_SelectByID", param);
        }

        public void UpdateApproveByPass(string orgStrucID, string approvePart, string approveFlag, DateTime effectiveDate, DateTime expireDate,
            string status, string updateBy)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStruc_Id", orgStrucID));
            param.Add(new SqlParameter("@Approve_Part", approvePart));
            param.Add(new SqlParameter("@Approve_Flag", approveFlag));
            if (effectiveDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Effective_Date", effectiveDate));
            if (expireDate > DateTime.MinValue)
                param.Add(new SqlParameter("@Expire_Date", expireDate));
            param.Add(new SqlParameter("@Status", status));
            param.Add(new SqlParameter("@Update_By", updateBy));

            new DatabaseHelper().ExecuteNonQuery("sp_Inv_Approve_ByPass_Update", param);
        }

        #region Nin 22072013
        public DataTable GetOrgStructureDivDep(int orgStructureID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@OrgStrucId", orgStructureID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_GetDivDep", param);
        }
        public DataSet GetOrgBudget(string BudgetYear, string OrgStrucID, int pageNum, int pageSize,
                 string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Budget_Year", BudgetYear));
            param.Add(new SqlParameter("@OrgStruc_Id", OrgStrucID));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_OrgBudget_CateItem_SelectPaging", param);
        }

        public DataTable GetOrgBudgetByID(string BudgetYear, string OrgStrucID)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Budget_Year", BudgetYear));
            param.Add(new SqlParameter("@OrgStruc_Id", OrgStrucID));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_OrgBudget_CateItem_ByID", param);
        }

        public DataTable OrgBudgetInsertUpdate(List<SqlParameter> param)
        {
            return new DatabaseHelper().ExecuteDataTable("sp_Inv_OrgBudget_InsertUpdate", param);
        }

        public DataSet GetTypeRequisition(string cate_id, string type_id, string org_id, int pageNum, int pageSize,
           string sortField, string sortOrder)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_Id", cate_id));
            param.Add(new SqlParameter("@Type_Id", type_id));
            param.Add(new SqlParameter("@OrgStruc_ID", org_id));
            param.Add(new SqlParameter("@PageNum", pageNum));
            param.Add(new SqlParameter("@PageSize", pageSize));
            param.Add(new SqlParameter("@SortField", sortField));
            param.Add(new SqlParameter("@SortOrder", sortOrder));

            return new DatabaseHelper().ExecuteDataSet("sp_Inv_TypeRequisition_SelectPaging", param);
        }

        public DataTable GetTypeRequisitionByID(string cate_id, string type_id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Cate_Id", cate_id));
            param.Add(new SqlParameter("@Type_Id", type_id));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_TypeRequisition_SelectByID", param);
        }

        public bool TypeRequisition_InsertUpdate(DataTable dtFile, DataTable dtFileDeleted, string CateID, string TypeID, string Create_By)
        {

            bool chk = false;

            this.UseTransaction = true; // Request Transaction
            
            DataRow[] drs = null;

            if (dtFile != null)
            {
                drs = dtFile.Select("TrypeReq_ID < 0");
            }


            int chk_insert = 0;
            int chk_delete = 0;

            if (drs != null)
            {
                chk_insert = drs.Length;
            }

            if (dtFileDeleted != null)
            {
                chk_delete = dtFileDeleted.Rows.Count;
            }

            int chk_Data_All = chk_insert + chk_delete;
            int chk_Data = 0;

            try
            {
                this.BeginParameter();

                // ลบรายการ Inv_TypeRequisition

                if (dtFileDeleted != null)
                {
                    if (dtFileDeleted.Rows.Count > 0)
                    {
                        foreach (DataRow r in dtFileDeleted.Rows)
                        {
                            int TrypeReq_ID = Convert.ToInt32(r["TrypeReq_ID"].ToString() == "" ? "0" : r["TrypeReq_ID"].ToString());

                            this.BeginParameter();

                            this.Parameters.Add(Parameter("@TrypeReq_ID", TrypeReq_ID));

                            int result = 0;

                            result = this.ExecuteNonQuery_Chk("sp_Inv_TypeRequisition_Delete", this.Parameters);

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
                    }
                }

                // --------------------------------- เพิ่มข้อมูล-----------------------------

                // เพิ่มรายการ Inv_TypeRequisition
                if (dtFile.Rows.Count > 0 && dtFile != null)
                {
                    foreach (DataRow row in dtFile.Rows)
                    {
                        if (Convert.ToInt32(row["TrypeReq_ID"].ToString()) < 0)
                        {
                            this.BeginParameter();

                            this.Parameters.Add(Parameter("@Cate_Id", CateID));
                            this.Parameters.Add(Parameter("@Type_Id", TypeID));
                            this.Parameters.Add(Parameter("@OrgStruc_ID", row["OrgStruc_Id"].ToString()));
                            this.Parameters.Add(Parameter("@Status", "1"));
                            this.Parameters.Add(Parameter("@Create_By", Create_By));
                            
                            int result = 0;

                            result = this.ExecuteNonQuery_Chk("sp_Inv_TypeRequisition_Insert", this.Parameters);

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

        #endregion

        #region Ed 31/07/2013
        public DataTable GetOrgStructureDivDepAll(string Div_code)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Code", Div_code));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_GetDivDep_Select", param);
        }
        #endregion Ed 31/07/2013

        public DataTable GetOrgStructureByDivCode(string divCode)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@DivCode", divCode));

            return new DatabaseHelper().ExecuteDataTable("sp_Inv_OrgStructure_GetOrgIdByDivCode", param);
        }
    }
}