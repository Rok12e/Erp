using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace YamyProject.UI.Default
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
            headerUC1.FormText = this.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServerName.Text.Trim()))
            {
                MessageBox.Show("Enter server / host name");
                return;
            }

            if (string.IsNullOrEmpty(txtCompanyCode.Text.Trim()))
            {
                if (string.IsNullOrEmpty(txtDataBaseName.Text.Trim()))
                {
                    MessageBox.Show("Enter database name");
                    return;
                }
            }
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                MessageBox.Show("Enter user name");
                return;
            }
            //if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            //{
            //    MessageBox.Show("Enter password");
            //    return;
            //}
            string server = txtServerName.Text.Trim();
            string db = txtDataBaseName.Text.Trim();
            string user = txtUserName.Text.Trim();
            string pass = txtPassword.Text.Trim();

            string connStr = $"Server={server};Database={db};Uid={user};";
            if (!string.IsNullOrEmpty(pass))
            {
                connStr += $"Pwd={pass};";
            }
            connStr += "Max Pool Size=1000;charset=utf8mb4;";

            Environment.SetEnvironmentVariable("yamy_connection", CryptoHelper.Encrypt(connStr), EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("yamy_company_code", txtCompanyCode.Text.Trim(), EnvironmentVariableTarget.User);
            MessageBox.Show("Server connection saved. Please restart...", "Saved");
            createCompanyIfNot();
        }

        private void createCompanyIfNot()
        {
            string server = txtServerName.Text.Trim();
            string db = "yamycompany";
            string user = txtUserName.Text.Trim();
            string pass = txtPassword.Text.Trim();

            string serverOnlyConnStr = $"Server={server};Uid={user};Pwd={pass};Max Pool Size=1000;";

            try
            {
                using (var conn = new MySqlConnection(serverOnlyConnStr))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("CREATE DATABASE IF NOT EXISTS yamycompany CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.ChangeDatabase("yamycompany");
                    string createTable = @"CREATE TABLE IF NOT EXISTS `tbl_company` (
                                            `id` int NOT NULL AUTO_INCREMENT,
                                            `database_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `code` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `descriptions` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `phone1` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `phone2` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `gmail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `mobile_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `website` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `address` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `trn_no` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            `logoComp` longblob,
                                            `default_company` int NOT NULL DEFAULT '0',
                                            `customer_code` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                                            PRIMARY KEY(`id`) USING BTREE
                                        ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;";
                    using (var cmd = new MySqlCommand(createTable, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    Application.Restart();
                }
                //using (var conn = new MySqlConnection(serverOnlyConnStr))
                //{
                //    string createDb = @"CREATE DATABASE yamycompany CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
                //                USE yamycompany; CREATE TABLE IF NOT EXISTS `tbl_company` (
                //                    `id` int NOT NULL AUTO_INCREMENT,
                //                    `database_name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `code` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `descriptions` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `phone1` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `phone2` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `gmail` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `mobile_number` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `website` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `address` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `trn_no` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    `logoComp` longblob,
                //                    `default_company` int NOT NULL DEFAULT '0',
                //                    `customer_code` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT '',
                //                    PRIMARY KEY(`id`) USING BTREE
                //                ) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4 COLLATE = utf8mb4_general_ci;
                //            ";

                //    using (var mysqlCommand = new MySqlCommand(createDb, conn))
                //    {
                //        if (mysqlCommand.Connection.State != ConnectionState.Open)
                //        {
                //            mysqlCommand.Connection.Open();
                //        }
                //        mysqlCommand.ExecuteNonQuery();
                //    }

                //    // Optional: Restart application to reinitialize with new database
                //    Application.Restart();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MySQL error:\n{ex.Message}", "MySQL Error");
                Application.Restart();
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

            string server = txtServerName.Text.Trim();
            string user = txtUserName.Text.Trim();
            string pass = txtPassword.Text.Trim();

            string connStr = $"Server={server};Uid={user};";
            if (!string.IsNullOrEmpty(pass))
            {
                connStr += $"Pwd={pass};";
            }
            connStr += "Max Pool Size=1000;";
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    MessageBox.Show("Connection successful!", "Success");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed:\n{ex.Message}", "Error");
            }
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            string con = Environment.GetEnvironmentVariable("yamy_connection", EnvironmentVariableTarget.User);
            string connStr = string.IsNullOrEmpty(con) ? "" : CryptoHelper.Decrypt(con);
            string comCode = Environment.GetEnvironmentVariable("yamy_company_code", EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(comCode))
            {
                txtCompanyCode.Text = comCode;
            }

            // If the connection string exists, set it to the text box
            if (!string.IsNullOrEmpty(connStr))
            {
                var parameters = connStr.Split(';');

                foreach (var parameter in parameters)
                {
                    var keyValue = parameter.Split('=');

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();
                        
                        switch (key.ToLower())
                        {
                            case "server":
                                txtServerName.Text = value;
                                break;
                            case "database":
                                txtDataBaseName.Text = value;
                                break;
                            case "uid":
                                txtUserName.Text = value;
                                break;
                            case "pwd":
                                txtPassword.Text = value;
                                break;
                            case "max pool size":
                                // You might want to add logic for max pool size, or ignore it
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No connection string found. Please set it first.", "Info");
            }
        }
    }
}
