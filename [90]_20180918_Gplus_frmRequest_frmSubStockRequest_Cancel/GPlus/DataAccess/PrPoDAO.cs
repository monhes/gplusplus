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
    public class PrPoDAO : DataAccessBase
    {
        public DataSet GetAllDivDep(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_PRPO_report_GetAllDivDep", sqlParamList);
        }

        public string GetDivCode(int orgID)
        {
            DataSet ds = new DatabaseHelper().ExecuteQuery(
                       "SELECT Div_Code FROM [dbo].Inv_OrgStructure WHERE OrgStruc_Id="
                       + orgID);

            return ds.Tables[0].Rows[0]["Div_Code"].ToString();
        }

        /// <summary>
        ///     Retrieve division name and develop name through sp_PRPO_report_divdep.
        /// </summary>
        /// <param name="orgID">
        ///     orgID must be known. Once the user loggin in to the system, This can be retrieved from Session["orgID"].
        /// </param>
        /// <returns>
        ///     Array of two string, 
        ///         string[0] indicates div name
        ///         string[1] indicates dev name.
        /// </returns>
        public string[] GetDivDepName(int orgID)
        {
            SQLParameterList sqlParamList = new SQLParameterList();
            sqlParamList.AddIntegerField("OrgStrucId", orgID);

            DataSet dataSet = ExecuteDataSet("sp_Inv_GetDivDep", sqlParamList);

            return new string[] { dataSet.Tables[0].Rows[0][1].ToString(), dataSet.Tables[0].Rows[0][3].ToString() };
        }

        public DataSet GetPOCode(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_PO_Form1_POCode", sqlParamList);
        }

        public DataSet GetPRCode(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_PR_Form1_PRCode", sqlParamList);
        }

        public DataSet GetSupplierName(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_PO_Form1_Supplier", sqlParamList);
        }

        public DataSet GetOrgName(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_PR_Form1_OrgStruct", sqlParamList);
        }

        public DataSet GetPRTrackingReport(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_PRTrackingReport", sqlParamList);
        }

        public DataSet GetPOTrackingReport(SQLParameterList paramList)
        {
            return ExecuteDataSet("sp_POTrackingReport", paramList);
        }

        public DataSet GetSupplierName1(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_PO_Form1_SupplierName", sqlParamList);
        }
    }
}