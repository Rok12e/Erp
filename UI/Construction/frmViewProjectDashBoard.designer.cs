namespace YamyProject
{
    partial class frmViewProjectDashBoard
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
            this.lblTotalProjects = new System.Windows.Forms.Label();
            this.lblTotalEstimates = new System.Windows.Forms.Label();
            this.dgvProjects = new System.Windows.Forms.DataGridView();
            this.dgvProjectTenders = new System.Windows.Forms.DataGridView();
            this.projectStatusChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblTotalTenders = new System.Windows.Forms.Label();
            this.cmbProjectName = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkProject = new Guna.UI2.WinForms.Guna2CheckBox();
            this.headerUC1 = new YamyProject.HeaderUC();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjectTenders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectStatusChart)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTotalProjects
            // 
            this.lblTotalProjects.AutoSize = true;
            this.lblTotalProjects.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalProjects.Location = new System.Drawing.Point(20, 60);
            this.lblTotalProjects.Name = "lblTotalProjects";
            this.lblTotalProjects.Size = new System.Drawing.Size(155, 22);
            this.lblTotalProjects.TabIndex = 0;
            this.lblTotalProjects.Text = "Total Project : 0";
            // 
            // lblTotalEstimates
            // 
            this.lblTotalEstimates.AutoSize = true;
            this.lblTotalEstimates.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalEstimates.Location = new System.Drawing.Point(568, 60);
            this.lblTotalEstimates.Name = "lblTotalEstimates";
            this.lblTotalEstimates.Size = new System.Drawing.Size(180, 22);
            this.lblTotalEstimates.TabIndex = 1;
            this.lblTotalEstimates.Text = "Total Estimates : 0";
            // 
            // dgvProjects
            // 
            this.dgvProjects.AllowUserToAddRows = false;
            this.dgvProjects.AllowUserToDeleteRows = false;
            this.dgvProjects.BackgroundColor = System.Drawing.Color.White;
            this.dgvProjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjects.Location = new System.Drawing.Point(10, 125);
            this.dgvProjects.Name = "dgvProjects";
            this.dgvProjects.ReadOnly = true;
            this.dgvProjects.RowHeadersVisible = false;
            this.dgvProjects.Size = new System.Drawing.Size(941, 338);
            this.dgvProjects.TabIndex = 2;
            // 
            // dgvProjectTenders
            // 
            this.dgvProjectTenders.AllowUserToAddRows = false;
            this.dgvProjectTenders.AllowUserToDeleteRows = false;
            this.dgvProjectTenders.BackgroundColor = System.Drawing.Color.White;
            this.dgvProjectTenders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProjectTenders.Location = new System.Drawing.Point(10, 481);
            this.dgvProjectTenders.Name = "dgvProjectTenders";
            this.dgvProjectTenders.ReadOnly = true;
            this.dgvProjectTenders.RowHeadersVisible = false;
            this.dgvProjectTenders.Size = new System.Drawing.Size(941, 310);
            this.dgvProjectTenders.TabIndex = 3;
            // 
            // projectStatusChart
            // 
            this.projectStatusChart.BorderlineColor = System.Drawing.Color.Black;
            this.projectStatusChart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.projectStatusChart.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Raised;
            this.projectStatusChart.Location = new System.Drawing.Point(990, 125);
            this.projectStatusChart.Name = "projectStatusChart";
            this.projectStatusChart.Size = new System.Drawing.Size(443, 376);
            this.projectStatusChart.TabIndex = 4;
            // 
            // lblTotalTenders
            // 
            this.lblTotalTenders.AutoSize = true;
            this.lblTotalTenders.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotalTenders.Location = new System.Drawing.Point(309, 60);
            this.lblTotalTenders.Name = "lblTotalTenders";
            this.lblTotalTenders.Size = new System.Drawing.Size(166, 22);
            this.lblTotalTenders.TabIndex = 5;
            this.lblTotalTenders.Text = "Total Tenders : 0";
            // 
            // cmbProjectName
            // 
            this.cmbProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProjectName.BackColor = System.Drawing.Color.Transparent;
            this.cmbProjectName.BorderRadius = 5;
            this.cmbProjectName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbProjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProjectName.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProjectName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cmbProjectName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbProjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbProjectName.ItemHeight = 18;
            this.cmbProjectName.Location = new System.Drawing.Point(1008, 60);
            this.cmbProjectName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbProjectName.Name = "cmbProjectName";
            this.cmbProjectName.Size = new System.Drawing.Size(278, 24);
            this.cmbProjectName.TabIndex = 17;
            this.cmbProjectName.SelectedIndexChanged += new System.EventHandler(this.cmbProjectName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(51)))), ((int)(((byte)(67)))));
            this.label1.Location = new System.Drawing.Point(942, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 29);
            this.label1.TabIndex = 18;
            this.label1.Text = "Project";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkProject
            // 
            this.chkProject.AutoSize = true;
            this.chkProject.BackColor = System.Drawing.Color.Transparent;
            this.chkProject.Checked = true;
            this.chkProject.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkProject.CheckedState.BorderRadius = 0;
            this.chkProject.CheckedState.BorderThickness = 0;
            this.chkProject.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkProject.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkProject.ForeColor = System.Drawing.Color.Black;
            this.chkProject.Location = new System.Drawing.Point(1291, 64);
            this.chkProject.Name = "chkProject";
            this.chkProject.Size = new System.Drawing.Size(37, 17);
            this.chkProject.TabIndex = 19;
            this.chkProject.Text = "All";
            this.chkProject.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkProject.UncheckedState.BorderRadius = 0;
            this.chkProject.UncheckedState.BorderThickness = 0;
            this.chkProject.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkProject.UseVisualStyleBackColor = false;
            this.chkProject.CheckedChanged += new System.EventHandler(this.chkProject_CheckedChanged);
            // 
            // headerUC1
            // 
            this.headerUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(41)))), ((int)(((byte)(83)))));
            this.headerUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerUC1.FormText = "";
            this.headerUC1.HeaderText = "";
            this.headerUC1.Location = new System.Drawing.Point(0, 0);
            this.headerUC1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.headerUC1.Name = "headerUC1";
            this.headerUC1.Size = new System.Drawing.Size(1456, 46);
            this.headerUC1.TabIndex = 0;
            // 
            // frmViewProjectDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1456, 847);
            this.Controls.Add(this.chkProject);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbProjectName);
            this.Controls.Add(this.lblTotalTenders);
            this.Controls.Add(this.headerUC1);
            this.Controls.Add(this.projectStatusChart);
            this.Controls.Add(this.dgvProjectTenders);
            this.Controls.Add(this.dgvProjects);
            this.Controls.Add(this.lblTotalEstimates);
            this.Controls.Add(this.lblTotalProjects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmViewProjectDashBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Management Dashboard";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmViewProjectDashBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjects)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProjectTenders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.projectStatusChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private HeaderUC headerUC1;
        private System.Windows.Forms.Label lblTotalProjects;
        private System.Windows.Forms.Label lblTotalEstimates;
        private System.Windows.Forms.DataGridView dgvProjects;
        private System.Windows.Forms.DataGridView dgvProjectTenders;
        private System.Windows.Forms.DataVisualization.Charting.Chart projectStatusChart;
        private System.Windows.Forms.Label lblTotalTenders;
        private Guna.UI2.WinForms.Guna2ComboBox cmbProjectName;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2CheckBox chkProject;
    }
}