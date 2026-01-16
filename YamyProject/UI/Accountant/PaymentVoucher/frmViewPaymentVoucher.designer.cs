using System.ComponentModel;
using System.Windows.Forms;

namespace YamyProject
{
    partial class frmViewPaymentVoucher
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
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.txtcostcenter = new Guna.UI2.WinForms.Guna2GroupBox();
            this.CbSubcontractors = new Guna.UI2.WinForms.Guna2CheckBox();
            this.cbpettycash = new Guna.UI2.WinForms.Guna2CheckBox();
            this.txtAmount = new Guna.UI2.WinForms.Guna2TextBox();
            this.dtOpen = new System.Windows.Forms.DateTimePicker();
            this.txtAmountInWord = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbPaymentType = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbMethod = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblBalance = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2GroupBox2 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2ShadowPanel1 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.txtDebitCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbDebitCostCenter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDebitAccountName = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCostcenter = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDescription1 = new System.Windows.Forms.RichTextBox();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.lstOfEmp = new System.Windows.Forms.CheckedListBox();
            this.pnlCustomer = new System.Windows.Forms.Panel();
            this.txtVendor = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbVendor = new System.Windows.Forms.ComboBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblTransDate = new System.Windows.Forms.Label();
            this.dtpTransDate = new System.Windows.Forms.DateTimePicker();
            this.txtTransName = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTransRef = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtBankCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtCreditAccountCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCreditAccountName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.guna2GroupBox3 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2ShadowPanel2 = new Guna.UI2.WinForms.Guna2ShadowPanel();
            this.cmbCreditCostCenter = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlBank = new System.Windows.Forms.Panel();
            this.cmbCheckNo = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbBookNo = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbBankAccountName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dt_check_date = new System.Windows.Forms.DateTimePicker();
            this.cmbBankName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblBankName = new System.Windows.Forms.Label();
            this.txtCheckName = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblBookNo = new System.Windows.Forms.Label();
            this.lblBankAccountName = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlTrans = new System.Windows.Forms.Panel();
            this.lblTransName = new System.Windows.Forms.Label();
            this.lblTransRef = new System.Windows.Forms.Label();
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
            this.voucherType = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.txtPVCode = new System.Windows.Forms.TextBox();
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
            this.txtId = new System.Windows.Forms.TextBox();
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
            this.panel1.SuspendLayout();
            this.txtcostcenter.SuspendLayout();
            this.guna2GroupBox2.SuspendLayout();
            this.guna2ShadowPanel1.SuspendLayout();
            this.pnlCustomer.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.guna2ShadowPanel2.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(899, 33);
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
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button3.ForeColor = System.Drawing.Color.White;
            this.guna2Button3.Location = new System.Drawing.Point(636, 4);
            this.guna2Button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(103, 27);
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
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(823, 4);
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
            this.btnSave.Location = new System.Drawing.Point(749, 4);
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
            this.txtcostcenter.Controls.Add(this.CbSubcontractors);
            this.txtcostcenter.Controls.Add(this.cbpettycash);
            this.txtcostcenter.Controls.Add(this.txtAmount);
            this.txtcostcenter.Controls.Add(this.dtOpen);
            this.txtcostcenter.Controls.Add(this.txtAmountInWord);
            this.txtcostcenter.Controls.Add(this.cmbPaymentType);
            this.txtcostcenter.Controls.Add(this.cmbMethod);
            this.txtcostcenter.Controls.Add(this.label2);
            this.txtcostcenter.Controls.Add(this.lblBalance);
            this.txtcostcenter.Controls.Add(this.label1);
            this.txtcostcenter.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.txtcostcenter.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.txtcostcenter.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtcostcenter.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtcostcenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtcostcenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtcostcenter.Location = new System.Drawing.Point(2, 121);
            this.txtcostcenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtcostcenter.Name = "txtcostcenter";
            this.txtcostcenter.Size = new System.Drawing.Size(899, 83);
            this.txtcostcenter.TabIndex = 3;
            this.txtcostcenter.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // CbSubcontractors
            // 
            this.CbSubcontractors.AutoSize = true;
            this.CbSubcontractors.BackColor = System.Drawing.Color.White;
            this.CbSubcontractors.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.CbSubcontractors.CheckedState.BorderRadius = 0;
            this.CbSubcontractors.CheckedState.BorderThickness = 0;
            this.CbSubcontractors.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.CbSubcontractors.ForeColor = System.Drawing.Color.Black;
            this.CbSubcontractors.Location = new System.Drawing.Point(737, 18);
            this.CbSubcontractors.Margin = new System.Windows.Forms.Padding(2);
            this.CbSubcontractors.Name = "CbSubcontractors";
            this.CbSubcontractors.Size = new System.Drawing.Size(104, 17);
            this.CbSubcontractors.TabIndex = 35;
            this.CbSubcontractors.Text = "Subcontractors";
            this.CbSubcontractors.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.CbSubcontractors.UncheckedState.BorderRadius = 0;
            this.CbSubcontractors.UncheckedState.BorderThickness = 0;
            this.CbSubcontractors.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.CbSubcontractors.UseVisualStyleBackColor = false;
            this.CbSubcontractors.CheckedChanged += new System.EventHandler(this.CbSubcontractors_CheckedChanged);
            // 
            // cbpettycash
            // 
            this.cbpettycash.AutoSize = true;
            this.cbpettycash.BackColor = System.Drawing.Color.White;
            this.cbpettycash.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbpettycash.CheckedState.BorderRadius = 0;
            this.cbpettycash.CheckedState.BorderThickness = 0;
            this.cbpettycash.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbpettycash.Enabled = false;
            this.cbpettycash.ForeColor = System.Drawing.Color.Black;
            this.cbpettycash.Location = new System.Drawing.Point(737, 54);
            this.cbpettycash.Margin = new System.Windows.Forms.Padding(2);
            this.cbpettycash.Name = "cbpettycash";
            this.cbpettycash.Size = new System.Drawing.Size(15, 14);
            this.cbpettycash.TabIndex = 34;
            this.cbpettycash.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.cbpettycash.UncheckedState.BorderRadius = 0;
            this.cbpettycash.UncheckedState.BorderThickness = 0;
            this.cbpettycash.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.cbpettycash.UseVisualStyleBackColor = false;
            this.cbpettycash.Visible = false;
            this.cbpettycash.CheckedChanged += new System.EventHandler(this.cbpettycash_CheckedChanged);
            // 
            // txtAmount
            // 
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
            this.txtAmount.Location = new System.Drawing.Point(5, 54);
            this.txtAmount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAmount.PlaceholderText = "Amount";
            this.txtAmount.SelectedText = "";
            this.txtAmount.Size = new System.Drawing.Size(152, 25);
            this.txtAmount.TabIndex = 27;
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
            // 
            // dtOpen
            // 
            this.dtOpen.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtOpen.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtOpen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtOpen.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtOpen.Location = new System.Drawing.Point(5, 14);
            this.dtOpen.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtOpen.Name = "dtOpen";
            this.dtOpen.Size = new System.Drawing.Size(153, 22);
            this.dtOpen.TabIndex = 14;
            // 
            // txtAmountInWord
            // 
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
            this.txtAmountInWord.Location = new System.Drawing.Point(163, 54);
            this.txtAmountInWord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAmountInWord.Name = "txtAmountInWord";
            this.txtAmountInWord.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtAmountInWord.PlaceholderText = "Amount In Word";
            this.txtAmountInWord.SelectedText = "";
            this.txtAmountInWord.Size = new System.Drawing.Size(356, 25);
            this.txtAmountInWord.TabIndex = 26;
            // 
            // cmbPaymentType
            // 
            this.cmbPaymentType.BackColor = System.Drawing.Color.Transparent;
            this.cmbPaymentType.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbPaymentType.BorderRadius = 3;
            this.cmbPaymentType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentType.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbPaymentType.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentType.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentType.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cmbPaymentType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPaymentType.ItemHeight = 18;
            this.cmbPaymentType.Items.AddRange(new object[] {
            "Vendor",
            "Employee",
            "General"});
            this.cmbPaymentType.Location = new System.Drawing.Point(523, 12);
            this.cmbPaymentType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPaymentType.Name = "cmbPaymentType";
            this.cmbPaymentType.Size = new System.Drawing.Size(209, 24);
            this.cmbPaymentType.TabIndex = 11;
            this.cmbPaymentType.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentType_SelectedIndexChanged);
            // 
            // cmbMethod
            // 
            this.cmbMethod.BackColor = System.Drawing.Color.Transparent;
            this.cmbMethod.BorderColor = System.Drawing.SystemColors.ActiveBorder;
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
            this.cmbMethod.Location = new System.Drawing.Point(524, 53);
            this.cmbMethod.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(209, 24);
            this.cmbMethod.TabIndex = 9;
            this.cmbMethod.SelectedIndexChanged += new System.EventHandler(this.cmbMethod_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(4, -10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 29);
            this.label2.TabIndex = 10;
            this.label2.Text = "Date";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBalance
            // 
            this.lblBalance.BackColor = System.Drawing.Color.Transparent;
            this.lblBalance.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblBalance.Location = new System.Drawing.Point(521, 30);
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
            this.label1.Location = new System.Drawing.Point(520, -10);
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
            this.guna2GroupBox2.Controls.Add(this.guna2ShadowPanel1);
            this.guna2GroupBox2.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox2.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox2.Location = new System.Drawing.Point(2, 204);
            this.guna2GroupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox2.Name = "guna2GroupBox2";
            this.guna2GroupBox2.Size = new System.Drawing.Size(899, 173);
            this.guna2GroupBox2.TabIndex = 4;
            this.guna2GroupBox2.Text = "Pay To - Debit";
            this.guna2GroupBox2.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // guna2ShadowPanel1
            // 
            this.guna2ShadowPanel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel1.Controls.Add(this.txtDebitCode);
            this.guna2ShadowPanel1.Controls.Add(this.cmbDebitCostCenter);
            this.guna2ShadowPanel1.Controls.Add(this.label3);
            this.guna2ShadowPanel1.Controls.Add(this.cmbDebitAccountName);
            this.guna2ShadowPanel1.Controls.Add(this.label9);
            this.guna2ShadowPanel1.Controls.Add(this.lblCostcenter);
            this.guna2ShadowPanel1.Controls.Add(this.label4);
            this.guna2ShadowPanel1.Controls.Add(this.txtDescription1);
            this.guna2ShadowPanel1.Controls.Add(this.guna2TextBox1);
            this.guna2ShadowPanel1.Controls.Add(this.lstOfEmp);
            this.guna2ShadowPanel1.Controls.Add(this.pnlCustomer);
            this.guna2ShadowPanel1.FillColor = System.Drawing.Color.White;
            this.guna2ShadowPanel1.Location = new System.Drawing.Point(5, 23);
            this.guna2ShadowPanel1.Name = "guna2ShadowPanel1";
            this.guna2ShadowPanel1.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel1.Size = new System.Drawing.Size(888, 150);
            this.guna2ShadowPanel1.TabIndex = 34;
            // 
            // txtDebitCode
            // 
            this.txtDebitCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtDebitCode.BorderRadius = 3;
            this.txtDebitCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDebitCode.DefaultText = "";
            this.txtDebitCode.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtDebitCode.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtDebitCode.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDebitCode.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtDebitCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtDebitCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDebitCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDebitCode.ForeColor = System.Drawing.Color.Black;
            this.txtDebitCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtDebitCode.Location = new System.Drawing.Point(8, 25);
            this.txtDebitCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDebitCode.Name = "txtDebitCode";
            this.txtDebitCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtDebitCode.PlaceholderText = "";
            this.txtDebitCode.SelectedText = "";
            this.txtDebitCode.Size = new System.Drawing.Size(152, 25);
            this.txtDebitCode.TabIndex = 24;
            this.txtDebitCode.TextChanged += new System.EventHandler(this.txtDebitCode_TextChanged);
            this.txtDebitCode.Leave += new System.EventHandler(this.txtDebitCode_Leave);
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
            this.cmbDebitCostCenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDebitCostCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDebitCostCenter.ItemHeight = 18;
            this.cmbDebitCostCenter.Location = new System.Drawing.Point(399, 24);
            this.cmbDebitCostCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbDebitCostCenter.Name = "cmbDebitCostCenter";
            this.cmbDebitCostCenter.Size = new System.Drawing.Size(115, 24);
            this.cmbDebitCostCenter.TabIndex = 22;
            this.cmbDebitCostCenter.SelectedIndexChanged += new System.EventHandler(this.cmbDebitCostCenter_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(7, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 18);
            this.label3.TabIndex = 28;
            this.label3.Text = "Note";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDebitAccountName
            // 
            this.cmbDebitAccountName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDebitAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDebitAccountName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDebitAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbDebitAccountName.Location = new System.Drawing.Point(166, 27);
            this.cmbDebitAccountName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbDebitAccountName.Name = "cmbDebitAccountName";
            this.cmbDebitAccountName.Size = new System.Drawing.Size(227, 21);
            this.cmbDebitAccountName.TabIndex = 22;
            this.cmbDebitAccountName.SelectedIndexChanged += new System.EventHandler(this.cmbDebitAccountName_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label9.Location = new System.Drawing.Point(8, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(85, 24);
            this.label9.TabIndex = 23;
            this.label9.Text = "Account Code";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCostcenter
            // 
            this.lblCostcenter.BackColor = System.Drawing.Color.Transparent;
            this.lblCostcenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCostcenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblCostcenter.Location = new System.Drawing.Point(412, 8);
            this.lblCostcenter.Name = "lblCostcenter";
            this.lblCostcenter.Size = new System.Drawing.Size(75, 16);
            this.lblCostcenter.TabIndex = 23;
            this.lblCostcenter.Text = "Cost Center";
            this.lblCostcenter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label4.Location = new System.Drawing.Point(166, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 24);
            this.label4.TabIndex = 23;
            this.label4.Text = "Account Name";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescription1
            // 
            this.txtDescription1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtDescription1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription1.Location = new System.Drawing.Point(10, 68);
            this.txtDescription1.Name = "txtDescription1";
            this.txtDescription1.Size = new System.Drawing.Size(502, 61);
            this.txtDescription1.TabIndex = 32;
            this.txtDescription1.Text = "";
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
            this.guna2TextBox1.Location = new System.Drawing.Point(8, 65);
            this.guna2TextBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.guna2TextBox1.PlaceholderText = "";
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(506, 66);
            this.guna2TextBox1.TabIndex = 33;
            // 
            // lstOfEmp
            // 
            this.lstOfEmp.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstOfEmp.FormattingEnabled = true;
            this.lstOfEmp.Location = new System.Drawing.Point(518, 26);
            this.lstOfEmp.Name = "lstOfEmp";
            this.lstOfEmp.Size = new System.Drawing.Size(266, 55);
            this.lstOfEmp.TabIndex = 30;
            this.lstOfEmp.Visible = false;
            this.lstOfEmp.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lstOfEmp_ItemCheck);
            this.lstOfEmp.SelectedIndexChanged += new System.EventHandler(this.lstOfEmp_SelectedIndexChanged);
            // 
            // pnlCustomer
            // 
            this.pnlCustomer.BackColor = System.Drawing.Color.Transparent;
            this.pnlCustomer.Controls.Add(this.txtVendor);
            this.pnlCustomer.Controls.Add(this.cmbVendor);
            this.pnlCustomer.Controls.Add(this.lblCode);
            this.pnlCustomer.Controls.Add(this.lblCustomer);
            this.pnlCustomer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlCustomer.Location = new System.Drawing.Point(519, -4);
            this.pnlCustomer.Name = "pnlCustomer";
            this.pnlCustomer.Size = new System.Drawing.Size(313, 102);
            this.pnlCustomer.TabIndex = 31;
            // 
            // txtVendor
            // 
            this.txtVendor.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtVendor.BorderRadius = 3;
            this.txtVendor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVendor.DefaultText = "";
            this.txtVendor.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtVendor.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtVendor.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVendor.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtVendor.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtVendor.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVendor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVendor.ForeColor = System.Drawing.Color.Black;
            this.txtVendor.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVendor.Location = new System.Drawing.Point(3, 28);
            this.txtVendor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVendor.Name = "txtVendor";
            this.txtVendor.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtVendor.PlaceholderText = "Vendor Code";
            this.txtVendor.SelectedText = "";
            this.txtVendor.Size = new System.Drawing.Size(209, 25);
            this.txtVendor.TabIndex = 27;
            this.txtVendor.TextChanged += new System.EventHandler(this.txtVendor_TextChanged);
            this.txtVendor.Leave += new System.EventHandler(this.txtVendor_Leave);
            // 
            // cmbVendor
            // 
            this.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVendor.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVendor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbVendor.Location = new System.Drawing.Point(4, 69);
            this.cmbVendor.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(289, 21);
            this.cmbVendor.TabIndex = 25;
            this.cmbVendor.SelectedIndexChanged += new System.EventHandler(this.cmbVendor_SelectedIndexChanged);
            // 
            // lblCode
            // 
            this.lblCode.BackColor = System.Drawing.Color.Transparent;
            this.lblCode.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblCode.Location = new System.Drawing.Point(3, 7);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(101, 24);
            this.lblCode.TabIndex = 26;
            this.lblCode.Text = "Vendor Code";
            this.lblCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lblCustomer.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblCustomer.Location = new System.Drawing.Point(4, 49);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(101, 24);
            this.lblCustomer.TabIndex = 26;
            this.lblCustomer.Text = "Vendor Name";
            this.lblCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTransDate
            // 
            this.lblTransDate.BackColor = System.Drawing.Color.Transparent;
            this.lblTransDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblTransDate.Location = new System.Drawing.Point(9, -2);
            this.lblTransDate.Name = "lblTransDate";
            this.lblTransDate.Size = new System.Drawing.Size(95, 28);
            this.lblTransDate.TabIndex = 25;
            this.lblTransDate.Text = "Transfer Date";
            this.lblTransDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTransDate
            // 
            this.dtpTransDate.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dtpTransDate.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTransDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtpTransDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTransDate.Location = new System.Drawing.Point(9, 24);
            this.dtpTransDate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpTransDate.Name = "dtpTransDate";
            this.dtpTransDate.Size = new System.Drawing.Size(169, 22);
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
            this.txtTransName.Location = new System.Drawing.Point(9, 65);
            this.txtTransName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTransName.Name = "txtTransName";
            this.txtTransName.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTransName.PlaceholderText = "TRNS / Name";
            this.txtTransName.SelectedText = "";
            this.txtTransName.Size = new System.Drawing.Size(168, 25);
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
            this.txtTransRef.Location = new System.Drawing.Point(9, 106);
            this.txtTransRef.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTransRef.Name = "txtTransRef";
            this.txtTransRef.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTransRef.PlaceholderText = "TRNS / REF";
            this.txtTransRef.SelectedText = "";
            this.txtTransRef.Size = new System.Drawing.Size(168, 25);
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
            this.txtBankCode.Location = new System.Drawing.Point(4, 26);
            this.txtBankCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBankCode.Name = "txtBankCode";
            this.txtBankCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtBankCode.PlaceholderText = "Bank Code";
            this.txtBankCode.SelectedText = "";
            this.txtBankCode.Size = new System.Drawing.Size(110, 25);
            this.txtBankCode.TabIndex = 24;
            this.txtBankCode.TextChanged += new System.EventHandler(this.txtBankCode_TextChanged);
            this.txtBankCode.Leave += new System.EventHandler(this.txtBankCode_Leave);
            // 
            // txtCreditAccountCode
            // 
            this.txtCreditAccountCode.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtCreditAccountCode.BorderRadius = 3;
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
            this.txtCreditAccountCode.Location = new System.Drawing.Point(4, 20);
            this.txtCreditAccountCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCreditAccountCode.Name = "txtCreditAccountCode";
            this.txtCreditAccountCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCreditAccountCode.PlaceholderText = "";
            this.txtCreditAccountCode.SelectedText = "";
            this.txtCreditAccountCode.Size = new System.Drawing.Size(130, 25);
            this.txtCreditAccountCode.TabIndex = 15;
            this.txtCreditAccountCode.TextChanged += new System.EventHandler(this.txtCreditAccountCode_TextChanged);
            this.txtCreditAccountCode.Leave += new System.EventHandler(this.txtCreditAccountCode_Leave);
            // 
            // cmbCreditAccountName
            // 
            this.cmbCreditAccountName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCreditAccountName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCreditAccountName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCreditAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCreditAccountName.Location = new System.Drawing.Point(139, 22);
            this.cmbCreditAccountName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCreditAccountName.Name = "cmbCreditAccountName";
            this.cmbCreditAccountName.Size = new System.Drawing.Size(230, 21);
            this.cmbCreditAccountName.TabIndex = 13;
            this.cmbCreditAccountName.SelectedIndexChanged += new System.EventHandler(this.cmbCreditAccountName_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label6.Location = new System.Drawing.Point(139, -2);
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
            this.guna2GroupBox3.Controls.Add(this.guna2ShadowPanel2);
            this.guna2GroupBox3.CustomBorderColor = System.Drawing.Color.White;
            this.guna2GroupBox3.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2GroupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox3.Location = new System.Drawing.Point(2, 377);
            this.guna2GroupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Size = new System.Drawing.Size(899, 165);
            this.guna2GroupBox3.TabIndex = 5;
            this.guna2GroupBox3.Text = "Pay From - Credit";
            this.guna2GroupBox3.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // guna2ShadowPanel2
            // 
            this.guna2ShadowPanel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2ShadowPanel2.Controls.Add(this.cmbCreditAccountName);
            this.guna2ShadowPanel2.Controls.Add(this.txtCreditAccountCode);
            this.guna2ShadowPanel2.Controls.Add(this.cmbCreditCostCenter);
            this.guna2ShadowPanel2.Controls.Add(this.label6);
            this.guna2ShadowPanel2.Controls.Add(this.label10);
            this.guna2ShadowPanel2.Controls.Add(this.label7);
            this.guna2ShadowPanel2.Controls.Add(this.pnlBank);
            this.guna2ShadowPanel2.Controls.Add(this.pnlTrans);
            this.guna2ShadowPanel2.FillColor = System.Drawing.Color.White;
            this.guna2ShadowPanel2.Location = new System.Drawing.Point(5, 22);
            this.guna2ShadowPanel2.Name = "guna2ShadowPanel2";
            this.guna2ShadowPanel2.ShadowColor = System.Drawing.Color.Black;
            this.guna2ShadowPanel2.Size = new System.Drawing.Size(888, 137);
            this.guna2ShadowPanel2.TabIndex = 35;
            // 
            // cmbCreditCostCenter
            // 
            this.cmbCreditCostCenter.BackColor = System.Drawing.Color.Transparent;
            this.cmbCreditCostCenter.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbCreditCostCenter.BorderRadius = 3;
            this.cmbCreditCostCenter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCreditCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCreditCostCenter.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbCreditCostCenter.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCreditCostCenter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCreditCostCenter.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCreditCostCenter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCreditCostCenter.ItemHeight = 18;
            this.cmbCreditCostCenter.Location = new System.Drawing.Point(371, 20);
            this.cmbCreditCostCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCreditCostCenter.Name = "cmbCreditCostCenter";
            this.cmbCreditCostCenter.Size = new System.Drawing.Size(116, 24);
            this.cmbCreditCostCenter.TabIndex = 22;
            this.cmbCreditCostCenter.SelectedIndexChanged += new System.EventHandler(this.cmbCreditCostCenter_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label10.Location = new System.Drawing.Point(5, -3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 28);
            this.label10.TabIndex = 14;
            this.label10.Text = "Account Code";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label7.Location = new System.Drawing.Point(369, -2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 28);
            this.label7.TabIndex = 23;
            this.label7.Text = "Cost Center";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBank
            // 
            this.pnlBank.BackColor = System.Drawing.Color.Transparent;
            this.pnlBank.Controls.Add(this.cmbCheckNo);
            this.pnlBank.Controls.Add(this.cmbBookNo);
            this.pnlBank.Controls.Add(this.cmbBankAccountName);
            this.pnlBank.Controls.Add(this.dt_check_date);
            this.pnlBank.Controls.Add(this.txtBankCode);
            this.pnlBank.Controls.Add(this.cmbBankName);
            this.pnlBank.Controls.Add(this.lblBankName);
            this.pnlBank.Controls.Add(this.txtCheckName);
            this.pnlBank.Controls.Add(this.lblBookNo);
            this.pnlBank.Controls.Add(this.lblBankAccountName);
            this.pnlBank.Controls.Add(this.label12);
            this.pnlBank.Controls.Add(this.label5);
            this.pnlBank.Controls.Add(this.label8);
            this.pnlBank.Location = new System.Drawing.Point(488, -6);
            this.pnlBank.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlBank.Name = "pnlBank";
            this.pnlBank.Size = new System.Drawing.Size(386, 145);
            this.pnlBank.TabIndex = 27;
            // 
            // cmbCheckNo
            // 
            this.cmbCheckNo.BackColor = System.Drawing.Color.Transparent;
            this.cmbCheckNo.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbCheckNo.BorderRadius = 3;
            this.cmbCheckNo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCheckNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCheckNo.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbCheckNo.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCheckNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCheckNo.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cmbCheckNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCheckNo.ItemHeight = 18;
            this.cmbCheckNo.Location = new System.Drawing.Point(181, 66);
            this.cmbCheckNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCheckNo.Name = "cmbCheckNo";
            this.cmbCheckNo.Size = new System.Drawing.Size(87, 24);
            this.cmbCheckNo.TabIndex = 33;
            // 
            // cmbBookNo
            // 
            this.cmbBookNo.BackColor = System.Drawing.Color.Transparent;
            this.cmbBookNo.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbBookNo.BorderRadius = 3;
            this.cmbBookNo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBookNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBookNo.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbBookNo.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBookNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBookNo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBookNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbBookNo.ItemHeight = 18;
            this.cmbBookNo.Location = new System.Drawing.Point(270, 105);
            this.cmbBookNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbBookNo.Name = "cmbBookNo";
            this.cmbBookNo.Size = new System.Drawing.Size(109, 24);
            this.cmbBookNo.TabIndex = 29;
            this.cmbBookNo.SelectedIndexChanged += new System.EventHandler(this.cmbBookNo_SelectedIndexChanged);
            // 
            // cmbBankAccountName
            // 
            this.cmbBankAccountName.BackColor = System.Drawing.Color.Transparent;
            this.cmbBankAccountName.BorderColor = System.Drawing.SystemColors.ActiveBorder;
            this.cmbBankAccountName.BorderRadius = 3;
            this.cmbBankAccountName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbBankAccountName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBankAccountName.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbBankAccountName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBankAccountName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbBankAccountName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbBankAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbBankAccountName.ItemHeight = 18;
            this.cmbBankAccountName.Location = new System.Drawing.Point(3, 105);
            this.cmbBankAccountName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbBankAccountName.Name = "cmbBankAccountName";
            this.cmbBankAccountName.Size = new System.Drawing.Size(265, 24);
            this.cmbBankAccountName.TabIndex = 30;
            this.cmbBankAccountName.SelectedIndexChanged += new System.EventHandler(this.cmbBankAccountName_SelectedIndexChanged);
            // 
            // dt_check_date
            // 
            this.dt_check_date.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dt_check_date.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dt_check_date.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dt_check_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dt_check_date.Location = new System.Drawing.Point(270, 64);
            this.dt_check_date.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dt_check_date.Name = "dt_check_date";
            this.dt_check_date.Size = new System.Drawing.Size(109, 22);
            this.dt_check_date.TabIndex = 28;
            this.dt_check_date.ValueChanged += new System.EventHandler(this.dt_check_date_ValueChanged);
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
            this.cmbBankName.Location = new System.Drawing.Point(120, 26);
            this.cmbBankName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbBankName.Name = "cmbBankName";
            this.cmbBankName.Size = new System.Drawing.Size(176, 24);
            this.cmbBankName.TabIndex = 16;
            this.cmbBankName.SelectedIndexChanged += new System.EventHandler(this.cmbBankName_SelectedIndexChanged);
            // 
            // lblBankName
            // 
            this.lblBankName.BackColor = System.Drawing.Color.Transparent;
            this.lblBankName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBankName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblBankName.Location = new System.Drawing.Point(120, 4);
            this.lblBankName.Name = "lblBankName";
            this.lblBankName.Size = new System.Drawing.Size(96, 26);
            this.lblBankName.TabIndex = 18;
            this.lblBankName.Text = "Bank Name";
            this.lblBankName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.txtCheckName.Location = new System.Drawing.Point(4, 66);
            this.txtCheckName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCheckName.Name = "txtCheckName";
            this.txtCheckName.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCheckName.PlaceholderText = "Cheque Name";
            this.txtCheckName.SelectedText = "";
            this.txtCheckName.Size = new System.Drawing.Size(176, 25);
            this.txtCheckName.TabIndex = 20;
            // 
            // lblBookNo
            // 
            this.lblBookNo.BackColor = System.Drawing.Color.Transparent;
            this.lblBookNo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBookNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblBookNo.Location = new System.Drawing.Point(272, 86);
            this.lblBookNo.Name = "lblBookNo";
            this.lblBookNo.Size = new System.Drawing.Size(60, 20);
            this.lblBookNo.TabIndex = 31;
            this.lblBookNo.Text = "Book NO";
            this.lblBookNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBankAccountName
            // 
            this.lblBankAccountName.BackColor = System.Drawing.Color.Transparent;
            this.lblBankAccountName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBankAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblBankAccountName.Location = new System.Drawing.Point(3, 82);
            this.lblBankAccountName.Name = "lblBankAccountName";
            this.lblBankAccountName.Size = new System.Drawing.Size(127, 28);
            this.lblBankAccountName.TabIndex = 32;
            this.lblBankAccountName.Text = "Bank Account Name";
            this.lblBankAccountName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(270, 44);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(74, 29);
            this.label12.TabIndex = 27;
            this.label12.Text = "Cheque Date";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(4, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 20);
            this.label5.TabIndex = 18;
            this.label5.Text = "Cheque Name";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label8.Location = new System.Drawing.Point(181, 48);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = "Cheque No";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTrans
            // 
            this.pnlTrans.BackColor = System.Drawing.Color.Transparent;
            this.pnlTrans.Controls.Add(this.dtpTransDate);
            this.pnlTrans.Controls.Add(this.txtTransRef);
            this.pnlTrans.Controls.Add(this.txtTransName);
            this.pnlTrans.Controls.Add(this.lblTransDate);
            this.pnlTrans.Controls.Add(this.lblTransName);
            this.pnlTrans.Controls.Add(this.lblTransRef);
            this.pnlTrans.Location = new System.Drawing.Point(482, -4);
            this.pnlTrans.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlTrans.Name = "pnlTrans";
            this.pnlTrans.Size = new System.Drawing.Size(193, 165);
            this.pnlTrans.TabIndex = 26;
            this.pnlTrans.Visible = false;
            // 
            // lblTransName
            // 
            this.lblTransName.BackColor = System.Drawing.Color.Transparent;
            this.lblTransName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblTransName.Location = new System.Drawing.Point(9, 41);
            this.lblTransName.Name = "lblTransName";
            this.lblTransName.Size = new System.Drawing.Size(95, 28);
            this.lblTransName.TabIndex = 25;
            this.lblTransName.Text = "TRNS / Name";
            this.lblTransName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTransRef
            // 
            this.lblTransRef.BackColor = System.Drawing.Color.Transparent;
            this.lblTransRef.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTransRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.lblTransRef.Location = new System.Drawing.Point(9, 82);
            this.lblTransRef.Name = "lblTransRef";
            this.lblTransRef.Size = new System.Drawing.Size(95, 28);
            this.lblTransRef.TabIndex = 25;
            this.lblTransRef.Text = "TRNS / REF";
            this.lblTransRef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.humId,
            this.invId,
            this.invDate,
            this.InvNo,
            this.Total,
            this.chkPay,
            this.Pay,
            this.Description,
            this.voucherType});
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
            this.dgvInv.Location = new System.Drawing.Point(2, 542);
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
            this.dgvInv.Size = new System.Drawing.Size(899, 63);
            this.dgvInv.TabIndex = 8;
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvInv.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvInv.ThemeStyle.BackColor = System.Drawing.Color.White;
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
            this.dgvInv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellContentClick);
            this.dgvInv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInv_CellValueChanged);
            this.dgvInv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvInv_EditingControlShowing);
            // 
            // no
            // 
            this.no.HeaderText = "no";
            this.no.MinimumWidth = 6;
            this.no.Name = "no";
            this.no.ReadOnly = true;
            this.no.Width = 125;
            // 
            // humId
            // 
            this.humId.HeaderText = "humId";
            this.humId.MinimumWidth = 6;
            this.humId.Name = "humId";
            this.humId.ReadOnly = true;
            this.humId.Visible = false;
            this.humId.Width = 125;
            // 
            // invId
            // 
            this.invId.HeaderText = "invId";
            this.invId.MinimumWidth = 6;
            this.invId.Name = "invId";
            this.invId.ReadOnly = true;
            this.invId.Visible = false;
            this.invId.Width = 125;
            // 
            // invDate
            // 
            this.invDate.HeaderText = "Date";
            this.invDate.MinimumWidth = 6;
            this.invDate.Name = "invDate";
            this.invDate.ReadOnly = true;
            this.invDate.Width = 125;
            // 
            // InvNo
            // 
            this.InvNo.HeaderText = "Inv No";
            this.InvNo.MinimumWidth = 6;
            this.InvNo.Name = "InvNo";
            this.InvNo.ReadOnly = true;
            this.InvNo.Width = 125;
            // 
            // Total
            // 
            this.Total.HeaderText = "Amount";
            this.Total.MinimumWidth = 6;
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 125;
            // 
            // chkPay
            // 
            this.chkPay.HeaderText = "Pay";
            this.chkPay.MinimumWidth = 6;
            this.chkPay.Name = "chkPay";
            this.chkPay.Width = 125;
            // 
            // Pay
            // 
            this.Pay.HeaderText = "Payment";
            this.Pay.MinimumWidth = 6;
            this.Pay.Name = "Pay";
            this.Pay.Width = 125;
            // 
            // Description
            // 
            this.Description.HeaderText = "Description";
            this.Description.MinimumWidth = 6;
            this.Description.Name = "Description";
            this.Description.Width = 125;
            // 
            // voucherType
            // 
            this.voucherType.HeaderText = "";
            this.voucherType.MinimumWidth = 6;
            this.voucherType.Name = "voucherType";
            this.voucherType.Width = 125;
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
            this.headerUC1.Size = new System.Drawing.Size(899, 37);
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
            this.guna2Button2.Location = new System.Drawing.Point(839, 4);
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
            this.guna2Button1.Location = new System.Drawing.Point(861, 4);
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
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(304, 3);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(314, 20);
            this.guna2HtmlLabel2.TabIndex = 19;
            this.guna2HtmlLabel2.Text = "Payment Voucher";
            this.guna2HtmlLabel2.TextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(880, 4);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(15, 15);
            this.guna2ControlBox1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(901, 0);
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
            this.panel3.Size = new System.Drawing.Size(899, 2);
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
            this.panel9.Size = new System.Drawing.Size(899, 84);
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
            this.tabControl2.Size = new System.Drawing.Size(899, 84);
            this.tabControl2.TabIndex = 39;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.txtPVCode);
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
            this.tabPage3.Controls.Add(this.txtId);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(891, 58);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Main";
            // 
            // txtPVCode
            // 
            this.txtPVCode.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtPVCode.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPVCode.Enabled = false;
            this.txtPVCode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPVCode.Location = new System.Drawing.Point(33, 18);
            this.txtPVCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPVCode.Multiline = true;
            this.txtPVCode.Name = "txtPVCode";
            this.txtPVCode.Size = new System.Drawing.Size(66, 24);
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
            this.BtnClear.Location = new System.Drawing.Point(139, 6);
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
            this.BtnSaveNew.Location = new System.Drawing.Point(191, 6);
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
            this.BtnDelete.Location = new System.Drawing.Point(247, 6);
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
            this.BtnAttach.Image = global::YamyProject.Properties.Resources.attachN;
            this.BtnAttach.Location = new System.Drawing.Point(613, 7);
            this.BtnAttach.Name = "BtnAttach";
            this.BtnAttach.Size = new System.Drawing.Size(54, 47);
            this.BtnAttach.TabIndex = 30;
            this.BtnAttach.Text = "Attach File";
            this.BtnAttach.Visible = false;
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
            this.BtnEmail.Location = new System.Drawing.Point(463, 8);
            this.BtnEmail.Name = "BtnEmail";
            this.BtnEmail.Size = new System.Drawing.Size(54, 47);
            this.BtnEmail.TabIndex = 26;
            this.BtnEmail.Text = "Email";
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtId.Enabled = false;
            this.txtId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtId.Location = new System.Drawing.Point(33, 18);
            this.txtId.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtId.Multiline = true;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(66, 24);
            this.txtId.TabIndex = 33;
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
            this.tabPage4.Size = new System.Drawing.Size(891, 58);
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
            // frmViewPaymentVoucher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(903, 640);
            this.Controls.Add(this.dgvInv);
            this.Controls.Add(this.guna2GroupBox3);
            this.Controls.Add(this.guna2GroupBox2);
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
            this.Name = "frmViewPaymentVoucher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Payment Voucher";
            this.Load += new System.EventHandler(this.frmViewPaymentVoucher_Load);
            this.panel1.ResumeLayout(false);
            this.txtcostcenter.ResumeLayout(false);
            this.txtcostcenter.PerformLayout();
            this.guna2GroupBox2.ResumeLayout(false);
            this.guna2ShadowPanel1.ResumeLayout(false);
            this.pnlCustomer.ResumeLayout(false);
            this.guna2GroupBox3.ResumeLayout(false);
            this.guna2ShadowPanel2.ResumeLayout(false);
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
        private Guna.UI2.WinForms.Guna2GroupBox txtcostcenter;
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
        private Guna.UI2.WinForms.Guna2TextBox txtCreditAccountCode;
        private System.Windows.Forms.ComboBox cmbCreditAccountName;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox txtVendor;
        private Guna.UI2.WinForms.Guna2TextBox txtDebitCode;
        private System.Windows.Forms.ComboBox cmbVendor;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblCustomer;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox3;
        private System.Windows.Forms.DateTimePicker dtOpen;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtAmount;
        private Guna.UI2.WinForms.Guna2TextBox txtAmountInWord;
        private System.Windows.Forms.Label lblCostcenter;
        private Guna.UI2.WinForms.Guna2ComboBox cmbDebitCostCenter;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblCode;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCreditCostCenter;
        private Guna.UI2.WinForms.Guna2ComboBox cmbBankName;
        private System.Windows.Forms.Label lblTransRef;
        private System.Windows.Forms.Label lblBankName;
        private System.Windows.Forms.Label lblTransName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlBank;
        private System.Windows.Forms.Panel pnlTrans;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDebitAccountName;
        private System.Windows.Forms.CheckedListBox lstOfEmp;
        private System.Windows.Forms.Panel pnlCustomer;
        private Guna.UI2.WinForms.Guna2DataGridView dgvInv;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2TextBox txtCheckName;
        private System.Windows.Forms.DateTimePicker dt_check_date;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2ComboBox cmbBookNo;
        private Guna.UI2.WinForms.Guna2ComboBox cmbBankAccountName;
        private System.Windows.Forms.Label lblBookNo;
        private System.Windows.Forms.Label lblBankAccountName;
        private System.Windows.Forms.RichTextBox txtDescription1;
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
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel1;
        private Guna.UI2.WinForms.Guna2ShadowPanel guna2ShadowPanel2;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCheckNo;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private System.Windows.Forms.ListBox lstAccountSuggestions;
        private DataGridViewTextBoxColumn no;
        private DataGridViewTextBoxColumn humId;
        private DataGridViewTextBoxColumn invId;
        private DataGridViewTextBoxColumn invDate;
        private DataGridViewTextBoxColumn InvNo;
        private DataGridViewTextBoxColumn Total;
        private DataGridViewCheckBoxColumn chkPay;
        private DataGridViewTextBoxColumn Pay;
        private DataGridViewTextBoxColumn Description;
        private DataGridViewTextBoxColumn voucherType;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private Guna.UI2.WinForms.Guna2CheckBox cbpettycash;
        private Guna.UI2.WinForms.Guna2CheckBox CbSubcontractors;
        private TextBox txtId;
    }
}