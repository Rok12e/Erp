using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterFixedAssets : Form
    {
        EventHandler fixedAsset;
        public MasterFixedAssets( )
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            fixedAsset = (sender, args) => BindFixedAseet();
            EventHub.FixedAsset += fixedAsset;
            headerUC1.FormText = this.Text;
        }
        private void MasterFixedAssets_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.FixedAsset -= fixedAsset;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewFixedAssets());
        }
        private void MasterFixedAssets_Load(object sender, EventArgs e)
        {
            dgvMain.Columns.Add("SN", "SN");
            dgvMain.Columns.Add("DATE", "DATE");
            dgvMain.Columns.Add("DAYS", "DAYS");
            dgvMain.Columns.Add("DESCRIPTION", "DESCRIPTION");
            dgvMain.Columns.Add("AMOUNT", "AMOUNT");
            dgvMain.Columns.Add("NOTE", "NOTE");
            dgvMain.Columns["note"].Width = 160;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvMain);
            BindFixedAseet();

            dgvCustomer.Columns["code"].Width = dgvCustomer.Columns["sn"].Width = 40;
            dgvCustomer.Columns["from"].Width = dgvCustomer.Columns["to"].Width = 60;
            dgvMain.Columns["description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        public void BindFixedAseet()
        {
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT 
            ROW_NUMBER() OVER (ORDER BY tbl_fixed_assets.id) AS `SN`, tbl_fixed_assets.id,
            tbl_fixed_assets.code AS 'Code', tbl_fixed_assets.name as 'Name', purchase_date as 'From',end_date as 'To',
            tbl_fixed_assets.purchase_price as Total from tbl_fixed_assets where tbl_fixed_assets.state = 0");
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["id"].Visible = false;
            if (dgvCustomer.Rows.Count > 0)
            {
                editCustomerToolStripMenuItem1.Visible = UserPermissions.canEdit("Fixed Assets");
                deleteCustomerToolStripMenuItem.Visible = UserPermissions.canDelete("Fixed Assets");
                bindFixedAseetData();
            }
        }

        //private void bindFixedAseetData()
        //{
        //    DateTime startDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["from"].Value.ToString());
        //    DateTime endDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["to"].Value.ToString());
        //    decimal totalAmount = decimal.Parse(dgvCustomer.SelectedRows[0].Cells["total"].Value.ToString());
        //    decimal actualTotal = 0;
        //    int totalDays = (endDate - startDate).Days + 1;

        //    dgvMain.Rows.Clear();

        //    DateTime currentDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month)); // Last day of first month
        //    int serialNumber = 1;

        //    int daysInFirstMonth = (currentDate - startDate).Days + 1;
        //    decimal firstMonthAmount = Math.Round((totalAmount / totalDays) * daysInFirstMonth, 4);
        //    actualTotal += firstMonthAmount;
        //    dgvMain.Rows.Add(serialNumber,
        //                     currentDate.ToString("MM-dd-yyyy"),
        //                     daysInFirstMonth,
        //                     dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
        //                     firstMonthAmount.ToString(),
        //                     "Fixed Asset REGISTER");
        //    serialNumber++;

        //    // Move to the last day of the next month
        //    currentDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
        //    currentDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));

        //    while (currentDate <= endDate)
        //    {
        //        int daysInMonth = (currentDate - new DateTime(currentDate.Year, currentDate.Month, 1)).Days + 1;

        //        if (currentDate > endDate)
        //            daysInMonth = (endDate - new DateTime(endDate.Year, endDate.Month, 1)).Days + 1;

        //        decimal monthlyAmount = Math.Round((totalAmount / totalDays) * daysInMonth, 4);
        //        actualTotal += monthlyAmount;

        //        dgvMain.Rows.Add(serialNumber,
        //                         currentDate.ToString("MM-dd-yyyy"),
        //                         daysInMonth,
        //                         dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
        //                         monthlyAmount.ToString(),
        //                         "Fixed Asset REGISTER");

        //        serialNumber++;

        //        // Move to the last day of the next month
        //        currentDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
        //        currentDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
        //    }

        //    dgvMain.Rows.Add(serialNumber, "", totalDays, "", actualTotal, "");
        //}
        private void bindFixedAseetData()
        {
            DateTime startDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["from"].Value.ToString());
            DateTime endDate = DateTime.Parse(dgvCustomer.SelectedRows[0].Cells["to"].Value.ToString());
            decimal totalAmount = decimal.Parse(dgvCustomer.SelectedRows[0].Cells["total"].Value.ToString());
            decimal actualTotal = 0;
            int totalDays = (endDate - startDate).Days + 1;

            dgvMain.Rows.Clear();

            DateTime currentMonthStart = startDate;
            int serialNumber = 1;

            while (currentMonthStart <= endDate)
            {
                DateTime currentMonthEnd = new DateTime(currentMonthStart.Year, currentMonthStart.Month, DateTime.DaysInMonth(currentMonthStart.Year, currentMonthStart.Month));
                
                if (currentMonthEnd > endDate)
                    currentMonthEnd = endDate;
                
                DateTime periodStart = currentMonthStart < startDate ? startDate : currentMonthStart;
                DateTime periodEnd = currentMonthEnd > endDate ? endDate : currentMonthEnd;

                int daysInPeriod = (periodEnd - periodStart).Days + 1;
                decimal amount = Math.Round((totalAmount / totalDays) * daysInPeriod, 2);
                
                if (periodEnd == endDate)
                {
                    amount = totalAmount - actualTotal;
                }

                actualTotal += amount;

                dgvMain.Rows.Add(serialNumber,
                                 periodEnd.ToString("MM-dd-yyyy"),
                                 daysInPeriod,
                                 dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString(),
                                 amount.ToString("N2"),
                                 "Fixed Asset REGISTER");

                serialNumber++;
                currentMonthStart = currentMonthStart.AddMonths(1);
                currentMonthStart = new DateTime(currentMonthStart.Year, currentMonthStart.Month, 1);
            }

            dgvMain.Rows.Add(serialNumber, "", totalDays, "", actualTotal.ToString("N2"), "");
        }


        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFixedAseet();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewFixedAssets( int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewFixedAssets( int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
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
            DBClass.ExecuteNonQuery("UPDATE tbl_fixed_assets SET state = -1 WHERE id = @id ",
                                          DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            Utilities.LogAudit(frmLogin.userId, "Delete Fixed Asset", "Fixed Asset", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Fixed Asset: " + dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString());
            BindFixedAseet();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));

        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bindFixedAseetData();

        }

        private void lnkCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new frmViewFixedAssetsCategory().Show();
        }

        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewFixedAssets());
        }

        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewFixedAssets(int.Parse(dgvCustomer.CurrentRow.Cells["id"].Value.ToString())));
        }

        private void newCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewFixedAssetsCategory().Show();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
