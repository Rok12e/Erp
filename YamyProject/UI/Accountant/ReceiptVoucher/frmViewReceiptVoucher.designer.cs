using System.ComponentModel;
using System.Windows.Forms;

namespace YamyProject
{
    partial class frmViewReceiptVoucher
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.cmbMethod = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.dtOpen = new System.Windows.Forms.DateTimePicker();
            this.txtAmountInWord = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPaymentType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblBalance = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2GroupBox2 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCreditAccountName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCreditAccountCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbCreditCostCenter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblCostcenter = new System.Windows.Forms.Label();
            this.pnlCustomer = new System.Windows.Forms.Panel();
            this.txtCustomerCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCustomer = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblCode = new System.Windows.Forms.Label();
            this.lstAccountSuggestions = new System.Windows.Forms.ListBox();
            this.lblTransDate = new System.Windows.Forms.Label();
            this.dtpTransDate = new System.Windows.Forms.DateTimePicker();
            this.txtTransName = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTransRef = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtBankCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.guna2GroupBox3 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.cmbDebitAccountName = new System.Windows.Forms.ComboBox();
            this.txtDebitAccountCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbDebitCostCenter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlBank = new System.Windows.Forms.Panel();
            this.dt_check_date = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCheckNo = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbBankName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtCheckName = new Guna.UI2.WinForms.Guna2TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lblBankName = new System.Windows.Forms.Label();
            this.pnlTrans = new System.Windows.Forms.Panel();
            this.lblTransName = new System.Windows.Forms.Label();
            this.lblTransRef = new System.Windows.Forms.Label();
            this.costCenter = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgvInv = new Guna.UI2.WinForms.Guna2DataGridView();
            this.no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.humId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkPay = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Pay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtPVCode = new System.Windows.Forms.TextBox();
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.btnPrevious = new Guna.UI2.WinForms.Guna2Button();
            this.guna2VSeparator12 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.btnNew = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton15 = new Guna.UI2.WinForms.Guna2TileButton();
            this.btnSaveNew = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator9 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.btnDelete = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton16 = new Guna.UI2.WinForms.Guna2TileButton();
            this.btnCopy = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator10 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton19 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator11 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.chkPrint = new System.Windows.Forms.CheckBox();
            this.btnPrint = new Guna.UI2.WinForms.Guna2TileButton();
            this.btnEmail = new Guna.UI2.WinForms.Guna2TileButton();
            this.txtId = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.guna2TileButton27 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton28 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton29 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator13 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton26 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton25 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton24 = new Guna.UI2.WinForms.Guna2TileButton();
            this.panel1.SuspendLayout();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2ShadowPanel1.SuspendLayout();
            this.guna2GroupBox2.SuspendLayout();
            this.guna2ShadowPanel2.SuspendLayout();
            this.pnlCustomer.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.pnlBank.SuspendLayout();
            this.pnlTrans.SuspendLayout();
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
            this.panel1.Controls.Add(this.guna2Button3);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 605);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 33);
            this.panel1.TabIndex = 2;
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
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.guna2Button3.ForeColor = System.Drawing.Color.White;
            this.guna2Button3.Location = new System.Drawing.Point(671, 3);
            this.guna2Button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(114, 27);
            this.guna2Button3.TabIndex = 8;
            this.guna2Button3.Text = "Save && Close";
            this.guna2Button3.Click += new System.EventHandler(this.guna2Button3_Click);
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
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(871, 3);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 27);
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
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(791, 3);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 27);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BorderThickness = 0;
            this.guna2GroupBox1.Controls.Add(this.guna2ShadowPanel1);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.White;
            this.guna2GroupBox1.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(2, 121);
            this.guna2GroupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(954, 125);
            this.guna2GroupBox1.TabIndex = 3;
            this.guna2GroupBox1.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.cmbMethod);
            this.guna2ShadowPanel1.Controls.Add(this.txtAmount);
            this.guna2ShadowPanel1.Controls.Add(this.dtOpen);
            this.guna2ShadowPanel1.Controls.Add(this.txtAmountInWord);
            this.guna2ShadowPanel1.Controls.Add(this.label2);
            this.guna2ShadowPanel1.Controls.Add(this.cmbPaymentType);
            this.guna2ShadowPanel1.Controls.Add(this.lblBalance);
            this.guna2ShadowPanel1.Controls.Add(this.label1);
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(3, 2);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.Radius = 5;
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(950, 118);
            this.guna2ShadowPanel1.TabIndex = 28;
            // 
            // cmbMethod
            // 
            this.cmbMethod.BackColor = System.Drawing.Color.Transparent;
            this.cmbMethod.BorderColor = System.Drawing.Color.DarkGray;
            this.cmbMethod.BorderRadius = 3;
            this.cmbMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbMethod.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbMethod.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbMethod.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cmbMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbMethod.ItemHeight = 18;
            this.cmbMethod.Items.AddRange(new object[] {
            "Cash",
            "Cheque",
            "Transfer"});
            this.cmbMethod.Location = new System.Drawing.Point(552, 79);
            this.cmbMethod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(205, 24);
            this.cmbMethod.TabIndex = 9;
            this.cmbMethod.SelectedIndexChanged += new System.EventHandler(this.cmbMethod_SelectedIndexChanged);
            // 
            // txtAmount
            // 
            this.txtAmount.BackColor = System.Drawing.Color.Transparent;
            this.txtAmount.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtAmount.BorderRadius = 3;
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
            this.txtAmount.Location = new System.Drawing.Point(12, 80);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAmount.PlaceholderText = "Amount";
            this.txtAmount.SelectedText = "";
            this.txtAmount.Size = new System.Drawing.Size(187, 25);
            this.txtAmount.TabIndex = 27;
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
            // 
            // dtOpen
            // 
            this.dtOpen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtOpen.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtOpen.Location = new System.Drawing.Point(11, 38);
            this.dtOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtOpen.Name = "dtOpen";
            this.dtOpen.Size = new System.Drawing.Size(189, 22);
            this.dtOpen.TabIndex = 22;
            // 
            // txtAmountInWord
            // 
            this.txtAmountInWord.BackColor = System.Drawing.Color.Transparent;
            this.txtAmountInWord.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtAmountInWord.BorderRadius = 3;
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
            this.txtAmountInWord.Location = new System.Drawing.Point(205, 79);
            this.txtAmountInWord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmountInWord.Name = "txtAmountInWord";
            this.txtAmountInWord.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAmountInWord.PlaceholderText = "Amount In Word";
            this.txtAmountInWord.SelectedText = "";
            this.txtAmountInWord.Size = new System.Drawing.Size(340, 26);
            this.txtAmountInWord.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(10, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPaymentType
            // 
            this.cmbPaymentType.BackColor = System.Drawing.Color.Transparent;
            this.cmbPaymentType.BorderColor = System.Drawing.Color.DarkGray;
            this.cmbPaymentType.BorderRadius = 3;
            this.cmbPaymentType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentType.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbPaymentType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentType.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPaymentType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPaymentType.ItemHeight = 18;
            this.cmbPaymentType.Items.AddRange(new object[] {
            "Customer",
            "General"});
            this.cmbPaymentType.Location = new System.Drawing.Point(552, 38);
            this.cmbPaymentType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPaymentType.Name = "cmbPaymentType";
            this.cmbPaymentType.Size = new System.Drawing.Size(205, 24);
            this.cmbPaymentType.TabIndex = 11;
            this.cmbPaymentType.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentType_SelectedIndexChanged);
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.Transparent;
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblBalance.Location = new System.Drawing.Point(549, 56);
            this.lblBalance.Name = "lblBalance";
            this.lblBalance.Size = new System.Drawing.Size(97, 29);
            this.lblBalance.TabIndex = 10;
            this.lblBalance.Text = "Payment Method";
            this.lblBalance.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(549, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Payment Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guna2GroupBox2
            // 
            this.guna2GroupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox2.BorderThickness = 0;
            this.guna2GroupBox2.Controls.Add(this.guna2ShadowPanel2);
            this.guna2GroupBox2.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox2.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox2.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox2.Location = new System.Drawing.Point(2, 246);
            this.guna2GroupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox2.Name = "guna2GroupBox2";
            this.guna2GroupBox2.Size = new System.Drawing.Size(954, 174);
            this.guna2GroupBox2.TabIndex = 4;
            this.guna2GroupBox2.Text = "Received From - Credit";
            this.guna2GroupBox2.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // guna2ShadowPanel2
            // 
            this.guna2ShadowPanel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.Controls.Add(this.txtDescription);
            this.guna2ShadowPanel2.Controls.Add(this.guna2TextBox1);
            this.guna2ShadowPanel2.Controls.Add(this.cmbCreditAccountName);
            this.guna2ShadowPanel2.Controls.Add(this.label4);
            this.guna2ShadowPanel2.Controls.Add(this.txtCreditAccountCode);
            this.guna2ShadowPanel2.Controls.Add(this.label9);
            this.guna2ShadowPanel2.Controls.Add(this.label3);
            this.guna2ShadowPanel2.Controls.Add(this.cmbCreditCostCenter);
            this.guna2ShadowPanel2.Controls.Add(this.lblCostcenter);
            this.guna2ShadowPanel2.Controls.Add(this.pnlCustomer);
            this.guna2ShadowPanel2.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2ShadowPanel2.Location = new System.Drawing.Point(7, 28);
            this.guna2ShadowPanel2.Name = "guna2ShadowPanel2";
            this.guna2ShadowPanel2.Radius = 5;
            this.guna2ShadowPanel2.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel2.Size = new System.Drawing.Size(942, 140);
            this.guna2ShadowPanel2.TabIndex = 34;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(13, 82);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(524, 36);
            this.txtDescription.TabIndex = 33;
            this.txtDescription.Text = "";
            // 
            // guna2TextBox1
            // 
            this.guna2TextBox1.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.guna2TextBox1.BorderRadius = 3;
            this.guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox1.DefaultText = "";
            this.guna2TextBox1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TextBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Location = new System.Drawing.Point(10, 77);
            this.guna2TextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.guna2TextBox1.PlaceholderText = "";
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(531, 47);
            this.guna2TextBox1.TabIndex = 34;
            // 
            // cmbCreditAccountName
            // 
            this.cmbCreditAccountName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCreditAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCreditAccountName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCreditAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCreditAccountName.ItemHeight = 13;
            this.cmbCreditAccountName.Location = new System.Drawing.Point(134, 31);
            this.cmbCreditAccountName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCreditAccountName.Name = "cmbCreditAccountName";
            this.cmbCreditAccountName.Size = new System.Drawing.Size(244, 21);
            this.cmbCreditAccountName.TabIndex = 31;
            this.cmbCreditAccountName.SelectedIndexChanged += new System.EventHandler(this.cmbCreditAccountName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label4.Location = new System.Drawing.Point(134, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 24);
            this.label4.TabIndex = 23;
            this.label4.Text = "Account Name";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCreditAccountCode
            // 
            this.txtCreditAccountCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtCreditAccountCode.BorderRadius = 5;
            this.txtCreditAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCreditAccountCode.DefaultText = "";
            this.txtCreditAccountCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCreditAccountCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCreditAccountCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCreditAccountCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCreditAccountCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCreditAccountCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCreditAccountCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreditAccountCode.ForeColor = System.Drawing.Color.Black;
            this.txtCreditAccountCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCreditAccountCode.Location = new System.Drawing.Point(10, 32);
            this.txtCreditAccountCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCreditAccountCode.Name = "txtCreditAccountCode";
            this.txtCreditAccountCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCreditAccountCode.PlaceholderText = "";
            this.txtCreditAccountCode.SelectedText = "";
            this.txtCreditAccountCode.Size = new System.Drawing.Size(118, 24);
            this.txtCreditAccountCode.TabIndex = 27;
            this.txtCreditAccountCode.TextChanged += new System.EventHandler(this.txtCreditAccountCode_TextChanged);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label9.Location = new System.Drawing.Point(7, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 24);
            this.label9.TabIndex = 23;
            this.label9.Text = "Account Code";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(10, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 28;
            this.label3.Text = "Note";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCreditCostCenter
            // 
            this.cmbCreditCostCenter.BackColor = System.Drawing.Color.Transparent;
            this.cmbCreditCostCenter.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbCreditCostCenter.BorderRadius = 5;
            this.cmbCreditCostCenter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCreditCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCreditCostCenter.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbCreditCostCenter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCreditCostCenter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCreditCostCenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCreditCostCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCreditCostCenter.ItemHeight = 16;
            this.cmbCreditCostCenter.Location = new System.Drawing.Point(384, 30);
            this.cmbCreditCostCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCreditCostCenter.Name = "cmbCreditCostCenter";
            this.cmbCreditCostCenter.Size = new System.Drawing.Size(157, 22);
            this.cmbCreditCostCenter.TabIndex = 22;
            this.cmbCreditCostCenter.SelectedIndexChanged += new System.EventHandler(this.cmbCreditCostCenter_SelectedIndexChanged);
            // 
            // lblCostcenter
            // 
            this.lblCostcenter.BackColor = System.Drawing.Color.Transparent;
            this.lblCostcenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostcenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblCostcenter.Location = new System.Drawing.Point(384, 5);
            this.lblCostcenter.Name = "lblCostcenter";
            this.lblCostcenter.Size = new System.Drawing.Size(71, 28);
            this.lblCostcenter.TabIndex = 23;
            this.lblCostcenter.Text = "Cost Center";
            this.lblCostcenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlCustomer
            // 
            this.pnlCustomer.BackColor = System.Drawing.Color.Transparent;
            this.pnlCustomer.Controls.Add(this.txtCustomerCode);
            this.pnlCustomer.Controls.Add(this.cmbCustomer);
            this.pnlCustomer.Controls.Add(this.lblCustomer);
            this.pnlCustomer.Controls.Add(this.lblCode);
            this.pnlCustomer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlCustomer.Location = new System.Drawing.Point(543, 10);
            this.pnlCustomer.Name = "pnlCustomer";
            this.pnlCustomer.Size = new System.Drawing.Size(380, 114);
            this.pnlCustomer.TabIndex = 32;
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtCustomerCode.BorderRadius = 3;
            this.txtCustomerCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerCode.DefaultText = "";
            this.txtCustomerCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCustomerCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCustomerCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCustomerCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCustomerCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCustomerCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCustomerCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtCustomerCode.ForeColor = System.Drawing.Color.Black;
            this.txtCustomerCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCustomerCode.Location = new System.Drawing.Point(3, 21);
            this.txtCustomerCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCustomerCode.PlaceholderText = "Customer Code";
            this.txtCustomerCode.SelectedText = "";
            this.txtCustomerCode.Size = new System.Drawing.Size(118, 27);
            this.txtCustomerCode.TabIndex = 27;
            this.txtCustomerCode.TextChanged += new System.EventHandler(this.txtCustomerCode_TextChanged);
            this.txtCustomerCode.Leave += new System.EventHandler(this.txtCustomerCode_Leave);
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.BackColor = System.Drawing.Color.Transparent;
            this.cmbCustomer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomer.FocusedColor = System.Drawing.Color.Empty;
            this.cmbCustomer.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCustomer.ItemHeight = 18;
            this.cmbCustomer.Location = new System.Drawing.Point(3, 67);
            this.cmbCustomer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(354, 24);
            this.cmbCustomer.TabIndex = 22;
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblCustomer.Location = new System.Drawing.Point(3, 47);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(101, 24);
            this.lblCustomer.TabIndex = 26;
            this.lblCustomer.Text = "Customer Name";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.Transparent;
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblCode.Location = new System.Drawing.Point(3, 2);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(101, 19);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "Customer Code";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lstAccountSuggestions
            // 
            this.lstAccountSuggestions.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAccountSuggestions.FormattingEnabled = true;
            this.lstAccountSuggestions.Location = new System.Drawing.Point(295, -7);
            this.lstAccountSuggestions.Name = "lstAccountSuggestions";
            this.lstAccountSuggestions.Size = new System.Drawing.Size(308, 43);
            this.lstAccountSuggestions.TabIndex = 41;
            this.lstAccountSuggestions.TabStop = false;
            this.lstAccountSuggestions.Visible = false;
            this.lstAccountSuggestions.Click += new System.EventHandler(this.lstAccountSuggestions_Click);
            // 
            // lblTransDate
            // 
            this.lblTransDate.BackColor = System.Drawing.Color.Transparent;
            this.lblTransDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblTransDate.Location = new System.Drawing.Point(9, 22);
            this.lblTransDate.Name = "lblTransDate";
            this.lblTransDate.Size = new System.Drawing.Size(95, 13);
            this.lblTransDate.TabIndex = 25;
            this.lblTransDate.Text = "Transfer Date";
            this.lblTransDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTransDate
            // 
            this.dtpTransDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTransDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpTransDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTransDate.Location = new System.Drawing.Point(9, 35);
            this.dtpTransDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpTransDate.Name = "dtpTransDate";
            this.dtpTransDate.Size = new System.Drawing.Size(166, 22);
            this.dtpTransDate.TabIndex = 17;
            // 
            // txtTransName
            // 
            this.txtTransName.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtTransName.BorderRadius = 3;
            this.txtTransName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTransName.DefaultText = "";
            this.txtTransName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTransName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTransName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTransName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTransName.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtTransName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTransName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransName.ForeColor = System.Drawing.Color.Black;
            this.txtTransName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTransName.Location = new System.Drawing.Point(9, 74);
            this.txtTransName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTransName.Name = "txtTransName";
            this.txtTransName.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTransName.PlaceholderText = "TRNS / Name";
            this.txtTransName.SelectedText = "";
            this.txtTransName.Size = new System.Drawing.Size(141, 25);
            this.txtTransName.TabIndex = 22;
            // 
            // txtTransRef
            // 
            this.txtTransRef.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtTransRef.BorderRadius = 3;
            this.txtTransRef.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTransRef.DefaultText = "";
            this.txtTransRef.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTransRef.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTransRef.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTransRef.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTransRef.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtTransRef.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTransRef.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransRef.ForeColor = System.Drawing.Color.Black;
            this.txtTransRef.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTransRef.Location = new System.Drawing.Point(9, 112);
            this.txtTransRef.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTransRef.Name = "txtTransRef";
            this.txtTransRef.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTransRef.PlaceholderText = "TRNS / REF";
            this.txtTransRef.SelectedText = "";
            this.txtTransRef.Size = new System.Drawing.Size(141, 25);
            this.txtTransRef.TabIndex = 23;
            // 
            // txtBankCode
            // 
            this.txtBankCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtBankCode.BorderRadius = 3;
            this.txtBankCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBankCode.DefaultText = "";
            this.txtBankCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBankCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBankCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBankCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBankCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtBankCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBankCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBankCode.ForeColor = System.Drawing.Color.Black;
            this.txtBankCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBankCode.Location = new System.Drawing.Point(262, 76);
            this.txtBankCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtBankCode.PlaceholderText = "Bank Code";
            this.txtBankCode.SelectedText = "";
            this.txtBankCode.Size = new System.Drawing.Size(119, 25);
            this.txtBankCode.TabIndex = 24;
            this.txtBankCode.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label6.Location = new System.Drawing.Point(138, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 28);
            this.label6.TabIndex = 14;
            this.label6.Text = "Account Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guna2GroupBox3
            // 
            this.guna2GroupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox3.BorderThickness = 0;
            this.guna2GroupBox3.Controls.Add(this.cmbDebitAccountName);
            this.guna2GroupBox3.Controls.Add(this.txtDebitAccountCode);
            this.guna2GroupBox3.Controls.Add(this.cmbDebitCostCenter);
            this.guna2GroupBox3.Controls.Add(this.label10);
            this.guna2GroupBox3.Controls.Add(this.label6);
            this.guna2GroupBox3.Controls.Add(this.label7);
            this.guna2GroupBox3.Controls.Add(this.pnlBank);
            this.guna2GroupBox3.Controls.Add(this.pnlTrans);
            this.guna2GroupBox3.CustomBorderColor = System.Drawing.Color.White;
            this.guna2GroupBox3.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox3.Location = new System.Drawing.Point(2, 420);
            this.guna2GroupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Size = new System.Drawing.Size(954, 149);
            this.guna2GroupBox3.TabIndex = 5;
            this.guna2GroupBox3.Text = "Received To - Debit";
            this.guna2GroupBox3.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // cmbDebitAccountName
            // 
            this.cmbDebitAccountName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDebitAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDebitAccountName.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cmbDebitAccountName.ItemHeight = 13;
            this.cmbDebitAccountName.Location = new System.Drawing.Point(141, 38);
            this.cmbDebitAccountName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbDebitAccountName.Name = "cmbDebitAccountName";
            this.cmbDebitAccountName.Size = new System.Drawing.Size(244, 21);
            this.cmbDebitAccountName.TabIndex = 31;
            this.cmbDebitAccountName.SelectedIndexChanged += new System.EventHandler(this.cmbDebitAccountName_SelectedIndexChanged);
            // 
            // txtDebitAccountCode
            // 
            this.txtDebitAccountCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtDebitAccountCode.BorderRadius = 3;
            this.txtDebitAccountCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDebitAccountCode.DefaultText = "";
            this.txtDebitAccountCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDebitAccountCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDebitAccountCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDebitAccountCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDebitAccountCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtDebitAccountCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDebitAccountCode.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.txtDebitAccountCode.ForeColor = System.Drawing.Color.Black;
            this.txtDebitAccountCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDebitAccountCode.Location = new System.Drawing.Point(17, 39);
            this.txtDebitAccountCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDebitAccountCode.Name = "txtDebitAccountCode";
            this.txtDebitAccountCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtDebitAccountCode.PlaceholderText = "";
            this.txtDebitAccountCode.SelectedText = "";
            this.txtDebitAccountCode.Size = new System.Drawing.Size(118, 25);
            this.txtDebitAccountCode.TabIndex = 27;
            this.txtDebitAccountCode.TextChanged += new System.EventHandler(this.txtDebitCode_TextChanged);
            // 
            // cmbDebitCostCenter
            // 
            this.cmbDebitCostCenter.BackColor = System.Drawing.Color.Transparent;
            this.cmbDebitCostCenter.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbDebitCostCenter.BorderRadius = 3;
            this.cmbDebitCostCenter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDebitCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDebitCostCenter.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbDebitCostCenter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDebitCostCenter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbDebitCostCenter.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cmbDebitCostCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDebitCostCenter.ItemHeight = 18;
            this.cmbDebitCostCenter.Location = new System.Drawing.Point(391, 38);
            this.cmbDebitCostCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbDebitCostCenter.Name = "cmbDebitCostCenter";
            this.cmbDebitCostCenter.Size = new System.Drawing.Size(157, 24);
            this.cmbDebitCostCenter.TabIndex = 22;
            this.cmbDebitCostCenter.SelectedIndexChanged += new System.EventHandler(this.cmbDebitCostCenter_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label10.Location = new System.Drawing.Point(16, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(85, 28);
            this.label10.TabIndex = 14;
            this.label10.Text = "Account Code";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label7.Location = new System.Drawing.Point(389, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 28);
            this.label7.TabIndex = 23;
            this.label7.Text = "Cost Center";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBank
            // 
            this.pnlBank.BackColor = System.Drawing.Color.Transparent;
            this.pnlBank.Controls.Add(this.dt_check_date);
            this.pnlBank.Controls.Add(this.txtBankCode);
            this.pnlBank.Controls.Add(this.label5);
            this.pnlBank.Controls.Add(this.txtCheckNo);
            this.pnlBank.Controls.Add(this.cmbBankName);
            this.pnlBank.Controls.Add(this.txtCheckName);
            this.pnlBank.Controls.Add(this.label11);
            this.pnlBank.Controls.Add(this.label8);
            this.pnlBank.Controls.Add(this.label12);
            this.pnlBank.Controls.Add(this.lblBankName);
            this.pnlBank.Location = new System.Drawing.Point(550, 3);
            this.pnlBank.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBank.Name = "pnlBank";
            this.pnlBank.Size = new System.Drawing.Size(401, 112);
            this.pnlBank.TabIndex = 27;
            this.pnlBank.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlBank_Paint);
            // 
            // dt_check_date
            // 
            this.dt_check_date.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dt_check_date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dt_check_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dt_check_date.Location = new System.Drawing.Point(262, 35);
            this.dt_check_date.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dt_check_date.Name = "dt_check_date";
            this.dt_check_date.Size = new System.Drawing.Size(120, 22);
            this.dt_check_date.TabIndex = 26;
            this.dt_check_date.ValueChanged += new System.EventHandler(this.dt_check_date_ValueChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(3, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Cheque Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCheckNo
            // 
            this.txtCheckNo.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtCheckNo.BorderRadius = 3;
            this.txtCheckNo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCheckNo.DefaultText = "";
            this.txtCheckNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCheckNo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCheckNo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCheckNo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCheckNo.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCheckNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCheckNo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckNo.ForeColor = System.Drawing.Color.Black;
            this.txtCheckNo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCheckNo.Location = new System.Drawing.Point(179, 35);
            this.txtCheckNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCheckNo.Name = "txtCheckNo";
            this.txtCheckNo.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCheckNo.PlaceholderText = "Cheque No";
            this.txtCheckNo.SelectedText = "";
            this.txtCheckNo.Size = new System.Drawing.Size(81, 26);
            this.txtCheckNo.TabIndex = 20;
            // 
            // cmbBankName
            // 
            this.cmbBankName.BackColor = System.Drawing.Color.Transparent;
            this.cmbBankName.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbBankName.BorderRadius = 3;
            this.cmbBankName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBankName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankName.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbBankName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBankName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBankName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBankName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbBankName.ItemHeight = 18;
            this.cmbBankName.Location = new System.Drawing.Point(6, 76);
            this.cmbBankName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbBankName.Name = "cmbBankName";
            this.cmbBankName.Size = new System.Drawing.Size(254, 24);
            this.cmbBankName.TabIndex = 16;
            this.cmbBankName.Visible = false;
            this.cmbBankName.SelectedIndexChanged += new System.EventHandler(this.cmbBankName_SelectedIndexChanged);
            // 
            // txtCheckName
            // 
            this.txtCheckName.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtCheckName.BorderRadius = 3;
            this.txtCheckName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCheckName.DefaultText = "";
            this.txtCheckName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtCheckName.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtCheckName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCheckName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtCheckName.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtCheckName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCheckName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCheckName.ForeColor = System.Drawing.Color.Black;
            this.txtCheckName.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCheckName.Location = new System.Drawing.Point(6, 36);
            this.txtCheckName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCheckName.Name = "txtCheckName";
            this.txtCheckName.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCheckName.PlaceholderText = "Cheque Name";
            this.txtCheckName.SelectedText = "";
            this.txtCheckName.Size = new System.Drawing.Size(171, 25);
            this.txtCheckName.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label11.Location = new System.Drawing.Point(259, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 20);
            this.label11.TabIndex = 18;
            this.label11.Text = "Bank Code";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label11.Visible = false;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label8.Location = new System.Drawing.Point(176, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Cheque No";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(260, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 13);
            this.label12.TabIndex = 25;
            this.label12.Text = "Cheque Date";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBankName
            // 
            this.lblBankName.BackColor = System.Drawing.Color.Transparent;
            this.lblBankName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBankName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblBankName.Location = new System.Drawing.Point(3, 63);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new System.Drawing.Size(96, 13);
            this.lblBankName.TabIndex = 18;
            this.lblBankName.Text = "Bank Name";
            this.lblBankName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblBankName.Visible = false;
            // 
            // pnlTrans
            // 
            this.pnlTrans.BackColor = System.Drawing.Color.White;
            this.pnlTrans.Controls.Add(this.dtpTransDate);
            this.pnlTrans.Controls.Add(this.txtTransRef);
            this.pnlTrans.Controls.Add(this.txtTransName);
            this.pnlTrans.Controls.Add(this.lblTransDate);
            this.pnlTrans.Controls.Add(this.lblTransName);
            this.pnlTrans.Controls.Add(this.lblTransRef);
            this.pnlTrans.Location = new System.Drawing.Point(550, 3);
            this.pnlTrans.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlTrans.Name = "pnlTrans";
            this.pnlTrans.Size = new System.Drawing.Size(185, 142);
            this.pnlTrans.TabIndex = 26;
            this.pnlTrans.Visible = false;
            // 
            // lblTransName
            // 
            this.lblTransName.BackColor = System.Drawing.Color.Transparent;
            this.lblTransName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblTransName.Location = new System.Drawing.Point(9, 61);
            this.lblTransName.Name = "lblTransName";
            this.lblTransName.Size = new System.Drawing.Size(95, 13);
            this.lblTransName.TabIndex = 25;
            this.lblTransName.Text = "TRNS / Name";
            this.lblTransName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTransRef
            // 
            this.lblTransRef.BackColor = System.Drawing.Color.Transparent;
            this.lblTransRef.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblTransRef.Location = new System.Drawing.Point(9, 99);
            this.lblTransRef.Name = "lblTransRef";
            this.lblTransRef.Size = new System.Drawing.Size(95, 13);
            this.lblTransRef.TabIndex = 25;
            this.lblTransRef.Text = "TRNS / REF";
            this.lblTransRef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // costCenter
            // 
            this.costCenter.HeaderText = "Cost Center";
            this.costCenter.MinimumWidth = 6;
            this.costCenter.Name = "costCenter";
            this.costCenter.Width = 160;
            // 
            // dgvInv
            // 
            this.dgvInv.AllowUserToAddRows = false;
            this.dgvInv.AllowUserToDeleteRows = false;
            this.dgvInv.AllowUserToResizeColumns = false;
            this.dgvInv.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvInv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvInv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvInv.BackgroundColor = System.Drawing.Color.WhiteSmoke;
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
            this.dgvInv.ColumnHeadersHeight = 18;
            this.dgvInv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvInv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.no,
            this.humId,
            this.invId,
            this.invDate,
            this.InvNo,
            this.Total,
            this.chkPay,
            this.Pay,
            this.Description});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInv.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvInv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvInv.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.Location = new System.Drawing.Point(2, 569);
            this.dgvInv.MultiSelect = false;
            this.dgvInv.Name = "dgvInv";
            this.dgvInv.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInv.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvInv.RowHeadersVisible = false;
            this.dgvInv.RowHeadersWidth = 51;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvInv.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvInv.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvInv.RowTemplate.Height = 25;
            this.dgvInv.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvInv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvInv.Size = new System.Drawing.Size(954, 36);
            this.dgvInv.TabIndex = 10;
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
            this.dgvInv.ThemeStyle.HeaderStyle.Height = 18;
            this.dgvInv.ThemeStyle.ReadOnly = false;
            this.dgvInv.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvInv.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvInv.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvInv.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.ThemeStyle.RowsStyle.Height = 25;
            this.dgvInv.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellContentClick);
            this.dgvInv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellValueChanged);
            this.dgvInv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvInv_EditingControlShowing);
            // 
            // no
            // 
            this.no.HeaderText = "SN#";
            this.no.MinimumWidth = 6;
            this.no.Name = "no";
            this.no.ReadOnly = true;
            this.no.Width = 45;
            // 
            // humId
            // 
            this.humId.HeaderText = "id";
            this.humId.MinimumWidth = 6;
            this.humId.Name = "humId";
            this.humId.Visible = false;
            this.humId.Width = 125;
            // 
            // invId
            // 
            this.invId.HeaderText = "id";
            this.invId.MinimumWidth = 6;
            this.invId.Name = "invId";
            this.invId.Visible = false;
            this.invId.Width = 125;
            // 
            // invDate
            // 
            this.invDate.HeaderText = "Date";
            this.invDate.MinimumWidth = 6;
            this.invDate.Name = "invDate";
            this.invDate.ReadOnly = true;
            this.invDate.Width = 120;
            // 
            // InvNo
            // 
            this.InvNo.HeaderText = "INV NO";
            this.InvNo.MinimumWidth = 6;
            this.InvNo.Name = "InvNo";
            this.InvNo.ReadOnly = true;
            this.InvNo.Width = 120;
            // 
            // Total
            // 
            this.Total.HeaderText = "Amount";
            this.Total.MinimumWidth = 6;
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 120;
            // 
            // chkPay
            // 
            this.chkPay.HeaderText = "Pay";
            this.chkPay.MinimumWidth = 6;
            this.chkPay.Name = "chkPay";
            this.chkPay.Width = 50;
            // 
            // Pay
            // 
            this.Pay.HeaderText = "Payment";
            this.Pay.MinimumWidth = 6;
            this.Pay.Name = "Pay";
            this.Pay.Width = 120;
            // 
            // Description
            // 
            this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
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
            this.headerUC1.Size = new System.Drawing.Size(954, 37);
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
            this.guna2Button2.Location = new System.Drawing.Point(894, 5);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(15, 15);
            this.guna2Button2.TabIndex = 26;
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
            this.guna2Button1.Location = new System.Drawing.Point(915, 5);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(15, 15);
            this.guna2Button1.TabIndex = 25;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(936, 5);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(15, 15);
            this.guna2ControlBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(956, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2, 640);
            this.panel2.TabIndex = 19;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(2, 638);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(954, 2);
            this.panel3.TabIndex = 20;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 640);
            this.panel4.TabIndex = 21;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.tabControl2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(2, 37);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(954, 84);
            this.panel9.TabIndex = 40;
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
            this.tabControl2.Size = new System.Drawing.Size(954, 84);
            this.tabControl2.TabIndex = 39;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.txtPVCode);
            this.tabPage3.Controls.Add(this.btnNext);
            this.tabPage3.Controls.Add(this.btnPrevious);
            this.tabPage3.Controls.Add(this.guna2VSeparator12);
            this.tabPage3.Controls.Add(this.btnNew);
            this.tabPage3.Controls.Add(this.guna2TileButton15);
            this.tabPage3.Controls.Add(this.btnSaveNew);
            this.tabPage3.Controls.Add(this.guna2VSeparator9);
            this.tabPage3.Controls.Add(this.btnDelete);
            this.tabPage3.Controls.Add(this.guna2TileButton16);
            this.tabPage3.Controls.Add(this.btnCopy);
            this.tabPage3.Controls.Add(this.guna2VSeparator10);
            this.tabPage3.Controls.Add(this.guna2TileButton19);
            this.tabPage3.Controls.Add(this.guna2VSeparator11);
            this.tabPage3.Controls.Add(this.chkPrint);
            this.tabPage3.Controls.Add(this.btnPrint);
            this.tabPage3.Controls.Add(this.btnEmail);
            this.tabPage3.Controls.Add(this.txtId);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(946, 58);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Main";
            // 
            // txtPVCode
            // 
            this.txtPVCode.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtPVCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPVCode.Enabled = false;
            this.txtPVCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPVCode.Location = new System.Drawing.Point(33, 15);
            this.txtPVCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPVCode.Multiline = true;
            this.txtPVCode.Name = "txtPVCode";
            this.txtPVCode.Size = new System.Drawing.Size(66, 29);
            this.txtPVCode.TabIndex = 16;
            this.txtPVCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.BorderColor = System.Drawing.Color.Transparent;
            this.btnNew.BorderRadius = 10;
            this.btnNew.BorderThickness = 2;
            this.btnNew.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnNew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNew.FillColor = System.Drawing.Color.Transparent;
            this.btnNew.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ForeColor = System.Drawing.Color.Black;
            this.btnNew.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.btnNew.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnNew.Image = global::YamyProject.Properties.Resources.add_fileN;
            this.btnNew.Location = new System.Drawing.Point(139, 8);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(54, 47);
            this.btnNew.TabIndex = 19;
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
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
            this.guna2TileButton15.Location = new System.Drawing.Point(675, 7);
            this.guna2TileButton15.Name = "guna2TileButton15";
            this.guna2TileButton15.Size = new System.Drawing.Size(100, 23);
            this.guna2TileButton15.TabIndex = 32;
            this.guna2TileButton15.Text = "Add Time / Costs";
            this.guna2TileButton15.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton15.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton15.Visible = false;
            // 
            // btnSaveNew
            // 
            this.btnSaveNew.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveNew.BorderColor = System.Drawing.Color.Transparent;
            this.btnSaveNew.BorderRadius = 10;
            this.btnSaveNew.BorderThickness = 2;
            this.btnSaveNew.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnSaveNew.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSaveNew.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSaveNew.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSaveNew.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSaveNew.FillColor = System.Drawing.Color.Transparent;
            this.btnSaveNew.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveNew.ForeColor = System.Drawing.Color.Black;
            this.btnSaveNew.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.btnSaveNew.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnSaveNew.Image = global::YamyProject.Properties.Resources.Save;
            this.btnSaveNew.Location = new System.Drawing.Point(193, 8);
            this.btnSaveNew.Name = "btnSaveNew";
            this.btnSaveNew.Size = new System.Drawing.Size(54, 47);
            this.btnSaveNew.TabIndex = 20;
            this.btnSaveNew.Text = "Save";
            this.btnSaveNew.Click += new System.EventHandler(this.btnSaveNew_Click);
            // 
            // guna2VSeparator9
            // 
            this.guna2VSeparator9.Location = new System.Drawing.Point(667, 6);
            this.guna2VSeparator9.Name = "guna2VSeparator9";
            this.guna2VSeparator9.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator9.TabIndex = 31;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.BorderColor = System.Drawing.Color.Transparent;
            this.btnDelete.BorderRadius = 10;
            this.btnDelete.BorderThickness = 2;
            this.btnDelete.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnDelete.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnDelete.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnDelete.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnDelete.FillColor = System.Drawing.Color.Transparent;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.btnDelete.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnDelete.Image = global::YamyProject.Properties.Resources.deleteN;
            this.btnDelete.Location = new System.Drawing.Point(247, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(54, 47);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // guna2TileButton16
            // 
            this.guna2TileButton16.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton16.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton16.BorderRadius = 10;
            this.guna2TileButton16.BorderThickness = 2;
            this.guna2TileButton16.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton16.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton16.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton16.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton16.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton16.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton16.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton16.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton16.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton16.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton16.Image = global::YamyProject.Properties.Resources.attachN;
            this.guna2TileButton16.Location = new System.Drawing.Point(613, 7);
            this.guna2TileButton16.Name = "guna2TileButton16";
            this.guna2TileButton16.Size = new System.Drawing.Size(54, 47);
            this.guna2TileButton16.TabIndex = 30;
            this.guna2TileButton16.Text = "Attach File";
            this.guna2TileButton16.Visible = false;
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.Transparent;
            this.btnCopy.BorderColor = System.Drawing.Color.Transparent;
            this.btnCopy.BorderRadius = 10;
            this.btnCopy.BorderThickness = 2;
            this.btnCopy.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnCopy.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCopy.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCopy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCopy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCopy.FillColor = System.Drawing.Color.Transparent;
            this.btnCopy.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.ForeColor = System.Drawing.Color.Black;
            this.btnCopy.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.btnCopy.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnCopy.Image = global::YamyProject.Properties.Resources.copyN;
            this.btnCopy.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCopy.ImageOffset = new System.Drawing.Point(-10, 10);
            this.btnCopy.Location = new System.Drawing.Point(301, 8);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 23);
            this.btnCopy.TabIndex = 22;
            this.btnCopy.Text = "Create a Copy";
            this.btnCopy.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnCopy.TextOffset = new System.Drawing.Point(10, -12);
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
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
            this.guna2TileButton19.Location = new System.Drawing.Point(300, 31);
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
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.Transparent;
            this.btnPrint.BorderColor = System.Drawing.Color.Transparent;
            this.btnPrint.BorderRadius = 10;
            this.btnPrint.BorderThickness = 2;
            this.btnPrint.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnPrint.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPrint.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPrint.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPrint.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPrint.FillColor = System.Drawing.Color.Transparent;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.btnPrint.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnPrint.Image = global::YamyProject.Properties.Resources.printN;
            this.btnPrint.Location = new System.Drawing.Point(409, 8);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(54, 47);
            this.btnPrint.TabIndex = 25;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.guna2TileButton18_Click);
            // 
            // btnEmail
            // 
            this.btnEmail.BackColor = System.Drawing.Color.Transparent;
            this.btnEmail.BorderColor = System.Drawing.Color.Transparent;
            this.btnEmail.BorderRadius = 10;
            this.btnEmail.BorderThickness = 2;
            this.btnEmail.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.btnEmail.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnEmail.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnEmail.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnEmail.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnEmail.FillColor = System.Drawing.Color.Transparent;
            this.btnEmail.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEmail.ForeColor = System.Drawing.Color.Black;
            this.btnEmail.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.btnEmail.HoverState.FillColor = System.Drawing.Color.Silver;
            this.btnEmail.Image = global::YamyProject.Properties.Resources.emailN;
            this.btnEmail.Location = new System.Drawing.Point(463, 8);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(54, 47);
            this.btnEmail.TabIndex = 26;
            this.btnEmail.Text = "Email";
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtId.Enabled = false;
            this.txtId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtId.Location = new System.Drawing.Point(33, 15);
            this.txtId.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtId.Multiline = true;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(66, 29);
            this.txtId.TabIndex = 34;
            this.txtId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtId.Visible = false;
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
            this.tabPage4.Size = new System.Drawing.Size(946, 58);
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
            // frmViewReceiptVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(958, 640);
            this.Controls.Add(this.dgvInv);
            this.Controls.Add(this.guna2GroupBox3);
            this.Controls.Add(this.guna2GroupBox2);
            this.Controls.Add(this.guna2GroupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lstAccountSuggestions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmViewReceiptVoucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Receipt Voucher";
            this.Load += new System.EventHandler(this.frmViewReceiptVoucher_Load);
            this.panel1.ResumeLayout(false);
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.guna2GroupBox2.ResumeLayout(false);
            this.guna2ShadowPanel2.ResumeLayout(false);
            this.pnlCustomer.ResumeLayout(false);
            this.guna2GroupBox3.ResumeLayout(false);
            this.pnlBank.ResumeLayout(false);
            this.pnlTrans.ResumeLayout(false);
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
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox2;
        public Guna.UI2.WinForms.Guna2ComboBox cmbPaymentType;
        private System.Windows.Forms.Label label1;
        public Guna.UI2.WinForms.Guna2ComboBox cmbMethod;
        private System.Windows.Forms.Label lblBalance;
        private System.Windows.Forms.Label lblTransDate;
        private System.Windows.Forms.DateTimePicker dtpTransDate;
        private Guna.UI2.WinForms.Guna2TextBox txtTransName;
        private Guna.UI2.WinForms.Guna2TextBox txtTransRef;
        private Guna.UI2.WinForms.Guna2TextBox txtBankCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox3;
        private System.Windows.Forms.DateTimePicker dtOpen;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtAmount;
        private Guna.UI2.WinForms.Guna2TextBox txtAmountInWord;
        private System.Windows.Forms.Label lblCostcenter;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCreditCostCenter;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDebitCostCenter;
        private Guna.UI2.WinForms.Guna2ComboBox cmbBankName;
        private System.Windows.Forms.Label lblTransRef;
        private System.Windows.Forms.Label lblBankName;
        private System.Windows.Forms.Label lblTransName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlBank;
        private System.Windows.Forms.Panel pnlTrans;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewComboBoxColumn costCenter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox txtCheckNo;
        private Guna.UI2.WinForms.Guna2TextBox txtCheckName;
        private Guna.UI2.WinForms.Guna2TextBox txtCreditAccountCode;
        private Guna.UI2.WinForms.Guna2TextBox txtDebitAccountCode;
        private System.Windows.Forms.ComboBox cmbCreditAccountName;
        private System.Windows.Forms.ComboBox cmbDebitAccountName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dt_check_date;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel pnlCustomer;
        private Guna.UI2.WinForms.Guna2TextBox txtCustomerCode;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCustomer;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.Label lblCustomer;
        private Guna.UI2.WinForms.Guna2DataGridView dgvInv;
        private System.Windows.Forms.RichTextBox txtDescription;
        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
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
        private Guna.UI2.WinForms.Guna2TileButton btnNew;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton15;
        private Guna.UI2.WinForms.Guna2TileButton btnSaveNew;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator9;
        private Guna.UI2.WinForms.Guna2TileButton btnDelete;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton16;
        private Guna.UI2.WinForms.Guna2TileButton btnCopy;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator10;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton19;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator11;
        private System.Windows.Forms.CheckBox chkPrint;
        private Guna.UI2.WinForms.Guna2TileButton btnPrint;
        private Guna.UI2.WinForms.Guna2TileButton btnEmail;
        private System.Windows.Forms.TabPage tabPage4;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton27;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton28;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton29;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator13;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton26;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton25;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton24;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel2;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private DataGridViewTextBoxColumn no;
        private DataGridViewTextBoxColumn humId;
        private DataGridViewTextBoxColumn invId;
        private DataGridViewTextBoxColumn invDate;
        private DataGridViewTextBoxColumn InvNo;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewCheckBoxColumn chkPay;
        private DataGridViewTextBoxColumn Pay;
        private DataGridViewTextBoxColumn Description;
        private ListBox lstAccountSuggestions;
        private TextBox txtId;
    }
}