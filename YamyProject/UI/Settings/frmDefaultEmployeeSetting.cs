using DocumentFormat.OpenXml.Bibliography;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmDefaultEmployeeSetting : Form
    {
        public frmDefaultEmployeeSetting()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void frmDefaultEmployeeSetting_Load(object sender, EventArgs e)
        {
            for (int i = 2000; i <= 2050; i++)
            {
                cmbYear.Items.Add(i);
            }
            cmbYear.SelectedItem = DateTime.Now.Year; // Set default to current year

            // Fill Month ComboBox
            string[] months = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            foreach (string month in months)
            {
                if (!string.IsNullOrEmpty(month)) // Avoid empty entry
                    cmbMonth.Items.Add(month);
            }
            cmbMonth.SelectedIndex = DateTime.Now.Month - 1; // Set default to current month

            LoadData();
        }

        private void LoadData()
        {
            if (cmbYear.SelectedItem == null || cmbMonth.SelectedItem == null)
            {
                //MessageBox.Show("Please select a valid Year and Month.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int year = Convert.ToInt32(cmbYear.SelectedItem);
            int month = cmbMonth.SelectedIndex + 1;
            int dayCount = 1;
            dgvMain.Rows.Clear();
            dgvDays.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_setting_attendance
                        WHERE MONTH(`date`) = @month AND YEAR(`date`) = @year;",
              DBClass.CreateParameter("month", month),
              DBClass.CreateParameter("year", year)
              ))
                while (reader.Read())
                {
                    //date`, `day`, `timein`, `timeout`, `state
                    var _date = reader["date"];
                    string _day = reader["day"].ToString();
                    var _timein = reader["timein"];
                    var _timeout = reader["timeout"];
                    int _state = (int)reader["state"];

                    txtIn.Text = _timein.ToString();
                    txtOut.Text = _timeout.ToString();

                    //int daysInMonth = DateTime.DaysInMonth(year, month); // Get total days in the selected month
                    //string inTime = txtIn.Text.Trim();
                    //string outTime = txtOut.Text.Trim();
                    // Loop through each day of the selected month and add to DataGridView
                    //for (int day = 1; day <= daysInMonth; day++)
                    //{

                        DateTime date = new DateTime(year, month, dayCount);

                        bool exists = dgvDays.Rows.Cast<DataGridViewRow>()
                            .Any(row => row.Cells["Day"].Value != null &&
                                        row.Cells["Day"].Value.ToString().Trim().Equals(date.DayOfWeek.ToString(), StringComparison.OrdinalIgnoreCase));
                        if (cmbTimIn.SelectedIndex == 0)
                            dgvMain.Rows.Add(dayCount, date.ToString("yyyy-MM-dd"), date.DayOfWeek, _timein, _timeout, _state);
                        else
                            dgvMain.Rows.Add(dayCount, date.ToString("yyyy-MM-dd"), date.DayOfWeek, _timein, _timeout, _state);

                    if (_state == 0)
                    {
                        bool dayExist = false;
                        for (int i = 0; i < dgvDays.Rows.Count; i++)
                        {
                            if (dgvDays.Rows[i].Cells[0].Value.ToString() == _day)
                            {
                                dayExist = true;
                            }
                        }
                        if (dgvDays.Rows.Count <= 0)
                        {
                            dgvDays.Rows.Add(_day);
                        } else if (!dayExist)
                        {
                            dgvDays.Rows.Add(_day);
                        }
                    }
                    //}
                    dayCount++;
                }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvDays.Rows.Count; i++)
            {
                if (dgvDays.Rows[i].Cells[0].Value.ToString() == cmbDay.Text)
                {
                    MessageBox.Show("Day Already Add ");
                    return;
                }
            }
            dgvDays.Rows.Add(cmbDay.Text);
        }

        private void btModify_Click(object sender, EventArgs e)
        {
            if (txtIn.Text.Trim() == "" || txtOut.Text.Trim() == "")
            {
                MessageBox.Show("Enter Time", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string inTime = txtIn.Text.Trim();
            string outTime = txtOut.Text.Trim();
            // Ensure a year and month are selected
            if (cmbYear.SelectedItem == null || cmbMonth.SelectedItem == null)
            {
                MessageBox.Show("Please select a valid Year and Month.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int year = Convert.ToInt32(cmbYear.SelectedItem);
            int month = cmbMonth.SelectedIndex + 1; // Convert month name to month number

            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.Cells["Date"].Value != null)
                {
                    DateTime existingDate;
                    if (DateTime.TryParse(row.Cells["Date"].Value.ToString(), out existingDate))
                    {
                        if (existingDate.Year == year && existingDate.Month == month)
                        {
                            MessageBox.Show($"Entries for {cmbMonth.SelectedItem} {year} already exist!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }
            }
            int daysInMonth = DateTime.DaysInMonth(year, month); // Get total days in the selected month

            // Loop through each day of the selected month and add to DataGridView
            for (int day = 1; day <= daysInMonth; day++)
            {

                DateTime date = new DateTime(year, month, day);

                bool exists = dgvDays.Rows.Cast<DataGridViewRow>()
                    .Any(row => row.Cells["Day"].Value != null &&
                                row.Cells["Day"].Value.ToString().Trim().Equals(date.DayOfWeek.ToString(), StringComparison.OrdinalIgnoreCase));
                if (cmbTimIn.SelectedIndex == 0)
                    dgvMain.Rows.Add(day, date.ToString("yyyy-MM-dd"), date.DayOfWeek, inTime, outTime, exists ? 0 : 1);
                else
                    dgvMain.Rows.Add(day, date.ToString("yyyy-MM-dd"), date.DayOfWeek, inTime, outTime, exists ? 0 : 1);
            }

            MessageBox.Show($"Entries for {cmbMonth.SelectedItem} {year} have been added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveAttendanceToDatabase()
        {
            int year = Convert.ToInt32(cmbYear.SelectedItem);
            int month = cmbMonth.SelectedIndex + 1;

            int count = 0;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT COUNT(*) 
                        FROM tbl_setting_attendance
                        WHERE MONTH(`date`) = @month AND YEAR(`date`) = @year;",
              DBClass.CreateParameter("month", month),
              DBClass.CreateParameter("year", year)
              ))
                if (reader.Read())
                {
                    count = Convert.ToInt32(reader[0]);
                }
            if (count > 0)
            {
                MessageBox.Show($"Attendance data for {cmbMonth.SelectedItem} {year} already exists in the database!", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            foreach (DataGridViewRow row in dgvMain.Rows)
            {
                if (row.Cells["date"].Value != null && row.Cells["dayy"].Value != null)
                {
                    string inTime = txtIn.Text.Trim();
                    string outTime = txtOut.Text.Trim();
                    string timeInFormatted = row.Cells["timein"].Value.ToString() + ":00";
                    string timeOutFormatted = row.Cells["timeout"].Value.ToString() + ":00";
                    if (!System.Text.RegularExpressions.Regex.IsMatch(inTime, @"^\d{1,2}:\d{2}$") ||
                        !System.Text.RegularExpressions.Regex.IsMatch(outTime, @"^\d{1,2}:\d{2}$"))
                    {
                        MessageBox.Show("Invalid IN or OUT time format. Please enter time as HH:mm (e.g. 08:00).", "Time Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string query = "INSERT INTO tbl_setting_attendance (`date`, `day`, `timein`, `timeout`, `state`) " +
                               "VALUES (@date, @day, @timein, @timeout,@state)";
                    DateTime attendanceDate = Convert.ToDateTime(row.Cells["date"].Value);
                    string attendanceDay = row.Cells["dayy"].Value.ToString();
                    int state = Convert.ToInt32(row.Cells["state"].Value);
                    DBClass.ExecuteNonQuery(query,
                        DBClass.CreateParameter("@date", attendanceDate.ToString("yyyy-MM-dd")),
                        DBClass.CreateParameter("@day", attendanceDay),
                        DBClass.CreateParameter("@timein", timeInFormatted),
                        DBClass.CreateParameter("@timeout", timeOutFormatted),
                        DBClass.CreateParameter("@state", state));
                }
            }
            MessageBox.Show("Attendance data saved successfully!");
            ResetAttendanceForm();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveAttendanceToDatabase();
        }

        private void cmbTimIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTimIn.SelectedIndex == 0)
                cmbTimeOut.SelectedIndex = 1;
            else
                cmbTimeOut.SelectedIndex = 0;
        }
        private void ResetAttendanceForm()
        {
            dgvMain.Rows.Clear();     // Clear all rows in the main attendance grid
        }

        private void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
