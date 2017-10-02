
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace webDmsApi.Controllers
{
    public class SqlHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DBServer"].ConnectionString;
        private static int commandTimeout = 30;

        /// <summary>
        /// 命令超时时长
        /// </summary>
        public static int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        #region ExecuteNonQuery

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                cmd.CommandTimeout = CommandTimeout;

                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);

                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                cmd.CommandTimeout = CommandTimeout;
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="trans">The trans.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;

            }
            catch (Exception exc)
            {
                conn.Close();
                throw exc;
            }
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            SqlConnection conn = new SqlConnection(connectionString);
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令内容</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>reader</returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception exc)
            {
                conn.Close();
                throw exc;
            }
        }

        /// <summary>
        /// 执行数据读取器(当DataReader关闭时，相关的数据库连接不关闭)
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令内容</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>reader</returns>
        public static SqlDataReader ExecuteReaderWithoutClosingConnection(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                SqlDataReader rdr = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception exc)
            {
                conn.Close();
                throw exc;
            }
        }

        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteDataSet(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataAdapter oda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                oda.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }

        #endregion

        #region ExecuteScalar
        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(CommandType.Text, cmdText, commandParameters);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandParameters">The command parameters.</param>
        /// <returns></returns>
        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked	or commited, please	provide	an open	transaction.", "transaction");

            // Create a	command	and	prepare	it for execution
            SqlCommand cmd = new SqlCommand();
            cmd.CommandTimeout = CommandTimeout;
            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // Execute the command & return	the	results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters	from the command object, so	they can be	used again
            cmd.Parameters.Clear();
            return retval;
        }

        ///// <summary>
        ///// Executes the scalar.
        ///// </summary>
        ///// <param name="connectionString">The connection string.</param>
        ///// <param name="cmdType">Type of the CMD.</param>
        ///// <param name="cmdText">The CMD text.</param>
        ///// <param name="commandParameters">The command parameters.</param>
        ///// <returns></returns>
        //public static object ExecuteScalar(SqlConnection connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{
        //    SqlCommand cmd = new SqlCommand();

        //    PrepareCommand(cmd, connectionString, null, cmdType, cmdText, commandParameters);
        //    object val = cmd.ExecuteScalar();
        //    cmd.Parameters.Clear();
        //    return val;
        //}

        #endregion

        /// <summary>
        /// Caches the parameters.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="commandParameters">The command parameters.</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Gets the cached parameters.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns></returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Oras the bit.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string SqlBit(bool value)
        {
            if (value)
                return "Y";
            else
                return "N";
        }

        /// <summary>
        /// Oras the bool.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool SqlBool(string value)
        {
            if (value.Equals("Y"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Prepares the command.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        /// <param name="conn">The conn.</param>
        /// <param name="trans">The trans.</param>
        /// <param name="cmdType">Type of the CMD.</param>
        /// <param name="cmdText">The CMD text.</param>
        /// <param name="cmdParms">The CMD parms.</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }

        #region IDataBase 成员

        /// <summary>
        /// 获得数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public static System.Data.Common.DbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        #endregion
    }
}