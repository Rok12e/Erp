using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterSalesQuotation : Form
    {
        private EventHandler customerUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;

        public MasterSalesQuotation()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.Customer += customerUpdatedHandler;
            EventHub.SalesQuotation += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterSalesQuotation_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Customer -= customerUpdatedHandler;
            EventHub.SalesQuotation -= invoiceUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmAddQuotation(0));
        }
        private void MasterSalesQuotation_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            BindInvoices();
        }
       
        public void BindInvoices()
        {
            string query = GetQuery();
            List<MySqlParameter> parameters = GetQueryParameters();

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            BindDataToGrid(dt);
            HandleInvoiceItems();
        }

        private string GetQuery()
        {
            string query = cmbSelectionMethod.Text == "Default" ? GetDefaultQuery() : GetDetailedQuery();
            return query;
        }
        private string GetWhereClause()
        {
            List<string> filters = new List<string>();

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                filters.Add("tbl_sales_quotation.customer_id = @id");
            }
            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                filters.Add("tbl_sales_quotation.payment_method = @payment");
            }
            if (!chkDate.Checked)
            {
                filters.Add("tbl_sales_quotation.date >= @dateFrom and tbl_sales_quotation.date <= @dateTo");
            }

            return string.Join(" AND ", filters);
        }
        private string GetDefaultQuery()
        {
            return @"SELECT 
                        ROW_NUMBER() OVER (ORDER BY tbl_sales_quotation.date) AS `SN`, 
                        tbl_sales_quotation.date AS Date, 
                        tbl_sales_quotation.id,
                        tbl_sales_quotation.invoice_id AS 'INV NO', 
                        CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', 
                        tbl_sales_quotation.payment_method AS 'Payment Method',
                        tbl_sales_quotation.total AS Total,
                        tbl_sales_quotation.vat AS Vat,
                        tbl_sales_quotation.net AS 'Net',
                        tbl_sales_quotation.sales_id AS 'SalesId',
                        tbl_sales_quotation.tranfer_status
                    FROM tbl_sales_quotation
                    INNER JOIN tbl_customer ON tbl_sales_quotation.customer_id = tbl_customer.id
                    WHERE tbl_sales_quotation.state = 0
                    GROUP BY tbl_sales_quotation.id, tbl_sales_quotation.date, tbl_sales_quotation.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales_quotation.total;";
        }

        private string GetDetailedQuery()
        {
            return @"SELECT 
                        ROW_NUMBER() OVER (ORDER BY tbl_sales_quotation.date) AS `SN`, 
                        tbl_sales_quotation.date AS Date, 
                        tbl_sales_quotation.id,
                        tbl_sales_quotation.invoice_id AS 'INV NO', 
                        CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', 
                        tbl_sales_quotation.payment_method AS 'Payment Method',
                        tbl_sales_quotation.total AS Total,
                        tbl_sales_quotation.vat AS Vat,
                        tbl_sales_quotation.net AS 'Net',
                        tbl_sales_quotation.sales_id AS 'SalesId',
                        tbl_sales_quotation.tranfer_status,
                        CONCAT(ti.code, ' - ', ti.name) AS 'Item Name',
                        ts.qty AS Qty,
                        ts.price AS Price,
                        ts.vat AS 'Item Vat',
                        ts.total AS 'Item Total'
                    FROM tbl_sales_quotation
                    INNER JOIN tbl_sales_quotation_details ts ON tbl_sales_quotation.id = ts.sales_id
                    INNER JOIN tbl_items ti ON ts.item_id = ti.id
                    INNER JOIN tbl_customer ON tbl_sales_quotation.customer_id = tbl_customer.id
                    WHERE tbl_sales_quotation.state = 0
                    GROUP BY tbl_sales_quotation.id, tbl_sales_quotation.date, tbl_sales_quotation.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales_quotation.total, ti.code, ti.name, ts.qty, ts.price, ts.vat, ts.total;";
        }

        private List<MySqlParameter> GetQueryParameters()
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>
    {
        DBClass.CreateParameter("dateFrom", dtFrom.Value.Date),
        DBClass.CreateParameter("dateTo", dtTo.Value.Date)
    };

            AddCustomerAndPaymentFilters(parameters);
            return parameters;
        }

        private void AddCustomerAndPaymentFilters(List<MySqlParameter> parameters)
        {
            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
            }
            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
            }
        }

        private void BindDataToGrid(DataTable dt)
        {
            dgvInvoice.DataSource = dt;
            dgvInvoice.Columns["Customer Name"].MinimumWidth = 200;
            dgvInvoice.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvInvoice.Columns["id"].Visible = dgvInvoice.Columns["tranfer_status"].Visible = false;
            if (dgvInvoice.Rows.Count > 0)
            {
                btnEdit.Enabled = UserPermissions.canEdit("Quotation");
                btnDelete.Enabled = btnRecycle.Enabled = UserPermissions.canDelete("Quotation");
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoice);
        }

        private void HandleInvoiceItems()
        {
            if (dgvInvoice.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            {
                bindInvoiceItems();
            }
            else
            {
                dgvItems.DataSource = null;
            }
        }
        private void bindInvoiceItems()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.price as Price , ts.vat as Vat , ts.total as Total from 
                                                     tbl_sales_quotation_details ts inner join tbl_items ti on ts.item_id = ti.id where sales_id = @id",
                                                     DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            dgvItems.DataSource = dt;
            dgvItems.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
            frmLogin.frmMain.openChildForm(new frmSales(int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmAddQuotation(int.Parse(dgvInvoice.CurrentRow.Cells["id"].Value.ToString())));
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
            if (dgvInvoice.Rows.Count == 0)
                return;

            DBClass.ExecuteNonQuery("UPDATE tbl_sales_quotation SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Sales Quotation", "Sales Quotation", int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Sales Quotation: " + dgvInvoice.SelectedRows[0].Cells["INV NO"].Value.ToString());

            BindInvoices();
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

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
            if (int.Parse(dgvInvoice.SelectedRows[0].Cells["tranfer_status"].Value.ToString()) <= 0)
            {         
                frmLogin.frmMain.openChildForm(new frmSales(int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()), "SQ"));
            }
            else
            {
                MessageBox.Show("Sales Quotation already created.", "Cannot Create", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRecycle_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("SalesQutation");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            BindInvoices();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
