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
    public partial class frmWarehouseDetail : Form
    {
        int warehouseId;
        int itemId;
        int id;

        public frmWarehouseDetail(int id = 0,int warehouseId = 0, int _itemId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.warehouseId = warehouseId;
            this.itemId = _itemId;
        }

        private void frmWarehouseDetail_Load(object sender, EventArgs e)
        {
            dgvData.Columns.Clear();

            dgvData.Columns.Add("SN", "SN");
            dgvData.Columns.Add("Date", "Date");
            dgvData.Columns.Add("WarehouseFrom", "Warehouse From");
            dgvData.Columns.Add("WarehouseTo", "Warehouse To");
            dgvData.Columns.Add("ItemName", "Item Name");
            dgvData.Columns.Add("Qty", "Qty");
            dgvData.Columns.Add("Description", "Description");

            dgvData.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvData.Columns["Qty"].DefaultCellStyle.Format = "N2";
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            BindData();
        }

        DataTable data;

        public void BindData()
        {
            bool isLoading = false;
            string query = @"
                            SELECT 
                                wt.date AS 'Date',
                                CONCAT(w1.code, ' - ', w1.name) AS 'Warehouse From',
                                CONCAT(w2.code, ' - ', w2.name) AS 'Warehouse To',
                                CONCAT(i.code, ' - ', i.name) AS 'Item Name',
                                wt.qty,
                                wt.description
                            FROM tbl_item_warehouse_transaction wt
                            INNER JOIN tbl_warehouse w1 ON wt.warehouse_from = w1.id
                            INNER JOIN tbl_warehouse w2 ON wt.warehouse_to = w2.id
                            INNER JOIN tbl_items i ON wt.item_id = i.id
                            WHERE wt.id > 0
                        ";

            var parameters = new List<MySqlParameter>();

            if (id > 0)
            {
                query += " AND wt.id = @id";
                parameters.Add(new MySqlParameter("id", id));
            }

            if (itemId > 0)
            {
                query += " AND i.id = @itemId";
                parameters.Add(new MySqlParameter("itemId", itemId));
            }

            if (chkDate.Checked)
            {
                query += " AND wt.date BETWEEN @dateFrom AND @dateTo";
                parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            }

            if (isLoading) return;
            isLoading = true;

            dgvData.Rows.Clear();

            Task.Run(() =>
            {
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
                        Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd"),
                        reader["Warehouse From"],
                        reader["Warehouse To"],
                        reader["Item Name"],
                        Convert.ToDecimal(reader["qty"]).ToString("N2"),
                        reader["description"]
                    };

                    this.Invoke(new Action(() =>
                    {
                        dgvData.Rows.Add(row);
                    }));
                }

                this.Invoke(new Action(() =>
                {
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvData);

                    // Optional: Format qty column (right align and 2 decimals)
                    int qtyColumnIndex = dgvData.Columns["qty"]?.Index ?? -1;
                    if (qtyColumnIndex != -1)
                    {
                        dgvData.Columns[qtyColumnIndex].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        dgvData.Columns[qtyColumnIndex].DefaultCellStyle.Format = "N2";
                    }

                    isLoading = false;
                }));
            });
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Rows.Count == 0)
                return;
        }

        private void dgvData_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = true;
            else
                dtFrom.Enabled = dtTo.Enabled = false;
            
            BindData();
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
            headerTable.AddColumn("8cm");
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
            centre.AddFormattedText("Warehouse Transfer Report\n", TextFormat.Bold).Font.Size = 12;
            centre.AddFormattedText("Generated Report", TextFormat.NotBold).Font.Size = 9;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // ──────── 3. Data table ────────
            Table table = section.AddTable();
            table.Borders.Width = 0.75;

            // columns: SN | Date | Warehouse From | Warehouse To | Item Name | Qty | Description
            table.AddColumn("1.5cm"); // SN
            table.AddColumn("2.5cm"); // Date
            table.AddColumn("4cm");   // Warehouse From
            table.AddColumn("4cm");   // Warehouse To
            table.AddColumn("5cm");   // Item Name
            table.AddColumn("2.5cm"); // Qty
            table.AddColumn("5cm");   // Description

            // header row
            Row hdr = table.AddRow();
            hdr.Shading.Color = Colors.LightGray;
            hdr.Cells[0].AddParagraph("SN").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[1].AddParagraph("Date").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[2].AddParagraph("Warehouse From").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[3].AddParagraph("Warehouse To").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[4].AddParagraph("Item Name").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[5].AddParagraph("Qty").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[6].AddParagraph("Description").Format.Alignment = ParagraphAlignment.Center;

            // ──────── 4. Fill rows from DataGridView ────────
            int sn = 1;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.IsNewRow) continue;

                Row tRow = table.AddRow();
                tRow.Cells[0].AddParagraph(sn++.ToString());
                tRow.Cells[1].AddParagraph(row.Cells["Date"].Value?.ToString() ?? "");
                tRow.Cells[2].AddParagraph(row.Cells["WarehouseFrom"].Value?.ToString() ?? "");
                tRow.Cells[3].AddParagraph(row.Cells["WarehouseTo"].Value?.ToString() ?? "");
                tRow.Cells[4].AddParagraph(row.Cells["ItemName"].Value?.ToString() ?? "");

                tRow.Cells[5].AddParagraph(
                    string.Format("{0:N2}", row.Cells["Qty"].Value))
                    .Format.Alignment = ParagraphAlignment.Right;

                tRow.Cells[6].AddParagraph(row.Cells["Description"].Value?.ToString() ?? "");

                // Optional: unified font
                for (int i = 0; i < 7; i++)
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
                            Environment.SpecialFolder.Desktop), "WarehouseTransferReport.pdf");
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
                FileName = "WarehouseTransferReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add(Type.Missing);
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "Warehouse Transfers";

                // ─────── 1. Merge and format title row ───────
                Excel.Range headerRange = worksheet.Range["A1", "G1"];
                headerRange.Merge();
                headerRange.Value = "Warehouse Transfer Report - " + DateTime.Now.ToString("dd MMM yyyy");
                headerRange.Font.Bold = true;
                headerRange.Font.Name = "Times New Roman";
                headerRange.Font.Size = 11;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // ─────── 2. Column headers ───────
                string[] headers = { "SN", "Date", "Warehouse From", "Warehouse To", "Item Name", "Qty", "Description" };
                string[] fieldNames = { "SN", "Date", "WarehouseFrom", "WarehouseTo", "ItemName", "Qty", "Description" };

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

                // ─────── 3. Data rows ───────
                int rowIndex = 3;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int col = 0; col < fieldNames.Length; col++)
                    {
                        object value = row.Cells[fieldNames[col]].Value;
                        var cell = worksheet.Cells[rowIndex, col + 1];

                        // Format based on column
                        if (fieldNames[col] == "Qty")
                        {
                            if (decimal.TryParse(value?.ToString(), out decimal qty))
                                cell.Value = qty;
                            else
                                cell.Value = 0;

                            cell.NumberFormat = "#,##0.00";
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        else if (fieldNames[col] == "SN")
                        {
                            cell.Value = value;
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        }
                        else
                        {
                            cell.Value = value;
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }

                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 9;
                    }

                    rowIndex++;
                }

                // ─────── 4. Auto-fit columns ───────
                for (int i = 1; i <= headers.Length; i++)
                {
                    ((Excel.Range)worksheet.Columns[i]).AutoFit();
                }

                // ─────── 5. Save and close ───────
                workbook.SaveAs(saveDialog.FileName);
                workbook.Close();
                excelApp.Quit();

                MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
