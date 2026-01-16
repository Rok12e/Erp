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
    public partial class frmPrepaidExpenseDetails : Form
    {
        int id;
        public frmPrepaidExpenseDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmPrepaidExpenseDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print";
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();
            loadCompany();
            LoadFixedAssetDetails();
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
        private void LoadFixedAssetDetails()
        {
            dgvSales.Rows.Clear();

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "SN");
                dgvSales.Columns.Add("Date", "Date");
                dgvSales.Columns.Add("transactionId", "Transaction ID");
                dgvSales.Columns.Add("AccountCode", "A/C CODE");
                dgvSales.Columns.Add("AccountName", "A/C NAME");
                dgvSales.Columns.Add("Debit", "Debit");
                dgvSales.Columns.Add("Credit", "Credit");
                dgvSales.Columns.Add("Type", "Type");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["transactionId"].Visible = false;

                foreach (DataGridViewColumn col in dgvSales.Columns)
                {
                    col.DefaultCellStyle.Font = new Font("Times New Roman", 8F, FontStyle.Regular);
                }

                dgvSales.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 8F, FontStyle.Bold);
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.DefaultCellStyle.ForeColor = Color.Black;
                dgvSales.DefaultCellStyle.BackColor = Color.White;
                dgvSales.GridColor = Color.LightGray;
                dgvSales.BorderStyle = BorderStyle.None;
                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
                dgvSales.RowHeadersVisible = false;
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("transaction_id", id)
            };

            string dateFilter = "";

            if (!chkDate.Checked)
            {
                dateFilter = " AND t.date BETWEEN @dateFrom AND @dateTo";
                parameters.Add(DBClass.CreateParameter("dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY t.date) AS `SN`,
                                t.date,
                                t.transaction_id,
                                t.type AS `Type`,
                                c.code AS `A/C CODE`,
                                c.name AS `A/C NAME`,
                                t.debit AS `DEBIT`,
                                t.credit AS `CREDIT`
                            FROM 
                                tbl_transaction t
                            INNER JOIN 
                                tbl_coa_level_4 c ON t.account_id = c.id
                            WHERE 
                                t.transaction_id = @transaction_id
                                AND t.type = 'Prepaid Expense'
                                {dateFilter};";

            decimal totalDebit = 0;
            decimal totalCredit = 0;

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    DateTime date = Convert.ToDateTime(reader["date"]);
                    string transactionId = reader["transaction_id"].ToString();
                    string accountCode = reader["A/C CODE"].ToString();
                    string accountName = reader["A/C NAME"].ToString();
                    decimal debit = Convert.ToDecimal(reader["DEBIT"]);
                    decimal credit = Convert.ToDecimal(reader["CREDIT"]);
                    string Type = reader["Type"].ToString();

                    int rowIndex = dgvSales.Rows.Add(
                        reader["SN"].ToString(),
                        date.ToString("MMM dd, yyyy"),
                        transactionId,
                        accountCode,
                        accountName,
                        debit.ToString("N2"),
                        credit.ToString("N2"),
                        Type
                    );

                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new Font("Times New Roman", 8F, FontStyle.Regular);

                    totalDebit += debit;
                    totalCredit += credit;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add("", "", "", "TOTAL", "", totalDebit.ToString("N2"), totalCredit.ToString("N2"));
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new Font("Times New Roman", 8F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = Color.Black;
            dgvSales.Rows[totalRow].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSales.Rows[totalRow].Cells[6].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["transactionId"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                    if (_type.Contains("Prepaid Expense"))
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPrepaidExpense(_id));
                    }
                }
            }
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {

        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "CustomerBalanceDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Customer Balance Details";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "G1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Adding Column Headers
                    worksheet.Cells[2, 1] = "ID";
                    worksheet.Cells[2, 2] = "Type";
                    worksheet.Cells[2, 3] = "Date";
                    worksheet.Cells[2, 4] = "transactionId";
                    worksheet.Cells[2, 5] = "Account";
                    worksheet.Cells[2, 6] = "Amount";
                    worksheet.Cells[2, 7] = "Balance";

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string id = row.Cells["id"].Value?.ToString();
                        string type = row.Cells["Type"].Value?.ToString();
                        string date = row.Cells["Date"].Value?.ToString();
                        string num = row.Cells["transactionId"].Value?.ToString();
                        string account = row.Cells["Account"].Value?.ToString();
                        string amount = row.Cells["Amount"].Value?.ToString();
                        string balance = row.Cells["Balance"].Value?.ToString();

                        worksheet.Cells[rowIndex, 1] = id;
                        worksheet.Cells[rowIndex, 2] = type;
                        worksheet.Cells[rowIndex, 3] = date;
                        worksheet.Cells[rowIndex, 4] = num;
                        worksheet.Cells[rowIndex, 5] = account;
                        worksheet.Cells[rowIndex, 6] = amount;
                        worksheet.Cells[rowIndex, 7] = balance;

                        // Formatting the cells
                        var accountCell = worksheet.Cells[rowIndex, 5];
                        var amountCell = worksheet.Cells[rowIndex, 6];

                        accountCell.Font.Name = "Times New Roman";
                        amountCell.Font.Name = "Times New Roman";

                        // If account contains 'TOTAL', apply special formatting
                        if (account.ToUpper().Contains("TOTAL"))
                        {
                            accountCell.Font.Size = 10;
                            accountCell.Font.Bold = true;

                            amountCell.Font.Size = 10;
                            amountCell.Font.Bold = true;
                            amountCell.Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }
                        else
                        {
                            accountCell.Font.Size = 10;
                            accountCell.Font.Bold = true;

                            amountCell.Font.Size = 9;
                        }

                        amountCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                        rowIndex++;
                    }

                    worksheet.Columns[1].AutoFit();
                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();
                    worksheet.Columns[5].AutoFit();
                    worksheet.Columns[6].AutoFit();
                    worksheet.Columns[7].AutoFit();

                    // Save the workbook and close
                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadFixedAssetDetails();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadFixedAssetDetails();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            LoadFixedAssetDetails();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadFixedAssetDetails();
            }
        }
    }
}
