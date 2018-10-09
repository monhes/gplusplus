using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace GPlus.DataAccess
{
    public class CalendarDAO : DataAccessBase
    {
        public CalendarDAO()
        {
        }

        public DataSet Insert(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_Calendar_Insert", sqlParamList);
        }

        public DataSet Select(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_Calendar_Select", sqlParamList);
        }

        public DataSet Update(SQLParameterList sqlParamList)
        {
            return ExecuteDataSet("sp_Inv_Calendar_Update", sqlParamList);
        }
    }
}