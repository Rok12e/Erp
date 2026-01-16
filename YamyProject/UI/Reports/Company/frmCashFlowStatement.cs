using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using Font = System.Drawing.Font;

namespace YamyProject
{
    public partial class frmCashFlowStatement : Form
    {
        public frmCashFlowStatement()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void frmCashFlowStatement_Load(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            lblTime.Text = DateTime.Now.ToString("hh:mm:tt");
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            }
            btnPrint.Text = "Print ▼";
            LoadData();
        }

        private void LoadData()
        {
            AddDGVColumns();
            BindDGV();
        }

        private void AddDGVColumns()
        {
            dgvReport.Columns.Add("state", "");
            dgvReport.Columns.Add("loadState", "");
            dgvReport.Columns.Add("currLvl", "");
            dgvReport.Columns.Add("lvl1", "Account Name");
            dgvReport.Columns.Add("id", "");
            dgvReport.Columns.Add("currname", "");
            dgvReport.Columns.Add("balance", "Amount");
            LocalizationManager.LocalizeDataGridViewHeaders(dgvReport);
            dgvReport.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvReport.Columns["state"].Visible = dgvReport.Columns["currname"].Visible = dgvReport.Columns["id"].Visible = dgvReport.Columns["loadState"].Visible = dgvReport.Columns["currLvl"].Visible = false;

            //dgvReport.Columns["colName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvReport.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvReport.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvReport.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            dgvReport.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            dgvReport.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvReport.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
        }

