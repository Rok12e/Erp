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
    public partial class frmLeaveSalary : Form
    {

        public frmLeaveSalary(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;

        }

        private void frmLeaveSalary_Load(object sender, EventArgs e)
        {
            //EmpLeaveSalary();
            bindLeaveSalary();
        }
        private void bindLeaveSalary()
        {
          
                DataTable dt = DBClass.ExecuteDataTable(@"SELECT 
                                    CODE AS 'Employee Code',
                                    NAME AS 'Employee Name',
                                    SUM(leave_days) AS 'Leave Salary Days',
                                    SUM(credit) AS 'Leave Salary Amount',
                                    SUM(debit) / (SUM(credit) / SUM(leave_days)) AS 'L.S Used Days',
                                    SUM(leave_days) - (SUM(debit) / (SUM(credit) / SUM(leave_days))) AS 'L.S Remaining Days',
                                    SUM(debit) AS 'L.S Received Amount',
                                    SUM(credit) - SUM(debit) AS 'L.S Remaining Amount'
                                FROM tbl_leave_salary
                                GROUP BY CODE,name;
                                ");
            dgvLeaveSalary.DataSource = dt;

            dgvLeaveSalary.Columns["Employee Code"].Width = 100;
            dgvLeaveSalary.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["Leave Salary Days"].Width = 120;
            dgvLeaveSalary.Columns["Leave Salary Days"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["Leave Salary Amount"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["Leave Salary Amount"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["L.S Used Days"].Width = 100;
            dgvLeaveSalary.Columns["L.S Used Days"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["L.S REMAINING DAYS"].Width = 100;
            dgvLeaveSalary.Columns["L.S REMAINING DAYS"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["L.S RECEIVED AMOUNT"].Width = 150;
            dgvLeaveSalary.Columns["L.S RECEIVED AMOUNT"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["L.S REMAINING AMOUNT"].Width = 160;
            dgvLeaveSalary.Columns["L.S REMAINING AMOUNT"].DefaultCellStyle.Format = "N2";
            LocalizationManager.LocalizeDataGridViewHeaders(dgvLeaveSalary);
        }

        private void EmpLeaveSalary()
        {
            DataTable dt;
            string query = @"
        WITH LastMonthAttendance AS (
            -- Get the most recent month worked for each employee
            SELECT 
                EmpID, YEAR(WorkDate) AS YearWorked, MONTH(WorkDate) AS MonthWorked
            FROM tbl_attendancesheet
            WHERE WorkDate = (
                SELECT MAX(WorkDate)
                FROM tbl_attendancesheet sub
                WHERE sub.EmpID = tbl_attendancesheet.EmpID
            )
            GROUP BY EmpID, YearWorked, MonthWorked
        )
        SELECT 
            e.code AS `EmpID`, 
            e.EmployeeName AS `EmpName`, 
            e.WorkDays AS `WorkDays`, 
            COALESCE(latest_attendance.`Absent Days`, 0) AS `Absent Days`, 
            e.BasicSalary AS `Basic Salary`, 
            (e.HousingAllowance + e.TransportationAllowance) AS `Allowance`, 
            e.Other AS `Others`, 
            (e.BasicSalary + e.HousingAllowance + e.TransportationAllowance + e.Other) AS `Total Additions`, 
            ((e.WorkDays - COALESCE(latest_attendance.`Absent Days`, 0)) / 365 * 30) AS `Leave Salary Days`, 
            (e.BasicSalary * 12 / 365 * ((e.WorkDays - COALESCE(latest_attendance.`Absent Days`, 0)) / 365 * 30)) AS `Leave Salary Amount`,
            
            -- Vacation count for the last month worked
            COUNT(CASE WHEN a.Status = 'v' AND YEAR(a.WorkDate) = lma.YearWorked AND MONTH(a.WorkDate) = lma.MonthWorked THEN 1 END) AS vacation_count,

            -- Total vacation days from settings
            (SELECT COUNT(*) FROM tbl_setting_attendance WHERE state = 0) AS total_vacation_days,

            -- Extra vacation days (L.S USED DAYS)
            GREATEST(
                COUNT(CASE WHEN a.Status = 'v' AND YEAR(a.WorkDate) = lma.YearWorked AND MONTH(a.WorkDate) = lma.MonthWorked THEN 1 END) - 
                (SELECT COUNT(*) FROM tbl_setting_attendance WHERE state = 0), 0
            ) AS `L.S Used Days`,

            -- L.S REMAINING DAYS
            ((e.WorkDays - COALESCE(latest_attendance.`Absent Days`, 0)) / 365 * 30) - 
            GREATEST(
                COUNT(CASE WHEN a.Status = 'v' AND YEAR(a.WorkDate) = lma.YearWorked AND MONTH(a.WorkDate) = lma.MonthWorked THEN 1 END) - 
                (SELECT COUNT(*) FROM tbl_setting_attendance WHERE state = 0), 0
            ) AS `L.S Remaining Days`,

            -- L.S RECEIVED AMOUNT (Kept as 0 for now)
            0 AS `L.S Received Amount`,

            -- L.S REMAINING AMOUNT = Leave Salary Amount - L.S RECEIVED AMOUNT
            (e.BasicSalary * 12 / 365 * ((e.WorkDays - COALESCE(latest_attendance.`Absent Days`, 0)) / 365 * 30)) - 0 AS `L.S Remaining Amount`
            
        FROM tbl_employee e
        INNER JOIN tbl_attendancesheet a 
            ON e.code = a.EmpID
        INNER JOIN LastMonthAttendance lma 
            ON a.EmpID = lma.EmpID
            AND YEAR(a.WorkDate) = lma.YearWorked
            AND MONTH(a.WorkDate) = lma.MonthWorked
        LEFT JOIN (
            -- Get the most recent attendance data per employee (most recent attendance date)
            SELECT 
                a.EmpID, 
                SUM(CASE WHEN a.Status = 'A' THEN 1 ELSE 0 END) AS `Absent Days`
            FROM tbl_attendancesheet a
            WHERE a.WorkDate = (
                SELECT MAX(sub.WorkDate)
                FROM tbl_attendancesheet sub
                WHERE sub.EmpID = a.EmpID
            )
            GROUP BY a.EmpID
        ) AS latest_attendance 
            ON e.code = latest_attendance.EmpID

        GROUP BY e.code, e.EmployeeName, e.WorkDays, e.BasicSalary, 
                 e.HousingAllowance, e.TransportationAllowance, 
                 e.Other, latest_attendance.`Absent Days`, 
                 lma.YearWorked, lma.MonthWorked;";

            dt = DBClass.ExecuteDataTable(query, new MySqlParameter[] { });
            dgvLeaveSalary.DataSource = dt;

            // Adjust column widths dynamically for a better UI experience
            dgvLeaveSalary.Columns["EmpID"].Width = 100;
            dgvLeaveSalary.Columns["EmpName"].MinimumWidth = 180;
            dgvLeaveSalary.Columns["EmpName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvLeaveSalary.Columns["WorkDays"].Visible = false;
            dgvLeaveSalary.Columns["Absent Days"].Visible = false;
            dgvLeaveSalary.Columns["Basic Salary"].Visible = false;
            dgvLeaveSalary.Columns["Allowance"].Visible = false;
            dgvLeaveSalary.Columns["Others"].Visible = false;
            dgvLeaveSalary.Columns["Total Additions"].Visible = false;
            dgvLeaveSalary.Columns["Leave Salary Days"].Width = 140;
            dgvLeaveSalary.Columns["Leave Salary Days"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["Leave Salary Amount"].Width = 160;
            dgvLeaveSalary.Columns["Leave Salary Amount"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["L.S USED DAYS"].Width = 140;
            dgvLeaveSalary.Columns["L.S REMAINING DAYS"].Width = 140;
            dgvLeaveSalary.Columns["L.S REMAINING DAYS"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["L.S RECEIVED AMOUNT"].Width = 140;
            dgvLeaveSalary.Columns["L.S RECEIVED AMOUNT"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["L.S REMAINING AMOUNT"].Width = 160;
            dgvLeaveSalary.Columns["L.S REMAINING AMOUNT"].DefaultCellStyle.Format = "N2";
            dgvLeaveSalary.Columns["vacation_count"].Visible = false;
            dgvLeaveSalary.Columns["total_vacation_days"].Visible = false;

            // Apply professional styling
            dgvLeaveSalary.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            dgvLeaveSalary.DefaultCellStyle.Font = new Font("Times New Roman", 9);
            dgvLeaveSalary.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSlateGray; // Header background color
            dgvLeaveSalary.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; // Header text color
            dgvLeaveSalary.EnableHeadersVisualStyles = false;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvLeaveSalary);
        }

        private void dgvLeaveSalary_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLeaveSalary.Rows.Count != 0)
            {
                frmLogin.frmMain.openChildForm(new frmLeaveSalaryStatement(int.Parse(dgvLeaveSalary.CurrentRow.Cells["Employee Code"].Value.ToString())));
            }
        }
    }
}
