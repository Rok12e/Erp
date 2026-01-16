using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Interop;
using YamyProject.Localization;
using YamyProject.UI.CRM;
using YamyProject.UI.Manufacturing.Viewform;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace YamyProject
{
   
    public partial class frmViewFixedAssets : Form
    {
        public int manID;
        int id;
        private EventHandler FixedAssetCategoryUpdatedHandler;

        public frmViewFixedAssets(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            FixedAssetCategoryUpdatedHandler = (sender, args) => setComboBoxCategory();
            this.id = id;
            headerUC1.FormText = id == 0 ? "Fixed Asset - New" : "Fixed Asset - Edit";
            EventHub.FixedAssetsCategory += FixedAssetCategoryUpdatedHandler;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            EventHub.FixedAssetsCategory -= FixedAssetCategoryUpdatedHandler;
            this.Close();
        }
        private void frmViewFixedAssets_Load(object sender, EventArgs e)
        {
            txtCode.Text = GenerateNextFixedAssetCode();
            BindCombos.PopulateFixedAssetsCategories(cmbCategory);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            BindCombos.PopulateAllLevel4Account(cmbCreditAccount);
            BindCombos.PopulateAllLevel4Account(cmbExpenceAccount);
            cmbCreditAccount.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Fixed Asset Credit Account") ? frmLogin.defaultAccounts["Fixed Asset Credit Account"] : 0;
            cmbAccountName.SelectedValue = frmLogin.defaultAccounts.ContainsKey("Fixed Asset Debit Account") ? frmLogin.defaultAccounts["Fixed Asset Debit Account"] : 0;
            if (id != 0)
                BindAsset();
        }
        private void setComboBoxCategory()
        {
            BindCombos.PopulateFixedAssetsCategories(cmbCategory);
            SetSelectedIndexToLastItem(cmbCategory);
        }

        private void SetSelectedIndexToLastItem(Guna.UI2.WinForms.Guna2ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = comboBox.Items.Count - 1;
            }
        }

        private void txtDepreciationLife_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDepreciationLife.Text))
            {
                txtDepreciationValue.Text = string.Empty;
                txtTotalDays.Text = string.Empty;
                return;
            }
            int lifeYears;
            if (int.TryParse(txtDepreciationLife.Text, out lifeYears) && lifeYears > 0)
            {
                txtDepreciationValue.Text = (100 / lifeYears).ToString();

                DateTime purchaseDate = dtpPurchaseDate.Value.Date;
                DateTime endDate = purchaseDate.AddYears(lifeYears).AddDays(-1);
                dtpEndDate.Value = endDate;

                txtTotalDays.Text = ((endDate - purchaseDate).Days + 1).ToString();
            }
            else
            {
                txtDepreciationValue.Text = string.Empty;
                txtTotalDays.Text = string.Empty;
            }
        }

        private void dtpPurchaseDate_ValueChanged(object sender, EventArgs e)
        {
            if (txtDepreciationLife.Text.Trim() == "")
                return;
            dtpEndDate.Value = dtpPurchaseDate.Value.Date.AddYears(int.Parse(txtDepreciationLife.Text)).AddDays(-1); ;
            txtTotalDays.Text = ((dtpEndDate.Value.Date - dtpPurchaseDate.Value.Date).Days + 1).ToString();
        }
        private void BindAsset()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_fixed_assets where id = " + id))
                if (reader.Read())
                {
                    txtAssetBrand.Text = reader["brand"].ToString();
                    txtAssetName.Text = reader["name"].ToString();
                    txtCode.Text = reader["code"].ToString();
                    txtDepreciationLife.Text = reader["depreciation_life"].ToString();
                    txtInvoiceNumber.Text = reader["invoice_number"].ToString();
                    txtModel.Text = reader["model"].ToString();
                    txtPurchasePrice.Text = reader["purchase_price"].ToString();
                    txtStatus.Text = reader["status"].ToString();
                    txtSupplierName.Text = reader["supplier"].ToString();
                    cmbAccountName.SelectedValue = reader["debit_account_id"].ToString();
                    cmbCreditAccount.SelectedValue = reader["credit_account_id"].ToString();
                    cmbExpenceAccount.SelectedValue = reader["expence_account_id"].ToString();
                    cmbCategory.SelectedValue = reader["category_id"].ToString();
                    dtpAssets.Value = DateTime.Parse(reader["date"].ToString());
                    dtpPurchaseDate.Value = DateTime.Parse(reader["purchase_date"].ToString());
                    if (int.Parse(reader["manufacture"].ToString()) == 1)
                    {
                        guna2ToggleSwitch1.Checked = pnlmanifacuter.Visible == true;
                        var mfrStatus = reader["manufactureStatus"].ToString();
                    }

                    using (MySqlDataReader reader0 = DBClass.ExecuteReader("select * from tbl_purchase where invoice_id = @id and fixed_asset_category_id=@catId and purchase_type='Fixed Assets'",
                        DBClass.CreateParameter("id", txtInvoiceNumber.Text), DBClass.CreateParameter("catId", cmbCategory.SelectedValue.ToString())))
                        if (reader0.Read())
                        {
                            cmbAccountName.SelectedValue = int.Parse(reader0["account_cash_id"].ToString());
                            dtpPurchaseDate.Enabled = false;
                            txtSupplierName.Enabled = false;
                            txtInvoiceNumber.Enabled = false;
                            txtPurchasePrice.Enabled = false;
                            dtpEndDate.Enabled = true;
                        }
                }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (guna2ToggleSwitch1.Checked &&  pnlmanifacuter.Visible == true)
            {
                manID = 1;
            }
            else
            {
                manID = 0;
            }


            if (id == 0)
            {
                if (insertAsset())
                {
                    EventHub.RefreshFixedAsset();
                    this.Close();
                }
            }
            else
            {
                if (updateAsset())
                {
                    EventHub.RefreshFixedAsset();
                    this.Close();
                }
            }
        }
        private bool updateAsset()
        {
            if (!chkRequireData())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_fixed_assets 
                            SET name = @name,date=@date,
                                brand = @brand,
                                category_id = @category_id,
                                model=@model,
                                supplier=@supplier,
                                status=@status,
                                invoice_number=@invoice_number,
                                purchase_date=@purchase_date,
                                end_date=@end_date,
                                depreciation_life=@depreciation_life,
                                purchase_price=@purchase_price,
                                debit_account_id=@debit_account_id,
                                credit_account_id=@credit_account_id,
                                expence_account_id=@expence_account_id,
                                modified_by = @modified_by,
                                modified_date = @modified_date,
                                manufacture = @manufacture
                                WHERE id = @id;",
                              DBClass.CreateParameter("id", id),
                                                        DBClass.CreateParameter("date", dtpAssets.Value.Date),
                              DBClass.CreateParameter("name", txtAssetName.Text),
                              DBClass.CreateParameter("brand", txtAssetName.Text),
                              DBClass.CreateParameter("category_id", cmbCategory.SelectedValue.ToString()),
                              DBClass.CreateParameter("model", txtModel.Text),
                              DBClass.CreateParameter("supplier", txtSupplierName.Text),
                              DBClass.CreateParameter("status", txtStatus.Text),
                              DBClass.CreateParameter("invoice_number", txtInvoiceNumber.Text),
                              DBClass.CreateParameter("purchase_date", dtpPurchaseDate.Value.Date),
                              DBClass.CreateParameter("end_date", dtpEndDate.Value.Date),
                              DBClass.CreateParameter("depreciation_life", txtDepreciationLife.Text),
                              DBClass.CreateParameter("purchase_price", txtPurchasePrice.Text),
                              DBClass.CreateParameter("debit_account_id", cmbAccountName.SelectedValue.ToString()),
                              DBClass.CreateParameter("credit_account_id", cmbCreditAccount.SelectedValue.ToString()),
                              DBClass.CreateParameter("expence_account_id", cmbExpenceAccount.SelectedValue.ToString()),
                              DBClass.CreateParameter("modified_by", frmLogin.userId),
                              DBClass.CreateParameter("modified_date", DateTime.Now.Date),
                              DBClass.CreateParameter("manufacture", manID)).ToString();
            DBClass.ExecuteNonQuery("delete from tbl_transaction where transaction_id = @id and type = 'Fixed Assets'",
                                DBClass.CreateParameter("id", id));
            if (chkJournal.Checked)
            {
                InsertJournal(id);
            }
            Utilities.LogAudit(frmLogin.userId, "Update Fixed Assets", "Fixed Assets", id, "Updated Fixed Assets: " + txtAssetName.Text);
            return true;
        }
        decimal AssetId;
        private string GenerateNextFixedAssetCode()
        {
            int code;
            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_fixed_assets"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 0001;
            }
            return code.ToString("D5");
        }
        private bool insertAsset()
        {
            if (!chkRequireData())
                return false;
            AssetId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_fixed_assets`(date,
                                                            `code`, name, brand, `category_id`, `model`, `supplier`, `status`, 
                                                            `invoice_number`, `purchase_date`, `end_date`, `depreciation_life`,  
                                                            `purchase_price`, debit_account_id,credit_account_id, expence_account_id, `created_by`, `created_date`, `state`,manufacture,
                                                             manufactureStatus) 
                                                        VALUES (@date,
                                                            @code, @name, @brand, @category_id, @model, @supplier, @status, 
                                                            @invoice_number, @purchase_date, @end_date, @depreciation_life, 
                                                            @purchase_price, @debit_account_id, @credit_account_id, @expence_account_id, @created_by, @created_date, 0,
                                                        @manufacture,'Draft');
                                                        SELECT LAST_INSERT_ID();",
                                                        DBClass.CreateParameter("date", dtpAssets.Value.Date),
                              DBClass.CreateParameter("code", GenerateNextFixedAssetCode()),
                              DBClass.CreateParameter("name", txtAssetName.Text),
                              DBClass.CreateParameter("brand", txtAssetBrand.Text),
                              DBClass.CreateParameter("category_id", cmbCategory.SelectedValue.ToString()),
                              DBClass.CreateParameter("model", txtModel.Text),
                              DBClass.CreateParameter("supplier", txtSupplierName.Text),
                              DBClass.CreateParameter("status", txtStatus.Text),
                              DBClass.CreateParameter("invoice_number", txtInvoiceNumber.Text),
                              DBClass.CreateParameter("purchase_date", dtpPurchaseDate.Value.Date),
                              DBClass.CreateParameter("end_date", dtpEndDate.Value.Date),
                              DBClass.CreateParameter("depreciation_life", txtDepreciationLife.Text),
                              DBClass.CreateParameter("purchase_price", txtPurchasePrice.Text),
                              DBClass.CreateParameter("debit_account_id", cmbAccountName.SelectedValue.ToString()),
                              DBClass.CreateParameter("credit_account_id", cmbCreditAccount.SelectedValue.ToString()),
                              DBClass.CreateParameter("expence_account_id", cmbExpenceAccount.SelectedValue.ToString()),
                              DBClass.CreateParameter("created_by", frmLogin.userId),
                              DBClass.CreateParameter("created_date", DateTime.Now.Date),
                              DBClass.CreateParameter("manufacture", manID)).ToString());

            if (chkJournal.Checked)
            {
                InsertJournal((int)AssetId);
            }
            Utilities.LogAudit(frmLogin.userId, "Insert Fixed Assets", "Fixed Assets", (int)AssetId, "Inserted Fixed Assets: " + txtAssetName.Text);
            return true;
        }
        public static void UpdateOrAddFixedAssets(int pId,int cId)
        {
            //load from purchase 
            using (MySqlDataReader reader1 = DBClass.ExecuteReader("select net,(tbl_vendor.name) AS 'Name',tbl_purchase.invoice_id as VoucherNo,tbl_purchase.date as Date from tbl_purchase INNER JOIN tbl_vendor ON tbl_purchase.vendor_id = tbl_vendor.id where tbl_purchase.id =@id",
             DBClass.CreateParameter("id", pId)))
                if (reader1.Read())
                {
                    string PurchasePrice = reader1["net"].ToString();
                    string SupplierName = reader1["Name"].ToString();
                    string Status = "Draft";
                    string voucherNo = reader1["VoucherNo"].ToString();
                    DateTime date = DateTime.Parse(reader1["Date"].ToString());
                    int oldVId = 0;
                    using (MySqlDataReader reader0 = DBClass.ExecuteReader("select * from tbl_fixed_assets WHERE purchase_date = @date AND supplier = @SupplierName AND invoice_number = @voucherNo",
                        DBClass.CreateParameter("date", date),
                        DBClass.CreateParameter("SupplierName", SupplierName),
                        DBClass.CreateParameter("voucherNo", voucherNo)))
                        if (reader0.Read())
                        {
                            oldVId = int.Parse(reader0["id"].ToString());
                        }
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_fixed_assets_category where id = @id", DBClass.CreateParameter("id", cId.ToString())))
                    {
                        if (reader.Read())
                        {
                            int assets_account_id = 0, depreciation_account_id = 0, expense_account_id = 0;

                            if (reader["assets_account_id"] != DBNull.Value && int.Parse(reader["assets_account_id"].ToString()) > 0)
                                assets_account_id = int.Parse(reader["assets_account_id"].ToString());

                            if (reader["depreciation_account_id"] != DBNull.Value && int.Parse(reader["depreciation_account_id"].ToString()) > 0)
                                depreciation_account_id = int.Parse(reader["depreciation_account_id"].ToString());

                            if (reader["expence_account_id"] != DBNull.Value && int.Parse(reader["expence_account_id"].ToString()) > 0)
                                expense_account_id = int.Parse(reader["expence_account_id"].ToString());

                            string AssetName = reader["category_name"].ToString();

                            if (PurchasePrice.Trim() != "")
                            {
                                if (oldVId > 0)
                                {
                                    DBClass.ExecuteNonQuery(@"UPDATE tbl_fixed_assets 
                                                            SET date=@date,
                                                                category_id = @category_id,
                                                                supplier=@supplier,
                                                                status=@status,
                                                                invoice_number=@invoice_number,
                                                                purchase_date=@purchase_date,
                                                                end_date=@end_date,
                                                                depreciation_life=@depreciation_life,
                                                                purchase_price=@purchase_price,
                                                                debit_account_id=@debit_account_id,
                                                                credit_account_id=@credit_account_id,
                                                                expence_account_id=@expence_account_id,
                                                                modified_by = @modified_by,
                                                                modified_date = @modified_date
                                                                WHERE id = @id;",
                                                      DBClass.CreateParameter("id", oldVId),
                                                      DBClass.CreateParameter("date", date),
                                                      DBClass.CreateParameter("category_id", cId.ToString()),
                                                      DBClass.CreateParameter("supplier", SupplierName),
                                                      DBClass.CreateParameter("status", Status),
                                                      DBClass.CreateParameter("invoice_number", voucherNo),
                                                      DBClass.CreateParameter("purchase_date", date),
                                                      DBClass.CreateParameter("end_date", date),
                                                      DBClass.CreateParameter("depreciation_life", "0"),
                                                      DBClass.CreateParameter("purchase_price", PurchasePrice),
                                                      DBClass.CreateParameter("debit_account_id", assets_account_id.ToString()),
                                                      DBClass.CreateParameter("credit_account_id", depreciation_account_id.ToString()),
                                                      DBClass.CreateParameter("expence_account_id", expense_account_id.ToString()),
                                                      DBClass.CreateParameter("modified_by", frmLogin.userId),
                                                      DBClass.CreateParameter("modified_date", DateTime.Now.Date));

                                    Utilities.LogAudit(frmLogin.userId, "Update Fixed Assets", "Fixed Assets", oldVId, "Updated Fixed Assets: " + AssetName);
                                }
                                else
                                {
                                    int code;
                                    using (var reader2 = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_fixed_assets"))
                                    {
                                        if (reader2.Read() && reader2["lastCode"] != DBNull.Value)
                                            code = int.Parse(reader2["lastCode"].ToString()) + 1;
                                        else
                                            code = 0001;
                                    }
                                    string coded = code.ToString("D5");

                                    var AssetId = DBClass.ExecuteScalar(@"INSERT INTO `tbl_fixed_assets`(date,
                                                            `code`, name, brand, `category_id`, `model`, `supplier`, `status`, 
                                                            `invoice_number`, `purchase_date`, `end_date`, `depreciation_life`,  
                                                            `purchase_price`, debit_account_id,credit_account_id, expence_account_id, `created_by`, `created_date`, `state`) 
                                                        VALUES (@date,
                                                            @code, @name, @brand, @category_id, @model, @supplier, @status, 
                                                            @invoice_number, @purchase_date, @end_date, @depreciation_life, 
                                                            @purchase_price, @debit_account_id, @credit_account_id, @expence_account_id, @created_by, @created_date, 0
                                                        );
                                                        SELECT LAST_INSERT_ID();",
                                                                                DBClass.CreateParameter("date", date),
                                                      DBClass.CreateParameter("code", coded),
                                                      DBClass.CreateParameter("name", AssetName),
                                                      DBClass.CreateParameter("brand", ""),
                                                      DBClass.CreateParameter("category_id", cId.ToString()),
                                                      DBClass.CreateParameter("model", ""),
                                                      DBClass.CreateParameter("supplier", SupplierName),
                                                      DBClass.CreateParameter("status", Status),
                                                      DBClass.CreateParameter("invoice_number", voucherNo),
                                                      DBClass.CreateParameter("purchase_date", date),
                                                      DBClass.CreateParameter("end_date", date),
                                                      DBClass.CreateParameter("depreciation_life", "0"),
                                                      DBClass.CreateParameter("purchase_price", PurchasePrice),
                                                      DBClass.CreateParameter("debit_account_id", assets_account_id.ToString()),
                                                      DBClass.CreateParameter("credit_account_id", depreciation_account_id.ToString()),
                                                      DBClass.CreateParameter("expence_account_id", expense_account_id.ToString()),
                                                      DBClass.CreateParameter("created_by", frmLogin.userId),
                                                      DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString();
                                    Utilities.LogAudit(frmLogin.userId, "Insert Fixed Assets", "Fixed Assets", int.Parse(AssetId), "Inserted Fixed Assets: " + AssetName);
                                }
                            }
                        }
                    }
                }
        }

        private void InsertJournal(int pId)
        {
            DateTime startDate = dtpPurchaseDate.Value.Date;
            DateTime endDate = dtpEndDate.Value.Date;
            decimal totalAmount = decimal.Parse(txtPurchasePrice.Text);
            int totalDays = (endDate - startDate).Days + 1;
            string depexpencac = cmbExpenceAccount.SelectedValue.ToString();
            decimal actualTotal = 0;

            if (txtSupplierName.Enabled)
            {
                CommonInsert.InsertTransactionEntry(dtpAssets.Value,
                    cmbAccountName.SelectedValue.ToString(), totalAmount.ToString("F2"), "0", pId.ToString(), "0", "Fixed Assets",
                    txtAssetName.Text + " - Fixed Assets No. " + pId, frmLogin.userId, DateTime.Now.Date);

                CommonInsert.InsertTransactionEntry(dtpAssets.Value,
                    cmbCreditAccount.SelectedValue.ToString(), "0", totalAmount.ToString("F2"), pId.ToString(), "0", "Fixed Assets",
                    txtAssetName.Text + " - Fixed Assets No. " + pId, frmLogin.userId, DateTime.Now.Date);
            }

            DateTime currentMonthStart = startDate;

            while (currentMonthStart <= endDate)
            {
                DateTime currentMonthEnd = new DateTime(currentMonthStart.Year, currentMonthStart.Month, DateTime.DaysInMonth(currentMonthStart.Year, currentMonthStart.Month));
                if (currentMonthEnd > endDate)
                    currentMonthEnd = endDate;

                DateTime periodStart = currentMonthStart < startDate ? startDate : currentMonthStart;
                DateTime periodEnd = currentMonthEnd > endDate ? endDate : currentMonthEnd;

                int daysInPeriod = (periodEnd - periodStart).Days + 1;

                decimal amount = Math.Round((totalAmount / totalDays) * daysInPeriod, 2);

                // Final adjustment to correct rounding
                if (periodEnd == endDate)
                {
                    amount = totalAmount - actualTotal;
                }

                actualTotal += amount;

                CommonInsert.InsertTransactionEntry(periodEnd,
                    depexpencac, amount.ToString("F2"), "0", pId.ToString(), "0", "Fixed Assets",
                    txtAssetName.Text + " - Fixed Assets No. " + pId, frmLogin.userId, DateTime.Now.Date);

                CommonInsert.InsertTransactionEntry(periodEnd,
                    cmbCreditAccount.SelectedValue.ToString(), "0", amount.ToString("F2"), pId.ToString(), "0", "Fixed Assets",
                    txtAssetName.Text + " - Fixed Assets No. " + pId, frmLogin.userId, DateTime.Now.Date);

                currentMonthStart = currentMonthStart.AddMonths(1);
                currentMonthStart = new DateTime(currentMonthStart.Year, currentMonthStart.Month, 1);
            }
        }

        private bool chkRequireData()
        {
            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Select Category First");
                cmbCategory.Focus();
                return false;
            }
            if (txtPurchasePrice.Text.Trim() == "")
            {
                MessageBox.Show("Enter Purchase Price First.");
                txtPurchasePrice.Focus();
                return false;
            }
            if (txtDepreciationValue.Text == "")
            {
                MessageBox.Show("Enter Depreciation In Life");
                return false;
            }
            if (txtAssetName.Text == "")
            {
                MessageBox.Show("Enter Asset Name First");
                txtAssetName.Focus();
                return false;
            }
            return true;
        }


        private void lnkCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //new frmViewFixedAssetsCategory().Show();
            new frmViewFixedAssetsCategory().Show();
        }
        private void cmbAccountName_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cmbCreditAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCreditAccount.SelectedValue == null)
            {
                txtCreditAccountCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbCreditAccount.SelectedValue.ToString()))
                if (reader.Read())
                    txtCreditAccountCode.Text = reader["code"].ToString();
                else
                    txtCreditAccountCode.Text = "";

        }

        private void txtCreditAccountCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (reader.Read())
                    cmbCreditAccount.SelectedValue = int.Parse(reader["id"].ToString());
            
        }

        private void txtCreditAccountCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (!reader.Read())
                    cmbCreditAccount.SelectedIndex = -1;
            
        }

        private void txtPurchasePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && txtPurchasePrice.Text.Contains("."))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-' && txtPurchasePrice.SelectionStart != 0)
            {
                e.Handled = true;
            }
        }

        private void txtDepreciationLife_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && txtDepreciationLife.Text.Contains("."))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-' && txtDepreciationLife.SelectionStart != 0)
            {
                e.Handled = true;
            }
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void cmbexpenceAccount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (!reader.Read())
                    cmbExpenceAccount.SelectedIndex = -1;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
             DBClass.CreateParameter("code", txtExpenceAccount.Text)))
                if (reader.Read())
                    cmbExpenceAccount.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtInvoiceNumber_TextChanged(object sender, EventArgs e)
        {
            //
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedValue!=null && int.Parse(cmbCategory.SelectedValue.ToString()) != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_fixed_assets_category where id =@id", DBClass.CreateParameter("id", cmbCategory.SelectedValue.ToString())))
                {
                    if (reader.Read())
                    {
                        if (reader["assets_account_id"] != DBNull.Value && int.Parse(reader["assets_account_id"].ToString()) > 0)
                            cmbAccountName.SelectedValue = int.Parse(reader["assets_account_id"].ToString());

                        if (reader["depreciation_account_id"] != DBNull.Value && int.Parse(reader["depreciation_account_id"].ToString()) > 0)
                            cmbCreditAccount.SelectedValue = int.Parse(reader["depreciation_account_id"].ToString());

                        if (reader["expence_account_id"] != DBNull.Value && int.Parse(reader["expence_account_id"].ToString()) > 0)
                            cmbExpenceAccount.SelectedValue = int.Parse(reader["expence_account_id"].ToString());
                    }
                }
            }
        }

        private void cmbExpenceAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExpenceAccount.SelectedValue == null)
            {
                txtExpenceAccount.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbExpenceAccount.SelectedValue.ToString()))
                if (reader.Read())
                    txtExpenceAccount.Text = reader["code"].ToString();
                else
                    txtExpenceAccount.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                //
            }
        }

        private void Lbheader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2ToggleSwitch1_CheckedChanged(object sender, EventArgs e)
        {
          
        }
    }
}
