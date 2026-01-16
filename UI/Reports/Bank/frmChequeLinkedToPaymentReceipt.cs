using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmChequeLinkedToPaymentReceipt : Form
    {
        public frmChequeLinkedToPaymentReceipt()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmChequeLinkedToPaymentReceipt_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            LoadData();
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
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
            dgvSales.Columns.Clear();

            // Set up columns to match the query result
            dgvSales.Columns.Add("id", "ID");
            dgvSales.Columns.Add("CheckNo", "Check No");
            dgvSales.Columns.Add("CheckDate", "Check Date");
            dgvSales.Columns.Add("Amount", "Amount");
            dgvSales.Columns.Add("CheckType", "Check Type");
            dgvSales.Columns.Add("PaymentVoucher", "Payment Voucher");
            dgvSales.Columns.Add("ReceiptVoucher", "Receipt Voucher");

            LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

            dgvSales.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSales.Columns["Amount"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["CheckNo"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["CheckDate"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["CheckType"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["PaymentVoucher"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["ReceiptVoucher"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

            dgvSales.Columns["id"].Visible = false;

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

            decimal totalAmount = 0;

            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                    SELECT 
                        ifnull(cd.check_no,'') check_no,
                        cd.check_date,
                        cd.amount,
                        cd.check_type,
                        ifnull(pv.code,'') AS PaymentVoucher,
                        ifnull(rv.code,'') AS ReceiptVoucher
                    FROM tbl_check_details cd
                    LEFT JOIN tbl_payment_voucher pv ON cd.pvc_no = pv.id AND cd.check_type = 'Payment'
                    LEFT JOIN tbl_receipt_voucher rv ON cd.pvc_no = rv.id AND cd.check_type = 'Receipt';
                "))
            {
                int rowId = 1;
                while (reader.Read())
                {
                    string checkNo = reader["check_no"].ToString();
                    string checkDate = reader["check_date"] != DBNull.Value ? Convert.ToDateTime(reader["check_date"]).ToString("dd/MM/yyyy") : "";
                    decimal amount = reader["amount"] != DBNull.Value ? Convert.ToDecimal(reader["amount"]) : 0;
                    string checkType = reader["check_type"]?.ToString() ?? "";
                    string paymentVoucher = reader["PaymentVoucher"]?.ToString() ?? "";
                    string receiptVoucher = reader["ReceiptVoucher"]?.ToString() ?? "";

                    int rowIndex = dgvSales.Rows.Add(rowId, checkNo, checkDate, amount.ToString("N2"), checkType, paymentVoucher, receiptVoucher);
                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                    totalAmount += amount;
                    rowId++;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add("", "", "", totalAmount.ToString("N2"), "", "", "");
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.Rows[totalRow].Cells[3].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
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
            // Adjust page margins to position content to top-left
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Create a header table with 3 columns
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");  // Time/Date
            headerTable.AddColumn("8cm");  // Title Center
            headerTable.AddColumn("5cm");  // Empty for spacing

            Row headerRow = headerTable.AddRow();

            // Left cell - Time & Date (Top-left aligned, Times New Roman)
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            left.Format.SpaceAfter = 0;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell - Company Name & Report Titles
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.Format.SpaceAfter = 0;

            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;

            FormattedText summaryText = center.AddFormattedText("PDC Cleared History\n", TextFormat.Bold);
            summaryText.Font.Size = 12;

            FormattedText allTransText = center.AddFormattedText("All Cleared PDCs", TextFormat.NotBold);
            allTransText.Font.Size = 9;

            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // Bold line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0;
            dataTable.AddColumn("2.5cm"); // Check No
            dataTable.AddColumn("2.5cm"); // Check Date
            dataTable.AddColumn("2.5cm"); // Amount
            dataTable.AddColumn("2.5cm"); // Check Type
            dataTable.AddColumn("3cm");   // Payment Voucher
            dataTable.AddColumn("3cm");   // Receipt Voucher

            // Add header row
            Row header = dataTable.AddRow();
            header.Cells[0].AddParagraph("Check No").Format.Font.Bold = true;
            header.Cells[1].AddParagraph("Check Date").Format.Font.Bold = true;
            header.Cells[2].AddParagraph("Amount").Format.Font.Bold = true;
            header.Cells[3].AddParagraph("Check Type").Format.Font.Bold = true;
            header.Cells[4].AddParagraph("Payment Voucher").Format.Font.Bold = true;
            header.Cells[5].AddParagraph("Receipt Voucher").Format.Font.Bold = true;

            decimal totalAmount = 0;

            // Load from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string checkNo = row.Cells["CheckNo"].Value?.ToString() ?? "";
                string checkDate = row.Cells["CheckDate"].Value?.ToString() ?? "";
                string amountStr = row.Cells["Amount"].Value?.ToString() ?? "0";
                string checkType = row.Cells["CheckType"].Value?.ToString() ?? "";
                string paymentVoucher = row.Cells["PaymentVoucher"].Value?.ToString() ?? "";
                string receiptVoucher = row.Cells["ReceiptVoucher"].Value?.ToString() ?? "";

                // Skip total row
                if (checkDate.ToUpper().Contains("TOTAL")) continue;

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(checkNo);
                tRow.Cells[1].AddParagraph(checkDate);
                tRow.Cells[2].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[3].AddParagraph(checkType);
                tRow.Cells[4].AddParagraph(paymentVoucher);
                tRow.Cells[5].AddParagraph(receiptVoucher);

                totalAmount += amount;
            }

            // TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 1;
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;
            Paragraph totalText = totalRow.Cells[2].AddParagraph(totalAmount.ToString("N2"));
            totalText.Format.Font.Bold = true;
            totalText.Format.Font.Underline = Underline.Single;
            totalRow.Cells[2].Format.Alignment = ParagraphAlignment.Right;
            // Leave other cells empty for total row

            // Render and save
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PDCClearedHistory.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "PDCClearedHistory.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "PDC Cleared";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "G1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers
                    worksheet.Cells[2, 2] = "Check No";
                    worksheet.Cells[2, 3] = "Check Date";
                    worksheet.Cells[2, 4] = "Amount";
                    worksheet.Cells[2, 5] = "Check Type";
                    worksheet.Cells[2, 6] = "Payment Voucher";
                    worksheet.Cells[2, 7] = "Receipt Voucher";
                    for (int i = 2; i <= 7; i++)
                    {
                        var cell = worksheet.Cells[2, i];
                        cell.Font.Bold = true;
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 10;
                        cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string checkNo = row.Cells["CheckNo"].Value?.ToString();
                        string checkDate = row.Cells["CheckDate"].Value?.ToString();
                        string amount = row.Cells["Amount"].Value?.ToString();
                        string checkType = row.Cells["CheckType"].Value?.ToString();
                        string paymentVoucher = row.Cells["PaymentVoucher"].Value?.ToString();
                        string receiptVoucher = row.Cells["ReceiptVoucher"].Value?.ToString();

                        var checkNoCell = worksheet.Cells[rowIndex, 2];
                        var checkDateCell = worksheet.Cells[rowIndex, 3];
                        var amountCell = worksheet.Cells[rowIndex, 4];
                        var checkTypeCell = worksheet.Cells[rowIndex, 5];
                        var paymentVoucherCell = worksheet.Cells[rowIndex, 6];
                        var receiptVoucherCell = worksheet.Cells[rowIndex, 7];

                        checkNoCell.Value = checkNo;
                        checkDateCell.Value = checkDate;
                        amountCell.Value = amount;
                        checkTypeCell.Value = checkType;
                        paymentVoucherCell.Value = paymentVoucher;
                        receiptVoucherCell.Value = receiptVoucher;

                        checkNoCell.Font.Name = "Times New Roman";
                        checkDateCell.Font.Name = "Times New Roman";
                        amountCell.Font.Name = "Times New Roman";
                        checkTypeCell.Font.Name = "Times New Roman";
                        paymentVoucherCell.Font.Name = "Times New Roman";
                        receiptVoucherCell.Font.Name = "Times New Roman";

                        // Detect total row by checking if CheckDate cell contains "TOTAL"
                        if (checkDate != null && checkDate.ToUpper().Contains("TOTAL"))
                        {
                            checkNoCell.Font.Bold = true;
                            checkDateCell.Font.Bold = true;
                            amountCell.Font.Bold = true;
                            checkTypeCell.Font.Bold = true;
                            paymentVoucherCell.Font.Bold = true;
                            receiptVoucherCell.Font.Bold = true;

                            checkNoCell.Font.Size = 10;
                            checkDateCell.Font.Size = 10;
                            amountCell.Font.Size = 10;
                            checkTypeCell.Font.Size = 10;
                            paymentVoucherCell.Font.Size = 10;
                            receiptVoucherCell.Font.Size = 10;

                            amountCell.Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }
                        else
                        {
                            checkNoCell.Font.Size = 10;
                            checkDateCell.Font.Size = 10;
                            amountCell.Font.Size = 9;
                            checkTypeCell.Font.Size = 10;
                            paymentVoucherCell.Font.Size = 10;
                            receiptVoucherCell.Font.Size = 10;
                        }

                        amountCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        rowIndex++;
                    }

                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();
                    worksheet.Columns[5].AutoFit();
                    worksheet.Columns[6].AutoFit();
                    worksheet.Columns[7].AutoFit();

                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (Utilities.UserPermissionCheck("CustomerBalanceSummary"))
            //{
                int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
                //frmLogin.frmMain.openChildForm(new frmCustomerBalanceDetails(id));
            //}
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            //LoadData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}