using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmVendorBalanceDetails : Form
    {
        int id;
        public frmVendorBalanceDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmVendorBalanceDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadSalesData();
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();
            loadCompany();
            LoadSalesData();
        }
        private void loadCompany()
        {
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    guna2HtmlLabel8.Text = reader["name"].ToString();
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));
        }

        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }
        private void LoadSalesData()
        {
            dgvSales.Rows.Clear();

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "id");
                dgvSales.Columns.Add("Type", "Type");
                dgvSales.Columns.Add("Date", "Date");
                dgvSales.Columns.Add("Num", "Num");
                dgvSales.Columns.Add("Account", "Account");
                dgvSales.Columns.Add("Amount", "Amount");
                dgvSales.Columns.Add("Balance", "Balance");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["Account"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSales.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Amount"].DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
                dgvSales.Columns["Account"].DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Regular);

                dgvSales.Columns["id"].Visible = false;
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.DefaultCellStyle.ForeColor = Color.Black;
                dgvSales.DefaultCellStyle.BackColor = Color.White;
                dgvSales.GridColor = Color.LightGray;
                dgvSales.BorderStyle = BorderStyle.None;
                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dgvSales.RowHeadersVisible = false;
            }

            decimal totalAmount = 0, totalBalance = 0;

            List<MySqlParameter> parameters = new List<MySqlParameter>
                {
                    DBClass.CreateParameter("id", id)
                };

            string dateFilter = "";
            if (!chkDate.Checked)
            {
                dateFilter = " AND tbl_transaction.date >= @dateFrom AND tbl_transaction.date <= @dateTo";
                parameters.Add(DBClass.CreateParameter("dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                            SELECT 
                                DATE_FORMAT(tbl_transaction.date, '%M %d %Y') AS date,
                                tbl_transaction.transaction_id,
                                CASE 
                                    WHEN tbl_transaction.type = 'Vendor Payment' THEN 
                                        (SELECT tbl_payment_voucher.code 
                                         FROM tbl_payment_voucher 
                                         WHERE tbl_payment_voucher.state = 0 AND tbl_payment_voucher.id = tbl_transaction.transaction_id)
                                    WHEN tbl_transaction.type = 'Purchase Invoice' OR tbl_transaction.type = 'Purchase Invoice Cash' THEN 
                                        (SELECT tbl_purchase.invoice_id 
                                         FROM tbl_purchase 
                                         WHERE tbl_purchase.state = 0 AND tbl_purchase.id = tbl_transaction.transaction_id)
                                    WHEN tbl_transaction.type = 'Purchase Return Invoice' THEN 
                                        (SELECT tbl_purchase_return.invoice_id 
                                         FROM tbl_purchase_return 
                                         WHERE tbl_purchase_return.state = 0 AND tbl_purchase_return.id = tbl_transaction.transaction_id)
                                    ELSE ''
                                END AS `Num`,
                                tbl_transaction.type AS `Type`,  
                                tbl_coa_level_4.name AS 'A/C NAME',
                                (tbl_transaction.credit - tbl_transaction.debit) AS `AMOUNT`,
                                SUM(tbl_transaction.debit - tbl_transaction.credit) 
                                    OVER (PARTITION BY tbl_transaction.hum_id ORDER BY tbl_transaction.date) AS `BALANCE`
                            FROM 
                                tbl_transaction  
                            INNER JOIN tbl_coa_level_4 
                                ON tbl_transaction.account_id = tbl_coa_level_4.id 
                            WHERE 
                                tbl_transaction.hum_id = @id
                                AND tbl_transaction.type IN (
                                    'Vendor Payment', 
                                    'Purchase Invoice',
                                    'Vendor Opening Balance',
                                    'Check Cancel (Vendor)',
                                    'Purchase Return Invoice', 
                                    'Debit Note',
                                    'PDC Payable'
                                )
                                {dateFilter}
                            ORDER BY tbl_transaction.date;";

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    if (reader["Date"] == DBNull.Value)
                        continue;

                    decimal amount = Convert.ToDecimal(reader["AMOUNT"]);
                    decimal balance = Convert.ToDecimal(reader["BALANCE"]);

                    int rowIndex = dgvSales.Rows.Add(
                        reader["transaction_id"].ToString(),
                        reader["Type"].ToString(),
                        reader["date"].ToString(),
                        reader["Num"].ToString(),
                        reader["A/C NAME"].ToString(),
                        amount.ToString("N2"),
                        balance.ToString("N2") + " ◀"
                    );

                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Regular);

                    totalAmount += amount;
                    totalBalance += balance;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add(null, null, null, null, "TOTAL", totalAmount.ToString("N2"), "");
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = Color.Black;
            dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                    if (_type.Contains("Purchase"))
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchase(_id, "", id));
                    }
                    else if (_type.Contains("Vendor Payment"))
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(id));
                    }
                    else if (_type == "Vendor Opening Balance")
                    {
                        frmLogin.frmMain.openChildForm(new frmTransactionJournal(_id, _type, _id.ToString()));
                    }
                }
            }
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            LoadSalesData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadSalesData();
            }
        }
    }
}
