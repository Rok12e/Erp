using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmPettyCashSubmission : Form
    {
        int id;
        int level4VatId = 0, level4PaymentCreditMethodId = 0;
        int _submissionId = 0;
        public frmPettyCashSubmission(int submissionId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            _submissionId = submissionId;

            if (id != 0)
                this.Text = "Petty Cash Submission - Edit";
            else
                this.Text = "Petty Cash Submission - New";
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmPettyCashSubmission_Load(object sender, EventArgs e)
        {
            dtpPettySubmition.Value = DateTime.Now.Date;
            this.dgvPetty.Columns.AddRange(new DataGridViewColumn[] {
            this.sn,
            //this.PCDate,
            this.account_name,
            this.Category,
            this.CostCenterName,
            this.amount,
            this.VATCheckBox,
            this.vat,
            this.Total,
            this.note});
            dgvPetty.Columns.Insert(1, new CalendarColumn() { Name = "PCDate", HeaderText = "Date" });

            BindCombos.PopulatePettyCashCard(cmbPettyCashCard);
            BindAccounts();
            if (_submissionId != 0)
            {
                BindSubmission(_submissionId);
            }

            var defaultAccounts = BindCombos.LoadDefaultAccounts();
            level4VatId = defaultAccounts.ContainsKey("Vat Output") ? defaultAccounts["Vat Input"] : 0;
            level4PaymentCreditMethodId = defaultAccounts.ContainsKey("Petty Cash Account") ? defaultAccounts["Petty Cash Account"] : 0;

            if(level4PaymentCreditMethodId==null|| level4PaymentCreditMethodId == 0)
            {
                level4PaymentCreditMethodId = defaultAccounts.ContainsKey("Default Account For Cash") ? defaultAccounts["Default Account For Cash"] : 0;
            }

            LocalizationManager.LocalizeDataGridViewHeaders(dgvPetty);
        }
        private void BindSubmission(int id)
        {

            string query = "SELECT * FROM tbl_petty_cash_submition WHERE id = @id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", id)))
            {
                if (reader.Read())
                {
                    txtREF.Text = reader["code"].ToString();
                    dtpPettySubmition.Value = Convert.ToDateTime(reader["date"]);
                    cmbPettyCashCard.SelectedValue = reader["name"].ToString();
                    txtAmount.Text = Convert.ToDecimal(reader["amount"]).ToString("N2");
                    txtTotalBefore.Text = Convert.ToDecimal(reader["total_before_vat"]).ToString("N2");
                    txtTotalVat.Text = Convert.ToDecimal(reader["total_vat"]).ToString("N2");
                    txtTotal.Text = Convert.ToDecimal(reader["net_amount"]).ToString("N2");
                }
            }

            dgvPetty.Rows.Clear();
            string detailQuery = @"
                                    SELECT 
                                        pcd.date,
                                        coa.id AS account_id, coa.name AS account_name,
                                        cat.id AS category_id, cat.name AS category_name,
                                        IFNULL ((select id from tbl_sub_cost_center where id = pcd.cost_center_id),0) AS cost_center_id, 
													 IFNULL((SELECT NAME from tbl_sub_cost_center where id = pcd.cost_center_id),0) AS cost_center_name,
                                        pcd.amount,
                                        pcd.vat,
                                        pcd.total,
                                        pcd.note
                                    FROM tbl_petty_cash_submition_details pcd
                                    INNER JOIN tbl_coa_level_4 coa ON pcd.account_id = coa.id
                                    INNER JOIN tbl_petty_cash_category cat ON pcd.category = cat.id
                                    -- INNER JOIN tbl_sub_cost_center cc ON pcd.cost_center_id = cc.id
                                    WHERE pcd.petty_id = @id";

            DataTable dt = DBClass.ExecuteDataTable(detailQuery, DBClass.CreateParameter("@id", id));

            int sn = 1;
            foreach (DataRow row in dt.Rows)
            {
                int index = dgvPetty.Rows.Add();
                DataGridViewRow dgvRow = dgvPetty.Rows[index];
                dgvRow.Cells["sn"].Value = sn++;
                dgvRow.Cells["PCDate"].Value = Convert.ToDateTime(row["date"]).ToString("yyyy-MM-dd");
                dgvRow.Cells["account_name"].Value = Convert.ToInt32(row["account_id"]);

                dgvRow.Cells["Category"].Value = Convert.ToInt32(row["category_id"]);
                if (int.Parse(row["cost_center_id"].ToString()) > 0)
                {
                    dgvRow.Cells["CostCenterName"].Value = row["cost_center_id"].ToString();
                }
                dgvRow.Cells["amount"].Value = Convert.ToDecimal(row["amount"]).ToString("N2");

                dgvRow.Cells["VATCheckBox"].Value = Convert.ToDecimal(row["vat"]) > 0; // true/false
                dgvRow.Cells["vat"].Value = Convert.ToDecimal(row["vat"]).ToString("N2");
                dgvRow.Cells["Total"].Value = Convert.ToDecimal(row["total"]).ToString("N2");
                dgvRow.Cells["note"].Value = row["note"].ToString();
            }
        }
        private void BindAccounts()
        {
            string query = "SELECT CONCAT(CODE , ' - ' , NAME) AS name, id FROM tbl_coa_level_4";
            DataTable dt = DBClass.ExecuteDataTable(query);

            // Find the ComboBox column inside dgvPetty
            DataGridViewComboBoxColumn cmbAccount = dgvPetty.Columns["account_name"] as DataGridViewComboBoxColumn;

            if (cmbAccount != null)
            {
                cmbAccount.DataSource = dt;
                cmbAccount.DisplayMember = "name";
                cmbAccount.ValueMember = "id";
            }
            string queryC = "select * from tbl_petty_cash_category";
            DataTable dtc = DBClass.ExecuteDataTable(queryC);

            // Find the ComboBox column inside dgvPetty
            DataGridViewComboBoxColumn cmbCategory = dgvPetty.Columns["Category"] as DataGridViewComboBoxColumn;

            if (cmbCategory != null)
            {
                cmbCategory.DataSource = dtc;
                cmbCategory.DisplayMember = "name";
                cmbCategory.ValueMember = "id";
            }
            //cost center
            string querycc = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_sub_cost_center";
            DataTable dtcc = DBClass.ExecuteDataTable(querycc);

            // Find the ComboBox column inside dgvPetty
            DataGridViewComboBoxColumn cmbcostcenter = dgvPetty.Columns["CostCenterName"] as DataGridViewComboBoxColumn;

            if (cmbcostcenter != null)
            {
                cmbcostcenter.DataSource = dtcc;
                cmbcostcenter.DisplayMember = "name";
                cmbcostcenter.ValueMember = "id";
            }
        }
        //private void BindSubmission(int id)
        //{
        //    string query = "SELECT * FROM tbl_petty_cash_submition WHERE id = @id";
        //    using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", id)))
        //    {
        //        if (reader.Read())
        //        {
        //            txt.Text = reader["code"].ToString();
        //            dtpSubmissionDate.Value = Convert.ToDateTime(reader["date"]);
        //            txtName.Text = reader["name"].ToString();
        //            txtAmount.Text = Convert.ToDecimal(reader["amount"]).ToString("N2");
        //            txtTotalBeforeVAT.Text = Convert.ToDecimal(reader["total_before_vat"]).ToString("N2");
        //            txtVAT.Text = Convert.ToDecimal(reader["total_vat"]).ToString("N2");
        //            txtNetAmount.Text = Convert.ToDecimal(reader["net_amount"]).ToString("N2");
        //        }
        //    }

        //    // Load submission details
        //    string detailQuery = @"
        //SELECT 
        //    pcd.id, pcd.date, coa.name AS AccountName, 
        //    cat.name AS Category, cc.name AS CostCenter,
        //    pcd.amount, pcd.vat, pcd.total, pcd.note
        //FROM tbl_petty_cash_submition_details pcd
        //INNER JOIN tbl_coa_level_4 coa ON pcd.account_id = coa.id
        //INNER JOIN tbl_petty_cash_category cat ON pcd.category = cat.id
        //INNER JOIN tbl_sub_cost_center cc ON pcd.cost_center_id = cc.id
        //WHERE pcd.petty_id = @id";

        //    DataTable dt = DBClass.ExecuteDataTable(detailQuery, DBClass.CreateParameter("@id", id));
        //    dgvDetails.DataSource = dt;
        //}
        private string GenerateNextPettyCode()
        {
            string newCode = "PS-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)) AS lastCode FROM tbl_petty_cash_submition"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "PS-" + code.ToString("D4");
                }
            }

            return newCode;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!checkData())
            {
                if (id == 0)
                {
                    if (insertPetty())
                        this.Close();
                }
                else
                 if (updatePetty())
                    this.Close();
            }
            else
            {
                MessageBox.Show("Check the default settings first.");
            }
        }
        private bool checkData()
        {
            if (level4VatId > 0 && level4PaymentCreditMethodId > 0)
            {
                return false;
            }
            return true;
        }

        private bool updatePetty()
        {
            try
            {
                string updateHeader = @"
            UPDATE tbl_petty_cash_submition
            SET 
                date = @date,
                name = @name,
                amount = @amount,
                total_before_vat = @total_before_vat,
                total_vat = @total_vat,
                net_amount = @net_amount
            WHERE id = @id";

                DBClass.ExecuteNonQuery(updateHeader,
                    DBClass.CreateParameter("date", dtpPettySubmition.Value.Date),
                    DBClass.CreateParameter("name", cmbPettyCashCard.SelectedValue?.ToString() ?? ""),
                    DBClass.CreateParameter("amount", Convert.ToDecimal(txtAmount.Text)),
                    DBClass.CreateParameter("total_before_vat", Convert.ToDecimal(txtTotalBefore.Text)),
                    DBClass.CreateParameter("total_vat", Convert.ToDecimal(txtTotalVat.Text)),
                    DBClass.CreateParameter("net_amount", Convert.ToDecimal(txtTotal.Text)),
                    DBClass.CreateParameter("id", id)
                );

                DBClass.ExecuteNonQuery(
                    "DELETE FROM tbl_petty_cash_submition_details WHERE petty_id = @id",
                    DBClass.CreateParameter("id", id)
                );

                DBClass.ExecuteNonQuery(
                    "DELETE FROM tbl_transaction WHERE transaction_id = @id AND type = 'Petty Cash Submission'",
                    DBClass.CreateParameter("id", id)
                );

                insertINV(id);

                MessageBox.Show("Submission updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        string pettyCashCode;
        private bool insertPetty()
        {
            if (!chkRequiredDate())
                return false;

            if (cmbPettyCashCard.Text.Trim() == "")
            {
                MessageBox.Show("Please Choose Name First.");
                return false;
            }
            txtREF.Text = GenerateNextPettyCode();
            decimal totalBeforeVat = decimal.Parse(txtTotalBefore.Text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            decimal totalVat = decimal.Parse(txtTotalVat.Text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            decimal netAmount = decimal.Parse(txtTotal.Text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            decimal amount = decimal.Parse(txtAmount.Text, NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            int id = (int)decimal.Parse(DBClass.ExecuteScalar(@"
            INSERT INTO `tbl_petty_cash_submition` 
            (`date`,`code`, `name`, `amount`,total_before_vat, `total_vat`, `net_amount`, `created_by`, `created_date`) 
            VALUES (@date,@code, @name, @amount, @total_before_vat, @total_vat, @net_amount, @created_by, @created_date);
            SELECT LAST_INSERT_ID();",
                DBClass.CreateParameter("date", dtpPettySubmition.Value.Date),
                DBClass.CreateParameter("code", txtREF.Text),
                DBClass.CreateParameter("name", cmbPettyCashCard.SelectedValue.ToString()),
                DBClass.CreateParameter("amount", amount),
                DBClass.CreateParameter("total_before_vat", totalBeforeVat),
                DBClass.CreateParameter("total_vat", totalVat),
                DBClass.CreateParameter("net_amount", netAmount),
                DBClass.CreateParameter("created_by", frmLogin.userId),
                DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
            insertINV(id);
            MessageBox.Show("Petty Cash Submission successfully saved!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            reset();
            return true;
        }
        private bool insertINV(int pvId)
        {
            decimal totalPaid = 0;

            for (int i = 0; i < dgvPetty.Rows.Count - 1; i++)
            {

                DateTime date = Convert.ToDateTime(dgvPetty.Rows[i].Cells["PCDate"].Value);
                int accountId = Convert.ToInt32(dgvPetty.Rows[i].Cells["account_name"].Value);
                string category = dgvPetty.Rows[i].Cells["Category"].Value.ToString();
                int costCenterId = Convert.ToInt32(dgvPetty.Rows[i].Cells["CostCenterName"].Value);
                decimal amount = Convert.ToDecimal(dgvPetty.Rows[i].Cells["amount"].Value);
                decimal vat = Convert.ToDecimal(dgvPetty.Rows[i].Cells["vat"].Value);
                decimal total = Convert.ToDecimal(dgvPetty.Rows[i].Cells["Total"].Value);
                string note = dgvPetty.Rows[i].Cells["note"].Value?.ToString() ?? "";

                totalPaid += total;

                DBClass.ExecuteNonQuery(@"
                INSERT INTO `tbl_petty_cash_submition_details`
                (`petty_id`, `date`, `account_id`, `category`, `cost_center_id`, `amount`, `vat`, `total`, `note`,state)
                VALUES (@petty_id, @date, @account_id, @category, @cost_center_id, @amount, @vat, @total, @note,@state);",
                    DBClass.CreateParameter("@petty_id", pvId),
                    DBClass.CreateParameter("@date", date),
                    DBClass.CreateParameter("@account_id", accountId),
                    DBClass.CreateParameter("@category", category),
                    DBClass.CreateParameter("@cost_center_id", costCenterId),
                    DBClass.CreateParameter("@amount", amount),
                    DBClass.CreateParameter("@vat", vat),
                    DBClass.CreateParameter("@total", total),
                    DBClass.CreateParameter("@note", note),
                    DBClass.CreateParameter("@state", "New")

                );

                CommonInsert.InsertTransactionEntry(date, accountId.ToString(),
                    amount.ToString(), "0", pvId.ToString(), "0", "Petty Cash Submission", "Petty Cash Submission - NO. " + pettyCashCode,
                    frmLogin.userId, DateTime.Now.Date);
                if (vat > 0)
                    CommonInsert.InsertTransactionEntry(date,
                  level4VatId.ToString(), vat.ToString(), "0", pvId.ToString(), "0", "Petty Cash Submission",
                  "Vat Input For Submission No. " + pettyCashCode, frmLogin.userId, DateTime.Now.Date);

                CommonInsert.InsertTransactionEntry(date, level4PaymentCreditMethodId.ToString(), "0", total.ToString(), pvId.ToString(), cmbPettyCashCard.SelectedValue.ToString(),
                    "Petty Cash Submission", "Petty Cash Submission NO. " + pettyCashCode,
                    frmLogin.userId, DateTime.Now.Date);


                //add cost center
                if (costCenterId> 0)
                    CommonInsert.InsertCostCenterTransaction(DateTime.Now.Date, total.ToString(), "0", pvId.ToString(), "Petty Cash", "", costCenterId.ToString());
            }
            //int pettyCashCardId = Convert.ToInt32(cmbPettyCashCard.SelectedValue);
            //DBClass.ExecuteNonQuery(@"
            //UPDATE tbl_petty_cash_request
            //SET pay = pay - @totalPaid
            //WHERE id = @id;",
            //DBClass.CreateParameter("@totalPaid", totalPaid),
            //DBClass.CreateParameter("@id", pettyCashCardId)
            //);
            return true;
        }

        private void reset()
        {
            txtTotalBefore.Text = "";
            txtTotalVat.Text = "";
            txtTotal.Text = "";
            txtAmount.Text = "";
            cmbPettyCashCard.SelectedIndex = -1;
            dgvPetty.Rows.Clear();
            dtpPettySubmition.Value = DateTime.Today;
        }
        private bool chkRequiredDate()
        {
            // Validate allowed petty cash amount
            decimal allowedAmount;
            if (!decimal.TryParse(txtAmount.Text.Trim(), out allowedAmount))
            {
                MessageBox.Show("Invalid petty cash amount!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            decimal totalSubmissionAmount = 0;

            if (dgvPetty.Rows.Count <= 1) // Only header or 1 empty row
            {
                MessageBox.Show("Please enter submission details first.", "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            for (int i = 0; i < dgvPetty.Rows.Count; i++)
            {
                if (dgvPetty.Rows[i].IsNewRow)
                    continue;

                var row = dgvPetty.Rows[i];

                // PCDate check
                if (row.Cells["PCDate"].Value == null || string.IsNullOrWhiteSpace(row.Cells["PCDate"].Value.ToString()))
                {
                    MessageBox.Show($"Row {i + 1}: PC Date cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Account name check
                if (row.Cells["account_name"].Value == null || string.IsNullOrWhiteSpace(row.Cells["account_name"].Value.ToString()))
                {
                    MessageBox.Show($"Row {i + 1}: Account Name cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Category check
                if (row.Cells["Category"].Value == null || string.IsNullOrWhiteSpace(row.Cells["Category"].Value.ToString()))
                {
                    MessageBox.Show($"Row {i + 1}: Category cannot be empty!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Amount check
                decimal amount;
                if (!decimal.TryParse(row.Cells["amount"].Value?.ToString().Replace(",", "").Trim(), out amount))
                {
                    MessageBox.Show($"Row {i + 1}: Amount is empty or invalid!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // VAT check
                decimal vat;
                if (!decimal.TryParse(row.Cells["vat"].Value?.ToString().Replace(",", "").Trim(), out vat))
                {
                    MessageBox.Show($"Row {i + 1}: VAT is empty or invalid!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                // Total check and accumulate
                decimal totalValue;
                if (!decimal.TryParse(row.Cells["total"].Value?.ToString().Replace(",", "").Trim(), out totalValue))
                {
                    MessageBox.Show($"Row {i + 1}: Total is empty or invalid!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                totalSubmissionAmount += totalValue;
            }

            // Final check: submission amount must not exceed allowed amount
            if (totalSubmissionAmount > allowedAmount)
            {
                MessageBox.Show($"The total amount you're trying to submit is more than the available petty cash.\n\nAvailable: {allowedAmount:N2}\nSubmitted: {totalSubmissionAmount:N2}\n\nPlease reduce the total before submitting.",
                                "Submission Limit Reached",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private decimal GetDecimalValue(DataGridViewRow row, string columnName)
        {
            decimal result;
            var cellValue = row.Cells[columnName].Value;
            if (cellValue != null && decimal.TryParse(cellValue.ToString(), out result))
                return result;
            else
                return 0;
        }

        private void cmbPettyCashCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPettyCashCard.SelectedValue == null)
            {
                txtAmount.Text = "";
                return;
            }
            if (cmbPettyCashCard.SelectedItem is DataRowView)
            {
                DataRowView row = cmbPettyCashCard.SelectedItem as DataRowView;
                decimal total = Convert.ToDecimal(row["total_amount"]);
                txtAmount.Text = total.ToString("N2");
            }
            else
                txtAmount.Text = "";
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_petty_cash_request where pay =@pay",
                DBClass.CreateParameter("pay", txtAmount));
            if (reader.Read())
                cmbPettyCashCard.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtAmount_Leave(object sender, EventArgs e)
        {
            //MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_petty_cash_request where pay =@pay",
            //      DBClass.CreateParameter("pay", txtAmount.Text));
            //if (!reader.Read())
            //    cmbPettyCashCard.SelectedIndex = -1;
        }

        private void dgvPetty_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPetty.CurrentRow == null)
                return;

            if (e.RowIndex >= 0 && (dgvPetty.Columns[e.ColumnIndex].Name == "VATCheckBox" ||
                              dgvPetty.Columns[e.ColumnIndex].Name == "vat" ||
                              dgvPetty.Columns[e.ColumnIndex].Name == "amount"))
            {
                UpdateAndCalculateTotal();
            }
            if (e.ColumnIndex == dgvPetty.Columns["amount"].Index)
            {
                if (dgvPetty.CurrentRow.Cells["amount"].Value == null || dgvPetty.CurrentRow.Cells["amount"].Value.ToString() == "" || dgvPetty.CurrentRow.Cells["total"].Value == null)
                    return;
                if (decimal.Parse(dgvPetty.CurrentRow.Cells["amount"].Value.ToString()) > decimal.Parse(txtAmount.Text))
                    dgvPetty.CurrentRow.Cells["amount"].Value = Utilities.FormatDecimal(txtAmount.Text);
            }
        }
        private void UpdateAndCalculateTotal()
        {
            decimal totalBeforeVAT = 0;
            decimal totalVAT = 0;
            decimal totalWithVAT = 0;

            foreach (DataGridViewRow row in dgvPetty.Rows)
            {
                if (row.IsNewRow) continue;

                decimal originalAmount = 0, vatValue = 0;
                bool isChecked = false;
                if (row.Cells["amount"].Value != null &&
                    decimal.TryParse(row.Cells["amount"].Value.ToString(), out originalAmount))
                {
                    totalBeforeVAT += originalAmount;
                }
                DataGridViewCheckBoxCell comboCell = (DataGridViewCheckBoxCell)dgvPetty.CurrentRow.Cells["VATCheckBox"];
                if (comboCell.Value != null)
                {
                    //
                }
                else
                {
                    comboCell.Value = false;
                }
                if (row.Cells["VATCheckBox"].Value != null)
                {
                    isChecked = Convert.ToBoolean(row.Cells["VATCheckBox"].Value);
                }

                vatValue = isChecked ? originalAmount * 0.05m : 0;
                decimal updatedTotal = originalAmount + vatValue;

                row.Cells["vat"].Value = vatValue.ToString("N2");
                row.Cells["total"].Value = updatedTotal.ToString("N2");
                row.Cells["CostCenterName"].Value = null;

                totalVAT += vatValue;
            }

            totalWithVAT = totalBeforeVAT + totalVAT;

            txtTotalBefore.Text = totalBeforeVAT.ToString("N2");
            txtTotalVat.Text = totalVAT.ToString("N2");
            txtTotal.Text = totalWithVAT.ToString("N2");
        }

        private void dgvPetty_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvPetty.Columns["vat"].Index)
            {
                chkRowValidty();
            }
        }
        private void chkRowValidty()
        {
            decimal amount = GetDecimalValue(dgvPetty.CurrentRow, "amount");

            if (amount == 0)
                dgvPetty.CurrentRow.Cells["total"].Value = "0";
            else
            {
                try
                {
                    dgvPetty.CurrentRow.Cells["total"].Value = amount;
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }

        private void dgvPetty_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dgvPetty.Columns[e.ColumnIndex].Name == "VATCheckBox" && e.RowIndex >= 0)
            {
                bool isChecked = Convert.ToBoolean(dgvPetty.Rows[e.RowIndex].Cells["VATCheckBox"].Value);

                if (isChecked)
                {
                    dgvPetty.Rows[e.RowIndex].Cells["VATCheckBox"].Value = false;
                }
                else
                {
                    dgvPetty.Rows[e.RowIndex].Cells["VATCheckBox"].Value = true;
                }
            }
        }
    }
}
