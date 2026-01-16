using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterPettyCashCategory : Form
    {
        public MasterPettyCashCategory()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        private void MasterPettyCashCategory_Load(object sender, EventArgs e)
        {
            BindPettyCash();
        }

        public void BindPettyCash()
        {
            try
            {
                string query = "SELECT NAME AS 'Category Name' , Description FROM tbl_petty_cash_category";
                DataTable dt = DBClass.ExecuteDataTable(query);

                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data found!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvPetty.SuspendLayout(); 
                dgvPetty.DataSource = dt;

                if (dgvPetty.Columns.Contains("id"))
                    dgvPetty.Columns["id"].Visible = false;

                if (dgvPetty.Columns.Contains("name"))
                {
                    dgvPetty.Columns["Category Name"].MinimumWidth = 180;
                    dgvPetty.Columns["Category Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                if (dgvPetty.Columns.Contains("description"))
                {
                    dgvPetty.Columns["Description"].MinimumWidth = 150;
                    dgvPetty.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                dgvPetty.ColumnHeadersDefaultCellStyle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
                dgvPetty.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
                dgvPetty.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                dgvPetty.EnableHeadersVisualStyles = false;

                dgvPetty.RowsDefaultCellStyle.BackColor = Color.White;
                dgvPetty.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#eaf1fa");

                dgvPetty.DefaultCellStyle.SelectionBackColor = ColorTranslator.FromHtml("#d5dbdb");
                dgvPetty.DefaultCellStyle.SelectionForeColor = Color.Black;

                dgvPetty.BorderStyle = BorderStyle.None;
                dgvPetty.CellBorderStyle = DataGridViewCellBorderStyle.None;
                dgvPetty.RowHeadersVisible = false;
                dgvPetty.AllowUserToAddRows = false;
                dgvPetty.AllowUserToResizeRows = false;

                dgvPetty.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading petty cash data:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPetty_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void dgvPetty_Leave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //if (dgvPetty.Rows.Count == 0)
            //    return;
            //DBClass.ExecuteNonQuery("UPDATE tbl_customer SET state = -1 WHERE id = @id ",
            //                              DBClass.CreateParameter("id", dgvPetty.SelectedRows[0].Cells["id"].Value.ToString()));
            //BindCustomer();
        }

        private void btnNewRequest_Click(object sender, EventArgs e)
        {
            new frmPettyCashCategory().ShowDialog();
        }
    }
}
