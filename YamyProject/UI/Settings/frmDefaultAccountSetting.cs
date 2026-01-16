using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmDefaultAccountSetting : Form
    {
        private Dictionary<string, Guna2ComboBox> accountCategoryComboBoxMap;

        public frmDefaultAccountSetting()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            InitializeComboBoxMapping();
        }

        private void InitializeComboBoxMapping()
        {
            accountCategoryComboBoxMap = new Dictionary<string, Guna2ComboBox>
            {
                { "Sales", cmbSalesInvoice },
                { "COGS", cmbItemCogs },
                { "Default Account For Cash", cmbCash },
                { "Inventory", cmbInventory },
                { "Item Damage", cmbItemDamage },
                { "Stock Settlement", cmbStockSettlement },
                { "Invoice Payment Cash Method", cmbInvoicePaymentCashMethod },
                { "Vat Output", cmbVatOutput },
                { "Vendor", cmbVendor },
                { "Vat Input", cmbVatInput },
                { "Purchase Payment Cash Method", cmbPurchasePaymentCashMethod },
                { "Opening Balance", cmbOpeningBalance },
                { "Opening Balance Equity", cmbOpeningBalanceEquity },
                { "Accrued Salaries", cmbAccruedSalaries },
                { "Salaries", cmbSalaries },
                { "Acroal Leave Salary", cmbAcroalLeaveSalary },
                { "Employee Receivable", cmbEmployeeRecivable },
                { "Gratuit", cmbGratuit },
                { "Prepaid Expense Debit Account", cmbPrepaidExpenseDebitAccount },
                { "Prepaid Expense Credit Account", cmbPrepaidExpenseCreditAccount },
                { "Fixed Asset Debit Account", cmbFixedAssetDebitAccount },
                { "Fixed Asset Credit Account", cmbFixedAssetCreditAccount },
                { "PDC Payable", cmbPDCPayable },
                { "PDC Payable Return", cmbPDCPayableReturn },
                { "PDC Payable Hold", cmbPDCPayableHold },
                { "PDC Receivable", cmbPDCReceivable },
                { "PDC Receivable Return", cmbPDCReceivableReturn },
                { "PDC Receivable Hold", cmbPDCReceivableHold },
                { "Customer", cmbCustomer },
                { "End of Service Debit", cmbEndofServiceDebit },
                { "End of Service Credit", cmbEndofServiceCredit },
                { "Leave Salary Debit", cmbLeaveSalaryDebit },
                { "Leave Salary Credit", cmbLeaveSalaryCredit },
                { "Petty Cash Account", cmbPettyCashAccount },
                { "Purchase", cmbPurchaseInvoice },
                { "PurchaseReturn", cmbPurchaseReturnInvoice },
                { "SalesReturn", cmbSalesReturnInvoice },
                { "Default Account For Bank", cmbDefaultBankAccount },
            };
        }

        private void frmDefaultAccountSetting_Load(object sender, EventArgs e)
        {
            // Load cache once
            LoadCache();

            // Show default panel and load combos only for it
            ShowPanelAndLoadCombos(pnlCommon);
        }

        private void LoadCache()
        {
            BindDataTable.coaConfigDict = BindDataTable.tableCoaConfig.AsEnumerable()
                .GroupBy(row => row.Field<string>("category"))
                .ToDictionary(g => g.Key, g => g.First().Field<int>("account_id"));
        }

        private void ShowPanelAndLoadCombos(System.Windows.Forms.Panel panelToShow)
        {
            // Hide all panels
            pnlCommon.Visible = pnlBank.Visible = pnlEmplyee.Visible = pnlCustomersales.Visible = pnlinventory.Visible = pnlVendorPurchase.Visible = false;

            // Show target panel
            panelToShow.Visible = true;

            // Load combos for this panel only
            if (panelToShow == pnlCommon)
            {
                LoadCombos(new string[]
                {
                "Salaries", "Accrued Salaries", "Default Account For Cash", "Petty Cash Account",
                "Opening Balance", "Opening Balance Equity", "PDC Receivable", "PDC Receivable Return", "PDC Receivable Hold",
                "PDC Payable", "PDC Payable Return", "PDC Payable Hold", "Prepaid Expense Debit Account", "Prepaid Expense Credit Account",
                "Fixed Asset Debit Account", "Fixed Asset Credit Account"
                });
            }
            else if (panelToShow == pnlVendorPurchase)
            {
                LoadCombos(new string[]
                {
                "Vendor", "Vat Input", "Purchase Payment Cash Method", "Purchase", "PurchaseReturn"
                });
            }
            else if (panelToShow == pnlinventory)
            {
                LoadCombos(new string[]
                {
                "Inventory", "COGS", "Item Damage", "Stock Settlement"
                });
            }
            else if (panelToShow == pnlCustomersales)
            {
                LoadCombos(new string[]
                {
                "Customer", "Invoice Payment Cash Method", "Vat Output", "Sales", "SalesReturn"
                });
            }
            else if (panelToShow == pnlEmplyee)
            {
                LoadCombos(new string[]
                {
                "Accrued Salaries", "Salaries", "Acroal Leave Salary", "Employee Receivable", "Gratuit",
                "End of Service Debit", "End of Service Credit", "Leave Salary Debit", "Leave Salary Credit"
                });
            }
            else if (panelToShow == pnlBank)
            {
                LoadCombos(new string[]
                {
                "Default Account For Bank"
                });
            }
        }

        private void LoadCombos(IEnumerable<string> categories)
        {
            foreach (var category in categories)
            {
                if (accountCategoryComboBoxMap.TryGetValue(category, out Guna2ComboBox comboBox))
                {
                    AccountSettingsHelper.LoadAndSelect(comboBox, category);
                }
            }
        }

        // Buttons to switch panels
        private void guna2Button1_Click(object sender, EventArgs e) => ShowPanelAndLoadCombos(pnlCommon);
        private void guna2Button3_Click(object sender, EventArgs e) => ShowPanelAndLoadCombos(pnlVendorPurchase);
        private void guna2Button5_Click(object sender, EventArgs e) => ShowPanelAndLoadCombos(pnlinventory);
        private void guna2Button6_Click(object sender, EventArgs e) => ShowPanelAndLoadCombos(pnlCustomersales);
        private void guna2Button2_Click(object sender, EventArgs e) => ShowPanelAndLoadCombos(pnlEmplyee);
        private void guna2Button4_Click(object sender, EventArgs e) => ShowPanelAndLoadCombos(pnlBank);

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear existing settings before saving new ones
                //DBClass.ExecuteNonQuery("DELETE FROM tbl_coa_config;");

                foreach (var entry in accountCategoryComboBoxMap)
                {
                    string category = entry.Key;
                    Guna2ComboBox comboBox = entry.Value;

                    int accountId = -1;

                    // Safely get the selected value as int from data bound combobox
                    if (comboBox.SelectedValue != null && int.TryParse(comboBox.SelectedValue.ToString(), out int val))
                    {
                        accountId = val;
                    }

                    if (accountId > 0)
                    {
                        string query = @"
                                        INSERT INTO tbl_coa_config (category, account_id) 
                                        VALUES (@category, @account_id)
                                        ON DUPLICATE KEY UPDATE account_id = @account_id;";

                        DBClass.ExecuteNonQuery(query,
                            DBClass.CreateParameter("@category", category),
                            DBClass.CreateParameter("@account_id", accountId));
                    }
                }

                MessageBox.Show("Default account settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                BindDataTable.LoadConfigData();
                LoadCache();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving default accounts: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

public class ComboBoxItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public override string ToString() => Name;
}

