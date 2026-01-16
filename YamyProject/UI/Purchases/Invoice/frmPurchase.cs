using CrystalDecisions.CrystalReports.Engine;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Reports.Design;
using YamyProject.UI.Settings;

namespace YamyProject
{

    public partial class frmPurchase : Form
    {
        private EventHandler vendorUpdatedHandler;
        private EventHandler itemUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;

        decimal invId;
        int level4PaymentCreditMethodId, level4VatId, level4PurchaseInvoice, level4Inventory;
        int id, purchaseOrderId = 0, defaultVendorId = 0;
        string XinvCode, PO;
        bool EnableBarcode = false;
        string typed = "";

        public frmPurchase(int _id = 0, string _PO = "", int _vendorId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            vendorUpdatedHandler = (sender, args) => BindCombos.PopulateVendors(cmbVendor);
            itemUpdatedHandler = (sender, args) => RefreshItems();
            warehouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            EventHub.Item += itemUpdatedHandler;
            EventHub.wareHouse += warehouseUpdatedHandler;
            EventHub.Vendor += vendorUpdatedHandler;
            this.PO = _PO;
            if (PO == "PO")
            {
                txtPONO.Text = _id.ToString();
            }
            else
            {
                this.id = _id;
                txtPONO.Text = "";
            }
            this.defaultVendorId = _vendorId;
            headerUC1.FormText = this.Text;
        }

