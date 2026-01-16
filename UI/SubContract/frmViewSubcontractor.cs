using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewSubcontractor : Form
    {
        double dbt = 0;
        double crd = 0;
        int id;
        private EventHandler SubContractCategoryUpdatedHandler;

        public frmViewSubcontractor(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            SubContractCategoryUpdatedHandler = (sender, args) => BindCombos.PopulateSubContractCategory(cmbCategory);
            EventHub.VendorCategory += SubContractCategoryUpdatedHandler;
            this.Text = id != 0 ? "SubContract - Edit SubContract" : "SubContract - New SubContract";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewSubcontractor_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCountries(cmbCountry);
            BindCombos.PopulateCities(cmbCity, (int)cmbCountry.SelectedValue);
            BindCombos.PopulateAllLevel4Account(cmbAccount);
            cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Vendor");

            BindCombos.PopulateSubContractCategory(cmbCategory);
            BindCombos.PopulateProjects(CmbProject);
            BindCombos.PopulateListProject(ChkLBox,false,true);
            if (id != 0)
            {
                BindSubContract();
                //btnSave.Enabled = btnSaveAndNew.Enabled = UserPermissions.canEdit("SubContract Center");
            }
        }
        private void BindSubContract()
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
                    chkActive.Checked = (int.Parse(reader["active"].ToString()) == 0) ? true : false;
                    if (decimal.Parse(reader["Balance"].ToString()) != 0)
                    {
                        using (MySqlDataReader dr = DBClass.ExecuteReader("SELECT debit,credit FROM tbl_transaction WHERE hum_id=@id AND TYPE='Subcontractor Opening Balance'", DBClass.CreateParameter("id", id)))
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
                if (insertSubContract())
                {
                    EventHub.RefreshSubContract();
                    this.Close();
                }
            }
            else
            {
                if (updateSubContract())
                {
                    EventHub.RefreshSubContract();
                    this.Close();
                }
            }
        }
        private bool updateSubContract()
        {
            if (!ValidateSubContractInputs())
                return false;

            ValidateOpeningBalance();
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_vendor where name = @name",
               DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    if (id != int.Parse(reader["id"].ToString()))
                    {
                        MessageBox.Show("Vendor Already Exists. Enter Another Name.");
                        return false;
                    }
                }
            string project_site = string.Join(",", ChkLBox.CheckedItems.Cast<object>().Select(item => item.GetType().GetProperty("Value").GetValue(item, null).ToString()));
            DBClass.ExecuteNonQuery(@"Update tbl_vendor Set code = @code,
                            NAME=@NAME, Cat_id = @Cat_id, DATE=@date, main_phone=@main_phone, work_phone=@work_phone, mobile=@mobile, email=@email,
                            ccemail=@ccemail, website=@website, country=@country, city=@city, region=@region, project_id=@project_id,project_site=@project_site,
                            building_name=@building_name, account_id=@account_id, trn=@trn, facilty_name=@facilty_name, active=@active,balance=@balance,type='Subcontractor' where id = @id",
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
                                                          DBClass.CreateParameter("balance", decimal.Parse(txtCredit.Text) - decimal.Parse(txtDepit.Text)),
                                          DBClass.CreateParameter("id", id));


            DBClass.ExecuteNonQuery("DELETE from tbl_transaction WHERE transaction_id=@id AND TYPE='Subcontractor Opening Balance'", DBClass.CreateParameter("id", id));
            ProcessOpeningBalanceTransactions(id, txtCode.Text);
            Utilities.LogAudit(frmLogin.userId, "Update SubContract", "SubContract Center", id, "Updated SubContract: " + txtName.Text);
            return true;
        }

        private bool ValidateSubContractInputs()
        {

            if (txtTRN.Text.Length > 0 && (txtTRN.Text.Length < 3 || txtTRN.Text.Length > 15))
            {
                MessageBox.Show("TRN must be between 3 and 15 characters.");
                txtTRN.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Enter SubContract Name First.");
                return false;
            }

            if (cmbAccount.SelectedValue == null)
            {
                MessageBox.Show("Account Must Be Set For The SubContract");
                return false;
            }
            return true;
        }
        private bool insertSubContract()
        {
            string formattedCode = GenerateNextSubContractCode();

            ValidateOpeningBalance();
            if (!ValidateSubContractInputs()) return false;

            if (IsSubContractExists(txtName.Text)) return false;

            int SubContractId = InsertSubContractRecord(formattedCode);
            if (SubContractId <= 0) return false;

            ProcessOpeningBalanceTransactions(SubContractId, formattedCode);

            txtCode.Text = formattedCode;

            return true;
        }


        private string GenerateNextSubContractCode()
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

        private bool IsSubContractExists(string SubContractName)
        {
            using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_vendor WHERE name = @name",
                           DBClass.CreateParameter("name", SubContractName)))
            {
                if (reader.Read())
                {
                    MessageBox.Show("SubContract Already Exists. Enter Another Name.");
                    return true;
                }
            }
            return false;
        }

        private int InsertSubContractRecord(string formattedCode)
        {
            string project_site = string.Join(",", ChkLBox.CheckedItems.Cast<object>().Select(item => item.GetType().GetProperty("Value").GetValue(item, null).ToString()));
            int retId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_vendor(code,
                            NAME, Cat_id, Balance, DATE, main_phone, work_phone, mobile, email, ccemail, website, country, city, region,
                            building_name, account_id, trn, facilty_name, active, created_by, created_date, state, type, project_id, project_site)
                            VALUES(@code,@name, @cat_id, @balance, @date, @main_phone, @work_phone, @mobile, @email, @ccemail, @website, @country,
                            @city, @region, @building_name, @account_id, @trn, @facilty_name, @active, @created_by, @created_date, @state,'Subcontractor', @project_id, @project_site);SELECT LAST_INSERT_ID();",
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
                                                          DBClass.CreateParameter("project_site", project_site ?? ""),
                                                          DBClass.CreateParameter("region", txtRegion.Text),
                                                          DBClass.CreateParameter("building_name", txtBuildingName.Text),
                                                          DBClass.CreateParameter("account_id", cmbAccount.SelectedValue ?? 0),
                                                          DBClass.CreateParameter("trn", txtTRN.Text),
                                                          DBClass.CreateParameter("facilty_name", txtFacilty.Text),
                                                          DBClass.CreateParameter("active", chkActive.Checked ? 0 : -1),
                                                          DBClass.CreateParameter("created_by", frmLogin.userId),
                                                          DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                                          DBClass.CreateParameter("state", 0)).ToString());
            Utilities.LogAudit(frmLogin.userId, "Add SubContract", "SubContract Center", retId, "Added SubContract: " + txtName.Text);
            return retId;
        }

        private void ProcessOpeningBalanceTransactions(int SubContractId, string formattedCode)
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
                      SubContractId.ToString(), "0", "Subcontractor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, "0", creditAmount.ToString(),
                       SubContractId.ToString(), SubContractId.ToString(), "Subcontractor Opening Balance", "OPENING BALANCE", "Account Payable - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
                }

                if (debitAmount != 0)
                {
                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                            SubContractId.ToString(), "0", "Subcontractor Opening Balance", "OPENING BALANCE", "Opening Balance Equity - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, debitAmount.ToString(), "0",
                       SubContractId.ToString(), SubContractId.ToString(), "Subcontractor Opening Balance", "OPENING BALANCE", "Account Payable - Subcontractor Code - " + formattedCode, frmLogin.userId, DateTime.Now.Date, "");
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
                if (insertSubContract())
                    resetTextBox();
            }
            else
               if (updateSubContract())
            {
                id = 0;
                resetTextBox();
            }
            EventHub.RefreshSubContract();
        }

        private void frmViewSubcontractor_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.SubContractCategory -= SubContractCategoryUpdatedHandler;
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
