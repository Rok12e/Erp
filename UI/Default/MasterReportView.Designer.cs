namespace YamyProject
{
    partial class MasterReportView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterReportView));
            this.crReportViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.SuspendLayout();
            // 
            // crReportViewer
            // 
            this.crReportViewer.ActiveViewIndex = -1;
            this.crReportViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crReportViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.crReportViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crReportViewer.Location = new System.Drawing.Point(0, 0);
            this.crReportViewer.Margin = new System.Windows.Forms.Padding(2);
            this.crReportViewer.Name = "crReportViewer";
            this.crReportViewer.Size = new System.Drawing.Size(1040, 628);
            this.crReportViewer.TabIndex = 0;
            this.crReportViewer.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crReportViewer.ToolPanelWidth = 150;
            // 
            // MasterReportView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 628);
            this.Controls.Add(this.crReportViewer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MasterReportView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MasterReportView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MasterReportView_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public CrystalDecisions.Windows.Forms.CrystalReportViewer crReportViewer;
    }
}