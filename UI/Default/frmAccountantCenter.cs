using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;


namespace YamyProject
{
    public partial class frmAccountantCenter : Form
    {

        public frmAccountantCenter()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            LoadTools();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAccountantCenter_Load(object sender, EventArgs e)
        {
            Lbheader.Text = "Accountant Center";
            LoadTools();
            dgvTools.CellClick += dgvTools_CellClick;
            //dgvTools2_CellClick += dgvTools2_CellClick;
            customize.Click += customize_Click;

            cmbMemorized.Items.Add("Accountant");
            cmbMemorized.Items.Add("Banking");
            cmbMemorized.Items.Add("Company");
            cmbMemorized.Items.Add("Customers");
            cmbMemorized.Items.Add("Employee");
            cmbMemorized.Items.Add("NonProfit");
            cmbMemorized.Items.Add("Vendor");

            // Set default selection to "Accountant"
            cmbMemorized.SelectedIndex = 0;

            // Call the method to show "Accountant" panel by default
            ShowPanel("Accountant");

            // Attach event handler for selection change
            cmbMemorized.SelectedIndexChanged += cmbMemorized_SelectedIndexChanged;
            dgvReconcile.Rows.Add("Petty Cash");
            dgvReconcile.Rows[0].Cells["account"].Style.ForeColor = Color.Blue;
            dgvReconcile.Rows[0].Cells["account"].Style.Font = new Font(dgvReconcile.Font, FontStyle.Underline);
            dgvReconcile.Cursor = Cursors.Hand;
        }
        private void LoadTools()
        {
            try
            {
                string query = "SELECT id, tool_name FROM tbl_tools WHERE is_selected = 0";
                DataTable dt = DBClass.ExecuteDataTable(query);

                // Clone schema for each table
                DataTable dtLeft = dt.Clone();
                DataTable dtRight = dt.Clone();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                        dtLeft.ImportRow(dt.Rows[i]);
                    else
                        dtRight.ImportRow(dt.Rows[i]);
                }

                dgvTools.DataSource = dtLeft;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvTools);
                dgvTools2.DataSource = dtRight;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvTools2);

                FinalizeToolsUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading tools: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FinalizeToolsUI()
        {
            dgvTools.Refresh();
            dgvTools2.Refresh();

            if (dgvTools.Columns.Contains("id")) dgvTools.Columns["id"].Visible = false;
            if (dgvTools2.Columns.Contains("id")) dgvTools2.Columns["id"].Visible = false;

            dgvTools.ColumnHeadersVisible = false;
            dgvTools2.ColumnHeadersVisible = false;

            CustomizeDataGridView(dgvTools);
            CustomizeDataGridView(dgvTools2);

            AdjustDataGridViewSize(dgvTools);
            AdjustDataGridViewSize(dgvTools2);
        }
        private void CustomizeDataGridView(DataGridView dgv)
        {
            Font linkFont = new Font("Segoe UI", 9F, FontStyle.Underline); // Match "Customize"/"Refresh"
            Color linkColor = Color.Blue;

            dgv.DefaultCellStyle.Font = linkFont;
            dgv.DefaultCellStyle.ForeColor = linkColor;

            // ✅ Make selection invisible (no black background)
            dgv.DefaultCellStyle.SelectionBackColor = dgv.DefaultCellStyle.BackColor;
            dgv.DefaultCellStyle.SelectionForeColor = linkColor;

            dgv.EnableHeadersVisualStyles = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.GridColor = Color.LightGray;
            dgv.RowTemplate.Height = 24;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowsDefaultCellStyle.BackColor = Color.FromArgb(227, 239, 254); 

        }

        private void AdjustDataGridViewSize(DataGridView dgv)
        {
            int rowHeight = dgv.RowTemplate.Height;
            int rowCount = dgv.Rows.Count;

            int headerHeight = dgv.ColumnHeadersVisible ? dgv.ColumnHeadersHeight : 0;

            // Max height allowed (adjust based on your form's layout)
            int maxHeight = 600; // or something like panel/container height

            // Calculate needed height
            int neededHeight = headerHeight + (rowHeight * rowCount);

            // Add a little margin
            neededHeight += 10;

            // Apply size
            dgv.Height = Math.Min(neededHeight, maxHeight);
        }

