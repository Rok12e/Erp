using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewBankCard : Form
    {

        int id;
        public frmViewBankCard(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = id == 0 ? "Bank Card - New Bank Card" : "Bank Card - Edit Bank Card";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewBankCard_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateRegisterBanks(cmbBankName);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            if (id != 0)
            {
                BindBankCard();
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                    if (reader.Read())
                    {
                        txtAccountName.Text = reader["name"].ToString();
                    }
            }
        }
        private void BindBankCard()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank_card where id = " + id))
                if (reader.Read())
                {
                    txtAccManager.Text = reader["account_manager"].ToString();
                    txtAccMob.Text = reader["account_mob"].ToString();
                    txtAccountName.Text = reader["account_name"].ToString();
                    txtAccountNo.Text = reader["account_no"].ToString();
                    txtAccountType.Text = reader["account_type"].ToString();
                    txtAccSign.Text = reader["account_sign"].ToString();
                    txtBranchName.Text = reader["branch_name"].ToString();
                    txtCurrency.Text = reader["currency"].ToString();
                    txtIBANNo.Text = reader["iban_no"].ToString();
                    txtSwift.Text = reader["swift"].ToString();
                    cmbAccountName.SelectedValue = reader["account_id"].ToString();
                    cmbBankName.SelectedValue = reader["bank_id"].ToString();
                    cmbCity.Text = reader["emirates"].ToString();
                    checkBoxCompanyBankAC.Checked = reader["company_ac"].ToString() == "1";

                    btnDelete.Enabled = true;
                }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertCard())
                {
                    EventHub.RefreshBankCard();
                    this.Close();
                }
            }
            else
                if (updateCard())
            {
                EventHub.RefreshBankCard();
                this.Close();
            }
        }
        private bool updateCard()
        {
            if (!ChkValidation())
                return false;
            DBClass.ExecuteNonQuery(@"
                UPDATE tbl_bank_card SET 
                    bank_id = @bank_id,
                    account_name = @account_name,
                    account_type = @account_type,
                    account_no = @account_no,
                    swift = @swift,
                    iban_no = @iban_no,
                    branch_name = @branch_name,
                    emirates = @emirates,
                    currency = @currency,
                    account_manager = @account_manager,
                    account_sign = @account_sign,
                    account_mob = @account_mob,
                    account_id = @account_id,company_ac = @companyAc
                WHERE id = @id;",
                DBClass.CreateParameter("@bank_id", cmbBankName.SelectedValue),
                DBClass.CreateParameter("@account_name", txtAccountName.Text),
                DBClass.CreateParameter("@account_type", txtAccountType.Text),
                DBClass.CreateParameter("@account_no", txtAccountNo.Text),
                DBClass.CreateParameter("@swift", txtSwift.Text),
                DBClass.CreateParameter("@iban_no", txtIBANNo.Text),
                DBClass.CreateParameter("@branch_name", txtBranchName.Text),
                DBClass.CreateParameter("@emirates", cmbCity.Text),
                DBClass.CreateParameter("@currency", txtCurrency.Text),
                DBClass.CreateParameter("@account_manager", txtAccManager.Text),
                DBClass.CreateParameter("@account_sign", txtAccSign.Text),
                DBClass.CreateParameter("@account_mob", txtAccMob.Text),
                DBClass.CreateParameter("@account_id", cmbAccountName.SelectedValue),
                DBClass.CreateParameter("@id", id),
                DBClass.CreateParameter("@companyAc", checkBoxCompanyBankAC.Checked ? 1 : 0)
            );
            return true;
        }
        private bool insertCard()
        {
            if (!ChkValidation())
                return false;
            DBClass.ExecuteNonQuery(@"
                    INSERT INTO tbl_bank_card (
                        bank_id, account_name, account_type, account_no, swift, 
                        iban_no, branch_name, emirates, currency, account_manager, 
                        account_sign, account_mob, account_id, state, created_by, created_date, company_ac
                    )
                    VALUES (
                        @bank_id, @account_name, @account_type, @account_no, @swift, 
                        @iban_no, @branch_name, @emirates, @currency, @account_manager, 
                        @account_sign, @account_mob, @account_id, @state, @created_by, @created_date, @companyAc
                    );",
              DBClass.CreateParameter("@bank_id", cmbBankName.SelectedValue),
              DBClass.CreateParameter("@account_name", txtAccountName.Text),
              DBClass.CreateParameter("@account_type", txtAccountType.Text),
              DBClass.CreateParameter("@account_no", txtAccountNo.Text),
              DBClass.CreateParameter("@swift", txtSwift.Text),
              DBClass.CreateParameter("@iban_no", txtIBANNo.Text),
              DBClass.CreateParameter("@branch_name", txtBranchName.Text),
              DBClass.CreateParameter("@emirates", cmbCity.Text),
              DBClass.CreateParameter("@currency", txtCurrency.Text),
              DBClass.CreateParameter("@account_manager", txtAccManager.Text),
              DBClass.CreateParameter("@account_sign", txtAccSign.Text),
              DBClass.CreateParameter("@account_mob", txtAccMob.Text),
              DBClass.CreateParameter("@account_id", cmbAccountName.SelectedValue),
              DBClass.CreateParameter("@state", 0),
              DBClass.CreateParameter("@created_by", frmLogin.userId),
              DBClass.CreateParameter("@created_date", DateTime.Now.Date),
              DBClass.CreateParameter("@companyAc", checkBoxCompanyBankAC.Checked ? 1 : 0)
            );
            return true;
        }

        private bool ChkValidation()
        {
            if (txtAccountName.Text == "")
            {
                MessageBox.Show("enter Account Name First.");
                return false;
            }
            if (txtAccountNo.Text == "")
            {
                MessageBox.Show("enter Account No First.");
                return false;
            }
            if (txtIBANNo.Text == "")
            {
                MessageBox.Show("enter IBAN No. First.");
                return false;
            }
            if (cmbAccountName.SelectedValue == null || int.Parse(cmbAccountName.SelectedValue.ToString()) <1)
            {
                MessageBox.Show("select Account Name First.");
                return false;
            }
            if(cmbBankName.SelectedValue ==null || int.Parse(cmbBankName.SelectedValue.ToString()) < 1)
            {
                MessageBox.Show("select Bank Name First.");
                return false;
            }
            if (checkBoxCompanyBankAC.Checked)
            {
                object ifExists = DBClass.ExecuteScalar("SELECT COUNT(*) FROM tbl_bank_card WHERE company_ac = 1 and id !=@id",DBClass.CreateParameter("id", id));
                if (ifExists != null && Convert.ToInt32(ifExists) > 0)
                {
                    MessageBox.Show("You can only have one company bank account.");
                    return false;
                }
            }
            return true;
        }
        private void resetTextBox()
        {
            txtAccManager.Text = txtAccMob.Text = txtAccountName.Text = txtAccountNo.Text =
            txtAccountType.Text = txtAccSign.Text = txtBranchName.Text = txtCurrency.Text =
            txtIBANNo.Text = txtSwift.Text = "";
            id = 0;
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

        private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBankName.SelectedValue == null)
            {
                txtBankCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where id = " + cmbBankName.SelectedValue.ToString()))
                if (reader.Read())
                    txtBankCode.Text = reader["code"].ToString();
                else
                    txtBankCode.Text = "";
        }

        private void txtBankCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where code =@code",
                DBClass.CreateParameter("code", txtBankCode.Text)))
                if (reader.Read())
                    cmbBankName.SelectedValue = int.Parse(reader["id"].ToString());

        }

        private void txtBankCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where code =@code",
            DBClass.CreateParameter("code", txtBankCode.Text)))
                if (!reader.Read())
                    cmbBankName.SelectedIndex = -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to delete this bank card?", "Confirm Delete!!", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_cheque where bank_card_id = " + id))
                {
                    if (reader.Read())
                    {
                        MessageBox.Show("You cannot delete this bank card because it is linked to existing cheques.");
                        return;
                    }
                }
                DBClass.ExecuteNonQuery("Delete FROM tbl_bank_card WHERE id = @id ", DBClass.CreateParameter("id", id));
                Utilities.LogAudit(frmLogin.userId, "Delete Bank Card", "Bank Card", id, "Deleted Bank Card: " + txtAccountName.Text);
                EventHub.RefreshBankCard();
                this.Close();
            }
        }
    }
}
