using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;
using YamyProject.UI.Default;

namespace YamyProject
{
    public partial class frmOpeningQty : Form
    {
        DataTable datatable;
        string openingBalanceEquity = "0", assetAccount = "0", COGSAccount = "0", incomeAccount = "0";
        public frmOpeningQty()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "Opening Qty";
            headerUC1.FormText = this.Text;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmOpeningQty_Load(object sender, EventArgs e)
        {
            BindData();
            BindCombos.PopulateAllLevel4Account(cmbAccount);
            cmbAccount.SelectedValue = BindCombos.SelectDefaultLevelAccount("Inventory");

            openingBalanceEquity = BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString();
            assetAccount = BindCombos.SelectDefaultLevelAccount("Inventory").ToString();
            COGSAccount = BindCombos.SelectDefaultLevelAccount("COGS").ToString();
            incomeAccount = BindCombos.SelectDefaultLevelAccount("Sales").ToString();
        }
        private void BindData()
        {
            string query = @"SELECT Code,Name,cost_price CostPrice,sales_price SalesPrice,IFNULL((select name from tbl_unit WHERE id = unit_id),'') Unit,on_hand Qty,Barcode,min_amount MinAmount,max_amount MaxAmount,method Method,IFNULL((SELECT NAME FROM tbl_item_category WHERE id=category_id),'') Category,IFNULL((SELECT NAME FROM tbl_warehouse WHERE id=warehouse_id),'') Warehouse from tbl_items";
            datatable = DBClass.ExecuteDataTable(query);
            if (datatable != null && datatable.Rows.Count > 0)
            {
                dgvCustomer.DataSource = datatable;
                dgvCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
            }
        }
        private int GenerateNextCustomerCode()
        {
            string query = "SELECT RIGHT(code, 4) FROM tbl_items ORDER BY code DESC LIMIT 1";
            object result = DBClass.ExecuteScalar(query);
            int nextSerial = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
            string newItemCode = nextSerial.ToString().PadLeft(4, '0');
            return int.Parse(newItemCode);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int acId = cmbAccount.SelectedValue == null ? 0 : int.Parse(cmbAccount.SelectedValue.ToString());
            var loadingForm = new LoadingForm();
            Task.Run(() =>
            {
                if (chkRequiredDate())
                {
                    insertData(loadingForm, acId);
                }
                else
                {
                    // Close the dialog safely from background thread
                    loadingForm.Invoke(new Action(() => loadingForm.Close()));
                }
            });

            loadingForm.ShowDialog();
        }

        private string ConvertFromDecimal(object value)
        {
            return Utilities.ParseDecimalValue(value).ToString();
        }

        private void insertData(Form loadingForm, int cmbAccountId)
        {
            try
            {
                int formattedCode = GenerateNextCustomerCode();

                for (int i = 0; i < dgvCustomer.Rows.Count; i++)
                {
                    string code = (formattedCode + 1).ToString("D5");
                    if (dgvCustomer.Rows[i].Cells["Name"].Value != null && dgvCustomer.Rows[i].Cells["Name"].Value.ToString() != "")
                    {
                        string _code = dgvCustomer.Rows[i].Cells["Code"].Value.ToString().Trim();
                        string _name = dgvCustomer.Rows[i].Cells["Name"].Value.ToString().Trim();
                        string _costPrice = dgvCustomer.Rows[i].Cells["CostPrice"].Value.ToString().Trim() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["CostPrice"].Value.ToString().Trim());
                        string _salesPrice = dgvCustomer.Rows[i].Cells["SalesPrice"].Value.ToString().Trim() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["SalesPrice"].Value.ToString().Trim());
                        string _unit = dgvCustomer.Rows[i].Cells["Unit"].Value.ToString().Trim();
                        string _qty = dgvCustomer.Rows[i].Cells["Qty"].Value.ToString().Trim() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["Qty"].Value.ToString().Trim());
                        string _barcode = dgvCustomer.Rows[i].Cells["Barcode"].Value.ToString().Trim();
                        string _minAmount = dgvCustomer.Rows[i].Cells["MinAmount"].Value.ToString().Trim() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["MinAmount"].Value.ToString().Trim());
                        string _maxAmount = dgvCustomer.Rows[i].Cells["MaxAmount"].Value.ToString().Trim() == "" ? "0" : ConvertFromDecimal(dgvCustomer.Rows[i].Cells["MaxAmount"].Value.ToString().Trim());
                        string _method = dgvCustomer.Rows[i].Cells["Method"].Value.ToString().Trim() == "" ? "avg" : dgvCustomer.Rows[i].Cells["Method"].Value.ToString().Trim();
                        string _category = dgvCustomer.Rows[i].Cells["Category"].Value.ToString().Trim();
                        string _warehouse = dgvCustomer.Rows[i].Cells["Warehouse"].Value.ToString().Trim();
                        string warehouseId = "0", categoryId = "0", unitId = "0";
                        decimal totalValue = decimal.Parse(_costPrice) * decimal.Parse(_qty);

