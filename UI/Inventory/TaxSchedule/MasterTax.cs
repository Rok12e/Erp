using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;
namespace YamyProject
{
    public partial class MasterItemTaxCode : Form
    {
        private EventHandler taxUpdatedHandler;

        public MasterItemTaxCode()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            taxUpdatedHandler = (sender, args) => BindTax();
            EventHub.TaxSchedule += taxUpdatedHandler;
            headerUC1.FormText = "Master ItemTaxCode";
        }
        private void MasterTax_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.TaxSchedule -= taxUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmViewItemTaxCodes().ShowDialog();
        }
        private void MasterTax_Load(object sender, EventArgs e)
        {
            BindTax();
        }
        public void BindTax()
        {
            string query = "SELECT id, Name, value, Description FROM tbl_tax WHERE state = 0";
            DataTable dt = DBClass.ExecuteDataTable(query);

            dgvTax.SuspendLayout();
            dgvTax.DataSource = dt;

            // Adjust column widths and styles
            dgvTax.Columns["id"].Visible = false;
            if (dgvTax.Columns.Contains("Name"))
            {
                dgvTax.Columns["Name"].HeaderText = "Tax Name";
                dgvTax.Columns["Name"].MinimumWidth = 180;
                dgvTax.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvTax.Columns.Contains("value"))
            {
                dgvTax.Columns["value"].HeaderText = "Rate (%)";
                dgvTax.Columns["value"].Width = 100;
                dgvTax.Columns["value"].DefaultCellStyle.Format = "N2";
            }

            if (dgvTax.Columns.Contains("Description"))
            {
                dgvTax.Columns["Description"].MinimumWidth = 150;
                dgvTax.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // Header styling
            dgvTax.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvTax.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvTax.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvTax.EnableHeadersVisualStyles = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvTax);

            // Row styling
            dgvTax.RowsDefaultCellStyle.BackColor = Color.White;
            dgvTax.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");

            dgvTax.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d5dbdb");
            dgvTax.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvTax.BorderStyle = BorderStyle.None;
            dgvTax.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvTax.RowHeadersVisible = false;
            dgvTax.AllowUserToAddRows = false;
            dgvTax.AllowUserToResizeRows = false;

            dgvTax.ResumeLayout();

            if (dgvTax.Rows.Count > 0)
            {
                btnDelete.Enabled = btnRecycle.Enabled = UserPermissions.canDelete("Inventory Center");
                btnEdit.Enabled = UserPermissions.canEdit("Inventory Center");
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvTax.Rows.Count == 0)
                return;
            new frmViewItemTaxCodes(int.Parse(dgvTax.CurrentRow.Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvTax.Rows.Count == 0)
                return;
            new frmViewItemTaxCodes(int.Parse(dgvTax.CurrentRow.Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTax.Rows.Count == 0)
                return;

            DBClass.ExecuteNonQuery("UPDATE tbl_tax SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvTax.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Tax", "Tax", int.Parse(dgvTax.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Tax: " + dgvTax.SelectedRows[0].Cells["Name"].Value.ToString());
            BindTax();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRecycle_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("Tax Form");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            BindTax();
        }

    }
}
