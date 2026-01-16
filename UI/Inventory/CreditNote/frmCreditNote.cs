using CrystalDecisions.CrystalReports.Engine;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Reports;
using YamyProject.UI.Reports.Design;

namespace YamyProject
{
    public partial class frmCreditNote : Form
    {
        private EventHandler customerUpdatedHandler;

        decimal invId;
        int level4CustomerId, level4VatId, level4SalesReturn, level4COGS, level4Inventory;
        int id;

        public frmCreditNote(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            customerUpdatedHandler = (sender, args) => BindCombos.PopulateCustomers(cmbCustomer);
            this.id = id;
            headerUC1.FormText = id == 0 ? "Credit Note - New" : "Credit Note - Edit";
        }
     
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmCreditNote_Load(object sender, EventArgs e)
        {
            dtInv.Value = DateTime.Now.Date;
            bindCombo();
            txtInvoiceId.Text = GenerateNextSalesCode();
            if (id != 0)
            {
                BindInvoice();
                btnSave.Enabled = UserPermissions.canEdit("Credit Note");
            }
        }

        private string GenerateNextSalesCode()
        {
            string newCode = "CN-0001"; 

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(invoice_id, 4) AS UNSIGNED)) AS lastCode FROM tbl_credit_note"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "CN-" + code.ToString("D4"); 
                }
            }

            return newCode;
        }
        string invCode;
        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_credit_note where id = @id",
                DBClass.CreateParameter("id", id)))
                if (reader.Read())
                {
                    dtInv.Value = DateTime.Parse(reader["date"].ToString());
                    
                        //load customer
                        BindCombos.PopulateCustomers(cmbCustomer);
                        cmbCustomer.SelectedValue = reader["credit_account"].ToString();
                    
                    cmbAccountName.SelectedValue = reader["debit_account"].ToString();
                    txtInvoiceId.Text = reader["invoice_id"].ToString();
                    txtAmount.Text = reader["amount"].ToString();
                    invCode = txtInvoiceId.Text = reader["invoice_id"].ToString();
                    invId = id;
                    txtVat.Text = reader["vat"].ToString();
                    txtTotalAmount.Text = reader["total"].ToString();
                    txtDescription.Text = reader["Description"].ToString();
                    BindInvoiceItems();
                    CalculateTotal();
                }
        }
        private void BindInvoiceItems()
        {
            dgvItems.Rows.Clear();
            int count = 1;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_credit_note_details Where ref_id = @id;",
                                                            DBClass.CreateParameter("id", id)))
            while (reader.Read())
            {
                DateTime date = Convert.ToDateTime(reader["invoice_date"]);
                string dated = date.ToString("dd/MM/yyyy");
                dgvItems.Rows.Add(count.ToString(), reader["invoice_date"].ToString(), dated, reader["inv_no"].ToString(), reader["invoice_id"].ToString(), reader["total"].ToString(), reader["vat"].ToString(), reader["balance"].ToString(),true, reader["amount"].ToString(), reader["remaining"].ToString());
                count++;
            }
        }
        
        private void bindCombo()
        {

            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            cmbAccountName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Invoice Payment Cash Method");
            loadAccounts();
            level4COGS = BindCombos.SelectDefaultLevelAccount("COGS");
            level4Inventory = BindCombos.SelectDefaultLevelAccount("Inventory");
        }
        private void loadAccounts()
        {
            BindCombos.PopulateCustomers(cmbCustomer);
            level4CustomerId = BindCombos.SelectDefaultLevelAccount("Customer");
            level4VatId = BindCombos.SelectDefaultLevelAccount("Vat Output");
            level4SalesReturn = BindCombos.SelectDefaultLevelAccount("SalesReturn");
            //level4VatId = BindCombos.SelectDefaultLevelAccount("Vat Input");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Utilities.AreDefaultAccountsSet(new List<string> { "Sales", "Vendor", "COGS", "Customer", "Vat Input", "Vat Output", "Inventory" }))
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (id == 0)
            {
                if (insertInvoice())
                {
                    EventHub.RefreshCreditNote();
                    if (checkBox4.Checked == true)
                    {
                        loadPrint();
                    }
                    MessageBox.Show("The Credit Saved");
                    dgvItems.Rows.Clear();

                }
            }
            else
            {
                if (updateInvoice())
                {
                    EventHub.RefreshCreditNote();
                    if (checkBox4.Checked == true)
                    {
                        loadPrint();
                    }
                    MessageBox.Show("The Credit Updated");
                    dgvItems.Rows.Clear();
                }
            }
        }
        private void loadPrint()
        {
            //DialogResult result = MessageBox.Show("Do You want To Show This Bill ",
            //                                    "Confirmation",
            //                                    MessageBoxButtons.YesNo,
            //                                    MessageBoxIcon.Question);
            //// Check if the user clicked Yes or No
            //if (result == DialogResult.Yes)
            //{
                // Code for when the user clicks "Yes"

                
                ShowReport();

            //}
            //else if (result == DialogResult.No)
            //{
            //    // Code for when the user clicks "No"
            //    this.Close();
            //}
        }
        private bool updateInvoice()
        {
            if (!chkRequiredDate())
                return false;
            DBClass.ExecuteNonQuery(@"UPDATE tbl_credit_note 
                                     SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date, credit_account = @creditAccount,debit_account=@debitAccount, invoice_id = @invoice_id,
                                     amount = @amount, vat = @vat, total = @total, 
                                     description = @description WHERE id = @id;",
                 DBClass.CreateParameter("id", id),
                  DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("creditAccount", cmbCustomer.SelectedValue),
               DBClass.CreateParameter("debitAccount", cmbAccountName.SelectedValue == null ? "0" : cmbAccountName.SelectedValue.ToString()),
               DBClass.CreateParameter("invoice_id", invCode),
               DBClass.CreateParameter("amount", txtAmount.Text),
               DBClass.CreateParameter("vat", txtVat.Text),
               DBClass.CreateParameter("total", txtTotalAmount.Text),
               DBClass.CreateParameter("description", txtDescription.Text),
               DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
               DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));

            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_credit_note_details WHERE ref_Id = @id", DBClass.CreateParameter("id", id));
            insertInvItems();
            CommonInsert.DeleteTransactionEntry(id, "Credit Note");
            addTransaction();
            Utilities.LogAudit(frmLogin.userId, "Update Credit Note", "Credit Note", id, "Updated Credit Note: " + invCode);

            return true;
        }

        private bool insertInvoice()
        {
            if (!chkRequiredDate())
                return false;
            invCode = GenerateNextSalesCode();
            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_credit_note (date, credit_account,debit_account,type,invoice_id,
                    amount,vat,total, description,created_by, created_date, state) VALUES (@date, @creditAccount,@debitAccount,@type,@invoice_id,
                   @amount,  @vat,@total, @description,@created_by, @created_date, @state);
                   SELECT LAST_INSERT_ID();",
               DBClass.CreateParameter("date", dtInv.Value.Date),
               DBClass.CreateParameter("creditAccount", cmbCustomer.SelectedValue),
               DBClass.CreateParameter("debitAccount", cmbAccountName.SelectedValue),
               DBClass.CreateParameter("type", "Customer"),
               DBClass.CreateParameter("invoice_id", invCode),
               DBClass.CreateParameter("amount", txtAmount.Text),
               DBClass.CreateParameter("vat", txtVat.Text),
               DBClass.CreateParameter("total", txtTotalAmount.Text),
               DBClass.CreateParameter("description", txtDescription.Text),
               DBClass.CreateParameter("created_by", frmLogin.userId),
               DBClass.CreateParameter("created_date", DateTime.Now.Date),
               DBClass.CreateParameter("state", 0)).ToString());

            insertInvItems();
            txtInvoiceId.Text = invCode.ToString();
            addTransaction();
            Utilities.LogAudit(frmLogin.userId, "Add Credit Note", "Credit Note", (int)invId, "Added Credit Note: " + invCode);

            return true;
        }
        private void addTransaction() {
            CommonInsert.addTransactionEntry(dtInv.Value.Date, level4CustomerId.ToString(),
                 "0", txtTotalAmount.Text, invId.ToString(), cmbCustomer.SelectedValue.ToString(), "Credit Note", "Credit Note", "Credit Note NO. " + invCode,
                 frmLogin.userId, DateTime.Now.Date,txtInvoiceId.Text);

            if (decimal.Parse(txtVat.Text) > 0)
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
              level4VatId.ToString(), txtVat.Text, "0", invId.ToString(), "0", "Credit Note", "Credit Note",
              "Vat Output For Invoice No. " + invCode, frmLogin.userId, DateTime.Now.Date,txtCustomerCode.Text);
            CommonInsert.addTransactionEntry(dtInv.Value.Date,
              level4SalesReturn.ToString(), txtAmount.Text, "0", invId.ToString(), "0", "Credit Note", "Credit Note",
              "Revenue For Invoice No. " + invCode, frmLogin.userId, DateTime.Now.Date,txtInvoiceId.Text);
        }

        private void insertInvItems()
        {
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (Convert.ToBoolean(row.Cells["select"].Value))
                {
                    int _invId = Convert.ToInt32(row.Cells["invoice_id"].Value);
                    decimal _total = row.Cells["total"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["total"].Value);
                    decimal _amount = row.Cells["amount"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["amount"].Value);
                    decimal _vat = row.Cells["vat"].Value == DBNull.Value ? 0 : Convert.ToDecimal(row.Cells["vat"].Value);
                    string _type = "SALES";
                    DateTime _invDate = DateTime.Parse(row.Cells["inv_dated"].Value.ToString());
                    string _invNo = row.Cells["invoice_no"].Value.ToString();
                    string _balance = row.Cells["balance"].Value.ToString();
                    string _remaining = row.Cells["remaining"].Value.ToString();
                    
                    DBClass.ExecuteNonQuery(@"
                        INSERT INTO tbl_credit_note_details(ref_id, inv_no, invoice_id, invoice_date, invoice_type,total,vat,amount,balance,remaining)
                        VALUES (@refId, @inv_no, @invoiceId, @invoiceDate, @invoiceType, @total, @vat,@amount,@balance,@remaining);",
                        DBClass.CreateParameter("refId", invId),
                        DBClass.CreateParameter("inv_no", _invNo),
                        DBClass.CreateParameter("invoiceId", _invId),
                        DBClass.CreateParameter("invoiceDate", _invDate.Date),
                        DBClass.CreateParameter("invoiceType", _type),
                        DBClass.CreateParameter("total", _total),
                        DBClass.CreateParameter("vat", _vat),
                        DBClass.CreateParameter("amount", _amount),
                        DBClass.CreateParameter("balance", _balance),
                        DBClass.CreateParameter("remaining", _remaining)
                    );
                    object result = DBClass.ExecuteScalar(
                          "SELECT SUM(`change`) FROM tbl_sales WHERE id = @id",
                          DBClass.CreateParameter("id", _invId)
                      );
                    decimal totalPaid = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                    DBClass.ExecuteNonQuery(
                           "UPDATE tbl_sales SET pay = @pay, `change` = @change WHERE id = @id",
                           DBClass.CreateParameter("pay", totalPaid),
                           DBClass.CreateParameter("change", _remaining),
                           DBClass.CreateParameter("id", _invId)
                       );
                    Utilities.LogAudit(frmLogin.userId, "Add Credit Note Item", "Credit Note", (int)invId, 
                        $"Added Credit Note Item: Invoice No. {_invNo}, Invoice ID: {_invId}, Total: {_total}, VAT: {_vat}, Amount: {_amount}");
                }
            }
        }

        private bool chkRequiredDate()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["total"].Value == null
                    || dgvItems.Rows[i].Cells["total"].Value.ToString() == ""
                    || decimal.Parse(dgvItems.Rows[i].Cells["total"].Value.ToString()) == 0)
                {
                    MessageBox.Show("Total Item In Row " + (dgvItems.Rows[i].Index + 1) + " Can't Be 0 or Null");
                    return false;
                }
            }
            if (cmbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Customer Must be Selected.");
                txtCustomerCode.Focus();
                return false;
            }
            //if (dgvItems.Rows.Count == 1)
            //{
            //    MessageBox.Show("Insert Items First.");
            //    return false;
            //}

            if (level4CustomerId == 0 | level4VatId == 0 | level4SalesReturn == 0 || level4COGS == 0 | level4Inventory == 0)
            {
                MessageBox.Show("Default accounts for invoice are not properly configured. Please check your settings.", "Error");
                return false;
            }
            if (txtTotalAmount.Text == "" || decimal.Parse(txtTotalAmount.Text) == 0)
            {
                MessageBox.Show("Total Must Be Bigger Than Zero");
                return false;
            }

            if (string.IsNullOrEmpty(txtVat.Text))
            {
                txtVat.Text = "0";
            }
            return true;
        }
        private void resetTextBox()
        {
            txtAmount.Text = txtInvoiceId.Text = txtTotalAmount.Text = txtVat.Text = "";
            dtInv.Value = DateTime.Now;
            dgvItems.Rows.Clear();
        }
        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
                e.Handled = true;
        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedValue == null)
                return;

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where id = " + cmbCustomer.SelectedValue.ToString()))
                if (reader.Read())
                {
                    txtCustomerCode.Text = reader["code"].ToString();
                }
        }
        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadAccounts();
        }
        private void txtCustomerCode_TextChanged(object sender, EventArgs e)
        {

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where code =@code",
            DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (reader.Read())
                    cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());

        }
        private void txtCustomerCode_Leave(object sender, EventArgs e)
        {

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where code =@code",
            DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (!reader.Read())
                    cmbCustomer.SelectedIndex = -1;
        }
        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            chkRowValidty();
            CalculateTotal();
        }
        private void chkRowValidty()
        {
            decimal invoiceTotal = GetDecimalValue(dgvItems.CurrentRow, "total");
            decimal payAmount = GetDecimalValue(dgvItems.CurrentRow, "balance");
            decimal aAmount = GetDecimalValue(dgvItems.CurrentRow, "amount");

            if (invoiceTotal == 0 || payAmount == 0)
                dgvItems.CurrentRow.Cells["amount"].Value = dgvItems.CurrentRow.Cells["remaining"].Value = "0";
            else
            {
                if (dgvItems.CurrentRow.Cells["select"].Value != null && bool.Parse(dgvItems.CurrentRow.Cells["select"].Value.ToString()) == true)
                {
                    decimal payableAmount = GetDecimalValue(dgvItems.CurrentRow, "amount");
                    if (payableAmount <= 0)
                    {
                        dgvItems.CurrentRow.Cells["amount"].Value = payableAmount;
                    }
                    dgvItems.CurrentRow.Cells["remaining"].Value = payAmount -payableAmount;
                }
            }
        }
        void CalculateTotal()
        {
            decimal total = 0, vat = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["select"].Value != null && bool.Parse(row.Cells["select"].Value.ToString()) == true)
                {
                    if (row.Cells["amount"].Value != null)
                        total += Convert.ToDecimal(row.Cells["amount"].Value);
                    if (row.Cells["vat"].Value != null && row.Cells["vat"].Value.ToString().Trim() != "")
                        vat += Convert.ToDecimal(row.Cells["vat"].Value);

                    //if(Convert.ToDecimal(row.Cells["amount"].Value)+ Convert.ToDecimal(row.Cells["vat"].Value) != Convert.ToDecimal(row.Cells["total"].Value))
                    //{
                    //    //remaining
                    //    decimal amt = Convert.ToDecimal(row.Cells["amount"].Value);
                    //    decimal vatAmt = (amt * 5 / 100);
                    //    vat += vatAmt;
                    //    total += amt;
                    //}
                }

            }
            txtAmount.Text = (total - vat).ToString("0.000");
            txtVat.Text = vat.ToString("0.000");
            txtTotalAmount.Text = total.ToString("0.000");
        }
        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 0)
            {
                var row = dgvItems.Rows[e.RowIndex];
                decimal invoiceTotal = GetDecimalValue(row, "total");
                decimal invoiceVat = GetDecimalValue(row, "vat");

                chkRowValidty();
                CalculateTotal();
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

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceId.Text))
                return;

            int? currentId = Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId == null || currentId <= 1)
                return;

            currentId = currentId - 1;
            if (currentId <= 0)
            {
                clear();
                MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "select id from tbl_credit_note where state = 0 and id =@id";
            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clear();
                    MessageBox.Show("No previous record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceId.Text))
                return;

            int? currentId = Utilities.GetVoucherIdFromCode(txtInvoiceId.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            string query = "SELECT id FROM tbl_credit_note WHERE state = 0 AND id =@id";

            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindInvoice();
                }
                else
                {
                    clear();
                    MessageBox.Show("No next record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void lnkNewCustomer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewCustomer(0));
        }

        private void frmCreditNote_FormClosing(object sender, FormClosingEventArgs e)
        {
            //EventHub.Customer -= customerUpdatedHandler;
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            laodInvoice();
        }

        private void laodInvoice()
        {
            dgvItems.Rows.Clear();
            string query = @"SELECT 
                                s.id,
                                s.date,
                                s.invoice_id,
                                s.total,
                                s.net AS 'Total With VAT',
                                s.vat,
                                s.change AS Remaining
                            FROM 
                                tbl_sales s
                            INNER JOIN 
                                tbl_transaction t 
                                ON t.transaction_id = s.id 
                                AND t.`type` IN ('Customer Opening Balance', 'Sales Invoice') 
                                AND t.hum_id = s.customer_id
                            WHERE 
                                s.customer_id = @id
                            GROUP BY 
                                s.id, s.date, s.invoice_id, s.total, s.net, s.vat
                            HAVING 
                                Remaining > 0;
                                ";
            
            int count = 1;
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString())))
            while (reader.Read())
            {
                DateTime date = Convert.ToDateTime(reader["date"]);
                string dated = date.ToString("dd/MM/yyyy");
                    dgvItems.Rows.Add(
                        count.ToString(),
                        reader["date"].ToString(),
                        dated,
                        reader["invoice_id"].ToString(),
                        reader["id"].ToString(),
                        reader["Total With VAT"].ToString(),
                        reader["vat"].ToString(),
                        reader["Remaining"].ToString(),   // Now this is calculated properly
                        false,
                        reader["Remaining"].ToString()
                    );
                    count++;
            }

        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && txtAmount.Text.Contains("."))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '-' && txtAmount.SelectionStart != 0)
            {
                e.Handled = true;
            }
        }

        private void txtVat_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && txtVat.Text.Contains("."))
            {
                e.Handled = true;
            }
            
            if (e.KeyChar == '-' && txtVat.SelectionStart != 0)
            {
                e.Handled = true;
            }
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvItems.Columns[e.ColumnIndex].Name == "select" && e.RowIndex >= 0)
            {
                bool isChecked = Convert.ToBoolean(dgvItems.Rows[e.RowIndex].Cells["select"].Value);

                if (isChecked)
                {
                    dgvItems.Rows[e.RowIndex].Cells["select"].Value = false;
                }
                else
                {
                    dgvItems.Rows[e.RowIndex].Cells["select"].Value = true;
                }
            }
        }
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            {
                dgvItems.Rows.Remove(dgvItems.CurrentRow);
                CalculateTotal();
            }
            else if (dgvItems.Rows.Count > 0 && dgvItems.CurrentRow.Cells["view_Item"].ColumnIndex == e.ColumnIndex)
            {
                int refId = int.Parse(dgvItems.CurrentRow.Cells["invoice_id"].Value.ToString());
                ShowItemsData(refId);
            }
            else if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["select"].ColumnIndex == e.ColumnIndex)
            {
                decimal invoiceTotal = GetDecimalValue(dgvItems.CurrentRow, "total");
                decimal payAmount = GetDecimalValue(dgvItems.CurrentRow, "balance");
                decimal payableAmount = GetDecimalValue(dgvItems.CurrentRow, "amount");
                if (invoiceTotal > 0)
                {
                    dgvItems.CurrentRow.Cells["amount"].Value = payAmount;
                    dgvItems.CurrentRow.Cells["remaining"].Value = invoiceTotal;
                }
            }
        }
        private void ShowItemsData(int refId)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(
                    @"SELECT CONCAT(ti.code,' - ', ti.name) AS 'Item Name', 
                             ts.qty AS Qty, 
                             ts.price AS 'Price', 
                             ts.vatp AS Vat, 
                             ts.total AS Total 
                      FROM tbl_sales_details ts 
                      INNER JOIN tbl_items ti ON ts.item_id = ti.id 
                      WHERE sales_id = @id",
                    DBClass.CreateParameter("id", refId)))
            {
                // Create a dynamic form for showing the details in a table (DataGridView)
                Form detailForm = new Form
                {
                    Text = "Item Details",
                    Size = new System.Drawing.Size(800, 400),
                    StartPosition = FormStartPosition.CenterParent,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false
                };

                // Create a DataGridView to display the items in a tabular format
                DataGridView dgv = new DataGridView
                {
                    Left = 10,
                    Top = 10,
                    Width = 760,
                    Height = 250,
                    AutoGenerateColumns = false,  // We want to specify columns manually
                    ReadOnly = true,             // Make the entire DataGridView read-only
                    AllowUserToAddRows = false,  // Disable adding new rows by the user
                    AllowUserToDeleteRows = false, // Disable deleting rows
                    AllowUserToOrderColumns = false, // Disable reordering columns
                    AllowUserToResizeColumns = false, // Disable resizing columns
                    AllowUserToResizeRows = false, // Disable resizing rows
                    RowHeadersVisible = false
                };

                // Define columns for the DataGridView
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Item Name",
                    DataPropertyName = "ItemName",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                    //Width = 120
                });
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Quantity",
                    DataPropertyName = "Qty",
                    Width = 80,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "F2", Alignment = DataGridViewContentAlignment.MiddleRight } // Right align and format to 2 decimal places
                });
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Price",
                    DataPropertyName = "Price",
                    Width = 80,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "F2", Alignment = DataGridViewContentAlignment.MiddleRight } // Right align and format to 2 decimal places
                });
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "VAT",
                    DataPropertyName = "Vat",
                    Width = 60,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "F2", Alignment = DataGridViewContentAlignment.MiddleRight } // Right align and format to 2 decimal places
                });
                dgv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    HeaderText = "Total",
                    DataPropertyName = "Total",
                    Width = 80,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "F2", Alignment = DataGridViewContentAlignment.MiddleRight } // Right align and format to 2 decimal places
                });

                // Create a DataTable to hold the data
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ItemName");
                dataTable.Columns.Add("Qty");
                dataTable.Columns.Add("Price");
                dataTable.Columns.Add("Vat");
                dataTable.Columns.Add("Total");

                // Loop through the reader and fill the DataTable with items
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["ItemName"] = reader["Item Name"]?.ToString() ?? "";
                    row["Qty"] = reader["Qty"]?.ToString() ?? "";
                    row["Price"] = reader["Price"]?.ToString() ?? "";
                    row["Vat"] = reader["Vat"]?.ToString() ?? "";
                    row["Total"] = reader["Total"]?.ToString() ?? "";
                    dataTable.Rows.Add(row);
                }

                // Bind the DataTable to the DataGridView
                dgv.DataSource = dataTable;

                // Add the DataGridView to the form
                detailForm.Controls.Add(dgv);
                LocalizationManager.LocalizeDataGridViewHeaders(dgv);

                // Add a Close button
                Button btnClose = new Button
                {
                    Text = "Close",
                    Left = 200,
                    Top = dgv.Bottom + 10,
                    Width = 80
                };
                btnClose.Click += (s, args) => detailForm.Close();
                detailForm.Controls.Add(btnClose);

                // Show the form
                detailForm.ShowDialog();
            }
        }
        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }
        private void cmbPaymentTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }
        private void DecimalTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //
        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer where code =@code",
            DBClass.CreateParameter("code", txtCustomerCode.Text)))
                if (reader.Read())
                    cmbCustomer.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            if (txtVat.Focused)
            {
                if (txtVat.Text == "")
                {
                    txtVat.Text = "0";
                }
                decimal total = 0, vat = decimal.Parse(txtVat.Text);

                total = (decimal.Parse(txtAmount.Text) + vat);

                txtTotalAmount.Text = total.ToString("0.000");
            }
            else
            {
                CalculateTotal();
            }
        }

        private void guna2TileButton23_Click(object sender, EventArgs e)
        {
            txtInvoiceId.Text = GenerateNextSalesCode();
            clear();
        }

        private void clear()
        {
            resetTextBox();
            id = 0;
            invId = 0;
            invCode = "";
            cmbCustomer.SelectedIndex = -1;
            cmbAccountName.SelectedIndex = -1;
            txtCustomerCode.Text = "";
            dgvItems.Rows.Clear();
            txtInvoiceId.Text = "";
        }

        public DataTable COMPANYINFO(int a1)
        {
            return DBClass.ExecuteDataTable("SELECT * FROM tbl_company Limit 1");
        }
        public DataTable headerData(string a1)
        {
            return DBClass.ExecuteDataTable(@"select tbl_credit_note.* ,CASE WHEN TYPE = 'Customer' THEN (SELECT NAME FROM tbl_customer WHERE id = tbl_credit_note.credit_account) 
                                             WHEN type = 'Vedor' THEN (select Name from tbl_vendor  WHERE id = tbl_credit_note.credit_account)  ELSE '' END  AS NAME from tbl_credit_note 
                                              where id = @id;", DBClass.CreateParameter("@id", a1));
        }
        public DataTable detailsData(string a1)
        {
            return DBClass.ExecuteDataTable(@"select  * from tbl_credit_note_details where ref_id = @id;", DBClass.CreateParameter("@id", a1));
        }
        public void ShowReport()
        {
            try
            {
                // Create the report 
                //CPVCreditNote cr = new CPVCreditNote();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "CreditNote.rpt");
                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                // Load the main report data
                DataTable companyData = COMPANYINFO(1);  // Assuming you want to pass ID 1
                DataTable headData = headerData(invId.ToString());
                DataTable detailData = detailsData(invId.ToString());
                if (companyData != null)  // Ensure that data was successfully retrieved
                {
                    //cr.SetDataSource(companyData);
                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["VoucherHeader"].SetDataSource(headData);
                    cr.Subreports["VoucherDetails"].SetDataSource(detailData);
                    cr.Subreports["VoucherFooter"].SetDataSource(headData);
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;  // Exit the method if no data is available
                }

                // Assign the main report to the viewer
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
    }
}
