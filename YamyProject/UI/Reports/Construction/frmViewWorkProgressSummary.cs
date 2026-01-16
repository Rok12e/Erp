using Guna.UI2.WinForms;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using Orientation = MigraDoc.DocumentObjectModel.Orientation;

namespace YamyProject
{
    public partial class frmViewWorkProgressSummary : Form
    {
        private string type;

        public frmViewWorkProgressSummary(string _type="")
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            type = _type;
            headerUC1.FormText = this.Text;
            dtFrom.Value = DateTime.Now;
            dtTo.Value = DateTime.Now;
        }
        
        private void frmViewWorkProgressSummary_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateProjects(cmbProject);
            
            BindData();
        }
        public void BindData()
        {
            DataTable dt;
            string query = "";
            query = @"SELECT (select name from tbl_items_boq where id = code) `Description`,
                            SUM(qty_total) AS `Total Done Qty`
                        FROM tbl_project_work_done_details
                        WHERE id > 0 ";
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (!chkCustomer.Checked)
            {
                query += " and ref_id = @project_id ";
            }
            query += " GROUP BY code;";
            dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("project_id", cmbProject.SelectedValue));
            dgvItems.DataSource = dt;
            
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvItems);
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }
        
        private void dgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count == 0)
                return;
            //if (dgvItems.Columns["JV NO"].Index == e.ColumnIndex)
            //    _mainForm.openChildForm(new MasterTransactionJournal(_mainForm, dgvItems.CurrentRow.Cells["JV NO"].Value.ToString(), "Sales"));
        }
        private void dgvItems_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvItems_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        
        private void chkCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomer.Checked)
                cmbProject.Enabled = false;
            else
                cmbProject.Enabled = true;
            BindData();
        }
        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayment.Checked)
                cmbPaymentMethod.Enabled = false;
            else
                cmbPaymentMethod.Enabled = true;
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

        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvItems.Rows.Count > 0 && cmbSelectionMethod.Text == "Default")
            //    bindInvoiceItems();
            //else
            //    dgvItems.DataSource = null;
        }

        private void cmbSalesType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            BindData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvItems.Rows.Count > 0)
            {
                GeneratePDF(dgvItems, "Work Progress Summary");
                MessageBox.Show("PDF generated successfully!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No data available to pdf.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GeneratePDF(Guna2DataGridView dgvItems, string title)
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
            foreach (DataGridViewColumn col in dgvItems.Columns)
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
            foreach (DataGridViewColumn col in dgvItems.Columns)
            {
                if (col.Visible)
                {
                    headerRow.Cells[visibleColIndex].AddParagraph(col.HeaderText);
                    visibleColIndex++;
                }
            }

            // Add rows from DataGridView
            foreach (DataGridViewRow dgvRow in dgvItems.Rows)
            {
                if (dgvRow.IsNewRow) continue;
                Row row = table.AddRow();

                int colIndex = 0;
                foreach (DataGridViewColumn col in dgvItems.Columns)
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
            if (dgvItems.Rows.Count > 0)
            {
                GenerateExcel(dgvItems, "Work Progress Summary");
                MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No data available to excel.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void GenerateExcel(Guna2DataGridView dgvItems, string title)
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

                    int totalVisibleCols = dgvItems.Columns.Cast<DataGridViewColumn>().Count(col => col.Visible);

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
                    foreach (DataGridViewColumn col in dgvItems.Columns)
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
                    foreach (DataGridViewRow dgvRow in dgvItems.Rows)
                    {
                        if (dgvRow.IsNewRow) continue;

                        colIndex = 1;
                        foreach (DataGridViewColumn col in dgvItems.Columns)
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
