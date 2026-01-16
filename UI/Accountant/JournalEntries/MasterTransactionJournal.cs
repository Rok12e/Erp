using DocumentFormat.OpenXml.Office2010.Excel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterTransactionJournal : Form
    {
        private EventHandler JournalVoucherUpdatedHandler;
        string transactionId;
        string type;
        public MasterTransactionJournal(string transactionId, string type)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            JournalVoucherUpdatedHandler = (sender, args) => BindJournal();
            EventHub.Journal += JournalVoucherUpdatedHandler;
            this.type = type;
            this.transactionId = transactionId;
            headerUC1.FormText = this.Text;
        }

        private void MasterTransactionJournal_Load(object sender, EventArgs e)
        {
            lblJVNO.Text = !string.IsNullOrEmpty(transactionId) ? transactionId + " " + type : "";
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            BindJournal();
        }

        public void BindJournal()
        {
            DataTable dt;
            string condition = "";
            
            if (cmbSelectionMethod.Text == "General Ledger")
            {
                condition = " and tbl_transaction.state = 0 and tbl_transaction.hum_id !=0 ";
            }
            else if(cmbSelectionMethod.Text == "Default")
            {
                condition = " and tbl_transaction.state = 0 ";
                //condition = " and tbl_transaction.hum_id =1 ";
            }
            else if(cmbSelectionMethod.Text == "Inventory Opening Stock")
            {
                condition = " and tbl_transaction.state = 0 and tbl_transaction.type ='Opening Balance' ";
            }
            else
            {
                condition = " and tbl_transaction.state = 0 and tbl_transaction.type ='" + cmbSelectionMethod.Text.ToString() + "'";
            }
            List<MySqlParameter> parameters = new List<MySqlParameter>
            {
                DBClass.CreateParameter("id", transactionId)
            };
            if (!chkDate.Checked)
            {
                condition += " and tbl_transaction.date >= @dateFrom and tbl_transaction.date <=@dateTo";
                parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            }
            if (cmbSelectionMethod.Text == "Sales Invoice")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date AS `Date`, tbl_transaction.transaction_id,
                                tbl_transaction.type `Type`,
                                tbl_coa_level_4.code AS 'A/C Code',
                                tbl_coa_level_4.name AS `A/C NAME`, 
                                tbl_transaction.description AS `Description`,
                                tbl_transaction.debit AS `DEBIT`, 
                                tbl_transaction.credit AS `CREDIT`
                            FROM tbl_transaction 
                            INNER JOIN tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id where (tbl_transaction.type = 'Sales Invoice Cash' or tbl_transaction.type = 'Sales Invoice') and tbl_transaction.state = 0",
                            parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["A/C name"].Width = 230;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else
            if (cmbSelectionMethod.Text == "Purchase Invoice")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date AS `Date`, tbl_transaction.transaction_id,
                                tbl_transaction.type `Type`,
                                tbl_coa_level_4.code AS 'A/C Code',
                                tbl_coa_level_4.name AS `A/C NAME`, 
                                tbl_transaction.description AS `Description`,
                                tbl_transaction.debit AS `DEBIT`, 
                                tbl_transaction.credit AS `CREDIT`
                            FROM tbl_transaction 
                            INNER JOIN tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id where (tbl_transaction.type = 'Purchase Invoice Cash' or tbl_transaction.type = 'Purchase Invoice') and tbl_transaction.state = 0",
                            parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["A/C name"].Width = 230;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else
            if (type.ToLower() == "sales")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date, tbl_transaction.transaction_id,tbl_transaction.type `Type`,  tbl_coa_level_4.code AS 'A/C CODE', 
                                tbl_coa_level_4.name AS 'A/C NAME', tbl_transaction.description AS Description, 
                                tbl_transaction.debit AS DEBIT, tbl_transaction.credit AS CREDIT, tbl_customer.name AS Partner
                                FROM tbl_transaction  INNER JOIN tbl_coa_level_4 ON tbl_transaction.account_id = tbl_coa_level_4.id 
                                INNER JOIN tbl_sales ON tbl_transaction.transaction_id = tbl_sales.id INNER JOIN 
                                tbl_customer ON tbl_sales.customer_id = tbl_customer.id where transaction_id=@id and tbl_transaction.t_type = 'SALES'",
                                parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if (type.ToLower() == "purchase")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (
                                ORDER BY tbl_transaction.date) AS `SN`,
                                 concat('000', transaction_id) as `Ref Id`,
                                 tbl_transaction.date, tbl_transaction.transaction_id,tbl_transaction.type `Type`,  tbl_coa_level_4.code AS 'A/C CODE', 
                                 tbl_coa_level_4.name AS 'A/C NAME', tbl_transaction.description AS Description, 
                                 tbl_transaction.debit AS DEBIT, tbl_transaction.credit AS CREDIT, tbl_vendor.name AS Partner
                                FROM tbl_transaction
                                INNER JOIN tbl_coa_level_4 ON tbl_transaction.account_id = tbl_coa_level_4.id
                                INNER JOIN tbl_purchase ON tbl_transaction.transaction_id = tbl_purchase.id
                                INNER JOIN 
                                 tbl_vendor ON tbl_purchase.vendor_id = tbl_vendor.id
                                WHERE transaction_id= @id AND t_type = 'PURCHASE'",
                                parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if (type.ToLower() == "purchase return")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (
                                ORDER BY tbl_transaction.date) AS `SN`,
                                 concat('000', transaction_id) as `Ref Id`,
                                 tbl_transaction.date, tbl_transaction.transaction_id,tbl_transaction.type `Type`,  tbl_coa_level_4.code AS 'A/C CODE', 
                                 tbl_coa_level_4.name AS 'A/C NAME', tbl_transaction.description AS Description, 
                                 tbl_transaction.debit AS DEBIT, tbl_transaction.credit AS CREDIT, tbl_vendor.name AS Partner
                                FROM tbl_transaction
                                INNER JOIN tbl_coa_level_4 ON tbl_transaction.account_id = tbl_coa_level_4.id
                                INNER JOIN tbl_purchase_return ON tbl_transaction.transaction_id = tbl_purchase_return.id
                                INNER JOIN 
                                 tbl_vendor ON tbl_purchase_return.vendor_id = tbl_vendor.id
                                WHERE transaction_id= @id AND t_type = 'PURCHASE RETURN'",
                                parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if (type.ToLower() == "payment")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                             ROW_NUMBER() OVER (
                            ORDER BY tbl_transaction.date) AS `SN`,
                                                            concat('000', transaction_id) as `Ref Id`,
                             tbl_transaction.date, tbl_transaction.transaction_id,tbl_transaction.type `Type`,  tbl_coa_level_4.code AS 'A/C CODE', 
                             tbl_coa_level_4.name AS 'A/C NAME', tbl_transaction.description AS Description, 
                             tbl_transaction.debit AS DEBIT, tbl_transaction.credit AS CREDIT, CASE WHEN tbl_payment_voucher.type = 'Vendor' THEN (
                            SELECT name
                            FROM tbl_vendor
                            WHERE id=(
                            SELECT hum_id
                            FROM tbl_payment_voucher_details
                            WHERE payment_id = tbl_payment_voucher.id)) WHEN tbl_payment_voucher.type = 'Employee' THEN (
                            SELECT name
                            FROM tbl_employee
                            WHERE id=(
                            SELECT hum_id
                            FROM tbl_payment_voucher_details
                            WHERE payment_id = tbl_payment_voucher.id)) WHEN tbl_payment_voucher.type = 'General' THEN (
                            SELECT name
                            FROM tbl_coa_level_4
                            WHERE id=(
                            SELECT hum_id
                            FROM tbl_payment_voucher_details
                            WHERE payment_id = tbl_payment_voucher.id)) ELSE '' END Partner
                            FROM tbl_transaction
                            INNER JOIN tbl_coa_level_4 ON tbl_transaction.account_id = tbl_coa_level_4.id
                            INNER JOIN tbl_payment_voucher ON tbl_transaction.transaction_id = tbl_payment_voucher.id
                            WHERE transaction_id= @id AND t_type = 'PAYMENT'",
                                parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if (type.ToLower() == "receipt")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (
                                ORDER BY tbl_transaction.date) AS `SN`,
                                                                concat('000', transaction_id) as `Ref Id`,
                                 tbl_transaction.date, tbl_transaction.transaction_id,tbl_transaction.type `Type`,  tbl_coa_level_4.code AS 'A/C CODE', 
                                 tbl_coa_level_4.name AS 'A/C NAME', tbl_transaction.description AS Description, 
                                 tbl_transaction.debit AS DEBIT, tbl_transaction.credit AS CREDIT, CASE WHEN tbl_receipt_voucher.type = 'Customer' THEN (
                                SELECT name
                                FROM tbl_customer
                                WHERE id=(
                                SELECT hum_id
                                FROM tbl_receipt_voucher_details
                                WHERE payment_id = tbl_receipt_voucher.id limit 1)) WHEN tbl_receipt_voucher.type = 'General' THEN (
                                SELECT name
                                FROM tbl_coa_level_4
                                WHERE id=(
                                SELECT hum_id
                                FROM tbl_receipt_voucher_details
                                WHERE payment_id = tbl_receipt_voucher.id)) ELSE '' END Partner
                                FROM tbl_transaction
                                INNER JOIN tbl_coa_level_4 ON tbl_transaction.account_id = tbl_coa_level_4.id
                                INNER JOIN tbl_receipt_voucher ON tbl_transaction.transaction_id = tbl_receipt_voucher.id
                                WHERE transaction_id= @id AND t_type = 'RECEIPT'",
                                parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if (type.ToLower() == "journal")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (
                                ORDER BY tbl_transaction.date) AS `SN`,
                                                                concat('000', transaction_id) as `Ref Id`,
                                 tbl_transaction.date, tbl_transaction.transaction_id,
                                 tbl_transaction.type `Type`, tbl_coa_level_4.code AS 'A/C CODE', 
                                 tbl_coa_level_4.name AS 'A/C NAME', tbl_transaction.description AS Description, 
                                 tbl_transaction.debit AS DEBIT, tbl_transaction.credit AS CREDIT, (SELECT partner FROM tbl_journal_voucher_details
                                WHERE inv_id = tbl_journal_voucher.id LIMIT 1
                                 ) as parent
                                FROM tbl_transaction
                                INNER JOIN tbl_coa_level_4 ON tbl_transaction.account_id = tbl_coa_level_4.id
                                INNER JOIN tbl_journal_voucher ON tbl_transaction.transaction_id = tbl_journal_voucher.id
                                WHERE transaction_id= @id AND t_type = 'JOURNAL'",
                                parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit, null);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if (type.ToLower() == "inventory")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date AS `Date`, tbl_transaction.transaction_id,
                                tbl_transaction.type `Type`,
                                tbl_coa_level_4.code AS 'A/C Code',
                                tbl_coa_level_4.name AS `A/C NAME`, 
                                tbl_transaction.description AS `Description`,
                                tbl_transaction.debit AS `DEBIT`, 
                                tbl_transaction.credit AS `CREDIT`
                            FROM tbl_transaction 
                            INNER JOIN tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id where description 
                                like '%- Item Code - " + transactionId + "'",
                             parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["A/C name"].Width = 230;
                dgvJournal.Columns["sn"].Width = 50;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
            }
            else if(type.ToLower() == "vat input" || type.ToLower() == "vat output")
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date AS `Date`, tbl_transaction.transaction_id,
                                tbl_transaction.type `Type`,
                                tbl_coa_level_4.code AS 'A/C Code',
                                tbl_coa_level_4.name AS `A/C NAME`, 
                                tbl_transaction.description AS `Description`,
                                tbl_transaction.debit AS `DEBIT`, 
                                tbl_transaction.credit AS `CREDIT`
                            FROM tbl_transaction 
                            INNER JOIN tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id " + condition+ @" and tbl_transaction.account_id = (SELECT account_id FROM tbl_coa_config WHERE category = '" + type+"')",
                            parameters.ToArray());
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
                try
                {
                    dgvJournal.Columns["A/C name"].Width = 230;
                    dgvJournal.Columns["sn"].Width = 50;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else if (new[] {"Sales All","Purchase All","Purchase Return All","Sales Return All","Debit Note All","Credit Note All","Petty Cash All"}.Contains(type, StringComparer.OrdinalIgnoreCase))
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date AS `Date`, tbl_transaction.transaction_id,
                                tbl_transaction.type `Type`,
                                tbl_coa_level_4.code AS 'A/C Code',
                                tbl_coa_level_4.name AS `A/C NAME`, 
                                tbl_transaction.description AS `Description`,
                                tbl_transaction.debit AS `DEBIT`, 
                                tbl_transaction.credit AS `CREDIT`
                            FROM tbl_transaction 
                            INNER JOIN tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id " + condition + @" and tbl_transaction.type Like '" + type.Replace(" All","") + "%'",
                            parameters.ToArray());


                if (dt == null)
                {
                    dgvJournal.DataSource = null;
                    return;
                }
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
                try
                {
                    dgvJournal.Columns["A/C name"].Width = 230;
                    dgvJournal.Columns["sn"].Width = 50;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
            else
            {
                dt = DBClass.ExecuteDataTable(@"SELECT 
                                 ROW_NUMBER() OVER (ORDER BY tbl_transaction.date) AS `SN`,
                                concat('000', transaction_id) as `Ref Id`,
                                tbl_transaction.date AS `Date`, tbl_transaction.transaction_id,
                                tbl_transaction.type `Type`,
                                tbl_coa_level_4.code AS 'A/C Code',
                                tbl_coa_level_4.name AS `A/C NAME`, 
                                tbl_transaction.description AS `Description`,
                                tbl_transaction.debit AS `DEBIT`, 
                                tbl_transaction.credit AS `CREDIT`
                            FROM tbl_transaction 
                            INNER JOIN tbl_coa_level_4 
                            ON tbl_transaction.account_id = tbl_coa_level_4.id " + condition,
                            parameters.ToArray());
                if (dt == null)
                {
                    dgvJournal.DataSource = null;
                    return;
                }
                decimal totalDebit = dt.AsEnumerable().Sum(row => row.Field<decimal>("debit"));
                decimal totalCredit = dt.AsEnumerable().Sum(row => row.Field<decimal>("credit"));

                dt.Rows.Add(null, null, null, null, null, null, null, "TOTAL", totalDebit, totalCredit);
                dgvJournal.DataSource = dt;
                dgvJournal.Columns["A/C NAME"].AutoSizeMode = dgvJournal.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvJournal.Columns["transaction_id"].Visible = dgvJournal.Columns["Description"].Visible = false;
                try
                {
                    dgvJournal.Columns["A/C name"].Width = 230;
                    dgvJournal.Columns["sn"].Width = 50;
                }catch(Exception ex)
                {
                    ex.ToString();
                }
            }

            dgvJournal.CellPainting += new DataGridViewCellPaintingEventHandler(dgvJournal_CellPainting);

            LocalizationManager.LocalizeDataGridViewHeaders(dgvJournal);
        }

        private void dgvJournal_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }
        private void dgvJournal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvJournal.Rows.Count == 0)
                return;

            frmLogin.frmMain.openChildForm(new frmTransactionJournal(int.Parse(dgvJournal.SelectedRows[0].Cells["transaction_id"].Value.ToString()), dgvJournal.SelectedRows[0].Cells["type"].Value.ToString(), dgvJournal.SelectedRows[0].Cells["Ref Id"].Value.ToString(), dgvJournal.SelectedRows[0].Cells["Date"].Value.ToString()));
        }

        private void cmbSelectionMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindJournal();
        }

       
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dgvJournal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvJournal_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            BindJournal();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindJournal();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindJournal();
        }
    }
}
