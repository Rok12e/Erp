using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterSetting : Form
    {
        public Form LoadFormIntoPanel(Form f)
        {
            foreach (Control control in panel2.Controls)
            {
                if (control is Form && control.GetType() == f.GetType())
                {
                    Form existingForm = (Form)control;
                    if (existingForm.WindowState == FormWindowState.Minimized && existingForm.Tag != null)
                    {
                        existingForm.WindowState = FormWindowState.Normal;
                    }
                    existingForm.BringToFront();
                    return existingForm;
                }
            }

            // Prepare new form
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.Manual;

            // Center it in the panel
            int x = (panel2.Width - f.Width) / 2;
            int y = (panel2.Height - f.Height) / 2;
            f.Location = new Point(x, y);

            // Add and show it
            panel2.Controls.Add(f);
            f.BringToFront();
            f.Show();

            return f;
        }

        public Form addcontrols2(Form f)
        {
            panel2.Controls.Clear();
            f.TopLevel = false;
            panel2.Controls.Add(f);
            f.Show();
            return f;
        }



        public MasterSetting()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void MasterSetting_Load(object sender, EventArgs e)
        {
            string level4Query = "SELECT CONCAT(code, ' - ', name) AS name, id FROM tbl_coa_level_4 ORDER BY code";
            BindDataTable.tableLevel4 = DBClass.ExecuteDataTable(level4Query);

            BindDataTable.LoadConfigData();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmDefaultAccountSetting());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmDefaultEmployeeSetting());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmUserAccess());
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            addcontrols2(new frmGeneralSettings());
        }
    }
}

public static class AccountSettingsHelper
{
    public static void LoadAndSelect(Guna2ComboBox comboBox, string category)
    {
        if (BindDataTable.tableLevel4 == null || comboBox == null)
            return;

        // Bind the shared DataTable copy as DataSource
        comboBox.DataSource = BindDataTable.tableLevel4.Copy();
        comboBox.DisplayMember = "name";
        comboBox.ValueMember = "id";

        // Select value from cached config dictionary
        if (BindDataTable.coaConfigDict != null && BindDataTable.coaConfigDict.TryGetValue(category, out int selectedId))
            comboBox.SelectedValue = selectedId;
        else
            comboBox.SelectedIndex = -1;
    }
}

