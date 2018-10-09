using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace GPlus.PRPO.PRPOHelper
{
    public class DatabaseAccess
    {
        private SqlConnection   Connection;
        private SqlCommand      Command;
        private bool            UseTransaction;

        private string ConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

        /// <summary>
        ///     สร้างออบเจ็คและเปิดการเชื่อมต่อกับฐานข้อมูล
        /// </summary>
        protected DatabaseAccess()
        {
            Connection = new SqlConnection(ConnectionString);
            Command = new SqlCommand();
            Connection.Open();
            Command.Connection = Connection;
        }

        public void BeginTransaction()
        {
            Command.Transaction = Connection.BeginTransaction(IsolationLevel.ReadCommitted);
            UseTransaction = true;
        }

        public void CommitTransaction()
        {
            Command.Transaction.Commit();
            UseTransaction = false;
            Connection.Close();
        }

        public void RollbackTransaction()
        {
            Command.Transaction.Rollback();
            UseTransaction = false;
            Connection.Close();
        }

        protected DataTable ExecuteDataTable(string storeProcedureName, List<SqlParameter> sqlParams)
        {
            if ((!UseTransaction) && (Connection.State == ConnectionState.Closed))
                Connection.Open();

            DataTable dt = new DataTable();

            Command.CommandText = storeProcedureName;
            Command.CommandType = CommandType.StoredProcedure;

            Command.Parameters.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter(Command);

            for (int i = 0; i < sqlParams.Count; ++i)
            {
                adapter.SelectCommand.Parameters.Add(sqlParams[i]);
            }

            try
            {
                adapter.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            if ((!UseTransaction) && (Connection.State == ConnectionState.Open))
                Connection.Close();

            return dt;
        }

        protected object ExecuteScalar(string storeProcedureName, List<SqlParameter> sqlParams)
        {
            if ((!UseTransaction) && (Connection.State == ConnectionState.Closed))
                Connection.Open();

            Command.CommandText = storeProcedureName;
            Command.CommandType = CommandType.StoredProcedure;

            Command.Parameters.Clear();

            foreach (SqlParameter sqlParam in sqlParams)
                Command.Parameters.Add(sqlParam);

            object identity = Command.ExecuteScalar();

            if ((!UseTransaction) && (Connection.State == ConnectionState.Open))
                Connection.Close();

            return identity;
        }

        protected int ExecuteNonQuery(string storeProcedureName, List<SqlParameter> sqlParams)
        {
            if ((!UseTransaction) && (Connection.State == ConnectionState.Closed))
                Connection.Open();

            Command.CommandText = storeProcedureName;
            Command.CommandType = CommandType.StoredProcedure;

            Command.Parameters.Clear();

            foreach (SqlParameter sqlParam in sqlParams)
                Command.Parameters.Add(sqlParam);

            int numRows = Command.ExecuteNonQuery();

            if ((!UseTransaction) && (Connection.State == ConnectionState.Open))
                Connection.Close();

            return numRows;
        }
    }
}