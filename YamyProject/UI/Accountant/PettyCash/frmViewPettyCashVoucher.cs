using CrystalDecisions.CrystalReports.Engine;
using DocumentFormat.OpenXml.Spreadsheet;
using Guna.UI2.WinForms;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Xceed.Document.NET;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewPettyCashVoucher : Form
    {
        int id;
        decimal totalAmount = 0;
        string code;

        public frmViewPettyCashVoucher(int id = 0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;

            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /**/
        // Guna2DataGridView does not have a built-in calendar column.
        // But you can add a standard DataGridViewCalendarColumn (custom) and it works fine with Guna2DataGridView.

        // Step 1: Create a custom Calendar column class
        public class DataGridViewCalendarColumn : DataGridViewColumn
        {
            public DataGridViewCalendarColumn() : base(new DataGridViewCalendarCell())
            {
            }

            public override DataGridViewCell CellTemplate
            {
                get { return base.CellTemplate; }
                set
                {
                    if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewCalendarCell)))
                    {
                        throw new InvalidCastException("Must be a DataGridViewCalendarCell");
                    }
                    base.CellTemplate = value;
                }
            }
        }

        public class DataGridViewCalendarCell : DataGridViewTextBoxCell
        {
            public DataGridViewCalendarCell() : base()
            {
                this.Style.Format = "d"; // short date format
            }

            public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            {
                base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
                CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;
                if (this.Value == null || this.Value == DBNull.Value)
                {
                    ctl.Value = DateTime.Today;
                }
                else
                {
                    ctl.Value = Convert.ToDateTime(this.Value);
                }
            }

            public override Type EditType => typeof(CalendarEditingControl);
            public override Type ValueType => typeof(DateTime);
            public override object DefaultNewRowValue => DateTime.Today;
        }

        class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
        {
            DataGridView dataGridView;
            private bool valueChanged = false;
            int rowIndex;

            public CalendarEditingControl()
            {
                this.Format = DateTimePickerFormat.Short;
            }

            public object EditingControlFormattedValue
            {
                get { return this.Value.ToShortDateString(); }
                set
                {
                    if (value is string)
                    {
                        this.Value = DateTime.Parse((string)value);
                    }
                }
            }

            public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
            {
                return EditingControlFormattedValue;
            }

            public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
                this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
                this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
            }

            public int EditingControlRowIndex
            {
                get { return rowIndex; }
                set { rowIndex = value; }
            }

            public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
            {
                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Right:
                    case Keys.Home:
                    case Keys.End:
                    case Keys.PageDown:
                    case Keys.PageUp:
                        return true;
                    default:
                        return !dataGridViewWantsInputKey;
                }
            }

            public void PrepareEditingControlForEdit(bool selectAll) { }

            public bool RepositionEditingControlOnValueChange => false;

            public DataGridView EditingControlDataGridView
            {
                get { return dataGridView; }
                set { dataGridView = value; }
            }

            public bool EditingControlValueChanged
            {
                get { return valueChanged; }
                set { valueChanged = value; }
            }

            public Cursor EditingPanelCursor => base.Cursor;

            protected override void OnValueChanged(EventArgs eventargs)
            {
                valueChanged = true;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(eventargs);
            }
        }

        /**/
        private void frmViewPettyCashVoucher_Load(object sender, EventArgs e)
        {
            dtOpen.Value = DateTime.Now.Date;
            DataGridViewCalendarColumn calendarColumn = new DataGridViewCalendarColumn();
            calendarColumn.HeaderText = "Date";
            calendarColumn.Name = "colDate";
            int colIndex = dgvInv.Columns["colDate"].Index;
            // Remove the old column
            dgvInv.Columns.RemoveAt(colIndex);
            // Insert the calendar column at the same position
            dgvInv.Columns.Insert(colIndex, calendarColumn);
            string queryC = "select id,name from tbl_petty_cash_category";
            DataTable dtc = DBClass.ExecuteDataTable(queryC);
            DataGridViewComboBoxColumn cmbCategory = (DataGridViewComboBoxColumn)dgvInv.Columns["category"];
            if( cmbCategory != null)
            {
                cmbCategory.DataSource = dtc;
                cmbCategory.DisplayMember = "name";
                cmbCategory.ValueMember = "id";
            }
            //cmbCategory.Items.AddRange("Salary", "General", "Purchase", "Subcontractor");

            //cost center
            string queryCC = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_sub_cost_center";
            DataTable dtCC = DBClass.ExecuteDataTable(queryCC);
            DataGridViewComboBoxColumn cmbCostcenter = dgvInv.Columns["CostCenter"] as DataGridViewComboBoxColumn;
            if (cmbCostcenter != null)
            {
                cmbCostcenter.DataSource = dtCC;
                cmbCostcenter.DisplayMember = "name";
                cmbCostcenter.ValueMember = "id";
            }

            //queryCC = "select CONCAT(CODE , ' - ' , NAME) AS name, id from tbl_vendor";
            //dtCC = DBClass.ExecuteDataTable(queryCC);
            //DataGridViewComboBoxColumn cmbLinkType = dgvInv.Columns["humId"] as DataGridViewComboBoxColumn;
            //if (cmbLinkType != null)
            //{
            //    cmbLinkType.DataSource = dtCC;
            //    cmbLinkType.DisplayMember = "name";
            //    cmbLinkType.ValueMember = "id";
            //}
            if(id <= 0)
                BindCombos.PopulatePettyCash(cmbPettyCash);
            else
                BindCombos.PopulateEmployeesToPettyCardAllEmp(cmbPettyCash);

            txtPVCode.Text = GenerateNextPettyCashCode();
            txtNo.Text = txtPVCode.Text;

            if (id > 0)
            {
                btnSave.Enabled = BtnSaveNew.Enabled = UserPermissions.canEdit("Vouchers");
                BtnDelete.Enabled = UserPermissions.canDelete("Vouchers");
                BindVoucher();
            }
            else
            {
                SetDefault();
            }
            //DataGridViewCell buttonColumn = dgvInv.CurrentRow.Cells["humId"];
            //loadNameList(buttonColumn);
            LocalizationManager.LocalizeDataGridViewHeaders(dgvInv);
        }

        private void BindVoucher()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT * FROM tbl_petty_cash WHERE id = " + id))
                if (reader.Read())
                {
                    txtId.Text = reader["id"].ToString();
                    txtPVCode.Text = reader["code"].ToString();
                    dtOpen.Value = DateTime.Parse(reader["voucher_date"].ToString());
                    code = reader["code"].ToString();
                    string cash_account_id = reader["cash_account_id"].ToString();
                    string employee_id = reader["employee_id"].ToString();
                    cmbPettyCash.SelectedValue = int.Parse(employee_id);
                    txtAmount.Text = reader["total"].ToString();
                    //txtDescription1.Text = reader["description"].ToString();
                    //cmbAccountName.SelectedValue = reader["debit_account_id"].ToString();
                    /*
                     * (code, voucher_date, cash_account_id, employee_id, notes, total, created_by
                     */

                    loadPettyCashDataDetails();
                }
        }

        private void loadPettyCashDataDetails()
        {
            dgvInv.Rows.Clear();

            int serialNumber = 1;
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"
                SELECT ROW_NUMBER() OVER (ORDER BY entry_date) AS SN, 
                       dt.id, dt.petty_cash_id, dt.entry_date, dt.ref_id, dt.hum_id, dt.hum_name, dt.category,
                       dt.cost_center_id, dt.description, dt.amount, dt.project_id, dt.note
                FROM tbl_petty_cash_details dt 
                WHERE dt.petty_cash_id = @id",
                DBClass.CreateParameter("id", id)))
            {
                while (reader.Read())
                {
                    string inv_id = "0";
                    string inv_code = reader["ref_id"].ToString();
                    string inv_date = DateTime.Parse(reader["entry_date"].ToString()).ToShortDateString();
                    string description = reader["description"].ToString();
                    string PettyCashAmount = Utilities.FormatDecimal(reader["amount"].ToString());
                    string _humId = reader["hum_id"].ToString();
                    string _humName = reader["hum_name"].ToString();
                    string vCategory = reader["category"].ToString();
                    string colNote = reader["note"].ToString() ?? "";

                    string cost_center_id = reader["cost_center_id"].ToString();
                    if (string.IsNullOrEmpty(cost_center_id))
                        cost_center_id = null; // optional: null if empty

                    _humId = _humId == "0" ? null : _humId;
                    // Add row with null placeholder for CostCenter
                    dgvInv.Rows.Add(serialNumber.ToString(), inv_date, inv_id, null, _humName, _humId, null, description, PettyCashAmount, colNote);

                    //Get the last added row(always last)
                    DataGridViewComboBoxCell comboCellCategory = dgvInv.Rows[dgvInv.Rows.Count - 2].Cells["category"] as DataGridViewComboBoxCell;
                    if (comboCellCategory != null && !string.IsNullOrEmpty(vCategory) && vCategory != "0")
                    {
                        comboCellCategory.Value = int.Parse(vCategory);
                    }
                    //DataGridViewComboBoxCell comboCellType = dgvInv.Rows[dgvInv.Rows.Count - 2].Cells["humId"] as DataGridViewComboBoxCell;
                    //if (comboCellType != null && !string.IsNullOrEmpty(_humId) && _humId != "0")
                    //{
                    //    comboCellType.Value = int.Parse(_humId);
                    //}
                    DataGridViewComboBoxCell comboCellCostCenter = dgvInv.Rows[dgvInv.Rows.Count - 2].Cells["CostCenter"] as DataGridViewComboBoxCell;
                    if (comboCellCostCenter != null && !string.IsNullOrEmpty(cost_center_id) && cost_center_id != "0")
                    {
                        comboCellCostCenter.Value = int.Parse(cost_center_id);
                    }

                    serialNumber++;
                }

                calculateTotal();
            }
        }

        private string GenerateNextPettyCashCode()
        {
            string newCode = "PC-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(id) AS lastId FROM tbl_petty_cash"))
            {
                if (reader.Read() && reader["lastId"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastId"].ToString()) + 1;
                    txtId.Text = code.ToString();
                }
            }

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)) AS lastCode FROM tbl_petty_cash"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "PC-" + code.ToString("D4");
                }
            }

            return newCode;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertPV())
                {
                    EventHub.RefreshPettyCashVoucher();
                    MessageBox.Show("The PettyCash Voucher Paid");
                    if (chkPrint.Checked == true)
                    {
                        loadPrint();
                    }
                }
            }
            else
            {
                if (updatePV())
                {
                    EventHub.RefreshPettyCashVoucher();
                    MessageBox.Show("The PettyCash Voucher Updated !");
                    if (chkPrint.Checked == true)
                    {
                        loadPrint();
                    }
                }
            }
        }

        private void loadPrint()
        {
            ShowReport();
        }

        private bool insertPV()
        {
            if (!chkRequireData())
                return false;
            code = GenerateNextPettyCashCode();
            id = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_petty_cash(code, voucher_date, cash_account_id, employee_id, notes, total, created_by) 
                                                            VALUES (@code, @voucher_date, @cash_account_id, @employee_id, @notes, @total, @created_by); SELECT LAST_INSERT_ID();",
                                                            DBClass.CreateParameter("code", code),
                                                            DBClass.CreateParameter("voucher_date", dtOpen.Value.Date),
                                                            DBClass.CreateParameter("cash_account_id", cmbPettyCash.SelectedValue ?? 0),
                                                            DBClass.CreateParameter("employee_id", employeeId),
                                                            DBClass.CreateParameter("notes", ""),
                                                            DBClass.CreateParameter("total", txtAmount.Text),
                                                            DBClass.CreateParameter("created_by", frmLogin.userId)).ToString());

            insertINV(id);
            Utilities.LogAudit(frmLogin.userId, "Insert PettyCash Voucher", "PettyCash Voucher", id, "Inserted PettyCash Voucher: " + code);

            return true;
        }

        private bool updatePV()
        {
            if (!chkRequireData())
                return false;
            DBClass.ExecuteScalar(@"UPDATE tbl_petty_cash SET code = @code, voucher_date = @voucher_date, cash_account_id = @cash_account_id, employee_id = @employee_id, total = @total, notes = @notes WHERE id = @id;",
                                    DBClass.CreateParameter("id", id),
                                    DBClass.CreateParameter("code", code),
                                    DBClass.CreateParameter("voucher_date", dtOpen.Value.Date),
                                    DBClass.CreateParameter("cash_account_id", cmbPettyCash.SelectedValue ?? 0),
                                    DBClass.CreateParameter("employee_id", employeeId),
                                    DBClass.CreateParameter("total", txtAmount.Text),
                                    DBClass.CreateParameter("notes", "")
                                    );
            DBClass.ExecuteNonQuery("Delete from tbl_petty_cash_details where petty_cash_id=@id", DBClass.CreateParameter("id", id));
            CommonInsert.DeleteTransactionEntry(id, "PettyCash");

            insertINV(id);
            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "PettyCash");
            Utilities.LogAudit(frmLogin.userId, "Update PettyCash Voucher", "PettyCash Voucher", id, "Updated PettyCash Voucher: " + code);
            return true;
        }

        private bool chkRequireData()
        {
            if (txtAmount.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter PettyCash Amount.");
                return false;
            }
            //if (cmbAccountName.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Choose Debit Account Name");
            //    return false;
            //}
            if ((txtAmount.Text == "" || decimal.Parse(txtAmount.Text) == 0) && dgvInv.Rows.Count == 0)
            {
                MessageBox.Show("Enter Amount");
                return false;
            }

            return true;
        }

        private void insertINV(int pvId)
        {
            // no, humId, colDate, invId, Category, CostCenter, Description, Amount, colNote

            for (int i = 0; i < dgvInv.Rows.Count; i++)
            {
                if (dgvInv.Rows[i].Cells["Amount"].Value == null ||
                    dgvInv.Rows[i].Cells["Amount"].Value.ToString() == "" ||
                    decimal.Parse(dgvInv.Rows[i].Cells["Amount"].Value.ToString()) == 0)
                    continue;
                
                DateTime date = DateTime.Parse(dgvInv.Rows[i].Cells["colDate"].Value.ToString());
                var invId = dgvInv.Rows[i].Cells["invId"].Value?.ToString() != "" ? dgvInv.Rows[i].Cells["invId"].Value?.ToString() : "0";

                string categoryId = dgvInv.Rows[i].Cells["Category"].Value == null ? "0" : dgvInv.Rows[i].Cells["Category"].Value?.ToString() ?? "0";
                string CostCenterId = dgvInv.Rows[i].Cells["CostCenter"].Value == null ? "0" : dgvInv.Rows[i].Cells["CostCenter"].Value.ToString() ?? "0";

                string humId = dgvInv.Rows[i].Cells["humId"].Value == null ? "0" : dgvInv.Rows[i].Cells["humId"].Value?.ToString() ?? "0";
                string humName = dgvInv.Rows[i].Cells["type"].Value == null ? "" : dgvInv.Rows[i].Cells["type"].Value?.ToString() ?? "";
                var description = dgvInv.Rows[i].Cells["Description"].Value?.ToString() ?? "";
                var amount = dgvInv.Rows[i].Cells["Amount"].Value?.ToString() ?? "0";
                var colNote = dgvInv.Rows[i].Cells["colNote"].Value?.ToString() ?? "";

                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_petty_cash_details(entry_date,petty_cash_id,hum_id, hum_name, ref_id,cost_center_id,amount, description,category, note) 
                                        VALUES (@date,@Petty_Cash_id,@hum_id , @hum_name, @inv_code, @cost_center_id,@amount, @description,@category, @note);",
                                        DBClass.CreateParameter("@Petty_Cash_id", pvId),
                                        DBClass.CreateParameter("@date", date),
                                        DBClass.CreateParameter("@hum_id", humId),
                                        DBClass.CreateParameter("@hum_name", humName),
                                        DBClass.CreateParameter("@amount", amount),
                                        DBClass.CreateParameter("@inv_code", invId),
                                        DBClass.CreateParameter("@cost_center_id", CostCenterId),
                                        DBClass.CreateParameter("@description", description),
                                        DBClass.CreateParameter("@category", categoryId),
                                        DBClass.CreateParameter("@note", colNote)
                                        );

            }
        }

        private void guna2TileButton25_Click(object sender, EventArgs e)
        {

        }

        private void guna2TileButton26_Click(object sender, EventArgs e)
        {

        }

        private void SetDefault()
        {
            //using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT id,code FROM tbl_coa_level_4 WHERE id = (select account_id from tbl_coa_config where category=@cat)", DBClass.CreateParameter("@cat", "Default Account For Cash")))
            //    if (reader.Read())
            //    {
            //        txtAccountCode.Text = reader["code"].ToString();
            //        string accountId = reader["id"].ToString();
            //        if (!string.IsNullOrEmpty(accountId))
            //        {
            //            cmbAccountName.SelectedValue = int.Parse(accountId);
            //        }
            //    }
        }

        private void dgvInv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvInv.Rows.Count <= 0)
                return;
            if (dgvInv.CurrentRow == null)
                return;

            calculateTotal();
        }
        private void calculateTotal()
        {
            for (int i = 0; i < dgvInv.Rows.Count; i++)
            {
                if (dgvInv.Rows[i].Cells["Amount"].Value != null)
                {
                    if (dgvInv.Rows[i].Cells["Amount"].Value.ToString() == "")
                        continue;
                    totalAmount += decimal.Parse(dgvInv.Rows[i].Cells["Amount"].Value.ToString());
                }
            }
            txtAmount.Text = totalAmount.ToString();
            totalAmount = 0;
        }
        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtAmount.Text == "")
            {
                txtAmountInWord.Text = "";
                return;
            }
            txtAmountInWord.Text = GeneralConfiguration.NumberToWords(decimal.Parse(txtAmount.Text));
        }

        private void dgvInv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //
        }

        public DataTable COMPANYINFO(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT * FROM tbl_company ;", DBClass.CreateParameter("@id", a1));
        }
        public DataTable PettyCashVoucherData(string a1)
        {
            return DBClass.ExecuteDataTable(@"
                                            SELECT pc.id,pc.code,pc.voucher_date,pc.cash_account_id,pc.employee_id,e.name as employee_name,pc.total,pc.notes,pc.created_by,pc.project_id,IFNULL((select tbl_projects.code FROM tbl_projects WHERE tbl_projects.id = pc.project_id),'') as project_name FROM tbl_petty_cash pc LEFT JOIN tbl_employee e ON e.id = pc.employee_id
                                            where pc.id=@PettyCashId;", DBClass.CreateParameter("@PettyCashId", a1));
        }

        public DataTable PettyCashDetail(string a1)
        {
            return DBClass.ExecuteDataTable(@"SELECT id,petty_cash_id,'' petty_cash_name,entry_date,ref_id,IFNULL((select tbl_cost_center.code FROM tbl_cost_center WHERE tbl_cost_center.id=cost_center_id),'') as cost_center,description,CAST(amount AS DECIMAL(18,2)) AS amount,note,category FROM tbl_petty_cash_details
                                             WHERE petty_cash_id = @PettyCashId;", DBClass.CreateParameter("@PettyCashId", a1));
        }

        public void ShowReport()
        {

            try
            {
                // Create the report 
                //CPV_GENERAL cr = new CPV_GENERAL();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "PettyCashVoucherGeneral.rpt");

                // Load the report file from disk
                ReportDocument cr = new ReportDocument();
                cr.Load(reportPath);
                // Load the main report data
                DataTable companyData = COMPANYINFO("1");  // Assuming you want to pass ID 1
                DataTable PettyCashDetails = PettyCashDetail(id.ToString());
                DataTable PettyCashVoucher = PettyCashVoucherData(id.ToString());
                if (companyData != null)  // Ensure that data was successfully retrieved
                {
                    //cr.SetDataSource(companyData);
                    cr.Subreports["Company"].SetDataSource(companyData);
                    cr.Subreports["PettyCashHeader"].SetDataSource(PettyCashVoucher);
                    cr.Subreports["PettyCashDetails"].SetDataSource(PettyCashDetails);
                    ((TextObject)cr.ReportDefinition.Sections["Section4"].ReportObjects["Text6"]).Text = frmLogin.userName;
                }
                else
                {
                    MessageBox.Show("No data available for the report.");
                    return;  // Exit the method if no data is available
                }

                // Assign the main report to the viewer
                MasterReportView reportForm = new MasterReportView();
                reportForm.crReportViewer.ReportSource = cr;
                reportForm.crReportViewer.RefreshReport();
                reportForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void guna2TileButton18_Click(object sender, EventArgs e)
        {
            double amount = 0;

            bool hasValidAmount = double.TryParse(txtAmount.Text, out amount) && amount > 0;

            if (id > 0 && hasValidAmount)
            {
                loadPrint();
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertPV())
                {
                    EventHub.RefreshPettyCashVoucher();
                    MessageBox.Show("The PettyCash Voucher Paid  ");
                    if (chkPrint.Checked == true)
                    {
                        loadPrint();
                    }
                    dgvInv.Rows.Clear();
                    loadPettyCashDataDetails();
                    txtPVCode.Text = GenerateNextPettyCashCode();
                    this.Close();
                }
            }
            else
            {
                if (updatePV())
                {
                    EventHub.RefreshPettyCashVoucher();
                    MessageBox.Show("The PettyCash Voucher Updated !  ");
                    if (chkPrint.Checked == true)
                    {
                        loadPrint();
                    }
                    dgvInv.Rows.Clear();
                    loadPettyCashDataDetails();
                    txtPVCode.Text = GenerateNextPettyCashCode();
                    this.Close();
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal; // Restore down
            }
            else
            {
                this.WindowState = FormWindowState.Maximized; // Maximize
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int? currentId = int.Parse(txtId.Text.ToString() ?? "0");//Utilities.GetVoucherIdFromCode(txtPVCode.Text);
            if (currentId == null || currentId <= 1)
                return;

            currentId = currentId - 1;
            txtId.Text = currentId.ToString();
            if (currentId <= 0)
            {
                clear();
                MessageBox.Show("No previous records.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string query = "select id from tbl_petty_cash where id =@id";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindVoucher();
                }
                else
                {
                    clear();
                    MessageBox.Show("No previous record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int? currentId = int.Parse(txtId.Text.ToString()??"0");// Utilities.GetVoucherIdFromCode(txtPVCode.Text);
            if (currentId is null) return;

            currentId = currentId + 1;
            txtId.Text = currentId.ToString();
            string query = "SELECT id FROM tbl_petty_cash WHERE id =@id";
            using (var reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("@id", currentId)))
            {
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    BindVoucher();
                }
                else
                {
                    clear();
                    MessageBox.Show("No next record found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtPVCode.Text = GenerateNextPettyCashCode();
            clear();
        }
        private void clear()
        {
            dgvInv.Rows.Clear();
            totalAmount = 0;
            id = 0;
        }

        private void BtnSaveNew_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
            dgvInv.Rows.Clear();
            txtPVCode.Text = GenerateNextPettyCashCode();
            loadPettyCashDataDetails();
            clear();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            //DBClass.ExecuteNonQuery("UPDATE tbl_PettyCash_voucher SET state = -1 WHERE id = @id ",
            //                              DBClass.CreateParameter("id", id.ToString()));

            // 1. Read all records before deletion
            DataTable dtVoucher = DBClass.ExecuteDataTable("SELECT * FROM tbl_petty_cash WHERE id = @id",
                DBClass.CreateParameter("id", id.ToString()));

            DataTable dtDetails = DBClass.ExecuteDataTable("SELECT * FROM tbl_petty_cash_details WHERE Petty_Cash_id = @id",
                DBClass.CreateParameter("id", id.ToString()));

            DataTable dtTransaction = DBClass.ExecuteDataTable("SELECT * FROM tbl_transaction WHERE transaction_id = @id AND t_type='PettyCash'",
                DBClass.CreateParameter("id", id.ToString()));

            // 2. Insert backups into tbl_deleted_records
            foreach (DataRow row in dtVoucher.Rows)
            {
                DBClass.ExecuteNonQuery(
                    "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                    DBClass.CreateParameter("table", "tbl_PettyCash_voucher"),
                    DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                    DBClass.CreateParameter("user", frmLogin.userId.ToString())
                );
            }

            foreach (DataRow row in dtDetails.Rows)
            {
                DBClass.ExecuteNonQuery(
                    "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                    DBClass.CreateParameter("table", "tbl_PettyCash_voucher_details"),
                    DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                    DBClass.CreateParameter("user", frmLogin.userId.ToString())
                );
            }

            foreach (DataRow row in dtTransaction.Rows)
            {
                DBClass.ExecuteNonQuery(
                    "INSERT INTO tbl_deleted_records (table_name, record_data, deleted_by) VALUES (@table, @data, @user)",
                    DBClass.CreateParameter("table", "tbl_transaction"),
                    DBClass.CreateParameter("data", Newtonsoft.Json.JsonConvert.SerializeObject(row)),
                    DBClass.CreateParameter("user", frmLogin.userId.ToString())
                );
            }

            // 3. Now delete records permanently
            CommonInsert.DeleteTransactionEntry(id, "PettyCash");
            CommonInsert.DeleteCostCenterTransactionEntry(id.ToString(), "PettyCash");
            DBClass.ExecuteNonQuery("DELETE FROM tbl_petty_cash_details WHERE Petty_Cash_id = @id",
                DBClass.CreateParameter("id", id.ToString()));
            DBClass.ExecuteNonQuery("DELETE FROM tbl_petty_cash WHERE id = @id",
                DBClass.CreateParameter("id", id.ToString()));

            Utilities.LogAudit(frmLogin.userId, "PettyCash Voucher Permanently Deleted", "PettyCash Voucher", id, "Deleted PettyCash Voucher with ID: " + id);
            clear();
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            id = 0;
        }

        //private void txtAccountCode_TextChanged(object sender, EventArgs e)
        //{
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
        //      DBClass.CreateParameter("code", txtAccountCode.Text)))
        //    {
        //        if (reader.Read())
        //            cmbAccountName.SelectedValue = int.Parse(reader["id"].ToString());
        //    }

        //    if (txtAccountCode.Focused)
        //    {
        //        string input = txtAccountCode.Text.Trim();

        //        if (string.IsNullOrEmpty(input))
        //        {
        //            lstAccountSuggestions.Visible = false;
        //            return;
        //        }
        //        string query = @"SELECT code, name FROM tbl_coa_level_4 
        //                 WHERE code LIKE @search OR name LIKE @search LIMIT 20";

        //        DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

        //        lstAccountSuggestions.Items.Clear();

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
        //        }
        //        if (lstAccountSuggestions.Items.Count > 0)
        //        {
        //            Point locationOnForm = txtAccountCode.Parent.PointToScreen(txtAccountCode.Location);
        //            Point locationRelativeToForm = this.PointToClient(locationOnForm);

        //            lstAccountSuggestions.SetBounds(
        //                locationRelativeToForm.X,
        //                locationRelativeToForm.Y + txtAccountCode.Height,
        //                txtAccountCode.Width + 100,
        //                120
        //            );

        //            lstAccountSuggestions.Tag = txtAccountCode;
        //            lstAccountSuggestions.Visible = true;
        //            lstAccountSuggestions.BringToFront();
        //        }
        //        else
        //        {
        //            lstAccountSuggestions.Visible = false;
        //        }
        //    }
        //}

        //private void txtAccountCode_Leave(object sender, EventArgs e)
        //{
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code", DBClass.CreateParameter("code", txtAccountCode.Text)))
        //        if (!reader.Read())
        //            cmbAccountName.SelectedIndex = -1;

        //    BeginInvoke((Action)(() =>
        //    {
        //        if (!lstAccountSuggestions.Focused)
        //            lstAccountSuggestions.Visible = false;
        //    }));
        //}

        //bool isAccountNameRefreshing = false;
        //private void cmbAccountName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cmbAccountName.SelectedValue == null)
        //    {
        //        txtAccountCode.Text = "";
        //        return;
        //    }

        //    if (cmbAccountName.SelectedItem != null)
        //    {
        //        if (cmbAccountName.SelectedItem is DataRowView row)
        //        {
        //            string code = row["code"].ToString();
        //            txtAccountCode.Text = code == "<< Add >>" ? "" : code;
        //        }
        //    }
        //    else
        //    {
        //        txtAccountCode.Text = "";
        //    }


        //    if (cmbAccountName.Focused)
        //    {
        //        if (isAccountNameRefreshing) return;

        //        if (cmbAccountName.SelectedValue != null &&
        //        (cmbAccountName.SelectedValue.ToString() == "0" || cmbAccountName.Text == "<< Add >>"))
        //        {
        //            new frmAddAccount().ShowDialog();
        //            isAccountNameRefreshing = true;
        //            BindCombos.PopulateAllLevel4Account(cmbAccountName, true, true);
        //            cmbAccountName.SelectedIndex = cmbAccountName.Items.Count - 1;
        //            isAccountNameRefreshing = false;
        //        }
        //    }
        //}

        //private void txtEmployeeCode_TextChanged(object sender, EventArgs e)
        //{
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select id,CODE from tbl_vendor where code =@code",
        //          DBClass.CreateParameter("code", txtEmployeeCode.Text)))
        //        if (reader.Read())
        //            cmbEmployee.SelectedValue = int.Parse(reader["id"].ToString());

        //    if (txtEmployeeCode.Focused)
        //    {
        //        string input = txtEmployeeCode.Text.Trim();

        //        if (string.IsNullOrEmpty(input))
        //        {
        //            lstAccountSuggestions.Visible = false;
        //            return;
        //        }
        //        string query = @"SELECT code, name FROM tbl_vendor 
        //                 WHERE code LIKE @search OR name LIKE @search LIMIT 20";

        //        DataTable dt = DBClass.ExecuteDataTable(query, DBClass.CreateParameter("@search", "%" + input + "%"));

        //        lstAccountSuggestions.Items.Clear();

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            lstAccountSuggestions.Items.Add($"{row["code"]} - {row["name"]}");
        //        }
        //        if (lstAccountSuggestions.Items.Count > 0)
        //        {
        //            Point locationOnForm = txtEmployeeCode.Parent.PointToScreen(txtEmployeeCode.Location);
        //            Point locationRelativeToForm = this.PointToClient(locationOnForm);

        //            lstAccountSuggestions.SetBounds(
        //                locationRelativeToForm.X,
        //                locationRelativeToForm.Y + txtEmployeeCode.Height,
        //                txtEmployeeCode.Width + 100,
        //                120
        //            );

        //            lstAccountSuggestions.Tag = txtEmployeeCode;
        //            lstAccountSuggestions.Visible = true;
        //            lstAccountSuggestions.BringToFront();
        //        }
        //        else
        //        {
        //            lstAccountSuggestions.Visible = false;
        //        }
        //    }
        //}

        //private void txtEmployeeCode_Leave(object sender, EventArgs e)
        //{
        //    using (MySqlDataReader reader = DBClass.ExecuteReader("select id,CODE from tbl_vendor where code =@code",
        //        DBClass.CreateParameter("code", txtEmployeeCode.Text)))
        //        if (!reader.Read())
        //            cmbEmployee.SelectedIndex = -1;

        //    BeginInvoke((Action)(() =>
        //    {
        //        if (!lstAccountSuggestions.Focused)
        //            lstAccountSuggestions.Visible = false;
        //    }));
        //}

        //bool isVendorRefreshing = false;

        //private void cmbEmployee_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (id > 0)
        //        return;

        //    if (cmbEmployee.SelectedValue == null)
        //    {
        //        txtEmployeeCode.Text = "";
        //        //cmbAccountName.SelectedIndex = -1;
        //        return;
        //    }

        //    if (cmbEmployee.Focused)
        //    {
        //        if (isVendorRefreshing) return;

        //        if (cmbEmployee.SelectedValue != null &&
        //        (cmbEmployee.SelectedValue.ToString() == "0" || cmbEmployee.Text == "<< Add >>"))
        //        {
        //            var addVendorForm = new frmViewVendor();
        //            addVendorForm.ShowDialog();

        //            isVendorRefreshing = true;
        //            BindCombos.PopulateEmployees(cmbEmployee, true, true);
        //            cmbEmployee.SelectedIndex = cmbEmployee.Items.Count - 1;
        //            isVendorRefreshing = false;
        //        }
        //    }

        //    if (cmbEmployee.SelectedItem != null)
        //    {
        //        if (cmbEmployee.SelectedItem is DataRowView row)
        //        {
        //            string code = row["code"].ToString();
        //            txtEmployeeCode.Text = code == "<< Add >>" ? "" : code;
        //        }
        //    }
        //    else
        //    {
        //        txtEmployeeCode.Text = "";
        //    }

        //    dgvInv.Rows.Clear();
        //    if (cmbEmployee.SelectedValue == null)
        //        return;
        //    int counter = 0; decimal vendorTotalBalance = 0, vendorTotalOB = 0;
        //    string vendorOBDate = DateTime.Now.ToShortDateString(); // Default date
        //    using (MySqlDataReader obReader = DBClass.ExecuteReader(@"SELECT balance, date FROM tbl_vendor WHERE id = @id",
        //        DBClass.CreateParameter("@id", cmbEmployee.SelectedValue)))

        //        if (obReader.Read())
        //        {
        //            if (!string.IsNullOrEmpty(obReader["balance"].ToString()))
        //                vendorTotalOB = decimal.Parse(obReader["balance"].ToString());

        //            if (!string.IsNullOrEmpty(obReader["colDate"].ToString()))
        //                vendorOBDate = DateTime.Parse(obReader["colDate"].ToString()).ToShortDateString();
        //        }
        //    // 2️⃣ Get how much has been paid from opening balance
        //    using (MySqlDataReader paidReader = DBClass.ExecuteReader(@"SELECT 
        //                    (SELECT SUM(payment) 
        //                     FROM tbl_payment_voucher_details 
        //                     WHERE inv_code = 'Vendor Opening Balance' AND hum_id = @id) AS amount",
        //        DBClass.CreateParameter("@id", cmbEmployee.SelectedValue)))

        //        if (paidReader.Read() && paidReader["amount"].ToString() != "")
        //            vendorTotalBalance = decimal.Parse(paidReader["amount"].ToString());

        //    // 3️⃣ Add opening balance row if there's an unpaid amount
        //    if (vendorTotalOB != vendorTotalBalance)
        //    {
        //        decimal pendingOB = vendorTotalOB - vendorTotalBalance;
        //        if (pendingOB != 0)
        //        {
        //            dgvInv.Rows.Add(++counter, cmbEmployee.SelectedValue, "", vendorOBDate, "Vendor Opening Balance", pendingOB, 0, 0);
        //        }
        //    }
        //    using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT ROW_NUMBER() OVER (
        //        ORDER BY tbl_purchase.date) AS SN, 
        //         tbl_purchase.date AS DATE, tbl_purchase.id, tbl_purchase.invoice_id AS 'INV NO', 
        //         tbl_purchase.change
        //        FROM tbl_purchase
        //        INNER JOIN tbl_vendor ON tbl_purchase.vendor_id = tbl_vendor.id
        //        WHERE tbl_purchase.state = 0 AND tbl_purchase.change <> 0 AND tbl_vendor.id = @id
        //        GROUP BY tbl_purchase.id, tbl_purchase.date;",
        //    DBClass.CreateParameter("id", cmbEmployee.SelectedValue)))
        //        while (reader.Read())
        //            dgvInv.Rows.Add((int.Parse(reader["SN"].ToString()) + counter).ToString(), cmbEmployee.SelectedValue, reader["id"].ToString(),
        //            DateTime.Parse(reader["colDate"].ToString()).ToShortDateString(), reader["INV NO"].ToString(), reader["change"].ToString(), 0, 0, "");

        //}

        private void lstAccountSuggestions_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    lstAccountSuggestions_Click(sender, e);
            //    e.Handled = true;
            //}

            //int index = lstAccountSuggestions.IndexFromPoint(e.Location);
            //if (index != ListBox.NoMatches)
            //{
            //    lstAccountSuggestions.SelectedIndex = index;
            //    string selectedCode = lstAccountSuggestions.SelectedItem.ToString().Split('-')[0].Trim();

            //    if (lstAccountSuggestions.Tag is Guna2TextBox tb)
            //    {
            //        _isSelectingFromList = true;
            //        _suppressTextChanged = true;
            //        tb.Text = selectedCode;
            //        _suppressTextChanged = false;
            //        _isSelectingFromList = false;
            //    }

            //    lstAccountSuggestions.Visible = false;
            //}
        }

        private void lstAccountSuggestions_Click(object sender, EventArgs e)
        {
            if (lstAccountSuggestions.SelectedItem != null && lstAccountSuggestions.Tag is Guna2TextBox targetTextBox)
            {
                string selected = lstAccountSuggestions.SelectedItem.ToString();
                string selectedCode = selected.Split('-')[0].Trim();

                targetTextBox.Text = selectedCode;
                lstAccountSuggestions.Visible = false;

                // Trigger TextChanged manually if needed
                targetTextBox.Focus();
                targetTextBox.SelectionStart = targetTextBox.Text.Length;

                // Example of post-selection logic:
                //if (targetTextBox == txtAccountCode)
                //{
                //    using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                //        DBClass.CreateParameter("code", selectedCode)))
                //        if (reader.Read())
                //            cmbAccountName.SelectedValue = int.Parse(reader["id"].ToString());
                //}
                //else if (targetTextBox == txtEmployeeCode)
                //{
                //    using (MySqlDataReader reader = DBClass.ExecuteReader("select id from tbl_employee where code =@code",
                //        DBClass.CreateParameter("code", selectedCode)))
                //        if (reader.Read())
                //            cmbEmployee.SelectedValue = int.Parse(reader["id"].ToString());
                //}
            }

            //if (lstAccountSuggestions.SelectedItem != null)
            //{
            //    txtDebitCode.Text = lstAccountSuggestions.SelectedItem.ToString();
            //    lstAccountSuggestions.Visible = false;

            //    MessageBox.Show("You selected: " + txtDebitCode.Text);
            //}
        }

        private void BtnAttach_Click(object sender, EventArgs e)
        {
            try
            {
                //frmImportOpeningBalance importForm = new frmImportOpeningBalance();
                //if (importForm.ShowDialog() == DialogResult.OK)
                //{
                //    DataTable importedData = importForm.ImportedData;
                //    dgvInv.DataSource = importedData;
                //    LocalizationManager.LocalizeDataGridViewHeaders(dgvInv);
                //}
                using (frmImportPettyCash frm = new frmImportPettyCash())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        int slNo = 0;
                        dgvInv.Rows.Clear();
                        foreach (DataRow row in frm.ImportedData.Rows)
                        {
                            //no, humId, colDate, invId, Category, CostCenter, Description, Amount, colNote
                            string cost_center_id = row["cost_center"].ToString();
                            string category = row["category"].ToString();
                            int idx = dgvInv.Rows.Add();
                            dgvInv.Rows[idx].Cells["no"].Value = ++slNo;
                            dgvInv.Rows[idx].Cells["humId"].Value = null;
                            dgvInv.Rows[idx].Cells["type"].Value = "";
                            dgvInv.Rows[idx].Cells["colDate"].Value = row["date"];
                            dgvInv.Rows[idx].Cells["invId"].Value = row["REF"];
                            dgvInv.Rows[idx].Cells["Category"].Value = null;
                            dgvInv.Rows[idx].Cells["CostCenter"].Value = null;
                            dgvInv.Rows[idx].Cells["Description"].Value = row["description"];
                            dgvInv.Rows[idx].Cells["Amount"].Value = row["amount"];
                            dgvInv.Rows[idx].Cells["colNote"].Value = row["note"];
                            //DataGridViewComboBoxCell comboCellCategory = dgvInv.Rows[dgvInv.Rows.Count - 2].Cells["Category"] as DataGridViewComboBoxCell;
                            //if (comboCellCategory != null && !string.IsNullOrEmpty(category) && category != "")
                            //{
                            //    bool exists = false;
                            //    foreach (var item in comboCellCategory.Items)
                            //    {
                            //        string displayText;
                            //        string displayValue;

                            //        if (item is DataRowView row1)
                            //        {
                            //            displayText = row1[((DataGridViewComboBoxColumn)comboCellCategory.OwningColumn).DisplayMember].ToString();
                            //            displayValue = row1[((DataGridViewComboBoxColumn)comboCellCategory.OwningColumn).ValueMember].ToString();
                            //        }
                            //        else
                            //        {
                            //            displayText = item.ToString();
                            //            displayValue = item.ToString();
                            //        }

                            //        if (displayText.ToLower().Contains(category.ToLower()))
                            //        {
                            //            exists = true;
                            //            comboCellCategory.Value = displayValue;
                            //            break;
                            //        }
                            //    }

                            //    if (!exists)
                            //        comboCellCategory.Value = null;
                            //}

                            DataGridViewComboBoxCell comboCellCostCenter = dgvInv.Rows[dgvInv.Rows.Count - 2].Cells["CostCenter"] as DataGridViewComboBoxCell;
                            if (comboCellCostCenter != null && !string.IsNullOrEmpty(cost_center_id) && cost_center_id != "0")
                            {
                                if (comboCellCostCenter.Items.Contains(int.Parse(cost_center_id)))
                                {
                                    comboCellCostCenter.Value = int.Parse(cost_center_id);
                                }
                                else
                                {
                                    comboCellCostCenter.Value = null; // or leave blank
                                }
                            }
                        }
                    }
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int employeeId = 0;
        private void cmbPettyCash_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPettyCash.SelectedValue == null)
                return;

            employeeId = int.Parse(cmbPettyCash.SelectedValue.ToString());

            var retResult = DBClass.ExecuteScalar(
                        @"SELECT pcc.code
                        FROM tbl_petty_cash_card pcc 
                        JOIN tbl_employee emp ON CAST(pcc.name AS UNSIGNED) = emp.id WHERE emp.id = @id",
                        DBClass.CreateParameter("id", cmbPettyCash.SelectedValue.ToString())
                    );
            string ret = retResult != null ? retResult.ToString() : "0";

            txtCode.Text = ret.ToString();
        }

        private void BtnJournal_Click(object sender, EventArgs e)
        {
            if (id == 0) return;
            frmLogin.frmMain.openChildForm(new frmViewPettyCashVoucherJournal(id));
        }

        private void dgvInv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvInv.CurrentCell.ColumnIndex == dgvInv.Columns["Amount"].Index)
            {
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    // Remove existing event handlers (to avoid duplicate binding)
                    txt.KeyPress -= Txt_KeyPress;

                    // Add our event handler
                    txt.KeyPress += Txt_KeyPress;
                }
            }
            else
            {
                // Optional: Remove the event handler from other columns to avoid unwanted behavior
                TextBox txt = e.Control as TextBox;
                if (txt != null)
                {
                    txt.KeyPress -= Txt_KeyPress;
                }
            }
        }

        private void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow control keys (backspace, delete, etc.)
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true; // Block the input
            }

            // Allow only one decimal point
            TextBox txt = sender as TextBox;
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void dgvInv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Only trigger when humId column is clicked
            if (dgvInv.Columns[e.ColumnIndex].Name == "type")
            {
                loadNameList(dgvInv.Rows[e.RowIndex].Cells["type"], e.RowIndex);
            }
        }

        private void loadNameList(DataGridViewCell cell, int rowIndex)
        {
            string categoryName = dgvInv.Rows[rowIndex].Cells["Category"].FormattedValue?.ToString() ?? "";
            string categoryId = dgvInv.Rows[rowIndex].Cells["Category"].Value?.ToString() ?? "";
            string query = "";

            if (!string.IsNullOrEmpty(categoryName) && !categoryName.ToLower().Contains("general"))
            {
                if (categoryName.ToLower().Contains("purchase") ||
                    categoryName.ToLower().Contains("subcontractor") ||
                    categoryName.ToLower().Contains("vendor") ||
                    categoryName.ToLower().Contains("payment"))
                {
                    query = @"SELECT id, CONCAT(CODE ,' - ' , NAME) as NAME FROM tbl_vendor";
                }
                else if (categoryName.ToLower().Contains("sale") ||
                         categoryName.ToLower().Contains("receipt") ||
                         categoryName.ToLower().Contains("customer"))
                {
                    query = @"SELECT id, CONCAT(CODE ,' - ' , NAME) as NAME FROM tbl_customer";
                }
                else if (categoryName.ToLower().Contains("salary") ||
                         categoryName.ToLower().Contains("employee"))
                {
                    query = @"SELECT id, CONCAT(CODE ,' - ' , NAME) as NAME FROM tbl_employee";
                }
                else if (categoryName.ToLower().Contains("petty cash"))
                {
                    query = @"SELECT e.id, CONCAT(e.code,' - ' , e.name) as NAME FROM tbl_employee e WHERE id  IN (SELECT CAST(name AS UNSIGNED) FROM tbl_petty_cash_card)";
                }
            }

            if (!string.IsNullOrEmpty(query))
            {
                // Position ListView under clicked cell
                Rectangle cellRect = dgvInv.GetCellDisplayRectangle(cell.ColumnIndex, rowIndex, true);
                Point cellLocation = dgvInv.PointToScreen(cellRect.Location);
                Point listLocation = this.PointToClient(cellLocation);

                nameslistView.SetBounds(listLocation.X, listLocation.Y + cell.Size.Height, 250, 120);
                nameslistView.Visible = true;
                nameslistView.BringToFront();

                loadDataToListView(query);
            }
        }

        private void loadDataToListView(string query)
        {
            try
            {
                DataTable dt = DBClass.ExecuteDataTable(query);

                nameslistView.Clear();
                nameslistView.View = View.Details;
                nameslistView.FullRowSelect = true;
                nameslistView.Columns.Add("Name", 250);

                foreach (DataRow row in dt.Rows)
                {
                    ListViewItem item = new ListViewItem(row["NAME"].ToString());
                    item.SubItems.Add(row["id"].ToString());
                    nameslistView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void nameslistView_Click(object sender, EventArgs e)
        {
            if (nameslistView.SelectedItems.Count > 0 && dgvInv.CurrentCell != null)
            {
                string accountName = nameslistView.SelectedItems[0].Text;
                string accountId = nameslistView.SelectedItems[0].SubItems[1].Text; // hidden id

                // Put display name into humId column
                dgvInv.CurrentRow.Cells["type"].Value = accountName;

                // Put id into hId column
                dgvInv.CurrentRow.Cells["humId"].Value = accountId;
                // Assign selected value directly
                //dgvInv.CurrentCell.Value = accountName;

                nameslistView.Visible = false;
            }
        }

        private void nameslistView_Leave(object sender, EventArgs e)
        {
            nameslistView.Visible = false;
            nameslistView.SendToBack();
        }
    }

    public class frmImportPettyCash : Form
    {
        private TextBox txtStartCell, txtEndCell, txtDateCell, txtAccountCodeCell, txtAccountNameCell,
                        txtCostCenterCell, txtDescriptionCell, txtAmountCell, txtNoteCell;
        private Button btnLoadExcel, btnImport;
        private DataGridView dataGridView;
        private string excelPath = string.Empty;

        private DataTable dtFiltered = new DataTable();
        public DataTable ImportedData { get; private set; }

        public frmImportPettyCash()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            this.Text = "Import Petty Cash from Excel";
            this.Size = new Size(1400, 800);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ==== Top Panel ====
            Panel panelTop = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(10),
            };

            int x = 10, spacing = 8, labelWidth = 90, textboxWidth = 30, controlHeight = 28;

            // Buttons
            btnLoadExcel = new Button() { Text = "Load Excel", Size = new Size(100, controlHeight), Location = new Point(x, 10) };
            x += 100 + spacing;
            btnImport = new Button() { Text = "Set Data", Size = new Size(100, controlHeight), Location = new Point(x, 10) };
            x += 100 + spacing;

            // Add mapping fields
            panelTop.Controls.Add(new Label() { Text = "Start Row:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtStartCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "End Row:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtEndCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "Date Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtDateCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "REF Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtAccountCodeCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "Category Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtAccountNameCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "CostCenter Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtCostCenterCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "Description Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtDescriptionCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "Amount Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtAmountCell = new TextBox() { Width = 50, Location = new Point(x, 12) };
            x += textboxWidth + 50;

            panelTop.Controls.Add(new Label() { Text = "Note Col:", AutoSize = true, Location = new Point(x, 15) });
            x += labelWidth;
            txtNoteCell = new TextBox() { Width = 50, Location = new Point(x, 12) };

            // Add controls
            panelTop.Controls.Add(btnLoadExcel);
            panelTop.Controls.Add(btnImport);
            panelTop.Controls.Add(txtStartCell);
            panelTop.Controls.Add(txtEndCell);
            panelTop.Controls.Add(txtDateCell);
            panelTop.Controls.Add(txtAccountCodeCell);
            panelTop.Controls.Add(txtAccountNameCell);
            panelTop.Controls.Add(txtCostCenterCell);
            panelTop.Controls.Add(txtDescriptionCell);
            panelTop.Controls.Add(txtAmountCell);
            panelTop.Controls.Add(txtNoteCell);

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

            this.Controls.Add(panelMain);
            this.Controls.Add(panelTop);
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

        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(excelPath))
            {
                MessageBox.Show("Please load an Excel file first.");
                return;
            }

            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                dtFiltered = new DataTable();
                dtFiltered.Columns.Add("date");
                dtFiltered.Columns.Add("REF");
                dtFiltered.Columns.Add("category");
                dtFiltered.Columns.Add("cost_center");
                dtFiltered.Columns.Add("description");
                dtFiltered.Columns.Add("amount");
                dtFiltered.Columns.Add("note");

                var ws = package.Workbook.Worksheets[0];

                int startRow = ToNumber(txtStartCell.Text);
                int endRow = ToNumber(txtEndCell.Text);

                int colDate = ExcelColumnNameToNumber(txtDateCell.Text);
                int colACCode = ExcelColumnNameToNumber(txtAccountCodeCell.Text);
                int colACName = ExcelColumnNameToNumber(txtAccountNameCell.Text);
                int colCostCenter = ExcelColumnNameToNumber(txtCostCenterCell.Text);
                int colDesc = ExcelColumnNameToNumber(txtDescriptionCell.Text);
                int colAmount = ExcelColumnNameToNumber(txtAmountCell.Text);
                int colNote = ExcelColumnNameToNumber(txtNoteCell.Text);

                for (int row = startRow; row <= endRow; row++)
                {
                    if (IsRowEmpty(ws, row, ws.Dimension.End.Column)) continue;

                    DataRow dr = dtFiltered.NewRow();
                    dr["date"] = (colDate > 0) ? ws.Cells[row, colDate].Text : "";
                    dr["REF"] = (colACCode > 0) ? ws.Cells[row, colACCode].Text : "";
                    dr["category"] = (colACName > 0) ? ws.Cells[row, colACName].Text : "";
                    dr["cost_center"] = (colCostCenter > 0) ? ws.Cells[row, colCostCenter].Text : "";
                    dr["description"] = (colDesc > 0) ? ws.Cells[row, colDesc].Text : "";
                    //dr["amount"] = (colAmount > 0) ? ws.Cells[row, colAmount].Text : "0";
                    if (colAmount > 0)
                    {
                        object rawValue = ws.Cells[row, colAmount].Value;
                        if (rawValue != null && decimal.TryParse(rawValue.ToString(),
                                NumberStyles.Any,
                                CultureInfo.InvariantCulture,
                                out decimal amount))
                        {
                            dr["amount"] = amount.ToString("0.##"); // clean number, no commas
                        }
                        else
                        {
                            dr["amount"] = "0";
                        }
                    }
                    else
                    {
                        dr["amount"] = "0";
                    }
                    dr["note"] = (colNote > 0) ? ws.Cells[row, colNote].Text : "";

                    dtFiltered.Rows.Add(dr);
                }

                dataGridView.DataSource = dtFiltered;
            }

            this.ImportedData = dtFiltered;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ImportToGrid()
        {
            using (var package = new ExcelPackage(new FileInfo(excelPath)))
            {
                var ws = package.Workbook.Worksheets[0];
                int startRow = ws.Dimension.Start.Row;
                int endRow = ws.Dimension.End.Row;
                int startCol = ws.Dimension.Start.Column;
                int endCol = ws.Dimension.End.Column;

                DataTable dt = new DataTable();
                dt.Columns.Add("Index");

                for (int col = startCol; col <= endCol; col++)
                    dt.Columns.Add(GetExcelColumnLetter(col));

                for (int row = startRow; row <= endRow; row++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Index"] = row.ToString();
                    for (int col = startCol; col <= endCol; col++)
                        dr[col - startCol + 1] = ws.Cells[row, col].Text;

                    dt.Rows.Add(dr);
                }

                dataGridView.DataSource = dt;
                dataGridView.ReadOnly = true;
                dataGridView.RowHeadersVisible = false;
            }
        }

        private int ToNumber(string text)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            return int.TryParse(text, out int num) ? num : -1;
        }

        private int ExcelColumnNameToNumber(string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) return -1;

            columnName = columnName.ToUpperInvariant();
            int sum = 0;
            foreach (char c in columnName)
            {
                if (c < 'A' || c > 'Z') return -1;
                sum *= 26;
                sum += (c - 'A' + 1);
            }
            return sum;
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

}