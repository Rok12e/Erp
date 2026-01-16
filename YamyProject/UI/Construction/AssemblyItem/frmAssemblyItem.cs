using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAssemblyItem : Form
    {

        int id, refId;
        string WarehouseId = "0",UnitId="0",CategoryID="0";
        string mainItemCode;
        string Barcode="",Type="";
        string CostPrice = "0";
        string SalesPrice = "0";
        string TaxCodeId = "0";
        string MinAmount = "0";
        string MaxAmount = "0";
        string OnHand = "0";
        string TotalValue = "0";
        string Method = "fifo";
        string Dated = "2025-01-01";
        int genCode = 0;
        string description;
        public event EventHandler FormClosedWithResult;

        public frmAssemblyItem(int _id = 0, string _mainItemCode = "0", int _refId=0,string _description="")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = _id;
            this.mainItemCode = _mainItemCode;
            this.refId = _refId;
            this.description = _description;
            this.Text = _id != 0 ? "Assembly - Edit Item" : "Assembly - New Item";
            //btnSave.Text = _id != 0 ? "Update" : btnSave.Text;
            headerUC1.FormText = this.Text;
        }
        private void frmAssemblyItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            //EventHub.ItemCategory -= itemCategoryUpdatedHandler;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            FormClosedWithResult?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
        private void frmAssemblyItem_Load(object sender, EventArgs e)
        {
            pnlAssembly.Height = 190;
            this.Height = 665 - pnlAssembly.Height;

            if (!string.IsNullOrEmpty(description))
            {
                txtName.Text = description;
            }
            if (!string.IsNullOrEmpty(mainItemCode))
            {
                txtcode.Text = mainItemCode;
            }
            bindCombo();
            bindItemList();
            pnlAssembly.Visible = true;
            if (id != 0)
                BindItem();
        }
        private void bindItemList()
        {
            if (AssemblyItemManager.GetItemsList().Count > 0)
            {
                dgvItems.Rows.Clear();
                var result = AssemblyItemManager.GetItemsListWhere(mainItemCode.ToString());
                int no = 1;
                foreach (var item in result)
                {
                    dgvItems.Rows.Add(item.Code, (no++).ToString(), item.Code, item.Name, item.Cost, item.Qty, (decimal.Parse(item.Total.ToString())).ToString("N2"));

                    //txtCostPrice.Enabled = false;
                    //this.Height = 665;
                    //pnlAssembly.Visible = true;
                    calculateCost();
                }
            }
        }

        public void bindCombo()
        {
            BindCombos.PopulateVendors(cmbVendorAccount);
            cmbVendorAccount.SelectedValue = -1;
            BindCombos.PopulateAllLevel4Account(cmbIncomeAccount);
            BindCombos.PopulateAllLevel4Account(cmbAssetAccount);
            BindCombos.PopulateAllLevel4Account(cmbCOGSAccount);
            cmbIncomeAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Item Income");
            cmbAssetAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Inventory");
            cmbCOGSAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("COGS");
        }
        private void BindItem()
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id = " + id);
            if (reader.Read())
            {
                txtName.Text = reader["name"].ToString();
                cmbCOGSAccount.SelectedValue = reader["cogs_account_id"].ToString();
                cmbVendorAccount.SelectedValue = reader["vendor_id"].ToString();
                cmbIncomeAccount.SelectedValue = reader["income_account_id"].ToString();
                cmbAssetAccount.SelectedValue = reader["asset_account_id"].ToString();
                WarehouseId = reader["warehouse_id"].ToString();
                Type = reader["type"].ToString();
                txtcode.Text = reader["code"].ToString();
                UnitId = reader["unit_id"].ToString();
                Barcode = reader["barcode"].ToString();
                CostPrice = reader["cost_price"].ToString();
                SalesPrice = reader["sales_price"].ToString();
                TaxCodeId = reader["tax_code_id"].ToString();
                MinAmount = reader["min_amount"].ToString();
                MaxAmount = reader["max_amount"].ToString();
                OnHand = reader["on_hand"].ToString();
                TotalValue = reader["total_value"].ToString();
                Method = reader["method"].ToString();
                Dated = reader["date"].ToString();
                CategoryID = reader["category_id"].ToString();
                int no = 1;
                reader = DBClass.ExecuteReader("select ti.code,ti.id,ta.qty,ti.cost_price rate from tbl_item_assembly ta inner join tbl_items ti on ta.item_id = ti.id where ta.assembly_id=@id", DBClass.CreateParameter("id", id));
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["id"].ToString(), (no++).ToString(), reader["code"].ToString(), reader["code"].ToString(), reader["rate"].ToString(), reader["qty"].ToString(),(decimal.Parse(reader["rate"].ToString()) * decimal.Parse(reader["qty"].ToString())).ToString("N2"));
                }
                //txtCostPrice.Enabled = false;
                this.Height = 665;
                pnlAssembly.Visible = true;
                calculateCost();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertItems())
                {
                    FormClosedWithResult?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
            }
            else
               if (updateItems())
            {
                FormClosedWithResult?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
        }
        private bool insertItems()
        {
            if(!chkRequiredDate())
                return false;

            int count = 1;
            string _ref = txtcode.Text.ToString();
            AssemblyItemManager.RemoveItemByRefId(_ref);
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (!row.IsNewRow)
                {
                    AssemblyItemManager.AddItem(new AssemblyItemModel
                    {
                        ItemId = count,
                        RefId = _ref,
                        No = "",
                        Code = row.Cells["Code"].Value.ToString(),
                        Name = row.Cells["Name"].Value.ToString(),
                        Cost = Convert.ToDecimal(row.Cells["cost"].Value),
                        Qty = Convert.ToInt32(row.Cells["Qty"].Value),
                        Total = Convert.ToDecimal(row.Cells["Total"].Value),
                        AssetAccountId = Convert.ToInt32((cmbAssetAccount.SelectedValue??0)),
                        COGSAccountId = Convert.ToInt32((cmbCOGSAccount.SelectedValue??0)),
                        IncomeAccountId = Convert.ToInt32((cmbIncomeAccount.SelectedValue ?? 0)),
                        VendorAccountId = Convert.ToInt32((cmbVendorAccount.SelectedValue ?? 0))
                    });
                }
            }
            return true;
        }
        private bool updateItems()
        {
            return true;
        }
        private bool updateItem()
        {
            if (!chkRequiredDate())
                return false;
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                string _itemId = dgvItems.Rows[i].Cells["itemId"].Value.ToString();
                decimal _qty = decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
                decimal _price = decimal.Parse(dgvItems.Rows[i].Cells["cost"].Value.ToString());
                decimal _costPrice = _qty * _price;

                DBClass.ExecuteNonQuery(@"UPDATE tbl_items SET cost_price = @cost_price,cogs_account_id = @cogs_account_id,vendor_id = @vendor_id,sales_price = @sales_price,
                              income_account_id = @income_account_id,asset_account_id = @asset_account_id,
                              created_By = @created_By,created_date = @created_date WHERE id = @id",
                                DBClass.CreateParameter("cost_price", _price),
                                DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount.SelectedValue),
                                DBClass.CreateParameter("vendor_id", cmbVendorAccount.SelectedValue ?? 0),
                                DBClass.CreateParameter("sales_price", (_price+1)),
                                DBClass.CreateParameter("income_account_id", cmbIncomeAccount.SelectedValue),
                                DBClass.CreateParameter("asset_account_id", cmbAssetAccount.SelectedValue),
                                DBClass.CreateParameter("created_By", frmLogin.userId),
                                DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                DBClass.CreateParameter("id", _itemId));
                Utilities.LogAudit(frmLogin.userId, "Update Item Assembly", "Item", int.Parse(_itemId), "Updated Item: " + dgvItems.Rows[i].Cells["name"].Value + " with Code: " + dgvItems.Rows[i].Cells["code"].Value);
            }
            DBClass.ExecuteNonQuery(@"UPDATE tbl_items SET warehouse_id = @warehouse_id,type = @type,category_id=@category,name = @name,unit_id = @unit_id,barcode = @barcode,
                              cost_price = @cost_price,cogs_account_id = @cogs_account_id,vendor_id = @vendor_id,sales_price = @sales_price,
                              income_account_id = @income_account_id,asset_account_id = @asset_account_id,
                              min_amount = @min_amount,max_amount = @max_amount,on_hand = @on_hand,method=@method,total_value = @total_value,
                              date = @date,img = @img,active = @active,state = @state,created_By = @created_By,created_date = @created_date WHERE id = @id",
                            DBClass.CreateParameter("warehouse_id", WarehouseId),
                            DBClass.CreateParameter("type", Type),
                            DBClass.CreateParameter("name", txtName.Text),
                            DBClass.CreateParameter("category", CategoryID),
                            DBClass.CreateParameter("unit_id", UnitId),
                            DBClass.CreateParameter("barcode", Barcode),
                            DBClass.CreateParameter("cost_price", txtCostPrice.Text),
                            DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount.SelectedValue),
                            DBClass.CreateParameter("vendor_id", cmbVendorAccount.SelectedValue ?? 0),
                            DBClass.CreateParameter("sales_price", SalesPrice),
                            DBClass.CreateParameter("income_account_id", cmbIncomeAccount.SelectedValue),
                            DBClass.CreateParameter("asset_account_id", cmbAssetAccount.SelectedValue),
                            DBClass.CreateParameter("min_amount", MinAmount),
                            DBClass.CreateParameter("max_amount", MaxAmount),
                            DBClass.CreateParameter("on_hand", OnHand),
                            DBClass.CreateParameter("method", Method),
                            DBClass.CreateParameter("total_value", TotalValue),
                            DBClass.CreateParameter("date", DateTime.Parse(Dated)),
                            DBClass.CreateParameter("img", ""),
                            DBClass.CreateParameter("active", 0),
                            DBClass.CreateParameter("state", 0),
                            DBClass.CreateParameter("created_By", frmLogin.userId),
                            DBClass.CreateParameter("created_date", DateTime.Now.Date),
                            DBClass.CreateParameter("id", id));
            DBClass.ExecuteNonQuery("delete from tbl_items_unit where item_id=@id", DBClass.CreateParameter("id", id));
            DBClass.ExecuteNonQuery("delete from tbl_item_assembly where assembly_id=@id", DBClass.CreateParameter("id", id));
            
            if (Type == "13 - Inventory Assembly")
                insertItemsAssembly(id);

            Utilities.LogAudit(frmLogin.userId, "Update Item Assembly", "Item", id, "Updated Item: " + txtName.Text + " with Code: " + txtcode.Text);

            return true;
        }
        string getNextCodeProject()
        {
            string baseCode = refId + "00";

            //MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT code FROM tbl_items ORDER BY 
            //    CAST(SUBSTRING_INDEX(code, '-', -1) AS UNSIGNED) DESC LIMIT 1; ");

            string newItemCode = baseCode + "1";

            return newItemCode;
        }
        string getNextCodeX()
        {
            int baseCode=0;

            MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT code FROM tbl_items ORDER BY 
                CAST(SUBSTRING_INDEX(code, '-', -1) AS UNSIGNED) DESC LIMIT 1; ");
            if (reader.Read() && reader["code"].ToString() != "")
                baseCode = int.Parse(reader["code"].ToString()) + 1;
            return baseCode.ToString();
        }
        private bool insertItem()
        {
            
            if (!chkRequiredDate())
                return false;

            MySqlDataReader reader1 = DBClass.ExecuteReader("select * from tbl_items where name = @name",
                       DBClass.CreateParameter("name", txtName.Text));
            decimal assemblyId = 0;
            if (!reader1.Read())
            {
                assemblyId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(`code`, `warehouse_id`, `type`,category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                        `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                        `min_amount`, `max_amount`, `on_hand`,method, `total_value`, `date`, `img`, `active`, `state`, 
                                        `created_By`, `created_date`) VALUES (
                                        @code, @warehouse_id, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                        @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                        @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                        @created_By, @created_date); SELECT LAST_INSERT_ID();",
                                         DBClass.CreateParameter("code", txtcode.Text),
                                         DBClass.CreateParameter("warehouse_id", WarehouseId),
                                         DBClass.CreateParameter("type", "13 - Inventory Assembly"),
                                         DBClass.CreateParameter("category", CategoryID),
                                         DBClass.CreateParameter("name", txtName.Text),
                                         DBClass.CreateParameter("unit_id", UnitId),
                                         DBClass.CreateParameter("barcode", ""),
                                         DBClass.CreateParameter("cost_price", decimal.Parse(txtCostPrice.Text.ToString())),
                                         DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount.SelectedValue),
                                         DBClass.CreateParameter("vendor_id", cmbVendorAccount.SelectedValue ?? 0),
                                         DBClass.CreateParameter("sales_price", 0),
                                         DBClass.CreateParameter("income_account_id", cmbIncomeAccount.SelectedValue),
                                         DBClass.CreateParameter("asset_account_id", cmbAssetAccount.SelectedValue),
                                         DBClass.CreateParameter("min_amount", 0),
                                         DBClass.CreateParameter("max_amount", 0),
                                         DBClass.CreateParameter("on_hand", 0),
                                         DBClass.CreateParameter("method", "fifo"),
                                         DBClass.CreateParameter("total_value", 0),
                                         DBClass.CreateParameter("date", DateTime.Now.Date),
                                         DBClass.CreateParameter("img", ""),
                                         DBClass.CreateParameter("active", 0),
                                         DBClass.CreateParameter("state", 0),
                                         DBClass.CreateParameter("created_By", frmLogin.userId),
                                         DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());

                Utilities.LogAudit(frmLogin.userId, "Add Item Assembly", "Item", int.Parse(assemblyId.ToString()), "Added Item: " + txtName.Text + " with Code: " + txtcode.Text);
            }
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                string _itemId = dgvItems.Rows[i].Cells["itemId"].Value == null ? "0" : dgvItems.Rows[i].Cells["itemId"].Value.ToString();
                string _code = dgvItems.Rows[i].Cells["code"].Value == null ? "0" : dgvItems.Rows[i].Cells["code"].Value.ToString();
                string _qty = dgvItems.Rows[i].Cells["qty"].Value == null ? "0" : dgvItems.Rows[i].Cells["qty"].Value.ToString();
                string _costPrice = dgvItems.Rows[i].Cells["cost"].Value == null ? "0" : dgvItems.Rows[i].Cells["cost"].Value.ToString();
                string _itemName = dgvItems.Rows[i].Cells["name"].Value == null ? "" : dgvItems.Rows[i].Cells["name"].Value.ToString();

                //0, refId.ToString(), int.Parse(WarehouseId), int.Parse(CategoryID), int.Parse(UnitId), int.Parse(cmbCOGSAccount.SelectedValue == null ? "0" : cmbCOGSAccount.SelectedValue.ToString()), int.Parse(cmbVendorAccount.SelectedValue == null ? "0" : cmbVendorAccount.SelectedValue.ToString()), int.Parse(cmbIncomeAccount.SelectedValue == null ? "0" : cmbIncomeAccount.SelectedValue.ToString()), int.Parse(cmbAssetAccount.SelectedValue == null ? "0" : cmbAssetAccount.SelectedValue.ToString()),dgvItems.Rows.Count - 1);

                if (_itemName != "") { 
                MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where name = @name",
                       DBClass.CreateParameter("name", _itemName));
                if (!reader.Read())
                {
                    //MessageBox.Show("Item Already Exists. Enter Another Name.");
                    //return false;

                    decimal itemId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(`code`, `warehouse_id`, `type`,category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                        `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                        `min_amount`, `max_amount`, `on_hand`,method, `total_value`, `date`, `img`, `active`, `state`, 
                                        `created_By`, `created_date`) VALUES (
                                        @code, @warehouse_id, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                        @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                        @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                        @created_By, @created_date); SELECT LAST_INSERT_ID();",
                                     DBClass.CreateParameter("code", _code),
                                     DBClass.CreateParameter("warehouse_id", WarehouseId),
                                     DBClass.CreateParameter("type", "11 - Inventory Part"),
                                     DBClass.CreateParameter("category", CategoryID),
                                     DBClass.CreateParameter("name", _itemName),
                                     DBClass.CreateParameter("unit_id", UnitId),
                                     DBClass.CreateParameter("barcode", ""),
                                     DBClass.CreateParameter("cost_price", _costPrice),
                                     DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount.SelectedValue),
                                     DBClass.CreateParameter("vendor_id", cmbVendorAccount.SelectedValue ?? 0),
                                     DBClass.CreateParameter("sales_price", 0),
                                     DBClass.CreateParameter("income_account_id", cmbIncomeAccount.SelectedValue),
                                     DBClass.CreateParameter("asset_account_id", cmbAssetAccount.SelectedValue),
                                     DBClass.CreateParameter("min_amount", 0),
                                     DBClass.CreateParameter("max_amount", 0),
                                     DBClass.CreateParameter("on_hand", _qty),
                                     DBClass.CreateParameter("method", "fifo"),
                                     DBClass.CreateParameter("total_value", 0),
                                     DBClass.CreateParameter("date", DateTime.Now.Date),
                                     DBClass.CreateParameter("img", ""),
                                     DBClass.CreateParameter("active", 0),
                                     DBClass.CreateParameter("state", 0),
                                     DBClass.CreateParameter("created_By", frmLogin.userId),
                                     DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
                        //if (cmbType.Text != "12 - Service")
                        //    insertItemUnits(id);
                        //if (cmbType.Text == "13 - Inventory Assembly")
                        dgvItems.Rows[i].Cells["itemId"].Value = itemId;

                        DBClass.ExecuteNonQuery("insert into tbl_item_assembly (assembly_id,item_id,qty) values (@assembly_id,@item_id,@qty)",
                    DBClass.CreateParameter("assembly_id", int.Parse(assemblyId.ToString())),
                    DBClass.CreateParameter("item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                    DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
                        //if (txtOnHand.Text.Trim() != "0" && cmbType.Text != "12 - Service")
                        //{
                        //    insertItemTransaction();
                        //    insertItemJournal();
                        //}

                        Utilities.LogAudit(frmLogin.userId, "Add Item Assembly", "Item", int.Parse(assemblyId.ToString()), "Added Item: " + _itemName + " with Code: " + _code);
                    }
                }
            }

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
            //CommonInsert.InsertItemTransaction(dtStart.Value.Date, "Journal Voucher", "0", id.ToString(), txtCostPrice.Text,
            //    txtOnHand.Text, "0", "0", txtOnHand.Text, "Opening Balance");
        }
        private void insertItemJournal()
        {
            //CommonInsert.InsertTransactionEntry(dtStart.Value.Date, BindCombos.SelectDefaultLevelAccount("Opening Balance").ToString(), txtTotalValue.Text, "0",
            //    id.ToString(), id.ToString(), "Opening Balance", "Opening Balance - Item Code - " + txtcode.Text, frmLogin.userId, DateTime.Now.Date);
            //CommonInsert.InsertTransactionEntry(dtStart.Value.Date, BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString(), "0", txtTotalValue.Text,
            //        id.ToString(), "0", "Opening Balance", "Opening Balance Equity - Item Code - " + txtcode.Text, frmLogin.userId, DateTime.Now.Date);
        }

        private bool chkRequiredDate()
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Enter Item Name First.");
                txtName.Focus();
                return false;
            }
            
                for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                {
                    if (dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["name"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "" || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "0" || dgvItems.Rows[i].Cells["code"].Value == null || dgvItems.Rows[i].Cells["code"].Value.ToString() == "")
                    {
                        MessageBox.Show("Item Assembly Can't be 0 or empty");
                        return false;
                    }
                }
            

            return true;
        }

        private void resetTextBox()
        {
            txtcode.Text = "";
            txtName.Text = "";
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
            //EventHub.RefreshItem();

        }
        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }
        private void txtCostPrice_TextChanged(object sender, EventArgs e)
        {
            //decimal costPrice = string.IsNullOrWhiteSpace(txtCostPrice.Text) ? 0 : Convert.ToDecimal(txtCostPrice.Text);
            //decimal OnHand = string.IsNullOrWhiteSpace(txtOnHand.Text) ? 0 : Convert.ToDecimal(txtOnHand.Text);

            //decimal totalSalary = costPrice * OnHand;
            //txtTotalValue.Text = totalSalary.ToString();
        }

        private void lnkAddItemUnit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //new frmAddUnitsToItem(this).Show();
        }



        private void lnkNewWarehouse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewWarehouse().ShowDialog();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbType.Text == "13 - Inventory Assembly")
            //{
            //    txtCostPrice.Enabled = false;
            //    this.Height = 765;
            //    pnlSub.Visible = pnlAssembly.Visible = pnlOP.Visible = true;
            //}
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex && !dgvItems.CurrentRow.IsNewRow)
                dgvItems.Rows.Remove(dgvItems.CurrentRow);
                calculateCost();
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];

                if (e.ColumnIndex == dgvItems.Columns["code"].Index)
                {
                    string codeValue = row.Cells["code"].Value?.ToString();
                    //DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                    //if (comboCell != null)
                    //    insertItemThroughCodeOrCombo("code", comboCell, null);
                }
                else if (e.ColumnIndex == dgvItems.Columns["name"].Index)
                {
                    string nameValue = row.Cells["name"].Value == null ? "" : row.Cells["name"].Value.ToString();
                    string codeValue = row.Cells["code"].Value == null ? "" : row.Cells["code"].Value.ToString();
                    if (nameValue.Length>0&& codeValue.Length<=0)
                    {
                        row.Cells["code"].Value = refId + FormatItemSl(dgvItems.Rows.Count-1).ToString();
                    }
                }
                    decimal cost = dgvItems.CurrentRow.Cells["cost"].Value==null ? 0: decimal.Parse(dgvItems.CurrentRow.Cells["cost"].Value.ToString());
                decimal qty = dgvItems.CurrentRow.Cells["qty"].Value==null? 0 : decimal.Parse(dgvItems.CurrentRow.Cells["qty"].Value.ToString());

                if (cost == 0 || qty == 0)
                    dgvItems.CurrentRow.Cells["total"].Value = "0";
                else
                {
                    decimal totalAmount = qty * cost;
                    dgvItems.CurrentRow.Cells["total"].Value = totalAmount.ToString("N2");
                }
                calculateCost();
            }
        }

        private void insertItemThroughCodeOrComboX(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;
            if (type == "code")
            {
                reader = DBClass.ExecuteReader(@"SELECT *
                  FROM tbl_items 
                  WHERE code = @code AND warehouse_id = @w",
                    DBClass.CreateParameter("code", dgvItems.CurrentRow.Cells["code"].Value.ToString()),
                    DBClass.CreateParameter("w", WarehouseId.ToString()));
            }
            else if (type == "combo" && comboBox.SelectedValue != null && !comboBox.SelectedValue.ToString().ToLower().Contains("rowview"))
            {
                string selectedItemCode = comboBox.SelectedValue.ToString();
                reader = DBClass.ExecuteReader(@"SELECT tbl_items.id,method,type, code,  sales_price,  cost_price 
                  FROM tbl_items 
                  WHERE warehouse_id = @id AND code = @code",
                    DBClass.CreateParameter("id", WarehouseId.ToString()),
                    DBClass.CreateParameter("code", selectedItemCode));
            }
            int rowCount = dgvItems.Rows.Count-1;
            if (reader != null && reader.Read())
            {
                dgvItems.CurrentRow.Cells["no"].Value = (rowCount++).ToString();
                dgvItems.CurrentRow.Cells["qty"].Value = "1";
                dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                dgvItems.CurrentRow.Cells["cost"].Value = reader["cost_price"].ToString();
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
                if (dgvItems.Rows[i].Cells["cost"].Value == null || dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["cost"].Value.ToString() == "" || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "")
                    continue;
                totalCost += decimal.Parse(dgvItems.Rows[i].Cells["cost"].Value.ToString()) * decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
            }
            txtCostPrice.Text = totalCost.ToString("#.##");
            totalCost = 0;
        }

        int comboCount = 0;

        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //ComboBox comboBox = sender as ComboBox;
            //if (comboBox != null && comboBox.SelectedValue != null)
            //{
            //    if (comboBox.Text == "<< Add >>")
            //    {
            //        if (comboCount == 0)
            //        {
            //            //string srId = "1.0";
            //            //int itemId = 0;
            //            //if (dgvItems.Rows.Count > 1)
            //            //{
            //            //    int rowIndex = selectedRow.Index - 1;
            //            //    itemId = int.Parse(dgvItems.Rows[rowIndex].Cells[0].Value.ToString());
            //            //    srId = dgvItems.Rows[rowIndex].Cells[2].Value.ToString();
            //            //    itemTyped = dgvItems.Rows[rowIndex].Cells["type"].Value.ToString();
            //            //    selectedItemId = itemId.ToString();
            //            //}
            //            comboCount++;
            //            frmAddItem addForm = new frmAddItem(0, refId.ToString(), int.Parse(WarehouseId), int.Parse(CategoryID), int.Parse(UnitId), int.Parse(cmbCOGSAccount.SelectedValue == null ? "0" : cmbCOGSAccount.SelectedValue.ToString()), int.Parse(cmbVendorAccount.SelectedValue == null ? "0" : cmbVendorAccount.SelectedValue.ToString()), int.Parse(cmbIncomeAccount.SelectedValue == null ? "0" : cmbIncomeAccount.SelectedValue.ToString()), int.Parse(cmbAssetAccount.SelectedValue == null ? "0" : cmbAssetAccount.SelectedValue.ToString()),dgvItems.Rows.Count-1);
            //            //addForm.FormClosedWithResult += AddForm_FormClosedWithResult;
            //            addForm.ShowDialog();

            //            DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items where state = 0 and active = 0");
            //            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            //            DataRow newRow = dt.NewRow();
            //            newRow["code"] = 0;
            //            newRow["name"] = "<< Add >>";
            //            dt.Rows.InsertAt(newRow, 0);
            //            name.DataSource = dt;
            //            name.DisplayMember = "name";
            //            name.ValueMember = "code";
            //            comboBox.SelectedIndex = comboBox.Items.Count - 1;
            //        }
            //        else
            //            comboCount = 0;
            //    }
            //    else
            //        insertItemThroughCodeOrCombo("combo", null, comboBox);
            //}
        }
        //private void AddForm_FormClosedWithResult(object sender, EventArgs e)
        //{
        //    BindItem();
        //}

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
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lnkCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewCategory(0).ShowDialog();
        }
        public static string FormatItemSl(int itemSl)
        {
            string itemSlStr = itemSl.ToString();

            if (itemSlStr.Length == 1)
            {
                return $"00{itemSlStr}";
            }
            else if (itemSlStr.Length == 2)
            {
                return $"01{itemSlStr[0]}";
            }
            else if (itemSlStr.Length == 3)
            {
                return $"{itemSlStr[0]}{itemSlStr[1]}";
            }
            else
            {
                return itemSlStr;
            }
        }
    }
}

