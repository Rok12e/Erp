using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAddStockSettlement : Form
    {
        int inventoryAccount, stockSettleAccount;
        decimal invId;
       
        string invCode = "SIS-0001";
        int id;

        public frmAddStockSettlement(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            if (id != 0)
                this.Text = "Stock Settlement - Edit ";
            else
                this.Text = "Stock Settlement - New ";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmAddStockSettlement_Load(object sender, EventArgs e)
        {
            dgvItems.Columns["newQty"].DefaultCellStyle.BackColor = Color.LightGray;
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_coa_config WHERE category='Inventory'"))
                if (reader.Read())
                    inventoryAccount = int.Parse(reader["account_id"].ToString());
                else
                {
                    MessageBox.Show("Inventory Account Must Be Set First");
                    return;
                }
            using (var reader = DBClass.ExecuteReader("SELECT * FROM tbl_coa_config WHERE category='Stock Settlement'"))
                if (reader.Read())
                    stockSettleAccount = int.Parse(reader["account_id"].ToString());
                else
                {
                    MessageBox.Show("Stock Settlement Account Must Be Set First");
                    return;
                }
            BindCombos.PopulateWarehouse(cmbWarehouse);
            if (id != 0)
            {
                BindInvoice();
                btnSave.Enabled = btnSaveNew.Enabled = UserPermissions.canEdit("Stock Management"); 
            }
        }
        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_item_stock_settlement where id = @id",
                DBClass.CreateParameter("id", id)))
            {
                reader.Read();
                cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                invCode = txtInvoiceId.Text = reader["code"].ToString();
            }
            invId = id;
            BindInvoiceItems();
            CalculateTotal();
        }
        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_item_stock_settlement_details.*,tbl_items.type,method, tbl_items.code as code FROM tbl_item_stock_settlement_details INNER JOIN 
                                                                    tbl_items ON tbl_item_stock_settlement_details.item_id = tbl_items.id WHERE 
                                                                    tbl_item_stock_settlement_details.settle_id = @id;",
                                                            DBClass.CreateParameter("id", id)))
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["item_id"].ToString(), "", reader["code"].ToString(), reader["code"].ToString(), reader["on_hand"].ToString(),
                        reader["price"].ToString(), (decimal.Parse(reader["price"].ToString()) * decimal.Parse(reader["on_hand"].ToString())), reader["new_on_hand"].ToString(),
                        (decimal.Parse(reader["new_on_hand"].ToString()) - decimal.Parse(reader["on_hand"].ToString())));
                    if (decimal.Parse(reader["new_on_hand"].ToString()) > decimal.Parse(reader["on_hand"].ToString()))
                    {
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["minusamount"].Value = "0";
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["plusamount"].Value = (decimal.Parse(reader["price"].ToString()) * (decimal.Parse(reader["new_on_hand"].ToString()) - decimal.Parse(reader["on_hand"].ToString()))).ToString();
                    }
                    else
                    {
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["plusamount"].Value = "0";
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["minusamount"].Value = (decimal.Parse(reader["price"].ToString()) * (decimal.Parse(reader["on_hand"].ToString()) - decimal.Parse(reader["new_on_hand"].ToString()))).ToString();
                    }
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = reader["method"].ToString();
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = reader["method"].ToString();
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = reader["type"].ToString();
                }
        }
        private DataTable GetFilteredProductsByName(string name = "")
        {
            int warehouseId = cmbWarehouse.SelectedValue != null
                ? int.Parse(cmbWarehouse.SelectedValue.ToString())
                : 0;

            string query = @"SELECT DISTINCT i.code, i.name FROM tbl_items i LEFT JOIN tbl_item_transaction t ON t.item_id = i.id 
                WHERE t.warehouse_id = @warehouseId AND i.state = 0 AND i.active = 0 AND i.type != 'Service'";
            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@warehouseId", warehouseId)
            };

            if (!string.IsNullOrEmpty(name))
            {
                query += " AND i.name LIKE @name";
                parameters.Add(new MySqlParameter("@name", $"%{name}%"));
            }

            query += " ORDER BY i.name LIMIT 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private DataTable GetFilteredProductsByCode(string code = "")
        {
            int warehouseId = cmbWarehouse.SelectedValue != null ? int.Parse(cmbWarehouse.SelectedValue.ToString()) : 0;

            string query = @"SELECT DISTINCT i.code, i.name FROM tbl_items i LEFT JOIN tbl_item_transaction t ON t.item_id = i.id 
                    WHERE t.warehouse_id = @warehouseId AND i.state = 0 AND i.active = 0 AND i.type != 'Service'";

            var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@warehouseId", warehouseId)
            };

            if (!string.IsNullOrEmpty(code))
            {
                query += " AND i.code LIKE @code";
                parameters.Add(new MySqlParameter("@code", $"%{code}%"));
            }

            query += " ORDER BY i.code LIMIT 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_coa_config WHERE category='Inventory'"))
                if (reader.Read())
                    inventoryAccount = int.Parse(reader["account_id"].ToString());
                else
                {
                    MessageBox.Show("Inventory Account Must Be Set First");
                    return;
                }
            using (var reader = DBClass.ExecuteReader("SELECT * FROM tbl_coa_config WHERE category='Stock Settlement'"))
                if (reader.Read())
                    stockSettleAccount = int.Parse(reader["account_id"].ToString());
                else
                {
                    MessageBox.Show("Stock Settlement Account Must Be Set First");
                    return;
                }
            checkItemValidty();
            if (id == 0)
            {
                if (insertInvoice())
                    this.Close();
            }
            else
            {
                if (updateInvoice())
                    this.Close();
            }
        }
        private bool updateInvoice()
        {
            if (!chkRequiredDate())
                return false;
            DBClass.ExecuteNonQuery(@"
                UPDATE tbl_item_stock_settlement 
                SET 
                    code = @code,
                    date = @date,
                    warehouse_id = @warehouse_id,
                    total_plus = @total_plus,
                    total_minus = @total_minus,
                    created_by = @created_by,
                    created_date = @created_date
                WHERE id = @id;",
                  DBClass.CreateParameter("date", dtInv.Value.Date),
                  DBClass.CreateParameter("code", invCode),
                  DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
                  DBClass.CreateParameter("total_plus", txtAmountPlus.Text),
                  DBClass.CreateParameter("total_minus", txtAmountMinus.Text),
                  DBClass.CreateParameter("created_by", frmLogin.userId),
                  DBClass.CreateParameter("created_date", DateTime.Now.Date),
                  DBClass.CreateParameter("id", id)
              );
            DBClass.ExecuteReader(@"delete from tbl_transaction where transaction_id =@id and (description like 'Stock Inventory Settlement%')",
                                                       DBClass.CreateParameter("id", id));
            insertJournalEntries();
            ReturnItemsToInventory();
            insertInvItems();
         
            return true;
        }

        private void ReturnItemsToInventory()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"select tbl_item_stock_settlement_details.* ,tbl_items.method,tbl_items.type from tbl_item_stock_settlement_details
                                                           inner join tbl_items on tbl_item_stock_settlement_details.item_id = tbl_items.id 
                                                           where settle_id =" + id))
                while (reader.Read())
                {
                    if (decimal.Parse(reader["qty"].ToString()) > 0)
                    {
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand =on_hand- @qty where id =@id", DBClass.CreateParameter("id", reader["item_id"].ToString()), DBClass.CreateParameter("qty", reader["qty"].ToString()));
                        DBClass.ExecuteNonQuery("delete from tbl_item_transaction  where reference =@invId and type= 'Stock Inventory Settlement'", DBClass.CreateParameter("invId", id));
                    }
                    else
                    {
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand =on_hand+ @qty where id =@id", DBClass.CreateParameter("id", reader["item_id"].ToString()), DBClass.CreateParameter("qty", -decimal.Parse(reader["qty"].ToString())));
                        DBClass.ExecuteNonQuery("delete from tbl_item_transaction  where reference =@invId and type= 'Stock Inventory Settlement'",
                            DBClass.CreateParameter("invId", id));
                        string tq = "";
                        if (reader["method"].ToString() == "agv")
                            continue;
                        if (reader["method"].ToString() == "fifo")
                            tq = (@"SELECT * FROM tbl_item_transaction WHERE item_id = @id and qty_in != qty_inc
                                                                     and qty_out = 0 ORDER BY id asc");
                        else
                            tq = (@"SELECT * FROM tbl_item_transaction WHERE item_id = @id and qty_in != qty_inc
                                                                     and qty_out = 0 ORDER BY id asc");
                        decimal attempQty = -decimal.Parse(reader["qty"].ToString());
                        using(var tReader = DBClass.ExecuteReader(tq, DBClass.CreateParameter("id", reader["item_id"].ToString())))
                        while (tReader.Read())
                        {
                            if (decimal.Parse(tReader["qty_inc"].ToString()) + attempQty <= decimal.Parse(tReader["qty_in"].ToString()))
                            {
                                DBClass.ExecuteNonQuery("update tbl_item_transaction set qty_inc = qty_inc +@qty where id = @id",
                                    DBClass.CreateParameter("qty", attempQty),
                                    DBClass.CreateParameter("id", tReader["id"].ToString()));
                                break;
                            }
                            else
                            {
                                attempQty = attempQty - decimal.Parse(tReader["qty_in"].ToString()) - decimal.Parse(tReader["qty_inc"].ToString());
                                DBClass.ExecuteNonQuery("update tbl_item_transaction set qty_inc = qty_in where id = @id",
                                  DBClass.CreateParameter("id", tReader["id"].ToString()));
                            }
                        }
                    }
                    DBClass.ExecuteNonQuery("delete from tbl_item_stock_settlement_details where id =@id", DBClass.CreateParameter("id", reader["id"].ToString()));
                }
        }

        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;

            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT code FROM tbl_item_stock_settlement ORDER BY 
                CAST(SUBSTRING_INDEX(code, '-', -1) AS UNSIGNED) DESC LIMIT 1; "))
                if (reader.Read() && reader["code"].ToString() != "")
                    invCode = "SIS-000" + (int.Parse(reader["code"].ToString().Replace("SIS-", "")) + 1);

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_item_stock_settlement (code,date, warehouse_id, total_plus, total_minus,
                created_by, created_date, state) VALUES (@code,@DATE,@warehouse_id,@total_plus, @total_minus,@created_by, @created_date, 0);
                SELECT LAST_INSERT_ID();",
               DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("code", invCode),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("total_plus", txtAmountPlus.Text),
               DBClass.CreateParameter("total_minus", txtAmountMinus.Text),
               DBClass.CreateParameter("created_by", frmLogin.userId),
               DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
            insertJournalEntries();
            insertInvItems();
            txtInvoiceId.Text = invCode.ToString();

            Utilities.LogAudit(frmLogin.userId, "Insert Stock Settlement", "Stock Settlement", (int)invId, "Inserted Stock Settlement: " + invCode);

            return true;
        }

        private void insertJournalEntries()
        {
            if (decimal.Parse(txtAmountMinus.Text) > 0)
            {
                CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
                     stockSettleAccount.ToString(),
                       txtAmountMinus.Text, "0", invId.ToString(), invId.ToString(), "Stock Inventory Settlement", "Stock Inventory Settlement NO. " + invCode,
                     frmLogin.userId, DateTime.Now.Date);
                CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
                     inventoryAccount.ToString(),
                     "0", txtAmountMinus.Text, invId.ToString(), invId.ToString(), "Inventory Settlement", "Stock Inventory Settlement NO. " + invCode,
                    frmLogin.userId, DateTime.Now.Date);
            }
            if (decimal.Parse(txtAmountPlus.Text) > 0)
            {
                CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
                     stockSettleAccount.ToString(),
                        "0", txtAmountPlus.Text, invId.ToString(), invId.ToString(), "Stock Inventory Settlement", "Stock Inventory Settlement NO. " + invCode,
                     frmLogin.userId, DateTime.Now.Date);
                CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
                     inventoryAccount.ToString(),
                    txtAmountPlus.Text, "0", invId.ToString(), invId.ToString(), "Inventory Settlement", "Stock Inventory Settlement NO. " + invCode,
                    frmLogin.userId, DateTime.Now.Date);
            }
        }

        private void insertInvItems()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_item_stock_settlement_details (settle_id, item_id,on_hand, price, new_on_hand, qty, minusamount,plusamount)
                                         VALUES (@settle_id, @item_id, @on_hand,@price, @new_on_hand, @qty, @minusamount,@plusamount);",
                  DBClass.CreateParameter("@settle_id", invId),
                  DBClass.CreateParameter("@item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                  DBClass.CreateParameter("@on_hand", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value)),
                  DBClass.CreateParameter("@price", dgvItems.Rows[i].Cells["cost_price"].Value),
                  DBClass.CreateParameter("@new_on_hand", dgvItems.Rows[i].Cells["newQty"].Value),
                  DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qtydif"].Value),
                  DBClass.CreateParameter("@minusamount", decimal.Parse(dgvItems.Rows[i].Cells["minusamount"].Value.ToString())),
                DBClass.CreateParameter("@plusamount", (decimal.Parse(dgvItems.Rows[i].Cells["plusamount"].Value.ToString()))));
                DBClass.ExecuteNonQuery("update tbl_items set on_hand=@qty where id = @id",
                DBClass.CreateParameter("id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                DBClass.CreateParameter("qty", decimal.Parse(dgvItems.Rows[i].Cells["newqty"].Value.ToString())));
                insertItemTransaction(dgvItems.Rows[i]);

                Utilities.LogAudit(frmLogin.userId, "Insert Stock Settlement Item", "Stock Settlement", (int)invId, 
                    "Inserted Item: " + dgvItems.Rows[i].Cells["itemId"].Value.ToString() + " in Stock Settlement: " + invCode);
            }
        }
        private void insertItemTransaction(DataGridViewRow row)
        {
            decimal qty = 0;

            if (row.Cells["itemId"].Value == null || string.IsNullOrWhiteSpace(row.Cells["itemId"].Value.ToString()))
            {
                MessageBox.Show("Invalid Item ID.");
                return;
            }
            if (!decimal.TryParse(row.Cells["newqty"].Value.ToString(), out qty) || qty <= 0)
            {
                MessageBox.Show("Invalid Quantity.");
                return;
            }

            if (decimal.Parse(row.Cells["newQty"].Value.ToString()) < decimal.Parse(row.Cells["qty"].Value.ToString()))
            {
                qty = decimal.Parse(row.Cells["qty"].Value.ToString()) - decimal.Parse(row.Cells["newQty"].Value.ToString());
                if (row.Cells["method"].Value.ToString() == "fifo")
                {
                    decimal remainingQty = qty;
                    using (var reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_item_transaction WHERE qty_inc > 0 AND item_id = @id ORDER BY id ASC",
                        DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString())))
                        while (reader.Read() && remainingQty > 0)
                        {
                            decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
                            decimal qtyToUse = Math.Min(remainingQty, availableQty);
                            remainingQty -= qtyToUse;

                            CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Stock Settlement", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                                row.Cells["cost_price"].Value.ToString(), "0", "0", qtyToUse.ToString(), "0", "Stock Settlement No. " + invCode, "0");

                            DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
                                DBClass.CreateParameter("qty", qtyToUse),
                                DBClass.CreateParameter("id", reader["id"].ToString()));
                        }
                }
                else if (row.Cells["method"].Value.ToString() == "lifo")
                {
                    decimal remainingQty = qty;
                    using (var reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_item_transaction WHERE qty_inc > 0 AND item_id = @id ORDER BY id DESC",
                        DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString())))
                        while (reader.Read() && remainingQty > 0)
                        {
                            decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);

                            decimal qtyToUse = Math.Min(remainingQty, availableQty);
                            remainingQty -= qtyToUse;

                            CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Stock Settlement", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                                row.Cells["cost_price"].Value.ToString(), "0", "0", qtyToUse.ToString(), "0", "Stock Settlement No. " + invCode, "0");

                            DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
                                DBClass.CreateParameter("qty", qtyToUse),
                                DBClass.CreateParameter("id", reader["id"].ToString()));
                        }
                }
                else
                {

                    using (var reader = DBClass.ExecuteReader(@"SELECT SUM(qty_in * cost_price) / SUM(qty_in) AS cost_price 
                                         FROM tbl_item_transaction WHERE qty_in > 0 AND item_id = @id",
                        DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString())))

                        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Stock Settlement", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                           row.Cells["cost_price"].Value.ToString(), "0", row.Cells["cost_price"].Value.ToString(), qty.ToString(), "0", "Stock Settlement No. " + invCode, "0");
                }

            }
            else
            {
                qty = decimal.Parse(row.Cells["newQty"].Value.ToString()) - decimal.Parse(row.Cells["qty"].Value.ToString());

                CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Stock Settlement", invId.ToString(), row.Cells["itemId"].Value.ToString(), row.Cells["cost_price"].Value.ToString(),
                      qty.ToString(), "0", "0",qty.ToString(), "Stock Settlement No. " + invCode, "0");

            }

        }
        private bool chkRequiredDate()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if ((dgvItems.Rows[i].Cells["plusamount"].Value == null && dgvItems.Rows[i].Cells["minusamount"].Value == null)
                    || (dgvItems.Rows[i].Cells["plusamount"].Value.ToString() == "" && dgvItems.Rows[i].Cells["minusamount"].Value.ToString() == "")
                    || (decimal.Parse(dgvItems.Rows[i].Cells["plusamount"].Value.ToString()) == 0 && decimal.Parse(dgvItems.Rows[i].Cells["minusamount"].Value.ToString()) == 0))
                {
                    MessageBox.Show("Total Item In Row " + (dgvItems.Rows[i].Index + 1) + " Can't Be 0 or Null");
                    return false;
                }
            }

            if (dgvItems.Rows.Count == 1)
            {
                MessageBox.Show("Insert Items First.");
                return false;
            }
            if ((txtAmountMinus.Text == "" || decimal.Parse(txtAmountMinus.Text) == 0) && ((txtAmountPlus.Text == "" || decimal.Parse(txtAmountPlus.Text) == 0)))
            {
                MessageBox.Show("Amount Must Be Bigger Than Zero");
                return false;
            }
            return true;
        }
        private void resetTextBox()
        {
            txtInvoiceId.Text = txtAmountMinus.Text = txtAmountPlus.Text = "";
            dtInv.Value = DateTime.Now;
            dgvItems.Rows.Clear();
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertInvoice())
                    resetTextBox();
            }
            else
            {
                if (updateInvoice())
                    resetTextBox();
            }
        }
        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }


        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox0)
            {
                textBox0.TextChanged -= ItemCode_TextChanged;
                textBox0.TextChanged -= ItemName_TextChanged;
                if (lstAccountSuggestions.Visible)
                {
                    lstAccountSuggestions.Visible = false;
                }
                lstAccountSuggestions.SendToBack();
            }
            if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["name"].Index)
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.TextChanged -= ItemName_TextChanged;
                    textBox.TextChanged += ItemName_TextChanged;

                    lstAccountSuggestions.Tag = textBox;
                }
            }
            else if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["code"].Index)
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.TextChanged -= ItemCode_TextChanged;
                    textBox.TextChanged += ItemCode_TextChanged;

                    lstAccountSuggestions.Tag = textBox;
                }
            }
            else
            {
                lstAccountSuggestions.Visible = false;
                lstAccountSuggestions.Tag = null;
            }

        }

        private void ItemCode_TextChanged(object sender, EventArgs e)
        {
            TextBox editingTextBox = sender as TextBox;
            if (editingTextBox == null) return;

            string input = editingTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                lstAccountSuggestions.Visible = false;
                return;
            }
            DataTable filtered = GetFilteredProductsByCode(input);

            lstAccountSuggestions.Items.Clear();

            foreach (DataRow row in filtered.Rows)
            {
                lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
            }

            if (lstAccountSuggestions.Items.Count > 0)
            {
                // Get cell location
                var cellRect = dgvItems.GetCellDisplayRectangle(dgvItems.CurrentCell.ColumnIndex, dgvItems.CurrentCell.RowIndex, true);
                Point locationOnForm = dgvItems.PointToScreen(cellRect.Location);
                Point locationRelativeToForm = this.PointToClient(locationOnForm);

                lstAccountSuggestions.SetBounds(
                    locationRelativeToForm.X,
                    locationRelativeToForm.Y + cellRect.Height,
                    cellRect.Width + 100,
                    120
                );

                lstAccountSuggestions.Tag = editingTextBox;
                lstAccountSuggestions.Visible = true;
                lstAccountSuggestions.BringToFront();
            }
            else
            {
                lstAccountSuggestions.Visible = false;
            }
        }

        private void ItemName_TextChanged(object sender, EventArgs e)
        {
            TextBox editingTextBox = sender as TextBox;
            if (editingTextBox == null) return;

            string input = editingTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                lstAccountSuggestions.Visible = false;
                return;
            }
            DataTable filtered = GetFilteredProductsByName(input);

            lstAccountSuggestions.Items.Clear();

            foreach (DataRow row in filtered.Rows)
            {
                lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
            }

            if (lstAccountSuggestions.Items.Count > 0)
            {
                // Get cell location
                var cellRect = dgvItems.GetCellDisplayRectangle(dgvItems.CurrentCell.ColumnIndex, dgvItems.CurrentCell.RowIndex, true);
                Point locationOnForm = dgvItems.PointToScreen(cellRect.Location);
                Point locationRelativeToForm = this.PointToClient(locationOnForm);

                lstAccountSuggestions.SetBounds(
                    locationRelativeToForm.X,
                    locationRelativeToForm.Y + cellRect.Height,
                    cellRect.Width + 100,
                    120
                );

                lstAccountSuggestions.Tag = editingTextBox;
                lstAccountSuggestions.Visible = true;
                lstAccountSuggestions.BringToFront();
            }
            else
            {
                lstAccountSuggestions.Visible = false;
            }
        }

        void CalculateTotal()
        {
            decimal minus = 0, plus = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["minusamount"].Value != null)
                    minus += Convert.ToDecimal(row.Cells["minusamount"].Value);
                if (row.Cells["plusamount"].Value != null)
                    plus += Convert.ToDecimal(row.Cells["plusamount"].Value);

            }
            txtAmountPlus.Text = plus.ToString("0.000");
            txtAmountMinus.Text = minus.ToString("0.000");
        }
        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];
                decimal qty = GetDecimalValue(row, "qty");
                decimal costPrice = GetDecimalValue(row, "cost_price");
                if (e.ColumnIndex == dgvItems.Columns["Code"].Index)
                {
                    string codeValue = row.Cells["Code"].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(codeValue))
                        insertItemThroughCodeOrText("code", codeValue);
                }
                else if (e.ColumnIndex == dgvItems.Columns["Name"].Index)
                {
                    string nameValue = row.Cells["Name"].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(nameValue))
                        insertItemThroughCodeOrText("name", nameValue);
                }
                else if (e.ColumnIndex == dgvItems.Columns["newqty"].Index)
                {
                    if (row.Cells["qty"].Value == null)
                        return;

                    if (dgvItems.CurrentRow.Cells["itemId"].Value == null  || row.Cells["newqty"].Value.ToString() == "" || row.Cells["newqty"].Value.ToString() == row.Cells["qty"].Value.ToString())
                        row.Cells["minusamount"].Value = row.Cells["qtydif"].Value = row.Cells["plusamount"].Value = 0;
                    else
                    {
                        row.Cells["qtydif"].Value = (decimal.Parse(row.Cells["newqty"].Value.ToString()) - decimal.Parse(row.Cells["qty"].Value.ToString())).ToString();
                        if (decimal.Parse(row.Cells["qtydif"].Value.ToString()) > 0)
                        {
                            row.Cells["plusamount"].Value = (decimal.Parse(row.Cells["qtydif"].Value.ToString()) * decimal.Parse(row.Cells["cost_price"].Value.ToString())).ToString();
                            row.Cells["minusamount"].Value = "0";
                        }
                        else
                        {
                            row.Cells["minusamount"].Value = (-decimal.Parse(row.Cells["qtydif"].Value.ToString()) * decimal.Parse(row.Cells["cost_price"].Value.ToString())).ToString();
                            row.Cells["plusamount"].Value = "0";
                        }
                    }
                }
                CalculateTotal();
            }
        }
        void checkItemValidty()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["itemId"].Value == null)
                {
                    dgvItems.Rows.Remove(dgvItems.Rows[i]);
                    i--;
                }
            }
        }
        private decimal GetDecimalValue(DataGridViewRow row, string columnName)
        {
            decimal result;
            var cellValue = row.Cells[columnName].Value;
            if (cellValue != null && decimal.TryParse(cellValue.ToString(), out result))
                return result;
            else
                return 0;
        }

        private void insertItemThroughCodeOrText(string type, string inputText = null)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return;

            MySqlDataReader reader = null;
            try
            {
                if (type == "code")
                {
                    reader = DBClass.ExecuteReader(@"SELECT i.id,method,type, code, 
                            (SELECT IFNULL(SUM(qty_in - qty_out), 0) FROM tbl_item_transaction WHERE item_id = i.id AND warehouse_id = @wId) AS qty, cost_price, name
                          FROM tbl_items i LEFT JOIN tbl_items_warehouse w ON w.item_id = i.id
                          WHERE code = @code",
                        DBClass.CreateParameter("code", inputText.Trim()),
                        DBClass.CreateParameter("wId", cmbWarehouse.SelectedValue.ToString()));
                }
                else if (type == "name")
                {
                    reader = DBClass.ExecuteReader(@"SELECT i.id,method,type, code, 
                            (SELECT IFNULL(SUM(qty_in - qty_out), 0) FROM tbl_item_transaction WHERE item_id = i.id AND warehouse_id = @wId) AS qty, cost_price, name 
                          FROM tbl_items i LEFT JOIN tbl_items_warehouse w ON w.item_id = i.id
                          WHERE name = @name",
                        DBClass.CreateParameter("wId", cmbWarehouse.SelectedValue.ToString()),
                        DBClass.CreateParameter("name", inputText.Trim()));
                }

                if (reader != null && reader.Read())
                {
                    dgvItems.CurrentRow.Cells["qty"].Value = reader["qty"].ToString();
                    dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                    dgvItems.CurrentRow.Cells["name"].Value = reader["name"].ToString();
                    dgvItems.CurrentRow.Cells["itemid"].Value = reader["id"].ToString();
                    dgvItems.CurrentRow.Cells["cost_price"].Value = reader["cost_price"].ToString();
                    dgvItems.CurrentRow.Cells["total"].Value = Convert.ToDecimal(reader["cost_price"]) * Convert.ToDecimal(reader["qty"]);
                    dgvItems.CurrentRow.Cells["newqty"].Value = Convert.ToDecimal(reader["qty"]);
                    dgvItems.CurrentRow.Cells["qtydif"].Value = "0";
                    dgvItems.CurrentRow.Cells["minusamount"].Value = "0";
                    dgvItems.CurrentRow.Cells["plusamount"].Value = "0";
                    dgvItems.CurrentRow.Cells["method"].Value = reader["method"];
                    dgvItems.CurrentRow.Cells["type"].Value = reader["type"];
                }
                else
                {
                    DataGridViewRow row = dgvItems.CurrentRow;
                    if (row != null)
                    {
                        row.Cells["itemid"].Value = null;
                        row.Cells["cost_price"].Value = null;
                        row.Cells["total"].Value = null;
                        row.Cells["method"].Value = null;
                        row.Cells["type"].Value = null;
                        row.Cells["qty"].Value = 0;
                        row.Cells["newqty"].Value = null;
                        row.Cells["qtydif"].Value = null;
                        row.Cells["minusamount"].Value = null;
                        row.Cells["plusamount"].Value = null;
                        if (type == "name")
                            row.Cells["code"].Value = null;
                        if (type == "code")
                            row.Cells["name"].Value = null;
                    }
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceId.Text))
                return;
            int? currentId = Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId == null || currentId <= 1)
                return;

            currentId = currentId - 1;
            if (currentId <= 0)
            {
                clear();
                MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "select id from tbl_item_stock_settlement where state = 0 and id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clear();
                    MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceId.Text))
                return;
            int? currentId = Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_item_stock_settlement WHERE state = 0 AND id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clear();
                    MessageBox.Show("No next records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void clear()
        {
            id = 0;
            invId = 0;
            invCode = "";
            resetTextBox();
            dgvItems.Rows.Clear();
            cmbWarehouse.SelectedIndex = -1;
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            {
                dgvItems.Rows.Remove(dgvItems.CurrentRow);
                CalculateTotal();
            }
        }
        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }

        private void lstAccountSuggestions_Click(object sender, EventArgs e)
        {
            if (lstAccountSuggestions.SelectedItem == null)
                return;

            string selected = lstAccountSuggestions.SelectedItem.ToString();

            int separatorIndex = selected.IndexOf('-');
            if (separatorIndex == -1) return;

            string selectedCode = selected.Substring(0, separatorIndex).Trim();
            string selectedName = selected.Substring(separatorIndex + 1).Trim();
            
            if (lstAccountSuggestions.Tag is TextBox textBox)
            {
                if (dgvItems.CurrentCell != null && dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["code"].Index)
                {
                    dgvItems.CurrentCell.Value = selectedCode;
                }
                else if (dgvItems.CurrentCell != null && dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["name"].Index)
                {
                    dgvItems.CurrentCell.Value = selectedName;
                }
            }

            lstAccountSuggestions.Visible = false;
        }
    }
}
