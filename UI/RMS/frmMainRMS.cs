using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.RMS.Model;
using YamyProject.RMS.View;
using YamyProject.UI.RMS.Model;

namespace YamyProject.RMS
{
    public partial class frmMainRMS : Form
    {
        public  Form addcontrols2(Form f)
        {
            pnlcenter.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            pnlcenter.Controls.Add(f);
            f.Show();
            return f;
        }

        public frmMainRMS()
        {
            InitializeComponent();
        }
        
        static frmMainRMS _obj;
        
        public static frmMainRMS Instance
        {
            get {if(_obj == null) {_obj = new frmMainRMS(); } return _obj;}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMainRMS_Load(object sender, EventArgs e)
        {
            _obj = this;
            lbuser.Text = frmLogin.userName;
            btnHome.PerformClick();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMShome());
        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMSCategoryView());
        
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMSTableView());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMSStaff());
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMSProductView());
        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            frmRMSPOS frm = new frmRMSPOS();
            frm.Show();
            frm.Username = lbuser.Text;
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMSKitchview());
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmRMSSetting());
        }

        private void guna2ControlBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
