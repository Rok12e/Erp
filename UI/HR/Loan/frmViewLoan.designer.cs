namespace YamyProject
{
    partial class frmViewLoan
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.txtAccountCodeDebit = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbAccountNameDebit = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgvLoan = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtAccountCodeCredit = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbAccountNameCredit = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.dtpRequestDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEmployeeName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtEmployeeCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtInstallments = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtRequestAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtDescription = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.panel1.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoan)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.BorderRadius = 5;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(794, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 27);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.BorderRadius = 5;
            this.btnClose.BorderThickness = 1;
            this.btnClose.FillColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(894, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(94, 27);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 637);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 33);
            this.panel1.TabIndex = 2;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.BorderThickness = 0;
            this.guna2GroupBox1.Controls.Add(this.txtAccountCodeDebit);
            this.guna2GroupBox1.Controls.Add(this.cmbAccountNameDebit);
            this.guna2GroupBox1.Controls.Add(this.label5);
            this.guna2GroupBox1.Controls.Add(this.panel2);
            this.guna2GroupBox1.Controls.Add(this.panel3);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(0, 37);
            this.guna2GroupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(1000, 582);
            this.guna2GroupBox1.TabIndex = 4;
            this.guna2GroupBox1.Text = "New Loan";
            this.guna2GroupBox1.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // txtAccountCodeDebit
            // 
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
            this.txtAccountCodeDebit.Location = new System.Drawing.Point(824, 59);
            this.txtAccountCodeDebit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccountCodeDebit.Name = "txtAccountCodeDebit";
            this.txtAccountCodeDebit.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAccountCodeDebit.PlaceholderText = "";
            this.txtAccountCodeDebit.SelectedText = "";
            this.txtAccountCodeDebit.Size = new System.Drawing.Size(166, 24);
            this.txtAccountCodeDebit.TabIndex = 22;
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
            this.cmbAccountNameDebit.Location = new System.Drawing.Point(510, 59);
            this.cmbAccountNameDebit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAccountNameDebit.Name = "cmbAccountNameDebit";
            this.cmbAccountNameDebit.Size = new System.Drawing.Size(306, 24);
            this.cmbAccountNameDebit.TabIndex = 20;
            this.cmbAccountNameDebit.SelectedIndexChanged += new System.EventHandler(this.cmbAccountNameDebit_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(510, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 26);
            this.label5.TabIndex = 21;
            this.label5.Text = "Debit Account";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgvLoan);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 308);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 274);
            this.panel2.TabIndex = 10;
            // 
            // dgvLoan
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvLoan.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLoan.BackgroundColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLoan.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLoan.ColumnHeadersHeight = 19;
            this.dgvLoan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLoan.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLoan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLoan.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvLoan.Location = new System.Drawing.Point(0, 0);
            this.dgvLoan.Name = "dgvLoan";
            this.dgvLoan.RowHeadersVisible = false;
            this.dgvLoan.RowHeadersWidth = 51;
            this.dgvLoan.Size = new System.Drawing.Size(1000, 274);
            this.dgvLoan.TabIndex = 9;
            this.dgvLoan.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
            this.dgvLoan.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLoan.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvLoan.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvLoan.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvLoan.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvLoan.ThemeStyle.BackColor = System.Drawing.Color.Silver;
            this.dgvLoan.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvLoan.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(234)))), ((int)(((byte)(237)))));
            this.dgvLoan.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLoan.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dgvLoan.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvLoan.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvLoan.ThemeStyle.HeaderStyle.Height = 19;
            this.dgvLoan.ThemeStyle.ReadOnly = false;
            this.dgvLoan.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvLoan.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvLoan.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.dgvLoan.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvLoan.ThemeStyle.RowsStyle.Height = 22;
            this.dgvLoan.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(241)))), ((int)(((byte)(243)))));
            this.dgvLoan.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel3.Controls.Add(this.txtAccountCodeCredit);
            this.panel3.Controls.Add(this.cmbAccountNameCredit);
            this.panel3.Controls.Add(this.txtCode);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.dtpStartDate);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.dtpEndDate);
            this.panel3.Controls.Add(this.dtpRequestDate);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.cmbEmployeeName);
            this.panel3.Controls.Add(this.txtEmployeeCode);
            this.panel3.Controls.Add(this.txtInstallments);
            this.panel3.Controls.Add(this.txtRequestAmount);
            this.panel3.Controls.Add(this.txtDescription);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1000, 278);
            this.panel3.TabIndex = 26;
            // 
            // txtAccountCodeCredit
            // 
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
            this.txtAccountCodeCredit.Location = new System.Drawing.Point(824, 94);
            this.txtAccountCodeCredit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccountCodeCredit.Name = "txtAccountCodeCredit";
            this.txtAccountCodeCredit.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAccountCodeCredit.PlaceholderText = "";
            this.txtAccountCodeCredit.SelectedText = "";
            this.txtAccountCodeCredit.Size = new System.Drawing.Size(166, 24);
            this.txtAccountCodeCredit.TabIndex = 25;
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
            this.cmbAccountNameCredit.Location = new System.Drawing.Point(510, 94);
            this.cmbAccountNameCredit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAccountNameCredit.Name = "cmbAccountNameCredit";
            this.cmbAccountNameCredit.Size = new System.Drawing.Size(306, 24);
            this.cmbAccountNameCredit.TabIndex = 23;
            this.cmbAccountNameCredit.SelectedIndexChanged += new System.EventHandler(this.cmbAccountNameCredit_SelectedIndexChanged);
            // 
            // txtCode
            // 
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
            this.txtCode.Location = new System.Drawing.Point(320, 29);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCode.PlaceholderText = "Request No";
            this.txtCode.SelectedText = "";
            this.txtCode.Size = new System.Drawing.Size(184, 24);
            this.txtCode.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(21, 230);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Start Date";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.BackColor = System.Drawing.Color.Gainsboro;
            this.dtpStartDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpStartDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Location = new System.Drawing.Point(87, 234);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(153, 23);
            this.dtpStartDate.TabIndex = 6;
            this.dtpStartDate.ValueChanged += new System.EventHandler(this.txtInstallments_TextChanged);
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(510, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 29);
            this.label12.TabIndex = 24;
            this.label12.Text = "Credit Account";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label4.Location = new System.Drawing.Point(244, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 29);
            this.label4.TabIndex = 7;
            this.label4.Text = "End Date";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.BackColor = System.Drawing.Color.Gainsboro;
            this.dtpEndDate.Enabled = false;
            this.dtpEndDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpEndDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Location = new System.Drawing.Point(310, 234);
            this.dtpEndDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(153, 23);
            this.dtpEndDate.TabIndex = 8;
            // 
            // dtpRequestDate
            // 
            this.dtpRequestDate.BackColor = System.Drawing.Color.Gainsboro;
            this.dtpRequestDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpRequestDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpRequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpRequestDate.Location = new System.Drawing.Point(139, 29);
            this.dtpRequestDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpRequestDate.Name = "dtpRequestDate";
            this.dtpRequestDate.Size = new System.Drawing.Size(153, 23);
            this.dtpRequestDate.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(62, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 29);
            this.label2.TabIndex = 17;
            this.label2.Text = "Employee Name";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbEmployeeName
            // 
            this.cmbEmployeeName.BackColor = System.Drawing.Color.Transparent;
            this.cmbEmployeeName.BorderRadius = 5;
            this.cmbEmployeeName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEmployeeName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmployeeName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbEmployeeName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbEmployeeName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbEmployeeName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbEmployeeName.ItemHeight = 18;
            this.cmbEmployeeName.Location = new System.Drawing.Point(159, 94);
            this.cmbEmployeeName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbEmployeeName.Name = "cmbEmployeeName";
            this.cmbEmployeeName.Size = new System.Drawing.Size(202, 24);
            this.cmbEmployeeName.TabIndex = 16;
            this.cmbEmployeeName.SelectedIndexChanged += new System.EventHandler(this.cmbEmployeeName_SelectedIndexChanged);
            // 
            // txtEmployeeCode
            // 
            this.txtEmployeeCode.BorderRadius = 5;
            this.txtEmployeeCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmployeeCode.DefaultText = "";
            this.txtEmployeeCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtEmployeeCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtEmployeeCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmployeeCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtEmployeeCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmployeeCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtEmployeeCode.ForeColor = System.Drawing.Color.Black;
            this.txtEmployeeCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtEmployeeCode.Location = new System.Drawing.Point(367, 94);
            this.txtEmployeeCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmployeeCode.Name = "txtEmployeeCode";
            this.txtEmployeeCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtEmployeeCode.PlaceholderText = "Employee Code";
            this.txtEmployeeCode.SelectedText = "";
            this.txtEmployeeCode.Size = new System.Drawing.Size(137, 24);
            this.txtEmployeeCode.TabIndex = 15;
            // 
            // txtInstallments
            // 
            this.txtInstallments.BorderRadius = 5;
            this.txtInstallments.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtInstallments.DefaultText = "";
            this.txtInstallments.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtInstallments.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtInstallments.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtInstallments.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtInstallments.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtInstallments.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtInstallments.ForeColor = System.Drawing.Color.Black;
            this.txtInstallments.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtInstallments.Location = new System.Drawing.Point(309, 143);
            this.txtInstallments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInstallments.Name = "txtInstallments";
            this.txtInstallments.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtInstallments.PlaceholderText = "Installments";
            this.txtInstallments.SelectedText = "";
            this.txtInstallments.Size = new System.Drawing.Size(195, 24);
            this.txtInstallments.TabIndex = 11;
            this.txtInstallments.TextChanged += new System.EventHandler(this.txtInstallments_TextChanged);
            this.txtInstallments.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInstallments_KeyPress);
            // 
            // txtRequestAmount
            // 
            this.txtRequestAmount.BorderRadius = 5;
            this.txtRequestAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtRequestAmount.DefaultText = "";
            this.txtRequestAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtRequestAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtRequestAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRequestAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtRequestAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRequestAmount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtRequestAmount.ForeColor = System.Drawing.Color.Black;
            this.txtRequestAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtRequestAmount.Location = new System.Drawing.Point(65, 143);
            this.txtRequestAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRequestAmount.Name = "txtRequestAmount";
            this.txtRequestAmount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtRequestAmount.PlaceholderText = "Request Amount";
            this.txtRequestAmount.SelectedText = "";
            this.txtRequestAmount.Size = new System.Drawing.Size(227, 24);
            this.txtRequestAmount.TabIndex = 13;
            this.txtRequestAmount.TextChanged += new System.EventHandler(this.txtInstallments_TextChanged);
            this.txtRequestAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRequestAmount_KeyPress);
            // 
            // txtDescription
            // 
            this.txtDescription.BorderRadius = 5;
            this.txtDescription.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDescription.DefaultText = "";
            this.txtDescription.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDescription.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDescription.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDescription.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDescription.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtDescription.ForeColor = System.Drawing.Color.Black;
            this.txtDescription.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDescription.Location = new System.Drawing.Point(65, 175);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtDescription.PlaceholderText = "Description";
            this.txtDescription.SelectedText = "";
            this.txtDescription.Size = new System.Drawing.Size(439, 51);
            this.txtDescription.TabIndex = 12;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtInstallments_TextChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(59, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 29);
            this.label3.TabIndex = 18;
            this.label3.Text = "Request Date";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.headerUC1.Size = new System.Drawing.Size(1000, 37);
            this.headerUC1.TabIndex = 0;
            // 
            // frmViewLoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 670);
            this.Controls.Add(this.guna2GroupBox1);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmViewLoan";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Loan";
            this.Load += new System.EventHandler(this.frmViewLoan_Load);
            this.panel1.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLoan)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2DataGridView dgvLoan;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpRequestDate;
        public Guna.UI2.WinForms.Guna2ComboBox cmbEmployeeName;
        private Guna.UI2.WinForms.Guna2TextBox txtInstallments;
        private Guna.UI2.WinForms.Guna2TextBox txtDescription;
        private Guna.UI2.WinForms.Guna2TextBox txtRequestAmount;
        private Guna.UI2.WinForms.Guna2TextBox txtCode;
        private Guna.UI2.WinForms.Guna2TextBox txtEmployeeCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2TextBox txtAccountCodeDebit;
        public Guna.UI2.WinForms.Guna2ComboBox cmbAccountNameDebit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2TextBox txtAccountCodeCredit;
        public Guna.UI2.WinForms.Guna2ComboBox cmbAccountNameCredit;
        private System.Windows.Forms.Label label12;
    }
}