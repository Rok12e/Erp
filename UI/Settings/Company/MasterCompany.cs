using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using YamyProject.Localization;

namespace YamyProject
{
    public partial class MasterCompany : Form
    {
        public MasterCompany(bool Case = true)
        {
            InitializeComponent();
            LocalizationManager.LocalizeForm(this);
            headerUC1.FormText = this.Text;
        }
        string filePath;

        private void btnSave_Click(object sender, EventArgs e)
        {
            byte[] ImagebyteArray = null;
            byte[] ImagebyteArrayStamp = null;

            if (imgLogo.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imgLogo.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ImagebyteArray = ms.ToArray();
                }
            }
            if (imgStamp.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imgStamp.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ImagebyteArrayStamp = ms.ToArray();
                }
            }

            if (ValidateComp())
            {
                using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_company"))
                    if (reader.Read())
                    {
                        if (ChB_Default.Checked)
                        {
                            DBClass.ExecuteNonQuery(@"UPDATE yamycompany.tbl_company 
                                                        SET default_company = 0 
                                                        WHERE customer_code IN (SELECT customer_code FROM (SELECT customer_code FROM yamycompany.tbl_company WHERE database_name = @dbName) AS sub)",
                                                        DBClass.CreateParameter("dbName", DBClass.Database)
                                                    );
                        }

                        DBClass.ExecuteNonQuery(@"update  tbl_company set name=@name, address = @address,descriptions=@descriptions,phone1=@phone1,phone2=@phone2,gmail=@gmail,mobile_number=@mobile_number,website=@website,trn_no=@trn_no,country_id=@countryId,logoComp=@logoComp,stampComp=@stampComp;
                                                  update yamycompany.tbl_company set name=@name, address = @address,descriptions=@descriptions,phone1=@phone1,phone2=@phone2,gmail=@gmail,mobile_number=@mobile_number,website=@website,trn_no=@trn_no,logoComp=@logoComp,stampComp=@stampComp,default_company=@default_company where database_name=@dbName",
                                                  DBClass.CreateParameter("dbName", DBClass.Database),
                                                  DBClass.CreateParameter("@address", txtAddress.Text),
                                                DBClass.CreateParameter("@name", txtCompanyName.Text),
                                                DBClass.CreateParameter("@descriptions", txtCompanyDescription.Text),
                                                DBClass.CreateParameter("@phone1", txtCompanyPhone1.Text),
                                                DBClass.CreateParameter("@phone2", txtCompanyPhone2.Text),
                                                DBClass.CreateParameter("@gmail", txtCompanyGmail.Text),
                                                DBClass.CreateParameter("@mobile_number", txtCompanyMobile.Text),
                                                DBClass.CreateParameter("@website", txtCompanyWebsite.Text),
                                                DBClass.CreateParameter("@trn_no", txtCompanyTrn.Text),
                                                DBClass.CreateParameter("@countryId", cmbCountry.SelectedValue.ToString()),
                                                DBClass.CreateParameter("@default_company", ChB_Default.Checked ? "1" : "0"),
                                                DBClass.CreateParameter("@logoComp", ImagebyteArray),
                                                DBClass.CreateParameter("@stampComp", ImagebyteArrayStamp));

                        MessageBox.Show("Successfully Updated");
                    }
                    else
                    {
                        DBClass.ExecuteNonQuery(@"insert into  tbl_company(name, address,descriptions,phone1,phone2,gmail,mobile_number,website,trn_no,country_id,logoComp,default_company) values(@name, @address,@descriptions,@phone1,@phone2,@gmail,@mobile_number,@website,@trn_no,@countryId,@logoComp,@default_company)",
                            DBClass.CreateParameter("@address", txtAddress.Text),
                            DBClass.CreateParameter("@name", txtCompanyName.Text),
                            DBClass.CreateParameter("@descriptions", txtCompanyDescription.Text),
                            DBClass.CreateParameter("@phone1", txtCompanyPhone1.Text),
                            DBClass.CreateParameter("@phone2", txtCompanyPhone2.Text),
                            DBClass.CreateParameter("@gmail", txtCompanyGmail.Text),
                            DBClass.CreateParameter("@mobile_number", txtCompanyMobile.Text),
                            DBClass.CreateParameter("@website", txtCompanyWebsite.Text),
                            DBClass.CreateParameter("@trn_no", txtCompanyTrn.Text),
                            DBClass.CreateParameter("@countryId", cmbCountry.SelectedValue.ToString()),
                            DBClass.CreateParameter("@default_company", ChB_Default.Checked ? "1" : "0"),
                            DBClass.CreateParameter("@logoComp", ImagebyteArray),
                            DBClass.CreateParameter("@stampComp", ImagebyteArrayStamp));

                        MessageBox.Show("Successfully Saved");
                    }
            }
        }
    
        private void MasterCompany_Load(object sender, EventArgs e)
        {
            BindCombos.PopulateCountries(cmbCountry);
            using (MySqlDataReader reader = DBClass.ExecuteReader("select * from tbl_company"))
                if (reader.Read())
                {
                    txtCompanyName.Text = reader["name"].ToString();
                    txtCompanyDescription.Text = reader["descriptions"].ToString();
                    txtCompanyPhone1.Text = reader["phone1"].ToString();
                    txtCompanyPhone2.Text = reader["phone2"].ToString();
                    txtCompanyGmail.Text = reader["gmail"].ToString();
                    txtCompanyMobile.Text = reader["mobile_number"].ToString();
                    txtCompanyWebsite.Text = reader["website"].ToString();
                    txtCompanyTrn.Text = reader["trn_no"].ToString();
                    txtAddress.Text = reader["address"].ToString();

                    object valResult = DBClass.ExecuteScalar(
                        "SELECT default_company FROM yamycompany.tbl_company where database_name=@dbName", DBClass.CreateParameter("dbName", DBClass.Database)
                    );

                    int _default_company = valResult != DBNull.Value ? Convert.ToInt32(valResult) : 0;
                    ChB_Default.Checked = _default_company == 1 ? true : false;
                    if (reader["logoComp"] != DBNull.Value)
                    {
                        // Only attempt to cast if the value is not DBNull
                        byte[] imageBytes = (byte[])reader["logoComp"];
                        try
                        {
                            using (var ms = new MemoryStream(imageBytes))
                            {
                                imgLogo.Image = Image.FromStream(ms);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                            imgLogo.Image = null;
                        }
                    }
                    else
                    {
                        // Handle the case where the value is DBNull, e.g., set a default image or log an error
                        imgLogo.Image = null; // Or you could set a default image here
                    }
                    if (reader["stampComp"] != DBNull.Value)
                    {
                        // Only attempt to cast if the value is not DBNull
                        byte[] imageBytes = (byte[])reader["stampComp"];
                        try
                        {
                            using (var ms = new MemoryStream(imageBytes))
                            {
                                imgStamp.Image = Image.FromStream(ms);
                            }
                        }
                        catch (Exception ex)
                        {
                            ex.Message.ToString();
                            imgStamp.Image = null;
                        }
                    }
                    else
                    {
                        // Handle the case where the value is DBNull, e.g., set a default image or log an error
                        imgStamp.Image = null; // Or you could set a default image here
                    }
                    if (!string.IsNullOrEmpty(reader["country_id"].ToString()))
                        cmbCountry.SelectedValue = reader["country_id"].ToString();
                    else
                        cmbCountry.SelectedValue = 0;

                    LoadVATConfiguration();
                }
        }

        private void LoadVATConfiguration()
        {
            using (MySqlDataReader reader = DBClass.ExecuteReader("SELECT * FROM tbl_vat_configration ORDER BY id DESC LIMIT 1"))
            {
                if (reader.Read())
                {
                    txtRegistrationNo.Text = reader["registration_no"].ToString();
                    dtpTRNIssueDate.Value = Convert.ToDateTime(reader["TRNIssue_date"]);
                    dtpQuarterOneStartDate.Value = Convert.ToDateTime(reader["quarter_one_start_date"]);
                    dtpQuarterOneEndDate.Value = Convert.ToDateTime(reader["quarter_one_end_date"]);
                    dtpQuarterOneDueDate.Value = Convert.ToDateTime(reader["quarter_one_due_date"]);

                    dtpQuarterTwoStartDate.Value = Convert.ToDateTime(reader["quarter_two_start_date"]);
                    dtpQuarterTwoEndDate.Value = Convert.ToDateTime(reader["quarter_two_end_date"]);
                    dtpQuarterTwoDueDate.Value = Convert.ToDateTime(reader["quarter_two_due_date"]);

                    dtpQuarterThreeStartDate.Value = Convert.ToDateTime(reader["quarter_three_start_date"]);
                    dtpQuarterThreeEndDate.Value = Convert.ToDateTime(reader["quarter_three_end_date"]);
                    dtpQuarterThreeDueDate.Value = Convert.ToDateTime(reader["quarter_three_due_date"]);

                    dtpQuarterFourStartDate.Value = Convert.ToDateTime(reader["quarter_four_start_date"]);
                    dtpQuarterFourEndDate.Value = Convert.ToDateTime(reader["quarter_four_end_date"]);
                    dtpQuarterFourDueDate.Value = Convert.ToDateTime(reader["quarter_four_due_date"]);
                }
            }
        }

        private bool ValidateComp()
        {
            if (txtCompanyName.Text.Trim() == "")
            {
                MessageBox.Show("Enter Company Name First.");
                txtCompanyName.Focus();
                return false;
            }
            if (txtCompanyDescription.Text.Trim() == "")
            {
                MessageBox.Show("Enter Address1 First");
                txtCompanyDescription.Focus();
                return false;
            }
            if (txtCompanyPhone1.Text.Trim() == "")
            {
                MessageBox.Show("Enter Phone1 At Least");
                txtCompanyPhone1.Focus();
                return false;
            }
            if (cmbCountry.SelectedValue == null)
            {
                cmbCountry.SelectedIndex = 0;
            }
            return true;
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

        private void btnSaveVat_Click(object sender, EventArgs e)
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
                    DBClass.CreateParameter("@quarter_one_end_date", dtpQuarterOneEndDate.Value.Date),
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
            //ResetForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtpCorporateTaxStartDate_ValueChanged(object sender, EventArgs e)
        {
            dtpCorporateTaxEndDate.Value = dtpCorporateTaxStartDate.Value.Date.AddYears(1).AddDays(-1);
            dtpCorporateTaxDueDate.Value = dtpCorporateTaxEndDate.Value.AddDays(28);
        }

        private void btnBrows_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|* .png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                imgLogo.Image = new Bitmap(filePath);
            }
        }


        private void guna2Button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg, .png)|* .png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                imgStamp.Image = new Bitmap(filePath);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Close();
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

    }
}
