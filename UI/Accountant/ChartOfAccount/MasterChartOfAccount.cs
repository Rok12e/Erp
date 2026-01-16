using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterChartOfAccount : Form
    {
        EventHandler lvl1, lvl2, lvl3, lvl4;
        public MasterChartOfAccount()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            lvl1 = lvl2 = lvl3 = lvl4 = (sender, args) => BindDGV();
            EventHub.lvl1Account += lvl1;
            EventHub.lvl2Account += lvl2;
            EventHub.lvl3Account += lvl3;
            EventHub.lvl4Account += lvl4;
            headerUC1.FormText = this.Text;
        }
        private void MasterChartOfAccount_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.lvl1Account -= lvl1;
            EventHub.lvl2Account -= lvl2;
            EventHub.lvl3Account -= lvl3;
            EventHub.lvl4Account -= lvl4;
        }
        private void MasterChartOfAccount_Load(object sender, EventArgs e)
        {
            AddDGVColumns();
            BindDGV();
        }

        private void AddDGVColumns()
        {
            dgvCustomer.Columns.Add("state", "");
            dgvCustomer.Columns.Add("loadState", "");
            dgvCustomer.Columns.Add("currLvl", "");
            dgvCustomer.Columns.Add("lvl1", "Account Name");
            dgvCustomer.Columns.Add("id", "");
            dgvCustomer.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["state"].Visible = dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["loadState"].Visible = dgvCustomer.Columns["currLvl"].Visible = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        public void BindDGV()
        {
            dgvCustomer.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT concat (code , ' - ',name) as name , id FROM tbl_coa_level_1"))
            {
                while (reader.Read())
                    dgvCustomer.Rows.Add("e", "u", "1", "   ►     " + reader["name"].ToString(), reader["id"].ToString());
            }
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int currLvl = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            if (currLvl >= 4) {
                new frmEditLevel4(int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString())).ShowDialog();
                return;
            }

            string state = dgvCustomer.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);


            if (state == "e")
            {
                if (loadState == "u")
                {
                    int parentId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    using (MySqlDataReader reader = DBClass.ExecuteReader($@"SELECT concat(code,' - ',name) as name , id FROM tbl_coa_level_" + (currLvl + 1) + " WHERE main_id = @parentId ORDER BY code",
                                                             DBClass.CreateParameter("parentId", parentId)))
                    {
                        int insertIndex = e.RowIndex + 1;
                        while (reader.Read())
                        {
                            string nextState = (currLvl + 1 == 4) ? "n" : "e";
                            string nextIcon = (currLvl + 1 == 4) ? "  " : "►";

                            dgvCustomer.Rows.Insert(insertIndex, nextState, "u", currLvl + 1, space + nextIcon + "     " + reader["name"].ToString(), reader["id"].ToString());
                            insertIndex++;
                        }
                    }
                    dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                }
                else
                    GetAllChildren(e.RowIndex);

                dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("►", "▼");
                dgvCustomer.Rows[e.RowIndex].Cells["state"].Value = "c";
            }
            else if (state == "c")
            {
                ToggleChildRowsVisibility(e.RowIndex, currLvl, false);

                dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("▼", "►");
                dgvCustomer.Rows[e.RowIndex].Cells["state"].Value = "e";
            }

        }

        private void ToggleChildRowsVisibility(int parentRowIndex, int parentLevel, bool visible)
        {
            for (int i = parentRowIndex + 1; i < dgvCustomer.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvCustomer.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (visible)
                    {
                        dgvCustomer.Rows[i].Visible = true;

                        if (dgvCustomer.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(dgvCustomer.Rows[i].Index);
                    }
                    else
                        dgvCustomer.Rows[i].Visible = false;
                }

                else
                    break;
            }
        }

        private void GetAllChildren(int parentRowIndex)
        {
            int parentLevel = int.Parse(dgvCustomer.Rows[parentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = parentRowIndex + 1; i < dgvCustomer.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvCustomer.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (rowLevel == parentLevel + 1)
                    {
                        dgvCustomer.Rows[i].Visible = true;
                        if (dgvCustomer.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(i);
                    }
                    else
                        continue;
                }
                else
                    break;
            }

        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnImport_Export_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmChartOfAccountList(""));
        }

        private DataGridViewRow GetParentRow(int currentRowIndex)
        {
            if (currentRowIndex <= 0) return null;

            int currentLevel = int.Parse(dgvCustomer.Rows[currentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = currentRowIndex - 1; i >= 0; i--)
            {
                int rowLevel = int.Parse(dgvCustomer.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel == currentLevel - 1)
                    return dgvCustomer.Rows[i];
            }

            return null;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmAddAccount().ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (UserPermissions.canEdit("Chart Of Account"))
            {
                if (int.Parse(dgvCustomer.SelectedRows[0].Cells["currLvl"].Value.ToString()) != 4)
                {
                    MessageBox.Show("Select Level 4 First");
                    return;
                }
                new frmEditLevel4(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
            }
        }
    }
}
