using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewLevel2 : Form
    {
        public int l1Id,id;
        public frmViewLevel2(int _id = 0, int _l1Id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            this.l1Id = _l1Id;
            headerUC1.FormText = this.Text;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewLevel2_Load(object sender, EventArgs e)
        {
            BindCombos.Populatelevel1Account(cmbLevel1);
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_2 where id =@id", DBClass.CreateParameter("id", id)))
                    if (reader.Read())
                    {
                        cmbLevel1.SelectedValue = int.Parse(reader["main_id"].ToString());
                        cmbLevel1.Enabled = false;
                        txtLevel2.Text = reader["name"].ToString();
                    }
            }
            else
            {
                if (l1Id > 0)
                {
                    cmbLevel1.SelectedValue = l1Id;
                }
            }
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLevel2.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Level 2 Name First");
                return;
            }
            if (cmbLevel1.Text.Trim() == "")
            {
                MessageBox.Show("Please Chose Level1 First.");
                return;
            }

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_2 where name = @name",
                DBClass.CreateParameter("name", txtLevel2.Text)))
                if (reader.Read())
                {
                    if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                    {
                        MessageBox.Show("Name Already Exists. Enter Another Name.");
                        return;
                    }
                }

            if (id != 0)
            {
                DBClass.ExecuteNonQuery("update tbl_coa_level_2 set name = @name where id =@id",
                    DBClass.CreateParameter("name", txtLevel2.Text),
                    DBClass.CreateParameter("id", id));
                Utilities.LogAudit(frmLogin.userId, "Update Level 2 Account", "Level 2 Account", id, "Updated Level 2 Account: " + txtLevel2.Text);
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select code from tbl_coa_level_1 where id = @id",
                    DBClass.CreateParameter("id", cmbLevel1.SelectedValue.ToString())))
                {
                    reader.Read();
                    using (MySqlDataReader r = DBClass.ExecuteReader("select max(code) as code from tbl_coa_level_2 where code like '" + reader["code"].ToString() + "%'"))
                    {
                        if (r.Read() && r["code"].ToString() != "")
                        {
                            int refID = DBClass.ExecuteNonQuery(
                              "INSERT INTO tbl_coa_level_2 (name, code, main_id) VALUES (@name, @code, @main_id)",
                              DBClass.CreateParameter("name", txtLevel2.Text),
                              DBClass.CreateParameter("code", int.Parse(r["code"].ToString()) + 1),
                              DBClass.CreateParameter("main_id", cmbLevel1.SelectedValue.ToString()));
                            Utilities.LogAudit(frmLogin.userId, "Add Level 2 Account", "Level 2 Account", refID, "Added Level 2 Account: " + txtLevel2.Text);
                        }
                        else
                        {
                            int refId = DBClass.ExecuteNonQuery("INSERT INTO tbl_coa_level_2 (name, code, main_id) VALUES (@name, @code, @main_id)", 
                                DBClass.CreateParameter("name", txtLevel2.Text),
                                DBClass.CreateParameter("code", reader["code"].ToString() + "1"),
                                DBClass.CreateParameter("main_id", cmbLevel1.SelectedValue.ToString()));
                            Utilities.LogAudit(frmLogin.userId, "Add Level 2 Account", "Level 2 Account", refId, "Added Level 2 Account: " + txtLevel2.Text);
                        }
                    }
                }
            }
            EventHub.Refreshlvl2Account();
            this.Close();
        }
    }
}
