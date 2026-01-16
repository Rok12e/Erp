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

namespace YamyProject.UI.Default
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void guna2ShadowPanel1_Paint(object sender, PaintEventArgs e)
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

        private void panel8_Click(object sender, EventArgs e)
        {
            this.BringToFront();    
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterPurchaseOrder());
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterInventory());
        }

        private void guna2TileButton3_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchase());
        }

        private void guna2TileButton4_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher());
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterPurchases());
        }

        private void guna2TileButton6_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmSales());
        }

        private void guna2TileButton7_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterReceiptVoucher());
        }

        private void guna2TileButton8_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewReceiptVoucher());
        }

        private void guna2TileButton11_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterPurchaseOrder());
        }

        private void guna2TileButton5_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewItemTaxCodes());
        }

        private void guna2TileButton10_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterCustomer());
        }

        private void guna2TileButton12_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterChartOfAccount());
        }

        private void guna2TileButton14_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterInventory());
        }

        private void guna2TileButton13_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new ItemInAndOutReport());
        }

        private void guna2TileButton15_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterSalesOrder());
        }

        private void guna2TileButton16_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterTransactionJournal("0",""));
        }
        private MonthCalendar calendar;
        private void guna2TileButton17_Click(object sender, EventArgs e)
        {
            if (calendar == null)
            {
                calendar = new MonthCalendar();
                calendar.Location = new Point(guna2TileButton17.Left, guna2TileButton17.Bottom + 5);
                calendar.MaxSelectionCount = 1; // only allow single date selection
                calendar.DateSelected += Calendar_DateSelected;

                this.Controls.Add(calendar);
                calendar.BringToFront();
            }
            else
            {
                // Toggle visibility
                calendar.Visible = !calendar.Visible;
            }
        }
        private void Calendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            MessageBox.Show("Selected date: " + e.Start.ToShortDateString());
            calendar.Visible = false; // hide after selection
        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher());
        }

        private void guna2TileButton19_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterBankReconciliation());
        }

        private void guna2TileButton20_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher());
        }

        private void guna2TileButton21_Click(object sender, EventArgs e)
        {
            new frmViewCheque().ShowDialog();
        }

        private void guna2TileButton22_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterCheque());
        }
    }
}
