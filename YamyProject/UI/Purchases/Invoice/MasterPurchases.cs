using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterPurchases : Form
    {
        private EventHandler vendorUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;
        bool CbSubcontractorsChecked = false;

        int _currentPage = 1;
        int _pageSize = 50; // Or any default rows per page
        int _totalRecords = 0;
        int _totalPages = 0;

        public MasterPurchases(bool _cbSubcontractorsChecked=false)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            vendorUpdatedHandler = (sender, args) => BindCombos.PopulateVendors(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.Vendor += vendorUpdatedHandler;
            EventHub.PurchaseInv += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
            CbSubcontractorsChecked = _cbSubcontractorsChecked;
        }
        private void MasterPurchases_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Vendor -= vendorUpdatedHandler;
            EventHub.PurchaseInv -= invoiceUpdatedHandler;

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchase());
        }
        private void MasterPurchases_Load(object sender, EventArgs e)
        {
            CbSubcontractors.Checked = CbSubcontractorsChecked;
            btnDelete.Enabled = UserPermissions.canDelete("Create Purchases");
            BindCombos.PopulateVendors(cmbCustomer);
            BindInvoices();
        }

        public void BindInvoices()
        {
            DataTable dt;
            string query = "";
            if (cmbSelectionMethod.Text == "Default")
            {
                query = @"SELECT 
                ROW_NUMBER() OVER (ORDER BY tbl_purchase.date) AS `SN`, 
                concat('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                tbl_purchase.date AS Date, 
                tbl_purchase.id,
                tbl_purchase.invoice_id AS 'INV NO', 
                CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name', tbl_purchase.payment_method as 'Payment Method',
                tbl_purchase.total AS Total,tbl_purchase.vat AS Vat,tbl_purchase.net as 'Net'
                FROM 
                    tbl_purchase
                INNER JOIN 
                    tbl_transaction ON tbl_purchase.id = tbl_transaction.transaction_id
                INNER JOIN 
                    tbl_vendor ON tbl_purchase.vendor_id = tbl_vendor.id
                WHERE 
                    tbl_purchase.state = 0

                ";
            }
            else
            {
                query = @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY tbl_purchase.date) AS `SN`, 
                    concat('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                    tbl_purchase.date AS Date, 
                    tbl_purchase.id,
                    tbl_purchase.invoice_id AS 'INV NO', 
                    CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name', tbl_purchase.payment_method as 'Payment Method',
                    tbl_purchase.total AS Total,tbl_purchase.vat AS Vat,tbl_purchase.net as 'Net',concat(ti.code,' - ',ti.name) as 'Item Name' ,ts.qty As Qty,ts.cost_price 
                    As 'Cost Price' , ts.Vat as 'Item Vat' ,ts.total as 'Item Total'
                FROM 
                    tbl_purchase
                 INNER JOIN tbl_purchase_details ts ON tbl_purchase.id = ts.purchase_id
                inner join tbl_items ti on ts.item_id =ti.id
                INNER JOIN 
                    tbl_vendor ON tbl_purchase.vendor_id = tbl_vendor.id
                INNER JOIN 
                    tbl_transaction ON tbl_purchase.id = tbl_transaction.transaction_id
                WHERE 
                    tbl_purchase.state = 0

                ";
            }
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (CbSubcontractors.Checked)
            {
                query += " and tbl_vendor.type = 'Subcontractor'";
            }else { 
                query += " and tbl_vendor.type = 'Vendor'";
            }

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                query += " and tbl_purchase.vendor_id = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
            }
            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                query += " and tbl_purchase.payment_method = @payment";
                parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
            }
            
            if (!chkDate.Checked)
                query += " and tbl_purchase.date >= @dateFrom and tbl_purchase.date <= @dateTo";
            if (cmbSelectionMethod.Text == "Default")
                query += " GROUP BY tbl_purchase.id, tbl_purchase.date, tbl_purchase.invoice_id, tbl_vendor.code, tbl_vendor.name, tbl_purchase.total ";

            else
                query += " GROUP BY tbl_purchase.id, tbl_purchase.date, tbl_purchase.invoice_id, tbl_vendor.code, tbl_vendor.name, tbl_purchase.total,ti.code,ti.name,ts.qty,ts.cost_price,ts.vat,ts.total ";
            
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));

            int offset = (_currentPage - 1) * _pageSize;
            query += $" LIMIT @_limit OFFSET @_offset";

            parameters.Add(DBClass.CreateParameter("_limit", _pageSize));
            parameters.Add(DBClass.CreateParameter("_offset", offset));

            string countQuery = "SELECT COUNT(*) FROM tbl_purchase " +
                    "INNER JOIN tbl_vendor ON tbl_purchase.vendor_id = tbl_vendor.id " +
                    "WHERE tbl_purchase.state = 0";

            // Apply the same filters (vendor type, customer, payment, etc.)
            if (CbSubcontractors.Checked)
                countQuery += " AND tbl_vendor.type = 'Subcontractor'";
            else
                countQuery += " AND tbl_vendor.type = 'Vendor'";

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                countQuery += " AND tbl_purchase.vendor_id = @id";
                // Add param if not already
            }

            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                countQuery += " AND tbl_purchase.payment_method = @payment";
            }

            if (!chkDate.Checked)
                countQuery += " AND tbl_purchase.date >= @dateFrom AND tbl_purchase.date <= @dateTo";

            _totalRecords = Convert.ToInt32(DBClass.ExecuteScalar(countQuery, parameters.ToArray()));
            _totalPages = (_totalRecords + _pageSize - 1) / _pageSize;

            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            if (dt != null)
            {
                dgvCustomer.DataSource = dt;
                dgvCustomer.Columns["Vendor Name"].MinimumWidth = 200;
                dgvCustomer.Columns["Vendor Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvCustomer.Columns["id"].Visible = false;
                if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                    bindInvoiceItems();
                else
                    dgvItems.DataSource = null;

                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            }

            lblPageInfo.Text = $"Page {_currentPage} of {_totalPages}";
            btnPrev.Enabled = btnFirst.Enabled = _currentPage > 1;
            btnNext.Enabled = btnLast.Enabled = _currentPage < _totalPages;

        }

        private void bindInvoiceItems()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.cost_price as 'Cost Price' , ts.vat as Vat , ts.total as Total from 
                                                     tbl_purchase_details ts inner join tbl_items ti on ts.item_id = ti.id where purchase_id = @id",
                                                     DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            dgvItems.DataSource = dt;
            dgvItems.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            BindInvoices();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmPurchase( int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            if (dgvCustomer.Columns["JV NO"].Index == e.ColumnIndex)
                frmLogin.frmMain.openChildForm(new MasterTransactionJournal(dgvCustomer.CurrentRow.Cells["JV NO"].Value.ToString(), "PURCHASE"));
            else
                frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
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
            DBClass.ExecuteNonQuery("UPDATE tbl_purchase SET state = -1 WHERE id = @id; UPDATE tbl_transaction SET state= -1 WHERE transaction_id=@id AND t_type = 'PURCHASE';",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            CommonInsert.DeleteItemTransaction("Purchase", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString());
            Utilities.LogAudit(frmLogin.userId, "Delete Purchase", "Purchase", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Purchase: " + dgvCustomer.SelectedRows[0].Cells["INV NO"].Value.ToString());
            _currentPage = 1;
            BindInvoices();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("Purchase");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            _currentPage = 1;
            BindInvoices();
        }
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
                cmbCustomer.Enabled = false;
            else
                cmbCustomer.Enabled = true;

            _currentPage = 1;
            BindInvoices();
        }
        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayment.Checked)
                cmbPaymentMethod.Enabled = false;
            else
                cmbPaymentMethod.Enabled = true;

            _currentPage = 1;
            BindInvoices();
        }
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;

            _currentPage = 1;
            BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;
        }

        private void CbSubcontractors_CheckedChanged(object sender, EventArgs e)
        {
            _currentPage = 1;
            BindInvoices();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_currentPage < _totalPages)
            {
                _currentPage++;
                BindInvoices();
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                BindInvoices();
            }
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            _currentPage = 1;
            BindInvoices();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            _currentPage = _totalPages;
            BindInvoices();
        }
    }
}
