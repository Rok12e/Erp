using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterSalesReport : Form
    {
        private string type;

        public MasterSalesReport(string _type="")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            type = _type;
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }
        
        private void MasterSalesReport_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            
            cmbSelectionMethod.Text = type == "Customer Summary"? "" : type == "Item Details" ? "Item Wise" : type == "Item Summary" ? "Item Wise" : type== "Aging Summary" ? "A/R Aging Summary" : "";

            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            string query = "";

            if (cmbSelectionMethod.Text == "A/R Aging Details")
            {
                query = @"SELECT 
                                ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
                                tbl_sales.date AS Date, 
                                tbl_sales.id,
                                CONCAT('000', MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                                tbl_sales.invoice_id AS 'INV NO', 
                                CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', 
                                tbl_sales.payment_method AS 'Payment Method',
                                tbl_sales.total AS Total, 
                                tbl_sales.vat AS Vat,
                                tbl_sales.pay AS 'Pay', 
                                tbl_sales.change AS 'Change',
                                DATEDIFF(CURDATE(), tbl_sales.date) AS 'Days Overdue',
                                CASE 
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) BETWEEN 0 AND 30 THEN '0-30 days'
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) BETWEEN 31 AND 60 THEN '31-60 days'
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) BETWEEN 61 AND 90 THEN '61-90 days'
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) > 90 THEN '90+ days'
                                    ELSE 'Not Due Yet' 
                                END AS 'Aging'
                            FROM 
                                tbl_sales
                            INNER JOIN 
                                tbl_transaction ON tbl_sales.id = tbl_transaction.transaction_id
                            INNER JOIN 
                                tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                            WHERE 
                                tbl_sales.state = 0";

                if (cmbCustomer.Text != "" && !chkCustomer.Checked)
                {
                    query += " AND tbl_sales.customer_id = @id";
                    parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
                }

                if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
                {
                    query += " AND tbl_sales.payment_method = @payment";
                    parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
                }

                if (!chkDate.Checked)
                {
                    query += " AND tbl_sales.date BETWEEN @dateFrom AND @dateTo";
                    parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
                }

                query += @" GROUP BY
                    tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, 
                    tbl_sales.payment_method, tbl_sales.total, tbl_sales.vat, tbl_sales.pay, tbl_sales.change";
            }
            else if (cmbSelectionMethod.Text == "A/R Aging Summary")
            {
                query = @"
                        SELECT 
                            CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', 
                            SUM(CASE 
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) BETWEEN 0 AND 30 THEN tbl_sales.total
                                    ELSE 0
                                END) AS '0-30 Days',
                            SUM(CASE 
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) BETWEEN 31 AND 60 THEN tbl_sales.total
                                    ELSE 0
                                END) AS '31-60 Days',
                            SUM(CASE 
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) BETWEEN 61 AND 90 THEN tbl_sales.total
                                    ELSE 0
                                END) AS '61-90 Days',
                            SUM(CASE 
                                    WHEN DATEDIFF(CURDATE(), tbl_sales.date) > 90 THEN tbl_sales.total
                                    ELSE 0
                                END) AS '90+ Days',
                            SUM(tbl_sales.pay) AS 'Total Paid',
                            SUM(tbl_sales.total) - SUM(tbl_sales.pay) AS 'Outstanding Balance'
                        FROM 
                            tbl_sales
                        INNER JOIN 
                            tbl_transaction ON tbl_sales.id = tbl_transaction.transaction_id
                        INNER JOIN 
                            tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                        WHERE 
                            tbl_sales.state = 0";

                if (cmbCustomer.Text != "" && !chkCustomer.Checked)
                {
                    query += " AND tbl_sales.customer_id = @id";
                    parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
                }

                if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
                {
                    query += " AND tbl_sales.payment_method = @payment";
                    parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
                }

                if (!chkDate.Checked)
                {
                    query += " AND tbl_sales.date BETWEEN @dateFrom AND @dateTo";
                    parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
                }

                query += @" GROUP BY 
                                tbl_customer.id, tbl_customer.code, tbl_customer.name
                            ORDER BY 
                                tbl_customer.name";
            }
            else
            {
                if (cmbSelectionMethod.Text == "Item Wise")
                {
                    query = @"SELECT 
                                    ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
                                    tbl_sales.date AS Date, 
                                    tbl_sales.id,
                                    tbl_sales.invoice_id AS 'INV NO', 
                                    CONCAT(tbl_customer.code,' - ', tbl_customer.name) AS 'Customer Name', 
                                    tbl_sales.payment_method AS 'Payment Method',
                                    tbl_sales.total AS Total, 
                                    tbl_sales.vat AS Vat,
                                    tbl_sales.pay AS 'Pay',
                                    tbl_sales.change AS 'Change',
                                    CONCAT(ti.code,' - ',ti.name) AS 'Item Name',
                                    ts.qty AS Qty,
                                    ts.price AS Price,
                                    ts.vat AS 'Item Vat',
                                    ts.total AS 'Item Total'
                                FROM 
                                    tbl_sales
                                INNER JOIN 
                                    tbl_sales_details ts ON tbl_sales.id = ts.sales_id
                                INNER JOIN 
                                    tbl_items ti ON ts.item_id = ti.id
                                INNER JOIN 
                                    tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                                WHERE 
                                    tbl_sales.state = 0";
                }
                else // Default
                {
                    query = @"SELECT 
                                ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS `SN`, 
                                tbl_sales.date AS Date, 
                                tbl_sales.id,
                                CONCAT('000', MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                                tbl_sales.invoice_id AS 'INV NO', 
                                CONCAT(tbl_customer.code,' - ', tbl_customer.name) AS 'Customer Name', 
                                tbl_sales.payment_method AS 'Payment Method',
                                tbl_sales.total AS Total, 
                                tbl_sales.vat AS Vat,
                                tbl_sales.pay AS 'Pay',
                                tbl_sales.change AS 'Change'
                            FROM 
                                tbl_sales
                            INNER JOIN 
                                tbl_transaction ON tbl_sales.id = tbl_transaction.transaction_id
                            INNER JOIN 
                                tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                            WHERE 
                                tbl_sales.state = 0";
                }

                if (cmbCustomer.Text != "" && !chkCustomer.Checked)
                {
                    query += " AND tbl_sales.customer_id = @id";
                    parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
                }

                if (cmbPaymentMethod.Text != "" && !chkPayment.Checked)
                {
                    query += " AND tbl_sales.payment_method = @payment";
                    parameters.Add(DBClass.CreateParameter("payment", cmbPaymentMethod.Text));
                }

                if (!chkDate.Checked)
                {
                    query += " AND tbl_sales.date BETWEEN @dateFrom AND @dateTo";
                    parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
                }

                if (cmbSelectionMethod.Text == "Default")
                {
                    query += @" GROUP BY 
                        tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, 
                        tbl_sales.payment_method, tbl_sales.total";
                }
                else
                {
                    query += @" GROUP BY 
                        tbl_sales.id, tbl_sales.date, tbl_sales.invoice_id, tbl_customer.code, tbl_customer.name, 
                        tbl_sales.payment_method, tbl_sales.total, tbl_sales.vat, tbl_sales.pay, tbl_sales.change, 
                        ti.code, ti.name, ts.qty, ts.price, ts.vat, ts.total";
                }
            }

            // Execute and bind
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvItems.DataSource = dt;

            // Optional: UI Tweaks
            if (dgvItems.Columns.Contains("Customer Name"))
            {
                dgvItems.Columns["Customer Name"].MinimumWidth = 200;
                dgvItems.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
            if (dgvItems.Columns.Contains("id"))
                dgvItems.Columns["id"].Visible = false;

            // Localize headers
            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvCustomer.Rows.Count == 0)
            //    return;
            //if (dgvCustomer.Columns["JV NO"].Index == e.ColumnIndex)
            //    _mainForm.openChildForm(new MasterTransactionJournal(_mainForm, dgvCustomer.CurrentRow.Cells["JV NO"].Value.ToString(), "Sales"));
            //else
            //{
                //if (cmbSalesType.SelectedIndex == 1)
                //{//Sales Order
                //    _mainForm.openChildForm(new frmAddSalesOrder(_mainForm, this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
                //}
                //else if (cmbSalesType.SelectedIndex == 2)
                //{//Sales Quotation
                //    _mainForm.openChildForm(new frmAddSalesQuotation(_mainForm, this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
                //}
                //else
                //{
                    //_mainForm.openChildForm(new frmAddnvoice(_mainForm, this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
                //}
            //}
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
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            //    bindInvoiceItems();
            //else
            //    dgvItems.DataSource = null;
        }

        private void cmbSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
    }
}
