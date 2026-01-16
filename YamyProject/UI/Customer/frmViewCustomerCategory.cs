using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewCustomerCategory : Form
    {
        private int id;

        public frmViewCustomerCategory(int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            this.Text = id != 0 ? "Customers - Edit Customer Category" : "Customers - New Customer Category";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewCustomerCategory_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                try
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_customer_category WHERE id = @id",
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
                    MessageBox.Show("Error loading category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string categoryName = txtName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please enter a category name first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id FROM tbl_customer_category WHERE name = @name",
                    DBClass.CreateParameter("name", categoryName)))
                {
                    if (reader.Read())
                    {
                        int existingId = Convert.ToInt32(reader["id"]);
                        if (id == 0 || id != existingId)
                        {
                            MessageBox.Show("Category already exists. Please enter another name.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                if (id == 0)
                {
                    int refId = DBClass.ExecuteNonQuery("INSERT INTO tbl_customer_category (name) VALUES (@name)",
                        DBClass.CreateParameter("name", categoryName));
                    Utilities.LogAudit(frmLogin.userId, "Add Customer Category", "Customer Category", refId, "Added Customer Category: " + categoryName);
                }
                else
                {
                    DBClass.ExecuteNonQuery("UPDATE tbl_customer_category SET name = @name WHERE id = @id",
                        DBClass.CreateParameter("id", id),
                        DBClass.CreateParameter("name", categoryName));

                    Utilities.LogAudit(frmLogin.userId, "Update Customer Category", "Customer Category", id, "Updated Customer Category: " + categoryName);
                }

                MessageBox.Show("Category saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EventHub.RefreshCustomerCategory();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving category: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
