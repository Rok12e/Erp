using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmIncomeByVendorSummary : Form
    {
        public frmIncomeByVendorSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmIncomeByVendorSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ";
            LoadSalesData();
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
        }

        private void LoadSalesData()
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            string dateFilter = "";

            if (!chkDate.Checked)
            {
                dateFilter = " AND t.date >= @dateFrom AND t.date <= @dateTo";
                parameters.Add(DBClass.CreateParameter("@dateFrom", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("@dateTo", dtpTo.Value.Date.AddDays(1).AddSeconds(-1)));
            }

            string query = $@"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY Balance DESC) AS SN, 
                                min_id AS id,                                    
                                Name,
                                Balance
                            FROM (
                                SELECT 
                                    h.name AS Name,
                                    SUM(t.debit - t.credit) AS Balance,
                                    MIN(h.id) AS min_id
                                FROM 
                                    tbl_transaction t
                                JOIN 
                                    tbl_vendor h ON t.hum_id = h.id
                                WHERE 
                                    t.type IN ('Purchase Invoice', 'Purchase Invoice Cash', 'Vendor Opening Balance', 'Vendor Payment')
                                    {dateFilter}
                                GROUP BY 
                                    t.hum_id, h.name
                            ) AS sub
                            ORDER BY 
                                Balance DESC;
                        ";

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvSales.DataSource = dt;

            dgvSales.Columns["id"].Visible = false;
            dgvSales.Columns[0].Width = 60;  // SN
            dgvSales.Columns[1].Width = 250; // Vendor Name
            dgvSales.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSales.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvSales.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvSales.Columns[2].DefaultCellStyle.Format = "N2";

            dgvSales.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvSales.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9);

            LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);
        }


        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSales.CurrentRow.Cells["id"].Value == null || dgvSales.CurrentRow.Cells["id"].Value.ToString() == "")
                return;
            int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
            frmLogin.frmMain.openChildForm(new frmIncomeByVendorDetails(id));
        }

        private void btnPrint_Click(object sender, EventArgs e)
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

                // Set margins
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
                center.AddFormattedText("Income By Vendor Summary\n", TextFormat.Bold).Font.Size = 12;
                center.AddFormattedText("All Transactions", TextFormat.NotBold).Font.Size = 9;

                //section.Add(headerTable);

                // Separator line
                Paragraph line = section.AddParagraph();
                line.Format.Borders.Bottom.Width = 2;
                line.Format.SpaceAfter = "0.5cm";

                // Main data table
                Table dataTable = section.AddTable();
                dataTable.Borders.Width = 0.75;
                dataTable.Format.Font.Name = "Times New Roman";
                dataTable.Format.Font.Size = 9;

                dataTable.AddColumn("2cm");  // SN
                dataTable.AddColumn("11cm"); // Name
                dataTable.AddColumn("6cm");  // Total Income

                Row header = dataTable.AddRow();
                header.Shading.Color = Colors.LightGray;
                header.Format.Font.Bold = true;
                header.Cells[0].AddParagraph("SN");
                header.Cells[1].AddParagraph("Vendor Name");
                header.Cells[2].AddParagraph("Total Income");

                decimal totalAmount = 0;

                foreach (DataGridViewRow row in dgvSales.Rows)
                {
                    if (row.IsNewRow) continue;

                    string sn = row.Cells["SN"].Value?.ToString() ?? ""; // Serial Number column
                    string name = row.Cells["Name"].Value?.ToString() ?? "";
                    string incomeStr = row.Cells["Balance"].Value?.ToString() ?? "0";

                    decimal income;
                    decimal.TryParse(incomeStr, out income);
                    totalAmount += income;

                    Row tRow = dataTable.AddRow();
                    tRow.Cells[0].AddParagraph(sn);
                    tRow.Cells[1].AddParagraph(name);
                    tRow.Cells[2].AddParagraph(income.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                }

                Row totalRow = dataTable.AddRow();
                totalRow.Cells[0].MergeRight = 1;
                totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;

                Paragraph totalText = totalRow.Cells[2].AddParagraph();
                totalText.Format.Alignment = ParagraphAlignment.Right;
                totalText.AddFormattedText(totalAmount.ToString("N2"), TextFormat.Bold).Font.Underline = Underline.Single;

                //section.Add(dataTable);

                // Ask user to save file
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                    saveDialog.Title = "Save Income By Vendor Summary";
                    saveDialog.FileName = "IncomeByVendorSummary.pdf";

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
                    saveDialog.FileName = "IncomeByVendorSummary.xlsx";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        var excelApp = new Microsoft.Office.Interop.Excel.Application();
                        excelApp.Visible = false;
                        var workbook = excelApp.Workbooks.Add(Type.Missing);
                        var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                        worksheet.Name = "Income By Vendor";

                        // Header row (Date)
                        Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["A1", "C1"];
                        headerRange.Merge();
                        headerRange.Value = "Date: " + DateTime.Now.ToString("MMM dd, yyyy");
                        headerRange.Font.Bold = true;
                        headerRange.Font.Name = "Times New Roman";
                        headerRange.Font.Size = 10;
                        headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                        // Column headers
                        worksheet.Cells[2, 1] = "SN";
                        worksheet.Cells[2, 2] = "Vendor Name";
                        worksheet.Cells[2, 3] = "Total Income";

                        for (int i = 1; i <= 3; i++)
                        {
                            var headerCell = worksheet.Cells[2, i];
                            headerCell.Font.Bold = true;
                            headerCell.Font.Name = "Times New Roman";
                            headerCell.Font.Size = 10;
                            headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                            headerCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        }

                        int rowIndex = 3;
                        decimal totalAmount = 0;

                        foreach (DataGridViewRow row in dgvSales.Rows)
                        {
                            if (row.IsNewRow) continue;

                            string sn = row.Cells["SN"].Value?.ToString() ?? "";
                            string name = row.Cells["Name"].Value?.ToString() ?? "";
                            string incomeStr = row.Cells["Balance"].Value?.ToString() ?? "0";

                            decimal income = 0;
                            decimal.TryParse(incomeStr, out income);
                            totalAmount += income;

                            worksheet.Cells[rowIndex, 1] = sn;
                            worksheet.Cells[rowIndex, 2] = name;
                            worksheet.Cells[rowIndex, 3] = income.ToString("N2");

                            // Formatting for each row
                            for (int col = 1; col <= 3; col++)
                            {
                                var cell = worksheet.Cells[rowIndex, col];
                                cell.Font.Name = "Times New Roman";
                                cell.Font.Size = 9;

                                if (col == 3)
                                    cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                                else
                                    cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
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
