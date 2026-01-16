using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterVatReport : Form
    {
        public MasterVatReport()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new frmAddnvoice(_mainForm, this, 0));
        }
        private void MasterVatReport_Load(object sender, EventArgs e)
        {
            cmbType.SelectedIndex = 0;
            BindVat();
        }
        public void BindVat()
        {
            DataTable dt;

            if (cmbType.Text == "Vat Input")
            {
                if (chkDate.Checked)
                    dt = DBClass.ExecuteDataTable(@"SELECT s.id,'Purchase' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                             s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                            FROM tbl_purchase s, tbl_vendor c
                            WHERE s.vendor_id = c.id AND s.vat>0
                            UNION ALL
                            SELECT s.id,'Sales Return' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                    s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                                    FROM tbl_sales_return s, tbl_customer c
                                    WHERE s.customer_id = c.id AND s.vat>0
                            UNION ALL
                            SELECT s.id,'Debit Note' TYPE,s.date AS DATE,s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                     '' AS 'Payment Method',s.amount AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.total AS 'Total Amount' 
									FROM tbl_debit_note s, tbl_vendor c, tbl_purchase p
                            WHERE p.invoice_id = s.invoice_id and p.vendor_id = c.id AND s.vat>0 
                            UNION ALL
                            SELECT s.id,'Petty Cash' type,s.date AS DATE,s.code AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                     '' AS 'Payment Method',s.total_before_vat AS 'Amount Before Vat', s.total_vat AS 'Vat Amount', s.net_amount AS 'Total Amount' 
									FROM tbl_petty_cash_submition s, tbl_employee c
							WHERE s.name = c.id AND s.total_vat>0");
                else
                    dt = DBClass.ExecuteDataTable(@"SELECT s.id,'Purchase' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                             s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                            FROM tbl_purchase s, tbl_vendor c
                            WHERE s.vendor_id = c.id AND s.vat>0 AND s.DATE BETWEEN @datefrom AND @dateTo
                            UNION ALL
                            SELECT s.id,'Sales Return' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                   s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                                   FROM tbl_sales_return s, tbl_customer c
                                   WHERE s.customer_id = c.id AND s.vat>0 AND s.DATE BETWEEN @datefrom AND @dateTo
                            UNION ALL
                            SELECT s.id,'Debit Note' TYPE,s.date AS DATE,s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                   '' AS 'Payment Method',s.amount AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.total AS 'Total Amount' 
								   FROM tbl_debit_note s, tbl_vendor c, tbl_purchase p
                            WHERE p.invoice_id = s.invoice_id and p.vendor_id = c.id AND s.vat>0 AND s.DATE BETWEEN @datefrom AND @dateTo 
                            UNION ALL
                            SELECT s.id,'Petty Cash' type,s.date AS DATE,s.code AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                     '' AS 'Payment Method',s.total_before_vat AS 'Amount Before Vat', s.total_vat AS 'Vat Amount', s.net_amount AS 'Total Amount' 
									FROM tbl_petty_cash_submition s, tbl_employee c
							WHERE s.name = c.id AND s.total_vat>0 AND s.DATE BETWEEN @datefrom AND @dateTo",
                DBClass.CreateParameter("dateFrom", dtFrom.Value.Date),
                            DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            }
            else
            {
                if (chkDate.Checked)
                    dt = DBClass.ExecuteDataTable(@"SELECT s.id,'Sales' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                                    s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                                                    FROM tbl_sales s, tbl_customer c WHERE s.customer_id = c.id AND s.vat>0
                                                    UNION ALL
                                                    SELECT s.id,'Purchase Return' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                                    s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                                                    FROM tbl_purchase_return s, tbl_vendor c WHERE s.vendor_id = c.id AND s.vat>0
                                                    UNION ALL
                                                    SELECT s.id,'Debit Note' TYPE,s.date AS DATE,s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                                    '' AS 'Payment Method',s.amount AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.total AS 'Total Amount' 
                                                    FROM tbl_credit_note s, tbl_customer c, tbl_sales p
                                                    WHERE p.invoice_id = s.invoice_id and p.customer_id = c.id AND s.vat>0  ");
                else
                    dt = DBClass.ExecuteDataTable(@"SELECT s.id,'Sales' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                                    s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                                                    FROM tbl_sales s, tbl_customer c WHERE s.customer_id = c.id AND s.vat>0
                                                    UNION ALL
                                                    SELECT s.id,'Purchase Return' type,s.date AS DATE, s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                                    s.payment_method AS 'Payment Method',s.total AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.net AS 'Total Amount'
                                                    FROM tbl_purchase_return s, tbl_vendor c WHERE s.vendor_id = c.id AND s.vat>0
                                                    UNION ALL
                                                    SELECT s.id,'Credit Note' TYPE,s.date AS DATE,s.invoice_id AS 'Inv No', CONCAT(c.code,' - ',c.name) AS 'Name',
                                                    '' AS 'Payment Method',s.amount AS 'Amount Before Vat', s.vat AS 'Vat Amount', s.total AS 'Total Amount' 
                                                    FROM tbl_credit_note s, tbl_customer c, tbl_sales p
                                                    WHERE p.invoice_id = s.invoice_id and p.customer_id = c.id AND s.vat>0 AND s.DATE BETWEEN @datefrom AND @dateTo",
                                        DBClass.CreateParameter("dateFrom", dtFrom.Value.Date),
                                        DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            }
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["id"].Visible = false;
            dgvCustomer.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["Amount Before Vat"].Width = dgvCustomer.Columns["Vat Amount"].Width = dgvCustomer.Columns["Total Amount"].Width = 160;
            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.Format = "N2";
            style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCustomer.Columns["Amount Before Vat"].DefaultCellStyle = style;
            dgvCustomer.Columns["Vat Amount"].DefaultCellStyle = style;
            dgvCustomer.Columns["Total Amount"].DefaultCellStyle = style;
            dgvCustomer.Columns["Payment Method"].Width = 120;
            dgvCustomer.Columns["type"].Width = 100;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbType.Text == "Customer")
                BindVat();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            //_mainForm.openChildForm(new frmAddnvoice(_mainForm, this, int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            string type = cmbType.Text.Trim() == "Vat Input" ? "Vat Input" : "Vat Output";

            //frmLogin.frmMain.openChildForm(new MasterTransactionJournal("0", type));
            //else
            if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Sales")
                frmLogin.frmMain.openChildForm(new frmSales(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Purchase")
                frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Purchase Return")
                frmLogin.frmMain.openChildForm(new frmPurchaseReturn(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Sales Return")
                frmLogin.frmMain.openChildForm(new frmSalesReturn(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Debit Note")
                frmLogin.frmMain.openChildForm(new frmDebitNote(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Credit Note")
                frmLogin.frmMain.openChildForm(new frmCreditNote(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Petty Cash")
                frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
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
            if (dgvCustomer.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_sales SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Sales", "Sales", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Sales: " + dgvCustomer.SelectedRows[0].Cells["Inv No"].Value.ToString());
            
            BindVat();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));

        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindVat();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindVat();
            headerUC1.FormText = "Vat Report ( "+cmbType.Text+" )";
        }

        private void BindTypes(string text)
        {
          
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindVat();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
