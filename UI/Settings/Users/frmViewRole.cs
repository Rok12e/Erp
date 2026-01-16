using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewRole : Form
    {
        private int id;

        public frmViewRole(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            this.Text = id != 0 ? "Roles - Edit Role" : "Roles - New Role";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewRole_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                try
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_sec_roles WHERE id = @id",
                        DBClass.CreateParameter("id", id)))
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["name"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Role: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string categoryName = txtName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please enter a Role name first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id FROM tbl_sec_roles WHERE name = @name",
                    DBClass.CreateParameter("name", categoryName)))
                {
                    if (reader.Read())
                    {
                        int existingId = Convert.ToInt32(reader["id"]);
                        if (id == 0 || id != existingId)
                        {
                            MessageBox.Show("Role already exists. Please enter another name.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                if (id == 0)
                {
                    DBClass.ExecuteNonQuery("INSERT INTO tbl_sec_roles (name) VALUES (@name)",
                        DBClass.CreateParameter("name", categoryName));
                }
                else
                {
                    DBClass.ExecuteNonQuery("UPDATE tbl_sec_roles SET name = @name WHERE id = @id",
                        DBClass.CreateParameter("id", id),
                        DBClass.CreateParameter("name", categoryName));
                }

                MessageBox.Show("Role saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EventHub.RefreshRoles();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving Role: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
