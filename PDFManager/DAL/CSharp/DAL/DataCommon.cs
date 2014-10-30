using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.Data;


namespace PDFManager
{
    public class DataCommon
    {

        /// <summary>
        /// Executes a SQL Query and returns the results as a SafeReader object
        /// </summary>
        /// <param name="SQLQuery">The Query to execute</param>
        /// <param name="PID">The Program ID that matches the configured connection string name</param>
        /// <returns></returns>
        public static SafeReader ExecuteSafeReader(string SQLQuery, string PID)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(SQLQuery, cn))
                {
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        SafeReader sr = new SafeReader(dr);
                        return sr;
                    }
                }
            }
        }


        /// <summary>
        /// Returns data as a safereader
        /// </summary>
        /// <param name="StoredProcName"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        /// <remarks>
        /// [07/02/10] Created
        /// </remarks>
        public static SafeReader ExecuteSafeReader(string StoredProcName, SqlParameter Parameter, string PID)
        {
            SqlParameter[] ps = new SqlParameter[]{Parameter};
            return ExecuteSafeReader(StoredProcName, ps, PID);
        }


        /// <summary>
        /// Returns data in a safe reader
        /// </summary>
        /// <param name="bo"></param>
        /// <remarks>
        /// [07/02/10] Created
        /// </remarks>
        public static SafeReader ExecuteSafeReader(string StoredProcName, SqlParameter[] Parameters, string PID)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(StoredProcName, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(Parameters);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        SafeReader sr = new SafeReader(dr);
                        return sr;
                    }
                }
            }
        }


        /// <summary>
        /// Executes a SQL Query that returns no results
        /// </summary>
        /// <param name="SQLQuery">The Query to execute</param>
        /// <param name="PID">The Program ID that matches the configured connection string name</param>
        /// <returns></returns>
        public static void ExecuteNonQuery(string SQLQuery, string PID)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(SQLQuery, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }        



        /// <summary>
        /// Calls a stored procedure with an array of parameters
        /// </summary>
        /// <param name="StoredProcName"></param>
        /// <param name="Parameters"></param>
        public static void ExecuteNonQuery(string StoredProcName, SqlParameter[] Parameters, string PID)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(StoredProcName, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(Parameters);
                    cmd.ExecuteNonQuery();
                }
            }

        }


        /// <summary>
        /// Calls a stored procedure with one parameter
        /// </summary>
        /// <param name="StoredProcName"></param>
        /// <param name="Parameter"></param>
        public static void ExecuteNonQuery(string StoredProcName, SqlParameter Parameter, string PID)
        {
            SqlParameter[] ps = new SqlParameter[] { Parameter };
            ExecuteNonQuery(StoredProcName, ps, PID);
        }



        /// <summary>
        /// Executes a SQL Query and returns the result
        /// </summary>
        /// <param name="SQLQuery">The Query to execute</param>
        /// <param name="PID">The Program ID that matches the configured connection string name</param>
        /// <returns></returns>
        public static int ExecuteScalar(string SQLQuery, string PID)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(SQLQuery, cn))
                {
                    cmd.CommandType = CommandType.Text;
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
                    return id;
                }
            }
        }


        /// <summary>
        /// Calls a stored procedure with an array of parameters and returns a single value - useful 
        /// when the stored procedure returns a newly created identity
        /// </summary>
        /// <param name="StoredProcName"></param>
        /// <param name="Parameters"></param>
        /// <param name="PID"></param>
        /// <returns></returns>
        public static int ExecuteScalar(string StoredProcName, SqlParameter[] Parameters, string PID)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(StoredProcName, cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(Parameters);
                    int id = Convert.ToInt32(cmd.ExecuteScalar());
					return id;
                }
            }
        }


		/// <summary>
        /// Truncates a table
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="PID"></param>
        /// <remarks>
        /// [11/13/2013] Created
        /// </remarks>
        public static void TruncateTable(string PID, string TableName)
        {
            string cstr = ConfigurationManager.ConnectionStrings[PID].ToString();
            using (SqlConnection cn = new SqlConnection(cstr))
            {
                cn.Open();
                string sql = string.Format("TRUNCATE TABLE [{0}].[{1}]", PID, TableName);
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Ensures that no null value parameters exist in the collection (causes errors)
        /// </summary>
        /// <param name="Parameters"></param>
        private static void EnsureNoNullParameterValues(ref SqlParameter[] Parameters)
        {
            foreach (SqlParameter param in Parameters)
            {
                if (param.Value == null)
                {
                    param.Value = GetDefaultValue(param.SqlDbType);
                    param.SqlValue = param.Value;
                }
            }
        }

        /// <summary>
        /// Get the default value for the type of param
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object GetDefaultValue(SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.BigInt: case SqlDbType.Decimal: case SqlDbType.Float: case SqlDbType.Int: case SqlDbType.Money: case SqlDbType.SmallInt: case SqlDbType.SmallMoney: case SqlDbType.TinyInt:
                    return 0;
                case SqlDbType.Char: case SqlDbType.NChar: case SqlDbType.NText: case SqlDbType.NVarChar: case SqlDbType.Text: case SqlDbType.VarChar:
                    return string.Empty;
                case SqlDbType.UniqueIdentifier:
                    return Guid.Empty;
                default:
                    return new object();
            }
        }       

        
    }
}