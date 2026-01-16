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
    public partial class MasterVendor : Form
    {
        private DataView _dataView;
        private EventHandler vendorUpdatedHandler;
        private EventHandler InvoiceUpdatedHandler;
        private EventHandler PaymentVoucherHandler;

        public MasterVendor()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            vendorUpdatedHandler = PaymentVoucherHandler= InvoiceUpdatedHandler = (sender, args) => BindVendor();
            EventHub.Vendor += vendorUpdatedHandler;
            EventHub.PurchaseInv += InvoiceUpdatedHandler;
            EventHub.PaymentVoucher += PaymentVoucherHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterVendor_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.PurchaseInv -= InvoiceUpdatedHandler;
            EventHub.Vendor -= vendorUpdatedHandler;
            EventHub.PaymentVoucher -= PaymentVoucherHandler;

        }
        private void MasterVendor_Load(object sender, EventArgs e)
        {
            ConfigureDataGridViews();
            BindVendor();
        }
        private void ConfigureDataGridViews()
        {
            //DataGridView[] grids = { dgvTransactions, dgvInvoiceCash };
            //foreach (var grid in grids)
            //{
            //    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            //    grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            //    grid.EnableHeadersVisualStyles = false;
            //    grid.RowsDefaultCellStyle.BackColor = Color.White;
            //    grid.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            //    grid.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#D1EAD0");
            //    grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            //    grid.BorderStyle = BorderStyle.None;
            //    grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            //    grid.RowHeadersVisible = false;
            //    grid.AllowUserToAddRows = grid.AllowUserToResizeRows = false;
            //}
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
        public void BindVendor()
        {
            string query = @"SELECT c.id, 
                                   CONCAT(c.code, ' - ', c.name) AS Name, 
                                   c.work_phone, c.main_phone, tc.name AS Category, c.region, 
                                   c.email, c.trn, COALESCE(t.Amount, 0) AS Amount
                            FROM tbl_vendor c
                            LEFT JOIN (
                                    SELECT 
                                        hum_id,
                                        SUM(credit - debit) AS Amount
                                    FROM tbl_transaction
                                    WHERE 
                                        state = 0 AND
                                        type IN (
                                            'Vendor Payment',
                                            'Petty Cash',
                                            'Purchase Invoice',
                                            'Vendor Opening Balance',
                                            'Vendor Advance Payment',
                                            'Check Cancel (Vendor)',
                                            'Purchase Return Invoice',
                                            'Debit Note',
                                            'PDC Payable'
                                )
                            GROUP BY hum_id
                        ) t ON t.hum_id = c.id
                        LEFT JOIN tbl_vendor_category tc ON c.Cat_id = tc.id WHERE c.type='Vendor' ";
            if (cmbState.Text == "Active Vendor")
                query += " AND c.active = 0";
            else if (cmbState.Text == "Inactive Vendor")
                query += " AND c.active != 0";

            //query += " GROUP BY c.id, c.name, c.work_phone, c.main_phone, tc.name, c.region, c.email, c.trn;";
            DataTable dt = DBClass.ExecuteDataTable(query);
            if(dt==null)
            {
                dgvCustomer.DataSource = null;
                ClearVendorDetails();
                return;
            }
            _dataView = dt.DefaultView;
            dgvCustomer.DataSource = _dataView;
            dgvCustomer.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCustomer.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            HideUnusedColumns();
            if (dgvCustomer.Rows.Count > 0)
            {
                BindVendorInvoice();
                editCustomerToolStripMenuItem1.Visible = UserPermissions.canEdit("Vendor Center");
                deleteVendorToolStripMenuItem.Visible = UserPermissions.canDelete("Vendor Center");
            }
            else
                ClearVendorDetails();

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void HideUnusedColumns()
        {
            string[] hiddenColumns = { "id", "work_phone", "main_phone", "category", "trn", "email", "region" };
            foreach (var col in hiddenColumns)
                dgvCustomer.Columns[col].Visible = false;
        }
        private void BindVendorInvoice()
        {
            BindCustomerInfo();
            //BindInvoice(new List<string> { "%Purchase Invoice Cash%" }, new List<string>(), dgvInvoiceCash);
            //BindInvoice(new List<string> { "Vendor%", "Purchase%" , "Check Cancel (Vendor)" }, new List<string> { "%Purchase Invoice Cash%" }, dgvInvoiceCredit);
            BindInvoice();
            BindCashInvoice();
            AdjustColumnWidths();
        }
        private void BindInvoiceOld()
        {
            List<string> includeTypes = new List<string> { "Vendor%", "Purchase%", "Check Cancel (Vendor)", "%Purchase Invoice Cash%" };
            List<string> excludeTypes = new List<string> { };

            string query = @"WITH CTE AS (
                                SELECT 
                                    t.id,
                                    t.transaction_id AS `Invoice Id`,
                                    ROW_NUMBER() OVER (ORDER BY t.date, t.id) AS SN,
                                    t.voucher_no AS `V - No`,
                                    t.date AS `Date`,
                                    CONCAT(ta.code, ' - ', ta.name) AS `Account Name`,
                                    t.Type,
                                    -- Amount logic
                                    CASE
                                        WHEN t.type = 'Vendor Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Petty Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Purchase Invoice Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Vendor Opening Balance' AND t.debit > 0 THEN -t.debit
                                        WHEN t.type = 'Check Cancel (Vendor)' THEN t.credit
                                        WHEN t.type = 'Debit Note' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'PDC Payable' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type LIKE 'Vendor%' OR t.type LIKE 'Purchase%' THEN 
                                            IF(t.debit = 0, t.credit, t.debit)
                                        ELSE NULL
                                    END AS `Amount`,
                                    -- Paid Amount logic
                                    CASE
                                        WHEN t.type = 'Purchase Invoice Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Vendor Payment' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Petty Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Purchase Return Invoice' THEN IF(t.debit = 0, t.credit, t.debit)
                                        WHEN t.type = 'Debit Note' THEN IF(t.debit = 0, t.credit, t.debit)
                                        ELSE 0
                                    END AS `Paid Amt`
                                FROM tbl_transaction t
                                INNER JOIN tbl_coa_level_4 ta ON t.account_id = ta.id
                                WHERE t.hum_id = @id AND t.state = 0  
                                  AND t.type IN (
                                    'Vendor Payment',
                                    'Petty Cash',
                                    'Purchase Invoice',
                                    'Purchase Invoice Cash',
                                    'Vendor Opening Balance',
                                    'Check Cancel (Vendor)',
                                    'Purchase Return Invoice',
                                    'Debit Note',
                                    'PDC Payable'
                                )
                            )
                            , Running_Balance AS (
                                SELECT *,
                                    SUM(`Amount` - `Paid Amt`) OVER (ORDER BY `Date`, id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS `Balance`
                                FROM CTE
                            )
                            SELECT 
                                `id`,
                                `Invoice Id`,
                                `SN`,
                                `V - No`,
                                `Date`,
                                `Account Name`,
                                `Type`,
                                `Amount`,
                                `Paid Amt`,
                                `Balance`
                            FROM Running_Balance
                            ORDER BY `Date`, id;";

            //if (includeTypes.Count > 0)
            //{
            //    query += " AND (";
            //    query += string.Join(" OR ", includeTypes.Select((t, i) => $"t.type LIKE @include{i}"));
            //    query += ")";
            //}

            //if (excludeTypes.Count > 0)
            //{
            //    query += " AND (";
            //    query += string.Join(" AND ", excludeTypes.Select((t, i) => $"t.type NOT LIKE @exclude{i}"));
            //    query += ")";
            //}

            //query += " ORDER BY t.date;";

            var parameters = new List<MySqlParameter> { new MySqlParameter("@id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()) };
            //for (int i = 0; i < includeTypes.Count; i++)
            //    parameters.Add(new MySqlParameter($"@include{i}", includeTypes[i]));
            //for (int i = 0; i < excludeTypes.Count; i++)
            //    parameters.Add(new MySqlParameter($"@exclude{i}", excludeTypes[i]));

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
                    if (_type == "Vendor Payment" || _type == "Purchase Return Invoice" || _type == "PDC Payable")
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
        private void BindInvoice()
        {
            dgvTransactions.Rows.Clear();
            var parameters = new List<MySqlParameter> { new MySqlParameter("@id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()) };
            
            string query = @"SELECT 
                            ROW_NUMBER() OVER (ORDER BY t.id) AS `SN#`,
                            t.`Date`,
                            t.voucher_no as `No`,
                            t.type AS `Type`,
                            t.id,
                            t.transaction_id AS `Invoice Id`,
                            ta.name AS `Description`,
                            t.debit AS `Debit`,
                            t.credit AS `Credit`,
                            SUM(t.credit - t.debit) OVER (ORDER BY t.id ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS `Balance`
                        FROM 
                            tbl_transaction t
                        INNER JOIN 
                            tbl_coa_level_4 ta ON t.account_id = ta.id
                        WHERE
                            t.hum_id = @id 
                            AND t.state = 0
                            AND t.type IN (
                                'Vendor Payment', 
                                'Petty Cash', 
                                'Purchase Invoice', 
                                'Purchase Invoice Cash', 
                                'Vendor Opening Balance',
                                'Vendor Advance Payment',
                                'Check Cancel (Vendor)', 
                                'Purchase Return Invoice', 
                                'Debit Note', 
                                'PDC Payable'
                                 )";
            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            decimal totalAmount = 0, totalPaidAmount = 0, amount = 0, paidAmount = 0;
            string _type = "";
            foreach(DataRow row in dt.Rows)
            {
                _type = string.IsNullOrEmpty(row["Type"]?.ToString()) ? "" : row["Type"]?.ToString();
                dgvTransactions.Rows.Add(row["id"], dgvTransactions.Rows.Count + 1, row["Invoice Id"], 
                    DateTime.Parse(row["Date"].ToString()).ToShortDateString(),
                    (string.IsNullOrEmpty(row["No"].ToString()) ? ("GV-00" + row["Invoice Id"]) : row["No"]), 
                    _type, row["Description"], row["Debit"], row["Credit"], row["Balance"]);
            }
            //LocalizationManager.LocalizeDataGridViewHeaders(dgvTransactions);
        }
        private void BindCashInvoice()
        {
            List<string> includeTypes = new List<string> { "%Purchase Invoice Cash%" };
            List<string> excludeTypes = new List<string>();

            string query = @"SELECT t.id, 
                            t.transaction_id AS 'Invoice Id',
                            ROW_NUMBER() OVER (ORDER BY t.date, t.id) AS `SN`,
                            t.voucher_no as `V - No`,
                            t.date AS `Date`, 
                            CONCAT(ta.code, ' - ', ta.name) AS `Account Name`,
                            t.Type, 
                            CASE 
                                WHEN t.type = 'Purchase Invoice Cash' THEN IF(t.debit = 0, t.credit, t.debit)
                                ELSE NULL 
                            END AS `Amount`
                            FROM tbl_transaction t
                            INNER JOIN tbl_coa_level_4 ta ON t.account_id = ta.id
                            WHERE t.hum_id = @id AND t.state = 0 ";

            if (includeTypes.Count > 0)
            {
                query += " AND (";
                query += string.Join(" OR ", includeTypes.Select((t, i) => $"t.type LIKE @include{i}"));
                query += ")";
            }

            if (excludeTypes.Count > 0)
            {
                query += " AND (";
                query += string.Join(" AND ", excludeTypes.Select((t, i) => $"t.type NOT LIKE @exclude{i}"));
                query += ")";
            }

            query += " ORDER BY t.date;";

            var parameters = new List<MySqlParameter> { new MySqlParameter("@id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()) };
            for (int i = 0; i < includeTypes.Count; i++)
                parameters.Add(new MySqlParameter($"@include{i}", includeTypes[i]));
            for (int i = 0; i < excludeTypes.Count; i++)
                parameters.Add(new MySqlParameter($"@exclude{i}", excludeTypes[i]));

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            decimal totalAmount = 0, amount = 0;
            foreach (DataRow row in dt.Rows)
                if (decimal.TryParse(row["Amount"]?.ToString(), out amount))
                    totalAmount += amount;

            DataRow totalRow = dt.NewRow();
            totalRow["Account Name"] = "Total";
            totalRow["Amount"] = totalAmount.ToString("F3");
            dt.Rows.Add(totalRow);

            dgvInvoiceCash.DataSource = dt;

            dgvInvoiceCash.Columns["Amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvInvoiceCash.Columns["Account Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvInvoiceCash.Columns["Amount"].DefaultCellStyle.Format = "N2";
            foreach (DataGridViewColumn column in dgvInvoiceCash.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvInvoiceCash.Columns["id"].Visible = dgvInvoiceCash.Columns["Invoice Id"].Visible = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoiceCash);
        }
        private void AdjustColumnWidths()
        {
            //dgvTransactions.Columns["id"].Visible = dgvTransactions.Columns["Invoice Id"].Visible = false;
            //dgvInvoiceCash.Columns["id"].Visible = dgvInvoiceCash.Columns["type"].Visible = dgvInvoiceCash.Columns["Invoice Id"].Visible = false;
            //dgvInvoiceCash.Columns["SN"].Width = dgvTransactions.Columns["SN"].Width = 30;
            //dgvTransactions.Columns["V - No"].Width = 80;
            //dgvInvoiceCash.Columns["Date"].Width = dgvTransactions.Columns["Date"].Width = 90;
            //dgvTransactions.Columns["Debit"].Width = dgvTransactions.Columns["Debit"].Width = 90;
            //dgvTransactions.Columns["Credit"].Width = 90;
            //dgvTransactions.Columns["Balance"].Width = 100;
            //dgvTransactions.Columns["Balance"].DefaultCellStyle.Format = "N2";
            //dgvInvoiceCash.Columns["Type"].Width = dgvTransactions.Columns["Type"].Width = 240;
            //dgvInvoiceCash.Columns["Account Name"].AutoSizeMode = dgvTransactions.Columns["Account Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void BindCustomerInfo()
        {
            var row = dgvCustomer.SelectedRows[0].Cells;
            lblAddress.Text = string.IsNullOrWhiteSpace(row["region"].Value?.ToString()) ? "-" : row["region"].Value.ToString();
            lblCat.Text = string.IsNullOrWhiteSpace(row["category"].Value?.ToString()) ? "-" : row["category"].Value.ToString();
            lblEmail.Text = string.IsNullOrWhiteSpace(row["email"].Value?.ToString()) ? "-" : row["email"].Value.ToString();
            lblPhone.Text = string.IsNullOrWhiteSpace(row["main_phone"].Value?.ToString()) ? "-" : row["main_phone"].Value.ToString();
            lblWorkPhone.Text = string.IsNullOrWhiteSpace(row["work_phone"].Value?.ToString()) ? "-" : row["work_phone"].Value.ToString();
            lblTRN.Text = string.IsNullOrWhiteSpace(row["trn"].Value?.ToString()) ? "-" : row["trn"].Value.ToString();
            
            string nameAndCode = row["name"].Value?.ToString();
            lblCode.Text = nameAndCode.Split('-')[0];
            lblName.Text = nameAndCode.Split('-')[1];
            if (nameAndCode.Contains("-"))
            {
                var parts = nameAndCode.Split('-');
                if (parts.Length > 1)
                {
                    lblName.Text = string.Join(" - ", parts.Skip(1).Select(p => p.Trim()));
                }
            }
        }
        private void ClearVendorDetails()
        {
            dgvInvoiceCash.DataSource = dgvTransactions.DataSource = null;
            lblCode.Text = lblAddress.Text = lblCat.Text = lblEmail.Text = lblName.Text = lblPhone.Text = lblTRN.Text = lblWorkPhone.Text = "-";
            LocalizationManager.LocalizeDataGridViewHeaders(dgvInvoiceCash);
        }
        private void dgvInvoice_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTransactions.Rows.Count)
            {
                if (dgvTransactions.Columns.Contains("type"))
                {
                    if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Purchase")
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString())));
                    }
                    else if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Purchase Return")
                    {
                        frmLogin.frmMain.openChildForm(new frmPurchaseReturn(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString())));
                    }
                    else if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Payment")
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString()), int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
                    }
                    else if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Advance Payment")
                    {
                        frmLogin.frmMain.openChildForm(new frmViewPaymentVoucherAdvance(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString()), int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
                    }
                    else if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Vendor Opening Balance")
                    {
                        frmLogin.frmMain.openChildForm(new frmTransactionJournal(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString()), "Vendor Opening Balance", (dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString()), (dgvTransactions.Rows[e.RowIndex].Cells["Date"].Value.ToString())));
                    }
                    else if (CommonInsert.GetFormName(dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString()) == "Debit Note")
                    {
                        frmLogin.frmMain.openChildForm(new frmCreditNote(int.Parse(dgvTransactions.Rows[e.RowIndex].Cells["InvoiceId"].Value.ToString())));
                    }
                }
            }
        }
        private void dgvInvoiceCash_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvTransactions.Rows.Count)
            {
                if (dgvTransactions.Columns.Contains("type"))
                {
                    if (dgvTransactions.Rows[e.RowIndex].Cells["type"].Value.ToString().ToUpper().Contains("PURCHASE"))
                        frmLogin.frmMain.openChildForm(new frmPurchase(int.Parse(dgvInvoiceCash.Rows[e.RowIndex].Cells["Invoice Id"].Value.ToString())));
                }
            }
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
                                            "Vendor Data Report",
                                            " Vendor ",
                                            "All Vendors"
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
            BindVendor();
        }
        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewVendor(0).ShowDialog();
        }
        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            new frmViewVendor(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //BindInvoice(new List<string> { "%Purchase Invoice Cash%" }, new List<string>(), dgvInvoiceCash);
            //BindInvoice(new List<string> { "Vendor%", "Purchase%" ,"Check Cancel"}, new List<string> { "%Purchase Invoice Cash%" }, dgvTransactions);
            BindCustomerInfo();
            BindInvoice();
            BindCashInvoice();
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
            string customerName = dgvCustomer.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "Unknown Customer";

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
                string[] columnNames = { "SN", "No", "Date", "Description", "Type", "Debit", "Credit", "Balance" };

                Dictionary<string, float> widthRatios = new Dictionary<string, float>
                {
                    { "SN", 0.06f },
                    { "No", 0.08f },
                    { "Date", 0.10f },
                    { "Description", 0.24f },
                    { "Type", 0.18f },
                    { "Debit", 0.17f },
                    { "Credit", 0.17f },
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
            if (dgvInvoiceCash.Rows.Count == 0)
            {
                MessageBox.Show("No cash invoice data to print.");
                return;
            }

            // Get selected customer details
            string customerCode = "-", customerName = "-";
            if (dgvCustomer.SelectedRows.Count > 0)
            {
                string fullName = dgvCustomer.SelectedRows[0].Cells["Name"].Value?.ToString() ?? "";
                var parts = fullName.Split(new[] { '-' }, 2);
                if (parts.Length == 2)
                {
                    customerCode = parts[0].Trim();
                    customerName = parts[1].Trim();
                }
                else
                {
                    customerName = fullName;
                }
            }

            string companyName = GetCompanyName(); // Company name from DB

            PrintDocument printDocument = new PrintDocument();
            printDocument.DefaultPageSettings.Landscape = true;

            printDocument.PrintPage += (s, ev) =>
            {
                int leftMargin = 50;
                int rightMargin = 50;
                int topMargin = 50;
                int yPosition = topMargin;
                int lineHeight = 30;
                int pageWidth = ev.PageBounds.Width;
                int availableWidth = pageWidth - leftMargin - rightMargin;

                ev.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Fonts
                Font companyFont = new Font("Times New Roman", 18, FontStyle.Bold);
                Font subTitleFont = new Font("Times New Roman", 14, FontStyle.Bold);
                Font titleFont = new Font("Times New Roman", 13, FontStyle.Bold);
                Font colFont = new Font("Times New Roman", 11, FontStyle.Bold);
                Font cellFont = new Font("Times New Roman", 11);
                Font labelFont = new Font("Times New Roman", 11);

                // Time & Date (Top Left)
                string timeNow = DateTime.Now.ToString("hh:mm tt");
                string dateNow = DateTime.Now.ToString("dd/MM/yyyy");

                ev.Graphics.DrawString(timeNow, labelFont, Brushes.Black, leftMargin, yPosition);
                yPosition += (int)labelFont.GetHeight(ev.Graphics);
                ev.Graphics.DrawString(dateNow, labelFont, Brushes.Black, leftMargin, yPosition);
                yPosition += (int)labelFont.GetHeight(ev.Graphics) + 10;

                // Company Name (Centered)
                SizeF companySize = ev.Graphics.MeasureString(companyName, companyFont);
                ev.Graphics.DrawString(companyName, companyFont, Brushes.Black,
                    new PointF((pageWidth - companySize.Width) / 2, topMargin));

                // SubTitle
                string subTitle = "All Transactions for " + customerCode + " - " + customerName;
                SizeF subSize = ev.Graphics.MeasureString(subTitle, subTitleFont);
                ev.Graphics.DrawString(subTitle, subTitleFont, Brushes.Black,
                    new PointF((pageWidth - subSize.Width) / 2, yPosition));
                yPosition += (int)subSize.Height + 15;

                // Report Title
                SizeF reportTitleSize = ev.Graphics.MeasureString("Cash Invoice Report", titleFont);
                ev.Graphics.DrawString("Cash Invoice Report", titleFont, Brushes.Black,
                    new PointF((pageWidth - reportTitleSize.Width) / 2, yPosition));
                yPosition += (int)reportTitleSize.Height + 10;

                // Horizontal Line
                Pen boldPen = new Pen(Color.Black, 3);
                ev.Graphics.DrawLine(boldPen, leftMargin, yPosition, pageWidth - rightMargin, yPosition);
                yPosition += 15;

                // Table Layout
                string[] columns = { "SN", "V - No", "Date", "Account Name", "Amount" };
                Dictionary<string, float> widthRatio = new Dictionary<string, float>
        {
            { "SN", 0.08f },
            { "V - No", 0.12f },
            { "Date", 0.14f },
            { "Account Name", 0.46f },
            { "Amount", 0.20f }
        };
                Dictionary<string, int> widths = columns.ToDictionary(c => c, c => (int)(availableWidth * widthRatio[c]));

                // Draw headers
                int x = leftMargin;
                foreach (var col in columns)
                {
                    ev.Graphics.DrawString(col, colFont, Brushes.Black,
                        new RectangleF(x, yPosition, widths[col], lineHeight),
                        new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    ev.Graphics.DrawRectangle(Pens.Black, x, yPosition, widths[col], lineHeight);
                    x += widths[col];
                }
                yPosition += lineHeight;

                // Draw data rows
                foreach (DataGridViewRow row in dgvInvoiceCash.Rows)
                {
                    if (row.IsNewRow) continue;
                    x = leftMargin;

                    foreach (var col in columns)
                    {
                        string text = row.Cells[col].FormattedValue?.ToString() ?? "";
                        RectangleF bounds = new RectangleF(x, yPosition, widths[col], lineHeight);

                        StringFormat format = new StringFormat { LineAlignment = StringAlignment.Center };
                        if (col == "Amount")
                            format.Alignment = StringAlignment.Far;
                        else
                            format.Alignment = StringAlignment.Near;

                        ev.Graphics.DrawString(text, cellFont, Brushes.Black, bounds, format);
                        ev.Graphics.DrawRectangle(Pens.Black, x, yPosition, widths[col], lineHeight);
                        x += widths[col];
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
            xlWorkSheet.Cells[row, 1] = "Customer Name:";
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
                MessageBox.Show("Please select a customer first.");
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
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!");
                return;
            }

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            int row = 1;

            // Header info
            xlWorkSheet.Cells[row, 1] = "Customer Name:";
            xlWorkSheet.Cells[row, 2] = customerName;
            row += 2;

            // Get column indexes to export (skip "id", "Invoice Id", and "Balance")
            List<int> exportColumns = new List<int>();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                string colName = dgv.Columns[i].HeaderText.ToLower();
                if (colName != "id" && colName != "invoice id" && colName != "balance")
                {
                    exportColumns.Add(i);
                }
            }

            // Column Headers
            for (int i = 0; i < exportColumns.Count; i++)
            {
                xlWorkSheet.Cells[row, i + 1] = dgv.Columns[exportColumns[i]].HeaderText;
            }
            row++;

            // Data Rows
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                for (int j = 0; j < exportColumns.Count; j++)
                {
                    var cellValue = dgv.Rows[i].Cells[exportColumns[j]].Value;
                    xlWorkSheet.Cells[row + i, j + 1] = cellValue?.ToString();
                }
            }

            xlWorkSheet.Columns.AutoFit();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                xlWorkBook.SaveAs(saveFileDialog.FileName);
                MessageBox.Show("Cash Invoice Exported Successfully!");
            }

            xlWorkBook.Close(false);
            xlApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        }
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a customer first.");
                return;
            }

            try
            {
                string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();
                ExportCashInvoiceToExcel(dgvInvoiceCash, customerName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            new frmViewVendor(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }

        private void receivePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewPaymentVoucher(0));

        }

        private void deleteVendorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            int _id = (int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));

            if (dgvTransactions.Rows.Count <= 1 && dgvInvoiceCash.Rows.Count <= 1)
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
                MessageBox.Show("Vendor has transactions. Cannot delete.", "Cannot Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void DeleteData(int _id)
        {
            DBClass.ExecuteNonQuery(@"Delete from tbl_vendor where id = @id;Delete from tbl_transaction where transaction_id=@id and type='Vendor Opening Balance'",
                DBClass.CreateParameter("id", _id));
            Utilities.LogAudit(frmLogin.userId, "Vendor Deleted", "Vendor", _id, "Deleted Vendor with ID: " + _id);
            BindVendor();
            MessageBox.Show("Vendor deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmOpeningBalance("Vendor"));
        }

        private void toolStripMenuItemword_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a vendor.");
                return;
            }

            string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document (*.docx)|*.docx";
            saveDialog.Title = "Save Cash Invoices";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ExportCashToWord(dgvInvoiceCash, customerName, saveDialog.FileName);
                MessageBox.Show("Cash Invoice Exported Successfully!");
            }
        }

        private void toolStripMenuItemCashWord_Click(object sender, EventArgs e)
        {

            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a vendor.");
                return;
            }

            string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Word Document (*.docx)|*.docx";
            saveDialog.Title = "Save Cash Invoices";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ExportCashToWord(dgvInvoiceCash, customerName, saveDialog.FileName);
                MessageBox.Show("Cash Invoice Exported Successfully!");
            }
        }
        private void ExportCashToWord(DataGridView dgv, string customerName, string filePath)
        {
            var doc = DocX.Create(filePath);

            // Header
            var title = doc.InsertParagraph("Vendor Name: " + customerName)
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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            new frmViewVendor(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }
    }
}