        private void frmPurchase_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Vendor -= vendorUpdatedHandler;
            EventHub.Item -= itemUpdatedHandler;
            EventHub.wareHouse -= warehouseUpdatedHandler;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPurchase_Load(object sender, EventArgs e)
        {
            guna2Panel1.Height = 0;
            dtPaymentTerms.Value = dtInv.Value = dtShip.Value = DateTime.Now.Date;
            BindCombos.PopulateWarehouse(cmbWarehouse);
            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Enter At Least One Warehouse");
                new frmViewWarehouse().ShowDialog();
                BindCombos.PopulateWarehouse(cmbWarehouse, false, true);
            }
            loadVat();
            loadCostCenter();
            cmbPaymentMethod.SelectedIndex = 0;
            bindCombo();
            txtNextCode.Text = GenerateNextSalesCode();
            txtInvoiceId.Text = GetNextPurchaseId();
            if (id != 0)
            {
                btnSave.Enabled = btnSaveClose.Enabled = UserPermissions.canEdit("Purchases Center");
                BindInvoice();
            }
            else
            {
                if (defaultVendorId > 0)
                {
                    cmbVendor.SelectedValue = defaultVendorId;
                }
            }
            purchaseOrderId = PO == "" ? purchaseOrderId = 0 : int.Parse(txtPONO.Text.ToString());
            if (purchaseOrderId != 0)
            {
                loadPurchaseOrderData();
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

            ChkRowValidity(newRow);
            CalculateTotal();

            suppressCellValueChanged = false;
        }

        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_purchase where id = @id",
                DBClass.CreateParameter("id", id)))
            {
                if (reader.Read())
                {
                    dtInv.Value = DateTime.Parse(reader["date"].ToString());
                    txtNextCode.Text = reader["invoice_id"].ToString();
                    txtInvoiceId.Text = reader["id"].ToString();
                    var Vendor_Id = reader["vendor_id"].ToString();
                    //CbSubcontractors.Checked = Utilities.CheckVendorType(Vendor_Id);
                    cmbVendor.SelectedValue = Vendor_Id;
                    using (var rd = DBClass.ExecuteReader("select type,name from tbl_vendor where id=@id", DBClass.CreateParameter("id", Vendor_Id)))
                    {
                        if (rd.Read())
                        {
                            var t = rd["type"];
                            if (!string.IsNullOrWhiteSpace(t.ToString()))
                                CbSubcontractors.Checked = t.ToString() == "Subcontractor";

                            cmbVendor.Text = cmbVendor.SelectedText = rd["name"].ToString();
                        }
                    }

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
                    cmbPurchasetype.Text = reader["purchase_type"].ToString();
                    txtNextCode.Text = reader["invoice_id"].ToString();
                    txtInvoiceId.Text = reader["id"].ToString();
                    richTextDescription.Text = reader["description"].ToString();
                    invId = id;
                    txtBillTo.Text = reader["bill_to"].ToString();
                    txtShipTo.Text = reader["ship_to"].ToString();
                    txtSalesMan.Text = reader["sales_man"].ToString();
                    if (cmbPurchasetype.Text.ToString().Trim() == "Fixed Assets")
                    {
                        labelFixedAssetCategory.Visible = true;
                        cmbFixedAssetCategory.Visible = true;
                        cmbFixedAssetCategory.SelectedValue = int.Parse(reader["fixed_asset_category_id"].ToString());
                    }
                }
                BindInvoiceItems();
                CalculateTotal();
            }
        }

        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_purchase_details.*,tbl_items.type,method, tbl_items.code, tbl_items.name FROM tbl_purchase_details INNER JOIN 
                                                                    tbl_items ON tbl_purchase_details.item_id = tbl_items.id WHERE 
                                                                    tbl_purchase_details.purchase_id = @id;",
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
                    //net_price
                    decimal netPrice = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) * decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_price"].Value.ToString())
                        - decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["discount"].Value.ToString()));
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["net_price"].Value = netPrice;
                    dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["total"].Value = netPrice
                      + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString());

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

                string purchaseType = "";
                if (cmbPurchasetype.InvokeRequired)
                {
                    cmbPurchasetype.Invoke(new Action(() =>
                    {
                        purchaseType = cmbPurchasetype.Text.Trim();
                    }));
                }
                else
                {
                    purchaseType = cmbPurchasetype.Text.Trim();
                }

                if (!string.IsNullOrEmpty(purchaseType))
                {
                    query += " AND item_type = @itemType";
                    parameters.Add(new MySqlParameter("@itemType", purchaseType));
                }
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

                string purchaseType = "";
                if (cmbPurchasetype.InvokeRequired)
                {
                    cmbPurchasetype.Invoke(new Action(() =>
                    {
                        purchaseType = cmbPurchasetype.Text.Trim();
                    }));
                }
                else
                {
                    purchaseType = cmbPurchasetype.Text.Trim();
                }

                if (!string.IsNullOrEmpty(purchaseType))
                {
                    query += " AND item_type = @itemType";
                    parameters.Add(new MySqlParameter("@itemType", purchaseType));
                }
            }

            if (!string.IsNullOrEmpty(code))
            {
                query += " AND code LIKE @code";
                parameters.Add(new MySqlParameter("@code", $"%{code}%"));
            }
           
            query += " ORDER BY code Limit 20";

            return DBClass.ExecuteDataTable(query, parameters.ToArray());
        }

        private void loadVat()
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
            BindCombos.PopulateVendors(cmbVendor);
            BindCombos.PopulateAllLevel4Account(cmbAccountCashName);
            BindCombos.PopulateFixedAssetsCategories(cmbFixedAssetCategory);
            var defaultAccounts = BindCombos.LoadDefaultAccounts();

            cmbAccountCashName.SelectedValue = defaultAccounts.ContainsKey("Purchase Payment Cash Method")
                ? defaultAccounts["Purchase Payment Cash Method"] : 0;

            level4PaymentCreditMethodId = defaultAccounts.ContainsKey("Vendor") ? defaultAccounts["Vendor"] : 0;
            level4VatId = defaultAccounts.ContainsKey("Vat Output") ? defaultAccounts["Vat Input"] : 0;
            level4Inventory = defaultAccounts.ContainsKey("Inventory") ? defaultAccounts["Inventory"] : 0;
            level4PurchaseInvoice = defaultAccounts.ContainsKey("Purchase") ? defaultAccounts["Purchase"] : 0;
        }
        private string GenerateNextSalesCode()
        {
            string newCode = "PI-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(invoice_id, 4) AS UNSIGNED)) AS lastCode FROM tbl_purchase"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "PI-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count == 0 || dgvItems.Rows.Count == 1 && dgvItems.Rows[0].IsNewRow)
            {
                MessageBox.Show("Can't Save Empty Purchase Invoice");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }

            if (!Utilities.AreDefaultAccountsSet(new List<string> { "Vendor", "Purchase", "Vat Input", "Inventory" }))
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (id == 0)
            {
                if (insertInvoice())
                {
                    int pId = int.Parse(invId.ToString());
                    int cId = cmbFixedAssetCategory.SelectedValue == null ? 0 : int.Parse(cmbFixedAssetCategory.SelectedValue.ToString());
                    if (pId > 0 && cId > 0)
                    {
                        frmViewFixedAssets.UpdateOrAddFixedAssets(pId, cId);
                    }
                    EventHub.RefreshPurchase();
                    if (PO == "PO")
                    {
                        EventHub.RefreshPurchaseOrder();
                    }
                    loadPrint();
                    MessageBox.Show("The Purchase Invoice  Saved");
                    dgvItems.Rows.Clear();

                }
            }
            else
            {
                if (updateInvoice())
                {
                    int pId = int.Parse(id.ToString());
                    int cId = cmbFixedAssetCategory.SelectedValue == null ? 0 : int.Parse(cmbFixedAssetCategory.SelectedValue.ToString());
                    if (pId > 0 && cId > 0)
                    {
                        frmViewFixedAssets.UpdateOrAddFixedAssets(pId, cId);
                    }
                    EventHub.RefreshPurchase();
                    loadPrint();
                    MessageBox.Show("The Purchase Invoice  Updated");
                    dgvItems.Rows.Clear();
                }
            }
        }
        private void loadPrint()
        {
            //DialogResult result = MessageBox.Show("Do You want To Show This Bill ",
            //                                   "Confirmation",
            //                                   MessageBoxButtons.YesNo,
            //                                   MessageBoxIcon.Question);
            //// Check if the user clicked Yes or No
            //if (result == DialogResult.Yes)
            //{
            //    // Code for when the user clicks "Yes"

            if (chkPrint.Checked)
            {
                ShowReport();
            }

            //    if (checkBoxReceiverNote.Checked)
            //    {
            //        frmLogin.frmMain.openChildForm(new frmReport());
            //        ShowReportReceiverNote();
            //    }

            //}
            //else if (result == DialogResult.No)
            //{
            //    // Code for when the user clicks "No"
            //    this.Close();
            //}
        }
        private bool updateInvoice()
        {
            if (!chkRequiredDate())
                return false;

            decimal paidAmount = string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text);
            decimal changeAmount = string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text);
            if (cmbPaymentMethod.Text != "Cash")
            {
                object objOldPayment = DBClass.ExecuteScalar(@"SELECT IFNULL(sum(payment),0) amount FROM tbl_payment_voucher_details WHERE inv_id =@id",
                            DBClass.CreateParameter("@id", id));
                decimal oldPaymentTotal = (objOldPayment != null && objOldPayment != DBNull.Value) ? decimal.Parse(objOldPayment.ToString()) : 0;
                string total = txtTotal.Text;
                if (oldPaymentTotal > 0)
                {
                    paidAmount = oldPaymentTotal;
                }
                changeAmount = decimal.Parse(total);
                paidAmount = 0;
            }
            else
            {
                changeAmount = 0;
            }

            DBClass.ExecuteNonQuery(@"UPDATE tbl_purchase 
                                     SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date,sales_man=@sales_man, vendor_id = @vendor_id, invoice_id = @invoice_id, warehouse_id = @warehouse_id,
                                     po_num = @po_num, bill_to = @bill_to, ship_date = @ship_date, 
                                     ship_via = @ship_via, ship_to = @ship_to, payment_method = @payment_method, account_cash_id = @account_cash_id, 
                                     payment_terms = @payment_terms, payment_date = @payment_date, total = @total, 
                                     vat = @vat, net = @net, pay = @pay, `change` = @change, city = @city,purchase_type =@purchase_type, fixed_asset_category_id=@fixed_asset_category_id, description = @description WHERE id = @id;",
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
                                   DBClass.CreateParameter("vat", TotalVatAmount),
                                   DBClass.CreateParameter("total", TotalBeforeAmount),
                                   DBClass.CreateParameter("net", TotalAmount),
                                    DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                                    DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date),
                                DBClass.CreateParameter("pay", paidAmount),
                                DBClass.CreateParameter("change", changeAmount),
                                DBClass.CreateParameter("purchase_type", cmbPurchasetype.Text),
                                DBClass.CreateParameter("description", richTextDescription.Text),
                                   DBClass.CreateParameter("fixed_asset_category_id", cmbFixedAssetCategory.SelectedValue != null ? cmbFixedAssetCategory.SelectedValue.ToString() : "0"));
            ReturnItemsToInventory();
            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "Purchase");
            CommonInsert.DeleteTransactionEntry(id, "PURCHASE");

            insertInvItems();

            transactions();
            Utilities.LogAudit(frmLogin.userId, "Update Purchase Invoice", "Purchase", id, "Updated Purchase Invoice: " + txtNextCode.Text);

            return true;
        }

        private void ReturnItemsToInventory()
        {
            DBClass.ExecuteNonQuery(@"
                                    DELETE FROM tbl_item_transaction  
                                    WHERE reference = @id AND type = 'Purchase Invoice';
                                    DELETE FROM tbl_item_card_details 
                                    WHERE trans_type = 'Purchase Invoice' AND trans_no = @id;
                                    DELETE FROM tbl_purchase_details 
                                    WHERE purchase_id = @id;",
                                    DBClass.CreateParameter("id", id));
        }

        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;

            

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_purchase (date, vendor_id, invoice_id,warehouse_id, po_num, bill_to,city,sales_man,
               ship_date,ship_via, ship_to, payment_method,account_cash_id, payment_terms, payment_date,  
               total,vat,net, pay, `change`, created_by, created_date, state,purchase_type,fixed_asset_category_id,description) VALUES (@DATE, @vendor_id, @invoice_id,@warehouse_id,
               @po_num, @bill_to,@city,@sales_man, @ship_date, @ship_via,@ship_to, @payment_method,@account_cash_id ,@payment_terms, @payment_date, 
               @total,  @vat,@net, @pay, @change, @created_by, @created_date, @state,@purchase_type,@fixed_asset_category_id,@description);
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
               DBClass.CreateParameter("vat", TotalVatAmount),
               DBClass.CreateParameter("total", TotalBeforeAmount),
               DBClass.CreateParameter("net", TotalAmount),
               DBClass.CreateParameter("pay", cmbPaymentMethod.Text == "Cash" ? string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text) : 0),
               DBClass.CreateParameter("change", cmbPaymentMethod.Text == "Cash" ? 0 : string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text)),
               DBClass.CreateParameter("created_by", frmLogin.userId),
               DBClass.CreateParameter("created_date", DateTime.Now.Date),
               DBClass.CreateParameter("state", 0),
               DBClass.CreateParameter("purchase_type", cmbPurchasetype.Text),
               DBClass.CreateParameter("description", richTextDescription.Text),
               DBClass.CreateParameter("fixed_asset_category_id", cmbFixedAssetCategory.SelectedValue != null ? cmbFixedAssetCategory.SelectedValue.ToString() : "0")).ToString());

            insertInvItems();
            txtNextCode.Text.ToString();
            txtInvoiceId.Text = invId.ToString();
            transactions();
            if (!string.IsNullOrEmpty(txtPONO.Text))
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_purchase_order set tranfer_status=1, purchase_id=@id where id=@poId", DBClass.CreateParameter("poId", txtPONO.Text), DBClass.CreateParameter("id", invId));
            }
            Utilities.LogAudit(frmLogin.userId, "Add Purchase Invoice", "Purchase", (int)invId, "Added Purchase Invoice: " + txtNextCode.Text);

            return true;
        }

        //private void insertInvItemsOld()
        //{
        //    List<CostItem> costList = new List<CostItem>();

        //    for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
        //    {
        //        if (dgvItems.Rows[i].IsNewRow || string.IsNullOrWhiteSpace(dgvItems.Rows[i].Cells["itemId"].Value?.ToString()))
        //            continue;

        //        string itemId = dgvItems.Rows[i].Cells["itemId"].Value.ToString();
        //        decimal qty = dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value);
        //        decimal discount = string.IsNullOrWhiteSpace(dgvItems.Rows[i].Cells["discount"].Value?.ToString())
        //            ? 0
        //            : Convert.ToDecimal(dgvItems.Rows[i].Cells["discount"].Value);

        //        DBClass.ExecuteNonQuery(@"
        //            INSERT INTO tbl_purchase_details (purchase_id, item_id, qty, cost_price, price,discount, vatp, vat, total,cost_center_id)
        //            VALUES (@purchase_id, @item_id, @qty, @cost_price, @price,@discount, @vatp, @vat, @total,@costCenter);",
        //            DBClass.CreateParameter("@purchase_id", invId),
        //            DBClass.CreateParameter("@item_id", itemId),
        //            DBClass.CreateParameter("@qty", qty),
        //            DBClass.CreateParameter("@price", dgvItems.Rows[i].Cells["price"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["price"].Value.ToString()),
        //            DBClass.CreateParameter("@cost_price", dgvItems.Rows[i].Cells["cost_price"].Value ?? 0),
        //            DBClass.CreateParameter("@discount", discount),
        //            DBClass.CreateParameter("@vat", dgvItems.Rows[i].Cells["vat"].Value ?? 0),
        //            DBClass.CreateParameter("@vatp", dgvItems.Rows[i].Cells["vatp"].Value ?? 0),
        //            DBClass.CreateParameter("@total", dgvItems.Rows[i].Cells["total"].Value ?? 0),
        //            DBClass.CreateParameter("@costCenter", dgvItems.Rows[i].Cells["cost_center"].Value ?? 0));
        //    }

        //    for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
        //    {
        //        if (dgvItems.Rows[i].IsNewRow || string.IsNullOrWhiteSpace(dgvItems.Rows[i].Cells["itemId"].Value?.ToString()))
        //            continue;

        //        string itemId = dgvItems.Rows[i].Cells["itemId"].Value.ToString();
        //        decimal qty = dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value);
        //        decimal discount = string.IsNullOrWhiteSpace(dgvItems.Rows[i].Cells["discount"].Value?.ToString())
        //            ? 0
        //            : Convert.ToDecimal(dgvItems.Rows[i].Cells["discount"].Value);

        //        // Check if item is an assembly
        //        if (dgvItems.Rows[i].Cells["type"].Value.ToString() == "13 - Inventory Assembly")
        //        {
        //            //HandleAssemblyItems(itemId, qty);
        //        }
        //        else
        //        {
        //            // Regular item handling
        //            if (dgvItems.Rows[i].Cells["type"].Value.ToString() != "12 - Service")
        //            {
        //                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,on_hand FROM tbl_items WHERE id = @id",
        //                    DBClass.CreateParameter("id", itemId)))
        //                {
        //                    reader.Read();

        //                    decimal newQty = decimal.Parse(reader["on_hand"].ToString()) + qty;

        //                    // Update the inventory quantity
        //                    DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @qty WHERE id = @id",
        //                        DBClass.CreateParameter("id", reader["id"].ToString()),
        //                        DBClass.CreateParameter("qty", newQty));
        //                }
        //                // Record the item transaction
        //                insertItemTransaction(dgvItems.Rows[i]);
        //            }
        //        }
        //        //add cost center
        //        if (dgvItems.Rows[i].Cells["cost_center"].Value != null && int.Parse(dgvItems.Rows[i].Cells["cost_center"].Value.ToString()) > 0)
        //            CommonInsert.InsertCostCenterTransaction(dtInv.Value, Convert.ToDecimal(dgvItems.Rows[i].Cells["total"].Value ?? 0).ToString(), "0", invId.ToString(), "Purchase", "", (dgvItems.Rows[i].Cells["cost_center"].Value ?? 0).ToString());
        //        //if (Utilities.InventoryAssetFromProduct())
        //        //{
        //        //    decimal amt = 0;
        //        //    decimal amount = decimal.TryParse(dgvItems.Rows[i].Cells["total"].Value?.ToString(), out amt) ? amt : 0;
        //        //    object netResult = DBClass.ExecuteScalar("SELECT asset_account_id FROM tbl_items WHERE id = @id",DBClass.CreateParameter("id", itemId));
        //        //    int level4Id = netResult != DBNull.Value ? Convert.ToInt32(netResult) : 0;

        //        //    if (level4Id > 0)
        //        //    {
        //        //        var existing = costList.FirstOrDefault(x => x.Level4Id == level4Id);
        //        //        if (existing != null)
        //        //        {
        //        //            existing.Amount += amount;
        //        //        }
        //        //        else
        //        //        {
        //        //            costList.Add(new CostItem
        //        //            {
        //        //                Level4Id = level4Id,
        //        //                Amount = amount
        //        //            });
        //        //        }
        //        //    }
        //        //}
        //    }
        //    //if (Utilities.InventoryAssetFromProduct())
        //    //{
        //    //    foreach (var costOfItem in costList)
        //    //    {
        //    //        string level4AccountId = costOfItem.Level4Id.ToString();
        //    //        string itemAmount = costOfItem.Amount.ToString("N2");
        //    //        CommonInsert.addTransactionEntry(dtInv.Value.Date,
        //    //          level4AccountId.ToString(), itemAmount.ToString(), "0", invId.ToString(), "0", "Purchase Invoice", "PURCHASE",
        //    //          "Purchase For Invoice No. " + invCode, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
        //    //    }
        //    //}
        //}

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
                decimal qty = row.Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["qty"].Value);
                decimal discount = string.IsNullOrWhiteSpace(row.Cells["discount"].Value?.ToString()) ? 0 : Convert.ToDecimal(row.Cells["discount"].Value);

                // Prepare parameterized values for each row
                valueList.Add($"(@purchase_id, @item_id{paramIndex}, @qty{paramIndex}, @cost_price{paramIndex}, @price{paramIndex}, @discount{paramIndex}, @vatp{paramIndex}, @vat{paramIndex}, @total{paramIndex}, @costCenter{paramIndex})");
                parameters.Add(new MySqlParameter($"@item_id{paramIndex}", itemId));
                parameters.Add(new MySqlParameter($"@qty{paramIndex}", qty));
                parameters.Add(new MySqlParameter($"@cost_price{paramIndex}", row.Cells["cost_price"].Value ?? 0));
                parameters.Add(new MySqlParameter($"@price{paramIndex}", row.Cells["price"].Value?.ToString() == "" ? "0" : row.Cells["price"].Value));
                parameters.Add(new MySqlParameter($"@discount{paramIndex}", discount));
                parameters.Add(new MySqlParameter($"@vat{paramIndex}", row.Cells["vat"].Value ?? 0));
                parameters.Add(new MySqlParameter($"@vatp{paramIndex}", row.Cells["vatp"].Value ?? 0));
                parameters.Add(new MySqlParameter($"@total{paramIndex}", row.Cells["total"].Value ?? 0));
                parameters.Add(new MySqlParameter($"@costCenter{paramIndex}", row.Cells["cost_center"].Value ?? 0));
                paramIndex++;
            }

            if (valueList.Count == 0)
                return;

            string sql = $@"
                            INSERT INTO tbl_purchase_details
                            (purchase_id, item_id, qty, cost_price, price, discount, vatp, vat, total, cost_center_id)
                            VALUES {string.Join(", ", valueList)};";

            using (var conn = DBClass.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                using (var cmd = new MySqlCommand(sql, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@purchase_id", invId);
                    foreach (var p in parameters)
                        cmd.Parameters.Add(p);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
            }

            // Inventory and transaction updates (still per item)
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                var row = dgvItems.Rows[i];
                if (row.IsNewRow || string.IsNullOrWhiteSpace(row.Cells["itemId"].Value?.ToString()))
                    continue;

                string itemId = row.Cells["itemId"].Value.ToString();
                decimal qty = row.Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["qty"].Value);

                if (row.Cells["type"].Value.ToString() == "13 - Inventory Assembly")
                {
                    //HandleAssemblyItems(itemId, qty);
                }
                else if (row.Cells["type"].Value.ToString() != "12 - Service")
                {
                    //using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,on_hand FROM tbl_items WHERE id = @id",
                    //    DBClass.CreateParameter("id", itemId)))
                    //{
                    //    reader.Read();
                    //    decimal newQty = decimal.Parse(reader["on_hand"].ToString()) + qty;
                    //    DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @qty WHERE id = @id",
                    //        DBClass.CreateParameter("id", reader["id"].ToString()),
                    //        DBClass.CreateParameter("qty", newQty));
                    //}
                    insertItemTransaction(row);
                }

                if (row.Cells["cost_center"].Value != null && int.Parse(row.Cells["cost_center"].Value.ToString()) > 0)
                    CommonInsert.InsertCostCenterTransaction(dtInv.Value, Convert.ToDecimal(row.Cells["total"].Value ?? 0).ToString(), "0", invId.ToString(), "Purchase", "", (row.Cells["cost_center"].Value ?? 0).ToString());
            }
        }

        private void HandleAssemblyItems(string assemblyItemId, decimal assemblyQty)
        {
            // Get the components of the assembly
            using (MySqlDataReader componentReader = DBClass.ExecuteReader(@"SELECT * FROM tbl_item_assembly WHERE assembly_id = @assemblyItemId",
                DBClass.CreateParameter("assemblyItemId", assemblyItemId)))
                while (componentReader.Read())
                {
                    string componentItemId = componentReader["item_id"].ToString();
                    decimal componentQty = Convert.ToDecimal(componentReader["qty"]) * assemblyQty;

                    // Update the inventory for each component used in the assembly
                    MySqlDataReader componentDetails = DBClass.ExecuteReader("SELECT * FROM tbl_items WHERE id = @componentItemId",
                        DBClass.CreateParameter("componentItemId", componentItemId));
                    componentDetails.Read();

                    decimal newComponentQty = decimal.Parse(componentDetails["on_hand"].ToString()) - componentQty;

                    // Update the component inventory
                    DBClass.ExecuteNonQuery("UPDATE tbl_items SET on_hand = @qty WHERE id = @id",
                        DBClass.CreateParameter("id", componentDetails["id"].ToString()),
                        DBClass.CreateParameter("qty", newComponentQty));

                    // Record the transaction for the component
                    insertItemTransactionForComponent(componentItemId, componentQty, assemblyItemId);
                }
        }

        private void insertItemTransactionForComponent(string componentItemId, decimal componentQty, string assemblyItemId)
        {
            // Insert item transaction for the component used in the assembly
            CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Assembly Consumption", invId.ToString(), componentItemId,
                "0", componentQty.ToString(), "0", "0", componentQty.ToString(),
                "Assembly of Item No. " + assemblyItemId, cmbWarehouse.SelectedValue.ToString());
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

            // Insert the item transaction for the purchase=
            CommonInsert.InsertItemTransaction(dtInv.Value.Date, "Purchase Invoice", invId.ToString(), row.Cells["itemId"].Value.ToString(),
                row.Cells["cost_price"].Value.ToString(), row.Cells["qty"].Value.ToString(), "0", "0", row.Cells["qty"].Value.ToString(),
                "Purchase Invoice No. " + txtNextCode.Text, cmbWarehouse.SelectedValue.ToString());
        }

        private void transactions()
        {
            string accountId = cmbPaymentMethod.Text == "Credit" ? level4PaymentCreditMethodId.ToString()
                 : cmbAccountCashName.SelectedValue.ToString();
            if (!string.IsNullOrEmpty(txtTotal.Text))
            {
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                     accountId,
                      "0", TotalAmount.ToString(), invId.ToString(), cmbVendor.SelectedValue.ToString(), cmbPaymentMethod.Text == "Credit" ? "Purchase Invoice" : "Purchase Invoice Cash", "PURCHASE", "Purchase Invoice NO. " + txtNextCode.Text,
                     frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
            }
            if (TotalVatAmount > 0)
            {
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
              level4VatId.ToString(), TotalVatAmount.ToString(), "0", invId.ToString(), "0", "Purchase Invoice", "PURCHASE",
              "Vat Input For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
            }
            if (TotalBeforeAmount > 0)
            {
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                  level4Inventory.ToString(), TotalBeforeAmount.ToString(), "0", invId.ToString(), "0", "Purchase Invoice", "PURCHASE",
                  "Purchase For Invoice No. " + txtNextCode.Text, frmLogin.userId, DateTime.Now.Date, txtNextCode.Text);
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
                int invAcId = GetLevel4AccountIdByItemId(dgvItems.Rows[i].Cells["itemId"].Value.ToString());
                level4Inventory = invAcId > 0 ? invAcId : level4Inventory;
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
            if (level4PaymentCreditMethodId <= 0 && level4VatId <= 0 && level4PurchaseInvoice <= 0 && level4Inventory <= 0)
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private int GetLevel4AccountIdByItemId(string v)
        {
            object objInvAcId = DBClass.ExecuteScalar(@"SELECT asset_account_id FROM tbl_items WHERE id =@id",
                            DBClass.CreateParameter("@id", v));
            int invAcId = (objInvAcId != null && objInvAcId != DBNull.Value) ? int.Parse(objInvAcId.ToString()) : 0;
            return invAcId;
        }

        private void resetTextBox()
        {
            txtSalesMan.Text = txtBillTo.Text = txtShipTo.Text = txtPONO.Text =
                txtTotal.Text = txtTotalVat.Text = "";
            txtInvoiceId.Text = GetNextPurchaseId();
            cmbPaymentMethod.SelectedIndex = id = 0;
            dtInv.Value = DateTime.Now;
            dgvItems.Rows.Clear();
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            //if (id == 0)
            //{
            //    if (insertInvoice())
            //    {
            //        int pId = int.Parse(invId.ToString());
            //        int cId = cmbFixedAssetCategory.SelectedValue == null ? 0 : int.Parse(cmbFixedAssetCategory.SelectedValue.ToString());
            //        if (pId > 0 && cId > 0)
            //        {
            //            frmViewFixedAssets.UpdateOrAddFixedAssets(pId, cId);
            //        }
            //        resetTextBox();
            //    }
            //}

            if (!Utilities.AreDefaultAccountsSet(new List<string> { "Vendor", "Purchase", "Vat Input", "Inventory" }))
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (id == 0)
            {
                if (insertInvoice())
                {
                    int pId = int.Parse(invId.ToString());
                    int cId = cmbFixedAssetCategory.SelectedValue == null ? 0 : int.Parse(cmbFixedAssetCategory.SelectedValue.ToString());
                    if (pId > 0 && cId > 0)
                    {
                        frmViewFixedAssets.UpdateOrAddFixedAssets(pId, cId);
                    }
                    EventHub.RefreshPurchase();
                    if (PO == "PO")
                    {
                        EventHub.RefreshPurchaseOrder();
                    }
                    loadPrint();




                    this.Close();

                }
            }
            else
            {
                if (updateInvoice())
                {
                    int pId = int.Parse(invId.ToString());
                    int cId = cmbFixedAssetCategory.SelectedValue == null ? 0 : int.Parse(cmbFixedAssetCategory.SelectedValue.ToString());
                    if (pId > 0 && cId > 0)
                    {
                        frmViewFixedAssets.UpdateOrAddFixedAssets(pId, cId);
                    }
                    EventHub.RefreshPurchase();
                    loadPrint();
                    this.Close();
                }
            }

        }
        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }
        private void cmbVendor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVendor.SelectedValue == null)
            {
                txtBillTo.Text = txtVendorCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select code from tbl_vendor where id = @id", DBClass.CreateParameter("id", cmbVendor.SelectedValue.ToString())))
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

        private void txtVendorCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_vendor where code =@code",
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

        private void txtVendorCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_vendor where code =@code",
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
                if (e.Control is ComboBox comboBox)
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
                     dgvItems.CurrentCell.OwningColumn.Name == "cost_price" ||
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
                if (row == null || row.IsNewRow)
                    return;

                string taxId = selectedRow["id"].ToString();
                string taxP = selectedRow["value"].ToString();

                decimal price = GetDecimalValue(row, "cost_price");
                decimal qty = GetDecimalValue(row, "qty");
                decimal discount = GetDecimalValue(row, "discount");

                if (price == 0 || qty == 0)
                {
                    row.Cells["total"].Value = row.Cells["discount"].Value = row.Cells["vatp"].Value = "0";
                }
                else
                {
                    decimal netPrice = (qty * price) - discount;
                    row.Cells["net_price"].Value = netPrice;
                    row.Cells["vat"].Value = Convert.ToInt32(taxId);
                    row.Cells["vatp"].Value = (decimal.Parse(netPrice.ToString()) * decimal.Parse(taxP) / 100);
                    row.Cells["total"].Value = netPrice + decimal.Parse(row.Cells["vatp"].Value.ToString());
                }
            }
            //try
            //{
            //    DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)dgvItems.CurrentRow.Cells["vat"];
            //    if (dgvItems.CurrentCell.ColumnIndex != dgvItems.Columns["vat"].Index)
            //        return;
            //    if (comboCell != null)
            //    {
            //        ComboBox comboBox = sender as ComboBox;
            //        DataRowView dRow = (DataRowView)comboBox.SelectedItem;

            //        if (dRow != null)
            //        {
            //            var taxDetails = dRow.Row.ItemArray;
            //            string taxId = dRow.Row.ItemArray[0].ToString();
            //            string taxName = dRow.Row.ItemArray[1].ToString();
            //            string taxP = taxDetails[2].ToString();
            //            decimal price = GetDecimalValue(dgvItems.CurrentRow, "cost_price");
            //            decimal qty = GetDecimalValue(dgvItems.CurrentRow, "qty");
            //            decimal discount = GetDecimalValue(dgvItems.CurrentRow, "discount");

            //            if (price == 0 || qty == 0)
            //                dgvItems.CurrentRow.Cells["total"].Value = dgvItems.CurrentRow.Cells["discount"].Value = dgvItems.CurrentRow.Cells["vatp"].Value = "0";
            //            else
            //            {
            //                decimal netPrice = (qty * price) - discount;
            //                dgvItems.CurrentRow.Cells["net_price"].Value = netPrice;

            //                dgvItems.CurrentRow.Cells["vatp"].Value = (decimal.Parse(netPrice.ToString()) * decimal.Parse(taxP) / 100);
            //            }
            //        }
            //    }
            //    else
            //        dgvItems.CurrentRow.Cells["vatp"].Value = "0";

            //    ChkRowValidity();
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
        }

        private void ChkRowValidity(DataGridViewRow row)
        {
            if (row == null || row.IsNewRow)
                return;

            decimal price = GetDecimalValue(row, "cost_price");
            decimal qty = GetDecimalValue(row, "qty");
            decimal discount = GetDecimalValue(row, "discount");

            if (price == 0 || qty == 0)
            {
                row.Cells["total"].Value = row.Cells["discount"].Value = row.Cells["vatp"].Value = "0";
            }
            else
            {
                try
                {
                    decimal netPrice = (qty * price) - discount;
                    row.Cells["net_price"].Value = netPrice;

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
            //decimal price = GetDecimalValue(dgvItems.CurrentRow, "cost_price");
            //decimal qty = GetDecimalValue(dgvItems.CurrentRow, "qty");
            //decimal discount = GetDecimalValue(dgvItems.CurrentRow, "discount");

            //if (price == 0 || qty == 0)
            //    dgvItems.CurrentRow.Cells["total"].Value = dgvItems.CurrentRow.Cells["discount"].Value = dgvItems.CurrentRow.Cells["vatp"].Value = "0";
            //else
            //{
            //    try
            //    {
            //        decimal netPrice = (qty * price) - discount;
            //        dgvItems.CurrentRow.Cells["net_price"].Value = netPrice;

            //        DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)dgvItems.CurrentRow.Cells["vat"];
            //        if (comboCell != null && comboCell.Value != null)
            //        {
            //            string vP = "0";
            //            foreach (var item in comboCell.Items)
            //            {
            //                DataRowView dr = (DataRowView)item;
            //                string id = dr["id"].ToString();
            //                var name = dr["name"];
            //                var value = dr["value"];
            //                if (dgvItems.CurrentRow.Cells["vat"].Value.ToString() == id)
            //                {
            //                    vP = value.ToString();
            //                }
            //            }

            //            dgvItems.CurrentRow.Cells["vatp"].Value = (decimal.Parse(netPrice.ToString()) * decimal.Parse(vP) / 100);
            //        }
            //        else
            //            dgvItems.CurrentRow.Cells["vatp"].Value = "0";

            //        dgvItems.CurrentRow.Cells["total"].Value = netPrice + decimal.Parse(dgvItems.CurrentRow.Cells["vatp"].Value.ToString());
            //    }
            //    catch (Exception ex)
            //    {
            //        ex.ToString();
            //    }
            //}
        }

        private void RefreshItems()
        {
            //DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items where state = 0 and active = 0");
            //DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            //DataRow newRow = dt.NewRow();
            //newRow["code"] = 0;
            //newRow["name"] = "<< Add New Item >>";
            //dt.Rows.InsertAt(newRow, 0);
            //name.DataSource = dt;
            //name.DisplayMember = "name";
            //name.ValueMember = "code";
        }

        bool CheckItemValidity(int itemId)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select type from tbl_items where id = @id",
                DBClass.CreateParameter("id", itemId)))
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
            TotalBeforeAmount = (total - vat);
            txtTotalBefore.Text = TotalBeforeAmount.ToString("N2");
            TotalVatAmount = Convert.ToDecimal(vat.ToString("N2"));
            txtTotalVat.Text = TotalVatAmount.ToString("N2");
            TotalAmount = Convert.ToDecimal(total.ToString("N2"));
            txtTotal.Text = TotalAmount.ToString("N2");
            FormatNumberWithCommas(txtTotalBefore);
            FormatNumberWithCommas(txtTotalVat);
            FormatNumberWithCommas(txtTotal);
        }
        decimal TotalBeforeAmount = 0, TotalVatAmount = 0, TotalAmount = 0;
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
                    dgvItems.CurrentRow.Cells["itemId"].Value = reader["id"].ToString();
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
            int? currentId = Convert.ToInt32(txtInvoiceId.Text);//Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId == null || currentId <= 1)
                return;

            int previousId = currentId.Value - 1;

            // keep searching backwards until we either find a record or hit the first id
            while (previousId > 0)
            {
                string query = "SELECT id FROM tbl_purchase WHERE state = 0 AND id = @id";
                using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", previousId)))
                {
                    if (reader.Read())
                    {
                        id = int.Parse(reader["id"].ToString());
                        BindInvoice();   // found valid invoice
                        return;
                    }
                }

                previousId--; // try the next one back
            }

            // if nothing was found at all
            clear();
            MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int? currentId = Convert.ToInt32(txtInvoiceId.Text);// Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_purchase WHERE state = 0 AND id = @id";

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
            resetTextBox();
            dgvItems.Rows.Clear();
            txtInvoiceId.Text = GetNextPurchaseId();
            id = 0;
            invId = 0;
            cmbVendor.SelectedIndex = -1;
            cmbWarehouse.SelectedIndex = -1;
            cmbPaymentMethod.SelectedIndex = -1;
            cmbAccountCashName.SelectedIndex = -1;
            cmbShipVia.SelectedIndex = -1;
            dtInv.Value = DateTime.Now.Date;
            txtNextCode.Text = GenerateNextSalesCode();
        }

        private string GetNextPurchaseId()
        {
            string newCode = "1";
            using (var reader = DBClass.ExecuteReader("SELECT IFNULL(MAX(id),0) AS lastCode FROM tbl_purchase"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = code.ToString();
                }
            }

            return newCode;
        }

        private void lnkNewVendor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewVendor().ShowDialog();
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
                if (e.ColumnIndex == dgvItems.Columns["vat"].Index)
                {
                    ChkRowValidity(dgvItems.CurrentRow);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
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

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }


        private void cmbPaymentTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.Text == "Credit")
            {
                dtPaymentTerms.Value = dtPaymentTerms.Value.AddDays(int.Parse(cmbPaymentTerms.Text));
            }
            else
                dtPaymentTerms.Value = DateTime.Now.Date;
        }

        private void loadPurchaseOrderData()
        {
            //get purchase order
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_purchase_order where id = @id",
                DBClass.CreateParameter("id", purchaseOrderId)))
                if (reader.Read())
                {
                    txtPONO.Text = reader["id"].ToString();

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
                    txtNextCode.Text = reader["invoice_id"].ToString();
                    invId = id;
                    txtInvoiceId.Text = invId.ToString();
                    txtBillTo.Text = reader["bill_to"].ToString();
                    txtShipTo.Text = reader["ship_to"].ToString();

                    dgvItems.Rows.Clear();

                    using (MySqlDataReader readerDetails = DBClass.ExecuteReader(@"SELECT tbl_purchase_order_details.*,tbl_items.type,method, tbl_items.code, tbl_items.name FROM tbl_purchase_order_details INNER JOIN 
                                                                    tbl_items ON tbl_purchase_order_details.item_id = tbl_items.id WHERE 
                                                                    tbl_purchase_order_details.purchase_id = @id;",
                                                                    DBClass.CreateParameter("id", purchaseOrderId)))
                        while (readerDetails.Read())
                        {
                            decimal netPrice = Convert.ToDecimal(readerDetails["qty"].ToString()) * Convert.ToDecimal(readerDetails["cost_price"].ToString());
                            dgvItems.Rows.Add(readerDetails["item_id"].ToString(), "", readerDetails["code"].ToString(), readerDetails["name"].ToString(), readerDetails["qty"].ToString(),
                                readerDetails["cost_price"].ToString(), readerDetails["price"].ToString(), readerDetails["discount"].ToString(), netPrice, readerDetails["vat"].ToString(), readerDetails["vatp"].ToString());
                            DataGridViewComboBoxCell comboCell = dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vat"] as DataGridViewComboBoxCell;
                            if (comboCell != null && readerDetails["vat"].ToString() != "0")
                            {
                                comboCell.Value = int.Parse(readerDetails["vat"].ToString());
                                dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) *
                                                   decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_price"].Value.ToString()) * decimal.Parse(readerDetails["vat"].ToString()) / 100);
                            }
                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["total"].Value = ((decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) *
                              decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_price"].Value.ToString()))
                              + decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value == null ? "0" : dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["vatp"].Value.ToString()));

                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["method"].Value = readerDetails["method"].ToString();
                            dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["type"].Value = readerDetails["type"].ToString();
                        }

                    CalculateTotal();
                }
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
                txtBox.Text = number.ToString("N2");
                txtBox.SelectionStart = txtBox.Text.Length;
            }
        }

        public DataTable COMPANYINFO()
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company Limit 1");
        }

        public DataTable VendorDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT p.id,p.date,p.vendor_id,p.invoice_id,p.bill_to,p.city,p.sales_man,p.ship_date,p.ship_via,ship_to,po_num,payment_method,payment_terms,payment_date,(SELECT NAME FROM tbl_coa_level_4 WHERE id=account_cash_id) accountName,total,vat,net,pay,`change` 
                                  , v.name vendorName, v.main_phone,v.email,v.trn,v.mobile from tbl_purchase p, tbl_vendor v
                                  WHERE p.vendor_id = v.id AND p.id = @purchaseId; ", DBClass.CreateParameter("@purchaseId", a1));
        }
        public DataTable PurchaseDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT d.id,d.item_id,i.name,d.qty,d.cost_price,d.price,(d.qty*d.cost_price) subCostTotal,(d.qty*d.price) subPriceTotal,d.discount,((d.qty*d.cost_price)-d.discount) subTotal,d.vatp vatAmount,d.vat vatPercentage,d.total,d.cost_center_id,(SELECT NAME FROM tbl_sub_cost_center WHERE id=cost_center_id) costCenterName,i.type,i.method,i.unit_id,(select name from tbl_unit WHERE id=i.unit_id) unitName, i.code as code FROM  tbl_purchase_details d INNER JOIN tbl_items i ON d.item_id = i.id WHERE d.purchase_id =@purchaseId; ", DBClass.CreateParameter("@purchaseId", a1));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (guna2Panel1.Height == 55)

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
            if (guna2Panel1.Height >= 55)
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
            ShowReport();
        }

        private void guna2TileButton31_Click(object sender, EventArgs e)
        {
            ShowReportReceiverNote();
        }

        private void cmbPaymentTerms_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbPaymentTerms.Text))
            {
                int numberOfDays = Convert.ToInt32(cmbPaymentTerms.Text);
                dtPaymentTerms.Value = dtPaymentTerms.Value.AddDays(numberOfDays);
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {
            clearData();
        }
        private void clearData()
        {
            clear();
            txtBillTo.Text = "";
            id = 0;
        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {
            DBClass.ExecuteNonQuery("UPDATE tbl_purchase SET state = -1 WHERE id = @id; UPDATE tbl_transaction SET state= -1 WHERE transaction_id=@id AND t_type = 'PURCHASE';",
                                          DBClass.CreateParameter("id", id.ToString()));
            CommonInsert.DeleteItemTransaction("Purchase", id.ToString());
            Utilities.LogAudit(frmLogin.userId, "PURCHASE", "Delete Purchase Invoice", id, "Deleted Purchase Invoice with ID: " + id.ToString());
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
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal(id.ToString(), "PURCHASE"));
        }

        private void CbSubcontractors_CheckedChanged(object sender, EventArgs e)
        {
            typed = CbSubcontractors.Checked ? "Subcontractor" : "";
            BindCombos.PopulateVendors(cmbVendor, false, false, typed);
        }

        private void dtInv_ValueChanged(object sender, EventArgs e)
        {
            dtPaymentTerms.Value = dtShip.Value = dtInv.Value;
            if (!string.IsNullOrEmpty(cmbPaymentTerms.Text))
            {
                int numberOfDays = Convert.ToInt32(cmbPaymentTerms.Text);
                dtPaymentTerms.Value = dtPaymentTerms.Value.AddDays(numberOfDays);
            }
        }

        private void guna2TileButton26_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal(id.ToString(), "PURCHASE"));
        }

        private void cmbPurchasetype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPurchasetype.Text.ToString().Trim() == "Fixed Assets")
            {
                labelFixedAssetCategory.Visible = true;
                cmbFixedAssetCategory.Visible = true;
            }
            //else if (cmbPurchasetype.Text.ToString() == "Expense")
            //{
            //    //
            //}
            else
            {
                labelFixedAssetCategory.Visible = false;
                cmbFixedAssetCategory.Visible = false;
            }
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
                ItemNotFoundMessage(inputText);
            }
        }

        private void ItemNotFoundMessage(string name)
        {
            var result = MessageBox.Show(
                                            $"'{name}' not found!\nDo you want to create it?",
                                            "Invalid Item",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning
                                        );

            if (result == DialogResult.Yes)
            {
                new frmViewItem().ShowDialog();
            }

            DataGridViewRow row = dgvItems.CurrentRow;
            if (row != null)
            {
                row.Cells["itemid"].Value = null;
                row.Cells["cost_price"].Value = null;
                row.Cells["price"].Value = null;
                row.Cells["method"].Value = null;
                row.Cells["type"].Value = null;
                row.Cells["vat"].Value = null;
                row.Cells["name"].Value = null;
                row.Cells["code"].Value = null;
            }
        }

        private void guna2TileButton28_Click(object sender, EventArgs e)
        {
            int vendorId = cmbVendor.SelectedValue != null ? int.Parse(cmbVendor.SelectedValue.ToString()) : 0;
            frmLogin.frmMain.openChildForm(new frmPurchaseByVendorDetails(vendorId));
        }

        public void ShowReport()
        {
            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "PurchaseInvoice.rpt");

                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);

                // Fetch your data
                DataTable companyData = COMPANYINFO();
                DataTable VendorData = VendorDetails(invId.ToString());
                DataTable PurchaseData = PurchaseDetails(invId.ToString());

                // Assign data to subreports by name
                cr.Subreports["Company"].SetDataSource(companyData);
                cr.Subreports["Vendor"].SetDataSource(VendorData);
                cr.Subreports["Details"].SetDataSource(PurchaseData);

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

        public void ShowReportReceiverNote()
        {

            try
            {
                // Create the report 
                //ReceiverNote cr = new ReceiverNote();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "ReceiverNote.rpt");

                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                // Load the main report data
                DataTable companyData = COMPANYINFO();  // Assuming you want to pass ID 1
                DataTable VendorData = VendorDetails(invId.ToString());
                DataTable PurchaseData = PurchaseDetails(invId.ToString());
                if (companyData != null)  // Ensure that data was successfully retrieved
                {
                    //cr.SetDataSource(companyData);
                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["Vendor"].SetDataSource(VendorData);
                    cr.Subreports["Items"].SetDataSource(PurchaseData);
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;  // Exit the method if no data is available
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
