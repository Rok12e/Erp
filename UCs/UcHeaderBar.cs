using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YamyProject.UCs
{
    public partial class UcHeaderBar : UserControl
    {
        public UcHeaderBar()
        {
            InitializeComponent();
        }

        // Optional: Expose the header text as a property
        public string HeaderText
        {
            get => Lbheader.Text;
            set => Lbheader.Text = value;
        }
    }
}
