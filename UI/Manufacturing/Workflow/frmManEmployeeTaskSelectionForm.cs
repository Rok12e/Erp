using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.RMS.Class;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManEmployeeTaskSelectionForm : Form
    {
        private int loggedInEmployeeId = 0;

        public frmManEmployeeTaskSelectionForm(int loggedInEmployeeId)
        {
            InitializeComponent();
            this.loggedInEmployeeId = loggedInEmployeeId;
        }

        private void frmManEmployeeTaskSelectionForm_Load(object sender, EventArgs e)
        {
            lblEmpName.Text = "Employee ID: " + loggedInEmployeeId.ToString();
            LoadTasks();
        }

        private void LoadTasks()
        {
            clbTasks.Items.Clear();

            string query = @"SELECT td.id, CONCAT(d.name, ' (TaskID: ', td.id, ')') AS TaskName
                         FROM tbl_manufacturer_task_details td
                         JOIN departments d ON td.DepartmentID = d.id
                         WHERE td.EmployeeID = @empId AND td.Status IN ('Pending', 'Progress')";

            DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@empId", loggedInEmployeeId));

            foreach (DataRow row in dt.Rows)
            {
                clbTasks.Items.Add(new TaskItem
                {
                    TaskDetailId = Convert.ToInt32(row["id"]),
                    DisplayName = row["TaskName"].ToString()
                }, false);
            }
        }

        private void btnStartSelected_Click(object sender, EventArgs e)
        {
            UpdateTaskStatus("Progress");
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            UpdateTaskStatus("Done", true);
        }

        private void UpdateTaskStatus(string status, bool setEndTime = false)
        {
            foreach (TaskItem item in clbTasks.CheckedItems)
            {
                MySqlCommand cmd = new MySqlCommand(
                    @"UPDATE tbl_manufacturer_task_details
                  SET Status = @status " +
                    (setEndTime ? ", EndTime = @endTime " : "") +
                    "WHERE id = @id", RMSClass.conn());

                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id", item.TaskDetailId);
                if (setEndTime)
                    cmd.Parameters.AddWithValue("@endTime", DateTime.Now);

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }

            MessageBox.Show("Task(s) updated successfully!", "Success");
            LoadTasks();
        }

        class TaskItem
        {
            public int TaskDetailId { get; set; }
            public string DisplayName { get; set; }

            public override string ToString() => DisplayName;
        }
    }
    
}
