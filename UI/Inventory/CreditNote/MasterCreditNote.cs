using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;
namespace YamyProject
{
    public partial class MasterCreditNote : Form
    {
        private EventHandler customerUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;

        public MasterCreditNote()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.CreditNote += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterCreditNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            //EventHub.Customer -= customerUpdatedHandler;
            //EventHub.SalesInv -= invoiceUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmCreditNote(0));
        }
        private void MasterCreditNote_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            string query = "";
            if (cmbSelectionMethod.Text == "Default")
            {
                //SELECT c.id,c.date,c.credit_account,c.debit_account,c.invoice_id,c.amount,c.vat,c.description FROM tbl_credit_note c
                query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY c.date) AS `SN`, 
                            c.date AS Date, 
                            c.id,
                        CONCAT('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                            c.invoice_id AS 'INV NO',
                            c.amount AS Amount,c.vat AS Vat,c.credit_account,c.debit_account
                        FROM 
                            tbl_credit_note c
                        INNER JOIN 
                            tbl_transaction ON c.id = tbl_transaction.transaction_id
                        INNER JOIN 
                            tbl_customer ON c.credit_account = tbl_customer.id
                        WHERE 
                            c.state = 0
                        ";
            }
            else
            {
                query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY c.date) AS `SN`, 
                            c.date AS Date, 
                            c.id,
                            c.invoice_id AS 'INV NO',
                            c.amount AS Amount,c.vat AS Vat,c.credit_account,c.debit_account,ts.invoice_id,ts.invoice_date,ts.invoice_type 
                            ,ts.amount,ts.vat
                        FROM 
                            tbl_credit_note c
                         INNER JOIN tbl_credit_note_details ts ON c.id = ts.ref_id
                        WHERE 
                            c.state = 0
                        ";
            }
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            //if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            //{
            //    query += " and tbl_purchase.vendor_id = @id";
            //    parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
            //}
            //if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            //{
            //    query += " and tbl_purchase.payment_method = @payment";
            //    parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
            //}
            if (!chkDate.Checked)
                query += " and c.date >= @dateFrom and c.date <= @dateTo";
            if (cmbSelectionMethod.Text == "Default")
                query += " GROUP BY c.id,c.date,c.credit_account,c.debit_account,c.invoice_id,c.amount,c.vat,c.description; ";

            else
                query += " GROUP BY c.id,c.date,c.credit_account,c.debit_account,c.invoice_id,c.amount,c.vat,ts.invoice_id,ts.invoice_date,ts.invoice_type,ts.amount,ts.vat; ";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvInvoice.DataSource = dt;
            dgvInvoice.Columns["id"].Visible = false;
            dgvInvoice.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            dgvInvoice.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvInvoice.ColumnHeadersHeight = 18;
            dgvInvoice.EnableHeadersVisualStyles = false;
            dgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvInvoice.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoice);
        }

        private void bindInvoiceItems()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"select * from tbl_credit_note_details where ref_id = @id",
                                                     DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            dgvItems.DataSource = dt;
         
          
            //dgvItems.Columns["invoice_type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvItems.Columns["ref_id"].Visible = dgvItems.Columns["ref_id"].Visible = dgvItems.Columns["invoice_id"].Visible = false;
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvItems.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvItems.ColumnHeadersHeight = 18;
            dgvItems.EnableHeadersVisualStyles = false;
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvItems.Rows.Count > 0)
            {
                btnDelete.Enabled = btnRecycle.Enabled = UserPermissions.canDelete("Credit Note");
                btnEdit.Enabled = UserPermissions.canEdit("Credit Note");
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmCreditNote(int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
            //if (dgvInvoice.Columns["JV NO"].Index == e.ColumnIndex)
            //    frmLogin.frmMain.openChildForm(new MasterTransactionJournal(null, dgvCustomer.CurrentRow.Cells["JV NO"].Value.ToString(), "Purchse"));
            //else
                frmLogin.frmMain.openChildForm(new frmCreditNote(int.Parse(dgvInvoice.CurrentRow.Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
                cmbCustomer.Enabled = false;
            else
                cmbCustomer.Enabled = true;
            BindInvoices();
        }
        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayment.Checked)
                cmbPaymentMethod.Enabled = false;
            else
                cmbPaymentMethod.Enabled = true;
            BindInvoices();
        }
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkDate.Checked;
            dtFrom.Enabled = dtTo.Enabled = !isChecked;
            BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInvoice.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_credit_note SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Credit Note", "Credit Note", int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Credit Note: " + dgvInvoice.SelectedRows[0].Cells["INV NO"].Value.ToString());
            BindInvoices();
        }
        private void btnRecycle_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("CreditNote");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            BindInvoices();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
