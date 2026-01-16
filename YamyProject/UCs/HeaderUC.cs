using System;
using System.Drawing;
using System.Windows.Forms;

namespace YamyProject
{
    public partial class HeaderUC : UserControl
    {
        private Random random = new Random();
        private Point previousLocation;
        public HeaderUC()
        {
            InitializeComponent();
            this.MouseClick += (s, e) => { MessageBox.Show("Mouse clicked on header!"); };
        }
        private void HeaderUC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.ParentForm != null)
            {
                if (this.ParentForm.Dock == DockStyle.Fill)
                {
                    this.ParentForm.Dock = DockStyle.None; 
                        previousLocation = this.ParentForm.Location;
                }
                else
                    this.ParentForm.Dock = DockStyle.Fill;
            }
        }
        public string FormText
        {
            get { return lblFormName.Text; }
            set { lblFormName.Text = value; }
        }

        public string HeaderText {
            get { return lblFormName.Text; }
            set { lblFormName.Text = value; }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            var parentForm = this.ParentForm;
            if (parentForm != null)
                parentForm.WindowState = FormWindowState.Minimized;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            var parentForm = this.ParentForm;
            if (parentForm != null)
                parentForm.Close();
        }
        private void btnMaximize_Click(object sender, EventArgs e)
        {
            try
            {
                var parentForm = this.ParentForm;
                if (parentForm != null)
                {
                    if (parentForm.Dock == DockStyle.Fill)
                    {
                        parentForm.Dock = DockStyle.None;
                        Rectangle screenBounds = Screen.PrimaryScreen.WorkingArea;
                        int randomX = random.Next(screenBounds.Left, screenBounds.Right - parentForm.Width);
                        int randomY = random.Next(screenBounds.Top, screenBounds.Bottom - parentForm.Height);
                        parentForm.Location = new Point(randomX, randomY);
                    }
                    else
                        parentForm.Dock = DockStyle.Fill;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }
        private void lblFormName_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.ParentForm != null)
            {
                if (this.ParentForm.Dock == DockStyle.Fill)
                {
                    this.ParentForm.Dock = DockStyle.None;

                    int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
                    int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

                    int x = (screenWidth - this.ParentForm.Width) / 2;
                    int y = (screenHeight - this.ParentForm.Height) / 2;

                    this.ParentForm.Location = new Point(x, y);
                }
                else
                    this.ParentForm.Dock = DockStyle.Fill;
            }
        }

        private void lblFormName_Click(object sender, EventArgs e)
        {

        }
    }
}
