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

namespace YamyProject
{
    public partial class frmCustomerAgingSummary : Form
    {
        public frmCustomerAgingSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmCustomerAgingSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
            LoadData();
        }
        int days = 90;
        private void LoadData()
        {
            dgvSales.Rows.Clear();
            dgvSales.Columns.Clear();

            decimal totalAmount = 0, total_0_days = 0, total_1_30 = 0, total_31_60 = 0, total_61_90 = 0, total_91_plus = 0;

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "id");
                dgvSales.Columns["id"].Visible = false;
                dgvSales.Columns.Add("sn", "SN");
                dgvSales.Columns.Add("name", "Name");

                if (days == 90)
                {
                    dgvSales.Columns.Add("0_days", "Current");
                    dgvSales.Columns.Add("1_30", "1 - 30");
                    dgvSales.Columns.Add("31_60", "31 - 60");
                    dgvSales.Columns.Add("61_90", "61 - 90");
                    dgvSales.Columns.Add("91_plus", "> 90");
                    dgvSales.Columns.Add("total", "Total");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);
                }

                dgvSales.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                foreach (DataGridViewColumn col in dgvSales.Columns)
                {
                    col.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                }

                dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.SingleVertical;
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

            // Handle optional date filtering
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            string dateFilter = "";

            if (!chkDate.Checked)
            {
                dateFilter = " AND DATE(t.date) >= @dateFrom AND DATE(t.date) <= @dateTo ";
                parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date));
            }

            string query = $@"
        SELECT 
            ROW_NUMBER() OVER (ORDER BY c.id) AS SN,
            c.id AS customer_id,
            CONCAT(CODE,' ',c.name) AS customer_name,
            SUM(CASE WHEN DATEDIFF(CURDATE(), t.date) = 0 THEN t.debit - t.credit ELSE 0 END) AS '0_days',
            SUM(CASE WHEN DATEDIFF(CURDATE(), t.date) BETWEEN 1 AND 30 THEN t.debit - t.credit ELSE 0 END) AS '1_30',
            SUM(CASE WHEN DATEDIFF(CURDATE(), t.date) BETWEEN 31 AND 60 THEN t.debit - t.credit ELSE 0 END) AS '31_60',
            SUM(CASE WHEN DATEDIFF(CURDATE(), t.date) BETWEEN 61 AND 90 THEN t.debit - t.credit ELSE 0 END) AS '61_90',
            SUM(CASE WHEN DATEDIFF(CURDATE(), t.date) > 90 THEN t.debit - t.credit ELSE 0 END) AS '91_plus',
            SUM(t.debit - t.credit) AS total
        FROM tbl_customer c
        INNER JOIN tbl_transaction t ON t.hum_id = c.id
        WHERE 
            t.state = 0 AND t.type IN
            ('Customer Receipt','Sales Invoice','Check Cancel (Customer)','SalesReturn Invoice','Credit Note','PDC Receivable')
            {dateFilter}
        GROUP BY c.id, c.name
        HAVING SUM(t.debit - t.credit) > 0
        ORDER BY c.id;";

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    int sn = Convert.ToInt32(reader["SN"]);
                    string customer_id = reader["customer_id"].ToString();
                    string name = reader["customer_name"].ToString();

                    decimal _0_days = Convert.ToDecimal(reader["0_days"]);
                    decimal _1_30 = Convert.ToDecimal(reader["1_30"]);
                    decimal _31_60 = Convert.ToDecimal(reader["31_60"]);
                    decimal _61_90 = Convert.ToDecimal(reader["61_90"]);
                    decimal _91_plus = Convert.ToDecimal(reader["91_plus"]);
                    decimal total = Convert.ToDecimal(reader["total"]);

                    int rowIndex = dgvSales.Rows.Add(
                        customer_id, sn, name,
                        _0_days.ToString("N2"),
                        _1_30.ToString("N2"),
                        _31_60.ToString("N2"),
                        _61_90.ToString("N2"),
                        _91_plus.ToString("N2"),
                        total.ToString("N2")
                    );

                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                    total_0_days += _0_days;
                    total_1_30 += _1_30;
                    total_31_60 += _31_60;
                    total_61_90 += _61_90;
                    total_91_plus += _91_plus;
                    totalAmount += total;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add(
                "", "", "TOTAL",
                total_0_days.ToString("N2"),
                total_1_30.ToString("N2"),
                total_31_60.ToString("N2"),
                total_61_90.ToString("N2"),
                total_91_plus.ToString("N2"),
                totalAmount.ToString("N2")
            );

            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.Rows[totalRow].Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            string companyName = lblCompany.Text;

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

            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;
            center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("Customer Aging Summary\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("As of " + DateTime.Now.ToString("MMMM dd, yyyy"), TextFormat.NotBold).Font.Size = 9;

            //section.Add(headerTable);

            // Separator
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Main table
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 9;

            // Define aging columns
            table.AddColumn("1cm"); // SN
            table.AddColumn("6cm"); // Customer Name
            table.AddColumn("1.8cm"); // 0 Days
            table.AddColumn("1.8cm"); // 1–30
            table.AddColumn("1.8cm"); // 31–60
            table.AddColumn("1.8cm"); // 61–90
            table.AddColumn("1.8cm"); // 91+
            table.AddColumn("3.1cm"); // Balance

            Row header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Format.Font.Bold = true;

            header.Cells[0].AddParagraph("SN");
            header.Cells[1].AddParagraph("Name");
            header.Cells[2].AddParagraph("0 Days");
            header.Cells[3].AddParagraph("1–30 Days");
            header.Cells[4].AddParagraph("31–60 Days");
            header.Cells[5].AddParagraph("61–90 Days");
            header.Cells[6].AddParagraph("91+ Days");
            header.Cells[7].AddParagraph("Total");

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow || !row.Visible) continue;

                Row tRow = table.AddRow();
                tRow.Cells[0].AddParagraph(row.Cells["sn"].Value?.ToString() ?? "");
                tRow.Cells[1].AddParagraph(row.Cells["name"].Value?.ToString() ?? "");

                tRow.Cells[2].AddParagraph(row.Cells["0_days"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[3].AddParagraph(row.Cells["1_30"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[4].AddParagraph(row.Cells["31_60"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[5].AddParagraph(row.Cells["61_90"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[6].AddParagraph(row.Cells["91_plus"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[7].AddParagraph(row.Cells["total"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
            }

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFile.Title = "Save Customer Aging Summary";
                saveFile.FileName = "CustomerAgingSummary.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(saveFile.FileName);
                    Process.Start("explorer.exe", saveFile.FileName);
                }
            }
        }

        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }
        
        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = lblCompany.Text;
            // Create PDF document
            Document doc = new Document();
            Section section = doc.AddSection();

            // Adjust page margins
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Header table
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("5cm");

            Row headerRow = headerTable.AddRow();

            // Left cell (Time & Date)
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell (Company name + titles)
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;

            FormattedText summaryText = center.AddFormattedText("Customer Aging Summery\n", TextFormat.Bold);
            summaryText.Font.Size = 12;

            FormattedText allTransText = center.AddFormattedText("All Transactions", TextFormat.NotBold);
            allTransText.Font.Size = 9;

            // Add header table
            //section.Add(headerTable);

            // Bold separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for sales data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.75;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            // Define columns
            dataTable.AddColumn("1.5cm"); // SN
            dataTable.AddColumn("6.5cm"); // Name
            dataTable.AddColumn("3cm");   // Debit
            dataTable.AddColumn("3cm");   // Credit
            dataTable.AddColumn("3cm");   // Balance

            // Add table header row
            Row tableHeader = dataTable.AddRow();
            tableHeader.Shading.Color = Colors.LightGray;
            tableHeader.Format.Font.Bold = true;
            tableHeader.Cells[0].AddParagraph("SN");
            tableHeader.Cells[1].AddParagraph("Name");
            tableHeader.Cells[2].AddParagraph("Debit");
            tableHeader.Cells[3].AddParagraph("Credit");
            tableHeader.Cells[4].AddParagraph("Balance");

            decimal totalAmount = 0;

            // Load data from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string sn = row.Cells["sn"].Value?.ToString() ?? "";
                string name = row.Cells["name"].Value?.ToString() ?? "";
                string debitStr = row.Cells["debit"].Value?.ToString() ?? "0";
                string creditStr = row.Cells["credit"].Value?.ToString() ?? "0";
                string balanceStr = row.Cells["balance"].Value?.ToString() ?? "0";

                if (name.ToUpper().Contains("TOTAL")) continue;

                decimal balance = 0;
                decimal.TryParse(balanceStr.Replace("◀", "").Trim(), out balance);
                totalAmount += balance;

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(sn);
                tRow.Cells[1].AddParagraph(name);
                tRow.Cells[2].AddParagraph(debitStr).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[3].AddParagraph(creditStr).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[4].AddParagraph(balanceStr).Format.Alignment = ParagraphAlignment.Right;
            }

            // Add TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 3;
            Paragraph totalLabel = totalRow.Cells[0].AddParagraph("TOTAL");
            totalLabel.Format.Font.Bold = true;

            Paragraph totalText = totalRow.Cells[4].AddParagraph();
            FormattedText totalFormatted = totalText.AddFormattedText(totalAmount.ToString("N2"));
            totalFormatted.Font.Bold = true;
            totalFormatted.Font.Underline = Underline.Single;
            totalRow.Cells[4].Format.Alignment = ParagraphAlignment.Right;

            // Add table to document
            //section.Add(dataTable);

            // Render and save PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CustomerAgingSummarySummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "CustomerAgingSummery.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Customer Aging Summary";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "E1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers (row 2)
                    // Add column headers (row 2)
                    worksheet.Cells[2, 1] = "SN";
                    worksheet.Cells[2, 2] = "Name";
                    worksheet.Cells[2, 3] = "0 Days";
                    worksheet.Cells[2, 4] = "1-30 Days";
                    worksheet.Cells[2, 5] = "31-60 Days";
                    worksheet.Cells[2, 6] = "61-90 Days";
                    worksheet.Cells[2, 7] = "91+ Days";
                    worksheet.Cells[2, 8] = "Total Balance";

                    for (int i = 1; i <= 8; i++)
                    {
                        var headerCell = worksheet.Cells[2, i];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string sn = row.Cells["sn"].Value?.ToString() ?? "";
                        string name = row.Cells["name"].Value?.ToString() ?? "";
                        string d0 = row.Cells["0_days"].Value?.ToString() ?? "0";
                        string d30 = row.Cells["1_30"].Value?.ToString() ?? "0";
                        string d60 = row.Cells["31_60"].Value?.ToString() ?? "0";
                        string d90 = row.Cells["61_90"].Value?.ToString() ?? "0";
                        string d91 = row.Cells["91_plus"].Value?.ToString() ?? "0";
                        string balance = row.Cells["total"].Value?.ToString() ?? "0";

                        if (name.ToUpper().Contains("TOTAL"))
                        {
                            worksheet.Cells[rowIndex, 2] = "TOTAL";
                            worksheet.Cells[rowIndex, 8] = balance;

                            worksheet.Cells[rowIndex, 2].Font.Bold = true;
                            worksheet.Cells[rowIndex, 8].Font.Bold = true;
                            worksheet.Cells[rowIndex, 8].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                            worksheet.Cells[rowIndex, 8].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, 1] = sn;
                            worksheet.Cells[rowIndex, 2] = name;
                            worksheet.Cells[rowIndex, 3] = d0;
                            worksheet.Cells[rowIndex, 4] = d30;
                            worksheet.Cells[rowIndex, 5] = d60;
                            worksheet.Cells[rowIndex, 6] = d90;
                            worksheet.Cells[rowIndex, 7] = d91;
                            worksheet.Cells[rowIndex, 8] = balance;

                            for (int col = 1; col <= 8; col++)
                            {
                                var cell = worksheet.Cells[rowIndex, col];
                                cell.Font.Name = "Times New Roman";
                                cell.Font.Size = 9;
                                if (col >= 3) // Right-align aging columns and balance
                                    cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            }
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

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSales.CurrentRow.Cells["id"].Value == null|| dgvSales.CurrentRow.Cells["id"].Value.ToString()=="")
                return;
            //if (Utilities.UserPermissionCheck("CustomerAgingSummary"))
            //{
                int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
                frmLogin.frmMain.openChildForm(new frmCustomerAgingDetails(id));
            //}
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

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox3.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadData();
            }
        }
    }
}