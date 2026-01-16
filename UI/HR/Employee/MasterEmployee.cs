using MySql.Data.MySqlClient;
using Novacode;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class MasterEmployee : Form
    {
        private DataView _dataView;
        private EventHandler employeeUpdateHandler;

        public MasterEmployee()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            employeeUpdateHandler = (sender, args) => BindEmployee();
            EventHub.Employee += employeeUpdateHandler;

            headerUC1.FormText = Text;
        }
        private void MasterEmployee_Load(object sender, EventArgs e)
        {
            ConfigureDataGridViews();
            BindEmployee();
            pnlInfo.Height = 80;
            //btnExpand.Text = "More";
            headerUC1.FormText = "Employee Center";
        }
        private void MasterEmployee_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Employee -= employeeUpdateHandler;
        }
        private void ConfigureDataGridViews()
        {
            dgvTransactions.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvTransactions.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.RowsDefaultCellStyle.BackColor = Color.White;
            dgvTransactions.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            dgvTransactions.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#D1EAD0");
            dgvTransactions.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvTransactions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dgvTransactions.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.AllowUserToAddRows = dgvTransactions.AllowUserToResizeRows = false;
        }
        public void BindEmployee()
        {
            // Start building the base query
            string query = @"
                               SELECT 
                                c.id, 
                                c.code,
                                c.name AS Name,
                                p.name AS Position, 
                                c.Position_id,
                                c.address,
                                c.email,
                                c.phone,
                                COALESCE(SUM(
                                    CASE 
                                            WHEN t.type = 'Employee Salary Payment' THEN -IF(t.debit = 0, t.credit, t.debit)
                                            WHEN t.type = 'Petty Cash' THEN -IF(t.debit = 0, t.credit, t.debit)
                                            WHEN t.type = 'Employee Loan Payment' THEN 0 
                                            WHEN t.type = 'Check Cancel (Employee)' THEN t.credit
                                            WHEN t.type = 'Employee Salary' THEN IF(t.debit = 0, t.credit, t.debit)
                                            ELSE 0 
                                        END
                                    ), 0) AS Amount
                                FROM tbl_employee c
                                LEFT JOIN tbl_position p ON c.Position_id = p.id  
                                LEFT JOIN tbl_transaction t ON t.hum_id = c.id AND t.state = 0
                            ";

            // Add filter conditionally based on combo box
            if (cmbState.Text == "Active Employee")
            {
                query += " WHERE c.active = 0";
            }
            else if (cmbState.Text == "Inactive Employee")
            {
                query += " WHERE c.active != 0";
            }

            // Final GROUP BY (must match selected fields except aggregates)
            query += @"
                        GROUP BY 
                            c.id, 
                            c.code, 
                            c.name, 
                            p.name, 
                            c.Position_id,
                            c.address, 
                            c.email, 
                            c.phone;
                ";

            // Execute and bind to DataGridView
            DataTable dt = DBClass.ExecuteDataTable(query);
            _dataView = dt.DefaultView;
            dgvCustomer.DataSource = _dataView;

            // Format DataGridView
            dgvCustomer.Columns["id"].Visible = false;
            dgvCustomer.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCustomer.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            HideUnusedColumns();
            if (dgvCustomer.Rows.Count > 0)
            {
                BindEmployeeInvoice();

                editCustomerToolStripMenuItem1.Visible = UserPermissions.canEdit("Human Resource Center");
                deleteEmployeeToolStripMenuItem.Visible = UserPermissions.canDelete("Human Resource Center");
            }
            else
                ClearEmployeerDetails();

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }
        private void HideUnusedColumns()
        {
            string[] hiddenColumns = { "id", "code","Position_id", "Position", "phone", "email", "address", "email"};
            foreach (var col in hiddenColumns)
                dgvCustomer.Columns[col].Visible = false;
        }
        private void BindEmployeeInvoice()
        {
            BindCustomerInfo();
            //BindInvoice(new List<string> { "%Purchase Invoice Cash%" }, new List<string>(), dgvInvoiceCash);
            //BindInvoice(new List<string> { "Vendor%", "Purchase%" , "Check Cancel (Vendor)" }, new List<string> { "%Purchase Invoice Cash%" }, dgvInvoiceCredit);
            BindInvoice();
            AdjustColumnWidths();
        }
        private void BindInvoice()
        {
            List<string> includeTypes = new List<string> { "Employee%", "Employee Loan Payment%", "Employee Salary Payment%", "Check Cancel (Employee)", "%Employee Salary%" };
            List<string> excludeTypes = new List<string> { };

            string query = @"WITH CTE AS (
                            SELECT 
                                t.id,
                                t.transaction_id AS `Invoice Id`,
                                ROW_NUMBER() OVER (ORDER BY t.date, t.id) AS SN,
                                t.voucher_no AS `V - No`,
                                t.date AS `Date`,
                                CONCAT(ta.code, ' - ', ta.name) AS `Account Name`,
                                t.type AS `Type`,
                                CASE
                                    WHEN t.type = 'Employee Salary Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Petty Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Check Cancel (Employee)' THEN t.credit
                                    WHEN t.type = 'Employee Salary' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Employee Loan Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Employee Petty Cash Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                    ELSE 0
                                END AS `Amount`,
                                CASE
                                    WHEN t.type = 'Employee Salary Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Petty Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Employee Loan Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                     WHEN t.type = 'Employee Petty Cash Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                    WHEN t.type = 'Check Cancel (Employee)' THEN -t.credit
                                    ELSE 0
                                END AS `Paid Amt`
                
                            FROM tbl_transaction t
                            INNER JOIN tbl_coa_level_4 ta ON t.account_id = ta.id
                            WHERE t.hum_id = @id AND t.state = 0
                              AND (
                                    t.type LIKE 'Employee%' OR 
                                    t.type = 'Employee Loan Payment' OR 
                                    t.type = 'Employee Salary Payment' OR 
                                    t.type = 'Petty Cash' OR 
                                    t.type = 'Check Cancel (Employee)' OR
                                    t.type = 'Employee Petty Cash Payment' OR 
                                    t.type LIKE 'Employee Salary'
                                  )
                        ),
                        Running_Balance AS (
                            SELECT *,
                                SUM(`Amount` - `Paid Amt`) OVER (ORDER BY `Date`, id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS `Balance`
                            FROM CTE
                        )
                        SELECT 
                            `id`,
                            `Invoice Id`,
                            `SN` as `SN#`,
                            `V - No` as No,
                            `Date`,
                            `Account Name`,
                            `Type`,
                            `Amount`,
                            `Paid Amt`,
                            `Balance`
                        FROM Running_Balance
                        ORDER BY `Date`, id;";

            var parameters = new List<MySqlParameter> { new MySqlParameter("@id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()) };
            
            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            decimal totalAmount = 0, totalPaidAmount = 0, amount = 0, paidAmount = 0;
            string _type = "";
            foreach (DataRow row in dt.Rows)
            {
                _type = string.IsNullOrEmpty(row["Type"]?.ToString()) ? "" : row["Type"]?.ToString();

                if (decimal.TryParse(row["Amount"]?.ToString(), out amount))
                {
                    totalAmount += amount;
                }
                if (decimal.TryParse(row["Paid Amt"]?.ToString(), out paidAmount))
                {
                    if (_type == "Employee Payment"|| _type== "Employee Salary Payment" || _type == "Petty Cash")
                    {
                        totalPaidAmount += paidAmount;
                        totalAmount = totalAmount - paidAmount;
                    }
                    else
                    {
                        decimal _paid = paidAmount;// (paidAmount == 0 ? amount : 0);
                        totalPaidAmount += _paid;
                        //row["Paid Amt"] = _paid.ToString("N2");
                    }
                }
                row["Balance"] = (totalAmount - totalPaidAmount).ToString("F2");
            }
            //DataRow totalRow = dt.NewRow();
            //totalRow["Account Name"] = "Total";
            //totalRow["Amount"] = totalAmount.ToString("F2");
            //dt.Rows.Add(totalRow);
            dgvTransactions.DataSource = dt;
            dgvTransactions.Columns["Amount"].DefaultCellStyle.Alignment = dgvTransactions.Columns["Paid Amt"].DefaultCellStyle.Alignment = dgvTransactions.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTransactions.Columns["Account Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvTransactions.Columns["Amount"].DefaultCellStyle.Format = dgvTransactions.Columns["Paid Amt"].DefaultCellStyle.Format = dgvTransactions.Columns["Balance"].DefaultCellStyle.Format = "N2";
            foreach (DataGridViewColumn column in dgvTransactions.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            LocalizationManager.LocalizeDataGridViewHeaders(dgvTransactions);
        }
        private void AdjustColumnWidths()
        {
            dgvTransactions.Columns["id"].Visible = dgvTransactions.Columns["Invoice Id"].Visible = false;
            dgvTransactions.Columns["No"].Width = 80;
            dgvTransactions.Columns["Amount"].Width = dgvTransactions.Columns["Amount"].Width = 90;
            dgvTransactions.Columns["Paid Amt"].Width = 90;
            dgvTransactions.Columns["Balance"].Width = 100;
            dgvTransactions.Columns["Balance"].DefaultCellStyle.Format = "N2";
        }

        private void BindCustomerInfo()
        {
            var row = dgvCustomer.SelectedRows[0].Cells;
            lblAddress.Text = string.IsNullOrWhiteSpace(row["address"].Value?.ToString()) ? "-" : row["address"].Value.ToString();
            lblEmail.Text = string.IsNullOrWhiteSpace(row["Email"].Value?.ToString()) ? "-" : row["Email"].Value.ToString();
            lblPhone.Text = string.IsNullOrWhiteSpace(row["phone"].Value?.ToString()) ? "-" : row["phone"].Value.ToString();
            lblPosition.Text = string.IsNullOrWhiteSpace(row["Position"].Value?.ToString()) ? "-" : row["Position"].Value.ToString();

            lblCode.Text = row["code"].Value?.ToString().Split('-')[0];
            string nameValue = row["name"].Value?.ToString();
            var splitName = nameValue.Split('-');
            lblCode.Text = splitName.Length > 0 ? splitName[0].Trim() : "-";
            lblName.Text = splitName.Length > 1 ? splitName[1].Trim() : nameValue; // fallback to full name if not split

        }
        private void ClearEmployeerDetails()
        {
            dgvTransactions.DataSource = null;
            lblCode.Text = lblAddress.Text = lblPhone.Text = lblEmail.Text = lblName.Text = lblPosition.Text = lblPosition.Text = "-";
        }
        private void dgvInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTransactions.Rows.Count)
            {
                if (dgvTransactions.Columns.Contains("type"))
                {
                    if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Purchase")
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["Invoice Id"].Value.ToString())));
                    }
                    else if (dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString() == ("Purchase Invoice Cash") || dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString() == ("Purchase Invoice"))
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchaseReturn(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["Invoice Id"].Value.ToString())));
                    }
                    //if (dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString().ToUpper().Contains("PURCHASE"))
                    //    frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["Invoice Id"].Value.ToString())));
                }
            }
        }
        private void dgvInvoiceCash_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0 && e.RowIndex < dgvTransactions.Rows.Count)
            //{
            //    if (dgvTransactions.Columns.Contains("type"))
            //    {
            //        if (dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString().ToUpper().Contains("PURCHASE"))
            //            frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvInvoiceCash.Rows[e.RowIndex].Cells["Invoice Id"].Value.ToString())));
            //    }
            //}
        }
        private void btnExpand_Click(object sender, EventArgs e)
        {
            //if (pnlInfo.Height == 80)
            //{
            //    pnlInfo.Height = 240;
            //    btnExpand.Text = "Less";
            //}
            //else
            //{
            //    pnlInfo.Height = 80;
            //    btnExpand.Text = "More";
            //}
        }
        private void createInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmPurchase(0,"",int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += (s, ev) =>
            {
                int leftMargin = 50;
                int rightMargin = 50;
                int yPosition = 50;
                int lineHeight = 30;
                int pageWidth = ev.PageBounds.Width;
                int availableWidth = pageWidth - leftMargin - rightMargin;

                Font headerFont = new Font("Times New Roman", 16, FontStyle.Bold);
                Font columnHeaderFont = new Font("Times New Roman", 10, FontStyle.Bold);
                Font cellFont = new Font("Times New Roman", 10);

                // Center title
                string[] headerLines = {
                                            "Employee Data Report",
                                            " Employee ",
                                            "All Employee"
                                        };
                foreach (string line in headerLines)
                {
                    SizeF textSize = ev.Graphics.MeasureString(line, headerFont);
                    ev.Graphics.DrawString(line, headerFont, Brushes.Black, new PointF((pageWidth - textSize.Width) / 2, yPosition));
                    yPosition += (int)textSize.Height + 5;
                }

                yPosition += 10;
                string dateText = "Printed on: " + DateTime.Now.ToString("MM/dd/yyyy");
                ev.Graphics.DrawString(dateText, new Font("Segoe UI", 10), Brushes.Black, new PointF(leftMargin, yPosition));
                yPosition += 25;

                // Column widths
                int nameWidth = (int)(availableWidth * 0.6);
                int amountWidth = availableWidth - nameWidth;
                int colTop = yPosition;

                // Draw column headers
                ev.Graphics.FillRectangle(Brushes.White, leftMargin, yPosition, nameWidth, lineHeight);
                ev.Graphics.FillRectangle(Brushes.White, leftMargin + nameWidth, yPosition, amountWidth, lineHeight);

                StringFormat centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                ev.Graphics.DrawString("Name", columnHeaderFont, Brushes.Black, new RectangleF(leftMargin, yPosition, nameWidth, lineHeight), centerFormat);
                ev.Graphics.DrawString("Amount", columnHeaderFont, Brushes.Black, new RectangleF(leftMargin + nameWidth, yPosition, amountWidth, lineHeight), centerFormat);

                ev.Graphics.DrawRectangle(Pens.Black, leftMargin, yPosition, nameWidth, lineHeight);
                ev.Graphics.DrawRectangle(Pens.Black, leftMargin + nameWidth, yPosition, amountWidth, lineHeight);

                yPosition += lineHeight;

                // Row text formats
                //StringFormat nameFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };     // RIGHT
                StringFormat nameFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                StringFormat amountFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center }; // LEFT

                // Print rows
                foreach (DataGridViewRow row in dgvCustomer.Rows)
                {
                    if (row.IsNewRow) continue;

                    string name = row.Cells["Name"].Value?.ToString() ?? "";
                    string amount = row.Cells["Amount"].FormattedValue?.ToString() ?? "";

                    // Draw name (right aligned)
                    ev.Graphics.DrawString(name, cellFont, Brushes.Black,
                        new RectangleF(leftMargin, yPosition, nameWidth, lineHeight), nameFormat);

                    // Draw amount (left aligned)
                    ev.Graphics.DrawString(amount, cellFont, Brushes.Black,
                        new RectangleF(leftMargin + nameWidth, yPosition, amountWidth, lineHeight), amountFormat);

                    // Draw borders
                    ev.Graphics.DrawRectangle(Pens.Black, leftMargin, yPosition, nameWidth, lineHeight);
                    ev.Graphics.DrawRectangle(Pens.Black, leftMargin + nameWidth, yPosition, amountWidth, lineHeight);

                    yPosition += lineHeight;

                    if (yPosition > ev.MarginBounds.Bottom)
                    {
                        ev.HasMorePages = true;
                        return;
                    }
                }

                ev.HasMorePages = false;
            };

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 1000,
                Height = 800
            };
            previewDialog.ShowDialog();
        }

        private void customerListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save Excel File"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelExporter exporter = new ExcelExporter(dgvCustomer);
                exporter.ExportToExcel(saveFileDialog.FileName);
            }
        }
        private void exportTransactionListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Save Excel File"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExcelExporter exporter = new ExcelExporter(dgvTransactions);
                exporter.ExportToExcel(saveFileDialog.FileName);
            }
        }
        private void customerListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Save Word Document"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenXmlWordExporter wordExporter = new OpenXmlWordExporter();
                wordExporter.ExportToWord(dgvCustomer, saveFileDialog.FileName);
            }
        }
        private void transactionListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Save Word Document"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenXmlWordExporter wordExporter = new OpenXmlWordExporter();
                wordExporter.ExportToWord(dgvTransactions, saveFileDialog.FileName);
            }
        }
        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEmployee();
        }
        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewEmployee(0));
        }
        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewEmployee(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //BindInvoice(new List<string> { "%Purchase Invoice Cash%" }, new List<string>(), dgvInvoiceCash);
            //BindInvoice(new List<string> { "Vendor%", "Purchase%" ,"Check Cancel"}, new List<string> { "%Purchase Invoice Cash%" }, dgvTransactions);
            BindCustomerInfo();
            BindInvoice();
            ConfigureDataGridViews();
            AdjustColumnWidths();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _dataView.RowFilter = "name like '%" + txtSearch.Text + "%'";
        }
        string GetCompanyName()
        {
            return DBClass.ExecuteScalar("select name from tbl_company LIMIT 1")?.ToString() ?? "";
        }
        private void creditPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            string companyName = GetCompanyName(); // from DB
            string customerName = dgvCustomer.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "Unknown Employee";

            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.Landscape = true;

            printDocument.PrintPage += (s, ev) =>
            {
                int leftMargin = 50;
                int rightMargin = 50;
                int topMargin = 50;
                int yPosition = topMargin;
                int lineHeight = 40;
                int pageWidth = ev.PageBounds.Width;
                int availableWidth = pageWidth - leftMargin - rightMargin;

                // Fonts
                Font companyFont = new Font("Times New Roman", 18, FontStyle.Bold);
                Font subTitleFont = new Font("Times New Roman", 14, FontStyle.Bold);
                Font labelFont = new Font("Times New Roman", 11, FontStyle.Regular);
                Font colFont = new Font("Times New Roman", 11, FontStyle.Bold);
                Font cellFont = new Font("Times New Roman", 11);

                // 1. Time and Date (Top-left)
                string timeNow = DateTime.Now.ToString("hh:mm tt");
                string dateNow = DateTime.Now.ToString("dd/MM/yyyy");

                ev.Graphics.DrawString(timeNow, labelFont, Brushes.Black, leftMargin, yPosition);
                yPosition += (int)labelFont.GetHeight(ev.Graphics);

                ev.Graphics.DrawString(dateNow, labelFont, Brushes.Black, leftMargin, yPosition);
                yPosition += (int)labelFont.GetHeight(ev.Graphics);

                // 2. Company Name (Center)
                SizeF companySize = ev.Graphics.MeasureString(companyName, companyFont);
                ev.Graphics.DrawString(companyName, companyFont, Brushes.Black,
                    new PointF((pageWidth - companySize.Width) / 2, topMargin)); // align with original top

                // 3. Subtitle
                yPosition += 10;
                string subtitle = "All Transactions for " + customerName;
                SizeF subtitleSize = ev.Graphics.MeasureString(subtitle, subTitleFont);
                ev.Graphics.DrawString(subtitle, subTitleFont, Brushes.Black,
                    new PointF((pageWidth - subtitleSize.Width) / 2, yPosition));
                yPosition += (int)subtitleSize.Height + 10;

                // 4. Horizontal bold line
                Pen boldPen = new Pen(Color.Black, 4);
                ev.Graphics.DrawLine(boldPen, leftMargin, yPosition, pageWidth - rightMargin, yPosition);
                yPosition += 15;

                // 5. Column definitions
                string[] columnNames = { "SN#", "No", "Date", "Account Name", "Type", "Amount", "Balance" };

                Dictionary<string, float> widthRatios = new Dictionary<string, float>
    {
        { "SN#", 0.06f },
        { "No", 0.08f },
        { "Date", 0.10f },
        { "Account Name", 0.24f },
        { "Type", 0.18f },
        { "Amount", 0.17f },
        { "Balance", 0.17f }
    };

                Dictionary<string, int> columnWidths = new Dictionary<string, int>();
                foreach (var col in columnNames)
                {
                    columnWidths[col] = (int)(availableWidth * widthRatios[col]);
                }

                // 6. Draw Table Headers
                int rowX = leftMargin;
                foreach (string col in columnNames)
                {
                    ev.Graphics.DrawString(col, colFont, Brushes.Black,
                        new RectangleF(rowX, yPosition, columnWidths[col], lineHeight),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });

                    ev.Graphics.DrawRectangle(Pens.Black, rowX, yPosition, columnWidths[col], lineHeight);
                    rowX += columnWidths[col];
                }
                yPosition += lineHeight;

                // 7. Draw Table Rows
                foreach (DataGridViewRow row in dgvTransactions.Rows)
                {
                    if (row.IsNewRow) continue;
                    rowX = leftMargin;

                    foreach (string col in columnNames)
                    {
                        string value = row.Cells[col].FormattedValue?.ToString() ?? "";
                        RectangleF cellBounds = new RectangleF(rowX, yPosition, columnWidths[col], lineHeight);

                        StringFormat format = new StringFormat { LineAlignment = StringAlignment.Center };
                        if (col == "Amount" || col == "Balance")
                            format.Alignment = StringAlignment.Far;
                        else
                            format.Alignment = StringAlignment.Near;

                        ev.Graphics.DrawString(value, cellFont, Brushes.Black, cellBounds, format);
                        ev.Graphics.DrawRectangle(Pens.Black, rowX, yPosition, columnWidths[col], lineHeight);
                        rowX += columnWidths[col];
                    }

                    yPosition += lineHeight;

                    if (yPosition + lineHeight > ev.MarginBounds.Bottom)
                    {
                        ev.HasMorePages = true;
                        return;
                    }
                }

                ev.HasMorePages = false;
            };

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 1000,
                Height = 800
            };
            previewDialog.ShowDialog();
        }
        private void cashToolStripMenuItem_Click(object sender, EventArgs e)
        {
        //    // Get selected customer details
        //    string customerCode = "-", customerName = "-";
        //    if (dgvCustomer.SelectedRows.Count > 0)
        //    {
        //        string fullName = dgvCustomer.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";
        //        var parts = fullName.Split(new[] { '-' }, 2);
        //        if (parts.Length == 2)
        //        {
        //            customerCode = parts[0].Trim();
        //            customerName = parts[1].Trim();
        //        }
        //        else
        //        {
        //            customerName = fullName;
        //        }
        //    }

        //    string companyName = GetCompanyName(); // Company name from DB

        //    PrintDocument printDocument = new PrintDocument();
        //    printDocument.DefaultPageSettings.Landscape = true;

        //    printDocument.PrintPage += (s, ev) =>
        //    {
        //        int leftMargin = 50;
        //        int rightMargin = 50;
        //        int topMargin = 50;
        //        int yPosition = topMargin;
        //        int lineHeight = 30;
        //        int pageWidth = ev.PageBounds.Width;
        //        int availableWidth = pageWidth - leftMargin - rightMargin;

        //        ev.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        //        // Fonts
        //        Font companyFont = new Font("Times New Roman", 18, FontStyle.Bold);
        //        Font subTitleFont = new Font("Times New Roman", 14, FontStyle.Bold);
        //        Font titleFont = new Font("Times New Roman", 13, FontStyle.Bold);
        //        Font colFont = new Font("Times New Roman", 11, FontStyle.Bold);
        //        Font cellFont = new Font("Times New Roman", 11);
        //        Font labelFont = new Font("Times New Roman", 11);

        //        // Time & Date (Top Left)
        //        string timeNow = DateTime.Now.ToString("hh:mm tt");
        //        string dateNow = DateTime.Now.ToString("dd/MM/yyyy");

        //        ev.Graphics.DrawString(timeNow, labelFont, Brushes.Black, leftMargin, yPosition);
        //        yPosition += (int)labelFont.GetHeight(ev.Graphics);
        //        ev.Graphics.DrawString(dateNow, labelFont, Brushes.Black, leftMargin, yPosition);
        //        yPosition += (int)labelFont.GetHeight(ev.Graphics) + 10;

        //        // Company Name (Centered)
        //        SizeF companySize = ev.Graphics.MeasureString(companyName, companyFont);
        //        ev.Graphics.DrawString(companyName, companyFont, Brushes.Black,
        //            new PointF((pageWidth - companySize.Width) / 2, topMargin));

        //        // SubTitle
        //        string subTitle = "All Transactions for " + customerCode + " - " + customerName;
        //        SizeF subSize = ev.Graphics.MeasureString(subTitle, subTitleFont);
        //        ev.Graphics.DrawString(subTitle, subTitleFont, Brushes.Black,
        //            new PointF((pageWidth - subSize.Width) / 2, yPosition));
        //        yPosition += (int)subSize.Height + 15;

        //        // Report Title
        //        SizeF reportTitleSize = ev.Graphics.MeasureString("Cash Invoice Report", titleFont);
        //        ev.Graphics.DrawString("Cash Invoice Report", titleFont, Brushes.Black,
        //            new PointF((pageWidth - reportTitleSize.Width) / 2, yPosition));
        //        yPosition += (int)reportTitleSize.Height + 10;

        //        // Horizontal Line
        //        Pen boldPen = new Pen(Color.Black, 3);
        //        ev.Graphics.DrawLine(boldPen, leftMargin, yPosition, pageWidth - rightMargin, yPosition);
        //        yPosition += 15;

        //        // Table Layout
        //        string[] columns = { "SN", "V - No", "Date", "Account Name", "Amount" };
        //        Dictionary<string, float> widthRatio = new Dictionary<string, float>
        //{
        //    { "SN", 0.08f },
        //    { "V - No", 0.12f },
        //    { "Date", 0.14f },
        //    { "Account Name", 0.46f },
        //    { "Amount", 0.20f }
        //};
        //        Dictionary<string, int> widths = columns.ToDictionary(c => c, c => (int)(availableWidth * widthRatio[c]));

        //        // Draw headers
        //        int x = leftMargin;
        //        foreach (var col in columns)
        //        {
        //            ev.Graphics.DrawString(col, colFont, Brushes.Black,
        //                new RectangleF(x, yPosition, widths[col], lineHeight),
        //                new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        //            ev.Graphics.DrawRectangle(Pens.Black, x, yPosition, widths[col], lineHeight);
        //            x += widths[col];
        //        }
        //        yPosition += lineHeight;

        //        // Draw data rows
        //        foreach (DataGridViewRow row in dgvInvoiceCash.Rows)
        //        {
        //            if (row.IsNewRow) continue;
        //            x = leftMargin;

        //            foreach (var col in columns)
        //            {
        //                string text = row.Cells[col].FormattedValue?.ToString() ?? "";
        //                RectangleF bounds = new RectangleF(x, yPosition, widths[col], lineHeight);

        //                StringFormat format = new StringFormat { LineAlignment = StringAlignment.Center };
        //                if (col == "Amount")
        //                    format.Alignment = StringAlignment.Far;
        //                else
        //                    format.Alignment = StringAlignment.Near;

        //                ev.Graphics.DrawString(text, cellFont, Brushes.Black, bounds, format);
        //                ev.Graphics.DrawRectangle(Pens.Black, x, yPosition, widths[col], lineHeight);
        //                x += widths[col];
        //            }

        //            yPosition += lineHeight;

        //            if (yPosition + lineHeight > ev.MarginBounds.Bottom)
        //            {
        //                ev.HasMorePages = true;
        //                return;
        //            }
        //        }

        //        ev.HasMorePages = false;
        //    };

        //    PrintPreviewDialog previewDialog = new PrintPreviewDialog
        //    {
        //        Document = printDocument,
        //        Width = 1000,
        //        Height = 800
        //    };
        //    previewDialog.ShowDialog();
        }
        private void ExportToExcel(DataGridView dgvTransactions, string customerName, decimal customerBalance)
        {
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!");
                return;
            }

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            int row = 1;

            // Customer Info
            xlWorkSheet.Cells[row, 1] = "Employee Name:";
            xlWorkSheet.Cells[row, 2] = customerName;
            row++;
            xlWorkSheet.Cells[row, 1] = "Balance:";
            xlWorkSheet.Cells[row, 2] = customerBalance.ToString("N2");
            row += 2;

            // Column Headers
            for (int i = 2; i < dgvTransactions.Columns.Count; i++)
            {
                xlWorkSheet.Cells[row, i - 1] = dgvTransactions.Columns[i].HeaderText;
            }
            row++;

            // Data Rows
            for (int i = 0; i < dgvTransactions.Rows.Count; i++)
            {
                for (int j = 2; j < dgvTransactions.Columns.Count; j++)
                {
                    var cellValue = dgvTransactions.Rows[i].Cells[j].Value;
                    xlWorkSheet.Cells[row + i, j - 1] = cellValue?.ToString();
                }
            }

            xlWorkSheet.Columns.AutoFit();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                xlWorkBook.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Exported Successfully!");
            }

            xlWorkBook.Close(false);
            xlApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a Employee first.");
                return;
            }

            try
            {
                string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

                // Sum of all Balance values
                decimal customerBalance = 0;
                foreach (DataGridViewRow row in dgvTransactions.Rows)
                {
                    decimal value;
                    if (row.Cells["Balance"].Value != null && decimal.TryParse(row.Cells["Balance"].Value.ToString(), out value))
                    {
                        customerBalance += value;
                    }
                }

                ExportToExcel(dgvTransactions, customerName, customerBalance);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
        }
        private void ExportCashInvoiceToExcel(DataGridView dgv, string customerName)
        {
            //Excel.Application xlApp = new Excel.Application();
            //if (xlApp == null)
            //{
            //    MessageBox.Show("Excel is not properly installed!");
            //    return;
            //}

            //Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
            //Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            //int row = 1;

            //// Header info
            //xlWorkSheet.Cells[row, 1] = "Employee Name:";
            //xlWorkSheet.Cells[row, 2] = customerName;
            //row += 2;

            //// Get column indexes to export (skip "id", "Invoice Id", and "Balance")
            //List<int> exportColumns = new List<int>();
            //for (int i = 0; i < dgv.Columns.Count; i++)
            //{
            //    string colName = dgv.Columns[i].HeaderText.ToLower();
            //    if (colName != "id" && colName != "invoice id" && colName != "balance")
            //    {
            //        exportColumns.Add(i);
            //    }
            //}

            //// Column Headers
            //for (int i = 0; i < exportColumns.Count; i++)
            //{
            //    xlWorkSheet.Cells[row, i + 1] = dgv.Columns[exportColumns[i]].HeaderText;
            //}
            //row++;

            //// Data Rows
            //for (int i = 0; i < dgv.Rows.Count; i++)
            //{
            //    for (int j = 0; j < exportColumns.Count; j++)
            //    {
            //        var cellValue = dgv.Rows[i].Cells[exportColumns[j]].Value;
            //        xlWorkSheet.Cells[row + i, j + 1] = cellValue?.ToString();
            //    }
            //}

            //xlWorkSheet.Columns.AutoFit();

            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            //if (saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    xlWorkBook.SaveAs(saveFileDialog.FileName);
            //    MessageBox.Show("Cash Invoice Exported Successfully!");
            //}

            //xlWorkBook.Close(false);
            //xlApp.Quit();

            //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //if (dgvCustomer.CurrentRow == null)
            //{
            //    MessageBox.Show("Please select a customer first.");
            //    return;
            //}

            //try
            //{
            //    string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();
            //    ExportCashInvoiceToExcel(dgvInvoiceCash, customerName);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Export failed: " + ex.Message);
            //}
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewEmployee(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }

        private void receivePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(0));

        }
        private void DeleteData(int _id)
        {
            DBClass.ExecuteNonQuery(@"Delete from tbl_employee where id = @id;Delete from tbl_transaction where transaction_id=@id",
                DBClass.CreateParameter("id", _id));
            Utilities.LogAudit(frmLogin.userId,"Delete Employee", "HR", _id, "Deleted Employee with ID: " + _id);
            BindEmployee();
            MessageBox.Show("Employee deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void toolStripMenuItemword_Click(object sender, EventArgs e)
        {
            //if (dgvCustomer.CurrentRow == null)
            //{
            //    MessageBox.Show("Please select a Employee.");
            //    return;
            //}

            //string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

            //SaveFileDialog saveDialog = new SaveFileDialog();
            //saveDialog.Filter = "Word Document (*.docx)|*.docx";
            //saveDialog.Title = "Save Cash Invoices";

            //if (saveDialog.ShowDialog() == DialogResult.OK)
            //{
            //    ExportCashToWord(dgvInvoiceCash, customerName, saveDialog.FileName);
            //    MessageBox.Show("Cash Invoice Exported Successfully!");
            //}
        }

        private void toolStripMenuItemCashWord_Click(object sender, EventArgs e)
        {

            //if (dgvCustomer.CurrentRow == null)
            //{
            //    MessageBox.Show("Please select a customer.");
            //    return;
            //}

            //string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

            //SaveFileDialog saveDialog = new SaveFileDialog();
            //saveDialog.Filter = "Word Document (*.docx)|*.docx";
            //saveDialog.Title = "Save Cash Invoices";

            //if (saveDialog.ShowDialog() == DialogResult.OK)
            //{
            //    ExportCashToWord(dgvInvoiceCash, customerName, saveDialog.FileName);
            //    MessageBox.Show("Cash Invoice Exported Successfully!");
            //}
        }
        private void ExportCashToWord(DataGridView dgv, string customerName, string filePath)
        {
            var doc = DocX.Create(filePath);

            // Header
            var title = doc.InsertParagraph("Employee Name: " + customerName)
                           .FontSize(14)
                           .Bold()
                           .SpacingAfter(20);

            // Determine columns to export (skip "id", "invoice id")
            List<int> includedCols = new List<int>();
            for (int c = 0; c < dgv.Columns.Count; c++)
            {
                string header = dgv.Columns[c].HeaderText.ToLower();
                if (header != "id" && header != "invoice id")
                {
                    includedCols.Add(c);
                }
            }

            int rows = dgv.Rows.Count + 1;
            int cols = includedCols.Count;
            Table table = doc.AddTable(rows, cols);
            table.Design = TableDesign.TableGrid;

            // Set column width and padding
            foreach (var row in table.Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.Width = 100;
                    cell.MarginLeft = 5;
                    cell.MarginRight = 5;
                }
            }

            // Header row
            for (int i = 0; i < includedCols.Count; i++)
            {
                table.Rows[0].Cells[i].Paragraphs[0]
                    .Append(dgv.Columns[includedCols[i]].HeaderText)
                    .Bold();
            }

            // Data rows
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                for (int c = 0; c < includedCols.Count; c++)
                {
                    var val = dgv.Rows[r].Cells[includedCols[c]].Value?.ToString() ?? "";
                    table.Rows[r + 1].Cells[c].Paragraphs[0].Append(val);
                }
            }

            doc.InsertTable(table);
            doc.Save();
        }

        private void deleteEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            int _id = (int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));

            if (dgvTransactions.Rows.Count <= 1)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    DeleteData(_id);
                }
                else
                {
                    MessageBox.Show("Deletion canceled.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Employee has transactions. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmOpeningBalanceEmployee());
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewEmployee(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
    }
}
