using System.ComponentModel;
using System.Windows.Forms;

namespace YamyProject
{
    partial class frmViewPettyCashVoucher
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.txtAmountInWord = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtcostcenter = new Guna.UI2.WinForms.Guna2GroupBox();
            this.cmbPettyCash = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNo = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dtOpen = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvInv = new Guna.UI2.WinForms.Guna2DataGridView();
            this.no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.humId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CostCenter = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtId = new System.Windows.Forms.TextBox();
            this.txtPVCode = new System.Windows.Forms.TextBox();
            this.BtnJournal = new Guna.UI2.WinForms.Guna2Button();
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.btnPrevious = new Guna.UI2.WinForms.Guna2Button();
            this.guna2VSeparator12 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.BtnClear = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton15 = new Guna.UI2.WinForms.Guna2TileButton();
            this.BtnSaveNew = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator9 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.BtnDelete = new Guna.UI2.WinForms.Guna2TileButton();
            this.BtnAttach = new Guna.UI2.WinForms.Guna2TileButton();
            this.BtnCopy = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator10 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton19 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator11 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.chkPrint = new System.Windows.Forms.CheckBox();
            this.BtnPrint = new Guna.UI2.WinForms.Guna2TileButton();
            this.BtnEmail = new Guna.UI2.WinForms.Guna2TileButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.guna2TileButton27 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton28 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton29 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator13 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton26 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton25 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton24 = new Guna.UI2.WinForms.Guna2TileButton();
            this.lstAccountSuggestions = new System.Windows.Forms.ListBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameslistView = new System.Windows.Forms.ListView();
            this.panel1.SuspendLayout();
            this.txtcostcenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInv)).BeginInit();
            this.panel9.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.txtAmount);
            this.panel1.Controls.Add(this.guna2Button3);
            this.panel1.Controls.Add(this.txtAmountInWord);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 605);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1045, 33);
            this.panel1.TabIndex = 2;
            // 
            // txtAmount
            // 
            this.txtAmount.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtAmount.BorderRadius = 5;
            this.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAmount.DefaultText = "";
            this.txtAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAmount.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAmount.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.ForeColor = System.Drawing.Color.Black;
            this.txtAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAmount.Location = new System.Drawing.Point(4, 4);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAmount.PlaceholderText = "Amount";
            this.txtAmount.SelectedText = "";
            this.txtAmount.Size = new System.Drawing.Size(152, 25);
            this.txtAmount.TabIndex = 27;
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
            // 
            // guna2Button3
            // 
            this.guna2Button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.guna2Button3.BorderRadius = 5;
            this.guna2Button3.BorderThickness = 3;
            this.guna2Button3.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button3.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button3.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button3.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button3.ForeColor = System.Drawing.Color.White;
            this.guna2Button3.Location = new System.Drawing.Point(782, 4);
            this.guna2Button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(103, 27);
            this.guna2Button3.TabIndex = 8;
            this.guna2Button3.Text = "Save && Close";
            this.guna2Button3.Click += new System.EventHandler(this.guna2Button3_Click);
            // 
            // txtAmountInWord
            // 
            this.txtAmountInWord.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtAmountInWord.BorderRadius = 5;
            this.txtAmountInWord.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtAmountInWord.DefaultText = "";
            this.txtAmountInWord.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtAmountInWord.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtAmountInWord.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAmountInWord.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtAmountInWord.Enabled = false;
            this.txtAmountInWord.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtAmountInWord.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAmountInWord.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmountInWord.ForeColor = System.Drawing.Color.Black;
            this.txtAmountInWord.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtAmountInWord.Location = new System.Drawing.Point(162, 4);
            this.txtAmountInWord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmountInWord.Name = "txtAmountInWord";
            this.txtAmountInWord.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAmountInWord.PlaceholderText = "Amount In Word";
            this.txtAmountInWord.SelectedText = "";
            this.txtAmountInWord.Size = new System.Drawing.Size(490, 25);
            this.txtAmountInWord.TabIndex = 26;
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
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(969, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(68, 27);
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
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(895, 4);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 27);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtcostcenter
            // 
            this.txtcostcenter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtcostcenter.BorderThickness = 0;
            this.txtcostcenter.Controls.Add(this.cmbPettyCash);
            this.txtcostcenter.Controls.Add(this.txtCode);
            this.txtcostcenter.Controls.Add(this.label3);
            this.txtcostcenter.Controls.Add(this.label5);
            this.txtcostcenter.Controls.Add(this.txtNo);
            this.txtcostcenter.Controls.Add(this.label1);
            this.txtcostcenter.Controls.Add(this.dtOpen);
            this.txtcostcenter.Controls.Add(this.label2);
            this.txtcostcenter.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.txtcostcenter.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.txtcostcenter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtcostcenter.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtcostcenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcostcenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtcostcenter.Location = new System.Drawing.Point(2, 121);
            this.txtcostcenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtcostcenter.Name = "txtcostcenter";
            this.txtcostcenter.Size = new System.Drawing.Size(1045, 90);
            this.txtcostcenter.TabIndex = 3;
            this.txtcostcenter.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // cmbPettyCash
            // 
            this.cmbPettyCash.BackColor = System.Drawing.Color.Transparent;
            this.cmbPettyCash.BorderRadius = 5;
            this.cmbPettyCash.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPettyCash.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPettyCash.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPettyCash.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPettyCash.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPettyCash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPettyCash.ItemHeight = 18;
            this.cmbPettyCash.Location = new System.Drawing.Point(92, 45);
            this.cmbPettyCash.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPettyCash.Name = "cmbPettyCash";
            this.cmbPettyCash.Size = new System.Drawing.Size(375, 24);
            this.cmbPettyCash.TabIndex = 29;
            this.cmbPettyCash.SelectedIndexChanged += new System.EventHandler(this.cmbPettyCash_SelectedIndexChanged);
            // 
            // txtCode
            // 
            this.txtCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtCode.BorderRadius = 3;
            this.txtCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCode.DefaultText = "";
            this.txtCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.ForeColor = System.Drawing.Color.Black;
            this.txtCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCode.Location = new System.Drawing.Point(92, 16);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCode.PlaceholderText = "";
            this.txtCode.SelectedText = "";
            this.txtCode.Size = new System.Drawing.Size(152, 25);
            this.txtCode.TabIndex = 28;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(1, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 24);
            this.label3.TabIndex = 26;
            this.label3.Text = "PettyCashCode";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(1, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 24);
            this.label5.TabIndex = 27;
            this.label5.Text = "PettyCash Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNo
            // 
            this.txtNo.BackColor = System.Drawing.Color.Transparent;
            this.txtNo.BorderRadius = 5;
            this.txtNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNo.DefaultText = "";
            this.txtNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtNo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtNo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtNo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNo.Location = new System.Drawing.Point(882, 45);
            this.txtNo.Name = "txtNo";
            this.txtNo.PlaceholderText = "";
            this.txtNo.SelectedText = "";
            this.txtNo.Size = new System.Drawing.Size(106, 24);
            this.txtNo.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(847, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 29);
            this.label1.TabIndex = 15;
            this.label1.Text = "V No";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtOpen
            // 
            this.dtOpen.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtOpen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtOpen.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtOpen.Location = new System.Drawing.Point(882, 16);
            this.dtOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtOpen.Name = "dtOpen";
            this.dtOpen.Size = new System.Drawing.Size(153, 22);
            this.dtOpen.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(847, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvInv
            // 
            this.dgvInv.AllowUserToDeleteRows = false;
            this.dgvInv.AllowUserToResizeColumns = false;
            this.dgvInv.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvInv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvInv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dgvInv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dgvInv.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvInv.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInv.ColumnHeadersHeight = 35;
            this.dgvInv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvInv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no,
            this.colDate,
            this.invId,
            this.Category,
            this.type,
            this.humId,
            this.CostCenter,
            this.Description,
            this.Amount,
            this.colNote});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInv.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvInv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvInv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.Location = new System.Drawing.Point(2, 211);
            this.dgvInv.MultiSelect = false;
            this.dgvInv.Name = "dgvInv";
            this.dgvInv.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvInv.RowHeadersVisible = false;
            this.dgvInv.RowHeadersWidth = 51;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvInv.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvInv.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvInv.RowTemplate.Height = 25;
            this.dgvInv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInv.Size = new System.Drawing.Size(1045, 394);
            this.dgvInv.TabIndex = 8;
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.ThemeStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvInv.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.Silver;
            this.dgvInv.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvInv.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvInv.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvInv.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvInv.ThemeStyle.HeaderStyle.Height = 35;
            this.dgvInv.ThemeStyle.ReadOnly = false;
            this.dgvInv.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvInv.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvInv.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvInv.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.ThemeStyle.RowsStyle.Height = 25;
            this.dgvInv.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellClick);
            this.dgvInv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellContentClick);
            this.dgvInv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellValueChanged);
            this.dgvInv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvInv_EditingControlShowing);
            // 
            // no
            // 
            this.no.HeaderText = "No";
            this.no.MinimumWidth = 6;
            this.no.Name = "no";
            this.no.ReadOnly = true;
            this.no.Width = 45;
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            this.colDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDate.Width = 70;
            // 
            // invId
            // 
            this.invId.HeaderText = "Ref";
            this.invId.MinimumWidth = 6;
            this.invId.Name = "invId";
            this.invId.Width = 75;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.Name = "Category";
            this.Category.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Category.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // type
            // 
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // humId
            // 
            this.humId.HeaderText = "humId";
            this.humId.Name = "humId";
            this.humId.ReadOnly = true;
            this.humId.Visible = false;
            // 
            // CostCenter
            // 
            this.CostCenter.HeaderText = "Project";
            this.CostCenter.MinimumWidth = 6;
            this.CostCenter.Name = "CostCenter";
            this.CostCenter.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CostCenter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.CostCenter.Width = 125;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
            // 
            // Amount
            // 
            this.Amount.HeaderText = "Amount";
            this.Amount.MinimumWidth = 6;
            this.Amount.Name = "Amount";
            this.Amount.Width = 125;
            // 
            // colNote
            // 
            this.colNote.HeaderText = "Note";
            this.colNote.Name = "colNote";
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
            this.headerUC1.Size = new System.Drawing.Size(1045, 37);
            this.headerUC1.TabIndex = 238;
            // 
            // guna2Button2
            // 
            this.guna2Button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button2.BorderColor = System.Drawing.Color.Transparent;
            this.guna2Button2.BorderRadius = 3;
            this.guna2Button2.BorderThickness = 1;
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Image = global::YamyProject.Properties.Resources.Subtract;
            this.guna2Button2.ImageSize = new System.Drawing.Size(15, 15);
            this.guna2Button2.Location = new System.Drawing.Point(985, 4);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(15, 15);
            this.guna2Button2.TabIndex = 21;
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
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
            this.guna2Button1.Image = global::YamyProject.Properties.Resources.Restore_Down1;
            this.guna2Button1.ImageSize = new System.Drawing.Size(15, 15);
            this.guna2Button1.Location = new System.Drawing.Point(1007, 4);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(15, 15);
            this.guna2Button1.TabIndex = 20;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.guna2HtmlLabel2.AutoSize = false;
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.ForeColor = System.Drawing.Color.White;
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(377, 1);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(314, 23);
            this.guna2HtmlLabel2.TabIndex = 19;
            this.guna2HtmlLabel2.Text = "Petty Cash";
            this.guna2HtmlLabel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1026, 4);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(15, 15);
            this.guna2ControlBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1047, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2, 640);
            this.panel2.TabIndex = 20;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(2, 638);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1045, 2);
            this.panel3.TabIndex = 21;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 640);
            this.panel4.TabIndex = 22;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.tabControl2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(2, 37);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1045, 84);
            this.panel9.TabIndex = 41;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.Padding = new System.Drawing.Point(20, 3);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1045, 84);
            this.tabControl2.TabIndex = 39;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.txtId);
            this.tabPage3.Controls.Add(this.txtPVCode);
            this.tabPage3.Controls.Add(this.BtnJournal);
            this.tabPage3.Controls.Add(this.btnNext);
            this.tabPage3.Controls.Add(this.btnPrevious);
            this.tabPage3.Controls.Add(this.guna2VSeparator12);
            this.tabPage3.Controls.Add(this.BtnClear);
            this.tabPage3.Controls.Add(this.guna2TileButton15);
            this.tabPage3.Controls.Add(this.BtnSaveNew);
            this.tabPage3.Controls.Add(this.guna2VSeparator9);
            this.tabPage3.Controls.Add(this.BtnDelete);
            this.tabPage3.Controls.Add(this.BtnAttach);
            this.tabPage3.Controls.Add(this.BtnCopy);
            this.tabPage3.Controls.Add(this.guna2VSeparator10);
            this.tabPage3.Controls.Add(this.guna2TileButton19);
            this.tabPage3.Controls.Add(this.guna2VSeparator11);
            this.tabPage3.Controls.Add(this.chkPrint);
            this.tabPage3.Controls.Add(this.BtnPrint);
            this.tabPage3.Controls.Add(this.BtnEmail);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1037, 58);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Main";
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtId.Enabled = false;
            this.txtId.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtId.Location = new System.Drawing.Point(30, 16);
            this.txtId.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtId.Multiline = true;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(69, 25);
            this.txtId.TabIndex = 33;
            this.txtId.Text = "0";
            this.txtId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtPVCode
            // 
            this.txtPVCode.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtPVCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPVCode.Enabled = false;
            this.txtPVCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPVCode.Location = new System.Drawing.Point(75, 31);
            this.txtPVCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPVCode.Multiline = true;
            this.txtPVCode.Name = "txtPVCode";
            this.txtPVCode.ReadOnly = true;
            this.txtPVCode.Size = new System.Drawing.Size(24, 10);
            this.txtPVCode.TabIndex = 16;
            this.txtPVCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPVCode.Visible = false;
            // 
            // BtnJournal
            // 
            this.BtnJournal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnJournal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.BtnJournal.BorderRadius = 5;
            this.BtnJournal.BorderThickness = 3;
            this.BtnJournal.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnJournal.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnJournal.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnJournal.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnJournal.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.BtnJournal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnJournal.ForeColor = System.Drawing.Color.White;
            this.BtnJournal.Location = new System.Drawing.Point(907, 15);
            this.BtnJournal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnJournal.Name = "BtnJournal";
            this.BtnJournal.Size = new System.Drawing.Size(124, 27);
            this.BtnJournal.TabIndex = 28;
            this.BtnJournal.Text = "Submit";
            this.BtnJournal.Click += new System.EventHandler(this.BtnJournal_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.FillColor = System.Drawing.Color.Transparent;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Image = global::YamyProject.Properties.Resources.Arrow22;
            this.btnNext.ImageSize = new System.Drawing.Size(25, 25);
            this.btnNext.Location = new System.Drawing.Point(99, 15);
            this.btnNext.Margin = new System.Windows.Forms.Padding(2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(32, 29);
            this.btnNext.TabIndex = 2;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.FillColor = System.Drawing.Color.Transparent;
            this.btnPrevious.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPrevious.ForeColor = System.Drawing.Color.White;
            this.btnPrevious.Image = global::YamyProject.Properties.Resources.Arrow_Pointing_Left;
            this.btnPrevious.Location = new System.Drawing.Point(3, 16);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(30, 28);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // guna2VSeparator12
            // 
            this.guna2VSeparator12.Location = new System.Drawing.Point(131, 6);
            this.guna2VSeparator12.Name = "guna2VSeparator12";
            this.guna2VSeparator12.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator12.TabIndex = 17;
            // 
            // BtnClear
            // 
            this.BtnClear.BackColor = System.Drawing.Color.Transparent;
            this.BtnClear.BorderColor = System.Drawing.Color.Transparent;
            this.BtnClear.BorderRadius = 10;
            this.BtnClear.BorderThickness = 2;
            this.BtnClear.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnClear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnClear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnClear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnClear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnClear.FillColor = System.Drawing.Color.Transparent;
            this.BtnClear.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnClear.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnClear.Image = global::YamyProject.Properties.Resources.add_fileN;
            this.BtnClear.Location = new System.Drawing.Point(139, 8);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(54, 47);
            this.BtnClear.TabIndex = 19;
            this.BtnClear.Text = "New";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // guna2TileButton15
            // 
            this.guna2TileButton15.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton15.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton15.BorderRadius = 10;
            this.guna2TileButton15.BorderThickness = 2;
            this.guna2TileButton15.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton15.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton15.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton15.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton15.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton15.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton15.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton15.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton15.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton15.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton15.Image = global::YamyProject.Properties.Resources.Invoice_Paid;
            this.guna2TileButton15.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton15.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton15.Location = new System.Drawing.Point(681, 29);
            this.guna2TileButton15.Name = "guna2TileButton15";
            this.guna2TileButton15.Size = new System.Drawing.Size(100, 23);
            this.guna2TileButton15.TabIndex = 32;
            this.guna2TileButton15.Text = "Add Time / Costs";
            this.guna2TileButton15.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton15.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton15.Visible = false;
            // 
            // BtnSaveNew
            // 
            this.BtnSaveNew.BackColor = System.Drawing.Color.Transparent;
            this.BtnSaveNew.BorderColor = System.Drawing.Color.Transparent;
            this.BtnSaveNew.BorderRadius = 10;
            this.BtnSaveNew.BorderThickness = 2;
            this.BtnSaveNew.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnSaveNew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnSaveNew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnSaveNew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnSaveNew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnSaveNew.FillColor = System.Drawing.Color.Transparent;
            this.BtnSaveNew.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSaveNew.ForeColor = System.Drawing.Color.Black;
            this.BtnSaveNew.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnSaveNew.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnSaveNew.Image = global::YamyProject.Properties.Resources.Save;
            this.BtnSaveNew.Location = new System.Drawing.Point(191, 8);
            this.BtnSaveNew.Name = "BtnSaveNew";
            this.BtnSaveNew.Size = new System.Drawing.Size(54, 47);
            this.BtnSaveNew.TabIndex = 20;
            this.BtnSaveNew.Text = "Save";
            this.BtnSaveNew.Click += new System.EventHandler(this.BtnSaveNew_Click);
            // 
            // guna2VSeparator9
            // 
            this.guna2VSeparator9.Location = new System.Drawing.Point(667, 6);
            this.guna2VSeparator9.Name = "guna2VSeparator9";
            this.guna2VSeparator9.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator9.TabIndex = 31;
            // 
            // BtnDelete
            // 
            this.BtnDelete.BackColor = System.Drawing.Color.Transparent;
            this.BtnDelete.BorderColor = System.Drawing.Color.Transparent;
            this.BtnDelete.BorderRadius = 10;
            this.BtnDelete.BorderThickness = 2;
            this.BtnDelete.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnDelete.FillColor = System.Drawing.Color.Transparent;
            this.BtnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDelete.ForeColor = System.Drawing.Color.Black;
            this.BtnDelete.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnDelete.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnDelete.Image = global::YamyProject.Properties.Resources.deleteN;
            this.BtnDelete.Location = new System.Drawing.Point(247, 8);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(54, 47);
            this.BtnDelete.TabIndex = 21;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnAttach
            // 
            this.BtnAttach.BackColor = System.Drawing.Color.Transparent;
            this.BtnAttach.BorderColor = System.Drawing.Color.Transparent;
            this.BtnAttach.BorderRadius = 10;
            this.BtnAttach.BorderThickness = 2;
            this.BtnAttach.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnAttach.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnAttach.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnAttach.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnAttach.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnAttach.FillColor = System.Drawing.Color.Transparent;
            this.BtnAttach.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAttach.ForeColor = System.Drawing.Color.Black;
            this.BtnAttach.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnAttach.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnAttach.Image = global::YamyProject.Properties.Resources.excel;
            this.BtnAttach.ImageSize = new System.Drawing.Size(25, 25);
            this.BtnAttach.Location = new System.Drawing.Point(613, 7);
            this.BtnAttach.Name = "BtnAttach";
            this.BtnAttach.Size = new System.Drawing.Size(54, 47);
            this.BtnAttach.TabIndex = 30;
            this.BtnAttach.Text = "Import File";
            this.BtnAttach.Click += new System.EventHandler(this.BtnAttach_Click);
            // 
            // BtnCopy
            // 
            this.BtnCopy.BackColor = System.Drawing.Color.Transparent;
            this.BtnCopy.BorderColor = System.Drawing.Color.Transparent;
            this.BtnCopy.BorderRadius = 10;
            this.BtnCopy.BorderThickness = 2;
            this.BtnCopy.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnCopy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnCopy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnCopy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnCopy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnCopy.FillColor = System.Drawing.Color.Transparent;
            this.BtnCopy.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCopy.ForeColor = System.Drawing.Color.Black;
            this.BtnCopy.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnCopy.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnCopy.Image = global::YamyProject.Properties.Resources.copyN;
            this.BtnCopy.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.BtnCopy.ImageOffset = new System.Drawing.Point(-10, 10);
            this.BtnCopy.Location = new System.Drawing.Point(301, 8);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(100, 23);
            this.BtnCopy.TabIndex = 22;
            this.BtnCopy.Text = "Create a Copy";
            this.BtnCopy.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.BtnCopy.TextOffset = new System.Drawing.Point(10, -12);
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // guna2VSeparator10
            // 
            this.guna2VSeparator10.Location = new System.Drawing.Point(605, 6);
            this.guna2VSeparator10.Name = "guna2VSeparator10";
            this.guna2VSeparator10.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator10.TabIndex = 29;
            // 
            // guna2TileButton19
            // 
            this.guna2TileButton19.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton19.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton19.BorderRadius = 10;
            this.guna2TileButton19.BorderThickness = 2;
            this.guna2TileButton19.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton19.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton19.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton19.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton19.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton19.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton19.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton19.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton19.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton19.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton19.Image = global::YamyProject.Properties.Resources.Cloud;
            this.guna2TileButton19.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton19.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton19.Location = new System.Drawing.Point(309, 29);
            this.guna2TileButton19.Name = "guna2TileButton19";
            this.guna2TileButton19.Size = new System.Drawing.Size(100, 23);
            this.guna2TileButton19.TabIndex = 23;
            this.guna2TileButton19.Text = "Memorize";
            this.guna2TileButton19.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton19.TextOffset = new System.Drawing.Point(10, -12);
            // 
            // guna2VSeparator11
            // 
            this.guna2VSeparator11.Location = new System.Drawing.Point(401, 6);
            this.guna2VSeparator11.Name = "guna2VSeparator11";
            this.guna2VSeparator11.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator11.TabIndex = 24;
            // 
            // chkPrint
            // 
            this.chkPrint.AutoSize = true;
            this.chkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrint.Location = new System.Drawing.Point(517, 34);
            this.chkPrint.Margin = new System.Windows.Forms.Padding(2);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Size = new System.Drawing.Size(88, 16);
            this.chkPrint.TabIndex = 27;
            this.chkPrint.Text = "Print  After Save";
            this.chkPrint.UseVisualStyleBackColor = true;
            // 
            // BtnPrint
            // 
            this.BtnPrint.BackColor = System.Drawing.Color.Transparent;
            this.BtnPrint.BorderColor = System.Drawing.Color.Transparent;
            this.BtnPrint.BorderRadius = 10;
            this.BtnPrint.BorderThickness = 2;
            this.BtnPrint.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnPrint.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnPrint.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnPrint.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnPrint.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnPrint.FillColor = System.Drawing.Color.Transparent;
            this.BtnPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrint.ForeColor = System.Drawing.Color.Black;
            this.BtnPrint.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnPrint.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnPrint.Image = global::YamyProject.Properties.Resources.printN;
            this.BtnPrint.ImageSize = new System.Drawing.Size(25, 25);
            this.BtnPrint.Location = new System.Drawing.Point(409, 8);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(54, 47);
            this.BtnPrint.TabIndex = 25;
            this.BtnPrint.Text = "Print";
            this.BtnPrint.Click += new System.EventHandler(this.guna2TileButton18_Click);
            // 
            // BtnEmail
            // 
            this.BtnEmail.BackColor = System.Drawing.Color.Transparent;
            this.BtnEmail.BorderColor = System.Drawing.Color.Transparent;
            this.BtnEmail.BorderRadius = 10;
            this.BtnEmail.BorderThickness = 2;
            this.BtnEmail.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.BtnEmail.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.BtnEmail.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.BtnEmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.BtnEmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.BtnEmail.FillColor = System.Drawing.Color.Transparent;
            this.BtnEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnEmail.ForeColor = System.Drawing.Color.Black;
            this.BtnEmail.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.BtnEmail.HoverState.FillColor = System.Drawing.Color.Silver;
            this.BtnEmail.Image = global::YamyProject.Properties.Resources.emailN;
            this.BtnEmail.ImageSize = new System.Drawing.Size(25, 25);
            this.BtnEmail.Location = new System.Drawing.Point(463, 8);
            this.BtnEmail.Name = "BtnEmail";
            this.BtnEmail.Size = new System.Drawing.Size(54, 47);
            this.BtnEmail.TabIndex = 26;
            this.BtnEmail.Text = "Email";
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage4.Controls.Add(this.guna2TileButton27);
            this.tabPage4.Controls.Add(this.guna2TileButton28);
            this.tabPage4.Controls.Add(this.guna2TileButton29);
            this.tabPage4.Controls.Add(this.guna2VSeparator13);
            this.tabPage4.Controls.Add(this.guna2TileButton26);
            this.tabPage4.Controls.Add(this.guna2TileButton25);
            this.tabPage4.Controls.Add(this.guna2TileButton24);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1037, 58);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Reports";
            // 
            // guna2TileButton27
            // 
            this.guna2TileButton27.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton27.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton27.BorderRadius = 10;
            this.guna2TileButton27.BorderThickness = 2;
            this.guna2TileButton27.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton27.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton27.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton27.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton27.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton27.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton27.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton27.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton27.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton27.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton27.Image = global::YamyProject.Properties.Resources._6;
            this.guna2TileButton27.Location = new System.Drawing.Point(385, 7);
            this.guna2TileButton27.Name = "guna2TileButton27";
            this.guna2TileButton27.Size = new System.Drawing.Size(94, 47);
            this.guna2TileButton27.TabIndex = 26;
            this.guna2TileButton27.Text = "Average Days To Pay Summary";
            this.guna2TileButton27.Visible = false;
            // 
            // guna2TileButton28
            // 
            this.guna2TileButton28.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton28.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton28.BorderRadius = 10;
            this.guna2TileButton28.BorderThickness = 2;
            this.guna2TileButton28.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton28.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton28.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton28.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton28.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton28.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton28.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton28.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton28.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton28.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton28.Image = global::YamyProject.Properties.Resources._5;
            this.guna2TileButton28.Location = new System.Drawing.Point(292, 7);
            this.guna2TileButton28.Name = "guna2TileButton28";
            this.guna2TileButton28.Size = new System.Drawing.Size(93, 47);
            this.guna2TileButton28.TabIndex = 25;
            this.guna2TileButton28.Text = "Sales By Customer Details";
            this.guna2TileButton28.Visible = false;
            // 
            // guna2TileButton29
            // 
            this.guna2TileButton29.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton29.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton29.BorderRadius = 10;
            this.guna2TileButton29.BorderThickness = 2;
            this.guna2TileButton29.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton29.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton29.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton29.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton29.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton29.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton29.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton29.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton29.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton29.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton29.Image = global::YamyProject.Properties.Resources._4;
            this.guna2TileButton29.Location = new System.Drawing.Point(222, 7);
            this.guna2TileButton29.Name = "guna2TileButton29";
            this.guna2TileButton29.Size = new System.Drawing.Size(70, 47);
            this.guna2TileButton29.TabIndex = 24;
            this.guna2TileButton29.Text = "View Open Invoice";
            this.guna2TileButton29.Visible = false;
            // 
            // guna2VSeparator13
            // 
            this.guna2VSeparator13.Location = new System.Drawing.Point(214, 7);
            this.guna2VSeparator13.Name = "guna2VSeparator13";
            this.guna2VSeparator13.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator13.TabIndex = 23;
            // 
            // guna2TileButton26
            // 
            this.guna2TileButton26.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton26.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton26.BorderRadius = 10;
            this.guna2TileButton26.BorderThickness = 2;
            this.guna2TileButton26.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton26.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton26.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton26.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton26.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton26.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton26.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton26.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton26.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton26.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton26.Image = global::YamyProject.Properties.Resources._3;
            this.guna2TileButton26.Location = new System.Drawing.Point(135, 7);
            this.guna2TileButton26.Name = "guna2TileButton26";
            this.guna2TileButton26.Size = new System.Drawing.Size(79, 47);
            this.guna2TileButton26.TabIndex = 22;
            this.guna2TileButton26.Text = "Transaction Journal";
            this.guna2TileButton26.Click += new System.EventHandler(this.guna2TileButton26_Click);
            // 
            // guna2TileButton25
            // 
            this.guna2TileButton25.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton25.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton25.BorderRadius = 10;
            this.guna2TileButton25.BorderThickness = 2;
            this.guna2TileButton25.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton25.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton25.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton25.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton25.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton25.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton25.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton25.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton25.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton25.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton25.Image = global::YamyProject.Properties.Resources._2;
            this.guna2TileButton25.Location = new System.Drawing.Point(63, 7);
            this.guna2TileButton25.Name = "guna2TileButton25";
            this.guna2TileButton25.Size = new System.Drawing.Size(72, 47);
            this.guna2TileButton25.TabIndex = 21;
            this.guna2TileButton25.Text = "Transaction History";
            this.guna2TileButton25.Click += new System.EventHandler(this.guna2TileButton25_Click);
            // 
            // guna2TileButton24
            // 
            this.guna2TileButton24.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton24.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton24.BorderRadius = 10;
            this.guna2TileButton24.BorderThickness = 2;
            this.guna2TileButton24.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton24.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton24.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton24.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton24.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton24.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton24.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton24.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton24.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton24.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton24.Image = global::YamyProject.Properties.Resources._1;
            this.guna2TileButton24.Location = new System.Drawing.Point(3, 3);
            this.guna2TileButton24.Name = "guna2TileButton24";
            this.guna2TileButton24.Size = new System.Drawing.Size(60, 51);
            this.guna2TileButton24.TabIndex = 20;
            this.guna2TileButton24.Text = "Quick Report";
            // 
            // lstAccountSuggestions
            // 
            this.lstAccountSuggestions.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAccountSuggestions.FormattingEnabled = true;
            this.lstAccountSuggestions.Location = new System.Drawing.Point(295, -7);
            this.lstAccountSuggestions.Name = "lstAccountSuggestions";
            this.lstAccountSuggestions.Size = new System.Drawing.Size(308, 30);
            this.lstAccountSuggestions.TabIndex = 12;
            this.lstAccountSuggestions.TabStop = false;
            this.lstAccountSuggestions.Visible = false;
            this.lstAccountSuggestions.Click += new System.EventHandler(this.lstAccountSuggestions_Click);
            this.lstAccountSuggestions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstAccountSuggestions_MouseDown);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "no";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "humId";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            this.dataGridViewTextBoxColumn2.Width = 125;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "invId";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Visible = false;
            this.dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "invDate";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "InvNo";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Total";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Pay";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Description";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 125;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Width = 125;
            // 
            // nameslistView
            // 
            this.nameslistView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nameslistView.BackColor = System.Drawing.Color.Gainsboro;
            this.nameslistView.HideSelection = false;
            this.nameslistView.Location = new System.Drawing.Point(1170, 73);
            this.nameslistView.Margin = new System.Windows.Forms.Padding(4);
            this.nameslistView.Name = "nameslistView";
            this.nameslistView.Size = new System.Drawing.Size(38, 22);
            this.nameslistView.TabIndex = 9;
            this.nameslistView.UseCompatibleStateImageBehavior = false;
            this.nameslistView.Visible = false;
            this.nameslistView.Click += new System.EventHandler(this.nameslistView_Click);
            this.nameslistView.Leave += new System.EventHandler(this.nameslistView_Leave);
            // 
            // frmViewPettyCashVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1049, 640);
            this.Controls.Add(this.nameslistView);
            this.Controls.Add(this.dgvInv);
            this.Controls.Add(this.txtcostcenter);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lstAccountSuggestions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmViewPettyCashVoucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Payment Voucher";
            this.Load += new System.EventHandler(this.frmViewPettyCashVoucher_Load);
            this.panel1.ResumeLayout(false);
            this.txtcostcenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInv)).EndInit();
            this.panel9.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2GroupBox txtcostcenter;
        private System.Windows.Forms.DateTimePicker dtOpen;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtAmount;
        private Guna.UI2.WinForms.Guna2TextBox txtAmountInWord;
        private Guna.UI2.WinForms.Guna2DataGridView dgvInv;
        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtPVCode;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2Button btnPrevious;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator12;
        private Guna.UI2.WinForms.Guna2TileButton BtnClear;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton15;
        private Guna.UI2.WinForms.Guna2TileButton BtnSaveNew;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator9;
        private Guna.UI2.WinForms.Guna2TileButton BtnDelete;
        private Guna.UI2.WinForms.Guna2TileButton BtnAttach;
        private Guna.UI2.WinForms.Guna2TileButton BtnCopy;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator10;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton19;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator11;
        private System.Windows.Forms.CheckBox chkPrint;
        private Guna.UI2.WinForms.Guna2TileButton BtnPrint;
        private Guna.UI2.WinForms.Guna2TileButton BtnEmail;
        private System.Windows.Forms.TabPage tabPage4;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton27;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton28;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton29;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator13;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton26;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton25;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton24;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private System.Windows.Forms.ListBox lstAccountSuggestions;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DAL.CalendarColumn invDate;
        private Guna.UI2.WinForms.Guna2TextBox txtNo;
        private Label label1;
        private Guna.UI2.WinForms.Guna2TextBox txtCode;
        private Label label3;
        private Label label5;
        public Guna.UI2.WinForms.Guna2ComboBox cmbPettyCash;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button BtnJournal;
        private System.Windows.Forms.ListView nameslistView;
        private DataGridViewTextBoxColumn no;
        private DataGridViewTextBoxColumn colDate;
        private DataGridViewTextBoxColumn invId;
        private DataGridViewComboBoxColumn Category;
        private DataGridViewTextBoxColumn type;
        private DataGridViewTextBoxColumn humId;
        private DataGridViewComboBoxColumn CostCenter;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn Amount;
        private DataGridViewTextBoxColumn colNote;
        private TextBox txtId;
    }
}