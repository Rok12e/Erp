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
using YamyProject.RMS.Class;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;

namespace YamyProject
{
    public partial class frmManWorkFlowReportSummary : Form
    {
        public frmManWorkFlowReportSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void frmManWorkFlowReportSummary_Load(object sender, EventArgs e)
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

            // Set margins
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = Orientation.Portrait;

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
            center.AddFormattedText("Work Flow Report Summary\n", TextFormat.Bold).Font.Size = 12;
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
            dataTable.AddColumn("2.0cm");  // Batch ID
            dataTable.AddColumn("3.0cm");  // Batch Name
            dataTable.AddColumn("3.0cm");  // Machine Name
            dataTable.AddColumn("2.5cm");  // Machine Model
            dataTable.AddColumn("2.0cm");  // Product ID
            dataTable.AddColumn("3.0cm");  // Product Name
            dataTable.AddColumn("2.0cm");  // Planned Qty
            dataTable.AddColumn("2.0cm");  // Total Tasks
            dataTable.AddColumn("2.5cm");  // Start Date
            dataTable.AddColumn("2.5cm");  // End Date
            dataTable.AddColumn("2.0cm");  // Total Hours
            dataTable.AddColumn("2.5cm");  // Cost Amount

            // Table header row
            Row pdfHeader = dataTable.AddRow();
            string[] headers = {
                "Batch ID", "Batch Name", "Machine Name", "Machine Model", "Product ID", "Product Name",
                "Planned Qty", "Total Tasks", "Start Date", "End Date", "Total Hours", "Cost Amount"
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

                string batchId = row.Cells["BatchID"]?.Value?.ToString() ?? "";
                string batchName = row.Cells["BatchName"]?.Value?.ToString() ?? "";
                string machineName = row.Cells["MachineName"]?.Value?.ToString() ?? "";
                string machineModel = row.Cells["MachineModel"]?.Value?.ToString() ?? "";
                string productId = row.Cells["ProductID"]?.Value?.ToString() ?? "";
                string productName = row.Cells["ProductName"]?.Value?.ToString() ?? "";
                string plannedQty = row.Cells["PlannedQty"]?.Value?.ToString() ?? "";
                string totalTasks = row.Cells["TotalTasks"]?.Value?.ToString() ?? "";
                string startDate = row.Cells["StartDate"]?.Value?.ToString() ?? "";
                string endDate = row.Cells["EndDate"]?.Value?.ToString() ?? "";
                string totalHours = row.Cells["TotalHours"]?.Value?.ToString() ?? "";
                string costAmount = row.Cells["CostAmount"]?.Value?.ToString() ?? "";

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(batchId);
                dataRow.Cells[1].AddParagraph(batchName);
                dataRow.Cells[2].AddParagraph(machineName);
                dataRow.Cells[3].AddParagraph(machineModel);
                dataRow.Cells[4].AddParagraph(productId);
                dataRow.Cells[5].AddParagraph(productName);
                dataRow.Cells[6].AddParagraph(plannedQty);
                dataRow.Cells[7].AddParagraph(totalTasks);
                dataRow.Cells[8].AddParagraph(startDate);
                dataRow.Cells[9].AddParagraph(endDate);
                dataRow.Cells[10].AddParagraph(totalHours);
                dataRow.Cells[11].AddParagraph(costAmount);
            }

