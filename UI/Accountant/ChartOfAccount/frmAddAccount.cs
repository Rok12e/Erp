using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAddAccount : Form
    {
        EventHandler lvl1, lvl2, lvl3;
        public frmAddAccount()
        {
            InitializeComponent(); 
            LocalizationManager.LocalizeForm(this);
            btnSave.Enabled = lnkNewCategory.Enabled = lnlLvl2Add.Enabled = lnkLvl3Add.Enabled = UserPermissions.canView("Chart Of Account");
            lnkLvl1Edit.Enabled = lnkLvl2Edit.Enabled = lnkLvl3Edit.Enabled = UserPermissions.canEdit("Chart Of Account");
            lnkLvl1Delete.Enabled = lnkLvl2Delete.Enabled = lnkLvl3Delete.Enabled = UserPermissions.canDelete("Chart Of Account");
            lvl1 = (sender, args) => setComboBoxLevel1();
            lvl2 = (sender, args) => setComboBoxLevel2();
            lvl3 = (sender, args) => setComboBoxLevel3();
            EventHub.lvl1Account += lvl1;
            EventHub.lvl2Account += lvl2;
            EventHub.lvl3Account += lvl3;
            headerUC1.FormText = "COA - New Account";
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmAddAccount_Load(object sender, EventArgs e)
        {
            dtOpen.Value = DateTime.Now.Date;
            BindCombos.Populatelevel1Account(cmbLevel1);
            BindCombos.Populatelevel2Account(cmbLevel2, cmbLevel1.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel1.SelectedValue);
            BindCombos.Populatelevel3Account(cmbLevel3, cmbLevel2.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel2.SelectedValue);
        }
        private void setComboBoxLevel1()
        {
            BindCombos.Populatelevel1Account(cmbLevel1);
            SetSelectedIndexToLastItem(cmbLevel1);
        }
        private void setComboBoxLevel2()
        {
            BindCombos.Populatelevel2Account(cmbLevel2, cmbLevel1.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel1.SelectedValue);
            SetSelectedIndexToLastItem(cmbLevel2);
        }
        private void setComboBoxLevel3()
        {
            BindCombos.Populatelevel3Account(cmbLevel3, cmbLevel2.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel2.SelectedValue);
            SetSelectedIndexToLastItem(cmbLevel3);
        }

        private void SetSelectedIndexToLastItem(Guna.UI2.WinForms.Guna2ComboBox comboBox)
        {
            if (comboBox.Items.Count > 0)
            {
                comboBox.SelectedIndex = comboBox.Items.Count - 1;
            }
        }
        private void cmbLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCombos.Populatelevel2Account(cmbLevel2, cmbLevel1.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel1.SelectedValue);
            if (cmbLevel2.SelectedValue == null)
            {
                cmbLevel3.DataSource = null;
                return;
            }
            BindCombos.Populatelevel3Account(cmbLevel3, cmbLevel2.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel2.SelectedValue);
        }

        private void cmbLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCombos.Populatelevel3Account(cmbLevel3, cmbLevel2.SelectedValue.ToString() == "" ? 0 : (int)cmbLevel2.SelectedValue);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (insertAccount())
            {
                EventHub.Refreshlvl4Account();
                this.Close();
            }
        }
        private bool insertAccount()
        {
            if (dtOpen.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Enter Correct Date");
                return false;
            }
            if (txtLevel4.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Level 4 Account Name First.");
                return false;
            }
            if (cmbLevel3.SelectedValue == null || cmbLevel3.Text == "")
            {
                MessageBox.Show("Please Select Level 3 First.");
                return false;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where name = @name",
               DBClass.CreateParameter("name", txtLevel4.Text)))
                if (reader.Read())
                {
                    MessageBox.Show("Account Already Exists. Enter Another Name.");
                    reader.Dispose();
                    return false;
                }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select code from tbl_coa_level_3 where id = @id",
                DBClass.CreateParameter("id", cmbLevel3.SelectedValue.ToString())))
            {
                reader.Read();
                using (MySqlDataReader r = DBClass.ExecuteReader("select max(code) as code from tbl_coa_level_4 where code like '" + reader["code"].ToString() + "%'"))
                {

                    if (txtCredit.Text == "")
                    {
                        txtCredit.Text = "0";
                    }
                    if (txtDebit.Text == "")
                    {
                        txtDebit.Text = "0";
                    }
                    if (r.Read() && r["code"].ToString() != "")
                    {
                        int refId = int.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_coa_level_4`(`name`, `code`, `main_id`,debit,credit,date)
                                    VALUES(@name,@code, @id,@debit,@credit,@date); SELECT LAST_INSERT_ID();",
                                                       DBClass.CreateParameter("name", txtLevel4.Text),
                                                       DBClass.CreateParameter("id", cmbLevel3.SelectedValue.ToString()),
                                                       DBClass.CreateParameter("code", int.Parse(r["code"].ToString()) + 1),
                                                       DBClass.CreateParameter("debit", txtDebit.Text.ToString()),
                                                       DBClass.CreateParameter("credit", txtCredit.Text),
                                                       DBClass.CreateParameter("date", dtOpen.Value)).ToString());
                        if (decimal.Parse(txtCredit.Text.ToString().Trim()) > 0 || decimal.Parse(txtDebit.Text.ToString().Trim()) > 0)
                        {
                            insertTransactions(refId, (int.Parse(r["code"].ToString()) + 1).ToString());
                        }
                        Utilities.LogAudit(frmLogin.userId, "Add Account", "Chart Of Account", refId, "Added Account: " + txtLevel4.Text + " with Code: " + (int.Parse(r["code"].ToString()) + 1).ToString());
                    }
                    else
                    {
                        int refId = int.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_coa_level_4`(`name`, `code`, `main_id`,debit,credit,date)
                                    VALUES(@name,@code, @id,@debit,@credit,@date); SELECT LAST_INSERT_ID();",
                                                  DBClass.CreateParameter("name", txtLevel4.Text),
                                                  DBClass.CreateParameter("id", cmbLevel3.SelectedValue.ToString()),
                                                  DBClass.CreateParameter("code", int.Parse(reader["code"].ToString()) + "001"),
                                                       DBClass.CreateParameter("debit", txtDebit.Text.ToString()),
                                                       DBClass.CreateParameter("credit", txtCredit.Text),
                                                       DBClass.CreateParameter("date", dtOpen.Value)).ToString());
                        if (decimal.Parse(txtCredit.Text.ToString().Trim()) > 0 || decimal.Parse(txtDebit.Text.ToString().Trim()) > 0)
                        {
                            insertTransactions(refId, (int.Parse(reader["code"].ToString()) + "001").ToString());
                        }
                        Utilities.LogAudit(frmLogin.userId, "Add Account", "Chart Of Account", refId, "Added Account: " + txtLevel4.Text + " with Code: " + (int.Parse(reader["code"].ToString()) + "001").ToString());
                    }
                }
            }
            return true;
        }
        private void insertTransactions(int refId, string code)
        {
            string accountId = refId.ToString();
            string openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();

            decimal creditAmount = decimal.Parse(txtCredit.Text);
            decimal debitAmount = decimal.Parse(txtDebit.Text);
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
                      refId.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Opening Balance Equity - Ledger", frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, "0", creditAmount.ToString(),
                       refId.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Account Payable - Ledger Code ", frmLogin.userId, DateTime.Now.Date, "");
                }

                if (debitAmount != 0)
                {
                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, openingBalanceEquity, "0", debitAmount.ToString(),
                            refId.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Opening Balance Equity - Ledger Code ", frmLogin.userId, DateTime.Now.Date, "");

                    CommonInsert.addTransactionEntry(dtOpen.Value.Date, accountId, debitAmount.ToString(), "0",
                       refId.ToString(), "0", "General Ledger Opening Balance", "GENERAL LEDGER OPENING BALANCE", "Account Payable - Ledger Code - ", frmLogin.userId, DateTime.Now.Date, "");
                }
            }
            else
            {
                MessageBox.Show("Cannot make opening balance without opening balance equity account");
            }
        }

        private void lnkNewCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewLevel1(0).ShowDialog();
        }


        private void lnkLvl1Edit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbLevel1.SelectedValue == null || cmbLevel1.Text == "")
            {
                MessageBox.Show("Select Level 1 First.");
                return;
            }
            new frmViewLevel1(int.Parse(cmbLevel1.SelectedValue.ToString())).ShowDialog();
        }

        private void lnkLvl2Edit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbLevel2.SelectedValue == null || cmbLevel2.Text == "")
            {
                MessageBox.Show("Select Level 2 First.");
                return;
            }
            new frmViewLevel2(int.Parse(cmbLevel2.SelectedValue.ToString())).ShowDialog();
        }

        private void lnkLvl3Edit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbLevel3.SelectedValue == null || cmbLevel2.Text == "")
            {
                MessageBox.Show("Select Level 3 First.");
                return;
            }
            new frmViewLevel3(int.Parse(cmbLevel3.SelectedValue.ToString())).ShowDialog();
        }
        private void lnkLvl3Add_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewLevel3(0, int.Parse(cmbLevel1.SelectedValue.ToString()), int.Parse(cmbLevel2.SelectedValue.ToString())).ShowDialog();
        }

        private void lnkLvl1Delete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbLevel1.SelectedValue == null || cmbLevel1.Text == "")
            {
                MessageBox.Show("Select Level 1 First.");
                return;
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select count(*) from tbl_coa_level_2 where main_id = @id",
                DBClass.CreateParameter("id", cmbLevel1.SelectedValue.ToString())))
                    if (reader.Read())
                    {
                        if (int.Parse(reader[0].ToString()) > 0)
                        {
                            MessageBox.Show(cmbLevel1.Text.ToString() + " is used. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            DBClass.ExecuteNonQuery("delete from tbl_coa_level_1 where id = @id",
                            DBClass.CreateParameter("id", cmbLevel1.SelectedValue.ToString()));

                            MessageBox.Show(cmbLevel1.Text.ToString() + " deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EventHub.Refreshlvl4Account();
                        }
                    }
            }
        }

        private void lnkLvl2Delete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbLevel2.SelectedValue == null || cmbLevel2.Text == "")
            {
                MessageBox.Show("Select Level 2 First.");
                return;
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select count(*) from tbl_coa_level_3 where main_id = @id",
                DBClass.CreateParameter("id", cmbLevel2.SelectedValue.ToString())))
                    if (reader.Read())
                    {
                        if (int.Parse(reader[0].ToString()) > 0)
                        {
                            MessageBox.Show(cmbLevel2.Text.ToString() + " is used. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            DBClass.ExecuteNonQuery("delete from tbl_coa_level_2 where id = @id",
                            DBClass.CreateParameter("id", cmbLevel2.SelectedValue.ToString()));

                            MessageBox.Show(cmbLevel2.Text.ToString() + " deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EventHub.Refreshlvl4Account();
                        }
                    }
            }
        }

        private void lnkLvl3Delete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbLevel3.SelectedValue == null || cmbLevel3.Text == "")
            {
                MessageBox.Show("Select Level 3 First.");
                return;
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select count(*) from tbl_coa_level_4 where main_id = @id",
                DBClass.CreateParameter("id", cmbLevel3.SelectedValue.ToString())))
                    if (reader.Read())
                    {
                        if (int.Parse(reader[0].ToString()) > 0)
                        {
                            MessageBox.Show(cmbLevel3.Text.ToString() + " is used. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            DBClass.ExecuteNonQuery("delete from tbl_coa_level_3 where id = @id",
                            DBClass.CreateParameter("id", cmbLevel3.SelectedValue.ToString()));

                            MessageBox.Show(cmbLevel3.Text.ToString() + " deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            EventHub.Refreshlvl4Account();
                        }
                    }
            }
        }

        private void guna2GroupBox2_Click(object sender, EventArgs e)
        {

        }

        private void txtDebit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void txtCredit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.' && (sender as TextBox).Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void frmAddAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.lvl1Account -= lvl1;
            EventHub.lvl2Account -= lvl2;
            EventHub.lvl3Account -= lvl3;
        }


        private void lnlLvl2Add_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewLevel2(0, int.Parse(cmbLevel1.SelectedValue.ToString())).ShowDialog();
        }
    }
}
