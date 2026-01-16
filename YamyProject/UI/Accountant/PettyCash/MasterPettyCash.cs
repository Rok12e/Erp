using DocumentFormat.OpenXml.Office2010.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterPettyCash : Form
    {
        private DataView _dataView;
        private EventHandler employeeUpdateHandler;
        public MasterPettyCash()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            employeeUpdateHandler = (sender, args) => BindEmployees();
            EventHub.Employee += employeeUpdateHandler;
        }
        public void BindEmployees()
        {
            
                string query = @"SELECT e.id, e.code, e.name AS 'Employee Name' 
                              FROM tbl_employee e WHERE id  IN (SELECT CAST(name AS UNSIGNED) 
                              FROM tbl_petty_cash_card) GROUP BY e.id, e.code, e.name, e.Department_id, e.email, e.phone;";

                DataTable dt = DBClass.ExecuteDataTable(query);
                _dataView = dt.DefaultView;
                dgvEmployees.DataSource = _dataView;

                dgvEmployees.Columns["id"].Visible = false;
                dgvEmployees.Columns["code"].MinimumWidth = 100;
                dgvEmployees.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            
            LocalizationManager.LocalizeDataGridViewHeaders(dgvEmployees);
        }

        bool EnableApproval = false;

        private void MasterPettyCash_Load(object sender, EventArgs e)
        {
            var barcodeS = Utilities.GeneralSettingsState("ENABLE PETTYCASH APPROVAL");
            if (!string.IsNullOrEmpty(barcodeS) && int.Parse(barcodeS) > 0)
            {
                EnableApproval = true;
            }
            else
            {
                EnableApproval = false;
            }
            if (!EnableApproval)
            {
                toolStripMenuItem30.Visible = toolStripMenuItem12.Visible = false;
            }
            else
            {
                newPettyCashToolStripMenuItem.Visible = editPettyCashToolStripMenuItem.Visible = deletePettyCashToolStripMenuItem.Visible = true;
            }

            BindEmployees();
            dgvInvoice.ColumnHeadersVisible = false;
        }

        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string empCode = dgvEmployees.Rows[e.RowIndex].Cells["code"].Value.ToString();
                BindEmployeePettyCash(empCode);
                BindEmployeePettyCashTransaction(empCode);
            }   
        }

        private void BindEmployeePettyCash(string empCode)
        {
            dgvInvoiceCash.DataSource = null;
            dgvInvoiceCash.ColumnHeadersVisible = true;
            if (!EnableApproval)
            {
                dgvInvoiceCash.DataSource = null;
                dgvInvoiceCash.ColumnHeadersVisible = true;

                dgvInvoiceCash.Rows.Clear();
                string query = @"
                                SELECT 
                                d.id,
                                d.date AS `Date`,
                                ps.code,
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
                                WHERE e.code = @empCode;";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@empCode", empCode));
                dgvInvoiceCash.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    dgvInvoiceCash.ColumnHeadersVisible = false;
                    return;
                }

                dgvInvoiceCash.Columns["Date"].HeaderText = "Submission Date";
                dgvInvoiceCash.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd";

                dgvInvoiceCash.Columns["Account"].HeaderText = "Account";
                dgvInvoiceCash.Columns["Category"].HeaderText = "Category";
                //dgvInvoiceCash.Columns["Cost Center"].HeaderText = "Cost Center";
                //dgvInvoiceCash.Columns["Amount"].DefaultCellStyle.Format = "N2";
                //dgvInvoiceCash.Columns["VAT"].DefaultCellStyle.Format = "N2";
                dgvInvoiceCash.Columns["Total With VAT"].DefaultCellStyle.Format = "N2";
                dgvInvoiceCash.Columns["Total With VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvInvoiceCash.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvInvoiceCash.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dgvInvoiceCash.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
                dgvInvoiceCash.DefaultCellStyle.Font = new Font("Times New Roman", 9);
                dgvInvoiceCash.EnableHeadersVisualStyles = false;
                dgvInvoiceCash.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoiceCash);
            }
            else {
                dgvInvoiceCash.Rows.Clear();
                string query = @"
                            SELECT 
                            p.id,p.code as Code,p.voucher_date as Date,p.total as Total,
                            CASE 
                                WHEN p.`status` = 0 THEN 'Pending' 
                                ELSE 'Confirmed' 
                            END AS Status
                            From tbl_petty_cash p
                            INNER JOIN 
                            tbl_petty_cash_card c ON p.employee_id = c.name
                            INNER JOIN 
                            tbl_employee e ON e.id = p.employee_id
                            WHERE e.code = @empCode;";
                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@empCode", empCode));
                dgvInvoiceCash.DataSource = dt;
                if (dt.Rows.Count == 0)
                {
                    dgvInvoiceCash.ColumnHeadersVisible = false;
                    return;
                }

                dgvInvoiceCash.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd";
                dgvInvoiceCash.Columns["Total"].DefaultCellStyle.Format = "N2";
                dgvInvoiceCash.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInvoiceCash.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvInvoiceCash.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dgvInvoiceCash.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
                dgvInvoiceCash.DefaultCellStyle.Font = new Font("Times New Roman", 9);
                dgvInvoiceCash.EnableHeadersVisualStyles = false;
                dgvInvoiceCash.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoiceCash);
            }
        }

        private void BindEmployeePettyCashTransaction(string empCode)
        {
            dgvInvoice.DataSource = null;
            dgvInvoice.ColumnHeadersVisible = true;
            if (!EnableApproval)
            {
                dgvInvoice.Rows.Clear();
                string query = @"
                            SELECT 
                            d.id,
                            d.date AS `Date`,
                            ps.code,
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
                            WHERE e.code = @empCode;";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@empCode", empCode));
                dgvInvoice.DataSource = dt;

                if (dt.Rows.Count == 0)
                {
                    dgvInvoice.ColumnHeadersVisible = false;
                    return;
                }

                dgvInvoice.Columns["Date"].HeaderText = "Submission Date";
                dgvInvoice.Columns["Date"].DefaultCellStyle.Format = "yyyy-MM-dd";

                dgvInvoice.Columns["Account"].HeaderText = "Account";
                dgvInvoice.Columns["Category"].HeaderText = "Category";
                //dgvInvoice.Columns["Cost Center"].HeaderText = "Cost Center";

                //dgvInvoice.Columns["Amount"].DefaultCellStyle.Format = "N2";
                //dgvInvoice.Columns["VAT"].DefaultCellStyle.Format = "N2";
                dgvInvoice.Columns["Total With VAT"].DefaultCellStyle.Format = "N2";
                dgvInvoiceCash.Columns["Total With VAT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvInvoice.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
                dgvInvoice.DefaultCellStyle.Font = new Font("Times New Roman", 9);
                dgvInvoice.EnableHeadersVisualStyles = false; 
                dgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoice);
            }
            else
            {
                dgvInvoice.Rows.Clear();
                dgvInvoice.Columns.Clear();

                // Define columns
                dgvInvoice.Columns.Add("id", "ID");
                dgvInvoice.Columns["id"].Visible = false;

                dgvInvoice.Columns.Add("SN", "SN#");
                dgvInvoice.Columns.Add("Date", "Date");
                dgvInvoice.Columns.Add("No", "Voucher No");
                dgvInvoice.Columns.Add("Type", "Type");
                dgvInvoice.Columns.Add("InvoiceId", "Inv Id");
                dgvInvoice.Columns.Add("Description", "Description");
                dgvInvoice.Columns.Add("Debit", "Debit");
                dgvInvoice.Columns.Add("Credit", "Credit");
                dgvInvoice.Columns.Add("Balance", "Balance");

                // Format numeric columns
                dgvInvoice.Columns["Debit"].DefaultCellStyle.Format = "N2";
                dgvInvoice.Columns["Debit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInvoice.Columns["Credit"].DefaultCellStyle.Format = "N2";
                dgvInvoice.Columns["Credit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvInvoice.Columns["Balance"].DefaultCellStyle.Format = "N2";
                dgvInvoice.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                // Query
                string query = @"
                                SELECT 
                                    t.id,
                                    t.`Date`,
                                    CAST(t.voucher_no AS CHAR CHARACTER SET utf8mb4) AS `No`,
                                    CAST(t.type AS CHAR CHARACTER SET utf8mb4) AS `Type`,
                                    t.transaction_id AS `Invoice Id`,
                                    CAST(coa.name AS CHAR CHARACTER SET utf8mb4) AS `Description`,
                                    t.debit AS `Debit`,
                                    t.credit AS `Credit`
                                FROM 
                                    tbl_transaction t
                                INNER JOIN 
                                    tbl_coa_level_4 coa ON t.account_id = coa.id
                                WHERE 
                                    t.transaction_id IN (
                                        SELECT pt.id 
                                        FROM tbl_employee e
                                        INNER JOIN tbl_petty_cash pt ON pt.employee_id = e.id
                                        WHERE e.code = @empCode
                                    )
                                    AND t.state = 0
                                    AND t.type = 'Petty Cash'
                                ORDER BY t.`Date`, t.id;
                                ";

                // Fetch data
                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@empCode", empCode));

                // Fill rows with running balance
                decimal balance = 0;
                int sn = 1;

                foreach (DataRow row in dt.Rows)
                {
                    decimal debit = row["Debit"] != DBNull.Value ? Convert.ToDecimal(row["Debit"]) : 0;
                    decimal credit = row["Credit"] != DBNull.Value ? Convert.ToDecimal(row["Credit"]) : 0;
                    balance += credit - debit;

                    dgvInvoice.Rows.Add(
                        row["id"],
                        sn++,
                        DateTime.Parse(row["Date"].ToString()).ToShortDateString(),
                        string.IsNullOrEmpty(row["No"].ToString()) ? ("PC-00" + row["id"]) : row["No"],
                        row["Type"],
                        row["Invoice Id"] != DBNull.Value ? row["Invoice Id"].ToString() : null,
                        row["Description"],
                        debit,
                        credit,
                        balance
                    );
                }

                // Styling
                dgvInvoice.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                dgvInvoice.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                dgvInvoice.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
                dgvInvoice.DefaultCellStyle.Font = new Font("Times New Roman", 9);
                dgvInvoice.EnableHeadersVisualStyles = false;

                dgvInvoice.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                //LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoice);
            }
        }

        private void newTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MasterPettyCashCategory().ShowDialog();
        }
        private void addAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmPettyCashCard().ShowDialog();
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new MasterPettyCashCategory().ShowDialog();
        }

        private void addRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmPettyCashRequest(0).ShowDialog();
        }

        private void approvalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterPettyCashApproval(0));
        }

        private void submitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(0));
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            new frmPettyCashCard().ShowDialog();
        
        }

        private void toolStripMenuItem11_Click(object sender, EventArgs e)
        {
            new frmPettyCashCategory().ShowDialog();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            new frmPettyCashRequest(0).ShowDialog();
        }
        private void dgvInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var cell = dgvInvoice.Rows[e.RowIndex].Cells["InvoiceId"];
                if (cell != null && cell.Value != null)
                {
                    int submissionId = Convert.ToInt32(cell.Value);
                    if (!EnableApproval)
                    {
                        frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(submissionId));
                    }
                    else
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPettyCashVoucher(submissionId));
                    }
                }
                else
                {
                    MessageBox.Show("Invalid or missing ID.");
                }
            }
        }

        private void approvalSubmitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterPettyCashApprovalSubmission(0));
        }

        private void newPettyCashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewPettyCashVoucher(0));
        }

        private void editPettyCashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInvoiceCash.SelectedRows.Count > 0)
            {
                var cell = dgvInvoiceCash.SelectedRows[0].Cells["id"];
                if (cell != null && cell.Value != null)
                {
                    int _invId = Convert.ToInt32(cell.Value);
                    frmLogin.frmMain.openChildForm(new frmViewPettyCashVoucher(_invId));
                }
                else
                {
                    MessageBox.Show("Invalid or missing ID.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row first.");
            }
        }

        private void deletePettyCashToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvInvoice.SelectedRows.Count > 0)
            {
                var cell = dgvInvoice.SelectedRows[0].Cells["id"];
                if (cell != null && cell.Value != null)
                {
                    int id = Convert.ToInt32(cell.Value);
                    if (!EnableApproval)
                    {
                        //DBClass.ExecuteNonQuery("UPDATE tbl_PettyCash_voucher SET state = -1 WHERE id = @id ",
                        //                              DBClass.CreateParameter("id", id.ToString()));
                    }
                    else
                    {
                        // 1. Read all records before deletion
                        DataTable dtVoucher = DBClass.ExecuteDataTable("SELECT * FROM tbl_petty_cash WHERE id = @id",
                        DBClass.CreateParameter("id", id.ToString()));

                        DataTable dtDetails = DBClass.ExecuteDataTable("SELECT * FROM tbl_petty_cash_details WHERE petty_cash_id = @id",
                            DBClass.CreateParameter("id", id.ToString()));

                        DataTable dtTransaction = DBClass.ExecuteDataTable("SELECT * FROM tbl_transaction WHERE transaction_id = @id AND t_type='PettyCash'",
                            DBClass.CreateParameter("id", id.ToString()));

                        // 2. Insert backups into tbl_deleted_records
                        foreach (DataRow row in dtVoucher.Rows)
                        {
                            DBClass.ExecuteNonQuery(
                                "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                                DBClass.CreateParameter("table", "tbl_petty_cash"),
                                DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                                DBClass.CreateParameter("user", frmLogin.userId.ToString())
                            );
                        }

                        foreach (DataRow row in dtDetails.Rows)
                        {
                            DBClass.ExecuteNonQuery(
                                "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                                DBClass.CreateParameter("table", "tbl_petty_cash_details"),
                                DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                                DBClass.CreateParameter("user", frmLogin.userId.ToString())
                            );
                        }

                        foreach (DataRow row in dtTransaction.Rows)
                        {
                            DBClass.ExecuteNonQuery(
                                "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                                DBClass.CreateParameter("table", "tbl_transaction"),
                                DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                                DBClass.CreateParameter("user", frmLogin.userId.ToString())
                            );
                        }

                        // 3. Now delete records permanently
                        CommonInsert.DeleteTransactionEntry(id, "PettyCash");
                        CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "PettyCash");
                        DBClass.ExecuteNonQuery("DELETE FROM tbl_petty_cash_details WHERE petty_cash_id = @id",
                            DBClass.CreateParameter("id", id.ToString()));
                        DBClass.ExecuteNonQuery("DELETE FROM tbl_petty_cash WHERE id = @id",
                            DBClass.CreateParameter("id", id.ToString()));

                        Utilities.LogAudit(frmLogin.userId, "PettyCash Voucher Permanently Deleted", "PettyCash Voucher", id, "Deleted PettyCash Voucher with ID: " + id);
                    }
                    BindEmployees();
                }
            }
        }

        private void dgvEmployees_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                //var cell = dgvEmployees.Rows[e.RowIndex].Cells["id"];
                //if (cell != null && cell.Value != null)
                //{
                //    int eId = Convert.ToInt32(cell.Value);
                    
                //}
                //else
                //{
                //    MessageBox.Show("Invalid or missing ID.");
                //}
            }
        }

        private void deletePCACToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.SelectedRows.Count > 0)
            {
                var cell = dgvEmployees.SelectedRows[0].Cells["id"];
                if (cell != null && cell.Value != null)
                {
                    int eId = Convert.ToInt32(cell.Value);
                    var EmployeeName = dgvEmployees.SelectedRows[0].Cells["Employee Name"].Value.ToString();

                    // 1. Read all records before deletion
                    object getId = DBClass.ExecuteScalar(
                        "SELECT id FROM tbl_petty_cash_card WHERE name = @id",
                        DBClass.CreateParameter("@id", eId));
                    int pId = (getId == DBNull.Value || getId == null) ? 0 : Convert.ToInt32(getId);
                    if (!EnableApproval)
                    {
                        //DBClass.ExecuteNonQuery("UPDATE tbl_PettyCash_voucher SET state = -1 WHERE id = @id ",
                        //                              DBClass.CreateParameter("id", pId.ToString()));
                    }
                    else
                    {
                        var result = MessageBox.Show("Are you sure you want to delete this Petty Cash Card?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id FROM tbl_petty_cash WHERE employee_id In (SELECT c.name from tbl_petty_cash_card c WHERE c.id = @id)", DBClass.CreateParameter("id", pId)))
                                if (reader.Read())
                                {
                                    MessageBox.Show("Cannot delete this Petty Cash Card because it is referenced in Petty Cash Vouchers.", "Deletion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                            DBClass.ExecuteNonQuery("DELETE FROM tbl_petty_cash_card WHERE id = @id", DBClass.CreateParameter("id", pId));
                            Utilities.LogAudit(frmLogin.userId, "Delete Petty Cash Card", "Petty Cash Card", pId, "Deleted Petty Cash Card: " + EmployeeName);
                            EventHub.RefreshEmployee();
                            this.Close();
                        }
                    }
                    BindEmployees();
                }
            }
        }

        private void editPCACToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( dgvEmployees.SelectedRows.Count > 0)
            {
                var cell = dgvEmployees.SelectedRows[0].Cells["id"];
                if (cell != null && cell.Value != null)
                {
                    int eId = Convert.ToInt32(cell.Value);
                    var EmployeeName = dgvEmployees.SelectedRows[0].Cells["Employee Name"].Value.ToString();
                    // 1. Read all records before deletion
                    object getId = DBClass.ExecuteScalar(
                        "SELECT id FROM tbl_petty_cash_card WHERE name = @id",
                        DBClass.CreateParameter("@id", eId));
                    int pId = (getId == DBNull.Value || getId == null) ? 0 : Convert.ToInt32(getId);
                    if (!EnableApproval)
                    {
                        //DBClass.ExecuteNonQuery("UPDATE tbl_PettyCash_voucher SET state = -1 WHERE id = @id ",
                        //                              DBClass.CreateParameter("id", pId.ToString()));
                    }
                    else
                    {
                        new frmPettyCashCard(pId).ShowDialog();
                    }
                }
            }
        }

        private void dgvInvoiceCash_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                var cell = dgvInvoiceCash.Rows[e.RowIndex].Cells["id"];
                if (cell != null && cell.Value != null)
                {
                    int submissionId = Convert.ToInt32(cell.Value);
                    if (!EnableApproval)
                    {
                        frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(submissionId));
                    }
                    else
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPettyCashVoucher(submissionId));
                    }
                }
                else
                {
                    MessageBox.Show("Invalid or missing ID.");
                }
            }
        }
    }
}