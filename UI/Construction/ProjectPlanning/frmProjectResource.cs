using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmProjectResource : Form
    {
        private frmProjectAssignResource master;
        int id, planningId, tenderId;
        List<string> assignedTeam = new List<string>();

        public frmProjectResource(frmProjectAssignResource _master, int _id, int _tenderId, int _planningId)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.master = _master;
            this.id = _id;
            this.planningId = _planningId;
            this.tenderId = _tenderId;
            if (id != 0)
                this.Text = "Project Resource - Edit";
            else
                this.Text = "Project Resource - New";
            headerUC1.FormText = this.Text;
        }
        private void frmProjectResource_Load(object sender, EventArgs e)
        {
            dtDate.Value = DateTime.Now.Date;
            loadCombo();
            object result = DBClass.ExecuteScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM tbl_project_resource");
            int recordCount = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
            if (recordCount > 0)
            {
                txtCode.Text = "R" + recordCount;
            }
            if (rbLabour.Checked)
            {
                pnlEmployee.Visible = true;
            }

            if (id != 0)
                BindData();
        }

        private void BindData()
        {
            MySqlDataReader reader = DBClass.ExecuteReader(" select * from tbl_project_resource where id = @id",
                DBClass.CreateParameter("id", id));
            if (reader.Read())
            {
                dtDate.Value = DateTime.Parse(reader["date"].ToString());
                txtCode.Text = reader["code"].ToString();
                txtName.Text = reader["name"].ToString();
                cmbRole.SelectedValue = reader["role"].ToString();
                txtPhone.Text = reader["phone"].ToString();
                string type = reader["type"].ToString();
                if (type == "Labour")
                {
                    rbLabour.Checked = true;
                    pnlEmployee.Visible = true;
                    cmbEmployee.SelectedValue = reader["employee_id"].ToString();
                }
                else if (type == "Material")
                {
                    rbMaterial.Checked = true;
                }
                else if (type == "Non")
                {
                    rbNonLabour.Checked = true;
                }
                txtPriceUnit.Text = decimal.Parse(reader["price_unit"].ToString()).ToString("N2");
                txtUnitTime.Text = decimal.Parse(reader["unit_time"].ToString()).ToString("N2");
                txtMaxUnitTime.Text = decimal.Parse(reader["max_unit_time"].ToString()).ToString("N2");
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
            master.LoaddgvItems();
        }

        private bool updateData()
        {
            if (!chkRequiredDate())
                return false;

            string type = rbLabour.Checked ? "Labour" : rbMaterial.Checked ? "Material" : "Non";
            int empId = rbLabour.Checked ? int.Parse(cmbEmployee.SelectedValue.ToString()) : 0;

            DBClass.ExecuteNonQuery(@"UPDATE tbl_project_resource
                          SET date = @date, name = @name, role = @role, phone = @phone, type = @type, code = @code,
                              price_unit = @priceUnit, unit_time = @unitTime, max_unit_time = @maxUnitTime, employee_id = @empId
                          WHERE id = @id",
                          DBClass.CreateParameter("id", id),
                          DBClass.CreateParameter("code", txtCode.Text),
                          DBClass.CreateParameter("date", dtDate.Value.Date),
                          DBClass.CreateParameter("name", txtName.Text),
                          DBClass.CreateParameter("role", cmbRole.SelectedValue.ToString()),
                          DBClass.CreateParameter("phone", txtPhone.Text),
                          DBClass.CreateParameter("type", type),
                          DBClass.CreateParameter("priceUnit", string.IsNullOrEmpty(txtPriceUnit.Text) ? "0" : txtPriceUnit.Text),
                          DBClass.CreateParameter("unitTime", string.IsNullOrEmpty(txtUnitTime.Text) ? "8" : txtUnitTime.Text),
                          DBClass.CreateParameter("maxUnitTime", string.IsNullOrEmpty(txtMaxUnitTime.Text) ? "8" : txtMaxUnitTime.Text),
                          DBClass.CreateParameter("empId", empId));
            Utilities.LogAudit(frmLogin.userId, "Update Project Resource", "Project Resource", id, "Updated Project Resource: " + txtName.Text);

            return true;
        }

        private bool insertData()
        {
            if (!chkRequiredDate())
                return false;

            string type = rbLabour.Checked ? "Labour" : rbMaterial.Checked ? "Material" : "Non";
            int empId = rbLabour.Checked ? int.Parse(cmbEmployee.SelectedValue.ToString()) : 0;

            int refId = DBClass.ExecuteNonQuery(@"INSERT INTO tbl_project_resource(code, date, name, role, phone, type, price_unit, unit_time, max_unit_time, employee_id)
                          VALUES (@code, @date, @name, @role, @phone, @type, @priceUnit, @unitTime, @maxUnitTime, @empId)",
                DBClass.CreateParameter("code", txtCode.Text),
                DBClass.CreateParameter("date", dtDate.Value.Date),
                DBClass.CreateParameter("name", txtName.Text),
                DBClass.CreateParameter("role", cmbRole.SelectedValue.ToString()),
                DBClass.CreateParameter("phone", txtPhone.Text),
                DBClass.CreateParameter("type", type),
                DBClass.CreateParameter("priceUnit", string.IsNullOrEmpty(txtPriceUnit.Text) ? "0" : txtPriceUnit.Text),
                DBClass.CreateParameter("unitTime", string.IsNullOrEmpty(txtUnitTime.Text) ? "8" : txtUnitTime.Text),
                DBClass.CreateParameter("maxUnitTime", string.IsNullOrEmpty(txtMaxUnitTime.Text) ? "8" : txtMaxUnitTime.Text),
                DBClass.CreateParameter("empId", empId));
            Utilities.LogAudit(frmLogin.userId, "Insert Project Resource", "Project Resource", refId, "Inserted Project Resource: " + txtName.Text);

            return true;
        }
        private bool chkRequiredDate()
        {
            if (string.IsNullOrEmpty(txtCode.Text))
            {
                MessageBox.Show("Code Can't Be Empty");
                return false;
            }
            if (string.IsNullOrEmpty(cmbRole.Text))
            {
                MessageBox.Show("Role Can't Be Empty");
                return false;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Name Can't Be Empty");
                return false;
            }
            if (rbLabour.Checked && cmbEmployee.SelectedIndex<0)
            {
                MessageBox.Show("Employee Name Can't Be Empty");
                return false;
            }
            return true;
        }

        private void gunaLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowRoleDialog();
        }

        private void resetTextBox()
        {
            id = 0;
            dtDate.Value = DateTime.Now;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void loadCombo()
        {
            BindCombos.PopulateProjectResource(cmbRole);
            BindCombos.PopulateEmployees(cmbEmployee);
            cmbEmployee.SelectedIndex = -1;
        }

        private void rbLabour_CheckedChanged(object sender, EventArgs e)
        {
            pnlEmployee.Visible = rbLabour.Checked ? true : false;
        }

        private void ShowRoleDialog()
        {
            // Create the form
            Form dialog = new Form();
            dialog.Text = "Enter Role and Code";
            dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.Width = 320;
            dialog.Height = 180;
            dialog.MinimizeBox = false;
            dialog.MaximizeBox = false;
            dialog.ShowInTaskbar = false;

            // Label: Code
            Label lblCode = new Label();
            lblCode.Text = "Code:";
            lblCode.Left = 10;
            lblCode.Top = 20;
            lblCode.Width = 50;
            dialog.Controls.Add(lblCode);

            // TextBox: Code input
            TextBox txtCode = new TextBox();
            txtCode.Left = 70;
            txtCode.Top = 18;
            txtCode.Enabled = false;
            txtCode.Width = 220;
            
                object result = DBClass.ExecuteScalar("SELECT IFNULL(MAX(id), 0) + 1 FROM tbl_project_role");
                int recordCount = (result != null && result != DBNull.Value) ? Convert.ToInt32(result) : 0;
                if (recordCount > 0)
                {
                    txtCode.Text = "R" + recordCount;
                }
            dialog.Controls.Add(txtCode);

            // Label: Role
            Label lblRole = new Label();
            lblRole.Text = "Role:";
            lblRole.Left = 10;
            lblRole.Top = 60;
            lblRole.Width = 50;
            dialog.Controls.Add(lblRole);

            // TextBox: Role input
            TextBox txtRole = new TextBox();
            txtRole.Left = 70;
            txtRole.Top = 58;
            txtRole.Width = 220;
            dialog.Controls.Add(txtRole);

            // OK Button
            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Left = 110;
            btnOk.Top = 100;
            dialog.Controls.Add(btnOk);

            dialog.AcceptButton = btnOk;

            // Show as dialog
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string enteredCode = txtCode.Text;
                string enteredRole = txtRole.Text;
                SaveOrUpdateRole(0, enteredCode, enteredRole);
            }

            dialog.Dispose();

        }
        private void SaveOrUpdateRole(int _id, string code, string name)
        {
            if (_id > 0)
            {
                string updateQuery = "UPDATE tbl_project_role SET name = @name,code = @code WHERE id = @id";
                int res = DBClass.ExecuteNonQuery(updateQuery,
                    DBClass.CreateParameter("@id", _id),
                    DBClass.CreateParameter("@name", name),
                    DBClass.CreateParameter("@code", code));
                Utilities.LogAudit(frmLogin.userId, "Update Project Role", "Project Role", _id, "Updated Project Role: " + name);
                if (res > 0)
                {
                    MessageBox.Show("Role updated successfully.");
                    loadCombo();
                }
            }
            else
            {
                string insertQuery = "INSERT INTO tbl_project_role (code, name) VALUES (@code, @name)";
                int res = DBClass.ExecuteNonQuery(insertQuery,
                    DBClass.CreateParameter("@code", code),
                    DBClass.CreateParameter("@name", name));
                Utilities.LogAudit(frmLogin.userId, "Insert Project Role", "Project Role", res, "Inserted Project Role: " + name);
                if (res > 0)
                {
                    MessageBox.Show("Role inserted successfully.");
                    loadCombo();
                }
            }
        }
    }
}

