using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewPrepaidExpenseCategory : Form
    {
        private int id;
        public frmViewPrepaidExpenseCategory(int id)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;

            headerUC1.FormText = this.Text;
        }

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewPrepaidExpenseCategory_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_customer_category where id =@id", DBClass.CreateParameter("id", id)))
                    if (reader.Read())
                        txtCategoryName.Text = reader["name"].ToString();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCategoryName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Category Name First");
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_prepaid_expense_category where name = @name",
                DBClass.CreateParameter("name", txtCategoryName.Text)))
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
                DBClass.ExecuteNonQuery("insert into tbl_prepaid_expense_category (name) values(@name)",
                    DBClass.CreateParameter("name", txtCategoryName.Text));
                Utilities.LogAudit(frmLogin.userId, "Add Prepaid Expense Category", "Prepaid Expense Category", 0, "Added Prepaid Expense Category: " + txtCategoryName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery("update tbl_prepaid_expense_category set name=@name where id = @id",
                    DBClass.CreateParameter("id", id),
              DBClass.CreateParameter("name", txtCategoryName.Text));
                Utilities.LogAudit(frmLogin.userId, "Update Prepaid Expense Category", "Prepaid Expense Category", id, "Updated Prepaid Expense Category: " + txtCategoryName.Text);
            }
            this.Close();
        }
    }
}
