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
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class ProfitandLossReport : Form
    {
        string reportType = "";
        public ProfitandLossReport(string _type)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.reportType = _type;
            headerUC1.FormText = this.Text;
        }

        void Dates(DateTime d)
        {
            DateTime sDateOfY = new DateTime(d.Year, 1, 1);
            DateTime eDateOfY = new DateTime(d.Year, 12, 31);
            
            dtFrom.Value = sDateOfY;
            dtTo.Value = eDateOfY;
        }

        private void ProfitandLossReport_Load(object sender, EventArgs e)
        {
            Dates(DateTime.Now);
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    lblCompany.Text = reader["name"].ToString();
                }
            }

            btnPrint.Text = "Print ▼";
            AddDGVColumns();
            BindDGV();
        }

        private void AddDGVColumns()
        {
            dgvCustomer.Columns.Add("state", "");
            dgvCustomer.Columns.Add("loadState", "");
            dgvCustomer.Columns.Add("currLvl", "");
            dgvCustomer.Columns.Add("lvl1", "Account Name");
            dgvCustomer.Columns.Add("id", "");
            dgvCustomer.Columns.Add("currname", "");
            dgvCustomer.Columns.Add("balance", "Amount");
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            dgvCustomer.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["state"].Visible = dgvCustomer.Columns["currname"].Visible = dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["loadState"].Visible = dgvCustomer.Columns["currLvl"].Visible = false;

            dgvCustomer.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvCustomer.DefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvCustomer.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            dgvCustomer.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;

            dgvCustomer.RowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgvCustomer.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
        }

        public void BindDGV()
        {
            dgvCustomer.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                        WITH AccountBalances AS (
                            -- Calculate balances for Income, Cost, and Expenses
                            SELECT 
                                l1.name AS Category, 
                                COALESCE(SUM(t.debit) - SUM(t.credit), 0) AS Balance
                            FROM tbl_coa_level_1 l1
                            LEFT JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
                            LEFT JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
                            LEFT JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
                            LEFT JOIN tbl_transaction t ON t.account_id = l4.id
                            WHERE l1.name IN ('Income', 'Cost', 'General & Direct Expenses')
                            GROUP BY l1.name
                        ),

                        FinalBalance AS (
                            -- Calculate final balance: Income - Cost - Expenses
                            SELECT 
                                'Final Balance (Income - Cost - Expenses)' AS Category,
                                COALESCE(
                                    (SELECT Balance FROM AccountBalances WHERE Category = 'Income'), 0
                                ) -
                                COALESCE(
                                    (SELECT Balance FROM AccountBalances WHERE Category = 'Cost'), 0
                                ) -
                                COALESCE(
                                    (SELECT Balance FROM AccountBalances WHERE Category = 'General & Direct Expenses'), 0
                                ) AS Balance
                        ),

                        EquityBalances AS (
                            -- Calculate balances for Assets, Liabilities, and Equity
                            SELECT 
                                l1.name, 
                                l1.id, 
                                COALESCE(SUM(t.debit) - SUM(t.credit), 0) AS balance
                            FROM tbl_coa_level_1 l1
                            LEFT JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
                            LEFT JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
                            LEFT JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
                            LEFT JOIN tbl_transaction t ON t.account_id = l4.id
                            WHERE l1.name IN ('Income', 'Cost', 'General & Direct Expenses')
                            GROUP BY l1.id, l1.name
                        )

                        -- Final result combining the balances
                        SELECT 
                            eb.name, 
                            eb.id, 
                            CASE 
                                -- For Equity, add the final balance from Income - Cost - Expenses to Retained Earnings
                                WHEN eb.name = 'Equity' THEN 
                                    eb.balance + 
                                    (SELECT Balance FROM FinalBalance)
                                ELSE 
                                    eb.balance
                            END AS balance
                        FROM EquityBalances eb
                        ORDER BY eb.id;

                        "))
            {
                while (reader.Read())
                    dgvCustomer.Rows.Add("e", "u", "1", "   ►     " + reader["name"].ToString(),
                        reader["id"].ToString(), reader["name"].ToString(),
                        reader["balance"].ToString());

                // Auto-expand everything
                for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                {
                    ExpandRowRecursively(i);
                }
            }
            //dgvCustomer.DefaultCellStyle.BackColor = Color.LightCyan;
            //dgvCustomer.Columns["balance"].DefaultCellStyle.BackColor = Color.LightGray;
            //dgvCustomer.Columns["balance"].DefaultCellStyle.ForeColor = Color.DarkBlue;
            //dgvCustomer.Columns["balance"].DefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }
        private void ExpandRowRecursively(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvCustomer.Rows.Count)
                return;

            DataGridViewRow row = dgvCustomer.Rows[rowIndex];
            int currLvl = int.Parse(row.Cells["currLvl"].Value.ToString());
            string state = row.Cells["state"].Value.ToString();
            string loadState = row.Cells["loadState"].Value.ToString();
            string cellValue = row.Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);

            try
            {
                if (state == "e")
                {
                    if (loadState == "u")
                    {
                        int parentId = int.Parse(row.Cells["id"].Value.ToString());
                        List<DataGridViewRow> newRows = new List<DataGridViewRow>();

                        using (MySqlDataReader reader = DBClass.ExecuteReader(
                            $@"SELECT name as currname, concat(code,' - ',name) as name , id 
                       FROM tbl_coa_level_" + (currLvl + 1) + " WHERE main_id = @parentId ORDER BY code",
                            DBClass.CreateParameter("parentId", parentId)))
                        {
                            int insertIndex = rowIndex + 1;
                            dgvCustomer.Rows[rowIndex].Cells["balance"].Value = "";

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
                                    newRow.Height = dgvCustomer.Rows[rowIndex].Height;

                                    ApplyRowStyle(newRow, currLvl + 1);
                                    newRows.Add(newRow);
                                }
                            }

                            foreach (var r in newRows)
                            {
                                dgvCustomer.Rows.Insert(insertIndex, r);
                                insertIndex++;
                            }
                        }

                        dgvCustomer.Rows[rowIndex].Cells["loadState"].Value = "l";
                    }
                    else
                    {
                        dgvCustomer.Rows[rowIndex].Cells["balance"].Value = "";
                        GetAllChildren(rowIndex);
                    }

                    dgvCustomer.Rows[rowIndex].Cells["lvl1"].Value = cellValue.Replace("►", "▼");
                    dgvCustomer.Rows[rowIndex].Cells["state"].Value = "c";
                }

                // Expand child rows recursively
                int currentRowLevel = int.Parse(row.Cells["currLvl"].Value.ToString());
                int nextIndex = rowIndex + 1;
                while (nextIndex < dgvCustomer.Rows.Count)
                {
                    int childLvl = int.Parse(dgvCustomer.Rows[nextIndex].Cells["currLvl"].Value.ToString());
                    if (childLvl <= currentRowLevel)
                        break;

                    if (dgvCustomer.Rows[nextIndex].Cells["state"].Value.ToString() == "e")
                    {
                        ExpandRowRecursively(nextIndex);
                    }
                    nextIndex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while expanding: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int currLvl = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            if (currLvl > 4) return;
            if (currLvl == 4)
            {
                frmLogin.frmMain.openChildForm(new MasterTransactionByAccount(int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString())));
            }

            string state = dgvCustomer.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);

            try
            {
                if (state == "e")
                {
                    if (loadState == "u")
                    {
                        int parentId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString());
                        List<DataGridViewRow> newRows = new List<DataGridViewRow>();

                        using (MySqlDataReader reader = DBClass.ExecuteReader($@"SELECT name as currname, concat(code,' - ',name) as name , id FROM tbl_coa_level_" + (currLvl + 1) + " WHERE main_id = @parentId ORDER BY code",
                                                                          DBClass.CreateParameter("parentId", parentId)))
                        {
                            int insertIndex = e.RowIndex + 1;

                            //dgvCustomer.Rows[e.RowIndex].Cells["balance"].Value = "";

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
                                    newRow.Height = dgvCustomer.Rows[e.RowIndex].Height;

                                    ApplyRowStyle(newRow, currLvl + 1);
                                    newRows.Add(newRow);
                                }
                            }

                            // Add all new rows in one go for better performance
                            foreach (var row in newRows)
                            {
                                dgvCustomer.Rows.Insert(insertIndex, row);
                                insertIndex++;
                            }
                        }

                        dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                    }
                    else
                    {
                        //dgvCustomer.Rows[e.RowIndex].Cells["balance"].Value = "";

                        GetAllChildren(e.RowIndex);
                    }

                    dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("►", "▼");
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Value = "c";
                }
                else if (state == "c")
                {
                    var parentBalanceData = GetBalanceData(currLvl, dgvCustomer.Rows[e.RowIndex].Cells["currname"].Value.ToString().Replace("▼", ""));

                    if (!string.IsNullOrEmpty(parentBalanceData))
                    {
                        dgvCustomer.Rows[e.RowIndex].Cells["balance"].Value = parentBalanceData;
                    }
                    ToggleChildRowsVisibility(e.RowIndex, currLvl, false);
                    dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("▼", "►");
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Value = "e";
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
                             WHERE tt.account_id IN (
                                 SELECT id FROM tbl_coa_level_4 
                                 WHERE main_id IN (SELECT id FROM tbl_coa_level_3 
                                                  WHERE main_id IN (SELECT id FROM tbl_coa_level_2 
                                                                   WHERE main_id = t1.id))
                             )
                            ) AS level1_balance,

                            -- Balance for Level 2 (Summing its child transactions)
                            (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                             FROM tbl_transaction tt
                             WHERE tt.account_id IN (
                                 SELECT id FROM tbl_coa_level_4 
                                 WHERE main_id IN (SELECT id FROM tbl_coa_level_3 
                                                  WHERE main_id = t2.id)
                             )
                            ) AS level2_balance,

                            -- Balance for Level 3 (Summing only Level 4 transactions)
                            (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                             FROM tbl_transaction tt
                             WHERE tt.account_id IN (
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

                        WHERE t1.name IN ('Income', 'Cost', 'General & Direct Expenses') AND t{level}.name = @name

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
            for (int i = parentRowIndex + 1; i < dgvCustomer.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvCustomer.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (visible)
                    {
                        dgvCustomer.Rows[i].Visible = true;

                        if (dgvCustomer.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(dgvCustomer.Rows[i].Index);
                    }
                    else
                        dgvCustomer.Rows[i].Visible = false;
                }

                else
                    break;
            }
        }

        private void GetAllChildren(int parentRowIndex)
        {
            int parentLevel = int.Parse(dgvCustomer.Rows[parentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = parentRowIndex + 1; i < dgvCustomer.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvCustomer.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (rowLevel == parentLevel + 1)
                    {
                        dgvCustomer.Rows[i].Visible = true;
                        if (dgvCustomer.Rows[i].Cells["state"].Value.ToString() == "c")
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

            int currentLevel = int.Parse(dgvCustomer.Rows[currentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = currentRowIndex - 1; i >= 0; i--)
            {
                int rowLevel = int.Parse(dgvCustomer.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel == currentLevel - 1)
                    return dgvCustomer.Rows[i];
            }

            return null;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            contextMenuExport.Show(btnPrint, new Point(0, btnPrint.Height));
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = "ProfitAndLoss.xlsx";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = "ProfitAndLoss";

                    // Merge and format header row (Date)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range["B1", "C1"];
                    headerRange.Merge();
                    headerRange.Value = DateTime.Now.ToString("MMM dd, yy");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 10;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

                    int rowIndex = 2;
                    foreach (DataGridViewRow row in dgvCustomer.Rows)
                    {
                        if (row.IsNewRow) continue;

                        string account = row.Cells["lvl1"].Value?.ToString();
                        string amount = row.Cells["balance"].Value?.ToString();

                        var accountCell = worksheet.Cells[rowIndex, 2];
                        var amountCell = worksheet.Cells[rowIndex, 3];

                        accountCell.Value = account.Replace("▶", "").Trim().Replace("▼", "").Trim();
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

            // "ProfitAndLoss Summary" - Bold, size 10
            FormattedText summaryText = center.AddFormattedText("ProfitAndLoss Summary\n", TextFormat.Bold);
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
            foreach (DataGridViewRow row in dgvCustomer.Rows)
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

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProfitAndLoss.pdf");
            renderer.PdfDocument.Save(filePath);
            Process.Start("explorer.exe", filePath);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtFrom, dtTo);
                AddDGVColumns();
                BindDGV();
            }
        }
    }
}