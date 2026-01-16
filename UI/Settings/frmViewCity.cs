using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewCity : Form
    {
        private int id;

        public frmViewCity(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            this.Text = id != 0 ? "City - Edit" : "City - New";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewCity_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCountries(cmbCountry);

            if (id != 0)
            {
                try
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_city WHERE id = @id",
                        DBClass.CreateParameter("id", id)))
                    {
                        if (reader.Read())
                        {
                            cmbCity.SelectedValue = reader["id"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading City: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCountry.SelectedValue != null)
                PopulateCities((int)cmbCountry.SelectedValue);
        }
        private void PopulateCities(int countryId)
        {
            DataTable dt = DBClass.ExecuteDataTable("select * from tbl_city where country_id=@id",
                DBClass.CreateParameter("id", countryId));
            cmbCity.ValueMember = "id";
            cmbCity.DisplayMember = "name";
            cmbCity.DataSource = dt;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string cityName = cmbCity.Text.Trim();

            if (string.IsNullOrEmpty(cityName))
            {
                MessageBox.Show("Please enter a City name first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id FROM tbl_city WHERE name = @name",
                    DBClass.CreateParameter("name", cityName)))
                {
                    if (reader.Read())
                    {
                        int existingId = Convert.ToInt32(reader["id"]);
                        if (id == 0 || id != existingId)
                        {
                            MessageBox.Show("City already exists. Please enter another name.", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }

                if (id == 0)
                {
                    DBClass.ExecuteNonQuery("INSERT INTO tbl_city (name,country_id) VALUES (@name,@cId)",
                        DBClass.CreateParameter("name", cityName),
                        DBClass.CreateParameter("cId", cmbCountry.SelectedValue.ToString()));
                    Utilities.LogAudit(frmLogin.userId, "Add City", "City", 0, "Added City: " + cityName);
                }
                else
                {
                    DBClass.ExecuteNonQuery("UPDATE tbl_city SET name = @name,country_id = @cId WHERE id = @id",
                        DBClass.CreateParameter("id", id),
                        DBClass.CreateParameter("name", cityName),
                        DBClass.CreateParameter("cId", cmbCountry.SelectedValue.ToString()));
                    Utilities.LogAudit(frmLogin.userId, "Update City", "City", id, "Updated City: " + cityName);
                }

                MessageBox.Show("City saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EventHub.RefreshRoles();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving City: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
