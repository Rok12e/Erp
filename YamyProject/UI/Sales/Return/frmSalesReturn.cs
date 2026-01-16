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
    public partial class frmSalesReturn : Form
    {
        private EventHandler customerUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;

        decimal invId;
        int level4PaymentCreditMethodId, level4VatId, level4SalesReturnId, level4COGS, level4Inventory;
        private MasterSalesReturn _sales;
        MasterCustomer _customer;
        int id;

        public frmSalesReturn(int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            warehouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            EventHub.Customer += customerUpdatedHandler;
            EventHub.wareHouse += warehouseUpdatedHandler;
            this.id = id;
            if (id != 0)
                this.Text = "SalesReturn - Edit";
            else
                this.Text = "SalesReturn - New";
           headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmSalesReturn_Load(object sender, EventArgs e)
        {
            txtNextCode.Text = GenerateNextSalesCode();
            dtPaymentTerms.Value = dtInv.Value = dtShip.Value = DateTime.Now.Date;
            bindCombo();
            LoadVat();
            loadCostCenter();
            cmbPaymentMethod.SelectedIndex = 0;
            txtId.Text = GenerateNextSalesId();
            if (id != 0)
            {
                btnSave.Enabled = UserPermissions.canEdit("Sales Return");
                BindInvoice();
            }
        }
        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_sales_return where id = @id",
                DBClass.CreateParameter("id", id)))
            {
                reader.Read();
                txtId.Text = reader["id"].ToString();
                dtInv.Value = DateTime.Parse(reader["date"].ToString());
                txtNextCode.Text = reader["invoice_id"].ToString();
                cmbCustomer.SelectedValue = reader["customer_id"].ToString();
                cmbPaymentMethod.Text = reader["payment_method"].ToString();
                if (reader["payment_method"].ToString() == "Cash")
                    cmbAccountCashName.SelectedValue = reader["account_cash_id"].ToString();
                else
                {
                    cmbPaymentTerms.Text = reader["payment_terms"].ToString();
                    dtPaymentTerms.Value = DateTime.Parse(reader["payment_date"].ToString());
                }
                cmbShipVia.Text = reader["ship_via"].ToString();
                cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                invId = id;
                txtBillTo.Text = reader["bill_to"].ToString();
                txtShipTo.Text = reader["ship_to"].ToString();
                txtSalesMan.Text = reader["sales_man"].ToString();
                richTextDescription.Text = reader["description"].ToString();
                BindInvoiceItems();
                CalculateTotal();
            }
        }
        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_sales_return_details.*,tbl_items.type,method, tbl_items.code ,tbl_items.name FROM tbl_sales_return_details INNER JOIN 
                                                                    tbl_items ON tbl_sales_return_details.item_id = tbl_items.id WHERE 
                                                                    tbl_sales_return_details.sales_id = @id;",
                                                            DBClass.CreateParameter("id", id)))

                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["item_id"].ToString(), "", reader["code"].ToString(), reader["name"].ToString(), reader["qty"].ToString(),
                        reader["cost_price"].ToString(), reader["price"].ToString());
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
                   decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["price"].Value.ToString()))
                   + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString()));
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = reader["method"].ToString();
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = reader["type"].ToString();
                }
        }

        private void LoadVat()
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
        private void loadCostCenter()
        {
            DataTable dt = DBClass.ExecuteDataTable("select id,code as name from tbl_sub_cost_center");
            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)dgvItems.Columns["Cost_Center"];
            col.DataSource = dt;
            col.DisplayMember = "name";
            col.ValueMember = "id";
        }
        string defaultTax = "";
        private void bindCombo()
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            BindCombos.PopulateAllLevel4Account(cmbAccountCashName);
            BindCombos.PopulateWarehouse(cmbWarehouse);
            cmbAccountCashName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Invoice Payment Cash Method");
            aa: level4PaymentCreditMethodId = BindCombos.SelectDefaultLevelAccount("Invoice Payment Credit Method");
            level4VatId = BindCombos.SelectDefaultLevelAccount("Vat Output");
            level4SalesReturnId = BindCombos.SelectDefaultLevelAccount("SalesReturn");
            level4COGS = BindCombos.SelectDefaultLevelAccount("COGS");
            level4Inventory = BindCombos.SelectDefaultLevelAccount("Inventory");
            if (level4Inventory == 0 || level4COGS == 0 || level4SalesReturnId == 0 || level4VatId == 0 || level4PaymentCreditMethodId == 0)
            {
                MessageBox.Show("Default Accounts For Invoice Must Be Set First");
                //frmLogin.frmMain.openChildForm(new frmDefaultAccountSetting());
                //goto aa;
                //this.Close();
                return;
            }
        }
        private DataTable GetFilteredProductsByName(string name = "")
        {
            int selectedWarehouseId = cmbWarehouse.SelectedValue == null ? 0 : Convert.ToInt32(cmbWarehouse.SelectedValue);

            string allItemsOption = Utilities.GeneralSettingsState("ALL ITEMS IN SALES");
            string warehouseOption = Utilities.GeneralSettingsState("WAREHOUSE ITEMS IN SALES 0");

            var parameters = new List<MySqlParameter>();
            string query;

            if (!string.IsNullOrEmpty(warehouseOption) && cmbWarehouse.Items.Count > 1)
            {
                // Use tbl_items_warehouse join
                query = @"
                        SELECT i.code, i.name 
                        FROM tbl_items_warehouse w
                        INNER JOIN tbl_items i ON w.item_id = i.id
                        WHERE w.warehouse_id = @warehouseId
                          AND i.state = 0 AND i.active = 0";

                parameters.Add(new MySqlParameter("@warehouseId", selectedWarehouseId));
            }
            else
            {
                // Use tbl_items directly
                query = "SELECT code, name FROM tbl_items WHERE state = 0 AND active = 0";

                if (string.IsNullOrEmpty(allItemsOption) || int.Parse(allItemsOption) <= 0)
                {
                    query += " AND type IN ('11 - Inventory Part', '12 - Service')";
                }

                if (!string.IsNullOrEmpty(warehouseOption))
                {
                    query += " AND warehouse_id = @warehouseId";
                    parameters.Add(new MySqlParameter("@warehouseId", selectedWarehouseId));
                }
            }

            // Apply name filter if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                query += " AND name LIKE @name";
                parameters.Add(new MySqlParameter("@name", $"%{name}%"));
            }

            query += " ORDER BY name LIMIT 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private DataTable GetFilteredProductsByCode(string code = "")
        {
            int selectedWarehouseId = cmbWarehouse.SelectedValue == null ? 0 : Convert.ToInt32(cmbWarehouse.SelectedValue);

            string allItemsOption = Utilities.GeneralSettingsState("ALL ITEMS IN SALES");
            string warehouseOption = Utilities.GeneralSettingsState("WAREHOUSE ITEMS IN SALES 0");

            var parameters = new List<MySqlParameter>();
            string query;

            if (!string.IsNullOrEmpty(warehouseOption) && cmbWarehouse.Items.Count > 1)
            {
                // Use tbl_items_warehouse join
                query = @"
                        SELECT i.code, i.name 
                        FROM tbl_items_warehouse w
                        INNER JOIN tbl_items i ON w.item_id = i.id
                        WHERE w.warehouse_id = @warehouseId
                          AND i.state = 0 AND i.active = 0";

                parameters.Add(new MySqlParameter("@warehouseId", selectedWarehouseId));
            }
            else
            {
                // Use tbl_items directly
                query = "SELECT code, name FROM tbl_items WHERE state = 0 AND active = 0";

                if (string.IsNullOrEmpty(allItemsOption) || int.Parse(allItemsOption) <= 0)
                {
                    query += " AND type IN ('11 - Inventory Part', '12 - Service')";
                }

                if (!string.IsNullOrEmpty(warehouseOption))
                {
                    query += " AND warehouse_id = @warehouseId";
                    parameters.Add(new MySqlParameter("@warehouseId", selectedWarehouseId));
                }
            }

            // Apply code filter if provided
            if (!string.IsNullOrWhiteSpace(code))
            {
                query += " AND code LIKE @code";
                parameters.Add(new MySqlParameter("@code", $"%{code}%"));
            }

            query += " ORDER BY code LIMIT 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            {
                MessageBox.Show("The Return Invoice  Update");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }


            if (id == 0)
            {
                if (insertInvoice())
                {
                    EventHub.RefreshSales();
                    MessageBox.Show("The Return Invoice  Saved");
                    dgvItems.Rows.Clear();

                    if (checkBox4.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
            else
            {
                if (updateInvoice())
                {
                    EventHub.RefreshSales();
                    MessageBox.Show("The Return Invoice  Saved");
                    dgvItems.Rows.Clear();

                    if (checkBox4.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
        }
        private void preview()
        {
            //DialogResult result = MessageBox.Show("Do You want To Show This Bill ",
            //                                    "Confirmation",
            //                                    MessageBoxButtons.YesNo,
            //                                    MessageBoxIcon.Question);
            //if (result == DialogResult.Yes)
            //{
                //new MasterReportView().Show();

                ShowReport();
            //}else if(result == DialogResult.No)
            //{
            //    this.Close();
            //}
        }
        private bool updateInvoice()
        {
            if (!chkRequiredDate())
                return false;
            
            DBClass.ExecuteNonQuery(@"UPDATE tbl_sales_return 
                                     SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date,sales_man=@sales_man, customer_id = @customer_id, invoice_id = @invoice_id, warehouse_id = @warehouse_id,
                                     po_num = @po_num, bill_to = @bill_to, ship_date = @ship_date, 
                                     ship_via = @ship_via, ship_to = @ship_to, payment_method = @payment_method, account_cash_id = @account_cash_id, 
                                     payment_terms = @payment_terms, payment_date = @payment_date, total = @total, 
                                     vat = @vat, net = @net, pay = @pay, `change` = @change, city = @city, description = @description WHERE id = @id;",
                 DBClass.CreateParameter("id", id),
                  DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("customer_id", cmbCustomer.SelectedValue),
               DBClass.CreateParameter("invoice_id", txtNextCode.Text),
               DBClass.CreateParameter("city", cmbCity.Text),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("po_num", ""),
               DBClass.CreateParameter("bill_to", txtBillTo.Text),
               DBClass.CreateParameter("ship_date", dtShip.Value.Date),
               DBClass.CreateParameter("sales_man", txtSalesMan.Text),
               DBClass.CreateParameter("ship_via", cmbShipVia.Text),
               DBClass.CreateParameter("ship_to", txtShipTo.Text),
               DBClass.CreateParameter("payment_method", cmbPaymentMethod.Text),
               DBClass.CreateParameter("account_cash_id", cmbPaymentMethod.Text == "Credit" ? level4PaymentCreditMethodId.ToString() : cmbAccountCashName.SelectedValue.ToString()),
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
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction WHERE reference = @invId AND type = 'Sales Return Invoice';
                DELETE FROM tbl_item_card_details WHERE trans_type = 'Sales Return Invoice' and trans_no=@invId", DBClass.CreateParameter("invId", id));

            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "Sales Return");
            insertInvItems();

            CommonInsert.DeleteTransactionEntry(id, "SALES RETURN");
            transaction();
            Utilities.LogAudit(frmLogin.userId, "Update Sales Return Invoice", "Sales Return", id, "Updated Sales Return Invoice: " + txtNextCode.Text);

            return true;
        }

        private void ReturnItemsToInventory()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"select tbl_sales_return_details.* ,tbl_items.method,tbl_items.type from tbl_sales_return_details
                                                           inner join tbl_items on tbl_sales_return_details.item_id = tbl_items.id 
                                                           where sales_id =@id", DBClass.CreateParameter("id", id)))
                while (reader.Read())
                {
                    if (reader["type"].ToString() != "Service")
                    {
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand =on_hand+ @qty where id =@id", DBClass.CreateParameter("id", reader["item_id"].ToString()), DBClass.CreateParameter("qty", reader["qty"].ToString()));
                        DBClass.ExecuteNonQuery("delete from tbl_item_transaction  where reference =@invId and type ='Sales Return Invoice'", DBClass.CreateParameter("invId", id));
                        if (reader["method"].ToString() == "agv")
                            continue;
                        string qr = "";
                        if (reader["method"].ToString() == "fifo")
                            qr = (@"SELECT * FROM tbl_item_transaction WHERE item_id = @id and qty_in != qty_inc
                                                                     and qty_out = 0 ORDER BY id asc");
                        else
                            qr = (@"SELECT * FROM tbl_item_transaction WHERE item_id = @id and qty_in != qty_inc
                                                                     and qty_out = 0 ORDER BY id asc");
                        decimal attempQty = decimal.Parse(reader["qty"].ToString());
                        using (var tReader = DBClass.ExecuteReader(qr, DBClass.CreateParameter("id", reader["item_id"].ToString())))
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
                    DBClass.ExecuteNonQuery("delete from tbl_sales_return_details where id =@id", DBClass.CreateParameter("id", reader["id"].ToString()));
                }
        }
        private string GenerateNextSalesCode()
        {
            string newCode = "SRI-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(invoice_id, 5) AS UNSIGNED)) AS lastCode FROM tbl_sales_return"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "SRI-" + code.ToString("D5");
                }
            }

            return newCode;
        }
        private string GenerateNextSalesId()
        {
            string newCode = "1";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(id) AS lastCode FROM tbl_sales_return"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = code.ToString();
                }
            }

            return newCode;
        }

        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;

                invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_sales_return (date, customer_id, invoice_id,warehouse_id, po_num, bill_to,city,sales_man,
               ship_date,ship_via, ship_to, payment_method,account_cash_id, payment_terms, payment_date,  
               total,vat,net, pay, `change`, created_by, created_date, state, description) VALUES (@DATE, @customer_id, @invoice_id,@warehouse_id,
               @po_num, @bill_to,@city,@sales_man, @ship_date, @ship_via,@ship_to, @payment_method,@account_cash_id ,@payment_terms, @payment_date, 
               @total,  @vat,@net, @pay, @change, @created_by, @created_date, @state, @description);
               SELECT LAST_INSERT_ID();",
               DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("customer_id", cmbCustomer.SelectedValue),
               DBClass.CreateParameter("invoice_id", txtNextCode.Text),
               DBClass.CreateParameter("city", cmbCity.Text),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("po_num", ""),
               DBClass.CreateParameter("bill_to", txtBillTo.Text),
               DBClass.CreateParameter("ship_date", dtShip.Value.Date),
               DBClass.CreateParameter("sales_man", txtSalesMan.Text),
               DBClass.CreateParameter("ship_via", cmbShipVia.Text),
               DBClass.CreateParameter("ship_to", txtShipTo.Text),
               DBClass.CreateParameter("payment_method", cmbPaymentMethod.Text),
               DBClass.CreateParameter("account_cash_id", cmbPaymentMethod.Text == "Credit" ? level4PaymentCreditMethodId.ToString() : cmbAccountCashName.SelectedValue.ToString()),
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

            transaction();
            Utilities.LogAudit(frmLogin.userId, "Add Sales Return Invoice", "Sales Return", (int)invId, "Added Sales Return Invoice: " + txtNextCode.Text);

            return true;
        }
        private void transaction()
        {
            if (decimal.Parse(txtTotal.Text) > 0)
            {
                string _accountId = cmbPaymentMethod.Text == "Credit" ? level4PaymentCreditMethodId.ToString() : cmbAccountCashName.SelectedValue.ToString();
                CommonInsert.addTransactionEntry(dtInv.Value.Date, _accountId, "0", txtTotal.Text, invId.ToString(), cmbCustomer.SelectedValue.ToString(), "SalesReturn Invoice", "SALES RETURN", "SalesReturn Invoice NO. " + txtNextCode.Text,
                 frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);

                if (decimal.Parse(txtTotalVat.Text) > 0)
                {
                    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                  level4VatId.ToString(), txtTotalVat.Text, "0", invId.ToString(), "0", "SalesReturn Invoice", "SALES RETURN",
                  "Vat Output For Return Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
                }
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                  level4SalesReturnId.ToString(), txtTotalBefore.Text, "0", invId.ToString(), "0", "SalesReturn Invoice", "SALES RETURN",
                  "SalesReturn For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
            }
        }
        private void insertInvItems()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_sales_return_details (sales_id, item_id, qty,cost_price, price, vatp, vat, total, cost_center_id)
                                         VALUES (@sales_id, @item_id, @qty,@cost_price, @price, @vatp, @vat, @total, @costCenter);",
                  DBClass.CreateParameter("@sales_id", invId),
                  DBClass.CreateParameter("@item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                  DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value)),
                  DBClass.CreateParameter("@price", dgvItems.Rows[i].Cells["price"].Value == DBNull.Value || dgvItems.Rows[i].Cells["price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["price"].Value)),
                  DBClass.CreateParameter("@cost_price", dgvItems.Rows[i].Cells["cost_price"].Value == DBNull.Value || dgvItems.Rows[i].Cells["cost_price"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["cost_price"].Value)),
                  DBClass.CreateParameter("@vat", dgvItems.Rows[i].Cells["vat"].Value == null ? 0 : dgvItems.Rows[i].Cells["vat"].Value),
                  DBClass.CreateParameter("@vatp", dgvItems.Rows[i].Cells["vat"].Value == null ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["vatp"].Value)),
                  DBClass.CreateParameter("@total", dgvItems.Rows[i].Cells["total"].Value == null || dgvItems.Rows[i].Cells["total"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value)),
                  DBClass.CreateParameter("@costCenter", dgvItems.Rows[i].Cells["Cost_Center"].Value ?? 0));

                if (dgvItems.Rows[i].Cells["type"].Value.ToString() != "Service")
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id=@id",
                    DBClass.CreateParameter("id", dgvItems.Rows[i].Cells["itemId"].Value.ToString())))
                    {
                        reader.Read();
                        DBClass.ExecuteNonQuery("update tbl_items set on_hand=@qty where id = @id",
                            DBClass.CreateParameter("id", reader["id"].ToString()),
                            DBClass.CreateParameter("qty", decimal.Parse(reader["on_hand"].ToString()) + decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString())));
                        insertItemTransaction(dgvItems.Rows[i]);
                    }
                }
                //add cost center
                if (dgvItems.Rows[i].Cells["Cost_Center"].Value != null && int.Parse(dgvItems.Rows[i].Cells["Cost_Center"].Value.ToString()) > 0)
                    CommonInsert.InsertCostCenterTransaction(dtInv.Value, Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value ?? 0).ToString(), "0", invId.ToString(), "Sales Return", "", (dgvItems.Rows[i].Cells["Cost_Center"].Value ?? 0).ToString());
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

            CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Return Invoice", invId.ToString(), row.Cells["itemId"].Value.ToString(), row.Cells["cost_price"].Value.ToString(),
                        row.Cells["qty"].Value.ToString(), row.Cells["price"].Value.ToString(), "0", row.Cells["qty"].Value.ToString(), "SalesReturn Invoice No. " + txtNextCode.Text, cmbWarehouse.SelectedValue.ToString());

            //MySqlDataReader reader = null;
            //decimal cost_price = 0;
            //decimal totalCost = 0;

            //if (row.Cells["method"].Value.ToString() == "fifo")
            //{
            //    decimal remainingQty = qty;
            //    reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_item_transaction WHERE qty_inc > 0 AND item_id = @id ORDER BY id ASC",
            //        DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString()));

            //    while (reader.Read() && remainingQty > 0)
            //    {
            //        decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
            //        cost_price = Convert.ToDecimal(reader["cost_price"]);

            //        decimal qtyToUse = Math.Min(remainingQty, availableQty);
            //        remainingQty -= qtyToUse;
            //        totalCost += cost_price * qtyToUse;

            //        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Return Invoice", invId.ToString(), row.Cells["itemId"].Value.ToString(),
            //            cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), qtyToUse.ToString(), "0", "SalesReturn Invoice No. " + invCode);

            //        DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
            //            DBClass.CreateParameter("qty", qtyToUse),
            //            DBClass.CreateParameter("id", reader["id"].ToString()));
            //    }
            //}
            //else if (row.Cells["method"].Value.ToString() == "lifo")
            //{
            //    decimal remainingQty = qty;
            //    reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_item_transaction WHERE qty_inc > 0 AND item_id = @id ORDER BY id DESC",
            //        DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString()));

            //    while (reader.Read() && remainingQty > 0)
            //    {
            //        decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
            //        cost_price = Convert.ToDecimal(reader["cost_price"]);

            //        decimal qtyToUse = Math.Min(remainingQty, availableQty);
            //        remainingQty -= qtyToUse;
            //        totalCost += cost_price * qtyToUse;

            //        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Return Invoice", invId.ToString(), row.Cells["itemId"].Value.ToString(),
            //            cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), qtyToUse.ToString(), "0", "SalesReturn Invoice No. " + invCode);

            //        DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
            //            DBClass.CreateParameter("qty", qtyToUse),
            //            DBClass.CreateParameter("id", reader["id"].ToString()));
            //    }
            //}
            //else
            //{
            //    reader = DBClass.ExecuteReader(@"SELECT SUM(qty_in * cost_price) / SUM(qty_in) AS cost_price 
            //                             FROM tbl_item_transaction WHERE qty_in > 0 AND item_id = @id",
            //        DBClass.CreateParameter("id", row.Cells["itemId"].Value.ToString()));

            //    if (reader.Read() && reader["cost_price"] != DBNull.Value)
            //    {
            //        cost_price = Convert.ToDecimal(reader["cost_price"]);
            //    }

            //    totalCost = cost_price * qty;

            //    CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Return Invoice", invId.ToString(), row.Cells["itemId"].Value.ToString(),
            //        cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), qty.ToString(), "0", "SalesReturn Invoice No. " + invCode);
            //}

            //if (totalCost > 0)
            //{
            //    CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
            //        level4COGS.ToString(),
            //        totalCost.ToString(), "0",
            //        invId.ToString(), "0", "SalesReturn Invoice",
            //        "COGS For SalesReturnNo. " + invCode + " - Item Code - " + row.Cells["code"].Value.ToString(),
            //        frmLogin.userId, DateTime.Now.Date);

            //    CommonInsert.InsertTransactionEntry(dtInv.Value.Date,
            //        level4Inventory.ToString(),
            //        "0", totalCost.ToString(),
            //        invId.ToString(), "0", "SalesReturn Invoice",
            //        "Item Sold For SalesReturnNo. " + invCode + " - Item Code - " + row.Cells["code"].Value.ToString(),
            //        frmLogin.userId, DateTime.Now.Date);
            //}
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
                MessageBox.Show("Customer Must be Selected.");
                txtCustomerCode.Focus();
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
            txtSalesMan.Text = txtBillTo.Text = txtShipTo.Text =
                txtTotal.Text = txtTotalVat.Text = "";
            cmbPaymentMethod.SelectedIndex = id = 0;
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
                txtBillTo.Text = txtCustomerCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where id = " + cmbCustomer.SelectedValue.ToString()))
                if (reader.Read())
                {
                    txtCustomerCode.Text = reader["code"].ToString();
                    txtBillTo.Text = cmbCustomer.Text;
                }
                else
                    txtBillTo.Text = txtCustomerCode.Text = "";
        }
        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.Text == "Cash")
            {
                cmbPaymentTerms.SelectedIndex = -1;
                cmbAccountCashName.Enabled = true;
                cmbPaymentTerms.Enabled = false;
            }
            else if (cmbPaymentMethod.Text == "Credit")
            {
                cmbPaymentTerms.SelectedIndex = 0;
                cmbAccountCashName.Enabled = false;
                cmbPaymentTerms.Enabled = true;
            }
        }
        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where code =@code",
                DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (reader.Read())
                    cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());

            if (txtCustomerCode.Focused)
            {
                string input = txtCustomerCode.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    lstAccountSuggestions.Visible = false;
                    return;
                }
                string query = @"SELECT code, name FROM tbl_customer 
                         WHERE code LIKE @search OR name LIKE @search LIMIT 20";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

                lstAccountSuggestions.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
                }
                if (lstAccountSuggestions.Items.Count > 0)
                {
                    Point locationOnForm = txtCustomerCode.Parent.PointToScreen(txtCustomerCode.Location);
                    Point locationRelativeToForm = this.PointToClient(locationOnForm);

                    lstAccountSuggestions.SetBounds(
                        locationRelativeToForm.X,
                        locationRelativeToForm.Y + txtCustomerCode.Height,
                        txtCustomerCode.Width + 100,
                        120
                    );

                    lstAccountSuggestions.Tag = txtCustomerCode;
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
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where code =@code",
            DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (!reader.Read())
                    cmbCustomer.SelectedIndex = -1;

            BeginInvoke((Action)(() =>
            {
                if (!lstAccountSuggestions.Focused)
                    lstAccountSuggestions.Visible = false;
            }));
        }
        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int colIndex = dgvItems.CurrentCell.ColumnIndex;

            if (e.Control is TextBox textBoxBase)
            {
                textBoxBase.TextChanged -= ItemCode_TextChanged;
                textBoxBase.TextChanged -= ItemName_TextChanged;
                textBoxBase.KeyPress -= NumericColumn_KeyPress;

                if (lstAccountSuggestions.Visible)
                    lstAccountSuggestions.Visible = false;

                lstAccountSuggestions.SendToBack();
            }

            if (colIndex == dgvItems.Columns["name"].Index)
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.TextChanged -= ItemName_TextChanged;
                    textBox.TextChanged += ItemName_TextChanged;
                    lstAccountSuggestions.Tag = textBox;
                }
            }
            else if (colIndex == dgvItems.Columns["vat"].Index)
            {
                if (e.Control is ComboBox comboBox)
                {
                    comboBox.SelectedIndexChanged -= ComboBoxTax_SelectedIndexChanged;
                    comboBox.SelectedIndexChanged += ComboBoxTax_SelectedIndexChanged;
                }
            }
            else if (colIndex == dgvItems.Columns["code"].Index)
            {
                if (e.Control is TextBox textBox)
                {
                    textBox.TextChanged -= ItemCode_TextChanged;
                    textBox.TextChanged += ItemCode_TextChanged;
                    lstAccountSuggestions.Tag = textBox;
                }
            }
            else if (dgvItems.CurrentCell.OwningColumn.Name == "qty" ||
                     dgvItems.CurrentCell.OwningColumn.Name == "price")
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
                if (row == null) return;

                string taxId = selectedRow["id"].ToString();
                string taxP = selectedRow["value"].ToString();

                decimal price = GetDecimalValue(row, "price");
                decimal qty = GetDecimalValue(row, "qty");

                if (price == 0 || qty == 0)
                {
                    row.Cells["vatp"].Value = row.Cells["total"].Value = "0";
                }
                else
                {
                    decimal net = (price * qty);
                    row.Cells["vat"].Value = Convert.ToInt32(taxId);
                    row.Cells["vatp"].Value = (net * decimal.Parse(taxP) / 100);
                    row.Cells["total"].Value = net + Convert.ToDecimal(row.Cells["vatp"].Value);
                }

                ChkRowValidity();
            }
        }
        private void ChkRowValidity()
        {
            var row = dgvItems.CurrentRow;
            if (row == null) return;

            decimal price = GetDecimalValue(dgvItems.CurrentRow, "price");
            decimal qty = GetDecimalValue(dgvItems.CurrentRow, "qty");

            if (price == 0 || qty == 0)
            {
                row.Cells["vatp"].Value = "0";
                row.Cells["total"].Value = "0";
                return;
            }

            try
            {
                decimal netPrice = (qty * price);

                string vatId = row.Cells["vat"].Value?.ToString() ?? "0";
                string vP = "0";

                DataTable vatTable = ((DataGridViewComboBoxColumn)dgvItems.Columns["vat"]).DataSource as DataTable;
                if (vatTable != null)
                {
                    DataRow[] matches = vatTable.Select("id = " + vatId);
                    if (matches.Length > 0)
                    {
                        vP = matches[0]["value"].ToString();
                    }
                }

                decimal vatValue = netPrice * Convert.ToDecimal(vP) / 100;
                row.Cells["vatp"].Value = vatValue;
                row.Cells["total"].Value = netPrice + vatValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("VAT calc error: " + ex.Message);
            }
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
                if (e.ColumnIndex == dgvItems.Columns["price"].Index && dgvItems.Rows[e.RowIndex].Cells["type"].Value != null && dgvItems.Rows[e.RowIndex].Cells["type"].Value.ToString() != "Service")
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
                    if (dgvItems.CurrentRow.Cells["itemId"].Value == null)
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
            if (string.IsNullOrWhiteSpace(txtNextCode.Text))
                return;

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

            string query = "select id from tbl_sales_return where state = 0 and id =@id";

            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clearData();
                    MessageBox.Show("No Previous Invoice Found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
                return;

            int? currentId = Convert.ToInt32(txtId.Text); // Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_sales_return WHERE state = 0 AND id =@id";

            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clearData();
                    MessageBox.Show("No Next Invoice Found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void lnkNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //frmViewCustomer frm = new frmViewCustomer(_mainForm, this, 0);
            //_mainForm.openChildForm(frm);
        }

        private void frmSalesReturn_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Customer -= customerUpdatedHandler;
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

        public DataTable COMPANYINFO(int id)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company ", DBClass.CreateParameter("@1", id));
        }

        public DataTable salesDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT s.id,s.date,s.customer_id,s.invoice_id,s.bill_to,s.city,s.sales_man,s.ship_date,s.ship_via,s.ship_to,(SELECT NAME FROM tbl_coa_level_4 WHERE id=s.account_cash_id) accountName,s.po_num,s.payment_method,s.payment_terms,s.payment_date,s.total,s.vat,s.net,s.pay,s.change 
                                    ,c.name customerName,c.main_phone,c.email,c.trn FROM tbl_sales_return s,tbl_customer c WHERE s.customer_id = c.id AND s.id AND s.id=@salesId;", DBClass.CreateParameter("@salesId", a1));
        }
        public DataTable ItemDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT d.id,d.item_id,i.name,d.qty,d.cost_price,d.price,(d.qty*d.cost_price) subCostTotal,(d.qty*d.price) subPriceTotal,d.discount,((d.qty*d.price)-d.discount) subTotal,d.vatp vatAmount,d.vat vatPercentage,d.total,d.cost_center_id,(SELECT NAME FROM tbl_sub_cost_center WHERE id=cost_center_id) costCenterName,i.type,i.method,i.unit_id,(select name from tbl_unit WHERE id=i.unit_id) unitName, i.code as code FROM tbl_sales_return_details d 
                                       INNER JOIN tbl_items i ON d.item_id = i.id WHERE d.sales_id=@salesId; ", DBClass.CreateParameter("@salesId", a1));
        }

        public void ShowReport()
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "SalesReturnInvoice.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                DataTable companyData = COMPANYINFO(1);
                DataTable saleData = salesDetails(invId.ToString());
                DataTable itemData = ItemDetails(invId.ToString());
                if (companyData != null)
                {
                    cr.Subreports["CompanyHeader"].SetDataSource(companyData);
                    cr.Subreports["InvoiceHeader"].SetDataSource(saleData);
                    cr.Subreports["ItemDetails"].SetDataSource(itemData);
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;
                }

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
            preview();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            {
                MessageBox.Show("The Quotation Invoice  Update");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }


            if (id == 0)
            {
                if (insertInvoice())
                {
                    EventHub.RefreshSales();
                    MessageBox.Show("The Return Invoice  Saved");
                    dgvItems.Rows.Clear();
                    this.Close();
                    if (checkBox4.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
            else
            {
                if (updateInvoice())
                {
                    EventHub.RefreshSales();
                    MessageBox.Show("The Return Invoice  Saved");
                    dgvItems.Rows.Clear();
                    this.Close();
                    if (checkBox4.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {
            DBClass.ExecuteNonQuery("UPDATE tbl_sales_return SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", id.ToString()));
            CommonInsert.DeleteItemTransaction("Sales Return", id.ToString());
            Utilities.LogAudit(frmLogin.userId, "Delete Sales Return", "Sales Return", id, "Deleted Sales Return Invoice No. " + txtNextCode.Text);
            clearData();
        }

        private void guna2TileButton20_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {
            clearData();
        }
        private void clearData()
        {
            resetTextBox();
            id = 0; 
            txtNextCode.Text = GenerateNextSalesCode();
            txtId.Text = GenerateNextSalesId().ToString();
        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void guna2TileButton28_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmSalesByCustomerDetails(int.Parse(cmbCustomer.SelectedValue.ToString()), "tbl_sales_return"));
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

                if (gunaTextBox == txtCustomerCode)
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_customer where code =@code",
                        DBClass.CreateParameter("code", selectedCode)))
                        if (reader.Read())
                            cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());
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

        // Example method to get data for subreport 1 (replace with your actual logic)
        private DataTable GetSubreportData1()
        {
            // Fetch or prepare data for subreport 1
            DataTable subreportData = new DataTable();
            // Your logic to fetch data for subreport 1
            return subreportData;
        }

        // Example method to get data for subreport 2 (replace with your actual logic)
        private DataTable GetSubreportData2()
        {
            // Fetch or prepare data for subreport 2
            DataTable subreportData = new DataTable();
            // Your logic to fetch data for subreport 2
            return subreportData;
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
