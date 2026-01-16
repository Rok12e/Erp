using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmIssueMaterial : Form
    {
        decimal invId;
        private frmViewProjectPlanning master;
        int id, planningId, tenderId;
        List<string> assignedTeam = new List<string>();
        bool isExported = false;
        public frmIssueMaterial(frmViewProjectPlanning _master, int _id,int _tenderId,int _planningId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = _id;
            this.planningId = _planningId;
            this.tenderId = _tenderId;
            if (id != 0)
                this.Text = "Issue Material - Edit";
            else
                this.Text = "Issue Material - New";
            headerUC1.FormText = this.Text;
        }
        private void frmIssueMaterial_Load(object sender, EventArgs e)
        {
            dtInv.Value = DateTime.Now.Date;
            LoaddgvItems();

            if (id != 0)
                BindData();
        }
        private void LoaddgvItems()
        {
            //string query = @"SELECT CONCAT(tbl_items_boq.sr ,' - ' , tbl_items_boq.name) as name ,tbl_items_boq.id FROM tbl_project_tender_details 
            //                    INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.sr=tbl_items_boq.sr
            //                    WHERE tbl_project_tender_details.tender_id=@tenderId";
            string query = @"SELECT CONCAT(boq.sr ,' - ' , boq.name) as name ,boq.id,rdm.RequestedQty qty,rdm.unit unit_name FROM tbl_project_material_requests rdm, tbl_project_planning plan,tbl_project_tender tender, tbl_items_boq boq WHERE rdm.planning_id = plan.id and tender.id=plan.tender_id AND rdm.itemId=boq.id and plan.id=@planId";
            dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable(query,DBClass.CreateParameter("planId", planningId));
            DataGridViewComboBoxColumn cmbItemName = (DataGridViewComboBoxColumn)dgvItems.Columns["name"];
            cmbItemName.DataSource = dt;
            cmbItemName.DisplayMember = "name";
            cmbItemName.ValueMember = "id";
        }
        private void BindData()
        {
            MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_material_requests where planning_id = @id and IssuedQty > 0",
                DBClass.CreateParameter("id", id));
            if (reader.Read())
            {
                invId = id;
                dtInv.Value = DateTime.Parse(reader["IssuedDate"].ToString());
                
                //cmbFundPeriod.Text = reader["fund_period"].ToString();
                //txtDescription.Text = reader["description"].ToString();
                //txtAssignedTeam.Text = reader["assigned_team"].ToString();
                //txtProgress.Text = reader["progress"].ToString();
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
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT 
                                                                    CONCAT(boq.sr, ' - ', boq.name) AS name,
                                                                    boq.id,
                                                                    rm.IssuedQty qty,
                                                                    rm.unit unit_name
                                                                FROM tbl_project_material_requests rm
                                                                INNER JOIN tbl_items_boq boq 
                                                                    ON rm.tender_id = boq.ref_id AND rm.itemId = boq.id
                                                                WHERE 
                                                                    rm.tender_id = @tenderId 
                                                                    AND rm.IssuedQty > 0 
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
            //if (id == 0)
            //{
            //    if (insertData())
            //        this.Close();
            //}
            //else
            //{
                if (updateData())
                    this.Close();
            //}
            master.BindIssuedDate();
        }

        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            for (int i = 0; i < dgvItems.Rows.Count - 1; i++)
            {
                if (dgvItems.Rows[i].Cells["name"].Value != null)
                {
                    
                        DBClass.ExecuteNonQuery(@"UPDATE tbl_project_material_requests 
                                    SET IssuedDate = @date, IssuedQty=@qty WHERE planning_id =@planningId and tender_id=@tenderId and itemId=@itemId",
                        DBClass.CreateParameter("itemId", dgvItems.Rows[i].Cells["name"].Value.ToString()),
                        DBClass.CreateParameter("planningId", planningId),
                        DBClass.CreateParameter("tenderId", tenderId),
                        DBClass.CreateParameter("date", dtInv.Value.Date),
                        DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
                    Utilities.LogAudit(frmLogin.userId, "Update Project Issue Material", "tbl_project_material_requests", planningId, "Updated Item: " + dgvItems.Rows[i].Cells["name"].Value.ToString() + " Qty: " + dgvItems.Rows[i].Cells["qty"].Value.ToString());
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
                //if (dgvItems.Rows[i].Cells["name"].Value !=null) {
                //    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_issue_material(ref_id, date, itemId, unit,qty) VALUES (@refId,@date, @itemId,@unit, @qty)",
                //    DBClass.CreateParameter("refId", planningId),
                //    DBClass.CreateParameter("date", dtInv.Value.Date),
                //    DBClass.CreateParameter("itemId", dgvItems.Rows[i].Cells["name"].Value.ToString()),
                //    DBClass.CreateParameter("unit", dgvItems.Rows[i].Cells["unit"].Value == null ? "" : dgvItems.Rows[i].Cells["unit"].Value.ToString()),
                //    DBClass.CreateParameter("qty", dgvItems.Rows[i].Cells["qty"].Value.ToString()));
                //}
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

            if (type == "combo")
            {
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
