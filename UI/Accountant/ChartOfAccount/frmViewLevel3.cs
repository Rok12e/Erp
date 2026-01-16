using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewLevel3 : Form
    {

        public int l1Id, l2Id, id;
        public frmViewLevel3(int _id = 0, int _l1Id = 0, int _l2Id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            this.l1Id = _l1Id;
            this.l2Id = _l2Id;
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewLevel3_Load(object sender, EventArgs e)
        {
            BindCombos.Populatelevel1Account(cmbLevel1);
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_coa_level_1.id AS lvl1Id,tbl_coa_level_2.id AS lvl2Id,
                                                                 tbl_coa_level_3.id,tbl_coa_level_3.name FROM tbl_coa_level_3
                                                                 INNER JOIN tbl_coa_level_2 ON tbl_coa_level_3.main_id = tbl_coa_level_2.id
                                                                 INNER JOIN tbl_coa_level_1 ON tbl_coa_level_2.main_id = tbl_coa_level_1.id where tbl_coa_level_3.id = @id", DBClass.CreateParameter("@id", id)))
                    if (reader.Read())
                    {
                        cmbLevel1.SelectedValue = int.Parse(reader["lvl1Id"].ToString());
                        cmbLevel2.SelectedValue = int.Parse(reader["lvl2Id"].ToString());
                        cmbLevel1.Enabled = cmbLevel2.Enabled = false;
                        txtLevel3.Text = reader["name"].ToString();
                    }
            }
            else
            {
                if (l1Id > 0)
                {
                    cmbLevel1.SelectedValue = l1Id;
                }
                if (l2Id > 0)
                {
                    cmbLevel2.SelectedValue = l2Id;
                }
            }
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLevel3.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Level 3 Name First");
                return;
            }
            if (cmbLevel2.Text.Trim() == "")
            {
                MessageBox.Show("Please Chose Level 2 First.");
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_3 where name = @name",
                DBClass.CreateParameter("name", txtLevel3.Text)))
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
                DBClass.ExecuteNonQuery("update tbl_coa_level_3 set name = @name where id =@id",
                    DBClass.CreateParameter("name", txtLevel3.Text),
                    DBClass.CreateParameter("id", id));
                Utilities.LogAudit(frmLogin.userId, "Update Level 3 Account", "Chart of Account", id, "Updated Level 3 Account: " + txtLevel3.Text);
            }
            else
            {
                using (var reader = DBClass.ExecuteReader("select code from tbl_coa_level_2 where id = @id",
                DBClass.CreateParameter("id", cmbLevel2.SelectedValue.ToString())))
                {
                    reader.Read();
                    MySqlDataReader r = DBClass.ExecuteReader("select max(code) as code from tbl_coa_level_3 where code like '" + reader["code"].ToString() + "%'");
                    if (r.Read() && r["code"].ToString() != "")
                    {
                        int refId = DBClass.ExecuteNonQuery(
                          "INSERT INTO tbl_coa_level_3 (name, code, main_id) VALUES (@name, @code, @main_id)",
                          DBClass.CreateParameter("name", txtLevel3.Text),
                          DBClass.CreateParameter("code", int.Parse(r["code"].ToString()) + 1),
                          DBClass.CreateParameter("main_id", cmbLevel2.SelectedValue.ToString()));
                        Utilities.LogAudit(frmLogin.userId, "Add Level 3 Account", "Chart of Account", refId, "Added Level 3 Account: " + txtLevel3.Text);
                    }
                    else
                    {
                        int refId = DBClass.ExecuteNonQuery("INSERT INTO tbl_coa_level_3 (name, code, main_id) VALUES (@name, @code, @main_id)",
                            DBClass.CreateParameter("name", txtLevel3.Text),
                            DBClass.CreateParameter("code", reader["code"].ToString() + "01"),
                            DBClass.CreateParameter("main_id", cmbLevel2.SelectedValue.ToString()));
                        Utilities.LogAudit(frmLogin.userId, "Add Level 3 Account", "Chart of Account", refId, "Added Level 3 Account: " + txtLevel3.Text);
                    }
                    r.Dispose();
                }
            }
            EventHub.Refreshlvl3Account();
            this.Close();
        }

        private void cmbLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCombos.Populatelevel2Account(cmbLevel2, cmbLevel1.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel1.SelectedValue);

        }
    }
}
