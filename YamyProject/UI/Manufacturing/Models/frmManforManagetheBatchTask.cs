using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using YamyProject.RMS.Class;
using YamyProject.UI.Manufacturing.Viewform;

namespace YamyProject.UI.Manufacturing.Models
{
    public partial class frmManforManagetheBatchTask : Form
    {
        private frmManScreen _formA;

        public int idmach;
        public string nameMachin;
        private List<EmployeeModel> employeesList;
        private List<EmployeeModel> selectedEmployees;

        public frmManforManagetheBatchTask(frmManScreen formA)
        {
            InitializeComponent();
            _formA = formA;

        }

        private void frmManforManagetheBatchTask_Load(object sender, EventArgs e)
        {
            GetData();

            DataTable dt = DBClass.ExecuteDataTable(@"SELECT CONCAT(e.CODE, ' - ', e.Name) AS NAME, e.id, CONCAT(d.id, ' - ', d.name, d.Department) AS Department,
                CONCAT(p.id, ' - ', p.name) AS Position FROM tbl_employee e JOIN tbl_departments d ON e.Department_id = d.id JOIN tbl_position p ON e.position_id = p.id
                WHERE d.Department = 'Manufacture';");
            employeesList = new List<EmployeeModel>();
            foreach (DataRow row in dt.Rows)
            {
                employeesList.Add(new EmployeeModel
                {
                    Id = Convert.ToInt32(row["id"]),
                    Name = Convert.ToString(row["NAME"]),
                    Department = Convert.ToString(row["Department"]),
                    Position = Convert.ToString(row["Position"])
                });
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddWorkOrder());
            GetData();
        }

        public void GetData()
        {
            //string qry = @" SELECT id,batchname,hours,amount,Description,'Add..' resource FROM tbl_manufacturer_batch 
            //             WHERE
            //             batchname LIKE '%" + guna2TextBox1.Text + "%' and fixedassetsID = '" + idmach + "' ";
            string qry = @"SELECT b.id, b.batchname, b.hours, b.amount, b.Description,
                                COALESCE(
                                    (
                                        SELECT GROUP_CONCAT(e.name SEPARATOR ', ')
                                        FROM tbl_manufacturer_task_details d
                                        JOIN tbl_employee e ON e.id = d.EmployeeID
                                        WHERE d.TaskID = (
                                            SELECT id 
                                            FROM tbl_manufacturer_task 
                                            WHERE BatchID = b.id AND MachineID = b.fixedassetsID
                                            LIMIT 1
                                        )
                                    ),
                                    'Add...'
                                ) AS resource
                            FROM tbl_manufacturer_batch b
                            WHERE b.batchname LIKE '%" + guna2TextBox1.Text + @"%' 
                              AND b.fixedassetsID = '" + idmach + @"';
                            ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvdate);
            lb.Items.Add(dgvamount);
            lb.Items.Add(dgvdescr);
            lb.Items.Add(dgvlinkResource);
            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;
            if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                return;

            if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "Add...")
            {
                using(MySqlDataReader reader = DBClass.ExecuteReader(
                    @"SELECT e.id, CONCAT(e.CODE, ' - ', e.Name) AS Name, 
                      CONCAT(d.id, ' - ', d.name, d.Department) AS Department, 
                      CONCAT(p.id, ' - ', p.name) AS Position
                      FROM tbl_manufacturer_task_details td
                      JOIN tbl_employee e ON td.EmployeeID = e.id
                      JOIN tbl_departments d ON e.Department_id = d.id
                      JOIN tbl_position p ON d.id = p.department_id
                      WHERE td.TaskID = " + Convert.ToInt32(guna2DataGridView1.Rows[e.RowIndex].Cells["dgvSno"].Value)))
                {
                    if (reader.HasRows)
                    {
                        selectedEmployees = new List<EmployeeModel>();
                    }
                    while (reader.Read())
                    {
                        selectedEmployees.Add(new EmployeeModel
                        {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetString("Name"),
                            Department = reader.GetString("Department"),
                            Position = reader.GetString("Position")
                        });
                    }
                }
            }
            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvlinkResource") {
                frmManEmployeeSelectionForm form = new frmManEmployeeSelectionForm(employeesList);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (form.SelectedEmployees.Count > 0)
                    {
                        List<string> selected = form.SelectedEmployees.Select(emp => emp.ToString()).ToList();
                        string combined = string.Join(", ", selected);
                        if (!string.IsNullOrEmpty(combined))
                        {
                            guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = combined;
                            selectedEmployees = form.SelectedEmployees;
                        }
                        else
                        {
                            guna2DataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "Add...";
                            selectedEmployees.Clear();
                        }
                    }
                }
            } 
            else
            {
                string message = "Do you want to run this machine production: '" + nameMachin + "'\n" +
                 "Start Task: '" + Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value) + "'";

