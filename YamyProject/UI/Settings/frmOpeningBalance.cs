using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Default;

namespace YamyProject
{
    public partial class frmOpeningBalance : Form
    {
        DataTable datatable;
        string type = "";
        public frmOpeningBalance(string _type)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.type = _type;
            this.Text = type+" Opening Balance";
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmOpeningBalance_Load(object sender, EventArgs e)
        {
            BindData();
            BindCombos.PopulateAllLevel4Account(cmbAccount);
            if (type == "Customer")
            {
                cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Customer");
            } else if (type == "Vendor")
            {
                cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Vendor");
            }
            else if (type == "Subcontractor")
            {
                cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Vendor");
            }
        }
        private void BindData()
        {
            string query = "select a as 'Name';";
            if (type == "Customer")
            {
                query= @"SELECT code,name,balance debit,0 credit,main_phone phone,mobile,email,IFNULL((SELECT NAME FROM tbl_country WHERE id=country),'') country,IFNULL((SELECT NAME FROM tbl_city WHERE id=city),'') city,trn TRN,building_name FROM tbl_customer";
            }
            else if (type == "Vendor")
            {
                query = @"SELECT code,name,0 debit,balance credit,main_phone phone,mobile,email,IFNULL((SELECT NAME FROM tbl_country WHERE id=country),'') country,IFNULL((SELECT NAME FROM tbl_city WHERE id=city),'') city,trn TRN,building_name FROM tbl_vendor where type ='Vendor'";
            }
            else if (type == "Subcontractor")
            {
                query = @"SELECT code,name,0 debit,balance credit,main_phone phone,mobile,email,IFNULL((SELECT NAME FROM tbl_country WHERE id=country),'') country,IFNULL((SELECT NAME FROM tbl_city WHERE id=city),'') city,trn TRN,building_name FROM tbl_vendor where type='Subcontractor'";
            }
            datatable = DBClass.ExecuteDataTable(query);
            if (datatable != null && datatable.Rows.Count > 0)
            {
                dgvCustomer.DataSource = datatable;
                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            }
        }
        private int GenerateNextCustomerCode()
        {
            int code = 0;
            if (type == "Customer")
            {
                using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_customer"))
                {
                    if (reader.Read() && reader["lastCode"] != DBNull.Value)
                        code = int.Parse(reader["lastCode"].ToString());
                    else
                        code = 10001;
                }
            }
            if (type == "Vendor")
            {
                using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_vendor"))
                {
                    if (reader.Read() && reader["lastCode"] != DBNull.Value)
                        code = int.Parse(reader["lastCode"].ToString());
                    else
                        code = 20001;
                }
            }
            if (type == "Subcontractor")
            {
                using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_vendor"))
                {
                    if (reader.Read() && reader["lastCode"] != DBNull.Value)
                        code = int.Parse(reader["lastCode"].ToString());
                    else
                        code = 20001;
                }
            }
            return code;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string accountId = cmbAccount.SelectedValue?.ToString() ?? "0";
            var loadingForm = new LoadingForm();
            Task.Run(() =>
            {
                if (chkRequiredDate())
            {
                insertData(loadingForm, accountId);
                }
                else
                {
                    // Close the dialog safely from background thread
                    loadingForm.Invoke(new Action(() => loadingForm.Close()));
                }
            });

            loadingForm.ShowDialog();
        }

        private string ConvertFromDecimal(object value)
        {
            return Utilities.ParseDecimalValue(value).ToString();
        }

