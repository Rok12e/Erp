using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterAdvancePaymentVoucher : Form
    {
        private EventHandler PaymentVoucherUpdatedHandler;

        public MasterAdvancePaymentVoucher()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            PaymentVoucherUpdatedHandler = (sender, args) => BindPayment(cmbType.Text);
            EventHub.PaymentVoucher += PaymentVoucherUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterAdvancePaymentVoucher_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.PaymentVoucher -= PaymentVoucherUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucherAdvance());
        }
        private void MasterAdvancePaymentVoucher_Load(object sender, EventArgs e)
        {
            cmbType.SelectedIndex = 0;
            StyleGrid(dgvCustomer);
            StyleGrid(dgvDetails);
            guna2HtmlLabel2.Text  = "Advance Payment Voucher Center";
        }

        private void StyleGrid(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(204, 229, 255);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            // Header styles
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Silver; // Set header background to Silver
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;  // Keep text black for contrast
            dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);

            // General Grid Appearance
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowTemplate.Height = 28;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public void BindPayment(string type)
        {
            DataTable dt;
            if (chkDate.Checked)
                dt = DBClass.ExecuteDataTable(@"
            SELECT 
                tp.id, 
                tp.date AS Date,
                tp.pv_code AS 'Payment Code',
                (select concat('000',    MAX(tbl_transaction.transaction_id)) FROM tbl_transaction WHERE tbl_transaction.transaction_id=tp.id) AS 'JV NO', 
                tp.amount AS Amount,
                CONCAT(tc.code, ' - ', tc.name) AS 'Debit Account', 
                CONCAT(tc1.code, ' - ', tc1.name) AS 'Credit Account' 
            FROM tbl_advance_payment_voucher tp
            INNER JOIN tbl_coa_level_4 tc ON tp.debit_account_id = tc.id
            INNER JOIN tbl_coa_level_4 tc1 ON tp.credit_account_id = tc1.id
            WHERE tp.type = @type",
                    DBClass.CreateParameter("type", type));
            else
                dt = DBClass.ExecuteDataTable(@"
            SELECT 
                tp.id, 
                tp.pv_code AS 'Payment Code',
                tp.amount AS Amount,
                CONCAT(tc.code, ' - ', tc.name) AS 'Debit Account', 
                CONCAT(tc1.code, ' - ', tc1.name) AS 'Credit Account' 
            FROM tbl_advance_payment_voucher tp
            INNER JOIN tbl_coa_level_4 tc ON tp.debit_account_id = tc.id
            INNER JOIN tbl_coa_level_4 tc1 ON tp.credit_account_id = tc1.id
            WHERE  tp.type = @type 
                AND tp.date >= @dateFrom 
                AND tp.date <= @dateTo",
                    DBClass.CreateParameter("dateFrom", dtFrom.Value.Date.ToString("yyyy-MM-dd")),
                    DBClass.CreateParameter("dateTo", dtTo.Value.Date.ToString("yyyy-MM-dd")),
                    DBClass.CreateParameter("type", type));

            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["id"].Visible = false;
            if (dgvCustomer.Rows.Count > 0)
                BindPVDetails();
            else
                dgvDetails.DataSource = null;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            if (dgvCustomer.Columns["JV NO"].Index == e.ColumnIndex)
                frmLogin.frmMain.openChildForm(new MasterTransactionJournal(dgvCustomer.CurrentRow.Cells["JV NO"].Value.ToString(), "PAYMENT"));
            else
                frmLogin.frmMain.openChildForm(new frmViewPaymentVoucherAdvance(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindPayment(cmbType.Text);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPayment(cmbType.Text);

        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindPVDetails();
        }

        private void BindPVDetails()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"
                                   SELECT 
                                   e.name,
                                   pd.amount,
                                   pd.description
                                FROM tbl_advance_payment_voucher_details pd
                                INNER JOIN tbl_employee e ON pd.name = e.id
                                WHERE pd.payment_id =@id;",
                      DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));

            dgvDetails.DataSource = dt;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvDetails);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            if (UserPermissions.canDelete("Vouchers")) {
                DBClass.ExecuteNonQuery("UPDATE tbl_advance_payment_voucher SET state = -1 WHERE id = @id ",
                                              DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
                BindPayment(cmbType.SelectedItem.ToString());
                Utilities.LogAudit(frmLogin.userId, "Delete Advance Payment Voucher", "Advance Payment Voucher", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Advance Payment Voucher: " + dgvCustomer.SelectedRows[0].Cells["Payment Code"].Value.ToString());
            }
        }

        private void btnRecycle_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("Payment");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            BindPayment(cmbType.SelectedItem.ToString());
        }
    }
}
