using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmRequestMaterial : Form
    {
        decimal invId;
        private frmViewProjectPlanning master;
        int id, planningId, tenderId;
        List<string> assignedTeam = new List<string>();
        bool isExported = false;
        public frmRequestMaterial(frmViewProjectPlanning _master, int _id,int _tenderId,int _planningId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = _id;
            this.planningId = _planningId;
            this.tenderId = _tenderId;
            if (id != 0)
                this.Text = "RequestMaterial - Edit";
            else
                this.Text = "RequestMaterial - New";
            headerUC1.FormText = this.Text;
        }
        private void frmRequestMaterial_Load(object sender, EventArgs e)
        {
            dtInv.Value = DateTime.Now.Date;
            LoaddgvItems();

            if (id != 0)
                BindData();
        }
        private void LoaddgvItems()
        {
            string query = @"SELECT CONCAT(tbl_items_boq.sr ,' - ' , tbl_items_boq.name) as name ,tbl_items_boq.id,tbl_items_boq.qty,tbl_items_boq.unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.item_id=tbl_items_boq.id
                                WHERE tbl_project_tender_details.tender_id=@tenderId";
            dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable(query,DBClass.CreateParameter("tenderId", tenderId));
            DataGridViewComboBoxColumn cmbItemName = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            cmbItemName.DataSource = dt;
            cmbItemName.DisplayMember = "name";
            cmbItemName.ValueMember = "id";
        }
        private void BindData()
        {
            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_project_material_requests where planning_id = @id", // select * from tbl_project_planning where id = @id",
                DBClass.CreateParameter("id", id));
            if (reader.Read())
            {
                invId = id;
                dtInv.Value = DateTime.Parse(reader["RequestedDate"].ToString());
                planningId = int.Parse(reader["planning_id"].ToString());
                tenderId = int.Parse(reader["tender_id"].ToString());

                string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
                int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", tenderId)));
                isExported = count > 0 ? true : false;
                //if (isExported)
                //{
                //    resetGridView();
                //}
                BindItems();
            }
        }
        private void BindItems()
        {
            dgvItems.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                                                                    CONCAT(boq.sr, ' - ', boq.name) AS name,
                                                                    boq.id,
                                                                    rm.RequestedQty qty,
                                                                    rm.unit unit_name
                                                                FROM tbl_project_material_requests rm
                                                                INNER JOIN tbl_items_boq boq 
                                                                    ON rm.tender_id = boq.ref_id AND rm.itemId = boq.id
                                                                WHERE 
                                                                    rm.tender_id = @tenderId
                                                                    AND rm.planning_id = @planId",
                                                            DBClass.CreateParameter("tenderId", tenderId),
                                                            DBClass.CreateParameter("planId", planningId)))
            {
                int count = 1;
                while (reader.Read())
                {
                    int rowIndex = dgvItems.Rows.Add(); 
                    DataGridViewRow row = dgvItems.Rows[rowIndex];
                    row.Cells[0].Value = count;
                    row.Cells[1].Value = Convert.ToInt32(reader["id"]);
                    row.Cells[2].Value = decimal.Parse(reader["qty"].ToString()).ToString("F2");
                    row.Cells[3].Value = reader["unit_name"].ToString();

                    count++;
                }
            }
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
            master.BindRequestedDate();
        }

        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["name"].Value != null)
                {
                    object objId = DBClass.ExecuteScalar(@"SELECT id FROM tbl_project_material_requests WHERE planning_id =@id and tender_id=@tenderId and itemId=@itemId",
                                    DBClass.CreateParameter("@id", planningId),
                                    DBClass.CreateParameter("itemId", dgvItems.Rows[i].Cells["name"].Value.ToString()), DBClass.CreateParameter("tenderId", tenderId));
                    int idIn = (objId != null && objId != DBNull.Value) ? int.Parse(objId.ToString()) : 0;
                    if (idIn > 0)
                    {
                        DBClass.ExecuteNonQuery(@"UPDATE tbl_project_material_requests 
                                    SET RequestedDate = @date, itemId=@itemId, unit=@unit,RequestedQty=@qty WHERE id=@id;",
                        DBClass.CreateParameter("id", idIn),
                        DBClass.CreateParameter("date", dtInv.Value.Date),
                        DBClass.CreateParameter("itemId", dgvItems.Rows[i].Cells["name"].Value.ToString()),
                        DBClass.CreateParameter("unit", dgvItems.Rows[i].Cells["unit"].Value.ToString()),
                        DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
                        Utilities.LogAudit(frmLogin.userId, "Update Project Material Request", "Project Material Request", idIn, "Updated Project Material Request: " + dgvItems.Rows[i].Cells["name"].Value.ToString() + " with Qty: " + dgvItems.Rows[i].Cells["qty"].Value.ToString());
                    }
                    else
                    {
                        DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_material_requests(tender_id,planning_id, RequestedDate, itemId, unit,RequestedQty,IssuedQty,ReceivedQty) VALUES (@tenderId,@refId,@date, @itemId,@unit, @qty,0,0)",
                        DBClass.CreateParameter("tenderId", tenderId),
                        DBClass.CreateParameter("refId", planningId),
                        DBClass.CreateParameter("date", dtInv.Value.Date),
                        DBClass.CreateParameter("itemId", dgvItems.Rows[i].Cells["name"].Value.ToString()),
                        DBClass.CreateParameter("unit", dgvItems.Rows[i].Cells["unit"].Value != null ? dgvItems.Rows[i].Cells["unit"].Value.ToString() : ""),
                        DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
                        Utilities.LogAudit(frmLogin.userId, "Insert Project Material Request", "Project Material Request", idIn, "Inserted Project Material Request: " + dgvItems.Rows[i].Cells["name"].Value.ToString() + " with Qty: " + dgvItems.Rows[i].Cells["qty"].Value.ToString());
                    }
                }
            }

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["name"].Value !=null) {
                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_material_requests(tender_id,planning_id, RequestedDate, itemId, unit,RequestedQty,IssuedQty,ReceivedQty) VALUES (@tenderId,@refId,@date, @itemId,@unit, @qty,0,0)",
                    DBClass.CreateParameter("tenderId", tenderId),
                    DBClass.CreateParameter("refId", planningId),
                    DBClass.CreateParameter("date", dtInv.Value.Date),
                    DBClass.CreateParameter("itemId", dgvItems.Rows[i].Cells["name"].Value.ToString()),
                    DBClass.CreateParameter("unit", dgvItems.Rows[i].Cells["unit"].Value!=null ? dgvItems.Rows[i].Cells["unit"].Value.ToString() : ""),
                    DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));

                    Utilities.LogAudit(frmLogin.userId, "Insert Project Material Request", "Project Material Request", 0, "Inserted Project Material Request: " + dgvItems.Rows[i].Cells["name"].Value.ToString() + " with Qty: " + dgvItems.Rows[i].Cells["qty"].Value.ToString());
                }
            }

            return true;
        }
        private bool chkRequiredDate()
        {
            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["qty"].Value == null
                    || dgvItems.Rows[i].Cells["qty"].Value.ToString() == ""
                    || decimal.Parse(dgvItems.Rows[i].Cells["qty"].Value.ToString()) == 0)
                {
                    MessageBox.Show("Total Item In Row " + (dgvItems.Rows[i].Index + 1) + " Can't Be 0 or Null");
                    return false;
                }
            }
            return true;
        }
        private void resetTextBox()
        {
            id = 0;
            dtInv.Value = DateTime.Now;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
        }

        private void dgvItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ToString();
        }

        private void ComboBoxName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null && comboBox.SelectedValue != null)
            {
                insertItemThroughCodeOrCombo("combo", null, comboBox);
            }
        }
        private void insertItemThroughCodeOrCombo(string type, DataGridViewComboBoxCell comboCell, ComboBox comboBox)
        {
            var currentRow = dgvItems.CurrentRow;
            if (currentRow == null || comboBox.SelectedValue == null)
                return;

            if (type == "combo") {
                var comboCell1 = currentRow.Cells["name"] as DataGridViewComboBoxCell;
                if (comboCell1 == null)
                    return;

                List<DataRowView> selectedItems = new List<DataRowView>();

                foreach (var item in comboCell1.Items)
                {
                    DataRowView dr = (DataRowView)item;
                    if (dr["id"].ToString() == comboBox.SelectedValue.ToString())
                    {
                        var _unit = dr["unit_name"].ToString();
                        var _qty = dr["qty"].ToString();
                        dgvItems.CurrentRow.Cells["qty"].Value = _qty;
                        dgvItems.CurrentRow.Cells["unit"].Value = _unit;
                        break;
                    }
                }
            }
        }
    }
}