        private void insertData(Form loadingForm, string accountId)
        {
            try
            {
                int formattedCode = GenerateNextCustomerCode();
                string openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();

                for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                {
                    string code = (formattedCode + 1).ToString("D5");
                    if (dgvCustomer.Rows[i].Cells["name"].Value != null && dgvCustomer.Rows[i].Cells["name"].Value.ToString() != "")
                    {
                        string name = dgvCustomer.Rows[i].Cells["name"].Value.ToString();
                        string debit = dgvCustomer.Rows[i].Cells["debit"].Value.ToString() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["debit"].Value.ToString());
                        string credit = dgvCustomer.Rows[i].Cells["credit"].Value.ToString() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["credit"].Value.ToString());
                        string phone = dgvCustomer.Rows[i].Cells["phone"].Value.ToString();
                        string mobile = dgvCustomer.Rows[i].Cells["mobile"].Value.ToString();
                        string email = dgvCustomer.Rows[i].Cells["email"].Value.ToString();
                        string country = dgvCustomer.Rows[i].Cells["country"].Value.ToString();
                        string city = dgvCustomer.Rows[i].Cells["city"].Value.ToString();
                        string TRN = dgvCustomer.Rows[i].Cells["TRN"].Value.ToString();
                        string buildingName = dgvCustomer.Rows[i].Cells["building_name"].Value.ToString();
                        debit = debit.Replace(",", "");
                        credit = credit.Replace(",", "");
                        if (type == "Customer")
                        {
                            using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_customer WHERE name = @name", DBClass.CreateParameter("name", name)))
                            {
                                if (!reader.Read())
                                {
                                    int countryId = 0, cityId = 0;
                                    if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(city))
                                    {
                                        DataRow[] rows1 = BindDataTable.tableCountries.Select($"name = '{country}'");
                                        if (rows1.Length > 0)
                                        {
                                            countryId = Convert.ToInt32(rows1[0]["id"]);
                                            DataRow[] rows2 = BindDataTable.tableCities.Select($"name = '{city}'");
                                            if (rows2.Length > 0)
                                            {
                                                cityId = Convert.ToInt32(rows2[0]["id"]);
                                            }
                                            else
                                            {
                                                cityId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_city (name,country_id) VALUES (@name,@cId);SELECT LAST_INSERT_ID();",
                                                DBClass.CreateParameter("name", city),
                                                DBClass.CreateParameter("cId", countryId)).ToString());
                                            }
                                        }
                                    }
                                    int customerId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_customer (code, NAME, Cat_id, Balance, DATE, 
                                                main_phone, work_phone, mobile, email, ccemail, website, country, city, region, building_name, account_id, 
                                                trn, facilty_name, active, created_by, created_date, state) 
                                                VALUES(@code, @name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website, 
                                                @country, @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state);
                                                SELECT LAST_INSERT_ID();",
                                                        DBClass.CreateParameter("code", code),
                                                        DBClass.CreateParameter("name", name),
                                                        DBClass.CreateParameter("cat_id", 0),
                                                        DBClass.CreateParameter("balance", decimal.Parse(debit) - decimal.Parse(credit)),
                                                        DBClass.CreateParameter("date", dtpDate.Value.Date),
                                                        DBClass.CreateParameter("main_phone", phone),
                                                        DBClass.CreateParameter("work_phone", ""),
                                                        DBClass.CreateParameter("mobile", mobile),
                                                        DBClass.CreateParameter("email", email),
                                                        DBClass.CreateParameter("ccemail", ""),
                                                        DBClass.CreateParameter("website", ""),
                                                        DBClass.CreateParameter("country", countryId),
                                                        DBClass.CreateParameter("city", cityId),
                                                        DBClass.CreateParameter("region", ""),
                                                        DBClass.CreateParameter("building_name", buildingName),
                                                        DBClass.CreateParameter("account_id", accountId),
                                                        DBClass.CreateParameter("trn", TRN),
                                                        DBClass.CreateParameter("facilty_name", ""),
                                                        DBClass.CreateParameter("active", 0),
                                                        DBClass.CreateParameter("created_by", frmLogin.userId),
                                                        DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                                        DBClass.CreateParameter("state", 0)));

