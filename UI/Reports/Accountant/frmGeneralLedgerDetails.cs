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
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class frmGeneralLedgerDetails : Form
    {
        string name = "";
        int id = 0;
        public frmGeneralLedgerDetails(int _id=0,string name = "")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
            this.name = name;
        }

        private void frmGeneralLedgerDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadData();
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

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "");
                dgvSales.Columns.Add("Type", "Type");
                dgvSales.Columns.Add("Date", "Date");
                dgvSales.Columns.Add("Num", "Num");
                dgvSales.Columns.Add("Description", "Description");
                dgvSales.Columns.Add("Debit", "Debit");
                dgvSales.Columns.Add("Credit", "Credit");
                dgvSales.Columns.Add("Balance", "Balance");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                //dgvSales.Columns["Account"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSales.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Debit"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                dgvSales.Columns["Credit"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                //dgvSales.Columns["Account"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);


                dgvSales.Columns["id"].Visible = false;
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgvSales.GridColor = System.Drawing.Color.LightGray;
                dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                dgvSales.RowHeadersVisible = false;
            }

            //decimal totalAmount = 0, totalDebit = 0, totalCredit = 0;

            //MySqlDataReader readerL = DBClass.ExecuteReader("select id,CONCAT(code,' - ',name) name from tbl_coa_level_4");
            //while (readerL.Read())
            //{
                //int id = int.Parse(readerL["id"].ToString());
                dgvSales.Rows.Add(id, "", "", "", name.ToString(), "", "", "", "");
                decimal lineTotalDebit = 0, lineTotalCredit = 0;
                List<MySqlParameter> parameters = new List<MySqlParameter>
                {
                    DBClass.CreateParameter("id", id)
                };

            string query = @"
                            SELECT 
                            DATE_FORMAT(tbl_transaction.date, '%M %d %Y') AS date,
                            tbl_transaction.transaction_id AS `Num`,
                            tbl_transaction.type AS `Type`,
                            tbl_transaction.t_type, 
                            tbl_transaction.description,
                            tbl_transaction.debit,
                            tbl_transaction.credit,
                            (tbl_transaction.debit - tbl_transaction.credit) AS amount
                            FROM 
                            tbl_transaction  
                            INNER JOIN 
                            tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id 
                            WHERE 
                            tbl_transaction.account_id = @id ";
            if (!chkDate.Checked)
                query += " and tbl_transaction.date >= @dateFrom and tbl_transaction.date <= @dateTo";
            query +=
                " ORDER BY tbl_transaction.date;";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtpFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtpTo.Value.Date));

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
            {
                decimal subTotalDebit = 0, subTotalCredit = 0;

                while (reader.Read())
                {
                    if (reader["date"] == DBNull.Value) continue;

                    decimal debit = Convert.ToDecimal(reader["debit"]);
                    decimal credit = Convert.ToDecimal(reader["credit"]);

                    subTotalDebit += debit;
                    subTotalCredit += credit;

                    int rowIndex = dgvSales.Rows.Add(
                        reader["Num"].ToString(),
                        reader["Type"].ToString(),
                        reader["date"].ToString(),
                        reader["Num"].ToString(),
                        reader["description"].ToString(),
                        debit.ToString("N2"),
                        credit.ToString("N2"),
                        (subTotalDebit - subTotalCredit).ToString("N2")
                    );
                }

                lineTotalDebit += subTotalDebit;
                lineTotalCredit += subTotalCredit;
            }

            int totalRow0 = dgvSales.Rows.Add(null, null, null, null, "TOTAL",
                lineTotalDebit.ToString("N2"),
                lineTotalCredit.ToString("N2"),
                (lineTotalDebit - lineTotalCredit).ToString("N2"));

            dgvSales.Rows[totalRow0].DefaultCellStyle.Font =
                new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow0].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
    }

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                //{
                //    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                //    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                //    if (_type.Contains("Sales"))
                //    {
                //        frmLogin.frmMain.openChildForm(new frmAddSales(_id, "", id));
                //    }
                //    else
                //    {
                //        new frmViewCustomer(id);
                //    }
                //}
            }
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = guna2HtmlLabel8.Text;

            Document doc = new Document();
            Section section = doc.AddSection();

            // Adjust page margins to position content to top-left
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;

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
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell - Company Name & Report Titles
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.Format.SpaceAfter = 0;

            // Company name - Bold, size 10
            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;

            // "Customer Balance Details" - Bold, size 10
            FormattedText summaryText = center.AddFormattedText("General Ledger Details\n", TextFormat.Bold);
            summaryText.Font.Size = 12;

            // "All Transactions" - Regular, size 9
            FormattedText allTransText = center.AddFormattedText("All Transactions", TextFormat.NotBold);
            allTransText.Font.Size = 9;

            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // Bold line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0;
            dataTable.AddColumn("5cm");   // Type Column
            dataTable.AddColumn("3cm");   // Date Column
            dataTable.AddColumn("1cm");   // Num Column
            dataTable.AddColumn("6cm");   // Description Column
            dataTable.AddColumn("4cm");   // Debit Column
            dataTable.AddColumn("4cm");   // Credit Column
            dataTable.AddColumn("4cm");   // Balance Column

            decimal totalDebit = 0, totalCredit = 0;
            decimal totalBalance = 0;

            // Add header row for data table
            Row headerDataRow = dataTable.AddRow();
            headerDataRow.Cells[0].AddParagraph("Type").Format.Alignment = ParagraphAlignment.Center;
            headerDataRow.Cells[1].AddParagraph("Date").Format.Alignment = ParagraphAlignment.Center;
            headerDataRow.Cells[2].AddParagraph("Num").Format.Alignment = ParagraphAlignment.Center;
            headerDataRow.Cells[3].AddParagraph("Description").Format.Alignment = ParagraphAlignment.Center;
            headerDataRow.Cells[4].AddParagraph("Debit").Format.Alignment = ParagraphAlignment.Center;
            headerDataRow.Cells[5].AddParagraph("Credit").Format.Alignment = ParagraphAlignment.Center;
            headerDataRow.Cells[6].AddParagraph("Balance").Format.Alignment = ParagraphAlignment.Center;

            // Loop through DataGridView rows to add data to the table
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string type = row.Cells["Type"].Value?.ToString() ?? "";
                string date = row.Cells["Date"].Value?.ToString() ?? "";
                string num = row.Cells["Num"].Value?.ToString() ?? "";
                string description = row.Cells["Description"].Value?.ToString() ?? "";
                string debitStr = row.Cells["Debit"].Value?.ToString() ?? "0";
                string creditStr = row.Cells["Credit"].Value?.ToString() ?? "0";
                string balance = row.Cells["Balance"].Value?.ToString() ?? "";

                decimal debit = 0, credit = 0;
                decimal.TryParse(debitStr, out debit);
                decimal.TryParse(creditStr, out credit);

                // Calculate cumulative balance (starting with previous balance or 0)
                totalBalance += debit-credit;

                // Add a new row for data
                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(type);
                tRow.Cells[1].AddParagraph(date);
                tRow.Cells[2].AddParagraph(num);
                tRow.Cells[3].AddParagraph(description);
                tRow.Cells[4].AddParagraph(debit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[5].AddParagraph(credit.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[6].AddParagraph(totalBalance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                totalDebit += debit;
                totalCredit += credit;  // Accumulate total debit
            }

            // Render and save the PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "GeneralLedgerDetails.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);


        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Save Excel File",
                FileName = "GeneralLedgerDetails.xlsx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add(Type.Missing);
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "General Ledger Details";

                // Merge and format header row (Date)
                Excel.Range headerRange = worksheet.Range["A1", "F1"];
                headerRange.Merge();
                headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                headerRange.Font.Bold = true;
                headerRange.Font.Name = "Times New Roman";
                headerRange.Font.Size = 10;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Adding Column Headers
                worksheet.Cells[2, 1] = "Type";
                worksheet.Cells[2, 2] = "Date";
                worksheet.Cells[2, 3] = "Num";
                worksheet.Cells[2, 4] = "Description";
                worksheet.Cells[2, 5] = "Debit";
                worksheet.Cells[2, 6] = "Credit";
                worksheet.Cells[2, 7] = "Balance";

                int rowIndex = 3;
                decimal cumulativeBalance = 0;  // This will track the running balance

                foreach (DataGridViewRow row in dgvSales.Rows)
                {
                    if (row.IsNewRow) continue;

                    string type = row.Cells["Type"].Value?.ToString();
                    string date = row.Cells["Date"].Value?.ToString();
                    string num = row.Cells["Num"].Value?.ToString();
                    string description = row.Cells["Description"].Value?.ToString();
                    string debitStr = row.Cells["Debit"].Value?.ToString();
                    string creditStr = row.Cells["Credit"].Value?.ToString();
                    string balance = row.Cells["Balance"].Value?.ToString();

                    // Convert debit to decimal
                    decimal debit = 0, credit = 0;
                    decimal.TryParse(debitStr, out debit);
                    decimal.TryParse(creditStr, out credit);

                    // Update cumulative balance
                    cumulativeBalance += debit - credit;
                    if (description.ToLower() == "total")
                    {
                        cumulativeBalance = debit + credit;
                    }

                    // Fill the Excel sheet with data
                    worksheet.Cells[rowIndex, 1] = type;
                    worksheet.Cells[rowIndex, 2] = date;
                    worksheet.Cells[rowIndex, 3] = num;
                    worksheet.Cells[rowIndex, 4] = description;
                    worksheet.Cells[rowIndex, 5] = debit.ToString("N2");
                    worksheet.Cells[rowIndex, 6] = credit.ToString("N2");
                    worksheet.Cells[rowIndex, 7] = cumulativeBalance.ToString("N2");

                    // Formatting the cells
                    var debitCell = worksheet.Cells[rowIndex, 5];
                    var creditCell = worksheet.Cells[rowIndex, 6];
                    var balanceCell = worksheet.Cells[rowIndex, 7];

                    debitCell.Font.Name = creditCell.Font.Name = balanceCell.Font.Name = "Times New Roman";

                    // If the account contains 'TOTAL', apply special formatting
                    if (description.ToUpper().Contains("TOTAL"))
                    {
                        debitCell.Font.Size = 10;
                        debitCell.Font.Bold = true;
                        creditCell.Font.Size = 10;
                        creditCell.Font.Bold = true;
                        balanceCell.Font.Size = 10;
                        balanceCell.Font.Bold = true;
                        balanceCell.Font.Underline = Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                    }
                    else
                    {
                        debitCell.Font.Size = 9;
                        creditCell.Font.Size = 9;
                        balanceCell.Font.Size = 9;
                    }

                    debitCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    creditCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    balanceCell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;

                    rowIndex++;
                }

                // Auto-fit the columns for readability
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
        public static class SqlFilterHelper
        {
            public static string GetDateFilter(DateTime? from, DateTime? to, List<MySqlParameter> parameters)
            {
                if (from.HasValue && to.HasValue)
                {
                    parameters.Add(DBClass.CreateParameter("from", from.Value.Date));
                    parameters.Add(DBClass.CreateParameter("to", to.Value.Date));
                    return " AND tbl_transaction.date BETWEEN @from AND @to ";
                }
                return "";
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            ApplyDateFilter();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            ApplyDateFilter();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            ApplyDateFilter();

        }
        private void ApplyDateFilter()
        {
            if (chkDate.Checked)
            {
                LoadData(); // No filter
            }
            else
            {
                DateTime from = dtpFrom.Value.Date;
                DateTime to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1); // End of the day
                LoadData();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
            }
        }
    }
}