            // Save the PDF
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveDialog.Title = "Save Work Flow Report Summary";
                saveDialog.FileName = "WorkFlowReportSummary.pdf";

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
                    dgvSales.Columns.Add("BatchID", "Batch ID");
                    dgvSales.Columns.Add("BatchName", "Batch Name");
                    dgvSales.Columns.Add("MachineName", "Machine Name");
                    dgvSales.Columns.Add("MachineModel", "Machine Model");
                    dgvSales.Columns.Add("ProductID", "Product ID");
                    dgvSales.Columns.Add("ProductName", "Product Name");
                    dgvSales.Columns.Add("PlannedQty", "Planned Qty");
                    dgvSales.Columns.Add("TotalTasks", "Total Tasks");
                    dgvSales.Columns.Add("StartDate", "Start Date");
                    dgvSales.Columns.Add("EndDate", "End Date");
                    dgvSales.Columns.Add("TotalHours", "Total Hours");
                    dgvSales.Columns.Add("CostAmount", "Cost Amount");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["BatchID"].Visible = false;
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
                    dateFilter = " AND b.date >= @dateFrom AND b.date <= @dateTo";
                    parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                }

                string query = $@"SELECT
                                      b.id AS BatchID,
                                      b.batchname AS BatchName,
                                      i.name AS ProductName,
                                      b.product_id AS ProductID,
                                      b.product_qty AS PlannedQty,
                                      a.name AS MachineName,
                                      a.model AS MachineModel,
                                      MIN(t.StartTime) AS StartDate,
                                      MAX(t.EndTime) AS EndDate,
                                      COUNT(t.id) AS TotalTasks,
                                      b.hours AS TotalHours,
                                      b.Costamount AS CostAmount,
                                      CASE 
                                        WHEN t.Status = 'Cancel' THEN 'Canceled'
                                        WHEN t.Status = 'Done' THEN 'Completed'
                                        ELSE 'Pending'
                                      END AS Status
                                    FROM tbl_manufacturer_batch b
                                    LEFT JOIN tbl_items i ON b.product_id = i.id
                                    LEFT JOIN tbl_fixed_assets a ON b.fixedassetsID = a.id
                                    LEFT JOIN tbl_manufacturer_task t ON t.BatchID = b.id
                                    {dateFilter}
                                    GROUP BY
                                      b.id, b.batchname, i.name, b.product_id, b.product_qty,
                                      a.name, a.model, b.hours, b.Costamount, t.Status
                                    ORDER BY b.id DESC;
                                    ";

                using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        int rowIndex = dgvSales.Rows.Add(
                            reader["BatchID"]?.ToString() ?? "",
                            reader["BatchName"]?.ToString() ?? "",
                            reader["MachineName"]?.ToString() ?? "",
                            reader["MachineModel"]?.ToString() ?? "",
                            reader["ProductID"]?.ToString() ?? "",
                            reader["ProductName"]?.ToString() ?? "",
                            reader["PlannedQty"]?.ToString() ?? "",
                            reader["TotalTasks"]?.ToString() ?? "",
                            reader["StartDate"]?.ToString() ?? "",
                            reader["EndDate"]?.ToString() ?? "",
                            reader["TotalHours"]?.ToString() ?? "",
                            reader["CostAmount"]?.ToString() ?? ""
                        );
                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
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
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = Orientation.Portrait;

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
            center.AddFormattedText("Work Flow Report Summary\n", TextFormat.Bold).Font.Size = 12;
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

            // Add columns for new structure
            dataTable.AddColumn("2.0cm");  // Batch ID
            dataTable.AddColumn("3.0cm");  // Batch Name
            dataTable.AddColumn("3.0cm");  // Machine Name
            dataTable.AddColumn("2.5cm");  // Machine Model
            dataTable.AddColumn("2.0cm");  // Product ID
            dataTable.AddColumn("3.0cm");  // Product Name
            dataTable.AddColumn("2.0cm");  // Planned Qty
            dataTable.AddColumn("2.0cm");  // Total Tasks
            dataTable.AddColumn("2.5cm");  // Start Date
            dataTable.AddColumn("2.5cm");  // End Date
            dataTable.AddColumn("2.0cm");  // Total Hours
            dataTable.AddColumn("2.5cm");  // Cost Amount

