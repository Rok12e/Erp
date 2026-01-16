namespace YamyProject
{
    partial class frmDataHandler
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
            this.headerUC1 = new YamyProject.HeaderUC();
            this.btnClear = new Guna.UI2.WinForms.Guna2Button();
            this.ChkBoxTables = new System.Windows.Forms.CheckedListBox();
            this.ChkAll = new System.Windows.Forms.CheckBox();
            this.ChkLevel2 = new System.Windows.Forms.CheckBox();
            this.ChkLevel1 = new System.Windows.Forms.CheckBox();
            this.ChkLevel3 = new System.Windows.Forms.CheckBox();
            this.ChkLevel4 = new System.Windows.Forms.CheckBox();
            this.ChkLevel4OBOnly = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // headerUC1
            // 
            this.headerUC1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.headerUC1.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerUC1.FormText = "";
            this.headerUC1.HeaderText = "";
            this.headerUC1.Location = new System.Drawing.Point(0, 0);
            this.headerUC1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.headerUC1.Name = "headerUC1";
            this.headerUC1.Size = new System.Drawing.Size(808, 37);
            this.headerUC1.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.BorderRadius = 5;
            this.btnClear.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnClear.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnClear.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnClear.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(41)))), ((int)(((byte)(83)))));
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(646, 498);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(135, 37);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear All Data";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ChkBoxTables
            // 
            this.ChkBoxTables.FormattingEnabled = true;
            this.ChkBoxTables.Items.AddRange(new object[] {
            "Attendance Sheet",
            "Attendance Salary",
            "Bank Card",
            "Bank Register",
            "Bank",
            "Cheque Details",
            "Cheque",
            "Corporate Tax Configuration",
            "Cost Center",
            "Credit Note",
            "Customer",
            "Customer Category",
            "Damage",
            "Debit Note",
            "Departments",
            "Employee",
            "End Of Service",
            "Final Settlement",
            "Fixed Assets",
            "Fixed Assets Category",
            "General Config",
            "Items",
            "Items Unit",
            "Items Warehouse",
            "Item Assembly",
            "Item Category",
            "Item Stock Settlement",
            "journal Voucher",
            "Leave Salary",
            "Loan",
            "Payment",
            "Petty Cash Card",
            "petty Cash Category",
            "petty Cash Request",
            "petty Cash Submission",
            "Petty Cash",
            "Position",
            "Prepaid Expense",
            "Purchase",
            "Purchase Order",
            "Purchase Return",
            "Receipt Voucher",
            "Salary",
            "Sales",
            "Sales Order",
            "Sales Quotation",
            "Sales Return",
            "Sales Performa",
            "Sub Cost Center",
            "Unit",
            "Vat",
            "Vendor",
            "Vendor Category",
            "Warehouse",
            "Salary Adjustments",
            "Advance Payment Voucher"});
            this.ChkBoxTables.Location = new System.Drawing.Point(12, 52);
            this.ChkBoxTables.Name = "ChkBoxTables";
            this.ChkBoxTables.Size = new System.Drawing.Size(477, 484);
            this.ChkBoxTables.TabIndex = 3;
            // 
            // ChkAll
            // 
            this.ChkAll.AutoSize = true;
            this.ChkAll.Location = new System.Drawing.Point(517, 52);
            this.ChkAll.Name = "ChkAll";
            this.ChkAll.Size = new System.Drawing.Size(70, 17);
            this.ChkAll.TabIndex = 4;
            this.ChkAll.Text = "Select All";
            this.ChkAll.UseVisualStyleBackColor = true;
            this.ChkAll.CheckedChanged += new System.EventHandler(this.ChkAll_CheckedChanged);
            // 
            // ChkLevel2
            // 
            this.ChkLevel2.AutoSize = true;
            this.ChkLevel2.Location = new System.Drawing.Point(517, 221);
            this.ChkLevel2.Name = "ChkLevel2";
            this.ChkLevel2.Size = new System.Drawing.Size(123, 17);
            this.ChkLevel2.TabIndex = 5;
            this.ChkLevel2.Text = "Chart of A/C Level 2";
            this.ChkLevel2.UseVisualStyleBackColor = true;
            // 
            // ChkLevel1
            // 
            this.ChkLevel1.AutoSize = true;
            this.ChkLevel1.Location = new System.Drawing.Point(517, 181);
            this.ChkLevel1.Name = "ChkLevel1";
            this.ChkLevel1.Size = new System.Drawing.Size(123, 17);
            this.ChkLevel1.TabIndex = 6;
            this.ChkLevel1.Text = "Chart of A/C Level 1";
            this.ChkLevel1.UseVisualStyleBackColor = true;
            // 
            // ChkLevel3
            // 
            this.ChkLevel3.AutoSize = true;
            this.ChkLevel3.Location = new System.Drawing.Point(517, 259);
            this.ChkLevel3.Name = "ChkLevel3";
            this.ChkLevel3.Size = new System.Drawing.Size(123, 17);
            this.ChkLevel3.TabIndex = 7;
            this.ChkLevel3.Text = "Chart of A/C Level 3";
            this.ChkLevel3.UseVisualStyleBackColor = true;
            // 
            // ChkLevel4
            // 
            this.ChkLevel4.AutoSize = true;
            this.ChkLevel4.Location = new System.Drawing.Point(517, 295);
            this.ChkLevel4.Name = "ChkLevel4";
            this.ChkLevel4.Size = new System.Drawing.Size(123, 17);
            this.ChkLevel4.TabIndex = 8;
            this.ChkLevel4.Text = "Chart of A/C Level 4";
            this.ChkLevel4.UseVisualStyleBackColor = true;
            // 
            // ChkLevel4OBOnly
            // 
            this.ChkLevel4OBOnly.AutoSize = true;
            this.ChkLevel4OBOnly.Location = new System.Drawing.Point(517, 332);
            this.ChkLevel4OBOnly.Name = "ChkLevel4OBOnly";
            this.ChkLevel4OBOnly.Size = new System.Drawing.Size(141, 17);
            this.ChkLevel4OBOnly.TabIndex = 9;
            this.ChkLevel4OBOnly.Text = "General Ledger OB Only";
            this.ChkLevel4OBOnly.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 37);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2, 509);
            this.panel3.TabIndex = 171;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(806, 37);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2, 509);
            this.panel4.TabIndex = 172;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(2, 544);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(804, 2);
            this.panel5.TabIndex = 173;
            // 
            // frmDataHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(808, 546);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.ChkLevel4OBOnly);
            this.Controls.Add(this.ChkLevel4);
            this.Controls.Add(this.ChkLevel3);
            this.Controls.Add(this.ChkLevel1);
            this.Controls.Add(this.ChkLevel2);
            this.Controls.Add(this.ChkAll);
            this.Controls.Add(this.ChkBoxTables);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.headerUC1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmDataHandler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDataHandler";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HeaderUC headerUC1;
        private Guna.UI2.WinForms.Guna2Button btnClear;
        private System.Windows.Forms.CheckedListBox ChkBoxTables;
        private System.Windows.Forms.CheckBox ChkAll;
        private System.Windows.Forms.CheckBox ChkLevel2;
        private System.Windows.Forms.CheckBox ChkLevel1;
        private System.Windows.Forms.CheckBox ChkLevel3;
        private System.Windows.Forms.CheckBox ChkLevel4;
        private System.Windows.Forms.CheckBox ChkLevel4OBOnly;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
    }
}