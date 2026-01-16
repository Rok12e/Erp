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
    public partial class frmIncomeByVendorDetails : Form
    {
        int id;
        public frmIncomeByVendorDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmIncomeByVendorDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ";
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();
            loadCompany();
            LoadData();
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
        private void LoadData()
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>
    {
        DBClass.CreateParameter("@CustomerID", id)
    };

            string dateFilter = "";
            if (!chkDate.Checked)
            {
                dateFilter = " AND t.date >= @dateFrom AND t.date <= @dateTo ";
                parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                                SELECT
                                    t.id,
                                    t.transaction_id,
                                    h.name AS 'Customer',
                                    t.type AS 'Type',
                                    t.date AS 'Date',
                                    t.voucher_no AS 'Num',
                                    t.description AS 'Memo',
                                    acc.name AS 'Account',
                                    t.debit AS 'Debit',
                                    t.credit AS 'Credit'
                                FROM
                                    tbl_transaction t
                                JOIN
                                    tbl_vendor h ON t.hum_id = h.id
                                LEFT JOIN
                                    tbl_coa_level_4 acc ON t.account_id = acc.id
                                WHERE
                                    t.type IN ('Purchase Invoice', 'Purchase Invoice Cash', 'Vendor Opening Balance', 'Vendor Payment')
                                    AND h.id = @CustomerID
                                    {dateFilter}
                                ORDER BY
                                    h.name, t.date, t.id;
                            ";

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

            // Add Balance column
            dt.Columns.Add("Balance", typeof(decimal));

            // Calculate running balance
            decimal balance = 0;
            foreach (DataRow row in dt.Rows)
            {
                decimal debit = row["Debit"] != DBNull.Value ? Convert.ToDecimal(row["Debit"]) : 0;
                decimal credit = row["Credit"] != DBNull.Value ? Convert.ToDecimal(row["Credit"]) : 0;
                balance += debit - credit;
                row["Balance"] = balance;
            }

            dgvSales.DataSource = dt;
            dgvSales.Columns["id"].Visible = false;
            dgvSales.DefaultCellStyle.ForeColor = Color.Black;
            dgvSales.DefaultCellStyle.Font = new Font("Times New Roman", 9);

            dgvSales.Columns["Debit"].DefaultCellStyle.Format = "N2";
            dgvSales.Columns["Credit"].DefaultCellStyle.Format = "N2";
            dgvSales.Columns["Balance"].DefaultCellStyle.Format = "N2";
            dgvSales.Columns["Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);
        }


        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Rows[e.RowIndex].Cells["transaction_id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["transaction_id"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                    if (_type.Contains("Purchase Invoice") || _type.Contains("Purchase Invoice Cash") )
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchase(_id));
                    }
                    else if(_type == "Vendor Payment")
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(_id));
                    }
                    else if (_type == "Vendor Opening Balance")
                    {
                        frmLogin.frmMain.openChildForm(new frmTransactionJournal(_id,_type,_id.ToString()));
                    }
                }
            }
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            LoadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadData();
            }
        }
    }
}
