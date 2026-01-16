using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.RMS.Class;
using YamyProject.RMS.View;

namespace YamyProject.RMS.Model
{
    public partial class frmRMSCheckout : frmRMSAddSample
    {
        public frmRMSCheckout()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        public double amt;
        public int Mainid =0;
        private void txtRecevied_TextChanged(object sender, EventArgs e)
        {
            double amt = 0;
            double recipt = 0;
            double change = 0;

            double.TryParse(txtBillAmount.Text, out amt);
            double.TryParse(txtRecevied.Text, out recipt);

            change = Math.Abs (amt - recipt);
            txtChange.Text = change.ToString();
        }
             public virtual void btnclose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public override void btnSave_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtRecevied.Text))
            {
                // Show a message box if the TextBox is empty
                MessageBox.Show("Please Insert Payment Bill");
                // Optionally, you can stop the event if needed, but normally the event will end after the message.
                return;
            }

            string qry = @"Update tbl_rmsmain SET Total = @total,received = @rec , changetot = @chang ,
                           Status ='Paid' where MainId=@id";

            Hashtable ht = new Hashtable();
            ht.Add("@id", Mainid);
            ht.Add("@total", txtBillAmount.Text);
            ht.Add("@rec", txtRecevied.Text);
            ht.Add("@chang", txtChange.Text);
            decimal TotalVat = 0, TotalBefore = 0, dicountAmount = 0, receviedAmount = decimal.Parse(txtRecevied.Text), totalBillAmount = decimal.Parse(txtBillAmount.Text);
            string invCode = "RMS-" + Mainid;
            if (!string.IsNullOrEmpty(txtChange.Text) && decimal.Parse(txtChange.Text) > 0)
            {
                dicountAmount = decimal.Parse(txtChange.Text);
            }

            if (checkBoxTax.Checked)
            {
                TotalVat = receviedAmount / (5 * 100);
            }
            else
            {
                TotalVat = 0;
            }

            TotalBefore = receviedAmount - TotalVat;

            if (RMSClass.SQl(qry,ht) >0 )
            {
                string _accountId = cmbAccountCashName.SelectedValue.ToString();
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                       _accountId,
                       totalBillAmount.ToString(), "0", Mainid.ToString(), _accountId, "RMS Bill", "RMS BILL", "RMS Bill NO. " + Mainid,
                       frmLogin.userId, DateTime.Now.Date, invCode);

                if (dicountAmount > 0)
                {
                    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                        level4SalesDiscount.ToString(), "0" , dicountAmount.ToString(), Mainid.ToString(), "0", "RMS Bill", "RMS BILL",
                        "Discount For Invoice No. " + invCode, frmLogin.userId, DateTime.Now.Date, invCode);
                }
                if (TotalVat > 0)
                {
                    CommonInsert.addTransactionEntry(dtInv.Value.Date,
                        level4VatId.ToString(), "0", TotalVat.ToString(), Mainid.ToString(), "0", "RMS Bill", "RMS BILL",
                        "Vat Output For Invoice No. " + invCode, frmLogin.userId, DateTime.Now.Date, invCode);
                }
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                        level4SalesInvoice.ToString(), "0", TotalBefore.ToString(), Mainid.ToString(), "0", "RMS Bill", "RMS BILL",
                        "Sales Revenue For Invoice No. " + invCode, frmLogin.userId, DateTime.Now.Date, invCode);

                Utilities.LogAudit(frmLogin.userId, "RMS Bill Payment", "RMS Bill", Mainid, "Paid RMS Bill: " + Mainid + " Amount: " + txtRecevied.Text);
                MessageBox.Show("Saved Successfully");
                this.Close();
            }
        }
        int level4PaymentCreditMethodId, level4VatId, level4SalesInvoice, level4COGS, level4Inventory,level4SalesDiscount;

        private void radioButtonCard_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCard.Checked)
            {
                radioButtonCash.Checked = false;
                loadAccounts();
            }
        }

        private void radioButtonCash_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonCash.Checked)
            {
                radioButtonCard.Checked = false;
                loadAccounts();
            }
        }
        private void loadAccounts()
        {
            string query = "SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)";
            string catName = "Default Account For Cash";
            if (radioButtonCard.Checked)
            {
                query = "SELECT id,code FROM tbl_coa_level_4 WHERE id =@cat";
                //, DBClass.CreateParameter("@cat", "PDC Payable")
                catName = "1";
            }
            else
            {
                cmbAccountCashName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Invoice Payment Cash Method");
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@cat", catName)))
                if (reader.Read())
                {
                    //txtCreditAccountCode.Text = reader["code"].ToString();
                    string accountId = reader["id"].ToString();
                    if (!string.IsNullOrEmpty(accountId))
                    {
                        cmbAccountCashName.SelectedValue = int.Parse(accountId);
                    }
                }
        }

        private void frmRMSCheckout_Load(object sender, EventArgs e)
        {
            txtBillAmount.Text = amt.ToString();
            dtInv.Value = DateTime.Now.Date;
            BindCombos.PopulateAllLevel4Account(cmbAccountCashName);
            cmbAccountCashName.SelectedValue = BindCombos.SelectDefaultLevelAccount("Invoice Payment Cash Method");

            level4PaymentCreditMethodId = frmLogin.defaultAccounts.ContainsKey("Customer") ? frmLogin.defaultAccounts["Customer"] : 0;
            level4VatId = frmLogin.defaultAccounts.ContainsKey("Vat Output") ? frmLogin.defaultAccounts["Vat Output"] : 0;
            level4SalesInvoice = frmLogin.defaultAccounts.ContainsKey("Sales") ? frmLogin.defaultAccounts["Sales"] : 0;
            level4COGS = frmLogin.defaultAccounts.ContainsKey("COGS") ? frmLogin.defaultAccounts["COGS"] : 0;
            level4Inventory = frmLogin.defaultAccounts.ContainsKey("Inventory") ? frmLogin.defaultAccounts["Inventory"] : 0;

            object ref_no_result = DBClass.ExecuteScalar(@"SELECT IFNULL(
                                                            (SELECT id FROM tbl_coa_level_4 WHERE name LIKE '%Discount%' LIMIT 1),
                                                            (SELECT id FROM tbl_coa_level_4 WHERE main_id = 25 LIMIT 1)
                                                        ) AS result;");

            if (ref_no_result != null && ref_no_result != DBNull.Value && Convert.ToInt32(ref_no_result) > 0)
            {
                level4SalesDiscount = Convert.ToInt32(ref_no_result);
            }
        }

        private void txtRecevied_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar)) return;

            // Allow digits and decimal point
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // Prevent more than one decimal point
            if (e.KeyChar == '.' && txtRecevied.Text.Contains("."))
            {
                e.Handled = true;
            }
        }
    }
}
