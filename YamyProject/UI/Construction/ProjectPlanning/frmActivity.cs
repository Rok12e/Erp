using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using YamyProject.DAL;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmActivity : Form
    {
        private MasterActivity master;
        int id, tenderId, projectId, siteId, tenderNameId, planningId, itemId;
        List<string> assignedTeam = new List<string>();

        public frmActivity(MasterActivity _master, int id, int _projectId, int _tenderNameId, int _siteId, int _tenderId, int _planningId, int _itemId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = id;
            this.tenderId = _tenderId;
            this.projectId = _projectId;
            this.siteId = _siteId;
            this.planningId = _planningId;
            this.tenderNameId = _tenderNameId;
            this.itemId = _itemId;
        }
        private void frmActivity_Load(object sender, EventArgs e)
        {

            txtDescription.Enabled = false;
            BindCombo();

            BindData();
            
            if (itemId != 0)
            {
                BindGridData();
            }
        }
        public void BindCombo()
        {
            populateList(lstOfEmp);
        }

        private void populateList(CheckedListBox lst)
        {
            var dt = DBClass.ExecuteDataTable(@"
                        SELECT pr.id, pr.code, date, pr.name, r.name AS roleName, phone, type, price_unit, unit_time, max_unit_time 
                        FROM tbl_project_resource pr
                        JOIN tbl_project_role r ON r.id = pr.role
                        WHERE EXISTS (
                            SELECT 1 
                            FROM tbl_project_planning p 
                            WHERE p.id = @planningId 
                              AND FIND_IN_SET(pr.id, assigned_team) > 0
                        );",
                DBClass.CreateParameter("planningId", planningId)); // Fixed typo

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    // You can create a custom item or display just the name
                    lst.Items.Add(new ListItem
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Name = row["name"].ToString()
                    }, false); // false = unchecked
                }
            }
        }

        private void BindData()
        {
            string query = "SELECT id,start_date,end_date,progress,status FROM tbl_project_activity WHERE planning_id = @pId AND code = @code";
            using (MySqlDataReader reader = DBClass.ExecuteReader(query,
                            DBClass.CreateParameter("@pId", planningId),
                            DBClass.CreateParameter("@code", itemId.ToString())))
            {
                if (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    dtStart.Value = DateTime.Parse(reader["start_date"].ToString());
                    dtEnd.Value = DateTime.Parse(reader["end_date"].ToString());
                    List<int> assignedResourceIds = new List<int>();
                    using (MySqlDataReader reader1 = DBClass.ExecuteReader(@"SELECT resource_id FROM tbl_project_activity_assignment WHERE activity_id = @activityId",
                        DBClass.CreateParameter("@activityId", id)))
                    {
                        while (reader1.Read())
                        {
                            assignedResourceIds.Add(Convert.ToInt32(reader1["resource_id"]));
                        }
                    }
                    for (int i = 0; i < lstOfEmp.Items.Count; i++)
                    {
                        ListItem listItem = lstOfEmp.Items[i] as ListItem;
                        if (listItem != null && assignedResourceIds.Contains(listItem.Id))
                        {
                            lstOfEmp.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }
        private void BindGridData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.sr=tbl_items_boq.sr
                                WHERE tbl_project_tender_details.id = @id;", DBClass.CreateParameter("id", itemId)))
            {
                if (reader.Read())
                {
                    string dated = DateTime.Now.Date.ToString();
                    DateTime dateX = Convert.ToDateTime(dated);
                    dated = dateX.ToString("dd/MM/yyyy");
                    string startDate = dated;
                    string endDate = dated;
                    if (reader["start_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["start_date"]);
                        startDate = date.ToString("dd/MM/yyyy");
                    }
                    if (reader["end_date"] != DBNull.Value)
                    {
                        DateTime date = Convert.ToDateTime(reader["end_date"]);
                        endDate = date.ToString("dd/MM/yyyy");
                    }

                    string empId = "0";
                    if (reader["assigned"] == null || reader["assigned"].ToString().Length > 0)
                    {
                        empId = reader["assigned"].ToString();
                    }
                    string progress = "0";
                    if (reader["progress"] == null || reader["progress"].ToString().Length > 0)
                    {
                        progress = decimal.Parse(reader["progress"].ToString()).ToString("#.##");
                    }
                    string sr = reader["sr"].ToString();
                    string name = reader["name"].ToString();
                    txtDescription.Text = sr + " - " + name;
                    string format = "dd/MM/yyyy";
                    DateTime sDate;
                    DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out sDate);
                    dtStart.Value = sDate;
                    DateTime eDate;
                    DateTime.TryParseExact(startDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out eDate);
                    dtEnd.Value = eDate;
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
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            int progress = string.IsNullOrEmpty(txtProgress.Text) ? 0 : int.Parse(txtProgress.Text);
            string status = "";
            if (progress == 100)
                status = "Completed";
            else if (DateTime.Today >= dtStart.Value)
                status = "In Progress";
            else
                status = "Not Started";

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_activity 
                                      SET planning_id = @planningId,code = @code,name = @name,start_date = @startDate,end_date = @endDate,progress = @progress,status = @status WHERE id = @id;",
                                        DBClass.CreateParameter("planningId", planningId),
                                        DBClass.CreateParameter("code", itemId),
                                        DBClass.CreateParameter("name", itemId),
                                        DBClass.CreateParameter("startDate", dtStart.Value),
                                        DBClass.CreateParameter("endDate", dtEnd.Value),
                                        DBClass.CreateParameter("progress", progress),
                                        DBClass.CreateParameter("status", status),
                                        DBClass.CreateParameter("id", id)
                                    );

            DBClass.ExecuteNonQuery("DELETE FROM tbl_project_activity_assignment WHERE activity_id= @id", DBClass.CreateParameter("id" , id));

            foreach (var item in lstOfEmp.CheckedItems)
            {
                ListItem listItem = item as ListItem;
                if (listItem != null)
                {
                    int result1 = DBClass.ExecuteNonQuery(
                        @"INSERT INTO tbl_project_activity_assignment(activity_id, resource_id) VALUES(@id, @resId);",
                        DBClass.CreateParameter("@id", id),
                        DBClass.CreateParameter("@resId", listItem.Id));
                    Utilities.LogAudit(frmLogin.userId, "Update Project Activity Assignment", "Project Activity Assignment", result1, "Updated Project Activity Assignment: " + listItem.Name);
                }
            }

            updatePlaning(progress);
            Utilities.LogAudit(frmLogin.userId, "Update Project Activity", "Project Activity", id, "Updated Project Activity: " + itemId);

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            int progress = string.IsNullOrEmpty(txtProgress.Text) ? 0 : int.Parse(txtProgress.Text);
            string status = "";
            if (progress == 100)
                status = "Completed";
            else if (DateTime.Today >= dtStart.Value)
                status = "In Progress";
            else
                status = "Not Started";

            id = int.Parse(DBClass.ExecuteScalar(@"INSERT INTO tbl_project_activity (planning_id, code, name, start_date, end_date,progress,status) 
                                        VALUES(@planningId, @code, @name, @startDate, @endDate, @progress, @status); SELECT LAST_INSERT_ID();",
                                        DBClass.CreateParameter("planningId", planningId),
                                        DBClass.CreateParameter("code", itemId),
                                        DBClass.CreateParameter("name", itemId),
                                        DBClass.CreateParameter("startDate", dtStart.Value),
                                        DBClass.CreateParameter("endDate", dtEnd.Value),
                                        DBClass.CreateParameter("progress", progress),
                                        DBClass.CreateParameter("status", status)).ToString());

            foreach (var item in lstOfEmp.CheckedItems)
            {
                ListItem listItem = item as ListItem;
                if (listItem != null)
                {
                    int result1 = DBClass.ExecuteNonQuery(
                        @"INSERT INTO tbl_project_activity_assignment(activity_id, resource_id) VALUES(@id, @resId);",
                        DBClass.CreateParameter("@id", id),
                        DBClass.CreateParameter("@resId", listItem.Id));
                    Utilities.LogAudit(frmLogin.userId, "Insert Project Activity Assignment", "Project Activity Assignment", result1, "Inserted Project Activity Assignment: " + listItem.Name);
                }
            }
            updatePlaning(progress);

            Utilities.LogAudit(frmLogin.userId, "Insert Project Activity", "Project Activity", id, "Inserted Project Activity: " + itemId);
            return true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            ///this.minimize;
        }
        
        private void txtProgress_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            // Allow digits, control keys (like backspace), and one dot
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true; // Block the key
            }

            // Only allow one decimal point
            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
            }
        }

        private void updatePlaning(int progress)
        {
            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_tender_details 
                                    SET start_date=@startDate,end_date=@endDate, progress=@progress WHERE id=@id and tender_id = @tenderId",
            DBClass.CreateParameter("tenderId", tenderId), 
            DBClass.CreateParameter("id", itemId),
            DBClass.CreateParameter("startDate", dtStart.Value),
            DBClass.CreateParameter("endDate", dtEnd.Value),
            DBClass.CreateParameter("progress", progress));

            //DBClass.ExecuteNonQuery(@"UPDATE tbl_project_planning SET modified_by = @modifiedBy, modified_date = @modifiedDate, progress=@progress WHERE id = @id;",
            //DBClass.CreateParameter("id", planningId),
            //DBClass.CreateParameter("progress", 0),
            //DBClass.CreateParameter("@modifiedBy", frmLogin.userId),
            //DBClass.CreateParameter("@modifiedDate", DateTime.Now.Date));
            Utilities.LogAudit(frmLogin.userId, "Update Project Planning", "Project Planning", planningId, "Updated Project Planning: " + planningId);
        }
        private bool chkRequiredDate()
        {
            if (string.IsNullOrEmpty(projectId.ToString()))
            {
                MessageBox.Show("Select Project First.");
                return false;
            }
            if (string.IsNullOrEmpty(tenderId.ToString()))
            {
                MessageBox.Show("Select Tender First.");
                return false;
            }
            if (string.IsNullOrEmpty(planningId.ToString()))
            {
                MessageBox.Show("Select planning Not initiated.");
                return false;
            }
            return true;
        }
        private void resetTextBox()
        {
            id = 0;
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
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BindItemsLoad(int refId)
        {
            string query = "";
            //resetGridView();
            query = @"SELECT tbl_project_tender_details.*, tbl_items_boq.id as code,tbl_items_boq.name,tbl_items_boq.type,tbl_items_boq.unit_name as unit_name FROM tbl_project_tender_details 
                                INNER JOIN tbl_items_boq ON tbl_project_tender_details.tender_id = ref_id AND tbl_project_tender_details.sr=tbl_items_boq.sr
                                WHERE tbl_project_tender_details.tender_id= @id;";
            
            MySqlDataReader reader = DBClass.ExecuteReader(query, DBClass.CreateParameter("id", refId));
            int count = 0;
            while (reader.Read())
            {
                decimal totalAmount = 0, subTotal = 0, marginPercentage = 0, margin = 0;
                subTotal = Convert.ToDecimal(reader["rate"].ToString()) * Convert.ToDecimal(reader["qty"].ToString());
                marginPercentage = Convert.ToDecimal(reader["margin_percentage"].ToString());
                margin = Convert.ToDecimal(reader["margin_amount"].ToString());
                totalAmount = subTotal + margin;

                string dated = DateTime.Now.Date.ToString();
                DateTime dateX = Convert.ToDateTime(dated);
                dated = dateX.ToString("dd/MM/yyyy");
                string startDate = dated;
                string endDate = dated;
                if (reader["start_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["start_date"]);
                    startDate = date.ToString("dd/MM/yyyy");
                }
                if (reader["end_date"] != DBNull.Value)
                {
                    DateTime date = Convert.ToDateTime(reader["end_date"]);
                    endDate = date.ToString("dd/MM/yyyy");
                }

                string empId = "0";
                if (reader["assigned"] == null || reader["assigned"].ToString().Length > 0)
                {
                    empId = reader["assigned"].ToString();
                }
            }
        }
    }
}

public class ListItem
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name; // This is what will display in the list
    }
}