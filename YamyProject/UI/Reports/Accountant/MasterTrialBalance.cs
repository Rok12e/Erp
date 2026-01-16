using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterTrialBalance : Form
    {
        bool DrawRect, showPnlHeader;
        private int rowIndex = 0,pageNumber = 1;
        DataTable dt;
        private EventHandler Customer;
        private EventHandler SalesInv;
        private EventHandler SalesReturn;
        private EventHandler Vendor;
        private EventHandler PurchaseInv;
        private EventHandler PurchaseReturn;
        private EventHandler DamageInv;
        private EventHandler Item;
        private EventHandler ReceiptVoucher;
        public MasterTrialBalance()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            Customer = SalesInv = SalesReturn = Vendor = PurchaseInv = PurchaseReturn = DamageInv = Item = ReceiptVoucher = (sender, args) => bindDGVTrial();
            EventHub.Customer += Customer;
            EventHub.SalesInv += SalesInv;
            EventHub.SalesReturn += SalesReturn;
            EventHub.Vendor += Vendor;
            EventHub.PurchaseInv += PurchaseInv;
            EventHub.DamageInv += DamageInv;
            EventHub.Item += Item;
            EventHub.ReceiptVoucher += ReceiptVoucher;

            dgvCustomer.DataBindingComplete += dgvCustomer_DataBindingComplete;

            headerUC1.FormText = this.Text;
        }
        private void MasterTrialBalance_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Customer -= Customer;
            EventHub.SalesInv -= SalesInv;
            EventHub.SalesReturn -= SalesReturn;
            EventHub.Vendor -= Vendor;
            EventHub.PurchaseInv -= PurchaseInv;
            EventHub.DamageInv -= DamageInv;
            EventHub.Item -= Item;
            EventHub.ReceiptVoucher -= ReceiptVoucher;

        }
        private void MasterTrialBalance_Load(object sender, EventArgs e)
        {
            bindDGVTrial();
            loadCompany();
        }
        private void bindDGVTrial()
        {
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            string dateFilter = chkDate.Checked ? "" : " AND t.date >= @startDate AND t.date <= @endDate";
            string query = "";

            if (comboBox1.SelectedIndex == 0)
            {
                query = $@"SELECT * FROM (
                                        SELECT 
                                            t4.id,
                                            CONCAT(t4.code, ' - ', t4.name) AS 'Account Name',
                                            SUM(CASE WHEN t.debit > t.credit THEN t.debit - t.credit ELSE 0 END) AS Debit,  
                                            SUM(CASE WHEN t.debit < t.credit THEN t.credit - t.debit ELSE 0 END) AS Credit,
                                            -- Calculated Balance
                                            SUM(CASE WHEN t.debit > t.credit THEN t.debit - t.credit ELSE 0 END) - 
                                            SUM(CASE WHEN t.debit < t.credit THEN t.credit - t.debit ELSE 0 END) AS Balance
                                        FROM 
                                            tbl_transaction t 
                                            INNER JOIN tbl_coa_level_4 t4 ON t.account_id = t4.id AND t.state = 0 {dateFilter}
                                        GROUP BY 
                                            t4.id, t4.code, t4.name
                                    ) AS result
                                    ORDER BY 
                                        CASE WHEN result.id IS NULL THEN 1 ELSE 0 END,
                                        result.id;";
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                query = $@"SELECT * FROM (
                                        SELECT 
                                            t4.id,
                                            CONCAT(t4.code, ' - ', t4.name) AS `Account Name`,
                                            CASE 
                                                WHEN SUM(t.debit - t.credit) >= 0 THEN SUM(t.debit - t.credit) 
                                                ELSE 0 
                                            END AS Debit,
                                            CASE 
                                                WHEN SUM(t.debit - t.credit) < 0 THEN -SUM(t.debit - t.credit) 
                                                ELSE 0 
                                            END AS Credit
                                        FROM 
                                            tbl_transaction t
                                            INNER JOIN tbl_coa_level_4 t4 ON t.account_id = t4.id
                                        WHERE 
                                            t.state = 0 {dateFilter}
                                        GROUP BY 
                                            t4.id, t4.code, t4.name

                                        UNION ALL

                                        -- TOTAL ROW (fixed: only use Balance from the subquery)
                                        SELECT 
                                            NULL AS id,
                                            'TOTAL' AS `Account Name`,
                                            SUM(CASE WHEN Balance >= 0 THEN Balance ELSE 0 END) AS Debit,
                                            SUM(CASE WHEN Balance < 0 THEN -Balance ELSE 0 END) AS Credit
                                        FROM (
                                            SELECT 
                                                t.account_id,
                                                SUM(t.debit - t.credit) AS Balance
                                            FROM 
                                                tbl_transaction t
                                            WHERE 
                                                t.state = 0 {dateFilter}
                                            GROUP BY 
                                                t.account_id
                                        ) AS balances
                                    ) AS result
                                    ORDER BY 
                                        CASE WHEN result.id IS NULL THEN 1 ELSE 0 END,
                                        result.id;
                                    ";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                query = $@"
                                SELECT * FROM (
                                    SELECT 
                                        t2.id,
                                        CONCAT(t2.code, ' - ', t2.name) AS 'Account Name',

                                        -- Opening Balance before start date
                                        IFNULL((
                                            SELECT SUM(tt.debit - tt.credit)
                                            FROM tbl_transaction tt
                                            WHERE tt.account_id = t2.id AND tt.state = 0 AND tt.date < @startDate
                                        ), 0) AS Opening,

                                        -- Debit during period
                                        IFNULL(SUM(CASE WHEN t.date >= @startDate AND t.date <= @endDate THEN t.debit ELSE 0 END), 0) AS Debit,

                                        -- Credit during period
                                        IFNULL(SUM(CASE WHEN t.date >= @startDate AND t.date <= @endDate THEN t.credit ELSE 0 END), 0) AS Credit,

                                        -- Closing Balance = Opening + (Debit - Credit)
                                        IFNULL((
                                            SELECT SUM(tt.debit - tt.credit)
                                            FROM tbl_transaction tt
                                            WHERE tt.account_id = t2.id AND tt.state = 0 AND tt.date <= @endDate
                                        ), 0) AS Balance

                                    FROM 
                                        tbl_transaction t
                                        INNER JOIN tbl_coa_level_2 t2 ON t.account_id = t2.id AND t.state = 0
                                    WHERE 
                                        t.id > 0
                                    GROUP BY 
                                        t2.id, t2.code, t2.name

                                    UNION ALL

                                    -- TOTAL ROW
                                    SELECT 
                                        NULL,
                                        'TOTAL',
                                        -- Opening Total
                                        IFNULL((
                                            SELECT SUM(debit - credit)
                                            FROM tbl_transaction
                                            WHERE state = 0 AND date < @startDate
                                        ), 0),

                                        -- Debit Total
                                        IFNULL((
                                            SELECT SUM(debit)
                                            FROM tbl_transaction
                                            WHERE state = 0 AND date >= @startDate AND date <= @endDate
                                        ), 0),

                                        -- Credit Total
                                        IFNULL((
                                            SELECT SUM(credit)
                                            FROM tbl_transaction
                                            WHERE state = 0 AND date >= @startDate AND date <= @endDate
                                        ), 0),

                                        -- Closing Balance
                                        IFNULL((
                                            SELECT SUM(debit - credit)
                                            FROM tbl_transaction
                                            WHERE state = 0 AND date <= @endDate
                                        ), 0)
                                ) AS result
                                ORDER BY 
                                    CASE WHEN result.id IS NULL THEN 1 ELSE 0 END,
                                    result.id;";
                
            }
            else
            {
                query = $@"
                        SELECT * FROM (
                                        SELECT 
                                            t4.id,
                                            CONCAT(t4.code, ' - ', t4.name) AS 'Account Name',
                                            SUM(CASE WHEN t.debit > t.credit THEN t.debit - t.credit ELSE 0 END) AS Debit,  
                                            SUM(CASE WHEN t.debit < t.credit THEN t.credit - t.debit ELSE 0 END) AS Credit,
                                            -- Calculated Balance
                                            SUM(CASE WHEN t.debit > t.credit THEN t.debit - t.credit ELSE 0 END) - 
                                            SUM(CASE WHEN t.debit < t.credit THEN t.credit - t.debit ELSE 0 END) AS Balance
                                        FROM 
                                            tbl_transaction t 
                                            INNER JOIN tbl_coa_level_4 t4 ON t.account_id = t4.id AND t.state = 0 {dateFilter}
                                        GROUP BY 
                                            t4.id, t4.code, t4.name
                                    ) AS result
                                    ORDER BY 
                                        CASE WHEN result.id IS NULL THEN 1 ELSE 0 END,
                                        result.id;";

            }
            
            parameters.Add(DBClass.CreateParameter("startDate", dtFrom.Value.Date));
            parameters.Add(DBClass.CreateParameter("endDate", dtTo.Value.Date));

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["id"].Visible = false;

            string[] numericCols = { "debit", "credit", "balance" };
            if(comboBox1.SelectedIndex == 0)
            {
                numericCols = new string[] { "Debit", "Credit" };
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                numericCols = new string[] { "Debit", "Credit" };
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                numericCols = new string[] { "Opening", "Debit", "Credit", "Balance" };
            }
            foreach (string col in numericCols)
            {
                if (dgvCustomer.Columns.Contains(col))
                    dgvCustomer.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
        }

        private void chkDate_CheckedChanged(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            //lblFrom.Text = chkDate.Checked ? "-" : dtFrom.Value.Date.ToShortDateString();
            //lblTo.Text = chkDate.Checked ? "-" : dtTo.Value.Date.ToShortDateString();
            bindDGVTrial();
        }
        private void ChangeRowColors()
        {
            foreach (DataGridViewRow row in dgvCustomer.Rows)
            {
                if (row.Cells["Account Name"].Value.ToString().Trim() == "TOTAL")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(238, 244, 247);
                }
            }
        }
        private void dgvCustomer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ChangeRowColors();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            showPnlHeader = pnlHeader.Visible;
            DrawRect = true;
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += PrintDoc_PrintPage;
            printDoc.EndPrint += PrintDoc_EndPrint;

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDoc
            };

            rowIndex = 0;
            PrintDialog printDialog = new PrintDialog
            {
                Document = printDoc
            };

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
            frmLogin.frmMain.WindowState = FormWindowState.Maximized;
            frmLogin.frmMain.BringToFront();
        }
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvCustomer.RowTemplate.DefaultCellStyle.SelectionBackColor = dgvCustomer.CurrentRow.DefaultCellStyle.BackColor;
        }
        private void PrintDoc_EndPrint(object sender, PrintEventArgs e)
        {
            SetForegroundWindow(this.Handle);
        }
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            int adjustedLeft = e.MarginBounds.Left - 65;
            int adjustedWidth = e.MarginBounds.Width + 130;

            int startX = adjustedLeft;
            int startY = e.MarginBounds.Top;
            int rowHeight = 25;
            int maxRowsPerPage = (e.MarginBounds.Height - startY - 40) / rowHeight;

            int dgvTotalWidth = dgvCustomer.Width;
            int rowsPrinted = 0;

            Font regularFont = new Font("Segoe UI", 10);
            Font boldFont = new Font("Segoe UI", 10, FontStyle.Bold);
            Font pageFont = new Font("Segoe UI", 10, FontStyle.Italic);
            if (showPnlHeader)
            {
                int pnlHeaderWidth = pnlHeader.Width;
                int availableWidth = e.MarginBounds.Width + 130;
                int centerX = e.MarginBounds.Left + (availableWidth - pnlHeaderWidth) / 2;
                using (SolidBrush headerBrush = new SolidBrush(pnlHeader.BackColor))
                {
                    e.Graphics.FillRectangle(headerBrush, centerX, startY - pnlHeader.Height, pnlHeaderWidth, pnlHeader.Height);
                }
                StringFormat stringFormat = new StringFormat()
                {
                    Alignment = DrawRect ? StringAlignment.Center : StringAlignment.Near,
                    LineAlignment = DrawRect ? StringAlignment.Center : StringAlignment.Near,
                };
                foreach (Control control in pnlHeader.Controls)
                {
                    if (control is Label)
                    {
                        e.Graphics.DrawString(control.Text, boldFont, Brushes.Black, centerX + control.Left, startY - pnlHeader.Height + control.Top);
                    }
                    else if (control is PictureBox)
                    {
                        PictureBox pictureBox = (PictureBox)control;
                        if (pictureBox.BackgroundImage != null)
                        {
                            int imageWidth = pictureBox.Width;
                            int imageHeight = pictureBox.Height;
                            int imageX = e.MarginBounds.Right + 80 - imageWidth; 
                            int imageY = e.MarginBounds.Top - 90; 
                            Rectangle imageBounds = new Rectangle(imageX, imageY, imageWidth, imageHeight);
                            e.Graphics.DrawImage(pictureBox.BackgroundImage, imageBounds);
                        }
                    }

                }
            }

            List<int> colWidths = new List<int>();
            List<int> colPositions = new List<int>();
            int currentX = startX;

            foreach (DataGridViewColumn col in dgvCustomer.Columns)
            {
                if (col.Visible)
                {
                    float columnPercentage = (float)col.Width / dgvTotalWidth;
                    int columnPageWidth = (int)(adjustedWidth * columnPercentage);

                    colWidths.Add(columnPageWidth);
                    colPositions.Add(currentX);
                    currentX += columnPageWidth;
                }
            }


            int colIndex = 0;
            foreach (DataGridViewColumn col in dgvCustomer.Columns)
            {
                if (col.Visible)
                {
                    StringFormat stringFormat = new StringFormat()
                    {
                        Alignment = DrawRect ? StringAlignment.Center : StringAlignment.Near,
                        LineAlignment = DrawRect ? StringAlignment.Center : StringAlignment.Near,
                    };

                    e.Graphics.DrawString(col.HeaderText, boldFont, Brushes.Black,
                        new RectangleF(colPositions[colIndex], startY, colWidths[colIndex], rowHeight), stringFormat);

                    if (DrawRect)
                        e.Graphics.DrawRectangle(Pens.Black, colPositions[colIndex], startY, colWidths[colIndex], rowHeight);

                    colIndex++;
                }
            }


            startY += rowHeight;

            while (rowIndex < dgvCustomer.Rows.Count && rowsPrinted < maxRowsPerPage)
            {
                DataGridViewRow row = dgvCustomer.Rows[rowIndex];
                colIndex = 0;
                bool isTotalRow = row.Cells["Account Name"].Value?.ToString().Trim().Equals("TOTAL", StringComparison.OrdinalIgnoreCase) == true;

                int currentRowHeight = rowHeight;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (dgvCustomer.Columns[cell.ColumnIndex].Visible)
                    {
                        Font font = isTotalRow ? boldFont : regularFont;
                        Brush brush = isTotalRow ? Brushes.Blue : Brushes.Black;

                        string cellText = cell.Value?.ToString();
                        if (!string.IsNullOrEmpty(cellText))
                        {
                            StringFormat stringFormat = new StringFormat()
                            {
                                FormatFlags = StringFormatFlags.NoClip,
                                LineAlignment = StringAlignment.Near,
                                Alignment = StringAlignment.Near
                            };

                            SizeF textSize = e.Graphics.MeasureString(cellText, font, colWidths[colIndex], stringFormat);

                            if (textSize.Height > currentRowHeight)
                                currentRowHeight = (int)Math.Ceiling(textSize.Height);

                            RectangleF cellBounds = new RectangleF(colPositions[colIndex], startY + 3, colWidths[colIndex], currentRowHeight);

                            e.Graphics.DrawString(cellText, font, brush, cellBounds, stringFormat);
                        }
                        if (DrawRect)
                            e.Graphics.DrawRectangle(Pens.Black, colPositions[colIndex], startY, colWidths[colIndex], currentRowHeight);

                        colIndex++;
                    }
                }

                startY += currentRowHeight;
                rowIndex++;
                rowsPrinted++;
                if (startY + currentRowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    pageNumber++;
                    return;
                }
            }

            string pageNumText = "Page " + pageNumber;
            SizeF textSizePage = e.Graphics.MeasureString(pageNumText, pageFont);
            float pageX = e.MarginBounds.Left + (e.MarginBounds.Width - textSizePage.Width) / 2;
            float pageY = e.MarginBounds.Bottom - textSizePage.Height - 10;
            e.Graphics.DrawString(pageNumText, pageFont, Brushes.Black, pageX, pageY);

            if (rowIndex < dgvCustomer.Rows.Count)
            {
                pageNumber++;
                e.HasMorePages = true;
            }
            else
                pageNumber = 1;
        }

        private void btnHideHeader_Click(object sender, EventArgs e)
        {
            pnlHeader.Visible = !pnlHeader.Visible;
            btnHideHeader.Text = pnlHeader.Visible ? "Hide Header" : "Show Header";
        }

        private void btnToWord_Click(object sender, EventArgs e)
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

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            //lblFrom.Text = chkDate.Checked ? "-" : dtFrom.Value.Date.ToShortDateString();
            //lblTo.Text = chkDate.Checked ? "-" : dtTo.Value.Date.ToShortDateString();
            bindDGVTrial();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            //lblFrom.Text = chkDate.Checked ? "-" : dtFrom.Value.Date.ToShortDateString();
            //lblTo.Text = chkDate.Checked ? "-" : dtTo.Value.Date.ToShortDateString();
            bindDGVTrial();
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            dtFrom.Enabled = dtTo.Enabled = !chkDate.Checked;
            //lblFrom.Text = chkDate.Checked ? "-" : dtFrom.Value.Date.ToShortDateString();
            //lblTo.Text = chkDate.Checked ? "-" : dtTo.Value.Date.ToShortDateString();
            bindDGVTrial();
        }
        private void loadCompany()
        {
            using (var reader = DBClass.ExecuteReader("SELECT name FROM tbl_company"))
            {
                if (reader.Read() && reader["name"] != DBNull.Value)
                {
                    label3.Text = reader["name"].ToString();
                }
            }
        }
        private void guna2HtmlLabel6_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                DateRangeHelper.SetDateRange(comboBox1.SelectedItem.ToString(), dtFrom, dtTo);
                bindDGVTrial();
            }
        }

        private void btnToExcel_Click(object sender, EventArgs e)
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

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString() == "")
                return;
            frmLogin.frmMain.openChildForm(new MasterTransactionByAccount(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
    }
}
