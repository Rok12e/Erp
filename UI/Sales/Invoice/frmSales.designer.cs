using System.Windows.Forms;

namespace YamyProject
{
    partial class frmSales
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.cmbPaymentMethod = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.txtCustomerCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.guna2GroupBox1 = new Guna.UI2.WinForms.Guna2GroupBox();
            this.guna2TextBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtSalesMan = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbPaymentTerms = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbAccountCashName = new System.Windows.Forms.ComboBox();
            this.dtShip = new System.Windows.Forms.DateTimePicker();
            this.dtPaymentTerms = new System.Windows.Forms.DateTimePicker();
            this.dtInv = new System.Windows.Forms.DateTimePicker();
            this.cmbCity = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbShipVia = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbWarehouse = new Guna.UI2.WinForms.Guna2ComboBox();
            this.txtNextCode = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtPONO = new Guna.UI2.WinForms.Guna2TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtShipTo = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtBillTo = new Guna.UI2.WinForms.Guna2TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.txtBarcodeScan = new Guna.UI2.WinForms.Guna2TextBox();
            this.lnkNewCustomer = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2Button10 = new Guna.UI2.WinForms.Guna2Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2TileButton31 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2TileButton30 = new Guna.UI2.WinForms.Guna2TileButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel9 = new System.Windows.Forms.Panel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtId = new System.Windows.Forms.TextBox();
            this.guna2VSeparator7 = new Guna.UI2.WinForms.Guna2VSeparator();
            this.guna2TileButton12 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2Button3 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TileButton13 = new Guna.UI2.WinForms.Guna2TileButton();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
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
            this.guna2ControlBox1 = new Guna.UI2.WinForms.Guna2ControlBox();
            this.headerUC1 = new YamyProject.HeaderUC();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button4 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2ResizeForm1 = new Guna.UI2.WinForms.Guna2ResizeForm(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
            this.lstAccountSuggestions = new System.Windows.Forms.ListBox();
            this.guna2GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.guna2GroupBox1.SuspendLayout();
            this.guna2GroupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.panel9.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
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
            this.btnClose.Location = new System.Drawing.Point(959, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(74, 27);
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
            this.btnSave.Location = new System.Drawing.Point(877, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 27);
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
            this.guna2GroupBox4.Location = new System.Drawing.Point(2, 298);
            this.guna2GroupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.guna2GroupBox4.Name = "guna2GroupBox4";
            this.guna2GroupBox4.ShadowDecoration.BorderRadius = 0;
            this.guna2GroupBox4.ShadowDecoration.Depth = 0;
            this.guna2GroupBox4.Size = new System.Drawing.Size(1036, 204);
            this.guna2GroupBox4.TabIndex = 10;
            // 
            // dgvItems
            // 
            this.dgvItems.AllowUserToDeleteRows = false;
            this.dgvItems.AllowUserToResizeColumns = false;
            this.dgvItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle16.ForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
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
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.DefaultCellStyle = dataGridViewCellStyle20;
            this.dgvItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvItems.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.Location = new System.Drawing.Point(0, 0);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dgvItems.MultiSelect = false;
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvItems.RowHeadersVisible = false;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.dgvItems.RowsDefaultCellStyle = dataGridViewCellStyle21;
            this.dgvItems.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItems.RowTemplate.Height = 25;
            this.dgvItems.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvItems.Size = new System.Drawing.Size(1036, 204);
            this.dgvItems.TabIndex = 7;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.Silver;
            this.dgvItems.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvItems.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold);
            this.dgvItems.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvItems.ThemeStyle.HeaderStyle.Height = 18;
            this.dgvItems.ThemeStyle.ReadOnly = false;
            this.dgvItems.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Single;
            this.dgvItems.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItems.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.RowsStyle.Height = 25;
            this.dgvItems.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellClick);
            this.dgvItems.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellEndEdit);
            this.dgvItems.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItems_CellValueChanged);
            this.dgvItems.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvItems_DefaultValuesNeeded);
            this.dgvItems.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvItems_EditingControlShowing);
            this.dgvItems.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvItems_RowPostPaint);
            // 
            // itemId
            // 
            this.itemId.HeaderText = "id";
            this.itemId.Name = "itemId";
            this.itemId.Visible = false;
            // 
            // no
            // 
            this.no.HeaderText = "#";
            this.no.Name = "no";
            this.no.ReadOnly = true;
            this.no.Width = 35;
            // 
            // code
            // 
            this.code.FillWeight = 70F;
            this.code.HeaderText = "Item Code";
            this.code.MinimumWidth = 70;
            this.code.Name = "code";
            this.code.Width = 70;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Item Name";
            this.name.MinimumWidth = 100;
            this.name.Name = "name";
            this.name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // qty
            // 
            this.qty.FillWeight = 80F;
            this.qty.HeaderText = "Qty";
            this.qty.MinimumWidth = 70;
            this.qty.Name = "qty";
            this.qty.Width = 70;
            // 
            // cost_price
            // 
            this.cost_price.HeaderText = "Cost Price";
            this.cost_price.Name = "cost_price";
            this.cost_price.Visible = false;
            // 
            // price
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.price.DefaultCellStyle = dataGridViewCellStyle17;
            this.price.HeaderText = "Price";
            this.price.Name = "price";
            this.price.Width = 70;
            // 
            // discount
            // 
            this.discount.HeaderText = "Disc";
            this.discount.MinimumWidth = 60;
            this.discount.Name = "discount";
            this.discount.Width = 60;
            // 
            // net_price
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.Format = "N2";
            dataGridViewCellStyle18.NullValue = "0";
            this.net_price.DefaultCellStyle = dataGridViewCellStyle18;
            this.net_price.HeaderText = "Net Price";
            this.net_price.MinimumWidth = 70;
            this.net_price.Name = "net_price";
            this.net_price.ReadOnly = true;
            this.net_price.Width = 70;
            // 
            // vat
            // 
            this.vat.HeaderText = "Vat";
            this.vat.Name = "vat";
            this.vat.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // VatP
            // 
            this.VatP.HeaderText = "~";
            this.VatP.Name = "VatP";
            // 
            // total
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.Format = "N2";
            dataGridViewCellStyle19.NullValue = "0";
            this.total.DefaultCellStyle = dataGridViewCellStyle19;
            this.total.HeaderText = "Amount";
            this.total.MinimumWidth = 80;
            this.total.Name = "total";
            this.total.ReadOnly = true;
            this.total.Width = 80;
            // 
            // method
            // 
            this.method.HeaderText = "method";
            this.method.Name = "method";
            this.method.Visible = false;
            // 
            // type
            // 
            this.type.HeaderText = "type";
            this.type.Name = "type";
            this.type.Visible = false;
            // 
            // cost_center
            // 
            this.cost_center.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cost_center.FillWeight = 20F;
            this.cost_center.HeaderText = "Cost Center";
            this.cost_center.MinimumWidth = 70;
            this.cost_center.Name = "cost_center";
            // 
            // delete
            // 
            this.delete.HeaderText = "DEL";
            this.delete.MinimumWidth = 40;
            this.delete.Name = "delete";
            this.delete.Text = "x";
            this.delete.Width = 40;
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
            this.cmbPaymentMethod.Location = new System.Drawing.Point(2, 69);
            this.cmbPaymentMethod.Name = "cmbPaymentMethod";
            this.cmbPaymentMethod.Size = new System.Drawing.Size(123, 24);
            this.cmbPaymentMethod.TabIndex = 5;
            this.cmbPaymentMethod.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentMethod_SelectedIndexChanged);
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCustomer.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCustomer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbCustomer.ItemHeight = 15;
            this.cmbCustomer.Location = new System.Drawing.Point(236, 6);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(338, 23);
            this.cmbCustomer.TabIndex = 4;
            this.cmbCustomer.SelectedIndexChanged += new System.EventHandler(this.cmbCustomer_SelectedIndexChanged);
            // 
            // txtCustomerCode
            // 
            this.txtCustomerCode.BackColor = System.Drawing.Color.Transparent;
            this.txtCustomerCode.BorderRadius = 5;
            this.txtCustomerCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCustomerCode.DefaultText = "";
            this.txtCustomerCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCustomerCode.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold);
            this.txtCustomerCode.ForeColor = System.Drawing.Color.Black;
            this.txtCustomerCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtCustomerCode.Location = new System.Drawing.Point(69, 5);
            this.txtCustomerCode.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.txtCustomerCode.Name = "txtCustomerCode";
            this.txtCustomerCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtCustomerCode.PlaceholderText = "";
            this.txtCustomerCode.SelectedText = "";
            this.txtCustomerCode.Size = new System.Drawing.Size(112, 24);
            this.txtCustomerCode.TabIndex = 3;
            this.txtCustomerCode.TextChanged += new System.EventHandler(this.txtCustomerCode_TextChanged);
            this.txtCustomerCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            this.txtCustomerCode.Leave += new System.EventHandler(this.txtCustomerCode_Leave);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label3.Location = new System.Drawing.Point(0, 51);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Invoice Type";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(184, 8);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "Customer";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guna2GroupBox1
            // 
            this.guna2GroupBox1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.BorderThickness = 0;
            this.guna2GroupBox1.Controls.Add(this.guna2TextBox2);
            this.guna2GroupBox1.Controls.Add(this.guna2TextBox1);
            this.guna2GroupBox1.Controls.Add(this.txtSalesMan);
            this.guna2GroupBox1.Controls.Add(this.cmbPaymentTerms);
            this.guna2GroupBox1.Controls.Add(this.cmbAccountCashName);
            this.guna2GroupBox1.Controls.Add(this.cmbPaymentMethod);
            this.guna2GroupBox1.Controls.Add(this.dtShip);
            this.guna2GroupBox1.Controls.Add(this.dtPaymentTerms);
            this.guna2GroupBox1.Controls.Add(this.dtInv);
            this.guna2GroupBox1.Controls.Add(this.cmbCity);
            this.guna2GroupBox1.Controls.Add(this.cmbShipVia);
            this.guna2GroupBox1.Controls.Add(this.cmbWarehouse);
            this.guna2GroupBox1.Controls.Add(this.txtNextCode);
            this.guna2GroupBox1.Controls.Add(this.txtPONO);
            this.guna2GroupBox1.Controls.Add(this.label18);
            this.guna2GroupBox1.Controls.Add(this.label12);
            this.guna2GroupBox1.Controls.Add(this.label8);
            this.guna2GroupBox1.Controls.Add(this.label14);
            this.guna2GroupBox1.Controls.Add(this.label19);
            this.guna2GroupBox1.Controls.Add(this.label11);
            this.guna2GroupBox1.Controls.Add(this.txtShipTo);
            this.guna2GroupBox1.Controls.Add(this.txtBillTo);
            this.guna2GroupBox1.Controls.Add(this.label15);
            this.guna2GroupBox1.Controls.Add(this.label9);
            this.guna2GroupBox1.Controls.Add(this.label10);
            this.guna2GroupBox1.Controls.Add(this.label3);
            this.guna2GroupBox1.Controls.Add(this.label6);
            this.guna2GroupBox1.Controls.Add(this.label4);
            this.guna2GroupBox1.Controls.Add(this.label5);
            this.guna2GroupBox1.Controls.Add(this.label20);
            this.guna2GroupBox1.Controls.Add(this.label27);
            this.guna2GroupBox1.Controls.Add(this.label28);
            this.guna2GroupBox1.CustomBorderColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.CustomBorderThickness = new System.Windows.Forms.Padding(0, 30, 0, 0);
            this.guna2GroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2GroupBox1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2GroupBox1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.guna2GroupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2GroupBox1.Location = new System.Drawing.Point(2, 142);
            this.guna2GroupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.guna2GroupBox1.Name = "guna2GroupBox1";
            this.guna2GroupBox1.Size = new System.Drawing.Size(1036, 156);
            this.guna2GroupBox1.TabIndex = 7;
            this.guna2GroupBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.guna2GroupBox1.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // guna2TextBox2
            // 
            this.guna2TextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2TextBox2.BackColor = System.Drawing.Color.Transparent;
            this.guna2TextBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2TextBox2.BorderRadius = 5;
            this.guna2TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox2.DefaultText = "";
            this.guna2TextBox2.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2TextBox2.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TextBox2.ForeColor = System.Drawing.Color.Black;
            this.guna2TextBox2.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Location = new System.Drawing.Point(921, 129);
            this.guna2TextBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBox2.Name = "guna2TextBox2";
            this.guna2TextBox2.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.guna2TextBox2.PlaceholderText = "P.O No";
            this.guna2TextBox2.SelectedText = "";
            this.guna2TextBox2.Size = new System.Drawing.Size(96, 20);
            this.guna2TextBox2.TabIndex = 23;
            // 
            // guna2TextBox1
            // 
            this.guna2TextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2TextBox1.BackColor = System.Drawing.Color.Transparent;
            this.guna2TextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.guna2TextBox1.BorderRadius = 5;
            this.guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox1.DefaultText = "";
            this.guna2TextBox1.FillColor = System.Drawing.Color.WhiteSmoke;
            this.guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TextBox1.ForeColor = System.Drawing.Color.Black;
            this.guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Location = new System.Drawing.Point(921, 96);
            this.guna2TextBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.guna2TextBox1.PlaceholderText = "P.O No";
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(95, 20);
            this.guna2TextBox1.TabIndex = 21;
            // 
            // txtSalesMan
            // 
            this.txtSalesMan.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSalesMan.BackColor = System.Drawing.Color.Transparent;
            this.txtSalesMan.BorderRadius = 5;
            this.txtSalesMan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSalesMan.DefaultText = "";
            this.txtSalesMan.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtSalesMan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSalesMan.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalesMan.ForeColor = System.Drawing.Color.Black;
            this.txtSalesMan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtSalesMan.Location = new System.Drawing.Point(444, 110);
            this.txtSalesMan.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtSalesMan.Name = "txtSalesMan";
            this.txtSalesMan.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtSalesMan.PlaceholderText = "Sales Man";
            this.txtSalesMan.SelectedText = "";
            this.txtSalesMan.Size = new System.Drawing.Size(191, 24);
            this.txtSalesMan.TabIndex = 4;
            // 
            // cmbPaymentTerms
            // 
            this.cmbPaymentTerms.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            this.cmbPaymentTerms.Location = new System.Drawing.Point(501, 69);
            this.cmbPaymentTerms.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPaymentTerms.Name = "cmbPaymentTerms";
            this.cmbPaymentTerms.Size = new System.Drawing.Size(123, 24);
            this.cmbPaymentTerms.TabIndex = 4;
            this.cmbPaymentTerms.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentTerms_SelectedIndexChanged);
            // 
            // cmbAccountCashName
            // 
            this.cmbAccountCashName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbAccountCashName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbAccountCashName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbAccountCashName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cmbAccountCashName.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccountCashName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbAccountCashName.ItemHeight = 12;
            this.cmbAccountCashName.Location = new System.Drawing.Point(641, 112);
            this.cmbAccountCashName.Margin = new System.Windows.Forms.Padding(2);
            this.cmbAccountCashName.Name = "cmbAccountCashName";
            this.cmbAccountCashName.Size = new System.Drawing.Size(255, 20);
            this.cmbAccountCashName.TabIndex = 4;
            this.cmbAccountCashName.SelectedIndexChanged += new System.EventHandler(this.cmbAccountCashName_SelectedIndexChanged);
            // 
            // dtShip
            // 
            this.dtShip.BackColor = System.Drawing.Color.Gainsboro;
            this.dtShip.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtShip.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtShip.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtShip.Location = new System.Drawing.Point(147, 112);
            this.dtShip.Margin = new System.Windows.Forms.Padding(2);
            this.dtShip.Name = "dtShip";
            this.dtShip.Size = new System.Drawing.Size(149, 19);
            this.dtShip.TabIndex = 6;
            // 
            // dtPaymentTerms
            // 
            this.dtPaymentTerms.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtPaymentTerms.BackColor = System.Drawing.Color.Gainsboro;
            this.dtPaymentTerms.Enabled = false;
            this.dtPaymentTerms.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPaymentTerms.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtPaymentTerms.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtPaymentTerms.Location = new System.Drawing.Point(638, 67);
            this.dtPaymentTerms.Margin = new System.Windows.Forms.Padding(2);
            this.dtPaymentTerms.Name = "dtPaymentTerms";
            this.dtPaymentTerms.Size = new System.Drawing.Size(124, 19);
            this.dtPaymentTerms.TabIndex = 6;
            // 
            // dtInv
            // 
            this.dtInv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtInv.Font = new System.Drawing.Font("Segoe UI Semibold", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtInv.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtInv.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtInv.Location = new System.Drawing.Point(560, 19);
            this.dtInv.Margin = new System.Windows.Forms.Padding(2);
            this.dtInv.Name = "dtInv";
            this.dtInv.Size = new System.Drawing.Size(129, 22);
            this.dtInv.TabIndex = 6;
            this.dtInv.ValueChanged += new System.EventHandler(this.dtInv_ValueChanged);
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
            this.cmbCity.Location = new System.Drawing.Point(145, 69);
            this.cmbCity.Margin = new System.Windows.Forms.Padding(2);
            this.cmbCity.Name = "cmbCity";
            this.cmbCity.Size = new System.Drawing.Size(155, 24);
            this.cmbCity.StartIndex = 0;
            this.cmbCity.TabIndex = 4;
            // 
            // cmbShipVia
            // 
            this.cmbShipVia.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            this.cmbShipVia.Location = new System.Drawing.Point(304, 110);
            this.cmbShipVia.Margin = new System.Windows.Forms.Padding(2);
            this.cmbShipVia.Name = "cmbShipVia";
            this.cmbShipVia.Size = new System.Drawing.Size(124, 24);
            this.cmbShipVia.TabIndex = 4;
            // 
            // cmbWarehouse
            // 
            this.cmbWarehouse.Anchor = System.Windows.Forms.AnchorStyles.Top;
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
            this.cmbWarehouse.Location = new System.Drawing.Point(304, 69);
            this.cmbWarehouse.Margin = new System.Windows.Forms.Padding(2);
            this.cmbWarehouse.Name = "cmbWarehouse";
            this.cmbWarehouse.Size = new System.Drawing.Size(170, 24);
            this.cmbWarehouse.TabIndex = 7;
            // 
            // txtNextCode
            // 
            this.txtNextCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNextCode.BackColor = System.Drawing.Color.Transparent;
            this.txtNextCode.BorderRadius = 5;
            this.txtNextCode.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNextCode.DefaultText = "";
            this.txtNextCode.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtNextCode.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNextCode.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNextCode.ForeColor = System.Drawing.Color.Black;
            this.txtNextCode.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtNextCode.Location = new System.Drawing.Point(921, 19);
            this.txtNextCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtNextCode.Name = "txtNextCode";
            this.txtNextCode.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtNextCode.PlaceholderText = "";
            this.txtNextCode.SelectedText = "";
            this.txtNextCode.Size = new System.Drawing.Size(100, 24);
            this.txtNextCode.TabIndex = 8;
            this.txtNextCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // txtPONO
            // 
            this.txtPONO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.txtPONO.Location = new System.Drawing.Point(921, 58);
            this.txtPONO.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtPONO.Name = "txtPONO";
            this.txtPONO.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtPONO.PlaceholderText = "P.O No";
            this.txtPONO.SelectedText = "";
            this.txtPONO.Size = new System.Drawing.Size(96, 20);
            this.txtPONO.TabIndex = 8;
            this.txtPONO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label18.Location = new System.Drawing.Point(145, 51);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(99, 16);
            this.label18.TabIndex = 9;
            this.label18.Text = "Emirates";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(639, 51);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(127, 16);
            this.label12.TabIndex = 10;
            this.label12.Text = "due to";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label8.Location = new System.Drawing.Point(301, 96);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 16);
            this.label8.TabIndex = 11;
            this.label8.Text = "Via";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label14.Location = new System.Drawing.Point(302, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(117, 16);
            this.label14.TabIndex = 12;
            this.label14.Text = "Warehouse";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label19.Location = new System.Drawing.Point(921, 0);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(96, 14);
            this.label19.TabIndex = 5;
            this.label19.Text = "Invoice #";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label11.Location = new System.Drawing.Point(560, -1);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 16);
            this.label11.TabIndex = 14;
            this.label11.Text = "Date";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.txtShipTo.Location = new System.Drawing.Point(3, 110);
            this.txtShipTo.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.txtShipTo.Name = "txtShipTo";
            this.txtShipTo.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtShipTo.PlaceholderText = "Ship To";
            this.txtShipTo.SelectedText = "";
            this.txtShipTo.Size = new System.Drawing.Size(122, 24);
            this.txtShipTo.TabIndex = 3;
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
            this.txtBillTo.Location = new System.Drawing.Point(2, 19);
            this.txtBillTo.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.txtBillTo.Name = "txtBillTo";
            this.txtBillTo.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtBillTo.PlaceholderText = "Bill To";
            this.txtBillTo.SelectedText = "";
            this.txtBillTo.Size = new System.Drawing.Size(368, 24);
            this.txtBillTo.TabIndex = 15;
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label15.Location = new System.Drawing.Point(442, 96);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 16);
            this.label15.TabIndex = 13;
            this.label15.Text = "Sales Man";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label9.Location = new System.Drawing.Point(499, 51);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(136, 16);
            this.label9.TabIndex = 5;
            this.label9.Text = "Payment Terms";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label10.Location = new System.Drawing.Point(641, 96);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(191, 16);
            this.label10.TabIndex = 5;
            this.label10.Text = "Account Name";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label6.Location = new System.Drawing.Point(1, 96);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 16);
            this.label6.TabIndex = 5;
            this.label6.Text = "Ship";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label4.Location = new System.Drawing.Point(0, -1);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(214, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Bill To";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label5.Location = new System.Drawing.Point(922, 40);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 22);
            this.label5.TabIndex = 19;
            this.label5.Text = "P.O NO";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label20
            // 
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label20.Location = new System.Drawing.Point(145, 96);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(58, 16);
            this.label20.TabIndex = 20;
            this.label20.Text = "Ship Date";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label27.Location = new System.Drawing.Point(923, 75);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(94, 29);
            this.label27.TabIndex = 22;
            this.label27.Text = "Performa Invoice NO.";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label28.BackColor = System.Drawing.Color.Transparent;
            this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label28.Location = new System.Drawing.Point(922, 108);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(94, 29);
            this.label28.TabIndex = 24;
            this.label28.Text = "Quotation No.";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtBarcodeScan
            // 
            this.txtBarcodeScan.BorderRadius = 6;
            this.txtBarcodeScan.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBarcodeScan.DefaultText = "";
            this.txtBarcodeScan.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtBarcodeScan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtBarcodeScan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBarcodeScan.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtBarcodeScan.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBarcodeScan.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBarcodeScan.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtBarcodeScan.Location = new System.Drawing.Point(5, 4);
            this.txtBarcodeScan.Name = "txtBarcodeScan";
            this.txtBarcodeScan.PlaceholderText = "Scan Barcode or Enter Code";
            this.txtBarcodeScan.SelectedText = "";
            this.txtBarcodeScan.Size = new System.Drawing.Size(167, 24);
            this.txtBarcodeScan.TabIndex = 26;
            this.txtBarcodeScan.Visible = false;
            this.txtBarcodeScan.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TxtBarcodeScan_KeyDown);
            // 
            // lnkNewCustomer
            // 
            this.lnkNewCustomer.ActiveLinkColor = System.Drawing.Color.Green;
            this.lnkNewCustomer.BackColor = System.Drawing.Color.Transparent;
            this.lnkNewCustomer.DisabledLinkColor = System.Drawing.Color.White;
            this.lnkNewCustomer.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkNewCustomer.ForeColor = System.Drawing.Color.Black;
            this.lnkNewCustomer.LinkColor = System.Drawing.Color.White;
            this.lnkNewCustomer.Location = new System.Drawing.Point(562, 8);
            this.lnkNewCustomer.Name = "lnkNewCustomer";
            this.lnkNewCustomer.Size = new System.Drawing.Size(50, 18);
            this.lnkNewCustomer.TabIndex = 0;
            this.lnkNewCustomer.TabStop = true;
            this.lnkNewCustomer.Text = "New";
            this.lnkNewCustomer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkNewCustomer.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkNewCustomer_LinkClicked);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 22);
            this.label2.TabIndex = 13;
            this.label2.Text = "Customer Code";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // guna2GroupBox3
            // 
            this.guna2GroupBox3.BackColor = System.Drawing.Color.WhiteSmoke;
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
            this.guna2GroupBox3.Location = new System.Drawing.Point(2, 502);
            this.guna2GroupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.guna2GroupBox3.Name = "guna2GroupBox3";
            this.guna2GroupBox3.Size = new System.Drawing.Size(1036, 103);
            this.guna2GroupBox3.TabIndex = 9;
            this.guna2GroupBox3.TextOffset = new System.Drawing.Point(0, -5);
            // 
            // richTextDescription
            // 
            this.richTextDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextDescription.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.richTextDescription.Location = new System.Drawing.Point(9, 12);
            this.richTextDescription.Name = "richTextDescription";
            this.richTextDescription.Size = new System.Drawing.Size(749, 85);
            this.richTextDescription.TabIndex = 7;
            this.richTextDescription.Text = "";
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.Transparent;
            this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label29.Location = new System.Drawing.Point(8, -9);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(214, 29);
            this.label29.TabIndex = 6;
            this.label29.Text = "Notes";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTotalVat
            // 
            this.txtTotalVat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.txtTotalVat.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.txtTotalVat.ForeColor = System.Drawing.Color.Black;
            this.txtTotalVat.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalVat.Location = new System.Drawing.Point(897, 37);
            this.txtTotalVat.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTotalVat.Name = "txtTotalVat";
            this.txtTotalVat.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotalVat.PlaceholderText = "Total Vat";
            this.txtTotalVat.ReadOnly = true;
            this.txtTotalVat.SelectedText = "";
            this.txtTotalVat.Size = new System.Drawing.Size(134, 29);
            this.txtTotalVat.TabIndex = 3;
            this.txtTotalVat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalVat.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // txtTotalBefore
            // 
            this.txtTotalBefore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.txtTotalBefore.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTotalBefore.ForeColor = System.Drawing.Color.Black;
            this.txtTotalBefore.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotalBefore.Location = new System.Drawing.Point(897, 6);
            this.txtTotalBefore.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTotalBefore.Name = "txtTotalBefore";
            this.txtTotalBefore.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotalBefore.PlaceholderText = "Total Before Vat";
            this.txtTotalBefore.ReadOnly = true;
            this.txtTotalBefore.SelectedText = "";
            this.txtTotalBefore.Size = new System.Drawing.Size(134, 29);
            this.txtTotalBefore.TabIndex = 3;
            this.txtTotalBefore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalBefore.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // txtTotal
            // 
            this.txtTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.txtTotal.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.txtTotal.ForeColor = System.Drawing.Color.Black;
            this.txtTotal.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTotal.Location = new System.Drawing.Point(897, 68);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txtTotal.PlaceholderText = "Net Amount";
            this.txtTotal.ReadOnly = true;
            this.txtTotal.SelectedText = "";
            this.txtTotal.Size = new System.Drawing.Size(134, 29);
            this.txtTotal.TabIndex = 3;
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Location = new System.Drawing.Point(763, 12);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 29);
            this.label1.TabIndex = 5;
            this.label1.Text = "Total Before Vat";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.label17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label17.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label17.Location = new System.Drawing.Point(763, 41);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(129, 29);
            this.label17.TabIndex = 5;
            this.label17.Text = "Total Vat";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label16.Location = new System.Drawing.Point(763, 68);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(129, 29);
            this.label16.TabIndex = 5;
            this.label16.Text = "Net Amount";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(2, 638);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1036, 2);
            this.panel5.TabIndex = 14;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1038, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 640);
            this.panel4.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.txtBarcodeScan);
            this.panel1.Controls.Add(this.guna2Button10);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 605);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1036, 33);
            this.panel1.TabIndex = 2;
            // 
            // guna2Button10
            // 
            this.guna2Button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button10.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.guna2Button10.BorderRadius = 5;
            this.guna2Button10.BorderThickness = 3;
            this.guna2Button10.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.guna2Button10.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button10.ForeColor = System.Drawing.Color.White;
            this.guna2Button10.Location = new System.Drawing.Point(771, 3);
            this.guna2Button10.Name = "guna2Button10";
            this.guna2Button10.Size = new System.Drawing.Size(100, 27);
            this.guna2Button10.TabIndex = 3;
            this.guna2Button10.Text = "Save && Close";
            this.guna2Button10.Click += new System.EventHandler(this.guna2Button10_Click);
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(128)))), ((int)(((byte)(160)))));
            this.panel7.Controls.Add(this.cmbCustomer);
            this.panel7.Controls.Add(this.lnkNewCustomer);
            this.panel7.Controls.Add(this.txtCustomerCode);
            this.panel7.Controls.Add(this.label2);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(2, 107);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1036, 35);
            this.panel7.TabIndex = 17;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BorderColor = System.Drawing.Color.Silver;
            this.guna2Panel1.BorderThickness = 1;
            this.guna2Panel1.Controls.Add(this.guna2TileButton31);
            this.guna2Panel1.Controls.Add(this.guna2TileButton30);
            this.guna2Panel1.CustomBorderThickness = new System.Windows.Forms.Padding(1, 0, 1, 1);
            this.guna2Panel1.Location = new System.Drawing.Point(423, 96);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(111, 44);
            this.guna2Panel1.TabIndex = 21;
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
            this.guna2TileButton31.Location = new System.Drawing.Point(0, 23);
            this.guna2TileButton31.Name = "guna2TileButton31";
            this.guna2TileButton31.Size = new System.Drawing.Size(111, 23);
            this.guna2TileButton31.TabIndex = 34;
            this.guna2TileButton31.Text = "Delivery Report";
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
            this.guna2TileButton30.Name = "guna2TileButton30";
            this.guna2TileButton30.Size = new System.Drawing.Size(111, 23);
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
            // panel9
            // 
            this.panel9.Controls.Add(this.tabControl2);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel9.Location = new System.Drawing.Point(2, 23);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1036, 84);
            this.panel9.TabIndex = 38;
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
            this.tabControl2.Size = new System.Drawing.Size(1036, 84);
            this.tabControl2.TabIndex = 39;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPage3.Controls.Add(this.txtId);
            this.tabPage3.Controls.Add(this.guna2VSeparator7);
            this.tabPage3.Controls.Add(this.guna2TileButton12);
            this.tabPage3.Controls.Add(this.guna2Button3);
            this.tabPage3.Controls.Add(this.guna2TileButton13);
            this.tabPage3.Controls.Add(this.guna2Button2);
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
            this.tabPage3.Controls.Add(this.guna2VSeparator11);
            this.tabPage3.Controls.Add(this.chkPrint);
            this.tabPage3.Controls.Add(this.guna2TileButton18);
            this.tabPage3.Controls.Add(this.guna2TileButton17);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1028, 58);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Main";
            // 
            // txtId
            // 
            this.txtId.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.txtId.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtId.Enabled = false;
            this.txtId.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtId.Location = new System.Drawing.Point(33, 21);
            this.txtId.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtId.Multiline = true;
            this.txtId.Name = "txtId";
            this.txtId.ReadOnly = true;
            this.txtId.Size = new System.Drawing.Size(66, 18);
            this.txtId.TabIndex = 38;
            this.txtId.Text = "0";
            this.txtId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // guna2VSeparator7
            // 
            this.guna2VSeparator7.Location = new System.Drawing.Point(1023, 6);
            this.guna2VSeparator7.Name = "guna2VSeparator7";
            this.guna2VSeparator7.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator7.TabIndex = 37;
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
            this.guna2TileButton12.Location = new System.Drawing.Point(903, 29);
            this.guna2TileButton12.Name = "guna2TileButton12";
            this.guna2TileButton12.Size = new System.Drawing.Size(114, 23);
            this.guna2TileButton12.TabIndex = 36;
            this.guna2TileButton12.Text = "Refund/ Payment";
            this.guna2TileButton12.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton12.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton12.Visible = false;
            // 
            // guna2Button3
            // 
            this.guna2Button3.BackColor = System.Drawing.Color.Transparent;
            this.guna2Button3.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button3.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button3.ForeColor = System.Drawing.Color.White;
            this.guna2Button3.Image = global::YamyProject.Properties.Resources.Arrow22;
            this.guna2Button3.ImageSize = new System.Drawing.Size(25, 25);
            this.guna2Button3.Location = new System.Drawing.Point(99, 15);
            this.guna2Button3.Margin = new System.Windows.Forms.Padding(2);
            this.guna2Button3.Name = "guna2Button3";
            this.guna2Button3.Size = new System.Drawing.Size(32, 29);
            this.guna2Button3.TabIndex = 2;
            this.guna2Button3.Click += new System.EventHandler(this.btnNext_Click);
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
            this.guna2TileButton13.Location = new System.Drawing.Point(840, 7);
            this.guna2TileButton13.Name = "guna2TileButton13";
            this.guna2TileButton13.Size = new System.Drawing.Size(69, 47);
            this.guna2TileButton13.TabIndex = 35;
            this.guna2TileButton13.Text = "Receive Payments";
            this.guna2TileButton13.Visible = false;
            // 
            // guna2Button2
            // 
            this.guna2Button2.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Image = global::YamyProject.Properties.Resources.Arrow_Pointing_Left;
            this.guna2Button2.Location = new System.Drawing.Point(3, 16);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(30, 28);
            this.guna2Button2.TabIndex = 3;
            this.guna2Button2.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // guna2VSeparator8
            // 
            this.guna2VSeparator8.Location = new System.Drawing.Point(832, 7);
            this.guna2VSeparator8.Name = "guna2VSeparator8";
            this.guna2VSeparator8.Size = new System.Drawing.Size(8, 46);
            this.guna2VSeparator8.TabIndex = 34;
            // 
            // guna2VSeparator12
            // 
            this.guna2VSeparator12.Location = new System.Drawing.Point(131, 6);
            this.guna2VSeparator12.Name = "guna2VSeparator12";
            this.guna2VSeparator12.Size = new System.Drawing.Size(8, 46);
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
            this.guna2TileButton14.Location = new System.Drawing.Point(734, 31);
            this.guna2TileButton14.Name = "guna2TileButton14";
            this.guna2TileButton14.Size = new System.Drawing.Size(100, 23);
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
            this.guna2TileButton23.Location = new System.Drawing.Point(139, 6);
            this.guna2TileButton23.Name = "guna2TileButton23";
            this.guna2TileButton23.Size = new System.Drawing.Size(54, 47);
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
            this.guna2TileButton15.Location = new System.Drawing.Point(732, 7);
            this.guna2TileButton15.Name = "guna2TileButton15";
            this.guna2TileButton15.Size = new System.Drawing.Size(100, 23);
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
            this.guna2TileButton22.Location = new System.Drawing.Point(193, 6);
            this.guna2TileButton22.Name = "guna2TileButton22";
            this.guna2TileButton22.Size = new System.Drawing.Size(54, 47);
            this.guna2TileButton22.TabIndex = 20;
            this.guna2TileButton22.Text = "Save";
            this.guna2TileButton22.Click += new System.EventHandler(this.guna2TileButton22_Click);
            // 
            // guna2VSeparator9
            // 
            this.guna2VSeparator9.Location = new System.Drawing.Point(724, 6);
            this.guna2VSeparator9.Name = "guna2VSeparator9";
            this.guna2VSeparator9.Size = new System.Drawing.Size(8, 46);
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
            this.guna2TileButton21.Location = new System.Drawing.Point(247, 6);
            this.guna2TileButton21.Name = "guna2TileButton21";
            this.guna2TileButton21.Size = new System.Drawing.Size(54, 47);
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
            this.guna2TileButton16.Location = new System.Drawing.Point(670, 7);
            this.guna2TileButton16.Name = "guna2TileButton16";
            this.guna2TileButton16.Size = new System.Drawing.Size(54, 47);
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
            this.guna2TileButton20.Location = new System.Drawing.Point(301, 8);
            this.guna2TileButton20.Name = "guna2TileButton20";
            this.guna2TileButton20.Size = new System.Drawing.Size(100, 23);
            this.guna2TileButton20.TabIndex = 22;
            this.guna2TileButton20.Text = "Create a Copy";
            this.guna2TileButton20.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.guna2TileButton20.TextOffset = new System.Drawing.Point(10, -12);
            this.guna2TileButton20.Click += new System.EventHandler(this.guna2TileButton20_Click);
            // 
            // guna2VSeparator10
            // 
            this.guna2VSeparator10.Location = new System.Drawing.Point(662, 6);
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
            this.chkPrint.Checked = true;
            this.chkPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrint.Font = new System.Drawing.Font("Segoe UI Semibold", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPrint.Location = new System.Drawing.Point(522, 36);
            this.chkPrint.Margin = new System.Windows.Forms.Padding(2);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Size = new System.Drawing.Size(133, 16);
            this.chkPrint.TabIndex = 27;
            this.chkPrint.Text = "Print Tax Invoice After Save";
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
            this.guna2TileButton18.Location = new System.Drawing.Point(409, 8);
            this.guna2TileButton18.Name = "guna2TileButton18";
            this.guna2TileButton18.Size = new System.Drawing.Size(54, 47);
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
            this.guna2TileButton17.Location = new System.Drawing.Point(463, 8);
            this.guna2TileButton17.Name = "guna2TileButton17";
            this.guna2TileButton17.Size = new System.Drawing.Size(54, 47);
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
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1028, 58);
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
            this.guna2TileButton24.Click += new System.EventHandler(this.guna2TileButton24_Click);
            // 
            // guna2ControlBox1
            // 
            this.guna2ControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ControlBox1.FillColor = System.Drawing.Color.Transparent;
            this.guna2ControlBox1.IconColor = System.Drawing.Color.White;
            this.guna2ControlBox1.Location = new System.Drawing.Point(1018, 5);
            this.guna2ControlBox1.Name = "guna2ControlBox1";
            this.guna2ControlBox1.Size = new System.Drawing.Size(15, 15);
            this.guna2ControlBox1.TabIndex = 0;
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
            this.headerUC1.Size = new System.Drawing.Size(595, 37);
            this.headerUC1.TabIndex = 0;
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
            this.guna2Button1.Location = new System.Drawing.Point(976, 5);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(15, 15);
            this.guna2Button1.TabIndex = 28;
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click_1);
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
            this.guna2Button4.Location = new System.Drawing.Point(997, 5);
            this.guna2Button4.Name = "guna2Button4";
            this.guna2Button4.Size = new System.Drawing.Size(15, 15);
            this.guna2Button4.TabIndex = 27;
            this.guna2Button4.Click += new System.EventHandler(this.guna2Button4_Click);
            // 
            // guna2ResizeForm1
            // 
            this.guna2ResizeForm1.TargetForm = this;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 640);
            this.panel3.TabIndex = 12;
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
            this.lstAccountSuggestions.Location = new System.Drawing.Point(366, 297);
            this.lstAccountSuggestions.Name = "lstAccountSuggestions";
            this.lstAccountSuggestions.Size = new System.Drawing.Size(308, 43);
            this.lstAccountSuggestions.TabIndex = 42;
            this.lstAccountSuggestions.TabStop = false;
            this.lstAccountSuggestions.Visible = false;
            this.lstAccountSuggestions.Click += new System.EventHandler(this.lstAccountSuggestions_Click);
            // 
            // frmSales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1040, 640);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.guna2GroupBox4);
            this.Controls.Add(this.guna2GroupBox3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.guna2GroupBox1);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lstAccountSuggestions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmSales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Tax Invoice";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSales_FormClosing);
            this.Load += new System.EventHandler(this.frmSales_Load);
            this.guna2GroupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.guna2GroupBox1.ResumeLayout(false);
            this.guna2GroupBox3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2Button btnClose;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox4;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPaymentMethod;
        private Guna.UI2.WinForms.Guna2TextBox txtCustomerCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox1;
        private Guna.UI2.WinForms.Guna2TextBox txtBillTo;
        private Guna.UI2.WinForms.Guna2GroupBox guna2GroupBox3;
        private System.Windows.Forms.DateTimePicker dtInv;
        private Guna.UI2.WinForms.Guna2TextBox txtPONO;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbAccountCashName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker dtShip;
        private System.Windows.Forms.DateTimePicker dtPaymentTerms;
        private Guna.UI2.WinForms.Guna2ComboBox cmbShipVia;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox txtShipTo;
        private Guna.UI2.WinForms.Guna2DataGridView dgvItems;
        private Guna.UI2.WinForms.Guna2TextBox txtTotal;
        private Guna.UI2.WinForms.Guna2ComboBox cmbPaymentTerms;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2ComboBox cmbWarehouse;
        private System.Windows.Forms.Label label14;
        private Guna.UI2.WinForms.Guna2TextBox txtSalesMan;
        private Guna.UI2.WinForms.Guna2ComboBox cmbCity;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private Guna.UI2.WinForms.Guna2TextBox txtTotalVat;
        private Guna.UI2.WinForms.Guna2TextBox txtTotalBefore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.LinkLabel lnkNewCustomer;
        public System.Windows.Forms.ComboBox cmbCustomer;
        private Guna.UI2.WinForms.Guna2TextBox txtNextCode;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton31;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton30;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label20;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private System.Windows.Forms.Label label27;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox2;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.RichTextBox richTextDescription;
        private Guna.UI2.WinForms.Guna2Button guna2Button10;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private Guna.UI2.WinForms.Guna2VSeparator guna2VSeparator7;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton12;
        private Guna.UI2.WinForms.Guna2Button guna2Button3;
        private Guna.UI2.WinForms.Guna2TileButton guna2TileButton13;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
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
        private Guna.UI2.WinForms.Guna2ControlBox guna2ControlBox1;
        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2ResizeForm guna2ResizeForm1;
        private System.Windows.Forms.Panel panel3;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button4;
        private Guna.UI2.WinForms.Guna2DragControl guna2DragControl1;
        private System.Windows.Forms.ListBox lstAccountSuggestions;
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
        private TextBox txtId;
    }
}