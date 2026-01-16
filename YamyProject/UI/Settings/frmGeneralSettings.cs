using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmGeneralSettings : Form
    {
        public frmGeneralSettings()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void frmGeneralSettings_Load(object sender, EventArgs e)
        {
            DataTable dt = DBClass.ExecuteDataTable("select id, concat(name,'-',value , '%') as name from tbl_tax");
            if (dt.Rows.Count > 0)
            {
                cmbTax.DataSource = dt;
                cmbTax.DisplayMember = "name";
                cmbTax.ValueMember = "id";
            }
            LoadGeneral();
        }

        private void LoadGeneral()
        {
            if (BindDataTable.tableGeneralSettings.Rows.Count > 0)
            {
                if (cmbTax.Items.Count > 0)
                {
                    DataRow[] rows = BindDataTable.tableGeneralSettings.Select($"name = 'DEFAULT TAX PERCENTAGE'");
                    if (rows.Length > 0)
                    {
                        cmbTax.SelectedValue = rows[0]["value"];
                    }
                }

                foreach (DataRow row in BindDataTable.tableGeneralSettings.Rows)
                {
                    string itemName = row["name"].ToString();
                    bool isChecked = Convert.ToBoolean(row["status"]);
                    chkListSettings.Items.Add(itemName, isChecked);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbTax.Items.Count > 0 && cmbTax.SelectedValue != null)
            {
                string value = cmbTax.SelectedValue.ToString();
                string name = "DEFAULT TAX PERCENTAGE";
                DataRow[] rows = BindDataTable.tableGeneralSettings.Select($"name = '{name}'");
                foreach (DataRow row in rows)
                {
                    row["value"] = value;
                }
            }
            if (DBClass.ExecuteDataTableData(BindDataTable.tableGeneralSettings))
            {
                MessageBox.Show("Saved!");
            }
        }

        private void chkListSettings_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var clickedItem = chkListSettings.Items[e.Index].ToString();
            bool isChecked = e.NewValue == CheckState.Checked;

            DataRow[] rows = BindDataTable.tableGeneralSettings.Select($"name = '{clickedItem}'");

            foreach (DataRow row in rows)
            {
                row["status"] = isChecked ? "1" : "0";
            }
        }
    }
}
