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
    public partial class MasterSalesReturn : Form
    {
        private EventHandler customerUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;
        public MasterSalesReturn()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.Customer += customerUpdatedHandler;
            EventHub.SalesInv += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmSalesReturn(0));
        }
        private void MasterSalesReturn_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            string query = @"SELECT 
                                ROW_NUMBER() OVER (ORDER BY tbl_sales_return.date) AS `SN`, 
                                tbl_sales_return.date AS Date, 
                                tbl_sales_return.id,
                            concat('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                                tbl_sales_return.invoice_id AS 'INV NO', 
                                CONCAT(tbl_customer.code,'-', tbl_customer.name) AS 'Customer Name', tbl_sales_return.payment_method as 'Payment Method',
                                tbl_sales_return.total AS Total,tbl_sales_return.vat AS Vat,tbl_sales_return.net as 'Net'
                            FROM 
                                tbl_sales_return
                            INNER JOIN 
                                tbl_transaction ON tbl_sales_return.id = tbl_transaction.transaction_id
                            INNER JOIN 
                                tbl_customer ON tbl_sales_return.customer_id = tbl_customer.id
                            WHERE 
                                tbl_sales_return.state = 0

                            ";
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                query += " and tbl_sales_return.customer_id = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
            }
            if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
            {
                query += " and tbl_sales_return.payment_method = @payment";
                parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
            }
            if (!chkDate.Checked)
                query += " and tbl_sales_return.date >= @dateFrom and tbl_sales_return.date <= @dateTo";
            query+= " GROUP BY tbl_sales_return.id, tbl_sales_return.date, tbl_sales_return.invoice_id, tbl_customer.code, tbl_customer.name, tbl_sales_return.total; ";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            BindDataToGrid(dt);
            HandleInvoiceItems();
        }
        private void BindDataToGrid(DataTable dt)
        {
            dgvInvoice.DataSource = dt;
            dgvInvoice.Columns["Customer Name"].MinimumWidth = 200;
            dgvInvoice.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvInvoice.Columns["id"].Visible = false;
            if (dgvInvoice.Rows.Count > 0)
            {
                btnEdit.Enabled = UserPermissions.canEdit("Sales Return");
                btnDelete.Enabled = btnRestore.Enabled = UserPermissions.canDelete("Sales Return");
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
                dgvInvoice.DataSource = null;
            }
        }
        private void bindInvoiceItems()
        {
            //DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.price as Price , ts.vat as Vat , ts.total as Total from 
            //                                         tbl_sales_details ts inner join tbl_items ti on ts.item_id = ti.id where sales_id = @id",
            //                                         DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            //dgvInvoice.DataSource = dt;
            //dgvInvoice.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmSalesReturn(int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInvoice.Rows.Count == 0)
                return;
                frmLogin.frmMain.openChildForm(new frmSalesReturn(int.Parse(dgvInvoice.CurrentRow.Cells["JV NO"].Value.ToString())));
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
            DBClass.ExecuteNonQuery("UPDATE tbl_sales_return SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()));
            CommonInsert.DeleteItemTransaction("Sales Return", dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString());
            Utilities.LogAudit(frmLogin.userId, "Delete Sales Return", "Sales Return", int.Parse(dgvInvoice.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Sales Return: " + dgvInvoice.SelectedRows[0].Cells["INV NO"].Value.ToString());
            BindInvoices();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("SalesReturn");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
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
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindInvoices();
        }

        private void MasterSalesReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Customer -= customerUpdatedHandler;
            EventHub.SalesInv -= invoiceUpdatedHandler;
        }

        private void dgvInvoice_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
