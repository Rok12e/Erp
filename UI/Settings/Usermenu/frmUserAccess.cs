using MySql.Data.MySqlClient;
using Novacode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class frmUserAccess : Form
    {
        private DataView _dataView;
        private EventHandler UserUpdatedHandler;
        private EventHandler InvoiceUpdatedHandler;
        private EventHandler PaymentVoucherHandler;
        int userId = 0;
        DataTable table;

        public frmUserAccess()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            UserUpdatedHandler = PaymentVoucherHandler = InvoiceUpdatedHandler = (sender, args) => BindUser();
            EventHub.User += UserUpdatedHandler;
            EventHub.PurchaseInv += InvoiceUpdatedHandler;
            EventHub.PaymentVoucher += PaymentVoucherHandler;
            headerUC1.FormText = "Access User Control";
            table = DBClass.ExecuteDataTable("SELECT a.id AS main_id,a.name AS main_name,b.id AS sub_id,b.name AS sub_name from tbl_main_menus a,tbl_sub_menus b WHERE a.id=b.m_id;");
        }
        private void frmUserAccess_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.PurchaseInv -= InvoiceUpdatedHandler;
            EventHub.User -= UserUpdatedHandler;
            EventHub.PaymentVoucher -= PaymentVoucherHandler;

        }
        private void frmUserAccess_Load(object sender, EventArgs e)
        {
            pnlFileAccess.Visible = false;
            pnlsettingn.Visible = false;
            pnlReport.Visible = false;
            pnlbank.Visible=false;
            pnlHR.Visible = false;
            pnlVendor.Visible = false;
            pnlCustomer.Visible = false;
            pnlaccounting.Visible = false;
            pnlInventory.Visible = false;
            BindUser();
        }
        
        public void BindUser()
        {
            string query = @"SELECT  id,CONCAT(first_name , last_name) as NAME FROM tbl_sec_users c where id >0 ";
            if (cmbState.Text == "Active User")
                query += " AND c.active = 0";
            else if (cmbState.Text == "Inactive User")
                query += " AND c.active != 0";

            //query += " GROUP BY c.id, c.name, c.work_phone, c.main_phone, tc.name, c.region, c.email, c.trn;";
            DataTable dt = DBClass.ExecuteDataTable(query);
            _dataView = dt.DefaultView;
            dgvCustomer.DataSource = _dataView;
            dgvCustomer.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }   

     
        private void createInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.LoadFormIntoPanel(new frmPurchase(0,"",int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void customerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        private void exportTransactionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
        private void customerListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }
        private void transactionListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }
        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUser();
        }
        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewUser(0).ShowDialog();
        }
        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewUser(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            userId = int.Parse(dgvCustomer.SelectedRows[0].Cells[0].Value.ToString());
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_sec_users where id=@id",DBClass.CreateParameter("id",userId)))
            {
                if (reader.Read())
                {
                    lblCode.Text = userId.ToString();
                    lblName.Text = reader["user_name"].ToString();
                    lblTRN.Text = reader["first_name"].ToString() + " " + reader["last_name"].ToString();
                    lblEmail.Text = reader["active"].ToString() == "1" ? "Active" : "Un Active";
                    loadUserPermission();
                }
            }
        }
        private void loadUserPermission()
        {
            object netResult = DBClass.ExecuteScalar("SELECT count(*) FROM tbl_user_permissions where user_id = @user_id", DBClass.CreateParameter("user_id", userId));
            int count = netResult != DBNull.Value ? Convert.ToInt32(netResult) : 0;
            if (count>0)
            {
                DataTable userTable = DBClass.ExecuteDataTable("SELECT p.id,p.user_id,s.id AS sub_menu_id,s.name AS sub_menu_name,m.id AS main_menu_id,m.name AS main_menu_name,p.can_view,p.can_edit,p.can_delete from tbl_user_permissions p,tbl_main_menus m,tbl_sub_menus s WHERE m.id= s.m_id AND p.sub_menu_id=s.id and p.user_id = @user_id", DBClass.CreateParameter("user_id", userId));
                setUserOption(table, userTable);
            }
            else
            {
                setOption(table);
            }
        }

        private void setUserOption(DataTable table, DataTable userTable)
        {
            foreach (DataRow row in userTable.Rows)
            {
                string menu = row["main_menu_name"].ToString().Trim();
                string sub_menu = row["sub_menu_name"].ToString().Trim();

                if (sub_menu.Equals("Chart Of Account", StringComparison.OrdinalIgnoreCase))
                {
                    CBChartofaccount.Checked = Convert.ToBoolean(row["can_view"]);
                    CBChartofaccountEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBChartofaccountDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBChartofaccountEdit.Enabled = CBChartofaccount.Checked;
                    CBChartofaccountDelete.Enabled = CBChartofaccount.Checked;
                }
                else if (sub_menu.Equals("Cost Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBCostCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBCostCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBCostCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBCostCenterEdit.Enabled = CBCostCenter.Checked;
                    CBCostCenterDelete.Enabled = CBCostCenter.Checked;
                }
                else if (sub_menu.Equals("Transactions Journal", StringComparison.OrdinalIgnoreCase))
                {
                    CBTransactionsJ.Checked = Convert.ToBoolean(row["can_view"]);
                    CBTransactionsJEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBTransactionsJDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBTransactionsJEdit.Enabled = CBTransactionsJ.Checked;
                    CBTransactionsJDelete.Enabled = CBTransactionsJ.Checked;
                }
                else if (sub_menu.Equals("Fixed Assets", StringComparison.OrdinalIgnoreCase))
                {
                    CBFixedAssets.Checked = Convert.ToBoolean(row["can_view"]);
                    CBFixedAssetsEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBFixedAssetsDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBFixedAssetsEdit.Enabled = CBFixedAssets.Checked;
                    CBFixedAssetsDelete.Enabled = CBFixedAssets.Checked;
                }
                else if (sub_menu.Equals("Vouchers", StringComparison.OrdinalIgnoreCase))
                {
                    CBVouchers.Checked = Convert.ToBoolean(row["can_view"]);
                    CBVouchersEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBVouchersDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBVouchersEdit.Enabled = CBVouchers.Checked;
                    CBVouchersDelete.Enabled = CBVouchers.Checked;
                }
                else if (sub_menu.Equals("Prepaid Expense", StringComparison.OrdinalIgnoreCase))
                {
                    CBPrepaid.Checked = Convert.ToBoolean(row["can_view"]);
                    CBPrepaidEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBPrepaidDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBPrepaidEdit.Enabled = CBPrepaid.Checked;
                    CBPrepaidDelete.Enabled = CBPrepaid.Checked;
                }
                else if (sub_menu.Equals("Petty Cash", StringComparison.OrdinalIgnoreCase))
                {
                    CBPetty.Checked = Convert.ToBoolean(row["can_view"]);
                    CBPettyEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBPettyDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBPettyEdit.Enabled = CBPetty.Checked;
                    CBPettyDelete.Enabled = CBPetty.Checked;
                }
                else if (sub_menu.Equals("Inventory Items", StringComparison.OrdinalIgnoreCase))
                {
                    CBInventory.Checked = Convert.ToBoolean(row["can_view"]);
                    CBInventoryCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBInventoryCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBInventoryCenterEdit.Enabled = CBInventory.Checked;
                    CBInventoryCenterDelete.Enabled = CBInventory.Checked;
                }
                else if (sub_menu.Equals("Stock Management", StringComparison.OrdinalIgnoreCase))
                {
                    CBSTOCK.Checked = Convert.ToBoolean(row["can_view"]);
                    CBSTOCKEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBSTOCKDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBSTOCKEdit.Enabled = CBSTOCK.Checked;
                    CBSTOCKDelete.Enabled = CBSTOCK.Checked;
                }
                else if (sub_menu.Equals("Inventory Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBInventoryCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBInventoryCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBInventoryCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBInventoryCenterEdit.Enabled = CBInventoryCenter.Checked;
                    CBInventoryCenterDelete.Enabled = CBInventoryCenter.Checked;
                }
                else if (sub_menu.Equals("Warehouse Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBWarehouseCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBWarehouseCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBWarehouseCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBWarehouseCenterEdit.Enabled = CBWarehouseCenter.Checked;
                    CBWarehouseCenterDelete.Enabled = CBWarehouseCenter.Checked;
                }
                else if (sub_menu.Equals("Customer Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBCustomerCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBCustomerCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBCustomerCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBCustomerCenterEdit.Enabled = CBCustomerCenter.Checked;
                    CBCustomerCenterDelete.Enabled = CBCustomerCenter.Checked;
                }
                else if (sub_menu.Equals("Sales Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBSalesCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBSalesCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBSalesCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBSalesCenterEdit.Enabled = CBSalesCenter.Checked;
                    CBSalesCenterDelete.Enabled = CBSalesCenter.Checked;
                }
                else if (sub_menu.Equals("Credit Note", StringComparison.OrdinalIgnoreCase))
                {
                    CBCreditNote.Checked = Convert.ToBoolean(row["can_view"]);
                    CBCreditNoteEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBCreditNoteDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBCreditNoteEdit.Enabled = CBCreditNote.Checked;
                    CBCreditNoteDelete.Enabled = CBCreditNote.Checked;
                }
                else if (sub_menu.Equals("Quotation", StringComparison.OrdinalIgnoreCase))
                {
                    CBQuotation.Checked = Convert.ToBoolean(row["can_view"]);
                    CBQuotationEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBQuotationDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBQuotationEdit.Enabled = CBQuotation.Checked;
                    CBQuotationDelete.Enabled = CBQuotation.Checked;
                }
                else if (sub_menu.Equals("Sales Order", StringComparison.OrdinalIgnoreCase))
                {
                    CBSalesOrder.Checked = Convert.ToBoolean(row["can_view"]);
                    CBSalesOrderEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBSalesOrderDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBSalesOrderEdit.Enabled = CBSalesOrder.Checked;
                    CBSalesOrderDelete.Enabled = CBSalesOrder.Checked;
                }
                else if (sub_menu.Equals("Sales Return", StringComparison.OrdinalIgnoreCase))
                {
                    CBSalesReturn.Checked = Convert.ToBoolean(row["can_view"]);
                    CBSalesReturnEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBSalesReturnDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBSalesReturnEdit.Enabled = CBSalesReturn.Checked;
                    CBSalesReturnDelete.Enabled = CBSalesReturn.Checked;
                }
                else if (sub_menu.Equals("Sales Proforma", StringComparison.OrdinalIgnoreCase))
                {
                    CBSalesProforma.Checked = Convert.ToBoolean(row["can_view"]);
                    CBSalesProformaEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBSalesProformaDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBSalesProformaEdit.Enabled = CBSalesProforma.Checked;
                    CBSalesProformaDelete.Enabled = CBSalesProforma.Checked;
                }
                else if (sub_menu.Equals("Receipt Voucher", StringComparison.OrdinalIgnoreCase))
                {
                    CBReceiveVoucher.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Create Invoice", StringComparison.OrdinalIgnoreCase))
                {
                    CBCreateInvoice.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Create Purchases", StringComparison.OrdinalIgnoreCase))
                {
                    CBCreatePurchase.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Vendor Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBVendorCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBVendorCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBVendorCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBVendorCenterEdit.Enabled = CBVendorCenter.Checked;
                    CBVendorCenterDelete.Enabled = CBVendorCenter.Checked;
                }
                else if (sub_menu.Equals("Purchases Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBPurchasesCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBPurchasesCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBPurchasesCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBPurchasesCenterEdit.Enabled = CBPurchasesCenter.Checked;
                    CBPurchasesCenterDelete.Enabled = CBPurchasesCenter.Checked;
                }
                else if (sub_menu.Equals("Payment Voucher", StringComparison.OrdinalIgnoreCase))
                {
                    if (menu.Equals("Vendor", StringComparison.OrdinalIgnoreCase))
                    {
                        CBPaymentVoucher.Checked = Convert.ToBoolean(row["can_view"]);
                    }
                    else if (menu.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    {
                        CBPaymentVoucher2.Checked = Convert.ToBoolean(row["can_view"]);
                    }
                }
                else if (sub_menu.Equals("Debit Note", StringComparison.OrdinalIgnoreCase))
                {
                    CBDebitNote.Checked = Convert.ToBoolean(row["can_view"]);
                    CBDebitNoteEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBDebitNoteDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBDebitNoteEdit.Enabled = CBDebitNote.Checked;
                    CBDebitNoteDelete.Enabled = CBDebitNote.Checked;
                }
                else if (sub_menu.Equals("Purchase Order", StringComparison.OrdinalIgnoreCase))
                {
                    CBPurchaseOrder.Checked = Convert.ToBoolean(row["can_view"]);
                    CBPurchaseOrderEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBPurchaseOrderDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBPurchaseOrderEdit.Enabled = CBPurchaseOrder.Checked;
                    CBPurchaseOrderDelete.Enabled = CBPurchaseOrder.Checked;
                }
                else if (sub_menu.Equals("Purchase Return", StringComparison.OrdinalIgnoreCase))
                {
                    CBPurchaseReturn.Checked = Convert.ToBoolean(row["can_view"]);
                    CBPurchaseReturnEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBPurchaseReturnDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBPurchaseReturnEdit.Enabled = CBPurchaseReturn.Checked;
                    CBPurchaseReturnDelete.Enabled = CBPurchaseReturn.Checked;
                }
                else if (sub_menu.Equals("Human Resource Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBHumanResourceCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBHumanResourceCenterEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBHumanResourceCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBHumanResourceCenterEdit.Enabled = CBHumanResourceCenter.Checked;
                    CBHumanResourceCenterDelete.Enabled = CBHumanResourceCenter.Checked;
                }
                else if (sub_menu.Equals("Loans", StringComparison.OrdinalIgnoreCase))
                {
                    CBLoans.Checked = Convert.ToBoolean(row["can_view"]);
                    CBLoansEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBLoansDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBLoansEdit.Enabled = CBLoans.Checked;
                    CBLoansDelete.Enabled = CBLoans.Checked;
                }
                else if (sub_menu.Equals("Attendance Sheet", StringComparison.OrdinalIgnoreCase))
                {
                    CBAttendanceSheet.Checked = Convert.ToBoolean(row["can_view"]);
                    CBAttendanceSheetEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBAttendanceSheetDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBAttendanceSheetEdit.Enabled = CBAttendanceSheet.Checked;
                    CBAttendanceSheetDelete.Enabled = CBAttendanceSheet.Checked;
                }
                else if (sub_menu.Equals("Salary Sheet", StringComparison.OrdinalIgnoreCase))
                {
                    CBSalarySheet.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Leave Salary", StringComparison.OrdinalIgnoreCase))
                {
                    CBLeaveSalary.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("End Of Services", StringComparison.OrdinalIgnoreCase))
                {
                    CBEndOfServices.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Final Settlement", StringComparison.OrdinalIgnoreCase))
                {
                    CBFinalSettlement.Checked = Convert.ToBoolean(row["can_view"]);
                    CBFinalSettlementEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBFinalSettlementDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBFinalSettlementEdit.Enabled = CBFinalSettlement.Checked;
                    CBFinalSettlementDelete.Enabled = CBFinalSettlement.Checked;
                }
                else if (sub_menu.Equals("Bank Center", StringComparison.OrdinalIgnoreCase))
                {
                    CBBankCenter.Checked = Convert.ToBoolean(row["can_view"]);
                    CBBankCenterDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBBankCenterDelete.Enabled = CBBankCenter.Checked;
                }
                else if (sub_menu.Equals("Open Bank Card", StringComparison.OrdinalIgnoreCase))
                {
                    CBOpenBankCard.Checked = Convert.ToBoolean(row["can_view"]);
                    CBOpenBankCardEdit.Checked = Convert.ToBoolean(row["can_edit"]);
                    CBOpenBankCardDelete.Checked = Convert.ToBoolean(row["can_delete"]);
                    CBOpenBankCardEdit.Enabled = CBOpenBankCard.Checked;
                    CBOpenBankCardDelete.Enabled = CBOpenBankCard.Checked;
                }
                else if (sub_menu.Equals("Cheques", StringComparison.OrdinalIgnoreCase))
                {
                    CBCheques.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("PDC", StringComparison.OrdinalIgnoreCase))
                {
                    CBPDC.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Company", StringComparison.OrdinalIgnoreCase))
                {
                    CBCompany.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Customer & Receivable", StringComparison.OrdinalIgnoreCase))
                {
                    CBCustomerReceivable.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Sales", StringComparison.OrdinalIgnoreCase))
                {
                    CBSales.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Vendor & Payable", StringComparison.OrdinalIgnoreCase))
                {
                    CBVendorPayable.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Purchases", StringComparison.OrdinalIgnoreCase))
                {
                    CBPurchases.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Employees", StringComparison.OrdinalIgnoreCase))
                {
                    CBEmployees.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Accountant", StringComparison.OrdinalIgnoreCase))
                {
                    CBAccountant.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Inventory", StringComparison.OrdinalIgnoreCase))
                {
                    CBInventory2.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("List", StringComparison.OrdinalIgnoreCase))
                {
                    CBList.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Setting", StringComparison.OrdinalIgnoreCase))
                {
                    cbSettings.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Change Current Password", StringComparison.OrdinalIgnoreCase))
                {
                    CBChangeCurrentPassword.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Clear Data", StringComparison.OrdinalIgnoreCase))
                {
                    CBClearData.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Users", StringComparison.OrdinalIgnoreCase))
                {
                    CBUsers.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Company List", StringComparison.OrdinalIgnoreCase))
                {
                    CBCompanyList.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Back Up Company", StringComparison.OrdinalIgnoreCase))
                {
                    CBBackUpCompany.Checked = Convert.ToBoolean(row["can_view"]);
                }
                else if (sub_menu.Equals("Restore Company", StringComparison.OrdinalIgnoreCase))
                {
                    CBRestoreCompany.Checked = Convert.ToBoolean(row["can_view"]);
                }

            }

            if (CBChartofaccount.Checked && CBCostCenter.Checked && CBTransactionsJ.Checked && CBFixedAssets.Checked && CBVouchers.Checked && CBPrepaid.Checked && CBPetty.Checked)
                CBSELECTALLACCOUNT.Checked = true;

            if (CBInventory.Checked && CBSTOCK.Checked && CBInventoryCenter.Checked && CBWarehouseCenter.Checked)
                CBSELECTALLInventory.Checked = true;

            if (CBCustomerCenter.Checked && CBSalesCenter.Checked && CBCreateInvoice.Checked && CBReceiveVoucher.Checked && CBCreditNote.Checked && CBQuotation.Checked && CBSalesOrder.Checked && CBSalesReturn.Checked && CBSalesProforma.Checked)
                CBAllCustomer.Checked = true;

            if (CBVendorCenter.Checked && CBPurchasesCenter.Checked && CBCreatePurchase.Checked && CBPaymentVoucher.Checked && CBDebitNote.Checked && CBPurchaseOrder.Checked && CBPurchaseReturn.Checked)
                CBALLSELECTVENDOR.Checked = true;

            if (CBHumanResourceCenter.Checked && CBAttendanceSheet.Checked && CBSalarySheet.Checked && CBLeaveSalary.Checked && CBEndOfServices.Checked && CBLoans.Checked && CBFinalSettlement.Checked && CBPaymentVoucher.Checked)
                CBSelectAllHr.Checked = true;

            if (CBBankCenter.Checked && CBOpenBankCard.Checked && CBCheques.Checked && CBPDC.Checked)
                CBSelectAllBank.Checked = true;

            if (CBCompany.Checked && CBCustomerReceivable.Checked && CBSales.Checked && CBVendorPayable.Checked && CBPurchases.Checked && CBEmployees.Checked && CBAccountant.Checked && CBInventory.Checked && CBList.Checked)
                CBSelectAllReport.Checked = true;

            if (cbSettings.Checked && CBChangeCurrentPassword.Checked && CBClearData.Checked && CBUsers.Checked)
                CBSelectAllSetting.Checked = true;

            if (CBCompanyList.Checked && CBBackUpCompany.Checked && CBRestoreCompany.Checked)
                CBSelectAllFile.Checked = true;

            if (CBProjectDashBoard.Checked && CBProjectTender.Checked && CBProjectEstimate.Checked && CBProjectPlanning.Checked)
                CBSelectAllConstruction.Checked = true;


        }

        private void setOption(DataTable table)
        {
            //foreach (DataRow row in table.Select("main_name = 'Accountant'"))
            //{
            //    string menu = row["main_name"].ToString();
            //    string sub_menu = row["sub_name"].ToString();
            //    if (sub_menu.Equals("Chart Of Account"))
            //    {
            //        bool canOption = Convert.ToBoolean(row["can_view"]);
            //        CBChartofaccount.Checked = canOption;
            //        bool canView = Convert.ToBoolean(row["can_view"]);
            //        bool canEdit = Convert.ToBoolean(row["can_edit"]);
            //        bool canDelete = Convert.ToBoolean(row["can_delete"]);
            //    }
            //    var ooo = "";
            //}
        }
        //private void setUserOption(DataTable table, DataTable userTable)
        //{
        //    foreach (DataRow row in userTable.Select("main_menu_name = 'Accountant'"))
        //    {
        //        string menu = row["main_menu_name"].ToString();
        //        string sub_menu = row["sub_menu_name"].ToString();
        //        if (sub_menu.Equals("Chart Of Account", StringComparison.OrdinalIgnoreCase))
        //        {
        //            CBChartofaccount.Checked = Convert.ToBoolean(row["can_view"]);
        //            CBChartofaccountEdit.Checked = Convert.ToBoolean(row["can_edit"]);
        //            CBChartofaccountDelete.Checked = Convert.ToBoolean(row["can_delete"]);
        //            CBChartofaccountEdit.Enabled = CBChartofaccount.Checked;
        //            CBChartofaccountDelete.Enabled = CBChartofaccount.Checked;
        //        }
        //    }
        //}

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _dataView.RowFilter = "name like '%" + txtSearch.Text + "%'";
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewUser(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }

        
        private void deleteUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (dgvCustomer.Rows.Count == 0)
            //    return;
            //int _id = (int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));

            //if (dgvTransactions.Rows.Count <= 1 && dgvInvoiceCash.Rows.Count <= 1)
            //{
            //    DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (result == DialogResult.Yes)
            //    {
            //        DeleteData(_id);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("User has transactions. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }
        private void DeleteData(int _id)
        {
            DBClass.ExecuteNonQuery(@"Delete from tbl_user where id = @id",
                DBClass.CreateParameter("id", _id));
            BindUser();
            MessageBox.Show("User deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmOpeningBalance("User"));
        }

        private void toolStripMenuItemword_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document (*.docx)|*.docx";
            saveDialog.Title = "Save Cash Invoices";

            //if (saveDialog.ShowDialog() == DialogResult.OK)
            //{
            //    ExportCashToWord(dgvInvoiceCash, customerName, saveDialog.FileName);
            //    MessageBox.Show("Cash Invoice Exported Successfully!");
            //}
        }

        private void toolStripMenuItemCashWord_Click(object sender, EventArgs e)
        {

            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document (*.docx)|*.docx";
            saveDialog.Title = "Save Cash Invoices";

            //if (saveDialog.ShowDialog() == DialogResult.OK)
            //{
            //    ExportCashToWord(dgvInvoiceCash, customerName, saveDialog.FileName);
            //    MessageBox.Show("Cash Invoice Exported Successfully!");
            //}
        }
        private void ExportCashToWord(DataGridView dgv, string customerName, string filePath)
        {
            var doc = DocX.Create(filePath);

            // Header
            var title = doc.InsertParagraph("Customer Name: " + customerName)
                           .FontSize(14)
                           .Bold()
                           .SpacingAfter(20);

            // Determine columns to export (skip "id", "invoice id")
            List<int> includedCols = new List<int>();
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                string header = dgv.Columns[c].HeaderText.ToLower();
                if (header != "id" && header != "invoice id")
                {
                    includedCols.Add(c);
                }
            }

            int rows = dgv.Rows.Count + 1;
            int cols = includedCols.Count;
            Table table = doc.AddTable(rows, cols);
            table.Design = TableDesign.TableGrid;

            // Set column width and padding
            foreach (var row in table.Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.Width = 100;
                    cell.MarginLeft = 5;
                    cell.MarginRight = 5;
                }
            }

            // Header row
            for (int i = 0; i < includedCols.Count; i++)
            {
                table.Rows[0].Cells[i].Paragraphs[0]
                    .Append(dgv.Columns[includedCols[i]].HeaderText)
                    .Bold();
            }

            // Data rows
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                for (int c = 0; c < includedCols.Count; c++)
                {
                    var val = dgv.Rows[r].Cells[includedCols[c]].Value?.ToString() ?? "";
                    table.Rows[r + 1].Cells[c].Paragraphs[0].Append(val);
                }
            }

            doc.InsertTable(table);
            doc.Save();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.LoadFormIntoPanel(new frmViewUser(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (pnlaccounting.Height >= 208)

            {
                timer1.Stop();

            }
            else
            {
                pnlaccounting.Height = pnlaccounting.Height + 25;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (pnlaccounting.Height == 0)
            {
                timer2.Stop();
            }
            else
            {
                pnlaccounting.Height = pnlaccounting.Height - 25;
            }
        }

        private void btnExpand_Click(object sender, EventArgs e)
        {
            //if (pnlaccounting.Height >= 208)
            //{
            //    timer2.Start();
            //    timer1.Stop();
            //    btnExpandAccount.Image = YamyProject.Properties.Resources.ight;  
            //    btnExpandAccount.ImageSize = new Size(15, 15);
            //}
            //else
            //{
            //    timer5.Start();
            //    timer2.Stop();
            //    timer1.Start();
            //    btnExpandAccount.Image = YamyProject.Properties.Resources.Sort_Down;
            //    btnExpandAccount.ImageSize = new Size(20, 20);
            //}
            if (pnlaccounting.Visible == false)
            {
                pnlaccounting.Visible = true;
                btnExpandAccount.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandAccount.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandAccount.Image = YamyProject.Properties.Resources.ight;
                btnExpandAccount.ImageSize = new Size(15, 15);
                pnlaccounting.Visible = false;

            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            //if (pnlInventory.Height >= 146)
            //{
            //    timer5.Start();
            //    timer3.Stop();
            //    btnExpandInventory.Image = YamyProject.Properties.Resources.ight;
            //    btnExpandInventory.ImageSize = new Size(15, 15);
            //}
            //else
            //{
            //    timer5.Stop();
            //    timer3.Start();
            //    timer2.Start();
            //    btnExpandInventory.Image = YamyProject.Properties.Resources.Sort_Down;
            //    btnExpandInventory.ImageSize = new Size(20, 20);
            //}

            if (pnlInventory.Visible == false)
            {
                pnlInventory.Visible = true;
                btnExpandInventory.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandInventory.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandInventory.Image = YamyProject.Properties.Resources.ight;
                btnExpandInventory.ImageSize = new Size(15, 15);
                pnlInventory.Visible = false;

            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (pnlInventory.Height >= 146)

            {
                timer1.Stop();

            }
            else
            {
                pnlInventory.Height = pnlInventory.Height + 25;
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            if (pnlInventory.Height == 0)
            {
                timer2.Stop();
            }
            else
            {
                pnlInventory.Height = pnlInventory.Height - 25;
            }
        }

        private void guna2Panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click_1(object sender, EventArgs e)
        {
            if (pnlCustomer.Visible == false)
            {
                pnlCustomer.Visible = true;
                btnExpandCustomer.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandCustomer.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandCustomer.Image = YamyProject.Properties.Resources.ight;
                btnExpandCustomer.ImageSize = new Size(15, 15);
                pnlCustomer.Visible = false;

            }
        }

        private void CBChartofaccount_Click(object sender, EventArgs e)
        {
            if (CBChartofaccount.Checked == true)
            {
                CBChartofaccountEdit.Enabled = true;
                CBChartofaccountDelete.Enabled = true;
                CBChartofaccountEdit.Checked = true;
                CBChartofaccountDelete.Checked = true;
            }
            else
            {
                CBChartofaccountEdit.Enabled = false;
                CBChartofaccountDelete.Enabled = false;
                CBChartofaccountEdit.Checked = false;
                CBChartofaccountDelete.Checked = false;
            }
            updateOrEditData(
                subMenuId: subMenuId("Chart Of Account"),
                canView: CBChartofaccount.Checked,
                canEdit: CBChartofaccountEdit.Checked,
                canDelete: CBChartofaccountDelete.Checked);
        }
        private int subMenuId(string subName)
        {
            try
            {
                DataRow row = table.Select("sub_name = '" + subName + "'").First();
                return int.Parse(row["sub_id"].ToString());
            } catch(Exception ex)
            {
                ex.ToString();
                return 0;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CustomCheckBox2_Click(object sender, EventArgs e)
        {
            if (CBSELECTALLACCOUNT.Checked == true)
            {
                if (pnlaccounting.Visible == false)
                {
                    pnlaccounting.Visible = true;
                }

                CBChartofaccount.Checked = true;
                CBChartofaccountEdit.Checked = true;
                CBChartofaccountDelete.Checked = true;
                CBChartofaccountEdit.Enabled = true;
                CBChartofaccountDelete.Enabled = true;
                CBCostCenterEdit.Enabled = true;
                CBCostCenterDelete.Enabled = true;
                CBCostCenterEdit.Checked = true;
                CBCostCenterDelete.Checked = true;
                CBCostCenter.Checked = true;
                CBTransactionsJEdit.Enabled = true;
                CBTransactionsJDelete.Enabled = true;
                CBTransactionsJEdit.Checked = true;
                CBTransactionsJDelete.Checked = true;
                CBTransactionsJ.Checked = true;
                CBFixedAssetsEdit.Enabled = true;
                CBFixedAssetsDelete.Enabled = true;
                CBFixedAssetsEdit.Checked = true;
                CBFixedAssetsDelete.Checked = true;
                CBFixedAssets.Checked = true;
                CBVouchersEdit.Enabled = true;
                CBVouchersDelete.Enabled = true;
                CBVouchersEdit.Checked = true;
                CBVouchersDelete.Checked = true;
                CBVouchers.Checked = true;
                CBPrepaidEdit.Enabled = true;
                CBPrepaidDelete.Enabled = true;
                CBPrepaidEdit.Checked = true;
                CBPrepaidDelete.Checked = true;
                CBPrepaid.Checked = true;
                CBPettyEdit.Enabled = true;
                CBPettyDelete.Enabled = true;
                CBPettyEdit.Checked = true;
                CBPettyDelete.Checked = true;
                CBPetty.Checked = true;
                btnExpandAccount.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandAccount.ImageSize = new Size(20, 20);
            }
            else
            {
                CBChartofaccount.Checked = false;
                CBChartofaccountEdit.Enabled = false;
                CBChartofaccountDelete.Enabled = false;
                CBChartofaccountEdit.Checked = false;
                CBChartofaccountDelete.Checked = false;
                CBCostCenterEdit.Enabled = false;
                CBCostCenterDelete.Enabled = false;
                CBCostCenterEdit.Checked = false;
                CBCostCenterDelete.Checked = false;
                CBCostCenter.Checked = false;
                CBTransactionsJEdit.Enabled = false;
                CBTransactionsJDelete.Enabled = false;
                CBTransactionsJEdit.Checked = false;
                CBTransactionsJDelete.Checked = false;
                CBTransactionsJ.Checked = false;
                CBFixedAssetsEdit.Enabled = false;
                CBFixedAssetsDelete.Enabled = false;
                CBFixedAssetsEdit.Checked = false;
                CBFixedAssetsDelete.Checked = false;
                CBFixedAssets.Checked = false;
                CBVouchers.Checked = false;
                CBVouchersEdit.Enabled = false;
                CBVouchersDelete.Enabled = false;
                CBVouchersEdit.Checked = false;
                CBVouchersDelete.Checked = false;
                CBVouchers.Checked = false;
                CBPrepaidEdit.Enabled = false;
                CBPrepaidDelete.Enabled = false;
                CBPrepaidEdit.Checked = false;
                CBPrepaidDelete.Checked = false;
                CBPetty.Checked = false;
                CBPettyEdit.Enabled = false;
                CBPettyDelete.Enabled = false;
                CBPettyEdit.Checked = false;
                CBPettyDelete.Checked = false;
                CBPrepaid.Checked = false;

            }


            updateOrEditData(
                subMenuId: subMenuId("Chart Of Account"),
                canView: CBChartofaccount.Checked,
                canEdit: CBChartofaccountEdit.Checked,
                canDelete: CBChartofaccountDelete.Checked);

            updateOrEditData(
                subMenuId: subMenuId("Cost Center"),
                canView: CBCostCenter.Checked,
                canEdit: CBCostCenterEdit.Checked,
                canDelete: CBCostCenterDelete.Checked);

            updateOrEditData(
                subMenuId: subMenuId("Transactions Journal"),
                canView: CBTransactionsJ.Checked,
                canEdit: CBTransactionsJEdit.Checked,
                canDelete: CBTransactionsJDelete.Checked);

            updateOrEditData(
                subMenuId: subMenuId("Fixed Assets"),
                canView: CBFixedAssets.Checked,
                canEdit: CBFixedAssetsEdit.Checked,
                canDelete: CBFixedAssetsDelete.Checked);

            updateOrEditData(
                subMenuId: subMenuId("Vouchers"),
                canView: CBVouchers.Checked,
                canEdit: CBVouchersEdit.Checked,
                canDelete: CBVouchersDelete.Checked);

            updateOrEditData(
                subMenuId: subMenuId("Prepaid"),
                canView: CBPrepaid.Checked,
                canEdit: CBPrepaidEdit.Checked,
                canDelete: CBPrepaidDelete.Checked);

            updateOrEditData(
                subMenuId: subMenuId("Petty Cash"),
                canView: CBPetty.Checked,
                canEdit: CBPettyEdit.Checked,
                canDelete: CBPettyDelete.Checked);

        }

        private void updateOrEditData(int subMenuId, bool canView, bool canEdit, bool canDelete)
        {
            if (userId > 0)
            {
                string query = @"INSERT INTO tbl_user_permissions (user_id, sub_menu_id, can_view, can_edit, can_delete)
                                VALUES (@userId, @subMenuId, @canView, @canEdit, @canDelete)
                                ON DUPLICATE KEY UPDATE 
                                    can_view = @canView,
                                    can_edit = @canEdit,
                                    can_delete = @canDelete;
                            ";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@userId", userId),
                    new MySqlParameter("@subMenuId", subMenuId),
                    new MySqlParameter("@canView", canView),
                    new MySqlParameter("@canEdit", canEdit),
                    new MySqlParameter("@canDelete", canDelete)
                };

                DBClass.ExecuteNonQuery(query, parameters);
            } else
            {
                MessageBox.Show("Select user first");
            }
        }

        private void CBCostCenter_Click(object sender, EventArgs e)
        {
            if (CBCostCenter.Checked == true)
            {
                CBCostCenterEdit.Enabled = true;
                CBCostCenterDelete.Enabled = true;
                CBCostCenterEdit.Checked = true;
                CBCostCenterDelete.Checked = true;
            }
            else
            {
                CBCostCenterEdit.Enabled = false;
                CBCostCenterDelete.Enabled = false;
                CBCostCenterEdit.Checked = false;
                CBCostCenterDelete.Checked = false;
            }
            updateOrEditData(
                subMenuId: subMenuId("Cost Center"),
                canView: CBCostCenter.Checked,
                canEdit: CBCostCenterEdit.Checked,
                canDelete: CBCostCenterDelete.Checked);
        }

        private void CBTransactionsJ_Click(object sender, EventArgs e)
        {
            if (CBTransactionsJ.Checked == true)
            {
                CBTransactionsJEdit.Enabled = true;
                CBTransactionsJDelete.Enabled = true;
                CBTransactionsJEdit.Checked = true;
                CBTransactionsJDelete.Checked = true;
            }
            else
            {
                CBTransactionsJEdit.Enabled = false;
                CBTransactionsJDelete.Enabled = false;
                CBTransactionsJEdit.Checked = false;
                CBTransactionsJDelete.Checked = false;
            }
            updateOrEditData(
                subMenuId: subMenuId("Transactions Journal"),
                canView: CBTransactionsJ.Checked,
                canEdit: CBTransactionsJEdit.Checked,
                canDelete: CBTransactionsJDelete.Checked);
        }

        private void CBFixedAssets_Click(object sender, EventArgs e)
        {
            if (CBFixedAssets.Checked == true)
            {
                CBFixedAssetsEdit.Enabled = true;
                CBFixedAssetsDelete.Enabled = true;
                CBFixedAssetsEdit.Checked = true;
                CBFixedAssetsDelete.Checked = true;
            }
            else
            {
                CBFixedAssetsEdit.Enabled = false;
                CBFixedAssetsDelete.Enabled = false;
                CBFixedAssetsEdit.Checked = false;
                CBFixedAssetsDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Fixed Assets"),
               canView: CBFixedAssets.Checked,
               canEdit: CBFixedAssetsEdit.Checked,
               canDelete: CBFixedAssetsDelete.Checked);
        }

        private void CBVouchers_Click(object sender, EventArgs e)
        {
            if (CBVouchers.Checked == true)
            {
                CBVouchersEdit.Enabled = true;
                CBVouchersDelete.Enabled = true;
                CBVouchersEdit.Checked = true;
                CBVouchersDelete.Checked = true;
            }
            else
            {
                CBVouchersEdit.Enabled = false;
                CBVouchersDelete.Enabled = false;
                CBVouchersEdit.Checked = false;
                CBVouchersDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Vouchers"),
               canView: CBVouchers.Checked,
               canEdit: CBVouchersEdit.Checked,
               canDelete: CBVouchersDelete.Checked);
        }

        private void CBPrepaid_Click(object sender, EventArgs e)
        {
            if (CBPrepaid.Checked == true)
            {
                CBPrepaidEdit.Enabled = true;
                CBPrepaidDelete.Enabled = true;
                CBPrepaidEdit.Checked = true;
                CBPrepaidDelete.Checked = true;
            }
            else
            {
                CBPrepaidEdit.Enabled = false;
                CBPrepaidDelete.Enabled = false;
                CBPrepaidEdit.Checked = false;
                CBPrepaidDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Prepaid Expense"),
               canView: CBPrepaid.Checked,
               canEdit: CBPrepaidEdit.Checked,
               canDelete: CBPrepaidDelete.Checked);
        }

        private void CBPetty_Click(object sender, EventArgs e)
        {
            if (CBPetty.Checked == true)
            {
                CBPettyEdit.Enabled = true;
                CBPettyDelete.Enabled = true;
                CBPettyEdit.Checked = true;
                CBPettyDelete.Checked = true;
            }
            else
            {
                CBPettyEdit.Enabled = false;
                CBPettyDelete.Enabled = false;
                CBPettyEdit.Checked = false;
                CBPettyDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Petty Cash"),
               canView: CBPetty.Checked,
               canEdit: CBPettyEdit.Checked,
               canDelete: CBPettyDelete.Checked);
        }

        private void CBSELECTALLInventory_Click(object sender, EventArgs e)
        {
            if (CBSELECTALLInventory.Checked == true)
            {
                if (pnlInventory.Visible == false)
                {
                    pnlInventory.Visible = true;
                }          
                CBInventory.Checked = true;
                CBSTOCKEdit.Enabled = true;
                CBSTOCKDelete.Enabled = true;
                CBSTOCKEdit.Checked = true;
                CBSTOCKDelete.Checked = true;
                CBSTOCK.Checked = true;
                CBInventoryCenterEdit.Enabled = true;
                CBInventoryCenterDelete.Enabled = true;
                CBInventoryCenterEdit.Checked = true;
                CBInventoryCenterDelete.Checked = true;
                CBInventoryCenter.Checked = true;
                CBWarehouseCenterEdit.Enabled = true;
                CBWarehouseCenterDelete.Enabled = true;
                CBWarehouseCenterEdit.Checked = true;
                CBWarehouseCenterDelete.Checked = true;
                CBWarehouseCenter.Checked = true;
                btnExpandInventory.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandInventory.ImageSize = new Size(20, 20);
            }
            else
            {       
                CBInventory.Checked = false;
                CBSTOCKEdit.Enabled = false;
                CBSTOCKDelete.Enabled = false;
                CBSTOCKEdit.Checked = false;
                CBSTOCKDelete.Checked = false;
                CBSTOCK.Checked = false;
                CBInventoryCenter.Checked = false;
                CBInventoryCenterEdit.Enabled = false;
                CBInventoryCenterDelete.Enabled = false;
                CBInventoryCenterEdit.Checked = false;
                CBInventoryCenterDelete.Checked = false;
                CBWarehouseCenter.Checked = false;
                CBWarehouseCenterEdit.Enabled = false;
                CBWarehouseCenterDelete.Enabled = false;
                CBWarehouseCenterEdit.Checked = false;
                CBWarehouseCenterDelete.Checked = false;
            }

            updateOrEditData(
               subMenuId: subMenuId("Inventory Items"),
               canView: CBInventory.Checked,
               canEdit: CBInventoryCenterEdit.Checked,
               canDelete: CBInventoryCenterDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Stock Management"),
               canView: CBSTOCK.Checked,
               canEdit: CBSTOCKEdit.Checked,
               canDelete: CBSTOCKDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Inventory Center"),
               canView: CBInventoryCenter.Checked,
               canEdit: CBInventoryCenterEdit.Checked,
               canDelete: CBInventoryCenterDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Warehouse Center"),
               canView: CBWarehouseCenter.Checked,
               canEdit: CBWarehouseCenterEdit.Checked,
               canDelete: CBWarehouseCenterDelete.Checked);
        }

        private void CBInventory_Click(object sender, EventArgs e)
        {
            if (CBInventory.Checked == true)
            {
               
            }
            else
            {
               
            }
            updateOrEditData(
               subMenuId: subMenuId("Inventory Items"),
               canView: CBInventory.Checked,
               canEdit: CBInventoryCenterEdit.Checked,
               canDelete: CBInventoryCenterDelete.Checked);
        }

        private void CBSTOCK_Click(object sender, EventArgs e)
        {
            if (CBSTOCK.Checked == true)
            {
                CBSTOCKEdit.Enabled = true;
                CBSTOCKDelete.Enabled = true;
                CBSTOCKEdit.Checked = true;
                CBSTOCKDelete.Checked = true;
            }
            else
            {
                CBSTOCKEdit.Enabled = false;
                CBSTOCKDelete.Enabled = false;
                CBSTOCKEdit.Checked = false;
                CBSTOCKDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Stock Management"),
               canView: CBSTOCK.Checked,
               canEdit: CBSTOCKEdit.Checked,
               canDelete: CBSTOCKDelete.Checked);
        }

        private void guna2CustomCheckBox34_Click(object sender, EventArgs e)
        {
            if (CBInventoryCenter.Checked == true)
            {
                CBInventoryCenterEdit.Enabled = true;
                CBInventoryCenterDelete.Enabled = true;
                CBInventoryCenterEdit.Checked = true;
                CBInventoryCenterDelete.Checked = true;
            }
            else
            {
                CBInventoryCenterEdit.Enabled = false;
                CBInventoryCenterDelete.Enabled = false;
                CBInventoryCenterEdit.Checked = false;
                CBInventoryCenterDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Inventory Center"),
               canView: CBInventoryCenter.Checked,
               canEdit: CBInventoryCenterEdit.Checked,
               canDelete: CBInventoryCenterDelete.Checked);
        }

        private void CBWarehouseCenter_Click(object sender, EventArgs e)
        {
            if (CBWarehouseCenter.Checked == true)
            {
                CBWarehouseCenterEdit.Enabled = true;
                CBWarehouseCenterDelete.Enabled = true;
                CBWarehouseCenterEdit.Checked = true;
                CBWarehouseCenterDelete.Checked = true;
            }
            else
            {
                CBWarehouseCenterEdit.Enabled = false;
                CBWarehouseCenterDelete.Enabled = false;
                CBWarehouseCenterEdit.Checked = false;
                CBWarehouseCenterDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Warehouse Center"),
               canView: CBWarehouseCenter.Checked,
               canEdit: CBWarehouseCenterEdit.Checked,
               canDelete: CBWarehouseCenterDelete.Checked);
        }

        private void CBCustomer_Click(object sender, EventArgs e)
        {
            if (CBAllCustomer.Checked == true)
            {
                if (pnlCustomer.Visible == false)
                {
                    pnlCustomer.Visible = true;
                }

                CBCustomerCenterEdit.Enabled = true;
                CBCustomerCenterDelete.Enabled = true;
                CBCustomerCenterEdit.Checked = true;
                CBCustomerCenterDelete.Checked = true;
                CBCustomerCenter.Checked = true;
                CBSalesCenterEdit.Enabled = true;
                CBSalesCenterDelete.Enabled = true;
                CBSalesCenterEdit.Checked = true;
                CBSalesCenterDelete.Checked = true;
                CBSalesCenter.Checked = true;
                CBCreditNoteEdit.Enabled = true;
                CBCreditNoteDelete.Enabled = true;
                CBCreditNoteEdit.Checked = true;
                CBCreditNoteDelete.Checked = true;
                CBCreditNote.Checked = true;
                CBQuotationEdit.Enabled = true;
                CBQuotationDelete.Enabled = true;
                CBQuotationEdit.Checked = true;
                CBQuotationDelete.Checked = true;
                CBQuotation.Checked = true;
                CBSalesOrderEdit.Enabled = true;
                CBSalesOrderDelete.Enabled = true;
                CBSalesOrderEdit.Checked = true;
                CBSalesOrderDelete.Checked = true;
                CBSalesOrder.Checked = true;
                CBSalesReturnEdit.Enabled = true;
                CBSalesReturnDelete.Enabled = true;
                CBSalesReturnEdit.Checked = true;
                CBSalesReturnDelete.Checked = true;
                CBSalesReturn.Checked = true;
                CBSalesProformaEdit.Enabled = true;
                CBSalesProformaDelete.Enabled = true;
                CBSalesProformaEdit.Checked = true;
                CBSalesProformaDelete.Checked = true;
                CBSalesProforma.Checked = true;
                CBCreateInvoice.Checked = true;
                CBReceiveVoucher.Checked = true;
                btnExpandCustomer.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandCustomer.ImageSize = new Size(20, 20);
            }
            else
            {
                CBCustomerCenter.Checked = false;
                CBCustomerCenterEdit.Enabled = false;
                CBCustomerCenterDelete.Enabled = false;
                CBCustomerCenterEdit.Checked = false;
                CBCustomerCenterDelete.Checked = false;
                CBSalesCenter.Checked = false;
                CBSalesCenterEdit.Enabled = false;
                CBSalesCenterDelete.Enabled = false;
                CBSalesCenterEdit.Checked = false;
                CBSalesCenterDelete.Checked = false;
                CBCreditNoteEdit.Enabled = false;
                CBCreditNoteDelete.Enabled = false;
                CBCreditNoteEdit.Checked = false;
                CBCreditNoteDelete.Checked = false;
                CBCreditNote.Checked = false;
                CBCreditNoteEdit.Enabled = false;
                CBCreditNoteDelete.Enabled = false;
                CBCreditNoteEdit.Checked = false;
                CBCreditNoteDelete.Checked = false;
                CBQuotation.Checked = false;
                CBQuotationEdit.Enabled = false;
                CBQuotationDelete.Enabled = false;
                CBQuotationEdit.Checked = false;
                CBQuotationDelete.Checked = false;
                CBSalesOrder.Checked = false;
                CBSalesOrderEdit.Enabled = false;
                CBSalesOrderDelete.Enabled = false;
                CBSalesOrderEdit.Checked = false;
                CBSalesOrderDelete.Checked = false;
                CBSalesReturnEdit.Enabled = false;
                CBSalesReturnDelete.Enabled = false;
                CBSalesReturnEdit.Checked = false;
                CBSalesReturnDelete.Checked = false;
                CBSalesReturn.Checked = false;
                CBSalesProforma.Checked = false;
                CBSalesProformaEdit.Enabled = false;
                CBSalesProformaDelete.Enabled = false;
                CBSalesProformaEdit.Checked = false;
                CBSalesProformaDelete.Checked = false;
                CBCreateInvoice.Checked = false;
                CBReceiveVoucher.Checked = false;
            }

            updateOrEditData(
               subMenuId: subMenuId("Customer Center"),
               canView: CBCustomerCenter.Checked,
               canEdit: CBCustomerCenterEdit.Checked,
               canDelete: CBCustomerCenterDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Sales Center"),
               canView: CBSalesCenter.Checked,
               canEdit: CBSalesCenterEdit.Checked,
               canDelete: CBSalesCenterDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Create Invoice"),
               canView: CBCreateInvoice.Checked,
               canEdit: CBCreateInvoice.Checked,
               canDelete: CBCreateInvoice.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Receipt Voucher"),
               canView: CBReceiveVoucher.Checked,
               canEdit: CBReceiveVoucher.Checked,
               canDelete: CBReceiveVoucher.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Credit Note"),
               canView: CBCreditNote.Checked,
               canEdit: CBCreditNoteEdit.Checked,
               canDelete: CBCreditNoteDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Quotation"),
               canView: CBQuotation.Checked,
               canEdit: CBQuotationEdit.Checked,
               canDelete: CBQuotationDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Sales Order"),
               canView: CBSalesOrder.Checked,
               canEdit: CBSalesOrderEdit.Checked,
               canDelete: CBSalesOrderDelete.Checked);
            updateOrEditData(
               subMenuId: subMenuId("Sales Return"),
               canView: CBSalesReturn.Checked,
               canEdit: CBSalesReturnEdit.Checked,
               canDelete: CBSalesReturnDelete.Checked);
            updateOrEditData(
              subMenuId: subMenuId("Sales Proforma"),
              canView: CBSalesProforma.Checked,
              canEdit: CBSalesProformaEdit.Checked,
              canDelete: CBSalesProformaDelete.Checked);
        }

        private void CBCustomerCenter_Click(object sender, EventArgs e)
        {
            if (CBCustomerCenter.Checked == true)
            {
                CBCustomerCenterEdit.Enabled = true;
                CBCustomerCenterDelete.Enabled = true;
                CBCustomerCenterEdit.Checked = true;
                CBCustomerCenterDelete.Checked = true;
            }
            else
            {
                CBCustomerCenterEdit.Enabled = false;
                CBCustomerCenterDelete.Enabled = false;
                CBCustomerCenterEdit.Checked = false;
                CBCustomerCenterDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Customer Center"),
               canView: CBCustomerCenter.Checked,
               canEdit: CBCustomerCenterEdit.Checked,
               canDelete: CBCustomerCenterDelete.Checked);
        }

        private void CBSalesCenter_Click(object sender, EventArgs e)
        {
            if (CBSalesCenter.Checked == true)
            {
                CBSalesCenterEdit.Enabled = true;
                CBSalesCenterDelete.Enabled = true;
                CBSalesCenterEdit.Checked = true;
                CBSalesCenterDelete.Checked = true;
            }
            else
            {
                CBSalesCenterEdit.Enabled = false;
                CBSalesCenterDelete.Enabled = false;
                CBSalesCenterEdit.Checked = false;
                CBSalesCenterDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Sales Center"),
               canView: CBSalesCenter.Checked,
               canEdit: CBSalesCenterEdit.Checked,
               canDelete: CBSalesCenterDelete.Checked);
        }

        private void CBCreditNote_Click(object sender, EventArgs e)
        {
            if (CBCreditNote.Checked == true)
            {
                CBCreditNoteEdit.Enabled = true;
                CBCreditNoteDelete.Enabled = true;
                CBCreditNoteEdit.Checked = true;
                CBCreditNoteDelete.Checked = true;
            }
            else
            {
                CBCreditNoteEdit.Enabled = false;
                CBCreditNoteDelete.Enabled = false;
                CBCreditNoteEdit.Checked = false;
                CBCreditNoteDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Credit Note"),
               canView: CBCreditNote.Checked,
               canEdit: CBCreditNoteEdit.Checked,
               canDelete: CBCreditNoteDelete.Checked);
        }

        private void CBQuotation_Click(object sender, EventArgs e)
        {
            if (CBQuotation.Checked == true)
            {
                CBQuotationEdit.Enabled = true;
                CBQuotationDelete.Enabled = true;
                CBQuotationEdit.Checked = true;
                CBQuotationDelete.Checked = true;
            }
            else
            {
                CBQuotationEdit.Enabled = false;
                CBQuotationDelete.Enabled = false;
                CBQuotationEdit.Checked = false;
                CBQuotationDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Quotation"),
               canView: CBQuotation.Checked,
               canEdit: CBQuotationEdit.Checked,
               canDelete: CBQuotationDelete.Checked);
        }

        private void CBSalesOrder_Click(object sender, EventArgs e)
        {
            if (CBSalesOrder.Checked == true)
            {
                CBSalesOrderEdit.Enabled = true;
                CBSalesOrderDelete.Enabled = true;
                CBSalesOrderEdit.Checked = true;
                CBSalesOrderDelete.Checked = true;
            }
            else
            {
                CBSalesOrderEdit.Enabled = false;
                CBSalesOrderDelete.Enabled = false;
                CBSalesOrderEdit.Checked = false;
                CBSalesOrderDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Sales Order"),
               canView: CBSalesOrder.Checked,
               canEdit: CBSalesOrderEdit.Checked,
               canDelete: CBSalesOrderDelete.Checked);
        }

        private void CBSalesReturn_Click(object sender, EventArgs e)
        {
            if (CBSalesReturn.Checked == true)
            {
                CBSalesReturnEdit.Enabled = true;
                CBSalesReturnDelete.Enabled = true;
                CBSalesReturnEdit.Checked = true;
                CBSalesReturnDelete.Checked = true;
            }
            else
            {
                CBSalesReturnEdit.Enabled = false;
                CBSalesReturnDelete.Enabled = false;
                CBSalesReturnEdit.Checked = false;
                CBSalesReturnDelete.Checked = false;
            }
            updateOrEditData(
               subMenuId: subMenuId("Sales Return"),
               canView: CBSalesReturn.Checked,
               canEdit: CBSalesReturnEdit.Checked,
               canDelete: CBSalesReturnDelete.Checked);
        }

        private void CBSalesProforma_Click(object sender, EventArgs e)
        {
            if (CBSalesProforma.Checked == true)
            {
                CBSalesProformaEdit.Enabled = true;
                CBSalesProformaDelete.Enabled = true;
                CBSalesProformaEdit.Checked = true;
                CBSalesProformaDelete.Checked = true;
            }
            else
            {
                CBSalesProformaEdit.Enabled = false;
                CBSalesProformaDelete.Enabled = false;
                CBSalesProformaEdit.Checked = false;
                CBSalesProformaDelete.Checked = false;
            }
            updateOrEditData(
              subMenuId: subMenuId("Sales Proforma"),
              canView: CBSalesProforma.Checked,
              canEdit: CBSalesProformaEdit.Checked,
              canDelete: CBSalesProformaDelete.Checked);
        }
        private void VBALLSELECTVENDOR_Click(object sender, EventArgs e)
        {
            if (CBALLSELECTVENDOR.Checked == true)
            {
                if (pnlVendor.Visible == false)
                {
                    pnlVendor.Visible = true;
                }
                CBVendorCenter.Checked = true;
                CBVendorCenterEdit.Enabled = true;
                CBVendorCenterDelete.Enabled = true;
                CBVendorCenterEdit.Checked = true;
                CBVendorCenterDelete.Checked = true;
                CBPurchasesCenterEdit.Enabled = true;
                CBPurchasesCenterDelete.Enabled = true;
                CBPurchasesCenterEdit.Checked = true;
                CBPurchasesCenterDelete.Checked = true;
                CBPurchasesCenter.Checked = true;
                CBCreatePurchase.Checked = true;
                CBPaymentVoucher.Checked = true;
                CBDebitNoteEdit.Enabled = true;
                CBDebitNoteDelete.Enabled = true;
                CBDebitNoteEdit.Checked = true;
                CBDebitNoteDelete.Checked = true;
                CBDebitNote.Checked = true;
                CBPurchaseOrderEdit.Enabled = true;
                CBPurchaseOrderDelete.Enabled = true;
                CBPurchaseOrderEdit.Checked = true;
                CBPurchaseOrderDelete.Checked = true;
                CBPurchaseOrder.Checked = true;
                CBPurchaseReturn.Checked = true;
                CBPurchaseReturnEdit.Enabled = true;
                CBPurchaseReturnDelete.Enabled = true;
                CBPurchaseReturnEdit.Checked = true;
                CBPurchaseReturnDelete.Checked = true;
                guna2Button5.Image = YamyProject.Properties.Resources.Sort_Down;
                guna2Button5.ImageSize = new Size(20, 20);
            }
            else
            {
                CBVendorCenterEdit.Enabled = false;
                CBVendorCenterDelete.Enabled = false;
                CBVendorCenterEdit.Checked = false;
                CBVendorCenterDelete.Checked = false;
                CBVendorCenter.Checked = false;
                CBPurchasesCenter.Checked = false;
                CBPurchasesCenterEdit.Enabled = false;
                CBPurchasesCenterDelete.Enabled = false;
                CBPurchasesCenterEdit.Checked = false;
                CBPurchasesCenterDelete.Checked = false;
                CBCreatePurchase.Checked = false;
                CBPaymentVoucher.Checked = false;
                CBDebitNoteEdit.Enabled = false;
                CBDebitNoteDelete.Enabled = false;
                CBDebitNoteEdit.Checked = false;
                CBDebitNoteDelete.Checked = false;
                CBDebitNote.Checked = false;
                CBPurchaseOrder.Checked = false;
                CBPurchaseOrderEdit.Enabled = false;
                CBPurchaseOrderDelete.Enabled = false;
                CBPurchaseOrderEdit.Checked = false;
                CBPurchaseOrderDelete.Checked = false;
                CBPurchaseReturn.Checked = false;
                CBPurchaseReturnEdit.Enabled = false;
                CBPurchaseReturnDelete.Enabled = false;
                CBPurchaseReturnEdit.Checked = false;
                CBPurchaseReturnDelete.Checked = false;

            }
            updateOrEditData(
              subMenuId: subMenuId("Vendor Center"),
              canView: CBVendorCenter.Checked,
              canEdit: CBVendorCenterEdit.Checked,
              canDelete: CBVendorCenterDelete.Checked);
            updateOrEditData(
              subMenuId: subMenuId("Purchases Center"),
              canView: CBPurchasesCenter.Checked,
              canEdit: CBPurchasesCenterEdit.Checked,
              canDelete: CBPurchasesCenterDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Create Purchases"),
             canView: CBCreatePurchase.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Payment Voucher"),
             canView: CBPaymentVoucher.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Debit Note"),
             canView: CBDebitNote.Checked,
             canEdit: CBDebitNoteEdit.Checked,
             canDelete: CBDebitNoteDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Purchase Order"),
             canView: CBPurchaseOrder.Checked,
             canEdit: CBPurchaseOrderEdit.Checked,
             canDelete: CBPurchaseOrderDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Purchase Return"),
             canView: CBPurchaseReturn.Checked,
             canEdit: CBPurchaseReturnEdit.Checked,
             canDelete: CBPurchaseReturnDelete.Checked);
        }
        private void CBUserCenter_Click(object sender, EventArgs e)
        {
            if (CBVendorCenter.Checked == true)
            {
                CBVendorCenterEdit.Enabled = true;
                CBVendorCenterDelete.Enabled = true;
                CBVendorCenterEdit.Checked = true;
                CBVendorCenterDelete.Checked = true;
            }
            else
            {
                CBVendorCenterEdit.Enabled = false;
                CBVendorCenterDelete.Enabled = false;
                CBVendorCenterEdit.Checked = false;
                CBVendorCenterDelete.Checked = false;
            }
            updateOrEditData(
              subMenuId: subMenuId("Vendor Center"),
              canView: CBVendorCenter.Checked,
              canEdit: CBVendorCenterEdit.Checked,
              canDelete: CBVendorCenterDelete.Checked);
        }

        private void CBPurchasesCenter_Click(object sender, EventArgs e)
        {
            if (CBPurchasesCenter.Checked == true)
            {
                CBPurchasesCenterEdit.Enabled = true;
                CBPurchasesCenterDelete.Enabled = true;
                CBPurchasesCenterEdit.Checked = true;
                CBPurchasesCenterDelete.Checked = true;
            }
            else
            {
                CBPurchasesCenterEdit.Enabled = false;
                CBPurchasesCenterDelete.Enabled = false;
                CBPurchasesCenterEdit.Checked = false;
                CBPurchasesCenterDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Purchases Center"),
             canView: CBPurchasesCenter.Checked,
             canEdit: CBPurchasesCenterEdit.Checked,
             canDelete: CBPurchasesCenterDelete.Checked);
        }

        private void CBCreatePurchase_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Create Purchases"),
             canView: CBCreatePurchase.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBPaymentVoucher_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Payment Voucher"),
             canView: CBPaymentVoucher.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBDebitNote_Click(object sender, EventArgs e)
        {
            if (CBDebitNote.Checked == true)
            {
                CBDebitNoteEdit.Enabled = true;
                CBDebitNoteDelete.Enabled = true;
                CBDebitNoteEdit.Checked = true;
                CBDebitNoteDelete.Checked = true;
            }
            else
            {
                CBDebitNoteEdit.Enabled = false;
                CBDebitNoteDelete.Enabled = false;
                CBDebitNoteEdit.Checked = false;
                CBDebitNoteDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Debit Note"),
             canView: CBDebitNote.Checked,
             canEdit: CBDebitNoteEdit.Checked,
             canDelete: CBDebitNoteDelete.Checked);
        }

        private void CBPurchaseOrder_Click(object sender, EventArgs e)
        {
            if (CBPurchaseOrder.Checked == true)
            {
                CBPurchaseOrderEdit.Enabled = true;
                CBPurchaseOrderDelete.Enabled = true;
                CBPurchaseOrderEdit.Checked = true;
                CBPurchaseOrderDelete.Checked = true;
            }
            else
            {
                CBPurchaseOrderEdit.Enabled = false;
                CBPurchaseOrderDelete.Enabled = false;
                CBPurchaseOrderEdit.Checked = false;
                CBPurchaseOrderDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Purchase Order"),
             canView: CBPurchaseOrder.Checked,
             canEdit: CBPurchaseOrderEdit.Checked,
             canDelete: CBPurchaseOrderDelete.Checked);
        }

        private void CBPurchaseReturn_Click(object sender, EventArgs e)
        {
            if (CBPurchaseReturn.Checked == true)
            {
                CBPurchaseReturnEdit.Enabled = true;
                CBPurchaseReturnDelete.Enabled = true;
                CBPurchaseReturnEdit.Checked = true;
                CBPurchaseReturnDelete.Checked = true;
            }
            else
            {
                CBPurchaseReturnEdit.Enabled = false;
                CBPurchaseReturnDelete.Enabled = false;
                CBPurchaseReturnEdit.Checked = false;
                CBPurchaseReturnDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Purchase Return"),
             canView: CBPurchaseReturn.Checked,
             canEdit: CBPurchaseReturnEdit.Checked,
             canDelete: CBPurchaseReturnDelete.Checked);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (pnlVendor.Visible == false)
            {
                pnlVendor.Visible = true;
                guna2Button5.Image = YamyProject.Properties.Resources.Sort_Down;
                guna2Button5.ImageSize = new Size(20, 20);
            }
            else
            {
                guna2Button5.Image = YamyProject.Properties.Resources.ight;
                guna2Button5.ImageSize = new Size(15, 15);
                pnlVendor.Visible = false;

            }
        }
        private void btnExpandHR_Click(object sender, EventArgs e)
        {
            if (pnlHR.Visible == false)
            {
                pnlHR.Visible = true;
                btnExpandHR.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandHR.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandHR.Image = YamyProject.Properties.Resources.ight;
                btnExpandHR.ImageSize = new Size(15, 15);
                pnlHR.Visible = false;

            }
        }

        private void guna2CustomCheckBox1_Click(object sender, EventArgs e)
        {
            if (CBSelectAllHr.Checked == true)
            {
                if (pnlHR.Visible == false)
                {
                    pnlHR.Visible = true;
                }
                btnExpandHR.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandHR.ImageSize = new Size(20, 20);
                CBHumanResourceCenterEdit.Enabled = true;
                CBHumanResourceCenterDelete.Enabled = true;
                CBHumanResourceCenterEdit.Checked = true;
                CBHumanResourceCenterDelete.Checked = true;
                CBHumanResourceCenter.Checked = true;
                CBLoansEdit.Enabled = true;
                CBLoansDelete.Enabled = true;
                CBLoansEdit.Checked = true;
                CBLoansDelete.Checked = true;
                CBLoans.Checked = true;
                CBAttendanceSheetEdit.Enabled = true;
                CBAttendanceSheetDelete.Enabled = true;
                CBAttendanceSheetEdit.Checked = true;
                CBAttendanceSheetDelete.Checked = true;
                CBAttendanceSheet.Checked = true;
                CBSalarySheet.Checked = true;
                CBLeaveSalary.Checked = true;
                CBEndOfServices.Checked = true;
                CBFinalSettlementEdit.Enabled = true;
                CBFinalSettlementDelete.Enabled = true;
                CBFinalSettlementEdit.Checked = true;
                CBFinalSettlementDelete.Checked = true;
                CBFinalSettlement.Checked = true;
                CBPaymentVoucher2.Checked = true;
            }
            else
            {
                CBHumanResourceCenterEdit.Enabled = false;
                CBHumanResourceCenterDelete.Enabled = false;
                CBHumanResourceCenterEdit.Checked = false;
                CBHumanResourceCenterDelete.Checked = false;
                CBHumanResourceCenter.Checked = false;
                CBAttendanceSheetEdit.Enabled = false;
                CBAttendanceSheetDelete.Enabled = false;
                CBAttendanceSheetEdit.Checked = false;
                CBAttendanceSheetDelete.Checked = false;
                CBAttendanceSheet.Checked = false;
                CBSalarySheet.Checked =false;
                CBLeaveSalary.Checked = false;
                CBEndOfServices.Checked = false;
                CBLoansEdit.Enabled = false;
                CBLoansDelete.Enabled = false;
                CBLoansEdit.Checked = false;
                CBLoansDelete.Checked = false;
                CBFinalSettlementEdit.Enabled = false;
                CBFinalSettlementDelete.Enabled = false;
                CBFinalSettlementEdit.Checked = false;
                CBFinalSettlementDelete.Checked = false;
                CBFinalSettlement.Checked = false;
                CBPaymentVoucher2.Checked = false;
                CBLoans.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Human Resource Center"),
             canView: CBHumanResourceCenter.Checked,
             canEdit: CBHumanResourceCenterEdit.Checked,
             canDelete: CBHumanResourceCenterDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Attendance Sheet"),
             canView: CBAttendanceSheet.Checked,
             canEdit: CBAttendanceSheetEdit.Checked,
             canDelete: CBAttendanceSheetDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Salary Sheet"),
             canView: CBSalarySheet.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Leave Salary"),
             canView: CBLeaveSalary.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("End Of Services"),
             canView: CBEndOfServices.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Loans"),
             canView: CBLoans.Checked,
             canEdit: CBLoansEdit.Checked,
             canDelete: CBLoansDelete.Checked);
            updateOrEditData(
            subMenuId: subMenuId("Final Settlement"),
            canView: CBFinalSettlement.Checked,
            canEdit: CBFinalSettlementEdit.Checked,
            canDelete: CBFinalSettlementDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Payment Voucher"),
             canView: CBPaymentVoucher2.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void guna2CustomCheckBox16_Click(object sender, EventArgs e)
        {
            if (CBHumanResourceCenter.Checked == true)
            {
                CBHumanResourceCenterEdit.Enabled = true;
                CBHumanResourceCenterDelete.Enabled = true;
                CBHumanResourceCenterEdit.Checked = true;
                CBHumanResourceCenterDelete.Checked = true;
                
            }
            else
            {
                CBHumanResourceCenterEdit.Enabled = false;
                CBHumanResourceCenterDelete.Enabled = false;
                CBHumanResourceCenterEdit.Checked = false;
                CBHumanResourceCenterDelete.Checked = false;
              
            }
            updateOrEditData(
             subMenuId: subMenuId("Human Resource Center"),
             canView: CBHumanResourceCenter.Checked,
             canEdit: CBHumanResourceCenterEdit.Checked,
             canDelete: CBHumanResourceCenterDelete.Checked);
        }

        private void CBAttendanceSheet_Click(object sender, EventArgs e)
        {

            if (CBAttendanceSheet.Checked == true)
            {
                CBAttendanceSheetEdit.Enabled = true;
                CBAttendanceSheetDelete.Enabled = true;
                CBAttendanceSheetEdit.Checked = true;
                CBAttendanceSheetDelete.Checked = true;
            }
            else
            {
                CBAttendanceSheetEdit.Enabled = false;
                CBAttendanceSheetDelete.Enabled = false;
                CBAttendanceSheetEdit.Checked = false;
                CBAttendanceSheetDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Attendance Sheet"),
             canView: CBAttendanceSheet.Checked,
             canEdit: CBAttendanceSheetEdit.Checked,
             canDelete: CBAttendanceSheetDelete.Checked);
        }

        private void CBSalarySheet_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Salary Sheet"),
             canView: CBSalarySheet.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBLoans_Click(object sender, EventArgs e)
        {
            if (CBLoans.Checked == true)
            {
                CBLoansEdit.Enabled = true;
                CBLoansDelete.Enabled = true;
                CBLoansEdit.Checked = true;
                CBLoansDelete.Checked = true;
            }
            else
            {
                CBLoansEdit.Enabled = false;
                CBLoansDelete.Enabled = false;
                CBLoansEdit.Checked = false;
                CBLoansDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Loans"),
             canView: CBLoans.Checked,
             canEdit: CBLoansEdit.Checked,
             canDelete: CBLoansDelete.Checked);
        }

        private void btnExpandHR_Click_1(object sender, EventArgs e)
        {
            if (pnlHR.Visible == false)
            {
                pnlHR.Visible = true;
                btnExpandHR.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandHR.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandHR.Image = YamyProject.Properties.Resources.ight;
                btnExpandHR.ImageSize = new Size(15, 15);
                pnlHR.Visible = false;

            }
        }

        private void CBFinalSettlement_Click(object sender, EventArgs e)
        {
            if (CBFinalSettlement.Checked == true)
            {
                CBFinalSettlementEdit.Enabled = true;
                CBFinalSettlementDelete.Enabled = true;
                CBFinalSettlementEdit.Checked = true;
                CBFinalSettlementDelete.Checked = true;
            }
            else
            {
                CBFinalSettlementEdit.Enabled = false;
                CBFinalSettlementDelete.Enabled = false;
                CBFinalSettlementEdit.Checked = false;
                CBFinalSettlementDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Final Settlement"),
             canView: CBFinalSettlement.Checked,
             canEdit: CBFinalSettlementEdit.Checked,
             canDelete: CBFinalSettlementDelete.Checked);
        }
      
        private void btnExpandBank_Click(object sender, EventArgs e)
        {
            if (pnlbank.Visible == false)
            {
                pnlbank.Visible = true;
                btnExpandBank.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandBank.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandBank.Image = YamyProject.Properties.Resources.ight;
                btnExpandBank.ImageSize = new Size(15, 15);
                pnlbank.Visible = false;

            }
        }

        private void CBSelectAllBank_Click(object sender, EventArgs e)
        {
            if (CBSelectAllBank.Checked == true)
            {
                if (pnlbank.Visible == false)
                {
                    pnlbank.Visible = true;
                }
                btnExpandBank.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandBank.ImageSize = new Size(20, 20);
                CBBankCenterDelete.Enabled = true;
                CBBankCenterDelete.Checked = true;
                CBBankCenter.Checked = true;
                CBOpenBankCardEdit.Enabled = true;
                CBOpenBankCardDelete.Enabled = true;
                CBOpenBankCardEdit.Checked = true;
                CBOpenBankCardDelete.Checked = true;
                CBOpenBankCard.Checked = true;
                CBCheques.Checked = true;
                CBPDC.Checked = true;
            }
            else
            {
                CBBankCenter.Checked = false;
                CBBankCenterDelete.Enabled = false;
                CBBankCenterDelete.Checked = false;
                CBOpenBankCardEdit.Enabled = false;
                CBOpenBankCardDelete.Enabled = false;
                CBOpenBankCardEdit.Checked = false;
                CBOpenBankCardDelete.Checked = false;
                CBOpenBankCard.Checked = false;
                CBCheques.Checked = false;
                CBPDC.Checked = false;

            }
            updateOrEditData(
             subMenuId: subMenuId("Bank Center"),
             canView: CBBankCenter.Checked,
             canEdit: false,
             canDelete: CBBankCenterDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Open Bank Card"),
             canView: CBOpenBankCard.Checked,
             canEdit: CBOpenBankCardEdit.Checked,
             canDelete: CBOpenBankCardDelete.Checked);
            updateOrEditData(
             subMenuId: subMenuId("Cheques"),
             canView: CBCheques.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("PDC"),
             canView: CBPDC.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBBankCenter_Click(object sender, EventArgs e)
        {
            if (CBBankCenter.Checked == true)
            {
                CBBankCenterDelete.Enabled = true;
                CBBankCenterDelete.Checked = true;
            }
            else
            {
                CBBankCenterDelete.Enabled = false;
                CBBankCenterDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Bank Center"),
             canView: CBBankCenter.Checked,
             canEdit: false,
             canDelete: CBBankCenterDelete.Checked);
        }

        private void CBOpenBankCard_Click(object sender, EventArgs e)
        {
            if (CBOpenBankCard.Checked == true)
            {
                CBOpenBankCardEdit.Enabled = true;
                CBOpenBankCardDelete.Enabled = true;
                CBOpenBankCardEdit.Checked = true;
                CBOpenBankCardDelete.Checked = true;
            }
            else
            {
                CBOpenBankCardEdit.Enabled = false;
                CBOpenBankCardDelete.Enabled = false;
                CBOpenBankCardEdit.Checked = false;
                CBOpenBankCardDelete.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Open Bank Card"),
             canView: CBOpenBankCard.Checked,
             canEdit: CBOpenBankCardEdit.Checked,
             canDelete: CBOpenBankCardDelete.Checked);
        }

        private void btnExpandAccess_Click(object sender, EventArgs e)
        {
            if (pnlReport.Visible == false)
            {
                pnlReport.Visible = true;
                btnExpandAccess.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandAccess.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandAccess.Image = YamyProject.Properties.Resources.ight;
                btnExpandAccess.ImageSize = new Size(15, 15);
                pnlReport.Visible = false;

            }
        }

        private void CBSelectAllReport_Click(object sender, EventArgs e)
        {
            if (CBSelectAllReport.Checked == true)
            {
                if (pnlReport.Visible == false)
                {
                    pnlReport.Visible = true;
                }
                CBCompany.Checked = true;
                CBCustomerReceivable.Checked = true;
                CBSales.Checked = true;
                CBVendorPayable.Checked = true;
                CBPurchases.Checked = true;
                CBEmployees.Checked = true;
                CBAccountant.Checked = true;
                CBInventory2.Checked = true;
                CBList.Checked = true;
            }
            else
            {
                CBCompany.Checked = false;
                CBCustomerReceivable.Checked = false;
                CBSales.Checked = false;
                CBVendorPayable.Checked = false;
                CBPurchases.Checked = false;
                CBEmployees.Checked = false;
                CBAccountant.Checked = false;
                CBInventory2.Checked = false;
                CBList.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Company"),
             canView: CBCompany.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Customer & Receivable"),
             canView: CBCustomerReceivable.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Sales"),
             canView: CBSales.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Vendor & Payable"),
             canView: CBVendorPayable.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Purchases"),
             canView: CBPurchases.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Employees"),
             canView: CBEmployees.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Accountant"),
             canView: CBAccountant.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Inventory"),
             canView: CBInventory2.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("List"),
             canView: CBList.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void btnExpandSetting_Click(object sender, EventArgs e)
        {
            if (pnlsettingn.Visible == false)
            {
                pnlsettingn.Visible = true;
                btnExpandSetting.Image = YamyProject.Properties.Resources.Sort_Down;
                btnExpandSetting.ImageSize = new Size(20, 20);
            }
            else
            {
                btnExpandSetting.Image = YamyProject.Properties.Resources.ight;
                btnExpandSetting.ImageSize = new Size(15, 15);
                pnlsettingn.Visible = false;

            }
        }

        private void CBSelectAllSetting_Click(object sender, EventArgs e)
        {
            if (CBSelectAllSetting.Checked == true)
            {
                if (pnlsettingn.Visible == false)
                {
                    pnlsettingn.Visible = true;
                }
                cbSettings.Checked = true;
                CBChangeCurrentPassword.Checked = true;
                CBClearData.Checked = true;
                CBUsers.Checked = true;
            }
            else
            {
                cbSettings.Checked = false;
                CBChangeCurrentPassword.Checked = false;
                CBClearData.Checked = false;
                CBUsers.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Setting"),
             canView: cbSettings.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Change Current Password"),
             canView: CBChangeCurrentPassword.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Clear Data"),
             canView: CBClearData.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Users"),
             canView: CBUsers.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (pnlFileAccess.Visible == false)
            {
                pnlFileAccess.Visible = true;
                btnFile.Image = YamyProject.Properties.Resources.Sort_Down;
                btnFile.ImageSize = new Size(20, 20);
            }
            else
            {
                btnFile.Image = YamyProject.Properties.Resources.ight;
                btnFile.ImageSize = new Size(15, 15);
                pnlFileAccess.Visible = false;

            }
        }

        private void CBSelectAllFile_Click(object sender, EventArgs e)
        {
            if (CBSelectAllFile.Checked == true)
            {
                if (pnlFileAccess.Visible == false)
                {
                    pnlFileAccess.Visible = true;
                }
                CBCompanyList.Checked = true;
                CBBackUpCompany.Checked = true;
                CBRestoreCompany.Checked = true;

            }
            else
            {
                CBCompanyList.Checked = false;
                CBBackUpCompany.Checked = false;
                CBRestoreCompany.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Company List"),
             canView: CBCompanyList.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Back Up Company"),
             canView: CBBackUpCompany.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Restore Company"),
             canView: CBRestoreCompany.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBChartofaccountEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
                subMenuId: subMenuId("Chart Of Account"),
                canView: CBChartofaccount.Checked,
                canEdit: CBChartofaccountEdit.Checked,
                canDelete: CBChartofaccountDelete.Checked);
        }

        private void CBChartofaccountDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
                subMenuId: subMenuId("Chart Of Account"),
                canView: CBChartofaccount.Checked,
                canEdit: CBChartofaccountEdit.Checked,
                canDelete: CBChartofaccountDelete.Checked);
        }

        private void CBCostCenterEdit_Click(object sender, EventArgs e)
        {

            updateOrEditData(
                subMenuId: subMenuId("Cost Center"),
                canView: CBCostCenter.Checked,
                canEdit: CBCostCenterEdit.Checked,
                canDelete: CBCostCenterDelete.Checked);
        }

        private void CBCostCenterDelete_Click(object sender, EventArgs e)
        {

            updateOrEditData(
                subMenuId: subMenuId("Cost Center"),
                canView: CBCostCenter.Checked,
                canEdit: CBCostCenterEdit.Checked,
                canDelete: CBCostCenterDelete.Checked);
        }

        private void CBTransactionsJEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
                subMenuId: subMenuId("Transactions Journal"),
                canView: CBTransactionsJ.Checked,
                canEdit: CBTransactionsJEdit.Checked,
                canDelete: CBTransactionsJDelete.Checked);
        }

        private void CBTransactionsJDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
                subMenuId: subMenuId("Transactions Journal"),
                canView: CBTransactionsJ.Checked,
                canEdit: CBTransactionsJEdit.Checked,
                canDelete: CBTransactionsJDelete.Checked);
        }

        private void CBFixedAssetsEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Fixed Assets"),
               canView: CBFixedAssets.Checked,
               canEdit: CBFixedAssetsEdit.Checked,
               canDelete: CBFixedAssetsDelete.Checked);
        }

        private void CBFixedAssetsDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Fixed Assets"),
               canView: CBFixedAssets.Checked,
               canEdit: CBFixedAssetsEdit.Checked,
               canDelete: CBFixedAssetsDelete.Checked);
        }

        private void CBVouchersEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Vouchers"),
               canView: CBVouchers.Checked,
               canEdit: CBVouchersEdit.Checked,
               canDelete: CBVouchersDelete.Checked);
        }

        private void CBVouchersDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Vouchers"),
               canView: CBVouchers.Checked,
               canEdit: CBVouchersEdit.Checked,
               canDelete: CBVouchersDelete.Checked);
        }

        private void CBPrepaidEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Prepaid Expense"),
               canView: CBPrepaid.Checked,
               canEdit: CBPrepaidEdit.Checked,
               canDelete: CBPrepaidDelete.Checked);
        }

        private void CBPrepaidDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Prepaid Expense"),
               canView: CBPrepaid.Checked,
               canEdit: CBPrepaidEdit.Checked,
               canDelete: CBPrepaidDelete.Checked);
        }

        private void CBPettyEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Petty Cash"),
               canView: CBPetty.Checked,
               canEdit: CBPettyEdit.Checked,
               canDelete: CBPettyDelete.Checked);
        }

        private void CBPettyDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Petty Cash"),
               canView: CBPetty.Checked,
               canEdit: CBPettyEdit.Checked,
               canDelete: CBPettyDelete.Checked);
        }

        private void CBSTOCKEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Stock Management"),
               canView: CBSTOCK.Checked,
               canEdit: CBSTOCKEdit.Checked,
               canDelete: CBSTOCKDelete.Checked);
        }

        private void CBSTOCKDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Stock Management"),
               canView: CBSTOCK.Checked,
               canEdit: CBSTOCKEdit.Checked,
               canDelete: CBSTOCKDelete.Checked);
        }

        private void CBInventoryCenterEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Inventory Center"),
               canView: CBInventoryCenter.Checked,
               canEdit: CBInventoryCenterEdit.Checked,
               canDelete: CBInventoryCenterDelete.Checked);
        }

        private void CBInventoryCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Inventory Center"),
               canView: CBInventoryCenter.Checked,
               canEdit: CBInventoryCenterEdit.Checked,
               canDelete: CBInventoryCenterDelete.Checked);
        }

        private void CBWarehouseCenterEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Warehouse Center"),
               canView: CBWarehouseCenter.Checked,
               canEdit: CBWarehouseCenterEdit.Checked,
               canDelete: CBWarehouseCenterDelete.Checked);
        }

        private void CBWarehouseCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Warehouse Center"),
               canView: CBWarehouseCenter.Checked,
               canEdit: CBWarehouseCenterEdit.Checked,
               canDelete: CBWarehouseCenterDelete.Checked);
        }

        private void CBCustomerCenterEdit_Click(object sender, EventArgs e)
        {

            updateOrEditData(
               subMenuId: subMenuId("Customer Center"),
               canView: CBCustomerCenter.Checked,
               canEdit: CBCustomerCenterEdit.Checked,
               canDelete: CBCustomerCenterDelete.Checked);
        }

        private void CBCustomerCenterDelete_Click(object sender, EventArgs e)
        {

            updateOrEditData(
               subMenuId: subMenuId("Customer Center"),
               canView: CBCustomerCenter.Checked,
               canEdit: CBCustomerCenterEdit.Checked,
               canDelete: CBCustomerCenterDelete.Checked);
        }

        private void CBSalesCenterEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Sales Center"),
               canView: CBSalesCenter.Checked,
               canEdit: CBSalesCenterEdit.Checked,
               canDelete: CBSalesCenterDelete.Checked);
        }

        private void CBSalesCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Sales Center"),
               canView: CBSalesCenter.Checked,
               canEdit: CBSalesCenterEdit.Checked,
               canDelete: CBSalesCenterDelete.Checked);
        }

        private void CBCreateInvoice_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Create Invoice"),
               canView: CBCreateInvoice.Checked,
               canEdit: CBCreateInvoice.Checked,
               canDelete: CBCreateInvoice.Checked);
        }

        private void CBReceiveVoucher_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Receipt Voucher"),
               canView: CBReceiveVoucher.Checked,
               canEdit: CBReceiveVoucher.Checked,
               canDelete: CBReceiveVoucher.Checked);
        }

        private void CBCreditNoteEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Credit Note"),
               canView: CBCreditNote.Checked,
               canEdit: CBCreditNoteEdit.Checked,
               canDelete: CBCreditNoteDelete.Checked);
        }

        private void CBCreditNoteDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Credit Note"),
               canView: CBCreditNote.Checked,
               canEdit: CBCreditNoteEdit.Checked,
               canDelete: CBCreditNoteDelete.Checked);
        }

        private void CBQuotationEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Quotation"),
               canView: CBQuotation.Checked,
               canEdit: CBQuotationEdit.Checked,
               canDelete: CBQuotationDelete.Checked);
        }

        private void CBQuotationDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Quotation"),
               canView: CBQuotation.Checked,
               canEdit: CBQuotationEdit.Checked,
               canDelete: CBQuotationDelete.Checked);
        }

        private void CBSalesOrderEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Sales Order"),
               canView: CBSalesOrder.Checked,
               canEdit: CBSalesOrderEdit.Checked,
               canDelete: CBSalesOrderDelete.Checked);
        }

        private void CBSalesOrderDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Sales Order"),
               canView: CBSalesOrder.Checked,
               canEdit: CBSalesOrderEdit.Checked,
               canDelete: CBSalesOrderDelete.Checked);
        }

        private void CBSalesReturnEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Sales Return"),
               canView: CBSalesReturn.Checked,
               canEdit: CBSalesReturnEdit.Checked,
               canDelete: CBSalesReturnDelete.Checked);
        }

        private void CBSalesReturnDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
               subMenuId: subMenuId("Sales Return"),
               canView: CBSalesReturn.Checked,
               canEdit: CBSalesReturnEdit.Checked,
               canDelete: CBSalesReturnDelete.Checked);
        }

        private void CBSalesProformaEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
              subMenuId: subMenuId("Sales Proforma"),
              canView: CBSalesProforma.Checked,
              canEdit: CBSalesProformaEdit.Checked,
              canDelete: CBSalesProformaDelete.Checked);
        }

        private void CBSalesProformaDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
              subMenuId: subMenuId("Sales Proforma"),
              canView: CBSalesProforma.Checked,
              canEdit: CBSalesProformaEdit.Checked,
              canDelete: CBSalesProformaDelete.Checked);
        }

        private void CBVendorCenterEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
              subMenuId: subMenuId("Vendor Center"),
              canView: CBVendorCenter.Checked,
              canEdit: CBVendorCenterEdit.Checked,
              canDelete: CBVendorCenterDelete.Checked);
        }

        private void CBVendorCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
              subMenuId: subMenuId("Vendor Center"),
              canView: CBVendorCenter.Checked,
              canEdit: CBVendorCenterEdit.Checked,
              canDelete: CBVendorCenterDelete.Checked);
        }

        private void CBPurchasesCenterEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchases Center"),
             canView: CBPurchasesCenter.Checked,
             canEdit: CBPurchasesCenterEdit.Checked,
             canDelete: CBPurchasesCenterDelete.Checked);
        }

        private void CBPurchasesCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchases Center"),
             canView: CBPurchasesCenter.Checked,
             canEdit: CBPurchasesCenterEdit.Checked,
             canDelete: CBPurchasesCenterDelete.Checked);
        }

        private void CBDebitNoteEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Debit Note"),
             canView: CBDebitNote.Checked,
             canEdit: CBDebitNoteEdit.Checked,
             canDelete: CBDebitNoteDelete.Checked);
        }

        private void CBDebitNoteDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Debit Note"),
             canView: CBDebitNote.Checked,
             canEdit: CBDebitNoteEdit.Checked,
             canDelete: CBDebitNoteDelete.Checked);
        }

        private void CBPurchaseOrderEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchase Order"),
             canView: CBPurchaseOrder.Checked,
             canEdit: CBPurchaseOrderEdit.Checked,
             canDelete: CBPurchaseOrderDelete.Checked);
        }

        private void CBPurchaseOrderDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchase Order"),
             canView: CBPurchaseOrder.Checked,
             canEdit: CBPurchaseOrderEdit.Checked,
             canDelete: CBPurchaseOrderDelete.Checked);
        }

        private void CBPurchaseReturnEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchase Return"),
             canView: CBPurchaseReturn.Checked,
             canEdit: CBPurchaseReturnEdit.Checked,
             canDelete: CBPurchaseReturnDelete.Checked);
        }

        private void CBPurchaseReturnDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchase Return"),
             canView: CBPurchaseReturn.Checked,
             canEdit: CBPurchaseReturnEdit.Checked,
             canDelete: CBPurchaseReturnDelete.Checked);
        }

        private void CBHumanResourceCenterEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Human Resource Center"),
             canView: CBHumanResourceCenter.Checked,
             canEdit: CBHumanResourceCenterEdit.Checked,
             canDelete: CBHumanResourceCenterDelete.Checked);
        }

        private void CBHumanResourceCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Human Resource Center"),
             canView: CBHumanResourceCenter.Checked,
             canEdit: CBHumanResourceCenterEdit.Checked,
             canDelete: CBHumanResourceCenterDelete.Checked);
        }

        private void CBAttendanceSheetEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Attendance Sheet"),
             canView: CBAttendanceSheet.Checked,
             canEdit: CBAttendanceSheetEdit.Checked,
             canDelete: CBAttendanceSheetDelete.Checked);
        }

        private void CBAttendanceSheetDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Attendance Sheet"),
             canView: CBAttendanceSheet.Checked,
             canEdit: CBAttendanceSheetEdit.Checked,
             canDelete: CBAttendanceSheetDelete.Checked);
        }

        private void CBLeaveSalary_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Leave Salary"),
             canView: CBLeaveSalary.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBEndOfServices_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("End Of Services"),
             canView: CBEndOfServices.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBLoansEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Loans"),
             canView: CBLoans.Checked,
             canEdit: CBLoansEdit.Checked,
             canDelete: CBLoansDelete.Checked);
        }

        private void CBLoansDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Loans"),
             canView: CBLoans.Checked,
             canEdit: CBLoansEdit.Checked,
             canDelete: CBLoansDelete.Checked);
        }

        private void CBFinalSettlementEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
            subMenuId: subMenuId("Final Settlement"),
            canView: CBFinalSettlement.Checked,
            canEdit: CBFinalSettlementEdit.Checked,
            canDelete: CBFinalSettlementDelete.Checked);
        }

        private void CBFinalSettlementDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
            subMenuId: subMenuId("Final Settlement"),
            canView: CBFinalSettlement.Checked,
            canEdit: CBFinalSettlementEdit.Checked,
            canDelete: CBFinalSettlementDelete.Checked);
        }

        private void CBPaymentVoucher2_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Payment Voucher"),
             canView: CBPaymentVoucher2.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBBankCenterDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Bank Center"),
             canView: CBBankCenter.Checked,
             canEdit: false,
             canDelete: CBBankCenterDelete.Checked);
        }

        private void CBOpenBankCardEdit_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Open Bank Card"),
             canView: CBOpenBankCard.Checked,
             canEdit: CBOpenBankCardEdit.Checked,
             canDelete: CBOpenBankCardDelete.Checked);
        }

        private void CBOpenBankCardDelete_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Open Bank Card"),
             canView: CBOpenBankCard.Checked,
             canEdit: CBOpenBankCardEdit.Checked,
             canDelete: CBOpenBankCardDelete.Checked);
        }

        private void CBCheques_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Cheques"),
             canView: CBCheques.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBPDC_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("PDC"),
             canView: CBPDC.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBCompany_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Company"),
             canView: CBCompany.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBCustomerReceivable_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Customer & Receivable"),
             canView: CBCustomerReceivable.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBSales_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Sales"),
             canView: CBSales.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBVendorPayable_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Vendor & Payable"),
             canView: CBVendorPayable.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBPurchases_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Purchases"),
             canView: CBPurchases.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBEmployees_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Employees"),
             canView: CBEmployees.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBAccountant_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Accountant"),
             canView: CBAccountant.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBInventory2_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Inventory"),
             canView: CBInventory2.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBList_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("List"),
             canView: CBList.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void cbSettings_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Setting"),
             canView: cbSettings.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBChangeCurrentPassword_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Change Current Password"),
             canView: CBChangeCurrentPassword.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBClearData_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Clear Data"),
             canView: CBClearData.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBUsers_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Users"),
             canView: CBUsers.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBCompanyList_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Company List"),
             canView: CBCompanyList.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBBackUpCompany_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Back Up Company"),
             canView: CBBackUpCompany.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBRestoreCompany_Click(object sender, EventArgs e)
        {
            updateOrEditData(
             subMenuId: subMenuId("Restore Company"),
             canView: CBRestoreCompany.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBSelectAllConstruction_Click(object sender, EventArgs e)
        {
            if (CBSelectAllConstruction.Checked == true)
            {
                if (pnlConstructionAccess.Visible == false)
                {
                    pnlConstructionAccess.Visible = true;
                }
                CBProjectDashBoard.Checked = true;
                CBProjectTender.Checked = true;
                CBProjectEstimate.Checked = true;
                CBProjectPlanning.Checked = true;

            }
            else
            {
                CBProjectDashBoard.Checked = false;
                CBProjectTender.Checked = false;
                CBProjectEstimate.Checked = false;
                CBProjectPlanning.Checked = false;
            }
            updateOrEditData(
             subMenuId: subMenuId("Project DashBoard"),
             canView: CBProjectDashBoard.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Project Tender"),
             canView: CBProjectTender.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Project Estimate"),
             canView: CBProjectEstimate.Checked,
             canEdit: false,
             canDelete: false);
            updateOrEditData(
             subMenuId: subMenuId("Project Planning"),
             canView: CBProjectPlanning.Checked,
             canEdit: false,
             canDelete: false);
        }

        private void CBProjectDashBoard_Click(object sender, EventArgs e)
        {

        }

        private void CBProjectTender_Click(object sender, EventArgs e)
        {

        }

        private void CBProjectEstimate_Click(object sender, EventArgs e)
        {

        }

        private void CBProjectPlanning_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            frmLogin.frmMain.LoadFormIntoPanel(new frmUserActivityReport(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
    }
}
