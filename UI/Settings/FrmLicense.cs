using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;
using System.Drawing;
using static HardwareInfo;

namespace YamyProject.UI.Settings
{
    public partial class FrmLicense : Form
    {
        public FrmLicense()
        {
            InitializeComponent();

            // Attach same event handler to all
            textBox1.TextChanged += TextBox_TextChanged;
            textBox2.TextChanged += TextBox_TextChanged;
            textBox3.TextChanged += TextBox_TextChanged;
            textBox4.TextChanged += TextBox_TextChanged;

            // Optional: allow only letters and numbers
            textBox1.KeyPress += TextBox_KeyPress;
            textBox2.KeyPress += TextBox_KeyPress;
            textBox3.KeyPress += TextBox_KeyPress;
            textBox4.KeyPress += TextBox_KeyPress;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (sender is Guna2TextBox current)
            {
                if (current.Text.Length >= current.MaxLength && current.MaxLength > 0)
                {
                    // Move focus to next control
                    this.SelectNextControl(current, true, true, true, true);
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allow only A–Z, 0–9 and backspace
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar))
            {
                e.Handled = true;
            }

            // Force uppercase
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            if (ActivationManager.IsActivated())
            {
                MessageBox.Show("Already Activated ✅");
                return;
            }

            string enteredKey = $"{textBox1.Text}-{textBox2.Text}-{textBox3.Text}-{textBox4.Text}";

            if (ActivationManager.ValidateLicense(enteredKey))
            {
                MessageBox.Show("License Activated ✅");
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid License ❌");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLicense_Load(object sender, EventArgs e)
        {
            if (ActivationManager.IsActivated())
            {
                textBox1.Visible = textBox2.Visible = textBox3.Visible = textBox4.Visible = false;

                // Update button
                btnActivate.Text = "Already Activated ✅";
                btnActivate.ForeColor = Color.White;
                btnActivate.FillColor = Color.FromArgb(76, 175, 80); // Green background

                // Optional: disable button to prevent clicks
                btnActivate.Enabled = false;

                // Optional: show label
                lblInfo.Text = "Your ERP is fully activated!";
                lblInfo.ForeColor = Color.White;
            }
            else
            {
                if (TrialManager.IsTrialValid())
                {
                    int daysLeft = TrialManager.GetRemainingTrialDays();
                    lblInfo.Text = $"Trial active: {daysLeft} day(s) remaining.";
                    lblInfo.ForeColor = Color.Yellow;
                }
                else
                {
                    lblInfo.Text = "Trial expired! Enter a valid license to continue.";
                    lblInfo.ForeColor = Color.Red;
                }
                txtSerial.Text = HardwareInfo.GetMotherboardSerial();
            }
        }
    }
}
