using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterSales : Form
    {
        private EventHandler customerUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;

        public MasterSales()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.Customer += customerUpdatedHandler;
            EventHub.SalesInv += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Customer -= customerUpdatedHandler;
            EventHub.SalesInv -= invoiceUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {          
            frmLogin.frmMain.openChildForm(new frmSales(0));
        }
        private void MasterSales_Load(object sender, EventArgs e)
        {
            btnDelete.Enabled = UserPermissions.canDelete("Create Invoice");
            BindCombos.PopulateCustomers(cmbCustomer);
            BindInvoices();
        }
        //        public void BindInvoices()
        //        {
        //            DataTable dt;
        //            string query = "";
        //            if (cmbSelectionMethod.Text == "Default")
        //            {
        //                query = @"SELECT 
        //    ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
        //    tbl_sales.date AS Date, 
        //    tbl_sales.id,
        //concat('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
        //    tbl_sales.invoice_id AS 'INV NO', 
        //    CONCAT(tbl_customer.code,' - ', tbl_customer.name) AS 'Customer Name', tbl_sales.payment_method as 'Payment Method',
        //    tbl_sales.total AS Total,tbl_sales.vat AS Vat,tbl_sales.pay as 'Pay',tbl_sales.change as 'Change'
        //FROM 
        //    tbl_sales
        //INNER JOIN 
        //    tbl_transaction ON tbl_sales.id = tbl_transaction.transaction_id
        //INNER JOIN 
        //    tbl_customer ON tbl_sales.customer_id = tbl_customer.id
        //WHERE 
        //    tbl_sales.state = 0

        //";
        //            }
        //            else
        //            {
        //                query = @"SELECT 
        //    ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
        //    tbl_sales.date AS Date, 
        //    tbl_sales.id,
        //    tbl_sales.invoice_id AS 'INV NO', 
        //    CONCAT(tbl_customer.code,' - ', tbl_customer.name) AS 'Customer Name', tbl_sales.payment_method as 'Payment Method',
        //    tbl_sales.total AS Total,tbl_sales.vat AS Vat,tbl_sales.pay as 'Pay',tbl_sales.change as 'Change',concat(ti.code,' - ',ti.name) as 'Item Name' ,ts.qty As Qty,ts.price 
        //    As Price , ts.Vat as 'Item Vat' ,ts.total as 'Item Total'
        //FROM 
        //    tbl_sales
        // INNER JOIN tbl_sales_details ts ON tbl_sales.id = ts.sales_id
        //inner join tbl_items ti on ts.item_id =ti.id
        //INNER JOIN 
        //    tbl_customer ON tbl_sales.customer_id = tbl_customer.id
        //WHERE 
        //    tbl_sales.state = 0

        //";
        //            }
        //            List<MySqlParameter> parameters = new List<MySqlParameter>();

        //            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
        //            {
        //                query += " and tbl_sales.customer_id = @id";
        //                parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
        //            }
        //            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
        //            {
        //                query += " and tbl_sales.payment_method = @payment";
        //                parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
        //            }
        //            if (!chkDate.Checked)
        //                query += " and tbl_sales.date >= @dateFrom and tbl_sales.date <= @dateTo";
        //            if (cmbSelectionMethod.Text == "Default")
        //                query += " GROUP BY tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales.total; ";

        //            else
        //                query += " GROUP BY tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales.total,ti.code,ti.name,ts.qty,ts.price,ts.vat,ts.total; ";
        //            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
        //            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
        //            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
        //            dgvInvoice.DataSource = dt;
        //            dgvInvoice.Columns["Customer Name"].MinimumWidth = 200;
        //            dgvInvoice.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        //            dgvInvoice.Columns["id"].Visible = false;
        //            if (dgvInvoice.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
        //                bindInvoiceItems();
        //            else
        //                dgvItems.DataSource = null;
        //        }

        //public void BindInvoices()
        //{
        //    string query = GetQuery();
        //    List<MySqlParameter> parameters = GetQueryParameters();

        //    DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
        //    BindDataToGrid(dt);
        //    HandleInvoiceItems();
        //}
        private void BindInvoices()
        {
            // Disable UI controls while loading (optional)
            dgvInvoice.Enabled = false;

            // Capture current filter values to avoid cross-thread issues
            string selectionMethod = cmbSelectionMethod.Text;
            string customerText = cmbCustomer.Text;
            bool customerChecked = chkCustomer.Checked;
            string paymentMethod = cmbPaymentMethod.Text;
            bool paymentChecked = chkPayment.Checked;
            bool dateChecked = chkDate.Checked;
            DateTime dateFrom = dtFrom.Value.Date;
            DateTime dateTo = dtTo.Value.Date;

            // Prepare query and parameters on background thread
            string query = GetQuery();
            List<MySqlParameter> parameters = GetQueryParameters();

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

            BindDataToGrid(dt);
            HandleInvoiceItems();
            dgvInvoice.Enabled = true;
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
                filters.Add("tbl_sales.customer_id = @id");
            }
            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                filters.Add("tbl_sales.payment_method = @payment");
            }
            if (!chkDate.Checked)
            {
                filters.Add("tbl_sales.date >= @dateFrom and tbl_sales.date <= @dateTo");
            }

            return string.Join(" AND ", filters);
        }
        private string GetDefaultQuery()
        {
            return @"SELECT 
                        ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
                        tbl_sales.date AS Date, 
                        tbl_sales.id,
                        tbl_sales.invoice_id AS 'INV NO', 
                        concat('000',    MAX(tbl_transaction.transaction_id)) AS 'Jv No', 
                        CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', 
                        tbl_sales.payment_method AS 'Payment Method',
                        tbl_sales.total AS Total,
                        tbl_sales.vat AS Vat,
                        tbl_sales.Net AS 'Net'
                    FROM tbl_sales
                    INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                    INNER JOIN 
                    tbl_transaction ON tbl_sales.id = tbl_transaction.transaction_id
                    WHERE tbl_sales.state = 0
                    GROUP BY tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales.total;";
        }

        private string GetDetailedQuery()
        {
            return @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
                    tbl_sales.date AS Date, 
                    tbl_sales.id,
                    tbl_sales.invoice_id AS 'INV NO', 
                    CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', 
                    tbl_sales.payment_method AS 'Payment Method',
                    tbl_sales.total AS Total,
                    tbl_sales.vat AS Vat,
                    tbl_sales.net AS 'Net',
                    CONCAT(ti.code, ' - ', ti.name) AS 'Item Name',
                    ts.qty AS Qty,
                    ts.price AS Price,
                    ts.vat AS 'Item Vat',
                    ts.total AS 'Item Total'
                FROM tbl_sales
                INNER JOIN tbl_sales_details ts ON tbl_sales.id = ts.sales_id
                INNER JOIN tbl_items ti ON ts.item_id = ti.id
                INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                WHERE tbl_sales.state = 0
                GROUP BY tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales.total, ti.code, ti.name, ts.qty, ts.price, ts.vat, ts.total;";
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
            dgvInvoice.Columns["id"].Visible = false;
            if (dgvInvoice.Rows.Count > 0)
            {
                btnEdit.Enabled = UserPermissions.canEdit("Create Invoice");
                btnDelete.Enabled = btnRecycle.Enabled = UserPermissions.canDelete("Create Invoice");
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
                                                     tbl_sales_details ts inner join tbl_items ti on ts.item_id = ti.id where sales_id = @id",
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
            if(dgvInvoice.Columns["Jv No"].Index == e.ColumnIndex)
            {
                frmLogin.frmMain.openChildForm(new MasterTransactionJournal(dgvInvoice.CurrentRow.Cells["Jv No"].Value.ToString(),"Sales"));
            }
            else
            {
                frmLogin.frmMain.openChildForm(new frmSales(int.Parse(dgvInvoice.CurrentRow.Cells["id"].Value.ToString())));
            }
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

            DBClass.ExecuteNonQuery("UPDATE tbl_sales SET state = -1 WHERE id = @id ; UPDATE tbl_transaction SET state= -1 WHERE transaction_id=@id AND t_type = 'SALES'; ",
                                          DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            CommonInsert.DeleteItemTransaction("Sales", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString());

            Utilities.LogAudit(frmLogin.userId, "Delete Invoice", "Sales", int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Invoice: " + dgvInvoice.SelectedRows[0].Cells["INV NO"].Value.ToString());

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

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRecycle_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("Sales");
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
