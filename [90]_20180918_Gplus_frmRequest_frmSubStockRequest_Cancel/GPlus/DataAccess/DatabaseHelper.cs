using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace GPlus.DataAccess
{
    public class DatabaseHelper
    {
        private string cnnString = "";
        public string ConnectionString
        {
            get
            {
                if (cnnString == "")
                    cnnString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
                return cnnString;
            }
            set
            {
                cnnString = value;
            }
        }

        public string TestConnection()
        {
            this.commandType = CommandType.Text;
            try
            {
                this.ExecuteNonQuery("SELECT 1");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        private CommandType commandType = CommandType.StoredProcedure;
        public CommandType SQLCommandType
        {
            get
            {
                return commandType;
            }
            set
            {
                commandType = value;
            }
        }

        public void BackupDatabase(string path)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            string strConnection = this.ConnectionString;
            string databaseName = cnn.Database;
            strConnection = strConnection.Replace(cnn.Database, "master");
            cnn = new SqlConnection(strConnection);
            SqlCommand cmm = new SqlCommand("BACKUP DATABASE [" + databaseName + "] TO " +
                " DISK = N'" + path + "' WITH NOFORMAT, " +
                " INIT,  NAME = N'Docs-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10 ", cnn);
            cmm.CommandType = this.SQLCommandType;

            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();

                cmm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
        }

        public void RestoreDatabase(string path)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            string strConnection = this.ConnectionString;
            string databaseName = cnn.Database;
            strConnection = strConnection.Replace(cnn.Database, "master");
            cnn = new SqlConnection(strConnection);
            SqlCommand cmm = new SqlCommand("RESTORE DATABASE [" + databaseName + "] FROM  DISK = N'" + path + "' WITH  FILE = 1,  NOUNLOAD,  STATS = 10", cnn);
            cmm.CommandType = this.SQLCommandType;

            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();

                cmm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            return this.ExecuteNonQuery(sql, new List<SqlParameter>());
        }

        public int ExecuteNonQuery(string sql, List<SqlParameter> param)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            SqlCommand cmm = new SqlCommand(sql, cnn);
            cmm.CommandType = this.SQLCommandType;

            int retValue = 0;

            for (int i = 0; i < param.Count; i++)
            {
                cmm.Parameters.Add(ToDBDateParam(param[i]));
            }
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();

                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                retValue = cmm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");
            }

            return retValue;
        }

        public object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(sql, new List<SqlParameter>());
        }

        public object ExecuteScalar(string sql, List<SqlParameter> param)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            SqlCommand cmm = new SqlCommand(sql, cnn);
            cmm.CommandType = this.SQLCommandType;
            object retValue = 0;

            for (int i = 0; i < param.Count; i++)
            {
                cmm.Parameters.Add(ToDBDateParam(param[i]));

            }
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();

                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                retValue = cmm.ExecuteScalar();


            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");
            }

            return retValue;
        }

        public DataTable ExecuteDataTable(string sql)
        {
            return this.ExecuteDataTable(sql, new List<SqlParameter>());
        }

        public DataTable ExecuteDataTable(string sql, List<SqlParameter> param)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            SqlDataAdapter ad = new SqlDataAdapter(sql, cnn);
            DataTable dt = new DataTable();
            ad.SelectCommand.CommandType = this.SQLCommandType;


            for (int i = 0; i < param.Count; i++)
            {
                ad.SelectCommand.Parameters.Add(ToDBDateParam(param[i]));
            }
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                ad.Fill(dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");
            }

            return dt;
        }

        public DataSet ExecuteDataSet(string sql)
        {
            return this.ExecuteDataSet(sql, new List<SqlParameter>());
        }

        public DataSet ExecuteDataSet(string sql, List<SqlParameter> param)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            SqlDataAdapter ad = new SqlDataAdapter(sql, cnn);
            DataSet ds = new DataSet();
            ad.SelectCommand.CommandType = this.SQLCommandType;


            for (int i = 0; i < param.Count; i++)
            {
                ad.SelectCommand.Parameters.Add(ToDBDateParam(param[i]));
            }
            try
            {
                if (cnn.State == ConnectionState.Closed) cnn.Open();
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                ad.Fill(ds);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("th-TH");
            }

            return ds;
        }

        public SqlParameter ToDBDateParam(SqlParameter myParam)
        {
            if (myParam.Value  is System.DateTime)
            {
                myParam.Value = DateString((DateTime)myParam.Value);
            }

            return myParam;
        }

        public string DateString(DateTime date)
        {
            int year = date.Year;
            if (year > 2500)
                year -= 543;
            else if(year < 1700)
                year += 543;

            return date.Month.ToString() + "/" + date.Day.ToString() + "/" + year.ToString();
        }

#region Green 19072013

        public DataSet ExecuteQuery(string query)
        {
            SqlConnection cnn = new SqlConnection(this.ConnectionString);
            SqlDataAdapter ad = new SqlDataAdapter();
            ad.SelectCommand = new SqlCommand(query, cnn);

            DataSet ds = new DataSet();

            try
            {
                if (cnn.State == ConnectionState.Closed)
                    cnn.Open();
                ad.Fill(ds);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }

            return ds;
        }

#endregion

    }
}