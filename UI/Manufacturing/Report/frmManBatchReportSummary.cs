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
using Orientation = MigraDoc.DocumentObjectModel.Orientation;

namespace YamyProject
{
    public partial class frmManBatchReportSummary : Form
    {
        public frmManBatchReportSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void frmManBatchReportSummary_Load(object sender, EventArgs e)
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
            string companyName = lblCompany.Text;
            Document doc = new Document();
            Section section = doc.AddSection();

            // Set margins and landscape orientation
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = Orientation.Landscape;

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
            center.AddFormattedText("Work Batch Summary\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;

            // Separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Main data table
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.75;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            // Add columns for new structure
            dataTable.AddColumn("2.0cm");  // ID
            dataTable.AddColumn("3.0cm");  // Batch Name
            dataTable.AddColumn("2.5cm");  // Cost Amount
            dataTable.AddColumn("2.5cm");  // Amount
            dataTable.AddColumn("2.0cm");  // Hours
            dataTable.AddColumn("2.5cm");  // Date
            dataTable.AddColumn("3.0cm");  // Product Name
            dataTable.AddColumn("3.0cm");  // Machine Name
            dataTable.AddColumn("4.0cm");  // Description

            // Table header row
            Row pdfHeader = dataTable.AddRow();
            string[] headers = { "ID", "Batch Name", "Cost Amount", "Amount", "Hours", "Date", "Product Name", "Machine Name", "Description" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = pdfHeader.Cells[i];
                cell.AddParagraph(headers[i]);
                cell.Format.Font.Bold = true;
                cell.Format.Font.Size = 9;
                cell.Borders.Bottom.Width = 0.5;
                cell.Format.Alignment = ParagraphAlignment.Left;
            }

            // Data rows
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string id = row.Cells["id"]?.Value?.ToString() ?? "";
                string batchname = row.Cells["batchname"]?.Value?.ToString() ?? "";
                string costamount = row.Cells["costamount"]?.Value?.ToString() ?? "";
                string amount = row.Cells["amount"]?.Value?.ToString() ?? "";
                string hours = row.Cells["hours"]?.Value?.ToString() ?? "";
                string date = row.Cells["date"]?.Value?.ToString() ?? "";
                string productName = row.Cells["ProductName"]?.Value?.ToString() ?? "";
                string machineName = row.Cells["MachineName"]?.Value?.ToString() ?? "";
                string description = row.Cells["Description"]?.Value?.ToString() ?? "";

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(id);
                dataRow.Cells[1].AddParagraph(batchname);
                dataRow.Cells[2].AddParagraph(costamount);
                dataRow.Cells[3].AddParagraph(amount);
                dataRow.Cells[4].AddParagraph(hours);
                dataRow.Cells[5].AddParagraph(date);
                dataRow.Cells[6].AddParagraph(productName);
                dataRow.Cells[7].AddParagraph(machineName);
                dataRow.Cells[8].AddParagraph(description);
            }

