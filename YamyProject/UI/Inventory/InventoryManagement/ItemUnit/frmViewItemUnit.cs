using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewItemUnit : Form
    {
        private int id;
        public frmViewItemUnit(int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            headerUC1.FormText = id == 0 ? "Inventory - Add Item Unit" : "Inventory - Edit Item Unit";
        }
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewItemUnit_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_unit where id =@id", DBClass.CreateParameter("id", id)))
                {
                    reader.Read();
                    txtName.Text = reader["name"].ToString();
                }
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Item Unit First");
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_unit where name = @name",
                DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                    {
                        MessageBox.Show("Item Unit Already Exists. Enter Another Name.");
                        return;
                    }
                }
            if (id == 0)
            {
                int refId = DBClass.ExecuteNonQuery("insert into tbl_unit (name) values(@name)",
                    DBClass.CreateParameter("name", txtName.Text));
                Utilities.LogAudit(frmLogin.userId, "Add Item Unit", "Item Unit", refId, "Added Item Unit: " + txtName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery("update tbl_unit set name=@name where id = @id",
                    DBClass.CreateParameter("id", id),
                    DBClass.CreateParameter("name", txtName.Text));
                Utilities.LogAudit(frmLogin.userId, "Update Item Unit", "Item Unit", id, "Updated Item Unit: " + txtName.Text);
            }
            EventHub.RefreshItemUnit();
            this.Close();
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