                                    Utilities.LogAudit(frmLogin.userId, "Add Opening Balance", "Opening Balance", customerId, "Added Opening Balance for Customer: " + name);
                                    if (customerId > 0)
                                    {
                                        decimal creditAmount = decimal.Parse(credit);
                                        decimal debitAmount = decimal.Parse(debit);

                                        if (creditAmount != 0)
                                        {
                                            CommonInsert.InsertTransactionEntry(dtpDate.Value.Date, openingBalanceEquity, creditAmount.ToString(), "0",
                                                customerId.ToString(), "0", "Customer Opening Balance", "Opening Balance Equity - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date);

                                            CommonInsert.InsertTransactionEntry(dtpDate.Value.Date, accountId, "0", creditAmount.ToString(),
                                                customerId.ToString(), customerId.ToString(), "Customer Opening Balance", "Opening Balance - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date);

                                        }

                                        if (debitAmount != 0)
                                        {
                                            CommonInsert.InsertTransactionEntry(dtpDate.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                                                customerId.ToString(), "0", "Customer Opening Balance", "Opening Balance Equity - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date);

                                            CommonInsert.InsertTransactionEntry(dtpDate.Value.Date, accountId, debitAmount.ToString(), "0",
                                                customerId.ToString(), customerId.ToString(), "Customer Opening Balance", "Opening Balance - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date);
                                        }
                                    }
                                    formattedCode++;
                                }
                            }
                        }

                        else if (type == "Vendor")
                        {
                            using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_vendor WHERE type='Vendor' AND name = @name", DBClass.CreateParameter("name", name)))
                            {
                                if (!reader.Read())
                                {
                                    int countryId = 0, cityId = 0;
                                    if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(city))
                                    {
                                        DataRow[] rows1 = BindDataTable.tableCountries.Select($"name = '{country}'");
                                        if (rows1.Length > 0)
                                        {
                                            countryId = Convert.ToInt32(rows1[0]["id"]);
                                            DataRow[] rows2 = BindDataTable.tableCities.Select($"name = '{city}'");
                                            if (rows2.Length > 0)
                                            {
                                                cityId = Convert.ToInt32(rows2[0]["id"]);
                                            }
                                            else
                                            {
                                                cityId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_city (name,country_id) VALUES (@name,@cId);SELECT LAST_INSERT_ID();",
                                                DBClass.CreateParameter("name", city),
                                                DBClass.CreateParameter("cId", countryId)).ToString());
                                            }
                                        }
                                    }
                                    int vendorId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_vendor(code,
                                                    NAME, Cat_id, Balance, DATE, main_phone, work_phone, mobile, email, ccemail, website, country, city, region,
                                                    building_name, account_id, trn, facilty_name, active, created_by, created_date, state, type)
                                                    VALUES(@code,@name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website, @country,
                                                    @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state, 'Vendor');SELECT LAST_INSERT_ID();",
                                                              DBClass.CreateParameter("code", code),
                                                              DBClass.CreateParameter("name", name),
                                                              DBClass.CreateParameter("cat_id", 0),
                                                              DBClass.CreateParameter("balance", decimal.Parse(credit) - decimal.Parse(debit)),
                                                              DBClass.CreateParameter("date", dtpDate.Value.Date),
                                                              DBClass.CreateParameter("main_phone", phone),
                                                              DBClass.CreateParameter("work_phone", ""),
                                                              DBClass.CreateParameter("mobile", mobile),
                                                              DBClass.CreateParameter("email", email),
                                                              DBClass.CreateParameter("ccemail", ""),
                                                              DBClass.CreateParameter("website", ""),
                                                              DBClass.CreateParameter("country", countryId),
                                                              DBClass.CreateParameter("city", cityId),
                                                              DBClass.CreateParameter("region", ""),
                                                              DBClass.CreateParameter("building_name", buildingName),
                                                              DBClass.CreateParameter("account_id", accountId),
                                                              DBClass.CreateParameter("trn", TRN),
                                                              DBClass.CreateParameter("facilty_name", ""),
                                                              DBClass.CreateParameter("active", 0),
                                                              DBClass.CreateParameter("created_by", frmLogin.userId),
                                                              DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                                              DBClass.CreateParameter("state", 0)).ToString());

                                    Utilities.LogAudit(frmLogin.userId, "Add Opening Balance", "Opening Balance", vendorId, "Added Opening Balance for Vendor: " + name);