            // Table header row
            Row pdfHeader = dataTable.AddRow();
            string[] headers = {
                "Batch ID", "Batch Name", "Machine Name", "Machine Model", "Product ID", "Product Name",
                "Planned Qty", "Total Tasks", "Start Date", "End Date", "Total Hours", "Cost Amount"
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

                string batchId = row.Cells["BatchID"]?.Value?.ToString() ?? "";
                string batchName = row.Cells["BatchName"]?.Value?.ToString() ?? "";
                string machineName = row.Cells["MachineName"]?.Value?.ToString() ?? "";
                string machineModel = row.Cells["MachineModel"]?.Value?.ToString() ?? "";
                string productId = row.Cells["ProductID"]?.Value?.ToString() ?? "";
                string productName = row.Cells["ProductName"]?.Value?.ToString() ?? "";
                string plannedQty = row.Cells["PlannedQty"]?.Value?.ToString() ?? "";
                string totalTasks = row.Cells["TotalTasks"]?.Value?.ToString() ?? "";
                string startDate = row.Cells["StartDate"]?.Value?.ToString() ?? "";
                string endDate = row.Cells["EndDate"]?.Value?.ToString() ?? "";
                string totalHours = row.Cells["TotalHours"]?.Value?.ToString() ?? "";
                string costAmount = row.Cells["CostAmount"]?.Value?.ToString() ?? "";

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(batchId);
                dataRow.Cells[1].AddParagraph(batchName);
                dataRow.Cells[2].AddParagraph(machineName);
                dataRow.Cells[3].AddParagraph(machineModel);
                dataRow.Cells[4].AddParagraph(productId);
                dataRow.Cells[5].AddParagraph(productName);
                dataRow.Cells[6].AddParagraph(plannedQty);
                dataRow.Cells[7].AddParagraph(totalTasks);
                dataRow.Cells[8].AddParagraph(startDate);
                dataRow.Cells[9].AddParagraph(endDate);
                dataRow.Cells[10].AddParagraph(totalHours);
                dataRow.Cells[11].AddParagraph(costAmount);
            }

            // Render and save
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "WorkFlowReportSummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "WorkFlowReportSummary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Work Flow Report";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "L1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Column headers
                    worksheet.Cells[2, 1] = "Batch ID";
                    worksheet.Cells[2, 2] = "Batch Name";
                    worksheet.Cells[2, 3] = "Machine Name";
                    worksheet.Cells[2, 4] = "Machine Model";
                    worksheet.Cells[2, 5] = "Product ID";
                    worksheet.Cells[2, 6] = "Product Name";
                    worksheet.Cells[2, 7] = "Planned Qty";
                    worksheet.Cells[2, 8] = "Total Tasks";
                    worksheet.Cells[2, 9] = "Start Date";
                    worksheet.Cells[2, 10] = "End Date";
                    worksheet.Cells[2, 11] = "Total Hours";
                    worksheet.Cells[2, 12] = "Cost Amount";

                    for (int col = 1; col <= 12; col++)
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

                        worksheet.Cells[rowIndex, 1] = row.Cells["BatchID"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 2] = row.Cells["BatchName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 3] = row.Cells["MachineName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 4] = row.Cells["MachineModel"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 5] = row.Cells["ProductID"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 6] = row.Cells["ProductName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 7] = row.Cells["PlannedQty"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 8] = row.Cells["TotalTasks"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 9] = row.Cells["StartDate"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 10] = row.Cells["EndDate"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 11] = row.Cells["TotalHours"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 12] = row.Cells["CostAmount"].Value?.ToString() ?? "";

                        for (int col = 1; col <= 12; col++)
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
            if (dgvSales.CurrentRow.Cells["BatchID"].Value == null || dgvSales.CurrentRow.Cells["BatchID"].Value.ToString() == "")
                return;

            int id = int.Parse(dgvSales.CurrentRow.Cells["BatchID"].Value.ToString());

            var frm = new frmManWorkFlowReportDetails(id);
            RMSClass.blurbackground3(frm);
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