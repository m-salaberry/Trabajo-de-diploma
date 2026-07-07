using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Data.SqlClient;
using Services.Contracts.CustomsException;

namespace DAL.Helpers
{
    internal static class SqlHelper
    {
        readonly static string conString;

        /// <summary>
        /// Initializes the shared connection string from configuration, substituting the SQL user and
        /// password placeholders with their configured values.
        /// </summary>
        static SqlHelper()
        {
            conString = ConfigurationManager.ConnectionStrings["iamDb"].ConnectionString;
            conString = conString.Replace("{sqlUser}", ConfigurationManager.AppSettings["sqlUser"]);
            conString = conString.Replace("{sqlPassword}", ConfigurationManager.AppSettings["sqlPassword"]);
        }
        /// <summary>
        /// Opens a connection and executes a command that does not return rows, returning the number of rows affected.
        /// Failures are logged and rethrown as a <see cref="MySystemException"/> for the DAL layer.
        /// </summary>
        /// <param name="commandText">The SQL statement or stored procedure name to execute.</param>
        /// <param name="commandType">The type of command (for example Text or StoredProcedure).</param>
        /// <param name="parameters">The parameters to pass to the command.</param>
        /// <returns>The number of rows affected by the command.</returns>
        public static Int32 ExecuteNonQuery(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            try
            {
                CheckNullables(parameters);

                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(commandText, conn))
                    {
                        cmd.CommandType = commandType;
                        cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                new DALExceptionHandler(ex.Message).Handler();
                throw new MySystemException(ex.Message, "DAL", ex);
            }
        }

        /// <summary>
        /// Replaces any null parameter values with <see cref="DBNull.Value"/> so they are sent correctly to SQL Server.
        /// </summary>
        /// <param name="parameters">The parameters to inspect and normalize.</param>
        private static void CheckNullables(SqlParameter[] parameters)
        {
            foreach (SqlParameter item in parameters)
            {
                if (item.SqlValue == null)
                {
                    item.SqlValue = DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Set the connection, command, and then execute the command and only return one value.
        /// </summary>
        public static Object ExecuteScalar(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(conString))
                {
                    using (SqlCommand cmd = new SqlCommand(commandText, conn))
                    {
                        cmd.CommandType = commandType;
                        cmd.Parameters.AddRange(parameters);

                        conn.Open();
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                new DALExceptionHandler(ex.Message).Handler();
                throw new MySystemException(ex.Message, "DAL", ex);
            }

        }

        /// <summary>
        /// Set the connection, command, and then execute the command with query and return the reader.
        /// </summary>
        public static SqlDataReader ExecuteReader(String commandText,
            CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(conString);
            try
            {
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.CommandType = commandType;
                    cmd.Parameters.AddRange(parameters);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    return reader;
                }
            }
            catch (Exception ex)
            {
                conn.Dispose();
                new DALExceptionHandler(ex.Message).Handler();
                throw new MySystemException(ex.Message, "DAL", ex);
            }

        }
    }
}
