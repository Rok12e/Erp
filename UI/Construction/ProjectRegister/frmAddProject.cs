using MySql.Data.MySqlClient;
using System;
using System.Security.Principal;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAddProject : Form
    {
        private int id;
        private Form parentForm;

        public frmAddProject(Form parent, int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            parentForm = parent;
            if (id == 0)
                this.Text = "Project";
            headerUC1.FormText = this.Text;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Name First");
                return;
            }

            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_projects where name = @name",
            DBClass.CreateParameter("name", txtName.Text));
            if (reader.Read())
            {
                if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                {
                    MessageBox.Show(" Name Already In Use. Enter Another Name.");
                    return;
                }
            }


            if (id == 0)
            {
                int projectId = Convert.ToInt32(DBClass.ExecuteScalar(@"
                                                    INSERT INTO tbl_projects (code, name, category, description, start_date, end_date, country_id, city_id) 
                                                    VALUES (@code, @name, @category, @description, @start_date, @end_date, @country_id, @city_id);
                                                    SELECT LAST_INSERT_ID();",
                                                    DBClass.CreateParameter("@code", txtCode.Text),
                                                    DBClass.CreateParameter("@name", txtName.Text),
                                                    DBClass.CreateParameter("@category", cmbCategory.Text),
                                                    DBClass.CreateParameter("@description", txtDescription.Text),
                                                    DBClass.CreateParameter("@start_date", dtStart.Value.Date),
                                                    DBClass.CreateParameter("@end_date", dtEnd.Value.Date),
                                                    DBClass.CreateParameter("@country_id", cmbCountry.SelectedValue),
                                                    DBClass.CreateParameter("@city_id", cmbCity.SelectedValue)
                                                ));

                if (IsProjectOption)
                {
                    var mainId = DBClass.ExecuteScalar(@"INSERT INTO tbl_cost_center (name, code, project_id)
                                  VALUES (@name, @code, @project_id);SELECT LAST_INSERT_ID();",
                            DBClass.CreateParameter("name", txtName.Text),
                            DBClass.CreateParameter("code", txtCode.Text),
                            DBClass.CreateParameter("project_id", projectId));

                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_sub_cost_center (code, name, main_id, project_id)
                                  VALUES (@code, @name, @main_id, @project_id);",
                            DBClass.CreateParameter("code", txtCode.Text),
                            DBClass.CreateParameter("name", txtName.Text),
                            DBClass.CreateParameter("project_id", projectId),
                            DBClass.CreateParameter("main_id", mainId.ToString()));
                }

                Utilities.LogAudit(frmLogin.userId, "Add Project", "Project", projectId,
                    "Added New Project: " + txtName.Text + " (Code: " + txtCode.Text + ")");
            }
            else
            {
                DBClass.ExecuteNonQuery(@"
                    UPDATE tbl_projects
                    SET code = @code, name = @name, category = @category, description = @description, 
                        start_date = @start_date, end_date = @end_date, country_id  = @country_id, city_id = @city_id
                    WHERE id = @id;",
                    DBClass.CreateParameter("@code", txtCode.Text),
                    DBClass.CreateParameter("@name", txtName.Text),
                    DBClass.CreateParameter("@category", cmbCategory.Text),
                    DBClass.CreateParameter("@description", txtDescription.Text),
                    DBClass.CreateParameter("@start_date", dtStart.Value.Date),
                    DBClass.CreateParameter("@end_date", dtEnd.Value.Date),
                    DBClass.CreateParameter("@country_id", cmbCountry.SelectedValue ?? 0),
                    DBClass.CreateParameter("@city_id", cmbCity.SelectedValue ?? 0),
                    DBClass.CreateParameter("@id", id)
                );


                Utilities.LogAudit(frmLogin.userId, "Update Project", "Project", id,
                    "Updated Project: " + txtName.Text);

                if (IsProjectOption)
                {
                    int cId = 0;
                    using (MySqlDataReader reader0 = DBClass.ExecuteReader("select * from tbl_sub_cost_center where name = @name",
                        DBClass.CreateParameter("name", txtName.Text)))
                        if (reader0.Read())
                        {
                            cId = int.Parse(reader0["id"].ToString());

                            DBClass.ExecuteNonQuery("update tbl_sub_cost_center set name=@name where id = @id",
                                DBClass.CreateParameter("id", cId),
                            DBClass.CreateParameter("name", txtName.Text));
                        }
                }

            }

            if (parentForm is frmViewProjectManagement)
            {
                frmViewProjectManagement frm = parentForm as frmViewProjectManagement;
                frm.BindCombo();
            }
            else if (parentForm is frmViewProjectEstimating)
            {
                frmViewProjectEstimating frm = parentForm as frmViewProjectEstimating;
                frm.BindCombo();
            }
            else if (parentForm is frmViewProjectTendering)
            {
                frmViewProjectTendering frm = parentForm as frmViewProjectTendering;
                frm.BindCombo();
            }
            else
            {
                EventHub.RefreshProject();
            }

            this.Close();
        }

        bool IsProjectOption = false;

        private void frmAddProject_Load(object sender, EventArgs e)
        {
            var generalS = Utilities.GeneralSettingsState("PROJECT OPTION");
            if (!string.IsNullOrEmpty(generalS) & int.Parse(generalS) > 0)
            {
                IsProjectOption = true;
            }
            else
            {
                IsProjectOption = false;
            }
            BindCombos.PopulateCountries(cmbCountry);
            loadCity();
            loadCategory();

            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_projects where id = @id",
                      DBClass.CreateParameter("id", id)))
                    if (reader.Read())
                    {
                        txtName.Text = reader["name"].ToString();
                        txtCode.Text = reader["code"].ToString();
                        cmbCategory.Text = reader["category"].ToString();
                        txtDescription.Text = reader["description"].ToString();
                        dtStart.Value = reader["start_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["start_date"]);
                        dtEnd.Value = reader["end_date"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["end_date"]);
                        cmbCountry.SelectedValue = reader["country_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["country_id"]);
                        if (cmbCountry.SelectedValue != null)
                        {
                            loadCity();
                            cmbCity.SelectedValue = reader["city_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["city_id"]);
                        }
                    }
            }
            else
            {
                object result = DBClass.ExecuteScalar("SELECT LPAD(MAX(CAST(code AS UNSIGNED)), 4, '0') FROM tbl_projects;");
                int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                string formattedId = nextId.ToString().PadLeft(3, '0');
                txtCode.Text = formattedId;
            }
        }

        private void loadCategory()
        {
            cmbCategory.DataSource = cmbCategory.DataSource = DBClass.ExecuteDataTable("SELECT DISTINCT TRIM(category) AS cat FROM tbl_projects ORDER BY cat");
            cmbCategory.DisplayMember = "cat";
            cmbCategory.ValueMember = "cat";
        }

        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (
                cmbCountry.SelectedValue == null
                )
                return;

            loadCity();
        }

        private void loadCity()
        {
            BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmCostCenter(0).ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewVendor().ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewCustomer().ShowDialog();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewSubcontractor().ShowDialog();
        }
    }
}
