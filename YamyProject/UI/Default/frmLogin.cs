using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS;

namespace YamyProject
{
    public partial class frmLogin : Form
    {
        public static int userId, RoleId;
        public static string userFName, userLName,userName;
        public frmLogin()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        public static MainForm frmMain;
        //public static Dictionary<string, int> defaultAccounts = BindCombos.LoadDefaultAccounts();
        public static Dictionary<string, int> defaultAccounts;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            if (username == SecurityConfig.DeveloperLogUsername && password == SecurityConfig.DeveloperLogPassword)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@" SELECT * FROM tbl_sec_users where Role_Id =1 and active=0 Limit 1"))
                {
                    if (reader.Read())
                    {
                        userId = int.Parse(reader["id"].ToString());
                        RoleId = int.Parse(reader["role_id"].ToString());
                        userFName = reader["first_name"].ToString();
                        userLName = reader["Last_name"].ToString();
                        userName = reader["User_Name"].ToString();

                        loadUserPermission(userId);
                        this.Hide();
                        frmMain = new MainForm();
                        frmMain.Show();
                    }
                }
            }
            else
            { 
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                                SELECT id, role_id, first_name, last_name, user_name, passwordhash, salt, active FROM tbl_sec_users WHERE User_Name = @UserName",
                                DBClass.CreateParameter("@UserName", username)))
                if (reader.Read())
                {
                    if (reader["active"].ToString() != "0")
                    {
                        lblError.Text = " User Is Not Active.";
                        lblError.Visible = true;
                        return;
                    }
                    if (PasswordHelper.VerifyPassword(txtPassword.Text, reader["PasswordHash"].ToString(), reader["salt"].ToString()))
                    {

                        userId = int.Parse(reader["id"].ToString());
                        RoleId = int.Parse(reader["role_id"].ToString());
                        userFName = reader["first_name"].ToString();
                        userLName = reader["Last_name"].ToString();
                        userName = reader["User_Name"].ToString();

                        loadUserPermission(userId);
                        this.Hide();
                        frmMain = new MainForm();
                        frmMain.Show();
                    }
                    else
                    {
                        lblError.Text = "Invalid Username or Password.";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "Invalid Username.";
                    lblError.Visible = true;
                }
            }
        }
        private void loadUserPermission(int _userId)
        {
            object netResult = DBClass.ExecuteScalar(
                "SELECT count(*) FROM tbl_user_permissions WHERE user_id = @user_id",
                DBClass.CreateParameter("user_id", _userId)
            );

            int count = netResult != DBNull.Value ? Convert.ToInt32(netResult) : 0;

            if (count > 0)
            {
                DataTable userTable = DBClass.ExecuteDataTable(
                    "SELECT p.id, p.user_id, s.id AS sub_menu_id, s.name AS sub_menu_name, m.id AS main_menu_id, m.name AS main_menu_name, p.can_view, p.can_edit, p.can_delete FROM tbl_user_permissions p JOIN tbl_sub_menus s ON p.sub_menu_id = s.id JOIN tbl_main_menus m ON m.id = s.m_id WHERE p.user_id = @user_id",
                    DBClass.CreateParameter("user_id", _userId)
                );
                
                UserPermissions.LoadPermissions(userTable);
            }
            else
            {
                UserPermissions.ViewPermissions.Clear();
                UserPermissions.EditPermissions.Clear();
                UserPermissions.DeletePermissions.Clear();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (defaultAccounts == null)
                defaultAccounts = BindCombos.LoadDefaultAccounts();
            //Console.WriteLine("Loading data...");
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            // Run LoadAllData() in the background
            //await Task.Run(() =>
            //{
            //    BindDataTable.LoadAllData();
            //});
            //sw.Stop();
            //Console.WriteLine("Data loaded successfully. Time taken: " + sw.Elapsed.TotalSeconds + " seconds.");
        }

        private void lblLinkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtUsername.Text))
                new frmChangePassword(txtUsername.Text).ShowDialog();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to exit the application?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel; // Or Abort
                Application.Exit();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnLogin.PerformClick();
        }

    }
}
