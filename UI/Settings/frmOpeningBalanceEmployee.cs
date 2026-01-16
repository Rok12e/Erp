using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Default;

namespace YamyProject
{
    public partial class frmOpeningBalanceEmployee : Form
    {
        DataTable datatable;
        public frmOpeningBalanceEmployee()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "Employee Date Import";
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmOpeningBalanceEmployee_Load(object sender, EventArgs e)
        {
            BindData();
            BindCombos.PopulateAllLevel4Account(cmbAccount);
            cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Vendor");
        }
        private void BindData()
        {
            string query = @"SELECT code,name,address,phone,birth_day,BasicSalary,e.WorkDays,e.workinghours,e.CountryOfIssue, IFNULL((SELECT NAME FROM tbl_city WHERE id=city_id),'') city FROM tbl_employee;";
            datatable = DBClass.ExecuteDataTable(query);
            if (datatable != null && datatable.Rows.Count > 0)
            {
                dgvCustomer.DataSource = datatable;
                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            }
        }
        private int GenerateNextEmployeeCode()
        {
            int code = 0;

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_employee"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 30001;
            }

            return code;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string accountId = cmbAccount.SelectedValue?.ToString() ?? "0";

            var _AccountNameId = frmLogin.defaultAccounts.ContainsKey("Employee")
               ? frmLogin.defaultAccounts["Employee"] : 0;
            var _AccruedSalariesId = GetDefaultAccountId("Accrued Salaries");
            var _EmployeeRecivableId = GetDefaultAccountId("Employee Receivable");
            var _AcroalLeaveSalaryId = GetDefaultAccountId("Acroal Leave Salary");
            var _GratuitId = GetDefaultAccountId("Gratuit");
            var _PettyCashId = frmLogin.defaultAccounts.ContainsKey("Petty Cash")
               ? frmLogin.defaultAccounts["Petty Cash"] : 0;

            var loadingForm = new LoadingForm();
            Task.Run(() =>
            {
                if (chkRequiredDate())
                {
                    insertEmployeeData(loadingForm, _AccountNameId.ToString(), _AccruedSalariesId.ToString(), _EmployeeRecivableId.ToString(), _AcroalLeaveSalaryId.ToString(), _GratuitId.ToString(), _PettyCashId.ToString());
                }
                else
                {
                    // Close the dialog safely from background thread
                    loadingForm.Invoke(new Action(() => loadingForm.Close()));
                }
            });

            loadingForm.ShowDialog();
        }

