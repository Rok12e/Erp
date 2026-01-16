namespace YamyProject.UI.Manufacturing.Models
{
    partial class frmManEmployeeSelectionForm
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
            this.checkedListBoxEmployees = new System.Windows.Forms.CheckedListBox();
            this.btnOK = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // checkedListBoxEmployees
            // 
            this.checkedListBoxEmployees.FormattingEnabled = true;
            this.checkedListBoxEmployees.Location = new System.Drawing.Point(12, 12);
            this.checkedListBoxEmployees.Name = "checkedListBoxEmployees";
            this.checkedListBoxEmployees.Size = new System.Drawing.Size(721, 454);
            this.checkedListBoxEmployees.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.BorderRadius = 5;
            this.btnOK.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnOK.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnOK.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnOK.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(335, 472);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 28);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmManEmployeeSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(745, 512);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.checkedListBoxEmployees);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmManEmployeeSelectionForm";
            this.Text = "EmployeeSelectionForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxEmployees;
        private Guna.UI2.WinForms.Guna2Button btnOK;
    }
}