        public void RefreshTools()
        {
            LoadTools();
        }
        private void OpenToolForm(string toolText)
        {
                string query = "SELECT form_name FROM tbl_set_menu_forms WHERE form_text = @formText LIMIT 1";
                DataTable dt = DBClass.ExecuteDataTable(query, new MySqlParameter("@formText", toolText));

                if (dt.Rows.Count > 0)
                {
                    string formName = dt.Rows[0]["form_name"].ToString();
                    Type formType = Type.GetType($"YamyProject.{formName}");

                    if (formType != null)
                    {
                    //Form formToOpen = (Form)Activator.CreateInstance(formType);
                    //formToOpen.Show();
                    frmLogin.frmMain.openChildForm((Form)Activator.CreateInstance(formType));
                    }
            }

        }
        private void dgvTools_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string toolName = dgvTools.Rows[e.RowIndex].Cells["tool_name"].Value.ToString();
                OpenToolForm(toolName);
            }
        }
        private void dgvTools2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string toolName = dgvTools2.Rows[e.RowIndex].Cells["tool_name"].Value.ToString();
                OpenToolForm(toolName);
            }
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            LoadTools();
        }

        private void refresh_MouseEnter(object sender, EventArgs e)
        {
            refresh.Font = new Font(customize.Font, FontStyle.Underline);
            refresh.Cursor = Cursors.Hand;
        }

        private void refresh_MouseLeave(object sender, EventArgs e)
        {
            refresh.Font = new Font(customize.Font, FontStyle.Regular);
            refresh.Cursor = Cursors.Default;
        }

        private void cmbMemorized_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMemorized.SelectedItem != null)
            {
                string selectedValue = cmbMemorized.SelectedItem.ToString();
                ShowPanel(selectedValue);
            }
        }
        private void ShowPanel(string panelName)
        {
            pnlAccounting.Visible = false;
            pnlVendor.Visible = false;
            pnlBanking.Visible = false;
            pnlCompany.Visible = false;
            pnlCustomers.Visible = false;
            pnlEmployee.Visible = false;
            pnlNonProfit.Visible = false;

            switch (panelName)
            {
                case "Accountant":
                    pnlAccounting.Visible = true;
                    break;
                case "Banking":
                    pnlBanking.Visible = true;
                    break;
                case "Company":
                    pnlCompany.Visible = true;
                    break;
                case "Customers":
                    pnlCustomers.Visible = true;
                    break;
                case "Employee":
                    pnlEmployee.Visible = true;
                    break;
                case "NonProfit":
                    pnlNonProfit.Visible = true;
                    break;
                case "Vendor":
                    pnlVendor.Visible = true;
                    break;
            }
        }
        private void customize_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmCustomizeList());

        }
        private void dgvReconcile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0) 
            {
                string clickedAccount = dgvReconcile.Rows[e.RowIndex].Cells["account"].Value.ToString();

                if (clickedAccount == "Petty Cash")
                {
                    frmLogin.frmMain.openChildForm(new frmPettyCashSubmission(0));
                }
            }
        }

        private void customize_MouseEnter(object sender, EventArgs e)
        {
            customize.Font = new Font(customize.Font, FontStyle.Underline);
            customize.Cursor = Cursors.Hand;
        }

        private void customize_MouseLeave(object sender, EventArgs e)
        {
            customize.Font = new Font(customize.Font, FontStyle.Regular);
            customize.Cursor = Cursors.Default;
        }

        private void dgvTools_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ((DataGridView)sender).Cursor = Cursors.Hand;

        }

        private void dgvTools_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ((DataGridView)sender).Cursor = Cursors.Default;
        }

        private void dgvTools2_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ((DataGridView)sender).Cursor = Cursors.Hand;
        }

        private void dgvTools2_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ((DataGridView)sender).Cursor = Cursors.Default;
        }

        private void guna2Separator3_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void panel8_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void Lbheader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
