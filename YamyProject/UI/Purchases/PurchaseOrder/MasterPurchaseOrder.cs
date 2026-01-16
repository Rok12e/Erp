using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterPurchaseOrder : Form
    {
        private EventHandler vendorUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;

        int _currentPage = 1;
        int _pageSize = 50; // Or any default rows per page
        int _totalRecords = 0;
        int _totalPages = 0;

        public MasterPurchaseOrder()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            vendorUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.Vendor += vendorUpdatedHandler;
            EventHub.PurchaseOrder += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterPurchaseOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Vendor -= vendorUpdatedHandler;
            EventHub.PurchaseOrder -= invoiceUpdatedHandler;

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchaseOrder());
        }
        private void MasterPurchaseOrder_Load(object sender, EventArgs e)
        {
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
                        ROW_NUMBER() OVER (ORDER BY tbl_purchase_order.date) AS `SN`, 
                        tbl_purchase_order.date AS Date, 
                        tbl_purchase_order.id,
                        tbl_purchase_order.invoice_id AS 'INV NO', 
                        CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name', tbl_purchase_order.payment_method as 'Payment Method',
                        tbl_purchase_order.total AS Total,tbl_purchase_order.vat AS Vat,tbl_purchase_order.net as 'Net',tbl_purchase_order.purchase_id as 'P - No',tbl_purchase_order.tranfer_status
                    FROM 
                        tbl_purchase_order
                    INNER JOIN 
                        tbl_vendor ON tbl_purchase_order.vendor_id = tbl_vendor.id
                    WHERE 
                        tbl_purchase_order.state = 0

                    ";
            }
            else
            {
                query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY tbl_purchase_order.date) AS `SN`, 
                            tbl_purchase_order.date AS Date, 
                            tbl_purchase_order.id,
                            tbl_purchase_order.invoice_id AS 'INV NO', 
                            CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name', tbl_purchase_order.payment_method as 'Payment Method',
                            tbl_purchase_order.total AS Total,tbl_purchase_order.vat AS Vat,tbl_purchase_order.net as 'Net',tbl_purchase_order.purchase_id as 'P - No',concat(ti.code,' - ',ti.name) as 'Item Name' ,ts.qty As Qty,ts.cost_price 
                            As 'Cost Price' , ts.Vat as 'Item Vat' ,ts.total as 'Item Total',tbl_purchase_order.tranfer_status
                        FROM 
                            tbl_purchase_order
                         INNER JOIN tbl_purchase_order_details ts ON tbl_purchase_order.id = ts.purchase_id
                        inner join tbl_items ti on ts.item_id =ti.id
                        INNER JOIN 
                            tbl_vendor ON tbl_purchase_order.vendor_id = tbl_vendor.id
                        WHERE 
                            tbl_purchase_order.state = 0

                        ";
            }
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                query += " and tbl_purchase_order.vendor_id = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
            }
            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                query += " and tbl_purchase_order.payment_method = @payment";
                parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
            }
            if (!chkDate.Checked)
                query += " and tbl_purchase_order.date >= @dateFrom and tbl_purchase_order.date <= @dateTo";
            if (cmbSelectionMethod.Text == "Default")
                query += " GROUP BY tbl_purchase_order.id, tbl_purchase_order.date, tbl_purchase_order.invoice_id, tbl_vendor.code, tbl_vendor.name, tbl_purchase_order.total ";

            else
                query += " GROUP BY tbl_purchase_order.id, tbl_purchase_order.date, tbl_purchase_order.invoice_id, tbl_vendor.code, tbl_vendor.name, tbl_purchase_order.total,ti.code,ti.name,ts.qty,ts.cost_price,ts.vat,ts.total ";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));

            int offset = (_currentPage - 1) * _pageSize;
            query += $" LIMIT @_limit OFFSET @_offset";

            parameters.Add(DBClass.CreateParameter("_limit", _pageSize));
            parameters.Add(DBClass.CreateParameter("_offset", offset));
            string countQuery = "SELECT COUNT(*) FROM tbl_purchase_order " +
                    "INNER JOIN tbl_vendor ON tbl_purchase_order.vendor_id = tbl_vendor.id " +
                    "WHERE tbl_purchase_order.state = 0";

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                countQuery += " AND tbl_purchase_order.vendor_id = @id";
            }

            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                countQuery += " AND tbl_purchase_order.payment_method = @payment";
            }

            if (!chkDate.Checked)
                countQuery += " AND tbl_purchase_order.date >= @dateFrom AND tbl_purchase_order.date <= @dateTo";

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

                if (dgvCustomer.Rows.Count > 0)
                {
                    btnEdit.Enabled = UserPermissions.canEdit("Purchase Order");
                    btnDelete.Enabled = btnRecycle.Enabled = UserPermissions.canDelete("Purchase Order");
                }

                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            }

            lblPageInfo.Text = $"Page {_currentPage} of {_totalPages}";
            btnPrev.Enabled = btnFirst.Enabled = _currentPage > 1;
            btnNext.Enabled = btnLast.Enabled = _currentPage < _totalPages;
        }

        private void bindInvoiceItems()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.cost_price as 'Cost Price' , ts.vat as Vat , ts.total as Total from 
                                                     tbl_purchase_order_details ts inner join tbl_items ti on ts.item_id = ti.id where purchase_id = @id",
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
            frmLogin.frmMain.openChildForm(new frmPurchaseOrder( int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmPurchaseOrder(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
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

        private void btnCreateBill_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            if (int.Parse(dgvCustomer.SelectedRows[0].Cells["tranfer_status"].Value.ToString()) <= 0)
            {
                frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString()), "PO"));
            }
            else
            {
                MessageBox.Show("PurchaseOrder already created.", "Cannot Create", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_purchase_order SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Purchase Order", "Purchase Order", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Purchase Order: " + dgvCustomer.SelectedRows[0].Cells["INV NO"].Value.ToString());

            _currentPage = 1;
            BindInvoices();
        }
        private void btnRecycle_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("PurchaseOrder");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();
        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            _currentPage = 1;
            BindInvoices();
        }
    }
}
