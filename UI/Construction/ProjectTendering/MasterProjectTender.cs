using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterProjectTender : Form
    {
        private EventHandler projectUpdatedHandler;

        public MasterProjectTender()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            projectUpdatedHandler  = (sender, args) => ProjectSiteData();
            EventHub.Project += projectUpdatedHandler;
            this.Text = "Tender Center";
            headerUC1.FormText = this.Text;
        }
        private void MasterProjectTender_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Project -= projectUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmAddTenderName(null, 0).ShowDialog();
        }
        private void MasterProjectTender_Load(object sender, EventArgs e)
        {
            ProjectSiteData();
        }
        public void ProjectSiteData()
        {
            DataTable dT = DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY id) AS SN, id, code, name FROM tbl_tender_names;");
            dgvCustomer.DataSource = dT;
            dgvCustomer.Columns["id"].Visible = false;
            dgvCustomer.Columns["SN"].Width = 50;
            dgvCustomer.Columns["code"].Width = 100;
            dgvCustomer.Columns["Name"].MinimumWidth = 200;
            dgvCustomer.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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

            object result = DBClass.ExecuteScalar(@"SELECT COUNT(1) FROM tbl_project_tender 
                  WHERE tender_name_id = @id", DBClass.CreateParameter("id", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            int recordCount = 0;
            if (result != null && result != DBNull.Value)
                recordCount = Convert.ToInt32(result);
            if (recordCount > 0)
            {
                MessageBox.Show("Already used");
                return;
            }
            DBClass.ExecuteNonQuery("DELETE FROM tbl_tender_names WHERE id=@id", DBClass.CreateParameter("id", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            MessageBox.Show("Deleted");
            ProjectSiteData();
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            new frmAddTenderName(null, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())).ShowDialog();
        }
    }
}
