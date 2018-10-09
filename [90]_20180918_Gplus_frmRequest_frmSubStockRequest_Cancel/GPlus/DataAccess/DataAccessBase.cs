using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace GPlus.DataAccess
{
    public abstract class DataAccessBase
    {
        public DataAccessBase()
        {
            
        }

        protected DataSet ExecuteDataSet(string storeProcedure, SQLParameterList sqlParamList)
        {
            return new DatabaseHelper().ExecuteDataSet(storeProcedure, sqlParamList.GetSqlParameterList());
        }
    }

    /// <summary>
    ///     SQLParameterList class provides a convenient way to create
    ///     a list of SqlParameter and by this way it is safer than using 
    ///     previous DataAccessBase.MakeSqlParamsList().
    ///     
    ///     By using this class you can benefit with 4 things.
    ///     
    ///     First, It is impossible to introduce missmatch key and value. 
    ///     That is length of key and length of value always match.
    ///    
    ///     Second, It is type safe. There is no need to cast types.
    ///     
    ///     Third, for lazy people, you do not need to type '@' before field names and 
    ///     also no need '=' to indicate a type because method is well-named
    ///     
    ///     Fourth, more clean code :D
    /// </summary>
    public class SQLParameterList
    {
        private List<SqlParameter> _SqlParamsList = new List<SqlParameter>();

        public void AddIntegerField(string fieldName, int value)
        {
            _SqlParamsList.Add(new SqlParameter(AddAtSign(fieldName), value));
        }

        public void AddDoubleField(string fieldName, double value)
        {
            _SqlParamsList.Add(new SqlParameter(AddAtSign(fieldName), value));
        }

        public void AddStringField(string fieldName, string value)
        {
            _SqlParamsList.Add(new SqlParameter(AddAtSign(fieldName), value));
        }

        public void AddDecimalField(string fieldName, decimal value)
        {
            _SqlParamsList.Add(new SqlParameter(AddAtSign(fieldName), value));
        }

        public void AddBooleanField(string fieldName, bool value)
        {
            _SqlParamsList.Add(new SqlParameter(AddAtSign(fieldName), value));
        }

        public List<SqlParameter> GetSqlParameterList()
        {
            return _SqlParamsList;
        }

        private string AddAtSign(string fieldName)
        {
            if (fieldName.Length > 0 && fieldName[0] != '@')
                return "@" + fieldName;
            else
                return fieldName;
        }
    }
}