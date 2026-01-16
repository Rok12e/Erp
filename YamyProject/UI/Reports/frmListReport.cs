using DocumentFormat.OpenXml.Presentation;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject.UI.Reports
{
    public partial class frmListReport : Form
    {
        string type;
        public frmListReport(string _type)
        {
            InitializeComponent();
            this.type = _type;
            LbHeader.Text = type;
        }

        private void frmListReport_Load(object sender, EventArgs e)
        {
            BindData();
            LocalizationManager.LocalizeDataGridViewHeaders(dgvData);
        }

        public void BindData()
        {
            dgvData.Rows.Clear();
            string query = "";
            if (type == "Bank List") { query = "SELECT * FROM tbl_bank"; }
            else if (type == "Bank Card List") { query = "SELECT * FROM tbl_bank_card"; }
            else if (type == "Cheque List") { query = "SELECT * FROM tbl_cheque"; }
            else if (type == "City List") { query = "SELECT * FROM tbl_city"; }
            else if (type == "Country List") { query = "SELECT * FROM tbl_country"; }
            else if (type == "Cost Center List") { query = "SELECT * FROM tbl_Cost_Center"; }
            else if (type == "Customer List") { query = "SELECT * FROM tbl_customer"; }
            else if (type == "Customer Category List") { query = "SELECT * FROM tbl_customer_category"; }
            else if (type == "Department List") { query = "SELECT * FROM tbl_departments"; }
            else if (type == "Employee List") { query = "SELECT * FROM tbl_employee"; }
            else if (type == "Fixed Asset List") { query = "SELECT * FROM tbl_fixed_assets"; }
            else if (type == "Fixed Asset Category List") { query = "SELECT * FROM tbl_fixed_assets_category"; }
            else if (type == "FixedAsset Item List") { 
                query = "SELECT id, code, name, type, barcode, cost_price,sales_price, min_amount, max_amount, on_hand qty FROM tbl_items where item_type = 'Fixed Assets'"; 
            }
            else if (type == "Item List") { 
                query = "SELECT id, code, name, type, barcode, cost_price,sales_price, min_amount, max_amount, on_hand qty FROM tbl_items"; 
            }
            else if (type == "Item Category List") { query = "SELECT * FROM tbl_item_category"; }
            else if (type == "Item Tax Code List") { query = "SELECT * FROM tbl_tax"; }
            else if (type == "Item Unit List") { query = "SELECT * FROM tbl_unit"; }
            else if (type == "Item Warehouse List") { query = "SELECT * FROM tbl_warehouse"; }
            else if (type == "Petty Cash Category List") { query = "SELECT * FROM tbl_petty_cash_category"; }
            else if (type == "Position List") { query = "SELECT * FROM tbl_position"; }
            else if (type == "Prepaid Expense List") { query = "SELECT * FROM tbl_prepaid_expense"; }
            else if (type == "Prepaid Expense Category List") { query = "SELECT * FROM tbl_prepaid_expense_category"; }
            else if (type == "Vendor List") { query = "SELECT * FROM tbl_vendor"; }
            else if (type == "Vendor Category List") { query = "SELECT * FROM tbl_vendor_category"; }

            if (string.IsNullOrWhiteSpace(query))
                return;

            DataTable dt = DBClass.ExecuteDataTable(query);
            dgvData.DataSource = dt;
            try
            {
                dgvData.Columns["id"].Visible = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvData.CurrentRow.Cells["id"].Value == null || string.IsNullOrWhiteSpace(dgvData.CurrentRow.Cells["id"].Value.ToString()))
                return;

            int refId = int.Parse(dgvData.CurrentRow.Cells["id"].Value.ToString());
            Form form = null;

            switch (type)
            {
                case "Bank List":
                    form = new frmBankRegister(refId);
                    break;
                case "Bank Card List":
                    form = new frmViewBankCard(refId);
                    break;
                case "Cheque List":
                    form = new frmViewCheque(refId);
                    break;
                case "City List":
                    form = new frmViewCity(refId);
                    break;
                //case "Country List":
                //    form = new frmCountry();
                //    break;
                case "Cost Center List":
                    form = new frmCostCenter(refId);
                    break;
                case "Customer List":
                    form = new frmViewCustomer(refId);
                    break;
                case "Customer Category List":
                    form = new frmViewCustomerCategory(refId);
                    break;
                case "Department List":
                    form = new frmViewDepartments(refId);
                    break;
                case "Employee List":
                    form = new frmViewEmployee(refId);
                    break;
                case "Fixed Asset List":
                    form = new frmViewFixedAssets(refId);
                    break;
                case "Fixed Asset Category List":
                    form = new frmViewFixedAssetsCategory(refId);
                    break;
                case "FixedAsset Item List":
                    form = new frmViewItem(refId); // or a specific form like frmFixedAssetItem
                    break;
                case "Item List":
                    form = new frmViewItem(refId);
                    break;
                case "Item Category List":
                    form = new frmViewCategory(refId);
                    break;
                case "Item Tax Code List":
                    form = new frmViewItemTaxCodes(refId);
                    break;
                case "Item Unit List":
                    form = new frmViewItemUnit(refId);
                    break;
                case "Item Warehouse List":
                    form = new frmViewWarehouse(refId);
                    break;
                case "Petty Cash Category List":
                    form = new frmPettyCashCategory(refId);
                    break;
                case "Position List":
                    form = new frmViewPosition(refId);
                    break;
                case "Prepaid Expense List":
                    form = new frmViewPrepaidExpense(refId);
                    break;
                case "Prepaid Expense Category List":
                    form = new frmViewPrepaidExpenseCategory(refId);
                    break;
                case "Vendor List":
                    form = new frmViewVendor(refId);
                    break;
                case "Vendor Category List":
                    form = new frmViewVendorCategory(refId);
                    break;
            }

            if (form != null)
            {
                form.Show();
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnPDF_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a MigraDoc document
            Document doc = new Document();
            Section section = doc.AddSection();

            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            section.PageSetup.TopMargin = Unit.FromCentimeter(1);
            section.PageSetup.LeftMargin = Unit.FromCentimeter(1);

            Paragraph heading = section.AddParagraph("Grid Data Export");
            heading.Format.Font.Size = 14;
            heading.Format.Font.Bold = true;
            heading.Format.SpaceAfter = "1cm";

            Table table = section.AddTable();
            table.Borders.Width = 0.75;

            // Add columns from DataGridView
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                if (col.Visible)
                {
                    Column column = table.AddColumn(Unit.FromCentimeter(4));
                    column.Format.Alignment = ParagraphAlignment.Left;
                }
            }

            // Add header row
            Row headerRow = table.AddRow();
            headerRow.Shading.Color = Colors.LightGray;
            int headerIndex = 0;
            foreach (DataGridViewColumn col in dgvData.Columns)
            {
                if (col.Visible)
                {
                    headerRow.Cells[headerIndex].AddParagraph(col.HeaderText);
                    headerRow.Cells[headerIndex].Format.Font.Bold = true;
                    headerIndex++;
                }
            }

            // Add data rows
            foreach (DataGridViewRow dgvRow in dgvData.Rows)
            {
                if (dgvRow.IsNewRow) continue;
                Row row = table.AddRow();
                int cellIndex = 0;

                foreach (DataGridViewCell cell in dgvRow.Cells)
                {
                    if (dgvData.Columns[cell.ColumnIndex].Visible)
                    {
                        row.Cells[cellIndex].AddParagraph(Convert.ToString(cell.Value));
                        cellIndex++;
                    }
                }
            }

            // Render the document to PDF
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
            renderer.Document = doc;
            renderer.RenderDocument();

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                FileName = type.Trim().Replace(" ","")+".pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    renderer.PdfDocument.Save(sfd.FileName);
                    MessageBox.Show("PDF saved successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnExcel_Click(object sender, EventArgs e)
        {
            if (dgvData.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = type.Trim().Replace(" ", "") + ".xlsx"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var excelApp = new Microsoft.Office.Interop.Excel.Application();
                    excelApp.Visible = false;
                    var workbook = excelApp.Workbooks.Add(Type.Missing);
                    var worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets[1];
                    worksheet.Name = type.Trim();

                    int colIndex = 1;
                    foreach (DataGridViewColumn col in dgvData.Columns)
                    {
                        if (col.Visible)
                        {
                            worksheet.Cells[1, colIndex] = col.HeaderText;
                            ((Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, colIndex]).Font.Bold = true;
                            colIndex++;
                        }
                    }

                    int rowIndex = 2;
                    foreach (DataGridViewRow row in dgvData.Rows)
                    {
                        if (row.IsNewRow) continue;
                        colIndex = 1;
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            if (dgvData.Columns[cell.ColumnIndex].Visible)
                            {
                                worksheet.Cells[rowIndex, colIndex] = Convert.ToString(cell.Value);
                                colIndex++;
                            }
                        }
                        rowIndex++;
                    }

                    // Auto-fit columns
                    worksheet.Columns.AutoFit();

                    // Save and release
                    workbook.SaveAs(sfd.FileName);
                    workbook.Close(false);
                    excelApp.Quit();

                    // Release COM objects
                    Marshal.ReleaseComObject(worksheet);
                    Marshal.ReleaseComObject(workbook);
                    Marshal.ReleaseComObject(excelApp);

                    MessageBox.Show("Excel file saved successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            //maximize the form
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // minimize the form
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
        }
    }
}
