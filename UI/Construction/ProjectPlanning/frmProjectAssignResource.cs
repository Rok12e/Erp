using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmProjectAssignResource : Form
    {
        decimal invId;
        private frmViewProjectPlanning master;
        int id, planningId, tenderId;
        List<string> assignedTeam = new List<string>();

        public frmProjectAssignResource(frmViewProjectPlanning _master, int _id, int _tenderId, int _planningId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = _id;
            this.planningId = _planningId;
            this.tenderId = _tenderId;
            if (id != 0)
                this.Text = "Assign Resource - Edit";
            else
                this.Text = "Assign Resource - New";
            headerUC1.FormText = this.Text;
        }
        private void frmProjectAssignResource_Load(object sender, EventArgs e)
        {
            LoaddgvItems();
            if (id != 0)
                BindData();
        }
        public void LoaddgvItems()
        {
            dgvResource.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT pr.id, pr.code, date, pr.name,r.name as roleName, phone, type, price_unit, unit_time, max_unit_time FROM tbl_project_resource pr, tbl_project_role r where r.id = pr.role"))
            {
                int count = 1;
                while (reader.Read())
                {
                    dgvResource.Rows.Add(count, reader["id"], reader["code"], reader["name"], reader["type"], reader["roleName"], reader["unit_time"]);
                    count++;
                }
            }
        }

        private void BindData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT id FROM tbl_project_resource WHERE EXISTS(SELECT 1 FROM tbl_project_planning p WHERE p.id = @plannigId AND FIND_IN_SET(tbl_project_resource.id, assigned_team) > 0);",
                DBClass.CreateParameter("plannigId", planningId)))
            {
                while (reader.Read())
                {
                    foreach(DataGridViewRow dr in dgvResource.Rows)
                    {
                        if (dr.Cells["ResourceId"].Value.ToString() == reader["id"].ToString())
                        {
                            dr.Cells["select"].Value = true;
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object senderc, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (updateData())
            {
                this.Close();
                master.BindResourceDate();
            }
        }

        private void dgvResource_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvResource.Rows.Count == 0)
                return;

            if (id > 0 && tenderId > 0 && int.Parse(dgvResource.CurrentRow.Cells["ResourceId"].Value.ToString()) > 0)
            {
                new frmProjectResource(this, int.Parse(dgvResource.CurrentRow.Cells["ResourceId"].Value.ToString()), tenderId, planningId).ShowDialog();
            }
        }

        private bool updateData()
        {
            string assignedTeam = "";
            foreach (DataGridViewRow row in dgvResource.Rows)
            {
                if (Convert.ToBoolean(row.Cells["select"].Value))
                {
                    assignedTeam += string.IsNullOrEmpty(assignedTeam) ? row.Cells["ResourceId"].Value.ToString() : ("," + row.Cells["ResourceId"].Value.ToString());
                }
            }

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_planning SET assigned_team = @assignedTeam WHERE id = @id; ",
            DBClass.CreateParameter("id", planningId),

            DBClass.CreateParameter("assignedTeam", assignedTeam));
            Utilities.LogAudit(frmLogin.userId, (id == 0 ? "Add Project Resource" : "Update Project Resource"), "Project Resource", id, (id == 0 ? "Added Project Resource: " : "Updated Project Resource: ") + assignedTeam);
            return true;
        }

        private void btnAddResource_Click(object sender, EventArgs e)
        {
            new frmProjectResource(this, 0, 0, 0).ShowDialog();
        }
    }
}

