using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewUser : Form
    {
        private EventHandler employeeUpdatedHandler;
        private EventHandler roleUpdatedHandler;

        int id;
        public frmViewUser(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            employeeUpdatedHandler = (sender, args) => BindCombos.PopulateEmployees(cmbEmployee);
            roleUpdatedHandler = (sender, args) => BindCombos.PopulateUserRoles(cmbRoles);

            EventHub.Employee += employeeUpdatedHandler;
            EventHub.Roles += roleUpdatedHandler;

            headerUC1.FormText = id == 0 ? "Users - New User" : "Users - Update User";
        }
        private void frmViewUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Employee -= employeeUpdatedHandler;
            EventHub.Roles -= roleUpdatedHandler;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string salt;
        private void btnSave_Click(object sender, EventArgs e)
        {
            int employeeId = 0;
            if (cmbEmployee.SelectedValue == null)
            {
                //MessageBox.Show("Choose Employee First");
                //return;
                employeeId = -1;
            }
            if (cmbRoles.SelectedValue == null)
            {
                MessageBox.Show("Choose Role First");
                return;
            }
            if (txtUsername.Text.Trim() == "")
            {
                MessageBox.Show("Enter At least User Name ");
                txtUsername.Focus();
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_sec_users where user_name = @name",
                DBClass.CreateParameter("name", txtUsername.Text)))
                if (reader.Read() && int.Parse(reader["id"].ToString()) != id)
                {
                    MessageBox.Show("User Name Already Exist");
                    return;
                }
            if (id == 0)
            {
                if (txtNew.Text.Trim() == "")
                {
                    MessageBox.Show("Enter Your New Password First");
                    txtNew.Focus();
                    return;
                }
                if (txtNew.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Confirm Password Mismatch With New Password, Please Check First.");
                    txtNew.Focus();
                    return;
                }

                DBClass.ExecuteNonQuery(@"insert into tbl_sec_users (user_name,passwordhash,salt,emp_id,first_name,last_name,role_id,active,state)values 
                                         (@user_name,@passwordhash,@salt,@emp_id,@first_name,@last_name,@role_id,@active,0)",
                  DBClass.CreateParameter("user_name", txtUsername.Text),
                  DBClass.CreateParameter("passwordhash", PasswordHelper.HashPassword(txtNew.Text, out salt)),
                    DBClass.CreateParameter("emp_id", employeeId),
                    DBClass.CreateParameter("first_name", txtFirstName.Text),
                    DBClass.CreateParameter("salt", salt),
                    DBClass.CreateParameter("last_name", txtLastName.Text),
                    DBClass.CreateParameter("role_id", cmbRoles.SelectedValue),
                    DBClass.CreateParameter("active", chkActive.Checked ? 0 : 1));
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_sec_users 
                                set user_name = @user_name, 
                               emp_id = @emp_id, 
                               first_name = @first_name, 
                               last_name = @last_name, 
                               role_id = @role_id, 
                               active = @active
                           WHERE id = @id",
    DBClass.CreateParameter("user_name", txtUsername.Text),
    DBClass.CreateParameter("emp_id", employeeId),
    DBClass.CreateParameter("first_name", txtFirstName.Text),
    DBClass.CreateParameter("last_name", txtLastName.Text),
    DBClass.CreateParameter("role_id", cmbRoles.SelectedValue),
    DBClass.CreateParameter("active", chkActive.Checked ? 0 : 1),
    DBClass.CreateParameter("id", id));

            }
            EventHub.RefreshUser();
            this.Close();
        }
       

        private void frmViewTax_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateEmployees(cmbEmployee);
            BindCombos.PopulateUserRoles(cmbRoles);
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_sec_users where id = @id",
                    DBClass.CreateParameter("id", id)))
                {
                    reader.Read();
                    txtUsername.Text = reader["user_name"].ToString();
                    txtFirstName.Text = reader["first_name"].ToString();
                    txtLastName.Text = reader["last_name"].ToString();
                    cmbEmployee.SelectedValue = reader["emp_id"].ToString();
                    cmbRoles.SelectedValue = reader["role_id"].ToString();
                    pnlPassword.Visible = false;
                    chkActive.Checked = int.Parse(reader["active"].ToString()) == 0;
                }
            }
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

        private void lnkNewRole_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewRole().ShowDialog();
        }

        private void lnkNewEmp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewEmployee());
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
    }
}
