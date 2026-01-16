using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewLevel1 : Form
    {
        private int id;
        public frmViewLevel1(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            int code = 1;
            if (txtLevel1.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Level 1 Name First");
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_1 where name = @name",
                DBClass.CreateParameter("name", txtLevel1.Text)))
                if (reader.Read())
                {
                    if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                    {
                        MessageBox.Show("Name Already Exists. Enter Another Name.");
                        return;
                    }
                }


            if (id == 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select max(code) as code from tbl_coa_level_1"))
                    if (reader.Read() && reader["code"].ToString() != "")
                        code = int.Parse(reader["code"].ToString()) + 1;

                DBClass.ExecuteNonQuery("insert into tbl_coa_level_1(`name`, `code`,`category_code`) values (@name,@code,@categoryCode)",
                    DBClass.CreateParameter("name", txtLevel1.Text),
                    DBClass.CreateParameter("code", code),
                    DBClass.CreateParameter("categoryCode", comboCategoryCode.Text));
                Utilities.LogAudit(frmLogin.userId, "Add Level 1 Account", "Chart Of Account", code, "Added Level 1 Account: " + txtLevel1.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery("update tbl_coa_level_1 set name=@name, category_code=@categoryCode where id = @id",
                    DBClass.CreateParameter("id", id),
                    DBClass.CreateParameter("name", txtLevel1.Text),
                    DBClass.CreateParameter("categoryCode", comboCategoryCode.Text)
                );
                Utilities.LogAudit(frmLogin.userId, "Edit Level 1 Account", "Chart Of Account", id, "Edited Level 1 Account: " + txtLevel1.Text);
            }
            EventHub.Refreshlvl1Account();
            this.Close();
        }

        private void frmViewLevel1_Load(object sender, EventArgs e)
        {
            comboCategoryCode.Items.AddRange(AccountCategory.Codes);
            comboCategoryCode.SelectedIndex = 0; // Default to the first item

            if (id != 0)
            {
                btnSave.Enabled = UserPermissions.canEdit("Chart Of Account");
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_1 where id = @id",
                    DBClass.CreateParameter("id", id)))
                    if (reader.Read())
                    {
                        txtLevel1.Text = reader["name"].ToString();
                        comboCategoryCode.Text = reader["category_code"].ToString();
                    }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button4_Click(object sender, EventArgs e)
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

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
