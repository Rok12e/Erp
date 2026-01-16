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
    public partial class frmLeaveSalaryStatement : Form
    {
        int id;

        public frmLeaveSalaryStatement( int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            this.id = id;
        }

        private void frmLeaveSalaryStatement_Load(object sender, EventArgs e)
        {
            DataTable dt = DBClass.ExecuteDataTable(@"
                                SELECT 
                                    ROW_NUMBER() OVER (ORDER BY id) AS SN, 
                                    `code`, 
                                    `name`, 
                                    `Reference`, 
                                    `description`, 
                                    leave_days, 
                                    debit, 
                                    credit,
                                    SUM(credit - debit) OVER (PARTITION BY code ORDER BY id) AS Balance
                                FROM tbl_leave_salary
                                WHERE code = @id
                                ORDER BY code, id",
                            DBClass.CreateParameter("@id", id));

            dgvLeaveSalary.DataSource = dt;
            dgvLeaveSalary.Columns["code"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["Reference"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["leave_days"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["leave_days"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["debit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["debit"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["credit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["credit"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["Balance"].DefaultCellStyle.Format = "N2";

            dgvLeaveSalary.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvLeaveSalary.DefaultCellStyle.Font = new Font("Times New Roman", 9);
            dgvLeaveSalary.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray;
            dgvLeaveSalary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvLeaveSalary.EnableHeadersVisualStyles = false;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvLeaveSalary);
        }

    }
}
