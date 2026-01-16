


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
    public partial class MasterBalanceSheet : Form
    {
        public MasterBalanceSheet()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void MasterBalanceSheet_Load(object sender, EventArgs e)
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
            dgvCustomer.Columns.Add("currname", "");
            dgvCustomer.Columns.Add("balance", "Amount");
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            dgvCustomer.Columns["lvl1"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["state"].Visible = dgvCustomer.Columns["currname"].Visible = dgvCustomer.Columns["id"].Visible = dgvCustomer.Columns["loadState"].Visible = dgvCustomer.Columns["currLvl"].Visible = false;
        }

        public void BindDGV()
        {
            dgvCustomer.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                    WITH AccountBalances AS (
                        -- Calculate balances for INCOME, COST, and EXPENSE
                        SELECT 
                            l1.category_code AS Category, 
                            COALESCE(SUM(t.debit) - SUM(t.credit), 0) AS Balance
                        FROM tbl_coa_level_1 l1
                        LEFT JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
                        LEFT JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
                        LEFT JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
                        LEFT JOIN tbl_transaction t ON t.account_id = l4.id
                        WHERE l1.category_code IN ('INCOME', 'COST', 'EXPENSE') AND t.state = 0
                        GROUP BY l1.category_code
                    ),

                    FinalBalance AS (
                        -- Calculate final balance: Income - Cost - Expenses
                        SELECT 
                            'Final Balance (Income - Cost - Expenses)' AS Category,
                            COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'INCOME'), 0)
                            - COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'COST'), 0)
                            - COALESCE((SELECT Balance FROM AccountBalances WHERE Category = 'EXPENSE'), 0)
                            AS Balance
                    ),

                    EquityBalances AS (
                        -- Calculate balances for ASSET, LIABILITY, and EQUITY
                        SELECT 
                            l1.name, 
                            l1.id, 
                            l1.category_code,
                            COALESCE(SUM(t.debit) - SUM(t.credit), 0) AS balance
                        FROM tbl_coa_level_1 l1
                        LEFT JOIN tbl_coa_level_2 l2 ON l2.main_id = l1.id
                        LEFT JOIN tbl_coa_level_3 l3 ON l3.main_id = l2.id
                        LEFT JOIN tbl_coa_level_4 l4 ON l4.main_id = l3.id
                        LEFT JOIN tbl_transaction t ON t.account_id = l4.id
                        WHERE t.state = 0 AND l1.category_code IN ('ASSET', 'LIABILITY', 'EQUITY')
                        GROUP BY l1.id, l1.name, l1.category_code
                    )

                    -- Final result combining the balances
                    SELECT 
                        eb.name, 
                        eb.id, 
                        CASE 
                            WHEN eb.category_code = 'EQUITY' THEN 
                                eb.balance + (SELECT Balance FROM FinalBalance)
                            ELSE 
                                eb.balance
                        END AS balance
                    FROM EquityBalances eb
                    ORDER BY eb.id;
                "))
            {
                while (reader.Read())
                    dgvCustomer.Rows.Add("e", "u", "1", "   ►     " + reader["name"].ToString(),
                        reader["id"].ToString(), reader["name"].ToString(),
                        reader["balance"].ToString());
            }
        }


        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int currLvl = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["currLvl"].Value.ToString());
            if (currLvl > 4) return;
            if (currLvl==4)
            {
                frmLogin.frmMain.openChildForm(new MasterTransactionByAccount(int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString())));
            }

            string state = dgvCustomer.Rows[e.RowIndex].Cells["state"].Value.ToString();
            string loadState = dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value.ToString();
            string cellValue = dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value.ToString();
            string space = new string(' ', currLvl * 6 + 3);

            try
            {
                if (state == "e")
                {
                    if (loadState == "u")
                    {
                        int parentId = int.Parse(dgvCustomer.Rows[e.RowIndex].Cells["id"].Value.ToString());
                        List<DataGridViewRow> newRows = new List<DataGridViewRow>();

                        using (MySqlDataReader reader = DBClass.ExecuteReader($@"SELECT name as currname, concat(code,' - ',name) as name , id FROM tbl_coa_level_" + (currLvl + 1) + " WHERE main_id = @parentId ORDER BY code",
                                                                          DBClass.CreateParameter("parentId", parentId)))
                        {
                            int insertIndex = e.RowIndex + 1;

                             dgvCustomer.Rows[e.RowIndex].Cells["balance"].Value = "";

                            while (reader.Read())
                            {
                                string nextState = (currLvl + 1 == 4) ? "n" : "e";
                                string nextIcon = (currLvl + 1 == 4) ? "  " : "►";
                                var balanceData = GetBalanceData(currLvl + 1, reader["currname"].ToString());
                                if (balanceData != null)
                                {
                                    var newRow = new DataGridViewRow();
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = nextState });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = "u" });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = currLvl + 1 });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = space + nextIcon + "     " + reader["name"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["id"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["currname"].ToString() });
                                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = balanceData });
                                    newRow.Height = dgvCustomer.Rows[e.RowIndex].Height;

                                    ApplyRowStyle(newRow, currLvl + 1);
                                    newRows.Add(newRow);
                                }
                            }

                            // Add all new rows in one go for better performance
                            foreach (var row in newRows)
                            {
                                dgvCustomer.Rows.Insert(insertIndex, row);
                                insertIndex++;
                            }
                        }

                        dgvCustomer.Rows[e.RowIndex].Cells["loadState"].Value = "l";
                    }
                    else
                    {
                        dgvCustomer.Rows[e.RowIndex].Cells["balance"].Value = "";

                        GetAllChildren(e.RowIndex);
                    }

                    dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("►", "▼");
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Value = "c";
                }
                else if (state == "c")
                {
                    var parentBalanceData = GetBalanceData(currLvl, dgvCustomer.Rows[e.RowIndex].Cells["currname"].Value.ToString().Replace("▼",""));

                    if (!string.IsNullOrEmpty(parentBalanceData))
                    {
                        dgvCustomer.Rows[e.RowIndex].Cells["balance"].Value = parentBalanceData;
                    }
                    ToggleChildRowsVisibility(e.RowIndex, currLvl, false);
                    dgvCustomer.Rows[e.RowIndex].Cells["lvl1"].Value = cellValue.Replace("▼", "►");
                    dgvCustomer.Rows[e.RowIndex].Cells["state"].Value = "e";
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

        private string GetBalanceData(int level, string name)
        {
            using (MySqlDataReader read = DBClass.ExecuteReader($@"SELECT 
                        t1.name AS level1_name, 
                        t2.name AS level2_name, 
                        t3.name AS level3_name, 
                        t4.name AS level4_name, 
                        t1.id AS level1_id, 
                        t2.id AS level2_id, 
                        t3.id AS level3_id, 
                        t4.id AS level4_id,

                        -- Balance for Level 1 (Summing all child transactions)
                        (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                         FROM tbl_transaction tt
                         WHERE  tt.state=0 and tt.account_id IN (
                             SELECT id FROM tbl_coa_level_4 
                             WHERE main_id IN (SELECT id FROM tbl_coa_level_3 
                                              WHERE main_id IN (SELECT id FROM tbl_coa_level_2 
                                                               WHERE main_id = t1.id))
                         )
                        ) AS level1_balance,

                        -- Balance for Level 2 (Summing its child transactions)
                        (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                         FROM tbl_transaction tt
                         WHERE  tt.state=0 and tt.account_id IN (
                             SELECT id FROM tbl_coa_level_4 
                             WHERE main_id IN (SELECT id FROM tbl_coa_level_3 
                                              WHERE main_id = t2.id)
                         )
                        ) AS level2_balance,

                        -- Balance for Level 3 (Summing only Level 4 transactions)
                        (SELECT COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) 
                         FROM tbl_transaction tt
                         WHERE  tt.state=0 and tt.account_id IN (
                             SELECT id FROM tbl_coa_level_4 
                             WHERE main_id = t3.id
                         )
                        ) AS level3_balance,

                        -- Balance for Level 4 (Direct transactions only)
                        COALESCE(SUM(tt.debit) - SUM(tt.credit), 0) AS level4_balance

                    FROM tbl_coa_level_1 t1
                    LEFT JOIN tbl_coa_level_2 t2 ON t2.main_id = t1.id
                    LEFT JOIN tbl_coa_level_3 t3 ON t3.main_id = t2.id
                    LEFT JOIN tbl_coa_level_4 t4 ON t4.main_id = t3.id
                    LEFT JOIN tbl_transaction tt ON tt.account_id = t4.id  

                    WHERE  tt.state=0 and t1.category_code IN ('ASSET', 'LIABILITY', 'EQUITY') AND t{level}.name = @name

                    GROUP BY t1.id, t2.id, t3.id, t4.id;
                    ", DBClass.CreateParameter("name", name)))
            {
                if (read.Read())
                {
                    return read[$"level{level}_balance"].ToString();
                }
            }

            return null;
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            new frmAddAccount().ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(int.Parse(dgvCustomer.SelectedRows[0].Cells["currLvl"].Value.ToString())!=4)
            {
                MessageBox.Show("Select Level 4 First");
                return;
            }
            new frmEditLevel4(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }
    }
}
