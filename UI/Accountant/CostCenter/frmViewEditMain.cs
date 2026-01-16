using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewEditMain : Form
    {

        private int id;
        public frmViewEditMain(int id = 0)
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
            if (cmbMain.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Main Name First");
                return;
            }
            using (MySqlDataReader readerA = DBClass.ExecuteReader("select * from tbl_cost_center where name = @name",
                DBClass.CreateParameter("name", cmbMain.SelectedValue)))
                if (readerA.Read())
                {
                    if (id == 0 || (id != int.Parse(readerA["id"].ToString())))
                    {
                        MessageBox.Show("Name Already Exists. Enter Another Name.");
                        return;
                    }
                }

            MySqlDataReader reader;
            if (id == 0)
            {
                reader = DBClass.ExecuteReader("select max(code) as code from tbl_cost_center");
                if (reader.Read() && reader["code"].ToString() != "")
                    code = int.Parse(reader["code"].ToString()) + 1;
                reader.Dispose();
                DBClass.ExecuteNonQuery("insert into tbl_cost_center (`name`, `code`) values (@name,@code)",
                    DBClass.CreateParameter("name", cmbMain.SelectedValue),
                    DBClass.CreateParameter("code", code));

                Utilities.LogAudit(frmLogin.userId, "Add Main Cost Center", "Cost Center", code, "Added Main Cost Center: " + cmbMain.SelectedValue);
            }
            else
            {
                DBClass.ExecuteNonQuery("update tbl_cost_center set name=@name where id = @id",
                    DBClass.CreateParameter("id", id),
              DBClass.CreateParameter("name", cmbMain.SelectedValue));

                Utilities.LogAudit(frmLogin.userId, "Edit Main Cost Center", "Cost Center", id, "Edited Main Cost Center: " + cmbMain.SelectedValue);
            }
            EventHub.RefreshMainCostCenter();

            this.Close();
        }

        private void frmViewEditMain_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                btnSave.Enabled = UserPermissions.canEdit("Cost Center");
                btnDelete.Enabled = UserPermissions.canDelete("Cost Center");
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_cost_center ORDER BY name"))
                {
                    while (reader.Read())
                    {
                        cmbMain.Items.Add(reader["name"].ToString());
                    }
                }
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbMain.Text == "")
            {
                MessageBox.Show("Select Main Account First.");
                return;
            }
            object result = DBClass.ExecuteScalar("SELECT id FROM tbl_cost_center WHERE name = @name",
                DBClass.CreateParameter("name", cmbMain.Text));

            if (result == null)
            {
                MessageBox.Show("Invalid selection.");
                return;
            }
            int selectedId = Convert.ToInt32(result);

            // Check usage
            object count = DBClass.ExecuteScalar("SELECT COUNT(*) FROM tbl_sub_cost_center WHERE main_id = @id",
                DBClass.CreateParameter("id", selectedId));

            if (Convert.ToInt32(count) > 0)
            {
                MessageBox.Show($"{cmbMain.Text} is used. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DBClass.ExecuteNonQuery("DELETE FROM tbl_cost_center WHERE id = @id",
                DBClass.CreateParameter("id", selectedId));

            MessageBox.Show($"{cmbMain.Text} deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            EventHub.Refreshlvl4Account();
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
    }
}
