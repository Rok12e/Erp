namespace YamyProject
{
    partial class frmProjectPlanning
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.budgetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timelineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabProjects = new System.Windows.Forms.TabPage();
            this.btnAddTender = new Guna.UI2.WinForms.Guna2Button();
            this.cmbProjectSite = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dgvItems = new Guna.UI2.WinForms.Guna2DataGridView();
            this.cmbTenderName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProjectName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.tabActivity = new System.Windows.Forms.TabPage();
            this.panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.chkDate = new Guna.UI2.WinForms.Guna2CheckBox();
            this.tabResources = new System.Windows.Forms.TabPage();
            this.tabBudget = new System.Windows.Forms.TabPage();
            this.tabReports = new System.Windows.Forms.TabPage();
            this.tabTimeline = new System.Windows.Forms.TabPage();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabProjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).BeginInit();
            this.tabActivity.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectToolStripMenuItem,
            this.activityToolStripMenuItem,
            this.materialToolStripMenuItem,
            this.budgetToolStripMenuItem,
            this.reportsToolStripMenuItem,
            this.timelineToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1354, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "Project";
            this.projectToolStripMenuItem.Click += new System.EventHandler(this.projectToolStripMenuItem_Click);
            // 
            // activityToolStripMenuItem
            // 
            this.activityToolStripMenuItem.Name = "activityToolStripMenuItem";
            this.activityToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.activityToolStripMenuItem.Text = "Activity";
            this.activityToolStripMenuItem.Click += new System.EventHandler(this.TasksToolStripMenuItem_Click);
            // 
            // materialToolStripMenuItem
            // 
            this.materialToolStripMenuItem.Name = "materialToolStripMenuItem";
            this.materialToolStripMenuItem.Size = new System.Drawing.Size(107, 20);
            this.materialToolStripMenuItem.Text = "Request Material";
            this.materialToolStripMenuItem.Click += new System.EventHandler(this.ResourcesToolStripMenuItem_Click);
            // 
            // budgetToolStripMenuItem
            // 
            this.budgetToolStripMenuItem.Name = "budgetToolStripMenuItem";
            this.budgetToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.budgetToolStripMenuItem.Text = "Received Material";
            this.budgetToolStripMenuItem.Click += new System.EventHandler(this.BudgetToolStripMenuItem_Click);
            // 
            // reportsToolStripMenuItem
            // 
            this.reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
            this.reportsToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.reportsToolStripMenuItem.Text = "Issue Material";
            this.reportsToolStripMenuItem.Click += new System.EventHandler(this.ReportsToolStripMenuItem_Click);
            // 
            // timelineToolStripMenuItem
            // 
            this.timelineToolStripMenuItem.Name = "timelineToolStripMenuItem";
            this.timelineToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.timelineToolStripMenuItem.Text = "Timeline";
            this.timelineToolStripMenuItem.Click += new System.EventHandler(this.TimelineToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabProjects);
            this.tabControl1.Controls.Add(this.tabActivity);
            this.tabControl1.Controls.Add(this.tabResources);
            this.tabControl1.Controls.Add(this.tabBudget);
            this.tabControl1.Controls.Add(this.tabReports);
            this.tabControl1.Controls.Add(this.tabTimeline);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1354, 537);
            this.tabControl1.TabIndex = 3;
            // 
            // tabProjects
            // 
            this.tabProjects.Controls.Add(this.btnAddTender);
            this.tabProjects.Controls.Add(this.cmbProjectSite);
            this.tabProjects.Controls.Add(this.label12);
            this.tabProjects.Controls.Add(this.dgvItems);
            this.tabProjects.Controls.Add(this.cmbTenderName);
            this.tabProjects.Controls.Add(this.label16);
            this.tabProjects.Controls.Add(this.label1);
            this.tabProjects.Controls.Add(this.cmbProjectName);
            this.tabProjects.Location = new System.Drawing.Point(4, 22);
            this.tabProjects.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabProjects.Name = "tabProjects";
            this.tabProjects.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabProjects.Size = new System.Drawing.Size(1346, 511);
            this.tabProjects.TabIndex = 0;
            this.tabProjects.Text = "Projects";
            this.tabProjects.UseVisualStyleBackColor = true;
            // 
            // btnAddTender
            // 
            this.btnAddTender.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAddTender.BorderRadius = 5;
            this.btnAddTender.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAddTender.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAddTender.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAddTender.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAddTender.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAddTender.ForeColor = System.Drawing.Color.White;
            this.btnAddTender.Location = new System.Drawing.Point(841, 47);
            this.btnAddTender.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnAddTender.Name = "btnAddTender";
            this.btnAddTender.Size = new System.Drawing.Size(35, 24);
            this.btnAddTender.TabIndex = 129;
            this.btnAddTender.Text = "+";
            this.btnAddTender.Click += new System.EventHandler(this.btnAddTender_Click);
            // 
            // cmbProjectSite
            // 
            this.cmbProjectSite.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbProjectSite.BackColor = System.Drawing.Color.Transparent;
            this.cmbProjectSite.BorderRadius = 5;
            this.cmbProjectSite.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProjectSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjectSite.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProjectSite.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProjectSite.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProjectSite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbProjectSite.ItemHeight = 18;
            this.cmbProjectSite.Location = new System.Drawing.Point(547, 47);
            this.cmbProjectSite.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbProjectSite.Name = "cmbProjectSite";
            this.cmbProjectSite.Size = new System.Drawing.Size(290, 24);
            this.cmbProjectSite.TabIndex = 128;
            this.cmbProjectSite.SelectedIndexChanged += new System.EventHandler(this.cmbProjectSite_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label12.Location = new System.Drawing.Point(544, 14);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 29);
            this.label12.TabIndex = 127;
            this.label12.Text = "Project Site";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvItems
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvItems.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItems.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItems.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItems.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.Location = new System.Drawing.Point(15, 92);
            this.dgvItems.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvItems.Name = "dgvItems";
            this.dgvItems.RowHeadersVisible = false;
            this.dgvItems.RowTemplate.Height = 24;
            this.dgvItems.Size = new System.Drawing.Size(642, 405);
            this.dgvItems.TabIndex = 126;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvItems.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvItems.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvItems.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItems.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItems.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvItems.ThemeStyle.ReadOnly = false;
            this.dgvItems.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItems.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvItems.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItems.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItems.ThemeStyle.RowsStyle.Height = 24;
            this.dgvItems.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItems.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // cmbTenderName
            // 
            this.cmbTenderName.BackColor = System.Drawing.Color.Transparent;
            this.cmbTenderName.BorderRadius = 5;
            this.cmbTenderName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTenderName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTenderName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbTenderName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbTenderName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTenderName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbTenderName.ItemHeight = 18;
            this.cmbTenderName.Location = new System.Drawing.Point(16, 46);
            this.cmbTenderName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbTenderName.Name = "cmbTenderName";
            this.cmbTenderName.Size = new System.Drawing.Size(213, 24);
            this.cmbTenderName.TabIndex = 49;
            this.cmbTenderName.SelectedIndexChanged += new System.EventHandler(this.cmbTenderName_SelectedIndexChanged);
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label16.Location = new System.Drawing.Point(13, 14);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(98, 29);
            this.label16.TabIndex = 48;
            this.label16.Text = "Tender Name";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(242, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 29);
            this.label1.TabIndex = 19;
            this.label1.Text = "Project Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbProjectName
            // 
            this.cmbProjectName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmbProjectName.BackColor = System.Drawing.Color.Transparent;
            this.cmbProjectName.BorderRadius = 5;
            this.cmbProjectName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjectName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProjectName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProjectName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbProjectName.ItemHeight = 18;
            this.cmbProjectName.Location = new System.Drawing.Point(244, 47);
            this.cmbProjectName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbProjectName.Name = "cmbProjectName";
            this.cmbProjectName.Size = new System.Drawing.Size(290, 24);
            this.cmbProjectName.TabIndex = 18;
            this.cmbProjectName.SelectedIndexChanged += new System.EventHandler(this.cmbProjectName_SelectedIndexChanged);
            // 
            // tabActivity
            // 
            this.tabActivity.Controls.Add(this.panel1);
            this.tabActivity.Location = new System.Drawing.Point(4, 22);
            this.tabActivity.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabActivity.Name = "tabActivity";
            this.tabActivity.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabActivity.Size = new System.Drawing.Size(1346, 511);
            this.tabActivity.TabIndex = 0;
            this.tabActivity.Text = "Activity";
            this.tabActivity.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1342, 507);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 51);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1342, 456);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dtTo);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.dtFrom);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.chkDate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1342, 51);
            this.panel2.TabIndex = 0;
            // 
            // dtTo
            // 
            this.dtTo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtTo.BackColor = System.Drawing.Color.Gainsboro;
            this.dtTo.Enabled = false;
            this.dtTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(657, 13);
            this.dtTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(161, 23);
            this.dtTo.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label2.Location = new System.Drawing.Point(630, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "To";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtFrom
            // 
            this.dtFrom.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.dtFrom.BackColor = System.Drawing.Color.Gainsboro;
            this.dtFrom.Enabled = false;
            this.dtFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(434, 13);
            this.dtFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(161, 23);
            this.dtFrom.TabIndex = 16;
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label11.Location = new System.Drawing.Point(392, 16);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(36, 20);
            this.label11.TabIndex = 14;
            this.label11.Text = "From";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkDate
            // 
            this.chkDate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.chkDate.AutoSize = true;
            this.chkDate.BackColor = System.Drawing.Color.Transparent;
            this.chkDate.Checked = true;
            this.chkDate.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkDate.CheckedState.BorderRadius = 0;
            this.chkDate.CheckedState.BorderThickness = 0;
            this.chkDate.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDate.ForeColor = System.Drawing.Color.Black;
            this.chkDate.Location = new System.Drawing.Point(822, 20);
            this.chkDate.Name = "chkDate";
            this.chkDate.Size = new System.Drawing.Size(37, 17);
            this.chkDate.TabIndex = 12;
            this.chkDate.Text = "All";
            this.chkDate.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkDate.UncheckedState.BorderRadius = 0;
            this.chkDate.UncheckedState.BorderThickness = 0;
            this.chkDate.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkDate.UseVisualStyleBackColor = false;
            // 
            // tabResources
            // 
            this.tabResources.Location = new System.Drawing.Point(4, 22);
            this.tabResources.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabResources.Name = "tabResources";
            this.tabResources.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabResources.Size = new System.Drawing.Size(1346, 511);
            this.tabResources.TabIndex = 1;
            this.tabResources.Text = "Resources";
            this.tabResources.UseVisualStyleBackColor = true;
            // 
            // tabBudget
            // 
            this.tabBudget.Location = new System.Drawing.Point(4, 22);
            this.tabBudget.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabBudget.Name = "tabBudget";
            this.tabBudget.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabBudget.Size = new System.Drawing.Size(1346, 511);
            this.tabBudget.TabIndex = 2;
            this.tabBudget.Text = "Budget";
            this.tabBudget.UseVisualStyleBackColor = true;
            // 
            // tabReports
            // 
            this.tabReports.Location = new System.Drawing.Point(4, 22);
            this.tabReports.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabReports.Size = new System.Drawing.Size(1346, 511);
            this.tabReports.TabIndex = 3;
            this.tabReports.Text = "Reports";
            this.tabReports.UseVisualStyleBackColor = true;
            // 
            // tabTimeline
            // 
            this.tabTimeline.Location = new System.Drawing.Point(4, 22);
            this.tabTimeline.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabTimeline.Name = "tabTimeline";
            this.tabTimeline.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabTimeline.Size = new System.Drawing.Size(1346, 511);
            this.tabTimeline.TabIndex = 4;
            this.tabTimeline.Text = "Timeline";
            this.tabTimeline.UseVisualStyleBackColor = true;
            // 
            // frmProjectPlanning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1354, 537);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "frmProjectPlanning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "main";
            this.Text = "Project Planning";
            this.Load += new System.EventHandler(this.frmProjectPlanning_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabProjects.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvItems)).EndInit();
            this.tabActivity.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem materialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem budgetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timelineToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabProjects;
        private System.Windows.Forms.TabPage tabActivity;
        private System.Windows.Forms.TabPage tabResources;
        private System.Windows.Forms.TabPage tabBudget;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.TabPage tabTimeline;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2ComboBox cmbProjectName;
        public Guna.UI2.WinForms.Guna2ComboBox cmbTenderName;
        private System.Windows.Forms.Label label16;
        private Guna.UI2.WinForms.Guna2DataGridView dgvItems;
        private Guna.UI2.WinForms.Guna2ComboBox cmbProjectSite;
        private System.Windows.Forms.Label label12;
        private Guna.UI2.WinForms.Guna2Button btnAddTender;
        private Guna.UI2.WinForms.Guna2Panel panel1;
        private Guna.UI2.WinForms.Guna2Panel panel3;
        private Guna.UI2.WinForms.Guna2Panel panel2;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2CheckBox chkDate;
    }

}