            // Save the PDF
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveDialog.Title = "Save Work Batch Summary";
                saveDialog.FileName = "WorkBatchSummary.pdf";

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
        private void LoadData()
        {
            try
            {
                dgvSales.Rows.Clear();

                if (dgvSales.Columns.Count == 0)
                {
                    dgvSales.Columns.Add("id", "ID");
                    dgvSales.Columns.Add("batchname", "Batch Name");
                    dgvSales.Columns.Add("costamount", "Cost Amount");
                    dgvSales.Columns.Add("amount", "Amount");
                    dgvSales.Columns.Add("hours", "Hours");
                    dgvSales.Columns.Add("date", "Date");
                    dgvSales.Columns.Add("ProductName", "Product Name");
                    dgvSales.Columns.Add("MachineName", "Machine Name");
                    dgvSales.Columns.Add("Description", "Description");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["id"].Visible = false;

                    dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    dgvSales.EnableHeadersVisualStyles = false;
                    dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgvSales.GridColor = System.Drawing.Color.LightGray;
                    dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    dgvSales.RowHeadersVisible = false;
                }

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                string dateFilter = "";

                if (!chkDate.Checked)
                {
                    dateFilter = " AND b.date >= @dateFrom AND b.date <= @dateTo";
                    parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                }

                string query = $@"SELECT b.id, b.batchname, b.costamount, b.amount, b.hours, b.date, i.name ProductName, CONCAT(a.code,' - ',a.name) MachineName, b.Description 
                          FROM tbl_manufacturer_batch b, tbl_fixed_assets a, tbl_items i 
                          WHERE b.fixedassetsid = a.id AND i.id = b.product_id and a.manufacture = 1 AND a.state =0 {dateFilter}";

                using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        int rowIndex = dgvSales.Rows.Add(
                            reader["id"]?.ToString() ?? "",
                            reader["batchname"]?.ToString() ?? "",
                            reader["costamount"]?.ToString() ?? "",
                            reader["amount"]?.ToString() ?? "",
                            reader["hours"]?.ToString() ?? "",
                            reader["date"]?.ToString() ?? "",
                            reader["ProductName"]?.ToString() ?? "",
                            reader["MachineName"]?.ToString() ?? "",
                            reader["Description"]?.ToString() ?? ""
                        );
                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log or show error
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
            // Set margins and landscape orientation
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = Orientation.Landscape;

            // Header Table
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("5cm");

            Row headerRow = headerTable.AddRow();

            // Left cell - Time and Date
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell - Company Info
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("Work Batch Summary\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;

            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // Bold line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.5;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            // Add columns
            dataTable.AddColumn("2.0cm");  // ID
            dataTable.AddColumn("3.0cm");  // Batch Name
            dataTable.AddColumn("2.5cm");  // Cost Amount
            dataTable.AddColumn("2.5cm");  // Amount
            dataTable.AddColumn("2.0cm");  // Hours
            dataTable.AddColumn("2.5cm");  // Date
            dataTable.AddColumn("3.0cm");  // Product Name
            dataTable.AddColumn("3.0cm");  // Machine Name
            dataTable.AddColumn("4.0cm");  // Description

            // Header row
            Row header = dataTable.AddRow();
            string[] headers = { "ID", "Batch Name", "Cost Amount", "Amount", "Hours", "Date", "Product Name", "Machine Name", "Description" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = header.Cells[i];
                cell.AddParagraph(headers[i]);
                cell.Format.Font.Bold = true;
                cell.Format.Font.Size = 9;
                cell.Borders.Bottom.Width = 0.5;
                cell.Format.Alignment = ParagraphAlignment.Left;
            }

            // Data rows
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string id = row.Cells["id"]?.Value?.ToString() ?? "";
                string batchname = row.Cells["batchname"]?.Value?.ToString() ?? "";
                string costamount = row.Cells["costamount"]?.Value?.ToString() ?? "";
                string amount = row.Cells["amount"]?.Value?.ToString() ?? "";
                string hours = row.Cells["hours"]?.Value?.ToString() ?? "";
                string date = row.Cells["date"]?.Value?.ToString() ?? "";
                string productName = row.Cells["ProductName"]?.Value?.ToString() ?? "";
                string machineName = row.Cells["MachineName"]?.Value?.ToString() ?? "";
                string description = row.Cells["Description"]?.Value?.ToString() ?? "";

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(id);
                dataRow.Cells[1].AddParagraph(batchname);
                dataRow.Cells[2].AddParagraph(costamount);
                dataRow.Cells[3].AddParagraph(amount);
                dataRow.Cells[4].AddParagraph(hours);
                dataRow.Cells[5].AddParagraph(date);
                dataRow.Cells[6].AddParagraph(productName);
                dataRow.Cells[7].AddParagraph(machineName);
                dataRow.Cells[8].AddParagraph(description);
            }

            // Render and save
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WorkBatchSummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "WorkBatchSummary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Work Batch";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "I1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Column headers
                    worksheet.Cells[2, 1] = "ID";
                    worksheet.Cells[2, 2] = "Batch Name";
                    worksheet.Cells[2, 3] = "Cost Amount";
                    worksheet.Cells[2, 4] = "Amount";
                    worksheet.Cells[2, 5] = "Hours";
                    worksheet.Cells[2, 6] = "Date";
                    worksheet.Cells[2, 7] = "Product Name";
                    worksheet.Cells[2, 8] = "Machine Name";
                    worksheet.Cells[2, 9] = "Description";

                    for (int col = 1; col <= 9; col++)
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

                        worksheet.Cells[rowIndex, 1] = row.Cells["id"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 2] = row.Cells["batchname"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 3] = row.Cells["costamount"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 4] = row.Cells["amount"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 5] = row.Cells["hours"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 6] = row.Cells["date"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 7] = row.Cells["ProductName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 8] = row.Cells["MachineName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 9] = row.Cells["Description"].Value?.ToString() ?? "";

                        for (int col = 1; col <= 9; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 10;
                            cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
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
            using (var dlg = new frmManBatchReportDetails(id))
            {
                dlg.ShowDialog();
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