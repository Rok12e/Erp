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
    public partial class frmCostCenter : Form
    {
        int id, projectId=0;
        EventHandler editMain;
        public frmCostCenter(int id, int _projectId=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            editMain = (sender, args) => BindCombo();
            EventHub.MainCostCenter += editMain;
            this.id = id;
            headerUC1.FormText = this.Text;

            if (id == 0)
            {
                if (_projectId != 0)
                {
                    projectId = _projectId;
                    radioSub.Checked = true;
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCostCenter_Load(object sender, EventArgs e)
        {
            BindCombo();
            radioMain.Checked = true;
            cmbCostCenter.Visible = false;
            lblMain.Visible = false;
        }

        private void BindCombo()
        {
            BindCombos.PopulateCostCenter(cmbCostCenter);
            EventHub.Refreshlvl1Account();
            EventHub.Refreshlvl2Account();
            if(selectedIndex>0)
             cmbCostCenter.SelectedIndex = selectedIndex;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!radioMain.Checked && !radioSub.Checked)
            {
                MessageBox.Show("Choose one Account first");
                return;
            }
            if (insertCost())
            {
                EventHub.Refreshlvl1Account();
                EventHub.Refreshlvl2Account();
                this.Close();
            }
            else
            {
                if (updateCost())
                EventHub.Refreshlvl1Account();
                EventHub.Refreshlvl2Account();
                this.Close();
            }
        }

        private bool updateCost()
        {
            return true;
        }

        private bool insertCost()
        {
            int mainCode = 101;
            if (txtName.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Account Name First.");
                return false;
            }

            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_cost_center WHERE name = @name",
                DBClass.CreateParameter("name", txtName.Text)))
                if (reader.Read())
                {
                    MessageBox.Show("Account Name Already Exists. Enter Another Name.");
                    return false;
                }

            if (radioMain.Checked)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader(@"SELECT code FROM tbl_cost_center ORDER BY 
                CAST(SUBSTRING_INDEX(code, '-', -1) AS UNSIGNED) DESC LIMIT 1;"))
                {

                    if (reader.Read() && reader["code"].ToString() != "")
                        mainCode = int.Parse(reader["code"].ToString()) + 1;

                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_cost_center (name, code, project_id)
                                  VALUES (@name, @code, @project_id);",
                        DBClass.CreateParameter("name", txtName.Text),
                        DBClass.CreateParameter("code", mainCode),
                        DBClass.CreateParameter("project_id", projectId));
                    Utilities.LogAudit(frmLogin.userId, "Insert Cost Center", "Cost Center", mainCode, "Inserted Cost Center: " + txtName.Text);
                }
                return true;
            }
            else if (radioSub.Checked)
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT code FROM tbl_cost_center WHERE id = @id",
                    DBClass.CreateParameter("id", cmbCostCenter.SelectedValue.ToString())))
                {
                    if (!reader.Read()) return false;
                    string mainCostCode = reader["code"].ToString();

                    int newSubCode = 1;
                    using (MySqlDataReader r = DBClass.ExecuteReader("SELECT MAX(code) FROM tbl_sub_cost_center WHERE code LIKE @mainCode",
                        DBClass.CreateParameter("mainCode", mainCostCode + "%")))
                        if (r.Read() && r[0] != DBNull.Value)
                        {
                            string lastCode = r[0].ToString();
                            newSubCode = int.Parse(lastCode.Substring(mainCostCode.Length)) + 1;
                        }

                    string formattedSubCode = mainCostCode + newSubCode.ToString("D3");

                    DBClass.ExecuteNonQuery(@"INSERT INTO tbl_sub_cost_center (code, name, main_id, project_id)
                                  VALUES (@code, @name, @main_id, @project_id);",
                        DBClass.CreateParameter("code", formattedSubCode),
                        DBClass.CreateParameter("name", txtName.Text),
                        DBClass.CreateParameter("project_id", projectId),
                        DBClass.CreateParameter("main_id", cmbCostCenter.SelectedValue.ToString()));
                    Utilities.LogAudit(frmLogin.userId, "Insert Sub Cost Center", "Sub Cost Center", newSubCode, "Inserted Sub Cost Center: " + txtName.Text);
                    return true;
                }
            }

            return false;
        }

        private void lnkLvl1Edit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(cmbCostCenter.SelectedValue==null || cmbCostCenter.Text == "")
            {
                MessageBox.Show("Select Level 1 First.");
                return;
            }
        }

        private void radioSub_CheckedChanged(object sender, EventArgs e)
        {
            if (radioSub.Checked)
            {
                cmbCostCenter.Visible = true;
                lblMain.Visible = true;
            }
            else
            {
                cmbCostCenter.Visible = false;
                lblMain.Visible = false;
            }
        }

        int selectedIndex=0;
        private void lnkEditCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (cmbCostCenter.SelectedValue == null)
                return;
            new frmViewEditMain(int.Parse(cmbCostCenter.SelectedValue.ToString())).ShowDialog();
            selectedIndex = cmbCostCenter.SelectedIndex;
        }

        private void LbHeader_Click(object sender, EventArgs e)
        {
            this.BringToFront();
        }
    }
}
