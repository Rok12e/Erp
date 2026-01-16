using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterLoan : Form
    {
        private DataView _dataView;
        private EventHandler employeeUpdateHandler;

        public MasterLoan()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            employeeUpdateHandler = (sender, args) => BindEmployee();
            EventHub.Employee += employeeUpdateHandler;

            headerUC1.FormText = Text;
        }

        private void MasterLoan_Load(object sender, EventArgs e)
        {
            ConfigureDataGridViews();
            BindEmployee();
            pnlInfo.Height = 80;
            btnExpand.Text = "More";
        }
        private void MasterLoan_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Employee -= employeeUpdateHandler;
        }

        private void ConfigureDataGridViews()
        {
            dgvEmployee.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvEmployee.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvEmployee.EnableHeadersVisualStyles = false;
            dgvEmployee.RowsDefaultCellStyle.BackColor = Color.White;
            dgvEmployee.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            dgvEmployee.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#D1EAD0");
            dgvEmployee.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvEmployee.BorderStyle = BorderStyle.None;
            dgvEmployee.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvEmployee.RowHeadersVisible = false;
            dgvEmployee.AllowUserToAddRows = dgvEmployee.AllowUserToResizeRows = false;
        }

        public void BindEmployee()
        {
            string query = @"
                    SELECT 
                        e.id,
                        e.code AS `Employee Code`,
                        e.name AS `Name`
                    FROM tbl_employee e";

            DataTable dt = DBClass.ExecuteDataTable(query);
            _dataView = dt.DefaultView;
            dgvEmployee.DataSource = _dataView;
            dgvEmployee.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            // Hide all other columns except "Employee Code" and "Name"
            foreach (DataGridViewColumn column in dgvEmployee.Columns)
            {
                if (column.Name != "Employee Code" && column.Name != "Name")
                    column.Visible = false;
            }

            if (dgvEmployee.Rows.Count > 0)
                BindEmployeeInfo();
            else
                ClearEmployeeDetails();

            LocalizationManager.LocalizeDataGridViewHeaders(dgvEmployee);
            LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoice);
        }

        private void HideUnusedColumns()
        {
            string[] hiddenColumns = { "id", "work_phone", "main_phone", "category", "trn", "email", "region" };
            foreach (var col in hiddenColumns)
                dgvEmployee.Columns[col].Visible = false;
        }

        private void BindVendorInvoice()
        {
            BindEmployeeInfo();
        }

        private void BindEmployeeInfo()
        {
            if (dgvEmployee.SelectedRows.Count == 0)
                return;

            string empCode = dgvEmployee.SelectedRows[0].Cells["Employee Code"].Value.ToString();

            // Fetch Employee Personal Info
            string employeeQuery = @"
                                    SELECT 
                                        e.code,
                                        e.name AS Name,
                                        p.name AS Position, 
                                        e.address AS Address,
                                        e.email AS Email,
                                        e.phone
                                    FROM tbl_employee e
                                    LEFT JOIN tbl_position p ON e.Position_id = p.id  
                                    WHERE e.code = @empCode;
                                    ";

                    var employeeParams = new List<MySqlParameter>
                {
                    new MySqlParameter("@empCode", empCode)
                };

            DataTable empDt = DBClass.ExecuteDataTable(employeeQuery, employeeParams.ToArray());

            if (empDt.Rows.Count > 0)
            {
                lblCode.Text = empDt.Rows[0]["code"].ToString();
                lblName.Text = empDt.Rows[0]["Name"].ToString();
                lblCat.Text = empDt.Rows[0]["Position"].ToString();
                lblAddress.Text = empDt.Rows[0]["Address"].ToString();
                lblE.Text = empDt.Rows[0]["Email"].ToString();
                lblPh.Text = empDt.Rows[0]["phone"].ToString();
            }
            else
            {
                lblName.Text = lblAddress.Text = lblE.Text = "-";
            }

            // Fetch Salary Records and Display in dgvInvoice
            BindEmployeeSalary(empCode);
        }

        private void BindEmployeeSalary(string empCode)
        {
            string salaryQuery = @"
                WITH combined AS (
                    -- Loans Received: TYPE = 'P.V', Order = 1
                    SELECT 
                        pv.DATE,
                        pv.id,
                        'P.V' AS `Type`,
                        'LOANS RECEIVED' AS DESCRIPTION,
                        pvd.total AS `RECEIVED_AMOUNT`,
                        0 AS `PAY_AMOUNT`,
                        1 AS order_type
                    FROM tbl_payment_voucher_details pvd
                    INNER JOIN tbl_payment_voucher pv ON pv.id = pvd.payment_id
                    WHERE 
                        pv.`type` = 'Employee' 
                        AND pvd.voucher_type = 'Employee Loan Payment' 
                        AND pvd.hum_id = (select id from tbl_employee where code=@empCode)

                    UNION ALL

                    -- Loan Payments: TYPE = 'S.S', Order = 2
                    SELECT 
                        s.date,
                        s.id,
                        'S.S' AS `Type`,
                        'SALARY - LOAN PAYMENT INSTALLMENT' AS DESCRIPTION,
                        0 AS `RECEIVED_AMOUNT`,
                        s.total_loan AS `PAY_AMOUNT`,
                        2 AS order_type
                    FROM tbl_attendance_salary s
                    WHERE s.emp_code = @empCode AND s.pay <> 0
                )

                SELECT 
                    DATE,
                    id,
                    `Type`,
                    DESCRIPTION,
                    RECEIVED_AMOUNT,
                    PAY_AMOUNT,
                    SUM(RECEIVED_AMOUNT - PAY_AMOUNT) OVER (ORDER BY order_type, DATE, id ROWS UNBOUNDED PRECEDING) AS BALANCE
                FROM combined
                ORDER BY order_type, DATE, id;";

                    var salaryParams = new List<MySqlParameter>
            {
                new MySqlParameter("@empCode", empCode)
            };

            DataTable salaryDt = DBClass.ExecuteDataTable(salaryQuery, salaryParams.ToArray());

            // Optional total row for final balance
            if (salaryDt.Rows.Count > 0)
            {
                decimal finalBalance = Convert.ToDecimal(salaryDt.Rows[salaryDt.Rows.Count - 1]["BALANCE"]);
                DataRow totalRow = salaryDt.NewRow();
                totalRow["Type"] = "Total";
                totalRow["DESCRIPTION"] = DBNull.Value;
                totalRow["RECEIVED_AMOUNT"] = DBNull.Value;
                totalRow["PAY_AMOUNT"] = DBNull.Value;
                totalRow["BALANCE"] = finalBalance;

                salaryDt.Rows.Add(totalRow);
            }

            dgvInvoice.DataSource = salaryDt;
            dgvInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvInvoice.Rows.Count == 0)
            {
                MessageBox.Show("No salary/loan records found for this employee.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (dgvInvoice.Rows.Count > 0)
            {
                //editCustomerToolStripMenuItem1.v
            }
        }
        private void dgvInvoice_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvInvoice.Rows[e.RowIndex].Cells["Type"].Value?.ToString() == "Total")
            {
                e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold); 
                e.CellStyle.BackColor = Color.LightGray;
            }
        }
        private void ClearEmployeeDetails()
        {
            dgvEmployee.DataSource = null;
            lblName.Text = lblAddress.Text = lblE.Text = "-";
        }

        private void dgvEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                BindEmployeeInfo();
        }
        private void customerListToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void transactionListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Save Word Document"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenXmlWordExporter wordExporter = new OpenXmlWordExporter();
                wordExporter.ExportToWord(dgvEmployee, saveFileDialog.FileName);
            }
        }

        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void receivePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            if (pnlInfo.Height == 80)
            {
                pnlInfo.Height = 240;
                btnExpand.Text = "Less";
            }
            else
            {
                pnlInfo.Height = 80;
                btnExpand.Text = "More";
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ReportPrinter reportPrinter = new ReportPrinter(dgvEmployee, "Employee Data Report \n ( Employee ) \n All Employee", true);
            reportPrinter.Print();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save Excel File"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelExporter exporter = new ExcelExporter(dgvEmployee);
                exporter.ExportToExcel(saveFileDialog.FileName);
            }
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Save Word Document"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenXmlWordExporter wordExporter = new OpenXmlWordExporter();
                wordExporter.ExportToWord(dgvEmployee, saveFileDialog.FileName);
            }
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterAttendanceSheet(0));
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmSalarySheet(0));
        }

        private void endOfServicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmLeaveSalary(0));
        }

        private void endOfServicesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmEndOfService(0));
        }

        private void loanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewLoan(0));
        }

        private void finalSettlementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmFinalSettlement(0));
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewEmployee(0));
        }

        private void editEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewEmployee(int.Parse(dgvEmployee.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void deleteEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.Rows.Count == 0)
                return;
            int _id = (int.Parse(dgvEmployee.SelectedRows[0].Cells["id"].Value.ToString()));

            if (dgvInvoice.Rows.Count <= 1)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteData(_id);
                }
                else
                {
                    MessageBox.Show("Deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Employee has transactions. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DeleteData(int _id)
        {
            DBClass.ExecuteNonQuery(@"Delete from tbl_employee where id = @id;Delete from tbl_transaction where transaction_id=@id and type='Employee Payment'",
                DBClass.CreateParameter("id", _id));
            Utilities.LogAudit(frmLogin.userId, "Delete", "Employee", _id, "Deleted Employee with ID: " + _id);
            BindEmployee();
            MessageBox.Show("Employee deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewLoan());
        }
    }
}
