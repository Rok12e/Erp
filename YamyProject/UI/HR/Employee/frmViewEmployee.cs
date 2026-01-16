using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewEmployee : Form
    {
        int id;
        EventHandler empDept, empPosition,employee;

        public frmViewEmployee(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            empDept = (sender, args) => BindCombos.PopulateDepartments(cmbDepartments);
            empPosition = (sender, args) => BindCombos.PopulatePositions(cmbPosition, cmbDepartments.SelectedValue.ToString() == "" ? 0 : (int)cmbDepartments.SelectedValue);
            EventHub.EmployeeDept += empDept;
            EventHub.EmployeePosition += empPosition;
            
            this.id = id;
            headerUC1.FormText = id == 0 ? "Employee - New Employee" : "Employee - Edit Employee";
        }
        private void frmViewEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.EmployeeDept -= empDept;
            EventHub.EmployeePosition -= empPosition;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewEmployee_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCountries(cmbCountry);
            BindCombos.PopulateCountries(cmbCountryOfIssue);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            BindCombos.PopulateAllLevel4Account(cmbAccruedSalaries);
            BindCombos.PopulateAllLevel4Account(cmbEmployeeRecivable);
            BindCombos.PopulateAllLevel4Account(cmbAcroalLeaveSalary);
            BindCombos.PopulateAllLevel4Account(cmbPettyCash);
            BindCombos.PopulateAllLevel4Account(cmbGratuit);
            BindCombos.PopulateDepartments(cmbDepartments);
            BindCombos.PopulateRegisterBanks(cmbBankName);

            cmbAccountName.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Employee")
               ? frmLogin.defaultAccounts["Employee"] : 0;
            cmbAccruedSalaries.SelectedValue = GetDefaultAccountId("Accrued Salaries");
            cmbEmployeeRecivable.SelectedValue = GetDefaultAccountId("Employee Receivable");
            cmbAcroalLeaveSalary.SelectedValue = GetDefaultAccountId("Acroal Leave Salary");
            cmbGratuit.SelectedValue = GetDefaultAccountId("Gratuit");
            cmbPettyCash.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Petty Cash")
               ? frmLogin.defaultAccounts["Petty Cash"] : 0;
            if(cmbDepartments.SelectedValue != null)
                BindCombos.PopulatePositions(cmbPosition, cmbDepartments.SelectedValue.ToString() == "" ? 0 : (int)cmbDepartments.SelectedValue);
            if (id != 0)
                BindEmployee();
        }
        private void BindEmployee()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT tbl_employee.* , tbl_city.country_id FROM tbl_employee left join tbl_city on tbl_employee.city_id = tbl_city.id  WHERE tbl_employee.id = " + id))
                if (reader.Read())
                {
                    txtEmployeeCode.Text = reader["code"].ToString();
                    txtEmpName.Text = reader["name"].ToString();
                    cmbCountry.SelectedValue = Convert.ToInt32(reader["country_id"]);
                    cmbCity.SelectedValue = Convert.ToInt32(reader["city_id"]);
                    txtAddress.Text = reader["address"].ToString();

                    txtPhoneNum.Text = reader["phone"].ToString();
                    dtOfBirth.Value = Convert.ToDateTime(reader["Birth_day"]);
                    cmbSocialState.Text = reader["Social_Status"].ToString();
                    txtEmail.Text = reader["Email"].ToString();
                    txtSocialInsurance.Text = reader["Social_Insurance_Number"].ToString();

                    txtEmergency.Text = reader["EmergencyName"].ToString();
                    txtAddressEmerg.Text = reader["EmergencyAddress"].ToString();
                    txtPhoneNumEmerg.Text = reader["EmergencyPhone"].ToString();
                    cmbRelation.Text = reader["relation"].ToString();

                    txtBasicSalary.Text = reader["BasicSalary"].ToString();
                    txtHouseingAllowance.Text = reader["HousingAllowance"].ToString();
                    txtTransportaionAllow.Text = reader["TransportationAllowance"].ToString();
                    txtSalaryOther.Text = reader["Other"].ToString();

                    cmbBankName.SelectedValue = Convert.ToInt32(reader["bank_id"]);
                    txtIBanNum.Text = reader["Iban_Number"].ToString();
                    txtBankAccountNumber.Text = reader["Bank_Account_Number"].ToString();

                    txtEmiratesIdFileNum.Text = reader["EmiratesIDFileNumber"].ToString();
                    txtEmiratesIdIssue.Text = reader["EmiratesIDIssuingAuthority"].ToString();
                    dtEmiratesIdIssue.Value = Convert.ToDateTime(reader["EmiratesIDIssueDate"]);
                    dtEmiratesIdExpire.Value = Convert.ToDateTime(reader["EmiratesIDExpiryDate"]);

                    txtPassportNum.Text = reader["PassportNumber"].ToString();
                    cmbCountryOfIssue.Text = reader["CountryOfIssue"].ToString();
                    dtPassportIssue.Value = Convert.ToDateTime(reader["PassportIssueDate"]);
                    dtPassportEnd.Value = Convert.ToDateTime(reader["PassportExpiryDate"]);

                    txtWorkContractNum.Text = reader["WorkContractNumber"].ToString();
                    txtContractType.Text = reader["WorkContractType"].ToString();
                    cmbPosition.SelectedValue = Convert.ToInt32(reader["Position_id"]);
                    cmbDepartments.SelectedValue = Convert.ToInt32(reader["Department_id"]);
                    txtWorkDays.Text = reader["WorkDays"].ToString();
                    txtWorkingHours.Text = reader["workinghours"].ToString();
                    dtContractIssue.Value = Convert.ToDateTime(reader["ContractIssueDate"]);
                    dtContractExpire.Value = Convert.ToDateTime(reader["ContractExpiryDate"]);

                    txtResidencyFileNum.Text = reader["ResidencyFileNumber"].ToString();
                    txtResidencyIssue.Text = reader["ResidencyIssuingAuthority"].ToString();
                    dtResidencyIsseue.Value = Convert.ToDateTime(reader["ResidencyIssueDate"]);
                    dtResidencyExpire.Value = Convert.ToDateTime(reader["ResidencyExpiryDate"]);

                    cmbAccountName.SelectedValue = Convert.ToInt32(reader["Account_id"]);
                    cmbEmployeeRecivable.SelectedValue = Convert.ToInt32(reader["Employee_Recivable_id"]);
                    cmbAccruedSalaries.SelectedValue = Convert.ToInt32(reader["Accrued_Salaries_id"]);
                    cmbAcroalLeaveSalary.SelectedValue = Convert.ToInt32(reader["Acroal_Leave_Salary_id"]);
                    cmbGratuit.SelectedValue = Convert.ToInt32(reader["Gratuit_id"]);
                    cmbPettyCash.SelectedValue = Convert.ToInt32(reader["Petty_Cash_id"]);
                    chkActive.Checked = !Convert.ToBoolean(reader["active"]);
                }
        }
        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCountry.SelectedValue != null)
                BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertEmployee())
                {
                    EventHub.RefreshEmployee();
                    this.Close();
                }
            }
            else
                if (updateEmployee())
            {
                EventHub.RefreshEmployee();
                this.Close();
            }
        }
        private bool updateEmployee()
        {
            if (!chkRequiredDate())
                return false;
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where  Name = @name",
               DBClass.CreateParameter("name", txtEmpName.Text)))
                if (reader.Read() && id != int.Parse(reader["id"].ToString()))
                {
                    MessageBox.Show("Employee Name Already Exists. Enter Another Name.");
                    return false;
                }
            DBClass.ExecuteNonQuery(@"UPDATE `tbl_employee` SET
                        `code` = @code, `name` = @name, `birth_day` = @birth_day, `Social_Status` = @Social_Status,
                        `city_id` = @city_id, `address` = @address, `phone` = @phone, `Email` = @Email,
                        `Social_Insurance_Number` = @Social_Insurance_Number, `EmergencyName` = @EmergencyName,
                        `EmergencyAddress` = @EmergencyAddress, `EmergencyPhone` = @EmergencyPhone, `Relation` = @Relation,
                        `PassportNumber` = @PassportNumber, `CountryOfIssue` = @CountryOfIssue, `PassportIssueDate` = @PassportIssueDate,
                        `PassportExpiryDate` = @PassportExpiryDate, `WorkContractNumber` = @WorkContractNumber, `Position_id` = @Position_id,
                        `Department_id` = @Department_id, `WorkContractType` = @WorkContractType, `WorkDays` = @WorkDays,
                        `workinghours` = @workinghours, `ContractIssueDate` = @ContractIssueDate, `ContractExpiryDate` = @ContractExpiryDate,
                        `ResidencyFileNumber` = @ResidencyFileNumber, `ResidencyIssuingAuthority` = @ResidencyIssuingAuthority,
                        `ResidencyIssueDate` = @ResidencyIssueDate, `ResidencyExpiryDate` = @ResidencyExpiryDate,
                        `EmiratesIDFileNumber` = @EmiratesIDFileNumber, `EmiratesIDIssuingAuthority` = @EmiratesIDIssuingAuthority,
                        `EmiratesIDIssueDate` = @EmiratesIDIssueDate, `EmiratesIDExpiryDate` = @EmiratesIDExpiryDate,
                        `BasicSalary` = @BasicSalary, `HousingAllowance` = @HousingAllowance, `TransportationAllowance` = @TransportationAllowance,
                        `Other` = @Other, `account_id` = @account_id, `bank_id` = @bank_id, `Iban_Number` = @Iban_Number,
                        `Bank_account_Number` = @Bank_account_Number, `Employee_Recivable_id` = @Employee_Recivable_id,
                        `Accrued_Salaries_id` = @Accrued_Salaries_id, `Acroal_Leave_Salary_id` = @Acroal_Leave_Salary_id,
                        `Gratuit_id` = @Gratuit_id,Petty_Cash_id=@Petty_Cash_id, `active` = @active, `state` = 0 WHERE `id` = @id;",
                DBClass.CreateParameter("id", id),
                DBClass.CreateParameter("code", txtEmployeeCode.Text),
                DBClass.CreateParameter("name", txtEmpName.Text),
                DBClass.CreateParameter("birth_day", dtOfBirth.Value.Date),
                DBClass.CreateParameter("Social_Status", cmbSocialState.Text),
                DBClass.CreateParameter("city_id", cmbCity.SelectedValue ?? 0),
                DBClass.CreateParameter("address", txtAddress.Text),
                DBClass.CreateParameter("phone", txtPhoneNum.Text),
                DBClass.CreateParameter("Email", txtEmail.Text),
                DBClass.CreateParameter("Social_Insurance_Number", txtSocialInsurance.Text),
                DBClass.CreateParameter("EmergencyName", txtEmergency.Text),
                DBClass.CreateParameter("EmergencyAddress", txtAddressEmerg.Text),
                DBClass.CreateParameter("EmergencyPhone", txtPhoneNumEmerg.Text),
                DBClass.CreateParameter("Relation", cmbRelation.Text),
                DBClass.CreateParameter("PassportNumber", txtPassportNum.Text),
                DBClass.CreateParameter("CountryOfIssue", cmbCountryOfIssue.Text),
                DBClass.CreateParameter("PassportIssueDate", dtPassportIssue.Value.Date),
                DBClass.CreateParameter("PassportExpiryDate", dtPassportEnd.Value.Date),
                DBClass.CreateParameter("WorkContractNumber", txtWorkContractNum.Text),
                DBClass.CreateParameter("Position_id", cmbPosition.SelectedValue ?? 0),
                DBClass.CreateParameter("Department_id", cmbDepartments.SelectedValue ?? 0),
                DBClass.CreateParameter("WorkContractType", txtContractType.Text),
                DBClass.CreateParameter("WorkDays", txtWorkDays.Text),
                DBClass.CreateParameter("workinghours", txtWorkingHours.Text),
                DBClass.CreateParameter("ContractIssueDate", dtContractIssue.Value.Date),
                DBClass.CreateParameter("ContractExpiryDate", dtContractExpire.Value.Date),
                DBClass.CreateParameter("ResidencyFileNumber", txtResidencyFileNum.Text),
                DBClass.CreateParameter("ResidencyIssuingAuthority", txtResidencyIssue.Text),
                DBClass.CreateParameter("ResidencyIssueDate", dtResidencyIsseue.Value.Date),
                DBClass.CreateParameter("ResidencyExpiryDate", dtResidencyExpire.Value.Date),
                DBClass.CreateParameter("EmiratesIDFileNumber", txtEmiratesIdFileNum.Text),
                DBClass.CreateParameter("EmiratesIDIssuingAuthority", txtEmiratesIdIssue.Text),
                DBClass.CreateParameter("EmiratesIDIssueDate", dtEmiratesIdIssue.Value.Date),
                DBClass.CreateParameter("EmiratesIDExpiryDate", dtEmiratesIdExpire.Value.Date),
                DBClass.CreateParameter("BasicSalary", txtBasicSalary.Text),
                DBClass.CreateParameter("HousingAllowance", txtHouseingAllowance.Text),
                DBClass.CreateParameter("TransportationAllowance", txtTransportaionAllow.Text),
                DBClass.CreateParameter("Other", txtSalaryOther.Text),
                DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue ?? 0),
                DBClass.CreateParameter("bank_id", cmbBankName.SelectedValue ?? 0),
                DBClass.CreateParameter("Iban_Number", txtIBanNum.Text),
                DBClass.CreateParameter("Bank_account_Number", txtBankAccountNumber.Text),
                DBClass.CreateParameter("Employee_Recivable_id", cmbEmployeeRecivable.SelectedValue ?? 0),
                DBClass.CreateParameter("Accrued_Salaries_id", cmbAccruedSalaries.SelectedValue ?? 0),
                DBClass.CreateParameter("Acroal_Leave_Salary_id", cmbAcroalLeaveSalary.SelectedValue ?? 0),
                DBClass.CreateParameter("Gratuit_id", cmbGratuit.SelectedValue ?? 0),
                DBClass.CreateParameter("Petty_Cash_id", cmbPettyCash.SelectedValue ?? 0),
                DBClass.CreateParameter("active", chkActive.Checked ? 0 : 1));
            Utilities.LogAudit(frmLogin.userId, "Update Employee", "Employee", id,GenerateNextEmployeeCode() + ", Updated Employee: " + txtEmpName.Text + ", Code: " + txtEmployeeCode.Text);
            
            return true;
        }
        private string GenerateNextEmployeeCode()
        {
            int code;
            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_employee"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 30001;
            }
            return code.ToString("D5");
        }
        private bool insertEmployee()
        {
            if (!chkRequiredDate())
                return false;
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where Name = @name",
               DBClass.CreateParameter("name", txtEmpName.Text)))
                if (reader.Read())
                {
                    MessageBox.Show("Employee Name Already Exists. Enter Another Name.");
                    return false;
                }


            int refId = DBClass.ExecuteNonQuery(@"INSERT INTO `tbl_employee`(`code`, `name`, `birth_day`, `Social_Status`, `city_id`, 
                                `address`, `phone`, `Email`, `Social_Insurance_Number`, `EmergencyName`, 
                                `EmergencyAddress`, `EmergencyPhone`, `Relation`, `PassportNumber`, `CountryOfIssue`, 
                                `PassportIssueDate`, `PassportExpiryDate`, `WorkContractNumber`, `Position_id`, 
                                `Department_id`, `WorkContractType`, `WorkDays`, `workinghours`, `ContractIssueDate`, 
                                `ContractExpiryDate`, `ResidencyFileNumber`, `ResidencyIssuingAuthority`, 
                                `ResidencyIssueDate`, `ResidencyExpiryDate`, `EmiratesIDFileNumber`, 
                                `EmiratesIDIssuingAuthority`, `EmiratesIDIssueDate`, `EmiratesIDExpiryDate`, 
                                `BasicSalary`, `HousingAllowance`, `TransportationAllowance`, `Other`, 
                                `account_id`, `bank_id`, `Iban_Number`, `Bank_account_Number`, 
                                `Employee_Recivable_id`, `Accrued_Salaries_id`, `Acroal_Leave_Salary_id`, `Gratuit_id`,Petty_Cash_id,
                                `active`, `state`) 
                        VALUES(@code, @name, @birth_day, @Social_Status, @city_id, @address, @phone, @Email, 
                               @Social_Insurance_Number, @EmergencyName, @EmergencyAddress, @EmergencyPhone, 
                               @Relation, @PassportNumber, @CountryOfIssue, @PassportIssueDate, @PassportExpiryDate, 
                               @WorkContractNumber, @Position_id, @Department_id, @WorkContractType, @WorkDays, 
                               @workinghours, @ContractIssueDate, @ContractExpiryDate, @ResidencyFileNumber, 
                               @ResidencyIssuingAuthority, @ResidencyIssueDate, @ResidencyExpiryDate, 
                               @EmiratesIDFileNumber, @EmiratesIDIssuingAuthority, @EmiratesIDIssueDate, 
                               @EmiratesIDExpiryDate, @BasicSalary, @HousingAllowance, @TransportationAllowance, 
                               @Other, @account_id, @bank_id, @Iban_Number, @Bank_account_Number, 
                               @Employee_Recivable_id, @Accrued_Salaries_id, @Acroal_Leave_Salary_id, @Gratuit_id,@Petty_Cash_id, 
                               @active, 0);",
                   DBClass.CreateParameter("code", GenerateNextEmployeeCode()),
                   DBClass.CreateParameter("name", txtEmpName.Text),
                   DBClass.CreateParameter("birth_day", dtOfBirth.Value.Date),
                   DBClass.CreateParameter("Social_Status", cmbSocialState.Text),
                   DBClass.CreateParameter("city_id", cmbCity.SelectedValue ?? 0),
                   DBClass.CreateParameter("address", txtAddress.Text),
                   DBClass.CreateParameter("phone", txtPhoneNum.Text),
                   DBClass.CreateParameter("Email", txtEmail.Text),
                   DBClass.CreateParameter("Social_Insurance_Number", txtSocialInsurance.Text),
                   DBClass.CreateParameter("EmergencyName", txtEmergency.Text),
                   DBClass.CreateParameter("EmergencyAddress", txtAddressEmerg.Text),
                   DBClass.CreateParameter("EmergencyPhone", txtPhoneNumEmerg.Text),
                   DBClass.CreateParameter("Relation", cmbRelation.Text),
                   DBClass.CreateParameter("PassportNumber", txtPassportNum.Text),
                   DBClass.CreateParameter("CountryOfIssue", cmbCountryOfIssue.Text),
                   DBClass.CreateParameter("PassportIssueDate", dtPassportIssue.Value.Date),
                   DBClass.CreateParameter("PassportExpiryDate", dtPassportEnd.Value.Date),
                   DBClass.CreateParameter("WorkContractNumber", txtWorkContractNum.Text),
                   DBClass.CreateParameter("Position_id", cmbPosition.SelectedValue ?? 0),
                   DBClass.CreateParameter("Department_id", cmbDepartments.SelectedValue ?? 0),
                   DBClass.CreateParameter("WorkContractType", txtContractType.Text),
                   DBClass.CreateParameter("WorkDays", txtWorkDays.Text),
                   DBClass.CreateParameter("workinghours", txtWorkingHours.Text),
                   DBClass.CreateParameter("ContractIssueDate", dtContractIssue.Value.Date),
                   DBClass.CreateParameter("ContractExpiryDate", dtContractExpire.Value.Date),
                   DBClass.CreateParameter("ResidencyFileNumber", txtResidencyFileNum.Text),
                   DBClass.CreateParameter("ResidencyIssuingAuthority", txtResidencyIssue.Text),
                   DBClass.CreateParameter("ResidencyIssueDate", dtResidencyIsseue.Value.Date),
                   DBClass.CreateParameter("ResidencyExpiryDate", dtResidencyExpire.Value.Date),
                   DBClass.CreateParameter("EmiratesIDFileNumber", txtEmiratesIdFileNum.Text),
                   DBClass.CreateParameter("EmiratesIDIssuingAuthority", txtEmiratesIdIssue.Text),
                   DBClass.CreateParameter("EmiratesIDIssueDate", dtEmiratesIdIssue.Value.Date),
                   DBClass.CreateParameter("EmiratesIDExpiryDate", dtEmiratesIdExpire.Value.Date),
                   DBClass.CreateParameter("BasicSalary", txtBasicSalary.Text),
                   DBClass.CreateParameter("HousingAllowance", txtHouseingAllowance.Text),
                   DBClass.CreateParameter("TransportationAllowance", txtTransportaionAllow.Text),
                   DBClass.CreateParameter("Other",txtSalaryOther.Text),
                   DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue ?? 0),
                   DBClass.CreateParameter("bank_id", cmbBankName.SelectedValue ?? 0),
                   DBClass.CreateParameter("Iban_Number", txtIBanNum.Text),
                   DBClass.CreateParameter("Bank_account_Number", txtBankAccountNumber.Text),
                   DBClass.CreateParameter("Employee_Recivable_id", cmbEmployeeRecivable.SelectedValue ?? 0),
                   DBClass.CreateParameter("Accrued_Salaries_id", cmbAccruedSalaries.SelectedValue ?? 0),
                   DBClass.CreateParameter("Acroal_Leave_Salary_id", cmbAcroalLeaveSalary.SelectedValue ?? 0),
                   DBClass.CreateParameter("Gratuit_id", cmbGratuit.SelectedValue ?? 0),
                   DBClass.CreateParameter("Petty_Cash_id", cmbPettyCash.SelectedValue ?? 0),
                   DBClass.CreateParameter("active", chkActive.Checked ? 0 :1));
            Utilities.LogAudit(frmLogin.userId, "Insert Employee", "Employee", refId,
                "Inserted Employee: " + txtEmpName.Text + ", Code: " + txtEmployeeCode.Text);
            return true;
        }
        private bool chkRequiredDate()
        {
            if (txtPhoneNum.Text.Trim() == "")
            {
                MessageBox.Show("Enter Main Phone First.");
                tbC.SelectedIndex = 0;
                txtPhoneNum.Focus();
                return false;
            }

            if (txtEmpName.Text.Trim() == "")
            {
                MessageBox.Show("Enter Employee Name First");
                tbC.SelectedIndex = 0;
                txtEmpName.Focus();
                return false;
            }
            if(cmbCity.SelectedValue==null)
            {
                MessageBox.Show("Choose City First");
                return false;
            }
            if (dtOfBirth.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Date Value Must Be Less Or Equal Today");
                tbC.SelectedIndex = 0;
                return false;
            }
            if (txtBasicSalary.Text.Trim() == "")
            {
                MessageBox.Show("Enter Basic Salary Please.");
                tbC.SelectedIndex = 0;
                txtBasicSalary.Focus();
                return false;
            }
            if (txtWorkingHours.Text.Trim() == "")
            {
                MessageBox.Show("Enter Working Hours Please.");
                tbC.SelectedIndex = 1;
                txtWorkingHours.Focus();
                return false;
            }
            if (txtWorkDays.Text.Trim() == "")
            {
                MessageBox.Show("Enter Work Days Please.");
                tbC.SelectedIndex = 1;
                txtWorkDays.Focus();
                return false;
            }

            //if (cmbAccountName.Text.Trim() == "")
            //{
            //    MessageBox.Show("Enter Account Name First");
            //    tbC.SelectedIndex = 1;
            //    cmbAccountName.Focus();
            //    return false;
            //}
            if (cmbPosition.Text.Trim() == "")
            {
                MessageBox.Show("Enter Position Name First");
                tbC.SelectedIndex = 1;
                cmbPosition.Focus();
                return false;
            }
            if (cmbDepartments.Text.Trim() == "")
            {
                MessageBox.Show("Enter Department Name First");
                tbC.SelectedIndex = 1;
                cmbDepartments.Focus();
                return false;
            }
            //if (cmbAccountName.Text.Trim() == "")
            //{
            //    MessageBox.Show("Enter Account Name First");
            //    tbC.SelectedIndex = 1;
            //    cmbAccountName.Focus();
            //    return false;
            //}
            if (txtAccruedSalaries.Text == "")
                txtAccruedSalaries.Text = "0";
            if (txtHouseingAllowance.Text == "")
                txtHouseingAllowance.Text = "0";
            if (txtTransportaionAllow.Text == "")
                txtTransportaionAllow.Text = "0";
            if (txtSalaryOther.Text == "")
                txtSalaryOther.Text = "0";
            if (txtEmployeeRecivable.Text == "")
                txtEmployeeRecivable.Text = "0";
            if (txtAcroalLeaveSalary.Text == "")
                txtAcroalLeaveSalary.Text = "0";
            if (txtGratuit.Text == "")
                txtGratuit.Text = "0";
            if (txtPettyCash.Text == "")
                txtPettyCash.Text = "0";
            return true;
        }

        private void cmbAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccountName.SelectedValue == null)
            {
                txtAccountCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAccountName.SelectedValue.ToString()))
                if (reader.Read())
                    txtAccountCode.Text = reader["code"].ToString();
                else
                    txtAccountCode.Text = "";
        }

        private void txtBasicSalary_TextChanged(object sender, EventArgs e)
        {
            decimal basicSalary = string.IsNullOrWhiteSpace(txtBasicSalary.Text) ? 0 : Convert.ToDecimal(txtBasicSalary.Text);
            decimal housingAllowance = string.IsNullOrWhiteSpace(txtHouseingAllowance.Text) ? 0 : Convert.ToDecimal(txtHouseingAllowance.Text);
            decimal transportationAllowance = string.IsNullOrWhiteSpace(txtTransportaionAllow.Text) ? 0 : Convert.ToDecimal(txtTransportaionAllow.Text);
            decimal otherSalary = string.IsNullOrWhiteSpace(txtSalaryOther.Text) ? 0 : Convert.ToDecimal(txtSalaryOther.Text);

            decimal totalSalary = basicSalary + housingAllowance + transportationAllowance + otherSalary;
            txtTotalSalary.Text = totalSalary.ToString();
        }

        private void txtBasicSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }

        private void txtAccountCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtAccountCode.Text)))
                if (reader.Read())
                    cmbAccountName.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtAccountCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
            DBClass.CreateParameter("code", txtAccountCode.Text)))
                if (!reader.Read())
                    cmbAccountName.SelectedIndex = -1;
        }

        private void cmbAccruedSalaries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccruedSalaries.SelectedValue == null)
            {
                txtAccruedSalaries.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAccruedSalaries.SelectedValue.ToString()))
                if (reader.Read())
                    txtAccruedSalaries.Text = reader["code"].ToString();
                else
                    txtAccruedSalaries.Text = "";
        }

        private void cmbEmployeeRecivable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmployeeRecivable.SelectedValue == null)
            {
                txtEmployeeRecivable.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbEmployeeRecivable.SelectedValue.ToString()))
                if (reader.Read())
                    txtEmployeeRecivable.Text = reader["code"].ToString();
                else
                    txtEmployeeRecivable.Text = "";
        }

        private void cmbAcroalLeaveSalary_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAcroalLeaveSalary.SelectedValue == null)
            {
                txtAcroalLeaveSalary.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAcroalLeaveSalary.SelectedValue.ToString()))
                if (reader.Read())
                    txtAcroalLeaveSalary.Text = reader["code"].ToString();
                else
                    txtAcroalLeaveSalary.Text = "";
        }

        private void cmbGratuit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbGratuit.SelectedValue == null)
            {
                txtGratuit.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbGratuit.SelectedValue.ToString()))
                if (reader.Read())
                    txtGratuit.Text = reader["code"].ToString();
                else
                    txtGratuit.Text = "";
        }

        private void txtBasicSalary_KeyPress(object sender, EventArgs e)
        {

        }

        private void lnkTax_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmViewDepartments frm = new frmViewDepartments();
            frm.BringToFront();
            frm.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewPosition().ShowDialog();
        }

        private void cmbDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCombos.PopulatePositions(cmbPosition, cmbDepartments.SelectedValue == null ? 0 : (int)cmbDepartments.SelectedValue);
        }

        private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBankName.SelectedValue == null)
            {
                txtBankCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where id = " + cmbBankName.SelectedValue.ToString()))
                if (reader.Read())
                    txtBankCode.Text = reader["code"].ToString();
                else
                    txtBankCode.Text = "";
        }

        private void txtBankCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where code =@code",
                DBClass.CreateParameter("code", txtBankCode.Text)))
                if (reader.Read())
                    cmbBankName.SelectedValue = int.Parse(reader["id"].ToString());

        }

        private void txtBankCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where code =@code",
            DBClass.CreateParameter("code", txtBankCode.Text)))
                if (!reader.Read())
                    cmbBankName.SelectedIndex = -1;
        }

        private void txtAccruedSalaries_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", txtAccruedSalaries.Text)))
                if (reader.Read())
                    cmbAccruedSalaries.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtEmployeeRecivable_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", txtEmployeeRecivable.Text)))
                if (reader.Read())
                    cmbEmployeeRecivable.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtAcroalLeaveSalary_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", txtAcroalLeaveSalary.Text)))
                if (reader.Read())
                    cmbAcroalLeaveSalary.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtGratuit_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", cmbGratuit.Text)))
                if (reader.Read())
                    cmbGratuit.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtGratuit_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
          DBClass.CreateParameter("code", cmbGratuit.Text)))
                if (!reader.Read())
                    cmbGratuit.SelectedIndex = -1;
        }

        private void txtAcroalLeaveSalary_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
          DBClass.CreateParameter("code", txtAcroalLeaveSalary.Text)))
                if (!reader.Read())
                    cmbAcroalLeaveSalary.SelectedIndex = -1;
        }

        private void txtEmployeeRecivable_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
          DBClass.CreateParameter("code", txtEmployeeRecivable.Text)))
                if (!reader.Read())
                    cmbEmployeeRecivable.SelectedIndex = -1;
        }

        private void cmbPettyCash_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPettyCash.SelectedValue == null)
            {
                txtPettyCash.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbPettyCash.SelectedValue.ToString()))
                if (reader.Read())
                    txtPettyCash.Text = reader["code"].ToString();
                else
                    txtPettyCash.Text = "";
        }

        private void txtPettyCash_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBasicSalary_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void txtTransportaionAllow_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtHouseingAllowance_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtSalaryOther_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtAccruedSalaries_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
          DBClass.CreateParameter("code", txtAccruedSalaries.Text)))
                if (!reader.Read())
                    cmbAccruedSalaries.SelectedIndex = -1;
        }
        private void FormatNumberWithCommas(TextBox txtBox)
        {
            if (string.IsNullOrWhiteSpace(txtBox.Text))
                return;

            string rawText = txtBox.Text.Replace(",", "");

            decimal number;
            if (decimal.TryParse(rawText, out number))
            {
                int cursorPosition = txtBox.SelectionStart;
                txtBox.Text = number.ToString("N0");
                txtBox.SelectionStart = txtBox.Text.Length;
            }
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

    }
}
