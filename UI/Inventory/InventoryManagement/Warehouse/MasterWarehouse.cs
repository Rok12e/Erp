using DocumentFormat.OpenXml.Office2010.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterWarehouse : Form
    {
        private EventHandler wareHouseUpdatedHandler;
        private EventHandler itemUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;
        private EventHandler purchaseUpdatedHandler;

        public MasterWarehouse()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            wareHouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            itemUpdatedHandler = (sender, args) => BindItems();
            invoiceUpdatedHandler = purchaseUpdatedHandler=(sender, args) => BindItems();
            EventHub.wareHouse += wareHouseUpdatedHandler;
            EventHub.Item += itemUpdatedHandler;
            EventHub.SalesInv += invoiceUpdatedHandler;
            EventHub.PurchaseInv += purchaseUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterWarehouse_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.wareHouse -= wareHouseUpdatedHandler;
            EventHub.Item -= itemUpdatedHandler;
            EventHub.SalesInv -= invoiceUpdatedHandler;
            EventHub.PurchaseInv -= purchaseUpdatedHandler;


        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmViewWarehouse().ShowDialog();
        }
        private void MasterWarehouse_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateWarehouse(cmbWarehouse);
            BindItems();
        }
        public void BindItems()
        {
            if (cmbWarehouse.SelectedValue == null)
                return;
            //DataTable dt = DBClass.ExecuteDataTable(@"
            //                                        SELECT 
            //                                            i.id,
            //                                            CONCAT(i.code, ' - ', i.name) AS 'Item Name',
            //                                            i.barcode,
            //                                            i.cost_price AS 'Cost Price',
            //                                            i.sales_price AS 'Sales Price',
            //                                            ROUND(SUM(t.qty_in) - SUM(t.qty_out), 2) AS 'QTY',
            //                                            i.type
            //                                        FROM tbl_item_transaction t
            //                                        INNER JOIN tbl_items i ON t.item_id = i.id
            //                                        WHERE t.warehouse_id = @id
            //                                        GROUP BY i.id, i.code, i.name, i.barcode, i.cost_price, i.sales_price, i.type
            //                                        HAVING `QTY` <> 0
            //                                    ", DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT w.id, w.item_id,CONCAT(i.code ,' - ', i.name ) AS 'Item Name' ,i.barcode, i.cost_price AS 'Cost Price', i.sales_price AS 'Sales Price', w.qty AS 'QTY' ,
                                        i.TYPE FROM tbl_items_warehouse w INNER join tbl_items i ON w.item_id = i.id where w.warehouse_id = @id ",
                                        DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["Item Name"].MinimumWidth = 200;
            dgvCustomer.Columns["Item Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["type"].Width = 140;
            dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["item_id"].Visible = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindItems();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (cmbWarehouse.SelectedValue==null)
                return;
            new frmViewWarehouse(int.Parse(cmbWarehouse.SelectedValue.ToString())).ShowDialog();
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterWarehouseTransfer());
        }

        private void Lbheader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void newTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterWarehouseTransfer());
        }

        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewWarehouse().ShowDialog();
        }

        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (cmbWarehouse.SelectedValue == null)
                return;
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewWarehouse(int.Parse(cmbWarehouse.SelectedValue.ToString())));
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            if (dgvCustomer.SelectedRows.Count == 0)
                return;
            if (dgvCustomer.SelectedRows[0].Cells["item_id"].Value == null || dgvCustomer.SelectedRows[0].Cells["item_id"].Value.ToString() == "")
                return;

            int itemId = int.Parse(dgvCustomer.SelectedRows[0].Cells["item_id"].Value.ToString());
            int warehouseId = (cmbWarehouse.SelectedValue == null || string.IsNullOrWhiteSpace(cmbWarehouse.SelectedValue.ToString())) ? 0 : int.Parse(cmbWarehouse.SelectedValue.ToString());
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewWarehouseTransfer(0, itemId, warehouseId));
        }

        private void transferHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewWarehouseTransfer());
        }

        private void deleteCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbWarehouse.SelectedValue == null)
                return;

            object objRecord = DBClass.ExecuteScalar(@"SELECT 
                                                      CASE 
                                                        WHEN EXISTS (SELECT 1 FROM tbl_items_warehouse WHERE warehouse_id = @warehouse_id)
                                                          OR EXISTS (SELECT 1 FROM tbl_item_transaction WHERE warehouse_id = @warehouse_id)
                                                        THEN 1
                                                        ELSE 0
                                                      END AS exists_flag;",
                DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue.ToString()));
            int oldRecord = (objRecord != null && objRecord != DBNull.Value) ? int.Parse(objRecord.ToString()) : 0;

            if (oldRecord > 0)
            {
                MessageBox.Show("Cannot delete this warehouse because it has associated transactions. Please transfer or remove the transactions first.");
                return;
            }
            else
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this warehouse?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    DBClass.ExecuteNonQuery("DELETE FROM tbl_warehouse WHERE id = @id",
                        DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
                    Utilities.LogAudit(frmLogin.userId, "Delete Warehouse", "Warehouse", int.Parse(cmbWarehouse.SelectedValue.ToString()), "Deleted Warehouse: " + cmbWarehouse.Text);
                    BindCombos.PopulateWarehouse(cmbWarehouse);
                    MessageBox.Show("Warehouse deleted successfully.");
                }

            }
        }
    }
}
