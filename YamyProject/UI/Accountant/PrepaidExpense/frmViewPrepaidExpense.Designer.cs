namespace YamyProject
{
    partial class frmViewPrepaidExpense
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.lnkCategory = new System.Windows.Forms.LinkLabel();
            this.txtAccountCodeCredit = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbAccountNameCredit = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtAccountCodeDebit = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbAccountNameDebit = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtTotalDays = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTotal = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtAgrementFeesAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtAgrementAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.dtpAgrementEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtPrepaid = new System.Windows.Forms.DateTimePicker();
            this.dtpAgrementStartDate = new System.Windows.Forms.DateTimePicker();
            this.txtCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtName = new Guna.UI2.WinForms.Guna2TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 535);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 33);
            this.panel1.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.BorderRadius = 5;
            this.btnClose.BorderThickness = 1;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(436, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 27);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.BorderRadius = 5;
            this.btnSave.BorderThickness = 3;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(316, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(113, 27);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.BorderRadius = 5;
            this.guna2GroupBox1.BorderThickness = 0;
            this.guna2GroupBox1.Controls.Add(this.lnkCategory);
            this.guna2GroupBox1.Controls.Add(this.txtAccountCodeCredit);
            this.guna2GroupBox1.Controls.Add(this.cmbAccountNameCredit);
            this.guna2GroupBox1.Controls.Add(this.txtAccountCodeDebit);
            this.guna2GroupBox1.Controls.Add(this.cmbAccountNameDebit);
            this.guna2GroupBox1.Controls.Add(this.txtTotalDays);
            this.guna2GroupBox1.Controls.Add(this.txtTotal);
            this.guna2GroupBox1.Controls.Add(this.txtAgrementFeesAmount);
            this.guna2GroupBox1.Controls.Add(this.txtAgrementAmount);
            this.guna2GroupBox1.Controls.Add(this.dtpAgrementEndDate);
            this.guna2GroupBox1.Controls.Add(this.dtPrepaid);
            this.guna2GroupBox1.Controls.Add(this.dtpAgrementStartDate);
            this.guna2GroupBox1.Controls.Add(this.txtCode);
            this.guna2GroupBox1.Controls.Add(this.cmbCategory);
            this.guna2GroupBox1.Controls.Add(this.txtName);
            this.guna2GroupBox1.Controls.Add(this.label12);
            this.guna2GroupBox1.Controls.Add(this.label10);
            this.guna2GroupBox1.Controls.Add(this.label4);
            this.guna2GroupBox1.Controls.Add(this.label5);
            this.guna2GroupBox1.Controls.Add(this.label1);
            this.guna2GroupBox1.Controls.Add(this.label9);
            this.guna2GroupBox1.Controls.Add(this.label11);
            this.guna2GroupBox1.Controls.Add(this.label8);
            this.guna2GroupBox1.Controls.Add(this.label6);
            this.guna2GroupBox1.Controls.Add(this.label2);
            this.guna2GroupBox1.Controls.Add(this.label3);
            this.guna2GroupBox1.Controls.Add(this.label7);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GroupBox1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(2, 37);
            this.guna2GroupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(553, 498);
            this.guna2GroupBox1.TabIndex = 3;
            this.guna2GroupBox1.Text = "Basic Data";
            this.guna2GroupBox1.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // lnkCategory
            // 
            this.lnkCategory.ActiveLinkColor = System.Drawing.Color.Silver;
            this.lnkCategory.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lnkCategory.DisabledLinkColor = System.Drawing.Color.Black;
            this.lnkCategory.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCategory.ForeColor = System.Drawing.Color.Black;
            this.lnkCategory.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lnkCategory.Location = new System.Drawing.Point(489, 129);
            this.lnkCategory.Name = "lnkCategory";
            this.lnkCategory.Size = new System.Drawing.Size(56, 25);
            this.lnkCategory.TabIndex = 209;
            this.lnkCategory.TabStop = true;
            this.lnkCategory.Text = "New";
            this.lnkCategory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkCategory.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCategory_LinkClicked);
            // 
            // txtAccountCodeCredit
            // 
            this.txtAccountCodeCredit.BackColor = System.Drawing.Color.Transparent;
            this.txtAccountCodeCredit.BorderRadius = 5;
            this.txtAccountCodeCredit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAccountCodeCredit.DefaultText = "";
            this.txtAccountCodeCredit.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAccountCodeCredit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAccountCodeCredit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAccountCodeCredit.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAccountCodeCredit.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAccountCodeCredit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAccountCodeCredit.ForeColor = System.Drawing.Color.Black;
            this.txtAccountCodeCredit.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAccountCodeCredit.Location = new System.Drawing.Point(346, 410);
            this.txtAccountCodeCredit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccountCodeCredit.Name = "txtAccountCodeCredit";
            this.txtAccountCodeCredit.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAccountCodeCredit.PlaceholderText = "";
            this.txtAccountCodeCredit.SelectedText = "";
            this.txtAccountCodeCredit.Size = new System.Drawing.Size(166, 25);
            this.txtAccountCodeCredit.TabIndex = 16;
            this.txtAccountCodeCredit.TextChanged += new System.EventHandler(this.txtAccountCodeCredit_TextChanged);
            this.txtAccountCodeCredit.Leave += new System.EventHandler(this.txtAccountCodeCredit_Leave);
            // 
            // cmbAccountNameCredit
            // 
            this.cmbAccountNameCredit.BackColor = System.Drawing.Color.Transparent;
            this.cmbAccountNameCredit.BorderRadius = 5;
            this.cmbAccountNameCredit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAccountNameCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountNameCredit.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAccountNameCredit.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAccountNameCredit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAccountNameCredit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAccountNameCredit.ItemHeight = 18;
            this.cmbAccountNameCredit.Location = new System.Drawing.Point(32, 410);
            this.cmbAccountNameCredit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAccountNameCredit.Name = "cmbAccountNameCredit";
            this.cmbAccountNameCredit.Size = new System.Drawing.Size(306, 24);
            this.cmbAccountNameCredit.TabIndex = 14;
            this.cmbAccountNameCredit.SelectedIndexChanged += new System.EventHandler(this.cmbAccountNameCredit_SelectedIndexChanged);
            // 
            // txtAccountCodeDebit
            // 
            this.txtAccountCodeDebit.BackColor = System.Drawing.Color.Transparent;
            this.txtAccountCodeDebit.BorderRadius = 5;
            this.txtAccountCodeDebit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAccountCodeDebit.DefaultText = "";
            this.txtAccountCodeDebit.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAccountCodeDebit.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAccountCodeDebit.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAccountCodeDebit.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAccountCodeDebit.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAccountCodeDebit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAccountCodeDebit.ForeColor = System.Drawing.Color.Black;
            this.txtAccountCodeDebit.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAccountCodeDebit.Location = new System.Drawing.Point(346, 345);
            this.txtAccountCodeDebit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccountCodeDebit.Name = "txtAccountCodeDebit";
            this.txtAccountCodeDebit.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAccountCodeDebit.PlaceholderText = "";
            this.txtAccountCodeDebit.SelectedText = "";
            this.txtAccountCodeDebit.Size = new System.Drawing.Size(166, 25);
            this.txtAccountCodeDebit.TabIndex = 12;
            this.txtAccountCodeDebit.TextChanged += new System.EventHandler(this.txtAccountCodeDebit_TextChanged);
            this.txtAccountCodeDebit.Leave += new System.EventHandler(this.txtAccountCodeDebit_Leave);
            // 
            // cmbAccountNameDebit
            // 
            this.cmbAccountNameDebit.BackColor = System.Drawing.Color.Transparent;
            this.cmbAccountNameDebit.BorderRadius = 5;
            this.cmbAccountNameDebit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAccountNameDebit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccountNameDebit.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAccountNameDebit.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAccountNameDebit.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbAccountNameDebit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAccountNameDebit.ItemHeight = 18;
            this.cmbAccountNameDebit.Location = new System.Drawing.Point(32, 345);
            this.cmbAccountNameDebit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAccountNameDebit.Name = "cmbAccountNameDebit";
            this.cmbAccountNameDebit.Size = new System.Drawing.Size(306, 24);
            this.cmbAccountNameDebit.TabIndex = 10;
            this.cmbAccountNameDebit.SelectedIndexChanged += new System.EventHandler(this.cmbAccountNameDebit_SelectedIndexChanged);
            // 
            // txtTotalDays
            // 
            this.txtTotalDays.BackColor = System.Drawing.Color.Transparent;
            this.txtTotalDays.BorderRadius = 5;
            this.txtTotalDays.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotalDays.DefaultText = "";
            this.txtTotalDays.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTotalDays.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTotalDays.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotalDays.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotalDays.Enabled = false;
            this.txtTotalDays.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalDays.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTotalDays.ForeColor = System.Drawing.Color.Black;
            this.txtTotalDays.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalDays.Location = new System.Drawing.Point(346, 195);
            this.txtTotalDays.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTotalDays.Name = "txtTotalDays";
            this.txtTotalDays.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotalDays.PlaceholderText = "";
            this.txtTotalDays.SelectedText = "";
            this.txtTotalDays.Size = new System.Drawing.Size(163, 25);
            this.txtTotalDays.TabIndex = 8;
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.Transparent;
            this.txtTotal.BorderRadius = 5;
            this.txtTotal.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotal.DefaultText = "";
            this.txtTotal.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTotal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTotal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotal.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotal.Enabled = false;
            this.txtTotal.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTotal.ForeColor = System.Drawing.Color.Black;
            this.txtTotal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Location = new System.Drawing.Point(346, 267);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotal.PlaceholderText = "";
            this.txtTotal.SelectedText = "";
            this.txtTotal.Size = new System.Drawing.Size(166, 25);
            this.txtTotal.TabIndex = 8;
            // 
            // txtAgrementFeesAmount
            // 
            this.txtAgrementFeesAmount.BackColor = System.Drawing.Color.Transparent;
            this.txtAgrementFeesAmount.BorderRadius = 5;
            this.txtAgrementFeesAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAgrementFeesAmount.DefaultText = "";
            this.txtAgrementFeesAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAgrementFeesAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAgrementFeesAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAgrementFeesAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAgrementFeesAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAgrementFeesAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAgrementFeesAmount.ForeColor = System.Drawing.Color.Black;
            this.txtAgrementFeesAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAgrementFeesAmount.Location = new System.Drawing.Point(207, 267);
            this.txtAgrementFeesAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAgrementFeesAmount.Name = "txtAgrementFeesAmount";
            this.txtAgrementFeesAmount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAgrementFeesAmount.PlaceholderText = "";
            this.txtAgrementFeesAmount.SelectedText = "";
            this.txtAgrementFeesAmount.Size = new System.Drawing.Size(133, 25);
            this.txtAgrementFeesAmount.TabIndex = 8;
            this.txtAgrementFeesAmount.TextChanged += new System.EventHandler(this.txtAgrementAmount_TextChanged);
            this.txtAgrementFeesAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAgrementFeesAmount_KeyPress);
            // 
            // txtAgrementAmount
            // 
            this.txtAgrementAmount.BackColor = System.Drawing.Color.Transparent;
            this.txtAgrementAmount.BorderRadius = 5;
            this.txtAgrementAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAgrementAmount.DefaultText = "";
            this.txtAgrementAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAgrementAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAgrementAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAgrementAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAgrementAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAgrementAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtAgrementAmount.ForeColor = System.Drawing.Color.Black;
            this.txtAgrementAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAgrementAmount.Location = new System.Drawing.Point(32, 267);
            this.txtAgrementAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAgrementAmount.Name = "txtAgrementAmount";
            this.txtAgrementAmount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAgrementAmount.PlaceholderText = "";
            this.txtAgrementAmount.SelectedText = "";
            this.txtAgrementAmount.Size = new System.Drawing.Size(169, 25);
            this.txtAgrementAmount.TabIndex = 8;
            this.txtAgrementAmount.TextChanged += new System.EventHandler(this.txtAgrementAmount_TextChanged);
            this.txtAgrementAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAgrementAmount_KeyPress);
            // 
            // dtpAgrementEndDate
            // 
            this.dtpAgrementEndDate.BackColor = System.Drawing.Color.Gainsboro;
            this.dtpAgrementEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpAgrementEndDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpAgrementEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAgrementEndDate.Location = new System.Drawing.Point(207, 195);
            this.dtpAgrementEndDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpAgrementEndDate.Name = "dtpAgrementEndDate";
            this.dtpAgrementEndDate.Size = new System.Drawing.Size(133, 23);
            this.dtpAgrementEndDate.TabIndex = 7;
            this.dtpAgrementEndDate.ValueChanged += new System.EventHandler(this.dtpAgrementStartDate_ValueChanged);
            // 
            // dtPrepaid
            // 
            this.dtPrepaid.BackColor = System.Drawing.Color.Gainsboro;
            this.dtPrepaid.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtPrepaid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtPrepaid.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPrepaid.Location = new System.Drawing.Point(209, 63);
            this.dtPrepaid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtPrepaid.Name = "dtPrepaid";
            this.dtPrepaid.Size = new System.Drawing.Size(133, 23);
            this.dtPrepaid.TabIndex = 7;
            // 
            // dtpAgrementStartDate
            // 
            this.dtpAgrementStartDate.BackColor = System.Drawing.Color.Gainsboro;
            this.dtpAgrementStartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpAgrementStartDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpAgrementStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAgrementStartDate.Location = new System.Drawing.Point(32, 195);
            this.dtpAgrementStartDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpAgrementStartDate.Name = "dtpAgrementStartDate";
            this.dtpAgrementStartDate.Size = new System.Drawing.Size(169, 23);
            this.dtpAgrementStartDate.TabIndex = 7;
            this.dtpAgrementStartDate.ValueChanged += new System.EventHandler(this.dtpAgrementStartDate_ValueChanged);
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.Transparent;
            this.txtCode.BorderRadius = 5;
            this.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCode.DefaultText = "";
            this.txtCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCode.Enabled = false;
            this.txtCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCode.ForeColor = System.Drawing.Color.Black;
            this.txtCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCode.Location = new System.Drawing.Point(32, 63);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCode.PlaceholderText = "";
            this.txtCode.ReadOnly = true;
            this.txtCode.SelectedText = "";
            this.txtCode.Size = new System.Drawing.Size(169, 25);
            this.txtCode.TabIndex = 3;
            // 
            // cmbCategory
            // 
            this.cmbCategory.BackColor = System.Drawing.Color.Transparent;
            this.cmbCategory.BorderRadius = 5;
            this.cmbCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCategory.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCategory.ItemHeight = 18;
            this.cmbCategory.Location = new System.Drawing.Point(346, 129);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(138, 24);
            this.cmbCategory.TabIndex = 4;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.Transparent;
            this.txtName.BorderRadius = 5;
            this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.DefaultText = "";
            this.txtName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtName.Location = new System.Drawing.Point(32, 129);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtName.PlaceholderText = "";
            this.txtName.SelectedText = "";
            this.txtName.Size = new System.Drawing.Size(308, 25);
            this.txtName.TabIndex = 3;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(32, 379);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 29);
            this.label12.TabIndex = 15;
            this.label12.Text = "Credit Account";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label10.Location = new System.Drawing.Point(346, 163);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(165, 29);
            this.label10.TabIndex = 9;
            this.label10.Text = "Total Days";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label4.Location = new System.Drawing.Point(32, 314);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 29);
            this.label4.TabIndex = 11;
            this.label4.Text = "Debit Account";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(346, 236);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 29);
            this.label5.TabIndex = 9;
            this.label5.Text = "Agreement Total";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(207, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 29);
            this.label1.TabIndex = 9;
            this.label1.Text = "Fees Amount";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label9.Location = new System.Drawing.Point(32, 236);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(139, 29);
            this.label9.TabIndex = 9;
            this.label9.Text = "Agreement Amount";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label11.Location = new System.Drawing.Point(209, 34);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 29);
            this.label11.TabIndex = 5;
            this.label11.Text = "Date";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label8.Location = new System.Drawing.Point(207, 163);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 29);
            this.label8.TabIndex = 5;
            this.label8.Text = "Agreement End Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label6.Location = new System.Drawing.Point(32, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 29);
            this.label6.TabIndex = 5;
            this.label6.Text = "Agreement Start Date";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(32, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "Code";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(32, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label7.Location = new System.Drawing.Point(346, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 29);
            this.label7.TabIndex = 5;
            this.label7.Text = "Category";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // headerUC1
            // 
            this.headerUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.headerUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerUC1.FormText = "";
            this.headerUC1.HeaderText = "";
            this.headerUC1.Location = new System.Drawing.Point(0, 0);
            this.headerUC1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.headerUC1.Name = "headerUC1";
            this.headerUC1.Size = new System.Drawing.Size(557, 37);
            this.headerUC1.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(2, 568);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(553, 2);
            this.panel5.TabIndex = 14;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(555, 37);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 533);
            this.panel4.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 533);
            this.panel3.TabIndex = 12;
            // 
            // frmViewPrepaidExpense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(557, 570);
            this.Controls.Add(this.guna2GroupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.headerUC1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmViewPrepaidExpense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Prepaid Expense";
            this.Load += new System.EventHandler(this.frmViewPrepaidExpense_Load);
            this.panel1.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private HeaderUC headerUC1;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2TextBox txtCode;
        public Guna.UI2.WinForms.Guna2ComboBox cmbCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpAgrementEndDate;
        private System.Windows.Forms.DateTimePicker dtpAgrementStartDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox txtAgrementFeesAmount;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtAgrementAmount;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2TextBox txtAccountCodeCredit;
        public Guna.UI2.WinForms.Guna2ComboBox cmbAccountNameCredit;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2TextBox txtAccountCodeDebit;
        public Guna.UI2.WinForms.Guna2ComboBox cmbAccountNameDebit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.LinkLabel lnkCategory;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2TextBox txtTotalDays;
        private Guna.UI2.WinForms.Guna2TextBox txtTotal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtPrepaid;
        private System.Windows.Forms.Label label11;
    }
}