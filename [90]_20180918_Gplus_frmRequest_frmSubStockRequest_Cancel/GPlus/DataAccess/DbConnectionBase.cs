using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GPlus.DataAccess
{
    public class DbConnectionBase
    {
        public DatabaseHelper DbQuery { get; set; }
        public List<SqlParameter> Parameters { get; private set; }
        private SqlConnection _conn;
        public DbConnectionBase()
        {
            this.DbQuery = new DatabaseHelper();
            this.Parameters = new List<SqlParameter>();
            this._conn = new SqlConnection(DbQuery.ConnectionString);
        }
        
        private SqlTransaction Trans { get; set; }
        private bool _useTransaction = false;
        /// <summary>
        /// Get or set transaction
        /// </summary>
        public bool UseTransaction 
        {
            get { return _useTransaction; }
            set
            {
                _useTransaction = value;
                if (value)
                {
                    if (_conn.State == ConnectionState.Closed)
                    {
                        _conn.Open();
                    }
                    Trans = _conn.BeginTransaction();

                }
            }
        }
        /// <summary>
        /// This method use to execution query with non query types
        /// </summary>
        /// <param name="spName">StoredProcedure Name</param>
        /// <param name="parameters">Parameter List</param>
        public void ExecuteNonQuery(string spName, List<SqlParameter> parameters)
        {
            try
            {

                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                SqlCommand comm = new SqlCommand(spName, _conn);
                if (this.UseTransaction)
                {
                    comm.Transaction = Trans;
                }
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                {
                    comm.Parameters.Add(param);
                }

                comm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Inner Exception: " + ex.InnerException);
#endif
            }
        }
        /// <summary>
        /// This method use for commit transaction
        /// </summary>
        public void CommitTransaction()
        {
            if (this.Trans != null)
            {
                try
                {
                    Trans.Commit();
                    if (_conn.State == ConnectionState.Open)
                    {
                        _conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
#if DEBUG
                    Console.WriteLine("Commit Exception: " + ex.Message);
                    Console.WriteLine("Commit inner Exception: " + ex.InnerException);
#endif
                }
            }
        }
        /// <summary>
        /// This method use for create sql parameter
        /// </summary>
        /// <param name="fieldName">field name</param>
        /// <param name="value">value object</param>
        /// <returns>sqlparameter</returns>
        public SqlParameter Parameter(string fieldName, object value)
        {
            return new SqlParameter(fieldName, value);
        }
        /// <summary>
        /// This method use to set parameter list ready to used.
        /// </summary>
        public void BeginParameter()
        {
            this.Parameters.Clear();
        }
        /// <summary>
        /// This method use to execution query with DataTable result
        /// </summary>
        /// <param name="spName">StoredProcedure Name</param>
        /// <param name="parameters">Parameter List</param>
        public DataTable ExecuteDataTable(string spName, List<SqlParameter> parameters = null)
        {
            if (parameters == null)
            {
                return this.DbQuery.ExecuteDataTable(spName, new List<SqlParameter>());
            }
            else
            {
                return this.DbQuery.ExecuteDataTable(spName, parameters);
            }
        }

        /// <summary>
        /// This method use to execution query with DataSet result
        /// </summary>
        /// <param name="spName">StoredProcedure Name</param>
        /// <param name="parameters">Parameter List</param>
        public DataSet ExecuteDataSet(string spName, List<SqlParameter> parameters = null)
        {
            if (parameters == null)
            {
                return this.DbQuery.ExecuteDataSet(spName, new List<SqlParameter>());
            }
            else
            {
                return this.DbQuery.ExecuteDataSet(spName, parameters);
            }
        }

        #region Ibest 19072013

        public int ExecuteNonQuery_Chk(string spName, List<SqlParameter> parameters)
        {
            try
            {

                if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                SqlCommand comm = new SqlCommand(spName, _conn);
                if (this.UseTransaction)
                {
                    comm.Transaction = Trans;
                }
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (SqlParameter param in parameters)
                {
                    comm.Parameters.Add(param);
                }

                comm.ExecuteNonQuery();

                return 1;

            }
            catch (Exception ex)
            {
//#if DEBUG
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Inner Exception: " + ex.InnerException);

                return 0;
//#endif
            }
        }

        public DataSet ExecuteDataSet_2(string spName, List<SqlParameter> parameters = null)
        {
            if (parameters == null)
            {
                return this.DbQuery.ExecuteDataSet(spName, new List<SqlParameter>());
            }
            else
            {

                SqlDataAdapter ad = new SqlDataAdapter(spName, this._conn);
                DataSet ds = new DataSet();
                ad.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                ad.SelectCommand.Transaction = this.Trans;

                foreach (SqlParameter param in parameters)
                {
                    ad.SelectCommand.Parameters.Add(param);
                }
                try
                {
                    if (_conn.State == ConnectionState.Closed) _conn.Open();
                    //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    ad.Fill(ds);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    //_conn.Close();
                    //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");
                }

                return ds;
            }
        }

        public void Rollback()
        {
            if (this.Trans != null)
            {
                try
                {
                    Trans.Rollback();
                    if (_conn.State == ConnectionState.Open)
                    {
                        _conn.Close();
                    }
                }
                catch (Exception ex)
                {

#if DEBUG
                    Console.WriteLine("Commit Exception: " + ex.Message);
                    Console.WriteLine("Commit inner Exception: " + ex.InnerException);
#endif
                }
            }
        }

        #endregion
    }
}