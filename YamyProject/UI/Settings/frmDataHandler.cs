using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmDataHandler : Form
    {
        public frmDataHandler()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = "Data Handler";
        }
        
        private void btnClear_Click(object sender, EventArgs e)
        {
            DeleteDataWithPasswordPrompt();
        }
        private void DeleteDataWithPasswordPrompt()
        {
            using (var passwordForm = new PasswordPromptForm("Please enter the password to confirm deletion:"))
            {
                if (passwordForm.ShowDialog() == DialogResult.OK)
                {
                    string enteredPassword = passwordForm.EnteredPassword;

                    if (enteredPassword == SecurityConfig.DeveloperPassword)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete all data?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                string query = "";
                                if (ChkAll.Checked)
                                {
                                    query = @"
                            TRUNCATE TABLE tbl_attendancesheet;
                            TRUNCATE TABLE tbl_attendance_salary;
                            TRUNCATE TABLE tbl_bank_card;
                            TRUNCATE TABLE tbl_bank_register;
                            TRUNCATE TABLE tbl_bank;
                            TRUNCATE TABLE tbl_check_details;
                            TRUNCATE TABLE tbl_cheque;
                            TRUNCATE TABLE tbl_corporate_tax_configration;
                            TRUNCATE TABLE tbl_cost_center;
                            TRUNCATE TABLE tbl_credit_note;
                            TRUNCATE TABLE tbl_credit_note_details;
                            TRUNCATE TABLE tbl_customer;
                            TRUNCATE TABLE tbl_customer_category;
                            TRUNCATE TABLE tbl_damage;
                            TRUNCATE TABLE tbl_damage_details;
                            TRUNCATE TABLE tbl_debit_note;
                            TRUNCATE TABLE tbl_debit_note_details;
                            TRUNCATE TABLE tbl_departments;
                            TRUNCATE TABLE tbl_employee;
                            TRUNCATE TABLE tbl_end_of_service;
                            TRUNCATE TABLE tbl_final_settlement;
                            TRUNCATE TABLE tbl_fixed_assets;
                            TRUNCATE TABLE tbl_fixed_assets_category;
                            TRUNCATE TABLE tbl_items;
                            TRUNCATE TABLE tbl_items_unit;
                            TRUNCATE TABLE tbl_items_warehouse;
                            TRUNCATE TABLE tbl_item_assembly;
                            TRUNCATE TABLE tbl_item_category;
                            TRUNCATE TABLE tbl_item_stock_settlement;
                            TRUNCATE TABLE tbl_item_stock_settlement_details;
                            TRUNCATE TABLE tbl_item_transaction;
                            TRUNCATE TABLE tbl_item_warehouse_transaction;
                            TRUNCATE TABLE tbl_journal_voucher;
                            TRUNCATE TABLE tbl_journal_voucher_details;
                            TRUNCATE TABLE tbl_leave_salary;
                            TRUNCATE TABLE tbl_loan;
                            TRUNCATE TABLE tbl_payment_voucher;
                            TRUNCATE TABLE tbl_payment_voucher_details;
                            TRUNCATE TABLE tbl_petty_cash_card;
                            TRUNCATE TABLE tbl_petty_cash_category;
                            TRUNCATE TABLE tbl_petty_cash_request;
                            TRUNCATE TABLE tbl_petty_cash_submition;
                            TRUNCATE TABLE tbl_petty_cash_submition_details;
                            TRUNCATE TABLE tbl_petty_cash;
                            TRUNCATE TABLE tbl_petty_cash_details;
                            TRUNCATE TABLE tbl_position;
                            TRUNCATE TABLE tbl_prepaid_expense;
                            TRUNCATE TABLE tbl_prepaid_expense_category;
                            TRUNCATE TABLE tbl_purchase;
                            TRUNCATE TABLE tbl_purchase_details;
                            TRUNCATE TABLE tbl_purchase_order;
                            TRUNCATE TABLE tbl_purchase_order_details;
                            TRUNCATE TABLE tbl_purchase_return;
                            TRUNCATE TABLE tbl_purchase_return_details;
                            TRUNCATE TABLE tbl_receipt_voucher;
                            TRUNCATE TABLE tbl_receipt_voucher_details;
                            TRUNCATE TABLE tbl_salary;
                            TRUNCATE TABLE tbl_sales;
                            TRUNCATE TABLE tbl_sales_details;
                            TRUNCATE TABLE tbl_sales_order;
                            TRUNCATE TABLE tbl_sales_order_details;
                            TRUNCATE TABLE tbl_sales_quotation;
                            TRUNCATE TABLE tbl_sales_quotation_details;
                            TRUNCATE TABLE tbl_sales_return;
                            TRUNCATE TABLE tbl_sales_return_details;
                            TRUNCATE TABLE tbl_transaction;
                            TRUNCATE TABLE tbl_sub_cost_center;
                            TRUNCATE TABLE tbl_unit;
                            TRUNCATE TABLE tbl_vat;
                            TRUNCATE TABLE tbl_vendor;
                            TRUNCATE TABLE tbl_vendor_category;
                            TRUNCATE TABLE tbl_warehouse;
                            TRUNCATE TABLE tbl_salary_adjustments;
                            TRUNCATE TABLE tbl_advance_payment_voucher;
                            TRUNCATE TABLE tbl_advance_payment_voucher_details;";
                                }
                                else
                                {
                                    if (ChkBoxTables.CheckedItems.Count == 0)
                                    {
                                        MessageBox.Show("Please select at least one table to clear.");
                                        return;
                                    }
                                    else
                                    {
                                        foreach (string item in ChkBoxTables.CheckedItems)
                                        {
                                            if (item == "Attendance Sheet")
                                            {
                                                query += "TRUNCATE TABLE tbl_attendancesheet;";
                                            }
                                            else if (item == "Attendance Salary")
                                            {
                                                query += "TRUNCATE TABLE tbl_attendance_salary;";
                                            }
                                            else if (item == "Bank Card")
                                            {
                                                query += "TRUNCATE TABLE tbl_bank_card;";
                                            }
                                            else if (item == "Bank Register")
                                            {
                                                query += "TRUNCATE TABLE tbl_bank_register;";
                                            }
                                            else if (item == "Bank")
                                            {
                                                query += "TRUNCATE TABLE tbl_bank;";
                                            }
                                            else if (item == "Cheque Details")
                                            {
                                                query += "TRUNCATE TABLE tbl_check_details;";
                                            }
                                            else if (item == "Cheque")
                                            {
                                                query += "TRUNCATE TABLE tbl_cheque;";
                                            }
                                            else if (item == "Corporate Tax Configuration")
                                            {
                                                query += "TRUNCATE TABLE tbl_corporate_tax_configration;";
                                            }
                                            else if (item == "Cost Center")
                                            {
                                                query += "TRUNCATE TABLE tbl_cost_center;";
                                            }
                                            else if (item == "Credit Note")
                                            {
                                                query += "TRUNCATE TABLE tbl_credit_note; TRUNCATE TABLE tbl_credit_note_details;";
                                                query += "Delete from tbl_transaction where type = 'Credit Note';";
                                            }
                                            else if (item == "Customer")
                                            {
                                                query += "TRUNCATE TABLE tbl_customer;";
                                                query += "Delete from tbl_transaction where type IN ('Sales Invoice','Customer Opening Balance','Customer Receipt','Customer Advance Payment');";
                                            }
                                            else if (item == "Customer Category")
                                            {
                                                query += "TRUNCATE TABLE tbl_customer_category;";
                                            }
                                            else if (item == "Damage")
                                            {
                                                query += "TRUNCATE TABLE tbl_damage; TRUNCATE TABLE tbl_damage_details;";
                                                query += "Delete from tbl_transaction where type = 'Damage Invoice';";
                                            }
                                            else if (item == "Debit Note")
                                            {
                                                query += "TRUNCATE TABLE tbl_debit_note; TRUNCATE TABLE tbl_debit_note_details;";
                                                query += "Delete from tbl_transaction where type = 'Debit Note';";
                                            }
                                            else if (item == "Departments")
                                            {
                                                query += "TRUNCATE TABLE tbl_departments;";
                                            }
                                            else if (item == "Employee")
                                            {
                                                query += "TRUNCATE TABLE tbl_employee;";
                                                query += "Delete from tbl_transaction where type IN ('Employee Opening Balance','Employee Advance Payment','Employee Salary Payment');";
                                            }
                                            else if (item == "End Of Service")
                                            {
                                                query += "TRUNCATE TABLE tbl_end_of_service;";
                                                query += "Delete from tbl_transaction where type = 'End Of Service';";
                                            }
                                            else if (item == "Final Settlement")
                                            {
                                                query += "TRUNCATE TABLE tbl_final_settlement;";
                                                query += "Delete from tbl_transaction where type = 'Final Settlement';";
                                            }
                                            else if (item == "Fixed Assets")
                                            {
                                                query += "TRUNCATE TABLE tbl_fixed_assets;";
                                                query += "DELETE FROM tbl_transaction WHERE type = 'Fixed Assets';";
                                            }
                                            else if (item == "Fixed Assets Category")
                                            {
                                                query += "TRUNCATE TABLE tbl_fixed_assets_category;";
                                            }
                                            //else if (item == "General Config")
                                            //{
                                            //    query += "TRUNCATE TABLE tbl_general_settings;";
                                            //}
                                            else if (item == "Items")
                                            {
                                                query += "TRUNCATE TABLE tbl_items;";
                                                query += "DELETE FROM tbl_transaction WHERE type = 'Opening Qty';";
                                                query += "TRUNCATE TABLE tbl_item_transaction;";
                                            }
                                            else if (item == "Items Unit")
                                            {
                                                query += "TRUNCATE TABLE tbl_items_unit;";
                                            }
                                            else if (item == "Items Warehouse")
                                            {
                                                query += "TRUNCATE TABLE tbl_items_warehouse;";
                                                query += "TRUNCATE TABLE tbl_item_warehouse_transaction;";
                                            }
                                            else if (item == "Item Assembly")
                                            {
                                                query += "TRUNCATE TABLE tbl_item_assembly;";
                                            }
                                            else if (item == "Item Category")
                                            {
                                                query += "TRUNCATE TABLE tbl_item_category;";
                                            }
                                            else if (item == "Item Stock Settlement")
                                            {
                                                query += "TRUNCATE TABLE tbl_item_stock_settlement;TRUNCATE TABLE tbl_item_stock_settlement_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type = 'Stock Settlement';";
                                            }
                                            else if (item == "journal Voucher")
                                            {
                                                query += "TRUNCATE TABLE tbl_journal_voucher; TRUNCATE TABLE tbl_journal_voucher_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type = 'JOURNAL VOUCHER';";
                                            }
                                            else if (item == "Leave Salary")
                                            {
                                                query += "TRUNCATE TABLE tbl_leave_salary;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Leave Salary');";
                                            }
                                            else if (item == "Loan")
                                            {
                                                query += "TRUNCATE TABLE tbl_loan;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Loan Request');";
                                            }
                                            else if (item == "Payment")
                                            {
                                                query += "UPDATE tbl_purchase SET pay=0 , `CHANGE`= net WHERE id IN (select inv_id from tbl_payment_voucher_details);";
                                                query += "TRUNCATE TABLE tbl_payment_voucher; TRUNCATE TABLE tbl_payment_voucher_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Payment Voucher','Vendor Payment','Vendor Opening Balance','Vendor Advance Payment','Employee Salary Payment','Employee Petty Cash Payment','Subcontractor Payment','Subcontractor Opening Balance');";
                                            }
                                            else if (item == "Petty Cash Card")
                                            {
                                                query += "TRUNCATE TABLE tbl_petty_cash_card;";
                                            }
                                            else if (item == "Petty Cash")
                                            {
                                                query += "TRUNCATE TABLE tbl_petty_cash;";
                                                query += "TRUNCATE TABLE tbl_petty_cash_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Petty Cash');";
                                            }
                                            else if (item == "petty Cash Category")
                                            {
                                                query += "TRUNCATE TABLE tbl_petty_cash_category;";
                                            }
                                            else if (item == "petty Cash Request")
                                            {
                                                query += "TRUNCATE TABLE tbl_petty_cash_request;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Petty Cash Request');";
                                            }
                                            else if (item == "petty Cash Submission")
                                            {
                                                query += "TRUNCATE TABLE tbl_petty_cash_submition; TRUNCATE TABLE tbl_petty_cash_submition_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Petty Cash Submission',' Petty Cash Submission');";
                                                query += "DELETE FROM tbl_cost_center_transaction WHERE type = 'Petty Cash';";
                                            }
                                            else if (item == "Position")
                                            {
                                                query += "TRUNCATE TABLE tbl_position;";
                                            }
                                            else if (item == "Prepaid Expense")
                                            {
                                                query += "TRUNCATE TABLE tbl_prepaid_expense; TRUNCATE TABLE tbl_prepaid_expense_category;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Prepaid Expense');";
                                            }
                                            else if (item == "Purchase")
                                            {
                                                query += "TRUNCATE TABLE tbl_purchase; TRUNCATE TABLE tbl_purchase_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Purchase Invoice','Purchase Invoice Cash');";
                                                query += "DELETE FROM tbl_cost_center_transaction WHERE type = 'Purchase';";
                                            }
                                            else if (item == "Purchase Order")
                                            {
                                                query += "TRUNCATE TABLE tbl_purchase_order; TRUNCATE TABLE tbl_purchase_order_details;";
                                            }
                                            else if (item == "Purchase Return")
                                            {
                                                query += "TRUNCATE TABLE tbl_purchase_return; TRUNCATE TABLE tbl_purchase_return_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Purchase Return Invoice','Purchase Return Invoice Cash');";
                                                query += "DELETE FROM tbl_cost_center_transaction WHERE type = 'Purchase Return';";
                                            }
                                            else if (item == "Receipt Voucher")
                                            {
                                                query += "UPDATE tbl_sales SET pay=0 , `CHANGE`= net WHERE id IN (select inv_id from tbl_receipt_voucher_details);";
                                                query += "TRUNCATE TABLE tbl_receipt_voucher; TRUNCATE TABLE tbl_receipt_voucher_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Customer Receipt','PDC Receivable');";
                                            }
                                            else if (item == "Salary")
                                            {
                                                query += "TRUNCATE TABLE tbl_salary;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Employee Salary');";
                                            }
                                            else if (item == "Sales")
                                            {
                                                query += "TRUNCATE TABLE tbl_sales; TRUNCATE TABLE tbl_sales_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Sales Invoice','Sales Invoice Cash');";
                                                query += "DELETE FROM tbl_cost_center_transaction WHERE type = 'Sales';";
                                            }
                                            else if (item == "Sales Order")
                                            {
                                                query += "TRUNCATE TABLE tbl_sales_order; TRUNCATE TABLE tbl_sales_order_details;";
                                            }
                                            else if (item == "Sales Quotation")
                                            {
                                                query += "TRUNCATE TABLE tbl_sales_quotation; TRUNCATE TABLE tbl_sales_quotation_details;";
                                            }
                                            else if (item == "Sales Performa")
                                            {
                                                query += "TRUNCATE TABLE tbl_sales_proforma; TRUNCATE TABLE tbl_sales_proforma_details;";
                                            }
                                            else if (item == "Sales Return")
                                            {
                                                query += "TRUNCATE TABLE tbl_sales_return; TRUNCATE TABLE tbl_sales_return_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('SalesReturn Invoice','SalesReturn Invoice Cash');";
                                                query += "DELETE FROM tbl_cost_center_transaction WHERE type = 'Sales Return';";
                                            }
                                            else if (item == "Sub Cost Center")
                                            {
                                                query += "TRUNCATE TABLE tbl_sub_cost_center;TRUNCATE TABLE tbl_cost_center_transaction;";
                                            }
                                            else if (item == "Unit")
                                            {
                                                query += "TRUNCATE TABLE tbl_unit;";
                                            }
                                            else if (item == "Vat")
                                            {
                                                query += "TRUNCATE TABLE tbl_vat;";
                                            }
                                            else if (item == "Vendor")
                                            {
                                                query += "TRUNCATE TABLE tbl_vendor;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Vendor Opening Balance','Vendor Advance Payment','Vendor Payment','Subcontractor Payment','Subcontractor Opening Balance');";
                                            }
                                            else if (item == "Vendor Category")
                                            {
                                                query += "TRUNCATE TABLE tbl_vendor_category;";
                                            }
                                            else if (item == "Warehouse")
                                            {
                                                query += "TRUNCATE TABLE tbl_warehouse;";
                                            }
                                            else if (item == "Salary Adjustments")
                                            {
                                                query += "TRUNCATE TABLE tbl_salary_adjustments;";
                                            }
                                            else if (item == "Advance Payment Voucher")
                                            {
                                                query += "TRUNCATE TABLE tbl_advance_payment_voucher; TRUNCATE TABLE tbl_advance_payment_voucher_details;";
                                                query += "DELETE FROM tbl_transaction WHERE type IN ('Customer Advance Payment','Vendor Advance Payment');";
                                            }
                                        }
                                    }
                                }
                                //DBClass.ExecuteNonQuery(query);
                                using (var conn = DBClass.GetConnection())
                                {
                                    conn.Open();
                                    using (var trans = conn.BeginTransaction())
                                    {
                                        try
                                        {
                                            foreach (string stmt in query.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                                            {
                                                DBClass.ExecuteNonQueryCommit(stmt + ";", trans);
                                            }

                                            trans.Commit();
                                            MessageBox.Show("Data Cleared");
                                        }
                                        catch (Exception ex)
                                        {
                                            trans.Rollback();
                                            MessageBox.Show("Transaction failed: " + ex.Message);
                                        }
                                    }
                                }

                                if (ChkLevel4.Checked)
                                {
                                    DBClass.ExecuteNonQuery("TRUNCATE TABLE tbl_coa_level_4;");
                                    DBClass.ExecuteNonQuery("Delete from tbl_transaction where type ='General Ledger Opening Balance';");
                                }
                                else if (ChkLevel4OBOnly.Checked)
                                {
                                    DBClass.ExecuteNonQuery("Update tbl_coa_level_4 SET debit=0,credit=0 where id > 0;");
                                    DBClass.ExecuteNonQuery("Delete from tbl_transaction where type ='General Ledger Opening Balance';");
                                }
                                if (ChkLevel3.Checked)
                                {
                                    DBClass.ExecuteNonQuery("TRUNCATE TABLE tbl_coa_level_3;");
                                }
                                if (ChkLevel2.Checked)
                                {
                                    DBClass.ExecuteNonQuery("TRUNCATE TABLE tbl_coa_level_2;");
                                }
                                if (ChkLevel1.Checked)
                                {
                                    DBClass.ExecuteNonQuery("TRUNCATE TABLE tbl_coa_level_1;");
                                }

                                MessageBox.Show("Data Cleared");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error: " + ex.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect password. Deletion canceled.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // User cancelled the password prompt
                    MessageBox.Show("Deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ChkAll_CheckedChanged(object sender, EventArgs e)
        {
            //if checked , check all items in the checked list box
            if (ChkAll.Checked)
            {
                for (int i = 0; i < ChkBoxTables.Items.Count; i++)
                {
                    ChkBoxTables.SetItemChecked(i, true);
                }
                ChkLevel4OBOnly.Checked = true;
            }
            else
            {
                for (int i = 0; i < ChkBoxTables.Items.Count; i++)
                {
                    ChkBoxTables.SetItemChecked(i, false);
                }
                ChkLevel4OBOnly.Checked = false;
            }
        }
    }
}
