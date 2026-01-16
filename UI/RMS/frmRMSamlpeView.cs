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

namespace YamyProject.RMS
{
    public partial class frmRMSamlpeView : Form
    {
        public frmRMSamlpeView()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

       public virtual void btnAdd_Click(object sender, EventArgs e)
        {

        }

        public virtual void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

      
    }
}
