using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterActivity : Form
    {
        private frmViewProjectPlanning master;
        int id, tenderId, projectId, siteId, tenderNameId, planningId;
        List<string> assignedTeam = new List<string>();
        bool isExported = false;
        public MasterActivity(frmViewProjectPlanning _master, int id, int _projectId, int _tenderNameId, int _siteId, int _tenderId, int _planningId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = id;
            this.tenderId = _tenderId;
            this.projectId = _projectId;
            this.siteId = _siteId;
            this.planningId = _planningId;
            this.tenderNameId = _tenderNameId;
        }
        private void MasterActivity_Load(object sender, EventArgs e)
        {
            LoaddgvItems();
            BindCombo();

            if (id != 0)
            {
                BindData();
            }
            if (tenderId != 0)
            {
                BindGridData();
            }
        }
        private void LoaddgvItems()
        {
            dgvItems.Rows.Clear();
            //DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee");
            //DataGridViewComboBoxColumn cmbItemName = (DataGridViewComboBoxColumn)dgvItems.Columns["assigned"];
            //cmbItemName.DataSource = dt;
            //cmbItemName.DisplayMember = "name";
            //cmbItemName.ValueMember = "id";
        }
        public void BindCombo()
        {
            //
        }
        private void BindData()
        {
            //
        }
        private void BindGridData()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.item_id = tbl_items_boq.id
                                WHERE tbl_project_tender_details.tender_id = @id;", DBClass.CreateParameter("id", tenderId)))
            {
                int count = 0;
                while (reader.Read())
                {
                    string dated = DateTime.Now.Date.ToString();
                    DateTime dateX = Convert.ToDateTime(dated);
                    dated = dateX.ToString("dd/MM/yyyy");
                    string startDate = dated;
                    string endDate = dated;
                    if (reader["start_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["start_date"]);
                        startDate = date.ToString("dd/MM/yyyy");
                    }
                    if (reader["end_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["end_date"]);
                        endDate = date.ToString("dd/MM/yyyy");
                    }

                    string empId = "0";
                    if (reader["assigned"] == null || reader["assigned"].ToString().Length > 0)
                    {
                        empId = reader["assigned"].ToString();
                    }
                    string progress = "0";
                    if (reader["progress"] == null || reader["progress"].ToString().Length > 0)
                    {
                        progress = decimal.Parse(reader["progress"].ToString()).ToString("#.##");
                    }
                    dgvItems.Rows.Add((count++), reader["id"].ToString(), reader["sr"].ToString(), reader["name"].ToString(),
                    startDate,
                    endDate,
                    progress);
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (id == 0)
            //{
            //    if (insertData())
            //        this.Close();
            //}
            //else
            //{
            if (updateData())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            //}
        }

        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            //dooo
            updatePlaning();

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;
            //dooo
            updatePlaning();

            return true;
        }
        private void updatePlaning()
        {
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    string startDate = "2025-01-01";
                    if (row.Cells["start_date"].Value != null && !string.IsNullOrWhiteSpace(row.Cells["start_date"].Value.ToString()))
                    {
                        startDate = DateTime.ParseExact(
                            row.Cells["start_date"].Value.ToString(),
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture
                        ).ToString("yyyy-MM-dd");
                    }
                    string endDate = "2025-01-01";
                    if (row.Cells["end_date"].Value != null && !string.IsNullOrWhiteSpace(row.Cells["end_date"].Value.ToString()))
                    {
                        endDate = DateTime.ParseExact(
                            row.Cells["end_date"].Value.ToString(),
                            "dd/MM/yyyy",
                            CultureInfo.InvariantCulture
                        ).ToString("yyyy-MM-dd");
                    }
                    DateTime startedDate = DateTime.Parse(startDate);
                    DateTime entedDate = DateTime.Parse(endDate);
                    string _progress = row.Cells["progress"].Value == null ? "0" : row.Cells["progress"].Value.ToString().Length > 0 ? row.Cells["progress"].Value.ToString() : "0";

                    DBClass.ExecuteNonQuery(@"UPDATE tbl_project_tender_details 
                                    SET progress=@progress,start_date=@startDate,end_date=@endDate WHERE tender_id = @tenderId AND id=@id",
                    DBClass.CreateParameter("tenderId", tenderId),
                    DBClass.CreateParameter("id", row.Cells["tid"].Value),
                    DBClass.CreateParameter("progress", _progress),
                    DBClass.CreateParameter("startDate", startedDate.Date),
                    DBClass.CreateParameter("endDate", entedDate.Date));
                    Utilities.LogAudit(frmLogin.userId, "Update Project Tender Details", "Project Tender Details", Convert.ToInt32(row.Cells["tid"].Value), 
                        $"Updated Project Tender Details: {row.Cells["name"].Value} with Start Date: {startDate}, End Date: {endDate}, Progress: {_progress}");
                }
            }

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_planning SET modified_by = @modifiedBy, modified_date = @modifiedDate, progress=@progress WHERE id = @id;",
            DBClass.CreateParameter("id", planningId),
            DBClass.CreateParameter("progress", 0),
            DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
            DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));
            Utilities.LogAudit(frmLogin.userId, "Update Project Planning", "Project Planning", planningId, "Updated Project Planning with ID: " + planningId);

        }
        private bool chkRequiredDate()
        {
            if (string.IsNullOrEmpty(projectId.ToString()))
            {
                MessageBox.Show("Select Project First.");
                return false;
            }
            if (string.IsNullOrEmpty(tenderId.ToString()))
            {
                MessageBox.Show("Select Tender First.");
                return false;
            }
            if (string.IsNullOrEmpty(planningId.ToString()))
            {
                MessageBox.Show("Select planning Not initiated.");
                return false;
            }
            return true;
        }

        private void headerUC1_Load(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void resetTextBox()
        {
            id = 0;
        }

        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertData())
                    resetTextBox();
            }
            else
            {
                if (updateData())
                    resetTextBox();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvItems.Columns[e.ColumnIndex].Name == "start_activity")
            {
                var row = dgvItems.Rows[e.RowIndex];
                int itemId = Convert.ToInt32(row.Cells["tid"].Value);
                
                new frmActivity(this, 0, projectId, tenderId, siteId, tenderId, planningId, itemId).ShowDialog();
                BindGridData();
            }
        }

        private void BindItemsLoad(int refId)
        {
            string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
            int countX = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", refId)));
            isExported = countX > 0 ? true : false;

            dgvItems.Rows.Clear();
            string query = "";
            if (isExported)
            {
                //resetGridView();
                query = @"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.sr=tbl_items_boq.sr
                                WHERE tbl_project_tender_details.tender_id= @id;";
            }
            else
            {
                query = @"SELECT tbl_project_tender_details.*, tbl_items.code as code,tbl_items.name,tbl_items.type,(select name from tbl_unit where id=tbl_project_tender_details.unit_id) as unit_name FROM tbl_project_tender_details INNER JOIN 
                                                                    tbl_items ON tbl_project_tender_details.item_id = tbl_items.id WHERE 
                                                                    tbl_project_tender_details.tender_id = @id;";
            }
            MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", refId));
            int count = 0;
            while (reader.Read())
            {
                decimal totalAmount = 0, subTotal = 0, marginPercentage = 0, margin = 0;
                subTotal = Convert.ToDecimal(reader["rate"].ToString()) * Convert.ToDecimal(reader["qty"].ToString());
                marginPercentage = Convert.ToDecimal(reader["margin_percentage"].ToString());
                margin = Convert.ToDecimal(reader["margin_amount"].ToString());
                totalAmount = subTotal + margin;
                if (isExported)
                {
                    string dated = DateTime.Now.Date.ToString();
                    DateTime dateX = Convert.ToDateTime(dated);
                    dated = dateX.ToString("dd/MM/yyyy");
                    string startDate = dated;
                    string endDate = dated;
                    if (reader["start_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["start_date"]);
                        startDate = date.ToString("dd/MM/yyyy");
                    }
                    if (reader["end_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["end_date"]);
                        endDate = date.ToString("dd/MM/yyyy");
                    }

                    string empId = "0";
                    if (reader["assigned"] == null || reader["assigned"].ToString().Length > 0)
                    {
                        empId = reader["assigned"].ToString();
                    }
                    dgvItems.Rows.Add((count++), reader["id"].ToString(), reader["sr"].ToString(), reader["name"].ToString(), 
                        (int.Parse(empId) > 0 ? reader["assigned"] : null),
                        startDate,
                        endDate,
                        0);
                }
                else
                {
                    string dated = DateTime.Now.Date.ToString();
                    DateTime dateX = Convert.ToDateTime(dated);
                    dated = dateX.ToString("dd/MM/yyyy");
                    string startDate = dated;
                    string endDate = dated;
                    if (reader["start_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["start_date"]);
                        startDate = date.ToString("dd/MM/yyyy");
                    }
                    if (reader["end_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["end_date"]);
                        endDate = date.ToString("dd/MM/yyyy");
                    }

                    dgvItems.Rows.Add((count++), reader["id"].ToString(), reader["sr"].ToString(), reader["name"].ToString(),
                        (int.Parse(reader["assigned"].ToString()) > 0 ? reader["assigned"] : null),
                        startDate,
                        endDate,
                        0);
                }
            }
        }
    }
}
