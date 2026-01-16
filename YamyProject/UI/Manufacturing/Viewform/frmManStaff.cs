using System;
using System.Collections;
using System.Windows.Forms;
using YamyProject.RMS.Class;
using YamyProject.UI.CRM.Models;
using YamyProject.UI.Manufacturing.Models;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManStaff : Form
    {
        public int mainid;
        public int activ = 0;
        public frmManStaff()
        {
            InitializeComponent();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddNewStaff());
            GetData();
        }

        //public void GetData()
        //{ 
        //    string qry = @" SELECT id,batchname,hours,amount,Description FROM tbl_manufacturer_batch 
        //                 WHERE
        //                 batchname LIKE '%" + guna2TextBox1.Text + "%'";
        //    ListBox lb = new ListBox();
        //    lb.Items.Add(dgvSno);
        //    lb.Items.Add(dgvName);
        //    lb.Items.Add(dgvdate);
        //    lb.Items.Add(dgvamount);
  
        //    RMSClass.loadData(qry, guna2DataGridView1, lb);
        //}

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void frmManStaff_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmManAddNewStaff frm = new frmManAddNewStaff();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                frm.txtcode.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvcode"].Value);
                frm.txtname.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.CbDpartment.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvdate"].Value);
                frm.cbMachin.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvamount"].Value);
                frm.guna2TextBox1.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvphone"].Value);
                frm.guna2TextBox3.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvEmail"].Value);
                frm.guna2TextBox6.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvaddress"].Value);
                activ = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvactive"].Value);
                if (activ == 0)
                {
                    frm.guna2ToggleSwitch1.Checked = true;
                }
                else
                    frm.guna2ToggleSwitch1.Checked = true;
                RMSClass.blurbackground3(frm);
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
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
                    string qry1 = "Delete from  tbl_manufacturer_batch where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId, "Delete Staff", "Staff", id, "Deleted Staff: " + guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                    MessageBox.Show("Delete Successfully");
                    GetData();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
        public void GetData()
        {
            string qry = @"SELECT 
                            e.id,
                            e.code,
                            e.name AS employee_name,
                            d.name AS department_name,
                            p.name AS position_name,
                            e.email,
                            e.phone,
                            e.address,
                            e.active
                        
                        FROM 
                            tbl_employee e
                        LEFT JOIN 
                            tbl_departments d ON e.department_id = d.id
                        LEFT JOIN 
                            tbl_position p ON e.position_id = p.id
                       WHERE e.name LIKE '%" + guna2TextBox1.Text + "%'";

            System.Windows.Forms.ListBox lb = new System.Windows.Forms.ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvcode);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvdate);
            lb.Items.Add(dgvamount);
            lb.Items.Add(dgvEmail);
            lb.Items.Add(dgvphone);
            lb.Items.Add(dgvaddress);
            lb.Items.Add(dgvactive);
            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            RMSClass.blurbackground3(new frmManAddPositionStaff());
            GetData();
        }
    }
    
}
