using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace GPlus.DataAccess
{
    public class TempDAO
    {
        #region Nin
        public string UpdateStockPayManual(string Pay_ID, string Pay_ID2, string Pay_ID3, string Pay_ID4, string Pay_ID5, string Inv_ItemID, string Pack_ID, string Unit_Price)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            if (Pay_ID.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Pay_ID", Convert.ToInt32(Pay_ID)));
            }
            if (Pay_ID2.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Pay_ID2", Convert.ToInt32(Pay_ID2)));
            }
            if (Pay_ID3.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Pay_ID3", Convert.ToInt32(Pay_ID3)));
            }
            if (Pay_ID4.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Pay_ID4", Convert.ToInt32(Pay_ID4)));
            }
            if (Pay_ID5.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Pay_ID5", Convert.ToInt32(Pay_ID5)));
            }
            if (Inv_ItemID.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Inv_ItemID", Convert.ToInt32(Inv_ItemID)));
            }
            if (Pack_ID.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Pack_ID", Convert.ToInt32(Pack_ID)));
            }
            if (Unit_Price.Trim().Length > 0)
            {
                param.Add(new SqlParameter("@Unit_Price", Convert.ToDecimal(Unit_Price)));
            }

            return new DatabaseHelper().ExecuteScalar("sp_Inv_Update_StockPay_Manual", param).ToString();
        }
        #endregion

    }
}
