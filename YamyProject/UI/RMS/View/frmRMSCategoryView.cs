using DocumentFormat.OpenXml.Office2010.Excel;
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
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.RMS.Model;

namespace YamyProject.RMS.View
{
    public partial class frmRMSCategoryView : frmRMSamlpeView
    {
        public frmRMSCategoryView()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

        }
        public void GetData()
        {
            string qry = "SELECT * FROM `tbl_item_category` where name like '%" + txtSearch.Text + "%'";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);

            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void frmRMSCategoryView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            //  frmRMSAddCategore frm = new frmRMSAddCategore();
            //frm.ShowDialog();
            RMSClass.blurbackground(new frmRMSAddCategore());
            GetData();
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                frmRMSAddCategore frm = new frmRMSAddCategore();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                frm.txtname.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvname"].Value);
                RMSClass.blurbackground(frm);
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this category?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    string qry1 = "Delete from tbl_item_category where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId, "Delete Category", "Category", id, "Deleted Category: " + guna2DataGridView1.CurrentRow.Cells["dgvname"].Value);
                    MessageBox.Show("Delete Successfully");
                    GetData();
                }
            }

        }
    }
}
