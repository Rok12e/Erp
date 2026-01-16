using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmCustomizeList : Form
    {
        public frmCustomizeList()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            LoadTools();
        }
        private void frmCustomizeList_Load(object sender, EventArgs e)
        {
            headerUC1.FormText = "Customize Your List Of Tools";
        }
        private void LoadTools()
        {
            try
            {
                string queryAvailable = "SELECT id, tool_name FROM tbl_tools WHERE is_selected = 0";
                dgvItems.DataSource = DBClass.ExecuteDataTable(queryAvailable);

                string queryChosen = "SELECT id, tool_name FROM tbl_tools WHERE is_selected = 1";
                dgvItems2.DataSource = DBClass.ExecuteDataTable(queryChosen);

                if (dgvItems.Columns.Contains("id")) dgvItems.Columns["id"].Visible = false;
                if (dgvItems2.Columns.Contains("id")) dgvItems2.Columns["id"].Visible = false;
                dgvItems.ColumnHeadersVisible = false;
                dgvItems2.ColumnHeadersVisible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tools: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgvItems2.Rows)
                {
                    int toolId = Convert.ToInt32(row.Cells["id"].Value);
                    string query = "UPDATE tbl_tools SET is_selected = 1 WHERE id = @id";
                    DBClass.ExecuteNonQuery(query, DBClass.CreateParameter("@id", toolId));
                    Utilities.LogAudit(frmLogin.userId, "Add Tool", "Tools", toolId, "Added Tool: " + row.Cells["tool_name"].Value);
                }

                MessageBox.Show("Tools saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving tools: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                int selectedId = Convert.ToInt32(dgvItems.SelectedRows[0].Cells["id"].Value);
                string query = "UPDATE tbl_tools SET is_selected = 1 WHERE id = @id";
                DBClass.ExecuteNonQuery(query, DBClass.CreateParameter("@id", selectedId));
                Utilities.LogAudit(frmLogin.userId, "Add Tool", "Tools", selectedId, "Added Tool: " + dgvItems.SelectedRows[0].Cells["tool_name"].Value);

                LoadTools(); 
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (dgvItems2.SelectedRows.Count > 0)
            {
                int selectedId = Convert.ToInt32(dgvItems2.SelectedRows[0].Cells["id"].Value);
                string query = "UPDATE tbl_tools SET is_selected = 0 WHERE id = @id";
                DBClass.ExecuteNonQuery(query, DBClass.CreateParameter("@id", selectedId));
                Utilities.LogAudit(frmLogin.userId, "Remove Tool", "Tools", selectedId, "Removed Tool: " + dgvItems2.SelectedRows[0].Cells["tool_name"].Value);
                LoadTools();
            }
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();

        }
    }
}