        public void BindDGV()
        {
            dgvReport.Rows.Clear();
            decimal totalAmount = 0,totalBalance = 0;
            var parameters = new[]
                {
                    DBClass.CreateParameter("startDate", dateTimePicker1.Value.Date),
                    DBClass.CreateParameter("endDate", dateTimePicker2.Value.Date)
                };

            string sql = @"SELECT 'OPERATING ACTIVITIES' AS name, '' AS id, NULL AS balance, 'u' AS mode, '1' AS level, ' ►     ' AS symbol
                            UNION
                            SELECT 'Net Income', '', (
                                WITH AccountBalances AS (
                                    SELECT 
                                        l1.category_code AS Category,
                                        SUM(t.credit - t.debit) AS Balance
                                    FROM tbl_transaction t
                                    JOIN tbl_coa_level_4 l4 ON t.account_id = l4.id
                                    JOIN tbl_coa_level_3 l3 ON l4.main_id = l3.id
                                    JOIN tbl_coa_level_2 l2 ON l3.main_id = l2.id
                                    JOIN tbl_coa_level_1 l1 ON l2.main_id = l1.id
                                    WHERE l1.category_code IN ('INCOME', 'COST', 'EXPENSE') 
                                      AND t.state = 0
                                      AND t.date >= @startDate AND t.date <= @endDate
                                    GROUP BY l1.category_code
                                )
                                SELECT
                                    COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'INCOME'), 0)
                                    - COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'COST'), 0)
                                    - COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'EXPENSE'), 0)
                            ) AS balance, '2' AS level, 'n' AS mode, '                    ' AS symbol
                            UNION
                            SELECT 'Adjustments to reconcile Net Income', '', NULL, 'u' AS mode, '2' AS level, '   ►     ' AS symbol
                            UNION
                            SELECT 'to net cash provided by operations:', '', NULL, 'u' AS mode, '2' AS level, '   ►     ' AS symbol
                            UNION
                            SELECT CONCAT('  ', l4.name), l4.id, SUM(t.debit - t.credit), 'n' AS mode, '4' AS level, '                    ' AS symbol
                            FROM tbl_coa_level_1 l1
                            JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
                            JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
                            JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
                            LEFT JOIN tbl_transaction t ON t.account_id = l4.id AND t.state = 0
                                AND t.date >= @startDate AND t.date <= @endDate
                            WHERE l1.category_code = 'ASSET'
                            GROUP BY l4.id, l4.name;
                            ";
            //using (MySqlDataReader reader = DBClass.ExecuteReader(@"
            //            SELECT 'OPERATING ACTIVITIES' AS name, '' AS id, NULL AS balance, 'u' AS mode, '1' AS level, ' ►     ' AS symbol

            //            UNION

            //            SELECT 'Net Income', '', (
            //                WITH AccountBalances AS (
            //                    SELECT 
            //                        l1.category_code AS Category,
            //                        SUM(t.credit - t.debit) AS Balance
            //                    FROM tbl_transaction t
            //                    JOIN tbl_coa_level_4 l4 ON t.account_id = l4.id
            //                    JOIN tbl_coa_level_3 l3 ON l4.main_id = l3.id
            //                    JOIN tbl_coa_level_2 l2 ON l3.main_id = l2.id
            //                    JOIN tbl_coa_level_1 l1 ON l2.main_id = l1.id
            //                    WHERE l1.category_code IN ('INCOME', 'COST', 'EXPENSE') AND t.state = 0
            //                    GROUP BY l1.category_code
            //                )
            //                SELECT
            //                    COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'INCOME'), 0)
            //                    - COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'COST'), 0)
            //                    - COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'EXPENSE'), 0)
            //            ) AS balance, '2' AS level, 'n' AS mode, '                    ' AS symbol

            //            UNION

            //            SELECT 'Adjustments to reconcile Net Income', '', NULL, 'u' AS mode, '2' AS level, '   ►     ' AS symbol

            //            UNION

            //            SELECT 'to net cash provided by operations:', '', NULL, 'u' AS mode, '2' AS level, '   ►     ' AS symbol

            //            UNION

            //            SELECT CONCAT('  ', l4.name), l4.id, SUM(t.debit - t.credit), 'n' AS mode, '4' AS level, '                    ' AS symbol
            //            FROM tbl_coa_level_1 l1
            //            JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
            //            JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
            //            JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
            //            LEFT JOIN tbl_transaction t ON t.account_id = l4.id AND t.state = 0
            //            WHERE l1.category_code = 'ASSET'
            //            GROUP BY l4.id, l4.name;
            //        ",
            using (MySqlDataReader reader = DBClass.ExecuteReader(sql, parameters.ToArray()))
            {
                while (reader.Read())
                {
                    dgvReport.Rows.Add("e", reader["mode"], reader["level"], reader["symbol"] + reader["name"].ToString(),
                        reader["id"].ToString(), reader["name"].ToString(),
                        reader["balance"].ToString());

                    totalBalance += Convert.ToDecimal(reader["balance"] == DBNull.Value ? "0" : reader["balance"]);
                }
            }
            dgvReport.Rows.Add("e", "n", "2", "         " + "Net cash provided by Operating Activities",
                        "0", "Net cash provided by Operating Activities",totalBalance.ToString("N2"));

            totalAmount = totalBalance;
            totalBalance = 0;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                                                SELECT 'INVESTING ACTIVITIES' AS name, '' AS id, NULL AS balance,'u' as mode,'1' level,' ►     ' as symbol
                                            UNION
                                                SELECT CONCAT('  ', l4.name), l4.id, SUM(t.debit - t.credit),'n' as mode,'4' level,'                    ' as symbol
                                                FROM tbl_coa_level_1 l1
                                                JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
                                                JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
                                                JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
                                                LEFT JOIN tbl_transaction t ON t.account_id = l4.id AND t.state = 0
                                                WHERE l1.name IN('Fixed Assets', 'Investments')
                                                AND t.date >= @startDate AND t.date <= @endDate
                                                GROUP BY l4.id, l4.name
                                            ", parameters.ToArray()))
            {
                while (reader.Read())
                {
                    dgvReport.Rows.Add("e", reader["mode"], reader["level"], reader["symbol"] + reader["name"].ToString(),
                        reader["id"].ToString(), reader["name"].ToString(),
                        reader["balance"].ToString());

                    totalBalance += Convert.ToDecimal(reader["balance"] == DBNull.Value ? "0" : reader["balance"]);
                }
            }
            
            dgvReport.Rows.Add("e", "n", "3", "         " + "Net cash provided by Investing Activities",
                        "0", "Net cash provided by Investing Activities", totalBalance.ToString("N2"));
            
            totalAmount += totalBalance;
            
            dgvReport.Rows.Add("e", "n", "2", "    " + "Net cash increase for period",
                        "0", "Net cash increase for period", totalAmount.ToString("N2"));
            dgvReport.Rows.Add("e", "n", "1", "  " + "Cash at end of period",
                        "0", "Cash at end of period", totalAmount.ToString("N2"));
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.Cells["lvl1"].Value.ToString().ToUpper() == " ►     OPERATING ACTIVITIES" ||
                    row.Cells["lvl1"].Value.ToString().ToUpper() == " ►     INVESTING ACTIVITIES")
                {
                    row.DefaultCellStyle.Font = new Font(dgvReport.Font, FontStyle.Bold);
                }
            }

        }
        private void ExpandRowRecursively(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvReport.Rows.Count)
                return;

            var row = dgvReport.Rows[rowIndex];

            string state = row.Cells["state"].Value.ToString();
            string loadState = row.Cells["loadState"].Value.ToString();
            int currLvl = int.Parse(row.Cells["currLvl"].Value.ToString());

            if (currLvl >= 4 || state != "e") return;

            string space = new string(' ', currLvl * 6 + 3);
            string cellValue = row.Cells["lvl1"].Value.ToString();

            if (loadState == "u")
            {
                int parentId = int.Parse(row.Cells["id"].Value.ToString());
                List<DataGridViewRow> newRows = new List<DataGridViewRow>();

                using (MySqlDataReader reader = DBClass.ExecuteReader($@"SELECT name as currname, concat(code,' - ',name) as name , id FROM tbl_coa_level_" + (currLvl + 1) + " WHERE main_id = @parentId ORDER BY code",
                                                                      DBClass.CreateParameter("parentId", parentId)))
                {
                    int insertIndex = rowIndex + 1;

                    //row.Cells["balance"].Value = "";

                    while (reader.Read())
                    {
                        string nextState = (currLvl + 1 == 4) ? "n" : "e";
                        string nextIcon = (currLvl + 1 == 4) ? "  " : "►";
                        var balanceData = GetBalanceData(currLvl + 1, reader["currname"].ToString());
                        if (balanceData != null)
                        {
                            var newRow = new DataGridViewRow();
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = nextState });
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = "u" });
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = currLvl + 1 });
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = space + nextIcon + "     " + reader["name"].ToString() });
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["id"].ToString() });
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["currname"].ToString() });
                            newRow.Cells.Add(new DataGridViewTextBoxCell { Value = balanceData });
                            newRow.Height = row.Height;

                            ApplyRowStyle(newRow, currLvl + 1);
                            newRows.Add(newRow);
                        }
                    }

                    foreach (var newRow in newRows)
                    {
                        dgvReport.Rows.Insert(insertIndex, newRow);
                        insertIndex++;
                    }
                }

                row.Cells["loadState"].Value = "l";
            }

            row.Cells["lvl1"].Value = cellValue.Replace("►", "▼");
            row.Cells["state"].Value = "c";

            // Recursively expand all newly inserted child rows
            int nextRowIndex = rowIndex + 1;
            while (nextRowIndex < dgvReport.Rows.Count &&
                   int.Parse(dgvReport.Rows[nextRowIndex].Cells["currLvl"].Value.ToString()) > currLvl)
            {
                if (int.Parse(dgvReport.Rows[nextRowIndex].Cells["currLvl"].Value.ToString()) == currLvl + 1)
                {
                    ExpandRowRecursively(nextRowIndex);
                }
                nextRowIndex++;
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

            // "Cash Flow Statement Summary" - Bold, size 10
            FormattedText summaryText = center.AddFormattedText("Cash Flow Statement Summary\n", TextFormat.Bold);
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
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                if (row.IsNewRow) continue;

                string account = row.Cells["lvl1"].Value?.ToString() ?? "";
                if (account.ToUpper().Contains("TOTAL")) continue;

                string amountStr = row.Cells["balance"].Value?.ToString() ?? "0";
                account = account.Replace("▶", "").Trim();
                amountStr = amountStr.Replace("◀", "").Trim();

                decimal amount = 0;
                decimal.TryParse(amountStr, out amount);

                Row tRow = dataTable.AddRow();
                tRow.Cells[0].AddParagraph(account);
                tRow.Cells[1].AddParagraph(amount.ToString("N2")).Format.Alignment = ParagraphAlignment.Right;

                totalAmount += amount;
            }

            // TOTAL row
            Row totalRow = dataTable.AddRow();
            totalRow.Cells[0].AddParagraph("TOTAL").Format.Font.Bold = true;
            Paragraph totalText = totalRow.Cells[1].AddParagraph(totalAmount.ToString("N2"));
            totalText.Format.Font.Bold = true;
            totalText.Format.Font.Underline = Underline.Single;
            totalRow.Cells[1].Format.Alignment = ParagraphAlignment.Right;

            //section.Add(dataTable);

            // Render and save
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CashFlowStatement.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "CashFlowStatement.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "Cash Flow Statement";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "C1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    int rowIndex = 2;
                    foreach (DataGridViewRow row in dgvReport.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string account = row.Cells["lvl1"].Value?.ToString();
                        string amount = row.Cells["balance"].Value?.ToString();

                        var accountCell = worksheet.Cells[rowIndex, 2];
                        var amountCell = worksheet.Cells[rowIndex, 3];

                        accountCell.Value = account.Replace("▶", "").Trim().Replace("▼","").Trim();
                        amountCell.Value = amount.Replace("◀", "").Trim().Trim().Replace("▼", "").Trim();

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
            //if (Utilities.UserPermissionCheck("CustomerBalanceSummery"))
            //{
            //int id = int.Parse(dgvReport.CurrentRow.Cells["id"].Value.ToString());
            //frmLogin.frmMain.openChildForm(new frmCustomerBalanceDetails(id));
            //}
            if (e.RowIndex < 0) return;

            int currLvl = int.Parse(dgvReport.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            if (currLvl > 4) return;
            if (currLvl == 4)
            {
                frmLogin.frmMain.openChildForm(new MasterTransactionByAccount(int.Parse(dgvReport.Rows[e.RowIndex].Cells["id"].Value.ToString())));
            }

            string state = dgvReport.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvReport.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvReport.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);

            try
            {
                if (state == "e")
                {
                    if (loadState == "u")
                    {
                        int parentId = int.Parse(dgvReport.Rows[e.RowIndex].Cells["id"].Value.ToString());
                        List<DataGridViewRow> newRows = new List<DataGridViewRow>();

                        using (MySqlDataReader reader = DBClass.ExecuteReader($@"SELECT name as currname, concat(code,' - ',name) as name , id FROM tbl_coa_level_" + (currLvl + 1) + " WHERE main_id = @parentId ORDER BY code",
                                                                          DBClass.CreateParameter("parentId", parentId)))
                        {
                            int insertIndex = e.RowIndex + 1;

                            //dgvReport.Rows[e.RowIndex].Cells["balance"].Value = "";

                            while (reader.Read())
                            {
                                string nextState = (currLvl + 1 == 4) ? "n" : "e";
                                string nextIcon = (currLvl + 1 == 4) ? "  " : "►";
                                var balanceData = GetBalanceData(currLvl + 1, reader["currname"].ToString());
                                if (balanceData != null)
                                {
                                    var newRow = new DataGridViewRow();
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = nextState });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = "u" });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = currLvl + 1 });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = space + nextIcon + "     " + reader["name"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["id"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["currname"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = balanceData });
                                    newRow.Height = dgvReport.Rows[e.RowIndex].Height;

                                    ApplyRowStyle(newRow, currLvl + 1);
                                    newRows.Add(newRow);
                                }
                            }

                            // Add all new rows in one go for better performance
                            foreach (var row in newRows)
                            {
                                dgvReport.Rows.Insert(insertIndex, row);
                                insertIndex++;
                            }
                        }

                        dgvReport.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                    }
                    else
                    {
                        //dgvReport.Rows[e.RowIndex].Cells["balance"].Value = "";

                        GetAllChildren(e.RowIndex);
                    }

                    dgvReport.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("►", "▼");
                    dgvReport.Rows[e.RowIndex].Cells["state"].Value = "c";
                }
                else if (state == "c")
                {
                    var parentBalanceData = GetBalanceData(currLvl, dgvReport.Rows[e.RowIndex].Cells["currname"].Value.ToString().Replace("▼", ""));

                    if (!string.IsNullOrEmpty(parentBalanceData))
                    {
                        //dgvReport.Rows[e.RowIndex].Cells["balance"].Value = parentBalanceData;
                    }
                    ToggleChildRowsVisibility(e.RowIndex, currLvl, false);
                    dgvReport.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("▼", "►");
                    dgvReport.Rows[e.RowIndex].Cells["state"].Value = "e";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyRowStyle(DataGridViewRow row, int level)
        {
            if (level == 2)
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            else if (level == 3)
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Gainsboro;
            else
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Silver;
        }

        private string GetBalanceData(int level, string name)
        {
            using (MySqlDataReader read = DBClass.ExecuteReader($@"SELECT 
                        t1.name AS level1_name, 
                        t2.name AS level2_name, 
                        t3.name AS level3_name, 
                        t4.name AS level4_name, 
                        t1.id AS level1_id, 
                        t2.id AS level2_id, 
                        t3.id AS level3_id, 
                        t4.id AS level4_id,

                        -- Balance for Level 1 (Summing all child transactions)
                        (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                         FROM tbl_transaction tt
                         WHERE  tt.state=0 and tt.account_id IN (
                             SELECT id FROM tbl_coa_level_4 
                             WHERE main_id IN (SELECT id FROM tbl_coa_level_3 
                                              WHERE main_id IN (SELECT id FROM tbl_coa_level_2 
                                                               WHERE main_id = t1.id))
                         )
                        ) AS level1_balance,

                        -- Balance for Level 2 (Summing its child transactions)
                        (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                         FROM tbl_transaction tt
                         WHERE  tt.state=0 and tt.account_id IN (
                             SELECT id FROM tbl_coa_level_4 
                             WHERE main_id IN (SELECT id FROM tbl_coa_level_3 
                                              WHERE main_id = t2.id)
                         )
                        ) AS level2_balance,

                        -- Balance for Level 3 (Summing only Level 4 transactions)
                        (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                         FROM tbl_transaction tt
                         WHERE  tt.state=0 and tt.account_id IN (
                             SELECT id FROM tbl_coa_level_4 
                             WHERE main_id = t3.id
                         )
                        ) AS level3_balance,

                        -- Balance for Level 4 (Direct transactions only)
                        COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) AS level4_balance

                    FROM tbl_coa_level_1 t1
                    LEFT JOIN tbl_coa_level_2 t2 ON t2.main_id = t1.id
                    LEFT JOIN tbl_coa_level_3 t3 ON t3.main_id = t2.id
                    LEFT JOIN tbl_coa_level_4 t4 ON t4.main_id = t3.id
                    LEFT JOIN tbl_transaction tt ON tt.account_id = t4.id  

                    WHERE  tt.state=0 and t1.category_code IN ('ASSET', 'LIABILITY', 'EQUITY') AND t{level}.name = @name

                    GROUP BY t1.id, t2.id, t3.id, t4.id;
                    ", DBClass.CreateParameter("name", name)))
            {
                if (read.Read())
                {
                    return read[$"level{level}_balance"].ToString();
                }
            }

            return null;
        }

        private void ToggleChildRowsVisibility(int parentRowIndex, int parentLevel, bool visible)
        {
            for (int i = parentRowIndex + 1; i < dgvReport.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvReport.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (visible)
                    {
                        dgvReport.Rows[i].Visible = true;

                        if (dgvReport.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(dgvReport.Rows[i].Index);
                    }
                    else
                        dgvReport.Rows[i].Visible = false;
                }

                else
                    break;
            }
        }

        private void GetAllChildren(int parentRowIndex)
        {
            int parentLevel = int.Parse(dgvReport.Rows[parentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = parentRowIndex + 1; i < dgvReport.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvReport.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (rowLevel == parentLevel + 1)
                    {
                        dgvReport.Rows[i].Visible = true;
                        if (dgvReport.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(i);
                    }
                    else
                        continue;
                }
                else
                    break;
            }

        }

        private DataGridViewRow GetParentRow(int currentRowIndex)
        {
            if (currentRowIndex <= 0) return null;

            int currentLevel = int.Parse(dgvReport.Rows[currentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = currentRowIndex - 1; i >= 0; i--)
            {
                int rowLevel = int.Parse(dgvReport.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel == currentLevel - 1)
                    return dgvReport.Rows[i];
            }

            return null;
        }

        private void ExpandAll()
        {
            for (int i = 0; i < dgvReport.Rows.Count; i++)
            {
                string state = dgvReport.Rows[i].Cells["state"].Value.ToString();
                string loadState = dgvReport.Rows[i].Cells["loadState"].Value.ToString();
                int currLvl = int.Parse(dgvReport.Rows[i].Cells["currLvl"].Value.ToString()??"0");

                if (state == "e")
                {
                    dgvSales_CellDoubleClick(dgvReport, new DataGridViewCellEventArgs(0, i));
                }

                // Re-check i after possible row insertions to ensure all nodes are processed
                if (currLvl < 4)
                {
                    i = Math.Max(i - 1, 0); // Step back and reprocess new rows
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            BindDGV();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            BindDGV();
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