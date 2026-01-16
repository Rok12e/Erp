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
    public partial class frmVendorSummary : Form
    {
        public frmVendorSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmVendorSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadSalesData();
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");

            CmbType.Text = "Vendor";
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));
        }

        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }

        private void LoadSalesData()
        {
            dgvSales.Rows.Clear();
            string firstDateStr = "";

            // Get the first transaction date
            object firstDateObj = DBClass.ExecuteScalar("SELECT MIN(`date`) FROM tbl_transaction WHERE `date` IS NOT NULL");
            if (firstDateObj != null && firstDateObj != DBNull.Value)
            {
                DateTime firstDate = Convert.ToDateTime(firstDateObj);
                firstDateStr = firstDate.ToString("MMM dd, yy");
            }

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "id");
                dgvSales.Columns["id"].Visible = false;
                dgvSales.Columns.Add("sn", "SN");
                dgvSales.Columns.Add("name", "Name");
                dgvSales.Columns.Add("debit", "Debit");
                dgvSales.Columns.Add("credit", "Credit");
                dgvSales.Columns.Add("balance", "Balance");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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

            decimal totalAmount = 0;

            // Optional date filter
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            string dateFilter = "";

            if (!chkDate.Checked)
            {
                dateFilter = " AND t.date >= @dateFrom AND t.date <= @dateTo ";
                parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                            SELECT 
                                ROW_NUMBER() OVER(ORDER BY MIN(t.date)) AS SN,
                                v.id, 
                                CONCAT(v.code, ' - ', v.name) AS `name`, 
                                IFNULL(SUM(t.debit), 0) AS debit, 
                                IFNULL(SUM(t.credit), 0) AS credit,
                                COALESCE(SUM(CASE 
                                    WHEN t.type = 'Vendor Payment' THEN -IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Purchase Invoice Cash' THEN 0 
                                    WHEN t.type = 'Vendor Opening Balance' AND t.debit > 0 THEN -t.debit 
                                    WHEN t.type = 'Check Cancel (Vendor)' THEN t.credit
                                    WHEN t.type LIKE 'Vendor%' OR t.type LIKE 'Purchase%' THEN IF(t.debit = 0, t.credit, t.debit) 
                                    ELSE 0 
                                END), 0) AS balance
                            FROM tbl_vendor v
                            INNER  JOIN tbl_transaction t 
                                ON v.id = t.hum_id
                                AND t.type IN (
                                                                'Vendor Payment',
                                                                'Purchase Invoice',
                                                                'Vendor Opening Balance',
                                                                'Check Cancel (Vendor)',
                                                                'Purchase Return Invoice',
                                                                'Debit Note',
                                                                'PDC Payable'
                                                    )
                                {dateFilter}
                            AND v.type='Vendor' 
                            GROUP BY v.id, v.code, v.name;";

            if (CmbType.Text == "Subcontractor")
            {
                query = $@"
                            SELECT 
                                ROW_NUMBER() OVER(ORDER BY MIN(t.date)) AS SN,
                                v.id, 
                                CONCAT(v.code, ' - ', v.name) AS `name`, 
                                IFNULL(SUM(t.debit), 0) AS debit, 
                                IFNULL(SUM(t.credit), 0) AS credit,
                                COALESCE(SUM(CASE 
                                    WHEN t.type = 'Subcontractor Payment' THEN -IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Subcontractor Opening Balance' AND t.debit > 0 THEN -t.debit 
                                    WHEN t.type LIKE 'Subcontractor%' OR t.type LIKE 'Purchase%' THEN IF(t.debit = 0, t.credit, t.debit) 
                                    ELSE 0 
                                END), 0) AS balance
                            FROM tbl_vendor v
                            INNER JOIN tbl_transaction t 
                                ON v.id = t.hum_id
                                AND t.type IN (
                                                                'Subcontractor Payment',
                                                                'Purchase Invoice',
                                                                'Subcontractor Opening Balance',
                                                                'Check Cancel (Vendor)',
                                                                'Purchase Return Invoice',
                                                                'Debit Note',
                                                                'PDC Payable'
                                                    )
                                {dateFilter}
                            AND v.type='Subcontractor' 
                            GROUP BY v.id, v.code, v.name;";
            } else if (CmbType.Text == "All")
            {
                query = $@"
                            SELECT 
                                ROW_NUMBER() OVER(ORDER BY MIN(t.date)) AS SN,
                                v.id, 
                                CONCAT(v.code, ' - ', v.name) AS `name`, 
                                IFNULL(SUM(t.debit), 0) AS debit, 
                                IFNULL(SUM(t.credit), 0) AS credit,
                                COALESCE(SUM(CASE 
                                    WHEN t.type IN ('Vendor Payment', 'Subcontractor Payment') THEN -IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type IN ('Purchase Invoice Cash', 'Purchase Invoice') THEN 0 
                                    WHEN t.type LIKE 'Vendor Opening Balance' AND t.debit > 0 THEN -t.debit 
                                    WHEN t.type LIKE 'Check Cancel (Vendor)' THEN t.credit
                                    WHEN t.type LIKE 'Purchase Return Invoice' OR t.type LIKE 'Debit Note' OR t.type LIKE 'PDC Payable' THEN IF(t.debit = 0, t.credit, t.debit) 
                                    ELSE 0 
                                END), 0) AS balance
                            FROM tbl_vendor v
                            INNER JOIN tbl_transaction t 
                                ON v.id = t.hum_id
                                AND t.type IN (
                                                                'Vendor Payment',
                                                                'Subcontractor Payment',
                                                                'Purchase Invoice Cash',
                                                                'Purchase Invoice',
                                                                'Vendor Opening Balance',
                                                                'Subcontractor Opening Balance',
                                                                'Check Cancel (Vendor)',
                                                                'Purchase Return Invoice',
                                                                'Debit Note',
                                                                'PDC Payable'
                                                    )
                                {dateFilter}
                            GROUP BY v.id, v.code, v.name;";
            }

                using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    if (reader["name"] == DBNull.Value) continue;

                    int sn = Convert.ToInt32(reader["SN"]);
                    string name = reader["name"].ToString();
                    decimal debit = Convert.ToDecimal(reader["debit"]);
                    decimal credit = Convert.ToDecimal(reader["credit"]);
                    decimal balance = Convert.ToDecimal(reader["balance"]);

                    int rowIndex = dgvSales.Rows.Add(reader["id"].ToString(), sn, name, debit.ToString("N2"), credit.ToString("N2"), balance.ToString("N2"));
                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                    totalAmount += balance;
                }
            }

            // Total row
            int totalRow = dgvSales.Rows.Add("", "", "TOTAL", "", "", totalAmount.ToString("N2"));
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.Rows[totalRow].Cells[4].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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

            FormattedText summaryText = center.AddFormattedText("Vendor Summary\n", TextFormat.Bold);
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

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "VendorSummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "VendorSummary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Vendor Summary";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "E1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers (row 2)
                    worksheet.Cells[2, 1] = "SN";
                    worksheet.Cells[2, 2] = "Name";
                    worksheet.Cells[2, 3] = "Debit";
                    worksheet.Cells[2, 4] = "Credit";
                    worksheet.Cells[2, 5] = "Balance";

                    for (int i = 1; i <= 5; i++)
                    {
                        var headerCell = worksheet.Cells[2, i];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    }

                    int rowIndex = 3; // Start from row 3
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string sn = row.Cells["sn"].Value?.ToString() ?? "";
                        string name = row.Cells["name"].Value?.ToString() ?? "";
                        string debit = row.Cells["debit"].Value?.ToString() ?? "0";
                        string credit = row.Cells["credit"].Value?.ToString() ?? "0";
                        string balance = row.Cells["balance"].Value?.ToString() ?? "0";

                        if (name.ToUpper().Contains("TOTAL"))
                        {
                            worksheet.Cells[rowIndex, 1] = "";
                            worksheet.Cells[rowIndex, 2] = "TOTAL";
                            worksheet.Cells[rowIndex, 5] = balance;

                            worksheet.Cells[rowIndex, 2].Font.Bold = true;
                            worksheet.Cells[rowIndex, 5].Font.Bold = true;
                            worksheet.Cells[rowIndex, 5].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                            worksheet.Cells[rowIndex, 5].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }
                        else
                        {
                            worksheet.Cells[rowIndex, 1] = sn;
                            worksheet.Cells[rowIndex, 2] = name;
                            worksheet.Cells[rowIndex, 3] = debit;
                            worksheet.Cells[rowIndex, 4] = credit;
                            worksheet.Cells[rowIndex, 5] = balance;

                            for (int col = 1; col <= 5; col++)
                            {
                                var cell = worksheet.Cells[rowIndex, col];
                                cell.Font.Name = "Times New Roman";
                                cell.Font.Size = 9;
                                if (col >= 3) // Right align Debit, Credit, Balance
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
            if (dgvSales.CurrentRow.Cells["id"].Value == null || dgvSales.CurrentRow.Cells["id"].Value.ToString() == "")
                return;
            //if (Utilities.UserPermissionCheck("VendorSummary"))
            //{
            int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
                frmLogin.frmMain.openChildForm(new frmVendorBalanceDetails(id));
            //}
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            LoadSalesData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbType.Text == "All" || CmbType.Text == "Vendor")
            {
                guna2HtmlLabel7.Text = "Vendor Summary";
            }
            else
            {
                guna2HtmlLabel7.Text = CmbType.Text + " Summary";
            }
            LoadSalesData();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox3.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadSalesData();
            }
        }
    }
}