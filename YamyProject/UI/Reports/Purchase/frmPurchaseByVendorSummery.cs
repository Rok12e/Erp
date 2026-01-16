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
    public partial class frmPurchaseByVendorSummary : Form
    {
        bool isSubcontractors;
        public frmPurchaseByVendorSummary(bool isSubcontractors = false)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.isSubcontractors = isSubcontractors;
            headerUC1.FormText = this.Text;
        }

        private void frmPurchaseByVendorSummary_Load(object sender, EventArgs e)
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

            if (dgvSales.Columns.Count == 0)
            {
                dgvSales.Columns.Add("id", "id");
                dgvSales.Columns["id"].Visible = false;
                dgvSales.Columns.Add("sn", "SN");
                dgvSales.Columns.Add("name", "Name");
                dgvSales.Columns.Add("debit", "Net Sales");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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

            string query = @"
                            SELECT 
                            ROW_NUMBER() OVER(ORDER BY c.id) AS SN,
                            c.id AS vendor_id,
                            c.name AS vendor_name,
                            SUM(s.net) AS `Total Purchase`
                            FROM 
                            tbl_purchase s
                            JOIN 
                            tbl_vendor c ON s.vendor_id = c.id
                            and s.state = 0
                            AND s.date >= @startDate AND s.date<= @endDate
                            WHERE c.type = 'Vendor'
                            GROUP BY 
                            c.id, c.name
                            ORDER BY 
                            c.id;";
            if (isSubcontractors)
            {
                // Modify query for subcontractors
                query = @"
                            SELECT 
                            ROW_NUMBER() OVER(ORDER BY c.id) AS SN,
                            c.id AS vendor_id,
                            c.name AS vendor_name,
                            SUM(s.net) AS `Total Purchase`
                            FROM 
                            tbl_purchase s
                            JOIN 
                            tbl_vendor c ON s.vendor_id = c.id
                            and s.state = 0
                            AND s.date >= @startDate AND s.date<= @endDate
                            WHERE c.type = 'Subcontractor'
                            GROUP BY 
                            c.id, c.name
                            ORDER BY 
                            c.id;";
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader(query,
                DBClass.CreateParameter("startDate", dtpFrom.Value.Date),
                DBClass.CreateParameter("endDate", dtpTo.Value.Date)))
            {
                while (reader.Read())
                {
                    int sn = Convert.ToInt32(reader["SN"]);
                    string name = reader["vendor_name"].ToString();
                    decimal netSales = Convert.ToDecimal(reader["Total Purchase"]);
                    int rowIndex = dgvSales.Rows.Add(reader["vendor_id"].ToString(), sn, name, netSales.ToString("N2"), "0.00", "0.00");

                    dgvSales.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    dgvSales.Rows[rowIndex].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

                    totalAmount += netSales;
                }
            }

            // Add total row
            int totalRow = dgvSales.Rows.Add("", "", "TOTAL", totalAmount.ToString("N2"), "", "");
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
            {
                if (reader.Read())
                    companyName = reader["name"].ToString();
            }

            // Create PDF document
            Document doc = new Document();
            Section section = doc.AddSection();

            // Adjust margins
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
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            left.Format.Alignment = ParagraphAlignment.Left;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("Purchase By Vendor Summary\n", TextFormat.Bold).Font.Size = 12;
            center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;

            //section.Add(headerTable); // ✅ Add header

            // Separator
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Main table
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0.75;
            dataTable.Format.Font.Name = "Times New Roman";
            dataTable.Format.Font.Size = 9;

            dataTable.AddColumn("2cm"); // SN
            dataTable.AddColumn("11cm"); // Name
            dataTable.AddColumn("6cm");   // Net Sales

            Row header = dataTable.AddRow();
            header.Shading.Color = Colors.LightGray;
            header.Format.Font.Bold = true;
            header.Cells[0].AddParagraph("SN");
            header.Cells[1].AddParagraph("Name");
            header.Cells[2].AddParagraph("Net Sales");

            decimal totalAmount = 0;

            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string sn = row.Cells["sn"].Value?.ToString() ?? "";
                string name = row.Cells["name"].Value?.ToString() ?? "";
                string salesStr = row.Cells["debit"].Value?.ToString() ?? "0";

                if (name.ToUpper().Contains("TOTAL")) continue;

                decimal amount;
                decimal.TryParse(salesStr, out amount);
                totalAmount += amount;

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(sn);
                tRow.Cells[1].AddParagraph(name);
                tRow.Cells[2].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
            }

            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].MergeRight = 1;
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;

            Paragraph totalText = totalRow.Cells[2].AddParagraph();
            totalText.Format.Alignment = ParagraphAlignment.Right;
            totalText.AddFormattedText(totalAmount.ToString("N2"), TextFormat.Bold).Font.Underline = Underline.Single;

            //section.Add(dataTable); // ✅ Add main table only once

            // Let user choose path
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveDialog.Title = "Save Vendor Summary PDF";
                saveDialog.FileName = "PurchaseByVendorSummary.pdf";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                    renderer.Document = doc;
                    renderer.RenderDocument();
                    renderer.PdfDocument.Save(saveDialog.FileName);
                    Process.Start("explorer.exe", saveDialog.FileName);
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "PurchaseByVendorBalanceSummary.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Purchase By Vendor Balance";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "C1"];
                    headerRange.Merge();
                    headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yyyy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    // Add column headers (row 2)
                    worksheet.Cells[2, 1] = "SN";
                    worksheet.Cells[2, 2] = "Name";
                    worksheet.Cells[2, 3] = "Net Sales";

                    for (int i = 1; i <= 3; i++)
                    {
                        var headerCell = worksheet.Cells[2, i];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    }

                    int rowIndex = 3;
                    decimal totalAmount = 0;

                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string sn = row.Cells["sn"].Value?.ToString() ?? "";
                        string name = row.Cells["name"].Value?.ToString() ?? "";
                        string netSalesStr = row.Cells["debit"].Value?.ToString() ?? "0";

                        if (name.ToUpper().Contains("TOTAL"))
                            continue;

                        decimal netSales = 0;
                        decimal.TryParse(netSalesStr, out netSales);
                        totalAmount += netSales;

                        worksheet.Cells[rowIndex, 1] = sn;
                        worksheet.Cells[rowIndex, 2] = name;
                        worksheet.Cells[rowIndex, 3] = netSales.ToString("N2");

                        for (int col = 1; col <= 3; col++)
                        {
                            var cell = worksheet.Cells[rowIndex, col];
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;
                            if (col == 3)
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                        }

                        rowIndex++;
                    }

                    // Add TOTAL row
                    worksheet.Cells[rowIndex, 1] = "";
                    worksheet.Cells[rowIndex, 2] = "TOTAL";
                    worksheet.Cells[rowIndex, 3] = totalAmount.ToString("N2");

                    worksheet.Cells[rowIndex, 2].Font.Bold = true;
                    worksheet.Cells[rowIndex, 3].Font.Bold = true;
                    worksheet.Cells[rowIndex, 3].Font.Underline = Microsoft.Office.Interop.Excel.XlUnderlineStyle.xlUnderlineStyleDouble;
                    worksheet.Cells[rowIndex, 3].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

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
            //if (Utilities.UserPermissionCheck("CustomerBalanceSummary"))
            //{
                int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
                frmLogin.frmMain.openChildForm(new frmPurchaseByVendorDetails(id));
            //}
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