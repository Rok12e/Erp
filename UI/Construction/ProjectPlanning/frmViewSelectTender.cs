using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewSelectTender : Form
    {

        public int SelectedTenderId { get; private set; }
        private int id,pId;

        public frmViewSelectTender( int id=0,int _pId=0)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);

            this.id = id;
            this.pId = _pId;
            if (id == 0)
                this.Text = id == 0 ? "Tender List" : "Tender List";
            headerUC1.FormText = this.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //
            this.Close();
        }

        private void dgvItems_DoubleClick(object sender, EventArgs e)
        {
            if (dgvItems.SelectedRows.Count > 0)
            {
                SelectedTenderId = Convert.ToInt32(dgvItems.SelectedRows[0].Cells["id"].Value);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void frmViewSelectTender_Load(object sender, EventArgs e)
        {
            if (id != 0)
            {
                DataTable  dt = DBClass.ExecuteDataTable("select id,date,amount from tbl_project_tender WHERE project_id=@pId AND tender_name_id=@id",
                      DBClass.CreateParameter("id", id),
                      DBClass.CreateParameter("pId", pId));
                dgvItems.DataSource = dt;
                dgvItems.Columns["date"].HeaderText = "Date";
                dgvItems.Columns["amount"].HeaderText = "Amount";
                //dgvItems.Columns["id"].Visible = false;
                dgvItems.Columns["date"].ReadOnly = true;
                dgvItems.Columns["amount"].ReadOnly = true;
            }
        }
    }
}
