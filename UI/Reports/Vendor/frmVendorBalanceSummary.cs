using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmVendorBalanceSummary : Form
    {
        public frmVendorBalanceSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmVendorBalanceSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();

            CmbType.Text = "Vendor";
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
                dgvSales.Columns.Add("id", "");
                dgvSales.Columns.Add("Account", "");
                dgvSales.Columns.Add("Amount", DateTime.Now.Date.ToShortDateString());
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["Account"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvSales.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["Amount"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                dgvSales.Columns["Account"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                dgvSales.Columns["id"].Visible = false;

                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgvSales.GridColor = System.Drawing.Color.LightGray;
                dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dgvSales.RowHeadersVisible = false;
            }

            decimal totalAmount = 0;
            var parameters = new[]
                {
                    DBClass.CreateParameter("startDate", dateTimePicker1.Value.Date),
                    DBClass.CreateParameter("endDate", dateTimePicker2.Value.Date)
                };

            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                                                                        v.id,
                                                                        CAST(NOW() as Date) Date,
                                                                        CONCAT(v.code,' - ',v.name) as 'name',
                                                                        IFNULL(SUM(t.credit - t.debit), 0) AS balance
                                                                    FROM 
                                                                        tbl_vendor v
                                                                    LEFT JOIN 
                                                                        tbl_transaction t ON v.id = t.hum_id
                                                                    where t.state = 0 AND
                                                                                        t.type IN (
                                                                                            'Vendor Payment',
                                                                                            'Purchase Invoice',
                                                                                            'Vendor Opening Balance',
                                                                                            'Check Cancel (Vendor)',
                                                                                            'Purchase Return Invoice',
                                                                                            'Debit Note',
                                                                                            'PDC Payable'
                                                                                        )
                                                                    AND t.date >= @startDate AND t.date <= @endDate
                                                                    GROUP BY 
                                                                        v.id, v.name;", parameters.ToArray()))
            {
                while (reader.Read())
                {
                    if (reader["Date"] == DBNull.Value)
                        continue;

                    string accountName = reader["name"].ToString();
                    decimal amount = Convert.ToDecimal(reader["balance"]);

                    string displayName = $"{accountName.PadRight(30)}";
                    string displayAmount = $" {amount:N2}";

                    int rowIndex = dgvSales.Rows.Add(reader["id"], displayName, amount.ToString("N2") + " ◀");
                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                    totalAmount += amount;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add("0", "TOTAL", totalAmount.ToString("N2"));
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.Columns["id"].Visible = false;
            dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
            frmLogin.frmMain.openChildForm(new frmVendorBalanceDetails(id));
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbType.Text == "All" || CmbType.Text == "Vendor")
            {
                guna2HtmlLabel7.Text = "Vendor Balance Summary";
            }
            else
            {
                guna2HtmlLabel7.Text = CmbType.Text + " Balance Summary";
            }
            LoadSalesData();
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = guna2HtmlLabel8.Text.Trim();

            Document doc = new Document();
            Section section = doc.AddSection();

            // Margins
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Header
            Paragraph header = section.AddParagraph();
            header.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            header.AddFormattedText("Vendor Balance Summary\n", TextFormat.Bold);
            header.AddFormattedText("From " + dateTimePicker1.Value.ToShortDateString() +
                                    " To " + dateTimePicker2.Value.ToShortDateString(), TextFormat.NotBold);
            header.Format.Alignment = ParagraphAlignment.Center;
            header.Format.SpaceAfter = "0.5cm";

            // Table
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 9;

            table.AddColumn("1cm");  // SN
            table.AddColumn("8cm");  // Account
            table.AddColumn("4cm");  // Amount

            Row th = table.AddRow();
            th.Shading.Color = Colors.LightGray;
            th.Format.Font.Bold = true;
            th.Cells[0].AddParagraph("SN");
            th.Cells[1].AddParagraph("Account");
            th.Cells[2].AddParagraph("Amount");

            decimal total = 0;
            int sn = 1;

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string account = row.Cells["Account"].Value?.ToString() ?? "";
                string amountStr = row.Cells["Amount"].Value?.ToString()?.Replace("◀", "").Trim() ?? "0";

                if (account.ToUpper().Contains("TOTAL")) continue;

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);
                total += amount;

                Row r = table.AddRow();
                r.Cells[0].AddParagraph(sn.ToString());
                r.Cells[1].AddParagraph(account);
                r.Cells[2].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                sn++;
            }

            // Total row
            Row totalRow = table.AddRow();
            totalRow.Cells[0].MergeRight = 1;
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;
            totalRow.Cells[2].AddParagraph(total.ToString("N2")).Format.Font.Bold = true;
            totalRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VendorBalanceSummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "Vendor Balance Summary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Vendor Balance Summary";

                    // Merge and format header row (Company + Date Range)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "C1"];
                    headerRange.Merge();
                    headerRange.Value = $"Vendor Balance Summary  ({dateTimePicker1.Value:dd/MM/yyyy} - {dateTimePicker2.Value:dd/MM/yyyy})";
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 11;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers (row 2)
                    worksheet.Cells[2, 1] = "SN";
                    worksheet.Cells[2, 2] = "Account";
                    worksheet.Cells[2, 3] = "Amount";

                    for (int i = 1; i <= 3; i++)
                    {
                        var headerCell = worksheet.Cells[2, i];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    }

                    // Fill data
                    int rowIndex = 3;
                    int sn = 1;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string account = row.Cells["Account"].Value?.ToString() ?? "";
                        string amountStr = row.Cells["Amount"].Value?.ToString()?.Replace("◀", "").Trim() ?? "0";

                        if (account.ToUpper().Contains("TOTAL"))
                        {
                            worksheet.Cells[rowIndex, 1] = "";
                            worksheet.Cells[rowIndex, 2] = "TOTAL";
                            worksheet.Cells[rowIndex, 3] = amountStr;

                            worksheet.Cells[rowIndex, 2].Font.Bold = true;
                            worksheet.Cells[rowIndex, 3].Font.Bold = true;
                            worksheet.Cells[rowIndex, 3].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                            worksheet.Cells[rowIndex, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, 1] = sn;
                            worksheet.Cells[rowIndex, 2] = account;
                            worksheet.Cells[rowIndex, 3] = amountStr;

                            for (int col = 1; col <= 3; col++)
                            {
                                var cell = worksheet.Cells[rowIndex, col];
                                cell.Font.Name = "Times New Roman";
                                cell.Font.Size = 9;
                                if (col == 3) // Right align Amount
                                    cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            }
                            sn++;
                        }

                        rowIndex++;
                    }

                    worksheet.Columns.AutoFit();
                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close(false);
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox3.SelectedItem.ToString(), dateTimePicker1, dateTimePicker2);
                LoadSalesData();
            }
        }
    }
}
