using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAmountAdjustments : Form
    {
        public decimal TotalAmount { get; private set; } = 0;
        public string AdjustmentType { get; set; }
        private string empName, empCode,refId="0";
        private DataTable dtAdjustments;

        public frmAmountAdjustments(string name, string code, string type,string _refId="0")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = "Edit Amount";
            lblDate.Text = DateTime.Now.Date.ToString("yyyy-MM-dd");
            lblName.Text = name;
            lblCode.Text = code;
            empName = name;
            empCode = code;
            refId = _refId;
            AdjustmentType = type;
        }

        private void frmAmountAdjustments_Load(object sender, EventArgs e)
        {
            dtAdjustments = new DataTable();

            // 💥 Ensure these exact names match everywhere
            dtAdjustments.Columns.Add("Description", typeof(string));
            dtAdjustments.Columns.Add("Amount", typeof(decimal));

            dgvAdjustments.DataSource = dtAdjustments;

            dgvAdjustments.AllowUserToAddRows = true;
            dgvAdjustments.AllowUserToDeleteRows = true;
            dgvAdjustments.Columns["Amount"].DefaultCellStyle.Format = "N3";
            dgvAdjustments.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvAdjustments.CellEndEdit += dgvAdjustments_CellEndEdit;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvAdjustments);
            UpdateTotalLabel();
        }

        private void UpdateTotalLabel()
        {
            if (dtAdjustments == null) return;

            decimal total = 0;

            foreach (DataRow row in dtAdjustments.Rows)
            {
                string desc = row["Description"]?.ToString();
                if (string.IsNullOrWhiteSpace(desc))
                    continue;

                decimal value = 0;
                decimal.TryParse(row["Amount"]?.ToString(), out value);
                total += value;
            }

            lblTotal.Text = $"Total: {total.ToString()}";
        }

        private void dgvAdjustments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotalLabel();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal total = 0;

            if (dtAdjustments == null || !dtAdjustments.Columns.Contains("Description") || !dtAdjustments.Columns.Contains("Amount"))
            {
                MessageBox.Show("Columns not found in DataTable.");
                return;
            }

            foreach (DataRow row in dtAdjustments.Rows)
            {
                if (!dtAdjustments.Columns.Contains("Description") || !dtAdjustments.Columns.Contains("Amount"))
                {
                    MessageBox.Show("Missing expected columns in the row.");
                    continue;
                }

                string desc = row["Description"]?.ToString();
                if (string.IsNullOrWhiteSpace(desc))
                    continue;

                decimal amount = 0;
                decimal.TryParse(row["Amount"]?.ToString(), out amount);
                total += amount;

                if (amount > 0)
                {
                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_salary_adjustments 
                        (code, name, adjustment_type, description, amount,date,ref_id)
                        VALUES (@code, @name, @adjustment_type, @description, @amount,@date,@refId)",
                        DBClass.CreateParameter("@code", lblCode.Text),
                        DBClass.CreateParameter("@name", lblName.Text),
                        DBClass.CreateParameter("@adjustment_type", AdjustmentType),
                        DBClass.CreateParameter("@description", desc),
                        DBClass.CreateParameter("@amount", amount),
                        DBClass.CreateParameter("@refId", refId),
                        DBClass.CreateParameter("@date", lblDate.Text)
                    );
                    Utilities.LogAudit(frmLogin.userId, "Add Salary Adjustment", "Salary Adjustment", int.Parse(refId), 
                        $"Added Salary Adjustment for {lblName.Text} ({lblCode.Text}): {desc} - Amount: {amount}");
                }
            }

            TotalAmount = total;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dgvAdjustments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTotalLabel();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
