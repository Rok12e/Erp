using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmTransactionJournal : Form
    {
        int id;
        string vType = "";
        string refId = "";
        string date = "";
        string currenthumId = "", currentVoucherName = "", currentVoucherTable = "", currentVoucherId = "", currentVoucherDate = "";
        TransactionHelper helper = new TransactionHelper();
        public frmTransactionJournal(int id, string _vType = "", string _refId = "", string _date = "")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = this.Text;
            vType = _vType.Trim();
            refId = _refId.Trim();
            if (!string.IsNullOrEmpty(_date))
            {
                this.date = _date;
            }
            else
            {
                dtJV.Value = DateTime.Now;
                date = dtJV.Value.ToShortDateString();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmTransactionJournal_Load(object sender, EventArgs e)
        {
            
            LoaddGVAccounts();
            //BindCombos.PopulateAllLevel4Account(cmbCostCenter);
            //BindCombos.PopulateEmployees(cmbEmployee);
            //cmbCostCenter.SelectedIndex = -1;
            if (id != 0)
            {
                btnSave.Enabled = UserPermissions.canEdit("Transactions Journal");
                BindJournal();
            }
        }
        //private string GenerateNextJournalCode()
        //{
        //    string newCode = "JV-0001";

        //    using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)) AS lastCode FROM tbl_journal_voucher"))
        //    {
        //        if (reader.Read() && reader["lastCode"] != DBNull.Value)
        //        {
        //            int code = int.Parse(reader["lastCode"].ToString()) + 1;
        //            newCode = "JV-" + code.ToString("D4");
        //        }
        //    }

        //    return newCode;
        //}
        private void LoaddGVAccounts()
        {
            string query = @"SELECT CAST(code as CHAR) code,name FROM tbl_coa_level_4;";
            //if (vType == "PDC Payable")
            //{
            //    query = @"SELECT CAST(code as CHAR) code,name FROM tbl_coa_level_4
            //                UNION ALL 
            //                SELECT CAST(CODE AS CHAR) CODE,NAME FROM tbl_bank;";
            //}
            dgvJV.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable(query);
            DataGridViewComboBoxColumn account = (DataGridViewComboBoxColumn)dgvJV.Columns["account_name"];
            account.DataSource = dt;
            account.DisplayMember = "name";
            account.ValueMember = "code";

            //if (dgvJV.CurrentRow != null)
            //{
            //    DataGridViewCell buttonColumn = dgvJV.CurrentRow.Cells["partner_name"];
            //    if (buttonColumn.Value == null || buttonColumn.Value.ToString() == "")
            //    {
            //        buttonColumn.Value = "▼";
            //        //dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    }

            //    //loadNameList(buttonColumn);
            //}
        }
        //private void loadNameList(DataGridViewCell buttonColumn)
        //{
        //    string query = @"SELECT id,CONCAT(CODE ,' - ' , Name) as NAME FROM tbl_employee
        //                     UNION ALL
        //                     SELECT id,CONCAT(CODE ,' - ' , NAME) as NAME FROM tbl_vendor
        //                     UNION ALL
        //                     SELECT id,CONCAT(CODE,' - ', NAME) as NAME FROM tbl_customer";

        //    if (buttonColumn.Value == null || buttonColumn.Value.ToString() == "")
        //    {
        //        buttonColumn.Value = "▼";
        //        //dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //    }
        //    else if (buttonColumn.Value.ToString() == "▼")
        //    {
        //        //dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //    }
        //    else
        //    {
        //        dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        //    }
        //    loadDataToListView(query);
        //}
        private void loadDataToListView(string query)
        {
            //try
            //{
            //    DataTable dt = DBClass.ExecuteDataTable(query);

            //    nameslistView.Clear();
            //    nameslistView.View = View.Details;
            //    nameslistView.FullRowSelect = true;
            //    nameslistView.Columns.Add(" Name", 250);
            //    //ListViewItem newItem = new ListViewItem("<< Add New >>");
            //    //newItem.SubItems.Add("-1");
            //    //newItem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            //    //nameslistView.Items.Add(newItem);

            //    foreach (DataRow row in dt.Rows)
            //    {
            //        ListViewItem item = new ListViewItem(row["name"].ToString());
            //        item.Font = new Font("Times New Roman", 9F);
            //        item.SubItems.Add(row["id"].ToString());
            //        nameslistView.Items.Add(item);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.ToString();
            //}
        }
        private void BindJournal()
        {
            dgvJV.Rows.Clear();
            string comboQuery = "";
            int humId = 0;
            string query = "";
            if (vType == "Sales Invoice" || vType == "Sales Invoice Cash")
            {
                query = @"SELECT t.*, 
                                       ac.*, 
                                       ac.code AS `Account Code`,
                                       CASE 
                                           WHEN t.type IN ('Customer Receipt', 'Sales Invoice', 'Sales Invoice Cash', 'Customer Opening Balance', 'Check Cancel (Customer)', 'Customer%', 'Sales%') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_customer WHERE id = hum_id), '')
                                           WHEN t.type IN ('Vendor Payment', 'Purchase Invoice', 'Purchase Invoice Cash', 'Vendor Opening Balance', 'Check Cancel (Vendor)', 'Vendor%', 'Purchase%') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_vendor WHERE id = hum_id), '')
                                           WHEN t.type IN ('Employee Salary', 'Employee Payment') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_employee WHERE id = hum_id), '')
                                           ELSE '' 
                                       END AS `hum name`
                                FROM tbl_transaction t 
                                INNER JOIN tbl_coa_level_4 ac ON ac.id = t.account_id
                                WHERE (t.type = 'Sales Invoice Cash' or t.type = 'Sales Invoice') 
                                  AND t.transaction_id = @id;";
            }
            else if(vType == "Purchase Invoice" || vType == "Purchase Invoice Cash")
            {
                query = @"SELECT t.*, 
                                       ac.*, 
                                       ac.code AS `Account Code`,
                                       CASE 
                                           WHEN t.type IN ('Customer Receipt', 'Sales Invoice', 'Sales Invoice Cash', 'Customer Opening Balance', 'Check Cancel (Customer)', 'Customer%', 'Sales%') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_customer WHERE id = hum_id), '')
                                           WHEN t.type IN ('Vendor Payment', 'Purchase Invoice', 'Purchase Invoice Cash', 'Vendor Opening Balance', 'Check Cancel (Vendor)', 'Vendor%', 'Purchase%') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_vendor WHERE id = hum_id), '')
                                           WHEN t.type IN ('Employee Salary', 'Employee Payment') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_employee WHERE id = hum_id), '')
                                           ELSE '' 
                                       END AS `hum name`
                                FROM tbl_transaction t 
                                INNER JOIN tbl_coa_level_4 ac ON ac.id = t.account_id
                                WHERE (t.type = 'Purchase Invoice Cash' or t.type = 'Purchase Invoice')
                                  AND t.transaction_id = @id;";
            }
            else
            {
                query = @"SELECT t.*, 
                                       ac.*, 
                                       ac.code AS `Account Code`,
                                       CASE 
                                           WHEN t.type IN ('Customer Receipt', 'Sales Invoice', 'Sales Invoice Cash', 'Customer Opening Balance', 'Check Cancel (Customer)', 'Customer%', 'Sales%') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_customer WHERE id = hum_id), '')
                                           WHEN t.type IN ('Vendor Payment', 'Purchase Invoice', 'Purchase Invoice Cash', 'Vendor Opening Balance', 'Check Cancel (Vendor)', 'Vendor%', 'Purchase%') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_vendor WHERE id = hum_id), '')
                                           WHEN t.type IN ('Employee Salary', 'Employee Payment') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_employee WHERE id = hum_id), '')
                                           WHEN t.type IN ('SubContract Payment','Subcontractor Opening Balance','Check Cancel (SubContract)') 
                                                THEN IFNULL((SELECT CONCAT(CODE, ' - ', NAME) FROM tbl_vendor WHERE id = hum_id), '')
                                           ELSE '' 
                                       END AS `hum name`
                                FROM tbl_transaction t 
                                INNER JOIN tbl_coa_level_4 ac ON ac.id = t.account_id
                                WHERE t.TYPE = @type 
                                  AND t.transaction_id = @id and t.date=@date;";
            }
            MySqlDataReader dr_details = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", int.Parse(refId)), DBClass.CreateParameter("type", vType), DBClass.CreateParameter("date", DateTime.Parse(date)));

            int count = 1;
            while (dr_details.Read())
            {
                dtJV.Value = DateTime.Parse(dr_details["date"].ToString());
                string type = dr_details["type"].ToString();
                currentVoucherName = type;
                if (!string.IsNullOrEmpty(dr_details["transaction_id"].ToString()) && int.Parse(dr_details["transaction_id"].ToString()) > 0)
                {
                    txt_jv_code.Text = dr_details["transaction_id"].ToString();
                    currentVoucherId = txt_jv_code.Text;
                }
                if (!string.IsNullOrEmpty(dr_details["hum name"].ToString()))
                {
                    txtName.Text = dr_details["hum name"].ToString();
                    humId = string.IsNullOrEmpty(dr_details["hum_id"].ToString()) ? int.Parse(dr_details["hum_id"].ToString()) : 0;
                    
                    string tableName = "tbl_coa_level_4";
                    tableName = helper.IsCustomer(type) ? "tbl_customer" : helper.IsVendor(type) ? "tbl_vendor" : helper.IsEmployee(type) ? "tbl_employee" : tableName;
                    currenthumId = humId.ToString();
                    currentVoucherDate = dtJV.Value.ToShortDateString();
                    if (!string.IsNullOrEmpty(type))
                    {
                        comboQuery = @"SELECT CONCAT(CODE ,' - ' , NAME) as NAME ,id FROM " + tableName + "";
                    }

                    
                    currentVoucherTable = helper.IsTableName(type);
                }
                dgvJV.Rows.Add(dr_details["account_id"].ToString(), count.ToString(), dr_details["Account Code"].ToString(), dr_details["Account Code"].ToString(), Utilities.FormatDecimal(dr_details["Debit"]), Utilities.FormatDecimal(dr_details["Credit"]), dr_details["Description"].ToString(), dr_details["id"].ToString(), dr_details["hum_id"].ToString());
                count++;
            }
            dr_details.Dispose();
            //if (!string.IsNullOrEmpty(comboQuery))
            //{
            //    DataTable dt = DBClass.ExecuteDataTable(comboQuery);
            //    cmbName.ValueMember = "id";
            //    cmbName.DisplayMember = "name";
            //    cmbName.DataSource = dt;
            //}
            calculateTotal();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            calculateTotal();
            if (validateDebitAndCreditAmount())
            {
                if (id != 0)
                {
                    if (chkRequiredDate())
                    {
                        updateDetails();
                        EventHub.RefreshJournal();
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Debit Total and Credit Total are not equal.");
            }
        }
        private bool chkRequiredDate()
        {
            for (int i = 0; i < dgvJV.Rows.Count - 1; i++)
            {
                if (dgvJV.Rows[i].Cells["account_name"].Value == null
                    || dgvJV.Rows[i].Cells["account_name"].Value.ToString() == ""
                    || decimal.Parse(dgvJV.Rows[i].Cells["account_name"].Value.ToString()) == 0)
                {
                    MessageBox.Show("Total Item In Row " + (dgvJV.Rows[i].Index + 1) + " Can't Be 0 or Null");
                    return false;
                }
            }
            return true;
        }
        private void updateDetails()
        {
            decimal totalDebitAmount = 0, totalCreditAmount = 0;
            for (int i = 0; i < dgvJV.Rows.Count; i++)
            {
                if (dgvJV.Rows[i].Cells["account_id"].Value == null || dgvJV.Rows[i].Cells["account_id"].Value.ToString() == "")
                {
                    //
                }
                else
                {
                    if (dgvJV.Rows[i].Cells["Debit"].Value == null || dgvJV.Rows[i].Cells["Debit"].Value.ToString() == "" || dgvJV.Rows[i].Cells["Credit"].Value == null || dgvJV.Rows[i].Cells["Credit"].Value.ToString() == "")
                        continue;
                    decimal DebitAmount = dgvJV.Rows[i].Cells["Debit"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvJV.Rows[i].Cells["Debit"].Value);
                    decimal CreditAmount = dgvJV.Rows[i].Cells["Credit"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvJV.Rows[i].Cells["Credit"].Value);
                    string accountId = dgvJV.Rows[i].Cells["account_id"].Value.ToString();
                    string description = dgvJV.Rows[i].Cells["description"].Value?.ToString() ?? "";
                    string humId = dgvJV.Rows[i].Cells["hum_id"].Value?.ToString() ?? "0";
                    string tId = dgvJV.Rows[i].Cells["t_id"].Value?.ToString() ?? "0";
                    DateTime date = DateTime.Parse(dtJV.Value.Date.ToString());


                    DBClass.ExecuteNonQuery(@"UPDATE tbl_transaction 
                            SET date = @date, 
                                account_id = @accountId, 
                                debit = @debit, 
                                credit = @credit, 
                                transaction_id = @transactionId, 
                                hum_id =@hum_id,
                                type = @type,
                                description = @description, 
                                modified_by = @modifiedBy, 
                                modified_date = @modifiedDate
                            WHERE id = @journalId;",
                        DBClass.CreateParameter("@date", date),
                        DBClass.CreateParameter("@accountId", accountId),
                        DBClass.CreateParameter("@debit", DebitAmount),
                        DBClass.CreateParameter("@credit", CreditAmount),
                        DBClass.CreateParameter("@transactionId", id),
                        DBClass.CreateParameter("@hum_id", humId),
                        DBClass.CreateParameter("@type", vType),
                        DBClass.CreateParameter("@description", description),
                        DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                        DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date),
                        DBClass.CreateParameter("@journalId", tId)
                    );

                    totalDebitAmount += DebitAmount;
                    totalCreditAmount += CreditAmount;
                    string type = currentVoucherName, query = "";
                    if (type == ("Customer Opening Balance"))
                    {
                        var q = @"UPDATE tbl_customer SET account_id = @acId where id = @hId";
                        DBClass.ExecuteNonQuery(q, DBClass.CreateParameter("hId", humId), DBClass.CreateParameter("acId", accountId));
                    }
                    else if (type == ("Vendor Opening Balance"))
                    {
                        var q = @"UPDATE tbl_vendor SET account_id = @acId where id = @hId";
                        DBClass.ExecuteNonQuery(q, DBClass.CreateParameter("hId", humId), DBClass.CreateParameter("acId", accountId));
                    }
                    else if (type == ("Subcontractor Opening Balance"))
                    {
                        var q = @"UPDATE tbl_vendor SET account_id = @acId where id = @hId";
                        DBClass.ExecuteNonQuery(q, DBClass.CreateParameter("hId", humId), DBClass.CreateParameter("acId", accountId));
                    }
                    else if (type == ("Purchase Invoice Cash") || type == ("Purchase Invoice"))
                    {
                        //query = @"UPDATE tbl_purchase SET account_cash_id = '"+accountId+"' where vendor_id='"+humId+"'";
                    }
                    else if (type == ("Sales Invoice Cash") || type == ("Sales Invoice"))
                    {
                        //var q = @"UPDATE tbl_sales SET account_cash_id = '"+accountId+"' where customer_id='" + humId + "' and id='"+ id +"'";
                        //DBClass.ExecuteNonQuery(q, DBClass.CreateParameter("hId", humId), DBClass.CreateParameter("acId", accountId));
                    }
                    else if (type.StartsWith("SalesReturn"))
                    {
                        //query = "UPDATE tbl_sales_return SET";
                        //query = @"UPDATE tbl_sales_return SET account_cash_id = '" + accountId + "' where vendor_id='" + humId + "'";
                    }
                    else if (type.StartsWith("PurchaseReturn"))
                    {
                        //query = "UPDATE tbl_purchase_return SET";
                        //query = @"UPDATE tbl_purchase_return SET account_cash_id = '" + accountId + "' where vendor_id='" + humId + "'";
                    }
                    else if (type == ("Vendor Payment") || type == ("Employee Loan Payment") || type == "Employee Petty Cash Payment" || type == "Employee Salary Payment")
                    {
                        //query = "UPDATE tbl_payment_voucher SET";
                    }
                    else if (type == ("Petty Cash"))
                    {
                        //query = "UPDATE tbl_payment_voucher SET";
                    }
                    else if (type == "Customer Receipt" || type == "General Receipt")
                    {
                        //query = "UPDATE tbl_receipt_voucher SET";
                    }
                    else if (type == "SALES RETURN")
                    {
                        //query = "UPDATE tbl_sales_return SET";
                    }
                    else if (type == "PURCHASE RETURN")
                    {
                        //query = "UPDATE tbl_purchase_return SET";
                    }
                    else
                    {
                        query = "";
                        //if(vType == "General Ledger Opening Balance")
                        //{
                        //    var q = @"UPDATE tbl_coa_level_4 SET account_id = @acId where id = @hId";
                        //    DBClass.ExecuteNonQuery(q, DBClass.CreateParameter("hId", humId), DBClass.CreateParameter("acId", accountId));
                        //}
                    }

                    Utilities.LogAudit(frmLogin.userId, "Update Transaction Journal", "Transaction Journal", int.Parse(tId), "Updated Transaction Journal: " + description + " with Amount: " + DebitAmount + " and Account ID: " + accountId);
                }
            }
            if (totalDebitAmount >=0|| totalCreditAmount >=0)
            {
                string amount = totalDebitAmount > totalCreditAmount ? totalDebitAmount.ToString() : totalCreditAmount.ToString();
                updateVoucherTables(currentVoucherName,amount,currenthumId, currentVoucherId);
            }

        }

        private void updateVoucherTables(string type, string _amount, string _humId, string _id)
        {
            string query = "";
            if (type == ("Customer Opening Balance"))
            {
                query = @"UPDATE tbl_customer SET Balance='" + _amount + "' where id='" + _humId + "'";
            }
            else if (type == ("Vendor Opening Balance"))
            {
                query = @"UPDATE tbl_vendor SET Balance='" + _amount + "' where id='" + _humId + "'";
            }
            else if (type == ("Subcontractor Opening Balance"))
            {
                query = @"UPDATE tbl_vendor SET Balance='" + _amount + "' where id='" + _humId + "'";
            }
            else if (type == ("Purchase Invoice Cash") || type == ("Purchase Invoice"))
            {
                //query = @"UPDATE tbl_purchase SET account_cash_id=";
            }
            else if (type == ("Sales Invoice Cash") || type == ("Sales Invoice"))
            {
                //query = @"UPDATE tbl_sales SET";
            }
            else if (type.StartsWith("SalesReturn"))
            {
                //query = "UPDATE tbl_sales_return SET";
            }
            else if (type.StartsWith("PurchaseReturn"))
            {
                //query = "UPDATE tbl_purchase_return SET";
            }
            else if (type == ("Vendor Payment") || type == ("Employee Loan Payment") || type == "Employee Petty Cash Payment" || type == "Employee Salary Payment")
            {
                //query = "UPDATE tbl_payment_voucher SET";
            }
            else if (type == "Customer Receipt" || type == "General Receipt")
            {
                //query = "UPDATE tbl_receipt_voucher SET";
            }
            else if (type == "SALES RETURN")
            {
                //query = "UPDATE tbl_sales_return SET";
            }
            else if (type == "PURCHASE RETURN")
            {
                //query = "UPDATE tbl_purchase_return SET";
            }
            if (!string.IsNullOrEmpty(query))
            {
                DBClass.ExecuteNonQuery(query);
            }
            _id = string.IsNullOrEmpty(_id) ? "0" : _id;
            Utilities.LogAudit(frmLogin.userId, "Update Voucher Tables", "Transaction Journal", int.Parse(_id), "Updated Voucher Tables for Transaction Journal: " + type + " with Amount: " + _amount + " and Hum ID: " + _humId);
        }

        private void calculateTotal()
        {
            decimal totalDebit = 0, totalCredit = 0;
            for (int i = 0; i < dgvJV.Rows.Count; i++)
            {
                if (dgvJV.Columns.Contains("Debit") && dgvJV.Columns.Contains("Credit"))
                {
                    var debitColumnIndex = dgvJV.Columns["Debit"].Index;
                    var creditColumnIndex = dgvJV.Columns["Credit"].Index;
                    var debitCellValue = dgvJV.Rows[i].Cells[debitColumnIndex].Value;
                    var creditCellValue = dgvJV.Rows[i].Cells[creditColumnIndex].Value;
                    if (debitCellValue == DBNull.Value || debitCellValue == null || string.IsNullOrEmpty(debitCellValue.ToString().Trim()) || creditCellValue == DBNull.Value || creditCellValue == null || string.IsNullOrEmpty(creditCellValue.ToString().Trim()))
                    {
                        //
                    }
                    else
                    {

                        if (dgvJV.Rows[i].Cells["Debit"].Value.ToString() != "" || dgvJV.CurrentRow.Cells["Credit"].Value.ToString() != "")
                        {
                            totalDebit += decimal.Parse(dgvJV.Rows[i].Cells["Debit"].Value.ToString());
                            totalCredit += decimal.Parse(dgvJV.Rows[i].Cells["Credit"].Value.ToString());
                        }
                    }
                }
            }
            txtDebitAmount.Text = totalDebit.ToString();
            txtCreditAmount.Text = totalCredit.ToString();
            validateDebitAndCreditAmount();
        }
        private bool validateDebitAndCreditAmount()
        {
            if (!string.IsNullOrEmpty(txtDebitAmount.Text) && !string.IsNullOrEmpty(txtCreditAmount.Text))
            {
                decimal dr = decimal.Parse(txtDebitAmount.Text);
                decimal cr = decimal.Parse(txtCreditAmount.Text);
                if (dr == 0 && cr == 0)
                {
                    return false;
                }
                if (dr == cr)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        private void dgvJV_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvJV.Rows.Count > 1)
                {
                    var row = dgvJV.Rows[e.RowIndex];
                    if (e.ColumnIndex == dgvJV.Columns["account_code"].Index)
                    {
                        string codeValue = row.Cells["account_code"].Value?.ToString();
                        DataGridViewComboBoxCell comboCell = row.Cells["account_name"] as DataGridViewComboBoxCell;
                        if (comboCell != null)
                            insertAccountThroughCodeOrCombo("account_code", comboCell, null);
                    }
                    if (e.RowIndex >= 0)
                    {
                        if (e.ColumnIndex == dgvJV.Columns["Debit"].Index || e.ColumnIndex == dgvJV.Columns["Credit"].Index)
                        {
                            var debitColumnIndex = dgvJV.Columns["Debit"].Index;
                            var creditColumnIndex = dgvJV.Columns["Credit"].Index;
                            var debitCellValue = dgvJV.Rows[e.RowIndex].Cells[debitColumnIndex].Value;
                            var creditCellValue = dgvJV.Rows[e.RowIndex].Cells[creditColumnIndex].Value;


                            if (debitCellValue == DBNull.Value || debitCellValue == null || string.IsNullOrEmpty(debitCellValue.ToString().Trim()))
                            {
                                dgvJV.Rows[e.RowIndex].Cells[debitColumnIndex].Value = "0";
                            }
                            if (creditCellValue == DBNull.Value || creditCellValue == null || string.IsNullOrEmpty(creditCellValue.ToString().Trim()))
                            {
                                dgvJV.Rows[e.RowIndex].Cells[creditColumnIndex].Value = "0";
                            }
                            var debitValue = Convert.ToDecimal(dgvJV.Rows[e.RowIndex].Cells[debitColumnIndex].Value ?? 0);
                            var creditValue = Convert.ToDecimal(dgvJV.Rows[e.RowIndex].Cells[creditColumnIndex].Value ?? 0);

                            if (debitValue > 0 && creditValue > 0)
                            {
                                MessageBox.Show("You cannot enter a value in both Debit and Credit columns.");
                                dgvJV.Rows[e.RowIndex].Cells[creditColumnIndex].Value = DBNull.Value;
                            }
                            else if (creditValue > 0 && debitValue > 0)
                            {
                                MessageBox.Show("You cannot enter a value in both Debit and Credit columns.");
                                dgvJV.Rows[e.RowIndex].Cells[debitColumnIndex].Value = DBNull.Value;
                            }
                            calculateTotal();
                        }
                    }
                }
            } catch(Exception ex)
            {
                ex.ToString();
            }
        }
        private void dgvJV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (dgvJV.CurrentCell.ColumnIndex == dgvJV.Columns["account_name"].Index)
                {
                    if (comboBox != null)
                    {
                        comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxName_SelectedIndexChanged);
                        comboBox.SelectedIndexChanged += new EventHandler(ComboBoxName_SelectedIndexChanged);
                    }
                }
                else if (dgvJV.CurrentCell.ColumnIndex == dgvJV.Columns["Debit"].Index)
                {
                    var debitColumnIndex = dgvJV.Columns["Debit"].Index;

                    if (dgvJV.CurrentCell.ColumnIndex == debitColumnIndex)
                    {
                        e.Control.KeyPress -= new KeyPressEventHandler(ValidateDebitInput);
                        e.Control.KeyPress += new KeyPressEventHandler(ValidateDebitInput);
                    }
                }
                else if (dgvJV.CurrentCell.ColumnIndex == dgvJV.Columns["Credit"].Index)
                {
                    var creditColumnIndex = dgvJV.Columns["Credit"].Index;
                    if (dgvJV.CurrentCell.ColumnIndex == creditColumnIndex)
                    {
                        e.Control.KeyPress -= new KeyPressEventHandler(ValidateCreditInput);
                        e.Control.KeyPress += new KeyPressEventHandler(ValidateCreditInput);
                    }
                }
                else
                {
                    e.Control.KeyPress -= new KeyPressEventHandler(ValidateText);
                    e.Control.KeyPress += new KeyPressEventHandler(ValidateText);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertAccountThroughCodeOrCombo("combo", null, comboBox);
            }
        }
        private void ValidateText(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void ValidateDebitInput(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void ValidateCreditInput(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
        private void insertAccountThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;

            try
            {
                if (type == "account_code")
                {
                    string query = @"SELECT id,CAST(code as CHAR) code
                  FROM tbl_coa_level_4 
                  WHERE code = @code ";
                    //if (vType=="PDC Payable")
                    //{
                    //    query = @"SELECT id,CAST(code as CHAR) code
                    //  FROM tbl_bank 
                    //  WHERE code = @code ";
                    //}
                    reader = DBClass.ExecuteReader(query,
                        DBClass.CreateParameter("code", dgvJV.CurrentRow.Cells["account_code"].Value.ToString()));
                }
                else if (type == "combo" && comboBox.SelectedValue != null)
                {
                    string selectedAccountCode = comboBox.SelectedValue.ToString();
                    string query = @"SELECT id,CAST(code as CHAR) code FROM tbl_coa_level_4 WHERE code = @code";
                    //if (vType == "PDC Payable")
                    //{
                    //    query = @"SELECT id,CAST(code as CHAR) code FROM tbl_bank WHERE code = @code";
                    //}
                    reader = DBClass.ExecuteReader(query,
                        DBClass.CreateParameter("code", selectedAccountCode));
                }

                if (reader != null && reader.Read())
                {
                    dgvJV.CurrentRow.Cells["account_code"].Value = reader["code"].ToString();
                    dgvJV.CurrentRow.Cells["account_id"].Value = reader["id"].ToString();
                    if (type == "account_code" && comboCell != null)
                        comboCell.Value = dgvJV.CurrentRow.Cells["account_code"].Value.ToString();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
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

        private void Lbheader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void dgvJV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvJV.Rows.Count > 1 && dgvJV.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            //{
            //    dgvJV.Rows.Remove(dgvJV.CurrentRow);
            //    calculateTotal();
            //}
            //if (dgvJV.Rows.Count > 1 && dgvJV.CurrentRow.Cells["partner_name"].ColumnIndex == e.ColumnIndex)
            //{
            //    nameslistView.Visible = true;
            //    nameslistView.BringToFront();
            //    DataGridViewCell buttonColumn = dgvJV.CurrentRow.Cells["partner_name"];
            //    if (buttonColumn.Value == null || buttonColumn.Value.ToString() == "")
            //    {
            //        buttonColumn.Value = "▼";
            //        //dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    }
            //    loadNameList(buttonColumn);

            //    Rectangle cellRect = dgvJV.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            //    Point screenPoint = dgvJV.PointToScreen(new Point(cellRect.X, cellRect.Y + cellRect.Height - 160));
            //    Point formPoint = this.PointToClient(screenPoint);

            //    nameslistView.Location = formPoint;
            //    nameslistView.Width = 400;
            //    nameslistView.Height = 180;
            //    nameslistView.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            //}
        }

        private void dgvJV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvJV.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }

        private void cmbNameType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataGridViewCell buttonColumn = dgvJV.CurrentRow.Cells["partner_name"];
            //if (buttonColumn.Value == null)
            //{
            //    buttonColumn.Value = "▼";
            //    //dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //}
        }

        private void nameslistView_Click(object sender, EventArgs e)
        {
            //if (nameslistView.SelectedItems.Count > 0 && dgvJV.CurrentCell != null)
            //{
            //    string accountName = nameslistView.SelectedItems[0].SubItems[0].Text;
            //    //if (accountName == "<< Add New >>")
            //    //{
            //    //    //
            //    //}

            //    nameslistView.Visible = false;

            //    DataGridViewCell comboCell = dgvJV.CurrentRow.Cells["partner_name"];
            //    if (comboCell.Value != null)
            //    {
            //        comboCell.Value = accountName;
            //    }
            //    if (comboCell.Value.ToString() == "▼")
            //    {
            //        //dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //    }
            //    else
            //    {
            //        dgvJV.Columns["partner_name"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //    }
            //}
        }
    }
}

