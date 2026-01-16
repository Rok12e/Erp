using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmPurchaseByVendorDetails : Form
    {
        int id;
        public frmPurchaseByVendorDetails(int _id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmPurchaseByVendorDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ";
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

            // Ensure columns exist
            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "Id");
                dgvSales.Columns["id"].Visible = false;
                dgvSales.Columns.Add("type", "Type");
                dgvSales.Columns.Add("date", "Date");
                dgvSales.Columns.Add("num", "Num");
                //dgvSales.Columns.Add("memo", "Memo");
                dgvSales.Columns.Add("name", "Name");
                dgvSales.Columns.Add("item", "Item");
                dgvSales.Columns.Add("qty", "Qty");
                dgvSales.Columns.Add("price", "Sales Price");
                dgvSales.Columns.Add("amount", "Amount");
                dgvSales.Columns.Add("CostCenter", "CostCenter");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvSales.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
                dgvSales.EnableHeadersVisualStyles = false;
                dgvSales.RowHeadersVisible = false;
                dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

            string salesQuery = @"
                                SELECT 
                                    s.id AS id,
                                    (
                                        SELECT t.type 
                                        FROM tbl_transaction t 
                                        WHERE t.transaction_id = s.id 
                                          AND t.t_type LIKE '%purchase%' 
                                        LIMIT 1
                                    ) AS type,
                                    s.date,
                                    s.invoice_id AS num,
                                    c.name AS Vendor_name
                                FROM 
                                    tbl_purchase s
                                JOIN 
                                    tbl_vendor c ON s.vendor_id = c.id
                                WHERE 
                                    s.vendor_id = @cid
                                And s.state = 0
                                AND s.date >= @startDate AND s.date<= @endDate
                                ORDER BY 
                                    s.date;
                        ";

            var parameters = new[]
                {
                    DBClass.CreateParameter("@cid", id),
                    DBClass.CreateParameter("startDate", dateTimePicker1.Value.Date),
                    DBClass.CreateParameter("endDate", dateTimePicker2.Value.Date)
                };

            using (MySqlDataReader salesReader = DBClass.ExecuteReader(salesQuery, parameters.ToArray()))
            {
                decimal customerTotal = 0;

                while (salesReader.Read())
                {
                    int saleId = Convert.ToInt32(salesReader["id"]);
                    string type = salesReader["type"].ToString();
                    string date = Convert.ToDateTime(salesReader["date"]).ToString("dd/MM/yyyy");
                    string num = salesReader["num"].ToString();
                    string customer = salesReader["vendor_name"].ToString();

                    string itemQuery = @"
                                        SELECT 
                                        CONCAT(ti.code,' - ',ti.name) AS `Item Name`,
                                        ts.qty AS Qty,
                                        ts.price AS Price,
                                        ts.total AS Total,
                                        IFNULL((select CONCAT(CODE,' - ',NAME) FROM tbl_cost_center WHERE id = ts.cost_center_id),'') AS CostCenter
                                        FROM 
                                        tbl_purchase_details ts
                                        INNER JOIN 
                                        tbl_items ti ON ts.item_id = ti.id
                                        WHERE 
                                        ts.purchase_id = @id;
                                        ";

                    MySqlParameter itemParam = new MySqlParameter("@id", saleId);

                    using (MySqlDataReader itemReader = DBClass.ExecuteReader(itemQuery, itemParam))
                    {
                        bool firstRow = true;

                        while (itemReader.Read())
                        {
                            string item = itemReader["Item Name"].ToString();
                            decimal qty = Convert.ToDecimal(itemReader["Qty"]);
                            decimal price = Convert.ToDecimal(itemReader["Price"]);
                            decimal amount = Convert.ToDecimal(itemReader["Total"]);
                            string costCenter = itemReader["CostCenter"].ToString();

                            dgvSales.Rows.Add(
                                saleId.ToString(), // id
                                type,
                                date,
                                num,
                                customer,
                                item,
                                qty.ToString("N2"),
                                price.ToString("N2"),
                                amount.ToString("N2"),
                                costCenter.ToString()
                            );

                            customerTotal += amount;

                            //if (firstRow)
                            //{
                            //    // Only clear values AFTER first row of this invoice
                            //    type = "";
                            //    date = "";
                            //    num = "";
                            //    firstRow = false;
                            //}
                        }
                    }
                }
                // Final TOTAL row
                dgvSales.Rows.Add("", "", "", "", "TOTAL", "", "", "", customerTotal.ToString("N2"));
                dgvSales.Rows[dgvSales.Rows.Count - 1].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
                dgvSales.Rows[dgvSales.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            }
        }
        
        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string name = dgvSales.Rows[e.RowIndex].Cells["name"].Value?.ToString();
                if (string.Equals(name, "TOTAL", StringComparison.OrdinalIgnoreCase))
                    return;
                if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["type"].Value.ToString();
                    if (_type.Contains("Invoice"))
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString())));
                    }
                    //else if(_type == "Purchase Return Invoice")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmPurchaseReturn(id));
                    //}
                }
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

            FormattedText titleText = center.AddFormattedText("Purchase By Vendor Details\n", TextFormat.Bold);
            titleText.Font.Size = 12;

            FormattedText subtitleText = center.AddFormattedText("All Transaction", TextFormat.NotBold);
            subtitleText.Font.Size = 9;

            //section.Add(headerTable); // ✅ Attach header table

            // Bold separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Main table
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.75;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            // Define columns (total = 18.5cm)
            dataTable.AddColumn("2.2cm"); // Type
            dataTable.AddColumn("2.2cm"); // Date
            dataTable.AddColumn("2.2cm"); // Num
            dataTable.AddColumn("3.2cm"); // Name
            dataTable.AddColumn("4.0cm"); // Item
            dataTable.AddColumn("1.3cm"); // Qty
            dataTable.AddColumn("2.0cm"); // Sales Price
            dataTable.AddColumn("2.0cm"); // Amount

            // Header row
            Row tableHeader = dataTable.AddRow();
            tableHeader.Shading.Color = Colors.LightGray;
            tableHeader.Format.Font.Bold = true;
            tableHeader.Cells[0].AddParagraph("Type");
            tableHeader.Cells[1].AddParagraph("Date");
            tableHeader.Cells[2].AddParagraph("Num");
            tableHeader.Cells[3].AddParagraph("Name");
            tableHeader.Cells[4].AddParagraph("Item");
            tableHeader.Cells[5].AddParagraph("Qty");
            tableHeader.Cells[6].AddParagraph("Sales Price");
            tableHeader.Cells[7].AddParagraph("Amount");

            decimal totalAmount = 0;

            // Add data rows
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string type = row.Cells["type"]?.Value?.ToString() ?? "";
                string date = row.Cells["date"]?.Value?.ToString() ?? "";
                string num = row.Cells["num"]?.Value?.ToString() ?? "";
                string name = row.Cells["name"]?.Value?.ToString() ?? "";
                string item = row.Cells["item"]?.Value?.ToString() ?? "";
                string qty = row.Cells["qty"]?.Value?.ToString() ?? "";
                string price = row.Cells["price"]?.Value?.ToString() ?? "";
                string amountStr = row.Cells["amount"]?.Value?.ToString() ?? "0";

                // Avoid printing rows with TOTAL in name (just in case)
                if (name.Trim().ToUpper() == "TOTAL") continue;

                decimal amount;
                decimal.TryParse(amountStr, out amount);
                totalAmount += amount;

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(type);
                tRow.Cells[1].AddParagraph(date);
                tRow.Cells[2].AddParagraph(num);
                tRow.Cells[3].AddParagraph(name);
                tRow.Cells[4].AddParagraph(item);
                tRow.Cells[5].AddParagraph(qty).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[6].AddParagraph(price).Format.Alignment = ParagraphAlignment.Right;
                Paragraph amt = tRow.Cells[7].AddParagraph();
                amt.AddFormattedText(amount.ToString("N2"));
                tRow.Cells[7].Format.Alignment = ParagraphAlignment.Right;
            }

            // Total row (only once)
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 6;
            Paragraph totalLabel = totalRow.Cells[0].AddParagraph("TOTAL");
            totalLabel.Format.Font.Bold = true;

            Paragraph totalVal = totalRow.Cells[7].AddParagraph();
            FormattedText formattedTotal = totalVal.AddFormattedText(totalAmount.ToString("N2"));
            formattedTotal.Font.Bold = true;
            formattedTotal.Font.Underline = Underline.Single;
            totalRow.Cells[7].Format.Alignment = ParagraphAlignment.Right;

            //section.Add(dataTable); // ✅ Attach main table

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFile.Title = "Save Sales By Customer Details";
                saveFile.FileName = "PurchaseByVendorDetails.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(saveFile.FileName);
                    Process.Start("explorer.exe", saveFile.FileName);
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "PurchaseByVendorDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Purchase By Vendor";

                    // Add Title
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "H1"];
                    headerRange.Merge();
                    headerRange.Value = "Purchase By Vendor Details - " + DateTime.Now.ToString("dd/MM/yyyy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Size = 12;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add Column Headers
                    string[] headers = { "Type", "Date", "Num", "Name", "Item", "Qty", "Sales Price", "Amount" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[2, i + 1] = headers[i];
                        var cell = worksheet.Cells[2, i + 1];
                        cell.Font.Bold = true;
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 10;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    }

                    int rowIndex = 3;
                    decimal totalAmount = 0;

                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string type = row.Cells["type"]?.Value?.ToString() ?? "";
                        string date = row.Cells["date"]?.Value?.ToString() ?? "";
                        string num = row.Cells["num"]?.Value?.ToString() ?? "";
                        string name = row.Cells["name"]?.Value?.ToString() ?? "";
                        string item = row.Cells["item"]?.Value?.ToString() ?? "";
                        string qty = row.Cells["qty"]?.Value?.ToString() ?? "";
                        string price = row.Cells["price"]?.Value?.ToString() ?? "";
                        string amountStr = row.Cells["amount"]?.Value?.ToString() ?? "0";

                        if (name.Trim().ToUpper() == "TOTAL") continue;

                        decimal amount;
                        decimal.TryParse(amountStr, out amount);
                        totalAmount += amount;

                        worksheet.Cells[rowIndex, 1] = type;
                        worksheet.Cells[rowIndex, 2] = date;
                        worksheet.Cells[rowIndex, 3] = num;
                        worksheet.Cells[rowIndex, 4] = name;
                        worksheet.Cells[rowIndex, 5] = item;
                        worksheet.Cells[rowIndex, 6] = qty;
                        worksheet.Cells[rowIndex, 7] = price;
                        worksheet.Cells[rowIndex, 8] = amount.ToString("N2");

                        for (int col = 1; col <= 8; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;
                            if (col >= 6) // Qty, Price, Amount
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }

                        rowIndex++;
                    }

                    // TOTAL Row
                    worksheet.Cells[rowIndex, 1] = "";
                    worksheet.Cells[rowIndex, 2] = "";
                    worksheet.Cells[rowIndex, 3] = "";
                    worksheet.Cells[rowIndex, 4] = "";
                    worksheet.Cells[rowIndex, 5] = "TOTAL";
                    worksheet.Cells[rowIndex, 5].Font.Bold = true;

                    worksheet.Cells[rowIndex, 8] = totalAmount.ToString("N2");
                    worksheet.Cells[rowIndex, 8].Font.Bold = true;
                    worksheet.Cells[rowIndex, 8].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                    worksheet.Cells[rowIndex, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                    // Auto-fit columns
                    worksheet.Columns.AutoFit();

                    // Save
                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close(false);
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dateTimePicker1, dateTimePicker2);
                LoadData();
            }
        }
    }
}
