using MySql.Data.MySqlClient;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Forms;

namespace YamyProject
{
    class DBClass
    {
        public static string Database = "yamycompany";

        public static string GetConnectionString()
        {
            string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);

            string connectionString = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);
            //var builder = new MySqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Secret"].ConnectionString);
            var builder = new MySqlConnectionStringBuilder(connectionString);
            builder.Database = Database;
            return builder.ToString();
        }

        public static MySqlParameter CreateParameter(string name, object value)
        {
            return new MySqlParameter(name, value);
        }

        public static MySqlTransaction CreateTransaction()
        {
            MySqlConnection connection = new MySqlConnection(GetConnectionString());
            connection.Open();
            MySqlTransaction transaction = connection.BeginTransaction();
            return transaction;
        }

        static MySqlCommand CreateCommand(string query, params MySqlParameter[] prmArray)
        {
            MySqlCommand mysqlCommand = null;
            MySqlConnection mysqlConnection = new MySqlConnection(GetConnectionString());

            try
            {
                mysqlCommand = new MySqlCommand(query, mysqlConnection);

                if (!query.Contains(" "))
                    mysqlCommand.CommandType = CommandType.StoredProcedure;

                if (prmArray.Length > 0)
                    mysqlCommand.Parameters.AddRange(prmArray);
                mysqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                mysqlConnection.Close();
                return null;
            }

            return mysqlCommand;
        }

        public static int ExecuteNonQuery(string query, params MySqlParameter[] prmArray)
        {
            MySqlCommand mysqlCommand = CreateCommand(query, prmArray);

            try
            {
                if (mysqlCommand.Connection.State != ConnectionState.Open)
                {
                    mysqlCommand.Connection.Open();
                }

                int affectedRows = mysqlCommand.ExecuteNonQuery();

                return affectedRows;
            }
            catch (MySqlException mysqlEx)
            {
                // Specific MySQL error handling
                Console.WriteLine("MySQL Error: " + mysqlEx.Message);
                Console.WriteLine("Error Code: " + mysqlEx.Number);

                // Example: You can handle duplicate key error here if needed
                if (mysqlEx.Number == 1062) // Duplicate entry
                {
                    Console.WriteLine("Duplicate entry error: " + mysqlEx.Message, "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Console.WriteLine("MySQL Exception: " + mysqlEx.Message, "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Utilities.LogError("ExecuteNonQuery", mysqlEx.Message, frmLogin.userId);
                //throw; // rethrow to upper level
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                //throw;
            }
            finally
            {
                if (mysqlCommand.Connection.State == ConnectionState.Open)
                {
                    mysqlCommand.Connection.Close();
                }
            }
            return 0;
        }

        public static int ExecuteNonQueryCommit(string query, MySqlTransaction transaction = null, params MySqlParameter[] prmArray)
        {
            MySqlCommand mysqlCommand = CreateCommand(query, prmArray);

            try
            {
                if (transaction != null)
                {
                    mysqlCommand.Connection = transaction.Connection;
                    mysqlCommand.Transaction = transaction;
                }

                if (mysqlCommand.Connection.State != ConnectionState.Open)
                {
                    mysqlCommand.Connection.Open();
                }

                int affectedRows = mysqlCommand.ExecuteNonQuery();
                return affectedRows;
            }
            catch (MySqlException mysqlEx)
            {
                Console.WriteLine("MySQL Error: " + mysqlEx.Message);
                Console.WriteLine("Error Code: " + mysqlEx.Number);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
            finally
            {
                if (transaction == null && mysqlCommand.Connection.State == ConnectionState.Open)
                {
                    mysqlCommand.Connection.Close();
                }
            }
        }

        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            command.Parameters.AddRange(parameters);
                        }

                        connection.Open();

                        var result = command.ExecuteScalar();

                        return result == DBNull.Value ? null : result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return 0;
            }
        }


        public static MySqlDataReader ExecuteReader(string query, params MySqlParameter[] prmArray)
        {
            MySqlCommand mysqlCommand = CreateCommand(query, prmArray);
            
            try
            {
                if (mysqlCommand.Connection.State != ConnectionState.Open)
                {
                    mysqlCommand.Connection.Open();
                }

                return mysqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                mysqlCommand.Connection.Close();
                return null;
            }
        }

        public static DataTable ExecuteDataTable(string query, params MySqlParameter[] prmArray)
        {
            DataTable dt = new DataTable();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    using (MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(query, connection))
                    {
                        if (!query.Contains(" "))
                            mysqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        if (prmArray.Length > 0)
                            mysqlDataAdapter.SelectCommand.Parameters.AddRange(prmArray);

                        mysqlDataAdapter.Fill(dt);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                return null;
            }
        }

        public static DataSet ExecuteDataSet(string query, params MySqlParameter[] prmArray)
        {
            DataSet ds = new DataSet();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(GetConnectionString()))
                {
                    connection.Open();

                    using (MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(query, connection))
                    {
                        if (query.Trim().StartsWith("EXEC", StringComparison.OrdinalIgnoreCase) || query.Trim().Contains(" "))
                        {
                            mysqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        }
                        else
                        {
                            mysqlDataAdapter.SelectCommand.CommandType = CommandType.Text;
                        }

                        if (prmArray.Length > 0)
                            mysqlDataAdapter.SelectCommand.Parameters.AddRange(prmArray);

                        mysqlDataAdapter.Fill(ds);
                    }
                }
                return ds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                return null;
            }
        }
        public static bool ExecuteDataTableData(DataTable dataTable)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
                {
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter("SELECT * FROM tbl_general_settings", conn);
                    MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(dataAdapter);

                    if (dataAdapter.Update(dataTable)>0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                return false;
            }
        }

        public static int ExecuteStoredProcedure(string procedureName, List<MySqlParameter> parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(GetConnectionString()))
            using (MySqlCommand cmd = new MySqlCommand(procedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                try
                {
                    conn.Open();
                    int affected = cmd.ExecuteNonQuery();
                    return affected;
                }
                catch (MySql.Data.MySqlClient.MySqlException mysqlEx)
                {
                    // Specific MySQL error handling
                    Console.WriteLine("MySQL Error: " + mysqlEx.Message);
                    Console.WriteLine("Error Code: " + mysqlEx.Number);

                    // Example: You can handle duplicate key error here if needed
                    if (mysqlEx.Number == 1062) // Duplicate entry
                    {
                        Console.WriteLine("Duplicate entry error: " + mysqlEx.Message, "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Console.WriteLine("MySQL Exception: " + mysqlEx.Message, "MySQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    Utilities.LogError("sp_save_item_category", mysqlEx.Message, frmLogin.userId);
                    return 0; // rethrow to upper level
                }
                catch (Exception ex)
                {
                    // General error (non-MySQL)
                    Console.WriteLine("General Error: " + ex.Message);
                    Console.WriteLine("Unexpected error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 0;
                }
            }
        }

        public static void ExecuteNonQueryLog(string query, params MySqlParameter[] prmArray)
        {
            MySqlCommand mysqlCommand = CreateCommand(query, prmArray);
            try
            {
                if (mysqlCommand.Connection.State != ConnectionState.Open)
                {
                    mysqlCommand.Connection.Open();
                }

                int affectedRows = mysqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                //throw;
            }
            finally
            {
                if (mysqlCommand.Connection.State == ConnectionState.Open)
                {
                    mysqlCommand.Connection.Close();
                }
            }
        }

        internal static MySqlConnection GetConnection()
        {
            return new MySqlConnection(GetConnectionString());
        }
    }

    //class DBClass
    //{
    //    public static string connectionString = ConfigurationManager.ConnectionStrings["Secret"].ConnectionString;

    //    public static MySqlParameter CreateParameter(string name, object value)
    //    {
    //        return new MySqlParameter(name, value);
    //    }
    //    public static MySqlTransaction CreateTransaction()
    //    {
    //        MySqlConnection connection = new MySqlConnection(connectionString);
    //        if (connection.State != System.Data.ConnectionState.Open)
    //        {
    //            connection.Open();
    //        }
    //        MySqlTransaction transaction = connection.BeginTransaction();
    //        return transaction;
    //    }
    //    static MySqlCommand CreateCommand(string query, params MySqlParameter[] prmArray)
    //    {
    //        MySqlConnection mysqlConnection = new MySqlConnection(connectionString);

    //        MySqlCommand mysqlCommand = new MySqlCommand(query, mysqlConnection);

    //        if (!query.Contains(" "))
    //            mysqlCommand.CommandType = CommandType.StoredProcedure;

    //        if (prmArray.Length > 0)
    //            mysqlCommand.Parameters.AddRange(prmArray);

    //        mysqlConnection.Open();

    //        return mysqlCommand;
    //    }

    //    public static int ExecuteNonQuery(string query, params MySqlParameter[] prmArray)
    //    {
    //        MySqlCommand mysqlCommand = CreateCommand(query, prmArray);

    //        int affectedRows = mysqlCommand.ExecuteNonQuery();

    //        mysqlCommand.Connection.Close();

    //        return affectedRows;
    //    }

    //    public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
    //    {
    //        using (MySqlConnection connection = new MySqlConnection(connectionString))
    //        {
    //            using (MySqlCommand command = new MySqlCommand(query, connection))
    //            {
    //                if (parameters != null)
    //                    command.Parameters.AddRange(parameters);

    //                connection.Open();
    //                var result = command.ExecuteScalar();
    //                return result == DBNull.Value ? null : result;
    //            }
    //        }
    //    }


    //    public static MySqlDataReader ExecuteReader(string query, params MySqlParameter[] prmArray)
    //    {
    //        MySqlCommand mysqlCommand = CreateCommand(query, prmArray);

    //        MySqlDataReader mysqlDataReader = mysqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
    //        return mysqlDataReader;
    //    }

    //    public static DataTable ExecuteDataTable(string query, params MySqlParameter[] prmArray)
    //    {
    //        using (MySqlConnection connection = new MySqlConnection(connectionString))
    //        {
    //            connection.Open();
    //            using (MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(query, connection))
    //            {
    //                if (!query.Contains(" "))
    //                    mysqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

    //                if (prmArray.Length > 0)
    //                    mysqlDataAdapter.SelectCommand.Parameters.AddRange(prmArray);

    //                DataTable dt = new DataTable();
    //                mysqlDataAdapter.Fill(dt);

    //                return dt;
    //            }
    //        }
    //    }

    //    public static DataSet ExecuteDataSet(string query, params MySqlParameter[] prmArray)
    //    {
    //        using (MySqlDataAdapter mysqlDataAdapter = new MySqlDataAdapter(query, connectionString))
    //        {
    //            if (!query.Contains(" "))
    //                mysqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

    //            if (prmArray.Length > 0)
    //                mysqlDataAdapter.SelectCommand.Parameters.AddRange(prmArray);

    //            DataSet ds = new DataSet();
    //            mysqlDataAdapter.Fill(ds);

    //            return ds;
    //        }
    //    }
    //}


}
