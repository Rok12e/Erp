using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewVendorCategory : Form
    {
        private int id;
        public frmViewVendorCategory( int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            this.Text = id != 0 ? "Vendors - Edit Vendors Category" : "Vendors - New Vendors Category";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNewCustomer_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor_category where id =@id", DBClass.CreateParameter("id", id)))
                {
                    reader.Read();
                    txtName.Text = reader["name"].ToString();
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
            using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_vendor_category where name = @name",
                DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    int existingId = Convert.ToInt32(reader["id"]);
                    if (id == 0 || id != existingId)
                    {
                        MessageBox.Show("Category already exists. Please enter another name.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            if (id == 0)
            {
                int refId = DBClass.ExecuteNonQuery("insert into tbl_vendor_category (name) values(@name)",
                    DBClass.CreateParameter("name", txtName.Text));
                Utilities.LogAudit(frmLogin.userId, "Add Vendor Category", "Vendor Category", refId, "Added Vendor Category: " + txtName.Text);
            }
            else
                DBClass.ExecuteNonQuery("update tbl_vendor_category set name=@name where id = @id",
                    DBClass.CreateParameter("id", id),
              DBClass.CreateParameter("name", txtName.Text));
            Utilities.LogAudit(frmLogin.userId, (id == 0 ? "Add Vendor Category" : "Update Vendor Category"), "Vendor Category", id, (id == 0 ? "Added Vendor Category: " : "Updated Vendor Category: ") + txtName.Text);
            MessageBox.Show("Category saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            EventHub.RefreshVendorCategory();
            this.Close();
        }
    }
}
