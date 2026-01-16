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
using YamyProject.RMS;
using YamyProject.UI.CRM.Pages;

namespace YamyProject.UI.CRM
{
   
    public partial class frmCRMmain : Form
    {
        static frmCRMmain _obj;
        public static frmCRMmain Instance
        {
            get { if (_obj == null) { _obj = new frmCRMmain(); } return _obj; }
        }

        public Form addcontrols2(Form f)
        {
            pnlcenter.Controls.Clear();
            f.TopLevel = false;
            pnlcenter.Controls.Add(f);
            f.Show();
            return f;
        }
        public frmCRMmain()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void frmCRMmain_Load(object sender, EventArgs e)
        {
            _obj = this;
            indicator.Visible = false;


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

        private void btndashboard_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var btndashboard = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = btndashboard.Top + (btndashboard.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmCRMDashboard());

        }

   

        private void guna2CircleButton2_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var guna2CircleButton2 = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = guna2CircleButton2.Top + (guna2CircleButton2.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmCRMLeads());

        }

        private void guna2GradientPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CircleButton5_Click(object sender, EventArgs e)
        {
            indicator.Visible = true;
            var guna2CircleButton2 = (Control)sender;
            //Pages.SetPage(btn.Tag.ToString());     
            indicator.Top = guna2CircleButton2.Top + (guna2CircleButton2.Height / 2 - indicator.Height / 2);
            addcontrols2(new frmCRMCustomer());
        }

        private void guna2CircleButton6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
