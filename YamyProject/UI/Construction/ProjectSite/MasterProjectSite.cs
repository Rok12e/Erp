using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterProjectSite : Form
    {
        private EventHandler projectUpdatedHandler;

        public MasterProjectSite()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            projectUpdatedHandler  = (sender, args) => ProjectSiteData();
            EventHub.Project += projectUpdatedHandler;
            this.Text = "Project Site Center";
            headerUC1.FormText = this.Text;
        }
        private void MasterProjectSite_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Project -= projectUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmProjectSites(null, 0).ShowDialog();
        }
        private void MasterProjectSite_Load(object sender, EventArgs e)
        {
            ProjectSiteData();
        }
        public void ProjectSiteData()
        {
            DataTable dT = DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY ps.id) AS SN, ps.id, ps.code, ps.name, COALESCE(c.name, 'Unknown') AS location, ps.plot_number, ps.address FROM tbl_project_sites ps LEFT JOIN tbl_city c ON c.id = ps.location_id;");
            dgvCustomer.DataSource = dT;
            dgvCustomer.Columns["id"].Visible = false;
            dgvCustomer.Columns["SN"].Width = 50;
            dgvCustomer.Columns["code"].Width = 100;
            dgvCustomer.Columns["Name"].MinimumWidth = 200;
            dgvCustomer.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["location"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["plot_number"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["address"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //if (dgvCustomer.Rows.Count > 0)
            //{
            //    btnRemove.Enabled = UserPermissions.canDelete("Project Center");
            //}
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            object result = DBClass.ExecuteScalar(@"SELECT COUNT(1) FROM tbl_project_planning 
                  WHERE site = @id", DBClass.CreateParameter("id", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            int recordCount = 0;
            if (result != null && result != DBNull.Value)
                recordCount = Convert.ToInt32(result);
            if (recordCount > 0)
            {
                MessageBox.Show("Already used");
                return;
            }
            DBClass.ExecuteNonQuery("DELETE FROM tbl_project_sites WHERE id=@id", DBClass.CreateParameter("id", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            MessageBox.Show("Deleted");
            ProjectSiteData();
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            new frmProjectSites(null, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())).ShowDialog();
        }
    }
}