        private int GetDefaultAccountId(string category)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT account_id FROM tbl_coa_config WHERE category = @category",
                DBClass.CreateParameter("category", category)))
                if (reader.Read())
                    return Convert.ToInt32(reader["account_id"]);
                else
                    return 0;
        }

        private string ConvertFromDecimal(object value)
        {
            return Utilities.ParseDecimalValue(value).ToString();
        }

        private void insertEmployeeData(Form loadingForm,string _AccountNameId, string _AccruedSalariesId, string _EmployeeRecivableId, string _AcroalLeaveSalaryId, string _GratuitIdstring, string _PettyCashId)
        {
            try
            {
                int formattedCode = GenerateNextEmployeeCode(); // Similar to customer code generator

                for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                {
                    string code = (formattedCode + 1).ToString("D5");
                    if (dgvCustomer.Rows[i].Cells["name"].Value != null && dgvCustomer.Rows[i].Cells["name"].Value.ToString() != "")
                    {
                        string name = dgvCustomer.Rows[i].Cells["name"].Value.ToString();
                        string phone = dgvCustomer.Rows[i].Cells["phone"].Value?.ToString() ?? "";
                        string address = dgvCustomer.Rows[i].Cells["address"].Value?.ToString() ?? "";
                        string email = "";
                        string birthDayStr = dgvCustomer.Rows[i].Cells["birth_day"].Value?.ToString() ?? "";
                        string basicSalaryStr = dgvCustomer.Rows[i].Cells["BasicSalary"].Value?.ToString() ?? "0";
                        string workDaysStr = dgvCustomer.Rows[i].Cells["WorkDays"].Value?.ToString() ?? "0";
                        string workingHoursStr = dgvCustomer.Rows[i].Cells["workinghours"].Value?.ToString() ?? "0";
                        string countryOfIssue = dgvCustomer.Rows[i].Cells["CountryOfIssue"].Value?.ToString() ?? "";
                        string city = dgvCustomer.Rows[i].Cells["city"].Value?.ToString() ?? "";

                        DateTime? birthDay = string.IsNullOrEmpty(birthDayStr) ? (DateTime?)null : Convert.ToDateTime(birthDayStr);
                        decimal basicSalary = string.IsNullOrEmpty(basicSalaryStr) ? 0 : decimal.Parse(basicSalaryStr);
                        int workDays = string.IsNullOrEmpty(workDaysStr) ? 0 : int.Parse(workDaysStr);
                        int workingHours = string.IsNullOrEmpty(workingHoursStr) ? 0 : int.Parse(workingHoursStr);

                        // Handle Country + City
                        int cityId = 212;
                        if (!string.IsNullOrEmpty(city))
                        {
                            DataRow[] rows = BindDataTable.tableCities.Select($"name = '{city}'");
                            if (rows.Length > 0)
                            {
                                cityId = Convert.ToInt32(rows[0]["id"]);
                            }
                            else
                            {
                                cityId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_city (name) VALUES (@name); SELECT LAST_INSERT_ID();",
                                    DBClass.CreateParameter("name", city)));
                            }
                        }

                        // Prevent duplicates (by name + phone)
                        using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_employee WHERE name=@name AND phone=@phone",
                                    DBClass.CreateParameter("name", name),
                                    DBClass.CreateParameter("phone", phone)))
                        {
                            if (!reader.Read())
                            {
                                int empId = Convert.ToInt32(DBClass.ExecuteScalar(@"
                                            INSERT INTO tbl_employee 
                                            (code, name, birth_day, Social_Status, city_id, address, phone, Email, 
                                             Social_Insurance_Number, EmergencyName, EmergencyAddress, EmergencyPhone, Relation, 
                                             PassportNumber, CountryOfIssue, PassportIssueDate, PassportExpiryDate, 
                                             WorkContractNumber, Position_id, Department_id, WorkContractType, WorkDays, workinghours, 
                                             ContractIssueDate, ContractExpiryDate, ResidencyFileNumber, ResidencyIssuingAuthority, 
                                             ResidencyIssueDate, ResidencyExpiryDate, EmiratesIDFileNumber, EmiratesIDIssuingAuthority, 
                                             EmiratesIDIssueDate, EmiratesIDExpiryDate, BasicSalary, HousingAllowance, 
                                             TransportationAllowance, Other, account_id, bank_id, Iban_Number, Bank_account_Number, 
                                             Employee_Recivable_id, Accrued_Salaries_id, Acroal_Leave_Salary_id, Gratuit_id, Petty_Cash_id, 
                                             active, state, project_id) 
                                            VALUES 
                                            (@code, @name, @birth_day, @Social_Status, @city_id, @address, @phone, @Email, 
                                             @Social_Insurance_Number, @EmergencyName, @EmergencyAddress, @EmergencyPhone, @Relation, 
                                             @PassportNumber, @CountryOfIssue, @PassportIssueDate, @PassportExpiryDate, 
                                             @WorkContractNumber, @Position_id, @Department_id, @WorkContractType, @WorkDays, @workinghours, 
                                             @ContractIssueDate, @ContractExpiryDate, @ResidencyFileNumber, @ResidencyIssuingAuthority, 
                                             @ResidencyIssueDate, @ResidencyExpiryDate, @EmiratesIDFileNumber, @EmiratesIDIssuingAuthority, 
                                             @EmiratesIDIssueDate, @EmiratesIDExpiryDate, @BasicSalary, @HousingAllowance, 
                                             @TransportationAllowance, @Other, @account_id, @bank_id, @Iban_Number, @Bank_account_Number, 
                                             @Employee_Recivable_id, @Accrued_Salaries_id, @Acroal_Leave_Salary_id, @Gratuit_id, @Petty_Cash_id, 
                                             @active, @state, @project_id);
                                            SELECT LAST_INSERT_ID();",
                                                DBClass.CreateParameter("code", code),
                                                DBClass.CreateParameter("name", name),
                                                DBClass.CreateParameter("birth_day", "2025-01-01"),
                                                DBClass.CreateParameter("Social_Status", ""),
                                                DBClass.CreateParameter("city_id", cityId),
                                                DBClass.CreateParameter("address", address),
                                                DBClass.CreateParameter("phone", phone),
                                                DBClass.CreateParameter("Email", email),
                                                DBClass.CreateParameter("Social_Insurance_Number", ""),
                                                DBClass.CreateParameter("EmergencyName", ""),
                                                DBClass.CreateParameter("EmergencyAddress", ""),
                                                DBClass.CreateParameter("EmergencyPhone", ""),
                                                DBClass.CreateParameter("Relation", ""),
                                                DBClass.CreateParameter("PassportNumber", ""),
                                                DBClass.CreateParameter("CountryOfIssue", "Albania"),
                                                DBClass.CreateParameter("PassportIssueDate", "2025-01-01"),
                                                DBClass.CreateParameter("PassportExpiryDate", "2025-01-01"),
                                                DBClass.CreateParameter("WorkContractNumber", ""),
                                                DBClass.CreateParameter("Position_id", 0),
                                                DBClass.CreateParameter("Department_id", 0),
                                                DBClass.CreateParameter("WorkContractType", ""),
                                                DBClass.CreateParameter("WorkDays", workDays),
                                                DBClass.CreateParameter("workinghours", workingHours),
                                                DBClass.CreateParameter("ContractIssueDate", "2025-01-01"),
                                                DBClass.CreateParameter("ContractExpiryDate", "2025-03-24"),
                                                DBClass.CreateParameter("ResidencyFileNumber", ""),
                                                DBClass.CreateParameter("ResidencyIssuingAuthority", ""),
                                                DBClass.CreateParameter("ResidencyIssueDate", "2025-03-24"),
                                                DBClass.CreateParameter("ResidencyExpiryDate", "2025-03-24"),
                                                DBClass.CreateParameter("EmiratesIDFileNumber", ""),
                                                DBClass.CreateParameter("EmiratesIDIssuingAuthority", ""),
                                                DBClass.CreateParameter("EmiratesIDIssueDate", "2025-03-24"),
                                                DBClass.CreateParameter("EmiratesIDExpiryDate", "2025-03-24"),
                                                DBClass.CreateParameter("BasicSalary", basicSalary),
                                                DBClass.CreateParameter("HousingAllowance", "0"),
                                                DBClass.CreateParameter("TransportationAllowance", "0"),
                                                DBClass.CreateParameter("Other", "0"),
                                                DBClass.CreateParameter("account_id", 0),
                                                DBClass.CreateParameter("bank_id", 0),
                                                DBClass.CreateParameter("Iban_Number", ""),
                                                DBClass.CreateParameter("Bank_account_Number", ""),
                                                DBClass.CreateParameter("Employee_Recivable_id", _EmployeeRecivableId),
                                                DBClass.CreateParameter("Accrued_Salaries_id", _AccruedSalariesId),
                                                DBClass.CreateParameter("Acroal_Leave_Salary_id", _AcroalLeaveSalaryId),
                                                DBClass.CreateParameter("Gratuit_id", _GratuitIdstring),
                                                DBClass.CreateParameter("Petty_Cash_id", _PettyCashId),
                                                DBClass.CreateParameter("active", 0),
                                                DBClass.CreateParameter("state", 0),
                                                DBClass.CreateParameter("project_id", 0)
                                            ));


                                Utilities.LogAudit(frmLogin.userId, "Add Employee", "Employee", empId, "Added Employee: " + name);
                                formattedCode++;
                            }
                        }
                    }
                }

                loadingForm.Invoke(new Action(() => loadingForm.Close()));
                MessageBox.Show("Successfully saved! Employee List", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Employee");
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
            frmImportOpeningBalanceEmployee importForm = new frmImportOpeningBalanceEmployee();
            if (importForm.ShowDialog() == DialogResult.OK)
            {
                DataTable importedData = importForm.ImportedData;
                dgvCustomer.DataSource = importedData;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
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

public class frmImportOpeningBalanceEmployee : Form
{
    private TextBox txtStartCell, txtEndCell; 
    TextBox txtCodeCell;
    TextBox txtNameCell;
    TextBox txtAddressCell;
    TextBox txtPhoneCell;
    TextBox txtBirthDayCell;
    TextBox txtBasicSalaryCell;
    TextBox txtWorkDaysCell;
    TextBox txtWorkingHoursCell;
    TextBox txtCountryOfIssueCell;
    TextBox txtCityCell;
    private Button btnLoadExcel, btnImport;
    private DataGridView dataGridView;
    private string excelPath = string.Empty;
    DataTable dtFiltered = new DataTable();
    public DataTable ImportedData { get; private set; }

    public frmImportOpeningBalanceEmployee()
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

        // Code Cell
        panelTop.Controls.Add(new Label() { Text = "Code Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtCodeCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Name Cell
        panelTop.Controls.Add(new Label() { Text = "Name Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtNameCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Address Cell
        panelTop.Controls.Add(new Label() { Text = "Address Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtAddressCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Phone Cell
        panelTop.Controls.Add(new Label() { Text = "Phone Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtPhoneCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Birth Day Cell
        panelTop.Controls.Add(new Label() { Text = "Birth Day Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtBirthDayCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Basic Salary Cell
        panelTop.Controls.Add(new Label() { Text = "Basic Salary Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtBasicSalaryCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Work Days Cell
        panelTop.Controls.Add(new Label() { Text = "Work Days Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtWorkDaysCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Working Hours Cell
        panelTop.Controls.Add(new Label() { Text = "Working Hours Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtWorkingHoursCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // Country Of Issue Cell
        panelTop.Controls.Add(new Label() { Text = "Country Of Issue Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtCountryOfIssueCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;

        // City Cell
        panelTop.Controls.Add(new Label() { Text = "City Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtCityCell = new TextBox() { Width = textboxWidth, Location = new Point(x, 12) };
        x += textboxWidth + spacing;


        // Add controls
        panelTop.Controls.Add(btnLoadExcel);
        panelTop.Controls.Add(btnImport);
        panelTop.Controls.Add(txtStartCell);
        panelTop.Controls.Add(txtEndCell);
        panelTop.Controls.Add(txtCodeCell);
        panelTop.Controls.Add(txtNameCell);
        panelTop.Controls.Add(txtAddressCell);
        panelTop.Controls.Add(txtPhoneCell);
        panelTop.Controls.Add(txtBirthDayCell);
        panelTop.Controls.Add(txtBasicSalaryCell);
        panelTop.Controls.Add(txtWorkDaysCell);
        panelTop.Controls.Add(txtWorkingHoursCell);
        panelTop.Controls.Add(txtCountryOfIssueCell);
        panelTop.Controls.Add(txtCityCell);

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

        // Letter-only for others (your actual text fields)
        txtCodeCell.KeyPress += TxtLettersOnly_KeyPress;
        txtNameCell.KeyPress += TxtLettersOnly_KeyPress;
        txtAddressCell.KeyPress += TxtLettersOnly_KeyPress;
        txtPhoneCell.KeyPress += TxtLettersOnly_KeyPress;
        txtBirthDayCell.KeyPress += TxtLettersOnly_KeyPress;
        txtBasicSalaryCell.KeyPress += TxtLettersOnly_KeyPress;
        txtWorkDaysCell.KeyPress += TxtLettersOnly_KeyPress;
        txtWorkingHoursCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCountryOfIssueCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCityCell.KeyPress += TxtLettersOnly_KeyPress;
    }
    private void BtnImport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            MessageBox.Show("Please load an Excel file first.");
            return;
        }

        if (dataGridView.Rows.Count == 0)
        {
            MessageBox.Show("Please load an Excel file first.");
            return;
        }

        int startCol = ToNumber(txtStartCell.Text);
        int endCol = ToNumber(txtEndCell.Text);

        if (startCol == -1 || endCol == -1 || endCol < startCol)
        {
            MessageBox.Show("Invalid start or end column.");
            return;
        }

        // Map Excel columns to employee table fields
        int colCode = ExcelColumnNameToNumber(txtCodeCell.Text);
        int colName = ExcelColumnNameToNumber(txtNameCell.Text);
        int colAddress = ExcelColumnNameToNumber(txtAddressCell.Text);
        int colPhone = ExcelColumnNameToNumber(txtPhoneCell.Text);
        int colBirthDay = ExcelColumnNameToNumber(txtBirthDayCell.Text);
        int colBasicSalary = ExcelColumnNameToNumber(txtBasicSalaryCell.Text);
        int colWorkDays = ExcelColumnNameToNumber(txtWorkDaysCell.Text);
        int colWorkingHours = ExcelColumnNameToNumber(txtWorkingHoursCell.Text);
        int colCountryOfIssue = ExcelColumnNameToNumber(txtCountryOfIssueCell.Text);
        int colCity = ExcelColumnNameToNumber(txtCityCell.Text);

        dtFiltered.Columns.Clear();
        dtFiltered.Columns.Add("code");
        dtFiltered.Columns.Add("name");
        dtFiltered.Columns.Add("address");
        dtFiltered.Columns.Add("phone");
        dtFiltered.Columns.Add("birth_day");
        dtFiltered.Columns.Add("BasicSalary");
        dtFiltered.Columns.Add("WorkDays");
        dtFiltered.Columns.Add("workinghours");
        dtFiltered.Columns.Add("CountryOfIssue");
        dtFiltered.Columns.Add("city");

        using (var package = new ExcelPackage(new FileInfo(excelPath)))
        {
            var ws = package.Workbook.Worksheets[0];

            for (int row = ws.Dimension.Start.Row + 1; row <= ws.Dimension.End.Row; row++)
            {
                if (IsRowEmpty(ws, row, endCol)) continue;

                DataRow dr = dtFiltered.NewRow();

                dr["code"] = (colCode != -1) ? ws.Cells[row, colCode].Text : "";
                dr["name"] = (colName != -1) ? ws.Cells[row, colName].Text : "";
                dr["address"] = (colAddress != -1) ? ws.Cells[row, colAddress].Text : "";
                dr["phone"] = (colPhone != -1) ? ws.Cells[row, colPhone].Text : "";
                dr["birth_day"] = (colBirthDay != -1) ? ws.Cells[row, colBirthDay].Text : "";
                dr["BasicSalary"] = (colBasicSalary != -1) ? ws.Cells[row, colBasicSalary].Text : "0";
                dr["WorkDays"] = (colWorkDays != -1) ? ws.Cells[row, colWorkDays].Text : "0";
                dr["workinghours"] = (colWorkingHours != -1) ? ws.Cells[row, colWorkingHours].Text : "0";
                dr["CountryOfIssue"] = (colCountryOfIssue != -1) ? ws.Cells[row, colCountryOfIssue].Text : "";
                dr["city"] = (colCity != -1) ? ws.Cells[row, colCity].Text : "";

                dtFiltered.Rows.Add(dr);
            }

            dataGridView.DataSource = dtFiltered;
        }

        this.ImportedData = dtFiltered;
        this.DialogResult = DialogResult.OK;
        this.Close();
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