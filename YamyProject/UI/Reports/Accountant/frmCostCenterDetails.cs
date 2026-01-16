using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmCostCenterDetails : Form
    {
        int id;
        public frmCostCenterDetails(int _id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = _id;
        }

        private void frmCostCenterDetails_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            DateTime dated = DateTime.Now;
            guna2HtmlLabel11.Text = dated.TimeOfDay.ToString();
            guna2HtmlLabel11.Text = dated.Date.ToShortDateString();
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
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));
        }

        private void Report_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Exporting Report...");
        }
        private void LoadData()
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
                dgvSales.Columns.Add("id", "Id");
                dgvSales.Columns.Add("date", "Date");
                dgvSales.Columns.Add("name", "Description");
                dgvSales.Columns.Add("VoucherNo", "Voucher No");
                dgvSales.Columns.Add("debit", "Debit");
                dgvSales.Columns.Add("credit", "Credit");
                dgvSales.Columns.Add("balance", "Balance");
                LocalizationManager.LocalizeDataGridViewHeaders(dgvSales);

                dgvSales.Columns["debit"].DefaultCellStyle.Alignment = dgvSales.Columns["credit"].DefaultCellStyle.Alignment = dgvSales.Columns["balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvSales.Columns["debit"].DefaultCellStyle.Font = dgvSales.Columns["credit"].DefaultCellStyle.Font = dgvSales.Columns["balance"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);
                dgvSales.Columns["VoucherNo"].DefaultCellStyle.Font = dgvSales.Columns["name"].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

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
            }

            decimal totalAmount = 0;

            string sql = @"
                                SELECT t.id, c.name, DATE_FORMAT(t.date, '%M %d %Y') AS Date, t.`type`, t.debit,
                                       t.credit, t.ref_id, t.description, 0 AS Balance
                                FROM tbl_cost_center c
                                JOIN tbl_cost_center_transaction t ON c.id = t.cost_center_id
                                WHERE c.id = @id
                            ";

            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("id", id)
            };

            if (!chkDate.Checked)
            {
                sql += " AND t.date >= @startDate AND t.date <= @endDate ";
                parameters.Add(DBClass.CreateParameter("startDate", dtpFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("endDate", dtpTo.Value.Date));
            }

            using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    if (reader["Date"] == DBNull.Value)
                        continue;

                    string name = reader["name"].ToString();
                    string date = reader["Date"].ToString();
                    string Inv_No = reader["ref_id"].ToString();
                    decimal balance = Convert.ToDecimal(reader["Balance"] ?? "");
                    decimal debit = reader["Debit"] as decimal? ?? 0;
                    decimal credit = reader["Credit"] as decimal? ?? 0;

                    totalAmount += (debit - credit);
                    dgvSales.Rows.Add(reader["id"], date, name, Inv_No, debit.ToString("N2"), credit.ToString("N2"), totalAmount.ToString("N2"));

                }
            }
        }
        
        private void dgvSales_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                //if (dgvSales.Rows[e.RowIndex].Cells["id"].Value != null)
                //{
                //    int _id = int.Parse(dgvSales.Rows[e.RowIndex].Cells["id"].Value.ToString());
                //    var _type = dgvSales.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                //    if (_type.Contains("Sales"))
                //    {
                //        new frmAddSales(_id, "", id);
                //    }
                //    else if(_type == "Customer Receipt")
                //    {
                //        new frmViewReceiptVoucher(id);
                //    }
                //    else if (_type == "Customer Opening Balance")
                //    {
                //        new frmTransactionJournal(_id, _type, _id.ToString());
                //    }
                //}
            }
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = guna2HtmlLabel8.Text;
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
            //left.AddFormattedText("Time: ", TextFormat.Bold);
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n");
            //left.AddFormattedText("Date: ", TextFormat.Bold);
            left.AddText(DateTime.Now.ToString("dd/MM/yyyy"));

            // Center cell - Company Name & Report Titles
            Paragraph center = headerRow.Cells[1].AddParagraph();
            center.Format.Font.Name = "Times New Roman";
            center.Format.Alignment = ParagraphAlignment.Center;
            center.Format.SpaceAfter = 0;

            // Company name - Bold, size 10
            FormattedText companyText = center.AddFormattedText(companyName.ToUpper() + "\n", TextFormat.Bold);
            companyText.Font.Size = 12;

            // "Cost Center Summary" - Bold, size 10
            FormattedText summaryText = center.AddFormattedText("Cost Center Summary\n", TextFormat.Bold);
            summaryText.Font.Size = 12;

            // "All Transactions" - Regular, size 9
            FormattedText allTransText = center.AddFormattedText("All Transactions", TextFormat.NotBold);
            allTransText.Font.Size = 9;

            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;


            //section.Add(headerTable);

            // Bold line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // Table for data
            Table dataTable = section.AddTable();
            dataTable.Borders.Width = 0;
            dataTable.AddColumn("10cm");
            dataTable.AddColumn("5cm");

            decimal totalAmount = 0;

            // Load from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;

                string id = row.Cells["id"].Value?.ToString() ?? "0";
                string date = row.Cells["date"].Value?.ToString() ?? "";
                string name = row.Cells["name"].Value?.ToString() ?? "";
                string vno = row.Cells["VoucherNo"].Value?.ToString() ?? "";
                decimal dr = Convert.ToDecimal(row.Cells["debit"].Value?.ToString() ?? "0");
                decimal cr = Convert.ToDecimal(row.Cells["credit"].Value?.ToString() ?? "0");
                decimal balance = Convert.ToDecimal(row.Cells["balance"].Value?.ToString() ?? "0");

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(date);
                tRow.Cells[1].AddParagraph(name);
                tRow.Cells[2].AddParagraph(vno);
                tRow.Cells[3].AddParagraph(dr.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[4].AddParagraph(cr.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[5].AddParagraph(balance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
            }

            // Render and save
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CostCenterSummary.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "CustomerBalanceDetails.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Customer Balance Details";

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

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpFrom.Enabled = dtpTo.Enabled = !chkDate.Checked;
            LoadData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtpFrom, dtpTo);
                LoadData();
            }
        }
    }
}
