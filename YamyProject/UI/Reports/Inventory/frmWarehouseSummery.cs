using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class frmWarehouseSummary : Form
    {
        public frmWarehouseSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmWarehouseSummery_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateWarehouse(cmbWarehouse);
            
            dgvData.Columns.Clear();
            dgvData.Columns.Add("SN", "SN");
            dgvData.Columns.Add("id", "id");
            dgvData.Columns.Add("WarehouseName", "Warehouse Name");
            dgvData.Columns.Add("ItemId", "Item Id");
            dgvData.Columns.Add("ItemName", "Item Name");
            dgvData.Columns.Add("Qty", "Qty");

            // Align and format decimal columns
            dgvData.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvData.Columns["Qty"].DefaultCellStyle.Format = "N2";
            dgvData.Columns["id"].Visible = dgvData.Columns["ItemId"].Visible = false; // Hide id column
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            BindInvoices();
        }

        DataTable data;

        public void BindInvoices()
        {
            bool isLoading = false;
            string query = @"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY wt.id DESC) AS SN,
                                wt.id,w.Name as `WarehouseName`,
                                i.id AS `ItemId`,CONCAT(i.code,' - ',i.name) AS `ItemName`,
                                wt.qty AS `Qty`
                            FROM tbl_items_warehouse wt INNER JOIN tbl_warehouse w ON wt.warehouse_id = w.id
                            INNER JOIN tbl_items i ON wt.item_id = i.id AND i.state = 0
                        ";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (ChkWarehouse.Checked)
            {
                query += " WHERE wt.warehouse_id = @warehouseId";
                parameters.Add(new MySqlParameter("warehouseId", cmbWarehouse.SelectedValue));
            }

            query += " ORDER BY wt.id DESC";

            if (isLoading) return;
            isLoading = true;

            dgvData.Rows.Clear();

            Task.Run(() =>
            {
                data = new DataTable();

                this.Invoke(new Action(() =>
                {
                    data = DBClass.ExecuteDataTable(query, parameters.ToArray());
                }));

                int rowNumber = 0;
                foreach (DataRow reader in data.Rows)
                {
                    var row = new object[]
                    {
                        ++rowNumber,
                        reader["id"],
                        reader["WarehouseName"],
                        reader["ItemId"],
                        reader["ItemName"],
                        reader["Qty"]
                    };

                    this.Invoke(new Action(() =>
                    {
                        dgvData.Rows.Add(row);
                    }));
                }

                this.Invoke(new Action(() =>
                {
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
                    isLoading = false;
                }));
            });
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Rows.Count == 0)
                return;

            if(e.RowIndex < 0 || e.RowIndex >= dgvData.Rows.Count)
                return;
            if (dgvData.SelectedRows.Count == 0)
                return;
            if (dgvData.SelectedRows[0].Cells["ItemId"].Value == null || string.IsNullOrWhiteSpace(dgvData.SelectedRows[0].Cells["ItemId"].Value.ToString()))
                return;

            frmLogin.frmMain.openChildForm(new frmWarehouseDetail(0, 0, int.Parse(dgvData.SelectedRows[0].Cells["ItemId"].Value.ToString())));
        }

        private void dgvData_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (data == null || data.Rows.Count == 0)
                return;

            string searchText = txtSearch.Text.Trim().ToLower();

            dgvData.Rows.Clear();

            int sn = 0;
            foreach (DataRow row in data.Rows)
            {
                string name = row["ItemName"].ToString().ToLower();

                if (name.Contains(searchText))
                {
                    var newRow = new object[]
                    {
                        ++sn,
                        row["id"],
                        row["WarehouseName"],
                        row["ItemId"],
                        row["ItemName"],
                        Convert.ToDecimal(row["Qty"]).ToString("N2")
                    };

                    dgvData.Rows.Add(newRow);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ──────── 1. Create document & page ────────
            Document doc = new Document();
            Section section = doc.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;

            // ──────── 2. Header (time ‑ title ‑ date) ────────
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("10cm");
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("5cm");

            Row headerRow = headerTable.AddRow();

            // left cell – time/date
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n" +
                         DateTime.Now.ToString("dd/MM/yyyy"));

            // centre cell – title
            Paragraph centre = headerRow.Cells[1].AddParagraph();
            centre.Format.Font.Name = "Times New Roman";
            centre.Format.Alignment = ParagraphAlignment.Center;
            centre.AddFormattedText("Warehouse Item Report\n", TextFormat.Bold).Font.Size = 12;
            centre.AddFormattedText("Generated Report", TextFormat.NotBold).Font.Size = 9;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // ──────── 3. Data table ────────
            Table table = section.AddTable();
            table.Borders.Width = 0.75;

            // columns: SN | Warehouse Name | Item Name | Qty
            table.AddColumn("2cm");   // SN
            table.AddColumn("6cm");   // Warehouse Name
            table.AddColumn("8cm");   // Item Name
            table.AddColumn("3cm");   // Qty

            // header row
            Row hdr = table.AddRow();
            hdr.Shading.Color = Colors.LightGray;
            hdr.Cells[0].AddParagraph("SN").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[1].AddParagraph("Warehouse Name").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[2].AddParagraph("Item Name").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[3].AddParagraph("Qty").Format.Alignment = ParagraphAlignment.Center;

            // ──────── 4. Fill rows from DataGridView ────────
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.IsNewRow) continue;

                Row tRow = table.AddRow();

                // Fill data from your specific columns (skipping hidden id and ItemId columns)
                tRow.Cells[0].AddParagraph(row.Cells["SN"].Value?.ToString() ?? "")
                    .Format.Alignment = ParagraphAlignment.Center;
                tRow.Cells[1].AddParagraph(row.Cells["WarehouseName"].Value?.ToString() ?? "");
                tRow.Cells[2].AddParagraph(row.Cells["ItemName"].Value?.ToString() ?? "");
                tRow.Cells[3].AddParagraph(row.Cells["Qty"].Value?.ToString() ?? "")
                    .Format.Alignment = ParagraphAlignment.Right;

                // optional: unifying font for data cells
                for (int i = 0; i < 4; i++)
                {
                    tRow.Cells[i].Format.Font.Name = "Times New Roman";
                    tRow.Cells[i].Format.Font.Size = 9;
                }
            }

            // ──────── 5. Render and open PDF ────────
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string path = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop), "WarehouseItemReport.pdf");
            renderer.PdfDocument.Save(path);
            Process.Start("explorer.exe", path);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "WarehouseItemReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add(Type.Missing);
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "Warehouse Items";

                // Merge and format header row (Report Title) - Updated range for 4 columns
                Excel.Range headerRange = worksheet.Range["A1", "D1"];
                headerRange.Merge();
                headerRange.Value = "Warehouse Item Report - " + DateTime.Now.ToString("dd MMM yyyy");
                headerRange.Font.Bold = true;
                headerRange.Font.Name = "Times New Roman";
                headerRange.Font.Size = 11;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Define headers and field mapping for your columns (excluding hidden id and ItemId)
                string[] headers = { "SN", "Warehouse Name", "Item Name", "Qty" };
                string[] fieldNames = { "SN", "WarehouseName", "ItemName", "Qty" };

                // Write column headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[2, i + 1] = headers[i];
                    var cell = worksheet.Cells[2, i + 1];
                    cell.Font.Bold = true;
                    cell.Font.Name = "Times New Roman";
                    cell.Font.Size = 10;
                    cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Write data rows
                int rowIndex = 3;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int col = 0; col < fieldNames.Length; col++)
                    {
                        object value = row.Cells[fieldNames[col]].Value;
                        worksheet.Cells[rowIndex, col + 1] = value;
                        var cell = worksheet.Cells[rowIndex, col + 1];
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 9;

                        if (fieldNames[col] == "Qty")
                        {
                            // Format quantity as number (assuming it could be decimal)
                            if (decimal.TryParse(value?.ToString(), out decimal qty))
                                cell.Value = qty;
                            cell.NumberFormat = "#,##0.00";
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        else if (fieldNames[col] == "SN")
                        {
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        }
                        else
                        {
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                    }
                    rowIndex++;
                }

                // Auto-fit all columns
                for (int i = 1; i <= headers.Length; i++)
                {
                    ((Excel.Range)worksheet.Columns[i]).AutoFit();
                }

                // Save the file
                workbook.SaveAs(saveDialog.FileName);
                workbook.Close();
                excelApp.Quit();

                MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ChkWarehouse_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkWarehouse.Checked)
                cmbWarehouse.Enabled = true;
            else
                cmbWarehouse.Enabled = false;

            BindInvoices();
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
    }
}
