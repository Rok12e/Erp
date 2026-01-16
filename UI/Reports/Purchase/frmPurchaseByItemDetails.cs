using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmPurchaseByItemDetails : Form
    {
        int _itemId;
        public frmPurchaseByItemDetails(int itemId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            _itemId = itemId;
        }

        private void frmPurchaseByItemDetails_Load(object sender, EventArgs e)
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

            dgvSales.Columns.Add("state", "");
            dgvSales.Columns.Add("loadState", "");
            dgvSales.Columns.Add("currLvl", "");
            dgvSales.Columns.Add("colName", "Type");
            dgvSales.Columns.Add("date", "Date");
            dgvSales.Columns.Add("num", "Invoice No");
            dgvSales.Columns.Add("memo", "Memo");
            dgvSales.Columns.Add("customer", "Customer");
            dgvSales.Columns.Add("qty", "Qty");
            dgvSales.Columns.Add("price", "Sales Price");
            dgvSales.Columns.Add("amount", "Amount");
            dgvSales.Columns.Add("balance", "Balance");
            dgvSales.Columns.Add("id", "ID");
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

            string sql = @"
                    SELECT 
                        i.id AS item_id,
                        i.name AS item_name,
                        i.type,
                        ic.id AS category_id,
                        ic.code,
                        ic.name AS category_name
                    FROM 
                        tbl_items i
                    JOIN 
                        tbl_item_category ic ON i.category_id = ic.id
                    WHERE 
                        i.id = @itemId
                    AND i.state = 0";

            DataTable dt = DBClass.ExecuteDataTable(sql, DBClass.CreateParameter("@itemId", _itemId));

            if (dt.Rows.Count == 0)
                return;

            var row = dt.Rows[0];

            string itemName = row["item_name"].ToString();
            string type = row["type"].ToString();
            string category = $"{row["code"]} - {row["category_name"]}";
            int categoryId = Convert.ToInt32(row["category_id"]);

            // Add: Type (Level 1)
            dgvSales.Rows.Add("e", "l", "1", "   ▼   " + type, "", "", "", "", "", "", "", "", type);

            // Add: Category (Level 2)
            dgvSales.Rows.Add("e", "l", "2", "         ▼ " + category, "", "", "", "", "", "", "", "", categoryId + "|" + type);

            // Add: Item (Level 3)
            dgvSales.Rows.Add("e", "l", "3", "               " + itemName, "", "", "", "", "", "", "", "", _itemId.ToString());

            // Add: Invoices (Level 4)
            LoadInvoicesForItem(_itemId, dgvSales.Rows.Count - 1);  // insert below item
        }
        private void LoadInvoicesForItem(int itemId, int insertIndex)
        {
            string sql = @"
                        SELECT 
                        sd.purchase_id AS purchase_id,
                        i.id AS item_id,
                        i.name AS item_name,
                        s.date,
                        s.invoice_id AS num,
                        s.bill_to AS vendor,
                        CONCAT(i.type,' - ' ,i.name, ' - ', ic.name) AS memo,
                        sd.qty,
                        sd.price,
                        sd.total AS amount,
                        (sd.qty * sd.price) - sd.discount AS balance
                        FROM tbl_purchase_details sd
                        JOIN tbl_items i ON sd.item_id = i.id
                        JOIN tbl_purchase s ON sd.purchase_id = s.id
                        JOIN tbl_item_category ic ON i.category_id = ic.id
                        WHERE sd.item_id = @itemId AND s.state = 0
                            AND s.date >= @startDate AND s.date<= @endDate
                        ORDER BY s.date";

            DataTable invoices = DBClass.ExecuteDataTable(sql, DBClass.CreateParameter("@itemId", itemId), DBClass.CreateParameter("startDate", dtpFrom.Value.Date),
                DBClass.CreateParameter("endDate", dtpTo.Value.Date));

            foreach (DataRow row in invoices.Rows)
            {
                dgvSales.Rows.Insert(insertIndex + 1, "n", "l", "4", "",
                    Convert.ToDateTime(row["date"]).ToString("dd/MM/yyyy"),
                    row["num"], row["memo"], row["vendor"],
                    row["qty"], row["price"], row["amount"], row["balance"], row["purchase_id"]);
                insertIndex++;
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
                        // Expand: Type -> Categories
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
                                            c.code;";

                        DataTable categoryTable = DBClass.ExecuteDataTable(sql, DBClass.CreateParameter("@type", key));
                        int insertIndex = e.RowIndex + 1;

                        foreach (DataRow cat in categoryTable.Rows)
                        {
                            string catName = cat["category_name"].ToString();
                            string catId = cat["id"].ToString();
                            dgvSales.Rows.Insert(insertIndex, "e", "u", "2", space + "► " + catName, "", "", "", "", "", "", "", "", catId + "|" + key);
                            insertIndex++;
                        }
                    }
                    else if (currLvl == 2)
                    {
                        // Expand: Category -> Invoice Rows by Item
                        string[] split = key.Split('|');
                        int catId = int.Parse(split[0]);
                        string itemType = split[1];

                        string sql = @"
                                SELECT 
                                    i.id AS id,
                                    i.name AS item_name,
                                    s.date,
                                    s.invoice_id AS num,
                                    s.bill_to AS customer,
                                    CONCAT(i.type,' - ' ,i.name, ' - ', ic.name) AS memo,
                                    sd.qty,
                                    sd.price,
                                    sd.total AS amount,
                                    (sd.qty * sd.price) - sd.discount AS balance
                                FROM 
                                    tbl_purchase_details sd
                                JOIN 
                                    tbl_items i ON sd.item_id = i.id
                                JOIN 
                                    tbl_purchase s ON sd.purchase_id = s.id
                                JOIN 
                                    tbl_item_category ic ON i.category_id = ic.id
                                WHERE 
                                i.category_id = @catId AND i.type = @type
                                AND s.state = 0
                                AND s.date >= @startDate AND s.date <= @endDate 
                            ORDER BY 
                                i.name, s.date;";

                        DataTable items = DBClass.ExecuteDataTable(sql,
                            DBClass.CreateParameter("@catId", catId),
                            DBClass.CreateParameter("@type", itemType), DBClass.CreateParameter("startDate", dtpFrom.Value.Date),
                DBClass.CreateParameter("endDate", dtpTo.Value.Date));

                        int insertIndex = e.RowIndex + 1;
                        string currentItem = "";

                        foreach (DataRow row in items.Rows)
                        {
                            string itemName = row["item_name"].ToString();

                            if (itemName != currentItem)
                            {
                                dgvSales.Rows.Insert(insertIndex, "e", "l", "3", space + "      " + itemName, "", "", "", "", "", "", "",row["id"].ToString());
                                insertIndex++;
                                currentItem = itemName;
                            }

                            dgvSales.Rows.Insert(insertIndex, "n", "l", "4","",
                                Convert.ToDateTime(row["date"]).ToString("dd/MM/yyyy"),
                                row["num"], row["memo"], row["customer"],
                                row["qty"], row["price"], row["amount"], row["balance"], "");
                            insertIndex++;
                        }
                    }
                     else if (currLvl == 3)
            {
                // Expand Item → Invoices
                int itemId;
                if (int.TryParse(key, out itemId))
                {
                    string sql = @"
                                SELECT 
                                    s.date,
                                    s.invoice_id AS num,
                                    s.sales_man AS customer,
                                    s.po_num AS memo,
                                    sd.qty,
                                    sd.price,
                                    sd.total AS amount,
                                    (sd.qty * sd.price) - sd.discount AS balance
                                FROM 
                                    tbl_purchase_details sd
                                JOIN 
                                    tbl_purchase s ON sd.purchase_id = s.id
                                WHERE 
                                    sd.item_id = @itemId AND s.state = 0
                                    AND s.date >= @startDate AND s.date <= @endDate
                                ORDER BY 
                                    s.date";

                    DataTable invoices = DBClass.ExecuteDataTable(sql, DBClass.CreateParameter("@itemId", itemId),
                        DBClass.CreateParameter("startDate", dtpFrom.Value.Date),
                DBClass.CreateParameter("endDate", dtpTo.Value.Date));
                    int insertIndex = e.RowIndex + 1;

                    foreach (DataRow row in invoices.Rows)
                    {
                        dgvSales.Rows.Insert(insertIndex, "n", "l", "4", "", 
                            Convert.ToDateTime(row["date"]).ToString("dd/MM/yyyy"),
                            row["num"], row["memo"], row["customer"],
                            row["qty"], row["price"], row["amount"], row["balance"], "");
                        insertIndex++;
                    }

                    dgvSales.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                }
            }

            dgvSales.Rows[e.RowIndex].Cells["colName"].Value = cellValue.Replace("►", "▼");
            dgvSales.Rows[e.RowIndex].Cells["state"].Value = "c";
        }
        else
        {
            ToggleSalesChildren(e.RowIndex, true);
        }
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

            // 🔹 Open only on Level 4 (invoices)
            if (currLvl != 4)
                return;

            int salesId;
            if (int.TryParse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString(), out salesId))
            {
                frmLogin.frmMain.openChildForm(new frmPurchase(salesId));  
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ExpandAllSalesItems();
            string companyName = "Company Name";

            // Get company name
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
            {
                if (reader.Read())
                    companyName = reader["name"].ToString();
            }

            // Create document
            Document doc = new Document();
            Section section = doc.AddSection();

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

            // Left cell: time + date
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center: Company + title
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;
            FormattedText titleText = center.AddFormattedText("Purchase By Item Details\n", TextFormat.Bold);
            titleText.Font.Size = 12;
            FormattedText subTitle = center.AddFormattedText("All Transaction", TextFormat.NotBold);
            subTitle.Font.Size = 9;

            // Separator
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Data table
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 9;

            // Columns
            table.AddColumn("3.0cm"); // Type
            table.AddColumn("2.2cm"); // Date
            table.AddColumn("2.0cm"); // Invoice No
            table.AddColumn("2.5cm"); // Memo
            table.AddColumn("2.5cm"); // Customer
            table.AddColumn("1.2cm"); // Qty
            table.AddColumn("2.0cm"); // Sales Price
            table.AddColumn("1.9cm"); // Amount
            table.AddColumn("1.9cm"); // Balance


            Row head = table.AddRow();
            head.Shading.Color = Colors.LightGray;
            head.Format.Font.Bold = true;

            string[] headers = { "Type", "Date", "Invoice No", "Memo", "Customer", "Qty", "Sales Price", "Amount", "Balance" };
            for (int i = 0; i < headers.Length; i++)
                head.Cells[i].AddParagraph(headers[i]);

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow || !row.Visible) continue;

                Row tRow = table.AddRow();

                string itemText = row.Cells["colName"].Value?.ToString()?.Replace("►", "").Replace("▼", "").Trim() ?? "";
                tRow.Cells[0].AddParagraph(itemText);
                tRow.Cells[1].AddParagraph(row.Cells["date"].Value?.ToString() ?? "");
                tRow.Cells[2].AddParagraph(row.Cells["num"].Value?.ToString() ?? "");
                tRow.Cells[3].AddParagraph(row.Cells["memo"].Value?.ToString() ?? "");
                tRow.Cells[4].AddParagraph(row.Cells["customer"].Value?.ToString() ?? "");
                decimal qtyVal;
                tRow.Cells[5].AddParagraph(decimal.TryParse(row.Cells["qty"].Value?.ToString(), out qtyVal) ? qtyVal.ToString("N2") : "").Format.Alignment = ParagraphAlignment.Right;

                decimal priceVal;
                tRow.Cells[6].AddParagraph(decimal.TryParse(row.Cells["price"].Value?.ToString(), out priceVal) ? priceVal.ToString("N2") : "").Format.Alignment = ParagraphAlignment.Right;

                decimal amtVal;
                tRow.Cells[7].AddParagraph(decimal.TryParse(row.Cells["amount"].Value?.ToString(), out amtVal) ? amtVal.ToString("N2") : "").Format.Alignment = ParagraphAlignment.Right;

                decimal balVal;
                tRow.Cells[8].AddParagraph(decimal.TryParse(row.Cells["balance"].Value?.ToString(), out balVal) ? balVal.ToString("N2") : "").Format.Alignment = ParagraphAlignment.Right;
            }

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            // Save to file
            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFile.Title = "Save Sales By Item Details";
                saveFile.FileName = "PurchaseByItemDetails.pdf";

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
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Sales By Item Details";
                saveDialog.FileName = "PurchaseByItemDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Purchase Item Details";

                    // Header with date and time
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "I1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Column headers
                    string[] headers = { "Type", "Date", "Invoice No", "Memo", "Customer", "Qty", "Sales Price", "Amount", "Balance" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[2, i + 1] = headers[i];
                        var cell = worksheet.Cells[2, i + 1];
                        cell.Font.Bold = true;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 10;
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow || !row.Visible) continue;

                        string type = (row.Cells["colName"].Value?.ToString() ?? "").Replace("►", "").Replace("▼", "").Trim();
                        string date = row.Cells["date"].Value?.ToString() ?? "";
                        string num = row.Cells["num"].Value?.ToString() ?? "";
                        string memo = row.Cells["memo"].Value?.ToString() ?? "";
                        string customer = row.Cells["customer"].Value?.ToString() ?? "";
                        string qty = FormatNumber(row.Cells["qty"].Value);
                        string price = FormatNumber(row.Cells["price"].Value);
                        string amount = FormatNumber(row.Cells["amount"].Value);
                        string balance = FormatNumber(row.Cells["balance"].Value);

                        worksheet.Cells[rowIndex, 1] = type;
                        worksheet.Cells[rowIndex, 2] = date;
                        worksheet.Cells[rowIndex, 3] = num;
                        worksheet.Cells[rowIndex, 4] = memo;
                        worksheet.Cells[rowIndex, 5] = customer;
                        worksheet.Cells[rowIndex, 6] = qty;
                        worksheet.Cells[rowIndex, 7] = price;
                        worksheet.Cells[rowIndex, 8] = amount;
                        worksheet.Cells[rowIndex, 9] = balance;

                        // Align right for numbers
                        for (int col = 6; col <= 9; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;
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
        private string FormatNumber(object val)
        {
            decimal result;
            return decimal.TryParse(Convert.ToString(val), out result) ? result.ToString("N2") : "";
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