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
    public partial class frmCostCenterSummary : Form
    {
        public frmCostCenterSummary()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmCostCenterSummary_Load(object sender, EventArgs e)
        {
            btnPrint.Text = "Print ▼";
            dtFrom.Value = dtTo.Value = DateTime.Now;
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
            BindCombos.PopulateCostCenter(comboBox1);
            comboBox1.SelectedIndex = -1;
            LoadData();
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
            string query1 = @"
                                SELECT c.id, c.name, COUNT(t.id) AS Num, SUM(t.debit) AS Total_Debit, SUM(t.credit) AS Total_Credit
                                FROM tbl_cost_center c JOIN tbl_cost_center_transaction t ON c.id = t.cost_center_id
                                WHERE c.id>0 ";
            if (!chkDate.Checked)
                query1 += " AND t.date >= @from AND t.date <= @to";

            if (!guna2CheckBox1.Checked && comboBox1.SelectedIndex != -1)
                query1 += " AND c.id = @cId ";

            query1 += " GROUP BY c.id,c.name ORDER BY c.id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query1,
                DBClass.CreateParameter("from",dtFrom.Value), 
                DBClass.CreateParameter("to", dtTo.Value),
                DBClass.CreateParameter("cId", (comboBox1.SelectedValue??0))))
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string Inv_No = reader["Num"].ToString();
                    decimal debit = reader["Total_Debit"] as decimal? ?? 0;
                    decimal credit = reader["Total_Credit"] as decimal? ?? 0;
                    decimal balance = (debit - credit);

                    totalAmount += balance;
                    dgvSales.Rows.Add(reader["id"], name, Inv_No, debit.ToString("N2"), credit.ToString("N2"), balance.ToString("N2"));

                }

            // Add total row
            //int totalRow = dgvSales.Rows.Add(null,null,"TOTAL",debitTotal,creditTotal, totalAmount.ToString("N2"));
            //dgvSales.Rows[totalRow].DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold | FontStyle.Underline);
            //dgvSales.Rows[totalRow].DefaultCellStyle.BackColor = System.Drawing.Color.Gainsboro;
            //dgvSales.Rows[totalRow].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            //dgvSales.Rows[totalRow].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void SavePDF_Click(object sender, EventArgs e)
        {
            string companyName = lblCompany.Text;
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
            dataTable.AddColumn("6cm");
            dataTable.AddColumn("1cm");
            dataTable.AddColumn("1.5cm");
            dataTable.AddColumn("1.5cm");
            dataTable.AddColumn("2cm");

            decimal totalAmount = 0;

            // Load from DataGridView
            foreach (DataGridViewRow row in dgvSales.Rows)
            {
                if (row.IsNewRow) continue;
                
                string id = row.Cells["id"].Value?.ToString() ?? "0";
                string name = row.Cells["name"].Value?.ToString() ?? "";
                string vno = row.Cells["VoucherNo"].Value?.ToString() ?? "";
                decimal dr = Convert.ToDecimal(row.Cells["debit"].Value?.ToString() ?? "0");
                decimal cr = Convert.ToDecimal(row.Cells["credit"].Value?.ToString() ?? "0");
                decimal balance = Convert.ToDecimal(row.Cells["balance"].Value?.ToString() ?? "0");

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(name);
                tRow.Cells[1].AddParagraph(vno);
                tRow.Cells[2].AddParagraph(dr.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[3].AddParagraph(cr.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[4].AddParagraph(balance.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;
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
                saveDialog.FileName = "CustomerBalance.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Cost Center";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "C1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    int rowIndex = 2;
                    foreach (DataGridViewRow row in dgvSales.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string account = row.Cells["Account"].Value?.ToString();
                        string amount = row.Cells["Amount"].Value?.ToString();

                        var accountCell = worksheet.Cells[rowIndex, 2];
                        var amountCell = worksheet.Cells[rowIndex, 3];

                        accountCell.Value = account.Replace("▶", "").Trim();
                        //amountCell.Value = amount.Replace("◀", "").Trim();

                        accountCell.Font.Name = "Times New Roman";
                        amountCell.Font.Name = "Times New Roman";

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

                    worksheet.Columns[2].AutoFit();
                    worksheet.Columns[3].AutoFit();

                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close();
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void dgvSales_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dgvSales.CurrentRow.Cells["id"].Value.ToString());
            //string inv = dgvSales.CurrentRow.Cells["VoucherNo"].Value.ToString();
            //decimal Debit = decimal.Parse(dgvSales.CurrentRow.Cells["Debit"].Value.ToString());
            //string type = Debit > 0 ? "purchase" : "sales";
            frmLogin.frmMain.openChildForm(new frmCostCenterDetails(id));
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtFrom, dtTo);
                LoadData();
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = !guna2CheckBox1.Checked;
            LoadData();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            LoadData();
        }
    }
}