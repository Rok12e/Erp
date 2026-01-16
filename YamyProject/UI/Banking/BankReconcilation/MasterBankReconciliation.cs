using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterBankReconciliation : Form
    {
        public MasterBankReconciliation()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            headerUC1.FormText = this.Text;
            dtOpen.Value = DateTime.Now;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void MasterBankReconciliation_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateAllLevel4Account(cmbBankName);
            loadBankBalance();
            loadData();
            loadBanklevel();
            BindBankCode();
        }
        private void BindBankCode()
        {
            MySqlDataReader reader = DBClass.ExecuteReader("SELECT code FROM tbl_coa_level_4 WHERE NAME=@name",
                DBClass.CreateParameter("name",cmbBankName.SelectedItem.ToString()));
            if (reader.Read())
            {
                txtCode.Text = reader["code"].ToString();
                cmbBankName.SelectedValue = reader["name"].ToString();
            }

        }
        private void loadData()
        {
            dgv_transactions.Columns.Add("SN", "SN#");
            dgv_transactions.Columns.Add("DATE", "DATE");
            dgv_transactions.Columns.Add("PVC_NO", "PVC NO");
            dgv_transactions.Columns.Add("NAME", "NAME");
            dgv_transactions.Columns.Add("DEBIT", "DEBIT");
            dgv_transactions.Columns.Add("CREDIT", "CREDIT");
            dgv_transactions.Columns.Add("BALANCE", "BALANCE");
            DataGridViewCheckBoxColumn chkColumn = new DataGridViewCheckBoxColumn();
            chkColumn.HeaderText = "T";
            chkColumn.Name = "status";
            dgv_transactions.Columns.Add(chkColumn);
            
            dgv_transactions.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Blue;
            dgv_transactions.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv_transactions.EnableHeadersVisualStyles = false;
            dgv_transactions.Columns["SN"].Width = 60;
            dgv_transactions.Columns["DATE"].Width = 110;
            dgv_transactions.Columns["PVC_NO"].Width = 90;
            dgv_transactions.Columns["NAME"].Width = 620;
            dgv_transactions.Columns["DEBIT"].Width = 120;
            dgv_transactions.Columns["CREDIT"].Width = 120;
            dgv_transactions.Columns["BALANCE"].Width = 150;
            LocalizationManager.LocalizeDataGridViewHeaders(dgv_transactions);

            dgv_book.Columns.Add("date", "DATE");
            dgv_book.Columns.Add("reference", "Reference");
            dgv_book.Columns.Add("description", "Description");
            LocalizationManager.LocalizeDataGridViewHeaders(dgv_book);

            dgv_reconsolation.Columns.Add("amount_debit", "Amount Debit");
            dgv_reconsolation.Columns.Add("amount_credit", "Amount Credit");
            dgv_reconsolation.Columns.Add("reconciliation_amount", "Reconciliation");
            LocalizationManager.LocalizeDataGridViewHeaders(dgv_reconsolation);

            dgv_bank_statement.Columns.Add("date", "DATE");
            dgv_bank_statement.Columns.Add("reference", "Reference");
            dgv_bank_statement.Columns.Add("description", "Description");
            LocalizationManager.LocalizeDataGridViewHeaders(dgv_bank_statement);

            loadBankTransactions();
        }

        private void loadBankTransactions()
        {
            decimal totalDebit = 0, totalCredit = 0;
            MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT ROW_NUMBER() OVER (ORDER BY X.id) sn,DATE_FORMAT(x.date,'%d/%m/%Y') date,x.transaction_id pv_no,x.name,x.debit,x.credit,FALSE status FROM
                (
                SELECT tt.id,tt.date,tt.transaction_id,t4.name,tt.debit,tt.credit FROM tbl_transaction tt INNER JOIN tbl_coa_level_4 t4 ON tt.account_id= t4.id 
                WHERE tt.transaction_id IN ((SELECT id FROM tbl_payment_voucher WHERE bank_account_id=@id),(SELECT id FROM tbl_receipt_voucher WHERE bank_account_id=@id)) AND t4.main_id =1 AND type ='BankReconciliation'
                )X
                UNION ALL 
                SELECT ROW_NUMBER() OVER (ORDER BY id) sn,DATE_FORMAT(date,'%d/%m/%Y') date,transaction_id pv_no,(SELECT NAME FROM tbl_coa_level_4 WHERE id=account_id)name,debit,credit,FALSE status from tbl_transaction where type ='BankReconciliation'", 
                DBClass.CreateParameter("id", cmbBankName.SelectedValue));
            while (reader.Read())
            {
                totalDebit += decimal.Parse(reader["debit"].ToString());
                totalCredit += decimal.Parse(reader["credit"].ToString());
                dgv_transactions.Rows.Add(reader["sn"], reader["date"], reader["pv_no"], reader["name"], reader["debit"], reader["credit"], (totalDebit-totalCredit).ToString(), (reader["status"].ToString()=="0"?false:true));
            }
            txt_total_debit_amount.Text = totalDebit.ToString();
            txt_total_credit_amount.Text = totalCredit.ToString();
            txt_total_amount_balance.Text = (totalDebit - totalCredit).ToString();

        }

        private void loadBanklevel()
        {
            dgv_bank_statement.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT id,name FROM tbl_coa_level_4 WHERE main_id IN (SELECT id FROM tbl_coa_level_3 WHERE NAME='Banks')");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgv_bank_statement.Columns["description"];
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "id";
        }
        private void loadBankBalance()
        {
            if (cmbBankName.Text.Trim() != "")
            {
                MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT DATE_FORMAT(CURDATE(), '%Y-%m-%d') dated,DATE_FORMAT(CURTIME(),'%H:%i:%s') time,sum(tt.debit) debit,sum(tt.credit) credit FROM tbl_transaction tt INNER JOIN tbl_coa_level_4 t4 ON tt.account_id= t4.id 
                WHERE tt.transaction_id IN ((SELECT id FROM tbl_payment_voucher WHERE bank_account_id=@id),(SELECT id FROM tbl_receipt_voucher WHERE bank_account_id=@id)) AND t4.main_id =1 AND tt.DATE<=@dated",
                   DBClass.CreateParameter("id", cmbBankName.SelectedValue?.ToString() ?? "0"),
                   DBClass.CreateParameter("dated", dtOpen.Value));
                if (reader.Read())
                {
                    txt_time.Text = reader["time"].ToString();
                    txt_bank_date.Text = reader["dated"].ToString();
                    txt_debit_amount.Text = reader["debit"].ToString();
                    txt_credit_amount.Text = reader["credit"].ToString();
                    if (!string.IsNullOrEmpty(txt_debit_amount.Text) && !string.IsNullOrEmpty(txt_debit_amount.Text))
                    {
                        txt_balance_amount.Text = (decimal.Parse(reader["debit"].ToString()) - decimal.Parse(reader["credit"].ToString())).ToString();
                    }
                    CalculateTotal();
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (insertData())
            {
                this.Close();
                MessageBox.Show("BankReconciliation Saved");
            }
        }
        private bool insertData()
        {
            MySqlDataReader dr = DBClass.ExecuteReader(@"select * from tbl_transaction where type='BankReconciliation'");
            if (dr.Read())
            {
                DBClass.ExecuteNonQuery(@"delete from tbl_transaction where type='BankReconciliation'");
            }
            for (int i = 0; i < dgv_bank_statement.Rows.Count; i++)
            {
                dgv_bank_statement.Rows[i].Cells["amount_debit"].Value = dgv_bank_statement.Rows[i].Cells["amount_debit"].Value != null ? decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_debit"].Value.ToString()) : 0;
                dgv_bank_statement.Rows[i].Cells["amount_credit"].Value = dgv_bank_statement.Rows[i].Cells["amount_credit"].Value != null ? decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_debit"].Value.ToString()) : 0;
                if (decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_debit"].Value.ToString()) > 0 || decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_credit"].Value.ToString()) > 0) {
                    int debit_account_id = 0;
                    int credit_account_id = 0;
                    decimal amount_dr = 0, amount_cr = 0;
                    if (decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_debit"].Value.ToString()) > 0)
                    {
                        debit_account_id = int.Parse(dgv_bank_statement.Rows[i].Cells["account_id"].Value.ToString());
                        credit_account_id = int.Parse(cmbBankName.SelectedValue.ToString());
                        amount_dr = decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_debit"].Value.ToString());
                        amount_cr = 0;
                    }
                    else if (decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_credit"].Value.ToString()) > 0)
                    {
                        debit_account_id = int.Parse(cmbBankName.SelectedValue.ToString());
                        credit_account_id = int.Parse(dgv_bank_statement.Rows[i].Cells["account_id"].Value.ToString());
                        amount_dr = 0;
                        amount_cr = decimal.Parse(dgv_bank_statement.Rows[i].Cells["amount_credit"].Value.ToString());
                    }
                    CommonInsert.InsertTransactionEntry(DateTime.Now.Date,
                           debit_account_id.ToString(),
                           amount_dr.ToString(), amount_cr.ToString(), i.ToString(), "0", "BankReconciliation", "BankReconciliation ref book. " + i,
                            frmLogin.userId, DateTime.Now.Date);
                    CommonInsert.InsertTransactionEntry(DateTime.Now.Date,
                                          credit_account_id.ToString(),
                                         amount_dr.ToString(), amount_cr.ToString(), i.ToString(), "0", "BankReconciliation", "BankReconciliation ref book. " + i,
                                           frmLogin.userId, DateTime.Now.Date);

                }
            }
            return true;
        }
        private void lnkNewCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //frmViewCustomerCategory frm = new frmViewCustomerCategory(_mainForm, this,0);
            //_mainForm.openChildForm(frm);
        }
        private void resetTextBox()
        {
            //txtAbbName.Text = txtEntId.Text = txtRoute.Text = txtCode.Text = "";
            //id = 0;
        }
        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            insertData();
            BindCombos.PopulateBanks(cmbBankName);
        }

        private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBankName.SelectedValue == null)
            {
                txtCode.Text = "";
                return;
            }
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where id = " + cmbBankName.SelectedValue.ToString());
            if (reader.Read())
            {
                txtCode.Text = reader["code"].ToString();
                //txtAbbName.Text = reader["abb_name"].ToString();
                //txtEntId.Text = reader["ent_id"].ToString();
                //txtRoute.Text = reader["route_num"].ToString();
                //cmbCountry.SelectedValue = reader["country_id"].ToString();
            }
            else
                txtCode.Text = "";
        }

        private void guna2GroupBox3_Click(object sender, EventArgs e)
        {

        }

        private void dgv_transactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgv_transactions.Columns["status"].Index)
            {
                if (dgv_transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || dgv_transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "true")
                {
                    dgv_transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "true";
                    addrowsTo();
                }
                else
                {
                    dgv_transactions.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "false";
                    addrowsTo();
                }
            }
        }
        private void addrowsTo()
        {
            dgv_book.Rows.Clear();
            dgv_reconsolation.Rows.Clear();
            decimal totalDebit = 0, totalCredit = 0;
            for (int i = 0; i < dgv_transactions.Rows.Count; i++)
            {
                if (dgv_transactions.Rows[i].Cells["status"].Value == null || dgv_transactions.Rows[i].Cells["status"].Value.ToString() == "true")
                {
                    dgv_book.Rows.Add(dgv_transactions.Rows[i].Cells["date"].Value, dgv_transactions.Rows[i].Cells[2].Value, dgv_transactions.Rows[i].Cells["name"].Value);
                    totalDebit += decimal.Parse(dgv_transactions.Rows[i].Cells["debit"].Value.ToString());
                    totalCredit += decimal.Parse(dgv_transactions.Rows[i].Cells["credit"].Value.ToString());
                    decimal balance = totalDebit-totalCredit;
                    dgv_reconsolation.Rows.Add(dgv_transactions.Rows[i].Cells["debit"].Value, dgv_transactions.Rows[i].Cells["credit"].Value,balance);
                }
            }
        }

        private void dgv_bank_statement_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_bank_statement.Rows.Count > 1)
            {
                var row = dgv_bank_statement.Rows[e.RowIndex];
                decimal amount_debit = GetDecimalValue(row, "amount_debit");
                decimal amount_credit = GetDecimalValue(row, "amount_credit");

                //else if (e.ColumnIndex == dgv_bank_statement.Columns["Code"].Index)
                //{
                //    string codeValue = row.Cells["Code"].Value?.ToString();
                //    DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                //    if (comboCell != null)
                //        insertItemThroughCodeOrCombo("code", comboCell, null);
                //}
                //else if (e.ColumnIndex == dgv_bank_statement.Columns["amount_credit"].Index)
                //{
                //    if (dgv_bank_statement.CurrentRow.Cells["itemId"].Value == null)
                //        row.Cells["amount_credit"].Value = 0;
                //}
                CalculateTotal();
            }
        }

        private void dgv_bank_statement_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgv_bank_statement.CurrentCell.ColumnIndex == dgv_bank_statement.Columns["description"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxAccount_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxAccount_SelectedIndexChanged);
                }
            }
            else if (dgv_bank_statement.CurrentCell.ColumnIndex == dgv_bank_statement.Columns["amount_debit"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxAmount_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxAccount_SelectedIndexChanged);
                }
            }
            else if (dgv_bank_statement.CurrentCell.ColumnIndex == dgv_bank_statement.Columns["amount_credit"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxAccount_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxAccount_SelectedIndexChanged);
                }
            }
        }
        private void ComboBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
        }

        private void ComboBoxAmount_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal amount_credit = GetDecimalValue(dgv_bank_statement.CurrentRow, "amount_debit");
            decimal amount_debit = GetDecimalValue(dgv_bank_statement.CurrentRow, "amount_credit");

            if (amount_debit == 0 || amount_credit == 0)
                dgv_bank_statement.CurrentRow.Cells["reconciliation_amount"].Value = (amount_debit - amount_credit).ToString();
            else
            {
                dgv_bank_statement.CurrentRow.Cells["amount_debit"].Value = 0;
                dgv_bank_statement.CurrentRow.Cells["amount_credit"].Value = 0;
                dgv_bank_statement.CurrentRow.Cells["reconciliation_amount"].Value = 0;
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
        private void insertItemThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;

            //if (type == "code")
            //{
            //    reader = DBClass.ExecuteReader(@"SELECT *
            //      FROM tbl_items 
            //      WHERE code = @code AND warehouse_id = @w",
            //        DBClass.CreateParameter("code", dgv_bank_statement.CurrentRow.Cells["code"].Value.ToString()),
            //        DBClass.CreateParameter("w", cmbWarehouse.SelectedValue.ToString()));
            //}
            if (type == "combo" && comboBox.SelectedValue != null)
            {
                //string selectedItemCode = comboBox.SelectedValue.ToString();
                //reader = DBClass.ExecuteReader(@"SELECT tbl_items.id,method,type, code,  sales_price,  cost_price 
                //  FROM tbl_items 
                //  WHERE warehouse_id = @id AND code = @code",
                //    DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()),
                //    DBClass.CreateParameter("code", selectedItemCode));
                dgv_bank_statement.CurrentRow.Cells["account_id"].Value = comboBox.SelectedValue;
            }

            //if (reader != null && reader.Read())
            //{
            //    dgv_bank_statement.CurrentRow.Cells["qty"].Value = 0;
            //    dgv_bank_statement.CurrentRow.Cells["itemid"].Value = reader["id"].ToString();
            //    dgv_bank_statement.CurrentRow.Cells["cost_price"].Value = reader["cost_price"].ToString();
            //    dgv_bank_statement.CurrentRow.Cells["price"].Value = Convert.ToDecimal(reader["sales_price"]);
            //    dgv_bank_statement.CurrentRow.Cells["method"].Value = reader["method"];
            //    if (type == "code" && comboCell != null)
            //        comboCell.Value = dgv_bank_statement.CurrentRow.Cells["code"].Value.ToString();
            //}
        }

        void CalculateTotal()
        {
            decimal total = 0;
            if (txt_balance_amount.ToString().Length>0)
            {
                total += decimal.Parse(txt_balance_amount.Text.ToString());
            }
            foreach (DataGridViewRow row in dgv_bank_statement.Rows)
            {
                if (row.Cells["amount_debit"].Value != null && row.Cells["amount_credit"].Value != null)
                {
                    total += Convert.ToDecimal(decimal.Parse(row.Cells["amount_debit"].Value.ToString()) - decimal.Parse(row.Cells["amount_credit"].Value.ToString()));
                }
                else if (row.Cells["amount_debit"].Value != null)
                {
                    total += Convert.ToDecimal(decimal.Parse(row.Cells["amount_debit"].Value.ToString()));
                }
                else if (row.Cells["amount_credit"].Value != null)
                {
                    total -= Convert.ToDecimal(decimal.Parse(row.Cells["amount_credit"].Value.ToString()));
                }
            }
            txt_total.Text = total.ToString("0.000");
        }

        private void dgv_bank_statement_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv_bank_statement.Rows.Count > 1 && dgv_bank_statement.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            {
                dgv_bank_statement.Rows.Remove(dgv_bank_statement.CurrentRow);
                CalculateTotal();
            }
        }
    }
}
