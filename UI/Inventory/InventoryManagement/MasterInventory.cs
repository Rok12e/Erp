using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Inventory.InventoryManagement;
using Excel = Microsoft.Office.Interop.Excel;

namespace YamyProject
{
    public partial class MasterInventory : Form
    {
        private DataView _dataView;
        private EventHandler itemUpdatedHandler;
        private EventHandler invoiceUpdatedHandler;
        private EventHandler purchaseUpdatedHandler;
        public MasterInventory()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            itemUpdatedHandler = (sender, args) => BindItems();
            invoiceUpdatedHandler = purchaseUpdatedHandler = (sender, args) => BindItems();
            EventHub.Item += itemUpdatedHandler;
            EventHub.SalesInv += invoiceUpdatedHandler;
            EventHub.PurchaseInv += purchaseUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterInventory_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.Item -= itemUpdatedHandler;
            EventHub.SalesInv -= invoiceUpdatedHandler;
            EventHub.PurchaseInv -= purchaseUpdatedHandler;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {

        }
        private void MasterInventory_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateItemsCategory(cmbCategory);
            BindItems();
        }
        public void BindItems()
        {
            DataTable dt;
            string query = @"Select id,Concat( code , ' - ',name) as 'Item Name',barcode as Barcode,
              Type from tbl_items where state = 0";
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbCategory.Text != "" && !chkAllCategory.Checked)
            {
                query += " and category_id = @category_id";
                parameters.Add(DBClass.CreateParameter("category_id", cmbCategory.SelectedValue.ToString()));
            }
            if (cmbType.Text != "" && !chkAllType.Checked)
            {
                query += " and type = @type";
                parameters.Add(DBClass.CreateParameter("type", cmbType.Text));
            }
            if (cmbState.Text == "Active")
                query += " and active = 0";
            else if (cmbState.Text != "All")
                query += " and active != 0";
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            if (dt == null)
            {
                dgvCustomer.DataSource = null;
                return;
            }
            _dataView = dt.DefaultView;
            dgvCustomer.DataSource = dt;
            dgvCustomer.Columns["Item name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCustomer.Columns["type"].Width = 140;
            dgvCustomer.Columns["id"].Visible = false;
            LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);

