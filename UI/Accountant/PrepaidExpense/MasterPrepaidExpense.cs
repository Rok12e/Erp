using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterPrepaidExpense : Form
    {
        EventHandler PrepaidExpense;
        public MasterPrepaidExpense()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            PrepaidExpense = (sender, args) => BindPrepaid();
            EventHub.PrepaidExpense += PrepaidExpense;
            headerUC1.FormText = this.Text;
        }
        private void MasterPrepaidExpense_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.PrepaidExpense -= PrepaidExpense;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewPrepaidExpense());
        }
        private void MasterPrepaidExpense_Load(object sender, EventArgs e)
        {
            dgvMain.Columns.Add("SN", "SN");
            dgvMain.Columns.Add("DATE", "DATE");
            dgvMain.Columns.Add("DAYS", "DAYS");
            dgvMain.Columns.Add("DESCRIPTION", "DESCRIPTION");
            dgvMain.Columns.Add("AMOUNT", "AMOUNT");
            dgvMain.Columns.Add("NOTE", "NOTE");
            dgvMain.Columns["note"].Width = 160;
            BindPrepaid();

            dgvCustomer.Columns["code"].Width = dgvCustomer.Columns["sn"].Width = 40;
            dgvCustomer.Columns["from"].Width = dgvCustomer.Columns["to"].Width = 60;
            dgvMain.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvMain);

        }
        public void BindPrepaid()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT 
            ROW_NUMBER() OVER (ORDER BY tbl_prepaid_expense.id) AS `SN`, tbl_prepaid_expense.id,
            tbl_prepaid_expense.code AS 'Code', tbl_prepaid_expense.name as 'Name', start_date as 'From',end_date as 'To',
            tbl_prepaid_expense.total as Total from tbl_prepaid_expense where tbl_prepaid_expense.state = 0");
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["id"].Visible = false;
            if (dgvCustomer.Rows.Count > 0)
                bindPrepaidData();

            btnEdit.Enabled = UserPermissions.canEdit("Prepaid Expense");


            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        private void bindPrepaidData()
        {
            DateTime startDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["from"].Value.ToString());
            DateTime endDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["to"].Value.ToString());
            decimal totalAmount = decimal.Parse(dgvCustomer.SelectedRows[0].Cells["total"].Value.ToString());
            int totalDays = (endDate - startDate).Days + 1;

            dgvMain.Rows.Clear();

            DateTime currentDate = startDate;
            int serialNumber = 1;

            // Get last day of the first month
            int lastDayOfFirstMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            DateTime lastDateOfFirstMonth = new DateTime(currentDate.Year, currentDate.Month, lastDayOfFirstMonth);

            if (lastDateOfFirstMonth > endDate) lastDateOfFirstMonth = endDate; // Ensure it doesn't exceed endDate

            int daysInFirstMonth = (lastDateOfFirstMonth - startDate).Days + 1;
            decimal firstMonthAmount = Math.Round((totalAmount / totalDays) * daysInFirstMonth, 3);

            dgvMain.Rows.Add(serialNumber,
                             lastDateOfFirstMonth.ToString("MM-dd-yyyy"),
                             daysInFirstMonth,
                             dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
                             firstMonthAmount.ToString(),
                             "Prepaid Expense REGISTER");
            serialNumber++;

            // Move to the next month
            currentDate = lastDateOfFirstMonth.AddDays(1);

            while (currentDate <= endDate)
            {
                int lastDay = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                DateTime lastDateOfMonth = new DateTime(currentDate.Year, currentDate.Month, lastDay);

                if (lastDateOfMonth > endDate)
                    lastDateOfMonth = endDate;

                int daysInMonth = (lastDateOfMonth - currentDate).Days + 1;
                decimal monthlyAmount = Math.Round((totalAmount / totalDays) * daysInMonth, 2);

                dgvMain.Rows.Add(serialNumber,
                                 lastDateOfMonth.ToString("MM-dd-yyyy"),
                                 daysInMonth,
                                 dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
                                 monthlyAmount.ToString(),
                                 "Prepaid Expense REGISTER");

                serialNumber++;
                currentDate = lastDateOfMonth.AddDays(1);
            }

            dgvMain.Rows.Add(serialNumber, "", totalDays, "", totalAmount, "");
        }

        //private void bindPrepaidData()
        //{
        //    DateTime startDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["from"].Value.ToString());
        //    DateTime endDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["to"].Value.ToString());
        //    decimal totalAmount = decimal.Parse(dgvCustomer.SelectedRows[0].Cells["total"].Value.ToString());
        //    int totalDays = (endDate - startDate).Days + 1;

        //    dgvMain.Rows.Clear();

        //    DateTime currentDate = startDate;
        //    int serialNumber = 1;

        //    int daysInFirstMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month) - currentDate.Day + 1;
        //    decimal firstMonthAmount = Math.Round((totalAmount / totalDays) * daysInFirstMonth, 3);

        //    dgvMain.Rows.Add(serialNumber,
        //                     currentDate.ToString("MM-dd-yyyy"),
        //                     daysInFirstMonth,
        //                     dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
        //                     firstMonthAmount.ToString(),
        //                     "Prepaid Expense REGISTER");
        //    serialNumber++;

        //    currentDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);

        //    while (currentDate <= endDate)
        //    {
        //        int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);

        //        if (currentDate.AddDays(daysInMonth - 1) > endDate)
        //            daysInMonth = (endDate - currentDate).Days + 1;

        //        decimal monthlyAmount = Math.Round((totalAmount / totalDays) * daysInMonth, 2);

        //        dgvMain.Rows.Add(serialNumber,
        //                         currentDate.ToString("MM-dd-yyyy"),
        //                         daysInMonth,
        //                         dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
        //                         monthlyAmount.ToString(),
        //                         "Prepaid Expense REGISTER");
        //        serialNumber++;
        //        currentDate = currentDate.AddMonths(1);


        //    }
        //    dgvMain.Rows.Add(serialNumber,
        //                        "",
        //                        totalDays,
        //                       "",
        //                        totalAmount,
        //                        "");
        //}

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindPrepaid();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewPrepaidExpense(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewPrepaidExpense(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            DBClass.ExecuteNonQuery("UPDATE tbl_sales SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Prepaid Expense", "Prepaid Expense", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Prepaid Expense: " + dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString());
            BindPrepaid();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bindPrepaidData();
        }

        
    }
}
