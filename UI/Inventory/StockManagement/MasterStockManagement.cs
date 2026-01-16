using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterStockManagement : Form
    {

        public MasterStockManagement()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmAddStockSettlement());
        }
        private void MasterStockManagement_Load(object sender, EventArgs e)
        {
            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            string query = "";
            if (cmbSelectionMethod.Text == "Default")
                query = @"SELECT ROW_NUMBER() OVER (ORDER BY tbl_item_stock_settlement.date) AS `SN`, 
                        tbl_item_stock_settlement.date AS Date, tbl_item_stock_settlement.id,
                        concat('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                        tbl_item_stock_settlement.code AS 'INV NO'
                        FROM tbl_item_stock_settlement
                        INNER JOIN tbl_transaction ON tbl_item_stock_settlement.id = tbl_transaction.transaction_id
                        WHERE tbl_item_stock_settlement.state = 0 ";
            else
                query = @"SELECT ROW_NUMBER() OVER (ORDER BY tbl_item_stock_settlement.date) AS `SN`, 
                        tbl_item_stock_settlement.date AS Date, 
                        tbl_item_stock_settlement.id,
                        tbl_item_stock_settlement.code AS 'INV NO', 
                        concat(ti.code,' - ',ti.name) as 'Item Name' ,ts.On_Hand As Qty,ts.price 
                        As 'Cost Price' , ts.new_on_hand  ,ts.minusamount ,ts.plusamount
                        FROM tbl_item_stock_settlement
                        INNER JOIN tbl_item_stock_settlement_details ts ON tbl_item_stock_settlement.id = ts.settle_id
                        inner join tbl_items ti on ts.item_id =ti.id
                        WHERE tbl_item_stock_settlement.state = 0";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (!chkDate.Checked)
                query += " and tbl_item_stock_settlement.date >= @dateFrom and tbl_item_stock_settlement.date <= @dateTo";
            if (cmbSelectionMethod.Text == "Default")
                query += " GROUP BY tbl_item_stock_settlement.id, tbl_item_stock_settlement.date, tbl_item_stock_settlement.code; ";

            else
                query += " GROUP BY tbl_item_stock_settlement.id, tbl_item_stock_settlement.date, tbl_item_stock_settlement.code,ti.code,ti.name,ts.on_hand,ts.price,ts.new_on_hand  ,ts.minusamount ,ts.plusamount; ";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvCustomer.DataSource = dt;
           dgvCustomer.Columns["id"].Visible = false;
            if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;

            if (dgvCustomer.Rows.Count > 0)
            {
                btnEdit.Enabled = UserPermissions.canEdit("Stock Management");
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void bindInvoiceItems()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.on_hand as Qty ,ts.price as 'Cost Price' , ts.new_on_hand  from 
                                                     tbl_item_stock_settlement_details ts inner join tbl_items ti on ts.item_id = ti.id where settle_id = @id",
                                                     DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            dgvItems.DataSource = dt;
            dgvItems.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmAddStockSettlement( int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmAddStockSettlement( int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
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
            if (dgvCustomer.Rows.Count == 0)
                return;
            //DBClass.ExecuteNonQuery("UPDATE tbl_items SET state = -1 WHERE id = @id ",
            //                              DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            //BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;
        }
    }
}
