using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterRecycle : Form
    {
        string form = "";
        public event EventHandler DataRestored;

        public MasterRecycle(string _form)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.form = _form;
            headerUC1.FormText = this.Text + form;
        }
     
        private void MasterRecycle_Load(object sender, EventArgs e)
        {
            BindData();
        }
        public void BindData()
        {
            if (form == "Item")
            {
                DataTable dt;
                string query = @"select id, code as 'Item Code',name as 'Item Name',barcode as Barcode,cost_price AS 'Cost Price',
               sales_price as 'Sales Price',on_hand as 'On Hand' from tbl_items where state = -1";
                dt = DBClass.ExecuteDataTable(query);
                dgvData.DataSource = dt;
                dgvData.Columns["Item code"].Width = 110;
                dgvData.Columns["Item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "Sales")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select s.id,s.date,
                    CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', s.total from tbl_sales s
                    INNER JOIN tbl_customer ON s.customer_id = tbl_customer.id where s.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "Purchase")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select p.id,p.date,
                    CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name',p.total from tbl_purchase p
                    INNER JOIN tbl_vendor ON p.vendor_id = tbl_vendor.id WHERE p.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Vendor Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "PurchaseOrder")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select p.id,p.date,
                    CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name',p.total from tbl_purchase_order p
                    INNER JOIN tbl_vendor ON p.vendor_id = tbl_vendor.id WHERE p.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Vendor Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "PurchaseReturn")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select p.id,p.date,
                    CONCAT(tbl_vendor.code,' - ', tbl_vendor.name) AS 'Vendor Name',p.total from tbl_purchase_return p
                    INNER JOIN tbl_vendor ON p.vendor_id = tbl_vendor.id WHERE p.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Vendor Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if(form== "SalesOrder")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select s.id,s.date,
                    CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', s.total from tbl_sales_order s
                    INNER JOIN tbl_customer ON s.customer_id = tbl_customer.id where s.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "SalesQutation")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select s.id,s.date,
                    CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', s.total from tbl_sales_quotation s
                    INNER JOIN tbl_customer ON s.customer_id = tbl_customer.id where s.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if(form== "SalesReturn")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select s.id,s.date,
                    CONCAT(tbl_customer.code, ' - ', tbl_customer.name) AS 'Customer Name', s.total from tbl_sales_return s
                    INNER JOIN tbl_customer ON s.customer_id = tbl_customer.id where s.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Customer Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if(form== "Damage")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select d.id,d.date,
                    CONCAT(tbl_employee.code,' - ', tbl_employee.name) AS 'Employee Name', d.total from tbl_damage d
                    INNER JOIN tbl_employee ON d.reported_by = tbl_employee.id where d.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form== "Receipt")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select r.id,r.date,
                    concat(tc.code,' - ',tc.name) AS 'Account Name', r.total from tbl_receipt_voucher r
                    INNER JOIN tbl_coa_level_4 tc ON r.debit_account_id = tc.id where r.state = -1");
                dgvData.DataSource = dt;
                if (dt ==null || dt.Rows.Count < 0)
                    return;
                dgvData.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "Payment")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select p.id,p.date,
                    concat(tc.code,' - ',tc.name) AS 'Account Name', p.total from tbl_receipt_voucher p
                    INNER JOIN tbl_coa_level_4 tc ON p.debit_account_id = tc.id where p.state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
            else if (form == "Tax Form")
            {
                DataTable dt = DBClass.ExecuteDataTable(@"select id, Name, value, Description from tbl_tax where state = -1");
                dgvData.DataSource = dt;
                dgvData.Columns["Name"].AutoSizeMode = dgvData.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvData.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            }
        }
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAll.Checked)
                cmbCategory.Enabled = false;
            else
                cmbCategory.Enabled = true;
            BindData();
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
                return;

            if (form == "Item")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_items SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND type = 'Opening Balance';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Item", "Item", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Item: " + dgvData.SelectedRows[0].Cells["Item Name"].Value);
            }
            else if (form == "Sales")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_sales SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'SALES';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Sales", "Sales", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Sales: " + dgvData.SelectedRows[0].Cells["Customer Name"].Value);
            }
            else if (form == "Purchase")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_purchase SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'PURCHASE';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Purchase", "Purchase", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Purchase: " + dgvData.SelectedRows[0].Cells["Vendor Name"].Value);
            }
            else if (form == "PurchaseOrder")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_purchase_order SET state = 0 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Purchase Order", "Purchase Order", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Purchase Order: " + dgvData.SelectedRows[0].Cells["Vendor Name"].Value);
            }
            else if (form == "SalesOrder")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_sales_order SET state = 0 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Sales Order", "Sales Order", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Sales Order: " + dgvData.SelectedRows[0].Cells["Customer Name"].Value);
            }
            else if (form == "SalesQutation")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_sales_quotation SET state = 0 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Sales Quotation", "Sales Quotation", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Sales Quotation: " + dgvData.SelectedRows[0].Cells["Customer Name"].Value);
            }
            else if (form == "SalesReturn")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_sales_return SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'SALES RETURN';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Sales Return", "Sales Return", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Sales Return: " + dgvData.SelectedRows[0].Cells["Customer Name"].Value);
            }
            else if (form == "Damage")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_damage SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'DAMAGE';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Damage", "Damage", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Damage: " + dgvData.SelectedRows[0].Cells["Employee Name"].Value);
            }
            else if (form == "PurchaseReturn")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_purchase_return SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'PURCHASE RETURN';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Purchase Return", "Purchase Return", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Purchase Return: " + dgvData.SelectedRows[0].Cells["Vendor Name"].Value);
            }
            else if (form == "Receipt")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_receipt_voucher SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'RECEIPT';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Receipt", "Receipt", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Receipt: " + dgvData.SelectedRows[0].Cells["Account Name"].Value);
            }
            else if (form == "Payment")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_payment_voucher SET state = 0 WHERE id = @id  ; UPDATE tbl_transaction SET state= 0 WHERE transaction_id=@id AND t_type = 'PAYMENT';",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Payment", "Payment", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Payment: " + dgvData.SelectedRows[0].Cells["Account Name"].Value);
            }
            else if (form == "Tax Form")
            {
                DBClass.ExecuteNonQuery("UPDATE tbl_tax SET state = 0 WHERE id = @id  ;",
                                          DBClass.CreateParameter("id", dgvData.SelectedRows[0].Cells["id"].Value.ToString()));
                Utilities.LogAudit(frmLogin.userId, "Restore Tax Form", "Tax Form", Convert.ToInt32(dgvData.SelectedRows[0].Cells["id"].Value), "Restored Tax Form: " + dgvData.SelectedRows[0].Cells["Name"].Value);
            }
            BindData();
            OnDataRestored(EventArgs.Empty);
            this.Close();
        }
        protected virtual void OnDataRestored(EventArgs e)
        {
            DataRestored?.Invoke(this, e);
        }
    }
}
