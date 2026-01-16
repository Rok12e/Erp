using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewFixedAssetsCategory : Form
    {
        private int id;
        public frmViewFixedAssetsCategory(int id=0)
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

        private void frmViewFixedAssetsCategory_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateAllLevel4Account(cmbAssetsAccount);
            BindCombos.PopulateAllLevel4Account(cmbDepreciationAccount);
            BindCombos.PopulateAllLevel4Account(cmbExpenceAccount);

            DataTable dt = DBClass.ExecuteDataTable("select * from tbl_fixed_assets_category");
            cmbCategoryName.ValueMember = "id";
            cmbCategoryName.DisplayMember = "category_name";
            cmbCategoryName.DataSource = dt;
            cmbCategoryName.SelectedIndex = -1;

            if (dt.Rows.Count > 0)
            {
                AutoCompleteStringCollection autoSource = new AutoCompleteStringCollection();
                foreach (DataRow row in dt.Rows)
                {
                    autoSource.Add(row["category_name"].ToString());
                }
                cmbCategoryName.AutoCompleteCustomSource = autoSource;
            }

            if (id != 0)
            {
                BindData();
            }
        }
        private void BindData()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_fixed_assets_category where id =@id", DBClass.CreateParameter("id", id)))
            {
                reader.Read();
                cmbCategoryName.Text = reader["category_name"].ToString();

                if (reader["assets_account_id"] != DBNull.Value && int.Parse(reader["assets_account_id"].ToString()) > 0)
                    cmbAssetsAccount.SelectedValue = int.Parse(reader["assets_account_id"].ToString());

                if (reader["depreciation_account_id"] != DBNull.Value && int.Parse(reader["depreciation_account_id"].ToString()) > 0)
                    cmbDepreciationAccount.SelectedValue = int.Parse(reader["depreciation_account_id"].ToString());

                if (reader["expence_account_id"] != DBNull.Value && int.Parse(reader["expence_account_id"].ToString()) > 0)
                    cmbExpenceAccount.SelectedValue = int.Parse(reader["expence_account_id"].ToString());

            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmbCategoryName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Category Name First");
                return;
            }
            using(MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_fixed_assets_category where category_name = @category_name",
                DBClass.CreateParameter("category_name", cmbCategoryName.Text)))
            if (reader.Read())
            {
                if (id == 0 || (id != int.Parse(reader["id"].ToString())))
                {
                    reader.Dispose();
                    MessageBox.Show("Category Already Exists. Enter Another Name.");
                    return;
                }
            }
            if (id == 0)
                DBClass.ExecuteNonQuery(@"
                        INSERT INTO tbl_fixed_assets_category (
                            category_name, assets_account_id, depreciation_account_id, expence_account_id
                        ) VALUES (
                            @category_name, @assets_account_id, @depreciation_account_id, @expence_account_id
                        );",
                        DBClass.CreateParameter("category_name", cmbCategoryName.Text),
                        DBClass.CreateParameter("assets_account_id", cmbAssetsAccount.SelectedValue),
                        DBClass.CreateParameter("depreciation_account_id", cmbDepreciationAccount.SelectedValue),
                        DBClass.CreateParameter("expence_account_id", cmbExpenceAccount.SelectedValue)
                );
            else
                DBClass.ExecuteNonQuery(@"
                    UPDATE tbl_fixed_assets_category 
                    SET 
                        category_name = @category_name, 
                        assets_account_id = @assets_account_id, 
                        depreciation_account_id = @depreciation_account_id, 
                        expence_account_id = @expence_account_id 
                    WHERE id = @id;",
                    DBClass.CreateParameter("id", id),
                    DBClass.CreateParameter("category_name", cmbCategoryName.Text),
                    DBClass.CreateParameter("assets_account_id", cmbAssetsAccount.SelectedValue),
                    DBClass.CreateParameter("depreciation_account_id", cmbDepreciationAccount.SelectedValue),
                    DBClass.CreateParameter("expence_account_id", cmbExpenceAccount.SelectedValue)
                );

            EventHub.RefreshFixedAssetsCategory();
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DBClass.ExecuteNonQuery(@"DELETE from tbl_fixed_assets_category WHERE id = @id;", DBClass.CreateParameter("id", id));
            id = 0;
            MessageBox.Show("Category Deleted");
            EventHub.RefreshFixedAssetsCategory();
        }

        private void cmbAssetsAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAssetsAccount.SelectedValue == null)
            {
                txtAccountCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbAssetsAccount.SelectedValue.ToString()))
                if (reader.Read())
                    txtAccountCode.Text = reader["code"].ToString();
                else
                    txtAccountCode.Text = "";

        }

        private void cmbDepreciationAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDepreciationAccount.SelectedValue == null)
            {
                txtCreditAccountCode.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbDepreciationAccount.SelectedValue.ToString()))
                if (reader.Read())
                    txtCreditAccountCode.Text = reader["code"].ToString();
                else
                    txtCreditAccountCode.Text = "";

        }

        private void cmbExpenceAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbExpenceAccount.SelectedValue == null)
            {
                txtExpenceAccount.Text = "";
                return;
            }
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where id = " + cmbExpenceAccount.SelectedValue.ToString()))
                if (reader.Read())
                    txtExpenceAccount.Text = reader["code"].ToString();
                else
                    txtExpenceAccount.Text = "";

        }

        private void txtAccountCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtAccountCode.Text)))
                if (reader.Read())
                    cmbAssetsAccount.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtCreditAccountCode_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtCreditAccountCode.Text)))
                if (reader.Read())
                    cmbDepreciationAccount.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void txtExpenceAccount_TextChanged(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_coa_level_4 where code =@code",
                DBClass.CreateParameter("code", txtExpenceAccount.Text)))
                if (reader.Read())
                    cmbExpenceAccount.SelectedValue = int.Parse(reader["id"].ToString());
        }

        private void cmbCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategoryName.Focused)
            {
                if (cmbCategoryName.SelectedValue == null)
                {
                    txtCategoryCode.Text = "";
                    return;
                }

                if (cmbCategoryName.SelectedValue != null && int.Parse(cmbCategoryName.SelectedValue.ToString()) != 0)
                {
                    object obj = DBClass.ExecuteScalar(@"select id from tbl_fixed_assets_category where id =@id",
                                DBClass.CreateParameter("@id", cmbCategoryName.SelectedValue.ToString()));
                    int oldId = (obj != null && obj != DBNull.Value) ? int.Parse(obj.ToString()) : 0;
                    if (oldId > 0)
                    {
                        id = oldId;
                        BindData();
                    }
                    //using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_fixed_assets_category where id =@id", DBClass.CreateParameter("id", cmbCategoryName.SelectedValue.ToString())))
                    //{
                    //    if (reader.Read())
                    //    {
                    //        id = int.Parse(reader["id"].ToString());
                    //        txtCategoryCode.Text = reader["code"].ToString();
                    //        if (reader["assets_account_id"] != DBNull.Value && int.Parse(reader["assets_account_id"].ToString()) > 0)
                    //            cmbAssetsAccount.SelectedValue = int.Parse(reader["assets_account_id"].ToString());

                    //        if (reader["depreciation_account_id"] != DBNull.Value && int.Parse(reader["depreciation_account_id"].ToString()) > 0)
                    //            cmbDepreciationAccount.SelectedValue = int.Parse(reader["depreciation_account_id"].ToString());

                    //        if (reader["expence_account_id"] != DBNull.Value && int.Parse(reader["expence_account_id"].ToString()) > 0)
                    //            cmbExpenceAccount.SelectedValue = int.Parse(reader["expence_account_id"].ToString());
                    //    }
                    //    else
                    //    {
                    //        txtCategoryCode.Text = "";
                    //    }
                    //}
                }
            }
        }

        private void Lbheader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

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
    }
}
