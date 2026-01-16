using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;

namespace YamyProject.UI.Settings
{
    partial class FrmLicense
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelMain = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.textBox4 = new Guna.UI2.WinForms.Guna2TextBox();
            this.textBox3 = new Guna.UI2.WinForms.Guna2TextBox();
            this.textBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            this.textBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblTitle = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblInfo = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.txtSerial = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnActivate = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.shadow = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.Transparent;
            this.panelMain.BackgroundImage = global::YamyProject.Properties.Resources.YAMY_Splash1;
            this.panelMain.Controls.Add(this.guna2HtmlLabel1);
            this.panelMain.Controls.Add(this.textBox4);
            this.panelMain.Controls.Add(this.textBox3);
            this.panelMain.Controls.Add(this.textBox2);
            this.panelMain.Controls.Add(this.textBox1);
            this.panelMain.Controls.Add(this.pictureBox1);
            this.panelMain.Controls.Add(this.lblTitle);
            this.panelMain.Controls.Add(this.lblInfo);
            this.panelMain.Controls.Add(this.txtSerial);
            this.panelMain.Controls.Add(this.btnActivate);
            this.panelMain.Controls.Add(this.btnClose);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Padding = new System.Windows.Forms.Padding(30);
            this.panelMain.Size = new System.Drawing.Size(800, 405);
            this.panelMain.TabIndex = 0;
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(146, 171);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(303, 19);
            this.guna2HtmlLabel1.TabIndex = 10;
            this.guna2HtmlLabel1.Text = "Enter your serial key below to activate the software:";
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.Transparent;
            this.textBox4.BorderRadius = 10;
            this.textBox4.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox4.DefaultText = "";
            this.textBox4.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.Black;
            this.textBox4.Location = new System.Drawing.Point(557, 258);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4);
            this.textBox4.MaxLength = 4;
            this.textBox4.Name = "textBox4";
            this.textBox4.PlaceholderText = "XXXX";
            this.textBox4.SelectedText = "";
            this.textBox4.Size = new System.Drawing.Size(96, 30);
            this.textBox4.TabIndex = 9;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Transparent;
            this.textBox3.BorderRadius = 10;
            this.textBox3.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox3.DefaultText = "";
            this.textBox3.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Black;
            this.textBox3.Location = new System.Drawing.Point(426, 258);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.MaxLength = 4;
            this.textBox3.Name = "textBox3";
            this.textBox3.PlaceholderText = "XXXX";
            this.textBox3.SelectedText = "";
            this.textBox3.Size = new System.Drawing.Size(96, 30);
            this.textBox3.TabIndex = 8;
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.Transparent;
            this.textBox2.BorderRadius = 10;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox2.DefaultText = "";
            this.textBox2.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Black;
            this.textBox2.Location = new System.Drawing.Point(286, 258);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.MaxLength = 4;
            this.textBox2.Name = "textBox2";
            this.textBox2.PlaceholderText = "XXXX";
            this.textBox2.SelectedText = "";
            this.textBox2.Size = new System.Drawing.Size(96, 30);
            this.textBox2.TabIndex = 7;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Transparent;
            this.textBox1.BorderRadius = 10;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.DefaultText = "";
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(146, 258);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.MaxLength = 4;
            this.textBox1.Name = "textBox1";
            this.textBox1.PlaceholderText = "XXXX";
            this.textBox1.SelectedText = "";
            this.textBox1.Size = new System.Drawing.Size(96, 30);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::YamyProject.Properties.Resources.app_icon;
            this.pictureBox1.Location = new System.Drawing.Point(339, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(139, 133);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(209, 34);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Activate Your ERP";
            this.lblTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInfo
            // 
            this.lblInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblInfo.ForeColor = System.Drawing.Color.White;
            this.lblInfo.Location = new System.Drawing.Point(245, 232);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(303, 19);
            this.lblInfo.TabIndex = 1;
            this.lblInfo.Text = "Enter your serial key below to activate the software:";
            // 
            // txtSerial
            // 
            this.txtSerial.BackColor = System.Drawing.Color.Transparent;
            this.txtSerial.BorderRadius = 10;
            this.txtSerial.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSerial.DefaultText = "";
            this.txtSerial.Enabled = false;
            this.txtSerial.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.Location = new System.Drawing.Point(146, 197);
            this.txtSerial.Margin = new System.Windows.Forms.Padding(4);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.PlaceholderText = "XXXX-XXXX-XXXX-XXXX";
            this.txtSerial.SelectedText = "";
            this.txtSerial.Size = new System.Drawing.Size(507, 30);
            this.txtSerial.TabIndex = 2;
            this.txtSerial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnActivate
            // 
            this.btnActivate.BackColor = System.Drawing.Color.Transparent;
            this.btnActivate.BorderRadius = 10;
            this.btnActivate.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnActivate.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnActivate.ForeColor = System.Drawing.Color.White;
            this.btnActivate.Location = new System.Drawing.Point(354, 303);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(106, 40);
            this.btnActivate.TabIndex = 3;
            this.btnActivate.Text = "Activate";
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BorderRadius = 10;
            this.btnClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(354, 352);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // shadow
            // 
            this.shadow.TargetForm = this;
            // 
            // FrmLicense
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(41)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(800, 405);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmLicense_Load);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2Panel panelMain;
        private Guna2HtmlLabel lblTitle;
        private Guna2HtmlLabel lblInfo;
        private Guna2TextBox txtSerial;
        private Guna2Button btnActivate;
        private Guna2Button btnClose;
        private Guna2ShadowForm shadow;
        private PictureBox pictureBox1;
        private Guna2TextBox textBox4;
        private Guna2TextBox textBox3;
        private Guna2TextBox textBox2;
        private Guna2TextBox textBox1;
        private Guna2HtmlLabel guna2HtmlLabel1;
    }
}