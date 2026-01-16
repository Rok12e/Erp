using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterProjectTendering : Form
    {
        private EventHandler eventHandler;
        bool isExported = false;
        public MasterProjectTendering()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            eventHandler = (sender, args) => BindInvoices();
            EventHub.ProjectTendering += eventHandler;
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
            this.Text = "Project Tender Center";
            headerUC1.FormText = this.Text;
            LoadCategories();
        }

        private void LoadCategories()
        {
            //
        }
        

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewProjectTendering(0));
        }
        private void MasterProjectTendering_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateProjects(cmbProject);
            BindInvoices();
        }
        public void BindInvoices()
        {
            DataTable dt;
            string query = @"SELECT 
                    ROW_NUMBER() OVER (ORDER BY tbl_project_tender.date) AS `SN`, tbl_project_tender.date AS DATE,tbl_project_tender.id,
						  CONCAT(tbl_projects.code,' - ', tbl_projects.name) AS 'Project Name',
						  CONCAT(tbl_tender_names.code,' - ', tbl_tender_names.name) AS 'Tender Name', 
						  tbl_project_tender.submission_date as 'Submit Date',
						  tbl_project_tender.fees as 'Fees'
                    FROM  tbl_project_tender
						  INNER JOIN 
                        tbl_projects ON tbl_project_tender.project_id = tbl_projects.id
						  INNER JOIN 
                        tbl_tender_names ON tbl_project_tender.tender_name_id = tbl_tender_names.id
                    WHERE 
                        tbl_project_tender.state = 0
                    ";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbProject.Text != "" && !chkProject.Checked)
            {
                query += " and tbl_project_tender.project_id = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbProject.SelectedValue.ToString()));
            }
            if (cmbTenderName.Text != "" && !chkTenderName.Checked)
            {
                query += " and tbl_project_tender.tender_name_id = @tender";
                parameters.Add(DBClass.CreateParameter("tender", cmbTenderName.SelectedValue.ToString()));
            }
            if (!chkDate.Checked)
                query += " and tbl_project_tender.date >= @dateFrom and tbl_project_tender.date <= @dateTo";

            // query += " GROUP BY tbl_project_tender.id, tbl_project_tender.date, tbl_project_tender.invoice_id, tbl_project.code, tbl_project.name, tbl_project_tender.total,ti.code,ti.name,ts.qty,ts.price,ts.vat,ts.total; ";

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
            string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
            int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", dgView.SelectedRows[0].Cells["id"].Value.ToString())));

            isExported = count > 0 ? true : false;

            string query = "";
            if (isExported)
            {
                query = @"Select concat(ti.sr,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.rate as Rate , 
                            ts.unit_id as Unit , ts.amount as Amount 
                            from tbl_project_tender_details ts 
                            inner join tbl_items_boq ti on tender_id = ref_id AND ts.item_id = ti.id
                            where tender_id = @id";
            }
            else
            {
                query = @"Select concat(ti.code,' - ' ,ti.name) as 'Item Name' ,ts.qty as Qty ,ts.rate as Rate , 
                            (select NAME FROM tbl_unit WHERE id=ts.unit_id) as Unit , ts.amount as Amount 
                            from tbl_project_tender_details ts 
                            inner join tbl_items ti on ts.item_id = ti.id
                            where tender_id = @id";
            }
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
            frmLogin.frmMain.openChildForm(new frmViewProjectTendering(int.Parse(dgView.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;

            frmLogin.frmMain.openChildForm(new frmViewProjectTendering(int.Parse(dgView.CurrentRow.Cells["id"].Value.ToString())));
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
            string query = "UPDATE tbl_project_tender SET state = -1 WHERE id = @id ";
            DBClass.ExecuteNonQuery(query, DBClass.CreateParameter("id", dgView.SelectedRows[0].Cells["id"].Value.ToString()));
            BindInvoices();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterRecycle("Project Tender"));
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
