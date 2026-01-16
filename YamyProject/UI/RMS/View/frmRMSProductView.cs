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
using YamyProject.RMS.Model;

namespace YamyProject.RMS.View
{
    public partial class frmRMSProductView : frmRMSamlpeView
    {
        public frmRMSProductView()
        {
            InitializeComponent();
        }

        private void frmRMSProductView_Load(object sender, EventArgs e)
        {
            GetData();
        }
        //private string GenerateNextEmployeeCode()
        //{

        //    using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_employee"))
        //    {
        //        if (reader.Read() && reader["lastCode"] != DBNull.Value)
        //            code = int.Parse(reader["lastCode"].ToString()) + 1;
        //        else
        //            code = 30001;
        //    }
        //    return code.ToString("D5");
        //}
        public void GetData()
        {
            //string qry = @"SELECT P.id, P.code, P.name, P.sales_price, C.name 
            //               FROM `tbl_items` P 
            //               INNER JOIN tbl_item_category C ON C.id = P.category_id where p.name like '%" + txtSearch.Text + "%' and p.code > 1300001";


            string qry = @"SELECT P.id, P.code, P.name, P.sales_price, C.id ,C.name 
                           FROM `tbl_items` P 
                           INNER JOIN tbl_item_category C ON C.id = P.category_id and P.posItem = 1";
            //string qry = @"SELECT P.id, P.code, P.name, P.sales_price, C.id , C.name AS cat
            //               FROM tbl_items p
            //               INNER JOIN tbl_item_category C ON C.id = tbl_items.category_id
            //               WHERE tbl_items.name LIKE '%" + txtSearch.Text + "%' ";


            ListBox lb = new ListBox();
            lb.Items.Add(dgvSno);
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvPrice);
            lb.Items.Add(dgvcatid);
            lb.Items.Add(dgvCat);

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
                frmRMSAddProduct frm = new frmRMSAddProduct();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                frm.CID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvcatid"].Value);

                RMSClass.blurbackground(frm);
                GetData();
            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete these products?",
                    "Confirmation",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {

                    int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    string qry1 = "Delete from tbl_items where id= " + id + "";
                    Hashtable ht = new Hashtable();
                    RMSClass.SQl(qry1, ht);
                    Utilities.LogAudit(frmLogin.userId, "Delete Product", "Product", id, "Deleted Product: " + guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
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

                    frmRMSAddProduct frm = new frmRMSAddProduct();
                    frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                    frm.CID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvcatid"].Value);

                    RMSClass.blurbackground(frm);
                    GetData();
                }
                if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvdel")
                {
                    DialogResult result = MessageBox.Show(
                        "Are you sure you want to delete these products?",
                        "Confirmation",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {

                        int id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvSno"].Value);
                        string qry1 = "Delete from tbl_items where id= " + id + "";
                        Hashtable ht = new Hashtable();
                        RMSClass.SQl(qry1, ht);
                        Utilities.LogAudit(frmLogin.userId, "Delete Product", "Product", id, "Deleted Product: " + guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                        MessageBox.Show("Delete Successfully");
                        GetData();
                    }
                }

            }
        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            RMSClass.blurbackground(new frmRMSAddProduct());
            GetData();

        }

        private void btnAdd_Click_2(object sender, EventArgs e)
        {
            RMSClass.blurbackground(new frmRMSAddProduct());
            GetData();
        }
    }
}
