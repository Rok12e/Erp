using CrystalDecisions.CrystalReports.Engine;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmPurchaseReturn : Form
    {
        private EventHandler vendorUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;

        decimal invId;
        int level4PaymentCreditMethodId, level4VatId, level4PurchaseReturnId;
        int id;

        public frmPurchaseReturn(int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            vendorUpdatedHandler = (sender, args) => BindCombos.PopulateVendors(cmbVendor);
            warehouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            EventHub.wareHouse += warehouseUpdatedHandler;
            EventHub.Vendor += vendorUpdatedHandler;
            this.id = id;
            if (id != 0)
                this.Text = "Purchase Return- Edit";
            else
                this.Text = "Purchase Return- New";
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmPurchaseReturn_Load(object sender, EventArgs e)
        {
            dtPaymentTerms.Value = dtInv.Value = dtShip.Value = DateTime.Now.Date;
            BindCombos.PopulateWarehouse(cmbWarehouse);
            cmbPaymentMethod.SelectedIndex = 0;
            bindCombo();
            loadCostCenter();
            loadTaxCombo();
            txtNextCode.Text = GenerateNextSalesCode();
            txtId.Text = GenerateNextSalesId();
            if (id != 0)
            {
                BindInvoice();
                btnSave.Enabled = UserPermissions.canEdit("Purchase Return");
            }
        }

        private void loadTaxCombo()
        {
            DataTable dt = DBClass.ExecuteDataTable("select id, concat(name,'-',value , '%') as name,value from tbl_tax");
            DataGridViewComboBoxColumn vat = (DataGridViewComboBoxColumn)dgvItems.Columns["vat"];
            vat.DataSource = dt;
            vat.DisplayMember = "name";
            vat.ValueMember = "id";
            var dTax = Utilities.GeneralSettings("DEFAULT TAX PERCENTAGE");
            if (!string.IsNullOrEmpty(dTax) & int.Parse(dTax) > 0)
            {
                defaultTax = dTax;
            }
        }

        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_purchase_return  where id = @id",
                DBClass.CreateParameter("id", id)))
            {
                reader.Read();
                txtId.Text = reader["id"].ToString();
                txtNextCode.Text = reader["invoice_id"].ToString();
                dtInv.Value = DateTime.Parse(reader["date"].ToString());
                cmbVendor.SelectedValue = reader["vendor_id"].ToString();
                cmbPaymentMethod.Text = reader["payment_method"].ToString();

                if (reader["payment_method"].ToString() == "Cash")
                    cmbAccountCashName.SelectedValue = reader["account_cash_id"].ToString();
                else
                {
                    cmbPaymentTerms.Text = reader["payment_terms"].ToString();
                    dtPaymentTerms.Value = DateTime.Parse(reader["payment_date"].ToString());
                }
                cmbShipVia.Text = reader["ship_via"].ToString();
                richTextDescription.Text = reader["description"].ToString();
                cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                invId = id;
                txtBillTo.Text = reader["bill_to"].ToString();
                txtShipTo.Text = reader["ship_to"].ToString();
                txtSalesMan.Text = reader["sales_man"].ToString();
                BindInvoiceItems();
                CalculateTotal();
            }
        }
        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_purchase_return_details.*,tbl_items.type,method, tbl_items.code ,tbl_items.name FROM tbl_purchase_return_details INNER JOIN 
                                                                    tbl_items ON tbl_purchase_return_details.item_id = tbl_items.id WHERE 
                                                                    tbl_purchase_return_details.purchase_id = @id;",
                                                            DBClass.CreateParameter("id", id)))
                
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["item_id"].ToString(), "", reader["code"].ToString(), reader["name"].ToString(), Utilities.FormatDecimal(reader["qty"].ToString()),
                        Utilities.FormatDecimal(reader["cost_price"].ToString()), Utilities.FormatDecimal(reader["price"].ToString()));
                    DataGridViewComboBoxCell comboCell = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vat"] as DataGridViewComboBoxCell;
                    if (comboCell != null && reader["vat"].ToString() != "0")
                    {
                        comboCell.Value = int.Parse(reader["vat"].ToString());
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value = decimal.Parse(reader["vatP"].ToString());
                    }
                    DataGridViewComboBoxCell comboCellCostCenter = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_center"] as DataGridViewComboBoxCell;
                    if (comboCellCostCenter != null && reader["cost_center_id"].ToString() != "0")
                    {
                        comboCellCostCenter.Value = int.Parse(reader["cost_center_id"].ToString());
                    }
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["total"].Value = ((decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) *
                      decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_price"].Value.ToString()))
                      + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString()));

                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = reader["method"].ToString();
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = reader["type"].ToString();
                }
        }

        private DataTable GetFilteredProductsByName(string name = "")
        {
            string option = Utilities.GeneralSettingsState("ALL ITEMS IN PURCHASE");

            string query = "select code,name from tbl_items where state = 0 AND active = 0";
            var parameters = new List<MySqlParameter>();

            if (string.IsNullOrEmpty(option) || int.Parse(option) <= 0)
            {
                query += " AND type = '11 - Inventory Part'";
            }

            if (!string.IsNullOrEmpty(name))
            {
                query += " AND name LIKE @name";
                parameters.Add(new MySqlParameter("@name", $"%{name}%"));
            }
            query += " ORDER BY name LIMIT 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private DataTable GetFilteredProductsByCode(string code = "")
        {
            string option = Utilities.GeneralSettingsState("ALL ITEMS IN PURCHASE");

            string query = "select code,name from tbl_items where state = 0 AND active = 0";
            var parameters = new List<MySqlParameter>();

            if (string.IsNullOrEmpty(option) || int.Parse(option) <= 0)
            {
                query += " AND type = '11 - Inventory Part'";
            }

            if (!string.IsNullOrEmpty(code))
            {
                query += " AND code LIKE @code";
                parameters.Add(new MySqlParameter("@code", $"%{code}%"));
            }

            query += " ORDER BY code Limit 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private void loadCostCenter()
        {
            DataTable dt = DBClass.ExecuteDataTable("select id,code as name from tbl_sub_cost_center");
            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)dgvItems.Columns["Cost_Center"];
            col.DataSource = dt;
            col.DisplayMember = "name";
            col.ValueMember = "id";
        }
        string defaultTax = "";
        private void RefreshItems()
        {
            DataTable dt = DBClass.ExecuteDataTable("select code,name,value from tbl_items where state = 0 and active = 0");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
        }
        private void bindCombo()
        {
            BindCombos.PopulateVendors(cmbVendor);
            BindCombos.PopulateAllLevel4Account(cmbAccountCashName);
            var defaultAccounts = BindCombos.LoadDefaultAccounts();

            cmbAccountCashName.SelectedValue = defaultAccounts.ContainsKey("Purchase Payment Cash Method")
                ? defaultAccounts["Purchase Payment Cash Method"] : 0;

            level4PaymentCreditMethodId = defaultAccounts.ContainsKey("Vendor") ? defaultAccounts["Vendor"] : 0;
            level4VatId = defaultAccounts.ContainsKey("Vat Input") ? defaultAccounts["Vat Output"] : 0;
            level4PurchaseReturnId = defaultAccounts.ContainsKey("PurchaseReturn") ? defaultAccounts["PurchaseReturn"] : 0;
        }
        private string GenerateNextSalesCode()
        {
            string newCode = "PR-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(invoice_id, 4) AS UNSIGNED)) AS lastCode FROM tbl_purchase_return"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "PR-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private string GenerateNextSalesId()
        {
            string newCode = "1";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(id) AS lastCode FROM tbl_purchase_return"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = code.ToString();
                }
            }

            return newCode;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Utilities.AreDefaultAccountsSet(new List<string> { "Vendor", "Vat Output", "Inventory" }))
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (id == 0)
            {
                if (insertInvoice())
                {
                    EventHub.RefreshPurchaseReturn();
                    MessageBox.Show("Saved");
                    clearData();
                }
            }
            else
            {
                if (updateInvoice())
                {
                    EventHub.RefreshPurchaseReturn();
                    MessageBox.Show("Edited");
                    clearData();
                }
            }
        }
        private bool updateInvoice()
        {
            if (!chkRequiredDate())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_purchase_return  
                                     SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date,sales_man=@sales_man, vendor_id = @vendor_id, invoice_id = @invoice_id, warehouse_id = @warehouse_id,
                                     po_num = @po_num, bill_to = @bill_to, ship_date = @ship_date, 
                                     ship_via = @ship_via, ship_to = @ship_to, payment_method = @payment_method, account_cash_id = @account_cash_id, 
                                     payment_terms = @payment_terms, payment_date = @payment_date, total = @total, 
                                     vat = @vat, net = @net, pay = @pay, `change` = @change, city = @city, @description = @description WHERE id = @id;",
                 DBClass.CreateParameter("id", id),
                  DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("vendor_id", cmbVendor.SelectedValue),
               DBClass.CreateParameter("invoice_id", txtNextCode.Text),
               DBClass.CreateParameter("city", cmbCity.Text),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("po_num", txtPONO.Text),
               DBClass.CreateParameter("bill_to", txtBillTo.Text),
               DBClass.CreateParameter("ship_date", dtShip.Value.Date),
               DBClass.CreateParameter("sales_man", txtSalesMan.Text),
               DBClass.CreateParameter("ship_via", cmbShipVia.Text),
               DBClass.CreateParameter("ship_to", txtShipTo.Text),
               DBClass.CreateParameter("payment_method", cmbPaymentMethod.Text),
               DBClass.CreateParameter("account_cash_id", cmbAccountCashName.SelectedValue),
               DBClass.CreateParameter("payment_terms", cmbPaymentTerms.Text),
               DBClass.CreateParameter("payment_date", dtPaymentTerms.Value.Date),
               DBClass.CreateParameter("vat", txtTotalVat.Text),
               DBClass.CreateParameter("total", txtTotalBefore.Text),
               DBClass.CreateParameter("net", txtTotal.Text),
               DBClass.CreateParameter("description", richTextDescription.Text),
            DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date),
            DBClass.CreateParameter("pay", cmbPaymentMethod.Text == "Cash" ? string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text) : 0),
            DBClass.CreateParameter("change", cmbPaymentMethod.Text == "Cash" ? 0 : string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text)));
            ReturnItemsToInventory();
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction WHERE reference = @invId AND type = 'Purchase Return Invoice';
                DELETE FROM tbl_item_card_details WHERE trans_type = 'Purchase Return Invoice' and trans_no=@invId", DBClass.CreateParameter("invId", id));

            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "Purchase Return");
            insertInvItems();
            CommonInsert.DeleteTransactionEntry(id, "PURCHASE RETURN");
            addJournalTransaction();
            Utilities.LogAudit(frmLogin.userId, "Edit Purchase Return Invoice", "Purchase Return", id, "Edited Purchase Return Invoice No. " + txtNextCode.Text);

            return true;
        }
        private void addJournalTransaction()
        {
            CommonInsert.addTransactionEntry(dtInv.Value.Date,
                 cmbPaymentMethod.Text == "Credit" ? level4PaymentCreditMethodId.ToString()
                 : cmbAccountCashName.SelectedValue.ToString(),
                  "0", txtTotal.Text, invId.ToString(), cmbVendor.SelectedValue.ToString(), "Purchase Return Invoice", "PURCHASE RETURN", "Purchase Return Invoice NO. " + txtNextCode.Text,
                 frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);

            if (decimal.Parse(txtTotalVat.Text) > 0)
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
              level4VatId.ToString(), txtTotalVat.Text, "0", invId.ToString(), "0", "Purchase Return Invoice", "PURCHASE RETURN",
              "Vat Input For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
            CommonInsert.addTransactionEntry(dtInv.Value.Date,
              level4PurchaseReturnId.ToString(), txtTotalBefore.Text, "0", invId.ToString(), "0", "Purchase Return Invoice", "PURCHASE RETURN",
              "Purchase Return For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
        }
        private void ReturnItemsToInventory()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"select tbl_purchase_return_details.* ,tbl_items.method,tbl_items.type from tbl_purchase_return_details
                                                           inner join tbl_items on tbl_purchase_return_details.item_id = tbl_items.id 
                                                           where purchase_id =@id", DBClass.CreateParameter("id", id)))
                while (reader.Read())
                {
                    if (reader["type"].ToString() != "Service")
                    {
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand =on_hand+ @qty where id =@id", DBClass.CreateParameter("id", reader["item_id"].ToString()), DBClass.CreateParameter("qty", reader["qty"].ToString()));
                        DBClass.ExecuteNonQuery("delete from tbl_item_transaction  where reference =@invId and type ='Purchase Return Invoice'", DBClass.CreateParameter("invId", id));
                    }
                    DBClass.ExecuteNonQuery("delete from tbl_purchase_return_details where id =@id", DBClass.CreateParameter("id", reader["id"].ToString()));
                }
        }

        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;
            
            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_purchase_return  (date, vendor_id, invoice_id,warehouse_id, po_num, bill_to,city,sales_man,
                   ship_date,ship_via, ship_to, payment_method,account_cash_id, payment_terms, payment_date,  
                   total,vat,net, pay, `change`, created_by, created_date, state, description) VALUES (@DATE, @vendor_id, @invoice_id,@warehouse_id,
                   @po_num, @bill_to,@city,@sales_man, @ship_date, @ship_via,@ship_to, @payment_method,@account_cash_id ,@payment_terms, @payment_date, 
                   @total,  @vat,@net, @pay, @change, @created_by, @created_date, @state, @description);
                   SELECT LAST_INSERT_ID();",
                   DBClass.CreateParameter("date", dtInv.Value.Date),
                   DBClass.CreateParameter("vendor_id", cmbVendor.SelectedValue),
                   DBClass.CreateParameter("invoice_id", txtNextCode.Text),
                   DBClass.CreateParameter("city", cmbCity.Text),
                   DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
                   DBClass.CreateParameter("po_num", txtPONO.Text),
                   DBClass.CreateParameter("bill_to", txtBillTo.Text),
                   DBClass.CreateParameter("ship_date", dtShip.Value.Date),
                   DBClass.CreateParameter("sales_man", txtSalesMan.Text),
                   DBClass.CreateParameter("ship_via", cmbShipVia.Text),
                   DBClass.CreateParameter("ship_to", txtShipTo.Text),
                   DBClass.CreateParameter("payment_method", cmbPaymentMethod.Text),
                   DBClass.CreateParameter("account_cash_id", cmbAccountCashName.SelectedValue),
                   DBClass.CreateParameter("payment_terms", cmbPaymentTerms.Text),
                   DBClass.CreateParameter("payment_date", dtPaymentTerms.Value.Date),
                   DBClass.CreateParameter("vat", txtTotalVat.Text),
                   DBClass.CreateParameter("total", txtTotalBefore.Text),
                   DBClass.CreateParameter("net", txtTotal.Text),
                   DBClass.CreateParameter("description", richTextDescription.Text),
                   DBClass.CreateParameter("pay", cmbPaymentMethod.Text == "Cash" ? string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text) : 0),
                   DBClass.CreateParameter("change", cmbPaymentMethod.Text == "Cash" ? 0 : string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text)),
                   DBClass.CreateParameter("created_by", frmLogin.userId),
                   DBClass.CreateParameter("created_date", DateTime.Now.Date),
                   DBClass.CreateParameter("state", 0)).ToString());

            
            insertInvItems();
            //txtInvoiceId.Text = invCode.ToString();
            addJournalTransaction();
            Utilities.LogAudit(frmLogin.userId, "Add Purchase Return Invoice", "Purchase Return", (int)invId, "Added Purchase Return Invoice No. " + txtNextCode.Text);

            return true;
        }
        private void insertInvItems()
        {
            var valueList = new List<string>();
            var parameters = new List<MySqlParameter>();
            int paramIndex = 0;

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                var row = dgvItems.Rows[i];
                if (row.IsNewRow || string.IsNullOrWhiteSpace(row.Cells["itemId"].Value?.ToString()))
                    continue;

                string itemId = row.Cells["itemId"].Value.ToString();
                decimal qty = row.Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["qty"].Value.ToString());

                //DBClass.ExecuteNonQuery(@"INSERT INTO tbl_purchase_return_details (purchase_id, item_id, qty,cost_price, price, vatp, vat, total, cost_center_id)
                //         VALUES (@purchase_id, @item_id, @qty,@cost_price, @price, @vatp, @vat, @total, @costCenter);",
                //DBClass.CreateParameter("@purchase_id", invId),
                //DBClass.CreateParameter("@item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                //DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value)),
                //DBClass.CreateParameter("@price", dgvItems.Rows[i].Cells["price"].Value == DBNull.Value || dgvItems.Rows[i].Cells["price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["price"].Value)),
                //DBClass.CreateParameter("@cost_price", dgvItems.Rows[i].Cells["cost_price"].Value == DBNull.Value || dgvItems.Rows[i].Cells["cost_price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["cost_price"].Value)),
                //DBClass.CreateParameter("@vat", dgvItems.Rows[i].Cells["vat"].Value == null ? 0 : dgvItems.Rows[i].Cells["vat"].Value),
                //DBClass.CreateParameter("@vatp", dgvItems.Rows[i].Cells["vat"].Value == null ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["vatp"].Value)),
                //DBClass.CreateParameter("@total", dgvItems.Rows[i].Cells["total"].Value == null || dgvItems.Rows[i].Cells["total"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value)),
                //DBClass.CreateParameter("@costCenter", dgvItems.Rows[i].Cells["cost_center"].Value ?? 0));

                valueList.Add($"(@purchase_id, @item_id{paramIndex}, @qty{paramIndex}, @cost_price{paramIndex}, @price{paramIndex}, @vatp{paramIndex}, @vat{paramIndex}, @total{paramIndex}, @costCenter{paramIndex})");
                parameters.Add(new MySqlParameter($"item_id{paramIndex}", itemId));
                parameters.Add(new MySqlParameter($"qty{paramIndex}", qty));
                parameters.Add(new MySqlParameter($"cost_price{paramIndex}", row.Cells["cost_price"].Value == DBNull.Value || row.Cells["cost_price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(row.Cells["cost_price"].Value)));
                parameters.Add(new MySqlParameter($"price{paramIndex}", row.Cells["price"].Value == DBNull.Value || row.Cells["price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(row.Cells["price"].Value)));
                parameters.Add(new MySqlParameter($"vatp{paramIndex}", row.Cells["vatp"].Value == null ? 0 : Convert.ToDecimal(row.Cells["vatp"].Value)));
                parameters.Add(new MySqlParameter($"vat{paramIndex}", row.Cells["vat"].Value == null ? 0 : row.Cells["vat"].Value));
                parameters.Add(new MySqlParameter($"total{paramIndex}", row.Cells["total"].Value == null || row.Cells["total"].Value.ToString() == "" ? 0 : Convert.ToDecimal(row.Cells["total"].Value)));
                parameters.Add(new MySqlParameter($"costCenter{paramIndex}", row.Cells["cost_center"].Value ?? 0));
                paramIndex++;
            }

            if (valueList.Count == 0)
                return;

            string sql = $@"
                            INSERT INTO tbl_purchase_return_details 
                            (purchase_id, item_id, qty, cost_price, price, vatp, vat, total, cost_center_id)
                            VALUES {string.Join(", ", valueList)}";
            using(var conn = DBClass.GetConnection())
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("purchase_id", invId);
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.Add(param);
                    }
                    cmd.ExecuteNonQuery();
                }
            }

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                var row = dgvItems.Rows[i];
                if(row.IsNewRow || string.IsNullOrWhiteSpace(row.Cells["itemId"].Value?.ToString()))
                    continue;

                if (row.Cells["type"].Value.ToString() != "Service")
                {
                    //using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id=@id",
                    //DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString())))
                    //{
                    //    reader.Read();
                    //    DBClass.ExecuteNonQuery("update tbl_items set on_hand=@qty where id = @id",
                    //    DBClass.CreateParameter("id", reader["id"].ToString()),
                    //    DBClass.CreateParameter("qty", decimal.Parse(reader["on_hand"].ToString()) - decimal.Parse(row.Cells["qty"].Value.ToString())));
                    //}

                    insertItemTransaction(row);
                }
                //add cost center
                if (row.Cells["cost_center"].Value != null && int.Parse(row.Cells["cost_center"].Value.ToString()) > 0)
                    CommonInsert.InsertCostCenterTransaction(dtInv.Value, "0", Convert.ToDecimal(row.Cells["total"].Value ?? 0).ToString(), invId.ToString(), "Purchase Return", "", (row.Cells["cost_center"].Value ?? 0).ToString());
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

            CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Purchase Return Invoice", invId.ToString(), row.Cells["itemId"].Value.ToString(), row.Cells["cost_price"].Value.ToString(),
                        "0", row.Cells["price"].Value.ToString(), row.Cells["qty"].Value.ToString(), "0", "Purchase Return Invoice No. " + txtNextCode.Text, cmbWarehouse.SelectedValue.ToString());
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
            if (cmbVendor.SelectedValue == null)
            {
                MessageBox.Show("Vendor Must be Selected.");
                txtVendorCode.Focus();
                return false;
            }
            if (cmbAccountCashName.SelectedValue == null)
            {
                MessageBox.Show("Account Cash Name Must be Selected.");
                cmbAccountCashName.Focus();
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
            txtSalesMan.Text = txtBillTo.Text = txtShipTo.Text = txtPONO.Text =
                txtTotal.Text = txtTotalVat.Text = "";
            cmbPaymentMethod.SelectedIndex = id = 0;
            dtInv.Value = DateTime.Now;
            dgvItems.Rows.Clear();
            txtNextCode.Text = GenerateNextSalesCode();
            txtId.Text = GenerateNextSalesId();
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
            if (cmbVendor.SelectedValue == null)
            {
                txtBillTo.Text = txtVendorCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor where id = " + cmbVendor.SelectedValue.ToString()))
                if (reader.Read())
                {
                    txtVendorCode.Text = reader["code"].ToString();
                    txtBillTo.Text = cmbVendor.Text;
                }
                else
                    txtBillTo.Text = txtVendorCode.Text = "";
        }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.Text == "Cash")
            {
                cmbPaymentTerms.SelectedIndex = -1;
                cmbAccountCashName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Purchase Payment Cash Method");
                cmbAccountCashName.Enabled = true;
                cmbPaymentTerms.Enabled = false;
            }
            else if (cmbPaymentMethod.Text == "Credit")
            {
                cmbPaymentTerms.SelectedIndex = 0;
                cmbAccountCashName.Enabled = false;
                cmbAccountCashName.SelectedValue = level4PaymentCreditMethodId;
                cmbPaymentTerms.Enabled = true;
            }
        }
        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor where code =@code",
                DBClass.CreateParameter("code", txtVendorCode.Text)))
                if (reader.Read())
                    cmbVendor.SelectedValue = int.Parse(reader["id"].ToString());

            if (txtVendorCode.Focused)
            {
                string input = txtVendorCode.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    lstAccountSuggestions.Visible = false;
                    return;
                }
                string query = @"SELECT code, name FROM tbl_vendor 
                         WHERE code LIKE @search OR name LIKE @search LIMIT 20";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

                lstAccountSuggestions.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
                }
                if (lstAccountSuggestions.Items.Count > 0)
                {
                    Point locationOnForm = txtVendorCode.Parent.PointToScreen(txtVendorCode.Location);
                    Point locationRelativeToForm = this.PointToClient(locationOnForm);

                    lstAccountSuggestions.SetBounds(
                        locationRelativeToForm.X,
                        locationRelativeToForm.Y + txtVendorCode.Height,
                        txtVendorCode.Width + 100,
                        120
                    );

                    lstAccountSuggestions.Tag = txtVendorCode;
                    lstAccountSuggestions.Visible = true;
                    lstAccountSuggestions.BringToFront();
                }
                else
                {
                    lstAccountSuggestions.Visible = false;
                }
            }
        }
        private void txtCustomerCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor where code =@code",
            DBClass.CreateParameter("code", txtVendorCode.Text)))
                if (!reader.Read())
                    cmbVendor.SelectedIndex = -1;

            BeginInvoke((Action)(() =>
            {
                if (!lstAccountSuggestions.Focused)
                    lstAccountSuggestions.Visible = false;
            }));
        }
        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is TextBox textBox0)
            {
                textBox0.TextChanged -= ItemCode_TextChanged;
                textBox0.TextChanged -= ItemName_TextChanged;
                textBox0.KeyPress -= NumericColumn_KeyPress;
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
            else if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["vat"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxTax_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxTax_SelectedIndexChanged);
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
            else if (dgvItems.CurrentCell.OwningColumn.Name == "qty" ||
                     dgvItems.CurrentCell.OwningColumn.Name == "cost_price")
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.KeyPress -= NumericColumn_KeyPress;
                    textBox.KeyPress += NumericColumn_KeyPress;
                }
            }
            else
            {
                lstAccountSuggestions.Visible = false;
                lstAccountSuggestions.Tag = null;
            }
        }

        private void NumericColumn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // block anything that's not a digit or decimal point
            }

            // Allow only one decimal point
            TextBox txt = sender as TextBox;
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
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

        private void ComboBoxTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox combo && combo.SelectedItem is DataRowView selectedRow)
            {
                if (dgvItems.CurrentCell == null || dgvItems.CurrentCell.ColumnIndex != dgvItems.Columns["vat"].Index)
                    return;

                var row = dgvItems.CurrentRow;
                if (row == null || row.IsNewRow)
                    return;

                string taxId = selectedRow["id"].ToString();
                string taxP = selectedRow["value"].ToString();

                decimal price = GetDecimalValue(row, "cost_price");
                decimal qty = GetDecimalValue(row, "qty");

                if (price == 0 || qty == 0)
                {
                    row.Cells["total"].Value = row.Cells["vatp"].Value = "0";
                }
                else
                {
                    decimal netPrice = (qty * price);
                    row.Cells["vat"].Value = Convert.ToInt32(taxId);
                    row.Cells["vatp"].Value = (decimal.Parse(netPrice.ToString()) * decimal.Parse(taxP) / 100);
                    row.Cells["total"].Value = netPrice + decimal.Parse(row.Cells["vatp"].Value.ToString());
                }
            }
        }

        private void ChkRowValidity()
        {
            var row = dgvItems.CurrentRow;
            if (row == null || row.IsNewRow)
                return;

            decimal price = GetDecimalValue(row, "cost_price");
            decimal qty = GetDecimalValue(row, "qty");

            if (price == 0 || qty == 0)
            {
                row.Cells["total"].Value = row.Cells["vatp"].Value = "0";
            }
            else
            {
                try
                {
                    decimal netPrice = (qty * price);

                    string vatValue = row.Cells["vat"].Value?.ToString() ?? "0";
                    string vatPercentage = "0";

                    DataTable vatTable = ((DataGridViewComboBoxColumn)dgvItems.Columns["vat"]).DataSource as DataTable;
                    if (vatTable != null)
                    {
                        DataRow[] vatRows = vatTable.Select($"id = '{vatValue}'");
                        if (vatRows.Length > 0)
                        {
                            vatPercentage = vatRows[0]["value"].ToString();
                        }
                    }

                    decimal vatAmount = (netPrice * Convert.ToDecimal(vatPercentage)) / 100;
                    row.Cells["vatp"].Value = vatAmount;
                    row.Cells["total"].Value = netPrice + vatAmount;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("VAT calc error: " + ex.Message);
                }
            }
        }

        bool CheckItemValidity(int itemId)
        {
            decimal qty = 0;
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id = @id",
                DBClass.CreateParameter("id", itemId)))
            {
                {
                    reader.Read();
                    if (reader["type"].ToString() == "Service")
                        return true;
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
            txtTotalBefore.Text = (total - vat).ToString("0.000");
            txtTotalVat.Text = vat.ToString("0.000");
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
                else if (e.ColumnIndex == dgvItems.Columns["qty"].Index)
                {
                    if (dgvItems.CurrentRow.Cells["itemId"].Value == null || !CheckItemValidity(int.Parse(dgvItems.CurrentRow.Cells["itemId"].Value.ToString())))
                        row.Cells["qty"].Value = 0;
                }
                ChkRowValidity();
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

        private void insertItemThroughCodeOrText(string type, string inputText = null)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return;

            MySqlDataReader reader = null;
            try
            {
                if (type == "code")
                {
                    reader = DBClass.ExecuteReader(@"
                            SELECT id, method, type, code, sales_price, cost_price, name 
                            FROM tbl_items 
                            WHERE code = @code",
                        DBClass.CreateParameter("code", inputText.Trim()));
                }
                else if (type == "name")
                {
                    // Search by name (case-insensitive)
                    reader = DBClass.ExecuteReader(@"
                                SELECT id, method, type, code, sales_price, cost_price, name 
                                FROM tbl_items 
                                WHERE name = @name COLLATE utf8mb4_general_ci LIMIT 1",
                        DBClass.CreateParameter("name", inputText.Trim()));
                }

                if (reader != null && reader.Read())
                {
                    dgvItems.CurrentRow.Cells["qty"].Value = 0;
                    dgvItems.CurrentRow.Cells["itemid"].Value = reader["id"].ToString();
                    dgvItems.CurrentRow.Cells["cost_price"].Value = reader["cost_price"].ToString();
                    dgvItems.CurrentRow.Cells["price"].Value = reader["sales_price"];
                    dgvItems.CurrentRow.Cells["method"].Value = reader["method"];
                    dgvItems.CurrentRow.Cells["type"].Value = reader["type"];
                    dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                    dgvItems.CurrentRow.Cells["name"].Value = reader["name"].ToString();

                    // Optional default VAT
                    if (!string.IsNullOrEmpty(defaultTax))
                    {
                        dgvItems.CurrentRow.Cells["vat"].Value = int.Parse(defaultTax);
                    }
                }
                else
                {
                    DataGridViewRow row = dgvItems.CurrentRow;
                    if (row != null)
                    {
                        row.Cells["itemid"].Value = null;
                        row.Cells["cost_price"].Value = null;
                        row.Cells["price"].Value = null;
                        row.Cells["method"].Value = null;
                        row.Cells["type"].Value = null;
                        row.Cells["vat"].Value = null;
                        if (type == "name")
                            row.Cells["code"].Value = null;
                        if (type == "code")
                            row.Cells["name"].Value = null;
                    }
                }
            }
            finally
            {
                reader?.Close();
                reader?.Dispose();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int? currentId = Convert.ToInt32(txtId.Text); // Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId == null || currentId <= 1)
                return;

            currentId = currentId - 1;
            if (currentId <= 0)
            {
                clearData();
                MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "select id from tbl_purchase_return  where state = 0 and id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clearData();
                    MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int? currentId = Convert.ToInt32(txtId.Text); // Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_purchase_return  WHERE state = 0 AND id =@id";

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                } else
                {
                    clearData();
                    MessageBox.Show("No next records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }


        private void lnkNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmViewVendor frm = new frmViewVendor(0);
            frm.ShowDialog();
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
                {
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
                    CalculateTotal();
                }
                else if (e.ColumnIndex == dgvItems.Columns["vat"].Index)
                {
                    ChkRowValidity();
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }

        private void frmPurchaseReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Vendor -= vendorUpdatedHandler;
            EventHub.wareHouse -= warehouseUpdatedHandler;
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

        private void cmbAccountCashName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAccountCashName.Focused && int.Parse(cmbAccountCashName.SelectedValue.ToString()) == -1)
            {
                new frmAddAccount().ShowDialog();
            }
            else
            {
            }
        }

        public DataTable COMPANYINFO(Int64 a1)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company Limit 1");
        }

        public DataTable vendorDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT p.id,p.date,p.vendor_id,p.invoice_id,p.bill_to,p.city,p.sales_man,p.ship_date,p.ship_via,ship_to,po_num,payment_method,payment_terms,payment_date,(SELECT NAME FROM tbl_coa_level_4 WHERE id=account_cash_id) accountName,total,vat,net,pay,`change` 
                                  , v.name vendorName, v.main_phone,v.email,v.trn,v.mobile from tbl_purchase_return p, tbl_vendor v
                                  WHERE p.vendor_id = v.id AND p.id = @purchaseId; ", DBClass.CreateParameter("@purchaseId", a1));
        }
        public DataTable purchaseDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT d.id,d.item_id,i.name,d.qty,d.cost_price,d.price,(d.qty*d.cost_price) subCostTotal,(d.qty*d.price) subPriceTotal,d.discount,((d.qty*d.price)-d.discount) subTotal,d.vatp vatAmount,d.vat vatPercentage,d.total,d.cost_center_id,(SELECT NAME FROM tbl_sub_cost_center WHERE id=cost_center_id) costCenterName,i.type,i.method,i.unit_id,(select name from tbl_unit WHERE id=i.unit_id) unitName, i.code as code FROM  tbl_purchase_return_details d INNER JOIN tbl_items i ON d.item_id = i.id WHERE d.purchase_id =@purchaseId; ",
                DBClass.CreateParameter("@purchaseId", a1));
        }
        public void ShowReport()
        {

            try
            {
                // Create the report object
                //CPVPurchaseeReturn cr = new CPVPurchaseeReturn ();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "PurchaseReturn.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                // Load the main report data
                DataTable companyData = COMPANYINFO(1);  // Assuming you want to pass ID 1
                DataTable vendorData = vendorDetails(invId.ToString());
                DataTable purchaseData = purchaseDetails(invId.ToString());
                if (companyData != null)  // Ensure that data was successfully retrieved
                {
                    //cr.SetDataSource(companyData);
                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["InfoHeader"].SetDataSource(vendorData);
                    cr.Subreports["Details"].SetDataSource(purchaseData);
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;  // Exit the method if no data is available
                }

                // Assign the main report to the viewer
                MasterReportView reportForm = new MasterReportView();
                reportForm.crReportViewer.ReportSource = cr;
                reportForm.crReportViewer.RefreshReport();
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {
            clearData();
        }
        private void clearData()
        {
            resetTextBox();
            id = 0;
        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {
            DBClass.ExecuteNonQuery("UPDATE tbl_purchase_return SET state = -1 WHERE id = @id; UPDATE tbl_transaction SET state= -1 WHERE transaction_id=@id AND t_type = 'PURCHASE';",
                                          DBClass.CreateParameter("id", id.ToString()));
            CommonInsert.DeleteItemTransaction("Purchase Return", id.ToString());
            Utilities.LogAudit(frmLogin.userId, "Delete Purchase Return", "Purchase Return", id, "Deleted Purchase Return: " + txtNextCode.Text);
            clearData();
        }

        private void guna2TileButton20_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        private void guna2TileButton24_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchaseByVendorSummary());
        }

        private void guna2TileButton25_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal(id.ToString(), "PURCHASE RETURN"));
        }

        private void guna2TileButton26_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal(id.ToString(), "PURCHASE RETURN"));
        }

        private void guna2TileButton28_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchaseByVendorDetails(int.Parse(cmbVendor.SelectedValue.ToString())));
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

            if (lstAccountSuggestions.Tag is Guna2TextBox gunaTextBox)
            {
                gunaTextBox.Text = selectedCode;
                gunaTextBox.Focus();
                gunaTextBox.SelectionStart = gunaTextBox.Text.Length;

                if (gunaTextBox == txtVendorCode)
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader(
                        "SELECT id FROM tbl_vendor WHERE code = @code",
                        DBClass.CreateParameter("code", selectedCode)))
                    {
                        if (reader.Read())
                            cmbVendor.SelectedValue = int.Parse(reader["id"].ToString());
                    }
                }
            }
            else if (lstAccountSuggestions.Tag is TextBox textBox)
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

        private void dgvItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var row = dgvItems.Rows[e.RowIndex];
            var columnName = dgvItems.Columns[e.ColumnIndex].Name;
            string inputText = row.Cells[e.ColumnIndex].Value?.ToString()?.Trim();
            var itemId = row.Cells["itemId"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(itemId) || string.IsNullOrWhiteSpace(inputText))
                return;

            if ((columnName == "code" || columnName == "name") && lstAccountSuggestions.Items.Count <= 0)
            {
                DataGridViewRow newRow = dgvItems.CurrentRow;
                if (newRow != null)
                {   
                    newRow.Cells["itemid"].Value = null;
                    newRow.Cells["cost_price"].Value = null;
                    newRow.Cells["price"].Value = null;
                    newRow.Cells["method"].Value = null;
                    newRow.Cells["type"].Value = null;
                    newRow.Cells["vat"].Value = null;
                    newRow.Cells["name"].Value = null;
                    newRow.Cells["code"].Value = null;
                }
            }
        }

        // Example method to get data for subreport 3 (replace with your actual logic)
        private DataTable GetSubreportData3()
        {
            // Fetch or prepare data for subreport 3
            DataTable subreportData = new DataTable();
            // Your logic to fetch data for subreport 3
            return subreportData;
        }

        // Example method to get data for subreport 4 (replace with your actual logic)
        private DataTable GetSubreportData4()
        {
            // Fetch or prepare data for subreport 4
            DataTable subreportData = new DataTable();
            // Your logic to fetch data for subreport 4
            return subreportData;
        }
    }
}
