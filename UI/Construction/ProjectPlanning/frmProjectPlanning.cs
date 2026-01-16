using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmProjectPlanning : Form
    {
        decimal invId;
        private frmProjectPlanning master;
        int id, tenderId;
        List<string> assignedTeam = new List<string>();
        bool isExported = false;
        public frmProjectPlanning(frmProjectPlanning _master, int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = id;
            if (id != 0)
                this.Text = "Planning - Edit";
            else
                this.Text = "Planning - New";
        }
        private void TasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabActivity;
        }

        private void ResourcesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabResources;
        }

        private void BudgetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabBudget;
        }

        private void ReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabReports;
        }

        private void newSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new frmSites(this,0).ShowDialog();
        }

        private void cmbTenderName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbTenderName.SelectedValue!=null)
                loadTenderedProjectList();
        }

        private void loadTenderedProjectList()
        {
            int tId = cmbTenderName.SelectedValue == null? 0 : int.Parse(cmbTenderName.SelectedValue.ToString());
            int pId = cmbProjectName.SelectedValue ==null ? 0 : int.Parse(cmbProjectName.SelectedValue.ToString());

            DataTable dt = DBClass.ExecuteDataTable("select id,date,amount from tbl_project_tender WHERE project_id=@pId AND tender_name_id=@tId",
                      DBClass.CreateParameter("tId", tId),
                      DBClass.CreateParameter("pId", pId));
            dgvItems.DataSource = dt;
            dgvItems.Columns["date"].HeaderText = "Date";
            dgvItems.Columns["amount"].HeaderText = "Amount";
            dgvItems.Columns["id"].Visible = false;
            dgvItems.Columns["date"].ReadOnly = true;
            dgvItems.Columns["amount"].ReadOnly = true;
        }

        private void TimelineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabTimeline;
        }

        private void frmProjectPlanning_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateTenderedProjects(cmbProjectName);
            BindCombos.PopulateTenderNames(cmbTenderName);

            BindCombo();
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabProjects;
        }

        private void cmbProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProjectName.SelectedValue != null)
                loadTenderedProjectList();
        }

        private void cmbProjectSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProjectSite.SelectedValue != null)
                loadTenderedProjectList();
        }

        private void btnAddTender_Click(object sender, EventArgs e)
        {
            //new frmSites(this, 0).ShowDialog();
        }

        private void newActivityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
        }

        private void assignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new frmRequestMaterialForm(this, 0).Show();
        }

        private void receivedMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new frmReceivedMaterialForm(this, 0).Show();
        }

        private void issueMaterialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new frmIssueMaterialForm(this, 0).Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //
        }

        public void BindCombo()
        {
            BindCombos.populateSites(cmbProjectSite);
        }

    }
}
