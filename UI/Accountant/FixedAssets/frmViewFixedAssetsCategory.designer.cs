namespace YamyProject
{
    partial class frmViewFixedAssetsCategory
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtExpenceAccount = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbExpenceAccount = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtCreditAccountCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtAccountCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbDepreciationAccount = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbAssetsAccount = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.txtCategoryCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.cmbCategoryName = new System.Windows.Forms.ComboBox();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 265);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 33);
            this.panel1.TabIndex = 2;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnDelete.BorderRadius = 5;
            this.btnDelete.BorderThickness = 3;
            this.btnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDelete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Location = new System.Drawing.Point(236, 2);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(82, 28);
            this.btnDelete.TabIndex = 8;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.BorderRadius = 5;
            this.btnClose.BorderThickness = 1;
            this.btnClose.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClose.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClose.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClose.FillColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(413, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 27);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.BorderRadius = 5;
            this.btnSave.BorderThickness = 3;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(324, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(82, 28);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(10, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 24);
            this.label1.TabIndex = 18;
            this.label1.Text = "Category Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(2, 298);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(498, 2);
            this.panel5.TabIndex = 24;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(500, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 300);
            this.panel4.TabIndex = 23;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 300);
            this.panel3.TabIndex = 22;
            // 
            // txtExpenceAccount
            // 
            this.txtExpenceAccount.BorderRadius = 7;
            this.txtExpenceAccount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtExpenceAccount.DefaultText = "";
            this.txtExpenceAccount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtExpenceAccount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtExpenceAccount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtExpenceAccount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtExpenceAccount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtExpenceAccount.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExpenceAccount.ForeColor = System.Drawing.Color.Black;
            this.txtExpenceAccount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtExpenceAccount.Location = new System.Drawing.Point(320, 230);
            this.txtExpenceAccount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExpenceAccount.Name = "txtExpenceAccount";
            this.txtExpenceAccount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtExpenceAccount.PlaceholderText = "";
            this.txtExpenceAccount.ReadOnly = true;
            this.txtExpenceAccount.SelectedText = "";
            this.txtExpenceAccount.Size = new System.Drawing.Size(173, 24);
            this.txtExpenceAccount.TabIndex = 233;
            this.txtExpenceAccount.TextChanged += new System.EventHandler(this.txtExpenceAccount_TextChanged);
            // 
            // cmbExpenceAccount
            // 
            this.cmbExpenceAccount.BackColor = System.Drawing.Color.Transparent;
            this.cmbExpenceAccount.BorderRadius = 7;
            this.cmbExpenceAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbExpenceAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpenceAccount.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbExpenceAccount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbExpenceAccount.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.cmbExpenceAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbExpenceAccount.ItemHeight = 18;
            this.cmbExpenceAccount.Location = new System.Drawing.Point(10, 231);
            this.cmbExpenceAccount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbExpenceAccount.Name = "cmbExpenceAccount";
            this.cmbExpenceAccount.Size = new System.Drawing.Size(308, 24);
            this.cmbExpenceAccount.TabIndex = 231;
            this.cmbExpenceAccount.SelectedIndexChanged += new System.EventHandler(this.cmbExpenceAccount_SelectedIndexChanged);
            // 
            // txtCreditAccountCode
            // 
            this.txtCreditAccountCode.BorderRadius = 7;
            this.txtCreditAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCreditAccountCode.DefaultText = "";
            this.txtCreditAccountCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCreditAccountCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCreditAccountCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCreditAccountCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCreditAccountCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCreditAccountCode.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreditAccountCode.ForeColor = System.Drawing.Color.Black;
            this.txtCreditAccountCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCreditAccountCode.Location = new System.Drawing.Point(320, 180);
            this.txtCreditAccountCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCreditAccountCode.Name = "txtCreditAccountCode";
            this.txtCreditAccountCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCreditAccountCode.PlaceholderText = "";
            this.txtCreditAccountCode.ReadOnly = true;
            this.txtCreditAccountCode.SelectedText = "";
            this.txtCreditAccountCode.Size = new System.Drawing.Size(173, 24);
            this.txtCreditAccountCode.TabIndex = 228;
            this.txtCreditAccountCode.TextChanged += new System.EventHandler(this.txtCreditAccountCode_TextChanged);
            // 
            // txtAccountCode
            // 
            this.txtAccountCode.BorderRadius = 7;
            this.txtAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAccountCode.DefaultText = "";
            this.txtAccountCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAccountCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAccountCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAccountCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAccountCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAccountCode.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAccountCode.ForeColor = System.Drawing.Color.Black;
            this.txtAccountCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAccountCode.Location = new System.Drawing.Point(320, 136);
            this.txtAccountCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAccountCode.Name = "txtAccountCode";
            this.txtAccountCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAccountCode.PlaceholderText = "";
            this.txtAccountCode.SelectedText = "";
            this.txtAccountCode.Size = new System.Drawing.Size(173, 24);
            this.txtAccountCode.TabIndex = 229;
            this.txtAccountCode.TextChanged += new System.EventHandler(this.txtAccountCode_TextChanged);
            // 
            // cmbDepreciationAccount
            // 
            this.cmbDepreciationAccount.BackColor = System.Drawing.Color.Transparent;
            this.cmbDepreciationAccount.BorderRadius = 7;
            this.cmbDepreciationAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDepreciationAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDepreciationAccount.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDepreciationAccount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDepreciationAccount.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDepreciationAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDepreciationAccount.ItemHeight = 18;
            this.cmbDepreciationAccount.Location = new System.Drawing.Point(10, 180);
            this.cmbDepreciationAccount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbDepreciationAccount.Name = "cmbDepreciationAccount";
            this.cmbDepreciationAccount.Size = new System.Drawing.Size(308, 24);
            this.cmbDepreciationAccount.TabIndex = 224;
            this.cmbDepreciationAccount.SelectedIndexChanged += new System.EventHandler(this.cmbDepreciationAccount_SelectedIndexChanged);
            // 
            // cmbAssetsAccount
            // 
            this.cmbAssetsAccount.BackColor = System.Drawing.Color.Transparent;
            this.cmbAssetsAccount.BorderRadius = 7;
            this.cmbAssetsAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbAssetsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssetsAccount.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAssetsAccount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbAssetsAccount.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAssetsAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAssetsAccount.ItemHeight = 18;
            this.cmbAssetsAccount.Location = new System.Drawing.Point(10, 136);
            this.cmbAssetsAccount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAssetsAccount.Name = "cmbAssetsAccount";
            this.cmbAssetsAccount.Size = new System.Drawing.Size(308, 24);
            this.cmbAssetsAccount.TabIndex = 225;
            this.cmbAssetsAccount.SelectedIndexChanged += new System.EventHandler(this.cmbAssetsAccount_SelectedIndexChanged);
            // 
            // label19
            // 
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label19.Location = new System.Drawing.Point(8, 205);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(163, 29);
            this.label19.TabIndex = 226;
            this.label19.Text = "Depreciation Expence";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(10, 156);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(163, 29);
            this.label12.TabIndex = 227;
            this.label12.Text = "Accumulated Depreciation";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label17.Location = new System.Drawing.Point(320, 157);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(91, 29);
            this.label17.TabIndex = 222;
            this.label17.Text = "Account Code";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label15.Location = new System.Drawing.Point(320, 112);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 29);
            this.label15.TabIndex = 223;
            this.label15.Text = "Account Code";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label21.Location = new System.Drawing.Point(320, 205);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(91, 29);
            this.label21.TabIndex = 230;
            this.label21.Text = "Account Code";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label20.Location = new System.Drawing.Point(9, 113);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(163, 29);
            this.label20.TabIndex = 232;
            this.label20.Text = "Fixed Assets Account";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCategoryCode
            // 
            this.txtCategoryCode.BorderRadius = 7;
            this.txtCategoryCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCategoryCode.DefaultText = "";
            this.txtCategoryCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCategoryCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCategoryCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCategoryCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCategoryCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCategoryCode.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCategoryCode.ForeColor = System.Drawing.Color.Black;
            this.txtCategoryCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCategoryCode.Location = new System.Drawing.Point(320, 84);
            this.txtCategoryCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCategoryCode.Name = "txtCategoryCode";
            this.txtCategoryCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCategoryCode.PlaceholderText = "";
            this.txtCategoryCode.ReadOnly = true;
            this.txtCategoryCode.SelectedText = "";
            this.txtCategoryCode.Size = new System.Drawing.Size(173, 24);
            this.txtCategoryCode.TabIndex = 236;
            this.txtCategoryCode.Visible = false;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(320, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 29);
            this.label2.TabIndex = 235;
            this.label2.Text = "Category Code";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Visible = false;
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockForm = true;
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.1D;
            this.guna2DragControl1.DragEndTransparencyValue = 0.1D;
            this.guna2DragControl1.TransparentWhileDrag = false;
            // 
            // cmbCategoryName
            // 
            this.cmbCategoryName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCategoryName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCategoryName.ItemHeight = 13;
            this.cmbCategoryName.Location = new System.Drawing.Point(13, 84);
            this.cmbCategoryName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbCategoryName.Name = "cmbCategoryName";
            this.cmbCategoryName.Size = new System.Drawing.Size(302, 21);
            this.cmbCategoryName.TabIndex = 237;
            this.cmbCategoryName.SelectedIndexChanged += new System.EventHandler(this.cmbCategoryName_SelectedIndexChanged);
            // 
            // headerUC1
            // 
            this.headerUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(41)))), ((int)(((byte)(83)))));
            this.headerUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerUC1.FormText = "";
            this.headerUC1.HeaderText = "";
            this.headerUC1.Location = new System.Drawing.Point(2, 0);
            this.headerUC1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.headerUC1.Name = "headerUC1";
            this.headerUC1.Size = new System.Drawing.Size(498, 37);
            this.headerUC1.TabIndex = 238;
            // 
            // frmViewFixedAssetsCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(502, 300);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.cmbCategoryName);
            this.Controls.Add(this.txtCategoryCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtExpenceAccount);
            this.Controls.Add(this.cmbExpenceAccount);
            this.Controls.Add(this.txtCreditAccountCode);
            this.Controls.Add(this.txtAccountCode);
            this.Controls.Add(this.cmbDepreciationAccount);
            this.Controls.Add(this.cmbAssetsAccount);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmViewFixedAssetsCategory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fixed Asset Category";
            this.Load += new System.EventHandler(this.frmViewFixedAssetsCategory_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2TextBox txtExpenceAccount;
        public Guna.UI2.WinForms.Guna2ComboBox cmbExpenceAccount;
        private Guna.UI2.WinForms.Guna2TextBox txtCreditAccountCode;
        private Guna.UI2.WinForms.Guna2TextBox txtAccountCode;
        public Guna.UI2.WinForms.Guna2ComboBox cmbDepreciationAccount;
        public Guna.UI2.WinForms.Guna2ComboBox cmbAssetsAccount;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private Guna.UI2.WinForms.Guna2TextBox txtCategoryCode;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2Button btnDelete;
        private System.Windows.Forms.ComboBox cmbCategoryName;
        private HeaderUC headerUC1;
    }
}