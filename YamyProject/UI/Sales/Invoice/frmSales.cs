using CrystalDecisions.CrystalReports.Engine;
using DocumentFormat.OpenXml.Wordprocessing;
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
    public partial class frmSales : Form
    {
        private EventHandler customerUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;

        decimal invId;
        int level4PaymentCreditMethodId, level4VatId, level4SalesInvoice, level4COGS, level4Inventory;
        int id,defaultCustomerId=0;
        bool AllowItemWithOutQty = true, EnableBarcode = false;
        string formType;

        public frmSales(int _id = 0, string _formType = "",int _customerId=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            warehouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            EventHub.Customer += customerUpdatedHandler;
            EventHub.wareHouse += warehouseUpdatedHandler;
            this.formType = _formType;
            if (formType == "SQ") {
                txtPONO.Text = _id.ToString();
            } else if (formType == "SO")
            {
                txtPONO.Text = _id.ToString();
            }
            else
            {
                this.id = _id;
                txtPONO.Text = "";
                this.defaultCustomerId = _customerId;
            }
            headerUC1.FormText = id == 0 || formType != "" ? "New Tax Invoice" : "Edit Tax Invoice";
        }
        private void frmSales_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Customer -= customerUpdatedHandler;
            EventHub.wareHouse -= warehouseUpdatedHandler;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmSales_Load(object sender, EventArgs e)
        {
            txtNextCode.Text = GenerateNextSalesCode();
            guna2Panel1.Height = 0;
            dtPaymentTerms.Value = dtInv.Value = dtShip.Value = DateTime.Now.Date;
            var generalS = Utilities.GeneralSettingsState("ALLOW ITEM WITHOUT QTY");
            if (!string.IsNullOrEmpty(generalS) & int.Parse(generalS) > 0)
            {
                AllowItemWithOutQty = true;
            }
            else
            {
                AllowItemWithOutQty = false;
            }
            var barcodeS = Utilities.GeneralSettingsState("ENABLE BARCODE");
            if (!string.IsNullOrEmpty(barcodeS) && int.Parse(barcodeS) > 0)
            {
                EnableBarcode = true;
            }
            else
            {
                EnableBarcode = false;
            }

            BindCombos.PopulateWarehouse(cmbWarehouse);
            loadVat();
            loadCostCenter();
            cmbPaymentMethod.SelectedIndex = 0;
            bindCombo();
            txtId.Text = GenerateNextSalesId();
            if (formType == "")
            {
                if (id != 0)
                {
                    guna2Button10.Enabled = btnSave.Enabled = UserPermissions.canEdit("Create Invoice");
                    BindInvoice();
                }
                else
                {
                    if (defaultCustomerId > 0)
                    {
                        cmbCustomer.SelectedValue = defaultCustomerId;
                    }
                }
            }
            else
            {
                if (id == 0 && formType== "SQ")
                {
                    LoadSalesQuotationData();
                }
                else if (id == 0 && formType == "SO")
                {
                    loadSalesOrderData();
                }
            }

            if (EnableBarcode)
            {
                txtBarcodeScan.Visible = true;
                this.ActiveControl = txtBarcodeScan;
            }
        }

        private bool suppressCellValueChanged = false;

        private void TxtBarcodeScan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string scannedCode = txtBarcodeScan.Text.Trim();
                if (!string.IsNullOrEmpty(scannedCode))
                {
                    InsertItemByBarcode(scannedCode);
                    txtBarcodeScan.Clear();
                }
            }
        }

        private void InsertItemByBarcode(string barcode)
        {
            suppressCellValueChanged = true;
            // 1. Fetch the item from DB
            DataTable dt = DBClass.ExecuteDataTable(
                "SELECT id, method, type, code, IFNULL(sales_price,0) sales_price, IFNULL(cost_price,0) cost_price, name FROM tbl_items WHERE barcode = @barcode",
                DBClass.CreateParameter("@barcode", barcode)
            );

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Item not found for barcode: " + barcode);
                return;
            }

            // Always reference this row freshly
            DataRow item = dt.Rows[0];

            string itemCode = item["code"].ToString();
            string itemName = item["name"].ToString();
            string itemId = item["id"].ToString();
            string itemMethod = item["method"].ToString();
            string itemType = item["type"].ToString();
            decimal salesPrice = Convert.ToDecimal(item["sales_price"]);
            decimal costPrice = Convert.ToDecimal(item["cost_price"]);

            // 2. Check if item already in grid, then increase qty
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["code"].Value?.ToString() == itemCode)
                {
                    decimal currentQty = GetDecimalValue(row, "qty");
                    if (CheckItemValidity(int.Parse(row.Cells["itemId"].Value.ToString()), currentQty + 1))
                        row.Cells["qty"].Value = currentQty + 1;

                    ChkRowValidity(row);
                    CalculateTotal();
                    return;
                }
            }

            // 3. Add a new row for new item
            int rowIndex = dgvItems.Rows.Add();
            var newRow = dgvItems.Rows[rowIndex];

            newRow.Cells["itemId"].Value = itemId;
            newRow.Cells["code"].Value = itemCode;
            newRow.Cells["name"].Value = itemName;
            newRow.Cells["qty"].Value = 1;
            newRow.Cells["discount"].Value = 0;
            newRow.Cells["cost_price"].Value = costPrice;
            newRow.Cells["price"].Value = salesPrice;
            newRow.Cells["method"].Value = itemMethod;
            newRow.Cells["type"].Value = itemType;

            if (!string.IsNullOrEmpty(defaultTax))
                newRow.Cells["vat"].Value = int.Parse(defaultTax);

            decimal discount = 0;
            decimal qty = 1;
            decimal netPrice = (salesPrice - discount) * qty;
            decimal vatAmount = 0;
            if (!string.IsNullOrEmpty(defaultTax))
            {
                int vatRate = int.Parse(defaultTax);
                vatAmount = netPrice * vatRate / 100;
            }

            newRow.Cells["net_price"].Value = netPrice.ToString("0.00");
            newRow.Cells["VatP"].Value = vatAmount;
            newRow.Cells["total"].Value = (netPrice + vatAmount).ToString("0.00");

            //remove this row if no qty available in stock
            if (itemType == "11 - Inventory Part" && !CheckItemValidity(int.Parse(itemId), 1))
            {
                dgvItems.Rows.Remove(newRow);
                MessageBox.Show("No Qty Available for Item: " + itemName);
                suppressCellValueChanged = false;

                return;
            }
            else
            {
                ChkRowValidity(newRow);
                CalculateTotal();
            }
            suppressCellValueChanged = false;
        }

        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_sales where id = @id",
                DBClass.CreateParameter("id", id)))
                if (reader.Read())
                {
                    txtId.Text = reader["id"].ToString();
                    dtInv.Value = DateTime.Parse(reader["date"].ToString());
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
                    richTextDescription.Text = reader["description"].ToString();
                    cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                    txtNextCode.Text = reader["invoice_id"].ToString();
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
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_sales_details.*,tbl_items.type,method, tbl_items.code, tbl_items.name FROM tbl_sales_details INNER JOIN 
                                                                    tbl_items ON tbl_sales_details.item_id = tbl_items.id WHERE 
                                                                    tbl_sales_details.sales_id = @id;",
                                                            DBClass.CreateParameter("id", id)))
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["item_id"].ToString(), "", reader["code"].ToString(), reader["name"].ToString(), Utilities.FormatDecimal(reader["qty"]),
                        Utilities.FormatDecimal(reader["cost_price"]), Utilities.FormatDecimal(reader["price"]), Utilities.FormatDecimal(reader["discount"]));
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
                    decimal netPrice = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) * decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["price"].Value.ToString())) - (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["discount"].Value.ToString()));
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["total"].Value = (netPrice + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString()));
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = reader["method"].ToString();
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = reader["type"].ToString();
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
                // Use warehouse-specific query
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
                // Use general items query
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

            // Filter by name if provided
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
                // Use warehouse-specific join query
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
                // General items query
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

            // Apply filter by code if provided
            if (!string.IsNullOrWhiteSpace(code))
            {
                query += " AND code LIKE @code";
                parameters.Add(new MySqlParameter("@code", $"%{code}%"));
            }

            query += " ORDER BY code LIMIT 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private void loadVat()
        {
            DataTable dt = DBClass.ExecuteDataTable("SELECT id, CONCAT(name, '-', value, '%') AS display_text, value FROM tbl_tax");

            DataGridViewComboBoxColumn vatCol = (DataGridViewComboBoxColumn)dgvItems.Columns["vat"];
            vatCol.DataSource = dt;
            vatCol.DisplayMember = "display_text";
            vatCol.ValueMember = "id";

            string dTax = Utilities.GeneralSettings("DEFAULT TAX PERCENTAGE");
            if (!string.IsNullOrEmpty(dTax) && int.TryParse(dTax, out int val) && val > 0)
            {
                defaultTax = dTax;
            }
            //DataTable dt = DBClass.ExecuteDataTable("select id, concat(name,'-',value , '%') as name,value from tbl_tax");
            //DataGridViewComboBoxColumn vat = (DataGridViewComboBoxColumn)dgvItems.Columns["vat"];
            //vat.DataSource = dt;
            //vat.DisplayMember = "name";
            //vat.ValueMember = "id";
            //var dTax = Utilities.GeneralSettings("DEFAULT TAX PERCENTAGE");
            //if (!string.IsNullOrEmpty(dTax) & int.Parse(dTax) > 0)
            //{
            //    defaultTax = dTax;
            //}
        }
        string defaultTax = "";
        private void loadCostCenter()
        {
            DataTable dt = DBClass.ExecuteDataTable("select id,code as name from tbl_sub_cost_center");
            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)dgvItems.Columns["cost_center"];
            col.DataSource = dt;
            col.DisplayMember = "name";
            col.ValueMember = "id";
        }
        private void bindCombo()
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            BindCombos.PopulateAllLevel4Account(cmbAccountCashName);

            cmbAccountCashName.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Invoice Payment Cash Method")
                ? frmLogin.defaultAccounts["Invoice Payment Cash Method"] : 0;

            level4PaymentCreditMethodId = frmLogin.defaultAccounts.ContainsKey("Customer") ? frmLogin.defaultAccounts["Customer"] : 0;
            level4VatId = frmLogin.defaultAccounts.ContainsKey("Vat Output") ? frmLogin.defaultAccounts["Vat Output"] : 0;
            level4SalesInvoice = frmLogin.defaultAccounts.ContainsKey("Sales") ? frmLogin.defaultAccounts["Sales"] : 0;
            level4COGS = frmLogin.defaultAccounts.ContainsKey("COGS") ? frmLogin.defaultAccounts["COGS"] : 0;
            level4Inventory = frmLogin.defaultAccounts.ContainsKey("Inventory") ? frmLogin.defaultAccounts["Inventory"] : 0;

        }
        private string GenerateNextSalesCode()
        {
            string newCode = "SI-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(invoice_id, 4) AS UNSIGNED)) AS lastCode FROM tbl_sales"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "SI-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private string GenerateNextSalesId()
        {
            string newCode = "1";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(id) AS lastCode FROM tbl_sales"))
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
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            {
                MessageBox.Show("Can't Save Empty Tax Invoice");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            if (!Utilities.AreDefaultAccountsSet(new List<string> { "Sales", "COGS", "Customer", "Vat Output", "Inventory" }))
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CheckQtyAvailable();
            if (isSaved)
            {
                MessageBox.Show("The Tax Invoice  Saved");
                dgvItems.Rows.Clear();

                if (chkPrint.Checked == true)
                {
                    ShowReport();
                }
            }
            else
            {
                MessageBox.Show("Please check stock before save! or Correct the Entry");
            }
        }
        bool isSaved = false;
        private void CheckQtyAvailable()
        {
            bool allItemsAvailable = true;

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                int itemId = Convert.ToInt32(dgvItems.Rows[i].Cells["itemId"].Value);
                decimal qty = dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value);

                if (!CheckItemValidity(itemId, qty))
                {
                    allItemsAvailable = false;
                    break;
                }
            }

            if (allItemsAvailable)
            {
                Save_Action();
            }
            else
            {

                if (AllowItemWithOutQty)
                {
                    DialogResult result = MessageBox.Show("No Qty Available for some Item! Do you want to proceed?",
                                                      "Warning",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        Save_Action();
                        isSaved = true;
                    }
                    else
                    {
                        isSaved = false;
                    }
                }
                else
                {
                    isSaved = false;
                }
            }
        }
        private void Save_Action()
        {
            if (id == 0)
            {
                if (InsertInvoice())
                {
                    EventHub.RefreshSales();
                    //loadPrint();
                    EventHub.RefreshPurchase();
                    if (formType == "SQ")
                    {
                        EventHub.RefreshQuotation();
                    }
                    else if (formType == "SO")
                    {
                        EventHub.RefreshSalesOrder();
                    }
                    isSaved = true;
                } else { 
                    isSaved = false; 
                }
            }
            else
            {
                if (UpdateInvoice())
                {
                    EventHub.RefreshSales();
                    //loadPrint();
                    EventHub.RefreshPurchase();
                    if (formType == "SQ")
                    {
                        EventHub.RefreshQuotation();
                    }
                    else if (formType == "SO")
                    {
                        EventHub.RefreshSalesOrder();
                    }
                    isSaved = true;
                }
                else
                {
                    isSaved = false;
                }
            }
        }

        private bool UpdateInvoice()
        {
            if (!ChkRequiredData())
                return false;

            decimal paidAmount = string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text);
            decimal changeAmount = string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text);
            if (cmbPaymentMethod.Text != "Cash")
            {
                object objOldPayment = DBClass.ExecuteScalar(@"SELECT IFNULL(sum(payment),0) amount FROM tbl_receipt_voucher_details WHERE inv_id =@id",
                            DBClass.CreateParameter("@id", id));
                decimal oldPaymentTotal = (objOldPayment != null && objOldPayment != DBNull.Value) ? decimal.Parse(objOldPayment.ToString()) : 0;
                string total = txtTotal.Text;
                if (oldPaymentTotal > 0)
                {
                    paidAmount = oldPaymentTotal;
                }

                changeAmount = decimal.Parse(total);
                paidAmount = 0;
            } else
            {
                changeAmount = 0;
            }

            DBClass.ExecuteNonQuery(@"UPDATE tbl_sales 
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
               DBClass.CreateParameter("po_num", txtPONO.Text),
               DBClass.CreateParameter("bill_to", txtBillTo.Text),
               DBClass.CreateParameter("ship_date", dtShip.Value.Date),

               DBClass.CreateParameter("sales_man", txtSalesMan.Text),
               DBClass.CreateParameter("ship_via", cmbShipVia.Text),
               DBClass.CreateParameter("ship_to", txtShipTo.Text),
               DBClass.CreateParameter("payment_method", cmbPaymentMethod.Text),
               DBClass.CreateParameter("account_cash_id", cmbAccountCashName.SelectedValue == null ? "0" : cmbAccountCashName.SelectedValue.ToString()),
               DBClass.CreateParameter("payment_terms", cmbPaymentTerms.Text),
               DBClass.CreateParameter("payment_date", dtPaymentTerms.Value.Date),
               DBClass.CreateParameter("vat", txtTotalVat.Text),
               DBClass.CreateParameter("total", txtTotalBefore.Text),
               DBClass.CreateParameter("net", txtTotal.Text),
               DBClass.CreateParameter("description", richTextDescription.Text),
               DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
               DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date),
               DBClass.CreateParameter("pay", paidAmount),
               DBClass.CreateParameter("change", changeAmount));
            
            ReturnItemsToInventory();
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_sales_details WHERE sales_id = @salesDetailId", DBClass.CreateParameter("salesDetailId", id.ToString()));
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction WHERE reference = @invId AND type = 'Sales Invoice';DELETE FROM tbl_item_card_details 
                            WHERE trans_type = 'Sales Invoice' and trans_no=@invId", DBClass.CreateParameter("invId", id));
            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "Sales");
            CommonInsert.DeleteTransactionEntry(id, "SALES");
            InsertInvItems();
            Transaction();
            Utilities.LogAudit(frmLogin.userId, "Update Sales Invoice", "Sales Invoice", id, "Updated Sales Invoice: " + txtNextCode.Text);

            return true;
        }
        private void ReturnItemsToInventory()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                    SELECT tbl_sales_details.*, tbl_items.method, tbl_items.type 
                    FROM tbl_sales_details
                    INNER JOIN tbl_items ON tbl_sales_details.item_id = tbl_items.id 
                    WHERE sales_id = @salesId",
                DBClass.CreateParameter("salesId", id)))
            {
                while (reader.Read())
                {
                    int itemId = Convert.ToInt32(reader["item_id"]);
                    decimal qtyToReturn = Convert.ToDecimal(reader["qty"]);
                    string method = reader["method"].ToString();
                    string itemType = reader["type"].ToString();

                    if (itemType == "12 - Service")
                        continue;

                    if (itemType == "13 - Inventory Assembly")
                    {
                        using (MySqlDataReader componentReader = DBClass.ExecuteReader(@"
                            SELECT item_id, qty 
                            FROM tbl_item_assembly 
                            WHERE assembly_id = @assemblyId",
                            DBClass.CreateParameter("assemblyId", itemId)))
                        {
                            while (componentReader.Read())
                            {
                                int componentId = Convert.ToInt32(componentReader["item_id"]);
                                decimal componentQty = Convert.ToDecimal(componentReader["qty"]) * qtyToReturn;

                                DBClass.ExecuteNonQuery(@"
                                    UPDATE tbl_items 
                                    SET on_hand = on_hand + @qty 
                                    WHERE id = @componentId",
                                    DBClass.CreateParameter("qty", componentQty),
                                    DBClass.CreateParameter("componentId", componentId));

                                ProcessInventoryReturn(componentId, componentQty, method);
                            }
                        }
                    }
                    else
                    {
                        DBClass.ExecuteNonQuery(@"
                            UPDATE tbl_items 
                            SET on_hand = on_hand + @qty 
                            WHERE id = @itemId",
                            DBClass.CreateParameter("qty", qtyToReturn),
                            DBClass.CreateParameter("itemId", itemId));

                        ProcessInventoryReturn(itemId, qtyToReturn, method);
                    }
                }
            }
        }
        private void ProcessInventoryReturn(int itemId, decimal qtyToReturn, string method)
        {
            if (method != "fifo" && method != "lifo")
                return;

            string orderBy = method == "fifo" ? "ASC" : "DESC";

            using (MySqlDataReader tReader = DBClass.ExecuteReader(@"
                SELECT * FROM tbl_item_transaction 
                WHERE item_id = @itemId AND qty_in > qty_inc AND qty_out = 0 
                ORDER BY id " + orderBy,
                DBClass.CreateParameter("itemId", itemId)))
            {
                while (tReader.Read() && qtyToReturn > 0)
                {
                    int transactionId = Convert.ToInt32(tReader["id"]);
                    decimal qtyIn = Convert.ToDecimal(tReader["qty_in"]);
                    decimal qtyInc = Convert.ToDecimal(tReader["qty_inc"]);
                    decimal availableQty = qtyIn - qtyInc;

                    if (qtyToReturn <= availableQty)
                    {
                        DBClass.ExecuteNonQuery(@"
                        UPDATE tbl_item_transaction 
                        SET qty_inc = qty_inc + @qty 
                        WHERE id = @transactionId",
                            DBClass.CreateParameter("qty", qtyToReturn),
                            DBClass.CreateParameter("transactionId", transactionId));

                        qtyToReturn = 0;
                    }
                    else
                    {
                        DBClass.ExecuteNonQuery(@"
                        UPDATE tbl_item_transaction 
                        SET qty_inc = qty_in 
                        WHERE id = @transactionId",
                            DBClass.CreateParameter("transactionId", transactionId));

                        qtyToReturn -= availableQty;
                    }
                }
            }
        }
        private bool InsertInvoice()
        {
            if (!ChkRequiredData())
                return false;

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_sales (date, customer_id, invoice_id,warehouse_id, po_num, bill_to,city,sales_man,
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
               DBClass.CreateParameter("po_num", txtPONO.Text),
               DBClass.CreateParameter("bill_to", txtBillTo.Text),
               DBClass.CreateParameter("ship_date", dtShip.Value.Date),
               DBClass.CreateParameter("sales_man", txtSalesMan.Text),
               DBClass.CreateParameter("ship_via", cmbShipVia.Text),
               DBClass.CreateParameter("ship_to", txtShipTo.Text),
               DBClass.CreateParameter("payment_method", cmbPaymentMethod.Text),
               DBClass.CreateParameter("account_cash_id", cmbAccountCashName.SelectedValue.ToString()),
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
            InsertInvItems();
            
            TransferSale();
            Transaction();
            Utilities.LogAudit(frmLogin.userId, "Create Invoice", "Sales Invoice", (int)invId, "Created Sales Invoice No. " + txtNextCode.Text);

            return true;
        }
        private void Transaction()
        {
            if (decimal.Parse(txtTotal.Text) > 0)
            {
                string _accountId = cmbPaymentMethod.Text == "Credit" ? level4PaymentCreditMethodId.ToString()
                       : cmbAccountCashName.SelectedValue.ToString();
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                       _accountId,
                       txtTotal.Text, "0", invId.ToString(), cmbCustomer.SelectedValue.ToString(), cmbPaymentMethod.Text == "Credit" ? "Sales Invoice" : "Sales Invoice Cash","SALES", "Sales Invoice NO. " + txtNextCode.Text,
                       frmLogin.userId, DateTime.Now.Date,txtNextCode.Text);


                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                  level4SalesInvoice.ToString(), "0", txtTotalBefore.Text, invId.ToString(), "0", cmbPaymentMethod.Text == "Credit" ? "Sales Invoice" : "Sales Invoice Cash", "SALES",
                  "Sales Revenue For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);

                if (decimal.Parse(txtTotalVat.Text) > 0)
                {
                    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                  level4VatId.ToString(), "0", txtTotalVat.Text, invId.ToString(), "0", cmbPaymentMethod.Text == "Credit" ? "Sales Invoice" : "Sales Invoice Cash","SALES",
                  "Vat Output For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
                }

                object netResult = DBClass.ExecuteScalar("SELECT account_id FROM tbl_warehouse WHERE id = @id", DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
                int level4Id = netResult != DBNull.Value ? Convert.ToInt32(netResult) : 0;
                if (itemInventoryCost>0)
                {
                    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                        level4COGS.ToString(), itemInventoryCost.ToString("N2"), "0",
                        invId.ToString(), "0", "Sales Invoice", "SALES",
                        "COGS For Sales No. " + txtNextCode.Text,
                        frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);

                    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                            level4Id.ToString(), "0", itemInventoryCost.ToString("N2"),
                            invId.ToString(), "0", "Sales Invoice", "SALES",
                            "Item Sold For Sales No. " + txtNextCode.Text,
                            frmLogin.userId, DateTime.Now.Date, txtNextCode.Text
                        );
                }
            }
        }
        private void TransferSale()
        {
            if (formType == "SQ")
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_sales_quotation set tranfer_status=1, sales_id=@id where id=@sqId", DBClass.CreateParameter("sqId", txtPONO.Text), DBClass.CreateParameter("id", invId));
            } else if(formType=="SO")
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_sales_order set tranfer_status=1, sales_id=@id where id=@soId", DBClass.CreateParameter("soId", txtPONO.Text), DBClass.CreateParameter("id",invId));
            }
        }

        bool CheckItemAvailabilityOld(int itemId, decimal salesQty)
        {
            decimal totalQtyInGrid = 0;

            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id = @id",
                DBClass.CreateParameter("id", itemId)))
            {
                if (reader.Read())
                {
                    string itemType = reader["type"].ToString();

                    if (itemType == "12 - Service")
                        return true;

                    if (itemType == "11 - Inventory Part")
                    {
                        totalQtyInGrid = GetTotalQtyInGrid(itemId);

                        decimal onHand = decimal.Parse(reader["on_hand"].ToString());
                        if (totalQtyInGrid > onHand)
                        {
                            return false;
                        }
                    }
                    else if (itemType == "13 - Inventory Assembly")
                    {
                        using (MySqlDataReader assemblyReader = DBClass.ExecuteReader(@"
                            SELECT   a.qty, i.on_hand, i.name
                            FROM tbl_item_assembly a
                            JOIN tbl_items i ON a.item_id = i.id
                            WHERE a.assembly_id = @assemblyId",
                            DBClass.CreateParameter("@assemblyId", itemId)))
                        {
                            while (assemblyReader.Read())
                            {
                                string ComponentName = assemblyReader["name"].ToString();
                                decimal requiredPerUnit = Convert.ToDecimal(assemblyReader["qty"]);
                                decimal availableQty = Convert.ToDecimal(assemblyReader["on_hand"]);
                                decimal totalRequiredQty = GetTotalQtyInGrid(itemId) * requiredPerUnit;
                                if (totalRequiredQty > availableQty)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        decimal itemInventoryCost = 0;
        private void InsertInvItems()
        {
            decimal totalCostAllItems = 0;
            List<CostItem> costList = new List<CostItem>();
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                int itemId = Convert.ToInt32(dgvItems.Rows[i].Cells["itemId"].Value);
                decimal qty = dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value);
                decimal discount = string.IsNullOrWhiteSpace(dgvItems.Rows[i].Cells["discount"].Value?.ToString()) ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["discount"].Value);
                string itemType = dgvItems.Rows[i].Cells["type"].Value.ToString();
                string method = dgvItems.Rows[i].Cells["method"].Value.ToString();
                decimal costAmount = 0;

                DBClass.ExecuteNonQuery(@"
                    INSERT INTO tbl_sales_details (sales_id, item_id, qty, cost_price, price,discount, vatp, vat, total,cost_center_id)
                    VALUES (@sales_id, @item_id, @qty, @cost_price, @price,@discount, @vatp, @vat, @total,@costCenter);",
                        DBClass.CreateParameter("sales_id", invId),
                        DBClass.CreateParameter("item_id", itemId),
                        DBClass.CreateParameter("qty", qty),
                        DBClass.CreateParameter("price", dgvItems.Rows[i].Cells["price"].Value ?? 0),
                        DBClass.CreateParameter("cost_price", dgvItems.Rows[i].Cells["cost_price"].Value.ToString() == "" ? null : dgvItems.Rows[i].Cells["cost_price"].Value.ToString()),
                        DBClass.CreateParameter("discount", discount),
                        DBClass.CreateParameter("vat", dgvItems.Rows[i].Cells["vat"].Value ?? 0),
                        DBClass.CreateParameter("vatp", Convert.ToDecimal(dgvItems.Rows[i].Cells["vatp"].Value ?? 0)),
                        DBClass.CreateParameter("total", Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value ?? 0)),
                        DBClass.CreateParameter("@costCenter", dgvItems.Rows[i].Cells["cost_center"].Value ?? 0)
                    );

                if (itemType == "12 - Service") continue;

                if (itemType == "13 - Inventory Assembly")
                {
                    using (MySqlDataReader componentReader = DBClass.ExecuteReader(@"
                            SELECT item_id, qty,(select method FROM tbl_items WHERE tbl_items.id = tbl_item_assembly.item_id) as method 
                            FROM tbl_item_assembly 
                            WHERE assembly_id = @assemblyId",
                        DBClass.CreateParameter("assemblyId", itemId)))
                    {
                        while (componentReader.Read())
                        {
                            int componentId = Convert.ToInt32(componentReader["item_id"]);
                            decimal componentQty = Convert.ToDecimal(componentReader["qty"]) * qty;
                            string methodOfIngredients = componentReader["method"].ToString().Trim();

                            DBClass.ExecuteNonQuery(@"
                                UPDATE tbl_items 
                                SET on_hand = on_hand - @qty 
                                WHERE id = @componentId",
                                DBClass.CreateParameter("qty", componentQty),
                                DBClass.CreateParameter("componentId", componentId));

                            decimal costReturned = InsertItemTransaction(dgvItems.Rows[i], componentId, componentQty, methodOfIngredients);
                            costAmount = costReturned;
                            totalCostAllItems += costReturned;
                        }
                    }
                }
                else
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id=@id",
                        DBClass.CreateParameter("id", itemId)))
                    {
                        if (reader.Read())
                        {
                            decimal onHand = Convert.ToDecimal(reader["on_hand"]);
                            DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @newQty WHERE id = @id",
                                DBClass.CreateParameter("newQty", onHand - qty),
                                DBClass.CreateParameter("id", itemId));

                            decimal costReturned = InsertItemTransaction(dgvItems.Rows[i], itemId, qty, method);
                            costAmount = costReturned;
                            totalCostAllItems += costReturned;
                        }
                    }
                }
                //add cost center
                if (dgvItems.Rows[i].Cells["cost_center"].Value != null && int.Parse(dgvItems.Rows[i].Cells["cost_center"].Value.ToString()) > 0)
                    CommonInsert.InsertCostCenterTransaction(dtInv.Value, "0", Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value ?? 0).ToString(), invId.ToString(), "Sales", "", (dgvItems.Rows[i].Cells["cost_center"].Value ?? 0).ToString());

                //if (Utilities.InventoryAssetFromProduct())
                //{
                //    decimal amt = 0;
                //    decimal amount = costAmount;
                //    object netResult = DBClass.ExecuteScalar("SELECT account_id FROM tbl_warehouse WHERE id = @id", DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
                //    int level4Id = netResult != DBNull.Value ? Convert.ToInt32(netResult) : 0;

                //    if (level4Id > 0)
                //    {
                //        var existing = costList.FirstOrDefault(x => x.Level4Id == level4Id);
                //        if (existing != null)
                //        {
                //            existing.Amount += amount;
                //        }
                //        else
                //        {
                //            costList.Add(new CostItem
                //            {
                //                Level4Id = level4Id,
                //                Amount = amount
                //            });
                //        }
                //    }
                //}
            }
            //if (totalCostAllItems > 0)
            //{
            //    itemInventoryCost = totalCostAllItems;
                //    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                //        level4COGS.ToString(), totalCostAllItems.ToString(), "0",
                //        invId.ToString(), "0", "Sales Invoice", "SALES",
                //        "COGS For Sales No. " + invCode,
                //        frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);

                //    if (Utilities.InventoryAssetFromProduct())
                //    {
                //        foreach (var costOfItem in costList)
                //        {
                //            string level4AccountId = costOfItem.Level4Id.ToString();
                //            string itemAmount = costOfItem.Amount.ToString("N2");
                //            CommonInsert.addTransactionEntry(dtInv.Value.Date,
                //                level4AccountId.ToString(), "0", itemAmount.ToString(),
                //                invId.ToString(), "0", "Sales Invoice", "SALES",
                //                "Item Sold For Sales No. " + invCode,
                //                frmLogin.userId, DateTime.Now.Date, txtNextCode.Text
                //            );
                //        }
                //    }
                //    else
                //    {
                //        CommonInsert.addTransactionEntry(dtInv.Value.Date,
                //                level4Inventory.ToString(), "0", totalCostAllItems.ToString(),
                //                invId.ToString(), "0", "Sales Invoice", "SALES",
                //                "Item Sold For Sales No. " + invCode,
                //                frmLogin.userId, DateTime.Now.Date, txtNextCode.Text
                //            );
                //    }
            //}
        }
        private decimal InsertItemTransaction(DataGridViewRow row, int itemId, decimal qty, string method)
        {
            if (AllowItemWithOutQty)
            {
                // Allow selling item even without available stock (negative stock allowed)
                decimal cost_price = 0;
                decimal totalCost = 0;

                // Try to get a recent cost price (optional, to assign value for accounting)
                object result = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
                                            WHERE item_id = @id AND date <= @date 
                                            ORDER BY date DESC LIMIT 1",
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));

                if (result != null && result != DBNull.Value)
                {
                    cost_price = Convert.ToDecimal(result);
                }

                totalCost = cost_price * qty;

                CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Invoice", invId.ToString(), itemId.ToString(),
                    cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), qty.ToString(), "0",
                    "Sales Invoice No. " + txtNextCode.Text + " (Negative Stock)", cmbWarehouse.SelectedValue.ToString());

                return totalCost;
            }
            else
            {
                if (itemId <= 0 || qty <= 0)
                {
                    MessageBox.Show("Invalid Item ID or Quantity.");
                    return 0;
                }

                decimal cost_price = 0;
                decimal totalCost = 0;
                MySqlDataReader reader = null;

                if (method == "fifo" || method == "lifo")
                {
                    string orderBy = method == "fifo" ? "ASC" : "DESC";
                    decimal remainingQty = qty;

                    reader = DBClass.ExecuteReader(@"
                                SELECT * FROM tbl_item_transaction 
                                WHERE date <= @date AND qty_inc > 0 AND item_id = @id 
                                ORDER BY id " + orderBy,
                        DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));

                    while (reader.Read() && remainingQty > 0)
                    {
                        decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
                        cost_price = Convert.ToDecimal(reader["cost_price"]);
                        decimal qtyToUse = Math.Min(remainingQty, availableQty);

                        remainingQty -= qtyToUse;
                        totalCost += cost_price * qtyToUse;

                        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Invoice", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), qtyToUse.ToString(), "0",
                            "Sales Invoice No. " + txtNextCode.Text, cmbWarehouse.SelectedValue.ToString());

                        DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
                            DBClass.CreateParameter("qty", qtyToUse),
                            DBClass.CreateParameter("id", reader["id"].ToString()));
                    }

                    // If some quantity remains unprocessed (not enough stock), allow negative stock handling
                    if (remainingQty > 0)
                    {
                        // Use last known or default cost
                        if (cost_price <= 0)
                        {
                            object fallbackCost = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
                                                              WHERE item_id = @id AND date <= @date 
                                                              ORDER BY date DESC LIMIT 1",
                                        DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));
                            if (fallbackCost != null && fallbackCost != DBNull.Value)
                                cost_price = Convert.ToDecimal(fallbackCost);
                        }

                        totalCost += cost_price * remainingQty;

                        CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Invoice", invId.ToString(), itemId.ToString(),
                            cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), remainingQty.ToString(), "0",
                            "Sales Invoice No. " + txtNextCode.Text + " (Neg. Stock)", cmbWarehouse.SelectedValue.ToString());
                    }
                }
                else
                {
                    object result = DBClass.ExecuteScalar(@"SELECT 
                            CASE 
                                WHEN SUM(qty_in - qty_out) = 0 THEN 0
                                ELSE SUM((qty_in - qty_out) * cost_price) / SUM(qty_in - qty_out)
                            END AS cost_price 
                        FROM 
                            tbl_item_transaction 
                        WHERE item_id = @id AND date <= @date",
                                DBClass.CreateParameter("id", itemId), DBClass.CreateParameter("date", dtInv.Value));

                    decimal recordcost_price = (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

                    if (recordcost_price > 0)
                    {
                        cost_price = recordcost_price;
                    }

                    CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Sales Invoice", invId.ToString(), itemId.ToString(),
                        cost_price.ToString(), "0", row.Cells["price"].Value.ToString(), qty.ToString(), "0",
                        "Sales Invoice No. " + txtNextCode.Text, cmbWarehouse.SelectedValue.ToString());

                    using (MySqlDataReader dr = DBClass.ExecuteReader(@"SELECT (balance / qty_balance) cost FROM tbl_item_card_details 
                WHERE DATE <= @date AND trans_type = 'Purchase Invoice' AND itemId = @itemId
                ORDER BY trans_no DESC LIMIT 1",
                            DBClass.CreateParameter("date", dtInv.Value.Date),
                            DBClass.CreateParameter("itemId", itemId.ToString())))
                    {
                        if (dr.Read())
                        {
                            totalCost = decimal.Parse(dr["cost"].ToString()) * qty;
                        }
                    }
                }

                return totalCost;
            }
        }
        // FIFO/LIFO/AVG logic for warehouse transfer:
        //private void InsertWarehouseTransferTransaction(int itemId, decimal qty, string method, bool isOut)
        //{
        //    string transactionType = "Warehouse Transfer";
        //    int warehouseId = isOut ? Convert.ToInt32(cmbWareFrom.SelectedValue) : Convert.ToInt32(cmbWareTo.SelectedValue);
        //    string description = isOut
        //        ? $"Transferred to {cmbWareTo.Text}"
        //        : $"Received from {cmbWareFrom.Text}";

        //    decimal totalCost = 0;
        //    decimal cost_price = 0;

        //    if (method == "fifo" || method == "lifo")
        //    {
        //        string orderBy = method == "fifo" ? "ASC" : "DESC";
        //        decimal remainingQty = qty;

        //        var reader = DBClass.ExecuteReader(@"SELECT id, qty_inc, cost_price FROM tbl_item_transaction 
        //    WHERE date <= @date AND qty_inc > 0 AND item_id = @id AND warehouse_id = @warehouse_id 
        //    ORDER BY id " + orderBy,
        //            DBClass.CreateParameter("id", itemId),
        //            DBClass.CreateParameter("date", DateTime.Now),
        //            DBClass.CreateParameter("warehouse_id", warehouseId));

        //        while (reader.Read() && remainingQty > 0)
        //        {
        //            decimal availableQty = Convert.ToDecimal(reader["qty_inc"]);
        //            cost_price = Convert.ToDecimal(reader["cost_price"]);
        //            decimal qtyToUse = Math.Min(remainingQty, availableQty);
        //            remainingQty -= qtyToUse;

        //            CommonInsert.InsertItemTransaction(DateTime.Now, transactionType, "0", itemId.ToString(),
        //                cost_price.ToString(),
        //                isOut ? "0" : qtyToUse.ToString(),
        //                "0",
        //                isOut ? qtyToUse.ToString() : "0",
        //                "0",
        //                description,
        //                warehouseId.ToString());

        //            // Update qty_inc only for OUT
        //            if (isOut)
        //            {
        //                DBClass.ExecuteNonQuery("UPDATE tbl_item_transaction SET qty_inc = qty_inc - @qty WHERE id = @id",
        //                    DBClass.CreateParameter("qty", qtyToUse),
        //                    DBClass.CreateParameter("id", reader["id"].ToString()));
        //            }
        //        }

        //        // If not enough stock, add negative stock entry
        //        if (remainingQty > 0)
        //        {
        //            // Fallback to last cost
        //            if (cost_price <= 0)
        //            {
        //                object fallbackCost = DBClass.ExecuteScalar(@"SELECT cost_price FROM tbl_item_transaction 
        //                WHERE item_id = @id AND date <= @date ORDER BY date DESC LIMIT 1",
        //                        DBClass.CreateParameter("id", itemId),
        //                        DBClass.CreateParameter("date", DateTime.Now));
        //                if (fallbackCost != null && fallbackCost != DBNull.Value)
        //                    cost_price = Convert.ToDecimal(fallbackCost);
        //            }

        //            CommonInsert.InsertItemTransaction(DateTime.Now, transactionType, "0", itemId.ToString(),
        //                cost_price.ToString(),
        //                isOut ? "0" : remainingQty.ToString(),
        //                "0",
        //                isOut ? remainingQty.ToString() : "0",
        //                "0",
        //                description + " (Neg. Stock)",
        //                warehouseId.ToString());
        //        }
        //    }
        //    else if (method == "avg")
        //    {
        //        object result = DBClass.ExecuteScalar(@"SELECT 
        //    CASE 
        //        WHEN SUM(qty_in - qty_out) = 0 THEN 0
        //        ELSE SUM((qty_in - qty_out) * cost_price) / SUM(qty_in - qty_out)
        //    END AS cost_price 
        //FROM tbl_item_transaction 
        //WHERE item_id = @id AND date <= @date AND warehouse_id = @warehouse_id",
        //            DBClass.CreateParameter("id", itemId),
        //            DBClass.CreateParameter("date", DateTime.Now),
        //            DBClass.CreateParameter("warehouse_id", warehouseId));

        //        cost_price = (result != null && result != DBNull.Value) ? Convert.ToDecimal(result) : 0;

        //        CommonInsert.InsertItemTransaction(DateTime.Now, transactionType, "0", itemId.ToString(),
        //            cost_price.ToString(),
        //            isOut ? "0" : qty.ToString(),
        //            "0",
        //            isOut ? qty.ToString() : "0",
        //            "0",
        //            description,
        //            warehouseId.ToString());
        //    }
        //}

        private bool ChkRequiredData()
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
            if (cmbPaymentMethod.SelectedItem.ToString() == "")
            {
                MessageBox.Show("Payment Method Must be Selected.");
                return false;
            }
            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Warehouse Must be Selected.");
                return false;
            }
            if (cmbAccountCashName.SelectedValue == null)
            {
                MessageBox.Show("Cash Account Must be Selected.");
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
            if (level4PaymentCreditMethodId <= 0 && level4VatId <= 0 && level4SalesInvoice <= 0 && level4COGS <= 0 && level4Inventory <= 0)
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void ResetTextBox()
        {
            txtSalesMan.Text = txtBillTo.Text = txtShipTo.Text = txtPONO.Text =
                txtTotal.Text = txtTotalVat.Text = "";
            cmbPaymentMethod.SelectedIndex = id = 0;
            dtInv.Value = DateTime.Now;
            dgvItems.Rows.Clear();
            txtId.Text = GenerateNextSalesId().ToString();
            txtNextCode.Text = GenerateNextSalesCode();
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (InsertInvoice())
                    ResetTextBox();
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
                cmbAccountCashName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Invoice Payment Cash Method");
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
                     dgvItems.CurrentCell.OwningColumn.Name == "price" ||
                     dgvItems.CurrentCell.OwningColumn.Name == "discount")
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
                decimal discount = GetDecimalValue(row, "discount");

                if (price == 0 || qty == 0)
                {
                    row.Cells["vatp"].Value = row.Cells["discount"].Value = row.Cells["total"].Value = "0";
                }
                else
                {
                    decimal net = (price * qty) - discount;
                    row.Cells["net_price"].Value = net;
                    row.Cells["vat"].Value = Convert.ToInt32(taxId);
                    row.Cells["vatp"].Value = (net * decimal.Parse(taxP) / 100);
                    row.Cells["total"].Value = net + Convert.ToDecimal(row.Cells["vatp"].Value);
                }

                ChkRowValidity(row);
            }
        }
        private void ChkRowValidity(DataGridViewRow row)
        {
            if (row == null) return;

            decimal price = GetDecimalValue(row, "price");
            decimal qty = GetDecimalValue(row, "qty");
            decimal discount = GetDecimalValue(row, "discount");

            if (price == 0 || qty == 0)
            {
                row.Cells["vatp"].Value = "0";
                row.Cells["total"].Value = "0";
                return;
            }

            try
            {
                decimal netPrice = (qty * price) - discount;
                row.Cells["net_price"].Value = netPrice;

                // --- Get VAT value from DataSource by selected VAT ID ---
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

        bool CheckItemValidity(int itemId, decimal salesQty)
        {
            if (AllowItemWithOutQty)
                return true;

            using (MySqlDataReader reader = DBClass.ExecuteReader(
         //       "SELECT(SELECT type FROM tbl_items WHERE id = t.item_id)type, sum(t.qty_in - t.qty_out)on_hand  " +
           //     "FROM tbl_item_transaction  t WHERE t.item_id = @id AND t.reference != @refId AND t.type != 'Sales Invoice'",
             @" SELECT
    IFNULL(
        (SELECT SUM(t.qty_in - t.qty_out)
         FROM tbl_item_transaction t
         WHERE t.item_id = i.id
           AND t.reference != @refId
           AND t.type != 'Sales Invoice'),
        i.on_hand
    ) AS on_hand,
    i.type
FROM tbl_items i
WHERE i.id = @id;",

            DBClass.CreateParameter("id", itemId),
                DBClass.CreateParameter("refId", id)))
            {
                if (!reader.Read())
                    return false;

                string itemType = reader["type"].ToString(); //qty 9,1 ed 1-5 

                if (itemType == "12 - Service")
                    return true;

                decimal onHand = Convert.ToDecimal(reader["on_hand"]);
                decimal totalQtyInGrid = GetTotalQtyInGrid(itemId);

                if (itemType == "11 - Inventory Part")
                {
                    if (totalQtyInGrid > onHand)
                    {
                        MessageBox.Show($"Item Out Of Stock. Only {onHand.ToString("0.00")} available, but requested {totalQtyInGrid.ToString("0.00")}");
                        return false;
                    }
                }
                else if (itemType == "13 - Inventory Assembly")
                {
                    using (MySqlDataReader assemblyReader = DBClass.ExecuteReader(@"
                        SELECT i.name, a.qty, i.on_hand 
                        FROM tbl_item_assembly a
                        JOIN tbl_items i ON a.item_id = i.id
                        WHERE a.assembly_id = @assemblyId",
                        DBClass.CreateParameter("@assemblyId", itemId)))
                    {
                        string msg = "";
                        while (assemblyReader.Read())
                        {
                            string name = assemblyReader["name"].ToString();
                            decimal required = Convert.ToDecimal(assemblyReader["qty"]) * totalQtyInGrid;
                            decimal available = Convert.ToDecimal(assemblyReader["on_hand"]);

                            if (required > available)
                                msg += $"Component Out Of Stock: {name}. Needs {required}, but only {available} available.\n";
                        }

                        if (!string.IsNullOrEmpty(msg))
                        {
                            MessageBox.Show(msg);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        decimal GetTotalQtyInGrid(int itemId)
        {
            decimal totalQty = 0;

            for (int i = 0; i < dgvItems.Rows.Count; i++)
                if (dgvItems.Rows[i].Cells["itemId"].Value != null &&
                    dgvItems.Rows[i].Cells["itemId"].Value.ToString() == itemId.ToString())
                    if (dgvItems.Rows[i].Cells["qty"].Value != null &&
                        dgvItems.Rows[i].Cells["qty"].Value.ToString() != "")
                        totalQty += decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());

            return totalQty;
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
            if (suppressCellValueChanged) return;

            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];
                decimal price = GetDecimalValue(row, "price");
                decimal qty = GetDecimalValue(row, "qty");
                decimal costPrice = GetDecimalValue(row, "cost_price");
                decimal discount = GetDecimalValue(row, "discount");
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
                    decimal salesQty = 0;
                    if (id != 0)
                    {
                        if (dgvItems.CurrentRow.Cells["itemid"].Value != null)
                        {
                            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_sales_details where sales_id = @sales_id and item_id = @item_id",
                              DBClass.CreateParameter("sales_id", id),
                              DBClass.CreateParameter("item_id", dgvItems.CurrentRow.Cells["itemid"].Value.ToString())))
                                if (reader.Read() && reader["qty"].ToString() != "")
                                    salesQty = decimal.Parse(reader["qty"].ToString());
                        }
                    }
                    if (dgvItems.CurrentRow.Cells["itemId"].Value == null || !CheckItemValidity(int.Parse(dgvItems.CurrentRow.Cells["itemId"].Value.ToString()), salesQty))
                        row.Cells["qty"].Value = 0;
                }
                ChkRowValidity(row);
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
                    dgvItems.CurrentRow.Cells["discount"].Value = 0;
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
                        row.Cells["discount"].Value = null;
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

            string query = "select id from tbl_sales where state = 0 and id =@id";

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
            string query = "SELECT id FROM tbl_sales WHERE state = 0 AND id =@id";

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clearData();
                    MessageBox.Show("No More Records");
                }
        }

        private void lnkNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewCustomer(0));
        }

        private void FormatNumberWithCommas(Guna2TextBox txtBox)
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
            try
            {
                if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
                {
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
                    CalculateTotal();
                }
            }catch(Exception ex)
            {
                ex.ToString();
            }
        }
        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }
        private void cmbPaymentTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.Text == "Credit")
                dtPaymentTerms.Value = dtPaymentTerms.Value.AddDays(int.Parse(cmbPaymentTerms.Text));
            else
                dtPaymentTerms.Value = dtInv.Value;
        }

        private void loadSalesOrderData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_sales_order where id = @id",
                DBClass.CreateParameter("id", txtPONO.Text)))
                if (reader.Read())
                {
                    txtPONO.Text = reader["id"].ToString();
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
                    richTextDescription.Text = reader["description"].ToString();
                    cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                    txtNextCode.Text = reader["invoice_id"].ToString();
                    invId = id;
                    txtBillTo.Text = reader["bill_to"].ToString();
                    txtShipTo.Text = reader["ship_to"].ToString();

                    dgvItems.Rows.Clear();
                    using (MySqlDataReader readerDetails = DBClass.ExecuteReader(@"SELECT tbl_sales_order_details.*,tbl_items.type,method, tbl_items.code as code FROM tbl_sales_order_details INNER JOIN 
                                                                    tbl_items ON tbl_sales_order_details.item_id = tbl_items.id WHERE 
                                                                    tbl_sales_order_details.sales_id = @id;",
                                                                    DBClass.CreateParameter("id", txtPONO.Text)))
                        while (readerDetails.Read())
                        {
                            dgvItems.Rows.Add(readerDetails["item_id"].ToString(), "", readerDetails["code"].ToString(), readerDetails["code"].ToString(), readerDetails["qty"].ToString(),
                                readerDetails["cost_price"].ToString(), readerDetails["price"].ToString(), "0");
                            DataGridViewComboBoxCell comboCell = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vat"] as DataGridViewComboBoxCell;
                            if (comboCell != null && readerDetails["vat"].ToString() != "0")
                            {
                                comboCell.Value = int.Parse(readerDetails["vat"].ToString());
                                dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) *
                                decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["price"].Value.ToString()) * decimal.Parse(readerDetails["vat"].ToString()) / 100);
                            }
                            //DataGridViewComboBoxCell comboCellCostCenter = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_center"] as DataGridViewComboBoxCell;
                            //if (comboCellCostCenter != null && readerDetails["cost_center_id"].ToString() != "0")
                            //{
                            //    comboCellCostCenter.Value = int.Parse(readerDetails["cost_center_id"].ToString());
                            //}
                            decimal netPrice = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) * decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["price"].Value.ToString()));
                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["total"].Value = (netPrice + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString()));
                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = readerDetails["method"].ToString();
                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = readerDetails["type"].ToString();
                        }

                    CalculateTotal();
                }
        }
        private void LoadSalesQuotationData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_sales_quotation where id = @id",
                DBClass.CreateParameter("id", txtPONO.Text)))
                if (reader.Read())
                {
                    txtPONO.Text = reader["id"].ToString();
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
                    richTextDescription.Text = reader["description"].ToString();
                    cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                    txtCustomerCode.Text = reader["invoice_id"].ToString();
                    invId = id;
                    txtBillTo.Text = reader["bill_to"].ToString();
                    txtShipTo.Text = reader["ship_to"].ToString();

                    dgvItems.Rows.Clear();
                    MySqlDataReader readerDetails = DBClass.ExecuteReader(@"SELECT tbl_sales_quotation_details.*,tbl_items.type,method, tbl_items.code , tbl_items name FROM tbl_sales_quotation_details INNER JOIN 
                                                                    tbl_items ON tbl_sales_quotation_details.item_id = tbl_items.id WHERE 
                                                                    tbl_sales_quotation_details.sales_id = @id;",
                                                                    DBClass.CreateParameter("id", txtPONO.Text));
                    while (readerDetails.Read())
                    {
                        dgvItems.Rows.Add(readerDetails["item_id"].ToString(), "", readerDetails["code"].ToString(), readerDetails["name"].ToString(), readerDetails["qty"].ToString(),
                            readerDetails["cost_price"].ToString(), readerDetails["price"].ToString(), "0");
                        DataGridViewComboBoxCell comboCell = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vat"] as DataGridViewComboBoxCell;
                        if (comboCell != null && readerDetails["vat"].ToString() != "0")
                        {
                            comboCell.Value = int.Parse(readerDetails["vat"].ToString());
                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) *
                            decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["price"].Value.ToString()) * decimal.Parse(readerDetails["vat"].ToString()) / 100);
                        }
                        //DataGridViewComboBoxCell comboCellCostCenter = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_center"] as DataGridViewComboBoxCell;
                        //if (comboCellCostCenter != null && readerDetails["cost_center_id"].ToString() != "0")
                        //{
                        //    comboCellCostCenter.Value = int.Parse(readerDetails["cost_center_id"].ToString());
                        //}
                        decimal netPrice = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) * decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["price"].Value.ToString()));
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["total"].Value = (netPrice + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString()));
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = readerDetails["method"].ToString();
                        dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = readerDetails["type"].ToString();
                    }

                    CalculateTotal();
                }
        }

        private void guna2TileButton25_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal(id.ToString(), "SALES"));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (guna2Panel1.Height == 44)

            {
                timer1.Stop();

            }
            else
            {
               guna2Panel1.Height = guna2Panel1.Height + 11;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (guna2Panel1.Height == 0)
            {
                timer2.Stop();  
            }
            else
            {
                guna2Panel1.Height = guna2Panel1.Height - 11;
            }
                
        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            if (guna2Panel1.Height >= 44)
                {
                timer2.Start();
                timer1.Stop();
            }
            else
            {
                timer2.Stop();
                timer1.Start();
            }

        }

        private void guna2TileButton30_Click(object sender, EventArgs e)
        {
            //frmLogin.frmMain.openChildForm(new MasterReportView());
            ShowReport();
        }

        private void guna2TileButton31_Click(object sender, EventArgs e)
        {
            //frmLogin.frmMain.openChildForm(new MasterReportView());
            ShowReportDelivery();
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            {
                MessageBox.Show("Can't Save Empty Tax Invoice");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }
            if (!Utilities.AreDefaultAccountsSet(new List<string> { "Sales", "COGS", "Customer", "Vat Output", "Inventory" }))
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CheckQtyAvailable();
            if (isSaved)
            {
                MessageBox.Show("The Tax Invoice  Saved");
                dgvItems.Rows.Clear();

                if (chkPrint.Checked == true)
                {
                    ShowReport();
                }

                //if (chkPrint.Checked == true)
                //{
                //    ShowReport();
                //}
                //this.Close();
            }
            else
            {
                MessageBox.Show("Please check stock before save! or Correct the Entry");
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

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {
            clearData();
        }
        private void clearData()
        {
            dgvItems.Rows.Clear();
            txtBillTo.Text = "";
            id = 0;
            txtNextCode.Text = GenerateNextSalesCode();
            txtId.Text = GenerateNextSalesId();
        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {
            DBClass.ExecuteNonQuery("UPDATE tbl_sales SET state = -1 WHERE id = @id; UPDATE tbl_transaction SET state= -1 WHERE transaction_id = @id AND t_type = 'SALES';",
                                          DBClass.CreateParameter("id", id.ToString()));
            CommonInsert.DeleteItemTransaction("Sales", id.ToString());
            Utilities.LogAudit(frmLogin.userId,"Sales", "Delete Sales Invoice", id, "Deleted Sales Invoice with ID: " + id.ToString());
            clearData();
        }

        private void guna2TileButton20_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        private void guna2TileButton24_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmSalesByCustomerSummary());
        }

        private void guna2TileButton26_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal(id.ToString(), "SALES"));
        }

        private void guna2TileButton28_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmSalesByCustomerDetails(int.Parse(cmbCustomer.SelectedValue.ToString()),"tbl_sales"));
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

        private void dgvItems_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            if (!string.IsNullOrEmpty(defaultTax))
            {
                DataTable dt = ((DataGridViewComboBoxColumn)dgvItems.Columns["vat"]).DataSource as DataTable;
                if (dt != null)
                {
                    DataRow[] rows = dt.Select("value = " + defaultTax);
                    if (rows.Length > 0)
                    {
                        // Always assign the correct ID
                        e.Row.Cells["vat"].Value = rows[0]["id"];
                    }
                }
            }
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
                    newRow.Cells["discount"].Value = null;
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


        public DataTable COMPANYINFO(int id)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company limit 1");
        }

        private void dtInv_ValueChanged(object sender, EventArgs e)
        {
            dtPaymentTerms.Value = dtShip.Value = dtInv.Value;

            if (cmbPaymentMethod.Text == "Credit")
                dtPaymentTerms.Value = dtPaymentTerms.Value.AddDays(int.Parse(cmbPaymentTerms.Text));
            else
                dtPaymentTerms.Value = dtInv.Value;
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        public DataTable SalesDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT s.id,s.date,s.customer_id,s.invoice_id,s.bill_to,s.city,s.sales_man,s.ship_date,s.ship_via,s.ship_to,(SELECT NAME FROM tbl_coa_level_4 WHERE id=s.account_cash_id) accountName,s.po_num,s.payment_method,s.payment_terms,s.payment_date,s.total,s.vat,s.net,s.pay,s.change 
                                    ,c.name customerName,c.main_phone,c.email,c.trn FROM tbl_sales s,tbl_customer c WHERE s.customer_id = c.id AND s.id=@salesId;", DBClass.CreateParameter("@salesId", a1));
        }

        public DataTable ItemDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT d.id,d.item_id,i.name,d.qty,d.cost_price,d.price,(d.qty*d.cost_price) subCostTotal,(d.qty*d.price) subPriceTotal,d.discount,((d.qty*d.price)-d.discount) subTotal,d.vatp vatAmount,d.vat vatPercentage,d.total,d.cost_center_id,(SELECT NAME FROM tbl_sub_cost_center WHERE id=cost_center_id) costCenterName,i.type,i.method,i.unit_id,(select name from tbl_unit WHERE id=i.unit_id) unitName, i.code as code FROM tbl_sales_details d 
                                       INNER JOIN tbl_items i ON d.item_id = i.id WHERE d.sales_id=@salesId; ", DBClass.CreateParameter("@salesId", a1));
        }

        public void ShowReport()
        {
            ShowReport("SalesInvoice.rpt", "CompanyHeader", "InfoHeader", "ItemHeader");
        }

        public void ShowReportDelivery()
        {
            ShowReport("DeliveryNote.rpt", "CompanyHeader", "SalesHeader", "ItemsHeader");
        }

        private void ShowReport(string reportFileName, string companySubReportName, string salesSubReportName, string itemsSubReportName)
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", reportFileName);
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);

                DataTable companyData = COMPANYINFO(1);
                DataTable salesData = SalesDetails(invId.ToString());
                DataTable itemData = ItemDetails(invId.ToString());

                if (companyData == null || companyData.Rows.Count == 0)
                {
                    MessageBox.Show("No company data available for the report.");
                    return;
                }

                if (salesData == null || salesData.Rows.Count == 0)
                {
                    MessageBox.Show("No sales data available for the report.");
                    return;
                }

                if (itemData == null || itemData.Rows.Count == 0)
                {
                    MessageBox.Show("No item data available for the report.");
                    return;
                }

                cr.Subreports[companySubReportName].SetDataSource(companyData);
                cr.Subreports[salesSubReportName].SetDataSource(salesData);
                cr.Subreports[itemsSubReportName].SetDataSource(itemData);

                using (var reportForm = new MasterReportView())
                {
                    reportForm.crReportViewer.ReportSource = cr;
                    reportForm.crReportViewer.RefreshReport();
                    reportForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing report: " + ex.Message);
            }
        }

        public void ShowReportOld()
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "SalesInvoice.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                DataTable companyData = COMPANYINFO(1);
                DataTable salesData = SalesDetails(invId.ToString());
                DataTable itemData = ItemDetails(invId.ToString());
                if (companyData != null)
                {
                    cr.Subreports["CompanyHeader"].SetDataSource(companyData);
                    cr.Subreports["InfoHeader"].SetDataSource(salesData);
                    cr.Subreports["ItemHeader"].SetDataSource(itemData);
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

        public void ShowReportDeliveryOld()
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "DeliveryNote.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                DataTable companyData = COMPANYINFO(1);
                DataTable salesData = SalesDetails(invId.ToString());
                DataTable itemData = ItemDetails(invId.ToString());
                if (companyData != null)
                {
                    cr.Subreports["CompanyHeader"].SetDataSource(companyData);
                    cr.Subreports["SalesHeader"].SetDataSource(salesData);
                    cr.Subreports["ItemsHeader"].SetDataSource(itemData);
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
    }
}
