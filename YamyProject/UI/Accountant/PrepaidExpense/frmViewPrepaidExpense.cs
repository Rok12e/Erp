using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;
namespace YamyProject
{
    public partial class frmViewPrepaidExpense : Form
    {
        int id;
        decimal prepaidId;

        public frmViewPrepaidExpense(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;

            headerUC1.FormText = id == 0 ? "Prepaid Expense - New" : "Prepaid Expense - Edit";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewPrepaidExpense_Load(object sender, EventArgs e)
        {
            dtpAgrementEndDate.Value = dtpAgrementStartDate.Value = dtPrepaid.Value = DateTime.Now.Date;
            BindCombos.PopulatePrepaidExpenseCategories(cmbCategory);
            BindCombos.PopulateAllLevel4Account(cmbAccountNameCredit);
            BindCombos.PopulateAllLevel4Account(cmbAccountNameDebit);
            cmbAccountNameCredit.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Prepaid Expense Credit Account") ? frmLogin.defaultAccounts["Prepaid Expense Credit Account"] : 0;
            cmbAccountNameDebit.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Prepaid Expense Debit Account") ? frmLogin.defaultAccounts["Prepaid Expense Debit Account"] : 0;

            if (id != 0)
            {
                BindPrepaid();
                btnSave.Enabled = UserPermissions.canEdit("Prepaid Expense");
            }
        }
        private void BindPrepaid()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_prepaid_expense where id = " + id))
                if (reader.Read())
                {
                    txtCode.Text = reader["code"].ToString();
                    txtName.Text = reader["name"].ToString();
                    cmbCategory.SelectedValue = reader["category_id"].ToString();
                    cmbAccountNameCredit.SelectedValue = reader["credit_account_id"].ToString();
                    cmbAccountNameDebit.SelectedValue = reader["debit_account_id"].ToString();
                    dtpAgrementStartDate.Value = DateTime.Parse(reader["start_date"].ToString());
                    dtpAgrementEndDate.Value = DateTime.Parse(reader["end_date"].ToString());
                    txtAgrementAmount.Text = reader["amount"].ToString();
                    txtAgrementFeesAmount.Text = reader["fee"].ToString();
                    txtTotal.Text = reader["total"].ToString();
                }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertPrepaid())
                {
                    EventHub.RefreshPrepaidExpense();
                    this.Close();
                }
            }
            else
            {
                if (updatePrepaid())
                {
                    EventHub.RefreshPrepaidExpense();
                    this.Close();
                }
            }
        }
        private bool updatePrepaid()
        {
            if (!chkRequireData())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_prepaid_expense 
                            SET name = @name,
                                category_id = @category_id,
                                debit_account_id = @debit_account_id,
                                credit_account_id = @credit_account_id,
                                start_date = @start_date,
                                end_date = @end_date,
                                amount = @amount,
                                fee = @fee,
                                total = @total,
                                modified_by = @modified_by,
                                modified_date = @modified_date
                            WHERE id = @id;",
                                         DBClass.CreateParameter("id", id),
                                         DBClass.CreateParameter("name", txtName.Text),
                                         DBClass.CreateParameter("category_id", cmbCategory.SelectedValue.ToString()),
                                         DBClass.CreateParameter("debit_account_id", cmbAccountNameDebit.SelectedValue.ToString()),
                                         DBClass.CreateParameter("credit_account_id", cmbAccountNameCredit.SelectedValue.ToString()),
                                         DBClass.CreateParameter("start_date", dtpAgrementStartDate.Value.Date),
                                         DBClass.CreateParameter("end_date", dtpAgrementEndDate.Value.Date),
                                         DBClass.CreateParameter("amount", txtAgrementAmount.Text),
                                         DBClass.CreateParameter("fee", txtAgrementFeesAmount.Text),
                                         DBClass.CreateParameter("total", txtTotal.Text),
                                         DBClass.CreateParameter("modified_by", frmLogin.userId),
                                         DBClass.CreateParameter("modified_date", DateTime.Now.Date));
            DBClass.ExecuteNonQuery("delete from tbl_transaction where transaction_id = @id and type = 'Prepaid Expense'",
                DBClass.CreateParameter("id", id));
            InsertJournal(id);
            Utilities.LogAudit(frmLogin.userId, "Update Prepaid Expense", "Prepaid Expense", id, "Updated Prepaid Expense: " + txtName.Text);
            return true;
        }
        private string GenerateNextPrepaidCode()
        {
            int code;
            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_prepaid_expense"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 0001;
            }
            return code.ToString("D5");
        }
        private bool insertPrepaid()
        {
            prepaidId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_prepaid_expense(code, name, category_id, debit_account_id, credit_account_id, start_date, end_date,amount,fee,total, created_by, created_date)
                            VALUES(@code, @name, @category_id, @debit_account_id, @credit_account_id, @start_date, @end_date,@amount,@fee,@total, @created_by, @created_date);SELECT LAST_INSERT_ID();",
                              DBClass.CreateParameter("code", GenerateNextPrepaidCode()),
                              DBClass.CreateParameter("name", txtName.Text),
                              DBClass.CreateParameter("category_id", cmbCategory.SelectedValue.ToString()),
                              DBClass.CreateParameter("debit_account_id", cmbAccountNameDebit.SelectedValue.ToString()),
                              DBClass.CreateParameter("credit_account_id", cmbAccountNameCredit.SelectedValue.ToString()),
                              DBClass.CreateParameter("start_date", dtpAgrementStartDate.Value.Date),
                              DBClass.CreateParameter("end_date", dtpAgrementEndDate.Value.Date),
                              DBClass.CreateParameter("amount", txtAgrementAmount.Text),
                              DBClass.CreateParameter("fee", txtAgrementFeesAmount.Text),
                              DBClass.CreateParameter("total", txtTotal.Text),
                              DBClass.CreateParameter("created_by", frmLogin.userId),
                              DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
            InsertJournal((int)prepaidId);
            Utilities.LogAudit(frmLogin.userId, "Create Prepaid Expense", "Prepaid Expense", (int)prepaidId, "Created Prepaid Expense: " + txtName.Text);
            return true;
        }
        private void InsertJournal(int pId)
        {
            DateTime startDate = dtpAgrementStartDate.Value.Date;
            DateTime endDate = dtpAgrementEndDate.Value.Date;
            decimal totalAmount = decimal.Parse(txtTotal.Text);
            int totalDays = (endDate - startDate).Days + 1;

            DateTime currentDate = startDate;

            int lastDayOfFirstMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            DateTime lastDateOfFirstMonth = new DateTime(currentDate.Year, currentDate.Month, lastDayOfFirstMonth);

            if (lastDateOfFirstMonth > endDate) lastDateOfFirstMonth = endDate;

            int daysInFirstMonth = (lastDateOfFirstMonth - startDate).Days + 1;
            decimal firstMonthAmount = Math.Round((totalAmount / totalDays) * daysInFirstMonth, 3);

            CommonInsert.InsertTransactionEntry(lastDateOfFirstMonth,
                cmbAccountNameDebit.SelectedValue?.ToString() ?? "0", firstMonthAmount.ToString(), "0", pId.ToString(), "0", "Prepaid Expense",
                txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);
            CommonInsert.InsertTransactionEntry(lastDateOfFirstMonth,
                cmbAccountNameCredit.SelectedValue.ToString(), "0", firstMonthAmount.ToString(), pId.ToString(), "0", "Prepaid Expense",
                 txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);

            currentDate = lastDateOfFirstMonth.AddDays(1);

            while (currentDate <= endDate)
            {
                int lastDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                DateTime lastDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, lastDay);

                if (lastDateOfMonth > endDate)
                    lastDateOfMonth = endDate;

                int daysInMonth = (lastDateOfMonth - currentDate).Days + 1;
                decimal monthlyAmount = Math.Round((totalAmount / totalDays) * daysInMonth, 2);

                CommonInsert.InsertTransactionEntry(lastDateOfMonth,
                    cmbAccountNameDebit.SelectedValue.ToString(), monthlyAmount.ToString(), "0", pId.ToString(), "0", "Prepaid Expense",
                    txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);
                CommonInsert.InsertTransactionEntry(lastDateOfMonth,
                    cmbAccountNameCredit.SelectedValue.ToString(), "0", monthlyAmount.ToString(), pId.ToString(), "0", "Prepaid Expense",
                     txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);

                currentDate = lastDateOfMonth.AddDays(1);
            }
        }

        //private void InsertJournal(int pId)
        //{

        //    DateTime startDate = dtpAgrementStartDate.Value.Date;
        //    DateTime endDate = dtpAgrementEndDate.Value.Date;
        //    decimal totalAmount = decimal.Parse(txtTotal.Text);
        //    int totalDays = (endDate - startDate).Days + 1;

        //    DateTime currentDate = startDate;

        //    int daysInFirstMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day + 1;
        //    decimal firstMonthAmount = Math.Round((totalAmount / totalDays) * daysInFirstMonth, 3);

        //    CommonInsert.InsertTransactionEntry(dtpAgrementStartDate.Value.Date,
        //cmbAccountNameDebit.SelectedValue?.ToString() ?? "0", firstMonthAmount.ToString(), "0", pId.ToString(), "0", "Prepaid Expense",
        //txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);
        //    CommonInsert.InsertTransactionEntry(dtpAgrementStartDate.Value.Date,
        //cmbAccountNameCredit.SelectedValue.ToString(), "0", firstMonthAmount.ToString(), pId.ToString(), "0", "Prepaid Expense",
        // txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);


        //    currentDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);

        //    while (currentDate <= endDate)
        //    {
        //        int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

        //        if (currentDate.AddDays(daysInMonth - 1) > endDate)
        //            daysInMonth = (endDate - currentDate).Days + 1;

        //        decimal monthlyAmount = Math.Round((totalAmount / totalDays) * daysInMonth, 2);

        //        CommonInsert.InsertTransactionEntry(currentDate,
        //    cmbAccountNameDebit.SelectedValue.ToString(), monthlyAmount.ToString(), "0", pId.ToString(), "0", "Prepaid Expense",
        //    txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);
        //        CommonInsert.InsertTransactionEntry(currentDate,
        //    cmbAccountNameCredit.SelectedValue.ToString(), "0", monthlyAmount.ToString(), pId.ToString(), "0", "Prepaid Expense",
        //     txtName.Text + " - Prepaid Expense No. " + pId, frmLogin.userId, DateTime.Now.Date);
        //                      currentDate = currentDate.AddMonths(1);


        //    }
        //}

        private bool chkRequireData()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Enter Name First.");
                return false;
            }
            if (txtTotal.Text == "" || txtTotal.Text == "0")
            {
                MessageBox.Show("Total Amount Can't Be Null Or Zero");
                return false;
            }
            if (txtTotalDays.Text == "")
            {
                MessageBox.Show("Check Agreement Date First.");
                return false;
            }
            return true;
        }

        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
                if (!insertPrepaid())
                    return;
                else
               if (!updatePrepaid())
                    return;
            txtAgrementAmount.Text = txtAgrementFeesAmount.Text = "";
            dtpAgrementEndDate.Value = dtpAgrementStartDate.Value = dtPrepaid.Value = DateTime.Now.Date;
        }

        private void lnkCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewPrepaidExpenseCategory(0).ShowDialog();
            BindCombos.PopulatePrepaidExpenseCategories(cmbCategory);
        }
        private void LoaddgvItems()
        {
            DataTable dt = DBClass.ExecuteDataTable("SELECT code, name FROM tbl_items WHERE state = 0 AND active = 0");

            // Add the "<< Add >>" option at the top
            DataRow newRow = dt.NewRow();
            newRow["code"] = 0;
            newRow["name"] = "<< Add >>";
            dt.Rows.InsertAt(newRow, 0);

            // Bind to your ComboBox
            cmbAccountNameDebit.DataSource = dt;
            cmbAccountNameDebit.DisplayMember = "name";
            cmbAccountNameDebit.ValueMember = "code";
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

        private void txtAccountCodeCredit_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                     DBClass.CreateParameter("code", txtAccountCodeCredit.Text)))
                if (!reader.Read())
                    cmbAccountNameCredit.SelectedIndex = -1;
        }

        private void txtAccountCodeDebit_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code", DBClass.CreateParameter("code", txtAccountCodeDebit.Text)))
                if (!reader.Read())
                    cmbAccountNameDebit.SelectedIndex = -1;
        }

        private void dtpAgrementStartDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtpAgrementEndDate.Value.Date < dtpAgrementStartDate.Value.Date)
            {
                txtTotalDays.Text = "";
                return;
            }
            txtTotalDays.Text = ((dtpAgrementEndDate.Value.Date - dtpAgrementStartDate.Value.Date).Days + 1).ToString();
        }

        private void txtAgrementAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAgrementAmount.Text == "")
            {
                txtTotal.Text = "";
                return;
            }
            if (txtAgrementFeesAmount.Text == "")
                txtAgrementFeesAmount.Text = "0";
            txtTotal.Text = (decimal.Parse(txtAgrementFeesAmount.Text) + decimal.Parse(txtAgrementAmount.Text)).ToString();
        }

        private void txtAgrementAmount_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtAgrementFeesAmount_KeyPress(object sender, KeyPressEventArgs e)
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
