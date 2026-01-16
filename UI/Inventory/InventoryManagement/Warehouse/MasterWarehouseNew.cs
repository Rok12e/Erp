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
    public partial class MasterWarehouseNew : Form
    {
        private EventHandler wareHouseUpdatedHandler;
        private EventHandler itemUpdatedHandlerX;
        private EventHandler invoiceUpdatedHandler;
        private EventHandler purchaseUpdatedHandler;
        public MasterWarehouseNew()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            wareHouseUpdatedHandler = (sender, args) => BindWarehouse();
            invoiceUpdatedHandler = purchaseUpdatedHandler = (sender, args) => BindWarehouse();
            EventHub.wareHouse += wareHouseUpdatedHandler;
            //EventHub.Item += itemUpdatedHandler;
            EventHub.SalesInv += invoiceUpdatedHandler;
            EventHub.PurchaseInv += purchaseUpdatedHandler;
            headerUC1.FormText = this.Text;
        }
        private void MasterWarehouseNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.wareHouse -= wareHouseUpdatedHandler;
            //EventHub.Item -= itemUpdatedHandler;
            EventHub.SalesInv -= invoiceUpdatedHandler;
            EventHub.PurchaseInv -= purchaseUpdatedHandler;
        }

        private void MasterWarehouseNew_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateEmployees(cmbEmployee);
            BindCombos.PopulateCityAllNormalComboBox(guna2ComboBox1);
            BindWarehouse();
        }
        public void BindWarehouse()
        {
            DataTable dt;
            string query = @"Select id,Concat( code , ' - ',name) as 'Name' from tbl_warehouse where state = 0";
            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbEmployee.Text != "" && !chkAllEmployee.Checked)
            {
                query += " and emp_id = @emp_id";
                parameters.Add(DBClass.CreateParameter("emp_id", cmbEmployee.SelectedValue.ToString()));
            }
            if (guna2ComboBox1.Text != "" && !guna2CheckBox1.Checked)
            {
                query += " and city = @city";
                parameters.Add(DBClass.CreateParameter("city", guna2ComboBox1.Text));
            }
            if (cmbState.Text == "Active")
                query += " and state = 0";
            else if (cmbState.Text != "All")
                query += " and state != 0";
            dt = DBClass.ExecuteDataTable(query, parameters.ToArray());
            dgvCustomer.DataSource = null;
            if (dt != null)
            {
                dgvCustomer.DataSource = dt;
                dgvCustomer.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvCustomer.Columns["id"].Visible = false;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);

                if (dgvCustomer.Rows.Count > 0)
                {
                    deleteWarehouseToolStripMenuItem.Visible = UserPermissions.canDelete("Inventory Items");
                    editWarehouseToolStripMenuItem.Visible = UserPermissions.canEdit("Inventory Items");
                }
            }
        }
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            cmbEmployee.Enabled = !chkAllEmployee.Checked;
            guna2ComboBox1.Enabled = !guna2CheckBox1.Checked;
            BindWarehouse();
        }
        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWarehouse();
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {

        }
        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            new frmViewWarehouse(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
            if (dgvCustomer.Rows.Count == 0)
                return;
        }
        private void dgvCustomer_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvCustomer_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void dgvCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            BindItemDetails();
            BindItemTransaction();
        }
        private void BindItemDetails() {
            int wId = int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString());
            string query = @"SELECT w.*,e.NAME AS eName,a.NAME AS aName FROM tbl_warehouse w
                            LEFT JOIN tbl_employee e ON e.id = w.emp_id
                            LEFT JOIN tbl_coa_level_4 a ON a.id = w.account_id
                            WHERE w.id=@id";
            DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("id", wId));
            foreach (DataRow row in dt.Rows)
            {
                lblCode.Text = row["code"].ToString();
                lblName.Text = row["name"].ToString();
                lblEmployee.Text = row["eName"].ToString();
                lblCity.Text = row["city"].ToString();
                lblAccount.Text = row["aName"].ToString();
            }
        }
        private void BindItemTransaction()
        {
            dgvDetails.Rows.Clear();
            int _Id = int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString());
            //string query = "SELECT * FROM tbl_item_transaction WHERE item_id=@itemId";
            //string type = dgvCustomer.SelectedRows[0].Cells["type"].Value.ToString();
            //if (type == "12 - Service")
            //{
            //    query = "SELECT * FROM tbl_item_transaction WHERE item_id=@itemId";
            //} else if (type == "13 - Inventory Assembly")
            //{
            //    query = "SELECT * FROM tbl_item_transaction WHERE item_id=@itemId";
            //}
            decimal QtyBalance = 0;
            //DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("itemId", itemId));
            DataTable dt = DBClass.ExecuteDataTable(@"SELECT w.id, w.item_id,CONCAT(i.code ,' - ', i.name ) AS 'Item Name' ,i.barcode, i.cost_price AS 'Cost Price', i.sales_price AS 'Sales Price', w.qty AS 'QTY' ,
                                        i.TYPE FROM tbl_items_warehouse w INNER join tbl_items i ON w.item_id = i.id where w.warehouse_id = @id ",
                                        DBClass.CreateParameter("id", _Id.ToString()));
            foreach (DataRow row in dt.Rows)
            {
                string _id = row["id"].ToString();
                string _itemId = row["item_id"].ToString();
                string _ItemName = row["Item Name"].ToString();
                string _barcode = row["barcode"].ToString();
                decimal _costPrice = Convert.ToDecimal(row["Cost Price"]);
                decimal _salesPrice = Convert.ToDecimal(row["Sales Price"]);
                decimal _qty = Convert.ToDecimal(row["qty"]);
                string _type = row["type"].ToString();
                decimal _qtyBalance = _qty;

                QtyBalance += _qtyBalance;
                dgvDetails.Rows.Add(
                    _id,
                    _itemId,
                    _ItemName,
                    _barcode,
                    _costPrice.ToString("N2"),
                    _salesPrice.ToString("N2"),
                    _qty.ToString("N2"),
                    _type
                );
            }
            //dgvDetails.Columns["t_qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvDetails.Columns["t_cost_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvDetails.Columns["t_sales_price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            //dgvDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewWarehouse().ShowDialog();
        }

        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            new frmViewWarehouse(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
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
            BindWarehouse();
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

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow == null)
            {
                MessageBox.Show("Please select a item first.");
                return;
            }

            try
            {
                string customerName = dgvCustomer.CurrentRow.Cells["Name"].Value.ToString();
                string cityName = lblCity.Text.ToString();

                ExportToExcel(dgvDetails, customerName, cityName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export failed: " + ex.Message);
            }
        }
        private void ExportToExcel(DataGridView dgvTransactions, string customerName, string cityName)
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
            xlWorkSheet.Cells[row, 1] = "Warehouse Name:";
            xlWorkSheet.Cells[row, 2] = customerName;
            row++;
            xlWorkSheet.Cells[row, 1] = "City:";
            xlWorkSheet.Cells[row, 2] = cityName;
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
                MessageBox.Show("Please add warehouse first.");
                return;
            }

            string query = @"SELECT w.id,w.code,e.NAME AS EmployeeName,w.city,w.building_name,a.NAME AS AccountName,w.created_date FROM tbl_warehouse w
                            LEFT JOIN tbl_employee e ON e.id = w.emp_id
                            LEFT JOIN tbl_coa_level_4 a ON a.id = w.account_id 
                            WHERE w.id > 0";

            List<MySqlParameter> parameters = new List<MySqlParameter>();

            if (cmbEmployee.Text != "" && !chkAllEmployee.Checked)
            {
                query += " and w.emp_id = @emp_id";
                parameters.Add(DBClass.CreateParameter("emp_id", cmbEmployee.SelectedValue.ToString()));
            }
            if (guna2ComboBox1.Text != "" && !guna2CheckBox1.Checked)
            {
                query += " and w.city = @city";
                parameters.Add(DBClass.CreateParameter("city", guna2ComboBox1.Text));
            }
            if (cmbState.Text == "Active")
                query += " and w.state = 0";
            else if (cmbState.Text != "All")
                query += " and w.state != 0";

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

        private void newWarehouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewWarehouse().ShowDialog();
        }

        private void editWarehouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == 0)
                return;

            frmLogin.frmMain.LoadFormIntoPanel(new frmViewWarehouse(int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString())));
        }

        private void deleteWarehouseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvCustomer.Rows.Count == null)
                return;

            object objRecord = DBClass.ExecuteScalar(@"SELECT 
                                                      CASE 
                                                        WHEN EXISTS (SELECT 1 FROM tbl_items_warehouse WHERE warehouse_id = @warehouse_id)
                                                          OR EXISTS (SELECT 1 FROM tbl_item_transaction WHERE warehouse_id = @warehouse_id)
                                                        THEN 1
                                                        ELSE 0
                                                      END AS exists_flag;",
                DBClass.CreateParameter("warehouse_id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
            int oldRecord = (objRecord != null && objRecord != DBNull.Value) ? int.Parse(objRecord.ToString()) : 0;

            if (oldRecord > 0)
            {
                MessageBox.Show("Cannot delete this warehouse because it has associated transactions. Please transfer or remove the transactions first.");
                return;
            }
            else
            {
                var confirmResult = MessageBox.Show("Are you sure to delete this warehouse?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    DBClass.ExecuteNonQuery("DELETE FROM tbl_warehouse WHERE id = @id",
                        DBClass.CreateParameter("id", dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()));
                    Utilities.LogAudit(frmLogin.userId, "Delete Warehouse", "Warehouse", int.Parse(dgvCustomer.SelectedRows[0].Cells["id"].Value.ToString()), "Deleted Warehouse: " + dgvCustomer.SelectedRows[0].Cells["name"].Value.ToString());
                    MessageBox.Show("Warehouse deleted successfully.");
                    BindWarehouse();
                }
            }
        }

        private void TransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new MasterWarehouseTransfer());
        }

        private void transferHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLogin.frmMain.openChildForm(new frmViewWarehouseTransfer());
        }
    }
}
