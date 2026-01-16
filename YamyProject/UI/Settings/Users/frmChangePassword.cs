using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmChangePassword : Form
    {
        string userName;
        public frmChangePassword(string _userName = "")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.userName = _userName;
            headerUC1.FormText = string.IsNullOrEmpty(_userName) ? "Users - Change Password" : "User - Forgot Password";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string salt;
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtOld.Visible && txtOld.Text.Trim() == "")
            {
                MessageBox.Show("Enter Old Password ");
                txtOld.Focus();
                return;
            }
            if (txtNew.Text.Trim() == "")
            {
                MessageBox.Show("Enter Your New Password First");
                txtNew.Focus();
                return;
            }
            if (txtConfirm.Text.Trim() == "")
            {
                MessageBox.Show("Enter Confirm Password");
                txtConfirm.Focus();
                return;
            }
            if (txtNew.Text != txtConfirm.Text)
            {
                MessageBox.Show("Confirm Password Mismatch With New Password, Please Check First.");
                txtNew.Focus();
                return;
            }
            int uId = 0;
            if (string.IsNullOrEmpty(userName))
            {
                uId = frmLogin.userId;
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                                SELECT * FROM tbl_sec_users WHERE User_Name = @UserName",
                                DBClass.CreateParameter("@UserName", userName)))
                    if (reader.Read())
                    {
                        uId = int.Parse(reader["id"].ToString());
                    }
            }
            string storedHash = "";
            string storedSalt = "";
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_sec_users WHERE id = @id", DBClass.CreateParameter("@id", uId)))
                if (reader.Read())
                {
                    storedHash = reader["PasswordHash"].ToString();
                    storedSalt = reader["salt"].ToString();

                    if (txtOld.Visible && !PasswordHelper.VerifyPassword(txtOld.Text, storedHash, storedSalt))
                    {
                        MessageBox.Show("Old password is incorrect");
                        return;
                    }

                    string newSalt;
                    string newHash = PasswordHelper.HashPassword(txtNew.Text, out newSalt);

                    DBClass.ExecuteNonQuery(@"UPDATE tbl_sec_users SET PasswordHash = @hash, Salt = @salt, 
                             password_updated_by = @adminId,
                             password_last_update = @date  WHERE Id = @id",
                    DBClass.CreateParameter("@hash", newHash),
                    DBClass.CreateParameter("@salt", newSalt),
                    DBClass.CreateParameter("@adminId", uId),
                    DBClass.CreateParameter("@date", DateTime.Now.Date),
                    DBClass.CreateParameter("@id", uId));

                    MessageBox.Show("Password changed successfully");

                    EventHub.RefreshUser();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("User not found.");
                }
        }

        private void btnHideOld_MouseDown(object sender, MouseEventArgs e)
        {
            txtOld.PasswordChar = '\0';
        }

        private void btnHideOld_MouseUp(object sender, MouseEventArgs e)
        {
            txtOld.PasswordChar = '*';
        }

        private void btnHideNew_MouseDown(object sender, MouseEventArgs e)
        {
            txtNew.PasswordChar = '\0';
        }

        private void btnHideNew_MouseUp(object sender, MouseEventArgs e)
        {
            txtNew.PasswordChar = '*';
        }

        private void btnHideConfirm_MouseDown(object sender, MouseEventArgs e)
        {
            txtConfirm.PasswordChar = '\0';
        }

        private void btnHideConfirm_MouseUp(object sender, MouseEventArgs e)
        {
            txtConfirm.PasswordChar = '*';
        }
        
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userName))
            {
                txtUserName.Text = frmLogin.userName;
            }
            else
            {
                txtUserName.Text = userName;
                btnHideOld.Hide();
                txtOld.Hide();
                label1.Hide();
            }
        }
    }
}
