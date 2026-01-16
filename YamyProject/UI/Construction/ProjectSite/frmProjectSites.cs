using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmProjectSites : Form
    {
      
        private int id;
        frmViewProjectPlanning master;

        public frmProjectSites(frmViewProjectPlanning _master, int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            this.master = _master;
            if (id == 0)
                this.Text = id == 0 ? "New Site" : "Edit Site";
            headerUC1.FormText = this.Text;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtSiteName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Name First");
                return;
            }

            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_project_sites where name = @name",
            DBClass.CreateParameter("name", txtSiteName.Text));
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
                object result = DBClass.ExecuteScalar("SELECT LPAD(MAX(CAST(code AS UNSIGNED)), 3, '0') FROM tbl_project_sites;");
                int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                string formattedId = nextId.ToString().PadLeft(3, '0');
                txtSiteCode.Text = formattedId;
                int refId = DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_sites (code, name,location_id,plot_number,address) VALUES (@code, @name,@locationId,@plotNumber,@address);",
                            DBClass.CreateParameter("code", formattedId),
                            DBClass.CreateParameter("name", txtSiteName.Text),
                            DBClass.CreateParameter("locationId", cmbProjectLocation.SelectedValue.ToString()),
                            DBClass.CreateParameter("plotNumber", txtPlotNumber.Text),
                            DBClass.CreateParameter("address", txtAddress.Text));
                Utilities.LogAudit(frmLogin.userId, "Add Project Site", "Project Site", refId, "Added Project Site: " + txtSiteName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_project_sites SET name = @name, location_id = @locationId, plot_number = @plotNumber, address = @address WHERE id = @id;",
                            DBClass.CreateParameter("name", txtSiteName.Text),
                            DBClass.CreateParameter("locationId", cmbProjectLocation.SelectedValue),
                            DBClass.CreateParameter("plotNumber", txtPlotNumber.Text),
                            DBClass.CreateParameter("address", txtAddress.Text),
                        DBClass.CreateParameter("id", id));
                Utilities.LogAudit(frmLogin.userId, "Update Project Site", "Project Site", id, "Updated Project Site: " + txtSiteName.Text);
            }
            if (master != null)
            {
                master.BindCombo();
            }
            else
            {
                EventHub.RefreshProject();
            }
            this.Close();
        }

        private void frmProjectSites_Load(object sender, EventArgs e)
        {
            BindCombo();

            if (id != 0)
            {
                MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_project_sites where id = @id",
                      DBClass.CreateParameter("id", id));
                if (reader.Read())
                {
                    txtSiteName.Text = reader["name"].ToString();
                    txtSiteCode.Text = reader["code"].ToString();
                    cmbProjectLocation.SelectedValue = reader["location_id"].ToString();
                    txtPlotNumber.Text = reader["plot_number"].ToString();
                    txtAddress.Text = reader["address"].ToString();
                }
            }

        }
        public void BindCombo()
        {
            BindCombos.PopulateCityAllNormalComboBox(cmbProjectLocation);
        }

        private void btnAddLocation_Click(object sender, EventArgs e)
        {
            new frmViewCity().Show();
        }
    }
}
