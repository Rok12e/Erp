using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject.UI.Default
{
    public partial class frmCompanyList : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
            (
            int nLeftRect,
            int nTopRect,
            int RightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        bool idDefault;
        public frmCompanyList(bool _idDefault = false)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.idDefault = _idDefault;
            Region = Region.FromHrgn(CreateRoundRectRgn(0,0,Width,Height,25,25));
            ProgressBar1.Value = 0;

            // Set culture here (can be from user selection)
            //LocalizationManager.ApplySavedLanguage(); // or "en"
            LocalizationManager.LocalizeForm(this);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.Hide();
            new frmSignup().Show();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            loadCompany();
        }

        private void frmCompanyList_Load(object sender, EventArgs e)
        {
            string customerCode = Environment.GetEnvironmentVariable("yamy_company_code", EnvironmentVariableTarget.User);

            DataTable table = DBClass.ExecuteDataTable("SELECT id,`Code`,`Name`,Descriptions,default_company,database_name FROM yamycompany.tbl_company WHERE customer_code = @customerCode", DBClass.CreateParameter("customerCode", customerCode));

            if(table!=null && table.Rows.Count == 0)
            {
                btnCreate.Enabled = true;
                if(string.IsNullOrEmpty(customerCode) || !idDefault)
                {
                    btnCreate.Enabled = true;
                }
            } else
            {
                btnCreate.Enabled = false;
            }
            dgvCompanyList.DataSource = table;
            dgvCompanyList.Columns["id"].Visible = dgvCompanyList.Columns["default_company"].Visible = dgvCompanyList.Columns["database_name"].Visible = false;

            dgvCompanyList.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            dgvCompanyList.EnableHeadersVisualStyles = false;
            dgvCompanyList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewRow row in dgvCompanyList.Rows)
            {
                if (row.Cells["default_company"].Value != DBNull.Value && Convert.ToInt32(row.Cells["default_company"].Value) == 1)
                {
                    //row.DefaultCellStyle.BackColor = Color.LightGreen;
                    dgvCompanyList.ClearSelection();
                    row.Selected = true;
                }
            }
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCompanyList);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.D))
            {
                using (var passwordForm = new PasswordPromptForm("Enter developer password:"))
                {
                    if (passwordForm.ShowDialog() == DialogResult.OK)
                    {
                        if (passwordForm.EnteredPassword == SecurityConfig.DeveloperPassword)
                        {
                            MessageBox.Show("Developer mode enabled.");
                            btnCreate.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Incorrect password.");
                        }
                    }
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void dgvCompanyList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvCompanyList.Rows.Count == 0)
            {
                MessageBox.Show("Company Not Found");
                return;
            }

            var selectedRow = dgvCompanyList.Rows[e.RowIndex];

            // Ensure the selected row has valid data for 'id' and 'database_name'
            var id = selectedRow.Cells["id"].Value?.ToString();
            var dbName = selectedRow.Cells["database_name"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(dbName))
            {
                MessageBox.Show("Company Not Found");
                return;
            }

            // Set the database from the selected row's database name
            DBClass.Database = dbName;
            new frmLogin().Show();
            this.Hide();
        }
        private void loadCompany()
        {
            if (dgvCompanyList.Rows.Count == 0)
            {
                MessageBox.Show("Company Not Found");
                return;
            }

            DataGridViewRow selectedRow = dgvCompanyList.CurrentRow;

            if (selectedRow == null)
                return;

            var id = selectedRow.Cells["id"].Value?.ToString();
            var dbName = selectedRow.Cells["database_name"].Value?.ToString();

            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(dbName))
            {
                MessageBox.Show("Company Not Found");
                return;
            }

            DBClass.Database = dbName;

            new frmLogin().Show();
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProgressBar1.Value += 1;
            ProgressBar1.Text = ProgressBar1.Value.ToString() + "%";
            if (ProgressBar1.Value == 100)
            {
                timer1.Enabled = false;
                guna2Panel2.Visible = false;
                guna2Panel1.Visible = true;
                guna2Panel1.Focus();

            }
        }

        private void dgvCompanyList_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            loadCompany();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }
    }
}
