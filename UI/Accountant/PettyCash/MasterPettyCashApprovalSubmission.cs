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
    public partial class MasterPettyCashApprovalSubmission : Form
    {
        int id;
        public MasterPettyCashApprovalSubmission(int _id)
        {   
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            headerUC1.FormText = this.Text;
        }

        private void MasterPettyCashApproval_Load(object sender, EventArgs e)
        {
             radioNew.Checked = true;
             BindPettyCash("New");
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
        }


        public void BindPettyCash(string state)
        {
            DataTable dt;
            string query = @"
            SELECT 
            d.id,
            d.date AS `Date`,
            ps.code,
            e.name AS 'Employee Name',
            coa.name AS `Account`,
            d.total AS `Total With VAT`,
            cat.name AS `Category`,
            d.note
            FROM 
            tbl_petty_cash_submition_details d
            INNER JOIN 
            tbl_petty_cash_submition ps ON d.petty_id = ps.id
            INNER JOIN 
            tbl_employee e ON ps.name = e.id
            INNER JOIN tbl_petty_cash_category cat ON d.category = cat.id
            INNER JOIN tbl_coa_level_4 coa ON d.account_id = coa.id
            WHERE d.state ='" + state + "'";

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(DBClass.CreateParameter("state", "New"));

            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvPetty.DataSource = dt;

            dgvPetty.Columns["id"].Visible = false;
            dgvPetty.Columns["Employee Name"].MinimumWidth = 180;
            dgvPetty.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPetty.Columns["Total With VAT"].DefaultCellStyle.Format = "N2";

            for (int i = 0; i < dgvPetty.Columns.Count; i++)
                dgvPetty.Columns[i].ReadOnly = true;
            AddActionButtons();
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

            LocalizationManager.LocalizeDataGridViewHeaders(dgvPetty);
        }
        public void BindPettyCashDeclined()
        {

            DataTable dt;
            string query = @"
                          SELECT 
            d.id,
            d.date AS `Date`,
            ps.code,
            e.name As 'Employee Name',
            coa.name AS `Account`,
            d.total AS `Total With VAT`,
            cat.name AS `Category`,
            d.note
            FROM 
            tbl_petty_cash_submition_details d
            INNER JOIN 
            tbl_petty_cash_submition ps ON d.petty_id = ps.id
            INNER JOIN 
            tbl_employee e ON ps.name = e.id
            INNER JOIN tbl_petty_cash_category cat ON d.category = cat.id
            INNER JOIN tbl_coa_level_4 coa ON d.account_id = coa.id
            WHERE d.state = 'Declined'";

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            parameters.Add(DBClass.CreateParameter("state", "Declined"));

            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvPetty.DataSource = dt;

            dgvPetty.Columns["id"].Visible = false;
            dgvPetty.Columns["Employee Name"].MinimumWidth = 180;
            dgvPetty.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPetty.Columns["Total With VAT"].DefaultCellStyle.Format = "N2";

            for (int i = 0; i < dgvPetty.Columns.Count; i++)
                dgvPetty.Columns[i].ReadOnly = true;

            AddActionButtons(); 

            dgvPetty.Columns["Decline"].Visible = false; 
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

            //    dgvPetty.CellClick += dgvPetty_CellClick;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvPetty);
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
            string pettyId = "";
            string debitAccountId = "";
            string creditAccountId = "";
            string employeeId = "";
            decimal amount = 0;

            try
            {
            string detailQuery = @"
            SELECT d.petty_id, d.account_id, d.total, ps.name AS employee_id
            FROM tbl_petty_cash_submition_details d
            INNER JOIN tbl_petty_cash_submition ps ON d.petty_id = ps.id
            WHERE d.id = @id";

                using (DataTable dt = DBClass.ExecuteDataTable(detailQuery, new MySqlParameter("@id", requestId)))
                {
                    if (dt.Rows.Count > 0)
                    {
                        pettyId = dt.Rows[0]["petty_id"].ToString();
                        debitAccountId = dt.Rows[0]["account_id"].ToString();
                        amount = Convert.ToDecimal(dt.Rows[0]["total"]);
                        employeeId = dt.Rows[0]["employee_id"].ToString();
                    }
                }
                // Get credit account from petty_cash_card
                string creditQuery = @"SELECT 
                COALESCE(
                    (SELECT account_id FROM tbl_petty_cash_card WHERE name = @id LIMIT 1),
                    (SELECT id FROM tbl_coa_level_4 WHERE name = 'Petty Cash' LIMIT 1)
                ) AS account_id;
                            ";

                using (DataTable dt = DBClass.ExecuteDataTable(creditQuery, new MySqlParameter("@id", employeeId)))
                {
                    if (dt.Rows.Count > 0)
                        creditAccountId = dt.Rows[0]["account_id"].ToString();
                }

                // Update the petty cash submission status
                string updateQuery = "UPDATE tbl_petty_cash_submition_details SET state = @state WHERE id = @id";
                DBClass.ExecuteNonQuery(updateQuery,
                    new MySqlParameter("@state", newStatus),
                    new MySqlParameter("@id", requestId));

                if (newStatus == "Approved")
                {
                    DBClass.ExecuteNonQuery(@"
                    UPDATE tbl_petty_cash_request
                    SET pay = pay - @totalPaid
                    WHERE id = @id;",
                    DBClass.CreateParameter("@totalPaid", amount),
                    DBClass.CreateParameter("@id", pettyId)
                    );
                    // Insert journal entries only if approved
                    string refNote = "Petty Cash Approval - REF " + pettyId;

                    CommonInsert.InsertTransactionEntry(DateTime.Now.Date, creditAccountId, amount.ToString(), "0",
                        requestId, employeeId, "Petty Cash Submition Approval", refNote, frmLogin.userId, DateTime.Now.Date);

                    CommonInsert.InsertTransactionEntry(DateTime.Now.Date, debitAccountId, "0", amount.ToString(),
                        requestId, "0", "Petty Cash Submition Approval", refNote, frmLogin.userId, DateTime.Now.Date);
                }
                Utilities.LogAudit(frmLogin.userId, $"Petty Cash Request {newStatus}", "Petty Cash", Convert.ToInt32(requestId),
                    $"{newStatus} Petty Cash Request: {requestId}");
                BindPettyCash("New"); // Refresh grid
                MessageBox.Show($"Petty Cash Request has been {newStatus}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //
            }
        }
        private void radioApproved_CheckedChanged(object sender, EventArgs e)
        {
            string query = @"
                            SELECT 
            d.id,
            d.date AS `Date`,
            ps.code,
            e.name,
            coa.name AS `Account`,
            d.total AS `Total With VAT`,
            cat.name AS `Category`,
            d.note
            FROM 
            tbl_petty_cash_submition_details d
            INNER JOIN 
            tbl_petty_cash_submition ps ON d.petty_id = ps.id
            INNER JOIN 
            tbl_employee e ON ps.name = e.id
            INNER JOIN tbl_petty_cash_category cat ON d.category = cat.id
            INNER JOIN tbl_coa_level_4 coa ON d.account_id = coa.id
            WHERE d.state = 'Approved';";

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

            LocalizationManager.LocalizeDataGridViewHeaders(dgvPetty);
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

    }
}
