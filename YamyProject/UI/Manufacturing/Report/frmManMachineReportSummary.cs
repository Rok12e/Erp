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
    public partial class frmManMachineReportSummary : Form
    {
        public frmManMachineReportSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void frmManMachineReportSummary_Load(object sender, EventArgs e)
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
                    dgvSales.Columns.Add("name", "Machine Name");
                    dgvSales.Columns.Add("model", "Model");
                    dgvSales.Columns.Add("TotalWorkOrders", "Total Work Orders");
                    dgvSales.Columns.Add("ProductTypesInProgress", "Product Types In Progress");
                    dgvSales.Columns.Add("TotalQuantityPlanned", "Total Quantity Planned");
                    dgvSales.Columns.Add("TotalQuantityProduced", "Total Quantity Produced");
                    dgvSales.Columns.Add("TotalCompletedHours", "Total Completed Hours");
                    dgvSales.Columns.Add("TotalWorkingHours", "Total Working Hours");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["id"].Visible = false;

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

                List<MySqlParameter> parameters = new List<MySqlParameter>();
                string dateFilter = "";

                if (!chkDate.Checked)
                {
                    dateFilter = " AND t.StartTime >= @dateFrom AND t.EndTime <= @dateTo";
                    parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                }

                string query = $@"SELECT 
                                X.id,
                                X.name,
                                X.model,
                                COUNT(DISTINCT b.id) AS `Total Work Orders`,
                                COUNT(DISTINCT b.product_id) AS `Product Types In Progress`,
                                SUM(b.product_qty) AS `Total Quantity Planned`,
                                SUM(CASE WHEN t.Status = 'Done' THEN b.product_qty ELSE 0 END) AS `Total Quantity Produced`,
                                SUM(CASE WHEN t.Status = 'Done' 
                                         THEN TIMESTAMPDIFF(HOUR, t.StartTime, t.EndTime) 
                                         ELSE 0 
                                    END) AS `Total Completed Hours`,
                                SUM(TIMESTAMPDIFF(HOUR, t.StartTime, t.EndTime)) AS `Total Working Hours`
                            FROM (
                                SELECT id, name, model 
                                FROM tbl_fixed_assets a 
                                WHERE a.manufacture = 1 AND a.state =0
                            ) X
                            LEFT JOIN tbl_manufacturer_batch b ON b.fixedassetsID = X.id
                            LEFT JOIN tbl_manufacturer_task t ON t.BatchID = b.id
                            GROUP BY X.id, X.name, X.model
                            {dateFilter}
                            ";

                using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        int rowIndex = dgvSales.Rows.Add(
                            reader["id"]?.ToString() ?? "",
                            reader["name"]?.ToString() ?? "",
                            reader["model"]?.ToString() ?? "",
                            reader["Total Work Orders"]?.ToString() ?? "",
                            reader["Product Types In Progress"]?.ToString() ?? "",
                            reader["Total Quantity Planned"]?.ToString() ?? "",
                            reader["Total Quantity Produced"]?.ToString() ?? "",
                            reader["Total Completed Hours"]?.ToString() ?? "",
                            reader["Total Working Hours"]?.ToString() ?? ""
                        );
                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void SavePDF_Click(object sender, EventArgs e)
        {
            try
            {
                string companyName = "Company Name";

                // Get company name from DB
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                {
                    if (reader.Read())
                    {
                        companyName = reader["name"].ToString();
                    }
                }

                // Create PDF document
                Document doc = new Document();
                Section section = doc.AddSection();
                // Adjust page margins to position content to top-left
                section.PageSetup.TopMargin = Unit.FromCentimeter(1);
                section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
                section.PageSetup.RightMargin = Unit.FromCentimeter(1);
                section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
                section.PageSetup.Orientation = Orientation.Landscape; // Set to landscape

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

                FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
                companyText.Font.Size = 12;
                FormattedText summaryText = center.AddFormattedText("Machine Work Summary\n", TextFormat.Bold);
                summaryText.Font.Size = 12;
                FormattedText allTransText = center.AddFormattedText("All Transactions", TextFormat.NotBold);
                allTransText.Font.Size = 9;

                headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

                // Bold line
                Paragraph line = section.AddParagraph();
                line.Format.Borders.Bottom.Width = 2;
                line.Format.SpaceAfter = "0.5cm";

                // Table for data
                Table dataTable = section.AddTable();
                dataTable.Borders.Width = 0.75;
                dataTable.Format.Font.Name = "Times New Roman";
                dataTable.Format.Font.Size = 9;

                // Add columns
                dataTable.AddColumn("2.0cm");  // ID
                dataTable.AddColumn("3.0cm");  // Machine Name
                dataTable.AddColumn("2.5cm");  // Model
                dataTable.AddColumn("2.5cm");  // Total Work Orders
                dataTable.AddColumn("3.0cm");  // Product Types In Progress
                dataTable.AddColumn("3.0cm");  // Total Quantity Planned
                dataTable.AddColumn("3.0cm");  // Total Quantity Produced
                dataTable.AddColumn("3.0cm");  // Total Completed Hours
                dataTable.AddColumn("3.0cm");  // Total Working Hours

                // Header row
                Row header = dataTable.AddRow();
                string[] headers = {
            "ID", "Machine Name", "Model", "Total Work Orders", "Product Types In Progress",
            "Total Quantity Planned", "Total Quantity Produced", "Total Completed Hours", "Total Working Hours"
        };
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
                    string name = row.Cells["name"]?.Value?.ToString() ?? "";
                    string model = row.Cells["model"]?.Value?.ToString() ?? "";
                    string totalWorkOrders = row.Cells["TotalWorkOrders"]?.Value?.ToString() ?? "";
                    string productTypesInProgress = row.Cells["ProductTypesInProgress"]?.Value?.ToString() ?? "";
                    string totalQuantityPlanned = row.Cells["TotalQuantityPlanned"]?.Value?.ToString() ?? "";
                    string totalQuantityProduced = row.Cells["TotalQuantityProduced"]?.Value?.ToString() ?? "";
                    string totalCompletedHours = row.Cells["TotalCompletedHours"]?.Value?.ToString() ?? "";
                    string totalWorkingHours = row.Cells["TotalWorkingHours"]?.Value?.ToString() ?? "";

                    Row dataRow = dataTable.AddRow();
                    dataRow.Cells[0].AddParagraph(id);
                    dataRow.Cells[1].AddParagraph(name);
                    dataRow.Cells[2].AddParagraph(model);
                    dataRow.Cells[3].AddParagraph(totalWorkOrders);
                    dataRow.Cells[4].AddParagraph(productTypesInProgress);
                    dataRow.Cells[5].AddParagraph(totalQuantityPlanned);
                    dataRow.Cells[6].AddParagraph(totalQuantityProduced);
                    dataRow.Cells[7].AddParagraph(totalCompletedHours);
                    dataRow.Cells[8].AddParagraph(totalWorkingHours);
                }

                // Render and save
                PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                renderer.Document = doc;
                renderer.RenderDocument();

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "MachineWorkSummary.pdf");
                renderer.PdfDocument.Save(filePath);
                Process.Start("explorer.exe", filePath);

                MessageBox.Show("PDF exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while exporting the PDF:\n" + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string companyName = lblCompany.Text;
                Document doc = new Document();
                Section section = doc.AddSection();

                // Set margins and orientation
                section.PageSetup.TopMargin = Unit.FromCentimeter(1);
                section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
                section.PageSetup.RightMargin = Unit.FromCentimeter(1);
                section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
                section.PageSetup.Orientation = Orientation.Landscape; // Set to landscape

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
                center.AddFormattedText("Machine Work Summary\n", TextFormat.Bold).Font.Size = 12;
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
                dataTable.AddColumn("3.0cm");  // Machine Name
                dataTable.AddColumn("2.5cm");  // Model
                dataTable.AddColumn("2.5cm");  // Total Work Orders
                dataTable.AddColumn("3.0cm");  // Product Types In Progress
                dataTable.AddColumn("3.0cm");  // Total Quantity Planned
                dataTable.AddColumn("3.0cm");  // Total Quantity Produced
                dataTable.AddColumn("3.0cm");  // Total Completed Hours
                dataTable.AddColumn("3.0cm");  // Total Working Hours

                // Table header row
                Row pdfHeader = dataTable.AddRow();
                string[] headers = {
            "ID", "Machine Name", "Model", "Total Work Orders", "Product Types In Progress",
            "Total Quantity Planned", "Total Quantity Produced", "Total Completed Hours", "Total Working Hours"
        };
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
                    string name = row.Cells["name"]?.Value?.ToString() ?? "";
                    string model = row.Cells["model"]?.Value?.ToString() ?? "";
                    string totalWorkOrders = row.Cells["TotalWorkOrders"]?.Value?.ToString() ?? "";
                    string productTypesInProgress = row.Cells["ProductTypesInProgress"]?.Value?.ToString() ?? "";
                    string totalQuantityPlanned = row.Cells["TotalQuantityPlanned"]?.Value?.ToString() ?? "";
                    string totalQuantityProduced = row.Cells["TotalQuantityProduced"]?.Value?.ToString() ?? "";
                    string totalCompletedHours = row.Cells["TotalCompletedHours"]?.Value?.ToString() ?? "";
                    string totalWorkingHours = row.Cells["TotalWorkingHours"]?.Value?.ToString() ?? "";

                    Row dataRow = dataTable.AddRow();
                    dataRow.Cells[0].AddParagraph(id);
                    dataRow.Cells[1].AddParagraph(name);
                    dataRow.Cells[2].AddParagraph(model);
                    dataRow.Cells[3].AddParagraph(totalWorkOrders);
                    dataRow.Cells[4].AddParagraph(productTypesInProgress);
                    dataRow.Cells[5].AddParagraph(totalQuantityPlanned);
                    dataRow.Cells[6].AddParagraph(totalQuantityProduced);
                    dataRow.Cells[7].AddParagraph(totalCompletedHours);
                    dataRow.Cells[8].AddParagraph(totalWorkingHours);
                }

                // Save the PDF
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.Title = "Save Machine Work Summary";
                    saveDialog.FileName = "MachineWorkSummary.pdf";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                        renderer.Document = doc;
                        renderer.RenderDocument();
                        renderer.PdfDocument.Save(saveDialog.FileName);
                        Process.Start("explorer.exe", saveDialog.FileName);

                        MessageBox.Show("PDF exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while exporting the PDF:\n" + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    saveDialog.Title = "Save Excel File";
                    saveDialog.FileName = "MachineWorkSummary.xlsx";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var excelApp = new Microsoft.Office.Interop.Excel.Application();
                        excelApp.Visible = false;
                        var workbook = excelApp.Workbooks.Add(Type.Missing);
                        var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                        worksheet.Name = "Machine Work Summary";

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
                        worksheet.Cells[2, 2] = "Machine Name";
                        worksheet.Cells[2, 3] = "Model";
                        worksheet.Cells[2, 4] = "Total Work Orders";
                        worksheet.Cells[2, 5] = "Product Types In Progress";
                        worksheet.Cells[2, 6] = "Total Quantity Planned";
                        worksheet.Cells[2, 7] = "Total Quantity Produced";
                        worksheet.Cells[2, 8] = "Total Completed Hours";
                        worksheet.Cells[2, 9] = "Total Working Hours";

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
                            worksheet.Cells[rowIndex, 2] = row.Cells["name"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 3] = row.Cells["model"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 4] = row.Cells["TotalWorkOrders"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 5] = row.Cells["ProductTypesInProgress"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 6] = row.Cells["TotalQuantityPlanned"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 7] = row.Cells["TotalQuantityProduced"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 8] = row.Cells["TotalCompletedHours"].Value?.ToString() ?? "";
                            worksheet.Cells[rowIndex, 9] = row.Cells["TotalWorkingHours"].Value?.ToString() ?? "";

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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while exporting to Excel:\n" + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSales.CurrentRow.Cells["id"].Value == null || dgvSales.CurrentRow.Cells["id"].Value.ToString() == "")
                return;

            int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
            using (var dlg = new frmManMachineReportDetails(id))
            {
                dlg.ShowDialog(this);
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