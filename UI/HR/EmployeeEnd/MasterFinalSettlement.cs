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
    public partial class MasterFinalSettlement : Form
    {

        public MasterFinalSettlement()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmFinalSettlement());
        }
        private void MasterFinalSettlement_Load(object sender, EventArgs e)
        {
            BindFinalSet();
        }
        public void BindFinalSet()
        {
            DataTable dt;
            string query = @"
                            SELECT 
                                e.id,
                                e.code AS 'Emp Code', 
                                e.name AS 'Employee Name',
                                tf.DateCommencement AS 'DateCommencement', 
                                tf.DateLastWork AS 'DateLastWork',
                                tf.TotalSalary AS 'TotalSalary', 
                                tf.TotalAdditions AS 'TotalAdditions', 
                                tf.TotalDeductions AS 'TotalDeductions',
                                tf.NetAccruals AS 'NetAccruals'
                            FROM tbl_final_settlement tf
                            INNER JOIN tbl_employee e ON tf.emp_id = e.id";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbState.Text == "Active")
                query += " AND active = 0";
            else if (cmbState.Text != "All")
                query += " AND active != 0";

            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvCustomer.DataSource = dt;

            // Set column widths and alignment
            dgvCustomer.Columns["Emp Code"].Width = 100;
            dgvCustomer.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["DateCommencement"].Width = 120;
            dgvCustomer.Columns["DateLastWork"].Width = 120;
            dgvCustomer.Columns["TotalSalary"].Width = 100;
            dgvCustomer.Columns["TotalAdditions"].Width = 100;
            dgvCustomer.Columns["TotalDeductions"].Width = 100;
            dgvCustomer.Columns["NetAccruals"].Width = 120;
            dgvCustomer.Columns["id"].Visible = false;

            //// Align numeric values for better readability
            //dgvCustomer.Columns["TotalSalary"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvCustomer.Columns["TotalAdditions"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvCustomer.Columns["TotalDeductions"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvCustomer.Columns["NetAccruals"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvCustomer.DefaultCellStyle.Font = new Font("Times New Roman", 9);
            dgvCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray; // Header background color
            dgvCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Header text color
            dgvCustomer.EnableHeadersVisualStyles = false;

            if (dgvCustomer.Rows.Count > 0)
            {
                btnDelete.Visible = UserPermissions.canDelete("Final Settlement");
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }


        //private void btnEdit_Click(object sender, EventArgs e)
        //{
        //    if (dgvCustomer.Rows.Count == 0)
        //        return;
        //    _mainForm.openChildForm(new frmViewEmployee(_mainForm, this, int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        //}
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_employee SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Final Settlement", "Final Settlement", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Final Settlement for Employee: " + dgvCustomer.SelectedRows[0].Cells["Employee Name"].Value.ToString());
            BindFinalSet();
        }

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFinalSet();
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmFinalSettlement(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }

        //private void btnOther_Click(object sender, EventArgs e)
        //{
        //    if (pnlWindow.Visible == true)
        //        pnlWindow.Visible = false;
        //    else pnlWindow.Visible = true;
        //}

        //private void btnAttendenceSheet_Click(object sender, EventArgs e)
        //{
        //    pnlWindow.Visible = false;
        //    _mainForm.openChildForm(new MasterAttendanceSheet(_mainForm));
        //}

        //private void btnSalarySheet_Click(object sender, EventArgs e)
        //{
        //    pnlWindow.Visible = false;
        //    _mainForm.openChildForm(new frmSalarySheet(_mainForm));
        //}

        //private void btnLoan_Click(object sender, EventArgs e)
        //{
        //    pnlWindow.Visible = false;
        //    _mainForm.openChildForm(new frmViewLoan(_mainForm));
        //}

        //private void btnEndOfService_Click(object sender, EventArgs e)
        //{
        //    pnlWindow.Visible = false;
        //    _mainForm.openChildForm(new frmEndOfServices(_mainForm));
        //}

        //private void btnLeaveSalary_Click(object sender, EventArgs e)
        //{
        //    pnlWindow.Visible = false;
        //    _mainForm.openChildForm(new frmLeaveSalary(_mainForm));
        //}

        //private void btnFinalSettlement_Click(object sender, EventArgs e)
        //{
        //    pnlWindow.Visible = false;
        //    _mainForm.openChildForm(new frmFinalSettlement(_mainForm));
        //}
    }
}
