using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewVendor : Form
    {
        double dbt = 0;
        double crd = 0;
        int id;
        private EventHandler vendorCategoryUpdatedHandler;

        public frmViewVendor( int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            vendorCategoryUpdatedHandler = (sender, args) => BindCombos.PopulateVendorCategory(cmbCategory);
            EventHub.VendorCategory += vendorCategoryUpdatedHandler;
            this.Text = id != 0 ? "Vendor - Edit Vendor" : "Vendor - New Vendor";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewVendor_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCountries(cmbCountry);
            BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
            BindCombos.PopulateAllLevel4Account(cmbAccount);
            cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Vendor");

            BindCombos.PopulateVendorCategory(cmbCategory);
            BindCombos.PopulateProjects(CmbProject);
            if (id != 0)
            {
                BindVendor();
                btnSave.Enabled = btnSaveAndNew.Enabled = UserPermissions.canEdit("Vendor Center");
            }
        }
        private void BindVendor()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor where id = " + id))
                if (reader.Read())
                {
                    txtBuildingName.Text = reader["building_name"].ToString();
                    txtCCEmail.Text = reader["ccemail"].ToString();
                    txtCode.Text = reader["code"].ToString();
                    txtEmail.Text = reader["email"].ToString();
                    txtFacilty.Text = reader["facilty_name"].ToString();
                    txtFax.Text = reader["mobile"].ToString();
                    txtMainPhone.Text = reader["main_phone"].ToString();
                    txtName.Text = reader["name"].ToString();
                    txtRegion.Text = reader["region"].ToString();
                    txtTRN.Text = reader["trn"].ToString();
                    txtWebsite.Text = reader["website"].ToString();
                    txtWorkPhone.Text = reader["work_phone"].ToString();
                    cmbAccount.SelectedValue = reader["account_id"].ToString();
                    cmbCategory.SelectedValue = reader["Cat_id"].ToString();
                    cmbCountry.SelectedValue = reader["country"].ToString();
                    cmbCity.SelectedValue = reader["city"].ToString();
                    CmbProject.SelectedValue = reader["project_id"].ToString();
                    dtOpen.Value = DateTime.Parse(reader["date"].ToString());
                    chkActive.Checked = (int.Parse(reader["active"].ToString()) == 0) ? true : false;
                    if (decimal.Parse(reader["Balance"].ToString()) != 0)
                    {
                        using (MySqlDataReader dr = DBClass.ExecuteReader("SELECT debit,credit FROM tbl_transaction WHERE hum_id=@id AND TYPE='Vendor Opening Balance'", DBClass.CreateParameter("id", id)))
                            if (dr.Read())
                            {
                                txtDepit.Text = dr["debit"].ToString();
                                txtCredit.Text = dr["credit"].ToString();
                            }
                    }
                    //lblBalance.Visible = txtDepit.Visible = txtCredit.Visible = dtOpen.Visible = false;
                }
        }
        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (
                cmbCountry.SelectedValue == null
                )
                return;

            BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertVendor())
                {
                    EventHub.RefreshVendor();
                    this.Close();
                }
            }
            else
            {
                if (updateVendor())
                {
                    EventHub.RefreshVendor();
                    this.Close();
                }
            }
        }
        private bool updateVendor()
        {
            if (!ValidateVendorInputs())
                return false;

            ValidateOpeningBalance();
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor where name = @name AND type='Vendor'",
               DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    if (id != int.Parse(reader["id"].ToString()))
                    {
                        MessageBox.Show("Vendor Already Exists. Enter Another Name.");
                        return false;
                    }
                }
            DBClass.ExecuteNonQuery(@"Update tbl_vendor Set code = @code,
                                NAME=@NAME, Cat_id = @Cat_id, DATE=@date, main_phone=@main_phone, work_phone=@work_phone, mobile=@mobile, email=@email,
                                ccemail=@ccemail, website=@website, country=@country, city=@city, region=@region, project_id=@project_id,
                                building_name=@building_name, account_id=@account_id, trn=@trn, facilty_name=@facilty_name, active=@active,balance=@balance,type='Vendor' where id = @id",
                                          DBClass.CreateParameter("code", txtCode.Text),
                                          DBClass.CreateParameter("name", txtName.Text),
                                          DBClass.CreateParameter("cat_id", cmbCategory.SelectedValue ?? 0),
                                          DBClass.CreateParameter("date", dtOpen.Value.Date),
                                          DBClass.CreateParameter("main_phone", txtMainPhone.Text),
                                          DBClass.CreateParameter("work_phone", txtWorkPhone.Text),
                                          DBClass.CreateParameter("mobile", txtFax.Text),
                                          DBClass.CreateParameter("email", txtEmail.Text),
                                          DBClass.CreateParameter("ccemail", txtCCEmail.Text),
                                          DBClass.CreateParameter("website", txtWebsite.Text),
                                          DBClass.CreateParameter("country", cmbCountry.SelectedValue ?? 0),
                                          DBClass.CreateParameter("city", cmbCity.SelectedValue ?? 0),
                                          DBClass.CreateParameter("project_id", CmbProject.SelectedValue ?? 0),
                                          DBClass.CreateParameter("region", txtRegion.Text),
                                          DBClass.CreateParameter("building_name", txtBuildingName.Text),
                                          DBClass.CreateParameter("account_id", cmbAccount.SelectedValue ?? 0),
                                          DBClass.CreateParameter("trn", txtTRN.Text),
                                          DBClass.CreateParameter("facilty_name", txtFacilty.Text),
                                          DBClass.CreateParameter("active", chkActive.Checked ? 0 : -1),
                                                          DBClass.CreateParameter("balance", decimal.Parse(txtCredit.Text) - decimal.Parse(txtDepit.Text)),
                                          DBClass.CreateParameter("id", id));


            DBClass.ExecuteNonQuery("DELETE from tbl_transaction WHERE transaction_id=@id AND TYPE='Vendor Opening Balance'", DBClass.CreateParameter("id", id));
            ProcessOpeningBalanceTransactions(id, txtCode.Text);
            Utilities.LogAudit(frmLogin.userId, "Update Vendor", "Vendor Center", id, "Updated Vendor: " + txtName.Text);
            return true;
        }

        private bool ValidateVendorInputs()
        {

            if (txtTRN.Text.Length > 0 && (txtTRN.Text.Length < 3 || txtTRN.Text.Length > 15))
            {
                MessageBox.Show("TRN must be between 3 and 15 characters.");
                txtTRN.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter Vendor Name First.");
                return false;
            }

            if (cmbAccount.SelectedValue == null)
            {
                MessageBox.Show("Account Must Be Set For The Vendor");
                return false;
            }
            return true;
        }
        private bool insertVendor()
        {
            string formattedCode = GenerateNextVendorCode();

            ValidateOpeningBalance();
            if (!ValidateVendorInputs()) return false;

            if (IsVendorExists(txtName.Text)) return false;

            int vendorId = InsertVendorRecord(formattedCode);
            if (vendorId <= 0) return false;

            ProcessOpeningBalanceTransactions(vendorId, formattedCode);

            txtCode.Text = formattedCode;

            return true;
        }


        private string GenerateNextVendorCode()
        {
            int code;
            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_vendor"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 20001;
            }
            return code.ToString("D5");
        }

        private bool ValidateOpeningBalance()
        {
            if (string.IsNullOrWhiteSpace(txtDepit.Text)) txtDepit.Text = "0";
            if (string.IsNullOrWhiteSpace(txtCredit.Text)) txtCredit.Text = "0";

            if (dtOpen.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Date Value Must Be Less Or Equal to Today");
                return false;
            }
            return true;
        }

        private bool IsVendorExists(string vendorName)
        {
            using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_vendor WHERE name = @name and type='Vendor'",
                           DBClass.CreateParameter("name", vendorName)))
            {
                if (reader.Read())
                {
                    MessageBox.Show("Vendor Already Exists. Enter Another Name.");
                    return true;
                }
            }
            return false;
        }

        private int InsertVendorRecord(string formattedCode)
        {
            int retId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_vendor(code,
                                NAME, Cat_id, Balance, DATE, main_phone, work_phone, mobile, email, ccemail, website, country, city, region,
                                building_name, account_id, trn, facilty_name, active, created_by, created_date, state,type,project_id)
                                VALUES(@code,@name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website, @country,
                                @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state,'Vendor',@project_id);SELECT LAST_INSERT_ID();",
                                                          DBClass.CreateParameter("code", formattedCode),
                                                          DBClass.CreateParameter("name", txtName.Text),
                                                          DBClass.CreateParameter("cat_id", cmbCategory.SelectedValue ?? 0),
                                                          DBClass.CreateParameter("balance", decimal.Parse(txtCredit.Text) - decimal.Parse(txtDepit.Text)),
                                                          DBClass.CreateParameter("date", dtOpen.Value.Date),
                                                          DBClass.CreateParameter("main_phone", txtMainPhone.Text),
                                                          DBClass.CreateParameter("work_phone", txtWorkPhone.Text),
                                                          DBClass.CreateParameter("mobile", txtFax.Text),
                                                          DBClass.CreateParameter("email", txtEmail.Text),
                                                          DBClass.CreateParameter("ccemail", txtCCEmail.Text),
                                                          DBClass.CreateParameter("website", txtWebsite.Text),
                                                          DBClass.CreateParameter("country", cmbCountry.SelectedValue ?? 0),
                                                          DBClass.CreateParameter("city", cmbCity.SelectedValue ?? 0),
                                                          DBClass.CreateParameter("project_id", CmbProject.SelectedValue ?? 0),
                                                          DBClass.CreateParameter("region", txtRegion.Text),
                                                          DBClass.CreateParameter("building_name", txtBuildingName.Text),
                                                          DBClass.CreateParameter("account_id", cmbAccount.SelectedValue ?? 0),
                                                          DBClass.CreateParameter("trn", txtTRN.Text),
                                                          DBClass.CreateParameter("facilty_name", txtFacilty.Text),
                                                          DBClass.CreateParameter("active", chkActive.Checked ? 0 : -1),
                                                          DBClass.CreateParameter("created_by", frmLogin.userId),
                                                          DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                                          DBClass.CreateParameter("state", 0)).ToString());
            Utilities.LogAudit(frmLogin.userId, "Add Vendor", "Vendor Center", retId, "Added Vendor: " + txtName.Text);
            return retId;
        }

        private void ProcessOpeningBalanceTransactions(int vendorId, string formattedCode)
        {
            string accountId = cmbAccount.SelectedValue?.ToString() ?? "0";
            string openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();

            decimal creditAmount = decimal.Parse(txtCredit.Text);
            decimal debitAmount = decimal.Parse(txtDepit.Text);
            if (int.Parse(openingBalanceEquity) == 0)
            {
                object result = DBClass.ExecuteScalar(@"SELECT id FROM tbl_coa_level_4 WHERE name = 'Opening Balance Equity'");
                if (result != null && result != DBNull.Value)
                {
                    openingBalanceEquity = result.ToString();
                }
            }
            if (int.Parse(openingBalanceEquity) > 0)
            {
                if (creditAmount != 0)
                {
                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, openingBalanceEquity, creditAmount.ToString(), "0",
                      vendorId.ToString(), "0", "Vendor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, "0", creditAmount.ToString(),
                       vendorId.ToString(), vendorId.ToString(), "Vendor Opening Balance", "OPENING BALANCE", "Account Payable - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                }

                if (debitAmount != 0)
                {
                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                            vendorId.ToString(), "0", "Vendor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, debitAmount.ToString(), "0",
                       vendorId.ToString(), vendorId.ToString(), "Vendor Opening Balance", "OPENING BALANCE", "Account Payable - Vendor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                }
            }
            else
            {
                MessageBox.Show("Cannot make opening balance without opening balance equity account");
            }
        }

        private void lnkNewCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewVendorCategory(0).ShowDialog();
        }
        private void resetTextBox()
        {
            txtBuildingName.Text = txtCCEmail.Text = txtCredit.Text = txtDepit.Text = txtEmail.Text = txtFacilty.Text = txtFax.Text
            = txtCode.Text = txtMainPhone.Text = txtName.Text = txtRegion.Text = txtTRN.Text = txtWebsite.Text = txtWorkPhone.Text = "";
            id = 0;
        }
        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertVendor())
                    resetTextBox();
            }
            else
               if (updateVendor())
            {
                id = 0;
                resetTextBox();
            }
            EventHub.RefreshVendor();
        }

        private void frmViewVendor_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.VendorCategory -= vendorCategoryUpdatedHandler;
        }

        private void guna2GroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtDepit_TextChanged(object sender, EventArgs e)
        {
            //txtCredit.Text = "0";

        }

        private void txtCredit_TextChanged(object sender, EventArgs e)
        {
            //txtDepit.Text = "0";

        }

        private void txtDepit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;

            // Allow digits and decimal point
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if (e.KeyChar == '.' && txtDepit.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtCredit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;

            // Allow digits and decimal point
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if (e.KeyChar == '.' && txtCredit.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
