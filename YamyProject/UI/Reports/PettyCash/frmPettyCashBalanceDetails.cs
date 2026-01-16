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
    public partial class frmPettyCashBalanceDetails : Form
    {
        int id;
        public frmPettyCashBalanceDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        bool EnableApproval = false;

        private void frmPettyCashBalanceDetails_Load(object sender, EventArgs e)
        {
            DateTime dated = DateTime.Now;
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");

            var barcodeS = Utilities.GeneralSettingsState("ENABLE PETTYCASH APPROVAL");
            if (!string.IsNullOrEmpty(barcodeS) && int.Parse(barcodeS) > 0)
            {
                EnableApproval = true;
            }
            else
            {
                EnableApproval = false;
            }

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
            center.AddFormattedText("Petty Cash Summary\n", TextFormat.Bold).Font.Size = 10;
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

            dataTable.AddColumn("3.5cm"); // Type
            dataTable.AddColumn("2.5cm"); // Date
            dataTable.AddColumn("2.5cm"); // Num
            dataTable.AddColumn("4.5cm"); // Account
            dataTable.AddColumn("3cm");   // Amount
            dataTable.AddColumn("3cm");   // Balance

            // Header row
            Row header = dataTable.AddRow();
            string[] headers = { "Type", "Date", "Num", "Account", "Amount", "Balance" };
            for (int i = 0; i < headers.Length; i++)
            {
                var cell = header.Cells[i];
                cell.AddParagraph(headers[i]);
                cell.Format.Font.Bold = true;
                cell.Format.Font.Size = 9;
                cell.Borders.Bottom.Width = 0.5;
                cell.Format.Alignment = (i >= 4) ? ParagraphAlignment.Right : ParagraphAlignment.Left;
            }

            // Data rows
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string type = row.Cells["Type"]?.Value?.ToString() ?? "";
                string date = row.Cells["Date"]?.Value?.ToString() ?? "";
                string num = row.Cells["Num"]?.Value?.ToString() ?? "";
                string account = row.Cells["Account"]?.Value?.ToString() ?? "";
                string amountStr = row.Cells["Amount"]?.Value?.ToString()?.Replace("◀", "").Trim() ?? "0";
                string balance = row.Cells["Balance"]?.Value?.ToString()?.Replace("◀", "").Trim() ?? "0";

                if (account.ToUpper().Contains("TOTAL")) continue;

                Row dataRow = dataTable.AddRow();
                dataRow.Cells[0].AddParagraph(type).Format.Font.Size = 9;
                dataRow.Cells[1].AddParagraph(date).Format.Font.Size = 9;
                dataRow.Cells[2].AddParagraph(num).Format.Font.Size = 9;
                dataRow.Cells[3].AddParagraph(account).Format.Font.Size = 9;
                dataRow.Cells[4].AddParagraph(amountStr).Format.Alignment = ParagraphAlignment.Right;
                dataRow.Cells[5].AddParagraph(balance).Format.Alignment = ParagraphAlignment.Right;
                decimal amt;
                decimal.TryParse(amountStr, out amt);
                totalAmount += amt;
            }

            // Total row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[3].AddParagraph("TOTAL").Format.Font.Bold = true;
            Paragraph totalAmountCell = totalRow.Cells[4].AddParagraph(totalAmount.ToString("N2"));
            totalAmountCell.Format.Font.Bold = true;
            totalAmountCell.Format.Font.Underline = Underline.Single;
            totalRow.Cells[4].Format.Alignment = ParagraphAlignment.Right;
            totalRow.Cells[5].AddParagraph(""); // Leave balance cell blank

            // Export
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Save PDF";
                saveDialog.Filter = "PDF files (*.pdf)|*.pdf";
                saveDialog.FileName = "PettyCash.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(saveDialog.FileName);
                    Process.Start("explorer.exe", saveDialog.FileName);
                }
            }
        }
        private void LoadData()
        {
            dgvSales.Rows.Clear();
            if (!EnableApproval)
            {
                if (dgvSales.Columns.Count == 0)
                {
                    dgvSales.Columns.Add("id", "");
                    dgvSales.Columns.Add("Type", "Type");
                    dgvSales.Columns.Add("Date", "Date");
                    dgvSales.Columns.Add("Num", "Num");
                    dgvSales.Columns.Add("Account", "Account");
                    dgvSales.Columns.Add("Amount", "Amount");
                    dgvSales.Columns.Add("Balance", "Balance");
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["Account"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvSales.Columns["Amount"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                    dgvSales.Columns["Account"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);


                    dgvSales.Columns["id"].Visible = false;
                    dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    dgvSales.EnableHeadersVisualStyles = false;
                    dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgvSales.GridColor = System.Drawing.Color.LightGray;
                    dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    dgvSales.RowHeadersVisible = false;
                }

                decimal totalAmount = 0, totalBalance = 0;
                List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("id", id),
                DBClass.CreateParameter("startDate", dateTimePicker1.Value.Date),
                DBClass.CreateParameter("endDate", dateTimePicker2.Value.Date)
            };
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                        DATE_FORMAT(t.date, '%Y-%m-%d') AS `Date`,
                        t.transaction_id,

                        -- Display reference for both types
                        CASE 
                        WHEN t.type = 'Petty Cash Request' THEN 
                            (SELECT CONCAT('Petty Cash Approval - REF - ', pr.request_ref)
                             FROM tbl_petty_cash_request pr 
                             WHERE pr.id = t.transaction_id
                             LIMIT 1)
                        WHEN t.type = 'Petty Cash Submission' THEN 
                            'Petty Cash Submission NO.'
                        WHEN t.type = 'Employee Petty Cash Payment' THEN 
                            'Employee Petty Cash Payment'
                        ELSE ''
                        END AS `Num`,
    
                        t.type AS `Type`,
                        coa.code AS `A/C CODE`,
                        coa.name AS `A/C NAME`,

                        -- Amount (debit - credit)
                        (t.credit - t.debit) AS `Amount`,

                        -- Running Balance per hum_id
                        SUM(t.credit - t.debit) OVER (
                            PARTITION BY t.hum_id
                            ORDER BY t.date, t.id
                        ) AS `Balance`

                    FROM 
                        tbl_transaction t
                    JOIN 
                        tbl_coa_level_4 coa ON t.account_id = coa.id

                    WHERE 
                        t.hum_id = @id
                        AND t.type IN ('Petty Cash Request', 'Petty Cash Submission', 'Employee Petty Cash Payment')
                        AND t.date >= @startDate AND t.date <= @endDate
                    ORDER BY 
                        t.date, t.id;",
                     parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        if (reader["Date"] == DBNull.Value)
                            continue;

                        decimal amount = Convert.ToDecimal(reader["AMOUNT"]);
                        decimal balance = Convert.ToDecimal(reader["BALANCE"]);


                        int rowIndex = dgvSales.Rows.Add(reader["transaction_id"].ToString(), reader["Type"].ToString(), reader["date"].ToString(), reader["Num"].ToString(), reader["A/C NAME"].ToString(), reader["AMOUNT"].ToString(), balance.ToString("N2") + " ◀");
                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                        totalAmount += amount;
                        totalBalance += balance;
                    }
                }

                // Add total row
                int totalRow = dgvSales.Rows.Add(null, null, null, null, "TOTAL", totalAmount.ToString("N2")/*, totalAmount.ToString("N2")*/);
                dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
                dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.Columns["id"].Visible = false;
                dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else
            {
                dgvSales.Columns.Clear();

                if (dgvSales.Columns.Count == 0)
                {
                    dgvSales.Columns.Add("id", "");
                    dgvSales.Columns.Add("Type", "Type");
                    dgvSales.Columns.Add("Date", "Date");
                    dgvSales.Columns.Add("Num", "Num");
                    dgvSales.Columns.Add("Account", "Account");
                    dgvSales.Columns.Add("Amount", "Amount");
                    dgvSales.Columns.Add("Balance", "Balance");

                    LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                    dgvSales.Columns["Account"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvSales.Columns["Amount"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                    dgvSales.Columns["Account"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                    dgvSales.Columns["id"].Visible = false;
                    dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvSales.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold);
                    dgvSales.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                    dgvSales.EnableHeadersVisualStyles = false;
                    dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    dgvSales.GridColor = System.Drawing.Color.LightGray;
                    dgvSales.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    dgvSales.CellBorderStyle = DataGridViewCellBorderStyle.None;
                    dgvSales.RowHeadersVisible = false;
                }

                decimal totalAmount = 0;

                List<MySqlParameter> parameters = new List<MySqlParameter>
                {
                    DBClass.CreateParameter("empCode", id), // Assuming `id` holds employee code
                    DBClass.CreateParameter("startDate", dateTimePicker1.Value.Date),
                    DBClass.CreateParameter("endDate", dateTimePicker2.Value.Date)
                };

                using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                        SELECT 
                            p.id,
                            p.voucher_date AS `Date`,
                            'Petty Cash Request' AS `Type`,
                            CONCAT('Petty Cash Approval - REF - ', p.id) AS `Num`,
                            e.name AS `Account`,
                            pd.amount AS `Amount`,
                            '' AS `Balance`
                        FROM 
                            tbl_petty_cash p
                        INNER JOIN 
                            tbl_employee e ON p.employee_id = e.id
                        INNER JOIN 
                            tbl_petty_cash_details pd ON pd.petty_cash_id = p.id
                        INNER JOIN 
                            tbl_petty_cash_card pcc ON CAST(pcc.name AS UNSIGNED) = e.id
                        WHERE 
                            -- e.code = @empCode AND 
                            p.voucher_date >= @startDate AND p.voucher_date <= @endDate
                        ORDER BY 
                            p.voucher_date, p.id;
                    ", parameters.ToArray()))
                {
                    while (reader.Read())
                    {
                        if (reader["Date"] == DBNull.Value)
                            continue;

                        decimal amount = Convert.ToDecimal(reader["Amount"]);

                        int rowIndex = dgvSales.Rows.Add(
                            reader["id"].ToString(),
                            reader["Type"].ToString(),
                            Convert.ToDateTime(reader["Date"]).ToString("yyyy-MM-dd"),
                            reader["Num"].ToString(),
                            reader["Account"].ToString(),
                            amount.ToString("N2"),
                            "" // No running balance in this query
                        );

                        dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                        dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                        totalAmount += amount;
                    }
                }

                // Add total row
                int totalRow = dgvSales.Rows.Add(null, null, null, null, "TOTAL", totalAmount.ToString("N2"));
                dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
                dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
                dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                dgvSales.Columns["id"].Visible = false;
                dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            }
        }

        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                    if (EnableApproval)
                    {
                        //if(_type == "Petty Cash") { }
                        frmLogin.frmMain.openChildForm(new frmViewPettyCashVoucher(_id));
                    }
                    else
                    {
                        if (_type == "Petty Cash Request")
                        {
                            frmLogin.frmMain.openChildForm(new frmPettyCashRequest(_id));
                        }
                        else if (_type == "Petty Cash Submission")
                        {
                            frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(_id));
                        }
                        else if (_type == "Employee Petty Cash Payment")
                        {
                            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(_id));

                        }
                    }
                }
            }
        }
        private void btn_Excel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "PettyCashDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Petty Cash Details";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "G1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Adding Column Headers
                    worksheet.Cells[2, 1] = "ID";
                    worksheet.Cells[2, 2] = "Type";
                    worksheet.Cells[2, 3] = "Date";
                    worksheet.Cells[2, 4] = "Num";
                    worksheet.Cells[2, 5] = "Account";
                    worksheet.Cells[2, 6] = "Amount";
                    worksheet.Cells[2, 7] = "Balance";

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string id = row.Cells["id"].Value?.ToString();
                        string type = row.Cells["Type"].Value?.ToString();
                        string date = row.Cells["Date"].Value?.ToString();
                        string num = row.Cells["Num"].Value?.ToString();
                        string account = row.Cells["Account"].Value?.ToString();
                        string amount = row.Cells["Amount"].Value?.ToString();
                        string balance = row.Cells["Balance"].Value?.ToString();

                        worksheet.Cells[rowIndex, 1] = id;
                        worksheet.Cells[rowIndex, 2] = type;
                        worksheet.Cells[rowIndex, 3] = date;
                        worksheet.Cells[rowIndex, 4] = num;
                        worksheet.Cells[rowIndex, 5] = account;
                        worksheet.Cells[rowIndex, 6] = amount;
                        worksheet.Cells[rowIndex, 7] = balance;

                        // Formatting the cells
                        var accountCell = worksheet.Cells[rowIndex, 5];
                        var amountCell = worksheet.Cells[rowIndex, 6];

                        accountCell.Font.Name = "Times New Roman";
                        amountCell.Font.Name = "Times New Roman";

                        // If account contains 'TOTAL', apply special formatting
                        if (account.ToUpper().Contains("TOTAL"))
                        {
                            accountCell.Font.Size = 10;
                            accountCell.Font.Bold = true;

                            amountCell.Font.Size = 10;
                            amountCell.Font.Bold = true;
                            amountCell.Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }
                        else
                        {
                            accountCell.Font.Size = 10;
                            accountCell.Font.Bold = true;

                            amountCell.Font.Size = 9;
                        }

                        amountCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

                        rowIndex++;
                    }

                    worksheet.Columns[1].AutoFit();
                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();
                    worksheet.Columns[5].AutoFit();
                    worksheet.Columns[6].AutoFit();
                    worksheet.Columns[7].AutoFit();

                    // Save the workbook and close
                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close();
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
