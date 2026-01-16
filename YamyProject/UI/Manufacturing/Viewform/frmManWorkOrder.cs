using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.RMS.Class;
using YamyProject.UI.Manufacturing.Models;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManWorkOrder : Form
    {

        public frmManWorkOrder()
        {
            InitializeComponent();
        }

        string status = "";

        private void frmManWorkOrder_Load(object sender, EventArgs e)
        {

            DataTable dt = DBClass.ExecuteDataTable("select id,batchname name from tbl_manufacturer_batch");
            cmbWorkOrder.ValueMember = "id";
            cmbWorkOrder.DisplayMember = "name";
            cmbWorkOrder.DataSource = dt;

            cmbWorkOrder.Text = "Search By Batch  Name ....";
            cmbWorkOrder.SelectedIndex = -1;

            Details();
        }

        private void Details()
        {
            dgvItems.Rows.Clear();
            string query = "";
            if (cmbWorkOrder.SelectedIndex != -1)
            {
                query = @" SELECT a.id, a.batchname, a.date, a.hours, a.amount, a.Description, IFNULL(b.status,'Pending') status,a.warehouse_id FROM tbl_manufacturer_batch a LEFT JOIN (
                                SELECT t.batchId,t.status FROM tbl_manufacturer_task t WHERE t.id IN (SELECT MAX(id) FROM tbl_manufacturer_task GROUP BY BatchID)
                            ) b ON b.BatchID = a.id WHERE a.batchname LIKE '%" + cmbWorkOrder.Text + "%'";
            }
            else
            {
                query = @" SELECT a.id, a.batchname, a.date, a.hours, a.amount, a.Description, IFNULL(b.status,'Pending') status,a.warehouse_id FROM tbl_manufacturer_batch a LEFT JOIN (
                                SELECT t.batchId,t.status FROM tbl_manufacturer_task t WHERE t.id IN (SELECT MAX(id) FROM tbl_manufacturer_task GROUP BY BatchID)
                            ) b ON b.BatchID = a.id";
            }

            using (MySqlDataReader reader = DBClass.ExecuteReader(query))
            {
                int count = 1;
                while (reader.Read())
                {
                    status = reader["status"].ToString();

                    dgvItems.Rows.Add(count.ToString(),
                        reader["id"],
                        reader["batchname"],
                        reader["date"],
                        reader["hours"],
                        reader["amount"],
                        reader["Description"],
                        status
                    );

                    count++;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Details();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        private void btAddWork_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddWorkOrder());
        }

        private void dgvItems_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 0 && dgvItems.CurrentRow != null)
            {
                int id = Convert.ToInt32(dgvItems.CurrentRow.Cells["dgvid"].Value);
                frmManWorkOrderDetails frm = new frmManWorkOrderDetails(id);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a work order to view details.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
