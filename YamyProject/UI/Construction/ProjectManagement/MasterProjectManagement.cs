using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;

namespace YamyProject
{
    public partial class MasterProjectManagement : Form
    {
        //private MainForm _mainForm;
        public MasterProjectManagement()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            //_mainForm = mainForm;
            this.Text = "Project Management";
            headerUC1.FormText = this.Text;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewProjectManagement(this, 0,0));
        }
        private void MasterProjectManagement_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateProjects(cmbProject);
            BindData();
        }
        public void BindData()
        {
            DataTable dt;
            string query = "";
            query = @"SELECT 
                        ROW_NUMBER() OVER (ORDER BY tbl_project_planning.date) AS `SN`, 
                        tbl_project_planning.date AS DATE,
                        tbl_project_planning.id AS 'P NO',
                        CONCAT(tbl_projects.code, ' - ', tbl_projects.name) AS 'Project Name',
                        tbl_project_planning.start_date AS 'Start Date',
                        tbl_project_planning.end_date AS 'End Date',
                        tbl_project_planning.`Status`,
                        tbl_project_planning.project_type AS 'Project Type',
                        tbl_project_planning.estimated_budget AS 'Est Budget',
                        COALESCE(tbl_project_management.id, '0') AS 'id',
                        COALESCE(tbl_project_management.budget, '') AS 'Budget',
                        COALESCE(tbl_project_management.actual_cost, '') AS 'Actual Cost',
                        COALESCE(tbl_project_management.remaining_budget, '') AS 'Remaining Budget'
                    FROM 
                        tbl_project_planning
                    INNER JOIN
                        tbl_projects ON tbl_project_planning.project_id = tbl_projects.id
                    LEFT JOIN
                        tbl_project_management ON tbl_project_management.project_planning_id = tbl_project_planning.id AND tbl_project_management.project_id=tbl_project_planning.project_id
                    WHERE 
                        tbl_project_planning.state = 0 ";

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
            //if (cmbProjectStatus.Text == "Default")
            //    query += " GROUP BY tbl_project_planning.id, tbl_project_planning.date, tbl_project_planning.estimated_budget; ";

            //else
            //    query += " GROUP BY tbl_project_planning.id, tbl_project_planning.date, tbl_project_planning.estimated_budget";
            parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgView.DataSource = dt;

            gridDesign();
        }
        private void gridDesign()
        {
            dgView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.AliceBlue;
            dgView.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            dgView.GridColor = System.Drawing.Color.Gray;
            dgView.CellBorderStyle = DataGridViewCellBorderStyle.Single;

            dgView.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.LightGrid;
            dgView.EnableHeadersVisualStyles = false;
            dgView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            dgView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold);

            // Style DataGridView
            dgView.AutoGenerateColumns = false;
            dgView.AllowUserToResizeColumns = true;
            dgView.AllowUserToResizeRows = false;
            dgView.RowHeadersVisible = false;
            dgView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.LightSkyBlue;
            dgView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgView.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold);
            dgView.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            dgView.DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            dgView.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            dgView.GridColor = System.Drawing.Color.Gray;

            dgView.Columns["SN"].Width = 50;
            dgView.Columns["DATE"].Width = 60;
            dgView.Columns["P NO"].Width = 50;
            dgView.Columns["Project Name"].Width = 220;
            dgView.Columns["Start Date"].Width = 60;
            dgView.Columns["End Date"].Width = 60;
            dgView.Columns["Status"].Width = 100;
            dgView.Columns["Project Type"].Width = 90;
            dgView.Columns["Est Budget"].Width = 80;
            dgView.Columns["Budget"].Width = 80;
            dgView.Columns["Actual Cost"].Width = 80;
            dgView.Columns["Remaining Budget"].Width = 80;

            dgView.Columns["DATE"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgView.Columns["Start Date"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgView.Columns["End Date"].DefaultCellStyle.Format = "dd/MM/yyyy";

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.Format = "N3";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.NullValue = null;
            dgView.Columns["Est Budget"].DefaultCellStyle = dataGridViewCellStyle2;
            dgView.Columns["Budget"].DefaultCellStyle = dataGridViewCellStyle2;
            dgView.Columns["Actual Cost"].DefaultCellStyle = dataGridViewCellStyle2;
            dgView.Columns["Remaining Budget"].DefaultCellStyle = dataGridViewCellStyle2;

            dgView.Columns["SN"].Frozen = true;
            dgView.Columns["id"].Visible = false;
        }
        private void dgvCustomer_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //
        }


        private void cmbOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewProjectManagement(this, int.Parse(dgView.SelectedRows[0].Cells["id"].Value.ToString()), int.Parse(dgView.SelectedRows[0].Cells["P NO"].Value.ToString())));
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgView.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewProjectManagement(this, int.Parse(dgView.CurrentRow.Cells["id"].Value.ToString()), int.Parse(dgView.SelectedRows[0].Cells["P NO"].Value.ToString())));
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
        }
        private void btnRestore_Click(object sender, EventArgs e)
        {
            //_mainForm.openChildForm(new MasterInventoryRecycle(_mainForm, this));

        }
        private void chkProject_CheckedChanged(object sender, EventArgs e)
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
            //
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindData();
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count > 0)
            {
                GeneratePDF(dgView, "Project Management Summary");
                MessageBox.Show("PDF generated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No data available to pdf.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GeneratePDF(Guna.UI2.WinForms.Guna2DataGridView dgvItems, string title)
        {
            string companyName = "";
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT name FROM tbl_company LIMIT 1"))
                if (reader.Read())
                    companyName = reader["name"].ToString();

            Document doc = new Document();
            Section section = doc.AddSection();
            int visibleColumnCount = dgvItems.Columns.Cast<DataGridViewColumn>().Count(col => col.Visible);
            section.PageSetup.Orientation = visibleColumnCount > 6 ? Orientation.Landscape : Orientation.Portrait;

            // Page margins
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);

            // Header
            Paragraph header = section.AddParagraph();
            header.Format.Alignment = ParagraphAlignment.Center;
            header.Format.Font.Size = 12;
            header.Format.Font.Bold = true;
            header.AddText(companyName + Environment.NewLine);
            header.AddFormattedText(title + Environment.NewLine, TextFormat.Bold);
            header.Format.SpaceAfter = "0.5cm";

            // Date/Time
            Paragraph timeInfo = section.AddParagraph();
            timeInfo.Format.Alignment = ParagraphAlignment.Right;
            timeInfo.Format.Font.Size = 9;
            timeInfo.AddText(DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"));
            timeInfo.Format.SpaceAfter = "0.5cm";

            // Table for data
            Table table = section.AddTable();
            table.Borders.Width = 0.5;
            table.Format.Font.Name = "Times New Roman";
            table.Format.Font.Size = 9;

            // Define columns dynamically
            foreach (DataGridViewColumn col in dgView.Columns)
            {
                if (col.Visible)
                {
                    Column tableCol = table.AddColumn(Unit.FromCentimeter(5)); // Adjust width as needed
                    tableCol.Format.Alignment = ParagraphAlignment.Left;
                }
            }

            // Add table header
            Row headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            headerRow.Format.Font.Bold = true;

            int visibleColIndex = 0;
            foreach (DataGridViewColumn col in dgView.Columns)
            {
                if (col.Visible)
                {
                    headerRow.Cells[visibleColIndex].AddParagraph(col.HeaderText);
                    visibleColIndex++;
                }
            }

            // Add rows from DataGridView
            foreach (DataGridViewRow dgvRow in dgView.Rows)
            {
                if (dgvRow.IsNewRow) continue;
                Row row = table.AddRow();

                int colIndex = 0;
                foreach (DataGridViewColumn col in dgView.Columns)
                {
                    if (col.Visible)
                    {
                        string value = dgvRow.Cells[col.Index].Value?.ToString() ?? "";
                        row.Cells[colIndex].AddParagraph(value);
                        row.Cells[colIndex].Format.Alignment = ParagraphAlignment.Left;
                        colIndex++;
                    }
                }
            }

            // Render and save PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true)
            {
                Document = doc
            };
            renderer.RenderDocument();

            string filename = $"{title.Replace(" ", "_")}Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), filename);
            renderer.PdfDocument.Save(filePath);

            // Open file
            Process.Start("explorer.exe", filePath);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (dgView.Rows.Count > 0)
            {
                GenerateExcel(dgView, "Project Management Summary");
                MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No data available to excel.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GenerateExcel(Guna.UI2.WinForms.Guna2DataGridView dgvItems, string title)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                string fileName = $"{title.Replace(" ", "_")}_Report_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                saveDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveDialog.Title = "Save Excel File";
                saveDialog.FileName = fileName;

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = title;

                    int totalVisibleCols = dgView.Columns.Cast<DataGridViewColumn>().Count(col => col.Visible);

                    // Merge and format header (Date and Title)
                    Microsoft.Office.Interop.Excel.Range headerRange = worksheet.Range[
                        worksheet.Cells[1, 1], worksheet.Cells[1, totalVisibleCols]];
                    headerRange.Merge();
                    headerRange.Value = title + "  |  Date: " + DateTime.Now.ToString("MMM dd, yyyy hh:mm tt");
                    headerRange.Font.Bold = true;
                    headerRange.Font.Name = "Times New Roman";
                    headerRange.Font.Size = 11;
                    headerRange.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                    headerRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightYellow);

                    // Add column headers (row 2)
                    int colIndex = 1;
                    foreach (DataGridViewColumn col in dgView.Columns)
                    {
                        if (!col.Visible) continue;

                        worksheet.Cells[2, colIndex] = col.HeaderText;
                        var headerCell = worksheet.Cells[2, colIndex];
                        headerCell.Font.Bold = true;
                        headerCell.Font.Name = "Times New Roman";
                        headerCell.Font.Size = 10;
                        headerCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                        headerCell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                        colIndex++;
                    }

                    // Add data rows (from row 3)
                    int rowIndex = 3;
                    foreach (DataGridViewRow dgvRow in dgView.Rows)
                    {
                        if (dgvRow.IsNewRow) continue;

                        colIndex = 1;
                        foreach (DataGridViewColumn col in dgView.Columns)
                        {
                            if (!col.Visible) continue;

                            string value = dgvRow.Cells[col.Index].Value?.ToString() ?? "";
                            var cell = worksheet.Cells[rowIndex, colIndex];
                            cell.Value = value;
                            cell.Font.Name = "Times New Roman";
                            cell.Font.Size = 9;

                            // Optional: Right-align numeric columns (if needed)
                            if (decimal.TryParse(value, out _))
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;
                            else
                                cell.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

                            colIndex++;
                        }

                        rowIndex++;
                    }

                    worksheet.Columns.AutoFit();

                    workbook.SaveAs(saveDialog.FileName);
                    workbook.Close(false);
                    excelApp.Quit();

                    MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
