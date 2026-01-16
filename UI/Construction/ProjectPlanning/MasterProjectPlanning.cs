using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterProjectPlanning : Form
    {
        //private MainForm _mainForm;
        public MasterProjectPlanning()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            //_mainForm = mainForm;
            this.Text = "Project Planning Center";
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewProjectPlanning(this, 0));
        }
        private void MasterProjectPlanning_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateProjects(cmbProject);
            BindData();
        }
        public void BindData()
        {
            DataTable dt;
            string query = "";
            query = @"SELECT 
                         ROW_NUMBER() OVER (
                        ORDER BY tbl_project_planning.date) AS `SN`, 
                         tbl_project_planning.date AS DATE,  
                         tbl_project_planning.id, 
                         tbl_project_planning.id AS 'P NO',
                         CONCAT(tbl_projects.code,' - ',tbl_projects.name) AS 'Project Name',
                         tbl_project_planning.start_date AS 'Start Date',
                         tbl_project_planning.start_date AS 'Start Date',
                         tbl_project_planning.end_date AS 'End Date',
                         tbl_project_planning.`Status`,
                         tbl_project_planning.project_type AS 'Project Type',
                         tbl_project_planning.estimated_budget AS 'Est Budget',
                         tbl_project_planning.progress AS 'Progress'
                        FROM 
                         tbl_project_planning
                        INNER JOIN
                        tbl_projects ON tbl_project_planning.project_id=tbl_projects.id 
                                                WHERE 
                                                 tbl_project_planning.state = 0
                        ";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbProject.Text != "" && !chkProject.Checked)
            {
                query += " and tbl_project_planning.project_id = @id";
                parameters.Add(DBClass.CreateParameter("id", cmbProject.SelectedValue.ToString()));
            }
            if (cmbProjectStatus.Text != "" && !chkProjectStatus.Checked)
            {
                query += " and tbl_project_planning.status = @status";
                parameters.Add(DBClass.CreateParameter("status", cmbProjectStatus.Text.ToString()));
            }
            if (cmbProjectType.Text != "" && !chkProjectType.Checked)
            {
                query += " and tbl_project_planning.project_type = @type";
                parameters.Add(DBClass.CreateParameter("type", cmbProjectType.Text.ToString()));
            }
            if (!chkDate.Checked)
                query += " and tbl_project_planning.date >= @dateFrom and tbl_project_planning.date <= @dateTo";
            if (cmbProjectStatus.Text == "Default")
                query += " GROUP BY tbl_project_planning.id, tbl_project_planning.date, tbl_project_planning.estimated_budget; ";

            else
                query += " GROUP BY tbl_project_planning.id, tbl_project_planning.date, tbl_project_planning.estimated_budget";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgView.DataSource = dt;
            //dgvCustomer.Columns["Employee Name"].MinimumWidth = 100;
            //dgvCustomer.Columns["Project Name"].MinimumWidth = 100;
            //dgvCustomer.Columns["Project Type"].MinimumWidth = 100; 
            //dgvCustomer.Columns["Employee Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgvCustomer.Columns["Project Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgvCustomer.Columns["Project Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgvCustomer.Columns["id"].Visible = false;
            gridDesign();
        }
        private void gridDesign()
        {
            dgView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;
            dgView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgView.GridColor = Color.Gray;
            dgView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            dgView.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
            dgView.EnableHeadersVisualStyles = false;
            dgView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dgView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);

            // Style DataGridView
            dgView.AutoGenerateColumns = false;
            dgView.AllowUserToResizeColumns = true;
            dgView.AllowUserToResizeRows = false;
            dgView.RowHeadersVisible = false;
            dgView.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSkyBlue;
            dgView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            dgView.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgView.DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgView.DefaultCellStyle.ForeColor = Color.Black;
            dgView.GridColor = Color.Gray;

            dgView.Columns["SN"].Width = 50;
            dgView.Columns["DATE"].Width = 60;
            dgView.Columns["P NO"].Width = 50;
            dgView.Columns["Project Name"].Width = 270;
            dgView.Columns["Start Date"].Width = 60;
            dgView.Columns["End Date"].Width = 60;
            dgView.Columns["Status"].Width = 100;
            dgView.Columns["Project Type"].Width = 120;
            dgView.Columns["Est Budget"].Width = 80;
            dgView.Columns["Progress"].Width = 80;

            //dgvCustomer.Columns["Progress"].DefaultCellStyle.Format = "P0";
            //dgvCustomer.Columns["Start Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            //dgvCustomer.Columns["End Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            //dgvCustomer.Columns["Est Budget"].DefaultCellStyle.Format = "₱ #,0.00";
            dgView.Columns["Est Budget"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgView.AlternatingRowsDefaultCellStyle.BackColor = Color.AliceBlue;

            // Freeze the first column (SN)
            dgView.Columns["SN"].Frozen = true;
            dgView.Columns["id"].Visible = false;

            // Auto Resize Columns
            //dgvCustomer.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        private void dgvCustomer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgView.Columns[e.ColumnIndex].Name == "Status")
            {
                string status = e.Value.ToString().Trim();

                switch (status)
                {
                    case "Planning":
                        e.CellStyle.BackColor = Color.LightYellow;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Progress":
                        e.CellStyle.BackColor = Color.LightBlue;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    case "Completed":
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                        break;

                    default:
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.ForeColor = Color.Black;
                        break;
                }
            }
        }


        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;
            //frmLogin.frmMain.openChildForm(new frmViewProjectPlanning(this, int.Parse(dgView.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewProjectPlanning(this, int.Parse(dgView.CurrentRow.Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;
            //DBClass.ExecuteNonQuery("UPDATE tbl_damage SET state = -1 WHERE id = @id ",
            //                              DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            BindData();
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));

        }
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProject.Checked)
                cmbProject.Enabled = false;
            else
                cmbProject.Enabled = true;
            BindData();
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            BindData();
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        private void dgvCustomer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dgView.Rows)
            {
                if (row.IsNewRow) continue;
                
                string status = row.Cells["Status"].Value.ToString().Trim();
                switch (status)
                {
                    case "Planning":
                        row.DefaultCellStyle.BackColor = Color.LightYellow;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;

                    case "Progress":
                        row.DefaultCellStyle.BackColor = Color.LightBlue;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;

                    case "Completed":
                        row.DefaultCellStyle.BackColor = Color.LightGreen;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;

                    default:
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                        break;
                }
            }
        }

        private void chkProjectType_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProjectType.Checked)
                cmbProjectType.Enabled = false;
            else
                cmbProjectType.Enabled = true;
            BindData();
        }

        private void chkProjectStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (chkProjectStatus.Checked)
                cmbProjectStatus.Enabled = false;
            else
                cmbProjectStatus.Enabled = true;
            BindData();
        }
        private void dtDate_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dgvCustomer_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && dgView.Columns[e.ColumnIndex].Name == "Progress")
            {
                if (dgView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    //dgvCustomer.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                    float progress = 0;
                    if (float.TryParse(dgView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString(), out progress))
                    {
                        progress = Math.Max(0, Math.Min(100, progress)); // Ensure progress is between 0 and 100
                        
                        int progressWidth = (int)(progress / 100 * e.CellBounds.Width);
                        
                        e.PaintBackground(e.ClipBounds, true);
                        
                        Color progressColor;

                        if (progress <= 10)
                        {
                            progressColor = Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(42)))), ((int)(((byte)(83)))));  // 0% - 10%
                        }
                        else if (progress <= 30)
                        {
                            progressColor = Color.Orange;  // 10% - 30%
                        }
                        else if (progress <= 50)
                        {
                            progressColor = Color.Yellow;  // 30% - 50%
                        }
                        else if (progress <= 70)
                        {
                            progressColor = Color.LightGreen;  // 50% - 70%
                        }
                        else if (progress <= 90)
                        {
                            progressColor = Color.Green;  // 70% - 90%
                        }
                        else
                        {
                            progressColor = Color.DarkGreen;  // 90% - 100%
                        }
                        
                        using (Brush brush = new SolidBrush(progressColor))
                        {
                            e.Graphics.FillRectangle(brush, e.CellBounds.X, e.CellBounds.Y,
                                                     progressWidth, e.CellBounds.Height);
                        }
                        
                        using (Pen borderPen = new Pen(Color.Gray))
                        {
                            e.Graphics.DrawRectangle(borderPen, e.CellBounds);
                        }
                        
                        string progressText = progress.ToString("0") + "%";
                        using (Brush textBrush = new SolidBrush(Color.Black))
                        {
                            SizeF textSize = e.Graphics.MeasureString(progressText, e.CellStyle.Font);
                            
                            float x = e.CellBounds.X + (e.CellBounds.Width - textSize.Width) / 2;
                            float y = e.CellBounds.Y + (e.CellBounds.Height - textSize.Height) / 2;
                            
                            e.Graphics.DrawString(progressText, e.CellStyle.Font, textBrush, x, y);
                        }
                        
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
