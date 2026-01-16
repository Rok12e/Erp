using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewItem : Form
    {

        int id; string itemCode;
        public List<int> unitIds = new List<int>();
        public List<decimal> unitFactors = new List<decimal>();
        public List<string> unitNames = new List<string>();
        private EventHandler itemCategoryUpdatedHandler;
        private EventHandler itemUnitUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;

        public frmViewItem(int id = 0)
        { 
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            itemCategoryUpdatedHandler = (sender, args) => setComboBoxCategory();
            itemUnitUpdatedHandler = (sender, args) => BindCombos.PopulateItemUnit(cmbMainUnit);
            warehouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            EventHub.ItemCategory += itemCategoryUpdatedHandler;
            EventHub.ItemUnit += itemUnitUpdatedHandler;
            EventHub.wareHouse += warehouseUpdatedHandler;

            this.id = id;
            this.Text = id != 0 ? "Inventory - Edit Item" : "Inventory - New Item";
            //btnSave.Text = id != 0 ? "Update" : btnSave.Text;
            dtStart.Value = DateTime.Now;
            headerUC1.FormText = this.Text;
        }
        private void frmViewItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.ItemCategory -= itemCategoryUpdatedHandler;
            EventHub.ItemUnit -= itemUnitUpdatedHandler;
            EventHub.wareHouse -= warehouseUpdatedHandler;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewItem_Load(object sender, EventArgs e)
        {
            this.Height = 765 - pnlAssembly.Height;
            bindCombo();
            bindItemAssembly();
            if (id != 0)
            {
                BindItem();
                btnSave.Enabled = UserPermissions.canEdit("Inventory Items");
            }
        }

        private void bindItemAssembly()
        {
            dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items where  state = 0 and active = 0");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
        }
        private void setComboBoxCategory()
        {
            BindCombos.PopulateItemsCategory(cmbCategory);
            SetSelectedIndexToLastItem(cmbCategory);
        }

        private void SetSelectedIndexToLastItem(Guna.UI2.WinForms.Guna2ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = comboBox.Items.Count - 1;
            }
        }

        public void bindCombo()
        {
            BindCombos.PopulateWarehouse(cmbWarehouse);
            BindCombos.PopulateItemsCategory(cmbCategory);
            BindCombos.PopulateItemUnit(cmbMainUnit);
            BindCombos.PopulateVendors(cmbVendorAccount);
            cmbVendorAccount.SelectedValue = -1;
            BindCombos.PopulateAllLevel4Account(cmbIncomeAccount);
            BindCombos.PopulateAllLevel4Account(cmbAssetAccount);
            BindCombos.PopulateAllLevel4Account(cmbCOGSAccount);
            cmbIncomeAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Sales");
            cmbAssetAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Inventory");
            cmbCOGSAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("COGS");
        }
        private void BindItem()
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id = " + id);
            if (reader.Read())
            {
                if (reader["method"].ToString() == "lifo")
                    RadLifo.Checked = true;
                else if (reader["method"].ToString() == "fifo")
                    radFifo.Checked = true;
                else
                    radAvg.Checked = true;
                //cmbCategory.Enabled = cmbType.Enabled = cmbWarehouse.Enabled = false;
                cmbCategory.SelectedValue = int.Parse(reader["category_id"].ToString());
                txtName.Text = reader["name"].ToString();
                txtBarcode.Text = reader["barcode"].ToString();
                txtCostPrice.Text = reader["cost_price"].ToString();
                txtSalesPrice.Text = reader["sales_price"].ToString();
                txtMin.Text = reader["min_amount"].ToString();
                txtMax.Text = reader["max_amount"].ToString();
                string OnHand = reader["on_hand"].ToString();
                txtTotalValue.Text = reader["total_value"].ToString();
                cmbType.Text = reader["type"].ToString();
                cmbMainUnit.SelectedValue = reader["unit_id"].ToString();
                txtcode.Text = itemCode = reader["code"].ToString();
                cmbCOGSAccount.SelectedValue = reader["cogs_account_id"].ToString();
                cmbVendorAccount.SelectedValue = reader["vendor_id"].ToString();
                cmbIncomeAccount.SelectedValue = reader["income_account_id"].ToString();
                cmbAssetAccount.SelectedValue = reader["asset_account_id"].ToString();
                cmbPurchasetype.Text = reader["Item_type"].ToString();
                dtStart.Value = DateTime.Parse(reader["date"].ToString());
                chkActive.Checked = (int.Parse(reader["active"].ToString()) == 0) ? true : false;
                txtCostPrice.Enabled = txtSalesPrice.Enabled = txtOnHand.Enabled = dtStart.Enabled = true;
                reader = DBClass.ExecuteReader("select tbl_items_unit.* , tbl_unit.name from tbl_items_unit  inner join tbl_unit on tbl_items_unit.unit_id = tbl_unit.id where item_id =@id", DBClass.CreateParameter("id", id));
                while (reader.Read())
                {
                    if (cmbMainUnit.SelectedValue != null && int.Parse(reader["unit_id"].ToString()) == int.Parse(cmbMainUnit.SelectedValue.ToString()))
                        continue;
                    unitIds.Add(int.Parse(reader["unit_id"].ToString()));
                    unitFactors.Add(decimal.Parse(reader["factor"].ToString()));
                    unitNames.Add(reader["name"].ToString());
                }
                reader = DBClass.ExecuteReader("select ti.code,ti.id,ta.qty,ti.cost_price from tbl_item_assembly ta inner join tbl_items ti on ta.item_id = ti.id where ta.assembly_id =@id", DBClass.CreateParameter("id", id));
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["id"].ToString(), "", reader["code"].ToString(), reader["code"].ToString(),reader["cost_price"].ToString(), reader["qty"].ToString());
                }

                object result0 = DBClass.ExecuteScalar(@"SELECT COUNT(1) from tbl_item_transaction where type != 'Opening Qty' and item_id = @id", DBClass.CreateParameter("id", id));
                int recordCount0 = 0;
                if (result0 != null && result0 != DBNull.Value)
                    recordCount0 = Convert.ToInt32(result0);
                if (recordCount0 > 0)
                {
                    txtOnHand.Enabled = false;
                    object result1 = DBClass.ExecuteScalar(@"SELECT IFNULL(qty_in,0) from tbl_item_transaction where type = 'Opening Qty' and item_id = @id", DBClass.CreateParameter("id", id));
                    decimal recordQty = 0;
                    if (result1 != null && result1 != DBNull.Value)
                        recordQty = Convert.ToInt32(result1);
                    if (recordQty > 0)
                    {
                        txtOnHand.Text = recordQty.ToString("N2");
                    }
                }
                else
                {
                    txtOnHand.Enabled = true;
                    txtOnHand.Text = Convert.ToDecimal(OnHand).ToString("N2");
                }
                
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertItem())
                {
                    EventHub.RefreshItem();
                    this.Close();
                }
            }
            else if (updateItem())
            {
                EventHub.RefreshItem();
                this.Close();
            }
        }
        void insertItemUnits(int id)
        {
            DBClass.ExecuteReader(@"insert into tbl_items_unit (item_id,unit_id,factor) values (@id,@unit_id,@factor)",
                   DBClass.CreateParameter("id", id),
                   DBClass.CreateParameter("unit_id", cmbMainUnit.SelectedValue),
                   DBClass.CreateParameter("factor", 1));
            Utilities.LogAudit(frmLogin.userId, "Insert Item Unit", "Item Unit", id, "Inserted Main Unit: " + cmbMainUnit.Text + " for Item: " + txtName.Text);
            if (unitFactors != null)
                for (int i = 0; i < unitFactors.Count; i++)
                {
                    DBClass.ExecuteReader(@"insert into tbl_items_unit (item_id,unit_id,factor) values (@id,@unit_id,@factor)",
                        DBClass.CreateParameter("id", id),
                        DBClass.CreateParameter("unit_id", unitIds[i]),
                        DBClass.CreateParameter("factor", unitFactors[i]));
                    Utilities.LogAudit(frmLogin.userId, "Insert Item Unit", "Item Unit", id, "Inserted Sub Unit: " + unitNames[i] + " for Item: " + txtName.Text);
                }
        }
        private bool updateItem()
        {
            if (!chkRequiredDate())
                return false;
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where name = @name",
               DBClass.CreateParameter("name", txtName.Text));
            if (reader.Read() && id != int.Parse(reader["id"].ToString()))
            {
                MessageBox.Show("Item Already Exists. Enter Another Name.");
                return false;
            }
            DBClass.ExecuteNonQuery(@"UPDATE tbl_items SET type = @type,category_id=@category,name = @name,unit_id = @unit_id,barcode = @barcode,
                              cost_price = @cost_price,cogs_account_id = @cogs_account_id,vendor_id = @vendor_id,sales_price = @sales_price,
                              income_account_id = @income_account_id,asset_account_id = @asset_account_id,
                              min_amount = @min_amount,max_amount = @max_amount,on_hand = @on_hand,method=@method,total_value = @total_value,
                              date = @date,img = @img,active = @active,state = @state,created_By = @created_By,created_date = @created_date,Item_type=@Item_type WHERE id = @id",
                            DBClass.CreateParameter("type", cmbType.Text),
                            DBClass.CreateParameter("name", txtName.Text),
                            DBClass.CreateParameter("category", cmbCategory.SelectedValue),
                             DBClass.CreateParameter("unit_id", cmbMainUnit.SelectedValue ?? 0),
                            DBClass.CreateParameter("barcode", txtBarcode.Text),
                            DBClass.CreateParameter("cost_price", txtCostPrice.Text == "" ? null : txtCostPrice.Text),
                             DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount.SelectedValue),
                             DBClass.CreateParameter("vendor_id", cmbVendorAccount.SelectedValue ?? 0),
                             DBClass.CreateParameter("sales_price", txtSalesPrice.Text == "" ? null : txtSalesPrice.Text),
                            DBClass.CreateParameter("income_account_id", cmbIncomeAccount.SelectedValue),
                            DBClass.CreateParameter("asset_account_id", cmbAssetAccount.SelectedValue),
                            DBClass.CreateParameter("min_amount", txtMin.Text),
                            DBClass.CreateParameter("max_amount", txtMax.Text),
                            DBClass.CreateParameter("on_hand", txtOnHand.Text),
                            DBClass.CreateParameter("method", radAvg.Checked ? "avg" : radFifo.Checked ? "fifo" : "lifo"),
                            DBClass.CreateParameter("total_value", txtTotalValue.Text),
                            DBClass.CreateParameter("date", dtStart.Value.Date),
                            DBClass.CreateParameter("img", ""),
                            DBClass.CreateParameter("active", chkActive.Checked ? 0 : 1),
                            DBClass.CreateParameter("state", 0),
                            DBClass.CreateParameter("created_By", frmLogin.userId),
                            DBClass.CreateParameter("created_date", DateTime.Now.Date),
                            DBClass.CreateParameter("id", id),
                            DBClass.CreateParameter("@Item_type", cmbPurchasetype.Text));
            DBClass.ExecuteNonQuery("delete from tbl_items_unit where item_id=@id", DBClass.CreateParameter("id", id));
            DBClass.ExecuteNonQuery("delete from tbl_item_assembly where assembly_id=@id", DBClass.CreateParameter("id", id));
            if (cmbType.Text != "12 - Service" && cmbMainUnit.SelectedValue != null)
                insertItemUnits(id);
            if (cmbType.Text == "13 - Inventory Assembly")
                insertItemsAssembly(id);

            //if (txtOnHand.Text.Trim() != "0" && cmbType.Text != "12 - Service")
            //{
                DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction where type = 'Opening Qty' and item_id = @id;DELETE FROM tbl_transaction where type = 'Opening Qty' AND transaction_id= @id", DBClass.CreateParameter("id", id));
                DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Opening Qty' and itemId = @id", DBClass.CreateParameter("id", id));
                
                insertItemTransaction();
                insertItemJournal();
            //}
            Utilities.LogAudit(frmLogin.userId, "Update Item", "Item", id, "Updated Item: " + txtName.Text + " with Code: " + itemCode);

            return true;
        }
        string getNextCode()
        {
            string typeCategory = cmbType.Text.Split('-')[0].Trim() + cmbCategory.Text.Split('-')[0].Trim();
            string query = "SELECT MAX(RIGHT(code, 4)) FROM tbl_items WHERE LEFT(code, 5)=@typeCategory";
            object result = DBClass.ExecuteScalar(query, DBClass.CreateParameter("@typeCategory", typeCategory));
            int nextSerial = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            string formattedSerial = nextSerial.ToString().PadLeft(4, '0');
            string newItemCode = typeCategory + formattedSerial;
            return newItemCode;
        }
        private bool insertItem()
        {
            if (!chkRequiredDate())
                return false;
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where name = @name",
               DBClass.CreateParameter("name", txtName.Text));
            if (reader.Read())
            {
                MessageBox.Show("Item Already Exists. Enter Another Name.");
                return false;
            }
            itemCode = getNextCode();
            id = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(`code`,`warehouse_id`,  `type`,category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                    `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                    `min_amount`, `max_amount`, `on_hand`,method, `total_value`, `date`, `img`, `active`, `state`, 
                                    `created_By`, `created_date`,Item_type) VALUES (
                                    @code, @warehouseId, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                    @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                    @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                    @created_By, @created_date,@Item_type); SELECT LAST_INSERT_ID();",
                             DBClass.CreateParameter("code", itemCode),
                             DBClass.CreateParameter("warehouseId", cmbWarehouse.SelectedValue.ToString()),
                             DBClass.CreateParameter("type", cmbType.Text),
                             DBClass.CreateParameter("category", cmbCategory.SelectedValue),
                             DBClass.CreateParameter("name", txtName.Text),
                             DBClass.CreateParameter("unit_id", cmbMainUnit.SelectedValue ?? 0),
                             DBClass.CreateParameter("barcode", txtBarcode.Text),
                             DBClass.CreateParameter("cost_price", txtCostPrice.Text == "" ? "0" : txtCostPrice.Text),
                             DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount.SelectedValue),
                             DBClass.CreateParameter("vendor_id", cmbVendorAccount.SelectedValue ?? 0),
                             DBClass.CreateParameter("sales_price", txtSalesPrice.Text == "" ? "0" : txtSalesPrice.Text),
                             DBClass.CreateParameter("income_account_id", cmbIncomeAccount.SelectedValue),
                             DBClass.CreateParameter("asset_account_id", cmbAssetAccount.SelectedValue),
                             DBClass.CreateParameter("min_amount", txtMin.Text),
                             DBClass.CreateParameter("max_amount", txtMax.Text),
                             DBClass.CreateParameter("on_hand", txtOnHand.Text),
                             DBClass.CreateParameter("method", radAvg.Checked ? "avg" : radFifo.Checked ? "fifo" : "lifo"),
                             DBClass.CreateParameter("total_value", txtTotalValue.Text),
                             DBClass.CreateParameter("date", dtStart.Value.Date),
                             DBClass.CreateParameter("img", ""),
                             DBClass.CreateParameter("active", chkActive.Checked ? 0 : 1),
                             DBClass.CreateParameter("state", 0),
                             DBClass.CreateParameter("created_By", frmLogin.userId),
                             DBClass.CreateParameter("created_date", DateTime.Now.Date),
                             DBClass.CreateParameter("@Item_type", cmbPurchasetype.Text)).ToString());
            if (cmbType.Text != "12 - Service" && cmbMainUnit.SelectedValue != null)
                insertItemUnits(id);
            if (cmbType.Text == "13 - Inventory Assembly")
                insertItemsAssembly(id);
            if (txtOnHand.Text.Trim() != "0" && cmbType.Text != "12 - Service")
            {
                insertItemTransaction();
                insertItemJournal();
            }
            Utilities.LogAudit(frmLogin.userId, "Insert Item", "Item", id, "Inserted Item: " + txtName.Text + " with Code: " + itemCode);
            return true;
        }

        private void insertItemsAssembly(int id)
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery("insert into tbl_item_assembly (assembly_id,item_id,qty) values (@assembly_id,@item_id,@qty)",
                    DBClass.CreateParameter("assembly_id", id),
                    DBClass.CreateParameter("item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                    DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
            }
        }

        private void insertItemTransaction()
        {
            CommonInsert.InsertItemTransaction(dtStart.Value.Date, "Opening Qty", "0", id.ToString(), txtCostPrice.Text,
                txtOnHand.Text, "0", "0", txtOnHand.Text, "Opening Balance",cmbWarehouse.SelectedValue.ToString());
        }
        private void insertItemJournal()
        {
            CommonInsert.InsertTransactionEntry(dtStart.Value.Date, BindCombos.SelectDefaultLevelAccount("Inventory").ToString(), txtTotalValue.Text, "0",
                id.ToString(), id.ToString(), "Opening Qty", "Opening Balance - Item Code - " + itemCode, frmLogin.userId, DateTime.Now.Date);
            CommonInsert.InsertTransactionEntry(dtStart.Value.Date, BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString(), "0", txtTotalValue.Text,
                    id.ToString(), "0", "Opening Qty", "Opening Balance Equity - Item Code - " + itemCode, frmLogin.userId, DateTime.Now.Date);
        }

        private bool chkRequiredDate()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Enter Item Name First.");
                txtName.Focus();
                return false;
            }
            if (cmbWarehouse.Text.Trim() == "")
            {
                MessageBox.Show("Enter Warehouse First");
                cmbWarehouse.Focus();
                return false;
            }
            if (cmbCategory.Text.Trim() == "")
            {
                MessageBox.Show("Enter Category First");
                cmbWarehouse.Focus();
                return false;
            }
            if (txtOnHand.Text != "" && txtCostPrice.Text == "")
            {
                MessageBox.Show("Enter Cost Price");
                return false;
            }

            if (cmbType.Text != "12 - Service")
            {

                if (dtStart.Value.Date > DateTime.Now.Date && txtOnHand.Text != "")
                {
                    MessageBox.Show("Date Must Be Less Or Equal Today");
                    return false;
                }

                //if (decimal.Parse(txtCostPrice.Text) >= decimal.Parse(txtSalesPrice.Text))
                //{
                //    MessageBox.Show("Cost Price Must Be Less Than Sale Price");
                //    return false;
                //}
                if (!Utilities.AreDefaultAccountsSet(new List<string> { "COGS", "Sales", "Inventory" }))
                {
                    MessageBox.Show("Default accounts for Item are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                foreach (int unit in unitIds)
                {
                    if (unit == (int)cmbMainUnit.SelectedValue)
                    {
                        MessageBox.Show("Main Unit Can't Be Included As Sub Unit");
                        return false;
                    }
                }
            }
            if (txtMin.Text.Trim() == "")
                txtMin.Text = "0";
            if (txtMax.Text.Trim() == "")
                txtMax.Text = "0";
            if (txtOnHand.Text.Trim() == "")
                txtOnHand.Text = "0";

            if (txtTotalValue.Text.Trim() == "")
                txtTotalValue.Text = "0";
            if (cmbType.Text == "13 - Inventory Assembly")
            {
                for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                {
                    if (dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["name"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "" || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "0" || dgvItems.Rows[i].Cells["code"].Value == null || dgvItems.Rows[i].Cells["code"].Value.ToString() == "")
                    {
                        MessageBox.Show("Item Assembly Can't be 0 or empty");
                        return false;
                    }
                }
            }


            return true;
        }

        private void resetTextBox()
        {
            txtcode.Text = "";
            txtName.Text = "";
            txtBarcode.Text = "";
            txtCostPrice.Text = "";
            txtSalesPrice.Text = "";
            txtMin.Text = "";
            txtMax.Text = "";
            txtOnHand.Text = "";
            dtStart.Value = DateTime.Now;
            dgvItems.Rows.Clear();
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertItem())
                    resetTextBox();
            }
            else
              if (updateItem())
            {
                id = 0;
                resetTextBox();
            }
            EventHub.RefreshItem();

        }
        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }
        private void txtCostPrice_TextChanged(object sender, EventArgs e)
        {
            
            decimal costPrice = string.IsNullOrWhiteSpace(txtCostPrice.Text) ? 0 : Convert.ToDecimal(txtCostPrice.Text);
            decimal OnHand = string.IsNullOrWhiteSpace(txtOnHand.Text) ? 0 : Convert.ToDecimal(txtOnHand.Text);

            decimal totalSalary = costPrice * OnHand;
            txtTotalValue.Text = totalSalary.ToString();
        }

        private void lnkAddItemUnit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewItemUnit(0));
            BindCombos.PopulateItemUnit(cmbMainUnit, false, true);
        }



        private void lnkNewWarehouse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewWarehouse());
            BindCombos.PopulateWarehouse(cmbWarehouse, false, true);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.Text == "12 - Service")
            {
                this.Height = 765 - pnlAssembly.Height - pnlOP.Height - pnlSub.Height;
                pnlSub.Visible = pnlAssembly.Visible = pnlOP.Visible = false;
            }
            else if (cmbType.Text == "11 - Inventory Part")
            {
                txtCostPrice.Enabled = true;
                this.Height = 765 - pnlAssembly.Height;
                pnlAssembly.Visible = false;
                pnlSub.Visible = pnlOP.Visible = true;
            }
            else if (cmbType.Text == "13 - Inventory Assembly")
            {
                txtCostPrice.Enabled = false;
                this.Height = 765;
                pnlSub.Visible = pnlAssembly.Visible = pnlOP.Visible = true;
            }
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex && !dgvItems.CurrentRow.IsNewRow)
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
            } catch(Exception ex)
            {
                ex.ToString();
            }
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];

                if (e.ColumnIndex == dgvItems.Columns["Code"].Index)
                {
                    string codeValue = row.Cells["Code"].Value?.ToString();
                    DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                    if (comboCell != null)
                        insertItemThroughCodeOrCombo("code", comboCell, null);
                }
                calculateCost();
            }
        }

        private void insertItemThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;
            if (type == "code")
            {
                reader = DBClass.ExecuteReader(@"SELECT *
                  FROM tbl_items 
                  WHERE code = @code ",
                    DBClass.CreateParameter("code", dgvItems.CurrentRow.Cells["code"].Value.ToString()));
            }
            else if (type == "combo" && comboBox.SelectedValue != null && !comboBox.SelectedValue.ToString().ToLower().Contains("rowview"))
            {
                string selectedItemCode = comboBox.SelectedValue.ToString();
                reader = DBClass.ExecuteReader(@"SELECT tbl_items.id,method,type, code,  sales_price,  cost_price 
                  FROM tbl_items 
                  WHERE  code = @code",
                    DBClass.CreateParameter("code", selectedItemCode));
            }
            if (reader != null && reader.Read())
            {
                dgvItems.CurrentRow.Cells["qty"].Value = "1";
                dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                dgvItems.CurrentRow.Cells["price"].Value = reader["cost_price"].ToString();
                dgvItems.CurrentRow.Cells["itemid"].Value = reader["id"].ToString();
                if (type == "code" && comboCell != null)
                    comboCell.Value = dgvItems.CurrentRow.Cells["code"].Value.ToString();
            }
            else
            {
                if (!dgvItems.CurrentRow.IsNewRow)
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
            }
        }
        decimal totalCost = 0;
        private void calculateCost()
        {
            for (int i = 0; i < dgvItems.Rows.Count; i++)
            {
                if (dgvItems.Rows[i].Cells["price"].Value == null || dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["price"].Value.ToString() == "" || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "")
                    continue;
                totalCost += decimal.Parse(dgvItems.Rows[i].Cells["price"].Value.ToString()) * decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
            }
            txtCostPrice.Text = totalCost.ToString();
            totalCost = 0;
        }

        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
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
            else if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["qty"].Index|| dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["price"].Index)
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

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lnkCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewCategory(0));
            BindCombos.PopulateItemsCategory(cmbCategory, false, true);
        }
        
        private void txtTotalValue_TextChanged(object sender, EventArgs e)
        {
       
        }

        private void txtSalesPrice_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            this.BringToFront();    
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
