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
    public partial class frmVendorAgingDetails : Form
    {
        int id;
        public frmVendorAgingDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }
        private void frmVendorAgingDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print";
            LoadSalesData();
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();
            loadCompany();
            LoadSalesData();
        }
        private void loadCompany()
        {
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            }
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
            center.AddFormattedText("Vendor Aging Details\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("As of " + DateTime.Now.ToString("MMMM dd, yyyy"), TextFormat.NotBold).Font.Size = 9;
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";
            Table table = section.AddTable();
            table.Borders.Width = 0.75;
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 9;

            table.AddColumn("1.5cm"); // Type
            table.AddColumn("2.0cm"); // Date
            table.AddColumn("2.0cm"); // Num
            table.AddColumn("2.0cm"); // Voucher
            table.AddColumn("3.5cm"); // Name
            table.AddColumn("2.0cm"); // Terms
            table.AddColumn("2.0cm"); // DueDate
            table.AddColumn("1.5cm"); // Aging
            table.AddColumn("2.5cm"); // Open Balance

            Row header = table.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Format.Font.Bold = true;

            header.Cells[0].AddParagraph("Type");
            header.Cells[1].AddParagraph("Date");
            header.Cells[2].AddParagraph("Num");
            header.Cells[3].AddParagraph("Voucher");
            header.Cells[4].AddParagraph("Name");
            header.Cells[5].AddParagraph("Terms");
            header.Cells[6].AddParagraph("Due Date");
            header.Cells[7].AddParagraph("Aging");
            header.Cells[8].AddParagraph("Open Balance");

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow || !row.Visible) continue;

                Row tRow = table.AddRow();
                tRow.Cells[0].AddParagraph(row.Cells["Type"].Value?.ToString() ?? "");
                tRow.Cells[1].AddParagraph(row.Cells["Date"].Value?.ToString() ?? "");
                tRow.Cells[2].AddParagraph(row.Cells["Num"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[3].AddParagraph(row.Cells["Voucher"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[4].AddParagraph(row.Cells["Name"].Value?.ToString() ?? "");
                tRow.Cells[5].AddParagraph(row.Cells["Terms"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[6].AddParagraph(row.Cells["DueDate"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[7].AddParagraph(row.Cells["Aging"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[8].AddParagraph(row.Cells["OpenigBalance"].Value?.ToString() ?? "").Format.Alignment = ParagraphAlignment.Right;
            }

            // Render PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog saveFile = new SaveFileDialog())
            {
                saveFile.Filter = "PDF Files (*.pdf)|*.pdf";
                saveFile.Title = "Save Vendor Aging Details";
                saveFile.FileName = "VendorAgingDetails.pdf";

                if (saveFile.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(saveFile.FileName);
                    Process.Start("explorer.exe", saveFile.FileName);
                }
            }
        }
        private void LoadSalesData()
        {
            dgvSales.Rows.Clear();

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "");
                dgvSales.Columns.Add("Type", "Type");
                dgvSales.Columns.Add("Date", "Date");
                dgvSales.Columns.Add("Num", "Num");
                dgvSales.Columns.Add("Voucher", "V .No");
                dgvSales.Columns.Add("Name", "Name");
                dgvSales.Columns.Add("Terms", "Terms");
                dgvSales.Columns.Add("DueDate", "Due Date");
                dgvSales.Columns.Add("Aging", "Aging");
                dgvSales.Columns.Add("OpenigBalance", "Opening Balance");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["OpenigBalance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["OpenigBalance"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);
                dgvSales.Columns["Name"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

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
            decimal totalBalance = 0;
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("id", id)
            };

            string dateFilter = "";
            if (!chkDate.Checked)
            {
                dateFilter = " AND t.date >= @dateFrom AND t.date <= @dateTo";
                parameters.Add(DBClass.CreateParameter("dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                            SELECT 
                            ROW_NUMBER() OVER (ORDER BY t.id) AS SN,
                            t.type AS Type,
                            t.date AS Date,
                            t.transaction_id id,
                            t.transaction_id,
                            t.voucher_no,
                            c.name,
                            '30 Days' AS Terms,
                            DATE_ADD(t.date, INTERVAL 30 DAY) AS `Due Date`,
                            DATEDIFF(CURDATE(), t.Date) AS Aging,	
                            t.debit - t.credit AS `Open Balance`
                            FROM 
                            tbl_transaction t
                            JOIN 
                            tbl_vendor c ON t.hum_id = c.id
                            WHERE 
                            c.id = @id
                            AND t.state = 0
                            AND t.type IN ('Vendor Payment','Purchase Invoice','Check Cancel (Vendor)','Purchase Return Invoice','Credit Note','PDC Payable')
                            {dateFilter}
                            ORDER BY 
                            t.date;";

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    if (reader["Date"] == DBNull.Value)
                        continue;

                    decimal balance = Convert.ToDecimal(reader["Open Balance"]);

                    int rowIndex = dgvSales.Rows.Add(
                        reader["id"].ToString(),
                        reader["Type"].ToString(),
                        Convert.ToDateTime(reader["Date"]).ToString("dd/MM/yyyy"),
                        reader["transaction_id"].ToString(),
                        reader["voucher_no"].ToString(),
                        reader["name"].ToString(),
                        reader["Terms"].ToString(),
                        Convert.ToDateTime(reader["Due Date"]).ToString("dd/MM/yyyy"),
                        reader["Aging"].ToString(),
                        balance.ToString("N2")
                    );

                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Regular);

                    totalBalance += balance;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add(null, null, null, null, null, "TOTAL", null, null, null, totalBalance.ToString("N2"));
            dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 8F, FontStyle.Bold | FontStyle.Underline);
            dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvSales.Rows[totalRow].Cells[2].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }
        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                {
                    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                    if (_type.Contains("Sales"))
                    {
                        frmLogin.frmMain.openChildForm(new frmSales(_id, "", id));
                    }
                    //else if(_type == "Vendor Receipt")
                    //{
                    //    frmLogin.frmMain.openChildForm(new frmViewReceiptVoucher(id));
                    //}
                }
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "VendorAgingDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Vendor Aging Details";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "I1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yyyy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Column headers
                    string[] headers = { "Type", "Date", "Num", "Voucher", "Name", "Terms", "Due Date", "Aging", "Open Balance" };
                    for (int i = 0; i < headers.Length; i++)
                    {
                        worksheet.Cells[2, i + 1] = headers[i];
                        var cell = worksheet.Cells[2, i + 1];
                        cell.Font.Bold = true;
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 10;
                        cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    }

                    int rowIndex = 3;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        worksheet.Cells[rowIndex, 1] = row.Cells["Type"].Value?.ToString();
                        worksheet.Cells[rowIndex, 2] = row.Cells["Date"].Value?.ToString();
                        worksheet.Cells[rowIndex, 3] = row.Cells["Num"].Value?.ToString();
                        worksheet.Cells[rowIndex, 4] = row.Cells["Voucher"].Value?.ToString();
                        worksheet.Cells[rowIndex, 5] = row.Cells["Name"].Value?.ToString();
                        worksheet.Cells[rowIndex, 6] = row.Cells["Terms"].Value?.ToString();
                        worksheet.Cells[rowIndex, 7] = row.Cells["DueDate"].Value?.ToString();
                        worksheet.Cells[rowIndex, 8] = row.Cells["Aging"].Value?.ToString();

                        string balance = row.Cells["OpenigBalance"].Value?.ToString() ?? "0";
                        worksheet.Cells[rowIndex, 9] = balance;

                        for (int col = 1; col <= 9; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;

                            if (col >= 6) // Right-align from Terms onward
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            else
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                        }

                        // If it's TOTAL row, apply formatting
                        if ((row.Cells["Name"].Value?.ToString() ?? "").ToUpper() == "TOTAL")
                        {
                            worksheet.Cells[rowIndex, 5].Font.Bold = true;
                            worksheet.Cells[rowIndex, 9].Font.Bold = true;
                            worksheet.Cells[rowIndex, 9].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadSalesData();
            }
        }
    }
}
