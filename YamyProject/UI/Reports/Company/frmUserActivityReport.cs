using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;
using DataTable = System.Data.DataTable;

namespace YamyProject
{
    public partial class frmUserActivityReport : Form
    {
        private readonly int userId = 0;
        public frmUserActivityReport(int _userId = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
            this.userId = _userId;
        }
        
        private void frmAuditTrail_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateProjects(cmbCustomer);
            
            BindData();
        }
        public void BindData()
        {
            DataTable dt;
            string query = @"SELECT a.id, u.user_name username, a.action_type, a.module_name, a.record_id, a.details, a.action_time, a.ip_address
                        FROM tbl_audit_log a
                        JOIN tbl_sec_users u ON a.user_id = u.id
                        ";
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (!chkDate.Checked)
            {
                query += " and a.action_time >= @dateFrom and action_time <= @dateTo";
            }
            if (userId != 0)
            {
                query += " and a.user_id = @userId ";
                parameters.Add(DBClass.CreateParameter("userId", userId));
            }
            query += " ORDER BY a.action_time DESC;";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value));
            
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvItems.DataSource = dt;

            foreach (DataGridViewColumn column in dgvItems.Columns)
            {
                if (column.Name == "progress")
                {
                    column.DefaultCellStyle.Format = "N2";
                }
            }
            
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        
        private void dgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count == 0)
                return;
            //if (dgvItems.Columns["JV NO"].Index == e.ColumnIndex)
            //    _mainForm.openChildForm(new MasterTransactionJournal(_mainForm, dgvItems.CurrentRow.Cells["JV NO"].Value.ToString(), "Sales"));
        }
        private void dgvItems_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvItems_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
                cmbCustomer.Enabled = false;
            else
                cmbCustomer.Enabled = true;
            BindData();
        }
        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayment.Checked)
                cmbPaymentMethod.Enabled = false;
            else
                cmbPaymentMethod.Enabled = true;
            BindData();
        }
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindData();
        }

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvItems.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            //    bindInvoiceItems();
            //else
            //    dgvItems.DataSource = null;
        }

        private void cmbSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtFrom_VisibleChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }
    }
}
