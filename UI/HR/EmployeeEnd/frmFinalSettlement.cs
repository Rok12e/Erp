using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmFinalSettlement : Form
    {
       
        int id;
        public frmFinalSettlement(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            this.Text = "Final Settlement";

            headerUC2.FormText = this.Text;
        }

        private void frmFinalSettlement_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
     
        private void frmFinalSettlement_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateEmployees(cmbEmployeeName);
            if (id != 0)
            {
                bindFinalsettle();
                btnSave.Enabled = btnSaveClose.Enabled = UserPermissions.canDelete("Final Settlement");
            }
        }
        private void cmbEmployeeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEmployeeName.SelectedValue == null || cmbEmployeeName.SelectedValue.ToString().ToLower().Contains("rowview"))
            {
                txtEmpCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where id = " + cmbEmployeeName.SelectedValue.ToString()))
                if (reader.Read())
                    txtEmpCode.Text = reader["code"].ToString();
                else
                    txtEmpCode.Text = "";
            if (cmbEmployeeName.SelectedValue == null || cmbEmployeeName.SelectedIndex == -1)
            {
                ClearTextFields();
                return;
            }
            string query = "SELECT BasicSalary, HousingAllowance, TransportationAllowance, Other FROM tbl_employee WHERE id = @id";

            MySqlParameter param = DBClass.CreateParameter("@id", cmbEmployeeName.SelectedValue);

            using (var reader = DBClass.ExecuteReader(query, param))
            {
                if (reader.Read())
                {
                    txtBasicSalary.Text = reader["BasicSalary"]?.ToString() ?? "";
                    txtHousingAllowance.Text = reader["HousingAllowance"]?.ToString() ?? "";
                    txtTransportationAllowance.Text = reader["TransportationAllowance"]?.ToString() ?? "";
                    txtOthers.Text = reader["Other"]?.ToString() ?? "";
                }
                else
                {
                    ClearTextFields();
                }
            }

            string Lreader = "SELECT SUM(credit) - SUM(debit) AS 'Sum' FROM tbl_leave_salary";
            MySqlParameter Lparam = DBClass.CreateParameter("@id", cmbEmployeeName.SelectedValue);

            using (var reader = DBClass.ExecuteReader(Lreader, Lparam))
            {
                if (reader.Read())
                {
                    txtLeaveSalary.Text = reader["Sum"]?.ToString() ?? "";
                }
                else
                {
                    ClearTextFields();
                }
            }

            string Ereader = "SELECT SUM(credit) - SUM(debit) AS 'EOS' FROM tbl_end_of_service";
            MySqlParameter Eparam = DBClass.CreateParameter("@id", cmbEmployeeName.SelectedValue);

            using (var reader = DBClass.ExecuteReader(Ereader, Eparam))
            {
                if (reader.Read())
                {
                    txtEndOfService.Text = reader["EOS"]?.ToString() ?? "";
                }
                else
                {
                    ClearTextFields();
                }
            }

            using (MySqlDataReader read = DBClass.ExecuteReader("SELECT SUM(Amount) AS amount FROM tbl_loan WHERE EmployeeID = @1 AND loanDates >= CURDATE();",
                DBClass.CreateParameter("1", txtEmpCode.Text)))
                if (read.Read())
                    txtEmployeeAdvances.Text = read["amount"].ToString();
            Salarytotal();
            Salaries();
        }

        private void ClearTextFields()
        {
            txtBasicSalary.Text = "";
            txtHousingAllowance.Text = "";
            txtTransportationAllowance.Text = "";
            txtOthers.Text = "";
        }

        private void Salarytotal()
        {
            decimal basicSalary, houseAllowance, transportationAllowance, other, totalSalary;
            decimal.TryParse(txtBasicSalary.Text, out basicSalary);
            decimal.TryParse(txtHousingAllowance.Text, out houseAllowance);
            decimal.TryParse(txtTransportationAllowance.Text, out transportationAllowance);
            decimal.TryParse(txtOthers.Text, out other);
            totalSalary = basicSalary + transportationAllowance + houseAllowance + other;
            txtTotalSalary.Text = totalSalary.ToString();
        }

        private void Salaries()
        {
            decimal totalSalary, calculatedValue1,  otherAdditions, totalAdditions, LeveSalary, EndOfServices;
            decimal.TryParse(txtTotalSalary.Text, out totalSalary);
            decimal.TryParse(txtOtherAdditions.Text, out otherAdditions);
            calculatedValue1 = (totalSalary / 31) * 29;
            //calculatedValue2 = (totalSalary / 30) * 20;
            txtSalaries.Text = calculatedValue1.ToString("N2");
            decimal.TryParse(txtEndOfService.Text, out EndOfServices);
            decimal.TryParse(txtLeaveSalary.Text, out LeveSalary);
            totalAdditions = calculatedValue1 + LeveSalary + EndOfServices + otherAdditions;
            txtTotalAdditions.Text = totalAdditions.ToString("N2");
            CalculateNetAccruals();
        }

        //private void CalculateTotalDeductions()
        //{
        //    decimal employeeAdvances, otherDeductions, payments, totalDeductions;
        //    decimal.TryParse(txtEmployeeAdvances.Text, out employeeAdvances);
        //    decimal.TryParse(txtOtherDeductions.Text, out otherDeductions);
        //    decimal.TryParse(txtPayments.Text, out payments);
        //    totalDeductions = employeeAdvances + otherDeductions + payments;
        //    txtTotalDeductions.Text = totalDeductions.ToString("");
        //    CalculateNetAccruals();
        //}

        private void CalculateNetAccruals()
        {
            decimal totalAdditions, totalDeductions, netAccruals;
            decimal.TryParse(txtTotalAdditions.Text, out totalAdditions);
            decimal.TryParse(txtTotalDeductions.Text, out totalDeductions);
            netAccruals = totalAdditions - totalDeductions;
            txtNetAccruals.Text = Math.Abs(netAccruals).ToString("");

        }


        private void txtTotalDeductions_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmpCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where code =@code",
       DBClass.CreateParameter("code", txtEmpCode.Text)))
                if (reader.Read())
                    cmbEmployeeName.SelectedValue = int.Parse(reader["id"].ToString());

        }

        private void txtEmpCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where code =@code",
        DBClass.CreateParameter("code", txtEmpCode.Text)))
                if (!reader.Read())
                    cmbEmployeeName.SelectedIndex = -1;
        }

        private void txtSalaries_TextChanged(object sender, EventArgs e)
        {
            if (txtSalaries.Text == "")
                txtSalaries.Text = "0";
            if (txtOtherAdditions.Text == "")
                txtOtherAdditions.Text = "0";
            if (txtEndOfService.Text == "")
                txtEndOfService.Text = "0";
            if (txtLeaveSalary.Text == "")
                txtLeaveSalary.Text = "0";
            txtTotalAdditions.Text = (decimal.Parse(txtSalaries.Text) + decimal.Parse(txtLeaveSalary.Text) + decimal.Parse(txtEndOfService.Text) + decimal.Parse(txtOtherAdditions.Text)).ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertEmployeeFinallSet())
                    this.Close();
            }
            else
               if (bindFinalsettle())
                this.Close();
        }

        bool bindFinalsettle()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                                    tf.code,tf.Date,e.code AS 'Employee ID',e.Name as EmployeeName,tf.emp_id,
                                     tf.DateCommencement, 
                                     tf.DateLastWork,
                                      tf.TotalSalary, 
                                    tf.OtherAdditions,
                                      tf.TotalAdditions, 
                                    tf.Payments,
                                    tf.OtherDeductions,
                                      tf.TotalDeductions,
                                       tf.NetAccruals
                                       from tbl_final_settlement tf
                                       INNER JOIN tbl_employee e ON tf.emp_id = e.id where tf.emp_id=@id
                                    ",
                                    DBClass.CreateParameter("id", id)))
                if (reader.Read())
                {
                    dtpDate.Value = Convert.ToDateTime(reader["Date"]);
                    txtCode.Text = reader["code"].ToString();
                    cmbEmployeeName.SelectedValue = Convert.ToInt32(reader["emp_id"]);
                    dtpCommencement.Value = Convert.ToDateTime(reader["DateCommencement"]);
                    dtpLastWorkDay.Value = Convert.ToDateTime(reader["DateLastWork"]);
                    txtTotalSalary.Text = reader["TotalSalary"].ToString();
                    txtOtherAdditions.Text = reader["OtherAdditions"].ToString();
                    txtTotalAdditions.Text = reader["TotalAdditions"].ToString();
                    txtPayments.Text = reader["Payments"].ToString();
                    txtOtherDeductions.Text = reader["OtherDeductions"].ToString();
                    txtTotalDeductions.Text = reader["TotalDeductions"].ToString();
                    txtNetAccruals.Text = reader["NetAccruals"].ToString();
                }
            return true;
        }
        private bool insertEmployeeFinallSet()
        {
            string code = "FS-0001";
            if (cmbEmployeeName.Text.Trim() == "")
            {
                MessageBox.Show("Please Chose Employee First.");
                return false;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT code FROM tbl_final_settlement 
                                                           ORDER BY CAST(SUBSTRING_INDEX(code, '-', -1) AS UNSIGNED) DESC LIMIT 1;"))
                if (reader.Read() && reader["code"].ToString() != "")
                    code = "FS-000" + (int.Parse(reader["code"].ToString().Replace("FS-", "")) + 1);

            int refId = DBClass.ExecuteNonQuery(@"INSERT INTO `tbl_final_settlement`(`code`, `Date`, `emp_id`, `DateCommencement`, `DateLastWork`,
                                `TotalSalary`, `TotalAdditions`, `TotalDeductions`, `NetAccruals`) 
                                VALUES(@code, @Date,@emp_id, @DateCommencement, @DateLastWork, @TotalSalary, @TotalAdditions, @TotalDeductions,
                                    @NetAccruals);",
                    DBClass.CreateParameter("code", code),
                    DBClass.CreateParameter("Date", dtpDate.Value.Date),
                    DBClass.CreateParameter("emp_id", cmbEmployeeName.SelectedValue),
                    DBClass.CreateParameter("DateCommencement", dtpCommencement.Value.Date),
                    DBClass.CreateParameter("DateLastWork", dtpLastWorkDay.Value.Date),
                    DBClass.CreateParameter("TotalSalary", txtTotalSalary.Text),
                    DBClass.CreateParameter("TotalAdditions", txtTotalAdditions.Text),
                    DBClass.CreateParameter("TotalDeductions", txtTotalDeductions.Text),
                    DBClass.CreateParameter("NetAccruals", txtNetAccruals.Text));
            Utilities.LogAudit(frmLogin.userId, "Insert Final Settlement", "Final Settlement", refId, "Inserted Final Settlement for Employee: " + cmbEmployeeName.Text + " with Code: " + code);
            return true;
        }

        private void txtOtherAdditions_TextChanged(object sender, EventArgs e)
        {

            if (txtSalaries.Text == "")
                txtSalaries.Text = "0";
            if (txtEndOfService.Text == "")
                txtEndOfService.Text = "0";
            if (txtLeaveSalary.Text == "")
                txtLeaveSalary.Text = "0";
            if (txtOtherAdditions.Text == "")
                txtOtherAdditions.Text = "0";
            txtTotalAdditions.Text = (decimal.Parse(txtSalaries.Text) + decimal.Parse(txtEndOfService.Text) + decimal.Parse(txtLeaveSalary.Text) + decimal.Parse(txtOtherAdditions.Text)).ToString();
            txtNetAccruals.Text = (decimal.Parse(txtTotalAdditions.Text)).ToString();
        }

        private void txtOtherDeductions_TextChanged(object sender, EventArgs e)
        {
            if (txtEmployeeAdvances.Text == "")
                txtEmployeeAdvances.Text = "0";
            if (txtPayments.Text == "")
                txtPayments.Text = "0";
            if (txtOtherDeductions.Text == "")
                txtOtherDeductions.Text = "0";
            txtTotalDeductions.Text = (decimal.Parse(txtEmployeeAdvances.Text) + decimal.Parse(txtPayments.Text) + decimal.Parse(txtOtherDeductions.Text)).ToString();
            txtNetAccruals.Text = (decimal.Parse(txtTotalAdditions.Text) - decimal.Parse(txtTotalDeductions.Text)).ToString();
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

        private void txtTotalDeductions_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void txtBasicSalary_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTransportationAllowance_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtHousingAllowance_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtOthers_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTotalSalary_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtSalaries_TextChanged_1(object sender, EventArgs e)
        {
        }

        private void txtEndOfService_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtLeaveSalary_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTotalAdditions_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtEmployeeAdvances_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtNetAccruals_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTotalSalary_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtSalaries_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtEndOfService_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtLeaveSalary_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtOtherAdditions_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTotalAdditions_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtEmployeeAdvances_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtPayments_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtOtherDeductions_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTotalDeductions_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtNetAccruals_KeyPress(object sender, KeyPressEventArgs e)
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