            if (dgvCustomer.Rows.Count > 0)
            {
                deleteCustomerToolStripMenuItem.Visible = recycleItemToolStripMenuItem.Visible = UserPermissions.canDelete("Inventory Items");
                editCustomerToolStripMenuItem1.Visible = UserPermissions.canEdit("Inventory Items");
            }
        }
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            cmbCategory.Enabled = !chkAllCategory.Checked;
            cmbType.Enabled = !chkAllType.Checked;
            BindItems();
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindItems();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {

        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            frmLogin.frmMain.openChildForm(new frmViewItem(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
        }

        private void btnRecycle_Click(object sender, EventArgs e)
        {


        }
        private void RestoreForm_DataRestored(object sender, EventArgs e)
        {
            BindItems();
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BindItemDetails();
            BindItemTransaction();
        }
        private void BindItemDetails() {
            int itemId = int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString());
            string query = "SELECT * FROM tbl_items WHERE id=@itemId";
            DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("itemId", itemId));
            foreach (DataRow row in dt.Rows)
            {
                lblCode.Text = row["code"].ToString();
                lblName.Text = row["name"].ToString();
                lblPrice.Text = Convert.ToDecimal(row["cost_price"]).ToString("N2");
                lblSalePrice.Text = Convert.ToDecimal(row["sales_price"]).ToString("N2");
                lblOnHand.Text = Convert.ToDecimal(row["on_hand"]).ToString("N2");
            }
        }
        private void BindItemTransaction()
        {
            dgvDetails.Rows.Clear();
            int itemId = int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString());
            string query = "SELECT * FROM tbl_item_transaction WHERE item_id=@itemId";
            string type = dgvCustomer.SelectedRows[0].Cells["type"].Value.ToString();
            if (type == "12 - Service")
            {
                query = "SELECT * FROM tbl_item_transaction WHERE item_id=@itemId";
            } else if (type == "13 - Inventory Assembly")
            {
                query = "SELECT * FROM tbl_item_transaction WHERE item_id=@itemId";
            }
            decimal QtyBalance = 0;
            DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("itemId", itemId));
            foreach (DataRow row in dt.Rows)
            {
                string _id = row["id"].ToString();
                string _date = Convert.ToDateTime(row["date"]).ToString("dd-MM-yyyy");
                string _reference = row["reference"].ToString();
                string _type = row["type"].ToString();
                decimal _qtyIn = Convert.ToDecimal(row["qty_in"]);
                decimal _qtyOut = Convert.ToDecimal(row["qty_out"]);
                decimal _costPrice = Convert.ToDecimal(row["cost_price"]);
                decimal _salesPrice = Convert.ToDecimal(row["sales_price"]);
                decimal _qtyBalance = _qtyIn - _qtyOut;

                QtyBalance += _qtyBalance;
                dgvDetails.Rows.Add(
                    _id,
                    _date,
                    _reference,
                    _type,
                    _qtyIn.ToString("N2"),
                    _qtyOut.ToString("N2"),
                    _costPrice.ToString("N2"),
                    _salesPrice.ToString("N2"),
                    QtyBalance.ToString("N2")
                );
            }
            dgvDetails.Columns["t_qty_in"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetails.Columns["t_qty_out"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetails.Columns["t_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetails.Columns["t_sales_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDetails.Columns["t_qty_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewItem());
        }

        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;
            frmLogin.frmMain.openChildForm(new frmViewItem(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }

        private void deleteCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            int id = int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString());
            object result = DBClass.ExecuteScalar(@"SELECT COUNT(1) from tbl_item_transaction where type != 'Opening Qty' and item_id = @id", DBClass.CreateParameter("id", id));
            int recordCount = 0;
            if (result != null && result != DBNull.Value)
                recordCount = Convert.ToInt32(result);
            if (recordCount > 0)
            {
                MessageBox.Show("Item already used cannot delete! \n you can inactive the Item");
                return;
            }

            DBClass.ExecuteNonQuery("delete from tbl_items where id = @id; delete from tbl_item_transaction WHERE item_id = @id", DBClass.CreateParameter("id", id));
            BindItems();
        }

        private void recycleItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterRecycle restoreForm = new MasterRecycle("Item");
            restoreForm.DataRestored += RestoreForm_DataRestored;
            restoreForm.ShowDialog();
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCard_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count > 0)
            {
                var selectedRow = dgvCustomer.SelectedRows[0];

                string[] value = selectedRow.Cells["item name"].Value.ToString().Split(new string[] { " - " }, StringSplitOptions.None);
                if (value.Length >= 2)
                {
                    string code = value[0].Trim();
                    string name = value[1].Trim();
                    int itemId = int.Parse(selectedRow.Cells["id"].Value.ToString());

                    frmLogin.frmMain.openChildForm(new frmItemCard(code, name, itemId));
                }
                else
                {
                    MessageBox.Show("Item name format is incorrect. Expected format: 'code - name'");
                }
            }
            else
            {
                MessageBox.Show("Please select a row first.");
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            BindItemDetails();
            BindItemTransaction();
        }

        private void pDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvDetails.Rows.Count == 0)
            {
                MessageBox.Show("No data to print.");
                return;
            }

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            PrintPreviewDialog previewDialog = new PrintPreviewDialog
            {
                Document = printDocument,
                Width = 1000,
                Height = 800
            };

            previewDialog.ShowDialog();
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int leftMargin = 40;
            int topMargin = 40;
            int rowHeight = 25;
            int colSpacing = 5;

            Font font = new Font("Segoe UI", 9);
            Font headerFont = new Font("Segoe UI", 9, FontStyle.Bold);
            Brush brush = Brushes.Black;

            int y = topMargin;
            int x = leftMargin;

            // Print headers
            for (int i = 0; i < dgvDetails.Columns.Count; i++)
            {
                string header = dgvDetails.Columns[i].HeaderText;
                e.Graphics.DrawString(header, headerFont, brush, x, y);
                x += 100; // fixed width; you can customize per column
            }

            y += rowHeight;

            // Print rows
            int currentRow = 0;
            while (currentRow < dgvDetails.Rows.Count)
            {
                DataGridViewRow row = dgvDetails.Rows[currentRow];

                x = leftMargin;
                for (int i = 0; i < dgvDetails.Columns.Count; i++)
                {
                    string cellText = row.Cells[i].FormattedValue?.ToString() ?? "";
                    StringFormat format = new StringFormat();
                    decimal _;
                    // Right-align decimals
                    if (decimal.TryParse(cellText, out _))
                        format.Alignment = StringAlignment.Far;

                    RectangleF rect = new RectangleF(x, y, 100 - colSpacing, rowHeight);
                    e.Graphics.DrawString(cellText, font, brush, rect, format);
                    x += 100;
                }

                y += rowHeight;
                currentRow++;

                // New page if beyond margin
                if (y + rowHeight > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            currentRow = 0; // Reset after done
            e.HasMorePages = false;
        }
        string GetCompanyName()
        {
            return DBClass.ExecuteScalar("select name from tbl_company LIMIT 1")?.ToString() ?? "";
        }

        private void exportImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmOpeningQty());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a item first.");
                return;
            }

            try
            {
                string customerName = dgvCustomer.CurrentRow.Cells["Item Name"].Value.ToString();
                
                decimal balance = 0;
                if (!string.IsNullOrEmpty(lblOnHand.Text.ToString()))
                {
                    balance = decimal.Parse(lblOnHand.Text.ToString());
                }

                ExportToExcel(dgvDetails, customerName, balance);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
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
            xlWorkSheet.Cells[row, 1] = "Product Name:";
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
        private void itemListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count <= 0)
            {
                MessageBox.Show("Please add item first.");
                return;
            }

            string query = @"SELECT i.code Code, i.name Name, i.barcode Barcode, 
                (SELECT price FROM tbl_item_card_details WHERE tbl_item_card_details.itemId = i.id ORDER BY id DESC LIMIT 1) AS `Cost Price`,
                (SELECT sales_price FROM tbl_item_transaction WHERE tbl_item_transaction.item_id = i.id ORDER BY id DESC LIMIT 1) AS `Sales Price`,
                i.on_hand Qty, i.type Type, IFNULL(u.name, '') AS Unit 
                FROM tbl_items i 
                LEFT JOIN tbl_unit u ON u.id = i.unit_id 
                WHERE i.id > 0";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbCategory.Text != "" && !chkAllCategory.Checked)
            {
                query += " AND i.category_id = @category_id";
                parameters.Add(DBClass.CreateParameter("category_id", cmbCategory.SelectedValue.ToString()));
            }
            if (cmbType.Text != "" && !chkAllType.Checked)
            {
                query += " AND i.type = @type";
                parameters.Add(DBClass.CreateParameter("type", cmbType.Text));
            }
            if (cmbState.Text == "Active")
                query += " AND i.active = 0";
            else if (cmbState.Text != "All")
                query += " AND i.active != 0";

            DataTable dt = DBClass.ExecuteDataTable(query, parameters.ToArray());

            // Export to Excel
            Excel.Application xlApp = new Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!");
                return;
            }

            Excel.Workbook xlWorkBook = xlApp.Workbooks.Add();
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            // Write column headers
            for (int col = 0; col < dt.Columns.Count; col++)
            {
                xlWorkSheet.Cells[1, col + 1] = dt.Columns[col].ColumnName;
            }

            // Write data rows
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    xlWorkSheet.Cells[row + 2, col + 1] = dt.Rows[row][col].ToString();
                }
            }

            // Optional: Autofit columns
            xlWorkSheet.Columns.AutoFit();

            // Show Excel
            xlApp.Visible = true;
        }

        private void customerListToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            //
        }
    }
}
