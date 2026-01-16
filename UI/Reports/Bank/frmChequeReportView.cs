using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace YamyProject.UI.Reports.Bank
{
    public partial class frmChequeReportView : Form
    {

        public frmChequeReportView()
        {
            InitializeComponent();
            headerUC1.FormText = this.Text;
        }

        private void frmChequeReportView_Load(object sender, EventArgs e)
        {
            DataTable dt = DBClass.ExecuteDataTable("SELECT distinct check_no FROM tbl_check_details WHERE check_no > 0;");
            cmbCheque.DataSource = dt;
            cmbCheque.DisplayMember = "check_no";
            cmbCheque.ValueMember = "check_no";
            cmbCheque.SelectedIndex = -1;
        }

        private void ShowData(int chequeNo)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT
                                                                        cd.pvc_no AS id,
                                                                        cd.check_type as type,
                                                                        cd.date,
                                                                        coa.name AS account_name,
                                                                        cd.amount AS amount,
                                                                        b.name AS bank_name
                                                                    FROM tbl_check_details cd
                                                                    JOIN tbl_bank_card bc ON cd.check_id = bc.id
                                                                    JOIN tbl_bank b ON bc.bank_id = b.id
                                                                    JOIN tbl_coa_level_4 coa ON coa.id = bc.account_id
                                                                    WHERE cd.check_no = @id;",
                                                                      DBClass.CreateParameter("id", chequeNo)))
            {
                if (reader.HasRows)
                {
                    int rowIndex = 1;
                    while (reader.Read())
                    {
                        dgvData.Rows.Add(
                            rowIndex++,
                            Convert.ToDateTime(reader["date"]).ToShortDateString(),
                            reader["id"].ToString(),
                            reader["type"].ToString(),
                            reader["account_name"].ToString(),
                            Convert.ToDecimal(reader["amount"]).ToString("N2"),
                            reader["bank_name"].ToString()
                        );
                    }
                }
                else
                {
                    MessageBox.Show("No data found for the given cheque number.");
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            dgvData.Rows.Clear();

            if (string.IsNullOrWhiteSpace(cmbCheque.Text))
            {
                MessageBox.Show("Enter the cheque number");
            }
            else
            {
                ShowData(Convert.ToInt32(cmbCheque.Text));
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "ChequeReport.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Cheque Report";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "F1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers (row 2)
                    worksheet.Cells[2, 1] = "ID";
                    worksheet.Cells[2, 2] = "Check Type";
                    worksheet.Cells[2, 3] = "Date";
                    worksheet.Cells[2, 4] = "Account Name";
                    worksheet.Cells[2, 5] = "Amount";
                    worksheet.Cells[2, 6] = "Bank Name";

                    for (int i = 1; i <= 6; i++)
                    {
                        var headerCell = worksheet.Cells[2, i];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    }

                    int rowIndex = 3; // Start from row 3
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string id = row.Cells["id"].Value?.ToString() ?? "";
                        string checkType = row.Cells["type"].Value?.ToString() ?? "";
                        string date = row.Cells["date"].Value?.ToString() ?? "";
                        string accountName = row.Cells["account_name"].Value?.ToString() ?? "";
                        string amount = row.Cells["amount"].Value?.ToString() ?? "0";
                        string bankName = row.Cells["bank_name"].Value?.ToString() ?? "";

                        worksheet.Cells[rowIndex, 1] = id;
                        worksheet.Cells[rowIndex, 2] = checkType;
                        worksheet.Cells[rowIndex, 3] = date;
                        worksheet.Cells[rowIndex, 4] = accountName;
                        worksheet.Cells[rowIndex, 5] = amount;
                        worksheet.Cells[rowIndex, 6] = bankName;

                        for (int col = 1; col <= 6; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;
                            if (col == 5) // Right align Amount
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

        private void btnPrint_Click(object sender, EventArgs e)
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

            FormattedText summaryText = center.AddFormattedText("Cheque Report\n", TextFormat.Bold);
            summaryText.Font.Size = 12;

            FormattedText allTransText = center.AddFormattedText("Cheque Details", TextFormat.NotBold);
            allTransText.Font.Size = 9;

            // Bold separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for cheque data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.75;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            // Define columns
            dataTable.AddColumn("2cm"); // ID
            dataTable.AddColumn("2.5cm"); // Check Type
            dataTable.AddColumn("2.5cm"); // Date
            dataTable.AddColumn("4cm");   // Account Name
            dataTable.AddColumn("2.5cm"); // Amount
            dataTable.AddColumn("3cm");   // Bank Name

            // Add table header row
            Row tableHeader = dataTable.AddRow();
            tableHeader.Shading.Color = Colors.LightGray;
            tableHeader.Format.Font.Bold = true;
            tableHeader.Cells[0].AddParagraph("ID");
            tableHeader.Cells[1].AddParagraph("Check Type");
            tableHeader.Cells[2].AddParagraph("Date");
            tableHeader.Cells[3].AddParagraph("Account Name");
            tableHeader.Cells[4].AddParagraph("Amount");
            tableHeader.Cells[5].AddParagraph("Bank Name");

            decimal totalAmount = 0;

            // Load data from DataGridView
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.IsNewRow) continue;

                string id = row.Cells["id"].Value?.ToString() ?? "";
                string checkType = row.Cells["type"].Value?.ToString() ?? "";
                string date = row.Cells["date"].Value?.ToString() ?? "";
                string accountName = row.Cells["account_name"].Value?.ToString() ?? "";
                string amountStr = row.Cells["amount"].Value?.ToString() ?? "0";
                string bankName = row.Cells["bank_name"].Value?.ToString() ?? "";

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);
                totalAmount += amount;

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(id);
                tRow.Cells[1].AddParagraph(checkType);
                tRow.Cells[2].AddParagraph(date);
                tRow.Cells[3].AddParagraph(accountName);
                tRow.Cells[4].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[5].AddParagraph(bankName);
            }

            // Add TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 3;
            Paragraph totalLabel = totalRow.Cells[0].AddParagraph("TOTAL");
            totalLabel.Format.Font.Bold = true;

            Paragraph totalText = totalRow.Cells[4].AddParagraph(totalAmount.ToString("N2"));
            totalText.Format.Font.Bold = true;
            totalText.Format.Font.Underline = Underline.Single;
            totalRow.Cells[4].Format.Alignment = ParagraphAlignment.Right;

            // Render and save PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ChequeReport.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            DataGridViewRow row = dgvData.Rows[e.RowIndex];
            int chequeId = Convert.ToInt32(row.Cells["id"].Value); // column name must exist
            string voucherType = row.Cells["type"].Value.ToString();
            string bankName = row.Cells["bank_name"].Value.ToString();

            if (voucherType.Equals("Payment"))
            {
                frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(chequeId));
            }
            else if(voucherType.Equals("Receipt"))
            {
                frmLogin.frmMain.openChildForm(new frmViewReceiptVoucher(chequeId));
            }
        }
    }
}