public class TransactionHelper
{
    public bool IsCustomer(string type)
    {
        if (type.Contains("Customer") ||
            type.Contains("Customer Receipt") ||
            type.Contains("Sales Invoice Cash") ||
            type.Contains("Sales Invoice") ||
            type.Contains("Customer Opening Balance") ||
            type.Contains("Check Cancel (Customer)") ||
            type.StartsWith("Sales"))
        {
            return true;
        }

        return false;
    }
    public bool IsVendor(string type)
    {
        if (type.Contains("Vendor") ||
            type.Contains("Vendor Payment") ||
            type.Contains("Purchase Invoice Cash") ||
            type.Contains("Purchase Invoice") ||
            (type.Contains("Vendor Opening Balance") && type.Contains("Vendor Opening Balance")) ||
            type.Contains("Check Cancel (Vendor)") ||
            type.StartsWith("Purchase"))
        {
            return true;
        }

        return false;
    }
    public bool IsEmployee(string type)
    {
        if (type.Contains("Employee") ||
            type.StartsWith("Employee") ||
            type.Contains("Employee Loan Payment") ||
            type.Contains("Employee Loan Payment") ||
            type.Contains("Employee Petty Cash Payment") ||
            type.Contains("Employee Salary Payment") ||
            type.Contains("Employee Salary")||
            type.Contains("Loan Request"))
        {
            return true;
        }

        return false;
    }
    public bool IsGeneral(string type)
    {
        if (type.Contains("General") ||
            type.StartsWith("General"))
        {
            return true;
        }

        return false;
    }

    public string IsTableName(string type)
    {
        if (type == ("Customer Opening Balance"))
        {
            return "tbl_customer";
        }
        else if (type == ("Vendor Opening Balance"))
        {
            return "tbl_vendor";
        }
        else if (type==("Purchase Invoice Cash") || type==("Purchase Invoice"))
        {
            return "tbl_purchase";
        }
        else if (type==("Sales Invoice Cash") || type==("Sales Invoice"))
        {
            return "tbl_sales";
        }
        else if (type.StartsWith("SalesReturn"))
        {
            return "tbl_sales_return";
        }
        else if (type.StartsWith("PurchaseReturn"))
        {
            return "tbl_purchase_return";
        }
        else if (type==("Vendor Payment")|| type== ("Employee Loan Payment")|| type== "Employee Petty Cash Payment"|| type == "Employee Salary Payment")
        {
            return "tbl_payment_voucher";
        }
        else if (type == "Customer Receipt" || type == "General Receipt")
        {
            return "tbl_receipt_voucher";
        }
        else if (type == "SALES RETURN")
        {
            return "tbl_sales_return";
        }
        else if (type == "PURCHASE RETURN")
        {
            return "tbl_purchase_return";
        }
        return "";
    }
}