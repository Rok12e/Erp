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
    public partial class MasterPDC : Form
    {
        private EventHandler checkUpdatedHandler;
        public MasterPDC()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            checkUpdatedHandler = (sender, args) => BinChqPayable();
            EventHub.Check += checkUpdatedHandler;
            headerUC1.FormText = this.Text;

            DateTime dated = DateTime.Now;
            dtFrom.Value = dtTo.Value = dated;
        }

        private void MasterCheque_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Check -= checkUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmViewCheque().ShowDialog();
        }
        int pdcPayable = 0, pdcPayableReturn = 0, pdcPayableHold = 0;
        int pdcReceivable = 0, pdcReceivableReturn = 0, pdcReceivableHold = 0;
        private void MasterPDC_Load(object sender, EventArgs e)
        {
            var defaultAccounts = BindCombos.LoadDefaultAccounts();
            pdcPayable = defaultAccounts.ContainsKey("PDC Payable") ? defaultAccounts["PDC Payable"] : 0;
            pdcPayableReturn = defaultAccounts.ContainsKey("PDC Payable Return") ? defaultAccounts["PDC Payable Return"] : 0;
            //pdcPayableCancel = defaultAccounts.ContainsKey("PDC Payable Cancel") ? defaultAccounts["PDC Payable Cancel"] : 0;
            pdcPayableHold = defaultAccounts.ContainsKey("PDC Payable Hold") ? defaultAccounts["PDC Payable Hold"] : 0;

            pdcReceivable = defaultAccounts.ContainsKey("PDC Receivable") ? defaultAccounts["PDC Receivable"] : 0;
            pdcReceivableReturn = defaultAccounts.ContainsKey("PDC Receivable Return") ? defaultAccounts["PDC Receivable Return"] : 0;
            //pdcReceivableCancel = defaultAccounts.ContainsKey("PDC Receivable Cancel") ? defaultAccounts["PDC Receivable Cancel"] : 0;
            pdcReceivableHold = defaultAccounts.ContainsKey("PDC Receivable Hold") ? defaultAccounts["PDC Receivable Hold"] : 0;

            BinChqPayable();
        }
        private void BinChqReceivable()
        {
            dgvCustomer.DataSource = null;
            dgvCustomer.Columns.Clear();
            dgvCustomer.Refresh();
            dgvCustomer.DataSource = chkDate.Checked ? DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY cd.date) AS `SN`, 
               pv.date AS 'TRS Date',
               cd.check_date AS 'Check Date',cd.id, pass_date,return_date,hold_date,cancel_date,
               pv.code AS 'TRS REF',
               cd.check_no AS 'Check No',
               cd.check_name AS 'Check Name', 
               '' AS 'Bank Name',
               pv.id AS 'REFID',
               cast( cd.Amount as decimal(18,3)) AS Amount,cd.State
               FROM tbl_check_details cd
                    INNER JOIN tbl_receipt_voucher pv ON cd.pvc_no = pv.id
                    -- INNER JOIN tbl_bank b ON pv.bank_id = b.id
                    WHERE cd.check_type = @type
 
               ",
                                                                DBClass.CreateParameter("type", radPayable.Checked ? "Payment" : "Receipt"))
       : DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY cd.date) AS `SN`, 
                               pv.date AS 'TRS Date',
                               cd.check_date AS 'Check Date',cd.id, pass_date,return_date,hold_date,cancel_date,
                               pv.code AS 'TRS REF',
                               cd.check_no AS 'Check No',
                               cd.check_name AS 'Check Name', 
                               '' AS 'Bank Name',
               pv.id AS 'REFID',
                               cast( cd.Amount as decimal(18,3)) AS Amount,cd.State
                               FROM tbl_check_details cd
                        INNER JOIN tbl_receipt_voucher pv ON cd.pvc_no = pv.id
                        -- INNER JOIN tbl_bank b ON pv.bank_id = b.id
                        WHERE cd.check_type = @type 

                          AND cd.date BETWEEN @startDate AND @endDate",
                                                DBClass.CreateParameter("startDate", dtFrom.Value.Date), DBClass.CreateParameter("endDate", dtTo.Value.Date),
                                                          DBClass.CreateParameter("type", radPayable.Checked ? "Payment" : "Receipt"));

            dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["pass_date"].Visible = dgvCustomer.Columns["return_date"].Visible = dgvCustomer.Columns["hold_date"].Visible = dgvCustomer.Columns["cancel_date"].Visible = false;
            dgvCustomer.Columns["SN"].Width = 35;
            dgvCustomer.Columns["Cheque No"].Width = 90;
            dgvCustomer.Columns["state"].Width = 50;
            dgvCustomer.Columns["TRS REF"].Width = 75;
            dgvCustomer.Columns["TRS Date"].Width = 80;
            dgvCustomer.Columns["Cheque Date"].Width = 90;
            dgvCustomer.Columns["Bank Name"].MinimumWidth = 150;
            dgvCustomer.Columns["Bank Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["Cheque Name"].MinimumWidth = 130;
            DataGridViewButtonColumn btnPass = new DataGridViewButtonColumn();
            btnPass.Name = "Pass";
            DataGridViewButtonColumn btnReturn = new DataGridViewButtonColumn();
            btnReturn.Name = "Return";
            DataGridViewButtonColumn btnHold = new DataGridViewButtonColumn();
            btnHold.Name = "Hold";
            DataGridViewButtonColumn btnCancel = new DataGridViewButtonColumn();
            btnCancel.Name = "Cancel";
            btnCancel.HeaderText = btnPass.HeaderText = btnHold.HeaderText = btnReturn.HeaderText = "";
            dgvCustomer.Columns.Add(btnPass);
            dgvCustomer.Columns.Add(btnReturn);
            dgvCustomer.Columns.Add(btnHold);
            dgvCustomer.Columns.Add(btnCancel);
            dgvCustomer.CellPainting += dgvCustomer_CellPainting;
            dgvCustomer.CellMouseEnter += dgvCustomer_CellMouseEnter;
            dgvCustomer.CellMouseLeave += dgvCustomer_CellMouseLeave;
            dgvCustomer.CellFormatting += dgvCustomer_CellFormatting;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        public void BinChqPayable()
        {
            dgvCustomer.DataSource = null;
            dgvCustomer.Columns.Clear();
            dgvCustomer.Refresh();
            dgvCustomer.DataSource = chkDate.Checked ? DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY cd.date) AS `SN`, 
                                               pv.date AS 'TRS Date',
                                               cd.check_date AS 'Cheque Date',cd.id, pass_date,return_date,hold_date,cancel_date,
                                               pv.code AS 'TRS REF',
                                               cd.check_no AS 'Cheque No',
                                               cd.check_name AS 'Cheque Name', 
                                               b.name AS 'Bank Name',
                                               cast( cd.Amount as decimal(18,3)) AS Amount,cd.State
                                               FROM tbl_check_details cd
                                            INNER JOIN tbl_payment_voucher pv ON cd.pvc_no = pv.id
                                            INNER JOIN tbl_cheque chq ON cd.check_id = chq.id
                                            INNER JOIN tbl_bank_card bc ON chq.bank_card_id = bc.id
                                            INNER JOIN tbl_bank b ON bc.bank_id = b.id
                                            WHERE cd.check_type = @type ",
         DBClass.CreateParameter("type", radPayable.Checked ? "Payment" : "Receipt"))
       : DBClass.ExecuteDataTable(@"SELECT ROW_NUMBER() OVER (ORDER BY cd.date) AS `SN`, 
                                                   pv.date AS 'TRS Date',
                                                   cd.check_date AS 'Cheque Date',cd.id,  pass_date,return_date,hold_date,cancel_date,
                                                   pv.code AS 'TRS REF',
                                                   cd.check_no AS 'Cheque No',
                                                   cd.check_name AS 'Cheque Name', 
                                                   b.name AS 'Bank Name',
                                                  cast( cd.Amount as decimal(18,3)) AS Amount,cd.State
                                                    FROM tbl_check_details cd
                                                    INNER JOIN tbl_payment_voucher pv ON cd.pvc_no = pv.id
                                                    INNER JOIN tbl_cheque chq ON cd.check_id = chq.id
                                                    INNER JOIN tbl_bank_card bc ON chq.bank_card_id = bc.id
                                                    INNER JOIN tbl_bank b ON bc.bank_id = b.id
                                                    WHERE cd.check_type = @type 

                                              AND cd.date BETWEEN @startDate AND @endDate",
                                                DBClass.CreateParameter("startDate", dtFrom.Value.Date), DBClass.CreateParameter("endDate", dtTo.Value.Date),
                                                          DBClass.CreateParameter("type", radPayable.Checked ? "Payment" : "Receipt"));

            dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["pass_date"].Visible = dgvCustomer.Columns["return_date"].Visible = dgvCustomer.Columns["hold_date"].Visible = dgvCustomer.Columns["cancel_date"].Visible = false;
            dgvCustomer.Columns["SN"].Width = 35;
            dgvCustomer.Columns["Cheque No"].Width = 90;
            dgvCustomer.Columns["state"].Width = 50;
            dgvCustomer.Columns["TRS REF"].Width = 75;
            dgvCustomer.Columns["TRS Date"].Width = 80;
            dgvCustomer.Columns["Cheque Date"].Width = 90;
            dgvCustomer.Columns["Bank Name"].MinimumWidth = 150;
            dgvCustomer.Columns["Bank Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["Cheque Name"].MinimumWidth = 130;
            DataGridViewButtonColumn btnPass = new DataGridViewButtonColumn();
            btnPass.Name = "Pass";
            DataGridViewButtonColumn btnReturn = new DataGridViewButtonColumn();
            btnReturn.Name = "Return";
            DataGridViewButtonColumn btnHold = new DataGridViewButtonColumn();
            btnHold.Name = "Hold";
            DataGridViewButtonColumn btnCancel = new DataGridViewButtonColumn();
            btnCancel.Name = "Cancel";
            btnCancel.HeaderText = btnPass.HeaderText = btnHold.HeaderText = btnReturn.HeaderText = "";
            dgvCustomer.Columns.Add(btnPass);
            dgvCustomer.Columns.Add(btnReturn);
            dgvCustomer.Columns.Add(btnHold);
            dgvCustomer.Columns.Add(btnCancel);
            dgvCustomer.CellPainting += dgvCustomer_CellPainting;
            dgvCustomer.CellMouseEnter += dgvCustomer_CellMouseEnter;
            dgvCustomer.CellMouseLeave += dgvCustomer_CellMouseLeave;
            dgvCustomer.CellFormatting += dgvCustomer_CellFormatting;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);

        }
        private void dgvCustomer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var stateValue = dgvCustomer.Rows[e.RowIndex].Cells["State"].Value?.ToString();

                if (stateValue == "Pass")
                {
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.BackColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionBackColor = Color.FromArgb(84, 130, 53);
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.ForeColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionForeColor = Color.White;
                }
                else if (stateValue == "Return")
                {
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.BackColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionBackColor = Color.Red;
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.ForeColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionForeColor = Color.White; // Optional: change text color to white for better contrast
                }
                else if (stateValue == "Hold")
                {
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.BackColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionBackColor = Color.FromArgb(0, 112, 192);
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.ForeColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionForeColor = Color.White;
                }
                else if (stateValue == "Cancel")
                {
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.BackColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionBackColor = Color.Black;
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.ForeColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionForeColor = Color.White;
                }
                else
                {
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.ForeColor = dgvCustomer.Rows[e.RowIndex].Cells["state"].Style.SelectionForeColor = Color.Black;
                }
            }
        }

        private void dgvCustomer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvCustomer.Rows[e.RowIndex].Cells["Cheque Name"].Value != null &&
                dgvCustomer.Rows[e.RowIndex].Cells["Cheque Name"].Value.ToString() == "TOTAL")
                return;
             if (e.ColumnIndex >= 0 && dgvCustomer.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                e.PaintBackground(e.CellBounds, true);
                Brush brush;

                if (dgvCustomer.Columns[e.ColumnIndex].Name == "Pass")
                    brush = new SolidBrush(Color.FromArgb(84, 130, 53));
                else if (dgvCustomer.Columns[e.ColumnIndex].Name == "Return")
                    brush = new SolidBrush(Color.Red);
                else if (dgvCustomer.Columns[e.ColumnIndex].Name == "Hold")
                    brush = new SolidBrush(Color.FromArgb(0, 112, 192));
                else
                    brush = new SolidBrush(Color.Black);
                e.Graphics.FillRectangle(brush,new RectangleF(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width-1, e.CellBounds.Height-1));
                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    SizeF textSize = e.Graphics.MeasureString(dgvCustomer.Columns[e.ColumnIndex].Name, new Font(FontFamily.GenericSansSerif, 10));
                    PointF textLocation = new PointF(
                        e.CellBounds.Left + (e.CellBounds.Width - textSize.Width) / 2,
                        e.CellBounds.Top + (e.CellBounds.Height - textSize.Height) / 2
                    );
                    e.Graphics.DrawString(dgvCustomer.Columns[e.ColumnIndex].Name, new Font(FontFamily.GenericSansSerif, 10), textBrush, textLocation);
                }
                e.Handled = true;
            }
        }

        private void dgvCustomer_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && dgvCustomer.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                dgvCustomer.Cursor = Cursors.Hand;
        }

        private void dgvCustomer_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvCustomer.Cursor = Cursors.Default; 
        }

        private void redPayable_CheckedChanged(object sender, EventArgs e)
        {
            if (radPayable.Checked)
                BinChqPayable();
            else BinChqReceivable();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            if (radPayable.Checked)
                BinChqPayable();
            else BinChqReceivable();
        }
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows[e.RowIndex].Cells["Cheque Name"].Value.ToString() == "TOTAL")
                return;

            string state = dgvCustomer.CurrentRow.Cells["state"].Value.ToString();
            string action = dgvCustomer.Columns[e.ColumnIndex].Name;
            string REFID = radPayable.Checked ? "0" : dgvCustomer.CurrentRow.Cells["REFID"].Value.ToString();

            
            if (action == "Pass")
            {
                //if (state == "Return")
                //{
                //    MessageBox.Show("Can't Pass Return Cheque");
                //    return;
                //}
                if (state == "Cancel")
                {
                    MessageBox.Show("Can't Pass Cancel Cheque");
                    return;
                }
            }
            else if (action == "Return")
            {
                if (state != "Pass")
                {
                    MessageBox.Show("Can't Return Cheque Before Passing It First");
                    return;
                }
            }
            else if (action == "Hold")
            {
                if (state != "New")
                {
                    MessageBox.Show("Only New Cheque Can Be Hold");
                    return;
                }
            }
            else if (action == "Cancel")
            {
                if (state == "Pass")
                {
                    MessageBox.Show("Can't Cancel Passed Cheque");
                    return;
                }
            }
            else
            {
                return;
            }
            string type = radPayable.Checked ? "Payment" : "Receipt";
           
            var frm = new frmCheckStateDate(
                                              type,
                                             dgvCustomer.CurrentRow.Cells["TRS REF"].Value.ToString(), // code
                                             int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString()), // checkDetailId
                                             DateTime.Parse(dgvCustomer.CurrentRow.Cells["Cheque Date"].Value.ToString()), // checkDate
                                             dgvCustomer.CurrentRow.Cells["Cheque Name"].Value.ToString(), // name
                                             action, // state/action
                                             action == "Return" && dgvCustomer.CurrentRow.Cells["pass_date"].Value != DBNull.Value
                                                 ? (DateTime?)Convert.ToDateTime(dgvCustomer.CurrentRow.Cells["pass_date"].Value)
                                                 : null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                DateTime selectedDate = frm.SelectedDate;
                    int checkDetailId = int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString());
                    if (type == "Payment")
                    {
                        GetFullTransactionDataPayable(checkDetailId, selectedDate, true, action);
                    }
                    else if (type == "Receipt")
                    {
                        GetFullTransactionDataReceivable(int.Parse(REFID), selectedDate, true, action);
                    }
                    if (radPayable.Checked)
                        BinChqPayable();
                    else BinChqReceivable();
            }
        //}
        }
        public void GetFullTransactionDataPayable(int checkId, DateTime selectedDate, bool isPayable, string action)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                            SELECT 
                        cd.id AS check_detail_id,
                        cd.amount AS check_amount,
                        cd.check_date,
                        cd.pass_date,
                        cd.check_no,
                        cd.check_name,
                        cd.check_type,
                        cd.state AS check_state,

                        pv.id AS payment_voucher_id,
                        pv.code AS voucher_code,
                        pv.date AS voucher_date,
                        pv.debit_account_id,
                        pv.credit_account_id,
                        pv.trans_name,
                        pv.trans_ref,
                        pv.bank_id,

                        b.name AS bank_name,

                        pvd.id AS voucher_detail_id,
                        pvd.hum_id,
                        pvd.inv_id,
                        pvd.inv_code,
                        pvd.payment,
                        pvd.voucher_type

                    FROM tbl_check_details cd
                    INNER JOIN tbl_payment_voucher pv ON cd.pvc_no = pv.id
                    INNER JOIN tbl_bank b ON pv.bank_id = b.id
                    INNER JOIN tbl_payment_voucher_details pvd ON pvd.payment_id = pv.id
                    WHERE cd.id = @checkId;",
                DBClass.CreateParameter("checkId", checkId)))
            {
                while (reader.Read())
                {
                    int creditAccountId = 0, debitAccountId = 0;
                    int bankId = reader.GetInt32("bank_id");
                    int crId = reader.GetInt32("debit_account_id");
                    int drId = reader.GetInt32("credit_account_id");
                    string humId = reader["hum_id"].ToString();
                    decimal amount = reader.GetDecimal("payment");
                    string checkDetailId = reader["check_detail_id"].ToString();
                    string tType = reader["check_type"].ToString();
                    string code = reader["voucher_code"].ToString();
                    string currentState = dgvCustomer.CurrentRow.Cells["state"].Value.ToString();

                    object result = DBClass.ExecuteScalar(@"SELECT a.id FROM tbl_coa_level_4 a JOIN tbl_coa_level_3 b ON a.main_id = b.id WHERE b.NAME = 'Banks' LIMIT 1");

                    int bankAccountId = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                    // Set default values
                    debitAccountId = 0;
                    creditAccountId = 0;
                    if (currentState == "New")
                    {
                        if (action == "Pass")
                        {
                            // credit to bank
                            if (bankAccountId > 0)
                            {
                                creditAccountId = bankAccountId;
                            }
                            debitAccountId = pdcPayable;
                        }
                        if (action == "Return")
                        {
                            // credit to bank
                            if (bankAccountId > 0)
                            {
                                debitAccountId = bankAccountId;
                            }
                            creditAccountId = pdcPayableReturn;
                        }
                        if (action == "Cancel")
                        {
                            debitAccountId = pdcPayable;
                            creditAccountId = crId;//pdcPayableCancel;
                        }
                        if (action == "Hold")
                        {
                            debitAccountId = pdcPayable;
                            creditAccountId = pdcPayableHold;
                        }
                    }
                    else
                    {
                        using (MySqlDataReader red = DBClass.ExecuteReader(@"SELECT account_id,debit,credit,transaction_id FROM tbl_transaction WHERE type = 'PDC Payable' and transaction_id=@id order by id DESC limit 2", DBClass.CreateParameter("id", checkDetailId)))
                        {
                            int drAcId = 0, crAcId = 0;
                            string ac = "", transId = "";
                            while (red.Read())
                            {
                                ac = red["account_id"].ToString();
                                transId = red["transaction_id"].ToString();
                                if ((decimal.Parse(red["debit"].ToString()) > 0))
                                {
                                    drAcId = int.Parse(red["account_id"].ToString());
                                }
                                else
                                {
                                    crAcId = int.Parse(red["account_id"].ToString());
                                }
                            }
                            if (action == "Pass")
                            {
                                // Debit Bank, Credit drId
                                if (crAcId > 0)
                                    debitAccountId = crAcId;

                                if (bankAccountId > 0)
                                    creditAccountId = bankAccountId;
                                // credit to bank
                                if (bankAccountId > 0)
                                {
                                    creditAccountId = bankAccountId;
                                }
                                debitAccountId = pdcPayable;
                            }
                            if (action == "Return")
                            {
                                // Debit Bank, Credit PDC Receivable Return
                                if (crAcId > 0)
                                    debitAccountId = crAcId;
                                
                                creditAccountId = pdcPayableReturn;
                            }
                            if (action == "Cancel")
                            {
                                if (drAcId > 0)
                                    debitAccountId = drAcId;

                                creditAccountId = pdcPayable;

                                debitAccountId = pdcPayable;
                                creditAccountId = crId;//pdcPayableCancel;
                            }
                            if (action == "Hold")
                            {
                                debitAccountId = pdcPayable;
                                creditAccountId = pdcPayableHold;
                            }
                        }

                    }
                    if (currentState.Trim().ToLower() != action.Trim().ToLower())
                    {
                        InsertJournalEntriesPayable(selectedDate, debitAccountId.ToString(), creditAccountId.ToString(), amount, checkDetailId, humId, tType, code, isPayable, action);
                    }
                }
            }
        }
        //Receivable
        public void GetFullTransactionDataReceivable(int Id, DateTime selectedDate, bool isPayable, string action)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                               SELECT 
                                cd.id AS check_detail_id,
                                cd.amount AS check_amount,
                                cd.check_date,
                                cd.pass_date,
                                cd.check_no,
                                cd.check_name,
                                cd.check_type,
                                cd.state AS check_state,
                                pv.id AS payment_voucher_id,
                                pv.code AS voucher_code,
                                pv.date AS voucher_date,
                                pv.debit_account_id,
                                pv.credit_account_id,
                                pv.trans_name,
                                pv.trans_ref,
                                pv.bank_id,
                                '' AS bank_name,
                                pvd.id AS voucher_detail_id,
                                pvd.hum_id,
                                pvd.inv_id,
                                pvd.inv_code,
                                pvd.payment,
                               '' voucher_type
                            FROM tbl_check_details cd
                            INNER JOIN tbl_receipt_voucher pv ON cd.pvc_no = pv.id
                            -- INNER JOIN tbl_bank b ON pv.bank_id = b.id
                            INNER JOIN tbl_receipt_voucher_details pvd ON pvd.payment_id = pv.id
                            WHERE cd.check_type ='Receipt' AND pv.id=@id",
                DBClass.CreateParameter("id", Id)))
            {
                while (reader.Read())
                {
                    int debitAccountId = 0, creditAccountId = 0;
                    int drId = reader.GetInt32("debit_account_id");
                    int crId = reader.GetInt32("credit_account_id");
                    string humId = reader["hum_id"].ToString();
                    decimal amount = reader.GetDecimal("payment");
                    string checkDetailId = reader["check_detail_id"].ToString();
                    string tType = reader["check_type"].ToString();
                    string code = reader["voucher_code"].ToString();
                    string currentState = dgvCustomer.CurrentRow.Cells["state"].Value.ToString();

                    object result = DBClass.ExecuteScalar(@"SELECT a.id FROM tbl_coa_level_4 a JOIN tbl_coa_level_3 b ON a.main_id = b.id WHERE b.NAME = 'Banks' LIMIT 1");

                    int bankAccountId = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                    // Set default values
                    debitAccountId = 0;
                    creditAccountId = 0;

                    if (currentState == "New")
                    {
                        if (action == "Pass")
                        {
                            // Debit Bank, Credit drId
                            if (bankAccountId > 0)
                                debitAccountId = bankAccountId;

                            creditAccountId = drId;
                        }
                        else if (action == "Return")
                        {
                            // Debit Bank, Credit PDC Receivable Return
                            if (bankAccountId > 0)
                                debitAccountId = bankAccountId;

                            creditAccountId = pdcReceivableReturn;
                        }
                        else if (action == "Cancel")
                        {
                            // Debit PDC Receivable, Credit crId or Bank if drId <= 0
                            debitAccountId = pdcReceivable;

                            if (drId <= 0)
                            {
                                if (bankAccountId > 0)
                                    creditAccountId = bankAccountId;
                            }
                            else
                            {
                                creditAccountId = crId; // originally pdcReceivableCancel
                            }
                        }
                        else if (action == "Hold")
                        {
                            // Debit PDC Receivable, Credit PDC Receivable Hold
                            debitAccountId = pdcReceivable;
                            creditAccountId = pdcReceivableHold;
                        }
                    } else
                    {
                        using (MySqlDataReader red = DBClass.ExecuteReader(@"SELECT account_id,debit,credit,transaction_id FROM tbl_transaction WHERE type = 'PDC Receivable' and transaction_id=@id order by id DESC limit 2", DBClass.CreateParameter("id", checkDetailId)))
                        {
                            int drAcId = 0, crAcId = 0;
                            string ac = "", transId = "";
                            while (red.Read())
                            {
                                ac = red["account_id"].ToString();
                                transId = red["transaction_id"].ToString();
                                if ((decimal.Parse(red["debit"].ToString()) > 0))
                                {
                                    drAcId = int.Parse(red["account_id"].ToString());
                                }
                                else
                                {
                                    crAcId = int.Parse(red["account_id"].ToString());
                                }
                            }
                            if (action == "Pass")
                            {
                                // Debit Bank, Credit drId
                                if (drAcId > 0)
                                    debitAccountId = drAcId;

                                if(bankAccountId>0)
                                    creditAccountId = bankAccountId;
                            }
                            else if (action == "Return")
                            {
                                // Debit Bank, Credit PDC Receivable Return
                                if (drAcId > 0)
                                    debitAccountId = drAcId;

                                creditAccountId = pdcReceivableReturn;
                            }
                            else if (action == "Cancel")
                            {
                                // Debit PDC Receivable, Credit crId or Bank if drId <= 0'

                                if (drAcId > 0)
                                    debitAccountId = drAcId;

                                creditAccountId = pdcReceivable;

                                //if (drId <= 0)
                                //{
                                //    if (bankAccountId > 0)
                                //        creditAccountId = bankAccountId;
                                //}
                                //else
                                //{
                                //    creditAccountId = crId; // originally pdcReceivableCancel
                                //}
                            }
                            else if (action == "Hold")
                            {
                                // Debit PDC Receivable, Credit PDC Receivable Hold
                                debitAccountId = pdcReceivable;
                                creditAccountId = pdcReceivableHold;
                            }
                        }
                    }
                    if (currentState.Trim().ToLower() != action.Trim().ToLower())
                    {
                        // Insert journal entry
                        InsertJournalEntriesReceivable(
                            selectedDate,
                            debitAccountId.ToString(),
                            creditAccountId.ToString(),
                            amount,
                            checkDetailId,
                            humId,
                            tType,
                            code,
                            isPayable,
                            action
                        );
                    }
                }
            }
        }
        //Payable
        private void InsertJournalEntriesPayable(DateTime selectedDate,string debitAccountId,string creditAccountId, decimal amount,string checkDetailId,string humId,string tType,string code,bool isPayable,string action)
        {
            string category = "PDC Payable";
            if (action == "Return")
                category = "PDC Payable Return";
            else if (action == "Hold")
                category = "PDC Payable Hold";
            else if (action == "Cancel")
                category = "PDC Payable Cancel";
            
            string pdcAccountId = "";

            using (MySqlDataReader reader = DBClass.ExecuteReader(
                "SELECT id FROM tbl_coa_level_4 WHERE id = (SELECT account_id FROM tbl_coa_config WHERE category = @cat)",
                DBClass.CreateParameter("@cat", category)))
            {
                if (reader.Read())
                {
                    pdcAccountId = reader["id"].ToString();
                    if (category != "PDC Payable")
                    {
                        debitAccountId = creditAccountId;
                        creditAccountId = pdcAccountId;
                    }
                }
            }

            if (string.IsNullOrEmpty(pdcAccountId))
            {
                MessageBox.Show($"Account not found for category: {category}");
                return;
            }

            CommonInsert.addTransactionEntry(
                selectedDate,
                debitAccountId,
                amount.ToString("0.000"),
                "0",
                checkDetailId,
                humId,
                "PDC Payable",
                "PDC Payable",
                "PDC Payable NO. " + $"{action} Cheque",
                frmLogin.userId,
                DateTime.Now.Date,
                ""
            );
            
            CommonInsert.addTransactionEntry(
                selectedDate,
                creditAccountId,
                "0",
                amount.ToString("0.000"),
                checkDetailId,
                "0",
                "PDC Payable",
                "PDC Payable",
                "PDC Payable NO. " + $"{action} Cheque",
                frmLogin.userId,
                DateTime.Now.Date,
                ""
            );
        }

        private void InsertJournalEntriesReceivable(DateTime selectedDate, string debitAccountId,string creditAccountId, decimal amount, string checkDetailId, string humId, string tType, string code, bool isPayable, string action)
        {
            string category = "PDC Receivable";
            if (action == "Return")
                category = "PDC Receivable Return";
            else if (action == "Hold")
                category = "PDC Receivable Hold";
            else if (action == "Cancel")
                category = "PDC Receivable Cancel";
            
            string pdcAccountId = debitAccountId;

            if (string.IsNullOrEmpty(pdcAccountId))
            {
                MessageBox.Show($"Account not found for category: {category}");
                return;
            }

            CommonInsert.addTransactionEntry(
                selectedDate,
                debitAccountId,
                "0",
                amount.ToString("0.000"),
                checkDetailId,
                "0",
                "PDC Receivable",
                "PDC Receivable",
                "PDC Receivable NO. " + $"{action} Cheque",
                frmLogin.userId,
                DateTime.Now.Date,
                ""
            );
            CommonInsert.addTransactionEntry(
                selectedDate,
                creditAccountId,
                amount.ToString("0.000"),
                "0",
                checkDetailId,
                humId,
               "PDC Receivable",
                "PDC Receivable",
                "PDC Receivable NO. " + $"{action} Cheque",
                frmLogin.userId,
                DateTime.Now.Date,
                ""
            );
            
        }
    }
}
