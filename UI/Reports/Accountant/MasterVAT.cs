using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;
namespace YamyProject
{
    public partial class MasterVAT : Form
    {
        public MasterVAT()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void MasterVAT_Load(object sender, EventArgs e)
        {
            BindItems();
        }
        public void BindItems()
        {
            try
            {
                string query = "";
                List<MySqlParameter> parameters = new List<MySqlParameter>();

                if (cmbState.Text.Trim() == "All")
                {
                    query = @"SELECT 
                            'Purchase' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_purchase p
                        JOIN tbl_purchase_details s ON p.id = s.purchase_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description
                        UNION ALL
                        SELECT 
                            'Purchase Return' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_purchase_return p
                        JOIN tbl_purchase_return_details s ON p.id = s.purchase_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description
                        UNION ALL
                        SELECT 
                            'Sales' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_sales p
                        JOIN tbl_sales_details s ON p.id = s.sales_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description
                        UNION ALL
                        SELECT 
                            'Sales Return' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_sales_return p
                        JOIN tbl_sales_return_details s ON p.id = s.sales_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description
                        UNION ALL
                        SELECT 
                            'Debit Note' AS `Type`,
                            'VAT 5 %' AS name,
                            ''as description,
                            IFNULL(SUM(s.amount),0) AS `Amount`,
                            IFNULL(SUM(s.vat),0) AS `VAT Amount`
                        FROM tbl_debit_note_details s
                        UNION ALL
                        SELECT 
                            'Credit Note' AS `Type`,
                            'VAT 5 %' as name,
                            '' as description,
                            IFNULL(SUM(s.amount),0) AS `Amount`,
                            IFNULL(SUM(s.vat),0) AS `VAT Amount`
                        FROM tbl_debit_note_details s
                        UNION ALL
                        SELECT 'Petty Cash' AS `Type`, 'VAT 5 %' as name, '' description, 
                                SUM(total_before_vat) AS `Amount`, SUM(total_vat) AS `VAT Amount` FROM tbl_petty_cash_submition s;
                        ";
                }
                else if (cmbState.Text.Trim() == "Sales")
                {
                    query = @"SELECT 
                            'Sales' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_sales p
                        JOIN tbl_sales_details s ON p.id = s.sales_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description;";
                }
                else if (cmbState.Text.Trim() == "Sales Return")
                {
                    query = @"SELECT 
                            'Sales Return' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_sales_return p
                        JOIN tbl_sales_return_details s ON p.id = s.sales_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description;";
                }
                else if (cmbState.Text.Trim() == "Purchase")
                {
                    query = @"SELECT 
                            'Purchase' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_purchase p
                        JOIN tbl_purchase_details s ON p.id = s.purchase_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description;";
                }
                else if (cmbState.Text.Trim() == "Purchase Return")
                {
                    query = @"SELECT 
                            'Purchase Return' AS `Type`,
                            CONCAT(t.name,' ',t.value,' %') name,
                            t.description,
                            IFNULL(SUM(p.total),0) AS `Amount`,
                            IFNULL(SUM(p.vat),0) AS `VAT Amount`
                        FROM tbl_purchase_return p
                        JOIN tbl_purchase_return_details s ON p.id = s.purchase_id
                        JOIN tbl_items i ON i.id = s.item_id
                        JOIN tbl_tax t ON t.id = s.vat
                        GROUP BY t.name,t.value, t.description;";
                }
                else if (cmbState.Text.Trim() == "Debit Note")
                {
                    query = @"SELECT 
                            'Debit Note' AS `Type`,
                            'VAT 5 %' AS name,
                            ''as description,
                            IFNULL(SUM(s.amount),0) AS `Amount`,
                            IFNULL(SUM(s.vat),0) AS `VAT Amount`
                        FROM tbl_debit_note_details s";
                }
                else if (cmbState.Text.Trim() == "Credit Note")
                {
                    query = @"SELECT 
                            'Credit Note' AS `Type`,
                            'VAT 5 %' as name,
                            '' as description,
                            IFNULL(SUM(s.amount),0) AS `Amount`,
                            IFNULL(SUM(s.vat),0) AS `VAT Amount`
                        FROM tbl_debit_note_details s";
                }
                else if (cmbState.Text.Trim() == "Petty Cash")
                {
                    query = @"SELECT 'Petty Cash' AS `Type`, 'VAT 5%' as name,
                          '' description, SUM(total_before_vat) AS `Amount`,
                         SUM(total_vat) AS `VAT Amount` FROM tbl_petty_cash_submition s;";
                }
                DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
                dgvCustomer.DataSource = dt;

                // ------------------ Table Design Enhancements ------------------
                dgvCustomer.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                dgvCustomer.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGray;
                dgvCustomer.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgvCustomer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvCustomer.EnableHeadersVisualStyles = false;

                dgvCustomer.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
                dgvCustomer.DefaultCellStyle.ForeColor = Color.Black;
                dgvCustomer.DefaultCellStyle.BackColor = Color.White;
                dgvCustomer.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 255); // Light Blue Tint

                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCustomer.RowHeadersVisible = false;
                dgvCustomer.AllowUserToAddRows = false;
                dgvCustomer.AllowUserToResizeRows = false;
                dgvCustomer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                if (dt.Columns.Contains("Type"))
                    dgvCustomer.Columns["Type"].Width = 80;

                dgvCustomer.Columns["name"].HeaderText = "Tax Name";
                dgvCustomer.Columns["description"].HeaderText = "Tax Description";

                dgvCustomer.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvCustomer.Columns["VAT Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                dgvCustomer.Columns["Amount"].DefaultCellStyle.Format = "N2";
                dgvCustomer.Columns["VAT Amount"].DefaultCellStyle.Format = "N2";
                decimal totalAmount = 0;
                decimal totalVat = 0;

                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
                foreach (DataRow row in dt.Rows)
                {
                    totalAmount += Convert.ToDecimal(row["Amount"]);
                    totalVat += Convert.ToDecimal(row["VAT Amount"]);
                }

                // Add total row to the DataTable
                DataRow totalRow = dt.NewRow();
                totalRow["description"] = "TOTAL";
                totalRow["Amount"] = totalAmount;
                totalRow["VAT Amount"] = totalVat;

                dt.Rows.Add(totalRow);
            }catch(Exception ex)
            {
                ex.ToString();
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            BindItems();
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindItems();
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            string type = dgvCustomer.CurrentRow.Cells["type"].Value.ToString();
            //if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Sales All")
                frmLogin.frmMain.openChildForm(new MasterTransactionJournal("0", type + " All"));
            //else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Purchase All")
            //    //
            //else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Purchase Return All")
            //    frmLogin.frmMain.openChildForm(new frmPurchaseReturn(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            //else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Sales Return All")
            //    frmLogin.frmMain.openChildForm(new frmSalesReturn(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            //else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Debit Note All")
            //    frmLogin.frmMain.openChildForm(new frmDebitNote(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            //else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Credit Note All")
            //    frmLogin.frmMain.openChildForm(new frmCreditNote(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
            //else if (dgvCustomer.CurrentRow.Cells["type"].Value.ToString() == "Petty Cash All")
            //    frmLogin.frmMain.openChildForm(new frmPurchaseReturn(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
        }
    }
}
