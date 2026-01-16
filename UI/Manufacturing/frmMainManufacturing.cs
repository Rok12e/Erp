using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.UI.CRM;
using YamyProject.UI.CRM.Pages;
using YamyProject.UI.Manufacturing.Viewform;

namespace YamyProject.UI.Manufacturing
{
    public partial class frmMainManufacturing : Form
    {

        static frmMainManufacturing _obj;
        public static frmMainManufacturing Instance
        {
            get { if (_obj == null) { _obj = new frmMainManufacturing(); } return _obj; }
        }
        public frmMainManufacturing()
        {
            InitializeComponent();
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmManBatchProduction());
        }
       

        private void frmMainManufacturing_Load(object sender, EventArgs e)
        {
            _obj = this;
            indicator.Visible = false;
        }

        public Form addcontrols2(Form f)
        {
            pnlcenter.Controls.Clear();
            f.TopLevel = false;
            pnlcenter.Controls.Add(f);
            f.Show();
            return f;
        }

        private void btnNav_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmManDashboard());
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

        private void guna2GradientPanel1_DoubleClick(object sender, EventArgs e)
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

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmManMachinRecordView());

        }

        private void btnOrders_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmManScreen());
        }

        private void btnOrderD_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            //addcontrols2(new frmManDashboard());
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2CircleButton3_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btnSuff = (Control)sender;
            indicator.Top = btnSuff.Top + (btnSuff.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmManStaff());

            //Pages.SetPage(btn.Tag.ToString());     
        }

        private void guna2CircleButton4_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            addcontrols2(new frmManWorkOrder());
        }

        private void guna2CircleButton5_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
        }

        private void guna2CircleButton6_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btnHome = (Control)sender;
            indicator.Top = btnHome.Top + (btnHome.Height / 2 - indicator.Height / 2);
            pnlcenter.Controls.Clear();
        }

        private void btnWorkFlowCenter_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmManWorkFlow());
        }

        private void guna2CircleButton1_Click_1(object sender, EventArgs e)
        {
            if (contextMenuStrip1 != null && contextMenuStrip1.Items.Count <= 0)
            {
                ToolStripMenuItem item1 = new ToolStripMenuItem("Machine Report");
                item1.Click += (s, args) =>
                {
                    indicator.Visible = true;
                    indicator.Top = ((Control)sender).Top + (((Control)sender).Height / 2 - indicator.Height / 2);
                    addcontrols2(new frmManMachineReportSummary());
                };
                ToolStripMenuItem item2 = new ToolStripMenuItem("Batch Report");
                item2.Click += (s, args) =>
                {
                    indicator.Visible = true;
                    indicator.Top = ((Control)sender).Top + (((Control)sender).Height / 2 - indicator.Height / 2);
                    addcontrols2(new frmManBatchReportSummary());
                };
                ToolStripMenuItem item3 = new ToolStripMenuItem("Work Order Report");
                item3.Click += (s, args) =>
                {
                    indicator.Visible = true;
                    indicator.Top = ((Control)sender).Top + (((Control)sender).Height / 2 - indicator.Height / 2);
                    addcontrols2(new frmManWorkOrderReportSummary());
                };
                ToolStripMenuItem item4 = new ToolStripMenuItem("Work Flow Report");
                item4.Click += (s, args) =>
                {
                    indicator.Visible = true;
                    indicator.Top = ((Control)sender).Top + (((Control)sender).Height / 2 - indicator.Height / 2);
                    addcontrols2(new frmManWorkFlowReportSummary());
                };
                ToolStripMenuItem item5 = new ToolStripMenuItem("Employee Report");
                item5.Click += (s, args) =>
                {
                    indicator.Visible = true;
                    indicator.Top = ((Control)sender).Top + (((Control)sender).Height / 2 - indicator.Height / 2);
                    addcontrols2(new frmManEmployeeReportSummary());
                };
                contextMenuStrip1.Items.Add(item1);
                contextMenuStrip1.Items.Add(item2);
                contextMenuStrip1.Items.Add(item3);
                contextMenuStrip1.Items.Add(item4);
                contextMenuStrip1.Items.Add(item5);
            }
            contextMenuStrip1.Show(guna2CircleButton1, new Point(0, guna2CircleButton1.Height));
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
