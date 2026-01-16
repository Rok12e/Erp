using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class frmViewVATConfiguration : Form
    {

        public frmViewVATConfiguration()
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            this.Text = "VAT Configuration - New VAT Configuration";
            headerUC1.FormText = this.Text;
        }

       
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmViewVATConfiguration_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
                if (txtRegistrationNo.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter TAX Registration No First");
                return;
            }

            if (txtCorporateTaxNo.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Corporate Tax No First");
                return;
            }
            DBClass.ExecuteNonQuery(@"
                        INSERT INTO `tbl_vat_configration` (
                            `registration_no`, `TRNIssue_date`, `quarter_one_start_date`, `quarter_one_end_date`, 
                            `quarter_one_due_date`, `quarter_two_start_date`, `quarter_two_end_date`, `quarter_two_due_date`, 
                            `quarter_three_start_date`, `quarter_three_end_date`, `quarter_three_due_date`, 
                            `quarter_four_start_date`, `quarter_four_end_date`, `quarter_four_due_date`
                        ) 
                        VALUES (
                            @registration_no, @TRNIssue_date, @quarter_one_start_date, @quarter_one_end_date, 
                            @quarter_one_due_date, @quarter_two_start_date, @quarter_two_end_date, @quarter_two_due_date, 
                            @quarter_three_start_date, @quarter_three_end_date, @quarter_three_due_date, 
                            @quarter_four_start_date, @quarter_four_end_date, @quarter_four_due_date
                        );",
    
                    DBClass.CreateParameter("@registration_no", txtRegistrationNo.Text),
                    DBClass.CreateParameter("@TRNIssue_date", dtpTRNIssueDate.Value.Date),
                    DBClass.CreateParameter("@quarter_one_start_date", dtpQuarterOneStartDate.Value.Date),
                    DBClass.CreateParameter( "@quarter_one_end_date", dtpQuarterOneEndDate.Value.Date),
                    DBClass.CreateParameter("@quarter_one_due_date", dtpQuarterOneDueDate.Value.Date), 
                    DBClass.CreateParameter("@quarter_two_start_date", dtpQuarterTwoStartDate.Value.Date),
                    DBClass.CreateParameter("@quarter_two_end_date", dtpQuarterTwoEndDate.Value.Date),
                    DBClass.CreateParameter("@quarter_two_due_date", dtpQuarterTwoDueDate.Value.Date),
                    DBClass.CreateParameter("@quarter_three_start_date", dtpQuarterThreeStartDate.Value.Date),
                    DBClass.CreateParameter("@quarter_three_end_date", dtpQuarterThreeEndDate.Value.Date),
                    DBClass.CreateParameter("@quarter_three_due_date", dtpQuarterThreeDueDate.Value.Date),
                    DBClass.CreateParameter("@quarter_four_start_date", dtpQuarterFourStartDate.Value.Date),
                    DBClass.CreateParameter("@quarter_four_end_date", dtpQuarterFourEndDate.Value.Date),
                    DBClass.CreateParameter("@quarter_four_due_date", dtpQuarterFourDueDate.Value.Date));

                        DBClass.ExecuteNonQuery(@"
                INSERT INTO `tbl_corporate_tax_configration` (
                    `corporateTax_no`, `trn_issue_date`, `corporatetax_start_date`,     
                    `corporatetax_end_date`, `corporatetax_due_date`
                ) 
                VALUES (
                    @corporateTax_no, @trn_issue_date, @corporatetax_start_date, 
                    @corporatetax_end_date, @corporatetax_due_date
                );", 
                DBClass.CreateParameter("corporateTax_no", txtCorporateTaxNo.Text),
                DBClass.CreateParameter("trn_issue_date", dtpTRNIssueDate.Value.Date),
                DBClass.CreateParameter("corporatetax_start_date", dtpCorporateTaxStartDate.Value.Date),
                DBClass.CreateParameter("corporatetax_end_date", dtpCorporateTaxEndDate.Value.Date),
                DBClass.CreateParameter("corporatetax_due_date", dtpCorporateTaxDueDate.Value.Date)
            );
            MessageBox.Show("Date Save Successfully");
            ResetForm();
        }

        private void ResetForm()
        {

            txtRegistrationNo.Text = txtCorporateTaxNo.Text = "";
            dtpTRNIssueDate.Value = DateTime.Today;
            dtpQuarterOneStartDate.Value = DateTime.Today;
            dtpQuarterOneEndDate.Value = DateTime.Today;
            dtpQuarterOneDueDate.Value = DateTime.Today;
            dtpQuarterTwoStartDate.Value = DateTime.Today;
            dtpQuarterTwoEndDate.Value = DateTime.Today;
            dtpQuarterTwoDueDate.Value = DateTime.Today;
            dtpQuarterThreeStartDate.Value = DateTime.Today;
            dtpQuarterThreeEndDate.Value = DateTime.Today;
            dtpQuarterThreeDueDate.Value = DateTime.Today;
            dtpQuarterFourStartDate.Value = DateTime.Today;
            dtpQuarterFourEndDate.Value = DateTime.Today;
            dtpQuarterFourDueDate.Value = DateTime.Today;
            dtpCorporateTaxStartDate.Value = DateTime.Today;
            dtpCorporateTaxEndDate.Value = DateTime.Today;
            dtpCorporateTaxDueDate.Value = DateTime.Today;
        }


        private void dtpQuarterOneStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpQuarterOneEndDate.Value = dtpQuarterOneStartDate.Value.Date.AddMonths(3).AddDays(-1);
            dtpQuarterOneDueDate.Value = dtpQuarterOneEndDate.Value.Date.AddDays(28);

            dtpQuarterTwoStartDate.Value = dtpQuarterOneEndDate.Value.Date.AddDays(1);
            dtpQuarterTwoEndDate.Value = dtpQuarterTwoStartDate.Value.Date.AddMonths(3).AddDays(-1);
            dtpQuarterTwoDueDate.Value = dtpQuarterTwoEndDate.Value.Date.AddDays(28);

            dtpQuarterThreeStartDate.Value = dtpQuarterTwoEndDate.Value.Date.AddDays(1);
            dtpQuarterThreeEndDate.Value = dtpQuarterThreeStartDate.Value.Date.AddMonths(3).AddDays(-1);
            dtpQuarterThreeDueDate.Value = dtpQuarterThreeEndDate.Value.Date.AddDays(28);

            dtpQuarterFourStartDate.Value = dtpQuarterThreeEndDate.Value.Date.AddDays(1);
            dtpQuarterFourEndDate.Value = dtpQuarterFourStartDate.Value.Date.AddMonths(3).AddDays(-1);
            dtpQuarterFourDueDate.Value = dtpQuarterFourEndDate.Value.Date.AddDays(28);
        }


        private void dtpCorporateTaxStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpCorporateTaxEndDate.Value = dtpCorporateTaxStartDate.Value.Date.AddYears(1).AddDays(-1);
            dtpCorporateTaxDueDate.Value = dtpCorporateTaxEndDate.Value.AddDays(28); 
        }

    }
}
