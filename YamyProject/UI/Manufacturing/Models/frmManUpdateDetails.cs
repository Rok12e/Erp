using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.RMS.Class;

namespace YamyProject.UI.Manufacturing.Models
{
    public partial class frmManUpdateDetails : Form
    {
        public int id = 0;
        public frmManUpdateDetails()
        {
            InitializeComponent();
        }

        private void frmManUpdateDetails_Load(object sender, EventArgs e)
        {
          
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            string qry1 = @"UPDATE tbl_manufacturer_task_details 
                     SET StartTime = @StartTime,
                         EndTime = @EndTime,
                         Status = @Status,
                         Remarks = @Remarks 
                     WHERE id = @ID";
            MySqlCommand cmd = new MySqlCommand(qry1, RMSClass.conn());
            DateTime startDateTime = dateTimePicker1.Value.Date.Add(timepicker1.Value.TimeOfDay);
            DateTime endDateTime = dateTimePicker1.Value.Date.Add(dateTimePicker1.Value.TimeOfDay);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@StartTime", startDateTime);
            cmd.Parameters.AddWithValue("@EndTime", endDateTime);
            cmd.Parameters.AddWithValue("@Status", guna2ComboBox1.Text);
            cmd.Parameters.AddWithValue("@Remarks", txtdiscription.Text);
            if (cmd.Connection.State != ConnectionState.Open)
            {
                cmd.Connection.Open();
            }
            if (id == 0)
            {
                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            else
            {
                cmd.ExecuteNonQuery();
            }
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            id = 0;
            Utilities.LogAudit(frmLogin.userId, (id == 0 ? "Add Task Details" : "Update Task Details"), "Task Details", id, (id == 0 ? "Added Task Details: " : "Updated Task Details: ") + txtdiscription.Text);
            this.Close();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
