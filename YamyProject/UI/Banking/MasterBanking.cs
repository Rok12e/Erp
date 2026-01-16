using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterBanking : Form
    {
        private EventHandler bankUpdatedHandler;

        public MasterBanking()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            bankUpdatedHandler  = (sender, args) => BindBanks();
            EventHub.Bank += bankUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterBanking_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Bank -= bankUpdatedHandler;

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmBankRegister().ShowDialog();
        }
        private void MasterBanking_Load(object sender, EventArgs e)
        {
            BindBanks();
        }
        public void BindBanks()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY id) AS SN, id,Code, 
                                                      name AS 'Bank Name', abb_name AS 'Abbreviation Name',ent_id as 'Ent Id', route_num as 'Route NO'
                                                      FROM tbl_bank WHERE state = @state", DBClass.CreateParameter("@state", (CmbState.Text == "Registered" ? "0" : "-1" )));
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["id"].Visible = false;
            dgvCustomer.Columns["SN"].Width = 50;
            dgvCustomer.Columns["code"].Width = 100;
            dgvCustomer.Columns["Bank Name"].MinimumWidth = 200;
            dgvCustomer.Columns["Abbreviation Name"].MinimumWidth = 150;
            dgvCustomer.Columns["Bank Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            if (dgvCustomer.Rows.Count > 0)
            {
                btnRemove.Enabled = UserPermissions.canDelete("Bank Center");
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("update tbl_bank set state = -1 where id =@id",
                DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Bank", "Bank", Convert.ToInt32(dgvCustomer.SelectedRows[0].Cells["id"].Value), "Deleted Bank: " + dgvCustomer.SelectedRows[0].Cells["Bank Name"].Value);
            BindBanks();
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomer.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["id"].Value);
                new frmBankRegister(selectedId).ShowDialog();
            }
        }

        private void CmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindBanks();
        }
    }
}
