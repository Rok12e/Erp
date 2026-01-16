using CrystalDecisions.CrystalReports.Engine;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Reports.Design;
using YamyProject.UI.Settings;

namespace YamyProject
{
    public partial class frmViewReceiptVoucher : Form
    {
        int id, defaultCustomerId = 0;
        decimal totalAmount = 0;
        string code;
        public frmViewReceiptVoucher(int id = 0, int _customerId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = this.Text; //id == 0 ? "Receipt Voucher - New" : "Receipt Voucher - Edit";

            this.defaultCustomerId = _customerId;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewReceiptVoucher_Load(object sender, EventArgs e)
        {
            dtOpen.Value = dtpTransDate.Value = dt_check_date.Value = DateTime.Now.Date;
            txtPVCode.Text = GenerateNextReceiptCode();
            txtId.Text = GenerateNextReceiptId();
            BindCombos.PopulateCustomers(cmbCustomer, true);
            BindCombos.PopulateCostCenter(cmbCreditCostCenter, true);
            BindCombos.PopulateCostCenter(cmbDebitCostCenter, true);
            BindCombos.PopulateAllLevel4Account(cmbDebitAccountName, true);
            BindCombos.PopulateAllLevel4Account(cmbCreditAccountName, true);
            //BindCombos.PopulateRegisterBanks(cmbBankName, true);
            BindCombosBindBankAccount();
            
            cmbMethod.SelectedIndex = cmbPaymentType.SelectedIndex = 0;
            if (id > 0)
            {
                BindVoucher();
            } else {
                if (defaultCustomerId > 0)
                {
                    cmbCustomer.SelectedValue = defaultCustomerId;
                    LoadData();
                }
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvInv);
        }

        private void BindCombosBindBankAccount()
        {
            DataTable dt = DBClass.ExecuteDataTable("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_bank where state = 0 and id IN(select bank_id from tbl_bank_card where company_ac=1);");
            cmbBankName.ValueMember = "id";
            cmbBankName.DisplayMember = "name";
            cmbBankName.DataSource = dt;

            if (dt.Rows.Count > 0)
            {
                cmbBankName.SelectedIndex = 0;
            }
        }

        private void BindVoucher()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_receipt_voucher WHERE id = " + id))
                if (reader.Read())
                {
                    txtId.Text = reader["id"].ToString();
                    dtOpen.Value = DateTime.Parse(reader["date"].ToString());
                    code = reader["code"].ToString();
                    txtPVCode.Text = reader["code"].ToString();
                    cmbPaymentType.Text = reader["type"].ToString();
                    cmbMethod.Text = reader["method"].ToString();
                    txtAmount.Text = reader["amount"].ToString();
                    cmbCreditAccountName.SelectedValue = reader["credit_account_id"].ToString();
                    cmbCreditCostCenter.SelectedValue = reader["credit_cost_center_id"].ToString();
                    txtDescription.Text = reader["description"].ToString();
                    cmbDebitAccountName.SelectedValue = reader["debit_account_id"].ToString();
                    cmbDebitCostCenter.SelectedValue = reader["debit_cost_center_id"].ToString();

                    if (reader["bank_id"] != null && !string.IsNullOrEmpty(reader["bank_id"].ToString()))
                    {
                        if (cmbMethod.Text == "Cheque")
                        {
                            cmbBankName.SelectedValue = reader["bank_id"].ToString();
                        }
                    }
                    else
                    {
                        //
                    }
                    txtBankCode.Text = reader["bank_code"].ToString();
                    txtCheckName.Text = reader["check_name"].ToString();
                    txtCheckNo.Text = reader["check_no"].ToString();
                    if (reader["trans_date"].ToString() != "")
                    {
                        dtpTransDate.Value = DateTime.Parse(reader["trans_date"].ToString());
                    }
                    txtTransName.Text = reader["trans_name"].ToString();
                    txtTransRef.Text = reader["trans_ref"].ToString();

                    if (cmbPaymentType.Text == "Customer")
                    {
                        txtAmount.Enabled = false;
                        BindCombos.PopulateCustomers(cmbCustomer);
                        dgvInv.Columns["InvNo"].HeaderText = "Inv No";
                        cmbCreditCostCenter.Visible =
                        lblCostcenter.Visible =
                        dgvInv.Visible = true;
                    }
                    else if (cmbPaymentType.Text == "General")
                    {
                        txtAmount.Enabled = true;
                        cmbCreditCostCenter.Visible = false;
                        lblCostcenter.Visible = false;
                        dgvInv.Visible = false;
                    }

                    loadReceiptDataDetails();
                }
        }
        private void loadReceiptDataDetails()
        {
            dgvInv.Rows.Clear();
            int serialNumber = 1;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                     SELECT dt.id,dt.date,dt.payment_id,dt.hum_id,dt.inv_code,dt.inv_id,dt.payment,dt.total,dt.description,dt.cost_center_id
                     FROM tbl_receipt_voucher_details dt
                     WHERE dt.payment_id = @id;",
                    DBClass.CreateParameter("id", id)))

                //reader = DBClass.ExecuteReader(@"
                //                SELECT 
                //                    c.id, c.date,

                //                    COALESCE(SUM(
                //                        CASE 
                //                            WHEN t.type = 'Customer Payment' THEN -IF(t.debit = 0, t.credit, t.debit)
                //                            WHEN t.type = 'Sales Invoice Cash' THEN 0
                //                            WHEN t.type = 'Customer Opening Balance' AND t.credit > 0 THEN -t.credit
                //                            WHEN t.type LIKE 'Customer%' OR t.type LIKE 'Sales%' THEN IF(t.debit = 0, t.credit, t.debit) 
                //                            ELSE 0  
                //                        END
                //                    ), 0) AS Amount

