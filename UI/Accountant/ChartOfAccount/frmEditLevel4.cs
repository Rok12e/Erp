using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmEditLevel4 : Form
    {
        private int id;
        public frmEditLevel4(int id)
        {
            InitializeComponent(); 
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtAccount.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Level 4 Name First");
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where name = @name",
                DBClass.CreateParameter("name", txtAccount.Text)))
                if (reader.Read())
                {
                    if ((id != int.Parse(reader["id"].ToString())))
                    {
                        MessageBox.Show("Name Already Exists. Enter Another Name.");
                        return;
                    }
                }
            DBClass.ExecuteNonQuery("update tbl_coa_level_4 set name=@name,debit=@debit,credit=@credit,date=@date where id = @id",
                DBClass.CreateParameter("id", id),
            DBClass.CreateParameter("name", txtAccount.Text),
                                               DBClass.CreateParameter("debit", txtDebit.Text.ToString()),
                                               DBClass.CreateParameter("credit", txtCredit.Text),
                                               DBClass.CreateParameter("date", dtOpen.Value));

            Utilities.LogAudit(frmLogin.userId, "Update Level 4 Account", "Level 4 Account", id, "Updated Level 4 Account: " + txtAccount.Text);
            if (!string.IsNullOrEmpty(txtCredit.Text.ToString().Trim()) || !string.IsNullOrEmpty(txtDebit.Text.ToString().Trim()))
            {
                if (txtCredit.Text == "")
                {
                    txtCredit.Text = "0";
                }
                if (txtDebit.Text == "")
                {
                    txtDebit.Text = "0";
                }
                insertTransactions(id);
            }

            EventHub.Refreshlvl4Account();
            this.Close();
        }
        private void insertTransactions(int refId)
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
            DBClass.ExecuteNonQuery("DELETE FROM tbl_transaction where transaction_id=@refId and t_type='GENERAL LEDGER OPENING BALANCE';", DBClass.CreateParameter("refId", refId));
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
        private void frmEditLevel4_Load(object sender, EventArgs e)
        {
            dtOpen.Value = DateTime.Now;
            if (id > 0)
            {
                btnDelete.Enabled = true;
                btnSave.Enabled = UserPermissions.canEdit("Chart Of Account");
            }
            else
            {
                btnDelete.Enabled = false;
            }
            btnDelete.Enabled = UserPermissions.canDelete("Chart Of Account");
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + id))
                if (reader.Read())
                {
                    txtAccount.Text = reader["name"].ToString();
                    txtDebit.Text = reader["debit"].ToString();
                    txtCredit.Text = reader["credit"].ToString();
                    if (reader["date"] != DBNull.Value)
                    {
                        dtOpen.Value = DateTime.Parse(reader["date"].ToString());
                    }
                }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtAccount.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Level 4 Name First");
                return;
            }
            object result = DBClass.ExecuteScalar(@"SELECT COUNT(1) FROM tbl_transaction 
                  WHERE type !='General Ledger Opening Balance' and account_id = @id", DBClass.CreateParameter("id", id));
            int recordCount = 0;
            if (result != null && result != DBNull.Value)
                recordCount = Convert.ToInt32(result);
            if (recordCount > 0)
            {
                MessageBox.Show("Already used");
                return;
            }
            DBClass.ExecuteNonQuery("delete from tbl_coa_level_4 where id = @id; delete from tbl_transaction WHERE type ='General Ledger Opening Balance' and transaction_id = @id",
            DBClass.CreateParameter("id", id));
            EventHub.Refreshlvl4Account();
            this.Close();
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

        private void txtDebit_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCredit_TextChanged(object sender, EventArgs e)
        {

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

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
