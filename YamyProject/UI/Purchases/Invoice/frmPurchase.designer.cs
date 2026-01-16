using System.Windows.Forms;

namespace YamyProject
{
    partial class frmPurchase
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle53 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle54 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtBarcodeScan = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnSaveClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnClose = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.guna2GroupBox4 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.dgvItems = new Guna.UI2.WinForms.Guna2DataGridView();
            this.itemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.net_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vat = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.VatP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.method = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cost_center = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.cmbFixedAssetCategory = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtPaymentTerms = new System.Windows.Forms.DateTimePicker();
            this.cmbPurchasetype = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbPaymentTerms = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbAccountCashName = new System.Windows.Forms.ComboBox();
            this.cmbPaymentMethod = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtNextCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPONO = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtSalesMan = new Guna.UI2.WinForms.Guna2TextBox();
            this.dtInv = new System.Windows.Forms.DateTimePicker();
            this.cmbWarehouse = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbCity = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelFixedAssetCategory = new System.Windows.Forms.Label();
            this.cmbShipVia = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtShip = new System.Windows.Forms.DateTimePicker();
            this.txtShipTo = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtBillTo = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2GroupBox3 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.richTextDescription = new System.Windows.Forms.RichTextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtTotalVat = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTotalBefore = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTotal = new Guna.UI2.WinForms.Guna2TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.CbSubcontractors = new Guna.UI2.WinForms.Guna2CheckBox();
            this.guna2VSeparator7 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.txtInvoiceId = new System.Windows.Forms.TextBox();
            this.guna2TileButton12 = new Guna.UI2.WinForms.Guna2TileButton();
            this.btnNext = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TileButton13 = new Guna.UI2.WinForms.Guna2TileButton();
            this.btnPrevious = new Guna.UI2.WinForms.Guna2Button();
            this.guna2VSeparator8 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2VSeparator12 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton14 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton23 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton15 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton22 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator9 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton21 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton16 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton20 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator10 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton19 = new Guna.UI2.WinForms.Guna2TileButton();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.guna2VSeparator11 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.chkPrint = new System.Windows.Forms.CheckBox();
            this.guna2TileButton18 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton17 = new Guna.UI2.WinForms.Guna2TileButton();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.guna2TileButton27 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton28 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton29 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2VSeparator13 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton26 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton25 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton24 = new Guna.UI2.WinForms.Guna2TileButton();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.cmbVendor = new System.Windows.Forms.ComboBox();
            this.lnkNewVendor = new System.Windows.Forms.LinkLabel();
            this.txtVendorCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2TileButton31 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton30 = new Guna.UI2.WinForms.Guna2TileButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
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
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.guna2GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.panel9.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel7.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.txtBarcodeScan);
            this.panel1.Controls.Add(this.btnSaveClose);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 703);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1244, 44);
            this.panel1.TabIndex = 2;
            // 
            // txtBarcodeScan
            // 
            this.txtBarcodeScan.BorderRadius = 5;
            this.txtBarcodeScan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBarcodeScan.DefaultText = "";
            this.txtBarcodeScan.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBarcodeScan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBarcodeScan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBarcodeScan.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBarcodeScan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBarcodeScan.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcodeScan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBarcodeScan.Location = new System.Drawing.Point(12, 10);
            this.txtBarcodeScan.Name = "txtBarcodeScan";
            this.txtBarcodeScan.PlaceholderText = "Scan Barcode or Enter Code";
            this.txtBarcodeScan.SelectedText = "";
            this.txtBarcodeScan.Size = new System.Drawing.Size(167, 24);
            this.txtBarcodeScan.TabIndex = 59;
            this.txtBarcodeScan.Visible = false;
            this.txtBarcodeScan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBarcodeScan_KeyDown);
            // 
            // btnSaveClose
            // 
            this.btnSaveClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSaveClose.BorderRadius = 5;
            this.btnSaveClose.BorderThickness = 3;
            this.btnSaveClose.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSaveClose.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveClose.ForeColor = System.Drawing.Color.White;
            this.btnSaveClose.Location = new System.Drawing.Point(932, 7);
            this.btnSaveClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveClose.Name = "btnSaveClose";
            this.btnSaveClose.Size = new System.Drawing.Size(129, 30);
            this.btnSaveClose.TabIndex = 4;
            this.btnSaveClose.Text = "Save && Close";
            this.btnSaveClose.Click += new System.EventHandler(this.btnSaveNew_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.BorderRadius = 5;
            this.btnClose.BorderThickness = 1;
            this.btnClose.FillColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnClose.Location = new System.Drawing.Point(1162, 7);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(79, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.BorderRadius = 5;
            this.btnSave.BorderThickness = 3;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(1070, 7);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // guna2GroupBox4
            // 
            this.guna2GroupBox4.BorderThickness = 0;
            this.guna2GroupBox4.Controls.Add(this.dgvItems);
            this.guna2GroupBox4.CustomBorderColor = System.Drawing.Color.White;
            this.guna2GroupBox4.CustomBorderThickness = new System.Windows.Forms.Padding(0);
            this.guna2GroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guna2GroupBox4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox4.Location = new System.Drawing.Point(3, 311);
            this.guna2GroupBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox4.Name = "guna2GroupBox4";
            this.guna2GroupBox4.ShadowDecoration.BorderRadius = 0;
            this.guna2GroupBox4.ShadowDecoration.Depth = 0;
            this.guna2GroupBox4.Size = new System.Drawing.Size(1244, 298);
            this.guna2GroupBox4.TabIndex = 10;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeColumns = false;
            this.dgvItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle28.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle28.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle28.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle28.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle28;
            this.dgvItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle29.BackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle29.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle29.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle29.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle29;
            this.dgvItems.ColumnHeadersHeight = 18;
            this.dgvItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemId,
            this.no,
            this.code,
            this.name,
            this.qty,
            this.cost_price,
            this.price,
            this.discount,
            this.net_price,
            this.vat,
            this.VatP,
            this.total,
            this.method,
            this.type,
            this.cost_center,
            this.delete});
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle42.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle42.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle42.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle42.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle42.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle42.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.DefaultCellStyle = dataGridViewCellStyle42;
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvItems.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.Location = new System.Drawing.Point(0, 0);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvItems.MultiSelect = false;
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle43.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle43.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle43.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle43.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle43.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle43.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.RowHeadersDefaultCellStyle = dataGridViewCellStyle43;
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowHeadersWidth = 51;
            dataGridViewCellStyle44.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.dgvItems.RowsDefaultCellStyle = dataGridViewCellStyle44;
            this.dgvItems.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItems.RowTemplate.Height = 25;
            this.dgvItems.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.Size = new System.Drawing.Size(1244, 298);
            this.dgvItems.TabIndex = 7;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.Silver;
            this.dgvItems.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvItems.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.dgvItems.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvItems.ThemeStyle.HeaderStyle.Height = 18;
            this.dgvItems.ThemeStyle.ReadOnly = false;
            this.dgvItems.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvItems.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 7.8F);
            this.dgvItems.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.RowsStyle.Height = 25;
            this.dgvItems.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
            this.dgvItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellEndEdit);
            this.dgvItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellValueChanged);
            this.dgvItems.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvItems_EditingControlShowing);
            this.dgvItems.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvItems_RowPostPaint);
            // 
            // itemId
            // 
            this.itemId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemId.HeaderText = "id";
            this.itemId.MinimumWidth = 100;
            this.itemId.Name = "itemId";
            this.itemId.Visible = false;
            this.itemId.Width = 125;
            // 
            // no
            // 
            this.no.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.no.FillWeight = 30F;
            this.no.HeaderText = "#Sn";
            this.no.MinimumWidth = 30;
            this.no.Name = "no";
            this.no.ReadOnly = true;
            this.no.Width = 30;
            // 
            // code
            // 
            this.code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle30.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code.DefaultCellStyle = dataGridViewCellStyle30;
            this.code.HeaderText = "Item Code";
            this.code.MinimumWidth = 100;
            this.code.Name = "code";
            this.code.Width = 125;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Item Name";
            this.name.MinimumWidth = 100;
            this.name.Name = "name";
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.qty.DefaultCellStyle = dataGridViewCellStyle31;
            this.qty.FillWeight = 75F;
            this.qty.HeaderText = "Qty";
            this.qty.MinimumWidth = 55;
            this.qty.Name = "qty";
            this.qty.Width = 55;
            // 
            // cost_price
            // 
            this.cost_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle32.Format = "N2";
            dataGridViewCellStyle32.NullValue = "0";
            this.cost_price.DefaultCellStyle = dataGridViewCellStyle32;
            this.cost_price.HeaderText = "Cost Price";
            this.cost_price.MinimumWidth = 100;
            this.cost_price.Name = "cost_price";
            this.cost_price.Width = 125;
            // 
            // price
            // 
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle33.Format = "N2";
            dataGridViewCellStyle33.NullValue = "0";
            this.price.DefaultCellStyle = dataGridViewCellStyle33;
            this.price.HeaderText = "Price";
            this.price.MinimumWidth = 6;
            this.price.Name = "price";
            this.price.Visible = false;
            // 
            // discount
            // 
            this.discount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle34.Format = "N2";
            dataGridViewCellStyle34.NullValue = "0";
            this.discount.DefaultCellStyle = dataGridViewCellStyle34;
            this.discount.HeaderText = "Disc";
            this.discount.MinimumWidth = 100;
            this.discount.Name = "discount";
            this.discount.Width = 125;
            // 
            // net_price
            // 
            this.net_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle35.Format = "N2";
            dataGridViewCellStyle35.NullValue = "0";
            this.net_price.DefaultCellStyle = dataGridViewCellStyle35;
            this.net_price.HeaderText = "Net Price";
            this.net_price.MinimumWidth = 110;
            this.net_price.Name = "net_price";
            this.net_price.ReadOnly = true;
            this.net_price.Width = 110;
            // 
            // vat
            // 
            this.vat.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.vat.DefaultCellStyle = dataGridViewCellStyle36;
            this.vat.FillWeight = 75F;
            this.vat.HeaderText = "Vat";
            this.vat.MinimumWidth = 75;
            this.vat.Name = "vat";
            this.vat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.vat.Width = 75;
            // 
            // VatP
            // 
            this.VatP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle37.Format = "N2";
            dataGridViewCellStyle37.NullValue = "0";
            this.VatP.DefaultCellStyle = dataGridViewCellStyle37;
            this.VatP.HeaderText = "~";
            this.VatP.MinimumWidth = 100;
            this.VatP.Name = "VatP";
            this.VatP.Width = 125;
            // 
            // total
            // 
            this.total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle38.Format = "N2";
            dataGridViewCellStyle38.NullValue = "0";
            this.total.DefaultCellStyle = dataGridViewCellStyle38;
            this.total.HeaderText = "Amount";
            this.total.MinimumWidth = 110;
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.Width = 110;
            // 
            // method
            // 
            this.method.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.method.DefaultCellStyle = dataGridViewCellStyle39;
            this.method.HeaderText = "method";
            this.method.MinimumWidth = 100;
            this.method.Name = "method";
            this.method.Visible = false;
            this.method.Width = 125;
            // 
            // type
            // 
            this.type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.type.DefaultCellStyle = dataGridViewCellStyle40;
            this.type.HeaderText = "type";
            this.type.MinimumWidth = 100;
            this.type.Name = "type";
            this.type.Visible = false;
            this.type.Width = 125;
            // 
            // cost_center
            // 
            this.cost_center.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.cost_center.DefaultCellStyle = dataGridViewCellStyle41;
            this.cost_center.HeaderText = "Cost Center";
            this.cost_center.MinimumWidth = 100;
            this.cost_center.Name = "cost_center";
            this.cost_center.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cost_center.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cost_center.Width = 125;
            // 
            // delete
            // 
            this.delete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.delete.HeaderText = "DEL";
            this.delete.MinimumWidth = 50;
            this.delete.Name = "delete";
            this.delete.Text = "x";
            this.delete.Width = 50;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.guna2GroupBox1.BorderThickness = 0;
            this.guna2GroupBox1.Controls.Add(this.cmbFixedAssetCategory);
            this.guna2GroupBox1.Controls.Add(this.dtPaymentTerms);
            this.guna2GroupBox1.Controls.Add(this.cmbPurchasetype);
            this.guna2GroupBox1.Controls.Add(this.cmbPaymentTerms);
            this.guna2GroupBox1.Controls.Add(this.cmbAccountCashName);
            this.guna2GroupBox1.Controls.Add(this.cmbPaymentMethod);
            this.guna2GroupBox1.Controls.Add(this.label5);
            this.guna2GroupBox1.Controls.Add(this.label15);
            this.guna2GroupBox1.Controls.Add(this.txtNextCode);
            this.guna2GroupBox1.Controls.Add(this.label4);
            this.guna2GroupBox1.Controls.Add(this.txtPONO);
            this.guna2GroupBox1.Controls.Add(this.txtSalesMan);
            this.guna2GroupBox1.Controls.Add(this.dtInv);
            this.guna2GroupBox1.Controls.Add(this.cmbWarehouse);
            this.guna2GroupBox1.Controls.Add(this.label21);
            this.guna2GroupBox1.Controls.Add(this.label22);
            this.guna2GroupBox1.Controls.Add(this.label2);
            this.guna2GroupBox1.Controls.Add(this.label3);
            this.guna2GroupBox1.Controls.Add(this.cmbCity);
            this.guna2GroupBox1.Controls.Add(this.label9);
            this.guna2GroupBox1.Controls.Add(this.label20);
            this.guna2GroupBox1.Controls.Add(this.label13);
            this.guna2GroupBox1.Controls.Add(this.label8);
            this.guna2GroupBox1.Controls.Add(this.labelFixedAssetCategory);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(3, 192);
            this.guna2GroupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(1244, 119);
            this.guna2GroupBox1.TabIndex = 7;
            this.guna2GroupBox1.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // cmbFixedAssetCategory
            // 
            this.cmbFixedAssetCategory.BackColor = System.Drawing.Color.Transparent;
            this.cmbFixedAssetCategory.BorderRadius = 5;
            this.cmbFixedAssetCategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFixedAssetCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFixedAssetCategory.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbFixedAssetCategory.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbFixedAssetCategory.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbFixedAssetCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbFixedAssetCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbFixedAssetCategory.ItemHeight = 18;
            this.cmbFixedAssetCategory.Location = new System.Drawing.Point(775, 79);
            this.cmbFixedAssetCategory.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbFixedAssetCategory.Name = "cmbFixedAssetCategory";
            this.cmbFixedAssetCategory.Size = new System.Drawing.Size(189, 24);
            this.cmbFixedAssetCategory.TabIndex = 59;
            this.cmbFixedAssetCategory.Visible = false;
            // 
            // dtPaymentTerms
            // 
            this.dtPaymentTerms.BackColor = System.Drawing.Color.Gainsboro;
            this.dtPaymentTerms.Enabled = false;
            this.dtPaymentTerms.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPaymentTerms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtPaymentTerms.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPaymentTerms.Location = new System.Drawing.Point(563, 27);
            this.dtPaymentTerms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtPaymentTerms.Name = "dtPaymentTerms";
            this.dtPaymentTerms.Size = new System.Drawing.Size(205, 23);
            this.dtPaymentTerms.TabIndex = 47;
            // 
            // cmbPurchasetype
            // 
            this.cmbPurchasetype.BackColor = System.Drawing.Color.Transparent;
            this.cmbPurchasetype.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cmbPurchasetype.BorderRadius = 5;
            this.cmbPurchasetype.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPurchasetype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurchasetype.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbPurchasetype.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPurchasetype.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPurchasetype.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.cmbPurchasetype.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPurchasetype.ItemHeight = 18;
            this.cmbPurchasetype.Items.AddRange(new object[] {
            "Inventory ",
            "Fixed Assets",
            "Expense"});
            this.cmbPurchasetype.Location = new System.Drawing.Point(642, 79);
            this.cmbPurchasetype.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPurchasetype.Name = "cmbPurchasetype";
            this.cmbPurchasetype.Size = new System.Drawing.Size(126, 24);
            this.cmbPurchasetype.TabIndex = 57;
            this.cmbPurchasetype.SelectedIndexChanged += new System.EventHandler(this.cmbPurchasetype_SelectedIndexChanged);
            // 
            // cmbPaymentTerms
            // 
            this.cmbPaymentTerms.BackColor = System.Drawing.Color.Transparent;
            this.cmbPaymentTerms.BorderRadius = 5;
            this.cmbPaymentTerms.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPaymentTerms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentTerms.Enabled = false;
            this.cmbPaymentTerms.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbPaymentTerms.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentTerms.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentTerms.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPaymentTerms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPaymentTerms.ItemHeight = 18;
            this.cmbPaymentTerms.Items.AddRange(new object[] {
            "30",
            "60",
            "90",
            "120",
            "150",
            "180",
            "210",
            "240",
            "270",
            "300",
            "330",
            "360"});
            this.cmbPaymentTerms.Location = new System.Drawing.Point(405, 26);
            this.cmbPaymentTerms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPaymentTerms.Name = "cmbPaymentTerms";
            this.cmbPaymentTerms.Size = new System.Drawing.Size(152, 24);
            this.cmbPaymentTerms.TabIndex = 41;
            this.cmbPaymentTerms.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentTerms_SelectedIndexChanged_1);
            // 
            // cmbAccountCashName
            // 
            this.cmbAccountCashName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAccountCashName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAccountCashName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccountCashName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAccountCashName.ItemHeight = 15;
            this.cmbAccountCashName.Location = new System.Drawing.Point(405, 79);
            this.cmbAccountCashName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAccountCashName.Name = "cmbAccountCashName";
            this.cmbAccountCashName.Size = new System.Drawing.Size(230, 23);
            this.cmbAccountCashName.TabIndex = 42;
            this.cmbAccountCashName.SelectedIndexChanged += new System.EventHandler(this.cmbAccountCashName_SelectedIndexChanged);
            // 
            // cmbPaymentMethod
            // 
            this.cmbPaymentMethod.BackColor = System.Drawing.Color.Transparent;
            this.cmbPaymentMethod.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cmbPaymentMethod.BorderRadius = 5;
            this.cmbPaymentMethod.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMethod.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbPaymentMethod.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentMethod.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbPaymentMethod.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.cmbPaymentMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbPaymentMethod.ItemHeight = 18;
            this.cmbPaymentMethod.Items.AddRange(new object[] {
            "Cash",
            "Credit"});
            this.cmbPaymentMethod.Location = new System.Drawing.Point(10, 24);
            this.cmbPaymentMethod.Margin = new System.Windows.Forms.Padding(4);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(147, 24);
            this.cmbPaymentMethod.TabIndex = 43;
            this.cmbPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentMethod_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(405, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 15);
            this.label5.TabIndex = 44;
            this.label5.Text = "Payment Terms";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label15.Location = new System.Drawing.Point(10, 1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(122, 19);
            this.label15.TabIndex = 46;
            this.label15.Text = "Invoice Type";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNextCode
            // 
            this.txtNextCode.BackColor = System.Drawing.Color.Transparent;
            this.txtNextCode.BorderRadius = 5;
            this.txtNextCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNextCode.DefaultText = "";
            this.txtNextCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtNextCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNextCode.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNextCode.ForeColor = System.Drawing.Color.Black;
            this.txtNextCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNextCode.Location = new System.Drawing.Point(1128, 23);
            this.txtNextCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNextCode.Name = "txtNextCode";
            this.txtNextCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtNextCode.PlaceholderText = "";
            this.txtNextCode.SelectedText = "";
            this.txtNextCode.Size = new System.Drawing.Size(110, 24);
            this.txtNextCode.TabIndex = 40;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label4.Location = new System.Drawing.Point(1127, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 18);
            this.label4.TabIndex = 39;
            this.label4.Text = "Invoice #";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPONO
            // 
            this.txtPONO.BackColor = System.Drawing.Color.Transparent;
            this.txtPONO.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtPONO.BorderRadius = 5;
            this.txtPONO.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPONO.DefaultText = "";
            this.txtPONO.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtPONO.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPONO.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPONO.ForeColor = System.Drawing.Color.Black;
            this.txtPONO.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtPONO.Location = new System.Drawing.Point(774, 25);
            this.txtPONO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPONO.Name = "txtPONO";
            this.txtPONO.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtPONO.PlaceholderText = "P.O No";
            this.txtPONO.SelectedText = "";
            this.txtPONO.Size = new System.Drawing.Size(75, 24);
            this.txtPONO.TabIndex = 35;
            // 
            // txtSalesMan
            // 
            this.txtSalesMan.BackColor = System.Drawing.Color.Transparent;
            this.txtSalesMan.BorderRadius = 5;
            this.txtSalesMan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSalesMan.DefaultText = "";
            this.txtSalesMan.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtSalesMan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSalesMan.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalesMan.ForeColor = System.Drawing.Color.Black;
            this.txtSalesMan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSalesMan.Location = new System.Drawing.Point(160, 82);
            this.txtSalesMan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSalesMan.Name = "txtSalesMan";
            this.txtSalesMan.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtSalesMan.PlaceholderText = "Sales Man";
            this.txtSalesMan.SelectedText = "";
            this.txtSalesMan.Size = new System.Drawing.Size(162, 24);
            this.txtSalesMan.TabIndex = 28;
            // 
            // dtInv
            // 
            this.dtInv.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtInv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtInv.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtInv.Location = new System.Drawing.Point(1129, 85);
            this.dtInv.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtInv.Name = "dtInv";
            this.dtInv.Size = new System.Drawing.Size(110, 22);
            this.dtInv.TabIndex = 29;
            this.dtInv.ValueChanged += new System.EventHandler(this.dtInv_ValueChanged);
            // 
            // cmbWarehouse
            // 
            this.cmbWarehouse.BackColor = System.Drawing.Color.Transparent;
            this.cmbWarehouse.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cmbWarehouse.BorderRadius = 5;
            this.cmbWarehouse.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbWarehouse.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWarehouse.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbWarehouse.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWarehouse.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbWarehouse.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbWarehouse.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbWarehouse.ItemHeight = 18;
            this.cmbWarehouse.Location = new System.Drawing.Point(160, 24);
            this.cmbWarehouse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(162, 24);
            this.cmbWarehouse.TabIndex = 30;
            // 
            // label21
            // 
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label21.Location = new System.Drawing.Point(1128, 61);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(59, 21);
            this.label21.TabIndex = 33;
            this.label21.Text = "Date";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label22.Location = new System.Drawing.Point(160, 63);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(162, 21);
            this.label22.TabIndex = 32;
            this.label22.Text = "Sales Man";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(776, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 19);
            this.label2.TabIndex = 36;
            this.label2.Text = "P.O NO";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(603, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(163, 17);
            this.label3.TabIndex = 48;
            this.label3.Text = "due to";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbCity
            // 
            this.cmbCity.BackColor = System.Drawing.Color.Transparent;
            this.cmbCity.BorderRadius = 5;
            this.cmbCity.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCity.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbCity.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCity.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbCity.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.cmbCity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCity.ItemHeight = 18;
            this.cmbCity.Items.AddRange(new object[] {
            "Abu Dhabi",
            "Dubai",
            "Sharjah",
            "Ajman",
            "Fujairah",
            "Ras Al Khaimah"});
            this.cmbCity.Location = new System.Drawing.Point(7, 81);
            this.cmbCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCity.Name = "cmbCity";
            this.cmbCity.Size = new System.Drawing.Size(150, 24);
            this.cmbCity.StartIndex = 0;
            this.cmbCity.TabIndex = 49;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label9.Location = new System.Drawing.Point(7, 63);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 15);
            this.label9.TabIndex = 50;
            this.label9.Text = "Emirates";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label20.Location = new System.Drawing.Point(160, 5);
            this.label20.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(162, 17);
            this.label20.TabIndex = 31;
            this.label20.Text = "Warehouse";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label13.Location = new System.Drawing.Point(405, 61);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(191, 17);
            this.label13.TabIndex = 45;
            this.label13.Text = "Account Name";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label8.Location = new System.Drawing.Point(639, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 15);
            this.label8.TabIndex = 58;
            this.label8.Text = "Purchase Type";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFixedAssetCategory
            // 
            this.labelFixedAssetCategory.BackColor = System.Drawing.Color.Transparent;
            this.labelFixedAssetCategory.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFixedAssetCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.labelFixedAssetCategory.Location = new System.Drawing.Point(776, 59);
            this.labelFixedAssetCategory.Name = "labelFixedAssetCategory";
            this.labelFixedAssetCategory.Size = new System.Drawing.Size(186, 20);
            this.labelFixedAssetCategory.TabIndex = 60;
            this.labelFixedAssetCategory.Text = "Fixed Asset Category";
            this.labelFixedAssetCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelFixedAssetCategory.Visible = false;
            // 
            // cmbShipVia
            // 
            this.cmbShipVia.BackColor = System.Drawing.Color.Transparent;
            this.cmbShipVia.BorderColor = System.Drawing.Color.Silver;
            this.cmbShipVia.BorderRadius = 5;
            this.cmbShipVia.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbShipVia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShipVia.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbShipVia.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbShipVia.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbShipVia.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbShipVia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbShipVia.ItemHeight = 18;
            this.cmbShipVia.Items.AddRange(new object[] {
            "DHL",
            "MAIL",
            "On Site",
            "Other"});
            this.cmbShipVia.Location = new System.Drawing.Point(1127, 10);
            this.cmbShipVia.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbShipVia.Name = "cmbShipVia";
            this.cmbShipVia.Size = new System.Drawing.Size(112, 24);
            this.cmbShipVia.TabIndex = 55;
            // 
            // dtShip
            // 
            this.dtShip.BackColor = System.Drawing.Color.Gainsboro;
            this.dtShip.Font = new System.Drawing.Font("Segoe UI Semibold", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtShip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtShip.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtShip.Location = new System.Drawing.Point(982, 9);
            this.dtShip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtShip.Name = "dtShip";
            this.dtShip.Size = new System.Drawing.Size(106, 23);
            this.dtShip.TabIndex = 53;
            // 
            // txtShipTo
            // 
            this.txtShipTo.BackColor = System.Drawing.Color.Transparent;
            this.txtShipTo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(221)))), ((int)(((byte)(226)))));
            this.txtShipTo.BorderRadius = 5;
            this.txtShipTo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtShipTo.DefaultText = "";
            this.txtShipTo.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtShipTo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtShipTo.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.txtShipTo.ForeColor = System.Drawing.Color.Black;
            this.txtShipTo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtShipTo.Location = new System.Drawing.Point(721, 10);
            this.txtShipTo.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.txtShipTo.Name = "txtShipTo";
            this.txtShipTo.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtShipTo.PlaceholderText = "Ship To";
            this.txtShipTo.SelectedText = "";
            this.txtShipTo.Size = new System.Drawing.Size(155, 24);
            this.txtShipTo.TabIndex = 51;
            // 
            // txtBillTo
            // 
            this.txtBillTo.BackColor = System.Drawing.Color.Transparent;
            this.txtBillTo.BorderRadius = 5;
            this.txtBillTo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBillTo.DefaultText = "";
            this.txtBillTo.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtBillTo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBillTo.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBillTo.ForeColor = System.Drawing.Color.Black;
            this.txtBillTo.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBillTo.Location = new System.Drawing.Point(483, 10);
            this.txtBillTo.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.txtBillTo.Name = "txtBillTo";
            this.txtBillTo.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtBillTo.PlaceholderText = "Bill To";
            this.txtBillTo.SelectedText = "";
            this.txtBillTo.Size = new System.Drawing.Size(200, 24);
            this.txtBillTo.TabIndex = 38;
            // 
            // guna2GroupBox3
            // 
            this.guna2GroupBox3.BackColor = System.Drawing.Color.Gainsboro;
            this.guna2GroupBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2GroupBox3.BorderThickness = 2;
            this.guna2GroupBox3.Controls.Add(this.richTextDescription);
            this.guna2GroupBox3.Controls.Add(this.label29);
            this.guna2GroupBox3.Controls.Add(this.txtTotalVat);
            this.guna2GroupBox3.Controls.Add(this.txtTotalBefore);
            this.guna2GroupBox3.Controls.Add(this.txtTotal);
            this.guna2GroupBox3.Controls.Add(this.label1);
            this.guna2GroupBox3.Controls.Add(this.label17);
            this.guna2GroupBox3.Controls.Add(this.label16);
            this.guna2GroupBox3.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox3.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2GroupBox3.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox3.Location = new System.Drawing.Point(3, 609);
            this.guna2GroupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Size = new System.Drawing.Size(1244, 94);
            this.guna2GroupBox3.TabIndex = 9;
            this.guna2GroupBox3.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // richTextDescription
            // 
            this.richTextDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextDescription.Location = new System.Drawing.Point(3, 22);
            this.richTextDescription.Name = "richTextDescription";
            this.richTextDescription.Size = new System.Drawing.Size(749, 69);
            this.richTextDescription.TabIndex = 9;
            this.richTextDescription.Text = "";
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.Transparent;
            this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label29.Location = new System.Drawing.Point(7, 4);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(214, 18);
            this.label29.TabIndex = 8;
            this.label29.Text = "Notes";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTotalVat
            // 
            this.txtTotalVat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalVat.BackColor = System.Drawing.Color.Transparent;
            this.txtTotalVat.BorderRadius = 5;
            this.txtTotalVat.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotalVat.DefaultText = "";
            this.txtTotalVat.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTotalVat.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTotalVat.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotalVat.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotalVat.Enabled = false;
            this.txtTotalVat.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalVat.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalVat.ForeColor = System.Drawing.Color.Black;
            this.txtTotalVat.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalVat.Location = new System.Drawing.Point(1098, 33);
            this.txtTotalVat.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtTotalVat.Name = "txtTotalVat";
            this.txtTotalVat.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotalVat.PlaceholderText = "Total Vat";
            this.txtTotalVat.ReadOnly = true;
            this.txtTotalVat.SelectedText = "";
            this.txtTotalVat.Size = new System.Drawing.Size(141, 24);
            this.txtTotalVat.TabIndex = 3;
            this.txtTotalVat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalVat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // txtTotalBefore
            // 
            this.txtTotalBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTotalBefore.BackColor = System.Drawing.Color.Transparent;
            this.txtTotalBefore.BorderRadius = 5;
            this.txtTotalBefore.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTotalBefore.DefaultText = "";
            this.txtTotalBefore.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTotalBefore.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTotalBefore.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotalBefore.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTotalBefore.Enabled = false;
            this.txtTotalBefore.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalBefore.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalBefore.ForeColor = System.Drawing.Color.Black;
            this.txtTotalBefore.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalBefore.Location = new System.Drawing.Point(1097, 4);
            this.txtTotalBefore.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.txtTotalBefore.Name = "txtTotalBefore";
            this.txtTotalBefore.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotalBefore.PlaceholderText = "Total Before Vat";
            this.txtTotalBefore.ReadOnly = true;
            this.txtTotalBefore.SelectedText = "";
            this.txtTotalBefore.Size = new System.Drawing.Size(142, 24);
            this.txtTotalBefore.TabIndex = 3;
            this.txtTotalBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalBefore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // txtTotal
            // 
            this.txtTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.ForeColor = System.Drawing.Color.Black;
            this.txtTotal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Location = new System.Drawing.Point(1098, 63);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotal.PlaceholderText = "Net Amount";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.SelectedText = "";
            this.txtTotal.Size = new System.Drawing.Size(142, 24);
            this.txtTotal.TabIndex = 3;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(977, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "Total Before Vat";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label17.Location = new System.Drawing.Point(979, 34);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(116, 23);
            this.label17.TabIndex = 5;
            this.label17.Text = "Total Vat";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label16.Location = new System.Drawing.Point(977, 63);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(116, 23);
            this.label16.TabIndex = 5;
            this.label16.Text = "Net Amount";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(3, 747);
            this.panel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1244, 2);
            this.panel5.TabIndex = 14;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1247, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(3, 749);
            this.panel4.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(3, 749);
            this.panel3.TabIndex = 12;
            // 
            // headerUC1
            // 
            this.headerUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(41)))), ((int)(((byte)(83)))));
            this.headerUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerUC1.FormText = "";
            this.headerUC1.HeaderText = "";
            this.headerUC1.Location = new System.Drawing.Point(3, 0);
            this.headerUC1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.headerUC1.Name = "headerUC1";
            this.headerUC1.Size = new System.Drawing.Size(1244, 37);
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
            this.guna2Button1.Location = new System.Drawing.Point(1173, 5);
            this.guna2Button1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(18, 19);
            this.guna2Button1.TabIndex = 30;
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
            this.guna2Button4.Location = new System.Drawing.Point(1199, 5);
            this.guna2Button4.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.Size = new System.Drawing.Size(18, 19);
            this.guna2Button4.TabIndex = 29;
            this.guna2Button4.Click += new System.EventHandler(this.guna2Button4_Click);
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1222, 5);
            this.guna2ControlBox1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(18, 19);
            this.guna2ControlBox1.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.tabControl2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(3, 37);
            this.panel9.Margin = new System.Windows.Forms.Padding(4);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1244, 109);
            this.panel9.TabIndex = 39;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.HotTrack = true;
            this.tabControl2.ImeMode = System.Windows.Forms.ImeMode.Katakana;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.Padding = new System.Drawing.Point(20, 3);
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1244, 109);
            this.tabControl2.TabIndex = 39;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage3.Controls.Add(this.CbSubcontractors);
            this.tabPage3.Controls.Add(this.guna2VSeparator7);
            this.tabPage3.Controls.Add(this.txtInvoiceId);
            this.tabPage3.Controls.Add(this.guna2TileButton12);
            this.tabPage3.Controls.Add(this.btnNext);
            this.tabPage3.Controls.Add(this.guna2TileButton13);
            this.tabPage3.Controls.Add(this.btnPrevious);
            this.tabPage3.Controls.Add(this.guna2VSeparator8);
            this.tabPage3.Controls.Add(this.guna2VSeparator12);
            this.tabPage3.Controls.Add(this.guna2TileButton14);
            this.tabPage3.Controls.Add(this.guna2TileButton23);
            this.tabPage3.Controls.Add(this.guna2TileButton15);
            this.tabPage3.Controls.Add(this.guna2TileButton22);
            this.tabPage3.Controls.Add(this.guna2VSeparator9);
            this.tabPage3.Controls.Add(this.guna2TileButton21);
            this.tabPage3.Controls.Add(this.guna2TileButton16);
            this.tabPage3.Controls.Add(this.guna2TileButton20);
            this.tabPage3.Controls.Add(this.guna2VSeparator10);
            this.tabPage3.Controls.Add(this.guna2TileButton19);
            this.tabPage3.Controls.Add(this.checkBox3);
            this.tabPage3.Controls.Add(this.guna2VSeparator11);
            this.tabPage3.Controls.Add(this.chkPrint);
            this.tabPage3.Controls.Add(this.guna2TileButton18);
            this.tabPage3.Controls.Add(this.guna2TileButton17);
            this.tabPage3.Location = new System.Drawing.Point(4, 21);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(1236, 84);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Main";
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
            this.CbSubcontractors.Location = new System.Drawing.Point(351, 65);
            this.CbSubcontractors.Margin = new System.Windows.Forms.Padding(2);
            this.CbSubcontractors.Name = "CbSubcontractors";
            this.CbSubcontractors.Size = new System.Drawing.Size(104, 17);
            this.CbSubcontractors.TabIndex = 59;
            this.CbSubcontractors.Text = "Subcontractors";
            this.CbSubcontractors.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.CbSubcontractors.UncheckedState.BorderRadius = 0;
            this.CbSubcontractors.UncheckedState.BorderThickness = 0;
            this.CbSubcontractors.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.CbSubcontractors.UseVisualStyleBackColor = false;
            this.CbSubcontractors.CheckedChanged += new System.EventHandler(this.CbSubcontractors_CheckedChanged);
            // 
            // guna2VSeparator7
            // 
            this.guna2VSeparator7.Location = new System.Drawing.Point(1158, 7);
            this.guna2VSeparator7.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator7.Name = "guna2VSeparator7";
            this.guna2VSeparator7.Size = new System.Drawing.Size(10, 61);
            this.guna2VSeparator7.TabIndex = 37;
            // 
            // txtInvoiceId
            // 
            this.txtInvoiceId.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtInvoiceId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInvoiceId.Enabled = false;
            this.txtInvoiceId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtInvoiceId.Location = new System.Drawing.Point(38, 29);
            this.txtInvoiceId.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInvoiceId.Multiline = true;
            this.txtInvoiceId.Name = "txtInvoiceId";
            this.txtInvoiceId.ReadOnly = true;
            this.txtInvoiceId.Size = new System.Drawing.Size(77, 18);
            this.txtInvoiceId.TabIndex = 16;
            this.txtInvoiceId.Text = "0";
            this.txtInvoiceId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // guna2TileButton12
            // 
            this.guna2TileButton12.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton12.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton12.BorderRadius = 10;
            this.guna2TileButton12.BorderThickness = 2;
            this.guna2TileButton12.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton12.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton12.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton12.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton12.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton12.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton12.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton12.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton12.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton12.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton12.Image = global::YamyProject.Properties.Resources.Get_Cash;
            this.guna2TileButton12.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton12.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton12.Location = new System.Drawing.Point(1016, 11);
            this.guna2TileButton12.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton12.Name = "guna2TileButton12";
            this.guna2TileButton12.Size = new System.Drawing.Size(146, 30);
            this.guna2TileButton12.TabIndex = 36;
            this.guna2TileButton12.Text = "Refund/ Payment";
            this.guna2TileButton12.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton12.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton12.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.FillColor = System.Drawing.Color.Transparent;
            this.btnNext.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Image = global::YamyProject.Properties.Resources.Arrow22;
            this.btnNext.ImageSize = new System.Drawing.Size(25, 25);
            this.btnNext.Location = new System.Drawing.Point(116, 19);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(38, 38);
            this.btnNext.TabIndex = 2;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // guna2TileButton13
            // 
            this.guna2TileButton13.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton13.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton13.BorderRadius = 10;
            this.guna2TileButton13.BorderThickness = 2;
            this.guna2TileButton13.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton13.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton13.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton13.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton13.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton13.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton13.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton13.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton13.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton13.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton13.Image = global::YamyProject.Properties.Resources.Online_Payment;
            this.guna2TileButton13.Location = new System.Drawing.Point(937, 10);
            this.guna2TileButton13.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton13.Name = "guna2TileButton13";
            this.guna2TileButton13.Size = new System.Drawing.Size(80, 62);
            this.guna2TileButton13.TabIndex = 35;
            this.guna2TileButton13.Text = "Receive Payments";
            this.guna2TileButton13.Visible = false;
            // 
            // btnPrevious
            // 
            this.btnPrevious.FillColor = System.Drawing.Color.Transparent;
            this.btnPrevious.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnPrevious.ForeColor = System.Drawing.Color.White;
            this.btnPrevious.Image = global::YamyProject.Properties.Resources.Arrow_Pointing_Left;
            this.btnPrevious.Location = new System.Drawing.Point(4, 21);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(35, 36);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // guna2VSeparator8
            // 
            this.guna2VSeparator8.Location = new System.Drawing.Point(928, 7);
            this.guna2VSeparator8.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator8.Name = "guna2VSeparator8";
            this.guna2VSeparator8.Size = new System.Drawing.Size(10, 61);
            this.guna2VSeparator8.TabIndex = 34;
            // 
            // guna2VSeparator12
            // 
            this.guna2VSeparator12.Location = new System.Drawing.Point(153, 7);
            this.guna2VSeparator12.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator12.Name = "guna2VSeparator12";
            this.guna2VSeparator12.Size = new System.Drawing.Size(10, 61);
            this.guna2VSeparator12.TabIndex = 17;
            // 
            // guna2TileButton14
            // 
            this.guna2TileButton14.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton14.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton14.BorderRadius = 10;
            this.guna2TileButton14.BorderThickness = 2;
            this.guna2TileButton14.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton14.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton14.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton14.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton14.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton14.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton14.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton14.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton14.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton14.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton14.Image = global::YamyProject.Properties.Resources.Clipboard_Approve;
            this.guna2TileButton14.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton14.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton14.Location = new System.Drawing.Point(813, 40);
            this.guna2TileButton14.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton14.Name = "guna2TileButton14";
            this.guna2TileButton14.Size = new System.Drawing.Size(116, 30);
            this.guna2TileButton14.TabIndex = 33;
            this.guna2TileButton14.Text = "Create a Copy";
            this.guna2TileButton14.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton14.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton14.Visible = false;
            // 
            // guna2TileButton23
            // 
            this.guna2TileButton23.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton23.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton23.BorderRadius = 10;
            this.guna2TileButton23.BorderThickness = 2;
            this.guna2TileButton23.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton23.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton23.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton23.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton23.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton23.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton23.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton23.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton23.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton23.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton23.Image = global::YamyProject.Properties.Resources.add_fileN;
            this.guna2TileButton23.Location = new System.Drawing.Point(162, 11);
            this.guna2TileButton23.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton23.Name = "guna2TileButton23";
            this.guna2TileButton23.Size = new System.Drawing.Size(63, 62);
            this.guna2TileButton23.TabIndex = 19;
            this.guna2TileButton23.Text = "New";
            this.guna2TileButton23.Click += new System.EventHandler(this.guna2TileButton23_Click);
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
            this.guna2TileButton15.Location = new System.Drawing.Point(811, 10);
            this.guna2TileButton15.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton15.Name = "guna2TileButton15";
            this.guna2TileButton15.Size = new System.Drawing.Size(135, 30);
            this.guna2TileButton15.TabIndex = 32;
            this.guna2TileButton15.Text = "Add Time / Costs";
            this.guna2TileButton15.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton15.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton15.Visible = false;
            // 
            // guna2TileButton22
            // 
            this.guna2TileButton22.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton22.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton22.BorderRadius = 10;
            this.guna2TileButton22.BorderThickness = 2;
            this.guna2TileButton22.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton22.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton22.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton22.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton22.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton22.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton22.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton22.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton22.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton22.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton22.Image = global::YamyProject.Properties.Resources.Save;
            this.guna2TileButton22.Location = new System.Drawing.Point(225, 11);
            this.guna2TileButton22.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton22.Name = "guna2TileButton22";
            this.guna2TileButton22.Size = new System.Drawing.Size(63, 62);
            this.guna2TileButton22.TabIndex = 20;
            this.guna2TileButton22.Text = "Save";
            this.guna2TileButton22.Click += new System.EventHandler(this.guna2TileButton22_Click);
            // 
            // guna2VSeparator9
            // 
            this.guna2VSeparator9.Location = new System.Drawing.Point(802, 7);
            this.guna2VSeparator9.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator9.Name = "guna2VSeparator9";
            this.guna2VSeparator9.Size = new System.Drawing.Size(10, 61);
            this.guna2VSeparator9.TabIndex = 31;
            // 
            // guna2TileButton21
            // 
            this.guna2TileButton21.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton21.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton21.BorderRadius = 10;
            this.guna2TileButton21.BorderThickness = 2;
            this.guna2TileButton21.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton21.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton21.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton21.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton21.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton21.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton21.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton21.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton21.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton21.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton21.Image = global::YamyProject.Properties.Resources.deleteN;
            this.guna2TileButton21.Location = new System.Drawing.Point(279, 11);
            this.guna2TileButton21.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton21.Name = "guna2TileButton21";
            this.guna2TileButton21.Size = new System.Drawing.Size(64, 62);
            this.guna2TileButton21.TabIndex = 21;
            this.guna2TileButton21.Text = "Delete";
            this.guna2TileButton21.Click += new System.EventHandler(this.guna2TileButton21_Click);
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
            this.guna2TileButton16.Location = new System.Drawing.Point(728, 10);
            this.guna2TileButton16.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton16.Name = "guna2TileButton16";
            this.guna2TileButton16.Size = new System.Drawing.Size(74, 62);
            this.guna2TileButton16.TabIndex = 30;
            this.guna2TileButton16.Text = "Attach File";
            this.guna2TileButton16.Visible = false;
            // 
            // guna2TileButton20
            // 
            this.guna2TileButton20.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton20.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton20.BorderRadius = 10;
            this.guna2TileButton20.BorderThickness = 2;
            this.guna2TileButton20.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton20.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton20.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton20.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton20.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton20.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton20.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton20.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton20.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton20.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton20.Image = global::YamyProject.Properties.Resources.copyN;
            this.guna2TileButton20.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton20.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton20.Location = new System.Drawing.Point(351, 11);
            this.guna2TileButton20.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton20.Name = "guna2TileButton20";
            this.guna2TileButton20.Size = new System.Drawing.Size(116, 30);
            this.guna2TileButton20.TabIndex = 22;
            this.guna2TileButton20.Text = "Create a Copy";
            this.guna2TileButton20.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton20.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton20.Click += new System.EventHandler(this.guna2TileButton20_Click);
            // 
            // guna2VSeparator10
            // 
            this.guna2VSeparator10.Location = new System.Drawing.Point(713, 7);
            this.guna2VSeparator10.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator10.Name = "guna2VSeparator10";
            this.guna2VSeparator10.Size = new System.Drawing.Size(10, 61);
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
            this.guna2TileButton19.Location = new System.Drawing.Point(350, 40);
            this.guna2TileButton19.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton19.Name = "guna2TileButton19";
            this.guna2TileButton19.Size = new System.Drawing.Size(116, 30);
            this.guna2TileButton19.TabIndex = 23;
            this.guna2TileButton19.Text = "Memorize";
            this.guna2TileButton19.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton19.TextOffset = new System.Drawing.Point(10, -12);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox3.Location = new System.Drawing.Point(602, 44);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(68, 16);
            this.checkBox3.TabIndex = 28;
            this.checkBox3.Text = "Email Later";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // guna2VSeparator11
            // 
            this.guna2VSeparator11.Location = new System.Drawing.Point(468, 7);
            this.guna2VSeparator11.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator11.Name = "guna2VSeparator11";
            this.guna2VSeparator11.Size = new System.Drawing.Size(10, 61);
            this.guna2VSeparator11.TabIndex = 24;
            // 
            // chkPrint
            // 
            this.chkPrint.AutoSize = true;
            this.chkPrint.Checked = true;
            this.chkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrint.Location = new System.Drawing.Point(603, 16);
            this.chkPrint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Size = new System.Drawing.Size(64, 16);
            this.chkPrint.TabIndex = 27;
            this.chkPrint.Text = "Print Later";
            this.chkPrint.UseVisualStyleBackColor = true;
            // 
            // guna2TileButton18
            // 
            this.guna2TileButton18.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton18.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton18.BorderRadius = 10;
            this.guna2TileButton18.BorderThickness = 2;
            this.guna2TileButton18.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton18.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton18.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton18.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton18.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton18.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton18.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton18.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton18.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton18.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton18.Image = global::YamyProject.Properties.Resources.printN;
            this.guna2TileButton18.Location = new System.Drawing.Point(477, 11);
            this.guna2TileButton18.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton18.Name = "guna2TileButton18";
            this.guna2TileButton18.Size = new System.Drawing.Size(63, 62);
            this.guna2TileButton18.TabIndex = 25;
            this.guna2TileButton18.Text = "Print";
            this.guna2TileButton18.Click += new System.EventHandler(this.guna2TileButton18_Click);
            // 
            // guna2TileButton17
            // 
            this.guna2TileButton17.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton17.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton17.BorderRadius = 10;
            this.guna2TileButton17.BorderThickness = 2;
            this.guna2TileButton17.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton17.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton17.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton17.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton17.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton17.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton17.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TileButton17.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton17.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton17.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton17.Image = global::YamyProject.Properties.Resources.emailN;
            this.guna2TileButton17.Location = new System.Drawing.Point(535, 11);
            this.guna2TileButton17.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton17.Name = "guna2TileButton17";
            this.guna2TileButton17.Size = new System.Drawing.Size(69, 62);
            this.guna2TileButton17.TabIndex = 26;
            this.guna2TileButton17.Text = "Email";
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
            this.tabPage4.Location = new System.Drawing.Point(4, 21);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage4.Size = new System.Drawing.Size(1236, 84);
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
            this.guna2TileButton27.Location = new System.Drawing.Point(449, 10);
            this.guna2TileButton27.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton27.Name = "guna2TileButton27";
            this.guna2TileButton27.Size = new System.Drawing.Size(109, 62);
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
            this.guna2TileButton28.Location = new System.Drawing.Point(340, 10);
            this.guna2TileButton28.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton28.Name = "guna2TileButton28";
            this.guna2TileButton28.Size = new System.Drawing.Size(108, 62);
            this.guna2TileButton28.TabIndex = 25;
            this.guna2TileButton28.Text = "Purchase By Vendor Details";
            this.guna2TileButton28.Click += new System.EventHandler(this.guna2TileButton28_Click);
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
            this.guna2TileButton29.Location = new System.Drawing.Point(259, 10);
            this.guna2TileButton29.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton29.Name = "guna2TileButton29";
            this.guna2TileButton29.Size = new System.Drawing.Size(81, 62);
            this.guna2TileButton29.TabIndex = 24;
            this.guna2TileButton29.Text = "View Open Invoice";
            // 
            // guna2VSeparator13
            // 
            this.guna2VSeparator13.Location = new System.Drawing.Point(249, 10);
            this.guna2VSeparator13.Margin = new System.Windows.Forms.Padding(4);
            this.guna2VSeparator13.Name = "guna2VSeparator13";
            this.guna2VSeparator13.Size = new System.Drawing.Size(10, 61);
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
            this.guna2TileButton26.Location = new System.Drawing.Point(158, 10);
            this.guna2TileButton26.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton26.Name = "guna2TileButton26";
            this.guna2TileButton26.Size = new System.Drawing.Size(92, 62);
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
            this.guna2TileButton25.Location = new System.Drawing.Point(74, 10);
            this.guna2TileButton25.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton25.Name = "guna2TileButton25";
            this.guna2TileButton25.Size = new System.Drawing.Size(84, 62);
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
            this.guna2TileButton24.Location = new System.Drawing.Point(4, 4);
            this.guna2TileButton24.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton24.Name = "guna2TileButton24";
            this.guna2TileButton24.Size = new System.Drawing.Size(70, 67);
            this.guna2TileButton24.TabIndex = 20;
            this.guna2TileButton24.Text = "Quick Report";
            this.guna2TileButton24.Click += new System.EventHandler(this.guna2TileButton24_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(221)))), ((int)(((byte)(238)))));
            this.panel7.Controls.Add(this.txtBillTo);
            this.panel7.Controls.Add(this.cmbShipVia);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.label18);
            this.panel7.Controls.Add(this.cmbVendor);
            this.panel7.Controls.Add(this.lnkNewVendor);
            this.panel7.Controls.Add(this.dtShip);
            this.panel7.Controls.Add(this.txtVendorCode);
            this.panel7.Controls.Add(this.label11);
            this.panel7.Controls.Add(this.label14);
            this.panel7.Controls.Add(this.txtShipTo);
            this.panel7.Controls.Add(this.label10);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 146);
            this.panel7.Margin = new System.Windows.Forms.Padding(4);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1244, 46);
            this.panel7.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(1096, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 21);
            this.label6.TabIndex = 54;
            this.label6.Text = "Via";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label18.Location = new System.Drawing.Point(459, 15);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(25, 21);
            this.label18.TabIndex = 39;
            this.label18.Text = "Bill To";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbVendor
            // 
            this.cmbVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVendor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVendor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbVendor.ItemHeight = 15;
            this.cmbVendor.Location = new System.Drawing.Point(166, 10);
            this.cmbVendor.Margin = new System.Windows.Forms.Padding(4);
            this.cmbVendor.Name = "cmbVendor";
            this.cmbVendor.Size = new System.Drawing.Size(260, 23);
            this.cmbVendor.TabIndex = 4;
            this.cmbVendor.SelectedIndexChanged += new System.EventHandler(this.cmbVendor_SelectedIndexChanged);
            // 
            // lnkNewVendor
            // 
            this.lnkNewVendor.ActiveLinkColor = System.Drawing.Color.Green;
            this.lnkNewVendor.BackColor = System.Drawing.Color.Transparent;
            this.lnkNewVendor.DisabledLinkColor = System.Drawing.Color.Blue;
            this.lnkNewVendor.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkNewVendor.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lnkNewVendor.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lnkNewVendor.Location = new System.Drawing.Point(409, 10);
            this.lnkNewVendor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lnkNewVendor.Name = "lnkNewVendor";
            this.lnkNewVendor.Size = new System.Drawing.Size(59, 20);
            this.lnkNewVendor.TabIndex = 0;
            this.lnkNewVendor.TabStop = true;
            this.lnkNewVendor.Text = "New";
            this.lnkNewVendor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkNewVendor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNewVendor_LinkClicked);
            // 
            // txtVendorCode
            // 
            this.txtVendorCode.BackColor = System.Drawing.Color.Transparent;
            this.txtVendorCode.BorderRadius = 5;
            this.txtVendorCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVendorCode.DefaultText = "";
            this.txtVendorCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVendorCode.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.txtVendorCode.ForeColor = System.Drawing.Color.Black;
            this.txtVendorCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtVendorCode.Location = new System.Drawing.Point(38, 9);
            this.txtVendorCode.Margin = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.txtVendorCode.Name = "txtVendorCode";
            this.txtVendorCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtVendorCode.PlaceholderText = "";
            this.txtVendorCode.SelectedText = "";
            this.txtVendorCode.Size = new System.Drawing.Size(90, 24);
            this.txtVendorCode.TabIndex = 3;
            this.txtVendorCode.TextChanged += new System.EventHandler(this.txtVendorCode_TextChanged);
            this.txtVendorCode.Leave += new System.EventHandler(this.txtVendorCode_Leave);
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label11.Location = new System.Drawing.Point(9, 13);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 17);
            this.label11.TabIndex = 13;
            this.label11.Text = "Code";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label14.Location = new System.Drawing.Point(136, 10);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 21);
            this.label14.TabIndex = 5;
            this.label14.Text = "Vendor";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label10.Location = new System.Drawing.Point(916, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 21);
            this.label10.TabIndex = 52;
            this.label10.Text = "Ship Date";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.Location = new System.Drawing.Point(693, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 21);
            this.label7.TabIndex = 40;
            this.label7.Text = "Ship";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BorderColor = System.Drawing.Color.Silver;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.guna2TileButton31);
            this.guna2Panel1.Controls.Add(this.guna2TileButton30);
            this.guna2Panel1.CustomBorderThickness = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.guna2Panel1.Location = new System.Drawing.Point(513, 125);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(130, 68);
            this.guna2Panel1.TabIndex = 57;
            // 
            // guna2TileButton31
            // 
            this.guna2TileButton31.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton31.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton31.BorderRadius = 1;
            this.guna2TileButton31.BorderThickness = 2;
            this.guna2TileButton31.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton31.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton31.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton31.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton31.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton31.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2TileButton31.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton31.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.guna2TileButton31.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton31.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton31.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton31.Image = global::YamyProject.Properties.Resources.Delivery1;
            this.guna2TileButton31.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton31.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton31.Location = new System.Drawing.Point(0, 30);
            this.guna2TileButton31.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton31.Name = "guna2TileButton31";
            this.guna2TileButton31.Size = new System.Drawing.Size(130, 30);
            this.guna2TileButton31.TabIndex = 34;
            this.guna2TileButton31.Text = "Recive Report";
            this.guna2TileButton31.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton31.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton31.Click += new System.EventHandler(this.guna2TileButton31_Click);
            // 
            // guna2TileButton30
            // 
            this.guna2TileButton30.BackColor = System.Drawing.Color.Transparent;
            this.guna2TileButton30.BorderColor = System.Drawing.Color.Transparent;
            this.guna2TileButton30.BorderRadius = 1;
            this.guna2TileButton30.BorderThickness = 2;
            this.guna2TileButton30.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(55)))), ((int)(((byte)(89)))));
            this.guna2TileButton30.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton30.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2TileButton30.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2TileButton30.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2TileButton30.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2TileButton30.FillColor = System.Drawing.Color.Transparent;
            this.guna2TileButton30.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.guna2TileButton30.ForeColor = System.Drawing.Color.Black;
            this.guna2TileButton30.HoverState.BorderColor = System.Drawing.Color.Silver;
            this.guna2TileButton30.HoverState.FillColor = System.Drawing.Color.Silver;
            this.guna2TileButton30.Image = global::YamyProject.Properties.Resources.Invoice_Paid;
            this.guna2TileButton30.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton30.ImageOffset = new System.Drawing.Point(-10, 10);
            this.guna2TileButton30.Location = new System.Drawing.Point(0, 0);
            this.guna2TileButton30.Margin = new System.Windows.Forms.Padding(4);
            this.guna2TileButton30.Name = "guna2TileButton30";
            this.guna2TileButton30.Size = new System.Drawing.Size(130, 30);
            this.guna2TileButton30.TabIndex = 33;
            this.guna2TileButton30.Text = "invoice Report ";
            this.guna2TileButton30.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton30.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton30.Click += new System.EventHandler(this.guna2TileButton30_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 6;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 6;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // guna2DragControl1
            // 
            this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2DragControl1.UseTransparentDrag = true;
            // 
            // lstAccountSuggestions
            // 
            this.lstAccountSuggestions.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstAccountSuggestions.FormattingEnabled = true;
            this.lstAccountSuggestions.Location = new System.Drawing.Point(471, 351);
            this.lstAccountSuggestions.Name = "lstAccountSuggestions";
            this.lstAccountSuggestions.Size = new System.Drawing.Size(308, 30);
            this.lstAccountSuggestions.TabIndex = 58;
            this.lstAccountSuggestions.TabStop = false;
            this.lstAccountSuggestions.Visible = false;
            this.lstAccountSuggestions.Click += new System.EventHandler(this.lstAccountSuggestions_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn1.HeaderText = "id";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Visible = false;
            this.dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewTextBoxColumn2.FillWeight = 30F;
            this.dataGridViewTextBoxColumn2.HeaderText = "#";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 35;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle45.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle45;
            this.dataGridViewTextBoxColumn3.HeaderText = "Item Code";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 180;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.HeaderText = "QTY";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle46.Format = "N2";
            dataGridViewCellStyle46.NullValue = "0";
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle46;
            this.dataGridViewTextBoxColumn5.FillWeight = 75F;
            this.dataGridViewTextBoxColumn5.HeaderText = "cost_price";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle47.Format = "N2";
            dataGridViewCellStyle47.NullValue = "0";
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle47;
            this.dataGridViewTextBoxColumn6.HeaderText = "Price";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 80;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle48.Format = "N2";
            dataGridViewCellStyle48.NullValue = "0";
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle48;
            this.dataGridViewTextBoxColumn7.HeaderText = "Disc";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Visible = false;
            this.dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle49.Format = "N2";
            dataGridViewCellStyle49.NullValue = "0";
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle49;
            this.dataGridViewTextBoxColumn8.HeaderText = "Net Price";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 125;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle50.Format = "N2";
            dataGridViewCellStyle50.NullValue = "0";
            this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle50;
            this.dataGridViewTextBoxColumn9.HeaderText = "~";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 125;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle51.Format = "N2";
            dataGridViewCellStyle51.NullValue = "0";
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle51;
            this.dataGridViewTextBoxColumn10.HeaderText = "Amount";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 130;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            dataGridViewCellStyle52.Format = "N2";
            dataGridViewCellStyle52.NullValue = "0";
            this.dataGridViewTextBoxColumn11.DefaultCellStyle = dataGridViewCellStyle52;
            this.dataGridViewTextBoxColumn11.HeaderText = "method";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Visible = false;
            this.dataGridViewTextBoxColumn11.Width = 125;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle53.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewTextBoxColumn12.DefaultCellStyle = dataGridViewCellStyle53;
            this.dataGridViewTextBoxColumn12.HeaderText = "type";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.Visible = false;
            this.dataGridViewTextBoxColumn12.Width = 125;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle54.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter;
            this.dataGridViewTextBoxColumn13.DefaultCellStyle = dataGridViewCellStyle54;
            this.dataGridViewTextBoxColumn13.HeaderText = "type";
            this.dataGridViewTextBoxColumn13.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.Visible = false;
            this.dataGridViewTextBoxColumn13.Width = 125;
            // 
            // frmPurchase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1250, 749);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2GroupBox4);
            this.Controls.Add(this.guna2GroupBox1);
            this.Controls.Add(this.guna2GroupBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.lstAccountSuggestions);
            this.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmPurchase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Purchase Invoice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPurchase_FormClosing);
            this.Load += new System.EventHandler(this.frmPurchase_Load);
            this.panel1.ResumeLayout(false);
            this.guna2GroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox3.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox4;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox3;
        private Guna.UI2.WinForms.Guna2DataGridView dgvItems;
        private Guna.UI2.WinForms.Guna2TextBox txtTotal;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private Guna.UI2.WinForms.Guna2TextBox txtTotalVat;
        private Guna.UI2.WinForms.Guna2TextBox txtTotalBefore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private Guna.UI2.WinForms.Guna2TextBox txtSalesMan;
        private System.Windows.Forms.DateTimePicker dtInv;
        private Guna.UI2.WinForms.Guna2ComboBox cmbWarehouse;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator7;
        private System.Windows.Forms.TextBox txtInvoiceId;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton12;
        private Guna.UI2.WinForms.Guna2Button btnNext;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton13;
        private Guna.UI2.WinForms.Guna2Button btnPrevious;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator8;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator12;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton14;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton23;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton15;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton22;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator9;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton21;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton16;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton20;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator10;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton19;
        private System.Windows.Forms.CheckBox checkBox3;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator11;
        private System.Windows.Forms.CheckBox chkPrint;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton18;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton17;
        private System.Windows.Forms.TabPage tabPage4;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton27;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton28;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton29;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator13;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton26;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton25;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton24;
        private System.Windows.Forms.Panel panel7;
        public System.Windows.Forms.ComboBox cmbVendor;
        private System.Windows.Forms.LinkLabel lnkNewVendor;
        private Guna.UI2.WinForms.Guna2TextBox txtVendorCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label14;
        private Guna.UI2.WinForms.Guna2TextBox txtPONO;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2TextBox txtBillTo;
        private Guna.UI2.WinForms.Guna2TextBox txtNextCode;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPaymentTerms;
        private System.Windows.Forms.ComboBox cmbAccountCashName;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPaymentMethod;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtPaymentTerms;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCity;
        private System.Windows.Forms.Label label9;
        private Guna.UI2.WinForms.Guna2TextBox txtShipTo;
        private System.Windows.Forms.DateTimePicker dtShip;
        private Guna.UI2.WinForms.Guna2ComboBox cmbShipVia;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton31;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton30;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label20;
        private Guna.UI2.WinForms.Guna2Button btnSaveClose;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPurchasetype;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFixedAssetCategory;
        private System.Windows.Forms.Label labelFixedAssetCategory;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private ListBox lstAccountSuggestions;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn itemId;
        private DataGridViewTextBoxColumn no;
        private DataGridViewTextBoxColumn code;
        private DataGridViewTextBoxColumn name;
        private DataGridViewTextBoxColumn qty;
        private DataGridViewTextBoxColumn cost_price;
        private DataGridViewTextBoxColumn price;
        private DataGridViewTextBoxColumn discount;
        private DataGridViewTextBoxColumn net_price;
        private DataGridViewComboBoxColumn vat;
        private DataGridViewTextBoxColumn VatP;
        private DataGridViewTextBoxColumn total;
        private DataGridViewTextBoxColumn method;
        private DataGridViewTextBoxColumn type;
        private DataGridViewComboBoxColumn cost_center;
        private DataGridViewButtonColumn delete;
        private Guna.UI2.WinForms.Guna2TextBox txtBarcodeScan;
        private Guna.UI2.WinForms.Guna2CheckBox CbSubcontractors;
        private RichTextBox richTextDescription;
        private Label label29;
    }
}