                                    if (vendorId > 0)
                                    {
                                        decimal creditAmount = decimal.Parse(credit);
                                        decimal debitAmount = decimal.Parse(debit);
                                        
                                        if (creditAmount != 0)
                                        {
                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, openingBalanceEquity, creditAmount.ToString(), "0",
                                              vendorId.ToString(), "0", "Vendor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, accountId, "0", creditAmount.ToString(),
                                               vendorId.ToString(), vendorId.ToString(), "Vendor Opening Balance", "OPENING BALANCE", "Account Payable - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                                        }

                                        if (debitAmount != 0)
                                        {
                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                                                    vendorId.ToString(), "0", "Vendor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, accountId, debitAmount.ToString(), "0",
                                               vendorId.ToString(), vendorId.ToString(), "Vendor Opening Balance", "OPENING BALANCE", "Account Payable - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                                        }
                                    }
                                    formattedCode++;
                                }

                            }
                        }
                        else if (type == "Subcontractor")
                        {
                            using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_vendor WHERE type='Subcontractor' AND name = @name", DBClass.CreateParameter("name", name)))
                            {
                                if (!reader.Read())
                                {
                                    int countryId = 0, cityId = 0;
                                    if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(city))
                                    {
                                        DataRow[] rows1 = BindDataTable.tableCountries.Select($"name = '{country}'");
                                        if (rows1.Length > 0)
                                        {
                                            countryId = Convert.ToInt32(rows1[0]["id"]);
                                            DataRow[] rows2 = BindDataTable.tableCities.Select($"name = '{city}'");
                                            if (rows2.Length > 0)
                                            {
                                                cityId = Convert.ToInt32(rows2[0]["id"]);
                                            }
                                            else
                                            {
                                                cityId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_city (name,country_id) VALUES (@name,@cId);SELECT LAST_INSERT_ID();",
                                                DBClass.CreateParameter("name", city),
                                                DBClass.CreateParameter("cId", countryId)).ToString());
                                            }
                                        }
                                    }
                                    int SubcontractorId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_vendor(code,
                                                    NAME, Cat_id, Balance, DATE, main_phone, work_phone, mobile, email, ccemail, website, country, city, region,
                                                    building_name, account_id, trn, facilty_name, active, created_by, created_date, state, type)
                                                    VALUES(@code,@name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website, @country,
                                                    @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state, 'Subcontractor');SELECT LAST_INSERT_ID();",
                                                              DBClass.CreateParameter("code", code),
                                                              DBClass.CreateParameter("name", name),
                                                              DBClass.CreateParameter("cat_id", 0),
                                                              DBClass.CreateParameter("balance", decimal.Parse(credit) - decimal.Parse(debit)),
                                                              DBClass.CreateParameter("date", dtpDate.Value.Date),
                                                              DBClass.CreateParameter("main_phone", phone),
                                                              DBClass.CreateParameter("work_phone", ""),
                                                              DBClass.CreateParameter("mobile", mobile),
                                                              DBClass.CreateParameter("email", email),
                                                              DBClass.CreateParameter("ccemail", ""),
                                                              DBClass.CreateParameter("website", ""),
                                                              DBClass.CreateParameter("country", countryId),
                                                              DBClass.CreateParameter("city", cityId),
                                                              DBClass.CreateParameter("region", ""),
                                                              DBClass.CreateParameter("building_name", buildingName),
                                                              DBClass.CreateParameter("account_id", accountId),
                                                              DBClass.CreateParameter("trn", TRN),
                                                              DBClass.CreateParameter("facilty_name", ""),
                                                              DBClass.CreateParameter("active", 0),
                                                              DBClass.CreateParameter("created_by", frmLogin.userId),
                                                              DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                                              DBClass.CreateParameter("state", 0)).ToString());

                                    Utilities.LogAudit(frmLogin.userId, "Add Opening Balance", "Opening Balance", SubcontractorId, "Added Opening Balance for Subcontractor: " + name);

                                    if (SubcontractorId > 0)
                                    {
                                        decimal creditAmount = decimal.Parse(credit);
                                        decimal debitAmount = decimal.Parse(debit);

                                        if (creditAmount != 0)
                                        {
                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, openingBalanceEquity, creditAmount.ToString(), "0",
                                              SubcontractorId.ToString(), "0", "Subcontractor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, accountId, "0", creditAmount.ToString(),
                                               SubcontractorId.ToString(), SubcontractorId.ToString(), "Subcontractor Opening Balance", "OPENING BALANCE", "Account Payable - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                                        }

                                        if (debitAmount != 0)
                                        {
                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                                                    SubcontractorId.ToString(), "0", "Subcontractor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                                            CommonInsert.addTransactionEntry(dtpDate.Value.Date, accountId, debitAmount.ToString(), "0",
                                               SubcontractorId.ToString(), SubcontractorId.ToString(), "Subcontractor Opening Balance", "OPENING BALANCE", "Account Payable - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                                        }
                                    }
                                    formattedCode++;
                                }

                            }
                        }
                    }
                }
                loadingForm.Invoke(new Action(() => loadingForm.Close()));
                MessageBox.Show("Successfully saved! "+type+" List", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                loadingForm.Invoke(new Action(() => loadingForm.Close()));
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool chkRequiredDate()
        {
            if(dgvCustomer.Rows.Count == 1)
            {
                MessageBox.Show("Please Enter Date First");
                return false;
            }

            return true;
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
                    type = string.IsNullOrEmpty(type) ? "sheet1" : type;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(type);
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
            if (chkCustomizedFile.Checked)
            {
                frmImportOpeningBalance importForm = new frmImportOpeningBalance();
                if (importForm.ShowDialog() == DialogResult.OK)
                {
                    DataTable importedData = importForm.ImportedData;
                    dgvCustomer.DataSource = importedData;
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
                }
            }
            else {
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
    }
}

public class frmImportOpeningBalance : Form
{
    private TextBox txtStartCell, txtEndCell, txtNameCell, txtDebitCell, txtCreditCell, txtPhoneCell, txtMobileCell, txtEmailCell, txtCountryCell, txtCityCell, txtTaxNoCell, txtAddressCell;
    private Button btnLoadExcel, btnImport;
    private DataGridView dataGridView;
    private string excelPath = string.Empty;
    DataTable dtFiltered = new DataTable();
    public DataTable ImportedData { get; private set; }

    public frmImportOpeningBalance()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        this.Text = "Excel Import Form";
        this.Size = new Size(1624, 900);
        this.StartPosition = FormStartPosition.CenterScreen;

        // ==== Top Panel ====
        Panel panelTop = new Panel()
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(10),
        };

        // Starting X position for controls
        int x = 10;
        int spacing = 8;
        int labelWidth = 80;
        int textboxWidth = 15;
        int controlHeight = 28;

        // Excel buttons
        btnLoadExcel = new Button() { Text = "Load Excel", Size = new Size(100, controlHeight), Location = new Point(x, 10) };
        x += 100 + spacing;

        btnImport = new Button() { Text = "Set Data", Size = new Size(80, controlHeight), Location = new Point(x, 10) };
        x += 80 + spacing;

        // Start Cell
        panelTop.Controls.Add(new Label() { Text = "Start Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtStartCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 13, 12) };
        x += textboxWidth + spacing;

        // End Cell
        panelTop.Controls.Add(new Label() { Text = "End Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtEndCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 15, 12) };
        x += textboxWidth + spacing;

        // Name Cell
        panelTop.Controls.Add(new Label() { Text = "Name Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtNameCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 6, 12) };
        x += textboxWidth + spacing;

        // Debit Cell
        panelTop.Controls.Add(new Label() { Text = "Debit Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtDebitCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 10, 12) };
        x += textboxWidth + spacing;

        // Credit Cell
        panelTop.Controls.Add(new Label() { Text = "Credit Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtCreditCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 5, 12) };
        x += textboxWidth + spacing;

        // Phone Cell
        panelTop.Controls.Add(new Label() { Text = "Phone Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtPhoneCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Mobile Cell
        panelTop.Controls.Add(new Label() { Text = "Mobile Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtMobileCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Email Cell
        panelTop.Controls.Add(new Label() { Text = "Email Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtEmailCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 8, 12) };
        x += textboxWidth + spacing;

        // Country Cell
        panelTop.Controls.Add(new Label() { Text = "Country Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtCountryCell = new TextBox() { Width = textboxWidth, Location = new Point(x + 6, 12) };
        x += textboxWidth + spacing;

        // City Cell
        panelTop.Controls.Add(new Label() { Text = "City Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtCityCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 14, 12) };
        x += textboxWidth + spacing;

        // Address Cell
        panelTop.Controls.Add(new Label() { Text = "Address Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtAddressCell = new TextBox() { Width = textboxWidth, Location = new Point(x + 10, 12) };
        x += textboxWidth + spacing;

        // Tax No Cell
        panelTop.Controls.Add(new Label() { Text = "TaxNo Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtTaxNoCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Add controls
        panelTop.Controls.Add(btnLoadExcel);
        panelTop.Controls.Add(btnImport);
        panelTop.Controls.Add(txtStartCell);
        panelTop.Controls.Add(txtEndCell);
        panelTop.Controls.Add(txtNameCell);
        panelTop.Controls.Add(txtDebitCell);
        panelTop.Controls.Add(txtCreditCell);
        panelTop.Controls.Add(txtPhoneCell);
        panelTop.Controls.Add(txtMobileCell);
        panelTop.Controls.Add(txtEmailCell);
        panelTop.Controls.Add(txtCountryCell);
        panelTop.Controls.Add(txtCityCell);
        panelTop.Controls.Add(txtAddressCell);
        panelTop.Controls.Add(txtTaxNoCell);

        // Add top panel to form
        this.Controls.Add(panelTop);


        btnLoadExcel.Click += BtnLoadExcel_Click;
        btnImport.Click += BtnImport_Click;

        // ==== Main Panel ====
        Panel panelMain = new Panel()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
        };

        dataGridView = new DataGridView()
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        panelMain.Controls.Add(dataGridView);

        // Add both panels to form
        this.Controls.Add(panelMain);
        this.Controls.Add(panelTop);

        // Digit-only for start/end
        txtStartCell.KeyPress += TxtIntOnly_KeyPress;
        txtEndCell.KeyPress += TxtIntOnly_KeyPress;

        // Letter-only for others
        txtNameCell.KeyPress += TxtLettersOnly_KeyPress;
        txtDebitCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCreditCell.KeyPress += TxtLettersOnly_KeyPress;
        txtPhoneCell.KeyPress += TxtLettersOnly_KeyPress;
        txtMobileCell.KeyPress += TxtLettersOnly_KeyPress;
        txtEmailCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCountryCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCityCell.KeyPress += TxtLettersOnly_KeyPress;
        txtAddressCell.KeyPress += TxtLettersOnly_KeyPress;
        txtTaxNoCell.KeyPress += TxtLettersOnly_KeyPress;
    }
    private void BtnImport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            MessageBox.Show("Please load an Excel file first.");
            return;
        }
        else
        {
            //dataGridView.Rows.Clear();
            //dataGridView.Columns.Clear();
            if (dataGridView.Rows.Count > 0)
            {
                int startCol = ToNumber(txtStartCell.Text);
                int endCol = ToNumber(txtEndCell.Text);

                // Check for invalid inputs
                if (startCol == -1 || endCol == -1 || endCol < startCol)
                {
                    MessageBox.Show("Invalid start or end column.");
                    return;
                }

                // Map specific fields to column numbers
                int colName = ExcelColumnNameToNumber(txtNameCell.Text);
                int colDebit = ExcelColumnNameToNumber(txtDebitCell.Text);
                int colCredit = ExcelColumnNameToNumber(txtCreditCell.Text);
                int colPhone = ExcelColumnNameToNumber(txtPhoneCell.Text);
                int colMobile = ExcelColumnNameToNumber(txtMobileCell.Text);
                int colEmail = ExcelColumnNameToNumber(txtEmailCell.Text);
                int colCountry = ExcelColumnNameToNumber(txtCountryCell.Text);
                int colCity = ExcelColumnNameToNumber(txtCityCell.Text);
                int colAddress = ExcelColumnNameToNumber(txtAddressCell.Text);
                int colTaxNo = ExcelColumnNameToNumber(txtTaxNoCell.Text);

                using (var package = new ExcelPackage(new FileInfo(excelPath)))
                {
                    dtFiltered.Columns.Add("code");
                    dtFiltered.Columns.Add("name");
                    dtFiltered.Columns.Add("debit");
                    dtFiltered.Columns.Add("credit");
                    dtFiltered.Columns.Add("phone");
                    dtFiltered.Columns.Add("mobile");
                    dtFiltered.Columns.Add("email");
                    dtFiltered.Columns.Add("country");
                    dtFiltered.Columns.Add("city");
                    dtFiltered.Columns.Add("TRN");
                    dtFiltered.Columns.Add("building_name");
                    var ws = package.Workbook.Worksheets[0];
                    for (int row = ws.Dimension.Start.Row + 1; row <= ws.Dimension.End.Row; row++)
                    {
                        if (IsRowEmpty(ws, row, endCol)) continue;

                        DataRow dr = dtFiltered.NewRow();

                        dr["Code"] = "";
                        dr["Name"] = (colName != -1) ? ws.Cells[row, colName].Text : "";
                        dr["Debit"] = (colDebit != -1) ? ws.Cells[row, colDebit].Text : "0";
                        dr["Credit"] = (colCredit != -1) ? ws.Cells[row, colCredit].Text : "0";
                        dr["Phone"] = (colPhone != -1) ? ws.Cells[row, colPhone].Text : "";
                        dr["Mobile"] = (colMobile != -1) ? ws.Cells[row, colMobile].Text : "";
                        dr["Email"] = (colEmail != -1) ? ws.Cells[row, colEmail].Text : "";
                        dr["Country"] = (colCountry != -1) ? ws.Cells[row, colCountry].Text : "";
                        dr["City"] = (colCity != -1) ? ws.Cells[row, colCity].Text : "";
                        dr["building_name"] = (colAddress != -1) ? ws.Cells[row, colAddress].Text : "";
                        dr["TRN"] = (colTaxNo != -1) ? ws.Cells[row, colTaxNo].Text : "";

                        dtFiltered.Rows.Add(dr);
                    }

                    // Now you can bind the filtered table somewhere or return it
                    dataGridView.DataSource = dtFiltered;
                }

                this.ImportedData = dtFiltered;
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            else
            {
                MessageBox.Show("Please load an Excel file first.");
                return;
            }
        }
    }

    private int ToNumber(string text)
    {
        if (string.IsNullOrEmpty(text)) return -1;
        else return int.Parse(text);
    }

    int ExcelColumnNameToNumber(string columnName)
    {
        if (string.IsNullOrEmpty(columnName)) return -1;

        columnName = columnName.ToUpperInvariant();
        int sum = 0;
        foreach (char c in columnName)
        {
            if (c < 'A' || c > 'Z') return -1; // invalid char
            sum *= 26;
            sum += (c - 'A' + 1);
        }
        return sum;
    }
    private void TxtIntOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Allow control keys like Backspace
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true; // Reject non-digit keys
        }
    }
    private void TxtLettersOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
        {
            e.Handled = true; // block non-letter keys
        }
    }

    private void BtnLoadExcel_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            excelPath = ofd.FileName;
            ImportToGrid();
        }
    }

    private void ImportToGrid()
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            MessageBox.Show("Please load an Excel file first.");
            return;
        }

        try
        {
            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                var ws = package.Workbook.Worksheets[0];
                int startRow = ws.Dimension.Start.Row;
                int endRow = ws.Dimension.End.Row;
                int startCol = ws.Dimension.Start.Column;
                int endCol = ws.Dimension.End.Column;

                DataTable dt = new DataTable();

                // First column: "Index" or "Row"
                dt.Columns.Add("Index");

                // Set headers manually as A, B, C, etc.
                for (int col = startCol; col <= endCol; col++)
                {
                    string colLetter = GetExcelColumnLetter(col);
                    dt.Columns.Add(colLetter);
                }

                // Fill data from all rows
                for (int row = startRow; row <= endRow; row++)
                {
                    DataRow dr = dt.NewRow();

                    dr["Index"] = row.ToString();  // Add Excel-like row number

                    for (int col = startCol; col <= endCol; col++)
                    {
                        dr[col - startCol + 1] = ws.Cells[row, col].Text;
                    }

                    dt.Rows.Add(dr);
                }

                dataGridView.DataSource = dt;

                dataGridView.ReadOnly = true;
                dataGridView.RowHeadersVisible = false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error reading Excel: {ex.Message}");
        }
    }
    private string GetExcelColumnLetter(int col)
    {
        string columnString = "";
        int columnNumber = col;
        while (columnNumber > 0)
        {
            int currentLetterNumber = (columnNumber - 1) % 26;
            char currentLetter = (char)(currentLetterNumber + 65);
            columnString = currentLetter + columnString;
            columnNumber = (columnNumber - 1) / 26;
        }
        return columnString;
    }

    private bool IsRowEmpty(ExcelWorksheet ws, int row, int maxCol)
    {
        for (int col = 1; col <= maxCol; col++)
        {
            if (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Text))
                return false;
        }
        return true;
    }
}