public class AssemblyItemModel
{
    public string RefId { get; set; }
    public int ItemId { get; set; }
    public string No { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal Cost { get; set; }
    public decimal Qty { get; set; }
    public decimal Total { get; set; }
    public int AssetAccountId { get; set; }
    public int IncomeAccountId { get; set; }
    public int VendorAccountId { get; set; }
    public int COGSAccountId { get; set; }
}


public static class AssemblyItemManager
{
    private static List<AssemblyItemModel> _itemsList = new List<AssemblyItemModel>();

    public static List<AssemblyItemModel> GetItemsList()
    {
        return _itemsList;
    }
    public static List<AssemblyItemModel> GetItemsListWhere(string refId)
    {
        return _itemsList.Where(item => item.RefId == refId).ToList();
    }

    public static void AddItem(AssemblyItemModel item)
    {
        _itemsList.Add(item);
    }
    public static void RemoveItemByRefId(string refId)
    {
        int removedCount = _itemsList.RemoveAll(item => item.RefId == refId);

        if (removedCount > 0)
        {
            Console.WriteLine($"{removedCount} item(s) with RefId {refId} have been removed.");
        }
        else
        {
            Console.WriteLine($"No items with RefId {refId} found.");
        }
    }

    public static void ClearItemsList()
    {
        _itemsList.Clear();
    }

    public static void PrintItemsList()
    {
        foreach (var item in _itemsList)
        {
            Console.WriteLine($"RefId: {item.RefId}, Name: {item.Name}, Cost: {item.Cost}");
        }
    }
}