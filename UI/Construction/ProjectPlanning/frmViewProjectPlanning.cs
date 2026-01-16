using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewProjectPlanning : Form
    {
        decimal invId;
        private MasterProjectPlanning master;
        int id, tenderId, tenderNameId;
        List<string> assignedTeam = new List<string>();
        bool isExported = false;
        public frmViewProjectPlanning(MasterProjectPlanning _master, int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = id;
            if (id != 0)
                this.Text = "Planning - Edit";
            else
                this.Text = "Planning - New";
            headerUC1.FormText = this.Text;
        }
        int AccountCashId = 0;
        private void frmViewProjectPlanning_Load(object sender, EventArgs e)
        {
            dtInv.Value = dtp_start.Value = dtp_end.Value = DateTime.Now.Date;

            //BindCombos.PopulateCitieAllNormalComboBox(cmbProjectLocation);

            var defaultAccounts = BindCombos.LoadDefaultAccounts();
            AccountCashId = defaultAccounts.ContainsKey("Purchase Payment Cash Method")
                ? defaultAccounts["Purchase Payment Cash Method"] : 0;
            BindCombos.PopulateAllLevel4Account(cmbFundAccount);
            
            LoaddgvItems();

            BindCombo();

            if (id != 0)
            {
                tabControl1.Visible = true;
                BindData();
            } else
            {
                tabControl1.Visible = false;
            }
        }
        private void LoaddgvItems()
        {
            dgvItems.Rows.Clear();
            DataTable dt = DBClass.ExecuteDataTable("SELECT CONCAT(CODE ,' - ' , Name) as NAME ,id FROM tbl_employee");
            DataGridViewComboBoxColumn cmbItemName = (DataGridViewComboBoxColumn)dgvItems.Columns["assigned"];
            cmbItemName.DataSource = dt;
            cmbItemName.DisplayMember = "name";
            cmbItemName.ValueMember = "id";
        }
        public void BindCombo()
        {
            BindCombos.PopulateTenderedProjects(cmbProjectName);
            BindCombos.PopulateTenderNames(cmbTenderName);
            BindCombos.populateSites(cmbProjectSite);
        }
        private void BindData()
        {
            MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_planning where id = @id",
                DBClass.CreateParameter("id", id));
            if (reader.Read())
            {
                invId = id;
                dtInv.Value = DateTime.Parse(reader["date"].ToString());
                cmbProjectName.SelectedValue = int.Parse(reader["project_id"].ToString());
                cmbProjectSite.SelectedValue = int.Parse(reader["site"].ToString());
                cmbProjectStatus.Text = reader["status"].ToString();
                txtEstimatedBudget.Text = Utilities.FormatDecimal(decimal.Parse(reader["estimated_budget"].ToString()));
                cmbProjectType.Text = reader["project_type"].ToString();
                cmbFundAccount.SelectedValue = int.Parse(reader["fund_account_id"].ToString());
                //cmbFundPeriod.Text = reader["fund_period"].ToString();
                //txtDescription.Text = reader["description"].ToString();
                //txtAssignedTeam.Text = reader["assigned_team"].ToString();
                //txtProgress.Text = reader["progress"].ToString();
                tenderId = int.Parse(reader["tender_id"].ToString());
                cmbTenderName.SelectedValue = int.Parse(reader["tender_name_id"].ToString());

                string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
                int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", tenderId)));
                isExported = count > 0 ? true : false;
                //if (isExported)
                //{
                //    resetGridView();
                //}
                BindItemsLoad();
                BindRequestedDate();
                BindIssuedDate();
                BindReceivedDate();
                BindResourceDate();
            }
        }

        public void BindRequestedDate()
        {
            dgvRequest.Rows.Clear();
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT CONCAT(boq.sr, ' - ', boq.name) AS name, rm.id, rm.RequestedQty Qty, rm.RequestedDate Date, rm.unit,
                                                                CASE WHEN ReceivedQty > 0 THEN 'Received' WHEN IssuedQty > 0 THEN 'Issued' ELSE 'Requested' END AS status                
                                                                FROM tbl_project_material_requests rm
                                                                INNER JOIN tbl_items_boq boq ON rm.tender_id = boq.ref_id AND rm.itemId = boq.id
                                                                WHERE rm.planning_id = @plannigId", DBClass.CreateParameter("plannigId", id)))
            {
                int count = 1;
                while (reader.Read())
                {
                    string dated = "";
                    if (reader["Date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["Date"]);
                        dated = date.ToString("dd/MM/yyyy");
                    }
                    dgvRequest.Rows.Add(count, reader["id"].ToString(), id, dated, reader["name"].ToString(), reader["unit"].ToString(), decimal.Parse(reader["Qty"].ToString()).ToString("N2"), reader["status"].ToString());
                    count++;
                }
            }
        }

        public void BindIssuedDate()
        {
            dgvItemsIssued.Rows.Clear();
            MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT CONCAT(boq.sr, ' - ', boq.name) AS name, rm.id, rm.IssuedQty Qty, rm.IssuedDate Date, rm.unit,
                                                                CASE WHEN ReceivedQty > 0 THEN 'Received' WHEN IssuedQty > 0 THEN 'Issued' ELSE 'Requested' END AS status     
                                                                FROM tbl_project_material_requests rm
                                                                INNER JOIN tbl_items_boq boq ON rm.tender_id = boq.ref_id AND rm.itemId = boq.id
                                                                WHERE IssuedQty > 0 and rm.planning_id = @plannigId", DBClass.CreateParameter("plannigId", id));
            int count = 1;
            while (reader.Read())
            {
                string dated = "";
                if (reader["Date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["Date"]);
                    dated = date.ToString("dd/MM/yyyy");
                }
                dgvItemsIssued.Rows.Add(count, reader["id"].ToString(), id, dated, reader["name"].ToString(), reader["unit"].ToString(), decimal.Parse(reader["Qty"].ToString()).ToString("N2"), reader["status"].ToString());
                count++;
            }
        }

        public void BindReceivedDate()
        {
            dgvItemsReceived.Rows.Clear();
            MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT CONCAT(boq.sr, ' - ', boq.name) AS name, rm.id, rm.ReceivedQty Qty, rm.IssuedDate Date, rm.unit,
                                                                CASE WHEN ReceivedQty > 0 THEN 'Received' WHEN IssuedQty > 0 THEN 'Issued' ELSE 'Requested' END AS status     
                                                                FROM tbl_project_material_requests rm
                                                                INNER JOIN tbl_items_boq boq ON rm.tender_id = boq.ref_id AND rm.itemId = boq.id
                                                                WHERE ReceivedQty > 0 and rm.planning_id = @plannigId", DBClass.CreateParameter("plannigId", id));
            int count = 1;
            while (reader.Read())
            {
                string dated = "";
                if (reader["Date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["Date"]);
                    dated = date.ToString("dd/MM/yyyy");
                }
                dgvItemsReceived.Rows.Add(count, reader["id"].ToString(), id, dated, reader["name"].ToString(), reader["unit"].ToString(), decimal.Parse(reader["Qty"].ToString()).ToString("N2"), reader["status"].ToString());
                count++;
            }
            reader.Close();
        }
        public void BindResourceDate()
        {
            dgvResource.Rows.Clear();
            MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT pr.id, pr.code, date, pr.name,r.name as roleName, phone, type, price_unit, unit_time, max_unit_time FROM tbl_project_resource pr, tbl_project_role r where r.id = pr.role AND EXISTS(SELECT 1 FROM tbl_project_planning p WHERE p.id = @plannigId AND FIND_IN_SET(pr.id, assigned_team) > 0); ", 
                DBClass.CreateParameter("plannigId", id));
            int count = 1;
            while (reader.Read())
            {
                string dated = "";
                if (reader["Date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["Date"]);
                    dated = date.ToString("dd/MM/yyyy");
                }
                dgvResource.Rows.Add(count, reader["id"].ToString(), reader["code"].ToString(), reader["name"].ToString(), reader["type"].ToString(), reader["roleName"].ToString(), decimal.Parse(reader["price_unit"].ToString()).ToString("N2"));
                count++;
            }
        }
        //private void BindItems(int refId)
        //{
        //    dgvItems.Rows.Clear();
        //    MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_project_tender_details.*, tbl_items.code as code,tbl_items.type,(select name from tbl_unit where id=tbl_project_tender_details.unit_id) as unit_name FROM tbl_project_tender_details INNER JOIN 
        //                                                            tbl_items ON tbl_project_tender_details.item_id = tbl_items.id WHERE 
        //                                                            tbl_project_tender_details.tender_id = @id;",
        //                                                    DBClass.CreateParameter("id", id));
        //    int count = 0;
        //    while (reader.Read())
        //    {
        //        decimal totalAmount = 0, subTotal = 0, marginPercentage = 0, margin = 0;
        //        subTotal = Convert.ToDecimal(reader["rate"].ToString()) * Convert.ToDecimal(reader["qty"].ToString());
        //        marginPercentage = Convert.ToDecimal(reader["margin_percentage"].ToString());
        //        margin = Convert.ToDecimal(reader["margin_amount"].ToString());
        //        totalAmount = subTotal + margin;
        //        dgvItems.Rows.Add(reader["item_id"].ToString(), (count++), reader["sr"].ToString(), reader["code"].ToString(), reader["code"].ToString(),
        //            decimal.Parse(reader["qty"].ToString()).ToString("F2"),
        //            int.Parse(reader["unit_id"].ToString()), reader["unit_id"].ToString(),
        //            //reader["rate"].ToString(), 
        //            //subTotal.ToString(), 
        //            //marginPercentage.ToString(), 
        //            //margin.ToString(),
        //            //totalAmount.ToString(),
        //            decimal.Parse(reader["rate"].ToString()).ToString("F2"),
        //            decimal.Parse(subTotal.ToString()).ToString("F2"),
        //            decimal.Parse(marginPercentage.ToString()).ToString("F2"),
        //            decimal.Parse(margin.ToString()).ToString("F2"),
        //            decimal.Parse(totalAmount.ToString()).ToString("F2"),
        //            reader["type"].ToString());
        //    }
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                if (insertData())
                    reloadData();
            }
            else
            {
                if (updateData())
                    reloadData();
            }
            //master.BindData();
        }
        private void resetGridViewX()
        {
            dgvItems.Rows.Clear();
            dgvItems.Columns.Add("itemId", "id");
            dgvItems.Columns.Add("no", "#");
            dgvItems.Columns.Add("sr", "Sr.");
            dgvItems.Columns.Add("name", "Item Name");
            dgvItems.Columns.Add("qty", "Qty");
            dgvItems.Columns.Add("unit", "Unit");
            dgvItems.Columns.Add("rate", "Rate");
            dgvItems.Columns.Add("amount", "Amount");
            dgvItems.Columns.Add("marginPercentage", "Margin %");
            dgvItems.Columns.Add("marginAmount", "Margin Amount");
            dgvItems.Columns.Add("total", "Total");
            dgvItems.Columns.Add("type", "Type");

            //DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            //deleteButtonColumn.Name = "delete";
            //deleteButtonColumn.HeaderText = "DEL";
            ////deleteButtonColumn.Text = "Remove";
            //deleteButtonColumn.UseColumnTextForButtonValue = true;
            //dgvItems.Columns.Add(deleteButtonColumn);

            // this.sr,this.code,this.name,this.qty,this.unit,this.unitId,this.rate,this.amount,this.marginPercentage,this.marginAmount,this.total,this.type,this.delete

            dgvItems.Columns["no"].Width = 35;
            dgvItems.Columns["sr"].Width = 45;
            dgvItems.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvItems.Columns["qty"].Width = 80;
            dgvItems.Columns["unit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvItems.Columns["unit"].FillWeight = 20F;
            dgvItems.Columns["amount"].Width = 130;
            dgvItems.Columns["marginAmount"].Width = 120;
            //dgvItems.Columns["delete"].Width = 60;

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
        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_planning 
                                    SET modified_by = @modifiedBy, modified_date = @modifiedDate ,date=@date, project_id=@projectId, location=@location,site=@site,plot_number=@plotNumber, start_date=@startDate, end_date=@endDate, status=@status, estimated_budget=@estimatedBudget, project_type=@projectType, description=@description, fund_account_id=@accountId, fund_period=@fundPeriod,assigned_team=@assignedTeam,progress=@progress WHERE id = @id;",
            DBClass.CreateParameter("id", id),
            DBClass.CreateParameter("projectId", cmbProjectName.SelectedValue.ToString()),
            DBClass.CreateParameter("date", dtInv.Value.Date),
            DBClass.CreateParameter("location", "0"),
            DBClass.CreateParameter("site", cmbProjectSite.SelectedValue.ToString()),
            DBClass.CreateParameter("plotNumber", ""),
            DBClass.CreateParameter("startDate", dtp_start.Value.Date),
            DBClass.CreateParameter("endDate", dtp_end.Value.Date),
            DBClass.CreateParameter("status", cmbProjectStatus.SelectedItem.ToString()),
            DBClass.CreateParameter("projectType", cmbProjectType.SelectedItem.ToString()),
            DBClass.CreateParameter("estimatedBudget", txtEstimatedBudget.Text.ToString()),
            DBClass.CreateParameter("description", ""),
            DBClass.CreateParameter("accountId", cmbFundAccount.SelectedValue.ToString()),
            DBClass.CreateParameter("fundPeriod", ""),
            DBClass.CreateParameter("assignedTeam", ""),
            DBClass.CreateParameter("progress", 0),
            //DBClass.CreateParameter("tenderId", cmbTenderName.SelectedValue.ToString()),
            //DBClass.CreateParameter("tender_name_id", cmbTenderName.SelectedValue.ToString()),
            DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
            DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));
            //updatePlaning();
            CommonInsert.DeleteTransactionEntry(id, "PROJECT PLANNING");
            transactions();
            Utilities.LogAudit(frmLogin.userId, "Update Project Planning", "Project Planning", id, "Updated Project Planning: " + cmbProjectName.Text + " Tender Name: " + cmbTenderName.Text);
            return true;
        }
        private void transactions()
        {
            if (!string.IsNullOrEmpty(txtEstimatedBudget.Text))
            {
                decimal TotalAmount = decimal.Parse(txtEstimatedBudget.Text.ToString());
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                     cmbFundAccount.SelectedValue.ToString(),
                      TotalAmount.ToString(), "0", id.ToString(), "0", "Project Planning", "PROJECT PLANNING", "Project Planning Invoice NO. ",
                     frmLogin.userId, DateTime.Now.Date, "0");
                CommonInsert.addTransactionEntry(dtInv.Value.Date,
                     AccountCashId.ToString(),
                      "0", TotalAmount.ToString(), id.ToString(), "0", "Project Planning", "PROJECT PLANNING", "Project Planning Invoice NO. ",
                     frmLogin.userId, DateTime.Now.Date, "0");
            }
        }
        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            invId = decimal.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_project_planning(date,project_id, location,site,plot_number, start_date, end_date, status, estimated_budget,project_type,description,fund_account_id,fund_period,assigned_team, progress,tender_id, created_by, created_date, tender_name_id) VALUES (@date,@projectId, @location,@site,@plotNumber, @startDate, @endDate, @status, @estimatedBudget,@projectType,@description,@accountId,@fundPeriod,@assignedTeam,@progress,@tenderId, @created_by, @created_date,@tenderNameId);SELECT LAST_INSERT_ID();",
            DBClass.CreateParameter("date", dtInv.Value.Date),
            DBClass.CreateParameter("projectId", cmbProjectName.SelectedValue.ToString()),
            DBClass.CreateParameter("location", "0"),
            DBClass.CreateParameter("site", cmbProjectSite.SelectedValue.ToString()),
            DBClass.CreateParameter("plotNumber", ""),
            DBClass.CreateParameter("startDate", DateTime.Now.Date),
            DBClass.CreateParameter("endDate", DateTime.Now.Date),
            DBClass.CreateParameter("status", cmbProjectStatus.SelectedItem.ToString()),
            DBClass.CreateParameter("projectType", cmbProjectType.SelectedItem.ToString()),
            DBClass.CreateParameter("estimatedBudget", txtEstimatedBudget.Text.ToString()),
            DBClass.CreateParameter("description", ""),
            DBClass.CreateParameter("accountId", cmbFundAccount.SelectedValue.ToString()),
            DBClass.CreateParameter("fundPeriod", ""),
            DBClass.CreateParameter("assignedTeam", ""),
            DBClass.CreateParameter("progress", 0),
            DBClass.CreateParameter("tenderId", tenderId.ToString()),
            DBClass.CreateParameter("tenderNameId", cmbTenderName.SelectedValue.ToString()),
            DBClass.CreateParameter("created_by", frmLogin.userId),
            DBClass.CreateParameter("created_date", DateTime.Now.Date)).ToString());
            id = (int) decimal.Parse(invId.ToString());
            //updatePlaning();
            transactions();
            Utilities.LogAudit(frmLogin.userId, "Create Project Planning", "Project Planning", id, "Created Project Planning: " + cmbProjectName.Text + " Tender Name: " + cmbTenderName.Text);

            return true;
        }
        private void updatePlaning()
        {
            foreach (DataGridViewRow row in dgvItems.Rows)
            {
                if (row.Cells[1].Value!=null)
                {
                    string startDate = row.Cells["start_date"].Value == null ? "2025-01-01" : DateTime.Parse(row.Cells["start_date"].Value.ToString()).ToString();
                    string endDate = row.Cells["end_date"].Value == null ? "2025-01-01" : DateTime.Parse(row.Cells["end_date"].Value.ToString()).ToString();
                    DateTime startedDate = DateTime.ParseExact(startDate, "dd/MMM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime entedDate = DateTime.ParseExact(startDate, "dd/MMM/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);

                    DBClass.ExecuteNonQuery(@"UPDATE tbl_project_tender_details 
                                    SET progress=@progress,assigned=@assigned,start_date=@startDate,end_date=@endDate WHERE tender_id = @tenderId AND sr=@sr",
                    DBClass.CreateParameter("tenderId", tenderId),
                    DBClass.CreateParameter("sr", row.Cells["sr"].Value),
                    DBClass.CreateParameter("progress", row.Cells["progress"].Value == null ? "0" : row.Cells["progress"].Value.ToString()),
                    DBClass.CreateParameter("assigned", row.Cells["assigned"].Value == null ? null : row.Cells["assigned"].Value.ToString()),
                    DBClass.CreateParameter("startDate", startedDate.Date),
                    DBClass.CreateParameter("endDate", entedDate.Date));
                }
            }
        }
        private bool chkRequiredDate()
        {
            if (!string.IsNullOrEmpty(cmbProjectName.SelectedText.ToString()))
            {
                MessageBox.Show("Select Project First.");
                return false;
            }
            if (txtEstimatedBudget.Text == "" || decimal.Parse(txtEstimatedBudget.Text) == 0)
            {
                MessageBox.Show("Budget Must Be Bigger Than Zero");
                return false;
            }
            if (tenderId<=0)
            {
                MessageBox.Show("Tender is not initiated for this project");
                return false;
            }
            if (cmbTenderName.SelectedValue==null)
            {
                MessageBox.Show("Select Tender Name First.");
                return false;
            }
            if (cmbProjectSite.SelectedValue == null)
            {
                MessageBox.Show("Select Site Name First.");
                return false;
            }
            return true;
        }
        private void resetTextBox()
        {

            txtEstimatedBudget.Text = "";
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

        private void lnkProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //new frmViewProject(this).Show();
        }

        private void txtEstimatedBudget_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }
            if (char.IsDigit(e.KeyChar))
            {
                return;
            }
            if (e.KeyChar == '.' && !txtEstimatedBudget.Text.Contains("."))
            {
                return;
            }
            e.Handled = true;
        }

        private void cmbProjectStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void cmbTenderName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_planning where id = @id",
            //    DBClass.CreateParameter("id", id));
            MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_tender WHERE project_id=@pId AND tender_name_id=@tId",
                DBClass.CreateParameter("pId", cmbProjectName.SelectedValue),
                DBClass.CreateParameter("tId", cmbTenderName.SelectedValue));
            if (reader.Read())
            {
                //dtpt.Value = DateTime.Parse(reader["date"].ToString());
                //cmbProject.SelectedValue = reader["project_id"].ToString();
                //cmbAccountName.SelectedValue = reader["account_id"].ToString();
                //cmbTenderName.SelectedValue = reader["tender_name_id"].ToString();
                //cmbWarehouse.SelectedValue = reader["warehouse_id"].ToString();
                //dtsd.Value = DateTime.Parse(reader["submission_date"].ToString());
                //var fees = reader["fees"];

                //if (fees != DBNull.Value && Convert.ToDecimal(fees) > 0)
                //{
                //    txtFees.Text = fees.ToString();
                //}
                //else
                //{
                //    txtFees.Text = "";
                //}
                //txtDescription.Text = reader["description"].ToString();
                //MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_tender WHERE project_id=@pId AND tender_name_id=@tId",
                string checkQuery = "SELECT COUNT(*) from tbl_project_tender WHERE project_id=@pId AND tender_name_id=@tId";
                int count = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery,
                    DBClass.CreateParameter("pId", cmbProjectName.SelectedValue),
                DBClass.CreateParameter("tId", cmbTenderName.SelectedValue)));
                if (count == 1) {
                    tenderId = int.Parse(reader["id"].ToString());
                    BindItemsLoad();
                }
            }
            else
            {
                dgvItems.Rows.Clear();
                dataGridView1.Rows.Clear();
            }
        }
        private void BindItemsLoad()
        {
            string checkQuery = "SELECT COUNT(*) FROM tbl_items_boq where ref_id =@id";
            int countX = Convert.ToInt32(DBClass.ExecuteScalar(checkQuery, DBClass.CreateParameter("id", tenderId)));
            isExported = countX > 0 ? true : false;
            lbl_boq.Text = "BOQ-" + countX;

            dgvItems.Rows.Clear();
            string query = "";
            if (isExported)
            {
                //resetGridView();
                query = @"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.item_id=tbl_items_boq.id
                                WHERE tbl_project_tender_details.tender_id= @id;";
            }
            else
            {
                query = @"SELECT tbl_project_tender_details.*, tbl_items.code as code,tbl_items.name,tbl_items.type,(select name from tbl_unit where id=tbl_project_tender_details.unit_id) as unit_name FROM tbl_project_tender_details INNER JOIN 
                                                                    tbl_items ON tbl_project_tender_details.item_id = tbl_items.id WHERE 
                                                                    tbl_project_tender_details.tender_id = @id;";
            }
            MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", tenderId));
            loadActivityPalnning(reader);
            reader.Close();
            //int count = 1;
            //while (reader.Read())
            //{
            //    decimal totalAmount = 0, subTotal = 0, marginPercentage = 0, margin = 0;
            //    subTotal = Convert.ToDecimal(reader["rate"].ToString()) * Convert.ToDecimal(reader["qty"].ToString());
            //    marginPercentage = Convert.ToDecimal(reader["margin_percentage"].ToString());
            //    margin = Convert.ToDecimal(reader["margin_amount"].ToString());
            //    totalAmount = subTotal + margin;
            //    if (isExported)
            //    {
            //        string dated = DateTime.Now.Date.ToString();
            //        DateTime dateX = Convert.ToDateTime(dated);
            //        dated = dateX.ToString("dd/MM/yyyy");
            //        string startDate = dated;
            //        string endDate = dated;
            //        if (reader["start_date"] != DBNull.Value)
            //        {
            //            DateTime date = Convert.ToDateTime(reader["start_date"]);
            //            startDate = date.ToString("dd/MM/yyyy");
            //        }
            //        if (reader["end_date"] != DBNull.Value)
            //        {
            //            DateTime date = Convert.ToDateTime(reader["end_date"]);
            //            endDate = date.ToString("dd/MM/yyyy");
            //        }

            //        string empId = "0";
            //        if (reader["assigned"] == null || reader["assigned"].ToString().Length > 0)
            //        {
            //            empId = reader["assigned"].ToString();
            //        }
            //        dgvItems.Rows.Add((count++), reader["sr"].ToString(), reader["name"].ToString(), 
            //            (int.Parse(empId) > 0 ? reader["assigned"] : null),
            //            startDate,
            //            endDate,
            //            0);
            //    }
            //    else
            //    {
            //        string dated = DateTime.Now.Date.ToString();
            //        DateTime dateX = Convert.ToDateTime(dated);
            //        dated = dateX.ToString("dd/MM/yyyy");
            //        string startDate = dated;
            //        string endDate = dated;
            //        if (reader["start_date"] != DBNull.Value)
            //        {
            //            DateTime date = Convert.ToDateTime(reader["start_date"]);
            //            startDate = date.ToString("dd/MM/yyyy");
            //        }
            //        if (reader["end_date"] != DBNull.Value)
            //        {
            //            DateTime date = Convert.ToDateTime(reader["end_date"]);
            //            endDate = date.ToString("dd/MM/yyyy");
            //        }

            //        dgvItems.Rows.Add((count++), reader["sr"].ToString(), reader["name"].ToString(),
            //            (int.Parse(reader["assigned"].ToString()) > 0 ? reader["assigned"] : null),
            //            startDate,
            //            endDate,
            //            0);
            //        //dgvItems.Rows.Add(reader["item_id"].ToString(), (count++), reader["sr"].ToString(), reader["code"].ToString(), reader["code"].ToString(),
            //        //decimal.Parse(reader["qty"].ToString()).ToString("F2"),
            //        //int.Parse(reader["unit_id"].ToString()), reader["unit_id"].ToString(),
            //        //decimal.Parse(reader["rate"].ToString()).ToString("F2"),
            //        //decimal.Parse(subTotal.ToString()).ToString("F2"),
            //        //decimal.Parse(marginPercentage.ToString()).ToString("F2"),
            //        //decimal.Parse(margin.ToString()).ToString("F2"),
            //        //decimal.Parse(totalAmount.ToString()).ToString("F2"),
            //        //reader["type"].ToString());
            //    }
            //}
        }
        private void loadBOQ()
        {
            ShowGridInsteadOfTabs();
            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("no", "#");
            dataGridView1.Columns.Add("sr", "Sr.");
            dataGridView1.Columns.Add("name", "Description of work");
            dataGridView1.Columns.Add("qty", "Qty");
            dataGridView1.Columns.Add("unit", "Unit");
            dataGridView1.Columns.Add("Length", "Length");
            dataGridView1.Columns.Add("Width", "Width");
            dataGridView1.Columns.Add("Thick", "Thick");
            dataGridView1.Columns.Add("rate", "Rate");
            dataGridView1.Columns.Add("amount", "Amount");
            dataGridView1.Columns.Add("marginPercentage", "Margin %");
            dataGridView1.Columns.Add("marginAmount", "Margin Amount");
            dataGridView1.Columns.Add("total", "Total");
            dataGridView1.Columns.Add("type", "Type");
            dataGridView1.Columns.Add("Note", "Note");

            dataGridView1.Columns["no"].Width = 35;
            dataGridView1.Columns["sr"].Width = 45;
            dataGridView1.Columns["name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["qty"].Width = 80;
            dataGridView1.Columns["unit"].Width = 50;
            dataGridView1.Columns["amount"].Width = 130;
            dataGridView1.Columns["marginAmount"].Width = 120;
            
            dataGridView1.Columns["type"].Visible = false;

            dataGridView1.Columns["no"].ReadOnly = true;
            dataGridView1.Columns["sr"].ReadOnly = true;
            dataGridView1.Columns["amount"].ReadOnly = true;
            dataGridView1.Columns["total"].ReadOnly = true;
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.Format = "N3";
            dataGridViewCellStyle1.NullValue = null;
            dataGridView1.Columns["rate"].DefaultCellStyle = dataGridViewCellStyle1;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.Format = "N3";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.TopRight;
            dataGridViewCellStyle2.NullValue = null;
            dataGridView1.Columns["amount"].DefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridView1.Columns["marginPercentage"].DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.Columns["marginAmount"].DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Columns["total"].DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ReadOnly = true;
            dataGridView1.Rows.Clear();
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
                                                            DBClass.CreateParameter("id", tenderId));
            int count = 1;
            decimal totalBoqAmount = 0;
            while (reader.Read())
            {
                decimal totalAmount = 0, subTotal = 0, marginPercentage = 0, margin = 0;
                subTotal = Convert.ToDecimal(reader["rate"].ToString()) * Convert.ToDecimal(reader["qty"].ToString());
                marginPercentage = Convert.ToDecimal(reader["margin_percentage"].ToString());
                margin = Convert.ToDecimal(reader["margin_amount"].ToString());
                totalAmount = subTotal + margin;
                totalBoqAmount += totalAmount;
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
                    dataGridView1.Rows.Add((count), reader["sr"].ToString(), reader["name"].ToString(),
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
                    dataGridView1.Rows.Add(reader["item_id"].ToString(), (count++), reader["sr"].ToString(), reader["code"].ToString(), reader["code"].ToString(),
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
                count++;
            }

            lblBOQTotal.Text = "BOQ Total = " + decimal.Parse(totalBoqAmount.ToString()).ToString("F2");

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) continue;

                var srValue = row.Cells["sr"].Value?.ToString().Trim();

                if (!string.IsNullOrEmpty(srValue) && Regex.IsMatch(srValue, @"^[A-Za-z]+$"))
                {
                    row.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
            ShowGridInsteadOfTabs();
        }

        private void loadActivityPalnning(MySqlDataReader reader)
        {
            //DateTime startDate = dtp_start.Value;
            //DateTime endDate = dtp_end.Value;
            //int totalDays = (endDate - startDate).Days;
            //// First, add the "standard" columns
            //dgvItems.Columns.Clear();
            //dgvItems.Columns.Add("Sr", "Sr");
            //dgvItems.Columns.Add("Name", "Name");
            //dgvItems.Columns.Add("StartDate", "Start Date");
            //dgvItems.Columns.Add("EndDate", "End Date");

            //// Now add dynamic columns for each day
            //for (int i = 0; i <= totalDays; i++)
            //{
            //    dgvItems.Columns.Add($"Day{i + 1}", startDate.AddDays(i).ToString("dd-MM-yyyy"));
            //}

            //// Then, loop through the rows in the data reader and add them to the DataGridView
            //int count = 0;
            //while (reader.Read())
            //{
            //    // Extract the values you need from the reader
            //    string sr = reader["sr"].ToString();
            //    string name = reader["name"].ToString();
            //    string startDateValue = reader["start_date"].ToString();
            //    string endDateValue = reader["end_date"].ToString();

            //    // Create a new row
            //    DataGridViewRow row = new DataGridViewRow();

            //    // Add standard values to the row
            //    row.Cells.Add(new DataGridViewTextBoxCell { Value = count++ });
            //    row.Cells.Add(new DataGridViewTextBoxCell { Value = name });
            //    row.Cells.Add(new DataGridViewTextBoxCell { Value = startDateValue });
            //    row.Cells.Add(new DataGridViewTextBoxCell { Value = endDateValue });

            //    // Add dynamic columns (Day1, Day2, ..., Day10) based on the start and end date
            //    if (totalDays > 0)
            //    {
            //        for (int i = 0; i <= totalDays; i++)
            //        {
            //            DateTime currentDay = startDate.AddDays(i);
            //            if (startDateValue == "" || endDateValue == "")
            //            {
            //                row.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
            //            }
            //            else
            //            {
            //                if (currentDay >= DateTime.Parse(startDateValue) && currentDay <= DateTime.Parse(endDateValue))
            //                {
            //                    row.Cells.Add(new DataGridViewTextBoxCell { Value = "X" }); // "X" or any other marker you want
            //                }
            //                else
            //                {
            //                    row.Cells.Add(new DataGridViewTextBoxCell { Value = "" }); // Empty if no task for that day
            //                }
            //            }
            //        }

            //        // Add the row to the DataGridView
            //        dgvItems.Rows.Add(row);
            //    }
            //}

            DateTime startDate = dtp_start.Value;
            DateTime endDate = dtp_end.Value;
            int totalDays = (endDate - startDate).Days;
            
            dgvItems.Columns.Clear();
            dgvItems.Columns.Add("no", "#");
            dgvItems.Columns.Add("Sr", "Sr");
            dgvItems.Columns.Add("Name", "Name");
            dgvItems.Columns.Add("StartDate", "Start Date");
            dgvItems.Columns.Add("EndDate", "End Date");
            dgvItems.Columns.Add("Progress", "Progress");
            
            for (int i = 0; i <= totalDays; i++)
            {
                dgvItems.Columns.Add($"Day{i + 1}", startDate.AddDays(i).ToString("dd-MM-yyyy"));
            }
            
            int count = 1;
            while (reader.Read())
            {
                string sr = reader["sr"].ToString();
                string name = reader["name"].ToString();
                string startDateValue = reader["start_date"].ToString();
                string endDateValue = reader["end_date"].ToString();
                string progress = decimal.Parse(reader["progress"].ToString()).ToString("#.##");
                
                DataGridViewRow row = new DataGridViewRow();
                
                string sDate = "",eDate="";
                if (reader["start_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["start_date"]);
                    sDate = date.ToString("dd/MM/yyyy");
                }
                if (reader["end_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["end_date"]);
                    eDate = date.ToString("dd/MM/yyyy");
                }
                row.Cells.Add(new DataGridViewTextBoxCell { Value = count++ });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = sr });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = name });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = sDate });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = eDate });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = progress });
                
                if (totalDays > 0)
                {
                    for (int i = 0; i <= totalDays; i++)
                    {
                        DateTime currentDay = startDate.AddDays(i);

                        if (string.IsNullOrEmpty(startDateValue) || string.IsNullOrEmpty(endDateValue))
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
                        }
                        else
                        {
                            if (currentDay >= DateTime.Parse(startDateValue) && currentDay <= DateTime.Parse(endDateValue))
                            {
                                DataGridViewCell cell = new DataGridViewTextBoxCell { Value = "" };
                                cell.Style.BackColor = System.Drawing.Color.IndianRed;
                                row.Cells.Add(cell);
                            }
                            else
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = "" });
                            }
                        }
                    }
                    dgvItems.Rows.Add(row);
                }
                else
                {
                    dgvItems.Rows.Add(row);
                }
            }
            dgvItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            if (!tabControl1.Visible)
            {
                dataGridView1.Visible = true;
                loadBOQ();
            }
            else
            {
                dataGridView1.Visible = false;
            }
        }

        private void btnAddTender_Click(object sender, EventArgs e)
        {
            new frmProjectSites(this,0).ShowDialog();
        }

        private void dgvItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int _aId = 0, _projectId=0, _siteId=0;
            _siteId = cmbProjectSite.SelectedValue == null ? 0 : int.Parse(cmbProjectSite.SelectedValue.ToString());
            _projectId = cmbProjectName.SelectedValue == null ? 0 : int.Parse(cmbProjectName.SelectedValue.ToString());
            int _tenderNameId = cmbTenderName.SelectedValue==null?0: int.Parse(cmbTenderName.SelectedValue.ToString());
            MasterActivity frm = new MasterActivity(this, _aId, _projectId, _tenderNameId, _siteId, tenderId, id);
            frm.WindowState = FormWindowState.Maximized;
            //frm.TopMost = true;
            if (frm.ShowDialog() == DialogResult.OK){
                BindItemsLoad();
            }
        }

        private void btnNewRequestMaterial_Click(object sender, EventArgs e)
        {
            if(id>0)
                frmLogin.frmMain.openChildForm(new frmRequestMaterial(this,0,tenderId,id));
        }

        private void btnNewReceivedMaterial_Click(object sender, EventArgs e)
        {
            if (id > 0)
                frmLogin.frmMain.openChildForm(new frmReceivedMaterial(this, 0, tenderId, id));
        }

        private void btnNewIssueMaterial_Click(object sender, EventArgs e)
        {
            if (id > 0)
                frmLogin.frmMain.openChildForm(new frmIssueMaterial(this, 0, tenderId, id));
        }

        private void btnShowTenderList_Click(object sender, EventArgs e)
        {
            frmViewSelectTender selectTenderForm = new frmViewSelectTender(int.Parse(cmbTenderName.SelectedValue.ToString()),int.Parse(cmbProjectName.SelectedValue.ToString()));
            if (selectTenderForm.ShowDialog() == DialogResult.OK)
            {
                int selectedTenderId = selectTenderForm.SelectedTenderId;
                tenderId = selectedTenderId;
                BindItemsLoad();
            }
        }

        private void reloadData()
        {
            if (id > 0)
            {
                tabControl1.Visible = true;
                ShowTabsInsteadOfGrid();
                BindItemsLoad();
            }
        }

        private void dgvRequest_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvRequest.Rows.Count == 0)
                return;

            if (id > 0 && tenderId > 0 && int.Parse(dgvRequest.CurrentRow.Cells["pId"].Value.ToString())>0)
            {
                frmLogin.frmMain.openChildForm(new frmRequestMaterial(this, int.Parse(dgvRequest.CurrentRow.Cells["pId"].Value.ToString()), tenderId, id));
            }
        }

        private void dgvItemsIssued_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItemsIssued.Rows.Count == 0)
                return;

            if (id > 0 && tenderId > 0 && int.Parse(dgvItemsIssued.CurrentRow.Cells["pId1"].Value.ToString()) > 0)
            {
                frmLogin.frmMain.openChildForm(new frmIssueMaterial(this, int.Parse(dgvItemsIssued.CurrentRow.Cells["pId1"].Value.ToString()), tenderId, id));
            }
        }

        private void dgvItemsReceived_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvItemsReceived.Rows.Count == 0)
                return;

            if (id > 0 && tenderId > 0 && int.Parse(dgvItemsReceived.CurrentRow.Cells["pId2"].Value.ToString()) > 0)
            {
                frmLogin.frmMain.openChildForm(new frmReceivedMaterial(this, int.Parse(dgvItemsReceived.CurrentRow.Cells["pId2"].Value.ToString()), tenderId, id));
            }
        }

        private void btnAddResource_Click(object sender, EventArgs e)
        {
            new frmProjectAssignResource(this, 0, tenderId, id).ShowDialog();
        }

        private void dgvResource_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvResource.Rows.Count == 0)
                return;

            if (id > 0 && tenderId > 0 && int.Parse(dgvResource.CurrentRow.Cells["ResourceId"].Value.ToString()) > 0)
            {
                new frmProjectAssignResource(this, int.Parse(dgvResource.CurrentRow.Cells["ResourceId"].Value.ToString()), tenderId, id).ShowDialog();
            }
        }

        private void ShowGridInsteadOfTabs()
        {
            // Hide the tab control
            tabControl1.Visible = false;

            // Make DataGridView visible
            dataGridView1.Visible = true;

            // Position and size DataGridView to match tabControl
            dataGridView1.Location = tabControl1.Location;
            dataGridView1.Size = tabControl1.Size;
        }
        private void ShowTabsInsteadOfGrid()
        {
            tabControl1.Visible = true;
            dataGridView1.Visible = false;
        }
    }
}
