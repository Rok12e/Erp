using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmChartOfAccountList : Form
    {
        DataTable datatable;
        public frmChartOfAccountList(string _type)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "Chart Of Account Level";
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmChartOfAccountList_Load(object sender, EventArgs e)
        {
            BindData();
            BindCombos.PopulateAllLevel4Account(cmbAccount);
        }
        private void BindData()
        {
            datatable = DBClass.ExecuteDataTable(@"SELECT l1.code AS `Level1 Code`,l1.name AS `Level1 Name`,
                                                    l2.code AS `Level2 Code`,l2.name AS `Level2 Name`,
                                                    l3.code AS `Level3 Code`,l3.name AS `Level3 Name`,
                                                    l4.code AS `Level4 Code`,l4.name AS `Level4 Name`,
                                                    l4.debit `Level4 Debit`,l4.credit `Level4 Credit`
                                                    FROM tbl_coa_level_4 l4,tbl_coa_level_3 l3,tbl_coa_level_2 l2,tbl_coa_level_1 l1 
                                                    WHERE l4.main_id=l3.id AND l3.main_id=l2.id AND l2.main_id=l1.id;");
            if (datatable != null && datatable.Rows.Count > 0)
            {
                dgvCustomer.DataSource = datatable;
                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                dgvCustomer.Columns["Level4 Debit"].DefaultCellStyle.Alignment = dgvCustomer.Columns["Level4 Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvCustomer.Columns["Level4 Debit"].DefaultCellStyle.Format = dgvCustomer.Columns["Level4 Credit"].DefaultCellStyle.Format = "N2";

                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);

                openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();
                if (int.Parse(openingBalanceEquity) == 0)
                {
                    object result = DBClass.ExecuteScalar(@"SELECT id FROM tbl_coa_level_4 WHERE name = 'Opening Balance Equity'");
                    if (result != null && result != DBNull.Value)
                    {
                        openingBalanceEquity = result.ToString();
                    }
                }
            }
        }

        string openingBalanceEquity = "0";

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (chkRequiredDate())
            {
                insertData();
            }
        }

        private void insertData()
        {
            DateTime obDate = new DateTime(DateTime.Now.Year, 1, 1);
            try
            {
                //string accountId = cmbAccount.SelectedValue?.ToString() ?? "0";

                for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                {
                    int level1Id = 0, level2Id = 0, level3Id = 0, level4Id = 0;
                    //level 1
                    if (dgvCustomer.Rows[i].Cells["Level1 Name"].Value != null && dgvCustomer.Rows[i].Cells["Level1 Name"].Value.ToString() != "")
                    {
                        string code = dgvCustomer.Rows[i].Cells["Level1 Code"].Value.ToString();
                        string name = dgvCustomer.Rows[i].Cells["Level1 Name"].Value.ToString();

                        using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_coa_level_1 WHERE name = @name or code=@code",
                            DBClass.CreateParameter("name", name),
                            DBClass.CreateParameter("code", code)))
                        {
                            if (reader.Read())
                            {
                                level1Id = Convert.ToInt32(reader["id"]);
                            }
                            else
                            {
                                level1Id = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_coa_level_1 (code, name) 
                                                VALUES(@code, @name);
                                                SELECT LAST_INSERT_ID();",
                                                    DBClass.CreateParameter("code", code),
                                                    DBClass.CreateParameter("name", name)).ToString());
                            }
                        }
                    }
                    //level 2
                    if (dgvCustomer.Rows[i].Cells["Level2 Name"].Value != null && dgvCustomer.Rows[i].Cells["Level2 Name"].Value.ToString() != "")
                    {
                        string code = dgvCustomer.Rows[i].Cells["Level2 Code"].Value.ToString();
                        string name = dgvCustomer.Rows[i].Cells["Level2 Name"].Value.ToString();

                        using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_coa_level_2 WHERE name = @name or code=@code",
                            DBClass.CreateParameter("name", name),
                            DBClass.CreateParameter("code", code)))
                        {
                            if (reader.Read())
                            {
                                level2Id = Convert.ToInt32(reader["id"]);
                            }
                            else
                            {
                                level2Id = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_coa_level_2 (code, name,main_id) 
                                                VALUES(@code, @name,@mainId);
                                                SELECT LAST_INSERT_ID();",
                                                    DBClass.CreateParameter("code", code),
                                                    DBClass.CreateParameter("name", name),
                                                    DBClass.CreateParameter("mainId", level1Id.ToString())).ToString());
                            }
                        }
                    }
                    //level 3
                    if (dgvCustomer.Rows[i].Cells["Level3 Name"].Value != null && dgvCustomer.Rows[i].Cells["Level3 Name"].Value.ToString() != "")
                    {
                        string code = dgvCustomer.Rows[i].Cells["Level3 Code"].Value.ToString();
                        string name = dgvCustomer.Rows[i].Cells["Level3 Name"].Value.ToString();

                        using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_coa_level_3 WHERE name = @name or code=@code",
                            DBClass.CreateParameter("name", name),
                            DBClass.CreateParameter("code", code)))
                        {
                            if (reader.Read())
                            {
                                level3Id = Convert.ToInt32(reader["id"]);
                            }
                            else
                            {
                                level3Id = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_coa_level_3 (code, name,main_id) 
                                                VALUES(@code, @name,@mainId);
                                                SELECT LAST_INSERT_ID();",
                                                    DBClass.CreateParameter("code", code),
                                                    DBClass.CreateParameter("name", name),
                                                    DBClass.CreateParameter("mainId", level2Id.ToString())).ToString());
                            }
                        }
                    }
                    //level 4
                    if (dgvCustomer.Rows[i].Cells["Level4 Name"].Value != null && dgvCustomer.Rows[i].Cells["Level4 Name"].Value.ToString() != "")
                    {
                        string code = dgvCustomer.Rows[i].Cells["Level4 Code"].Value.ToString();
                        string name = dgvCustomer.Rows[i].Cells["Level4 Name"].Value.ToString();
                        string debit = "0", credit = "0";
                        
                        if(dgvCustomer.Rows[i].Cells["Level4 Debit"].Value != null && dgvCustomer.Rows[i].Cells["Level4 Debit"].Value.ToString() != "") 
                            debit = Convert.ToDecimal(dgvCustomer.Rows[i].Cells["Level4 Debit"].Value.ToString()).ToString("0.00");
                        
                        if(dgvCustomer.Rows[i].Cells["Level4 Credit"].Value != null && dgvCustomer.Rows[i].Cells["Level4 Credit"].Value.ToString() != "")
                            credit = Convert.ToDecimal(dgvCustomer.Rows[i].Cells["Level4 Credit"].Value.ToString()).ToString("0.00");

                        using (var reader = DBClass.ExecuteReader("SELECT id,main_id FROM tbl_coa_level_4 WHERE name = @name or code=@code", 
                            DBClass.CreateParameter("name", name),
                            DBClass.CreateParameter("code", code)))
                        {
                            if (reader.Read())
                            {
                                level4Id = Convert.ToInt32(reader["id"]);
                            }
                            else
                            {
                                level4Id = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_coa_level_4 (code, name,main_id,debit,credit,date) 
                                                VALUES(@code, @name,@mainId,@debit,@credit,@date);
                                                SELECT LAST_INSERT_ID();",
                                                    DBClass.CreateParameter("code", code),
                                                    DBClass.CreateParameter("name", name),
                                                    DBClass.CreateParameter("mainId", level3Id.ToString()),
                                                    DBClass.CreateParameter("debit", debit.ToString()),
                                                    DBClass.CreateParameter("credit", credit.ToString()),
                                                    DBClass.CreateParameter("date", obDate.Date)).ToString());

                                decimal creditAmount = decimal.Parse(credit);
                                decimal debitAmount = decimal.Parse(debit);

                                if (int.Parse(openingBalanceEquity) > 0)
                                {
                                    if (creditAmount != 0)
                                    {
                                        CommonInsert.addTransactionEntry(obDate.Date, openingBalanceEquity, creditAmount.ToString(), "0",
                                          level4Id.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Opening Balance Equity - Ledger", frmLogin.userId, DateTime.Now.Date, "");

                                        CommonInsert.addTransactionEntry(obDate.Date, level4Id.ToString(), "0", creditAmount.ToString(),
                                           level4Id.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Account Payable - Ledger Code ", frmLogin.userId, DateTime.Now.Date, "");
                                    }

                                    if (debitAmount != 0)
                                    {
                                        CommonInsert.addTransactionEntry(obDate.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                                                level4Id.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Opening Balance Equity - Ledger Code ", frmLogin.userId, DateTime.Now.Date, "");

                                        CommonInsert.addTransactionEntry(obDate.Date, level4Id.ToString(), debitAmount.ToString(), "0",
                                           level4Id.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Account Payable - Ledger Code - ", frmLogin.userId, DateTime.Now.Date, "");
                                    }
                                }
                            }
                        }
                    }
                }

                MessageBox.Show("Successfully saved! ChartOfAccount List", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool chkRequiredDate()
        {
            if(dgvCustomer.Rows.Count == 1)
            {
                MessageBox.Show("Please Enter Data First");
                return false;
            }
            else {
                bool isValid = CheckForDuplicates(dgvCustomer);
                if (!isValid)
                {
                    MessageBox.Show("Please Check Duplicate Code and Name");
                    return false;
                }

            }

            return true;
        }
        private bool CheckForDuplicates(DataGridView dgvCustomer)
        {
            var levelNames = new[] { "Level1", "Level2", "Level3", "Level4" };
            var nameSets = new Dictionary<string, HashSet<string>>();
            var codeSets = new Dictionary<string, HashSet<string>>();

            // Initialize HashSets for each level
            foreach (var level in levelNames)
            {
                nameSets[level] = new HashSet<string>();
                codeSets[level] = new HashSet<string>();
            }

            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (var level in levelNames)
                {
                    string nameCol = $"{level} Name";
                    string codeCol = $"{level} Code";

                    string name = row.Cells[nameCol].Value?.ToString().Trim() ?? "";
                    string code = row.Cells[codeCol].Value?.ToString().Trim() ?? "";

                    // === Check Empty Name ===
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        MessageBox.Show($"Empty {level} Name found in the data.", $"{level} Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // === Check Empty Code ===
                    if (string.IsNullOrWhiteSpace(code))
                    {
                        MessageBox.Show($"Empty {level} Code found for Name: {name}", $"{level} Code Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // === Check Duplicate Name ===
                    if (!nameSets[level].Add(name))
                    {
                        MessageBox.Show($"Duplicate {level} Name found: {name}", $"{level} Name Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // === Check Duplicate Code ===
                    if (!codeSets[level].Add(code))
                    {
                        MessageBox.Show($"Duplicate {level} Code found: {code}", $"{level} Code Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
            }

            return true; // No duplicates or missing values found
        }

        private decimal GetDecimalValue(DataGridViewRow row, string columnName)
        {
            decimal result;
            var cellValue = row.Cells[columnName].Value;
            if (cellValue != null && decimal.TryParse(cellValue.ToString(), out result))
                return result;
            else
                return 0;
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ChartOfAccount");
                    worksheet.Cells["A1"].LoadFromDataTable(datatable, true);
                    
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);
                        MessageBox.Show("Export successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                openFileDialog.Title = "Select Excel File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    LoadExcelSheets(filePath);
                }
            }
        }
        private void LoadExcelSheets(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                dgvCustomer.Columns.Clear();
                dgvCustomer.DataSource = null;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    if (sheet.Dimension != null)
                    {
                        DataTable dataTable = new DataTable(sheet.Name);

                        for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                        {
                            string columnName = sheet.Cells[1, col].Text.Trim();
                            if (string.IsNullOrWhiteSpace(columnName)) columnName = "Column " + col;
                            dataTable.Columns.Add(columnName);
                        }

                        for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                        {
                            bool isRowEmpty = true;
                            DataRow dataRow = dataTable.NewRow();

                            for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                            {
                                string cellValue = sheet.Cells[row, col].Text.Trim();

                                if (!string.IsNullOrWhiteSpace(cellValue))
                                    isRowEmpty = false;

                                dataRow[col - 1] = cellValue;
                            }

                            if (!isRowEmpty)
                                dataTable.Rows.Add(dataRow);
                        }

                        //RemoveEmptyColumns(dataTable);

                        dgvCustomer.DataSource = dataTable;
                        foreach (DataGridViewColumn column in dgvCustomer.Columns)
                        {
                            if (column.HeaderText == "name" || column.HeaderText == "country" || column.HeaderText == "building_name" || column.HeaderText == "TRN")
                            {
                                column.Width = 550;
                            }
                            else
                            {
                                column.Width = 200;
                            }
                        }

                        LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
                    }
                }
            }
        }
        private void RemoveEmptyColumns(DataTable dt)
        {
            List<DataColumn> emptyColumns = new List<DataColumn>();

            foreach (DataColumn col in dt.Columns)
            {
                bool isEmpty = true;

                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row[col].ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                    emptyColumns.Add(col);
            }

            foreach (DataColumn col in emptyColumns)
            {
                dt.Columns.Remove(col);
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text) && txtCode.Text.Length > 0)
            {
                //
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
