using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewPaymentVoucherAdvance : Form
    {
        int id, defaultCustomerId = 0;
        decimal totalAmount = 0;
        string code;
        public frmViewPaymentVoucherAdvance(int id = 0, int _customerId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = id == 0 ? "New - Advance Payment Voucher" : "Edit - Advance Payment Voucher";

            this.defaultCustomerId = _customerId;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewPaymentVoucherAdvance_Load(object sender, EventArgs e)
        {
            //dtOpen.Value = dtpTransDate.Value = dt_check_date.Value = DateTime.Now.Date;
            txtPVCode.Text = GenerateNextPaymentCode();
            BindCombos.PopulateCostCenter(cmbDebitCostCenter);
            BindCombos.PopulateCostCenter(cmbCreditCostCenter);
            BindCombos.PopulateAllLevel4Account(cmbDebitAccountName);
            BindCombos.PopulateAllLevel4Account(cmbCreditAccountName);
            //BindCombos.PopulateRegisterBanks(cmbBankName);
            cmbMethod.SelectedIndex = cmbPaymentType.SelectedIndex = 0;

            if (id > 0)
            {
                btnSave.Enabled = btnSaveNew.Enabled = UserPermissions.canEdit("Payment Voucher");
                btnDelete.Enabled = UserPermissions.canDelete("Payment Voucher"); //"Advance Payment Voucher";
                BindVoucher();
            }
            else
            {
                if (defaultCustomerId > 0)
                {
                    //cmbVendor.SelectedValue = defaultCustomerId;
                }
            }
        }
        private void BindVoucher()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_advance_payment_voucher WHERE id = " + id))
                if (reader.Read())
                {
                    // Header values
                    txtPVCode.Text = reader["pv_code"].ToString();
                    dtOpen.Value = DateTime.Parse(reader["date"].ToString());
                    //code = reader["code"].ToString();

                    // Combo selections
                    cmbPaymentType.Text = reader["type"].ToString();
                    cmbMethod.Text = reader["method"].ToString();
                    txtAmount.Text = reader["amount"].ToString();
                    txtDescription.Text = reader["description"].ToString();

                    cmbDebitAccountName.SelectedValue = Convert.ToInt32(reader["debit_account_id"]);
                    cmbDebitCostCenter.SelectedValue = Convert.ToInt32(reader["debit_cost_center_id"]);
                    cmbCreditAccountName.SelectedValue = Convert.ToInt32(reader["credit_account_id"]);
                    cmbCreditCostCenter.SelectedValue = Convert.ToInt32(reader["credit_cost_center_id"]);

                    if (cmbMethod.Text == "Check")
                    {

                    }

                    if (cmbPaymentType.Text == "Vendor")
                    {

                    }
                }

            DataTable dtDetails = DBClass.ExecuteDataTable(@"SELECT *, CASE WHEN a.`type`='Customer' THEN (
            SELECT CODE
            FROM tbl_customer
            WHERE id = d.name) WHEN a.`type`= 'Vendor' THEN(
            SELECT CODE
            FROM tbl_vendor
            WHERE id = d.name) WHEN a.`type`= 'Employee' THEN(
            SELECT CODE
            FROM tbl_employee
            WHERE id = d.name) ELSE '' END code
            FROM tbl_advance_payment_voucher_details d,tbl_advance_payment_voucher a
            WHERE a.id = d.payment_id AND d.payment_id = @id",
                DBClass.CreateParameter("id", id)
            );

            dgvPartner.Rows.Clear();

            foreach (DataRow dr in dtDetails.Rows)
            {
                int rowIndex = dgvPartner.Rows.Add();
                DataGridViewRow row = dgvPartner.Rows[rowIndex];

                row.Cells["partnerId"].Value = dr["id"];
                row.Cells["name"].Value = int.Parse(dr["code"].ToString());
                row.Cells["amount"].Value = dr["amount"];
                row.Cells["dgvdescription"].Value = dr["description"];

                if (cmbMethod.Text == "Check")
                {
                    row.Cells["bankname"].Value = dr["bank_name"];
                    row.Cells["checkname"].Value = dr["check_name"];
                    row.Cells["checknumber"].Value = dr["check_no"];
                    row.Cells["checkdate"].Value = dr["check_date"] != DBNull.Value ? Convert.ToDateTime(dr["check_date"]).ToString("yyyy-MM-dd") : null;
                    row.Cells["bankaccountname"].Value = dr["bank_account_name"];
                    row.Cells["bookno"].Value = dr["book_no"];
                }
                else if (cmbMethod.Text == "Transfer")
                {
                    row.Cells["transdate"].Value = dr["trans_date"] != DBNull.Value ? Convert.ToDateTime(dr["trans_date"]).ToString("yyyy-MM-dd") : null;
                    row.Cells["transname"].Value = dr["trans_name"];
                    row.Cells["transref"].Value = dr["trans_ref"];
                }
            }
        }

        private string GenerateNextPaymentCode()
        {
            string newCode = "AP-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(pv_code, 4) AS UNSIGNED)) AS lastCode FROM tbl_advance_payment_voucher"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "AP-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertPV())
                {

                    MessageBox.Show("The Advance Payment  Paid  ");
                    dgvPartner.Rows.Clear();
                    txtPVCode.Text = GenerateNextPaymentCode();
                    EventHub.RefreshPaymentVoucher();
                    this.Close();
                }
            }
            else
            {
                if (updatePV())
                {
                    MessageBox.Show("The Advance Payment  Paid  ");
                    dgvPartner.Rows.Clear();
                    txtPVCode.Text = GenerateNextPaymentCode();
                    EventHub.RefreshPaymentVoucher();
                    this.Close();
                }
            }
        }
        private bool updatePV()
        {
            if (!chkRequireData())
                return false;
            DBClass.ExecuteNonQuery(@"
        UPDATE tbl_advance_payment_voucher 
        SET 
            date = @date,
            pv_code = @pv_code,
            type = @type,
            method = @method,
            amount = @amount,
            debit_account_id = @debit_account_id,
            debit_cost_center_id = @debit_cost_center_id,
            description = @description,
            credit_account_id = @credit_account_id,
            credit_cost_center_id = @credit_cost_center_id,
            modified_by = @modified_by,
            modified_date = @modified_date
        WHERE id = @id",
                DBClass.CreateParameter("id", id),
                DBClass.CreateParameter("date", dtOpen.Value.Date),
                DBClass.CreateParameter("pv_code", code),
                DBClass.CreateParameter("type", cmbPaymentType.Text),
                DBClass.CreateParameter("method", cmbMethod.Text),
                DBClass.CreateParameter("amount", txtAmount.Text),
                DBClass.CreateParameter("debit_account_id", cmbDebitAccountName.SelectedValue ?? 0),
                DBClass.CreateParameter("debit_cost_center_id", cmbDebitCostCenter.SelectedValue ?? 0),
                DBClass.CreateParameter("description", txtDescription.Text),
                DBClass.CreateParameter("credit_account_id", cmbCreditAccountName.SelectedValue ?? 0),
                DBClass.CreateParameter("credit_cost_center_id", cmbCreditCostCenter.SelectedValue ?? 0),
                DBClass.CreateParameter("modified_by", frmLogin.userId),
                DBClass.CreateParameter("modified_date", DateTime.Now.Date));
            DBClass.ExecuteNonQuery("Delete from tbl_advance_payment_voucher_details where payment_id=@id", DBClass.CreateParameter("id", id));
            CommonInsert.DeleteTransactionEntry(id, "Advance PAYMENT");

            if (cmbMethod.Text == "Check")
            {
                foreach (DataGridViewRow row in dgvPartner.Rows)
                {
                    if (row.IsNewRow || row.Cells["checknumber"].Value == null)
                        continue;

                    string checkNo = row.Cells["checknumber"].Value?.ToString();
                    string checkName = row.Cells["checkname"].Value?.ToString();
                    string bookNo = row.Cells["bookno"].Value?.ToString();
                    string checkDateStr = row.Cells["checkdate"].Value?.ToString();

                    DateTime checkDate = DateTime.Now.Date;
                    DateTime.TryParse(checkDateStr, out checkDate);
                    int bookId = 0;
                    int.TryParse(bookNo, out bookId);

                    object result = DBClass.ExecuteScalar(@"
                SELECT COUNT(1) FROM tbl_check_details 
                WHERE pvc_no = @pvc_no AND check_type = @check_type AND check_id = @check_id",
                        DBClass.CreateParameter("pvc_no", id),
                        DBClass.CreateParameter("check_type", "Payment"),
                        DBClass.CreateParameter("check_id", bookId)
                    );

                    int recordCount = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;

                    if (recordCount > 0)
                    {
                        DBClass.ExecuteNonQuery(@"
                    UPDATE tbl_check_details
                    SET 
                        check_no = @check_no,
                        check_date = @check_date,
                        check_name = @check_name,
                        amount = @amount
                    WHERE pvc_no = @pvc_no AND check_type = @check_type AND check_id = @check_id",
                            DBClass.CreateParameter("check_no", checkNo),
                            DBClass.CreateParameter("check_date", checkDate),
                            DBClass.CreateParameter("check_name", checkName),
                            DBClass.CreateParameter("amount", row.Cells["amount"].Value?.ToString() ?? "0"),
                            DBClass.CreateParameter("pvc_no", id),
                            DBClass.CreateParameter("check_type", "Payment"),
                            DBClass.CreateParameter("check_id", bookId)
                        );
                    }
                    else
                    {
                        DBClass.ExecuteNonQuery(@"
                    INSERT INTO tbl_check_details
                    (date, check_id, check_no, check_date, check_type, pvc_no, check_name, amount, state)
                    VALUES
                    (@date, @check_id, @check_no, @check_date, @check_type, @pvc_no, @check_name, @amount, @state)",
                            DBClass.CreateParameter("date", dtOpen.Value.Date),
                            DBClass.CreateParameter("check_id", bookId),
                            DBClass.CreateParameter("check_no", checkNo),
                            DBClass.CreateParameter("check_date", checkDate),
                            DBClass.CreateParameter("check_type", "Payment"),
                            DBClass.CreateParameter("pvc_no", id),
                            DBClass.CreateParameter("check_name", checkName),
                            DBClass.CreateParameter("amount", row.Cells["amount"].Value?.ToString() ?? "0"),
                            DBClass.CreateParameter("state", "New")
                        );
                    }
                }
            }
            else if (cmbMethod.Text == "Transfer")
            {

            }
            insertINV(id);
            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "AdvancePayment");
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, txtAmount.Text.ToString(), "0", id.ToString(), "AdvancePayment", "Advance Payment Debit Entry", cmbDebitCostCenter.SelectedValue.ToString());
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, "0", txtAmount.Text.ToString(), id.ToString(), "AdvancePayment", "Advance Payment Credit Entry", cmbCreditCostCenter.SelectedValue.ToString());
            Utilities.LogAudit(frmLogin.userId, "Update Advance Payment Voucher", "Advance Payment Voucher", id, "Updated Advance Payment Voucher: " + code);

            return true;
        }
        private bool insertPV()
        {
            if (!chkRequireData())
                return false;

            code = GenerateNextPaymentCode();

            id = (int)decimal.Parse(DBClass.ExecuteScalar(@"
                INSERT INTO tbl_advance_payment_voucher
                (date, pv_code, type, method, amount, debit_account_id, debit_cost_center_id,
                 description, credit_account_id, credit_cost_center_id, created_by, created_date, state)
                VALUES
                (@date, @pv_code, @type, @method, @amount, @debit_account_id, @debit_cost_center_id,
                 @description, @credit_account_id, @credit_cost_center_id, @created_by, @created_date, 0);
                 SELECT LAST_INSERT_ID();",

                DBClass.CreateParameter("date", dtOpen.Value.Date),
                DBClass.CreateParameter("pv_code", code),
                DBClass.CreateParameter("type", cmbPaymentType.Text),
                DBClass.CreateParameter("method", cmbMethod.Text),
                DBClass.CreateParameter("amount", txtAmount.Text),
                DBClass.CreateParameter("debit_account_id", cmbDebitAccountName.SelectedValue ?? 0),
                DBClass.CreateParameter("debit_cost_center_id", cmbDebitCostCenter.SelectedValue ?? 0),
                DBClass.CreateParameter("description", txtDescription.Text),
                DBClass.CreateParameter("credit_account_id", cmbCreditAccountName.SelectedValue ?? 0),
                DBClass.CreateParameter("credit_cost_center_id", cmbCreditCostCenter.SelectedValue ?? 0),
                DBClass.CreateParameter("created_by", frmLogin.userId),
                DBClass.CreateParameter("created_date", DateTime.Now.Date)
            ).ToString());
            if (cmbMethod.Text == "Check")
            {
                for (int i = 0; i < dgvPartner.Rows.Count; i++)
                {
                    DataGridViewRow row = dgvPartner.Rows[i];
                    if (row.IsNewRow || row.Cells["name"].Value == null)
                        continue;

                    string checkNo = row.Cells["checknumber"].Value?.ToString();
                    string checkName = row.Cells["checkname"].Value?.ToString();
                    string bookNo = row.Cells["bookno"].Value?.ToString();
                    string checkDate = Convert.ToDateTime(row.Cells["checkdate"].Value)
                                        .ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                    int bookId = 0;
                    int.TryParse(bookNo, out bookId);

                    if (!string.IsNullOrEmpty(checkNo))
                    {
                        DBClass.ExecuteNonQuery(@"INSERT INTO tbl_check_details
                            (date, check_id, check_no, check_date, check_type, pvc_no, check_name, amount, state)
                            VALUES
                            (@date, @check_id, @check_no, @check_date, @check_type, @pvc_no, @check_name, @amount, @state)",
                            DBClass.CreateParameter("date", dtOpen.Value.Date),
                            DBClass.CreateParameter("check_id", bookId),
                            DBClass.CreateParameter("check_no", checkNo),
                            DBClass.CreateParameter("check_date", checkDate),
                            DBClass.CreateParameter("check_type", "Advance Payment"),
                            DBClass.CreateParameter("pvc_no", id),
                            DBClass.CreateParameter("check_name", checkName),
                            DBClass.CreateParameter("amount", row.Cells["amount"].Value?.ToString() ?? "0"),
                            DBClass.CreateParameter("state", "New")
                        );
                    }
                }
            }
            insertINV(id);
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, txtAmount.Text.ToString(), "0", invId.ToString(), "AdvancePayment", "Advance Payment Debit Entry", cmbDebitCostCenter.SelectedValue.ToString());
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, "0", txtAmount.Text.ToString(), invId.ToString(), "AdvancePayment", "Advance Payment Credit Entry", cmbCreditCostCenter.SelectedValue.ToString());
            Utilities.LogAudit(frmLogin.userId, "Insert Advance Payment Voucher", "Advance Payment Voucher", id, "Inserted Advance Payment Voucher: " + code);
            return true;
        }
        private void insertINV(int pvId)
        {
            for (int i = 0; i < dgvPartner.Rows.Count; i++)
            {
                var row = dgvPartner.Rows[i];
                if (row.IsNewRow || row.Cells["name"].Value == null)
                    continue;

                string method = cmbMethod.Text;
                string name = row.Cells["partnerId"].Value?.ToString() ?? "";
                string bankName = row.Cells["bankname"].Value?.ToString();
                string checkName = row.Cells["checkname"].Value?.ToString();
                string checkNo = row.Cells["checknumber"].Value?.ToString();
                string checkDateStr = row.Cells["checkdate"].Value?.ToString();
                string bankAccountName = row.Cells["bankaccountname"].Value?.ToString();
                string bookNo = row.Cells["bookno"].Value?.ToString();
                string transDateStr = row.Cells["transdate"].Value?.ToString();
                string transName = row.Cells["transname"].Value?.ToString();
                string transRef = row.Cells["transref"].Value?.ToString();
                string description = row.Cells["dgvdescription"].Value?.ToString() ?? "";
                string amount = row.Cells["amount"].Value?.ToString();
                string humId = name;

                DateTime? checkDate = null, transDate = null;
                DateTime parsedCheck;
                if (DateTime.TryParse(checkDateStr, out parsedCheck))
                    checkDate = parsedCheck;
                DateTime parsedTrans;
                if (DateTime.TryParse(transDateStr, out parsedTrans))
                    transDate = parsedTrans;

                // Insert into details table
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_advance_payment_voucher_details
        (payment_id,name, bank_name, check_name, check_no, check_date, bank_account_name, book_no, 
         trans_date, trans_name, trans_ref, description, amount) 
         VALUES 
        (@payment_id,@name, @bank_name, @check_name, @check_no, @check_date, @bank_account_name, @book_no, 
         @trans_date, @trans_name, @trans_ref, @description, @amount);",
                    DBClass.CreateParameter("@payment_id",pvId),
                    DBClass.CreateParameter("@name", name),
                    DBClass.CreateParameter("@bank_name", method == "Check" ? bankName : null),
                    DBClass.CreateParameter("@check_name", method == "Check" ? checkName : null),
                    DBClass.CreateParameter("@check_no", method == "Check" && !string.IsNullOrEmpty(checkNo) ? (object)int.Parse(checkNo) : null),
                    DBClass.CreateParameter("@check_date", method == "Check" ? checkDate : null),
                    DBClass.CreateParameter("@bank_account_name", method == "Check" ? bankAccountName : null),
                    DBClass.CreateParameter("@book_no", method == "Check" && !string.IsNullOrEmpty(bookNo) ? (object)int.Parse(bookNo) : null),
                    DBClass.CreateParameter("@trans_date", method == "Transfer" ? transDate : null),
                    DBClass.CreateParameter("@trans_name", method == "Transfer" ? transName : null),
                    DBClass.CreateParameter("@trans_ref", method == "Transfer" ? transRef : null),
                    DBClass.CreateParameter("@description", description),
                    DBClass.CreateParameter("@amount", amount)
                );

                insertJournals(cmbPaymentType.Text, amount, description, humId);
            }
        }
        void insertJournals(string type, string amount, string description, string humId)
        {
            string tType = "";
            if (cmbPaymentType.Text == "Vendor")
            {
                tType = "Vendor Advance Payment";
            }
            else if (cmbPaymentType.Text == "Employee")
            {
                tType = "Employee Advance Payment";
            }
            else if (cmbPaymentType.Text == "Customer")
            {
                tType = "Customer Advance Payment";
            }
            CommonInsert.addTransactionEntry(dtOpen.Value.Date,
                       cmbDebitAccountName.SelectedValue.ToString(),
                       amount, "0", id.ToString(), humId, tType, "Advance PAYMENT", "ADVANCE Payment Voucher NO. " + code,
                       frmLogin.userId, DateTime.Now.Date, txtPVCode.Text);

            CommonInsert.addTransactionEntry(dtOpen.Value.Date,
                    cmbCreditAccountName.SelectedValue.ToString(),
                    "0", amount, id.ToString(), "0", tType, "Advance PAYMENT", "ADVANCE Payment Voucher NO. " + code,
                    frmLogin.userId, DateTime.Now.Date, txtPVCode.Text);
        }
        private bool chkRequireData()
        {
            if (cmbPaymentType.Text.Trim() == "")
            {
                MessageBox.Show("Please Choose Payment Type.");
                return false;
            }
            if (txtAmount.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Payment Amount.");
                return false;
            }
            if (cmbDebitAccountName.Text.Trim() == "")
            {
                MessageBox.Show("Please Choose Debit Account Name");
                return false;
            }
            if (cmbCreditAccountName.Text.Trim() == "")
            {
                MessageBox.Show("Please Choose Credit Account Name");
                return false;
            }
            if ((txtAmount.Text == "" || decimal.Parse(txtAmount.Text) == 0))
            {
                MessageBox.Show("Enter Amount");
                return false;
            }
            return true;
        }
        private void LoadVendor()
        {
            dgvPartner.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select id,code,name from tbl_vendor where state = 0");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvPartner.Columns["name"];
            DataRow newRow = dt.NewRow();
            newRow["code"] = 0;
            newRow["name"] = "<< Add >>";
            dt.Rows.InsertAt(newRow, 0);
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Vendor")))
                if (reader.Read())
                {
                    txtDebitCode.Text = reader["code"].ToString();
                    string accountId = reader["id"].ToString();
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        cmbDebitAccountName.SelectedValue = int.Parse(accountId);
                    }
                }
        }
        private void LoadCustomer()
        {
            dgvPartner.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select id,code,name from tbl_customer where state = 0");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvPartner.Columns["name"];
            DataRow newRow = dt.NewRow();
            newRow["code"] = 0;
            newRow["name"] = "<< Add >>";
            dt.Rows.InsertAt(newRow, 0);
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Customer")))
                if (reader.Read())
                {
                    txtDebitCode.Text = reader["code"].ToString();
                    string accountId = reader["id"].ToString();
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        cmbDebitAccountName.SelectedValue = int.Parse(accountId);
                    }
                }
        }
        private void LoadEmployee()
        {
            dgvPartner.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select id,code,name from tbl_employee where state = 0");
            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvPartner.Columns["name"];
            DataRow newRow = dt.NewRow();
            newRow["code"] = 0;
            newRow["name"] = "<< Add >>";
            dt.Rows.InsertAt(newRow, 0);
            name.DataSource = dt;
            name.DisplayMember = "name";
            name.ValueMember = "code";
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Accrued Salaries")))
                if (reader.Read())
                {
                    txtDebitCode.Text = reader["code"].ToString();
                    string accountId = reader["id"].ToString();
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        cmbDebitAccountName.SelectedValue = int.Parse(accountId);
                    }
                }
        }
        private void cmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentType.Text == "Vendor")
            {
                LoadVendor();
            }
            else if (cmbPaymentType.Text == "Customer")
            {
                LoadCustomer();
            }
            else if (cmbPaymentType.Text == "Employee")
            {
                LoadEmployee();
            }
        }
        private void cmbDebitAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDebitAccountName.SelectedValue == null)
            {
                txtDebitCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbDebitAccountName.SelectedValue.ToString()))
                if (reader.Read())
                    txtDebitCode.Text = reader["code"].ToString();
                else
                    txtDebitCode.Text = "";
        }
        private void cmbCreditAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCreditAccountName.SelectedValue == null)
            {
                txtCreditAccountCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbCreditAccountName.SelectedValue.ToString()))
                if (reader.Read())
                    txtCreditAccountCode.Text = reader["code"].ToString();
                else
                    txtCreditAccountCode.Text = "";
        }
        private void txtDebitCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", txtDebitCode.Text)))
                if (reader.Read())
                    cmbDebitAccountName.SelectedValue = int.Parse(reader["id"].ToString());
        }
        private void txtCreditAccountCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code", DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (reader.Read())
                    cmbCreditAccountName.SelectedValue = int.Parse(reader["id"].ToString());
        }
        private void txtDebitCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code", DBClass.CreateParameter("code", txtDebitCode.Text)))
                if (!reader.Read())
                    cmbDebitAccountName.SelectedIndex = -1;
        }
        private void txtCreditAccountCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code", DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (!reader.Read())
                    cmbCreditAccountName.SelectedIndex = -1;
        }
        private void BankColumns(bool visible)
        {
            dgvPartner.Columns["bankname"].Visible = visible;
            dgvPartner.Columns["checkname"].Visible = visible;
            dgvPartner.Columns["checknumber"].Visible = visible;
            dgvPartner.Columns["checkdate"].Visible = visible;
            dgvPartner.Columns["bankaccountname"].Visible = visible;
            dgvPartner.Columns["bookno"].Visible = visible;
        }
        private void TransColumns(bool visible)
        {
            dgvPartner.Columns["transdate"].Visible = visible;
            dgvPartner.Columns["transname"].Visible = visible;
            dgvPartner.Columns["transref"].Visible = visible;
        }
        private void cmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMethod.Text == "Cash")
            {
                BankColumns(false);
                TransColumns(false);
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Default Account For Cash")))
                    if (reader.Read())
                    {
                        txtCreditAccountCode.Text = reader["code"].ToString();
                        string accountId = reader["id"].ToString();
                        if (!string.IsNullOrEmpty(accountId))
                        {
                            cmbCreditAccountName.SelectedValue = int.Parse(accountId);
                        }
                    }
            }
            else if (cmbMethod.Text == "Check")
            {
                BankColumns(true);
                TransColumns(false);
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "PDC Payable")))
                    if (reader.Read())
                    {
                        txtCreditAccountCode.Text = reader["code"].ToString();
                        string accountId = reader["id"].ToString();
                        if (!string.IsNullOrEmpty(accountId))
                        {
                            cmbCreditAccountName.SelectedValue = int.Parse(accountId);
                        }
                    }
                var bankColumn = dgvPartner.Columns["bankname"] as DataGridViewComboBoxColumn;
                if (bankColumn != null)
                {
                    BindCombos.PopulateDGVRegisterBanks(bankColumn);
                }
            }
            else if (cmbMethod.Text == "Transfer")
            {
                BankColumns(false);
                TransColumns(true);
            }
        }
        private void calculateTotal()
        {
            txtAmount.Text = totalAmount.ToString();
            totalAmount = 0;
        }

        private void dgvPartner_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dgvPartner.Columns["account_code"].Index)
            {
                var row = dgvPartner.Rows[e.RowIndex];
                string codeValue = row.Cells["account_code"].Value?.ToString();
                DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                if (comboCell != null)
                    insertAccountThroughCodeOrCombo("account_code", comboCell, null);
            }
            if (e.RowIndex >= 0 && dgvPartner.Columns[e.ColumnIndex].Name == "name")
            {
                DataGridViewComboBoxCell comboCell = dgvPartner.Rows[e.RowIndex].Cells["name"] as DataGridViewComboBoxCell;
                if (comboCell != null && comboCell.Value != null)
                {
                    string selectedCode = comboCell.Value.ToString();

                    DataGridViewComboBoxColumn vendorColumn = dgvPartner.Columns["name"] as DataGridViewComboBoxColumn;
                    if (vendorColumn != null)
                    {
                        DataTable dt = vendorColumn.DataSource as DataTable;
                        if (dt != null)
                        {
                            DataRow[] match = dt.Select("code = '" + selectedCode + "'");
                            if (match.Length > 0)
                            {
                                dgvPartner.Rows[e.RowIndex].Cells["partnerId"].Value = match[0]["id"].ToString();
                            }
                        }
                    }
                }
            }
            // ✅ When bank name is selected
            if (dgvPartner.Columns[e.ColumnIndex].Name == "bankname")
            {
                string bankId = dgvPartner.Rows[e.RowIndex].Cells["bankname"].Value?.ToString();
                if (!string.IsNullOrEmpty(bankId))
                {
                    // 1. Load first related bank account
                    DataTable dtAccounts = DBClass.ExecuteDataTable("SELECT id, account_name FROM tbl_bank_card WHERE bank_id = @bank_id",
                        DBClass.CreateParameter("bank_id", bankId));

                    if (dtAccounts.Rows.Count > 0)
                    {
                        string accountName = dtAccounts.Rows[0]["name"].ToString();
                        dgvPartner.Rows[e.RowIndex].Cells["bankaccountname"].Value = accountName;
                    }

                    // 2. Load first cheque book from that bank's card
                    MySqlDataReader bookReader = DBClass.ExecuteReader(@"
                SELECT id, chq_book_no 
                FROM tbl_cheque 
                WHERE bank_card_id = (
                    SELECT id FROM tbl_bank_card WHERE bank_id = @bank_id LIMIT 1
                )
                LIMIT 1", DBClass.CreateParameter("bank_id", bankId));

                    if (bookReader.Read())
                    {
                        string bookId = bookReader["id"].ToString();
                        string bookNo = bookReader["chq_book_no"].ToString();
                        bookReader.Close();

                        dgvPartner.Rows[e.RowIndex].Cells["bookno"].Value = bookNo;

                        // 3. Get range
                        using (MySqlDataReader rangeReader = DBClass.ExecuteReader("SELECT leaves_start_from, leaves_end_in FROM tbl_cheque WHERE id = @id",
                            DBClass.CreateParameter("id", bookId)))
                            if (rangeReader.Read())
                            {
                                string start = rangeReader["leaves_start_from"].ToString();
                                string end = rangeReader["leaves_end_in"].ToString();

                                // 4. Get used check numbers
                                DataTable dtUsed = DBClass.ExecuteDataTable("SELECT check_no FROM tbl_check_details WHERE check_id = @id AND check_no IS NOT NULL",
                                    DBClass.CreateParameter("id", bookId));

                                List<int> usedNumbers = new List<int>();
                                foreach (DataRow row in dtUsed.Rows)
                                {
                                    int parsed;
                                    if (int.TryParse(row["check_no"].ToString(), out parsed))
                                    {
                                        usedNumbers.Add(parsed);
                                    }

                                    int startNum, endNum;
                                    if (int.TryParse(start, out startNum) && int.TryParse(end, out endNum))
                                    {
                                        for (int i = startNum; i <= endNum; i++)
                                        {
                                            if (!usedNumbers.Contains(i))
                                            {
                                                dgvPartner.Rows[e.RowIndex].Cells["checknumber"].Value = i.ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                dgvPartner.Rows[e.RowIndex].Cells["bookno"].Value = null;
                                dgvPartner.Rows[e.RowIndex].Cells["checknumber"].Value = null;
                            }
                    }
                }
            }
            CalculateTotal();
        }

        private void insertAccountThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;
            try
            {
                if (type == "account_code")
                {
                    if (dgvPartner.CurrentRow.Cells["account_code"].Value != null)
                    {
                        string query = "select 0 code,'' name";
                        if (cmbPaymentType.Text == "Vendor")
                        {
                            query = "select id.code from tbl_vendor where code=@code";
                        }
                        else if (cmbPaymentType.Text == "Customer")
                        {
                            query = "select id,code from tbl_customer where code=@code";
                        }
                        else if (cmbPaymentType.Text == "Employee")
                        {
                            query = "select id,code from tbl_employee where code=@code";
                        }
                        reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("code", dgvPartner.CurrentRow.Cells["account_code"].Value.ToString()));
                    }
                }
                else if (type == "combo" && comboBox.SelectedValue != null)
                {
                    string selectedAccountCode = comboBox.SelectedValue.ToString();
                    reader = DBClass.ExecuteReader(@"SELECT id,CAST(code as CHAR) code FROM tbl_coa_level_4 WHERE code = @code",
                        DBClass.CreateParameter("code", selectedAccountCode));
                }

                if (reader != null && reader.Read())
                {
                    dgvPartner.CurrentRow.Cells["account_code"].Value = reader["code"].ToString();
                    dgvPartner.CurrentRow.Cells["account_id"].Value = reader["id"].ToString();
                    if (type == "account_code" && comboCell != null)
                        comboCell.Value = dgvPartner.CurrentRow.Cells["account_code"].Value.ToString();
                }
            }
            finally
            {
                if (reader != null)
                {
                    //reader.Close();
                    //reader.Dispose();
                }
            }
        }

        private void dgvPartner_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvPartner.IsCurrentCellDirty)
            {
                dgvPartner.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAmount.Text == "")
            {
                txtAmountInWord.Text = "";
                return;
            }
            txtAmountInWord.Text = GeneralConfiguration.NumberToWords(decimal.Parse(txtAmount.Text));
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertPV())
                {

                    MessageBox.Show("The Advance Payment  Paid  ");
                    dgvPartner.Rows.Clear();
                    txtPVCode.Text = GenerateNextPaymentCode();
                    EventHub.RefreshPaymentVoucher();
           
                }
            }
            else
            {
                if (updatePV())
                {
                    MessageBox.Show("The Advance Payment  Paid  ");
                    dgvPartner.Rows.Clear();
                    txtPVCode.Text = GenerateNextPaymentCode();
                    EventHub.RefreshPaymentVoucher();
           
                }
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int? currentId = Utilities.GetVoucherIdFromCode(txtPVCode.Text);
            if (currentId == null || currentId <= 1)
                return;

            currentId = currentId - 1;
            if (currentId <= 0)
            {
                clear();
                MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "select id from tbl_advance_payment_voucher where state = 0 and id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindVoucher();
                }
                else
                {
                    clear();
                    MessageBox.Show("No previous record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                int? currentId = Utilities.GetVoucherIdFromCode(txtPVCode.Text);
                if (currentId is null) return;

                currentId = currentId + 1;
                string query = "SELECT id FROM tbl_advance_payment_voucher WHERE state = 0 AND id =@id";
                using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                {
                    if (reader.Read())
                    {
                        id = int.Parse(reader["id"].ToString());
                        BindVoucher();
                    }
                    else
                    {
                        clear();
                        MessageBox.Show("No next record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtPVCode.Text = GenerateNextPaymentCode();
            clear();
        }
        private void clear()
        {
            dgvPartner.Rows.Clear();
            id = 0;
            txtAmount.Text = "";
            txtAmountInWord.Text = "";
            txtcostcenter.Text = "";
            txtCreditAccountCode.Text = "";
            txtDebitCode.Text = "";
            txtDescription.Text = "";
            txtPVCode.Text = "";
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //DBClass.ExecuteNonQuery("UPDATE tbl_advance_payment_voucher SET state = -1 WHERE id = @id ",
            //                              DBClass.CreateParameter("id", id.ToString()));
            //Utilities.LogAudit(frmLogin.userId, "Delete Advance Payment Voucher", "Advance Payment Voucher", id, "Deleted Advance Payment Voucher: " + txtPVCode.Text);
            //clear();
            DataTable dtVoucher = DBClass.ExecuteDataTable("SELECT * FROM tbl_advance_payment_voucher WHERE id = @id",
                DBClass.CreateParameter("id", id.ToString()));

            DataTable dtDetails = DBClass.ExecuteDataTable("SELECT * FROM tbl_advance_payment_voucher_details WHERE payment_id = @id",
                DBClass.CreateParameter("id", id.ToString()));

            DataTable dtTransaction = DBClass.ExecuteDataTable("SELECT * FROM tbl_transaction WHERE transaction_id = @id AND t_type='Advance PAYMENT'",
                DBClass.CreateParameter("id", id.ToString()));

            // 2. Insert backups into tbl_deleted_records
            foreach (DataRow row in dtVoucher.Rows)
            {
                DBClass.ExecuteNonQuery(
                    "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                    DBClass.CreateParameter("table", "tbl_advance_payment_voucher"),
                    DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                    DBClass.CreateParameter("user", frmLogin.userId.ToString())
                );
            }

            foreach (DataRow row in dtDetails.Rows)
            {
                DBClass.ExecuteNonQuery(
                    "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                    DBClass.CreateParameter("table", "tbl_advance_payment_voucher_details"),
                    DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                    DBClass.CreateParameter("user", frmLogin.userId.ToString())
                );
            }

            foreach (DataRow row in dtTransaction.Rows)
            {
                DBClass.ExecuteNonQuery(
                    "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                    DBClass.CreateParameter("table", "tbl_transaction"),
                    DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                    DBClass.CreateParameter("user", frmLogin.userId.ToString())
                );
            }

            // 3. Now delete records permanently
            DBClass.ExecuteNonQuery("DELETE FROM tbl_transaction WHERE transaction_id = @id AND t_type='Advance PAYMENT'",
                DBClass.CreateParameter("id", id.ToString()));
            DBClass.ExecuteNonQuery("DELETE FROM tbl_advance_payment_voucher_details WHERE payment_id = @id",
                DBClass.CreateParameter("id", id.ToString()));
            DBClass.ExecuteNonQuery("DELETE FROM tbl_advance_payment_voucher WHERE id = @id",
                DBClass.CreateParameter("id", id.ToString()));

            Utilities.LogAudit(frmLogin.userId, "Advance Payment Voucher Permanently Deleted", "Advance Payment Voucher", id, "Deleted Advance Payment Voucher with ID: " + id);
            clear();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton26_Click(object sender, EventArgs e)
        {

        }

        void CalculateTotal()
        {
            decimal total = 0;

            foreach (DataGridViewRow row in dgvPartner.Rows)
            {
                if (row.IsNewRow) continue;
                decimal amount;
                var value = row.Cells["amount"].Value;
                if (value != null && decimal.TryParse(value.ToString(), out amount))
                {
                    total += amount;
                }
            }

            txtAmount.Text = total.ToString("0.000");
        }

        private void dgvPartner_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox comboBox = e.Control as ComboBox;
            //if (dgvJV.CurrentCell.ColumnIndex == dgvJV.Columns["account_name"].Index)
            //{
            //    if (e.Control is ComboBox combo)
            //    {
            //        combo.DropDownStyle = ComboBoxStyle.DropDown;
            //        combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //        combo.AutoCompleteSource = AutoCompleteSource.ListItems;

            //        combo.Text = "";
            //    }

            //    if (comboBox != null)
            //    {
            //        comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxName_SelectedIndexChanged);
            //        comboBox.SelectedIndexChanged += new EventHandler(ComboBoxName_SelectedIndexChanged);
            //    }
            //}
            //else if (dgvJV.CurrentCell.ColumnIndex == dgvJV.Columns["Debit"].Index)
            //{
            //    var debitColumnIndex = dgvJV.Columns["Debit"].Index;

            //    if (dgvJV.CurrentCell.ColumnIndex == debitColumnIndex)
            //    {
            //        e.Control.KeyPress -= new KeyPressEventHandler(ValidateDebitInput);
            //        e.Control.KeyPress += new KeyPressEventHandler(ValidateDebitInput);
            //    }
            //}
            //else if (dgvJV.CurrentCell.ColumnIndex == dgvJV.Columns["Credit"].Index)
            //{
            //    var creditColumnIndex = dgvJV.Columns["Credit"].Index;
            //    if (dgvJV.CurrentCell.ColumnIndex == creditColumnIndex)
            //    {
            //        e.Control.KeyPress -= new KeyPressEventHandler(ValidateCreditInput);
            //        e.Control.KeyPress += new KeyPressEventHandler(ValidateCreditInput);
            //    }
            //}
            if (dgvPartner.CurrentCell.ColumnIndex == dgvPartner.Columns["account_code"].Index)
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.TextChanged -= AccountCodeTextBox_TextChanged;
                    tb.TextChanged += AccountCodeTextBox_TextChanged;
                }
            }
            else
            {
                if (dgvPartner.CurrentCell.ColumnIndex == dgvPartner.Columns["Amount"].Index)
                {
                    TextBox txt = e.Control as TextBox;
                    if (txt != null)
                    {
                        txt.KeyPress -= Txt_KeyPress;
                        txt.KeyPress += Txt_KeyPress;
                    }
                }
                else
                {
                    // Ensure we remove suggestion logic from non-account_code columns
                    if (e.Control is TextBox tb)
                    {
                        tb.TextChanged -= AccountCodeTextBox_TextChanged;
                    }

                    // Hide suggestion box just in case
                    lstAccountSuggestions.Visible = false;

                    e.Control.KeyPress -= new KeyPressEventHandler(ValidateText);
                    e.Control.KeyPress += new KeyPressEventHandler(ValidateText);
                }
            }
        }

        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true; // Block the input
            }

            // Allow only one decimal point
            TextBox txt = sender as TextBox;
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void ValidateText(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
        }

        private void AccountCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null) return;

            string input = tb.Text.Trim();
            if (string.IsNullOrEmpty(input))
            {
                lstAccountSuggestions.Visible = false;
                return;
            }

            string query = @"SELECT 0 code,'' name";
            //string query = @"SELECT code, name FROM tbl_coa_level_4 
            //         WHERE code LIKE @search OR name LIKE @search LIMIT 20";
            if (cmbPaymentType.Text == "Vendor")
            {
                query = "select code, name from tbl_vendor where state = 0 and code LIKE @search OR name LIKE @search LIMIT 20";
            }
            else if (cmbPaymentType.Text == "Customer")
            {
                query = "select code,name from tbl_customer where state = 0 and code LIKE @search OR name LIKE @search LIMIT 20";
            }
            else if (cmbPaymentType.Text == "Employee")
            {
                query = "select code,name from tbl_employee where state = 0 and code LIKE @search OR name LIKE @search LIMIT 20";
            }

            DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

            lstAccountSuggestions.Items.Clear();
            foreach (DataRow row in dt.Rows)
            {
                lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
            }

            if (lstAccountSuggestions.Items.Count > 0)
            {
                lstAccountSuggestions.Visible = true;
                Rectangle cellRect = dgvPartner.GetCellDisplayRectangle(
                    dgvPartner.CurrentCell.ColumnIndex, dgvPartner.CurrentCell.RowIndex, true);

                lstAccountSuggestions.SetBounds(
                    dgvPartner.Left + cellRect.Left,
                    dgvPartner.Top + cellRect.Bottom,
                    cellRect.Width + 80,
                    120);
                lstAccountSuggestions.BringToFront();
            }
            else
            {
                lstAccountSuggestions.Visible = false;
            }
        }

        private void dgvPartner_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (lstAccountSuggestions.SelectedItem != null)
            {
                string selected = lstAccountSuggestions.SelectedItem.ToString();
                string code = selected.Split('-')[0].Trim();

                dgvPartner.CurrentCell.Value = code;
                lstAccountSuggestions.Visible = false;
            }
        }

        private void lstAccountSuggestions_Click(object sender, EventArgs e)
        {
            if (lstAccountSuggestions.SelectedItem != null)// && lstAccountSuggestions.Tag is TextBox targetTextBox)
            {
                if (lstAccountSuggestions.SelectedItem == null) return;

                string selected = lstAccountSuggestions.SelectedItem.ToString();
                string code = selected.Split('-')[0].Trim();

                // Set value to the current cell
                dgvPartner.CurrentCell.Value = code;

                // Hide the suggestion list
                lstAccountSuggestions.Visible = false;

                // Re-focus DataGridView if needed
                dgvPartner.Focus();

                //string selected = lstAccountSuggestions.SelectedItem.ToString();
                //string selectedCode = selected.Split('-')[0].Trim();

                //targetTextBox.Text = selectedCode;
                //lstAccountSuggestions.Visible = false;

                //// Trigger TextChanged manually if needed
                //targetTextBox.Focus();
                //targetTextBox.SelectionStart = targetTextBox.Text.Length;

                // Example of post-selection logic:
                //if (targetTextBox == txtDebitCode)
                //{
                //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                //        DBClass.CreateParameter("code", selectedCode)))
                //        if (reader.Read())
                //            cmbDebitAccountName.SelectedValue = int.Parse(reader["id"].ToString());
                //}
                //else if (targetTextBox == txtCreditAccountCode)
                //{
                //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                //        DBClass.CreateParameter("code", selectedCode)))
                //        if (reader.Read())
                //            cmbCreditAccountName.SelectedValue = int.Parse(reader["id"].ToString());
                //}
                //else if (targetTextBox == txtVendor)
                //{
                //    using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_vendor where code =@code",
                //        DBClass.CreateParameter("code", selectedCode)))
                //        if (reader.Read())
                //            cmbVendor.SelectedValue = int.Parse(reader["id"].ToString());
                //}
            }

            //if (lstAccountSuggestions.SelectedItem != null)
            //{
            //    txtDebitCode.Text = lstAccountSuggestions.SelectedItem.ToString();
            //    lstAccountSuggestions.Visible = false;

            //    MessageBox.Show("You selected: " + txtDebitCode.Text);
            //}
        }
    }
}