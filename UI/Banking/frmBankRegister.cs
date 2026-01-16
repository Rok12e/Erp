using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmBankRegister : Form
    {
        private bool _isLoading = false;
        int id;
        public frmBankRegister(int _id =0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "Register Bank";

            headerUC1.FormText = this.Text;
            this.id = _id;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmBankRegister_Load(object sender, EventArgs e)
        {
            _isLoading = true;
            BindCombos.PopulateRegisterBanksAll(cmbBankName);
            cmbBankName.SelectedIndex = -1;
            BindCombos.PopulateCountries(cmbCountry);
            if (id >0)
            {
                cmbBankName.SelectedValue = id;
                BindData();
            }
            _isLoading = false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!chkRequiredData())
                return;

            if (SaveOrUpdateBank())
            {
                EventHub.RefreshBank();
                this.Close();
            }
            //if (insertBank())
            //{
            //    EventHub.RefreshBank();
            //    this.Close();
            //}
        }
        private bool chkRequiredData()
        {
            if (string.IsNullOrWhiteSpace(txtAbbName.Text))
            {
                MessageBox.Show("Abbreviation name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAbbName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEntId.Text))
            {
                MessageBox.Show("Entity ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEntId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtRoute.Text))
            {
                MessageBox.Show("Route number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtRoute.Focus();
                return false;
            }

            if (cmbBankName.SelectedIndex == -1 && string.IsNullOrWhiteSpace(cmbBankName.Text))
            {
                MessageBox.Show("Bank name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbBankName.Focus();
                return false;
            }

            if (cmbCountry.SelectedValue == null)
            {
                MessageBox.Show("Country is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCountry.Focus();
                return false;
            }

            return true;
        }

        private bool SaveOrUpdateBank()
        {
            int bankId = cmbBankName.SelectedValue != null ? int.Parse(cmbBankName.SelectedValue.ToString()) : 0;
            string bankName = cmbBankName.Text.Trim();

            var countryId = cmbCountry.SelectedValue != null ? cmbCountry.SelectedValue.ToString() : null;
            
            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT * FROM tbl_bank WHERE id <> @id AND (ent_id = @ent_id OR abb_name = @name OR route_num = @route)",
                DBClass.CreateParameter("id", id),
                DBClass.CreateParameter("ent_id", txtEntId.Text.Trim()),
                DBClass.CreateParameter("name", txtAbbName.Text.Trim()),
                DBClass.CreateParameter("route", txtRoute.Text.Trim())))
            {
                if (reader.Read())
                {
                    MessageBox.Show("Duplicate record found for bank: " + reader["name"].ToString(), "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            
            string newCode = GenerateNextBankCode();
            
            string createdBy = frmLogin.userId.ToString();
            DateTime createdDate = DateTime.Now;

            if (id == 0)
            {
                int refId = DBClass.ExecuteNonQuery(@"INSERT INTO tbl_bank 
                          (abb_name, ent_id, route_num, state, name, code, country_id, created_by, created_date) 
                          VALUES (@name, @ent_id, @route, 0, @bankName, @code, @countryId, @createdBy, @createdDate)",
                    DBClass.CreateParameter("name", txtAbbName.Text.Trim()),
                    DBClass.CreateParameter("ent_id", txtEntId.Text.Trim()),
                    DBClass.CreateParameter("route", txtRoute.Text.Trim()),
                    DBClass.CreateParameter("bankName", bankName),
                    DBClass.CreateParameter("code", newCode),
                    DBClass.CreateParameter("countryId", countryId),
                    DBClass.CreateParameter("createdBy", createdBy),
                    DBClass.CreateParameter("createdDate", createdDate));
                Utilities.LogAudit(frmLogin.userId, "Insert Bank", "Bank", refId, "Inserted Bank: " + bankName + " with code: " + newCode);
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_bank SET abb_name = @name, ent_id = @ent_id, route_num = @route, state = 0, name = @bankName, 
                                            country_id = @countryId WHERE id = @id",
                    DBClass.CreateParameter("name", txtAbbName.Text.Trim()),
                    DBClass.CreateParameter("ent_id", txtEntId.Text.Trim()),
                    DBClass.CreateParameter("route", txtRoute.Text.Trim()),
                    DBClass.CreateParameter("bankName", bankName),
                    DBClass.CreateParameter("countryId", countryId),
                    DBClass.CreateParameter("id", id));

                Utilities.LogAudit(frmLogin.userId, "Update Bank", "Bank", id, "Updated Bank: " + bankName + " with code: " + newCode);
            }

            return true;
        }
        
        private string GenerateNextBankCode()
        {
            string lastCode = null;

            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT code FROM tbl_bank ORDER BY code DESC LIMIT 1"))
            {
                if (reader.Read())
                {
                    lastCode = reader["code"].ToString();
                }
            }

            int nextNumber = 1;
            int lastNumber;
            if (!string.IsNullOrEmpty(lastCode) && int.TryParse(lastCode, out lastNumber))
            {
                nextNumber = lastNumber + 1;
            }

            return nextNumber.ToString("D4");
        }

        //private bool insertBank()
        //{
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where id <> @id and (ent_id = @ent_id or abb_name =  @name or route_num = @route)",

        //     DBClass.CreateParameter("route", txtRoute.Text),
        //        DBClass.CreateParameter("ent_id", txtEntId.Text),
        //        DBClass.CreateParameter("name", txtAbbName.Text),
        //    DBClass.CreateParameter("id", cmbBankName.SelectedValue)))
        //        if (reader.Read())
        //        {
        //            MessageBox.Show("Data Exist for Bank : " + reader["name"].ToString());
        //            return false;
        //        }
        //    DBClass.ExecuteNonQuery("update tbl_bank set abb_name = @name, ent_id = @ent_id , route_num = @route, state =0 where id=@id",
        //        DBClass.CreateParameter("route", txtRoute.Text),
        //        DBClass.CreateParameter("ent_id", txtEntId.Text),
        //        DBClass.CreateParameter("name", txtAbbName.Text),
        //    DBClass.CreateParameter("id", cmbBankName.SelectedValue));
        //    return true;
        //}

        private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            BindData();
        }
        private void BindData() {
            if (cmbBankName.SelectedValue == null)
            {
                txtCode.Text = "";
                return;
            }
            string selectedId = cmbBankName.SelectedValue == null ? id.ToString() : cmbBankName.SelectedValue.ToString();
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where id = " + selectedId))
                if (reader.Read())
                {
                    txtCode.Text = reader["code"].ToString();
                    txtAbbName.Text = reader["abb_name"].ToString();
                    txtEntId.Text = reader["ent_id"].ToString();
                    txtRoute.Text = reader["route_num"].ToString();
                    cmbCountry.SelectedValue = reader["country_id"].ToString();
                    btnDelete.Enabled = true;
                }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Deletion functionality can be implemented here if needed
            var confirmResult = MessageBox.Show("Are you sure to delete this bank?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirmResult == DialogResult.Yes)
            {
                if (cmbBankName.SelectedValue == null)
                {
                    MessageBox.Show("Please select a bank to delete.", "No Bank Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                int bankId = (int)cmbBankName.SelectedValue;
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT COUNT(*) AS UsageCount FROM tbl_bank_card WHERE bank_id = @id", DBClass.CreateParameter("id", bankId)))
                {
                    if (reader.Read() && Convert.ToInt32(reader["UsageCount"]) > 0)
                    {
                        MessageBox.Show("Cannot delete this bank as it is referenced in other records.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                DBClass.ExecuteNonQuery("DELETE FROM tbl_bank WHERE id = @id", DBClass.CreateParameter("id", bankId));
                Utilities.LogAudit(frmLogin.userId, "Delete Bank", "Bank", bankId, "Deleted Bank with ID: " + bankId);
                EventHub.RefreshBank();
                this.Close();
            }
        }
    }
}
