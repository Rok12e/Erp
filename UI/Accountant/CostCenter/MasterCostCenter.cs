using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using YamyProject.Localization;
namespace YamyProject
{
    public partial class MasterCostCenter : Form
    {
        EventHandler lvl1, lvl2;
        public MasterCostCenter()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            AddDGVColumns();
            lvl1 = lvl2 = (sender, args) => BindRefresh();
            EventHub.lvl1Account += lvl1;
            EventHub.lvl2Account += lvl2;
            headerUC1.FormText = this.Text;
        }
        private void MasterCostCenter_Load(object sender, EventArgs e)
        {
            BindDGV();
        }
        private void BindRefresh()
        {
            //dgvCustomer.Rows.Clear();
            dgvCustomer.Columns.Clear();
            AddDGVColumns();
            BindDGV();
        }
        private void AddDGVColumns()
        {
            dgvCustomer.Columns.Add("state", "");
            dgvCustomer.Columns.Add("loadState", "");
            dgvCustomer.Columns.Add("currLvl", "");
            dgvCustomer.Columns.Add("lvl1", "Name");
            dgvCustomer.Columns.Add("id", "");
            dgvCustomer.Columns.Add("Add", "");
            dgvCustomer.Columns.Add("Edit", "");
            dgvCustomer.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["state"].Visible = dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["loadState"].Visible = dgvCustomer.Columns["currLvl"].Visible = false;
            dgvCustomer.Columns["Add"].Width = dgvCustomer.Columns["Edit"].Width = 70; 
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        public void BindDGV()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_cost_center"))
            {
                while (reader.Read())
                    dgvCustomer.Rows.Add("e", "u", "1", "► " + reader["name"].ToString(), reader["id"].ToString());
            }
        }

        private void dgvCustomer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvCustomer.Columns[e.ColumnIndex].Name == "Add" || dgvCustomer.Columns[e.ColumnIndex].Name == "Edit")
            {
                e.CellStyle.ForeColor = Color.Blue;
                e.CellStyle.Font = new Font(dgvCustomer.DefaultCellStyle.Font, FontStyle.Underline);
            }
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int lvl;
            bool success = int.TryParse(dgvCustomer.Rows[e.RowIndex].Cells["currLvl"].Value.ToString(), out lvl);

            int currLvl = success ? lvl : 0;

            if (currLvl >= 2)
            {
                int id;
                if (int.TryParse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString(), out id))
                {
                    new frmEditSub(id).ShowDialog();
                }
                else
                {
                    MessageBox.Show("Invalid ID format.");
                }
                return;
            }
            string columnName = dgvCustomer.Columns[e.ColumnIndex].Name;

            string state = dgvCustomer.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6);

            if (dgvCustomer.Columns["lvl1"].Index == e.ColumnIndex)
            {
                if (state == "e")
                {
                    if (loadState == "u")
                    {
                        int parentId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString());
                        using (MySqlDataReader reader = DBClass.ExecuteReader($@"select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_sub_cost_center WHERE main_id = @parentId ",
                                                                DBClass.CreateParameter("parentId", parentId)))
                        {
                            int insertIndex = e.RowIndex + 1;
                            while (reader.Read())
                            {
                                string nextState = (currLvl + 1 == 4) ? "n" : "e";
                                string nextIcon = (currLvl + 1 == 4) ? "  " : "►";

                                dgvCustomer.Rows.Insert(insertIndex, nextState, "u", currLvl + 1, space + "  " + reader["name"].ToString(), reader["id"].ToString());
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

                    if (columnName == "Add")
                    {
                        if (dgvCustomer.SelectedRows.Count == 0)
                        {
                            string idString = dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString();
                            int id;
                            if (!int.TryParse(idString, out id)) 
                            {
                                MessageBox.Show("Invalid ID format.");
                                return;
                            }
                            new frmCostCenter(id).ShowDialog();
                        }
                    }

                }
            }
        }
        private void dgvCustomer_MouseMove(object sender, MouseEventArgs e)
        {
            var hitTestInfo = dgvCustomer.HitTest(e.X, e.Y);

            if (hitTestInfo.RowIndex >= 0 && (dgvCustomer.Columns[hitTestInfo.ColumnIndex].Name == "Add" || dgvCustomer.Columns[hitTestInfo.ColumnIndex].Name == "Edit"))
                this.Cursor = Cursors.Hand; 
            else
                this.Cursor = Cursors.Default; 
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            new frmCostCenter(0).ShowDialog();
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MasterCostCenter_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
