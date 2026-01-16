using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmAddTenderName : Form
    {
      
        private int id;
        frmViewProjectTendering master;

        public frmAddTenderName(frmViewProjectTendering _master, int id=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            this.master = _master;
            if (id == 0)
                this.Text = id == 0 ? "New Tender" : "Edit Tender";
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

            MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_tender_names where name = @name",
            DBClass.CreateParameter("name", txtName.Text));
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
                object result = DBClass.ExecuteScalar("SELECT LPAD(MAX(CAST(code AS UNSIGNED)), 3, '0') FROM tbl_tender_names;");
                int nextId = (result == DBNull.Value || result == null) ? 1 : Convert.ToInt32(result) + 1;
                string formattedId = nextId.ToString().PadLeft(3, '0');
                DBClass.ExecuteNonQuery(@"INSERT INTO tbl_tender_names (code, name) VALUES (@code, @name);",
                            DBClass.CreateParameter("@code", formattedId),
                            DBClass.CreateParameter("@name", txtName.Text));

                Utilities.LogAudit(frmLogin.userId, "Add Tender Name", "Tender Name", nextId, "Added Tender Name: " + txtName.Text);
            }
            else
            {
                DBClass.ExecuteNonQuery(@"UPDATE tbl_tender_names SET  name = @name WHERE id = @id;",
                        DBClass.CreateParameter("@name", txtName.Text),
                        DBClass.CreateParameter("@id", id));
                Utilities.LogAudit(frmLogin.userId, "Update Tender Name", "Tender Name", id, "Updated Tender Name: " + txtName.Text);
            }
            if (master == null)
                EventHub.RefreshProject();
            else
                master.BindCombo();

            this.Close();
        }

        private void frmAddTenderName_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_tender_names where id = @id",
                      DBClass.CreateParameter("id", id));
                reader.Read();
                txtName.Text = reader["name"].ToString();
            }
        }
    }
}
