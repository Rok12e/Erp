using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewProjectWorkDone : Form
    {
        private MasterProjectWorkDone master;
        int id, planningId=0;
        int mainItemId = 0;

        public frmViewProjectWorkDone(MasterProjectWorkDone _master, int _id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = _id;
            if (id != 0)
                this.Text = "Project - Edit WorkDone";
            else
                this.Text = "Project - New WorkDone";
            Lbheader.Text = this.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmViewProjectWorkDone_Load(object sender, EventArgs e)
        {
            dtpt.Value = DateTime.Now.Date;
            BindCombos.PopulateWarehouse(cmbWarehouse);
            BindCombo();
            if (id != 0)
                BindData();
        }

        private void BindData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select wd.id,wd.date,wd.planning_id,wd.warehouse_id,wd.account_id,p.project_id,p.site,p.tender_id,p.tender_name_id from tbl_project_work_done wd,tbl_project_planning p WHERE p.id=wd.planning_id AND wd.id = @id",
                DBClass.CreateParameter("id", id)))
            {
                if (reader.Read())
                {
                    dtpt.Value = DateTime.Parse(reader["date"].ToString());
                    planningId = int.Parse(reader["planning_id"].ToString());
                    cmbAccountName.SelectedValue = reader["account_id"].ToString();
                    cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                    cmbProject.SelectedValue = reader["project_id"].ToString();
                    cmbProjectSite.SelectedValue = reader["site"].ToString();
                    cmbTenderName.SelectedValue = reader["tender_name_id"].ToString();
                    var tenderId = reader["tender_id"].ToString();

                    BindItems();
                    CalculateTotal();
                }
            }
        }

        private void BindItems()
        {
            using (MySqlDataReader dr = DBClass.ExecuteReader(@"SELECT id,item_id,main_id,code,qty_total,unit,qty_used FROM tbl_project_work_done_details WHERE ref_id=@id",
                DBClass.CreateParameter("id", id)))
            {
                while (dr.Read())
                {
                    string _id = dr["id"].ToString();
                    string _itemId = dr["item_id"].ToString();
                    string _mainId = dr["main_id"].ToString();
                    string _code = dr["code"].ToString();
                    string _qtyTotal = dr["qty_total"].ToString();
                    string _unit = dr["unit"].ToString();
                    string _qtyUsed = dr["qty_used"].ToString();

                    //MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT on_hand qty,name,ref_id from tbl_items_boq_details where id = @itemId and ref_id=@srId",
                    ////DBClass.CreateParameter("planningId", planningId),
                    //DBClass.CreateParameter("srId", _mainId.ToString()),
                    //DBClass.CreateParameter("itemId", _itemId.ToString()));
                    //dgvItemsAssembly.Rows.Clear();
                    //while (reader.Read())
                    //{
                    //    dgvItemsAssembly.Rows.Add(_id, _code, reader["name"].ToString(), reader["qty"].ToString(), _unit, _qtyUsed);

                    //}
                    //reader.Close();
                }
                loadDataItems();
                foreach (DataGridViewRow row in dgvPlanning.Rows)
                {
                    if (row.Cells["planning_id"].Value != null && row.Cells["planning_id"].Value.ToString() == planningId.ToString())
                    {
                        row.Selected = true;
                        dgvPlanning.CurrentCell = row.Cells[0]; // Set focus to first cell of that row (optional)
                        dgvPlanning.FirstDisplayedScrollingRowIndex = row.Index; // Scroll to the row (optional)
                        break;
                    }
                }
                loadBOQItems();
                addDataToAssembly();
            }
        }
        public void BindCombo()
        {
            BindCombos.PopulateProjects(cmbProject);
            BindCombos.PopulateAllLevel4Account(cmbAccountName);
            BindCombos.PopulateTenderNames(cmbTenderName);
            BindCombos.PopulateWarehouse(cmbWarehouse);
            BindCombos.populateSites(cmbProjectSite);
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
                if (updateData())
                    this.Close();
            }
        }
        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_work_done 
                                    SET modified_by = @modifiedBy, modified_date = @modifiedDate ,date = @date, planning_id = @planningId, 
                                    warehouse_id = @warehouse_id,account_id = @account_id WHERE id = @id;",
                 DBClass.CreateParameter("id", id),
                  DBClass.CreateParameter("date", dtpt.Value.Date),
               DBClass.CreateParameter("planningId", planningId),
               DBClass.CreateParameter("warehouse_id", cmbWarehouse.SelectedValue),
               DBClass.CreateParameter("account_id", cmbAccountName.SelectedValue),
            DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
                DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));
            DBClass.ExecuteNonQuery(@"DELETE FROM tbl_project_work_done_details WHERE ref_id=@id", DBClass.CreateParameter("id", id.ToString()));
            insertGridItems();
            Utilities.LogAudit(frmLogin.userId, "Update Project Work Done", "Project Work Done", id, "Updated Project Work Done: " + cmbProject.SelectedValue.ToString() + " - " + cmbTenderName.SelectedValue.ToString());

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            id = int.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_project_work_done 
                            (date, planning_id,account_id,warehouse_id, created_by, created_date,state)
                            VALUES 
                            (@date,@planningId,@accountId,@warehouseId, @createdBy, @createdDate,0);
                            SELECT LAST_INSERT_ID();",
            DBClass.CreateParameter("date", dtpt.Value.Date),
            DBClass.CreateParameter("planningId", planningId),
            DBClass.CreateParameter("accountId", cmbAccountName.SelectedValue),
            DBClass.CreateParameter("warehouseId", cmbWarehouse.SelectedValue),
            DBClass.CreateParameter("createdBy", frmLogin.userId),
            DBClass.CreateParameter("createdDate", DateTime.Now.Date)).ToString());

            insertGridItems();

            Utilities.LogAudit(frmLogin.userId, "Insert Project Work Done", "Project Work Done", id, "Inserted Project Work Done: " + cmbProject.SelectedValue.ToString() + " - " + cmbTenderName.SelectedValue.ToString());

            return true;
        }
        private void insertGridItems()
        {
            if (!string.IsNullOrEmpty(txtItemId.Text) && int.Parse(txtItemId.Text) > 0)
            {
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_work_done_details(ref_id,item_id,main_id,code,qty_total,unit,qty_used)
                                            VALUES(@id,@itemId,@mainItemId,@code, @qtyTotal,@unit,@qtyUsed)",
                      DBClass.CreateParameter("@id", id.ToString()),
                      DBClass.CreateParameter("@itemId", txtItemId.Text),
                      DBClass.CreateParameter("@mainItemId", mainItemId),
                      DBClass.CreateParameter("@code", txtItemCode.Text.ToString()),
                      DBClass.CreateParameter("@qtyTotal", txtReceivedQty.Text.ToString()),
                      DBClass.CreateParameter("@unit", txtUnit.Text.ToString()),
                      DBClass.CreateParameter("@qtyUsed", txtUsedQty.Text.ToString()));

                Utilities.LogAudit(frmLogin.userId, "Insert Project Work Done Item", "Project Work Done Item", id, "Inserted Project Work Done Item: " + txtItemCode.Text + " - " + txtDescription.Text);
            }
        }
        private bool chkRequiredDate()
        {
            if (string.IsNullOrEmpty(txtItemId.Text))
            {
                MessageBox.Show("Project must be selected at least one activity item.");
                return false;
            }
            if (string.IsNullOrEmpty(txtUsedQty.Text))
            {
                MessageBox.Show("Project must be selected at least one activity item.");
                return false;
            }
            else if (decimal.Parse(txtUsedQty.Text) <= 0)
            {
                MessageBox.Show("Used Qty can't be zero.");
                return false;
            }
            if (decimal.Parse(txtReceivedQty.Text) <= 0)
            {
                MessageBox.Show("Item not received yet.");
                return false;
            }
            if (cmbProject.SelectedValue == null)
            {
                MessageBox.Show("Project Must be Selected.");
                cmbProject.Focus();
                return false;
            }
            if (cmbTenderName.SelectedValue == null)
            {
                MessageBox.Show("tender Must be Selected.");
                cmbTenderName.Focus();
                return false;
            }
            if (cmbAccountName.SelectedValue == null)
            {
                MessageBox.Show("AccountName Must be Selected.");
                cmbAccountName.Focus();
                return false;
            }
            if (cmbProjectSite.SelectedValue == null)
            {
                MessageBox.Show("Project Site Must be Selected.");
                cmbProjectSite.Focus();
                return false;
            }
            if (cmbWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Warehouse Must be Selected.");
                cmbWarehouse.Focus();
                return false;
            }
            if (dgvItems.Rows.Count == 1)
            {
                MessageBox.Show("Insert Items First.");
                return false;
            }

            return true;
        }
        private void resetTextBox()
        {

            id = 0;
            dtpt.Value = DateTime.Now;
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
                if (updateData())
                    resetTextBox();
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
            
                //decimal amount = 0;
                //foreach (DataGridViewRow row in dgvItems.Rows)
                //{
                //    if (row.Cells["amount"].Value != null)
                //        amount += Convert.ToDecimal(row.Cells["amount"].Value);
                //}
            
        }
        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItems.Rows.Count > 1)
            {
                //var row = dgvItems.Rows[e.RowIndex];
                //decimal price = GetDecimalValue(row, "rate");
                //decimal qty = GetDecimalValue(row, "qty");
                //if (e.ColumnIndex == dgvItems.Columns["qty"].Index)
                //{
                //    if (dgvItems.CurrentRow.Cells["itemId"].Value == null || !checkItemValidty(int.Parse(dgvItems.CurrentRow.Cells["itemId"].Value.ToString())))
                //        row.Cells["qty"].Value = 0;
                //}
                //chkRowValidty();
                //CalculateTotal();
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

        private void dgvItems_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            dgvItems.Rows[e.RowIndex].Cells[1].Value = (e.RowIndex + 1).ToString();
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            // LoaddgvItems();
        }
        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProject.SelectedValue != null)
            {
                loadDataItems();
            }
        }

        private void loadDataItems()
        {
            string projectId = cmbProject.SelectedValue == null ? "0" : cmbProject.SelectedValue.ToString();
            string tenderNameId = cmbTenderName.SelectedValue == null ? "0" : cmbTenderName.SelectedValue.ToString();
            string siteId = cmbProjectSite.SelectedValue == null ? "0" : cmbProjectSite.SelectedValue.ToString();
            if (int.Parse(projectId)>0||int.Parse(tenderNameId)>0||int.Parse(siteId)>0) {
                string query = @"SELECT Id, Date FROM tbl_project_planning WHERE project_id = @project AND tender_name_id = @tenderNameId AND site = @siteId";
                using (MySqlDataReader reader = DBClass.ExecuteReader(query,
                    DBClass.CreateParameter("project", projectId),
                    DBClass.CreateParameter("tenderNameId", tenderNameId),
                    DBClass.CreateParameter("siteId", siteId)))
                {
                    dgvPlanning.Rows.Clear();
                    while (reader.Read())
                    {
                        DateTime date = Convert.ToDateTime(reader["Date"]);
                        string dated = date.ToString("dd/MM/yyyy");
                        dgvPlanning.Rows.Add(reader["Id"].ToString(), dated);
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

        private void cmbTenderName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbTenderName.SelectedValue != null)
            {
                loadDataItems();
            }
        }

        private void cmbProjectSite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProjectSite.SelectedValue != null)
            {
                loadDataItems();
            }
        }

        private void dgvPlanning_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get the clicked row and column index
                int rowIndex = e.RowIndex;
                int columnIndex = e.ColumnIndex;

                // Optionally, get the value of the clicked cell
                //var cellValue = dataGridView1.Rows[rowIndex].Cells[columnIndex].Value;

                // Display information or perform an action with the clicked cell data
                //MessageBox.Show($"You clicked on row {rowIndex}, column {columnIndex}. Cell value: {cellValue}");

                // For example, you can use the row index to get data from a specific row
                //var rowData = dataGridView1.Rows[rowIndex].Cells["ColumnName"].Value; // Use actual column name

                if (dgvPlanning.Rows.Count > 0)
                {
                    if (dgvPlanning.Rows[rowIndex].Cells["planning_id"].Value!=null)
                    {
                        planningId = int.Parse(dgvPlanning.Rows[rowIndex].Cells["planning_id"].Value.ToString());
                        loadBOQItems();
                    }
                }
            }
        }
        private void loadBOQItems()
        {
            string query = @"SELECT tbl_items_boq.id,tbl_project_tender_details.sr,tbl_project_tender_details.qty,tbl_project_tender_details.unit_id,tbl_project_tender_details.item_id, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                            INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.item_id = tbl_items_boq.id
                            WHERE tbl_project_tender_details.tender_id = (SELECT tender_id FROM tbl_project_planning WHERE id=@planningId)";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query,
            DBClass.CreateParameter("planningId", planningId)))
            {
                dgvItems.Rows.Clear();
                int count = 1;
                while (reader.Read())
                {
                    dgvItems.Rows.Add(reader["id"].ToString(), count, reader["sr"].ToString(), reader["name"].ToString(), reader["qty"].ToString(), reader["unit_name"].ToString());
                    count++;
                }
            }
        }
        private void dgvItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dgvItems.Rows.Count > 1)
                {
                    mainItemId = int.Parse(dgvItems.CurrentRow.Cells["itemId"].Value.ToString());
                    addDataToAssembly();
                }
            }
        }
        private void addDataToAssembly()
        {
            clearTextData();
            if (!string.IsNullOrEmpty(mainItemId.ToString()) || mainItemId != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"select ti.code,ti.id,ti.on_hand qty,ti.cost_price rate,ti.name,ti.ref_id,unit_id,(select unit_name from tbl_items_boq where id = ti.ref_id) unit_name from tbl_item_assembly_bos ta inner join tbl_items_boq_details ti on ta.item_id = ti.id and ti.ref_id=ta.assembly_id where ta.assembly_id =@itemId",
                        DBClass.CreateParameter("planningId", planningId),
                        //DBClass.CreateParameter("srId", mainItemId.ToString()),
                        DBClass.CreateParameter("itemId", mainItemId.ToString())))
                {
                    if (reader.Read())
                    {
                        txtItemId.Text = reader["id"].ToString();
                        txtItemSr.Text = reader["code"].ToString();
                        txtItemCode.Text = reader["ref_id"].ToString();
                        txtDescription.Text = reader["name"].ToString();
                        txtBoqQty.Text = reader["qty"].ToString();
                        txtUnit.Text = reader["unit_name"].ToString();

                        using (MySqlDataReader reader1 = DBClass.ExecuteReader(@"SELECT mr.RequestedDate,mr.IssuedDate,mr.ReceivedDate,mr.RequestedQty,mr.IssuedQty,mr.ReceivedQty FROM tbl_project_material_requests mr WHERE itemId = @itemId",
                            DBClass.CreateParameter("itemId", reader["ref_id"].ToString())))
                            if (reader1.Read())
                            {
                                txtReceivedQty.Text = reader1["ReceivedQty"].ToString();
                                txtIssuedQty.Text = reader1["IssuedQty"].ToString();
                                txtRequestedQty.Text = reader1["RequestedQty"].ToString();
                                txtUsedQty.Text = reader1["ReceivedQty"].ToString();
                            }
                    }
                }
            }
        }
        private void clearTextData()
        {
            txtReceivedQty.Text = string.Empty;
            txtIssuedQty.Text = string.Empty;
            txtRequestedQty.Text = string.Empty;
            txtUsedQty.Text = string.Empty;
            txtItemId.Text = string.Empty;
            txtItemSr.Text = string.Empty;
            txtItemCode.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtBoqQty.Text = string.Empty;
            txtUnit.Text = string.Empty;
        }
    }
}
