using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewProjectEstimating : Form
    {
        decimal invId;
        int level4Inventory;
        int id;
        bool isExported = false;

        public frmViewProjectEstimating(int _id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            if (id != 0)
                this.Text = "Project - Edit Estimate";
            else
                this.Text = "Project - New Estimate";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewProjectEstimating_Load(object sender, EventArgs e)
        {
            dtpt.Value = DateTime.Now.Date;
            BindCombos.PopulateWarehouse(cmbWarehouse);
            //LoaddgvItems();
            BindCombo();
            if (id != 0)
                BindData();
        }
        private void BindData()
        {
            MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_tender where id = @id",
                DBClass.CreateParameter("id", id));
            reader.Read();
            dtpt.Value = DateTime.Parse(reader["date"].ToString());
            cmbProject.SelectedValue = reader["project_id"].ToString();
            cmbAccountName.SelectedValue = reader["account_id"].ToString();
            cmbTenderName.SelectedValue = reader["tender_name_id"].ToString();
            cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
            dtsd.Value = DateTime.Parse(reader["submission_date"].ToString());
            var fees = reader["fees"];

            if (fees != DBNull.Value && Convert.ToDecimal(fees) > 0)
            {
                txtFees.Text = fees.ToString();
            }
            else
            {
                txtFees.Text = "";
            }
            txtDescription.Text = reader["description"].ToString();
            string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
            int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", id)));
            isExported = count > 0 ? true : false;

            invId = id;
            if (isExported)
            {
                dgvItems.Columns.Clear();
                dgvItems.Columns.Add("itemId", "id");
                dgvItems.Columns.Add("no", "#");
                dgvItems.Columns.Add("sr", "Sr.");
                dgvItems.Columns.Add("name", "Description of work");
                dgvItems.Columns.Add("qty", "Qty");
                dgvItems.Columns.Add("unit", "Unit");
                dgvItems.Columns.Add("Length", "Length");
                dgvItems.Columns.Add("Width", "Width");
                dgvItems.Columns.Add("Thick", "Thick");
                dgvItems.Columns.Add("rate", "Rate");
                dgvItems.Columns.Add("amount", "Amount");
                dgvItems.Columns.Add("marginPercentage", "Margin %");
                dgvItems.Columns.Add("marginAmount", "Margin Amount");
                dgvItems.Columns.Add("total", "Total");
                dgvItems.Columns.Add("type", "Type");
                dgvItems.Columns.Add("Note", "Note");

                DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
                deleteButtonColumn.Name = "delete";
                deleteButtonColumn.HeaderText = "DEL";
                //deleteButtonColumn.Text = "Remove";
                deleteButtonColumn.UseColumnTextForButtonValue = true;
                dgvItems.Columns.Add(deleteButtonColumn);

                // this.sr,this.code,this.name,this.qty,this.unit,this.unitId,this.rate,this.amount,this.marginPercentage,this.marginAmount,this.total,this.type,this.delete

                dgvItems.Columns["no"].Width = 35;
                dgvItems.Columns["sr"].Width = 45;
                dgvItems.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvItems.Columns["qty"].Width = 80;
                dgvItems.Columns["unit"].Width = 50;
                dgvItems.Columns["amount"].Width = 130;
                dgvItems.Columns["marginAmount"].Width = 120;
                dgvItems.Columns["delete"].Width = 60;

                dgvItems.Columns["itemId"].Visible = false;
                dgvItems.Columns["type"].Visible = false;

                dgvItems.Columns["no"].ReadOnly = true;
                dgvItems.Columns["sr"].ReadOnly = true;
                dgvItems.Columns["amount"].ReadOnly = true;
                dgvItems.Columns["total"].ReadOnly = true;
                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Format = "N3";
                dataGridViewCellStyle1.NullValue = null;
                dgvItems.Columns["rate"].DefaultCellStyle = dataGridViewCellStyle1;

                DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
                dataGridViewCellStyle2.Format = "N3";
                dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
                dataGridViewCellStyle2.NullValue = null;
                dgvItems.Columns["amount"].DefaultCellStyle = dataGridViewCellStyle2;

                DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
                dataGridViewCellStyle3.Format = "N2";
                dataGridViewCellStyle3.NullValue = null;
                dgvItems.Columns["marginPercentage"].DefaultCellStyle = dataGridViewCellStyle3;
                dgvItems.Columns["marginAmount"].DefaultCellStyle = dataGridViewCellStyle2;
                dgvItems.Columns["total"].DefaultCellStyle = dataGridViewCellStyle2;

            }
            BindItems();
            CalculateTotal();
        }

        private void BindItems()
        {
            dgvItems.Rows.Clear();
            string query = "";
            if (isExported)
            {
                query = @"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.item_id=tbl_items_boq.id
                                WHERE tbl_project_tender_details.tender_id= @id;";
            }
            else
            {
                query = @"SELECT tbl_project_tender_details.*, tbl_items.code as code,tbl_items.type,(select name from tbl_unit where id=tbl_project_tender_details.unit_id) as unit_name FROM tbl_project_tender_details INNER JOIN 
                                                                    tbl_items ON tbl_project_tender_details.item_id = tbl_items.id WHERE 
                                                                    tbl_project_tender_details.tender_id = @id;";
            }
            MySqlDataReader reader = DBClass.ExecuteReader(query,
                                                            DBClass.CreateParameter("id", id));
            int count = 0;
            while (reader.Read())
            {
                decimal totalAmount = 0, subTotal = 0, marginPercentage = 0, margin = 0;
                subTotal = Convert.ToDecimal(reader["rate"].ToString()) * Convert.ToDecimal(reader["qty"].ToString());
                marginPercentage = Convert.ToDecimal(reader["margin_percentage"].ToString());
                margin = Convert.ToDecimal(reader["margin_amount"].ToString());
                totalAmount = subTotal + margin;
                if (isExported)
                {
                    //dgvItems.Columns.Add("itemId", "id");
                    //dgvItems.Columns.Add("no", "#");
                    //dgvItems.Columns.Add("sr", "Sr.");
                    //dgvItems.Columns.Add("name", "Item Name");
                    //dgvItems.Columns.Add("qty", "Qty");
                    //dgvItems.Columns.Add("unit", "Unit");
                    //dgvItems.Columns.Add("rate", "Rate");
                    //dgvItems.Columns.Add("amount", "Amount");
                    //dgvItems.Columns.Add("marginPercentage", "Margin %");
                    //dgvItems.Columns.Add("marginAmount", "Margin Amount");
                    //dgvItems.Columns.Add("total", "Total");
                    //dgvItems.Columns.Add("type", "Type");
                    //dgvItems.Columns.Add("delete", "DEL");
                    dgvItems.Rows.Add(reader["id"].ToString(), (count++), reader["sr"].ToString(), reader["name"].ToString(),
                        decimal.Parse(reader["qty"].ToString()).ToString("F2"),
                        reader["unit_name"].ToString(),
                        decimal.Parse(reader["length"].ToString()).ToString("F2"),
                        decimal.Parse(reader["width"].ToString()).ToString("F2"),
                        reader["thickness"].ToString(),
                        decimal.Parse(reader["rate"].ToString()).ToString(),
                        decimal.Parse(subTotal.ToString()).ToString("F2"),
                        decimal.Parse(marginPercentage.ToString()).ToString("F2"),
                        decimal.Parse(margin.ToString()).ToString("F2"),
                        decimal.Parse(totalAmount.ToString()).ToString("F2"),
                        reader["type"].ToString(),
                        reader["note"].ToString());
                }
                else
                {
                    dgvItems.Rows.Add(reader["item_id"].ToString(), (count++), reader["sr"].ToString(), reader["code"].ToString(), reader["code"].ToString(),
                        decimal.Parse(reader["qty"].ToString()).ToString("F2"),
                        int.Parse(reader["unit_id"].ToString()), reader["unit_id"].ToString(),
                        //reader["rate"].ToString(), 
                        //subTotal.ToString(), 
                        //marginPercentage.ToString(), 
                        //margin.ToString(),
                        //totalAmount.ToString(),
                        decimal.Parse(reader["rate"].ToString()).ToString("F2"),
                        decimal.Parse(subTotal.ToString()).ToString("F2"),
                        decimal.Parse(marginPercentage.ToString()).ToString("F2"),
                        decimal.Parse(margin.ToString()).ToString("F2"),
                        decimal.Parse(totalAmount.ToString()).ToString("F2"),
                        reader["type"].ToString());
                }
            }
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.IsNewRow) continue;

                var srValue = row.Cells["sr"].Value?.ToString().Trim();

                if (!string.IsNullOrEmpty(srValue) && Regex.IsMatch(srValue, @"^[A-Za-z]+$"))
                {
                    row.DefaultCellStyle.Font = new Font(dgvItems.Font, FontStyle.Bold);
                }
            }
        }
        private void LoaddgvItems()
        {
            dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items where warehouse_id=@id and state = 0 and  active = 0",
                                DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
            DataGridViewComboBoxColumn cmbItemName = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            cmbItemName.DataSource = dt;
            cmbItemName.DisplayMember = "name";
            cmbItemName.ValueMember = "code";
            //change it based on selected item SELECT u.id,u.name from tbl_items_unit iu,tbl_unit u WHERE iu.unit_id=u.id AND iu.item_id = 50;
            dt = DBClass.ExecuteDataTable("select id, name from tbl_unit");
            DataGridViewComboBoxColumn cmbUnit = (DataGridViewComboBoxColumn)dgvItems.Columns["unit"];
            cmbUnit.DataSource = dt;
            cmbUnit.DisplayMember = "name";
            cmbUnit.ValueMember = "id";
        }
        int cmbIncomeAccount = 0, cmbAssetAccount = 0, cmbCOGSAccount = 0;
        public void BindCombo()
        {
            BindCombos.PopulateProjects(cmbProject);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            BindCombos.PopulateTenderNames(cmbTenderName);

            cmbIncomeAccount = BindCombos.SelectDefaultLevelAccount("Item Income");
            cmbAssetAccount = BindCombos.SelectDefaultLevelAccount("Inventory");
            cmbCOGSAccount = BindCombos.SelectDefaultLevelAccount("COGS");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (updateData())
            {
                var result = MessageBox.Show("Estimate updated successfully. Do you want to print excel?", "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    saveToExcel();
                }
                else
                {
                    this.Close();
                }
            }
        }
        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;


            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_tender 
                                    SET  modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date, project_id = @projectId, submission_date = @submissionDate, 
                                    description=@description,fees=@fees,amount=@amount,warehouse_id = @warehouse_id,account_id = @account_id,tender_name_id = @tenderNameId,estimate_status=1 WHERE id = @id;",
            DBClass.CreateParameter("id", id),
            DBClass.CreateParameter("date", dtpt.Value.Date),
            DBClass.CreateParameter("submissionDate", dtsd.Value.Date),
            DBClass.CreateParameter("projectId", cmbProject.SelectedValue),
            DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
            DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue),
            DBClass.CreateParameter("tenderNameId", cmbTenderName.SelectedValue),
            DBClass.CreateParameter("fees", txtFees.Text),
            DBClass.CreateParameter("amount", txtTotal.Text),
            DBClass.CreateParameter("description", txtDescription.Text),
            DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
            DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));

            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_project_tender_details WHERE tender_id=@tenderId;
                                      DELETE FROM tbl_items_boq_details WHERE ref_id IN (SELECT id FROM tbl_items_boq WHERE ref_id=@tenderId);
                                      DELETE FROM tbl_item_assembly_bos WHERE assembly_id IN (SELECT id FROM tbl_items_boq WHERE ref_id=@tenderId);
                                      DELETE FROM tbl_items_boq WHERE ref_id=@tenderId",
                                      DBClass.CreateParameter("@tenderId", invId.ToString()));
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction where type = 'Project Tender' and item_id = @tenderId;DELETE FROM tbl_transaction where type = 'Project Tender' AND transaction_id= @tenderId", DBClass.CreateParameter("tenderId", invId));
            DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Project Tender' and trans_no = @tenderId", DBClass.CreateParameter("tenderId", invId));

            insertGridItems();
            Utilities.LogAudit(frmLogin.userId, "Update Project Estimate", "Project Tender", id, "Updated Project Estimate: " + cmbTenderName.Text);
            EventHub.RefreshProjectTendering();

            return true;
        }
        private void insertGridItems()
        {
            int _itemCode = 0;
            object resultCode = DBClass.ExecuteScalar(@"SELECT IFNULL(MAX(CAST(code AS UNSIGNED)), 0) + 1 AS next_code FROM tbl_items;");
            int recordCount = (resultCode != null && resultCode != DBNull.Value) ? Convert.ToInt32(resultCode) : 0;
            if (recordCount > 0)
            {
                _itemCode = recordCount;
            }
            
                string refSr = "";
                int subId = 0;
                int assemblyItemId = 0;
                for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                {
                    string _sr = id > 0 ? (dgvItems.Rows[i].Cells["sr"].Value == null ? "" : dgvItems.Rows[i].Cells["sr"].Value.ToString()) : (dgvItems.Rows[i].Cells["sr"].Value == null ? "" : dgvItems.Rows[i].Cells["sr"].Value.ToString());
                    string _description = id > 0 ? (dgvItems.Rows[i].Cells["name"].Value ==null ? "" : dgvItems.Rows[i].Cells["name"].Value.ToString()) : (dgvItems.Rows[i].Cells["name"].Value == null ? "" : dgvItems.Rows[i].Cells["name"].Value.ToString());
                    string _rate = dgvItems.Rows[i].Cells["rate"].Value == null || dgvItems.Rows[i].Cells["rate"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["rate"].Value.ToString();
                    string _qty = dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["qty"].Value.ToString();
                string _amount = dgvItems.Rows[i].Cells["amount"].Value == null || dgvItems.Rows[i].Cells["amount"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["amount"].Value.ToString();
                string _marginAmount = dgvItems.Rows[i].Cells["marginAmount"].Value == null || dgvItems.Rows[i].Cells["marginAmount"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["marginAmount"].Value.ToString();
                string _marginPercentage = dgvItems.Rows[i].Cells["marginPercentage"].Value == null || dgvItems.Rows[i].Cells["marginPercentage"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["marginPercentage"].Value.ToString();

                if (_sr != "" && _description != "")
                    {
                        string _length = dgvItems.Rows[i].Cells["length"].Value == null || dgvItems.Rows[i].Cells["length"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["length"].Value.ToString();
                        string _width = dgvItems.Rows[i].Cells["width"].Value == null || dgvItems.Rows[i].Cells["width"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["width"].Value.ToString();
                        string _thick = dgvItems.Rows[i].Cells["thick"].Value == null || dgvItems.Rows[i].Cells["thick"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["thick"].Value.ToString();
                        string _note = dgvItems.Rows[i].Cells["note"].Value == null ? " " : dgvItems.Rows[i].Cells["note"].Value.ToString();

                        decimal _itemId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_items_boq(sr,ref_id,type,name,unit_name,qty,price,amount,length,width,thickness,note)
                                            VALUES(@sr,@tenderId,@type, @itemName,@unit, @qty,@rate, @amount,@length,@width,@thickness,@note); SELECT LAST_INSERT_ID();",
                          DBClass.CreateParameter("@sr", _sr),
                          DBClass.CreateParameter("@tenderId", invId.ToString()),
                          DBClass.CreateParameter("@type", "BOQ"),
                          DBClass.CreateParameter("@itemName", _description),
                          DBClass.CreateParameter("@qty", _qty),
                          DBClass.CreateParameter("@unit", dgvItems.Rows[i].Cells["unit"].Value == null ? " " : dgvItems.Rows[i].Cells["unit"].Value.ToString()),
                          DBClass.CreateParameter("@rate", _rate),
                          DBClass.CreateParameter("amount", _amount),
                          DBClass.CreateParameter("length", _length),
                          DBClass.CreateParameter("width", _width),
                          DBClass.CreateParameter("thickness", _thick),
                          DBClass.CreateParameter("note", _note)).ToString()
                          );

                        DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_tender_details (sr,tender_id, item_id, qty,unit_id,rate, amount,length,width,thickness,note,margin_percentage,margin_amount)
                                         VALUES (@sr,@tenderId, @itemId, @qty,@unitId,@rate, @amount,@length,@width,@thickness,@note,@margin_percentage,@margin_amount);",
                        DBClass.CreateParameter("sr", _sr),
                        DBClass.CreateParameter("tenderId", invId),
                        DBClass.CreateParameter("itemId", _itemId),
                        DBClass.CreateParameter("qty", _qty),
                        DBClass.CreateParameter("unitId", "0"),
                        DBClass.CreateParameter("rate", _rate),
                        DBClass.CreateParameter("amount", _amount),
                        DBClass.CreateParameter("length", _length),
                        DBClass.CreateParameter("width", _width),
                        DBClass.CreateParameter("thickness", _thick),
                        DBClass.CreateParameter("note", _note),
                        DBClass.CreateParameter("margin_percentage", _marginPercentage),
                        DBClass.CreateParameter("margin_amount", _marginAmount));

                        if (!string.IsNullOrEmpty(_sr) && Regex.IsMatch(_sr, @"^[A-Za-z]+$"))
                        {
                            refSr = _sr;
                            assemblyItemId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_items(
                                        code, warehouse_id, type, category_id, name, unit_id, barcode, cost_price, 
                                        cogs_account_id, vendor_id, sales_price, income_account_id, asset_account_id, 
                                        min_amount, max_amount, on_hand, method, total_value, date, img, active, state, 
                                        created_By, created_date, Item_type)
                                    SELECT
                                        @code, @warehouseId, @type, @category, @name, @unit_id, @barcode, @cost_price, 
                                        @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                        @min_amount, @max_amount, @on_hand, @method, @total_value, @date, @img, @active, @state, 
                                        @created_By, @created_date, @Item_type
                                    WHERE NOT EXISTS (
                                        SELECT 1 FROM tbl_items WHERE name = @name
                                    ); SELECT LAST_INSERT_ID();
                                    ",
                                DBClass.CreateParameter("code", _itemCode.ToString()),
                                DBClass.CreateParameter("warehouseId", cmbWarehouse.SelectedValue),
                                DBClass.CreateParameter("type", "13 - Inventory Assembly"),
                                DBClass.CreateParameter("category", 0),
                                DBClass.CreateParameter("name", _description),
                                DBClass.CreateParameter("unit_id", "0"),
                                DBClass.CreateParameter("barcode", ""),
                                DBClass.CreateParameter("cost_price", _rate),
                                DBClass.CreateParameter("cogs_account_id", cmbCOGSAccount),
                                DBClass.CreateParameter("vendor_id", 0),
                                DBClass.CreateParameter("sales_price", "0"),
                                DBClass.CreateParameter("income_account_id", cmbIncomeAccount),
                                DBClass.CreateParameter("asset_account_id", cmbAssetAccount),
                                DBClass.CreateParameter("min_amount", 0),
                                DBClass.CreateParameter("max_amount", 0),
                                DBClass.CreateParameter("on_hand", _qty),
                                DBClass.CreateParameter("method", "fifo"),
                                DBClass.CreateParameter("total_value", 0),
                                DBClass.CreateParameter("date", dtpt.Value.Date),
                                DBClass.CreateParameter("img", ""),
                                DBClass.CreateParameter("active", 0),
                                DBClass.CreateParameter("state", 0),
                                DBClass.CreateParameter("created_By", frmLogin.userId),
                                DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                DBClass.CreateParameter("Item_type", "Inventory")).ToString());
                            _itemCode = _itemCode + 1;
                            
                            Utilities.LogAudit(frmLogin.userId, "Add Project Tender Item", "Project Tender", (int)invId, "Added Item: " + _description + ", Code: " + _itemCode + ", Qty: " + _qty + ", Rate: " + _rate);

                    }
                        else
                        {
                            //if (AssemblyItemManager.GetItemsList().Count > 0)
                            //{
                            //    var result = AssemblyItemManager.GetItemsListWhere(_sr);
                            //    foreach (var item in result)
                            //    {
                            var item = new AssemblyItemModel
                            {
                                ItemId = 1,
                                RefId = "0",
                                No = "",
                                Code = refSr + (subId + 1),
                                Name = _description,
                                Cost = decimal.Parse(_rate),
                                Qty = decimal.Parse(_qty),
                                Total = decimal.Parse(_amount),
                                AssetAccountId = cmbAssetAccount,
                                COGSAccountId = cmbCOGSAccount,
                                IncomeAccountId = cmbIncomeAccount,
                                VendorAccountId = 0
                            };

                            int assemblyId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_items_boq_details(code, warehouse_id,type,category_id, name,unit_id,barcode,cost_price, 
                                        cogs_account_id,vendor_id,sales_price,income_account_id,asset_account_id, 
                                        min_amount,max_amount,on_hand,method, total_value,date,img,active,state, 
                                        created_By,created_date,ref_id) VALUES (
                                        @code, @warehouse_id, @type,@category, @name, @unit_id, @barcode, @cost_price, 
                                        @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                        @min_amount, @max_amount, @on_hand,@method, @total_value, @date, @img, @active, @state, 
                                        @created_By, @created_date,@refId); SELECT LAST_INSERT_ID();
                                        ",
                                                     DBClass.CreateParameter("code", item.Code),
                                                     DBClass.CreateParameter("warehouse_id", 0),
                                                     DBClass.CreateParameter("type", "13 - Inventory Assembly"),
                                                     DBClass.CreateParameter("category", 0),
                                                     DBClass.CreateParameter("name", item.Name),
                                                     DBClass.CreateParameter("unit_id", 0),
                                                     DBClass.CreateParameter("barcode", ""),
                                                     DBClass.CreateParameter("cost_price", decimal.Parse(item.Cost.ToString())),
                                                     DBClass.CreateParameter("cogs_account_id", item.COGSAccountId),
                                                     DBClass.CreateParameter("vendor_id", item.VendorAccountId),
                                                     DBClass.CreateParameter("sales_price", 0),
                                                     DBClass.CreateParameter("income_account_id", item.IncomeAccountId),
                                                     DBClass.CreateParameter("asset_account_id", item.AssetAccountId),
                                                     DBClass.CreateParameter("min_amount", 0),
                                                     DBClass.CreateParameter("max_amount", 0),
                                                     DBClass.CreateParameter("on_hand", item.Qty),
                                                     DBClass.CreateParameter("method", "fifo"),
                                                     DBClass.CreateParameter("total_value", 0),
                                                     DBClass.CreateParameter("date", dtpt.Value.Date),
                                                     DBClass.CreateParameter("img", ""),
                                                     DBClass.CreateParameter("active", 0),
                                                     DBClass.CreateParameter("state", 0),
                                                     DBClass.CreateParameter("created_By", frmLogin.userId),
                                                     DBClass.CreateParameter("created_date", DateTime.Now.Date),
                                                     DBClass.CreateParameter("refId", _itemId.ToString())).ToString());

                            int itemIdOf = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO `tbl_items`(
                                    `code`, `warehouse_id`, `type`, category_id, `name`, `unit_id`, `barcode`, `cost_price`, 
                                    `cogs_account_id`, `vendor_id`, `sales_price`, `income_account_id`, `asset_account_id`, 
                                    `min_amount`, `max_amount`, `on_hand`, method, `total_value`, `date`, `img`, `active`, `state`, 
                                    `created_By`, `created_date`, Item_type)
                                SELECT 
                                    @code, @warehouseId, @type, @category, @name, @unit_id, @barcode, @cost_price, 
                                    @cogs_account_id, @vendor_id, @sales_price, @income_account_id, @asset_account_id, 
                                    @min_amount, @max_amount, @on_hand, @method, @total_value, @date, @img, @active, @state, 
                                    @created_By, @created_date, @Item_type
                                WHERE NOT EXISTS (
                                    SELECT 1 FROM tbl_items WHERE name = @name
                                ); SELECT LAST_INSERT_ID();
                                ",
                            DBClass.CreateParameter("code", _itemCode),
                            DBClass.CreateParameter("warehouseId", cmbWarehouse.SelectedValue),
                            DBClass.CreateParameter("type", "11 - Inventory Part"),
                            DBClass.CreateParameter("category", 0),
                            DBClass.CreateParameter("name", item.Name),
                            DBClass.CreateParameter("unit_id", 0),
                            DBClass.CreateParameter("barcode", ""),
                            DBClass.CreateParameter("cost_price", decimal.Parse(item.Cost.ToString())),
                            DBClass.CreateParameter("cogs_account_id", item.COGSAccountId),
                            DBClass.CreateParameter("vendor_id", item.VendorAccountId),
                            DBClass.CreateParameter("sales_price", "0"),
                            DBClass.CreateParameter("income_account_id", item.IncomeAccountId),
                            DBClass.CreateParameter("asset_account_id", item.AssetAccountId),
                            DBClass.CreateParameter("min_amount", 0),
                            DBClass.CreateParameter("max_amount", 0),
                            DBClass.CreateParameter("on_hand", item.Qty),
                            DBClass.CreateParameter("method", "fifo"),
                            DBClass.CreateParameter("total_value", 0),
                            DBClass.CreateParameter("date", dtpt.Value),
                            DBClass.CreateParameter("img", ""),
                            DBClass.CreateParameter("active", 0),
                            DBClass.CreateParameter("state", 0),
                            DBClass.CreateParameter("created_By", frmLogin.userId),
                            DBClass.CreateParameter("created_date", DateTime.Now.Date),
                            DBClass.CreateParameter("@Item_type", "Inventory")).ToString());

                            DBClass.ExecuteNonQuery("insert into tbl_item_assembly_bos(assembly_id,item_id,qty) values (@assembly_id,@item_id,@qty);insert into tbl_item_assembly(assembly_id,item_id,qty) values (@assembly_item_id,@itemId,@qty)",
                            DBClass.CreateParameter("assembly_id", _itemId.ToString()),
                            DBClass.CreateParameter("assembly_item_id", itemIdOf.ToString()),
                            DBClass.CreateParameter("item_id", assemblyId.ToString()),
                            DBClass.CreateParameter("itemId", assemblyItemId.ToString()),
                            DBClass.CreateParameter("qty", _qty));

                            if (_qty != "0")
                            {
                                insertItemTransaction(item.Qty, dtpt.Value, decimal.Parse(item.Cost.ToString()), invId.ToString(), itemIdOf.ToString());
                                insertItemJournal(item.Qty, dtpt.Value, _itemCode.ToString(), decimal.Parse(item.Cost.ToString()), 0, invId.ToString());
                            }
                            _itemCode = _itemCode + 1;

                            Utilities.LogAudit(frmLogin.userId, "Add Project Tender Item", "Project Tender", (int)invId, "Added Item: " + _description + ", Code: " + _itemCode + ", Qty: " + _qty + ", Rate: " + _rate);
                    }
                        //    }
                        //}
                    }
                }
            
        }
        private void insertItemTransaction(decimal qty, DateTime dated, decimal cost, string pId, string itemId)
        {
            CommonInsert.InsertItemTransaction(dtpt.Value.Date, "Project Tender", pId.ToString(), itemId.ToString(), cost.ToString(),
                qty.ToString(), "0", "0", qty.ToString(), "Project Opening Balance", "0");
        }
        private void insertItemJournal(decimal qty, DateTime dated, string itemCode, decimal cost, decimal totalValue, string pId)
        {
            totalValue = qty * cost;
            CommonInsert.InsertTransactionEntry(dated.Date, BindCombos.SelectDefaultLevelAccount("Inventory").ToString(), totalValue.ToString(), "0",
                pId.ToString(), pId.ToString(), "Project Tender", "Project Opening Balance - Item Code - " + itemCode, frmLogin.userId, DateTime.Now.Date);
            CommonInsert.InsertTransactionEntry(dated.Date, BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString(), "0", totalValue.ToString(),
                    pId.ToString(), "0", "Project Tender", "Project Opening Balance Equity - Item Code - " + itemCode, frmLogin.userId, DateTime.Now.Date);
        }
        private void insertGridItemsOld()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_items_boq SET unit_name=@unit,qty=@qty,price=@rate,amount=@amount Where ref_id=@tenderId and id=(SELECT item_id from tbl_project_tender_details where id=@itemId)",
                      DBClass.CreateParameter("@itemId", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                      DBClass.CreateParameter("@tenderId", invId.ToString()),
                      DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? "0" : dgvItems.Rows[i].Cells["qty"].Value.ToString()),
                      DBClass.CreateParameter("@unit", dgvItems.Rows[i].Cells["unit"].Value == DBNull.Value ? "0" : dgvItems.Rows[i].Cells["unit"].Value.ToString()),
                      DBClass.CreateParameter("@rate", dgvItems.Rows[i].Cells["rate"].Value == DBNull.Value || dgvItems.Rows[i].Cells["rate"].Value.ToString() == "" ? "0" : dgvItems.Rows[i].Cells["rate"].Value.ToString()),
                      DBClass.CreateParameter("amount", dgvItems.Rows[i].Cells["amount"].Value == DBNull.Value ? "0" : dgvItems.Rows[i].Cells["amount"].Value.ToString()));

                DBClass.ExecuteNonQuery(@"UPDATE tbl_project_tender_details SET qty=@qty,unit_id=@unitId,rate=@rate, amount=@amount,margin_percentage=@marginPercentage,margin_amount=@marginAmount,total=@total
                                         WHERE tender_id = @tenderId and id=@item_id",
                  DBClass.CreateParameter("@tenderId", invId),
                  DBClass.CreateParameter("@item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                  DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value)),
                  DBClass.CreateParameter("@unitId", dgvItems.Rows[i].Cells["unit"].Value == DBNull.Value ? "0" : isExported ? dgvItems.Rows[i].Cells["unit"].Value.ToString() : Convert.ToDecimal(dgvItems.Rows[i].Cells["unit"].Value).ToString()),
                  DBClass.CreateParameter("@rate", dgvItems.Rows[i].Cells["rate"].Value == DBNull.Value || dgvItems.Rows[i].Cells["rate"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["rate"].Value)),
                  DBClass.CreateParameter("@amount", dgvItems.Rows[i].Cells["amount"].Value.ToString()),
                  DBClass.CreateParameter("@marginPercentage", dgvItems.Rows[i].Cells["marginPercentage"].Value == DBNull.Value || dgvItems.Rows[i].Cells["marginPercentage"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["marginPercentage"].Value)),
                  DBClass.CreateParameter("@marginAmount", dgvItems.Rows[i].Cells["marginAmount"].Value == DBNull.Value || dgvItems.Rows[i].Cells["marginAmount"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["marginAmount"].Value)),
                  DBClass.CreateParameter("@total", dgvItems.Rows[i].Cells["total"].Value.ToString()));
            }
        }
        private bool chkRequiredDate()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["qty"].Value == null
                    || dgvItems.Rows[i].Cells["qty"].Value.ToString() == ""
                    || decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString()) == 0)
                {
                    //MessageBox.Show("Qty In Row " + (dgvItems.Rows[i].Index + 1) + " Can't Be 0 or Null");
                    //return false;
                    dgvItems.Rows[i].Cells["qty"].Value = 0;
                    dgvItems.Rows[i].Cells["rate"].Value = 0;
                    dgvItems.Rows[i].Cells["amount"].Value = 0;
                    dgvItems.Rows[i].Cells["qty"].Value = 0;
                    dgvItems.Rows[i].Cells["rate"].Value = 0;
                    dgvItems.Rows[i].Cells["amount"].Value = 0;
                }
            }
            if (cmbProject.SelectedValue == null)
            {
                MessageBox.Show("Project Must be Selected.");
                cmbProject.Focus();
                return false;
            }
            if (cmbTenderName.SelectedValue == null)
            {
                MessageBox.Show("Tender Must be Selected.");
                cmbTenderName.Focus();
                return false;
            }
            if (dgvItems.Rows.Count == 1)
            {
                MessageBox.Show("Insert Items First.");
                return false;
            }
            if (txtFees.Text == "")
            {
                txtFees.Text = "0";
            }
            if (txtTotal.Text == "" || decimal.Parse(txtTotal.Text) == 0)
            {
                MessageBox.Show("Total Must Be Bigger Than Zero");
                return false;
            }

            return true;
        }
        private void resetTextBox()
        {

            txtDescription.Text = txtTotal.Text = txtFees.Text = "";
            id = 0;
            dtpt.Value = DateTime.Now;
            dtsd.Value = DateTime.Now;
            dgvItems.Rows.Clear();
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {

            if (updateData())
                resetTextBox();

        }
        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["name"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxName_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxName_SelectedIndexChanged);
                }
            }
            else if (dgvItems.CurrentCell.ColumnIndex == dgvItems.Columns["unit"].Index)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.SelectedIndexChanged -= new EventHandler(ComboBoxUnit_SelectedIndexChanged);
                    comboBox.SelectedIndexChanged += new EventHandler(ComboBoxUnit_SelectedIndexChanged);
                }
            }
        }
        private void ComboBoxUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                // insertItemThroughCodeOrCombo("combo", null, comboBox);
                dgvItems.CurrentRow.Cells["unitId"].Value = comboBox.SelectedValue.ToString();
            }
            //chkRowValidty();
        }
        private void chkRowValidty()
        {
            decimal price = GetDecimalValue(dgvItems.CurrentRow, "rate");
            decimal qty = GetDecimalValue(dgvItems.CurrentRow, "qty");

            if (price == 0 || qty == 0)
                dgvItems.CurrentRow.Cells["amount"].Value = "0";
            else
            {
                //DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)dgvItems.CurrentRow.Cells["unit"];
                //if (comboCell.Value != null)
                //{
                //    //
                //}
                //var comboCell2 = dgvItems.CurrentRow.Cells["unitId"];
                //if (comboCell2.Value != null)
                //{
                //    //
                //}
                decimal totalAmount = 0, subTotal = 0;
                subTotal = GetDecimalValue(dgvItems.CurrentRow, "rate") * GetDecimalValue(dgvItems.CurrentRow, "qty");
                totalAmount = subTotal + GetDecimalValue(dgvItems.CurrentRow, "marginAmount");

                dgvItems.CurrentRow.Cells["amount"].Value = ((decimal.Parse(dgvItems.CurrentRow.Cells["qty"].Value.ToString()) *
                    decimal.Parse(dgvItems.CurrentRow.Cells["rate"].Value.ToString())));
                dgvItems.CurrentRow.Cells["total"].Value = totalAmount;
            }
        }
        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
        }
        //bool checkItemValidty(int itemId)
        //{
            //decimal qty = 0;
            //MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items where id = @id",
            //    DBClass.CreateParameter("id", itemId));
            //reader.Read();

            //for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            //{
            //    if (dgvItems.Rows[i].Cells["itemId"].Value == null)
            //    {
            //        dgvItems.Rows.Remove(dgvItems.Rows[i]);
            //        continue;
            //    }
            //    if (dgvItems.Rows[i].Cells["itemId"].Value.ToString() == itemId.ToString())
            //    {
            //        if (dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "")
            //            continue;
            //        qty += decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
            //        //if (qty > decimal.Parse(reader["on_hand"].ToString()))
            //        //{
            //        //    MessageBox.Show("Item Out Of Stock. Item has Only " + reader["on_hand"].ToString() + " On Hand");
            //        //    return false;
            //        //}
            //    }
            //}
        //    return true;
        //}
        void CalculateTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["total"].Value != null)
                    total += Convert.ToDecimal(row.Cells["total"].Value);
            }
            if (total > 0)
            {
                if (txtFees.Text.ToString().Trim().Length > 0 && Convert.ToDecimal(txtFees.Text.ToString().Trim()) > 0)
                {
                    total = total + Convert.ToDecimal(txtFees.Text);
                }
                txtTotal.Text = total.ToString("0.000");
            }
        }
        private bool isUpdating = false;

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];
                decimal price = GetDecimalValue(row, "rate");
                decimal qty = GetDecimalValue(row, "qty");
                decimal marginPercentage = GetDecimalValue(row, "marginPercentage");
                decimal margin = GetDecimalValue(row, "marginAmount");
                decimal subTotal = price * qty;

                if (isUpdating) return;

                isUpdating = true;
                try
                {
                    if (isExported)
                    {
                        if (e.ColumnIndex == dgvItems.Columns["name"].Index)
                        {
                            string codeValue = row.Cells["name"].Value?.ToString();
                        }
                        else if (e.ColumnIndex == dgvItems.Columns["qty"].Index)
                        {
                            if (dgvItems.CurrentRow.Cells["name"].Value == null)
                                row.Cells["qty"].Value = 0;
                        }
                        else if (e.ColumnIndex == dgvItems.Columns["marginPercentage"].Index)
                        {
                            if (subTotal > 0)
                                row.Cells["marginAmount"].Value = decimal.Parse((subTotal * (marginPercentage / 100)).ToString()).ToString("F2");
                        }
                        else if (e.ColumnIndex == dgvItems.Columns["marginAmount"].Index)
                        {
                            if (subTotal > 0)
                                row.Cells["marginPercentage"].Value = decimal.Parse(((margin / subTotal) * 100).ToString()).ToString("F2");
                        }
                    }
                    else
                    {
                        if (e.ColumnIndex == dgvItems.Columns["Code"].Index)
                        {
                            string codeValue = row.Cells["Code"].Value?.ToString();
                            DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                            if (comboCell != null)
                                insertItemThroughCodeOrCombo("code", comboCell, null);
                        }
                        else if (e.ColumnIndex == dgvItems.Columns["qty"].Index)
                        {
                            if (dgvItems.CurrentRow.Cells["itemId"].Value == null)
                                row.Cells["qty"].Value = 0;
                        }
                        else if (e.ColumnIndex == dgvItems.Columns["marginPercentage"].Index)
                        {
                            if (subTotal > 0)
                                row.Cells["marginAmount"].Value = decimal.Parse((subTotal * (marginPercentage / 100)).ToString()).ToString("F2");
                        }
                        else if (e.ColumnIndex == dgvItems.Columns["marginAmount"].Index)
                        {
                            if (subTotal > 0)
                                row.Cells["marginPercentage"].Value = decimal.Parse(((margin / subTotal) * 100).ToString()).ToString("F2");
                        }
                    }
                    chkRowValidty();
                    CalculateTotal();
                }
                finally
                {
                    isUpdating = false;
                }
            }
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
        private void insertItemThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            MySqlDataReader reader = null;

            if (type == "code")
            {
                reader = DBClass.ExecuteReader(@"SELECT *
                  FROM tbl_items 
                  WHERE code = @code AND  warehouse_id = @w",
                    DBClass.CreateParameter("code", dgvItems.CurrentRow.Cells["code"].Value.ToString()),
                    DBClass.CreateParameter("w", cmbWarehouse.SelectedValue.ToString()));
            }
            else if (type == "combo" && comboBox.SelectedValue != null)
            {
                string selectedItemCode = comboBox.SelectedValue.ToString();
                reader = DBClass.ExecuteReader(@"SELECT tbl_items.id,method,type,code,sales_price,unit_id 
                  FROM tbl_items 
                  WHERE warehouse_id = @id  and code = @code",
                    DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()),
                    DBClass.CreateParameter("code", selectedItemCode));
            }

            if (reader != null && reader.Read())
            {
                dgvItems.CurrentRow.Cells["qty"].Value = 0;
                dgvItems.CurrentRow.Cells["code"].Value = reader["code"].ToString();
                dgvItems.CurrentRow.Cells["itemId"].Value = reader["id"].ToString();
                dgvItems.CurrentRow.Cells["rate"].Value = Convert.ToDecimal(reader["sales_price"]);
                dgvItems.CurrentRow.Cells["unitId"].Value = reader["unit_id"].ToString();
                if (type == "code" && comboCell != null)
                    comboCell.Value = dgvItems.CurrentRow.Cells["code"].Value.ToString();

                string itemType = reader["type"].ToString();
                string itemMethode = reader["method"].ToString();
                CalculateSRAndUpdateDataGridView(itemType);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            //MySqlDataReader reader;
            //if (id == 0)
            //    reader = DBClass.ExecuteReader("select max(id) as id from tbl_project_tender where state = 0");
            //else
            //    reader = DBClass.ExecuteReader("select max(id) as id from tbl_project_tender where state = 0 and id <" + id);
            //if (reader.Read() && reader["id"].ToString() != "")
            //{
            //    id = int.Parse(reader["id"].ToString());
            //    BindData();
            //}
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            //MySqlDataReader reader;
            //if (id == 0)
            //    reader = DBClass.ExecuteReader("SELECT MIN(id) AS id FROM tbl_project_tender WHERE state = 0");
            //else
            //    reader = DBClass.ExecuteReader("SELECT MIN(id) AS id FROM tbl_project_tender WHERE state = 0 AND id > " + id);

            //if (reader.Read() && reader["id"].ToString() != "")
            //{
            //    id = int.Parse(reader["id"].ToString());
            //    BindData();
            //}
        }


        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
                {
                    dgvItems.Rows.Remove(dgvItems.CurrentRow);
                    CalculateTotal();
                }
            }
            catch
            {
                //
            }
        }
        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoaddgvItems();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '.' && !txtFees.Text.Contains("."))
            {
                return;
            }
            e.Handled = true;
        }

        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProject.SelectedValue == null)
            {
                txtProjectCode.Text = "";
                return;
            }
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_projects where id = " + cmbProject.SelectedValue.ToString());
            if (reader.Read())
                txtProjectCode.Text = reader["code"].ToString();
            else
                txtProjectCode.Text = "";
        }

        private void txtProjectCode_TextChanged(object sender, EventArgs e)
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_projects where code =@code",
                DBClass.CreateParameter("code", txtProjectCode.Text));
            if (reader.Read())
                cmbAccountName.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtProjectCode_Leave(object sender, EventArgs e)
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_projects where code =@code",
           DBClass.CreateParameter("code", txtProjectCode.Text));
            if (!reader.Read())
                cmbAccountName.SelectedIndex = -1;
        }

        private void CalculateSRAndUpdateDataGridView(string itemType)
        {
            int mainCount = 0;
            Dictionary<int, int> subCountByMain = new Dictionary<int, int>();

            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells["type"].Value != null)
                {
                    string type = row.Cells["type"].Value.ToString();
                    if (type == "13 - Inventory Assembly")
                    {
                        mainCount++;
                    }
                    else if (type == "11 - Inventory Part")
                    {
                        int lastMainItem = mainCount;
                        if (!subCountByMain.ContainsKey(lastMainItem))
                        {
                            subCountByMain[lastMainItem] = 0;
                        }
                        subCountByMain[lastMainItem]++;
                    }
                }
            }

            if (itemType == "13 - Inventory Assembly")
            {
                double nextMainSR = mainCount + 1;
                string mainSR = nextMainSR.ToString() + ".0";
                AddItemToDataGridView("13 - Inventory Assembly", mainSR);
            }
            else if (itemType == "11 - Inventory Part")
            {
                int lastMainItem = mainCount;
                int subCount = subCountByMain.ContainsKey(lastMainItem) ? subCountByMain[lastMainItem] : 0;
                double nextSubSR = subCount + 1;
                string subSR = mainCount + "." + nextSubSR.ToString();
                AddItemToDataGridView("11 - Inventory Part", subSR);
            }
        }

        private void AddItemToDataGridView(string itemType, string sr)
        {
            if (dgvItems.CurrentRow.Cells["sr"].Value == null || dgvItems.CurrentRow.Cells["sr"].Value.ToString() == "")
            {
                dgvItems.CurrentRow.Cells["sr"].Value = sr;
                dgvItems.CurrentRow.Cells["type"].Value = itemType;
            }
        }

        private void txtFees_TextChanged(object sender, EventArgs e)
        {
            CalculateTotal();
        }

        private void dgvItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvItems.Columns[e.ColumnIndex].Name == "name")
            {
                var typeValue = dgvItems.Rows[e.RowIndex].Cells["type"].Value;

                if (typeValue != null && typeValue.ToString() == "13 - Inventory Assembly")
                {
                    DataGridViewCell itemNameCell = dgvItems.Rows[e.RowIndex].Cells["name"];
                    itemNameCell.Style.Font = new Font(dgvItems.DefaultCellStyle.Font, FontStyle.Bold);
                }
            }
        }

        private void saveToExcel()
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select a folder to save the Excel file";

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    string filePath = Path.Combine(selectedPath, "Project_"+id+".xlsx");

                    List<TenderItems> tenderItems = new List<TenderItems>();
                    List<EstimateItems> estimateItems = new List<EstimateItems>();
                    for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                    {
                        tenderItems.Add(new TenderItems() { Sr = dgvItems.Rows[i].Cells["sr"].Value.ToString(), Description = dgvItems.Rows[i].Cells["name"].FormattedValue.ToString(), Qty = dgvItems.Rows[i].Cells["qty"].Value.ToString(), Unit = dgvItems.Rows[i].Cells["unit"].FormattedValue.ToString(), Rate = dgvItems.Rows[i].Cells["rate"].Value.ToString(), Amount = dgvItems.Rows[i].Cells["amount"].Value.ToString() });
                        estimateItems.Add(new EstimateItems() { Sr = dgvItems.Rows[i].Cells["sr"].Value.ToString(), Description = dgvItems.Rows[i].Cells["name"].FormattedValue.ToString(), Qty = dgvItems.Rows[i].Cells["qty"].Value.ToString(), Unit = dgvItems.Rows[i].Cells["unit"].FormattedValue.ToString(), Rate = dgvItems.Rows[i].Cells["rate"].Value.ToString(), Amount = dgvItems.Rows[i].Cells["amount"].Value.ToString(), MarginPercentage = dgvItems.Rows[i].Cells["marginPercentage"].Value.ToString(), MarginAmount = dgvItems.Rows[i].Cells["marginAmount"].Value.ToString(), Total = dgvItems.Rows[i].Cells["total"].Value.ToString() });
                    }

                    DataTable dtTender = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(tenderItems), typeof(DataTable));
                    DataTable dtEstimate = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(estimateItems), typeof(DataTable));
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var excelPack = new ExcelPackage(filePath))
                    {
                        var ws = excelPack.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Tender");
                        if (ws != null)
                        {
                            excelPack.Workbook.Worksheets.Delete(ws);
                        }
                        var ws2 = excelPack.Workbook.Worksheets.FirstOrDefault(w => w.Name == "Estimate");
                        if (ws2 != null)
                        {
                            excelPack.Workbook.Worksheets.Delete(ws2);
                        }
                        ws = excelPack.Workbook.Worksheets.Add("Tender");
                        ws.Cells.LoadFromDataTable(dtTender, true, OfficeOpenXml.Table.TableStyles.Light8);
                        ws2 = excelPack.Workbook.Worksheets.Add("Estimate");
                        ws2.Cells.LoadFromDataTable(dtEstimate, true, OfficeOpenXml.Table.TableStyles.Light8);
                        excelPack.Save();

                        MessageBox.Show("Excel file saved successfully!", "Success");
                    }
                }
                else
                {
                    MessageBox.Show("Save canceled by user.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }

        private void txtDefaultMarginPercentage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '.' && !txtDefaultMarginPercentage.Text.Contains("."))
            {
                return;
            }
            e.Handled = true;
        }

        private void txtDefaultMarginPercentage_TextChanged(object sender, EventArgs e)
        {
            UpdateMarginAll();
        }
        private void UpdateMarginAll()
        {
            if (!string.IsNullOrEmpty(txtDefaultMarginPercentage.Text) && txtDefaultMarginPercentage.Text.Length > 0)
            {
                decimal marginPercentage = decimal.Parse(txtDefaultMarginPercentage.Text);
                if (marginPercentage < 0 || marginPercentage > 100)
                {
                    MessageBox.Show("Invalid margin percentage. It should be between 0 and 100.");
                    return;
                }

                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.Cells["name"].Value != null && row.Cells["name"].Value.ToString() != "")
                    {
                        if (row.Cells["marginPercentage"].Value != null)
                        {
                            row.Cells["marginPercentage"].Value = marginPercentage.ToString("0.##");
                            decimal price = GetDecimalValue(row, "rate");
                            decimal qty = GetDecimalValue(row, "qty");
                            decimal subTotal = price * qty;
                            decimal marginAmt = decimal.Parse((subTotal * (marginPercentage / 100)).ToString());
                            row.Cells["marginAmount"].Value = marginAmt.ToString("F2");
                            decimal totalAmount = subTotal + marginAmt;
                            row.Cells["total"].Value = totalAmount.ToString("0.##");
                        }
                        else
                        {
                            row.Cells["marginPercentage"].Value = marginPercentage.ToString("0.##");
                            decimal price = GetDecimalValue(row, "rate");
                            decimal qty = GetDecimalValue(row, "qty");
                            decimal subTotal = price * qty;
                            decimal marginAmt = decimal.Parse((subTotal * (marginPercentage / 100)).ToString());
                            row.Cells["marginAmount"].Value = marginAmt.ToString("F2");
                            decimal totalAmount = subTotal + marginAmt;
                            row.Cells["total"].Value = totalAmount.ToString("F2");
                        }
                    }
                }
                dgvItems.Refresh();
                CalculateTotal();
            }
            else
            {
                decimal marginPercentage = 0;
                if (marginPercentage < 0 || marginPercentage > 100)
                {
                    MessageBox.Show("Invalid margin percentage. It should be between 0 and 100.");
                    return;
                }

                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.Cells["name"].Value != null && row.Cells["name"].Value.ToString() != "")
                    {
                        row.Cells["marginPercentage"].Value = marginPercentage.ToString("0.##");
                        decimal price = GetDecimalValue(row, "rate");
                        decimal qty = GetDecimalValue(row, "qty");
                        decimal subTotal = price * qty;
                        decimal marginAmt = 0;
                        row.Cells["marginAmount"].Value = marginAmt.ToString("F2");
                        decimal totalAmount = subTotal + marginAmt;
                        row.Cells["total"].Value = totalAmount.ToString("F2");
                    }
                }
                dgvItems.Refresh();
            }
        }

    }
}

public class TenderItems
{
    public string Sr { get; set; }
    public string Description { get; set; }
    public string Qty { get; set; }
    public string Unit { get; set; }
    public string Rate { get; set; }
    public string Amount { get; set; }
}

public class EstimateItems
{
    public string Sr { get; set; }
    public string Description { get; set; }
    public string Qty { get; set; }
    public string Unit { get; set; }
    public string Rate { get; set; }
    public string Amount { get; set; }
    public string MarginPercentage { get; set; }
    public string MarginAmount { get; set; }
    public string Total { get; set; }
}