using System;

namespace YamyProject.UI.Manufacturing.Viewform
{
    partial class frmManEmployeeTaskSelectionForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblEmpName = new System.Windows.Forms.Label();
            this.clbTasks = new System.Windows.Forms.CheckedListBox();
            this.btnStartSelected = new Guna.UI2.WinForms.Guna2Button();
            this.btnComplete = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 8);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Employee Task Selection";
            // 
            // lblEmpName
            // 
            this.lblEmpName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblEmpName.Location = new System.Drawing.Point(15, 41);
            this.lblEmpName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEmpName.Name = "lblEmpName";
            this.lblEmpName.Size = new System.Drawing.Size(300, 16);
            this.lblEmpName.TabIndex = 1;
            this.lblEmpName.Text = "Employee ID:";
            // 
            // clbTasks
            // 
            this.clbTasks.FormattingEnabled = true;
            this.clbTasks.Location = new System.Drawing.Point(15, 65);
            this.clbTasks.Margin = new System.Windows.Forms.Padding(2);
            this.clbTasks.Name = "clbTasks";
            this.clbTasks.Size = new System.Drawing.Size(301, 154);
            this.clbTasks.TabIndex = 2;
            // 
            // btnStartSelected
            // 
            this.btnStartSelected.BorderRadius = 5;
            this.btnStartSelected.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnStartSelected.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnStartSelected.ForeColor = System.Drawing.Color.White;
            this.btnStartSelected.Location = new System.Drawing.Point(15, 244);
            this.btnStartSelected.Margin = new System.Windows.Forms.Padding(2);
            this.btnStartSelected.Name = "btnStartSelected";
            this.btnStartSelected.Size = new System.Drawing.Size(112, 32);
            this.btnStartSelected.TabIndex = 3;
            this.btnStartSelected.Text = "Start Selected";
            this.btnStartSelected.Click += new System.EventHandler(this.btnStartSelected_Click);
            // 
            // btnComplete
            // 
            this.btnComplete.BorderRadius = 5;
            this.btnComplete.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));
            this.btnComplete.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnComplete.ForeColor = System.Drawing.Color.White;
            this.btnComplete.Location = new System.Drawing.Point(186, 244);
            this.btnComplete.Margin = new System.Windows.Forms.Padding(2);
            this.btnComplete.Name = "btnComplete";
            this.btnComplete.Size = new System.Drawing.Size(129, 32);
            this.btnComplete.TabIndex = 4;
            this.btnComplete.Text = "Mark as Complete";
            this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
            // 
            // frmManEmployeeTaskSelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(338, 301);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblEmpName);
            this.Controls.Add(this.clbTasks);
            this.Controls.Add(this.btnStartSelected);
            this.Controls.Add(this.btnComplete);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmManEmployeeTaskSelectionForm";
            this.Text = "Task List";
            this.Load += new System.EventHandler(this.frmManEmployeeTaskSelectionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblEmpName;
        private System.Windows.Forms.CheckedListBox clbTasks;
        private Guna.UI2.WinForms.Guna2Button btnStartSelected;
        private Guna.UI2.WinForms.Guna2Button btnComplete;
    }
}