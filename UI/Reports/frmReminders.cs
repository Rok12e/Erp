using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject.UI.Reports
{
    public partial class frmReminders : Form
    {
        public frmReminders()
        {
            InitializeComponent();
        }

        private void frmReminders_Load(object sender, EventArgs e)
        {
            headerUC1.FormText = "Reminders";
            AddDGVColumns();
            BindDGV();
        }

        private void AddDGVColumns()
        {
            dgvData.Columns.Add("state", "");
            dgvData.Columns.Add("loadState", "");
            dgvData.Columns.Add("currLvl", "");
            dgvData.Columns.Add("lvl1", "Account Name");
            dgvData.Columns.Add("id", "");
            dgvData.Columns.Add("currname", "");
            dgvData.Columns.Add("balance", "Amount");
            LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            //dgvData.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvData.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            dgvData.Columns["state"].Visible = dgvData.Columns["currname"].Visible = dgvData.Columns["id"].Visible = dgvData.Columns["loadState"].Visible = dgvData.Columns["currLvl"].Visible = false;

            dgvData.Columns["balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public void BindDGV()
        {
            dgvData.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                    -- 1. MONEY TO DEPOSIT
                    SELECT 
                        CONCAT('MONEY TO DEPOSIT (', COUNT(*), ')') AS NAME,
                        1 AS id,
                        ROUND(SUM(pay), 2) AS balance
                    FROM tbl_sales
                    WHERE pay > 0 AND state = 0

                    UNION ALL

                    -- 2. OVERDUE INVOICES
                    SELECT 
                        CONCAT('OVERDUE INVOICES (', COUNT(*), ')') AS NAME,
                        2 AS id,
                        ROUND(SUM(`change`), 2) AS balance
                    FROM tbl_sales
                    WHERE `change` > 0 AND DATEDIFF(CURDATE(), date) > 0 AND state = 0

                    UNION ALL

                    -- 3. CHECKS TO PRINT
                    SELECT 
                        CONCAT('CHECKS TO PRINT (', COUNT(*), ')') AS NAME,
                        3 AS id,
                        ROUND(SUM(amount), 2) AS balance
                    FROM tbl_check_details
                    WHERE state = 'New'

                    UNION ALL

                    -- 4. INVOICES/CREDIT MEMOS TO PRINT
                    SELECT 
                        CONCAT('INVOICES/CREDIT MEMOS TO PRINT (', COUNT(*), ')') AS NAME,
                        4 AS id,
                        ROUND(SUM(net), 2) AS balance
                    FROM tbl_sales
                    WHERE state = 0;
                "))
            {
                while (reader.Read())
                    dgvData.Rows.Add("e", "u", "1", "   ►     " + reader["name"].ToString(),
                        reader["id"].ToString(), reader["name"].ToString(),
                        reader["balance"].ToString());
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int currLvl = int.Parse(dgvData.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            if (currLvl > 2) return;
            if (currLvl == 2)
            {
                frmLogin.frmMain.openChildForm(new MasterTransactionByAccount(int.Parse(dgvData.Rows[e.RowIndex].Cells["id"].Value.ToString())));
            }

            string state = dgvData.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvData.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvData.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);

            try
            {
                if (state == "e")
                {
                    if (loadState == "u")
                    {
                        int parentId = int.Parse(dgvData.Rows[e.RowIndex].Cells["id"].Value.ToString());
                        string parentName = dgvData.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
                        List<DataGridViewRow> newRows = new List<DataGridViewRow>();

                        string query = "";
                        if (parentName.Contains("MONEY TO DEPOSIT")) {
                            query = @"SELECT 
                                            CONCAT(DATE, '       ', id,' - ', bill_to) AS NAME,
                                            id,
                                            pay AS balance
                                        FROM tbl_sales
                                        WHERE pay > 0 AND state = 0";
                        }
                        else if (parentName.Contains("OVERDUE INVOICES"))
                        {
                            query = @"SELECT 
                                            CONCAT(DATE, '       ', id,' - ', bill_to) AS NAME,
                                            id,
                                            `change` AS balance
                                        FROM tbl_sales
                                        WHERE `change` > 0 AND DATEDIFF(CURDATE(), date) > 0 AND state = 0";
                        }
                        else if (parentName.Contains("CHECKS TO PRINT"))
                        {
                            query = @"SELECT 
                                            CONCAT(DATE, '       ', id,' - ',check_name) AS NAME,
                                            id,
                                            amount AS balance
                                        FROM tbl_check_details
                                        WHERE state = 'New'";
                        }
                        else if (parentName.Contains("INVOICES/CREDIT MEMOS TO PRINT"))
                        {
                            query = @"SELECT 
                                            CONCAT(DATE, '       ', id,' - ', bill_to) AS NAME,
                                            id,
                                            net AS balance
                                        FROM tbl_sales
                                        WHERE state = 0";
                        }

                        using (MySqlDataReader reader = DBClass.ExecuteReader(query))
                        {
                            int insertIndex = e.RowIndex + 1;

                            while (reader.Read())
                            {
                                string nextState = (currLvl + 1 == 2) ? "n" : "e";
                                string nextIcon = (currLvl + 1 == 2) ? "  " : "►";
                                var balanceData = reader["balance"].ToString();
                                if (balanceData != null)
                                {
                                    var newRow = new DataGridViewRow();
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = nextState });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = "u" });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = currLvl + 1 });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["name"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["id"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["name"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = balanceData });
                                    newRow.Height = dgvData.Rows[e.RowIndex].Height;

                                    ApplyRowStyle(newRow, currLvl + 1);
                                    newRows.Add(newRow);
                                }
                            }

                            // Add all new rows in one go for better performance
                            foreach (var row in newRows)
                            {
                                dgvData.Rows.Insert(insertIndex, row);
                                insertIndex++;
                            }
                        }

                        dgvData.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                    }
                    else
                    {
                        GetAllChildren(e.RowIndex);
                    }

                    dgvData.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("►", "▼");
                    dgvData.Rows[e.RowIndex].Cells["state"].Value = "c";
                }
                else if (state == "c")
                {
                    ToggleChildRowsVisibility(e.RowIndex, currLvl, false);
                    dgvData.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("▼", "►");
                    dgvData.Rows[e.RowIndex].Cells["state"].Value = "e";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyRowStyle(DataGridViewRow row, int level)
        {
            if (level == 2)
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor = Color.WhiteSmoke;
            else if (level == 3)
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor = Color.Gainsboro;
            else
                row.DefaultCellStyle.BackColor = row.DefaultCellStyle.SelectionBackColor = Color.Silver;
        }

        private void ToggleChildRowsVisibility(int parentRowIndex, int parentLevel, bool visible)
        {
            for (int i = parentRowIndex + 1; i < dgvData.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvData.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (visible)
                    {
                        dgvData.Rows[i].Visible = true;

                        if (dgvData.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(dgvData.Rows[i].Index);
                    }
                    else
                        dgvData.Rows[i].Visible = false;
                }

                else
                    break;
            }
        }

        private void GetAllChildren(int parentRowIndex)
        {
            int parentLevel = int.Parse(dgvData.Rows[parentRowIndex].Cells["currLvl"].Value.ToString());

            for (int i = parentRowIndex + 1; i < dgvData.Rows.Count; i++)
            {
                int rowLevel = int.Parse(dgvData.Rows[i].Cells["currLvl"].Value.ToString());

                if (rowLevel > parentLevel)
                {
                    if (rowLevel == parentLevel + 1)
                    {
                        dgvData.Rows[i].Visible = true;
                        if (dgvData.Rows[i].Cells["state"].Value.ToString() == "c")
                            GetAllChildren(i);
                    }
                    else
                        continue;
                }
                else
                    break;
            }

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
