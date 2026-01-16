using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmEndOfService : Form
    {

        public frmEndOfService(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;

        }

        private void frmEndOfService_Load(object sender, EventArgs e)
        {
            bindEndOfService();
        }

        private void bindEndOfService()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT 
                    CODE AS 'Employee Code',
                    NAME AS 'Employee Name',
                    SUM(leave_days) AS 'End Of Service Days',
                    SUM(credit) AS 'End Of Service Amount',
                    SUM(leave_days) - (SUM(debit) / (SUM(credit) / SUM(leave_days))) AS 'EOS Remaining Days',
                    SUM(debit) AS 'EOS Received Amount',
                    SUM(credit) - SUM(debit) AS 'EOS Remaining Amount'
                FROM tbl_end_of_service
                GROUP BY CODE,name;
                ");
            dgvLeaveSalary.DataSource = dt;

            dgvLeaveSalary.Columns["Employee Code"].Width = 100;
            dgvLeaveSalary.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["End Of Service Days"].Width = 120;
            dgvLeaveSalary.Columns["End Of Service Days"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["End Of Service Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["End Of Service Amount"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["EOS Remaining Days"].Width = 100;
            dgvLeaveSalary.Columns["EOS Remaining Days"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["EOS Received Amount"].Width = 150;
            dgvLeaveSalary.Columns["EOS Received Amount"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["EOS Remaining Amount"].Width = 160;
            dgvLeaveSalary.Columns["EOS Remaining Amount"].DefaultCellStyle.Format = "N2";
            LocalizationManager.LocalizeDataGridViewHeaders(dgvLeaveSalary);
        }


        private void dgvLeaveSalary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLeaveSalary.Rows.Count != 0)
            {
              frmLogin.frmMain.openChildForm(new frmEndOfServiceStatement(int.Parse(dgvLeaveSalary.CurrentRow.Cells["Employee Code"].Value.ToString())));
            }
        }
    }
}
