using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmSalesByItemSummary : Form
    {
        public frmSalesByItemSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmSalesByItemSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
            AddDGVColumns();
            LoadItemData();
        }
        private void AddDGVColumns()
        {
            dgvSales.Columns.Clear();

            dgvSales.Columns.Add("state", "");        // e = expandable, c = collapsed
            dgvSales.Columns.Add("loadState", "");
            dgvSales.Columns.Add("currLvl", "");
            dgvSales.Columns.Add("colName", "");
            dgvSales.Columns.Add("qty", "Qty");
            dgvSales.Columns.Add("amount", "Amount");
            dgvSales.Columns.Add("percent", "% of Sales");
            dgvSales.Columns.Add("avgPrice", "Avg Price");
            dgvSales.Columns.Add("cogs", "COGS");
            dgvSales.Columns.Add("avgCogs", "Avg COGS");
            dgvSales.Columns.Add("grossMargin", "Gross Margin");
            dgvSales.Columns.Add("grossMarginPercent", "Margin %");
            dgvSales.Columns.Add("id", "ID");           // category_id or item_id 
            LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

            dgvSales.Columns["state"].Visible = false;
            dgvSales.Columns["loadState"].Visible = false;
            dgvSales.Columns["currLvl"].Visible = false;
            dgvSales.Columns["id"].Visible = false;

            dgvSales.Columns["colName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvSales.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            dgvSales.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            dgvSales.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;

        }

        private void LoadItemData()
        {
            dgvSales.Rows.Clear();
            string query = "SELECT DISTINCT type FROM tbl_items ORDER BY type";
            using (var reader = DBClass.ExecuteReader(query))
            {
                while (reader.Read())
                {
                    //int catId = Convert.ToInt32(reader["id"]);
                    string type = reader["type"].ToString();
                    dgvSales.Rows.Add("e", "u", "1", "   ►   " + type, "", "", "", "", "", "", "", "", type.ToString());
                }
            }
        }
        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int currLvl = int.Parse(dgvSales.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            string state = dgvSales.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvSales.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvSales.Rows[e.RowIndex].Cells["colName"].Value.ToString();
            string key = dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);

            if (state == "e")
            {
                if (loadState == "u")
                {
                    if (currLvl == 1)
                    {
                        string sql = @"
                                        SELECT 
                                            c.id, 
                                            CONCAT(c.code, ' - ', c.name) AS category_name
                                        FROM 
                                            tbl_item_category c
                                        JOIN 
                                            tbl_items i ON i.category_id = c.id
                                        WHERE 
                                            i.type = @type
                                        GROUP BY 
                                            c.id, c.code, c.name
                                        ORDER BY 
                                            c.code;
                                        ";

                        DataTable categoryTable = DBClass.ExecuteDataTable(sql, DBClass.CreateParameter("@type", key));
                        int insertIndex = e.RowIndex + 1;

                        foreach (DataRow cat in categoryTable.Rows)
                        {
                            string catName = cat["category_name"].ToString();
                            string catId = cat["id"].ToString();
                            dgvSales.Rows.Insert(insertIndex, "e", "u", "2", space + "► " + catName, "", "", "", "", "", "", "", "", catId + "|" + key); // use id + type
                            insertIndex++;
                        }
                    }
                    else if (currLvl == 2)
                    {
                        // 🔽 Expand: category → items
                        string[] split = key.Split('|');
                        int catId = int.Parse(split[0]);
                        string itemType = split[1];

                        string sql = @"
                                        SELECT 
                                            i.id,
                                            i.name AS item_name,
                                            SUM(sd.qty) AS qty,
                                            SUM(sd.total) AS amount,
                                            ROUND((SUM(sd.total) / (
                                                SELECT SUM(total) FROM tbl_sales_details WHERE sales_id IN (SELECT id FROM tbl_sales WHERE state = 0 AND date >= @startDate AND date<= @endDate)
                                            )) * 100, 1) AS percent_of_sales,
                                            ROUND(SUM(sd.total) / SUM(sd.qty), 2) AS avg_price,
                                            ROUND(SUM(sd.cost_price * sd.qty), 2) AS cogs,
                                            ROUND(SUM(sd.cost_price * sd.qty) / SUM(sd.qty), 2) AS avg_cogs,
                                            ROUND(SUM(sd.total) - SUM(sd.cost_price * sd.qty), 2) AS gross_margin,
                                            ROUND((SUM(sd.total) - SUM(sd.cost_price * sd.qty)) / SUM(sd.total) * 100, 1) AS gross_margin_percent
                                        FROM 
                                            tbl_sales_details sd
                                        JOIN 
                                            tbl_items i ON sd.item_id = i.id
                                        WHERE 
                                            i.category_id = @catId AND i.type = @type
                                            AND sd.sales_id IN (SELECT id FROM tbl_sales WHERE state = 0 AND date >= @startDate AND date<= @endDate)
                                        GROUP BY 
                                            i.id, i.name
                                        ORDER BY 
                                            i.name";

                        DataTable items = DBClass.ExecuteDataTable(sql,
                            DBClass.CreateParameter("@catId", catId),
                            DBClass.CreateParameter("@type", itemType),
                            DBClass.CreateParameter("startDate", dtpFrom.Value.Date), 
                            DBClass.CreateParameter("endDate", dtpTo.Value.Date)
                        );

                        int insertIndex = e.RowIndex + 1;
                        foreach (DataRow row in items.Rows)
                        {
                            dgvSales.Rows.Insert(insertIndex, "n", "l", "3", space + "      " + row["item_name"], row["qty"], row["amount"],
                                row["percent_of_sales"] + "%", row["avg_price"], row["cogs"], row["avg_cogs"],
                                row["gross_margin"], row["gross_margin_percent"] + "%", row["id"].ToString(), row["id"]);
                            insertIndex++;
                        }
                    }

                    dgvSales.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                }
                else
                {
                    ToggleSalesChildren(e.RowIndex, true);
                }

                dgvSales.Rows[e.RowIndex].Cells["colName"].Value = cellValue.Replace("►", "▼");
                dgvSales.Rows[e.RowIndex].Cells["state"].Value = "c";
            }
            else if (state == "c")
            {
                ToggleSalesChildren(e.RowIndex, false);
                dgvSales.Rows[e.RowIndex].Cells["colName"].Value = cellValue.Replace("▼", "►");
                dgvSales.Rows[e.RowIndex].Cells["state"].Value = "e";
            }
        }

        private void ToggleSalesChildren(int parentRowIndex, bool visible)
        {
            int parentLevel = int.Parse(dgvSales.Rows[parentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = parentRowIndex + 1; i < dgvSales.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvSales.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    dgvSales.Rows[i].Visible = visible;

                    if (!visible)
                    {
                        // When collapsing, also reset arrow for children
                        dgvSales.Rows[i].Cells["state"].Value = "e";
                        dgvSales.Rows[i].Cells["loadState"].Value = "l"; // keep as loaded
                        dgvSales.Rows[i].Cells["colName"].Value = dgvSales.Rows[i].Cells["colName"].Value.ToString().Replace("▼", "►");
                    }
                }
                else
                {
                    // Stop once we hit a sibling or parent's sibling
                    break;
                }
            }
        }
        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            int currLvl = int.Parse(dgvSales.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            if (currLvl != 3)
                return;
            int itemId;
            if (int.TryParse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString(),out itemId))
            {
                frmLogin.frmMain.openChildForm(new frmSalesByItemDetails(itemId));
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ExpandAllSalesItems();
            string companyName = "Company Name";

            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    companyName = reader["name"].ToString();
                }

            // Create PDF document
            Document doc = new Document();
            Section section = doc.AddSection();

            // Page margins
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Header layout
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("5cm");

            Row headerRow = headerTable.AddRow();

            // Left side: Date/Time
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center: Company name + Title
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;

            FormattedText titleText = center.AddFormattedText("Sales By Item Summery\n", TextFormat.Bold);
            titleText.Font.Size = 12;

            FormattedText subtitleText = center.AddFormattedText("All Transaction", TextFormat.NotBold);
            subtitleText.Font.Size = 9;

            // Bold separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Main table
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 9;

            table.AddColumn("5cm");  // Item Name (full width)
            table.AddColumn("1.5cm");  // Qty
            table.AddColumn("2.5cm"); // Amount
            table.AddColumn("2.0cm");  // % of Sales
            table.AddColumn("1.8cm"); // Avg Price
            table.AddColumn("1.5cm"); // COGS
            table.AddColumn("1.5cm"); // Avg COGS
            table.AddColumn("1.5cm"); // Gross Margin
            table.AddColumn("1.8cm");  // Margin %

            Row header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Format.Font.Bold = true;

            header.Cells[0].AddParagraph("Item");
            header.Cells[1].AddParagraph("Qty");
            header.Cells[2].AddParagraph("Amount");
            header.Cells[3].AddParagraph("% of Sales");
            header.Cells[4].AddParagraph("Avg Price");
            header.Cells[5].AddParagraph("COGS");
            header.Cells[6].AddParagraph("Avg COGS");
            header.Cells[7].AddParagraph("Gross Margin");
            header.Cells[8].AddParagraph("Margin %");

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow || !row.Visible) continue;

                Row tRow = table.AddRow();
                string itemName = row.Cells["colName"].Value?.ToString()?.Replace("►", "").Replace("▼", "").Trim() ?? "";
                tRow.Cells[0].AddParagraph(itemName);
                tRow.Cells[1].AddParagraph(row.Cells["qty"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[2].AddParagraph(row.Cells["amount"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[3].AddParagraph(row.Cells["percent"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[4].AddParagraph(row.Cells["avgPrice"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[5].AddParagraph(row.Cells["cogs"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[6].AddParagraph(row.Cells["avgCogs"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[7].AddParagraph(row.Cells["grossMargin"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[8].AddParagraph(row.Cells["grossMarginPercent"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
            }

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFile.Title = "Save Sales By Item Summary";
                saveFile.FileName = "SalesByItemSummary.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(saveFile.FileName);
                    Process.Start("explorer.exe", saveFile.FileName);
                }
            }
        }
        private void ExpandAllSalesItems()
        {
            for (int i = 0; i < dgvSales.Rows.Count; i++)
            {
                var row = dgvSales.Rows[i];
                string state = row.Cells["state"].Value?.ToString();
                string loadState = row.Cells["loadState"].Value?.ToString();
                int currLvl = int.Parse(row.Cells["currLvl"].Value?.ToString() ?? "0");

                if (state == "e")
                {
                    dgvSales_CellClick(dgvSales, new DataGridViewCellEventArgs(0, i));
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            ExpandAllSalesItems();
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Sales By Item Summary";
                saveDialog.FileName = "SalesByItemSummary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Item Summary";

                    // Header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "I1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yyyy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Column headers (Row 2)
                    string[] headers = { "Item", "Qty", "Amount", "% of Sales", "Avg Price", "COGS", "Avg COGS", "Gross Margin", "Margin %" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[2, i + 1] = headers[i];
                        var headerCell = worksheet.Cells[2, i + 1];
                        headerCell.Font.Bold = true;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow || !row.Visible) continue;

                        string itemName = row.Cells["colName"].Value?.ToString()?.Replace("►", "").Replace("▼", "").Trim() ?? "";
                        worksheet.Cells[rowIndex, 1] = itemName;
                        worksheet.Cells[rowIndex, 2] = row.Cells["qty"].Value?.ToString();
                        worksheet.Cells[rowIndex, 3] = row.Cells["amount"].Value?.ToString();
                        worksheet.Cells[rowIndex, 4] = row.Cells["percent"].Value?.ToString();
                        worksheet.Cells[rowIndex, 5] = row.Cells["avgPrice"].Value?.ToString();
                        worksheet.Cells[rowIndex, 6] = row.Cells["cogs"].Value?.ToString();
                        worksheet.Cells[rowIndex, 7] = row.Cells["avgCogs"].Value?.ToString();
                        worksheet.Cells[rowIndex, 8] = row.Cells["grossMargin"].Value?.ToString();
                        worksheet.Cells[rowIndex, 9] = row.Cells["grossMarginPercent"].Value?.ToString();

                        for (int col = 1; col <= 9; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;
                            if (col >= 2) // Right-align numbers
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
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

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadItemData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadItemData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadItemData();
            }
        }
    }
}