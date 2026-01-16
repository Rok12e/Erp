using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewWarehouse : Form
    {

        private int id;

        public frmViewWarehouse(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            headerUC1.FormText = id == 0 ? "New Warehouse" : "Edit Warehouse";
            btnSave.Text = id == 0 ? "Save" : "Update";
        }
      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string GenerateNextSalesCode()
        {
            string newCode = "WH-0001";

            using (var reader = DBClass.ExecuteReader("SELECT MAX(CAST(SUBSTRING(code, 4) AS UNSIGNED)) AS lastCode FROM tbl_warehouse"))
            {
                if (reader.Read() && reader["lastCode"] != DBNull.Value)
                {
                    int code = int.Parse(reader["lastCode"].ToString()) + 1;
                    newCode = "WH-" + code.ToString("D4");
                }
            }

            return newCode;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Name First");
                return;
            }
            //if (cmbEmp.SelectedValue==null)
            //{
            //    MessageBox.Show("Please Enter Manager Name First");
            //    return;
            //}

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_warehouse where name = @name",
                DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                    {
                        MessageBox.Show(" Name Already In Use. Enter Another Name.");
                        return;
                    }
                }

            if (id == 0)
            {
                int refId = DBClass.ExecuteNonQuery(@"INSERT INTO `tbl_warehouse` (code, name, emp_id, city, building_name,
                                        account_id, state, created_by, created_date) VALUES (@code, @name, @emp_id,
                                        @city, @building_name, @account_id, 0, @created_by, @created_date);",
                            DBClass.CreateParameter("@code", GenerateNextSalesCode()),
                            DBClass.CreateParameter("@name", txtName.Text),
                            DBClass.CreateParameter("@emp_id", cmbEmp.SelectedValue ?? 0),
                            DBClass.CreateParameter("@city", cmbCity.Text),
                            DBClass.CreateParameter("@building_name", txtBuildingName.Text),
                            DBClass.CreateParameter("@account_id", cmbMainAccount.SelectedValue),
                            DBClass.CreateParameter("@created_by", frmLogin.userId),
                            DBClass.CreateParameter("@created_date", DateTime.Now.Date));
                Utilities.LogAudit(frmLogin.userId, "Add Item Warehouse", "Item Warehouse", refId, "Added Item Warehouse: " + txtName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE `tbl_warehouse` SET name = @name, emp_id = @emp_id, city = @city, 
                                         building_name = @building_name, account_id = @account_id WHERE id = @id;",
                        DBClass.CreateParameter("@name", txtName.Text),
                        DBClass.CreateParameter("@emp_id", cmbEmp.SelectedValue ?? 0),
                        DBClass.CreateParameter("@city", cmbCity.Text),
                        DBClass.CreateParameter("@building_name", txtBuildingName.Text),
                        DBClass.CreateParameter("@account_id", cmbMainAccount.SelectedValue),
                        DBClass.CreateParameter("@id", id));

                Utilities.LogAudit(frmLogin.userId, "Update Item Warehouse", "Item Warehouse", id, "Updated Item Warehouse: " + txtName.Text);
            }
            EventHub.RefreshWarehouse();
            this.Close();
        }

        private void frmViewWarehouse_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateEmployees(cmbEmp);
            BindCombos.PopulateAllLevel4Account(cmbMainAccount);
            BindCombos.PopulateCities(cmbCity, 49);
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_warehouse where id = @id",
                      DBClass.CreateParameter("id", id)))
                {
                    if (reader.Read() && reader.HasRows)
                    {
                        txtCode.Text = reader["code"].ToString();
                        txtName.Text = reader["name"].ToString();
                        txtBuildingName.Text = reader["building_name"].ToString();
                        cmbCity.Text = reader["city"].ToString();
                        cmbEmp.SelectedValue = int.Parse(reader["emp_id"].ToString());
                        cmbMainAccount.SelectedValue = int.Parse(reader["account_id"].ToString());
                    }
                }
            }
        }
        private void addEmployee_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //frmLogin.frmMain.openChildForm(new frmViewEmployee());
            frmViewEmployee emp = new frmViewEmployee();
            emp.ShowDialog();
        }

        private void addCity_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //(new frmViewCity());
            frmViewCity emp = new frmViewCity();
            emp.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
