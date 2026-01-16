using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewProjectManagement : Form
    {
        private MasterProjectManagement master;
        int id,pId;
        string projectPlanningId = "0", projectId = "0", tenderId = "0";
        public frmViewProjectManagement(MasterProjectManagement _master, int _id,int _pId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            //this.Height = 765;
            //this.Width = 800;
            this.master = _master;
            this.id = _id;
            this.pId = _pId;
            this.Text = "Projects - Management";
            headerUC1.FormText = this.Text;
        }
        private void frmViewProjectManagement_Load(object sender, EventArgs e)
        {
            dtDate.Value = dtProject.Value = dtp_end.Value = dtp_start.Value = DateTime.Now.Date;
            //BindCombos.PopulateCitieAll(cmbProjectLocation);
            BindCombos.PopulateAllLevel4Account(cmbFundAccount);
            BindCombo();
            BindInvoice();
        }
        public void BindCombo()
        {
            //BindCombos.PopulateProjects(cmbProjectName);
        }
        private void BindInvoice()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select pl.*, p.name,p.code, (SELECT NAME FROM tbl_tender_names WHERE tbl_tender_names.id=tender_id) AS `tender_name` from tbl_project_planning pl, tbl_projects p WHERE pl.project_id=p.id and pl.id = @id",
                DBClass.CreateParameter("id", pId)))
            {
                if (reader.Read())
                {
                    dtProject.Value = DateTime.Parse(reader["date"].ToString());
                    txtProjectName.Text = "Project Name : " + reader["code"].ToString() + "-" + reader["name"].ToString();
                    projectPlanningId = reader["id"].ToString();
                    projectId = reader["project_id"].ToString();
                    using (MySqlDataReader reader0 = DBClass.ExecuteReader("SELECT a.id,a.code,a.name site,a.plot_number,a.address,b.name location FROM tbl_project_sites a,tbl_city b WHERE a.location_id = b.id AND a.id = @id",
                    DBClass.CreateParameter("id", reader["site"].ToString())))
                        if (reader0.Read())
                        {
                            txtProjectLocation.Text = reader0["location"].ToString();
                            txtProjectSite.Text = reader0["site"].ToString();
                            txtPlotNumber.Text = reader0["plot_number"].ToString();
                        }
                    dtp_start.Value = DateTime.Parse(reader["start_date"].ToString());
                    dtp_end.Value = DateTime.Parse(reader["end_date"].ToString());
                    cmbProjectStatus.Text = reader["status"].ToString();
                    txtEstimatedBudget.Text = Utilities.FormatDecimal(decimal.Parse(reader["estimated_budget"].ToString()));
                    cmbProjectType.Text = reader["project_type"].ToString();
                    cmbFundAccount.SelectedValue = int.Parse(reader["fund_account_id"].ToString());
                    using (MySqlDataReader reader00 = DBClass.ExecuteReader(@"SELECT name FROM tbl_project_resource  
                        WHERE EXISTS(SELECT 1 FROM tbl_project_planning p 
                        WHERE p.id = @planId 
                        AND FIND_IN_SET(tbl_project_resource.id, assigned_team) > 0) AND employee_id > 0;",
                        DBClass.CreateParameter("planId", projectPlanningId.ToString())))
                        while (reader00.Read())
                        {
                            txtAssignedTeam.Text = string.IsNullOrEmpty(txtAssignedTeam.Text) ? reader00["name"].ToString() : (txtAssignedTeam.Text + ", "+ reader00["name"].ToString());
                        }
                    txtProgress.Text = reader["progress"].ToString();
                    int refId = int.Parse(reader["tender_id"].ToString());
                    txtTenderName.Text = reader["tender_name"].ToString();
                    tenderId = reader["tender_id"].ToString();
                    //cmbFundPeriod.Text = reader["fund_period"].ToString();
                    //txt_desc.Text = reader["description"].ToString();
                    using (MySqlDataReader reader1 = DBClass.ExecuteReader("select * FROM tbl_project_management WHERE id = @id",
                    DBClass.CreateParameter("id", id)))
                        if (reader1.Read())
                        {
                            txtBudget.Text = reader1["budget"].ToString();
                            txtActualCost.Text = reader1["actual_cost"].ToString();
                            txtRemainingBudget.Text = reader1["remaining_budget"].ToString();
                            dtDate.Value = DateTime.Parse(reader1["date"].ToString());
                            loadActivity();
                            loadAssignDetails();
                        }
                        else
                        {
                            if (int.Parse(projectPlanningId) > 0)
                            {
                                loadActivity();
                                loadAssignDetails();
                            }
                        }
                }
            }
        }

        private void loadActivity()
        {
            int count = 1;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                                                                        CONCAT(a.code, '-', b.NAME) AS description,
                                                                        b.qty AS QtyNeeded,
                                                                        IFNULL(c.ReceivedQty, 0) AS QtyUsed,
                                                                        CASE 
                                                                            WHEN IFNULL(c.ReceivedQty, 0) > 0 THEN 'Received' 
                                                                            WHEN IFNULL(c.IssuedQty, 0) > 0 THEN 'Issued' 
                                                                            ELSE 'Requested' 
                                                                        END AS status
                                                                    FROM tbl_project_activity a
                                                                    LEFT JOIN tbl_items_boq b ON b.id = a.code
                                                                    LEFT JOIN tbl_project_material_requests c 
                                                                        ON c.itemId = a.code AND a.planning_id = c.planning_id
                                                                    WHERE a.planning_id = @planningId; ",
                DBClass.CreateParameter("planningId", projectPlanningId)))
            {
                while (reader.Read())
                {
                    dgvQuantitySurvey.Rows.Add(count, reader["description"].ToString(), reader["QtyNeeded"].ToString(), reader["QtyUsed"].ToString(), reader["status"].ToString());
                    //dgvQuantitySurvey.Rows.Add(count, "Concrete", "500 m³", "200 m³", "In Progress");
                    count++;
                    //dgvQuantitySurvey.Rows.Add(count,"Steel", "100 tons", "50 tons", "Pending");
                }
            }
        }
        private void loadAssignDetails()
        {
            int count = 1;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                                                                        b.name,
                                                                        b.date,
                                                                        COUNT(a.resource_id) task,
                                                                        c.status
                                                                    FROM tbl_project_activity_assignment a
                                                                    JOIN tbl_project_activity c ON a.activity_id = c.id
                                                                    LEFT JOIN tbl_project_resource b ON a.resource_id = b.id
                                                                    WHERE c.planning_id = @planningId GROUP BY b.name,b.date,c.status;",
                                            DBClass.CreateParameter("planningId", projectPlanningId)))
            {
                while (reader.Read())
                {
                    string dated = DateTime.Now.Date.ToString();
                    if (reader["date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["date"]);
                        dated = date.ToString("dd/MM/yyyy");
                    }

                    dgvMilestones.Rows.Add(count, reader["name"].ToString(), dated, reader["status"].ToString());
                    count++;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertData())
                    this.Close();
            }
            else
            {
                if (updateData())
                    this.Close();
            }
            master.BindData();
        }
        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_management 
                                    SET date=@date,project_planning_id=@projectPlanningId,project_id=@projectId, budget=@budget, actual_cost=@actualCost, remaining_budget=@remainingBudget,modified_by=@modifiedBy,modified_date=@modifiedDate WHERE id = @id;",
            DBClass.CreateParameter("id", id),
            DBClass.CreateParameter("date", dtDate.Value.Date),
            DBClass.CreateParameter("projectPlanningId", projectPlanningId),
            DBClass.CreateParameter("projectId", projectId),
            DBClass.CreateParameter("budget", string.IsNullOrEmpty(txtBudget.Text) ? 0 : Convert.ToDouble(txtBudget.Text)),
            DBClass.CreateParameter("actualCost", string.IsNullOrEmpty(txtActualCost.Text) ? 0 : Convert.ToDouble(txtActualCost.Text)),
            DBClass.CreateParameter("remainingBudget", string.IsNullOrEmpty(txtRemainingBudget.Text) ? 0 : Convert.ToDouble(txtRemainingBudget.Text)),
            DBClass.CreateParameter("modifiedBy", frmLogin.userId.ToString()),
            DBClass.CreateParameter("modifiedDate", DateTime.Now.Date));

            //DBClass.ExecuteNonQuery(@"DELETE FROM tbl_project_management_quantity_survey WHERE ref_id=@id", DBClass.CreateParameter("id", id.ToString()));
            //DBClass.ExecuteNonQuery(@"DELETE FROM tbl_project_management_milestones WHERE ref_id=@id", DBClass.CreateParameter("id", id.ToString()));

            //insertGridItems(id);
            Utilities.LogAudit(frmLogin.userId, "Update Project Management", "Project Management", id, "Updated Project Management: " + txtProjectName.Text);
            updateProgress();

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            int _id = int.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_project_management(date,project_planning_id,project_id, budget, actual_cost, remaining_budget,created_by,created_date,state) VALUES (@date,@projectPlanningId,@projectId, @budget, @actualCost, @remainingBudget,@createdBy,@createdDate,0);
                            SELECT LAST_INSERT_ID();",
            DBClass.CreateParameter("date", dtDate.Value.Date),
            DBClass.CreateParameter("projectPlanningId", projectPlanningId),
            DBClass.CreateParameter("projectId", projectId),
            DBClass.CreateParameter("budget", string.IsNullOrEmpty(txtBudget.Text) ? 0 : Convert.ToDouble(txtBudget.Text)),
            DBClass.CreateParameter("actualCost", string.IsNullOrEmpty(txtActualCost.Text) ? 0 : Convert.ToDouble(txtActualCost.Text)),
            DBClass.CreateParameter("remainingBudget", string.IsNullOrEmpty(txtRemainingBudget.Text) ? 0 : Convert.ToDouble(txtRemainingBudget.Text)),
            DBClass.CreateParameter("createdBy", frmLogin.userId.ToString()),
            DBClass.CreateParameter("createdDate", DateTime.Now.Date)).ToString());

            //insertGridItems(_id);
            updateProgress();

            Utilities.LogAudit(frmLogin.userId, "Insert Project Management", "Project Management", _id, "Inserted Project Management: " + txtProjectName.Text);

            return true;
        }
        private void updateProgress()
        {
            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_planning 
                                    SET progress=@progress WHERE id = @id;",
            DBClass.CreateParameter("id", projectPlanningId),
            DBClass.CreateParameter("progress", string.IsNullOrEmpty(txtProgress.Text) ? 0 : Convert.ToDouble(txtProgress.Text)));
            Utilities.LogAudit(frmLogin.userId, "Update Project Progress", "Project Planning", int.Parse(projectPlanningId), "Updated Project Progress: " + txtProgress.Text);
        }
        private void insertGridItemsOldCode(int invId)
        {

            for (int i = 0; i < dgvQuantitySurvey.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_management_quantity_survey(material,qty_needed,qty_used,status,ref_id)
                                            VALUES(@material,@qtyNeeded,@qtyUsed, @status,@refId)",
                      DBClass.CreateParameter("@material", dgvQuantitySurvey.Rows[i].Cells["Material"].Value.ToString()),
                      DBClass.CreateParameter("@refId", invId.ToString()),
                      DBClass.CreateParameter("@status", dgvQuantitySurvey.Rows[i].Cells["Status"].Value.ToString()),
                      DBClass.CreateParameter("@qtyNeeded", dgvQuantitySurvey.Rows[i].Cells["QtyNeeded"].Value == DBNull.Value ? "0" : dgvQuantitySurvey.Rows[i].Cells["QtyNeeded"].Value.ToString()),
                      DBClass.CreateParameter("@qtyUsed", dgvQuantitySurvey.Rows[i].Cells["QtyUsed"].Value == DBNull.Value ? "0" : dgvQuantitySurvey.Rows[i].Cells["QtyUsed"].Value.ToString()));
            }
            for (int i = 0; i < dgvMilestones.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_management_milestones(milestone,date, status, ref_id)
                                         VALUES (@milestone,@date, @status, @refId);",
                  DBClass.CreateParameter("@milestone", dgvMilestones.Rows[i].Cells["Milestone"].Value.ToString()),
                  DBClass.CreateParameter("@date", DateTime.Parse(dgvMilestones.Rows[i].Cells["m_date"].Value.ToString())),
                  DBClass.CreateParameter("@status", dgvMilestones.Rows[i].Cells["m_status"].Value.ToString()),
                  DBClass.CreateParameter("@refId", invId.ToString()));
            }
        }
        private bool chkRequiredDate()
        {
            if (txtBudget.Text == "" || decimal.Parse(txtBudget.Text) == 0)
            {
                MessageBox.Show("Budget Must Be Bigger Than Zero");
                return false;
            }
            if (txtActualCost.Text == "" || decimal.Parse(txtBudget.Text) == 0)
            {
                txtActualCost.Text = "0";
            }
            if (txtRemainingBudget.Text == "" || decimal.Parse(txtRemainingBudget.Text) == 0)
            {
                txtRemainingBudget.Text = "0";
            }
            dgvMilestones.AllowUserToAddRows = false;
            dgvQuantitySurvey.AllowUserToAddRows = false;
            if (dgvMilestones.Rows.Count > 0) {
                for (int i = 0; i < dgvMilestones.Rows.Count - 1; i++)
                {
                    if (dgvMilestones.Rows[i].Cells["m_date"].Value == null
                        || dgvMilestones.Rows[i].Cells["m_date"].Value.ToString() == "")
                    {
                        DateTime parsedDate;
                        if (!DateTime.TryParse(dgvMilestones.Rows[i].Cells["m_date"].Value.ToString(), out parsedDate))
                        {
                            MessageBox.Show("Invalid date format. Please enter a valid date.");
                            return false;
                        }

                    }
                }
            }
            return true;
        }
        private void resetTextBox()
        {

            txtAssignedTeam.Text = txtEstimatedBudget.Text = txtProgress.Text = txtPlotNumber.Text = txtProjectLocation.Text = txtProjectName.Text = txtProjectSite.Text = "";
            id = 0;
            dtProject.Value = dtp_end.Value = dtp_start.Value = DateTime.Now;
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
            this.Close();
        }

        private void txtEstimatedBudget_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '.' && !txtEstimatedBudget.Text.Contains("."))
            {
                return;
            }
            e.Handled = true;
        }

        private void cmbProjectStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvMilestones_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvMilestones.Columns["m_date"].Index)
            {
                DateTime currentDate = DateTime.Now;
                dgvMilestones.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = currentDate;
                dgvMilestones.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = currentDate.ToString("dd/MM/yyyy");
            } else if (dgvMilestones.Rows.Count > 1 && dgvMilestones.CurrentRow.Cells["m_delete"].ColumnIndex == e.ColumnIndex)
            {
                dgvMilestones.Rows.Remove(dgvMilestones.CurrentRow);
            }
        }

        private void txtBudget_TextChanged(object sender, EventArgs e)
        {
            calculateBudget();
        }
        private void calculateBudget()
        {
            decimal budget = txtBudget.Text.ToString() == "" ? 0 : decimal.Parse(txtBudget.Text.ToString());
            decimal actualCost = txtActualCost.Text.ToString() == "" ? 0 : decimal.Parse(txtActualCost.Text.ToString());
            decimal estBudget = txtEstimatedBudget.Text.ToString() == "" ? 0 : decimal.Parse(txtEstimatedBudget.Text.ToString());

            txtRemainingBudget.Text = (budget - actualCost).ToString("N2");
        }

        private void txtActualCost_TextChanged(object sender, EventArgs e)
        {
            calculateBudget();
        }

        private void dgvQuantitySurvey_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvQuantitySurvey.Rows.Count > 1 && dgvQuantitySurvey.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            {
                dgvQuantitySurvey.Rows.Remove(dgvQuantitySurvey.CurrentRow);
            }
        }

        private void dgvMilestones_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            object cellValue = dgvMilestones.Rows[e.RowIndex].Cells["m_date"].Value;
            DateTime parsedDate;
            if (cellValue != null && !DateTime.TryParse(cellValue.ToString(),out parsedDate))
            {
                MessageBox.Show("Invalid date format. Please enter a valid date.");
                //e.Cancel = true;
                //dgvMilestones.Rows[e.RowIndex].Cells["m_date"].Selected = true;
                //dgvMilestones.CurrentCell = dgvMilestones.Rows[e.RowIndex].Cells["m_date"];
                // Optionally, start editing immediately
                //dgvMilestones.BeginEdit(true);
            }
        }

        private void dgvMilestones_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvMilestones.Columns["Milestone"].Index)
            {
                dgvMilestones.Rows[e.RowIndex].Cells["m_date"].Value = DateTime.Now.ToString("dd/MM/yyyy");
            }
            else if (e.ColumnIndex == dgvMilestones.Columns["m_date"].Index)
            {
                object cellValue = dgvMilestones.Rows[e.RowIndex].Cells["m_date"].Value;
                DateTime parsedDate;
                if (cellValue != null && !DateTime.TryParse(cellValue.ToString(), out parsedDate))
                {
                    MessageBox.Show("Invalid date format. Please enter a valid date.");
                    //e.Cancel = true;
                    //dgvMilestones.Rows[e.RowIndex].Cells["m_date"].Selected = true;
                    dgvMilestones.CurrentCell = dgvMilestones.Rows[e.RowIndex].Cells["m_date"];
                    //Optionally, start editing immediately
                    dgvMilestones.BeginEdit(true);
                }
            }
        }

        private void txtNumberBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '.' && !txtEstimatedBudget.Text.Contains("."))
            {
                return;
            }
            e.Handled = true;
        }
    }
}
