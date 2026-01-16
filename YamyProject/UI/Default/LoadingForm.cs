using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YamyProject.UI.Default
{
    public partial class LoadingForm : Form
    {
        public Guna2CircleProgressBar loadingCircle;

        public LoadingForm()
        {
            InitializeComponent();
            this.loadingCircle = new Guna2CircleProgressBar
            {
                // loadingCircle
                Animated = true,
                Location = new Point(30, 20),
                Size = new Size(120, 120),
                Value = 70, // Constant value to show spinner look
                FillThickness = 10,
                ProgressThickness = 10
            };
            this.loadingCircle.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;

            // LoadingForm
            this.Controls.Add(this.loadingCircle);
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(180, 160);
            this.BackColor = Color.White;
            this.ShowInTaskbar = false;
            this.TopMost = true;
        }
    }
}
