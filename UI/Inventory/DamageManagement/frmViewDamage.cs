using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewDamage : Form
    {
        decimal invId;
        int  level4Inventory;
        private MasterDamage _damage;
        string invCode = "DI-0001";
        int id;

        public frmViewDamage(MasterDamage _damage, int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this._damage = _damage;
            this.id = id;
            if (id != 0)
                this.Text = "Damage - Edit Damage Items";
            else
                this.Text = "Damage - New Damage Items";
            headerUC1.FormText = this.Text;
        }
      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewDamage_Load(object sender, EventArgs e)
        {
             dtInv.Value =  DateTime.Now.Date;
            BindCombos.PopulateWarehouse(cmbWarehouse);
            LoaddgvItems();
            bindCombo();
            if (id != 0)
            {
                BindInvoice();
                btnSave.Enabled = UserPermissions.canEdit("Damage");
            }
        }
        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_damage where id = @id",
                DBClass.CreateParameter("id", id)))
            {
                reader.Read();
                cmbCustomer.SelectedValue = reader["reported_by"].ToString();
                cmbAccountName.SelectedValue = reader["account_id"].ToString();
                cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                invCode = txtInvoiceId.Text = reader["reference_no"].ToString();
                txtDamageReason.Text = reader["damage_reason"].ToString();
                invId = id;
                BindInvoiceItems();
                CalculateTotal();
            }
        }
        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_damage_details.*,tbl_items.type,method, tbl_items.code as code FROM tbl_damage_details INNER JOIN 
                                                                    tbl_items ON tbl_damage_details.item_id = tbl_items.id WHERE 
                                                                    tbl_damage_details.damage_id = @id;",
                                                            DBClass.CreateParameter("id", id)))
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["item_id"].ToString(), "", reader["code"].ToString(), reader["code"].ToString(), reader["qty"].ToString(),
                        reader["cost_price"].ToString(), reader["cost_price"].ToString());
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = reader["method"].ToString();
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = reader["type"].ToString();
                }
        }
        private void LoaddgvItems()
        {
            dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items where warehouse_id=@id and state = 0 and type ='11 - Inventory Part' and  active = 0",
                                DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
            dt = DBClass.ExecuteDataTable("select id, concat(name,'-',value , '%') as name from tbl_tax");
            DataGridViewComboBoxColumn vat = (DataGridViewComboBoxColumn)dgvItems.Columns["vat"];
            vat.DataSource = dt;
            vat.DisplayMember = "name";
            vat.ValueMember = "id";
        }
        private void bindCombo()
        {
            BindCombos.PopulateEmployees(cmbCustomer);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            cmbAccountName.SelectedIndex = 0;
            cmbAccountName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Inventory Damage Account");
            aa: 
            level4Inventory = BindCombos.SelectDefaultLevelAccount("Inventory");
            if (level4Inventory == 0)
            {
                MessageBox.Show("Default Accounts For Invoice Must Be Set First");
                goto aa;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
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
            DBClass.ExecuteNonQuery(@"UPDATE tbl_damage 
                                    SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date, reported_by = @reported_by, reference_no = @reference_no, 
                                    damage_reason=@damage_reason,warehouse_id = @warehouse_id,account_id = @account_id, total = @total WHERE id = @id;",
                 DBClass.CreateParameter("id", id),
                  DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("reported_by", cmbCustomer.SelectedValue),
               DBClass.CreateParameter("reference_no", invCode),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue),
               DBClass.CreateParameter("total", txtTotal.Text),
               DBClass.CreateParameter("damage_reason", txtDamageReason.Text),
                 DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));
            UpdateJournalEntries();
            ReturnItemsToInventory();
            insertInvItems();
            Utilities.LogAudit(frmLogin.userId, "Update Damage", "Damage", id, "Updated Damage Invoice: " + invCode);

            if (_damage != null)
                _damage.BindInvoices();

            return true;
        }

        private void UpdateJournalEntries()
        {
            using (var reader = DBClass.ExecuteReader(@"select * from tbl_transaction where transaction_id =@id and (description like 'Damage Invoice%' )",
                                                        DBClass.CreateParameter("id", id)))
                if (reader.Read())
                {
                    CommonInsert.UpdateTransactionEntry(int.Parse(reader["id"].ToString()), dtInv.Value.Date,
                           cmbAccountName.SelectedValue.ToString(),
                           txtTotal.Text, "0", invId.ToString(), cmbCustomer.SelectedValue.ToString(), "Damage Invoice", "Damage Invoice NO. " + invCode,
                           frmLogin.userId, DateTime.Now.Date);
                }
            using (var reader = DBClass.ExecuteReader(@"select * from tbl_transaction where transaction_id =@id and ( description like 'Item Damage For Damage%')",
                                                             DBClass.CreateParameter("id", id)))
                while (reader.Read())
                    DBClass.ExecuteNonQuery("delete from tbl_transaction where id = @id", DBClass.CreateParameter("id", reader["id"].ToString()));

        }

        private void ReturnItemsToInventory()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"select tbl_damage_details.* ,tbl_items.method,tbl_items.type from tbl_damage_details
                                                           inner join tbl_items on tbl_damage_details.item_id = tbl_items.id 
                                                           where damage_id =@id", DBClass.CreateParameter("id", id)))
                while (reader.Read())
                {
                    if (reader["type"].ToString() != "Service")
                    {
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand =on_hand+ @qty where id =@id", DBClass.CreateParameter("id", reader["item_id"].ToString()), DBClass.CreateParameter("qty", reader["qty"].ToString()));
                        DBClass.ExecuteNonQuery("delete from tbl_item_transaction  where reference =@invId and type = 'Damage'", DBClass.CreateParameter("invId", id));
                        MySqlDataReader tReader;
                        if (reader["method"].ToString() == "agv")
                            continue;
                        string qq = "";
                        if (reader["method"].ToString() == "fifo")
                            qq = (@"SELECT * FROM tbl_item_transaction WHERE item_id = @id and qty_in != qty_inc
                                                                     and qty_out = 0 ORDER BY id asc");
                        else
                            qq = (@"SELECT * FROM tbl_item_transaction WHERE item_id = @id and qty_in != qty_inc
                                                                     and qty_out = 0 ORDER BY id asc");
                        decimal attempQty = decimal.Parse(reader["qty"].ToString());
                        using (tReader = DBClass.ExecuteReader(qq, DBClass.CreateParameter("id", reader["item_id"].ToString())))
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
                        Utilities.LogAudit(frmLogin.userId, "Return Item From Damage", "Item Transaction", int.Parse(reader["item_id"].ToString()),
                            "Returned Item From Damage Invoice: " + invCode + " - Item Code - " + reader["code"].ToString());
                    }
                    DBClass.ExecuteNonQuery("delete from tbl_damage_details where id =@id", DBClass.CreateParameter("id", reader["id"].ToString()));
                }
        }

        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;

            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT reference_no FROM tbl_damage ORDER BY 
                CAST(SUBSTRING_INDEX(reference_no, '-', -1) AS UNSIGNED) DESC LIMIT 1; "))
                if (reader.Read() && reader["reference_no"].ToString() != "")
                    invCode = "DI-000" + (int.Parse(reader["reference_no"].ToString().Replace("DI-", "")) + 1);

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_damage 
                            (date, warehouse_id, reference_no,reported_by, damage_reason,   total,account_id, created_by, created_date,state)
                            VALUES 
                            (@date, @warehouse_id, @reference_no, @reported_by, @damage_reason, @total, @account_id, @created_by, @created_date,0);
            SELECT LAST_INSERT_ID();",
            DBClass.CreateParameter("date", dtInv.Value.Date),
            DBClass.CreateParameter("reported_by", cmbCustomer.SelectedValue),
            DBClass.CreateParameter("reference_no", invCode),
            DBClass.CreateParameter("damage_reason", txtDamageReason.Text),
            DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
            DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue),
            DBClass.CreateParameter("total", txtTotal.Text),
            DBClass.CreateParameter("created_by", frmLogin.userId),
            DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString()); 
           
            CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
                 cmbAccountName.SelectedValue.ToString(),
                 txtTotal.Text, "0", invId.ToString(), cmbCustomer.SelectedValue.ToString(), "Damage Invoice", "Damage Invoice NO. " + invCode,
                 frmLogin.userId, DateTime.Now.Date);

             insertInvItems();
            txtInvoiceId.Text = invCode.ToString();
            if (_damage != null)
                _damage.BindInvoices();

            Utilities.LogAudit(frmLogin.userId, "Insert Damage", "Damage", (int)invId, "Inserted Damage Invoice: " + invCode);

            return true;
        }
        private void insertInvItems()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_damage_details (damage_id, item_id, qty,cost_price, total)
                                         VALUES (@damage_id, @item_id, @qty,@cost_price, @total);",
                  DBClass.CreateParameter("@damage_id", invId),
                  DBClass.CreateParameter("@item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                  DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value)),
                  DBClass.CreateParameter("@cost_price", dgvItems.Rows[i].Cells["cost_price"].Value == DBNull.Value || dgvItems.Rows[i].Cells["cost_price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["cost_price"].Value)),
                  DBClass.CreateParameter("@total", dgvItems.Rows[i].Cells["total"].Value == null || dgvItems.Rows[i].Cells["total"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value)));

                if (dgvItems.Rows[i].Cells["type"].Value.ToString() != "Service")
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id=@id",
                    DBClass.CreateParameter("id", dgvItems.Rows[i].Cells["itemId"].Value.ToString())))
                    {
                        reader.Read();
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand=@qty where id = @id",
                            DBClass.CreateParameter("id", reader["id"].ToString()),
                            DBClass.CreateParameter("qty", decimal.Parse(reader["on_hand"].ToString()) - decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString())));
                    }
                    insertItemTransaction(dgvItems.Rows[i]);
                }
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

            if (!decimal.TryParse(row.Cells["qty"].Value.ToString(), out qty) || qty <= 0)
            {
                MessageBox.Show("Invalid Quantity.");
                return;
            }
            
            decimal cost_price = 0;
            decimal totalCost = 0;

            if (row.Cells["method"].Value.ToString() == "fifo")
            {
                decimal remainingQty = qty;
                using (var reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_item_transaction WHERE qty_inc > 0 AND item_id = @id ORDER BY id ASC",
                    DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString())))
                    while (reader.Read() && remainingQty > 0)
                    {
                        decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
                        cost_price = Convert.ToDecimal(reader["cost_price"]);

                        decimal qtyToUse = Math.Min(remainingQty, availableQty);
                        remainingQty -= qtyToUse;
                        totalCost += cost_price * qtyToUse;

                        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Damage", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                            cost_price.ToString(), "0", cost_price.ToString(), qtyToUse.ToString(), "0", "Damage Invoice No. " + invCode, "0");

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
                        cost_price = Convert.ToDecimal(reader["cost_price"]);

                        decimal qtyToUse = Math.Min(remainingQty, availableQty);
                        remainingQty -= qtyToUse;
                        totalCost += cost_price * qtyToUse;

                        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Damage", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                            cost_price.ToString(), "0", cost_price.ToString(), qtyToUse.ToString(), "0", "Damage Invoice No. " + invCode, "0");

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
                    if (reader.Read() && reader["cost_price"] != DBNull.Value)
                    {
                        cost_price = Convert.ToDecimal(reader["cost_price"]);
                    }

                totalCost = cost_price * qty;

                CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Damage", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                    cost_price.ToString(), "0", cost_price.ToString(), qty.ToString(), "0", "Damage Invoice No. " + invCode, "0");
            }

            if (totalCost > 0)
            {
                CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
                    level4Inventory.ToString(),
                    "0", totalCost.ToString(),
                    invId.ToString(), cmbCustomer.SelectedValue.ToString(), "Damage Invoice",
                    "Item Damage For Damage No. " + invCode + " - Item Code - " + row.Cells["code"].Value.ToString(),
                    frmLogin.userId, DateTime.Now.Date);
            }
        }
        private bool chkRequiredDate()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["total"].Value == null
                    || dgvItems.Rows[i].Cells["total"].Value.ToString() == ""
                    || decimal.Parse(dgvItems.Rows[i].Cells["total"].Value.ToString()) == 0)
                {
                    MessageBox.Show("Total Item In Row " + (dgvItems.Rows[i].Index + 1) + " Can't Be 0 or Null");
                    return false;
                }
            }
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Employee Must be Selected.");
                txtCustomerCode.Focus();
                return false;
            }
            if (cmbAccountName.SelectedValue == null)
            {
                MessageBox.Show("Account Must be Selected.");
                txtAccountCode.Focus();
                return false;
            }
            if (dgvItems.Rows.Count == 1)
            {
                MessageBox.Show("Insert Items First.");
                return false;
            }
            if (txtTotal.Text == "" || decimal.Parse(txtTotal.Text) == 0)
            {
                MessageBox.Show("Total Must Be Bigger Than Zero");
                return false;
            }
            return true;
        }
        private void resetTextBox()
        {
            
                txtInvoiceId.Text = txtTotal.Text =  "";
           id = 0;
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
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue == null)
            {
                txtCustomerCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where id = " + cmbCustomer.SelectedValue.ToString()))
                if (reader.Read())
                {
                    txtCustomerCode.Text = reader["code"].ToString();
                }
                else
                    txtCustomerCode.Text = "";
        }
      
        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where code =@code",
                DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (reader.Read())
                    cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());
        }
        private void txtCustomerCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_employee where code =@code",
            DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (!reader.Read())
                    cmbCustomer.SelectedIndex = -1;
        }
        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["name"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxName_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxName_SelectedIndexChanged);
                }
            }
            else if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["vat"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxTax_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxTax_SelectedIndexChanged);
                }
            }
            else if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["cost_price"].Index|| dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["qty"].Index || dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["price"].Index)
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    txt.KeyPress -= Txt_KeyPress;
                    txt.KeyPress += Txt_KeyPress;
                }
            }
        }

        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true; // Block the input
            }

            // Allow only one decimal point
            TextBox txt = sender as TextBox;
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            } 
        }

        private void ComboBoxTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkRowValidty();
        }
        private void chkRowValidty()
        {
            decimal price = GetDecimalValue(dgvItems.CurrentRow, "cost_price");
            decimal qty = GetDecimalValue(dgvItems.CurrentRow, "qty");

            if (price == 0 || qty == 0)
                dgvItems.CurrentRow.Cells["total"].Value = dgvItems.CurrentRow.Cells["vatp"].Value = "0";
            else
            {
                DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)dgvItems.CurrentRow.Cells["vat"];
                if (comboCell.Value != null)
                {
                    string[] parts = comboCell.FormattedValue.ToString().Replace("%", "").Split('-');

                    dgvItems.CurrentRow.Cells["vatp"].Value = (decimal.Parse(dgvItems.CurrentRow.Cells["qty"].Value.ToString()) *
                     decimal.Parse(dgvItems.CurrentRow.Cells["cost_price"].Value.ToString()) * decimal.Parse(parts[1]) / 100);
                }
                else
                    dgvItems.CurrentRow.Cells["vatp"].Value = "0";

                dgvItems.CurrentRow.Cells["total"].Value = ((decimal.Parse(dgvItems.CurrentRow.Cells["qty"].Value.ToString()) *
                    decimal.Parse(dgvItems.CurrentRow.Cells["cost_price"].Value.ToString()))
                    + decimal.Parse(dgvItems.CurrentRow.Cells["vatp"].Value.ToString()));
            }
        }
        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
        }
        bool checkItemValidty(int itemId)
        {
            decimal qty = 0;
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id = @id",
                DBClass.CreateParameter("id", itemId)))
            {
                reader.Read();
                if (reader["type"].ToString() == "Service")
                {
                    MessageBox.Show("Item Service Can't Be Included In Damage Invoice");
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
                    return false;
                }
                for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                {
                    if (dgvItems.Rows[i].Cells["itemId"].Value == null)
                    {
                        dgvItems.Rows.Remove(dgvItems.Rows[i]);
                        continue;
                    }
                    if (dgvItems.Rows[i].Cells["itemId"].Value.ToString() == itemId.ToString())
                    {
                        if (dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "")
                            continue;
                        qty += decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
                        if (qty > decimal.Parse(reader["on_hand"].ToString()))
                        {
                            MessageBox.Show("Item Out Of Stock. Item has Only " + reader["on_hand"].ToString() + " On Hand");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        void CalculateTotal()
        {
            decimal total = 0, vat = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["total"].Value != null)
                    total += Convert.ToDecimal(row.Cells["total"].Value);
                if (row.Cells["vatp"].Value != null && row.Cells["vatp"].Value.ToString().Trim() != "")
                    vat += Convert.ToDecimal(row.Cells["vatp"].Value);

            }
            txtTotal.Text = total.ToString("0.000");
        }
        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];
                decimal price = GetDecimalValue(row, "price");
                decimal qty = GetDecimalValue(row, "qty");
                decimal costPrice = GetDecimalValue(row, "cost_price");
                if (e.ColumnIndex == dgvItems.Columns["price"].Index && dgvItems.Rows[e.RowIndex].Cells["type"].Value!=null && dgvItems.Rows[e.RowIndex].Cells["type"].Value.ToString()!="Service")
                {
                    if (price < costPrice)
                    {
                        MessageBox.Show("Sales Price Must Be Greater Than Cost Price");
                        row.Cells["price"].Value = costPrice;
                    }
                }
                else if (e.ColumnIndex == dgvItems.Columns["Code"].Index)
                {
                    string codeValue = row.Cells["Code"].Value?.ToString();
                    DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                    if (comboCell != null)
                        insertItemThroughCodeOrCombo("code", comboCell, null);
                }
                else if (e.ColumnIndex == dgvItems.Columns["qty"].Index)
                {
                    if (dgvItems.CurrentRow.Cells["itemId"].Value == null || !checkItemValidty(int.Parse(dgvItems.CurrentRow.Cells["itemId"].Value.ToString())))
                        row.Cells["qty"].Value = 0;
                }
                chkRowValidty();
                CalculateTotal();
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
        private void insertItemThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;
            try
            {
                if (type == "code")
                {
                    reader = DBClass.ExecuteReader(@"SELECT *
                  FROM tbl_items 
                  WHERE code = @code AND type = 'Inventory Part' AND  warehouse_id = @w",
                        DBClass.CreateParameter("code", dgvItems.CurrentRow.Cells["code"].Value.ToString()),
                        DBClass.CreateParameter("w", cmbWarehouse.SelectedValue.ToString()));
                }
                else if (type == "combo" && comboBox.SelectedValue != null)
                {
                    string selectedItemCode = comboBox.SelectedValue.ToString();
                    reader = DBClass.ExecuteReader(@"SELECT tbl_items.id,method,type, code,  sales_price,  cost_price 
                  FROM tbl_items 
                  WHERE warehouse_id = @id  and code = @code",
                        DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()),
                        DBClass.CreateParameter("code", selectedItemCode));
                }

                if (reader != null && reader.Read())
                {
                    dgvItems.CurrentRow.Cells["qty"].Value = 0;
                    dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                    dgvItems.CurrentRow.Cells["itemid"].Value = reader["id"].ToString();
                    dgvItems.CurrentRow.Cells["cost_price"].Value = reader["cost_price"].ToString();
                    dgvItems.CurrentRow.Cells["price"].Value = Convert.ToDecimal(reader["sales_price"]);
                    dgvItems.CurrentRow.Cells["method"].Value = reader["method"];
                    dgvItems.CurrentRow.Cells["type"].Value = reader["type"];
                    if (type == "code" && comboCell != null)
                        comboCell.Value = dgvItems.CurrentRow.Cells["code"].Value.ToString();
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

            string query = "select id from tbl_damage where state = 0 and id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clear();
                    MessageBox.Show("No previous record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

        }

        private void cmbAccountCashName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccountName.SelectedValue == null)
            {
                txtAccountCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAccountName.SelectedValue.ToString()))
                if (reader.Read())
                    txtAccountCode.Text = reader["code"].ToString();
                else
                    txtAccountCode.Text = "";
        }

        private void txtAccountCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtAccountCode.Text)))
                if (reader.Read())
                    cmbAccountName.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtAccountCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
           DBClass.CreateParameter("code", txtAccountCode.Text)))
                if (!reader.Read())
                    cmbAccountName.SelectedIndex = -1;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int? currentId = Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_damage WHERE state = 0 AND id =@id";
            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clear();
                    MessageBox.Show("No next record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void clear()
        {
            txtInvoiceId.Text = "";
            txtTotal.Text = "";
            dtInv.Value = DateTime.Now;
            cmbCustomer.SelectedIndex = -1;
            cmbAccountName.SelectedIndex = -1;
            cmbWarehouse.SelectedIndex = -1;
            txtDamageReason.Text = "";
            dgvItems.Rows.Clear();
            id = 0;
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
     
        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoaddgvItems();
        }
        private void FormatNumberWithCommas(Guna.UI2.WinForms.Guna2TextBox txtBox)
        {
            if (string.IsNullOrWhiteSpace(txtBox.Text))
                return;

            string rawText = txtBox.Text.Replace(",", "");

            decimal number;
            if (decimal.TryParse(rawText, out number))
            {
                int cursorPosition = txtBox.SelectionStart;
                txtBox.Text = number.ToString("N0");
                txtBox.SelectionStart = txtBox.Text.Length;
            }
        }
        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
