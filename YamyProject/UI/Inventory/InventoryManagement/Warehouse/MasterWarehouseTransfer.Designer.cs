namespace YamyProject
{
    partial class MasterWarehouseTransfer
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnHistory = new Guna.UI2.WinForms.Guna2Button();
            this.cmbWareFrom = new Guna.UI2.WinForms.Guna2ComboBox();
            this.Label25 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbWareTo = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnToFrom = new Guna.UI2.WinForms.Guna2Button();
            this.btnFromTo = new Guna.UI2.WinForms.Guna2Button();
            this.txtQty = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtSearchFrom = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtSearchTo = new Guna.UI2.WinForms.Guna2TextBox();
            this.dgvFrom = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dgvTo = new Guna.UI2.WinForms.Guna2DataGridView();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.dtpTransferDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTo)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnHistory);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 607);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1089, 31);
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
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(961, 2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(119, 27);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHistory.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnHistory.BorderRadius = 5;
            this.btnHistory.BorderThickness = 3;
            this.btnHistory.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnHistory.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnHistory.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHistory.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHistory.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnHistory.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHistory.ForeColor = System.Drawing.Color.White;
            this.btnHistory.Location = new System.Drawing.Point(824, 2);
            this.btnHistory.Margin = new System.Windows.Forms.Padding(2);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(132, 27);
            this.btnHistory.TabIndex = 6;
            this.btnHistory.Text = "Transfer History";
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // cmbWareFrom
            // 
            this.cmbWareFrom.BackColor = System.Drawing.Color.Transparent;
            this.cmbWareFrom.BorderRadius = 5;
            this.cmbWareFrom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWareFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWareFrom.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWareFrom.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWareFrom.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbWareFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbWareFrom.ItemHeight = 18;
            this.cmbWareFrom.Location = new System.Drawing.Point(16, 66);
            this.cmbWareFrom.Margin = new System.Windows.Forms.Padding(2);
            this.cmbWareFrom.Name = "cmbWareFrom";
            this.cmbWareFrom.Size = new System.Drawing.Size(456, 24);
            this.cmbWareFrom.TabIndex = 121;
            this.cmbWareFrom.SelectedIndexChanged += new System.EventHandler(this.cmbWareFrom_SelectedIndexChanged);
            // 
            // Label25
            // 
            this.Label25.AutoSize = true;
            this.Label25.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label25.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.Label25.Location = new System.Drawing.Point(18, 49);
            this.Label25.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label25.Name = "Label25";
            this.Label25.Size = new System.Drawing.Size(95, 13);
            this.Label25.TabIndex = 120;
            this.Label25.Text = "From Warehouse";
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(2, 638);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1089, 2);
            this.panel5.TabIndex = 124;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1091, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 640);
            this.panel4.TabIndex = 123;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 640);
            this.panel3.TabIndex = 122;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(619, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 120;
            this.label1.Text = "To Warehouse";
            // 
            // cmbWareTo
            // 
            this.cmbWareTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWareTo.BackColor = System.Drawing.Color.Transparent;
            this.cmbWareTo.BorderRadius = 5;
            this.cmbWareTo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWareTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWareTo.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWareTo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWareTo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbWareTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbWareTo.ItemHeight = 18;
            this.cmbWareTo.Location = new System.Drawing.Point(622, 68);
            this.cmbWareTo.Margin = new System.Windows.Forms.Padding(2);
            this.cmbWareTo.Name = "cmbWareTo";
            this.cmbWareTo.Size = new System.Drawing.Size(456, 24);
            this.cmbWareTo.TabIndex = 121;
            this.cmbWareTo.SelectedIndexChanged += new System.EventHandler(this.cmbWareTo_SelectedIndexChanged);
            // 
            // btnToFrom
            // 
            this.btnToFrom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnToFrom.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnToFrom.BorderRadius = 5;
            this.btnToFrom.BorderThickness = 1;
            this.btnToFrom.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnToFrom.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnToFrom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnToFrom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnToFrom.FillColor = System.Drawing.Color.White;
            this.btnToFrom.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnToFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnToFrom.Image = global::YamyProject.Properties.Resources.Double_Left2;
            this.btnToFrom.Location = new System.Drawing.Point(477, 357);
            this.btnToFrom.Margin = new System.Windows.Forms.Padding(2);
            this.btnToFrom.Name = "btnToFrom";
            this.btnToFrom.Size = new System.Drawing.Size(141, 27);
            this.btnToFrom.TabIndex = 8;
            this.btnToFrom.Click += new System.EventHandler(this.btnToFrom_Click);
            // 
            // btnFromTo
            // 
            this.btnFromTo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFromTo.BorderColor = System.Drawing.Color.MidnightBlue;
            this.btnFromTo.BorderRadius = 5;
            this.btnFromTo.BorderThickness = 1;
            this.btnFromTo.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFromTo.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFromTo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFromTo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFromTo.FillColor = System.Drawing.Color.White;
            this.btnFromTo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.btnFromTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnFromTo.Image = global::YamyProject.Properties.Resources.Double_Right;
            this.btnFromTo.Location = new System.Drawing.Point(477, 316);
            this.btnFromTo.Margin = new System.Windows.Forms.Padding(2);
            this.btnFromTo.Name = "btnFromTo";
            this.btnFromTo.Size = new System.Drawing.Size(141, 27);
            this.btnFromTo.TabIndex = 8;
            this.btnFromTo.Click += new System.EventHandler(this.btnFromTo_Click);
            // 
            // txtQty
            // 
            this.txtQty.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtQty.BorderRadius = 5;
            this.txtQty.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtQty.DefaultText = "";
            this.txtQty.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtQty.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtQty.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtQty.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtQty.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtQty.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtQty.ForeColor = System.Drawing.Color.Black;
            this.txtQty.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtQty.Location = new System.Drawing.Point(477, 268);
            this.txtQty.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtQty.Name = "txtQty";
            this.txtQty.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtQty.PlaceholderText = "QTY";
            this.txtQty.SelectedText = "";
            this.txtQty.Size = new System.Drawing.Size(141, 28);
            this.txtQty.TabIndex = 126;
            this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // txtSearchFrom
            // 
            this.txtSearchFrom.BorderRadius = 5;
            this.txtSearchFrom.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchFrom.DefaultText = "";
            this.txtSearchFrom.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearchFrom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearchFrom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchFrom.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchFrom.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchFrom.ForeColor = System.Drawing.Color.Black;
            this.txtSearchFrom.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchFrom.Location = new System.Drawing.Point(14, 98);
            this.txtSearchFrom.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtSearchFrom.Name = "txtSearchFrom";
            this.txtSearchFrom.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtSearchFrom.PlaceholderText = "Search By Item Name, Code";
            this.txtSearchFrom.SelectedText = "";
            this.txtSearchFrom.Size = new System.Drawing.Size(456, 24);
            this.txtSearchFrom.TabIndex = 126;
            this.txtSearchFrom.TextChanged += new System.EventHandler(this.txtSearchFrom_TextChanged);
            // 
            // txtSearchTo
            // 
            this.txtSearchTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchTo.BorderRadius = 5;
            this.txtSearchTo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchTo.DefaultText = "";
            this.txtSearchTo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtSearchTo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtSearchTo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchTo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtSearchTo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtSearchTo.ForeColor = System.Drawing.Color.Black;
            this.txtSearchTo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSearchTo.Location = new System.Drawing.Point(622, 98);
            this.txtSearchTo.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtSearchTo.Name = "txtSearchTo";
            this.txtSearchTo.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtSearchTo.PlaceholderText = "Search By Item Name, Code";
            this.txtSearchTo.SelectedText = "";
            this.txtSearchTo.Size = new System.Drawing.Size(456, 24);
            this.txtSearchTo.TabIndex = 126;
            this.txtSearchTo.TextChanged += new System.EventHandler(this.txtSearchTo_TextChanged);
            // 
            // dgvFrom
            // 
            this.dgvFrom.AllowUserToAddRows = false;
            this.dgvFrom.AllowUserToDeleteRows = false;
            this.dgvFrom.AllowUserToResizeColumns = false;
            this.dgvFrom.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvFrom.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvFrom.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvFrom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvFrom.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFrom.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFrom.ColumnHeadersHeight = 30;
            this.dgvFrom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(234)))), ((int)(((byte)(208)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFrom.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFrom.GridColor = System.Drawing.Color.White;
            this.dgvFrom.Location = new System.Drawing.Point(14, 134);
            this.dgvFrom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dgvFrom.MultiSelect = false;
            this.dgvFrom.Name = "dgvFrom";
            this.dgvFrom.ReadOnly = true;
            this.dgvFrom.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvFrom.RowHeadersVisible = false;
            this.dgvFrom.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(234)))), ((int)(((byte)(208)))));
            this.dgvFrom.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvFrom.RowTemplate.Height = 30;
            this.dgvFrom.Size = new System.Drawing.Size(456, 468);
            this.dgvFrom.TabIndex = 127;
            this.dgvFrom.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvFrom.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvFrom.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvFrom.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvFrom.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvFrom.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvFrom.ThemeStyle.GridColor = System.Drawing.Color.White;
            this.dgvFrom.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvFrom.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvFrom.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvFrom.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvFrom.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvFrom.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvFrom.ThemeStyle.ReadOnly = true;
            this.dgvFrom.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvFrom.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvFrom.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvFrom.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvFrom.ThemeStyle.RowsStyle.Height = 30;
            this.dgvFrom.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(234)))), ((int)(((byte)(208)))));
            this.dgvFrom.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
            // 
            // dgvTo
            // 
            this.dgvTo.AllowUserToAddRows = false;
            this.dgvTo.AllowUserToDeleteRows = false;
            this.dgvTo.AllowUserToResizeColumns = false;
            this.dgvTo.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvTo.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvTo.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 8F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvTo.ColumnHeadersHeight = 30;
            this.dgvTo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 8F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(234)))), ((int)(((byte)(208)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTo.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvTo.GridColor = System.Drawing.Color.White;
            this.dgvTo.Location = new System.Drawing.Point(622, 134);
            this.dgvTo.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dgvTo.MultiSelect = false;
            this.dgvTo.Name = "dgvTo";
            this.dgvTo.ReadOnly = true;
            this.dgvTo.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTo.RowHeadersVisible = false;
            this.dgvTo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(234)))), ((int)(((byte)(208)))));
            this.dgvTo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvTo.RowTemplate.Height = 30;
            this.dgvTo.Size = new System.Drawing.Size(456, 468);
            this.dgvTo.TabIndex = 127;
            this.dgvTo.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvTo.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvTo.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvTo.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvTo.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvTo.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvTo.ThemeStyle.GridColor = System.Drawing.Color.White;
            this.dgvTo.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.White;
            this.dgvTo.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvTo.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTo.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvTo.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvTo.ThemeStyle.HeaderStyle.Height = 30;
            this.dgvTo.ThemeStyle.ReadOnly = true;
            this.dgvTo.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvTo.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvTo.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvTo.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvTo.ThemeStyle.RowsStyle.Height = 30;
            this.dgvTo.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(234)))), ((int)(((byte)(208)))));
            this.dgvTo.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.Black;
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
            this.headerUC1.Size = new System.Drawing.Size(1089, 37);
            this.headerUC1.TabIndex = 238;
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button1.BorderColor = System.Drawing.Color.Transparent;
            this.guna2Button1.BorderRadius = 3;
            this.guna2Button1.BorderThickness = 1;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Image = global::YamyProject.Properties.Resources.Subtract;
            this.guna2Button1.ImageSize = new System.Drawing.Size(15, 15);
            this.guna2Button1.Location = new System.Drawing.Point(1027, 3);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(15, 15);
            this.guna2Button1.TabIndex = 32;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2Button4
            // 
            this.guna2Button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button4.BorderColor = System.Drawing.Color.Transparent;
            this.guna2Button4.BorderRadius = 3;
            this.guna2Button4.BorderThickness = 1;
            this.guna2Button4.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button4.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button4.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button4.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button4.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button4.ForeColor = System.Drawing.Color.White;
            this.guna2Button4.Image = global::YamyProject.Properties.Resources.Restore_Down1;
            this.guna2Button4.ImageSize = new System.Drawing.Size(15, 15);
            this.guna2Button4.Location = new System.Drawing.Point(1050, 3);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.Size = new System.Drawing.Size(15, 15);
            this.guna2Button4.TabIndex = 31;
            this.guna2Button4.Click += new System.EventHandler(this.guna2Button4_Click);
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1071, 4);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(15, 15);
            this.guna2ControlBox1.TabIndex = 0;
            // 
            // dtpTransferDate
            // 
            this.dtpTransferDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTransferDate.Location = new System.Drawing.Point(477, 68);
            this.dtpTransferDate.Name = "dtpTransferDate";
            this.dtpTransferDate.Size = new System.Drawing.Size(140, 20);
            this.dtpTransferDate.TabIndex = 144;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(474, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 145;
            this.label2.Text = "Date";
            // 
            // MasterWarehouseTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1093, 640);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtpTransferDate);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.dgvTo);
            this.Controls.Add(this.dgvFrom);
            this.Controls.Add(this.txtSearchTo);
            this.Controls.Add(this.txtSearchFrom);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.btnFromTo);
            this.Controls.Add(this.btnToFrom);
            this.Controls.Add(this.cmbWareTo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbWareFrom);
            this.Controls.Add(this.Label25);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MasterWarehouseTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Customer - Add New Category";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterWarehouseTransfer_FormClosing);
            this.Load += new System.EventHandler(this.MasterWarehouseTransfer_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2ComboBox cmbWareFrom;
        internal System.Windows.Forms.Label Label25;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbWareTo;
        private Guna.UI2.WinForms.Guna2Button btnToFrom;
        private Guna.UI2.WinForms.Guna2Button btnFromTo;
        private Guna.UI2.WinForms.Guna2TextBox txtQty;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchFrom;
        private Guna.UI2.WinForms.Guna2TextBox txtSearchTo;
        private Guna.UI2.WinForms.Guna2DataGridView dgvFrom;
        private Guna.UI2.WinForms.Guna2DataGridView dgvTo;
        private Guna.UI2.WinForms.Guna2Button btnHistory;
        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private System.Windows.Forms.DateTimePicker dtpTransferDate;
        internal System.Windows.Forms.Label label2;
    }
}