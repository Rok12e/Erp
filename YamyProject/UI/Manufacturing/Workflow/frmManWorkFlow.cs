using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.UI.Manufacturing.Models;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManWorkFlow : Form
    {
        public frmManWorkFlow()
        {
            InitializeComponent();
        }

        string status = "";
        private DataView _dataView;
        int idtask = 0;
        private void frmManWorkFlow_Load(object sender, EventArgs e)
        {

            DataTable dt = DBClass.ExecuteDataTable("select id,batchname name from tbl_manufacturer_batch");
            cmbWorkOrder.ValueMember = "id";
            cmbWorkOrder.DisplayMember = "name";
            cmbWorkOrder.DataSource = dt;

            cmbWorkOrder.Text = "Search By Batch  Name ....";
            cmbWorkOrder.SelectedIndex = -1;

            //Details();
            BindProcesses();
            BindWorkFlow();
            loaddataindatagraidview();
        }

        private void BindWorkFlow()
        {
            if (dgvCustomer.Rows.Count>0) {
                var row = dgvCustomer.SelectedRows[0].Cells;
                lblCode.Text = "0" + "0" + row["sn"].Value?.ToString().Split('-')[0];
                string nameValue = row["Batch Name"].Value?.ToString();
                var splitName = nameValue.Split('-');
                lblName.Text = splitName.Length > 1 ? splitName[1].Trim() : nameValue;
                lblstatus.Text = row["Process Status"].Value?.ToString();
                label5.Text = row["StartTime"].Value is DateTime dt ? dt.ToString("yyyy-MM-dd") : "-";
                LbMachinName.Text = row["Machine Name"].Value?.ToString() ?? "-";
            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //Details();
        }


        public void BindProcesses()
        {
            string query = @"
                                SELECT 
                                    p.id AS 'sn',
                                    b.batchname AS 'Batch Name',
                                    p.Status AS 'Process Status',
                                    p.StartTime,
                                    A.Name AS 'Machine Name'
                                FROM tbl_manufacturer_task p
                                LEFT JOIN tbl_manufacturer_batch b ON p.BatchID = b.id
                                LEFT JOIN tbl_fixed_assets A on p.MachineID = A.id
                            ";

            // لو عايز تضيف فلترة على Status مثلاً:   
            DataTable dt = DBClass.ExecuteDataTable(query);
            _dataView = dt.DefaultView;
            dgvCustomer.DataSource = _dataView;

            // تنسيقات الجدول
            dgvCustomer.Columns["sn"].Width = 80;
            dgvCustomer.Columns["Batch Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["Process Status"].Width = 100;

            // ترجمة العناوين إن كنت تستخدم اللغة العربية
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddWorkOrder());
        }

        private void dgvItems_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            var row = dgvItems.SelectedRows[0].Cells;

            frmManUpdateDetails frm = new frmManUpdateDetails();
            frm.label6.Text = row["employeeID"].Value?.ToString().Split('-')[0];
            frm.label9.Text = row["dgvempname"].Value?.ToString();
            frm.id = Convert.ToInt32(row["Id"].Value);

            DateTime temp;
            if (DateTime.TryParse(row["dgvstarttime"].Value?.ToString(), out temp))
                frm.timepicker1.Value = temp;
            else
                frm.timepicker1.Value = DateTime.Parse("00:00:00");

            if (DateTime.TryParse(row["dgvEndTime"].Value?.ToString(), out temp))
                frm.dateTimePicker1.Value = temp;
            else
                frm.dateTimePicker1.Value = DateTime.Parse("00:00:00");

            frm.guna2ComboBox1.Text = row["dgvStatus"].Value?.ToString();
            frm.txtdiscription.Text = row["dgvRemark"].Value?.ToString();
            RMSClass.blurbackground3(frm);
            loaddataindatagraidview();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindWorkFlow();
            //Details();
            loaddataindatagraidview();
        }

        private void dgvItems_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            Rectangle headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                grid.RowHeadersWidth,
                e.RowBounds.Height);

            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }


        private void loaddataindatagraidview()
        {
            if (dgvCustomer.Rows.Count > 0)
            {
                var row = dgvCustomer.SelectedRows[0].Cells;
                idtask = Convert.ToInt32(row["sn"].Value?.ToString().Split('-')[0]);
                dgvItems.Rows.Clear();
                string query = "";
                query = @" SELECT t.*,d.name depNAME, d.Department,CONCAT(e.code,'-',e.name) AS EmpName
                            FROM tbl_manufacturer_task_details t
                            LEFT JOIN tbl_departments d ON t.DepartmentID=d.id
                            LEFT JOIN tbl_employee e ON t.EmployeeID = e.id
                            WHERE taskid = " + idtask + "";
                using (MySqlDataReader reader = DBClass.ExecuteReader(query))
                {
                    int count = 1;
                    while (reader.Read())
                    {
                        dgvItems.Rows.Add(count.ToString(),
                            reader["Id"],
                            reader["TaskID"],
                            reader["DepartmentID"],
                            reader["EmployeeID"],
                            reader["EmpName"],
                            reader["depName"],
                            reader["StartTime"],
                            reader["EndTime"],
                            reader["Status"],
                            reader["Remarks"]
                        );

                        count++;
                    }
                }
            }
        }
    }

}
