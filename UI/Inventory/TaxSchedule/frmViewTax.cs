using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewItemTaxCodes : Form
    {
        int id = 0;

        public frmViewItemTaxCodes(int _id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = "New Item Tax Codes";
            id = _id;
        }

        private void frmViewItemTaxCodes_Load(object sender, EventArgs e)
        {
            LoadFormData(); // Load values into the fields
        }

        private void LoadFormData()
        {
            if (id > 0)
            {
                DataTable dt = DBClass.ExecuteDataTable("SELECT * FROM tbl_tax WHERE id = @id",
                    DBClass.CreateParameter("id", id));

                if (dt.Rows.Count > 0)
                {
                    var row = dt.Rows[0];
                    txtName.Text = row["name"].ToString();
                    txtValue.Text = row["value"].ToString();
                    txtDescription.Text = row["description"].ToString();
                }
                //btnSave.Enabled = UserPermissions.canEdit("MasterTax");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValue.Text))
            {
                MessageBox.Show("Please enter a value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool success;
            if (id == 0)
                success = insertTax();
            else
                success = updateTax();

            if (success)
            {
                EventHub.RefreshTaxSchedule();
                this.Close();
            }
        }

        private bool insertTax()
        {
            // Check for duplicate value
            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT * FROM tbl_tax WHERE name = @name",
                DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    MessageBox.Show("The value already exists!", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

            DBClass.ExecuteNonQuery(
                "INSERT INTO tbl_tax (name, value, description) VALUES (@name, @value, @description)",
                DBClass.CreateParameter("name", txtName.Text),
                DBClass.CreateParameter("value", txtValue.Text),
                DBClass.CreateParameter("description", txtDescription.Text)
            );

            Utilities.LogAudit(frmLogin.userId, "Insert Tax", "Tax", 0, "Inserted Tax: " + txtName.Text);

            return true;
        }

        private bool updateTax()
        {
            // Check for duplicate value
            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT * FROM tbl_tax WHERE value = @value",
                DBClass.CreateParameter("value", txtValue.Text)))

                if (reader.Read() && id != int.Parse(reader["id"].ToString()))
                {
                    MessageBox.Show("Value already exists. Enter another value.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

            DBClass.ExecuteNonQuery(
                "UPDATE tbl_tax SET name = @name, value = @value, description = @description WHERE id = @id",
                DBClass.CreateParameter("id", id),
                DBClass.CreateParameter("name", txtName.Text),
                DBClass.CreateParameter("value", txtValue.Text),
                DBClass.CreateParameter("description", txtDescription.Text)
            );
            Utilities.LogAudit(frmLogin.userId, "Update Tax", "Tax", id, "Updated Tax: " + txtName.Text);

            return true;
        }

        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}