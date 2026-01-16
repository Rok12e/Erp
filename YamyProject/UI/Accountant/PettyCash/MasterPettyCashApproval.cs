using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterPettyCashApproval : Form
    {
        int id;
        private string _initialState = "Approved";
        public MasterPettyCashApproval(int _id,string initialState = "Approved")
        {   
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            lbheader.Text = this.Text;
        }

        private void MasterPettyCashApproval_Load(object sender, EventArgs e)
        {
            if (_initialState == "Approved")
            {
                radioApproved.Checked = true;
                radioApproved_CheckedChanged(null, null);
            }
            else
            {
                //BindPettyCash(radioNew.Checked ? "New" : "Approved");
                radioNew.Checked = true;
                BindPettyCash("New");
            }
            BindAccounts();
        }
        private void AddActionButtons()
        {
            if (dgvPetty.Columns.Contains("Approve"))
                dgvPetty.Columns.Remove("Approve");
            if (dgvPetty.Columns.Contains("Decline"))
                dgvPetty.Columns.Remove("Decline");

            DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn
            {
                Name = "Approve",
                HeaderText = "Approve",
                Text = "Approve",
                UseColumnTextForButtonValue = true
            };

            DataGridViewButtonColumn declineButton = new DataGridViewButtonColumn
            {
                Name = "Decline",
                HeaderText = "Decline",
                Text = "Decline",
                UseColumnTextForButtonValue = true
            };

            dgvPetty.Columns.Add(approveButton);
            dgvPetty.Columns.Add(declineButton);

            LocalizationManager.LocalizeDataGridViewHeaders(dgvPetty);
        }


        public void BindPettyCash(string state)
        {
            //dgvPetty.Columns["debit_account_name"].Visible = dgvPetty.Columns["credit_account_name"].Visible = dgvPetty.Columns["debit_account_code"].Visible = dgvPetty.Columns["credit_account_code"].Visible = true;

            DataTable dt;
            string query = @"
                                      SELECT 
                                    r.id, 
                                    r.request_date AS 'Request Date', 
                                    r.request_ref AS 'Request REF', 
                                    e.name AS 'Employee Name',
                            e.id as 'EmpId',
                                    r.amount AS 'Amount'
    
                                FROM tbl_petty_cash_request r
                               INNER JOIN tbl_petty_cash_card c ON c.name = r.petty_cash_name   
                                INNER JOIN tbl_employee e ON c.name = e.id  
                                WHERE r.state = '" + state + "'";

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(DBClass.CreateParameter("state", "New"));

            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvPetty.DataSource = dt;

            dgvPetty.Columns["EmpId"].Visible = dgvPetty.Columns["id"].Visible = false;
            dgvPetty.Columns["Employee Name"].MinimumWidth = 180;
            dgvPetty.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            for (int i = 0; i < dgvPetty.Columns.Count; i++)
                dgvPetty.Columns[i].ReadOnly = true;
            AddActionButtons();

            //if (dgvPetty.Columns["Approve"] == null)
            //{
            //    DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn();
            //    approveButton.Name = "Approve";
            //    approveButton.HeaderText = "Approve";
            //    approveButton.Text = "Approve";
            //    approveButton.UseColumnTextForButtonValue = true;
            //    dgvPetty.Columns.Add(approveButton);
            //}

            //if (dgvPetty.Columns["Decline"] == null)
            //{
            //    DataGridViewButtonColumn declineButton = new DataGridViewButtonColumn();
            //    declineButton.Name = "Decline";
            //    declineButton.HeaderText = "Decline";
            //    declineButton.Text = "Decline";
            //    declineButton.UseColumnTextForButtonValue = true;
            //    dgvPetty.Columns.Add(declineButton);
            //}

            dgvPetty.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvPetty.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvPetty.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvPetty.EnableHeadersVisualStyles = false;

            dgvPetty.RowsDefaultCellStyle.BackColor = Color.White;
            dgvPetty.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            dgvPetty.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d5dbdb");
            dgvPetty.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvPetty.BorderStyle = BorderStyle.None;
            dgvPetty.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPetty.RowHeadersVisible = false;
            dgvPetty.AllowUserToAddRows = false;
            dgvPetty.AllowUserToResizeRows = false;

            //dgvPetty.CellClick += dgvPetty_CellClick;
        }
        public void BindPettyCashDeclined()
        {
            //dgvPetty.Columns["debit_account_name"].Visible =
            //    dgvPetty.Columns["credit_account_name"].Visible =
            //    dgvPetty.Columns["debit_account_code"].Visible =
            //    dgvPetty.Columns["credit_account_code"].Visible = true;

            DataTable dt;
            string query = @"
                          SELECT 
                            r.id, 
                            r.request_date AS 'Request Date', 
                            r.request_ref AS 'Request REF', 
                            e.name AS 'Employee Name',
                            e.id as 'EmpId',
                            r.amount AS 'Amount'
                        FROM tbl_petty_cash_request r
                        INNER JOIN tbl_petty_cash_card c ON c.name = r.petty_cash_name   
                        INNER JOIN tbl_employee e ON c.name = e.id  
                        WHERE r.state = 'Declined'";

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(DBClass.CreateParameter("state", "Declined"));

            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvPetty.DataSource = dt;

            dgvPetty.Columns["EmpId"].Visible = dgvPetty.Columns["id"].Visible = false;

            //for (int i = 4; i < dgvPetty.Columns.Count - 1; i++)
            //{
            //    dgvPetty.Columns[i].ReadOnly = true;
            //}
            dgvPetty.Columns["Employee Name"].MinimumWidth = 180;
            dgvPetty.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            for (int i = 0; i < dgvPetty.Columns.Count; i++)
                dgvPetty.Columns[i].ReadOnly = true;

            AddActionButtons(); // ✅ Add approve/decline buttons safely

            dgvPetty.Columns["Decline"].Visible = false; // Optional: hide decline button if not needed for declined entries
            //DataGridViewButtonColumn approveButton1 = new DataGridViewButtonColumn();
            //approveButton1.Name = "Approve";
            //approveButton1.HeaderText = "Approve";
            //approveButton1.Text = "Approve";
            //approveButton1.UseColumnTextForButtonValue = true;
            //dgvPetty.Columns.Add(approveButton1);

            //DataGridViewButtonColumn declineButton = new DataGridViewButtonColumn();
            //declineButton.Name = "Decline";
            //declineButton.HeaderText = "Decline";
            //declineButton.Text = "Decline";
            //declineButton.UseColumnTextForButtonValue = true;
            //dgvPetty.Columns.Add(declineButton);

            //dgvPetty.Columns["Employee Name"].MinimumWidth = 180;
            //dgvPetty.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //if (dgvPetty.Columns["Decline"] != null)
            //{
            //    dgvPetty.Columns["Decline"].Visible = false; ;
            //}

            //if (dgvPetty.Columns["Approve"] == null)
            //{
            //    DataGridViewButtonColumn approveButton = new DataGridViewButtonColumn();
            //    approveButton.Name = "Approve";
            //    approveButton.HeaderText = "Approve";
            //    approveButton.Text = "Approve";
            //    approveButton.UseColumnTextForButtonValue = true;
            //    dgvPetty.Columns.Add(approveButton);
            //}

            dgvPetty.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvPetty.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvPetty.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvPetty.EnableHeadersVisualStyles = false;

            dgvPetty.RowsDefaultCellStyle.BackColor = Color.White;
            dgvPetty.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            dgvPetty.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d5dbdb");
            dgvPetty.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvPetty.BorderStyle = BorderStyle.None;
            dgvPetty.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPetty.RowHeadersVisible = false;
            dgvPetty.AllowUserToAddRows = false;
            dgvPetty.AllowUserToResizeRows = false;

            //dgvPetty.CellClick += dgvPetty_CellClick;
        }
        private void BindAccounts()
        {
            string query = "SELECT CONCAT(CODE , ' - ' , NAME) AS name, id FROM tbl_coa_level_4";
            DataTable dt = DBClass.ExecuteDataTable(query);

            // List of ComboBox column names to update
            string[] comboColumns = { "debit_account_name", "credit_account_name" };

            foreach (string columnName in comboColumns)
            {
                DataGridViewComboBoxColumn cmbColumn = dgvPetty.Columns[columnName] as DataGridViewComboBoxColumn;

                if (cmbColumn != null)
                {
                    cmbColumn.DataSource = dt.Copy();
                    cmbColumn.DisplayMember = "name";
                    cmbColumn.ValueMember = "id";
                }
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvPetty);
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvPetty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPetty.Rows.Count <= 0)
                return;
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewRow row = dgvPetty.Rows[e.RowIndex];
                var cell = row.Cells["id"];

                if (cell != null && cell.Value != null)
                {
                    string requestId = cell.Value.ToString();
                    string columnName = dgvPetty.Columns[e.ColumnIndex].Name;

                    if (columnName == "Approve")
                    {
                        UpdatePettyCashStatus(requestId, "Approved", e.RowIndex);
                    }
                    else if (columnName == "Decline")
                    {
                        UpdatePettyCashStatus(requestId, "Declined", e.RowIndex);
                    }
                }
            }
        }
        private void UpdatePettyCashStatus(string requestId, string newStatus, int rowIndex)
        {
            int debitAccountId = 0;// dgvPetty.Rows[rowIndex].Cells["debit_account_name"].Value;
            int creditAccountId = 0;// dgvPetty.Rows[rowIndex].Cells["credit_account_name"].Value;
            object amountObj = dgvPetty.Rows[rowIndex].Cells["Amount"].Value;
            object requestRef = dgvPetty.Rows[rowIndex].Cells["Request REF"].Value;
            object employeeName = dgvPetty.Rows[rowIndex].Cells["Employee NAME"].Value;
            int empId = int.Parse(dgvPetty.Rows[rowIndex].Cells["EmpId"].Value.ToString());
            creditAccountId = empId;
            string empcode = "";
            MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT id,code,(select code from tbl_employee where id = Name) empCode, mobile, whatsapp_no, email,account_id FROM tbl_petty_cash_card WHERE NAME=@id", DBClass.CreateParameter("id", empId));
            if (reader.Read())
            {
                debitAccountId = int.Parse(reader["account_id"].ToString());
                empcode = reader["empCode"].ToString();
            }
            if (amountObj == null || requestRef == null || employeeName == null)
            {
                MessageBox.Show("Missing required fields for transaction!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal amount = Convert.ToDecimal(amountObj);
            string formattedRef = requestRef.ToString();
            string vendorId = employeeName.ToString();
            if (newStatus == "Approved")
            {
                string query = "UPDATE tbl_petty_cash_request " +
                               "SET state = @state, " +
                               "debit_account_id = @debit_account_id, " +
                               "credit_account_id = @credit_account_id, " +
                               "approved_date = @approved_date " +
                               "WHERE id = @id";

                List<MySqlParameter> parameters = new List<MySqlParameter>
                {
                    DBClass.CreateParameter("state", newStatus),
                    DBClass.CreateParameter("debit_account_id", debitAccountId),
                    DBClass.CreateParameter("credit_account_id", creditAccountId),
                    DBClass.CreateParameter("approved_date", DateTime.Now.Date),
                    DBClass.CreateParameter("id", requestId)
                };

                DBClass.ExecuteNonQuery(query, parameters.ToArray());

                //CommonInsert.InsertTransactionEntry(DateTime.Now.Date, creditAccountId.ToString(), amount.ToString(), "0",
                //    requestId, dgvPetty.Rows[rowIndex].Cells["empid"].Value.ToString(), "Petty Cash Request", "Petty Cash Approval - REF- " + formattedRef, frmLogin.userId, DateTime.Now.Date);

                //CommonInsert.InsertTransactionEntry(DateTime.Now.Date, debitAccountId.ToString(), "0", amount.ToString(),
                //    requestId, "0", "Petty Cash Request", "Petty Cash Approval - REF - " + formattedRef, frmLogin.userId, DateTime.Now.Date);

                // generatePaymentVoucher(debitAccountId.ToString(), creditAccountId.ToString(), amount.ToString(), requestId, empcode, employeeName.ToString());
                Utilities.LogAudit(frmLogin.userId, "Approve Petty Cash Request", "Petty Cash Request", int.Parse(requestId), $"Approved Petty Cash Request: {formattedRef} for Employee: {employeeName}");
            }
            else
            {
                string query = "UPDATE tbl_petty_cash_request SET state = @state WHERE id = @id";

                List<MySqlParameter> parameters = new List<MySqlParameter>
                    {
                        DBClass.CreateParameter("state", newStatus),
                        DBClass.CreateParameter("id", requestId)
                    };

                DBClass.ExecuteNonQuery(query, parameters.ToArray());
                Utilities.LogAudit(frmLogin.userId, "Decline Petty Cash Request", "Petty Cash Request", int.Parse(requestId), $"Declined Petty Cash Request: {formattedRef} for Employee: {employeeName}");
            }

            BindPettyCash(radioNew.Checked ? "New" : "Approved");

            MessageBox.Show($"Request {requestId} has been {newStatus}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void ShowPaymentConfirmationDialog(int _id)
        {
            DialogResult result = MessageBox.Show(
                "Do you want to make a payment for this?",
                "Payment Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(0, _id));
            }
            else
            {
                MessageBox.Show("Payment not processed.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void radioApproved_CheckedChanged(object sender, EventArgs e)
        {

            // dgvPetty.Columns["debit_account_name"].Visible = dgvPetty.Columns["credit_account_name"].Visible = dgvPetty.Columns["debit_account_code"].Visible = dgvPetty.Columns["credit_account_code"].Visible = false;
            string query = @"
                            SELECT 
                                r.id, 
                                r.request_date AS 'Request Date', 
                                r.request_ref AS 'Request REF', 
                                e.name AS 'Employee NAME',
                                e.id as 'EmpId',
                                r.amount AS 'Amount'
                            FROM tbl_petty_cash_request r
                            INNER JOIN tbl_petty_cash_card c ON c.name = r.petty_cash_name  
                            INNER JOIN tbl_employee e ON c.name = e.id  
                            WHERE r.state = 'Approved';";

            DataTable dt = DBClass.ExecuteDataTable(query);

            if (dt == null || dt.Rows.Count == 0)
            {
                //MessageBox.Show("No approved petty cash requests found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvPetty.DataSource = dt;

            if (dgvPetty.Columns.Contains("id"))
                dgvPetty.Columns["id"].Visible = false;
            if (dgvPetty.Columns.Contains("EmpId"))
                dgvPetty.Columns["EmpId"].Visible = false;

            if (dgvPetty.Columns.Contains("Request Date"))
            {
                dgvPetty.Columns["Request Date"].MinimumWidth = 100;
                dgvPetty.Columns["Request Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dgvPetty.Columns.Contains("Request REF"))
            {
                dgvPetty.Columns["Request REF"].MinimumWidth = 100;
                dgvPetty.Columns["Request REF"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            if (dgvPetty.Columns.Contains("Employee NAME"))
            {
                dgvPetty.Columns["Employee NAME"].MinimumWidth = 180;
                dgvPetty.Columns["Employee NAME"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvPetty.Columns.Contains("Approved By"))
            {
                dgvPetty.Columns["Approved By"].MinimumWidth = 150;
                dgvPetty.Columns["Approved By"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            if (dgvPetty.Columns.Contains("Amount"))
            {
                dgvPetty.Columns["Amount"].DefaultCellStyle.Format = "N2";
            }

            dgvPetty.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvPetty.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvPetty.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvPetty.EnableHeadersVisualStyles = false;

            dgvPetty.RowsDefaultCellStyle.BackColor = Color.White;
            dgvPetty.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            dgvPetty.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d5dbdb");
            dgvPetty.DefaultCellStyle.SelectionForeColor = Color.Black;

            dgvPetty.BorderStyle = BorderStyle.None;
            dgvPetty.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgvPetty.RowHeadersVisible = false;
            dgvPetty.AllowUserToAddRows = false;
            dgvPetty.AllowUserToResizeRows = false;

            foreach (string colName in new[] { "Approve", "Decline" })
            {
                if (dgvPetty.Columns.Contains(colName))
                    dgvPetty.Columns.Remove(colName);
            }
        }
        private void dgvPetty_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvPetty.CurrentCell == null) return;

            int colIndex = dgvPetty.CurrentCell.ColumnIndex;
            string colName = dgvPetty.Columns[colIndex].Name;

            // Check if it's a TextBox column (for account code input)
            if (colName == "debit_account_code" || colName == "credit_account_code")
            {
                TextBox textBoxControl = e.Control as TextBox;
                if (textBoxControl != null)
                {
                    textBoxControl.TextChanged -= TextBox_TextChanged;
                    textBoxControl.TextChanged += TextBox_TextChanged;
                }
            }

            if (colName == "debit_account_name" || colName == "credit_account_name")
            {
                ComboBox comboBoxControl = e.Control as ComboBox;
                if (comboBoxControl != null)
                {
                    comboBoxControl.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                    comboBoxControl.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
                }
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (dgvPetty.CurrentCell == null || dgvPetty.CurrentRow == null) return;

            string codeColumn = dgvPetty.Columns[dgvPetty.CurrentCell.ColumnIndex].Name;
            string nameColumn = codeColumn == "debit_account_code" ? "debit_account_name" : "credit_account_name";

            TextBox textBoxControl = sender as TextBox;
            if (textBoxControl == null) return;

            string enteredCode = textBoxControl.Text.Trim();

            if (!string.IsNullOrEmpty(enteredCode))
            {
                string query = "SELECT id FROM tbl_coa_level_4 WHERE code = @code";
                object result = DBClass.ExecuteScalar(query, DBClass.CreateParameter("code", enteredCode));

                if (result != null)
                {
                    dgvPetty.CurrentRow.Cells[nameColumn].Value = result;
                }
                else
                {
                    dgvPetty.CurrentRow.Cells[nameColumn].Value = DBNull.Value;
                }
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dgvPetty.CurrentCell == null || dgvPetty.CurrentRow == null) return;

            string nameColumn = dgvPetty.Columns[dgvPetty.CurrentCell.ColumnIndex].Name;
            string codeColumn = nameColumn == "debit_account_name" ? "debit_account_code" : "credit_account_code";

            ComboBox comboBoxControl = sender as ComboBox;
            if (comboBoxControl == null) return;

            if (comboBoxControl.SelectedValue == null)
            {
                return;
            }

            int selectedId;
            if (!int.TryParse(comboBoxControl.SelectedValue.ToString(), out selectedId))
            {
                return;
            }

            string query = "SELECT code FROM tbl_coa_level_4 WHERE id = @id";
            object result = DBClass.ExecuteScalar(query, DBClass.CreateParameter("id", selectedId));

            if (result != null)
            {
                dgvPetty.CurrentRow.Cells[codeColumn].Value = result.ToString();
            }
        }

        private void radioNew_CheckedChanged(object sender, EventArgs e)
        {
            BindPettyCash(radioNew.Checked ? "New" : "Approved");
        }

        private void radioDeclined_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDeclined.Checked)
            {
                BindPettyCashDeclined();
            }
        }

        private void generatePaymentVoucher(string drId, string crId, string amount, string requestId, string empcode, string empName)
        {
            //make auto payment
            string newCode = "PV-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)) AS lastCode FROM tbl_payment_voucher"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "PV-" + code.ToString("D4");
                }
            }
            //insert payment information
            var dated = DateTime.Now.Date;
            int _paymentId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_payment_voucher(date, code, type, method, amount,  
                                                            debit_account_id, debit_cost_center_id, description,credit_account_id,
                                                            credit_cost_center_id,bank_id, bank_account_id, book_no, check_name, check_no,
                                                            check_date, trans_date, trans_name, trans_ref, created_by, created_date, state) 
                                                            VALUES (@date, @code, @type, @method, @amount, @debit_account_id, @debit_cost_center_id,
                                                            @description,@credit_account_id, @credit_cost_center_id, @bank_id, @bank_account_id, 
                                                            @book_no, @check_name, @check_no,@check_date, @trans_date, @trans_name, @trans_ref, 
                                                            @created_by, @created_date, 0); SELECT LAST_INSERT_ID();",
                                                            DBClass.CreateParameter("date", dated),
                                                            DBClass.CreateParameter("code", newCode),
                                                            DBClass.CreateParameter("type", "Employee"),
                                                            DBClass.CreateParameter("method", "Cash"),
                                                            DBClass.CreateParameter("amount", amount),
                                                            DBClass.CreateParameter("debit_account_id", drId),
                                                            DBClass.CreateParameter("debit_cost_center_id", "0"),
                                                            DBClass.CreateParameter("description", ""),
                                                            DBClass.CreateParameter("credit_account_id", crId),
                                                            DBClass.CreateParameter("credit_cost_center_id", "0"),
                                                            DBClass.CreateParameter("bank_id", "0"),
                                                            DBClass.CreateParameter("bank_account_id", "0"),
                                                            DBClass.CreateParameter("book_no", "0"),
                                                            DBClass.CreateParameter("check_name", ""),
                                                            DBClass.CreateParameter("check_no", ""),
                                                            DBClass.CreateParameter("check_date", null),
                                                            DBClass.CreateParameter("trans_date", null),
                                                            DBClass.CreateParameter("trans_name", ""),
                                                            DBClass.CreateParameter("trans_ref", ""),
                                                            DBClass.CreateParameter("created_by", frmLogin.userId),
                                                            DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());

            //insertJournals()
            CommonInsert.InsertTransactionEntry(dated,
                    drId,
                    amount, "0", _paymentId.ToString(), "0", "Employee Payment", "Payment Voucher NO. " + newCode,
                     frmLogin.userId, DateTime.Now.Date);

            CommonInsert.InsertTransactionEntry(dated,
                                  crId,
                               "0", amount, _paymentId.ToString(), "0", "Employee Payment", "Payment Voucher NO. " + newCode,
                                   frmLogin.userId, DateTime.Now.Date);

            var invId = requestId;
            var hum = "0";
            var total = amount;
            string payment = amount;
            var description = "";
            DateTime date = DateTime.Now.Date;

            DBClass.ExecuteNonQuery(@"INSERT INTO tbl_payment_voucher_details(date,payment_id,hum_id, inv_id,inv_code,total, payment, description) 
                                        VALUES (@date,@payment_id,@hum_id ,@inv_id,@inv_code,@total, @payment, @description);",
                                    DBClass.CreateParameter("@payment_id", _paymentId),
                                    DBClass.CreateParameter("@date", date),
                                    DBClass.CreateParameter("@hum_id", hum),
                                    DBClass.CreateParameter("@total", total),
                                    DBClass.CreateParameter("@inv_code", empcode + " " + empName),//dgvInv.Rows[i].Cells["InvNo"].Value.ToString()),
                                    DBClass.CreateParameter("@inv_id", int.Parse(invId)),
                                    DBClass.CreateParameter("@payment", payment),
                                    DBClass.CreateParameter("@description", description));

            DBClass.ExecuteNonQuery(" UPDATE tbl_attendance_salary SET tbl_attendance_salary.pay = tbl_attendance_salary.pay+@pay , tbl_attendance_salary.change = tbl_attendance_salary.change-@change where id = @id",
            DBClass.CreateParameter("pay", decimal.Parse(amount)),
            DBClass.CreateParameter("change", decimal.Parse(amount)),
            DBClass.CreateParameter("id", int.Parse(requestId)));

            //Petty Cash Pay
            string req = "Petty Cash Request";
            if (req == "Petty Cash Request")
            {
                DBClass.ExecuteNonQuery(" UPDATE tbl_petty_cash_request SET tbl_petty_cash_request.pay = tbl_petty_cash_request.pay+@pay , tbl_petty_cash_request.change = tbl_petty_cash_request.change-@change where id = @id",
                DBClass.CreateParameter("pay", decimal.Parse(amount)),
                DBClass.CreateParameter("change", decimal.Parse(amount)),
                DBClass.CreateParameter("id", int.Parse(requestId)));
            }

            Utilities.LogAudit(frmLogin.userId, "Payment Voucher", "Petty Cash", _paymentId, "Created Payment Voucher: " + newCode + " for Request ID: " + requestId);
        }
        private void dgvPetty_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = dgvPetty.Rows[e.RowIndex].Cells["id"];
                int _id;
                if (cell != null && cell.Value != null && int.TryParse(cell.Value.ToString(), out _id))
                {
                    new frmPettyCashRequest(_id).ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid or missing Request ID.");
                }
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
