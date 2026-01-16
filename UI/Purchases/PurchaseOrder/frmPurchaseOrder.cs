using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmPurchaseOrder : Form
    {
        private EventHandler vendorUpdatedHandler;
        private EventHandler warehouseUpdatedHandler;

        decimal invId;
        int level4PaymentCreditMethodId;
        int id;

        public frmPurchaseOrder(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            vendorUpdatedHandler = (sender, args) => BindCombos.PopulateVendors(cmbVendor);
            warehouseUpdatedHandler = (sender, args) => BindCombos.PopulateWarehouse(cmbWarehouse);
            EventHub.wareHouse += warehouseUpdatedHandler;
            EventHub.Vendor += vendorUpdatedHandler;
            this.id = id;
        }
        private void frmPurchaseOrder_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Vendor -= vendorUpdatedHandler;
            EventHub.wareHouse -= warehouseUpdatedHandler;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            dtPaymentTerms.Value = dtInv.Value = dtShip.Value = DateTime.Now.Date;
            BindCombos.PopulateWarehouse(cmbWarehouse);
            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Enter At Least One Warehouse");
                new frmViewWarehouse().ShowDialog();
                BindCombos.PopulateWarehouse(cmbWarehouse, false, true);
            }
            loadVat();
            cmbPaymentMethod.SelectedIndex = 0;
            bindCombo();
            txtNextCode.Text = GenerateNextSalesCode();
            txtId.Text = GenerateNextSalesId();
            if (id != 0)
            {
                BindInvoice();
                btnSave.Enabled = UserPermissions.canEdit("Purchase Order");
            }

        }
        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_purchase_order where id = @id",
                DBClass.CreateParameter("id", id)))
            {
                reader.Read();
                txtId.Text = reader["id"].ToString();
                dtInv.Value = DateTime.Parse(reader["date"].ToString());
                txtNextCode.Text = reader["invoice_id"].ToString();
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
                if (IsProjectOption)
                    cmbProject.SelectedValue = int.Parse(reader["project_id"].ToString());
                BindInvoiceItems();
                CalculateTotal();
            }
        }
        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_purchase_order_details.*,tbl_items.type,method, tbl_items.code, tbl_items.name FROM tbl_purchase_order_details INNER JOIN 
                                                                    tbl_items ON tbl_purchase_order_details.item_id = tbl_items.id WHERE 
                                                                    tbl_purchase_order_details.purchase_id = @id;",
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
                    //net_price
                    decimal netPrice = (decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["qty"].Value.ToString()) * decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["cost_price"].Value.ToString())
                        - decimal.Parse(dgvItems.Rows[dgvItems.Rows.Count - 2].Cells["discount"].Value.ToString()));
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
        bool IsProjectOption = false;

        private void bindCombo()
        {
            BindCombos.PopulateVendors(cmbVendor);
            BindCombos.PopulateAllLevel4Account(cmbAccountCashName);
            var defaultAccounts = BindCombos.LoadDefaultAccounts();

            cmbAccountCashName.SelectedValue = defaultAccounts.ContainsKey("Purchase Payment Cash Method")
                ? defaultAccounts["Purchase Payment Cash Method"] : 0;

            level4PaymentCreditMethodId = defaultAccounts.ContainsKey("Vendor") ? defaultAccounts["Vendor"] : 0;
            
            var generalS = Utilities.GeneralSettingsState("PROJECT OPTION");
            if (!string.IsNullOrEmpty(generalS) & int.Parse(generalS) > 0)
            {
                IsProjectOption = true;
            }
            else
            {
                IsProjectOption = false;
            }
            cmbProject.Visible = lblProject.Visible = IsProjectOption;
            BindCombos.PopulateProjects(cmbProject);
        }
        private string GenerateNextSalesCode()
        {
            string newCode = "PO-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(invoice_id, 4) AS UNSIGNED)) AS lastCode FROM tbl_purchase_order"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "PO-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private string GenerateNextSalesId()
        {
            string newCode = "1";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(id) AS lastCode FROM tbl_purchase_order"))
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

            if (id == 0)
            {
                if (insertInvoice())
                {
                    EventHub.RefreshPurchaseOrder();
                    MessageBox.Show("The Purchase Order  Saved");
                    dgvItems.Rows.Clear();

                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
            else
            {
                if (updateInvoice())
                {
                    EventHub.RefreshPurchaseOrder();
                    this.ResetText();
                    MessageBox.Show("The Purchase Order Updated");
                    dgvItems.Rows.Clear();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
        }
        private bool updateInvoice()
        {
            if (!chkRequiredDate())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_purchase_order 
                                     SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date,sales_man=@sales_man, vendor_id = @vendor_id, invoice_id = @invoice_id, warehouse_id = @warehouse_id,
                                     po_num = @po_num, bill_to = @bill_to, ship_date = @ship_date, 
                                     ship_via = @ship_via, ship_to = @ship_to, payment_method = @payment_method, account_cash_id = @account_cash_id, 
                                     payment_terms = @payment_terms, payment_date = @payment_date, total = @total, project_id = @projectId,
                                     vat = @vat, net = @net, pay = @pay, `change` = @change, city = @city, description = @description WHERE id = @id;",
                 DBClass.CreateParameter("id", id),
                  DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("vendor_id", cmbVendor.SelectedValue),
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
               DBClass.CreateParameter("account_cash_id", cmbAccountCashName.SelectedValue),
               DBClass.CreateParameter("payment_terms", cmbPaymentTerms.Text),
               DBClass.CreateParameter("payment_date", dtPaymentTerms.Value.Date),
               DBClass.CreateParameter("vat", txtTotalVat.Text),
               DBClass.CreateParameter("total", txtTotalBefore.Text),
               DBClass.CreateParameter("net", txtTotal.Text),
               DBClass.CreateParameter("description", richTextDescription.Text),
                DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date),
               DBClass.CreateParameter("projectId", cmbProject.SelectedValue != null ? cmbProject.SelectedValue : 0),
            DBClass.CreateParameter("pay", cmbPaymentMethod.Text == "Cash" ? string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text) : 0),
            DBClass.CreateParameter("change", cmbPaymentMethod.Text == "Cash" ? 0 : string.IsNullOrEmpty(txtTotal.Text) ? 0 : Convert.ToDecimal(txtTotal.Text)));
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_purchase_order_details WHERE purchase_id=@id", DBClass.CreateParameter("id", id));

            insertInvItems();
            Utilities.LogAudit(frmLogin.userId, "Update Purchase Order", "Purchase Order", id, "Updated Purchase Order: " + txtNextCode.Text);

            return true;
        }
        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_purchase_order (date, vendor_id, invoice_id,warehouse_id, po_num, bill_to,city,sales_man,
               ship_date,ship_via, ship_to, payment_method,account_cash_id, payment_terms, payment_date,  
               total,vat,net, pay, `change`, created_by, created_date, state, project_id,description) VALUES (@DATE, @vendor_id, @invoice_id,@warehouse_id,
               @po_num, @bill_to,@city,@sales_man, @ship_date, @ship_via,@ship_to, @payment_method,@account_cash_id ,@payment_terms, @payment_date, 
               @total,  @vat,@net, @pay, @change, @created_by, @created_date, @state, @projectId,@description);
               SELECT LAST_INSERT_ID();",
               DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("vendor_id", cmbVendor.SelectedValue),
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
               DBClass.CreateParameter("projectId", cmbProject.SelectedValue!=null? cmbProject.SelectedValue:0),
               DBClass.CreateParameter("state", 0)).ToString());

            insertInvItems();

            Utilities.LogAudit(frmLogin.userId, "Add Purchase Order", "Purchase Order", (int)invId, "Added Purchase Order: " + txtNextCode.Text);
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
                decimal qty = row.Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["qty"].Value);
                decimal discount = string.IsNullOrWhiteSpace(row.Cells["discount"].Value?.ToString()) ? 0 : Convert.ToDecimal(row.Cells["discount"].Value);

                valueList.Add($"(@purchase_id, @item_id{paramIndex}, @qty{paramIndex}, @cost_price{paramIndex}, @price{paramIndex}, @discount{paramIndex}, @vatp{paramIndex}, @vat{paramIndex}, @total{paramIndex})");
                parameters.Add(DBClass.CreateParameter($"@item_id{paramIndex}", itemId));
                parameters.Add(DBClass.CreateParameter($"@qty{paramIndex}", qty));
                parameters.Add(DBClass.CreateParameter($"@cost_price{paramIndex}", row.Cells["cost_price"].Value ?? 0));
                parameters.Add(DBClass.CreateParameter($"@price{paramIndex}", row.Cells["price"].Value?.ToString() ?? "0"));
                parameters.Add(new MySqlParameter($"@discount{paramIndex}", discount));
                parameters.Add(DBClass.CreateParameter($"@vat{paramIndex}", row.Cells["vat"].Value ?? 0));
                parameters.Add(DBClass.CreateParameter($"@vatp{paramIndex}", row.Cells["vatp"].Value ?? 0));
                parameters.Add(DBClass.CreateParameter($"@total{paramIndex}", row.Cells["total"].Value ?? 0));
                paramIndex++;
            }
            if (valueList.Count == 0)
                return;

            string sql = $@"INSERT INTO tbl_purchase_order_details
                            (purchase_id, item_id, qty, cost_price, price, discount, vatp, vat, total)
                            VALUES {string.Join(", ", valueList)};";

            using(var conn = DBClass.GetConnection())
            {
                conn.Open();
                using(var trans = conn.BeginTransaction())
                using (var cmd = new MySqlCommand(sql, conn, trans))
                {
                    cmd.Parameters.AddWithValue("@purchase_id", invId);
                    foreach (var param in parameters)
                        cmd.Parameters.Add(param);

                    cmd.ExecuteNonQuery();
                    trans.Commit();
                }
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
        private void txtVendorCode_TextChanged(object sender, EventArgs e)
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
        private void txtVendorCode_Leave(object sender, EventArgs e)
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

        private bool CheckItemValidity(int itemId)
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
            txtTotalBefore.Text = (total - vat).ToString("0.000");
            txtTotalVat.Text = vat.ToString("0.000");
            txtTotal.Text = total.ToString("0.000");
        }
        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_isUpdatingGrid) return;

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

            string query = "select id from tbl_purchase_order where state = 0 and id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clearData();
                    MessageBox.Show("No previous records available.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int? currentId = Convert.ToInt32(txtId.Text); // Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_purchase_order WHERE state = 0 AND id =@id";

            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clearData();
                    MessageBox.Show("No more records available.");
                }
        }

        private void lnkNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmViewVendor frm = new frmViewVendor();
            frmLogin.frmMain.openChildForm(frm);
            BindCombos.PopulateVendors(cmbVendor,false,true);
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


        private void cmbPaymentTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMethod.Text == "Credit")
            {
                dtPaymentTerms.Value = dtPaymentTerms.Value.AddDays(int.Parse(cmbPaymentTerms.Text));
            }
            else
                dtPaymentTerms.Value = DateTime.Now.Date;
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
        public DataTable COMPANYINFO(int a1)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company Limit 1");
        }

        public DataTable VendorDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT p.id,p.date,p.vendor_id,p.invoice_id,p.bill_to,(SELECT CONCAT(CODE, ' ', NAME, description) FROM tbl_projects WHERE id = p.project_id) as city,p.sales_man,p.ship_date,p.ship_via,ship_to,po_num,payment_method,payment_terms,payment_date,(SELECT NAME FROM tbl_coa_level_4 WHERE id=account_cash_id) accountName,total,vat,net,pay,`change` 
                                  , v.name vendorName, v.main_phone,v.email,v.trn,v.mobile from tbl_purchase_order p, tbl_vendor v
                                  WHERE p.vendor_id = v.id AND p.id = @purchaseId; ",
                                  DBClass.CreateParameter("@purchaseId", a1));
        }
        public DataTable PurchaseDetails(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT d.id,d.item_id,i.name,d.qty,d.cost_price,d.price,(d.qty*d.cost_price) subCostTotal,(d.qty*d.price) subPriceTotal,d.discount,((d.qty*d.cost_price)-d.discount) subTotal,d.vatp vatAmount,d.vat vatPercentage,d.total,d.cost_center_id,(SELECT NAME FROM tbl_sub_cost_center WHERE id=cost_center_id) costCenterName,i.type,i.method,i.unit_id,(select name from tbl_unit WHERE id=i.unit_id) unitName, i.code as code FROM  tbl_purchase_order_details d INNER JOIN tbl_items i ON d.item_id = i.id WHERE d.purchase_id =@purchaseId; "
                            , DBClass.CreateParameter("@purchaseId", a1));
        }
        public void ShowReport()
        {
            try
            {
                //PurchaseOrder cr = new PurchaseOrder();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "PurchaseOrderARC.rpt");

                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                DataTable companyData = COMPANYINFO(1);
                DataTable VendorData = VendorDetails(invId.ToString());
                DataTable PurchaseData = PurchaseDetails(invId.ToString());
                if (companyData != null)
                {
                    cr.SetDataSource(companyData);

                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["Vendor"].SetDataSource(VendorData);
                    cr.Subreports["Details"].SetDataSource(PurchaseData);

                    // Set value of Text15 in "Details" subreport
                    ReportDocument subReport = cr.Subreports["Details"];
                    bool textSet = false;
                    foreach (Section section in subReport.ReportDefinition.Sections)
                    {
                        foreach (ReportObject reportObject in section.ReportObjects)
                        {
                            if (reportObject.Kind == ReportObjectKind.TextObject && reportObject.Name == "Text15")
                            {
                                ((TextObject)reportObject).Text = txtSalesMan.Text;
                                textSet = true;
                                break;
                            }
                        }
                        if (textSet)
                            break;
                    }
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
            ShowReport();
        }

        private void cmbPaymentTerms_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbPaymentTerms.Text))
            {
                int numberOfDays = Convert.ToInt32(cmbPaymentTerms.Text);
                dtPaymentTerms.Value = DateTime.Now.AddDays(numberOfDays);
            }
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {
            clearData();
        }
        private void clearData()
        {
            dgvItems.Rows.Clear();
            resetTextBox();
            txtNextCode.Text = GenerateNextSalesCode();
            txtId.Text = GenerateNextSalesId();
            id = 0;
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertInvoice())
                {
                    EventHub.RefreshPurchaseOrder();
                    this.Close();
                    //this.ResetText();
                    //MessageBox.Show("The Purchase Order  Saved");
                    //dgvItems.Rows.Clear();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
            else
            {
                if (updateInvoice())
                {
                    EventHub.RefreshPurchaseOrder();
                    this.Close();
                    //this.ResetText();
                    //MessageBox.Show("The Purchase Order Updated");
                    //dgvItems.Rows.Clear();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {
            DBClass.ExecuteNonQuery("UPDATE tbl_purchase_order SET state = -1 WHERE id = @id; UPDATE tbl_transaction SET state= -1 WHERE transaction_id=@id AND t_type = 'PURCHASE';",
                                          DBClass.CreateParameter("id", id.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Purchase Order", "Purchase Order", (int)id, "Deleted Purchase Order: " + id);
            clearData();
        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void guna2TileButton20_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        private void guna2TileButton24_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchaseByVendorSummary());
        }

        private bool _isUpdatingGrid = false;

        private void btnImport_Click(object sender, EventArgs e)
        {
            string COGSAccount = "0", incomeAccount = "0", assetAccount = "0", openingBalanceEquity="0";

            //openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();
            assetAccount = BindCombos.SelectDefaultLevelAccount("Inventory").ToString();
            COGSAccount = BindCombos.SelectDefaultLevelAccount("COGS").ToString();
            incomeAccount = BindCombos.SelectDefaultLevelAccount("Sales").ToString();

            frmImportPurchaseOrder importForm = new frmImportPurchaseOrder();
            if (importForm.ShowDialog() != DialogResult.OK) return;

            DataTable importedData = importForm.ImportedData;

            if (importedData == null || importedData.Rows.Count == 0)
            {
                MessageBox.Show("No data found in imported file.");
                return;
            }

            dgvItems.Rows.Clear();

            string VenderName = "", BillDate = "";

            _isUpdatingGrid = true;

            foreach (DataRow row in importedData.Rows)
            {
                string code = "";
                string name = row[0]?.ToString().Trim();
                string costPrice = row[2]?.ToString().Trim();
                string salesPrice = "0";
                string unitName = "";
                string qty = row[1]?.ToString().Trim();
                string barcode = "";
                string minAmount = "0";
                string maxAmount = "0";
                string method = "avg";
                string categoryName = "";
                string warehouseName = "";
                string taxValue = row[3]?.ToString().Trim();
                taxValue = string.IsNullOrEmpty(taxValue) ? "0" : taxValue;
                if (string.IsNullOrEmpty(VenderName))
                    VenderName = row[4]?.ToString().Trim();
                if (string.IsNullOrEmpty(BillDate))
                    BillDate = row[5]?.ToString().Trim();

                if (string.IsNullOrWhiteSpace(name)) continue;

                string warehouseId = cmbWarehouse.SelectedValue.ToString(), categoryId = "1", unitId = "1";
                int itemId = 0;

                //// Lookup/Create Warehouse
                //using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_warehouse WHERE name = @name", DBClass.CreateParameter("name", warehouseName)))
                //{
                //    if (reader.Read())
                //        warehouseId = reader["id"].ToString();
                //    else
                //        warehouseId = DBClass.ExecuteScalar("INSERT INTO tbl_warehouse (name) VALUES (@name); SELECT LAST_INSERT_ID();",
                //            DBClass.CreateParameter("name", warehouseName)).ToString();
                //}

                //// Lookup/Create Category
                //using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_item_category WHERE name = @name", DBClass.CreateParameter("name", categoryName)))
                //{
                //    if (reader.Read())
                //        categoryId = reader["id"].ToString();
                //    else
                //        categoryId = DBClass.ExecuteScalar("INSERT INTO tbl_item_category (code, name) SELECT LPAD(IFNULL(MAX(CAST(code AS UNSIGNED)) + 1, 1),3,'0'), @name FROM tbl_item_category; SELECT LAST_INSERT_ID();",
                //            DBClass.CreateParameter("name", categoryName)).ToString();
                //}

                //// Lookup/Create Unit
                //using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_unit WHERE name = @name", DBClass.CreateParameter("name", unitName)))
                //{
                //    if (reader.Read())
                //        unitId = reader["id"].ToString();
                //    else
                //        unitId = DBClass.ExecuteScalar("INSERT INTO tbl_unit (name) VALUES (@name); SELECT LAST_INSERT_ID();",
                //            DBClass.CreateParameter("name", unitName)).ToString();
                //}

                // Lookup/Create Item
                using (var reader = DBClass.ExecuteReader("SELECT id,code,sales_price FROM tbl_items WHERE name = @name", DBClass.CreateParameter("name", name)))
                {
                    if (reader.Read())
                    {
                        itemId = Convert.ToInt32(reader["id"]);
                        code = reader["code"].ToString();
                        salesPrice = reader["sales_price"].ToString();
                    }
                    else
                    {
                        string newItemCode = code;
                        if (string.IsNullOrEmpty(code))
                        {
                            string typeCategory = "11" + categoryId.PadLeft(3, '0');
                            object lastCode = DBClass.ExecuteScalar("SELECT RIGHT(code, 4) FROM tbl_items WHERE LEFT(code, 5) = @prefix ORDER BY code DESC LIMIT 1",
                                DBClass.CreateParameter("prefix", typeCategory));
                            int nextSerial = (lastCode == DBNull.Value || lastCode == null) ? 1 : Convert.ToInt32(lastCode) + 1;
                            newItemCode = typeCategory + nextSerial.ToString("D4");
                            code = newItemCode;
                        }

                        itemId = (int)decimal.Parse(
                        DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(`code`,`warehouse_id`,  `type`,category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                    `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                    `min_amount`, `max_amount`, `on_hand`,method, `total_value`, `date`, `img`, `active`, `state`, 
                                    `created_By`, `created_date`) VALUES (
                                    @code,@warehouseId, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                    @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                    @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                    @created_By, @created_date); SELECT LAST_INSERT_ID();",
                             DBClass.CreateParameter("code", code),
                             DBClass.CreateParameter("warehouseId", warehouseId),
                             DBClass.CreateParameter("type", "11 - Inventory Part"),
                             DBClass.CreateParameter("category", categoryId),
                             DBClass.CreateParameter("name", name),
                             DBClass.CreateParameter("unit_id", unitId),
                             DBClass.CreateParameter("barcode", barcode),
                             DBClass.CreateParameter("cost_price", costPrice),
                             DBClass.CreateParameter("cogs_account_id", COGSAccount),
                             DBClass.CreateParameter("vendor_id", "0"),
                             DBClass.CreateParameter("sales_price", salesPrice),
                             DBClass.CreateParameter("income_account_id", incomeAccount),
                             DBClass.CreateParameter("asset_account_id", assetAccount),
                             DBClass.CreateParameter("min_amount", minAmount),
                             DBClass.CreateParameter("max_amount", maxAmount),
                             DBClass.CreateParameter("on_hand", qty),
                             DBClass.CreateParameter("method", method.ToString().Trim().ToLower()),
                             DBClass.CreateParameter("total_value", (Convert.ToDecimal(costPrice) * Convert.ToDecimal(qty)).ToString()),
                             DBClass.CreateParameter("date", dtInv.Value.Date),
                             DBClass.CreateParameter("img", ""),
                             DBClass.CreateParameter("active", 0),
                             DBClass.CreateParameter("state", 0),
                             DBClass.CreateParameter("created_By", frmLogin.userId),
                             DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString()
                            );

                        Utilities.LogAudit(frmLogin.userId, "Create Item", "Purchase Order", itemId, "Created new item with code: " + newItemCode);
                        //var comboColumn = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];

                        //// Cast the current DataSource to a DataTable
                        //if (comboColumn.DataSource is DataTable dt)
                        //{
                        //    // Check if item already exists to avoid duplicates
                        //    if (dt.Select($"code = '{newItemCode}'").Length == 0)
                        //    {
                        //        DataRow newRow = dt.NewRow();
                        //        newRow["code"] = newItemCode;
                        //        newRow["name"] = name;
                        //        dt.Rows.Add(newRow);
                        //        dt.AcceptChanges(); // optional but clean
                        //    }
                        //}
                    }
                }

                // Add to dgvItems
                int rowIndex = dgvItems.Rows.Add();

                // Set values
                dgvItems.Rows[rowIndex].Cells["itemId"].Value = itemId;
                dgvItems.Rows[rowIndex].Cells["code"].Value = code;
                dgvItems.Rows[rowIndex].Cells["name"].Value = name;
                dgvItems.Rows[rowIndex].Cells["qty"].Value = Utilities.FormatDecimal(qty);
                dgvItems.Rows[rowIndex].Cells["cost_price"].Value = Utilities.FormatDecimal(costPrice);
                dgvItems.Rows[rowIndex].Cells["price"].Value = Utilities.FormatDecimal(salesPrice);

                // Set VAT combo and percentage
                DataGridViewComboBoxCell comboCell = dgvItems.Rows[rowIndex].Cells["vat"] as DataGridViewComboBoxCell;
                decimal taxP = 0;
                if (comboCell != null && decimal.TryParse(taxValue, out taxP))
                {
                    foreach (var item in comboCell.Items)
                    {
                        if (item is DataRowView drv &&
                            decimal.TryParse(drv["value"].ToString(), out decimal rate) &&
                            rate == taxP)
                        {
                            comboCell.Value = drv[comboCell.ValueMember]; // Set correct combo value (usually ID)
                            break;
                        }
                    }
                    // Set VAT percentage (used for total calculation)
                    dgvItems.Rows[rowIndex].Cells["vatp"].Value = taxP;
                }
                else
                {
                    dgvItems.Rows[rowIndex].Cells["vatp"].Value = 0;
                }

                // Calculate total with VAT
                decimal qtyVal = Utilities.ToDecimal(dgvItems.Rows[rowIndex].Cells["qty"].Value);
                decimal costVal = Utilities.ToDecimal(dgvItems.Rows[rowIndex].Cells["cost_price"].Value);
                decimal netPrice = qtyVal * costVal;
                decimal taxAmt = (netPrice * taxP) / 100;

                dgvItems.Rows[rowIndex].Cells["total"].Value = Utilities.FormatDecimal(netPrice + taxAmt);

                // Set default method and type
                dgvItems.Rows[rowIndex].Cells["method"].Value = "avg";
                dgvItems.Rows[rowIndex].Cells["type"].Value = "11 - Inventory Part";

            }
            _isUpdatingGrid = false;

            int vendorIdFromName = 0;
            if (!string.IsNullOrWhiteSpace(VenderName))
            {
                using (var reader = DBClass.ExecuteReader("SELECT id FROM tbl_vendor WHERE name = @name", DBClass.CreateParameter("name", VenderName)))
                {
                    if (reader.Read())
                    {
                        vendorIdFromName = Convert.ToInt32(reader["id"]);
                    }
                    else
                    {
                        var cmbAccountId = BindCombos.SelectDefaultLevelAccount("Vendor");
                        // Generate next vendor code
                        int nextVendorCode = 20001;
                        using (var codeReader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_vendor"))
                        {
                            if (codeReader.Read() && codeReader["lastCode"] != DBNull.Value)
                                nextVendorCode = Convert.ToInt32(codeReader["lastCode"]) + 1;
                        }
                        string vendorCode = nextVendorCode.ToString("D5");

                        // Optionally lookup country/city IDs (if you have values for them)
                        int countryId = 0;
                        int cityId = 0;

                        //if (!string.IsNullOrWhiteSpace(country))
                        //{
                        //    DataRow[] rows1 = BindDataTable.tablePopulateCountries.Select($"name = '{country}'");
                        //    if (rows1.Length > 0)
                        //        countryId = Convert.ToInt32(rows1[0]["id"]);
                        //}

                        //if (!string.IsNullOrWhiteSpace(city) && countryId > 0)
                        //{
                        //    DataRow[] rows2 = BindDataTable.tablePopulateCities.Select($"name = '{city}'");
                        //    if (rows2.Length > 0)
                        //    {
                        //        cityId = Convert.ToInt32(rows2[0]["id"]);
                        //    }
                        //    else
                        //    {
                        //        cityId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_city (name,country_id) VALUES (@name,@cId); SELECT LAST_INSERT_ID();",
                        //            DBClass.CreateParameter("name", city),
                        //            DBClass.CreateParameter("cId", countryId)));
                        //    }
                        //}

                        // Insert vendor
                        vendorIdFromName = Convert.ToInt32(DBClass.ExecuteScalar(@"
                                INSERT INTO tbl_vendor(code, NAME, Cat_id, Balance, DATE, main_phone, work_phone, mobile, email, ccemail, website,
                                    country, city, region, building_name, account_id, trn, facilty_name, active, created_by, created_date, state)
                                VALUES(@code, @name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website,
                                    @country, @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state);
                                SELECT LAST_INSERT_ID();",
                            DBClass.CreateParameter("code", vendorCode),
                            DBClass.CreateParameter("name", VenderName),
                            DBClass.CreateParameter("cat_id", 0),
                            DBClass.CreateParameter("balance", 0),
                            DBClass.CreateParameter("date", DateTime.Now.Date),
                            DBClass.CreateParameter("main_phone", ""),
                            DBClass.CreateParameter("work_phone", ""),
                            DBClass.CreateParameter("mobile", ""),
                            DBClass.CreateParameter("email", ""),
                            DBClass.CreateParameter("ccemail", ""),
                            DBClass.CreateParameter("website", ""),
                            DBClass.CreateParameter("country", countryId),
                            DBClass.CreateParameter("city", cityId),
                            DBClass.CreateParameter("region", ""),
                            DBClass.CreateParameter("building_name", ""),
                            DBClass.CreateParameter("account_id", cmbAccountId),
                            DBClass.CreateParameter("trn", ""),
                            DBClass.CreateParameter("facilty_name", ""),
                            DBClass.CreateParameter("active", 0),
                            DBClass.CreateParameter("created_by", frmLogin.userId),
                            DBClass.CreateParameter("created_date", DateTime.Now.Date),
                            DBClass.CreateParameter("state", 0)
                        ));
                    }
                }
                BindCombos.PopulateVendors(cmbVendor);
                cmbVendor.SelectedValue = vendorIdFromName;
            }

            CalculateTotal(); // Optional if you use it to recalculate bottom totals
            MessageBox.Show("Import completed successfully.");
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

            if ((columnName == "code" || columnName == "name") && lstAccountSuggestions.Items.Count<=0)
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
    }
}

public class frmImportPurchaseOrder : Form
{
    private TextBox txtStartCell, txtEndCell;
    private TextBox txtItemNameCell, txtQtyCell, txtPriceCell, txtTaxCell, txtVendorCell, txtDateCell;
    private Button btnLoadExcel, btnImport;

    private DataGridView dataGridView;
    private string excelPath = string.Empty;
    DataTable dtFiltered = new DataTable();
    public DataTable ImportedData { get; private set; }

    public frmImportPurchaseOrder()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        this.Text = "Import Purchase Order from Excel";
        this.Size = new Size(1200, 700);
        this.StartPosition = FormStartPosition.CenterScreen;

        Panel panelTop = new Panel() { Dock = DockStyle.Top, Height = 60, Padding = new Padding(10) };

        int x = 10, spacing = 8, labelWidth = 80, textboxWidth = 25, controlHeight = 28;

        btnLoadExcel = new Button() { Text = "Load Excel", Size = new Size(100, controlHeight), Location = new Point(x, 10) };
        x += 100 + spacing;

        btnImport = new Button() { Text = "Set Data", Size = new Size(80, controlHeight), Location = new Point(x, 10) };
        x += 80 + spacing;

        panelTop.Controls.Add(btnLoadExcel);
        panelTop.Controls.Add(btnImport);

        txtStartCell = AddLabelAndTextBox(panelTop, ref x, "Start Cell:", textboxWidth);
        txtEndCell = AddLabelAndTextBox(panelTop, ref x, "End Cell:", textboxWidth);
        txtItemNameCell = AddLabelAndTextBox(panelTop, ref x, "Item:", textboxWidth);
        txtQtyCell = AddLabelAndTextBox(panelTop, ref x, "Qty:", textboxWidth);
        txtPriceCell = AddLabelAndTextBox(panelTop, ref x, "Price:", textboxWidth);
        txtTaxCell = AddLabelAndTextBox(panelTop, ref x, "Tax:", textboxWidth);
        txtVendorCell = AddLabelAndTextBox(panelTop, ref x, "Vendor:", textboxWidth);
        txtDateCell = AddLabelAndTextBox(panelTop, ref x, "Date:", textboxWidth);


        Panel panelMain = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10) };
        dataGridView = new DataGridView() { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        panelMain.Controls.Add(dataGridView);
        this.Controls.Add(panelMain);
        this.Controls.Add(panelTop);

        btnLoadExcel.Click += BtnLoadExcel_Click;
        btnImport.Click += BtnImport_Click;
    }

    private TextBox AddLabelAndTextBox(Panel panel, ref int x, string label, int width)
    {
        panel.Controls.Add(new Label() { Text = label, AutoSize = true, Location = new Point(x, 15) });
        x += 70;
        var txt = new TextBox() { Width = width, Location = new Point(x, 12) };
        panel.Controls.Add(txt);
        x += width + 8;
        return txt;
    }

    private void BtnLoadExcel_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            excelPath = ofd.FileName;
            ImportToGrid();
        }
    }

    private void ImportToGrid()
    {
        try
        {
            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                var ws = package.Workbook.Worksheets[0];
                int startRow = ws.Dimension.Start.Row;
                int endRow = ws.Dimension.End.Row;
                int startCol = ws.Dimension.Start.Column;
                int endCol = ws.Dimension.End.Column;

                DataTable dt = new DataTable();
                dt.Columns.Add("Index");
                for (int col = startCol; col <= endCol; col++)
                    dt.Columns.Add(GetExcelColumnLetter(col));

                for (int row = startRow; row <= endRow; row++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Index"] = row.ToString();
                    for (int col = startCol; col <= endCol; col++)
                        dr[col - startCol + 1] = ws.Cells[row, col].Text;
                    dt.Rows.Add(dr);
                }
                dataGridView.DataSource = dt;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error reading Excel: {ex.Message}");
        }
    }

    private void BtnImport_Click(object sender, EventArgs e)
    {
        int colItem = ExcelColumnNameToNumber(txtItemNameCell.Text);
        int colQty = ExcelColumnNameToNumber(txtQtyCell.Text);
        int colPrice = ExcelColumnNameToNumber(txtPriceCell.Text);
        int colTax = ExcelColumnNameToNumber(txtTaxCell.Text);
        int colVendor = ExcelColumnNameToNumber(txtVendorCell.Text);
        int colDate = ExcelColumnNameToNumber(txtDateCell.Text);

        dtFiltered = new DataTable();
        dtFiltered.Columns.Add("Item name");
        dtFiltered.Columns.Add("Qty");
        dtFiltered.Columns.Add("Price");
        dtFiltered.Columns.Add("Tax");
        dtFiltered.Columns.Add("Vendor");
        dtFiltered.Columns.Add("Invoice/Bill Date");

        using (var package = new ExcelPackage(new FileInfo(excelPath)))
        {
            var ws = package.Workbook.Worksheets[0];
            int colStartIndex = string.IsNullOrWhiteSpace(txtStartCell.Text) ? ws.Dimension.Start.Row + 1 : int.Parse(txtStartCell.Text);
            int colEndIndex = string.IsNullOrWhiteSpace(txtEndCell.Text) ? ws.Dimension.End.Row : int.Parse(txtEndCell.Text);
            for (int row = colStartIndex; row <= colEndIndex; row++)
            {
                if (string.IsNullOrWhiteSpace(ws.Cells[row, colItem].Text)) continue;
                DataRow dr = dtFiltered.NewRow();
                dr["Item name"] = ws.Cells[row, colItem].Text;
                dr["Qty"] = ws.Cells[row, colQty].Text;
                dr["Price"] = ws.Cells[row, colPrice].Text;
                dr["Tax"] = ws.Cells[row, colTax].Text;
                dr["Vendor"] = ws.Cells[row, colVendor].Text;
                dr["Invoice/Bill Date"] = ws.Cells[row, colDate].Text;
                dtFiltered.Rows.Add(dr);
            }
        }

        this.ImportedData = dtFiltered;
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private int ExcelColumnNameToNumber(string columnName)
    {
        if (string.IsNullOrEmpty(columnName)) return -1;
        columnName = columnName.ToUpperInvariant();
        int sum = 0;
        foreach (char c in columnName)
        {
            if (c < 'A' || c > 'Z') return -1;
            sum *= 26;
            sum += (c - 'A' + 1);
        }
        return sum;
    }

    private string GetExcelColumnLetter(int col)
    {
        string columnString = "";
        int columnNumber = col;
        while (columnNumber > 0)
        {
            int currentLetterNumber = (columnNumber - 1) % 26;
            char currentLetter = (char)(currentLetterNumber + 65);
            columnString = currentLetter + columnString;
            columnNumber = (columnNumber - 1) / 26;
        }
        return columnString;
    }
}
