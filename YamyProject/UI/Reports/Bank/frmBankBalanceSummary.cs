using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class frmBankBalanceSummary : Form
    {
        public frmBankBalanceSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmBankBalanceSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadData();
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));
        }

        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }
        private void LoadData()
        {
            dgvSales.Rows.Clear();
            dgvSales.Columns.Clear();

            // Set up columns: Bank, Account, Balance
            dgvSales.Columns.Add("id", "");
            dgvSales.Columns.Add("Bank", "Bank");
            dgvSales.Columns.Add("Account", "Account");
            dgvSales.Columns.Add("Balance", "Balance");

            LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

            dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSales.Columns["Balance"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["Account"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["Bank"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

            dgvSales.Columns["id"].Visible = false;

            dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgvSales.EnableHeadersVisualStyles = false;
            dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvSales.GridColor = System.Drawing.Color.LightGray;
            dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvSales.RowHeadersVisible = false;

            decimal totalAmount = 0;

            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                                                SELECT 
                                                    b.name AS Bank,
                                                    bc.account_name,
                                                    SUM(t.debit - t.credit) AS Balance
                                                FROM tbl_transaction t
                                                JOIN tbl_bank_card bc ON t.account_id = bc.account_id
                                                JOIN tbl_bank b ON bc.bank_id = b.id
                                                WHERE t.`type` IN ('PDC Payable','PDC Receivable')
                                                GROUP BY b.name, bc.account_name;
                                            "))
            {
                int rowId = 1;
                while (reader.Read())
                {
                    string bankName = reader["Bank"].ToString();
                    string accountName = reader["account_name"].ToString();
                    decimal balance = reader["Balance"] != DBNull.Value ? Convert.ToDecimal(reader["Balance"]) : 0;

                    int rowIndex = dgvSales.Rows.Add(rowId, bankName, accountName, balance.ToString("N2"));
                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                    totalAmount += balance;
                    rowId++;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add("", "", "TOTAL", totalAmount.ToString("N2"));
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = "Company Name";

            // Get company name from DB
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    companyName = reader["name"].ToString();
                }

            // Create PDF document
            Document doc = new Document();
            Section section = doc.AddSection();
            // Adjust page margins to position content to top-left
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Create a header table with 3 columns
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");  // Time/Date
            headerTable.AddColumn("8cm");  // Title Center
            headerTable.AddColumn("5cm");  // Empty for spacing

            Row headerRow = headerTable.AddRow();

            // Left cell - Time & Date (Top-left aligned, Times New Roman)
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            left.Format.SpaceAfter = 0;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            //left.AddFormattedText("Time: ", TextFormat.Bold);
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            //left.AddFormattedText("Date: ", TextFormat.Bold);
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell - Company Name & Report Titles
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.Format.SpaceAfter = 0;

            // Company name - Bold, size 10
            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;

            // "Customer Balance Summary" - Bold, size 10
            FormattedText summaryText = center.AddFormattedText("Customer Balance Summary\n", TextFormat.Bold);
            summaryText.Font.Size = 12;

            // "All Transactions" - Regular, size 9
            FormattedText allTransText = center.AddFormattedText("All Transactions", TextFormat.NotBold);
            allTransText.Font.Size = 9;

            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;


            //section.Add(headerTable);

            // Bold line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0;
            dataTable.AddColumn("5cm"); // Bank
            dataTable.AddColumn("7cm"); // Account
            dataTable.AddColumn("4cm"); // Balance

            // Add header row
            Row header = dataTable.AddRow();
            header.Cells[0].AddParagraph("Bank").Format.Font.Bold = true;
            header.Cells[1].AddParagraph("Account").Format.Font.Bold = true;
            header.Cells[2].AddParagraph("Balance").Format.Font.Bold = true;

            decimal totalAmount = 0;

            // Load from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string bank = row.Cells["Bank"].Value?.ToString() ?? "";
                string account = row.Cells["Account"].Value?.ToString() ?? "";
                if (account.ToUpper().Contains("TOTAL")) continue;

                string balanceStr = row.Cells["Balance"].Value?.ToString() ?? "0";
                decimal balance = 0;
                decimal.TryParse(balanceStr, out balance);

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(bank);
                tRow.Cells[1].AddParagraph(account);
                tRow.Cells[2].AddParagraph(balance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                totalAmount += balance;
            }

            // TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 1;
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;
            Paragraph totalText = totalRow.Cells[2].AddParagraph(totalAmount.ToString("N2"));
            totalText.Format.Font.Bold = true;
            totalText.Format.Font.Underline = Underline.Single;
            totalRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            //section.Add(dataTable);

            // Render and save
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CustomerBalanceSummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "BankBalanceSummary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Bank Balance";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "D1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers
                    worksheet.Cells[2, 2] = "Bank";
                    worksheet.Cells[2, 3] = "Account";
                    worksheet.Cells[2, 4] = "Balance";
                    for (int i = 2; i <= 4; i++)
                    {
                        var cell = worksheet.Cells[2, i];
                        cell.Font.Bold = true;
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 10;
                        cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string bank = row.Cells["Bank"].Value?.ToString();
                        string account = row.Cells["Account"].Value?.ToString();
                        string balance = row.Cells["Balance"].Value?.ToString();

                        var bankCell = worksheet.Cells[rowIndex, 2];
                        var accountCell = worksheet.Cells[rowIndex, 3];
                        var balanceCell = worksheet.Cells[rowIndex, 4];

                        bankCell.Value = bank;
                        accountCell.Value = account;
                        balanceCell.Value = balance;

                        bankCell.Font.Name = "Times New Roman";
                        accountCell.Font.Name = "Times New Roman";
                        balanceCell.Font.Name = "Times New Roman";

                        if (account != null && account.ToUpper().Contains("TOTAL"))
                        {
                            bankCell.Font.Bold = true;
                            accountCell.Font.Bold = true;
                            balanceCell.Font.Bold = true;

                            bankCell.Font.Size = 10;
                            accountCell.Font.Size = 10;
                            balanceCell.Font.Size = 10;

                            balanceCell.Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }
                        else
                        {
                            bankCell.Font.Size = 10;
                            accountCell.Font.Size = 10;
                            balanceCell.Font.Size = 9;
                        }

                        balanceCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        rowIndex++;
                    }

                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();

                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (Utilities.UserPermissionCheck("CustomerBalanceSummary"))
            //{
                int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
                //frmLogin.frmMain.openChildForm(new frmCustomerBalanceDetails(id));
            //}
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            //LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}