                        using (var reader = DBClass.ExecuteReader("SELECT 1 FROM tbl_items WHERE name = @name", DBClass.CreateParameter("name", _name)))
                        {
                            if (!reader.Read())
                            {
                                if (!string.IsNullOrEmpty(_warehouse))
                                {
                                    //warehouse
                                    using (var readerWarehouse = DBClass.ExecuteReader("SELECT id FROM tbl_warehouse WHERE name = @name", DBClass.CreateParameter("name", _warehouse)))
                                    {
                                        if (readerWarehouse.Read())
                                        {
                                            warehouseId = readerWarehouse["id"].ToString();
                                        }
                                        else
                                        {
                                            warehouseId = Convert.ToInt32(DBClass.ExecuteScalar(@"INSERT INTO tbl_warehouse (code, name, emp_id, city, building_name,
                                                    account_id, state, created_by, created_date) SELECT CONCAT('WH-', IFNULL(MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)), 0) + 1), @name, @emp_id,
                                                    @city, @building_name, @account_id, 0, @created_by, @created_date FROM tbl_warehouse; SELECT LAST_INSERT_ID();",
                                                    DBClass.CreateParameter("@name", _warehouse),
                                                    DBClass.CreateParameter("@emp_id", 0),
                                                    DBClass.CreateParameter("@city", ""),
                                                    DBClass.CreateParameter("@building_name", ""),
                                                    DBClass.CreateParameter("@account_id", cmbAccountId),
                                                    DBClass.CreateParameter("@created_by", frmLogin.userId),
                                                    DBClass.CreateParameter("@created_date", DateTime.Now.Date))).ToString();

                                            Utilities.LogAudit(frmLogin.userId, "Add Warehouse", "Warehouse", int.Parse(warehouseId), "Added Warehouse: " + _warehouse);
                                        }
                                    }
                                }
                                //category
                                if (!string.IsNullOrEmpty(_category))
                                {
                                    using (var readerCategory = DBClass.ExecuteReader("SELECT id FROM tbl_item_category WHERE name = @name", DBClass.CreateParameter("name", _category)))
                                    {
                                        if (readerCategory.Read())
                                        {
                                            categoryId = readerCategory["id"].ToString();
                                        }
                                        else
                                        {
                                            categoryId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_item_category (code,name) SELECT LPAD(IFNULL(MAX(CAST(code AS UNSIGNED)) + 1, 1),3,'0') AS next_code, @name FROM tbl_item_category;SELECT LAST_INSERT_ID();",
                                                DBClass.CreateParameter("name", _category)).ToString()).ToString();

                                            Utilities.LogAudit(frmLogin.userId, "Add Item Category", "Item Category", int.Parse(categoryId), "Added Item Category: " + _category);
                                        }
                                    }
                                }
                                //unit
                                if (!string.IsNullOrEmpty(_unit))
                                {
                                    using (var readerUnit = DBClass.ExecuteReader("SELECT id FROM tbl_unit WHERE name = @name", DBClass.CreateParameter("name", _name)))
                                    {
                                        if (readerUnit.Read())
                                        {
                                            unitId = readerUnit["id"].ToString();
                                        }
                                        else
                                        {
                                            unitId = Convert.ToInt32(DBClass.ExecuteScalar("INSERT INTO tbl_unit (name) VALUES (@name);SELECT LAST_INSERT_ID();",
                                                DBClass.CreateParameter("name", _category)).ToString()).ToString();

                                            Utilities.LogAudit(frmLogin.userId, "Add Unit", "Unit", int.Parse(unitId), "Added Unit: " + _unit);
                                        }
                                    }
                                }
                                string newItemCode = "";
                                if (string.IsNullOrEmpty(_code.Trim()))
                                {
                                    string query = "SELECT code FROM tbl_item_category WHERE id = @id";
                                    object result = DBClass.ExecuteScalar(query, DBClass.CreateParameter("@id", categoryId));
                                    string categoryCode = (result == DBNull.Value || result == null) ? "000" : result.ToString();

                                    string typeCategory = "11" + categoryCode;
                                    query = "SELECT RIGHT(code, 4) FROM tbl_items WHERE LEFT(code, 5) = @typeCategory ORDER BY code DESC LIMIT 1";
                                    result = DBClass.ExecuteScalar(query, DBClass.CreateParameter("@typeCategory", typeCategory));
                                    int nextSerial = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                                    string formattedSerial = nextSerial.ToString().PadLeft(4, '0');
                                    newItemCode = typeCategory + formattedSerial;
                                }
                                else
                                {
                                    newItemCode = _code;
                                }

                                int id = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(`code`,`warehouse_id`,  `type`,category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                    `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                    `min_amount`, `max_amount`, `on_hand`,method, `total_value`, `date`, `img`, `active`, `state`, 
                                    `created_By`, `created_date`) VALUES (
                                    @code,@warehouseId, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                    @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                    @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                    @created_By, @created_date); SELECT LAST_INSERT_ID();",
                                 DBClass.CreateParameter("code", newItemCode),
                                 DBClass.CreateParameter("warehouseId", warehouseId),
                                 DBClass.CreateParameter("type", "11 - Inventory Part"),
                                 DBClass.CreateParameter("category", categoryId),
                                 DBClass.CreateParameter("name", _name),
                                 DBClass.CreateParameter("unit_id", unitId),
                                 DBClass.CreateParameter("barcode", _barcode),
                                 DBClass.CreateParameter("cost_price", _costPrice),
                                 DBClass.CreateParameter("cogs_account_id", COGSAccount),
                                 DBClass.CreateParameter("vendor_id", "0"),
                                 DBClass.CreateParameter("sales_price", _salesPrice),
                                 DBClass.CreateParameter("income_account_id", incomeAccount),
                                 DBClass.CreateParameter("asset_account_id", assetAccount),
                                 DBClass.CreateParameter("min_amount", _minAmount),
                                 DBClass.CreateParameter("max_amount", _maxAmount),
                                 DBClass.CreateParameter("on_hand", _qty),
                                 DBClass.CreateParameter("method", _method.ToString().Trim().ToLower()),
                                 DBClass.CreateParameter("total_value", totalValue),
                                 DBClass.CreateParameter("date", dtpDate.Value.Date),
                                 DBClass.CreateParameter("img", ""),
                                 DBClass.CreateParameter("active", 0),
                                 DBClass.CreateParameter("state", 0),
                                 DBClass.CreateParameter("created_By", frmLogin.userId),
                                 DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
                                //if (cmbType.Text != "12 - Service" && _unit != null)
                                //    insertItemUnits(id);
                                //if (cmbType.Text == "13 - Inventory Assembly")
                                //    insertItemsAssembly(id);
                                if (_qty != "0" || decimal.Parse(_qty) > 0)
                                {
                                    CommonInsert.InsertItemTransaction(dtpDate.Value.Date, "Opening Qty", "0", id.ToString(), _costPrice, _qty, _salesPrice, "0", _qty, "Opening Balance", "0");
                                    //journal transactions

                                    CommonInsert.InsertTransactionEntry(dtpDate.Value.Date, cmbAccountId.ToString(), totalValue.ToString(), "0",
                                        id.ToString(), id.ToString(), "Opening Qty", "Opening Balance - Item Code - " + _code, frmLogin.userId, DateTime.Now.Date);
                                    CommonInsert.InsertTransactionEntry(dtpDate.Value.Date, openingBalanceEquity.ToString(), "0", totalValue.ToString(),
                                        id.ToString(), "0", "Opening Qty", "Opening Balance Equity - Item Code - " + _code, frmLogin.userId, DateTime.Now.Date);
                                }

                                Utilities.LogAudit(frmLogin.userId, "Add Item", "Item", id, "Added Item: " + _name + " with Code: " + newItemCode);
                            }
                        }
                    }
                }
                loadingForm.Invoke(new Action(() => loadingForm.Close()));
                MessageBox.Show("Successfully saved! Item List", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                loadingForm.Invoke(new Action(() => loadingForm.Close()));
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool chkRequiredDate()
        {
            if (dgvCustomer.Rows.Count == 1)
            {
                MessageBox.Show("Please Enter Date First");
                return false;
            }
            else if (int.Parse(openingBalanceEquity) == 0)
            {
                object result = DBClass.ExecuteScalar(@"SELECT id FROM tbl_coa_level_4 WHERE name = 'Opening Balance Equity'");
                if (result != null && result != DBNull.Value)
                {
                    openingBalanceEquity = result.ToString();
                }

                if (int.Parse(openingBalanceEquity) == 0)
                {
                    MessageBox.Show("Cannot make opening balance without opening balance equity account");
                    return false;
                }
            }

            return true;
        }
        private decimal GetDecimalValue(DataGridViewRow row, string columnName)
        {
            decimal result;
            var cellValue = row.Cells[columnName].Value;
            if (cellValue != null && decimal.TryParse(cellValue.ToString(), out result))
                return result;
            else
                return 0;
        }
        
        private void btnExport_Click(object sender, EventArgs e)
        {
            string type = "ItemList";
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage())
                {
                    type = string.IsNullOrEmpty(type) ? "sheet1" : type;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(type);
                    worksheet.Cells["A1"].LoadFromDataTable(datatable, true);
                    
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fi = new FileInfo(saveFileDialog.FileName);
                        package.SaveAs(fi);
                        MessageBox.Show("Export successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (chkCustomizedFile.Checked)
            {
                frmImportItems importForm = new frmImportItems();
                if (importForm.ShowDialog() == DialogResult.OK)
                {
                    DataTable importedData = importForm.ImportedData;
                    dgvCustomer.DataSource = importedData;
                    LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);
                }
            }
            else
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                    openFileDialog.Title = "Select Excel File";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;
                        LoadExcelSheets(filePath);
                    }
                }
            }
        }
        private void LoadExcelSheets(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                dgvCustomer.Columns.Clear();
                dgvCustomer.DataSource = null;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                foreach (var sheet in package.Workbook.Worksheets)
                {
                    if (sheet.Dimension != null)
                    {
                        DataTable dataTable = new DataTable(sheet.Name);

                        for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                        {
                            string columnName = sheet.Cells[1, col].Text.Trim();
                            if (string.IsNullOrWhiteSpace(columnName)) columnName = "Column " + col;
                            dataTable.Columns.Add(columnName);
                        }

                        for (int row = 2; row <= sheet.Dimension.End.Row; row++)
                        {
                            bool isRowEmpty = true;
                            DataRow dataRow = dataTable.NewRow();

                            for (int col = 1; col <= sheet.Dimension.End.Column; col++)
                            {
                                string cellValue = sheet.Cells[row, col].Text.Trim();

                                if (!string.IsNullOrWhiteSpace(cellValue))
                                    isRowEmpty = false;

                                dataRow[col - 1] = cellValue;
                            }

                            if (!isRowEmpty)
                                dataTable.Rows.Add(dataRow);
                        }

                        //RemoveEmptyColumns(dataTable);

                        dgvCustomer.DataSource = dataTable;
                        foreach (DataGridViewColumn column in dgvCustomer.Columns)
                        {
                            if (column.HeaderText == "name" || column.HeaderText == "Category" || column.HeaderText == "Warehouse" || column.HeaderText == "Unit")
                            {
                                column.Width = 550;
                            }
                            else
                            {
                                column.Width = 200;
                            }
                        }
                        LocalizationManager.LocalizeDataGridViewHeaders(dgvCustomer);

                    }
                }
            }
        }
        private void RemoveEmptyColumns(DataTable dt)
        {
            List<DataColumn> emptyColumns = new List<DataColumn>();

            foreach (DataColumn col in dt.Columns)
            {
                bool isEmpty = true;

                foreach (DataRow row in dt.Rows)
                {
                    if (!string.IsNullOrWhiteSpace(row[col].ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                    emptyColumns.Add(col);
            }

            foreach (DataColumn col in emptyColumns)
            {
                dt.Columns.Remove(col);
            }
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCode.Text) && txtCode.Text.Length > 0)
            {
                //
            }
        }
    }
}

public class frmImportItems : Form
{
    private TextBox txtStartCell, txtEndCell;
    private TextBox txtCodeCell, txtNameCell, txtCostPriceCell, txtSalesPriceCell, txtUnitCell, txtQtyCell;
    private TextBox txtBarcodeCell, txtMinAmountCell, txtMaxAmountCell, txtMethodCell, txtCategoryCell, txtWarehouseCell;
    private Button btnLoadExcel, btnImport;

    private DataGridView dataGridView;
    private string excelPath = string.Empty;
    DataTable dtFiltered = new DataTable();
    public DataTable ImportedData { get; private set; }

    public frmImportItems()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        this.Text = "Excel Import Form";
        this.Size = new Size(1624, 900);
        this.StartPosition = FormStartPosition.CenterScreen;

        // ==== Top Panel ====
        Panel panelTop = new Panel()
        {
            Dock = DockStyle.Top,
            Height = 60,
            Padding = new Padding(10),
        };

        // Starting X position for controls
        int x = 10;
        int spacing = 8;
        int labelWidth = 80;
        int textboxWidth = 15;
        int controlHeight = 28;

        // Excel buttons
        btnLoadExcel = new Button() { Text = "Load Excel", Size = new Size(100, controlHeight), Location = new Point(x, 10) };
        x += 100 + spacing;

        btnImport = new Button() { Text = "Set Data", Size = new Size(80, controlHeight), Location = new Point(x, 10) };
        x += 80 + spacing;

        // Start Cell
        panelTop.Controls.Add(new Label() { Text = "Start Cell:", AutoSize = true, Location = new Point(x, 15) });
        x += labelWidth;
        txtStartCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 13, 12) };
        x += textboxWidth + spacing;

        // End Cell
        panelTop.Controls.Add(new Label() { Text = "End Cell:", AutoSize = true, Location = new Point(x-10, 15) });
        x += labelWidth;
        txtEndCell = new TextBox() { Width = textboxWidth, Location = new Point(x - 27, 12) };
        x += textboxWidth + spacing;
        
        panelTop.Controls.Add(new Label() { Text = "Code:", AutoSize = true, Location = new Point(x-26, 15) });
        x += labelWidth;
        txtCodeCell = new TextBox() { Width = textboxWidth, Location = new Point(x-60, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Name:", AutoSize = true, Location = new Point(x-60, 15) });
        x += labelWidth;
        txtNameCell = new TextBox() { Width = textboxWidth, Location = new Point(x-90, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "CostPrice:", AutoSize = true, Location = new Point(x-90, 15) });
        x += labelWidth;
        txtCostPriceCell = new TextBox() { Width = textboxWidth, Location = new Point(x-96, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "SalesPrice:", AutoSize = true, Location = new Point(x-100, 15) });
        x += labelWidth;
        txtSalesPriceCell = new TextBox() { Width = textboxWidth, Location = new Point(x-97, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Unit:", AutoSize = true, Location = new Point(x-100, 15) });
        x += labelWidth;
        txtUnitCell = new TextBox() { Width = textboxWidth, Location = new Point(x-140, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Qty:", AutoSize = true, Location = new Point(x-145, 15) });
        x += labelWidth;
        txtQtyCell = new TextBox() { Width = textboxWidth, Location = new Point(x-190, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Barcode:", AutoSize = true, Location = new Point(x-185, 15) });
        x += labelWidth;
        txtBarcodeCell = new TextBox() { Width = textboxWidth, Location = new Point(x-200, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "MinAmount:", AutoSize = true, Location = new Point(x-195, 15) });
        x += labelWidth;
        txtMinAmountCell = new TextBox() { Width = textboxWidth, Location = new Point(x-190, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "MaxAmount:", AutoSize = true, Location = new Point(x-190, 15) });
        x += labelWidth;
        txtMaxAmountCell = new TextBox() { Width = textboxWidth, Location = new Point(x-180, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Method:", AutoSize = true, Location = new Point(x-170, 15) });
        x += labelWidth;
        txtMethodCell = new TextBox() { Width = textboxWidth, Location = new Point(x-190, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Category:", AutoSize = true, Location = new Point(x-180, 15) });
        x += labelWidth;
        txtCategoryCell = new TextBox() { Width = textboxWidth, Location = new Point(x-190, 12) };
        x += textboxWidth + spacing;

        panelTop.Controls.Add(new Label() { Text = "Warehouse:", AutoSize = true, Location = new Point(x-190, 15) });
        x += labelWidth;
        txtWarehouseCell = new TextBox() { Width = textboxWidth, Location = new Point(x-170, 12) };
        x += textboxWidth + spacing;

        // Add controls to the panel
        panelTop.Controls.Add(txtStartCell);
        panelTop.Controls.Add(txtEndCell);
        panelTop.Controls.Add(txtCodeCell);
        panelTop.Controls.Add(txtNameCell);
        panelTop.Controls.Add(txtCostPriceCell);
        panelTop.Controls.Add(txtSalesPriceCell);
        panelTop.Controls.Add(txtUnitCell);
        panelTop.Controls.Add(txtQtyCell);
        panelTop.Controls.Add(txtBarcodeCell);
        panelTop.Controls.Add(txtMinAmountCell);
        panelTop.Controls.Add(txtMaxAmountCell);
        panelTop.Controls.Add(txtMethodCell);
        panelTop.Controls.Add(txtCategoryCell);
        panelTop.Controls.Add(txtWarehouseCell);

        // Add the Excel buttons
        panelTop.Controls.Add(btnLoadExcel);
        panelTop.Controls.Add(btnImport);

        // Add top panel to form
        this.Controls.Add(panelTop);


        btnLoadExcel.Click += BtnLoadExcel_Click;
        btnImport.Click += BtnImport_Click;

        // ==== Main Panel ====
        Panel panelMain = new Panel()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(10),
        };

        dataGridView = new DataGridView()
        {
            Dock = DockStyle.Fill,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        panelMain.Controls.Add(dataGridView);

        // Add both panels to form
        this.Controls.Add(panelMain);
        this.Controls.Add(panelTop);

        // Digit-only for Start and End Cell
        txtStartCell.KeyPress += TxtIntOnly_KeyPress;
        txtEndCell.KeyPress += TxtIntOnly_KeyPress;

        // Letter-only for Excel column letter inputs
        txtCodeCell.KeyPress += TxtLettersOnly_KeyPress;
        txtNameCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCostPriceCell.KeyPress += TxtLettersOnly_KeyPress;
        txtSalesPriceCell.KeyPress += TxtLettersOnly_KeyPress;
        txtUnitCell.KeyPress += TxtLettersOnly_KeyPress;
        txtQtyCell.KeyPress += TxtLettersOnly_KeyPress;
        txtBarcodeCell.KeyPress += TxtLettersOnly_KeyPress;
        txtMinAmountCell.KeyPress += TxtLettersOnly_KeyPress;
        txtMaxAmountCell.KeyPress += TxtLettersOnly_KeyPress;
        txtMethodCell.KeyPress += TxtLettersOnly_KeyPress;
        txtCategoryCell.KeyPress += TxtLettersOnly_KeyPress;
        txtWarehouseCell.KeyPress += TxtLettersOnly_KeyPress;

    }
    private void BtnImport_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            MessageBox.Show("Please load an Excel file first.");
            return;
        }
        else
        {
            //dataGridView.Rows.Clear();
            //dataGridView.Columns.Clear();
            if (dataGridView.Rows.Count > 0)
            {
                int startCol = ToNumber(txtStartCell.Text);
                int endCol = ToNumber(txtEndCell.Text);

                // Check for invalid inputs
                if (startCol == -1 || endCol == -1 || endCol < startCol)
                {
                    MessageBox.Show("Invalid start or end column.");
                    return;
                }

                // Map new fields to column numbers
                int colCode = ExcelColumnNameToNumber(txtCodeCell.Text);
                int colName = ExcelColumnNameToNumber(txtNameCell.Text);
                int colCostPrice = ExcelColumnNameToNumber(txtCostPriceCell.Text);
                int colSalesPrice = ExcelColumnNameToNumber(txtSalesPriceCell.Text);
                int colUnit = ExcelColumnNameToNumber(txtUnitCell.Text);
                int colQty = ExcelColumnNameToNumber(txtQtyCell.Text);
                int colBarcode = ExcelColumnNameToNumber(txtBarcodeCell.Text);
                int colMinAmount = ExcelColumnNameToNumber(txtMinAmountCell.Text);
                int colMaxAmount = ExcelColumnNameToNumber(txtMaxAmountCell.Text);
                int colMethod = ExcelColumnNameToNumber(txtMethodCell.Text);
                int colCategory = ExcelColumnNameToNumber(txtCategoryCell.Text);
                int colWarehouse = ExcelColumnNameToNumber(txtWarehouseCell.Text);

                using (var package = new ExcelPackage(new FileInfo(excelPath)))
                {
                    // Create DataTable with new columns
                    dtFiltered.Columns.Add("Code");
                    dtFiltered.Columns.Add("Name");
                    dtFiltered.Columns.Add("CostPrice");
                    dtFiltered.Columns.Add("SalesPrice");
                    dtFiltered.Columns.Add("Unit");
                    dtFiltered.Columns.Add("Qty");
                    dtFiltered.Columns.Add("Barcode");
                    dtFiltered.Columns.Add("MinAmount");
                    dtFiltered.Columns.Add("MaxAmount");
                    dtFiltered.Columns.Add("Method");
                    dtFiltered.Columns.Add("Category");
                    dtFiltered.Columns.Add("Warehouse");

                    var ws = package.Workbook.Worksheets[0];
                    for (int row = ws.Dimension.Start.Row + 1; row <= ws.Dimension.End.Row; row++)
                    {
                        if (IsRowEmpty(ws, row, endCol)) continue;

                        DataRow dr = dtFiltered.NewRow();

                        dr["Code"] = (colCode != -1) ? ws.Cells[row, colCode].Text : "";
                        dr["Name"] = (colName != -1) ? ws.Cells[row, colName].Text : "";
                        dr["CostPrice"] = (colCostPrice != -1) ? ws.Cells[row, colCostPrice].Text : "0";
                        dr["SalesPrice"] = (colSalesPrice != -1) ? ws.Cells[row, colSalesPrice].Text : "0";
                        dr["Unit"] = (colUnit != -1) ? ws.Cells[row, colUnit].Text : "";
                        dr["Qty"] = (colQty != -1) ? ws.Cells[row, colQty].Text : "0";
                        dr["Barcode"] = (colBarcode != -1) ? ws.Cells[row, colBarcode].Text : "";
                        dr["MinAmount"] = (colMinAmount != -1) ? ws.Cells[row, colMinAmount].Text : "0";
                        dr["MaxAmount"] = (colMaxAmount != -1) ? ws.Cells[row, colMaxAmount].Text : "0";
                        dr["Method"] = (colMethod != -1) ? ws.Cells[row, colMethod].Text : "";
                        dr["Category"] = (colCategory != -1) ? ws.Cells[row, colCategory].Text : "";
                        dr["Warehouse"] = (colWarehouse != -1) ? ws.Cells[row, colWarehouse].Text : "";

                        dtFiltered.Rows.Add(dr);
                    }

                    // Now you can bind the filtered table somewhere or return it
                    dataGridView.DataSource = dtFiltered;
                }

                this.ImportedData = dtFiltered;
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            else
            {
                MessageBox.Show("Please load an Excel file first.");
                return;
            }
        }
    }

    private int ToNumber(string text)
    {
        if (string.IsNullOrEmpty(text)) return -1;
        else return int.Parse(text);
    }

    int ExcelColumnNameToNumber(string columnName)
    {
        if (string.IsNullOrEmpty(columnName)) return -1;

        columnName = columnName.ToUpperInvariant();
        int sum = 0;
        foreach (char c in columnName)
        {
            if (c < 'A' || c > 'Z') return -1; // invalid char
            sum *= 26;
            sum += (c - 'A' + 1);
        }
        return sum;
    }
    private void TxtIntOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
        // Allow control keys like Backspace
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true; // Reject non-digit keys
        }
    }
    private void TxtLettersOnly_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
        {
            e.Handled = true; // block non-letter keys
        }
    }

    private void BtnLoadExcel_Click(object sender, EventArgs e)
    {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.Filter = "Excel Files (*.xlsx)|*.xlsx";
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            excelPath = ofd.FileName;
            ImportToGrid();
        }
    }

    private void ImportToGrid()
    {
        if (string.IsNullOrEmpty(excelPath))
        {
            MessageBox.Show("Please load an Excel file first.");
            return;
        }

        try
        {
            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                var ws = package.Workbook.Worksheets[0];
                int startRow = ws.Dimension.Start.Row;
                int endRow = ws.Dimension.End.Row;
                int startCol = ws.Dimension.Start.Column;
                int endCol = ws.Dimension.End.Column;

                DataTable dt = new DataTable();

                // First column: "Index" or "Row"
                dt.Columns.Add("Index");

                // Set headers manually as A, B, C, etc.
                for (int col = startCol; col <= endCol; col++)
                {
                    string colLetter = GetExcelColumnLetter(col);
                    dt.Columns.Add(colLetter);
                }

                // Fill data from all rows
                for (int row = startRow; row <= endRow; row++)
                {
                    DataRow dr = dt.NewRow();

                    dr["Index"] = row.ToString();  // Add Excel-like row number

                    for (int col = startCol; col <= endCol; col++)
                    {
                        dr[col - startCol + 1] = ws.Cells[row, col].Text;
                    }

                    dt.Rows.Add(dr);
                }

                dataGridView.DataSource = dt;

                dataGridView.ReadOnly = true;
                dataGridView.RowHeadersVisible = false;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error reading Excel: {ex.Message}");
        }
    }
    private string GetExcelColumnLetter(int col)
    {
        string columnString = "";
        int columnNumber = col;
        while (columnNumber > 0)
        {
            int currentLetterNumber = (columnNumber - 1) % 26;
            char currentLetter = (char)(currentLetterNumber + 65);
            columnString = currentLetter + columnString;
            columnNumber = (columnNumber - 1) / 26;
        }
        return columnString;
    }

    private bool IsRowEmpty(ExcelWorksheet ws, int row, int maxCol)
    {
        for (int col = 1; col <= maxCol; col++)
        {
            if (!string.IsNullOrWhiteSpace(ws.Cells[row, col].Text))
                return false;
        }
        return true;
    }
}