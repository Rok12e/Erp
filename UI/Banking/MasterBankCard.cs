using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterBankCard : Form
    {
        private EventHandler bankCardUpdatedHandler;

        public MasterBankCard()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            bankCardUpdatedHandler = (sender, args) => BindCards();
            EventHub.BankCard += bankCardUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterBankCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.BankCard -= bankCardUpdatedHandler;

        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmViewBankCard().ShowDialog();
        }
        private void MasterBankCard_Load(object sender, EventArgs e)
        {
            BindCards();
        }
        public void BindCards()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"
                                    SELECT 
                                        ROW_NUMBER() OVER (ORDER BY tbl_bank_card.id) AS `SN`,
                                        concat(tbl_bank.code ,'-', tbl_bank.name) AS 'Bank Name',
                                        tbl_bank_card.id,
                                        tbl_bank_card.account_name AS 'AC Name',
                                        tbl_bank_card.account_type AS 'AC Type',
                                        tbl_bank_card.account_no AS 'AC No',
                                        tbl_bank_card.swift AS 'Swift',
                                        tbl_bank_card.iban_no AS 'IBAN No',
                                        tbl_bank_card.branch_name AS 'Branch Name'
                                    FROM tbl_bank_card 
                                    INNER JOIN tbl_bank ON tbl_bank_card.bank_id = tbl_bank.id
                                    WHERE tbl_bank_card.state = 0");

            dgvBankCard.DataSource = dt;
            dgvBankCard.Columns["id"].Visible = false;
            dgvBankCard.Columns["SN"].Width = 50;
            dgvBankCard.Columns["Bank Name"].MinimumWidth = 180;
            dgvBankCard.Columns["Bank Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvBankCard.Columns["AC Name"].MinimumWidth = 150;
            dgvBankCard.Columns["AC Type"].MinimumWidth = 120;
            dgvBankCard.Columns["AC No"].MinimumWidth = 130;
            dgvBankCard.Columns["Swift"].MinimumWidth = 150;
            dgvBankCard.Columns["IBAN No"].MinimumWidth = 180;
            dgvBankCard.Columns["Branch Name"].MinimumWidth = 150;

            if (dgvBankCard.Rows.Count > 0)
            {
                btnEdit.Enabled = UserPermissions.canEdit("Open Bank Card");
            }

            LocalizationManager.LocalizeDataGridViewHeaders(dgvBankCard);
        }


        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCards();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBankCard.Rows.Count == 0)
                return;
            new frmViewBankCard(int.Parse(dgvBankCard.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBankCard.Rows.Count == 0)
                return;
            new frmViewBankCard(int.Parse(dgvBankCard.CurrentRow.Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBankCard.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_bank_card SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvBankCard.SelectedRows[0].Cells["id"].Value.ToString()));

            Utilities.LogAudit(frmLogin.userId, "Delete Bank Card", "Bank Card", int.Parse(dgvBankCard.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Bank Card: " + dgvBankCard.SelectedRows[0].Cells["AC Name"].Value.ToString());
            BindCards();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));
        }
    }
}