                DialogResult result = MessageBox.Show(
                    message,
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    object qtyResult = DBClass.ExecuteScalar(
                        @"SELECT CASE WHEN COUNT(*) = SUM(
                                    CASE WHEN b.qty = b.RequestQty AND b.RequestQty = b.ReceiveQty AND b.RequestQty > 0 AND b.ReceiveQty > 0 
                                        THEN 1 ELSE 0 END ) THEN 1
                                ELSE 0 END AS IsBatchComplete
                        FROM tbl_manufacturer_batch a JOIN tbl_manufacturer_batchdetails b ON a.id = b.batchID
                        WHERE a.batchname = @id;",
                        DBClass.CreateParameter("id", Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value))
                    );

                    decimal allItemsQtyTaken = qtyResult != DBNull.Value ? Convert.ToDecimal(qtyResult) : 0;
                    if (allItemsQtyTaken > 0)
                    {
                        doprogress();
                        insertthetask();
                        _formA.GetOrdersProgress();
                        _formA.GetOrdersDraft();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Production cannot proceed because raw materials are still pending receipt.", "Work (BOM) Material");

                        this.Close();
                    }
                }
            }
        }

        private void doprogress()
        {
            string qry1 = @"update  tbl_fixed_assets set manufactureStatus= @manufactureStatus where id =@ID";
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            cmd.Parameters.AddWithValue("@id", idmach);
            cmd.Parameters.AddWithValue("@manufactureStatus", "Progress");
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            cmd.ExecuteNonQuery();
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();

            }
            Utilities.LogAudit(frmLogin.userId, "Update Machine Status", "Machine", idmach, "Updated Manufacture Status to Progress for Machine: " + nameMachin);
        }
        private void insertthetask()
        {
            DateTime starttime = DateTime.Now;
            DateTime endtime = starttime.AddHours(Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvdate"].Value));
            string qry2 = @"INSERT INTO tbl_manufacturer_task (MachineID,BatchID,StartTime,EndTime,Status,userID)
                            VALUES (@MachineID,@BatchID,@StratTime,@EndTime,@Status,@userID) ; SELECT LAST_INSERT_ID();";
            MySqlCommand cmd = new MySqlCommand(qry2, RMSClass.conn());
            cmd.Parameters.AddWithValue("@MachineID", idmach);
            cmd.Parameters.AddWithValue("@BatchID", Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value));
            cmd.Parameters.AddWithValue("@StratTime", starttime);
            cmd.Parameters.AddWithValue("@EndTime", endtime);
            cmd.Parameters.AddWithValue("@Status", "Progress");
            cmd.Parameters.AddWithValue("@userID", frmLogin.userId);
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            var taskId = cmd.ExecuteScalar();
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }

            InsertTaskDetails(int.Parse(taskId.ToString()), selectedEmployees);
            Utilities.LogAudit(frmLogin.userId, "Add Task", "Task", Convert.ToInt32(taskId), "Added Task: " + guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
        }

        private void InsertTaskDetails(int taskId, List<EmployeeModel> selectedEmployees)
        {
            DBClass.ExecuteNonQuery(
                @"DELETE FROM tbl_manufacturer_task_details WHERE TaskID = @TaskID",
                DBClass.CreateParameter("@TaskID", taskId));

            if (selectedEmployees != null || selectedEmployees.Count != 0)
            {
                foreach (EmployeeModel employee in selectedEmployees)
                {
                    var departmentId = employee.Department.Split('-')[0].Trim();
                    var departmentName = employee.Department.Split('-')[1].Trim();
                    string query = @"INSERT INTO tbl_manufacturer_task_details(TaskID, DepartmentID, EmployeeID, StartTime, EndTime, Status) 
                                    VALUES (@TaskID, @DepartmentID, @EmployeeID, @StartTime, @EndTime, @Status)";
                    DBClass.ExecuteNonQuery(query,
                    DBClass.CreateParameter("@TaskID", taskId),
                    DBClass.CreateParameter("@DepartmentID", departmentId),
                    DBClass.CreateParameter("@EmployeeID", employee.Id),
                    DBClass.CreateParameter("@StartTime", DBNull.Value),
                    DBClass.CreateParameter("@EndTime", DBNull.Value),
                    DBClass.CreateParameter("@Status", "Pending"));
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}