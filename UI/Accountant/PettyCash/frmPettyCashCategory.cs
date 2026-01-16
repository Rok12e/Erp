using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmPettyCashCategory : Form
    {
        private int id;
        public frmPettyCashCategory(int _id =0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = _id;
            if (id != 0)
                this.Text = "Petty Cash Category Edit";
            else
                this.Text = "Petty Cash Category";

            headerUC1.FormText = this.Text;
        }
       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPettyCashCategory_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_petty_cash_category where id =@id", DBClass.CreateParameter("id", id)))
                    if (reader.Read())
                        txtName.Text = reader["name"].ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Category Name First");
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_petty_cash_category where name = @name",
                DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                    {
                        MessageBox.Show("Category Already Exists. Enter Another Name.");
                        return;
                    }
                }
            if (id == 0)
            {
                DBClass.ExecuteNonQuery("insert into tbl_petty_cash_category (name,description) values(@name,@description)",
                    DBClass.CreateParameter("name", txtName.Text),
                    DBClass.CreateParameter("description", txtDescription.Text));
                Utilities.LogAudit(frmLogin.userId, "Create Petty Cash Category", "Petty Cash Category", 0, "Created Petty Cash Category: " + txtName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery("update tbl_petty_cash_category set name=@name,description=@description where id = @id",
                    DBClass.CreateParameter("id", id),
                DBClass.CreateParameter("name", txtName.Text));
                Utilities.LogAudit(frmLogin.userId, "Update Petty Cash Category", "Petty Cash Category", id, "Updated Petty Cash Category: " + txtName.Text);
            }
            this.Close();
        }
    }
}
