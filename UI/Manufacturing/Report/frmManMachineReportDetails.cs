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
using Orientation = MigraDoc.DocumentObjectModel.Orientation;

namespace YamyProject
{
    public partial class frmManMachineReportDetails : Form
    {
        int id;
        public frmManMachineReportDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmManMachineReportDetails_Load(object sender, EventArgs e)
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
            try
            {
                dgvSales.Rows.Clear();

                if (dgvSales.Columns.Count == 0)
                {
                    dgvSales.Columns.Add("BatchID", "Batch ID");
                    dgvSales.Columns.Add("BatchDate", "Batch Date");
                    dgvSales.Columns.Add("CostAmount", "Cost Amount");
                    dgvSales.Columns.Add("ProductionAmount", "Production Amount");
                    dgvSales.Columns.Add("TotalHours", "Total Hours");
                    dgvSales.Columns.Add("ProductID", "Product ID");
                    dgvSales.Columns.Add("FinishedProduct", "Finished Product");
                    dgvSales.Columns.Add("WarehouseID", "Warehouse ID");
                    dgvSales.Columns.Add("WarehouseName", "Warehouse Name");
                    dgvSales.Columns.Add("QuantityProduced", "Quantity Produced");
                    dgvSales.Columns.Add("BatchDescription", "Batch Description");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["BatchID"].Visible = false;
                    dgvSales.Columns["ProductID"].Visible = false;
                    dgvSales.Columns["WarehouseID"].Visible = false;

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
                    DBClass.CreateParameter("@matchineID", id)
                };
                string dateFilter = "";

                if (!chkDate.Checked)
                {
                    dateFilter = " AND b.date >= @dateFrom AND b.date <= @dateTo";
                    parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                    parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
                }

                string query = $@"
                                SELECT 
                                    b.id AS `Batch ID`,
                                    b.date AS `Batch Date`,
                                    b.Costamount AS `Cost Amount`,
                                    b.amount AS `Production Amount`,
                                    b.hours AS `Total Hours`,
                                    b.product_id AS `Product ID`,
                                    (SELECT name FROM tbl_items WHERE id = b.product_id) AS `Finished Product`,
                                    b.warehouse_id AS `Warehouse ID`,
                                    (SELECT name FROM tbl_warehouse WHERE id = b.warehouse_id) AS `Warehouse Name`,
                                    b.product_qty AS `Quantity Produced`,
                                    b.description AS `Batch Description`
                                FROM 
                                    tbl_manufacturer_batch b
                                WHERE 
                                    b.fixedassetsID =  @matchineID {dateFilter};";

                using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        int rowIndex = dgvSales.Rows.Add(
                            reader["Batch ID"]?.ToString() ?? "",
                            reader["Batch Date"]?.ToString() ?? "",
                            reader["Cost Amount"]?.ToString() ?? "",
                            reader["Production Amount"]?.ToString() ?? "",
                            reader["Total Hours"]?.ToString() ?? "",
                            reader["Product ID"]?.ToString() ?? "",
                            reader["Finished Product"]?.ToString() ?? "",
                            reader["Warehouse ID"]?.ToString() ?? "",
                            reader["Warehouse Name"]?.ToString() ?? "",
                            reader["Quantity Produced"]?.ToString() ?? "",
                            reader["Batch Description"]?.ToString() ?? ""
                        );
                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
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
            section.PageSetup.Orientation = Orientation.Landscape; // Set to landscape

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
            center.AddFormattedText("Batch Details\n", TextFormat.Bold).Font.Size = 10;
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

            dataTable.AddColumn("2.0cm"); // Batch ID
            dataTable.AddColumn("2.5cm"); // Batch Date
            dataTable.AddColumn("2.5cm"); // Cost Amount
            dataTable.AddColumn("2.5cm"); // Production Amount
            dataTable.AddColumn("2.0cm"); // Total Hours
            dataTable.AddColumn("2.0cm"); // Product ID
            dataTable.AddColumn("3.0cm"); // Finished Product
            dataTable.AddColumn("2.0cm"); // Warehouse ID
            dataTable.AddColumn("3.0cm"); // Warehouse Name
            dataTable.AddColumn("2.5cm"); // Quantity Produced
            dataTable.AddColumn("4.0cm"); // Batch Description

            // Header row
            string[] headers = {
                                    "Batch ID", "Batch Date", "Cost Amount", "Production Amount", "Total Hours",
                                    "Product ID", "Finished Product", "Warehouse ID", "Warehouse Name", "Quantity Produced", "Batch Description"
                                };
            Row header = dataTable.AddRow();
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

                string batchId = row.Cells["BatchID"]?.Value?.ToString() ?? "";
                string batchDate = row.Cells["BatchDate"]?.Value?.ToString() ?? "";
                string costAmount = row.Cells["CostAmount"]?.Value?.ToString() ?? "";
                string productionAmount = row.Cells["ProductionAmount"]?.Value?.ToString() ?? "";
                string totalHours = row.Cells["TotalHours"]?.Value?.ToString() ?? "";
                string productId = row.Cells["ProductID"]?.Value?.ToString() ?? "";
                string finishedProduct = row.Cells["FinishedProduct"]?.Value?.ToString() ?? "";
                string warehouseId = row.Cells["WarehouseID"]?.Value?.ToString() ?? "";
                string warehouseName = row.Cells["WarehouseName"]?.Value?.ToString() ?? "";
                string quantityProduced = row.Cells["QuantityProduced"]?.Value?.ToString() ?? "";
                string batchDescription = row.Cells["BatchDescription"]?.Value?.ToString() ?? "";

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(batchId);
                dataRow.Cells[1].AddParagraph(batchDate);
                dataRow.Cells[2].AddParagraph(costAmount);
                dataRow.Cells[3].AddParagraph(productionAmount);
                dataRow.Cells[4].AddParagraph(totalHours);
                dataRow.Cells[5].AddParagraph(productId);
                dataRow.Cells[6].AddParagraph(finishedProduct);
                dataRow.Cells[7].AddParagraph(warehouseId);
                dataRow.Cells[8].AddParagraph(warehouseName);
                dataRow.Cells[9].AddParagraph(quantityProduced);
                dataRow.Cells[10].AddParagraph(batchDescription);
            }

