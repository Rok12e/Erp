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
    public partial class MasterCheque : Form
    {
        private EventHandler checkUpdatedHandler;

        public MasterCheque()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            checkUpdatedHandler  = (sender, args) => BinChq();
            EventHub.Check += checkUpdatedHandler;
            headerUC1.FormText = this.Text;
        }

        private void MasterCheque_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Check -= checkUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmViewCheque().ShowDialog();
        }

        private void MasterCheque_Load(object sender, EventArgs e)
        {
            headerUC1.FormText = "Cheque Center";
            BinChq();
        }

        public void BinChq()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY tbl_cheque.id) AS `SN`,tbl_cheque.id,concat(tbl_bank.code ,'-', tbl_bank.name) AS 'Bank Name',
                                                    tbl_bank_card.account_name AS 'AC Name',tbl_bank_card.account_type AS 'AC Type',tbl_cheque.chq_book_no AS 'Check Book NO',
                                                    tbl_cheque.chq_book_qty AS 'Check Book QTY',tbl_cheque.leaves_start_from AS 'Start From',tbl_cheque.leaves_end_in AS 'End In'            
                                                    FROM tbl_cheque INNER JOIN tbl_bank_card ON tbl_cheque.bank_card_id = tbl_bank_card.id
                                                    INNER JOIN tbl_bank ON tbl_bank_card.bank_id = tbl_bank.id");

            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["SN"].Width = 50;
            dgvCustomer.Columns["Bank Name"].MinimumWidth = 180;
            dgvCustomer.Columns["Bank Name"].AutoSizeMode = dgvCustomer.Columns["AC Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["AC Type"].MinimumWidth = 120;
            dgvCustomer.Columns["id"].Visible = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BinChq();
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            new frmViewCheque(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())).ShowDialog();
            
        }
    }
}
