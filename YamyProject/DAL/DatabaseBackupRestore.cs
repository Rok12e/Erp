using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
namespace YamyProject
{
    class DatabaseBackupRestore
    {

        private static string GetMySQLPath()
        {
            string[] possiblePaths = new string[]
            {
                @"C:\Program Files\MySQL\MySQL Server 9.2\bin\mysql.exe",
                @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe",
                @"C:\Program Files\MySQL\MySQL Server 5.7\bin\mysql.exe",
                @"C:\Program Files\MySQL\mysql-9.2.0-winx64\bin\mysql.exe",
                @"C:\Program Files\MySQL\mysql-8.0.33-winx64\bin\mysql.exe"
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            MessageBox.Show("MySQL executable not found! Please set the correct MySQL path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        private static string GetConfiguredMySQLPath()
        {
            string mysqlPath = ConfigurationManager.AppSettings["MySQLPath"];
            if (!File.Exists(mysqlPath))
            {
                MessageBox.Show("Invalid MySQL Path! Please update the configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return mysqlPath;
        }
        private static string GetMySQLDumpPath()
        {
            string[] possiblePaths = new string[]
            {
                @"C:\Program Files\MySQL\MySQL Server 9.2\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\mysql-9.2.0-winx64\bin\mysqldump.exe",
                @"C:\Program Files\MySQL\mysql-8.0.33-winx64\bin\mysqldump.exe"
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            MessageBox.Show("mysqldump.exe not found! Please configure the correct path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
        }
        private static string GetConfiguredMySQLDumpPath()
        {
            string mysqldumpPath = ConfigurationManager.AppSettings["MySQLDumpPath"];
            if (!File.Exists(mysqldumpPath))
            {
                MessageBox.Show("Invalid mysqldump Path! Please update the configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return mysqldumpPath;
        }

        public static bool BackupDatabase(string backupPath)
        {
            try
            {
                string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);
                string connStr = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);

                var builder = new MySqlConnectionStringBuilder(connStr);
                builder.Database = DBClass.Database;
                string fileName = $"{backupPath}\\{builder.Database}_Backup_{DateTime.Now:yyyyMMddHHmmss}.sql";

                using (MySqlConnection conn = new MySqlConnection(builder.ConnectionString))
                using (MySqlCommand cmd = new MySqlCommand())
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = conn;
                    conn.Open();
                    mb.ExportToFile(fileName);   // backup
                    conn.Close();
                }

                if (File.Exists(fileName))
                {
                    MessageBox.Show("Backup Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Backup failed! File not created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Backup Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void RestoreDatabase(string backupFilePath)
        {
            try
            {
                string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);
                string connStr = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);

                if (!File.Exists(backupFilePath))
                {
                    MessageBox.Show("Backup file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var builder = new MySqlConnectionStringBuilder(connStr);
                builder.Database = DBClass.Database;
                using (MySqlConnection conn = new MySqlConnection(builder.ConnectionString))
                using (MySqlCommand cmd = new MySqlCommand())
                using (MySqlBackup mb = new MySqlBackup(cmd))
                {
                    cmd.Connection = conn;
                    conn.Open();

                    // Make sure the restore runs in utf8mb4
                    using (var charsetCmd = new MySqlCommand("SET NAMES utf8mb4;", conn))
                    {
                        charsetCmd.ExecuteNonQuery();
                    }

                    mb.ImportFromFile(backupFilePath);
                    conn.Close();
                }

                MessageBox.Show("Database restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Restore Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //public static bool BackupDatabaseOld(string backupPath)
        //{
        //    try
        //    {
        //        string mysqldumpPath = GetMySQLDumpPath() ?? GetConfiguredMySQLDumpPath();
        //        if (mysqldumpPath == null) return false; // Stop if no valid path found

        //        string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);

        //        string connStr = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);
        //        var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connStr);

        //        string server = builder.Server;
        //        string user = builder.UserID;
        //        string password = builder.Password;
        //        string database = DBClass.Database;

        //        string fileName = $"{backupPath}\\{database}_Backup_{DateTime.Now:yyyyMMddHHmmss}.sql";

        //        // Build mysqldump command
        //        string arguments = $"--host={server} --user={user} ";
        //        if (!string.IsNullOrEmpty(password)) arguments += $"--password={password} ";
        //        arguments += $"{database} --result-file=\"{fileName}\" --routines --events --default-character-set=utf8mb4";

        //        ProcessStartInfo psi = new ProcessStartInfo
        //        {
        //            FileName = mysqldumpPath,
        //            Arguments = arguments,
        //            RedirectStandardError = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true
        //        };

        //        using (Process process = Process.Start(psi))
        //        {
        //            process.WaitForExit();
        //        }

        //        // Verify if the backup file was created
        //        if (File.Exists(fileName))
        //        {
        //            MessageBox.Show("Backup Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return true;
        //        }
        //        else
        //        {
        //            MessageBox.Show("Backup failed! File not created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Backup Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //}

        //public static void RestoreDatabaseOld(string backupFilePath)
        //{
        //    try
        //    {
        //        string mysqlPath = GetMySQLPath() ?? GetConfiguredMySQLPath();
        //        if (mysqlPath == null) return; // Stop if no valid MySQL path found

        //        string cc = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);

        //        string connStr = string.IsNullOrEmpty(cc) ? "" : CryptoHelper.Decrypt(cc);
        //        var builder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connStr);

        //        string server = builder.Server;
        //        string user = builder.UserID;
        //        string password = builder.Password;
        //        string database = DBClass.Database;

        //        if (!File.Exists(backupFilePath))
        //        {
        //            MessageBox.Show("Backup file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        ProcessStartInfo psi = new ProcessStartInfo
        //        {
        //            FileName = mysqlPath,
        //            RedirectStandardInput = true,
        //            RedirectStandardOutput = true,
        //            RedirectStandardError = true,
        //            UseShellExecute = false,
        //            CreateNoWindow = true,
        //            Arguments = $"-h {server} -u {user} {(string.IsNullOrEmpty(password) ? "" : "-p" + password)} {database}"
        //        };

        //        using (Process process = Process.Start(psi))
        //        {
        //            using (StreamReader fileStream = new StreamReader(backupFilePath))
        //            using (StreamWriter inputStream = process.StandardInput)
        //            {
        //                inputStream.Write(fileStream.ReadToEnd());
        //            }

        //            string output = process.StandardOutput.ReadToEnd();
        //            string error = process.StandardError.ReadToEnd();
        //            process.WaitForExit();

        //            if (process.ExitCode == 0)
        //            {
        //                MessageBox.Show("Database restored successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show($"Restore failed! Error: {error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Restore Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
    }
}