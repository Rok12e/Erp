using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterItemCategory : Form
    {
        private EventHandler itemCategoryUpdatedHandler;

        public MasterItemCategory()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            itemCategoryUpdatedHandler = (sender, args) => BindItems();
            headerUC1.FormText = this.Text;
            EventHub.ItemCategory += itemCategoryUpdatedHandler;
        }
        private void MasterItemCategory_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.ItemCategory -= itemCategoryUpdatedHandler;
            //EventHub.SalesInv -= invoiceUpdatedHandler;
            //EventHub.PurchaseInv -= purchaseUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmViewCategory().ShowDialog();
        }
        private void MasterItemCategory_Load(object sender, EventArgs e)
        {
            BindItems();
        }
        public void BindItems()
        {
            dgvCustomer.DataSource = DBClass.ExecuteDataTable(@"select id,Concat( code , ' - ',name) as 'Category Name' from tbl_item_category");
            dgvCustomer.Columns["Category name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvCustomer.Columns["Category Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["id"].Visible = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            //cmbCategory.Enabled = !chkAllCategory.Checked;
            //cmbType.Enabled = !chkAllType.Checked;
            //BindItems();
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindItems();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            new frmViewCategory( int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
          
                new frmViewCategory(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
            if (dgvCustomer.Rows.Count == 0)
                return;
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            object result = DBClass.ExecuteScalar(@"SELECT COUNT(1) FROM tbl_items 
                  WHERE category_id = @id", DBClass.CreateParameter("id", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            int recordCount = 0;
            if (result != null && result != DBNull.Value)
                recordCount = Convert.ToInt32(result);
            if (recordCount > 0)
            {
                MessageBox.Show("Already used");
                return;
            }
            DBClass.ExecuteNonQuery("DELETE FROM tbl_item_category WHERE id=@id", DBClass.CreateParameter("id", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
            BindItems();
            MessageBox.Show("Deleted");
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
