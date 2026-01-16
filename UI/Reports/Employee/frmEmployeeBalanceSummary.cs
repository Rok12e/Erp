using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmEmployeeBalanceSummary : Form
    {
        public frmEmployeeBalanceSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void frmEmployeeBalanceSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadSalesData();
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
            string companyName = lblCompany.Text;
            Document doc = new Document();
            Section section = doc.AddSection();

            // Set margins
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Header table
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("5cm");

            Row headerRow = headerTable.AddRow();
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("Employee Balance Summary\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;

            //section.Add(headerTable);

            // Separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Main data table
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.75;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            dataTable.AddColumn("1.0cm");  // SN
            dataTable.AddColumn("8.5cm");  // Account Name
            dataTable.AddColumn("3.0cm");  // Credit
            dataTable.AddColumn("3.0cm");  // Debit
            dataTable.AddColumn("3.7cm");    // Balance

            // Table header row
            Row pdfHeader = dataTable.AddRow();
            pdfHeader.Shading.Color = Colors.LightGray;
            pdfHeader.Format.Font.Bold = true;
            pdfHeader.Cells[0].AddParagraph("SN");
            pdfHeader.Cells[1].AddParagraph("Employee");
            pdfHeader.Cells[2].AddParagraph("Credit");
            pdfHeader.Cells[3].AddParagraph("Debit");
            pdfHeader.Cells[4].AddParagraph("Balance");

            decimal totalCredit = 0, totalDebit = 0, totalBalance = 0;

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string account = row.Cells["Account"].Value?.ToString()?.ToUpper() ?? "";
                if (account.Contains("TOTAL")) continue;

                string sn = row.Cells["SN"].Value?.ToString() ?? "";
                string creditStr = row.Cells["Credit"].Value?.ToString() ?? "0";
                string debitStr = row.Cells["Debit"].Value?.ToString() ?? "0";
                string balanceStr = row.Cells["Balance"].Value?.ToString() ?? "0";

                decimal credit = 0, debit = 0, balance = 0;
                decimal.TryParse(creditStr.Replace("◀", ""), out credit);
                decimal.TryParse(debitStr.Replace("◀", ""), out debit);
                decimal.TryParse(balanceStr.Replace("◀", ""), out balance);

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(sn);
                dataRow.Cells[1].AddParagraph(account);
                dataRow.Cells[2].AddParagraph(credit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                dataRow.Cells[3].AddParagraph(debit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                dataRow.Cells[4].AddParagraph(balance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                totalCredit += credit;
                totalDebit += debit;
                totalBalance += balance;
            }
            // TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 1;
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;

            totalRow.Cells[2].AddParagraph(totalCredit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
            totalRow.Cells[2].Format.Font.Bold = true;

            totalRow.Cells[3].AddParagraph(totalDebit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
            totalRow.Cells[3].Format.Font.Bold = true;

            Paragraph totalBalanceText = totalRow.Cells[4].AddParagraph();
            totalBalanceText.Format.Alignment = ParagraphAlignment.Right;
            totalBalanceText.AddFormattedText(totalBalance.ToString("N2"), TextFormat.Bold).Font.Underline = Underline.Single;

            // Add table to section
            //section.Add(dataTable);

            // Save the PDF
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveDialog.Title = "Save Employee Balance Summary";
                saveDialog.FileName = "EmployeeBalanceSummary.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                    renderer.Document = doc;
                    renderer.RenderDocument();
                    renderer.PdfDocument.Save(saveDialog.FileName);
                    Process.Start("explorer.exe", saveDialog.FileName);
                }
            }
        }
        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }
        private void LoadSalesData()
        {
            try
            {
                dgvSales.Rows.Clear();
                string firstDateStr = "";

                object firstDateObj = DBClass.ExecuteScalar("SELECT MIN(`date`) FROM tbl_transaction WHERE `date` IS NOT NULL");
                if (firstDateObj != null && firstDateObj != DBNull.Value)
                {
                    DateTime firstDate = Convert.ToDateTime(firstDateObj);
                    firstDateStr = firstDate.ToString("MMM dd, yy");
                }

                if (dgvSales.Columns.Count == 0)
                {
                    dgvSales.Columns.Add("SN", "SN");
                    dgvSales.Columns.Add("id", "ID");
                    dgvSales.Columns.Add("Account", "Account");
                    dgvSales.Columns.Add("Credit", "Credit");
                    dgvSales.Columns.Add("Debit", "Debit");
                    dgvSales.Columns.Add("Balance", "Balance");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["id"].Visible = false;

                    dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dgvSales.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                    dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
                    dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    dgvSales.EnableHeadersVisualStyles = false;
                    dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgvSales.GridColor = System.Drawing.Color.LightGray;
                    dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    dgvSales.RowHeadersVisible = false;
                }

                decimal totalCredit = 0, totalDebit = 0, totalAmount = 0;

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                string dateFilter = "";

                if (!chkDate.Checked)
                {
                    dateFilter = " AND t.date >= @dateFrom AND t.date <= @dateTo";
                    parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                }

                string query = $@"
                                SELECT 
                                    ROW_NUMBER() OVER (ORDER BY v.id) AS SN,
                                    CURDATE() AS `Date`,
                                    v.id,
                                    CONCAT(v.code, ' - ', v.name) AS name,
                                    IFNULL(SUM(
                                        CASE 
                                            WHEN t.type IN ('Employee Salary', 'Loan Request') THEN t.debit
                                            ELSE 0
                                        END
                                    ), 0) AS Credit,
                                    IFNULL(SUM(
                                        CASE 
                                            WHEN t.type IN ('Employee Salary Payment', 'Employee Loan Payment') THEN t.debit
                                            ELSE 0
                                        END
                                    ), 0) AS Debit,
                                    IFNULL(SUM(
                                        CASE 
                                            WHEN t.type IN ('Employee Salary', 'Loan Request') THEN t.debit
                                            WHEN t.type IN ('Employee Salary Payment', 'Employee Loan Payment') THEN -t.debit
                                            ELSE 0
                                        END
                                    ), 0) AS Balance
                                FROM
                                    tbl_employee v
                                INNER JOIN
                                    tbl_transaction t ON v.id = t.hum_id AND t.state = 0
                                    {dateFilter}
                                WHERE
                                    v.state = 0
                                GROUP BY
                                    v.id, v.code, v.name;
                            ";

                using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        string Sn = reader["SN"].ToString();
                        string Credit = Convert.ToDecimal(reader["Credit"]).ToString("N2");
                        string Debit = Convert.ToDecimal(reader["Debit"]).ToString("N2");
                        string accountName = reader["name"].ToString();
                        decimal amount = Convert.ToDecimal(reader["Balance"]);

                        int rowIndex = dgvSales.Rows.Add(Sn, reader["id"], accountName, Credit, Debit, amount.ToString("N2") + " ◀");
                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                        totalCredit += Convert.ToDecimal(reader["Credit"]);
                        totalDebit += Convert.ToDecimal(reader["Debit"]);
                        totalAmount += amount;
                    }
                }

                int totalRow = dgvSales.Rows.Add("", "", "TOTAL", totalCredit.ToString("N2"), totalDebit.ToString("N2"), totalAmount.ToString("N2") + " ◀");
                dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
                dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;

                dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Rows[totalRow].Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Rows[totalRow].Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Rows[totalRow].Cells[5].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch (Exception ex)
            {
                //
            }
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
            dataTable.AddColumn("10cm");
            dataTable.AddColumn("5cm");

            decimal totalAmount = 0;

            // Load from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string account = row.Cells["Account"].Value?.ToString() ?? "";
                if (account.ToUpper().Contains("TOTAL")) continue;

                string amountStr = row.Cells["Amount"].Value?.ToString() ?? "0";
                account = account.Replace("▶", "").Trim();
                amountStr = amountStr.Replace("◀", "").Trim();

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(account);
                tRow.Cells[1].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                totalAmount += amount;
            }

            // TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;
            Paragraph totalText = totalRow.Cells[1].AddParagraph(totalAmount.ToString("N2"));
            totalText.Format.Font.Bold = true;
            totalText.Format.Font.Underline = Underline.Single;
            totalRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;

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
                saveDialog.FileName = "EmployeeBalance.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Employee Balance";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "F1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Column headers
                    worksheet.Cells[2, 1] = "SN";
                    worksheet.Cells[2, 2] = "Account";
                    worksheet.Cells[2, 3] = "Credit";
                    worksheet.Cells[2, 4] = "Debit";
                    worksheet.Cells[2, 5] = "Balance";

                    for (int col = 1; col <= 5; col++)
                    {
                        var cell = worksheet.Cells[2, col];
                        cell.Font.Bold = true;
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 10;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string sn = row.Cells["SN"].Value?.ToString();
                        string account = row.Cells["Account"].Value?.ToString();
                        string credit = row.Cells["Credit"].Value?.ToString();
                        string debit = row.Cells["Debit"].Value?.ToString();
                        string balance = row.Cells["Balance"].Value?.ToString();

                        worksheet.Cells[rowIndex, 1] = sn;
                        worksheet.Cells[rowIndex, 2] = account?.Replace("▶", "").Replace("◀", "").Trim();
                        worksheet.Cells[rowIndex, 3] = credit?.Replace("◀", "").Trim();
                        worksheet.Cells[rowIndex, 4] = debit?.Replace("◀", "").Trim();
                        worksheet.Cells[rowIndex, 5] = balance?.Replace("◀", "").Trim();

                        for (int col = 1; col <= 5; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 10;

                            if (col >= 3) // Right-align Credit, Debit, Balance
                            {
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            }
                        }

                        if (!string.IsNullOrEmpty(account) && account.ToUpper().Contains("TOTAL"))
                        {
                            worksheet.Range[$"A{rowIndex}:E{rowIndex}"].Font.Bold = true;
                            worksheet.Cells[rowIndex, 5].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }

                        rowIndex++;
                    }

                    worksheet.Columns.AutoFit();
                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSales.CurrentRow.Cells["id"].Value == null || dgvSales.CurrentRow.Cells["id"].Value.ToString() == "")
                return;
            
                int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
                frmLogin.frmMain.openChildForm(new frmEmployeeBalanceDetails(id));
            
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