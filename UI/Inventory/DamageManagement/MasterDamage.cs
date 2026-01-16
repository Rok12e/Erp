using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterDamage : Form
    {
        private EventHandler employeeUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;
        public MasterDamage()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            employeeUpdatedHandler = (sender, args) => BindCombos.PopulateEmployees(cmbCustomer);
            invoiceUpdatedHandler = (sender, args) => BindInvoices();
            EventHub.Employee += employeeUpdatedHandler;
            EventHub.DamageInv += invoiceUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewDamage(this, 0));
        }
        private void MasterDamage_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateEmployees(cmbCustomer);
            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            string query = "";
            if (cmbSelectionMethod.Text == "Default")
            {
                query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY tbl_damage.date) AS `SN`, 
                            tbl_damage.date AS Date, 
                            tbl_damage.id,
                        concat('000',    MAX(tbl_transaction.transaction_id)) AS 'JV NO', 
                            tbl_damage.reference_no AS 'INV NO', 
                            CONCAT(tbl_employee.code,' - ', tbl_employee.name) AS 'Employee Name',
                            tbl_damage.total AS Total
                        FROM 
                            tbl_damage
                        INNER JOIN 
                            tbl_transaction ON tbl_damage.id = tbl_transaction.transaction_id
                        INNER JOIN 
                            tbl_employee ON tbl_damage.reported_by = tbl_employee.id
                        WHERE 
                            tbl_damage.state = 0

                        ";
            }
            else
            {
                query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY tbl_damage.date) AS `SN`, 
                            tbl_damage.date AS Date, 
                            tbl_damage.id,
                            tbl_damage.reference_no AS 'INV NO', 
                            CONCAT(tbl_employee.code,' - ', tbl_employee.name) AS 'Employee Name',
                            tbl_damage.total AS Total,concat(ti.code,' - ',ti.name) as 'Item Name' ,ts.qty As Qty,ts.cost_price 
                            As 'Cost Price'  ,ts.total as 'Item Total'
                        FROM 
                            tbl_damage
                         INNER JOIN tbl_damage_details ts ON tbl_damage.id = ts.damage_id
                        inner join tbl_items ti on ts.item_id =ti.id
                        INNER JOIN 
                            tbl_employee ON tbl_damage.reported_by = tbl_employee.id
                        WHERE 
                            tbl_damage.state = 0

                        ";
            }
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbCustomer.Text != "" && !chkCustomer.Checked)
            {
                query += " and tbl_damage.reported_by = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbCustomer.SelectedValue.ToString()));
            }
            if (!chkDate.Checked)
                query += " and tbl_damage.date >= @dateFrom and tbl_damage.date <= @dateTo";
            if (cmbSelectionMethod.Text == "Default")
                query += " GROUP BY tbl_damage.id, tbl_damage.date, tbl_damage.reference_no, tbl_employee.code, tbl_employee.name, tbl_damage.total; ";

            else
                query += " GROUP BY tbl_damage.id, tbl_damage.date, tbl_damage.reference_no, tbl_employee.code, tbl_employee.name, tbl_damage.total,ti.code,ti.name,ts.qty,ts.cost_price,ts.total; ";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["Employee Name"].MinimumWidth = 200;
            dgvCustomer.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["id"].Visible = false;
            if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;

            if (dgvCustomer.Rows.Count > 0)
            {
                btnDelete.Enabled = btnRestore.Enabled = UserPermissions.canDelete("Damage");
                btnEdit.Enabled = UserPermissions.canEdit("Damage");
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void bindInvoiceItems()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.cost_price as 'Cost Price'  , ts.total as Total from 
                                                     tbl_damage_details ts inner join tbl_items ti on ts.item_id = ti.id where damage_id = @id",
                                                     DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            dgvItems.DataSource = dt;
            dgvItems.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewDamage(this, int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewDamage(this, int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
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
            if (dgvCustomer.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_damage SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Damage", "Damage", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Damage: " + dgvCustomer.SelectedRows[0].Cells["INV NO"].Value.ToString());
            BindInvoices();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("Damage");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();

        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
                cmbCustomer.Enabled = false;
            else
                cmbCustomer.Enabled = true;
            BindInvoices();
        }
      
        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindInvoices();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;
        }

        private void MasterDamage_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Employee -= employeeUpdatedHandler;
            EventHub.DamageInv -= invoiceUpdatedHandler;
        }
    }
}
