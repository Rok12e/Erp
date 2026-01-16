using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmCheckStateDate : Form
    {
        private int id;
        private string state;
        private string type;
        private string code;
        private DateTime? passDate; // ✅ Nullable DateTime
        public DateTime SelectedDate { get; private set; }
        public frmCheckStateDate(string type, string code, int id, DateTime checkdate, string name, string state, DateTime? passDate = null)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.id = id;
            this.state = state;
            this.type = type;
            this.passDate = passDate;
            this.code = code;
            dtCheck.Value = checkdate;
            txtCheckName.Text = name;
            lblState.Text = state + " Date";
            headerUC1.FormText = "Check Date";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SelectedDate = dtState.Value;

            if (dtState.Value.Date < dtCheck.Value.Date)
            {
                MessageBox.Show("Enter a valid date");
                return;
            }

            // ✅ Safe check for passDate using Nullable logic
            if (passDate.HasValue && passDate.Value > dtState.Value.Date)
            {
                MessageBox.Show("Return Date must be greater than or equal to Pass Date");
                return;
            }

            // ✅ Save new check status
            DBClass.ExecuteNonQuery(
                "UPDATE tbl_check_details SET " + state + "_date = @date, state = @state WHERE id = @id",
                DBClass.CreateParameter("date", dtState.Value.Date),
                DBClass.CreateParameter("state", state),
                DBClass.CreateParameter("id", id)
            );
            Utilities.LogAudit(frmLogin.userId, "Update Check Date", "Check", id, $"{state} Date: {dtState.Value.Date}, Type: {type}, Code: {code}");

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void frmCheckStateDate_Load(object sender, EventArgs e)
        {
            if (passDate == null || state == "Return")
            {
                dtState.Value = DateTime.Now;
            }
        }
    }
}
