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
    public partial class frmPDCClearedHistory : Form
    {
        public frmPDCClearedHistory()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmPDCClearedHistory_Load(object sender, EventArgs e)
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
            dgvSales.Columns.Add("PassDate", "Pass Date");
            dgvSales.Columns.Add("Amount", "Amount");

            LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

            dgvSales.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSales.Columns["Amount"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["CheckNo"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["CheckDate"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
            dgvSales.Columns["PassDate"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

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
                        cd.check_no,
                        cd.check_date,
                        cd.pass_date,
                        cd.amount
                    FROM tbl_check_details cd
                    WHERE cd.state = 'Pass';
                "))
            {
                int rowId = 1;
                while (reader.Read())
                {
                    string checkNo = reader["check_no"].ToString();
                    string checkDate = reader["check_date"] != DBNull.Value ? Convert.ToDateTime(reader["check_date"]).ToString("dd/MM/yyyy") : "";
                    string passDate = reader["pass_date"] != DBNull.Value ? Convert.ToDateTime(reader["pass_date"]).ToString("dd/MM/yyyy") : "";
                    decimal amount = reader["amount"] != DBNull.Value ? Convert.ToDecimal(reader["amount"]) : 0;

                    int rowIndex = dgvSales.Rows.Add(rowId, checkNo, checkDate, passDate, amount.ToString("N2"));
                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                    totalAmount += amount;
                    rowId++;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add("", "", "", "TOTAL", totalAmount.ToString("N2"));
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
            dataTable.AddColumn("3cm"); // Check No
            dataTable.AddColumn("3.5cm"); // Check Date
            dataTable.AddColumn("3.5cm"); // Pass Date
            dataTable.AddColumn("4cm"); // Amount

            // Add header row
            Row header = dataTable.AddRow();
            header.Cells[0].AddParagraph("Check No").Format.Font.Bold = true;
            header.Cells[1].AddParagraph("Check Date").Format.Font.Bold = true;
            header.Cells[2].AddParagraph("Pass Date").Format.Font.Bold = true;
            header.Cells[3].AddParagraph("Amount").Format.Font.Bold = true;

            decimal totalAmount = 0;

            // Load from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string checkNo = row.Cells["CheckNo"].Value?.ToString() ?? "";
                string checkDate = row.Cells["CheckDate"].Value?.ToString() ?? "";
                string passDate = row.Cells["PassDate"].Value?.ToString() ?? "";
                string amountStr = row.Cells["Amount"].Value?.ToString() ?? "0";

                if (passDate.ToUpper().Contains("TOTAL")) continue;

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(checkNo);
                tRow.Cells[1].AddParagraph(checkDate);
                tRow.Cells[2].AddParagraph(passDate);
                tRow.Cells[3].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                totalAmount += amount;
            }

            // TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 2;
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;
            Paragraph totalText = totalRow.Cells[3].AddParagraph(totalAmount.ToString("N2"));
            totalText.Format.Font.Bold = true;
            totalText.Format.Font.Underline = Underline.Single;
            totalRow.Cells[3].Format.Alignment = ParagraphAlignment.Right;

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
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "E1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers
                    worksheet.Cells[2, 2] = "Check No";
                    worksheet.Cells[2, 3] = "Check Date";
                    worksheet.Cells[2, 4] = "Pass Date";
                    worksheet.Cells[2, 5] = "Amount";
                    for (int i = 2; i <= 5; i++)
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
                        string passDate = row.Cells["PassDate"].Value?.ToString();
                        string amount = row.Cells["Amount"].Value?.ToString();

                        var checkNoCell = worksheet.Cells[rowIndex, 2];
                        var checkDateCell = worksheet.Cells[rowIndex, 3];
                        var passDateCell = worksheet.Cells[rowIndex, 4];
                        var amountCell = worksheet.Cells[rowIndex, 5];

                        checkNoCell.Value = checkNo;
                        checkDateCell.Value = checkDate;
                        passDateCell.Value = passDate;
                        amountCell.Value = amount;

                        checkNoCell.Font.Name = "Times New Roman";
                        checkDateCell.Font.Name = "Times New Roman";
                        passDateCell.Font.Name = "Times New Roman";
                        amountCell.Font.Name = "Times New Roman";

                        if (passDate != null && passDate.ToUpper().Contains("TOTAL"))
                        {
                            checkNoCell.Font.Bold = true;
                            checkDateCell.Font.Bold = true;
                            passDateCell.Font.Bold = true;
                            amountCell.Font.Bold = true;

                            checkNoCell.Font.Size = 10;
                            checkDateCell.Font.Size = 10;
                            passDateCell.Font.Size = 10;
                            amountCell.Font.Size = 10;

                            amountCell.Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                        }
                        else
                        {
                            checkNoCell.Font.Size = 10;
                            checkDateCell.Font.Size = 10;
                            passDateCell.Font.Size = 10;
                            amountCell.Font.Size = 9;
                        }

                        amountCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        rowIndex++;
                    }

                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();
                    worksheet.Columns[4].AutoFit();
                    worksheet.Columns[5].AutoFit();

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