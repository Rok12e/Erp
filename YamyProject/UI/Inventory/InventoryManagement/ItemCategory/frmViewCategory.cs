using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewCategory : Form
    {
      
        private int id;

        public frmViewCategory( int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            if (id == 0)
                this.Text = id == 0 ? "New Category" : "Edit Category";
            headerUC1.FormText = this.Text;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Name First");
                return;
            }

            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_item_category where name = @name",
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
                object result = DBClass.ExecuteScalar("SELECT LPAD(MAX(CAST(code AS UNSIGNED)), 3, '0') FROM tbl_item_category;");
                int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                string formattedId = nextId.ToString().PadLeft(3, '0');
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_item_category (code, name) VALUES (@code, @name);",
                            DBClass.CreateParameter("@code", formattedId),
                            DBClass.CreateParameter("@name", txtName.Text));
                Utilities.LogAudit(frmLogin.userId, "Add Item Category", "Item Category", nextId, "Added Item Category: " + txtName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_item_category SET  name = @name WHERE id = @id;",
                        DBClass.CreateParameter("@name", txtName.Text),
                        DBClass.CreateParameter("@id", id));
                Utilities.LogAudit(frmLogin.userId, "Update Item Category", "Item Category", id, "Updated Item Category: " + txtName.Text);
            }

            EventHub.RefreshItemCategory();

            this.Close();
            //if (txtName.Text.Trim() == "")
            //{
            //    MessageBox.Show("Please Enter Name First");
            //    return;
            //}

            //string actionType = (id == 0) ? "save" : "edit";

            //var parameters = new List<MySqlParameter>
            //{
            //    new MySqlParameter("p_action", actionType),
            //    new MySqlParameter("p_id", id),
            //    new MySqlParameter("p_name", txtName.Text.Trim()),
            //    new MySqlParameter("p_new_id", MySqlDbType.Int32) { Direction = ParameterDirection.Output },
            //    new MySqlParameter("p_code", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Output },
            //    new MySqlParameter("p_duplicate", MySqlDbType.Bit) { Direction = ParameterDirection.Output }
            //};

            //try
            //{
            //    DBClass.ExecuteStoredProcedure("sp_item_category", parameters);

            //    bool isDuplicate = Convert.ToBoolean(parameters.Find(p => p.ParameterName == "p_duplicate").Value);
            //    if (isDuplicate)
            //    {
            //        MessageBox.Show("Name Already In Use. Enter Another Name.");
            //        return;
            //    }

            //    int newId = Convert.ToInt32(parameters.Find(p => p.ParameterName == "p_new_id").Value);
            //    string code = parameters.Find(p => p.ParameterName == "p_code").Value?.ToString();

            //    string action = (id == 0) ? "Add" : "Update";
            //    string logMessage = $"{action}d Item Category: {txtName.Text}";

            //    Utilities.LogAudit(frmLogin.userId, $"{action} Item Category", "Item Category", newId, logMessage);

            //    EventHub.RefreshItemCategory();
            //    this.Close();
            //}
            //catch (MySqlException ex)
            //{
            //    MessageBox.Show("MySQL Error: " + ex.Message);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("General Error: " + ex.Message);
            //}

        }

        private void frmViewCategory_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_item_category where id = @id",
                      DBClass.CreateParameter("id", id)))
                    if(reader.Read())
                        txtName.Text = reader["name"].ToString();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void panel6_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
