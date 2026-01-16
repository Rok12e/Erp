using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterUsers : Form
    {
        private DataView _dataView;
        private EventHandler userUpdatedHandler;
        private EventHandler roleUpdatedHandler;

        public MasterUsers(bool Case=true)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            userUpdatedHandler = roleUpdatedHandler=(sender, args) => BindUsers();
            EventHub.User += userUpdatedHandler;
            EventHub.Roles += roleUpdatedHandler;
            headerUC1.FormText = Text;
        }
        private void MasterUsers_FormClosing(object sender, FormClosingEventArgs e)
        {
            EventHub.User -= userUpdatedHandler;
            EventHub.Roles -= roleUpdatedHandler;
        }
        private void MasterUsers_Load(object sender, EventArgs e)
        {
            ConfigureDataGridViews();
            BindUsers();
        }
        private void ConfigureDataGridViews()
        {

            dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            dgvRoles.EnableHeadersVisualStyles = false;
            dgvRoles.RowsDefaultCellStyle.BackColor = Color.White;
            dgvRoles.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");
            dgvRoles.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#D1EAD0");
            dgvRoles.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvRoles.BorderStyle = BorderStyle.None;
            dgvRoles.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvRoles.RowHeadersVisible = false;
            dgvRoles.AllowUserToAddRows = false;
        }
        public void BindUsers()
        {
            string query = @"SELECT u.id,u.User_Name as 'User Name',
                         r.name ,first_name,last_name,role_id
                            FROM tbl_sec_users u
                            LEFT JOIN tbl_sec_roles r ON u.role_id = r.id WHERE 1 = 1";
            if (cmbState.Text == "Active User")
                query += " AND u.active = 0";
            else if (cmbState.Text == "Inactive User")
                query += " AND u.active != 0";

            DataTable dt = DBClass.ExecuteDataTable(query);
            _dataView = dt.DefaultView;
            dgvUsers.DataSource = _dataView;
            dgvUsers.Columns["user name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            HideUnusedColumns();
            if (dgvUsers.Rows.Count > 0)
                BindUserRoles();
            else
                ClearCustomerDetails();

            LocalizationManager.LocalizeDataGridViewHeaders(dgvUsers);
        }
        private void BindUserRoles()
        {
            //
        }
        private void HideUnusedColumns()
        {
            string[] hiddenColumns = { "id", "first_name", "last_name" , "role_id" };
            foreach (var col in hiddenColumns)
                dgvUsers.Columns[col].Visible = false;
        }
        
        private void ClearCustomerDetails()
        {
            dgvRoles.DataSource = null;
            lblFirstName.Text = lblLastName.Text = "-";
        }
        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindUsers();
        }
        private void nToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmViewUser().ShowDialog();
        }
        private void editCustomerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvUsers.Rows.Count == 0)
                return;
            new frmViewUser(int.Parse(dgvUsers.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }
        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //BindUserInfo();
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            _dataView.RowFilter = "first_name like '%" + txtSearch.Text + "%' or last_name like '%" + txtSearch.Text + "%' or [user name] like '%" + txtSearch.Text + "%'";
        }

        private void dgvCustomer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvUsers.Rows.Count == 0)
                return;
            new frmViewUser(int.Parse(dgvUsers.SelectedRows[0].Cells["id"].Value.ToString())).ShowDialog();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            new frmViewRole().ShowDialog();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (dgvUsers.Rows.Count == 0)
                return;
            new frmViewRole(int.Parse(dgvUsers.SelectedRows[0].Cells["role_id"].Value.ToString())).ShowDialog();
        }

        private void manageUserRoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void changeUserPasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

public class UserModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Status { get; set; }

    public UserModel() { }

    public UserModel(int id, string name, bool status)
    {
        Id = id;
        Name = name;
        Status = status;
    }
}
