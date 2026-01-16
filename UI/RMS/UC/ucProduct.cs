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

namespace YamyProject.RMS.UC
{
    public partial class ucProduct : UserControl
    {
        public ucProduct()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

       public event EventHandler onSelect = null;

        public int id {  get; set; }
        public string Pprice { get; set; }

        public string PCategory { get; set; }
        public string Pname
        {
            get { return LbName.Text;}
            set { LbName.Text = value; }
        }
        public Image Pimage
        {
            get { return txtImage.Image; }
            set { txtImage.Image = value; }
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {
            onSelect?.Invoke(this, e);
        }
    }
}
