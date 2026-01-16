using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterWarehouseTransfer : Form
    {
        DataView dvFrom, dvTo;
        System.Collections.Generic.List<string[]> lstOfItems = new System.Collections.Generic.List<string[]>();

        private EventHandler wareHouseUpdatedHandler;
        private EventHandler itemUpdatedHandler;

        public MasterWarehouseTransfer()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            wareHouseUpdatedHandler = (sender, args) => BindWarehouses();
            itemUpdatedHandler = (sender, args) => BindWarehousesItems();
            EventHub.wareHouse += wareHouseUpdatedHandler;
            EventHub.Item += itemUpdatedHandler;
            headerUC1.FormText = "Warehouse - Transfer Items Through Warehouse";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MasterWarehouseTransfer_Load(object sender, EventArgs e)
        {
            BindWarehouses();
        }

        private void BindWarehouses()
        {
            BindCombos.PopulateWarehouse(cmbWareFrom);
            BindCombos.PopulateWarehouse(cmbWareTo);
        }

        private void BindWarehousesItems()
        {
            if (cmbWareFrom.SelectedValue != null)
            {
                bindWarehouseFrom();
            }
            if (cmbWareTo.SelectedValue != null)
            {
                bindWarehouseTo();
            }
        }

        private void cmbWareFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWareFrom.SelectedValue == null)
                return;
            bindWarehouseFrom();
        }

        private void bindWarehouseFrom()
        {
            dgvFrom.DataSource = null;
            //string query = @"SELECT i.id, CONCAT(i.code, ' - ', i.name) AS 'Item Name', w.qty AS 'QTY' 
            //                 FROM tbl_items_warehouse w 
            //                 INNER JOIN tbl_items i ON w.item_id = i.id 
            //                 WHERE i.active=0 AND w.warehouse_id = @id";
            string query = @"SELECT 
                        i.id,
                        CONCAT(i.code, ' - ', i.name) AS 'Item Name',
                        IFNULL(SUM(t.qty_in - t.qty_out), 0) AS 'QTY'
                    FROM tbl_item_transaction t
                    INNER JOIN tbl_items i ON t.item_id = i.id
                    WHERE i.active = 0
                      AND t.warehouse_id = @id
                    GROUP BY i.id, i.code, i.name
                    HAVING `QTY` > 0";
            DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("id", cmbWareFrom.SelectedValue.ToString()));
            if (dt == null || dt.Rows.Count == 0)
                return;

            dvFrom = dt.DefaultView;
            dgvFrom.DataSource = dvFrom;
            dgvFrom.Columns["id"].Visible = false;
            dgvFrom.Columns["Item Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // Format "qty" column
            if (dgvFrom.Columns.Contains("qty"))
            {
                dgvFrom.Columns["qty"].DefaultCellStyle.Format = "N2"; // 2 decimal places
                dgvFrom.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvFrom);
        }

        private void cmbWareTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbWareTo.SelectedValue == null)
                return;
            bindWarehouseTo();
        }

        private void bindWarehouseTo()
        {
            dgvTo.DataSource = null;
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT i.id,CONCAT(i.code ,' - ', i.name ) AS 'Item Name' , w.qty AS 'QTY' 
                                                     FROM tbl_items_warehouse w INNER join tbl_items i ON w.item_id = i.id where i.active=0 and w.warehouse_id = @id",
                                                                           DBClass.CreateParameter("id", cmbWareTo.SelectedValue.ToString()));
            if (dt == null || dt.Rows.Count == 0)
                return;
            dvTo = dt.DefaultView;
            dgvTo.DataSource = dvTo;
            dgvTo.Columns["id"].Visible = false;
            dgvTo.Columns["Item Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // Format "qty" column
            if (dgvTo.Columns.Contains("qty"))
            {
                dgvTo.Columns["qty"].DefaultCellStyle.Format = "N2"; // 2 decimal places
                dgvTo.Columns["qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvTo);
        }

        private void btnFromTo_Click(object sender, EventArgs e)
        {
            TransferItem(true);
        }

        private void btnToFrom_Click(object sender, EventArgs e)
        {
            TransferItem(false);
        }

        private void TransferItem(bool isFromTo)
        {
            var dgvSource = isFromTo ? dgvFrom : dgvTo;
            var cmbSource = isFromTo ? cmbWareFrom : cmbWareTo;
            var cmbTarget = isFromTo ? cmbWareTo : cmbWareFrom;

            if (dgvSource.Rows.Count == 0)
            {
                MessageBox.Show("No Items To Transfer From This Warehouse.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtQty.Text) || !decimal.TryParse(txtQty.Text, out decimal qty) || qty <= 0)
            {
                MessageBox.Show("Enter Valid QTY");
                txtQty.Focus();
                return;
            }
            if (cmbSource.Text == cmbTarget.Text)
            {
                MessageBox.Show("Select Different Warehouses For Transfer");
                return;
            }

            var selectedRow = dgvSource.SelectedRows[0];
            int itemId = Convert.ToInt32(selectedRow.Cells["id"].Value);
            string itemName = selectedRow.Cells["Item Name"].Value.ToString();
            decimal availableQty = Convert.ToDecimal(selectedRow.Cells["qty"].Value);

            if (availableQty < qty)
            {
                MessageBox.Show("Qty Entered Is Greater Than Available");
                txtQty.Focus();
                return;
            }

            int sourceWarehouseId = Convert.ToInt32(cmbSource.SelectedValue);
            int targetWarehouseId = Convert.ToInt32(cmbTarget.SelectedValue);
            int invId = 0;

            // Insert/update destination warehouse stock
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items_warehouse WHERE warehouse_id = @warehouse_id AND item_id = @item_id",
                DBClass.CreateParameter("warehouse_id", targetWarehouseId.ToString()),
                DBClass.CreateParameter("item_id", itemId.ToString())))
            {
                if (reader.Read())
                {
                    invId = Convert.ToInt32(reader["id"]);
                    DBClass.ExecuteNonQuery("UPDATE tbl_items_warehouse SET qty = qty + @qty WHERE id = @id",
                        DBClass.CreateParameter("qty", qty),
                        DBClass.CreateParameter("id", invId));
                    Utilities.LogAudit(frmLogin.userId, "Update Warehouse Item", "Warehouse Item", invId, $"Updated Qty: {qty} for Item: {itemName}");
                }
                else
                {
                    invId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_items_warehouse (warehouse_id, item_id, qty)
                VALUES (@warehouse_id, @item_id, @qty); SELECT LAST_INSERT_ID();",
                        DBClass.CreateParameter("warehouse_id", targetWarehouseId.ToString()),
                        DBClass.CreateParameter("item_id", itemId.ToString()),
                        DBClass.CreateParameter("qty", qty)));
                    Utilities.LogAudit(frmLogin.userId, "Insert Warehouse Item", "Warehouse Item", itemId, $"Inserted Qty: {qty} for Item: {itemName}");
                }
            }

            // Reduce from source warehouse
            DBClass.ExecuteNonQuery("UPDATE tbl_items_warehouse SET qty = qty - @qty WHERE warehouse_id = @warehouse_id AND item_id = @item_id",
                DBClass.CreateParameter("qty", qty),
                DBClass.CreateParameter("warehouse_id", sourceWarehouseId.ToString()),
                DBClass.CreateParameter("item_id", itemId.ToString()));

            // Log transfer
            DBClass.ExecuteNonQuery(@"INSERT INTO tbl_item_warehouse_transaction 
                (`date`, `warehouse_from`, `warehouse_to`, `item_id`, `qty`, `description`, `created_by`, `created_date`) 
                VALUES (@date, @warehouse_from, @warehouse_to, @item_id, @qty, @description, @created_by, @created_date)",
                DBClass.CreateParameter("@date", DateTime.Now),
                DBClass.CreateParameter("@warehouse_from", sourceWarehouseId),
                DBClass.CreateParameter("@warehouse_to", targetWarehouseId),
                DBClass.CreateParameter("@item_id", itemId),
                DBClass.CreateParameter("@qty", qty),
                DBClass.CreateParameter("@description", "Item Transfer"),
                DBClass.CreateParameter("@created_by", frmLogin.userId),
                DBClass.CreateParameter("@created_date", DateTime.Now));

            // Get cost/sales prices
            decimal costPrice = 0, salesPrice = 0;

            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT cost_price, sales_price FROM tbl_item_transaction WHERE warehouse_id = @warehouse_id AND item_id = @item_id ORDER BY date DESC LIMIT 1",
                DBClass.CreateParameter("@warehouse_id", sourceWarehouseId),
                DBClass.CreateParameter("@item_id", itemId)))
            {
                if (reader.Read())
                {
                    if (reader["cost_price"] != DBNull.Value)
                        costPrice = Convert.ToDecimal(reader["cost_price"]);
                    if (reader["sales_price"] != DBNull.Value)
                        salesPrice = Convert.ToDecimal(reader["sales_price"]);
                }
            }

            if (costPrice == 0 || salesPrice == 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(
                    "SELECT cost_price, sales_price FROM tbl_items WHERE id = @item_id",
                    DBClass.CreateParameter("@item_id", itemId)))
                {
                    if (reader.Read())
                    {
                        if (costPrice == 0 && reader["cost_price"] != DBNull.Value)
                            costPrice = Convert.ToDecimal(reader["cost_price"]);
                        if (salesPrice == 0 && reader["sales_price"] != DBNull.Value)
                            salesPrice = Convert.ToDecimal(reader["sales_price"]);
                    }
                }
            }

            // Add OUT transaction from source warehouse
            addItemTransaction(DateTime.Now, invId, itemId, costPrice, salesPrice, 0, qty,
                $"Transferred to warehouse {cmbTarget.Text} | {itemName}", sourceWarehouseId);

            // Add IN transaction to target warehouse
            addItemTransaction(DateTime.Now, invId, itemId, costPrice, salesPrice, qty, 0,
                $"Received from warehouse {cmbSource.Text} | {itemName}", targetWarehouseId);

            // Refresh views
            bindWarehouseTo();
            bindWarehouseFrom();

            Utilities.LogAudit(frmLogin.userId, "Transfer Warehouse Item", "Warehouse Item", itemId, $"Transferred Qty: {qty} for Item: {itemName}");

            EventHub.RefreshItem();
        }

        private void addItemTransaction(DateTime date, int invId, int itemId, decimal cost_price, decimal price, decimal qtyIn, decimal qtyOut, string description, int warehouseId)
        {
            CommonInsert.InsertItemTransaction(date, "Warehouse Transfer", invId.ToString(), itemId.ToString(),
                    cost_price.ToString("N2"), qtyIn.ToString("N2"), price.ToString("N2"), qtyOut.ToString("N2"), "0",
                    "Warehouse Transfer No. " + invId + description, warehouseId.ToString());
        }

        private void txtSearchFrom_TextChanged(object sender, EventArgs e)
        {
            dvFrom.RowFilter = "[Item Name] like '%" + txtSearchFrom.Text + "%'";
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewWarehouseTransfer());
        }

        private void MasterWarehouseTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.wareHouse -= wareHouseUpdatedHandler;
            EventHub.Item -= itemUpdatedHandler;

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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton20_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {

        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtSearchTo_TextChanged(object sender, EventArgs e)
        {
            dvTo.RowFilter = "[Item Name] like '%" + txtSearchTo.Text + "%'";
        }

        //private void btnFromTo_Click(object sender, EventArgs e)
        //{
        //    if (dgvFrom.Rows.Count == 0)
        //    {
        //        MessageBox.Show("No Items To Transfer From This Warehouse.");
        //        return;
        //    }
        //    if (txtQty.Text == "" || decimal.Parse(txtQty.Text) == 0)
        //    {
        //        MessageBox.Show("Enter Valid QTY");
        //        txtQty.Focus();
        //        return;
        //    }
        //    if (cmbWareFrom.Text == cmbWareTo.Text)
        //    {
        //        MessageBox.Show("Select Warehouse To Transfer First");
        //        return;
        //    }
        //    if (decimal.Parse(dgvFrom.SelectedRows[0].Cells["qty"].Value.ToString()) < decimal.Parse(txtQty.Text))
        //    {
        //        MessageBox.Show("Qty Entered Is Greater Than Found, Enter Valid QTY");
        //        txtQty.Focus();
        //        return;
        //    }
        //    int invId = 0; // Assuming invId is not used in this context, set to 0 or modify as needed
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items_warehouse where warehouse_id = @warehouse_id and item_id = @id",
        //                                    DBClass.CreateParameter("warehouse_id", cmbWareTo.SelectedValue.ToString()),
        //                                    DBClass.CreateParameter("id", dgvFrom.SelectedRows[0].Cells["id"].Value.ToString())))
        //        if (reader.Read())
        //        {
        //            invId = int.Parse(reader["id"].ToString());
        //            DBClass.ExecuteNonQuery("update tbl_items_warehouse set qty = qty+@qty where id = @id",
        //                DBClass.CreateParameter("qty", txtQty.Text),
        //                DBClass.CreateParameter("id", reader["id"].ToString()));
        //            Utilities.LogAudit(frmLogin.userId, "Update Warehouse Item", "Warehouse Item", int.Parse(reader["id"].ToString()), "Updated Qty: " + txtQty.Text + " for Item: " + dgvFrom.SelectedRows[0].Cells["Item Name"].Value);
        //        }
        //        else
        //        {
        //            invId = int.Parse(DBClass.ExecuteScalar("insert into tbl_items_warehouse(warehouse_id,item_id,qty) values (@warehouse_id,@item_id,@qty);SELECT LAST_INSERT_ID();",
        //                DBClass.CreateParameter("warehouse_id", cmbWareTo.SelectedValue.ToString()),
        //                DBClass.CreateParameter("item_id", dgvFrom.SelectedRows[0].Cells["id"].Value.ToString()),
        //                DBClass.CreateParameter("qty", txtQty.Text)).ToString());

        //            Utilities.LogAudit(frmLogin.userId, "Insert Warehouse Item", "Warehouse Item", int.Parse(dgvFrom.SelectedRows[0].Cells["id"].Value.ToString()), "Inserted Qty: " + txtQty.Text + " for Item: " + dgvFrom.SelectedRows[0].Cells["Item Name"].Value);
        //        }

        //    DBClass.ExecuteNonQuery("update tbl_items_warehouse set qty = qty-@qty where warehouse_id = @warehouse_id and item_id = @item_id",
        //             DBClass.CreateParameter("qty", txtQty.Text),
        //             DBClass.CreateParameter("warehouse_id", cmbWareFrom.SelectedValue.ToString()),
        //             DBClass.CreateParameter("item_id", dgvFrom.SelectedRows[0].Cells["id"].Value.ToString()));
        //    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_item_warehouse_transaction (`date`, `warehouse_from`, `warehouse_to`, `item_id`, `qty`, `description`, `created_by`, `created_date`) 
        //                                VALUES (@date, @warehouse_from, @warehouse_to, @item_id, @qty, @description, @created_by, @created_date)",
        //                              DBClass.CreateParameter("@date", DateTime.Now.Date),
        //                              DBClass.CreateParameter("@warehouse_from", cmbWareFrom.SelectedValue.ToString()),
        //                              DBClass.CreateParameter("@warehouse_to", cmbWareTo.SelectedValue.ToString()),
        //                              DBClass.CreateParameter("@item_id", dgvFrom.SelectedRows[0].Cells["id"].Value.ToString()),
        //                              DBClass.CreateParameter("@qty", txtQty.Text),
        //                              DBClass.CreateParameter("@description", "Item Transfer:" + invId),
        //                              DBClass.CreateParameter("@created_by", frmLogin.userId),
        //                              DBClass.CreateParameter("@created_date", DateTime.Now.Date));


        //    // Common variables
        //    DateTime now = DateTime.Now;
        //    int itemId = Convert.ToInt32(dgvFrom.SelectedRows[0].Cells["id"].Value); // or dgvTo for btnToFrom_Click
        //    int wareFrom = Convert.ToInt32(cmbWareFrom.SelectedValue);
        //    int wareTo = Convert.ToInt32(cmbWareTo.SelectedValue);
        //    decimal qty = decimal.Parse(txtQty.Text);
        //    string itemName = dgvFrom.SelectedRows[0].Cells["Item Name"].Value.ToString(); // or dgvTo
        //    decimal costPrice = 0; // Optional: you can fetch from tbl_items or warehouse stock
        //    decimal salesPrice = 0;

        //    // Fetch cost_price and sales_price from latest item transaction
        //    using (MySqlDataReader reader = DBClass.ExecuteReader(
        //        "SELECT cost_price, sales_price FROM tbl_item_transaction WHERE warehouse_id = @warehouse_id AND item_id = @item_id ORDER BY date DESC LIMIT 1",
        //        DBClass.CreateParameter("@warehouse_id", wareFrom.ToString()),
        //        DBClass.CreateParameter("@item_id", itemId.ToString())))
        //    {
        //        if (reader.Read())
        //        {
        //            if (reader["cost_price"] != DBNull.Value)
        //                costPrice = Convert.ToDecimal(reader["cost_price"]);
        //            if (reader["sales_price"] != DBNull.Value)
        //                salesPrice = Convert.ToDecimal(reader["sales_price"]);
        //        }
        //    }

        //    // Fallback to tbl_items if no transaction found
        //    if (costPrice == 0 || salesPrice == 0)
        //    {
        //        using (MySqlDataReader reader = DBClass.ExecuteReader(
        //            "SELECT cost_price, sales_price FROM tbl_items WHERE id = @item_id",
        //            DBClass.CreateParameter("@item_id", itemId.ToString())))
        //        {
        //            if (reader.Read())
        //            {
        //                if (reader["cost_price"] != DBNull.Value && costPrice == 0)
        //                    costPrice = Convert.ToDecimal(reader["cost_price"]);
        //                if (reader["sales_price"] != DBNull.Value && salesPrice == 0)
        //                    salesPrice = Convert.ToDecimal(reader["sales_price"]);
        //            }
        //        }
        //    }

        //    // 1. OUT from Source Warehouse
        //    addItemTransaction(
        //        now,
        //        invId, // invId = 0 or warehouse transaction ID if needed
        //        itemId,
        //        costPrice,
        //        salesPrice,
        //        0, // qty_in
        //        qty, // qty_out
        //        $"Transferred to warehouse {cmbWareTo.Text} | {itemName}",
        //        wareFrom
        //    );

        //    // 2. IN to Destination Warehouse
        //    addItemTransaction(
        //        now,
        //        invId,
        //        itemId,
        //        costPrice,
        //        salesPrice,
        //        qty,
        //        0,
        //        $"Received from warehouse {cmbWareFrom.Text} | {itemName}",
        //        wareTo
        //    );

        //    bindWarehouseTo();
        //    bindWarehouseFrom();
        //    Utilities.LogAudit(frmLogin.userId, "Transfer Warehouse Item", "Warehouse Item", int.Parse(dgvFrom.SelectedRows[0].Cells["id"].Value.ToString()), "Transferred Qty: " + txtQty.Text + " for Item: " + dgvFrom.SelectedRows[0].Cells["Item Name"].Value);
        //    //EventHub.RefreshWarehouse();
        //    EventHub.RefreshItem();
        //}

        //private void btnToFrom_Click(object sender, EventArgs e)
        //{
        //    if (dgvTo.Rows.Count == 0)
        //    {
        //        MessageBox.Show("No Items To Transfer From This Warehouse.");
        //        return;
        //    }
        //    if (txtQty.Text == "" || decimal.Parse(txtQty.Text) == 0)
        //    {
        //        MessageBox.Show("Enter Valid QTY");
        //        txtQty.Focus();
        //        return;
        //    }
        //    if (cmbWareFrom.Text == cmbWareTo.Text)
        //    {
        //        MessageBox.Show("Select Warehouse To Transfer First");
        //        return;
        //    }
        //    if (decimal.Parse(dgvTo.SelectedRows[0].Cells["qty"].Value.ToString()) < decimal.Parse(txtQty.Text))
        //    {
        //        MessageBox.Show("Qty Entered Is Greater Than Found, Enter Valid QTY");
        //        txtQty.Focus();
        //        return;
        //    }
        //    int invId = 0; // Assuming invId is not used in this context, set to 0 or modify as needed
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items_warehouse where warehouse_id = @warehouse_id and item_id = @id",
        //                                    DBClass.CreateParameter("warehouse_id", cmbWareFrom.SelectedValue.ToString()),
        //                                    DBClass.CreateParameter("id", dgvTo.SelectedRows[0].Cells["id"].Value.ToString())))
        //        if (reader.Read())
        //        {
        //            invId = int.Parse(reader["id"].ToString());
        //            DBClass.ExecuteNonQuery("update tbl_items_warehouse set qty = qty+@qty where id = @id",
        //                DBClass.CreateParameter("qty", txtQty.Text),
        //                DBClass.CreateParameter("id", reader["id"].ToString()));
        //            Utilities.LogAudit(frmLogin.userId, "Update Warehouse Item", "Warehouse Item", int.Parse(reader["id"].ToString()), "Updated Qty: " + txtQty.Text + " for Item: " + dgvTo.SelectedRows[0].Cells["Item Name"].Value);
        //        }
        //        else
        //        {
        //            invId = int.Parse(DBClass.ExecuteScalar("insert into tbl_items_warehouse(warehouse_id,item_id,qty) values (@warehouse_id,@item_id,@qty);SELECT LAST_INSERT_ID();",
        //                DBClass.CreateParameter("warehouse_id", cmbWareFrom.SelectedValue.ToString()),
        //                DBClass.CreateParameter("item_id", dgvTo.SelectedRows[0].Cells["id"].Value.ToString()),
        //                DBClass.CreateParameter("qty", txtQty.Text)).ToString());
        //            Utilities.LogAudit(frmLogin.userId, "Insert Warehouse Item", "Warehouse Item", int.Parse(dgvTo.SelectedRows[0].Cells["id"].Value.ToString()), "Inserted Qty: " + txtQty.Text + " for Item: " + dgvTo.SelectedRows[0].Cells["Item Name"].Value);
        //        }

        //    DBClass.ExecuteNonQuery("update tbl_items_warehouse set qty = qty-@qty where warehouse_id = @warehouse_id and item_id = @item_id",
        //             DBClass.CreateParameter("qty", txtQty.Text),
        //             DBClass.CreateParameter("warehouse_id", cmbWareTo.SelectedValue.ToString()),
        //             DBClass.CreateParameter("item_id", dgvTo.SelectedRows[0].Cells["id"].Value.ToString()));
        //    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_item_warehouse_transaction (`date`, `warehouse_from`, `warehouse_to`, `item_id`, `qty`, `description`, `created_by`, `created_date`) 
        //                                VALUES (@date, @warehouse_from, @warehouse_to, @item_id, @qty, @description, @created_by, @created_date)",
        //                              DBClass.CreateParameter("@date", DateTime.Now.Date),
        //                              DBClass.CreateParameter("@warehouse_from", cmbWareTo.SelectedValue.ToString()),
        //                              DBClass.CreateParameter("@warehouse_to", cmbWareFrom.SelectedValue.ToString()),
        //                              DBClass.CreateParameter("@item_id", dgvTo.SelectedRows[0].Cells["id"].Value.ToString()),
        //                              DBClass.CreateParameter("@qty", txtQty.Text),
        //                              DBClass.CreateParameter("@description", "Item Transfer:"+invId),
        //                              DBClass.CreateParameter("@created_by", frmLogin.userId),
        //                              DBClass.CreateParameter("@created_date", DateTime.Now.Date));

        //    // Common variables
        //    DateTime now = DateTime.Now;
        //    int itemId = Convert.ToInt32(dgvTo.SelectedRows[0].Cells["id"].Value);            // or dgvTo for btnToFrom_Click
        //    int wareFrom = Convert.ToInt32(cmbWareTo.SelectedValue);
        //    int wareTo = Convert.ToInt32(cmbWareFrom.SelectedValue);
        //    decimal qty = decimal.Parse(txtQty.Text);
        //    string itemName = dgvTo.SelectedRows[0].Cells["Item Name"].Value.ToString(); // or dgvTo
        //    decimal costPrice = 0; // Optional: you can fetch from tbl_items or warehouse stock
        //    decimal salesPrice = 0;

        //    // Fetch cost_price and sales_price from latest item transaction
        //    using (MySqlDataReader reader = DBClass.ExecuteReader(
        //        "SELECT cost_price, sales_price FROM tbl_item_transaction WHERE warehouse_id = @warehouse_id AND item_id = @item_id ORDER BY date DESC LIMIT 1",
        //        DBClass.CreateParameter("@warehouse_id", wareFrom.ToString()),
        //        DBClass.CreateParameter("@item_id", itemId.ToString())))
        //    {
        //        if (reader.Read())
        //        {
        //            if (reader["cost_price"] != DBNull.Value)
        //                costPrice = Convert.ToDecimal(reader["cost_price"]);
        //            if (reader["sales_price"] != DBNull.Value)
        //                salesPrice = Convert.ToDecimal(reader["sales_price"]);
        //        }
        //    }

        //    // Fallback to tbl_items if no transaction found
        //    if (costPrice == 0 || salesPrice == 0)
        //    {
        //        using (MySqlDataReader reader = DBClass.ExecuteReader(
        //            "SELECT cost_price, sales_price FROM tbl_items WHERE id = @item_id",
        //            DBClass.CreateParameter("@item_id", itemId.ToString())))
        //        {
        //            if (reader.Read())
        //            {
        //                if (reader["cost_price"] != DBNull.Value && costPrice == 0)
        //                    costPrice = Convert.ToDecimal(reader["cost_price"]);
        //                if (reader["sales_price"] != DBNull.Value && salesPrice == 0)
        //                    salesPrice = Convert.ToDecimal(reader["sales_price"]);
        //            }
        //        }
        //    }

        //    // 1. OUT from Source Warehouse
        //    addItemTransaction(
        //        now,
        //        invId, // invId = 0 or warehouse transaction ID if needed
        //        itemId,
        //        costPrice,
        //        salesPrice,
        //        0, // qty_in
        //        qty, // qty_out
        //        $"Transferred to warehouse {cmbWareFrom.Text} | {itemName}",
        //        wareFrom
        //    );

        //    // 2. IN to Destination Warehouse
        //    addItemTransaction(
        //        now,
        //        invId,
        //        itemId,
        //        costPrice,
        //        salesPrice,
        //        qty,
        //        0,
        //        $"Received from warehouse {cmbWareTo.Text} | {itemName}",
        //        wareTo
        //    );

        //    bindWarehouseTo();
        //    bindWarehouseFrom();
        //    Utilities.LogAudit(frmLogin.userId, "Transfer Warehouse Item", "Warehouse Item", int.Parse(dgvTo.SelectedRows[0].Cells["id"].Value.ToString()), "Transferred Qty: " + txtQty.Text + " for Item: " + dgvTo.SelectedRows[0].Cells["Item Name"].Value);
        //    //EventHub.RefreshWarehouse();
        //    EventHub.RefreshItem();
        //}

    }
}
