using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewLoan : Form
    {
        int id;
        public frmViewLoan(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = this.Text;
        }

        private void frmViewLoan_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            BindCombos.PopulateEmployeesForLoan(cmbEmployeeName);
            BindCombos.PopulateAllLevel4Account(cmbAccountNameCredit);
            BindCombos.PopulateAllLevel4Account(cmbAccountNameDebit);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertLoan())
                    this.Close();
            }
            else
                  if (updatePetty())
                this.Close();
        }
        private bool updatePetty()
        {
            throw new NotImplementedException();
        }

        private void SetupDataGridView()
        {
            dgvLoan.Columns.Clear();
            dgvLoan.Columns.Add("SN", "SN#");
            dgvLoan.Columns.Add("Date", "Date");
            dgvLoan.Columns.Add("Month", "Month");
            dgvLoan.Columns.Add("Description", "Description");
            dgvLoan.Columns.Add("Amount", "Amount");
            LocalizationManager.LocalizeDataGridViewHeaders(dgvLoan);
        }
        private void FormatNumberWithCommas(Guna.UI2.WinForms.Guna2TextBox txtBox)
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
        private void txtInstallments_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        decimal attempInstall = 1;
        private void BindGrid()
        {
            if (txtInstallments.Text.Trim() == ""||txtRequestAmount.Text.Trim()=="")
            {
                return;
            }
            dtpEndDate.Value= dtpStartDate.Value.AddMonths(int.Parse(txtInstallments.Text)-1);
            dgvLoan.Rows.Clear();
            attempInstall = 1;
            for (DateTime dt = dtpStartDate.Value.Date; dt <= dtpEndDate.Value.Date; dt = dt.AddMonths(1))
            {
                decimal requstedAmount = decimal.Parse(txtRequestAmount.Text) / decimal.Parse(txtInstallments.Text);
                dgvLoan.Rows.Add(attempInstall, dt.ToShortDateString(), dt.ToString("MMMM"), txtDescription.Text, requstedAmount.ToString("N4"));
                attempInstall++;
            }
            }

        private void dtpStartDate_ValueChanged(object sender, EventArgs e)
        {
            //CalculateAndUpdateGrid();
        }

        private void dtpEndDate_ValueChanged(object sender, EventArgs e)
        {
            //CalculateAndUpdateGrid();
        }

        private bool insertLoan()
        {
            int code = 10001;
            string Ccode = "LO" + code;
            if (!chkRequiredDate())
                return false;

            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT code FROM tbl_loan ORDER BY CAST(SUBSTRING(code, 3) AS UNSIGNED) DESC LIMIT 1;"))
                if (reader.Read() && reader["code"].ToString() != "")
                    code = int.Parse(reader["code"].ToString().Replace("LO", "")) + 1;
            Ccode = "LO" + code;

            if (dgvLoan.Rows.Count == 0)
            {
                MessageBox.Show("No data available to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            DateTime loanDate = dtpRequestDate.Value;
            string employeeName = cmbEmployeeName.Text;
            int employeeCode = Convert.ToInt32(txtEmployeeCode.Text);
            decimal requestAmount = Convert.ToDecimal(txtRequestAmount.Text);
            int installments = Convert.ToInt32(txtInstallments.Text);
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            foreach (DataGridViewRow row in dgvLoan.Rows)
            {
                if (!row.IsNewRow)
                {
                    DateTime loanDates = Convert.ToDateTime(row.Cells["Date"].Value);
                    string months = row.Cells["Month"].Value.ToString();
                    string description = row.Cells["Description"].Value.ToString();
                    decimal amount = Convert.ToDecimal(row.Cells["Amount"].Value);

                    id = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_loan (code, LoanDate, EmployeeID, EmployeeName, RequestAmount, Installments,
                                            StartDate, EndDate, loanDates, Months, Description, Amount,debit_account_id,credit_account_id,`change`) 
                                            VALUES (@code, @LoanDate, @EmployeeID, @EmployeeName, @RequestAmount, @Installments, @StartDate, @EndDate,
                                            @loanDates, @Months, @Description, @Amount,@debit_account_id,@credit_account_id,@change); SELECT LAST_INSERT_ID();",
                        DBClass.CreateParameter("code", Ccode),
                        DBClass.CreateParameter("LoanDate", loanDate.ToString("yyyy-MM-dd")),
                        DBClass.CreateParameter("EmployeeID", employeeCode),
                        DBClass.CreateParameter("EmployeeName", employeeName),
                        DBClass.CreateParameter("RequestAmount", requestAmount),
                        DBClass.CreateParameter("Installments", installments),
                        DBClass.CreateParameter("StartDate", startDate.ToString("yyyy-MM-dd")),
                        DBClass.CreateParameter("EndDate", endDate.ToString("yyyy-MM-dd")),
                        DBClass.CreateParameter("loanDates", DateTime.Parse(row.Cells["Date"].Value.ToString()).ToString("yyyy-MM-dd")),
                        DBClass.CreateParameter("Months", row.Cells["Month"].Value.ToString()),
                        DBClass.CreateParameter("Description", row.Cells["Description"].Value.ToString()),
                        DBClass.CreateParameter("Amount", Convert.ToDecimal(row.Cells["Amount"].Value)),
                        DBClass.CreateParameter("debit_account_id", cmbAccountNameDebit.SelectedValue),
                        DBClass.CreateParameter("credit_account_id", cmbAccountNameCredit.SelectedValue),
                        DBClass.CreateParameter("change", requestAmount)
                        ).ToString());
                    DBClass.ExecuteNonQuery(@"
                UPDATE tbl_attendance_salary 
                SET total_loan = IFNULL(total_loan, 0) + @Amount
                WHERE emp_code = @EmployeeID
                  AND MONTH(`date`) = @Month
                  AND YEAR(`date`) = @Year;",
               DBClass.CreateParameter("EmployeeID", employeeCode),
               DBClass.CreateParameter("Amount", amount),
               DBClass.CreateParameter("Month", loanDates.Month),
               DBClass.CreateParameter("Year", loanDates.Year)
           );
                }
            }
            insertJournals();
            Utilities.LogAudit(frmLogin.userId, "Insert Loan", "Loan", id, "Inserted Loan: " + Ccode + " for Employee: " + employeeName);
            MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        private void insertJournals()
        {

            CommonInsert.addTransactionEntry(dtpRequestDate.Value.Date,
                  cmbAccountNameDebit.SelectedValue.ToString(),
                  txtRequestAmount.Text, "0", id.ToString(), cmbEmployeeName.SelectedValue.ToString(), "Loan Request", "Employee LOAN", "Loan Request " + txtCode.Text,
                   frmLogin.userId, DateTime.Now.Date,txtCode.Text);
            CommonInsert.addTransactionEntry(dtpRequestDate.Value.Date,
                                  cmbAccountNameCredit.SelectedValue.ToString(),
                                 "0", txtRequestAmount.Text, id.ToString(),cmbEmployeeName.SelectedValue.ToString(), "Loan Request", "Employee LOAN", "Loan Request " + txtCode.Text,
                   frmLogin.userId, DateTime.Now.Date,txtCode.Text);

        }
        private bool chkRequiredDate()
        {
            if (txtRequestAmount.Text.Trim() == "")
            {
                MessageBox.Show("Enter Request Amount Please.");
                return false;
            }

            return true;
        }
        private void btnSaveClose_Click(object sender, EventArgs e)
        {
                if (insertLoan())
                    resetTextBox();
        }
        private void resetTextBox()
        {

        }

        private void cmbEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmployeeName.SelectedValue == null)
            {
                txtEmployeeCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where id = " + cmbEmployeeName.SelectedValue.ToString()))
                if (reader.Read())
                    txtEmployeeCode.Text = reader["code"].ToString();
                else
                    txtEmployeeCode.Text = "";
        }

        private void cmbAccountNameDebit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccountNameDebit.SelectedValue == null)
            {
                txtAccountCodeDebit.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAccountNameDebit.SelectedValue.ToString()))
                if (reader.Read())
                    txtAccountCodeDebit.Text = reader["code"].ToString();
                else
                    txtAccountCodeDebit.Text = "";
        }

        private void cmbAccountNameCredit_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cmbAccountNameCredit.SelectedValue == null)
            {
                txtAccountCodeCredit.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAccountNameCredit.SelectedValue.ToString()))
                if (reader.Read())
                    txtAccountCodeCredit.Text = reader["code"].ToString();
                else
                    txtAccountCodeCredit.Text = "";
        }

        private void txtAccountCodeDebit_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
            DBClass.CreateParameter("code", txtAccountCodeDebit.Text)))
                if (reader.Read())
                    cmbAccountNameDebit.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtAccountCodeCredit_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
            DBClass.CreateParameter("code", txtAccountCodeCredit.Text)))
                if (reader.Read())
                    cmbAccountNameCredit.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtAccountCodeDebit_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
        DBClass.CreateParameter("code", txtAccountCodeDebit.Text)))
                if (!reader.Read())
                    cmbAccountNameDebit.SelectedIndex = -1;
        }

        private void txtAccountCodeCredit_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                     DBClass.CreateParameter("code", txtAccountCodeCredit.Text)))
                if (!reader.Read())
                    cmbAccountNameCredit.SelectedIndex = -1;
        }

        private void txtRequestAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtInstallments_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}