            // Export
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Save PDF";
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = "BatchDetails.pdf";

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
                saveDialog.FileName = "BatchDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Batch Details";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "K1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Adding Column Headers
                    worksheet.Cells[2, 1] = "Batch ID";
                    worksheet.Cells[2, 2] = "Batch Date";
                    worksheet.Cells[2, 3] = "Cost Amount";
                    worksheet.Cells[2, 4] = "Production Amount";
                    worksheet.Cells[2, 5] = "Total Hours";
                    worksheet.Cells[2, 6] = "Product ID";
                    worksheet.Cells[2, 7] = "Finished Product";
                    worksheet.Cells[2, 8] = "Warehouse ID";
                    worksheet.Cells[2, 9] = "Warehouse Name";
                    worksheet.Cells[2, 10] = "Quantity Produced";
                    worksheet.Cells[2, 11] = "Batch Description";

                    for (int col = 1; col <= 11; col++)
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
                        worksheet.Cells[rowIndex, 2] = row.Cells["BatchDate"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 3] = row.Cells["CostAmount"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 4] = row.Cells["ProductionAmount"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 5] = row.Cells["TotalHours"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 6] = row.Cells["ProductID"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 7] = row.Cells["FinishedProduct"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 8] = row.Cells["WarehouseID"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 9] = row.Cells["WarehouseName"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 10] = row.Cells["QuantityProduced"].Value?.ToString() ?? "";
                        worksheet.Cells[rowIndex, 11] = row.Cells["BatchDescription"].Value?.ToString() ?? "";

                        for (int col = 1; col <= 11; col++)
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

        private void frmManMachineReportDetails_FormClosing(object sender, FormClosingEventArgs e)
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
