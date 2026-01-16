using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class frmFixedAssetItemList : Form
    {
        public frmFixedAssetItemList()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }

        private void frmFixedAssetItemList_Load(object sender, EventArgs e)
        {
            dgvData.Columns.Clear();
            dgvData.Columns.Add("SN", "SN");
            dgvData.Columns.Add("Date", "Date");
            dgvData.Columns.Add("RefId", "Ref Id");
            dgvData.Columns.Add("Name", "Name");
            dgvData.Columns.Add("Code", "Code");
            dgvData.Columns.Add("ItemId", "Item Id");
            dgvData.Columns.Add("Account", "Account");
            dgvData.Columns.Add("CostPrice", "Cost Price");
            dgvData.Columns.Add("Description", "Description");

            // Align and format decimal columns
            dgvData.Columns["CostPrice"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvData.Columns["CostPrice"].DefaultCellStyle.Format = "N2"; 
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
            BindInvoices();
        }

        DataTable data;

        public void BindInvoices()
        {
            bool isLoading = false;
            string query = @"
                            SELECT 
                                ROW_NUMBER() OVER (ORDER BY t.date DESC) AS SN,
                                DATE_FORMAT(t.date, '%d/%m/%Y') AS `Date`,
                                t.reference AS `Ref Id`,
                                i.name AS `Name`,
                                i.code AS `Code`,
                                i.id AS `Item Id`,
                                (SELECT name FROM tbl_coa_level_4 WHERE id = i.asset_account_id) AS `Account`,
                                t.cost_price AS `Cost Price`,
                                t.description AS `Description`
                            FROM 
                                tbl_items i
                            LEFT JOIN 
                                tbl_item_transaction t ON t.id = (
                                    SELECT id 
                                    FROM tbl_item_transaction 
                                    WHERE item_id = i.id
                                    /**DATE_FILTER**/
                                    ORDER BY date DESC 
                                    LIMIT 1
                                )
                            WHERE 
                                i.item_type = 'Fixed Assets'
                                AND i.state = 0
                            ORDER BY t.date DESC;
                        ";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (!chkDate.Checked)
            {
                query = query.Replace("/**DATE_FILTER**/", "AND date BETWEEN @dateFrom AND @dateTo");

                parameters.Add(DBClass.CreateParameter("dateFrom", dtFrom.Value.Date));
                parameters.Add(DBClass.CreateParameter("dateTo", dtTo.Value.Date));
            }
            else
            {
                query = query.Replace("/**DATE_FILTER**/", ""); // remove placeholder
            }


            if (isLoading) return;
            isLoading = true;

            dgvData.Rows.Clear();

            Task.Run(() =>
            {
                data = new DataTable();

                this.Invoke(new Action(() =>
                {
                    data = DBClass.ExecuteDataTable(query, parameters.ToArray());
                }));

                int rowNumber = 0;
                foreach (DataRow reader in data.Rows)
                {
                    var row = new object[]
                    {
                        ++rowNumber,
                        reader["Date"],
                        reader["Ref Id"],
                        reader["Name"],
                        reader["Code"],
                        reader["Item Id"],
                        reader["Account"],
                        reader["Cost Price"],
                        reader["Description"]
                    };

                    this.Invoke(new Action(() =>
                    {
                        dgvData.Rows.Add(row);
                    }));
                }

                this.Invoke(new Action(() =>
                {
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
                    isLoading = false;
                }));
            });
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Rows.Count == 0)
                return;

            if(e.RowIndex < 0 || e.RowIndex >= dgvData.Rows.Count)
                return;
            if (dgvData.SelectedRows.Count == 0)
                return;
            if (dgvData.SelectedRows[0].Cells["RefId"].Value == null || string.IsNullOrWhiteSpace(dgvData.SelectedRows[0].Cells["RefId"].Value.ToString()))
                return;

            frmLogin.frmMain.openChildForm(new frmViewItem(int.Parse(dgvData.SelectedRows[0].Cells["RefId"].Value.ToString())));
        }

        private void dgvData_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDate.Checked)
                dtFrom.Enabled = dtTo.Enabled = false;
            else
                dtFrom.Enabled = dtTo.Enabled = true;
            
            BindInvoices();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (data == null || data.Rows.Count == 0)
                return;

            string searchText = txtSearch.Text.Trim().ToLower();

            dgvData.Rows.Clear();

            int sn = 0;
            foreach (DataRow row in data.Rows)
            {
                string name = row["Name"].ToString().ToLower();

                if (name.Contains(searchText))
                {
                    var newRow = new object[]
                    {
                ++sn,
                row["Date"],
                row["Ref Id"],
                row["Name"],
                row["Code"],
                row["Item Id"],
                row["Account"],
                row["Cost Price"],
                row["Description"]
                    };

                    dgvData.Rows.Add(newRow);
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ──────── 1. Create document & page ────────
            Document doc = new Document();
            Section section = doc.AddSection();
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);
            section.PageSetup.RightMargin = Unit.FromCentimeter(1);
            section.PageSetup.BottomMargin = Unit.FromCentimeter(1);
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;

            // ──────── 2. Header (time ‑ title ‑ date) ────────
            Table headerTable = section.AddTable();
            headerTable.Borders.Width = 0;
            headerTable.AddColumn("5cm");
            headerTable.AddColumn("8cm");
            headerTable.AddColumn("5cm");

            Row headerRow = headerTable.AddRow();

            // left cell – time/date
            Paragraph left = headerRow.Cells[0].AddParagraph();
            left.Format.Font.Name = "Times New Roman";
            left.Format.Font.Size = 10;
            headerRow.Cells[0].VerticalAlignment = VerticalAlignment.Top;
            left.AddText(DateTime.Now.ToString("hh:mm tt") + "\n" +
                         DateTime.Now.ToString("dd/MM/yyyy"));

            // centre cell – title
            Paragraph centre = headerRow.Cells[1].AddParagraph();
            centre.Format.Font.Name = "Times New Roman";
            centre.Format.Alignment = ParagraphAlignment.Center;
            centre.AddFormattedText("Fixed Asset Purchase Details\n", TextFormat.Bold).Font.Size = 12;
            centre.AddFormattedText("Generated Report", TextFormat.NotBold).Font.Size = 9;
            headerRow.Cells[1].VerticalAlignment = VerticalAlignment.Top;

            // separator line
            Paragraph line = section.AddParagraph();
            line.Format.Borders.Bottom.Width = 2;
            line.Format.SpaceAfter = "0.5cm";

            // ──────── 3. Data table ────────
            Table table = section.AddTable();
            table.Borders.Width = 0.75;

            // columns: Name | Date | Ref Id | Account | Cost Price | Description
            table.AddColumn("4cm");  // Name
            table.AddColumn("3cm");  // Date
            table.AddColumn("3cm");  // Ref Id
            table.AddColumn("4cm");  // Account
            table.AddColumn("3cm");  // Cost Price
            table.AddColumn("6cm");  // Description

            // header row
            Row hdr = table.AddRow();
            hdr.Shading.Color = Colors.LightGray;
            hdr.Cells[0].AddParagraph("Name").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[1].AddParagraph("Date").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[2].AddParagraph("Ref Id").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[3].AddParagraph("Account").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[4].AddParagraph("Cost Price").Format.Alignment = ParagraphAlignment.Center;
            hdr.Cells[5].AddParagraph("Description").Format.Alignment = ParagraphAlignment.Center;

            // ──────── 4. Fill rows from DataGridView ────────
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                if (row.IsNewRow) continue;

                Row tRow = table.AddRow();
                // Adjust column‑name strings if your Name properties differ
                tRow.Cells[0].AddParagraph(row.Cells["Name"].Value?.ToString() ?? "");
                tRow.Cells[1].AddParagraph(row.Cells["Date"].Value?.ToString() ?? "");
                tRow.Cells[2].AddParagraph(row.Cells["RefId"].Value?.ToString() ?? "");
                tRow.Cells[3].AddParagraph(row.Cells["Account"].Value?.ToString() ?? "");
                tRow.Cells[4].AddParagraph(
                    string.Format("{0:N2}", row.Cells["CostPrice"].Value))
                    .Format.Alignment = ParagraphAlignment.Right;
                tRow.Cells[5].AddParagraph(row.Cells["Description"].Value?.ToString() ?? "");

                // optional: unifying font for data cells
                for (int i = 0; i < 6; i++)
                {
                    tRow.Cells[i].Format.Font.Name = "Times New Roman";
                    tRow.Cells[i].Format.Font.Size = 9;
                }
            }

            // ──────── 5. Render and open PDF ────────
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            string path = Path.Combine(Environment.GetFolderPath(
                            Environment.SpecialFolder.Desktop), "FixedAssetPurchaseReport.pdf");
            renderer.PdfDocument.Save(path);
            Process.Start("explorer.exe", path);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "FixedAssetsReport_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                var excelApp = new Excel.Application();
                excelApp.Visible = false;
                var workbook = excelApp.Workbooks.Add(Type.Missing);
                var worksheet = (Excel.Worksheet)workbook.Sheets[1];
                worksheet.Name = "Fixed Assets";

                // Merge and format header row (Report Title)
                Excel.Range headerRange = worksheet.Range["A1", "G1"];
                headerRange.Merge();
                headerRange.Value = "Fixed Assets Report - " + DateTime.Now.ToString("dd MMM yyyy");
                headerRange.Font.Bold = true;
                headerRange.Font.Name = "Times New Roman";
                headerRange.Font.Size = 11;
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                // Define headers and field mapping
                string[] headers = { "SN", "Date", "Ref Id", "Name", "Account", "Cost Price", "Description" };
                string[] fieldNames = { "SN", "Date", "RefId", "Name", "Account", "CostPrice", "Description" };

                // Write column headers
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[2, i + 1] = headers[i];
                    var cell = worksheet.Cells[2, i + 1];
                    cell.Font.Bold = true;
                    cell.Font.Name = "Times New Roman";
                    cell.Font.Size = 10;
                    cell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);
                    cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                }

                // Write data rows
                int rowIndex = 3;
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.IsNewRow) continue;

                    for (int col = 0; col < fieldNames.Length; col++)
                    {
                        object value = row.Cells[fieldNames[col]].Value;
                        worksheet.Cells[rowIndex, col + 1] = value;

                        var cell = worksheet.Cells[rowIndex, col + 1];
                        cell.Font.Name = "Times New Roman";
                        cell.Font.Size = 9;

                        if (fieldNames[col] == "Cost Price")
                        {
                            if (decimal.TryParse(value?.ToString(), out decimal cost))
                                cell.Value = cost;
                            cell.NumberFormat = "#,##0.00";
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        }
                        else if (fieldNames[col] == "SN")
                        {
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        }
                        else
                        {
                            cell.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
                        }
                    }

                    rowIndex++;
                }

                // Auto-fit all columns
                for (int i = 1; i <= headers.Length; i++)
                {
                    ((Excel.Range)worksheet.Columns[i]).AutoFit();
                }

                // Save the file
                workbook.SaveAs(saveDialog.FileName);
                workbook.Close();
                excelApp.Quit();

                MessageBox.Show("Excel exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
