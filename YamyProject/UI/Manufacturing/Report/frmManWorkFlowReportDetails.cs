using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmManWorkFlowReportDetails : Form
    {
        int id;
        public frmManWorkFlowReportDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmManWorkFlowReportDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadData();
            DateTime dated = DateTime.Now;
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
            loadCompany();
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
                dgvSales.Columns.Add("BatchID", "Batch ID");
                dgvSales.Columns.Add("BatchName", "Batch Name");
                dgvSales.Columns.Add("MachineName", "Machine Name");
                dgvSales.Columns.Add("MachineModel", "Machine Model");
                dgvSales.Columns.Add("ProductID", "Product ID");
                dgvSales.Columns.Add("ProductName", "Product Name");
                dgvSales.Columns.Add("PlannedQty", "Planned Qty");
                dgvSales.Columns.Add("TaskID", "Task ID");
                dgvSales.Columns.Add("TaskStatus", "Task Status");
                dgvSales.Columns.Add("TaskStartTime", "Task Start Time");
                dgvSales.Columns.Add("TaskEndTime", "Task End Time");
                dgvSales.Columns.Add("TaskUser", "Task User");
                dgvSales.Columns.Add("CostAmount", "Cost Amount");
                dgvSales.Columns.Add("Hours", "Hours");
                dgvSales.Columns.Add("Description", "Description");
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

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("@id", id)
            };

            string dateFilter = "";
            if (!chkDate.Checked)
            {
                dateFilter = " AND t.StartTime >= @dateFrom AND t.EndTime <= @dateTo";
                parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                            SELECT
                                b.id AS BatchID,
                                b.batchname AS BatchName,
                                a.name AS MachineName,
                                a.model AS MachineModel,
                                b.product_id AS ProductID,
                                i.name AS ProductName,
                                b.product_qty AS PlannedQty,
                                t.id AS TaskID,
                                t.Status AS TaskStatus,
                                t.StartTime AS TaskStartTime,
                                t.EndTime AS TaskEndTime,
                                t.userID AS TaskUser,
                                b.Costamount AS CostAmount,
                                b.hours AS Hours,
                                b.Description AS Description
                            FROM tbl_manufacturer_batch b
                            LEFT JOIN tbl_fixed_assets a ON b.fixedassetsID = a.id
                            LEFT JOIN tbl_items i ON b.product_id = i.id
                            LEFT JOIN tbl_manufacturer_task t ON t.BatchID = b.id
                            WHERE b.id = @id{dateFilter}
                            ORDER BY t.id;";

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
                        reader["TaskID"]?.ToString() ?? "",
                        reader["TaskStatus"]?.ToString() ?? "",
                        reader["TaskStartTime"]?.ToString() ?? "",
                        reader["TaskEndTime"]?.ToString() ?? "",
                        reader["TaskUser"]?.ToString() ?? "",
                        reader["CostAmount"]?.ToString() ?? "",
                        reader["Hours"]?.ToString() ?? "",
                        reader["Description"]?.ToString() ?? ""
                    );
                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                }
            }
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            // Get company name from DB
            string companyName = "Company Name";
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
            {
                if (reader.Read())
                {
                    companyName = reader["name"].ToString();
                }
            }

            // Create PDF document
            Document doc = new Document();
            Section section = doc.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Header Table (Time/Date + Company Info)
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
            left.AddText("Time: " + DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText("Date: " + DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell - Company Info
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("Work Flow Details\n", TextFormat.Bold).Font.Size = 10;
            center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;

            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // Bold black line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Create table for report data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.5;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            // Add columns
            dataTable.AddColumn("2.0cm"); // ID
            dataTable.AddColumn("3.0cm"); // Batch Name
            dataTable.AddColumn("2.5cm"); // Cost Amount
            dataTable.AddColumn("2.5cm"); // Amount
            dataTable.AddColumn("2.0cm"); // Hours
            dataTable.AddColumn("2.5cm"); // Date
            dataTable.AddColumn("3.0cm"); // Machine Name
            dataTable.AddColumn("3.0cm"); // Description
            dataTable.AddColumn("3.0cm"); // Product Name
            dataTable.AddColumn("2.0cm"); // Product Qty

            // Header row
            Row header = dataTable.AddRow();
            string[] headers = { "ID", "Batch Name", "Cost Amount", "Amount", "Hours", "Date", "Machine Name", "Description", "Product Name", "Product Qty" };
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
                string machineName = row.Cells["MachineName"]?.Value?.ToString() ?? "";
                string description = row.Cells["Description"]?.Value?.ToString() ?? "";
                string productName = row.Cells["ProductName"]?.Value?.ToString() ?? "";
                string productQty = row.Cells["ProductQty"]?.Value?.ToString() ?? "";

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(id);
                dataRow.Cells[1].AddParagraph(batchname);
                dataRow.Cells[2].AddParagraph(costamount);
                dataRow.Cells[3].AddParagraph(amount);
                dataRow.Cells[4].AddParagraph(hours);
                dataRow.Cells[5].AddParagraph(date);
                dataRow.Cells[6].AddParagraph(machineName);
                dataRow.Cells[7].AddParagraph(description);
                dataRow.Cells[8].AddParagraph(productName);
                dataRow.Cells[9].AddParagraph(productQty);
            }

            // Export
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Save PDF";
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = "WorkFlowDetails.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(saveDialog.FileName);
                    Process.Start("explorer.exe", saveDialog.FileName);
                }
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "WorkFlowDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Work Flow Details";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "J1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Adding Column Headers
                    worksheet.Cells[2, 1] = "ID";
                    worksheet.Cells[2, 2] = "Batch Name";
                    worksheet.Cells[2, 3] = "Cost Amount";
                    worksheet.Cells[2, 4] = "Amount";
                    worksheet.Cells[2, 5] = "Hours";
                    worksheet.Cells[2, 6] = "Date";
                    worksheet.Cells[2, 7] = "Machine Name";
                    worksheet.Cells[2, 8] = "Description";
                    worksheet.Cells[2, 9] = "Product Name";
                    worksheet.Cells[2, 10] = "Product Qty";

                    for (int col = 1; col <= 10; col++)
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
                        worksheet.Cells[rowIndex, 7] = row.Cells["MachineName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 8] = row.Cells["Description"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 9] = row.Cells["ProductName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 10] = row.Cells["ProductQty"].Value?.ToString() ?? "";

                        for (int col = 1; col <= 10; col++)
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

        private void frmWorkFlowReportDetails_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            //dgvSales.CellMouseEnter -= dgvSales_CellMouseEnter;
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
