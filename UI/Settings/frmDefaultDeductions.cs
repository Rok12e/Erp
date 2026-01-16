using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmDefaultDeductions : Form
    {
        public frmDefaultDeductions()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
        }

        private void frmDefaultDeductions_Load(object sender, EventArgs e)
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("Select missedhoursdeduction, latearrivaldeduction, fulldaydeduction,delaytime from tbl_setting_deduction_config LIMIT 1"))
                if (reader.Read())
                {
                    txtMissedHours.Text = reader["missedhoursdeduction"].ToString();
                    txtLateArrival.Text = reader["latearrivaldeduction"].ToString();
                    txtFullDay.Text = reader["fulldaydeduction"].ToString();
                    txtdelaytime.Text = reader["delaytime"].ToString();
                }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtMissedHours.Text.Trim() == "")
                txtMissedHours.Text = "0";
            if (txtFullDay.Text.Trim() == "")
                txtMissedHours.Text = "0";
            if (txtLateArrival.Text.Trim() == "")
                txtMissedHours.Text = "0";

            DBClass.ExecuteNonQuery(@"UPDATE tbl_setting_deduction_config SET missedhoursdeduction = @missedhoursdeduction, 
                                     latearrivaldeduction = @latearrivaldeduction, fulldaydeduction = @fulldaydeduction, delaytime=@delaytime",
                                     DBClass.CreateParameter("missedhoursdeduction", txtMissedHours.Text),
                                     DBClass.CreateParameter("latearrivaldeduction", txtLateArrival.Text),
                                     DBClass.CreateParameter("fulldaydeduction", txtFullDay.Text),
                                     DBClass.CreateParameter("delaytime", txtdelaytime.Text));

            MessageBox.Show("Deductions Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}