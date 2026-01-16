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
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.RMS.Model;

namespace YamyProject.RMS.View
{
    public partial class frmRMSStaff : frmRMSamlpeView
    {
        public frmRMSStaff()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }
        int code;
        private void frmRMSStaff_Load(object sender, EventArgs e)
        {
            GetData();
            GenerateNextEmployeeCode();

        }
        private string GenerateNextEmployeeCode()
        {

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_employee"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 30001;
            }
            return code.ToString("D5");
        }
        public void GetData()
        {
            string qry = "SELECT Id,Code,name,phone,sRole FROM `tbl_employee` where name like '%" + txtSearch.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPhone);
            lb.Items.Add(dgvRole);

            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        //public override void btnAdd_Click(object sender, EventArgs e)
        //{
        //    //  frmRMSAddCategore frm = new frmRMSAddCategore();
        //    //frm.ShowDialog();
        //    RMSClass.blurbackground(new frmRMSAddStuff());
        //    GetData();
        //}

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
         
                frmRMSAddStaff frm = new frmRMSAddStaff();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                frm.txtcode.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                frm.txtname.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvname"].Value);
                frm.txtphone.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvPhone"].Value);
                frm.cbrole.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvRole"].Value);
                RMSClass.blurbackground(frm);
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this stuff?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {

                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    string qry1 = "Delete from tbl_employee where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId, "Delete Staff", "Staff", id, "Deleted Staff: " + guna2DataGridView1.CurrentRow.Cells["dgvname"].Value);
                    MessageBox.Show("Delete Successfully");
                    GetData();
                }
            }

        }

        private void guna2DataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
                {

                    frmRMSAddStaff frm = new frmRMSAddStaff();
                    frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    frm.txtcode.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvid"].Value);
                    frm.txtname.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvname"].Value);
                    frm.txtphone.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvPhone"].Value);
                    frm.cbrole.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvRole"].Value);
                    RMSClass.blurbackground(frm);
                    GetData();
                }
                if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to delete this stuff?",
                        "Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                        string qry1 = "Delete from tbl_employee where id= " + id + "";
                        Hashtable ht = new Hashtable();
                        RMSClass.SQl(qry1, ht);
                        Utilities.LogAudit(frmLogin.userId, "Delete Staff", "Staff", id, "Deleted Staff: " + guna2DataGridView1.CurrentRow.Cells["dgvname"].Value);
                        MessageBox.Show("Delete Successfully");
                        GetData();
                    }
                }

            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            RMSClass.blurbackground(new frmRMSAddStaff());
            GetData();
           
        }
    }
}
