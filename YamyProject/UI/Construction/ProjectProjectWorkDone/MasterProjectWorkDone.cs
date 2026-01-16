using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterProjectWorkDone : Form
    {

        bool isExported = false;
        public MasterProjectWorkDone()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "Project WorkDone Center";
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewProjectWorkDone(this,0));
        }
        private void MasterProjectWorkDone_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateProjects(cmbProject);
            BindCombos.PopulateTenderNames(cmbTenderName);
            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            string query = @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY tbl_project_work_done.date) AS `Sn`, tbl_project_work_done.date AS Date,tbl_project_work_done.id,
                    tbl_project_work_done.id 'V No',
						  CONCAT(tbl_projects.code,' - ', tbl_projects.name) AS 'Project Name',
						  CONCAT(tbl_tender_names.code,' - ', tbl_tender_names.name) AS 'Tender Name', 
						  (SELECT SUM(qty_total) from tbl_project_work_done_details WHERE ref_id = tbl_project_work_done.id) as 'total QtyP',
						  (SELECT SUM(qty_used) from tbl_project_work_done_details WHERE ref_id = tbl_project_work_done.id) AS 'total QtyWD'
                    FROM  tbl_project_work_done
						  INNER JOIN 
                        tbl_project_planning ON tbl_project_work_done.planning_id = tbl_project_planning.id
						  INNER JOIN 
                        tbl_projects ON tbl_project_planning.project_id = tbl_projects.id
						  INNER JOIN 
                        tbl_tender_names ON tbl_project_planning.tender_name_id = tbl_tender_names.id
                    WHERE 
                        tbl_project_work_done.state = 0";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbProject.Text != "" && !chkProject.Checked)
            {
                query += " and tbl_project_planning.project_id = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbProject.SelectedValue.ToString()));
            }
            if (cmbTenderName.Text != "" && !chkTenderName.Checked)
            {
                query += " and tbl_project_planning.tender_name_id = @tender";
                parameters.Add(DBClass.CreateParameter("tender", cmbTenderName.SelectedValue.ToString()));
            }
            if (!chkDate.Checked)
                query += " and tbl_project_work_done.date >= @dateFrom and tbl_project_work_done.date <= @dateTo";
            
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgView.DataSource = dt;
            dgView.Columns["Project Name"].MinimumWidth = 200;
            dgView.Columns["Project Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgView.Columns["Tender Name"].MinimumWidth = 200;
            dgView.Columns["Tender Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgView.Columns["id"].Visible = false;
            if (dgView.Rows.Count > 0)
            {
                bindInvoiceItems();
            } 
            else
                dgvItems.DataSource = null;
        }

        private void bindInvoiceItems()
        {
            string checkQuery = "SELECT COUNT(*) FROM tbl_project_work_done_details where ref_id =@id";
            int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", dgView.SelectedRows[0].Cells["id"].Value.ToString())));
            string query = @"SELECT CONCAT(ti.sr, ' - ', ti.name) AS 'Item Name', pwd.qty_used AS Qty, ti.price AS Rate,
                                pwd.unit AS Unit, (pwd.qty_used * ti.price) AS Amount
                            FROM tbl_project_work_done_details pwd INNER JOIN tbl_items_boq ti ON pwd.main_id = ti.id
                            WHERE pwd.ref_id = @id;";
            
            DataTable dt = DBClass.ExecuteDataTable(query,
                                                     DBClass.CreateParameter("id", dgView.SelectedRows[0].Cells["id"].Value.ToString()));
            dgvItems.DataSource = dt;
            dgvItems.Columns["item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void cmbTenderName_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewProjectWorkDone(this, int.Parse(dgView.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;

            frmLogin.frmMain.openChildForm(new frmViewProjectWorkDone(this, int.Parse(dgView.CurrentRow.Cells["id"].Value.ToString())));
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
            //string query= "UPDATE tbl_project_tender SET state = -1 WHERE id = @id ";
            //DBClass.ExecuteNonQuery(query, DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            //BindInvoices();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));

        }
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProject.Checked)
                cmbProject.Enabled = false;
            else
                cmbProject.Enabled = true;
            BindInvoices();
        }
        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTenderName.Checked)
                cmbTenderName.Enabled = false;
            else
                cmbTenderName.Enabled = true;
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
            if (dgView.Rows.Count > 0)
                bindInvoiceItems();
            else
                dgvItems.DataSource = null;
        }

        private void cmbSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            BindInvoices();
        }
    }
}
