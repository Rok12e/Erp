using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.RMS.Class;
using YamyProject.UI.CRM.Models;
using YamyProject.UI.Manufacturing.Models;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManBatchProduction : Form
    {
        public int mainid;
        public frmManBatchProduction()
        {
            InitializeComponent();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
          
        RMSClass.blurbackground3(new frmManAddWorkOrder());
            GetData();
        }

        public void GetData()
        {
            string qry = @" SELECT a.id, a.batchname, a.hours, a.amount, a.Description, IFNULL(b.status,'Pending') status FROM tbl_manufacturer_batch a LEFT JOIN (
                                SELECT t.batchId,t.status FROM tbl_manufacturer_task t WHERE t.id IN (SELECT MAX(id) FROM tbl_manufacturer_task GROUP BY BatchID)
                            ) b ON b.BatchID = a.id WHERE a.batchname LIKE '%" + guna2TextBox1.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvdate);
            lb.Items.Add(dgvamount);
            lb.Items.Add(dgvdescr);
            lb.Items.Add(dgvstatus);

            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void frmManBatchProduction_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                string status = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvstatus"].Value);
                if (status == "Done" || status == "Progress")
                {
                    MessageBox.Show("Cant edit this work already " + status);
                }
                else
                {
                    frmManAddWorkOrder frm = new frmManAddWorkOrder(Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value));
                    //    frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    //frm.txtname.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                    //frm.txtamount.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvamount"].Value);
                    //frm.txthourse.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvdate"].Value);
                    //frm.txtdiscription.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvdescr"].Value);
                    //frm.GetDataforupdatedetails(Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value));
                    //frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    //frm.GetDataforupdatedetails("1");
                    RMSClass.blurbackground3(frm);
                    GetData();
                }
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                string status = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvstatus"].Value);
                if (status == "Done" || status == "Progress")
                {
                    MessageBox.Show("Cant delete this work already " + status);
                }
                else
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to delete this batch?",
                        "Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                        DBClass.ExecuteNonQuery("Delete from  tbl_manufacturer_batch where id = @id",
                            DBClass.CreateParameter("id", id));
                        DBClass.ExecuteNonQuery("Delete from tbl_manufacturer_batchdetails where batchId = @id",
                            DBClass.CreateParameter("id", id));
                        Utilities.LogAudit(frmLogin.userId, "Delete Batch", "Batch", id, "Deleted Batch: " + guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                        MessageBox.Show(
                            "Delete Successfully",
                            "Information",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );

                        GetData();
                    }
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {



        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
