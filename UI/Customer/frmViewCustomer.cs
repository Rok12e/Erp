using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewCustomer : Form
    {
        int id;
        private EventHandler customerCategoryUpdatedHandler;

        public frmViewCustomer(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            customerCategoryUpdatedHandler = (sender, args) => BindCombos.PopulateCustomerCategory(cmbCategory);
            EventHub.CustomerCategory += customerCategoryUpdatedHandler;
            this.Text = id != 0 ? "Customers - Edit Customer" : "Customers - New Customer";
            headerUC1.FormText = id != 0 ? "Customers - Edit Customer" : "Customers - New Customer";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewCustomer_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCountries(cmbCountry);
            BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
            BindCombos.PopulateAllLevel4Account(cmbAccount);
            cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Customer");

            BindCombos.PopulateCustomerCategory(cmbCategory);
            BindCombos.PopulateProjects(CmbProject);
            BindCombos.PopulateListProject(ChkLBox, false, true);
            if (id != 0)
            {
                BindCustomer();
                btnSave.Enabled = UserPermissions.canEdit("Customer Center");
            }
        }
        private void BindCustomer()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where id = " + id))
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
                    string project_site = reader["project_site"].ToString();
                    if (!string.IsNullOrEmpty(project_site))
                    {
                        var project_site_ids = project_site.Split(',').ToList();

                        for (int i = 0; i < ChkLBox.Items.Count; i++)
                        {
                            var item = ChkLBox.Items[i];
                            string value = item.GetType().GetProperty("Value").GetValue(item, null).ToString();

                            if (project_site_ids.Contains(value))
                            {
                                ChkLBox.SetItemChecked(i, true);
                            }
                            else
                            {
                                ChkLBox.SetItemChecked(i, false);
                            }
                        }
                    }
                    dtOpen.Value = DateTime.Parse(reader["date"].ToString());
                    chkActive.Checked = int.Parse(reader["active"].ToString()) == 0;
                    if (decimal.Parse(reader["Balance"].ToString()) != 0)
                    {
                        using (MySqlDataReader dr = DBClass.ExecuteReader("SELECT debit,credit FROM tbl_transaction WHERE hum_id=@id AND TYPE='Customer Opening Balance'", DBClass.CreateParameter("id", id)))
                            if (dr.Read())
                            {
                                txtDepit.Text = decimal.Parse(dr["debit"].ToString()).ToString("N2");
                                txtCredit.Text = decimal.Parse(dr["credit"].ToString()).ToString("N2");
                            }
                    }
                    //lblBalance.Visible = txtDepit.Visible = txtCredit.Visible = dtOpen.Visible = false;
                }
        }
        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbCountry.Focused)
                return;

            try
            {
                BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
            } catch(Exception ex)
            {
                ex.ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertCustomer())
                {
                    EventHub.RefreshCustomer();
                    this.Close();
                }
            }
            else
            {
                if (updateCustomer())
                {
                    EventHub.RefreshCustomer();
                    this.Close();
                }
            }
        }
        private bool updateCustomer()
        {
            if (!ValidateCustomerInputs())
                return false;

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where name = @name",
               DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    if (id != int.Parse(reader["id"].ToString()))
                    {
                        MessageBox.Show("Customer Already Exists. Enter Another Name.");
                        return false;
                    }
                }
            ValidateOpeningBalance();

            string project_site = string.Join(",", ChkLBox.CheckedItems.Cast<object>().Select(item => item.GetType().GetProperty("Value").GetValue(item, null).ToString()));
            DBClass.ExecuteNonQuery(@"Update tbl_customer Set code = @code,
                                NAME=@NAME, Cat_id = @Cat_id, DATE=@date, main_phone=@main_phone, work_phone=@work_phone, mobile=@mobile, email=@email,
                                ccemail=@ccemail, website=@website, country=@country, city=@city, region=@region, project_id=@project_id,project_site=@project_site,
                                building_name=@building_name, account_id=@account_id, trn=@trn, facilty_name=@facilty_name, active=@active,Balance=@Balance where id = @id",
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
                                          DBClass.CreateParameter("project_site", project_site ?? ""),
                                          DBClass.CreateParameter("region", txtRegion.Text),
                                          DBClass.CreateParameter("building_name", txtBuildingName.Text),
                                          DBClass.CreateParameter("account_id", cmbAccount.SelectedValue ?? 0),
                                          DBClass.CreateParameter("trn", txtTRN.Text),
                                          DBClass.CreateParameter("facilty_name", txtFacilty.Text),
                                          DBClass.CreateParameter("active", chkActive.Checked ? 0 : -1),
                                          DBClass.CreateParameter("Balance", decimal.Parse(txtDepit.Text) - decimal.Parse(txtCredit.Text)),
                                          DBClass.CreateParameter("id", id));
            DBClass.ExecuteNonQuery("DELETE from tbl_transaction WHERE transaction_id= @id AND TYPE='Customer Opening Balance'", DBClass.CreateParameter("id",id));
            ProcessOpeningBalanceTransactions(id, txtCode.Text);

            Utilities.LogAudit(frmLogin.userId, "UPDATE", "Customer Center", id, "Customer Updated - Code: " + txtCode.Text + ", Name: " + txtName.Text);
            
            return true;
        }

        private bool ValidateCustomerInputs()
        {
           
            if (txtTRN.Text.Length > 0 && (txtTRN.Text.Length < 3 || txtTRN.Text.Length > 15))
            {
                MessageBox.Show("TRN must be between 3 and 15 characters.");
                txtTRN.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter Customer Name First.");
                return false;
            }
            if (cmbAccount.SelectedValue == null)
            {
                MessageBox.Show("Account Must Be Set For The Customer");
                return false;
            }
            return true;
        }

        private bool insertCustomer()
        {
            string formattedCode = GenerateNextCustomerCode();

            if (!ValidateCustomerInputs()) return false;
            if (!ValidateOpeningBalance()) return false;

            if (IsCustomerExists(txtName.Text)) return false;

            int customerId = InsertCustomerRecord(formattedCode);
            if (customerId <= 0) return false;

            ProcessOpeningBalanceTransactions(customerId, formattedCode);

            txtCode.Text = formattedCode;

            return true;

        }

        private string GenerateNextCustomerCode()
        {
            int code;
            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(code AS UNSIGNED)) AS lastCode FROM tbl_customer"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                    code = int.Parse(reader["lastCode"].ToString()) + 1;
                else
                    code = 10001;
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


        private bool IsCustomerExists(string customerName)
        {
            using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_customer WHERE name = @name",
                           DBClass.CreateParameter("name", customerName)))
            {
                if (reader.Read())
                {
                    MessageBox.Show("Customer Already Exists. Enter Another Name.");
                    return true;
                }
            }
            return false;
        }

        private int InsertCustomerRecord(string formattedCode)
        {
            string project_site = string.Join(",", ChkLBox.CheckedItems.Cast<object>().Select(item => item.GetType().GetProperty("Value").GetValue(item, null).ToString()));
            int customerId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_customer (code, NAME, Cat_id, Balance, DATE, 
                main_phone, work_phone, mobile, email, ccemail, website, country, city, region, building_name, account_id, 
                trn, facilty_name, active, created_by, created_date, state, project_id, project_site) 
                VALUES(@code, @name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website, 
                @country, @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state, @project_id, @project_site);
                SELECT LAST_INSERT_ID();",
                DBClass.CreateParameter("code", formattedCode),
                DBClass.CreateParameter("name", txtName.Text),
                DBClass.CreateParameter("cat_id", cmbCategory.SelectedValue ?? 0),
                DBClass.CreateParameter("balance", decimal.Parse(txtDepit.Text) - decimal.Parse(txtCredit.Text)),
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
                DBClass.CreateParameter("project_site", project_site ?? ""),
                DBClass.CreateParameter("region", txtRegion.Text),
                DBClass.CreateParameter("building_name", txtBuildingName.Text),
                DBClass.CreateParameter("account_id", cmbAccount.SelectedValue ?? 0),
                DBClass.CreateParameter("trn", txtTRN.Text),
                DBClass.CreateParameter("facilty_name", txtFacilty.Text),
                DBClass.CreateParameter("active", chkActive.Checked ? 0 : -1),
                DBClass.CreateParameter("created_by", frmLogin.userId),
                DBClass.CreateParameter("created_date", DateTime.Now.Date),
                DBClass.CreateParameter("state", 0)));

            Utilities.LogAudit(frmLogin.userId, "INSERT", "Customer Center", customerId, "New Customer Created - Code: " + formattedCode + ", Name: " + txtName.Text);
            return customerId;

        }


        private void ProcessOpeningBalanceTransactions(int customerId, string formattedCode)
        {
            string accountId = cmbAccount.SelectedValue?.ToString() ?? "0";
            string openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();
            ValidateOpeningBalance();
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
                        customerId.ToString(), "0", "Customer Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, "0", creditAmount.ToString(),
                        customerId.ToString(), customerId.ToString(), "Customer Opening Balance", "OPENING BALANCE", "Opening Balance - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                }

                if (debitAmount != 0)
                {
                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                        customerId.ToString(), "0", "Customer Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, debitAmount.ToString(), "0",
                        customerId.ToString(), customerId.ToString(), "Customer Opening Balance", "OPENING BALANCE", "Opening Balance - Customer Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                }
            }
            else
            {
                MessageBox.Show("Cannot make opening balance without opening balance equity account");
            }
        }



        private void lnkNewCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewCustomerCategory(0).ShowDialog();
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
                if (insertCustomer())
                    resetTextBox();
            }
            else
               if (updateCustomer())
            {
                id = 0;
                resetTextBox();
            }
            EventHub.RefreshCustomer();
        }

        private void frmViewCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.CustomerCategory -= customerCategoryUpdatedHandler;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
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

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            this.BringToFront();
        }

        private void txtDepit_TextChanged(object sender, EventArgs e)
        {
            //txtCredit.Text = "";
        }

        private void txtCredit_TextChanged(object sender, EventArgs e)
        {
            //txtDepit.Text = "";
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
    }
}
