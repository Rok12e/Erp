using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    
    public partial class frmViewProjectTendering : Form
    {

        decimal invId;
        int id;
        bool isExported = false;
        bool isEstimated = false;

        public frmViewProjectTendering(int _id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            if (id != 0)
                this.Text = "Project - Edit Tender";
            else
                this.Text = "Project - New Tender";
            headerUC1.FormText = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewProjectTendering_Load(object sender, EventArgs e)
        {
            dtpt.Value = dtsd.Value = DateTime.Now.Date;
            isExported = true;
            dgvItems.Visible = false;
            AssemblyItemManager.ClearItemsList();
            BindCombo();
            loadGridDesign();
            if (id != 0)
                BindData();
        }
        private void loadGridDesign()
        {
            dgvImportedItem.Visible = true;

            dgvImportedItem.Columns.Clear();
            dgvImportedItem.Columns.Add("id", "#");
            dgvImportedItem.Columns.Add("Sr.", "Sr.");
            dgvImportedItem.Columns.Add("Description of work", "Description of work");
            dgvImportedItem.Columns.Add("Qty", "Qty");
            dgvImportedItem.Columns.Add("Unit", "Unit");
            dgvImportedItem.Columns.Add("Length", "Length");
            dgvImportedItem.Columns.Add("Width", "Width");
            dgvImportedItem.Columns.Add("Thick", "Thick");
            dgvImportedItem.Columns.Add("Rate", "Rate");
            dgvImportedItem.Columns.Add("Amount", "Amount");
            dgvImportedItem.Columns.Add("Note", "Note");

            dgvImportedItem.Columns["id"].Width = 50;
            dgvImportedItem.Columns["Sr."].Width = 50;
            dgvImportedItem.Columns["Description of work"].Width = 650;
            dgvImportedItem.Columns["Qty"].Width = 80;
            dgvImportedItem.Columns["Unit"].Width = 130;
            dgvImportedItem.Columns["Rate"].Width = 120;
            dgvImportedItem.Columns["Amount"].Width = 150;
        }
        private void BindData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_tender where id = @id",
                DBClass.CreateParameter("id", id)))
                if (reader.Read())
                {
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
                    invId = id;
                    isEstimated = reader["estimate_status"] != null ? (int.Parse(reader["estimate_status"].ToString()) == 1 ? true : false) : false;
                    
                    string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
                    int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", id)));
                    isExported = count > 0 ? true : false;

                    BindItems();
                    CalculateTotal();
                }
        }

        private void BindItems()
        {
            if (isExported)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                                                    INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.item_id=tbl_items_boq.id
                                                                    WHERE tbl_project_tender_details.tender_id = @id;",
                                                                DBClass.CreateParameter("id", id)))
                {
                    int count = 0;
                    loadGridDesign();
                    dgvImportedItem.Rows.Clear();

                    //AssemblyItemManager.ClearItemsList();
                    while (reader.Read())
                    {
                        count++;
                        dgvImportedItem.Rows.Add((count).ToString(), reader["sr"].ToString(), reader["name"].ToString(), reader["qty"].ToString(),
                                reader["unit_name"].ToString(), reader["length"].ToString(), reader["width"].ToString(), reader["thickness"].ToString(), reader["rate"].ToString(), reader["amount"].ToString(), reader["note"].ToString());

                        //    using (MySqlDataReader dr = DBClass.ExecuteReader("select ti.code,ti.id,ti.on_hand qty,ti.cost_price rate,ti.name from tbl_item_assembly_bos ta inner join tbl_items_boq_details ti on ta.item_id = ti.id and ti.ref_id=ta.assembly_id where ta.assembly_id=@id", DBClass.CreateParameter("id", reader["item_id"].ToString())))
                        //        while (dr.Read())
                        //        {
                        //            decimal _cost = decimal.Parse(dr["rate"].ToString());
                        //            decimal _qty = decimal.Parse(dr["qty"].ToString());
                        //            AssemblyItemManager.AddItem(new AssemblyItemModel
                        //            {
                        //                ItemId = int.Parse(dr["id"].ToString()),
                        //                RefId = reader["sr"].ToString(),
                        //                No = "",
                        //                Code = dr["code"].ToString(),
                        //                Name = dr["name"].ToString(),
                        //                Cost = _cost,
                        //                Qty = _qty,
                        //                Total = _cost * _qty,
                        //                AssetAccountId = 0,
                        //                COGSAccountId = 0,
                        //                IncomeAccountId = 0,
                        //                VendorAccountId = 0
                        //            });
                        //        }
                    }
                    isExported = true;
                    dgvItems.Visible = false;
                    dgvImportedItem.Visible = true;
                    foreach (DataGridViewRow row in dgvImportedItem.Rows)
                    {
                        if (row.IsNewRow) continue;

                        var srValue = row.Cells["Sr."].Value?.ToString().Trim();

                        if (!string.IsNullOrEmpty(srValue) && Regex.IsMatch(srValue, @"^[A-Za-z]+$"))
                        {
                            row.DefaultCellStyle.Font = new Font(dgvImportedItem.Font, FontStyle.Bold);
                        }
                    }
                }
            }
            else
            {
                dgvItems.Rows.Clear();
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_project_tender_details.*, tbl_items_boq_details.code as code,tbl_items_boq_details.type,(select name from tbl_unit where id=tbl_project_tender_details.unit_id) as unit_name FROM tbl_project_tender_details INNER JOIN 
                                                                    tbl_items_boq_details ON tbl_project_tender_details.item_id = tbl_items_boq_details.id AND tbl_project_tender_details.item_id=tbl_items_boq_details.ref_id WHERE 
                                                                    tbl_project_tender_details.tender_id = @id;",
                                                                DBClass.CreateParameter("id", id)))
                {
                    int count = 0;
                    while (reader.Read())
                    {
                        dgvItems.Rows.Add(reader["item_id"].ToString(), (count++), reader["sr"].ToString(), reader["code"].ToString(), reader["code"].ToString(), reader["qty"].ToString(),
                            int.Parse(reader["unit_id"].ToString()), reader["unit_id"].ToString(), reader["rate"].ToString(), reader["amount"].ToString(), reader["type"].ToString());
                    }
                }
            }
        }
        private void LoaddgvItems()
        {
            dgvItems.Rows.Clear();
            string condition = "";
            if (id == 0)
            {
                condition = " and type='13 - Inventory Assembly'";
            }
                DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items_boq_details where warehouse_id=@id and state = 0 and  active = 0" + condition,
                                DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
            DataGridViewComboBoxColumn cmbItemName = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            cmbItemName.DataSource = dt;
            cmbItemName.DisplayMember = "name";
            cmbItemName.ValueMember = "code";
            DataRow newRow = dt.NewRow();
            newRow["code"] = 0;
            newRow["name"] = "<< Add Assembly Item >>";
            dt.Rows.InsertAt(newRow, 0);
            dt = DBClass.ExecuteDataTable("select id, name from tbl_unit");
            DataGridViewComboBoxColumn cmbUnit = (DataGridViewComboBoxColumn)dgvItems.Columns["unit"];
            cmbUnit.DataSource = dt;
            cmbUnit.DisplayMember = "name";
            cmbUnit.ValueMember = "id";
        }
        public void BindCombo()
        {
            BindCombos.PopulateProjects(cmbProject);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            BindCombos.PopulateTenderNames(cmbTenderName);
            BindCombos.PopulateWarehouse(cmbWarehouse);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertData())
                    this.Close();
            }
            else
            {
                if (isEstimated)
                {
                    MessageBox.Show("Estimate already generated.\nCan't edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (updateData())
                        this.Close();
                }
            }
        }
        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;
            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_tender 
                                    SET modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date, project_id = @projectId, submission_date = @submissionDate, 
                                    description=@description,fees=@fees,warehouse_id = @warehouse_id,account_id = @account_id,tender_name_id = @tenderNameId WHERE id = @id;",
               DBClass.CreateParameter("id", id),
               DBClass.CreateParameter("date", dtpt.Value.Date),
               DBClass.CreateParameter("submissionDate", dtsd.Value.Date),
               DBClass.CreateParameter("projectId", cmbProject.SelectedValue),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue),
               DBClass.CreateParameter("tenderNameId", cmbTenderName.SelectedValue),
               DBClass.CreateParameter("fees", txtFees.Text),
               DBClass.CreateParameter("description", txtDescription.Text),
                DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_project_tender_details WHERE tender_id=@id;", DBClass.CreateParameter("id", id.ToString()));
            //DBClass.ExecuteNonQuery(@"DELETE FROM tbl_project_tender_details WHERE tender_id=@id;
            //                      DELETE FROM tbl_items_boq_details WHERE ref_id IN (SELECT id FROM tbl_items_boq WHERE ref_id=@id);
            //                      DELETE FROM tbl_item_assembly_bos WHERE assembly_id IN (SELECT id FROM tbl_items_boq WHERE ref_id=@id);
            //DELETE FROM tbl_items_boq WHERE ref_id=@id", DBClass.CreateParameter("id", id.ToString()));
            //DBClass.ExecuteNonQuery("delete from tbl_item_assembly where assembly_id IN (SELECT distinct item_id FROM tbl_item_transactionwhere type = 'Project Tender' and item_id = @id;)" + id);
            //DBClass.ExecuteNonQuery(@"DELETE FROM tbl_item_transaction where type = 'Project Tender' and item_id = @id;DELETE FROM tbl_transaction where type = 'Project Tender' AND transaction_id= @id", DBClass.CreateParameter("id", id));
            //DBClass.ExecuteNonQuery("DELETE FROM tbl_item_card_details WHERE `trans_type` = 'Project Tender' and itemId = @id", DBClass.CreateParameter("id", id));

            insertGridItems();
            Utilities.LogAudit(frmLogin.userId, "Update Project Tender", "Project Tender", (int)id, "Updated Project Tender: " + cmbTenderName.Text + " with ID: " + id);
            EventHub.RefreshProjectTendering();

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_project_tender 
                            (date, tender_name_id,account_id,project_id,fees,submission_date,description, warehouse_id, created_by, created_date,state)
                            VALUES 
                            (@date,@tenderNameId,@accountId,@projectId,@fees, @submissionDate,@description, @warehouse_id, @created_by, @created_date,0);
                            SELECT LAST_INSERT_ID();",
            DBClass.CreateParameter("date", dtpt.Value.Date),
            DBClass.CreateParameter("tenderNameId", cmbTenderName.SelectedValue),
            DBClass.CreateParameter("accountId", cmbAccountName.SelectedValue),
            DBClass.CreateParameter("projectId", cmbProject.SelectedValue),
            DBClass.CreateParameter("fees", txtFees.Text),
            DBClass.CreateParameter("submissionDate", dtsd.Value.Date),
            DBClass.CreateParameter("description", txtDescription.Text),
            DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
            DBClass.CreateParameter("created_by", frmLogin.userId),
            DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());

            insertGridItems();
            Utilities.LogAudit(frmLogin.userId, "Create Project Tender", "Project Tender", (int)invId, "Created Project Tender: " + cmbTenderName.Text + " with ID: " + invId);
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

            if (isExported)
            {
                string refSr = "";
                int subId = 0;
                int assemblyItemId = 0;
                for (int i = 0; i < dgvImportedItem.Rows.Count - 1; i++)
                {
                    string _sr = id > 0 ? dgvImportedItem.Rows[i].Cells["sr."].Value.ToString() : dgvImportedItem.Rows[i].Cells["sr."].Value.ToString();
                    string _description = id > 0 ? dgvImportedItem.Rows[i].Cells["Description of work"].Value.ToString() : dgvImportedItem.Rows[i].Cells["Description of work"].Value.ToString();
                    string _rate = dgvImportedItem.Rows[i].Cells["rate"].Value == null || dgvImportedItem.Rows[i].Cells["rate"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["rate"].Value.ToString();
                    string _qty = dgvImportedItem.Rows[i].Cells["qty"].Value == null || dgvImportedItem.Rows[i].Cells["qty"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["qty"].Value.ToString();
                    string _amount = dgvImportedItem.Rows[i].Cells["amount"].Value == null || dgvImportedItem.Rows[i].Cells["amount"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["amount"].Value.ToString();
                    
                    if (_sr != "" && _description != "")
                    {
                        string _length = dgvImportedItem.Rows[i].Cells["length"].Value == null || dgvImportedItem.Rows[i].Cells["length"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["length"].Value.ToString();
                        string _width = dgvImportedItem.Rows[i].Cells["width"].Value == null || dgvImportedItem.Rows[i].Cells["width"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["width"].Value.ToString();
                        string _thick = dgvImportedItem.Rows[i].Cells["thick"].Value == null || dgvImportedItem.Rows[i].Cells["thick"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["thick"].Value.ToString();
                        string _note = dgvImportedItem.Rows[i].Cells["note"].Value == null ? " " : dgvImportedItem.Rows[i].Cells["note"].Value.ToString();

                        decimal _itemId = (int)decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_items_boq(sr,ref_id,type,name,unit_name,qty,price,amount,length,width,thickness,note)
                                            VALUES(@sr,@tenderId,@type, @itemName,@unit, @qty,@rate, @amount,@length,@width,@thickness,@note); SELECT LAST_INSERT_ID();",
                          DBClass.CreateParameter("@sr", _sr),
                          DBClass.CreateParameter("@tenderId", invId.ToString()),
                          DBClass.CreateParameter("@type", "BOQ"),
                          DBClass.CreateParameter("@itemName", _description),
                          DBClass.CreateParameter("@qty", _qty),
                          DBClass.CreateParameter("@unit", dgvImportedItem.Rows[i].Cells["unit"].Value == null ? " " : dgvImportedItem.Rows[i].Cells["unit"].Value.ToString()),
                          DBClass.CreateParameter("@rate", _rate),
                          DBClass.CreateParameter("amount", _amount),
                          DBClass.CreateParameter("length", _length),
                          DBClass.CreateParameter("width", _width),
                          DBClass.CreateParameter("thickness", _thick),
                          DBClass.CreateParameter("note", _note)).ToString()
                          );

                        DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_tender_details (sr,tender_id, item_id, qty,unit_id,rate, amount,length,width,thickness,note)
                                         VALUES (@sr,@tenderId, @itemId, @qty,@unitId,@rate, @amount,@length,@width,@thickness,@note);",
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
                        DBClass.CreateParameter("note", _note));

                        Utilities.LogAudit(frmLogin.userId, "Create Project Tender Item", "Project Tender Item", (int)invId, "Created Project Tender Item: " + _description + " with ID: " + _itemId);
                    }
                }
            }
            else
            {
                for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                {
                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_items_boq(sr,ref_id,type,name,unit_name,qty,price,amount)
                                            VALUES(@sr,@tenderId,@type, @itemName,@unit, @qty,@rate, @amount)",
                          DBClass.CreateParameter("@sr", dgvItems.Rows[i].Cells["sr"].Value.ToString()),
                          DBClass.CreateParameter("@tenderId", invId.ToString()),
                          DBClass.CreateParameter("@type", "BOQ"),
                          DBClass.CreateParameter("@itemName", dgvItems.Rows[i].Cells["name"].Value.ToString()),
                          DBClass.CreateParameter("@qty", dgvImportedItem.Rows[i].Cells["qty"].Value == DBNull.Value ? "0" : dgvImportedItem.Rows[i].Cells["qty"].Value.ToString()),
                          DBClass.CreateParameter("@unit", dgvImportedItem.Rows[i].Cells["unit"].Value == DBNull.Value ? "0" : dgvImportedItem.Rows[i].Cells["unit"].Value.ToString()),
                          DBClass.CreateParameter("@rate", dgvImportedItem.Rows[i].Cells["rate"].Value == DBNull.Value || dgvImportedItem.Rows[i].Cells["rate"].Value.ToString() == "" ? "0" : dgvImportedItem.Rows[i].Cells["rate"].Value.ToString()),
                          DBClass.CreateParameter("amount", dgvImportedItem.Rows[i].Cells["amount"].Value == DBNull.Value ? "0" : dgvImportedItem.Rows[i].Cells["amount"].Value.ToString()));

                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_tender_details (sr,tender_id, item_id, qty,unit_id,rate, amount)
                                         VALUES (@sr,@tenderId, @itemName, @qty,@unitId,@rate, @amount);",
                      DBClass.CreateParameter("@sr", dgvItems.Rows[i].Cells["sr"].Value.ToString()),
                      DBClass.CreateParameter("@tenderId", invId),
                      DBClass.CreateParameter("@item_id", dgvItems.Rows[i].Cells["itemId"].Value.ToString()),
                      DBClass.CreateParameter("@qty", dgvItems.Rows[i].Cells["qty"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["qty"].Value)),
                      DBClass.CreateParameter("@unitId", dgvItems.Rows[i].Cells["unitId"].Value == DBNull.Value ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["unitId"].Value)),
                      DBClass.CreateParameter("@rate", dgvItems.Rows[i].Cells["rate"].Value == DBNull.Value || dgvItems.Rows[i].Cells["rate"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dgvItems.Rows[i].Cells["rate"].Value)),
                      DBClass.CreateParameter("@amount", dgvItems.Rows[i].Cells["amount"].Value = 0));
                }
            }
        }
        //private void insertItemTransaction(decimal qty,DateTime dated,decimal cost, string pId, string itemId)
        //{
        //    CommonInsert.InsertItemTransaction(dtpt.Value.Date, "Project Tender", pId.ToString(), itemId.ToString(), cost.ToString(),
        //        qty.ToString(), "0", "0", qty.ToString(), "Project Opening Balance", "0");
        //}
        //private void insertItemJournal(decimal qty, DateTime dated, string itemCode, decimal cost,decimal totalValue, string pId)
        //{
        //    totalValue = qty * cost;
        //    CommonInsert.InsertTransactionEntry(dated.Date, BindCombos.SelectDefaultLevelAccount("Inventory").ToString(), totalValue.ToString(), "0",
        //        pId.ToString(), pId.ToString(), "Project Tender", "Project Opening Balance - Item Code - " + itemCode, frmLogin.userId, DateTime.Now.Date);
        //    CommonInsert.InsertTransactionEntry(dated.Date, BindCombos.SelectDefaultLevelAccount("Opening Balance Equity").ToString(), "0", totalValue.ToString(),
        //            pId.ToString(), "0", "Project Tender", "Project Opening Balance Equity - Item Code - " + itemCode, frmLogin.userId, DateTime.Now.Date);
        //}

        private bool chkRequiredDate()
        {
            if (cmbProject.SelectedValue == null)
            {
                MessageBox.Show("Project must be selected.");
                cmbProject.Focus();
                return false;
            }
            if (cmbTenderName.SelectedValue == null)
            {
                MessageBox.Show("Tender must be selected.");
                cmbTenderName.Focus();
                return false;
            }
            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Warehouse must be selected.");
                cmbWarehouse.Focus();
                return false;
            }
            if (dgvImportedItem.Rows.Count == 1)
            {
                MessageBox.Show("Insert Items First.");
                return false;
            }

            for (int i = 0; i < dgvImportedItem.Rows.Count - 1; i++)
            {
                if (dgvImportedItem.Rows[i].Cells["Sr."].Value == null || dgvImportedItem.Rows[i].Cells["Sr."].Value.ToString() == "")
                {
                    MessageBox.Show("Sr. cant be empty");
                    return false;
                }
            }

                    if (isExported)
            {
                for (int i = 0; i < dgvImportedItem.Rows.Count - 1; i++)
                {
                    if (dgvImportedItem.Rows[i].Cells["qty"].Value == null
                        || dgvImportedItem.Rows[i].Cells["qty"].Value.ToString() == ""
                        || decimal.Parse(dgvImportedItem.Rows[i].Cells["qty"].Value.ToString()) == 0)
                    {
                        //if (dgvImportedItem.Rows[i].Cells["qty"].Value.ToString() != "13 - Inventory Assembly") {
                        //    MessageBox.Show("Qty In Row " + (dgvImportedItem.Rows[i].Index + 1) + " Can't Be 0 or Null");
                        //    return false;
                        //}
                        dgvImportedItem.Rows[i].Cells["qty"].Value = 0;
                        dgvImportedItem.Rows[i].Cells["rate"].Value = 0;
                        dgvImportedItem.Rows[i].Cells["amount"].Value = 0;
                    }
                }
                if (txtFees.Text == "")
                {
                    txtFees.Text = "0";
                }

                return true;
            }
            else
            {
                dgvItems.AllowUserToAddRows = false;
                for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
                {
                    if (dgvItems.Rows[i].Cells["qty"].Value == null
                        || dgvItems.Rows[i].Cells["qty"].Value.ToString() == ""
                        || decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString()) == 0)
                    {
                        dgvItems.Rows[i].Cells["qty"].Value = 0;
                        dgvItems.Rows[i].Cells["rate"].Value = 0;
                        dgvItems.Rows[i].Cells["amount"].Value = 0;
                    }
                }
                if (txtFees.Text == "")
                {
                    txtFees.Text = "0";
                }

                return true;
            }
        }
        private void resetTextBox()
        {

            txtDescription.Text = txtFees.Text = "";
            id = 0;
            dtpt.Value = DateTime.Now;
            dtsd.Value = DateTime.Now;
            dgvItems.Rows.Clear();
        }
        private void btnSaveNew_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertData())
                    resetTextBox();
            }
            else
            {
                if (isEstimated)
                {
                    MessageBox.Show("Estimate already generated.\nCan't edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (updateData())
                        resetTextBox();
                }
            }
        }
        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            //
        }
        DataGridViewRow selectedRow;
        private void dgvItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            selectedRow = dgvItems.CurrentRow;
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
                DataGridViewComboBoxCell comboCell = (DataGridViewComboBoxCell)dgvItems.CurrentRow.Cells["unit"];
                if (comboCell.Value != null)
                {
                    //
                }
                var comboCell2 = dgvItems.CurrentRow.Cells["unitId"];
                if (comboCell2.Value != null)
                {
                    //
                }
                    dgvItems.CurrentRow.Cells["amount"].Value = ((decimal.Parse(dgvItems.CurrentRow.Cells["qty"].Value.ToString()) *
                    decimal.Parse(dgvItems.CurrentRow.Cells["rate"].Value.ToString())));
            }
        }
        int comboCount = 0;
        string itemTyped = "",selectedItemId="0";
        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                if (comboBox.Text == "<< Add Assembly Item >>")
                {
                    if (comboCount == 0)
                    {
                        string srId = "1.0";
                        int itemId = 0;
                        if (dgvItems.Rows.Count > 1)
                        {
                            int rowIndex = selectedRow.Index - 1;
                            itemId = int.Parse(dgvItems.Rows[rowIndex].Cells[0].Value.ToString());
                            srId = dgvItems.Rows[rowIndex].Cells[2].Value.ToString();
                            itemTyped = dgvItems.Rows[rowIndex].Cells["type"].Value.ToString();
                            selectedItemId = itemId.ToString();
                        }
                        if (itemTyped == "13 - Inventory Assembly")
                        {
                            comboCount++;
                            frmAssemblyItem assemblyForm = new frmAssemblyItem(itemId, srId, int.Parse(txtProjectCode.Text.ToString()));
                            assemblyForm.FormClosedWithResult += AssemblyForm_FormClosedWithResult;
                            assemblyForm.ShowDialog();

                            DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items_boq_details where warehouse_id=@id and state = 0 and active = 0",
                                                            DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
                            DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
                            DataRow newRow = dt.NewRow();
                            newRow["code"] = 0;
                            newRow["name"] = "<< Add Assembly Item >>";
                            dt.Rows.InsertAt(newRow, 0);
                            name.DataSource = dt;
                            name.DisplayMember = "name";
                            name.ValueMember = "code";
                            //comboBox.SelectedIndex = comboBox.Items.Count - 1;
                        } else
                        {
                            MessageBox.Show("Can't add Assembly here because the previous one is not main");
                        }
                    }
                    else
                        comboCount = 0;
                }
                else
                    insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
        }
        private void AssemblyForm_FormClosedWithResult(object sender, EventArgs e)
        {
            if (isExported)
            {
                DataGridViewRow currentRow = dgvImportedItem.CurrentRow;
                if (currentRow != null)
                {
                    var result = AssemblyItemManager.GetItemsListWhere(currentRow.Cells["Sr."].Value.ToString());
                    decimal qty = 0, cost = 0, total = 0;
                    foreach (var item in result)
                    {
                        qty += decimal.Parse(item.Qty.ToString());
                        cost += decimal.Parse(item.Cost.ToString());
                        total += decimal.Parse(item.Total.ToString());
                    }
                    currentRow.Cells["Qty"].Value = qty.ToString();
                    //currentRow.Cells[3].Value = "";
                    currentRow.Cells["Rate"].Value = cost.ToString();
                    currentRow.Cells["Amount"].Value = total.ToString();
                }
            }
            else
            {
                MySqlDataReader reader = DBClass.ExecuteReader("select ti.id item_id,ti.code,ti.id,ta.qty,ti.unit_id,ti.cost_price rate,ti.type FROM tbl_item_assembly ta inner join tbl_items_boq_details ti on ta.item_id = ti.id and ti.ref_id=ta.assembly_id where ta.assembly_id = " + selectedItemId);

                DataGridViewRow currentRow = dgvItems.CurrentRow;
                int count = 1;
                if (currentRow != null)
                {
                    count = int.Parse(currentRow.Cells[1].Value.ToString());
                }

                int currentRowIndex = (currentRow != null) ? currentRow.Index : dgvItems.Rows.Count;

                while (reader.Read())
                {
                    string amount = (decimal.Parse(reader["rate"].ToString()) * decimal.Parse(reader["qty"].ToString())).ToString("N2");
                    string itemType = reader["type"].ToString();

                    DataGridViewRow newRow = new DataGridViewRow();

                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["item_id"].ToString() });
                    newRow.Cells.Add(new DataGridViewTextBoxCell());
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = "0" });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["code"].ToString() });

                    DataGridViewComboBoxCell codeCell = new DataGridViewComboBoxCell();
                    codeCell.Items.Add(reader["code"].ToString());
                    codeCell.Value = reader["code"].ToString();
                    newRow.Cells.Add(codeCell);

                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["qty"].ToString() });

                    DataGridViewComboBoxCell unitCell = new DataGridViewComboBoxCell();
                    //unitCell.Items.Add(reader["unit_id"].ToString());
                    //unitCell.Value = reader["unit_id"].ToString();
                    newRow.Cells.Add(unitCell);

                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = int.Parse(reader["unit_id"].ToString()) });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = reader["rate"].ToString() });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = amount });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = itemType });
                    dgvItems.Rows.Insert(currentRowIndex, newRow);

                    currentRowIndex++;
                    CalculateSRAndUpdateDataGridViewNew();
                }

                //while (reader.Read())
                //{
                //    string amount = (decimal.Parse(reader["rate"].ToString()) * decimal.Parse(reader["qty"].ToString())).ToString("N2");
                //    dgvItems.Rows.Add(reader["item_id"].ToString(), (count++), "0", reader["code"].ToString(), reader["code"].ToString(), reader["qty"].ToString(),
                //        int.Parse(reader["unit_id"].ToString()), reader["unit_id"].ToString(), reader["rate"].ToString(), amount, reader["type"].ToString());

                //    CalculateSRAndUpdateDataGridView(itemTyped);
                //}
            }
        }
        bool checkItemValidty(int itemId)
        {
            decimal qty = 0;
            //MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_items_boq_details where id = @id",
            //    DBClass.CreateParameter("id", itemId));
            //reader.Read();

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["itemId"].Value == null)
                {
                    dgvItems.Rows.Remove(dgvItems.Rows[i]);
                    continue;
                }
                if (dgvItems.Rows[i].Cells["itemId"].Value.ToString() == itemId.ToString())
                {
                    if (dgvItems.Rows[i].Cells["qty"].Value == null || dgvItems.Rows[i].Cells["qty"].Value.ToString() == "")
                        continue;
                    qty += decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString());
                    //if (qty > decimal.Parse(reader["on_hand"].ToString()))
                    //{
                    //    MessageBox.Show("Item Out Of Stock. Item has Only " + reader["on_hand"].ToString() + " On Hand");
                    //    return false;
                    //}
                }
            }
            return true;
        }
        void CalculateTotal()
        {
            if (dgvImportedItem.Visible)
            {
                //decimal amount = 0;
                //foreach (DataGridViewRow row in dgvImportedItem.Rows)
                //{
                //    if (row.Cells["amount"].Value != null)
                //        amount += Convert.ToDecimal(row.Cells["amount"].Value);
                //}
            }
            else
            {
                decimal amount = 0;
                foreach (DataGridViewRow row in dgvItems.Rows)
                {
                    if (row.Cells["amount"].Value != null)
                        amount += Convert.ToDecimal(row.Cells["amount"].Value);
                }
            }
        }
        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                var row = dgvItems.Rows[e.RowIndex];
                decimal price = GetDecimalValue(row, "rate");
                decimal qty = GetDecimalValue(row, "qty");
                if (e.ColumnIndex == dgvItems.Columns["Code"].Index)
                {
                    string codeValue = row.Cells["Code"].Value?.ToString();
                    DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                    if (comboCell != null)
                        insertItemThroughCodeOrCombo("code", comboCell, null);
                }
                else if (e.ColumnIndex == dgvItems.Columns["qty"].Index)
                {
                    if (dgvItems.CurrentRow.Cells["itemId"].Value == null || !checkItemValidty(int.Parse(dgvItems.CurrentRow.Cells["itemId"].Value.ToString())))
                        row.Cells["qty"].Value = 0;
                }
                chkRowValidty();
                CalculateTotal();
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
                  FROM tbl_items_boq_details 
                  WHERE code = @code AND  warehouse_id = @w",
                    DBClass.CreateParameter("code", dgvItems.CurrentRow.Cells["code"].Value.ToString()),
                    DBClass.CreateParameter("w", cmbWarehouse.SelectedValue.ToString()));
            }
            else if (type == "combo" && comboBox.SelectedValue != null)
            {
                string selectedItemCode = comboBox.SelectedValue.ToString();
                reader = DBClass.ExecuteReader(@"SELECT tbl_items_boq_details.id,method,type,code,sales_price,unit_id 
                  FROM tbl_items_boq_details 
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
            if (dgvItems.Rows.Count > 1 && dgvItems.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            {
                dgvItems.Rows.Remove(dgvItems.CurrentRow);
                CalculateTotal();
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
                cmbProject.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtProjectCode_Leave(object sender, EventArgs e)
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_projects where code =@code",
           DBClass.CreateParameter("code", txtProjectCode.Text));
            if (!reader.Read())
                cmbProject.SelectedIndex = -1;
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

        private void CalculateSRAndUpdateDataGridViewNew()
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
                        row.Cells["SR"].Value = mainCount.ToString() + ".0"; // Assign SR number for main items
                    }
                    else if (type == "11 - Inventory Part")
                    {
                        int lastMainItem = mainCount;
                        if (!subCountByMain.ContainsKey(lastMainItem))
                        {
                            subCountByMain[lastMainItem] = 0;
                        }
                        subCountByMain[lastMainItem]++;

                        string subSR = lastMainItem + "." + subCountByMain[lastMainItem];
                        row.Cells["SR"].Value = subSR; // Assign SR number for sub-items
                    }
                }
            }
        }

        private void AddItemToDataGridView(string itemType, string sr)
        {
            if (dgvItems.CurrentRow.Cells["sr"].Value == null || dgvItems.CurrentRow.Cells["sr"].Value.ToString() == "")
            {
                // Add a new row to the DataGridView with the specified ItemType and SR 
                dgvItems.CurrentRow.Cells["sr"].Value = sr;
                dgvItems.CurrentRow.Cells["type"].Value = itemType;
            } else
            {
                //if(dgvItems.CurrentRow.Cells["sr"].Value != null || dgvItems.CurrentRow.Cells["sr"].Value.ToString() != "") {
                //    dgvItems.CurrentRow.Cells["sr"].Value = sr;
                //    dgvItems.CurrentRow.Cells["type"].Value = itemType;
                //}
            }
        }

        private void btn_import_excel_Click(object sender, EventArgs e)
        {
            
                // Show dialog and pass headers
                var columnSelectForm = new frmBoqImport();
            if (columnSelectForm.ShowDialog() == DialogResult.OK)
            {
                DataTable table = columnSelectForm.dataTable;
                dgvImportedItem.Columns.Clear();
                dgvImportedItem.DataSource = null;
                dgvImportedItem.DataSource = table;
                isExported = true;
                dgvItems.Visible = false;
                dgvImportedItem.Visible = true;
                foreach (DataGridViewColumn column in dgvImportedItem.Columns)
                {
                    if (column.HeaderText == "Description of work" || column.HeaderText == "Description" || column.HeaderText == "Description of items" || column.HeaderText == "Item")
                    {
                        column.Width = 550;
                    }
                    else
                    {
                        column.Width = 200;
                    }
                }
                foreach (DataGridViewRow row in dgvImportedItem.Rows)
                {
                    if (row.IsNewRow) continue;
                    
                    var srValue = row.Cells["Sr."].Value?.ToString().Trim();

                    if (!string.IsNullOrEmpty(srValue) && Regex.IsMatch(srValue, @"^[A-Za-z]+$"))
                    {
                        row.DefaultCellStyle.Font = new Font(dgvImportedItem.Font, FontStyle.Bold);
                    }
                }
            }

        }
        private void LoadExcelSheets(string filePath)
        {
            //if (!File.Exists(filePath))
            //{
            //    MessageBox.Show("The selected file does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            //// Load Excel using a library like EPPlus, ClosedXML, or Interop (assuming EPPlus here)
            //using (var package = new ExcelPackage(new FileInfo(filePath)))
            //{
            //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //    var worksheet = package.Workbook.Worksheets[0]; // assuming first worksheet

            //    // Read headers from first row (row 1)
            //    var headers = new Dictionary<string, string>(); // key: column letter, value: header text

            //    for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
            //    {
            //        string headerText = worksheet.Cells[1, col].Text;
            //        string colLetter = ExcelCellAddress.GetColumnLetter(1); // e.g., 1 -> A
            //        headers[colLetter] = headerText;
            //    }

            //    // Show dialog and pass headers
            //    var columnSelectForm = new frmBoqImport(headers);
            //    if (columnSelectForm.ShowDialog() == DialogResult.OK)
            //    {
            //        string snColumn = columnSelectForm.SelectedSnColumn;     // e.g., "A"
            //        string nameColumn = columnSelectForm.SelectedNameColumn; // e.g., "B"

            //        // Now you can use snColumn, nameColumn in your logic
            //        MessageBox.Show($"Selected SN Column: {snColumn}, Name Column: {nameColumn}");
            //    }
            //}
            //using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            //{
            //    dgvImportedItem.Columns.Clear();
            //    dgvImportedItem.DataSource = null;
            //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //    foreach (var sheet in package.Workbook.Worksheets)
            //    {
            //        if (sheet.Dimension != null)
            //        {
            //            DataTable dataTable = new DataTable(sheet.Name);

            //            for (int col = 1; col <= sheet.Dimension.End.Column; col++)
            //            {
            //                string columnName = sheet.Cells[1, col].Text.Trim();
            //                if (string.IsNullOrWhiteSpace(columnName)) columnName = "Column " + col;
            //                dataTable.Columns.Add(columnName);
            //            }

            //            for (int row = 2; row <= sheet.Dimension.End.Row; row++)
            //            {
            //                bool isRowEmpty = true;
            //                DataRow dataRow = dataTable.NewRow();

            //                for (int col = 1; col <= sheet.Dimension.End.Column; col++)
            //                {
            //                    string cellValue = sheet.Cells[row, col].Text.Trim();

            //                    if (!string.IsNullOrWhiteSpace(cellValue))
            //                        isRowEmpty = false;

            //                    dataRow[col - 1] = cellValue;
            //                }

            //                if (!isRowEmpty)
            //                    dataTable.Rows.Add(dataRow);
            //            }

            //            RemoveEmptyColumns(dataTable);

            //            //DataColumn checkBoxColumn = new DataColumn("Select", typeof(bool));
            //            //dataTable.Columns.Add(checkBoxColumn);

            //            dgvImportedItem.DataSource = dataTable;
            //            isExported = true;
            //            dgvItems.Visible = false;
            //            dgvImportedItem.Visible = true;
            //            //dgvImportedItem.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //            //dgvImportedItem.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.Fill);

            //            //dgvImportedItem.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            //            //dgvImportedItem.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //            // Optionally, resize rows as well
            //            //dgvImportedItem.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

            //            //dgvAttendance.Columns["Select"].ReadOnly = false;
            //            //dgvAttendance.Columns["Select"].Width = 50;

            //            //dgvAttendance.Columns["Select"].HeaderText = " ";
            //            //dgvAttendance.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //            //AddCheckBoxHeader();
            //            //cmbSheet.Items.Add(sheet.Name);
            //            //if (dgvAttendance.Rows.Count == 0)
            //            //    return;
            //            //pnlData.Visible = true;
            //            //lblCode.Text = dgvAttendance.Rows[0].Cells[0].Value.ToString();
            //            //lblName.Text = dgvAttendance.Rows[0].Cells[1].Value.ToString();
            //            foreach (DataGridViewColumn column in dgvImportedItem.Columns)
            //            {
            //                if (column.HeaderText == "Description of work"|| column.HeaderText == "Description"|| column.HeaderText == "Description of items"|| column.HeaderText == "Item")
            //                {
            //                    column.Width = 550;
            //                }
            //                else
            //                {
            //                    column.Width = 200;
            //                }
            //            }

            //        }
            //    }

            //    //if (cmbSheet.Items.Count > 0)
            //    //    cmbSheet.SelectedIndex = 0;
            //    //else
            //    //    MessageBox.Show("No sheets found in the Excel file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
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

        private void dgvImportedItem_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (selectedRow.Cells["Sr."].Value == null)
            //{
            //    selectedRow.Cells["Sr."].Value = "1";
            //}
            //if (cmbProject.SelectedValue!=null)
            //{
            //    OpenAssemblys();
            //}
            //else
            //{
            //    selectProjectFirst();
            //}
        }
        private void selectProjectFirst()
        {
            MessageBox.Show("Select Project first");
        }

        private void dgvImportedItem_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //selectedRow = dgvImportedItem.CurrentRow;
            //if (dgvImportedItem.CurrentCell.ColumnIndex == dgvImportedItem.Columns["Description of work"].Index)
            //{
            //    if (selectedRow.Cells["Description of work"].Value !=null && selectedRow.Cells["Description of work"].Value.ToString() == "")
            //    {
            //        if (selectedRow.Cells["Sr."].Value == null)
            //        {
            //            selectedRow.Cells["Sr."].Value = "0";
            //        }
            //        if (cmbProject.SelectedValue != null)
            //        {
            //            OpenAssemblys();
            //        }
            //        else
            //        {
            //            selectProjectFirst();
            //        }
            //    } else
            //    {
            //        if(selectedRow.Cells["Sr."].Value == null)
            //        {
            //            selectedRow.Cells["Sr."].Value = "1";
            //        }
            //    }
            //}
        }
        private void OpenAssemblysOld()
        {
            string srId = "1.0", description="";
            int itemId = 0;
            if (dgvImportedItem.Rows.Count > 1)
            {
                int rowIndex = selectedRow.Index;
                itemId = 0;// int.Parse(dgvImportedItem.Rows[rowIndex].Cells[0].Value.ToString());
                srId = dgvImportedItem.Rows[rowIndex].Cells["Sr."].Value.ToString();
                description = dgvImportedItem.Rows[rowIndex].Cells["Description of work"].Value==null?"":dgvImportedItem.Rows[rowIndex].Cells["Description of work"].Value.ToString();
                itemTyped = "";// dgvImportedItem.Rows[rowIndex].Cells["type"].Value.ToString();
                selectedItemId = itemId.ToString();
            }
            //if (itemTyped == "13 - Inventory Assembly")
            //{
                comboCount++;
                frmAssemblyItem assemblyForm = new frmAssemblyItem(itemId, srId, int.Parse(txtProjectCode.Text.ToString()),description);
                assemblyForm.FormClosedWithResult += AssemblyForm_FormClosedWithResult;
                assemblyForm.ShowDialog();

            //DataTable dt = DBClass.ExecuteDataTable("select code,name from tbl_items_boq_details where warehouse_id=@id and state = 0 and active = 0",
            //                                DBClass.CreateParameter("id", cmbWarehouse.SelectedValue.ToString()));
            //DataGridViewComboBoxColumn name = (DataGridViewComboBoxColumn)dgvImportedItem.Columns["name"];
            //DataRow newRow = dt.NewRow();
            //newRow["code"] = 0;
            //newRow["name"] = "<< Add Assembly Item >>";
            //dt.Rows.InsertAt(newRow, 0);
            //name.DataSource = dt;
            //name.DisplayMember = "name";
            //name.ValueMember = "code";
            //comboBox.SelectedIndex = comboBox.Items.Count - 1;
            //}
            //else
            //{
            //    MessageBox.Show("Can't add Assembly here because the previous one is not main");
            //}
        }

        private void dgvImportedItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvImportedItem.Rows.Count > 1 && dgvImportedItem.CurrentRow.Cells["delete"].ColumnIndex == e.ColumnIndex)
            //{
            //    dgvImportedItem.Rows.Remove(dgvImportedItem.CurrentRow);
            //    CalculateTotal();
            //}
        }

        private void dgvImportedItem_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvImportedItem.Rows.Count > 1)
            //{
            //    DataGridViewRow row = dgvImportedItem.Rows[e.RowIndex];
                //decimal price = GetDecimalValue(row, "rate");
                //decimal qty = GetDecimalValue(row, "qty");
                //if (e.ColumnIndex == dgvImportedItem.Columns["Description of work"].Index)
                //{
                //    if (e.RowIndex >= 0 && e.ColumnIndex == dgvImportedItem.Columns["Description of work"].Index)
                //    {
                //        if (row.Cells["Description of work"].Value != null && !string.IsNullOrEmpty(row.Cells["Description of work"].Value.ToString()))
                //        {
                //            row.Cells["Sr."].Value = (e.RowIndex + 1).ToString();
                //        }
                //        else
                //        {
                //            row.Cells["Sr."].Value = null;
                //        }
                //    }
                //}
                    //if (e.ColumnIndex == dgvImportedItem.Columns["Code"].Index)
                    //{
                    //    string codeValue = row.Cells["Code"].Value?.ToString();
                    //    DataGridViewComboBoxCell comboCell = row.Cells["name"] as DataGridViewComboBoxCell;
                    //    if (comboCell != null)
                    //        insertItemThroughCodeOrCombo("code", comboCell, null);
                    //}
                    //else 
                    //if (e.ColumnIndex == dgvImportedItem.Columns["qty"].Index)
                    //{
                    //    if (dgvImportedItem.CurrentRow.Cells["itemId"].Value == null || !checkItemValidty(int.Parse(dgvImportedItem.CurrentRow.Cells["itemId"].Value.ToString())))
                    //        row.Cells["qty"].Value = 0;
                    //}
                    //chkRowValidty();
            //    CalculateTotal();
            //}
        }

        private void dgvImportedItem_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string columnName = dgvImportedItem.Columns[e.ColumnIndex].Name;

            if (columnName.ToLower() == "rate" || columnName.ToLower() == "amount" || columnName.ToLower() == "qty" || columnName.ToLower() == "length" || columnName.ToLower() == "width")
            {
                string value = e.FormattedValue.ToString().Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    decimal _;
                    // Try parsing the value as decimal
                    if (!decimal.TryParse(value, out _))
                    {
                        e.Cancel = true; // Prevent the cell from losing focus
                        dgvImportedItem.Rows[e.RowIndex].ErrorText = "Only decimal numbers are allowed.";
                        MessageBox.Show("Please enter a valid decimal number.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnAddProject_Click(object sender, EventArgs e)
        {
            frmAddProject frm = new frmAddProject(this, 0);
            frm.ShowDialog();
        }

        private void btnAddTender_Click(object sender, EventArgs e)
        {
            frmAddTenderName frm = new frmAddTenderName(this, 0);
            frm.ShowDialog();
        }

        //private void CalculateSRAndUpdateDataGridView(string itemType)
        //{
        //    int mainCount = 0;
        //    Dictionary<int, int> subCountByMain = new Dictionary<int, int>();

        //    foreach (DataGridViewRow row in dgvItems.Rows)
        //    {
        //        if (row.Cells["type"].Value != null)
        //        {
        //            string type = row.Cells["type"].Value.ToString();
        //            if (type == "13 - Inventory Assembly")
        //            {
        //                mainCount++;
        //            }
        //            else if (type == "11 - Inventory Part")
        //            {
        //                int lastMainItem = mainCount;
        //                if (!subCountByMain.ContainsKey(lastMainItem))
        //                {
        //                    subCountByMain[lastMainItem] = 0;
        //                }
        //                subCountByMain[lastMainItem]++;
        //            }
        //        }
        //    }

        //    if (itemType == "13 - Inventory Assembly")
        //    {
        //        double nextMainSR = mainCount + 1;
        //        string mainSR = nextMainSR.ToString("0.0");
        //        AddItemToDataGridView("13 - Inventory Assembly", mainSR);
        //    }
        //    else if (itemType == "11 - Inventory Part")
        //    {
        //        int lastMainItem = mainCount;
        //        int subCount = subCountByMain.ContainsKey(lastMainItem) ? subCountByMain[lastMainItem] : 0;
        //        double nextSubSR = subCount + 1;
        //        string subSR = lastMainItem + "." + nextSubSR.ToString("0");
        //        AddItemToDataGridView("11 - Inventory Part", subSR);
        //    }
        //}

        //private void AddItemToDataGridView(string itemType, string sr)
        //{
        //    if (dgvItems.CurrentRow.Cells["sr"].Value == null || dgvItems.CurrentRow.Cells["sr"].Value.ToString() == "")
        //    {
        //        dgvItems.CurrentRow.Cells["sr"].Value = sr;
        //        dgvItems.CurrentRow.Cells["type"].Value = itemType;
        //    }
        //}

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
        
    }
}
