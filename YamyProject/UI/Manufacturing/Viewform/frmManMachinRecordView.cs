using System;
using System.Windows.Forms;
using YamyProject.RMS.Class;
using System.Collections;

namespace YamyProject.UI.Manufacturing.Viewform
{
    public partial class frmManMachinRecordView : Form
    {

        public int manid = 1;
        public frmManMachinRecordView()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

            frmViewFixedAssets frm = new frmViewFixedAssets();
            frm.pnlmanifacuter.Visible = true;
            RMSClass.blurbackground3(frm);
            GetData();
        }


        public void GetData()
        {
            string qry = @" SELECT id,name FROM  tbl_fixed_assets 
                         WHERE
                         name LIKE '%" + guna2TextBox1.Text + "%' and manufacture = 1";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvName);
      
            RMSClass.loadData(qry, guna2DataGridView1, lb);
        }

        private void frmManMachinRecordView_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit")
            {
                if (guna2DataGridView1.Rows.Count == 0)
                    return;
            
                int sno = int.Parse(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value.ToString());

                frmViewFixedAssets frm = new frmViewFixedAssets(sno);
                frm.pnlmanifacuter.Visible = true;
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
                    string qry1 = "Delete from  tbl_fixed_assets where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId, "Delete Fixed Assets", "Fixed Assets", id, "Deleted Fixed Assets: " + guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                    CommonInsert.DeleteTransactionEntry(id, "Fixed Assets");
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            frmViewFixedAssets frm = new frmViewFixedAssets();
            frm.pnlmanifacuter.Visible = true;
            RMSClass.blurbackground3(frm);
            GetData();
        }
    }
}