                //                FROM tbl_customer c
                //                LEFT JOIN tbl_transaction t ON t.hum_id = c.id
                //               where c.id = @id AND t.transaction_id NOT IN (select tbl_sales.id FROM tbl_sales
                //        INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                //        WHERE tbl_sales.state = 0 AND tbl_sales.change <> 0 AND tbl_customer.id =@id)
                //                 GROUP BY c.id,c.date;
                //                ",
            while (reader.Read())
            {
                string inv_id = "0", inv_code = reader["inv_code"].ToString(), bill_amount = "0", bill_date = "", payment = "0", description = "", cost_center_id = "0";
                cost_center_id = (reader["cost_center_id"].ToString() == "" ? null : reader["cost_center_id"].ToString());
                description = reader["description"].ToString();
                payment = reader["payment"].ToString();
                string humId = reader["hum_id"].ToString();
                cmbCustomer.SelectedValue = int.Parse(humId);
                    using (MySqlDataReader drSale = DBClass.ExecuteReader(@"
                            SELECT tbl_sales.date,tbl_sales.id,tbl_sales.invoice_id,tbl_sales.net,tbl_sales.pay,tbl_sales.change 
                            FROM tbl_sales 
                            INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                            WHERE tbl_sales.state = 0 AND tbl_sales.change <> 0 
	                         AND tbl_sales.id = @id
                            ",
                                        DBClass.CreateParameter("id", reader["inv_id"].ToString())))
                        if (drSale.Read())
                        {
                            inv_id = drSale["id"].ToString();
                            bill_amount = drSale["net"].ToString();
                            bill_date = DateTime.Parse(drSale["date"].ToString()).ToShortDateString();
                        }
                        else
                        {
                            bill_amount = reader["total"].ToString();
                            bill_date = DateTime.Parse(reader["date"].ToString()).ToShortDateString();
                        }
                dgvInv.Rows.Add(serialNumber.ToString(), humId, inv_id, bill_date, inv_code, bill_amount, (bill_amount == payment ? true: false), payment, description, cost_center_id);
                
                serialNumber++;
            }
            dgvInv.Columns["humId"].Visible = dgvInv.Columns["invId"].Visible = false;
            //dgvInv.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            //dgvInv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            //dgvInv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            //dgvInv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvInv.ColumnHeadersHeight = 40;
            //dgvInv.EnableHeadersVisualStyles = false;
            //dgvInv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            calculateTotal();
        }
        private string GenerateNextReceiptId()
        {
            string newCode = "1";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(id) AS lastCode FROM tbl_receipt_voucher"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = code.ToString();
                }
            }

            return newCode;
        }
        private string GenerateNextReceiptCode()
        {
            string newCode = "RV-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)) AS lastCode FROM tbl_receipt_voucher"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "RV-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
          
            calculateTotal();
            if (id == 0)
            {
                if (insertRV())
                {
                    dgvInv.Rows.Clear();
                    MessageBox.Show("The Receipt Voucher  Saved");
                    EventHub.RefreshReceiptVoucher();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                    txtPVCode.Text = GenerateNextReceiptCode();
                }
            } else
            {
                if (updateRV())
                {
                    dgvInv.Rows.Clear();
                    MessageBox.Show("The Receipt Voucher  Updated");
                    EventHub.RefreshReceiptVoucher();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                    txtPVCode.Text = GenerateNextReceiptCode();
                }
            }
     
           
          
        }
        private void loadPrint()
        {

            DialogResult result = MessageBox.Show("Do You want To Show This Payment ",
                                             "Confirmation",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                ShowReport();
            }
            else if (result == DialogResult.No)
            {
                this.Close();
            }
        }
        private bool updateRV()
        {
            if (!chkRequireData())
                return false;

            DBClass.ExecuteScalar(@"UPDATE tbl_receipt_voucher SET
                                                          date=@date, code=@code, type=@type, method=@method, amount=@amount,  debit_account_id=@debit_account_id,debit_cost_center_id=@debit_cost_center_id, 
                                                          description=@description,credit_account_id=@credit_account_id,credit_cost_center_id=@credit_cost_center_id,
                                                          bank_account_id=@bank_account, book_no=@book_no,bank_id=@bank_id, bank_code=@bank_code, check_name=@check_name, check_no=@check_no,
                                                          check_date=@check_date,trans_date=@trans_date, trans_name=@trans_name, trans_ref=@trans_ref, modified_by=@modified_by, modified_date=@modified_date where id=@id",
                                                          DBClass.CreateParameter("id", id),
                                                          DBClass.CreateParameter("date", dtOpen.Value.Date),
                   DBClass.CreateParameter("code", code),
                   DBClass.CreateParameter("type", cmbPaymentType.Text),
                   DBClass.CreateParameter("method", cmbMethod.Text),
                   DBClass.CreateParameter("amount", decimal.Parse(txtAmount.Text)),
                   DBClass.CreateParameter("credit_account_id", cmbCreditAccountName.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("credit_cost_center_id", cmbCreditCostCenter.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("description", txtDescription.Text),
                   DBClass.CreateParameter("debit_account_id", cmbDebitAccountName.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("debit_cost_center_id", cmbDebitCostCenter.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("bank_id", cmbMethod.Text == "Cheque" ? cmbBankName.SelectedValue ?? DBNull.Value : DBNull.Value),
                   DBClass.CreateParameter("bank_account", "0"),
                   DBClass.CreateParameter("book_no", "0"),
                   DBClass.CreateParameter("bank_code", txtBankCode.Text),
                   DBClass.CreateParameter("check_name", string.IsNullOrWhiteSpace(txtCheckName.Text) ? DBNull.Value : (object)txtCheckName.Text),
                   DBClass.CreateParameter("check_no", string.IsNullOrWhiteSpace(txtCheckNo.Text) ? DBNull.Value : (object)txtCheckNo.Text),
                   DBClass.CreateParameter("check_date", cmbMethod.Text == "Cheque" ? (object)dt_check_date.Value.Date : DBNull.Value),
                   DBClass.CreateParameter("trans_date", cmbMethod.Text == "Transfer" ? (object)dtpTransDate.Value.Date : DBNull.Value),
                   DBClass.CreateParameter("trans_name", string.IsNullOrWhiteSpace(txtTransName.Text) ? DBNull.Value : (object)txtTransName.Text),
                   DBClass.CreateParameter("trans_ref", string.IsNullOrWhiteSpace(txtTransRef.Text) ? DBNull.Value : (object)txtTransRef.Text),
                   DBClass.CreateParameter("modified_by", frmLogin.userId),
                   DBClass.CreateParameter("modified_date", DateTime.Now.Date));
            DBClass.ExecuteNonQuery("Delete from tbl_receipt_voucher_details where payment_id=@id", DBClass.CreateParameter("id", id));
            CommonInsert.DeleteTransactionEntry(id, "RECEIPT");
            
            if (cmbMethod.Text == "Cheque")
            {//DefaultBankId from first account and cheque id
                int chequeId = GetChequeBookIdByCompanyOrFirst();
                object result = DBClass.ExecuteScalar(@"SELECT id FROM tbl_check_details WHERE pvc_no = @pvc_no AND check_type = @check_type AND check_id = @check_id",
                    DBClass.CreateParameter("pvc_no", id),
                    DBClass.CreateParameter("check_type", "Receipt"),
                    DBClass.CreateParameter("check_id", chequeId.ToString())
                );
                int checkDetail_Id = Convert.ToInt32(result);
                if (checkDetail_Id > 0)
                {
                    DBClass.ExecuteNonQuery(@"
                        UPDATE tbl_check_details
                        SET check_no = @check_no, 
                            check_date = @check_date, 
                            check_name = @check_name, 
                            amount = @amount,
                        WHERE pvc_no = @pvc_no AND check_type = @check_type AND check_id = @check_id and id = @id",
                        DBClass.CreateParameter("check_no", txtCheckNo.Text),
                        DBClass.CreateParameter("check_date", dt_check_date.Value.Date),
                        DBClass.CreateParameter("check_name", txtCheckName.Text),
                        DBClass.CreateParameter("amount", decimal.Parse(txtAmount.Text)),
                        //DBClass.CreateParameter("state", "New"),
                        DBClass.CreateParameter("pvc_no", id),
                        DBClass.CreateParameter("check_type", "Receipt"),
                        DBClass.CreateParameter("check_id", chequeId.ToString()),
                        DBClass.CreateParameter("id", checkDetail_Id)
                    );
                }
                else
                {
                    int checkDetailId = (int)decimal.Parse(DBClass.ExecuteScalar(@"
                        INSERT INTO tbl_check_details
                        (date, check_id, check_no, check_date, check_type, pvc_no, check_name, amount, state)
                        VALUES
                        (@date, @check_id, @check_no, @check_date, @check_type, @pvc_no, @check_name, @amount, @state);
                        SELECT LAST_INSERT_ID();",
                        DBClass.CreateParameter("date", dtOpen.Value.Date),
                        DBClass.CreateParameter("check_id", chequeId.ToString()),
                        DBClass.CreateParameter("check_no", txtCheckNo.Text),
                        DBClass.CreateParameter("check_date", dt_check_date.Value.Date),
                        DBClass.CreateParameter("check_type", "Receipt"),
                        DBClass.CreateParameter("pvc_no", id),
                        DBClass.CreateParameter("check_name", txtCheckName.Text),
                        DBClass.CreateParameter("amount", decimal.Parse(txtAmount.Text)),
                        DBClass.CreateParameter("state", "New")
                    ).ToString());
                }

            }
            else if (cmbMethod.Text == "Transfer")
            {
            }
            //insertJournals();
            insertINV(id);
            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "Receipt");
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, txtAmount.Text.ToString(), "0", id.ToString(), "Receipt", "Receipt Debit Entry", cmbDebitCostCenter.SelectedValue.ToString());
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, "0", txtAmount.Text.ToString(), id.ToString(), "Receipt", "Receipt Credit Entry", cmbCreditCostCenter.SelectedValue.ToString());
            Utilities.LogAudit(frmLogin.userId, "Update Receipt Voucher", "Receipt Voucher", id, "Updated Receipt Voucher: " + code);
            return true;
        }

        private bool insertRV()
        {
            if (!chkRequireData())
                return false;
            code = GenerateNextReceiptCode();
            id = (int)decimal.Parse(DBClass.ExecuteScalar(@"
                    INSERT INTO tbl_receipt_voucher(
                        date, code, type, method, amount,  
                        debit_account_id, debit_cost_center_id, 
                        credit_account_id, credit_cost_center_id, description,
                        bank_id, bank_account_id, book_no, check_name, check_no, check_date,
                        trans_date, trans_name, trans_ref, created_by, created_date, state
                    ) 
                    VALUES (
                        @date, @code, @type, @method, @amount, 
                        @debit_account_id, @debit_cost_center_id, 
                        @credit_account_id, @credit_cost_center_id, @description,
                        @bank_id, @bank_account, @book_no, @check_name, @check_no, @check_date,
                        @trans_date, @trans_name, @trans_ref, @created_by, @created_date, 0
                    ); 
                    SELECT LAST_INSERT_ID();",

                   DBClass.CreateParameter("date", dtOpen.Value.Date),
                   DBClass.CreateParameter("code", code),
                   DBClass.CreateParameter("type", cmbPaymentType.Text),
                   DBClass.CreateParameter("method", cmbMethod.Text),
                   DBClass.CreateParameter("amount", decimal.Parse(txtAmount.Text)), 
                   DBClass.CreateParameter("credit_account_id", cmbCreditAccountName.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("credit_cost_center_id", cmbCreditCostCenter.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("description", txtDescription.Text),
                   DBClass.CreateParameter("debit_account_id", cmbDebitAccountName.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("debit_cost_center_id", cmbDebitCostCenter.SelectedValue ?? DBNull.Value),
                   DBClass.CreateParameter("bank_id", cmbMethod.Text == "Cheque" ? cmbBankName.SelectedValue ?? DBNull.Value : DBNull.Value),
                   DBClass.CreateParameter("bank_account", "0"), 
                   DBClass.CreateParameter("book_no", "0"),
                   DBClass.CreateParameter("check_name", string.IsNullOrWhiteSpace(txtCheckName.Text) ? DBNull.Value : (object)txtCheckName.Text),
                   DBClass.CreateParameter("check_no", string.IsNullOrWhiteSpace(txtCheckNo.Text) ? DBNull.Value : (object)txtCheckNo.Text),
                   DBClass.CreateParameter("check_date", cmbMethod.Text == "Cheque" ? (object)dt_check_date.Value.Date : DBNull.Value),
                   DBClass.CreateParameter("trans_date", cmbMethod.Text == "Transfer" ? (object)dtpTransDate.Value.Date : DBNull.Value),
                   DBClass.CreateParameter("trans_name", string.IsNullOrWhiteSpace(txtTransName.Text) ? DBNull.Value : (object)txtTransName.Text),
                   DBClass.CreateParameter("trans_ref", string.IsNullOrWhiteSpace(txtTransRef.Text) ? DBNull.Value : (object)txtTransRef.Text),
                   DBClass.CreateParameter("created_by", frmLogin.userId),
                   DBClass.CreateParameter("created_date", DateTime.Now.Date)
               ).ToString());

            if (cmbMethod.Text == "Cheque")
            {
                //DefaultBankId from first account and cheque id
                int chequeId = GetChequeBookIdByCompanyOrFirst();
                int checkDetailId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_check_details
                        (date, check_id, check_no, check_date, check_type, pvc_no, check_name, amount, state)
                        VALUES
                        (@date, @check_id, @check_no, @check_date, @check_type, @pvc_no, @check_name, @amount, @state); 
                        SELECT LAST_INSERT_ID();",
                        DBClass.CreateParameter("date", dtOpen.Value.Date),
                        DBClass.CreateParameter("check_id", chequeId.ToString()),
                        DBClass.CreateParameter("check_no", txtCheckNo.Text),
                        DBClass.CreateParameter("check_date", dt_check_date.Value.Date),
                        DBClass.CreateParameter("check_type", "Receipt"),
                        DBClass.CreateParameter("pvc_no", id),
                        DBClass.CreateParameter("check_name", txtCheckName.Text),
                        DBClass.CreateParameter("amount", decimal.Parse(txtAmount.Text)),
                        DBClass.CreateParameter("state", "New")
                    ).ToString());
            }
            else if (cmbMethod.Text == "Transfer")
            {
            }

            //insertJournals();
            insertINV(id);
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, txtAmount.Text.ToString(), "0", id.ToString(), "Receipt", "Receipt Debit Entry", cmbDebitCostCenter.SelectedValue.ToString());
            CommonInsert.InsertCostCenterTransaction(dtOpen.Value, "0", txtAmount.Text.ToString(), id.ToString(), "Receipt", "Receipt Credit Entry", cmbCreditCostCenter.SelectedValue.ToString());
            Utilities.LogAudit(frmLogin.userId, "Create Receipt Voucher", "Receipt Voucher", id, "Created Receipt Voucher: " + code);
            return true;
        }
        public static int GetChequeBookIdByCompanyOrFirst()
        {
            string query = @"
                            (
                                SELECT c.id
                                FROM tbl_cheque c
                                JOIN tbl_bank_card bc ON c.bank_card_id = bc.id
                                WHERE bc.account_name LIKE CONCAT('%', (SELECT name FROM tbl_company LIMIT 1), '%')
                                ORDER BY c.id ASC
                            )
                            UNION
                            (
                                SELECT c.id
                                FROM tbl_cheque c
                                JOIN tbl_bank_card bc ON c.bank_card_id = bc.id
                                ORDER BY c.id ASC
                            )
                            LIMIT 1;
                            ";
            object result = DBClass.ExecuteScalar(query);
            if (result != null && int.TryParse(result.ToString(), out int chequeBookId))
            {
                return chequeBookId;
            }
            return -1;
        }

        private bool chkRequireData()
        {
            if(txtCheckNo.Text.Trim()=="" & cmbMethod.Text == "Cheque")
            {
                MessageBox.Show("Please Enter Check NO.");
                return false;
            }
            if (txtTransName.Text.Trim() == "" & cmbMethod.Text == "Transfer")
            {
                MessageBox.Show("Please Enter Transfer Name ");
                return false;
            }
            if (txtTransRef.Text.Trim() == "" & cmbMethod.Text == "Transfer")
            {
                MessageBox.Show("Please Enter Transfer Reference ");
                return false;
            }
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
            if ((txtAmount.Text == "" || decimal.Parse(txtAmount.Text) == 0) && dgvInv.Rows.Count == 0)
            {
                MessageBox.Show("Enter Amount");
                return false;
            }
            if (cmbMethod.Text == "Cheque")
            {
                if (cmbBankName.SelectedValue == null)
                {
                    if (cmbBankName.Items.Count == 1)
                    {
                        cmbBankName.SelectedIndex = 0;
                    }

                    if (cmbBankName.SelectedValue == null)
                    {
                        MessageBox.Show("Please Add Company Bank Account");
                        return false;
                        //cmbBankName.SelectedValue = -1;
                    }
                }
            }
            else if (cmbMethod.Text == "Transfer")
            {
                if (txtTransName.Text == "" || txtTransRef.Text == "")
                {
                    MessageBox.Show("Please Enter Transfer Information First");
                    return false;
                }
            }
            return true;
        }
        private void insertINV(int pvId)
        {
            for (int i = 0; i < dgvInv.Rows.Count; i++)
            {
                if (dgvInv.Rows[i].Cells["pay"].Value == null || dgvInv.Rows[i].Cells["pay"].Value.ToString() == "" || decimal.Parse(dgvInv.Rows[i].Cells["pay"].Value.ToString()) == 0)
                    continue;
                var invId = dgvInv.Rows[i].Cells["invid"].Value?.ToString() != "" ? dgvInv.Rows[i].Cells["invid"].Value?.ToString() : "0";
                var hum = dgvInv.Rows[i].Cells["humId"].Value?.ToString() ?? string.Empty;
                var total = dgvInv.Rows[i].Cells["total"].Value?.ToString() ?? string.Empty;
                string payment = dgvInv.Rows[i].Cells["pay"].Value.ToString();
                var description = dgvInv.Rows[i].Cells["description"].Value?.ToString() ?? "";
                string voucherType = "Customer";
                DateTime date = DateTime.Parse(dgvInv.Rows[i].Cells["invDate"].Value.ToString());
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_receipt_voucher_details
                                        (date,payment_id,hum_id, inv_id,inv_code,total, payment, description) 
                                        VALUES (@date,@payment_id,@hum_id ,@inv_id,@inv_code,@total, @payment, @description);",
                    DBClass.CreateParameter("@payment_id", pvId),
                    DBClass.CreateParameter("@date", date.Date),
                    DBClass.CreateParameter("@hum_id", hum.ToString()),
                    DBClass.CreateParameter("@inv_code", dgvInv.Rows[i].Cells["InvNo"].Value?.ToString()),
                    DBClass.CreateParameter("@total", total),
                    DBClass.CreateParameter("@inv_id", int.Parse(invId)),
                    DBClass.CreateParameter("@payment", payment),
                    DBClass.CreateParameter("@description", description));

                if (invId != "")
                {
                    if (int.Parse(invId) > 0)
                    {
                        int currentInvId = int.Parse(invId);
                        
                        object netResult = DBClass.ExecuteScalar(
                            "SELECT net FROM tbl_sales WHERE id = @id",
                            DBClass.CreateParameter("id", currentInvId)
                        );
                        decimal net = netResult != DBNull.Value ? Convert.ToDecimal(netResult) : 0;
                        
                        object result = DBClass.ExecuteScalar(
                            "SELECT SUM(payment) FROM tbl_receipt_voucher_details WHERE inv_id = @id",
                            DBClass.CreateParameter("id", currentInvId)
                        );
                        decimal totalPaid = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                        
                        decimal remaining = net - totalPaid;
                        
                        DBClass.ExecuteNonQuery(
                            "UPDATE tbl_sales SET pay = @pay, `change` = @change WHERE id = @id",
                            DBClass.CreateParameter("pay", totalPaid),
                            DBClass.CreateParameter("change", remaining),
                            DBClass.CreateParameter("id", currentInvId)
                        );
                        Utilities.LogAudit(frmLogin.userId, "Update Sales Invoice", "Sales Invoice", currentInvId, "Updated Sales Invoice: " + dgvInv.Rows[i].Cells["InvNo"].Value.ToString());
                    }
                    else if (dgvInv.Rows[i].Cells["InvNo"].Value.ToString() == "C-OB")
                    {
                        // You can handle OB logic here if needed
                    }
                }
                insertJournals(cmbPaymentType.Text.ToString(), voucherType, payment, description);
            }
        }
        void insertJournals(string type, string voucherType, string amount, string description)
        {
            CommonInsert.addTransactionEntry(dtOpen.Value.Date,
                      cmbDebitAccountName.SelectedValue.ToString(),
                      amount, "0", id.ToString(), "0", cmbPaymentType.Text + " Receipt", "RECEIPT", "Receipt Voucher NO. " + code,
                       frmLogin.userId, DateTime.Now.Date, txtPVCode.Text);
            CommonInsert.addTransactionEntry(dtOpen.Value.Date,
                                  cmbCreditAccountName.SelectedValue.ToString(),
                                 "0", amount, id.ToString(), cmbPaymentType.Text == "Customer" ? cmbCustomer.SelectedValue.ToString() : "0", cmbPaymentType.Text + " Receipt", "RECEIPT", "Receipt Voucher NO. " + code,
                                   frmLogin.userId, DateTime.Now.Date, txtPVCode.Text);
            if (cmbMethod.Text == "Cheque")
            {
                //CommonInsert.addTransactionEntry(dtOpen.Value.Date,
                //      cmbDebitAccountName.SelectedValue.ToString(),
                //      amount, "0", id.ToString(), "0", cmbPaymentType.Text + " Receipt", "RECEIPT", "Receipt Voucher NO. " + code,
                //       frmLogin.userId, DateTime.Now.Date, txtPVCode.Text);
            }
        }
        //private void insertJournalsOld()
        //{
          
        //        CommonInsert.addTransactionEntry(dtOpen.Value.Date,
        //              cmbDebitAccountName.SelectedValue.ToString(),
        //              txtAmount.Text, "0", id.ToString(), "0", cmbPaymentType.Text + " Receipt", "RECEIPT", "Receipt Voucher NO. " + code,
        //               frmLogin.userId, DateTime.Now.Date);
        //        CommonInsert.addTransactionEntry(dtOpen.Value.Date,
        //                              cmbCreditAccountName.SelectedValue.ToString(),
        //                             "0", txtAmount.Text, id.ToString(), cmbPaymentType.Text == "Customer" ? cmbCustomer.SelectedValue.ToString() : "0",  cmbPaymentType.Text + " Receipt", "RECEIPT", "Receipt Voucher NO. " + code,
        //                               frmLogin.userId, DateTime.Now.Date);
            
        //}
        private void cmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvInv.Rows.Clear();
            if (cmbPaymentType.Text == "Customer")
            {
                dgvInv.Visible = pnlCustomer.Visible = true;
                txtAmount.Enabled = false;
                BindCombos.PopulateCustomers(cmbCustomer, true);
                //cmbCreditAccountName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Customer");
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Customer")))
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
            else
            {
                dgvInv.Visible = pnlCustomer.Visible = false;
                txtAmount.Enabled = true;
            }
        }
        bool isCustomerRefreshing = false;
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.Focused)
            {
                if (isCustomerRefreshing) return;

                if (cmbCustomer.SelectedValue != null &&
                (cmbCustomer.SelectedValue.ToString() == "0" || cmbCustomer.Text == "<< Add >>"))
                {
                    new frmViewCustomer().ShowDialog(); // This will wait until form is closed

                    isCustomerRefreshing = true;
                    BindCombos.PopulateCustomers(cmbCustomer, true);
                    cmbCustomer.SelectedIndex = cmbCustomer.Items.Count - 1;
                    isCustomerRefreshing = false;
                }
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT CODE, account_id FROM tbl_customer WHERE id = @id",
                DBClass.CreateParameter("@id", cmbCustomer.SelectedValue)))

                if (reader.Read())
                {
                    txtCustomerCode.Text = reader["CODE"].ToString();
                }
            if(id<=0)
                LoadData();
            AutoFillCheckNameCustomer();
        }
        private void LoadData()
        {
            decimal customerTotalBalance = 0, customerTotalOB = 0; int counter = 0;
            dgvInv.Rows.Clear();
            //MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT SUM(
            //         tbl_sales.change) as amount
            //        FROM tbl_sales
            //        INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
            //        WHERE tbl_sales.state = 0 AND tbl_sales.change <> 0 AND tbl_customer.id = @id
            //        ", DBClass.CreateParameter("id", cmbCustomer.SelectedValue));
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT balance as amount FROM tbl_customer where id = @id
                    ", DBClass.CreateParameter("id", cmbCustomer.SelectedValue)))
                if (reader.Read() && reader["amount"].ToString() != "")
                    customerTotalOB = decimal.Parse(reader["amount"].ToString());

            //reader = DBClass.ExecuteReader(@"
            //                SELECT 
            //                    c.id, c.date,

            //                    COALESCE(SUM(
            //                        CASE 
            //                            WHEN t.type = 'Customer Payment' THEN -IF(t.debit = 0, t.credit, t.debit)
            //                            WHEN t.type = 'Sales Invoice Cash' THEN 0
            //                            WHEN t.type = 'Customer Opening Balance' AND t.credit > 0 THEN -t.credit
            //                            WHEN t.type LIKE 'Customer%' OR t.type LIKE 'Sales%' THEN IF(t.debit = 0, t.credit, t.debit) 
            //                            ELSE 0  
            //                        END
            //                    ), 0) AS Amount

            //                FROM tbl_customer c
            //                LEFT JOIN tbl_transaction t ON t.hum_id = c.id
            //               where c.id = @id AND t.transaction_id NOT IN (select tbl_sales.id FROM tbl_sales
            //        INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
            //        WHERE tbl_sales.state = 0 AND tbl_sales.change <> 0 AND tbl_customer.id =@id)
            //                 GROUP BY c.id,c.date;
            //                ",
            //DBClass.CreateParameter("id", cmbCustomer.SelectedValue));
            using (var reader = DBClass.ExecuteReader(@"SELECT c.id, c.date,
                        (SELECT sum(t.payment) FROM tbl_receipt_voucher_details t WHERE t.inv_code ='C-OB' AND t.hum_id = c.id) as amount 
                        FROM tbl_customer c where c.id = @id;",
                      DBClass.CreateParameter("id", cmbCustomer.SelectedValue)))
                if (reader.Read())
                {
                    if (reader["amount"].ToString() != "")
                        customerTotalBalance = decimal.Parse(reader["amount"].ToString());

                    if (customerTotalOB != customerTotalBalance)
                        dgvInv.Rows.Add(++counter, cmbCustomer.SelectedValue, "", DateTime.Parse(reader["date"].ToString()).ToShortDateString(), "C-OB", customerTotalOB - customerTotalBalance, 0, 0);
                }

            using (var reader = DBClass.ExecuteReader(@"SELECT ROW_NUMBER() OVER (ORDER BY tbl_sales.date) AS SN, 
                                                                      tbl_sales.date AS DATE, tbl_sales.id, tbl_sales.invoice_id AS 'INV NO', 
                                                                      tbl_sales.change FROM tbl_sales INNER JOIN tbl_customer ON tbl_sales.customer_id = tbl_customer.id
                                                                      WHERE tbl_sales.state = 0 AND tbl_sales.change <> 0 AND tbl_customer.id = @id
                                                                      GROUP BY tbl_sales.id, tbl_sales.date;",
                                                                  DBClass.CreateParameter("id", cmbCustomer.SelectedValue)))
            {
                while (reader.Read())
                {
                    string date = DateTime.Parse(reader["DATE"].ToString()).ToShortDateString();
                    string invoiceId = reader["INV NO"].ToString();
                    string change = reader["change"].ToString();
                    dgvInv.Rows.Add((int.Parse(reader["SN"].ToString()) + counter).ToString(), cmbCustomer.SelectedValue, reader["id"].ToString(), date, invoiceId, change, 0, 0);
                }
            }
            dgvInv.Columns["humId"].Visible  = dgvInv.Columns["invId"].Visible = false;
            //dgvInv.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            //dgvInv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Regular);
            //dgvInv.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            //dgvInv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            //dgvInv.ColumnHeadersHeight = 18;
            //dgvInv.EnableHeadersVisualStyles = false;
            //dgvInv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void txtDebitCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
              DBClass.CreateParameter("code", txtDebitAccountCode.Text)))
                if (reader.Read())
                    cmbDebitAccountName.SelectedValue = int.Parse(reader["id"].ToString());

            if (txtDebitAccountCode.Focused)
            {
                string input = txtDebitAccountCode.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    lstAccountSuggestions.Visible = false;
                    return;
                }
                string query = @"SELECT code, name FROM tbl_coa_level_4 
                         WHERE code LIKE @search OR name LIKE @search LIMIT 20";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

                lstAccountSuggestions.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
                }
                if (lstAccountSuggestions.Items.Count > 0)
                {
                    Point locationOnForm = txtDebitAccountCode.Parent.PointToScreen(txtDebitAccountCode.Location);
                    Point locationRelativeToForm = this.PointToClient(locationOnForm);

                    lstAccountSuggestions.SetBounds(
                        locationRelativeToForm.X,
                        locationRelativeToForm.Y + txtDebitAccountCode.Height,
                        txtDebitAccountCode.Width + 100,
                        120
                    );

                    lstAccountSuggestions.Tag = txtDebitAccountCode;
                    lstAccountSuggestions.Visible = true;
                    lstAccountSuggestions.BringToFront();
                }
                else
                {
                    lstAccountSuggestions.Visible = false;
                }
            }
        }

        private void txtCreditAccountCode_TextChanged(object sender, EventArgs e)
        {

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
            DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (reader.Read())
                    cmbCreditAccountName.SelectedValue = int.Parse(reader["id"].ToString());

            if (txtCreditAccountCode.Focused)
            {
                string input = txtCreditAccountCode.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    lstAccountSuggestions.Visible = false;
                    return;
                }
                string query = @"SELECT code, name FROM tbl_coa_level_4 
                         WHERE code LIKE @search OR name LIKE @search LIMIT 20";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

                lstAccountSuggestions.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
                }
                if (lstAccountSuggestions.Items.Count > 0)
                {
                    Point locationOnForm = txtCreditAccountCode.Parent.PointToScreen(txtCreditAccountCode.Location);
                    Point locationRelativeToForm = this.PointToClient(locationOnForm);

                    lstAccountSuggestions.SetBounds(
                        locationRelativeToForm.X,
                        locationRelativeToForm.Y + txtCreditAccountCode.Height,
                        txtCreditAccountCode.Width + 100,
                        120
                    );

                    lstAccountSuggestions.Tag = txtCreditAccountCode;
                    lstAccountSuggestions.Visible = true;
                    lstAccountSuggestions.BringToFront();
                }
                else
                {
                    lstAccountSuggestions.Visible = false;
                }
            }
        }

        private void txtDebitCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
            DBClass.CreateParameter("code", txtDebitAccountCode.Text)))
                if (!reader.Read())
                    cmbDebitAccountName.SelectedIndex = -1;

            BeginInvoke((Action)(() =>
            {
                if (!lstAccountSuggestions.Focused)
                    lstAccountSuggestions.Visible = false;
            }));
        }

        private void txtCreditAccountCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
            DBClass.CreateParameter("code", txtDebitAccountCode.Text)))
                if (!reader.Read())
                    cmbCreditAccountName.SelectedIndex = -1;

            BeginInvoke((Action)(() =>
            {
                if (!lstAccountSuggestions.Focused)
                    lstAccountSuggestions.Visible = false;
            }));
        }

        private void cmbMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMethod.Text == "Cash")
            {
                pnlBank.Visible = pnlTrans.Visible = false;
                cmbBankName.SelectedValue = -1;
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Default Account For Cash")))
                    if (reader.Read())
                    {
                        txtDebitAccountCode.Text = reader["code"].ToString();
                        string accountId = reader["id"].ToString();
                        if (!string.IsNullOrEmpty(accountId))
                        {
                            cmbDebitAccountName.SelectedValue = int.Parse(accountId);
                        }
                    }
            }
            else if (cmbMethod.Text == "Cheque")
            {
                pnlBank.Visible = true;
                pnlTrans.Visible = false;
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "PDC Receivable")))
                    if (reader.Read())
                    {
                        txtDebitAccountCode.Text = reader["code"].ToString();
                        string accountId = reader["id"].ToString();
                        if (!string.IsNullOrEmpty(accountId))
                        {
                            cmbDebitAccountName.SelectedValue = int.Parse(accountId);
                        }
                    }
            }
            else
            {
                pnlTrans.Visible = true;
                pnlBank.Visible = false;
            }
            AutoFillCheckNameCustomer();
        }

        private void dt_check_date_ValueChanged(object sender, EventArgs e)
        {
            if (dt_check_date.Focused)
                CheckDateCheck();
        }

        private void CheckDateCheck()
        {
            if (!pnlBank.Visible)
                return;

            if (cmbMethod.Text == "Cheque")
            {
                if (dt_check_date.Value.Date == DateTime.Now.Date)
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_coa_level_4 WHERE main_id = (SELECT id FROM tbl_coa_level_3 WHERE NAME = 'Banks') LIMIT 1;"))
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
                else
                {
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
                }
            }
            else
            {
                dt_check_date.Enabled = false;
                cmbBankName.Enabled = false;
                txtCheckName.Enabled = false;
            }
        }

        bool isBankNameRefreshing = false;
        private void cmbBankName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBankName.SelectedValue == null)
            {
                txtBankCode.Text = "";
                return;
            }
            if (cmbBankName.Focused)
            {
                if (isBankNameRefreshing) return;
                if (cmbBankName.SelectedValue != null &&
            (cmbBankName.SelectedValue.ToString() == "0" || cmbBankName.Text == "<< Add >>"))
                {
                    var frm = new frmBankRegister(0);
                    frm.ShowDialog();
                    isBankNameRefreshing = true;
                    BindCombos.PopulateRegisterBanks(cmbBankName, true);
                    cmbBankName.SelectedIndex = cmbBankName.Items.Count - 1;
                    isBankNameRefreshing = false;
                }
            }

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_bank where id = " + cmbBankName.SelectedValue.ToString()))
                if (reader.Read())
                    txtBankCode.Text = reader["code"].ToString();
                else
                    txtBankCode.Text = "";
        }

        private void dgvInv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInv.CurrentRow == null)
                return;

            if (e.ColumnIndex == dgvInv.Columns["chkpay"].Index)
            {
                if (dgvInv.CurrentRow.Cells["chkpay"].Value.ToString() == "True")
                    dgvInv.CurrentRow.Cells["pay"].Value = dgvInv.CurrentRow.Cells["total"].Value.ToString();
                else
                    dgvInv.CurrentRow.Cells["pay"].Value = "0";
            }
            if (e.ColumnIndex == dgvInv.Columns["pay"].Index)
            {
                if (dgvInv.CurrentRow.Cells["pay"].Value.ToString() == "" || dgvInv.CurrentRow.Cells["total"].Value == null)
                    return;

                decimal pay = decimal.Parse(dgvInv.CurrentRow.Cells["pay"].Value.ToString());
                decimal total = decimal.Parse(dgvInv.CurrentRow.Cells["total"].Value.ToString());
                if (pay > 0)
                {
                    if (decimal.Parse(dgvInv.CurrentRow.Cells["pay"].Value.ToString()) > decimal.Parse(dgvInv.CurrentRow.Cells["total"].Value.ToString()))
                        dgvInv.CurrentRow.Cells["pay"].Value = dgvInv.CurrentRow.Cells["total"].Value.ToString();
                }
                else if (pay < 0)
                {
                    if (pay < total)
                        dgvInv.CurrentRow.Cells["pay"].Value = dgvInv.CurrentRow.Cells["total"].Value.ToString();
                }
            }
            calculateTotal();
        }
        private void calculateTotal()
        {
            for (int i = 0; i < dgvInv.Rows.Count; i++)
            {
                if (dgvInv.Rows[i].Cells["pay"].Value.ToString() == "" || dgvInv.CurrentRow.Cells["total"].Value.ToString() == "")
                    continue;
                totalAmount += decimal.Parse(dgvInv.Rows[i].Cells["pay"].Value.ToString());
            }
            txtAmount.Text = totalAmount.ToString();
            totalAmount = 0;
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

        bool isDebitAccountNameRefreshing = false;
        private void cmbDebitAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDebitAccountName.SelectedValue == null)
            {
                txtDebitAccountCode.Text = "";
                return;
            }

            if (cmbDebitAccountName.Focused && int.Parse(cmbDebitAccountName.SelectedValue.ToString()) == -1)
            {
                new frmAddAccount().ShowDialog();
                isDebitAccountNameRefreshing = true;
                BindCombos.PopulateAllLevel4Account(cmbDebitAccountName, true);
                cmbDebitAccountName.SelectedIndex = cmbDebitAccountName.Items.Count - 1;
                isDebitAccountNameRefreshing = false;
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbDebitAccountName.SelectedValue.ToString()))
                    if (reader.Read())
                        txtDebitAccountCode.Text = reader["code"].ToString();
                    else
                        txtDebitAccountCode.Text = "";
            }
        }

        bool isCreditAccountNameRefreshing = false;
        private void cmbCreditAccountName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCreditAccountName.SelectedValue == null)
            {
                txtCreditAccountCode.Text = "";
                return;
            }
            if (cmbCreditAccountName.Focused && int.Parse(cmbCreditAccountName.SelectedValue.ToString()) == -1)
            {
                new frmAddAccount().ShowDialog();

                isCreditAccountNameRefreshing = true;
                BindCombos.PopulateAllLevel4Account(cmbCreditAccountName, true);
                cmbCreditAccountName.SelectedIndex = cmbCreditAccountName.Items.Count - 1;
                isCreditAccountNameRefreshing = false;
            }
            else
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbCreditAccountName.SelectedValue.ToString()))
                    if (reader.Read())
                        txtCreditAccountCode.Text = reader["code"].ToString();
                    else
                        txtCreditAccountCode.Text = "";
            }
        }

        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {

            using (MySqlDataReader reader = DBClass.ExecuteReader("select id,CODE from tbl_customer where code =@code",
              DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (reader.Read())
                    cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());

            if (txtCustomerCode.Focused) {
                string input = txtCustomerCode.Text.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    lstAccountSuggestions.Visible = false;
                    return;
                }
                string query = @"SELECT code, name FROM tbl_customer 
                         WHERE code LIKE @search OR name LIKE @search LIMIT 20";

                DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

                lstAccountSuggestions.Items.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
                }
                if (lstAccountSuggestions.Items.Count > 0)
                {
                    Point locationOnForm = txtCustomerCode.Parent.PointToScreen(txtCustomerCode.Location);
                    Point locationRelativeToForm = this.PointToClient(locationOnForm);

                    lstAccountSuggestions.SetBounds(
                        locationRelativeToForm.X,
                        locationRelativeToForm.Y + txtCustomerCode.Height,
                        txtCustomerCode.Width + 100,
                        120
                    );

                    lstAccountSuggestions.Tag = txtCustomerCode;
                    lstAccountSuggestions.Visible = true;
                    lstAccountSuggestions.BringToFront();
                }
                else
                {
                    lstAccountSuggestions.Visible = false;
                }
            }
        }

        private void dgvInv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvInv.Columns[e.ColumnIndex].Name == "chkPay" && e.RowIndex >= 0)
            {
                bool isChecked = Convert.ToBoolean(dgvInv.Rows[e.RowIndex].Cells["chkPay"].Value);

                if (isChecked)
                {
                    dgvInv.Rows[e.RowIndex].Cells["chkPay"].Value = false;
                }
                else
                {
                    dgvInv.Rows[e.RowIndex].Cells["chkPay"].Value = true;
                }
            }
        }

        private void txtCustomerCode_Leave(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select id,CODE from tbl_customer where code =@code",
                DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (!reader.Read())
                    cmbCustomer.SelectedIndex = -1;

            BeginInvoke((Action)(() =>
            {
                if (!lstAccountSuggestions.Focused)
                    lstAccountSuggestions.Visible = false;
            }));
        }

        private void AutoFillCheckNameCustomer()
        {
            if (cmbMethod.Text != "Cheque")
                return;

            if (cmbPaymentType.Text == "Customer" && cmbCustomer.SelectedValue != null)
            {
                using (var reader = DBClass.ExecuteReader(
                    "SELECT name FROM tbl_customer WHERE id = @id",
                    DBClass.CreateParameter("@id", cmbCustomer.SelectedValue)))
                {
                    if (reader.Read())
                    {
                        txtCheckName.Text = reader["name"].ToString();
                    }
                }
            }
            else
            {
                txtCheckName.Text = "";
            }
        }


        public DataTable COMPANYINFO(int a1)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company limit 1");
        }

        public DataTable TransactionInfo(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY tr.id) AS sn,
                                    (SELECT Code FROM tbl_coa_level_4 WHERE id=tr.account_id)as acCode,
                                    (SELECT NAME FROM tbl_coa_level_4 WHERE id=tr.account_id)as acName,
                                     tr.debit,tr.credit,
                                    (SELECT NAME FROM tbl_sub_cost_center WHERE id = (select cost_center_id FROM tbl_receipt_voucher_details WHERE payment_id=r.id LIMIT 1)) AS centerName 
                                     FROM tbl_transaction tr,tbl_receipt_voucher r WHERE tr.transaction_id=r.id and r.id=@id AND tr.TYPE = 'Customer Receipt'; ",DBClass.CreateParameter("id",id));
                    
        }

        public DataTable ReceiptVoucher(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT id,date,CODE AS RvNo,type,method,amount,
                                (SELECT code FROM tbl_customer WHERE id=(SELECT hum_id FROM tbl_receipt_voucher_details WHERE payment_id = tbl_receipt_voucher.id)) reciverCode,
                                (SELECT name FROM tbl_customer WHERE id=(SELECT hum_id FROM tbl_receipt_voucher_details WHERE payment_id = tbl_receipt_voucher.id)) reciverName,
                                (SELECT code FROM tbl_coa_level_4 WHERE id=debit_account_id) debitAccountcode,
                                (SELECT NAME FROM tbl_coa_level_4 WHERE id=debit_account_id) debitAccount,
                                (SELECT NAME FROM tbl_sub_cost_center WHERE id=debit_cost_center_id) debitCostCenter,
                                (SELECT code FROM tbl_coa_level_4 WHERE id=credit_account_id) creditAccountcode,
                                (SELECT NAME FROM tbl_coa_level_4 WHERE id=credit_account_id) creditAccount,
                                (select name FROM tbl_sub_cost_center WHERE id=credit_cost_center_id) creditCostCenter,
                                description description,(select name FROM tbl_bank WHERE id = bank_id) bankName,
                                (select code FROM tbl_bank WHERE id = bank_id) bankAccountcode,
                                (select account_name FROM tbl_bank_card WHERE id = bank_account) bankAccount,
                                check_name,check_no,check_date,trans_date,trans_name,trans_ref 
                                FROM tbl_receipt_voucher where id=@id;", DBClass.CreateParameter("id", id));
        }

        public DataTable ReceiptDetail(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY d.id) AS sn,d.date,d.inv_id,
                                CASE
                                    WHEN i.type = 'Customer' THEN(SELECT NAME FROM tbl_customer WHERE id = d.hum_id)
                                    WHEN i.type = 'Vendor' THEN (SELECT NAME FROM tbl_vendor WHERE id = d.hum_id)
                                    WHEN i.type = 'Employee' THEN (SELECT NAME FROM tbl_employee WHERE id = d.hum_id)
                                    WHEN i.type = 'General' THEN (SELECT NAME FROM tbl_coa_level_4 WHERE id = d.hum_id)
                                    ELSE 'Unregistered'
                                END AS NAME,d.hum_id,d.inv_code,d.total,d.payment,d.description,(SELECT NAME FROM tbl_sub_cost_center WHERE id = d.cost_center_id) AS costCenterName
                            FROM tbl_receipt_voucher_details d,tbl_receipt_voucher i WHERE d.payment_id = i.id AND i.id= @receiptId;", DBClass.CreateParameter("@receiptId", a1));
        }

        public void ShowReport()
        {

            try
            {
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "ReceiptVoucher.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                // Load the main report data
                DataTable companyData = COMPANYINFO(1);  // Assuming you want to pass ID 1
                DataTable transactionData = TransactionInfo(id.ToString());
                DataTable receiptVoucherData = ReceiptVoucher(id.ToString());
                DataTable receiptDetailData = ReceiptDetail(id.ToString());
                if (companyData != null)  // Ensure that data was successfully retrieved
                {
                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["ReceiptHeader"].SetDataSource(receiptVoucherData);
                    cr.Subreports["AccountHeader"].SetDataSource(transactionData);
                    cr.Subreports["ChequeHeader"].SetDataSource(receiptVoucherData);
                    cr.Subreports["ReceiptDetails"].SetDataSource(receiptDetailData);
                    ((TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text2"]).Text = txtDescription.Text;
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;  // Exit the method if no data is available
                }

                MasterReportView reportForm = new MasterReportView();
                reportForm.crReportViewer.ReportSource = cr;
                reportForm.crReportViewer.RefreshReport();
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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

        private void pnlBank_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            calculateTotal();
            if (id == 0)
            {
                if (insertRV())
                {
                    EventHub.RefreshReceiptVoucher();
                    this.Close();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
            else
            {
                if (updateRV())
                {
                    EventHub.RefreshReceiptVoucher();
                    this.Close();
                    if (chkPrint.Checked == true)
                    {
                        ShowReport();
                    }
                }
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void panel6_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int? currentId = Convert.ToInt32(txtId.Text); // Utilities.GetVoucherIdFromCode(txtPVCode.Text);
            if (currentId == null || currentId <= 1)
                return;

            currentId = currentId - 1;
            txtId.Text = currentId.ToString();
            if (currentId <= 0)
            {
                clear();
                MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "SELECT id FROM tbl_receipt_voucher WHERE state = 0 AND id = @id";

            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
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
            int? currentId = Convert.ToInt32(txtId.Text); // Utilities.GetVoucherIdFromCode(txtPVCode.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            txtId.Text = currentId.ToString();
            string query = "SELECT id FROM tbl_receipt_voucher WHERE state = 0 AND id = @id";
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtPVCode.Text = GenerateNextReceiptCode();
            clear();
        }
        private void clear()
        {
            dgvInv.Rows.Clear();
            defaultCustomerId = 0;
            totalAmount = 0;
            id = 0;
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // 1. Read all records before deletion
            DataTable dtReceipt = DBClass.ExecuteDataTable(
                "SELECT * FROM tbl_receipt_voucher WHERE id = @id",
                DBClass.CreateParameter("id", id.ToString())
            );

            DataTable dtDetails = DBClass.ExecuteDataTable(
                "SELECT * FROM tbl_receipt_voucher_details WHERE payment_id = @id",
                DBClass.CreateParameter("id", id.ToString())
            );

            DataTable dtTransaction = DBClass.ExecuteDataTable(
                "SELECT * FROM tbl_transaction WHERE transaction_id = @id AND t_type='RECEIPT'",
                DBClass.CreateParameter("id", id.ToString())
            );

            // 2. Backup into tbl_deleted_records
            void BackupData(DataTable dt, string tableName)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DBClass.ExecuteNonQuery(
                        "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                        DBClass.CreateParameter("table", tableName),
                        DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                        DBClass.CreateParameter("user", frmLogin.userId.ToString())
                    );
                }
            }

            BackupData(dtReceipt, "tbl_receipt_voucher");
            BackupData(dtDetails, "tbl_receipt_voucher_details");
            BackupData(dtTransaction, "tbl_transaction");

            // 3. Permanently delete records
            DBClass.ExecuteNonQuery(
                "DELETE FROM tbl_transaction WHERE transaction_id = @id AND t_type='RECEIPT'",
                DBClass.CreateParameter("id", id.ToString())
            );
            DBClass.ExecuteNonQuery(
                "DELETE FROM tbl_receipt_voucher_details WHERE payment_id = @id",
                DBClass.CreateParameter("id", id.ToString())
            );
            DBClass.ExecuteNonQuery(
                "DELETE FROM tbl_receipt_voucher WHERE id = @id",
                DBClass.CreateParameter("id", id.ToString())
            );

            // 4. Log and clear
            Utilities.LogAudit(frmLogin.userId, "Receipt Voucher Permanently Deleted", "Receipt Voucher", id, "Deleted Receipt Voucher with ID: " + id);
            clear();
        }


        private void btnCopy_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        bool isDebitCostCenterRefreshing = false;

        private void cmbDebitCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbDebitCostCenter.Focused)
                return;

            if (isDebitCostCenterRefreshing) return;
            if (cmbDebitCostCenter.SelectedValue != null &&
        (cmbDebitCostCenter.SelectedValue.ToString() == "0" || cmbDebitCostCenter.Text == "<< Add >>"))
            {
                var frm = new frmCostCenter(0);
                frm.ShowDialog();

                isDebitCostCenterRefreshing = true;
                BindCombos.PopulateCostCenter(cmbDebitCostCenter, true);
                cmbDebitCostCenter.SelectedIndex = cmbDebitCostCenter.Items.Count - 1;
                isDebitCostCenterRefreshing = false;
            }
        }

        bool isCreditCostCenterRefreshing = false;

        private void cmbCreditCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmbCreditCostCenter.Focused)
                return;
            if (isCreditCostCenterRefreshing) return;

            if (cmbCreditCostCenter.SelectedValue != null &&
        (cmbCreditCostCenter.SelectedValue.ToString() == "0" || cmbCreditCostCenter.Text == "<< Add >>"))
            {
                var frm = new frmCostCenter(0);
                frm.ShowDialog();
                isCreditCostCenterRefreshing = true;
                BindCombos.PopulateCostCenter(cmbCreditCostCenter, true);
                cmbCreditCostCenter.SelectedIndex = cmbCreditCostCenter.Items.Count - 1;
                isCreditCostCenterRefreshing = false;
            }
        }

        private void lstAccountSuggestions_Click(object sender, EventArgs e)
        {
            if (lstAccountSuggestions.SelectedItem != null && lstAccountSuggestions.Tag is Guna2TextBox targetTextBox)
            {
                string selected = lstAccountSuggestions.SelectedItem.ToString();
                string selectedCode = selected.Split('-')[0].Trim();

                targetTextBox.Text = selectedCode;
                lstAccountSuggestions.Visible = false;

                // Trigger TextChanged manually if needed
                targetTextBox.Focus();
                targetTextBox.SelectionStart = targetTextBox.Text.Length;

                // Example of post-selection logic:
                if (targetTextBox == txtDebitAccountCode)
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                        DBClass.CreateParameter("code", selectedCode)))
                        if (reader.Read())
                            cmbDebitAccountName.SelectedValue = int.Parse(reader["id"].ToString());
                }
                else if (targetTextBox == txtCreditAccountCode)
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                        DBClass.CreateParameter("code", selectedCode)))
                        if (reader.Read())
                            cmbCreditAccountName.SelectedValue = int.Parse(reader["id"].ToString());
                }
                else if (targetTextBox == txtCustomerCode)
                {
                    using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_customer where code =@code",
                        DBClass.CreateParameter("code", selectedCode)))
                        if (reader.Read())
                            cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());
                }
            }
        }

        private void dgvInv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvInv.CurrentCell.ColumnIndex == dgvInv.Columns["Pay"].Index)
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    txt.KeyPress -= Txt_KeyPress;
                    txt.KeyPress += Txt_KeyPress;
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
    }
}